﻿namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    partial class VLogQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VLogQuery));
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colBillType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtOptrPerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Size = new System.Drawing.Size(948, 443);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(448, 20);
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(186, 21);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 31;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(203, 14);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 30;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(71, 14);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 28;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(6, 18);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 29;
            this.customLabel1.Text = "操作日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(618, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(76, 23);
            this.btnSearch.TabIndex = 33;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
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
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBillType,
            this.colOperation,
            this.colOperPerson,
            this.colOperDate,
            this.colBillId,
            this.colBillCode,
            this.colDescript,
            this.colProName});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(12, 59);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.PageIndex = 0;
            this.dgDetail.PageSize = 0;
            this.dgDetail.Query = null;
            this.dgDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDetail.ReadOnlyCols")));
            this.dgDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.RowHeadersVisible = false;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgDetail.Size = new System.Drawing.Size(924, 372);
            this.dgDetail.TabIndex = 34;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colBillType
            // 
            this.colBillType.HeaderText = "单据类型";
            this.colBillType.Name = "colBillType";
            // 
            // colOperation
            // 
            this.colOperation.HeaderText = "操作类型";
            this.colOperation.Name = "colOperation";
            // 
            // colOperPerson
            // 
            this.colOperPerson.HeaderText = "操作人";
            this.colOperPerson.Name = "colOperPerson";
            // 
            // colOperDate
            // 
            this.colOperDate.FillWeight = 80F;
            this.colOperDate.HeaderText = "操作时间";
            this.colOperDate.Name = "colOperDate";
            // 
            // colBillId
            // 
            this.colBillId.HeaderText = "单据ID";
            this.colBillId.Name = "colBillId";
            this.colBillId.Visible = false;
            // 
            // colBillCode
            // 
            this.colBillCode.HeaderText = "单据号";
            this.colBillCode.Name = "colBillCode";
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "描述";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 150;
            // 
            // colProName
            // 
            this.colProName.HeaderText = "项目名称";
            this.colProName.Name = "colProName";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.txtProject);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.txtOptrPerson);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Location = new System.Drawing.Point(14, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(832, 41);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            // 
            // txtOptrPerson
            // 
            this.txtOptrPerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOptrPerson.DrawSelf = false;
            this.txtOptrPerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOptrPerson.EnterToTab = false;
            this.txtOptrPerson.Location = new System.Drawing.Point(376, 16);
            this.txtOptrPerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOptrPerson.Name = "txtOptrPerson";
            this.txtOptrPerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtOptrPerson.ReadOnly = false;
            this.txtOptrPerson.Size = new System.Drawing.Size(80, 16);
            this.txtOptrPerson.TabIndex = 150;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(326, 18);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(47, 12);
            this.customLabel2.TabIndex = 149;
            this.customLabel2.Text = "操作人:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProject
            // 
            this.txtProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProject.DrawSelf = false;
            this.txtProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProject.EnterToTab = false;
            this.txtProject.Location = new System.Drawing.Point(504, 16);
            this.txtProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProject.Name = "txtProject";
            this.txtProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtProject.ReadOnly = false;
            this.txtProject.Size = new System.Drawing.Size(103, 16);
            this.txtProject.TabIndex = 152;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(466, 18);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(35, 12);
            this.customLabel3.TabIndex = 151;
            this.customLabel3.Text = "项目:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(708, 12);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 153;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // VLogQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 443);
            this.Name = "VLogQuery";
            this.Text = "日志查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOptrPerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
    }
}