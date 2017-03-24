﻿namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VOwnerQuantityDtlSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VOwnerQuantityDtlSelector));
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colBillCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQWBS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantityDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantityMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfirmDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfirmMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpDateBeginBill = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEndBill = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel20 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtBillNo = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Size = new System.Drawing.Size(938, 456);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(127, 1);
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.Text = "票据信息选择";
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.AllowUserToResizeColumns = false;
            this.dgDetail.AllowUserToResizeRows = false;
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
            this.colBillCode,
            this.colQWBS,
            this.colQuantityDate,
            this.colQuantityMoney,
            this.colConfirmDate,
            this.colConfirmMoney,
            this.colCreatePerson,
            this.colDescript});
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
            this.dgDetail.Location = new System.Drawing.Point(9, 55);
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
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetail.Size = new System.Drawing.Size(926, 362);
            this.dgDetail.TabIndex = 15;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colBillCode
            // 
            this.colBillCode.HeaderText = "报量单号";
            this.colBillCode.Name = "colBillCode";
            // 
            // colQWBS
            // 
            this.colQWBS.HeaderText = "清单任务";
            this.colQWBS.Name = "colQWBS";
            this.colQWBS.Width = 120;
            // 
            // colQuantityDate
            // 
            this.colQuantityDate.HeaderText = "报量日期";
            this.colQuantityDate.Name = "colQuantityDate";
            this.colQuantityDate.Width = 80;
            // 
            // colQuantityMoney
            // 
            this.colQuantityMoney.HeaderText = "报量金额";
            this.colQuantityMoney.Name = "colQuantityMoney";
            this.colQuantityMoney.Width = 80;
            // 
            // colConfirmDate
            // 
            this.colConfirmDate.HeaderText = "审定日期";
            this.colConfirmDate.Name = "colConfirmDate";
            this.colConfirmDate.Width = 80;
            // 
            // colConfirmMoney
            // 
            this.colConfirmMoney.HeaderText = "审定金额";
            this.colConfirmMoney.Name = "colConfirmMoney";
            this.colConfirmMoney.Width = 80;
            // 
            // colCreatePerson
            // 
            this.colCreatePerson.HeaderText = "创建人";
            this.colCreatePerson.Name = "colCreatePerson";
            this.colCreatePerson.Width = 80;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "情况说明";
            this.colDescript.Name = "colDescript";
            this.colDescript.Width = 150;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(365, 423);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dtpDateBeginBill);
            this.groupBox1.Controls.Add(this.dtpDateEndBill);
            this.groupBox1.Controls.Add(this.customLabel20);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Location = new System.Drawing.Point(9, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(929, 46);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // dtpDateBeginBill
            // 
            this.dtpDateBeginBill.Location = new System.Drawing.Point(378, 16);
            this.dtpDateBeginBill.Name = "dtpDateBeginBill";
            this.dtpDateBeginBill.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBeginBill.TabIndex = 139;
            // 
            // dtpDateEndBill
            // 
            this.dtpDateEndBill.Location = new System.Drawing.Point(501, 16);
            this.dtpDateEndBill.Name = "dtpDateEndBill";
            this.dtpDateEndBill.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEndBill.TabIndex = 140;
            // 
            // customLabel20
            // 
            this.customLabel20.AutoSize = true;
            this.customLabel20.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel20.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel20.Location = new System.Drawing.Point(488, 20);
            this.customLabel20.Name = "customLabel20";
            this.customLabel20.Size = new System.Drawing.Size(11, 12);
            this.customLabel20.TabIndex = 141;
            this.customLabel20.Text = "-";
            this.customLabel20.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(319, 21);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 138;
            this.customLabel4.Text = "报送日期:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.SystemColors.Control;
            this.txtBillNo.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtBillNo.DrawSelf = false;
            this.txtBillNo.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtBillNo.EnterToTab = false;
            this.txtBillNo.Location = new System.Drawing.Point(230, 18);
            this.txtBillNo.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Padding = new System.Windows.Forms.Padding(1);
            this.txtBillNo.ReadOnly = false;
            this.txtBillNo.Size = new System.Drawing.Size(82, 16);
            this.txtBillNo.TabIndex = 94;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(171, 20);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 95;
            this.customLabel1.Text = "报量单号:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(816, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(268, 423);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // VOwnerQuantityDtlSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 456);
            this.Name = "VOwnerQuantityDtlSelector";
            this.Text = "报量信息选择";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtBillNo;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBeginBill;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEndBill;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel20;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQWBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantityDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantityMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfirmDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfirmMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;

    }
}