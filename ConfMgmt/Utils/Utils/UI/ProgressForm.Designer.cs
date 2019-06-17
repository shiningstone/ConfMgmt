namespace Utils.UI
{
    partial class ProgressForm
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
            this.LB_Status = new System.Windows.Forms.Label();
            this.LB_Info = new System.Windows.Forms.Label();
            this.PB_Percentage = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // LB_Status
            // 
            this.LB_Status.AutoSize = true;
            this.LB_Status.Location = new System.Drawing.Point(12, 25);
            this.LB_Status.Name = "LB_Status";
            this.LB_Status.Size = new System.Drawing.Size(73, 17);
            this.LB_Status.TabIndex = 0;
            this.LB_Status.Text = "LB_Status";
            // 
            // LB_Info
            // 
            this.LB_Info.AutoSize = true;
            this.LB_Info.Location = new System.Drawing.Point(12, 59);
            this.LB_Info.Name = "LB_Info";
            this.LB_Info.Size = new System.Drawing.Size(56, 17);
            this.LB_Info.TabIndex = 1;
            this.LB_Info.Text = "LB_Info";
            // 
            // PB_Percentage
            // 
            this.PB_Percentage.Location = new System.Drawing.Point(15, 93);
            this.PB_Percentage.Name = "PB_Percentage";
            this.PB_Percentage.Size = new System.Drawing.Size(437, 23);
            this.PB_Percentage.TabIndex = 2;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 154);
            this.Controls.Add(this.PB_Percentage);
            this.Controls.Add(this.LB_Info);
            this.Controls.Add(this.LB_Status);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProgressForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgressForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LB_Status;
        private System.Windows.Forms.Label LB_Info;
        private System.Windows.Forms.ProgressBar PB_Percentage;
    }
}