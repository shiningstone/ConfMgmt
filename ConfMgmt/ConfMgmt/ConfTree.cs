//#define DebugDetailEnable

using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace JbConf
{
    public enum Source
    {
        Xml,
        Dictionary,
    }

    public class ConfItem
    {
        protected static Logger _log = new Logger("ConfTree");

        public string Name;
        public string Value;
        public string Path;
        public string Tag;

        public ConfItem(string name, string value = null)
        {
            Name = name;
            Value = value;
        }
        public virtual ConfItem Clone(string tag = null)
        {
            return new ConfItem(Name, Value)
            {
                Path = Path,
                Tag = tag == null ? Tag : tag,
            };
        }

        public override string ToString()
        {
            return $"{Name}:{(Value != null ? Value : Environment.NewLine)}{(Value == null ? "" : Environment.NewLine)}";
        }
        protected static string[] ExtractTag(string path)
        {
            if (path.Contains(":"))
            {
                var strs = path.Split(':');
                return new string[] { strs[0], strs[1] };
            }
            else
            {
                return new string[] { null, path };
            }
        }
        protected static string[] ExtractHead(string path)
        {
            var strs = path.Split('/').ToList();
            return new string[] { strs[0], string.Join(@"/", strs.Skip(1).ToArray()) };
        }
        protected static void DebugDetail(string str)
        {
#if DebugDetailEnable
            _log.Debug(str);
#endif
        }
    }
    public partial class ConfTree : ConfItem
    {
        public Source Source;
        public List<ConfItem> Sons = new List<ConfItem>();

        public object XmlFile;
        private int _depth = 0;

        public ConfTree(string name) : base(name, null)
        {
        }

        public override string ToString()
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

            var subtree = item as ConfTree;
            if (subtree != null)
            {
                subtree.PrefixPath($"{Path}/{Name}");
            }
            else
            {
                item.Path = $"{Path}/{Name}";
            }
        }
        public string this[string key]
        {
            get
            {
                var tag_path = ExtractTag(key);
                var item = Find(tag_path[1], tag_path[0] != null ? tag_path[0].Split('&').ToList() : null);
                if (item != null)
                {
                    return item.Value;
                }
                else
                {
                    throw new Exception($"ConfTree({Name}) doesn't contains key {key}");
                }
            }
            set
            {
                var item = Find(key);
                if (item != null)
                {
                    item.Value = value;
                    XmlBuilder.Modify(XmlFile, key, value);
                }
                else
                {
                    throw new Exception($"ConfTree({Name}({Tag})) doesn't contains key {key}");
                }
            }
        }

        public void Save(string path = null)
        {
            XmlBuilder.Save(this, path);
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
            };

            foreach (var son in Sons)
            {
                conf.Add(son.Clone());
            }

            return conf;
        }
    }
}
