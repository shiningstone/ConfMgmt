﻿using JbConf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using Utils;

namespace TestConfMgmt
{
    [TestClass]
    public class TestDictionaryBuild
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            GlobalVar.Initialize();
        }

        public TestDictionaryBuild()
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
        public void TestDictionaryBuild_Basic()
        {
            ConfTree conf = Builder.Code.Generate(new Dictionary<string, string> {
                { "Item1", "Value1" },
                { "Item2", "Value2" },
            }, "DictConf1");
            Debug.WriteLine(conf.ToString());

            Assert.IsTrue(conf["Item1"] == "Value1");
            Assert.IsTrue(conf["Item2"] == "Value2");
            Builder.Xml.Save(conf, $@"{GlobalVar.ResultPath}/DictConf1.xml");
        }
        [TestMethod]
        public void TestDictionaryBuild_Basic_Path()
        {
            ConfTree conf = Builder.Code.Generate(new Dictionary<string, string> {
                { "Item1", "Value1" },
                { "Item2", "Value2" },
            }, "DictConf1");
            new Logger().Debug($"{conf.ToString()}");

            JbAssert.Equal(conf.Find("Item1").Path, "/Root/DictConf1");
        }
    }
}
