namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    partial class VIndirectCost
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VIndirectCost));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.btnCopyDetail = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtFinanceMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel15 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtDescript = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.cmsDg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDel = new System.Windows.Forms.ToolStripMenuItem();
            this.txtCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSumMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtBudgetSumMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtIndircSumMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.colAccountTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBudgetMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlContent.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.cmsDg.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Size = new System.Drawing.Size(991, 440);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl1);
            this.pnlBody.Location = new System.Drawing.Point(0, 89);
            this.pnlBody.Size = new System.Drawing.Size(989, 301);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtIndircSumMoney);
            this.pnlFooter.Controls.Add(this.customLabel8);
            this.pnlFooter.Controls.Add(this.txtBudgetSumMoney);
            this.pnlFooter.Controls.Add(this.customLabel2);
            this.pnlFooter.Controls.Add(this.txtSumMoney);
            this.pnlFooter.Controls.Add(this.customLabel7);
            this.pnlFooter.Controls.Add(this.txtCreateDate);
            this.pnlFooter.Controls.Add(this.customLabel6);
            this.pnlFooter.Controls.Add(this.txtCreatePerson);
            this.pnlFooter.Controls.Add(this.customLabel5);
            this.pnlFooter.Location = new System.Drawing.Point(0, 390);
            this.pnlFooter.Size = new System.Drawing.Size(989, 48);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(991, 0);
            this.spTop.Visible = false;
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Size = new System.Drawing.Size(991, 440);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.groupSupply);
            this.pnlHeader.Size = new System.Drawing.Size(989, 89);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.groupSupply, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Visible = false;
            // 
            // groupSupply
            // 
            this.groupSupply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSupply.Controls.Add(this.btnCopyDetail);
            this.groupSupply.Controls.Add(this.txtFinanceMoney);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.customLabel1);
            this.groupSupply.Controls.Add(this.dtpDateEnd);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.customLabel15);
            this.groupSupply.Controls.Add(this.txtDescript);
            this.groupSupply.Controls.Add(this.customLabel3);
            this.groupSupply.Location = new System.Drawing.Point(9, 7);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(974, 69);
            this.groupSupply.TabIndex = 3;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>单据信息";
            // 
            // btnCopyDetail
            // 
            this.btnCopyDetail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCopyDetail.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCopyDetail.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCopyDetail.Location = new System.Drawing.Point(824, 14);
            this.btnCopyDetail.Name = "btnCopyDetail";
            this.btnCopyDetail.Size = new System.Drawing.Size(75, 23);
            this.btnCopyDetail.TabIndex = 153;
            this.btnCopyDetail.Text = "复制";
            this.btnCopyDetail.UseVisualStyleBackColor = true;
            // 
            // txtFinanceMoney
            // 
            this.txtFinanceMoney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtFinanceMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtFinanceMoney.DrawSelf = false;
            this.txtFinanceMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtFinanceMoney.EnterToTab = false;
            this.txtFinanceMoney.Location = new System.Drawing.Point(383, 21);
            this.txtFinanceMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtFinanceMoney.Name = "txtFinanceMoney";
            this.txtFinanceMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtFinanceMoney.ReadOnly = false;
            this.txtFinanceMoney.Size = new System.Drawing.Size(119, 16);
            this.txtFinanceMoney.TabIndex = 151;
            // 
            // customLabel4
            // 
            this.customLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(322, 24);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 152;
            this.customLabel4.Text = "财务费用:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(509, 23);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 150;
            this.customLabel1.Text = "截止日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(572, 18);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 149;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(76, 20);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(233, 16);
            this.txtCode.TabIndex = 148;
            // 
            // customLabel15
            // 
            this.customLabel15.AutoSize = true;
            this.customLabel15.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel15.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel15.Location = new System.Drawing.Point(12, 18);
            this.customLabel15.Name = "customLabel15";
            this.customLabel15.Size = new System.Drawing.Size(59, 12);
            this.customLabel15.TabIndex = 147;
            this.customLabel15.Text = "单据编号:";
            this.customLabel15.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtDescript
            // 
            this.txtDescript.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescript.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDescript.DrawSelf = false;
            this.txtDescript.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDescript.EnterToTab = false;
            this.txtDescript.Location = new System.Drawing.Point(76, 42);
            this.txtDescript.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDescript.Name = "txtDescript";
            this.txtDescript.Padding = new System.Windows.Forms.Padding(1);
            this.txtDescript.ReadOnly = false;
            this.txtDescript.Size = new System.Drawing.Size(823, 16);
            this.txtDescript.TabIndex = 145;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(36, 43);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(35, 12);
            this.customLabel3.TabIndex = 146;
            this.customLabel3.Text = "备注:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(977, 301);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(969, 275);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "明细信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = true;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAccountTitle,
            this.colBudgetMoney,
            this.colActualMoney,
            this.colRate,
            this.colDescript});
            this.dgDetail.ContextMenuStrip = this.cmsDg;
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
            this.dgDetail.Size = new System.Drawing.Size(963, 269);
            this.dgDetail.TabIndex = 9;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // cmsDg
            // 
            this.cmsDg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDel});
            this.cmsDg.Name = "cmsDg";
            this.cmsDg.Size = new System.Drawing.Size(101, 26);
            // 
            // tsmiDel
            // 
            this.tsmiDel.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.tsmiDel.Name = "tsmiDel";
            this.tsmiDel.Size = new System.Drawing.Size(100, 22);
            this.tsmiDel.Text = "删除";
            this.tsmiDel.ToolTipText = "删除当前选中的记录";
            // 
            // txtCreateDate
            // 
            this.txtCreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreateDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreateDate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreateDate.DrawSelf = false;
            this.txtCreateDate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreateDate.EnterToTab = false;
            this.txtCreateDate.Location = new System.Drawing.Point(193, 21);
            this.txtCreateDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateDate.ReadOnly = true;
            this.txtCreateDate.Size = new System.Drawing.Size(68, 16);
            this.txtCreateDate.TabIndex = 33;
            // 
            // customLabel6
            // 
            this.customLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(128, 25);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 37;
            this.customLabel6.Text = "制单日期:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(58, 21);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = true;
            this.txtCreatePerson.Size = new System.Drawing.Size(69, 16);
            this.txtCreatePerson.TabIndex = 32;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(7, 25);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(47, 12);
            this.customLabel5.TabIndex = 36;
            this.customLabel5.Text = "制单人:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSumMoney
            // 
            this.txtSumMoney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSumMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumMoney.DrawSelf = false;
            this.txtSumMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumMoney.EnterToTab = false;
            this.txtSumMoney.Location = new System.Drawing.Point(775, 20);
            this.txtSumMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumMoney.Name = "txtSumMoney";
            this.txtSumMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumMoney.ReadOnly = true;
            this.txtSumMoney.Size = new System.Drawing.Size(77, 16);
            this.txtSumMoney.TabIndex = 40;
            // 
            // customLabel7
            // 
            this.customLabel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(711, 24);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 41;
            this.customLabel7.Text = "合计金额:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtBudgetSumMoney
            // 
            this.txtBudgetSumMoney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBudgetSumMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtBudgetSumMoney.DrawSelf = false;
            this.txtBudgetSumMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtBudgetSumMoney.EnterToTab = false;
            this.txtBudgetSumMoney.Location = new System.Drawing.Point(404, 21);
            this.txtBudgetSumMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtBudgetSumMoney.Name = "txtBudgetSumMoney";
            this.txtBudgetSumMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtBudgetSumMoney.ReadOnly = true;
            this.txtBudgetSumMoney.Size = new System.Drawing.Size(77, 16);
            this.txtBudgetSumMoney.TabIndex = 42;
            // 
            // customLabel2
            // 
            this.customLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(270, 25);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(131, 12);
            this.customLabel2.TabIndex = 43;
            this.customLabel2.Text = "合计预算金额(间接费):";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtIndircSumMoney
            // 
            this.txtIndircSumMoney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtIndircSumMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtIndircSumMoney.DrawSelf = false;
            this.txtIndircSumMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtIndircSumMoney.EnterToTab = false;
            this.txtIndircSumMoney.Location = new System.Drawing.Point(620, 21);
            this.txtIndircSumMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtIndircSumMoney.Name = "txtIndircSumMoney";
            this.txtIndircSumMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtIndircSumMoney.ReadOnly = true;
            this.txtIndircSumMoney.Size = new System.Drawing.Size(77, 16);
            this.txtIndircSumMoney.TabIndex = 44;
            // 
            // customLabel8
            // 
            this.customLabel8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(485, 25);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(131, 12);
            this.customLabel8.TabIndex = 45;
            this.customLabel8.Text = "合计实际金额(间接费):";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // colAccountTitle
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.colAccountTitle.DefaultCellStyle = dataGridViewCellStyle1;
            this.colAccountTitle.HeaderText = "费用类型";
            this.colAccountTitle.Name = "colAccountTitle";
            this.colAccountTitle.ReadOnly = true;
            // 
            // colBudgetMoney
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colBudgetMoney.DefaultCellStyle = dataGridViewCellStyle2;
            this.colBudgetMoney.FillWeight = 80F;
            this.colBudgetMoney.HeaderText = "预算费用(元)";
            this.colBudgetMoney.Name = "colBudgetMoney";
            this.colBudgetMoney.Width = 150;
            // 
            // colActualMoney
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colActualMoney.DefaultCellStyle = dataGridViewCellStyle3;
            this.colActualMoney.HeaderText = "实际支出(元)";
            this.colActualMoney.Name = "colActualMoney";
            this.colActualMoney.Width = 150;
            // 
            // colRate
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.colRate.DefaultCellStyle = dataGridViewCellStyle4;
            this.colRate.HeaderText = "比例(%)";
            this.colRate.Name = "colRate";
            this.colRate.ReadOnly = true;
            this.colRate.Visible = false;
            // 
            // colDescript
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.colDescript.DefaultCellStyle = dataGridViewCellStyle5;
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 200;
            // 
            // VIndirectCost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 440);
            this.Name = "VIndirectCost";
            this.Text = "费用信息维护";
            this.pnlContent.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlWorkSpace.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.cmsDg.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private System.Windows.Forms.ContextMenuStrip cmsDg;
        private System.Windows.Forms.ToolStripMenuItem tsmiDel;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel15;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDescript;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtBudgetSumMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtFinanceMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCopyDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtIndircSumMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBudgetMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
    }
}