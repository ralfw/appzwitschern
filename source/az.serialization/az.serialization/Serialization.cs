using System.IO;
using Newtonsoft.Json;

namespace az.serialization
{
    public class Serialization<T>
    {
        public string Serialize(T t) {
            var stringWriter = new StringWriter();
            var serializer = new JsonSerializer();
            serializer.Serialize(stringWriter, t);
            return stringWriter.ToString();
        }

        public T Deserialize(string serialized) {
            if (serialized == null) {
                return default(T);
            }
            var serializer = new JsonSerializer();
            var stringReader = new StringReader(serialized);
            return (T)serializer.Deserialize(stringReader, typeof(T));
        }
    }
}