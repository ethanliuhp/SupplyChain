namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng
{
    partial class VDocumentSortSelectByIRP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDocumentSortSelectByIRP));
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnSortTree = new System.Windows.Forms.Button();
            this.tvwSort = new System.Windows.Forms.TreeView();
            this.btnOK = new System.Windows.Forms.Button();
            this.dgvSortList = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSortCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSortExplain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSortList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgvSortList);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.tvwSort);
            this.pnlFloor.Controls.Add(this.btnSortTree);
            this.pnlFloor.Controls.Add(this.btnSelect);
            this.pnlFloor.Controls.Add(this.txtKeyword);
            this.pnlFloor.Size = new System.Drawing.Size(447, 518);
            this.pnlFloor.Controls.SetChildIndex(this.txtKeyword, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSelect, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSortTree, 0);
            this.pnlFloor.Controls.SetChildIndex(this.tvwSort, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgvSortList, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 0);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(12, 12);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(261, 21);
            this.txtKeyword.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(279, 12);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "查询";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // btnSortTree
            // 
            this.btnSortTree.Location = new System.Drawing.Point(360, 12);
            this.btnSortTree.Name = "btnSortTree";
            this.btnSortTree.Size = new System.Drawing.Size(75, 23);
            this.btnSortTree.TabIndex = 3;
            this.btnSortTree.Text = "分类树";
            this.btnSortTree.UseVisualStyleBackColor = true;
            // 
            // tvwSort
            // 
            this.tvwSort.BackColor = System.Drawing.SystemColors.Window;
            this.tvwSort.Location = new System.Drawing.Point(12, 41);
            this.tvwSort.Name = "tvwSort";
            this.tvwSort.Size = new System.Drawing.Size(422, 436);
            this.tvwSort.TabIndex = 5;
            this.tvwSort.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(359, 483);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // dgvSortList
            // 
            this.dgvSortList.AddDefaultMenu = false;
            this.dgvSortList.AddNoColumn = true;
            this.dgvSortList.AllowUserToAddRows = false;
            this.dgvSortList.AllowUserToDeleteRows = false;
            this.dgvSortList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSortList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvSortList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSortList.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvSortList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvSortList.ColumnHeadersHeight = 24;
            this.dgvSortList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSortList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSortCode,
            this.colSortName,
            this.colSortExplain});
            this.dgvSortList.CustomBackColor = false;
            this.dgvSortList.EditCellBackColor = System.Drawing.Color.White;
            this.dgvSortList.EnableHeadersVisualStyles = false;
            this.dgvSortList.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgvSortList.FreezeFirstRow = false;
            this.dgvSortList.FreezeLastRow = false;
            this.dgvSortList.FrontColumnCount = 0;
            this.dgvSortList.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvSortList.HScrollOffset = 0;
            this.dgvSortList.IsAllowOrder = true;
            this.dgvSortList.IsConfirmDelete = true;
            this.dgvSortList.Location = new System.Drawing.Point(12, 41);
            this.dgvSortList.Name = "dgvSortList";
            this.dgvSortList.PageIndex = 0;
            this.dgvSortList.PageSize = 0;
            this.dgvSortList.Query = null;
            this.dgvSortList.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvSortList.ReadOnlyCols")));
            this.dgvSortList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvSortList.RowHeadersVisible = false;
            this.dgvSortList.RowHeadersWidth = 22;
            this.dgvSortList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSortList.RowTemplate.Height = 23;
            this.dgvSortList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSortList.Size = new System.Drawing.Size(422, 436);
            this.dgvSortList.TabIndex = 7;
            this.dgvSortList.TargetType = null;
            this.dgvSortList.VScrollOffset = 0;
            // 
            // colSortCode
            // 
            this.colSortCode.HeaderText = "分类编码";
            this.colSortCode.Name = "colSortCode";
            this.colSortCode.Width = 110;
            // 
            // colSortName
            // 
            this.colSortName.HeaderText = "分类名称";
            this.colSortName.Name = "colSortName";
            this.colSortName.Width = 110;
            // 
            // colSortExplain
            // 
            this.colSortExplain.HeaderText = "分类说明";
            this.colSortExplain.Name = "colSortExplain";
            this.colSortExplain.Width = 130;
            // 
            // VDocumentSortSelectByIRP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 518);
            this.Name = "VDocumentSortSelectByIRP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "分类查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSortList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSortTree;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.TreeView tvwSort;
        private System.Windows.Forms.Button btnOK;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvSortList;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSortCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSortName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSortExplain;
    }
}