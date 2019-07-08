using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JbConf
{
    public class UiSupport
    {
        public static DataTable ConvertToTable(ConfTree conf)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < conf.MaxDepth + 1; i++)
            {
                table.Columns.Add();
            }

            conf.Visit("ToTable", (item, level) =>
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

                    row[nodes.Length - 1] = item.Name;
                    row[nodes.Length] = !string.IsNullOrEmpty(item.Value) ? item.Value : "";
                }

                table.Rows.Add(row);

                return false;
            });

            return table;
        }
    }
}
