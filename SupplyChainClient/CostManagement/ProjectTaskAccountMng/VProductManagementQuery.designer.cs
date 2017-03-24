namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    partial class VProductManagementQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VProductManagementQuery));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gridDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBalOrg = new System.Windows.Forms.TextBox();
            this.btnSelectBalOrg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOwner = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.dtAccountEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnSelectAccountTaskRootNode = new System.Windows.Forms.Button();
            this.dtAccountStartTime = new System.Windows.Forms.DateTimePicker();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtAccountRootNode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblConfirmQnyCount = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblAccountQnyCount = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.DtlProjectTaskNode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlIsAccount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlAccountTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlConfirmTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlCostItemQuotaCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlProjectTaskDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlTaskBearer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlOwner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlMatFeeBalanceFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlConfirmProjectAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlAccountProjectAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlMainResourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlMainResourceSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlDigramNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOwner)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Size = new System.Drawing.Size(1051, 426);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1051, 426);
            this.panel1.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 80);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1051, 346);
            this.panel3.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblAccountQnyCount);
            this.groupBox2.Controls.Add(this.lblConfirmQnyCount);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.gridDetail);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1051, 346);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "工程量提报信息";
            // 
            // gridDetail
            // 
            this.gridDetail.AddDefaultMenu = false;
            this.gridDetail.AddNoColumn = true;
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDetail.BackgroundColor = System.Drawing.Color.White;
            this.gridDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DtlProjectTaskNode,
            this.DtlIsAccount,
            this.DtlAccountTime,
            this.DtlConfirmTime,
            this.DtlCostItemQuotaCode,
            this.DtlProjectTaskDetail,
            this.DtlTaskBearer,
            this.DtlOwner,
            this.DtlMatFeeBalanceFlag,
            this.DtlConfirmProjectAmount,
            this.DtlAccountProjectAmount,
            this.DtlMainResourceName,
            this.DtlMainResourceSpec,
            this.DtlDigramNumber});
            this.gridDetail.CustomBackColor = false;
            this.gridDetail.EditCellBackColor = System.Drawing.Color.White;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridDetail.FreezeFirstRow = false;
            this.gridDetail.FreezeLastRow = false;
            this.gridDetail.FrontColumnCount = 0;
            this.gridDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridDetail.HScrollOffset = 0;
            this.gridDetail.IsAllowOrder = true;
            this.gridDetail.IsConfirmDelete = true;
            this.gridDetail.Location = new System.Drawing.Point(3, 17);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.PageIndex = 0;
            this.gridDetail.PageSize = 0;
            this.gridDetail.Query = null;
            this.gridDetail.ReadOnly = true;
            this.gridDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridDetail.ReadOnlyCols")));
            this.gridDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDetail.RowHeadersWidth = 22;
            this.gridDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridDetail.RowTemplate.Height = 23;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(1045, 305);
            this.gridDetail.TabIndex = 48;
            this.gridDetail.TargetType = null;
            this.gridDetail.VScrollOffset = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1051, 80);
            this.panel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtBalOrg);
            this.groupBox1.Controls.Add(this.btnSelectBalOrg);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtOwner);
            this.groupBox1.Controls.Add(this.dtAccountEndDate);
            this.groupBox1.Controls.Add(this.btnSelectAccountTaskRootNode);
            this.groupBox1.Controls.Add(this.dtAccountStartTime);
            this.groupBox1.Controls.Add(this.btnExportExcel);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.txtAccountRootNode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1045, 73);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询";
            // 
            // txtBalOrg
            // 
            this.txtBalOrg.Location = new System.Drawing.Point(96, 44);
            this.txtBalOrg.Name = "txtBalOrg";
            this.txtBalOrg.Size = new System.Drawing.Size(326, 21);
            this.txtBalOrg.TabIndex = 93;
            // 
            // btnSelectBalOrg
            // 
            this.btnSelectBalOrg.Location = new System.Drawing.Point(421, 43);
            this.btnSelectBalOrg.Name = "btnSelectBalOrg";
            this.btnSelectBalOrg.Size = new System.Drawing.Size(44, 23);
            this.btnSelectBalOrg.TabIndex = 92;
            this.btnSelectBalOrg.Text = "选择";
            this.btnSelectBalOrg.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 91;
            this.label1.Text = "承担者(队伍)：";
            // 
            // txtOwner
            // 
            this.txtOwner.BackColor = System.Drawing.SystemColors.Control;
            this.txtOwner.Code = null;
            this.txtOwner.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtOwner.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtOwner.EnterToTab = false;
            this.txtOwner.Id = "";
            this.txtOwner.IsAllLoad = true;
            this.txtOwner.Location = new System.Drawing.Point(508, 44);
            this.txtOwner.Name = "txtOwner";
            this.txtOwner.Result = ((System.Collections.IList)(resources.GetObject("txtOwner.Result")));
            this.txtOwner.RightMouse = false;
            this.txtOwner.Size = new System.Drawing.Size(90, 21);
            this.txtOwner.TabIndex = 90;
            this.txtOwner.Tag = null;
            this.txtOwner.Value = "";
            this.txtOwner.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // dtAccountEndDate
            // 
            this.dtAccountEndDate.Location = new System.Drawing.Point(199, 17);
            this.dtAccountEndDate.Name = "dtAccountEndDate";
            this.dtAccountEndDate.Size = new System.Drawing.Size(112, 21);
            this.dtAccountEndDate.TabIndex = 2;
            // 
            // btnSelectAccountTaskRootNode
            // 
            this.btnSelectAccountTaskRootNode.Location = new System.Drawing.Point(554, 17);
            this.btnSelectAccountTaskRootNode.Name = "btnSelectAccountTaskRootNode";
            this.btnSelectAccountTaskRootNode.Size = new System.Drawing.Size(44, 23);
            this.btnSelectAccountTaskRootNode.TabIndex = 1;
            this.btnSelectAccountTaskRootNode.Text = "选择";
            this.btnSelectAccountTaskRootNode.UseVisualStyleBackColor = true;
            // 
            // dtAccountStartTime
            // 
            this.dtAccountStartTime.Location = new System.Drawing.Point(72, 17);
            this.dtAccountStartTime.Name = "dtAccountStartTime";
            this.dtAccountStartTime.Size = new System.Drawing.Size(112, 21);
            this.dtAccountStartTime.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(604, 42);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 23);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // txtAccountRootNode
            // 
            this.txtAccountRootNode.Location = new System.Drawing.Point(378, 18);
            this.txtAccountRootNode.Name = "txtAccountRootNode";
            this.txtAccountRootNode.Size = new System.Drawing.Size(176, 21);
            this.txtAccountRootNode.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "开单时间：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(317, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "核算范围：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(471, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "工长：";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(535, 328);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 50;
            this.label4.Text = "累计确认工程量：";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(767, 328);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 51;
            this.label5.Text = "累计核算工程量：";
            // 
            // lblConfirmQnyCount
            // 
            this.lblConfirmQnyCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfirmQnyCount.BackColor = System.Drawing.SystemColors.Control;
            this.lblConfirmQnyCount.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.lblConfirmQnyCount.DrawSelf = false;
            this.lblConfirmQnyCount.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.lblConfirmQnyCount.EnterToTab = false;
            this.lblConfirmQnyCount.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfirmQnyCount.ForeColor = System.Drawing.Color.Blue;
            this.lblConfirmQnyCount.Location = new System.Drawing.Point(630, 325);
            this.lblConfirmQnyCount.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.lblConfirmQnyCount.Name = "lblConfirmQnyCount";
            this.lblConfirmQnyCount.Padding = new System.Windows.Forms.Padding(1);
            this.lblConfirmQnyCount.ReadOnly = true;
            this.lblConfirmQnyCount.Size = new System.Drawing.Size(131, 16);
            this.lblConfirmQnyCount.TabIndex = 149;
            // 
            // lblAccountQnyCount
            // 
            this.lblAccountQnyCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAccountQnyCount.BackColor = System.Drawing.SystemColors.Control;
            this.lblAccountQnyCount.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.lblAccountQnyCount.DrawSelf = false;
            this.lblAccountQnyCount.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.lblAccountQnyCount.EnterToTab = false;
            this.lblAccountQnyCount.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAccountQnyCount.ForeColor = System.Drawing.Color.Blue;
            this.lblAccountQnyCount.Location = new System.Drawing.Point(862, 325);
            this.lblAccountQnyCount.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.lblAccountQnyCount.Name = "lblAccountQnyCount";
            this.lblAccountQnyCount.Padding = new System.Windows.Forms.Padding(1);
            this.lblAccountQnyCount.ReadOnly = true;
            this.lblAccountQnyCount.Size = new System.Drawing.Size(131, 16);
            this.lblAccountQnyCount.TabIndex = 150;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(690, 42);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(80, 23);
            this.btnExportExcel.TabIndex = 2;
            this.btnExportExcel.Text = "导出Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // DtlProjectTaskNode
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.DtlProjectTaskNode.DefaultCellStyle = dataGridViewCellStyle1;
            this.DtlProjectTaskNode.HeaderText = "工程任务节点";
            this.DtlProjectTaskNode.Name = "DtlProjectTaskNode";
            this.DtlProjectTaskNode.ReadOnly = true;
            this.DtlProjectTaskNode.Width = 110;
            // 
            // DtlIsAccount
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.DtlIsAccount.DefaultCellStyle = dataGridViewCellStyle2;
            this.DtlIsAccount.HeaderText = "是否核算";
            this.DtlIsAccount.Name = "DtlIsAccount";
            this.DtlIsAccount.ReadOnly = true;
            this.DtlIsAccount.Width = 80;
            // 
            // DtlAccountTime
            // 
            this.DtlAccountTime.HeaderText = "核算时间";
            this.DtlAccountTime.Name = "DtlAccountTime";
            this.DtlAccountTime.ReadOnly = true;
            // 
            // DtlConfirmTime
            // 
            this.DtlConfirmTime.HeaderText = "提报时间";
            this.DtlConfirmTime.Name = "DtlConfirmTime";
            this.DtlConfirmTime.ReadOnly = true;
            // 
            // DtlCostItemQuotaCode
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCostItemQuotaCode.DefaultCellStyle = dataGridViewCellStyle3;
            this.DtlCostItemQuotaCode.HeaderText = "定额编号";
            this.DtlCostItemQuotaCode.Name = "DtlCostItemQuotaCode";
            this.DtlCostItemQuotaCode.ReadOnly = true;
            this.DtlCostItemQuotaCode.Width = 80;
            // 
            // DtlProjectTaskDetail
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.DtlProjectTaskDetail.DefaultCellStyle = dataGridViewCellStyle4;
            this.DtlProjectTaskDetail.FillWeight = 80F;
            this.DtlProjectTaskDetail.HeaderText = "工程任务明细";
            this.DtlProjectTaskDetail.Name = "DtlProjectTaskDetail";
            this.DtlProjectTaskDetail.ReadOnly = true;
            this.DtlProjectTaskDetail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DtlProjectTaskDetail.Width = 110;
            // 
            // DtlTaskBearer
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.DtlTaskBearer.DefaultCellStyle = dataGridViewCellStyle5;
            this.DtlTaskBearer.HeaderText = "承担者";
            this.DtlTaskBearer.Name = "DtlTaskBearer";
            this.DtlTaskBearer.ReadOnly = true;
            // 
            // DtlOwner
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.DtlOwner.DefaultCellStyle = dataGridViewCellStyle6;
            this.DtlOwner.HeaderText = "工长";
            this.DtlOwner.Name = "DtlOwner";
            this.DtlOwner.ReadOnly = true;
            this.DtlOwner.Width = 70;
            // 
            // DtlMatFeeBalanceFlag
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.DtlMatFeeBalanceFlag.DefaultCellStyle = dataGridViewCellStyle7;
            this.DtlMatFeeBalanceFlag.HeaderText = "料费结算";
            this.DtlMatFeeBalanceFlag.Name = "DtlMatFeeBalanceFlag";
            this.DtlMatFeeBalanceFlag.ReadOnly = true;
            this.DtlMatFeeBalanceFlag.Width = 60;
            // 
            // DtlConfirmProjectAmount
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            this.DtlConfirmProjectAmount.DefaultCellStyle = dataGridViewCellStyle8;
            this.DtlConfirmProjectAmount.HeaderText = "确认工程量";
            this.DtlConfirmProjectAmount.Name = "DtlConfirmProjectAmount";
            this.DtlConfirmProjectAmount.ReadOnly = true;
            this.DtlConfirmProjectAmount.Width = 70;
            // 
            // DtlAccountProjectAmount
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            this.DtlAccountProjectAmount.DefaultCellStyle = dataGridViewCellStyle9;
            this.DtlAccountProjectAmount.HeaderText = "核算工程量";
            this.DtlAccountProjectAmount.Name = "DtlAccountProjectAmount";
            this.DtlAccountProjectAmount.Width = 70;
            // 
            // DtlMainResourceName
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            this.DtlMainResourceName.DefaultCellStyle = dataGridViewCellStyle10;
            this.DtlMainResourceName.HeaderText = "主资源类型";
            this.DtlMainResourceName.Name = "DtlMainResourceName";
            this.DtlMainResourceName.ReadOnly = true;
            // 
            // DtlMainResourceSpec
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            this.DtlMainResourceSpec.DefaultCellStyle = dataGridViewCellStyle11;
            this.DtlMainResourceSpec.HeaderText = "规格型号";
            this.DtlMainResourceSpec.Name = "DtlMainResourceSpec";
            this.DtlMainResourceSpec.ReadOnly = true;
            this.DtlMainResourceSpec.Width = 80;
            // 
            // DtlDigramNumber
            // 
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            this.DtlDigramNumber.DefaultCellStyle = dataGridViewCellStyle12;
            this.DtlDigramNumber.HeaderText = "图号";
            this.DtlDigramNumber.Name = "DtlDigramNumber";
            this.DtlDigramNumber.ReadOnly = true;
            this.DtlDigramNumber.Width = 80;
            // 
            // VProductManagementQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(1051, 426);
            this.Name = "VProductManagementQuery";
            this.Text = "工程量提报查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOwner)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtOwner;
        private System.Windows.Forms.DateTimePicker dtAccountEndDate;
        private System.Windows.Forms.Button btnSelectAccountTaskRootNode;
        private System.Windows.Forms.DateTimePicker dtAccountStartTime;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.TextBox txtAccountRootNode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBalOrg;
        private System.Windows.Forms.Button btnSelectBalOrg;
        private System.Windows.Forms.Label label1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridDetail;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit lblConfirmQnyCount;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit lblAccountQnyCount;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlProjectTaskNode;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlIsAccount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlAccountTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlConfirmTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCostItemQuotaCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlProjectTaskDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlTaskBearer;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlOwner;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlMatFeeBalanceFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlConfirmProjectAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlAccountProjectAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlMainResourceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlMainResourceSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlDigramNumber;




    }
}