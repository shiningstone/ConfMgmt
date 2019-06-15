using System.Collections.Generic;

namespace JbConf
{
    public class ConfMgmt
    {
        public static Dictionary<string, ConfTree> Trees = new Dictionary<string, ConfTree>();
        public static void Add(ConfTree tree)
        {
            if (!Trees.ContainsKey(tree.Name) || Trees[tree.Name].Source == Source.Dictionary)
            {
                Trees[tree.Name] = tree;
            }
        }
        public static string GetItem(string index)
        {
            string[] indexes = index.Split('\\');
            return Trees[indexes[0]][indexes[1]];
        }
    }
}
