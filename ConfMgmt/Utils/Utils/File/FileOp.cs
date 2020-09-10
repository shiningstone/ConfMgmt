using System;
using System.Collections.Generic;
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

        public static List<string> Read(string path, bool ignoreBlank = true)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var lines = new List<string>();
            string line = "";

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }

                    reader.Close();
                }

                fs.Close();
            }

            return lines.FindAll(x => x.Length > 0);
        }
        public static void Write(string path, List<string> lines)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }

                writer.Close();
            }
        }
    }
}
