﻿//#define DebugDetailEnable

using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace JbConf
{
    public partial class ConfTree : ConfItem
    {
        public Source Source;
        public List<ConfItem> Sons = new List<ConfItem>();

        public XmlDoc XmlDoc;           //Only valid if this ConfTree is generated from xml file
        public ConfTree Refer;          //the prototype of this ConfTree(Clone)

        public ConfTree(string name) : base(name, null)
        {
        }
        public ConfTree(string name, Dictionary<string, double> values) : base(name, null)
        {
            foreach (var v in values)
            {
                Add(new ConfItem(v.Key, v.Value.ToString()));
            }
        }

        public string ShowAll(Logger log = null, bool compact = true)
        {
            string output = compact ? "" : $"{Environment.NewLine}";

            Visit("ShowAll", (item, level) =>
            {
                if (compact)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        output += $"{item.ToString().Trim()};";
                    }
                }
                else
                {
                    for (int i = 0; i < level; i++)
                    {
                        output += "--";
                    }
                    output += $"{item}";
                }
                return false;
            });

            (log == null ? _log : log).Debug(output);
            return output;
        }
        public override string ToString()
        {
            return $"{Name}({(Tag != null ? Tag : "")}){Environment.NewLine}";
        }
        public List<ConfItem> Items
        {
            get
            {
                var result = new List<ConfItem>();
                Visit("Items", (item, level) => {
                    result.Add(item);
                    return false;
                });
                return result;
            }
        }
        public List<ConfTree> SubTrees
        {
            get
            {
                var result = new List<ConfTree>();
                Visit("SubTrees", (item, level) => {
                    if (level > 0 && item is ConfTree)
                    {
                        result.Add(item as ConfTree);
                    }
                    return false;
                });
                return result;
            }
        }

        public void Add(ConfItem item)
        {
            item.Parent = this;
            Sons.Add(item);
        }

        public void AddNode(ConfItem item)
        {
            item.Parent = this;
            Sons.Add(item);

            if (XmlDoc != null)
            {
                XmlDoc.AddNode(item.Path, item);
                Save();
            }
        }

        public void RemoveNode(ConfItem item)
        {
            Sons.Remove(item);

            if (XmlDoc != null)
            {
                XmlDoc.RemoveNode(item.SelfPath);
                Save();
            }
        }

        public ConfItem GetItem(string key)
        {
            var index = new Index(key);

            var item = Find(index.Path, index.Tag != null ? index.Tag.Split('&').ToList() : null);
            if (item != null)
            {
                return item;
            }
            else
            {
                throw new Exception($"ConfTree({Name}) doesn't contains key {key}");
            }
        }

        public string this[string key]
        {
            get
            {
                var index = new Index(key);

                var item = Find(index.Path, index.Tag != null ? index.Tag.Split('&').ToList() : null);
                if (item != null)
                {
                    return index.Attr != null ? item.Attributes[index.Attr] : item.Value;
                }
                else
                {
                    throw new Exception($"ConfTree({Name}) doesn't contains key {key}");
                }
            }
            set
            {
                var index = new Index(key);

                var item = Find(index.Path, index.Tag != null ? index.Tag.Split('&').ToList() : null);
                if (item != null)
                {
                    if (index.Attr == null)
                    {
                        item.Value = value;
                    }
                    else
                    {
                        item.Attributes[index.Attr] = value;
                    }

                    XmlDoc?.Modify($"{item.Path}/{item.Name}", value, index.Attr);
                }
                else
                {
                    throw new Exception($"ConfTree({Name}({Tag})) doesn't contains key {key}");
                }
            }
        }

        //如果是XML文件Load出来的ConfTree, path非必需参数
        public void Save(string path = null)
        {
            Builder.Xml.Save(this, path);
        }

        public override ConfItem Clone(string tag = null)
        {
            var conf = new ConfTree(Name)
            {
                Source = Source,
                Value = Value,
                Tag = tag == null ? Tag : tag,
                Attributes = Attributes,

                Refer = this,
            };

            foreach (var son in Sons)
            {
                conf.Add(son.Clone());
            }

            return conf;
        }

        public override void Clear()
        {
            Name = "";

            foreach (var son in Sons)
            {
                son.Clear();
            }
        }

        public override bool Equals(ConfItem tree)
        {
            var target = tree as ConfTree;
            if (target != null)
            {
                bool result = true;
                foreach (var son in Sons)
                {
                    var subtree = son as ConfTree;
                    var path = son.Path.Substring(target.Path == null ? 1 : target.Path.Length + 1);
                    if (subtree != null)
                    {
                        result &= subtree.Equals(target.Find($"{path}/{subtree.Name}"));
                    }
                    else
                    {
                        var item = target.Find($"{path}/{son.Name}");
                        if (item != null)
                        {
                            result &= son.Equals(item);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                return result;
            }
            else
            {
                return false;
            }
        }

        public ConfTree Append(ConfTree tree)
        {
            foreach (var item in tree.Items)
            {
                if (null == Find(item.Name))
                {
                    Add(item);
                }
                else
                {
                    _log.Warn($"ConfTree({this.Name}) Append Failed : Item({item.Name}) already exist({item.Value})");
                }
            }
            return this;
        }

        public List<ConfItem> AllItems
        {
            get
            {
                var items = new List<ConfItem>();
                Visit("GetItem", (item, level) => {
                    if (!(item is ConfTree))
                    {
                        items.Add(item);
                    }

                    return false;
                });

                return items;
            }
        }
        public Dictionary<string, string> ToDict()
        {
            var result = new Dictionary<string, string>();

            foreach (var item in AllItems)
            {
                result[item.Name] = item.Value;
            }

            return result;
        }
        /// <summary>
        /// tree覆盖this, 注意：Merge没有修改xml文件的内容
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        public ConfTree Merge(ConfTree tree, List<string> targets = null)
        {
            foreach (var item in tree.Items)
            {
                if (targets != null && !targets.Contains(item.Name))
                {
                    continue;
                }

                if (item is ConfTree)
                {
                    continue;
                }

                var orig = Find(item.Name);
                if (null != orig)
                {
                    orig.Value = item.Value;
                }
                else
                {
                    Add(item.Clone());
                }
            }

            return this;
        }
        /// <summary>
        /// 将this的ConItem值更新到tree
        /// </summary>
        /// <param name="target">被修改的Conf文件(XML)</param>
        public void OverWrite(ConfTree target)
        {
            Visit("OverWrite", (item, level) =>
            {
                var conf = target.Find(item.Name);
                if (conf != null && ((conf as ConfTree) == null))
                {
                    target[item.Name] = item.Value;
                }
                return false;
            });
        }
    }
}
