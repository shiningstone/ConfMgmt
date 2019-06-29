using System.Collections.Generic;

namespace JbConf
{
    public class ConfMgmt
    {
        public static ConfTree Root = new ConfTree("Root");
        public static void Clear()
        {
            Root = new ConfTree("Root");
        }
        public static void Add(ConfTree tree)
        {
            var target = Root.Find(tree.Name);
            if (target == null)
            {
                Root.Add(tree);
            }
            else if (target.Tag != tree.Tag)
            {
                (target.Parent as ConfTree).Add(tree);
            }
            else
            {
                throw new System.Exception($"ConfTree(Name:{tree.Name}, Tag:{tree.Tag}) already exist");
            }
        }
    }
}
