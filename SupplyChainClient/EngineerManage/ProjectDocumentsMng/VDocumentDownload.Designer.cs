namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    partial class VDocumentDownload
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
            this.lblFiles = new System.Windows.Forms.Label();
            this.txtFilesURL = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.dgvDownList = new System.Windows.Forms.DataGridView();
            this.colDocumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDownloadState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnBeginDownload = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDownList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.txtFilesURL);
            this.pnlFloor.Controls.Add(this.btnQuit);
            this.pnlFloor.Controls.Add(this.btnBeginDownload);
            this.pnlFloor.Controls.Add(this.lblFiles);
            this.pnlFloor.Controls.Add(this.btnOpenFolder);
            this.pnlFloor.Controls.Add(this.btnBrowse);
            this.pnlFloor.Controls.Add(this.dgvDownList);
            this.pnlFloor.Size = new System.Drawing.Size(626, 472);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgvDownList, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnBrowse, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOpenFolder, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblFiles, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnBeginDownload, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuit, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtFilesURL, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -1);
            // 
            // lblFiles
            // 
            this.lblFiles.AutoSize = true;
            this.lblFiles.Location = new System.Drawing.Point(12, 16);
            this.lblFiles.Name = "lblFiles";
            this.lblFiles.Size = new System.Drawing.Size(77, 12);
            this.lblFiles.TabIndex = 1;
            this.lblFiles.Text = "目标文件夹：";
            // 
            // txtFilesURL
            // 
            this.txtFilesURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilesURL.Location = new System.Drawing.Point(84, 12);
            this.txtFilesURL.Name = "txtFilesURL";
            this.txtFilesURL.Size = new System.Drawing.Size(343, 21);
            this.txtFilesURL.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(433, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // dgvDownList
            // 
            this.dgvDownList.AllowUserToAddRows = false;
            this.dgvDownList.AllowUserToDeleteRows = false;
            this.dgvDownList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDownList.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvDownList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvDownList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDownList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDocumentName,
            this.colDownloadState,
            this.colError});
            this.dgvDownList.Location = new System.Drawing.Point(12, 39);
            this.dgvDownList.Name = "dgvDownList";
            this.dgvDownList.RowHeadersVisible = false;
            this.dgvDownList.RowTemplate.Height = 23;
            this.dgvDownList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDownList.Size = new System.Drawing.Size(600, 397);
            this.dgvDownList.TabIndex = 4;
            // 
            // colDocumentName
            // 
            this.colDocumentName.HeaderText = "文档名称";
            this.colDocumentName.Name = "colDocumentName";
            this.colDocumentName.Width = 160;
            // 
            // colDownloadState
            // 
            this.colDownloadState.HeaderText = "下载状态";
            this.colDownloadState.Name = "colDownloadState";
            this.colDownloadState.Width = 140;
            // 
            // colError
            // 
            this.colError.HeaderText = "失败原因";
            this.colError.Name = "colError";
            this.colError.Width = 140;
            // 
            // btnBeginDownload
            // 
            this.btnBeginDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBeginDownload.Location = new System.Drawing.Point(352, 443);
            this.btnBeginDownload.Name = "btnBeginDownload";
            this.btnBeginDownload.Size = new System.Drawing.Size(75, 23);
            this.btnBeginDownload.TabIndex = 5;
            this.btnBeginDownload.Text = "开始下载";
            this.btnBeginDownload.UseVisualStyleBackColor = true;
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.Location = new System.Drawing.Point(468, 442);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 6;
            this.btnQuit.Text = "退出";
            this.btnQuit.UseVisualStyleBackColor = true;
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolder.Location = new System.Drawing.Point(514, 10);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(98, 23);
            this.btnOpenFolder.TabIndex = 3;
            this.btnOpenFolder.Text = "打开目标文件夹";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            // 
            // VDocumentDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 472);
            this.Name = "VDocumentDownload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "下载文档";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDownList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFiles;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilesURL;
        private System.Windows.Forms.DataGridView dgvDownList;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnBeginDownload;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDownloadState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colError;
    }
}