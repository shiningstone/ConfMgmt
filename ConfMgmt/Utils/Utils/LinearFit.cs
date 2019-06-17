using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class Linear
    {
        public static void Fit(double[] x, double[] y, out double k, out double b)
        {
            double xsum = 0;
            double ysum = 0;
            double xysum = 0;
            double x2sum = 0;
            int m = x.Length;

            for (int i = 0; i < m; i++)
            {
                xsum = xsum + x[i];
                ysum = ysum + y[i];
                xysum = xysum + x[i] * y[i];
                x2sum = x2sum + x[i] * x[i];
            }

            k = (m * xysum - xsum * ysum) / (m * x2sum - xsum * xsum + 1e-10);
            b = (ysum - k * xsum) / m;

            return;
        }

        public static void Calc(double[] x, double k, double b, double[] y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                y[i] = k * x[i] + b;
            }
        }

        public static double Corrcoef(double[] d1, double[] d2)
        {
            double xy = 0, x = 0, y = 0, xsum = 0, ysum = 0;
            double corrc;
            int m = d1.Length;

            for (int i = 0; i < m; i++)
            {
                xsum += d1[i];
                ysum += d2[i];
            }

            for (int i = 0; i < m; i++)
            {
                x = x + (m * d1[i] - xsum) * (m * d1[i] - xsum);
                y = y + (m * d2[i] - ysum) * (m * d2[i] - ysum);
                xy = xy + (m * d1[i] - xsum) * (m * d2[i] - ysum);
            }

            corrc = Math.Abs(xy) / (Math.Sqrt(x) * Math.Sqrt(y));
            return corrc;
        }

        #region reference
        //Linear.Fit(xfit, yfit, out double slope, out double interp);
        //Linear.Calc(xfit, slope, interp, y);
        //Corrcoef = Linear.Corrcoef(yfit, y);
        #endregion
    }
}
