﻿//#define DebugDetailEnable

using System;
using System.Collections.Generic;
using System.Linq;

namespace JbConf
{
    public partial class ConfTree : ConfItem
    {
        public Source Source;
        public List<ConfItem> Sons = new List<ConfItem>();

        public XmlDoc XmlDoc;           //Only valid if this ConfTree is generated from xml file
        public ConfTree Refer;          //the prototype of this ConfTree(Clone)

        public int _maxDepth;
        private int _depth = 0;

        public ConfTree(string name) : base(name, null)
        {
        }

        public string Show()
        {
            string output = "";
            Visit("ToString", (item, level) =>
            {
                for (int i = 0; i < level; i++)
                {
                    output += "--";
                }
                var tag = !string.IsNullOrEmpty(item.Tag) ? item.Tag : "";
                output += $"({item.Path}){item.Name}({tag}):{(item.Value != null ? item.Value : Environment.NewLine)}{(item.Value == null ? "" : Environment.NewLine)}";
                return false;
            });
            return output;
        }

        public override string ToString()
        {
            return $"{Name}:{(Value != null ? Value : Environment.NewLine)}{(Value == null ? "" : Environment.NewLine)}";
        }

        public int MaxDepth
        {
            get
            {
                var str = ToString();
                return _maxDepth + 1;
            }
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
        public void PrefixPath(string prefix)
        {
            Path = !string.IsNullOrEmpty(Path) ? $"{prefix}{Path}" : $"{prefix}";

            foreach (var son in Sons)
            {
                var subtree = son as ConfTree;
                if (subtree != null)
                {
                    subtree.PrefixPath(prefix);
                }
                else
                {
                    son.Path = !string.IsNullOrEmpty(son.Path) ? $"{prefix}{son.Path}" : $"{prefix}/{son.Path}";
                }
            }
        }
        public void Add(ConfItem item)
        {
            Sons.Add(item);
            item.Parent = this;

            var subtree = item as ConfTree;
            string tag = !string.IsNullOrEmpty(Tag) ? $"({Tag})" : "";
            if (subtree != null)
            {
                subtree.PrefixPath($"{Path}/{Name}{tag}");
            }
            else
            {
                item.Path = $"{Path}/{Name}{tag}";
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

        public ConfTree Merge(ConfTree tree)
        {
            foreach (var item in tree.Items)
            {
                if (null == Find(item.Name))
                {
                    Add(item);
                }
                else
                {
                    _log.Warn($"ConfTree({this.Name}) Merge Failed : Item({item.Name}) already exist({item.Value})");
                }
            }
            return this;
        }
        public override ConfItem Clone(string tag = null)
        {
            var conf = new ConfTree(Name)
            {
                Source = Source,
                Value = Value,
                Path = Path,
                Tag = tag == null ? Tag : tag,

                Refer = this,
            };

            foreach (var son in Sons)
            {
                conf.Add(son.Clone());
            }

            return conf;
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
    }
}
