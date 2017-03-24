namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng
{
    partial class VMonthlyPlanMng
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMonthlyPlanMng));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.txtMaterialCategory = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial();
            this.cboProfessionCat = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.lblCat = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.comPlanType = new System.Windows.Forms.ComboBox();
            this.txtHandlePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblHandlePerson = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealInQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNeedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlanQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiagramNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSpecailType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQualityStandard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApproachDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.colUsedPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsedRanks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colForwardBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantityTemp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsDg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDel = new System.Windows.Forms.ToolStripMenuItem();
            this.txtSumQuantity = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lbSumQuantity = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtSumMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlContent.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlFloor.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterialCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.cmsDg.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Size = new System.Drawing.Size(962, 441);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl1);
            this.pnlBody.Location = new System.Drawing.Point(0, 75);
            this.pnlBody.Size = new System.Drawing.Size(960, 326);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtSumMoney);
            this.pnlFooter.Controls.Add(this.customLabel2);
            this.pnlFooter.Controls.Add(this.txtProject);
            this.pnlFooter.Controls.Add(this.customLabel1);
            this.pnlFooter.Controls.Add(this.txtSumQuantity);
            this.pnlFooter.Controls.Add(this.lbSumQuantity);
            this.pnlFooter.Controls.Add(this.txtCreatePerson);
            this.pnlFooter.Controls.Add(this.customLabel5);
            this.pnlFooter.Location = new System.Drawing.Point(0, 401);
            this.pnlFooter.Size = new System.Drawing.Size(960, 38);
            // 
            // spTop
            // 
            this.spTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spTop.Size = new System.Drawing.Size(962, 441);
            this.spTop.Visible = false;
            // 
            // pnlFloor
            // 
            this.pnlFloor.Size = new System.Drawing.Size(962, 441);
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Size = new System.Drawing.Size(962, 441);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.groupSupply);
            this.pnlHeader.Size = new System.Drawing.Size(960, 75);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.groupSupply, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Visible = false;
            // 
            // groupSupply
            // 
            this.groupSupply.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSupply.Controls.Add(this.txtMaterialCategory);
            this.groupSupply.Controls.Add(this.cboProfessionCat);
            this.groupSupply.Controls.Add(this.lblCat);
            this.groupSupply.Controls.Add(this.comPlanType);
            this.groupSupply.Controls.Add(this.txtHandlePerson);
            this.groupSupply.Controls.Add(this.lblHandlePerson);
            this.groupSupply.Controls.Add(this.dtpDateBegin);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.txtRemark);
            this.groupSupply.Controls.Add(this.customLabel8);
            this.groupSupply.Controls.Add(this.customLabel3);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.lblSupplyContract);
            this.groupSupply.Location = new System.Drawing.Point(3, 3);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(943, 66);
            this.groupSupply.TabIndex = 2;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>单据信息";
            // 
            // txtMaterialCategory
            // 
            this.txtMaterialCategory.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterialCategory.Code = null;
            this.txtMaterialCategory.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtMaterialCategory.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtMaterialCategory.EnterToTab = false;
            this.txtMaterialCategory.Id = "";
            this.txtMaterialCategory.Location = new System.Drawing.Point(734, 17);
            this.txtMaterialCategory.Name = "txtMaterialCategory";
            this.txtMaterialCategory.ObjectType = Application.Business.Erp.ResourceManager.Client.Basic.Controls.MaterialSelectType.MaterialCatView;
            this.txtMaterialCategory.Result = ((System.Collections.IList)(resources.GetObject("txtMaterialCategory.Result")));
            this.txtMaterialCategory.RightMouse = false;
            this.txtMaterialCategory.Size = new System.Drawing.Size(156, 21);
            this.txtMaterialCategory.TabIndex = 137;
            this.txtMaterialCategory.Tag = null;
            this.txtMaterialCategory.Value = "";
            this.txtMaterialCategory.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // cboProfessionCat
            // 
            this.cboProfessionCat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboProfessionCat.FormattingEnabled = true;
            this.cboProfessionCat.Location = new System.Drawing.Point(735, 18);
            this.cboProfessionCat.Name = "cboProfessionCat";
            this.cboProfessionCat.Size = new System.Drawing.Size(147, 20);
            this.cboProfessionCat.TabIndex = 138;
            // 
            // lblCat
            // 
            this.lblCat.AutoSize = true;
            this.lblCat.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCat.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCat.Location = new System.Drawing.Point(669, 22);
            this.lblCat.Name = "lblCat";
            this.lblCat.Size = new System.Drawing.Size(59, 12);
            this.lblCat.TabIndex = 136;
            this.lblCat.Text = "物资分类:";
            this.lblCat.UnderLineColor = System.Drawing.Color.Red;
            // 
            // comPlanType
            // 
            this.comPlanType.FormattingEnabled = true;
            this.comPlanType.Location = new System.Drawing.Point(380, 17);
            this.comPlanType.Name = "comPlanType";
            this.comPlanType.Size = new System.Drawing.Size(92, 20);
            this.comPlanType.TabIndex = 132;
            // 
            // txtHandlePerson
            // 
            this.txtHandlePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtHandlePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtHandlePerson.DrawSelf = false;
            this.txtHandlePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtHandlePerson.EnterToTab = false;
            this.txtHandlePerson.Location = new System.Drawing.Point(322, -6);
            this.txtHandlePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtHandlePerson.ReadOnly = true;
            this.txtHandlePerson.Size = new System.Drawing.Size(99, 16);
            this.txtHandlePerson.TabIndex = 3;
            this.txtHandlePerson.Visible = false;
            // 
            // lblHandlePerson
            // 
            this.lblHandlePerson.AutoSize = true;
            this.lblHandlePerson.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHandlePerson.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblHandlePerson.Location = new System.Drawing.Point(276, -2);
            this.lblHandlePerson.Name = "lblHandlePerson";
            this.lblHandlePerson.Size = new System.Drawing.Size(47, 12);
            this.lblHandlePerson.TabIndex = 131;
            this.lblHandlePerson.Text = "责任人:";
            this.lblHandlePerson.UnderLineColor = System.Drawing.Color.Red;
            this.lblHandlePerson.Visible = false;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(539, 17);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 2;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(480, 21);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 130;
            this.customLabel4.Text = "签订时间:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(80, 44);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(823, 16);
            this.txtRemark.TabIndex = 6;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(320, 21);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(59, 12);
            this.customLabel8.TabIndex = 7;
            this.customLabel8.Text = "计划类型:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(40, 45);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(35, 12);
            this.customLabel3.TabIndex = 9;
            this.customLabel3.Text = "备注:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(81, 17);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(221, 16);
            this.txtCode.TabIndex = 1;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(28, 20);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(47, 12);
            this.lblSupplyContract.TabIndex = 0;
            this.lblSupplyContract.Text = "单据号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
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
            this.colMaterialCode,
            this.colMaterialName,
            this.colMaterialSpec,
            this.colSuff,
            this.colRealInQuantity,
            this.colNeedQuantity,
            this.colPlanQuantity,
            this.colPrice,
            this.colMoney,
            this.colDiagramNum,
            this.colMaterialType,
            this.colSpecailType,
            this.colQualityStandard,
            this.colApproachDate,
            this.colUsedPart,
            this.colUsedRanks,
            this.colUnit,
            this.colDescript,
            this.colForwardBill,
            this.colQuantityTemp});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(3, 3);
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
            this.dgDetail.Size = new System.Drawing.Size(934, 295);
            this.dgDetail.TabIndex = 7;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colMaterialCode
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.colMaterialCode.DefaultCellStyle = dataGridViewCellStyle5;
            this.colMaterialCode.HeaderText = "物资编码";
            this.colMaterialCode.Name = "colMaterialCode";
            this.colMaterialCode.Width = 80;
            // 
            // colMaterialName
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.colMaterialName.DefaultCellStyle = dataGridViewCellStyle6;
            this.colMaterialName.FillWeight = 80F;
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            this.colMaterialName.ReadOnly = true;
            this.colMaterialName.Width = 80;
            // 
            // colMaterialSpec
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            this.colMaterialSpec.DefaultCellStyle = dataGridViewCellStyle7;
            this.colMaterialSpec.FillWeight = 80F;
            this.colMaterialSpec.HeaderText = "规格型号";
            this.colMaterialSpec.Name = "colMaterialSpec";
            this.colMaterialSpec.ReadOnly = true;
            // 
            // colSuff
            // 
            this.colSuff.HeaderText = "材质";
            this.colSuff.Name = "colSuff";
            this.colSuff.Visible = false;
            // 
            // colRealInQuantity
            // 
            this.colRealInQuantity.HeaderText = "实际进场量";
            this.colRealInQuantity.Name = "colRealInQuantity";
            this.colRealInQuantity.ReadOnly = true;
            // 
            // colNeedQuantity
            // 
            this.colNeedQuantity.HeaderText = "需求数量";
            this.colNeedQuantity.Name = "colNeedQuantity";
            this.colNeedQuantity.Width = 80;
            // 
            // colPlanQuantity
            // 
            this.colPlanQuantity.HeaderText = "计划数量";
            this.colPlanQuantity.Name = "colPlanQuantity";
            // 
            // colPrice
            // 
            this.colPrice.HeaderText = "单价";
            this.colPrice.Name = "colPrice";
            // 
            // colMoney
            // 
            this.colMoney.HeaderText = "金额";
            this.colMoney.Name = "colMoney";
            this.colMoney.ReadOnly = true;
            // 
            // colDiagramNum
            // 
            this.colDiagramNum.HeaderText = "图号";
            this.colDiagramNum.Name = "colDiagramNum";
            // 
            // colMaterialType
            // 
            this.colMaterialType.HeaderText = "物资分类";
            this.colMaterialType.Name = "colMaterialType";
            this.colMaterialType.Visible = false;
            // 
            // colSpecailType
            // 
            this.colSpecailType.HeaderText = "专业分类";
            this.colSpecailType.Name = "colSpecailType";
            // 
            // colQualityStandard
            // 
            this.colQualityStandard.HeaderText = "质量标准";
            this.colQualityStandard.Name = "colQualityStandard";
            // 
            // colApproachDate
            // 
            this.colApproachDate.HeaderText = "进场时间";
            this.colApproachDate.Name = "colApproachDate";
            this.colApproachDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colApproachDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colApproachDate.Visible = false;
            // 
            // colUsedPart
            // 
            this.colUsedPart.HeaderText = "使用部位";
            this.colUsedPart.Name = "colUsedPart";
            // 
            // colUsedRanks
            // 
            this.colUsedRanks.HeaderText = "使用队伍";
            this.colUsedRanks.Name = "colUsedRanks";
            // 
            // colUnit
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            this.colUnit.DefaultCellStyle = dataGridViewCellStyle8;
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            this.colUnit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUnit.Width = 80;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 200;
            // 
            // colForwardBill
            // 
            this.colForwardBill.HeaderText = "前驱单号";
            this.colForwardBill.Name = "colForwardBill";
            this.colForwardBill.Visible = false;
            // 
            // colQuantityTemp
            // 
            this.colQuantityTemp.HeaderText = "临时数量";
            this.colQuantityTemp.Name = "colQuantityTemp";
            this.colQuantityTemp.Visible = false;
            // 
            // cmsDg
            // 
            this.cmsDg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDel});
            this.cmsDg.Name = "cmsDg";
            this.cmsDg.Size = new System.Drawing.Size(95, 26);
            // 
            // tsmiDel
            // 
            this.tsmiDel.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.tsmiDel.Name = "tsmiDel";
            this.tsmiDel.Size = new System.Drawing.Size(94, 22);
            this.tsmiDel.Text = "删除";
            this.tsmiDel.ToolTipText = "删除当前选中的记录";
            // 
            // txtSumQuantity
            // 
            this.txtSumQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSumQuantity.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumQuantity.DrawSelf = false;
            this.txtSumQuantity.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumQuantity.EnterToTab = false;
            this.txtSumQuantity.Location = new System.Drawing.Point(255, 12);
            this.txtSumQuantity.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumQuantity.Name = "txtSumQuantity";
            this.txtSumQuantity.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumQuantity.ReadOnly = true;
            this.txtSumQuantity.Size = new System.Drawing.Size(107, 16);
            this.txtSumQuantity.TabIndex = 10;
            // 
            // lbSumQuantity
            // 
            this.lbSumQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbSumQuantity.AutoSize = true;
            this.lbSumQuantity.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSumQuantity.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lbSumQuantity.Location = new System.Drawing.Point(190, 16);
            this.lbSumQuantity.Name = "lbSumQuantity";
            this.lbSumQuantity.Size = new System.Drawing.Size(59, 12);
            this.lbSumQuantity.TabIndex = 29;
            this.lbSumQuantity.Text = "合计数量:";
            this.lbSumQuantity.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(70, 11);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = true;
            this.txtCreatePerson.Size = new System.Drawing.Size(69, 16);
            this.txtCreatePerson.TabIndex = 8;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(19, 15);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(47, 12);
            this.customLabel5.TabIndex = 25;
            this.customLabel5.Text = "制单人:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProject
            // 
            this.txtProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProject.DrawSelf = false;
            this.txtProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProject.EnterToTab = false;
            this.txtProject.Location = new System.Drawing.Point(443, 12);
            this.txtProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProject.Name = "txtProject";
            this.txtProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtProject.ReadOnly = true;
            this.txtProject.Size = new System.Drawing.Size(137, 16);
            this.txtProject.TabIndex = 11;
            // 
            // customLabel1
            // 
            this.customLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(378, 16);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 31;
            this.customLabel1.Text = "归属项目:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(948, 326);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(940, 301);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "明细信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtSumMoney
            // 
            this.txtSumMoney.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSumMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumMoney.DrawSelf = false;
            this.txtSumMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumMoney.EnterToTab = false;
            this.txtSumMoney.Location = new System.Drawing.Point(670, 12);
            this.txtSumMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumMoney.Name = "txtSumMoney";
            this.txtSumMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumMoney.ReadOnly = true;
            this.txtSumMoney.Size = new System.Drawing.Size(118, 16);
            this.txtSumMoney.TabIndex = 32;
            // 
            // customLabel2
            // 
            this.customLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(605, 16);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 33;
            this.customLabel2.Text = "合计金额:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VMonthlyPlanMng
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(962, 441);
            this.Name = "VMonthlyPlanMng";
            this.Text = "月度需求计划";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlContent.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlFloor.ResumeLayout(false);
            this.pnlWorkSpace.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterialCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.cmsDg.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);







            this.flexGrid1 = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.groupSupply.Controls.Add(this.flexGrid1);
            // 
            // flexGrid1
            // 
            this.flexGrid1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flexGrid1.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid1.CheckedImage")));
            this.flexGrid1.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.flexGrid1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGrid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGrid1.Location = new System.Drawing.Point(277, 0);
            this.flexGrid1.Name = "flexGrid1";
            this.flexGrid1.Size = new System.Drawing.Size(473, 19);
            this.flexGrid1.TabIndex = 18;
            this.flexGrid1.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid1.UncheckedImage")));
            this.flexGrid1.Visible = false;
        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lbSumQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtHandlePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblHandlePerson;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ContextMenuStrip cmsDg;
        private System.Windows.Forms.ToolStripMenuItem tsmiDel;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.ComboBox comPlanType;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial txtMaterialCategory;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCat;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboProfessionCat;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuff;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealInQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNeedQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlanQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiagramNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSpecailType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQualityStandard;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colApproachDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsedPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsedRanks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colForwardBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantityTemp;
    }
}