using System;
using Utils;

namespace JbConf
{
    public class ConfMgmt
    {
        private static Logger _log = new Logger("ConfMgmt");
        public static ConfTree Root = new ConfTree("Root");
        public static void Clear()
        {
            Root = new ConfTree("Root");
        }

        public static void Add(ConfTree tree)
        {
            if (tree == null)
            {
                _log.Warn($"Add(null) ignored intentionally");
                return;
            }

            var target = Root.Find(tree.Name);
            if (target == null)
            {
                Root.Add(tree);
            }
            else
            {
                if (target.Tag == tree.Tag)
                {
                    _log.Warn($"ConfTree(Name:{tree.Name}, Tag:{tree.Tag}) already exist");
                }

                (target.Parent as ConfTree).Add(tree);
            }
        }

        public static void Generate(string path)
        {
            Act.Traverse(path, (file) =>
            {
                ConfTree conf = XmlBuilder.Generate(file);
            });
        }
        public static void Save(string path = null)
        {
            Root.Save(path);
        }
    }
}
