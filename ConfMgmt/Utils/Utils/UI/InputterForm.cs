using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils.UI
{
    public partial class InputterForm : Form
    {
        public abstract class Strategy
        {
            public abstract void Layout(InputterForm form);
            public abstract void GetInput(InputterForm form);
        }
        public class StrategyTextBox : Strategy
        {
            public override void Layout(InputterForm form)
            {
                form.TB_Value.Visible = true;
                form.CMB_Values.Visible = false;
            }
            public override void GetInput(InputterForm form)
            {
                try
                {
                    form.Value = double.Parse(form.TB_Value.Text);
                }
                catch (Exception ex)
                {
                    form.Value = double.NaN;
                }
            }
        }
        public class StrategyComboBox : Strategy
        {
            public override void Layout(InputterForm form)
            {
                form.TB_Value.Visible = false;
                form.CMB_Values.Visible = true;
            }
            public override void GetInput(InputterForm form)
            {
                form.Choice = form.CMB_Values.SelectedItem.ToString();
            }
        }
        public double Value = double.NaN;
        public string Choice;
        private Strategy _strategy;
        public InputterForm()
        {
            InitializeComponent();
        }
        public InputterForm(string paramName, string initVal, string paramUnit)
        {
            InitializeComponent();

            LB_ParamName.Text = paramName;
            LB_ParamUnit.Text = paramUnit;
            TB_Value.Text = initVal;

            _strategy = new StrategyTextBox();
            _strategy.Layout(this);
        }

        public InputterForm(string paramName, List<string> values, string paramUnit = null)
        {
            InitializeComponent();

            LB_ParamName.Text = paramName;
            LB_ParamUnit.Text = paramUnit;

            CMB_Values.DataSource = values;
            CMB_Values.SelectedIndex = 0;

            _strategy = new StrategyComboBox();
            _strategy.Layout(this);
        }

        private void BTN_Ok_Click(object sender, EventArgs e)
        {
            _strategy.GetInput(this);
            Close();
        }

        private void BTN_Cancel_Click(object sender, EventArgs e)
        {
            Value = double.NaN;
            Choice = null;

            Close();
        }
    }
}
