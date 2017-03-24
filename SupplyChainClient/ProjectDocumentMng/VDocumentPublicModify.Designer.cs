﻿namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    partial class VDocumentPublicModify
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDocumentPublicModify));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDocumentCate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbSecurityLevel = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtDocumentExplain = new System.Windows.Forms.TextBox();
            this.txtDocumentAuthor = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.cmbDocumentStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDocumentInforType = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtDocumentKeywords = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtDocumentTitle = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtDocumentCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtDocumentName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtResideProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnAddFile = new System.Windows.Forms.Button();
            this.btnClearSelected = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.gridFiles = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.btnQuit);
            this.pnlFloor.Controls.Add(this.btnSave);
            this.pnlFloor.Size = new System.Drawing.Size(756, 561);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSave, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuit, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 0);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtDocumentCate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbSecurityLevel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtDocumentExplain);
            this.groupBox1.Controls.Add(this.txtDocumentAuthor);
            this.groupBox1.Controls.Add(this.cmbDocumentStatus);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbDocumentInforType);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txtDocumentKeywords);
            this.groupBox1.Controls.Add(this.txtDocumentTitle);
            this.groupBox1.Controls.Add(this.txtDocumentCode);
            this.groupBox1.Controls.Add(this.txtDocumentName);
            this.groupBox1.Controls.Add(this.txtResideProject);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(732, 217);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文档信息";
            // 
            // txtDocumentCate
            // 
            this.txtDocumentCate.BackColor = System.Drawing.SystemColors.Control;
            this.txtDocumentCate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDocumentCate.DrawSelf = false;
            this.txtDocumentCate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDocumentCate.EnterToTab = false;
            this.txtDocumentCate.Location = new System.Drawing.Point(102, 104);
            this.txtDocumentCate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDocumentCate.Name = "txtDocumentCate";
            this.txtDocumentCate.Padding = new System.Windows.Forms.Padding(1);
            this.txtDocumentCate.ReadOnly = false;
            this.txtDocumentCate.Size = new System.Drawing.Size(606, 16);
            this.txtDocumentCate.TabIndex = 226;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 225;
            this.label6.Text = "文档分类名称：";
            // 
            // cmbSecurityLevel
            // 
            this.cmbSecurityLevel.FormattingEnabled = true;
            this.cmbSecurityLevel.Location = new System.Drawing.Point(568, 71);
            this.cmbSecurityLevel.Name = "cmbSecurityLevel";
            this.cmbSecurityLevel.Size = new System.Drawing.Size(140, 20);
            this.cmbSecurityLevel.TabIndex = 224;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(497, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 223;
            this.label4.Text = "文档密级：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(31, 43);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 206;
            this.label12.Text = "文档作者：";
            // 
            // txtDocumentExplain
            // 
            this.txtDocumentExplain.Location = new System.Drawing.Point(102, 133);
            this.txtDocumentExplain.Multiline = true;
            this.txtDocumentExplain.Name = "txtDocumentExplain";
            this.txtDocumentExplain.Size = new System.Drawing.Size(606, 78);
            this.txtDocumentExplain.TabIndex = 219;
            // 
            // txtDocumentAuthor
            // 
            this.txtDocumentAuthor.BackColor = System.Drawing.SystemColors.Control;
            this.txtDocumentAuthor.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDocumentAuthor.DrawSelf = false;
            this.txtDocumentAuthor.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDocumentAuthor.EnterToTab = false;
            this.txtDocumentAuthor.Location = new System.Drawing.Point(102, 41);
            this.txtDocumentAuthor.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDocumentAuthor.Name = "txtDocumentAuthor";
            this.txtDocumentAuthor.Padding = new System.Windows.Forms.Padding(1);
            this.txtDocumentAuthor.ReadOnly = false;
            this.txtDocumentAuthor.Size = new System.Drawing.Size(140, 16);
            this.txtDocumentAuthor.TabIndex = 213;
            // 
            // cmbDocumentStatus
            // 
            this.cmbDocumentStatus.FormattingEnabled = true;
            this.cmbDocumentStatus.Location = new System.Drawing.Point(328, 71);
            this.cmbDocumentStatus.Name = "cmbDocumentStatus";
            this.cmbDocumentStatus.Size = new System.Drawing.Size(140, 20);
            this.cmbDocumentStatus.TabIndex = 218;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(257, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 217;
            this.label1.Text = "文档状态：";
            // 
            // cmbDocumentInforType
            // 
            this.cmbDocumentInforType.FormattingEnabled = true;
            this.cmbDocumentInforType.Location = new System.Drawing.Point(102, 71);
            this.cmbDocumentInforType.Name = "cmbDocumentInforType";
            this.cmbDocumentInforType.Size = new System.Drawing.Size(140, 20);
            this.cmbDocumentInforType.TabIndex = 216;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(7, 75);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(89, 12);
            this.label18.TabIndex = 215;
            this.label18.Text = "文档信息类型：";
            // 
            // txtDocumentKeywords
            // 
            this.txtDocumentKeywords.BackColor = System.Drawing.SystemColors.Control;
            this.txtDocumentKeywords.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDocumentKeywords.DrawSelf = false;
            this.txtDocumentKeywords.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDocumentKeywords.EnterToTab = false;
            this.txtDocumentKeywords.Location = new System.Drawing.Point(568, 41);
            this.txtDocumentKeywords.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDocumentKeywords.Name = "txtDocumentKeywords";
            this.txtDocumentKeywords.Padding = new System.Windows.Forms.Padding(1);
            this.txtDocumentKeywords.ReadOnly = false;
            this.txtDocumentKeywords.Size = new System.Drawing.Size(140, 16);
            this.txtDocumentKeywords.TabIndex = 214;
            // 
            // txtDocumentTitle
            // 
            this.txtDocumentTitle.BackColor = System.Drawing.SystemColors.Control;
            this.txtDocumentTitle.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDocumentTitle.DrawSelf = false;
            this.txtDocumentTitle.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDocumentTitle.EnterToTab = false;
            this.txtDocumentTitle.Location = new System.Drawing.Point(328, 41);
            this.txtDocumentTitle.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDocumentTitle.Name = "txtDocumentTitle";
            this.txtDocumentTitle.Padding = new System.Windows.Forms.Padding(1);
            this.txtDocumentTitle.ReadOnly = false;
            this.txtDocumentTitle.Size = new System.Drawing.Size(140, 16);
            this.txtDocumentTitle.TabIndex = 212;
            // 
            // txtDocumentCode
            // 
            this.txtDocumentCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtDocumentCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDocumentCode.DrawSelf = false;
            this.txtDocumentCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDocumentCode.EnterToTab = false;
            this.txtDocumentCode.Location = new System.Drawing.Point(568, 15);
            this.txtDocumentCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDocumentCode.Name = "txtDocumentCode";
            this.txtDocumentCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtDocumentCode.ReadOnly = false;
            this.txtDocumentCode.Size = new System.Drawing.Size(140, 16);
            this.txtDocumentCode.TabIndex = 211;
            // 
            // txtDocumentName
            // 
            this.txtDocumentName.BackColor = System.Drawing.SystemColors.Control;
            this.txtDocumentName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDocumentName.DrawSelf = false;
            this.txtDocumentName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDocumentName.EnterToTab = false;
            this.txtDocumentName.Location = new System.Drawing.Point(328, 15);
            this.txtDocumentName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDocumentName.Name = "txtDocumentName";
            this.txtDocumentName.Padding = new System.Windows.Forms.Padding(1);
            this.txtDocumentName.ReadOnly = false;
            this.txtDocumentName.Size = new System.Drawing.Size(140, 16);
            this.txtDocumentName.TabIndex = 210;
            // 
            // txtResideProject
            // 
            this.txtResideProject.BackColor = System.Drawing.SystemColors.Control;
            this.txtResideProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtResideProject.DrawSelf = false;
            this.txtResideProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtResideProject.EnterToTab = false;
            this.txtResideProject.Location = new System.Drawing.Point(102, 15);
            this.txtResideProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtResideProject.Name = "txtResideProject";
            this.txtResideProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtResideProject.ReadOnly = false;
            this.txtResideProject.Size = new System.Drawing.Size(140, 16);
            this.txtResideProject.TabIndex = 209;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 202;
            this.label2.Text = "所属项目：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(497, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 204;
            this.label10.Text = "文档代码：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(31, 136);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 208;
            this.label14.Text = "文档说明：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(486, 43);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 207;
            this.label13.Text = "文档关键字：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(258, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 205;
            this.label11.Text = "文档标题：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(258, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 203;
            this.label3.Text = "文档名称：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnSelectFile);
            this.groupBox2.Controls.Add(this.btnAddFile);
            this.groupBox2.Controls.Add(this.btnClearSelected);
            this.groupBox2.Controls.Add(this.btnClearAll);
            this.groupBox2.Controls.Add(this.gridFiles);
            this.groupBox2.Location = new System.Drawing.Point(12, 235);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(732, 293);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文件";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectFile.Location = new System.Drawing.Point(344, 264);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(101, 23);
            this.btnSelectFile.TabIndex = 33;
            this.btnSelectFile.Text = "替换选中文件";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            // 
            // btnAddFile
            // 
            this.btnAddFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddFile.Location = new System.Drawing.Point(9, 264);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(101, 23);
            this.btnAddFile.TabIndex = 32;
            this.btnAddFile.Text = "添加文件";
            this.btnAddFile.UseVisualStyleBackColor = true;
            // 
            // btnClearSelected
            // 
            this.btnClearSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearSelected.Location = new System.Drawing.Point(123, 264);
            this.btnClearSelected.Name = "btnClearSelected";
            this.btnClearSelected.Size = new System.Drawing.Size(101, 23);
            this.btnClearSelected.TabIndex = 30;
            this.btnClearSelected.Text = "删除选中行";
            this.btnClearSelected.UseVisualStyleBackColor = true;
            // 
            // btnClearAll
            // 
            this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearAll.Location = new System.Drawing.Point(237, 264);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(101, 23);
            this.btnClearAll.TabIndex = 31;
            this.btnClearAll.Text = "删除全部";
            this.btnClearAll.UseVisualStyleBackColor = true;
            // 
            // gridFiles
            // 
            this.gridFiles.AddDefaultMenu = false;
            this.gridFiles.AddNoColumn = true;
            this.gridFiles.AllowUserToAddRows = false;
            this.gridFiles.AllowUserToDeleteRows = false;
            this.gridFiles.AllowUserToResizeRows = false;
            this.gridFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
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
            this.gridFiles.Location = new System.Drawing.Point(6, 20);
            this.gridFiles.MultiSelect = false;
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
            this.gridFiles.Size = new System.Drawing.Size(717, 238);
            this.gridFiles.TabIndex = 29;
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
            this.btnQuit.Location = new System.Drawing.Point(651, 534);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 7;
            this.btnQuit.Text = "放弃";
            this.btnQuit.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(570, 534);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // VDocumentPublicModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 561);
            this.Name = "VDocumentPublicModify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文档修改";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbSecurityLevel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtDocumentExplain;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDocumentAuthor;
        private System.Windows.Forms.ComboBox cmbDocumentStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDocumentInforType;
        private System.Windows.Forms.Label label18;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDocumentKeywords;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDocumentTitle;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDocumentCode;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDocumentName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtResideProject;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClearSelected;
        private System.Windows.Forms.Button btnClearAll;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnSave;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDocumentCate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.Button btnSelectFile;

    }
}