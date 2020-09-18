﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JbConf
{
    public class UiSupport
    {
        private static List<KeyValuePair<int, int>> CurrentLevels = new List<KeyValuePair<int, int>>();
        private static int ChangeOpLevel(ConfItem item, int level)
        {
            if (CurrentLevels.Count > 0 && level <= CurrentLevels.Last().Value)
            {
                CurrentLevels.RemoveAt(CurrentLevels.Count - 1);
            }
            
            if ((CurrentLevels.Count == 0) || (item.OpLevel > CurrentLevels.Last().Key))
            {
                CurrentLevels.Add(new KeyValuePair<int, int>(item.OpLevel, level));
            }

            return CurrentLevels.Last().Key;
        }

        public static DataTable ConvertToTable(ConfTree conf, string oplevel = null)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < conf.MaxDepth + 1; i++)
            {
                table.Columns.Add();
            }

            int userLevel = oplevel == null ? int.MaxValue : int.Parse(oplevel);
            conf.Visit("ToTable", (item, level) =>
            {
                var currentLevel = ChangeOpLevel(item, level);
                if(userLevel >= currentLevel)
                {
                    var row = table.NewRow();

                    if (string.IsNullOrEmpty(item.Path))
                    {
                        row[0] = item.Name;
                        row[1] = !string.IsNullOrEmpty(item.Value) ? item.Value : "";
                    }
                    else
                    {
                        var nodes = item.Path.Split('/');
                        row[nodes.Length - 1] = item.Name + (string.IsNullOrEmpty(item.Tag) ? "" : $"(tag: {item.Tag})");
                        row[table.Columns.Count - 1] = !string.IsNullOrEmpty(item.Value) ? item.Value : "";
                    }

                    table.Rows.Add(row);
                }

                return false;
            });

            return table;
        }
    }
}
