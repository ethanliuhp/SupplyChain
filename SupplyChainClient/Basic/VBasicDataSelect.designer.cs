namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    partial class VBasicDataSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VBasicDataSelect));
            this.btnSave = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnClose = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblBM = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblName = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.lnkNone = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.lnkAll = new VirtualMachine.Component.WinControls.Controls.CustomLinkLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvOptr = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptr)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.lblBM);
            this.pnlFloor.Controls.Add(this.txtCode);
            this.pnlFloor.Controls.Add(this.lblName);
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Controls.Add(this.txtName);
            this.pnlFloor.Size = new System.Drawing.Size(653, 492);
            this.pnlFloor.Controls.SetChildIndex(this.txtName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtCode, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblBM, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 16);
            this.lblTitle.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSave.Location = new System.Drawing.Point(190, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 54;
            this.btnSave.Text = "确　定";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnClose.Location = new System.Drawing.Point(321, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 55;
            this.btnClose.Text = "关　闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // txtCode
            // 
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(116, 24);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = false;
            this.txtCode.Size = new System.Drawing.Size(107, 15);
            this.txtCode.TabIndex = 57;
            // 
            // lblBM
            // 
            this.lblBM.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBM.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblBM.Location = new System.Drawing.Point(54, 24);
            this.lblBM.Name = "lblBM";
            this.lblBM.Size = new System.Drawing.Size(56, 12);
            this.lblBM.TabIndex = 56;
            this.lblBM.Text = "编码";
            this.lblBM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblBM.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtName
            // 
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = false;
            this.txtName.Location = new System.Drawing.Point(319, 24);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = false;
            this.txtName.Size = new System.Drawing.Size(107, 16);
            this.txtName.TabIndex = 59;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblName.Location = new System.Drawing.Point(252, 27);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(61, 12);
            this.lblName.TabIndex = 58;
            this.lblName.Text = "名称";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblName.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(432, 19);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 60;
            this.btnQuery.Text = "查　询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // lnkNone
            // 
            this.lnkNone.AutoSize = true;
            this.lnkNone.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkNone.Location = new System.Drawing.Point(88, 16);
            this.lnkNone.Name = "lnkNone";
            this.lnkNone.Size = new System.Drawing.Size(29, 12);
            this.lnkNone.TabIndex = 62;
            this.lnkNone.TabStop = true;
            this.lnkNone.Text = "全空";
            this.lnkNone.Visible = false;
            // 
            // lnkAll
            // 
            this.lnkAll.AutoSize = true;
            this.lnkAll.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkAll.Location = new System.Drawing.Point(51, 16);
            this.lnkAll.Name = "lnkAll";
            this.lnkAll.Size = new System.Drawing.Size(29, 12);
            this.lnkAll.TabIndex = 61;
            this.lnkAll.TabStop = true;
            this.lnkAll.Text = "全选";
            this.lnkAll.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.81715F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.18285F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(653, 492);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // dgvOptr
            // 
            this.dgvOptr.AddDefaultMenu = false;
            this.dgvOptr.AddNoColumn = true;
            this.dgvOptr.AllowUserToAddRows = false;
            this.dgvOptr.AllowUserToDeleteRows = false;
            this.dgvOptr.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOptr.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOptr.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvOptr.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvOptr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOptr.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.BCode,
            this.BName,
            this.Remark});
            this.dgvOptr.CustomBackColor = false;
            this.dgvOptr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOptr.EditCellBackColor = System.Drawing.Color.White;
            this.dgvOptr.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextRow;
            this.dgvOptr.FreezeFirstRow = false;
            this.dgvOptr.FreezeLastRow = false;
            this.dgvOptr.FrontColumnCount = 0;
            this.dgvOptr.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvOptr.IsAllowOrder = true;
            this.dgvOptr.IsConfirmDelete = true;
            this.dgvOptr.Location = new System.Drawing.Point(0, 0);
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
            this.dgvOptr.Size = new System.Drawing.Size(629, 391);
            this.dgvOptr.TabIndex = 4;
            this.dgvOptr.TargetType = null;
            // 
            // Selected
            // 
            this.Selected.HeaderText = "选择";
            this.Selected.Name = "Selected";
            this.Selected.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Selected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Selected.Visible = false;
            this.Selected.Width = 60;
            // 
            // BCode
            // 
            this.BCode.HeaderText = "编码";
            this.BCode.Name = "BCode";
            this.BCode.Width = 80;
            // 
            // BName
            // 
            this.BName.HeaderText = "名称";
            this.BName.Name = "BName";
            this.BName.Width = 240;
            // 
            // Remark
            // 
            this.Remark.HeaderText = "备注";
            this.Remark.Name = "Remark";
            this.Remark.Width = 160;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 48);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvOptr);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lnkNone);
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Panel2.Controls.Add(this.btnClose);
            this.splitContainer1.Panel2.Controls.Add(this.lnkAll);
            this.splitContainer1.Size = new System.Drawing.Size(629, 432);
            this.splitContainer1.SplitterDistance = 391;
            this.splitContainer1.TabIndex = 63;
            // 
            // VBasicDataSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 492);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "VBasicDataSelect";
            this.Text = "基础数据选择";
            this.Load += new System.EventHandler(this.VBasicDataSelect_Load);
            this.Controls.SetChildIndex(this.tableLayoutPanel1, 0);
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptr)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomButton btnClose;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSave;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblBM;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lnkNone;
        private VirtualMachine.Component.WinControls.Controls.CustomLinkLabel lnkAll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvOptr;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn BCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn BName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.SplitContainer splitContainer1;

    }
}