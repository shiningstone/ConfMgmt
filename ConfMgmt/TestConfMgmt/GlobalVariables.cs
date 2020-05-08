using System.IO;
using JbConf;
using Utils;

namespace TestConfMgmt
{
    internal class GlobalVar
    {
        public static string SamplePath = @"D:\ConfMgmt\ConfMgmt\TestSamples";
        public static string ResultPath = $@"TestResults";
        public static string RealConfPath = @"D:\DailyWork\20200213-Test3D重构\TestSystemConfigs";
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

        public static void Initialize(ConfMgmt conf)
        {
            conf.Clear();
        }
        public static Logger Log = new Logger("Tester");
    }
}
