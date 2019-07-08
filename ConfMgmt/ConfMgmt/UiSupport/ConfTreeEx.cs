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
        private static int MaxDepth(ConfTree conf)
        {
            return 3;
        }
        public static DataTable ConvertToTable(ConfTree conf)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < MaxDepth(conf); i++)
            {
                table.Columns.Add();
            }

            conf.Visit("ToTable", (item, level) =>
            {
                var row = table.NewRow();
                row[0] = item.Name;
                row[1] = !string.IsNullOrEmpty(item.Value) ? item.Value : "";

                table.Rows.Add(row);
                return false;
            });

            return table;
        }
    }
}
