namespace ConfViews
{
    partial class ConfEditForm
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
            this.confEditor1 = new ConfViews.ConfEditor();
            this.SuspendLayout();
            // 
            // confEditor1
            // 
            this.confEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.confEditor1.Location = new System.Drawing.Point(0, 0);
            this.confEditor1.Name = "confEditor1";
            this.confEditor1.Size = new System.Drawing.Size(379, 350);
            this.confEditor1.TabIndex = 0;
            // 
            // ConfEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 350);
            this.Controls.Add(this.confEditor1);
            this.Name = "ConfEditForm";
            this.Text = "CondEditForm";
            this.ResumeLayout(false);

        }

        #endregion

        private ConfEditor confEditor1;
    }
}