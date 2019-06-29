using System;
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
}
