using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;
using JbConf;

namespace ConfViews
{
    public static class Helper
    {
        public static void Apply(this ConfTree conf, ControlCollection Controls)
        {
            conf.Visit("Apply", (item, level) =>
            {
                if (item is ConfItem)
                {
                    if (item.Attributes.ContainsKey("guitype") && item.Attributes["guitype"] == "RadioButton")
                    {
                        var control = Controls.Find($"{item.Name}{item.Value}", true);
                        if (control.Length > 0)
                        {
                            (control[0] as RadioButton).Checked = true;
                        }
                    }
                    else
                    {
                        var control = Controls.Find(item.Name, true);
                        if (control.Length > 0)
                        {
                            control[0].Text = item.Value;
                        }
                    }
                }

                return false;
            });
        }
    }
}
