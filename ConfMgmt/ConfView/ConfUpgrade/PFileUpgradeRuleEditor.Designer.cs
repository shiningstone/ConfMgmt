namespace ConfViews.Upgrade
{
    partial class PFileUpgradeRuleEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.dgvAdditions = new System.Windows.Forms.DataGridView();
            this.lvMissings = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditions)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "废弃项";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(327, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "新增项";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(723, 513);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dgvAdditions
            // 
            this.dgvAdditions.AllowUserToAddRows = false;
            this.dgvAdditions.AllowUserToDeleteRows = false;
            this.dgvAdditions.AllowUserToResizeRows = false;
            this.dgvAdditions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdditions.Location = new System.Drawing.Point(315, 62);
            this.dgvAdditions.Name = "dgvAdditions";
            this.dgvAdditions.RowHeadersVisible = false;
            this.dgvAdditions.RowHeadersWidth = 51;
            this.dgvAdditions.RowTemplate.Height = 27;
            this.dgvAdditions.Size = new System.Drawing.Size(514, 431);
            this.dgvAdditions.TabIndex = 6;
            // 
            // lvMissings
            // 
            this.lvMissings.GridLines = true;
            this.lvMissings.HideSelection = false;
            this.lvMissings.Location = new System.Drawing.Point(12, 62);
            this.lvMissings.Name = "lvMissings";
            this.lvMissings.Size = new System.Drawing.Size(220, 431);
            this.lvMissings.TabIndex = 7;
            this.lvMissings.UseCompatibleStateImageBehavior = false;
            // 
            // PFileUpgradeRuleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 548);
            this.Controls.Add(this.lvMissings);
            this.Controls.Add(this.dgvAdditions);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PFileUpgradeRuleEditor";
            this.Text = "产品档比较结果";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DataGridView dgvAdditions;
        private System.Windows.Forms.ListView lvMissings;
    }
}