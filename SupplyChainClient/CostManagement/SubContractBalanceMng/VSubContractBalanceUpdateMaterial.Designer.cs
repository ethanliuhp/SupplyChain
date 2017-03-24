namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    partial class VSubContractBalanceUpdateMaterial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSubContractBalanceUpdateMaterial));
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSaveAndExit = new System.Windows.Forms.Button();
            this.gridDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.DtlProjectTaskDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlIsBalance = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DtlCostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlCostAccountSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlResourceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlResourceTypeQuanlity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlResourceTypeSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlDiagramNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlAccQuotaQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlAccQuantityPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlAccProjectAmountPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlAccUsageQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlAccTotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlCurrContractIncomeQny = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlCurrIncomeContractTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlCurrResponsibleCostQny = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlCurrResponsibleCostTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(891, 283);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnSaveAndExit);
            this.groupBox1.Controls.Add(this.gridDetail);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(891, 283);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工程任务明细分科目核算";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(446, 255);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(108, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保   存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(704, 255);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "退 出";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSaveAndExit
            // 
            this.btnSaveAndExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveAndExit.Location = new System.Drawing.Point(576, 255);
            this.btnSaveAndExit.Name = "btnSaveAndExit";
            this.btnSaveAndExit.Size = new System.Drawing.Size(108, 23);
            this.btnSaveAndExit.TabIndex = 0;
            this.btnSaveAndExit.Text = "保存及退出";
            this.btnSaveAndExit.UseVisualStyleBackColor = true;
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
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DtlProjectTaskDetail,
            this.DtlIsBalance,
            this.DtlCostName,
            this.DtlCostAccountSubject,
            this.DtlResourceType,
            this.DtlResourceTypeQuanlity,
            this.DtlResourceTypeSpec,
            this.DtlDiagramNumber,
            this.DtlAccQuotaQuantity,
            this.DtlAccQuantityPrice,
            this.DtlAccProjectAmountPrice,
            this.DtlAccUsageQuantity,
            this.DtlAccTotalPrice,
            this.DtlCurrContractIncomeQny,
            this.DtlCurrIncomeContractTotal,
            this.DtlCurrResponsibleCostQny,
            this.DtlCurrResponsibleCostTotal});
            this.gridDetail.CustomBackColor = false;
            this.gridDetail.EditCellBackColor = System.Drawing.Color.White;
            this.gridDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridDetail.FreezeFirstRow = false;
            this.gridDetail.FreezeLastRow = false;
            this.gridDetail.FrontColumnCount = 0;
            this.gridDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridDetail.HScrollOffset = 0;
            this.gridDetail.IsAllowOrder = true;
            this.gridDetail.IsConfirmDelete = true;
            this.gridDetail.Location = new System.Drawing.Point(3, 20);
            this.gridDetail.MultiSelect = false;
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
            this.gridDetail.Size = new System.Drawing.Size(885, 229);
            this.gridDetail.TabIndex = 42;
            this.gridDetail.TargetType = null;
            this.gridDetail.VScrollOffset = 0;
            // 
            // DtlProjectTaskDetail
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.DtlProjectTaskDetail.DefaultCellStyle = dataGridViewCellStyle1;
            this.DtlProjectTaskDetail.FillWeight = 80F;
            this.DtlProjectTaskDetail.HeaderText = "工程任务明细";
            this.DtlProjectTaskDetail.Name = "DtlProjectTaskDetail";
            this.DtlProjectTaskDetail.ReadOnly = true;
            this.DtlProjectTaskDetail.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DtlProjectTaskDetail.Width = 110;
            // 
            // DtlIsBalance
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.DtlIsBalance.DefaultCellStyle = dataGridViewCellStyle2;
            this.DtlIsBalance.HeaderText = "是否结算";
            this.DtlIsBalance.Items.AddRange(new object[] {
            "是",
            "否"});
            this.DtlIsBalance.Name = "DtlIsBalance";
            this.DtlIsBalance.ReadOnly = true;
            this.DtlIsBalance.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DtlIsBalance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DtlIsBalance.Width = 80;
            // 
            // DtlCostName
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCostName.DefaultCellStyle = dataGridViewCellStyle3;
            this.DtlCostName.HeaderText = "耗用名称";
            this.DtlCostName.Name = "DtlCostName";
            this.DtlCostName.ReadOnly = true;
            this.DtlCostName.Width = 80;
            // 
            // DtlCostAccountSubject
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCostAccountSubject.DefaultCellStyle = dataGridViewCellStyle4;
            this.DtlCostAccountSubject.HeaderText = "成本核算科目";
            this.DtlCostAccountSubject.Name = "DtlCostAccountSubject";
            this.DtlCostAccountSubject.ReadOnly = true;
            this.DtlCostAccountSubject.Width = 110;
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
            // DtlResourceTypeQuanlity
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.DtlResourceTypeQuanlity.DefaultCellStyle = dataGridViewCellStyle6;
            this.DtlResourceTypeQuanlity.HeaderText = "资源材质";
            this.DtlResourceTypeQuanlity.Name = "DtlResourceTypeQuanlity";
            this.DtlResourceTypeQuanlity.ReadOnly = true;
            this.DtlResourceTypeQuanlity.Visible = false;
            this.DtlResourceTypeQuanlity.Width = 80;
            // 
            // DtlResourceTypeSpec
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.DtlResourceTypeSpec.DefaultCellStyle = dataGridViewCellStyle7;
            this.DtlResourceTypeSpec.HeaderText = "资源规格";
            this.DtlResourceTypeSpec.Name = "DtlResourceTypeSpec";
            this.DtlResourceTypeSpec.ReadOnly = true;
            this.DtlResourceTypeSpec.Width = 80;
            // 
            // DtlDiagramNumber
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            this.DtlDiagramNumber.DefaultCellStyle = dataGridViewCellStyle8;
            this.DtlDiagramNumber.HeaderText = "图号";
            this.DtlDiagramNumber.Name = "DtlDiagramNumber";
            this.DtlDiagramNumber.ReadOnly = true;
            this.DtlDiagramNumber.Width = 80;
            // 
            // DtlAccQuotaQuantity
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            this.DtlAccQuotaQuantity.DefaultCellStyle = dataGridViewCellStyle9;
            this.DtlAccQuotaQuantity.HeaderText = "核算定额数量";
            this.DtlAccQuotaQuantity.Name = "DtlAccQuotaQuantity";
            this.DtlAccQuotaQuantity.ReadOnly = true;
            this.DtlAccQuotaQuantity.Width = 70;
            // 
            // DtlAccQuantityPrice
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            this.DtlAccQuantityPrice.DefaultCellStyle = dataGridViewCellStyle10;
            this.DtlAccQuantityPrice.HeaderText = "核算数量单价";
            this.DtlAccQuantityPrice.Name = "DtlAccQuantityPrice";
            this.DtlAccQuantityPrice.ReadOnly = true;
            this.DtlAccQuantityPrice.Width = 70;
            // 
            // DtlAccProjectAmountPrice
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            this.DtlAccProjectAmountPrice.DefaultCellStyle = dataGridViewCellStyle11;
            this.DtlAccProjectAmountPrice.HeaderText = "核算工程量单价";
            this.DtlAccProjectAmountPrice.Name = "DtlAccProjectAmountPrice";
            this.DtlAccProjectAmountPrice.ReadOnly = true;
            this.DtlAccProjectAmountPrice.Width = 80;
            // 
            // DtlAccUsageQuantity
            // 
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            this.DtlAccUsageQuantity.DefaultCellStyle = dataGridViewCellStyle12;
            this.DtlAccUsageQuantity.HeaderText = "核算消耗数量";
            this.DtlAccUsageQuantity.Name = "DtlAccUsageQuantity";
            this.DtlAccUsageQuantity.ReadOnly = true;
            this.DtlAccUsageQuantity.Width = 70;
            // 
            // DtlAccTotalPrice
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            this.DtlAccTotalPrice.DefaultCellStyle = dataGridViewCellStyle13;
            this.DtlAccTotalPrice.HeaderText = "核算合价";
            this.DtlAccTotalPrice.Name = "DtlAccTotalPrice";
            this.DtlAccTotalPrice.ReadOnly = true;
            this.DtlAccTotalPrice.Width = 80;
            // 
            // DtlCurrContractIncomeQny
            // 
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCurrContractIncomeQny.DefaultCellStyle = dataGridViewCellStyle14;
            this.DtlCurrContractIncomeQny.HeaderText = "本次实现合同收入量";
            this.DtlCurrContractIncomeQny.Name = "DtlCurrContractIncomeQny";
            this.DtlCurrContractIncomeQny.ReadOnly = true;
            this.DtlCurrContractIncomeQny.Width = 90;
            // 
            // DtlCurrIncomeContractTotal
            // 
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCurrIncomeContractTotal.DefaultCellStyle = dataGridViewCellStyle15;
            this.DtlCurrIncomeContractTotal.HeaderText = "本次实现合同收入合价";
            this.DtlCurrIncomeContractTotal.Name = "DtlCurrIncomeContractTotal";
            this.DtlCurrIncomeContractTotal.ReadOnly = true;
            this.DtlCurrIncomeContractTotal.Width = 90;
            // 
            // DtlCurrResponsibleCostQny
            // 
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCurrResponsibleCostQny.DefaultCellStyle = dataGridViewCellStyle16;
            this.DtlCurrResponsibleCostQny.HeaderText = "本次责任成本数量";
            this.DtlCurrResponsibleCostQny.Name = "DtlCurrResponsibleCostQny";
            this.DtlCurrResponsibleCostQny.ReadOnly = true;
            this.DtlCurrResponsibleCostQny.Width = 80;
            // 
            // DtlCurrResponsibleCostTotal
            // 
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control;
            this.DtlCurrResponsibleCostTotal.DefaultCellStyle = dataGridViewCellStyle17;
            this.DtlCurrResponsibleCostTotal.HeaderText = "本次责任成本合价";
            this.DtlCurrResponsibleCostTotal.Name = "DtlCurrResponsibleCostTotal";
            this.DtlCurrResponsibleCostTotal.ReadOnly = true;
            this.DtlCurrResponsibleCostTotal.Width = 80;
            // 
            // VSubContractBalanceUpdateMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 283);
            this.Name = "VSubContractBalanceUpdateMaterial";
            this.Text = "调整料费是否结算";
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
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSaveAndExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlProjectTaskDetail;
        private System.Windows.Forms.DataGridViewComboBoxColumn DtlIsBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCostName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCostAccountSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlResourceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlResourceTypeQuanlity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlResourceTypeSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlDiagramNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlAccQuotaQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlAccQuantityPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlAccProjectAmountPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlAccUsageQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlAccTotalPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCurrContractIncomeQny;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCurrIncomeContractTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCurrResponsibleCostQny;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCurrResponsibleCostTotal;

    }
}