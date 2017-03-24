namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng
{
    partial class VCostItemCategory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCostItemCategory));
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.istTree = new System.Windows.Forms.ImageList(this.components);
            this.gbBasic = new System.Windows.Forms.GroupBox();
            this.txtState = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCurrentPath = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.linkCancel = new System.Windows.Forms.LinkLabel();
            this.linkSave = new System.Windows.Forms.LinkLabel();
            this.linkDelete = new System.Windows.Forms.LinkLabel();
            this.linkUpdate = new System.Windows.Forms.LinkLabel();
            this.linkAdd = new System.Windows.Forms.LinkLabel();
            this.lblCurrentPath = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSummary = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtDesc = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblName = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvwCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.mnuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.增加子节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.修改节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除勾选节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.撤销 = new System.Windows.Forms.ToolStripMenuItem();
            this.保存节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.发布节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.冻结节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.作废节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.复制勾选节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴节点 = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlFloor.SuspendLayout();
            this.gbBasic.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mnuTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Size = new System.Drawing.Size(740, 490);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            // 
            // cmsTree
            // 
            this.cmsTree.Name = "cmsTree";
            this.cmsTree.Size = new System.Drawing.Size(61, 4);
            // 
            // istTree
            // 
            this.istTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("istTree.ImageStream")));
            this.istTree.TransparentColor = System.Drawing.Color.Transparent;
            this.istTree.Images.SetKeyName(0, "project.ico");
            this.istTree.Images.SetKeyName(1, "picfolder.ico");
            // 
            // gbBasic
            // 
            this.gbBasic.Controls.Add(this.txtState);
            this.gbBasic.Controls.Add(this.customLabel5);
            this.gbBasic.Controls.Add(this.txtCurrentPath);
            this.gbBasic.Controls.Add(this.linkCancel);
            this.gbBasic.Controls.Add(this.linkSave);
            this.gbBasic.Controls.Add(this.linkDelete);
            this.gbBasic.Controls.Add(this.linkUpdate);
            this.gbBasic.Controls.Add(this.linkAdd);
            this.gbBasic.Controls.Add(this.lblCurrentPath);
            this.gbBasic.Controls.Add(this.customLabel1);
            this.gbBasic.Controls.Add(this.txtCode);
            this.gbBasic.Controls.Add(this.customLabel2);
            this.gbBasic.Controls.Add(this.customLabel3);
            this.gbBasic.Controls.Add(this.txtSummary);
            this.gbBasic.Controls.Add(this.txtDesc);
            this.gbBasic.Controls.Add(this.lblName);
            this.gbBasic.Controls.Add(this.txtName);
            this.gbBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBasic.Location = new System.Drawing.Point(0, 0);
            this.gbBasic.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this.gbBasic.Name = "gbBasic";
            this.gbBasic.Size = new System.Drawing.Size(489, 465);
            this.gbBasic.TabIndex = 2;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "分类信息";
            // 
            // txtState
            // 
            this.txtState.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtState.DrawSelf = false;
            this.txtState.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtState.EnterToTab = false;
            this.txtState.Location = new System.Drawing.Point(83, 206);
            this.txtState.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtState.Name = "txtState";
            this.txtState.Padding = new System.Windows.Forms.Padding(1);
            this.txtState.ReadOnly = false;
            this.txtState.Size = new System.Drawing.Size(122, 16);
            this.txtState.TabIndex = 95;
            // 
            // customLabel5
            // 
            this.customLabel5.AddColonAuto = true;
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(42, 210);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(41, 12);
            this.customLabel5.TabIndex = 94;
            this.customLabel5.Text = "状态：";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCurrentPath
            // 
            this.txtCurrentPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentPath.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCurrentPath.DrawSelf = false;
            this.txtCurrentPath.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCurrentPath.EnterToTab = false;
            this.txtCurrentPath.Location = new System.Drawing.Point(83, 51);
            this.txtCurrentPath.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCurrentPath.Name = "txtCurrentPath";
            this.txtCurrentPath.Padding = new System.Windows.Forms.Padding(1);
            this.txtCurrentPath.ReadOnly = true;
            this.txtCurrentPath.Size = new System.Drawing.Size(387, 18);
            this.txtCurrentPath.TabIndex = 15;
            // 
            // linkCancel
            // 
            this.linkCancel.AutoSize = true;
            this.linkCancel.Location = new System.Drawing.Point(248, 27);
            this.linkCancel.Name = "linkCancel";
            this.linkCancel.Size = new System.Drawing.Size(29, 12);
            this.linkCancel.TabIndex = 18;
            this.linkCancel.TabStop = true;
            this.linkCancel.Text = "撤销";
            this.linkCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCancel_LinkClicked);
            // 
            // linkSave
            // 
            this.linkSave.AutoSize = true;
            this.linkSave.Location = new System.Drawing.Point(300, 27);
            this.linkSave.Name = "linkSave";
            this.linkSave.Size = new System.Drawing.Size(53, 12);
            this.linkSave.TabIndex = 18;
            this.linkSave.TabStop = true;
            this.linkSave.Text = "保存节点";
            this.linkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkSave_LinkClicked);
            // 
            // linkDelete
            // 
            this.linkDelete.AutoSize = true;
            this.linkDelete.Location = new System.Drawing.Point(172, 27);
            this.linkDelete.Name = "linkDelete";
            this.linkDelete.Size = new System.Drawing.Size(53, 12);
            this.linkDelete.TabIndex = 18;
            this.linkDelete.TabStop = true;
            this.linkDelete.Text = "删除节点";
            this.linkDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDelete_LinkClicked);
            // 
            // linkUpdate
            // 
            this.linkUpdate.AutoSize = true;
            this.linkUpdate.Location = new System.Drawing.Point(101, 27);
            this.linkUpdate.Name = "linkUpdate";
            this.linkUpdate.Size = new System.Drawing.Size(53, 12);
            this.linkUpdate.TabIndex = 18;
            this.linkUpdate.TabStop = true;
            this.linkUpdate.Text = "修改节点";
            this.linkUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkUpdate_LinkClicked);
            // 
            // linkAdd
            // 
            this.linkAdd.AutoSize = true;
            this.linkAdd.Location = new System.Drawing.Point(18, 27);
            this.linkAdd.Name = "linkAdd";
            this.linkAdd.Size = new System.Drawing.Size(65, 12);
            this.linkAdd.TabIndex = 18;
            this.linkAdd.TabStop = true;
            this.linkAdd.Text = "增加子节点";
            this.linkAdd.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkAdd_LinkClicked);
            // 
            // lblCurrentPath
            // 
            this.lblCurrentPath.AddColonAuto = true;
            this.lblCurrentPath.AutoSize = true;
            this.lblCurrentPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentPath.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCurrentPath.Location = new System.Drawing.Point(18, 55);
            this.lblCurrentPath.Name = "lblCurrentPath";
            this.lblCurrentPath.Size = new System.Drawing.Size(65, 12);
            this.lblCurrentPath.TabIndex = 13;
            this.lblCurrentPath.Text = "当前路径：";
            this.lblCurrentPath.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(18, 86);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 15;
            this.customLabel1.Text = "分类编码：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(83, 83);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = false;
            this.txtCode.Size = new System.Drawing.Size(387, 16);
            this.txtCode.TabIndex = 16;
            // 
            // customLabel2
            // 
            this.customLabel2.AddColonAuto = true;
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(42, 179);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(41, 12);
            this.customLabel2.TabIndex = 15;
            this.customLabel2.Text = "摘要：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AddColonAuto = true;
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(18, 150);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(65, 12);
            this.customLabel3.TabIndex = 15;
            this.customLabel3.Text = "分类说明：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSummary
            // 
            this.txtSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSummary.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSummary.DrawSelf = false;
            this.txtSummary.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSummary.EnterToTab = false;
            this.txtSummary.Location = new System.Drawing.Point(83, 175);
            this.txtSummary.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.Padding = new System.Windows.Forms.Padding(1);
            this.txtSummary.ReadOnly = false;
            this.txtSummary.Size = new System.Drawing.Size(387, 16);
            this.txtSummary.TabIndex = 16;
            // 
            // txtDesc
            // 
            this.txtDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDesc.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDesc.DrawSelf = false;
            this.txtDesc.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDesc.EnterToTab = false;
            this.txtDesc.Location = new System.Drawing.Point(83, 146);
            this.txtDesc.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Padding = new System.Windows.Forms.Padding(1);
            this.txtDesc.ReadOnly = false;
            this.txtDesc.Size = new System.Drawing.Size(387, 16);
            this.txtDesc.TabIndex = 16;
            // 
            // lblName
            // 
            this.lblName.AddColonAuto = true;
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblName.Location = new System.Drawing.Point(18, 118);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(65, 12);
            this.lblName.TabIndex = 15;
            this.lblName.Text = "分类名称：";
            this.lblName.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = false;
            this.txtName.Location = new System.Drawing.Point(83, 114);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = false;
            this.txtName.Size = new System.Drawing.Size(387, 16);
            this.txtName.TabIndex = 16;
            // 
            // splitContainer1
            // 
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
            this.splitContainer1.Panel2.Controls.Add(this.gbBasic);
            this.splitContainer1.Size = new System.Drawing.Size(740, 465);
            this.splitContainer1.SplitterDistance = 247;
            this.splitContainer1.TabIndex = 17;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // tvwCategory
            // 
            this.tvwCategory.AllowDrop = true;
            this.tvwCategory.BackColor = System.Drawing.SystemColors.Window;
            this.tvwCategory.CheckBoxes = true;
            this.tvwCategory.ContextMenuStrip = this.cmsTree;
            this.tvwCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwCategory.HideSelection = false;
            this.tvwCategory.ImageIndex = 0;
            this.tvwCategory.ImageList = this.istTree;
            this.tvwCategory.Location = new System.Drawing.Point(0, 0);
            this.tvwCategory.Name = "tvwCategory";
            this.tvwCategory.SelectedImageIndex = 0;
            this.tvwCategory.Size = new System.Drawing.Size(247, 465);
            this.tvwCategory.TabIndex = 0;
            // 
            // mnuTree
            // 
            this.mnuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.增加子节点,
            this.修改节点,
            this.删除节点,
            this.删除勾选节点,
            this.toolStripSeparator1,
            this.撤销,
            this.保存节点,
            this.toolStripSeparator3,
            this.发布节点,
            this.冻结节点,
            this.作废节点,
            this.toolStripSeparator2,
            this.复制勾选节点,
            this.粘贴节点});
            this.mnuTree.Name = "mnuTree";
            this.mnuTree.Size = new System.Drawing.Size(143, 264);
            // 
            // 增加子节点
            // 
            this.增加子节点.Name = "增加子节点";
            this.增加子节点.Size = new System.Drawing.Size(142, 22);
            this.增加子节点.Text = "增加子节点";
            // 
            // 修改节点
            // 
            this.修改节点.Name = "修改节点";
            this.修改节点.Size = new System.Drawing.Size(142, 22);
            this.修改节点.Text = "修改节点";
            // 
            // 删除节点
            // 
            this.删除节点.Name = "删除节点";
            this.删除节点.Size = new System.Drawing.Size(142, 22);
            this.删除节点.Text = "删除节点";
            // 
            // 删除勾选节点
            // 
            this.删除勾选节点.Name = "删除勾选节点";
            this.删除勾选节点.Size = new System.Drawing.Size(142, 22);
            this.删除勾选节点.Text = "删除勾选节点";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // 撤销
            // 
            this.撤销.Name = "撤销";
            this.撤销.Size = new System.Drawing.Size(142, 22);
            this.撤销.Text = "撤销";
            // 
            // 保存节点
            // 
            this.保存节点.Name = "保存节点";
            this.保存节点.Size = new System.Drawing.Size(142, 22);
            this.保存节点.Text = "保存节点";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(139, 6);
            // 
            // 发布节点
            // 
            this.发布节点.Name = "发布节点";
            this.发布节点.Size = new System.Drawing.Size(142, 22);
            this.发布节点.Text = "发布节点";
            // 
            // 冻结节点
            // 
            this.冻结节点.Name = "冻结节点";
            this.冻结节点.Size = new System.Drawing.Size(142, 22);
            this.冻结节点.Text = "冻结节点";
            // 
            // 作废节点
            // 
            this.作废节点.Name = "作废节点";
            this.作废节点.Size = new System.Drawing.Size(142, 22);
            this.作废节点.Text = "作废节点";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(139, 6);
            // 
            // 复制勾选节点
            // 
            this.复制勾选节点.Name = "复制勾选节点";
            this.复制勾选节点.Size = new System.Drawing.Size(142, 22);
            this.复制勾选节点.Text = "复制勾选节点";
            // 
            // 粘贴节点
            // 
            this.粘贴节点.Name = "粘贴节点";
            this.粘贴节点.Size = new System.Drawing.Size(142, 22);
            this.粘贴节点.Text = "粘贴节点";
            // 
            // VCostItemCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(740, 490);
            this.Name = "VCostItemCategory";
            this.Text = "成本项分类管理";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.gbBasic.ResumeLayout(false);
            this.gbBasic.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.mnuTree.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ContextMenuStrip cmsTree;
        private System.Windows.Forms.ImageList istTree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView tvwCategory;
        private System.Windows.Forms.GroupBox gbBasic;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCurrentPath;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private System.Windows.Forms.ContextMenuStrip mnuTree;
        private System.Windows.Forms.ToolStripMenuItem 增加子节点;
        private System.Windows.Forms.ToolStripMenuItem 修改节点;
        private System.Windows.Forms.ToolStripMenuItem 删除节点;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 撤销;
        private System.Windows.Forms.ToolStripMenuItem 保存节点;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDesc;
        private System.Windows.Forms.LinkLabel linkSave;
        private System.Windows.Forms.LinkLabel linkDelete;
        private System.Windows.Forms.LinkLabel linkUpdate;
        private System.Windows.Forms.LinkLabel linkAdd;
        private System.Windows.Forms.LinkLabel linkCancel;
        private System.Windows.Forms.ToolStripMenuItem 复制勾选节点;
        private System.Windows.Forms.ToolStripMenuItem 粘贴节点;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 删除勾选节点;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSummary;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCurrentPath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 发布节点;
        private System.Windows.Forms.ToolStripMenuItem 冻结节点;
        private System.Windows.Forms.ToolStripMenuItem 作废节点;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtState;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
	}
}