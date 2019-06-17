using System.Collections.Generic;

namespace JbConf
{
    public class ConfMgmt
    {
        public static Dictionary<string, ConfTree> Trees = new Dictionary<string, ConfTree>();
        public static void Add(ConfTree tree)
        {
            if (!Trees.ContainsKey(tree.Name))
            {
                Trees[tree.Name] = tree;
            }
            else
            {
                Trees[tree.Name] = Trees[tree.Name].Merge(tree);
            }
        }
        public static string GetItem(string index)
        {
            string[] indexes = index.Split('\\');
            return Trees[indexes[0]][indexes[1]];
        }
    }
}
