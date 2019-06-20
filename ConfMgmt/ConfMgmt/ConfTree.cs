using System;
using System.Collections.Generic;
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
        public string Name;
        public string Value;

        public string Path;

        public ConfItem(string name, string value = null)
        {
            Name = name;
            Value = value;
        }
        public override string ToString()
        {
            return $"{Name}:{(Value != null ? Value : Environment.NewLine)}{(Value == null ? "" : Environment.NewLine)}";
        }
    }
    public class ConfTree : ConfItem
    {
        public static Logger _log = new Logger("ConfTree");

        public Source Source;
        public List<ConfItem> Sons = new List<ConfItem>();

        private int _depth = 0;
        public object XmlFile;

        public ConfTree(string name) : base(name, null)
        {
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
        public ConfItem Find(string itemName)
        {
            ConfItem result = null;

            if (!itemName.Contains(@"\"))
            {
                Visit((item, level) =>
                {
                    if (item.Name == itemName)
                    {
                        result = item;
                    }
                });
            }
            else
            {
                string[] strs = itemName.Split('\\');

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
                output += $"({item.Path}){item.Name}:{(item.Value != null ? item.Value : Environment.NewLine)}{(item.Value == null ? "" : Environment.NewLine)}";
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
                    throw new Exception($"ConfTree({Name}) doesn't contains key {key}");
                }
            }
        }

        public List<ConfItem> Items
        {
            get
            {
                var result = new List<ConfItem>();
                Visit((item, level) => {
                    result.Add(item);
                });
                return result;
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
        public void Save(string path = null)
        {
            XmlBuilder.Save(this, path);
        }
    }
}
