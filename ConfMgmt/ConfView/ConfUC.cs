using System;
using System.Windows.Forms;
using JbConf;

namespace ConfViews
{
    /// <summary>
    /// 查找conf内各个item的同名控件并赋值
    /// </summary>
    public class ConfUC : UserControl
    {
        private ConfTree CurrentConf;
        public virtual void Apply(ConfTree conf)
        {
            CurrentConf = conf;
            conf.Visit("Apply", (item, level) =>
            {
                if (item is ConfItem)
                {
                    if (item.Attributes.ContainsKey("guitype") && item.Attributes["guitype"] == "RadioButton")
                    {
                        var control = Controls.Find($"{item.Name}_{item.Value}", true);
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
        //收集各个item（TextBox/RadioButton）的值生成ConfTree
        public virtual ConfTree Generate(string tag = null)
        {
            var conf = new ConfTree(GetType().Name);

            Traverse(Controls, (control) =>
            {
                if (control is TextBox)
                {
                    var tb = control as TextBox;
                    conf.Add(new ConfItem(tb.Name, tb.Text));
                }
                else if (control is RadioButton)
                {
                    var rb = control as RadioButton;
                    if (rb.Checked)
                    {
                        var values = rb.Name.Split('_');
                        var name = values[0];
                        var value = values[1];
                        var item = new ConfItem(name, value);
                        item.Attributes.Add("guitype", "RadioButton");
                        conf.Add(item);
                    }
                }
            });

            CurrentConf = conf;

            return conf;
        }

        private bool IsContainer(Control c)
        {
            return c is GroupBox || c is TableLayoutPanel;
        }
        private void Traverse(ControlCollection container, Action<Control> action)
        {
            foreach (Control control in container)
            {
                if (!IsContainer(control))
                {
                    action(control);
                }
                else
                {
                    Traverse(control.Controls, action);
                }
            }
        }
    }
}
