namespace ConfViews
{
    partial class ConfMgmtView
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
            this.confView1 = new ConfViews.ConfView();
            this.fileController = new ConfViews.ConfFileController();
            this.CMB_ShowLevel = new System.Windows.Forms.ComboBox();
            this.LBL_Level = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // confView1
            // 
            this.confView1.Location = new System.Drawing.Point(19, 56);
            this.confView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.confView1.Name = "confView1";
            this.confView1.Size = new System.Drawing.Size(933, 522);
            this.confView1.TabIndex = 23;
            // 
            // fileController
            // 
            this.fileController.Location = new System.Drawing.Point(19, 14);
            this.fileController.Margin = new System.Windows.Forms.Padding(4);
            this.fileController.Name = "fileController";
            this.fileController.Size = new System.Drawing.Size(651, 36);
            this.fileController.TabIndex = 24;
            // 
            // CMB_ShowLevel
            // 
            this.CMB_ShowLevel.FormattingEnabled = true;
            this.CMB_ShowLevel.Location = new System.Drawing.Point(829, 18);
            this.CMB_ShowLevel.Name = "CMB_ShowLevel";
            this.CMB_ShowLevel.Size = new System.Drawing.Size(121, 23);
            this.CMB_ShowLevel.TabIndex = 25;
            this.CMB_ShowLevel.SelectedIndexChanged += new System.EventHandler(this.CMB_ShowLevel_SelectedIndexChanged);
            // 
            // LBL_Level
            // 
            this.LBL_Level.AutoSize = true;
            this.LBL_Level.Location = new System.Drawing.Point(749, 24);
            this.LBL_Level.Name = "LBL_Level";
            this.LBL_Level.Size = new System.Drawing.Size(67, 15);
            this.LBL_Level.TabIndex = 26;
            this.LBL_Level.Text = "操作级别";
            // 
            // ConfMgmtView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LBL_Level);
            this.Controls.Add(this.CMB_ShowLevel);
            this.Controls.Add(this.fileController);
            this.Controls.Add(this.confView1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConfMgmtView";
            this.Size = new System.Drawing.Size(964, 588);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConfViews.ConfView confView1;
        private ConfViews.ConfFileController fileController;
        private System.Windows.Forms.ComboBox CMB_ShowLevel;
        private System.Windows.Forms.Label LBL_Level;
    }
}
