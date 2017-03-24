namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    partial class VMaterialHireSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMaterialHireSelector));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnQuery = new AuthManager.AuthMng.AuthControlsMng.AuthButton();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdMaterial = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new AuthManager.AuthMng.AuthControlsMng.AuthButton();
            this.btnOK = new AuthManager.AuthMng.AuthControlsMng.AuthButton();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMaterial)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Controls.Add(this.txtCode);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.txtName);
            this.pnlFloor.Size = new System.Drawing.Size(632, 394);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtCode, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.AutoProgress = true;
            this.btnQuery.ButtonCode = "G_Search";
            this.btnQuery.ButtonText = "查找";
            this.btnQuery.DealMethod = AuthManager.AuthMng.AuthControlsMng.DealMethodEnm.Enable;
            this.btnQuery.EnableAuth = false;
            this.btnQuery.Location = new System.Drawing.Point(390, 27);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.ParentCode = null;
            this.btnQuery.Size = new System.Drawing.Size(70, 23);
            this.btnQuery.Status = "正在处理,请稍后...";
            this.btnQuery.TabIndex = 122;
            this.btnQuery.Text = "查找";
            this.btnQuery.UsersCode = "";
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(210, 37);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(41, 12);
            this.customLabel1.TabIndex = 119;
            this.customLabel1.Text = "编码：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(257, 33);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = false;
            this.txtCode.Size = new System.Drawing.Size(100, 16);
            this.txtCode.TabIndex = 121;
            // 
            // label1
            // 
            this.label1.AddColonAuto = true;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.label1.Location = new System.Drawing.Point(14, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 118;
            this.label1.Text = "名称：";
            this.label1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtName
            // 
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = false;
            this.txtName.Location = new System.Drawing.Point(57, 33);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = false;
            this.txtName.Size = new System.Drawing.Size(147, 16);
            this.txtName.TabIndex = 120;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdMaterial);
            this.groupBox1.Location = new System.Drawing.Point(6, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(621, 284);
            this.groupBox1.TabIndex = 123;
            this.groupBox1.TabStop = false;
            // 
            // grdMaterial
            // 
            this.grdMaterial.AddDefaultMenu = false;
            this.grdMaterial.AddNoColumn = false;
            this.grdMaterial.AllowUserToAddRows = false;
            this.grdMaterial.AllowUserToDeleteRows = false;
            this.grdMaterial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdMaterial.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdMaterial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdMaterial.CellBackColor = System.Drawing.SystemColors.Control;
            this.grdMaterial.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grdMaterial.ColumnHeadersHeight = 24;
            this.grdMaterial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdMaterial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colMaterialCode,
            this.colMaterialName,
            this.colSpec,
            this.colPrice,
            this.colUnit});
            this.grdMaterial.CustomBackColor = false;
            this.grdMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMaterial.EditCellBackColor = System.Drawing.Color.White;
            this.grdMaterial.EnableHeadersVisualStyles = false;
            this.grdMaterial.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.grdMaterial.FreezeFirstRow = false;
            this.grdMaterial.FreezeLastRow = false;
            this.grdMaterial.FrontColumnCount = 0;
            this.grdMaterial.GridColor = System.Drawing.SystemColors.WindowText;
            this.grdMaterial.HScrollOffset = 0;
            this.grdMaterial.IsAllowOrder = true;
            this.grdMaterial.IsConfirmDelete = true;
            this.grdMaterial.Location = new System.Drawing.Point(3, 17);
            this.grdMaterial.Name = "grdMaterial";
            this.grdMaterial.PageIndex = 0;
            this.grdMaterial.PageSize = 0;
            this.grdMaterial.Query = null;
            this.grdMaterial.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("grdMaterial.ReadOnlyCols")));
            this.grdMaterial.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grdMaterial.RowHeadersVisible = false;
            this.grdMaterial.RowHeadersWidth = 22;
            this.grdMaterial.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdMaterial.RowTemplate.Height = 23;
            this.grdMaterial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdMaterial.Size = new System.Drawing.Size(615, 264);
            this.grdMaterial.TabIndex = 5;
            this.grdMaterial.TargetType = null;
            this.grdMaterial.VScrollOffset = 0;
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
            // colPrice
            // 
            dataGridViewCellStyle2.Format = "N3";
            this.colPrice.DefaultCellStyle = dataGridViewCellStyle2;
            this.colPrice.HeaderText = "租赁单价";
            this.colPrice.Name = "colPrice";
            this.colPrice.Width = 77;
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            this.colUnit.Width = 77;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoProgress = false;
            this.btnCancel.ButtonCode = "G_Close";
            this.btnCancel.ButtonText = "取消";
            this.btnCancel.DealMethod = AuthManager.AuthMng.AuthControlsMng.DealMethodEnm.Enable;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.EnableAuth = false;
            this.btnCancel.Location = new System.Drawing.Point(502, 359);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ParentCode = null;
            this.btnCancel.Size = new System.Drawing.Size(70, 23);
            this.btnCancel.Status = "正在处理,请稍后...";
            this.btnCancel.TabIndex = 125;
            this.btnCancel.Text = "取消";
            this.btnCancel.UsersCode = "";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoProgress = false;
            this.btnOK.ButtonCode = "G_OK";
            this.btnOK.ButtonText = "确定(&K)";
            this.btnOK.DealMethod = AuthManager.AuthMng.AuthControlsMng.DealMethodEnm.Enable;
            this.btnOK.EnableAuth = false;
            this.btnOK.Location = new System.Drawing.Point(366, 359);
            this.btnOK.Name = "btnOK";
            this.btnOK.ParentCode = null;
            this.btnOK.Size = new System.Drawing.Size(70, 23);
            this.btnOK.Status = "正在处理,请稍后...";
            this.btnOK.TabIndex = 124;
            this.btnOK.Text = "确定(&K)";
            this.btnOK.UsersCode = "";
            // 
            // VMaterialHireSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 394);
            this.Name = "VMaterialHireSelector";
            this.Text = "料具选择";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMaterial)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AuthManager.AuthMng.AuthControlsMng.AuthButton btnQuery;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel label1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView grdMaterial;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private AuthManager.AuthMng.AuthControlsMng.AuthButton btnCancel;
        private AuthManager.AuthMng.AuthControlsMng.AuthButton btnOK;
    }
}