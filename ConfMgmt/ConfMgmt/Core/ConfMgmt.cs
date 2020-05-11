using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace JbConf
{
    public class ConfMgmt
    {
        #region singleton
        private static Dictionary<string, ConfMgmt> _inst = new Dictionary<string, ConfMgmt>();
        public static ConfMgmt Default
        {
            get
            {
                if (!_inst.ContainsKey("Default"))
                {
                    _inst["Default"] = new ConfMgmt("Default");
                }
                return _inst["Default"];
            }
        }
        public static ConfMgmt Inst(string name)
        {
            if (!_inst.ContainsKey(name))
            {
                _inst[name] = new ConfMgmt(name);
            }

            return _inst[name];
        }
        public static void Register(string name, ConfMgmt conf)
        {
            if (_inst.ContainsKey(name))
            {
                conf._log.Warn($"ConfMgmt({name}) already exist");
            }

            _inst[name] = conf;
        }
        #endregion

        private Logger _log;

        public string Name;
        public Dictionary<string, ConfTree> Root = new Dictionary<string, ConfTree>();
        public ConfMgmt(string name = "Default")
        {
            Name = name;
            _log = new Logger($"{name}Conf");

            Register(name, this);
        }
        public void Clear()
        {
            Root = new Dictionary<string, ConfTree>();
        }

        public void Generate(string path)
        {
            Act.Traverse(path, (file) =>
            {
                Root[file] = Builder.Xml.Generate(file);
            });
        }
        public void ShowAll(Logger log = null)
        {
            foreach (var tree in Root.Values)
            {
                tree.ShowAll(log == null ? _log : log);
            }
        }
        public ConfTree ReadFile(string file)
        {
            foreach (var kv in Root)
            {
                if (kv.Key.Contains(file))
                {
                    return kv.Value;
                }
            }

            return null;
        }
        public string GetItem(string key)
        {
            var tags_key = key.Split(':');
            var tags = new List<string>();
            if (tags_key.Length > 1)
            {
                key = tags_key[1];
                tags.Add(tags_key[0]);
            }
            else
            {
                tags = null;
            }

            List<ConfItem> items = new List<ConfItem>();

            foreach (var tree in Root.Values)
            {
                var conf = tree.Find(key, tags);
                if (conf != null)
                {
                    items.Add(conf);
                }
            }

            if (items.Count == 1)
            {
                return items[0].Value;
            }

            if (items.Count == 0)
            {
                _log.Error($"No item({key}) found");
                throw new Exception($"No item({key}) found");
            }
            else
            {
                string errInfo = $"More than 1 item({key}) found: " +
                    $"{string.Join(Environment.NewLine, items.Select(x => $"{x.Path}/{x.Name}: {x.Value}"))}";
                _log.Error(errInfo);
                throw new Exception(errInfo);
            }
        }
        public void SetItem(string key, string value)
        {
            Dictionary<ConfItem, ConfTree> items = new Dictionary<ConfItem, ConfTree>();

            foreach (var tree in Root.Values)
            {
                var conf = tree.Find(key);
                if (conf != null)
                {
                    items[conf] = tree;
                }
            }

            if (items.Count == 1)
            {
                var kv = items.First();
                kv.Value[key] = value;
            }
            else if (items.Count == 0)
            {
                _log.Error($"No item({key}) found");
                throw new Exception($"No item({key}) found");
            }
            else
            {
                string errInfo = $"More than 1 item({key}) found: " +
                    $"{string.Join(Environment.NewLine, items.Select(x => $"{x.Key.Path}/{x.Key.Name}: {x.Key.Value}"))}";
                _log.Error(errInfo);
                throw new Exception(errInfo);
            }
        }

        public ConfTree Clone(string target, string tag)
        {
            var tree = ReadFile(target);
            var newtree = tree.Clone(tag);
            Builder.Xml.Save(newtree as ConfTree, $@"{Path.GetDirectoryName(FileOp.ExtractUrl(tree.XmlDoc.BaseURI))}\{tag}.xml");
            return newtree as ConfTree;
        }
        public void Save(string path = null)
        {
            if (path == null)
            {
                foreach (var conf in Root.Values)
                {
                    Builder.Xml.Save(conf, path);
                }
            }
            else
            {
                ConfTree root = new ConfTree("Root");
                foreach (var conf in Root.Values)
                {
                    root.Add(conf);
                }
                Builder.Xml.Save(root, path);
            }
        }
    }
}
