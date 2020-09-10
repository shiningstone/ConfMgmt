using System;
using System.Windows.Forms;

namespace Utils.UI
{
    public class Help
    {
        public static string AskPath(string cfgName = "")
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            string defaultPath = ProjectConfig.Get(cfgName);
            if (!string.IsNullOrEmpty(defaultPath))
            {
                d.SelectedPath = defaultPath;
            }
            d.ShowDialog();

            string path = d.SelectedPath;
            ProjectConfig.Set(cfgName, path);

            return path;
        }
        public static string AskFile(string cfgName = "")
        {
            FileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                ProjectConfig.Set(cfgName, d.FileName);
                return d.FileName;
            }
            else
            {
                return ProjectConfig.Get(cfgName);
            }
        }
        public static ErrInfo ConfirmAction(string message, Func<ErrInfo> positiveAction, Func<ErrInfo> negativeAction = null)
        {
            if (MessageBox.Show(message, "注意", MessageBoxButtons.OKCancel, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                return positiveAction();
            }
            else
            {
                negativeAction?.Invoke();
                return new ErrInfo(ErrCode.UserCanceled);
            }
        }
        public static void NoticeFailure(string message)
        {
            MessageBox.Show(message, "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static string Read(Control control)
        {
            if (control.InvokeRequired)
            {
                IAsyncResult ia = control.BeginInvoke(new Func<Control, string>(Read), control);
                return (string)control.EndInvoke(ia);
            }
            else
            {
                if (control is TextBox)
                {
                    return (control as TextBox).Text;
                }
                else if (control is ComboBox)
                {
                    return (control as ComboBox).Text;
                }
                else
                {
                    throw new Exception($"Failed to read {control.Name}");
                }
            }
        }
        public static void Set(Control control, string value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new Action<Control, string>(Set), control, value);
            }
            else
            {
                if (control is TextBox)
                {
                    (control as TextBox).Text = value;
                }
                else if (control is ComboBox)
                {
                    var cb = control as ComboBox;
                    cb.SelectedIndex = cb.Items.IndexOf(value);
                }
                else if (control is Label)
                {
                    (control as Label).Text = value;
                }
                else
                {
                    throw new Exception($"Failed to set {control.Name}");
                }
            }
        }
    }
}
