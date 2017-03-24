namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VPaymentInvoiceQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VPaymentInvoiceQuery));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnExcelBill = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearchBill = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.cmbInvoiceType = new System.Windows.Forms.ComboBox();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblPSelect = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupply = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.lblCat = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.datePeriodPicker1 = new Application.Business.Erp.SupplyChain.Client.Basic.Controls.DatePeriodPicker();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colRowNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCodeBill = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colGSFGS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPayOrg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNSSBH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPayType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaxRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaxMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBHSJE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransferType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransferTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIfDeduction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStateBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePersonBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperationDateBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescriptBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupply)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Size = new System.Drawing.Size(1017, 494);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnExcelBill);
            this.groupBox2.Controls.Add(this.btnSearchBill);
            this.groupBox2.Controls.Add(this.cmbInvoiceType);
            this.groupBox2.Controls.Add(this.customLabel9);
            this.groupBox2.Controls.Add(this.btnOperationOrg);
            this.groupBox2.Controls.Add(this.txtOperationOrg);
            this.groupBox2.Controls.Add(this.lblPSelect);
            this.groupBox2.Controls.Add(this.txtSupply);
            this.groupBox2.Controls.Add(this.lblCat);
            this.groupBox2.Controls.Add(this.datePeriodPicker1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1002, 93);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            // 
            // btnExcelBill
            // 
            this.btnExcelBill.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcelBill.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcelBill.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcelBill.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcelBill.Location = new System.Drawing.Point(859, 50);
            this.btnExcelBill.Name = "btnExcelBill";
            this.btnExcelBill.Size = new System.Drawing.Size(75, 23);
            this.btnExcelBill.TabIndex = 248;
            this.btnExcelBill.Text = "导出到Excel";
            this.btnExcelBill.UseVisualStyleBackColor = true;
            // 
            // btnSearchBill
            // 
            this.btnSearchBill.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearchBill.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearchBill.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearchBill.Location = new System.Drawing.Point(770, 50);
            this.btnSearchBill.Name = "btnSearchBill";
            this.btnSearchBill.Size = new System.Drawing.Size(75, 23);
            this.btnSearchBill.TabIndex = 247;
            this.btnSearchBill.Text = "查询";
            this.btnSearchBill.UseVisualStyleBackColor = true;
            // 
            // cmbInvoiceType
            // 
            this.cmbInvoiceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInvoiceType.FormattingEnabled = true;
            this.cmbInvoiceType.Items.AddRange(new object[] {
            "",
            "专票",
            "普票"});
            this.cmbInvoiceType.Location = new System.Drawing.Point(438, 51);
            this.cmbInvoiceType.Name = "cmbInvoiceType";
            this.cmbInvoiceType.Size = new System.Drawing.Size(121, 20);
            this.cmbInvoiceType.TabIndex = 246;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(379, 55);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(59, 12);
            this.customLabel9.TabIndex = 245;
            this.customLabel9.Text = "发票类别:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(689, 17);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 244;
            this.btnOperationOrg.Text = "选择";
            this.btnOperationOrg.UseVisualStyleBackColor = true;
            // 
            // txtOperationOrg
            // 
            this.txtOperationOrg.BackColor = System.Drawing.SystemColors.Control;
            this.txtOperationOrg.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOperationOrg.DrawSelf = false;
            this.txtOperationOrg.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOperationOrg.EnterToTab = false;
            this.txtOperationOrg.Location = new System.Drawing.Point(438, 20);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(254, 16);
            this.txtOperationOrg.TabIndex = 243;
            // 
            // lblPSelect
            // 
            this.lblPSelect.AutoSize = true;
            this.lblPSelect.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPSelect.Location = new System.Drawing.Point(379, 22);
            this.lblPSelect.Name = "lblPSelect";
            this.lblPSelect.Size = new System.Drawing.Size(59, 12);
            this.lblPSelect.TabIndex = 242;
            this.lblPSelect.Text = "范围选择:";
            this.lblPSelect.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSupply
            // 
            this.txtSupply.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupply.Code = null;
            this.txtSupply.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtSupply.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtSupply.EnterToTab = false;
            this.txtSupply.Id = "";
            this.txtSupply.Location = new System.Drawing.Point(104, 50);
            this.txtSupply.Name = "txtSupply";
            this.txtSupply.Result = ((System.Collections.IList)(resources.GetObject("txtSupply.Result")));
            this.txtSupply.RightMouse = false;
            this.txtSupply.Size = new System.Drawing.Size(239, 21);
            this.txtSupply.TabIndex = 241;
            this.txtSupply.Tag = null;
            this.txtSupply.Value = "";
            this.txtSupply.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // lblCat
            // 
            this.lblCat.AutoSize = true;
            this.lblCat.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCat.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCat.Location = new System.Drawing.Point(69, 54);
            this.lblCat.Name = "lblCat";
            this.lblCat.Size = new System.Drawing.Size(35, 12);
            this.lblCat.TabIndex = 240;
            this.lblCat.Text = "单位:";
            this.lblCat.UnderLineColor = System.Drawing.Color.Red;
            // 
            // datePeriodPicker1
            // 
            this.datePeriodPicker1.BeginValue = new System.DateTime(2016, 4, 22, 17, 25, 57, 659);
            this.datePeriodPicker1.EndValue = new System.DateTime(2016, 4, 22, 17, 25, 57, 657);
            this.datePeriodPicker1.Location = new System.Drawing.Point(104, 18);
            this.datePeriodPicker1.Name = "datePeriodPicker1";
            this.datePeriodPicker1.Size = new System.Drawing.Size(239, 20);
            this.datePeriodPicker1.TabIndex = 238;
            this.datePeriodPicker1.TheSameYear = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 239;
            this.label1.Text = "发票日期:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgMaster);
            this.groupBox1.Location = new System.Drawing.Point(12, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1002, 380);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
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
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRowNo,
            this.colCodeBill,
            this.colGSFGS,
            this.colProjectName,
            this.colPayOrg,
            this.colNSSBH,
            this.colPayType,
            this.colSupplierScale,
            this.colInvoiceType,
            this.colInvoiceCode,
            this.colInvoiceNo,
            this.colCreateDate,
            this.colMoney,
            this.colTaxRate,
            this.colTaxMoney,
            this.colBHSJE,
            this.colTransferType,
            this.colTransferTax,
            this.colIfDeduction,
            this.colStateBill,
            this.colCreatePersonBill,
            this.colRealOperationDateBill,
            this.colDescriptBill});
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
            this.dgMaster.Size = new System.Drawing.Size(996, 360);
            this.dgMaster.TabIndex = 11;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colRowNo
            // 
            this.colRowNo.HeaderText = "序号";
            this.colRowNo.Name = "colRowNo";
            this.colRowNo.ReadOnly = true;
            this.colRowNo.Width = 60;
            // 
            // colCodeBill
            // 
            this.colCodeBill.DataPropertyName = "Code";
            this.colCodeBill.HeaderText = "单据号";
            this.colCodeBill.Name = "colCodeBill";
            this.colCodeBill.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCodeBill.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCodeBill.Width = 150;
            // 
            // colGSFGS
            // 
            this.colGSFGS.DataPropertyName = "Temp1";
            this.colGSFGS.HeaderText = "归属分公司";
            this.colGSFGS.Name = "colGSFGS";
            // 
            // colProjectName
            // 
            this.colProjectName.DataPropertyName = "ProjectName";
            this.colProjectName.HeaderText = "归属项目";
            this.colProjectName.Name = "colProjectName";
            // 
            // colPayOrg
            // 
            this.colPayOrg.DataPropertyName = "TheSupplierName";
            this.colPayOrg.HeaderText = "单位";
            this.colPayOrg.Name = "colPayOrg";
            this.colPayOrg.Width = 200;
            // 
            // colNSSBH
            // 
            this.colNSSBH.DataPropertyName = "Temp2";
            this.colNSSBH.HeaderText = "纳税识别号";
            this.colNSSBH.Name = "colNSSBH";
            // 
            // colPayType
            // 
            this.colPayType.DataPropertyName = "AccountTitleName";
            this.colPayType.HeaderText = "款项类别";
            this.colPayType.Name = "colPayType";
            this.colPayType.Width = 90;
            // 
            // colSupplierScale
            // 
            this.colSupplierScale.DataPropertyName = "SupplierScale";
            this.colSupplierScale.HeaderText = "纳税人规模";
            this.colSupplierScale.Name = "colSupplierScale";
            this.colSupplierScale.ReadOnly = true;
            this.colSupplierScale.Width = 90;
            // 
            // colInvoiceType
            // 
            this.colInvoiceType.DataPropertyName = "InvoiceType";
            this.colInvoiceType.HeaderText = "发票类型";
            this.colInvoiceType.Name = "colInvoiceType";
            this.colInvoiceType.ReadOnly = true;
            this.colInvoiceType.Width = 90;
            // 
            // colInvoiceCode
            // 
            this.colInvoiceCode.DataPropertyName = "InvoiceCode";
            this.colInvoiceCode.HeaderText = "发票代码";
            this.colInvoiceCode.Name = "colInvoiceCode";
            this.colInvoiceCode.ReadOnly = true;
            this.colInvoiceCode.Width = 90;
            // 
            // colInvoiceNo
            // 
            this.colInvoiceNo.DataPropertyName = "InvoiceNo";
            this.colInvoiceNo.HeaderText = "发票号码";
            this.colInvoiceNo.Name = "colInvoiceNo";
            this.colInvoiceNo.ReadOnly = true;
            this.colInvoiceNo.Width = 90;
            // 
            // colCreateDate
            // 
            this.colCreateDate.DataPropertyName = "CreateDate";
            dataGridViewCellStyle1.Format = "yyyy-MM-dd";
            this.colCreateDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.colCreateDate.HeaderText = "发票日期";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.Width = 90;
            // 
            // colMoney
            // 
            this.colMoney.DataPropertyName = "SumMoney";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.colMoney.DefaultCellStyle = dataGridViewCellStyle2;
            this.colMoney.HeaderText = "发票金额";
            this.colMoney.Name = "colMoney";
            this.colMoney.Width = 90;
            // 
            // colTaxRate
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTaxRate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTaxRate.HeaderText = "税率";
            this.colTaxRate.Name = "colTaxRate";
            this.colTaxRate.ReadOnly = true;
            this.colTaxRate.Width = 80;
            // 
            // colTaxMoney
            // 
            this.colTaxMoney.DataPropertyName = "TaxMoney";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTaxMoney.DefaultCellStyle = dataGridViewCellStyle4;
            this.colTaxMoney.HeaderText = "税金";
            this.colTaxMoney.Name = "colTaxMoney";
            this.colTaxMoney.ReadOnly = true;
            this.colTaxMoney.Width = 80;
            // 
            // colBHSJE
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colBHSJE.DefaultCellStyle = dataGridViewCellStyle5;
            this.colBHSJE.HeaderText = "不含税金额";
            this.colBHSJE.Name = "colBHSJE";
            // 
            // colTransferType
            // 
            this.colTransferType.DataPropertyName = "TransferType";
            this.colTransferType.HeaderText = "转出类型";
            this.colTransferType.Name = "colTransferType";
            // 
            // colTransferTax
            // 
            this.colTransferTax.DataPropertyName = "TransferTax";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTransferTax.DefaultCellStyle = dataGridViewCellStyle6;
            this.colTransferTax.HeaderText = "转出税额";
            this.colTransferTax.Name = "colTransferTax";
            // 
            // colIfDeduction
            // 
            this.colIfDeduction.DataPropertyName = "IfDeduction";
            this.colIfDeduction.HeaderText = "是否抵扣分包";
            this.colIfDeduction.Name = "colIfDeduction";
            this.colIfDeduction.ReadOnly = true;
            // 
            // colStateBill
            // 
            this.colStateBill.HeaderText = "状态";
            this.colStateBill.Name = "colStateBill";
            this.colStateBill.Width = 80;
            // 
            // colCreatePersonBill
            // 
            this.colCreatePersonBill.DataPropertyName = "CreatePersonName";
            this.colCreatePersonBill.HeaderText = "制单人";
            this.colCreatePersonBill.Name = "colCreatePersonBill";
            this.colCreatePersonBill.Width = 80;
            // 
            // colRealOperationDateBill
            // 
            this.colRealOperationDateBill.DataPropertyName = "RealOperationDate";
            this.colRealOperationDateBill.HeaderText = "制单日期";
            this.colRealOperationDateBill.Name = "colRealOperationDateBill";
            this.colRealOperationDateBill.Width = 150;
            // 
            // colDescriptBill
            // 
            this.colDescriptBill.DataPropertyName = "Descript";
            this.colDescriptBill.HeaderText = "备注";
            this.colDescriptBill.Name = "colDescriptBill";
            // 
            // VPaymentInvoiceQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 494);
            this.Name = "VPaymentInvoiceQuery";
            this.Text = "付款发票查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupply)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNo;
        private System.Windows.Forms.DataGridViewLinkColumn colCodeBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGSFGS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayOrg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNSSBH;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaxRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaxMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBHSJE;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransferType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransferTax;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIfDeduction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStateBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePersonBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperationDateBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescriptBill;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcelBill;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearchBill;
        private System.Windows.Forms.ComboBox cmbInvoiceType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPSelect;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCat;
        private Application.Business.Erp.SupplyChain.Client.Basic.Controls.DatePeriodPicker datePeriodPicker1;
        private System.Windows.Forms.Label label1;
    }
}