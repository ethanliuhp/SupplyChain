namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    partial class VPBSBuilding
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
            this.gbBasic = new System.Windows.Forms.GroupBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.txtConstructionArea = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvTree = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.mnuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnReset = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRename = new System.Windows.Forms.ToolStripMenuItem();
            this.上移 = new System.Windows.Forms.ToolStripMenuItem();
            this.下移 = new System.Windows.Forms.ToolStripMenuItem();
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
            this.pnlFloor.Margin = new System.Windows.Forms.Padding(4);
            this.pnlFloor.Size = new System.Drawing.Size(879, 502);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            // 
            // gbBasic
            // 
            this.gbBasic.Controls.Add(this.txtRemark);
            this.gbBasic.Controls.Add(this.txtConstructionArea);
            this.gbBasic.Controls.Add(this.txtType);
            this.gbBasic.Controls.Add(this.txtName);
            this.gbBasic.Controls.Add(this.txtCode);
            this.gbBasic.Controls.Add(this.txtPath);
            this.gbBasic.Controls.Add(this.btnSave);
            this.gbBasic.Controls.Add(this.label6);
            this.gbBasic.Controls.Add(this.label5);
            this.gbBasic.Controls.Add(this.label4);
            this.gbBasic.Controls.Add(this.label3);
            this.gbBasic.Controls.Add(this.label2);
            this.gbBasic.Controls.Add(this.label1);
            this.gbBasic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbBasic.Location = new System.Drawing.Point(0, 0);
            this.gbBasic.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this.gbBasic.Name = "gbBasic";
            this.gbBasic.Size = new System.Drawing.Size(665, 502);
            this.gbBasic.TabIndex = 2;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "建筑结构详细信息";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(130, 322);
            this.txtRemark.Margin = new System.Windows.Forms.Padding(2);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ReadOnly = true;
            this.txtRemark.Size = new System.Drawing.Size(325, 21);
            this.txtRemark.TabIndex = 155;
            // 
            // txtConstructionArea
            // 
            this.txtConstructionArea.Location = new System.Drawing.Point(130, 264);
            this.txtConstructionArea.Margin = new System.Windows.Forms.Padding(2);
            this.txtConstructionArea.Name = "txtConstructionArea";
            this.txtConstructionArea.ReadOnly = true;
            this.txtConstructionArea.Size = new System.Drawing.Size(325, 21);
            this.txtConstructionArea.TabIndex = 154;
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(130, 218);
            this.txtType.Margin = new System.Windows.Forms.Padding(2);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(325, 21);
            this.txtType.TabIndex = 154;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(130, 162);
            this.txtName.Margin = new System.Windows.Forms.Padding(2);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(325, 21);
            this.txtName.TabIndex = 153;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(130, 106);
            this.txtCode.Margin = new System.Windows.Forms.Padding(2);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(325, 21);
            this.txtCode.TabIndex = 152;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(130, 52);
            this.txtPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(325, 21);
            this.txtPath.TabIndex = 151;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(398, 381);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(56, 29);
            this.btnSave.TabIndex = 150;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 267);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 146;
            this.label6.Text = "建筑面积：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(86, 325);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 148;
            this.label5.Text = "描述：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 221);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 146;
            this.label4.Text = "结构类型：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 164);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 144;
            this.label3.Text = "结构名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 109);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 142;
            this.label2.Text = "结构编码：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 54);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前节点路径：";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbBasic);
            this.splitContainer1.Size = new System.Drawing.Size(879, 502);
            this.splitContainer1.SplitterDistance = 210;
            this.splitContainer1.TabIndex = 17;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // tvTree
            // 
            this.tvTree.AllowDrop = true;
            this.tvTree.BackColor = System.Drawing.SystemColors.Window;
            this.tvTree.CheckBoxes = true;
            this.tvTree.ContextMenuStrip = this.mnuTree;
            this.tvTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTree.HideSelection = false;
            this.tvTree.Location = new System.Drawing.Point(0, 0);
            this.tvTree.Name = "tvTree";
            this.tvTree.Size = new System.Drawing.Size(210, 502);
            this.tvTree.TabIndex = 0;
            // 
            // mnuTree
            // 
            this.mnuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete,
            this.btnUpdate,
            this.toolStripSeparator1,
            this.btnDelAll,
            this.toolStripSeparator2,
            this.btnReset,
            this.btnRename,
            this.上移,
            this.下移});
            this.mnuTree.Name = "mnuTree";
            this.mnuTree.Size = new System.Drawing.Size(197, 192);
            // 
            // btnAdd
            // 
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(196, 22);
            this.btnAdd.Text = "增加子节点";
            // 
            // btnDelete
            // 
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(196, 22);
            this.btnDelete.Text = "删除节点";
            this.btnDelete.Visible = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(196, 22);
            this.btnUpdate.Text = "修改节点";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // btnDelAll
            // 
            this.btnDelAll.Name = "btnDelAll";
            this.btnDelAll.Size = new System.Drawing.Size(196, 22);
            this.btnDelAll.Text = "删除选中节点";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // btnReset
            // 
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(196, 22);
            this.btnReset.Text = "重新选择PBS模版";
            // 
            // btnRename
            // 
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(196, 22);
            this.btnRename.Text = "批量变更施工部位名称";
            // 
            // 上移
            // 
            this.上移.Name = "上移";
            this.上移.Size = new System.Drawing.Size(196, 22);
            this.上移.Text = "上移";
            // 
            // 下移
            // 
            this.下移.Name = "下移";
            this.下移.Size = new System.Drawing.Size(196, 22);
            this.下移.Text = "下移";
            // 
            // VPBSBuilding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(879, 502);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "VPBSBuilding";
            this.Text = "PBS节点维护";
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

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gbBasic;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView tvTree;
        private System.Windows.Forms.ContextMenuStrip mnuTree;
        private System.Windows.Forms.ToolStripMenuItem btnAdd;
        private System.Windows.Forms.ToolStripMenuItem btnDelete;
        private System.Windows.Forms.ToolStripMenuItem btnUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem btnDelAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem btnReset;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtConstructionArea;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem btnRename;
        private System.Windows.Forms.ToolStripMenuItem 上移;
        private System.Windows.Forms.ToolStripMenuItem 下移;
	}
}