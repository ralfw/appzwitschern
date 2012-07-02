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
    //public class Repository : IRepository
    //{
    //    private readonly FtpClient _ftp;
        

    //    private readonly Serialization<Versandauftrag> _serialization = new Serialization<Versandauftrag>();

    //    public Repository(string host, Token token, string repoFolderPath)
    //    {
    //        _ftp = new FtpClient(host, token);

    //        if (!repoFolderPath.StartsWith("/")) repoFolderPath = "/" + repoFolderPath;
    //        _ftp.CreateDirectory(repoFolderPath);
    //        _ftp.ChangeDirectory(repoFolderPath);
    //    }


    //    public void Store(Versandauftrag versandauftrag, Action onEndOfStream)
    //    {
    //        if (versandauftrag != null)
    //        {
    //            var data = _serialization.Serialize(versandauftrag);

    //            var filename = Build_filename(versandauftrag);
    //            File.WriteAllText(filename, data);
    //            try
    //            {
    //                _ftp.UploadFiles(filename);
    //            }
    //            finally
    //            {
    //                File.Delete(filename);
    //            }
    //        }
    //        else
    //            onEndOfStream();
    //    }


    //    public void Load(Action<Versandauftrag> onLoaded)
    //    {
    //        foreach (var filename in Directory.GetFiles(_repoFolderPath))
    //        {
    //            var data = File.ReadAllText(Build_filename(Path.GetFileNameWithoutExtension(filename)));
    //            var versandauftrag = _serialization.Deserialize(data);
    //            onLoaded(versandauftrag);
    //        }
    //        onLoaded(null);
    //    }


    //    public void Delete(string filename, Action onEndOfStream)
    //    {
    //        if (filename != null)
    //            _ftp.DeleteFiles(filename);
    //        else
    //            onEndOfStream();
    //    }


    //    private string Build_filename(Versandauftrag versandauftrag)
    //    {
    //        return string.Format("{0:s}#{1}.tweet", versandauftrag.Termin, versandauftrag.Id);
    //    }
    //}
}
