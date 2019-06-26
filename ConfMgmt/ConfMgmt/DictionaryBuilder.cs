using System.Collections.Generic;

namespace JbConf
{
    public class DictionaryBuilder
    {
        public static ConfTree Generate(Dictionary<string, string> kvs, string name)
        {
            ConfTree result = new ConfTree(name);
            result.Path = "";
            result.Source = Source.Dictionary;
            result.Tag = "Default";

            foreach (var kv in kvs)
            {
                result.Add(new ConfItem(kv.Key, kv.Value));
            }
            ConfMgmt.Add(result);
            return result;
        }
    }
}
