namespace Application.Business.Erp.SupplyChain.Client.TotalDemandPlan
{
    partial class VDetailInformation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDetailInformation));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTotal = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMonthDemand = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtDailyDemand = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtQuantityUnit = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtSHPDemand = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtGDDemand = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtMaterialType = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel14 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel13 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtTH = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel12 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel11 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel10 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel16 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnGaveup = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colGDGWBS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGDDemand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGDMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGDDaily = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGDZHX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colTotalMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGWBS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox3);
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.btnGaveup);
            this.pnlFloor.Controls.Add(this.customLabel4);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(945, 498);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel4, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnGaveup, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox3, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -8);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtTotal);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.txtMonthDemand);
            this.groupBox1.Controls.Add(this.txtDailyDemand);
            this.groupBox1.Controls.Add(this.txtQuantityUnit);
            this.groupBox1.Controls.Add(this.txtSHPDemand);
            this.groupBox1.Controls.Add(this.txtGDDemand);
            this.groupBox1.Controls.Add(this.txtMaterialType);
            this.groupBox1.Controls.Add(this.customLabel14);
            this.groupBox1.Controls.Add(this.customLabel13);
            this.groupBox1.Controls.Add(this.txtTH);
            this.groupBox1.Controls.Add(this.customLabel12);
            this.groupBox1.Controls.Add(this.customLabel11);
            this.groupBox1.Controls.Add(this.customLabel10);
            this.groupBox1.Controls.Add(this.customLabel9);
            this.groupBox1.Controls.Add(this.customLabel16);
            this.groupBox1.Location = new System.Drawing.Point(15, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(909, 84);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.SystemColors.Control;
            this.txtTotal.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtTotal.DrawSelf = false;
            this.txtTotal.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtTotal.EnterToTab = false;
            this.txtTotal.Location = new System.Drawing.Point(786, 24);
            this.txtTotal.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Padding = new System.Windows.Forms.Padding(1);
            this.txtTotal.ReadOnly = false;
            this.txtTotal.Size = new System.Drawing.Size(117, 16);
            this.txtTotal.TabIndex = 168;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(507, 28);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 167;
            this.customLabel1.Text = "数量单位:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtMonthDemand
            // 
            this.txtMonthDemand.BackColor = System.Drawing.SystemColors.Control;
            this.txtMonthDemand.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMonthDemand.DrawSelf = false;
            this.txtMonthDemand.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMonthDemand.EnterToTab = false;
            this.txtMonthDemand.Location = new System.Drawing.Point(555, 53);
            this.txtMonthDemand.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMonthDemand.Name = "txtMonthDemand";
            this.txtMonthDemand.Padding = new System.Windows.Forms.Padding(1);
            this.txtMonthDemand.ReadOnly = false;
            this.txtMonthDemand.Size = new System.Drawing.Size(117, 16);
            this.txtMonthDemand.TabIndex = 166;
            // 
            // txtDailyDemand
            // 
            this.txtDailyDemand.BackColor = System.Drawing.SystemColors.Control;
            this.txtDailyDemand.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDailyDemand.DrawSelf = false;
            this.txtDailyDemand.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDailyDemand.EnterToTab = false;
            this.txtDailyDemand.Location = new System.Drawing.Point(786, 53);
            this.txtDailyDemand.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDailyDemand.Name = "txtDailyDemand";
            this.txtDailyDemand.Padding = new System.Windows.Forms.Padding(1);
            this.txtDailyDemand.ReadOnly = false;
            this.txtDailyDemand.Size = new System.Drawing.Size(117, 16);
            this.txtDailyDemand.TabIndex = 165;
            // 
            // txtQuantityUnit
            // 
            this.txtQuantityUnit.BackColor = System.Drawing.SystemColors.Control;
            this.txtQuantityUnit.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtQuantityUnit.DrawSelf = false;
            this.txtQuantityUnit.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtQuantityUnit.EnterToTab = false;
            this.txtQuantityUnit.Location = new System.Drawing.Point(569, 24);
            this.txtQuantityUnit.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtQuantityUnit.Name = "txtQuantityUnit";
            this.txtQuantityUnit.Padding = new System.Windows.Forms.Padding(1);
            this.txtQuantityUnit.ReadOnly = false;
            this.txtQuantityUnit.Size = new System.Drawing.Size(118, 16);
            this.txtQuantityUnit.TabIndex = 164;
            // 
            // txtSHPDemand
            // 
            this.txtSHPDemand.BackColor = System.Drawing.SystemColors.Control;
            this.txtSHPDemand.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSHPDemand.DrawSelf = false;
            this.txtSHPDemand.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSHPDemand.EnterToTab = false;
            this.txtSHPDemand.Location = new System.Drawing.Point(325, 53);
            this.txtSHPDemand.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSHPDemand.Name = "txtSHPDemand";
            this.txtSHPDemand.Padding = new System.Windows.Forms.Padding(1);
            this.txtSHPDemand.ReadOnly = false;
            this.txtSHPDemand.Size = new System.Drawing.Size(117, 16);
            this.txtSHPDemand.TabIndex = 163;
            // 
            // txtGDDemand
            // 
            this.txtGDDemand.BackColor = System.Drawing.SystemColors.Control;
            this.txtGDDemand.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtGDDemand.DrawSelf = false;
            this.txtGDDemand.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtGDDemand.EnterToTab = false;
            this.txtGDDemand.Location = new System.Drawing.Point(121, 53);
            this.txtGDDemand.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtGDDemand.Name = "txtGDDemand";
            this.txtGDDemand.Padding = new System.Windows.Forms.Padding(1);
            this.txtGDDemand.ReadOnly = false;
            this.txtGDDemand.Size = new System.Drawing.Size(107, 16);
            this.txtGDDemand.TabIndex = 159;
            // 
            // txtMaterialType
            // 
            this.txtMaterialType.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterialType.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMaterialType.DrawSelf = false;
            this.txtMaterialType.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMaterialType.EnterToTab = false;
            this.txtMaterialType.Location = new System.Drawing.Point(79, 24);
            this.txtMaterialType.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMaterialType.Name = "txtMaterialType";
            this.txtMaterialType.Padding = new System.Windows.Forms.Padding(1);
            this.txtMaterialType.ReadOnly = false;
            this.txtMaterialType.Size = new System.Drawing.Size(169, 16);
            this.txtMaterialType.TabIndex = 157;
            // 
            // customLabel14
            // 
            this.customLabel14.AutoSize = true;
            this.customLabel14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel14.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel14.Location = new System.Drawing.Point(714, 28);
            this.customLabel14.Name = "customLabel14";
            this.customLabel14.Size = new System.Drawing.Size(71, 12);
            this.customLabel14.TabIndex = 87;
            this.customLabel14.Text = "已执行总量:";
            this.customLabel14.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel13
            // 
            this.customLabel13.AutoSize = true;
            this.customLabel13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel13.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel13.Location = new System.Drawing.Point(460, 57);
            this.customLabel13.Name = "customLabel13";
            this.customLabel13.Size = new System.Drawing.Size(95, 12);
            this.customLabel13.TabIndex = 82;
            this.customLabel13.Text = "月计划发布总量:";
            this.customLabel13.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtTH
            // 
            this.txtTH.BackColor = System.Drawing.SystemColors.Control;
            this.txtTH.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtTH.DrawSelf = false;
            this.txtTH.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtTH.EnterToTab = false;
            this.txtTH.Location = new System.Drawing.Point(309, 24);
            this.txtTH.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtTH.Name = "txtTH";
            this.txtTH.Padding = new System.Windows.Forms.Padding(1);
            this.txtTH.ReadOnly = false;
            this.txtTH.Size = new System.Drawing.Size(167, 16);
            this.txtTH.TabIndex = 58;
            // 
            // customLabel12
            // 
            this.customLabel12.AutoSize = true;
            this.customLabel12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel12.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel12.Location = new System.Drawing.Point(270, 28);
            this.customLabel12.Name = "customLabel12";
            this.customLabel12.Size = new System.Drawing.Size(35, 12);
            this.customLabel12.TabIndex = 57;
            this.customLabel12.Text = "图号:";
            this.customLabel12.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel11
            // 
            this.customLabel11.AutoSize = true;
            this.customLabel11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel11.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel11.Location = new System.Drawing.Point(246, 57);
            this.customLabel11.Name = "customLabel11";
            this.customLabel11.Size = new System.Drawing.Size(83, 12);
            this.customLabel11.TabIndex = 56;
            this.customLabel11.Text = "审批总需求量:";
            this.customLabel11.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel10
            // 
            this.customLabel10.AutoSize = true;
            this.customLabel10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel10.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel10.Location = new System.Drawing.Point(16, 28);
            this.customLabel10.Name = "customLabel10";
            this.customLabel10.Size = new System.Drawing.Size(59, 12);
            this.customLabel10.TabIndex = 55;
            this.customLabel10.Text = "资源类型:";
            this.customLabel10.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(16, 57);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(107, 12);
            this.customLabel9.TabIndex = 54;
            this.customLabel9.Text = "滚动计划总需求量:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel16
            // 
            this.customLabel16.AutoSize = true;
            this.customLabel16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel16.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel16.Location = new System.Drawing.Point(683, 57);
            this.customLabel16.Name = "customLabel16";
            this.customLabel16.Size = new System.Drawing.Size(107, 12);
            this.customLabel16.TabIndex = 86;
            this.customLabel16.Text = "日常计划发布总量:";
            this.customLabel16.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(15, 17);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(101, 12);
            this.customLabel4.TabIndex = 140;
            this.customLabel4.Text = "总量需求分析信息";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnGaveup
            // 
            this.btnGaveup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGaveup.Location = new System.Drawing.Point(826, 469);
            this.btnGaveup.Name = "btnGaveup";
            this.btnGaveup.Size = new System.Drawing.Size(75, 23);
            this.btnGaveup.TabIndex = 145;
            this.btnGaveup.Text = "关闭";
            this.btnGaveup.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgMaster);
            this.groupBox2.Location = new System.Drawing.Point(17, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(508, 350);
            this.groupBox2.TabIndex = 159;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "滚动计划总量需求明细";
            // 
            // dgMaster
            // 
            this.dgMaster.AddDefaultMenu = false;
            this.dgMaster.AddNoColumn = true;
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.AllowUserToDeleteRows = false;
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgMaster.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.ColumnHeadersHeight = 24;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGDGWBS,
            this.colGDDemand,
            this.colGDMonth,
            this.colGDDaily,
            this.colGDZHX});
            this.dgMaster.CustomBackColor = false;
            this.dgMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMaster.EditCellBackColor = System.Drawing.Color.White;
            this.dgMaster.EnableHeadersVisualStyles = false;
            this.dgMaster.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgMaster.FreezeFirstRow = false;
            this.dgMaster.FreezeLastRow = false;
            this.dgMaster.FrontColumnCount = 0;
            this.dgMaster.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgMaster.HScrollOffset = 0;
            this.dgMaster.IsAllowOrder = true;
            this.dgMaster.IsConfirmDelete = true;
            this.dgMaster.Location = new System.Drawing.Point(3, 17);
            this.dgMaster.Name = "dgMaster";
            this.dgMaster.PageIndex = 0;
            this.dgMaster.PageSize = 0;
            this.dgMaster.Query = null;
            this.dgMaster.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgMaster.ReadOnlyCols")));
            this.dgMaster.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.RowHeadersWidth = 22;
            this.dgMaster.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMaster.RowTemplate.Height = 23;
            this.dgMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMaster.Size = new System.Drawing.Size(502, 330);
            this.dgMaster.TabIndex = 5;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colGDGWBS
            // 
            this.colGDGWBS.HeaderText = "GWBS路径";
            this.colGDGWBS.Name = "colGDGWBS";
            // 
            // colGDDemand
            // 
            this.colGDDemand.HeaderText = "计划总需求量";
            this.colGDDemand.Name = "colGDDemand";
            // 
            // colGDMonth
            // 
            this.colGDMonth.HeaderText = "月计划发布总量";
            this.colGDMonth.Name = "colGDMonth";
            this.colGDMonth.ReadOnly = true;
            this.colGDMonth.Width = 120;
            // 
            // colGDDaily
            // 
            this.colGDDaily.HeaderText = "日常计划发布总量";
            this.colGDDaily.Name = "colGDDaily";
            this.colGDDaily.Width = 120;
            // 
            // colGDZHX
            // 
            this.colGDZHX.HeaderText = "已执行总量";
            this.colGDZHX.Name = "colGDZHX";
            this.colGDZHX.Width = 80;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgDetail);
            this.groupBox3.Location = new System.Drawing.Point(528, 116);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(396, 350);
            this.groupBox3.TabIndex = 160;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "总量需求单明细";
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = true;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTotalMaterial,
            this.colGWBS,
            this.colTotalQuantity});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.dgDetail.Location = new System.Drawing.Point(3, 17);
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
            this.dgDetail.Size = new System.Drawing.Size(390, 330);
            this.dgDetail.TabIndex = 4;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colTotalMaterial
            // 
            this.colTotalMaterial.HeaderText = "资源需求计划单";
            this.colTotalMaterial.Name = "colTotalMaterial";
            // 
            // colGWBS
            // 
            this.colGWBS.HeaderText = "GWBS节点路径";
            this.colGWBS.Name = "colGWBS";
            // 
            // colTotalQuantity
            // 
            this.colTotalQuantity.HeaderText = "计划总需求量";
            this.colTotalQuantity.Name = "colTotalQuantity";
            this.colTotalQuantity.ReadOnly = true;
            // 
            // VDetailInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 498);
            this.Name = "VDetailInformation";
            this.Text = "总量需求分析";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel16;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel10;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel12;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel11;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtTH;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel13;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel14;
        private System.Windows.Forms.Button btnGaveup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMaterialType;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtGDDemand;
        private System.Windows.Forms.GroupBox groupBox3;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.GroupBox groupBox2;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalMaterial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSHPDemand;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMonthDemand;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDailyDemand;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtQuantityUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGDGWBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGDDemand;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGDMonth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGDDaily;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGDZHX;
    }
}