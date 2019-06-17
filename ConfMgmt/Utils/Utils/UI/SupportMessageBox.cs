using System;
using System.Windows.Forms;

namespace Utils.UI
{
    public class SupportMessageBox
    {
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
    }
}
