using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using JbConf;

namespace TestConfMgmt
{
    [TestClass]
    public class TestRealConfig
    {
        private ConfMgmt ConfMgmt = new ConfMgmt();
        [TestMethod]
        public void TestRealConfig_Generate()
        {
            ConfMgmt.Generate(GlobalVar.RealConfPath);
        }
        [TestMethod]
        public void TestRealConfig_SaveWithFiles()
        {
            ConfMgmt.Generate(GlobalVar.RealConfPath);
            var orig = ConfMgmt.Root;

            FileOp.RmDir(GlobalVar.ResultPath);
            FileOp.CopyDir(GlobalVar.RealConfPath, GlobalVar.ResultPath);

            ConfMgmt.Clear();
            ConfMgmt.Generate(GlobalVar.ResultPath);
            ConfMgmt.Save();

            ConfMgmt.Generate(GlobalVar.ResultPath);
            var copy = ConfMgmt.Root;

            //foreach (var kv in orig)
            //{
            //    ConfTree tree = null;

            //    foreach (var kv2 in copy)
            //    {
            //        if (kv2.Key.Contains(Path.GetFileName(kv.Key)))
            //        {
            //            tree = copy[kv2.Key];
            //        }
            //    }

            //    Assert.IsTrue(kv.Value.Equals(tree));
            //}
        }
        [TestMethod]
        public void TestRealConfig_Modify()
        {
            ConfMgmt.Generate(GlobalVar.RealConfPath);
            var orig = ConfMgmt.Root;

            FileOp.RmDir(GlobalVar.ResultPath);
            FileOp.CopyDir(GlobalVar.RealConfPath, GlobalVar.ResultPath);

            ConfMgmt.Clear();
            ConfMgmt.Generate(GlobalVar.ResultPath);
            ConfMgmt.GetTree("BICalibration")["JMDM40_com"] = "";
            ConfMgmt.Save();

            ConfMgmt.Generate(GlobalVar.ResultPath);
            var copy = ConfMgmt.Root;
        }
        [TestMethod]
        public void TestRealConfig_CreateNew()
        {
            ConfMgmt.Generate(GlobalVar.RealConfPath);
            var orig = ConfMgmt.Root;

            FileOp.RmDir(GlobalVar.ResultPath);
            FileOp.CopyDir(GlobalVar.RealConfPath, GlobalVar.ResultPath);

            ConfMgmt.Clear();
            ConfMgmt.Generate(GlobalVar.ResultPath);
            var sanan = ConfMgmt.Clone("HUAXIN", "SANAN");
            sanan["CW/StartBias"] = "2";
            ConfMgmt.Save();

            ConfMgmt.Generate(GlobalVar.ResultPath);
            var copy = ConfMgmt.Root;
        }
    }
}
