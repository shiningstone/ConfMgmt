using System;
using System.Collections.Generic;
using System.IO;
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
        public static void Traverse(string path, Action<string> action)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            DirectoryInfo[] dirs = root.GetDirectories();
            if (dirs.Length == 0)
            {
                var files = root.GetFiles();
                foreach (var f in files)
                {
                    action(f.FullName);
                }
            }
            else
            {
                foreach (DirectoryInfo dir in dirs)
                {
                    Traverse(dir.FullName, action);
                }
            }
        }
        private static Logger _log = new Logger("Utils");
    }
}
