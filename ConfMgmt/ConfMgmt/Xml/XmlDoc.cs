﻿using System.IO;
using System.Xml;

namespace JbConf
{
    public class XmlDoc : XmlDocument
    {
        private bool IsMatch(XmlNode node, string[] name_tag)
        {
            var ele = node as XmlElement;
            if (ele != null)
            {
                if (node.Name == name_tag[0])
                {
                    if (name_tag.Length == 1)
                    {
                        var eleTag = ele.GetAttribute("tag");
                        if (string.IsNullOrEmpty(eleTag) || eleTag == node.Name || eleTag == "Default")
                        {
                            return true;
                        }
                        else if (ele.ParentNode is XmlDocument && eleTag == Path.GetFileNameWithoutExtension(BaseURI))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (ele.GetAttribute("tag") == name_tag[1])
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public XmlNode[] Find(string[] item)
        {
            XmlNode[] nodes = new XmlNode[item.Length];
            int i = 0;
            string[] name_tag;

            XmlNode temp = this;
            for (; i < nodes.Length - 1; i++)
            {
                name_tag = item[i].Split('(');
                if (name_tag.Length > 1)
                {
                    name_tag[1] = name_tag[1].Substring(0, name_tag[1].Length - 1);
                }

                foreach (XmlNode son in temp.ChildNodes)
                {
                    if (IsMatch(son, name_tag))
                    {
                        nodes[i] = son;
                        temp = son;
                        break;
                    }
                }
            }

            name_tag = item[i].Split('(');
            if (name_tag.Length > 1)
            {
                name_tag[1] = name_tag[1].Substring(0, name_tag[1].Length - 1);
            }
            foreach (XmlNode node in temp.ChildNodes)
            {
                if (IsMatch(node, name_tag))
                {
                    nodes[i] = node;
                }
            }

            return nodes;
        }

        public void Modify(string path, string value, string attr = null)
        {
            path = path.Substring(0, 1) == @"/" ? path.Substring(1) : path;

            var nodes = Find(path.Split('/'));
            var node = nodes[nodes.Length - 1];

            if (attr == null)
            {
                node.InnerText = value;
            }
            else
            {
                (node as XmlElement).GetAttributeNode(attr).Value = value;
            }
        }

        //添加同名节点
        public XmlDoc AddSibling(ConfTree refer, ConfTree tree)
        {
            XmlNode sibling = XmlOp.Find(this, tree.Name);
            if (sibling == ChildNodes[ChildNodes.Count - 1])//非唯一
            {
                RemoveChild(sibling);

                var parent = new ConfTree($"{tree.Name}s");
                parent.Add(refer);
                parent.Add(tree);
                AppendChild(XmlConf.CreateNode(this, parent));
            }
            else
            {
                sibling.ParentNode.AppendChild(XmlConf.CreateNode(this, tree));
            }

            return this;
        }

        //添加节点
        public void AddNode(string parentPath, ConfItem item)
        {
            parentPath = parentPath.Substring(0, 1) == @"/" ? parentPath.Substring(1) : parentPath;
            var nodes = Find(parentPath.Split('/'));
            nodes[nodes.Length - 1].AppendChild(XmlConf.CreateNode(this, item));
        }
        //删除节点
        public void RemoveNode(string path)
        {
            path = path.Substring(0, 1) == @"/" ? path.Substring(1) : path;
            var nodes = Find(path.Split('/'));
            nodes[nodes.Length - 2].RemoveChild(nodes[nodes.Length - 1]);
        }
    }
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
    }
}
