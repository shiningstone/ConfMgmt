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
        public void TestConfTree_AddItemToTree()
        {
            ConfTree conf = new ConfTree("TestBuildTree");
            Assert.IsTrue(conf.Name == "TestBuildTree");

            conf.Add(new ConfItem("Item1", "Value1"));
            Assert.IsTrue(conf["Item1"] == "Value1");
            Assert.IsTrue(conf.Find("Item1").Path == "/TestBuildTree");
        }
        [TestMethod]
        public void TestConfTree_AddTreeToTree()
        {
            ConfTree tree1 = new ConfTree("Tree1");
            tree1.Add(new ConfItem("Item1", "Value1"));

            ConfTree tree2 = new ConfTree("Tree2");
            tree2.Add(new ConfItem("Item2", "Value2"));

            tree1.Add(tree2);

            Assert.IsTrue(tree1.Find("Item1").Path == "/Tree1");
            Assert.IsTrue(tree1.Find("Tree2").Path == "/Tree1");
            Assert.IsTrue(tree1.Find("Item2").Path == "/Tree1/Tree2");

            Assert.IsTrue(tree1.Find("Item1").Parent.Name == "Tree1");
            Assert.IsTrue(tree1.Find("Tree2").Parent.Name == "Tree1");
            Assert.IsTrue(tree1.Find("Item2").Parent.Name == "Tree2");
        }
        [TestMethod]
        public void TestConfTree_AddMixToTree()
        {
            ConfTree tree1 = new ConfTree("Tree1");
            tree1.Add(new ConfItem("Item1-1", "Value1-1"));

            ConfTree tree2 = new ConfTree("Tree2");
            tree2.Add(new ConfItem("Item2-1", "Value2-1"));
            tree2.Add(new ConfItem("Item2-2", "Value2-2"));

            ConfTree tree3 = new ConfTree("Tree3");
            tree3.Add(new ConfItem("Item3-1", "Value3-1"));
            tree3.Add(new ConfItem("Item3-2", "Value3-2"));

            tree2.Add(tree3);
            tree1.Add(tree2);

            Assert.IsTrue(tree1.Find("Item1-1").Path == "/Tree1");
            Assert.IsTrue(tree1.Find("Tree2").Path == "/Tree1");
            Assert.IsTrue(tree1.Find("Item2-1").Path == "/Tree1/Tree2");
            Assert.IsTrue(tree1.Find("Tree3").Path == "/Tree1/Tree2");
            Assert.IsTrue(tree1.Find("Item3-1").Path == "/Tree1/Tree2/Tree3");
            Assert.IsTrue(tree1.Find("Item3-2").Path == "/Tree1/Tree2/Tree3");
        }
        [TestMethod]
        public void TestConfTree_AddMixToTree2()
        {
            ConfTree tree1 = new ConfTree("Tree1");
            tree1.Add(new ConfItem("Item1-1", "Value1-1"));

            ConfTree tree2 = new ConfTree("Tree2");
            tree2.Add(new ConfItem("Item2-1", "Value2-1"));
            tree2.Add(new ConfItem("Item2-2", "Value2-2"));

            ConfTree tree3 = new ConfTree("Tree3");
            tree3.Add(new ConfItem("Item3-1", "Value3-1"));
            tree3.Add(new ConfItem("Item3-2", "Value3-2"));

            tree1.Add(tree2);
            tree2.Add(tree3);

            Assert.IsTrue(tree1.Find("Item1-1").Path == "/Tree1");
            Assert.IsTrue(tree1.Find("Tree2").Path == "/Tree1");
            Assert.IsTrue(tree1.Find("Item2-1").Path == "/Tree1/Tree2");
            Assert.IsTrue(tree1.Find("Tree3").Path == "/Tree1/Tree2");
            Assert.IsTrue(tree1.Find("Item3-1").Path == "/Tree1/Tree2/Tree3");
            Assert.IsTrue(tree1.Find("Item3-2").Path == "/Tree1/Tree2/Tree3");
        }
        [TestMethod]
        public void TestConfTree_Clone_SaveToDiffFile()
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

            ConfTree conf2 = conf1.Clone("new") as ConfTree;
            Builder.Xml.Save(conf2 as ConfTree);

            Debug.WriteLine(conf1.ToString());
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
