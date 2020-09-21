using System;
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
            LoadConf(conf, null);
        }
        public void LoadConf(ConfTree conf, string oplevel)
        {
            _log.Debug($"Load ConfTree({(oplevel != null ? "" : "null")})");

            Conf = conf;
            DGV_ConfigItems.DataSource = UiSupport.ConvertToTable(Conf, oplevel);
            DGV_ConfigItems.Columns[DGV_ConfigItems.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        public void SetLevel(string oplevel)
        {
            LoadConf(Conf, oplevel);
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
                        nodeName = UiSupport.UnlableComment(nodeName);

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

        private ConfItem SelectedItem;
        private int GetValidColIdx(int rowIdx, int colIdx)
        {
            var row = (DGV_ConfigItems.DataSource as DataTable).Rows[rowIdx];
            if (colIdx == row.Table.Columns.Count - 1 && !string.IsNullOrEmpty(row[colIdx] as string))
            {
                return colIdx;
            }
            else
            {
                for (int i = row.Table.Columns.Count - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrEmpty(row[i] as string))
                    {
                        return i + 1;
                    }
                }

                return row.Table.Columns.Count - 1;
            }
        }
        private ConfItem GetSelectedItem(int rowIdx, int colIdx)
        {
            var validCol = GetValidColIdx(rowIdx, colIdx);
            string path = GetPath(DGV_ConfigItems, rowIdx, validCol);
            SelectedItem = Conf.GetItem(path);

            return SelectedItem;
        }

        private void UpdateSelection(int rowIdx, int colIdx)
        {
            if (DGV_ConfigItems.Rows[rowIdx].Selected == false)//若行已是选中状态就不再进行设置
            {
                DGV_ConfigItems.ClearSelection();
                DGV_ConfigItems.Rows[rowIdx].Selected = true;
            }

            if (DGV_ConfigItems.SelectedRows.Count == 1)//只选中一行时设置活动单元格
            {
                DGV_ConfigItems.CurrentCell = DGV_ConfigItems.Rows[rowIdx].Cells[colIdx];
            }
        }

        private void DGV_ConfigItems_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                UpdateSelection(e.RowIndex, e.ColumnIndex);

                SelectedItem = GetSelectedItem(e.RowIndex, e.ColumnIndex);
                if (SelectedItem.Attributes.ContainsKey("comment"))
                {
                    DGV_ConfigItems.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = SelectedItem.Attributes["comment"];
                }
            }
            catch (Exception ex)
            {
                //_log.Warn($"DGV_ConfigItems_CellMouseEnter({e.RowIndex},{e.ColumnIndex})", ex);
            }
        }

        private void DGV_ConfigItems_CellRightClicked(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex == 0)
                {
                    return;
                }

                try
                {
                    UpdateSelection(e.RowIndex, e.ColumnIndex);

                    SelectedItem = GetSelectedItem(e.RowIndex, e.ColumnIndex);
                    if (SelectedItem.Attributes.ContainsKey("allow"))
                    {
                        switch (SelectedItem.Attributes["allow"])
                        {
                            case "both":
                                MNU_BothOps.Show(MousePosition.X, MousePosition.Y);
                                break;
                            case "add":
                                MNU_AddOps.Show(MousePosition.X, MousePosition.Y);
                                break;
                            case "remove":
                                MNU_RemoveOps.Show(MousePosition.X, MousePosition.Y);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.Warn($"DGV_ConfigItems_CellMouseDown({e.RowIndex},{e.ColumnIndex})", ex);
                }
            }
        }

        private void Menu_AddSon_Click(object sender, EventArgs e)
        {
            var form = new ConfEditForm(Conf, SelectedItem, () => {
                LoadConf(Builder.Xml.Generate(Conf.XmlDoc.BaseURI.Substring(@"file:///".Length)));
            });
            form.Show();
        }

        private void Menu_RemoveThis_Click(object sender, EventArgs e)
        {
            Conf.RemoveNode(SelectedItem);
            LoadConf(Builder.Xml.Generate(Conf.XmlDoc.BaseURI.Substring(@"file:///".Length)));
        }
    }
}
