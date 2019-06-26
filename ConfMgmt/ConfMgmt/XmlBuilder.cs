using System;
using System.Xml;
using Utils;

namespace JbConf
{
    public class XmlBuilder
    {
        private static Logger _log = new Logger("XmlBuilder");

        public static ConfTree Generate(string xmlPath)
        {
            var xmlFile = new XmlDocument();
            xmlFile.Load(xmlPath);

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
            try
            {
                var doc = conf.XmlFile as XmlDocument;
                path = path == null ? doc.BaseURI.Substring(@"file:///".Length) : path;
                doc.Save(path);
            }
            catch (Exception ex)
            {
                _log.Error($"Save({conf.Name}, {path}) failed", ex);
            }
        }

        private static ConfTree GenerateTree(XmlNode node)
        {
            var result = new ConfTree(node.Name);
            var tag = (node as XmlElement).Attributes.GetNamedItem("tag");
            if (tag != null)
            {
                result.Tag = tag.Value;
            }

            foreach (XmlNode n in node.ChildNodes)
            {
                if (IsLeaf(n))
                {
                    result.Add(GenerateLeaf(n));
                }
                else
                {
                    if (n.ChildNodes.Count > 1)
                    {
                        result.Add(GenerateTree(n));
                    }
                    else if (n.ChildNodes.Count == 1 && IsLeaf(n.ChildNodes[0]))
                    {
                        result.Add(GenerateTree(n));
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
    }
}
