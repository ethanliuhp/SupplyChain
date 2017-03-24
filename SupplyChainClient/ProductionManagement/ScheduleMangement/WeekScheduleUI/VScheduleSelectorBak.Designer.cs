namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    partial class VScheduleSelectorBak
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VScheduleSelectorBak));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGWBSName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cbPlanVersion = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cboSchedulePlanName = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtHandlePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.customLabel16 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel18 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPlanVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGWBSTreeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFigureprogress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlannedBeginDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlannedEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlannedDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualBeginDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlanDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDtlRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lnkNone = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.lnkAll = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.lblRecordTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.lblRecordTotal);
            this.pnlFloor.Controls.Add(this.lnkNone);
            this.pnlFloor.Controls.Add(this.lnkAll);
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(1004, 493);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lnkAll, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lnkNone, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecordTotal, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(127, 6);
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.Text = "滚动计划引用";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtGWBSName);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.cbPlanVersion);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Controls.Add(this.cboSchedulePlanName);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtHandlePerson);
            this.groupBox1.Controls.Add(this.customLabel16);
            this.groupBox1.Controls.Add(this.customLabel18);
            this.groupBox1.Location = new System.Drawing.Point(14, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(980, 65);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // txtGWBSName
            // 
            this.txtGWBSName.BackColor = System.Drawing.SystemColors.Control;
            this.txtGWBSName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtGWBSName.DrawSelf = false;
            this.txtGWBSName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtGWBSName.EnterToTab = false;
            this.txtGWBSName.Location = new System.Drawing.Point(422, 39);
            this.txtGWBSName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtGWBSName.Name = "txtGWBSName";
            this.txtGWBSName.Padding = new System.Windows.Forms.Padding(1);
            this.txtGWBSName.ReadOnly = false;
            this.txtGWBSName.Size = new System.Drawing.Size(288, 16);
            this.txtGWBSName.TabIndex = 99;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(342, 41);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(83, 12);
            this.customLabel3.TabIndex = 100;
            this.customLabel3.Text = "项目任务名称:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cbPlanVersion
            // 
            this.cbPlanVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlanVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbPlanVersion.FormattingEnabled = true;
            this.cbPlanVersion.Location = new System.Drawing.Point(422, 12);
            this.cbPlanVersion.Name = "cbPlanVersion";
            this.cbPlanVersion.Size = new System.Drawing.Size(146, 20);
            this.cbPlanVersion.TabIndex = 98;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(364, 17);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 97;
            this.customLabel2.Text = "计划版本:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(12, 42);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(83, 12);
            this.customLabel1.TabIndex = 95;
            this.customLabel1.Text = "计划开始日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(95, 38);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 93;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(227, 38);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 94;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(210, 44);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 96;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cboSchedulePlanName
            // 
            this.cboSchedulePlanName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSchedulePlanName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboSchedulePlanName.FormattingEnabled = true;
            this.cboSchedulePlanName.Location = new System.Drawing.Point(95, 12);
            this.cboSchedulePlanName.Name = "cboSchedulePlanName";
            this.cboSchedulePlanName.Size = new System.Drawing.Size(241, 20);
            this.cboSchedulePlanName.TabIndex = 92;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(723, 36);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(94, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtHandlePerson
            // 
            this.txtHandlePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtHandlePerson.Code = null;
            this.txtHandlePerson.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtHandlePerson.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtHandlePerson.EnterToTab = false;
            this.txtHandlePerson.Id = "";
            this.txtHandlePerson.IsAllLoad = true;
            this.txtHandlePerson.Location = new System.Drawing.Point(620, 11);
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Result = ((System.Collections.IList)(resources.GetObject("txtHandlePerson.Result")));
            this.txtHandlePerson.RightMouse = false;
            this.txtHandlePerson.Size = new System.Drawing.Size(90, 21);
            this.txtHandlePerson.TabIndex = 8;
            this.txtHandlePerson.Tag = null;
            this.txtHandlePerson.Value = "";
            this.txtHandlePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel16
            // 
            this.customLabel16.AutoSize = true;
            this.customLabel16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel16.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel16.Location = new System.Drawing.Point(574, 15);
            this.customLabel16.Name = "customLabel16";
            this.customLabel16.Size = new System.Drawing.Size(47, 12);
            this.customLabel16.TabIndex = 86;
            this.customLabel16.Text = "责任人:";
            this.customLabel16.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel18
            // 
            this.customLabel18.AutoSize = true;
            this.customLabel18.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel18.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel18.Location = new System.Drawing.Point(37, 17);
            this.customLabel18.Name = "customLabel18";
            this.customLabel18.Size = new System.Drawing.Size(59, 12);
            this.customLabel18.TabIndex = 53;
            this.customLabel18.Text = "计划名称:";
            this.customLabel18.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.AllowUserToResizeRows = false;
            this.dgDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colPlanVersion,
            this.colGWBSTreeName,
            this.colFigureprogress,
            this.colPlannedBeginDate,
            this.colPlannedEndDate,
            this.colPlannedDuration,
            this.colActualBeginDate,
            this.colActualEndDate,
            this.colActualDuration,
            this.colUnit,
            this.colPlanDesc,
            this.colDtlRemark});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(12, 77);
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
            this.dgDetail.Size = new System.Drawing.Size(980, 384);
            this.dgDetail.TabIndex = 4;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 53;
            // 
            // colPlanVersion
            // 
            this.colPlanVersion.HeaderText = "计划版本";
            this.colPlanVersion.Name = "colPlanVersion";
            this.colPlanVersion.ReadOnly = true;
            this.colPlanVersion.Width = 77;
            // 
            // colGWBSTreeName
            // 
            this.colGWBSTreeName.HeaderText = "任务名称";
            this.colGWBSTreeName.Name = "colGWBSTreeName";
            this.colGWBSTreeName.ReadOnly = true;
            this.colGWBSTreeName.Width = 77;
            // 
            // colFigureprogress
            // 
            this.colFigureprogress.HeaderText = "累计形象进度(%)";
            this.colFigureprogress.Name = "colFigureprogress";
            this.colFigureprogress.ReadOnly = true;
            this.colFigureprogress.Width = 119;
            // 
            // colPlannedBeginDate
            // 
            this.colPlannedBeginDate.HeaderText = "计划开始时间";
            this.colPlannedBeginDate.Name = "colPlannedBeginDate";
            this.colPlannedBeginDate.ReadOnly = true;
            this.colPlannedBeginDate.Width = 101;
            // 
            // colPlannedEndDate
            // 
            this.colPlannedEndDate.HeaderText = "计划结束时间";
            this.colPlannedEndDate.Name = "colPlannedEndDate";
            this.colPlannedEndDate.ReadOnly = true;
            this.colPlannedEndDate.Width = 101;
            // 
            // colPlannedDuration
            // 
            this.colPlannedDuration.HeaderText = "计划工期";
            this.colPlannedDuration.Name = "colPlannedDuration";
            this.colPlannedDuration.ReadOnly = true;
            this.colPlannedDuration.Width = 77;
            // 
            // colActualBeginDate
            // 
            this.colActualBeginDate.HeaderText = "实际开始时间";
            this.colActualBeginDate.Name = "colActualBeginDate";
            this.colActualBeginDate.ReadOnly = true;
            this.colActualBeginDate.Width = 101;
            // 
            // colActualEndDate
            // 
            this.colActualEndDate.HeaderText = "实际结束时间";
            this.colActualEndDate.Name = "colActualEndDate";
            this.colActualEndDate.ReadOnly = true;
            this.colActualEndDate.Width = 101;
            // 
            // colActualDuration
            // 
            this.colActualDuration.HeaderText = "实际工期";
            this.colActualDuration.Name = "colActualDuration";
            this.colActualDuration.ReadOnly = true;
            this.colActualDuration.Width = 77;
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            this.colUnit.Width = 77;
            // 
            // colPlanDesc
            // 
            this.colPlanDesc.HeaderText = "计划说明";
            this.colPlanDesc.Name = "colPlanDesc";
            this.colPlanDesc.ReadOnly = true;
            this.colPlanDesc.Width = 77;
            // 
            // colDtlRemark
            // 
            this.colDtlRemark.HeaderText = "备注";
            this.colDtlRemark.Name = "colDtlRemark";
            this.colDtlRemark.ReadOnly = true;
            this.colDtlRemark.Width = 53;
            // 
            // lnkNone
            // 
            this.lnkNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkNone.AutoSize = true;
            this.lnkNone.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkNone.Location = new System.Drawing.Point(49, 467);
            this.lnkNone.Name = "lnkNone";
            this.lnkNone.Size = new System.Drawing.Size(29, 12);
            this.lnkNone.TabIndex = 25;
            this.lnkNone.TabStop = true;
            this.lnkNone.Text = "反选";
            // 
            // lnkAll
            // 
            this.lnkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkAll.AutoSize = true;
            this.lnkAll.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkAll.Location = new System.Drawing.Point(12, 467);
            this.lnkAll.Name = "lnkAll";
            this.lnkAll.Size = new System.Drawing.Size(29, 12);
            this.lnkAll.TabIndex = 24;
            this.lnkAll.TabStop = true;
            this.lnkAll.Text = "全选";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(737, 465);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(122, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(590, 465);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(125, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblRecordTotal
            // 
            this.lblRecordTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecordTotal.AutoSize = true;
            this.lblRecordTotal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecordTotal.ForeColor = System.Drawing.Color.Red;
            this.lblRecordTotal.Location = new System.Drawing.Point(302, 470);
            this.lblRecordTotal.Name = "lblRecordTotal";
            this.lblRecordTotal.Size = new System.Drawing.Size(107, 12);
            this.lblRecordTotal.TabIndex = 100;
            this.lblRecordTotal.Text = "共选择【0】条记录";
            this.lblRecordTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VScheduleSelectorBak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 493);
            this.Name = "VScheduleSelectorBak";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "滚动计划引用";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtHandlePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel16;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel18;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lnkNone;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lnkAll;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecordTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboSchedulePlanName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtGWBSName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cbPlanVersion;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlanVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBSTreeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFigureprogress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlannedBeginDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlannedEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlannedDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualBeginDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualDuration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlanDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtlRemark;

    }
}