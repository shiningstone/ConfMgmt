using System.Windows.Forms;
using JbConf;
using ConfViews;

namespace TestJbConfUi
{
    public partial class ConfigFiles : Form
    {
        ConfView confView;
        public ConfigFiles()
        {
            InitializeComponent();

            ConfTree conf = Builder.Xml.Generate($@"{GlobalVar.SamplePath}/MultiLevel.xml");
            confView = new ConfView(conf);
            Controls.Add(confView);
        }
    }
}
