namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI
{
    partial class VCostProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCostProject));
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsiSame = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiDown = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnTop = new VirtualMachine.Component.WinControls.Controls.CustomPanel();
            this.pnMain = new System.Windows.Forms.Panel();
            this.sc = new VirtualMachine.Component.WinControls.Controls.CustomSplitContainer();
            this.tvTitle = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.gbBasic = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.txtExpenseItem = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUnit = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tboxDescribe = new System.Windows.Forms.TextBox();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.commonCostProjectType1 = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonCostProjectType();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.commonAccTitle1 = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonAccTitle();
            this.rdBtnminus = new System.Windows.Forms.RadioButton();
            this.rdBtnAdd = new System.Windows.Forms.RadioButton();
            this.commonCostType1 = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonCostType();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.errInfo = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlFloor.SuspendLayout();
            this.cmsMenu.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.sc.Panel1.SuspendLayout();
            this.sc.Panel2.SuspendLayout();
            this.sc.SuspendLayout();
            this.gbBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commonCostProjectType1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commonCostType1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.pnMain);
            this.pnlFloor.Controls.Add(this.pnTop);
            this.pnlFloor.Size = new System.Drawing.Size(718, 481);
            this.pnlFloor.Controls.SetChildIndex(this.pnTop, 0);
            this.pnlFloor.Controls.SetChildIndex(this.pnMain, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(127, 20);
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.Text = "成本项目分类";
            // 
            // cmsMenu
            // 
            this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiSame,
            this.tsiDown,
            this.tsiDelete});
            this.cmsMenu.Name = "cmsMenu";
            this.cmsMenu.Size = new System.Drawing.Size(125, 70);
            // 
            // tsiSame
            // 
            this.tsiSame.Name = "tsiSame";
            this.tsiSame.Size = new System.Drawing.Size(124, 22);
            this.tsiSame.Text = "同级增加";
            // 
            // tsiDown
            // 
            this.tsiDown.Name = "tsiDown";
            this.tsiDown.Size = new System.Drawing.Size(124, 22);
            this.tsiDown.Text = "下级增加";
            // 
            // tsiDelete
            // 
            this.tsiDelete.Name = "tsiDelete";
            this.tsiDelete.Size = new System.Drawing.Size(124, 22);
            this.tsiDelete.Text = "删除";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "sport.ico");
            this.imageList1.Images.SetKeyName(1, "box2_Iblue.ico");
            this.imageList1.Images.SetKeyName(2, "I.ico");
            // 
            // pnTop
            // 
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.ExpandBorderColor = System.Drawing.SystemColors.ControlDark;
            this.pnTop.ExpandBorderStyle = VirtualMachine.Component.WinControls.Controls.ExpandBorder.None;
            this.pnTop.ExpandBorderWidth = 1F;
            this.pnTop.Location = new System.Drawing.Point(0, 0);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(718, 71);
            this.pnTop.TabIndex = 5;
            // 
            // pnMain
            // 
            this.pnMain.Controls.Add(this.sc);
            this.pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMain.Location = new System.Drawing.Point(0, 71);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(718, 410);
            this.pnMain.TabIndex = 6;
            // 
            // sc
            // 
            this.sc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sc.Location = new System.Drawing.Point(0, 0);
            this.sc.Name = "sc";
            // 
            // sc.Panel1
            // 
            this.sc.Panel1.Controls.Add(this.tvTitle);
            // 
            // sc.Panel2
            // 
            this.sc.Panel2.Controls.Add(this.gbBasic);
            this.sc.Size = new System.Drawing.Size(718, 410);
            this.sc.SplitterDistance = 233;
            this.sc.TabIndex = 0;
            // 
            // tvTitle
            // 
            this.tvTitle.ContextMenuStrip = this.cmsMenu;
            this.tvTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTitle.HideSelection = false;
            this.tvTitle.ImageIndex = 2;
            this.tvTitle.ImageList = this.imageList1;
            this.tvTitle.ItemHeight = 20;
            this.tvTitle.Location = new System.Drawing.Point(0, 0);
            this.tvTitle.Name = "tvTitle";
            this.tvTitle.SelectedImageIndex = 0;
            this.tvTitle.Size = new System.Drawing.Size(233, 410);
            this.tvTitle.TabIndex = 0;
            // 
            // gbBasic
            // 
            this.gbBasic.Controls.Add(this.txtExpenseItem);
            this.gbBasic.Controls.Add(this.label2);
            this.gbBasic.Controls.Add(this.txtUnit);
            this.gbBasic.Controls.Add(this.customLabel8);
            this.gbBasic.Controls.Add(this.tboxDescribe);
            this.gbBasic.Controls.Add(this.customLabel5);
            this.gbBasic.Controls.Add(this.commonCostProjectType1);
            this.gbBasic.Controls.Add(this.customLabel4);
            this.gbBasic.Controls.Add(this.commonAccTitle1);
            this.gbBasic.Controls.Add(this.rdBtnminus);
            this.gbBasic.Controls.Add(this.rdBtnAdd);
            this.gbBasic.Controls.Add(this.commonCostType1);
            this.gbBasic.Controls.Add(this.customLabel7);
            this.gbBasic.Controls.Add(this.customLabel6);
            this.gbBasic.Controls.Add(this.customLabel3);
            this.gbBasic.Controls.Add(this.txtName);
            this.gbBasic.Controls.Add(this.customLabel2);
            this.gbBasic.Controls.Add(this.txtCode);
            this.gbBasic.Controls.Add(this.customLabel1);
            this.gbBasic.Location = new System.Drawing.Point(23, 18);
            this.gbBasic.Name = "gbBasic";
            this.gbBasic.Size = new System.Drawing.Size(308, 360);
            this.gbBasic.TabIndex = 4;
            this.gbBasic.TabStop = false;
            this.gbBasic.Text = "基础属性";
            // 
            // txtExpenseItem
            // 
            this.txtExpenseItem.FormattingEnabled = true;
            this.txtExpenseItem.Location = new System.Drawing.Point(103, 288);
            this.txtExpenseItem.Name = "txtExpenseItem";
            this.txtExpenseItem.Size = new System.Drawing.Size(143, 20);
            this.txtExpenseItem.TabIndex = 69;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 294);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 68;
            this.label2.Text = "项目费用";
            // 
            // txtUnit
            // 
            this.txtUnit.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtUnit.DrawSelf = false;
            this.txtUnit.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtUnit.EnterToTab = true;
            this.txtUnit.Location = new System.Drawing.Point(102, 105);
            this.txtUnit.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Padding = new System.Windows.Forms.Padding(1);
            this.txtUnit.ReadOnly = false;
            this.txtUnit.Size = new System.Drawing.Size(144, 16);
            this.txtUnit.TabIndex = 29;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(34, 105);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(53, 12);
            this.customLabel8.TabIndex = 28;
            this.customLabel8.Text = "计量单位";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tboxDescribe
            // 
            this.tboxDescribe.Location = new System.Drawing.Point(102, 321);
            this.tboxDescribe.Name = "tboxDescribe";
            this.tboxDescribe.Size = new System.Drawing.Size(144, 21);
            this.tboxDescribe.TabIndex = 27;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(34, 326);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(53, 12);
            this.customLabel5.TabIndex = 26;
            this.customLabel5.Text = "项目备注";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // commonCostProjectType1
            // 
            this.commonCostProjectType1.AddItemSeparator = ';';
            this.commonCostProjectType1.AutoCompletion = true;
            this.commonCostProjectType1.AutoDropDown = true;
            this.commonCostProjectType1.Caption = "";
            this.commonCostProjectType1.CaptionHeight = 17;
            this.commonCostProjectType1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.commonCostProjectType1.ColumnCaptionHeight = 18;
            this.commonCostProjectType1.ColumnFooterHeight = 18;
            this.commonCostProjectType1.ColumnHeaders = false;
            this.commonCostProjectType1.ContentHeight = 16;
            this.commonCostProjectType1.DataMode = C1.Win.C1List.DataModeEnum.AddItem;
            this.commonCostProjectType1.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.commonCostProjectType1.EditorBackColor = System.Drawing.SystemColors.Window;
            this.commonCostProjectType1.EditorFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.commonCostProjectType1.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.commonCostProjectType1.EditorHeight = 16;
            this.commonCostProjectType1.FlatStyle = C1.Win.C1List.FlatModeEnum.Standard;
            this.commonCostProjectType1.Images.Add(((System.Drawing.Image)(resources.GetObject("commonCostProjectType1.Images"))));
            this.commonCostProjectType1.ItemHeight = 15;
            this.commonCostProjectType1.Location = new System.Drawing.Point(102, 183);
            this.commonCostProjectType1.MatchEntryTimeout = ((long)(2000));
            this.commonCostProjectType1.MaxDropDownItems = ((short)(5));
            this.commonCostProjectType1.MaxLength = 32767;
            this.commonCostProjectType1.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.commonCostProjectType1.Name = "commonCostProjectType1";
            this.commonCostProjectType1.Result = ((System.Collections.IList)(resources.GetObject("commonCostProjectType1.Result")));
            this.commonCostProjectType1.RowDivider.Color = System.Drawing.Color.DarkGray;
            this.commonCostProjectType1.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.commonCostProjectType1.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.commonCostProjectType1.Size = new System.Drawing.Size(144, 22);
            this.commonCostProjectType1.TabIndex = 25;
            this.commonCostProjectType1.PropBag = resources.GetString("commonCostProjectType1.PropBag");
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(34, 259);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(53, 12);
            this.customLabel4.TabIndex = 24;
            this.customLabel4.Text = "对应科目";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // commonAccTitle1
            // 
            this.commonAccTitle1.BackColor = System.Drawing.SystemColors.Control;
            this.commonAccTitle1.Code = null;
            this.commonAccTitle1.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.commonAccTitle1.EnterToTab = false;
            this.commonAccTitle1.Id = "";
            this.commonAccTitle1.IsAllowMulti = false;
            this.commonAccTitle1.Location = new System.Drawing.Point(102, 259);
            this.commonAccTitle1.Name = "commonAccTitle1";
            this.commonAccTitle1.OnlyFirstLevTitle = false;
            this.commonAccTitle1.OnlyLeafTitle = true;
            this.commonAccTitle1.Padding = new System.Windows.Forms.Padding(1);
            this.commonAccTitle1.QueryAssisInfo = false;
            this.commonAccTitle1.ReadOnly = false;
            this.commonAccTitle1.Result = ((System.Collections.IList)(resources.GetObject("commonAccTitle1.Result")));
            this.commonAccTitle1.RightMouse = false;
            this.commonAccTitle1.SelectedAccTitle = null;
            this.commonAccTitle1.Size = new System.Drawing.Size(144, 16);
            this.commonAccTitle1.TabIndex = 23;
            // 
            // rdBtnminus
            // 
            this.rdBtnminus.AutoSize = true;
            this.rdBtnminus.Location = new System.Drawing.Point(159, 223);
            this.rdBtnminus.Name = "rdBtnminus";
            this.rdBtnminus.Size = new System.Drawing.Size(35, 16);
            this.rdBtnminus.TabIndex = 22;
            this.rdBtnminus.TabStop = true;
            this.rdBtnminus.Text = "减";
            this.rdBtnminus.UseVisualStyleBackColor = true;
            // 
            // rdBtnAdd
            // 
            this.rdBtnAdd.AutoSize = true;
            this.rdBtnAdd.Location = new System.Drawing.Point(118, 223);
            this.rdBtnAdd.Name = "rdBtnAdd";
            this.rdBtnAdd.Size = new System.Drawing.Size(35, 16);
            this.rdBtnAdd.TabIndex = 21;
            this.rdBtnAdd.TabStop = true;
            this.rdBtnAdd.Text = "加";
            this.rdBtnAdd.UseVisualStyleBackColor = true;
            // 
            // commonCostType1
            // 
            this.commonCostType1.AddItemSeparator = ';';
            this.commonCostType1.AutoCompletion = true;
            this.commonCostType1.AutoDropDown = true;
            this.commonCostType1.Caption = "";
            this.commonCostType1.CaptionHeight = 17;
            this.commonCostType1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.commonCostType1.ColumnCaptionHeight = 18;
            this.commonCostType1.ColumnFooterHeight = 18;
            this.commonCostType1.ColumnHeaders = false;
            this.commonCostType1.ContentHeight = 16;
            this.commonCostType1.DataMode = C1.Win.C1List.DataModeEnum.AddItem;
            this.commonCostType1.DeadAreaBackColor = System.Drawing.Color.Empty;
            this.commonCostType1.EditorBackColor = System.Drawing.SystemColors.Window;
            this.commonCostType1.EditorFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.commonCostType1.EditorForeColor = System.Drawing.SystemColors.WindowText;
            this.commonCostType1.EditorHeight = 16;
            this.commonCostType1.FlatStyle = C1.Win.C1List.FlatModeEnum.Standard;
            this.commonCostType1.Images.Add(((System.Drawing.Image)(resources.GetObject("commonCostType1.Images"))));
            this.commonCostType1.ItemHeight = 15;
            this.commonCostType1.Location = new System.Drawing.Point(102, 143);
            this.commonCostType1.MatchEntryTimeout = ((long)(2000));
            this.commonCostType1.MaxDropDownItems = ((short)(5));
            this.commonCostType1.MaxLength = 32767;
            this.commonCostType1.MouseCursor = System.Windows.Forms.Cursors.Default;
            this.commonCostType1.Name = "commonCostType1";
            this.commonCostType1.Result = ((System.Collections.IList)(resources.GetObject("commonCostType1.Result")));
            this.commonCostType1.RowDivider.Color = System.Drawing.Color.DarkGray;
            this.commonCostType1.RowDivider.Style = C1.Win.C1List.LineStyleEnum.None;
            this.commonCostType1.RowSubDividerColor = System.Drawing.Color.DarkGray;
            this.commonCostType1.Size = new System.Drawing.Size(144, 22);
            this.commonCostType1.TabIndex = 20;
            this.commonCostType1.PropBag = resources.GetString("commonCostType1.PropBag");
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(34, 223);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(53, 12);
            this.customLabel7.TabIndex = 12;
            this.customLabel7.Text = "汇集方式";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(34, 187);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(53, 12);
            this.customLabel6.TabIndex = 10;
            this.customLabel6.Text = "项目属性";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(34, 147);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(53, 12);
            this.customLabel3.TabIndex = 4;
            this.customLabel3.Text = "项目分类";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtName
            // 
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = true;
            this.txtName.Location = new System.Drawing.Point(102, 66);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = false;
            this.txtName.Size = new System.Drawing.Size(144, 16);
            this.txtName.TabIndex = 3;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(34, 66);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(53, 12);
            this.customLabel2.TabIndex = 2;
            this.customLabel2.Text = "项目名称";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = true;
            this.txtCode.Location = new System.Drawing.Point(102, 31);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = false;
            this.txtCode.Size = new System.Drawing.Size(144, 16);
            this.txtCode.TabIndex = 1;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(34, 31);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(53, 12);
            this.customLabel1.TabIndex = 0;
            this.customLabel1.Text = "项目编号";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // errInfo
            // 
            this.errInfo.ContainerControl = this;
            // 
            // VCostProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 481);
            this.Name = "VCostProject";
            this.Text = "成本项目分类表";
            this.Load += new System.EventHandler(this.VAccountTitle_Load);
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.cmsMenu.ResumeLayout(false);
            this.pnMain.ResumeLayout(false);
            this.sc.Panel1.ResumeLayout(false);
            this.sc.Panel2.ResumeLayout(false);
            this.sc.ResumeLayout(false);
            this.gbBasic.ResumeLayout(false);
            this.gbBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commonCostProjectType1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commonCostType1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip cmsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsiSame;
        private System.Windows.Forms.ToolStripMenuItem tsiDown;
        private System.Windows.Forms.ToolStripMenuItem tsiDelete;
        private VirtualMachine.Component.WinControls.Controls.CustomPanel pnTop;
        private System.Windows.Forms.Panel pnMain;
        private VirtualMachine.Component.WinControls.Controls.CustomSplitContainer sc;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView tvTitle;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gbBasic;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.ErrorProvider errInfo;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonCostType commonCostType1;
        private System.Windows.Forms.RadioButton rdBtnminus;
        private System.Windows.Forms.RadioButton rdBtnAdd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonAccTitle commonAccTitle1;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonCostProjectType commonCostProjectType1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private System.Windows.Forms.TextBox tboxDescribe;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtUnit;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private System.Windows.Forms.ComboBox txtExpenseItem;
        private System.Windows.Forms.Label label2;
    }
}