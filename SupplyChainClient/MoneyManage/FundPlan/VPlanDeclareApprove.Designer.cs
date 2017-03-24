namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    partial class VPlanDeclareApprove
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VPlanDeclareApprove));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgBillList = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCodeBill = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colBillId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpProjectReport = new System.Windows.Forms.TabPage();
            this.gdProjectReport = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpCompanyReport = new System.Windows.Forms.TabPage();
            this.gdCompanyReport = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvAppSteps = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.StepOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StepName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppRelations = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbMoney = new System.Windows.Forms.Label();
            this.txtReark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtApproveMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgBillList)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpProjectReport.SuspendLayout();
            this.tpCompanyReport.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppSteps)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.groupBox3);
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.tabControl1);
            this.pnlFloor.Size = new System.Drawing.Size(1152, 622);
            this.pnlFloor.Controls.SetChildIndex(this.tabControl1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox3, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.dgBillList);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 616);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "待审批资金计划";
            // 
            // dgBillList
            // 
            this.dgBillList.AddDefaultMenu = false;
            this.dgBillList.AddNoColumn = true;
            this.dgBillList.AllowUserToAddRows = false;
            this.dgBillList.AllowUserToDeleteRows = false;
            this.dgBillList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgBillList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgBillList.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgBillList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgBillList.ColumnHeadersHeight = 24;
            this.dgBillList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgBillList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCodeBill,
            this.colBillId});
            this.dgBillList.CustomBackColor = false;
            this.dgBillList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgBillList.EditCellBackColor = System.Drawing.Color.White;
            this.dgBillList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgBillList.EnableHeadersVisualStyles = false;
            this.dgBillList.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgBillList.FreezeFirstRow = false;
            this.dgBillList.FreezeLastRow = false;
            this.dgBillList.FrontColumnCount = 0;
            this.dgBillList.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgBillList.HScrollOffset = 0;
            this.dgBillList.IsAllowOrder = true;
            this.dgBillList.IsConfirmDelete = true;
            this.dgBillList.Location = new System.Drawing.Point(3, 17);
            this.dgBillList.Name = "dgBillList";
            this.dgBillList.PageIndex = 0;
            this.dgBillList.PageSize = 0;
            this.dgBillList.Query = null;
            this.dgBillList.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgBillList.ReadOnlyCols")));
            this.dgBillList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgBillList.RowHeadersVisible = false;
            this.dgBillList.RowHeadersWidth = 22;
            this.dgBillList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgBillList.RowTemplate.Height = 23;
            this.dgBillList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgBillList.Size = new System.Drawing.Size(264, 596);
            this.dgBillList.TabIndex = 10;
            this.dgBillList.TargetType = null;
            this.dgBillList.VScrollOffset = 0;
            // 
            // colCodeBill
            // 
            this.colCodeBill.DataPropertyName = "BillCode";
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.colCodeBill.DefaultCellStyle = dataGridViewCellStyle2;
            this.colCodeBill.HeaderText = "单据号";
            this.colCodeBill.Name = "colCodeBill";
            this.colCodeBill.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCodeBill.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCodeBill.Width = 200;
            // 
            // colBillId
            // 
            this.colBillId.DataPropertyName = "BillId";
            this.colBillId.HeaderText = "BillId";
            this.colBillId.Name = "colBillId";
            this.colBillId.ReadOnly = true;
            this.colBillId.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpProjectReport);
            this.tabControl1.Controls.Add(this.tpCompanyReport);
            this.tabControl1.Location = new System.Drawing.Point(279, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(870, 422);
            this.tabControl1.TabIndex = 2;
            // 
            // tpProjectReport
            // 
            this.tpProjectReport.Controls.Add(this.gdProjectReport);
            this.tpProjectReport.Location = new System.Drawing.Point(4, 22);
            this.tpProjectReport.Name = "tpProjectReport";
            this.tpProjectReport.Padding = new System.Windows.Forms.Padding(3);
            this.tpProjectReport.Size = new System.Drawing.Size(862, 396);
            this.tpProjectReport.TabIndex = 0;
            this.tpProjectReport.Tag = "项目资金支付计划申报表";
            this.tpProjectReport.Text = "项目资金计划申请表";
            this.tpProjectReport.UseVisualStyleBackColor = true;
            // 
            // gdProjectReport
            // 
            this.gdProjectReport.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdProjectReport.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectReport.CheckedImage")));
            this.gdProjectReport.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdProjectReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdProjectReport.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdProjectReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdProjectReport.Location = new System.Drawing.Point(3, 3);
            this.gdProjectReport.Name = "gdProjectReport";
            this.gdProjectReport.Size = new System.Drawing.Size(856, 390);
            this.gdProjectReport.TabIndex = 236;
            this.gdProjectReport.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectReport.UncheckedImage")));
            // 
            // tpCompanyReport
            // 
            this.tpCompanyReport.Controls.Add(this.gdCompanyReport);
            this.tpCompanyReport.Location = new System.Drawing.Point(4, 22);
            this.tpCompanyReport.Name = "tpCompanyReport";
            this.tpCompanyReport.Padding = new System.Windows.Forms.Padding(3);
            this.tpCompanyReport.Size = new System.Drawing.Size(99, 57);
            this.tpCompanyReport.TabIndex = 1;
            this.tpCompanyReport.Tag = "分公司资金支付计划申报表";
            this.tpCompanyReport.Text = "分公司资金计划申请表";
            this.tpCompanyReport.UseVisualStyleBackColor = true;
            // 
            // gdCompanyReport
            // 
            this.gdCompanyReport.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdCompanyReport.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdCompanyReport.CheckedImage")));
            this.gdCompanyReport.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdCompanyReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdCompanyReport.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdCompanyReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdCompanyReport.Location = new System.Drawing.Point(3, 3);
            this.gdCompanyReport.Name = "gdCompanyReport";
            this.gdCompanyReport.Size = new System.Drawing.Size(93, 51);
            this.gdCompanyReport.TabIndex = 237;
            this.gdCompanyReport.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdCompanyReport.UncheckedImage")));
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvAppSteps);
            this.groupBox2.Location = new System.Drawing.Point(279, 426);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(870, 131);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "审批记录";
            // 
            // dgvAppSteps
            // 
            this.dgvAppSteps.AddDefaultMenu = false;
            this.dgvAppSteps.AddNoColumn = false;
            this.dgvAppSteps.AllowUserToAddRows = false;
            this.dgvAppSteps.AllowUserToDeleteRows = false;
            this.dgvAppSteps.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvAppSteps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvAppSteps.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvAppSteps.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvAppSteps.ColumnHeadersHeight = 24;
            this.dgvAppSteps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAppSteps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StepOrder,
            this.StepName,
            this.AppRelations,
            this.AppRole,
            this.AppPerson,
            this.AppDateTime,
            this.AppComments,
            this.AppStatus});
            this.dgvAppSteps.CustomBackColor = false;
            this.dgvAppSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAppSteps.EditCellBackColor = System.Drawing.Color.White;
            this.dgvAppSteps.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvAppSteps.EnableHeadersVisualStyles = false;
            this.dgvAppSteps.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgvAppSteps.FreezeFirstRow = false;
            this.dgvAppSteps.FreezeLastRow = false;
            this.dgvAppSteps.FrontColumnCount = 0;
            this.dgvAppSteps.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvAppSteps.HScrollOffset = 0;
            this.dgvAppSteps.IsAllowOrder = true;
            this.dgvAppSteps.IsConfirmDelete = true;
            this.dgvAppSteps.Location = new System.Drawing.Point(3, 17);
            this.dgvAppSteps.Name = "dgvAppSteps";
            this.dgvAppSteps.PageIndex = 0;
            this.dgvAppSteps.PageSize = 0;
            this.dgvAppSteps.Query = null;
            this.dgvAppSteps.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvAppSteps.ReadOnlyCols")));
            this.dgvAppSteps.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvAppSteps.RowHeadersVisible = false;
            this.dgvAppSteps.RowHeadersWidth = 22;
            this.dgvAppSteps.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvAppSteps.RowTemplate.Height = 23;
            this.dgvAppSteps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAppSteps.Size = new System.Drawing.Size(864, 111);
            this.dgvAppSteps.TabIndex = 46;
            this.dgvAppSteps.TargetType = null;
            this.dgvAppSteps.VScrollOffset = 0;
            // 
            // StepOrder
            // 
            this.StepOrder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.StepOrder.HeaderText = "审批步骤";
            this.StepOrder.Name = "StepOrder";
            this.StepOrder.Width = 77;
            // 
            // StepName
            // 
            this.StepName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.StepName.HeaderText = "审批步骤名称";
            this.StepName.Name = "StepName";
            this.StepName.Width = 101;
            // 
            // AppRelations
            // 
            this.AppRelations.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.AppRelations.HeaderText = "审批关系";
            this.AppRelations.Name = "AppRelations";
            this.AppRelations.Width = 77;
            // 
            // AppRole
            // 
            this.AppRole.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.AppRole.HeaderText = "审批角色";
            this.AppRole.Name = "AppRole";
            this.AppRole.Width = 77;
            // 
            // AppPerson
            // 
            this.AppPerson.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.AppPerson.HeaderText = "审批人";
            this.AppPerson.Name = "AppPerson";
            this.AppPerson.Width = 65;
            // 
            // AppDateTime
            // 
            this.AppDateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.AppDateTime.HeaderText = "审批日期";
            this.AppDateTime.Name = "AppDateTime";
            this.AppDateTime.Width = 77;
            // 
            // AppComments
            // 
            this.AppComments.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.AppComments.HeaderText = "审批意见";
            this.AppComments.Name = "AppComments";
            this.AppComments.Width = 77;
            // 
            // AppStatus
            // 
            this.AppStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.AppStatus.HeaderText = "审批状态";
            this.AppStatus.Name = "AppStatus";
            this.AppStatus.Width = 77;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lbMoney);
            this.groupBox3.Controls.Add(this.txtReark);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.btnReject);
            this.groupBox3.Controls.Add(this.btnOk);
            this.groupBox3.Controls.Add(this.txtApproveMoney);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(279, 555);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(870, 62);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "审批";
            // 
            // lbMoney
            // 
            this.lbMoney.AutoSize = true;
            this.lbMoney.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbMoney.ForeColor = System.Drawing.Color.Red;
            this.lbMoney.Location = new System.Drawing.Point(279, 16);
            this.lbMoney.MinimumSize = new System.Drawing.Size(65, 12);
            this.lbMoney.Name = "lbMoney";
            this.lbMoney.Size = new System.Drawing.Size(65, 16);
            this.lbMoney.TabIndex = 32;
            // 
            // txtReark
            // 
            this.txtReark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReark.BackColor = System.Drawing.SystemColors.Control;
            this.txtReark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtReark.DrawSelf = false;
            this.txtReark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtReark.EnterToTab = false;
            this.txtReark.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtReark.ForeColor = System.Drawing.Color.Red;
            this.txtReark.Location = new System.Drawing.Point(92, 40);
            this.txtReark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtReark.Name = "txtReark";
            this.txtReark.Padding = new System.Windows.Forms.Padding(1);
            this.txtReark.ReadOnly = false;
            this.txtReark.Size = new System.Drawing.Size(763, 16);
            this.txtReark.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 30;
            this.label2.Text = "审批意见：";
            // 
            // btnReject
            // 
            this.btnReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReject.ForeColor = System.Drawing.Color.Red;
            this.btnReject.Location = new System.Drawing.Point(789, 18);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 23);
            this.btnReject.TabIndex = 29;
            this.btnReject.Text = "驳回";
            this.btnReject.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.ForeColor = System.Drawing.Color.Green;
            this.btnOk.Location = new System.Drawing.Point(702, 18);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 28;
            this.btnOk.Text = "通过";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // txtApproveMoney
            // 
            this.txtApproveMoney.BackColor = System.Drawing.SystemColors.Control;
            this.txtApproveMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtApproveMoney.DrawSelf = false;
            this.txtApproveMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtApproveMoney.EnterToTab = false;
            this.txtApproveMoney.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtApproveMoney.ForeColor = System.Drawing.Color.Red;
            this.txtApproveMoney.Location = new System.Drawing.Point(92, 16);
            this.txtApproveMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtApproveMoney.Name = "txtApproveMoney";
            this.txtApproveMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtApproveMoney.ReadOnly = false;
            this.txtApproveMoney.Size = new System.Drawing.Size(172, 16);
            this.txtApproveMoney.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "批准额度：";
            // 
            // VPlanDeclareApprove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 622);
            this.Name = "VPlanDeclareApprove";
            this.Text = "资金计划审批";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgBillList)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpProjectReport.ResumeLayout(false);
            this.tpCompanyReport.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppSteps)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgBillList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpProjectReport;
        private System.Windows.Forms.TabPage tpCompanyReport;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdProjectReport;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdCompanyReport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvAppSteps;
        private System.Windows.Forms.Label label1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtApproveMoney;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DataGridViewLinkColumn colCodeBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillId;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtReark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.DataGridViewTextBoxColumn StepOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn StepName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppRelations;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppComments;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppStatus;
        private System.Windows.Forms.Label lbMoney;
    }
}