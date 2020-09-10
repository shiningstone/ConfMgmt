using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class UnitTransfer
    {
        public static double MsToSec(this double value)
        {
            return value / 1000;
        }
        public static double SecToMs(this double value)
        {
            return value * 1000;
        }
    }
}
