namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    partial class VDocumentsDownloadOrOpen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDocumentsDownloadOrOpen));
            this.cmsDocumentList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsiDocumentDetail = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsiDownloadDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsiUpDocumentNewVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgDocumentMast = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.DocumentSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDocumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentInforType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateTime = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.colOwnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lnkCheckAllNot = new System.Windows.Forms.LinkLabel();
            this.lnkCheckAll = new System.Windows.Forms.LinkLabel();
            this.btnDocumentDetailDownLoad = new System.Windows.Forms.Button();
            this.btnDocumentDetailShow = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgDocumentDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.FileSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.cmsDocumentList.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDocumentMast)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDocumentDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.splitContainer2);
            this.pnlFloor.Size = new System.Drawing.Size(864, 485);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer2, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 9);
            // 
            // cmsDocumentList
            // 
            this.cmsDocumentList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsiDocumentDetail,
            this.cmsiDownloadDocument,
            this.cmsiUpDocumentNewVersion});
            this.cmsDocumentList.Name = "cmsDocumentList";
            this.cmsDocumentList.Size = new System.Drawing.Size(167, 70);
            // 
            // cmsiDocumentDetail
            // 
            this.cmsiDocumentDetail.Name = "cmsiDocumentDetail";
            this.cmsiDocumentDetail.Size = new System.Drawing.Size(166, 22);
            this.cmsiDocumentDetail.Text = "查看文档详细信息";
            // 
            // cmsiDownloadDocument
            // 
            this.cmsiDownloadDocument.Name = "cmsiDownloadDocument";
            this.cmsiDownloadDocument.Size = new System.Drawing.Size(166, 22);
            this.cmsiDownloadDocument.Text = "下载文档";
            // 
            // cmsiUpDocumentNewVersion
            // 
            this.cmsiUpDocumentNewVersion.Name = "cmsiUpDocumentNewVersion";
            this.cmsiUpDocumentNewVersion.Size = new System.Drawing.Size(166, 22);
            this.cmsiUpDocumentNewVersion.Text = "上传文档新版本";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(864, 485);
            this.splitContainer2.SplitterDistance = 259;
            this.splitContainer2.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgDocumentMast);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(862, 257);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文档";
            // 
            // dgDocumentMast
            // 
            this.dgDocumentMast.AddDefaultMenu = false;
            this.dgDocumentMast.AddNoColumn = true;
            this.dgDocumentMast.AllowUserToAddRows = false;
            this.dgDocumentMast.AllowUserToDeleteRows = false;
            this.dgDocumentMast.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDocumentMast.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDocumentMast.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDocumentMast.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDocumentMast.ColumnHeadersHeight = 24;
            this.dgDocumentMast.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDocumentMast.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DocumentSelect,
            this.colDocumentName,
            this.colDocumentInforType,
            this.colDocumentCode,
            this.colCreateTime,
            this.colOwnerName,
            this.colDocumentState});
            this.dgDocumentMast.CustomBackColor = false;
            this.dgDocumentMast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDocumentMast.EditCellBackColor = System.Drawing.Color.White;
            this.dgDocumentMast.EnableHeadersVisualStyles = false;
            this.dgDocumentMast.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDocumentMast.FreezeFirstRow = false;
            this.dgDocumentMast.FreezeLastRow = false;
            this.dgDocumentMast.FrontColumnCount = 0;
            this.dgDocumentMast.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDocumentMast.HScrollOffset = 0;
            this.dgDocumentMast.IsAllowOrder = true;
            this.dgDocumentMast.IsConfirmDelete = true;
            this.dgDocumentMast.Location = new System.Drawing.Point(3, 17);
            this.dgDocumentMast.MultiSelect = false;
            this.dgDocumentMast.Name = "dgDocumentMast";
            this.dgDocumentMast.PageIndex = 0;
            this.dgDocumentMast.PageSize = 0;
            this.dgDocumentMast.Query = null;
            this.dgDocumentMast.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDocumentMast.ReadOnlyCols")));
            this.dgDocumentMast.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDocumentMast.RowHeadersVisible = false;
            this.dgDocumentMast.RowHeadersWidth = 22;
            this.dgDocumentMast.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDocumentMast.RowTemplate.Height = 23;
            this.dgDocumentMast.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDocumentMast.Size = new System.Drawing.Size(856, 237);
            this.dgDocumentMast.TabIndex = 11;
            this.dgDocumentMast.TargetType = null;
            this.dgDocumentMast.VScrollOffset = 0;
            // 
            // DocumentSelect
            // 
            this.DocumentSelect.HeaderText = "选择";
            this.DocumentSelect.Name = "DocumentSelect";
            this.DocumentSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DocumentSelect.Visible = false;
            this.DocumentSelect.Width = 80;
            // 
            // colDocumentName
            // 
            this.colDocumentName.HeaderText = "文档名称";
            this.colDocumentName.Name = "colDocumentName";
            // 
            // colDocumentInforType
            // 
            this.colDocumentInforType.HeaderText = "文档信息类型";
            this.colDocumentInforType.Name = "colDocumentInforType";
            // 
            // colDocumentCode
            // 
            this.colDocumentCode.HeaderText = "文档代码";
            this.colDocumentCode.Name = "colDocumentCode";
            // 
            // colCreateTime
            // 
            this.colCreateTime.HeaderText = "创建时间";
            this.colCreateTime.Name = "colCreateTime";
            this.colCreateTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCreateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colOwnerName
            // 
            this.colOwnerName.HeaderText = "责任人";
            this.colOwnerName.Name = "colOwnerName";
            // 
            // colDocumentState
            // 
            this.colDocumentState.HeaderText = "文档状态";
            this.colDocumentState.Name = "colDocumentState";
            // 
            // lnkCheckAllNot
            // 
            this.lnkCheckAllNot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkCheckAllNot.AutoSize = true;
            this.lnkCheckAllNot.Location = new System.Drawing.Point(58, 193);
            this.lnkCheckAllNot.Name = "lnkCheckAllNot";
            this.lnkCheckAllNot.Size = new System.Drawing.Size(29, 12);
            this.lnkCheckAllNot.TabIndex = 238;
            this.lnkCheckAllNot.TabStop = true;
            this.lnkCheckAllNot.Text = "反选";
            // 
            // lnkCheckAll
            // 
            this.lnkCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkCheckAll.AutoSize = true;
            this.lnkCheckAll.Location = new System.Drawing.Point(11, 193);
            this.lnkCheckAll.Name = "lnkCheckAll";
            this.lnkCheckAll.Size = new System.Drawing.Size(29, 12);
            this.lnkCheckAll.TabIndex = 237;
            this.lnkCheckAll.TabStop = true;
            this.lnkCheckAll.Text = "全选";
            // 
            // btnDocumentDetailDownLoad
            // 
            this.btnDocumentDetailDownLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDocumentDetailDownLoad.Location = new System.Drawing.Point(776, 188);
            this.btnDocumentDetailDownLoad.Name = "btnDocumentDetailDownLoad";
            this.btnDocumentDetailDownLoad.Size = new System.Drawing.Size(75, 23);
            this.btnDocumentDetailDownLoad.TabIndex = 236;
            this.btnDocumentDetailDownLoad.Text = "下载";
            this.btnDocumentDetailDownLoad.UseVisualStyleBackColor = true;
            // 
            // btnDocumentDetailShow
            // 
            this.btnDocumentDetailShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDocumentDetailShow.Location = new System.Drawing.Point(695, 188);
            this.btnDocumentDetailShow.Name = "btnDocumentDetailShow";
            this.btnDocumentDetailShow.Size = new System.Drawing.Size(75, 23);
            this.btnDocumentDetailShow.TabIndex = 235;
            this.btnDocumentDetailShow.Text = "预览";
            this.btnDocumentDetailShow.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnDocumentDetailDownLoad);
            this.groupBox2.Controls.Add(this.btnDocumentDetailShow);
            this.groupBox2.Controls.Add(this.lnkCheckAllNot);
            this.groupBox2.Controls.Add(this.dgDocumentDetail);
            this.groupBox2.Controls.Add(this.lnkCheckAll);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(859, 217);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文件";
            // 
            // dgDocumentDetail
            // 
            this.dgDocumentDetail.AddDefaultMenu = false;
            this.dgDocumentDetail.AddNoColumn = true;
            this.dgDocumentDetail.AllowUserToAddRows = false;
            this.dgDocumentDetail.AllowUserToDeleteRows = false;
            this.dgDocumentDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDocumentDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDocumentDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDocumentDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDocumentDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDocumentDetail.ColumnHeadersHeight = 24;
            this.dgDocumentDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDocumentDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileSelect,
            this.FileName});
            this.dgDocumentDetail.CustomBackColor = false;
            this.dgDocumentDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDocumentDetail.EnableHeadersVisualStyles = false;
            this.dgDocumentDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDocumentDetail.FreezeFirstRow = false;
            this.dgDocumentDetail.FreezeLastRow = false;
            this.dgDocumentDetail.FrontColumnCount = 0;
            this.dgDocumentDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDocumentDetail.HScrollOffset = 0;
            this.dgDocumentDetail.IsAllowOrder = true;
            this.dgDocumentDetail.IsConfirmDelete = true;
            this.dgDocumentDetail.Location = new System.Drawing.Point(3, 17);
            this.dgDocumentDetail.MultiSelect = false;
            this.dgDocumentDetail.Name = "dgDocumentDetail";
            this.dgDocumentDetail.PageIndex = 0;
            this.dgDocumentDetail.PageSize = 0;
            this.dgDocumentDetail.Query = null;
            this.dgDocumentDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDocumentDetail.ReadOnlyCols")));
            this.dgDocumentDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDocumentDetail.RowHeadersVisible = false;
            this.dgDocumentDetail.RowHeadersWidth = 22;
            this.dgDocumentDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDocumentDetail.RowTemplate.Height = 23;
            this.dgDocumentDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDocumentDetail.Size = new System.Drawing.Size(853, 165);
            this.dgDocumentDetail.TabIndex = 11;
            this.dgDocumentDetail.TargetType = null;
            this.dgDocumentDetail.VScrollOffset = 0;
            // 
            // FileSelect
            // 
            this.FileSelect.HeaderText = "选择";
            this.FileSelect.Name = "FileSelect";
            this.FileSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.FileSelect.Width = 60;
            // 
            // FileName
            // 
            this.FileName.HeaderText = "文件名称";
            this.FileName.Name = "FileName";
            this.FileName.Width = 200;
            // 
            // VDocumentsDownloadOrOpen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 485);
            this.Name = "VDocumentsDownloadOrOpen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "下载/预览";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.cmsDocumentList.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDocumentMast)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDocumentDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cmsDocumentList;
        private System.Windows.Forms.ToolStripMenuItem cmsiDocumentDetail;
        private System.Windows.Forms.ToolStripMenuItem cmsiDownloadDocument;
        private System.Windows.Forms.ToolStripMenuItem cmsiUpDocumentNewVersion;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDocumentMast;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DocumentSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentInforType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentCode;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colCreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOwnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentState;
        private System.Windows.Forms.GroupBox groupBox2;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDocumentDetail;
        private System.Windows.Forms.LinkLabel lnkCheckAllNot;
        private System.Windows.Forms.LinkLabel lnkCheckAll;
        private System.Windows.Forms.Button btnDocumentDetailDownLoad;
        private System.Windows.Forms.Button btnDocumentDetailShow;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FileSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;

    }
}