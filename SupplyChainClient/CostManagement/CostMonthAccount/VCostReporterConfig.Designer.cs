namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    partial class VCostReporterConfig
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPgCategory = new System.Windows.Forms.TabPage();
            this.dtgdCategoryList = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCategoryCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoryType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPgCheck = new System.Windows.Forms.TabPage();
            this.dtgdCheckList = new System.Windows.Forms.DataGridView();
            this.colChkSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colChkCategoryCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkOrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkCategoryType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCopyConfig = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPgCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgdCategoryList)).BeginInit();
            this.tabPgCheck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgdCheckList)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPgCategory);
            this.tabControl1.Controls.Add(this.tabPgCheck);
            this.tabControl1.Location = new System.Drawing.Point(-2, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(833, 388);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPgCategory
            // 
            this.tabPgCategory.Controls.Add(this.dtgdCategoryList);
            this.tabPgCategory.Location = new System.Drawing.Point(4, 21);
            this.tabPgCategory.Name = "tabPgCategory";
            this.tabPgCategory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPgCategory.Size = new System.Drawing.Size(825, 363);
            this.tabPgCategory.TabIndex = 0;
            this.tabPgCategory.Text = "资源分类";
            this.tabPgCategory.UseVisualStyleBackColor = true;
            // 
            // dtgdCategoryList
            // 
            this.dtgdCategoryList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgdCategoryList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colCategoryCode,
            this.colCategoryName,
            this.colProjectName,
            this.colOrderNo,
            this.colDisplayName,
            this.colPath,
            this.colCategoryType});
            this.dtgdCategoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgdCategoryList.Location = new System.Drawing.Point(3, 3);
            this.dtgdCategoryList.Name = "dtgdCategoryList";
            this.dtgdCategoryList.RowTemplate.Height = 23;
            this.dtgdCategoryList.Size = new System.Drawing.Size(819, 357);
            this.dtgdCategoryList.TabIndex = 3;
            // 
            // colSelect
            // 
            this.colSelect.FalseValue = "0";
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.TrueValue = "1";
            this.colSelect.Width = 40;
            // 
            // colCategoryCode
            // 
            this.colCategoryCode.HeaderText = "物资编码";
            this.colCategoryCode.Name = "colCategoryCode";
            this.colCategoryCode.ReadOnly = true;
            // 
            // colCategoryName
            // 
            this.colCategoryName.HeaderText = "物资名称";
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.ReadOnly = true;
            // 
            // colProjectName
            // 
            this.colProjectName.HeaderText = "项目名称";
            this.colProjectName.Name = "colProjectName";
            this.colProjectName.ReadOnly = true;
            this.colProjectName.Visible = false;
            // 
            // colOrderNo
            // 
            this.colOrderNo.HeaderText = "排列顺序";
            this.colOrderNo.Name = "colOrderNo";
            this.colOrderNo.Width = 80;
            // 
            // colDisplayName
            // 
            this.colDisplayName.HeaderText = "显示名称";
            this.colDisplayName.Name = "colDisplayName";
            this.colDisplayName.Width = 150;
            // 
            // colPath
            // 
            this.colPath.HeaderText = "物资路径";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 300;
            // 
            // colCategoryType
            // 
            this.colCategoryType.HeaderText = "类型";
            this.colCategoryType.Name = "colCategoryType";
            this.colCategoryType.Visible = false;
            // 
            // tabPgCheck
            // 
            this.tabPgCheck.Controls.Add(this.dtgdCheckList);
            this.tabPgCheck.Location = new System.Drawing.Point(4, 21);
            this.tabPgCheck.Name = "tabPgCheck";
            this.tabPgCheck.Padding = new System.Windows.Forms.Padding(3);
            this.tabPgCheck.Size = new System.Drawing.Size(825, 363);
            this.tabPgCheck.TabIndex = 1;
            this.tabPgCheck.Text = "核算科目";
            this.tabPgCheck.UseVisualStyleBackColor = true;
            // 
            // dtgdCheckList
            // 
            this.dtgdCheckList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgdCheckList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChkSelect,
            this.colChkCategoryCode,
            this.colChkCategoryName,
            this.colChkProjectName,
            this.colChkDisplayName,
            this.colChkOrderNo,
            this.colChkPath,
            this.colChkCategoryType});
            this.dtgdCheckList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgdCheckList.Location = new System.Drawing.Point(3, 3);
            this.dtgdCheckList.Name = "dtgdCheckList";
            this.dtgdCheckList.RowTemplate.Height = 23;
            this.dtgdCheckList.Size = new System.Drawing.Size(819, 357);
            this.dtgdCheckList.TabIndex = 4;
            // 
            // colChkSelect
            // 
            this.colChkSelect.FalseValue = "0";
            this.colChkSelect.HeaderText = "选择";
            this.colChkSelect.Name = "colChkSelect";
            this.colChkSelect.TrueValue = "1";
            this.colChkSelect.Width = 40;
            // 
            // colChkCategoryCode
            // 
            this.colChkCategoryCode.HeaderText = "科目编码";
            this.colChkCategoryCode.Name = "colChkCategoryCode";
            this.colChkCategoryCode.ReadOnly = true;
            // 
            // colChkCategoryName
            // 
            this.colChkCategoryName.HeaderText = "科目名称";
            this.colChkCategoryName.Name = "colChkCategoryName";
            this.colChkCategoryName.ReadOnly = true;
            // 
            // colChkProjectName
            // 
            this.colChkProjectName.HeaderText = "项目名称";
            this.colChkProjectName.Name = "colChkProjectName";
            this.colChkProjectName.ReadOnly = true;
            this.colChkProjectName.Visible = false;
            // 
            // colChkDisplayName
            // 
            this.colChkDisplayName.HeaderText = "显示名称";
            this.colChkDisplayName.Name = "colChkDisplayName";
            this.colChkDisplayName.Width = 150;
            // 
            // colChkOrderNo
            // 
            this.colChkOrderNo.HeaderText = "排列顺序";
            this.colChkOrderNo.Name = "colChkOrderNo";
            this.colChkOrderNo.Width = 80;
            // 
            // colChkPath
            // 
            this.colChkPath.HeaderText = "科目路径";
            this.colChkPath.Name = "colChkPath";
            this.colChkPath.ReadOnly = true;
            this.colChkPath.Width = 300;
            // 
            // colChkCategoryType
            // 
            this.colChkCategoryType.HeaderText = "类型";
            this.colChkCategoryType.Name = "colChkCategoryType";
            this.colChkCategoryType.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(212, 400);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(118, 400);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(584, 400);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCopyConfig
            // 
            this.btnCopyConfig.Location = new System.Drawing.Point(451, 400);
            this.btnCopyConfig.Name = "btnCopyConfig";
            this.btnCopyConfig.Size = new System.Drawing.Size(75, 23);
            this.btnCopyConfig.TabIndex = 4;
            this.btnCopyConfig.Text = "复制配置";
            this.btnCopyConfig.UseVisualStyleBackColor = true;
            // 
            // VCostReporterConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 442);
            this.Controls.Add(this.btnCopyConfig);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl1);
            this.Name = "VCostReporterConfig";
            this.Text = "月度成本报表配置页面";
            this.tabControl1.ResumeLayout(false);
            this.tabPgCategory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgdCategoryList)).EndInit();
            this.tabPgCheck.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgdCheckList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPgCategory;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dtgdCategoryList;
        //private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.TabPage tabPgCheck;
        private System.Windows.Forms.DataGridView dtgdCheckList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrderNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChkSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkCategoryCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkCategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkDisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkOrderNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkCategoryType;
        private System.Windows.Forms.Button btnCopyConfig;
       // private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dtgdCategoryList;
    }
}