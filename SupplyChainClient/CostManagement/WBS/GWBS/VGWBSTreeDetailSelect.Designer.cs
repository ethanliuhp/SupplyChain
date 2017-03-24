namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    partial class VGWBSTreeDetailSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VGWBSTreeDetailSelect));
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gridGWBDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.ckb = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DtlCostItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContractGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlMainResourceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlMainResourceTypeSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiagramNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlQuantityUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlPlanQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlPlanPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlPlanTotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlPriceUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlMainResourceTypeQuality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlCostItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DtlOrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGWBDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.gridGWBDetail);
            this.pnlFloor.Controls.Add(this.btnSave);
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Size = new System.Drawing.Size(946, 430);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSave, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.gridGWBDetail, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(145, 20);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(291, 394);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 21);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "确定";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(377, 394);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 21);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gridGWBDetail
            // 
            this.gridGWBDetail.AddDefaultMenu = false;
            this.gridGWBDetail.AddNoColumn = true;
            this.gridGWBDetail.AllowUserToAddRows = false;
            this.gridGWBDetail.AllowUserToDeleteRows = false;
            this.gridGWBDetail.AllowUserToOrderColumns = true;
            this.gridGWBDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridGWBDetail.BackgroundColor = System.Drawing.Color.White;
            this.gridGWBDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridGWBDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridGWBDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridGWBDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGWBDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ckb,
            this.DtlCostItemCode,
            this.DtlName,
            this.colContractGroupName,
            this.DtlMainResourceType,
            this.DtlMainResourceTypeSpec,
            this.DiagramNumber,
            this.DtlQuantityUnit,
            this.DtlPlanQuantity,
            this.DtlPlanPrice,
            this.DtlPlanTotalPrice,
            this.DtlPriceUnit,
            this.DtlState,
            this.DtlDesc,
            this.DtlMainResourceTypeQuality,
            this.DtlCostItem,
            this.DtlOrderNo});
            this.gridGWBDetail.CustomBackColor = false;
            this.gridGWBDetail.EditCellBackColor = System.Drawing.Color.White;
            this.gridGWBDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridGWBDetail.FreezeFirstRow = false;
            this.gridGWBDetail.FreezeLastRow = false;
            this.gridGWBDetail.FrontColumnCount = 0;
            this.gridGWBDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridGWBDetail.HScrollOffset = 0;
            this.gridGWBDetail.IsAllowOrder = false;
            this.gridGWBDetail.IsConfirmDelete = true;
            this.gridGWBDetail.Location = new System.Drawing.Point(3, 44);
            this.gridGWBDetail.Name = "gridGWBDetail";
            this.gridGWBDetail.PageIndex = 0;
            this.gridGWBDetail.PageSize = 0;
            this.gridGWBDetail.Query = null;
            this.gridGWBDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridGWBDetail.ReadOnlyCols")));
            this.gridGWBDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridGWBDetail.RowHeadersWidth = 22;
            this.gridGWBDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridGWBDetail.RowTemplate.Height = 23;
            this.gridGWBDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGWBDetail.Size = new System.Drawing.Size(941, 333);
            this.gridGWBDetail.TabIndex = 40;
            this.gridGWBDetail.TargetType = null;
            this.gridGWBDetail.VScrollOffset = 0;
            // 
            // ckb
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.NullValue = false;
            this.ckb.DefaultCellStyle = dataGridViewCellStyle1;
            this.ckb.HeaderText = "";
            this.ckb.Name = "ckb";
            this.ckb.Width = 30;
            // 
            // DtlCostItemCode
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            this.DtlCostItemCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.DtlCostItemCode.HeaderText = "定额编号";
            this.DtlCostItemCode.Name = "DtlCostItemCode";
            this.DtlCostItemCode.ReadOnly = true;
            this.DtlCostItemCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlCostItemCode.Width = 80;
            // 
            // DtlName
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.DtlName.DefaultCellStyle = dataGridViewCellStyle3;
            this.DtlName.HeaderText = "明细名称";
            this.DtlName.Name = "DtlName";
            this.DtlName.ReadOnly = true;
            this.DtlName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlName.Width = 120;
            // 
            // colContractGroupName
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.colContractGroupName.DefaultCellStyle = dataGridViewCellStyle4;
            this.colContractGroupName.HeaderText = "契约名称";
            this.colContractGroupName.Name = "colContractGroupName";
            this.colContractGroupName.ReadOnly = true;
            this.colContractGroupName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DtlMainResourceType
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            this.DtlMainResourceType.DefaultCellStyle = dataGridViewCellStyle5;
            this.DtlMainResourceType.HeaderText = "主资源类型";
            this.DtlMainResourceType.Name = "DtlMainResourceType";
            this.DtlMainResourceType.ReadOnly = true;
            this.DtlMainResourceType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlMainResourceType.Visible = false;
            this.DtlMainResourceType.Width = 90;
            // 
            // DtlMainResourceTypeSpec
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            this.DtlMainResourceTypeSpec.DefaultCellStyle = dataGridViewCellStyle6;
            this.DtlMainResourceTypeSpec.HeaderText = "规格型号";
            this.DtlMainResourceTypeSpec.Name = "DtlMainResourceTypeSpec";
            this.DtlMainResourceTypeSpec.ReadOnly = true;
            this.DtlMainResourceTypeSpec.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlMainResourceTypeSpec.Visible = false;
            this.DtlMainResourceTypeSpec.Width = 80;
            // 
            // DiagramNumber
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            this.DiagramNumber.DefaultCellStyle = dataGridViewCellStyle7;
            this.DiagramNumber.HeaderText = "图号";
            this.DiagramNumber.Name = "DiagramNumber";
            this.DiagramNumber.ReadOnly = true;
            this.DiagramNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DiagramNumber.Width = 80;
            // 
            // DtlQuantityUnit
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            this.DtlQuantityUnit.DefaultCellStyle = dataGridViewCellStyle8;
            this.DtlQuantityUnit.HeaderText = "工程量单位";
            this.DtlQuantityUnit.Name = "DtlQuantityUnit";
            this.DtlQuantityUnit.ReadOnly = true;
            this.DtlQuantityUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlQuantityUnit.Width = 90;
            // 
            // DtlPlanQuantity
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            this.DtlPlanQuantity.DefaultCellStyle = dataGridViewCellStyle9;
            this.DtlPlanQuantity.HeaderText = "计划工程量";
            this.DtlPlanQuantity.Name = "DtlPlanQuantity";
            this.DtlPlanQuantity.ReadOnly = true;
            this.DtlPlanQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlPlanQuantity.Width = 90;
            // 
            // DtlPlanPrice
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            this.DtlPlanPrice.DefaultCellStyle = dataGridViewCellStyle10;
            this.DtlPlanPrice.HeaderText = "计划单价";
            this.DtlPlanPrice.Name = "DtlPlanPrice";
            this.DtlPlanPrice.ReadOnly = true;
            this.DtlPlanPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlPlanPrice.Width = 80;
            // 
            // DtlPlanTotalPrice
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            this.DtlPlanTotalPrice.DefaultCellStyle = dataGridViewCellStyle11;
            this.DtlPlanTotalPrice.HeaderText = "计划合价";
            this.DtlPlanTotalPrice.Name = "DtlPlanTotalPrice";
            this.DtlPlanTotalPrice.ReadOnly = true;
            this.DtlPlanTotalPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlPlanTotalPrice.Width = 80;
            // 
            // DtlPriceUnit
            // 
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            this.DtlPriceUnit.DefaultCellStyle = dataGridViewCellStyle12;
            this.DtlPriceUnit.HeaderText = "价格单位";
            this.DtlPriceUnit.Name = "DtlPriceUnit";
            this.DtlPriceUnit.ReadOnly = true;
            this.DtlPriceUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlPriceUnit.Width = 70;
            // 
            // DtlState
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            this.DtlState.DefaultCellStyle = dataGridViewCellStyle13;
            this.DtlState.HeaderText = "状态";
            this.DtlState.Name = "DtlState";
            this.DtlState.ReadOnly = true;
            this.DtlState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlState.Width = 60;
            // 
            // DtlDesc
            // 
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.White;
            this.DtlDesc.DefaultCellStyle = dataGridViewCellStyle14;
            this.DtlDesc.HeaderText = "说明";
            this.DtlDesc.Name = "DtlDesc";
            this.DtlDesc.ReadOnly = true;
            this.DtlDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DtlMainResourceTypeQuality
            // 
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            this.DtlMainResourceTypeQuality.DefaultCellStyle = dataGridViewCellStyle15;
            this.DtlMainResourceTypeQuality.HeaderText = "材质";
            this.DtlMainResourceTypeQuality.Name = "DtlMainResourceTypeQuality";
            this.DtlMainResourceTypeQuality.ReadOnly = true;
            this.DtlMainResourceTypeQuality.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlMainResourceTypeQuality.Visible = false;
            this.DtlMainResourceTypeQuality.Width = 70;
            // 
            // DtlCostItem
            // 
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.White;
            this.DtlCostItem.DefaultCellStyle = dataGridViewCellStyle16;
            this.DtlCostItem.FillWeight = 80F;
            this.DtlCostItem.HeaderText = "成本项名称";
            this.DtlCostItem.Name = "DtlCostItem";
            this.DtlCostItem.ReadOnly = true;
            this.DtlCostItem.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DtlCostItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlCostItem.Visible = false;
            this.DtlCostItem.Width = 120;
            // 
            // DtlOrderNo
            // 
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.White;
            this.DtlOrderNo.DefaultCellStyle = dataGridViewCellStyle17;
            this.DtlOrderNo.HeaderText = "显示序号";
            this.DtlOrderNo.Name = "DtlOrderNo";
            this.DtlOrderNo.ReadOnly = true;
            this.DtlOrderNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DtlOrderNo.Visible = false;
            this.DtlOrderNo.Width = 80;
            // 
            // VGWBSTreeDetailSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 430);
            this.Name = "VGWBSTreeDetailSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择节点明细";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGWBDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridGWBDetail;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ckb;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCostItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContractGroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlMainResourceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlMainResourceTypeSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiagramNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlQuantityUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlPlanQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlPlanPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlPlanTotalPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlPriceUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlState;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlMainResourceTypeQuality;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlCostItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn DtlOrderNo;
    }
}