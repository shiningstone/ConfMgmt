using Microsoft.VisualStudio.TestTools.UnitTesting;
using JbConf;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestConfMgmt
{
    [TestClass]
    public class TestConfTree
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            GlobalVar.Initialize();
        }

        [TestMethod]
        public void TestConfTree_Clone()
        {
            ConfTree conf1 = DictionaryBuilder.Generate(new Dictionary<string, string> {
                { "Item1", "Value1" },
                { "Item2", "Value2" },
            }, "DictionaryConf");
            (conf1 as ConfTree).Save($"{GlobalVar.ResultPath}/Conf1.xml");

            var conf2 = conf1.Clone("new");
            Debug.WriteLine(conf2.ToString());
            (conf2 as ConfTree).Save($"{GlobalVar.ResultPath}/Conf2.xml");

            ConfTree conf = new ConfTree("DictionaryConf");
            conf.Add(conf1);
            conf.Add(conf2);
            Debug.WriteLine(conf.ToString());
            (conf as ConfTree).Save($"{GlobalVar.ResultPath}/Conf.xml");

            var conf4 = conf.Clone("conf4");

            ConfTree super = new ConfTree("SuperConf");
            super.Add(conf);
            super.Add(conf4);
            super.Save($"{GlobalVar.ResultPath}/Super.xml");
        }
    }
}
