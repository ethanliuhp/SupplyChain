namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    partial class VImportGWBSDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VImportGWBSDetail));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbUsage = new System.Windows.Forms.GroupBox();
            this.cbCostAccountFlag = new System.Windows.Forms.CheckBox();
            this.cbResponseAccountFlag = new System.Windows.Forms.CheckBox();
            this.btnRemoveSelectedRows = new System.Windows.Forms.Button();
            this.btnLookCostItemInfo = new System.Windows.Forms.Button();
            this.txtContractGroupType = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtContractGroupName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectContractGroup = new System.Windows.Forms.Button();
            this.gridDtl = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colQuotaCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDtlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlanQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtCurrentPath = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtExcelFilePath = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnBrownFile = new System.Windows.Forms.Button();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdAddTaskDtl = new System.Windows.Forms.RadioButton();
            this.rdClearTaskDtl = new System.Windows.Forms.RadioButton();
            this.cbTableNames = new System.Windows.Forms.ListBox();
            this.btnLoadExcelData = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ckbProduceConfirmFlag = new System.Windows.Forms.CheckBox();
            this.pnlFloor.SuspendLayout();
            this.gbUsage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDtl)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.btnSave);
            this.pnlFloor.Controls.Add(this.gbUsage);
            this.pnlFloor.Size = new System.Drawing.Size(955, 500);
            this.pnlFloor.Controls.SetChildIndex(this.gbUsage, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSave, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            // 
            // gbUsage
            // 
            this.gbUsage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbUsage.Controls.Add(this.ckbProduceConfirmFlag);
            this.gbUsage.Controls.Add(this.cbCostAccountFlag);
            this.gbUsage.Controls.Add(this.cbResponseAccountFlag);
            this.gbUsage.Controls.Add(this.btnRemoveSelectedRows);
            this.gbUsage.Controls.Add(this.btnLookCostItemInfo);
            this.gbUsage.Controls.Add(this.txtContractGroupType);
            this.gbUsage.Controls.Add(this.label6);
            this.gbUsage.Controls.Add(this.txtContractGroupName);
            this.gbUsage.Controls.Add(this.label1);
            this.gbUsage.Controls.Add(this.btnSelectContractGroup);
            this.gbUsage.Controls.Add(this.gridDtl);
            this.gbUsage.Controls.Add(this.label2);
            this.gbUsage.Location = new System.Drawing.Point(458, 43);
            this.gbUsage.Name = "gbUsage";
            this.gbUsage.Size = new System.Drawing.Size(494, 418);
            this.gbUsage.TabIndex = 102;
            this.gbUsage.TabStop = false;
            this.gbUsage.Text = "成本明细列表";
            // 
            // cbCostAccountFlag
            // 
            this.cbCostAccountFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbCostAccountFlag.AutoSize = true;
            this.cbCostAccountFlag.Checked = true;
            this.cbCostAccountFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCostAccountFlag.Location = new System.Drawing.Point(110, 376);
            this.cbCostAccountFlag.Name = "cbCostAccountFlag";
            this.cbCostAccountFlag.Size = new System.Drawing.Size(96, 16);
            this.cbCostAccountFlag.TabIndex = 120;
            this.cbCostAccountFlag.Text = "成本核算明细";
            this.cbCostAccountFlag.UseVisualStyleBackColor = true;
            // 
            // cbResponseAccountFlag
            // 
            this.cbResponseAccountFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbResponseAccountFlag.AutoSize = true;
            this.cbResponseAccountFlag.Checked = true;
            this.cbResponseAccountFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbResponseAccountFlag.Location = new System.Drawing.Point(8, 376);
            this.cbResponseAccountFlag.Name = "cbResponseAccountFlag";
            this.cbResponseAccountFlag.Size = new System.Drawing.Size(96, 16);
            this.cbResponseAccountFlag.TabIndex = 119;
            this.cbResponseAccountFlag.Text = "责任核算明细";
            this.cbResponseAccountFlag.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSelectedRows
            // 
            this.btnRemoveSelectedRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveSelectedRows.Location = new System.Drawing.Point(350, 392);
            this.btnRemoveSelectedRows.Name = "btnRemoveSelectedRows";
            this.btnRemoveSelectedRows.Size = new System.Drawing.Size(115, 23);
            this.btnRemoveSelectedRows.TabIndex = 49;
            this.btnRemoveSelectedRows.Text = "移除选中行";
            this.btnRemoveSelectedRows.UseVisualStyleBackColor = true;
            // 
            // btnLookCostItemInfo
            // 
            this.btnLookCostItemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLookCostItemInfo.Location = new System.Drawing.Point(212, 392);
            this.btnLookCostItemInfo.Name = "btnLookCostItemInfo";
            this.btnLookCostItemInfo.Size = new System.Drawing.Size(132, 23);
            this.btnLookCostItemInfo.TabIndex = 50;
            this.btnLookCostItemInfo.Text = "查看/修改成本项信息";
            this.btnLookCostItemInfo.UseVisualStyleBackColor = true;
            // 
            // txtContractGroupType
            // 
            this.txtContractGroupType.Location = new System.Drawing.Point(290, 20);
            this.txtContractGroupType.Name = "txtContractGroupType";
            this.txtContractGroupType.ReadOnly = true;
            this.txtContractGroupType.Size = new System.Drawing.Size(110, 21);
            this.txtContractGroupType.TabIndex = 117;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(230, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 118;
            this.label6.Text = "契约类型：";
            // 
            // txtContractGroupName
            // 
            this.txtContractGroupName.Location = new System.Drawing.Point(66, 21);
            this.txtContractGroupName.Name = "txtContractGroupName";
            this.txtContractGroupName.ReadOnly = true;
            this.txtContractGroupName.Size = new System.Drawing.Size(158, 21);
            this.txtContractGroupName.TabIndex = 114;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 116;
            this.label1.Text = "契约名称：";
            // 
            // btnSelectContractGroup
            // 
            this.btnSelectContractGroup.Location = new System.Drawing.Point(406, 19);
            this.btnSelectContractGroup.Name = "btnSelectContractGroup";
            this.btnSelectContractGroup.Size = new System.Drawing.Size(69, 23);
            this.btnSelectContractGroup.TabIndex = 115;
            this.btnSelectContractGroup.Text = "选择契约";
            this.btnSelectContractGroup.UseVisualStyleBackColor = true;
            // 
            // gridDtl
            // 
            this.gridDtl.AddDefaultMenu = false;
            this.gridDtl.AddNoColumn = true;
            this.gridDtl.AllowUserToAddRows = false;
            this.gridDtl.AllowUserToDeleteRows = false;
            this.gridDtl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDtl.BackgroundColor = System.Drawing.Color.White;
            this.gridDtl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridDtl.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridDtl.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDtl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDtl.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colQuotaCode,
            this.colDtlName,
            this.colPlanQuantity});
            this.gridDtl.CustomBackColor = false;
            this.gridDtl.EditCellBackColor = System.Drawing.Color.White;
            this.gridDtl.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDtl.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridDtl.FreezeFirstRow = false;
            this.gridDtl.FreezeLastRow = false;
            this.gridDtl.FrontColumnCount = 0;
            this.gridDtl.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridDtl.HScrollOffset = 0;
            this.gridDtl.IsAllowOrder = true;
            this.gridDtl.IsConfirmDelete = true;
            this.gridDtl.Location = new System.Drawing.Point(3, 68);
            this.gridDtl.Name = "gridDtl";
            this.gridDtl.PageIndex = 0;
            this.gridDtl.PageSize = 0;
            this.gridDtl.Query = null;
            this.gridDtl.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridDtl.ReadOnlyCols")));
            this.gridDtl.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridDtl.RowHeadersWidth = 22;
            this.gridDtl.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridDtl.RowTemplate.Height = 23;
            this.gridDtl.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDtl.Size = new System.Drawing.Size(491, 302);
            this.gridDtl.TabIndex = 46;
            this.gridDtl.TargetType = null;
            this.gridDtl.VScrollOffset = 0;
            // 
            // colQuotaCode
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.colQuotaCode.DefaultCellStyle = dataGridViewCellStyle4;
            this.colQuotaCode.HeaderText = "定额编号";
            this.colQuotaCode.Name = "colQuotaCode";
            this.colQuotaCode.ReadOnly = true;
            // 
            // colDtlName
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            this.colDtlName.DefaultCellStyle = dataGridViewCellStyle5;
            this.colDtlName.HeaderText = "明细名称";
            this.colDtlName.Name = "colDtlName";
            this.colDtlName.Width = 200;
            // 
            // colPlanQuantity
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            this.colPlanQuantity.DefaultCellStyle = dataGridViewCellStyle6;
            this.colPlanQuantity.HeaderText = "计划工程量";
            this.colPlanQuantity.Name = "colPlanQuantity";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 113;
            this.label2.Text = "注：双击查看/修改成本项信息";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(808, 467);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 23);
            this.btnCancel.TabIndex = 49;
            this.btnCancel.Text = "退出/取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(670, 467);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(132, 23);
            this.btnSave.TabIndex = 50;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.txtCurrentPath);
            this.panel1.Controls.Add(this.customLabel1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(949, 34);
            this.panel1.TabIndex = 103;
            // 
            // txtCurrentPath
            // 
            this.txtCurrentPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentPath.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCurrentPath.DrawSelf = false;
            this.txtCurrentPath.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCurrentPath.EnterToTab = false;
            this.txtCurrentPath.Location = new System.Drawing.Point(92, 5);
            this.txtCurrentPath.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCurrentPath.Name = "txtCurrentPath";
            this.txtCurrentPath.Padding = new System.Windows.Forms.Padding(1);
            this.txtCurrentPath.ReadOnly = true;
            this.txtCurrentPath.Size = new System.Drawing.Size(847, 18);
            this.txtCurrentPath.TabIndex = 120;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(6, 9);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(89, 12);
            this.customLabel1.TabIndex = 119;
            this.customLabel1.Text = "操作项目任务：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtExcelFilePath
            // 
            this.txtExcelFilePath.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtExcelFilePath.DrawSelf = false;
            this.txtExcelFilePath.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtExcelFilePath.EnterToTab = false;
            this.txtExcelFilePath.Location = new System.Drawing.Point(73, 26);
            this.txtExcelFilePath.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtExcelFilePath.Name = "txtExcelFilePath";
            this.txtExcelFilePath.Padding = new System.Windows.Forms.Padding(1);
            this.txtExcelFilePath.ReadOnly = true;
            this.txtExcelFilePath.Size = new System.Drawing.Size(309, 18);
            this.txtExcelFilePath.TabIndex = 112;
            // 
            // btnBrownFile
            // 
            this.btnBrownFile.Location = new System.Drawing.Point(385, 21);
            this.btnBrownFile.Name = "btnBrownFile";
            this.btnBrownFile.Size = new System.Drawing.Size(61, 23);
            this.btnBrownFile.TabIndex = 110;
            this.btnBrownFile.Text = "浏览";
            this.btnBrownFile.UseVisualStyleBackColor = true;
            // 
            // customLabel4
            // 
            this.customLabel4.AddColonAuto = true;
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(7, 30);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(71, 12);
            this.customLabel4.TabIndex = 111;
            this.customLabel4.Text = "Excel文件：";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.txtExcelFilePath);
            this.groupBox1.Controls.Add(this.rdAddTaskDtl);
            this.groupBox1.Controls.Add(this.rdClearTaskDtl);
            this.groupBox1.Controls.Add(this.cbTableNames);
            this.groupBox1.Controls.Add(this.btnLoadExcelData);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.btnBrownFile);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(3, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 454);
            this.groupBox1.TabIndex = 104;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Excel数据表";
            // 
            // rdAddTaskDtl
            // 
            this.rdAddTaskDtl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdAddTaskDtl.AutoSize = true;
            this.rdAddTaskDtl.Location = new System.Drawing.Point(191, 427);
            this.rdAddTaskDtl.Name = "rdAddTaskDtl";
            this.rdAddTaskDtl.Size = new System.Drawing.Size(95, 16);
            this.rdAddTaskDtl.TabIndex = 114;
            this.rdAddTaskDtl.Text = "追加成本明细";
            this.rdAddTaskDtl.UseVisualStyleBackColor = true;
            // 
            // rdClearTaskDtl
            // 
            this.rdClearTaskDtl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdClearTaskDtl.AutoSize = true;
            this.rdClearTaskDtl.Checked = true;
            this.rdClearTaskDtl.Location = new System.Drawing.Point(66, 427);
            this.rdClearTaskDtl.Name = "rdClearTaskDtl";
            this.rdClearTaskDtl.Size = new System.Drawing.Size(119, 16);
            this.rdClearTaskDtl.TabIndex = 114;
            this.rdClearTaskDtl.TabStop = true;
            this.rdClearTaskDtl.Text = "清空成本明细列表";
            this.rdClearTaskDtl.UseVisualStyleBackColor = true;
            // 
            // cbTableNames
            // 
            this.cbTableNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTableNames.BackColor = System.Drawing.Color.White;
            this.cbTableNames.FormattingEnabled = true;
            this.cbTableNames.ItemHeight = 12;
            this.cbTableNames.Location = new System.Drawing.Point(3, 92);
            this.cbTableNames.Name = "cbTableNames";
            this.cbTableNames.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.cbTableNames.Size = new System.Drawing.Size(443, 256);
            this.cbTableNames.TabIndex = 112;
            // 
            // btnLoadExcelData
            // 
            this.btnLoadExcelData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadExcelData.Location = new System.Drawing.Point(292, 424);
            this.btnLoadExcelData.Name = "btnLoadExcelData";
            this.btnLoadExcelData.Size = new System.Drawing.Size(132, 23);
            this.btnLoadExcelData.TabIndex = 50;
            this.btnLoadExcelData.Text = "加载选择表格数据>>";
            this.btnLoadExcelData.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.Color.Red;
            this.textBox1.Location = new System.Drawing.Point(9, 50);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(426, 36);
            this.textBox1.TabIndex = 116;
            this.textBox1.Text = "注：Excel文件应该包含以下列<定额编号>、<明细名称>、<计划工程量>、\r\n<成本项直接或间接父分类编码>；";
            // 
            // ckbProduceConfirmFlag
            // 
            this.ckbProduceConfirmFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckbProduceConfirmFlag.AutoSize = true;
            this.ckbProduceConfirmFlag.Checked = true;
            this.ckbProduceConfirmFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbProduceConfirmFlag.Location = new System.Drawing.Point(212, 376);
            this.ckbProduceConfirmFlag.Name = "ckbProduceConfirmFlag";
            this.ckbProduceConfirmFlag.Size = new System.Drawing.Size(96, 16);
            this.ckbProduceConfirmFlag.TabIndex = 120;
            this.ckbProduceConfirmFlag.Text = "生产确认明细";
            this.ckbProduceConfirmFlag.UseVisualStyleBackColor = true;
            // 
            // VImportGWBSDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 500);
            this.Name = "VImportGWBSDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导入成本数据";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.gbUsage.ResumeLayout(false);
            this.gbUsage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDtl)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbUsage;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridDtl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtExcelFilePath;
        private System.Windows.Forms.Button btnBrownFile;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.Button btnLookCostItemInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox cbTableNames;
        private System.Windows.Forms.Button btnLoadExcelData;
        private System.Windows.Forms.RadioButton rdAddTaskDtl;
        private System.Windows.Forms.RadioButton rdClearTaskDtl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuotaCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtlName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlanQuantity;
        private System.Windows.Forms.TextBox txtContractGroupName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectContractGroup;
        private System.Windows.Forms.TextBox txtContractGroupType;
        private System.Windows.Forms.Label label6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCurrentPath;
        private System.Windows.Forms.CheckBox cbCostAccountFlag;
        private System.Windows.Forms.CheckBox cbResponseAccountFlag;
        private System.Windows.Forms.Button btnRemoveSelectedRows;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox ckbProduceConfirmFlag;
    }
}