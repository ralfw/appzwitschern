using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AlexPilotti.FTPS.Client;
using az.contracts;
using az.serialization;
using az.security;
using System.IO;

namespace az.tweetstore.ftp
{
    public class Repository : IRepository
    {
        // FTP Library: http://ftps.codeplex.com/releases/view/72012
        private readonly FTPSClient _ftp;


        private readonly Serialization<Versandauftrag> _serialization = new Serialization<Versandauftrag>();

        public Repository() : this("ftp.ralfw.domainfactory-kunde.de", TokenRepository.LoadFrom("ftp.credentials.txt"), "AppZwitschern_TweetStore") {}
        public Repository(string host, Token token, string repoFolderPath)
        {
            _ftp = new FTPSClient();
            _ftp.Connect(host, 
                         new NetworkCredential(token.Key, token.Secret),
                         ESSLSupportMode.ClearText);

            try
            {
                _ftp.MakeDir(repoFolderPath);
            }
            catch {}
            _ftp.SetCurrentDirectory(repoFolderPath);
        }


        public void Store(Versandauftrag versandauftrag, Action onEndOfStream)
        {
            if (versandauftrag == null) { onEndOfStream(); return; }

            var data = _serialization.Serialize(versandauftrag);

            var filename = Build_filename(versandauftrag);
            File.WriteAllText(filename, data);
            try
            {
                _ftp.PutFile(filename, filename);
            }
            finally
            {
                File.Delete(filename);
            }
        }


        public void List(Action<string> onListed)
        {
            foreach (var dirEntry in _ftp.GetDirectoryList().Where(dli => !dli.IsDirectory))
                onListed(dirEntry.Name);
            onListed(null);
        }


        public void Load(string persistentId, Action<Versandauftrag> onLoaded)
        {
            if (persistentId == null) { onLoaded(null); return; }

            _ftp.GetFile(persistentId, persistentId);

            var data = File.ReadAllText(persistentId);
            File.Delete(persistentId);
            var versandauftrag = _serialization.Deserialize(data);
            onLoaded(versandauftrag);
        }


        public void Delete(Versandauftrag versandauftrag, Action onEndOfStream)
        {
            if (versandauftrag != null)
                _ftp.DeleteFile(Build_filename(versandauftrag));
            else
                onEndOfStream();
        }


        private string Build_filename(Versandauftrag versandauftrag)
        {
            var datum = versandauftrag.Termin.ToString("s").Replace("-", "").Replace(":", "");
            return string.Format("{0:s}-{1}.tweet", datum, versandauftrag.Id);
        }


        public void Dispose()
        {
            _ftp.Dispose();
        }
    }
}
