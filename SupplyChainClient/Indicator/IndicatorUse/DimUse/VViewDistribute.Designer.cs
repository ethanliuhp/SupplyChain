namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    partial class VViewDistribute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VViewDistribute));
            this.gbxViewSelect = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.lstViewSel = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.cboCubeSelect = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.gboDistribute = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.btnDistributeOk = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnGwDel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnGwSelect = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dgvGwSelect = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.edtViewType = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.edtCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblViewType = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnAddDistribute = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.opeOrg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.job = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.distributedate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.gbxViewSelect.SuspendLayout();
            this.gboDistribute.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGwSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.gboDistribute);
            this.pnlFloor.Controls.Add(this.gbxViewSelect);
            this.pnlFloor.Dock = System.Windows.Forms.DockStyle.None;
            this.pnlFloor.Location = new System.Drawing.Point(1, 1);
            this.pnlFloor.Size = new System.Drawing.Size(743, 459);
            // 
            // gbxViewSelect
            // 
            this.gbxViewSelect.Controls.Add(this.lstViewSel);
            this.gbxViewSelect.Controls.Add(this.cboCubeSelect);
            this.gbxViewSelect.Location = new System.Drawing.Point(23, 17);
            this.gbxViewSelect.Name = "gbxViewSelect";
            this.gbxViewSelect.Size = new System.Drawing.Size(221, 435);
            this.gbxViewSelect.TabIndex = 0;
            this.gbxViewSelect.TabStop = false;
            this.gbxViewSelect.Text = ">>>选择分发模板";
            // 
            // lstViewSel
            // 
            this.lstViewSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstViewSel.FormattingEnabled = true;
            this.lstViewSel.ItemHeight = 12;
            this.lstViewSel.Location = new System.Drawing.Point(28, 68);
            this.lstViewSel.Name = "lstViewSel";
            this.lstViewSel.Size = new System.Drawing.Size(165, 326);
            this.lstViewSel.TabIndex = 1;
            this.lstViewSel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstViewSel_MouseClick);
            // 
            // cboCubeSelect
            // 
            this.cboCubeSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboCubeSelect.FormattingEnabled = true;
            this.cboCubeSelect.Location = new System.Drawing.Point(26, 30);
            this.cboCubeSelect.Name = "cboCubeSelect";
            this.cboCubeSelect.Size = new System.Drawing.Size(168, 20);
            this.cboCubeSelect.TabIndex = 0;
            this.cboCubeSelect.SelectedValueChanged += new System.EventHandler(this.cboCubeSelect_SelectedValueChanged);
            // 
            // gboDistribute
            // 
            this.gboDistribute.Controls.Add(this.btnAddDistribute);
            this.gboDistribute.Controls.Add(this.btnDistributeOk);
            this.gboDistribute.Controls.Add(this.btnGwDel);
            this.gboDistribute.Controls.Add(this.btnGwSelect);
            this.gboDistribute.Controls.Add(this.dgvGwSelect);
            this.gboDistribute.Controls.Add(this.edtViewType);
            this.gboDistribute.Controls.Add(this.edtCreateDate);
            this.gboDistribute.Controls.Add(this.lblViewType);
            this.gboDistribute.Controls.Add(this.lblCreateDate);
            this.gboDistribute.Location = new System.Drawing.Point(260, 17);
            this.gboDistribute.Name = "gboDistribute";
            this.gboDistribute.Size = new System.Drawing.Size(451, 443);
            this.gboDistribute.TabIndex = 1;
            this.gboDistribute.TabStop = false;
            this.gboDistribute.Text = ">>>模板分发配置";
            // 
            // btnDistributeOk
            // 
            this.btnDistributeOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDistributeOk.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDistributeOk.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnDistributeOk.Location = new System.Drawing.Point(339, 373);
            this.btnDistributeOk.Name = "btnDistributeOk";
            this.btnDistributeOk.Size = new System.Drawing.Size(75, 23);
            this.btnDistributeOk.TabIndex = 7;
            this.btnDistributeOk.Text = "分发提交";
            this.btnDistributeOk.UseVisualStyleBackColor = true;
            this.btnDistributeOk.Click += new System.EventHandler(this.btnDistributeOk_Click);
            // 
            // btnGwDel
            // 
            this.btnGwDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGwDel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGwDel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnGwDel.Location = new System.Drawing.Point(223, 373);
            this.btnGwDel.Name = "btnGwDel";
            this.btnGwDel.Size = new System.Drawing.Size(75, 23);
            this.btnGwDel.TabIndex = 6;
            this.btnGwDel.Text = "删除岗位";
            this.btnGwDel.UseVisualStyleBackColor = true;
            this.btnGwDel.Click += new System.EventHandler(this.btnGwDel_Click);
            // 
            // btnGwSelect
            // 
            this.btnGwSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGwSelect.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGwSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnGwSelect.Location = new System.Drawing.Point(141, 373);
            this.btnGwSelect.Name = "btnGwSelect";
            this.btnGwSelect.Size = new System.Drawing.Size(75, 23);
            this.btnGwSelect.TabIndex = 5;
            this.btnGwSelect.Text = "选择岗位";
            this.btnGwSelect.UseVisualStyleBackColor = true;
            this.btnGwSelect.Click += new System.EventHandler(this.btnGwSelect_Click);
            // 
            // dgvGwSelect
            // 
            this.dgvGwSelect.AddDefaultMenu = false;
            this.dgvGwSelect.AddNoColumn = false;
            this.dgvGwSelect.AllowUserToAddRows = false;
            this.dgvGwSelect.AllowUserToDeleteRows = false;
            this.dgvGwSelect.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvGwSelect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGwSelect.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvGwSelect.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvGwSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGwSelect.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.opeOrg,
            this.job,
            this.distributedate});
            this.dgvGwSelect.CustomBackColor = false;
            this.dgvGwSelect.EditCellBackColor = System.Drawing.Color.White;
            this.dgvGwSelect.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextRow;
            this.dgvGwSelect.FreezeFirstRow = false;
            this.dgvGwSelect.FreezeLastRow = false;
            this.dgvGwSelect.FrontColumnCount = 0;
            this.dgvGwSelect.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvGwSelect.IsAllowOrder = true;
            this.dgvGwSelect.IsConfirmDelete = true;
            this.dgvGwSelect.Location = new System.Drawing.Point(22, 68);
            this.dgvGwSelect.Name = "dgvGwSelect";
            this.dgvGwSelect.PageIndex = 0;
            this.dgvGwSelect.PageSize = 0;
            this.dgvGwSelect.Query = null;
            this.dgvGwSelect.ReadOnly = true;
            this.dgvGwSelect.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvGwSelect.ReadOnlyCols")));
            this.dgvGwSelect.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvGwSelect.RowHeadersWidth = 22;
            this.dgvGwSelect.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGwSelect.RowTemplate.Height = 23;
            this.dgvGwSelect.Size = new System.Drawing.Size(392, 268);
            this.dgvGwSelect.TabIndex = 4;
            this.dgvGwSelect.TargetType = null;
            // 
            // edtViewType
            // 
            this.edtViewType.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.edtViewType.DrawSelf = false;
            this.edtViewType.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.edtViewType.EnterToTab = false;
            this.edtViewType.Location = new System.Drawing.Point(292, 29);
            this.edtViewType.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.edtViewType.Name = "edtViewType";
            this.edtViewType.Padding = new System.Windows.Forms.Padding(1);
            this.edtViewType.ReadOnly = true;
            this.edtViewType.Size = new System.Drawing.Size(105, 16);
            this.edtViewType.TabIndex = 3;
            // 
            // edtCreateDate
            // 
            this.edtCreateDate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.edtCreateDate.DrawSelf = false;
            this.edtCreateDate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.edtCreateDate.EnterToTab = false;
            this.edtCreateDate.Location = new System.Drawing.Point(83, 29);
            this.edtCreateDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.edtCreateDate.Name = "edtCreateDate";
            this.edtCreateDate.Padding = new System.Windows.Forms.Padding(1);
            this.edtCreateDate.ReadOnly = true;
            this.edtCreateDate.Size = new System.Drawing.Size(133, 16);
            this.edtCreateDate.TabIndex = 2;
            // 
            // lblViewType
            // 
            this.lblViewType.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblViewType.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblViewType.Location = new System.Drawing.Point(245, 30);
            this.lblViewType.Name = "lblViewType";
            this.lblViewType.Size = new System.Drawing.Size(53, 20);
            this.lblViewType.TabIndex = 1;
            this.lblViewType.Text = "类型:";
            this.lblViewType.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblCreateDate
            // 
            this.lblCreateDate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCreateDate.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCreateDate.Location = new System.Drawing.Point(6, 30);
            this.lblCreateDate.Name = "lblCreateDate";
            this.lblCreateDate.Size = new System.Drawing.Size(71, 20);
            this.lblCreateDate.TabIndex = 0;
            this.lblCreateDate.Text = "创建日期:";
            this.lblCreateDate.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnAddDistribute
            // 
            this.btnAddDistribute.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAddDistribute.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddDistribute.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnAddDistribute.Location = new System.Drawing.Point(22, 373);
            this.btnAddDistribute.Name = "btnAddDistribute";
            this.btnAddDistribute.Size = new System.Drawing.Size(75, 23);
            this.btnAddDistribute.TabIndex = 8;
            this.btnAddDistribute.Text = "新建分发";
            this.btnAddDistribute.UseVisualStyleBackColor = true;
            this.btnAddDistribute.Click += new System.EventHandler(this.btnAddDistribute_Click);
            // 
            // opeOrg
            // 
            this.opeOrg.HeaderText = "业务部门";
            this.opeOrg.Name = "opeOrg";
            this.opeOrg.ReadOnly = true;
            this.opeOrg.Width = 160;
            // 
            // job
            // 
            this.job.HeaderText = "岗位";
            this.job.Name = "job";
            this.job.ReadOnly = true;
            this.job.Width = 110;
            // 
            // distributedate
            // 
            this.distributedate.HeaderText = "分发日期";
            this.distributedate.Name = "distributedate";
            // 
            // VViewDistribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 462);
            this.Name = "VViewDistribute";
            this.Text = "模板分发界面";
            this.pnlFloor.ResumeLayout(false);
            this.gbxViewSelect.ResumeLayout(false);
            this.gboDistribute.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGwSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gbxViewSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboCubeSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gboDistribute;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblViewType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit edtCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit edtViewType;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvGwSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnGwSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnDistributeOk;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnGwDel;
        private Application.Business.Erp.ClientSystem.Template.CustomListBox lstViewSel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnAddDistribute;
        private System.Windows.Forms.DataGridViewTextBoxColumn opeOrg;
        private System.Windows.Forms.DataGridViewTextBoxColumn job;
        private System.Windows.Forms.DataGridViewTextBoxColumn distributedate;
    }
}