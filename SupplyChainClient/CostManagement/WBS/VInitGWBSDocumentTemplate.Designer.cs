namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    partial class VInitGWBSDocumentTemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VInitGWBSDocumentTemplate));
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblTempName = new System.Windows.Forms.Label();
            this.gridProject = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnInitialWBSDocTemplate = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProject)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnInitialWBSDocTemplate);
            this.pnlFloor.Controls.Add(this.gridProject);
            this.pnlFloor.Controls.Add(this.btnSearch);
            this.pnlFloor.Controls.Add(this.txtName);
            this.pnlFloor.Controls.Add(this.lblTempName);
            this.pnlFloor.Size = new System.Drawing.Size(859, 507);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTempName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSearch, 0);
            this.pnlFloor.Controls.SetChildIndex(this.gridProject, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnInitialWBSDocTemplate, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -13);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(274, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 189;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.Control;
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = false;
            this.txtName.Location = new System.Drawing.Point(72, 14);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = false;
            this.txtName.Size = new System.Drawing.Size(196, 16);
            this.txtName.TabIndex = 188;
            // 
            // lblTempName
            // 
            this.lblTempName.AutoSize = true;
            this.lblTempName.Location = new System.Drawing.Point(12, 17);
            this.lblTempName.Name = "lblTempName";
            this.lblTempName.Size = new System.Drawing.Size(65, 12);
            this.lblTempName.TabIndex = 187;
            this.lblTempName.Text = "项目名称：";
            // 
            // gridProject
            // 
            this.gridProject.AddDefaultMenu = false;
            this.gridProject.AddNoColumn = false;
            this.gridProject.AllowUserToAddRows = false;
            this.gridProject.AllowUserToDeleteRows = false;
            this.gridProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridProject.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridProject.BackgroundColor = System.Drawing.Color.White;
            this.gridProject.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridProject.CellBackColor = System.Drawing.SystemColors.Control;
            this.gridProject.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridProject.ColumnHeadersHeight = 24;
            this.gridProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridProject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colProjectName});
            this.gridProject.CustomBackColor = false;
            this.gridProject.EditCellBackColor = System.Drawing.Color.White;
            this.gridProject.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridProject.EnableHeadersVisualStyles = false;
            this.gridProject.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.gridProject.FreezeFirstRow = false;
            this.gridProject.FreezeLastRow = false;
            this.gridProject.FrontColumnCount = 0;
            this.gridProject.GridColor = System.Drawing.SystemColors.WindowText;
            this.gridProject.HScrollOffset = 0;
            this.gridProject.IsAllowOrder = true;
            this.gridProject.IsConfirmDelete = true;
            this.gridProject.Location = new System.Drawing.Point(12, 41);
            this.gridProject.Name = "gridProject";
            this.gridProject.PageIndex = 0;
            this.gridProject.PageSize = 0;
            this.gridProject.Query = null;
            this.gridProject.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("gridProject.ReadOnlyCols")));
            this.gridProject.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridProject.RowHeadersVisible = false;
            this.gridProject.RowHeadersWidth = 22;
            this.gridProject.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridProject.RowTemplate.Height = 23;
            this.gridProject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridProject.Size = new System.Drawing.Size(835, 415);
            this.gridProject.TabIndex = 190;
            this.gridProject.TargetType = null;
            this.gridProject.VScrollOffset = 0;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 53;
            // 
            // colProjectName
            // 
            this.colProjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colProjectName.FillWeight = 120F;
            this.colProjectName.HeaderText = "项目名称";
            this.colProjectName.Name = "colProjectName";
            this.colProjectName.ReadOnly = true;
            // 
            // btnInitialWBSDocTemplate
            // 
            this.btnInitialWBSDocTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInitialWBSDocTemplate.Location = new System.Drawing.Point(583, 472);
            this.btnInitialWBSDocTemplate.Name = "btnInitialWBSDocTemplate";
            this.btnInitialWBSDocTemplate.Size = new System.Drawing.Size(196, 23);
            this.btnInitialWBSDocTemplate.TabIndex = 192;
            this.btnInitialWBSDocTemplate.Text = "初始化选择项目的GWBS文档模板";
            this.btnInitialWBSDocTemplate.UseVisualStyleBackColor = true;
            // 
            // VInitGWBSDocumentTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 507);
            this.Name = "VInitGWBSDocumentTemplate";
            this.Text = "初始化项目文档模板";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private System.Windows.Forms.Label lblTempName;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridProject;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectName;
        private System.Windows.Forms.Button btnInitialWBSDocTemplate;


    }
}