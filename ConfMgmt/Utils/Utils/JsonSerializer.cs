using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Utils
{
    public class JsonSerializer
    {
        public static T Deserialize<T>(string content)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                return (T)json.ReadObject(stream);
            }
        }
        public static string Serialize<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(T));
                json.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
