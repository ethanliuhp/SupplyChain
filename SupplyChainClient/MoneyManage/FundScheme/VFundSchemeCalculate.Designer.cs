namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    partial class VFundSchemeCalculate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VFundSchemeCalculate));
            this.ucProjectSelector1 = new Application.Business.Erp.SupplyChain.Client.MoneyManage.UcProjectSelector();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbFundScheme = new System.Windows.Forms.ComboBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tbContent = new System.Windows.Forms.TabControl();
            this.tPageAmount = new System.Windows.Forms.TabPage();
            this.rptGridAmount = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsMenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsMenuCancelEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tPageCostTax = new System.Windows.Forms.TabPage();
            this.rptGridTax = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tPageTaxRate = new System.Windows.Forms.TabPage();
            this.rptGridIndRate = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tPageFee = new System.Windows.Forms.TabPage();
            this.rptGridFee = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tPageGether = new System.Windows.Forms.TabPage();
            this.rptGridGether = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tPagePayment = new System.Windows.Forms.TabPage();
            this.rptGridPayment = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tPageSummary = new System.Windows.Forms.TabPage();
            this.rptGridSummary = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tPageContrast = new System.Windows.Forms.TabPage();
            this.rptGridContrast = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tPageBalance = new System.Windows.Forms.TabPage();
            this.rptGridBalance = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tPageMaster = new System.Windows.Forms.TabPage();
            this.rptGridMaster = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnUnDo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbInfo = new System.Windows.Forms.TextBox();
            this.chkIsOnlyOne = new System.Windows.Forms.CheckBox();
            this.btnCompute = new System.Windows.Forms.Button();
            this.chkAutoCompute = new System.Windows.Forms.CheckBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            this.tbContent.SuspendLayout();
            this.tPageAmount.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tPageCostTax.SuspendLayout();
            this.tPageTaxRate.SuspendLayout();
            this.tPageFee.SuspendLayout();
            this.tPageGether.SuspendLayout();
            this.tPagePayment.SuspendLayout();
            this.tPageSummary.SuspendLayout();
            this.tPageContrast.SuspendLayout();
            this.tPageBalance.SuspendLayout();
            this.tPageMaster.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnSubmit);
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Controls.Add(this.btnUnDo);
            this.pnlFloor.Controls.Add(this.btnExport);
            this.pnlFloor.Controls.Add(this.tbContent);
            this.pnlFloor.Controls.Add(this.btnDelete);
            this.pnlFloor.Controls.Add(this.btnSave);
            this.pnlFloor.Controls.Add(this.btnCreate);
            this.pnlFloor.Controls.Add(this.cmbFundScheme);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.ucProjectSelector1);
            this.pnlFloor.Size = new System.Drawing.Size(1260, 582);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.ucProjectSelector1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cmbFundScheme, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCreate, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSave, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnDelete, 0);
            this.pnlFloor.Controls.SetChildIndex(this.tbContent, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExport, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnUnDo, 0);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSubmit, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 9);
            // 
            // ucProjectSelector1
            // 
            this.ucProjectSelector1.AutoSize = true;
            this.ucProjectSelector1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ucProjectSelector1.Location = new System.Drawing.Point(12, 12);
            this.ucProjectSelector1.Name = "ucProjectSelector1";
            this.ucProjectSelector1.SelectedProject = null;
            this.ucProjectSelector1.Size = new System.Drawing.Size(312, 26);
            this.ucProjectSelector1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(341, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "资金策划表:";
            // 
            // cmbFundScheme
            // 
            this.cmbFundScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFundScheme.FormattingEnabled = true;
            this.cmbFundScheme.Location = new System.Drawing.Point(414, 15);
            this.cmbFundScheme.Name = "cmbFundScheme";
            this.cmbFundScheme.Size = new System.Drawing.Size(180, 20);
            this.cmbFundScheme.TabIndex = 3;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(613, 14);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "重新生成";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(706, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "保存修改";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(892, 14);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "删除数据";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // tbContent
            // 
            this.tbContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbContent.Controls.Add(this.tPageAmount);
            this.tbContent.Controls.Add(this.tPageCostTax);
            this.tbContent.Controls.Add(this.tPageTaxRate);
            this.tbContent.Controls.Add(this.tPageFee);
            this.tbContent.Controls.Add(this.tPageGether);
            this.tbContent.Controls.Add(this.tPagePayment);
            this.tbContent.Controls.Add(this.tPageSummary);
            this.tbContent.Controls.Add(this.tPageContrast);
            this.tbContent.Controls.Add(this.tPageBalance);
            this.tbContent.Controls.Add(this.tPageMaster);
            this.tbContent.Location = new System.Drawing.Point(3, 54);
            this.tbContent.Name = "tbContent";
            this.tbContent.SelectedIndex = 0;
            this.tbContent.Size = new System.Drawing.Size(1254, 472);
            this.tbContent.TabIndex = 7;
            // 
            // tPageAmount
            // 
            this.tPageAmount.Controls.Add(this.rptGridAmount);
            this.tPageAmount.Location = new System.Drawing.Point(4, 22);
            this.tPageAmount.Name = "tPageAmount";
            this.tPageAmount.Padding = new System.Windows.Forms.Padding(3);
            this.tPageAmount.Size = new System.Drawing.Size(1246, 446);
            this.tPageAmount.TabIndex = 0;
            this.tPageAmount.Tag = "项目进度报量及成本测算表";
            this.tPageAmount.Text = "表5项目进度报量及成本测算表";
            this.tPageAmount.UseVisualStyleBackColor = true;
            // 
            // rptGridAmount
            // 
            this.rptGridAmount.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridAmount.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridAmount.CheckedImage")));
            this.rptGridAmount.ContextMenuStrip = this.contextMenuStrip1;
            this.rptGridAmount.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridAmount.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridAmount.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridAmount.Location = new System.Drawing.Point(3, 3);
            this.rptGridAmount.Name = "rptGridAmount";
            this.rptGridAmount.Size = new System.Drawing.Size(1240, 440);
            this.rptGridAmount.TabIndex = 11;
            this.rptGridAmount.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridAmount.UncheckedImage")));
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsMenuEdit,
            this.toolStripSeparator1,
            this.tsMenuCancelEdit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 54);
            // 
            // tsMenuEdit
            // 
            this.tsMenuEdit.Name = "tsMenuEdit";
            this.tsMenuEdit.Size = new System.Drawing.Size(124, 22);
            this.tsMenuEdit.Text = "编辑数据";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // tsMenuCancelEdit
            // 
            this.tsMenuCancelEdit.Name = "tsMenuCancelEdit";
            this.tsMenuCancelEdit.Size = new System.Drawing.Size(124, 22);
            this.tsMenuCancelEdit.Text = "结束编辑";
            // 
            // tPageCostTax
            // 
            this.tPageCostTax.Controls.Add(this.rptGridTax);
            this.tPageCostTax.Location = new System.Drawing.Point(4, 22);
            this.tPageCostTax.Name = "tPageCostTax";
            this.tPageCostTax.Padding = new System.Windows.Forms.Padding(3);
            this.tPageCostTax.Size = new System.Drawing.Size(375, 147);
            this.tPageCostTax.TabIndex = 1;
            this.tPageCostTax.Tag = "项目成本价税测算表";
            this.tPageCostTax.Text = "表6项目成本价税测算表";
            this.tPageCostTax.UseVisualStyleBackColor = true;
            // 
            // rptGridTax
            // 
            this.rptGridTax.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridTax.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridTax.CheckedImage")));
            this.rptGridTax.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridTax.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridTax.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridTax.Location = new System.Drawing.Point(3, 3);
            this.rptGridTax.Name = "rptGridTax";
            this.rptGridTax.Size = new System.Drawing.Size(369, 141);
            this.rptGridTax.TabIndex = 11;
            this.rptGridTax.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridTax.UncheckedImage")));
            // 
            // tPageTaxRate
            // 
            this.tPageTaxRate.Controls.Add(this.rptGridIndRate);
            this.tPageTaxRate.Location = new System.Drawing.Point(4, 22);
            this.tPageTaxRate.Name = "tPageTaxRate";
            this.tPageTaxRate.Size = new System.Drawing.Size(375, 147);
            this.tPageTaxRate.TabIndex = 8;
            this.tPageTaxRate.Tag = "间接费用及进项税测算表";
            this.tPageTaxRate.Text = "表7间接费用及进项税测算表";
            this.tPageTaxRate.UseVisualStyleBackColor = true;
            // 
            // rptGridIndRate
            // 
            this.rptGridIndRate.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridIndRate.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridIndRate.CheckedImage")));
            this.rptGridIndRate.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridIndRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridIndRate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridIndRate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridIndRate.Location = new System.Drawing.Point(0, 0);
            this.rptGridIndRate.Name = "rptGridIndRate";
            this.rptGridIndRate.Size = new System.Drawing.Size(375, 147);
            this.rptGridIndRate.TabIndex = 13;
            this.rptGridIndRate.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridIndRate.UncheckedImage")));
            // 
            // tPageFee
            // 
            this.tPageFee.Controls.Add(this.rptGridFee);
            this.tPageFee.Location = new System.Drawing.Point(4, 22);
            this.tPageFee.Name = "tPageFee";
            this.tPageFee.Size = new System.Drawing.Size(375, 147);
            this.tPageFee.TabIndex = 9;
            this.tPageFee.Tag = "项目财务费用测算表";
            this.tPageFee.Text = "表8项目财务费用测算表";
            this.tPageFee.UseVisualStyleBackColor = true;
            // 
            // rptGridFee
            // 
            this.rptGridFee.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridFee.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridFee.CheckedImage")));
            this.rptGridFee.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridFee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridFee.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridFee.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridFee.Location = new System.Drawing.Point(0, 0);
            this.rptGridFee.Name = "rptGridFee";
            this.rptGridFee.Size = new System.Drawing.Size(375, 147);
            this.rptGridFee.TabIndex = 13;
            this.rptGridFee.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridFee.UncheckedImage")));
            // 
            // tPageGether
            // 
            this.tPageGether.Controls.Add(this.rptGridGether);
            this.tPageGether.Location = new System.Drawing.Point(4, 22);
            this.tPageGether.Name = "tPageGether";
            this.tPageGether.Size = new System.Drawing.Size(375, 147);
            this.tPageGether.TabIndex = 2;
            this.tPageGether.Tag = "项目资金收款测算表";
            this.tPageGether.Text = "表9项目资金收款测算表";
            this.tPageGether.UseVisualStyleBackColor = true;
            // 
            // rptGridGether
            // 
            this.rptGridGether.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridGether.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridGether.CheckedImage")));
            this.rptGridGether.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridGether.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridGether.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridGether.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridGether.Location = new System.Drawing.Point(0, 0);
            this.rptGridGether.Name = "rptGridGether";
            this.rptGridGether.Size = new System.Drawing.Size(375, 147);
            this.rptGridGether.TabIndex = 11;
            this.rptGridGether.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridGether.UncheckedImage")));
            // 
            // tPagePayment
            // 
            this.tPagePayment.Controls.Add(this.rptGridPayment);
            this.tPagePayment.Location = new System.Drawing.Point(4, 22);
            this.tPagePayment.Name = "tPagePayment";
            this.tPagePayment.Size = new System.Drawing.Size(375, 147);
            this.tPagePayment.TabIndex = 3;
            this.tPagePayment.Tag = "项目资金支付款测算表";
            this.tPagePayment.Text = "表10项目资金支付款测算表";
            this.tPagePayment.UseVisualStyleBackColor = true;
            // 
            // rptGridPayment
            // 
            this.rptGridPayment.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridPayment.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridPayment.CheckedImage")));
            this.rptGridPayment.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridPayment.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridPayment.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridPayment.Location = new System.Drawing.Point(0, 0);
            this.rptGridPayment.Name = "rptGridPayment";
            this.rptGridPayment.Size = new System.Drawing.Size(375, 147);
            this.rptGridPayment.TabIndex = 11;
            this.rptGridPayment.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridPayment.UncheckedImage")));
            // 
            // tPageSummary
            // 
            this.tPageSummary.Controls.Add(this.rptGridSummary);
            this.tPageSummary.Location = new System.Drawing.Point(4, 22);
            this.tPageSummary.Name = "tPageSummary";
            this.tPageSummary.Size = new System.Drawing.Size(375, 147);
            this.tPageSummary.TabIndex = 5;
            this.tPageSummary.Tag = "项目资金策划表";
            this.tPageSummary.Text = "表4项目资金策划表";
            this.tPageSummary.UseVisualStyleBackColor = true;
            // 
            // rptGridSummary
            // 
            this.rptGridSummary.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridSummary.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridSummary.CheckedImage")));
            this.rptGridSummary.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridSummary.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridSummary.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridSummary.Location = new System.Drawing.Point(0, 0);
            this.rptGridSummary.Name = "rptGridSummary";
            this.rptGridSummary.Size = new System.Drawing.Size(375, 147);
            this.rptGridSummary.TabIndex = 11;
            this.rptGridSummary.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridSummary.UncheckedImage")));
            // 
            // tPageContrast
            // 
            this.tPageContrast.Controls.Add(this.rptGridContrast);
            this.tPageContrast.Location = new System.Drawing.Point(4, 22);
            this.tPageContrast.Name = "tPageContrast";
            this.tPageContrast.Size = new System.Drawing.Size(375, 147);
            this.tPageContrast.TabIndex = 4;
            this.tPageContrast.Tag = "现金流测算及资金策划对比表";
            this.tPageContrast.Text = "表3现金流测算及资金策划对比表";
            this.tPageContrast.UseVisualStyleBackColor = true;
            // 
            // rptGridContrast
            // 
            this.rptGridContrast.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridContrast.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridContrast.CheckedImage")));
            this.rptGridContrast.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridContrast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridContrast.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridContrast.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridContrast.Location = new System.Drawing.Point(0, 0);
            this.rptGridContrast.Name = "rptGridContrast";
            this.rptGridContrast.Size = new System.Drawing.Size(375, 147);
            this.rptGridContrast.TabIndex = 11;
            this.rptGridContrast.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridContrast.UncheckedImage")));
            // 
            // tPageBalance
            // 
            this.tPageBalance.Controls.Add(this.rptGridBalance);
            this.tPageBalance.Location = new System.Drawing.Point(4, 22);
            this.tPageBalance.Name = "tPageBalance";
            this.tPageBalance.Size = new System.Drawing.Size(375, 147);
            this.tPageBalance.TabIndex = 6;
            this.tPageBalance.Tag = "项目收支平衡点";
            this.tPageBalance.Text = "表2项目收支平衡点";
            this.tPageBalance.UseVisualStyleBackColor = true;
            // 
            // rptGridBalance
            // 
            this.rptGridBalance.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridBalance.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridBalance.CheckedImage")));
            this.rptGridBalance.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridBalance.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridBalance.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridBalance.Location = new System.Drawing.Point(0, 0);
            this.rptGridBalance.Name = "rptGridBalance";
            this.rptGridBalance.Size = new System.Drawing.Size(375, 147);
            this.rptGridBalance.TabIndex = 11;
            this.rptGridBalance.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridBalance.UncheckedImage")));
            // 
            // tPageMaster
            // 
            this.tPageMaster.Controls.Add(this.rptGridMaster);
            this.tPageMaster.Location = new System.Drawing.Point(4, 22);
            this.tPageMaster.Name = "tPageMaster";
            this.tPageMaster.Size = new System.Drawing.Size(375, 147);
            this.tPageMaster.TabIndex = 7;
            this.tPageMaster.Tag = "资金策划审批表";
            this.tPageMaster.Text = "表1资金策划审批表";
            this.tPageMaster.UseVisualStyleBackColor = true;
            // 
            // rptGridMaster
            // 
            this.rptGridMaster.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridMaster.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridMaster.CheckedImage")));
            this.rptGridMaster.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridMaster.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridMaster.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridMaster.Location = new System.Drawing.Point(0, 0);
            this.rptGridMaster.Name = "rptGridMaster";
            this.rptGridMaster.Size = new System.Drawing.Size(375, 147);
            this.rptGridMaster.TabIndex = 12;
            this.rptGridMaster.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridMaster.UncheckedImage")));
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(985, 14);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnUnDo
            // 
            this.btnUnDo.Location = new System.Drawing.Point(799, 14);
            this.btnUnDo.Name = "btnUnDo";
            this.btnUnDo.Size = new System.Drawing.Size(75, 23);
            this.btnUnDo.TabIndex = 10;
            this.btnUnDo.Text = "撤销修改";
            this.btnUnDo.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lbInfo);
            this.panel1.Controls.Add(this.chkIsOnlyOne);
            this.panel1.Controls.Add(this.btnCompute);
            this.panel1.Controls.Add(this.chkAutoCompute);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 532);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1260, 50);
            this.panel1.TabIndex = 11;
            // 
            // lbInfo
            // 
            this.lbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbInfo.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInfo.Location = new System.Drawing.Point(3, 3);
            this.lbInfo.Multiline = true;
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.ReadOnly = true;
            this.lbInfo.Size = new System.Drawing.Size(955, 40);
            this.lbInfo.TabIndex = 15;
            // 
            // chkIsOnlyOne
            // 
            this.chkIsOnlyOne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIsOnlyOne.AutoSize = true;
            this.chkIsOnlyOne.Location = new System.Drawing.Point(1061, 15);
            this.chkIsOnlyOne.Name = "chkIsOnlyOne";
            this.chkIsOnlyOne.Size = new System.Drawing.Size(96, 16);
            this.chkIsOnlyOne.TabIndex = 14;
            this.chkIsOnlyOne.Text = "只计算当前表";
            this.chkIsOnlyOne.UseVisualStyleBackColor = true;
            // 
            // btnCompute
            // 
            this.btnCompute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompute.Location = new System.Drawing.Point(964, 12);
            this.btnCompute.Name = "btnCompute";
            this.btnCompute.Size = new System.Drawing.Size(75, 23);
            this.btnCompute.TabIndex = 13;
            this.btnCompute.Text = "公式计算";
            this.btnCompute.UseVisualStyleBackColor = true;
            // 
            // chkAutoCompute
            // 
            this.chkAutoCompute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutoCompute.AutoSize = true;
            this.chkAutoCompute.Enabled = false;
            this.chkAutoCompute.Location = new System.Drawing.Point(1174, 15);
            this.chkAutoCompute.Name = "chkAutoCompute";
            this.chkAutoCompute.Size = new System.Drawing.Size(72, 16);
            this.chkAutoCompute.TabIndex = 1;
            this.chkAutoCompute.Text = "自动计算";
            this.chkAutoCompute.UseVisualStyleBackColor = true;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(1076, 14);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 9;
            this.btnSubmit.Text = "提交审核";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // VFundSchemeCalculate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 582);
            this.Name = "VFundSchemeCalculate";
            this.Text = "资金测算表";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.tbContent.ResumeLayout(false);
            this.tPageAmount.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tPageCostTax.ResumeLayout(false);
            this.tPageTaxRate.ResumeLayout(false);
            this.tPageFee.ResumeLayout(false);
            this.tPageGether.ResumeLayout(false);
            this.tPagePayment.ResumeLayout(false);
            this.tPageSummary.ResumeLayout(false);
            this.tPageContrast.ResumeLayout(false);
            this.tPageBalance.ResumeLayout(false);
            this.tPageMaster.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UcProjectSelector ucProjectSelector1;
        private System.Windows.Forms.ComboBox cmbFundScheme;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TabControl tbContent;
        private System.Windows.Forms.TabPage tPageAmount;
        private System.Windows.Forms.TabPage tPageCostTax;
        private System.Windows.Forms.TabPage tPageGether;
        private System.Windows.Forms.TabPage tPagePayment;
        private System.Windows.Forms.TabPage tPageContrast;
        private System.Windows.Forms.TabPage tPageSummary;
        private System.Windows.Forms.TabPage tPageBalance;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridAmount;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridTax;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridGether;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridPayment;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridContrast;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridSummary;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridBalance;
        private System.Windows.Forms.TabPage tPageMaster;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridMaster;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnUnDo;
        private System.Windows.Forms.TabPage tPageTaxRate;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridIndRate;
        private System.Windows.Forms.TabPage tPageFee;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridFee;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkAutoCompute;
        private System.Windows.Forms.Button btnCompute;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.CheckBox chkIsOnlyOne;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsMenuEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsMenuCancelEdit;
        private System.Windows.Forms.TextBox lbInfo;
    }
}