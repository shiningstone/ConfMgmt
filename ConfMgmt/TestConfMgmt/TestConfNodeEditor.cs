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
        public void TestConfTree_Clone_SaveToDiffFile()
        {
            ConfTree conf1 = Builder.Generate(new Dictionary<string, string> {
                { "Item1", "Value1" },
                { "Item2", "Value2" },
            }, "DictionaryConf");
            Assert.IsTrue(conf1.XmlDoc == null);
            Builder.Xml.Save(conf1, $"{GlobalVar.ResultPath}/Conf1.xml");
            JbAssert.PathEqual((conf1.XmlDoc as XmlDocument).BaseURI, $"file:///{GlobalVar.ResultPath}/Conf1.xml");

            var conf2 = conf1.Clone("new");
            Debug.WriteLine(conf2.ToString());
            Assert.IsTrue((conf2 as ConfTree).XmlDoc == null);
            Builder.Xml.Save(conf2 as ConfTree, $"{GlobalVar.ResultPath}/Conf2.xml");
            JbAssert.PathEqual(((conf2 as ConfTree).XmlDoc as XmlDocument).BaseURI, $"file:///{GlobalVar.ResultPath}/Conf2.xml");

            ConfTree conf = new ConfTree("DictionaryConf");
            conf.Add(conf1);
            conf.Add(conf2);
            Debug.WriteLine(conf.ToString());
            Builder.Xml.Save(conf as ConfTree, $"{GlobalVar.ResultPath}/Conf.xml");
            JbAssert.PathEqual(((conf as ConfTree).XmlDoc as XmlDocument).BaseURI, $"file:///{GlobalVar.ResultPath}/Conf.xml");

            var conf4 = conf.Clone("conf4");

            ConfTree super = new ConfTree("SuperConf");
            super.Add(conf);
            super.Add(conf4);
            Builder.Xml.Save(super, $"{GlobalVar.ResultPath}/Super.xml");
            JbAssert.PathEqual(((super as ConfTree).XmlDoc as XmlDocument).BaseURI, $"file:///{GlobalVar.ResultPath}/Super.xml");
        }
        [TestMethod]
        public void TestConfTree_Clone_SaveToSameFile()
        {
            ConfTree conf1 = Builder.Generate(new Dictionary<string, string> {
                { "Item1", "Value1" },
                { "Item2", "Value2" },
            }, "DictionaryConf");
            Builder.Xml.Save(conf1, $"{GlobalVar.ResultPath}/Conf.xml");

            ConfTree conf2 = conf1.Clone("new") as ConfTree;
            Builder.Xml.Save(conf2 as ConfTree);

            ConfTree readback = Builder.Xml.Generate($"{GlobalVar.ResultPath}/Conf.xml");
            Debug.WriteLine(readback.ToString());
            JbAssert.Equal(readback["Default:Item1"], "Value1");
            JbAssert.Equal(readback["new:Item1"], "Value1");
        }
        [TestMethod]
        public void TestConfTree_Clone_Modify_Save()
        {
            ConfTree conf1 = Builder.Generate(new Dictionary<string, string> {
                { "Item1", "Value1" },
                { "Item2", "Value2" },
            }, "DictionaryConf");
            Builder.Xml.Save(conf1, $"{GlobalVar.ResultPath}/Conf.xml");
            GlobalVar.Log.Debug(conf1.ShowAll());

            ConfTree conf2 = conf1.Clone("new") as ConfTree;
            Builder.Xml.Save(conf2 as ConfTree);

            conf1["Item1"] = "Value3";
            Builder.Xml.Save(conf1 as ConfTree);

            Debug.WriteLine(conf2.ToString());
            conf2["Item2"] = "Value4";
            Builder.Xml.Save(conf2 as ConfTree);

            ConfTree readback = Builder.Xml.Generate($"{GlobalVar.ResultPath}/Conf.xml");
            Debug.WriteLine(readback.ToString());
            JbAssert.Equal(readback["Default:Item1"], "Value3");
            JbAssert.Equal(readback["Default:Item2"], "Value2");
            JbAssert.Equal(readback["new:Item1"], "Value1");
            JbAssert.Equal(readback["new:Item2"], "Value4");
        }
    }
}
