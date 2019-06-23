#define DebugDetailEnable

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
    public class ConfTree : ConfItem
    {
        public Source Source;
        public List<ConfItem> Sons = new List<ConfItem>();

        public object XmlFile;
        private int _depth = 0;
        protected string _currentTag;

        public ConfTree(string name) : base(name, null)
        {
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
                Visit((item, level) => {
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
        public string this[string key]
        {
            get
            {
                var tag_path = ExtractTag(key);
                var item = Find(tag_path[1], tag_path[0]);
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

        public bool Visit(Func<ConfItem, int, bool> executor, ConfItem item = null)
        {
            if (item == null)
            {
                _depth = 0;
                item = this;
            }

            var tree = item as ConfTree;
            if(tree != null)
            {
                DebugDetail($@"Visiting ConfTree: {tree.Path}/{tree.Name}({tree.Tag})");

                executor(item, _depth);

                foreach (var c in tree.Sons)
                {
                    _depth++;
                    var ret = Visit(executor, c);
                    _depth--;

                    if (ret)
                    {
                        return true;
                    }
                }
            }
            else
            {
                DebugDetail($"Visiting ConfItem: {item.Name}({item.Value})");
                if (executor(item, _depth))
                {
                    return true;
                }
            }

            return false;
        }
        public ConfItem Find(string itemName, string tag = null)
        {
            ConfItem result = null;

            if (!itemName.Contains(@"/"))
            {
                Visit((item, level) =>
                {
                    if (!string.IsNullOrEmpty(item.Tag))
                    {
                        _currentTag = item.Tag;
                        DebugDetail($"_currentTag: {_currentTag}");
                    }

                    if (item.Name == itemName && (tag == null || _currentTag == tag))
                    {
                        DebugDetail($"Find {item.Path}/{item.Name}");
                        result = item;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            else
            {
                var head_tail = ExtractHead(itemName);

                var tree = Find(head_tail[0], tag) as ConfTree;
                if(tree != null && (tag == null || tag == tree.Tag))
                {
                    var item = tree.Find(head_tail[1]);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }
            return result;
        }
        public void Save(string path = null)
        {
            XmlBuilder.Save(this, path);
        }
    }
}
