namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    partial class VSharingProportion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSharingProportion));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.rdoAverage = new System.Windows.Forms.RadioButton();
            this.rdoByProportion = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.dgSharingProjectAmount = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.SharingLeafNodePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SharingProportion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSharingProjectAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgSharingProjectAmount);
            this.pnlFloor.Controls.Add(this.btnQuit);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.rdoByProportion);
            this.pnlFloor.Controls.Add(this.rdoAverage);
            this.pnlFloor.Size = new System.Drawing.Size(509, 477);
            this.pnlFloor.Controls.SetChildIndex(this.rdoAverage, 0);
            this.pnlFloor.Controls.SetChildIndex(this.rdoByProportion, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuit, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgSharingProjectAmount, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(229, 0);
            // 
            // rdoAverage
            // 
            this.rdoAverage.AutoSize = true;
            this.rdoAverage.Location = new System.Drawing.Point(137, 12);
            this.rdoAverage.Name = "rdoAverage";
            this.rdoAverage.Size = new System.Drawing.Size(47, 16);
            this.rdoAverage.TabIndex = 1;
            this.rdoAverage.Text = "平摊";
            this.rdoAverage.UseVisualStyleBackColor = true;
            // 
            // rdoByProportion
            // 
            this.rdoByProportion.AutoSize = true;
            this.rdoByProportion.Checked = true;
            this.rdoByProportion.Location = new System.Drawing.Point(12, 12);
            this.rdoByProportion.Name = "rdoByProportion";
            this.rdoByProportion.Size = new System.Drawing.Size(83, 16);
            this.rdoByProportion.TabIndex = 2;
            this.rdoByProportion.TabStop = true;
            this.rdoByProportion.Text = "按比例分摊";
            this.rdoByProportion.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(99, 442);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnQuit.Location = new System.Drawing.Point(180, 442);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 7;
            this.btnQuit.Text = "放弃";
            this.btnQuit.UseVisualStyleBackColor = true;
            // 
            // dgSharingProjectAmount
            // 
            this.dgSharingProjectAmount.AddDefaultMenu = false;
            this.dgSharingProjectAmount.AddNoColumn = true;
            this.dgSharingProjectAmount.AllowUserToAddRows = false;
            this.dgSharingProjectAmount.AllowUserToDeleteRows = false;
            this.dgSharingProjectAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgSharingProjectAmount.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgSharingProjectAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgSharingProjectAmount.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgSharingProjectAmount.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgSharingProjectAmount.ColumnHeadersHeight = 24;
            this.dgSharingProjectAmount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgSharingProjectAmount.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SharingLeafNodePath,
            this.SharingProportion});
            this.dgSharingProjectAmount.CustomBackColor = false;
            this.dgSharingProjectAmount.EditCellBackColor = System.Drawing.Color.White;
            this.dgSharingProjectAmount.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgSharingProjectAmount.EnableHeadersVisualStyles = false;
            this.dgSharingProjectAmount.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgSharingProjectAmount.FreezeFirstRow = false;
            this.dgSharingProjectAmount.FreezeLastRow = false;
            this.dgSharingProjectAmount.FrontColumnCount = 0;
            this.dgSharingProjectAmount.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgSharingProjectAmount.HScrollOffset = 0;
            this.dgSharingProjectAmount.IsAllowOrder = true;
            this.dgSharingProjectAmount.IsConfirmDelete = true;
            this.dgSharingProjectAmount.Location = new System.Drawing.Point(12, 34);
            this.dgSharingProjectAmount.Name = "dgSharingProjectAmount";
            this.dgSharingProjectAmount.PageIndex = 0;
            this.dgSharingProjectAmount.PageSize = 0;
            this.dgSharingProjectAmount.Query = null;
            this.dgSharingProjectAmount.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgSharingProjectAmount.ReadOnlyCols")));
            this.dgSharingProjectAmount.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgSharingProjectAmount.RowHeadersVisible = false;
            this.dgSharingProjectAmount.RowHeadersWidth = 22;
            this.dgSharingProjectAmount.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgSharingProjectAmount.RowTemplate.Height = 23;
            this.dgSharingProjectAmount.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSharingProjectAmount.Size = new System.Drawing.Size(485, 402);
            this.dgSharingProjectAmount.TabIndex = 151;
            this.dgSharingProjectAmount.TargetType = null;
            this.dgSharingProjectAmount.VScrollOffset = 0;
            // 
            // SharingLeafNodePath
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.SharingLeafNodePath.DefaultCellStyle = dataGridViewCellStyle1;
            this.SharingLeafNodePath.HeaderText = "叶节点路径";
            this.SharingLeafNodePath.Name = "SharingLeafNodePath";
            this.SharingLeafNodePath.ReadOnly = true;
            this.SharingLeafNodePath.Width = 300;
            // 
            // SharingProportion
            // 
            this.SharingProportion.HeaderText = "分摊比例";
            this.SharingProportion.Name = "SharingProportion";
            // 
            // VSharingProportion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 477);
            this.Name = "VSharingProportion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量分摊比例";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSharingProjectAmount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoByProportion;
        private System.Windows.Forms.RadioButton rdoAverage;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnOK;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgSharingProjectAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn SharingLeafNodePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn SharingProportion;
    }
}