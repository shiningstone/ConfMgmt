using Microsoft.VisualStudio.TestTools.UnitTesting;
using JbConf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Utils;

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
            ConfTree conf1 = Builder.Generate(new Dictionary<string, string> {
                { "Item1", "Value1" },
                { "Item2", "Value2" },
            }, "DictionaryConf");
            Assert.IsTrue(conf1.XmlFile == null);
            Builder.Xml.Save(conf1, $"{GlobalVar.ResultPath}/Conf1.xml");
            JbAssert.Equal((conf1.XmlFile as XmlDocument).BaseURI, $"file:///{GlobalVar.ResultPath}/Conf1.xml");

            var conf2 = conf1.Clone("new");
            Debug.WriteLine(conf2.ToString());
            Assert.IsTrue((conf2 as ConfTree).XmlFile == null);
            Builder.Xml.Save(conf2 as ConfTree, $"{GlobalVar.ResultPath}/Conf2.xml");
            JbAssert.Equal(((conf2 as ConfTree).XmlFile as XmlDocument).BaseURI, $"file:///{GlobalVar.ResultPath}/Conf2.xml");

            ConfTree conf = new ConfTree("DictionaryConf");
            conf.Add(conf1);
            conf.Add(conf2);
            Debug.WriteLine(conf.ToString());
            Builder.Xml.Save(conf as ConfTree, $"{GlobalVar.ResultPath}/Conf.xml");
            JbAssert.Equal(((conf as ConfTree).XmlFile as XmlDocument).BaseURI, $"file:///{GlobalVar.ResultPath}/Conf.xml");

            var conf4 = conf.Clone("conf4");

            ConfTree super = new ConfTree("SuperConf");
            super.Add(conf);
            super.Add(conf4);
            Builder.Xml.Save(super, $"{GlobalVar.ResultPath}/Super.xml");
            JbAssert.Equal(((super as ConfTree).XmlFile as XmlDocument).BaseURI, $"file:///{GlobalVar.ResultPath}/Super.xml");
        }
    }
}
