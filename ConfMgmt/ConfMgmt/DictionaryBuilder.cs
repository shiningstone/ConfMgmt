using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JbConf
{
    public class DictionaryBuilder
    {
        public static ConfTree Generate(Dictionary<string, string> kvs, string name)
        {
            ConfTree result = new ConfTree(name);
            result.Sons = new List<ConfItem>();
            foreach (var kv in kvs)
            {
                result.Sons.Add(new ConfItem(kv.Key, kv.Value));
            }
            return result;
        }
    }
}
