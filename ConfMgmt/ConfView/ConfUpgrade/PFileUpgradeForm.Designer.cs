namespace ConfViews.Upgrade
{
    partial class PFileUpgradeForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.tbPFileTemplate = new System.Windows.Forms.TextBox();
            this.tbOrigDir = new System.Windows.Forms.TextBox();
            this.tbNewDir = new System.Windows.Forms.TextBox();
            this.btnSelectTemplate = new System.Windows.Forms.Button();
            this.btnSelectOrigDir = new System.Windows.Forms.Button();
            this.btnSelectNewDir = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "产品档模板";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "原始目录";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "生成目录";
            // 
            // tbPFileTemplate
            // 
            this.tbPFileTemplate.Location = new System.Drawing.Point(112, 28);
            this.tbPFileTemplate.Name = "tbPFileTemplate";
            this.tbPFileTemplate.Size = new System.Drawing.Size(588, 25);
            this.tbPFileTemplate.TabIndex = 3;
            // 
            // tbOrigDir
            // 
            this.tbOrigDir.Location = new System.Drawing.Point(112, 63);
            this.tbOrigDir.Name = "tbOrigDir";
            this.tbOrigDir.Size = new System.Drawing.Size(588, 25);
            this.tbOrigDir.TabIndex = 4;
            // 
            // tbNewDir
            // 
            this.tbNewDir.Location = new System.Drawing.Point(112, 102);
            this.tbNewDir.Name = "tbNewDir";
            this.tbNewDir.Size = new System.Drawing.Size(588, 25);
            this.tbNewDir.TabIndex = 5;
            // 
            // btnSelectTemplate
            // 
            this.btnSelectTemplate.Location = new System.Drawing.Point(716, 23);
            this.btnSelectTemplate.Name = "btnSelectTemplate";
            this.btnSelectTemplate.Size = new System.Drawing.Size(114, 31);
            this.btnSelectTemplate.TabIndex = 6;
            this.btnSelectTemplate.Text = "选择文件";
            this.btnSelectTemplate.UseVisualStyleBackColor = true;
            this.btnSelectTemplate.Click += new System.EventHandler(this.btnSelectTemplate_Click);
            // 
            // btnSelectOrigDir
            // 
            this.btnSelectOrigDir.Location = new System.Drawing.Point(716, 60);
            this.btnSelectOrigDir.Name = "btnSelectOrigDir";
            this.btnSelectOrigDir.Size = new System.Drawing.Size(114, 31);
            this.btnSelectOrigDir.TabIndex = 7;
            this.btnSelectOrigDir.Text = "选择目录";
            this.btnSelectOrigDir.UseVisualStyleBackColor = true;
            this.btnSelectOrigDir.Click += new System.EventHandler(this.btnSelectOrigDir_Click);
            // 
            // btnSelectNewDir
            // 
            this.btnSelectNewDir.Location = new System.Drawing.Point(716, 97);
            this.btnSelectNewDir.Name = "btnSelectNewDir";
            this.btnSelectNewDir.Size = new System.Drawing.Size(114, 31);
            this.btnSelectNewDir.TabIndex = 8;
            this.btnSelectNewDir.Text = "选择目录";
            this.btnSelectNewDir.UseVisualStyleBackColor = true;
            this.btnSelectNewDir.Click += new System.EventHandler(this.btnSelectNewDir_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(15, 195);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tbLog.Size = new System.Drawing.Size(815, 351);
            this.tbLog.TabIndex = 9;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(244, 150);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 10;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(467, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PFileUpgradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 558);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btnSelectNewDir);
            this.Controls.Add(this.btnSelectOrigDir);
            this.Controls.Add(this.btnSelectTemplate);
            this.Controls.Add(this.tbNewDir);
            this.Controls.Add(this.tbOrigDir);
            this.Controls.Add(this.tbPFileTemplate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PFileUpgradeForm";
            this.Text = "产品档升级";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbPFileTemplate;
        private System.Windows.Forms.TextBox tbOrigDir;
        private System.Windows.Forms.TextBox tbNewDir;
        private System.Windows.Forms.Button btnSelectTemplate;
        private System.Windows.Forms.Button btnSelectOrigDir;
        private System.Windows.Forms.Button btnSelectNewDir;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
    }
}