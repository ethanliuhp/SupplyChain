namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    partial class VWeekSchedule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VWeekSchedule));
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.btnExportToMPP = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.cboSchedulePlanName = new System.Windows.Forms.ComboBox();
            this.cbPlanType = new System.Windows.Forms.ComboBox();
            this.lblSupplier = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmoStartMonth = new System.Windows.Forms.ComboBox();
            this.cmoYear = new System.Windows.Forms.ComboBox();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnProductSchedule = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtProductSchedule = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtPlanName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmsDg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDgOther = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDgOtherDel = new System.Windows.Forms.ToolStripMenuItem();
            this.tpTreePlan = new System.Windows.Forms.TabPage();
            this.flexGrid = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.pnlBody.SuspendLayout();
            this.pnlFloor.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            this.cmsDg.SuspendLayout();
            this.cmsDgOther.SuspendLayout();
            this.tpTreePlan.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Size = new System.Drawing.Size(1017, 492);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl1);
            this.pnlBody.Location = new System.Drawing.Point(0, 119);
            this.pnlBody.Size = new System.Drawing.Size(1017, 342);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Location = new System.Drawing.Point(0, 461);
            this.pnlFooter.Size = new System.Drawing.Size(1017, 31);
            this.pnlFooter.Visible = false;
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(1017, 0);
            this.spTop.Visible = false;
            // 
            // pnlFloor
            // 
            this.pnlFloor.Size = new System.Drawing.Size(1017, 492);
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Size = new System.Drawing.Size(1017, 492);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.groupSupply);
            this.pnlHeader.Size = new System.Drawing.Size(1017, 119);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.groupSupply, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Visible = false;
            // 
            // groupSupply
            // 
            this.groupSupply.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSupply.Controls.Add(this.btnExportToMPP);
            this.groupSupply.Controls.Add(this.cboSchedulePlanName);
            this.groupSupply.Controls.Add(this.cbPlanType);
            this.groupSupply.Controls.Add(this.lblSupplier);
            this.groupSupply.Controls.Add(this.label3);
            this.groupSupply.Controls.Add(this.label2);
            this.groupSupply.Controls.Add(this.cmoStartMonth);
            this.groupSupply.Controls.Add(this.cmoYear);
            this.groupSupply.Controls.Add(this.customLabel6);
            this.groupSupply.Controls.Add(this.btnProductSchedule);
            this.groupSupply.Controls.Add(this.txtProductSchedule);
            this.groupSupply.Controls.Add(this.customLabel7);
            this.groupSupply.Controls.Add(this.txtRemark);
            this.groupSupply.Controls.Add(this.customLabel9);
            this.groupSupply.Controls.Add(this.customLabel8);
            this.groupSupply.Controls.Add(this.dtpDateEnd);
            this.groupSupply.Controls.Add(this.dtpDateBegin);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.txtPlanName);
            this.groupSupply.Controls.Add(this.customLabel2);
            this.groupSupply.Location = new System.Drawing.Point(7, 3);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(1004, 111);
            this.groupSupply.TabIndex = 2;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>计划基本信息";
            // 
            // btnExportToMPP
            // 
            this.btnExportToMPP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportToMPP.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExportToMPP.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExportToMPP.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExportToMPP.Location = new System.Drawing.Point(768, 77);
            this.btnExportToMPP.Name = "btnExportToMPP";
            this.btnExportToMPP.Size = new System.Drawing.Size(76, 23);
            this.btnExportToMPP.TabIndex = 168;
            this.btnExportToMPP.Text = "导出MPP";
            this.btnExportToMPP.UseVisualStyleBackColor = true;
            // 
            // cboSchedulePlanName
            // 
            this.cboSchedulePlanName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSchedulePlanName.FormattingEnabled = true;
            this.cboSchedulePlanName.Location = new System.Drawing.Point(314, 20);
            this.cboSchedulePlanName.Name = "cboSchedulePlanName";
            this.cboSchedulePlanName.Size = new System.Drawing.Size(275, 20);
            this.cboSchedulePlanName.TabIndex = 255;
            // 
            // cbPlanType
            // 
            this.cbPlanType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlanType.FormattingEnabled = true;
            this.cbPlanType.Location = new System.Drawing.Point(92, 21);
            this.cbPlanType.Name = "cbPlanType";
            this.cbPlanType.Size = new System.Drawing.Size(130, 20);
            this.cbPlanType.TabIndex = 254;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplier.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplier.Location = new System.Drawing.Point(26, 25);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(59, 12);
            this.lblSupplier.TabIndex = 253;
            this.lblSupplier.Text = "计划类型:";
            this.lblSupplier.UnderLineColor = System.Drawing.Color.Red;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(709, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 250;
            this.label3.Text = "年";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(787, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 249;
            this.label2.Text = "月";
            // 
            // cmoStartMonth
            // 
            this.cmoStartMonth.FormattingEnabled = true;
            this.cmoStartMonth.Location = new System.Drawing.Point(738, 53);
            this.cmoStartMonth.Name = "cmoStartMonth";
            this.cmoStartMonth.Size = new System.Drawing.Size(48, 20);
            this.cmoStartMonth.TabIndex = 248;
            // 
            // cmoYear
            // 
            this.cmoYear.FormattingEnabled = true;
            this.cmoYear.Location = new System.Drawing.Point(651, 53);
            this.cmoYear.Name = "cmoYear";
            this.cmoYear.Size = new System.Drawing.Size(52, 20);
            this.cmoYear.TabIndex = 247;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(593, 57);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 246;
            this.customLabel6.Text = "计划年月:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnProductSchedule
            // 
            this.btnProductSchedule.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnProductSchedule.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProductSchedule.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnProductSchedule.Location = new System.Drawing.Point(610, 77);
            this.btnProductSchedule.Name = "btnProductSchedule";
            this.btnProductSchedule.Size = new System.Drawing.Size(150, 23);
            this.btnProductSchedule.TabIndex = 3;
            this.btnProductSchedule.Text = "生成年度进度计划";
            this.btnProductSchedule.UseVisualStyleBackColor = true;
            // 
            // txtProductSchedule
            // 
            this.txtProductSchedule.BackColor = System.Drawing.SystemColors.Control;
            this.txtProductSchedule.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProductSchedule.DrawSelf = false;
            this.txtProductSchedule.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProductSchedule.EnterToTab = false;
            this.txtProductSchedule.Location = new System.Drawing.Point(314, 21);
            this.txtProductSchedule.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProductSchedule.Name = "txtProductSchedule";
            this.txtProductSchedule.Padding = new System.Windows.Forms.Padding(1);
            this.txtProductSchedule.ReadOnly = true;
            this.txtProductSchedule.Size = new System.Drawing.Size(270, 20);
            this.txtProductSchedule.TabIndex = 143;
            this.txtProductSchedule.Visible = false;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(235, 25);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(83, 12);
            this.customLabel7.TabIndex = 142;
            this.customLabel7.Text = "总体进度计划:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(92, 78);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(497, 20);
            this.txtRemark.TabIndex = 4;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(26, 82);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(59, 12);
            this.customLabel9.TabIndex = 140;
            this.customLabel9.Text = "计划说明:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(766, 25);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(17, 12);
            this.customLabel8.TabIndex = 138;
            this.customLabel8.Text = "至";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(789, 21);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 2;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(651, 21);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 1;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(590, 25);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 130;
            this.customLabel4.Text = "计划日期:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtPlanName
            // 
            this.txtPlanName.BackColor = System.Drawing.SystemColors.Control;
            this.txtPlanName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPlanName.DrawSelf = false;
            this.txtPlanName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPlanName.EnterToTab = false;
            this.txtPlanName.Location = new System.Drawing.Point(92, 49);
            this.txtPlanName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPlanName.Name = "txtPlanName";
            this.txtPlanName.Padding = new System.Windows.Forms.Padding(1);
            this.txtPlanName.ReadOnly = false;
            this.txtPlanName.Size = new System.Drawing.Size(497, 20);
            this.txtPlanName.TabIndex = 0;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(26, 53);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 0;
            this.customLabel2.Text = "计划名称:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmsDg
            // 
            this.cmsDg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDel,
            this.tsmiCopy});
            this.cmsDg.Name = "cmsDg";
            this.cmsDg.Size = new System.Drawing.Size(101, 48);
            // 
            // tsmiDel
            // 
            this.tsmiDel.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.tsmiDel.Name = "tsmiDel";
            this.tsmiDel.Size = new System.Drawing.Size(100, 22);
            this.tsmiDel.Text = "删除";
            this.tsmiDel.ToolTipText = "删除当前选中的记录";
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.新增;
            this.tsmiCopy.Name = "tsmiCopy";
            this.tsmiCopy.Size = new System.Drawing.Size(100, 22);
            this.tsmiCopy.Text = "复制";
            // 
            // cmsDgOther
            // 
            this.cmsDgOther.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDgOtherDel});
            this.cmsDgOther.Name = "cmsDg";
            this.cmsDgOther.Size = new System.Drawing.Size(101, 26);
            // 
            // tsmiDgOtherDel
            // 
            this.tsmiDgOtherDel.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.tsmiDgOtherDel.Name = "tsmiDgOtherDel";
            this.tsmiDgOtherDel.Size = new System.Drawing.Size(100, 22);
            this.tsmiDgOtherDel.Text = "删除";
            this.tsmiDgOtherDel.ToolTipText = "删除当前选中的记录";
            // 
            // tpTreePlan
            // 
            this.tpTreePlan.BackColor = System.Drawing.SystemColors.Control;
            this.tpTreePlan.Controls.Add(this.flexGrid);
            this.tpTreePlan.Location = new System.Drawing.Point(4, 22);
            this.tpTreePlan.Name = "tpTreePlan";
            this.tpTreePlan.Size = new System.Drawing.Size(997, 316);
            this.tpTreePlan.TabIndex = 2;
            this.tpTreePlan.Text = "树方式";
            // 
            // flexGrid
            // 
            this.flexGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid.CheckedImage")));
            this.flexGrid.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.flexGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flexGrid.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGrid.Location = new System.Drawing.Point(0, 0);
            this.flexGrid.Name = "flexGrid";
            this.flexGrid.Size = new System.Drawing.Size(997, 316);
            this.flexGrid.TabIndex = 173;
            this.flexGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid.UncheckedImage")));
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpTreePlan);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1005, 342);
            this.tabControl1.TabIndex = 2;
            // 
            // VWeekSchedule
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1017, 492);
            this.Name = "VWeekSchedule";
            this.Text = "进度计划编制";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlBody.ResumeLayout(false);
            this.pnlFloor.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            this.cmsDg.ResumeLayout(false);
            this.cmsDgOther.ResumeLayout(false);
            this.tpTreePlan.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private System.Windows.Forms.ContextMenuStrip cmsDg;
        private System.Windows.Forms.ToolStripMenuItem tsmiDel;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private System.Windows.Forms.ContextMenuStrip cmsDgOther;
        private System.Windows.Forms.ToolStripMenuItem tsmiDgOtherDel;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnProductSchedule;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProductSchedule;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPlanName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmoStartMonth;
        private System.Windows.Forms.ComboBox cmoYear;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplier;
        private System.Windows.Forms.ComboBox cbPlanType;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExportToMPP;
        private System.Windows.Forms.ComboBox cboSchedulePlanName;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpTreePlan;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid;
    }
}