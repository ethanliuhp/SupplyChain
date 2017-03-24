namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI
{
    partial class VStockOutRed2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VStockOutRed2));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.txtContractNo = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnForward = new System.Windows.Forms.Button();
            this.txtStockOutPurpose = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtStationCategory = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtForward = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtCustomer = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.groupDetails = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblTally = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.MaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaterialSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoRefQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Money = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromFactory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromContract = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Batch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.theManageState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlForward = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockOutDtl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColQuantityTemp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlContent.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlFloor.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            this.groupDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Size = new System.Drawing.Size(856, 550);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.groupDetails);
            this.pnlBody.Controls.Add(this.groupSupply);
            this.pnlBody.Location = new System.Drawing.Point(0, 40);
            this.pnlBody.Size = new System.Drawing.Size(854, 462);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.lblTally);
            this.pnlFooter.Controls.Add(this.txtCreateDate);
            this.pnlFooter.Controls.Add(this.customLabel6);
            this.pnlFooter.Controls.Add(this.customLabel5);
            this.pnlFooter.Controls.Add(this.txtCreatePerson);
            this.pnlFooter.Location = new System.Drawing.Point(0, 502);
            this.pnlFooter.Size = new System.Drawing.Size(854, 46);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(856, 0);
            // 
            // pnlFloor
            // 
            this.pnlFloor.Size = new System.Drawing.Size(856, 550);
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Location = new System.Drawing.Point(0, 0);
            this.pnlWorkSpace.Size = new System.Drawing.Size(856, 550);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Size = new System.Drawing.Size(854, 40);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(395, 10);
            this.lblTitle.Size = new System.Drawing.Size(115, 20);
            this.lblTitle.Text = " 出库红单 ";
            // 
            // groupSupply
            // 
            this.groupSupply.Controls.Add(this.txtContractNo);
            this.groupSupply.Controls.Add(this.customLabel8);
            this.groupSupply.Controls.Add(this.btnForward);
            this.groupSupply.Controls.Add(this.txtStockOutPurpose);
            this.groupSupply.Controls.Add(this.txtStationCategory);
            this.groupSupply.Controls.Add(this.txtForward);
            this.groupSupply.Controls.Add(this.txtCustomer);
            this.groupSupply.Controls.Add(this.txtRemark);
            this.groupSupply.Controls.Add(this.customLabel3);
            this.groupSupply.Controls.Add(this.customLabel2);
            this.groupSupply.Controls.Add(this.customLabel7);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.customLabel1);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.lblSupplyContract);
            this.groupSupply.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupSupply.Location = new System.Drawing.Point(6, 0);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(842, 85);
            this.groupSupply.TabIndex = 1;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>单据信息";
            // 
            // txtContractNo
            // 
            this.txtContractNo.BackColor = System.Drawing.SystemColors.Control;
            this.txtContractNo.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtContractNo.DrawSelf = false;
            this.txtContractNo.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtContractNo.EnterToTab = false;
            this.txtContractNo.Location = new System.Drawing.Point(720, 30);
            this.txtContractNo.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.Padding = new System.Windows.Forms.Padding(1);
            this.txtContractNo.ReadOnly = false;
            this.txtContractNo.Size = new System.Drawing.Size(108, 16);
            this.txtContractNo.TabIndex = 26;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(674, 34);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(47, 12);
            this.customLabel8.TabIndex = 25;
            this.customLabel8.Text = "合同号:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnForward
            // 
            this.btnForward.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.dot;
            this.btnForward.Location = new System.Drawing.Point(342, 29);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(21, 18);
            this.btnForward.TabIndex = 19;
            this.btnForward.UseVisualStyleBackColor = true;
            // 
            // txtStockOutPurpose
            // 
            this.txtStockOutPurpose.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtStockOutPurpose.DrawSelf = false;
            this.txtStockOutPurpose.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtStockOutPurpose.EnterToTab = false;
            this.txtStockOutPurpose.Location = new System.Drawing.Point(568, 30);
            this.txtStockOutPurpose.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtStockOutPurpose.Name = "txtStockOutPurpose";
            this.txtStockOutPurpose.Padding = new System.Windows.Forms.Padding(1);
            this.txtStockOutPurpose.ReadOnly = false;
            this.txtStockOutPurpose.Size = new System.Drawing.Size(102, 16);
            this.txtStockOutPurpose.TabIndex = 18;
            // 
            // txtStationCategory
            // 
            this.txtStationCategory.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtStationCategory.DrawSelf = false;
            this.txtStationCategory.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtStationCategory.EnterToTab = false;
            this.txtStationCategory.Location = new System.Drawing.Point(403, 30);
            this.txtStationCategory.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtStationCategory.Name = "txtStationCategory";
            this.txtStationCategory.Padding = new System.Windows.Forms.Padding(1);
            this.txtStationCategory.ReadOnly = false;
            this.txtStationCategory.Size = new System.Drawing.Size(102, 16);
            this.txtStationCategory.TabIndex = 18;
            // 
            // txtForward
            // 
            this.txtForward.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtForward.DrawSelf = false;
            this.txtForward.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtForward.EnterToTab = false;
            this.txtForward.Location = new System.Drawing.Point(230, 31);
            this.txtForward.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtForward.Name = "txtForward";
            this.txtForward.Padding = new System.Windows.Forms.Padding(1);
            this.txtForward.ReadOnly = false;
            this.txtForward.Size = new System.Drawing.Size(105, 16);
            this.txtForward.TabIndex = 18;
            // 
            // txtCustomer
            // 
            this.txtCustomer.BackColor = System.Drawing.SystemColors.Control;
            this.txtCustomer.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCustomer.DrawSelf = false;
            this.txtCustomer.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCustomer.EnterToTab = false;
            this.txtCustomer.Location = new System.Drawing.Point(57, 59);
            this.txtCustomer.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Padding = new System.Windows.Forms.Padding(1);
            this.txtCustomer.ReadOnly = false;
            this.txtCustomer.Size = new System.Drawing.Size(190, 16);
            this.txtCustomer.TabIndex = 16;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(301, 59);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(527, 16);
            this.txtRemark.TabIndex = 10;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(260, 63);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(35, 12);
            this.customLabel3.TabIndex = 9;
            this.customLabel3.Text = "备注:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(18, 63);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(35, 12);
            this.customLabel2.TabIndex = 7;
            this.customLabel2.Text = "客户:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(373, 32);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(35, 12);
            this.customLabel7.TabIndex = 6;
            this.customLabel7.Text = "仓库:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(511, 32);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 6;
            this.customLabel4.Text = "出库用途:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(168, 35);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 6;
            this.customLabel1.Text = "前驱料单:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(57, 31);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = false;
            this.txtCode.Size = new System.Drawing.Size(104, 16);
            this.txtCode.TabIndex = 5;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(18, 35);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(35, 12);
            this.lblSupplyContract.TabIndex = 0;
            this.lblSupplyContract.Text = "单号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
            // 
            // groupDetails
            // 
            this.groupDetails.Controls.Add(this.dgDetail);
            this.groupDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupDetails.Location = new System.Drawing.Point(6, 85);
            this.groupDetails.Name = "groupDetails";
            this.groupDetails.Size = new System.Drawing.Size(842, 377);
            this.groupDetails.TabIndex = 0;
            this.groupDetails.TabStop = false;
            this.groupDetails.Text = ">>明细信息";
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaterialName,
            this.MaterialSpec,
            this.Unit,
            this.Quantity,
            this.NoRefQuantity,
            this.Price,
            this.Money,
            this.Remark,
            this.fromSupplier,
            this.fromFactory,
            this.Brand,
            this.fromContract,
            this.Batch,
            this.MaterialCode,
            this.theManageState,
            this.DtlForward,
            this.StockOutDtl,
            this.maxQuantity,
            this.ColQuantityTemp});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(3, 17);
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
            this.dgDetail.Size = new System.Drawing.Size(836, 357);
            this.dgDetail.TabIndex = 0;
            this.dgDetail.TargetType = null;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(66, 6);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = false;
            this.txtCreatePerson.Size = new System.Drawing.Size(79, 16);
            this.txtCreatePerson.TabIndex = 10;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(15, 10);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(47, 12);
            this.customLabel5.TabIndex = 9;
            this.customLabel5.Text = "制单人:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreateDate
            // 
            this.txtCreateDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreateDate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreateDate.DrawSelf = false;
            this.txtCreateDate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreateDate.EnterToTab = false;
            this.txtCreateDate.Location = new System.Drawing.Point(216, 6);
            this.txtCreateDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateDate.ReadOnly = false;
            this.txtCreateDate.Size = new System.Drawing.Size(79, 16);
            this.txtCreateDate.TabIndex = 12;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(151, 10);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 11;
            this.customLabel6.Text = "制单日期:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblTally
            // 
            this.lblTally.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTally.AutoSize = true;
            this.lblTally.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTally.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblTally.ForeColor = System.Drawing.Color.Red;
            this.lblTally.Location = new System.Drawing.Point(360, 10);
            this.lblTally.Name = "lblTally";
            this.lblTally.Size = new System.Drawing.Size(11, 12);
            this.lblTally.TabIndex = 32;
            this.lblTally.Text = ":";
            this.lblTally.UnderLineColor = System.Drawing.Color.Red;
            // 
            // MaterialName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.MaterialName.DefaultCellStyle = dataGridViewCellStyle1;
            this.MaterialName.HeaderText = "物料名称";
            this.MaterialName.Name = "MaterialName";
            this.MaterialName.ReadOnly = true;
            // 
            // MaterialSpec
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.MaterialSpec.DefaultCellStyle = dataGridViewCellStyle2;
            this.MaterialSpec.HeaderText = "物料规格";
            this.MaterialSpec.Name = "MaterialSpec";
            this.MaterialSpec.ReadOnly = true;
            // 
            // Unit
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.Unit.DefaultCellStyle = dataGridViewCellStyle3;
            this.Unit.HeaderText = "计量单位";
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            this.Unit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Quantity
            // 
            this.Quantity.HeaderText = "数量";
            this.Quantity.Name = "Quantity";
            // 
            // NoRefQuantity
            // 
            this.NoRefQuantity.HeaderText = "未结算数量";
            this.NoRefQuantity.Name = "NoRefQuantity";
            this.NoRefQuantity.Visible = false;
            // 
            // Price
            // 
            this.Price.HeaderText = "单价";
            this.Price.Name = "Price";
            // 
            // Money
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.Money.DefaultCellStyle = dataGridViewCellStyle4;
            this.Money.HeaderText = "金额";
            this.Money.Name = "Money";
            this.Money.ReadOnly = true;
            // 
            // Remark
            // 
            this.Remark.HeaderText = "备注";
            this.Remark.Name = "Remark";
            // 
            // fromSupplier
            // 
            this.fromSupplier.HeaderText = "供应商";
            this.fromSupplier.Name = "fromSupplier";
            // 
            // fromFactory
            // 
            this.fromFactory.HeaderText = "厂家";
            this.fromFactory.Name = "fromFactory";
            // 
            // Brand
            // 
            this.Brand.HeaderText = "品牌";
            this.Brand.Name = "Brand";
            this.Brand.Width = 80;
            // 
            // fromContract
            // 
            this.fromContract.HeaderText = "采购合同号";
            this.fromContract.Name = "fromContract";
            // 
            // Batch
            // 
            this.Batch.HeaderText = "批号";
            this.Batch.Name = "Batch";
            // 
            // MaterialCode
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.MaterialCode.DefaultCellStyle = dataGridViewCellStyle5;
            this.MaterialCode.HeaderText = "物料资源";
            this.MaterialCode.Name = "MaterialCode";
            this.MaterialCode.ReadOnly = true;
            // 
            // theManageState
            // 
            this.theManageState.HeaderText = "管理实例";
            this.theManageState.Name = "theManageState";
            this.theManageState.Visible = false;
            // 
            // DtlForward
            // 
            this.DtlForward.HeaderText = "明细前驱";
            this.DtlForward.Name = "DtlForward";
            this.DtlForward.ReadOnly = true;
            this.DtlForward.Visible = false;
            // 
            // StockOutDtl
            // 
            this.StockOutDtl.HeaderText = "出库蓝单明细";
            this.StockOutDtl.Name = "StockOutDtl";
            this.StockOutDtl.Visible = false;
            // 
            // maxQuantity
            // 
            this.maxQuantity.HeaderText = "最大冲红量";
            this.maxQuantity.Name = "maxQuantity";
            this.maxQuantity.Visible = false;
            // 
            // ColQuantityTemp
            // 
            this.ColQuantityTemp.HeaderText = "临时数量";
            this.ColQuantityTemp.Name = "ColQuantityTemp";
            this.ColQuantityTemp.Visible = false;
            // 
            // VStockOutRed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 550);
            this.Name = "VStockOutRed";
            this.Text = "出库红单";
            this.pnlContent.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlFloor.ResumeLayout(false);
            this.pnlWorkSpace.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            this.groupDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupDetails;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCustomer;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtForward;
        private System.Windows.Forms.Button btnForward;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtStationCategory;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtStockOutPurpose;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtContractNo;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblTally;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaterialSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoRefQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Money;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromFactory;
        private System.Windows.Forms.DataGridViewTextBoxColumn Brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromContract;
        private System.Windows.Forms.DataGridViewTextBoxColumn Batch;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn theManageState;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlForward;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockOutDtl;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColQuantityTemp;
    }
}