namespace Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng
{
    partial class VOperationOrgAsProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VOperationOrgAsProject));
            this.cmsTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.istTree = new System.Windows.Forms.ImageList(this.components);
            this.gbBasic = new System.Windows.Forms.GroupBox();
            this.txtPlace = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cbIsAccountOrg = new System.Windows.Forms.ComboBox();
            this.txtCurrentPath = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblCurrentPath = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblName = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvwCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.btnSaveProject = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbIfInProject = new System.Windows.Forms.ComboBox();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProjectType = new System.Windows.Forms.ComboBox();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cboProState = new System.Windows.Forms.ComboBox();
            this.txtCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel41 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel34 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtConstractStage = new System.Windows.Forms.ComboBox();
            this.txtProjectCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProjectName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.pnlFloor.SuspendLayout();
            this.gbBasic.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Size = new System.Drawing.Size(893, 469);
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
            this.istTree.Images.SetKeyName(0, "picfolder.ico");
            // 
            // gbBasic
            // 
            this.gbBasic.Controls.Add(this.txtPlace);
            this.gbBasic.Controls.Add(this.customLabel3);
            this.gbBasic.Controls.Add(this.cbIsAccountOrg);
            this.gbBasic.Controls.Add(this.txtCurrentPath);
            this.gbBasic.Controls.Add(this.lblCurrentPath);
            this.gbBasic.Controls.Add(this.customLabel2);
            this.gbBasic.Controls.Add(this.customLabel1);
            this.gbBasic.Controls.Add(this.txtCode);
            this.gbBasic.Controls.Add(this.lblName);
            this.gbBasic.Controls.Add(this.txtName);
            this.gbBasic.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbBasic.Location = new System.Drawing.Point(0, 0);
            this.gbBasic.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this.gbBasic.Name = "gbBasic";
            this.gbBasic.Size = new System.Drawing.Size(673, 110);
            this.gbBasic.TabIndex = 2;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "业务组织信息";
            // 
            // txtPlace
            // 
            this.txtPlace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPlace.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPlace.DrawSelf = false;
            this.txtPlace.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPlace.EnterToTab = false;
            this.txtPlace.Location = new System.Drawing.Point(255, 76);
            this.txtPlace.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPlace.Name = "txtPlace";
            this.txtPlace.Padding = new System.Windows.Forms.Padding(1);
            this.txtPlace.ReadOnly = true;
            this.txtPlace.Size = new System.Drawing.Size(397, 16);
            this.txtPlace.TabIndex = 22;
            // 
            // customLabel3
            // 
            this.customLabel3.AddColonAuto = true;
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(184, 76);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(65, 12);
            this.customLabel3.TabIndex = 21;
            this.customLabel3.Text = "地    址：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cbIsAccountOrg
            // 
            this.cbIsAccountOrg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIsAccountOrg.FormattingEnabled = true;
            this.cbIsAccountOrg.Location = new System.Drawing.Point(103, 71);
            this.cbIsAccountOrg.Name = "cbIsAccountOrg";
            this.cbIsAccountOrg.Size = new System.Drawing.Size(75, 20);
            this.cbIsAccountOrg.TabIndex = 20;
            // 
            // txtCurrentPath
            // 
            this.txtCurrentPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentPath.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCurrentPath.DrawSelf = false;
            this.txtCurrentPath.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCurrentPath.EnterToTab = false;
            this.txtCurrentPath.Location = new System.Drawing.Point(71, 19);
            this.txtCurrentPath.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCurrentPath.Name = "txtCurrentPath";
            this.txtCurrentPath.Padding = new System.Windows.Forms.Padding(1);
            this.txtCurrentPath.ReadOnly = true;
            this.txtCurrentPath.Size = new System.Drawing.Size(581, 18);
            this.txtCurrentPath.TabIndex = 14;
            // 
            // lblCurrentPath
            // 
            this.lblCurrentPath.AddColonAuto = true;
            this.lblCurrentPath.AutoSize = true;
            this.lblCurrentPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentPath.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCurrentPath.Location = new System.Drawing.Point(6, 22);
            this.lblCurrentPath.Name = "lblCurrentPath";
            this.lblCurrentPath.Size = new System.Drawing.Size(65, 12);
            this.lblCurrentPath.TabIndex = 13;
            this.lblCurrentPath.Text = "当前路径：";
            this.lblCurrentPath.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel2
            // 
            this.customLabel2.AddColonAuto = true;
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(6, 76);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(101, 12);
            this.customLabel2.TabIndex = 15;
            this.customLabel2.Text = "是否是核算组织：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(6, 50);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 15;
            this.customLabel1.Text = "编    码：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.Enabled = false;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(71, 47);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(107, 16);
            this.txtCode.TabIndex = 16;
            // 
            // lblName
            // 
            this.lblName.AddColonAuto = true;
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblName.Location = new System.Drawing.Point(184, 49);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(65, 12);
            this.lblName.TabIndex = 15;
            this.lblName.Text = "名    称：";
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
            this.txtName.Location = new System.Drawing.Point(255, 46);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(397, 16);
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
            this.splitContainer1.Panel2.Controls.Add(this.btnSaveProject);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.gbBasic);
            this.splitContainer1.Size = new System.Drawing.Size(893, 444);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 17;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // tvwCategory
            // 
            this.tvwCategory.AllowDrop = true;
            this.tvwCategory.BackColor = System.Drawing.SystemColors.Window;
            this.tvwCategory.ContextMenuStrip = this.cmsTree;
            this.tvwCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwCategory.HideSelection = false;
            this.tvwCategory.HotTracking = true;
            this.tvwCategory.ImageIndex = 0;
            this.tvwCategory.ImageList = this.istTree;
            this.tvwCategory.Location = new System.Drawing.Point(0, 0);
            this.tvwCategory.Name = "tvwCategory";
            this.tvwCategory.SelectedImageIndex = 0;
            this.tvwCategory.Size = new System.Drawing.Size(216, 444);
            this.tvwCategory.TabIndex = 0;
            this.tvwCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwCategory_AfterSelect);
            // 
            // btnSaveProject
            // 
            this.btnSaveProject.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSaveProject.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveProject.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSaveProject.Location = new System.Drawing.Point(255, 407);
            this.btnSaveProject.Name = "btnSaveProject";
            this.btnSaveProject.Size = new System.Drawing.Size(90, 34);
            this.btnSaveProject.TabIndex = 137;
            this.btnSaveProject.Text = "保存项目信息";
            this.btnSaveProject.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.customLabel8);
            this.groupBox1.Controls.Add(this.cmbIfInProject);
            this.groupBox1.Controls.Add(this.customLabel7);
            this.groupBox1.Controls.Add(this.txtProjectType);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Controls.Add(this.cboProState);
            this.groupBox1.Controls.Add(this.txtCreateDate);
            this.groupBox1.Controls.Add(this.customLabel41);
            this.groupBox1.Controls.Add(this.customLabel34);
            this.groupBox1.Controls.Add(this.txtConstractStage);
            this.groupBox1.Controls.Add(this.txtProjectCode);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.txtProjectName);
            this.groupBox1.Controls.Add(this.lblSupplyContract);
            this.groupBox1.Controls.Add(this.customLabel5);
            this.groupBox1.Controls.Add(this.txtProName);
            this.groupBox1.Location = new System.Drawing.Point(5, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(664, 285);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "项目相关信息";
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(302, 121);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(83, 12);
            this.customLabel8.TabIndex = 348;
            this.customLabel8.Text = "是否内部项目:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbIfInProject
            // 
            this.cmbIfInProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIfInProject.FormattingEnabled = true;
            this.cmbIfInProject.Location = new System.Drawing.Point(389, 118);
            this.cmbIfInProject.Name = "cmbIfInProject";
            this.cmbIfInProject.Size = new System.Drawing.Size(121, 20);
            this.cmbIfInProject.TabIndex = 347;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(26, 91);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 346;
            this.customLabel7.Text = "项目类型:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProjectType
            // 
            this.txtProjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtProjectType.FormattingEnabled = true;
            this.txtProjectType.Location = new System.Drawing.Point(87, 85);
            this.txtProjectType.Name = "txtProjectType";
            this.txtProjectType.Size = new System.Drawing.Size(121, 20);
            this.txtProjectType.TabIndex = 345;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(325, 89);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 344;
            this.customLabel6.Text = "商务状态:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            this.customLabel6.Visible = false;
            // 
            // cboProState
            // 
            this.cboProState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProState.FormattingEnabled = true;
            this.cboProState.Location = new System.Drawing.Point(389, 86);
            this.cboProState.Name = "cboProState";
            this.cboProState.Size = new System.Drawing.Size(121, 20);
            this.cboProState.TabIndex = 343;
            this.cboProState.Visible = false;
            // 
            // txtCreateDate
            // 
            this.txtCreateDate.Location = new System.Drawing.Point(87, 119);
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Size = new System.Drawing.Size(121, 21);
            this.txtCreateDate.TabIndex = 299;
            // 
            // customLabel41
            // 
            this.customLabel41.AutoSize = true;
            this.customLabel41.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel41.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel41.Location = new System.Drawing.Point(23, 126);
            this.customLabel41.Name = "customLabel41";
            this.customLabel41.Size = new System.Drawing.Size(59, 12);
            this.customLabel41.TabIndex = 298;
            this.customLabel41.Text = "开工日期:";
            this.customLabel41.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel34
            // 
            this.customLabel34.AutoSize = true;
            this.customLabel34.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel34.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel34.Location = new System.Drawing.Point(325, 57);
            this.customLabel34.Name = "customLabel34";
            this.customLabel34.Size = new System.Drawing.Size(59, 12);
            this.customLabel34.TabIndex = 342;
            this.customLabel34.Text = "施工阶段:";
            this.customLabel34.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtConstractStage
            // 
            this.txtConstractStage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtConstractStage.FormattingEnabled = true;
            this.txtConstractStage.Location = new System.Drawing.Point(389, 53);
            this.txtConstractStage.Name = "txtConstractStage";
            this.txtConstractStage.Size = new System.Drawing.Size(121, 20);
            this.txtConstractStage.TabIndex = 341;
            // 
            // txtProjectCode
            // 
            this.txtProjectCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtProjectCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProjectCode.DrawSelf = false;
            this.txtProjectCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProjectCode.EnterToTab = false;
            this.txtProjectCode.Location = new System.Drawing.Point(389, 24);
            this.txtProjectCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProjectCode.Name = "txtProjectCode";
            this.txtProjectCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtProjectCode.ReadOnly = true;
            this.txtProjectCode.Size = new System.Drawing.Size(121, 16);
            this.txtProjectCode.TabIndex = 324;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(26, 26);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 321;
            this.customLabel4.Text = "工程名称:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProjectName
            // 
            this.txtProjectName.BackColor = System.Drawing.SystemColors.Control;
            this.txtProjectName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProjectName.DrawSelf = false;
            this.txtProjectName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProjectName.EnterToTab = false;
            this.txtProjectName.Location = new System.Drawing.Point(90, 24);
            this.txtProjectName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Padding = new System.Windows.Forms.Padding(1);
            this.txtProjectName.ReadOnly = false;
            this.txtProjectName.Size = new System.Drawing.Size(221, 16);
            this.txtProjectName.TabIndex = 322;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(325, 26);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(59, 12);
            this.lblSupplyContract.TabIndex = 323;
            this.lblSupplyContract.Text = "项目编号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(26, 57);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(59, 12);
            this.customLabel5.TabIndex = 325;
            this.customLabel5.Text = "项目名称:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProName
            // 
            this.txtProName.BackColor = System.Drawing.SystemColors.Control;
            this.txtProName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProName.DrawSelf = false;
            this.txtProName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProName.EnterToTab = false;
            this.txtProName.Location = new System.Drawing.Point(90, 53);
            this.txtProName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProName.Name = "txtProName";
            this.txtProName.Padding = new System.Windows.Forms.Padding(1);
            this.txtProName.ReadOnly = false;
            this.txtProName.Size = new System.Drawing.Size(221, 16);
            this.txtProName.TabIndex = 326;
            // 
            // VOperationOrgAsProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(893, 469);
            this.Name = "VOperationOrgAsProject";
            this.Text = "组织关联工程项目信息";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.gbBasic.ResumeLayout(false);
            this.gbBasic.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ContextMenuStrip cmsTree;
        private System.Windows.Forms.ImageList istTree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView tvwCategory;
        private System.Windows.Forms.GroupBox gbBasic;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCurrentPath;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCurrentPath;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private System.Windows.Forms.ComboBox cbIsAccountOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPlace;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSaveProject;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProjectCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProjectName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel34;
        private System.Windows.Forms.ComboBox txtConstractStage;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel41;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker txtCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private System.Windows.Forms.ComboBox cboProState;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private System.Windows.Forms.ComboBox txtProjectType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private System.Windows.Forms.ComboBox cmbIfInProject;
	}
}