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
        private Func<string, bool> SaveEvtHandler;//返回值: true - 执行本模块内部保存流程; false - 自定义保存流程或者进行内容检查

        public ConfFileController()
        {
            InitializeComponent();
        }

        private bool IsBinded = false;
        private string InstName;
        private string RootPath;
        public string BackPath => $@"{RootPath}History";

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
            SaveEvtHandler = onSave;

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
        private void CMB_ProductFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsBinded)
            {
                Backup();
                OnChange?.Invoke(SelectedConf);
            }
        }

        private Dictionary<string, ConfTree> ConfTrees => ConfMgmt.Inst(InstName).Root;
        public string SelectedName => CMB_ProductFileList.Text;
        private string Version;
        private string SelectedFile => ConfTrees.Keys.ToList().Find(x => CMB_ProductFileList.Text == Path.GetFileNameWithoutExtension(x));
        public ConfTree SelectedConf => ConfTrees[SelectedFile];//jiangbo: dangerous
        private void Backup()
        {
            if (SelectedConf == null)
            {
                return;
            }

            if (!Directory.Exists(BackPath))
            {
                Directory.CreateDirectory(BackPath);
            }

            var version = SelectedConf.Find("Version");
            if (version != null)
            {
                Version = version.Value;

                var backFile = Calc.AddPostfix(SelectedFile, $"_{Version}");
                backFile = $"{BackPath}/{Path.GetFileName(backFile)}";
                if (!File.Exists($"{backFile}"))
                {
                    File.Copy(SelectedFile, $"{backFile}");
                }
            }
            else
            {
                Version = null;
            }
        }
        private void Backup(string file)
        {
            if (!Directory.Exists(BackPath))
            {
                Directory.CreateDirectory(BackPath);
            }

            var backup = $@"{BackPath}\{Calc.AddPostfix(Path.GetFileName(file), "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss"))}";
            try
            {
                File.Copy(file, backup);
            }
            catch (Exception ex)
            {
                Utils.UI.Help.NoticeFailure($"当前配置文件（{file}）备份失败: {ex}");
            }
        }
        private void Save(string path)
        {
            if (Version != null)
            {
                var curVer = SelectedConf["Version"];
                if (Version == curVer)
                {
                    var values = Version.Split('.');
                    var newVer = int.Parse(values.Last()) + 1;
                    SelectedConf["Version"] = string.Join(".", values.Skip(0).Take(values.Length - 1)) + $".{newVer}";
                }
            }

            if (path == null)
            {
                ConfTrees[SelectedFile].Save();
            }
            else
            {
                (ConfTrees[SelectedFile].Clone() as ConfTree).Save(path);
                ConfTrees[SelectedFile] = OrigConf;
            }

            CMB_ProductFileList_SelectedIndexChanged(null, null);
        }

        private void BTN_Save_Click(object sender, EventArgs e)
        {
            if (SaveEvtHandler == null ||  SaveEvtHandler(null))
            {
                Save(null);
                OnChange?.Invoke(SelectedConf);
            }
        }

        private ConfTree OrigConf;
        private void BTN_SaveAs_Click(object sender, EventArgs e)
        {
            var name = Interaction.InputBox("", "配置名称", "", 100, 200);
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            OrigConf = ConfTrees[SelectedFile].Clone() as ConfTree;
            var path = SelectedFile.Replace($"{SelectedName}.xml", $"{name}.xml");
            if (SaveEvtHandler == null || SaveEvtHandler(null))
            {
                Save(path);

                ConfMgmt.Inst(InstName).Generate(Path.GetDirectoryName(path), true);
                CMB_ProductFileList.DataSource = ConfTrees.Keys.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
                CMB_ProductFileList.Text = name;

                OnChange?.Invoke(SelectedConf);
            }
        }
    }
}
