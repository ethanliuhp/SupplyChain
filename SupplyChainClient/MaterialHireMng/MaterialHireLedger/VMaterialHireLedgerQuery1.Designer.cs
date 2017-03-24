namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireLedger
{
    partial class VMaterialHireLedgerQuery1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colOperDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOriContractNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplyInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCollQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReturnQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBroachQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepairQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscardQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRejectQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDamageQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConsumeQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLeftQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBalState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSystemDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbNoBal = new System.Windows.Forms.RadioButton();
            this.rbBal = new System.Windows.Forms.RadioButton();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtUsedBank = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtSpec = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMaterial = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial();
            this.txtOriContractNo = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupplier = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.业务日期 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsedBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(1015, 532);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(482, 20);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgDetail);
            this.groupBox2.Location = new System.Drawing.Point(7, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1001, 428);
            this.groupBox2.TabIndex = 109;
            this.groupBox2.TabStop = false;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOperDate,
            this.colType,
            this.colCode,
            this.colOriContractNo,
            this.colSupplyInfo,
            this.colRankName,
            this.colMaterialCode,
            this.colMaterialName,
            this.colSpec,
            this.colCollQuantity,
            this.colReturnQty,
            this.colBroachQty,
            this.colRepairQty,
            this.colDiscardQty,
            this.colRejectQty,
            this.colDamageQty,
            this.colConsumeQty,
            this.colLeftQuantity,
            this.colPrice,
            this.colBalState,
            this.colUnit,
            this.colSystemDate});
            this.dgDetail.CustomBackColor = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDetail.DefaultCellStyle = dataGridViewCellStyle2;
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
            this.dgDetail.Location = new System.Drawing.Point(3, 17);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.PageIndex = 0;
            this.dgDetail.PageSize = 0;
            this.dgDetail.Query = null;
            this.dgDetail.ReadOnlyCols = null;
            this.dgDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgDetail.RowHeadersVisible = false;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetail.Size = new System.Drawing.Size(992, 405);
            this.dgDetail.TabIndex = 55;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colOperDate
            // 
            this.colOperDate.HeaderText = "业务日期";
            this.colOperDate.Name = "colOperDate";
            this.colOperDate.Width = 80;
            // 
            // colType
            // 
            this.colType.HeaderText = "类型";
            this.colType.Name = "colType";
            this.colType.Width = 60;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "单据号";
            this.colCode.Name = "colCode";
            this.colCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCode.Width = 80;
            // 
            // colOriContractNo
            // 
            this.colOriContractNo.HeaderText = "租赁合同号";
            this.colOriContractNo.Name = "colOriContractNo";
            // 
            // colSupplyInfo
            // 
            this.colSupplyInfo.HeaderText = "出租方";
            this.colSupplyInfo.Name = "colSupplyInfo";
            // 
            // colRankName
            // 
            this.colRankName.HeaderText = "队伍";
            this.colRankName.Name = "colRankName";
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.HeaderText = "物料编码";
            this.colMaterialCode.Name = "colMaterialCode";
            this.colMaterialCode.Width = 80;
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
            // colCollQuantity
            // 
            this.colCollQuantity.HeaderText = "收料数量";
            this.colCollQuantity.Name = "colCollQuantity";
            this.colCollQuantity.Width = 80;
            // 
            // colReturnQty
            // 
            this.colReturnQty.HeaderText = "退料数量";
            this.colReturnQty.Name = "colReturnQty";
            // 
            // colBroachQty
            // 
            this.colBroachQty.HeaderText = "完好";
            this.colBroachQty.Name = "colBroachQty";
            // 
            // colRepairQty
            // 
            this.colRepairQty.HeaderText = "维修";
            this.colRepairQty.Name = "colRepairQty";
            // 
            // colDiscardQty
            // 
            this.colDiscardQty.HeaderText = "切头";
            this.colDiscardQty.Name = "colDiscardQty";
            // 
            // colRejectQty
            // 
            this.colRejectQty.HeaderText = "报废";
            this.colRejectQty.Name = "colRejectQty";
            // 
            // colDamageQty
            // 
            this.colDamageQty.HeaderText = "报损";
            this.colDamageQty.Name = "colDamageQty";
            // 
            // colConsumeQty
            // 
            this.colConsumeQty.HeaderText = "消耗";
            this.colConsumeQty.Name = "colConsumeQty";
            // 
            // colLeftQuantity
            // 
            this.colLeftQuantity.HeaderText = "结存数量";
            this.colLeftQuantity.Name = "colLeftQuantity";
            // 
            // colPrice
            // 
            this.colPrice.HeaderText = "租赁单价";
            this.colPrice.Name = "colPrice";
            this.colPrice.Width = 80;
            // 
            // colBalState
            // 
            this.colBalState.HeaderText = "计算状态";
            this.colBalState.Name = "colBalState";
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            // 
            // colSystemDate
            // 
            this.colSystemDate.HeaderText = "台账日期";
            this.colSystemDate.Name = "colSystemDate";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rbAll);
            this.groupBox1.Controls.Add(this.rbNoBal);
            this.groupBox1.Controls.Add(this.rbBal);
            this.groupBox1.Controls.Add(this.customLabel9);
            this.groupBox1.Controls.Add(this.txtUsedBank);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.txtMaterial);
            this.groupBox1.Controls.Add(this.txtOriContractNo);
            this.groupBox1.Controls.Add(this.customLabel8);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.txtSupplier);
            this.groupBox1.Controls.Add(this.业务日期);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Location = new System.Drawing.Point(6, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1001, 83);
            this.groupBox1.TabIndex = 108;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(707, 54);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(47, 16);
            this.rbAll.TabIndex = 115;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "全部";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbNoBal
            // 
            this.rbNoBal.AutoSize = true;
            this.rbNoBal.Location = new System.Drawing.Point(642, 54);
            this.rbNoBal.Name = "rbNoBal";
            this.rbNoBal.Size = new System.Drawing.Size(59, 16);
            this.rbNoBal.TabIndex = 114;
            this.rbNoBal.Text = "未结算";
            this.rbNoBal.UseVisualStyleBackColor = true;
            // 
            // rbBal
            // 
            this.rbBal.AutoSize = true;
            this.rbBal.Location = new System.Drawing.Point(577, 54);
            this.rbBal.Name = "rbBal";
            this.rbBal.Size = new System.Drawing.Size(59, 16);
            this.rbBal.TabIndex = 113;
            this.rbBal.Text = "已结算";
            this.rbBal.UseVisualStyleBackColor = true;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(328, 54);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(59, 12);
            this.customLabel9.TabIndex = 111;
            this.customLabel9.Text = "使用队伍:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtUsedBank
            // 
            this.txtUsedBank.BackColor = System.Drawing.SystemColors.Control;
            this.txtUsedBank.Code = null;
            this.txtUsedBank.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtUsedBank.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtUsedBank.EnterToTab = false;
            this.txtUsedBank.Id = "";
            this.txtUsedBank.Location = new System.Drawing.Point(398, 49);
            this.txtUsedBank.Name = "txtUsedBank";
            this.txtUsedBank.Result = null;
            this.txtUsedBank.RightMouse = false;
            this.txtUsedBank.Size = new System.Drawing.Size(153, 21);
            this.txtUsedBank.TabIndex = 110;
            this.txtUsedBank.Tag = null;
            this.txtUsedBank.Value = "";
            this.txtUsedBank.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(894, 47);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 103;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(802, 47);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 102;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.SystemColors.Control;
            this.txtSpec.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSpec.DrawSelf = false;
            this.txtSpec.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSpec.EnterToTab = false;
            this.txtSpec.Location = new System.Drawing.Point(640, 20);
            this.txtSpec.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Padding = new System.Windows.Forms.Padding(1);
            this.txtSpec.ReadOnly = false;
            this.txtSpec.Size = new System.Drawing.Size(105, 16);
            this.txtSpec.TabIndex = 101;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(575, 24);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 109;
            this.customLabel4.Text = "规格型号:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
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
            this.txtMaterial.Location = new System.Drawing.Point(397, 20);
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Result = null;
            this.txtMaterial.RightMouse = false;
            this.txtMaterial.Size = new System.Drawing.Size(154, 21);
            this.txtMaterial.TabIndex = 100;
            this.txtMaterial.Tag = null;
            this.txtMaterial.Value = "";
            this.txtMaterial.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // txtOriContractNo
            // 
            this.txtOriContractNo.BackColor = System.Drawing.SystemColors.Control;
            this.txtOriContractNo.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOriContractNo.DrawSelf = false;
            this.txtOriContractNo.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOriContractNo.EnterToTab = false;
            this.txtOriContractNo.Location = new System.Drawing.Point(848, 20);
            this.txtOriContractNo.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOriContractNo.Name = "txtOriContractNo";
            this.txtOriContractNo.Padding = new System.Windows.Forms.Padding(1);
            this.txtOriContractNo.ReadOnly = false;
            this.txtOriContractNo.Size = new System.Drawing.Size(121, 16);
            this.txtOriContractNo.TabIndex = 99;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(355, 25);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(35, 12);
            this.customLabel8.TabIndex = 106;
            this.customLabel8.Text = "物资:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(771, 23);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(71, 12);
            this.customLabel3.TabIndex = 108;
            this.customLabel3.Text = "租赁合同号:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(20, 54);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(47, 12);
            this.customLabel2.TabIndex = 107;
            this.customLabel2.Text = "出租方:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSupplier
            // 
            this.txtSupplier.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupplier.Code = null;
            this.txtSupplier.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtSupplier.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtSupplier.EnterToTab = false;
            this.txtSupplier.Id = "";
            this.txtSupplier.Location = new System.Drawing.Point(82, 49);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Result = null;
            this.txtSupplier.RightMouse = false;
            this.txtSupplier.Size = new System.Drawing.Size(231, 21);
            this.txtSupplier.TabIndex = 98;
            this.txtSupplier.Tag = null;
            this.txtSupplier.Value = "";
            this.txtSupplier.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // 业务日期
            // 
            this.业务日期.AutoSize = true;
            this.业务日期.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.业务日期.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.业务日期.Location = new System.Drawing.Point(12, 27);
            this.业务日期.Name = "业务日期";
            this.业务日期.Size = new System.Drawing.Size(59, 12);
            this.业务日期.TabIndex = 104;
            this.业务日期.Text = "业务日期:";
            this.业务日期.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(77, 20);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 96;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(209, 20);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 97;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(192, 27);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 105;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VMaterialHireLedgerQuery1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 532);
            this.Name = "VMaterialHireLedgerQuery1";
            this.Text = "料具租赁台账";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsedBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOriContractNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplyInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRankName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCollQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReturnQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBroachQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepairQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscardQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRejectQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDamageQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConsumeQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLeftQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBalState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSystemDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbNoBal;
        private System.Windows.Forms.RadioButton rbBal;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtUsedBank;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSpec;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial txtMaterial;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOriContractNo;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupplier;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel 业务日期;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
    }
}