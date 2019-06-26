using Microsoft.VisualStudio.TestTools.UnitTesting;
using JbConf;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestConfMgmt
{
    [TestClass]
    public class TestConfTree
    {
        [TestMethod]
        public void TestConfTree_Clone()
        {
            ConfTree conf1 = DictionaryBuilder.Generate(new Dictionary<string, string> {
                { "Item1", "Value1" },
                { "Item2", "Value2" },
            }, "DictionaryConf");
            (conf1 as ConfTree).Save($"{GlobalVariables.ResultPath}/Conf1.xml");

            var conf2 = conf1.Clone("new");
            Debug.WriteLine(conf2.ToString());
            (conf2 as ConfTree).Save($"{GlobalVariables.ResultPath}/Conf2.xml");
        }
    }
}
