using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace JbConf
{
    public class ConfMgmt
    {
        private static Logger _log = new Logger("ConfMgmt");

        public Dictionary<string, ConfTree> Root = new Dictionary<string, ConfTree>();
        public void Clear()
        {
            Root = new Dictionary<string, ConfTree>();
        }

        public void Generate(string path)
        {
            Act.Traverse(path, (file) =>
            {
                Root[file] = Builder.Xml.Generate(file);
                _log.Debug($"{Root[file].ShowAll()}");
            });
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
            List<ConfItem> items = new List<ConfItem>();

            foreach (var tree in Root.Values)
            {
                var conf = tree.Find(key);
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
