namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalLedgerMng
{
    partial class VMaterialRenLedSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMaterialRenLedSelector));
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBorrowUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStationQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPart = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtBorrowUnit = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSpec = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMaterialName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMaterialCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Size = new System.Drawing.Size(857, 456);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(127, 1);
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.Text = "料具台账引用";
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.AllowUserToResizeColumns = false;
            this.dgDetail.AllowUserToResizeRows = false;
            this.dgDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colMaterialCode,
            this.colMaterialName,
            this.colSpec,
            this.colBorrowUnit,
            this.colPart,
            this.colSubject,
            this.colStationQuantity,
            this.colUnit});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(9, 55);
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
            this.dgDetail.Size = new System.Drawing.Size(845, 362);
            this.dgDetail.TabIndex = 15;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 53;
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.HeaderText = "物料编码";
            this.colMaterialCode.Name = "colMaterialCode";
            this.colMaterialCode.Width = 77;
            // 
            // colMaterialName
            // 
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            this.colMaterialName.Width = 77;
            // 
            // colSpec
            // 
            this.colSpec.HeaderText = "规格型号";
            this.colSpec.Name = "colSpec";
            this.colSpec.Width = 77;
            // 
            // colBorrowUnit
            // 
            this.colBorrowUnit.HeaderText = "使用队伍";
            this.colBorrowUnit.Name = "colBorrowUnit";
            this.colBorrowUnit.Width = 77;
            // 
            // colPart
            // 
            this.colPart.HeaderText = "使用部位";
            this.colPart.Name = "colPart";
            this.colPart.Width = 77;
            // 
            // colSubject
            // 
            this.colSubject.HeaderText = "核算科目";
            this.colSubject.Name = "colSubject";
            this.colSubject.Width = 77;
            // 
            // colStationQuantity
            // 
            this.colStationQuantity.HeaderText = "库存数量";
            this.colStationQuantity.Name = "colStationQuantity";
            this.colStationQuantity.Width = 77;
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            this.colUnit.Width = 77;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(570, 425);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtPart);
            this.groupBox1.Controls.Add(this.customLabel5);
            this.groupBox1.Controls.Add(this.txtBorrowUnit);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.txtMaterialName);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.txtMaterialCode);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Location = new System.Drawing.Point(9, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(848, 46);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // txtPart
            // 
            this.txtPart.BackColor = System.Drawing.SystemColors.Control;
            this.txtPart.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPart.DrawSelf = false;
            this.txtPart.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPart.EnterToTab = false;
            this.txtPart.Location = new System.Drawing.Point(648, 19);
            this.txtPart.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPart.Name = "txtPart";
            this.txtPart.Padding = new System.Windows.Forms.Padding(1);
            this.txtPart.ReadOnly = false;
            this.txtPart.Size = new System.Drawing.Size(70, 16);
            this.txtPart.TabIndex = 100;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(591, 22);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(59, 12);
            this.customLabel5.TabIndex = 101;
            this.customLabel5.Text = "使用部位:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtBorrowUnit
            // 
            this.txtBorrowUnit.BackColor = System.Drawing.SystemColors.Control;
            this.txtBorrowUnit.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtBorrowUnit.DrawSelf = false;
            this.txtBorrowUnit.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtBorrowUnit.EnterToTab = false;
            this.txtBorrowUnit.Location = new System.Drawing.Point(497, 18);
            this.txtBorrowUnit.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtBorrowUnit.Name = "txtBorrowUnit";
            this.txtBorrowUnit.Padding = new System.Windows.Forms.Padding(1);
            this.txtBorrowUnit.ReadOnly = false;
            this.txtBorrowUnit.Size = new System.Drawing.Size(90, 16);
            this.txtBorrowUnit.TabIndex = 98;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(440, 21);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 99;
            this.customLabel4.Text = "使用队伍:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.SystemColors.Control;
            this.txtSpec.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSpec.DrawSelf = false;
            this.txtSpec.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSpec.EnterToTab = false;
            this.txtSpec.Location = new System.Drawing.Point(371, 18);
            this.txtSpec.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Padding = new System.Windows.Forms.Padding(1);
            this.txtSpec.ReadOnly = false;
            this.txtSpec.Size = new System.Drawing.Size(60, 16);
            this.txtSpec.TabIndex = 96;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(316, 21);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 97;
            this.customLabel2.Text = "规格型号:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtMaterialName
            // 
            this.txtMaterialName.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterialName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMaterialName.DrawSelf = false;
            this.txtMaterialName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMaterialName.EnterToTab = false;
            this.txtMaterialName.Location = new System.Drawing.Point(227, 18);
            this.txtMaterialName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMaterialName.Name = "txtMaterialName";
            this.txtMaterialName.Padding = new System.Windows.Forms.Padding(1);
            this.txtMaterialName.ReadOnly = false;
            this.txtMaterialName.Size = new System.Drawing.Size(85, 16);
            this.txtMaterialName.TabIndex = 94;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(166, 20);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 95;
            this.customLabel1.Text = "物料名称:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterialCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMaterialCode.DrawSelf = false;
            this.txtMaterialCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMaterialCode.EnterToTab = false;
            this.txtMaterialCode.Location = new System.Drawing.Point(77, 19);
            this.txtMaterialCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtMaterialCode.ReadOnly = false;
            this.txtMaterialCode.Size = new System.Drawing.Size(87, 16);
            this.txtMaterialCode.TabIndex = 92;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(15, 22);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 93;
            this.customLabel3.Text = "物料编码:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(730, 15);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(473, 425);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // VMaterialRenLedSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 456);
            this.Name = "VMaterialRenLedSelector";
            this.Text = "料具台账引用";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSpec;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMaterialName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMaterialCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBorrowUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStationQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtBorrowUnit;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPart;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;

    }
}