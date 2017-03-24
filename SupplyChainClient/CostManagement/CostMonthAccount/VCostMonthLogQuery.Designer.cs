namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    partial class VCostMonthLogQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCostMonthLogQuery));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtDescript = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtKjy = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtKjn = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtTaskNode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskNode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKjn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colKjy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRecordTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.lblRecordTotal);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(911, 498);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecordTotal, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -8);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.txtDescript);
            this.groupBox1.Controls.Add(this.txtKjy);
            this.groupBox1.Controls.Add(this.txtKjn);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtTaskNode);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.customLabel8);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(887, 42);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(113, 20);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(11, 12);
            this.customLabel3.TabIndex = 148;
            this.customLabel3.Text = "-";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtDescript
            // 
            this.txtDescript.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescript.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDescript.DrawSelf = false;
            this.txtDescript.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDescript.EnterToTab = false;
            this.txtDescript.Location = new System.Drawing.Point(470, 16);
            this.txtDescript.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDescript.Name = "txtDescript";
            this.txtDescript.Padding = new System.Windows.Forms.Padding(1);
            this.txtDescript.ReadOnly = false;
            this.txtDescript.Size = new System.Drawing.Size(72, 16);
            this.txtDescript.TabIndex = 97;
            // 
            // txtKjy
            // 
            this.txtKjy.BackColor = System.Drawing.SystemColors.Control;
            this.txtKjy.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtKjy.DrawSelf = false;
            this.txtKjy.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtKjy.EnterToTab = false;
            this.txtKjy.Location = new System.Drawing.Point(130, 16);
            this.txtKjy.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtKjy.Name = "txtKjy";
            this.txtKjy.Padding = new System.Windows.Forms.Padding(1);
            this.txtKjy.ReadOnly = false;
            this.txtKjy.Size = new System.Drawing.Size(53, 16);
            this.txtKjy.TabIndex = 96;
            // 
            // txtKjn
            // 
            this.txtKjn.BackColor = System.Drawing.SystemColors.Control;
            this.txtKjn.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtKjn.DrawSelf = false;
            this.txtKjn.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtKjn.EnterToTab = false;
            this.txtKjn.Location = new System.Drawing.Point(54, 16);
            this.txtKjn.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtKjn.Name = "txtKjn";
            this.txtKjn.Padding = new System.Windows.Forms.Padding(1);
            this.txtKjn.ReadOnly = false;
            this.txtKjn.Size = new System.Drawing.Size(53, 16);
            this.txtKjn.TabIndex = 94;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(750, 13);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 12;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(673, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtTaskNode
            // 
            this.txtTaskNode.BackColor = System.Drawing.SystemColors.Control;
            this.txtTaskNode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtTaskNode.DrawSelf = false;
            this.txtTaskNode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtTaskNode.EnterToTab = false;
            this.txtTaskNode.Location = new System.Drawing.Point(283, 16);
            this.txtTaskNode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtTaskNode.Name = "txtTaskNode";
            this.txtTaskNode.Padding = new System.Windows.Forms.Padding(1);
            this.txtTaskNode.ReadOnly = false;
            this.txtTaskNode.Size = new System.Drawing.Size(72, 16);
            this.txtTaskNode.TabIndex = 10;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(220, 19);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 93;
            this.customLabel4.Text = "核算节点:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(408, 19);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(59, 12);
            this.customLabel8.TabIndex = 84;
            this.customLabel8.Text = "日志描述:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(4, 18);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(47, 12);
            this.customLabel1.TabIndex = 80;
            this.customLabel1.Text = "会计期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumber,
            this.colTaskNode,
            this.colKjn,
            this.colKjy,
            this.colBillType,
            this.colDescript});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(12, 53);
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
            this.dgDetail.Size = new System.Drawing.Size(887, 414);
            this.dgDetail.TabIndex = 3;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colNumber
            // 
            this.colNumber.HeaderText = "顺序号";
            this.colNumber.Name = "colNumber";
            this.colNumber.Width = 70;
            // 
            // colTaskNode
            // 
            this.colTaskNode.HeaderText = "核算节点";
            this.colTaskNode.Name = "colTaskNode";
            // 
            // colKjn
            // 
            this.colKjn.HeaderText = "会计年";
            this.colKjn.Name = "colKjn";
            this.colKjn.Width = 70;
            // 
            // colKjy
            // 
            this.colKjy.HeaderText = "会计月";
            this.colKjy.Name = "colKjy";
            this.colKjy.Width = 70;
            // 
            // colBillType
            // 
            this.colBillType.HeaderText = "核算类型";
            this.colBillType.Name = "colBillType";
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "描述";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 400;
            // 
            // lblRecordTotal
            // 
            this.lblRecordTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecordTotal.AutoSize = true;
            this.lblRecordTotal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecordTotal.ForeColor = System.Drawing.Color.Red;
            this.lblRecordTotal.Location = new System.Drawing.Point(18, 477);
            this.lblRecordTotal.Name = "lblRecordTotal";
            this.lblRecordTotal.Size = new System.Drawing.Size(83, 12);
            this.lblRecordTotal.TabIndex = 97;
            this.lblRecordTotal.Text = "共【0】条记录";
            this.lblRecordTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VCostMonthLogQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 498);
            this.Name = "VCostMonthLogQuery";
            this.Text = "月度成本核算日志查询";
            this.ViewCaption = "";
            this.ViewName = "";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtTaskNode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecordTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtKjy;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtKjn;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskNode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKjn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKjy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
    }
}