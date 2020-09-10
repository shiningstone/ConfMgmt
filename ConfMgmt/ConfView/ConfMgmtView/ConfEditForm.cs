using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JbConf;

namespace ConfViews
{
    public partial class ConfEditForm : Form
    {
        public ConfEditForm()
        {
            InitializeComponent();
        }

        public ConfEditForm(ConfTree root, ConfItem item)
        {
            InitializeComponent();

            confEditor1.Bind(root, item);
        }
    }
}
