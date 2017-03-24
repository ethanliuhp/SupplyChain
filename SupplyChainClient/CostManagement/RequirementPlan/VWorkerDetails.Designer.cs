namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    partial class VWorkerDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VWorkerDetails));
            this.dgWorkerDeails = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colWorkerType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeopleNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWorkerDeails)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgWorkerDeails);
            this.pnlFloor.Size = new System.Drawing.Size(525, 325);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgWorkerDeails, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // dgWorkerDeails
            // 
            this.dgWorkerDeails.AddDefaultMenu = false;
            this.dgWorkerDeails.AddNoColumn = true;
            this.dgWorkerDeails.AllowUserToAddRows = false;
            this.dgWorkerDeails.AllowUserToDeleteRows = false;
            this.dgWorkerDeails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgWorkerDeails.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgWorkerDeails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgWorkerDeails.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgWorkerDeails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgWorkerDeails.ColumnHeadersHeight = 24;
            this.dgWorkerDeails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgWorkerDeails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWorkerType,
            this.colPeopleNum});
            this.dgWorkerDeails.CustomBackColor = false;
            this.dgWorkerDeails.EditCellBackColor = System.Drawing.Color.White;
            this.dgWorkerDeails.EnableHeadersVisualStyles = false;
            this.dgWorkerDeails.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgWorkerDeails.FreezeFirstRow = false;
            this.dgWorkerDeails.FreezeLastRow = false;
            this.dgWorkerDeails.FrontColumnCount = 0;
            this.dgWorkerDeails.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgWorkerDeails.HScrollOffset = 0;
            this.dgWorkerDeails.IsAllowOrder = true;
            this.dgWorkerDeails.IsConfirmDelete = true;
            this.dgWorkerDeails.Location = new System.Drawing.Point(12, 12);
            this.dgWorkerDeails.Name = "dgWorkerDeails";
            this.dgWorkerDeails.PageIndex = 0;
            this.dgWorkerDeails.PageSize = 0;
            this.dgWorkerDeails.Query = null;
            this.dgWorkerDeails.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgWorkerDeails.ReadOnlyCols")));
            this.dgWorkerDeails.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgWorkerDeails.RowHeadersWidth = 22;
            this.dgWorkerDeails.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgWorkerDeails.RowTemplate.Height = 23;
            this.dgWorkerDeails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgWorkerDeails.Size = new System.Drawing.Size(501, 301);
            this.dgWorkerDeails.TabIndex = 214;
            this.dgWorkerDeails.TargetType = null;
            this.dgWorkerDeails.VScrollOffset = 0;
            // 
            // colWorkerType
            // 
            this.colWorkerType.HeaderText = "工种";
            this.colWorkerType.Name = "colWorkerType";
            this.colWorkerType.Width = 260;
            // 
            // colPeopleNum
            // 
            this.colPeopleNum.HeaderText = "所需人数";
            this.colPeopleNum.Name = "colPeopleNum";
            this.colPeopleNum.Width = 150;
            // 
            // VWorkerDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 325);
            this.Name = "VWorkerDetails";
            this.Text = "工种明细";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWorkerDeails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgWorkerDeails;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWorkerType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeopleNum;
    }
}