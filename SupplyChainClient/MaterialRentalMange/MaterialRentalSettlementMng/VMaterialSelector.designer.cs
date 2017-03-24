namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng
{
    partial class VMaterialSelector
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMaterialSelector));
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCostName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSettleSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantityUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRentalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRentalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSettleMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnDelete);
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Size = new System.Drawing.Size(779, 342);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnDelete, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(95, 9);
            this.lblTitle.Size = new System.Drawing.Size(198, 20);
            this.lblTitle.Text = "结算分科目费用明细";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(636, 314);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(473, 314);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgDetail);
            this.panel1.Location = new System.Drawing.Point(0, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(779, 276);
            this.panel1.TabIndex = 12;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = true;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCostName,
            this.colSettleSubject,
            this.colMaterialCode,
            this.colQuantity,
            this.colQuantityUnit,
            this.colRentalDate,
            this.colDateUnit,
            this.colRentalPrice,
            this.colSettleMoney,
            this.colDescription});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(0, 0);
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
            this.dgDetail.Size = new System.Drawing.Size(779, 276);
            this.dgDetail.TabIndex = 8;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            this.dgDetail.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgDetail_CellMouseDown);
            // 
            // colCostName
            // 
            this.colCostName.HeaderText = "费用名称";
            this.colCostName.Name = "colCostName";
            this.colCostName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCostName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colSettleSubject
            // 
            this.colSettleSubject.HeaderText = "结算科目";
            this.colSettleSubject.Name = "colSettleSubject";
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.HeaderText = "资源类型";
            this.colMaterialCode.Name = "colMaterialCode";
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "数量";
            this.colQuantity.Name = "colQuantity";
            // 
            // colQuantityUnit
            // 
            this.colQuantityUnit.HeaderText = "数量单位";
            this.colQuantityUnit.Name = "colQuantityUnit";
            // 
            // colRentalDate
            // 
            this.colRentalDate.HeaderText = "租赁时间";
            this.colRentalDate.Name = "colRentalDate";
            // 
            // colDateUnit
            // 
            this.colDateUnit.HeaderText = "时间单位";
            this.colDateUnit.Name = "colDateUnit";
            // 
            // colRentalPrice
            // 
            this.colRentalPrice.HeaderText = "租赁单价";
            this.colRentalPrice.Name = "colRentalPrice";
            // 
            // colSettleMoney
            // 
            this.colSettleMoney.HeaderText = "结算合价";
            this.colSettleMoney.Name = "colSettleMoney";
            // 
            // colDescription
            // 
            this.colDescription.HeaderText = "备注";
            this.colDescription.Name = "colDescription";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.ToolTipText = "删除当前选中的记录";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDelete.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnDelete.Location = new System.Drawing.Point(554, 314);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // VMaterialSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 342);
            this.MaximizeBox = false;
            this.Name = "VMaterialSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "结算分科目费用明细";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
        private System.Windows.Forms.Panel panel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnDelete;
        private System.Windows.Forms.DataGridViewComboBoxColumn colCostName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSettleSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantityUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRentalDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRentalPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSettleMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;

    }
}