using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Dynamic;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Utils;
using JbConf;
using System.Collections;
using Utils.UI;

namespace ConfViews
{
    public partial class ConfPropertyGrid : UserControl
    {
        public ConfPropertyGrid()
        {
            InitializeComponent();
        }

        private ConfTree _conf;
        public void Bind(ConfTree conf)
        {
            _conf = conf;

            var dict = new Dictionary<string, string>();
            foreach (var kv in _conf.AllItems)
            {
                dict[kv.Name] = kv.Value;
            }

            propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter(dict);
        }
        public void Bind(string xmlfile)
        {
            Bind(ConfMgmt.Default.GetTree(xmlfile));
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _conf[e.ChangedItem.Label] = e.ChangedItem.Value as string;
            _conf.Save();
        }
    }
}
