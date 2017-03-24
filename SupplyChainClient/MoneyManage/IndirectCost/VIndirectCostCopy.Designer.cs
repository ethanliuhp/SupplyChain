namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    partial class VIndirectCostCopy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VIndirectCostCopy));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCodeBill = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colCreateDateBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSummoneyBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStateBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePersonBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperationDateBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescriptBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkRerveser = new System.Windows.Forms.CheckBox();
            this.chkCheckAll = new System.Windows.Forms.CheckBox();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSure = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearchBill = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtCodeBeginBill = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel13 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePersonBill = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.customLabel15 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel19 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBeginBill = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEndBill = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel20 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colAccountTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBudgetMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePersonBill)).BeginInit();
            this.SuspendLayout();
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
            this.colSelect,
            this.colAccountTitle,
            this.colBudgetMoney,
            this.colActualMoney,
            this.colRate,
            this.colDescript});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
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
            this.dgDetail.RowHeadersVisible = false;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetail.Size = new System.Drawing.Size(904, 184);
            this.dgDetail.TabIndex = 11;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
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
            this.colCodeBill,
            this.colCreateDateBill,
            this.colSummoneyBill,
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
            this.dgMaster.ReadOnly = true;
            this.dgMaster.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgMaster.ReadOnlyCols")));
            this.dgMaster.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.RowHeadersVisible = false;
            this.dgMaster.RowHeadersWidth = 22;
            this.dgMaster.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMaster.RowTemplate.Height = 23;
            this.dgMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMaster.Size = new System.Drawing.Size(904, 157);
            this.dgMaster.TabIndex = 12;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colCodeBill
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            this.colCodeBill.DefaultCellStyle = dataGridViewCellStyle8;
            this.colCodeBill.HeaderText = "单据号";
            this.colCodeBill.Name = "colCodeBill";
            this.colCodeBill.ReadOnly = true;
            this.colCodeBill.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCodeBill.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCodeBill.Width = 120;
            // 
            // colCreateDateBill
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            this.colCreateDateBill.DefaultCellStyle = dataGridViewCellStyle9;
            this.colCreateDateBill.HeaderText = "截至日期";
            this.colCreateDateBill.Name = "colCreateDateBill";
            // 
            // colSummoneyBill
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            this.colSummoneyBill.DefaultCellStyle = dataGridViewCellStyle10;
            this.colSummoneyBill.HeaderText = "金额";
            this.colSummoneyBill.Name = "colSummoneyBill";
            this.colSummoneyBill.Width = 150;
            // 
            // colStateBill
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            this.colStateBill.DefaultCellStyle = dataGridViewCellStyle11;
            this.colStateBill.HeaderText = "状态";
            this.colStateBill.Name = "colStateBill";
            this.colStateBill.Width = 80;
            // 
            // colCreatePersonBill
            // 
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            this.colCreatePersonBill.DefaultCellStyle = dataGridViewCellStyle12;
            this.colCreatePersonBill.HeaderText = "制单人";
            this.colCreatePersonBill.Name = "colCreatePersonBill";
            this.colCreatePersonBill.Width = 80;
            // 
            // colRealOperationDateBill
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            this.colRealOperationDateBill.DefaultCellStyle = dataGridViewCellStyle13;
            this.colRealOperationDateBill.HeaderText = "制单日期";
            this.colRealOperationDateBill.Name = "colRealOperationDateBill";
            this.colRealOperationDateBill.Width = 80;
            // 
            // colDescriptBill
            // 
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            this.colDescriptBill.DefaultCellStyle = dataGridViewCellStyle14;
            this.colDescriptBill.HeaderText = "备注";
            this.colDescriptBill.Name = "colDescriptBill";
            this.colDescriptBill.Width = 250;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgMaster);
            this.groupBox2.Location = new System.Drawing.Point(7, 68);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(910, 177);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "费用信息列表";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgDetail);
            this.groupBox3.Location = new System.Drawing.Point(7, 251);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(910, 204);
            this.groupBox3.TabIndex = 103;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "费用详细信息";
            // 
            // chkRerveser
            // 
            this.chkRerveser.AutoSize = true;
            this.chkRerveser.Location = new System.Drawing.Point(76, 465);
            this.chkRerveser.Name = "chkRerveser";
            this.chkRerveser.Size = new System.Drawing.Size(48, 16);
            this.chkRerveser.TabIndex = 107;
            this.chkRerveser.Text = "反选";
            this.chkRerveser.UseVisualStyleBackColor = true;
            // 
            // chkCheckAll
            // 
            this.chkCheckAll.AutoSize = true;
            this.chkCheckAll.Location = new System.Drawing.Point(12, 465);
            this.chkCheckAll.Name = "chkCheckAll";
            this.chkCheckAll.Size = new System.Drawing.Size(48, 16);
            this.chkCheckAll.TabIndex = 106;
            this.chkCheckAll.Text = "全选";
            this.chkCheckAll.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(665, 461);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 105;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSure
            // 
            this.btnSure.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSure.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSure.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSure.Location = new System.Drawing.Point(314, 461);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 104;
            this.btnSure.Text = "确认";
            this.btnSure.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearchBill);
            this.groupBox1.Controls.Add(this.txtCodeBeginBill);
            this.groupBox1.Controls.Add(this.customLabel13);
            this.groupBox1.Controls.Add(this.txtCreatePersonBill);
            this.groupBox1.Controls.Add(this.customLabel15);
            this.groupBox1.Controls.Add(this.customLabel19);
            this.groupBox1.Controls.Add(this.dtpDateBeginBill);
            this.groupBox1.Controls.Add(this.dtpDateEndBill);
            this.groupBox1.Controls.Add(this.customLabel20);
            this.groupBox1.Location = new System.Drawing.Point(7, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(910, 61);
            this.groupBox1.TabIndex = 108;
            this.groupBox1.TabStop = false;
            // 
            // btnSearchBill
            // 
            this.btnSearchBill.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearchBill.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearchBill.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearchBill.Location = new System.Drawing.Point(805, 19);
            this.btnSearchBill.Name = "btnSearchBill";
            this.btnSearchBill.Size = new System.Drawing.Size(75, 23);
            this.btnSearchBill.TabIndex = 97;
            this.btnSearchBill.Text = "查询";
            this.btnSearchBill.UseVisualStyleBackColor = true;
            // 
            // txtCodeBeginBill
            // 
            this.txtCodeBeginBill.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBeginBill.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBeginBill.DrawSelf = false;
            this.txtCodeBeginBill.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBeginBill.EnterToTab = false;
            this.txtCodeBeginBill.Location = new System.Drawing.Point(81, 21);
            this.txtCodeBeginBill.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBeginBill.Name = "txtCodeBeginBill";
            this.txtCodeBeginBill.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBeginBill.ReadOnly = false;
            this.txtCodeBeginBill.Size = new System.Drawing.Size(182, 16);
            this.txtCodeBeginBill.TabIndex = 89;
            // 
            // customLabel13
            // 
            this.customLabel13.AutoSize = true;
            this.customLabel13.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel13.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel13.Location = new System.Drawing.Point(293, 25);
            this.customLabel13.Name = "customLabel13";
            this.customLabel13.Size = new System.Drawing.Size(47, 12);
            this.customLabel13.TabIndex = 96;
            this.customLabel13.Text = "制单人:";
            this.customLabel13.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePersonBill
            // 
            this.txtCreatePersonBill.AcceptsEscape = false;
            this.txtCreatePersonBill.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePersonBill.Code = null;
            this.txtCreatePersonBill.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtCreatePersonBill.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtCreatePersonBill.EnterToTab = false;
            this.txtCreatePersonBill.Id = "";
            this.txtCreatePersonBill.IsAllLoad = true;
            this.txtCreatePersonBill.Location = new System.Drawing.Point(340, 21);
            this.txtCreatePersonBill.Name = "txtCreatePersonBill";
            this.txtCreatePersonBill.Result = ((System.Collections.IList)(resources.GetObject("txtCreatePersonBill.Result")));
            this.txtCreatePersonBill.RightMouse = false;
            this.txtCreatePersonBill.Size = new System.Drawing.Size(126, 21);
            this.txtCreatePersonBill.TabIndex = 92;
            this.txtCreatePersonBill.Tag = null;
            this.txtCreatePersonBill.Value = "";
            this.txtCreatePersonBill.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel15
            // 
            this.customLabel15.AutoSize = true;
            this.customLabel15.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel15.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel15.Location = new System.Drawing.Point(28, 24);
            this.customLabel15.Name = "customLabel15";
            this.customLabel15.Size = new System.Drawing.Size(47, 12);
            this.customLabel15.TabIndex = 93;
            this.customLabel15.Text = "单据号:";
            this.customLabel15.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel19
            // 
            this.customLabel19.AutoSize = true;
            this.customLabel19.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel19.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel19.Location = new System.Drawing.Point(486, 24);
            this.customLabel19.Name = "customLabel19";
            this.customLabel19.Size = new System.Drawing.Size(59, 12);
            this.customLabel19.TabIndex = 94;
            this.customLabel19.Text = "截至日期:";
            this.customLabel19.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBeginBill
            // 
            this.dtpDateBeginBill.Location = new System.Drawing.Point(545, 20);
            this.dtpDateBeginBill.Name = "dtpDateBeginBill";
            this.dtpDateBeginBill.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBeginBill.TabIndex = 90;
            // 
            // dtpDateEndBill
            // 
            this.dtpDateEndBill.Location = new System.Drawing.Point(670, 20);
            this.dtpDateEndBill.Name = "dtpDateEndBill";
            this.dtpDateEndBill.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEndBill.TabIndex = 91;
            // 
            // customLabel20
            // 
            this.customLabel20.AutoSize = true;
            this.customLabel20.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel20.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel20.Location = new System.Drawing.Point(656, 24);
            this.customLabel20.Name = "customLabel20";
            this.customLabel20.Size = new System.Drawing.Size(11, 12);
            this.customLabel20.TabIndex = 95;
            this.customLabel20.Text = "-";
            this.customLabel20.UnderLineColor = System.Drawing.Color.Red;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 50;
            // 
            // colAccountTitle
            // 
            this.colAccountTitle.HeaderText = "费用类型";
            this.colAccountTitle.Name = "colAccountTitle";
            this.colAccountTitle.ReadOnly = true;
            // 
            // colBudgetMoney
            // 
            this.colBudgetMoney.HeaderText = "预算金额(元)";
            this.colBudgetMoney.Name = "colBudgetMoney";
            this.colBudgetMoney.ReadOnly = true;
            this.colBudgetMoney.Width = 150;
            // 
            // colActualMoney
            // 
            this.colActualMoney.HeaderText = "实际支出(元)";
            this.colActualMoney.Name = "colActualMoney";
            this.colActualMoney.ReadOnly = true;
            this.colActualMoney.Width = 150;
            // 
            // colRate
            // 
            this.colRate.HeaderText = "比率(%)";
            this.colRate.Name = "colRate";
            this.colRate.ReadOnly = true;
            this.colRate.Width = 80;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            this.colDescript.ReadOnly = true;
            this.colDescript.Width = 300;
            // 
            // VIndirectCostCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 489);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkRerveser);
            this.Controls.Add(this.chkCheckAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "VIndirectCostCopy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "复制费用信息";
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePersonBill)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private System.Windows.Forms.DataGridViewLinkColumn colCodeBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDateBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSummoneyBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStateBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePersonBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperationDateBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescriptBill;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkRerveser;
        private System.Windows.Forms.CheckBox chkCheckAll;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSure;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearchBill;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBeginBill;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel13;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtCreatePersonBill;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel15;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel19;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBeginBill;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEndBill;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel20;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBudgetMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
    }
}