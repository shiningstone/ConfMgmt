using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using JbConf;
using Utils;
using System.Collections.Generic;

namespace ConfViews
{
    /// <summary>
    /// 1. 选择ConfTree(OnChange)
    /// 2. 修改/新建ConfTree(OnSave)
    /// </summary>
    public partial class ConfFileController : UserControl
    {
        private Action<ConfTree> OnChange;
        private Func<string, bool> OnSave;

        public ConfFileController()
        {
            InitializeComponent();
        }

        private bool IsBinded = false;
        private string InstName;
        private string RootPath;

        private List<string> Order = new List<string>();
        private List<string> ReOrder(List<string> names)
        {
            var reorder = new List<string>();

            foreach (var name in Order)
            {
                if (names.Contains(name))
                {
                    reorder.Add(name);
                }
            }

            foreach (var name in names)
            {
                if (!reorder.Contains(name))
                {
                    reorder.Add(name);
                }
            }

            return reorder;
        }

        public void Bind(string title, string confType, string path, Action<ConfTree> onChange, Func<string, bool> onSave = null)
        {
            IsBinded = true;

            OnChange = onChange;
            OnSave = onSave != null ? onSave : SaveCallback;

            if (path == null)
            {
                return;
            }

            LBL_Title.Text = title;
            InstName = confType;
            RootPath = path;

            ConfMgmt.Inst(InstName).Generate(path, true);
            var names = ReOrder(ConfMgmt.Inst(InstName).Root.Keys.Select(x => Path.GetFileNameWithoutExtension(x)).ToList());
            
            CMB_ProductFileList.DataSource = names;
            OnChange?.Invoke(SelectedConf);
        }
        public void InitOrder(List<string> names)
        {
            Order = names;
        }

        public string SelectedName => CMB_ProductFileList.Text;
        private string SelectedPath => ConfMgmt.Inst(InstName).Root.Keys.ToList().Find(x => CMB_ProductFileList.Text == Path.GetFileNameWithoutExtension(x));
        public ConfTree SelectedConf => ConfMgmt.Inst(InstName).Root[SelectedPath];//jiangbo: dangerous

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
        private bool SaveCallback(string path)
        {
            if (path == null)
            {
                ConfMgmt.Inst(InstName).Root[SelectedPath].Save();
                Backup(SelectedPath);
            }
            else
            {
                (ConfMgmt.Inst(InstName).Root[SelectedPath].Clone() as ConfTree).Save(path);
                Backup(path);
            }

            return true;
        }
        private void SaveAs()
        {
            var name = Interaction.InputBox("", "配置名称", "", 100, 200);
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var path = SelectedPath.Replace($"{SelectedName}.xml", $"{name}.xml");
            OnSave(path);

            ConfMgmt.Inst(InstName).Generate(Path.GetDirectoryName(path), true);
            CMB_ProductFileList.DataSource = ConfMgmt.Inst(InstName).Root.Keys.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
            CMB_ProductFileList.Text = name;
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
            OnSave(null);
            OnChange?.Invoke(SelectedConf);
        }
        private void BTN_SaveAs_Click(object sender, EventArgs e)
        {
            SaveAs();
            OnChange?.Invoke(SelectedConf);
        }
    }
}
