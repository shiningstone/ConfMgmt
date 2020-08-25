using System;
using System.Windows.Forms;
using JbConf;

namespace ConfViews
{
    public partial class ConfMgmtView : UserControl
    {
        public ConfMgmtView()
        {
            InitializeComponent();
        }

        private bool IsBinded = false;
        private Action OnUpdate;

        public void Bind(string path, Action onUpdate = null)
        {
            IsBinded = true;
            OnUpdate = onUpdate;

            pfileCtrlView.Bind("ProductFile", path, Change);
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
