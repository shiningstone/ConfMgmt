using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JbConf;
using System.IO;

namespace ConfViews
{
    public partial class ConfEditor : UserControl
    {
        public ConfEditor()
        {
            InitializeComponent();
        }

        private string ExtractTopNode(string path)
        {
            var values = path.Split('/').ToList().Skip(2);
            return string.Join("/", values);
        }

        private ConfTree _root;
        private ConfTree _tree;

        public void Bind(ConfTree root, ConfItem item)
        {
            _root = root;
            _tree = item as ConfTree;

            LBL_FileInfo.Text = Path.GetFileNameWithoutExtension(root.XmlDoc.BaseURI);
            LBL_SelectedNode.Text = $"为{ExtractTopNode(item.SelfPath)}添加";

            var tree = item as ConfTree;
            if (tree != null)
            {
                DataTable table;

                var subtree = tree.Sons[0] as ConfTree;

                if (subtree != null)
                {
                    var emptynode = ((subtree as ConfTree).Clone() as ConfTree);
                    emptynode.Clear();
                    table = UiSupport.ConvertToTable(emptynode);
                }
                else
                {
                    table = new DataTable();
                    table.Columns.AddRange(new DataColumn[] {
                        new DataColumn("Item.Name"),
                        new DataColumn("Item.Value"),
                    });
                    var row = table.NewRow();
                    table.Rows.Add(row);

                    DGV_NewNodeInfo.ColumnHeadersVisible = true;
                }

                DGV_NewNodeInfo.DataSource = table;
            }
        }

        private void BTN_Add_Click(object sender, EventArgs e)
        {
            var data = DGV_NewNodeInfo.DataSource as DataTable;

            ConfItem newnode;
            if (data.Columns.Count > 2)//ConfTree
            {
                newnode = _tree.Sons[0].Clone();

                var newtree = newnode as ConfTree;
                newtree.Clear();
                newtree.Name = data.Rows[0][0] as string;

                for (int i = 1; i < data.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(data.Rows[i][1] as string))
                    {
                        newtree[data.Rows[i][1] as string] = data.Rows[i][2] as string;
                    }
                }
            }
            else
            {
                return;
            }

            _tree.AddNode(newnode);
            _root.Save();
        }
    }
}
