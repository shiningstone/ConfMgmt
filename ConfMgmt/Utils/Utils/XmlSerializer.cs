using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Utils
{
    public class XmlSerializer
    {
        private static Dictionary<string, object> objDict = new Dictionary<string, object>();
        public static void Save<T>(string targetFile, T subject)
        {
            if (objDict.ContainsKey(targetFile) == false)
                objDict[targetFile] = new object();

            lock (objDict[targetFile])
            {
                new FileStream(targetFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None).Close();
                using (var fstream = new FileStream(targetFile, FileMode.Truncate, FileAccess.Write, FileShare.None))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    serializer.Serialize(fstream, subject);
                }
            }
        }
        public static void Load<T>(string targetFile, out T subject)
        {
            if (File.Exists(targetFile))
            {
                using (var fstream = new FileStream(targetFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    subject = (T)serializer.Deserialize(fstream);
                }
            }
            else
            {
                subject = default(T);
            }
        }
        public static void SaveDictionary<T>(string targetFile, T subject)
        {
            if (objDict.ContainsKey(targetFile) == false)
                objDict[targetFile] = new object();

            lock (objDict[targetFile])
            {
                new FileStream(targetFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None).Close();
                using (var fstream = new FileStream(targetFile, FileMode.Truncate, FileAccess.Write, FileShare.None))
                {
                    var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
                    serializer.WriteObject(fstream, subject);
                }
            }
        }
        public static void LoadDictionary<T>(string targetFile, out T subject)
        {
            if (File.Exists(targetFile))
            {
                using (var fstream = new FileStream(targetFile, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
                    subject = (T)serializer.ReadObject(fstream);
                }
            }
            else
            {
                subject = default(T);
            }
        }
    }
}
