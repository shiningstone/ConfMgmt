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
            this.pfileCtrlView = new ConfViews.ConfFileController();
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
            // productFileListView1
            // 
            this.pfileCtrlView.Location = new System.Drawing.Point(19, 14);
            this.pfileCtrlView.Margin = new System.Windows.Forms.Padding(4);
            this.pfileCtrlView.Name = "productFileListView1";
            this.pfileCtrlView.Size = new System.Drawing.Size(651, 36);
            this.pfileCtrlView.TabIndex = 24;
            // 
            // ProductMgmtView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pfileCtrlView);
            this.Controls.Add(this.confView1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProductMgmtView";
            this.Size = new System.Drawing.Size(964, 588);
            this.ResumeLayout(false);

        }

        #endregion

        private ConfViews.ConfView confView1;
        private ConfViews.ConfFileController pfileCtrlView;
    }
}
