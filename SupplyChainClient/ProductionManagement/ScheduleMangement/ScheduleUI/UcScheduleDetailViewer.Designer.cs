namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    partial class UcScheduleDetailViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcScheduleDetailViewer));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbxAllSel = new System.Windows.Forms.CheckBox();
            this.lbFindCount = new System.Windows.Forms.Label();
            this.lbRowCount = new System.Windows.Forms.Label();
            this.txtFindKey = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnFindNext = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.flexGrid = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgDocumentMast = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDocumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentInforType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateTime = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.colOwnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lnkCheckAllNot = new System.Windows.Forms.LinkLabel();
            this.lnkCheckAll = new System.Windows.Forms.LinkLabel();
            this.btnOpenDocument = new System.Windows.Forms.Button();
            this.btnDownLoadDocument = new System.Windows.Forms.Button();
            this.dgDocumentDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.FileSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileExtension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDocumentMast)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDocumentDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(865, 428);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbxAllSel);
            this.tabPage1.Controls.Add(this.lbFindCount);
            this.tabPage1.Controls.Add(this.lbRowCount);
            this.tabPage1.Controls.Add(this.txtFindKey);
            this.tabPage1.Controls.Add(this.btnFindNext);
            this.tabPage1.Controls.Add(this.flexGrid);
            this.tabPage1.Controls.Add(this.customLabel7);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(857, 402);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "计划明细";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbxAllSel
            // 
            this.cbxAllSel.AutoSize = true;
            this.cbxAllSel.BackColor = System.Drawing.Color.LightSteelBlue;
            this.cbxAllSel.Location = new System.Drawing.Point(8, 10);
            this.cbxAllSel.Name = "cbxAllSel";
            this.cbxAllSel.Size = new System.Drawing.Size(90, 16);
            this.cbxAllSel.TabIndex = 168;
            this.cbxAllSel.Text = "全选\\全不选";
            this.cbxAllSel.UseVisualStyleBackColor = false;
            // 
            // lbFindCount
            // 
            this.lbFindCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFindCount.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbFindCount.Location = new System.Drawing.Point(445, 387);
            this.lbFindCount.MinimumSize = new System.Drawing.Size(300, 12);
            this.lbFindCount.Name = "lbFindCount";
            this.lbFindCount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbFindCount.Size = new System.Drawing.Size(406, 12);
            this.lbFindCount.TabIndex = 167;
            // 
            // lbRowCount
            // 
            this.lbRowCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbRowCount.AutoSize = true;
            this.lbRowCount.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRowCount.Location = new System.Drawing.Point(6, 388);
            this.lbRowCount.Name = "lbRowCount";
            this.lbRowCount.Size = new System.Drawing.Size(65, 12);
            this.lbRowCount.TabIndex = 165;
            this.lbRowCount.Text = "共计 0 行";
            // 
            // txtFindKey
            // 
            this.txtFindKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFindKey.BackColor = System.Drawing.SystemColors.Control;
            this.txtFindKey.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtFindKey.DrawSelf = false;
            this.txtFindKey.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtFindKey.EnterToTab = false;
            this.txtFindKey.Location = new System.Drawing.Point(516, 10);
            this.txtFindKey.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtFindKey.Name = "txtFindKey";
            this.txtFindKey.Padding = new System.Windows.Forms.Padding(1);
            this.txtFindKey.ReadOnly = false;
            this.txtFindKey.Size = new System.Drawing.Size(252, 16);
            this.txtFindKey.TabIndex = 164;
            // 
            // btnFindNext
            // 
            this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFindNext.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFindNext.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnFindNext.Location = new System.Drawing.Point(774, 7);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(80, 23);
            this.btnFindNext.TabIndex = 159;
            this.btnFindNext.Text = "下一个";
            this.btnFindNext.UseVisualStyleBackColor = true;
            // 
            // flexGrid
            // 
            this.flexGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flexGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid.CheckedImage")));
            this.flexGrid.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.flexGrid.DefaultRowHeight = ((short)(21));
            this.flexGrid.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGrid.Location = new System.Drawing.Point(6, 36);
            this.flexGrid.Name = "flexGrid";
            this.flexGrid.Size = new System.Drawing.Size(845, 349);
            this.flexGrid.TabIndex = 158;
            this.flexGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid.UncheckedImage")));
            // 
            // customLabel7
            // 
            this.customLabel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(477, 12);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(41, 12);
            this.customLabel7.TabIndex = 163;
            this.customLabel7.Text = "查找：";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(857, 402);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "相关文档";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(851, 396);
            this.panel1.TabIndex = 0;
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
            this.splitContainer2.Size = new System.Drawing.Size(851, 396);
            this.splitContainer2.SplitterDistance = 158;
            this.splitContainer2.TabIndex = 227;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgDocumentMast);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(849, 156);
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
            this.dgDocumentMast.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDocumentMast.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDocumentMast.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDocumentMast.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDocumentMast.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDocumentMast.ColumnHeadersHeight = 24;
            this.dgDocumentMast.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDocumentMast.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.select,
            this.colDocumentName,
            this.colDocumentInforType,
            this.colDocumentCode,
            this.colCreateTime,
            this.colOwnerName,
            this.colDocumentState});
            this.dgDocumentMast.CustomBackColor = false;
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
            this.dgDocumentMast.Location = new System.Drawing.Point(5, 17);
            this.dgDocumentMast.MultiSelect = false;
            this.dgDocumentMast.Name = "dgDocumentMast";
            this.dgDocumentMast.PageIndex = 0;
            this.dgDocumentMast.PageSize = 0;
            this.dgDocumentMast.Query = null;
            this.dgDocumentMast.ReadOnly = true;
            this.dgDocumentMast.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDocumentMast.ReadOnlyCols")));
            this.dgDocumentMast.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDocumentMast.RowHeadersVisible = false;
            this.dgDocumentMast.RowHeadersWidth = 22;
            this.dgDocumentMast.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDocumentMast.RowTemplate.Height = 23;
            this.dgDocumentMast.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDocumentMast.Size = new System.Drawing.Size(840, 133);
            this.dgDocumentMast.TabIndex = 11;
            this.dgDocumentMast.TargetType = null;
            this.dgDocumentMast.VScrollOffset = 0;
            // 
            // select
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.NullValue = false;
            this.select.DefaultCellStyle = dataGridViewCellStyle1;
            this.select.HeaderText = "选择";
            this.select.Name = "select";
            this.select.ReadOnly = true;
            this.select.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.select.Visible = false;
            this.select.Width = 80;
            // 
            // colDocumentName
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colDocumentName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colDocumentName.HeaderText = "文档名称";
            this.colDocumentName.Name = "colDocumentName";
            this.colDocumentName.ReadOnly = true;
            // 
            // colDocumentInforType
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colDocumentInforType.DefaultCellStyle = dataGridViewCellStyle3;
            this.colDocumentInforType.HeaderText = "文档信息类型";
            this.colDocumentInforType.Name = "colDocumentInforType";
            this.colDocumentInforType.ReadOnly = true;
            // 
            // colDocumentCode
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.colDocumentCode.DefaultCellStyle = dataGridViewCellStyle4;
            this.colDocumentCode.HeaderText = "文档代码";
            this.colDocumentCode.Name = "colDocumentCode";
            this.colDocumentCode.ReadOnly = true;
            // 
            // colCreateTime
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.colCreateTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.colCreateTime.HeaderText = "创建时间";
            this.colCreateTime.Name = "colCreateTime";
            this.colCreateTime.ReadOnly = true;
            this.colCreateTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCreateTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colOwnerName
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.colOwnerName.DefaultCellStyle = dataGridViewCellStyle6;
            this.colOwnerName.HeaderText = "责任人";
            this.colOwnerName.Name = "colOwnerName";
            this.colOwnerName.ReadOnly = true;
            // 
            // colDocumentState
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.colDocumentState.DefaultCellStyle = dataGridViewCellStyle7;
            this.colDocumentState.HeaderText = "文档状态";
            this.colDocumentState.Name = "colDocumentState";
            this.colDocumentState.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lnkCheckAllNot);
            this.groupBox2.Controls.Add(this.lnkCheckAll);
            this.groupBox2.Controls.Add(this.btnOpenDocument);
            this.groupBox2.Controls.Add(this.btnDownLoadDocument);
            this.groupBox2.Controls.Add(this.dgDocumentDetail);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(849, 232);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文件";
            // 
            // lnkCheckAllNot
            // 
            this.lnkCheckAllNot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkCheckAllNot.AutoSize = true;
            this.lnkCheckAllNot.Location = new System.Drawing.Point(55, 203);
            this.lnkCheckAllNot.Name = "lnkCheckAllNot";
            this.lnkCheckAllNot.Size = new System.Drawing.Size(29, 12);
            this.lnkCheckAllNot.TabIndex = 232;
            this.lnkCheckAllNot.TabStop = true;
            this.lnkCheckAllNot.Text = "反选";
            // 
            // lnkCheckAll
            // 
            this.lnkCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkCheckAll.AutoSize = true;
            this.lnkCheckAll.Location = new System.Drawing.Point(8, 203);
            this.lnkCheckAll.Name = "lnkCheckAll";
            this.lnkCheckAll.Size = new System.Drawing.Size(29, 12);
            this.lnkCheckAll.TabIndex = 232;
            this.lnkCheckAll.TabStop = true;
            this.lnkCheckAll.Text = "全选";
            // 
            // btnOpenDocument
            // 
            this.btnOpenDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenDocument.Location = new System.Drawing.Point(765, 203);
            this.btnOpenDocument.Name = "btnOpenDocument";
            this.btnOpenDocument.Size = new System.Drawing.Size(75, 23);
            this.btnOpenDocument.TabIndex = 229;
            this.btnOpenDocument.Text = "预览";
            this.btnOpenDocument.UseVisualStyleBackColor = true;
            // 
            // btnDownLoadDocument
            // 
            this.btnDownLoadDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownLoadDocument.Location = new System.Drawing.Point(676, 203);
            this.btnDownLoadDocument.Name = "btnDownLoadDocument";
            this.btnDownLoadDocument.Size = new System.Drawing.Size(75, 23);
            this.btnDownLoadDocument.TabIndex = 227;
            this.btnDownLoadDocument.Text = "下载";
            this.btnDownLoadDocument.UseVisualStyleBackColor = true;
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
            this.FileName,
            this.FileExtension,
            this.FilePath});
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
            this.dgDocumentDetail.Location = new System.Drawing.Point(4, 17);
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
            this.dgDocumentDetail.Size = new System.Drawing.Size(840, 180);
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
            this.FileName.ReadOnly = true;
            this.FileName.Width = 300;
            // 
            // FileExtension
            // 
            this.FileExtension.HeaderText = "文件扩展名";
            this.FileExtension.Name = "FileExtension";
            this.FileExtension.ReadOnly = true;
            this.FileExtension.Visible = false;
            this.FileExtension.Width = 80;
            // 
            // FilePath
            // 
            this.FilePath.HeaderText = "文件部分路径";
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            this.FilePath.Visible = false;
            this.FilePath.Width = 400;
            // 
            // UcScheduleDetailViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "UcScheduleDetailViewer";
            this.Size = new System.Drawing.Size(865, 428);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
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

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lbFindCount;
        private System.Windows.Forms.Label lbRowCount;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtFindKey;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnFindNext;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDocumentMast;
        private System.Windows.Forms.DataGridViewCheckBoxColumn select;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentInforType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentCode;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colCreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOwnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentState;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.LinkLabel lnkCheckAllNot;
        private System.Windows.Forms.LinkLabel lnkCheckAll;
        private System.Windows.Forms.Button btnOpenDocument;
        private System.Windows.Forms.Button btnDownLoadDocument;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDocumentDetail;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FileSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileExtension;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.CheckBox cbxAllSel;
    }
}
