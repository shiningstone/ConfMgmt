using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using ConfMgmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestConfMgmt
{
    [TestClass]
    public class TestConfigBuilder
    {
        private static string SamplePath = @"D:\ConfMgmt\ConfMgmt\TestConfMgmt\TestSamples";
        [TestMethod]
        public void BuildConfigFromXmlFile()
        {
            ConfTree conf = new ConfTree($@"{SamplePath}\Sample1.xml");
            Debug.WriteLine(conf.ToString());

            Assert.IsTrue(conf["Item1"] == "Value1");
            Assert.IsTrue(conf["Item2"] == "Value2");
            Assert.IsTrue(conf["Item3"] == "Value3");
            Assert.IsTrue(conf["Item4"] == "Value4");

            Assert.ThrowsException<Exception>(() => { var result = conf["NonExisting"]; });

            conf["Item1"] = "Value5";
            Assert.IsTrue(conf["Item1"] == "Value5");
            conf.Save($@"{SamplePath}\Sample1-result.xml");
            //conf.Save();
        }
    }
}