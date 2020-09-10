using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;

namespace Utils
{
    public class XmlReader
    {
        private readonly XmlDocument _xml = null;
        private Logger _logger = new Logger(typeof(XmlReader));

        public XmlReader(string filePath)
        {
            _xml = new XmlDocument();
            _xml.Load(filePath);
        }
        public string GetItem(string[] nodeNames)
        {
            lock (_xml)
            {
                XmlNode[] nodePath = FindItem(nodeNames);

                if (nodePath != null)
                {
                    return nodePath[nodePath.Length - 1].InnerText;
                }
                else
                {
                    return "N/A";
                }
            }
        }
        public string GetItem(string item)
        {
            lock (_xml)
            {
                try
                {
                    return _xml.SelectSingleNode("CONFIG").SelectSingleNode(item).InnerText;
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to get item " + item);
                }
            }
        }
        public string GetItem(string boardName, string item)
        {
            lock (_xml)
            {
                try
                {
                    return _xml.SelectSingleNode("CONFIG").SelectSingleNode(boardName).SelectSingleNode(item).InnerText;
                }
                catch (Exception e)
                {
                    return GetItem(item);
                }
            }
        }

        private Tree _configuration = new Tree();
        public DataTable ToTable()
        {
            DataSet ds = new DataSet();

            _configuration.IsNode = SimpleNode.IsSimpleNode;
            _configuration.Root = _configuration.Build(_xml);

            DataTable table = new DataTable();
            for (int i = 0; i < _configuration.MaxDepth; i++)
            {
                table.Columns.Add();
            }

            _configuration.Visit((node) =>
            {
                DataRow r = table.NewRow();
                r[node.Level - 2] = node.Name;
                r[node.Level - 1] = node.Value;
                table.Rows.Add(r);
            });

            return table;
        }

        private XmlNode[] FindItem(string[] item)
        {
            lock (_xml)
            {
                XmlNode[] nodes = new XmlNode[item.Length];

                XmlNode temp = _xml;
                for (int i = 0; i < nodes.Length; i++)
                {
                    nodes[i] = temp.SelectSingleNode(item[i]);
                    if (nodes[i] != null)
                    {
                        temp = nodes[i];
                    }
                    else
                    {
                        string output = "";
                        for (int j = 0; j < i + 1; j++)
                        {
                            output += item[j];
                            if (j < i)
                            {
                                output += "->";
                            }
                        }
                        _logger.Info("Failed to find " + output);
                        return null;
                    }
                }

                return nodes;
            }
        }

        public void SetItem(string[] item, string value)
        {
            lock (_xml)
            {
                XmlNode[] nodePath = FindItem(item);

                if (nodePath!=null)
                {
                    nodePath[nodePath.Length - 1].InnerText = value;
                }
            }
        }

        public void Save(string file)
        {
            lock (_xml)
            {
                _xml.Save(file);
            }
        }
    }
    public class SimpleNode
    {
        public string Name;
        public string Value;
        public int Level;

        public SimpleNode(string name, int level, string value = "")
        {
            Name = name;
            Level = level;
            Value = value;
        }

        public static bool IsSimpleNode(XmlNode node)
        {
            XmlElement ele = node as XmlElement;
            if ((ele != null) && (!string.IsNullOrEmpty(ele.Name)))
            {
                return true;
            }

            return false;
        }
    }
    public class Tree
    {
        public delegate bool NodeChecker(XmlNode node);

        public NodeChecker IsNode;
        public List<object> Root;
        public int MaxDepth;
        private int _depth;

        public bool IsLeaf(XmlNode node)
        {
            return (node.ChildNodes.Count == 1 && node.FirstChild.NodeType == XmlNodeType.Text);
        }

        public List<object> Build(XmlNode node)
        {
            List<object> lists = new List<object>();

            XmlNodeList subNodes = node.ChildNodes;
            if (subNodes.Count == 0)/*comment*/
            {

            }
            else if (IsLeaf(node))
            {
                _depth++;

                XmlElement ele = node as XmlElement;
                lists.Add(new SimpleNode(ele.Name, _depth, ele.InnerText));

                MaxDepth = _depth > MaxDepth ? _depth : MaxDepth;
                _depth--;
            }
            else/*others*/
            {
                _depth++;

                if (IsNode(node))
                {
                    lists.Add(new SimpleNode(node.Name, _depth));
                }

                if (subNodes.Count >= 1 )
                {
                    foreach (var subNode in subNodes)
                    {
                        lists.Add(Build((XmlNode)subNode));
                    }
                }

                _depth--;
            }

            return lists;
        }
        
        public void Visit(Action<SimpleNode> executor, List<object> lists = null)
        {
            if (lists == null)
            {
                lists = Root;
            }

            foreach (var ele in lists)
            {
                List<object> list = ele as List<object>;
                if (list != null)
                {
                    Visit(executor, list);
                }
                else
                {
                    executor(ele as SimpleNode);
                }
            }
        }
    }
}
