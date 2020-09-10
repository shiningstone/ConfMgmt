using Microsoft.VisualStudio.TestTools.UnitTesting;
using JbConf;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Utils;

namespace TestConfMgmt
{
    [TestClass]
    public class TestConfNodeEditor
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            GlobalVar.Initialize(ConfMgmt);
        }

        private ConfMgmt ConfMgmt = new ConfMgmt();

        [TestMethod]
        public void TestConfNodeEditor_AddNode_ToDefaultTagParent()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/DefaultSpec.xml");
            conf.Save($@"{GlobalVar.ResultPath}\root.xml");
            conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}\root.xml");

            var node = conf.Find(@"Specs/BR");
            Assert.IsTrue(node == null);

            ConfTree newnode = new ConfTree("BR");
            newnode.Add(new ConfItem("Max", "10"));
            newnode.Add(new ConfItem("Min", "0"));
            var tree = conf.GetItem(@"Specs") as ConfTree;
            tree.AddNode(newnode);
            conf.Save();

            conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}\root.xml");
            node = conf.Find(@"Specs/BR");
            Assert.IsTrue(node != null);
        }
        [TestMethod]
        public void TestConfNodeEditor_AddNode_ToTagParent()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/DefaultSpec.xml");
            conf.Save($@"{GlobalVar.ResultPath}\root.xml");
            conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}\root.xml");

            var node = conf.Find(@"Specs/BR", new List<string>() { "HighTemp" });
            Assert.IsTrue(node == null);

            ConfTree newnode = new ConfTree("BR");
            newnode.Add(new ConfItem("Max", "10"));
            newnode.Add(new ConfItem("Min", "0"));
            var tree = conf.GetItem(@"HighTemp:Specs") as ConfTree;
            tree.AddNode(newnode);
            conf.Save();

            conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}\root.xml");
            node = conf.Find(@"Specs/BR", new List<string>() { "HighTemp" });
            Assert.IsTrue(node != null);
        }
        [TestMethod]
        public void TestConfNodeEditor_RemoveNode()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/DefaultSpec.xml");
            conf.Save($@"{GlobalVar.ResultPath}\root.xml");
            conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}\root.xml");

            var node = conf.Find(@"Specs/Ith", new List<string>() { "HighTemp" });
            Assert.IsTrue(node != null);

            conf.RemoveNode(node);

            conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}\root.xml");
            node = conf.Find(@"Specs/Ith", new List<string>() { "HighTemp" });
            Assert.IsTrue(node == null);
        }

        [TestMethod]
        public void TestConfNodeEditor_GetSonStructure()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/DefaultSpec.xml");
            conf.Save($@"{GlobalVar.ResultPath}\root.xml");
            conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}\root.xml");

            var item = (conf.GetItem(@"Specs") as ConfTree).Sons[0].Clone();

            Assert.IsTrue(item != null);
        }
    }
}
