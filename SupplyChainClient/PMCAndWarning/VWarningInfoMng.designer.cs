namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    partial class VWarningInfoMng
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VWarningInfoMng));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnQuery = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridWarningInfo = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colWarnProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWarnLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWarnTargetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWarnContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWarnSubmitTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cbWarningLevel = new System.Windows.Forms.ComboBox();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSelectProject = new System.Windows.Forms.Button();
            this.btnSelectWarningTarget = new System.Windows.Forms.Button();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtWarningTargetName = new System.Windows.Forms.TextBox();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtSubmitStartTime = new System.Windows.Forms.DateTimePicker();
            this.dtSubmitEndTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cbState = new System.Windows.Forms.ComboBox();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridWarningInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.txtWarningTargetName);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.dtSubmitEndTime);
            this.pnlFloor.Controls.Add(this.dtSubmitStartTime);
            this.pnlFloor.Controls.Add(this.cbState);
            this.pnlFloor.Controls.Add(this.cbWarningLevel);
            this.pnlFloor.Controls.Add(this.customLabel4);
            this.pnlFloor.Controls.Add(this.customLabel6);
            this.pnlFloor.Controls.Add(this.txtProjectName);
            this.pnlFloor.Controls.Add(this.customLabel2);
            this.pnlFloor.Controls.Add(this.customLabel3);
            this.pnlFloor.Controls.Add(this.btnSelectWarningTarget);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.btnSelectProject);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Size = new System.Drawing.Size(908, 511);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSelectProject, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSelectWarningTarget, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel3, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtProjectName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel6, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel4, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cbWarningLevel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cbState, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtSubmitStartTime, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtSubmitEndTime, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtWarningTargetName, 0);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(688, 37);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 23);
            this.btnQuery.TabIndex = 165;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gridWarningInfo);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Location = new System.Drawing.Point(3, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(902, 440);
            this.groupBox1.TabIndex = 167;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "预警信息";
            // 
            // gridWarningInfo
            // 
            this.gridWarningInfo.AddDefaultMenu = false;
            this.gridWarningInfo.AddNoColumn = true;
            this.gridWarningInfo.AllowUserToAddRows = false;
            this.gridWarningInfo.AllowUserToDeleteRows = false;
            this.gridWarningInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridWarningInfo.BackgroundColor = System.Drawing.Color.White;
            this.gridWarningInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridWarningInfo.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridWarningInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridWarningInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridWarningInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWarnProjectName,
            this.colWarnLevel,
            this.colWarnTargetName,
            this.colState,
            this.colWarnContent,
            this.colWarnSubmitTime});
            this.gridWarningInfo.CustomBackColor = false;
            this.gridWarningInfo.EditCellBackColor = System.Drawing.Color.White;
            this.gridWarningInfo.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridWarningInfo.FreezeFirstRow = false;
            this.gridWarningInfo.FreezeLastRow = false;
            this.gridWarningInfo.FrontColumnCount = 0;
            this.gridWarningInfo.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridWarningInfo.HScrollOffset = 0;
            this.gridWarningInfo.IsAllowOrder = false;
            this.gridWarningInfo.IsConfirmDelete = true;
            this.gridWarningInfo.Location = new System.Drawing.Point(6, 20);
            this.gridWarningInfo.Name = "gridWarningInfo";
            this.gridWarningInfo.PageIndex = 0;
            this.gridWarningInfo.PageSize = 0;
            this.gridWarningInfo.Query = null;
            this.gridWarningInfo.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridWarningInfo.ReadOnlyCols")));
            this.gridWarningInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridWarningInfo.RowHeadersWidth = 22;
            this.gridWarningInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridWarningInfo.RowTemplate.Height = 23;
            this.gridWarningInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridWarningInfo.Size = new System.Drawing.Size(890, 385);
            this.gridWarningInfo.TabIndex = 178;
            this.gridWarningInfo.TargetType = null;
            this.gridWarningInfo.VScrollOffset = 0;
            // 
            // colWarnProjectName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.colWarnProjectName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colWarnProjectName.HeaderText = "所属项目";
            this.colWarnProjectName.Name = "colWarnProjectName";
            this.colWarnProjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colWarnLevel
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            this.colWarnLevel.DefaultCellStyle = dataGridViewCellStyle2;
            this.colWarnLevel.HeaderText = "预警级别";
            this.colWarnLevel.Name = "colWarnLevel";
            this.colWarnLevel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colWarnTargetName
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.colWarnTargetName.DefaultCellStyle = dataGridViewCellStyle3;
            this.colWarnTargetName.HeaderText = "预警指标";
            this.colWarnTargetName.Name = "colWarnTargetName";
            this.colWarnTargetName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colState
            // 
            this.colState.HeaderText = "状态";
            this.colState.Name = "colState";
            this.colState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colState.Width = 55;
            // 
            // colWarnContent
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.colWarnContent.DefaultCellStyle = dataGridViewCellStyle4;
            this.colWarnContent.HeaderText = "预警信息";
            this.colWarnContent.Name = "colWarnContent";
            this.colWarnContent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colWarnContent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colWarnContent.Width = 350;
            // 
            // colWarnSubmitTime
            // 
            this.colWarnSubmitTime.HeaderText = "预警信息提报时间";
            this.colWarnSubmitTime.Name = "colWarnSubmitTime";
            this.colWarnSubmitTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colWarnSubmitTime.Width = 130;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(571, 411);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(132, 23);
            this.btnDelete.TabIndex = 163;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(82, 8);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(208, 21);
            this.txtProjectName.TabIndex = 169;
            // 
            // customLabel3
            // 
            this.customLabel3.AddColonAuto = true;
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(21, 13);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(65, 12);
            this.customLabel3.TabIndex = 168;
            this.customLabel3.Text = "所属项目：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cbWarningLevel
            // 
            this.cbWarningLevel.FormattingEnabled = true;
            this.cbWarningLevel.Location = new System.Drawing.Point(236, 38);
            this.cbWarningLevel.Name = "cbWarningLevel";
            this.cbWarningLevel.Size = new System.Drawing.Size(114, 20);
            this.cbWarningLevel.TabIndex = 171;
            // 
            // customLabel6
            // 
            this.customLabel6.AddColonAuto = true;
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(175, 43);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(65, 12);
            this.customLabel6.TabIndex = 170;
            this.customLabel6.Text = "预警级别：";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSelectProject
            // 
            this.btnSelectProject.Location = new System.Drawing.Point(291, 7);
            this.btnSelectProject.Name = "btnSelectProject";
            this.btnSelectProject.Size = new System.Drawing.Size(56, 23);
            this.btnSelectProject.TabIndex = 165;
            this.btnSelectProject.Text = "选择";
            this.btnSelectProject.UseVisualStyleBackColor = true;
            // 
            // btnSelectWarningTarget
            // 
            this.btnSelectWarningTarget.Location = new System.Drawing.Point(719, 7);
            this.btnSelectWarningTarget.Name = "btnSelectWarningTarget";
            this.btnSelectWarningTarget.Size = new System.Drawing.Size(59, 23);
            this.btnSelectWarningTarget.TabIndex = 165;
            this.btnSelectWarningTarget.Text = "选择";
            this.btnSelectWarningTarget.UseVisualStyleBackColor = true;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(353, 13);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 168;
            this.customLabel1.Text = "预警指标：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtWarningTargetName
            // 
            this.txtWarningTargetName.Location = new System.Drawing.Point(414, 8);
            this.txtWarningTargetName.Name = "txtWarningTargetName";
            this.txtWarningTargetName.Size = new System.Drawing.Size(304, 21);
            this.txtWarningTargetName.TabIndex = 169;
            // 
            // customLabel2
            // 
            this.customLabel2.AddColonAuto = true;
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(356, 43);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(65, 12);
            this.customLabel2.TabIndex = 168;
            this.customLabel2.Text = "提报时间：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtSubmitStartTime
            // 
            this.dtSubmitStartTime.Location = new System.Drawing.Point(416, 38);
            this.dtSubmitStartTime.Name = "dtSubmitStartTime";
            this.dtSubmitStartTime.Size = new System.Drawing.Size(116, 21);
            this.dtSubmitStartTime.TabIndex = 172;
            // 
            // dtSubmitEndTime
            // 
            this.dtSubmitEndTime.Location = new System.Drawing.Point(556, 39);
            this.dtSubmitEndTime.Name = "dtSubmitEndTime";
            this.dtSubmitEndTime.Size = new System.Drawing.Size(116, 21);
            this.dtSubmitEndTime.TabIndex = 172;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(539, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 173;
            this.label1.Text = "-";
            // 
            // customLabel4
            // 
            this.customLabel4.AddColonAuto = true;
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(45, 42);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(41, 12);
            this.customLabel4.TabIndex = 170;
            this.customLabel4.Text = "状态：";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cbState
            // 
            this.cbState.FormattingEnabled = true;
            this.cbState.Location = new System.Drawing.Point(82, 38);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(87, 20);
            this.cbState.TabIndex = 171;
            // 
            // VWarningInfoMng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(908, 511);
            this.Name = "VWarningInfoMng";
            this.Text = "状态检查动作及指标维护";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridWarningInfo)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtProjectName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridWarningInfo;
        private System.Windows.Forms.ComboBox cbWarningLevel;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtSubmitEndTime;
        private System.Windows.Forms.DateTimePicker dtSubmitStartTime;
        private System.Windows.Forms.TextBox txtWarningTargetName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.Button btnSelectWarningTarget;
        private System.Windows.Forms.Button btnSelectProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWarnProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWarnLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWarnTargetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWarnContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWarnSubmitTime;
        private System.Windows.Forms.ComboBox cbState;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
	}
}