using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class BitsArray
    {
        public byte[] mMap = new byte[4];
        public void Set(int[] bits)
        {
            for (int i = 0; i < bits.Length; i++)
            {
                Set(bits[i]);
            }
        }
        public void Clr(int[] bits)
        {
            for (int i = 0; i < bits.Length; i++)
            {
                Clr(bits[i]);
            }
        }
        public void Set(int bit)
        {
            if (bit < 20)
            {
                mMap[3 - bit / 8] |= (byte)(1 << bit % 8);
            }
            else
            {
                throw new Exception("BitsArray : error range " + bit.ToString() + ", should between 0-19");
            }
        }
        public void Clr(int bit)
        {
            if (bit < 20)
            {
                mMap[3 - bit / 8] &= (byte)(~(1 << bit % 8));
            }
            else
            {
                throw new Exception("BitsArray : error range " + bit.ToString() + ", should between 0-19");
            }
        }
    }
    public class Calc
    {
        #region bits operations
        public static ushort Bit(int i)
        {
            return (ushort)(1 << i);
        }
        public static ushort Bits(int[] flags/*start from 1*/)
        {
            ushort value = 0;

            for (int i = 0; i < flags.Length; i++)
            {
                value |= Bit(flags[i] - 1);
            }

            return value;
        }
        public static byte HighByte(int value)
        {
            return (byte)(value >> 8);
        }
        public static byte LowByte(int value)
        {
            return (byte)(value & 0xff);
        }
        public static ushort ConvertToShort(byte high, byte low)
        {
            return (ushort)((high << 8) + low);
        }
        public static byte[] ConvertTo4Bytes(int value)
        {
            byte[] ret = new byte[4];
            ret[0] = (byte)((value >> 24) & 0xff);
            ret[1] = (byte)((value >> 16) & 0xff);
            ret[2] = (byte)((value >> 8) & 0xff);
            ret[3] = (byte)((value >> 0) & 0xff);
            return ret;
        }
        #endregion
        #region math operations
        public static double Sum(double[] datas)
        {
            double sum = 0;
            foreach (double data in datas)
            {
                sum += data;
            }
            return sum;
        }
        public static double[] LinearFit(double[] x, double[] y)
        {
            double[] k_b = new double[2];
            double sumX = Sum(x);
            double sumY = Sum(y);

            double sumXY = 0;
            double sumXX = 0;
            for (int i = 0; i < x.Length; i++)
            {
                sumXY += x[i] * y[i];
                sumXX += x[i] * x[i];
            }

            k_b[0] = (sumXY - sumX * sumY / x.Length) / (sumXX - sumX * sumX / x.Length);
            k_b[1] = sumY / x.Length - k_b[0] * sumX / x.Length;

            return k_b;
        }
        public static int Complement16b(int value)
        {
            if (value < 0x8000)
            {
                return value;
            }
            else
            {
                value = ~value;
                value &= 0x7fff;
                return ((value + 1) % 0x8000) * -1; 
            }
        }
        #endregion
        public static int[] DivideByStep(int start, int stop, int step)
        {
            int distance = Math.Abs(stop - start);
            int pointNum = distance / step;
            if (distance % step != 0)
            {
                pointNum = pointNum + 1;
            }

            bool isUp = stop > start;

            int[] results = new int[pointNum];
            int b = start;
            for (int i = 0; i < pointNum - 1; i++)
            {
                results[i] = b + (isUp ? step : -step);
                b = results[i];
            }
            if (pointNum > 0)
            {
                results[pointNum - 1] = stop;
            }

            return results;
        }
        public static string AddPostfix(string filePath, string postfix)
        {
            string path = Path.GetDirectoryName(filePath);
            string filename = Path.GetFileNameWithoutExtension(filePath);
            string fileExt = Path.GetExtension(filePath);
            
            if (string.IsNullOrEmpty(path))
            {
                return filename + postfix + fileExt;
            }
            else
            {
                return path + @"/" + filename + postfix + fileExt;
            }
        }
        public static string Combine(List<string> filters, string header, string combiner)
        {
            string ret = "";

            for (int i = 0; i < filters.Count; i++)
            {
                ret += string.Format("{0} = '{1}'", header, filters[i]);
                if (i < filters.Count - 1)
                {
                    ret += string.Format(" {0} ", combiner);
                }
            }

            return ret;
        }

        public static string SqlValueString(List<KeyValuePair<string, string>> datas)
        {
            string keystr = "";
            string valuestr = "";

            foreach (var data in datas)
            {
                keystr += data.Key + ",";
                valuestr += "'" + data.Value + "'" + ",";
            }

            if (datas.Count > 0)
            {
                keystr = keystr.Substring(0, keystr.Length - 1);
                valuestr = valuestr.Substring(0, valuestr.Length - 1);
            }

            return string.Format("({0}) VALUES ({1})", keystr, valuestr);
        }
        public static string Combine(string origValues, string newValue)
        {
            int origLast = Int32.Parse(origValues.Substring(origValues.Length - 2, 2));
            int newInt = Int32.Parse(newValue.Substring(newValue.Length - 2, 2));

            string prefix = origValues.Substring(0, origValues.Length - 2);
            if (newInt == origLast)
            {
                return origValues;
            }
            else if (newInt == origLast + 1)
            {
                bool isContinuous = origValues.Substring(origValues.Length - 3, 1) == "-";

                if (isContinuous)
                {
                    return prefix + newInt.ToString("D2");
                }
                else
                {
                    return origValues + "-" + newInt.ToString("D2");
                }
            }
            else
            {
                return origValues + "," + newInt.ToString("D2");
            }
        }

        public static byte[] HexStringToBytes(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return returnBytes;
        }
        public static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;

            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                    strB.Append(" ");
                }

                hexString = strB.ToString();
            }
            return hexString;
        }

        public static string IntListToString(List<int> ints, int max)
        {
            string location = "";
            for (int i = 0; i < max; i++)
            {
                var selected = ints.Where(x => x == i).ToList();
                if (selected.Count > 0)
                {
                    location += "P,";
                }
                else
                {
                    location += "F,";
                }
            }

            return location.Substring(0, location.Length - 1);
        }
        public static List<int> StringToIntList(string queue, string tag)
        {
            List<int> ints = new List<int>();

            string[] nums = queue.Split(',');
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] == tag)
                {
                    ints.Add(i);
                }
            }

            return ints;
        }
        public static List<string> ToList(string info)
        {
            var array = info.Split(new char[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var ret = new List<string>();
            foreach (var item in array)
            {
                ret.Add(item.Trim());
            }
            return ret;
        }
        public static bool VersionNewer(string s1, string s2)
        {
            string[] v1 = s1.Split('.');
            string[] v2 = s2.Split('.');

            for (int i = 0; i < v1.Length; i++)
            {
                if (Int32.Parse(v1[i]) > Int32.Parse(v2[i]))
                {
                    return true;
                }
                else if (Int32.Parse(v1[i]) < Int32.Parse(v2[i]))
                {
                    return false;
                }
            }

            return false;
        }

        public static string GetVersion(string path)
        {
            return FileVersionInfo.GetVersionInfo(path).ProductVersion.Split('+')[0];
        }

        public static List<KeyValuePair<double, double>> GetRanges(string content)
        {
            List<KeyValuePair<double, double>> result = new List<KeyValuePair<double, double>>();
            var info = content.Split(';');
            foreach (var cv in info)
            {
                var c = cv.Split(':')[0];
                var v = cv.Split(':')[1];
                result.Add(new KeyValuePair<double, double>(
                    double.Parse(c.Substring(0, c.Length - 1)),
                    double.Parse(v.Substring(0, v.Length - 1))));
            }

            return result;
        }
        public static byte[] SubArray(byte[] content, byte start, int len)
        {
            var frame = new List<byte>();
            int idx = -1;

            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == start && idx == -1)
                {
                    idx = 0;
                }

                if (idx >= 0)
                {
                    frame.Add(content[i]);
                    if (frame.Count == len)
                    {
                        break;
                    }
                }
            }

            return frame.ToArray();
        }
        private static Logger _log = new Logger("Utils");
    }
}
