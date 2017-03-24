namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng
{
    partial class VDangerTypeSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDangerTypeSelector));
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDangerType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.dgMaster);
            this.pnlFloor.Size = new System.Drawing.Size(275, 321);
            this.pnlFloor.Controls.SetChildIndex(this.dgMaster, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -8);
            // 
            // dgMaster
            // 
            this.dgMaster.AddDefaultMenu = false;
            this.dgMaster.AddNoColumn = false;
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.AllowUserToDeleteRows = false;
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgMaster.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.ColumnHeadersHeight = 24;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colDangerType});
            this.dgMaster.CustomBackColor = false;
            this.dgMaster.EditCellBackColor = System.Drawing.Color.White;
            this.dgMaster.EnableHeadersVisualStyles = false;
            this.dgMaster.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgMaster.FreezeFirstRow = false;
            this.dgMaster.FreezeLastRow = false;
            this.dgMaster.FrontColumnCount = 0;
            this.dgMaster.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgMaster.HScrollOffset = 0;
            this.dgMaster.IsAllowOrder = true;
            this.dgMaster.IsConfirmDelete = true;
            this.dgMaster.Location = new System.Drawing.Point(3, 3);
            this.dgMaster.Name = "dgMaster";
            this.dgMaster.PageIndex = 0;
            this.dgMaster.PageSize = 0;
            this.dgMaster.Query = null;
            this.dgMaster.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgMaster.ReadOnlyCols")));
            this.dgMaster.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.RowHeadersVisible = false;
            this.dgMaster.RowHeadersWidth = 22;
            this.dgMaster.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMaster.RowTemplate.Height = 23;
            this.dgMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMaster.Size = new System.Drawing.Size(269, 276);
            this.dgMaster.TabIndex = 3;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 50;
            // 
            // colDangerType
            // 
            this.colDangerType.HeaderText = "隐患类型";
            this.colDangerType.Name = "colDangerType";
            this.colDangerType.Width = 120;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(58, 286);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(182, 286);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // VDangerTypeSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 321);
            this.Name = "VDangerTypeSelector";
            this.Text = "隐患类型选择";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDangerType;
    }
}