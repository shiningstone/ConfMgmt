using System.IO;
using System.Linq;
using System.Windows.Forms;
using JbConf;

namespace ConfViews
{
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
