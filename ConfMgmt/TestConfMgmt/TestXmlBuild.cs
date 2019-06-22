using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using JbConf;
using System.IO;

namespace TestConfMgmt
{
    [TestClass]
    public class TestXmlBuild
    {
        [TestMethod]
        public void TestXmlBuild_Basic()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\Basic.xml");
            Debug.WriteLine(conf.ToString());

            Assert.IsTrue(conf["Item1"] == "Value1");
            Assert.IsTrue(conf["Item2"] == "Value2");
            Assert.IsTrue(conf["Item3"] == "Value3");
            Assert.IsTrue(conf["Item4"] == "Value4");
            Assert.IsTrue(conf["Item5"] == "Value5");

            conf["Item1"] = "Value5";
            Assert.IsTrue(conf["Item1"] == "Value5");
            conf.Save($@"{GlobalVariables.SamplePath}\BasicResult.xml");
            //conf.Save();

            conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\BasicResult.xml");
            Assert.IsTrue(conf["Item1"] == "Value5");
            Assert.IsTrue(conf["Item2"] == "Value2");
            Assert.IsTrue(conf["Item3"] == "Value3");
            Assert.IsTrue(conf["Item4"] == "Value4");
        }
        [TestMethod]
        public void TestXmlBuild_Basic_Path()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\Basic.xml");
            Debug.WriteLine(conf.ToString());

            Assert.IsTrue(conf.Find("Item1").Path == "/Basic/Function1");
            Assert.IsTrue(conf.Find("Item2").Path == "/Basic/Function1");
            Assert.IsTrue(conf.Find("Item3").Path == "/Basic/Function2");
            Assert.IsTrue(conf.Find("Item4").Path == "/Basic/Function2");
            Assert.IsTrue(conf.Find("Item5").Path == "/Basic");

            conf["Item1"] = "Value5";
            Assert.IsTrue(conf.Find("Item1").Path == "/Basic/Function1");
        }
        [TestMethod]
        public void TestXmlBuild_BasicNoExist()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\Basic.xml");
            Assert.ThrowsException<Exception>(() => { var result = conf["NonExisting"]; });
        }
        [TestMethod]
        public void TestXmlBuild_SameItemName()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\SameItemName.xml");

            Debug.WriteLine(conf.ToString());
            Assert.IsTrue(conf[@"Function1\Item1"] == "Value1");
            Assert.IsTrue(conf[@"Function2\Item1"] == "Func2-1");
            Assert.IsTrue(conf[@"Function3\Item1"] == "Func3-1");
        }
        [TestMethod]
        public void TestXmlBuild_Tag()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\Tag.xml");
            Debug.WriteLine(conf.ToString());
            Assert.IsTrue(conf[@"Default:Func\Item1"] == "0");
            Assert.IsTrue(conf[@"Tag1:Func\Item1"] == "1.1");
            Assert.IsTrue(conf[@"Tag2:Func\Item1"] == "2.1");
        }
        [TestMethod]
        public void TestXmlBuild_BuildRealConf()
        {
            Traverse(GlobalVariables.RealConfPath, (path) =>
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