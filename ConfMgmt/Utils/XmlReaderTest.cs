using System;
using Utils;

namespace UtilsTest
{
    public class XmlReaderTest
    {
        private static string _sample = @"XmlReaderTest.xml";
        private static void TestRead()
        {
            XmlReader reader = new XmlReader(_sample);
            string value = reader.GetItem(new string[]
            {
                "HardwareEnv",
                "Floor1",
                "Slot1",
                "AirPressure",
                "GetValue"
            });
            if (value != "666")
            {
                throw new Exception();
            }

            value = reader.GetItem(new string[]
            {
                "HardwareEnv",
                "Floor1",
                "Slot2",
                "AirPressure",
                "GetValue"
            });
            if (value != "N/A")
            {
                throw new Exception();
            }
        }

        public static void Run()
        {
            TestRead();
        }
    }
}
