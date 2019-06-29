using System.IO;
using JbConf;

namespace TestConfMgmt
{
    internal class GlobalVar
    {
        public static string SamplePath = @"D:/ConfMgmt/ConfMgmt/TestSamples";
        public static string ResultPath = $@"{SamplePath}/Result";
        public static string RealConfPath = @"D:/TestSystem/Configurations/";
        static GlobalVar()
        {
            if (!Directory.Exists(SamplePath))
            {
                Directory.CreateDirectory(SamplePath);
            }
            if (!Directory.Exists(ResultPath))
            {
                Directory.CreateDirectory(ResultPath);
            }
        }
        public static void Initialize()
        {
            ConfMgmt.Clear();
        }
    }
}
