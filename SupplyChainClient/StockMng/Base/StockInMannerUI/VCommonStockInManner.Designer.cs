namespace Application.Business.Erp.SupplyChain.Client.StockManage.Base.StockInMannerUI
{
    partial class VCommonStockInManner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCommonStockInManner));
            this.grdDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.customPanel1 = new VirtualMachine.Component.WinControls.Controls.CustomPanel();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).BeginInit();
            this.customPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.customPanel1);
            this.pnlFloor.Controls.Add(this.grdDetail);
            this.pnlFloor.Size = new System.Drawing.Size(551, 388);
            this.pnlFloor.Controls.SetChildIndex(this.grdDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customPanel1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(148, 9);
            this.lblTitle.Size = new System.Drawing.Size(93, 20);
            this.lblTitle.Text = "入库方式";
            // 
            // grdDetail
            // 
            this.grdDetail.AddDefaultMenu = false;
            this.grdDetail.AddNoColumn = false;
            this.grdDetail.AllowUserToAddRows = false;
            this.grdDetail.AllowUserToDeleteRows = false;
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
            this.grdDetail.EditCellBackColor = System.Drawing.Color.White;
            this.grdDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.grdDetail.FreezeFirstRow = false;
            this.grdDetail.FreezeLastRow = false;
            this.grdDetail.FrontColumnCount = 0;
            this.grdDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.grdDetail.IsAllowOrder = true;
            this.grdDetail.IsConfirmDelete = true;
            this.grdDetail.Location = new System.Drawing.Point(12, 36);
            this.grdDetail.Name = "grdDetail";
            this.grdDetail.PageIndex = 0;
            this.grdDetail.PageSize = 0;
            this.grdDetail.Query = null;
            this.grdDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("grdDetail.ReadOnlyCols")));
            this.grdDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grdDetail.RowHeadersWidth = 22;
            this.grdDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdDetail.RowTemplate.Height = 23;
            this.grdDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdDetail.Size = new System.Drawing.Size(527, 303);
            this.grdDetail.TabIndex = 3;
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
            this.State.Visible = false;
            this.State.Width = 50;
            // 
            // customPanel1
            // 
            this.customPanel1.Controls.Add(this.btnCancel);
            this.customPanel1.Controls.Add(this.btnOK);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.customPanel1.ExpandBorderColor = System.Drawing.SystemColors.ControlDark;
            this.customPanel1.ExpandBorderStyle = VirtualMachine.Component.WinControls.Controls.ExpandBorder.None;
            this.customPanel1.ExpandBorderWidth = 1F;
            this.customPanel1.Location = new System.Drawing.Point(0, 345);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(551, 43);
            this.customPanel1.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(464, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(363, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // VCommonStockInManner
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(551, 388);            
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "入库方式选择";
            this.TopMost = true;
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDetail)).EndInit();
            this.customPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomPanel customPanel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView grdDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn State;



    }
}