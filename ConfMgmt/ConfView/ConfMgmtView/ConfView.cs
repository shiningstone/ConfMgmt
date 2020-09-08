﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

using Utils;
using JbConf;
using System.Data;
using System.Text.RegularExpressions;

namespace ConfViews
{
    /// <summary>
    /// 显示ConfTree下所有ConfItem
    /// </summary>
    public partial class ConfView : UserControl
    {
        private static Logger _log = new Logger("ConfView");
        public ConfView()
        {
            InitializeComponent();
        }
        public ConfTree Conf;
        public ConfView(ConfTree conf)
        {
            InitializeComponent();
            Dock = DockStyle.Fill;

            _log.Debug("Load ConfTree");
            Conf = conf;
            DGV_ConfigItems.DataSource = UiSupport.ConvertToTable(Conf);
            DGV_ConfigItems.Columns[DGV_ConfigItems.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void LoadConf(ConfTree conf)
        {
            _log.Debug("Load ConfTree");
            Conf = conf;
            DGV_ConfigItems.DataSource = UiSupport.ConvertToTable(Conf);
            DGV_ConfigItems.Columns[DGV_ConfigItems.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private bool IsValueOfItem(int row, int col)
        {
            if (col == 0)
            {
                return false;
            }

            return (!string.IsNullOrEmpty(DGV_ConfigItems[col, row].Value as string)) && (!string.IsNullOrEmpty(DGV_ConfigItems[col - 1, row].Value as string));
        }
        private void SetCellEditMode()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(SetCellEditMode));
            }
            else
            {
                var labelFont = new Font(this.Font, FontStyle.Bold);
                var valueFont = new Font(this.Font, FontStyle.Regular);

                for (int col = 0; col < DGV_ConfigItems.Columns.Count; col++)
                {
                    if (col < DGV_ConfigItems.ColumnCount - 1)
                    {
                        DGV_ConfigItems.Columns[col].ReadOnly = true;
                        DGV_ConfigItems.Columns[col].DefaultCellStyle.Font = labelFont;
                    }
                    else
                    {
                        DGV_ConfigItems.Columns[col].ReadOnly = false;
                        DGV_ConfigItems.Columns[col].DefaultCellStyle.Font = valueFont;
                    }
                }
            }
        }

        private string GetPath(DataGridView gridview, int rowIdx, int colIdx)
        {
            List<string> nodePath = new List<string>();
            List<string> tags = new List<string>();

            int scanedRow = rowIdx;
            for (int col = colIdx - 1; col >= 0; col--)
            {
                for (int row = scanedRow; row >= 0; row--)
                {
                    string nodeName = gridview[col, row].Value as string;
                    if (!string.IsNullOrEmpty(nodeName))
                    {
                        if (Regex.IsMatch(nodeName, @"\(tag: (.*)\)"))
                        {
                            tags.Add(Regex.Match(nodeName, @"\(tag: (.*)\)", RegexOptions.Singleline).Groups[1].Value);
                            nodeName = Regex.Replace(nodeName, @"\(tag: (.*)\)", "");
                        }

                        nodePath.Add(nodeName);
                        scanedRow = row;
                        break;
                    }
                }
            }

            var nodes = nodePath.Take(nodePath.Count - 1);
            var output = "";
            if (tags.Count > 0)
            {
                tags.Reverse();
                output += string.Join(@"&", tags) + ":";
            }

            output += string.Join(@"/", nodes.Reverse());

            return output;
        }

        private void DGV_ConfigItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string path = GetPath(sender as DataGridView, e.RowIndex, e.ColumnIndex);
            Conf[path] = DGV_ConfigItems[e.ColumnIndex, e.RowIndex].Value as string;
        }

        private void DGV_ConfigItems_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetCellEditMode();
        }

        private void DGV_ConfigItems_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //若行已是选中状态就不再进行设置
                if (DGV_ConfigItems.Rows[e.RowIndex].Selected == false)
                {
                    DGV_ConfigItems.ClearSelection();
                    DGV_ConfigItems.Rows[e.RowIndex].Selected = true;
                }
                //只选中一行时设置活动单元格
                if (DGV_ConfigItems.SelectedRows.Count == 1)
                {
                    DGV_ConfigItems.CurrentCell = DGV_ConfigItems.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }

                string path = GetPath(sender as DataGridView, e.RowIndex, e.ColumnIndex);

                var item = Conf.Find(path);
                if (item is ConfTree)
                {
                    MNU_Del.Show(MousePosition.X, MousePosition.Y);
                }
                else
                {
                    MNU_Add.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
    }
}
