using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utils
{
    public static class DictHelp
    {
        public static Dictionary<T1, T2> Combine<T1, T2>(this Dictionary<T1, T2> d1, Dictionary<T1, T2> d2)
        {
            foreach (var kv in d2)
            {
                d1[kv.Key] = d2[kv.Key];
            }

            return d1;
        }
        public static DataTable ToDataTable<T1, T2>(this Dictionary<T1, T2> d1)
        {
            var table = new DataTable();
            foreach (var key in d1.Keys)
            {
                table.Columns.Add(key.ToString());
            }

            var row = table.NewRow();
            foreach (var kv in d1)
            {
                row[kv.Key.ToString()] = kv.Value.ToString();
            }
            table.Rows.Add(row);

            return table;
        }

        public static string ToString<T1, T2>(Dictionary<T1, T2> dict, string seperator = ";")
        {
            string output = "";
            foreach (var kv in dict)
            {
                output += $"{kv.Key}:{kv.Value}{seperator}";
            }
            return output;
        }
        public static string ShowFields<T>(T target)
        {
            var fields = (typeof(T)).GetFields();
            string output = "";
            foreach (var p in fields)
            {
                var value = typeof(T).GetField(p.Name).GetValue(target);
                if (value != null)
                {
                    output += $"{p.Name}: {value}; ";
                }
            }
                                                      
            return output;
        }
    }
}
