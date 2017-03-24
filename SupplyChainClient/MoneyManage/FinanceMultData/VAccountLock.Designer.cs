namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VAccountLock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VAccountLock));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblPSelect = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnLock = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnUnLock = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.chkOther = new System.Windows.Forms.CheckBox();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCheckProject = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRowNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectOrg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriodEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.chkHasLocked = new System.Windows.Forms.CheckBox();
            this.chkUnLocked = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ucAccountPeriodCombox1 = new Application.Business.Erp.SupplyChain.Client.Basic.CommonForm.Controls.UcAccountPeriodCombox();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.ucAccountPeriodCombox1);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.chkUnLocked);
            this.pnlFloor.Controls.Add(this.chkHasLocked);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Controls.Add(this.chkOther);
            this.pnlFloor.Controls.Add(this.chkAll);
            this.pnlFloor.Controls.Add(this.btnUnLock);
            this.pnlFloor.Controls.Add(this.btnLock);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Controls.Add(this.btnOperationOrg);
            this.pnlFloor.Controls.Add(this.txtOperationOrg);
            this.pnlFloor.Controls.Add(this.lblPSelect);
            this.pnlFloor.Size = new System.Drawing.Size(970, 457);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblPSelect, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnLock, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnUnLock, 0);
            this.pnlFloor.Controls.SetChildIndex(this.chkAll, 0);
            this.pnlFloor.Controls.SetChildIndex(this.chkOther, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.chkHasLocked, 0);
            this.pnlFloor.Controls.SetChildIndex(this.chkUnLocked, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.ucAccountPeriodCombox1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(497, 14);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 176;
            this.btnOperationOrg.Text = "选择";
            this.btnOperationOrg.UseVisualStyleBackColor = true;
            // 
            // txtOperationOrg
            // 
            this.txtOperationOrg.BackColor = System.Drawing.SystemColors.Control;
            this.txtOperationOrg.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOperationOrg.DrawSelf = false;
            this.txtOperationOrg.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOperationOrg.EnterToTab = false;
            this.txtOperationOrg.Location = new System.Drawing.Point(322, 17);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(169, 16);
            this.txtOperationOrg.TabIndex = 175;
            // 
            // lblPSelect
            // 
            this.lblPSelect.AutoSize = true;
            this.lblPSelect.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPSelect.Location = new System.Drawing.Point(261, 19);
            this.lblPSelect.Name = "lblPSelect";
            this.lblPSelect.Size = new System.Drawing.Size(59, 12);
            this.lblPSelect.TabIndex = 174;
            this.lblPSelect.Text = "范围选择:";
            this.lblPSelect.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(709, 11);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 28);
            this.btnQuery.TabIndex = 168;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // btnLock
            // 
            this.btnLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLock.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnLock.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLock.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnLock.Location = new System.Drawing.Point(376, 417);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(80, 28);
            this.btnLock.TabIndex = 179;
            this.btnLock.Text = "结账";
            this.btnLock.UseVisualStyleBackColor = true;
            // 
            // btnUnLock
            // 
            this.btnUnLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnLock.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnLock.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUnLock.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnUnLock.Location = new System.Drawing.Point(515, 417);
            this.btnUnLock.Name = "btnUnLock";
            this.btnUnLock.Size = new System.Drawing.Size(80, 28);
            this.btnUnLock.TabIndex = 180;
            this.btnUnLock.Text = "反结账";
            this.btnUnLock.UseVisualStyleBackColor = true;
            // 
            // chkAll
            // 
            this.chkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(25, 406);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(48, 16);
            this.chkAll.TabIndex = 181;
            this.chkAll.Text = "全选";
            this.chkAll.UseVisualStyleBackColor = true;
            // 
            // chkOther
            // 
            this.chkOther.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkOther.AutoSize = true;
            this.chkOther.Location = new System.Drawing.Point(79, 406);
            this.chkOther.Name = "chkOther";
            this.chkOther.Size = new System.Drawing.Size(48, 16);
            this.chkOther.TabIndex = 182;
            this.chkOther.Text = "反选";
            this.chkOther.UseVisualStyleBackColor = true;
            // 
            // dgMaster
            // 
            this.dgMaster.AddDefaultMenu = false;
            this.dgMaster.AddNoColumn = false;
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgMaster.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckProject,
            this.colRowNo,
            this.colProjectOrg,
            this.colProjectName,
            this.colPeriodEndDate,
            this.colState});
            this.dgMaster.CustomBackColor = false;
            this.dgMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMaster.EditCellBackColor = System.Drawing.Color.White;
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
            this.dgMaster.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgMaster.ReadOnlyCols")));
            this.dgMaster.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.RowHeadersWidth = 22;
            this.dgMaster.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMaster.RowTemplate.Height = 23;
            this.dgMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMaster.Size = new System.Drawing.Size(940, 334);
            this.dgMaster.TabIndex = 183;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colCheckProject
            // 
            this.colCheckProject.HeaderText = "";
            this.colCheckProject.MinimumWidth = 20;
            this.colCheckProject.Name = "colCheckProject";
            this.colCheckProject.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCheckProject.Width = 50;
            // 
            // colRowNo
            // 
            this.colRowNo.HeaderText = "序号";
            this.colRowNo.Name = "colRowNo";
            this.colRowNo.Width = 60;
            // 
            // colProjectOrg
            // 
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.colProjectOrg.DefaultCellStyle = dataGridViewCellStyle1;
            this.colProjectOrg.HeaderText = "项目归属组织";
            this.colProjectOrg.Name = "colProjectOrg";
            this.colProjectOrg.Width = 300;
            // 
            // colProjectName
            // 
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.colProjectName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colProjectName.HeaderText = "项目名称";
            this.colProjectName.Name = "colProjectName";
            this.colProjectName.Width = 400;
            // 
            // colPeriodEndDate
            // 
            this.colPeriodEndDate.HeaderText = "结束日期";
            this.colPeriodEndDate.Name = "colPeriodEndDate";
            this.colPeriodEndDate.ReadOnly = true;
            // 
            // colState
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.colState.DefaultCellStyle = dataGridViewCellStyle3;
            this.colState.HeaderText = "状态";
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;
            this.colState.Width = 70;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(531, 19);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(0, 12);
            this.customLabel1.TabIndex = 184;
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // chkHasLocked
            // 
            this.chkHasLocked.AutoSize = true;
            this.chkHasLocked.Checked = true;
            this.chkHasLocked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHasLocked.Location = new System.Drawing.Point(559, 18);
            this.chkHasLocked.Name = "chkHasLocked";
            this.chkHasLocked.Size = new System.Drawing.Size(60, 16);
            this.chkHasLocked.TabIndex = 185;
            this.chkHasLocked.Text = "已结账";
            this.chkHasLocked.UseVisualStyleBackColor = true;
            // 
            // chkUnLocked
            // 
            this.chkUnLocked.AutoSize = true;
            this.chkUnLocked.Checked = true;
            this.chkUnLocked.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUnLocked.Location = new System.Drawing.Point(625, 18);
            this.chkUnLocked.Name = "chkUnLocked";
            this.chkUnLocked.Size = new System.Drawing.Size(60, 16);
            this.chkUnLocked.TabIndex = 186;
            this.chkUnLocked.Text = "未结账";
            this.chkUnLocked.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgMaster);
            this.groupBox1.Location = new System.Drawing.Point(12, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(946, 354);
            this.groupBox1.TabIndex = 187;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "项目列表";
            // 
            // ucAccountPeriodCombox1
            // 
            this.ucAccountPeriodCombox1.AutoSize = true;
            this.ucAccountPeriodCombox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ucAccountPeriodCombox1.Location = new System.Drawing.Point(12, 12);
            this.ucAccountPeriodCombox1.Name = "ucAccountPeriodCombox1";
            this.ucAccountPeriodCombox1.SelectedPeriod = null;
            this.ucAccountPeriodCombox1.Size = new System.Drawing.Size(234, 27);
            this.ucAccountPeriodCombox1.TabIndex = 188;
            // 
            // VAccountLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 457);
            this.Name = "VAccountLock";
            this.Text = "VAccountLock";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnUnLock;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnLock;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.CheckBox chkOther;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.CheckBox chkUnLocked;
        private System.Windows.Forms.CheckBox chkHasLocked;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectOrg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriodEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private Application.Business.Erp.SupplyChain.Client.Basic.CommonForm.Controls.UcAccountPeriodCombox ucAccountPeriodCombox1;
    }
}