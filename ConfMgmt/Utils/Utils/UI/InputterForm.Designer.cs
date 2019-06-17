namespace Utils.UI
{
    partial class InputterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LB_ParamName = new System.Windows.Forms.Label();
            this.TB_Value = new System.Windows.Forms.TextBox();
            this.LB_ParamUnit = new System.Windows.Forms.Label();
            this.BTN_Ok = new System.Windows.Forms.Button();
            this.BTN_Cancel = new System.Windows.Forms.Button();
            this.CMB_Values = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // LB_ParamName
            // 
            this.LB_ParamName.AutoSize = true;
            this.LB_ParamName.Location = new System.Drawing.Point(31, 32);
            this.LB_ParamName.Name = "LB_ParamName";
            this.LB_ParamName.Size = new System.Drawing.Size(111, 15);
            this.LB_ParamName.TabIndex = 0;
            this.LB_ParamName.Text = "ParameterName";
            // 
            // TB_Value
            // 
            this.TB_Value.Location = new System.Drawing.Point(205, 26);
            this.TB_Value.Name = "TB_Value";
            this.TB_Value.Size = new System.Drawing.Size(113, 25);
            this.TB_Value.TabIndex = 1;
            this.TB_Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LB_ParamUnit
            // 
            this.LB_ParamUnit.AutoSize = true;
            this.LB_ParamUnit.Location = new System.Drawing.Point(328, 32);
            this.LB_ParamUnit.Name = "LB_ParamUnit";
            this.LB_ParamUnit.Size = new System.Drawing.Size(111, 15);
            this.LB_ParamUnit.TabIndex = 2;
            this.LB_ParamUnit.Text = "ParameterUnit";
            // 
            // BTN_Ok
            // 
            this.BTN_Ok.Location = new System.Drawing.Point(90, 78);
            this.BTN_Ok.Name = "BTN_Ok";
            this.BTN_Ok.Size = new System.Drawing.Size(75, 23);
            this.BTN_Ok.TabIndex = 3;
            this.BTN_Ok.Text = "OK";
            this.BTN_Ok.UseVisualStyleBackColor = true;
            this.BTN_Ok.Click += new System.EventHandler(this.BTN_Ok_Click);
            // 
            // BTN_Cancel
            // 
            this.BTN_Cancel.Location = new System.Drawing.Point(281, 78);
            this.BTN_Cancel.Name = "BTN_Cancel";
            this.BTN_Cancel.Size = new System.Drawing.Size(75, 23);
            this.BTN_Cancel.TabIndex = 4;
            this.BTN_Cancel.Text = "Cancel";
            this.BTN_Cancel.UseVisualStyleBackColor = true;
            this.BTN_Cancel.Click += new System.EventHandler(this.BTN_Cancel_Click);
            // 
            // CMB_Values
            // 
            this.CMB_Values.FormattingEnabled = true;
            this.CMB_Values.Location = new System.Drawing.Point(197, 27);
            this.CMB_Values.Name = "CMB_Values";
            this.CMB_Values.Size = new System.Drawing.Size(121, 23);
            this.CMB_Values.TabIndex = 5;
            // 
            // InputterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(443, 113);
            this.Controls.Add(this.BTN_Cancel);
            this.Controls.Add(this.BTN_Ok);
            this.Controls.Add(this.LB_ParamUnit);
            this.Controls.Add(this.TB_Value);
            this.Controls.Add(this.LB_ParamName);
            this.Controls.Add(this.CMB_Values);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InputterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InputterForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LB_ParamName;
        private System.Windows.Forms.TextBox TB_Value;
        private System.Windows.Forms.Label LB_ParamUnit;
        private System.Windows.Forms.Button BTN_Ok;
        private System.Windows.Forms.Button BTN_Cancel;
        private System.Windows.Forms.ComboBox CMB_Values;
    }
}