using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class ProjectConfig
    {
        /// <summary>  
        /// 写入值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <param name="value"></param>  
        public static void Set(string key, string value)
        {
            //增加的内容写在appSettings段下 <add key="RegCode" value="0"/>  
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }

        /// <summary>  
        /// 读取指定key的值  
        /// </summary>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static string Get(string key)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] == null)
                return "";
            else
                return config.AppSettings.Settings[key].Value;
        }
        public static void Read(string itemName, ref int item)
        {
            string itemValue = "";
            try
            {
                itemValue = Get(itemName);
                if (!string.IsNullOrEmpty(itemValue))
                {
                    if (!itemValue.Contains("0x"))
                    {
                        item = int.Parse(itemValue);
                    }
                    else
                    {
                        item = Convert.ToInt32(itemValue, 16);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read config({itemName}:{itemValue})");
            }
        }
        public static void Read(string itemName, ref int[] item)
        {
            string itemValue = "";
            try
            {
                itemValue = Get(itemName);
                if (!string.IsNullOrEmpty(itemValue))
                {
                    string[] values = itemValue.Split(',');
                    item = new int[values.Length];

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (!values[i].Contains("0x"))
                        {
                            item[i] = int.Parse(values[i]);
                        }
                        else
                        {
                            item[i] = Convert.ToInt32(values[i], 16);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read config({itemName}:{itemValue}) - {ex}");
            }
        }
        public static void Read(string itemName, ref double item)
        {
            string itemValue = "";
            try
            {
                itemValue = Get(itemName);
                if (!string.IsNullOrEmpty(itemValue))
                {
                    item = double.Parse(itemValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read config({itemName}:{itemValue})");
            }
        }
        public static void Read(string itemName, ref double[] item)
        {
            string itemValue = "";
            try
            {
                itemValue = Get(itemName);
                if (!string.IsNullOrEmpty(itemValue))
                {
                    string[] values = itemValue.Split(',');
                    item = new double[values.Length];

                    for (int i = 0; i < values.Length; i++)
                    {
                        item[i] = double.Parse(values[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read config({itemName}:{itemValue}) - {ex}");
            }
        }
        public static void Write(string name, int item)
        {
            Set(name, item.ToString());
        }
        public static void Write(string name, int[] item)
        {
            string str = "";
            for (int i = 0; i<item.Length; i++)
            {
                str += $"{item[i]},";
            }
            str = str.Substring(0, str.Length - 1);

            Set(name, str);
        }
    }
}
