namespace Application.Business.Erp.SupplyChain.Client.FileUpload
{
    partial class VDocumentFileUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDocumentFileUpload));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClearSelected = new System.Windows.Forms.Button();
            this.btnBatchUpload = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.gridFiles = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnQuit = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnQuit);
            this.pnlFloor.Controls.Add(this.gridFiles);
            this.pnlFloor.Controls.Add(this.btnClearSelected);
            this.pnlFloor.Controls.Add(this.btnBatchUpload);
            this.pnlFloor.Controls.Add(this.btnSelectFile);
            this.pnlFloor.Controls.Add(this.btnClearAll);
            this.pnlFloor.Size = new System.Drawing.Size(723, 415);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnClearAll, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSelectFile, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnBatchUpload, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnClearSelected, 0);
            this.pnlFloor.Controls.SetChildIndex(this.gridFiles, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuit, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 9);
            // 
            // btnClearSelected
            // 
            this.btnClearSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearSelected.Location = new System.Drawing.Point(12, 380);
            this.btnClearSelected.Name = "btnClearSelected";
            this.btnClearSelected.Size = new System.Drawing.Size(101, 23);
            this.btnClearSelected.TabIndex = 25;
            this.btnClearSelected.Text = "移除选中行";
            this.btnClearSelected.UseVisualStyleBackColor = true;
            // 
            // btnBatchUpload
            // 
            this.btnBatchUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchUpload.Location = new System.Drawing.Point(503, 380);
            this.btnBatchUpload.Name = "btnBatchUpload";
            this.btnBatchUpload.Size = new System.Drawing.Size(101, 23);
            this.btnBatchUpload.TabIndex = 27;
            this.btnBatchUpload.Text = "添加文件";
            this.btnBatchUpload.UseVisualStyleBackColor = true;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(12, 12);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(101, 23);
            this.btnSelectFile.TabIndex = 23;
            this.btnSelectFile.Text = "选择文件";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearAll.Location = new System.Drawing.Point(119, 380);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(101, 23);
            this.btnClearAll.TabIndex = 26;
            this.btnClearAll.Text = "移除全部";
            this.btnClearAll.UseVisualStyleBackColor = true;
            // 
            // gridFiles
            // 
            this.gridFiles.AddDefaultMenu = false;
            this.gridFiles.AddNoColumn = true;
            this.gridFiles.AllowUserToAddRows = false;
            this.gridFiles.AllowUserToDeleteRows = false;
            this.gridFiles.AllowUserToResizeRows = false;
            this.gridFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFiles.BackgroundColor = System.Drawing.Color.White;
            this.gridFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridFiles.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridFiles.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileName,
            this.FileSize,
            this.FilePath});
            this.gridFiles.CustomBackColor = false;
            this.gridFiles.EditCellBackColor = System.Drawing.Color.White;
            this.gridFiles.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridFiles.FreezeFirstRow = false;
            this.gridFiles.FreezeLastRow = false;
            this.gridFiles.FrontColumnCount = 0;
            this.gridFiles.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridFiles.HScrollOffset = 0;
            this.gridFiles.IsAllowOrder = true;
            this.gridFiles.IsConfirmDelete = true;
            this.gridFiles.Location = new System.Drawing.Point(12, 41);
            this.gridFiles.Name = "gridFiles";
            this.gridFiles.PageIndex = 0;
            this.gridFiles.PageSize = 0;
            this.gridFiles.Query = null;
            this.gridFiles.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridFiles.ReadOnlyCols")));
            this.gridFiles.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridFiles.RowHeadersWidth = 22;
            this.gridFiles.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridFiles.RowTemplate.Height = 23;
            this.gridFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFiles.Size = new System.Drawing.Size(699, 333);
            this.gridFiles.TabIndex = 28;
            this.gridFiles.TargetType = null;
            this.gridFiles.VScrollOffset = 0;
            // 
            // FileName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.FileName.DefaultCellStyle = dataGridViewCellStyle1;
            this.FileName.HeaderText = "文件名称";
            this.FileName.Name = "FileName";
            this.FileName.Width = 150;
            // 
            // FileSize
            // 
            this.FileSize.HeaderText = "文件大小";
            this.FileSize.Name = "FileSize";
            // 
            // FilePath
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.FilePath.DefaultCellStyle = dataGridViewCellStyle2;
            this.FilePath.FillWeight = 80F;
            this.FilePath.HeaderText = "文件路径";
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            this.FilePath.Width = 500;
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnQuit.Location = new System.Drawing.Point(610, 380);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(101, 23);
            this.btnQuit.TabIndex = 29;
            this.btnQuit.Text = "放弃";
            this.btnQuit.UseVisualStyleBackColor = true;
            // 
            // VDocumentFileUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 415);
            this.Name = "VDocumentFileUpload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件上传";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClearSelected;
        private System.Windows.Forms.Button btnBatchUpload;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnClearAll;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.Button btnQuit;

    }
}