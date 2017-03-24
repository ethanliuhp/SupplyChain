namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    partial class VWeekScheduleSummary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VWeekScheduleSummary));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.dtpBusinessDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnWeekSchedule = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCompletionAnalysis = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmsDg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDel = new System.Windows.Forms.ToolStripMenuItem();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpWeekPlan = new System.Windows.Forms.TabPage();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colWeekPlanName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOwner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGWBSTree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMainTaskContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMonday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colTuesday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colWednesday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colThursday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFriday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSaturday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSunday = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPlannedDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOBSService = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPBSTree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colForwardBillDtlIdOfWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flexGrid1 = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.pnlContent.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.spTop.SuspendLayout();
            this.pnlFloor.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            this.cmsDg.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpWeekPlan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Size = new System.Drawing.Size(1047, 449);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl1);
            this.pnlBody.Size = new System.Drawing.Size(1045, 307);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtProject);
            this.pnlFooter.Controls.Add(this.customLabel1);
            this.pnlFooter.Controls.Add(this.txtCreatePerson);
            this.pnlFooter.Controls.Add(this.customLabel5);
            this.pnlFooter.Location = new System.Drawing.Point(0, 408);
            this.pnlFooter.Size = new System.Drawing.Size(1045, 39);
            // 
            // spTop
            // 
            this.spTop.Controls.Add(this.flexGrid1);
            this.spTop.Size = new System.Drawing.Size(1047, 45);
            this.spTop.Visible = false;
            // 
            // pnlFloor
            // 
            this.pnlFloor.Size = new System.Drawing.Size(1047, 494);
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Location = new System.Drawing.Point(0, 45);
            this.pnlWorkSpace.Size = new System.Drawing.Size(1047, 449);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.groupSupply);
            this.pnlHeader.Size = new System.Drawing.Size(1045, 101);
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
            this.groupSupply.Controls.Add(this.dtpBusinessDate);
            this.groupSupply.Controls.Add(this.customLabel7);
            this.groupSupply.Controls.Add(this.btnWeekSchedule);
            this.groupSupply.Controls.Add(this.txtRemark);
            this.groupSupply.Controls.Add(this.customLabel9);
            this.groupSupply.Controls.Add(this.customLabel8);
            this.groupSupply.Controls.Add(this.dtpDateEnd);
            this.groupSupply.Controls.Add(this.dtpDateBegin);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.txtCompletionAnalysis);
            this.groupSupply.Controls.Add(this.customLabel3);
            this.groupSupply.Controls.Add(this.txtName);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.customLabel2);
            this.groupSupply.Controls.Add(this.lblSupplyContract);
            this.groupSupply.Location = new System.Drawing.Point(7, 3);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(1032, 93);
            this.groupSupply.TabIndex = 2;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>项目周计划基本信息";
            // 
            // dtpBusinessDate
            // 
            this.dtpBusinessDate.Location = new System.Drawing.Point(786, 13);
            this.dtpBusinessDate.Name = "dtpBusinessDate";
            this.dtpBusinessDate.Size = new System.Drawing.Size(109, 21);
            this.dtpBusinessDate.TabIndex = 146;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(725, 19);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 147;
            this.customLabel7.Text = "业务日期:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnWeekSchedule
            // 
            this.btnWeekSchedule.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnWeekSchedule.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWeekSchedule.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnWeekSchedule.Location = new System.Drawing.Point(911, 12);
            this.btnWeekSchedule.Name = "btnWeekSchedule";
            this.btnWeekSchedule.Size = new System.Drawing.Size(113, 23);
            this.btnWeekSchedule.TabIndex = 142;
            this.btnWeekSchedule.Text = "选择工区周计划";
            this.btnWeekSchedule.UseVisualStyleBackColor = true;
            // 
            // txtRemark
            // 
            this.txtRemark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(84, 43);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(941, 16);
            this.txtRemark.TabIndex = 141;
            // 
            // customLabel9
            // 
            this.customLabel9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(28, 47);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(59, 12);
            this.customLabel9.TabIndex = 140;
            this.customLabel9.Text = "计划说明:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel8
            // 
            this.customLabel8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(585, 18);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(17, 12);
            this.customLabel8.TabIndex = 138;
            this.customLabel8.Text = "至";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(608, 14);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 137;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(470, 14);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 2;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(412, 19);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 130;
            this.customLabel4.Text = "计划日期:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCompletionAnalysis
            // 
            this.txtCompletionAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCompletionAnalysis.BackColor = System.Drawing.SystemColors.Control;
            this.txtCompletionAnalysis.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCompletionAnalysis.DrawSelf = false;
            this.txtCompletionAnalysis.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCompletionAnalysis.EnterToTab = false;
            this.txtCompletionAnalysis.Location = new System.Drawing.Point(84, 67);
            this.txtCompletionAnalysis.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCompletionAnalysis.Name = "txtCompletionAnalysis";
            this.txtCompletionAnalysis.Padding = new System.Windows.Forms.Padding(1);
            this.txtCompletionAnalysis.ReadOnly = false;
            this.txtCompletionAnalysis.Size = new System.Drawing.Size(941, 16);
            this.txtCompletionAnalysis.TabIndex = 6;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(4, 69);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(83, 12);
            this.customLabel3.TabIndex = 9;
            this.customLabel3.Text = "完成情况分析:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.Control;
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = false;
            this.txtName.Location = new System.Drawing.Point(84, 18);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = false;
            this.txtName.Size = new System.Drawing.Size(137, 16);
            this.txtName.TabIndex = 1;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(286, 18);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(120, 16);
            this.txtCode.TabIndex = 1;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(16, 22);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(71, 12);
            this.customLabel2.TabIndex = 0;
            this.customLabel2.Text = "周计划名称:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(227, 22);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(59, 12);
            this.lblSupplyContract.TabIndex = 0;
            this.lblSupplyContract.Text = "周计划号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmsDg
            // 
            this.cmsDg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDel});
            this.cmsDg.Name = "cmsDg";
            this.cmsDg.Size = new System.Drawing.Size(95, 26);
            // 
            // tsmiDel
            // 
            this.tsmiDel.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.tsmiDel.Name = "tsmiDel";
            this.tsmiDel.Size = new System.Drawing.Size(94, 22);
            this.tsmiDel.Text = "删除";
            this.tsmiDel.ToolTipText = "删除当前选中的记录";
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(63, 17);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = true;
            this.txtCreatePerson.Size = new System.Drawing.Size(57, 16);
            this.txtCreatePerson.TabIndex = 8;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(12, 19);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(47, 12);
            this.customLabel5.TabIndex = 25;
            this.customLabel5.Text = "制单人:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProject
            // 
            this.txtProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProject.DrawSelf = false;
            this.txtProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProject.EnterToTab = false;
            this.txtProject.Location = new System.Drawing.Point(184, 17);
            this.txtProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProject.Name = "txtProject";
            this.txtProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtProject.ReadOnly = true;
            this.txtProject.Size = new System.Drawing.Size(158, 16);
            this.txtProject.TabIndex = 11;
            // 
            // customLabel1
            // 
            this.customLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(126, 19);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 31;
            this.customLabel1.Text = "归属项目:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpWeekPlan);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1033, 307);
            this.tabControl1.TabIndex = 2;
            // 
            // tpWeekPlan
            // 
            this.tpWeekPlan.Controls.Add(this.dgDetail);
            this.tpWeekPlan.Location = new System.Drawing.Point(4, 21);
            this.tpWeekPlan.Name = "tpWeekPlan";
            this.tpWeekPlan.Padding = new System.Windows.Forms.Padding(3);
            this.tpWeekPlan.Size = new System.Drawing.Size(1025, 282);
            this.tpWeekPlan.TabIndex = 0;
            this.tpWeekPlan.Text = "项目周计划明细";
            this.tpWeekPlan.UseVisualStyleBackColor = true;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = true;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.AllowUserToResizeRows = false;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWeekPlanName,
            this.colOwner,
            this.colGWBSTree,
            this.colMainTaskContent,
            this.colMonday,
            this.colTuesday,
            this.colWednesday,
            this.colThursday,
            this.colFriday,
            this.colSaturday,
            this.colSunday,
            this.colPlannedDuration,
            this.colOBSService,
            this.colDescript,
            this.colPBSTree,
            this.colForwardBillDtlIdOfWeek});
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
            this.dgDetail.Location = new System.Drawing.Point(3, 3);
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
            this.dgDetail.Size = new System.Drawing.Size(1019, 276);
            this.dgDetail.TabIndex = 4;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colWeekPlanName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.colWeekPlanName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colWeekPlanName.HeaderText = "工区周计划名称";
            this.colWeekPlanName.Name = "colWeekPlanName";
            this.colWeekPlanName.ReadOnly = true;
            // 
            // colOwner
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colOwner.DefaultCellStyle = dataGridViewCellStyle2;
            this.colOwner.HeaderText = "工区周计划编制人";
            this.colOwner.Name = "colOwner";
            this.colOwner.ReadOnly = true;
            this.colOwner.Width = 110;
            // 
            // colGWBSTree
            // 
            this.colGWBSTree.HeaderText = "任务名称";
            this.colGWBSTree.Name = "colGWBSTree";
            // 
            // colMainTaskContent
            // 
            this.colMainTaskContent.HeaderText = "工作内容";
            this.colMainTaskContent.Name = "colMainTaskContent";
            // 
            // colMonday
            // 
            this.colMonday.HeaderText = "星期一";
            this.colMonday.Name = "colMonday";
            this.colMonday.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMonday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colMonday.Width = 50;
            // 
            // colTuesday
            // 
            this.colTuesday.HeaderText = "星期二";
            this.colTuesday.Name = "colTuesday";
            this.colTuesday.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTuesday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colTuesday.Width = 50;
            // 
            // colWednesday
            // 
            this.colWednesday.HeaderText = "星期三";
            this.colWednesday.Name = "colWednesday";
            this.colWednesday.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colWednesday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colWednesday.Width = 50;
            // 
            // colThursday
            // 
            this.colThursday.HeaderText = "星期四";
            this.colThursday.Name = "colThursday";
            this.colThursday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colThursday.Width = 50;
            // 
            // colFriday
            // 
            this.colFriday.HeaderText = "星期五";
            this.colFriday.Name = "colFriday";
            this.colFriday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colFriday.Width = 50;
            // 
            // colSaturday
            // 
            this.colSaturday.HeaderText = "星期六";
            this.colSaturday.Name = "colSaturday";
            this.colSaturday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSaturday.Width = 50;
            // 
            // colSunday
            // 
            this.colSunday.HeaderText = "星期日";
            this.colSunday.Name = "colSunday";
            this.colSunday.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSunday.Width = 50;
            // 
            // colPlannedDuration
            // 
            this.colPlannedDuration.HeaderText = "计划工期";
            this.colPlannedDuration.Name = "colPlannedDuration";
            this.colPlannedDuration.Width = 80;
            // 
            // colOBSService
            // 
            this.colOBSService.HeaderText = "承担者";
            this.colOBSService.Name = "colOBSService";
            this.colOBSService.ToolTipText = "双击单元格选择任务承担者";
            this.colOBSService.Width = 150;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            // 
            // colPBSTree
            // 
            this.colPBSTree.HeaderText = "工区";
            this.colPBSTree.Name = "colPBSTree";
            this.colPBSTree.Visible = false;
            // 
            // colForwardBillDtlIdOfWeek
            // 
            this.colForwardBillDtlIdOfWeek.HeaderText = "前驱明细Id";
            this.colForwardBillDtlIdOfWeek.Name = "colForwardBillDtlIdOfWeek";
            this.colForwardBillDtlIdOfWeek.Visible = false;
            // 
            // flexGrid1
            // 
            this.flexGrid1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flexGrid1.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid1.CheckedImage")));
            this.flexGrid1.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.flexGrid1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGrid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGrid1.Location = new System.Drawing.Point(98, 12);
            this.flexGrid1.Name = "flexGrid1";
            this.flexGrid1.Size = new System.Drawing.Size(473, 19);
            this.flexGrid1.TabIndex = 17;
            this.flexGrid1.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid1.UncheckedImage")));
            // 
            // VWeekScheduleSummary
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1047, 494);
            this.Name = "VWeekScheduleSummary";
            this.Text = "项目周计划维护";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlContent.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.spTop.ResumeLayout(false);
            this.pnlFloor.ResumeLayout(false);
            this.pnlWorkSpace.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            this.cmsDg.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpWeekPlan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCompletionAnalysis;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpWeekPlan;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid1;
        private System.Windows.Forms.ContextMenuStrip cmsDg;
        private System.Windows.Forms.ToolStripMenuItem tsmiDel;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnWeekSchedule;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeekPlanName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOwner;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBSTree;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMainTaskContent;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colMonday;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colTuesday;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colWednesday;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colThursday;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFriday;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSaturday;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSunday;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlannedDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOBSService;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPBSTree;
        private System.Windows.Forms.DataGridViewTextBoxColumn colForwardBillDtlIdOfWeek;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpBusinessDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
    }
}