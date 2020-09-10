using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utils
{
    public class LuaFile
    {
        FileStream _fileStream;
        StreamWriter _streamWriter;

        public LuaFile(string path)
        {
            if (path == null)
            {
                return;
            }

            if (!File.Exists(path))
            {
                _fileStream = new FileStream(path, FileMode.Create);
            }
            else
            {
                _fileStream = new FileStream(path, FileMode.Append);
            }

            _streamWriter = new StreamWriter(_fileStream, Encoding.UTF8);
        }

        public void Close()
        {
            _streamWriter.Close();
            _fileStream.Close();
        }

        public List<string> Load(string path)
        {
            var result = new List<string>();

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    result.Add(reader.ReadLine());
                }
            }

            return result;
        }
    }
}
