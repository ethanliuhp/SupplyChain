namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    partial class VPlanProjectAmountSharing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VPlanProjectAmountSharing));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAccountingNodePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmuGWBSDetailMouseRight = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.类比分摊 = new System.Windows.Forms.ToolStripMenuItem();
            this.批量分摊 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cbShowNoSharingDetail = new System.Windows.Forms.CheckBox();
            this.dgProjectTaskDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.tvwGWBS = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkInverse = new System.Windows.Forms.LinkLabel();
            this.lnkCheckAll = new System.Windows.Forms.LinkLabel();
            this.dgSharingProjectAmount = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.SharingLeafNodePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SharingQuotaCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SharingGWBSDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SharingExistProjectAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SharingTheSharingProjectAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSumbitSharing = new System.Windows.Forms.Button();
            this.cbBulkSharing = new System.Windows.Forms.CheckBox();
            this.PlanSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PlanQuotaCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanGWBSDetailName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanTheCostItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanMainResourceTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanMainResourceTypeSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanDiagramNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanProjectAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanSharingSummaryAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanTheSharingSummaryAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.cmuGWBSDetailMouseRight.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProjectTaskDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSharingProjectAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.cbBulkSharing);
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Controls.Add(this.txtAccountingNodePath);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Size = new System.Drawing.Size(1065, 633);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtAccountingNodePath, 0);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cbBulkSharing, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -11);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "核算节点路径：";
            // 
            // txtAccountingNodePath
            // 
            this.txtAccountingNodePath.Location = new System.Drawing.Point(108, 4);
            this.txtAccountingNodePath.Name = "txtAccountingNodePath";
            this.txtAccountingNodePath.Size = new System.Drawing.Size(767, 21);
            this.txtAccountingNodePath.TabIndex = 137;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 145;
            this.label2.Text = "工程任务明细";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 147;
            this.label4.Text = "分摊工程量";
            // 
            // cmuGWBSDetailMouseRight
            // 
            this.cmuGWBSDetailMouseRight.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.类比分摊,
            this.批量分摊});
            this.cmuGWBSDetailMouseRight.Name = "cmuGWBSDetailMouseRight";
            this.cmuGWBSDetailMouseRight.Size = new System.Drawing.Size(119, 48);
            // 
            // 类比分摊
            // 
            this.类比分摊.Name = "类比分摊";
            this.类比分摊.Size = new System.Drawing.Size(118, 22);
            this.类比分摊.Text = "类比分摊";
            // 
            // 批量分摊
            // 
            this.批量分摊.Name = "批量分摊";
            this.批量分摊.Size = new System.Drawing.Size(118, 22);
            this.批量分摊.Text = "批量分摊";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(3, 30);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.dgSharingProjectAmount);
            this.splitContainer1.Panel2.Controls.Add(this.btnSumbitSharing);
            this.splitContainer1.Size = new System.Drawing.Size(1059, 600);
            this.splitContainer1.SplitterDistance = 616;
            this.splitContainer1.TabIndex = 148;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.cbShowNoSharingDetail);
            this.splitContainer2.Panel1.Controls.Add(this.dgProjectTaskDetail);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tvwGWBS);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.lnkInverse);
            this.splitContainer2.Panel2.Controls.Add(this.lnkCheckAll);
            this.splitContainer2.Size = new System.Drawing.Size(616, 600);
            this.splitContainer2.SplitterDistance = 281;
            this.splitContainer2.TabIndex = 146;
            // 
            // cbShowNoSharingDetail
            // 
            this.cbShowNoSharingDetail.AutoSize = true;
            this.cbShowNoSharingDetail.Location = new System.Drawing.Point(437, 3);
            this.cbShowNoSharingDetail.Name = "cbShowNoSharingDetail";
            this.cbShowNoSharingDetail.Size = new System.Drawing.Size(132, 16);
            this.cbShowNoSharingDetail.TabIndex = 157;
            this.cbShowNoSharingDetail.Text = "显示未分摊完的明细";
            this.cbShowNoSharingDetail.UseVisualStyleBackColor = true;
            // 
            // dgProjectTaskDetail
            // 
            this.dgProjectTaskDetail.AddDefaultMenu = false;
            this.dgProjectTaskDetail.AddNoColumn = true;
            this.dgProjectTaskDetail.AllowUserToAddRows = false;
            this.dgProjectTaskDetail.AllowUserToDeleteRows = false;
            this.dgProjectTaskDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgProjectTaskDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgProjectTaskDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgProjectTaskDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgProjectTaskDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgProjectTaskDetail.ColumnHeadersHeight = 24;
            this.dgProjectTaskDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgProjectTaskDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PlanSelect,
            this.PlanQuotaCode,
            this.PlanGWBSDetailName,
            this.PlanTheCostItemName,
            this.PlanMainResourceTypeName,
            this.PlanMainResourceTypeSpec,
            this.PlanDiagramNumber,
            this.PlanProjectAmount,
            this.PlanSharingSummaryAmount,
            this.PlanTheSharingSummaryAmount});
            this.dgProjectTaskDetail.CustomBackColor = false;
            this.dgProjectTaskDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgProjectTaskDetail.EnableHeadersVisualStyles = false;
            this.dgProjectTaskDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgProjectTaskDetail.FreezeFirstRow = false;
            this.dgProjectTaskDetail.FreezeLastRow = false;
            this.dgProjectTaskDetail.FrontColumnCount = 0;
            this.dgProjectTaskDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgProjectTaskDetail.HScrollOffset = 0;
            this.dgProjectTaskDetail.IsAllowOrder = true;
            this.dgProjectTaskDetail.IsConfirmDelete = true;
            this.dgProjectTaskDetail.Location = new System.Drawing.Point(3, 25);
            this.dgProjectTaskDetail.Name = "dgProjectTaskDetail";
            this.dgProjectTaskDetail.PageIndex = 0;
            this.dgProjectTaskDetail.PageSize = 0;
            this.dgProjectTaskDetail.Query = null;
            this.dgProjectTaskDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgProjectTaskDetail.ReadOnlyCols")));
            this.dgProjectTaskDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgProjectTaskDetail.RowHeadersVisible = false;
            this.dgProjectTaskDetail.RowHeadersWidth = 22;
            this.dgProjectTaskDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgProjectTaskDetail.RowTemplate.Height = 23;
            this.dgProjectTaskDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgProjectTaskDetail.Size = new System.Drawing.Size(608, 251);
            this.dgProjectTaskDetail.TabIndex = 156;
            this.dgProjectTaskDetail.TargetType = null;
            this.dgProjectTaskDetail.VScrollOffset = 0;
            // 
            // tvwGWBS
            // 
            this.tvwGWBS.AllowDrop = true;
            this.tvwGWBS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvwGWBS.BackColor = System.Drawing.SystemColors.Window;
            this.tvwGWBS.CheckBoxes = true;
            this.tvwGWBS.HideSelection = false;
            this.tvwGWBS.Location = new System.Drawing.Point(3, 15);
            this.tvwGWBS.Name = "tvwGWBS";
            this.tvwGWBS.Size = new System.Drawing.Size(608, 267);
            this.tvwGWBS.TabIndex = 164;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 163;
            this.label3.Text = "下属生产节点";
            // 
            // lnkInverse
            // 
            this.lnkInverse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkInverse.AutoSize = true;
            this.lnkInverse.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lnkInverse.Location = new System.Drawing.Point(67, 293);
            this.lnkInverse.Name = "lnkInverse";
            this.lnkInverse.Size = new System.Drawing.Size(29, 12);
            this.lnkInverse.TabIndex = 162;
            this.lnkInverse.TabStop = true;
            this.lnkInverse.Text = "反选";
            // 
            // lnkCheckAll
            // 
            this.lnkCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkCheckAll.AutoSize = true;
            this.lnkCheckAll.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lnkCheckAll.Location = new System.Drawing.Point(32, 293);
            this.lnkCheckAll.Name = "lnkCheckAll";
            this.lnkCheckAll.Size = new System.Drawing.Size(29, 12);
            this.lnkCheckAll.TabIndex = 161;
            this.lnkCheckAll.TabStop = true;
            this.lnkCheckAll.Text = "全选";
            // 
            // dgSharingProjectAmount
            // 
            this.dgSharingProjectAmount.AddDefaultMenu = false;
            this.dgSharingProjectAmount.AddNoColumn = true;
            this.dgSharingProjectAmount.AllowUserToAddRows = false;
            this.dgSharingProjectAmount.AllowUserToDeleteRows = false;
            this.dgSharingProjectAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgSharingProjectAmount.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgSharingProjectAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgSharingProjectAmount.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgSharingProjectAmount.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgSharingProjectAmount.ColumnHeadersHeight = 24;
            this.dgSharingProjectAmount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgSharingProjectAmount.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SharingLeafNodePath,
            this.SharingQuotaCode,
            this.SharingGWBSDetail,
            this.SharingExistProjectAmount,
            this.SharingTheSharingProjectAmount});
            this.dgSharingProjectAmount.CustomBackColor = false;
            this.dgSharingProjectAmount.EditCellBackColor = System.Drawing.Color.White;
            this.dgSharingProjectAmount.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgSharingProjectAmount.EnableHeadersVisualStyles = false;
            this.dgSharingProjectAmount.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgSharingProjectAmount.FreezeFirstRow = false;
            this.dgSharingProjectAmount.FreezeLastRow = false;
            this.dgSharingProjectAmount.FrontColumnCount = 0;
            this.dgSharingProjectAmount.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgSharingProjectAmount.HScrollOffset = 0;
            this.dgSharingProjectAmount.IsAllowOrder = true;
            this.dgSharingProjectAmount.IsConfirmDelete = true;
            this.dgSharingProjectAmount.Location = new System.Drawing.Point(3, 15);
            this.dgSharingProjectAmount.Name = "dgSharingProjectAmount";
            this.dgSharingProjectAmount.PageIndex = 0;
            this.dgSharingProjectAmount.PageSize = 0;
            this.dgSharingProjectAmount.Query = null;
            this.dgSharingProjectAmount.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgSharingProjectAmount.ReadOnlyCols")));
            this.dgSharingProjectAmount.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgSharingProjectAmount.RowHeadersVisible = false;
            this.dgSharingProjectAmount.RowHeadersWidth = 22;
            this.dgSharingProjectAmount.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgSharingProjectAmount.RowTemplate.Height = 23;
            this.dgSharingProjectAmount.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSharingProjectAmount.Size = new System.Drawing.Size(431, 552);
            this.dgSharingProjectAmount.TabIndex = 150;
            this.dgSharingProjectAmount.TargetType = null;
            this.dgSharingProjectAmount.VScrollOffset = 0;
            // 
            // SharingLeafNodePath
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.SharingLeafNodePath.DefaultCellStyle = dataGridViewCellStyle1;
            this.SharingLeafNodePath.HeaderText = "叶节点路径";
            this.SharingLeafNodePath.Name = "SharingLeafNodePath";
            this.SharingLeafNodePath.ReadOnly = true;
            this.SharingLeafNodePath.Width = 80;
            // 
            // SharingQuotaCode
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.SharingQuotaCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.SharingQuotaCode.HeaderText = "定额编号";
            this.SharingQuotaCode.Name = "SharingQuotaCode";
            this.SharingQuotaCode.ReadOnly = true;
            this.SharingQuotaCode.Width = 70;
            // 
            // SharingGWBSDetail
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.SharingGWBSDetail.DefaultCellStyle = dataGridViewCellStyle3;
            this.SharingGWBSDetail.HeaderText = "工程任务明细";
            this.SharingGWBSDetail.Name = "SharingGWBSDetail";
            this.SharingGWBSDetail.ReadOnly = true;
            // 
            // SharingExistProjectAmount
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.SharingExistProjectAmount.DefaultCellStyle = dataGridViewCellStyle4;
            this.SharingExistProjectAmount.HeaderText = "已有工程量";
            this.SharingExistProjectAmount.Name = "SharingExistProjectAmount";
            this.SharingExistProjectAmount.ReadOnly = true;
            this.SharingExistProjectAmount.Width = 80;
            // 
            // SharingTheSharingProjectAmount
            // 
            this.SharingTheSharingProjectAmount.HeaderText = "本次分摊工作量";
            this.SharingTheSharingProjectAmount.Name = "SharingTheSharingProjectAmount";
            this.SharingTheSharingProjectAmount.Width = 80;
            // 
            // btnSumbitSharing
            // 
            this.btnSumbitSharing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSumbitSharing.Location = new System.Drawing.Point(329, 572);
            this.btnSumbitSharing.Name = "btnSumbitSharing";
            this.btnSumbitSharing.Size = new System.Drawing.Size(105, 23);
            this.btnSumbitSharing.TabIndex = 153;
            this.btnSumbitSharing.Text = "提交分摊";
            this.btnSumbitSharing.UseVisualStyleBackColor = true;
            // 
            // cbBulkSharing
            // 
            this.cbBulkSharing.AutoSize = true;
            this.cbBulkSharing.Location = new System.Drawing.Point(896, 8);
            this.cbBulkSharing.Name = "cbBulkSharing";
            this.cbBulkSharing.Size = new System.Drawing.Size(72, 16);
            this.cbBulkSharing.TabIndex = 149;
            this.cbBulkSharing.Text = "批量分摊";
            this.cbBulkSharing.UseVisualStyleBackColor = true;
            // 
            // PlanSelect
            // 
            this.PlanSelect.HeaderText = "选择";
            this.PlanSelect.Name = "PlanSelect";
            this.PlanSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.PlanSelect.Width = 50;
            // 
            // PlanQuotaCode
            // 
            this.PlanQuotaCode.HeaderText = "定额编号";
            this.PlanQuotaCode.Name = "PlanQuotaCode";
            this.PlanQuotaCode.ReadOnly = true;
            this.PlanQuotaCode.Width = 70;
            // 
            // PlanGWBSDetailName
            // 
            this.PlanGWBSDetailName.HeaderText = "工程任务明细名称";
            this.PlanGWBSDetailName.Name = "PlanGWBSDetailName";
            this.PlanGWBSDetailName.ReadOnly = true;
            this.PlanGWBSDetailName.Width = 133;
            // 
            // PlanTheCostItemName
            // 
            this.PlanTheCostItemName.HeaderText = "成本项";
            this.PlanTheCostItemName.Name = "PlanTheCostItemName";
            // 
            // PlanMainResourceTypeName
            // 
            this.PlanMainResourceTypeName.HeaderText = "主资源类型";
            this.PlanMainResourceTypeName.Name = "PlanMainResourceTypeName";
            this.PlanMainResourceTypeName.Width = 90;
            // 
            // PlanMainResourceTypeSpec
            // 
            this.PlanMainResourceTypeSpec.HeaderText = "规格型号";
            this.PlanMainResourceTypeSpec.Name = "PlanMainResourceTypeSpec";
            this.PlanMainResourceTypeSpec.Width = 90;
            // 
            // PlanDiagramNumber
            // 
            this.PlanDiagramNumber.HeaderText = "图号";
            this.PlanDiagramNumber.Name = "PlanDiagramNumber";
            this.PlanDiagramNumber.Width = 60;
            // 
            // PlanProjectAmount
            // 
            this.PlanProjectAmount.HeaderText = "计划工程量";
            this.PlanProjectAmount.Name = "PlanProjectAmount";
            this.PlanProjectAmount.ReadOnly = true;
            this.PlanProjectAmount.Width = 80;
            // 
            // PlanSharingSummaryAmount
            // 
            this.PlanSharingSummaryAmount.HeaderText = "分摊汇总量";
            this.PlanSharingSummaryAmount.Name = "PlanSharingSummaryAmount";
            this.PlanSharingSummaryAmount.ReadOnly = true;
            this.PlanSharingSummaryAmount.Width = 80;
            // 
            // PlanTheSharingSummaryAmount
            // 
            this.PlanTheSharingSummaryAmount.HeaderText = "本次分摊汇总量";
            this.PlanTheSharingSummaryAmount.Name = "PlanTheSharingSummaryAmount";
            this.PlanTheSharingSummaryAmount.ReadOnly = true;
            this.PlanTheSharingSummaryAmount.Width = 80;
            // 
            // VPlanProjectAmountSharing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 633);
            this.Name = "VPlanProjectAmountSharing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计划工程量分摊";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.cmuGWBSDetailMouseRight.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgProjectTaskDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgSharingProjectAmount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAccountingNodePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip cmuGWBSDetailMouseRight;
        private System.Windows.Forms.ToolStripMenuItem 类比分摊;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgSharingProjectAmount;
        private System.Windows.Forms.Button btnSumbitSharing;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgProjectTaskDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView tvwGWBS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lnkInverse;
        private System.Windows.Forms.LinkLabel lnkCheckAll;
        private System.Windows.Forms.CheckBox cbBulkSharing;
        private System.Windows.Forms.ToolStripMenuItem 批量分摊;
        private System.Windows.Forms.CheckBox cbShowNoSharingDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn SharingLeafNodePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn SharingQuotaCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn SharingGWBSDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn SharingExistProjectAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn SharingTheSharingProjectAmount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PlanSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanQuotaCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanGWBSDetailName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanTheCostItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanMainResourceTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanMainResourceTypeSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanDiagramNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanProjectAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanSharingSummaryAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanTheSharingSummaryAmount;
    }
}