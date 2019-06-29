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
        private static XmlNode CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value, string tag = null)
        {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            AddTag(xmlDoc, node, tag);
            node.InnerText = value;
            parentNode.AppendChild(node);
            return node;
        }
        private static XmlNode CreateTree(XmlDocument xmlDoc, XmlNode parentNode, ConfTree tree)
        {
            var treeNode = CreateNode(xmlDoc, parentNode, tree.Name, tree.Value, tree.Tag);
            foreach (var son in tree.Sons)
            {
                if (son is ConfTree)
                {
                    throw new System.Exception($"Doesn't support tree depth larger than 3");
                }
                else
                {
                    treeNode.AppendChild(
                        CreateNode(xmlDoc, parentNode, son.Name, son.Value, son.Tag)
                    );
                }
            }
            return treeNode;
        }
        public static void Save(ConfTree conf, string path = null)
        {
            conf.XmlFile = new XmlDocument();
            XmlDocument xmlDoc = conf.XmlFile as XmlDocument;

            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);

            XmlNode root = xmlDoc.CreateElement(conf.Name);
            AddTag(xmlDoc, root, conf.Tag);
            xmlDoc.AppendChild(root);

            foreach (var son in conf.Sons)
            {
                if (son is ConfTree)
                {
                    CreateTree(xmlDoc, root, son as ConfTree);
                }
                else
                {
                    CreateNode(xmlDoc, root, son.Name, son.Value, son.Tag);
                }
            }

            xmlDoc.Save(path);
        }
    }
}
