using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class ModBus
    {
        public static int AddrOffset = 40000;
        public static bool Write(ComPort com, int addr, int value)
        {
            int actualAddr = addr - AddrOffset - 1;
            List<byte> cmd = new List<byte>
            {
                0x01, 0x10, Utils.Calc.HighByte(actualAddr), Utils.Calc.LowByte(actualAddr),
                0x00, 0x01,
                0x02,
                Utils.Calc.HighByte(value), Utils.Calc.LowByte(value)
            };
            cmd.AddRange(CheckSum16.Calculate(cmd.ToArray()));

            byte[] response = com.Query(cmd.ToArray(), 200);
            if (response.Length != 8)
            {
                return false;
            }
            return true;
        }
        public static bool Read(ComPort com, int addr, int valueNum, out int[] values)
        {
            int actualAddr = addr - AddrOffset - 1;
            List<byte> cmd = new List<byte>
            {
                0x01, 0x03, Utils.Calc.HighByte(actualAddr), Utils.Calc.LowByte(actualAddr),
                Utils.Calc.HighByte(valueNum), Utils.Calc.LowByte(valueNum)
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
                values[i] = Utils.Calc.ConvertToShort(response[3 + 2 * i], response[4 + 2 * i]);
            }
            return true;
        }
    }
}
