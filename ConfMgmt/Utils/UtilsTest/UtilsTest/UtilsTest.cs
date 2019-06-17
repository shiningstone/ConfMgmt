using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using System.Collections.Generic;
using BiView;

using Feedlitech;
using System.Diagnostics;
using System.Threading;

namespace UtilsTest
{
    [TestClass]
    public class Utils_Test
    {
        private static void AssertArray(int[] a, int[] b)
        {
            if (a.Length != b.Length)
            {
                throw new Exception("");
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    throw new Exception("");
                }
            }
        }
        public static void AssertString(string s1, string s2)
        {
            if (s1 != s2)
            {
                throw new Exception("");
            }
        }
        [TestMethod]
        public void DivideByStepTest()
        {
            int[] a;

            a = Calc.DivideByStep(0, 0, 2);
            AssertArray(a, new int[0]);
            a = Calc.DivideByStep(0, 1, 2);
            AssertArray(a, new int[] { 1 });
            a = Calc.DivideByStep(0, 2, 2);
            AssertArray(a, new int[] { 2 });
            a = Calc.DivideByStep(0, 3, 2);
            AssertArray(a, new int[] { 2, 3 });
            a = Calc.DivideByStep(0, 4, 2);
            AssertArray(a, new int[] { 2, 4 });
            a = Calc.DivideByStep(0, 25089, 5000);
            AssertArray(a, new int[] { 5000, 10000, 15000, 20000, 25000, 25089 });

            a = Calc.DivideByStep(4, 0, 2);
            AssertArray(a, new int[] { 2, 0 });
            a = Calc.DivideByStep(3, 0, 2);
            AssertArray(a, new int[] { 1, 0 });
            a = Calc.DivideByStep(2, 0, 2);
            AssertArray(a, new int[] { 0 });
            a = Calc.DivideByStep(1, 0, 2);
            AssertArray(a, new int[] { 0 });
            a = Calc.DivideByStep(0, 0, 2);
            AssertArray(a, new int[0]);
        }
        [TestMethod]
        public void AddPostfixTest()
        {
            string filePath = @"E:\test dir\testFile.txt";
            Assert.IsTrue(Calc.AddPostfix(filePath, "1") == @"E:\test dir\testFile1.txt");
        }

        [TestMethod]
        public void ToHexStringTest()
        {
            byte[] a = new byte[] { 0 };
            Assert.IsTrue(Calc.ToHexString(a) == "00 ");

            a = new byte[] { 0, 1 };
            Assert.IsTrue(Calc.ToHexString(a) == "00 01 ");

            a = new byte[] { 10, 11 };
            Assert.IsTrue(Calc.ToHexString(a) == "0A 0B ");
        }

        [TestMethod]
        public void StrCombineTest()
        {
            string result = Utils.Calc.Combine("DUT01", "DUT02");
            Assert.IsTrue(result == "DUT01-02");

            result = Utils.Calc.Combine("DUT01", "DUT01");
            Assert.IsTrue(result == "DUT01");

            result = Utils.Calc.Combine("DUT01", "DUT03");
            Assert.IsTrue(result == "DUT01,03");

            result = Utils.Calc.Combine("DUT01-02", "DUT03");
            Assert.IsTrue(result == "DUT01-03");
        }
        [TestMethod]
        public void SqlCombineTest()
        {
            List<string> filters = new List<string>();
            filters.Add("Warn");
            string result = Utils.Calc.Combine(filters, "Level", "or");
            Assert.IsTrue(result == "Level = 'Warn'");

            filters.Add("Error");
            result = Utils.Calc.Combine(filters, "Level", "or");
            Assert.IsTrue(result == "Level = 'Warn' or Level = 'Error'");
        }

        [TestMethod]
        public void SqlValueStringTest()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "0"),
            };
            string result = Utils.Calc.SqlValueString(data);
            Assert.IsTrue(result == "(a) VALUES ('0')");

            data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("a", "0"),
                new KeyValuePair<string, string>("b", "1"),
            };
            result = Utils.Calc.SqlValueString(data);
            Assert.IsTrue(result == "(a,b) VALUES ('0','1')");
        }
        [TestMethod]
        public void IntListTest()
        {
            List<int> ints = new List<int>();
            for (int i = 0; i < 40; i++)
            {
                ints.Add(i);
            }

            string result = Utils.Calc.IntListToString(ints, 40);
            Assert.IsTrue(result ==
                "P,P,P,P,P,P,P,P,P,P," +
                "P,P,P,P,P,P,P,P,P,P," +
                "P,P,P,P,P,P,P,P,P,P," +
                "P,P,P,P,P,P,P,P,P,P");
            ints = Utils.Calc.StringToIntList(result, "F");
            Assert.IsTrue(ints.Count == 0);

            ints.Clear();
            for (int i = 0; i < 40; i++)
            {
                if (i != 0)
                {
                    ints.Add(i);
                }
            }
            result = Utils.Calc.IntListToString(ints, 40);
            Assert.IsTrue(result ==
                          "F,P,P,P,P,P,P,P,P,P," +
                          "P,P,P,P,P,P,P,P,P,P," +
                          "P,P,P,P,P,P,P,P,P,P," +
                          "P,P,P,P,P,P,P,P,P,P");
            ints = Utils.Calc.StringToIntList(result, "F");
            Assert.IsTrue(ints.Count == 1);
            Assert.IsTrue(ints.Contains(0));

            ints.Clear();
            for (int i = 0; i < 40; i++)
            {
                if (i != 39)
                {
                    ints.Add(i);
                }
            }
            result = Utils.Calc.IntListToString(ints, 40);
            Assert.IsTrue(result ==
                          "P,P,P,P,P,P,P,P,P,P," +
                          "P,P,P,P,P,P,P,P,P,P," +
                          "P,P,P,P,P,P,P,P,P,P," +
                          "P,P,P,P,P,P,P,P,P,F");
            ints = Utils.Calc.StringToIntList(result, "F");
            Assert.IsTrue(ints.Count == 1);
            Assert.IsTrue(ints.Contains(39));

            ints.Clear();
            for (int i = 0; i < 40; i++)
            {
                if (i != 9)
                {
                    ints.Add(i);
                }
            }
            result = Utils.Calc.IntListToString(ints, 40);
            Assert.IsTrue(result ==
                          "P,P,P,P,P,P,P,P,P,F," +
                          "P,P,P,P,P,P,P,P,P,P," +
                          "P,P,P,P,P,P,P,P,P,P," +
                          "P,P,P,P,P,P,P,P,P,P");
            ints = Utils.Calc.StringToIntList(result, "F");
            Assert.IsTrue(ints.Count == 1);
            Assert.IsTrue(ints.Contains(9));

            ints.Clear();
            for (int i = 0; i < 40; i++)
            {
                if (i != 19 && i != 29)
                {
                    ints.Add(i);
                }
            }
            result = Utils.Calc.IntListToString(ints, 40);
            Assert.IsTrue(result ==
                          "P,P,P,P,P,P,P,P,P,P," +
                          "P,P,P,P,P,P,P,P,P,F," +
                          "P,P,P,P,P,P,P,P,P,F," +
                          "P,P,P,P,P,P,P,P,P,P");
            ints = Utils.Calc.StringToIntList(result, "F");
            Assert.IsTrue(ints.Count == 2);
            Assert.IsTrue(ints.Contains(19));
            Assert.IsTrue(ints.Contains(29));
        }

        [TestMethod]
        public void VersionNewerTest()
        {
            Assert.IsFalse(Calc.VersionNewer("0.0.1", "0.0.1"));
            Assert.IsTrue(Calc.VersionNewer("0.0.2", "0.0.1"));
            Assert.IsTrue(Calc.VersionNewer("0.1.0", "0.0.2"));
            Assert.IsFalse(Calc.VersionNewer("0.0.0.1", "0.0.1"));
        }
        [TestMethod]
        public void LiscenseTest()
        {
            bool result = Liscense.IsExpire(DateTime.Now);
            Assert.IsTrue(!result);

            result = Liscense.IsExpire(DateTime.Now.AddDays(20));
            Assert.IsTrue(!result);

            result = Liscense.IsExpire(DateTime.Now.AddDays(31));
            Assert.IsTrue(result);

            Liscense.Reset("");
        }
        [TestMethod]
        public void RangesTest()
        {
            var result = Calc.GetRanges("0A:4V;4A:4.5V;5A:5V;6A:6V");
            Assert.IsTrue(result.Count == 4);

            double value = 3;
            for (int i = result.Count - 1; i >= 0; i--)
            {
                if (value > result[i].Key)
                {
                    break;
                }
            }
        }
        [TestMethod]
        public void TestBytesTransfer()
        {
            var result = System.Text.Encoding.Default.GetBytes("ABCD");
            byte[] vals = new byte[16];
            for (int i = 0; i < result.Length; i++)
            {
                vals[i] = result[i];
            }
            var str = System.Text.Encoding.Default.GetString(vals);
        }
        [TestMethod]
        public void TestConvertToInt32()
        {
            var Dac = Convert.ToInt32($"0xEB58", 16);
            var Dac2 = Convert.ToInt32($"0xffffe2b4", 16);
            Assert.IsTrue(Dac2 == -7500);
        }

        [TestMethod]
        public void TestExtraCorrection()
        {
            //纠正
            Assert.IsTrue(ExtraCorrection(98, 100) == 99.6);
            Assert.IsTrue(ExtraCorrection(99, 100) == 99.8);
            Assert.IsTrue(ExtraCorrection(100, 100) == 100);
            Assert.IsTrue(ExtraCorrection(101, 100) == 100.2);
            Assert.IsTrue(ExtraCorrection(102, 100) == 100.4);

            Assert.IsTrue(ExtraCorrection(97.9, 100) == 97.9);
            Assert.IsTrue(ExtraCorrection(102.1, 100) == 102.1);
        }
        private double ExtraCorrection(double readVal, double setVal)
        {
            double delta = setVal - readVal;

            if (Math.Abs(delta) / setVal <= 0.02)
            {
                return setVal - delta / 5;
            }

            return readVal;
        }
        [TestMethod]
        public void TestCalibrateDataSerialize()
        {
            CalibrateData a = new CalibrateData();
            XmlSerializer.Save(@"d:/a.xml", a);
            XmlSerializer.Load(@"d:/a.xml", out a);

            CalibrateData b = new CalibrateData();
            b.OutputValue = 100;
            XmlSerializer.Save(@"d:/b.xml", b);
            XmlSerializer.Load(@"d:/b.xml", out b);
        }
        public void TestCorrectVoltage()
        {
            double result;
            result = CorrectVoltage(1, 500);
            Thread.Sleep(50);
            result = CorrectVoltage(1, 500);
            Thread.Sleep(50);
            result = CorrectVoltage(1, 500);
            Thread.Sleep(50);
            result = CorrectVoltage(2, 500);
            Thread.Sleep(50);
            result = CorrectVoltage(2, 500);
            Thread.Sleep(50);
            result = CorrectVoltage(2, 500);
            Thread.Sleep(50);
        }
        private static double[] VoltageOffset = new double[] { -0.5, -0.45 };
        private static double VoltageSupplement = 0.0015;
        private static double Resistor = 10;
        private static double CorrectVoltage(int chnl, double current)
        {
            Random rd = new Random();
            double percentage = (double)rd.Next(1, 101) / 100;

            double reference = current * Resistor + VoltageOffset[chnl - 1] * 1000;
            double result = reference * (1 - VoltageSupplement + 2 * VoltageSupplement * percentage);

            Debug.WriteLine($"Chnl: {chnl}, Current: {current}, Reference: {reference}, Percentage: {VoltageSupplement}, Random: {percentage}, Result: {result}");
            return result;
        }
    }
    [TestClass]
    public class CmdRecord_Test
    {
        [TestMethod]
        public void TestCmdRecord_IgnoreDuplicateCmd()
        {
            CmdRecord aRecord = new CmdRecord() { CmdId = "TestCmd", Time = DateTime.Now, Value = "" };

            Assert.IsTrue(aRecord.IsNeedReSend("A"));
            Assert.IsTrue(!aRecord.IsNeedReSend(""));
        }
        [TestMethod]
        public void TestCmdRecord_ResendDuplicateCmd()
        {
            CmdRecord aRecord = new CmdRecord() { CmdId = "TestCmd", Time = DateTime.Now, Value = "" };

            aRecord.Time = DateTime.Now.Subtract(new TimeSpan(0, 0, 58));
            Assert.IsTrue(!aRecord.IsNeedReSend(""));

            aRecord.Time = DateTime.Now.Subtract(new TimeSpan(0, 0, 61));
            Assert.IsTrue(aRecord.IsNeedReSend(""));

            aRecord.Update("");
            Assert.IsTrue(!aRecord.IsNeedReSend(""));
        }
        [TestMethod]
        public void TestCmdController()
        {
            CmdController aController = new CmdController();
            CmdRecord aRecord = new CmdRecord() { CmdId = "TestCmd", Time = DateTime.Now, Value = "" };
            Func<string, ErrInfo> aCmd = TestSetOk;
            aController.Add(aRecord, aCmd);

            TestSetOk("");
        }
        private ErrInfo TestSetOk(string setVal)
        {
            return new ErrInfo(ErrCode.Ok);
        }
        private ErrInfo TestSetFail(string setVal)
        {
            return new ErrInfo(ErrCode.ErrorFormat);
        }
    }
    [TestClass]
    public class Linear_Test
    {
        [TestMethod]
        public void TestLinearFit()
        {
            double[] x = { 7680, 9216, 10752, 12288, 13824, 15360, 16896, 18432, 19968, 21504 };
            double[] y = { -6.8408108, -6.42208962, -6.00329227, -5.58466183, -5.16596443, -4.74735206, -4.32857319, -3.9099264, -3.49120335, -3.07256209 };
            double k, b;
            Linear.Fit(x, y, out k, out b);
            Linear.Fit(y, x, out k, out b);
        }
        [TestMethod]
        public void TestGetScale()
        {
            var result = GetScale(2700.123);
            result = GetScale(1999.123);
            result = GetScale(1100.123);
            result = GetScale(980.123);
            result = GetScale(340.123);
            result = GetScale(89.123);
            result = GetScale(32.123);
            result = GetScale(12.123);
            result = GetScale(8.123);
            result = GetScale(1.123);
        }
        private static double GetScale(double value)
        {
            if (value < 0.001)
            {
                return 0.001;
            }
            else if (value < 0.01)
            {
                return 0.01;
            }
            else if (value < 0.1)
            {
                return 0.1;
            }
            else if (value < 1)
            {
                return 1;
            }
            else if (value < 10)
            {
                return (int)value + 1;
            }
            else if (value < 100)
            {
                return ((int)(value / 10) + 1) * 10;
            }
            else if (value < 1000)
            {
                return ((int)(value / 100) + 1) * 100;
            }
            else if (value < 10000)
            {
                return ((int)(value / 1000) + 1) * 1000;
            }
            else
            {
                return value;
            }
        }
        [TestMethod]
        public void TestComplement()
        {
            int result = Calc.Complement16b(0xffff);
            Assert.IsTrue(result == -1);

            result = Calc.Complement16b(0xfffe);
            Assert.IsTrue(result == -2);

            result = Calc.Complement16b(0x8001);
            Assert.IsTrue(result == -32767);

            result = Calc.Complement16b(0x8000);
            Assert.IsTrue(result == 0);

            result = Calc.Complement16b(0x7fff);
            Assert.IsTrue(result == 32767);

            result = Calc.Complement16b(1);
            Assert.IsTrue(result == 1);

            result = Calc.Complement16b(0);
            Assert.IsTrue(result == 0);

            ////////////////////////////////////////////////////
            result = Calc.Complement16b(Calc.ConvertToShort(0xff, 0xff));
            Assert.IsTrue(result == -1);
        }
        private int AdcConvert(int adc)
        {
            if (adc > 32767)
            {
                return 65535 - adc;
            }
            else
            {
                return -1 * adc;
            }
        }
        [TestMethod]
        public void TestFLT804A00Adc()
        {
            byte byte0 = 0xff;
            byte byte1 = 0xff;
            var result = AdcConvert(Calc.ConvertToShort(byte0, byte1));
            Debug.WriteLine($"Hex: 0x{byte0:X2}{byte1:X2}, Adc:{result}");
            Assert.IsTrue(result == 0);

            byte0 = 0xff;
            byte1 = 0xfe;
            result = AdcConvert(Calc.ConvertToShort(byte0, byte1));
            Debug.WriteLine($"Hex: 0x{byte0:X2}{byte1:X2}, Adc:{result}");
            Assert.IsTrue(result == 1);

            byte0 = 0x80;
            byte1 = 0x01;
            result = AdcConvert(Calc.ConvertToShort(byte0, byte1));
            Debug.WriteLine($"Hex: 0x{byte0:X2}{byte1:X2}, Adc:{result}");
            Assert.IsTrue(result == 32766);

            byte0 = 0x80;
            byte1 = 0x00;
            result = AdcConvert(Calc.ConvertToShort(byte0, byte1));
            Debug.WriteLine($"Hex: 0x{byte0:X2}{byte1:X2}, Adc:{result}");
            Assert.IsTrue(result == 32767);

            byte0 = 0x7f;
            byte1 = 0xff;
            result = AdcConvert(Calc.ConvertToShort(byte0, byte1));
            Debug.WriteLine($"Hex: 0x{byte0:X2}{byte1:X2}, Adc:{result}");
            Assert.IsTrue(result == -32767);

            byte0 = 0x7f;
            byte1 = 0xfe;
            result = AdcConvert(Calc.ConvertToShort(byte0, byte1));
            Debug.WriteLine($"Hex: 0x{byte0:X2}{byte1:X2}, Adc:{result}");
            Assert.IsTrue(result == -32766);

            byte0 = 0x00;
            byte1 = 0x01;
            result = AdcConvert(Calc.ConvertToShort(byte0, byte1));
            Debug.WriteLine($"Hex: 0x{byte0:X2}{byte1:X2}, Adc:{result}");
            Assert.IsTrue(result == -1);

            byte0 = 0x00;
            byte1 = 0x00;
            result = AdcConvert(Calc.ConvertToShort(byte0, byte1));
            Debug.WriteLine($"Hex: 0x{byte0:X2}{byte1:X2}, Adc:{result}");
            Assert.IsTrue(result == 0);
        }
    }
}
