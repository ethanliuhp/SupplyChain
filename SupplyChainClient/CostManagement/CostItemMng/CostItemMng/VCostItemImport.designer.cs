namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    partial class VCostItemImport
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvwCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.btnAddCostitem = new System.Windows.Forms.Button();
            this.btnCheckNode = new System.Windows.Forms.Button();
            this.txtNode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtPersonQuotaPrice = new System.Windows.Forms.TextBox();
            this.cbContentType = new System.Windows.Forms.ComboBox();
            this.cbManagemode = new System.Windows.Forms.ComboBox();
            this.cbUsedLevel = new System.Windows.Forms.ComboBox();
            this.cbTableNames = new System.Windows.Forms.ListBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnUpdateCostItem = new System.Windows.Forms.Button();
            this.btnUpdateCostItemCateFilter = new System.Windows.Forms.Button();
            this.btnUpdateCategory = new System.Windows.Forms.Button();
            this.btnUpdateCostItemPricing = new System.Windows.Forms.Button();
            this.btnGetQuotaHasQuantity = new System.Windows.Forms.Button();
            this.btnCostItemMng = new System.Windows.Forms.Button();
            this.txtExcelFilePath = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnBrownFile = new System.Windows.Forms.Button();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnAddCostItem1 = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Size = new System.Drawing.Size(889, 524);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvwCategory);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnAddCostItem1);
            this.splitContainer1.Panel2.Controls.Add(this.btnAddCostitem);
            this.splitContainer1.Panel2.Controls.Add(this.btnCheckNode);
            this.splitContainer1.Panel2.Controls.Add(this.txtNode);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.btnAdd);
            this.splitContainer1.Panel2.Controls.Add(this.txtPersonQuotaPrice);
            this.splitContainer1.Panel2.Controls.Add(this.cbContentType);
            this.splitContainer1.Panel2.Controls.Add(this.cbManagemode);
            this.splitContainer1.Panel2.Controls.Add(this.cbUsedLevel);
            this.splitContainer1.Panel2.Controls.Add(this.cbTableNames);
            this.splitContainer1.Panel2.Controls.Add(this.btnImport);
            this.splitContainer1.Panel2.Controls.Add(this.btnUpdateCostItem);
            this.splitContainer1.Panel2.Controls.Add(this.btnUpdateCostItemCateFilter);
            this.splitContainer1.Panel2.Controls.Add(this.btnUpdateCategory);
            this.splitContainer1.Panel2.Controls.Add(this.btnUpdateCostItemPricing);
            this.splitContainer1.Panel2.Controls.Add(this.btnGetQuotaHasQuantity);
            this.splitContainer1.Panel2.Controls.Add(this.btnCostItemMng);
            this.splitContainer1.Panel2.Controls.Add(this.txtExcelFilePath);
            this.splitContainer1.Panel2.Controls.Add(this.btnBrownFile);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel3);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel2);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel5);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel1);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel4);
            this.splitContainer1.Size = new System.Drawing.Size(889, 499);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 6;
            // 
            // tvwCategory
            // 
            this.tvwCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvwCategory.BackColor = System.Drawing.SystemColors.Window;
            this.tvwCategory.HideSelection = false;
            this.tvwCategory.Location = new System.Drawing.Point(3, 3);
            this.tvwCategory.Name = "tvwCategory";
            this.tvwCategory.Size = new System.Drawing.Size(292, 491);
            this.tvwCategory.TabIndex = 1;
            // 
            // btnAddCostitem
            // 
            this.btnAddCostitem.Location = new System.Drawing.Point(215, 153);
            this.btnAddCostitem.Name = "btnAddCostitem";
            this.btnAddCostitem.Size = new System.Drawing.Size(167, 23);
            this.btnAddCostitem.TabIndex = 136;
            this.btnAddCostitem.Text = "批量添加成本项措施费";
            this.btnAddCostitem.UseVisualStyleBackColor = true;
            // 
            // btnCheckNode
            // 
            this.btnCheckNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckNode.Location = new System.Drawing.Point(466, 14);
            this.btnCheckNode.Name = "btnCheckNode";
            this.btnCheckNode.Size = new System.Drawing.Size(75, 23);
            this.btnCheckNode.TabIndex = 135;
            this.btnCheckNode.Text = "确定节点";
            this.btnCheckNode.UseVisualStyleBackColor = true;
            // 
            // txtNode
            // 
            this.txtNode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNode.Location = new System.Drawing.Point(82, 15);
            this.txtNode.Name = "txtNode";
            this.txtNode.ReadOnly = true;
            this.txtNode.Size = new System.Drawing.Size(378, 21);
            this.txtNode.TabIndex = 134;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 133;
            this.label1.Text = "请选择节点：";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(27, 153);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(167, 23);
            this.btnAdd.TabIndex = 132;
            this.btnAdd.Text = "批量添加成本项";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // txtPersonQuotaPrice
            // 
            this.txtPersonQuotaPrice.Location = new System.Drawing.Point(104, 81);
            this.txtPersonQuotaPrice.Name = "txtPersonQuotaPrice";
            this.txtPersonQuotaPrice.Size = new System.Drawing.Size(84, 21);
            this.txtPersonQuotaPrice.TabIndex = 131;
            this.txtPersonQuotaPrice.Text = "55";
            // 
            // cbContentType
            // 
            this.cbContentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbContentType.FormattingEnabled = true;
            this.cbContentType.Location = new System.Drawing.Point(255, 53);
            this.cbContentType.Name = "cbContentType";
            this.cbContentType.Size = new System.Drawing.Size(121, 20);
            this.cbContentType.TabIndex = 128;
            // 
            // cbManagemode
            // 
            this.cbManagemode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbManagemode.FormattingEnabled = true;
            this.cbManagemode.Location = new System.Drawing.Point(443, 53);
            this.cbManagemode.Name = "cbManagemode";
            this.cbManagemode.Size = new System.Drawing.Size(124, 20);
            this.cbManagemode.TabIndex = 129;
            // 
            // cbUsedLevel
            // 
            this.cbUsedLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsedLevel.FormattingEnabled = true;
            this.cbUsedLevel.Location = new System.Drawing.Point(67, 53);
            this.cbUsedLevel.Name = "cbUsedLevel";
            this.cbUsedLevel.Size = new System.Drawing.Size(121, 20);
            this.cbUsedLevel.TabIndex = 130;
            // 
            // cbTableNames
            // 
            this.cbTableNames.BackColor = System.Drawing.Color.White;
            this.cbTableNames.FormattingEnabled = true;
            this.cbTableNames.ItemHeight = 12;
            this.cbTableNames.Location = new System.Drawing.Point(95, 267);
            this.cbTableNames.Name = "cbTableNames";
            this.cbTableNames.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.cbTableNames.Size = new System.Drawing.Size(80, 28);
            this.cbTableNames.TabIndex = 127;
            this.cbTableNames.Visible = false;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(92, 366);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(167, 23);
            this.btnImport.TabIndex = 118;
            this.btnImport.Text = "导入选中表格的成本项数据";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Visible = false;
            // 
            // btnUpdateCostItem
            // 
            this.btnUpdateCostItem.Location = new System.Drawing.Point(67, 468);
            this.btnUpdateCostItem.Name = "btnUpdateCostItem";
            this.btnUpdateCostItem.Size = new System.Drawing.Size(153, 23);
            this.btnUpdateCostItem.TabIndex = 114;
            this.btnUpdateCostItem.Text = "更新成本项信息";
            this.btnUpdateCostItem.UseVisualStyleBackColor = true;
            this.btnUpdateCostItem.Visible = false;
            // 
            // btnUpdateCostItemCateFilter
            // 
            this.btnUpdateCostItemCateFilter.Location = new System.Drawing.Point(403, 462);
            this.btnUpdateCostItemCateFilter.Name = "btnUpdateCostItemCateFilter";
            this.btnUpdateCostItemCateFilter.Size = new System.Drawing.Size(153, 23);
            this.btnUpdateCostItemCateFilter.TabIndex = 113;
            this.btnUpdateCostItemCateFilter.Text = "更新成本项分类过滤条件";
            this.btnUpdateCostItemCateFilter.UseVisualStyleBackColor = true;
            this.btnUpdateCostItemCateFilter.Visible = false;
            // 
            // btnUpdateCategory
            // 
            this.btnUpdateCategory.Location = new System.Drawing.Point(229, 468);
            this.btnUpdateCategory.Name = "btnUpdateCategory";
            this.btnUpdateCategory.Size = new System.Drawing.Size(153, 23);
            this.btnUpdateCategory.TabIndex = 115;
            this.btnUpdateCategory.Text = "更新成本项分类信息";
            this.btnUpdateCategory.UseVisualStyleBackColor = true;
            this.btnUpdateCategory.Visible = false;
            // 
            // btnUpdateCostItemPricing
            // 
            this.btnUpdateCostItemPricing.Location = new System.Drawing.Point(385, 433);
            this.btnUpdateCostItemPricing.Name = "btnUpdateCostItemPricing";
            this.btnUpdateCostItemPricing.Size = new System.Drawing.Size(153, 23);
            this.btnUpdateCostItemPricing.TabIndex = 119;
            this.btnUpdateCostItemPricing.Text = "更新成本项的计价费率";
            this.btnUpdateCostItemPricing.UseVisualStyleBackColor = true;
            this.btnUpdateCostItemPricing.Visible = false;
            // 
            // btnGetQuotaHasQuantity
            // 
            this.btnGetQuotaHasQuantity.Location = new System.Drawing.Point(67, 433);
            this.btnGetQuotaHasQuantity.Name = "btnGetQuotaHasQuantity";
            this.btnGetQuotaHasQuantity.Size = new System.Drawing.Size(312, 23);
            this.btnGetQuotaHasQuantity.TabIndex = 117;
            this.btnGetQuotaHasQuantity.Text = "按系数更新主材的定额工程量（定额所含数量大于1）";
            this.btnGetQuotaHasQuantity.UseVisualStyleBackColor = true;
            this.btnGetQuotaHasQuantity.Visible = false;
            // 
            // btnCostItemMng
            // 
            this.btnCostItemMng.Location = new System.Drawing.Point(408, 153);
            this.btnCostItemMng.Name = "btnCostItemMng";
            this.btnCostItemMng.Size = new System.Drawing.Size(159, 23);
            this.btnCostItemMng.TabIndex = 116;
            this.btnCostItemMng.Text = "成本项管理";
            this.btnCostItemMng.UseVisualStyleBackColor = true;
            // 
            // txtExcelFilePath
            // 
            this.txtExcelFilePath.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtExcelFilePath.DrawSelf = false;
            this.txtExcelFilePath.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtExcelFilePath.EnterToTab = false;
            this.txtExcelFilePath.Location = new System.Drawing.Point(184, 243);
            this.txtExcelFilePath.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtExcelFilePath.Name = "txtExcelFilePath";
            this.txtExcelFilePath.Padding = new System.Windows.Forms.Padding(1);
            this.txtExcelFilePath.ReadOnly = true;
            this.txtExcelFilePath.Size = new System.Drawing.Size(18, 18);
            this.txtExcelFilePath.TabIndex = 126;
            this.txtExcelFilePath.Visible = false;
            // 
            // btnBrownFile
            // 
            this.btnBrownFile.Location = new System.Drawing.Point(214, 246);
            this.btnBrownFile.Name = "btnBrownFile";
            this.btnBrownFile.Size = new System.Drawing.Size(61, 23);
            this.btnBrownFile.TabIndex = 120;
            this.btnBrownFile.Text = "浏览";
            this.btnBrownFile.UseVisualStyleBackColor = true;
            this.btnBrownFile.Visible = false;
            // 
            // customLabel3
            // 
            this.customLabel3.AddColonAuto = true;
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(6, 86);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(101, 12);
            this.customLabel3.TabIndex = 121;
            this.customLabel3.Text = "人工费定额单价：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel2
            // 
            this.customLabel2.AddColonAuto = true;
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(194, 58);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(65, 12);
            this.customLabel2.TabIndex = 123;
            this.customLabel2.Text = "内容类型：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel5
            // 
            this.customLabel5.AddColonAuto = true;
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(382, 58);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(65, 12);
            this.customLabel5.TabIndex = 124;
            this.customLabel5.Text = "管理模式：";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(6, 58);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 122;
            this.customLabel1.Text = "使用级别：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel4
            // 
            this.customLabel4.AddColonAuto = true;
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(93, 246);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(95, 12);
            this.customLabel4.TabIndex = 125;
            this.customLabel4.Text = "Excel文件路径：";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            this.customLabel4.Visible = false;
            // 
            // btnAddCostItem1
            // 
            this.btnAddCostItem1.Location = new System.Drawing.Point(27, 184);
            this.btnAddCostItem1.Name = "btnAddCostItem1";
            this.btnAddCostItem1.Size = new System.Drawing.Size(167, 23);
            this.btnAddCostItem1.TabIndex = 137;
            this.btnAddCostItem1.Text = "批量添加成本项(定额基价)";
            this.btnAddCostItem1.UseVisualStyleBackColor = true;
            // 
            // VCostItemImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(889, 524);
            this.Name = "VCostItemImport";
            this.Text = "安装专业成本项定额数据导入";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtPersonQuotaPrice;
        private System.Windows.Forms.ComboBox cbContentType;
        private System.Windows.Forms.ComboBox cbManagemode;
        private System.Windows.Forms.ComboBox cbUsedLevel;
        private System.Windows.Forms.ListBox cbTableNames;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnUpdateCostItem;
        private System.Windows.Forms.Button btnUpdateCostItemCateFilter;
        private System.Windows.Forms.Button btnUpdateCategory;
        private System.Windows.Forms.Button btnUpdateCostItemPricing;
        private System.Windows.Forms.Button btnGetQuotaHasQuantity;
        private System.Windows.Forms.Button btnCostItemMng;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtExcelFilePath;
        private System.Windows.Forms.Button btnBrownFile;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView tvwCategory;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCheckNode;
        private System.Windows.Forms.TextBox txtNode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddCostitem;
        private System.Windows.Forms.Button btnAddCostItem1;

    }
}