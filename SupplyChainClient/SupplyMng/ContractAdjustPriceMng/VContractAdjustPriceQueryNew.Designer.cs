namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng
{
    partial class VContractAdjustPriceQueryNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VContractAdjustPriceQueryNew));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSupplyNum = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel10 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupplier = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMaterialSpec = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtMaterial = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCodeEnd = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtCodeBegin = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel21 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.customLabel17 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel18 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgExtDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCGCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGContractNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGSupply = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGMaterialSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGOldPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGNewPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGContractPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGContractDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCGContractReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgExtDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(1006, 525);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -8);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtSupplyNum);
            this.groupBox1.Controls.Add(this.customLabel10);
            this.groupBox1.Controls.Add(this.txtSupplier);
            this.groupBox1.Controls.Add(this.customLabel9);
            this.groupBox1.Controls.Add(this.txtMaterialSpec);
            this.groupBox1.Controls.Add(this.customLabel7);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtMaterial);
            this.groupBox1.Controls.Add(this.customLabel8);
            this.groupBox1.Controls.Add(this.txtCodeEnd);
            this.groupBox1.Controls.Add(this.txtCodeBegin);
            this.groupBox1.Controls.Add(this.customLabel21);
            this.groupBox1.Controls.Add(this.txtCreatePerson);
            this.groupBox1.Controls.Add(this.customLabel17);
            this.groupBox1.Controls.Add(this.customLabel18);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(987, 72);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtSupplyNum
            // 
            this.txtSupplyNum.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupplyNum.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSupplyNum.DrawSelf = false;
            this.txtSupplyNum.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSupplyNum.EnterToTab = false;
            this.txtSupplyNum.Location = new System.Drawing.Point(654, 18);
            this.txtSupplyNum.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSupplyNum.Name = "txtSupplyNum";
            this.txtSupplyNum.Padding = new System.Windows.Forms.Padding(1);
            this.txtSupplyNum.ReadOnly = false;
            this.txtSupplyNum.Size = new System.Drawing.Size(111, 16);
            this.txtSupplyNum.TabIndex = 98;
            // 
            // customLabel10
            // 
            this.customLabel10.AutoSize = true;
            this.customLabel10.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel10.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel10.Location = new System.Drawing.Point(608, 22);
            this.customLabel10.Name = "customLabel10";
            this.customLabel10.Size = new System.Drawing.Size(47, 12);
            this.customLabel10.TabIndex = 99;
            this.customLabel10.Text = "合同号:";
            this.customLabel10.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSupplier
            // 
            this.txtSupplier.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupplier.Code = null;
            this.txtSupplier.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtSupplier.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.ComboBox;
            this.txtSupplier.EnterToTab = false;
            this.txtSupplier.Id = "";
            this.txtSupplier.Location = new System.Drawing.Point(72, 43);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Padding = new System.Windows.Forms.Padding(1);
            this.txtSupplier.Result = ((System.Collections.IList)(resources.GetObject("txtSupplier.Result")));
            this.txtSupplier.RightMouse = false;
            this.txtSupplier.Size = new System.Drawing.Size(209, 23);
            this.txtSupplier.TabIndex = 96;
            this.txtSupplier.Tag = null;
            this.txtSupplier.Value = "";
            this.txtSupplier.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(19, 51);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(47, 12);
            this.customLabel9.TabIndex = 97;
            this.customLabel9.Text = "供应商:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtMaterialSpec
            // 
            this.txtMaterialSpec.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterialSpec.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMaterialSpec.DrawSelf = false;
            this.txtMaterialSpec.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMaterialSpec.EnterToTab = false;
            this.txtMaterialSpec.Location = new System.Drawing.Point(653, 44);
            this.txtMaterialSpec.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMaterialSpec.Name = "txtMaterialSpec";
            this.txtMaterialSpec.Padding = new System.Windows.Forms.Padding(1);
            this.txtMaterialSpec.ReadOnly = false;
            this.txtMaterialSpec.Size = new System.Drawing.Size(112, 16);
            this.txtMaterialSpec.TabIndex = 94;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(596, 47);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 95;
            this.customLabel7.Text = "规格型号:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(828, 42);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtMaterial
            // 
            this.txtMaterial.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterial.Code = null;
            this.txtMaterial.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtMaterial.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtMaterial.EnterToTab = false;
            this.txtMaterial.Id = "";
            this.txtMaterial.IsAllLoad = true;
            this.txtMaterial.IsCheckBox = false;
            this.txtMaterial.Location = new System.Drawing.Point(359, 43);
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Result = ((System.Collections.IList)(resources.GetObject("txtMaterial.Result")));
            this.txtMaterial.RightMouse = false;
            this.txtMaterial.Size = new System.Drawing.Size(231, 21);
            this.txtMaterial.TabIndex = 9;
            this.txtMaterial.Tag = null;
            this.txtMaterial.Value = "";
            this.txtMaterial.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(323, 49);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(35, 12);
            this.customLabel8.TabIndex = 84;
            this.customLabel8.Text = "物资:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCodeEnd
            // 
            this.txtCodeEnd.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeEnd.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeEnd.DrawSelf = false;
            this.txtCodeEnd.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeEnd.EnterToTab = false;
            this.txtCodeEnd.Location = new System.Drawing.Point(180, 18);
            this.txtCodeEnd.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeEnd.Name = "txtCodeEnd";
            this.txtCodeEnd.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeEnd.ReadOnly = false;
            this.txtCodeEnd.Size = new System.Drawing.Size(101, 16);
            this.txtCodeEnd.TabIndex = 2;
            // 
            // txtCodeBegin
            // 
            this.txtCodeBegin.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBegin.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBegin.DrawSelf = false;
            this.txtCodeBegin.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBegin.EnterToTab = false;
            this.txtCodeBegin.Location = new System.Drawing.Point(70, 18);
            this.txtCodeBegin.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBegin.Name = "txtCodeBegin";
            this.txtCodeBegin.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBegin.ReadOnly = false;
            this.txtCodeBegin.Size = new System.Drawing.Size(101, 16);
            this.txtCodeBegin.TabIndex = 1;
            // 
            // customLabel21
            // 
            this.customLabel21.AutoSize = true;
            this.customLabel21.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel21.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel21.Location = new System.Drawing.Point(777, 22);
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
            this.txtCreatePerson.IsAllLoad = true;
            this.txtCreatePerson.Location = new System.Drawing.Point(824, 15);
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Result = ((System.Collections.IList)(resources.GetObject("txtCreatePerson.Result")));
            this.txtCreatePerson.RightMouse = false;
            this.txtCreatePerson.Size = new System.Drawing.Size(141, 21);
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
            this.customLabel17.Location = new System.Drawing.Point(170, 22);
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
            this.customLabel18.Location = new System.Drawing.Point(19, 22);
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
            this.customLabel1.Location = new System.Drawing.Point(300, 22);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 80;
            this.customLabel1.Text = "业务日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(360, 15);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(106, 21);
            this.dtpDateBegin.TabIndex = 3;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(483, 15);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(106, 21);
            this.dtpDateEnd.TabIndex = 4;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(469, 21);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 83;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(0, 83);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1003, 349);
            this.panel1.TabIndex = 119;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1003, 349);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgExtDetail);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(995, 323);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "调价历史列表";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgExtDetail
            // 
            this.dgExtDetail.AddDefaultMenu = false;
            this.dgExtDetail.AddNoColumn = true;
            this.dgExtDetail.AllowUserToAddRows = false;
            this.dgExtDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgExtDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgExtDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgExtDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgExtDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgExtDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCGCode,
            this.colCGContractNo,
            this.colCGSupply,
            this.colCGState,
            this.colCGMaterialCode,
            this.colCGMaterialName,
            this.colCGMaterialSpec,
            this.colCGOldPrice,
            this.colCGNewPrice,
            this.colCGContractPerson,
            this.colCGContractDate,
            this.colCGContractReason});
            this.dgExtDetail.CustomBackColor = false;
            this.dgExtDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgExtDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgExtDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgExtDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgExtDetail.FreezeFirstRow = false;
            this.dgExtDetail.FreezeLastRow = false;
            this.dgExtDetail.FrontColumnCount = 0;
            this.dgExtDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgExtDetail.HScrollOffset = 0;
            this.dgExtDetail.IsAllowOrder = true;
            this.dgExtDetail.IsConfirmDelete = true;
            this.dgExtDetail.Location = new System.Drawing.Point(3, 3);
            this.dgExtDetail.Name = "dgExtDetail";
            this.dgExtDetail.PageIndex = 0;
            this.dgExtDetail.PageSize = 0;
            this.dgExtDetail.Query = null;
            this.dgExtDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgExtDetail.ReadOnlyCols")));
            this.dgExtDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgExtDetail.RowHeadersWidth = 22;
            this.dgExtDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgExtDetail.RowTemplate.Height = 23;
            this.dgExtDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgExtDetail.Size = new System.Drawing.Size(989, 317);
            this.dgExtDetail.TabIndex = 9;
            this.dgExtDetail.TargetType = null;
            this.dgExtDetail.VScrollOffset = 0;
            // 
            // colCGCode
            // 
            this.colCGCode.HeaderText = "单号";
            this.colCGCode.Name = "colCGCode";
            // 
            // colCGContractNo
            // 
            this.colCGContractNo.HeaderText = "采购合同号";
            this.colCGContractNo.Name = "colCGContractNo";
            // 
            // colCGSupply
            // 
            this.colCGSupply.HeaderText = "供应商";
            this.colCGSupply.Name = "colCGSupply";
            // 
            // colCGState
            // 
            this.colCGState.HeaderText = "状态";
            this.colCGState.Name = "colCGState";
            // 
            // colCGMaterialCode
            // 
            this.colCGMaterialCode.HeaderText = "物资编码";
            this.colCGMaterialCode.Name = "colCGMaterialCode";
            // 
            // colCGMaterialName
            // 
            this.colCGMaterialName.HeaderText = "物资名称";
            this.colCGMaterialName.Name = "colCGMaterialName";
            // 
            // colCGMaterialSpec
            // 
            this.colCGMaterialSpec.HeaderText = "规格型号";
            this.colCGMaterialSpec.Name = "colCGMaterialSpec";
            // 
            // colCGOldPrice
            // 
            this.colCGOldPrice.HeaderText = "调前价格";
            this.colCGOldPrice.Name = "colCGOldPrice";
            // 
            // colCGNewPrice
            // 
            this.colCGNewPrice.HeaderText = "调后价格";
            this.colCGNewPrice.Name = "colCGNewPrice";
            // 
            // colCGContractPerson
            // 
            this.colCGContractPerson.HeaderText = "调价人";
            this.colCGContractPerson.Name = "colCGContractPerson";
            // 
            // colCGContractDate
            // 
            this.colCGContractDate.HeaderText = "调价日期";
            this.colCGContractDate.Name = "colCGContractDate";
            // 
            // colCGContractReason
            // 
            this.colCGContractReason.HeaderText = "调价原因";
            this.colCGContractReason.Name = "colCGContractReason";
            // 
            // VContractAdjustPriceQueryNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 525);
            this.Name = "VContractAdjustPriceQueryNew";
            this.Text = "采购合同调价";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgExtDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel17;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel18;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel21;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtCreatePerson;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial txtMaterial;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMaterialSpec;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupplier;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSupplyNum;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel10;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgExtDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGContractNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGSupply;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGMaterialSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGOldPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGNewPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGContractPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGContractDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCGContractReason;
    }
}