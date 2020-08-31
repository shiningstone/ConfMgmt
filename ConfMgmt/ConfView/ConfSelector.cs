using System.IO;
using System.Linq;
using System.Windows.Forms;
using JbConf;

namespace ConfViews
{
    /// <summary>
    /// 提供ConfMgmt下所有ConfTree选项
    /// </summary>
    public class ConfSelector : ComboBox
    {
        public void Bind(ConfMgmt conf)
        {
            var list = conf.Root.Keys.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
            list.Insert(0, "");
            DataSource = list;
        }
    }
}
