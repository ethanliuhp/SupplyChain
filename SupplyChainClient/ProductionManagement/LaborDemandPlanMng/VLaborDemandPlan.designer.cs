namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng
{
    partial class VLaborDemandPlan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VLaborDemandPlan));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.dtpSignDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtPlanName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtHandlePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.customLabel16 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
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
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.colProjectTask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstimateProjectQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMainJobDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQualitySafetyRequirement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlanLaborDemandNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsedPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLaborRankInTime = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.colProjectQuantityUnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).BeginInit();
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
            this.pnlContent.Size = new System.Drawing.Size(938, 441);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl1);
            this.pnlBody.Location = new System.Drawing.Point(0, 75);
            this.pnlBody.Size = new System.Drawing.Size(898, 328);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtProject);
            this.pnlFooter.Controls.Add(this.customLabel1);
            this.pnlFooter.Controls.Add(this.txtCreateDate);
            this.pnlFooter.Controls.Add(this.customLabel6);
            this.pnlFooter.Controls.Add(this.txtCreatePerson);
            this.pnlFooter.Controls.Add(this.customLabel5);
            this.pnlFooter.Location = new System.Drawing.Point(0, 403);
            this.pnlFooter.Size = new System.Drawing.Size(898, 38);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(898, 0);
            this.spTop.Visible = false;
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Size = new System.Drawing.Size(938, 441);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.groupSupply);
            this.pnlHeader.Size = new System.Drawing.Size(898, 75);
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
            this.groupSupply.Controls.Add(this.dtpSignDate);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.txtPlanName);
            this.groupSupply.Controls.Add(this.customLabel2);
            this.groupSupply.Controls.Add(this.txtHandlePerson);
            this.groupSupply.Controls.Add(this.customLabel16);
            this.groupSupply.Controls.Add(this.txtDescript);
            this.groupSupply.Controls.Add(this.customLabel3);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.lblSupplyContract);
            this.groupSupply.Controls.Add(this.flexGrid1);
            this.groupSupply.Location = new System.Drawing.Point(6, 3);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(886, 67);
            this.groupSupply.TabIndex = 2;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>单据信息";
            // 
            // dtpSignDate
            // 
            this.dtpSignDate.Location = new System.Drawing.Point(491, 14);
            this.dtpSignDate.Name = "dtpSignDate";
            this.dtpSignDate.Size = new System.Drawing.Size(109, 21);
            this.dtpSignDate.TabIndex = 151;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(434, 18);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 152;
            this.customLabel4.Text = "提报时间:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtPlanName
            // 
            this.txtPlanName.BackColor = System.Drawing.SystemColors.Control;
            this.txtPlanName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPlanName.DrawSelf = false;
            this.txtPlanName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPlanName.EnterToTab = false;
            this.txtPlanName.Location = new System.Drawing.Point(276, 16);
            this.txtPlanName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPlanName.Name = "txtPlanName";
            this.txtPlanName.Padding = new System.Windows.Forms.Padding(1);
            this.txtPlanName.ReadOnly = false;
            this.txtPlanName.Size = new System.Drawing.Size(141, 16);
            this.txtPlanName.TabIndex = 149;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(213, 20);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 150;
            this.customLabel2.Text = "计划名称:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtHandlePerson
            // 
            this.txtHandlePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtHandlePerson.Code = null;
            this.txtHandlePerson.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtHandlePerson.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtHandlePerson.EnterToTab = false;
            this.txtHandlePerson.Id = "";
            this.txtHandlePerson.IsAllLoad = true;
            this.txtHandlePerson.Location = new System.Drawing.Point(668, 14);
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Result = ((System.Collections.IList)(resources.GetObject("txtHandlePerson.Result")));
            this.txtHandlePerson.RightMouse = false;
            this.txtHandlePerson.Size = new System.Drawing.Size(145, 21);
            this.txtHandlePerson.TabIndex = 147;
            this.txtHandlePerson.Tag = null;
            this.txtHandlePerson.Value = "";
            this.txtHandlePerson.Visible = false;
            this.txtHandlePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel16
            // 
            this.customLabel16.AutoSize = true;
            this.customLabel16.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel16.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel16.Location = new System.Drawing.Point(615, 18);
            this.customLabel16.Name = "customLabel16";
            this.customLabel16.Size = new System.Drawing.Size(47, 12);
            this.customLabel16.TabIndex = 148;
            this.customLabel16.Text = "责任人:";
            this.customLabel16.UnderLineColor = System.Drawing.Color.Red;
            this.customLabel16.Visible = false;
            // 
            // txtDescript
            // 
            this.txtDescript.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescript.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDescript.DrawSelf = false;
            this.txtDescript.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDescript.EnterToTab = false;
            this.txtDescript.Location = new System.Drawing.Point(77, 41);
            this.txtDescript.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDescript.Name = "txtDescript";
            this.txtDescript.Padding = new System.Windows.Forms.Padding(1);
            this.txtDescript.ReadOnly = false;
            this.txtDescript.Size = new System.Drawing.Size(793, 16);
            this.txtDescript.TabIndex = 6;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(38, 42);
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
            this.txtCode.Location = new System.Drawing.Point(79, 14);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(121, 16);
            this.txtCode.TabIndex = 1;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(26, 18);
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
            this.flexGrid1.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.flexGrid1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGrid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGrid1.Location = new System.Drawing.Point(258, 0);
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
            this.colProjectTask,
            this.colMaterialCode,
            this.colMaterialName,
            this.colEstimateProjectQuantity,
            this.colMainJobDescript,
            this.colQualitySafetyRequirement,
            this.colPlanLaborDemandNumber,
            this.colUsedPart,
            this.colLaborRankInTime,
            this.colProjectQuantityUnitName});
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
            this.dgDetail.Size = new System.Drawing.Size(872, 296);
            this.dgDetail.TabIndex = 7;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // cmsDg
            // 
            this.cmsDg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDel});
            this.cmsDg.Name = "cmsDg";
            this.cmsDg.Size = new System.Drawing.Size(101, 26);
            // 
            // tsmiDel
            // 
            this.tsmiDel.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.tsmiDel.Name = "tsmiDel";
            this.tsmiDel.Size = new System.Drawing.Size(100, 22);
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
            this.txtCreateDate.Location = new System.Drawing.Point(236, 12);
            this.txtCreateDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateDate.ReadOnly = true;
            this.txtCreateDate.Size = new System.Drawing.Size(68, 16);
            this.txtCreateDate.TabIndex = 9;
            // 
            // customLabel6
            // 
            this.customLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(175, 15);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 27;
            this.customLabel6.Text = "业务日期:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
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
            this.txtCreatePerson.Size = new System.Drawing.Size(69, 16);
            this.txtCreatePerson.TabIndex = 8;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(19, 15);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(47, 12);
            this.customLabel5.TabIndex = 25;
            this.customLabel5.Text = "制单人:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProject
            // 
            this.txtProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProject.DrawSelf = false;
            this.txtProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProject.EnterToTab = false;
            this.txtProject.Location = new System.Drawing.Point(415, 11);
            this.txtProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProject.Name = "txtProject";
            this.txtProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtProject.ReadOnly = true;
            this.txtProject.Size = new System.Drawing.Size(230, 16);
            this.txtProject.TabIndex = 11;
            // 
            // customLabel1
            // 
            this.customLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(350, 15);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 31;
            this.customLabel1.Text = "归属项目:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(886, 328);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgDetail);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(878, 302);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "明细信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // colProjectTask
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.colProjectTask.DefaultCellStyle = dataGridViewCellStyle1;
            this.colProjectTask.HeaderText = "工程任务";
            this.colProjectTask.Name = "colProjectTask";
            this.colProjectTask.Width = 80;
            // 
            // colMaterialCode
            // 
            this.colMaterialCode.HeaderText = "物资编码";
            this.colMaterialCode.Name = "colMaterialCode";
            // 
            // colMaterialName
            // 
            this.colMaterialName.HeaderText = "物资名称";
            this.colMaterialName.Name = "colMaterialName";
            // 
            // colEstimateProjectQuantity
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colEstimateProjectQuantity.DefaultCellStyle = dataGridViewCellStyle2;
            this.colEstimateProjectQuantity.FillWeight = 80F;
            this.colEstimateProjectQuantity.HeaderText = "预计工程量";
            this.colEstimateProjectQuantity.Name = "colEstimateProjectQuantity";
            // 
            // colMainJobDescript
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colMainJobDescript.DefaultCellStyle = dataGridViewCellStyle3;
            this.colMainJobDescript.HeaderText = "主要工作内容";
            this.colMainJobDescript.Name = "colMainJobDescript";
            this.colMainJobDescript.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colMainJobDescript.Width = 120;
            // 
            // colQualitySafetyRequirement
            // 
            this.colQualitySafetyRequirement.HeaderText = "质量安全专业要求";
            this.colQualitySafetyRequirement.Name = "colQualitySafetyRequirement";
            this.colQualitySafetyRequirement.Width = 140;
            // 
            // colPlanLaborDemandNumber
            // 
            this.colPlanLaborDemandNumber.HeaderText = "计划劳动力需求数量";
            this.colPlanLaborDemandNumber.Name = "colPlanLaborDemandNumber";
            // 
            // colUsedPart
            // 
            this.colUsedPart.HeaderText = "使用部位";
            this.colUsedPart.Name = "colUsedPart";
            // 
            // colLaborRankInTime
            // 
            this.colLaborRankInTime.HeaderText = "计划进场时间";
            this.colLaborRankInTime.Name = "colLaborRankInTime";
            this.colLaborRankInTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colLaborRankInTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colLaborRankInTime.Width = 120;
            // 
            // colProjectQuantityUnitName
            // 
            this.colProjectQuantityUnitName.HeaderText = "工程量单位";
            this.colProjectQuantityUnitName.Name = "colProjectQuantityUnitName";
            // 
            // VLaborDemandPlan
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(898, 441);
            this.Name = "VLaborDemandPlan";
            this.Text = "劳务需求计划";
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).EndInit();
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
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid1;
        private System.Windows.Forms.ContextMenuStrip cmsDg;
        private System.Windows.Forms.ToolStripMenuItem tsmiDel;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtHandlePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel16;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPlanName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpSignDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectTask;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstimateProjectQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMainJobDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQualitySafetyRequirement;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlanLaborDemandNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsedPart;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colLaborRankInTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectQuantityUnitName;
        //private System.Windows.Forms.DataGridViewComboBoxColumn colUsedRankType;
        //private System.Windows.Forms.DataGridViewTextBoxColumn colWokerType;
        //private System.Windows.Forms.DataGridViewTextBoxColumn colProjectTimeLimitUnitName;
    }
}