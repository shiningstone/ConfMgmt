using System;
using System.Collections.Generic;
using System.Data;
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
            if (path == null)
            {
                return;
            }

            if (!path.Contains(".csv"))
            {
                path += ".csv";
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

        public void Write(List<string> lines)
        {
            foreach (var line in lines)
            {
                _streamWriter.WriteLine(line);
            }

            _streamWriter.Flush();
        }
        public void WriteData(List<string> data)
        {
            _streamWriter.Write(string.Join(",", data));
            _streamWriter.Write(Environment.NewLine);
            _streamWriter.Flush();
        }

        public void WriteHeaders(DataTable table)
        {
            var col = new List<string>();

            foreach (DataColumn c in table.Columns)
            {
                col.Add(c.ColumnName);
            }

            Write(new List<string>() { string.Join(",", col) });
        }

        public void Write(DataTable table)
        {
            foreach (DataRow line in table.Rows)
            {
                WriteData(line.ItemArray.ToList().Select(x => x.ToString()).ToList());
            }
        }

        public void Close()
        {
            _streamWriter.Close();
            _fileStream.Close();
        }

        private DataTable CreateTable(string[] columns)
        {
            var table = new DataTable();

            for (int i = 0; i < columns.Length; i++)
            {
                table.Columns.Add(columns[i]);
            }

            return table;
        }

        public DataTable Load(string path)
        {
            DataTable table = null;
            bool isTableCreated = false;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');

                    if (!isTableCreated)
                    {
                        table = CreateTable(values);
                        isTableCreated = true;
                    }
                    else
                    {
                        table.Rows.Add(table.NewRow());
                        for (int i = 0; i < values.Length; i++)
                        {
                            table.Rows[table.Rows.Count - 1][i] = values[i];
                        }
                    }
                }
            }

            return table;
        }

    }
}