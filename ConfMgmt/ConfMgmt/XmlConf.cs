﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JbConf
{
    public class XmlConf
    {
        #region deserialize
        public static bool IsItem(XmlNode node)
        {
            var ele = node as XmlElement;
            if (ele != null && (ele.ChildNodes.Count == 0 || node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text) )
            {
                return true;
            }

            return false;
        }
        public static ConfItem ToItem(XmlNode node)
        {
            if (node.ChildNodes.Count == 0)
            {
                var item = new ConfItem(node.Name);

                foreach (XmlAttribute attr in (node as XmlElement).Attributes)
                {
                    item.Attributes[attr.Name] = attr.Value;
                }

                return item;
            }
            else
            {
                return new ConfItem(node.Name, node.FirstChild.InnerText);
            }
        }
        public static ConfTree ToTree(XmlNode node)
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
        #endregion
        #region Serialize
        public static void AddTag(XmlDocument xmlDoc, XmlNode node, string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                XmlAttribute attr = xmlDoc.CreateAttribute("tag");
                attr.Value = tag;
                node.Attributes.Append(attr);
            }
        }
        public static XmlNode CreateNode(XmlDocument xmlDoc, ConfItem conf)
        {
            var current = xmlDoc.CreateElement(conf.Name, null);
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
                if (!string.IsNullOrEmpty(conf.Value))
                {
                    current.InnerText = conf.Value;
                }

                foreach (var attr in conf.Attributes)
                {
                    if (attr.Key != "tag")
                    {
                        var ele = xmlDoc.CreateAttribute(attr.Key);
                        ele.Value = attr.Value;
                        current.Attributes.Append(ele);
                    }
                }
            }

            return current;
        }
        #endregion
    }
}
