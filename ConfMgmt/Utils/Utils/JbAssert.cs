using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Utils
{
    public class JbAssert
    {
        public static void Equal(string a, string b)
        {
            if (a != b)
            {
                throw new Exception($@"JbAssert.Equal({a}, {b})");
            }
        }
    }
}
