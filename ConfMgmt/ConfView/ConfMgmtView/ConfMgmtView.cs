using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JbConf;

namespace ConfViews
{
    /// <summary>
    /// 1. 通过ConfFileController在ConfView显示ConfItem
    /// 2. 通过ConfFileController修改/新建ConfTree
    /// </summary>
    public partial class ConfMgmtView : UserControl
    {
        public ConfMgmtView()
        {
            InitializeComponent();
        }

        private bool IsBinded = false;
        private Action OnUpdate;

        public void Bind(string title, string confType, string path, Action onUpdate = null, Func<string, bool> saveEvtHandler = null)
        {
            IsBinded = true;
            OnUpdate = onUpdate;

            fileController.Bind(title, confType, path, SwitchConf, saveEvtHandler);
        }
        
        public void InitOrder(List<string> names)
        {
            fileController.InitOrder(names);
        }
        public void InitShowLevel(List<string> levels)
        {
            if (levels != null)
            {
                CMB_ShowLevel.DataSource = levels;
                CMB_ShowLevel.SelectedIndex = 0;
                LBL_Level.Visible = CMB_ShowLevel.Visible = true;
            }
            else
            {
                LBL_Level.Visible = CMB_ShowLevel.Visible = false;
            }
        }
        private void SwitchConf(ConfTree conf)
        {
            if (IsBinded)
            {
                confView1.LoadConf(conf, CMB_ShowLevel.Visible ? (CMB_ShowLevel.SelectedIndex + 1).ToString() : null);
                OnUpdate?.Invoke();
            }
        }

        private void CMB_ShowLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            confView1.SetLevel((CMB_ShowLevel.SelectedIndex + 1).ToString());
        }
    }
}
