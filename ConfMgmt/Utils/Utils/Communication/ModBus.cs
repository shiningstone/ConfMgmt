using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Utils
{
    public class ModBus
    {
        static readonly int SHOW_LEN = 20;
        private static void DbgShow(string op, int[] buf)
        {
            if (buf.Length < SHOW_LEN)
            {
                _log.Debug($"{op} : {string.Join(",", buf.ToList().Select(x => $"0x{x:X4}"))}");
            }
            else
            {
                _log.Debug($"{op} : {string.Join(",", buf.ToList().Take(SHOW_LEN).Select(x => $"0x{x:X4}"))} ... {string.Join(",", buf.ToList().Skip(buf.Length - 4).Take(4).Select(x => $"0x{x:X4}"))}");
            }
        }

        private static Logger _log = new Logger("ModBus");

        public static int Delay = 0;
        public static bool Write(ComPort com, int devAddr, int startAddr, int[] values)
        {
            DbgShow($"{com.Name} Write(Dev:{devAddr}/Reg:0x{startAddr:X4}/Len:{values.Length})", values);

            List<byte> cmd = new List<byte>
            {
                (byte)devAddr, 0x10, Calc.HighByte(startAddr), Calc.LowByte(startAddr),
                Calc.HighByte(values.Length), Calc.LowByte(values.Length),
                (byte)(2 * values.Length),
            };
            foreach (var value in values)
            {
                cmd.Add(Calc.HighByte(value));
                cmd.Add(Calc.LowByte(value));
            }
            cmd.AddRange(CheckSum16.Calculate(cmd.ToArray()));

            byte[] response = com.Query(cmd.ToArray(), 200);
            if (response.Length != 8)
            {
                return false;
            }

            Thread.Sleep(Delay);
            return true;
        }
        public static bool Write(ComPort com, int devAddr, int regAddr, int value)
        {
            _log.Debug($"{com.Name} Write({devAddr}/0x{regAddr:X4}): 0x{value:X4}");

            List<byte> cmd = new List<byte>
            {
                (byte)devAddr, 0x10, Calc.HighByte(regAddr), Calc.LowByte(regAddr),
                0x00, 0x01,
                0x02,
                Calc.HighByte(value), Calc.LowByte(value)
            };
            cmd.AddRange(CheckSum16.Calculate(cmd.ToArray()));

            byte[] response = com.Query(cmd.ToArray(), 200);
            if (response.Length != 8)
            {
                return false;
            }

            Thread.Sleep(Delay);
            return true;
        }
        public static bool Read(ComPort com, int devAddr, int regAddr, int valueNum, out int[] values)
        {
            values = new int[valueNum];

            try
            {
                List<byte> cmd = new List<byte>
                {
                    (byte)devAddr, 0x03, Calc.HighByte(regAddr), Calc.LowByte(regAddr),
                    Calc.HighByte(valueNum), Calc.LowByte(valueNum)
                };
                cmd.AddRange(CheckSum16.Calculate(cmd.ToArray()));

                byte[] response = com.Query(cmd.ToArray(), 200);
                if (response.Length != 5 + 2 * valueNum)
                {
                    values = new int[0];
                    return false;
                }

                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = Calc.ConvertToShort(response[3 + 2 * i], response[4 + 2 * i]);
                }

                return true;
            }
            finally
            {
                DbgShow($"{com.Name} Read(Dev:{devAddr}/Reg:0x{regAddr:X4})", values);
            }
        }
        public static bool ReadO(ComPort com, int devAddr, int regAddr, int valueNum, out int[] values)
        {
            values = new int[valueNum];

            try
            {
                List<byte> cmd = new List<byte>
                {
                    (byte)devAddr, 0x04, Calc.HighByte(regAddr), Calc.LowByte(regAddr),
                    Calc.HighByte(valueNum), Calc.LowByte(valueNum)
                };
                cmd.AddRange(CheckSum16.Calculate(cmd.ToArray()));

                byte[] response = com.Query(cmd.ToArray(), 200);
                if (response.Length != 5 + 2 * valueNum)
                {
                    values = new int[0];
                    return false;
                }

                values = new int[valueNum];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = Calc.ConvertToShort(response[3 + 2 * i], response[4 + 2 * i]);
                }
                return true;
            }
            finally
            {
                DbgShow($"{com.Name} ReadO({devAddr}/0x{regAddr:X4})", values);
            }
        }
    }
}
