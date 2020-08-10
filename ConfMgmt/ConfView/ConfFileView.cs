using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using JbConf;
using Utils;

namespace ConfViews
{
    public partial class ConfFileView : UserControl
    {
        public ConfFileView()
        {
            InitializeComponent();
        }

        private bool IsBinded = false;
        private Action<ConfTree> OnChange;
        private Func<bool> OnSave;

        private void Backup(string file)
        {
            var backdir = $@"{Path.GetDirectoryName(file)}\..\ProductFileHistory";
            if (!Directory.Exists(backdir))
            {
                Directory.CreateDirectory(backdir);
            }

            var backup = $@"{backdir}\{Calc.AddPostfix(Path.GetFileName(file), "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss"))}";
            try
            {
                File.Copy(file, backup);
            }
            catch (Exception ex)
            {
                Utils.UI.Help.NoticeFailure($"当前配置文件（{file}）备份失败: {ex}");
            }
        }
        public void Bind(string path, Action<ConfTree> onChange, Func<bool> onSave = null)
        {
            IsBinded = true;

            OnChange = onChange;
            OnSave = onSave;

            path = @"Configs\ProductFile";
            if (path == null)
            {
                return;
            }

            ConfMgmt.Inst("ProductFile").Generate(path);
            CMB_ProductFileList.DataSource = ConfMgmt.Inst("ProductFile").Root.Keys.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
            OnChange?.Invoke(SelectedConf);
        }

        private string SelectedName => CMB_ProductFileList.Text;
        private string SelectedPath => ConfMgmt.Inst("ProductFile").Root.Keys.ToList().Find(x => CMB_ProductFileList.Text == Path.GetFileNameWithoutExtension(x));
        public ConfTree SelectedConf => ConfMgmt.Inst("ProductFile").Root[SelectedPath];//jiangbo: dangerous

        private void Save()
        {
            ConfMgmt.Inst("ProductFile").Root[SelectedPath].Save();
            Backup(SelectedPath);

            OnChange?.Invoke(SelectedConf);
        }
        private void SaveAs()
        {
            var name = Interaction.InputBox("", "ProductFile名称", "", 100, 200);
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var path = SelectedPath.Replace(SelectedName, name);

            var newconf = ConfMgmt.Inst("ProductFile").Root[SelectedPath].Clone() as ConfTree;
            newconf.Save(path);
            Backup(path);

            ConfMgmt.Inst("ProductFile").Generate(Path.GetDirectoryName(path));
            CMB_ProductFileList.DataSource = ConfMgmt.Inst("ProductFile").Root.Keys.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
            CMB_ProductFileList.Text = name;
            
            OnChange?.Invoke(SelectedConf);
        }

        private void CMB_ProductFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsBinded)
            {
                OnChange?.Invoke(SelectedConf);
            }
        }
        private void BTN_Save_Click(object sender, EventArgs e)
        {
            if (OnSave == null || OnSave.Invoke())
            {
                Save();
            }
        }
        private void BTN_SaveAs_Click(object sender, EventArgs e)
        {
            if (OnSave == null || OnSave.Invoke())
            {
                SaveAs();
            }
        }
    }
}
