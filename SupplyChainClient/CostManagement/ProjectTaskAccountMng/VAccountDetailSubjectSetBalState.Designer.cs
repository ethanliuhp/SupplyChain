namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    partial class VAccountDetailSubjectSetBalState
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlProjectTaskDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlIsBalance = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DtlCostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlCostAccountSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlResourceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlResourceTypeQuanlity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlResourceTypeSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlDiagramNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMatrailSetNoBal = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.btnSureAndExit = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnSureAndExit);
            this.pnlFloor.Controls.Add(this.btnSure);
            this.pnlFloor.Controls.Add(this.btnExit);
            this.pnlFloor.Controls.Add(this.btnMatrailSetNoBal);
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(967, 435);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnMatrailSetNoBal, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExit, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSure, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSureAndExit, 0);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gridDetail);
            this.groupBox1.Location = new System.Drawing.Point(3, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 318);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工程任务明细分科目核算";
            // 
            // gridDetail
            // 
            this.gridDetail.AddDefaultMenu = false;
            this.gridDetail.AddNoColumn = true;
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.Color.White;
            this.gridDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.DtlProjectTaskDetail,
            this.DtlIsBalance,
            this.DtlCostName,
            this.DtlCostAccountSubject,
            this.DtlResourceType,
            this.DtlResourceTypeQuanlity,
            this.DtlResourceTypeSpec,
            this.DtlDiagramNumber});
            this.gridDetail.CustomBackColor = false;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditCellBackColor = System.Drawing.Color.White;
            this.gridDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridDetail.FreezeFirstRow = false;
            this.gridDetail.FreezeLastRow = false;
            this.gridDetail.FrontColumnCount = 0;
            this.gridDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridDetail.HScrollOffset = 0;
            this.gridDetail.IsAllowOrder = true;
            this.gridDetail.IsConfirmDelete = true;
            this.gridDetail.Location = new System.Drawing.Point(3, 17);
            this.gridDetail.MultiSelect = false;
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.PageIndex = 0;
            this.gridDetail.PageSize = 0;
            this.gridDetail.Query = null;
            this.gridDetail.ReadOnly = true;
            this.gridDetail.ReadOnlyCols = null;
            this.gridDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDetail.RowHeadersWidth = 22;
            this.gridDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridDetail.RowTemplate.Height = 23;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(955, 298);
            this.gridDetail.TabIndex = 42;
            this.gridDetail.TargetType = null;
            this.gridDetail.VScrollOffset = 0;
            // 
            // colSelect
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.colSelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.Visible = false;
            // 
            // DtlProjectTaskDetail
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.DtlProjectTaskDetail.DefaultCellStyle = dataGridViewCellStyle2;
            this.DtlProjectTaskDetail.FillWeight = 80F;
            this.DtlProjectTaskDetail.HeaderText = "工程任务明细";
            this.DtlProjectTaskDetail.Name = "DtlProjectTaskDetail";
            this.DtlProjectTaskDetail.ReadOnly = true;
            this.DtlProjectTaskDetail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DtlProjectTaskDetail.Width = 200;
            // 
            // DtlIsBalance
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.DtlIsBalance.DefaultCellStyle = dataGridViewCellStyle3;
            this.DtlIsBalance.HeaderText = "是否结算";
            this.DtlIsBalance.Name = "DtlIsBalance";
            this.DtlIsBalance.ReadOnly = true;
            this.DtlIsBalance.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DtlIsBalance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DtlIsBalance.Width = 80;
            // 
            // DtlCostName
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCostName.DefaultCellStyle = dataGridViewCellStyle4;
            this.DtlCostName.HeaderText = "耗用名称";
            this.DtlCostName.Name = "DtlCostName";
            this.DtlCostName.ReadOnly = true;
            this.DtlCostName.Width = 120;
            // 
            // DtlCostAccountSubject
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCostAccountSubject.DefaultCellStyle = dataGridViewCellStyle5;
            this.DtlCostAccountSubject.HeaderText = "成本核算科目";
            this.DtlCostAccountSubject.Name = "DtlCostAccountSubject";
            this.DtlCostAccountSubject.ReadOnly = true;
            this.DtlCostAccountSubject.Width = 120;
            // 
            // DtlResourceType
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.DtlResourceType.DefaultCellStyle = dataGridViewCellStyle6;
            this.DtlResourceType.HeaderText = "资源类型";
            this.DtlResourceType.Name = "DtlResourceType";
            this.DtlResourceType.ReadOnly = true;
            this.DtlResourceType.Width = 150;
            // 
            // DtlResourceTypeQuanlity
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.DtlResourceTypeQuanlity.DefaultCellStyle = dataGridViewCellStyle7;
            this.DtlResourceTypeQuanlity.HeaderText = "资源材质";
            this.DtlResourceTypeQuanlity.Name = "DtlResourceTypeQuanlity";
            this.DtlResourceTypeQuanlity.ReadOnly = true;
            this.DtlResourceTypeQuanlity.Visible = false;
            this.DtlResourceTypeQuanlity.Width = 110;
            // 
            // DtlResourceTypeSpec
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            this.DtlResourceTypeSpec.DefaultCellStyle = dataGridViewCellStyle8;
            this.DtlResourceTypeSpec.HeaderText = "资源规格";
            this.DtlResourceTypeSpec.Name = "DtlResourceTypeSpec";
            this.DtlResourceTypeSpec.ReadOnly = true;
            this.DtlResourceTypeSpec.Width = 110;
            // 
            // DtlDiagramNumber
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            this.DtlDiagramNumber.DefaultCellStyle = dataGridViewCellStyle9;
            this.DtlDiagramNumber.HeaderText = "图号";
            this.DtlDiagramNumber.Name = "DtlDiagramNumber";
            this.DtlDiagramNumber.ReadOnly = true;
            this.DtlDiagramNumber.Width = 110;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnQuery);
            this.groupBox2.Controls.Add(this.txtTaskName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(3, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(955, 47);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "查询条件";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(284, 20);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(77, 23);
            this.btnQuery.TabIndex = 16;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // txtTaskName
            // 
            this.txtTaskName.Location = new System.Drawing.Point(76, 21);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(171, 21);
            this.txtTaskName.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "任务明细：";
            // 
            // btnMatrailSetNoBal
            // 
            this.btnMatrailSetNoBal.Location = new System.Drawing.Point(520, 405);
            this.btnMatrailSetNoBal.Name = "btnMatrailSetNoBal";
            this.btnMatrailSetNoBal.Size = new System.Drawing.Size(180, 23);
            this.btnMatrailSetNoBal.TabIndex = 17;
            this.btnMatrailSetNoBal.Text = "批量设置材料结算标志为否";
            this.btnMatrailSetNoBal.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(839, 405);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(77, 23);
            this.btnExit.TabIndex = 18;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(423, 405);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(77, 23);
            this.btnSure.TabIndex = 20;
            this.btnSure.Text = "确  认";
            this.btnSure.UseVisualStyleBackColor = true;
            // 
            // btnSureAndExit
            // 
            this.btnSureAndExit.Location = new System.Drawing.Point(735, 405);
            this.btnSureAndExit.Name = "btnSureAndExit";
            this.btnSureAndExit.Size = new System.Drawing.Size(77, 23);
            this.btnSureAndExit.TabIndex = 21;
            this.btnSureAndExit.Text = "确认及退出";
            this.btnSureAndExit.UseVisualStyleBackColor = true;
            // 
            // VAccountDetailSubjectSetBalState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(967, 435);
            this.Name = "VAccountDetailSubjectSetBalState";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "工程资源耗用核算";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridDetail;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtTaskName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMatrailSetNoBal;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Button btnSureAndExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlProjectTaskDetail;
        private System.Windows.Forms.DataGridViewComboBoxColumn DtlIsBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCostName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCostAccountSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlResourceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlResourceTypeQuanlity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlResourceTypeSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlDiagramNumber;


    }
}