namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireLedger
{
    partial class VMaterialHireLedgerQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMaterialHireLedgerQuery));
            this.txtSumCollQuantity = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSumReturnQuantity = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblRecordTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtUsedBank = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtSpec = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMaterial = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial();
            this.txtOriContractNo = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupplier = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOriContractNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplyInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLeftQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsedBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.txtSumCollQuantity);
            this.pnlFloor.Controls.Add(this.customLabel7);
            this.pnlFloor.Controls.Add(this.txtSumReturnQuantity);
            this.pnlFloor.Controls.Add(this.lblRecordTotal);
            this.pnlFloor.Controls.Add(this.customLabel5);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Size = new System.Drawing.Size(902, 531);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel5, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecordTotal, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtSumReturnQuantity, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel7, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtSumCollQuantity, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(425, 20);
            // 
            // txtSumCollQuantity
            // 
            this.txtSumCollQuantity.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtSumCollQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.txtSumCollQuantity.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumCollQuantity.DrawSelf = false;
            this.txtSumCollQuantity.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumCollQuantity.EnterToTab = false;
            this.txtSumCollQuantity.Location = new System.Drawing.Point(607, 497);
            this.txtSumCollQuantity.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumCollQuantity.Name = "txtSumCollQuantity";
            this.txtSumCollQuantity.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumCollQuantity.ReadOnly = true;
            this.txtSumCollQuantity.Size = new System.Drawing.Size(104, 16);
            this.txtSumCollQuantity.TabIndex = 113;
            // 
            // customLabel7
            // 
            this.customLabel7.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(542, 501);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 112;
            this.customLabel7.Text = "发料数量:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSumReturnQuantity
            // 
            this.txtSumReturnQuantity.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtSumReturnQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.txtSumReturnQuantity.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumReturnQuantity.DrawSelf = false;
            this.txtSumReturnQuantity.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumReturnQuantity.EnterToTab = false;
            this.txtSumReturnQuantity.Location = new System.Drawing.Point(792, 497);
            this.txtSumReturnQuantity.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumReturnQuantity.Name = "txtSumReturnQuantity";
            this.txtSumReturnQuantity.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumReturnQuantity.ReadOnly = true;
            this.txtSumReturnQuantity.Size = new System.Drawing.Size(104, 16);
            this.txtSumReturnQuantity.TabIndex = 111;
            // 
            // lblRecordTotal
            // 
            this.lblRecordTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblRecordTotal.AutoSize = true;
            this.lblRecordTotal.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecordTotal.ForeColor = System.Drawing.Color.Red;
            this.lblRecordTotal.Location = new System.Drawing.Point(435, 501);
            this.lblRecordTotal.Name = "lblRecordTotal";
            this.lblRecordTotal.Size = new System.Drawing.Size(83, 12);
            this.lblRecordTotal.TabIndex = 110;
            this.lblRecordTotal.Text = "共【0】条记录";
            this.lblRecordTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(727, 501);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(59, 12);
            this.customLabel5.TabIndex = 109;
            this.customLabel5.Text = "退料数量:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.customLabel9);
            this.groupBox1.Controls.Add(this.txtUsedBank);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.txtMaterial);
            this.groupBox1.Controls.Add(this.txtOriContractNo);
            this.groupBox1.Controls.Add(this.customLabel8);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.txtSupplier);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Location = new System.Drawing.Point(6, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(887, 89);
            this.groupBox1.TabIndex = 107;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(330, 52);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(59, 12);
            this.customLabel9.TabIndex = 111;
            this.customLabel9.Text = "使用队伍:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtUsedBank
            // 
            this.txtUsedBank.BackColor = System.Drawing.SystemColors.Control;
            this.txtUsedBank.Code = null;
            this.txtUsedBank.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtUsedBank.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtUsedBank.EnterToTab = false;
            this.txtUsedBank.Id = "";
            this.txtUsedBank.Location = new System.Drawing.Point(405, 47);
            this.txtUsedBank.Name = "txtUsedBank";
            this.txtUsedBank.Result = ((System.Collections.IList)(resources.GetObject("txtUsedBank.Result")));
            this.txtUsedBank.RightMouse = false;
            this.txtUsedBank.Size = new System.Drawing.Size(154, 21);
            this.txtUsedBank.TabIndex = 110;
            this.txtUsedBank.Tag = null;
            this.txtUsedBank.Value = "";
            this.txtUsedBank.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(769, 45);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 103;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(769, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 102;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.SystemColors.Control;
            this.txtSpec.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSpec.DrawSelf = false;
            this.txtSpec.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSpec.EnterToTab = false;
            this.txtSpec.Location = new System.Drawing.Point(642, 20);
            this.txtSpec.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Padding = new System.Windows.Forms.Padding(1);
            this.txtSpec.ReadOnly = false;
            this.txtSpec.Size = new System.Drawing.Size(104, 16);
            this.txtSpec.TabIndex = 101;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(577, 24);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 109;
            this.customLabel4.Text = "规格型号:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
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
            this.txtMaterial.Location = new System.Drawing.Point(405, 19);
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Result = ((System.Collections.IList)(resources.GetObject("txtMaterial.Result")));
            this.txtMaterial.RightMouse = false;
            this.txtMaterial.Size = new System.Drawing.Size(154, 21);
            this.txtMaterial.TabIndex = 100;
            this.txtMaterial.Tag = null;
            this.txtMaterial.Value = "";
            this.txtMaterial.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // txtOriContractNo
            // 
            this.txtOriContractNo.BackColor = System.Drawing.SystemColors.Control;
            this.txtOriContractNo.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOriContractNo.DrawSelf = false;
            this.txtOriContractNo.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOriContractNo.EnterToTab = false;
            this.txtOriContractNo.Location = new System.Drawing.Point(642, 47);
            this.txtOriContractNo.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOriContractNo.Name = "txtOriContractNo";
            this.txtOriContractNo.Padding = new System.Windows.Forms.Padding(1);
            this.txtOriContractNo.ReadOnly = false;
            this.txtOriContractNo.Size = new System.Drawing.Size(104, 16);
            this.txtOriContractNo.TabIndex = 99;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(354, 24);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(35, 12);
            this.customLabel8.TabIndex = 106;
            this.customLabel8.Text = "物资:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(565, 50);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(71, 12);
            this.customLabel3.TabIndex = 108;
            this.customLabel3.Text = "租赁合同号:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(23, 52);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(47, 12);
            this.customLabel2.TabIndex = 107;
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
            this.txtSupplier.Location = new System.Drawing.Point(76, 47);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Result = ((System.Collections.IList)(resources.GetObject("txtSupplier.Result")));
            this.txtSupplier.RightMouse = false;
            this.txtSupplier.Size = new System.Drawing.Size(231, 21);
            this.txtSupplier.TabIndex = 98;
            this.txtSupplier.Tag = null;
            this.txtSupplier.Value = "";
            this.txtSupplier.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(11, 27);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 104;
            this.customLabel1.Text = "台账日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(76, 20);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 96;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(208, 20);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 97;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(191, 27);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 105;
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
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCreateDate,
            this.colType,
            this.colCode,
            this.colOriContractNo,
            this.colSupplyInfo,
            this.colRankName,
            this.colMaterialCode,
            this.colMaterialName,
            this.colSpec,
            this.colQuantity,
            this.colLeftQuantity,
            this.colPrice,
            this.colOperDate,
            this.colUnit});
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
            this.dgDetail.Location = new System.Drawing.Point(6, 112);
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
            this.dgDetail.Size = new System.Drawing.Size(887, 379);
            this.dgDetail.TabIndex = 108;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colCreateDate
            // 
            this.colCreateDate.HeaderText = "台账日期";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.Width = 80;
            // 
            // colType
            // 
            this.colType.HeaderText = "类型";
            this.colType.Name = "colType";
            // 
            // colCode
            // 
            this.colCode.HeaderText = "单据号";
            this.colCode.Name = "colCode";
            this.colCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCode.Width = 80;
            // 
            // colOriContractNo
            // 
            this.colOriContractNo.HeaderText = "租赁合同号";
            this.colOriContractNo.Name = "colOriContractNo";
            // 
            // colSupplyInfo
            // 
            this.colSupplyInfo.HeaderText = "出租方";
            this.colSupplyInfo.Name = "colSupplyInfo";
            // 
            // colRankName
            // 
            this.colRankName.HeaderText = "队伍";
            this.colRankName.Name = "colRankName";
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.HeaderText = "物料编码";
            this.colMaterialCode.Name = "colMaterialCode";
            // 
            // colMaterialName
            // 
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            this.colMaterialName.Width = 80;
            // 
            // colSpec
            // 
            this.colSpec.HeaderText = "规格型号";
            this.colSpec.Name = "colSpec";
            this.colSpec.Width = 80;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "数量";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Width = 80;
            // 
            // colLeftQuantity
            // 
            this.colLeftQuantity.HeaderText = "剩余数量";
            this.colLeftQuantity.Name = "colLeftQuantity";
            // 
            // colPrice
            // 
            this.colPrice.HeaderText = "租赁单价";
            this.colPrice.Name = "colPrice";
            this.colPrice.Width = 80;
            // 
            // colOperDate
            // 
            this.colOperDate.HeaderText = "业务日期";
            this.colOperDate.Name = "colOperDate";
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            // 
            // VMaterialHireLedgerQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 531);
            this.Name = "VMaterialHireLedgerQuery";
            this.Text = "料具租赁流水账";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsedBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumCollQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumReturnQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecordTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtUsedBank;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSpec;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial txtMaterial;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOriContractNo;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupplier;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOriContractNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplyInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRankName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLeftQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
    }
}