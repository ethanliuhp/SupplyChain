namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI
{
    partial class VGWBSDetailSelectorBak
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VGWBSDetailSelector));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGWBSName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCodeEnd = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtCodeBegin = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel21 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtHandlePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.customLabel17 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel18 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colGWBSDetailName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCostItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colDgMasterSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDgMasterCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterGWBSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterActualBeginDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterActualEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterTaskContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterFinishPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterAnalysis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lnkNone = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.lnkAll = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.lblRecordTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSearchGWBS = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnSearchGWBS);
            this.pnlFloor.Controls.Add(this.lblRecordTotal);
            this.pnlFloor.Controls.Add(this.lnkNone);
            this.pnlFloor.Controls.Add(this.lnkAll);
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.dgMaster);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(879, 458);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgMaster, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lnkAll, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lnkNone, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecordTotal, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSearchGWBS, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(127, 6);
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.Text = "工程任务引用";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtGWBSName);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.txtCodeEnd);
            this.groupBox1.Controls.Add(this.txtCodeBegin);
            this.groupBox1.Controls.Add(this.customLabel21);
            this.groupBox1.Controls.Add(this.txtHandlePerson);
            this.groupBox1.Controls.Add(this.customLabel17);
            this.groupBox1.Controls.Add(this.customLabel18);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(855, 65);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // txtGWBSName
            // 
            this.txtGWBSName.BackColor = System.Drawing.SystemColors.Control;
            this.txtGWBSName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtGWBSName.DrawSelf = false;
            this.txtGWBSName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtGWBSName.EnterToTab = false;
            this.txtGWBSName.Location = new System.Drawing.Point(101, 38);
            this.txtGWBSName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtGWBSName.Name = "txtGWBSName";
            this.txtGWBSName.Padding = new System.Windows.Forms.Padding(1);
            this.txtGWBSName.ReadOnly = false;
            this.txtGWBSName.Size = new System.Drawing.Size(104, 16);
            this.txtGWBSName.TabIndex = 91;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(586, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(15, 42);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(83, 12);
            this.customLabel2.TabIndex = 90;
            this.customLabel2.Text = "工程任务名称:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCodeEnd
            // 
            this.txtCodeEnd.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeEnd.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeEnd.DrawSelf = false;
            this.txtCodeEnd.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeEnd.EnterToTab = false;
            this.txtCodeEnd.Location = new System.Drawing.Point(228, 13);
            this.txtCodeEnd.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeEnd.Name = "txtCodeEnd";
            this.txtCodeEnd.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeEnd.ReadOnly = false;
            this.txtCodeEnd.Size = new System.Drawing.Size(104, 16);
            this.txtCodeEnd.TabIndex = 2;
            // 
            // txtCodeBegin
            // 
            this.txtCodeBegin.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBegin.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBegin.DrawSelf = false;
            this.txtCodeBegin.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBegin.EnterToTab = false;
            this.txtCodeBegin.Location = new System.Drawing.Point(101, 13);
            this.txtCodeBegin.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBegin.Name = "txtCodeBegin";
            this.txtCodeBegin.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBegin.ReadOnly = false;
            this.txtCodeBegin.Size = new System.Drawing.Size(104, 16);
            this.txtCodeBegin.TabIndex = 1;
            // 
            // customLabel21
            // 
            this.customLabel21.AutoSize = true;
            this.customLabel21.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel21.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel21.Location = new System.Drawing.Point(367, 42);
            this.customLabel21.Name = "customLabel21";
            this.customLabel21.Size = new System.Drawing.Size(47, 12);
            this.customLabel21.TabIndex = 88;
            this.customLabel21.Text = "责任人:";
            this.customLabel21.UnderLineColor = System.Drawing.Color.Red;
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
            this.txtHandlePerson.Location = new System.Drawing.Point(420, 38);
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Result = ((System.Collections.IList)(resources.GetObject("txtHandlePerson.Result")));
            this.txtHandlePerson.RightMouse = false;
            this.txtHandlePerson.Size = new System.Drawing.Size(100, 21);
            this.txtHandlePerson.TabIndex = 5;
            this.txtHandlePerson.Tag = null;
            this.txtHandlePerson.Value = "";
            this.txtHandlePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel17
            // 
            this.customLabel17.AutoSize = true;
            this.customLabel17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel17.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel17.Location = new System.Drawing.Point(211, 17);
            this.customLabel17.Name = "customLabel17";
            this.customLabel17.Size = new System.Drawing.Size(11, 12);
            this.customLabel17.TabIndex = 52;
            this.customLabel17.Text = "-";
            this.customLabel17.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel18
            // 
            this.customLabel18.AutoSize = true;
            this.customLabel18.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel18.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel18.Location = new System.Drawing.Point(39, 17);
            this.customLabel18.Name = "customLabel18";
            this.customLabel18.Size = new System.Drawing.Size(59, 12);
            this.customLabel18.TabIndex = 53;
            this.customLabel18.Text = "周计划号:";
            this.customLabel18.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(355, 17);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 80;
            this.customLabel1.Text = "制单日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(420, 10);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 3;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(552, 10);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 4;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(535, 17);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 83;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
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
            this.dgDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colGWBSDetailName,
            this.colPart,
            this.colMaterial,
            this.colMethod,
            this.colDescript,
            this.colCostItemName,
            this.colOrg});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(12, 251);
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
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetail.Size = new System.Drawing.Size(855, 175);
            this.dgDetail.TabIndex = 4;
            this.dgDetail.TargetType = null;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.ReadOnly = true;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 53;
            // 
            // colGWBSDetailName
            // 
            this.colGWBSDetailName.HeaderText = "工程任务明细";
            this.colGWBSDetailName.Name = "colGWBSDetailName";
            this.colGWBSDetailName.Width = 101;
            // 
            // colPart
            // 
            this.colPart.HeaderText = "部位";
            this.colPart.Name = "colPart";
            this.colPart.Width = 53;
            // 
            // colMaterial
            // 
            this.colMaterial.HeaderText = "材料";
            this.colMaterial.Name = "colMaterial";
            this.colMaterial.Width = 53;
            // 
            // colMethod
            // 
            this.colMethod.HeaderText = "做法";
            this.colMethod.Name = "colMethod";
            this.colMethod.Width = 53;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "说明";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 53;
            // 
            // colCostItemName
            // 
            this.colCostItemName.HeaderText = "成本项名称";
            this.colCostItemName.Name = "colCostItemName";
            this.colCostItemName.Width = 89;
            // 
            // colOrg
            // 
            this.colOrg.HeaderText = "承担组织";
            this.colOrg.Name = "colOrg";
            this.colOrg.Width = 77;
            // 
            // dgMaster
            // 
            this.dgMaster.AddDefaultMenu = false;
            this.dgMaster.AddNoColumn = false;
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.AllowUserToDeleteRows = false;
            this.dgMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgMaster.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgMaster.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.ColumnHeadersHeight = 24;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDgMasterSelect,
            this.colDgMasterCode,
            this.colDgMasterGWBSName,
            this.colDgMasterActualBeginDate,
            this.colDgMasterActualEndDate,
            this.colDgMasterTaskContent,
            this.colDgMasterFinishPercent,
            this.colDgMasterAnalysis});
            this.dgMaster.CustomBackColor = false;
            this.dgMaster.EditCellBackColor = System.Drawing.Color.White;
            this.dgMaster.EnableHeadersVisualStyles = false;
            this.dgMaster.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgMaster.FreezeFirstRow = false;
            this.dgMaster.FreezeLastRow = false;
            this.dgMaster.FrontColumnCount = 0;
            this.dgMaster.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgMaster.IsAllowOrder = true;
            this.dgMaster.IsConfirmDelete = true;
            this.dgMaster.Location = new System.Drawing.Point(12, 95);
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
            this.dgMaster.Size = new System.Drawing.Size(855, 150);
            this.dgMaster.TabIndex = 5;
            this.dgMaster.TargetType = null;
            // 
            // colDgMasterSelect
            // 
            this.colDgMasterSelect.HeaderText = "选择";
            this.colDgMasterSelect.Name = "colDgMasterSelect";
            this.colDgMasterSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colDgMasterSelect.Width = 53;
            // 
            // colDgMasterCode
            // 
            this.colDgMasterCode.HeaderText = "单据号";
            this.colDgMasterCode.Name = "colDgMasterCode";
            this.colDgMasterCode.Visible = false;
            this.colDgMasterCode.Width = 66;
            // 
            // colDgMasterGWBSName
            // 
            this.colDgMasterGWBSName.HeaderText = "工程项目任务";
            this.colDgMasterGWBSName.Name = "colDgMasterGWBSName";
            this.colDgMasterGWBSName.Width = 101;
            // 
            // colDgMasterActualBeginDate
            // 
            this.colDgMasterActualBeginDate.HeaderText = "实际开始时间";
            this.colDgMasterActualBeginDate.Name = "colDgMasterActualBeginDate";
            this.colDgMasterActualBeginDate.Width = 101;
            // 
            // colDgMasterActualEndDate
            // 
            this.colDgMasterActualEndDate.HeaderText = "实际结束时间";
            this.colDgMasterActualEndDate.Name = "colDgMasterActualEndDate";
            this.colDgMasterActualEndDate.Width = 101;
            // 
            // colDgMasterTaskContent
            // 
            this.colDgMasterTaskContent.HeaderText = "工作内容描述";
            this.colDgMasterTaskContent.Name = "colDgMasterTaskContent";
            this.colDgMasterTaskContent.Width = 101;
            // 
            // colDgMasterFinishPercent
            // 
            this.colDgMasterFinishPercent.HeaderText = "形象进度(%)";
            this.colDgMasterFinishPercent.Name = "colDgMasterFinishPercent";
            this.colDgMasterFinishPercent.Width = 95;
            // 
            // colDgMasterAnalysis
            // 
            this.colDgMasterAnalysis.HeaderText = "完成情况分析";
            this.colDgMasterAnalysis.Name = "colDgMasterAnalysis";
            this.colDgMasterAnalysis.Width = 101;
            // 
            // lnkNone
            // 
            this.lnkNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkNone.AutoSize = true;
            this.lnkNone.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkNone.Location = new System.Drawing.Point(49, 432);
            this.lnkNone.Name = "lnkNone";
            this.lnkNone.Size = new System.Drawing.Size(29, 12);
            this.lnkNone.TabIndex = 25;
            this.lnkNone.TabStop = true;
            this.lnkNone.Text = "反选";
            // 
            // lnkAll
            // 
            this.lnkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkAll.AutoSize = true;
            this.lnkAll.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkAll.Location = new System.Drawing.Point(12, 432);
            this.lnkAll.Name = "lnkAll";
            this.lnkAll.Size = new System.Drawing.Size(29, 12);
            this.lnkAll.TabIndex = 24;
            this.lnkAll.TabStop = true;
            this.lnkAll.Text = "全选";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(785, 430);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(704, 430);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblRecordTotal
            // 
            this.lblRecordTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecordTotal.AutoSize = true;
            this.lblRecordTotal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecordTotal.ForeColor = System.Drawing.Color.Red;
            this.lblRecordTotal.Location = new System.Drawing.Point(422, 437);
            this.lblRecordTotal.Name = "lblRecordTotal";
            this.lblRecordTotal.Size = new System.Drawing.Size(107, 12);
            this.lblRecordTotal.TabIndex = 100;
            this.lblRecordTotal.Text = "共选择【0】条记录";
            this.lblRecordTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSearchGWBS
            // 
            this.btnSearchGWBS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearchGWBS.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearchGWBS.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearchGWBS.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearchGWBS.Location = new System.Drawing.Point(584, 430);
            this.btnSearchGWBS.Name = "btnSearchGWBS";
            this.btnSearchGWBS.Size = new System.Drawing.Size(114, 23);
            this.btnSearchGWBS.TabIndex = 101;
            this.btnSearchGWBS.Text = "查找工程任务明细";
            this.btnSearchGWBS.UseVisualStyleBackColor = true;
            // 
            // VGWBSDetailSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 458);
            this.MaximizeBox = false;
            this.Name = "VGWBSDetailSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工程任务引用";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel21;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtHandlePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel17;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel18;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lnkNone;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lnkAll;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecordTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtGWBSName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDgMasterSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterGWBSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterActualBeginDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterActualEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterTaskContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterFinishPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterAnalysis;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearchGWBS;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBSDetailName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCostItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrg;

    }
}