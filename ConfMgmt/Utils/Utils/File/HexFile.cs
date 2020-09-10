using System;
using System.Collections.Generic;
using System.IO;

namespace Utils
{
    public enum HexType
    {
        DataRecord,
        DataEnd,
        ExtendAddress,
        StartAddress,
        ExtendLinearAddress,
        EndLinearAddress
    }

    public class HexSection
    {
        public List<HexLine> Lines;
        public ushort Addr;
    }

    public struct HexLine
    {
        public string startTag;
        public int DataLen;
        public byte AddrLow;
        public byte AddrHigh;
        public HexType Type;
        public byte[] Data;
        public byte calcSum;
        public int Addr => AddrHigh << 8 | AddrLow;
    }

    public class HexFile
    {
        public List<HexSection> Sections;

        public HexFile Load(string path)
        {
            Sections = new List<HexSection>();

            string line = "";

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        HexLine hexline = new HexLine();

                        hexline.startTag = line.Substring(0, 1);
                        hexline.DataLen = Convert.ToInt32(line.Substring(1, 2), 16);
                        hexline.AddrHigh = Convert.ToByte(line.Substring(3, 2), 16);
                        hexline.AddrLow = Convert.ToByte(line.Substring(5, 2), 16);
                        hexline.Type = GetEnumFromInt<HexType>(Convert.ToInt32(line.Substring(7, 2), 16));
                        hexline.Data = HexStringToBytes(line.Substring(9, hexline.DataLen * 2));
                        hexline.calcSum = Convert.ToByte(line.Substring(line.Length - 2, 2), 16);

                        if (hexline.Type == HexType.ExtendLinearAddress)
                        {
                            Sections.Add(new HexSection());
                            Sections[Sections.Count - 1].Addr = (ushort)(hexline.Data[1] << 8 | hexline.Data[0]);
                            Sections[Sections.Count - 1].Lines = new List<HexLine>();
                        }

                        Sections[Sections.Count - 1].Lines.Add(hexline);
                    }

                    reader.Close();
                }

                fs.Close();
            }

            return this;
        }

        public T GetEnumFromInt<T>(int dataType)
        {
            return (T)Enum.ToObject(typeof(T), dataType);
        }

        public byte[] HexStringToBytes(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            for (int i = 0; i < returnBytes.Length / 2; i++)
            {
                byte temp = returnBytes[i * 2];
                returnBytes[i * 2] = returnBytes[i * 2 + 1];
                returnBytes[i * 2 + 1] = temp;
            }

            return returnBytes;
        }
    }
}
