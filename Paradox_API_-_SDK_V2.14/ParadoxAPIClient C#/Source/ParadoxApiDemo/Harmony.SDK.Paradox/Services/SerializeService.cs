using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Harmony.SDK.Paradox.Services
{
    public class SerializeService
    {
        public static string Serialize<T>(T obj, bool xml = true) where T : new()
        {
            return (xml ? SerializeXml(obj) : SerializeJson(obj));
        }

        public static T DeSerialize<T>(string str, bool xml = true) where T : new()
        {
            if (xml)
                return DeSerializeXml<T>(str);
            return DeSerializeJson<T>(str);
        }
        
        protected static string SerializeXml<T>(T obj) where T : new()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj, null);
                return writer.ToString();
            }
        }
        
        protected static T DeSerializeXml<T>(string str) where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(str))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        
        protected static string SerializeJson<T>(T obj) where T : new()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            };
            return JsonConvert.SerializeObject(obj, settings);
        }
        
        protected static T DeSerializeJson<T>(string str) where T : new()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.All
            };
            return JsonConvert.DeserializeObject<T>(str, settings);
        }
    }
}