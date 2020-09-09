using System;
using System.Collections.Generic;
using Utils;

namespace JbConf
{
    public enum Source
    {
        Code,
        Xml,
        Dictionary,
    }
    public class Index
    {
        public string Path;
        public string Tag;
        public string Attr;

        public Index(string fullpath)
        {
            Path = fullpath;

            if (Path.Contains(":"))
            {
                var strs = Path.Split(':');
                Tag = strs[0];
                Path = strs[1];
            }

            if (Path.Contains("."))
            {
                var strs = Path.Split('.');
                Path = strs[0];
                Attr = strs[1];
            }
        }
    }
    public class ConfItem
    {
        protected static Logger _log = new Logger("ConfTree");

        public string Name;
        public string Value;
        public ConfItem Parent;
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
        public string Tag;

        public string Path
        {
            get
            {
                if (Parent == null)
                {
                    return "";
                }
                else
                {
                    string tag = string.IsNullOrEmpty(Parent.Tag) ? "" : $"({Parent.Tag})";
                    return $"{Parent.Path}/{Parent.Name}{tag}";
                }
            }
        }
        public string SelfPath
        {
            get
            {
                string tag = string.IsNullOrEmpty(Tag) ? "" : $"({Parent.Tag})";
                return $"{Path}/{Name}{tag}";
            }
        }
        public ConfItem(string name, string value = null)
        {
            Name = name;
            Value = value;
        }
        public virtual ConfItem Clone(string tag = null)
        {
            return new ConfItem(Name, Value)
            { 
                Tag = tag == null ? Tag : tag,
            };
        }

        public virtual bool Equals(ConfItem item)
        {
            return (Name == item.Name) && (Value == item.Value);
        }

        public override string ToString()
        {
            return $"{Name}:{(Value != null ? Value : Environment.NewLine)}{(Value == null ? "" : Environment.NewLine)}";
        }
    }
}
