namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse
{
    partial class VViewDefFree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VViewDefFree));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnDel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnAdd = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.gbxViewDefine = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.cboMType = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cboYwzz = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnStyle = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.cboCollectType = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.lblCollectType = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnPreview = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnModify = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dgvDim = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRCSelect = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDim = new System.Windows.Forms.DataGridViewLinkColumn();
            this.btnSave = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtViewName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblViewName = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.gbxViewSelect = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.lstViewSel = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.cboCube = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.pnlFloor.SuspendLayout();
            this.gbxViewDefine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDim)).BeginInit();
            this.gbxViewSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.gbxViewDefine);
            this.pnlFloor.Controls.Add(this.gbxViewSelect);
            this.pnlFloor.Size = new System.Drawing.Size(805, 451);
            this.pnlFloor.Controls.SetChildIndex(this.gbxViewSelect, 0);
            this.pnlFloor.Controls.SetChildIndex(this.gbxViewDefine, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            this.lblTitle.Visible = false;
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // btnDel
            // 
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnDel.Location = new System.Drawing.Point(172, 382);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 7;
            this.btnDel.Text = "删 除";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnAdd.Location = new System.Drawing.Point(10, 382);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "新 增";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // gbxViewDefine
            // 
            this.gbxViewDefine.Controls.Add(this.cboMType);
            this.gbxViewDefine.Controls.Add(this.customLabel2);
            this.gbxViewDefine.Controls.Add(this.cboYwzz);
            this.gbxViewDefine.Controls.Add(this.customLabel1);
            this.gbxViewDefine.Controls.Add(this.btnStyle);
            this.gbxViewDefine.Controls.Add(this.cboCollectType);
            this.gbxViewDefine.Controls.Add(this.lblCollectType);
            this.gbxViewDefine.Controls.Add(this.btnPreview);
            this.gbxViewDefine.Controls.Add(this.btnModify);
            this.gbxViewDefine.Controls.Add(this.dgvDim);
            this.gbxViewDefine.Controls.Add(this.btnDel);
            this.gbxViewDefine.Controls.Add(this.btnSave);
            this.gbxViewDefine.Controls.Add(this.btnAdd);
            this.gbxViewDefine.Controls.Add(this.txtViewName);
            this.gbxViewDefine.Controls.Add(this.lblViewName);
            this.gbxViewDefine.Location = new System.Drawing.Point(258, 12);
            this.gbxViewDefine.Name = "gbxViewDefine";
            this.gbxViewDefine.Size = new System.Drawing.Size(539, 427);
            this.gbxViewDefine.TabIndex = 5;
            this.gbxViewDefine.TabStop = false;
            this.gbxViewDefine.Text = ">>>定义模板属性";
            // 
            // cboMType
            // 
            this.cboMType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboMType.FormattingEnabled = true;
            this.cboMType.Location = new System.Drawing.Point(415, 28);
            this.cboMType.Name = "cboMType";
            this.cboMType.Size = new System.Drawing.Size(106, 20);
            this.cboMType.TabIndex = 18;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(344, 34);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(65, 12);
            this.customLabel2.TabIndex = 17;
            this.customLabel2.Text = "模板类别：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cboYwzz
            // 
            this.cboYwzz.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboYwzz.FormattingEnabled = true;
            this.cboYwzz.Location = new System.Drawing.Point(415, 62);
            this.cboYwzz.Name = "cboYwzz";
            this.cboYwzz.Size = new System.Drawing.Size(106, 20);
            this.cboYwzz.TabIndex = 16;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(276, 65);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(113, 12);
            this.customLabel1.TabIndex = 15;
            this.customLabel1.Text = "是否显示业务组织：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnStyle
            // 
            this.btnStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStyle.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStyle.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnStyle.Location = new System.Drawing.Point(415, 382);
            this.btnStyle.Name = "btnStyle";
            this.btnStyle.Size = new System.Drawing.Size(75, 23);
            this.btnStyle.TabIndex = 14;
            this.btnStyle.Text = "格式定义";
            this.btnStyle.UseVisualStyleBackColor = true;
            // 
            // cboCollectType
            // 
            this.cboCollectType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboCollectType.FormattingEnabled = true;
            this.cboCollectType.Location = new System.Drawing.Point(106, 62);
            this.cboCollectType.Name = "cboCollectType";
            this.cboCollectType.Size = new System.Drawing.Size(113, 20);
            this.cboCollectType.TabIndex = 11;
            // 
            // lblCollectType
            // 
            this.lblCollectType.AutoSize = true;
            this.lblCollectType.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCollectType.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCollectType.Location = new System.Drawing.Point(11, 65);
            this.lblCollectType.Name = "lblCollectType";
            this.lblCollectType.Size = new System.Drawing.Size(89, 12);
            this.lblCollectType.TabIndex = 10;
            this.lblCollectType.Text = "模板采集频率：";
            this.lblCollectType.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnPreview
            // 
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPreview.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnPreview.Location = new System.Drawing.Point(334, 382);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 9;
            this.btnPreview.Text = "预 览";
            this.btnPreview.UseVisualStyleBackColor = true;
            // 
            // btnModify
            // 
            this.btnModify.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnModify.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnModify.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnModify.Location = new System.Drawing.Point(91, 382);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 8;
            this.btnModify.Text = "修 改";
            this.btnModify.UseVisualStyleBackColor = true;
            // 
            // dgvDim
            // 
            this.dgvDim.AddDefaultMenu = false;
            this.dgvDim.AddNoColumn = false;
            this.dgvDim.AllowUserToAddRows = false;
            this.dgvDim.AllowUserToDeleteRows = false;
            this.dgvDim.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvDim.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDim.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvDim.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvDim.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDim.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colRCSelect,
            this.colOrder,
            this.colDim});
            this.dgvDim.CustomBackColor = false;
            this.dgvDim.EditCellBackColor = System.Drawing.Color.White;
            this.dgvDim.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextRow;
            this.dgvDim.FreezeFirstRow = false;
            this.dgvDim.FreezeLastRow = false;
            this.dgvDim.FrontColumnCount = 0;
            this.dgvDim.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvDim.IsAllowOrder = false;
            this.dgvDim.IsConfirmDelete = true;
            this.dgvDim.Location = new System.Drawing.Point(24, 101);
            this.dgvDim.Name = "dgvDim";
            this.dgvDim.PageIndex = 0;
            this.dgvDim.PageSize = 0;
            this.dgvDim.Query = null;
            this.dgvDim.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvDim.ReadOnlyCols")));
            this.dgvDim.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvDim.RowHeadersWidth = 22;
            this.dgvDim.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvDim.RowTemplate.Height = 23;
            this.dgvDim.Size = new System.Drawing.Size(421, 275);
            this.dgvDim.TabIndex = 7;
            this.dgvDim.TargetType = null;
            // 
            // colSelect
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.NullValue = false;
            this.colSelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.ReadOnly = true;
            this.colSelect.Visible = false;
            this.colSelect.Width = 60;
            // 
            // colRCSelect
            // 
            this.colRCSelect.HeaderText = "行列选择";
            this.colRCSelect.Items.AddRange(new object[] {
            "行",
            "列"});
            this.colRCSelect.Name = "colRCSelect";
            this.colRCSelect.ReadOnly = true;
            // 
            // colOrder
            // 
            this.colOrder.HeaderText = "排列顺序";
            this.colOrder.Name = "colOrder";
            this.colOrder.ReadOnly = true;
            this.colOrder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colDim
            // 
            this.colDim.HeaderText = "维度";
            this.colDim.Name = "colDim";
            this.colDim.Width = 180;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSave.Location = new System.Drawing.Point(253, 382);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保 存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // txtViewName
            // 
            this.txtViewName.BackColor = System.Drawing.SystemColors.Control;
            this.txtViewName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtViewName.DrawSelf = false;
            this.txtViewName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtViewName.EnterToTab = false;
            this.txtViewName.Location = new System.Drawing.Point(80, 32);
            this.txtViewName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtViewName.Name = "txtViewName";
            this.txtViewName.Padding = new System.Windows.Forms.Padding(1);
            this.txtViewName.ReadOnly = false;
            this.txtViewName.Size = new System.Drawing.Size(248, 16);
            this.txtViewName.TabIndex = 1;
            // 
            // lblViewName
            // 
            this.lblViewName.AutoSize = true;
            this.lblViewName.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblViewName.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblViewName.Location = new System.Drawing.Point(11, 32);
            this.lblViewName.Name = "lblViewName";
            this.lblViewName.Size = new System.Drawing.Size(65, 12);
            this.lblViewName.TabIndex = 0;
            this.lblViewName.Text = "模板名称：";
            this.lblViewName.UnderLineColor = System.Drawing.Color.Red;
            // 
            // gbxViewSelect
            // 
            this.gbxViewSelect.Controls.Add(this.lstViewSel);
            this.gbxViewSelect.Controls.Add(this.cboCube);
            this.gbxViewSelect.Location = new System.Drawing.Point(12, 12);
            this.gbxViewSelect.Name = "gbxViewSelect";
            this.gbxViewSelect.Size = new System.Drawing.Size(226, 427);
            this.gbxViewSelect.TabIndex = 4;
            this.gbxViewSelect.TabStop = false;
            this.gbxViewSelect.Text = ">>>选择模板";
            // 
            // lstViewSel
            // 
            this.lstViewSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstViewSel.FormattingEnabled = true;
            this.lstViewSel.HorizontalScrollbar = true;
            this.lstViewSel.ItemHeight = 12;
            this.lstViewSel.Location = new System.Drawing.Point(17, 72);
            this.lstViewSel.Name = "lstViewSel";
            this.lstViewSel.Size = new System.Drawing.Size(192, 338);
            this.lstViewSel.TabIndex = 1;
            // 
            // cboCube
            // 
            this.cboCube.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboCube.FormattingEnabled = true;
            this.cboCube.Location = new System.Drawing.Point(17, 31);
            this.cboCube.Name = "cboCube";
            this.cboCube.Size = new System.Drawing.Size(192, 20);
            this.cboCube.TabIndex = 0;
            // 
            // VViewDefFree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 451);
            this.Name = "VViewDefFree";
            this.Text = "通用报表定义界面";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.gbxViewDefine.ResumeLayout(false);
            this.gbxViewDefine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDim)).EndInit();
            this.gbxViewSelect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomButton btnDel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnAdd;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gbxViewDefine;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvDim;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSave;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtViewName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblViewName;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gbxViewSelect;
        private Application.Business.Erp.ClientSystem.Template.CustomListBox lstViewSel;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboCube;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnModify;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnPreview;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboCollectType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCollectType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewComboBoxColumn colRCSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrder;
        private System.Windows.Forms.DataGridViewLinkColumn colDim;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnStyle;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboYwzz;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboMType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;

    }
}