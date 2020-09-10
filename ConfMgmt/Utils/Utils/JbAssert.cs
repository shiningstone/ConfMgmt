using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utils
{
    public class JbAssert
    {
        public static void WaitUntil(Func<bool> judge, int timeout = 1000)
        {
            int count = 0;
            while (count < timeout && !judge())
            {
                Thread.Sleep(50);
                count += 50;
            }
        }
        public static void Equal(string a, string b)
        {
            if (a != b)
            {
                throw new Exception($@"JbAssert.Equal({a}, {b})");
            }
        }
        private static string ExtractUrl(string url)
        {
            if (url.StartsWith(@"file:///"))
            {
                return url.Substring(@"file:///".Length);
            }

            return url;
        }
        public static void PathEqual(string a, string b)
        {
            if (a == b)
            {
                return;
            }
            else
            {
                var fa = new FileInfo(ExtractUrl(a));
                var fb = new FileInfo(ExtractUrl(b));
                if (fa.FullName != fb.FullName)
                {
                    throw new Exception($@"JbAssert.PathEqual({a}, {b})");
                }
            }
        }
    }
}
