namespace ConfViews
{
    partial class ConfView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DGV_ConfigItems = new System.Windows.Forms.DataGridView();
            this.MNU_Add = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MNU_Del = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_ConfigItems)).BeginInit();
            this.MNU_Add.SuspendLayout();
            this.MNU_Del.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGV_ConfigItems
            // 
            this.DGV_ConfigItems.AllowUserToAddRows = false;
            this.DGV_ConfigItems.AllowUserToDeleteRows = false;
            this.DGV_ConfigItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.DGV_ConfigItems.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGV_ConfigItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_ConfigItems.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_ConfigItems.DefaultCellStyle = dataGridViewCellStyle2;
            this.DGV_ConfigItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_ConfigItems.Location = new System.Drawing.Point(0, 0);
            this.DGV_ConfigItems.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DGV_ConfigItems.MultiSelect = false;
            this.DGV_ConfigItems.Name = "DGV_ConfigItems";
            this.DGV_ConfigItems.RowHeadersVisible = false;
            this.DGV_ConfigItems.RowTemplate.Height = 24;
            this.DGV_ConfigItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_ConfigItems.Size = new System.Drawing.Size(178, 116);
            this.DGV_ConfigItems.TabIndex = 0;
            this.DGV_ConfigItems.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_ConfigItems_CellMouseDown);
            this.DGV_ConfigItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_ConfigItems_CellValueChanged);
            this.DGV_ConfigItems.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DGV_ConfigItems_DataBindingComplete);
            // 
            // MNU_Add
            // 
            this.MNU_Add.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MNU_Add.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem});
            this.MNU_Add.Name = "MNU_Add";
            this.MNU_Add.Size = new System.Drawing.Size(109, 28);
            // 
            // MNU_Del
            // 
            this.MNU_Del.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MNU_Del.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.MNU_Del.Name = "MNU_Del";
            this.MNU_Del.Size = new System.Drawing.Size(211, 56);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.添加ToolStripMenuItem.Text = "添加";
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // ConfView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DGV_ConfigItems);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ConfView";
            this.Size = new System.Drawing.Size(178, 116);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_ConfigItems)).EndInit();
            this.MNU_Add.ResumeLayout(false);
            this.MNU_Del.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_ConfigItems;
        private System.Windows.Forms.ContextMenuStrip MNU_Add;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip MNU_Del;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
    }
}
