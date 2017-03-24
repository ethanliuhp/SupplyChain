namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng
{
    partial class VTimeDispatchingSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VTimeDispatchingSelect));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customLabel21 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.txtSupply = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.lblSupplier = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtCodeEnd = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtCodeBegin = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel17 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel18 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colDgMasterCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBearTeam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLaborType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMSubInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterCreatePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAppOpinion = new System.Windows.Forms.DataGridViewLinkColumn();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colproject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectTaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLaborSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealLabor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccountQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantityUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccountPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccountMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPriceUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTeam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBeginDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.dgMaster);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Margin = new System.Windows.Forms.Padding(4);
            this.pnlFloor.Size = new System.Drawing.Size(911, 487);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgMaster, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(106, 10);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Size = new System.Drawing.Size(177, 20);
            this.lblTitle.Text = "零星用工派工核算";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.customLabel21);
            this.groupBox1.Controls.Add(this.txtCreatePerson);
            this.groupBox1.Controls.Add(this.txtSupply);
            this.groupBox1.Controls.Add(this.lblSupplier);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtCodeEnd);
            this.groupBox1.Controls.Add(this.txtCodeBegin);
            this.groupBox1.Controls.Add(this.customLabel17);
            this.groupBox1.Controls.Add(this.customLabel18);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Location = new System.Drawing.Point(11, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(887, 65);
            this.groupBox1.TabIndex = 98;
            this.groupBox1.TabStop = false;
            // 
            // customLabel21
            // 
            this.customLabel21.AutoSize = true;
            this.customLabel21.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel21.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel21.Location = new System.Drawing.Point(337, 42);
            this.customLabel21.Name = "customLabel21";
            this.customLabel21.Size = new System.Drawing.Size(47, 12);
            this.customLabel21.TabIndex = 168;
            this.customLabel21.Text = "制单人:";
            this.customLabel21.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.AcceptsEscape = false;
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.Code = null;
            this.txtCreatePerson.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtCreatePerson.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Id = "";
            this.txtCreatePerson.IsAllLoad = true;
            this.txtCreatePerson.Location = new System.Drawing.Point(390, 37);
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Result = ((System.Collections.IList)(resources.GetObject("txtCreatePerson.Result")));
            this.txtCreatePerson.RightMouse = false;
            this.txtCreatePerson.Size = new System.Drawing.Size(138, 21);
            this.txtCreatePerson.TabIndex = 167;
            this.txtCreatePerson.Tag = null;
            this.txtCreatePerson.Value = "";
            this.txtCreatePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // txtSupply
            // 
            this.txtSupply.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupply.Code = null;
            this.txtSupply.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtSupply.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtSupply.EnterToTab = false;
            this.txtSupply.Id = "";
            this.txtSupply.Location = new System.Drawing.Point(87, 37);
            this.txtSupply.Name = "txtSupply";
            this.txtSupply.Result = ((System.Collections.IList)(resources.GetObject("txtSupply.Result")));
            this.txtSupply.RightMouse = false;
            this.txtSupply.Size = new System.Drawing.Size(156, 21);
            this.txtSupply.TabIndex = 165;
            this.txtSupply.Tag = null;
            this.txtSupply.Value = "";
            this.txtSupply.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplier.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplier.Location = new System.Drawing.Point(17, 42);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(59, 12);
            this.lblSupplier.TabIndex = 166;
            this.lblSupplier.Text = "承担队伍:";
            this.lblSupplier.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(734, 37);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtCodeEnd
            // 
            this.txtCodeEnd.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeEnd.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeEnd.DrawSelf = false;
            this.txtCodeEnd.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeEnd.EnterToTab = false;
            this.txtCodeEnd.Location = new System.Drawing.Point(198, 13);
            this.txtCodeEnd.Margin = new System.Windows.Forms.Padding(4);
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
            this.txtCodeBegin.Location = new System.Drawing.Point(77, 13);
            this.txtCodeBegin.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodeBegin.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBegin.Name = "txtCodeBegin";
            this.txtCodeBegin.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBegin.ReadOnly = false;
            this.txtCodeBegin.Size = new System.Drawing.Size(104, 16);
            this.txtCodeBegin.TabIndex = 1;
            // 
            // customLabel17
            // 
            this.customLabel17.AutoSize = true;
            this.customLabel17.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel17.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel17.Location = new System.Drawing.Point(184, 17);
            this.customLabel17.Name = "customLabel17";
            this.customLabel17.Size = new System.Drawing.Size(11, 12);
            this.customLabel17.TabIndex = 52;
            this.customLabel17.Text = "-";
            this.customLabel17.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel18
            // 
            this.customLabel18.AutoSize = true;
            this.customLabel18.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel18.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel18.Location = new System.Drawing.Point(15, 17);
            this.customLabel18.Name = "customLabel18";
            this.customLabel18.Size = new System.Drawing.Size(47, 12);
            this.customLabel18.TabIndex = 53;
            this.customLabel18.Text = "单据号:";
            this.customLabel18.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(325, 17);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 80;
            this.customLabel1.Text = "业务日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(390, 10);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 3;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(518, 10);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 4;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(502, 17);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 83;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
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
            this.colDgMasterCode,
            this.colDgMasterState,
            this.colBearTeam,
            this.colLaborType,
            this.colDgMasterCreateDate,
            this.colMSubInfo,
            this.colDgMasterCreatePerson,
            this.colRealOperationDate,
            this.colDgMasterDescript,
            this.colAppOpinion});
            this.dgMaster.CustomBackColor = false;
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
            this.dgMaster.Location = new System.Drawing.Point(11, 72);
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
            this.dgMaster.Size = new System.Drawing.Size(887, 150);
            this.dgMaster.TabIndex = 99;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colDgMasterCode
            // 
            this.colDgMasterCode.HeaderText = "单据号";
            this.colDgMasterCode.Name = "colDgMasterCode";
            this.colDgMasterCode.Width = 65;
            // 
            // colDgMasterState
            // 
            this.colDgMasterState.HeaderText = "状态";
            this.colDgMasterState.Name = "colDgMasterState";
            this.colDgMasterState.Width = 53;
            // 
            // colBearTeam
            // 
            this.colBearTeam.HeaderText = "承担队伍";
            this.colBearTeam.Name = "colBearTeam";
            this.colBearTeam.Width = 77;
            // 
            // colLaborType
            // 
            this.colLaborType.HeaderText = "用工类型";
            this.colLaborType.Name = "colLaborType";
            this.colLaborType.Width = 77;
            // 
            // colDgMasterCreateDate
            // 
            this.colDgMasterCreateDate.HeaderText = "业务日期";
            this.colDgMasterCreateDate.Name = "colDgMasterCreateDate";
            this.colDgMasterCreateDate.Width = 77;
            // 
            // colMSubInfo
            // 
            this.colMSubInfo.HeaderText = "分包信息";
            this.colMSubInfo.Name = "colMSubInfo";
            this.colMSubInfo.Width = 77;
            // 
            // colDgMasterCreatePerson
            // 
            this.colDgMasterCreatePerson.HeaderText = "制单人";
            this.colDgMasterCreatePerson.Name = "colDgMasterCreatePerson";
            this.colDgMasterCreatePerson.Width = 65;
            // 
            // colRealOperationDate
            // 
            this.colRealOperationDate.HeaderText = "制单日期";
            this.colRealOperationDate.Name = "colRealOperationDate";
            this.colRealOperationDate.Width = 77;
            // 
            // colDgMasterDescript
            // 
            this.colDgMasterDescript.HeaderText = "备注";
            this.colDgMasterDescript.Name = "colDgMasterDescript";
            this.colDgMasterDescript.Width = 53;
            // 
            // colAppOpinion
            // 
            this.colAppOpinion.HeaderText = "审批意见";
            this.colAppOpinion.Name = "colAppOpinion";
            this.colAppOpinion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colAppOpinion.Width = 77;
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
            this.colproject,
            this.colProjectTaskName,
            this.colLaborSubject,
            this.colRealLabor,
            this.colAccountQuantity,
            this.colQuantityUnit,
            this.colAccountPrice,
            this.colAccountMoney,
            this.colPriceUnit,
            this.colDescript,
            this.colTeam,
            this.colBeginDate,
            this.colEndDate,
            this.colDetailNumber});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(11, 227);
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
            this.dgDetail.Size = new System.Drawing.Size(887, 224);
            this.dgDetail.TabIndex = 100;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 53;
            // 
            // colproject
            // 
            this.colproject.HeaderText = "工程任务";
            this.colproject.Name = "colproject";
            this.colproject.ReadOnly = true;
            this.colproject.Width = 77;
            // 
            // colProjectTaskName
            // 
            this.colProjectTaskName.HeaderText = "工程任务明细";
            this.colProjectTaskName.Name = "colProjectTaskName";
            this.colProjectTaskName.ReadOnly = true;
            this.colProjectTaskName.Width = 101;
            // 
            // colLaborSubject
            // 
            this.colLaborSubject.HeaderText = "用工科目";
            this.colLaborSubject.Name = "colLaborSubject";
            this.colLaborSubject.ReadOnly = true;
            this.colLaborSubject.Width = 77;
            // 
            // colRealLabor
            // 
            this.colRealLabor.HeaderText = "实际用工量";
            this.colRealLabor.Name = "colRealLabor";
            this.colRealLabor.ReadOnly = true;
            this.colRealLabor.Width = 89;
            // 
            // colAccountQuantity
            // 
            this.colAccountQuantity.HeaderText = "核算工程量";
            this.colAccountQuantity.Name = "colAccountQuantity";
            this.colAccountQuantity.Width = 89;
            // 
            // colQuantityUnit
            // 
            this.colQuantityUnit.HeaderText = "数量单位";
            this.colQuantityUnit.Name = "colQuantityUnit";
            this.colQuantityUnit.Width = 77;
            // 
            // colAccountPrice
            // 
            this.colAccountPrice.HeaderText = "核算单价";
            this.colAccountPrice.Name = "colAccountPrice";
            this.colAccountPrice.Width = 77;
            // 
            // colAccountMoney
            // 
            this.colAccountMoney.HeaderText = "核算合价";
            this.colAccountMoney.Name = "colAccountMoney";
            this.colAccountMoney.ReadOnly = true;
            this.colAccountMoney.Width = 77;
            // 
            // colPriceUnit
            // 
            this.colPriceUnit.HeaderText = "价格单位";
            this.colPriceUnit.Name = "colPriceUnit";
            this.colPriceUnit.Width = 77;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "用工说明";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 77;
            // 
            // colTeam
            // 
            this.colTeam.HeaderText = "被代工队伍";
            this.colTeam.Name = "colTeam";
            this.colTeam.ReadOnly = true;
            this.colTeam.Width = 89;
            // 
            // colBeginDate
            // 
            this.colBeginDate.HeaderText = "开始时间";
            this.colBeginDate.Name = "colBeginDate";
            this.colBeginDate.ReadOnly = true;
            this.colBeginDate.Width = 77;
            // 
            // colEndDate
            // 
            this.colEndDate.HeaderText = "结束时间";
            this.colEndDate.Name = "colEndDate";
            this.colEndDate.ReadOnly = true;
            this.colEndDate.Width = 77;
            // 
            // colDetailNumber
            // 
            this.colDetailNumber.HeaderText = "明细编号";
            this.colDetailNumber.Name = "colDetailNumber";
            this.colDetailNumber.ReadOnly = true;
            this.colDetailNumber.Width = 77;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(791, 459);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 101;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // VTimeDispatchingSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 487);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "VTimeDispatchingSelect";
            this.Text = "VMaterialRentalOrderQuery";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplier;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel17;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel18;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBearTeam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLaborType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMSubInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterCreatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterDescript;
        private System.Windows.Forms.DataGridViewLinkColumn colAppOpinion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colproject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectTaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLaborSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealLabor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantityUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPriceUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTeam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBeginDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailNumber;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel21;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtCreatePerson;
    }
}