namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng
{
    partial class VDemandMasterPlanSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDemandMasterPlanSelector));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtCodeEnd = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtHandlePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.txtCodeBegin = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel16 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel21 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.customLabel17 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel18 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colDgMasterCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterCreatePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHandlePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterSumQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDgMasterDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lnkNone = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.lnkAll = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtSumQuantity = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblRecordTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiagramNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuoteQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsedPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.txtSumQuantity);
            this.pnlFloor.Controls.Add(this.lblRecordTotal);
            this.pnlFloor.Controls.Add(this.customLabel5);
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
            this.pnlFloor.Controls.SetChildIndex(this.customLabel5, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecordTotal, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtSumQuantity, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(116, -22);
            this.lblTitle.Size = new System.Drawing.Size(156, 20);
            this.lblTitle.Text = "需求总计划引用";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtCodeEnd);
            this.groupBox1.Controls.Add(this.txtHandlePerson);
            this.groupBox1.Controls.Add(this.txtCodeBegin);
            this.groupBox1.Controls.Add(this.customLabel16);
            this.groupBox1.Controls.Add(this.customLabel21);
            this.groupBox1.Controls.Add(this.txtCreatePerson);
            this.groupBox1.Controls.Add(this.customLabel17);
            this.groupBox1.Controls.Add(this.customLabel18);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Location = new System.Drawing.Point(12, -4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(855, 65);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(649, 34);
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
            this.txtCodeEnd.Location = new System.Drawing.Point(204, 13);
            this.txtCodeEnd.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeEnd.Name = "txtCodeEnd";
            this.txtCodeEnd.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeEnd.ReadOnly = false;
            this.txtCodeEnd.Size = new System.Drawing.Size(104, 16);
            this.txtCodeEnd.TabIndex = 2;
            // 
            // txtHandlePerson
            // 
            this.txtHandlePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtHandlePerson.Code = null;
            this.txtHandlePerson.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtHandlePerson.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtHandlePerson.EnterToTab = false;
            this.txtHandlePerson.Id = "";
            this.txtHandlePerson.Location = new System.Drawing.Point(392, 36);
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Result = ((System.Collections.IList)(resources.GetObject("txtHandlePerson.Result")));
            this.txtHandlePerson.RightMouse = false;
            this.txtHandlePerson.Size = new System.Drawing.Size(240, 21);
            this.txtHandlePerson.TabIndex = 8;
            this.txtHandlePerson.Tag = null;
            this.txtHandlePerson.Value = "";
            this.txtHandlePerson.Visible = false;
            this.txtHandlePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // txtCodeBegin
            // 
            this.txtCodeBegin.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBegin.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBegin.DrawSelf = false;
            this.txtCodeBegin.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBegin.EnterToTab = false;
            this.txtCodeBegin.Location = new System.Drawing.Point(77, 13);
            this.txtCodeBegin.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBegin.Name = "txtCodeBegin";
            this.txtCodeBegin.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBegin.ReadOnly = false;
            this.txtCodeBegin.Size = new System.Drawing.Size(104, 16);
            this.txtCodeBegin.TabIndex = 1;
            // 
            // customLabel16
            // 
            this.customLabel16.AutoSize = true;
            this.customLabel16.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel16.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel16.Location = new System.Drawing.Point(339, 42);
            this.customLabel16.Name = "customLabel16";
            this.customLabel16.Size = new System.Drawing.Size(47, 12);
            this.customLabel16.TabIndex = 86;
            this.customLabel16.Text = "责任人:";
            this.customLabel16.UnderLineColor = System.Drawing.Color.Red;
            this.customLabel16.Visible = false;
            // 
            // customLabel21
            // 
            this.customLabel21.AutoSize = true;
            this.customLabel21.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel21.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel21.Location = new System.Drawing.Point(26, 43);
            this.customLabel21.Name = "customLabel21";
            this.customLabel21.Size = new System.Drawing.Size(47, 12);
            this.customLabel21.TabIndex = 88;
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
            this.txtCreatePerson.Location = new System.Drawing.Point(77, 39);
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Result = ((System.Collections.IList)(resources.GetObject("txtCreatePerson.Result")));
            this.txtCreatePerson.RightMouse = false;
            this.txtCreatePerson.Size = new System.Drawing.Size(231, 21);
            this.txtCreatePerson.TabIndex = 5;
            this.txtCreatePerson.Tag = null;
            this.txtCreatePerson.Value = "";
            this.txtCreatePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel17
            // 
            this.customLabel17.AutoSize = true;
            this.customLabel17.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel17.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel17.Location = new System.Drawing.Point(187, 17);
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
            this.customLabel18.Location = new System.Drawing.Point(27, 16);
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
            this.customLabel1.Location = new System.Drawing.Point(326, 17);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 80;
            this.customLabel1.Text = "业务日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(391, 10);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 3;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(523, 10);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 4;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(506, 17);
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
            this.colMaterialCode,
            this.colMaterialName,
            this.colSpec,
            this.colDiagramNum,
            this.colQuantity,
            this.colQuoteQuantity,
            this.colPrice,
            this.colUsedPart,
            this.colDescript});
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
            this.dgDetail.Location = new System.Drawing.Point(12, 221);
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
            this.dgDetail.Size = new System.Drawing.Size(855, 203);
            this.dgDetail.TabIndex = 4;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
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
            this.colDgMasterCreateDate,
            this.colDgMasterCreatePerson,
            this.colHandlePerson,
            this.colDgMasterSumQuantity,
            this.colSumMoney,
            this.colDgMasterDescript});
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
            this.dgMaster.Location = new System.Drawing.Point(12, 65);
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
            // colDgMasterCreateDate
            // 
            this.colDgMasterCreateDate.HeaderText = "制单时间";
            this.colDgMasterCreateDate.Name = "colDgMasterCreateDate";
            this.colDgMasterCreateDate.Width = 77;
            // 
            // colDgMasterCreatePerson
            // 
            this.colDgMasterCreatePerson.HeaderText = "制单人";
            this.colDgMasterCreatePerson.Name = "colDgMasterCreatePerson";
            this.colDgMasterCreatePerson.Width = 65;
            // 
            // colHandlePerson
            // 
            this.colHandlePerson.HeaderText = "负责人";
            this.colHandlePerson.Name = "colHandlePerson";
            this.colHandlePerson.Width = 65;
            // 
            // colDgMasterSumQuantity
            // 
            this.colDgMasterSumQuantity.HeaderText = "总数量";
            this.colDgMasterSumQuantity.Name = "colDgMasterSumQuantity";
            this.colDgMasterSumQuantity.Width = 65;
            // 
            // colSumMoney
            // 
            this.colSumMoney.HeaderText = "总金额";
            this.colSumMoney.Name = "colSumMoney";
            this.colSumMoney.Width = 65;
            // 
            // colDgMasterDescript
            // 
            this.colDgMasterDescript.HeaderText = "备注";
            this.colDgMasterDescript.Name = "colDgMasterDescript";
            this.colDgMasterDescript.Width = 53;
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
            this.btnCancel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.btnOK.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(704, 430);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // txtSumQuantity
            // 
            this.txtSumQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSumQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.txtSumQuantity.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumQuantity.DrawSelf = false;
            this.txtSumQuantity.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumQuantity.EnterToTab = false;
            this.txtSumQuantity.Location = new System.Drawing.Point(612, 432);
            this.txtSumQuantity.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumQuantity.Name = "txtSumQuantity";
            this.txtSumQuantity.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumQuantity.ReadOnly = true;
            this.txtSumQuantity.Size = new System.Drawing.Size(74, 16);
            this.txtSumQuantity.TabIndex = 101;
            // 
            // lblRecordTotal
            // 
            this.lblRecordTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecordTotal.AutoSize = true;
            this.lblRecordTotal.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecordTotal.ForeColor = System.Drawing.Color.Red;
            this.lblRecordTotal.Location = new System.Drawing.Point(422, 437);
            this.lblRecordTotal.Name = "lblRecordTotal";
            this.lblRecordTotal.Size = new System.Drawing.Size(107, 12);
            this.lblRecordTotal.TabIndex = 100;
            this.lblRecordTotal.Text = "共选择【0】条记录";
            this.lblRecordTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(547, 437);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(59, 12);
            this.customLabel5.TabIndex = 99;
            this.customLabel5.Text = "合计数量:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 53;
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.HeaderText = "物料编码";
            this.colMaterialCode.Name = "colMaterialCode";
            this.colMaterialCode.Width = 77;
            // 
            // colMaterialName
            // 
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            this.colMaterialName.Width = 77;
            // 
            // colSpec
            // 
            this.colSpec.HeaderText = "规格型号";
            this.colSpec.Name = "colSpec";
            this.colSpec.Width = 77;
            // 
            // colDiagramNum
            // 
            this.colDiagramNum.HeaderText = "图号";
            this.colDiagramNum.Name = "colDiagramNum";
            this.colDiagramNum.Width = 53;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "数量";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Width = 53;
            // 
            // colQuoteQuantity
            // 
            this.colQuoteQuantity.HeaderText = "可引用数量";
            this.colQuoteQuantity.Name = "colQuoteQuantity";
            this.colQuoteQuantity.Width = 89;
            // 
            // colPrice
            // 
            this.colPrice.HeaderText = "单价";
            this.colPrice.Name = "colPrice";
            this.colPrice.Width = 53;
            // 
            // colUsedPart
            // 
            this.colUsedPart.HeaderText = "使用部位";
            this.colUsedPart.Name = "colUsedPart";
            this.colUsedPart.Width = 77;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 53;
            // 
            // VDemandMasterPlanSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 458);
            this.Name = "VDemandMasterPlanSelector";
            this.Text = "需求总计划引用";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeEnd;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtHandlePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel16;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel21;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtCreatePerson;
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
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecordTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterCreatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHandlePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterSumQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDgMasterDescript;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiagramNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuoteQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsedPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;

    }
}