using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using Utils;
using JbConf;

namespace TestConfMgmt
{
    [TestClass]
    public class TestXmlBuild
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            GlobalVar.Initialize();
        }

        [TestMethod]
        public void TestBuild_Dictionary_Save()
        {
            ConfTree conf = Builder.Generate(new Dictionary<string, string> {
                { "Item1", "Value1" },
                { "Item2", "Value2" },
            }, "DictConf1");
            Debug.WriteLine(conf.ToString());

            Assert.IsTrue(conf["Item1"] == "Value1");
            Assert.IsTrue(conf["Item2"] == "Value2");

            JbAssert.Equal(conf.Find("Item1").Path, "/DictConf1");
            JbAssert.Equal(conf.Find("Item2").Path, "/DictConf1");

            Builder.Xml.Save(conf, $@"{GlobalVar.ResultPath}/DictConf1.xml");
        }

        [TestMethod]
        public void TestBuild_Xml_Build_Modify_Save()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/Basic.xml");
            Debug.WriteLine(conf.ToString());

            JbAssert.Equal(conf["Item1"], "Value1");
            JbAssert.Equal(conf["Item2"], "Value2");
            JbAssert.Equal(conf["Item3"], "Value3");
            JbAssert.Equal(conf["Item4"], "Value4");
            JbAssert.Equal(conf["Item5"], "Value5");

            conf["Item1"] = "Value5";
            JbAssert.Equal(conf["Item1"], "Value5");
            Builder.Xml.Save(conf, $@"{GlobalVar.ResultPath}/BasicResult.xml");
            //conf.Save();

            GlobalVar.Initialize();

            conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}/BasicResult.xml");
            JbAssert.Equal(conf["Item1"], "Value5");
            JbAssert.Equal(conf["Item2"], "Value2");
            JbAssert.Equal(conf["Item3"], "Value3");
            JbAssert.Equal(conf["Item4"], "Value4");
        }
        [TestMethod]
        public void TestBuild_Xml_CheckPath()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/Basic.xml");
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
        public void TestBuild_Xml_ModifyException()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/Basic.xml");
            Assert.ThrowsException<Exception>(() => { var result = conf["NonExisting"]; });
        }
        [TestMethod]
        public void TestBuild_Xml_SameItemName()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/SameItemName.xml");

            Debug.WriteLine(conf.ToString());
            JbAssert.Equal(conf[@"Function1/Item1"], "Value1");
            JbAssert.Equal(conf[@"Function2/Item1"], "Func2-1");
            JbAssert.Equal(conf[@"Function3/Item1"], "Func3-1");
        }
        [TestMethod]
        public void TestBuild_Xml_Tag()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/Tag.xml");
            Debug.WriteLine(conf.ToString());
            JbAssert.Equal(conf[@"Default:Func/Item1"], "0");
            JbAssert.Equal(conf[@"Tag1:Func/Item1"], "1.1");
            JbAssert.Equal(conf[@"Tag2:Func/Item1"], "2.1");
        }
        [TestMethod]
        public void TestBuild_Xml_Tag_FindItem()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/Tag.xml");
            JbAssert.Equal(conf[@"Default:Item1"], "0");
            JbAssert.Equal(conf[@"Tag1:Item1"], "1.1");
            JbAssert.Equal(conf[@"Tag2:Item1"], "2.1");
        }
        [TestMethod]
        public void TestBuild_Xml_MultiTag()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/MultiTag.xml");
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
        public void TestBuild_Xml_MultiTagWithDefault()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/MultiTagWithDefault.xml");
            Debug.WriteLine(conf.ToString());

            JbAssert.Equal(conf[@"T1.1:Item1"], "1.1.1.1");
            JbAssert.Equal(conf[@"T1.2:Item1"], "1.2.2.1");
            JbAssert.Equal(conf[@"T1.2.1:Item1"], "1.2.1.3");
        }
        [TestMethod]
        public void TestBuild_Xml_MultiLevel()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/MultiLevel.xml");

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
        public void TestBuild_Xml_MultiLevel_SameItemName()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/MultiLevel.xml");
            Debug.Write(conf);

            JbAssert.Equal(conf[@"Level1/Item"], "1.0");
            JbAssert.Equal(conf[@"Level2/Item"], "2.0");
            JbAssert.Equal(conf[@"Level3/Item"], "3.0");
            JbAssert.Equal(conf[@"Level4/Item"], "4.0");
        }
        [TestMethod]
        public void TestBuild_Xml_Attribute()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/Attribute.xml");
            Debug.WriteLine(conf.ToString());

            JbAssert.Equal(conf[@"CW/Ith.Min"], "0");
            JbAssert.Equal(conf[@"CW/Ith.Max"], "10000");
        }
        [TestMethod]
        public void TestBuild_Xml_Attribute_Save()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/Attribute.xml");
            Debug.WriteLine(conf.ToString());
            Builder.Xml.Save(conf, $@"{GlobalVar.ResultPath}/Attribute.xml");
        }
        [TestMethod]
        public void TestBuild_Xml_Attribute_Modify()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/Attribute.xml");
            JbAssert.Equal(conf[@"CW/Ith.Min"], "0");

            Debug.WriteLine(conf.ToString());
            conf[@"Specs/Spec/CW/Ith.Min"] = "1";
            Builder.Xml.Save(conf, $@"{GlobalVar.ResultPath}/Attribute.xml");

            conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}/Attribute.xml");
            JbAssert.Equal(conf[@"CW/Ith.Min"], "1");
        }
    }
}