namespace PortalIntegrationConsole
{
    partial class FrmSetXML
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
            this.txtXMLFile = new System.Windows.Forms.TextBox();
            this.btnSelXMLFile = new System.Windows.Forms.Button();
            this.btnUserCodeToUpper = new System.Windows.Forms.Button();
            this.btnSelXMLDirFile = new System.Windows.Forms.Button();
            this.txtXMLDirFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户信息文件";
            // 
            // txtXMLFile
            // 
            this.txtXMLFile.Location = new System.Drawing.Point(95, 27);
            this.txtXMLFile.Name = "txtXMLFile";
            this.txtXMLFile.Size = new System.Drawing.Size(445, 21);
            this.txtXMLFile.TabIndex = 1;
            // 
            // btnSelXMLFile
            // 
            this.btnSelXMLFile.Location = new System.Drawing.Point(546, 25);
            this.btnSelXMLFile.Name = "btnSelXMLFile";
            this.btnSelXMLFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelXMLFile.TabIndex = 2;
            this.btnSelXMLFile.Text = "选择";
            this.btnSelXMLFile.UseVisualStyleBackColor = true;
            this.btnSelXMLFile.Click += new System.EventHandler(this.btnSelXMLFile_Click);
            // 
            // btnUserCodeToUpper
            // 
            this.btnUserCodeToUpper.Location = new System.Drawing.Point(66, 91);
            this.btnUserCodeToUpper.Name = "btnUserCodeToUpper";
            this.btnUserCodeToUpper.Size = new System.Drawing.Size(192, 35);
            this.btnUserCodeToUpper.TabIndex = 2;
            this.btnUserCodeToUpper.Text = "用户Code转换成大写";
            this.btnUserCodeToUpper.UseVisualStyleBackColor = true;
            this.btnUserCodeToUpper.Click += new System.EventHandler(this.btnUserCodeToUpper_Click);
            // 
            // btnSelXMLDirFile
            // 
            this.btnSelXMLDirFile.Location = new System.Drawing.Point(546, 52);
            this.btnSelXMLDirFile.Name = "btnSelXMLDirFile";
            this.btnSelXMLDirFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelXMLDirFile.TabIndex = 4;
            this.btnSelXMLDirFile.Text = "选择";
            this.btnSelXMLDirFile.UseVisualStyleBackColor = true;
            this.btnSelXMLDirFile.Click += new System.EventHandler(this.btnSelXMLDirFile_Click);
            // 
            // txtXMLDirFile
            // 
            this.txtXMLDirFile.Location = new System.Drawing.Point(95, 54);
            this.txtXMLDirFile.Name = "txtXMLDirFile";
            this.txtXMLDirFile.Size = new System.Drawing.Size(445, 21);
            this.txtXMLDirFile.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "用户字典文件";
            // 
            // FrmSetXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 456);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelXMLDirFile);
            this.Controls.Add(this.txtXMLDirFile);
            this.Controls.Add(this.btnUserCodeToUpper);
            this.Controls.Add(this.btnSelXMLFile);
            this.Controls.Add(this.txtXMLFile);
            this.Controls.Add(this.label1);
            this.Name = "FrmSetXML";
            this.Text = "设置XML文件信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtXMLFile;
        private System.Windows.Forms.Button btnSelXMLFile;
        private System.Windows.Forms.Button btnUserCodeToUpper;
        private System.Windows.Forms.Button btnSelXMLDirFile;
        private System.Windows.Forms.TextBox txtXMLDirFile;
        private System.Windows.Forms.Label label2;
    }
}