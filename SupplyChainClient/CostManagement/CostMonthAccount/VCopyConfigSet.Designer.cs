namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    partial class VCopyConfigSet
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dtGdProject = new System.Windows.Forms.DataGridView();
            this.colProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPgCategory = new System.Windows.Forms.TabPage();
            this.dtgdCategoryList = new System.Windows.Forms.DataGridView();
            this.colCategoryCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPgCheck = new System.Windows.Forms.TabPage();
            this.dtgdCheckList = new System.Windows.Forms.DataGridView();
            this.colChkCategoryCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChkPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dtGdProject)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPgCategory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgdCategoryList)).BeginInit();
            this.tabPgCheck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgdCheckList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "项目名称";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Location = new System.Drawing.Point(71, 6);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(100, 21);
            this.txtProjectName.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(177, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(761, 353);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 5;
            this.btnCopy.Text = "复制";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(566, 353);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // dtGdProject
            // 
            this.dtGdProject.AllowUserToAddRows = false;
            this.dtGdProject.AllowUserToDeleteRows = false;
            this.dtGdProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGdProject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProjectName,
            this.colProjectID});
            this.dtGdProject.Location = new System.Drawing.Point(-1, 33);
            this.dtGdProject.Name = "dtGdProject";
            this.dtGdProject.RowTemplate.Height = 23;
            this.dtGdProject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtGdProject.Size = new System.Drawing.Size(154, 314);
            this.dtGdProject.TabIndex = 7;
            // 
            // colProjectName
            // 
            this.colProjectName.HeaderText = "项目名称";
            this.colProjectName.Name = "colProjectName";
            this.colProjectName.ReadOnly = true;
            // 
            // colProjectID
            // 
            this.colProjectID.HeaderText = "项目ID";
            this.colProjectID.Name = "colProjectID";
            this.colProjectID.ReadOnly = true;
            this.colProjectID.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPgCategory);
            this.tabControl1.Controls.Add(this.tabPgCheck);
            this.tabControl1.Location = new System.Drawing.Point(159, 35);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(701, 312);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPgCategory
            // 
            this.tabPgCategory.Controls.Add(this.dtgdCategoryList);
            this.tabPgCategory.Location = new System.Drawing.Point(4, 21);
            this.tabPgCategory.Name = "tabPgCategory";
            this.tabPgCategory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPgCategory.Size = new System.Drawing.Size(693, 287);
            this.tabPgCategory.TabIndex = 0;
            this.tabPgCategory.Text = "资源分类";
            this.tabPgCategory.UseVisualStyleBackColor = true;
            // 
            // dtgdCategoryList
            // 
            this.dtgdCategoryList.AllowUserToAddRows = false;
            this.dtgdCategoryList.AllowUserToDeleteRows = false;
            this.dtgdCategoryList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgdCategoryList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCategoryCode,
            this.colCategoryName,
            this.colPath});
            this.dtgdCategoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgdCategoryList.Location = new System.Drawing.Point(3, 3);
            this.dtgdCategoryList.Name = "dtgdCategoryList";
            this.dtgdCategoryList.RowTemplate.Height = 23;
            this.dtgdCategoryList.Size = new System.Drawing.Size(687, 281);
            this.dtgdCategoryList.TabIndex = 3;
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
            // colPath
            // 
            this.colPath.HeaderText = "物资路径";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            this.colPath.Width = 300;
            // 
            // tabPgCheck
            // 
            this.tabPgCheck.Controls.Add(this.dtgdCheckList);
            this.tabPgCheck.Location = new System.Drawing.Point(4, 21);
            this.tabPgCheck.Name = "tabPgCheck";
            this.tabPgCheck.Padding = new System.Windows.Forms.Padding(3);
            this.tabPgCheck.Size = new System.Drawing.Size(693, 287);
            this.tabPgCheck.TabIndex = 1;
            this.tabPgCheck.Text = "核算科目";
            this.tabPgCheck.UseVisualStyleBackColor = true;
            // 
            // dtgdCheckList
            // 
            this.dtgdCheckList.AllowUserToAddRows = false;
            this.dtgdCheckList.AllowUserToDeleteRows = false;
            this.dtgdCheckList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgdCheckList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChkCategoryCode,
            this.colChkCategoryName,
            this.colChkPath});
            this.dtgdCheckList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgdCheckList.Location = new System.Drawing.Point(3, 3);
            this.dtgdCheckList.Name = "dtgdCheckList";
            this.dtgdCheckList.RowTemplate.Height = 23;
            this.dtgdCheckList.Size = new System.Drawing.Size(687, 281);
            this.dtgdCheckList.TabIndex = 4;
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
            // colChkPath
            // 
            this.colChkPath.HeaderText = "科目路径";
            this.colChkPath.Name = "colChkPath";
            this.colChkPath.ReadOnly = true;
            this.colChkPath.Width = 300;
            // 
            // VCopyConfigSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 385);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.dtGdProject);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtProjectName);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VCopyConfigSet";
            this.Text = "复制配置";
            ((System.ComponentModel.ISupportInitialize)(this.dtGdProject)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPgCategory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgdCategoryList)).EndInit();
            this.tabPgCheck.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgdCheckList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dtGdProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectID;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPgCategory;
        private System.Windows.Forms.DataGridView dtgdCategoryList;
        private System.Windows.Forms.TabPage tabPgCheck;
        private System.Windows.Forms.DataGridView dtgdCheckList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkCategoryCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkCategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChkPath;
    }
}