namespace Utils.UI
{
    partial class FileVersionView
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
            this.LV_FileInfos = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // LV_FileInfos
            // 
            this.LV_FileInfos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LV_FileInfos.Location = new System.Drawing.Point(0, 0);
            this.LV_FileInfos.Name = "LV_FileInfos";
            this.LV_FileInfos.Size = new System.Drawing.Size(505, 196);
            this.LV_FileInfos.TabIndex = 0;
            this.LV_FileInfos.UseCompatibleStateImageBehavior = false;
            this.LV_FileInfos.View = System.Windows.Forms.View.Details;
            // 
            // FileVersionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LV_FileInfos);
            this.Name = "FileVersionView";
            this.Size = new System.Drawing.Size(505, 196);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView LV_FileInfos;
    }
}
