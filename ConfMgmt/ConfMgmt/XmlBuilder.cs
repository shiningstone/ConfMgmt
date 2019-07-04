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
                    XmlAttribute attr = xmlDoc.CreateAttribute("tag");
                    attr.Value = tag;
                    node.Attributes.Append(attr);
                }
            }
            private static XmlNode CreateNode(XmlDocument xmlDoc, ConfItem conf)
            {
                XmlNode current = xmlDoc.CreateElement(conf.Name, null);
                AddTag(xmlDoc, current, conf.Tag);

                if (conf is ConfTree)
                {
                    foreach (var son in (conf as ConfTree).Sons)
                    {
                        if (son is ConfTree)
                        {
                            current.AppendChild(CreateNode(xmlDoc, son));
                        }
                        else
                        {
                            var temp = CreateNode(xmlDoc, son);
                            temp.InnerText = son.Value;
                            current.AppendChild(temp);
                        }
                    }
                }
                else
                {
                    current.InnerText = conf.Value;
                }

                return current;
            }
            public static XmlDocument GenerateXmlDoc(ConfTree conf)
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                xmlDoc.AppendChild(node);

                xmlDoc.AppendChild(CreateNode(xmlDoc, conf));

                return xmlDoc;
            }
            #endregion
            public static XmlNode Find(XmlDocument xmlFile, string path, string tag)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return xmlFile;
                }
                else
                {
                    var nodes = FindItem(xmlFile, path.Substring(1).Split('/'), tag);
                    return nodes[nodes.Length - 1];
                }

            }
            private static XmlNode[] FindItem(XmlDocument xmlFile, string[] item, string tag)
            {
                XmlNode[] nodes = new XmlNode[item.Length];

                XmlNode temp = xmlFile;
                for (int i = 0; i < nodes.Length - 1; i++)
                {
                    nodes[i] = temp.SelectSingleNode(item[i]);
                    if (nodes[i] != null)
                    {
                        temp = nodes[i];
                    }
                }

                var name = item[nodes.Length - 1];
                foreach (XmlNode node in temp.ChildNodes)
                {
                    var element = node as XmlElement;
                    if (element != null)
                    {
                        if (element.Name == name)
                        {
                            if (string.IsNullOrEmpty(tag) || element.GetAttribute("tag") == tag)
                            {
                                nodes[nodes.Length - 1] = node;
                                break;
                            }
                        }
                    }
                }

                return nodes;
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
                        if (path != null)
                        {
                            doc = GenerateXmlDoc(conf);
                            doc.Save(path);
                        }
                        else if (!string.IsNullOrEmpty((conf.Refer.XmlFile as XmlDocument).BaseURI))
                        {
                            doc = conf.Refer.XmlFile;
                            path = doc.BaseURI.Substring(@"file:///".Length);

                            XmlNode sibling = Find(doc as XmlNode, conf.Refer.Name);
                            if (sibling == doc.ChildNodes[doc.ChildNodes.Count - 1])
                            {
                                doc.RemoveChild(sibling);

                                var parent = new ConfTree($"{conf.Refer.Name}s");
                                parent.Add(conf.Refer);
                                parent.Add(conf);
                                doc.AppendChild(CreateNode(doc, parent));
                            }
                            else
                            {
                                sibling.ParentNode.AppendChild(CreateNode(doc, conf));
                            }

                            doc.Save(path);
                        }
                        else
                        {
                            throw new Exception($"Invalid param for Save");
                        }

                        conf.Refer.XmlFile = conf.XmlFile = Generate(path).XmlFile;
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
