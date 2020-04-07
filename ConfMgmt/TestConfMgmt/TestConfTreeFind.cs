using JbConf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace TestConfMgmt
{
    [TestClass]
    public class TestConfTreeFind
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            GlobalVar.Initialize();
        }

        [TestMethod]
        public void TestConfTreeFind_AddItemToTree()
        {
            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/MultiLevel.xml");
            GlobalVar.Log.Debug(conf.ShowAll());

            var result = conf.FindStrict("Item");
        }
    }
}
