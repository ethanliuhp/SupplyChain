namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    partial class VBasicDataQuery
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VBasicDataQuery));
            this.dgvOptr = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.BasicCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BasicName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.listBoxType = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptr)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.listBoxType);
            this.pnlFloor.Controls.Add(this.dgvOptr);
            this.pnlFloor.Size = new System.Drawing.Size(775, 530);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgvOptr, 0);
            this.pnlFloor.Controls.SetChildIndex(this.listBoxType, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(179, 9);
            this.lblTitle.Size = new System.Drawing.Size(30, 20);
            this.lblTitle.Text = "置";
            // 
            // dgvOptr
            // 
            this.dgvOptr.AddDefaultMenu = false;
            this.dgvOptr.AddNoColumn = true;
            this.dgvOptr.AllowUserToDeleteRows = false;
            this.dgvOptr.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOptr.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOptr.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvOptr.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvOptr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOptr.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BasicCode,
            this.BasicName,
            this.Remark});
            this.dgvOptr.CustomBackColor = false;
            this.dgvOptr.EditCellBackColor = System.Drawing.Color.White;
            this.dgvOptr.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextRow;
            this.dgvOptr.FreezeFirstRow = false;
            this.dgvOptr.FreezeLastRow = false;
            this.dgvOptr.FrontColumnCount = 0;
            this.dgvOptr.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvOptr.IsAllowOrder = true;
            this.dgvOptr.IsConfirmDelete = true;
            this.dgvOptr.Location = new System.Drawing.Point(202, 12);
            this.dgvOptr.Name = "dgvOptr";
            this.dgvOptr.PageIndex = 0;
            this.dgvOptr.PageSize = 0;
            this.dgvOptr.Query = null;
            this.dgvOptr.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvOptr.ReadOnlyCols")));
            this.dgvOptr.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvOptr.RowHeadersWidth = 22;
            this.dgvOptr.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvOptr.RowTemplate.Height = 23;
            this.dgvOptr.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOptr.Size = new System.Drawing.Size(561, 486);
            this.dgvOptr.TabIndex = 3;
            this.dgvOptr.TargetType = null;
            // 
            // BasicCode
            // 
            this.BasicCode.HeaderText = "编码";
            this.BasicCode.Name = "BasicCode";
            this.BasicCode.Width = 80;
            // 
            // BasicName
            // 
            this.BasicName.HeaderText = "名称";
            this.BasicName.Name = "BasicName";
            this.BasicName.Width = 220;
            // 
            // Remark
            // 
            this.Remark.HeaderText = "备注";
            this.Remark.Name = "Remark";
            this.Remark.Width = 200;
            // 
            // listBoxType
            // 
            this.listBoxType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxType.FormattingEnabled = true;
            this.listBoxType.ItemHeight = 12;
            this.listBoxType.Location = new System.Drawing.Point(29, 64);
            this.listBoxType.Name = "listBoxType";
            this.listBoxType.Size = new System.Drawing.Size(167, 338);
            this.listBoxType.TabIndex = 52;
            // 
            // No
            // 
            this.No.HeaderText = "序号";
            this.No.Name = "No";
            this.No.Width = 60;
            // 
            // VBasicDataQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 530);
            this.Name = "VBasicDataQuery";
            this.Text = "基础数据查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvOptr;
        private Application.Business.Erp.ClientSystem.Template.CustomListBox listBoxType;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn BasicCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn BasicName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
    }
}