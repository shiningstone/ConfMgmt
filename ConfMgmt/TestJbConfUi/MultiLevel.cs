using System.Windows.Forms;
using JbConf;
using ConfViews;

namespace TestJbConfUi
{
    public partial class MultiLevel : Form
    {
        public MultiLevel()
        {
            InitializeComponent();

            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/MultiLevel.xml");
            var view = new ConfView(conf);
            view.Dock = DockStyle.Fill;
            Controls.Add(view);
        }
    }
}
