namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI
{
    partial class VStockSelectMaterial
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMatSpec = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMaterialCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMatrailName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSure = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnClose = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAllSelect = new System.Windows.Forms.CheckBox();
            this.chkUnSelect = new System.Windows.Forms.CheckBox();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialStuff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMaterialDiagramNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRemainQuality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.chkUnSelect);
            this.pnlFloor.Controls.Add(this.chkAllSelect);
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(1040, 504);
            this.pnlFloor.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFloor_Paint);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.chkAllSelect, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.chkUnSelect, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -8);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtMatSpec);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.txtMaterialCode);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.txtMatrailName);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.customLabel8);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1016, 44);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtMatSpec
            // 
            this.txtMatSpec.BackColor = System.Drawing.SystemColors.Control;
            this.txtMatSpec.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMatSpec.DrawSelf = false;
            this.txtMatSpec.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMatSpec.EnterToTab = false;
            this.txtMatSpec.Location = new System.Drawing.Point(514, 13);
            this.txtMatSpec.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMatSpec.Name = "txtMatSpec";
            this.txtMatSpec.Padding = new System.Windows.Forms.Padding(1);
            this.txtMatSpec.ReadOnly = false;
            this.txtMatSpec.Size = new System.Drawing.Size(161, 16);
            this.txtMatSpec.TabIndex = 89;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(473, 17);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(35, 12);
            this.customLabel2.TabIndex = 88;
            this.customLabel2.Text = "规格:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtMaterialCode
            // 
            this.txtMaterialCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterialCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMaterialCode.DrawSelf = false;
            this.txtMaterialCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMaterialCode.EnterToTab = false;
            this.txtMaterialCode.Location = new System.Drawing.Point(292, 13);
            this.txtMaterialCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMaterialCode.Name = "txtMaterialCode";
            this.txtMaterialCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtMaterialCode.ReadOnly = false;
            this.txtMaterialCode.Size = new System.Drawing.Size(161, 16);
            this.txtMaterialCode.TabIndex = 87;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(251, 17);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(35, 12);
            this.customLabel1.TabIndex = 86;
            this.customLabel1.Text = "编码:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtMatrailName
            // 
            this.txtMatrailName.BackColor = System.Drawing.SystemColors.Control;
            this.txtMatrailName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMatrailName.DrawSelf = false;
            this.txtMatrailName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMatrailName.EnterToTab = false;
            this.txtMatrailName.Location = new System.Drawing.Point(47, 13);
            this.txtMatrailName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMatrailName.Name = "txtMatrailName";
            this.txtMatrailName.Padding = new System.Windows.Forms.Padding(1);
            this.txtMatrailName.ReadOnly = false;
            this.txtMatrailName.Size = new System.Drawing.Size(161, 16);
            this.txtMatrailName.TabIndex = 85;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(737, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(6, 17);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(35, 12);
            this.customLabel8.TabIndex = 84;
            this.customLabel8.Text = "名称:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSure
            // 
            this.btnSure.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSure.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSure.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSure.Location = new System.Drawing.Point(6, 11);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 149;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnClose.Location = new System.Drawing.Point(178, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 150;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
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
            this.colSelect,
            this.colMaterialName,
            this.colMaterialCode,
            this.colMaterialSpec,
            this.colMaterialStuff,
            this.colUnit,
            this.ColMaterialDiagramNum,
            this.ColRemainQuality});
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
            this.dgDetail.Location = new System.Drawing.Point(12, 55);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.PageIndex = 0;
            this.dgDetail.PageSize = 0;
            this.dgDetail.Query = null;
            this.dgDetail.ReadOnlyCols = null;
            this.dgDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.RowHeadersVisible = false;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetail.Size = new System.Drawing.Size(1016, 403);
            this.dgDetail.TabIndex = 3;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnSure);
            this.groupBox2.Location = new System.Drawing.Point(695, 464);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 37);
            this.groupBox2.TabIndex = 101;
            this.groupBox2.TabStop = false;
            // 
            // chkAllSelect
            // 
            this.chkAllSelect.AutoSize = true;
            this.chkAllSelect.Location = new System.Drawing.Point(20, 475);
            this.chkAllSelect.Name = "chkAllSelect";
            this.chkAllSelect.Size = new System.Drawing.Size(48, 16);
            this.chkAllSelect.TabIndex = 102;
            this.chkAllSelect.Text = "全选";
            this.chkAllSelect.UseVisualStyleBackColor = true;
            // 
            // chkUnSelect
            // 
            this.chkUnSelect.AutoSize = true;
            this.chkUnSelect.Location = new System.Drawing.Point(104, 475);
            this.chkUnSelect.Name = "chkUnSelect";
            this.chkUnSelect.Size = new System.Drawing.Size(48, 16);
            this.chkUnSelect.TabIndex = 103;
            this.chkUnSelect.Text = "反选";
            this.chkUnSelect.UseVisualStyleBackColor = true;
            // 
            // colSelect
            // 
            this.colSelect.FalseValue = "false";
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.TrueValue = "true";
            this.colSelect.Width = 50;
            // 
            // colMaterialName
            // 
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            this.colMaterialName.ReadOnly = true;
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.HeaderText = "编码";
            this.colMaterialCode.Name = "colMaterialCode";
            this.colMaterialCode.ReadOnly = true;
            // 
            // colMaterialSpec
            // 
            this.colMaterialSpec.HeaderText = "规格";
            this.colMaterialSpec.Name = "colMaterialSpec";
            this.colMaterialSpec.ReadOnly = true;
            // 
            // colMaterialStuff
            // 
            this.colMaterialStuff.HeaderText = "材质";
            this.colMaterialStuff.Name = "colMaterialStuff";
            this.colMaterialStuff.ReadOnly = true;
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "单位";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            // 
            // ColMaterialDiagramNum
            // 
            this.ColMaterialDiagramNum.HeaderText = "图号";
            this.ColMaterialDiagramNum.Name = "ColMaterialDiagramNum";
            // 
            // ColRemainQuality
            // 
            this.ColRemainQuality.HeaderText = "库存数量";
            this.ColRemainQuality.Name = "ColRemainQuality";
            this.ColRemainQuality.ReadOnly = true;
            // 
            // VStockSelectMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 504);
            this.Name = "VStockSelectMaterial";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSure;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMaterialCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMatrailName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMatSpec;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.CheckBox chkUnSelect;
        private System.Windows.Forms.CheckBox chkAllSelect;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialStuff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMaterialDiagramNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRemainQuality;
    }
}