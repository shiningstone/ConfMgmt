using System.IO;
using System.Linq;
using System.Windows.Forms;
using JbConf;

namespace ConfViews
{
    public class ConfMgmtComboBox : ComboBox
    {
        public void Bind(ConfMgmt conf)
        {
            DataSource = conf.Root.Keys.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
        }
    }
}
