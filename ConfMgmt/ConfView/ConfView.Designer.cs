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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DGV_ConfigItems = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_ConfigItems)).BeginInit();
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
            this.DGV_ConfigItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_ConfigItems_CellValueChanged);
            this.DGV_ConfigItems.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.DGV_ConfigItems_DataBindingComplete);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV_ConfigItems;
    }
}
