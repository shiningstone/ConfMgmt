using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils.UI
{
    public partial class InitForm : Form
    {
        private static Logger _log;
        private static InitForm _inst;
        private Bitmap _bitmap;
        public static InitForm Instance(Logger log = null)
        {
            _log = log;
            return  _inst ?? (_inst = new InitForm());
        }
        protected InitForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            _bitmap = new Bitmap(Properties.Resources.Logo);
            //ClientSize = _bitmap.Size;
            BackgroundImage = _bitmap;

            string info = @"Start initialize,please wait ......";
            _log?.Debug(info);
            using (Font font = new Font("Consoles", 30, FontStyle.Bold))
            {
                using (Graphics g = Graphics.FromImage(_bitmap))
                {
                    g.DrawString(info, Font, Brushes.White, 300, 100);
                }
            }
        }

        public void Update(string info)
        {
            _log?.Debug(info);
            using (Font font = new Font("Consoles", 30, FontStyle.Bold))
            {
                using (Graphics g = Graphics.FromImage(_bitmap))
                {
                    g.DrawString(info, Font, Brushes.White, 300, 100);
                }
            }
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
        }

        private void InitForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
    }
}
