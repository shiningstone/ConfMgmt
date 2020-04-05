using System;
using System.Collections.Generic;
using System.Linq;
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
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();

        public string Path;
        public string Tag;
        public ConfItem Parent;

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

        public virtual bool Equals(ConfItem item)
        {
            return (Name == item.Name) && (Value == item.Value);
        }

        public override string ToString()
        {
            return $"{Name}:{(Value != null ? Value : Environment.NewLine)}{(Value == null ? "" : Environment.NewLine)}";
        }
        protected static string[] ExtractHead(string path)
        {
            var strs = path.Split('/').ToList();
            return new string[] { strs[0], string.Join(@"/", strs.Skip(1).ToArray()) };
        }
        protected static void DebugDetail(string str)
        {
#if !DebugDetailEnable
            _log.Debug(str);
#endif
        }
    }
}
