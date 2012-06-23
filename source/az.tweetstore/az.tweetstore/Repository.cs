using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using az.contracts;
using az.serialization;

namespace az.tweetstore
{
    public class Repository : IRepository
    {
        private readonly string _repoFolderPath;

        private readonly Serialization<Versandauftrag> _serialization = new Serialization<Versandauftrag>();

        public Repository() : this("TweetStore") {}
        public Repository(string repoFolderPath)
        {
            _repoFolderPath = repoFolderPath;
            Directory.CreateDirectory(_repoFolderPath);
        }


        public void Store(Versandauftrag versandauftrag, Action onEndOfStream)
        {
            if (versandauftrag != null)
            {
                var data = _serialization.Serialize(versandauftrag);
                File.WriteAllText(Build_filename(versandauftrag.Id), data);
            }
            else
                onEndOfStream();
        }


        public void Load(Action<Versandauftrag> onLoaded)
        {
            foreach(var filename in Directory.GetFiles(_repoFolderPath))
            {
                var data = File.ReadAllText(Build_filename(Path.GetFileNameWithoutExtension(filename)));
                var versandauftrag = _serialization.Deserialize(data);
                onLoaded(versandauftrag);
            }
            onLoaded(null);
        }


        public void Delete(string versandauftragId, Action onEndOfStream)
        {
            if (versandauftragId != null)
                File.Delete(Build_filename(versandauftragId));
            else
                onEndOfStream();
        }

        
        private string Build_filename(string id)
        {
            return _repoFolderPath + @"\" + id + ".tweet";
        }
    }
}
