namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    partial class VSubContractBalanceSubject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSubContractBalanceSubject));
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.gridDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.DtlProjectTask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlProjectTaskDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlCostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlBalanceSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlResourceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlResourceSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlResDiagramNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlBalanceQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlBalancePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlBalanceTotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlQuantityUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlPriceUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(963, 436);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnUpdate);
            this.groupBox1.Controls.Add(this.gridDetail);
            this.groupBox1.Location = new System.Drawing.Point(3, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(957, 408);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "结算分科目明细";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(438, 379);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 23);
            this.btnClose.TabIndex = 45;
            this.btnClose.Text = "确 定";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(227, 379);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 23);
            this.btnSave.TabIndex = 44;
            this.btnSave.Text = "保 存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(116, 379);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(81, 23);
            this.btnUpdate.TabIndex = 43;
            this.btnUpdate.Text = "修 改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Visible = false;
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
            this.DtlProjectTask,
            this.DtlProjectTaskDetail,
            this.DtlCostName,
            this.DtlBalanceSubject,
            this.DtlResourceType,
            this.DtlResourceSpec,
            this.DtlResDiagramNumber,
            this.DtlBalanceQuantity,
            this.DtlBalancePrice,
            this.DtlBalanceTotalPrice,
            this.DtlQuantityUnit,
            this.DtlPriceUnit,
            this.DtlRemark});
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
            this.gridDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridDetail.ReadOnlyCols")));
            this.gridDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDetail.RowHeadersWidth = 22;
            this.gridDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridDetail.RowTemplate.Height = 23;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(948, 356);
            this.gridDetail.TabIndex = 42;
            this.gridDetail.TargetType = null;
            this.gridDetail.VScrollOffset = 0;
            // 
            // DtlProjectTask
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.DtlProjectTask.DefaultCellStyle = dataGridViewCellStyle1;
            this.DtlProjectTask.HeaderText = "工程任务";
            this.DtlProjectTask.Name = "DtlProjectTask";
            this.DtlProjectTask.ReadOnly = true;
            this.DtlProjectTask.Width = 90;
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
            this.DtlProjectTaskDetail.Width = 110;
            // 
            // DtlCostName
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCostName.DefaultCellStyle = dataGridViewCellStyle3;
            this.DtlCostName.HeaderText = "成本名称";
            this.DtlCostName.Name = "DtlCostName";
            this.DtlCostName.ReadOnly = true;
            this.DtlCostName.Width = 90;
            // 
            // DtlBalanceSubject
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.DtlBalanceSubject.DefaultCellStyle = dataGridViewCellStyle4;
            this.DtlBalanceSubject.HeaderText = "结算科目";
            this.DtlBalanceSubject.Name = "DtlBalanceSubject";
            this.DtlBalanceSubject.ReadOnly = true;
            this.DtlBalanceSubject.Width = 90;
            // 
            // DtlResourceType
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.DtlResourceType.DefaultCellStyle = dataGridViewCellStyle5;
            this.DtlResourceType.HeaderText = "资源类型";
            this.DtlResourceType.Name = "DtlResourceType";
            this.DtlResourceType.ReadOnly = true;
            this.DtlResourceType.Width = 90;
            // 
            // DtlResourceSpec
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.DtlResourceSpec.DefaultCellStyle = dataGridViewCellStyle6;
            this.DtlResourceSpec.HeaderText = "规格型号";
            this.DtlResourceSpec.Name = "DtlResourceSpec";
            this.DtlResourceSpec.ReadOnly = true;
            this.DtlResourceSpec.Width = 90;
            // 
            // DtlResDiagramNumber
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.DtlResDiagramNumber.DefaultCellStyle = dataGridViewCellStyle7;
            this.DtlResDiagramNumber.HeaderText = "图号";
            this.DtlResDiagramNumber.Name = "DtlResDiagramNumber";
            this.DtlResDiagramNumber.ReadOnly = true;
            this.DtlResDiagramNumber.Width = 90;
            // 
            // DtlBalanceQuantity
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            this.DtlBalanceQuantity.DefaultCellStyle = dataGridViewCellStyle8;
            this.DtlBalanceQuantity.HeaderText = "结算数量";
            this.DtlBalanceQuantity.Name = "DtlBalanceQuantity";
            this.DtlBalanceQuantity.ReadOnly = true;
            this.DtlBalanceQuantity.Width = 90;
            // 
            // DtlBalancePrice
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            this.DtlBalancePrice.DefaultCellStyle = dataGridViewCellStyle9;
            this.DtlBalancePrice.HeaderText = "结算单价";
            this.DtlBalancePrice.Name = "DtlBalancePrice";
            this.DtlBalancePrice.Width = 90;
            // 
            // DtlBalanceTotalPrice
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            this.DtlBalanceTotalPrice.DefaultCellStyle = dataGridViewCellStyle10;
            this.DtlBalanceTotalPrice.HeaderText = "结算合价";
            this.DtlBalanceTotalPrice.Name = "DtlBalanceTotalPrice";
            this.DtlBalanceTotalPrice.ReadOnly = true;
            this.DtlBalanceTotalPrice.Width = 90;
            // 
            // DtlQuantityUnit
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            this.DtlQuantityUnit.DefaultCellStyle = dataGridViewCellStyle11;
            this.DtlQuantityUnit.HeaderText = "数量单位";
            this.DtlQuantityUnit.Name = "DtlQuantityUnit";
            this.DtlQuantityUnit.ReadOnly = true;
            this.DtlQuantityUnit.Width = 90;
            // 
            // DtlPriceUnit
            // 
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            this.DtlPriceUnit.DefaultCellStyle = dataGridViewCellStyle12;
            this.DtlPriceUnit.HeaderText = "价格单位";
            this.DtlPriceUnit.Name = "DtlPriceUnit";
            this.DtlPriceUnit.ReadOnly = true;
            this.DtlPriceUnit.Width = 90;
            // 
            // DtlRemark
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            this.DtlRemark.DefaultCellStyle = dataGridViewCellStyle13;
            this.DtlRemark.HeaderText = "备注";
            this.DtlRemark.Name = "DtlRemark";
            // 
            // VSubContractBalanceSubject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(963, 436);
            this.Name = "VSubContractBalanceSubject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "结算分科目明细";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridDetail;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlProjectTask;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlProjectTaskDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCostName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlBalanceSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlResourceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlResourceSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlResDiagramNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlBalanceQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlBalancePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlBalanceTotalPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlQuantityUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlPriceUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlRemark;


    }
}