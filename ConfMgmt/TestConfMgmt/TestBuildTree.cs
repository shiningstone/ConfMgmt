using System;
using JbConf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestConfMgmt
{
    [TestClass]
    public class TestBuildConfTree
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            ConfMgmt.Clear();
        }

        [TestMethod]
        public void TestAddItemToTree()
        {
            ConfTree conf = new ConfTree("TestBuildTree");
            Assert.IsTrue(conf.Name == "TestBuildTree");

            conf.Add(new ConfItem("Item1", "Value1"));
            Assert.IsTrue(conf["Item1"] == "Value1");
            Assert.IsTrue(conf.Find("Item1").Path == "/TestBuildTree");
        }
        [TestMethod]
        public void TestAddTreeToTree()
        {
            ConfTree tree1 = new ConfTree("Tree1");
            tree1.Add(new ConfItem("Item1", "Value1"));

            ConfTree tree2 = new ConfTree("Tree2");
            tree2.Add(new ConfItem("Item2", "Value2"));

            tree1.Add(tree2);

            Assert.IsTrue(tree1.Find("Item1").Path == "/Tree1");
            Assert.IsTrue(tree1.Find("Tree2").Path == "/Tree1");
            Assert.IsTrue(tree1.Find("Item2").Path == "/Tree1/Tree2");
        }
        [TestMethod]
        public void TestAddMixToTree()
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
        public void TestAddMixToTree2()
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
    }
}
