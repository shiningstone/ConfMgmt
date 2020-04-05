﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JbConf;
using System.Diagnostics;
using Utils;

namespace TestConfMgmt
{
    /// <summary>
    /// TestConfMgmt 的摘要说明
    /// </summary>
    [TestClass]
    public class TestConfMgmt
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            GlobalVar.Initialize();
        }
        public TestConfMgmt()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestConfMgmt_Save()
        {
            ConfMgmt.Generate($@"{GlobalVar.SamplePath}/ConfigFiles");
            ConfMgmt.Save($@"{GlobalVar.ResultPath}/Root.xml");
        }
        [TestMethod]
        public void TestConfMgmt_Modify()
        {
            ConfMgmt.Generate($@"{GlobalVar.SamplePath}/ConfigFiles");
            var conf = ConfMgmt.GetTree("SystemSetting");

            conf["DutsCount"] = "0";
            ConfMgmt.Save();

            conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/ConfigFiles/Configs/SystemSetting.xml");
            JbAssert.Equal(conf["DutsCount"], "0");

            //restore
            ConfMgmt.Clear();
            ConfMgmt.Generate($@"{GlobalVar.SamplePath}/ConfigFiles");
            conf = ConfMgmt.GetTree("SystemSetting");

            conf["DutsCount"] = "40";
            ConfMgmt.Save();

            conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/ConfigFiles/Configs/SystemSetting.xml");
            JbAssert.Equal(conf["DutsCount"], "40");
        }
        [TestMethod]
        public void TestConfMgmt_AddNewConf()
        {
            ConfMgmt.Generate($@"{GlobalVar.SamplePath}/ConfigFiles");
            var conf = ConfMgmt.GetTree("SystemSetting");
            Debug.WriteLine(conf.ToString());
            Assert.ThrowsException<Exception>(() => { var result = conf["new:DutsCount"]; });

            var newconf = conf.Find("System").Clone("new") as ConfTree;
            newconf["DutsCount"] = "20";
            Builder.Xml.Save(newconf as ConfTree);

            //restore
            ConfMgmt.Clear();
            ConfMgmt.Generate($@"{GlobalVar.SamplePath}/ConfigFiles");
            conf = ConfMgmt.GetTree("SystemSetting");
            JbAssert.Equal(conf["DutsCount"], "40");
            JbAssert.Equal(conf["new:DutsCount"], "20");

            conf.XmlDoc.Remove(conf.Find("DutsCount", new List<string>() { "new" }).Path);
            conf.XmlDoc.Save($@"{GlobalVar.SamplePath}/ConfigFiles/Configs/SystemSetting.xml");
        }
    }
}
