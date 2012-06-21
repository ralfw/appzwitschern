using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using az.contracts;
using az.serialization;

namespace az.tweetstore
{
    public class Repository
    {
        private readonly string _repoFolderPath;

        private readonly Serialization<Versandauftrag> _serialization = new Serialization<Versandauftrag>();

        public Repository() : this("TweetStore") {}
        public Repository(string repoFolderPath)
        {
            _repoFolderPath = repoFolderPath;
            Directory.CreateDirectory(_repoFolderPath);
        }


        public void Store(Versandauftrag versandauftrag)
        {
            var data = _serialization.Serialize(versandauftrag);
            File.WriteAllText(Build_filename(versandauftrag.Id), data);
        }


        public void Load(Action<Versandauftrag> onLoaded, Action onEndOfLoad)
        {
            foreach(var filename in Directory.GetFiles(_repoFolderPath))
            {
                var data = File.ReadAllText(Build_filename(Path.GetFileNameWithoutExtension(filename)));
                var versandauftrag = _serialization.Deserialize(data);
                onLoaded(versandauftrag);
            }
            onEndOfLoad();
        }


        public void Delete(string versandauftragId)
        {
            File.Delete(Build_filename(versandauftragId));
        }

        
        private string Build_filename(string id)
        {
            return _repoFolderPath + @"\" + id + ".tweet";
        }
    }
}
