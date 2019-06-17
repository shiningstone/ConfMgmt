using System;
using System.IO;
using System.Windows.Forms;

namespace Utils.UI
{
    public partial class ConfigurationEditor : Form
    {
        private string _file;
        public ConfigurationEditor(string configFile = null)
        {
            InitializeComponent();

            if (configFile != null)
            {
                LoadConfigFile(configFile);
            }
        }

        private void BTN_Load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.CheckFileExists = true;
                dlg.DefaultExt = "xml";
                dlg.Title = "Select a configuration file";
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                dlg.RestoreDirectory = true;
                dlg.Multiselect = false;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadConfigFile(dlg.FileName);
                }
            }
        }

        private void BTN_Save_Click(object sender, EventArgs e)
        {
            File.Copy(_file, Calc.AddPostfix(_file, DateTime.Now.ToString("_yyMMdd-HHmmss")));
            configView1.Save(_file);
            MessageBox.Show(@"Save Configuration file Done");
        }
        private void LoadConfigFile(object file)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LoadConfigFile), file);
            }
            else
            {
                if (file != null)
                {
                    _file = file as string;
                    configView1.LoadXml(_file);
                    Text = $"Configuration Editor({_file})";
                }
            }
        }
    }
}
