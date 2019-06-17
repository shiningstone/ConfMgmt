namespace Utils.UI
{
    partial class ConfigurationEditor
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
            this.BTN_Load = new System.Windows.Forms.Button();
            this.BTN_Save = new System.Windows.Forms.Button();
            this.TBL_1UpDown = new System.Windows.Forms.TableLayoutPanel();
            this.TBL_2LeftRight = new System.Windows.Forms.TableLayoutPanel();
            this.configView1 = new Utils.UI.ConfigView();
            this.TBL_1UpDown.SuspendLayout();
            this.TBL_2LeftRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTN_Load
            // 
            this.BTN_Load.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BTN_Load.Location = new System.Drawing.Point(139, 4);
            this.BTN_Load.Name = "BTN_Load";
            this.BTN_Load.Size = new System.Drawing.Size(75, 22);
            this.BTN_Load.TabIndex = 0;
            this.BTN_Load.Text = "Load";
            this.BTN_Load.UseVisualStyleBackColor = true;
            this.BTN_Load.Click += new System.EventHandler(this.BTN_Load_Click);
            // 
            // BTN_Save
            // 
            this.BTN_Save.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BTN_Save.Location = new System.Drawing.Point(492, 4);
            this.BTN_Save.Name = "BTN_Save";
            this.BTN_Save.Size = new System.Drawing.Size(75, 22);
            this.BTN_Save.TabIndex = 1;
            this.BTN_Save.Text = "Save";
            this.BTN_Save.UseVisualStyleBackColor = true;
            this.BTN_Save.Click += new System.EventHandler(this.BTN_Save_Click);
            // 
            // TBL_1UpDown
            // 
            this.TBL_1UpDown.ColumnCount = 1;
            this.TBL_1UpDown.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_1UpDown.Controls.Add(this.TBL_2LeftRight, 0, 1);
            this.TBL_1UpDown.Controls.Add(this.configView1, 0, 0);
            this.TBL_1UpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBL_1UpDown.Location = new System.Drawing.Point(0, 0);
            this.TBL_1UpDown.Name = "TBL_1UpDown";
            this.TBL_1UpDown.RowCount = 2;
            this.TBL_1UpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.0668F));
            this.TBL_1UpDown.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.933194F));
            this.TBL_1UpDown.Size = new System.Drawing.Size(713, 449);
            this.TBL_1UpDown.TabIndex = 2;
            // 
            // TBL_2LeftRight
            // 
            this.TBL_2LeftRight.ColumnCount = 2;
            this.TBL_2LeftRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2LeftRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2LeftRight.Controls.Add(this.BTN_Load, 0, 0);
            this.TBL_2LeftRight.Controls.Add(this.BTN_Save, 1, 0);
            this.TBL_2LeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBL_2LeftRight.Location = new System.Drawing.Point(3, 416);
            this.TBL_2LeftRight.Name = "TBL_2LeftRight";
            this.TBL_2LeftRight.RowCount = 1;
            this.TBL_2LeftRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TBL_2LeftRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.TBL_2LeftRight.Size = new System.Drawing.Size(707, 30);
            this.TBL_2LeftRight.TabIndex = 0;
            // 
            // configView1
            // 
            this.configView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configView1.Location = new System.Drawing.Point(3, 3);
            this.configView1.Name = "configView1";
            this.configView1.Size = new System.Drawing.Size(707, 407);
            this.configView1.TabIndex = 1;
            // 
            // ConfigurationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 449);
            this.Controls.Add(this.TBL_1UpDown);
            this.Name = "ConfigurationEditor";
            this.Text = "配置编辑器";
            this.TBL_1UpDown.ResumeLayout(false);
            this.TBL_2LeftRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BTN_Load;
        private System.Windows.Forms.Button BTN_Save;
        private System.Windows.Forms.TableLayoutPanel TBL_1UpDown;
        private System.Windows.Forms.TableLayoutPanel TBL_2LeftRight;
        private ConfigView configView1;
    }
}