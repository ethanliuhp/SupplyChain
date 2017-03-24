namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng
{
    partial class VStockInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VStockInventory));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.lbUserPart = new System.Windows.Forms.Label();
            this.btnSelectUserPart = new System.Windows.Forms.Button();
            this.txtUserPart = new System.Windows.Forms.TextBox();
            this.cboProfessionCat = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.btnSelectUsedRank = new System.Windows.Forms.Button();
            this.lblUsedRank = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtUsedRank = new System.Windows.Forms.TextBox();
            this.txtMaterialCategory = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial();
            this.lblCat = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtHandlePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblHandlePerson = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpSignDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtDescript = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.flexGrid1 = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.cmsDg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDel = new System.Windows.Forms.ToolStripMenuItem();
            this.txtCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSumQuantity = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lbSumQuantity = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInventoryQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplyPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplyMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfirmPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfirmMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiagramNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialClassify = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubjectGuid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubjectName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colSubjectSysCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlContent.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterialCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.cmsDg.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Size = new System.Drawing.Size(879, 459);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl1);
            this.pnlBody.Location = new System.Drawing.Point(0, 74);
            this.pnlBody.Size = new System.Drawing.Size(877, 345);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtProject);
            this.pnlFooter.Controls.Add(this.customLabel7);
            this.pnlFooter.Controls.Add(this.txtCreateDate);
            this.pnlFooter.Controls.Add(this.customLabel6);
            this.pnlFooter.Controls.Add(this.txtSumQuantity);
            this.pnlFooter.Controls.Add(this.lbSumQuantity);
            this.pnlFooter.Controls.Add(this.txtCreatePerson);
            this.pnlFooter.Controls.Add(this.customLabel5);
            this.pnlFooter.Location = new System.Drawing.Point(0, 419);
            this.pnlFooter.Size = new System.Drawing.Size(877, 38);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(879, 0);
            this.spTop.Visible = false;
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Size = new System.Drawing.Size(879, 459);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.groupSupply);
            this.pnlHeader.Size = new System.Drawing.Size(877, 74);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.groupSupply, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Visible = false;
            // 
            // groupSupply
            // 
            this.groupSupply.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSupply.Controls.Add(this.lbUserPart);
            this.groupSupply.Controls.Add(this.btnSelectUserPart);
            this.groupSupply.Controls.Add(this.txtUserPart);
            this.groupSupply.Controls.Add(this.cboProfessionCat);
            this.groupSupply.Controls.Add(this.btnSelectUsedRank);
            this.groupSupply.Controls.Add(this.lblUsedRank);
            this.groupSupply.Controls.Add(this.txtUsedRank);
            this.groupSupply.Controls.Add(this.txtMaterialCategory);
            this.groupSupply.Controls.Add(this.lblCat);
            this.groupSupply.Controls.Add(this.txtHandlePerson);
            this.groupSupply.Controls.Add(this.lblHandlePerson);
            this.groupSupply.Controls.Add(this.dtpSignDate);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.txtDescript);
            this.groupSupply.Controls.Add(this.customLabel3);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.lblSupplyContract);
            this.groupSupply.Controls.Add(this.flexGrid1);
            this.groupSupply.Location = new System.Drawing.Point(3, 3);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(860, 66);
            this.groupSupply.TabIndex = 2;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>单据信息";
            // 
            // lbUserPart
            // 
            this.lbUserPart.AutoSize = true;
            this.lbUserPart.Location = new System.Drawing.Point(661, 16);
            this.lbUserPart.Name = "lbUserPart";
            this.lbUserPart.Size = new System.Drawing.Size(59, 12);
            this.lbUserPart.TabIndex = 137;
            this.lbUserPart.Text = "使用部位:";
            // 
            // btnSelectUserPart
            // 
            this.btnSelectUserPart.Location = new System.Drawing.Point(791, 10);
            this.btnSelectUserPart.Name = "btnSelectUserPart";
            this.btnSelectUserPart.Size = new System.Drawing.Size(63, 23);
            this.btnSelectUserPart.TabIndex = 139;
            this.btnSelectUserPart.Text = "选择";
            this.btnSelectUserPart.UseVisualStyleBackColor = true;
            // 
            // txtUserPart
            // 
            this.txtUserPart.Location = new System.Drawing.Point(726, 12);
            this.txtUserPart.Name = "txtUserPart";
            this.txtUserPart.Size = new System.Drawing.Size(59, 21);
            this.txtUserPart.TabIndex = 138;
            // 
            // cboProfessionCat
            // 
            this.cboProfessionCat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboProfessionCat.FormattingEnabled = true;
            this.cboProfessionCat.Location = new System.Drawing.Point(528, 14);
            this.cboProfessionCat.Name = "cboProfessionCat";
            this.cboProfessionCat.Size = new System.Drawing.Size(96, 20);
            this.cboProfessionCat.TabIndex = 9;
            // 
            // btnSelectUsedRank
            // 
            this.btnSelectUsedRank.Location = new System.Drawing.Point(634, 37);
            this.btnSelectUsedRank.Name = "btnSelectUsedRank";
            this.btnSelectUsedRank.Size = new System.Drawing.Size(63, 23);
            this.btnSelectUsedRank.TabIndex = 135;
            this.btnSelectUsedRank.Text = "选择";
            this.btnSelectUsedRank.UseVisualStyleBackColor = true;
            // 
            // lblUsedRank
            // 
            this.lblUsedRank.AutoSize = true;
            this.lblUsedRank.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUsedRank.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblUsedRank.Location = new System.Drawing.Point(468, 45);
            this.lblUsedRank.Name = "lblUsedRank";
            this.lblUsedRank.Size = new System.Drawing.Size(59, 12);
            this.lblUsedRank.TabIndex = 136;
            this.lblUsedRank.Text = "使用队伍:";
            this.lblUsedRank.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtUsedRank
            // 
            this.txtUsedRank.Location = new System.Drawing.Point(528, 38);
            this.txtUsedRank.Name = "txtUsedRank";
            this.txtUsedRank.Size = new System.Drawing.Size(100, 21);
            this.txtUsedRank.TabIndex = 134;
            // 
            // txtMaterialCategory
            // 
            this.txtMaterialCategory.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterialCategory.Code = null;
            this.txtMaterialCategory.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtMaterialCategory.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtMaterialCategory.EnterToTab = false;
            this.txtMaterialCategory.Id = "";
            this.txtMaterialCategory.IsAllLoad = true;
            this.txtMaterialCategory.Location = new System.Drawing.Point(528, 13);
            this.txtMaterialCategory.Name = "txtMaterialCategory";
            this.txtMaterialCategory.ObjectType = Application.Business.Erp.ResourceManager.Client.Basic.Controls.MaterialSelectType.MaterialCatView;
            this.txtMaterialCategory.Result = ((System.Collections.IList)(resources.GetObject("txtMaterialCategory.Result")));
            this.txtMaterialCategory.RightMouse = false;
            this.txtMaterialCategory.Size = new System.Drawing.Size(169, 21);
            this.txtMaterialCategory.TabIndex = 132;
            this.txtMaterialCategory.Tag = null;
            this.txtMaterialCategory.Value = "";
            this.txtMaterialCategory.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // lblCat
            // 
            this.lblCat.AutoSize = true;
            this.lblCat.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCat.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCat.Location = new System.Drawing.Point(466, 18);
            this.lblCat.Name = "lblCat";
            this.lblCat.Size = new System.Drawing.Size(59, 12);
            this.lblCat.TabIndex = 133;
            this.lblCat.Text = "物资分类:";
            this.lblCat.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtHandlePerson
            // 
            this.txtHandlePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtHandlePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtHandlePerson.DrawSelf = false;
            this.txtHandlePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtHandlePerson.EnterToTab = false;
            this.txtHandlePerson.Location = new System.Drawing.Point(754, 15);
            this.txtHandlePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtHandlePerson.ReadOnly = true;
            this.txtHandlePerson.Size = new System.Drawing.Size(99, 16);
            this.txtHandlePerson.TabIndex = 3;
            this.txtHandlePerson.Visible = false;
            // 
            // lblHandlePerson
            // 
            this.lblHandlePerson.AutoSize = true;
            this.lblHandlePerson.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHandlePerson.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblHandlePerson.Location = new System.Drawing.Point(703, 19);
            this.lblHandlePerson.Name = "lblHandlePerson";
            this.lblHandlePerson.Size = new System.Drawing.Size(47, 12);
            this.lblHandlePerson.TabIndex = 131;
            this.lblHandlePerson.Text = "责任人:";
            this.lblHandlePerson.UnderLineColor = System.Drawing.Color.Red;
            this.lblHandlePerson.Visible = false;
            // 
            // dtpSignDate
            // 
            this.dtpSignDate.Location = new System.Drawing.Point(334, 14);
            this.dtpSignDate.Name = "dtpSignDate";
            this.dtpSignDate.Size = new System.Drawing.Size(123, 21);
            this.dtpSignDate.TabIndex = 2;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(272, 18);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 130;
            this.customLabel4.Text = "盘点日期:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtDescript
            // 
            this.txtDescript.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescript.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDescript.DrawSelf = false;
            this.txtDescript.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDescript.EnterToTab = false;
            this.txtDescript.Location = new System.Drawing.Point(86, 43);
            this.txtDescript.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDescript.Name = "txtDescript";
            this.txtDescript.Padding = new System.Windows.Forms.Padding(1);
            this.txtDescript.ReadOnly = false;
            this.txtDescript.Size = new System.Drawing.Size(371, 16);
            this.txtDescript.TabIndex = 6;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(49, 44);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(35, 12);
            this.customLabel3.TabIndex = 9;
            this.customLabel3.Text = "备注:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(89, 16);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(174, 16);
            this.txtCode.TabIndex = 1;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(37, 20);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(47, 12);
            this.lblSupplyContract.TabIndex = 0;
            this.lblSupplyContract.Text = "单据号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
            // 
            // flexGrid1
            // 
            this.flexGrid1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flexGrid1.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid1.CheckedImage")));
            this.flexGrid1.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.flexGrid1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGrid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGrid1.Location = new System.Drawing.Point(277, 0);
            this.flexGrid1.Name = "flexGrid1";
            this.flexGrid1.Size = new System.Drawing.Size(473, 19);
            this.flexGrid1.TabIndex = 18;
            this.flexGrid1.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid1.UncheckedImage")));
            this.flexGrid1.Visible = false;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = true;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaterialCode,
            this.colMaterialName,
            this.colMaterialSpec,
            this.colUnit,
            this.colStockQuantity,
            this.colInventoryQuantity,
            this.colSupplyPrice,
            this.colSupplyMoney,
            this.colConfirmPrice,
            this.colConfirmMoney,
            this.colDiagramNum,
            this.colMaterialClassify,
            this.colDescript,
            this.colSubjectGuid,
            this.colSubjectName,
            this.colSubjectSysCode});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(3, 3);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.PageIndex = 0;
            this.dgDetail.PageSize = 0;
            this.dgDetail.Query = null;
            this.dgDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDetail.ReadOnlyCols")));
            this.dgDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetail.Size = new System.Drawing.Size(851, 314);
            this.dgDetail.TabIndex = 7;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // cmsDg
            // 
            this.cmsDg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDel});
            this.cmsDg.Name = "cmsDg";
            this.cmsDg.Size = new System.Drawing.Size(95, 26);
            // 
            // tsmiDel
            // 
            this.tsmiDel.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.tsmiDel.Name = "tsmiDel";
            this.tsmiDel.Size = new System.Drawing.Size(94, 22);
            this.tsmiDel.Text = "删除";
            this.tsmiDel.ToolTipText = "删除当前选中的记录";
            // 
            // txtCreateDate
            // 
            this.txtCreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreateDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreateDate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreateDate.DrawSelf = false;
            this.txtCreateDate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreateDate.EnterToTab = false;
            this.txtCreateDate.Location = new System.Drawing.Point(746, 12);
            this.txtCreateDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateDate.ReadOnly = true;
            this.txtCreateDate.Size = new System.Drawing.Size(117, 16);
            this.txtCreateDate.TabIndex = 9;
            this.txtCreateDate.Visible = false;
            // 
            // customLabel6
            // 
            this.customLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(685, 15);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 27;
            this.customLabel6.Text = "业务日期:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            this.customLabel6.Visible = false;
            // 
            // txtSumQuantity
            // 
            this.txtSumQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSumQuantity.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumQuantity.DrawSelf = false;
            this.txtSumQuantity.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumQuantity.EnterToTab = false;
            this.txtSumQuantity.Location = new System.Drawing.Point(298, 11);
            this.txtSumQuantity.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumQuantity.Name = "txtSumQuantity";
            this.txtSumQuantity.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumQuantity.ReadOnly = true;
            this.txtSumQuantity.Size = new System.Drawing.Size(107, 16);
            this.txtSumQuantity.TabIndex = 10;
            // 
            // lbSumQuantity
            // 
            this.lbSumQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbSumQuantity.AutoSize = true;
            this.lbSumQuantity.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSumQuantity.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lbSumQuantity.Location = new System.Drawing.Point(235, 15);
            this.lbSumQuantity.Name = "lbSumQuantity";
            this.lbSumQuantity.Size = new System.Drawing.Size(59, 12);
            this.lbSumQuantity.TabIndex = 29;
            this.lbSumQuantity.Text = "合计数量:";
            this.lbSumQuantity.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(70, 11);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = true;
            this.txtCreatePerson.Size = new System.Drawing.Size(119, 16);
            this.txtCreatePerson.TabIndex = 8;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(19, 15);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(47, 12);
            this.customLabel5.TabIndex = 25;
            this.customLabel5.Text = "制单人:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(865, 345);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(857, 320);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "明细信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtProject
            // 
            this.txtProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProject.DrawSelf = false;
            this.txtProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProject.EnterToTab = false;
            this.txtProject.Location = new System.Drawing.Point(473, 11);
            this.txtProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProject.Name = "txtProject";
            this.txtProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtProject.ReadOnly = true;
            this.txtProject.Size = new System.Drawing.Size(184, 16);
            this.txtProject.TabIndex = 34;
            // 
            // customLabel7
            // 
            this.customLabel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(413, 15);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 35;
            this.customLabel7.Text = "归属项目:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // colMaterialCode
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.colMaterialCode.DefaultCellStyle = dataGridViewCellStyle1;
            this.colMaterialCode.HeaderText = "物资编码";
            this.colMaterialCode.Name = "colMaterialCode";
            this.colMaterialCode.Width = 80;
            // 
            // colMaterialName
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colMaterialName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colMaterialName.FillWeight = 80F;
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            this.colMaterialName.ReadOnly = true;
            this.colMaterialName.Width = 80;
            // 
            // colMaterialSpec
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colMaterialSpec.DefaultCellStyle = dataGridViewCellStyle3;
            this.colMaterialSpec.FillWeight = 80F;
            this.colMaterialSpec.HeaderText = "规格型号";
            this.colMaterialSpec.Name = "colMaterialSpec";
            this.colMaterialSpec.ReadOnly = true;
            // 
            // colUnit
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.colUnit.DefaultCellStyle = dataGridViewCellStyle4;
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            this.colUnit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colUnit.Width = 80;
            // 
            // colStockQuantity
            // 
            this.colStockQuantity.HeaderText = "库存数量";
            this.colStockQuantity.Name = "colStockQuantity";
            this.colStockQuantity.Width = 80;
            // 
            // colInventoryQuantity
            // 
            this.colInventoryQuantity.HeaderText = "盘点数量";
            this.colInventoryQuantity.Name = "colInventoryQuantity";
            // 
            // colSupplyPrice
            // 
            this.colSupplyPrice.HeaderText = "采购单价";
            this.colSupplyPrice.Name = "colSupplyPrice";
            // 
            // colSupplyMoney
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.colSupplyMoney.DefaultCellStyle = dataGridViewCellStyle5;
            this.colSupplyMoney.HeaderText = "采购合价";
            this.colSupplyMoney.Name = "colSupplyMoney";
            // 
            // colConfirmPrice
            // 
            this.colConfirmPrice.HeaderText = "认价单价";
            this.colConfirmPrice.Name = "colConfirmPrice";
            // 
            // colConfirmMoney
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            this.colConfirmMoney.DefaultCellStyle = dataGridViewCellStyle6;
            this.colConfirmMoney.HeaderText = "认价合价";
            this.colConfirmMoney.Name = "colConfirmMoney";
            // 
            // colDiagramNum
            // 
            this.colDiagramNum.HeaderText = "图号";
            this.colDiagramNum.Name = "colDiagramNum";
            // 
            // colMaterialClassify
            // 
            this.colMaterialClassify.HeaderText = "物资分类层次";
            this.colMaterialClassify.Name = "colMaterialClassify";
            this.colMaterialClassify.Visible = false;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "备注";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 200;
            // 
            // colSubjectGuid
            // 
            this.colSubjectGuid.HeaderText = "核算科目ID";
            this.colSubjectGuid.Name = "colSubjectGuid";
            this.colSubjectGuid.ReadOnly = true;
            this.colSubjectGuid.Visible = false;
            // 
            // colSubjectName
            // 
            this.colSubjectName.HeaderText = "核算科目";
            this.colSubjectName.Name = "colSubjectName";
            this.colSubjectName.ReadOnly = true;
            this.colSubjectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSubjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colSubjectSysCode
            // 
            this.colSubjectSysCode.HeaderText = "核算科目编码";
            this.colSubjectSysCode.Name = "colSubjectSysCode";
            this.colSubjectSysCode.ReadOnly = true;
            this.colSubjectSysCode.Visible = false;
            // 
            // VStockInventory
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(879, 459);
            this.Name = "VStockInventory";
            this.Text = "月度盘点单";
            this.pnlContent.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlWorkSpace.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterialCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.cmsDg.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDescript;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lbSumQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpSignDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtHandlePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblHandlePerson;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid1;
        private System.Windows.Forms.ContextMenuStrip cmsDg;
        private System.Windows.Forms.ToolStripMenuItem tsmiDel;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial txtMaterialCategory;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCat;
        private System.Windows.Forms.Button btnSelectUsedRank;
        private System.Windows.Forms.TextBox txtUsedRank;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblUsedRank;
        //private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboProfessionCat;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboProfessionCat;
        private System.Windows.Forms.Button btnSelectUserPart;
        private System.Windows.Forms.TextBox txtUserPart;
        private System.Windows.Forms.Label lbUserPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInventoryQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplyPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplyMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfirmPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfirmMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiagramNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialClassify;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubjectGuid;
        private System.Windows.Forms.DataGridViewComboBoxColumn colSubjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubjectSysCode;
    }
}