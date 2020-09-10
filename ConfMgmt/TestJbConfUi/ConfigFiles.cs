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

            ConfTree conf = Builder.Xml.Generate($@"D:\DieTester\DieTester\DieTester\bin\Debug\Configs\SpecFile\DefaultSpec.xml");
            confView = new ConfView(conf);
            Controls.Add(confView);
        }
    }
}
