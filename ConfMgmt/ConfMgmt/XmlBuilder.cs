using System.Collections.Generic;
using System.Xml;

namespace JbConf
{
    public class XmlBuilder
    {
        public static ConfTree Generate(string path)
        {
            var xmlFile = new XmlDocument();
            xmlFile.Load(path);

            var node = xmlFile.ChildNodes[xmlFile.ChildNodes.Count - 1];
            var result = GenerateTree(node);
            result.Source = Source.Xml;
            result.XmlFile = xmlFile;

            ConfMgmt.Add(result);
            return result;
        }
        public static void Modify(object xmlFile, string key, string value)
        {
            Find(xmlFile as XmlNode, key).InnerText = value;
        }
        public static void Save(ConfTree conf, string path)
        {
            var doc = conf.XmlFile as XmlDocument;
            if (doc != null)
            {
                path = path == null ? doc.BaseURI.Substring(@"file:///".Length) : path;
                doc.Save(path);
            }
            else
            {
                SaveDictionaryConf(conf, path);
            }
        }

        private static ConfTree GenerateTree(XmlNode node)
        {
            var result = new ConfTree(node.Name);

            result.Sons = new List<ConfItem>();

            foreach (XmlNode n in node.ChildNodes)
            {
                if (IsLeaf(n))
                {
                    result.Sons.Add(GenerateLeaf(n));
                }
                else
                {
                    if (n.ChildNodes.Count > 1)
                    {
                        result.Sons.Add(GenerateTree(n));
                    }
                }
            }

            return result;
        }
        private static bool IsLeaf(XmlNode node)
        {
            return (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text);
        }
        private static ConfItem GenerateLeaf(XmlNode node)
        {
            return new ConfItem(node.Name, node.FirstChild.InnerText);
        }
        private static XmlNode Find(XmlNode node, string key)
        {
            XmlNode result = null;

            XmlNodeList subNodes = node.ChildNodes;
            foreach (XmlNode n in subNodes)
            {
                if (n.Name == key)
                {
                    result = n;
                    break;
                }
                else if (n.HasChildNodes)
                {
                    result = Find(n, key);
                    if (result != null)
                    {
                        break;
                    }
                }
            }

            return result;
        }
        private static void CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value)
        {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            node.InnerText = value;
            parentNode.AppendChild(node);
        }
        private static void SaveDictionaryConf(ConfTree conf, string path)
        {
            conf.XmlFile = new XmlDocument();
            XmlDocument xmlDoc = conf.XmlFile as XmlDocument;

            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);

            XmlNode root = xmlDoc.CreateElement(conf.Name);
            xmlDoc.AppendChild(root);

            foreach (ConfItem son in conf.Sons)
            {
                CreateNode(xmlDoc, root, son.Name, son.Value);
            }

            xmlDoc.Save(path);
        }
    }
}
