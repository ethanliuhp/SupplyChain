namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    partial class VWeekAssign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VWeekAssign));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cboSchedulePlanName = new System.Windows.Forms.ComboBox();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpCreatDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtPlanName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSetSelRowsDate = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dtpSetDateTime = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.flexGrid1 = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colGWBSTree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGWBSDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlanBeginDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.colPlanEndDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.colPlanWorkDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualBenginDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.colActualEndDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.colActualWorkDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAssWorkDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbTeam = new System.Windows.Forms.ComboBox();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnSendMsg = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.comDocState = new System.Windows.Forms.ComboBox();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbMsgState = new System.Windows.Forms.ComboBox();
            this.pnlBody.SuspendLayout();
            this.pnlFloor.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl1);
            this.pnlBody.Location = new System.Drawing.Point(0, 115);
            this.pnlBody.Size = new System.Drawing.Size(1017, 339);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Location = new System.Drawing.Point(0, 454);
            this.pnlFooter.Size = new System.Drawing.Size(1017, 38);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(1017, 0);
            // 
            // pnlFloor
            // 
            this.pnlFloor.Size = new System.Drawing.Size(1017, 492);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.customLabel8);
            this.pnlHeader.Controls.Add(this.cmbMsgState);
            this.pnlHeader.Controls.Add(this.customLabel6);
            this.pnlHeader.Controls.Add(this.comDocState);
            this.pnlHeader.Controls.Add(this.btnSendMsg);
            this.pnlHeader.Controls.Add(this.customLabel1);
            this.pnlHeader.Controls.Add(this.txtCreatePerson);
            this.pnlHeader.Controls.Add(this.txtRemark);
            this.pnlHeader.Controls.Add(this.customLabel9);
            this.pnlHeader.Controls.Add(this.txtPlanName);
            this.pnlHeader.Controls.Add(this.customLabel2);
            this.pnlHeader.Controls.Add(this.dtpCreatDate);
            this.pnlHeader.Controls.Add(this.customLabel3);
            this.pnlHeader.Controls.Add(this.customLabel4);
            this.pnlHeader.Controls.Add(this.cmbTeam);
            this.pnlHeader.Controls.Add(this.cboSchedulePlanName);
            this.pnlHeader.Controls.Add(this.customLabel7);
            this.pnlHeader.Size = new System.Drawing.Size(1017, 115);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel7, 0);
            this.pnlHeader.Controls.SetChildIndex(this.cboSchedulePlanName, 0);
            this.pnlHeader.Controls.SetChildIndex(this.cmbTeam, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel4, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel3, 0);
            this.pnlHeader.Controls.SetChildIndex(this.dtpCreatDate, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel2, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtPlanName, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel9, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtRemark, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtCreatePerson, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlHeader.Controls.SetChildIndex(this.btnSendMsg, 0);
            this.pnlHeader.Controls.SetChildIndex(this.comDocState, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel6, 0);
            this.pnlHeader.Controls.SetChildIndex(this.cmbMsgState, 0);
            this.pnlHeader.Controls.SetChildIndex(this.customLabel8, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(299, 5);
            this.lblTitle.Visible = false;
            // 
            // cboSchedulePlanName
            // 
            this.cboSchedulePlanName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSchedulePlanName.FormattingEnabled = true;
            this.cboSchedulePlanName.Location = new System.Drawing.Point(82, 7);
            this.cboSchedulePlanName.Name = "cboSchedulePlanName";
            this.cboSchedulePlanName.Size = new System.Drawing.Size(457, 20);
            this.cboSchedulePlanName.TabIndex = 257;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(10, 11);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(71, 12);
            this.customLabel7.TabIndex = 256;
            this.customLabel7.Text = "周进度计划:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpCreatDate
            // 
            this.dtpCreatDate.Enabled = false;
            this.dtpCreatDate.Location = new System.Drawing.Point(859, 34);
            this.dtpCreatDate.Name = "dtpCreatDate";
            this.dtpCreatDate.Size = new System.Drawing.Size(128, 21);
            this.dtpCreatDate.TabIndex = 258;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(798, 38);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 260;
            this.customLabel4.Text = "任务日期:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(82, 59);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(905, 20);
            this.txtRemark.TabIndex = 264;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(10, 63);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(71, 12);
            this.customLabel9.TabIndex = 265;
            this.customLabel9.Text = "任务单说明:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtPlanName
            // 
            this.txtPlanName.BackColor = System.Drawing.SystemColors.Control;
            this.txtPlanName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPlanName.DrawSelf = false;
            this.txtPlanName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPlanName.EnterToTab = false;
            this.txtPlanName.Location = new System.Drawing.Point(82, 34);
            this.txtPlanName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPlanName.Name = "txtPlanName";
            this.txtPlanName.Padding = new System.Windows.Forms.Padding(1);
            this.txtPlanName.ReadOnly = true;
            this.txtPlanName.Size = new System.Drawing.Size(457, 20);
            this.txtPlanName.TabIndex = 262;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(10, 38);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(71, 12);
            this.customLabel2.TabIndex = 263;
            this.customLabel2.Text = "任务单名称:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1005, 339);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.customLabel5);
            this.tabPage1.Controls.Add(this.btnSetSelRowsDate);
            this.tabPage1.Controls.Add(this.dtpSetDateTime);
            this.tabPage1.Controls.Add(this.flexGrid1);
            this.tabPage1.Controls.Add(this.dgDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(997, 313);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "任务单明细";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(70, 8);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(209, 12);
            this.customLabel5.TabIndex = 266;
            this.customLabel5.Text = "将选中行的实际开始时间统一设置为：";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSetSelRowsDate
            // 
            this.btnSetSelRowsDate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSetSelRowsDate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSetSelRowsDate.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSetSelRowsDate.Location = new System.Drawing.Point(428, 3);
            this.btnSetSelRowsDate.Name = "btnSetSelRowsDate";
            this.btnSetSelRowsDate.Size = new System.Drawing.Size(67, 23);
            this.btnSetSelRowsDate.TabIndex = 260;
            this.btnSetSelRowsDate.Text = "设置";
            this.btnSetSelRowsDate.UseVisualStyleBackColor = true;
            // 
            // dtpSetDateTime
            // 
            this.dtpSetDateTime.Enabled = false;
            this.dtpSetDateTime.Location = new System.Drawing.Point(281, 4);
            this.dtpSetDateTime.Name = "dtpSetDateTime";
            this.dtpSetDateTime.Size = new System.Drawing.Size(128, 21);
            this.dtpSetDateTime.TabIndex = 259;
            // 
            // flexGrid1
            // 
            this.flexGrid1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flexGrid1.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid1.CheckedImage")));
            this.flexGrid1.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.flexGrid1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGrid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGrid1.Location = new System.Drawing.Point(374, 170);
            this.flexGrid1.Name = "flexGrid1";
            this.flexGrid1.Size = new System.Drawing.Size(473, 19);
            this.flexGrid1.TabIndex = 19;
            this.flexGrid1.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid1.UncheckedImage")));
            this.flexGrid1.Visible = false;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = true;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGWBSTree,
            this.colGWBSDetail,
            this.colPlanBeginDate,
            this.colPlanEndDate,
            this.colPlanWorkDays,
            this.colActualBenginDate,
            this.colActualEndDate,
            this.colActualWorkDays,
            this.colAssWorkDesc});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(3, 28);
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
            this.dgDetail.Size = new System.Drawing.Size(991, 282);
            this.dgDetail.TabIndex = 8;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colGWBSTree
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.colGWBSTree.DefaultCellStyle = dataGridViewCellStyle7;
            this.colGWBSTree.HeaderText = "任务名称";
            this.colGWBSTree.Name = "colGWBSTree";
            this.colGWBSTree.ReadOnly = true;
            this.colGWBSTree.Width = 150;
            // 
            // colGWBSDetail
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            this.colGWBSDetail.DefaultCellStyle = dataGridViewCellStyle8;
            this.colGWBSDetail.FillWeight = 80F;
            this.colGWBSDetail.HeaderText = "任务明细";
            this.colGWBSDetail.Name = "colGWBSDetail";
            this.colGWBSDetail.ReadOnly = true;
            this.colGWBSDetail.Width = 120;
            // 
            // colPlanBeginDate
            // 
            this.colPlanBeginDate.HeaderText = "计划开始时间";
            this.colPlanBeginDate.Name = "colPlanBeginDate";
            this.colPlanBeginDate.ReadOnly = true;
            this.colPlanBeginDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPlanBeginDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colPlanEndDate
            // 
            this.colPlanEndDate.HeaderText = "计划结束时间";
            this.colPlanEndDate.Name = "colPlanEndDate";
            this.colPlanEndDate.ReadOnly = true;
            this.colPlanEndDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPlanEndDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colPlanWorkDays
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            this.colPlanWorkDays.DefaultCellStyle = dataGridViewCellStyle9;
            this.colPlanWorkDays.FillWeight = 80F;
            this.colPlanWorkDays.HeaderText = "计划工期";
            this.colPlanWorkDays.Name = "colPlanWorkDays";
            this.colPlanWorkDays.ReadOnly = true;
            this.colPlanWorkDays.Width = 80;
            // 
            // colActualBenginDate
            // 
            this.colActualBenginDate.HeaderText = "实际开始时间";
            this.colActualBenginDate.Name = "colActualBenginDate";
            this.colActualBenginDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colActualBenginDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colActualEndDate
            // 
            this.colActualEndDate.HeaderText = "实际结束时间";
            this.colActualEndDate.Name = "colActualEndDate";
            this.colActualEndDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colActualEndDate.Visible = false;
            // 
            // colActualWorkDays
            // 
            this.colActualWorkDays.HeaderText = "实际工期";
            this.colActualWorkDays.Name = "colActualWorkDays";
            this.colActualWorkDays.Visible = false;
            // 
            // colAssWorkDesc
            // 
            this.colAssWorkDesc.HeaderText = "任务说明";
            this.colAssWorkDesc.Name = "colAssWorkDesc";
            this.colAssWorkDesc.Width = 300;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(545, 11);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 267;
            this.customLabel1.Text = "任务队伍:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbTeam
            // 
            this.cmbTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeam.FormattingEnabled = true;
            this.cmbTeam.Location = new System.Drawing.Point(613, 7);
            this.cmbTeam.Name = "cmbTeam";
            this.cmbTeam.Size = new System.Drawing.Size(374, 20);
            this.cmbTeam.TabIndex = 257;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(545, 38);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 260;
            this.customLabel3.Text = "派 工 人:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(613, 34);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = true;
            this.txtCreatePerson.Size = new System.Drawing.Size(179, 20);
            this.txtCreatePerson.TabIndex = 264;
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSendMsg.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSendMsg.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSendMsg.Location = new System.Drawing.Point(459, 85);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(80, 23);
            this.btnSendMsg.TabIndex = 268;
            this.btnSendMsg.Text = "短信通知";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(10, 89);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 270;
            this.customLabel6.Text = "单据状态:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // comDocState
            // 
            this.comDocState.FormattingEnabled = true;
            this.comDocState.Location = new System.Drawing.Point(82, 85);
            this.comDocState.Name = "comDocState";
            this.comDocState.Size = new System.Drawing.Size(129, 20);
            this.comDocState.TabIndex = 269;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(225, 89);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(83, 12);
            this.customLabel8.TabIndex = 272;
            this.customLabel8.Text = "短信通知状态:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbMsgState
            // 
            this.cmbMsgState.FormattingEnabled = true;
            this.cmbMsgState.Location = new System.Drawing.Point(314, 85);
            this.cmbMsgState.Name = "cmbMsgState";
            this.cmbMsgState.Size = new System.Drawing.Size(129, 20);
            this.cmbMsgState.TabIndex = 271;
            // 
            // VWeekAssign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 492);
            this.Name = "VWeekAssign";
            this.Text = "任务单维护";
            this.pnlBody.ResumeLayout(false);
            this.pnlFloor.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSchedulePlanName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpCreatDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPlanName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBSTree;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBSDetail;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colPlanBeginDate;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colPlanEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlanWorkDays;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colActualBenginDate;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colActualEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualWorkDays;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAssWorkDesc;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.ComboBox cmbTeam;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpSetDateTime;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSetSelRowsDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSendMsg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private System.Windows.Forms.ComboBox comDocState;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private System.Windows.Forms.ComboBox cmbMsgState;
    }
}