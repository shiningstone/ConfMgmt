namespace ConfViews
{
    partial class ConfEditor
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
            this.LBL_FileInfo = new System.Windows.Forms.Label();
            this.LBL_SelectedNode = new System.Windows.Forms.Label();
            this.BTN_Add = new System.Windows.Forms.Button();
            this.DGV_NewNodeInfo = new System.Windows.Forms.DataGridView();
            this.BTN_DeleteRow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_NewNodeInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // LBL_FileInfo
            // 
            this.LBL_FileInfo.AutoSize = true;
            this.LBL_FileInfo.Location = new System.Drawing.Point(13, 17);
            this.LBL_FileInfo.Name = "LBL_FileInfo";
            this.LBL_FileInfo.Size = new System.Drawing.Size(71, 15);
            this.LBL_FileInfo.TabIndex = 0;
            this.LBL_FileInfo.Text = "FileInfo";
            // 
            // LBL_SelectedNode
            // 
            this.LBL_SelectedNode.AutoSize = true;
            this.LBL_SelectedNode.Location = new System.Drawing.Point(13, 44);
            this.LBL_SelectedNode.Name = "LBL_SelectedNode";
            this.LBL_SelectedNode.Size = new System.Drawing.Size(103, 15);
            this.LBL_SelectedNode.TabIndex = 1;
            this.LBL_SelectedNode.Text = "SelectedNode";
            // 
            // BTN_Add
            // 
            this.BTN_Add.Location = new System.Drawing.Point(75, 279);
            this.BTN_Add.Name = "BTN_Add";
            this.BTN_Add.Size = new System.Drawing.Size(75, 23);
            this.BTN_Add.TabIndex = 2;
            this.BTN_Add.Text = "添加";
            this.BTN_Add.UseVisualStyleBackColor = true;
            this.BTN_Add.Click += new System.EventHandler(this.BTN_Add_Click);
            // 
            // DGV_NewNodeInfo
            // 
            this.DGV_NewNodeInfo.AllowUserToResizeRows = false;
            this.DGV_NewNodeInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_NewNodeInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGV_NewNodeInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_NewNodeInfo.ColumnHeadersVisible = false;
            this.DGV_NewNodeInfo.Location = new System.Drawing.Point(75, 72);
            this.DGV_NewNodeInfo.Name = "DGV_NewNodeInfo";
            this.DGV_NewNodeInfo.RowHeadersVisible = false;
            this.DGV_NewNodeInfo.RowTemplate.Height = 27;
            this.DGV_NewNodeInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_NewNodeInfo.Size = new System.Drawing.Size(240, 182);
            this.DGV_NewNodeInfo.TabIndex = 4;
            // 
            // BTN_DeleteRow
            // 
            this.BTN_DeleteRow.Enabled = false;
            this.BTN_DeleteRow.Location = new System.Drawing.Point(240, 279);
            this.BTN_DeleteRow.Name = "BTN_DeleteRow";
            this.BTN_DeleteRow.Size = new System.Drawing.Size(75, 23);
            this.BTN_DeleteRow.TabIndex = 5;
            this.BTN_DeleteRow.Text = "删除";
            this.BTN_DeleteRow.UseVisualStyleBackColor = true;
            // 
            // ConfEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BTN_DeleteRow);
            this.Controls.Add(this.DGV_NewNodeInfo);
            this.Controls.Add(this.BTN_Add);
            this.Controls.Add(this.LBL_SelectedNode);
            this.Controls.Add(this.LBL_FileInfo);
            this.Name = "ConfEditor";
            this.Size = new System.Drawing.Size(379, 337);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_NewNodeInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LBL_FileInfo;
        private System.Windows.Forms.Label LBL_SelectedNode;
        private System.Windows.Forms.Button BTN_Add;
        private System.Windows.Forms.DataGridView DGV_NewNodeInfo;
        private System.Windows.Forms.Button BTN_DeleteRow;
    }
}
