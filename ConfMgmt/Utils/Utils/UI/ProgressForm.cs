using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Utils.UI
{
    public partial class ProgressForm : Form
    {
        public delegate void Update(ProgressInfo info);

        private Thread _thread;
        public ProgressForm(string caption)
        {
            InitializeComponent();
            Text = caption;
        }

        public void Show(Thread t, string name = null)
        {
            _thread = t;

            if (name != null)
            {
                _thread.Name = name;
            }
            _thread.IsBackground = true;
            _thread.Start();

            ShowDialog();
        }

        public void Notify(ProgressInfo info)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<ProgressInfo>(Notify), info);
                }
                else
                {
                    PB_Percentage.Value = info.Percentage;
                    if (info.Status != null)
                    {
                        LB_Status.Text = info.Status;
                    }
                    if (info.Detail != null)
                    {
                        LB_Info.Text = info.Detail;
                    }

                    if (info.Percentage >= 100)
                    {
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PB_Percentage.Value != 100)
            {
                SupportMessageBox.ConfirmAction("请确认是否取消当前运行的任务?", () =>
                {
                    _thread?.Abort();
                    return new ErrInfo(ErrCode.Ok);
                });
            }
        }
    }
    public class ProgressInfo
    {
        public string Status;
        public string Detail;
        public int Percentage;

        public ProgressInfo(int p, string s = null, string d = null)
        {
            Status = s;
            Detail = d;
            Percentage = p;
        }
    }
}
