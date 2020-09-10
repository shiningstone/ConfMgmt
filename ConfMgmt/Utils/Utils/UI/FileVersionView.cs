using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils.UI
{
    public partial class FileVersionView : UserControl
    {
        private static string[] Header =
        {
            "File Name",
            "File Version",
            "Date",
            "Path",
            "Detail Info",
        };
        public static DataTable GetInfo(string[] files)
        {
            DataTable result = new DataTable();
            Header.ToList().ForEach(x => result.Columns.Add(x));

            foreach (string path in files)
            {
                var row = result.NewRow();

                var file = new FileInfo(path);
                if (file.Exists)
                {
                    var version = FileVersionInfo.GetVersionInfo(path);

                    row["File Name"] = version.OriginalFilename;
                    row["File Version"] = version.FileVersion;
                    row["Date"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                    row["Path"] = path;
                    row["Detail Info"] = version.ProductVersion;
                }
                else
                {
                    row["File Name"] = path;
                    row["File Version"] = "N/A";
                }

                result.Rows.Add(row);
            }

            return result;
        }

        public void LoadFiles(string[] files)
        {
            LV_FileInfos.Items.Clear();

            foreach (string path in files)
            {
                var file = new FileInfo(path);
                if (file.Exists)
                {
                    var version = FileVersionInfo.GetVersionInfo(path);

                    var item = new ListViewItem(version.OriginalFilename);
                    item.SubItems.Add(version.FileVersion);
                    item.SubItems.Add(file.LastWriteTime.ToString("yyyy-MM-dd"));
                    item.SubItems.Add(path);
                    item.SubItems.Add(version.ProductVersion);
                    LV_FileInfos.Items.Add(item);
                }
                else
                {
                    var item = new ListViewItem(path);
                    item.SubItems.Add("NA");
                    LV_FileInfos.Items.Add(item);
                }
            }

            foreach (ColumnHeader c in LV_FileInfos.Columns)
            {
                c.Width = -2;
            }
        }
        public string Copy()
        {
            return LV_FileInfos.Items.Cast<ListViewItem>().Aggregate("", (current, item) =>
            {
                string output = "";
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    output += item.SubItems[i].Text + "\t";
                }
                output += Environment.NewLine;
                return current + output;
            });
        }
        public FileVersionView()
        {
            InitializeComponent();

            foreach (var h in Header)
            {
                LV_FileInfos.Columns.Add(
                    new ColumnHeader
                    {
                        Text = h,
                    }
                );
            }
        }
    }
}
