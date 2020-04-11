using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

using Utils;
using JbConf;

namespace ConfViews
{
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

            _log.Debug("Load ConfTree");
            Conf = conf;
            DGV_ConfigItems.DataSource = UiSupport.ConvertToTable(Conf);
            DGV_ConfigItems.Enabled = false;
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

        private string GetPath(DataGridView gridview, DataGridViewCellEventArgs activeCell)
        {
            List<string> nodePath = new List<string>();

            int scanedRow = activeCell.RowIndex;
            for (int col = activeCell.ColumnIndex - 1; col >= 0; col--)
            {
                for (int row = scanedRow; row >= 0; row--)
                {
                    string nodeName = gridview[col, row].Value as string;
                    if (!string.IsNullOrEmpty(nodeName))
                    {
                        nodePath.Add(nodeName);
                        scanedRow = row;
                        break;
                    }
                }
            }

            var nodes = nodePath.Take(nodePath.Count - 1);
            return string.Join(@"/", nodes.Reverse());
        }
        private void DGV_ConfigItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string path = GetPath(sender as DataGridView, e);
            Conf[path] = DGV_ConfigItems[e.ColumnIndex, e.RowIndex].Value as string;
        }
        public void Save(string file)
        {
            //reader.Save(file);
        }

        private void DGV_ConfigItems_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            SetCellEditMode();
        }
    }
}
