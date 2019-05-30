using System;
using System.Collections.Generic;
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
        public bool IsNode()
        {
            return Value == null;
        }
        public override string ToString()
        {
            return $"{Name}:{(Value != null ? Value : Environment.NewLine)}{(Value == null ? "" : Environment.NewLine)}";
        }
    }
    public class ConfTree
    {
        public static bool IsConfItem(XmlNode node)
        {
            XmlElement ele = node as XmlElement;
            return (ele != null) && (!string.IsNullOrEmpty(ele.Name));
        }
        public static bool IsLeaf(XmlNode node)
        {
            return (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text);
        }

        public List<object> Root;
        private int _depth = 0;
        private XmlNode _origXml;

        public ConfTree(string path)
        {
            var xml = new XmlDocument();
            xml.Load(path);

            Root = Build(xml);
        }

        public List<object> Build(XmlNode node)
        {
            List<object> lists = new List<object>();
            if (Root == null)
            {
                Root = lists;
                _origXml = node;
            }

            XmlNodeList subNodes = node.ChildNodes;
            if (subNodes.Count == 0)/*comment*/
            {

            }
            else if (IsLeaf(node))
            {
                XmlElement ele = node as XmlElement;
                lists.Add(new ConfItem(ele.Name, ele.InnerText));
            }
            else/*others*/
            {
                if (IsConfItem(node))
                {
                    lists.Add(new ConfItem(node.Name));
                }

                if (subNodes.Count >= 1)
                {
                    foreach (var subNode in subNodes)
                    {
                        lists.Add(Build((XmlNode)subNode));
                    }
                }
            }

            return lists;
        }
        public void Visit(Action<ConfItem, int> executor, List<object> lists = null)
        {
            if (lists == null)
            {
                lists = Root;
                _depth = 0;
            }

            var curDepth = _depth;
            foreach (var ele in lists)
            {
                List<object> list = ele as List<object>;
                if (list != null)
                {
                    Visit(executor, list);
                }
                else
                {
                    var item = ele as ConfItem;
                    executor(item, _depth);
                    if (item.IsNode())
                    {
                        _depth++;
                    }
                }
            }
            _depth = curDepth;
        }
        public ConfItem Find(string name)
        {
            ConfItem result = null;
            Visit((item, level) =>
            {
                if (item.Name == name)
                {
                    result = item;
                }
            });
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
                output += item.ToString();
            });
            return output;
        }

        public string Name
        {
            get
            {
                return ((Root[0] as List<object>)[0] as ConfItem).Name;
            }
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
                    var xmlNode = XmlOp.Find(_origXml, key);
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
            var doc = _origXml as XmlDocument;
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
