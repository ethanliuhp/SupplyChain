namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder
{
    partial class VStockBlockMaterial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VStockBlockMaterial));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.txtCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel11 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProjectName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupply = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnForward = new System.Windows.Forms.Button();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtContract = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplier = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel10 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtBalRule = new System.Windows.Forms.ComboBox();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtTheme = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtStockReason = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLeftQuanity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsedPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBorrowUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSumMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtOldCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel12 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.dgDetail);
            this.pnlBody.Location = new System.Drawing.Point(0, 136);
            this.pnlBody.Size = new System.Drawing.Size(974, 364);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtSumMoney);
            this.pnlFooter.Controls.Add(this.customLabel1);
            this.pnlFooter.Controls.Add(this.txtCreatePerson);
            this.pnlFooter.Controls.Add(this.customLabel5);
            this.pnlFooter.Location = new System.Drawing.Point(0, 500);
            this.pnlFooter.Size = new System.Drawing.Size(974, 38);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(974, 0);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.groupSupply);
            this.pnlHeader.Size = new System.Drawing.Size(974, 136);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.groupSupply, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(304, 0);
            this.lblTitle.Visible = false;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(583, 14);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(122, 21);
            this.dtpDateBegin.TabIndex = 2;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(519, 18);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 130;
            this.customLabel4.Text = "开始日期:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // groupSupply
            // 
            this.groupSupply.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSupply.Controls.Add(this.txtOldCode);
            this.groupSupply.Controls.Add(this.customLabel12);
            this.groupSupply.Controls.Add(this.txtCreateDate);
            this.groupSupply.Controls.Add(this.customLabel11);
            this.groupSupply.Controls.Add(this.txtProjectName);
            this.groupSupply.Controls.Add(this.customLabel7);
            this.groupSupply.Controls.Add(this.txtSupply);
            this.groupSupply.Controls.Add(this.btnForward);
            this.groupSupply.Controls.Add(this.customLabel9);
            this.groupSupply.Controls.Add(this.txtContract);
            this.groupSupply.Controls.Add(this.lblSupplier);
            this.groupSupply.Controls.Add(this.customLabel10);
            this.groupSupply.Controls.Add(this.txtBalRule);
            this.groupSupply.Controls.Add(this.dtpDateEnd);
            this.groupSupply.Controls.Add(this.customLabel2);
            this.groupSupply.Controls.Add(this.dtpDateBegin);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.txtRemark);
            this.groupSupply.Controls.Add(this.txtTheme);
            this.groupSupply.Controls.Add(this.txtStockReason);
            this.groupSupply.Controls.Add(this.customLabel8);
            this.groupSupply.Controls.Add(this.customLabel6);
            this.groupSupply.Controls.Add(this.customLabel3);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.lblSupplyContract);
            this.groupSupply.Location = new System.Drawing.Point(2, 10);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(960, 120);
            this.groupSupply.TabIndex = 3;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>单据信息";
            // 
            // txtCreateDate
            // 
            this.txtCreateDate.Location = new System.Drawing.Point(374, 14);
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Size = new System.Drawing.Size(122, 21);
            this.txtCreateDate.TabIndex = 159;
            // 
            // customLabel11
            // 
            this.customLabel11.AutoSize = true;
            this.customLabel11.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel11.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel11.Location = new System.Drawing.Point(313, 18);
            this.customLabel11.Name = "customLabel11";
            this.customLabel11.Size = new System.Drawing.Size(59, 12);
            this.customLabel11.TabIndex = 160;
            this.customLabel11.Text = "业务日期:";
            this.customLabel11.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProjectName
            // 
            this.txtProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProjectName.BackColor = System.Drawing.SystemColors.Control;
            this.txtProjectName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProjectName.DrawSelf = false;
            this.txtProjectName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProjectName.EnterToTab = false;
            this.txtProjectName.Location = new System.Drawing.Point(583, 46);
            this.txtProjectName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Padding = new System.Windows.Forms.Padding(1);
            this.txtProjectName.ReadOnly = true;
            this.txtProjectName.Size = new System.Drawing.Size(125, 16);
            this.txtProjectName.TabIndex = 158;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(531, 49);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(47, 12);
            this.customLabel7.TabIndex = 157;
            this.customLabel7.Text = "租赁方:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSupply
            // 
            this.txtSupply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSupply.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupply.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSupply.DrawSelf = false;
            this.txtSupply.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSupply.EnterToTab = false;
            this.txtSupply.Location = new System.Drawing.Point(374, 45);
            this.txtSupply.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSupply.Name = "txtSupply";
            this.txtSupply.Padding = new System.Windows.Forms.Padding(1);
            this.txtSupply.ReadOnly = true;
            this.txtSupply.Size = new System.Drawing.Size(123, 16);
            this.txtSupply.TabIndex = 156;
            // 
            // btnForward
            // 
            this.btnForward.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.dot;
            this.btnForward.Location = new System.Drawing.Point(254, 44);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(21, 18);
            this.btnForward.TabIndex = 155;
            this.btnForward.UseVisualStyleBackColor = true;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(22, 51);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(59, 12);
            this.customLabel9.TabIndex = 154;
            this.customLabel9.Text = "租赁合同:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtContract
            // 
            this.txtContract.BackColor = System.Drawing.SystemColors.Control;
            this.txtContract.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtContract.DrawSelf = false;
            this.txtContract.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtContract.EnterToTab = false;
            this.txtContract.Location = new System.Drawing.Point(84, 46);
            this.txtContract.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtContract.Name = "txtContract";
            this.txtContract.Padding = new System.Windows.Forms.Padding(1);
            this.txtContract.ReadOnly = true;
            this.txtContract.Size = new System.Drawing.Size(171, 16);
            this.txtContract.TabIndex = 153;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplier.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplier.Location = new System.Drawing.Point(325, 48);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(47, 12);
            this.lblSupplier.TabIndex = 152;
            this.lblSupplier.Text = "出租方:";
            this.lblSupplier.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel10
            // 
            this.customLabel10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel10.AutoSize = true;
            this.customLabel10.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel10.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel10.Location = new System.Drawing.Point(732, 45);
            this.customLabel10.Name = "customLabel10";
            this.customLabel10.Size = new System.Drawing.Size(59, 12);
            this.customLabel10.TabIndex = 146;
            this.customLabel10.Text = "结算规则:";
            this.customLabel10.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtBalRule
            // 
            this.txtBalRule.Enabled = false;
            this.txtBalRule.FormattingEnabled = true;
            this.txtBalRule.Location = new System.Drawing.Point(799, 40);
            this.txtBalRule.Name = "txtBalRule";
            this.txtBalRule.Size = new System.Drawing.Size(110, 20);
            this.txtBalRule.TabIndex = 145;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(799, 14);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(122, 21);
            this.dtpDateEnd.TabIndex = 2;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(732, 18);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 130;
            this.customLabel2.Text = "结束日期:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(245, 96);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(699, 16);
            this.txtRemark.TabIndex = 6;
            // 
            // txtTheme
            // 
            this.txtTheme.BackColor = System.Drawing.SystemColors.Control;
            this.txtTheme.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtTheme.DrawSelf = false;
            this.txtTheme.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtTheme.EnterToTab = false;
            this.txtTheme.Location = new System.Drawing.Point(89, 74);
            this.txtTheme.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtTheme.Name = "txtTheme";
            this.txtTheme.Padding = new System.Windows.Forms.Padding(1);
            this.txtTheme.ReadOnly = false;
            this.txtTheme.Size = new System.Drawing.Size(328, 16);
            this.txtTheme.TabIndex = 6;
            // 
            // txtStockReason
            // 
            this.txtStockReason.BackColor = System.Drawing.SystemColors.Control;
            this.txtStockReason.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtStockReason.DrawSelf = false;
            this.txtStockReason.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtStockReason.EnterToTab = false;
            this.txtStockReason.Location = new System.Drawing.Point(488, 74);
            this.txtStockReason.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtStockReason.Name = "txtStockReason";
            this.txtStockReason.Padding = new System.Windows.Forms.Padding(1);
            this.txtStockReason.ReadOnly = false;
            this.txtStockReason.Size = new System.Drawing.Size(456, 16);
            this.txtStockReason.TabIndex = 6;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(36, 75);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(35, 12);
            this.customLabel8.TabIndex = 9;
            this.customLabel8.Text = "主题:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(211, 100);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(35, 12);
            this.customLabel6.TabIndex = 9;
            this.customLabel6.Text = "备注:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(427, 75);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 9;
            this.customLabel3.Text = "封存事由:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(85, 19);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(192, 16);
            this.txtCode.TabIndex = 1;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(36, 21);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(47, 12);
            this.lblSupplyContract.TabIndex = 0;
            this.lblSupplyContract.Text = "单据号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = true;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colStockId,
            this.colMaterialCode,
            this.colMaterialName,
            this.colMaterialSpec,
            this.colType,
            this.colLength,
            this.colLeftQuanity,
            this.colQuantity,
            this.colUnit,
            this.colPrice,
            this.colMoney,
            this.colSubject,
            this.colUsedPart,
            this.colBorrowUnit,
            this.colDescript});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(6, 0);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.PageIndex = 0;
            this.dgDetail.PageSize = 0;
            this.dgDetail.Query = null;
            this.dgDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDetail.ReadOnlyCols")));
            this.dgDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetail.Size = new System.Drawing.Size(962, 364);
            this.dgDetail.TabIndex = 8;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colId
            // 
            this.colId.HeaderText = "封存明细ID";
            this.colId.Name = "colId";
            this.colId.Visible = false;
            // 
            // colStockId
            // 
            this.colStockId.HeaderText = "库存ID";
            this.colStockId.Name = "colStockId";
            this.colStockId.Visible = false;
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
            this.colMaterialName.ReadOnly = true;
            // 
            // colMaterialSpec
            // 
            this.colMaterialSpec.HeaderText = "规格型号";
            this.colMaterialSpec.Name = "colMaterialSpec";
            this.colMaterialSpec.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.HeaderText = "型号";
            this.colType.Name = "colType";
            this.colType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colLength
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colLength.DefaultCellStyle = dataGridViewCellStyle3;
            this.colLength.HeaderText = "长度";
            this.colLength.Name = "colLength";
            this.colLength.ReadOnly = true;
            // 
            // colLeftQuanity
            // 
            this.colLeftQuanity.HeaderText = "项目在用量";
            this.colLeftQuanity.Name = "colLeftQuanity";
            this.colLeftQuanity.ReadOnly = true;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "数量";
            this.colQuantity.Name = "colQuantity";
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "单位";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            // 
            // colPrice
            // 
            this.colPrice.HeaderText = "价格";
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            // 
            // colMoney
            // 
            this.colMoney.HeaderText = "金额";
            this.colMoney.Name = "colMoney";
            this.colMoney.ReadOnly = true;
            // 
            // colSubject
            // 
            this.colSubject.HeaderText = "科目";
            this.colSubject.Name = "colSubject";
            this.colSubject.ReadOnly = true;
            // 
            // colUsedPart
            // 
            this.colUsedPart.HeaderText = "部位";
            this.colUsedPart.Name = "colUsedPart";
            this.colUsedPart.ReadOnly = true;
            // 
            // colBorrowUnit
            // 
            this.colBorrowUnit.HeaderText = "使用单位";
            this.colBorrowUnit.Name = "colBorrowUnit";
            this.colBorrowUnit.ReadOnly = true;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 200;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(60, 16);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = true;
            this.txtCreatePerson.Size = new System.Drawing.Size(69, 16);
            this.txtCreatePerson.TabIndex = 44;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(14, 20);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(47, 12);
            this.customLabel5.TabIndex = 46;
            this.customLabel5.Text = "制单人:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSumMoney
            // 
            this.txtSumMoney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSumMoney.BackColor = System.Drawing.SystemColors.Control;
            this.txtSumMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumMoney.DrawSelf = false;
            this.txtSumMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumMoney.EnterToTab = false;
            this.txtSumMoney.Location = new System.Drawing.Point(188, 16);
            this.txtSumMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumMoney.Name = "txtSumMoney";
            this.txtSumMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumMoney.ReadOnly = true;
            this.txtSumMoney.Size = new System.Drawing.Size(69, 16);
            this.txtSumMoney.TabIndex = 50;
            // 
            // customLabel1
            // 
            this.customLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(151, 20);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(35, 12);
            this.customLabel1.TabIndex = 51;
            this.customLabel1.Text = "金额:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtOldCode
            // 
            this.txtOldCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtOldCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOldCode.DrawSelf = false;
            this.txtOldCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOldCode.EnterToTab = false;
            this.txtOldCode.Location = new System.Drawing.Point(85, 96);
            this.txtOldCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOldCode.Name = "txtOldCode";
            this.txtOldCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtOldCode.ReadOnly = false;
            this.txtOldCode.Size = new System.Drawing.Size(109, 16);
            this.txtOldCode.TabIndex = 161;
            // 
            // customLabel12
            // 
            this.customLabel12.AutoSize = true;
            this.customLabel12.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel12.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel12.Location = new System.Drawing.Point(20, 100);
            this.customLabel12.Name = "customLabel12";
            this.customLabel12.Size = new System.Drawing.Size(59, 12);
            this.customLabel12.TabIndex = 162;
            this.customLabel12.Text = "原单据号:";
            this.customLabel12.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VStockBlockMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 538);
            this.Name = "VStockBlockMaterial";
            this.Text = "料具封存单";
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtStockReason;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtTheme;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel10;
        private System.Windows.Forms.ComboBox txtBalRule;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProjectName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSupply;
        private System.Windows.Forms.Button btnForward;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtContract;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLeftQuanity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsedPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBorrowUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker txtCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel11;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOldCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel12;
    }
}