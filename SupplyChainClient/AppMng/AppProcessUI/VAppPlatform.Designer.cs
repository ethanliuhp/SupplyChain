namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    partial class VAppPlatform
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VAppPlatform));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BtnQuery = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CboxStatus = new System.Windows.Forms.CheckBox();
            this.DateBeg = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.DateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.CBoxDate = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgBill = new System.Windows.Forms.DataGridView();
            this.collBillName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FgAppSetpsInfo = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.StepOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StepName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppRelations = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.mxInfo = new System.Windows.Forms.TabPage();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.fileInfo = new System.Windows.Forms.TabPage();
            this.btnOpenDocument = new System.Windows.Forms.Button();
            this.btnDownLoadDocument = new System.Windows.Forms.Button();
            this.gridDocument = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.DocumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentCateCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UploadPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UploadDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.BtnDisagree = new System.Windows.Forms.Button();
            this.BtnAppAgree = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.pnlFloor.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgBill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FgAppSetpsInfo)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.mxInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.fileInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDocument)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Controls.Add(this.CBoxDate);
            this.pnlFloor.Controls.Add(this.label2);
            this.pnlFloor.Controls.Add(this.DateEnd);
            this.pnlFloor.Controls.Add(this.DateBeg);
            this.pnlFloor.Controls.Add(this.CboxStatus);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.BtnQuery);
            this.pnlFloor.Size = new System.Drawing.Size(1127, 516);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.BtnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.CboxStatus, 0);
            this.pnlFloor.Controls.SetChildIndex(this.DateBeg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.DateEnd, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.CBoxDate, 0);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(148, 6);
            this.lblTitle.Size = new System.Drawing.Size(93, 20);
            this.lblTitle.Text = "审批平台";
            // 
            // BtnQuery
            // 
            this.BtnQuery.Location = new System.Drawing.Point(338, 6);
            this.BtnQuery.Name = "BtnQuery";
            this.BtnQuery.Size = new System.Drawing.Size(87, 23);
            this.BtnQuery.TabIndex = 30;
            this.BtnQuery.Text = "查询";
            this.BtnQuery.UseVisualStyleBackColor = true;
            this.BtnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(838, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 32;
            this.label1.Text = "当前岗位";
            // 
            // CboxStatus
            // 
            this.CboxStatus.AutoSize = true;
            this.CboxStatus.Location = new System.Drawing.Point(955, 10);
            this.CboxStatus.Name = "CboxStatus";
            this.CboxStatus.Size = new System.Drawing.Size(84, 16);
            this.CboxStatus.TabIndex = 33;
            this.CboxStatus.Text = "包含已结束";
            this.CboxStatus.UseVisualStyleBackColor = true;
            this.CboxStatus.Visible = false;
            // 
            // DateBeg
            // 
            this.DateBeg.Location = new System.Drawing.Point(88, 6);
            this.DateBeg.Name = "DateBeg";
            this.DateBeg.Size = new System.Drawing.Size(113, 21);
            this.DateBeg.TabIndex = 34;
            // 
            // DateEnd
            // 
            this.DateEnd.Location = new System.Drawing.Point(222, 6);
            this.DateEnd.Name = "DateEnd";
            this.DateEnd.Size = new System.Drawing.Size(113, 21);
            this.DateEnd.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "-";
            // 
            // CBoxDate
            // 
            this.CBoxDate.AutoSize = true;
            this.CBoxDate.Location = new System.Drawing.Point(13, 9);
            this.CBoxDate.Name = "CBoxDate";
            this.CBoxDate.Size = new System.Drawing.Size(72, 16);
            this.CBoxDate.TabIndex = 38;
            this.CBoxDate.Text = "提交日期";
            this.CBoxDate.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(4, 30);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgBill);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.FgAppSetpsInfo);
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1111, 474);
            this.splitContainer1.SplitterDistance = 279;
            this.splitContainer1.TabIndex = 39;
            // 
            // dgBill
            // 
            this.dgBill.AllowUserToAddRows = false;
            this.dgBill.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgBill.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgBill.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.collBillName,
            this.colCode,
            this.colCreatePerson,
            this.colCreateDate});
            this.dgBill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgBill.Location = new System.Drawing.Point(0, 0);
            this.dgBill.MultiSelect = false;
            this.dgBill.Name = "dgBill";
            this.dgBill.ReadOnly = true;
            this.dgBill.RowHeadersVisible = false;
            this.dgBill.RowTemplate.Height = 23;
            this.dgBill.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgBill.Size = new System.Drawing.Size(279, 474);
            this.dgBill.TabIndex = 4;
            // 
            // collBillName
            // 
            this.collBillName.HeaderText = "任务类型";
            this.collBillName.Name = "collBillName";
            this.collBillName.ReadOnly = true;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "任务号";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            // 
            // colCreatePerson
            // 
            this.colCreatePerson.HeaderText = "提交人";
            this.colCreatePerson.Name = "colCreatePerson";
            this.colCreatePerson.ReadOnly = true;
            this.colCreatePerson.Width = 70;
            // 
            // colCreateDate
            // 
            this.colCreateDate.HeaderText = "提交日期";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.ReadOnly = true;
            // 
            // FgAppSetpsInfo
            // 
            this.FgAppSetpsInfo.AddDefaultMenu = false;
            this.FgAppSetpsInfo.AddNoColumn = false;
            this.FgAppSetpsInfo.AllowUserToAddRows = false;
            this.FgAppSetpsInfo.AllowUserToDeleteRows = false;
            this.FgAppSetpsInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FgAppSetpsInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.FgAppSetpsInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FgAppSetpsInfo.CellBackColor = System.Drawing.SystemColors.Control;
            this.FgAppSetpsInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.FgAppSetpsInfo.ColumnHeadersHeight = 24;
            this.FgAppSetpsInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.FgAppSetpsInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StepOrder,
            this.StepName,
            this.AppRelations,
            this.AppRole,
            this.AppPerson,
            this.AppDateTime,
            this.AppComments,
            this.AppStatus});
            this.FgAppSetpsInfo.CustomBackColor = false;
            this.FgAppSetpsInfo.EditCellBackColor = System.Drawing.Color.White;
            this.FgAppSetpsInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.FgAppSetpsInfo.EnableHeadersVisualStyles = false;
            this.FgAppSetpsInfo.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.FgAppSetpsInfo.FreezeFirstRow = false;
            this.FgAppSetpsInfo.FreezeLastRow = false;
            this.FgAppSetpsInfo.FrontColumnCount = 0;
            this.FgAppSetpsInfo.GridColor = System.Drawing.SystemColors.WindowText;
            this.FgAppSetpsInfo.HScrollOffset = 0;
            this.FgAppSetpsInfo.IsAllowOrder = true;
            this.FgAppSetpsInfo.IsConfirmDelete = true;
            this.FgAppSetpsInfo.Location = new System.Drawing.Point(8, 282);
            this.FgAppSetpsInfo.Name = "FgAppSetpsInfo";
            this.FgAppSetpsInfo.PageIndex = 0;
            this.FgAppSetpsInfo.PageSize = 0;
            this.FgAppSetpsInfo.Query = null;
            this.FgAppSetpsInfo.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("FgAppSetpsInfo.ReadOnlyCols")));
            this.FgAppSetpsInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.FgAppSetpsInfo.RowHeadersVisible = false;
            this.FgAppSetpsInfo.RowHeadersWidth = 22;
            this.FgAppSetpsInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.FgAppSetpsInfo.RowTemplate.Height = 23;
            this.FgAppSetpsInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FgAppSetpsInfo.Size = new System.Drawing.Size(814, 233);
            this.FgAppSetpsInfo.TabIndex = 44;
            this.FgAppSetpsInfo.TargetType = null;
            this.FgAppSetpsInfo.VScrollOffset = 0;
            // 
            // StepOrder
            // 
            this.StepOrder.HeaderText = "审批步骤";
            this.StepOrder.Name = "StepOrder";
            // 
            // StepName
            // 
            this.StepName.HeaderText = "审批步骤名称";
            this.StepName.Name = "StepName";
            // 
            // AppRelations
            // 
            this.AppRelations.HeaderText = "审批关系";
            this.AppRelations.Name = "AppRelations";
            // 
            // AppRole
            // 
            this.AppRole.HeaderText = "审批角色";
            this.AppRole.Name = "AppRole";
            // 
            // AppPerson
            // 
            this.AppPerson.HeaderText = "审批人";
            this.AppPerson.Name = "AppPerson";
            // 
            // AppDateTime
            // 
            this.AppDateTime.HeaderText = "审批日期";
            this.AppDateTime.Name = "AppDateTime";
            // 
            // AppComments
            // 
            this.AppComments.HeaderText = "审批意见";
            this.AppComments.Name = "AppComments";
            // 
            // AppStatus
            // 
            this.AppStatus.HeaderText = "审批状态";
            this.AppStatus.Name = "AppStatus";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.mxInfo);
            this.tabControl1.Controls.Add(this.fileInfo);
            this.tabControl1.Location = new System.Drawing.Point(9, 91);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(816, 189);
            this.tabControl1.TabIndex = 43;
            // 
            // mxInfo
            // 
            this.mxInfo.Controls.Add(this.dgDetail);
            this.mxInfo.Location = new System.Drawing.Point(4, 21);
            this.mxInfo.Name = "mxInfo";
            this.mxInfo.Padding = new System.Windows.Forms.Padding(3);
            this.mxInfo.Size = new System.Drawing.Size(808, 164);
            this.mxInfo.TabIndex = 0;
            this.mxInfo.Text = "明细信息";
            this.mxInfo.UseVisualStyleBackColor = true;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(3, 3);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.PageIndex = 0;
            this.dgDetail.PageSize = 0;
            this.dgDetail.Query = null;
            this.dgDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDetail.ReadOnlyCols")));
            this.dgDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.RowHeadersVisible = false;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.Size = new System.Drawing.Size(802, 158);
            this.dgDetail.TabIndex = 6;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // fileInfo
            // 
            this.fileInfo.Controls.Add(this.btnOpenDocument);
            this.fileInfo.Controls.Add(this.btnDownLoadDocument);
            this.fileInfo.Controls.Add(this.gridDocument);
            this.fileInfo.Location = new System.Drawing.Point(4, 21);
            this.fileInfo.Name = "fileInfo";
            this.fileInfo.Padding = new System.Windows.Forms.Padding(3);
            this.fileInfo.Size = new System.Drawing.Size(256, 164);
            this.fileInfo.TabIndex = 1;
            this.fileInfo.Text = "文档信息";
            this.fileInfo.UseVisualStyleBackColor = true;
            // 
            // btnOpenDocument
            // 
            this.btnOpenDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenDocument.Location = new System.Drawing.Point(619, 105);
            this.btnOpenDocument.Name = "btnOpenDocument";
            this.btnOpenDocument.Size = new System.Drawing.Size(75, 23);
            this.btnOpenDocument.TabIndex = 216;
            this.btnOpenDocument.Text = "预览";
            this.btnOpenDocument.UseVisualStyleBackColor = true;
            // 
            // btnDownLoadDocument
            // 
            this.btnDownLoadDocument.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDownLoadDocument.Location = new System.Drawing.Point(619, 72);
            this.btnDownLoadDocument.Name = "btnDownLoadDocument";
            this.btnDownLoadDocument.Size = new System.Drawing.Size(75, 23);
            this.btnDownLoadDocument.TabIndex = 215;
            this.btnDownLoadDocument.Text = "下载";
            this.btnDownLoadDocument.UseVisualStyleBackColor = true;
            // 
            // gridDocument
            // 
            this.gridDocument.AddDefaultMenu = false;
            this.gridDocument.AddNoColumn = true;
            this.gridDocument.AllowUserToAddRows = false;
            this.gridDocument.AllowUserToDeleteRows = false;
            this.gridDocument.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gridDocument.BackgroundColor = System.Drawing.Color.White;
            this.gridDocument.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridDocument.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridDocument.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDocument.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDocument.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DocumentName,
            this.DocumentCode,
            this.DocumentCateCode,
            this.DocumentDesc,
            this.UploadPerson,
            this.UploadDate});
            this.gridDocument.CustomBackColor = false;
            this.gridDocument.EditCellBackColor = System.Drawing.Color.White;
            this.gridDocument.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridDocument.FreezeFirstRow = false;
            this.gridDocument.FreezeLastRow = false;
            this.gridDocument.FrontColumnCount = 0;
            this.gridDocument.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridDocument.HScrollOffset = 0;
            this.gridDocument.IsAllowOrder = true;
            this.gridDocument.IsConfirmDelete = true;
            this.gridDocument.Location = new System.Drawing.Point(3, 3);
            this.gridDocument.Name = "gridDocument";
            this.gridDocument.PageIndex = 0;
            this.gridDocument.PageSize = 0;
            this.gridDocument.Query = null;
            this.gridDocument.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridDocument.ReadOnlyCols")));
            this.gridDocument.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDocument.RowHeadersWidth = 22;
            this.gridDocument.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridDocument.RowTemplate.Height = 23;
            this.gridDocument.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDocument.Size = new System.Drawing.Size(610, 157);
            this.gridDocument.TabIndex = 203;
            this.gridDocument.TargetType = null;
            this.gridDocument.VScrollOffset = 0;
            // 
            // DocumentName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.DocumentName.DefaultCellStyle = dataGridViewCellStyle1;
            this.DocumentName.HeaderText = "文档名称";
            this.DocumentName.Name = "DocumentName";
            this.DocumentName.Width = 130;
            // 
            // DocumentCode
            // 
            this.DocumentCode.HeaderText = "文档代码";
            this.DocumentCode.Name = "DocumentCode";
            // 
            // DocumentCateCode
            // 
            this.DocumentCateCode.HeaderText = "文档分类编码";
            this.DocumentCateCode.Name = "DocumentCateCode";
            // 
            // DocumentDesc
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.DocumentDesc.DefaultCellStyle = dataGridViewCellStyle2;
            this.DocumentDesc.FillWeight = 80F;
            this.DocumentDesc.HeaderText = "内容描述";
            this.DocumentDesc.Name = "DocumentDesc";
            this.DocumentDesc.ReadOnly = true;
            this.DocumentDesc.Width = 250;
            // 
            // UploadPerson
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.UploadPerson.DefaultCellStyle = dataGridViewCellStyle3;
            this.UploadPerson.FillWeight = 80F;
            this.UploadPerson.HeaderText = "上传人";
            this.UploadPerson.Name = "UploadPerson";
            this.UploadPerson.ReadOnly = true;
            this.UploadPerson.Visible = false;
            this.UploadPerson.Width = 70;
            // 
            // UploadDate
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.UploadDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.UploadDate.HeaderText = "上传日期";
            this.UploadDate.Name = "UploadDate";
            this.UploadDate.ReadOnly = true;
            this.UploadDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.UploadDate.Visible = false;
            this.UploadDate.Width = 110;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.Controls.Add(this.BtnDisagree);
            this.groupBox4.Controls.Add(this.BtnAppAgree);
            this.groupBox4.Location = new System.Drawing.Point(18, 433);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(734, 37);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 45;
            this.label3.Text = "审批意见:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(91, 9);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(453, 27);
            this.textBox1.TabIndex = 44;
            // 
            // BtnDisagree
            // 
            this.BtnDisagree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnDisagree.Enabled = false;
            this.BtnDisagree.Location = new System.Drawing.Point(625, 11);
            this.BtnDisagree.Name = "BtnDisagree";
            this.BtnDisagree.Size = new System.Drawing.Size(84, 26);
            this.BtnDisagree.TabIndex = 43;
            this.BtnDisagree.Text = "审批不同意";
            this.BtnDisagree.UseVisualStyleBackColor = true;
            // 
            // BtnAppAgree
            // 
            this.BtnAppAgree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnAppAgree.Enabled = false;
            this.BtnAppAgree.Location = new System.Drawing.Point(550, 11);
            this.BtnAppAgree.Name = "BtnAppAgree";
            this.BtnAppAgree.Size = new System.Drawing.Size(69, 26);
            this.BtnAppAgree.TabIndex = 42;
            this.BtnAppAgree.Text = "审批通过";
            this.BtnAppAgree.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgMaster);
            this.groupBox1.Location = new System.Drawing.Point(9, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(816, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "主表信息";
            // 
            // dgMaster
            // 
            this.dgMaster.AddDefaultMenu = false;
            this.dgMaster.AddNoColumn = false;
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.AllowUserToDeleteRows = false;
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgMaster.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.ColumnHeadersHeight = 24;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgMaster.CustomBackColor = false;
            this.dgMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMaster.EditCellBackColor = System.Drawing.Color.White;
            this.dgMaster.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgMaster.EnableHeadersVisualStyles = false;
            this.dgMaster.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgMaster.FreezeFirstRow = false;
            this.dgMaster.FreezeLastRow = false;
            this.dgMaster.FrontColumnCount = 0;
            this.dgMaster.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgMaster.HScrollOffset = 0;
            this.dgMaster.IsAllowOrder = true;
            this.dgMaster.IsConfirmDelete = true;
            this.dgMaster.Location = new System.Drawing.Point(3, 17);
            this.dgMaster.Name = "dgMaster";
            this.dgMaster.PageIndex = 0;
            this.dgMaster.PageSize = 0;
            this.dgMaster.Query = null;
            this.dgMaster.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgMaster.ReadOnlyCols")));
            this.dgMaster.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.RowHeadersVisible = false;
            this.dgMaster.RowHeadersWidth = 22;
            this.dgMaster.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMaster.RowTemplate.Height = 23;
            this.dgMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMaster.Size = new System.Drawing.Size(810, 61);
            this.dgMaster.TabIndex = 4;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // VAppPlatform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1127, 516);
            this.Name = "VAppPlatform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "审批表单定义";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgBill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FgAppSetpsInfo)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.mxInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.fileInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDocument)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CboxStatus;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker DateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker DateBeg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CBoxDate;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgBill;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn collBillName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button BtnDisagree;
        private System.Windows.Forms.Button BtnAppAgree;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage mxInfo;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.TabPage fileInfo;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridDocument;
        private System.Windows.Forms.Button btnOpenDocument;
        private System.Windows.Forms.Button btnDownLoadDocument;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView FgAppSetpsInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn StepOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn StepName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppRelations;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppComments;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentCateCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn UploadPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn UploadDate;

    }
}