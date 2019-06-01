using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ConfMgmt
{
    public class ConfItem
    {
        public string Name;
        public string Value;

        public ConfItem(string name, string value = null)
        {
            Name = name;
            Value = value;
        }
        public ConfItem(XmlNode node)
        {
            Name = node.Name;
            Value = node.FirstChild.InnerText;
        }
        public bool IsNode()
        {
            return Value == null;
        }
        public override string ToString()
        {
            return $"{Name}:{(Value != null ? Value : Environment.NewLine)}{(Value == null ? "" : Environment.NewLine)}";
        }
    }
    public class ConfTree : ConfItem
    {
        public List<ConfItem> Sons;

        public static bool IsConfItem(XmlNode node)
        {
            XmlElement ele = node as XmlElement;
            return (ele != null) && (!string.IsNullOrEmpty(ele.Name));
        }
        public static bool IsLeaf(XmlNode node)
        {
            return (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text);
        }

        private int _depth = 0;
        private XmlDocument _xmlFile;

        public ConfTree(string xmlPath) : base(Path.GetFileNameWithoutExtension(xmlPath), null)
        {
            _xmlFile = new XmlDocument();
            _xmlFile.Load(xmlPath);

            var node = _xmlFile.ChildNodes[_xmlFile.ChildNodes.Count - 1];

            Sons = new List<ConfItem>();

            foreach (XmlNode n in node.ChildNodes)
            {
                if (IsLeaf(n))
                {
                    Sons.Add(new ConfItem(n));
                }
                else
                {
                    if (n.ChildNodes.Count > 1)
                    {
                        Sons.Add(new ConfTree(n));
                    }
                }
            }
        }
        public ConfTree(XmlNode node) : base(node.Name, null)
        {
            Sons = new List<ConfItem>();

            foreach (XmlNode n in node.ChildNodes)
            {
                if (IsLeaf(n))
                {
                    Sons.Add(new ConfItem(n));
                }
                else
                {
                    if (n.ChildNodes.Count > 1)
                    {
                        Sons.Add(new ConfTree(n));
                    }
                }
            }
        }
        public void Visit(Action<ConfItem, int> executor, ConfItem item = null)
        {
            if (item == null)
            {
                _depth = 0;
                item = this;
            }

            var tree = item as ConfTree;
            if(tree != null)
            {
                executor(item, _depth);

                foreach (var c in tree.Sons)
                {
                    _depth++;
                    Visit(executor, c);
                    _depth--;
                }
            }
            else
            {
                executor(item, _depth);
            }
        }
        public ConfItem Find(string name)
        {
            ConfItem result = null;

            if (!name.Contains(@"\"))
            {
                Visit((item, level) =>
                {
                    if (item.Name == name)
                    {
                        result = item;
                    }
                });
            }
            else
            {
                string[] strs = name.Split('\\');

                var item = Find(strs[0]);
                var tree = item as ConfTree;
                if(tree != null)
                {
                    item = tree.Find(strs[1]);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }
            return result;
        }
        public override string ToString()
        {
            string output = "";
            Visit((item, level) =>
            {
                for (int i = 0; i < level; i++)
                {
                    output += "--";
                }
                output += $"{item.Name}:{(item.Value != null ? item.Value : Environment.NewLine)}{(item.Value == null ? "" : Environment.NewLine)}";
            });
            return output;
        }
        public string this[string key]
        {
            get
            {
                var item = Find(key);
                if (item != null)
                {
                    return item.Value;
                }
                else
                {
                    throw new System.Exception($"ConfTree({Name}) doesn't contains key {key}");
                }
            }
            set
            {
                var item = Find(key);
                if (item != null)
                {
                    item.Value = value;
                    var xmlNode = XmlOp.Find(_xmlFile, key);
                    xmlNode.InnerText = value;
                }
                else
                {
                    throw new System.Exception($"ConfTree({Name}) doesn't contains key {key}");
                }
            }
        }

        public void Save(string path = null)
        {
            var doc = _xmlFile as XmlDocument;
            path = path == null ? doc.BaseURI.Substring(@"file:///".Length) : path;
            doc.Save(path);
        }
    }
    internal class XmlOp
    {
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
