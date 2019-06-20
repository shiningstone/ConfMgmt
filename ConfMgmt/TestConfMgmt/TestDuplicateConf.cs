using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JbConf;

namespace TestConfMgmt
{
    /// <summary>
    /// TestDuplicateConf 的摘要说明
    /// </summary>
    [TestClass]
    public class TestDuplicateConf
    {
        public TestDuplicateConf()
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
        public void TestDuplicate_GlobalDict()
        {
            DictionaryBuilder.Generate(new Dictionary<string, string>
            {
                { "Item1", "V1" },
                { "Item2", "V2" },
            }, "Basic");

            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item1") == "V1");
            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item2") == "V2");
        }
        [TestMethod]
        public void TestDuplicate_GlobalXml()
        {
            XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\Basic.xml");

            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item1") == "Value1");
            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item2") == "Value2");
        }
        [TestMethod]
        public void TestDuplicate_XmlItemsPriorToDict()
        {
            DictionaryBuilder.Generate(new Dictionary<string, string>
            {
                { "Item1", "V1" },
                { "Item2", "V2" },
            }, "Basic");
            XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\Basic.xml");

            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item1") == "Value1");
            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item2") == "Value2");
        }
        [TestMethod]
        public void TestDuplicate_XmlItemsPriorToDict2()
        {
            XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\Basic.xml");
            DictionaryBuilder.Generate(new Dictionary<string, string>
            {
                { "Item1", "V1" },
                { "Item2", "V2" },
            }, "Basic");

            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item1") == "Value1");
            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item2") == "Value2");
        }
        [TestMethod]
        public void TestDuplicate_DictItemAddToXml()
        {
            XmlBuilder.Generate($@"{GlobalVariables.SamplePath}\Basic.xml");
            DictionaryBuilder.Generate(new Dictionary<string, string>
            {
                { "Item6", "V6" },
            }, "Basic");

            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item1") == "Value1");
            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item2") == "Value2");
            Assert.IsTrue(ConfMgmt.GetItem(@"Basic\Item6") == "V6");
        }
    }
}
