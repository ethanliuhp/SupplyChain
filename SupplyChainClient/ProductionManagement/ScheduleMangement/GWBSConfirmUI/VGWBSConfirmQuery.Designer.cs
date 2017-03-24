namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI
{
    partial class VGWBSConfirmQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VGWBSConfirmQuery));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtGWBSTreeName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCodeEnd = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtCodeBegin = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel21 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.customLabel17 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel18 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGWBSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGWBSDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantityBeforeConfirm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualCompletedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskCompletedPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumCompletedPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskHandler = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialFeeSettlement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRecordTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.lblRecordTotal);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(911, 498);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecordTotal, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -8);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtGWBSTreeName);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.txtCodeEnd);
            this.groupBox1.Controls.Add(this.txtCodeBegin);
            this.groupBox1.Controls.Add(this.customLabel21);
            this.groupBox1.Controls.Add(this.txtCreatePerson);
            this.groupBox1.Controls.Add(this.customLabel17);
            this.groupBox1.Controls.Add(this.customLabel18);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(887, 64);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(726, 34);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 12;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(649, 35);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtGWBSTreeName
            // 
            this.txtGWBSTreeName.BackColor = System.Drawing.SystemColors.Control;
            this.txtGWBSTreeName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtGWBSTreeName.DrawSelf = false;
            this.txtGWBSTreeName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtGWBSTreeName.EnterToTab = false;
            this.txtGWBSTreeName.Location = new System.Drawing.Point(77, 42);
            this.txtGWBSTreeName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtGWBSTreeName.Name = "txtGWBSTreeName";
            this.txtGWBSTreeName.Padding = new System.Windows.Forms.Padding(1);
            this.txtGWBSTreeName.ReadOnly = false;
            this.txtGWBSTreeName.Size = new System.Drawing.Size(231, 16);
            this.txtGWBSTreeName.TabIndex = 7;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(12, 42);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 91;
            this.customLabel3.Text = "工程任务:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCodeEnd
            // 
            this.txtCodeEnd.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeEnd.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeEnd.DrawSelf = false;
            this.txtCodeEnd.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeEnd.EnterToTab = false;
            this.txtCodeEnd.Location = new System.Drawing.Point(204, 13);
            this.txtCodeEnd.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeEnd.Name = "txtCodeEnd";
            this.txtCodeEnd.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeEnd.ReadOnly = false;
            this.txtCodeEnd.Size = new System.Drawing.Size(104, 16);
            this.txtCodeEnd.TabIndex = 2;
            // 
            // txtCodeBegin
            // 
            this.txtCodeBegin.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBegin.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBegin.DrawSelf = false;
            this.txtCodeBegin.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBegin.EnterToTab = false;
            this.txtCodeBegin.Location = new System.Drawing.Point(77, 13);
            this.txtCodeBegin.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBegin.Name = "txtCodeBegin";
            this.txtCodeBegin.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBegin.ReadOnly = false;
            this.txtCodeBegin.Size = new System.Drawing.Size(104, 16);
            this.txtCodeBegin.TabIndex = 1;
            // 
            // customLabel21
            // 
            this.customLabel21.AutoSize = true;
            this.customLabel21.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel21.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel21.Location = new System.Drawing.Point(647, 17);
            this.customLabel21.Name = "customLabel21";
            this.customLabel21.Size = new System.Drawing.Size(47, 12);
            this.customLabel21.TabIndex = 88;
            this.customLabel21.Text = "确认人:";
            this.customLabel21.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.AcceptsEscape = false;
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.Code = null;
            this.txtCreatePerson.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtCreatePerson.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Id = "";
            this.txtCreatePerson.IsAllLoad = true;
            this.txtCreatePerson.Location = new System.Drawing.Point(700, 13);
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Result = ((System.Collections.IList)(resources.GetObject("txtCreatePerson.Result")));
            this.txtCreatePerson.RightMouse = false;
            this.txtCreatePerson.Size = new System.Drawing.Size(100, 21);
            this.txtCreatePerson.TabIndex = 5;
            this.txtCreatePerson.Tag = null;
            this.txtCreatePerson.Value = "";
            this.txtCreatePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel17
            // 
            this.customLabel17.AutoSize = true;
            this.customLabel17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel17.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel17.Location = new System.Drawing.Point(187, 17);
            this.customLabel17.Name = "customLabel17";
            this.customLabel17.Size = new System.Drawing.Size(11, 12);
            this.customLabel17.TabIndex = 52;
            this.customLabel17.Text = "-";
            this.customLabel17.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel18
            // 
            this.customLabel18.AutoSize = true;
            this.customLabel18.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel18.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel18.Location = new System.Drawing.Point(24, 17);
            this.customLabel18.Name = "customLabel18";
            this.customLabel18.Size = new System.Drawing.Size(47, 12);
            this.customLabel18.TabIndex = 53;
            this.customLabel18.Text = "单据号:";
            this.customLabel18.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(326, 17);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 80;
            this.customLabel1.Text = "确认日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(391, 10);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 3;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(523, 10);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 4;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(506, 17);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 83;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
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
            this.colCode,
            this.colState,
            this.colGWBSName,
            this.colGWBSDetail,
            this.colQuantityBeforeConfirm,
            this.colActualCompletedQuantity,
            this.colTaskCompletedPercent,
            this.colSumCompletedPercent,
            this.colTaskHandler,
            this.colUnit,
            this.colDescript,
            this.colMaterialFeeSettlement});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(12, 75);
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
            this.dgDetail.Size = new System.Drawing.Size(887, 392);
            this.dgDetail.TabIndex = 3;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "单据号";
            this.colCode.Name = "colCode";
            this.colCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCode.Width = 80;
            // 
            // colState
            // 
            this.colState.HeaderText = "单据状态";
            this.colState.Name = "colState";
            this.colState.Width = 80;
            // 
            // colGWBSName
            // 
            this.colGWBSName.HeaderText = "工程任务";
            this.colGWBSName.Name = "colGWBSName";
            // 
            // colGWBSDetail
            // 
            this.colGWBSDetail.HeaderText = "工程任务明细";
            this.colGWBSDetail.Name = "colGWBSDetail";
            // 
            // colQuantityBeforeConfirm
            // 
            this.colQuantityBeforeConfirm.HeaderText = "确认前累计确认工程量";
            this.colQuantityBeforeConfirm.Name = "colQuantityBeforeConfirm";
            // 
            // colActualCompletedQuantity
            // 
            this.colActualCompletedQuantity.HeaderText = "本次确认工程量";
            this.colActualCompletedQuantity.Name = "colActualCompletedQuantity";
            // 
            // colTaskCompletedPercent
            // 
            this.colTaskCompletedPercent.HeaderText = "确认前累积形象进度(%)";
            this.colTaskCompletedPercent.Name = "colTaskCompletedPercent";
            this.colTaskCompletedPercent.Width = 150;
            // 
            // colSumCompletedPercent
            // 
            this.colSumCompletedPercent.HeaderText = "确认后累积形象进度(%)";
            this.colSumCompletedPercent.Name = "colSumCompletedPercent";
            this.colSumCompletedPercent.Width = 150;
            // 
            // colTaskHandler
            // 
            this.colTaskHandler.HeaderText = "任务承担者";
            this.colTaskHandler.Name = "colTaskHandler";
            this.colTaskHandler.Width = 80;
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            this.colUnit.Width = 80;
            // 
            // colDescript
            // 
            this.colDescript.HeaderText = "差异说明";
            this.colDescript.Name = "colDescript";
            // 
            // colMaterialFeeSettlement
            // 
            this.colMaterialFeeSettlement.HeaderText = "材料是否结算";
            this.colMaterialFeeSettlement.Name = "colMaterialFeeSettlement";
            // 
            // lblRecordTotal
            // 
            this.lblRecordTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecordTotal.AutoSize = true;
            this.lblRecordTotal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecordTotal.ForeColor = System.Drawing.Color.Red;
            this.lblRecordTotal.Location = new System.Drawing.Point(12, 477);
            this.lblRecordTotal.Name = "lblRecordTotal";
            this.lblRecordTotal.Size = new System.Drawing.Size(83, 12);
            this.lblRecordTotal.TabIndex = 97;
            this.lblRecordTotal.Text = "共【0】条记录";
            this.lblRecordTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VGWBSConfirmQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 498);
            this.Name = "VGWBSConfirmQuery";
            this.Text = "工程任务确认单查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel17;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel18;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel21;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtGWBSTreeName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecordTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.DataGridViewLinkColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBSDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantityBeforeConfirm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualCompletedQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskCompletedPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumCompletedPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskHandler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialFeeSettlement;
    }
}