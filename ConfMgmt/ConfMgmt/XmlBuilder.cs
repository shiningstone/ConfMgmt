using System;
using System.Collections.Generic;
using System.Xml;
using Utils;

namespace JbConf
{
    public class Builder
    {
        private static Logger _log = new Logger("Builder.Xml");

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

        public class Xml
        {
            #region Deserialize
            private static bool IsItem(XmlNode node)
            {
                return (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text);
            }
            private static ConfItem ToItem(XmlNode node)
            {
                return new ConfItem(node.Name, node.FirstChild.InnerText);
            }
            private static ConfTree ToTree(XmlNode node)
            {
                var result = new ConfTree(node.Name);
                var tag = (node as XmlElement).Attributes.GetNamedItem("tag");
                if (tag != null)
                {
                    result.Tag = tag.Value;
                }

                foreach (XmlNode n in node.ChildNodes)
                {
                    if (IsItem(n))
                    {
                        result.Add(ToItem(n));
                    }
                    else
                    {
                        if (n.ChildNodes.Count > 1)
                        {
                            result.Add(ToTree(n));
                        }
                        else if (n.ChildNodes.Count == 1 && IsItem(n.ChildNodes[0]))
                        {
                            result.Add(ToTree(n));
                        }
                    }
                }

                return result;
            }
            public static ConfTree Generate(string xmlPath)
            {
                _log.Info($"Generate({xmlPath})");

                try
                {
                    var xmlFile = new XmlDocument();
                    xmlFile.Load(xmlPath);

                    var node = xmlFile.ChildNodes[xmlFile.ChildNodes.Count - 1];
                    var result = ToTree(node);
                    result.Source = Source.Xml;
                    result.XmlFile = xmlFile;
                    _log.Debug(Environment.NewLine + result.ToString());

                    ConfMgmt.Add(result);
                    return result;
                }
                catch (Exception ex)
                {
                    _log.Warn($"Failed to BuildConfTree for {xmlPath}", ex);
                    return null;
                }
            }
            #endregion
            #region Serialize
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
            public static XmlNode CreateNode(XmlDocument xmlDoc, XmlNode parentNode, ConfTree conf)
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
            public static XmlDocument GenerateXmlDoc(ConfTree conf)
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                xmlDoc.AppendChild(node);

                CreateNode(xmlDoc, xmlDoc, conf);

                return xmlDoc;
            }
            #endregion

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
            public static void Modify(object xmlFile, string key, string value)
            {
                Find(xmlFile as XmlNode, key).InnerText = value;
            }
            public static void Add(XmlDocument xmlDoc, XmlNode parentNode, ConfTree tree)
            {

            }

            public static void Save(ConfTree conf, string path = null)
            {
                try
                {
                    XmlDocument doc = conf.XmlFile as XmlDocument;
                    if (doc != null)
                    {
                        path = path == null ? doc.BaseURI.Substring(@"file:///".Length) : path;
                        doc.Save(path);
                    }
                    else
                    {
                        doc = GenerateXmlDoc(conf);
                        doc.Save(path);

                        conf.XmlFile = Generate(path).XmlFile;
                    }
                }
                catch (Exception ex)
                {
                    _log.Error($"Save({conf.Name}, {path}) failed", ex);
                }
            }
        }
    }
}
