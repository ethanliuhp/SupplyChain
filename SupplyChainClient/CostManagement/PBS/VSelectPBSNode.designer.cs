namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    partial class VSelectPBSNode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSelectPBSNode));
            this.istTree = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtCurrentPath = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvwCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.cbSelectMethod = new System.Windows.Forms.ComboBox();
            this.lblCopyMethod = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblAlert = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblcopyLevel = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCopyLevel = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEnter = new System.Windows.Forms.Button();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtType = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblName = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.panel2);
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Size = new System.Drawing.Size(619, 496);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.panel2, 0);
            // 
            // istTree
            // 
            this.istTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("istTree.ImageStream")));
            this.istTree.TransparentColor = System.Drawing.Color.Transparent;
            this.istTree.Images.SetKeyName(0, "project.ico");
            this.istTree.Images.SetKeyName(1, "picfolder.ico");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtCurrentPath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(619, 22);
            this.panel1.TabIndex = 19;
            // 
            // txtCurrentPath
            // 
            this.txtCurrentPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentPath.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCurrentPath.DrawSelf = false;
            this.txtCurrentPath.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCurrentPath.EnterToTab = false;
            this.txtCurrentPath.Location = new System.Drawing.Point(1, 5);
            this.txtCurrentPath.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCurrentPath.Name = "txtCurrentPath";
            this.txtCurrentPath.Padding = new System.Windows.Forms.Padding(1);
            this.txtCurrentPath.ReadOnly = true;
            this.txtCurrentPath.Size = new System.Drawing.Size(618, 16);
            this.txtCurrentPath.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 22);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(619, 474);
            this.panel2.TabIndex = 20;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvwCategory);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cbSelectMethod);
            this.splitContainer1.Panel2.Controls.Add(this.lblCopyMethod);
            this.splitContainer1.Panel2.Controls.Add(this.lblAlert);
            this.splitContainer1.Panel2.Controls.Add(this.lblLevel);
            this.splitContainer1.Panel2.Controls.Add(this.lblcopyLevel);
            this.splitContainer1.Panel2.Controls.Add(this.txtCopyLevel);
            this.splitContainer1.Panel2.Controls.Add(this.txtDesc);
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btnEnter);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel1);
            this.splitContainer1.Panel2.Controls.Add(this.txtCode);
            this.splitContainer1.Panel2.Controls.Add(this.txtType);
            this.splitContainer1.Panel2.Controls.Add(this.txtName);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel2);
            this.splitContainer1.Panel2.Controls.Add(this.lblName);
            this.splitContainer1.Panel2.Controls.Add(this.customLabel3);
            this.splitContainer1.Size = new System.Drawing.Size(619, 474);
            this.splitContainer1.SplitterDistance = 335;
            this.splitContainer1.TabIndex = 18;
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
            this.tvwCategory.Size = new System.Drawing.Size(335, 474);
            this.tvwCategory.TabIndex = 0;
            // 
            // cbSelectMethod
            // 
            this.cbSelectMethod.DropDownHeight = 200;
            this.cbSelectMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectMethod.FormattingEnabled = true;
            this.cbSelectMethod.IntegralHeight = false;
            this.cbSelectMethod.Location = new System.Drawing.Point(81, 25);
            this.cbSelectMethod.Name = "cbSelectMethod";
            this.cbSelectMethod.Size = new System.Drawing.Size(177, 20);
            this.cbSelectMethod.TabIndex = 23;
            // 
            // lblCopyMethod
            // 
            this.lblCopyMethod.AddColonAuto = true;
            this.lblCopyMethod.AutoSize = true;
            this.lblCopyMethod.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCopyMethod.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCopyMethod.Location = new System.Drawing.Point(16, 30);
            this.lblCopyMethod.Name = "lblCopyMethod";
            this.lblCopyMethod.Size = new System.Drawing.Size(65, 12);
            this.lblCopyMethod.TabIndex = 22;
            this.lblCopyMethod.Text = "选择方式：";
            this.lblCopyMethod.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblAlert
            // 
            this.lblAlert.AutoSize = true;
            this.lblAlert.ForeColor = System.Drawing.Color.Red;
            this.lblAlert.Location = new System.Drawing.Point(153, 260);
            this.lblAlert.Name = "lblAlert";
            this.lblAlert.Size = new System.Drawing.Size(65, 12);
            this.lblAlert.TabIndex = 21;
            this.lblAlert.Text = ",0表示所有";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(136, 260);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(17, 12);
            this.lblLevel.TabIndex = 21;
            this.lblLevel.Text = "级";
            // 
            // lblcopyLevel
            // 
            this.lblcopyLevel.AddColonAuto = true;
            this.lblcopyLevel.AutoSize = true;
            this.lblcopyLevel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblcopyLevel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblcopyLevel.Location = new System.Drawing.Point(16, 260);
            this.lblcopyLevel.Name = "lblcopyLevel";
            this.lblcopyLevel.Size = new System.Drawing.Size(65, 12);
            this.lblcopyLevel.TabIndex = 19;
            this.lblcopyLevel.Text = "拷贝深度：";
            this.lblcopyLevel.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCopyLevel
            // 
            this.txtCopyLevel.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCopyLevel.DrawSelf = false;
            this.txtCopyLevel.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCopyLevel.EnterToTab = false;
            this.txtCopyLevel.Location = new System.Drawing.Point(81, 257);
            this.txtCopyLevel.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCopyLevel.Name = "txtCopyLevel";
            this.txtCopyLevel.Padding = new System.Windows.Forms.Padding(1);
            this.txtCopyLevel.ReadOnly = false;
            this.txtCopyLevel.Size = new System.Drawing.Size(53, 16);
            this.txtCopyLevel.TabIndex = 20;
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.SystemColors.Control;
            this.txtDesc.Location = new System.Drawing.Point(81, 156);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ReadOnly = true;
            this.txtDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDesc.Size = new System.Drawing.Size(177, 84);
            this.txtDesc.TabIndex = 18;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(151, 294);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(25, 294);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(107, 23);
            this.btnEnter.TabIndex = 3;
            this.btnEnter.Text = "确定";
            this.btnEnter.UseVisualStyleBackColor = true;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(16, 64);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 15;
            this.customLabel1.Text = "产品编码：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(81, 61);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(177, 16);
            this.txtCode.TabIndex = 16;
            // 
            // txtType
            // 
            this.txtType.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtType.DrawSelf = false;
            this.txtType.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtType.EnterToTab = false;
            this.txtType.Location = new System.Drawing.Point(81, 121);
            this.txtType.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtType.Name = "txtType";
            this.txtType.Padding = new System.Windows.Forms.Padding(1);
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(177, 16);
            this.txtType.TabIndex = 16;
            // 
            // txtName
            // 
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = false;
            this.txtName.Location = new System.Drawing.Point(81, 90);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(177, 16);
            this.txtName.TabIndex = 16;
            // 
            // customLabel2
            // 
            this.customLabel2.AddColonAuto = true;
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(16, 124);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(65, 12);
            this.customLabel2.TabIndex = 15;
            this.customLabel2.Text = "结构类型：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblName
            // 
            this.lblName.AddColonAuto = true;
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblName.Location = new System.Drawing.Point(16, 93);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(65, 12);
            this.lblName.TabIndex = 15;
            this.lblName.Text = "产品名称：";
            this.lblName.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AddColonAuto = true;
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(16, 159);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(65, 12);
            this.customLabel3.TabIndex = 15;
            this.customLabel3.Text = "产品描述：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VSelectPBSNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(619, 496);
            this.Name = "VSelectPBSNode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择PBS节点";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ImageList istTree;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView tvwCategory;
        private System.Windows.Forms.Label lblLevel;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblcopyLevel;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCopyLevel;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEnter;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private System.Windows.Forms.Panel panel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCurrentPath;
        private System.Windows.Forms.ComboBox cbSelectMethod;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCopyMethod;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtType;
        private System.Windows.Forms.Label lblAlert;
	}
}