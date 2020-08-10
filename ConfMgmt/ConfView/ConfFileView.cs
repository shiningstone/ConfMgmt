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
        private string InstName;
        private string RootPath;
        private Action<ConfTree> OnChange;
        private Func<bool> OnSave;

        private void Backup(string file)
        {
            var dir = $@"{Path.GetDirectoryName(file)}\";
            var backdir = $@"{RootPath}History";
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
        public void Bind(string name, string path, Action<ConfTree> onChange, Func<bool> onSave = null)
        {
            IsBinded = true;

            OnChange = onChange;
            OnSave = onSave;

            if (path == null)
            {
                return;
            }

            InstName = name;
            RootPath = path;
            ConfMgmt.Inst(InstName).Generate(path, true);
            CMB_ProductFileList.DataSource = ConfMgmt.Inst(InstName).Root.Keys.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
            OnChange?.Invoke(SelectedConf);
        }

        private string SelectedName => CMB_ProductFileList.Text;
        private string SelectedPath => ConfMgmt.Inst(InstName).Root.Keys.ToList().Find(x => CMB_ProductFileList.Text == Path.GetFileNameWithoutExtension(x));
        public ConfTree SelectedConf => ConfMgmt.Inst(InstName).Root[SelectedPath];//jiangbo: dangerous

        private void Save()
        {
            ConfMgmt.Inst(InstName).Root[SelectedPath].Save();
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

            var newconf = ConfMgmt.Inst(InstName).Root[SelectedPath].Clone() as ConfTree;
            newconf.Save(path);
            Backup(path);

            ConfMgmt.Inst(InstName).Generate(Path.GetDirectoryName(path), true);
            CMB_ProductFileList.DataSource = ConfMgmt.Inst(InstName).Root.Keys.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
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
