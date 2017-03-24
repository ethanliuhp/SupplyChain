namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    partial class VBorrowedOrderQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VBorrowedOrderQuery));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBorrowData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBorrowedType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBorrowedPurpose = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePersonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrgName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnAllMv = new System.Windows.Forms.RadioButton();
            this.btnNoSubmit = new System.Windows.Forms.RadioButton();
            this.btnSubmit = new System.Windows.Forms.RadioButton();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.cmbBorrowedType = new System.Windows.Forms.ComboBox();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSelectOrgName = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtOrgName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtCodeBegin = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.tabControl1);
            this.pnlFloor.Size = new System.Drawing.Size(999, 508);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.tabControl1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(999, 508);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(991, 483);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "明细查询";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(985, 477);
            this.panel2.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dgDetail);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 72);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(985, 405);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "借款明细列表";
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colBorrowData,
            this.colBorrowedType,
            this.colMoney,
            this.colBorrowedPurpose,
            this.colCreatePersonName,
            this.colRealOperateDate,
            this.colOrgName,
            this.colDescript});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.dgDetail.Size = new System.Drawing.Size(979, 385);
            this.dgDetail.TabIndex = 11;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colCode
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.colCode.DefaultCellStyle = dataGridViewCellStyle1;
            this.colCode.HeaderText = "单据编码";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            // 
            // colBorrowData
            // 
            this.colBorrowData.HeaderText = "借款日期";
            this.colBorrowData.Name = "colBorrowData";
            this.colBorrowData.Width = 80;
            // 
            // colBorrowedType
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colBorrowedType.DefaultCellStyle = dataGridViewCellStyle2;
            this.colBorrowedType.HeaderText = "借款性质";
            this.colBorrowedType.Name = "colBorrowedType";
            this.colBorrowedType.ReadOnly = true;
            // 
            // colMoney
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colMoney.DefaultCellStyle = dataGridViewCellStyle3;
            this.colMoney.FillWeight = 80F;
            this.colMoney.HeaderText = "借款金额";
            this.colMoney.Name = "colMoney";
            this.colMoney.Width = 120;
            // 
            // colBorrowedPurpose
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.colBorrowedPurpose.DefaultCellStyle = dataGridViewCellStyle4;
            this.colBorrowedPurpose.HeaderText = "借款用途";
            this.colBorrowedPurpose.Name = "colBorrowedPurpose";
            this.colBorrowedPurpose.Width = 200;
            // 
            // colCreatePersonName
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.colCreatePersonName.DefaultCellStyle = dataGridViewCellStyle5;
            this.colCreatePersonName.HeaderText = "制单人";
            this.colCreatePersonName.Name = "colCreatePersonName";
            this.colCreatePersonName.ReadOnly = true;
            this.colCreatePersonName.Width = 80;
            // 
            // colRealOperateDate
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.colRealOperateDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.colRealOperateDate.HeaderText = "制单时间";
            this.colRealOperateDate.Name = "colRealOperateDate";
            this.colRealOperateDate.ReadOnly = true;
            this.colRealOperateDate.Width = 80;
            // 
            // colOrgName
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.colOrgName.DefaultCellStyle = dataGridViewCellStyle7;
            this.colOrgName.HeaderText = "所属组织";
            this.colOrgName.Name = "colOrgName";
            // 
            // colDescript
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            this.colDescript.DefaultCellStyle = dataGridViewCellStyle8;
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 200;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnAllMv);
            this.groupBox4.Controls.Add(this.btnNoSubmit);
            this.groupBox4.Controls.Add(this.btnSubmit);
            this.groupBox4.Controls.Add(this.txtCreatePerson);
            this.groupBox4.Controls.Add(this.cmbBorrowedType);
            this.groupBox4.Controls.Add(this.customLabel7);
            this.groupBox4.Controls.Add(this.btnSelectOrgName);
            this.groupBox4.Controls.Add(this.txtOrgName);
            this.groupBox4.Controls.Add(this.customLabel6);
            this.groupBox4.Controls.Add(this.btnExcel);
            this.groupBox4.Controls.Add(this.btnSearch);
            this.groupBox4.Controls.Add(this.txtCodeBegin);
            this.groupBox4.Controls.Add(this.customLabel1);
            this.groupBox4.Controls.Add(this.customLabel2);
            this.groupBox4.Controls.Add(this.customLabel3);
            this.groupBox4.Controls.Add(this.dtpDateBegin);
            this.groupBox4.Controls.Add(this.dtpDateEnd);
            this.groupBox4.Controls.Add(this.customLabel4);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(985, 72);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            // 
            // btnAllMv
            // 
            this.btnAllMv.AutoSize = true;
            this.btnAllMv.Location = new System.Drawing.Point(866, 19);
            this.btnAllMv.Name = "btnAllMv";
            this.btnAllMv.Size = new System.Drawing.Size(47, 16);
            this.btnAllMv.TabIndex = 191;
            this.btnAllMv.TabStop = true;
            this.btnAllMv.Text = "全部";
            this.btnAllMv.UseVisualStyleBackColor = true;
            // 
            // btnNoSubmit
            // 
            this.btnNoSubmit.AutoSize = true;
            this.btnNoSubmit.Location = new System.Drawing.Point(806, 19);
            this.btnNoSubmit.Name = "btnNoSubmit";
            this.btnNoSubmit.Size = new System.Drawing.Size(59, 16);
            this.btnNoSubmit.TabIndex = 190;
            this.btnNoSubmit.TabStop = true;
            this.btnNoSubmit.Text = "未提交";
            this.btnNoSubmit.UseVisualStyleBackColor = true;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AutoSize = true;
            this.btnSubmit.Checked = true;
            this.btnSubmit.Location = new System.Drawing.Point(746, 19);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(59, 16);
            this.btnSubmit.TabIndex = 189;
            this.btnSubmit.TabStop = true;
            this.btnSubmit.Text = "已提交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(328, 17);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = false;
            this.txtCreatePerson.Size = new System.Drawing.Size(101, 16);
            this.txtCreatePerson.TabIndex = 174;
            // 
            // cmbBorrowedType
            // 
            this.cmbBorrowedType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBorrowedType.FormattingEnabled = true;
            this.cmbBorrowedType.Items.AddRange(new object[] {
            "",
            "策划内",
            "策划外"});
            this.cmbBorrowedType.Location = new System.Drawing.Point(328, 45);
            this.cmbBorrowedType.Name = "cmbBorrowedType";
            this.cmbBorrowedType.Size = new System.Drawing.Size(101, 20);
            this.cmbBorrowedType.TabIndex = 173;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(263, 49);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 172;
            this.customLabel7.Text = "借款性质:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSelectOrgName
            // 
            this.btnSelectOrgName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectOrgName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectOrgName.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSelectOrgName.Location = new System.Drawing.Point(213, 43);
            this.btnSelectOrgName.Name = "btnSelectOrgName";
            this.btnSelectOrgName.Size = new System.Drawing.Size(47, 23);
            this.btnSelectOrgName.TabIndex = 105;
            this.btnSelectOrgName.Text = "选择";
            this.btnSelectOrgName.UseVisualStyleBackColor = true;
            // 
            // txtOrgName
            // 
            this.txtOrgName.BackColor = System.Drawing.SystemColors.Control;
            this.txtOrgName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOrgName.DrawSelf = false;
            this.txtOrgName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOrgName.EnterToTab = false;
            this.txtOrgName.Location = new System.Drawing.Point(67, 46);
            this.txtOrgName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOrgName.Name = "txtOrgName";
            this.txtOrgName.Padding = new System.Windows.Forms.Padding(1);
            this.txtOrgName.ReadOnly = true;
            this.txtOrgName.Size = new System.Drawing.Size(146, 16);
            this.txtOrgName.TabIndex = 104;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(9, 48);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 103;
            this.customLabel6.Text = "范围选择:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(734, 46);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 94;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(635, 46);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 93;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtCodeBegin
            // 
            this.txtCodeBegin.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBegin.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBegin.DrawSelf = false;
            this.txtCodeBegin.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBegin.EnterToTab = false;
            this.txtCodeBegin.Location = new System.Drawing.Point(68, 18);
            this.txtCodeBegin.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBegin.Name = "txtCodeBegin";
            this.txtCodeBegin.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBegin.ReadOnly = false;
            this.txtCodeBegin.Size = new System.Drawing.Size(191, 16);
            this.txtCodeBegin.TabIndex = 89;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(278, 21);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(47, 12);
            this.customLabel1.TabIndex = 98;
            this.customLabel1.Text = "制单人:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(21, 20);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(47, 12);
            this.customLabel2.TabIndex = 95;
            this.customLabel2.Text = "单据号:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(440, 20);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 96;
            this.customLabel3.Text = "借款日期:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(499, 16);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 90;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(624, 16);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 91;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(610, 20);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(11, 12);
            this.customLabel4.TabIndex = 97;
            this.customLabel4.Text = "-";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VBorrowedOrderQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(999, 508);
            this.Name = "VBorrowedOrderQuery";
            this.Text = "借款单查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox5;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBorrowData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBorrowedType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBorrowedPurpose;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePersonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrgName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.GroupBox groupBox4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private System.Windows.Forms.ComboBox cmbBorrowedType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSelectOrgName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOrgName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private System.Windows.Forms.RadioButton btnAllMv;
        private System.Windows.Forms.RadioButton btnNoSubmit;
        private System.Windows.Forms.RadioButton btnSubmit;
    }
}