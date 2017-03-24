namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn
{
    partial class VMaterialHireRepair
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMaterialHireRepair));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgMatRepair = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colRepairContent = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colRepairQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepairDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dgDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMatRepair)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.dgMatRepair);
            this.pnlFloor.Size = new System.Drawing.Size(476, 250);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgMatRepair, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(212, 20);
            // 
            // dgMatRepair
            // 
            this.dgMatRepair.AddDefaultMenu = false;
            this.dgMatRepair.AddNoColumn = true;
            this.dgMatRepair.AllowUserToOrderColumns = true;
            this.dgMatRepair.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMatRepair.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgMatRepair.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMatRepair.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMatRepair.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMatRepair.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRepairContent,
            this.colRepairQuantity,
            this.colRepairDescript});
            this.dgMatRepair.CustomBackColor = false;
            this.dgMatRepair.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgMatRepair.EditCellBackColor = System.Drawing.Color.White;
            this.dgMatRepair.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgMatRepair.FreezeFirstRow = false;
            this.dgMatRepair.FreezeLastRow = false;
            this.dgMatRepair.FrontColumnCount = 0;
            this.dgMatRepair.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgMatRepair.HScrollOffset = 0;
            this.dgMatRepair.IsAllowOrder = true;
            this.dgMatRepair.IsConfirmDelete = true;
            this.dgMatRepair.Location = new System.Drawing.Point(0, 0);
            this.dgMatRepair.Name = "dgMatRepair";
            this.dgMatRepair.PageIndex = 0;
            this.dgMatRepair.PageSize = 0;
            this.dgMatRepair.Query = null;
            this.dgMatRepair.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgMatRepair.ReadOnlyCols")));
            this.dgMatRepair.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMatRepair.RowHeadersWidth = 22;
            this.dgMatRepair.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMatRepair.RowTemplate.Height = 23;
            this.dgMatRepair.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgMatRepair.Size = new System.Drawing.Size(476, 218);
            this.dgMatRepair.TabIndex = 24;
            this.dgMatRepair.TargetType = null;
            this.dgMatRepair.VScrollOffset = 0;
            // 
            // colRepairContent
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            this.colRepairContent.DefaultCellStyle = dataGridViewCellStyle5;
            this.colRepairContent.HeaderText = "维修内容";
            this.colRepairContent.Name = "colRepairContent";
            this.colRepairContent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRepairContent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colRepairContent.Width = 150;
            // 
            // colRepairQuantity
            // 
            dataGridViewCellStyle6.Format = "N3";
            this.colRepairQuantity.DefaultCellStyle = dataGridViewCellStyle6;
            this.colRepairQuantity.HeaderText = "数量";
            this.colRepairQuantity.Name = "colRepairQuantity";
            // 
            // colRepairDescript
            // 
            this.colRepairDescript.HeaderText = "备注";
            this.colRepairDescript.Name = "colRepairDescript";
            this.colRepairDescript.Width = 200;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(377, 224);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(289, 224);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dgDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // dgDelete
            // 
            this.dgDelete.Name = "dgDelete";
            this.dgDelete.Size = new System.Drawing.Size(152, 22);
            this.dgDelete.Text = "删除";
            // 
            // VMaterialHireRepair
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 250);
            this.Name = "VMaterialHireRepair";
            this.Text = "维修费用明细";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMatRepair)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMatRepair;
        private System.Windows.Forms.DataGridViewComboBoxColumn colRepairContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepairQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepairDescript;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dgDelete;
    }
}