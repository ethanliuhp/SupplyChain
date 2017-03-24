namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    partial class UcFundSchemeDetail
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcFundSchemeDetail));
            this.tbContent = new System.Windows.Forms.TabControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tspMenuYuan = new System.Windows.Forms.ToolStripMenuItem();
            this.tspMenuWanYuan = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tspMenuFrozen = new System.Windows.Forms.ToolStripMenuItem();
            this.tspMenuUnFrozen = new System.Windows.Forms.ToolStripMenuItem();
            this.tPageAmount = new System.Windows.Forms.TabPage();
            this.rptGridAmount = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
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
            this.tPageAttachment = new System.Windows.Forms.TabPage();
            this.ucAttachment1 = new Application.Business.Erp.SupplyChain.Client.MoneyManage.CommBusinessCtrl.UcAttachment();
            this.lbInfo = new System.Windows.Forms.TextBox();
            this.tbContent.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tPageAmount.SuspendLayout();
            this.tPageCostTax.SuspendLayout();
            this.tPageTaxRate.SuspendLayout();
            this.tPageFee.SuspendLayout();
            this.tPageGether.SuspendLayout();
            this.tPagePayment.SuspendLayout();
            this.tPageSummary.SuspendLayout();
            this.tPageContrast.SuspendLayout();
            this.tPageBalance.SuspendLayout();
            this.tPageMaster.SuspendLayout();
            this.tPageAttachment.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbContent
            // 
            this.tbContent.ContextMenuStrip = this.contextMenuStrip1;
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
            this.tbContent.Controls.Add(this.tPageAttachment);
            this.tbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbContent.Location = new System.Drawing.Point(0, 0);
            this.tbContent.Name = "tbContent";
            this.tbContent.SelectedIndex = 0;
            this.tbContent.Size = new System.Drawing.Size(1062, 577);
            this.tbContent.TabIndex = 327;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspMenuYuan,
            this.tspMenuWanYuan,
            this.toolStripSeparator1,
            this.tspMenuFrozen,
            this.tspMenuUnFrozen});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 98);
            // 
            // tspMenuYuan
            // 
            this.tspMenuYuan.Name = "tspMenuYuan";
            this.tspMenuYuan.Size = new System.Drawing.Size(172, 22);
            this.tspMenuYuan.Text = "以元为单位显示";
            // 
            // tspMenuWanYuan
            // 
            this.tspMenuWanYuan.Name = "tspMenuWanYuan";
            this.tspMenuWanYuan.Size = new System.Drawing.Size(172, 22);
            this.tspMenuWanYuan.Text = "以万元为单位显示";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // tspMenuFrozen
            // 
            this.tspMenuFrozen.Name = "tspMenuFrozen";
            this.tspMenuFrozen.Size = new System.Drawing.Size(172, 22);
            this.tspMenuFrozen.Text = "冻结表头";
            // 
            // tspMenuUnFrozen
            // 
            this.tspMenuUnFrozen.Name = "tspMenuUnFrozen";
            this.tspMenuUnFrozen.Size = new System.Drawing.Size(172, 22);
            this.tspMenuUnFrozen.Text = "取消冻结表头";
            // 
            // tPageAmount
            // 
            this.tPageAmount.Controls.Add(this.rptGridAmount);
            this.tPageAmount.Location = new System.Drawing.Point(4, 22);
            this.tPageAmount.Name = "tPageAmount";
            this.tPageAmount.Padding = new System.Windows.Forms.Padding(3);
            this.tPageAmount.Size = new System.Drawing.Size(1054, 551);
            this.tPageAmount.TabIndex = 0;
            this.tPageAmount.Tag = "项目进度报量及成本测算表";
            this.tPageAmount.Text = "表5项目进度报量及成本测算表";
            this.tPageAmount.UseVisualStyleBackColor = true;
            // 
            // rptGridAmount
            // 
            this.rptGridAmount.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.rptGridAmount.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridAmount.CheckedImage")));
            this.rptGridAmount.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.rptGridAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptGridAmount.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rptGridAmount.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rptGridAmount.Location = new System.Drawing.Point(3, 3);
            this.rptGridAmount.Name = "rptGridAmount";
            this.rptGridAmount.Size = new System.Drawing.Size(1048, 545);
            this.rptGridAmount.TabIndex = 11;
            this.rptGridAmount.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridAmount.UncheckedImage")));
            // 
            // tPageCostTax
            // 
            this.tPageCostTax.Controls.Add(this.rptGridTax);
            this.tPageCostTax.Location = new System.Drawing.Point(4, 22);
            this.tPageCostTax.Name = "tPageCostTax";
            this.tPageCostTax.Padding = new System.Windows.Forms.Padding(3);
            this.tPageCostTax.Size = new System.Drawing.Size(1054, 551);
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
            this.rptGridTax.Size = new System.Drawing.Size(1048, 545);
            this.rptGridTax.TabIndex = 11;
            this.rptGridTax.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridTax.UncheckedImage")));
            // 
            // tPageTaxRate
            // 
            this.tPageTaxRate.Controls.Add(this.rptGridIndRate);
            this.tPageTaxRate.Location = new System.Drawing.Point(4, 22);
            this.tPageTaxRate.Name = "tPageTaxRate";
            this.tPageTaxRate.Size = new System.Drawing.Size(1054, 551);
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
            this.rptGridIndRate.Size = new System.Drawing.Size(1054, 551);
            this.rptGridIndRate.TabIndex = 13;
            this.rptGridIndRate.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridIndRate.UncheckedImage")));
            // 
            // tPageFee
            // 
            this.tPageFee.Controls.Add(this.rptGridFee);
            this.tPageFee.Location = new System.Drawing.Point(4, 22);
            this.tPageFee.Name = "tPageFee";
            this.tPageFee.Size = new System.Drawing.Size(1054, 551);
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
            this.rptGridFee.Size = new System.Drawing.Size(1054, 551);
            this.rptGridFee.TabIndex = 13;
            this.rptGridFee.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridFee.UncheckedImage")));
            // 
            // tPageGether
            // 
            this.tPageGether.Controls.Add(this.rptGridGether);
            this.tPageGether.Location = new System.Drawing.Point(4, 22);
            this.tPageGether.Name = "tPageGether";
            this.tPageGether.Size = new System.Drawing.Size(1054, 551);
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
            this.rptGridGether.Size = new System.Drawing.Size(1054, 551);
            this.rptGridGether.TabIndex = 11;
            this.rptGridGether.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridGether.UncheckedImage")));
            // 
            // tPagePayment
            // 
            this.tPagePayment.Controls.Add(this.rptGridPayment);
            this.tPagePayment.Location = new System.Drawing.Point(4, 22);
            this.tPagePayment.Name = "tPagePayment";
            this.tPagePayment.Size = new System.Drawing.Size(1054, 551);
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
            this.rptGridPayment.Size = new System.Drawing.Size(1054, 551);
            this.rptGridPayment.TabIndex = 11;
            this.rptGridPayment.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridPayment.UncheckedImage")));
            // 
            // tPageSummary
            // 
            this.tPageSummary.Controls.Add(this.rptGridSummary);
            this.tPageSummary.Location = new System.Drawing.Point(4, 22);
            this.tPageSummary.Name = "tPageSummary";
            this.tPageSummary.Size = new System.Drawing.Size(1054, 551);
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
            this.rptGridSummary.Size = new System.Drawing.Size(1054, 551);
            this.rptGridSummary.TabIndex = 11;
            this.rptGridSummary.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridSummary.UncheckedImage")));
            // 
            // tPageContrast
            // 
            this.tPageContrast.Controls.Add(this.rptGridContrast);
            this.tPageContrast.Location = new System.Drawing.Point(4, 22);
            this.tPageContrast.Name = "tPageContrast";
            this.tPageContrast.Size = new System.Drawing.Size(1054, 551);
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
            this.rptGridContrast.Size = new System.Drawing.Size(1054, 551);
            this.rptGridContrast.TabIndex = 11;
            this.rptGridContrast.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridContrast.UncheckedImage")));
            // 
            // tPageBalance
            // 
            this.tPageBalance.Controls.Add(this.rptGridBalance);
            this.tPageBalance.Location = new System.Drawing.Point(4, 22);
            this.tPageBalance.Name = "tPageBalance";
            this.tPageBalance.Size = new System.Drawing.Size(1054, 551);
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
            this.rptGridBalance.Size = new System.Drawing.Size(1054, 551);
            this.rptGridBalance.TabIndex = 11;
            this.rptGridBalance.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridBalance.UncheckedImage")));
            // 
            // tPageMaster
            // 
            this.tPageMaster.Controls.Add(this.rptGridMaster);
            this.tPageMaster.Location = new System.Drawing.Point(4, 22);
            this.tPageMaster.Name = "tPageMaster";
            this.tPageMaster.Size = new System.Drawing.Size(1054, 551);
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
            this.rptGridMaster.Size = new System.Drawing.Size(1054, 551);
            this.rptGridMaster.TabIndex = 12;
            this.rptGridMaster.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("rptGridMaster.UncheckedImage")));
            // 
            // tPageAttachment
            // 
            this.tPageAttachment.Controls.Add(this.ucAttachment1);
            this.tPageAttachment.Location = new System.Drawing.Point(4, 22);
            this.tPageAttachment.Name = "tPageAttachment";
            this.tPageAttachment.Size = new System.Drawing.Size(1054, 551);
            this.tPageAttachment.TabIndex = 10;
            this.tPageAttachment.Text = "表1附件";
            this.tPageAttachment.UseVisualStyleBackColor = true;
            // 
            // ucAttachment1
            // 
            this.ucAttachment1.CurrentBill = null;
            this.ucAttachment1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucAttachment1.IsOnlyBrowse = false;
            this.ucAttachment1.Location = new System.Drawing.Point(0, 0);
            this.ucAttachment1.Name = "ucAttachment1";
            this.ucAttachment1.Size = new System.Drawing.Size(1054, 551);
            this.ucAttachment1.TabIndex = 0;
            // 
            // lbInfo
            // 
            this.lbInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbInfo.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInfo.Location = new System.Drawing.Point(0, 577);
            this.lbInfo.Multiline = true;
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.ReadOnly = true;
            this.lbInfo.Size = new System.Drawing.Size(1062, 40);
            this.lbInfo.TabIndex = 328;
            // 
            // UcFundSchemeDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbContent);
            this.Controls.Add(this.lbInfo);
            this.Name = "UcFundSchemeDetail";
            this.Size = new System.Drawing.Size(1062, 617);
            this.tbContent.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tPageAmount.ResumeLayout(false);
            this.tPageCostTax.ResumeLayout(false);
            this.tPageTaxRate.ResumeLayout(false);
            this.tPageFee.ResumeLayout(false);
            this.tPageGether.ResumeLayout(false);
            this.tPagePayment.ResumeLayout(false);
            this.tPageSummary.ResumeLayout(false);
            this.tPageContrast.ResumeLayout(false);
            this.tPageBalance.ResumeLayout(false);
            this.tPageMaster.ResumeLayout(false);
            this.tPageAttachment.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tbContent;
        private System.Windows.Forms.TabPage tPageAmount;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridAmount;
        private System.Windows.Forms.TabPage tPageCostTax;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridTax;
        private System.Windows.Forms.TabPage tPageTaxRate;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridIndRate;
        private System.Windows.Forms.TabPage tPageFee;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridFee;
        private System.Windows.Forms.TabPage tPageGether;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridGether;
        private System.Windows.Forms.TabPage tPagePayment;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridPayment;
        private System.Windows.Forms.TabPage tPageSummary;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridSummary;
        private System.Windows.Forms.TabPage tPageContrast;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridContrast;
        private System.Windows.Forms.TabPage tPageBalance;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridBalance;
        private System.Windows.Forms.TabPage tPageMaster;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid rptGridMaster;
        private System.Windows.Forms.TextBox lbInfo;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tspMenuYuan;
        private System.Windows.Forms.ToolStripMenuItem tspMenuWanYuan;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tspMenuFrozen;
        private System.Windows.Forms.ToolStripMenuItem tspMenuUnFrozen;
        private System.Windows.Forms.TabPage tPageAttachment;
        private Application.Business.Erp.SupplyChain.Client.MoneyManage.CommBusinessCtrl.UcAttachment ucAttachment1;
    }
}
