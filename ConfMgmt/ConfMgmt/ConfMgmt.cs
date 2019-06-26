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
            if (Root.Find(tree.Name) != null)
            {
                Root.Add(tree);
            }
            else
            {
                Root.Add(tree);
            }
        }
    }
}
