using JbConf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Utils;

namespace TestConfMgmt
{
    [TestClass]
    public class TestConfTreeFind
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            GlobalVar.Initialize(ConfMgmt);
        }

        private ConfMgmt ConfMgmt = new ConfMgmt();
        [TestMethod]
        public void TestConfTreeFind_Conflict()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/MultiLevel.xml");
            GlobalVar.Log.Debug(conf.ShowAll());

            JbAssert.Equal(conf[@"Level1/Item"], "1.0");
            JbAssert.Equal(conf[@"Level1/Item1"], "1.1");
            JbAssert.Equal(conf[@"Level2/Item"], "2.0");
            JbAssert.Equal(conf[@"Level2/Item2"], "2.1");
            JbAssert.Equal(conf[@"Level2/Item22"], "2.2");
            JbAssert.Equal(conf[@"Level1/Level2/Item"], "2.0");
            JbAssert.Equal(conf[@"Level1/Level2/Item2"], "2.1");
            JbAssert.Equal(conf[@"Level1/Level2/Item22"], "2.2");
            JbAssert.Equal(conf[@"Level3/Item"], "3.0");
            JbAssert.Equal(conf[@"Level3/Item3"], "3.1");
            JbAssert.Equal(conf[@"Level2/Level3/Item"], "3.0");
            JbAssert.Equal(conf[@"Level2/Level3/Item3"], "3.1");
            JbAssert.Equal(conf[@"Level1/Level2/Level3/Item"], "3.0");
            JbAssert.Equal(conf[@"Level1/Level2/Level3/Item3"], "3.1");
            JbAssert.Equal(conf[@"Level4/Item"], "4.0");

            JbAssert.Equal(conf.FindStrict("Item", null, false).Value, "1.0");
            JbAssert.Equal(conf.FindStrict(@"Level1/Item").Value, "1.0");
            Assert.ThrowsException<Exception>(() => { conf.FindStrict("Item"); });
        }
    }
}
