namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng
{
    partial class VDocApprovalQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDocApprovalQuery));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtChName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.dtpCreateDateBegin = new System.Windows.Forms.DateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpCreateDateEnd = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.cmoApprovalStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtPersonCharge = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colselect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colChName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubmitDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPersonCharge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApprovalStyle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(927, 509);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtChName);
            this.groupBox1.Controls.Add(this.dtpCreateDateBegin);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Controls.Add(this.dtpCreateDateEnd);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmoApprovalStyle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtPersonCharge);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(22, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(864, 118);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // txtChName
            // 
            this.txtChName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtChName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtChName.DrawSelf = false;
            this.txtChName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtChName.EnterToTab = false;
            this.txtChName.Location = new System.Drawing.Point(99, 32);
            this.txtChName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtChName.Name = "txtChName";
            this.txtChName.Padding = new System.Windows.Forms.Padding(1);
            this.txtChName.ReadOnly = false;
            this.txtChName.Size = new System.Drawing.Size(178, 16);
            this.txtChName.TabIndex = 235;
            // 
            // dtpCreateDateBegin
            // 
            this.dtpCreateDateBegin.Location = new System.Drawing.Point(106, 75);
            this.dtpCreateDateBegin.Name = "dtpCreateDateBegin";
            this.dtpCreateDateBegin.Size = new System.Drawing.Size(120, 21);
            this.dtpCreateDateBegin.TabIndex = 234;
            this.dtpCreateDateBegin.Value = new System.DateTime(2012, 7, 10, 8, 47, 0, 0);
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(232, 81);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 233;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpCreateDateEnd
            // 
            this.dtpCreateDateEnd.Location = new System.Drawing.Point(249, 75);
            this.dtpCreateDateEnd.Name = "dtpCreateDateEnd";
            this.dtpCreateDateEnd.Size = new System.Drawing.Size(120, 21);
            this.dtpCreateDateEnd.TabIndex = 232;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 231;
            this.label11.Text = "提交日期：";
            // 
            // cmoApprovalStyle
            // 
            this.cmoApprovalStyle.FormattingEnabled = true;
            this.cmoApprovalStyle.Location = new System.Drawing.Point(603, 28);
            this.cmoApprovalStyle.Name = "cmoApprovalStyle";
            this.cmoApprovalStyle.Size = new System.Drawing.Size(105, 20);
            this.cmoApprovalStyle.TabIndex = 225;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(523, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 224;
            this.label2.Text = "审批类型：";
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(633, 76);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 212;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(525, 76);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 211;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtPersonCharge
            // 
            this.txtPersonCharge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPersonCharge.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPersonCharge.DrawSelf = false;
            this.txtPersonCharge.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPersonCharge.EnterToTab = false;
            this.txtPersonCharge.Location = new System.Drawing.Point(354, 32);
            this.txtPersonCharge.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPersonCharge.Name = "txtPersonCharge";
            this.txtPersonCharge.Padding = new System.Windows.Forms.Padding(1);
            this.txtPersonCharge.ReadOnly = false;
            this.txtPersonCharge.Size = new System.Drawing.Size(145, 16);
            this.txtPersonCharge.TabIndex = 167;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "文档名称：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(295, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "责任人：";
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.ControlDark;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colselect,
            this.colChName,
            this.colRemark,
            this.colSubmitDate,
            this.colPersonCharge,
            this.colRealOperationDate,
            this.colCreateDate,
            this.colApprovalStyle,
            this.colState});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(22, 144);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.PageIndex = 0;
            this.dgDetail.PageSize = 0;
            this.dgDetail.Query = null;
            this.dgDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDetail.ReadOnlyCols")));
            this.dgDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDetail.RowHeadersVisible = false;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetail.Size = new System.Drawing.Size(864, 331);
            this.dgDetail.TabIndex = 6;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colselect
            // 
            this.colselect.HeaderText = "选择";
            this.colselect.Name = "colselect";
            this.colselect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colselect.Width = 50;
            // 
            // colChName
            // 
            this.colChName.HeaderText = "文档名称";
            this.colChName.Name = "colChName";
            this.colChName.Width = 150;
            // 
            // colRemark
            // 
            this.colRemark.HeaderText = "备注说明";
            this.colRemark.Name = "colRemark";
            this.colRemark.Width = 150;
            // 
            // colSubmitDate
            // 
            this.colSubmitDate.HeaderText = "提交时间";
            this.colSubmitDate.Name = "colSubmitDate";
            this.colSubmitDate.Width = 150;
            // 
            // colPersonCharge
            // 
            this.colPersonCharge.HeaderText = "责任人";
            this.colPersonCharge.Name = "colPersonCharge";
            this.colPersonCharge.Width = 150;
            // 
            // colRealOperationDate
            // 
            this.colRealOperationDate.HeaderText = "制单时间";
            this.colRealOperationDate.Name = "colRealOperationDate";
            // 
            // colCreateDate
            // 
            this.colCreateDate.HeaderText = "业务时间";
            this.colCreateDate.Name = "colCreateDate";
            // 
            // colApprovalStyle
            // 
            this.colApprovalStyle.HeaderText = "审批类型";
            this.colApprovalStyle.Name = "colApprovalStyle";
            this.colApprovalStyle.Width = 150;
            // 
            // colState
            // 
            this.colState.HeaderText = "状态";
            this.colState.Name = "colState";
            // 
            // VDocApprovalQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 509);
            this.Name = "VDocApprovalQuery";
            this.Text = "工程文档审批查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPersonCharge;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.ComboBox cmoApprovalStyle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpCreateDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private System.Windows.Forms.DateTimePicker dtpCreateDateEnd;
        private System.Windows.Forms.Label label11;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtChName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colselect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubmitDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPersonCharge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApprovalStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
    }
}