using System.Collections.Generic;
using System.Xml;

namespace ConfMgmt
{
    public class XmlBuilder
    {
        public static ConfTree Generate(string path)
        {
            var xmlFile = new XmlDocument();
            xmlFile.Load(path);

            var node = xmlFile.ChildNodes[xmlFile.ChildNodes.Count - 1];
            var result = GenerateTree(node);
            result.XmlFile = xmlFile;

            return result;
        }
        public static ConfTree GenerateTree(XmlNode node)
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
        public static bool IsLeaf(XmlNode node)
        {
            return (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text);
        }
        public static ConfItem GenerateLeaf(XmlNode node)
        {
            return new ConfItem(node.Name, node.FirstChild.InnerText);
        }
        public static XmlNode Find(XmlNode node, string key)
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
        public static void Modify(object xmlFile, string key, string value)
        {
            Find(xmlFile as XmlNode, key).InnerText = value;
        }
        public static void Save(object xmlFile, string path)
        {
            var doc = xmlFile as XmlDocument;
            path = path == null ? doc.BaseURI.Substring(@"file:///".Length) : path;
            doc.Save(path);
        }
    }
}
