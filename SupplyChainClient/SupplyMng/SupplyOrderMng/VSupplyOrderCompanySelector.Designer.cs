namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng
{
    partial class VSupplyOrderCompanySelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSupplyOrderCompanySelector));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtSupplier = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMaterial = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCodeEnd = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtCodeBegin = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel17 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel18 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colSupplyContractNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterial)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Location = new System.Drawing.Point(0, 353);
            this.pnlFloor.Size = new System.Drawing.Size(820, 41);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(67, 9);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.txtSupplier);
            this.panel1.Controls.Add(this.customLabel9);
            this.panel1.Controls.Add(this.txtMaterial);
            this.panel1.Controls.Add(this.customLabel8);
            this.panel1.Controls.Add(this.customLabel1);
            this.panel1.Controls.Add(this.dtpDateBegin);
            this.panel1.Controls.Add(this.dtpDateEnd);
            this.panel1.Controls.Add(this.customLabel6);
            this.panel1.Controls.Add(this.txtCodeEnd);
            this.panel1.Controls.Add(this.txtCodeBegin);
            this.panel1.Controls.Add(this.customLabel17);
            this.panel1.Controls.Add(this.customLabel18);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(820, 77);
            this.panel1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(551, 40);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 106;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtSupplier
            // 
            this.txtSupplier.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupplier.Code = null;
            this.txtSupplier.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtSupplier.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.ComboBox;
            this.txtSupplier.EnterToTab = false;
            this.txtSupplier.Id = "";
            this.txtSupplier.Location = new System.Drawing.Point(59, 40);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Padding = new System.Windows.Forms.Padding(1);
            this.txtSupplier.Result = ((System.Collections.IList)(resources.GetObject("txtSupplier.Result")));
            this.txtSupplier.RightMouse = false;
            this.txtSupplier.Size = new System.Drawing.Size(199, 23);
            this.txtSupplier.TabIndex = 104;
            this.txtSupplier.Tag = null;
            this.txtSupplier.Value = "";
            this.txtSupplier.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(9, 45);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(47, 12);
            this.customLabel9.TabIndex = 105;
            this.customLabel9.Text = "供应商:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtMaterial
            // 
            this.txtMaterial.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterial.Code = null;
            this.txtMaterial.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtMaterial.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtMaterial.EnterToTab = false;
            this.txtMaterial.Id = "";
            this.txtMaterial.IsAllLoad = true;
            this.txtMaterial.IsCheckBox = false;
            this.txtMaterial.Location = new System.Drawing.Point(404, 42);
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Result = ((System.Collections.IList)(resources.GetObject("txtMaterial.Result")));
            this.txtMaterial.RightMouse = false;
            this.txtMaterial.Size = new System.Drawing.Size(128, 21);
            this.txtMaterial.TabIndex = 100;
            this.txtMaterial.Tag = null;
            this.txtMaterial.Value = "";
            this.txtMaterial.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(360, 45);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(35, 12);
            this.customLabel8.TabIndex = 103;
            this.customLabel8.Text = "物资:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(336, 18);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 101;
            this.customLabel1.Text = "业务日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(404, 13);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(106, 21);
            this.dtpDateBegin.TabIndex = 98;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(520, 13);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(106, 21);
            this.dtpDateEnd.TabIndex = 99;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(510, 18);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 102;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCodeEnd
            // 
            this.txtCodeEnd.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeEnd.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeEnd.DrawSelf = false;
            this.txtCodeEnd.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeEnd.EnterToTab = false;
            this.txtCodeEnd.Location = new System.Drawing.Point(169, 12);
            this.txtCodeEnd.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeEnd.Name = "txtCodeEnd";
            this.txtCodeEnd.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeEnd.ReadOnly = false;
            this.txtCodeEnd.Size = new System.Drawing.Size(89, 16);
            this.txtCodeEnd.TabIndex = 55;
            // 
            // txtCodeBegin
            // 
            this.txtCodeBegin.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBegin.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBegin.DrawSelf = false;
            this.txtCodeBegin.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBegin.EnterToTab = false;
            this.txtCodeBegin.Location = new System.Drawing.Point(59, 12);
            this.txtCodeBegin.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBegin.Name = "txtCodeBegin";
            this.txtCodeBegin.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBegin.ReadOnly = false;
            this.txtCodeBegin.Size = new System.Drawing.Size(101, 16);
            this.txtCodeBegin.TabIndex = 54;
            // 
            // customLabel17
            // 
            this.customLabel17.AutoSize = true;
            this.customLabel17.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel17.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel17.Location = new System.Drawing.Point(159, 15);
            this.customLabel17.Name = "customLabel17";
            this.customLabel17.Size = new System.Drawing.Size(11, 12);
            this.customLabel17.TabIndex = 56;
            this.customLabel17.Text = "-";
            this.customLabel17.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel18
            // 
            this.customLabel18.AutoSize = true;
            this.customLabel18.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel18.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel18.Location = new System.Drawing.Point(9, 16);
            this.customLabel18.Name = "customLabel18";
            this.customLabel18.Size = new System.Drawing.Size(47, 12);
            this.customLabel18.TabIndex = 57;
            this.customLabel18.Text = "单据号:";
            this.customLabel18.UnderLineColor = System.Drawing.Color.Red;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgDetail);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 77);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(820, 276);
            this.panel2.TabIndex = 1;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colSupplyContractNum,
            this.colSupplier,
            this.colState,
            this.colMaterialCode,
            this.colMaterialName,
            this.colSpec,
            this.colQuantity,
            this.colPrice,
            this.colSumMoney,
            this.colCreatePerson,
            this.colUnit,
            this.colCreateDate,
            this.colDescript});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.dgDetail.Location = new System.Drawing.Point(0, 0);
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
            this.dgDetail.Size = new System.Drawing.Size(820, 276);
            this.dgDetail.TabIndex = 4;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "单据号";
            this.colCode.Name = "colCode";
            this.colCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCode.Width = 80;
            // 
            // colSupplyContractNum
            // 
            this.colSupplyContractNum.HeaderText = "采购合同号";
            this.colSupplyContractNum.Name = "colSupplyContractNum";
            // 
            // colSupplier
            // 
            this.colSupplier.HeaderText = "供应商";
            this.colSupplier.Name = "colSupplier";
            // 
            // colState
            // 
            this.colState.HeaderText = "状态";
            this.colState.Name = "colState";
            this.colState.Width = 80;
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.HeaderText = "物料编码";
            this.colMaterialCode.Name = "colMaterialCode";
            // 
            // colMaterialName
            // 
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            this.colMaterialName.Width = 80;
            // 
            // colSpec
            // 
            this.colSpec.HeaderText = "规格型号";
            this.colSpec.Name = "colSpec";
            this.colSpec.Width = 80;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "数量";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Width = 80;
            // 
            // colPrice
            // 
            this.colPrice.HeaderText = "单价";
            this.colPrice.Name = "colPrice";
            this.colPrice.Width = 80;
            // 
            // colSumMoney
            // 
            this.colSumMoney.HeaderText = "金额";
            this.colSumMoney.Name = "colSumMoney";
            // 
            // colCreatePerson
            // 
            this.colCreatePerson.HeaderText = "制单人";
            this.colCreatePerson.Name = "colCreatePerson";
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            // 
            // colCreateDate
            // 
            this.colCreateDate.HeaderText = "业务日期";
            this.colCreateDate.Name = "colCreateDate";
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(691, 359);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(712, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 90;
            this.btnOK.Text = "确 定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // VSupplyOrderCompanySelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 394);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "VSupplyOrderCompanySelector";
            this.Text = "公司合同选择";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterial)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel17;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel18;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupplier;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial txtMaterial;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.DataGridViewLinkColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplyContractNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
    }
}