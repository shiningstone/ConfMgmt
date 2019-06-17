using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utils
{
    public class CsvFile
    {
        FileStream _fileStream;
        StreamWriter _streamWriter;

        public CsvFile(string path)
        {
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
        public void Write(List<string> data)
        {
            _streamWriter.Write(string.Join(",", data));
            _streamWriter.Write(Environment.NewLine);
            _streamWriter.Flush();
        }
        public void Close()
        {
            _streamWriter.Close();
            _fileStream.Close();
        }
    }
}