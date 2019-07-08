﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

using JbConf;
using System.Drawing;

namespace ConfViews
{
    public partial class ConfView : UserControl
    {
        public ConfView()
        {
            InitializeComponent();
        }
        public ConfView(ConfTree conf)
        {
            InitializeComponent();

            DGV_ConfigItems.DataSource = UiSupport.ConvertToTable(conf);
            DGV_ConfigItems.Enabled = false;

            Timer t = new Timer(1000);
            t.AutoReset = true;
            t.Elapsed += (s, e) =>
            {
                SetCellEditMode();
                Invoke(new Action(() => {
                    DGV_ConfigItems.Enabled = true;
                }));
            };
            t.Start();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //Console.WriteLine("Readonly : " + DGV_ConfigItems.Rows[0].Cells[0].ReadOnly);
            //SetCellEditMode();
            //Console.WriteLine("Readonly : " + DGV_ConfigItems.Rows[0].Cells[0].ReadOnly);
            base.OnPaint(e);
        }
        private void SetCellEditMode()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(SetCellEditMode));
            }
            else
            {
                for (int row = 0; row < DGV_ConfigItems.Rows.Count; row++)
                {
                    for (int col = 0; col < DGV_ConfigItems.Columns.Count; col++)
                    {
                        if (IsValueOfItem(row, col))
                        {
                            DGV_ConfigItems.Rows[row].Cells[col].ReadOnly = false;
                            DGV_ConfigItems.Rows[row].Cells[col].Style.Font = new Font(this.Font, FontStyle.Regular);
                        }
                        else
                        {
                            DGV_ConfigItems.Rows[row].Cells[col].ReadOnly = true;
                            DGV_ConfigItems.Rows[row].Cells[col].Style.Font = new Font(this.Font, FontStyle.Bold);
                        }
                    }
                }
            }
        }

        private bool IsValueOfItem(int row, int col)
        {
            if (col == 0)
            {
                return false;
            }

            return (!string.IsNullOrEmpty(DGV_ConfigItems[col, row].Value as string)) && (!string.IsNullOrEmpty(DGV_ConfigItems[col - 1, row].Value as string));
        }
        private void DGV_ConfigItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string value = DGV_ConfigItems[e.ColumnIndex, e.RowIndex].Value as string;

            List<string> nodePath = new List<string>();

            int scanedRow = e.RowIndex;
            for (int col = e.ColumnIndex - 1; col >= 0; col--)
            {
                for (int row = scanedRow; row >= 0; row--)
                {
                    string nodeName = DGV_ConfigItems[col, row].Value as string;
                    if (!string.IsNullOrEmpty(nodeName))
                    {
                        nodePath.Add(nodeName);
                        scanedRow = row;
                        break;
                    }
                }
            }

            nodePath.Reverse();
            //reader.SetItem(nodePath.ToArray(), value);
        }
        public void Save(string file)
        {
            //reader.Save(file);
        }
    }
}