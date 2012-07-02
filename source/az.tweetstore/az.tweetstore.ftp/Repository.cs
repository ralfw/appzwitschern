using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using az.contracts;
using az.serialization;
using az.security;
using az.tweetstore.ftp.adapter;
using System.IO;

namespace az.tweetstore.ftp
{
    public class Repository : IRepository
    {
        private readonly FtpClient _ftp;


        private readonly Serialization<Versandauftrag> _serialization = new Serialization<Versandauftrag>();

        public Repository() : this("ftp://ftp.ralfw.domainfactory-kunde.de", TokenRepository.LoadFrom("ftp.credentials.txt"), "/AppZwitschern/TweetStore") {}
        public Repository(string host, Token token, string repoFolderPath)
        {
            _ftp = new FtpClient(host, token);

            if (!repoFolderPath.StartsWith("/")) repoFolderPath = "/" + repoFolderPath;
            _ftp.CreateDirectory(repoFolderPath);
            _ftp.ChangeDirectory(repoFolderPath);
        }


        public void Store(Versandauftrag versandauftrag, Action onEndOfStream)
        {
            if (versandauftrag == null) { onEndOfStream(); return; }

            var data = _serialization.Serialize(versandauftrag);

            var filename = Build_filename(versandauftrag);
            File.WriteAllText(filename, data);
            try
            {
                _ftp.UploadFiles(filename);
            }
            finally
            {
                File.Delete(filename);
            }
        }


        public void List(Action<string> onListed)
        {
            foreach (var dirEntry in _ftp.ListDirectory())
                onListed(dirEntry.Name);
            onListed(null);
        }


        public void Load(string persistentId, Action<Versandauftrag> onLoaded)
        {
            if (persistentId == null) { onLoaded(null); return; }

            _ftp.DownloadFiles("", persistentId);
            var data = File.ReadAllText(persistentId);
            File.Delete(persistentId);
            var versandauftrag = _serialization.Deserialize(data);
            onLoaded(versandauftrag);
        }


        public void Delete(Versandauftrag versandauftrag, Action onEndOfStream)
        {
            if (versandauftrag != null)
                _ftp.DeleteFiles(Build_filename(versandauftrag));
            else
                onEndOfStream();
        }


        private string Build_filename(Versandauftrag versandauftrag)
        {
            var datum = versandauftrag.Termin.ToString("s").Replace("-", "").Replace(":", "");
            return string.Format("{0:s}-{1}.tweet", datum, versandauftrag.Id);
        }
    }
}
