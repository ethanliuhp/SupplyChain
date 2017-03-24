namespace Application.Business.Erp.SupplyChain.Client.ExcelImportMng
{
    partial class VExcelImportMng
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VExcelImportMng));
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.istTree = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.btnAdd = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtfileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_fileView = new System.Windows.Forms.Button();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnExcelImport = new VirtualMachine.Component.WinControls.Controls.CustomButton();
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
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.mnuTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Size = new System.Drawing.Size(850, 493);
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtCategory);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnAdd);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(850, 493);
            this.splitContainer1.SplitterDistance = 246;
            this.splitContainer1.TabIndex = 17;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // txtCategory
            // 
            this.txtCategory.AllowDrop = true;
            this.txtCategory.BackColor = System.Drawing.SystemColors.Window;
            this.txtCategory.CheckBoxes = true;
            this.txtCategory.ContextMenuStrip = this.cmsTree;
            this.txtCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCategory.HideSelection = false;
            this.txtCategory.ImageIndex = 0;
            this.txtCategory.ImageList = this.istTree;
            this.txtCategory.Location = new System.Drawing.Point(0, 0);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.SelectedImageIndex = 0;
            this.txtCategory.Size = new System.Drawing.Size(246, 493);
            this.txtCategory.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnAdd.Location = new System.Drawing.Point(248, 223);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "批量添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtfileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_fileView);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnExcelImport);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 144);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "物料分类基础数据导入";
            // 
            // txtfileName
            // 
            this.txtfileName.Location = new System.Drawing.Point(66, 46);
            this.txtfileName.Name = "txtfileName";
            this.txtfileName.ReadOnly = true;
            this.txtfileName.Size = new System.Drawing.Size(268, 21);
            this.txtfileName.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "文件名:";
            // 
            // btn_fileView
            // 
            this.btn_fileView.Location = new System.Drawing.Point(341, 44);
            this.btn_fileView.Name = "btn_fileView";
            this.btn_fileView.Size = new System.Drawing.Size(75, 23);
            this.btn_fileView.TabIndex = 15;
            this.btn_fileView.Text = "浏览文件";
            this.btn_fileView.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(224, 98);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnExcelImport
            // 
            this.btnExcelImport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcelImport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcelImport.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcelImport.Location = new System.Drawing.Point(138, 98);
            this.btnExcelImport.Name = "btnExcelImport";
            this.btnExcelImport.Size = new System.Drawing.Size(75, 23);
            this.btnExcelImport.TabIndex = 11;
            this.btnExcelImport.Text = "导入";
            this.btnExcelImport.UseVisualStyleBackColor = true;
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
            // VExcelImportMng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(850, 493);
            this.Name = "VExcelImportMng";
            this.Text = "计量单位配置";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.mnuTree.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ContextMenuStrip cmsTree;
        private System.Windows.Forms.ImageList istTree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView txtCategory;
        private System.Windows.Forms.ContextMenuStrip mnuTree;
        private System.Windows.Forms.ToolStripMenuItem 增加子节点;
        private System.Windows.Forms.ToolStripMenuItem 修改节点;
        private System.Windows.Forms.ToolStripMenuItem 删除节点;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 撤销;
        private System.Windows.Forms.ToolStripMenuItem 保存节点;
        private System.Windows.Forms.ToolStripMenuItem 复制勾选节点;
        private System.Windows.Forms.ToolStripMenuItem 粘贴节点;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 删除勾选节点;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 发布节点;
        private System.Windows.Forms.ToolStripMenuItem 冻结节点;
        private System.Windows.Forms.ToolStripMenuItem 作废节点;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtfileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_fileView;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcelImport;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnAdd;
	}
}