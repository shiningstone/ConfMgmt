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

        public override string ToString()
        {
            return $"{Name}:{(Value != null ? Value : Environment.NewLine)}{(Value == null ? "" : Environment.NewLine)}";
        }
        protected static string[] SplitPath(string fullpath)
        {
            string tag = null, attr = null;
            string path = fullpath;

            if (path.Contains(":"))
            {
                var strs = path.Split(':');
                tag = strs[0];
                path = strs[1];
            }

            if (path.Contains("."))
            {
                var strs = path.Split('.');
                path = strs[0];
                attr = strs[1];
            }

            return new string[] { path, tag, attr };
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
}
