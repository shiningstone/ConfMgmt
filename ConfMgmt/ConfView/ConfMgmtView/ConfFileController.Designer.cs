﻿namespace ConfViews
{
    partial class ConfFileController
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BTN_SaveAs = new System.Windows.Forms.Button();
            this.BTN_Save = new System.Windows.Forms.Button();
            this.LBL_Title = new System.Windows.Forms.Label();
            this.CMB_ProductFileList = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BTN_SaveAs
            // 
            this.BTN_SaveAs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BTN_SaveAs.Location = new System.Drawing.Point(607, 5);
            this.BTN_SaveAs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BTN_SaveAs.Name = "BTN_SaveAs";
            this.BTN_SaveAs.Size = new System.Drawing.Size(121, 25);
            this.BTN_SaveAs.TabIndex = 22;
            this.BTN_SaveAs.Text = "另存为";
            this.BTN_SaveAs.UseVisualStyleBackColor = true;
            this.BTN_SaveAs.Click += new System.EventHandler(this.BTN_SaveAs_Click);
            // 
            // BTN_Save
            // 
            this.BTN_Save.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BTN_Save.Location = new System.Drawing.Point(473, 5);
            this.BTN_Save.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BTN_Save.Name = "BTN_Save";
            this.BTN_Save.Size = new System.Drawing.Size(126, 25);
            this.BTN_Save.TabIndex = 21;
            this.BTN_Save.Text = "保存";
            this.BTN_Save.UseVisualStyleBackColor = true;
            this.BTN_Save.Click += new System.EventHandler(this.BTN_Save_Click);
            // 
            // LBL_Title
            // 
            this.LBL_Title.AutoSize = true;
            this.LBL_Title.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LBL_Title.Location = new System.Drawing.Point(4, 11);
            this.LBL_Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LBL_Title.Name = "LBL_Title";
            this.LBL_Title.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.LBL_Title.Size = new System.Drawing.Size(88, 24);
            this.LBL_Title.TabIndex = 19;
            this.LBL_Title.Text = "配置文件";
            // 
            // CMB_ProductFileList
            // 
            this.CMB_ProductFileList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CMB_ProductFileList.FormattingEnabled = true;
            this.CMB_ProductFileList.Location = new System.Drawing.Point(100, 5);
            this.CMB_ProductFileList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CMB_ProductFileList.Name = "CMB_ProductFileList";
            this.CMB_ProductFileList.Size = new System.Drawing.Size(365, 26);
            this.CMB_ProductFileList.TabIndex = 20;
            this.CMB_ProductFileList.SelectedIndexChanged += new System.EventHandler(this.CMB_ProductFileList_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.54993F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.45007F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanel1.Controls.Add(this.LBL_Title, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.BTN_SaveAs, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.CMB_ProductFileList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.BTN_Save, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(732, 35);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // ConfFileController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ConfFileController";
            this.Size = new System.Drawing.Size(732, 35);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BTN_SaveAs;
        private System.Windows.Forms.Button BTN_Save;
        private System.Windows.Forms.Label LBL_Title;
        private System.Windows.Forms.ComboBox CMB_ProductFileList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
