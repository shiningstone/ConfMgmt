//#define DebugDetailEnable

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

        public void ShowAll(Logger log = null)
        {
            string output = $"{Environment.NewLine}";

            Visit("ShowAll", (item, level) =>
            {
                for (int i = 0; i < level; i++)
                {
                    output += "--";
                }
                output += $"{item}";
                return false;
            });

            (log == null ? _log : log).Debug(output);
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

        public void Add(ConfItem item)
        {
            item.Parent = this;
            Sons.Add(item);
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
    }
}
