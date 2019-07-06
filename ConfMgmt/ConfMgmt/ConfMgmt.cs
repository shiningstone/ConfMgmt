using System;
using System.Collections.Generic;
using Utils;

namespace JbConf
{
    public class ConfMgmt
    {
        public static Dictionary<string, ConfTree> Root = new Dictionary<string, ConfTree>();
        public static void Clear()
        {
            Root = new Dictionary<string, ConfTree>();
        }

        public static void Generate(string path)
        {
            Act.Traverse(path, (file) =>
            {
                Root[file] = Builder.Xml.Generate(file);
            });
        }
        public static void Save(string path = null)
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

        public static ConfTree GetTree(string file)
        {
            return Root[file];
        }
        public static string GetItem(string file, string path)
        {
            return Root[file][path];
        }
    }
}
