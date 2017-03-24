namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConCheckingMng
{
    partial class VConCheck
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VConCheck));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmoCreateTime = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtAdjustmentMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnCreateCheck = new System.Windows.Forms.Button();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnForward = new System.Windows.Forms.Button();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customEdit1 = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtHandlePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblHandlePerson = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupply = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblSupplier = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTicketVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLessPumpVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeductionVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBalVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsPump = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colUsedPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTempData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlanQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddCostDetial = new System.Windows.Forms.ToolStripMenuItem();
            this.dgDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.txtProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSumVolume = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lbSumQuantity = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSumMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lableSumMoney = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlContent.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlFloor.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupply)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Size = new System.Drawing.Size(1000, 535);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl1);
            this.pnlBody.Location = new System.Drawing.Point(0, 86);
            this.pnlBody.Size = new System.Drawing.Size(998, 409);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtSumMoney);
            this.pnlFooter.Controls.Add(this.lableSumMoney);
            this.pnlFooter.Controls.Add(this.txtProject);
            this.pnlFooter.Controls.Add(this.customLabel5);
            this.pnlFooter.Controls.Add(this.txtCreateDate);
            this.pnlFooter.Controls.Add(this.customLabel6);
            this.pnlFooter.Controls.Add(this.txtSumVolume);
            this.pnlFooter.Controls.Add(this.lbSumQuantity);
            this.pnlFooter.Controls.Add(this.txtCreatePerson);
            this.pnlFooter.Controls.Add(this.customLabel7);
            this.pnlFooter.Location = new System.Drawing.Point(0, 495);
            this.pnlFooter.Size = new System.Drawing.Size(998, 38);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(1000, 2);
            // 
            // pnlFloor
            // 
            this.pnlFloor.Size = new System.Drawing.Size(1000, 537);
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Location = new System.Drawing.Point(0, 2);
            this.pnlWorkSpace.Size = new System.Drawing.Size(1000, 535);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.groupSupply);
            this.pnlHeader.Size = new System.Drawing.Size(998, 86);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.groupSupply, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(405, 2);
            this.lblTitle.Visible = false;
            // 
            // groupSupply
            // 
            this.groupSupply.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSupply.Controls.Add(this.customLabel8);
            this.groupSupply.Controls.Add(this.cmoCreateTime);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.txtAdjustmentMoney);
            this.groupSupply.Controls.Add(this.btnCreateCheck);
            this.groupSupply.Controls.Add(this.dtpDateEnd);
            this.groupSupply.Controls.Add(this.customLabel2);
            this.groupSupply.Controls.Add(this.btnForward);
            this.groupSupply.Controls.Add(this.customLabel1);
            this.groupSupply.Controls.Add(this.customEdit1);
            this.groupSupply.Controls.Add(this.txtHandlePerson);
            this.groupSupply.Controls.Add(this.lblHandlePerson);
            this.groupSupply.Controls.Add(this.txtSupply);
            this.groupSupply.Controls.Add(this.txtRemark);
            this.groupSupply.Controls.Add(this.customLabel3);
            this.groupSupply.Controls.Add(this.lblSupplier);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.lblSupplyContract);
            this.groupSupply.Location = new System.Drawing.Point(3, 4);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(985, 76);
            this.groupSupply.TabIndex = 6;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>单据信息";
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(741, 20);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(59, 12);
            this.customLabel8.TabIndex = 143;
            this.customLabel8.Text = "对账时间:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmoCreateTime
            // 
            this.cmoCreateTime.CustomFormat = "yyyy-MM-dd";
            this.cmoCreateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.cmoCreateTime.Location = new System.Drawing.Point(810, 16);
            this.cmoCreateTime.Name = "cmoCreateTime";
            this.cmoCreateTime.Size = new System.Drawing.Size(109, 21);
            this.cmoCreateTime.TabIndex = 142;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(432, -2);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 141;
            this.customLabel4.Text = "调整金额:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            this.customLabel4.Visible = false;
            // 
            // txtAdjustmentMoney
            // 
            this.txtAdjustmentMoney.BackColor = System.Drawing.SystemColors.Control;
            this.txtAdjustmentMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtAdjustmentMoney.DrawSelf = false;
            this.txtAdjustmentMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtAdjustmentMoney.EnterToTab = false;
            this.txtAdjustmentMoney.Location = new System.Drawing.Point(497, -4);
            this.txtAdjustmentMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtAdjustmentMoney.Name = "txtAdjustmentMoney";
            this.txtAdjustmentMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtAdjustmentMoney.ReadOnly = false;
            this.txtAdjustmentMoney.Size = new System.Drawing.Size(86, 16);
            this.txtAdjustmentMoney.TabIndex = 140;
            this.txtAdjustmentMoney.Visible = false;
            // 
            // btnCreateCheck
            // 
            this.btnCreateCheck.Location = new System.Drawing.Point(751, 47);
            this.btnCreateCheck.Name = "btnCreateCheck";
            this.btnCreateCheck.Size = new System.Drawing.Size(92, 23);
            this.btnCreateCheck.TabIndex = 139;
            this.btnCreateCheck.Text = "生成对账单";
            this.btnCreateCheck.UseVisualStyleBackColor = true;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.CustomFormat = "yyyy-MM-dd";
            this.dtpDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDateEnd.Location = new System.Drawing.Point(353, 17);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 137;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(288, 21);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 138;
            this.customLabel2.Text = "结束时间:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnForward
            // 
            this.btnForward.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.dot;
            this.btnForward.Location = new System.Drawing.Point(249, 16);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(21, 18);
            this.btnForward.TabIndex = 136;
            this.btnForward.UseVisualStyleBackColor = true;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(12, 21);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(71, 12);
            this.customLabel1.TabIndex = 133;
            this.customLabel1.Text = "采购合同号:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customEdit1
            // 
            this.customEdit1.BackColor = System.Drawing.SystemColors.Control;
            this.customEdit1.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.customEdit1.DrawSelf = false;
            this.customEdit1.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.customEdit1.EnterToTab = false;
            this.customEdit1.Location = new System.Drawing.Point(91, 17);
            this.customEdit1.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.customEdit1.Name = "customEdit1";
            this.customEdit1.Padding = new System.Windows.Forms.Padding(1);
            this.customEdit1.ReadOnly = true;
            this.customEdit1.Size = new System.Drawing.Size(152, 16);
            this.customEdit1.TabIndex = 132;
            // 
            // txtHandlePerson
            // 
            this.txtHandlePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtHandlePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtHandlePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtHandlePerson.DrawSelf = false;
            this.txtHandlePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtHandlePerson.EnterToTab = false;
            this.txtHandlePerson.Location = new System.Drawing.Point(751, -41);
            this.txtHandlePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtHandlePerson.ReadOnly = true;
            this.txtHandlePerson.Size = new System.Drawing.Size(111, 16);
            this.txtHandlePerson.TabIndex = 3;
            this.txtHandlePerson.Visible = false;
            // 
            // lblHandlePerson
            // 
            this.lblHandlePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHandlePerson.AutoSize = true;
            this.lblHandlePerson.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHandlePerson.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblHandlePerson.Location = new System.Drawing.Point(701, -37);
            this.lblHandlePerson.Name = "lblHandlePerson";
            this.lblHandlePerson.Size = new System.Drawing.Size(47, 12);
            this.lblHandlePerson.TabIndex = 131;
            this.lblHandlePerson.Text = "责任人:";
            this.lblHandlePerson.UnderLineColor = System.Drawing.Color.Red;
            this.lblHandlePerson.Visible = false;
            // 
            // txtSupply
            // 
            this.txtSupply.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupply.Code = null;
            this.txtSupply.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtSupply.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtSupply.EnterToTab = false;
            this.txtSupply.Id = "";
            this.txtSupply.Location = new System.Drawing.Point(527, 17);
            this.txtSupply.Name = "txtSupply";
            this.txtSupply.Result = ((System.Collections.IList)(resources.GetObject("txtSupply.Result")));
            this.txtSupply.RightMouse = false;
            this.txtSupply.Size = new System.Drawing.Size(193, 21);
            this.txtSupply.TabIndex = 5;
            this.txtSupply.Tag = null;
            this.txtSupply.Value = "";
            this.txtSupply.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(307, 50);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(441, 16);
            this.txtRemark.TabIndex = 6;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(265, 52);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(35, 12);
            this.customLabel3.TabIndex = 9;
            this.customLabel3.Text = "备注:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplier.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplier.Location = new System.Drawing.Point(474, 21);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(47, 12);
            this.lblSupplier.TabIndex = 7;
            this.lblSupplier.Text = "供应商:";
            this.lblSupplier.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(91, 47);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(168, 16);
            this.txtCode.TabIndex = 1;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(35, 51);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(47, 12);
            this.lblSupplyContract.TabIndex = 0;
            this.lblSupplyContract.Text = "单据号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(3, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(986, 404);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(978, 379);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "明细信息";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.colMaterialCode,
            this.colMaterialName,
            this.colMaterialSpec,
            this.colTicketVolume,
            this.colLessPumpVolume,
            this.colDeductionVolume,
            this.colBalVolume,
            this.colPrice,
            this.colMoney,
            this.colIsPump,
            this.colUsedPart,
            this.colSubjectName,
            this.colDescript,
            this.colTempData,
            this.colPlanQuantity});
            this.dgDetail.ContextMenuStrip = this.contextMenuStrip2;
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
            this.dgDetail.Location = new System.Drawing.Point(3, 3);
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
            this.dgDetail.Size = new System.Drawing.Size(972, 373);
            this.dgDetail.TabIndex = 7;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colMaterialCode
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.colMaterialCode.DefaultCellStyle = dataGridViewCellStyle1;
            this.colMaterialCode.HeaderText = "物资编码";
            this.colMaterialCode.Name = "colMaterialCode";
            this.colMaterialCode.Width = 80;
            // 
            // colMaterialName
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colMaterialName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colMaterialName.FillWeight = 80F;
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            this.colMaterialName.ReadOnly = true;
            this.colMaterialName.Width = 80;
            // 
            // colMaterialSpec
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colMaterialSpec.DefaultCellStyle = dataGridViewCellStyle3;
            this.colMaterialSpec.FillWeight = 80F;
            this.colMaterialSpec.HeaderText = "规格型号";
            this.colMaterialSpec.Name = "colMaterialSpec";
            this.colMaterialSpec.ReadOnly = true;
            // 
            // colTicketVolume
            // 
            this.colTicketVolume.HeaderText = "小票方量";
            this.colTicketVolume.Name = "colTicketVolume";
            this.colTicketVolume.ReadOnly = true;
            this.colTicketVolume.Width = 80;
            // 
            // colLessPumpVolume
            // 
            this.colLessPumpVolume.HeaderText = "抽磅扣减";
            this.colLessPumpVolume.Name = "colLessPumpVolume";
            this.colLessPumpVolume.Width = 80;
            // 
            // colDeductionVolume
            // 
            this.colDeductionVolume.HeaderText = "其他扣减";
            this.colDeductionVolume.Name = "colDeductionVolume";
            this.colDeductionVolume.Width = 80;
            // 
            // colBalVolume
            // 
            this.colBalVolume.HeaderText = "应结方量";
            this.colBalVolume.Name = "colBalVolume";
            this.colBalVolume.ReadOnly = true;
            this.colBalVolume.Width = 80;
            // 
            // colPrice
            // 
            this.colPrice.HeaderText = "单价";
            this.colPrice.Name = "colPrice";
            this.colPrice.Width = 80;
            // 
            // colMoney
            // 
            this.colMoney.HeaderText = "金额";
            this.colMoney.Name = "colMoney";
            this.colMoney.ReadOnly = true;
            this.colMoney.Width = 80;
            // 
            // colIsPump
            // 
            this.colIsPump.HeaderText = "是否泵送";
            this.colIsPump.Name = "colIsPump";
            this.colIsPump.ReadOnly = true;
            this.colIsPump.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colIsPump.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colIsPump.Width = 80;
            // 
            // colUsedPart
            // 
            this.colUsedPart.HeaderText = "使用部位";
            this.colUsedPart.Name = "colUsedPart";
            // 
            // colSubjectName
            // 
            this.colSubjectName.HeaderText = "核算科目";
            this.colSubjectName.Name = "colSubjectName";
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 200;
            // 
            // colTempData
            // 
            this.colTempData.HeaderText = "临时值";
            this.colTempData.Name = "colTempData";
            this.colTempData.Visible = false;
            // 
            // colPlanQuantity
            // 
            this.colPlanQuantity.HeaderText = "计划数量";
            this.colPlanQuantity.Name = "colPlanQuantity";
            this.colPlanQuantity.Visible = false;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddCostDetial,
            this.dgDelete});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(153, 70);
            // 
            // AddCostDetial
            // 
            this.AddCostDetial.Name = "AddCostDetial";
            this.AddCostDetial.Size = new System.Drawing.Size(152, 22);
            this.AddCostDetial.Text = "添加费用明细";
            // 
            // dgDelete
            // 
            this.dgDelete.Name = "dgDelete";
            this.dgDelete.Size = new System.Drawing.Size(152, 22);
            this.dgDelete.Text = "删除";
            // 
            // txtProject
            // 
            this.txtProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProject.DrawSelf = false;
            this.txtProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProject.EnterToTab = false;
            this.txtProject.Location = new System.Drawing.Point(532, 11);
            this.txtProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProject.Name = "txtProject";
            this.txtProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtProject.ReadOnly = true;
            this.txtProject.Size = new System.Drawing.Size(175, 16);
            this.txtProject.TabIndex = 51;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(467, 15);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(59, 12);
            this.customLabel5.TabIndex = 55;
            this.customLabel5.Text = "归属项目:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreateDate
            // 
            this.txtCreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreateDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreateDate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreateDate.DrawSelf = false;
            this.txtCreateDate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreateDate.EnterToTab = false;
            this.txtCreateDate.Location = new System.Drawing.Point(799, 11);
            this.txtCreateDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateDate.ReadOnly = true;
            this.txtCreateDate.Size = new System.Drawing.Size(68, 16);
            this.txtCreateDate.TabIndex = 49;
            this.txtCreateDate.Visible = false;
            // 
            // customLabel6
            // 
            this.customLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(738, 14);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 53;
            this.customLabel6.Text = "业务日期:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            this.customLabel6.Visible = false;
            // 
            // txtSumVolume
            // 
            this.txtSumVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSumVolume.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumVolume.DrawSelf = false;
            this.txtSumVolume.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumVolume.EnterToTab = false;
            this.txtSumVolume.Location = new System.Drawing.Point(234, 11);
            this.txtSumVolume.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumVolume.Name = "txtSumVolume";
            this.txtSumVolume.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumVolume.ReadOnly = true;
            this.txtSumVolume.Size = new System.Drawing.Size(75, 16);
            this.txtSumVolume.TabIndex = 50;
            // 
            // lbSumQuantity
            // 
            this.lbSumQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbSumQuantity.AutoSize = true;
            this.lbSumQuantity.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSumQuantity.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lbSumQuantity.Location = new System.Drawing.Point(170, 15);
            this.lbSumQuantity.Name = "lbSumQuantity";
            this.lbSumQuantity.Size = new System.Drawing.Size(59, 12);
            this.lbSumQuantity.TabIndex = 54;
            this.lbSumQuantity.Text = "合计数量:";
            this.lbSumQuantity.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(69, 11);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = true;
            this.txtCreatePerson.Size = new System.Drawing.Size(69, 16);
            this.txtCreatePerson.TabIndex = 48;
            // 
            // customLabel7
            // 
            this.customLabel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(18, 15);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(47, 12);
            this.customLabel7.TabIndex = 52;
            this.customLabel7.Text = "制单人:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSumMoney
            // 
            this.txtSumMoney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSumMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumMoney.DrawSelf = false;
            this.txtSumMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumMoney.EnterToTab = false;
            this.txtSumMoney.Location = new System.Drawing.Point(386, 10);
            this.txtSumMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumMoney.Name = "txtSumMoney";
            this.txtSumMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumMoney.ReadOnly = true;
            this.txtSumMoney.Size = new System.Drawing.Size(75, 16);
            this.txtSumMoney.TabIndex = 56;
            // 
            // lableSumMoney
            // 
            this.lableSumMoney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lableSumMoney.AutoSize = true;
            this.lableSumMoney.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lableSumMoney.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lableSumMoney.Location = new System.Drawing.Point(321, 14);
            this.lableSumMoney.Name = "lableSumMoney";
            this.lableSumMoney.Size = new System.Drawing.Size(59, 12);
            this.lableSumMoney.TabIndex = 57;
            this.lableSumMoney.Text = "合计金额:";
            this.lableSumMoney.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VConCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 537);
            this.Name = "VConCheck";
            this.Text = "商品砼对帐单";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.txtSupply)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private System.Windows.Forms.Button btnForward;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit customEdit1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtHandlePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblHandlePerson;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplier;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.Button btnCreateCheck;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lableSumMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumVolume;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lbSumQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtAdjustmentMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker cmoCreateTime;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem AddCostDetial;
        private System.Windows.Forms.ToolStripMenuItem dgDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTicketVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLessPumpVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeductionVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBalVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMoney;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsPump;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsedPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTempData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlanQuantity;
    }
}