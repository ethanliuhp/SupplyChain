namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StockInMannerUI
{
    partial class VStockInManner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VStockInManner));
            this.grdDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.grdDetail);
            this.pnlFloor.Size = new System.Drawing.Size(691, 445);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.grdDetail, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(360, 28);
            // 
            // grdDetail
            // 
            this.grdDetail.AddDefaultMenu = false;
            this.grdDetail.AddNoColumn = false;
            this.grdDetail.AllowUserToOrderColumns = true;
            this.grdDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grdDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.grdDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grdDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Code,
            this.Name,
            this.State});
            this.grdDetail.CustomBackColor = false;
            this.grdDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grdDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.grdDetail.FreezeFirstRow = false;
            this.grdDetail.FreezeLastRow = false;
            this.grdDetail.FrontColumnCount = 0;
            this.grdDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.grdDetail.IsAllowOrder = true;
            this.grdDetail.Location = new System.Drawing.Point(78, 86);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.PageIndex = 0;
            this.grdDetail.PageSize = 0;
            this.grdDetail.Query = null;
            this.grdDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("grdDetail.ReadOnlyCols")));
            this.grdDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grdDetail.RowHeadersWidth = 22;
            this.grdDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdDetail.RowTemplate.Height = 23;
            this.grdDetail.Size = new System.Drawing.Size(548, 297);
            this.grdDetail.TabIndex = 2;
            this.grdDetail.TargetType = null;
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Code.FillWeight = 194.9239F;
            this.Code.HeaderText = "入库方式编码";
            this.Code.Name = "Code";
            this.Code.Width = 150;
            // 
            // Name
            // 
            this.Name.FillWeight = 5.076141F;
            this.Name.HeaderText = "入库方式名称";
            this.Name.Name = "Name";
            this.Name.Width = 300;
            // 
            // State
            // 
            this.State.HeaderText = "状态";
            this.State.Name = "State";
            this.State.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.State.Width = 50;
            // 
            // VStockInManner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 445);
            
            this.Text = "入库方式编码";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView grdDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn State;

    }
}