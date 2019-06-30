using System.Collections.Generic;
using System.Xml;

namespace JbConf
{
    public class DictionaryBuilder
    {
        public static ConfTree Generate(Dictionary<string, string> kvs, string name)
        {
            ConfTree result = new ConfTree(name);
            result.Path = "";
            result.Source = Source.Dictionary;
            result.Tag = "Default";

            foreach (var kv in kvs)
            {
                result.Add(new ConfItem(kv.Key, kv.Value));
            }

            ConfMgmt.Add(result);

            return result;
        }

        private static void AddTag(XmlDocument xmlDoc, XmlNode node, string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                XmlAttribute attr = xmlDoc.CreateAttribute("Tag");
                attr.Value = tag;
                node.Attributes.Append(attr);
            }
        }
        private static XmlNode CreateNode(XmlDocument xmlDoc, XmlNode parentNode, ConfItem conf)
        {
            XmlNode node = xmlDoc.CreateElement(conf.Name, null);
            AddTag(xmlDoc, node, conf.Tag);
            parentNode.AppendChild(node);
            return node;
        }
        private static XmlNode CreateNode(XmlDocument xmlDoc, XmlNode parentNode, ConfTree conf)
        {
            var current = CreateNode(xmlDoc, parentNode, conf as ConfItem);
            foreach (var son in conf.Sons)
            {
                if (son is ConfTree)
                {
                    CreateNode(xmlDoc, current, son as ConfTree);
                }
                else
                {
                    var newNode = CreateNode(xmlDoc, current, son);
                    newNode.InnerText = son.Value;
                }
            }
            return current;
        }
        public static void Save(ConfTree conf, string path = null)
        {
            /* xml file header */
            conf.XmlFile = new XmlDocument();
            XmlDocument xmlDoc = conf.XmlFile as XmlDocument;
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);

            CreateNode(xmlDoc, xmlDoc, conf);

            xmlDoc.Save(path);
        }
    }
}
