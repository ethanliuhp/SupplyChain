namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorDefine
{
    partial class VIndicatorDefine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VIndicatorDefine));
            this.lblCategoryAdd = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TVIndicatorCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dgvIndicator = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.IndicatorCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IndicatorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IndicatorRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripIndicatorDefinition = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemEditIndicator = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDeleteIndicator = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripIndicatorCategory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemAddNode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDeleteCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemRenameCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemAddIndicator = new System.Windows.Forms.ToolStripMenuItem();
            this.lblCategoryEdit = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.lblCategoryDelete = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.lblIndicatorDelete = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.lblIndicatorEdit = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.lblIndicatorAdd = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.customGroupBox1 = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.txtIndicatorCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtIndicatorName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnReset = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndicator)).BeginInit();
            this.contextMenuStripIndicatorDefinition.SuspendLayout();
            this.contextMenuStripIndicatorCategory.SuspendLayout();
            this.customGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.customGroupBox1);
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Size = new System.Drawing.Size(628, 437);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customGroupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // lblCategoryAdd
            // 
            this.lblCategoryAdd.AutoSize = true;
            this.lblCategoryAdd.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblCategoryAdd.Location = new System.Drawing.Point(73, 17);
            this.lblCategoryAdd.Name = "lblCategoryAdd";
            this.lblCategoryAdd.Size = new System.Drawing.Size(29, 12);
            this.lblCategoryAdd.TabIndex = 0;
            this.lblCategoryAdd.TabStop = true;
            this.lblCategoryAdd.Text = "新增";
            this.lblCategoryAdd.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblCategoryAdd_LinkClicked);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 76);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TVIndicatorCategory);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvIndicator);
            this.splitContainer1.Size = new System.Drawing.Size(622, 358);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 1;
            // 
            // TVIndicatorCategory
            // 
            this.TVIndicatorCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TVIndicatorCategory.HideSelection = false;
            this.TVIndicatorCategory.ImageIndex = 0;
            this.TVIndicatorCategory.ImageList = this.imageList1;
            this.TVIndicatorCategory.Location = new System.Drawing.Point(0, 0);
            this.TVIndicatorCategory.Name = "TVIndicatorCategory";
            this.TVIndicatorCategory.SelectedImageIndex = 1;
            this.TVIndicatorCategory.Size = new System.Drawing.Size(160, 358);
            this.TVIndicatorCategory.TabIndex = 0;
            this.TVIndicatorCategory.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TVIndicatorCategory_AfterLabelEdit);
            this.TVIndicatorCategory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TVIndicatorCategory_AfterSelect);
            this.TVIndicatorCategory.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TVIndicatorCategory_NodeMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "CLSDFOLD.ICO");
            this.imageList1.Images.SetKeyName(1, "OPENFOLD.ICO");
            // 
            // dgvIndicator
            // 
            this.dgvIndicator.AddDefaultMenu = false;
            this.dgvIndicator.AddNoColumn = false;
            this.dgvIndicator.AllowUserToAddRows = false;
            this.dgvIndicator.AllowUserToDeleteRows = false;
            this.dgvIndicator.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvIndicator.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvIndicator.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvIndicator.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvIndicator.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIndicator.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IndicatorCode,
            this.IndicatorName,
            this.Unit,
            this.IndicatorRemark});
            this.dgvIndicator.ContextMenuStrip = this.contextMenuStripIndicatorDefinition;
            this.dgvIndicator.CustomBackColor = false;
            this.dgvIndicator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIndicator.EditCellBackColor = System.Drawing.Color.White;
            this.dgvIndicator.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextRow;
            this.dgvIndicator.FreezeFirstRow = false;
            this.dgvIndicator.FreezeLastRow = false;
            this.dgvIndicator.FrontColumnCount = 0;
            this.dgvIndicator.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvIndicator.IsAllowOrder = true;
            this.dgvIndicator.IsConfirmDelete = true;
            this.dgvIndicator.Location = new System.Drawing.Point(0, 0);
            this.dgvIndicator.Name = "dgvIndicator";
            this.dgvIndicator.PageIndex = 0;
            this.dgvIndicator.PageSize = 0;
            this.dgvIndicator.Query = null;
            this.dgvIndicator.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvIndicator.ReadOnlyCols")));
            this.dgvIndicator.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvIndicator.RowHeadersWidth = 22;
            this.dgvIndicator.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvIndicator.RowTemplate.Height = 23;
            this.dgvIndicator.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIndicator.Size = new System.Drawing.Size(458, 358);
            this.dgvIndicator.TabIndex = 0;
            this.dgvIndicator.TargetType = null;
            this.dgvIndicator.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvIndicator_MouseDown);
            this.dgvIndicator.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvIndicator_CellDoubleClick);
            // 
            // IndicatorCode
            // 
            this.IndicatorCode.HeaderText = "指标编号";
            this.IndicatorCode.Name = "IndicatorCode";
            this.IndicatorCode.ReadOnly = true;
            // 
            // IndicatorName
            // 
            this.IndicatorName.HeaderText = "指标名称";
            this.IndicatorName.Name = "IndicatorName";
            this.IndicatorName.ReadOnly = true;
            // 
            // Unit
            // 
            this.Unit.HeaderText = "计量单位";
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            // 
            // IndicatorRemark
            // 
            this.IndicatorRemark.HeaderText = "备注";
            this.IndicatorRemark.Name = "IndicatorRemark";
            this.IndicatorRemark.ReadOnly = true;
            this.IndicatorRemark.Width = 280;
            // 
            // contextMenuStripIndicatorDefinition
            // 
            this.contextMenuStripIndicatorDefinition.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEditIndicator,
            this.toolStripSeparator2,
            this.menuItemDeleteIndicator});
            this.contextMenuStripIndicatorDefinition.Name = "contextMenuStripIndicatorDefinition";
            this.contextMenuStripIndicatorDefinition.Size = new System.Drawing.Size(95, 54);
            // 
            // menuItemEditIndicator
            // 
            this.menuItemEditIndicator.Name = "menuItemEditIndicator";
            this.menuItemEditIndicator.Size = new System.Drawing.Size(94, 22);
            this.menuItemEditIndicator.Text = "编辑";
            this.menuItemEditIndicator.Click += new System.EventHandler(this.menuItemEditIndicator_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(91, 6);
            // 
            // menuItemDeleteIndicator
            // 
            this.menuItemDeleteIndicator.Name = "menuItemDeleteIndicator";
            this.menuItemDeleteIndicator.Size = new System.Drawing.Size(94, 22);
            this.menuItemDeleteIndicator.Text = "删除";
            this.menuItemDeleteIndicator.Click += new System.EventHandler(this.menuItemDeleteIndicator_Click);
            // 
            // contextMenuStripIndicatorCategory
            // 
            this.contextMenuStripIndicatorCategory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAddNode,
            this.menuItemDeleteCategory,
            this.menuItemRenameCategory,
            this.toolStripSeparator1,
            this.menuItemAddIndicator});
            this.contextMenuStripIndicatorCategory.Name = "contextMenuStripIndicatorCategory";
            this.contextMenuStripIndicatorCategory.Size = new System.Drawing.Size(143, 98);
            // 
            // menuItemAddNode
            // 
            this.menuItemAddNode.Name = "menuItemAddNode";
            this.menuItemAddNode.Size = new System.Drawing.Size(142, 22);
            this.menuItemAddNode.Text = "添加下级类别";
            this.menuItemAddNode.Click += new System.EventHandler(this.menuItemAddNode_Click);
            // 
            // menuItemDeleteCategory
            // 
            this.menuItemDeleteCategory.Name = "menuItemDeleteCategory";
            this.menuItemDeleteCategory.Size = new System.Drawing.Size(142, 22);
            this.menuItemDeleteCategory.Text = "删除类别";
            this.menuItemDeleteCategory.Click += new System.EventHandler(this.menuItemDeleteCategory_Click);
            // 
            // menuItemRenameCategory
            // 
            this.menuItemRenameCategory.Name = "menuItemRenameCategory";
            this.menuItemRenameCategory.Size = new System.Drawing.Size(142, 22);
            this.menuItemRenameCategory.Text = "重命名";
            this.menuItemRenameCategory.Click += new System.EventHandler(this.menuItemRenameCagetory_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // menuItemAddIndicator
            // 
            this.menuItemAddIndicator.Name = "menuItemAddIndicator";
            this.menuItemAddIndicator.Size = new System.Drawing.Size(142, 22);
            this.menuItemAddIndicator.Text = "添加指标";
            this.menuItemAddIndicator.Click += new System.EventHandler(this.menuItemAddIndicator_Click);
            // 
            // lblCategoryEdit
            // 
            this.lblCategoryEdit.AutoSize = true;
            this.lblCategoryEdit.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblCategoryEdit.Location = new System.Drawing.Point(121, 17);
            this.lblCategoryEdit.Name = "lblCategoryEdit";
            this.lblCategoryEdit.Size = new System.Drawing.Size(29, 12);
            this.lblCategoryEdit.TabIndex = 2;
            this.lblCategoryEdit.TabStop = true;
            this.lblCategoryEdit.Text = "修改";
            this.lblCategoryEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblCategoryEdit_LinkClicked);
            // 
            // lblCategoryDelete
            // 
            this.lblCategoryDelete.AutoSize = true;
            this.lblCategoryDelete.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblCategoryDelete.Location = new System.Drawing.Point(170, 17);
            this.lblCategoryDelete.Name = "lblCategoryDelete";
            this.lblCategoryDelete.Size = new System.Drawing.Size(29, 12);
            this.lblCategoryDelete.TabIndex = 3;
            this.lblCategoryDelete.TabStop = true;
            this.lblCategoryDelete.Text = "删除";
            this.lblCategoryDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblCategoryDelete_LinkClicked);
            // 
            // lblIndicatorDelete
            // 
            this.lblIndicatorDelete.AutoSize = true;
            this.lblIndicatorDelete.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblIndicatorDelete.Location = new System.Drawing.Point(170, 38);
            this.lblIndicatorDelete.Name = "lblIndicatorDelete";
            this.lblIndicatorDelete.Size = new System.Drawing.Size(29, 12);
            this.lblIndicatorDelete.TabIndex = 6;
            this.lblIndicatorDelete.TabStop = true;
            this.lblIndicatorDelete.Text = "删除";
            this.lblIndicatorDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblIndicatorDelete_LinkClicked);
            // 
            // lblIndicatorEdit
            // 
            this.lblIndicatorEdit.AutoSize = true;
            this.lblIndicatorEdit.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblIndicatorEdit.Location = new System.Drawing.Point(121, 38);
            this.lblIndicatorEdit.Name = "lblIndicatorEdit";
            this.lblIndicatorEdit.Size = new System.Drawing.Size(29, 12);
            this.lblIndicatorEdit.TabIndex = 5;
            this.lblIndicatorEdit.TabStop = true;
            this.lblIndicatorEdit.Text = "修改";
            this.lblIndicatorEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblIndicatorEdit_LinkClicked);
            // 
            // lblIndicatorAdd
            // 
            this.lblIndicatorAdd.AutoSize = true;
            this.lblIndicatorAdd.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblIndicatorAdd.Location = new System.Drawing.Point(73, 38);
            this.lblIndicatorAdd.Name = "lblIndicatorAdd";
            this.lblIndicatorAdd.Size = new System.Drawing.Size(29, 12);
            this.lblIndicatorAdd.TabIndex = 4;
            this.lblIndicatorAdd.TabStop = true;
            this.lblIndicatorAdd.Text = "新增";
            this.lblIndicatorAdd.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblIndicatorAdd_LinkClicked);
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customGroupBox1.Controls.Add(this.txtIndicatorCode);
            this.customGroupBox1.Controls.Add(this.txtIndicatorName);
            this.customGroupBox1.Controls.Add(this.btnReset);
            this.customGroupBox1.Controls.Add(this.btnSearch);
            this.customGroupBox1.Controls.Add(this.customLabel4);
            this.customGroupBox1.Controls.Add(this.customLabel3);
            this.customGroupBox1.Controls.Add(this.customLabel2);
            this.customGroupBox1.Controls.Add(this.customLabel1);
            this.customGroupBox1.Controls.Add(this.lblIndicatorEdit);
            this.customGroupBox1.Controls.Add(this.lblCategoryDelete);
            this.customGroupBox1.Controls.Add(this.lblCategoryEdit);
            this.customGroupBox1.Controls.Add(this.lblIndicatorDelete);
            this.customGroupBox1.Controls.Add(this.lblIndicatorAdd);
            this.customGroupBox1.Controls.Add(this.lblCategoryAdd);
            this.customGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(622, 67);
            this.customGroupBox1.TabIndex = 7;
            this.customGroupBox1.TabStop = false;
            // 
            // txtIndicatorCode
            // 
            this.txtIndicatorCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtIndicatorCode.DrawSelf = false;
            this.txtIndicatorCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtIndicatorCode.EnterToTab = false;
            this.txtIndicatorCode.Location = new System.Drawing.Point(481, 13);
            this.txtIndicatorCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtIndicatorCode.Name = "txtIndicatorCode";
            this.txtIndicatorCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtIndicatorCode.ReadOnly = false;
            this.txtIndicatorCode.Size = new System.Drawing.Size(121, 16);
            this.txtIndicatorCode.TabIndex = 14;
            // 
            // txtIndicatorName
            // 
            this.txtIndicatorName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtIndicatorName.DrawSelf = false;
            this.txtIndicatorName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtIndicatorName.EnterToTab = false;
            this.txtIndicatorName.Location = new System.Drawing.Point(300, 13);
            this.txtIndicatorName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtIndicatorName.Name = "txtIndicatorName";
            this.txtIndicatorName.Padding = new System.Windows.Forms.Padding(1);
            this.txtIndicatorName.ReadOnly = false;
            this.txtIndicatorName.Size = new System.Drawing.Size(121, 16);
            this.txtIndicatorName.TabIndex = 13;
            // 
            // btnReset
            // 
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReset.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnReset.Location = new System.Drawing.Point(445, 38);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 12;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(355, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(434, 17);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(41, 12);
            this.customLabel4.TabIndex = 10;
            this.customLabel4.Text = "编号：";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(253, 17);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(41, 12);
            this.customLabel3.TabIndex = 9;
            this.customLabel3.Text = "名称：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(7, 38);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(65, 12);
            this.customLabel2.TabIndex = 8;
            this.customLabel2.Text = "指标操作：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(7, 17);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 7;
            this.customLabel1.Text = "分类操作：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VIndicatorDefine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 437);
            this.Name = "VIndicatorDefine";
            this.Text = "VIndicatorDefine";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndicator)).EndInit();
            this.contextMenuStripIndicatorDefinition.ResumeLayout(false);
            this.contextMenuStripIndicatorCategory.ResumeLayout(false);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lblCategoryAdd;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView TVIndicatorCategory;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvIndicator;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripIndicatorCategory;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddNode;
        private System.Windows.Forms.ToolStripMenuItem menuItemDeleteCategory;
        private System.Windows.Forms.ToolStripMenuItem menuItemRenameCategory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemAddIndicator;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripIndicatorDefinition;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditIndicator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuItemDeleteIndicator;
        private System.Windows.Forms.ImageList imageList1;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lblIndicatorDelete;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lblIndicatorEdit;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lblIndicatorAdd;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lblCategoryDelete;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lblCategoryEdit;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox customGroupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtIndicatorCode;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtIndicatorName;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnReset;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndicatorCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndicatorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndicatorRemark;
    }
}