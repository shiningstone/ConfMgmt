using System;
using System.Collections.Generic;
using System.IO;
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
                    var xmlDoc = new XmlDoc();
                    xmlDoc.Load(xmlPath);

                    var node = xmlDoc.ChildNodes[xmlDoc.ChildNodes.Count - 1];
                    var result = XmlConf.ToTree(node, xmlDoc);
                    if (string.IsNullOrEmpty(result.Tag))
                    {
                        result.Tag = Path.GetFileNameWithoutExtension(xmlPath);
                        XmlConf.AddTag(xmlDoc, node, result.Tag);
                    }
                    result.Source = Source.Xml;
                    result.XmlDoc = xmlDoc;
                    _log.Debug(Environment.NewLine + result.ToString());

                    return result;
                }
                catch (Exception ex)
                {
                    _log.Warn($"Failed to BuildConfTree for {xmlPath}", ex);
                    return null;
                }
            }

            public static void Save(ConfTree conf, string path = null)
            {
                try
                {
                    XmlDocument doc = conf.XmlDoc as XmlDocument;
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
                        else if (!string.IsNullOrEmpty((conf.Refer.XmlDoc as XmlDocument).BaseURI))
                        {
                            doc = conf.Refer.XmlDoc.AddSibling(conf.Refer, conf);
                            path = doc.BaseURI.Substring(@"file:///".Length);
                            doc.Save(path);
                        }
                        else
                        {
                            throw new Exception($"Invalid param for Save");
                        }

                        conf.Refer.XmlDoc = conf.XmlDoc = Generate(path).XmlDoc;
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
