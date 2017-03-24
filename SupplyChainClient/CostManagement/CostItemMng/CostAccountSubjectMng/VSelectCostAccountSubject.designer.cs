namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng
{
    partial class VSelectCostAccountSubject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSelectCostAccountSubject));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvwCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.istTree = new System.Windows.Forms.ImageList(this.components);
            this.txtSummary = new System.Windows.Forms.TextBox();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.btnEnter = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtOwner = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtState = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtCurrentPath = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblCurrentPath = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblName = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Size = new System.Drawing.Size(652, 490);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
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
            this.splitContainer1.Panel2.Controls.Add(this.txtSummary);
            this.splitContainer1.Panel2.Controls.Add(this.txtDesc);
            this.splitContainer1.Panel2.Controls.Add(this.btnEnter);
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.txtOwner);
            this.splitContainer1.Panel2.Controls.Add(this.txtState);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel5);
            this.splitContainer1.Panel2.Controls.Add(this.txtName);
            this.splitContainer1.Panel2.Controls.Add(this.txtCurrentPath);
            this.splitContainer1.Panel2.Controls.Add(this.lblCurrentPath);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel1);
            this.splitContainer1.Panel2.Controls.Add(this.txtCode);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel2);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel3);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel4);
            this.splitContainer1.Panel2.Controls.Add(this.lblName);
            this.splitContainer1.Size = new System.Drawing.Size(652, 465);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 17;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // tvwCategory
            // 
            this.tvwCategory.AllowDrop = true;
            this.tvwCategory.BackColor = System.Drawing.SystemColors.Window;
            this.tvwCategory.CheckBoxes = true;
            this.tvwCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwCategory.HideSelection = false;
            this.tvwCategory.ImageIndex = 0;
            this.tvwCategory.ImageList = this.istTree;
            this.tvwCategory.Location = new System.Drawing.Point(0, 0);
            this.tvwCategory.Name = "tvwCategory";
            this.tvwCategory.SelectedImageIndex = 0;
            this.tvwCategory.Size = new System.Drawing.Size(250, 465);
            this.tvwCategory.TabIndex = 0;
            // 
            // istTree
            // 
            this.istTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("istTree.ImageStream")));
            this.istTree.TransparentColor = System.Drawing.Color.Transparent;
            this.istTree.Images.SetKeyName(0, "project.ico");
            this.istTree.Images.SetKeyName(1, "picfolder.ico");
            // 
            // txtSummary
            // 
            this.txtSummary.BackColor = System.Drawing.SystemColors.Control;
            this.txtSummary.Location = new System.Drawing.Point(81, 277);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ReadOnly = true;
            this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSummary.Size = new System.Drawing.Size(177, 84);
            this.txtSummary.TabIndex = 107;
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.SystemColors.Control;
            this.txtDesc.Location = new System.Drawing.Point(81, 177);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ReadOnly = true;
            this.txtDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDesc.Size = new System.Drawing.Size(177, 84);
            this.txtDesc.TabIndex = 107;
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(42, 393);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(102, 23);
            this.btnEnter.TabIndex = 106;
            this.btnEnter.Text = "确定";
            this.btnEnter.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(156, 393);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 106;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtOwner
            // 
            this.txtOwner.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOwner.DrawSelf = false;
            this.txtOwner.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOwner.EnterToTab = false;
            this.txtOwner.Location = new System.Drawing.Point(81, 143);
            this.txtOwner.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOwner.Name = "txtOwner";
            this.txtOwner.Padding = new System.Windows.Forms.Padding(1);
            this.txtOwner.ReadOnly = false;
            this.txtOwner.Size = new System.Drawing.Size(177, 16);
            this.txtOwner.TabIndex = 104;
            // 
            // txtState
            // 
            this.txtState.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtState.DrawSelf = false;
            this.txtState.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtState.EnterToTab = false;
            this.txtState.Location = new System.Drawing.Point(81, 110);
            this.txtState.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtState.Name = "txtState";
            this.txtState.Padding = new System.Windows.Forms.Padding(1);
            this.txtState.ReadOnly = false;
            this.txtState.Size = new System.Drawing.Size(177, 16);
            this.txtState.TabIndex = 105;
            // 
            // customLabel5
            // 
            this.customLabel5.AddColonAuto = true;
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(40, 114);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(41, 12);
            this.customLabel5.TabIndex = 103;
            this.customLabel5.Text = "状态：";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtName
            // 
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = false;
            this.txtName.Location = new System.Drawing.Point(81, 79);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = false;
            this.txtName.Size = new System.Drawing.Size(177, 16);
            this.txtName.TabIndex = 102;
            // 
            // txtCurrentPath
            // 
            this.txtCurrentPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentPath.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCurrentPath.DrawSelf = false;
            this.txtCurrentPath.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCurrentPath.EnterToTab = false;
            this.txtCurrentPath.Location = new System.Drawing.Point(81, 16);
            this.txtCurrentPath.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCurrentPath.Name = "txtCurrentPath";
            this.txtCurrentPath.Padding = new System.Windows.Forms.Padding(1);
            this.txtCurrentPath.ReadOnly = true;
            this.txtCurrentPath.Size = new System.Drawing.Size(305, 18);
            this.txtCurrentPath.TabIndex = 98;
            // 
            // lblCurrentPath
            // 
            this.lblCurrentPath.AddColonAuto = true;
            this.lblCurrentPath.AutoSize = true;
            this.lblCurrentPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentPath.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCurrentPath.Location = new System.Drawing.Point(16, 20);
            this.lblCurrentPath.Name = "lblCurrentPath";
            this.lblCurrentPath.Size = new System.Drawing.Size(65, 12);
            this.lblCurrentPath.TabIndex = 92;
            this.lblCurrentPath.Text = "当前路径：";
            this.lblCurrentPath.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(16, 51);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 97;
            this.customLabel1.Text = "科目编码：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(81, 48);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = false;
            this.txtCode.Size = new System.Drawing.Size(177, 16);
            this.txtCode.TabIndex = 101;
            // 
            // customLabel2
            // 
            this.customLabel2.AddColonAuto = true;
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(40, 280);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(41, 12);
            this.customLabel2.TabIndex = 96;
            this.customLabel2.Text = "摘要：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AddColonAuto = true;
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(16, 180);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(65, 12);
            this.customLabel3.TabIndex = 95;
            this.customLabel3.Text = "科目说明：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel4
            // 
            this.customLabel4.AddColonAuto = true;
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(28, 146);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(53, 12);
            this.customLabel4.TabIndex = 94;
            this.customLabel4.Text = "责任人：";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblName
            // 
            this.lblName.AddColonAuto = true;
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblName.Location = new System.Drawing.Point(16, 79);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(65, 12);
            this.lblName.TabIndex = 93;
            this.lblName.Text = "科目名称：";
            this.lblName.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VSelectCostAccountSubject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(652, 490);
            this.Name = "VSelectCostAccountSubject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择核算科目";
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
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView tvwCategory;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Button btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOwner;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtState;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCurrentPath;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblName;
        private System.Windows.Forms.TextBox txtSummary;
        private System.Windows.Forms.TextBox txtDesc;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCurrentPath;
        private System.Windows.Forms.ImageList istTree;
	}
}