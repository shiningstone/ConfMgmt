using System;
using JbConf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestConfMgmt
{
    [TestClass]
    public class TestBuildConfTree
    {
        [TestMethod]
        public void TestBuildTree()
        {
            ConfTree conf = new ConfTree("TestBuildTree");
            Assert.IsTrue(conf.Name == "TestBuildTree");

            conf.Add(new ConfItem("Item1", "Value1"));
            Assert.IsTrue(conf["Item1"] == "Value1");
            Assert.IsTrue(conf.Find("Item1").Path == "/TestBuildTree");
        }
    }
}
