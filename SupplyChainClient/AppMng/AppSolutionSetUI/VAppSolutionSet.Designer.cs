namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI
{
    partial class VAppSolutionSet
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VAppSolutionSet));
            this.Dgv = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnSolutionDelete = new System.Windows.Forms.Button();
            this.btnSolutionSave = new System.Windows.Forms.Button();
            this.CboxTable = new System.Windows.Forms.ComboBox();
            this.FgSolution = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.FgRole = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.BtnRoleDel = new System.Windows.Forms.Button();
            this.BtnRoleSave = new System.Windows.Forms.Button();
            this.CboxStep = new System.Windows.Forms.ComboBox();
            this.CboxSolution = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.btnAddStep = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.FgSetp = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FgSolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FgRole)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FgSetp)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.CboxSolution);
            this.pnlFloor.Controls.Add(this.CboxStep);
            this.pnlFloor.Controls.Add(this.btnSolutionDelete);
            this.pnlFloor.Controls.Add(this.CboxTable);
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Controls.Add(this.btnSolutionSave);
            this.pnlFloor.Size = new System.Drawing.Size(1072, 539);
            this.pnlFloor.Controls.SetChildIndex(this.btnSolutionSave, 0);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.CboxTable, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSolutionDelete, 0);
            this.pnlFloor.Controls.SetChildIndex(this.CboxStep, 0);
            this.pnlFloor.Controls.SetChildIndex(this.CboxSolution, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(127, 9);
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.Text = "审批表单定义";
            // 
            // Dgv
            // 
            this.Dgv.AllowAddNew = true;
            this.Dgv.AllowEditing = false;
            this.Dgv.ColumnInfo = resources.GetString("Dgv.ColumnInfo");
            this.Dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv.Location = new System.Drawing.Point(0, 0);
            this.Dgv.Name = "Dgv";
            this.Dgv.Rows.Count = 1;
            this.Dgv.Rows.DefaultSize = 18;
            this.Dgv.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.Dgv.Size = new System.Drawing.Size(145, 491);
            this.Dgv.TabIndex = 12;
            // 
            // btnSolutionDelete
            // 
            this.btnSolutionDelete.Location = new System.Drawing.Point(915, 7);
            this.btnSolutionDelete.Name = "btnSolutionDelete";
            this.btnSolutionDelete.Size = new System.Drawing.Size(87, 23);
            this.btnSolutionDelete.TabIndex = 19;
            this.btnSolutionDelete.Text = "删除当前方案";
            this.btnSolutionDelete.UseVisualStyleBackColor = true;
            this.btnSolutionDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSolutionSave
            // 
            this.btnSolutionSave.Location = new System.Drawing.Point(822, 7);
            this.btnSolutionSave.Name = "btnSolutionSave";
            this.btnSolutionSave.Size = new System.Drawing.Size(87, 23);
            this.btnSolutionSave.TabIndex = 17;
            this.btnSolutionSave.Text = "保存当前方案";
            this.btnSolutionSave.UseVisualStyleBackColor = true;
            // 
            // CboxTable
            // 
            this.CboxTable.FormattingEnabled = true;
            this.CboxTable.Items.AddRange(new object[] {
            "启用",
            "停用"});
            this.CboxTable.Location = new System.Drawing.Point(12, 7);
            this.CboxTable.Name = "CboxTable";
            this.CboxTable.Size = new System.Drawing.Size(57, 20);
            this.CboxTable.TabIndex = 20;
            this.CboxTable.Visible = false;
            // 
            // FgSolution
            // 
            this.FgSolution.AllowAddNew = true;
            this.FgSolution.ColumnInfo = resources.GetString("FgSolution.ColumnInfo");
            this.FgSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FgSolution.Location = new System.Drawing.Point(0, 0);
            this.FgSolution.Name = "FgSolution";
            this.FgSolution.Rows.Count = 1;
            this.FgSolution.Rows.DefaultSize = 18;
            this.FgSolution.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.FgSolution.Size = new System.Drawing.Size(915, 115);
            this.FgSolution.TabIndex = 21;
            // 
            // FgRole
            // 
            this.FgRole.AllowAddNew = true;
            this.FgRole.AllowDelete = true;
            this.FgRole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FgRole.ColumnInfo = resources.GetString("FgRole.ColumnInfo");
            this.FgRole.Location = new System.Drawing.Point(3, 31);
            this.FgRole.Name = "FgRole";
            this.FgRole.Rows.Count = 1;
            this.FgRole.Rows.DefaultSize = 18;
            this.FgRole.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.FgRole.Size = new System.Drawing.Size(373, 344);
            this.FgRole.TabIndex = 23;
            // 
            // BtnRoleDel
            // 
            this.BtnRoleDel.Location = new System.Drawing.Point(106, 2);
            this.BtnRoleDel.Name = "BtnRoleDel";
            this.BtnRoleDel.Size = new System.Drawing.Size(87, 23);
            this.BtnRoleDel.TabIndex = 27;
            this.BtnRoleDel.Text = "删除审批流程";
            this.BtnRoleDel.UseVisualStyleBackColor = true;
            // 
            // BtnRoleSave
            // 
            this.BtnRoleSave.Location = new System.Drawing.Point(3, 2);
            this.BtnRoleSave.Name = "BtnRoleSave";
            this.BtnRoleSave.Size = new System.Drawing.Size(87, 23);
            this.BtnRoleSave.TabIndex = 26;
            this.BtnRoleSave.Text = "保存审批流程";
            this.BtnRoleSave.UseVisualStyleBackColor = true;
            // 
            // CboxStep
            // 
            this.CboxStep.FormattingEnabled = true;
            this.CboxStep.Items.AddRange(new object[] {
            "或",
            "与"});
            this.CboxStep.Location = new System.Drawing.Point(70, 6);
            this.CboxStep.Name = "CboxStep";
            this.CboxStep.Size = new System.Drawing.Size(56, 20);
            this.CboxStep.TabIndex = 28;
            this.CboxStep.Visible = false;
            // 
            // CboxSolution
            // 
            this.CboxSolution.FormattingEnabled = true;
            this.CboxSolution.Items.AddRange(new object[] {
            "无",
            "默认"});
            this.CboxSolution.Location = new System.Drawing.Point(131, 5);
            this.CboxSolution.Name = "CboxSolution";
            this.CboxSolution.Size = new System.Drawing.Size(67, 20);
            this.CboxSolution.TabIndex = 29;
            this.CboxSolution.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 36);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Dgv);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1064, 491);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.TabIndex = 30;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.FgSolution);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(915, 491);
            this.splitContainer2.SplitterDistance = 115;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.btnAddStep);
            this.splitContainer3.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel1);
            this.splitContainer3.Panel2.Controls.Add(this.FgRole);
            this.splitContainer3.Size = new System.Drawing.Size(915, 372);
            this.splitContainer3.SplitterDistance = 535;
            this.splitContainer3.TabIndex = 0;
            // 
            // btnAddStep
            // 
            this.btnAddStep.Location = new System.Drawing.Point(7, 343);
            this.btnAddStep.Name = "btnAddStep";
            this.btnAddStep.Size = new System.Drawing.Size(109, 23);
            this.btnAddStep.TabIndex = 1;
            this.btnAddStep.Text = "新增审批步骤";
            this.btnAddStep.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.FgSetp);
            this.panel2.Location = new System.Drawing.Point(2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(535, 330);
            this.panel2.TabIndex = 0;
            // 
            // FgSetp
            // 
            this.FgSetp.AllowDelete = true;
            this.FgSetp.ColumnInfo = resources.GetString("FgSetp.ColumnInfo");
            this.FgSetp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FgSetp.Location = new System.Drawing.Point(0, 0);
            this.FgSetp.Name = "FgSetp";
            this.FgSetp.Rows.Count = 1;
            this.FgSetp.Rows.DefaultSize = 18;
            this.FgSetp.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.FgSetp.Size = new System.Drawing.Size(535, 330);
            this.FgSetp.TabIndex = 28;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnRoleSave);
            this.panel1.Controls.Add(this.BtnRoleDel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(376, 29);
            this.panel1.TabIndex = 0;
            // 
            // VAppSolutionSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 539);
            this.Name = "VAppSolutionSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "审批表单定义";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FgSolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FgRole)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FgSetp)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSolutionDelete;
        private System.Windows.Forms.Button btnSolutionSave;
        private C1.Win.C1FlexGrid.C1FlexGrid Dgv;
        private System.Windows.Forms.ComboBox CboxTable;
        private C1.Win.C1FlexGrid.C1FlexGrid FgRole;
        private C1.Win.C1FlexGrid.C1FlexGrid FgSolution;
        private System.Windows.Forms.Button BtnRoleDel;
        private System.Windows.Forms.Button BtnRoleSave;
        private System.Windows.Forms.ComboBox CboxStep;
        private System.Windows.Forms.ComboBox CboxSolution;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAddStep;
        private C1.Win.C1FlexGrid.C1FlexGrid FgSetp;

    }
}