namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng
{
    partial class VOwnerQuantitySearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VOwnerQuantitySearch));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelect = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtCodeBegin = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel18 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubmitSumMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCollectSumMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfirmSumMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConstractSumMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPriceUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(854, 498);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -8);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnSelect);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtCodeBegin);
            this.groupBox1.Controls.Add(this.customLabel18);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(830, 42);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnSelect
            // 
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelect.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSelect.Location = new System.Drawing.Point(201, 13);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(50, 23);
            this.btnSelect.TabIndex = 55;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // btnExcel
            // 
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(453, 13);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 54;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(345, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtCodeBegin
            // 
            this.txtCodeBegin.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBegin.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBegin.DrawSelf = false;
            this.txtCodeBegin.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBegin.EnterToTab = false;
            this.txtCodeBegin.Location = new System.Drawing.Point(104, 16);
            this.txtCodeBegin.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBegin.Name = "txtCodeBegin";
            this.txtCodeBegin.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBegin.ReadOnly = false;
            this.txtCodeBegin.Size = new System.Drawing.Size(104, 16);
            this.txtCodeBegin.TabIndex = 1;
            // 
            // customLabel18
            // 
            this.customLabel18.AutoSize = true;
            this.customLabel18.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel18.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel18.Location = new System.Drawing.Point(46, 20);
            this.customLabel18.Name = "customLabel18";
            this.customLabel18.Size = new System.Drawing.Size(53, 12);
            this.customLabel18.TabIndex = 53;
            this.customLabel18.Text = "清单WBS:";
            this.customLabel18.UnderLineColor = System.Drawing.Color.Red;
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
            this.colProject,
            this.colState,
            this.colSubmitSumMoney,
            this.colCollectSumMoney,
            this.colConfirmSumMoney,
            this.colLastUpdateDate,
            this.colConstractSumMoney,
            this.colPriceUnit});
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
            this.dgDetail.Size = new System.Drawing.Size(830, 433);
            this.dgDetail.TabIndex = 3;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colProject
            // 
            this.colProject.HeaderText = "所属项目";
            this.colProject.Name = "colProject";
            // 
            // colState
            // 
            this.colState.HeaderText = "状态";
            this.colState.Name = "colState";
            this.colState.Visible = false;
            this.colState.Width = 80;
            // 
            // colSubmitSumMoney
            // 
            this.colSubmitSumMoney.HeaderText = "报送累计金额";
            this.colSubmitSumMoney.Name = "colSubmitSumMoney";
            this.colSubmitSumMoney.Width = 130;
            // 
            // colCollectSumMoney
            // 
            this.colCollectSumMoney.HeaderText = "实际收款累计金额";
            this.colCollectSumMoney.Name = "colCollectSumMoney";
            this.colCollectSumMoney.Width = 150;
            // 
            // colConfirmSumMoney
            // 
            this.colConfirmSumMoney.HeaderText = "业主确认累计金额";
            this.colConfirmSumMoney.Name = "colConfirmSumMoney";
            this.colConfirmSumMoney.Width = 150;
            // 
            // colLastUpdateDate
            // 
            this.colLastUpdateDate.HeaderText = "最后更新时间";
            this.colLastUpdateDate.Name = "colLastUpdateDate";
            this.colLastUpdateDate.Width = 120;
            // 
            // colConstractSumMoney
            // 
            this.colConstractSumMoney.HeaderText = "合同总金额";
            this.colConstractSumMoney.Name = "colConstractSumMoney";
            this.colConstractSumMoney.Visible = false;
            // 
            // colPriceUnit
            // 
            this.colPriceUnit.HeaderText = "价格单位";
            this.colPriceUnit.Name = "colPriceUnit";
            // 
            // VOwnerQuantitySearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 498);
            this.Name = "VOwnerQuantitySearch";
            this.Text = "VMaterialRentalOrderQuery";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel18;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubmitSumMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCollectSumMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfirmSumMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastUpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConstractSumMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPriceUnit;
    }
}