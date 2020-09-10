using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class Spec
    {
        private double UBound = double.NaN;
        private double LBound = double.NaN;

        public Spec(double[] values)
        {
            LBound = values[0];
            UBound = values[1];
        }

        public ErrInfo IsViolate(double value)
        {
            if (double.IsNaN(value) || (!double.IsNaN(LBound) && value < LBound) || (!double.IsNaN(UBound) && value > UBound))
            {
                return new ErrInfo(ErrCode.SpecViolation, $"{value} out of spec({LBound},{UBound})");
            }

            return new ErrInfo(ErrCode.Ok);
        }
    }
}
