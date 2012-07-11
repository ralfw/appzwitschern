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
                File.WriteAllText(Build_filename(versandauftrag), data);
            }
            else
                onEndOfStream();
        }


        public void List(Action<string> onListed)
        {
            foreach (var filename in Directory.GetFiles(_repoFolderPath))
                onListed(filename);
            onListed(null);
        }


        public void Load(string persistentId, Action<Versandauftrag> onLoaded)
        {
            if (persistentId == null) { onLoaded(null); return; }

            var data = File.ReadAllText(persistentId);
            var versandauftrag = _serialization.Deserialize(data);
            onLoaded(versandauftrag);
        }


        public void Delete(Versandauftrag versandauftrag, Action onEndOfStream)
        {
            if (versandauftrag != null)
                File.Delete(Build_filename(versandauftrag));
            else
                onEndOfStream();
        }

        
        private string Build_filename(Versandauftrag versandauftrag)
        {
            return _repoFolderPath + @"\" + versandauftrag.Id + ".tweet";
        }


        public void Dispose() {}
    }
}
