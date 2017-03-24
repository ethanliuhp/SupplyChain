namespace Application.Business.Erp.SupplyChain.Client.FileUpload
{
    partial class VDocumentFileModify
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
            this.btnBatchUpload = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.txtFilePath);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.btnQuit);
            this.pnlFloor.Controls.Add(this.btnBatchUpload);
            this.pnlFloor.Controls.Add(this.btnSelectFile);
            this.pnlFloor.Size = new System.Drawing.Size(559, 91);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSelectFile, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnBatchUpload, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuit, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtFilePath, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 45);
            // 
            // btnBatchUpload
            // 
            this.btnBatchUpload.Location = new System.Drawing.Point(382, 47);
            this.btnBatchUpload.Name = "btnBatchUpload";
            this.btnBatchUpload.Size = new System.Drawing.Size(72, 23);
            this.btnBatchUpload.TabIndex = 27;
            this.btnBatchUpload.Text = "确定";
            this.btnBatchUpload.UseVisualStyleBackColor = true;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(474, 12);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(72, 23);
            this.btnSelectFile.TabIndex = 23;
            this.btnSelectFile.Text = "浏览...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(474, 47);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(72, 23);
            this.btnQuit.TabIndex = 29;
            this.btnQuit.Text = "放弃";
            this.btnQuit.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 30;
            this.label1.Text = "文件地址：";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(83, 13);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(384, 21);
            this.txtFilePath.TabIndex = 31;
            // 
            // VDocumentFileModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 91);
            this.Name = "VDocumentFileModify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件修改";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBatchUpload;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label1;

    }
}