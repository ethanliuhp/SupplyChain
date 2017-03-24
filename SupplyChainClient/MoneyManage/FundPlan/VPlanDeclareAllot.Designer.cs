namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    partial class VPlanDeclareAllot
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VPlanDeclareAllot));
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpBeginCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpEndCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.lblEndCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgMaster = new System.Windows.Forms.DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPresentMonthPayment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApprovalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeclareDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePersonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tPOfficeExpend = new System.Windows.Forms.TabPage();
            this.gdOfficeExpend = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpOtherExpend = new System.Windows.Forms.TabPage();
            this.gdOtherExpend = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpProjectExpend = new System.Windows.Forms.TabPage();
            this.gdProjectExpend = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpTotalExpend1 = new System.Windows.Forms.TabPage();
            this.gdTotalExpend1 = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpTotalExpend2 = new System.Windows.Forms.TabPage();
            this.gdTotalExpend2 = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpProjectReport = new System.Windows.Forms.TabPage();
            this.gdProjectReport = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpCompanyReport = new System.Windows.Forms.TabPage();
            this.gdCompanyReport = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnSave = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tPOfficeExpend.SuspendLayout();
            this.tpOtherExpend.SuspendLayout();
            this.tpProjectExpend.SuspendLayout();
            this.tpTotalExpend1.SuspendLayout();
            this.tpTotalExpend2.SuspendLayout();
            this.tpProjectReport.SuspendLayout();
            this.tpCompanyReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnSave);
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Controls.Add(this.dtpBeginCreateDate);
            this.pnlFloor.Controls.Add(this.dtpEndCreateDate);
            this.pnlFloor.Controls.Add(this.lblEndCreateDate);
            this.pnlFloor.Controls.Add(this.label2);
            this.pnlFloor.Controls.Add(this.txtCode);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Size = new System.Drawing.Size(1037, 458);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtCode, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblEndCreateDate, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtpEndCreateDate, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtpBeginCreateDate, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSave, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 23);
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(94, 10);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = false;
            this.txtCode.Size = new System.Drawing.Size(175, 16);
            this.txtCode.TabIndex = 300;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 301;
            this.label1.Text = "资金计划单号：";
            // 
            // dtpBeginCreateDate
            // 
            this.dtpBeginCreateDate.Location = new System.Drawing.Point(346, 8);
            this.dtpBeginCreateDate.Name = "dtpBeginCreateDate";
            this.dtpBeginCreateDate.Size = new System.Drawing.Size(110, 21);
            this.dtpBeginCreateDate.TabIndex = 321;
            // 
            // dtpEndCreateDate
            // 
            this.dtpEndCreateDate.Location = new System.Drawing.Point(475, 8);
            this.dtpEndCreateDate.Name = "dtpEndCreateDate";
            this.dtpEndCreateDate.Size = new System.Drawing.Size(110, 21);
            this.dtpEndCreateDate.TabIndex = 319;
            // 
            // lblEndCreateDate
            // 
            this.lblEndCreateDate.AutoSize = true;
            this.lblEndCreateDate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblEndCreateDate.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblEndCreateDate.Location = new System.Drawing.Point(460, 12);
            this.lblEndCreateDate.Name = "lblEndCreateDate";
            this.lblEndCreateDate.Size = new System.Drawing.Size(11, 12);
            this.lblEndCreateDate.TabIndex = 320;
            this.lblEndCreateDate.Text = "-";
            this.lblEndCreateDate.UnderLineColor = System.Drawing.Color.Red;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(275, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 318;
            this.label2.Text = "业务时间：";
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(759, 6);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 28);
            this.btnQuery.TabIndex = 322;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgMaster);
            this.groupBox1.Location = new System.Drawing.Point(7, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1023, 168);
            this.groupBox1.TabIndex = 326;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "资金计划申报：";
            // 
            // dgMaster
            // 
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colPresentMonthPayment,
            this.colApprovalAmount,
            this.colDeclareDate,
            this.colState,
            this.colCreateDate,
            this.colCreatePersonName});
            this.dgMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMaster.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgMaster.Location = new System.Drawing.Point(3, 17);
            this.dgMaster.Name = "dgMaster";
            this.dgMaster.ReadOnly = true;
            this.dgMaster.RowTemplate.Height = 23;
            this.dgMaster.Size = new System.Drawing.Size(1017, 148);
            this.dgMaster.TabIndex = 0;
            this.dgMaster.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgMaster_CellDoubleClick);
            // 
            // colCode
            // 
            this.colCode.HeaderText = "单号";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 260;
            // 
            // colPresentMonthPayment
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            this.colPresentMonthPayment.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPresentMonthPayment.HeaderText = "本期计划付款";
            this.colPresentMonthPayment.Name = "colPresentMonthPayment";
            this.colPresentMonthPayment.ReadOnly = true;
            // 
            // colApprovalAmount
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            this.colApprovalAmount.DefaultCellStyle = dataGridViewCellStyle2;
            this.colApprovalAmount.HeaderText = "批准额度";
            this.colApprovalAmount.Name = "colApprovalAmount";
            this.colApprovalAmount.ReadOnly = true;
            this.colApprovalAmount.Width = 150;
            // 
            // colDeclareDate
            // 
            this.colDeclareDate.HeaderText = "申报日期";
            this.colDeclareDate.Name = "colDeclareDate";
            this.colDeclareDate.ReadOnly = true;
            // 
            // colState
            // 
            this.colState.HeaderText = "状态";
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;
            // 
            // colCreateDate
            // 
            this.colCreateDate.HeaderText = "业务日期";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.ReadOnly = true;
            // 
            // colCreatePersonName
            // 
            this.colCreatePersonName.HeaderText = "创建人名称";
            this.colCreatePersonName.Name = "colCreatePersonName";
            this.colCreatePersonName.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Location = new System.Drawing.Point(7, 214);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1023, 241);
            this.groupBox2.TabIndex = 327;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "资金计划申报明细：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tPOfficeExpend);
            this.tabControl1.Controls.Add(this.tpOtherExpend);
            this.tabControl1.Controls.Add(this.tpProjectExpend);
            this.tabControl1.Controls.Add(this.tpTotalExpend1);
            this.tabControl1.Controls.Add(this.tpTotalExpend2);
            this.tabControl1.Controls.Add(this.tpProjectReport);
            this.tabControl1.Controls.Add(this.tpCompanyReport);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1017, 221);
            this.tabControl1.TabIndex = 36;
            // 
            // tPOfficeExpend
            // 
            this.tPOfficeExpend.Controls.Add(this.gdOfficeExpend);
            this.tPOfficeExpend.Location = new System.Drawing.Point(4, 22);
            this.tPOfficeExpend.Name = "tPOfficeExpend";
            this.tPOfficeExpend.Size = new System.Drawing.Size(1009, 195);
            this.tPOfficeExpend.TabIndex = 3;
            this.tPOfficeExpend.Tag = "机关资金计划支出明细表";
            this.tPOfficeExpend.Text = "机关资金计划支出明细表";
            this.tPOfficeExpend.UseVisualStyleBackColor = true;
            // 
            // gdOfficeExpend
            // 
            this.gdOfficeExpend.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdOfficeExpend.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdOfficeExpend.CheckedImage")));
            this.gdOfficeExpend.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdOfficeExpend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdOfficeExpend.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdOfficeExpend.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdOfficeExpend.Location = new System.Drawing.Point(0, 0);
            this.gdOfficeExpend.Name = "gdOfficeExpend";
            this.gdOfficeExpend.Size = new System.Drawing.Size(1009, 195);
            this.gdOfficeExpend.TabIndex = 234;
            this.gdOfficeExpend.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdOfficeExpend.UncheckedImage")));
            // 
            // tpOtherExpend
            // 
            this.tpOtherExpend.Controls.Add(this.gdOtherExpend);
            this.tpOtherExpend.Location = new System.Drawing.Point(4, 22);
            this.tpOtherExpend.Name = "tpOtherExpend";
            this.tpOtherExpend.Padding = new System.Windows.Forms.Padding(3);
            this.tpOtherExpend.Size = new System.Drawing.Size(361, 20);
            this.tpOtherExpend.TabIndex = 0;
            this.tpOtherExpend.Tag = "项目资金支付计划申报其他支出明细表";
            this.tpOtherExpend.Text = "项目资金计划其他支出明细表";
            this.tpOtherExpend.UseVisualStyleBackColor = true;
            // 
            // gdOtherExpend
            // 
            this.gdOtherExpend.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdOtherExpend.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdOtherExpend.CheckedImage")));
            this.gdOtherExpend.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdOtherExpend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdOtherExpend.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdOtherExpend.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdOtherExpend.Location = new System.Drawing.Point(3, 3);
            this.gdOtherExpend.Name = "gdOtherExpend";
            this.gdOtherExpend.Size = new System.Drawing.Size(355, 14);
            this.gdOtherExpend.TabIndex = 235;
            this.gdOtherExpend.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdOtherExpend.UncheckedImage")));
            // 
            // tpProjectExpend
            // 
            this.tpProjectExpend.Controls.Add(this.gdProjectExpend);
            this.tpProjectExpend.Location = new System.Drawing.Point(4, 22);
            this.tpProjectExpend.Name = "tpProjectExpend";
            this.tpProjectExpend.Padding = new System.Windows.Forms.Padding(3);
            this.tpProjectExpend.Size = new System.Drawing.Size(361, 20);
            this.tpProjectExpend.TabIndex = 1;
            this.tpProjectExpend.Tag = "项目资金支付计划申报明细表";
            this.tpProjectExpend.Text = "资金计划申报明细表";
            this.tpProjectExpend.UseVisualStyleBackColor = true;
            // 
            // gdProjectExpend
            // 
            this.gdProjectExpend.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdProjectExpend.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectExpend.CheckedImage")));
            this.gdProjectExpend.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdProjectExpend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdProjectExpend.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdProjectExpend.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdProjectExpend.Location = new System.Drawing.Point(3, 3);
            this.gdProjectExpend.Name = "gdProjectExpend";
            this.gdProjectExpend.Size = new System.Drawing.Size(355, 14);
            this.gdProjectExpend.TabIndex = 235;
            this.gdProjectExpend.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectExpend.UncheckedImage")));
            // 
            // tpTotalExpend1
            // 
            this.tpTotalExpend1.Controls.Add(this.gdTotalExpend1);
            this.tpTotalExpend1.Location = new System.Drawing.Point(4, 22);
            this.tpTotalExpend1.Name = "tpTotalExpend1";
            this.tpTotalExpend1.Size = new System.Drawing.Size(361, 20);
            this.tpTotalExpend1.TabIndex = 2;
            this.tpTotalExpend1.Tag = "单位资金支付计划申报汇总表";
            this.tpTotalExpend1.Text = "资金计划申报汇总表1";
            this.tpTotalExpend1.UseVisualStyleBackColor = true;
            // 
            // gdTotalExpend1
            // 
            this.gdTotalExpend1.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdTotalExpend1.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdTotalExpend1.CheckedImage")));
            this.gdTotalExpend1.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdTotalExpend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdTotalExpend1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdTotalExpend1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdTotalExpend1.Location = new System.Drawing.Point(0, 0);
            this.gdTotalExpend1.Name = "gdTotalExpend1";
            this.gdTotalExpend1.Size = new System.Drawing.Size(361, 20);
            this.gdTotalExpend1.TabIndex = 235;
            this.gdTotalExpend1.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdTotalExpend1.UncheckedImage")));
            // 
            // tpTotalExpend2
            // 
            this.tpTotalExpend2.Controls.Add(this.gdTotalExpend2);
            this.tpTotalExpend2.Location = new System.Drawing.Point(4, 22);
            this.tpTotalExpend2.Name = "tpTotalExpend2";
            this.tpTotalExpend2.Size = new System.Drawing.Size(361, 20);
            this.tpTotalExpend2.TabIndex = 4;
            this.tpTotalExpend2.Tag = "月资金计划项目申报汇总表";
            this.tpTotalExpend2.Text = "资金计划申报汇总表2";
            this.tpTotalExpend2.UseVisualStyleBackColor = true;
            // 
            // gdTotalExpend2
            // 
            this.gdTotalExpend2.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdTotalExpend2.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdTotalExpend2.CheckedImage")));
            this.gdTotalExpend2.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdTotalExpend2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdTotalExpend2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdTotalExpend2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdTotalExpend2.Location = new System.Drawing.Point(0, 0);
            this.gdTotalExpend2.Name = "gdTotalExpend2";
            this.gdTotalExpend2.Size = new System.Drawing.Size(361, 20);
            this.gdTotalExpend2.TabIndex = 235;
            this.gdTotalExpend2.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdTotalExpend2.UncheckedImage")));
            // 
            // tpProjectReport
            // 
            this.tpProjectReport.Controls.Add(this.gdProjectReport);
            this.tpProjectReport.Location = new System.Drawing.Point(4, 22);
            this.tpProjectReport.Name = "tpProjectReport";
            this.tpProjectReport.Size = new System.Drawing.Size(361, 20);
            this.tpProjectReport.TabIndex = 5;
            this.tpProjectReport.Tag = "项目资金支付计划申报分配表";
            this.tpProjectReport.Text = "项目资金支付计划申报表";
            this.tpProjectReport.UseVisualStyleBackColor = true;
            // 
            // gdProjectReport
            // 
            this.gdProjectReport.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdProjectReport.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectReport.CheckedImage")));
            this.gdProjectReport.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdProjectReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdProjectReport.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdProjectReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdProjectReport.Location = new System.Drawing.Point(0, 0);
            this.gdProjectReport.Name = "gdProjectReport";
            this.gdProjectReport.Size = new System.Drawing.Size(361, 20);
            this.gdProjectReport.TabIndex = 235;
            this.gdProjectReport.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectReport.UncheckedImage")));
            // 
            // tpCompanyReport
            // 
            this.tpCompanyReport.Controls.Add(this.gdCompanyReport);
            this.tpCompanyReport.Location = new System.Drawing.Point(4, 22);
            this.tpCompanyReport.Name = "tpCompanyReport";
            this.tpCompanyReport.Size = new System.Drawing.Size(361, 20);
            this.tpCompanyReport.TabIndex = 6;
            this.tpCompanyReport.Tag = "分公司资金支付计划申报表";
            this.tpCompanyReport.Text = "分公司资金支付计划申报表";
            this.tpCompanyReport.UseVisualStyleBackColor = true;
            // 
            // gdCompanyReport
            // 
            this.gdCompanyReport.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdCompanyReport.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdCompanyReport.CheckedImage")));
            this.gdCompanyReport.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdCompanyReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdCompanyReport.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdCompanyReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdCompanyReport.Location = new System.Drawing.Point(0, 0);
            this.gdCompanyReport.Name = "gdCompanyReport";
            this.gdCompanyReport.Size = new System.Drawing.Size(361, 20);
            this.gdCompanyReport.TabIndex = 235;
            this.gdCompanyReport.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdCompanyReport.UncheckedImage")));
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSave.Location = new System.Drawing.Point(905, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 328;
            this.btnSave.Text = "保 存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // VPlanDeclareAllot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 458);
            this.Name = "VPlanDeclareAllot";
            this.Text = "资金计划分配";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tPOfficeExpend.ResumeLayout(false);
            this.tpOtherExpend.ResumeLayout(false);
            this.tpProjectExpend.ResumeLayout(false);
            this.tpTotalExpend1.ResumeLayout(false);
            this.tpTotalExpend2.ResumeLayout(false);
            this.tpProjectReport.ResumeLayout(false);
            this.tpCompanyReport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private System.Windows.Forms.Label label1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpBeginCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpEndCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblEndCreateDate;
        private System.Windows.Forms.Label label2;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgMaster;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tPOfficeExpend;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdOfficeExpend;
        private System.Windows.Forms.TabPage tpOtherExpend;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdOtherExpend;
        private System.Windows.Forms.TabPage tpProjectExpend;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdProjectExpend;
        private System.Windows.Forms.TabPage tpTotalExpend1;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdTotalExpend1;
        private System.Windows.Forms.TabPage tpTotalExpend2;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdTotalExpend2;
        private System.Windows.Forms.TabPage tpProjectReport;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdProjectReport;
        private System.Windows.Forms.TabPage tpCompanyReport;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdCompanyReport;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPresentMonthPayment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApprovalAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeclareDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePersonName;
    }
}