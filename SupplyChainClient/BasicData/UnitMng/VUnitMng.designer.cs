namespace Application.Business.Erp.SupplyChain.Client.BasicData.UnitMng
{
    partial class VUnitMng
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VUnitMng));
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.istTree = new System.Windows.Forms.ImageList(this.components);
            this.gbBasic = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgMessage = new System.Windows.Forms.DataGridView();
            this.colCostId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.mnuTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.�����ӽڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.�޸Ľڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.ɾ���ڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.ɾ����ѡ�ڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.���� = new System.Windows.Forms.ToolStripMenuItem();
            this.����ڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.�����ڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.����ڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.���Ͻڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.���ƹ�ѡ�ڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.ճ���ڵ� = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlFloor.SuspendLayout();
            this.gbBasic.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMessage)).BeginInit();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            // gbBasic
            // 
            this.gbBasic.Controls.Add(this.panel2);
            this.gbBasic.Controls.Add(this.panel1);
            this.gbBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbBasic.Location = new System.Drawing.Point(0, 0);
            this.gbBasic.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this.gbBasic.Name = "gbBasic";
            this.gbBasic.Size = new System.Drawing.Size(600, 424);
            this.gbBasic.TabIndex = 2;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "������λ�б�";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgMessage);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(515, 404);
            this.panel2.TabIndex = 50;
            // 
            // dgMessage
            // 
            this.dgMessage.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMessage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMessage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCostId,
            this.colUnitType,
            this.colUnitName});
            this.dgMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMessage.Location = new System.Drawing.Point(0, 0);
            this.dgMessage.Name = "dgMessage";
            this.dgMessage.RowTemplate.Height = 23;
            this.dgMessage.Size = new System.Drawing.Size(515, 404);
            this.dgMessage.TabIndex = 0;
            // 
            // colCostId
            // 
            this.colCostId.HeaderText = "ҵ�񵥾�";
            this.colCostId.Name = "colCostId";
            // 
            // colUnitType
            // 
            this.colUnitType.HeaderText = "������λ����";
            this.colUnitType.Name = "colUnitType";
            // 
            // colUnitName
            // 
            this.colUnitName.HeaderText = "������λ";
            this.colUnitName.Name = "colUnitName";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(518, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(79, 404);
            this.panel1.TabIndex = 49;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(10, 71);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 23);
            this.btnDelete.TabIndex = 46;
            this.btnDelete.Text = "ɾ��";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(10, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 23);
            this.btnSave.TabIndex = 47;
            this.btnSave.Text = "����";
            this.btnSave.UseVisualStyleBackColor = true;
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
            this.splitContainer1.Panel2.Controls.Add(this.gbBasic);
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
            // mnuTree
            // 
            this.mnuTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.�����ӽڵ�,
            this.�޸Ľڵ�,
            this.ɾ���ڵ�,
            this.ɾ����ѡ�ڵ�,
            this.toolStripSeparator1,
            this.����,
            this.����ڵ�,
            this.toolStripSeparator3,
            this.�����ڵ�,
            this.����ڵ�,
            this.���Ͻڵ�,
            this.toolStripSeparator2,
            this.���ƹ�ѡ�ڵ�,
            this.ճ���ڵ�});
            this.mnuTree.Name = "mnuTree";
            this.mnuTree.Size = new System.Drawing.Size(143, 264);
            // 
            // �����ӽڵ�
            // 
            this.�����ӽڵ�.Name = "�����ӽڵ�";
            this.�����ӽڵ�.Size = new System.Drawing.Size(142, 22);
            this.�����ӽڵ�.Text = "�����ӽڵ�";
            // 
            // �޸Ľڵ�
            // 
            this.�޸Ľڵ�.Name = "�޸Ľڵ�";
            this.�޸Ľڵ�.Size = new System.Drawing.Size(142, 22);
            this.�޸Ľڵ�.Text = "�޸Ľڵ�";
            // 
            // ɾ���ڵ�
            // 
            this.ɾ���ڵ�.Name = "ɾ���ڵ�";
            this.ɾ���ڵ�.Size = new System.Drawing.Size(142, 22);
            this.ɾ���ڵ�.Text = "ɾ���ڵ�";
            // 
            // ɾ����ѡ�ڵ�
            // 
            this.ɾ����ѡ�ڵ�.Name = "ɾ����ѡ�ڵ�";
            this.ɾ����ѡ�ڵ�.Size = new System.Drawing.Size(142, 22);
            this.ɾ����ѡ�ڵ�.Text = "ɾ����ѡ�ڵ�";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // ����
            // 
            this.����.Name = "����";
            this.����.Size = new System.Drawing.Size(142, 22);
            this.����.Text = "����";
            // 
            // ����ڵ�
            // 
            this.����ڵ�.Name = "����ڵ�";
            this.����ڵ�.Size = new System.Drawing.Size(142, 22);
            this.����ڵ�.Text = "����ڵ�";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(139, 6);
            // 
            // �����ڵ�
            // 
            this.�����ڵ�.Name = "�����ڵ�";
            this.�����ڵ�.Size = new System.Drawing.Size(142, 22);
            this.�����ڵ�.Text = "�����ڵ�";
            // 
            // ����ڵ�
            // 
            this.����ڵ�.Name = "����ڵ�";
            this.����ڵ�.Size = new System.Drawing.Size(142, 22);
            this.����ڵ�.Text = "����ڵ�";
            // 
            // ���Ͻڵ�
            // 
            this.���Ͻڵ�.Name = "���Ͻڵ�";
            this.���Ͻڵ�.Size = new System.Drawing.Size(142, 22);
            this.���Ͻڵ�.Text = "���Ͻڵ�";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(139, 6);
            // 
            // ���ƹ�ѡ�ڵ�
            // 
            this.���ƹ�ѡ�ڵ�.Name = "���ƹ�ѡ�ڵ�";
            this.���ƹ�ѡ�ڵ�.Size = new System.Drawing.Size(142, 22);
            this.���ƹ�ѡ�ڵ�.Text = "���ƹ�ѡ�ڵ�";
            // 
            // ճ���ڵ�
            // 
            this.ճ���ڵ�.Name = "ճ���ڵ�";
            this.ճ���ڵ�.Size = new System.Drawing.Size(142, 22);
            this.ճ���ڵ�.Text = "ճ���ڵ�";
            // 
            // VUnitMng
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(850, 493);
            this.Name = "VUnitMng";
            this.Text = "������λ����";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.gbBasic.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgMessage)).EndInit();
            this.panel1.ResumeLayout(false);
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
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView txtCategory;
        private System.Windows.Forms.GroupBox gbBasic;
        private System.Windows.Forms.ContextMenuStrip mnuTree;
        private System.Windows.Forms.ToolStripMenuItem �����ӽڵ�;
        private System.Windows.Forms.ToolStripMenuItem �޸Ľڵ�;
        private System.Windows.Forms.ToolStripMenuItem ɾ���ڵ�;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ����;
        private System.Windows.Forms.ToolStripMenuItem ����ڵ�;
        private System.Windows.Forms.ToolStripMenuItem ���ƹ�ѡ�ڵ�;
        private System.Windows.Forms.ToolStripMenuItem ճ���ڵ�;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ɾ����ѡ�ڵ�;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem �����ڵ�;
        private System.Windows.Forms.ToolStripMenuItem ����ڵ�;
        private System.Windows.Forms.ToolStripMenuItem ���Ͻڵ�;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCostId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitName;
	}
}