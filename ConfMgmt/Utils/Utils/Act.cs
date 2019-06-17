using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class Act
    {
        public static bool SafeExecute(string action, Action a, int maxTry = 3)
        {
            for (int i = 0; i < maxTry; i++)
            {
                try
                {
                    a();
                    return true;
                }
                catch (Exception ex)
                {
                    _log.Warn("Failed to " + action + "(" + i.ToString() + ")", ex);
                }
            }

            _log.Error("Failed to " + action);
            return false;
        }
        private static Logger _log = new Logger("Utils");
    }
}
