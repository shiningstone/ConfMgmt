﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using JbConf;
using System.IO;
using Utils;

namespace TestConfMgmt
{
    [TestClass]
    public class TestXmlBuild
    {
        [TestMethod]
        public void TestXmlBuild_Basic()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/Basic.xml");
            Debug.WriteLine(conf.ToString());

            JbAssert.Equal(conf["Item1"], "Value1");
            JbAssert.Equal(conf["Item2"], "Value2");
            JbAssert.Equal(conf["Item3"], "Value3");
            JbAssert.Equal(conf["Item4"], "Value4");
            JbAssert.Equal(conf["Item5"], "Value5");

            conf["Item1"] = "Value5";
            JbAssert.Equal(conf["Item1"], "Value5");
            conf.Save($@"{GlobalVariables.SamplePath}/BasicResult.xml");
            //conf.Save();

            conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/BasicResult.xml");
            JbAssert.Equal(conf["Item1"], "Value5");
            JbAssert.Equal(conf["Item2"], "Value2");
            JbAssert.Equal(conf["Item3"], "Value3");
            JbAssert.Equal(conf["Item4"], "Value4");
        }
        [TestMethod]
        public void TestXmlBuild_Basic_Save()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/Basic.xml");

            conf["Item1"] = "Value5";
            conf.Save($@"{GlobalVariables.SamplePath}/BasicResult.xml");
            //conf.Save();

            conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/BasicResult.xml");
            Debug.WriteLine(conf.ToString());
            JbAssert.Equal(conf["Item1"], "Value5");
            JbAssert.Equal(conf["Item2"], "Value2");
            JbAssert.Equal(conf["Item3"], "Value3");
            JbAssert.Equal(conf["Item4"], "Value4");
        }
        [TestMethod]
        public void TestXmlBuild_Basic_Path()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/Basic.xml");
            Debug.WriteLine(conf.ToString());

            JbAssert.Equal(conf.Find("Item1").Path, "/Basic/Function1");
            JbAssert.Equal(conf.Find("Item2").Path, "/Basic/Function1");
            JbAssert.Equal(conf.Find("Item3").Path, "/Basic/Function2");
            JbAssert.Equal(conf.Find("Item4").Path, "/Basic/Function2");
            JbAssert.Equal(conf.Find("Item5").Path, "/Basic");

            conf["Item1"] = "Value5";
            JbAssert.Equal(conf.Find("Item1").Path, "/Basic/Function1");
        }
        [TestMethod]
        public void TestXmlBuild_BasicNoExist()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/Basic.xml");
            Assert.ThrowsException<Exception>(() => { var result = conf["NonExisting"]; });
        }
        [TestMethod]
        public void TestXmlBuild_SameItemName()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/SameItemName.xml");

            Debug.WriteLine(conf.ToString());
            JbAssert.Equal(conf[@"Function1/Item1"], "Value1");
            JbAssert.Equal(conf[@"Function2/Item1"], "Func2-1");
            JbAssert.Equal(conf[@"Function3/Item1"], "Func3-1");
        }
        [TestMethod]
        public void TestXmlBuild_Tag()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/Tag.xml");
            Debug.WriteLine(conf.ToString());
            JbAssert.Equal(conf[@"Default:Func/Item1"], "0");
            JbAssert.Equal(conf[@"Tag1:Func/Item1"], "1.1");
            JbAssert.Equal(conf[@"Tag2:Func/Item1"], "2.1");
        }
        [TestMethod]
        public void TestXmlBuild_Tag_FindItem()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/Tag.xml");
            JbAssert.Equal(conf[@"Default:Item1"], "0");
            JbAssert.Equal(conf[@"Tag1:Item1"], "1.1");
            JbAssert.Equal(conf[@"Tag2:Item1"], "2.1");
        }
        [TestMethod]
        public void TestXmlBuild_MultiTag()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/MultiTag.xml");
            Debug.WriteLine(conf.ToString());

            JbAssert.Equal(conf[@"T1.1&T1.1.1:Item1"], "1.1.1.1");
            JbAssert.Equal(conf[@"T1.1&T1.1.1:Item2"], "1.1.1.2");
            JbAssert.Equal(conf[@"T1.1&T1.1.2:Item1"], "1.1.2.1");
            JbAssert.Equal(conf[@"T1.1&T1.1.2:Item2"], "1.1.2.2");

            JbAssert.Equal(conf[@"T1.2&T1.2.1:Item1"], "1.2.1.1");
            JbAssert.Equal(conf[@"T1.2&T1.2.1:Item2"], "1.2.1.2");
            JbAssert.Equal(conf[@"T1.2&T1.2.2:Item1"], "1.2.2.1");
            JbAssert.Equal(conf[@"T1.2&T1.2.2:Item2"], "1.2.2.2");
        }
        [TestMethod]
        public void TestXmlBuild_MultiTagWithDefault()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/MultiTagWithDefault.xml");
            Debug.WriteLine(conf.ToString());

            JbAssert.Equal(conf[@"T1.1:Item1"], "1.1.1.1");
            JbAssert.Equal(conf[@"T1.2:Item1"], "1.2.2.1");
            JbAssert.Equal(conf[@"T1.2.1:Item1"], "1.2.1.3");
        }
        [TestMethod]
        public void TestXmlBuild_MultiLevel()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/MultiLevel.xml");

            JbAssert.Equal(conf["Item1"], "1.1");
            JbAssert.Equal(conf[@"Level1/Item1"], "1.1");
            JbAssert.Equal(conf[@"MultiLevel/Level1/Item1"], "1.1");

            JbAssert.Equal(conf["Item2"], "2.1");
            JbAssert.Equal(conf[@"Level2/Item2"], "2.1");
            JbAssert.Equal(conf[@"MultiLevel/Level1/Level2/Item2"], "2.1");
            JbAssert.Equal(conf["Item22"], "2.2");

            JbAssert.Equal(conf["Item3"], "3.1");
            JbAssert.Equal(conf[@"Level3/Item3"], "3.1");
            JbAssert.Equal(conf[@"MultiLevel/Level1/Level2/Level3/Item3"], "3.1");
        }
        [TestMethod]
        public void TestXmlBuild_MultiLevel_SameItemName()
        {
            ConfTree conf = XmlBuilder.Generate($@"{GlobalVariables.SamplePath}/MultiLevel.xml");
            Debug.Write(conf);

            JbAssert.Equal(conf[@"Level1/Item"], "1.0");
            JbAssert.Equal(conf[@"Level2/Item"], "2.0");
            JbAssert.Equal(conf[@"Level3/Item"], "3.0");
            JbAssert.Equal(conf[@"Level4/Item"], "4.0");
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