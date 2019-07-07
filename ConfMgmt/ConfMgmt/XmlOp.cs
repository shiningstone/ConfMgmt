using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JbConf
{
    public class XmlOp
    {
        public static XmlDocument CreateDoc()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(node);
            return xmlDoc;
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
        public static XmlNode[] Find(XmlDocument doc, string[] item, string tag)
        {
            XmlNode[] nodes = new XmlNode[item.Length];

            XmlNode temp = doc;
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
        public static XmlNode Find(XmlDocument xmlFile, string path, string tag)
        {
            if (string.IsNullOrEmpty(path))
            {
                return xmlFile;
            }
            else
            {
                path = path.Substring(0, 1) == @"/" ? path.Substring(1) : path;
                var nodes = Find(xmlFile, path.Split('/'), tag);
                return nodes[nodes.Length - 1];
            }
        }

        public static void Modify(XmlNode node, string key, string value)
        {
            Find(node, key).InnerText = value;
        }
        public static void ModifyAttribute(XmlNode node, string attr, string value)
        {
            (node as XmlElement).GetAttributeNode(attr).Value = value;
        }
    }
}
