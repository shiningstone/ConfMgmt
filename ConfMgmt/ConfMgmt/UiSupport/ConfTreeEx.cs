using System;
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

        private static readonly string CommentLabel = "`";
        public static string DisplayName(ConfItem item)
        {
            return item.Name + (!item.Attributes.ContainsKey("comment") ? "" : CommentLabel)
                            + (string.IsNullOrEmpty(item.Tag) ? "" : $"(tag: {item.Tag})");
        }
        public static string UnlableComment(string lableName)
        {
            return lableName.Replace(CommentLabel, "");
        }
        public static DataTable ConvertToTable(ConfTree conf, string oplevel = null)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < conf.MaxDepth + 1; i++)
            {
                table.Columns.Add();
            }

            int userLevel = oplevel == null ? int.MaxValue : int.Parse(oplevel);
            CurrentLevels = new List<KeyValuePair<int, int>>();
            conf.Visit("ToTable", (item, level) =>
            {
                var currentLevel = ChangeOpLevel(item, level);
                if(userLevel >= currentLevel)
                {
                    if (!IsFiltered(conf, item, level))
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
                            row[nodes.Length - 1] = DisplayName(item);
                            row[table.Columns.Count - 1] = !string.IsNullOrEmpty(item.Value) ? item.Value : "";
                        }

                        table.Rows.Add(row);
                    }
                }

                return false;
            });

            return table;
        }

        private static int FilterLevel = -1;
        private static bool IsFiltered(ConfTree conf, ConfItem item, int level)
        {
            if (FilterLevel >= 0 && level > FilterLevel)
                {
                    return true;
                }
            else
            {
                if (item.Attributes.ContainsKey("show-only"))
                {
                    var filter = item.Attributes["show-only"].Split('=').Select(x => x.Trim()).ToList();
                    if (conf[filter[0]] != filter[1])
                    {
                        FilterLevel = level;
                        return true;
                    }
                    else
                    {
                        FilterLevel = -1;
                        return false;
                    }
                }
            }

            FilterLevel = -1;
            return false;
        }
    }
}
