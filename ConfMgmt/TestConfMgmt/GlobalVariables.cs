using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConfMgmt
{
    internal class GlobalVariables
    {
        public static string SamplePath = @"D:/ConfMgmt/ConfMgmt/TestConfMgmt/TestSamples";
        public static string ResultPath = $@"{SamplePath}/Result";
        public static string RealConfPath = @"D:/TestSystem/Configurations/";
        static GlobalVariables()
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
    }
}
