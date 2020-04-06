﻿using System;
using System.Collections.Generic;
using System.IO;
using Utils;

namespace JbConf
{
    public class ConfMgmt
    {
        private static Logger _log = new Logger("ConfMgmt");

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
                _log.Debug($"{Root[file].ShowAll()}");
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
        public static ConfTree Clone(string target, string tag)
        {
            var tree = ReadFile(target);
            var newtree = tree.Clone(tag);
            Builder.Xml.Save(newtree as ConfTree, $@"{Path.GetDirectoryName(FileOp.ExtractUrl(tree.XmlDoc.BaseURI))}\{tag}.xml");
            return newtree as ConfTree;
        }

        public static ConfTree ReadFile(string file)
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
    }
}
