using System;
using System.IO;

namespace Utils
{
    public class FileOp
    {
        private static Logger _log = new Logger("FileOp");

        public static string ExtractUrl(string url)
        {
            if (url.StartsWith(@"file:///"))
            {
                return url.Substring(@"file:///".Length);
            }

            return url;
        }
        public static bool CopyDir(string src, string dest, bool overwriteexisting = true)
        {
            bool ret = false;
            try
            {
                src = src.EndsWith(@"\") ? src : src + @"\";
                dest = dest.EndsWith(@"\") ? dest : dest + @"\";

                if (Directory.Exists(src))
                {
                    if (Directory.Exists(dest) == false)
                        Directory.CreateDirectory(dest);

                    foreach (string fls in Directory.GetFiles(src))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(dest + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(src))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDir(drs, dest + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                _log.Error($"CopyDir({src}, {dest})", ex);
                ret = false;
            }
            return ret;
        }
        public static bool RmDir(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    string[] fileSystemEntries = Directory.GetFileSystemEntries(dir);
                    for (int i = 0; i < fileSystemEntries.Length; i++)
                    {
                        string text = fileSystemEntries[i];
                        if (File.Exists(text))
                        {
                            File.Delete(text);
                        }
                        else
                        {
                            RmDir(text);
                        }
                    }

                    Directory.Delete(dir);
                }

                return true;
            }
            catch (Exception ex)
            {
                _log.Error($"RmDir({dir})", ex);
                return false;
            }
        }
    }
}
