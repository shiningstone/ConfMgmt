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
            public static ConfTree Generate(string xmlPath)
            {
                _log.Info($"Generate({xmlPath})");

                try
                {
                    var xmlFile = new XmlDocument();
                    xmlFile.Load(xmlPath);

                    var node = xmlFile.ChildNodes[xmlFile.ChildNodes.Count - 1];
                    var result = XmlConf.ToTree(node);
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

            public static void Add(XmlDocument xmlDoc, XmlNode parentNode, ConfTree tree)
            {

            }
            private static XmlDocument AddSibling(ConfTree refer, ConfTree tree)
            {
                XmlDocument doc = refer.XmlFile;

                XmlNode sibling = XmlOp.Find(doc, refer.Name);
                if (sibling == doc.ChildNodes[doc.ChildNodes.Count - 1])//非唯一
                {
                    doc.RemoveChild(sibling);

                    var parent = new ConfTree($"{refer.Name}s");
                    parent.Add(refer);
                    parent.Add(tree);
                    doc.AppendChild(XmlConf.CreateNode(doc, parent));
                }
                else
                {
                    sibling.ParentNode.AppendChild(XmlConf.CreateNode(doc, tree));
                }

                return doc;
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
                            doc = XmlOp.CreateDoc();
                            doc.AppendChild(XmlConf.CreateNode(doc, conf));
                            doc.Save(path);
                        }
                        else if (!string.IsNullOrEmpty((conf.Refer.XmlFile as XmlDocument).BaseURI))
                        {
                            doc = AddSibling(conf.Refer, conf);
                            path = doc.BaseURI.Substring(@"file:///".Length);
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
