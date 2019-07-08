using System.Windows.Forms;
using JbConf;
using ConfViews;

namespace TestJbConfUi
{
    public partial class Basic : Form
    {
        public Basic()
        {
            InitializeComponent();

            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/Basic.xml");
            var view = new ConfView(conf);
            view.Dock = DockStyle.Fill;
            Controls.Add(view);
        }
    }
}
