namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    partial class VResourcesDemandManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VResourcesDemandManagement));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mnuRollingDemandPlan = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemRDPShow = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRDPAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRDPUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRDPDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRDPInvalid = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuResourceRequireReceipt = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemRRRShow = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRRRAddTotal = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRRRPeriod = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRRRUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRRRDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRRRMerger = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProjectInfoName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gridRollingDemandPlan = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colMasterPlanName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMasterResponsiblePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMasterState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMasterCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridResourceRequireReceipt = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.ReceiptPlanType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiptName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiptResponsiblePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiptCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiptState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiptPlanStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiptPlanEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtHandlePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.cmbState = new System.Windows.Forms.ComboBox();
            this.cmbPlanType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlFloor.SuspendLayout();
            this.mnuRollingDemandPlan.SuspendLayout();
            this.mnuResourceRequireReceipt.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRollingDemandPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridResourceRequireReceipt)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Size = new System.Drawing.Size(841, 567);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // mnuRollingDemandPlan
            // 
            this.mnuRollingDemandPlan.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemRDPShow,
            this.itemRDPAdd,
            this.itemRDPUpdate,
            this.itemRDPDelete,
            this.itemRDPInvalid});
            this.mnuRollingDemandPlan.Name = "mnuRollingDemandPlan";
            this.mnuRollingDemandPlan.Size = new System.Drawing.Size(95, 114);
            // 
            // itemRDPShow
            // 
            this.itemRDPShow.Name = "itemRDPShow";
            this.itemRDPShow.Size = new System.Drawing.Size(94, 22);
            this.itemRDPShow.Text = "显示";
            // 
            // itemRDPAdd
            // 
            this.itemRDPAdd.Name = "itemRDPAdd";
            this.itemRDPAdd.Size = new System.Drawing.Size(94, 22);
            this.itemRDPAdd.Text = "新增";
            // 
            // itemRDPUpdate
            // 
            this.itemRDPUpdate.Name = "itemRDPUpdate";
            this.itemRDPUpdate.Size = new System.Drawing.Size(94, 22);
            this.itemRDPUpdate.Text = "修改";
            // 
            // itemRDPDelete
            // 
            this.itemRDPDelete.Name = "itemRDPDelete";
            this.itemRDPDelete.Size = new System.Drawing.Size(94, 22);
            this.itemRDPDelete.Text = "删除";
            // 
            // itemRDPInvalid
            // 
            this.itemRDPInvalid.Name = "itemRDPInvalid";
            this.itemRDPInvalid.Size = new System.Drawing.Size(94, 22);
            this.itemRDPInvalid.Text = "作废";
            // 
            // mnuResourceRequireReceipt
            // 
            this.mnuResourceRequireReceipt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemRRRShow,
            this.itemRRRAddTotal,
            this.itemRRRPeriod,
            this.itemRRRUpdate,
            this.itemRRRDelete,
            this.itemRRRMerger});
            this.mnuResourceRequireReceipt.Name = "contextMenuStrip2";
            this.mnuResourceRequireReceipt.Size = new System.Drawing.Size(155, 136);
            // 
            // itemRRRShow
            // 
            this.itemRRRShow.Name = "itemRRRShow";
            this.itemRRRShow.Size = new System.Drawing.Size(154, 22);
            this.itemRRRShow.Text = "显示";
            // 
            // itemRRRAddTotal
            // 
            this.itemRRRAddTotal.Name = "itemRRRAddTotal";
            this.itemRRRAddTotal.Size = new System.Drawing.Size(154, 22);
            this.itemRRRAddTotal.Text = "新增总量需求单";
            // 
            // itemRRRPeriod
            // 
            this.itemRRRPeriod.Name = "itemRRRPeriod";
            this.itemRRRPeriod.Size = new System.Drawing.Size(154, 22);
            this.itemRRRPeriod.Text = "新增期间需求单";
            // 
            // itemRRRUpdate
            // 
            this.itemRRRUpdate.Name = "itemRRRUpdate";
            this.itemRRRUpdate.Size = new System.Drawing.Size(154, 22);
            this.itemRRRUpdate.Text = "修改";
            // 
            // itemRRRDelete
            // 
            this.itemRRRDelete.Name = "itemRRRDelete";
            this.itemRRRDelete.Size = new System.Drawing.Size(154, 22);
            this.itemRRRDelete.Text = "删除";
            // 
            // itemRRRMerger
            // 
            this.itemRRRMerger.Name = "itemRRRMerger";
            this.itemRRRMerger.Size = new System.Drawing.Size(154, 22);
            this.itemRRRMerger.Text = "合并";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtProjectInfoName);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.gridRollingDemandPlan);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridResourceRequireReceipt);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Size = new System.Drawing.Size(841, 567);
            this.splitContainer1.SplitterDistance = 247;
            this.splitContainer1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "滚动需求计划：";
            // 
            // txtProjectInfoName
            // 
            this.txtProjectInfoName.Location = new System.Drawing.Point(83, 12);
            this.txtProjectInfoName.Name = "txtProjectInfoName";
            this.txtProjectInfoName.ReadOnly = true;
            this.txtProjectInfoName.Size = new System.Drawing.Size(746, 21);
            this.txtProjectInfoName.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "项目名称：";
            // 
            // gridRollingDemandPlan
            // 
            this.gridRollingDemandPlan.AddDefaultMenu = false;
            this.gridRollingDemandPlan.AddNoColumn = true;
            this.gridRollingDemandPlan.AllowUserToAddRows = false;
            this.gridRollingDemandPlan.AllowUserToDeleteRows = false;
            this.gridRollingDemandPlan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridRollingDemandPlan.BackgroundColor = System.Drawing.Color.White;
            this.gridRollingDemandPlan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridRollingDemandPlan.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridRollingDemandPlan.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridRollingDemandPlan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRollingDemandPlan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMasterPlanName,
            this.colMasterResponsiblePerson,
            this.colMasterState,
            this.colMasterCreateTime});
            this.gridRollingDemandPlan.CustomBackColor = false;
            this.gridRollingDemandPlan.EditCellBackColor = System.Drawing.Color.White;
            this.gridRollingDemandPlan.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridRollingDemandPlan.FreezeFirstRow = false;
            this.gridRollingDemandPlan.FreezeLastRow = false;
            this.gridRollingDemandPlan.FrontColumnCount = 0;
            this.gridRollingDemandPlan.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridRollingDemandPlan.HScrollOffset = 0;
            this.gridRollingDemandPlan.IsAllowOrder = true;
            this.gridRollingDemandPlan.IsConfirmDelete = true;
            this.gridRollingDemandPlan.Location = new System.Drawing.Point(14, 62);
            this.gridRollingDemandPlan.MultiSelect = false;
            this.gridRollingDemandPlan.Name = "gridRollingDemandPlan";
            this.gridRollingDemandPlan.PageIndex = 0;
            this.gridRollingDemandPlan.PageSize = 0;
            this.gridRollingDemandPlan.Query = null;
            this.gridRollingDemandPlan.ReadOnly = true;
            this.gridRollingDemandPlan.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridRollingDemandPlan.ReadOnlyCols")));
            this.gridRollingDemandPlan.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridRollingDemandPlan.RowHeadersVisible = false;
            this.gridRollingDemandPlan.RowHeadersWidth = 22;
            this.gridRollingDemandPlan.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridRollingDemandPlan.RowTemplate.Height = 23;
            this.gridRollingDemandPlan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRollingDemandPlan.Size = new System.Drawing.Size(815, 180);
            this.gridRollingDemandPlan.TabIndex = 23;
            this.gridRollingDemandPlan.TargetType = null;
            this.gridRollingDemandPlan.VScrollOffset = 0;
            // 
            // colMasterPlanName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.colMasterPlanName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colMasterPlanName.HeaderText = "计划名称";
            this.colMasterPlanName.Name = "colMasterPlanName";
            this.colMasterPlanName.ReadOnly = true;
            this.colMasterPlanName.Width = 120;
            // 
            // colMasterResponsiblePerson
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colMasterResponsiblePerson.DefaultCellStyle = dataGridViewCellStyle2;
            this.colMasterResponsiblePerson.HeaderText = "责任人";
            this.colMasterResponsiblePerson.Name = "colMasterResponsiblePerson";
            this.colMasterResponsiblePerson.ReadOnly = true;
            this.colMasterResponsiblePerson.Width = 70;
            // 
            // colMasterState
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colMasterState.DefaultCellStyle = dataGridViewCellStyle3;
            this.colMasterState.HeaderText = "状态";
            this.colMasterState.Name = "colMasterState";
            this.colMasterState.ReadOnly = true;
            this.colMasterState.Width = 70;
            // 
            // colMasterCreateTime
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.colMasterCreateTime.DefaultCellStyle = dataGridViewCellStyle4;
            this.colMasterCreateTime.HeaderText = "创建时间";
            this.colMasterCreateTime.Name = "colMasterCreateTime";
            this.colMasterCreateTime.Width = 120;
            // 
            // gridResourceRequireReceipt
            // 
            this.gridResourceRequireReceipt.AddDefaultMenu = false;
            this.gridResourceRequireReceipt.AddNoColumn = true;
            this.gridResourceRequireReceipt.AllowUserToAddRows = false;
            this.gridResourceRequireReceipt.AllowUserToDeleteRows = false;
            this.gridResourceRequireReceipt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridResourceRequireReceipt.BackgroundColor = System.Drawing.Color.White;
            this.gridResourceRequireReceipt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridResourceRequireReceipt.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridResourceRequireReceipt.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridResourceRequireReceipt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResourceRequireReceipt.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReceiptPlanType,
            this.ReceiptName,
            this.ReceiptResponsiblePerson,
            this.ReceiptCreateTime,
            this.ReceiptState,
            this.ReceiptPlanStartTime,
            this.ReceiptPlanEndTime});
            this.gridResourceRequireReceipt.CustomBackColor = false;
            this.gridResourceRequireReceipt.EditCellBackColor = System.Drawing.Color.White;
            this.gridResourceRequireReceipt.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridResourceRequireReceipt.FreezeFirstRow = false;
            this.gridResourceRequireReceipt.FreezeLastRow = false;
            this.gridResourceRequireReceipt.FrontColumnCount = 0;
            this.gridResourceRequireReceipt.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridResourceRequireReceipt.HScrollOffset = 0;
            this.gridResourceRequireReceipt.IsAllowOrder = true;
            this.gridResourceRequireReceipt.IsConfirmDelete = true;
            this.gridResourceRequireReceipt.Location = new System.Drawing.Point(10, 72);
            this.gridResourceRequireReceipt.Name = "gridResourceRequireReceipt";
            this.gridResourceRequireReceipt.PageIndex = 0;
            this.gridResourceRequireReceipt.PageSize = 0;
            this.gridResourceRequireReceipt.Query = null;
            this.gridResourceRequireReceipt.ReadOnly = true;
            this.gridResourceRequireReceipt.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridResourceRequireReceipt.ReadOnlyCols")));
            this.gridResourceRequireReceipt.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridResourceRequireReceipt.RowHeadersVisible = false;
            this.gridResourceRequireReceipt.RowHeadersWidth = 22;
            this.gridResourceRequireReceipt.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridResourceRequireReceipt.RowTemplate.Height = 23;
            this.gridResourceRequireReceipt.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResourceRequireReceipt.Size = new System.Drawing.Size(817, 226);
            this.gridResourceRequireReceipt.TabIndex = 32;
            this.gridResourceRequireReceipt.TargetType = null;
            this.gridResourceRequireReceipt.VScrollOffset = 0;
            // 
            // ReceiptPlanType
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.ReceiptPlanType.DefaultCellStyle = dataGridViewCellStyle5;
            this.ReceiptPlanType.HeaderText = "计划类型";
            this.ReceiptPlanType.Name = "ReceiptPlanType";
            // 
            // ReceiptName
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.ReceiptName.DefaultCellStyle = dataGridViewCellStyle6;
            this.ReceiptName.HeaderText = "名称";
            this.ReceiptName.Name = "ReceiptName";
            // 
            // ReceiptResponsiblePerson
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.ReceiptResponsiblePerson.DefaultCellStyle = dataGridViewCellStyle7;
            this.ReceiptResponsiblePerson.HeaderText = "责任人";
            this.ReceiptResponsiblePerson.Name = "ReceiptResponsiblePerson";
            // 
            // ReceiptCreateTime
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            this.ReceiptCreateTime.DefaultCellStyle = dataGridViewCellStyle8;
            this.ReceiptCreateTime.HeaderText = "创建时间";
            this.ReceiptCreateTime.Name = "ReceiptCreateTime";
            // 
            // ReceiptState
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            this.ReceiptState.DefaultCellStyle = dataGridViewCellStyle9;
            this.ReceiptState.HeaderText = "状态";
            this.ReceiptState.Name = "ReceiptState";
            // 
            // ReceiptPlanStartTime
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            this.ReceiptPlanStartTime.DefaultCellStyle = dataGridViewCellStyle10;
            this.ReceiptPlanStartTime.HeaderText = "计划开始时间";
            this.ReceiptPlanStartTime.Name = "ReceiptPlanStartTime";
            // 
            // ReceiptPlanEndTime
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            this.ReceiptPlanEndTime.DefaultCellStyle = dataGridViewCellStyle11;
            this.ReceiptPlanEndTime.HeaderText = "计划结束时间";
            this.ReceiptPlanEndTime.Name = "ReceiptPlanEndTime";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtHandlePerson);
            this.groupBox1.Controls.Add(this.cmbState);
            this.groupBox1.Controls.Add(this.cmbPlanType);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnFilter);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(10, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(815, 45);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            // 
            // txtHandlePerson
            // 
            this.txtHandlePerson.AcceptsEscape = false;
            this.txtHandlePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtHandlePerson.Code = null;
            this.txtHandlePerson.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtHandlePerson.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtHandlePerson.EnterToTab = false;
            this.txtHandlePerson.Id = "";
            this.txtHandlePerson.IsAllLoad = true;
            this.txtHandlePerson.Location = new System.Drawing.Point(535, 15);
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Result = ((System.Collections.IList)(resources.GetObject("txtHandlePerson.Result")));
            this.txtHandlePerson.RightMouse = false;
            this.txtHandlePerson.Size = new System.Drawing.Size(145, 21);
            this.txtHandlePerson.TabIndex = 22;
            this.txtHandlePerson.Tag = null;
            this.txtHandlePerson.Value = "";
            this.txtHandlePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // cmbState
            // 
            this.cmbState.FormattingEnabled = true;
            this.cmbState.Location = new System.Drawing.Point(314, 16);
            this.cmbState.Name = "cmbState";
            this.cmbState.Size = new System.Drawing.Size(145, 20);
            this.cmbState.TabIndex = 21;
            // 
            // cmbPlanType
            // 
            this.cmbPlanType.FormattingEnabled = true;
            this.cmbPlanType.Location = new System.Drawing.Point(81, 16);
            this.cmbPlanType.Name = "cmbPlanType";
            this.cmbPlanType.Size = new System.Drawing.Size(145, 20);
            this.cmbPlanType.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(243, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "计划状态：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "计划类型：";
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(697, 15);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(106, 23);
            this.btnFilter.TabIndex = 19;
            this.btnFilter.Text = "过滤需求计划单";
            this.btnFilter.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(476, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "责任人：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 30;
            this.label6.Text = "需求计划单：";
            // 
            // VResourcesDemandManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 567);
            this.Name = "VResourcesDemandManagement";
            this.Text = "资源需求管理";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.mnuRollingDemandPlan.ResumeLayout(false);
            this.mnuResourceRequireReceipt.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRollingDemandPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridResourceRequireReceipt)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuRollingDemandPlan;
        private System.Windows.Forms.ContextMenuStrip mnuResourceRequireReceipt;
        private System.Windows.Forms.ToolStripMenuItem itemRDPShow;
        private System.Windows.Forms.ToolStripMenuItem itemRDPAdd;
        private System.Windows.Forms.ToolStripMenuItem itemRDPUpdate;
        private System.Windows.Forms.ToolStripMenuItem itemRDPDelete;
        private System.Windows.Forms.ToolStripMenuItem itemRRRShow;
        private System.Windows.Forms.ToolStripMenuItem itemRRRAddTotal;
        private System.Windows.Forms.ToolStripMenuItem itemRRRPeriod;
        private System.Windows.Forms.ToolStripMenuItem itemRRRUpdate;
        private System.Windows.Forms.ToolStripMenuItem itemRRRDelete;
        private System.Windows.Forms.ToolStripMenuItem itemRRRMerger;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProjectInfoName;
        private System.Windows.Forms.Label label1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridRollingDemandPlan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMasterPlanName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMasterResponsiblePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMasterState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMasterCreateTime;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridResourceRequireReceipt;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiptPlanType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiptName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiptResponsiblePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiptCreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiptState;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiptPlanStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiptPlanEndTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtHandlePerson;
        private System.Windows.Forms.ComboBox cmbState;
        private System.Windows.Forms.ComboBox cmbPlanType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem itemRDPInvalid;
    }
}