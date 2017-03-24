namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    partial class VMaterialHireMonthlyBalanceQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMaterialHireMonthlyBalanceQuery));
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSupplyInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOriContractNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFiscalYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFiscalMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOtherMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TenantSelector = new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.CommonControl.UcTenantSelector();
            this.txtMonth = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtYear = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupplier = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgCostDetail1 = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colMatCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDtlStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDtlEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApproachQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatCollDtlQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExitQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatReturnDtlQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnusedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRentalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBalRule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatCollCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatReturnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgOtherCost = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colBusinessType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOtherCostType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBusinessCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOtherCostMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCostDetail1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOtherCost)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgMaster);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.tabControl1);
            this.pnlFloor.Size = new System.Drawing.Size(1081, 597);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.tabControl1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgMaster, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(515, 20);
            // 
            // dgMaster
            // 
            this.dgMaster.AddDefaultMenu = false;
            this.dgMaster.AddNoColumn = false;
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.AllowUserToDeleteRows = false;
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgMaster.CausesValidation = false;
            this.dgMaster.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.ColumnHeadersHeight = 24;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSupplyInfo,
            this.colOriContractNo,
            this.colFiscalYear,
            this.colFiscalMonth,
            this.colStartDate,
            this.colEndDate,
            this.colOtherMoney,
            this.colSumQuantity,
            this.colSumMoney,
            this.colOperDate});
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
            this.dgMaster.Location = new System.Drawing.Point(3, 60);
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
            this.dgMaster.Size = new System.Drawing.Size(1071, 136);
            this.dgMaster.TabIndex = 113;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colSupplyInfo
            // 
            this.colSupplyInfo.HeaderText = "出租方";
            this.colSupplyInfo.Name = "colSupplyInfo";
            // 
            // colOriContractNo
            // 
            this.colOriContractNo.HeaderText = "原始合同号";
            this.colOriContractNo.Name = "colOriContractNo";
            // 
            // colFiscalYear
            // 
            this.colFiscalYear.HeaderText = "会计年";
            this.colFiscalYear.Name = "colFiscalYear";
            // 
            // colFiscalMonth
            // 
            this.colFiscalMonth.HeaderText = "会计月";
            this.colFiscalMonth.Name = "colFiscalMonth";
            // 
            // colStartDate
            // 
            this.colStartDate.HeaderText = "开始日期";
            this.colStartDate.Name = "colStartDate";
            this.colStartDate.Visible = false;
            // 
            // colEndDate
            // 
            this.colEndDate.HeaderText = "结束日期";
            this.colEndDate.Name = "colEndDate";
            // 
            // colOtherMoney
            // 
            this.colOtherMoney.HeaderText = "调整费用";
            this.colOtherMoney.Name = "colOtherMoney";
            // 
            // colSumQuantity
            // 
            this.colSumQuantity.HeaderText = "总数量";
            this.colSumQuantity.Name = "colSumQuantity";
            this.colSumQuantity.Visible = false;
            this.colSumQuantity.Width = 80;
            // 
            // colSumMoney
            // 
            this.colSumMoney.HeaderText = "总金额";
            this.colSumMoney.Name = "colSumMoney";
            // 
            // colOperDate
            // 
            this.colOperDate.HeaderText = "制单日期";
            this.colOperDate.Name = "colOperDate";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TenantSelector);
            this.groupBox1.Controls.Add(this.txtMonth);
            this.groupBox1.Controls.Add(this.txtYear);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.txtSupplier);
            this.groupBox1.Location = new System.Drawing.Point(-2, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1076, 48);
            this.groupBox1.TabIndex = 111;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // TenantSelector
            // 
            this.TenantSelector.Location = new System.Drawing.Point(535, 12);
            this.TenantSelector.Name = "TenantSelector";
            this.TenantSelector.ProjectID = null;
            this.TenantSelector.SelectedProject = null;
            this.TenantSelector.Size = new System.Drawing.Size(282, 25);
            this.TenantSelector.TabIndex = 151;
            // 
            // txtMonth
            // 
            this.txtMonth.BackColor = System.Drawing.SystemColors.Control;
            this.txtMonth.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMonth.DrawSelf = false;
            this.txtMonth.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMonth.EnterToTab = false;
            this.txtMonth.Location = new System.Drawing.Point(160, 16);
            this.txtMonth.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Padding = new System.Windows.Forms.Padding(1);
            this.txtMonth.ReadOnly = false;
            this.txtMonth.Size = new System.Drawing.Size(72, 16);
            this.txtMonth.TabIndex = 150;
            // 
            // txtYear
            // 
            this.txtYear.BackColor = System.Drawing.SystemColors.Control;
            this.txtYear.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtYear.DrawSelf = false;
            this.txtYear.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtYear.EnterToTab = false;
            this.txtYear.Location = new System.Drawing.Point(65, 16);
            this.txtYear.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtYear.Name = "txtYear";
            this.txtYear.Padding = new System.Windows.Forms.Padding(1);
            this.txtYear.ReadOnly = false;
            this.txtYear.Size = new System.Drawing.Size(72, 16);
            this.txtYear.TabIndex = 149;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(18, 20);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(47, 12);
            this.customLabel1.TabIndex = 148;
            this.customLabel1.Text = "会计期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(143, 20);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(11, 12);
            this.customLabel3.TabIndex = 147;
            this.customLabel3.Text = "-";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(922, 11);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 12;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(830, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(244, 19);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(47, 12);
            this.customLabel2.TabIndex = 90;
            this.customLabel2.Text = "出租方:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSupplier
            // 
            this.txtSupplier.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupplier.Code = null;
            this.txtSupplier.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtSupplier.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtSupplier.EnterToTab = false;
            this.txtSupplier.Id = "";
            this.txtSupplier.Location = new System.Drawing.Point(295, 16);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Result = ((System.Collections.IList)(resources.GetObject("txtSupplier.Result")));
            this.txtSupplier.RightMouse = false;
            this.txtSupplier.Size = new System.Drawing.Size(234, 21);
            this.txtSupplier.TabIndex = 6;
            this.txtSupplier.Tag = null;
            this.txtSupplier.Value = "";
            this.txtSupplier.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 202);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1067, 388);
            this.tabControl1.TabIndex = 112;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgCostDetail1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1059, 362);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "租赁费用";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgCostDetail1
            // 
            this.dgCostDetail1.AddDefaultMenu = false;
            this.dgCostDetail1.AddNoColumn = false;
            this.dgCostDetail1.AllowUserToAddRows = false;
            this.dgCostDetail1.AllowUserToDeleteRows = false;
            this.dgCostDetail1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgCostDetail1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgCostDetail1.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgCostDetail1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgCostDetail1.ColumnHeadersHeight = 24;
            this.dgCostDetail1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgCostDetail1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMatCode,
            this.colMatName,
            this.colMatSpec,
            this.colDtlStartDate,
            this.colDtlEndDate,
            this.colApproachQuantity,
            this.colMatCollDtlQty,
            this.colExitQuantity,
            this.colMatReturnDtlQty,
            this.colUnusedQuantity,
            this.colRentalPrice,
            this.colDays,
            this.colMoney,
            this.colState,
            this.colBalRule,
            this.colMatCollCode,
            this.colMatReturnCode,
            this.colMatUnit});
            this.dgCostDetail1.CustomBackColor = false;
            this.dgCostDetail1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCostDetail1.EditCellBackColor = System.Drawing.Color.White;
            this.dgCostDetail1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgCostDetail1.EnableHeadersVisualStyles = false;
            this.dgCostDetail1.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgCostDetail1.FreezeFirstRow = false;
            this.dgCostDetail1.FreezeLastRow = false;
            this.dgCostDetail1.FrontColumnCount = 0;
            this.dgCostDetail1.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgCostDetail1.HScrollOffset = 0;
            this.dgCostDetail1.IsAllowOrder = true;
            this.dgCostDetail1.IsConfirmDelete = true;
            this.dgCostDetail1.Location = new System.Drawing.Point(3, 3);
            this.dgCostDetail1.Name = "dgCostDetail1";
            this.dgCostDetail1.PageIndex = 0;
            this.dgCostDetail1.PageSize = 0;
            this.dgCostDetail1.Query = null;
            this.dgCostDetail1.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgCostDetail1.ReadOnlyCols")));
            this.dgCostDetail1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgCostDetail1.RowHeadersVisible = false;
            this.dgCostDetail1.RowHeadersWidth = 22;
            this.dgCostDetail1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgCostDetail1.RowTemplate.Height = 23;
            this.dgCostDetail1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgCostDetail1.Size = new System.Drawing.Size(1053, 356);
            this.dgCostDetail1.TabIndex = 111;
            this.dgCostDetail1.TargetType = null;
            this.dgCostDetail1.VScrollOffset = 0;
            // 
            // colMatCode
            // 
            this.colMatCode.HeaderText = "物料编码";
            this.colMatCode.Name = "colMatCode";
            this.colMatCode.Width = 80;
            // 
            // colMatName
            // 
            this.colMatName.HeaderText = "物资名称";
            this.colMatName.Name = "colMatName";
            this.colMatName.Width = 80;
            // 
            // colMatSpec
            // 
            this.colMatSpec.HeaderText = "规格型号";
            this.colMatSpec.Name = "colMatSpec";
            this.colMatSpec.Width = 80;
            // 
            // colDtlStartDate
            // 
            this.colDtlStartDate.HeaderText = "收退料日期";
            this.colDtlStartDate.Name = "colDtlStartDate";
            this.colDtlStartDate.Width = 80;
            // 
            // colDtlEndDate
            // 
            this.colDtlEndDate.HeaderText = "结束日期";
            this.colDtlEndDate.Name = "colDtlEndDate";
            this.colDtlEndDate.Visible = false;
            // 
            // colApproachQuantity
            // 
            this.colApproachQuantity.HeaderText = "进场数量";
            this.colApproachQuantity.Name = "colApproachQuantity";
            this.colApproachQuantity.Width = 80;
            // 
            // colMatCollDtlQty
            // 
            this.colMatCollDtlQty.HeaderText = "发料明细数量";
            this.colMatCollDtlQty.Name = "colMatCollDtlQty";
            this.colMatCollDtlQty.Visible = false;
            // 
            // colExitQuantity
            // 
            this.colExitQuantity.HeaderText = "退场数量";
            this.colExitQuantity.Name = "colExitQuantity";
            this.colExitQuantity.Width = 80;
            // 
            // colMatReturnDtlQty
            // 
            this.colMatReturnDtlQty.HeaderText = "退料明细数量";
            this.colMatReturnDtlQty.Name = "colMatReturnDtlQty";
            this.colMatReturnDtlQty.Visible = false;
            // 
            // colUnusedQuantity
            // 
            this.colUnusedQuantity.HeaderText = "结存数量";
            this.colUnusedQuantity.Name = "colUnusedQuantity";
            this.colUnusedQuantity.Width = 80;
            // 
            // colRentalPrice
            // 
            this.colRentalPrice.HeaderText = "租赁单价";
            this.colRentalPrice.Name = "colRentalPrice";
            this.colRentalPrice.Width = 80;
            // 
            // colDays
            // 
            this.colDays.HeaderText = "租赁天数";
            this.colDays.Name = "colDays";
            this.colDays.Width = 80;
            // 
            // colMoney
            // 
            this.colMoney.HeaderText = "金额";
            this.colMoney.Name = "colMoney";
            // 
            // colState
            // 
            this.colState.HeaderText = "结算状态";
            this.colState.Name = "colState";
            // 
            // colBalRule
            // 
            this.colBalRule.HeaderText = "结算规则";
            this.colBalRule.Name = "colBalRule";
            this.colBalRule.Width = 80;
            // 
            // colMatCollCode
            // 
            this.colMatCollCode.HeaderText = "发料单号";
            this.colMatCollCode.Name = "colMatCollCode";
            // 
            // colMatReturnCode
            // 
            this.colMatReturnCode.HeaderText = "退料单号";
            this.colMatReturnCode.Name = "colMatReturnCode";
            // 
            // colMatUnit
            // 
            this.colMatUnit.HeaderText = "计量单位";
            this.colMatUnit.Name = "colMatUnit";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgOtherCost);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(373, 48);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "其他费用";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgOtherCost
            // 
            this.dgOtherCost.AddDefaultMenu = false;
            this.dgOtherCost.AddNoColumn = false;
            this.dgOtherCost.AllowUserToAddRows = false;
            this.dgOtherCost.AllowUserToDeleteRows = false;
            this.dgOtherCost.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgOtherCost.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgOtherCost.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgOtherCost.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgOtherCost.ColumnHeadersHeight = 24;
            this.dgOtherCost.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgOtherCost.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBusinessType,
            this.colOtherCostType,
            this.colBusinessCode,
            this.colOtherCostMoney,
            this.colMaterialCode,
            this.colMaterialName,
            this.colMaterialSpec});
            this.dgOtherCost.CustomBackColor = false;
            this.dgOtherCost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgOtherCost.EditCellBackColor = System.Drawing.Color.White;
            this.dgOtherCost.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgOtherCost.EnableHeadersVisualStyles = false;
            this.dgOtherCost.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgOtherCost.FreezeFirstRow = false;
            this.dgOtherCost.FreezeLastRow = false;
            this.dgOtherCost.FrontColumnCount = 0;
            this.dgOtherCost.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgOtherCost.HScrollOffset = 0;
            this.dgOtherCost.IsAllowOrder = true;
            this.dgOtherCost.IsConfirmDelete = true;
            this.dgOtherCost.Location = new System.Drawing.Point(3, 3);
            this.dgOtherCost.Name = "dgOtherCost";
            this.dgOtherCost.PageIndex = 0;
            this.dgOtherCost.PageSize = 0;
            this.dgOtherCost.Query = null;
            this.dgOtherCost.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgOtherCost.ReadOnlyCols")));
            this.dgOtherCost.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgOtherCost.RowHeadersVisible = false;
            this.dgOtherCost.RowHeadersWidth = 22;
            this.dgOtherCost.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgOtherCost.RowTemplate.Height = 23;
            this.dgOtherCost.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgOtherCost.Size = new System.Drawing.Size(367, 42);
            this.dgOtherCost.TabIndex = 110;
            this.dgOtherCost.TargetType = null;
            this.dgOtherCost.VScrollOffset = 0;
            // 
            // colBusinessType
            // 
            this.colBusinessType.HeaderText = "业务类型";
            this.colBusinessType.Name = "colBusinessType";
            this.colBusinessType.Width = 80;
            // 
            // colOtherCostType
            // 
            this.colOtherCostType.HeaderText = "费用类型";
            this.colOtherCostType.Name = "colOtherCostType";
            // 
            // colBusinessCode
            // 
            this.colBusinessCode.HeaderText = "业务单据号";
            this.colBusinessCode.Name = "colBusinessCode";
            // 
            // colOtherCostMoney
            // 
            this.colOtherCostMoney.HeaderText = "金额";
            this.colOtherCostMoney.Name = "colOtherCostMoney";
            this.colOtherCostMoney.Width = 80;
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.HeaderText = "物资编码";
            this.colMaterialCode.Name = "colMaterialCode";
            // 
            // colMaterialName
            // 
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            // 
            // colMaterialSpec
            // 
            this.colMaterialSpec.HeaderText = "规格";
            this.colMaterialSpec.Name = "colMaterialSpec";
            // 
            // VMaterialHireMonthlyBalanceQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 597);
            this.Name = "VMaterialHireMonthlyBalanceQuery";
            this.Text = "料具月报";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCostDetail1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgOtherCost)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplyInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOriContractNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFiscalYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFiscalMonth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOtherMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMonth;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtYear;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupplier;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgCostDetail1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtlStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtlEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApproachQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatCollDtlQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExitQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatReturnDtlQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnusedQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRentalPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDays;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBalRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatCollCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatReturnCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatUnit;
        private System.Windows.Forms.TabPage tabPage2;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgOtherCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBusinessType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOtherCostType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBusinessCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOtherCostMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialSpec;
        private Application.Business.Erp.SupplyChain.Client.MaterialHireMng.CommonControl.UcTenantSelector TenantSelector;
    }
}