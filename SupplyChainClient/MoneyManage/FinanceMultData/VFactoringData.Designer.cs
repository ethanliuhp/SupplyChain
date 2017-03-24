namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VFactoringData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VFactoringData));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblRemark = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel15 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtTotal = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreateName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmsDg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDel = new System.Windows.Forms.ToolStripMenuItem();
            this.txtCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.DepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.EndDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.PayType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.StartChargingDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.EndChargingDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.TotalDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmountPayable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.pnlContent.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            this.cmsDg.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
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
            this.pnlBody.Location = new System.Drawing.Point(0, 109);
            this.pnlBody.Size = new System.Drawing.Size(989, 291);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtTotal);
            this.pnlFooter.Controls.Add(this.txtCreateDate);
            this.pnlFooter.Controls.Add(this.lblTotal);
            this.pnlFooter.Controls.Add(this.customLabel4);
            this.pnlFooter.Controls.Add(this.customLabel1);
            this.pnlFooter.Controls.Add(this.txtCreateName);
            this.pnlFooter.Location = new System.Drawing.Point(0, 400);
            this.pnlFooter.Size = new System.Drawing.Size(989, 38);
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
            this.pnlHeader.Size = new System.Drawing.Size(989, 109);
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
            this.groupSupply.Controls.Add(this.dtpDateBegin);
            this.groupSupply.Controls.Add(this.customLabel2);
            this.groupSupply.Controls.Add(this.txtRemark);
            this.groupSupply.Controls.Add(this.lblRemark);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.customLabel15);
            this.groupSupply.Location = new System.Drawing.Point(9, 7);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(974, 78);
            this.groupSupply.TabIndex = 3;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>单据信息";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(75, 51);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(862, 16);
            this.txtRemark.TabIndex = 152;
            // 
            // lblRemark
            // 
            this.lblRemark.AutoSize = true;
            this.lblRemark.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRemark.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRemark.Location = new System.Drawing.Point(10, 52);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(59, 12);
            this.lblRemark.TabIndex = 151;
            this.lblRemark.Text = "备    注:";
            this.lblRemark.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(77, 24);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(141, 16);
            this.txtCode.TabIndex = 148;
            // 
            // customLabel15
            // 
            this.customLabel15.AutoSize = true;
            this.customLabel15.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel15.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel15.Location = new System.Drawing.Point(12, 24);
            this.customLabel15.Name = "customLabel15";
            this.customLabel15.Size = new System.Drawing.Size(59, 12);
            this.customLabel15.TabIndex = 147;
            this.customLabel15.Text = "单据编号:";
            this.customLabel15.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.SystemColors.Control;
            this.txtTotal.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtTotal.DrawSelf = false;
            this.txtTotal.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtTotal.EnterToTab = false;
            this.txtTotal.Location = new System.Drawing.Point(493, 18);
            this.txtTotal.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Padding = new System.Windows.Forms.Padding(1);
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(141, 16);
            this.txtTotal.TabIndex = 154;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblTotal.Location = new System.Drawing.Point(440, 18);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(47, 12);
            this.lblTotal.TabIndex = 153;
            this.lblTotal.Text = "总金额:";
            this.lblTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreateName
            // 
            this.txtCreateName.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreateName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreateName.DrawSelf = false;
            this.txtCreateName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreateName.EnterToTab = false;
            this.txtCreateName.Location = new System.Drawing.Point(74, 18);
            this.txtCreateName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateName.Name = "txtCreateName";
            this.txtCreateName.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateName.ReadOnly = true;
            this.txtCreateName.Size = new System.Drawing.Size(98, 16);
            this.txtCreateName.TabIndex = 152;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(21, 18);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(47, 12);
            this.customLabel4.TabIndex = 151;
            this.customLabel4.Text = "制单人:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(219, 18);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 150;
            this.customLabel1.Text = "制单日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
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
            this.txtCreateDate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreateDate.DrawSelf = false;
            this.txtCreateDate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreateDate.EnterToTab = false;
            this.txtCreateDate.Location = new System.Drawing.Point(284, 19);
            this.txtCreateDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateDate.ReadOnly = true;
            this.txtCreateDate.Size = new System.Drawing.Size(98, 16);
            this.txtCreateDate.TabIndex = 153;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(977, 291);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.dgDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(969, 265);
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
            this.DepartmentName,
            this.ProjectName,
            this.BankName,
            this.Balance,
            this.Rate,
            this.StartDate,
            this.EndDate,
            this.PayType,
            this.StartChargingDate,
            this.EndChargingDate,
            this.TotalDay,
            this.AmountPayable});
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
            this.dgDetail.Size = new System.Drawing.Size(963, 259);
            this.dgDetail.TabIndex = 9;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // DepartmentName
            // 
            dataGridViewCellStyle15.Format = "N2";
            dataGridViewCellStyle15.NullValue = null;
            this.DepartmentName.DefaultCellStyle = dataGridViewCellStyle15;
            this.DepartmentName.HeaderText = "单位名称";
            this.DepartmentName.Name = "DepartmentName";
            this.DepartmentName.Width = 120;
            // 
            // ProjectName
            // 
            this.ProjectName.HeaderText = "项目";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ProjectName.Width = 120;
            // 
            // BankName
            // 
            this.BankName.HeaderText = "银行";
            this.BankName.Name = "BankName";
            this.BankName.Width = 120;
            // 
            // Balance
            // 
            dataGridViewCellStyle16.Format = "N0";
            dataGridViewCellStyle16.NullValue = null;
            this.Balance.DefaultCellStyle = dataGridViewCellStyle16;
            this.Balance.HeaderText = "目前余额(元)";
            this.Balance.Name = "Balance";
            this.Balance.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Rate
            // 
            dataGridViewCellStyle17.Format = "P1";
            dataGridViewCellStyle17.NullValue = null;
            this.Rate.DefaultCellStyle = dataGridViewCellStyle17;
            this.Rate.HeaderText = "费率";
            this.Rate.Name = "Rate";
            this.Rate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Rate.Width = 60;
            // 
            // StartDate
            // 
            dataGridViewCellStyle18.Format = "N2";
            this.StartDate.DefaultCellStyle = dataGridViewCellStyle18;
            this.StartDate.HeaderText = "起始日期";
            this.StartDate.Name = "StartDate";
            this.StartDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.StartDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.StartDate.Width = 80;
            // 
            // EndDate
            // 
            dataGridViewCellStyle19.Format = "N2";
            this.EndDate.DefaultCellStyle = dataGridViewCellStyle19;
            this.EndDate.HeaderText = "终止日期";
            this.EndDate.Name = "EndDate";
            this.EndDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.EndDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.EndDate.Width = 80;
            // 
            // PayType
            // 
            dataGridViewCellStyle20.Format = "N2";
            this.PayType.DefaultCellStyle = dataGridViewCellStyle20;
            this.PayType.HeaderText = "付费方式";
            this.PayType.Items.AddRange(new object[] {
            "一次性前付费",
            "一次性后付费"});
            this.PayType.Name = "PayType";
            this.PayType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.PayType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.PayType.Width = 80;
            // 
            // StartChargingDate
            // 
            this.StartChargingDate.HeaderText = "计费起始日期";
            this.StartChargingDate.Name = "StartChargingDate";
            this.StartChargingDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.StartChargingDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.StartChargingDate.Width = 120;
            // 
            // EndChargingDate
            // 
            this.EndChargingDate.HeaderText = "计费终止日期";
            this.EndChargingDate.Name = "EndChargingDate";
            this.EndChargingDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.EndChargingDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.EndChargingDate.Width = 120;
            // 
            // TotalDay
            // 
            this.TotalDay.HeaderText = "天数";
            this.TotalDay.Name = "TotalDay";
            this.TotalDay.Width = 60;
            // 
            // AmountPayable
            // 
            dataGridViewCellStyle21.Format = "N0";
            dataGridViewCellStyle21.NullValue = null;
            this.AmountPayable.DefaultCellStyle = dataGridViewCellStyle21;
            this.AmountPayable.HeaderText = "利息及手续费";
            this.AmountPayable.Name = "AmountPayable";
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(256, 28);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 153;
            this.customLabel2.Text = "业务时间:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(315, 22);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(115, 21);
            this.dtpDateBegin.TabIndex = 154;
            // 
            // VFactoringData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 440);
            this.Name = "VFactoringData";
            this.Text = "保理单";
            this.pnlContent.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlWorkSpace.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            this.cmsDg.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private System.Windows.Forms.ContextMenuStrip cmsDg;
        private System.Windows.Forms.ToolStripMenuItem tsmiDel;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel15;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreateName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreateDate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BankName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rate;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn StartDate;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn EndDate;
        private System.Windows.Forms.DataGridViewComboBoxColumn PayType;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn StartChargingDate;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn EndChargingDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmountPayable;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
    }
}