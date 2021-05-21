using JbConf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace ConfViews.Upgrade
{
    public partial class PFileUpgradeRuleEditor : Form
    {
        private static Logger _log = new Logger("PFileUpgradeRuleEditor");

        public List<string> Missings;
        public List<ConfItem> Additions;

        public PFileUpgradeRuleEditor()
        {
            InitializeComponent();
        }
        public PFileUpgradeRuleEditor(List<string> missings, List<ConfItem> additions)
        {
            InitializeComponent();

            Missings = missings;
            Additions = additions;

            Show(Missings, lvMissings);
            Show(Additions, dgvAdditions);
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            Missings = Missings.Except(GetChecked(lvMissings)).ToList();
            Additions = GetNewDefVal(dgvAdditions);

            Close();
        }

        private void Show(List<string> missings, ListView lvMissings)
        {
            lvMissings.CheckBoxes = true;
            lvMissings.View = View.SmallIcon;
            lvMissings.BeginUpdate();
            foreach (var m in missings)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = m;
                lvMissings.Items.Add(lvi);
            }
            lvMissings.EndUpdate();
        }
        private List<string> GetChecked(ListView lvMissings)
        {
            var reserves = new List<string>();
            foreach (ListViewItem i in lvMissings.CheckedItems)
            {
                reserves.Add(i.Text);
            }

            if (reserves.Count > 0)
            {
                _log.Debug($"保留缺失项: {string.Join(",", reserves)}");
            }

            return reserves;
        }

        private void Show(List<ConfItem> additions, DataGridView dgvAdditions)
        {
            var items = new DataTable();
            items.Columns.AddRange(new DataColumn[] {
                new DataColumn("Item"),
                new DataColumn("缺省值"),
            });
            foreach (var a in additions)
            {
                var row = items.NewRow();
                row["Item"] = a.Name;
                row["缺省值"] = a.Value;
                items.Rows.Add(row);
            }

            dgvAdditions.DataSource = items;
        }
        private List<ConfItem> GetNewDefVal(DataGridView dgvAdditions)
        {
            var updates = new List<string>();

            for (int i = 0; i < dgvAdditions.Rows.Count; i++)
            {
                var name = dgvAdditions.Rows[i].Cells[0].Value.ToString();
                var value = dgvAdditions.Rows[i].Cells[1].Value.ToString();

                var item = Additions.Find(x => x.Name == name);
                if (item.Value != value)
                {
                    updates.Add($"{item.Name}({item.Value}->{value})");
                    item.Value = value;
                }
            }

            if (updates.Count > 0)
            {
                _log.Debug($"缺省值修改: {string.Join(";", updates)}");
            }

            return Additions;
        }
    }
}
