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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DGV_ConfigItems = new System.Windows.Forms.DataGridView();
            this.MNU_TreeOps = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TreeOp_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeOp_Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.MNU_ItemOps = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ItemOp_Remove = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_ConfigItems)).BeginInit();
            this.MNU_TreeOps.SuspendLayout();
            this.MNU_ItemOps.SuspendLayout();
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_ConfigItems.DefaultCellStyle = dataGridViewCellStyle4;
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
            this.DGV_ConfigItems.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_ConfigItems_CellRightClicked);
            this.DGV_ConfigItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_ConfigItems_CellValueChanged);
            this.DGV_ConfigItems.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DGV_ConfigItems_DataBindingComplete);
            // 
            // MNU_TreeOps
            // 
            this.MNU_TreeOps.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MNU_TreeOps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TreeOp_Add,
            this.TreeOp_Remove});
            this.MNU_TreeOps.Name = "MNU_TreeOps";
            this.MNU_TreeOps.Size = new System.Drawing.Size(211, 80);
            // 
            // TreeOp_Add
            // 
            this.TreeOp_Add.Name = "TreeOp_Add";
            this.TreeOp_Add.Size = new System.Drawing.Size(210, 24);
            this.TreeOp_Add.Text = "添加子节点";
            this.TreeOp_Add.Click += new System.EventHandler(this.TreeOp_AddSon_Click);
            // 
            // TreeOp_Remove
            // 
            this.TreeOp_Remove.Name = "TreeOp_Remove";
            this.TreeOp_Remove.Size = new System.Drawing.Size(210, 24);
            this.TreeOp_Remove.Text = "删除本节点";
            this.TreeOp_Remove.Click += new System.EventHandler(this.TreeOp_RemoveThis_Click);
            // 
            // MNU_ItemOps
            // 
            this.MNU_ItemOps.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MNU_ItemOps.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ItemOp_Remove});
            this.MNU_ItemOps.Name = "MNU_ItemOps";
            this.MNU_ItemOps.Size = new System.Drawing.Size(109, 28);
            // 
            // ItemOp_Remove
            // 
            this.ItemOp_Remove.Name = "ItemOp_Remove";
            this.ItemOp_Remove.Size = new System.Drawing.Size(210, 24);
            this.ItemOp_Remove.Text = "删除";
            this.ItemOp_Remove.Click += new System.EventHandler(this.ItemOp_RemoveThis_Click);
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
            this.MNU_TreeOps.ResumeLayout(false);
            this.MNU_ItemOps.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_ConfigItems;
        private System.Windows.Forms.ContextMenuStrip MNU_TreeOps;
        private System.Windows.Forms.ContextMenuStrip MNU_ItemOps;
        private System.Windows.Forms.ToolStripMenuItem ItemOp_Remove;
        private System.Windows.Forms.ToolStripMenuItem TreeOp_Add;
        private System.Windows.Forms.ToolStripMenuItem TreeOp_Remove;
    }
}
