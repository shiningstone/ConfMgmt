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

        public void Bind(string title, string confType, string path, Action onUpdate = null)
        {
            IsBinded = true;
            OnUpdate = onUpdate;

            fileController.Bind(title, confType, path, Change);
        }

        public void SetOrder(List<string> names)
        {
            fileController.SetOrder(names);
        }

        private void Change(ConfTree conf)
        {
            if (IsBinded)
            {
                confView1.LoadConf(conf);
                OnUpdate?.Invoke();
            }
        }
    }
}
