namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    partial class VDocCateCodeProfixMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDocCateCodeProfixMap));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSaveProfixMap = new System.Windows.Forms.Button();
            this.txtMBP_IRPCateCodeProfix = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtKBCateCodeProfix = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTempName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddObjTypeDefCate = new System.Windows.Forms.Button();
            this.btnDelObjTypeDefCateConfig = new System.Windows.Forms.Button();
            this.btnSaveObjTypeDefCateConfig = new System.Windows.Forms.Button();
            this.txtObjTypeAttDesc = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtObjTypeAttValue = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtObjTypeAttName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtObjTypeDesc = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtObjTypeName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnSearch = new System.Windows.Forms.Button();
            this.gridConfig = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colObjTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObjTypeDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObjTypeAttName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObjTypeAttDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObjTypeAttValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCateCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsDg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfig)).BeginInit();
            this.cmsDg.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(859, 526);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -13);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnSaveProfixMap);
            this.groupBox1.Controls.Add(this.txtMBP_IRPCateCodeProfix);
            this.groupBox1.Controls.Add(this.txtKBCateCodeProfix);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblTempName);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(853, 58);
            this.groupBox1.TabIndex = 193;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文档分类编码前缀映射配置";
            // 
            // btnSaveProfixMap
            // 
            this.btnSaveProfixMap.Location = new System.Drawing.Point(747, 21);
            this.btnSaveProfixMap.Name = "btnSaveProfixMap";
            this.btnSaveProfixMap.Size = new System.Drawing.Size(82, 23);
            this.btnSaveProfixMap.TabIndex = 194;
            this.btnSaveProfixMap.Text = "保存";
            this.btnSaveProfixMap.UseVisualStyleBackColor = true;
            // 
            // txtMBP_IRPCateCodeProfix
            // 
            this.txtMBP_IRPCateCodeProfix.BackColor = System.Drawing.SystemColors.Control;
            this.txtMBP_IRPCateCodeProfix.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMBP_IRPCateCodeProfix.DrawSelf = false;
            this.txtMBP_IRPCateCodeProfix.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMBP_IRPCateCodeProfix.EnterToTab = false;
            this.txtMBP_IRPCateCodeProfix.Location = new System.Drawing.Point(535, 25);
            this.txtMBP_IRPCateCodeProfix.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMBP_IRPCateCodeProfix.Name = "txtMBP_IRPCateCodeProfix";
            this.txtMBP_IRPCateCodeProfix.Padding = new System.Windows.Forms.Padding(1);
            this.txtMBP_IRPCateCodeProfix.ReadOnly = false;
            this.txtMBP_IRPCateCodeProfix.Size = new System.Drawing.Size(196, 16);
            this.txtMBP_IRPCateCodeProfix.TabIndex = 193;
            // 
            // txtKBCateCodeProfix
            // 
            this.txtKBCateCodeProfix.BackColor = System.Drawing.SystemColors.Control;
            this.txtKBCateCodeProfix.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtKBCateCodeProfix.DrawSelf = false;
            this.txtKBCateCodeProfix.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtKBCateCodeProfix.EnterToTab = false;
            this.txtKBCateCodeProfix.Location = new System.Drawing.Point(143, 25);
            this.txtKBCateCodeProfix.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtKBCateCodeProfix.Name = "txtKBCateCodeProfix";
            this.txtKBCateCodeProfix.Padding = new System.Windows.Forms.Padding(1);
            this.txtKBCateCodeProfix.ReadOnly = false;
            this.txtKBCateCodeProfix.Size = new System.Drawing.Size(196, 16);
            this.txtKBCateCodeProfix.TabIndex = 192;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(362, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 12);
            this.label1.TabIndex = 190;
            this.label1.Text = "对应项目管理IRP分类编码前缀：";
            // 
            // lblTempName
            // 
            this.lblTempName.AutoSize = true;
            this.lblTempName.Location = new System.Drawing.Point(24, 29);
            this.lblTempName.Name = "lblTempName";
            this.lblTempName.Size = new System.Drawing.Size(125, 12);
            this.lblTempName.TabIndex = 191;
            this.lblTempName.Text = "知识库分类编码前缀：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnAddObjTypeDefCate);
            this.groupBox2.Controls.Add(this.btnDelObjTypeDefCateConfig);
            this.groupBox2.Controls.Add(this.btnSaveObjTypeDefCateConfig);
            this.groupBox2.Controls.Add(this.txtObjTypeAttDesc);
            this.groupBox2.Controls.Add(this.txtObjTypeAttValue);
            this.groupBox2.Controls.Add(this.txtObjTypeAttName);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtObjTypeDesc);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtObjTypeName);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.gridConfig);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(3, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(853, 456);
            this.groupBox2.TabIndex = 194;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "对象类型关联文档分类配置";
            // 
            // btnAddObjTypeDefCate
            // 
            this.btnAddObjTypeDefCate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddObjTypeDefCate.Location = new System.Drawing.Point(275, 427);
            this.btnAddObjTypeDefCate.Name = "btnAddObjTypeDefCate";
            this.btnAddObjTypeDefCate.Size = new System.Drawing.Size(114, 23);
            this.btnAddObjTypeDefCate.TabIndex = 200;
            this.btnAddObjTypeDefCate.Text = "新增";
            this.btnAddObjTypeDefCate.UseVisualStyleBackColor = true;
            // 
            // btnDelObjTypeDefCateConfig
            // 
            this.btnDelObjTypeDefCateConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelObjTypeDefCateConfig.Location = new System.Drawing.Point(395, 427);
            this.btnDelObjTypeDefCateConfig.Name = "btnDelObjTypeDefCateConfig";
            this.btnDelObjTypeDefCateConfig.Size = new System.Drawing.Size(114, 23);
            this.btnDelObjTypeDefCateConfig.TabIndex = 200;
            this.btnDelObjTypeDefCateConfig.Text = "删除";
            this.btnDelObjTypeDefCateConfig.UseVisualStyleBackColor = true;
            // 
            // btnSaveObjTypeDefCateConfig
            // 
            this.btnSaveObjTypeDefCateConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveObjTypeDefCateConfig.Location = new System.Drawing.Point(550, 427);
            this.btnSaveObjTypeDefCateConfig.Name = "btnSaveObjTypeDefCateConfig";
            this.btnSaveObjTypeDefCateConfig.Size = new System.Drawing.Size(114, 23);
            this.btnSaveObjTypeDefCateConfig.TabIndex = 200;
            this.btnSaveObjTypeDefCateConfig.Text = "保存";
            this.btnSaveObjTypeDefCateConfig.UseVisualStyleBackColor = true;
            // 
            // txtObjTypeAttDesc
            // 
            this.txtObjTypeAttDesc.BackColor = System.Drawing.SystemColors.Control;
            this.txtObjTypeAttDesc.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtObjTypeAttDesc.DrawSelf = false;
            this.txtObjTypeAttDesc.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtObjTypeAttDesc.EnterToTab = false;
            this.txtObjTypeAttDesc.Location = new System.Drawing.Point(111, 51);
            this.txtObjTypeAttDesc.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtObjTypeAttDesc.Name = "txtObjTypeAttDesc";
            this.txtObjTypeAttDesc.Padding = new System.Windows.Forms.Padding(1);
            this.txtObjTypeAttDesc.ReadOnly = false;
            this.txtObjTypeAttDesc.Size = new System.Drawing.Size(159, 16);
            this.txtObjTypeAttDesc.TabIndex = 199;
            // 
            // txtObjTypeAttValue
            // 
            this.txtObjTypeAttValue.BackColor = System.Drawing.SystemColors.Control;
            this.txtObjTypeAttValue.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtObjTypeAttValue.DrawSelf = false;
            this.txtObjTypeAttValue.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtObjTypeAttValue.EnterToTab = false;
            this.txtObjTypeAttValue.Location = new System.Drawing.Point(372, 50);
            this.txtObjTypeAttValue.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtObjTypeAttValue.Name = "txtObjTypeAttValue";
            this.txtObjTypeAttValue.Padding = new System.Windows.Forms.Padding(1);
            this.txtObjTypeAttValue.ReadOnly = false;
            this.txtObjTypeAttValue.Size = new System.Drawing.Size(157, 16);
            this.txtObjTypeAttValue.TabIndex = 199;
            // 
            // txtObjTypeAttName
            // 
            this.txtObjTypeAttName.BackColor = System.Drawing.SystemColors.Control;
            this.txtObjTypeAttName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtObjTypeAttName.DrawSelf = false;
            this.txtObjTypeAttName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtObjTypeAttName.EnterToTab = false;
            this.txtObjTypeAttName.Location = new System.Drawing.Point(643, 23);
            this.txtObjTypeAttName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtObjTypeAttName.Name = "txtObjTypeAttName";
            this.txtObjTypeAttName.Padding = new System.Windows.Forms.Padding(1);
            this.txtObjTypeAttName.ReadOnly = false;
            this.txtObjTypeAttName.Size = new System.Drawing.Size(171, 16);
            this.txtObjTypeAttName.TabIndex = 199;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 198;
            this.label6.Text = "对象类型属性说明：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(276, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 198;
            this.label5.Text = "对象类型属性值：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(535, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 198;
            this.label3.Text = "对象类型属性名称：";
            // 
            // txtObjTypeDesc
            // 
            this.txtObjTypeDesc.BackColor = System.Drawing.SystemColors.Control;
            this.txtObjTypeDesc.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtObjTypeDesc.DrawSelf = false;
            this.txtObjTypeDesc.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtObjTypeDesc.EnterToTab = false;
            this.txtObjTypeDesc.Location = new System.Drawing.Point(372, 23);
            this.txtObjTypeDesc.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtObjTypeDesc.Name = "txtObjTypeDesc";
            this.txtObjTypeDesc.Padding = new System.Windows.Forms.Padding(1);
            this.txtObjTypeDesc.ReadOnly = false;
            this.txtObjTypeDesc.Size = new System.Drawing.Size(157, 16);
            this.txtObjTypeDesc.TabIndex = 197;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(288, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 196;
            this.label4.Text = "对象类型说明：";
            // 
            // txtObjTypeName
            // 
            this.txtObjTypeName.BackColor = System.Drawing.SystemColors.Control;
            this.txtObjTypeName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtObjTypeName.DrawSelf = false;
            this.txtObjTypeName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtObjTypeName.EnterToTab = false;
            this.txtObjTypeName.Location = new System.Drawing.Point(111, 23);
            this.txtObjTypeName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtObjTypeName.Name = "txtObjTypeName";
            this.txtObjTypeName.Padding = new System.Windows.Forms.Padding(1);
            this.txtObjTypeName.ReadOnly = false;
            this.txtObjTypeName.Size = new System.Drawing.Size(159, 16);
            this.txtObjTypeName.TabIndex = 195;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(643, 49);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(171, 23);
            this.btnSearch.TabIndex = 194;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // gridConfig
            // 
            this.gridConfig.AddDefaultMenu = false;
            this.gridConfig.AddNoColumn = true;
            this.gridConfig.AllowUserToAddRows = false;
            this.gridConfig.AllowUserToDeleteRows = false;
            this.gridConfig.AllowUserToResizeRows = false;
            this.gridConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridConfig.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridConfig.BackgroundColor = System.Drawing.Color.White;
            this.gridConfig.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridConfig.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridConfig.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridConfig.ColumnHeadersHeight = 24;
            this.gridConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridConfig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colObjTypeName,
            this.colObjTypeDesc,
            this.colObjTypeAttName,
            this.colObjTypeAttDesc,
            this.colObjTypeAttValue,
            this.colCateCode,
            this.colCateName});
            this.gridConfig.ContextMenuStrip = this.cmsDg;
            this.gridConfig.CustomBackColor = false;
            this.gridConfig.EditCellBackColor = System.Drawing.Color.White;
            this.gridConfig.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridConfig.EnableHeadersVisualStyles = false;
            this.gridConfig.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridConfig.FreezeFirstRow = false;
            this.gridConfig.FreezeLastRow = false;
            this.gridConfig.FrontColumnCount = 0;
            this.gridConfig.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridConfig.HScrollOffset = 0;
            this.gridConfig.IsAllowOrder = true;
            this.gridConfig.IsConfirmDelete = true;
            this.gridConfig.Location = new System.Drawing.Point(0, 78);
            this.gridConfig.Name = "gridConfig";
            this.gridConfig.PageIndex = 0;
            this.gridConfig.PageSize = 0;
            this.gridConfig.Query = null;
            this.gridConfig.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridConfig.ReadOnlyCols")));
            this.gridConfig.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridConfig.RowHeadersVisible = false;
            this.gridConfig.RowHeadersWidth = 22;
            this.gridConfig.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridConfig.RowTemplate.Height = 23;
            this.gridConfig.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridConfig.Size = new System.Drawing.Size(853, 343);
            this.gridConfig.TabIndex = 191;
            this.gridConfig.TargetType = null;
            this.gridConfig.VScrollOffset = 0;
            // 
            // colObjTypeName
            // 
            this.colObjTypeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colObjTypeName.FillWeight = 120F;
            this.colObjTypeName.HeaderText = "对象类型名称";
            this.colObjTypeName.Name = "colObjTypeName";
            // 
            // colObjTypeDesc
            // 
            this.colObjTypeDesc.HeaderText = "对象类型说明";
            this.colObjTypeDesc.Name = "colObjTypeDesc";
            this.colObjTypeDesc.Width = 101;
            // 
            // colObjTypeAttName
            // 
            this.colObjTypeAttName.HeaderText = "对象类型属性名称";
            this.colObjTypeAttName.Name = "colObjTypeAttName";
            this.colObjTypeAttName.Width = 125;
            // 
            // colObjTypeAttDesc
            // 
            this.colObjTypeAttDesc.HeaderText = "对象类型属性说明";
            this.colObjTypeAttDesc.Name = "colObjTypeAttDesc";
            this.colObjTypeAttDesc.Width = 125;
            // 
            // colObjTypeAttValue
            // 
            this.colObjTypeAttValue.HeaderText = "对象类型属性值";
            this.colObjTypeAttValue.Name = "colObjTypeAttValue";
            this.colObjTypeAttValue.Width = 113;
            // 
            // colCateCode
            // 
            this.colCateCode.HeaderText = "缺省使用的分类代码";
            this.colCateCode.Name = "colCateCode";
            this.colCateCode.Width = 137;
            // 
            // colCateName
            // 
            this.colCateName.HeaderText = "缺省使用的分类名称";
            this.colCateName.Name = "colCateName";
            this.colCateName.Width = 137;
            // 
            // cmsDg
            // 
            this.cmsDg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDel,
            this.tsmiCopy});
            this.cmsDg.Name = "cmsDg";
            this.cmsDg.Size = new System.Drawing.Size(101, 48);
            // 
            // tsmiDel
            // 
            this.tsmiDel.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.tsmiDel.Name = "tsmiDel";
            this.tsmiDel.Size = new System.Drawing.Size(100, 22);
            this.tsmiDel.Text = "删除";
            this.tsmiDel.ToolTipText = "删除当前选中的记录";
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.新增;
            this.tsmiCopy.Name = "tsmiCopy";
            this.tsmiCopy.Size = new System.Drawing.Size(100, 22);
            this.tsmiCopy.Text = "复制";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 191;
            this.label2.Text = "对象类型名称：";
            // 
            // VDocCateCodeProfixMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 526);
            this.Name = "VDocCateCodeProfixMap";
            this.Text = "项目文档使用分类配置";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridConfig)).EndInit();
            this.cmsDg.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSaveProfixMap;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMBP_IRPCateCodeProfix;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtKBCateCodeProfix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTempName;
        private System.Windows.Forms.GroupBox groupBox2;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridConfig;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtObjTypeAttValue;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtObjTypeAttName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtObjTypeDesc;
        private System.Windows.Forms.Label label4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtObjTypeName;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSaveObjTypeDefCateConfig;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtObjTypeAttDesc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ContextMenuStrip cmsDg;
        private System.Windows.Forms.ToolStripMenuItem tsmiDel;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private System.Windows.Forms.Button btnDelObjTypeDefCateConfig;
        private System.Windows.Forms.Button btnAddObjTypeDefCate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObjTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObjTypeDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObjTypeAttName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObjTypeAttDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObjTypeAttValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCateCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCateName;


    }
}