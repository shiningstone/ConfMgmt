using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ConfMgmt;
using System.IO;
using System.Xml;

namespace TestConfMgmt
{
    [TestClass]
    public class ConfTreeTest
    {
        private static string SamplePath = @"D:\ConfMgmt\ConfMgmt\TestConfMgmt\TestSamples";
        private static string RealConfPath = @"D:\TestSystem\Configurations\";
        [TestMethod]
        public void Test_Basic()
        {
            ConfTree conf = XmlBuilder.Generate($@"{SamplePath}\Basic.xml");
            Debug.WriteLine(conf.ToString());

            Assert.IsTrue(conf["Item1"] == "Value1");
            Assert.IsTrue(conf["Item2"] == "Value2");
            Assert.IsTrue(conf["Item3"] == "Value3");
            Assert.IsTrue(conf["Item4"] == "Value4");

            conf["Item1"] = "Value5";
            Assert.IsTrue(conf["Item1"] == "Value5");
            conf.Save($@"{SamplePath}\BasicResult.xml");
            //conf.Save();

            conf = XmlBuilder.Generate($@"{SamplePath}\BasicResult.xml");
            Assert.IsTrue(conf["Item1"] == "Value5");
            Assert.IsTrue(conf["Item2"] == "Value2");
            Assert.IsTrue(conf["Item3"] == "Value3");
            Assert.IsTrue(conf["Item4"] == "Value4");
        }
        [TestMethod]
        public void Test_BasicNoExist()
        {
            ConfTree conf = XmlBuilder.Generate($@"{SamplePath}\Basic.xml");
            Assert.ThrowsException<Exception>(() => { var result = conf["NonExisting"]; });
        }
        [TestMethod]
        public void Test_SameItemName()
        {
            ConfTree conf = XmlBuilder.Generate($@"{SamplePath}\SameItemName.xml");

            Debug.WriteLine(conf.ToString());
            Assert.IsTrue(conf[@"Function1\Item1"] == "Value1");
            Assert.IsTrue(conf[@"Function2\Item1"] == "Func2-1");
            Assert.IsTrue(conf[@"Function3\Item1"] == "Func3-1");
        }
        [TestMethod]
        public void BuildRealConf()
        {
            Traverse(RealConfPath, (path) =>
            {
                try
                {
                    Debug.WriteLine(path);
                    
                    ConfTree conf = XmlBuilder.Generate(path);
                    Debug.WriteLine(conf.ToString());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to BuildConfTree for {path}");
                }
            });
        }
        private void Traverse(string path, Action<string> action)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            DirectoryInfo[] dirs = root.GetDirectories();
            if (dirs.Length == 0)
            {
                var files = root.GetFiles();
                foreach (var f in files)
                {
                    action(f.FullName);
                }
            }
            else
            {
                foreach (DirectoryInfo dir in dirs)
                {
                    Traverse(dir.FullName, action);
                }
            }
        }
    }
}