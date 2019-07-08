using System.Windows.Forms;
using JbConf;
using ConfViews;

namespace TestJbConfUi
{
    public partial class ConfigFiles : Form
    {
        public ConfigFiles()
        {
            InitializeComponent();

            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.ResultPath}/Root.xml");
            var view = new ConfView(conf);
            view.Dock = DockStyle.Fill;
            Controls.Add(view);
        }
    }
}
