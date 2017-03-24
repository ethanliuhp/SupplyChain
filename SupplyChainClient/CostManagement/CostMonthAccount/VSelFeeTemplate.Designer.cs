﻿namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    partial class VSelFeeTemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSelFeeTemplate));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tspMenuSpecials = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tspMenuAddSpecial = new System.Windows.Forms.ToolStripMenuItem();
            this.tspMenuMainSubject = new System.Windows.Forms.ToolStripMenuItem();
            this.tspMenuName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tspMenuGetSubject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDeleteRow = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSpecialType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colMainAccSubjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMainAccSubjectCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccountSubjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccountSubjectCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBeginMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgRules = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colAccountSubject_rules = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccountSubjectCode_rules = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFormula_rules = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript_rules = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.comDocState = new System.Windows.Forms.ComboBox();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlBody.SuspendLayout();
            this.pnlFloor.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRules)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl1);
            this.pnlBody.Location = new System.Drawing.Point(0, 73);
            this.pnlBody.Size = new System.Drawing.Size(1029, 402);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Location = new System.Drawing.Point(0, 475);
            this.pnlFooter.Size = new System.Drawing.Size(1029, 38);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(1029, 0);
            // 
            // pnlFloor
            // 
            this.pnlFloor.Size = new System.Drawing.Size(1029, 513);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.txtRemark);
            this.pnlHeader.Controls.Add(this.customLabel9);
            this.pnlHeader.Controls.Add(this.txtName);
            this.pnlHeader.Controls.Add(this.customLabel2);
            this.pnlHeader.Controls.Add(this.txtCode);
            this.pnlHeader.Controls.Add(this.lblSupplyContract);
            this.pnlHeader.Controls.Add(this.customLabel6);
            this.pnlHeader.Controls.Add(this.comDocState);
            this.pnlHeader.Size = new System.Drawing.Size(1029, 73);
            this.pnlHeader.Controls.SetChildIndex(this.comDocState, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel6, 0);
            this.pnlHeader.Controls.SetChildIndex(this.lblSupplyContract, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtCode, 0);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel2, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtName, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel9, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtRemark, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(308, 10);
            // 
            // tabControl1
            // 
            this.tabControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1017, 402);
            this.tabControl1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspMenuSpecials,
            this.tspMenuMainSubject,
            this.tspMenuName,
            this.toolStripSeparator2,
            this.tspMenuGetSubject,
            this.menuItemDeleteRow});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 142);
            // 
            // tspMenuSpecials
            // 
            this.tspMenuSpecials.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.tspMenuAddSpecial});
            this.tspMenuSpecials.Name = "tspMenuSpecials";
            this.tspMenuSpecials.Size = new System.Drawing.Size(152, 22);
            this.tspMenuSpecials.Text = "专业类型";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // tspMenuAddSpecial
            // 
            this.tspMenuAddSpecial.Name = "tspMenuAddSpecial";
            this.tspMenuAddSpecial.Size = new System.Drawing.Size(124, 22);
            this.tspMenuAddSpecial.Text = "快速添加";
            // 
            // tspMenuMainSubject
            // 
            this.tspMenuMainSubject.Name = "tspMenuMainSubject";
            this.tspMenuMainSubject.Size = new System.Drawing.Size(152, 22);
            this.tspMenuMainSubject.Text = "复制科目大类";
            // 
            // tspMenuName
            // 
            this.tspMenuName.Name = "tspMenuName";
            this.tspMenuName.Size = new System.Drawing.Size(152, 22);
            this.tspMenuName.Text = "复制科目名称";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // tspMenuGetSubject
            // 
            this.tspMenuGetSubject.Name = "tspMenuGetSubject";
            this.tspMenuGetSubject.Size = new System.Drawing.Size(152, 22);
            this.tspMenuGetSubject.Text = "获取科目大类";
            // 
            // menuItemDeleteRow
            // 
            this.menuItemDeleteRow.Name = "menuItemDeleteRow";
            this.menuItemDeleteRow.Size = new System.Drawing.Size(152, 22);
            this.menuItemDeleteRow.Text = "删除所选行";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1009, 376);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "费用明细定义 ";
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
            this.colSpecialType,
            this.colMainAccSubjectName,
            this.colMainAccSubjectCode,
            this.colAccountSubjectName,
            this.colAccountSubjectCode,
            this.colRate,
            this.colBeginMoney,
            this.colEndMoney,
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
            this.dgDetail.Size = new System.Drawing.Size(1003, 370);
            this.dgDetail.TabIndex = 10;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colSpecialType
            // 
            this.colSpecialType.HeaderText = "专业类型";
            this.colSpecialType.Name = "colSpecialType";
            this.colSpecialType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSpecialType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colMainAccSubjectName
            // 
            this.colMainAccSubjectName.HeaderText = "科目大类";
            this.colMainAccSubjectName.Name = "colMainAccSubjectName";
            this.colMainAccSubjectName.ReadOnly = true;
            this.colMainAccSubjectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMainAccSubjectName.Width = 200;
            // 
            // colMainAccSubjectCode
            // 
            this.colMainAccSubjectCode.HeaderText = "科目大类编码";
            this.colMainAccSubjectCode.Name = "colMainAccSubjectCode";
            this.colMainAccSubjectCode.Visible = false;
            this.colMainAccSubjectCode.Width = 200;
            // 
            // colAccountSubjectName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.colAccountSubjectName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colAccountSubjectName.HeaderText = "科目名称";
            this.colAccountSubjectName.Name = "colAccountSubjectName";
            this.colAccountSubjectName.ReadOnly = true;
            this.colAccountSubjectName.Width = 140;
            // 
            // colAccountSubjectCode
            // 
            this.colAccountSubjectCode.HeaderText = "科目编码";
            this.colAccountSubjectCode.Name = "colAccountSubjectCode";
            this.colAccountSubjectCode.Visible = false;
            // 
            // colRate
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colRate.DefaultCellStyle = dataGridViewCellStyle2;
            this.colRate.FillWeight = 80F;
            this.colRate.HeaderText = "费率(%)";
            this.colRate.Name = "colRate";
            this.colRate.Width = 120;
            // 
            // colBeginMoney
            // 
            this.colBeginMoney.HeaderText = "开始金额";
            this.colBeginMoney.Name = "colBeginMoney";
            // 
            // colEndMoney
            // 
            this.colEndMoney.HeaderText = "结束金额";
            this.colEndMoney.Name = "colEndMoney";
            this.colEndMoney.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 300;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgRules);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(614, 188);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "计算规则定义";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgRules
            // 
            this.dgRules.AddDefaultMenu = false;
            this.dgRules.AddNoColumn = true;
            this.dgRules.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgRules.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgRules.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgRules.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgRules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAccountSubject_rules,
            this.colAccountSubjectCode_rules,
            this.colFormula_rules,
            this.colDescript_rules});
            this.dgRules.CustomBackColor = false;
            this.dgRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgRules.EditCellBackColor = System.Drawing.Color.White;
            this.dgRules.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgRules.FreezeFirstRow = false;
            this.dgRules.FreezeLastRow = false;
            this.dgRules.FrontColumnCount = 0;
            this.dgRules.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgRules.HScrollOffset = 0;
            this.dgRules.IsAllowOrder = true;
            this.dgRules.IsConfirmDelete = true;
            this.dgRules.Location = new System.Drawing.Point(3, 3);
            this.dgRules.Name = "dgRules";
            this.dgRules.PageIndex = 0;
            this.dgRules.PageSize = 0;
            this.dgRules.Query = null;
            this.dgRules.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgRules.ReadOnlyCols")));
            this.dgRules.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgRules.RowHeadersWidth = 22;
            this.dgRules.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgRules.RowTemplate.Height = 23;
            this.dgRules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgRules.Size = new System.Drawing.Size(608, 182);
            this.dgRules.TabIndex = 10;
            this.dgRules.TargetType = null;
            this.dgRules.VScrollOffset = 0;
            // 
            // colAccountSubject_rules
            // 
            this.colAccountSubject_rules.HeaderText = "科目";
            this.colAccountSubject_rules.Name = "colAccountSubject_rules";
            this.colAccountSubject_rules.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAccountSubject_rules.Width = 200;
            // 
            // colAccountSubjectCode_rules
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colAccountSubjectCode_rules.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAccountSubjectCode_rules.HeaderText = "科目编码";
            this.colAccountSubjectCode_rules.Name = "colAccountSubjectCode_rules";
            this.colAccountSubjectCode_rules.ReadOnly = true;
            this.colAccountSubjectCode_rules.Visible = false;
            this.colAccountSubjectCode_rules.Width = 140;
            // 
            // colFormula_rules
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.colFormula_rules.DefaultCellStyle = dataGridViewCellStyle4;
            this.colFormula_rules.FillWeight = 80F;
            this.colFormula_rules.HeaderText = "公式";
            this.colFormula_rules.Name = "colFormula_rules";
            this.colFormula_rules.Width = 400;
            // 
            // colDescript_rules
            // 
            this.colDescript_rules.HeaderText = "备注";
            this.colDescript_rules.Name = "colDescript_rules";
            this.colDescript_rules.Width = 400;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(785, 18);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 272;
            this.customLabel6.Text = "单据状态:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // comDocState
            // 
            this.comDocState.FormattingEnabled = true;
            this.comDocState.Location = new System.Drawing.Point(857, 14);
            this.comDocState.Name = "comDocState";
            this.comDocState.Size = new System.Drawing.Size(129, 20);
            this.comDocState.TabIndex = 271;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(72, 16);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(144, 16);
            this.txtCode.TabIndex = 274;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(26, 18);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(47, 12);
            this.lblSupplyContract.TabIndex = 273;
            this.lblSupplyContract.Text = "单据号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(72, 44);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(914, 20);
            this.txtRemark.TabIndex = 277;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(14, 48);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(59, 12);
            this.customLabel9.TabIndex = 278;
            this.customLabel9.Text = "模板描述:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.Control;
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = false;
            this.txtName.Location = new System.Drawing.Point(293, 14);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(457, 20);
            this.txtName.TabIndex = 275;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(221, 18);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 276;
            this.customLabel2.Text = "模板名称:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VSelFeeTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 513);
            this.Name = "VSelFeeTemplate";
            this.Text = "取费模板定义";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlBody.ResumeLayout(false);
            this.pnlFloor.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgRules)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgRules;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private System.Windows.Forms.ComboBox comDocState;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountSubject_rules;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountSubjectCode_rules;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFormula_rules;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript_rules;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tspMenuSpecials;
        private System.Windows.Forms.ToolStripMenuItem tspMenuMainSubject;
        private System.Windows.Forms.ToolStripMenuItem tspMenuName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tspMenuAddSpecial;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tspMenuGetSubject;
        private System.Windows.Forms.DataGridViewComboBoxColumn colSpecialType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMainAccSubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMainAccSubjectCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountSubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountSubjectCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBeginMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.ToolStripMenuItem menuItemDeleteRow;
    }
}