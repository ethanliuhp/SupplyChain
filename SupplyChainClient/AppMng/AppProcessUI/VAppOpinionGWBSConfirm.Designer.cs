namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI
{
    partial class VAppOpinionGWBSConfirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VAppOpinionGWBSConfirm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colConfirmHandlePersonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskHandler = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.FgAppSetpsInfo = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.StepOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StepName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppRelations = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetailNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGWBSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskHandle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPlannedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGWBSDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActualCompletedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantityBeforeConfirm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskCompletedPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSumCompletedPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescript = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialFeeSettlement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectTaskType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FgAppSetpsInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.splitContainer1);
            this.pnlFloor.Size = new System.Drawing.Size(937, 552);
            this.pnlFloor.Controls.SetChildIndex(this.splitContainer1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -201);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(937, 552);
            this.splitContainer1.SplitterDistance = 339;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgDetail);
            this.groupBox2.Location = new System.Drawing.Point(11, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(913, 192);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "明细信息";
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDetailNumber,
            this.colGWBSName,
            this.colTaskHandle,
            this.colPlannedQuantity,
            this.colGWBSDetail,
            this.colActualCompletedQuantity,
            this.colState,
            this.colQuantityBeforeConfirm,
            this.colTaskCompletedPercent,
            this.colSumCompletedPercent,
            this.colUnit,
            this.colDescript,
            this.colMaterialFeeSettlement,
            this.colRealOperationDate,
            this.colProjectTaskType});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.dgDetail.Location = new System.Drawing.Point(3, 17);
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
            this.dgDetail.Size = new System.Drawing.Size(907, 172);
            this.dgDetail.TabIndex = 8;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgMaster);
            this.groupBox1.Location = new System.Drawing.Point(11, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(913, 128);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "主表信息";
            // 
            // dgMaster
            // 
            this.dgMaster.AddDefaultMenu = false;
            this.dgMaster.AddNoColumn = false;
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.AllowUserToDeleteRows = false;
            this.dgMaster.AllowUserToResizeRows = false;
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgMaster.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.ColumnHeadersHeight = 24;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colConfirmHandlePersonName,
            this.colCreateDate,
            this.colProjectName,
            this.colTaskHandler,
            this.colDocState});
            this.dgMaster.CustomBackColor = false;
            this.dgMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMaster.EditCellBackColor = System.Drawing.Color.White;
            this.dgMaster.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgMaster.EnableHeadersVisualStyles = false;
            this.dgMaster.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgMaster.FreezeFirstRow = false;
            this.dgMaster.FreezeLastRow = false;
            this.dgMaster.FrontColumnCount = 0;
            this.dgMaster.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgMaster.HScrollOffset = 0;
            this.dgMaster.IsAllowOrder = true;
            this.dgMaster.IsConfirmDelete = true;
            this.dgMaster.Location = new System.Drawing.Point(3, 17);
            this.dgMaster.Name = "dgMaster";
            this.dgMaster.PageIndex = 0;
            this.dgMaster.PageSize = 0;
            this.dgMaster.Query = null;
            this.dgMaster.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgMaster.ReadOnlyCols")));
            this.dgMaster.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.RowHeadersVisible = false;
            this.dgMaster.RowHeadersWidth = 22;
            this.dgMaster.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMaster.RowTemplate.Height = 23;
            this.dgMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMaster.Size = new System.Drawing.Size(907, 108);
            this.dgMaster.TabIndex = 7;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colCode
            // 
            this.colCode.HeaderText = "单据号";
            this.colCode.Name = "colCode";
            this.colCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCode.Width = 80;
            // 
            // colConfirmHandlePersonName
            // 
            this.colConfirmHandlePersonName.HeaderText = "确认人";
            this.colConfirmHandlePersonName.Name = "colConfirmHandlePersonName";
            // 
            // colCreateDate
            // 
            this.colCreateDate.HeaderText = "确认时间";
            this.colCreateDate.Name = "colCreateDate";
            // 
            // colProjectName
            // 
            this.colProjectName.HeaderText = "所属项目";
            this.colProjectName.Name = "colProjectName";
            this.colProjectName.Width = 140;
            // 
            // colTaskHandler
            // 
            this.colTaskHandler.HeaderText = "任务承担者";
            this.colTaskHandler.Name = "colTaskHandler";
            this.colTaskHandler.Visible = false;
            this.colTaskHandler.Width = 145;
            // 
            // colDocState
            // 
            this.colDocState.HeaderText = "状态";
            this.colDocState.Name = "colDocState";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.FgAppSetpsInfo);
            this.groupBox3.Location = new System.Drawing.Point(11, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(910, 201);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "审批意见";
            // 
            // FgAppSetpsInfo
            // 
            this.FgAppSetpsInfo.AddDefaultMenu = false;
            this.FgAppSetpsInfo.AddNoColumn = false;
            this.FgAppSetpsInfo.AllowUserToAddRows = false;
            this.FgAppSetpsInfo.AllowUserToDeleteRows = false;
            this.FgAppSetpsInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.FgAppSetpsInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FgAppSetpsInfo.CellBackColor = System.Drawing.SystemColors.Control;
            this.FgAppSetpsInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.FgAppSetpsInfo.ColumnHeadersHeight = 24;
            this.FgAppSetpsInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.FgAppSetpsInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StepOrder,
            this.StepName,
            this.AppRelations,
            this.AppRole,
            this.AppPerson,
            this.AppDateTime,
            this.AppStatus,
            this.AppComments});
            this.FgAppSetpsInfo.CustomBackColor = false;
            this.FgAppSetpsInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FgAppSetpsInfo.EditCellBackColor = System.Drawing.Color.White;
            this.FgAppSetpsInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.FgAppSetpsInfo.EnableHeadersVisualStyles = false;
            this.FgAppSetpsInfo.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.FgAppSetpsInfo.FreezeFirstRow = false;
            this.FgAppSetpsInfo.FreezeLastRow = false;
            this.FgAppSetpsInfo.FrontColumnCount = 0;
            this.FgAppSetpsInfo.GridColor = System.Drawing.SystemColors.WindowText;
            this.FgAppSetpsInfo.HScrollOffset = 0;
            this.FgAppSetpsInfo.IsAllowOrder = true;
            this.FgAppSetpsInfo.IsConfirmDelete = true;
            this.FgAppSetpsInfo.Location = new System.Drawing.Point(3, 17);
            this.FgAppSetpsInfo.Name = "FgAppSetpsInfo";
            this.FgAppSetpsInfo.PageIndex = 0;
            this.FgAppSetpsInfo.PageSize = 0;
            this.FgAppSetpsInfo.Query = null;
            this.FgAppSetpsInfo.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("FgAppSetpsInfo.ReadOnlyCols")));
            this.FgAppSetpsInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.FgAppSetpsInfo.RowHeadersVisible = false;
            this.FgAppSetpsInfo.RowHeadersWidth = 22;
            this.FgAppSetpsInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.FgAppSetpsInfo.RowTemplate.Height = 23;
            this.FgAppSetpsInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FgAppSetpsInfo.Size = new System.Drawing.Size(904, 181);
            this.FgAppSetpsInfo.TabIndex = 9;
            this.FgAppSetpsInfo.TargetType = null;
            this.FgAppSetpsInfo.VScrollOffset = 0;
            // 
            // StepOrder
            // 
            this.StepOrder.HeaderText = "审批步骤";
            this.StepOrder.Name = "StepOrder";
            this.StepOrder.Width = 60;
            // 
            // StepName
            // 
            this.StepName.HeaderText = "审批步骤名称";
            this.StepName.Name = "StepName";
            this.StepName.Width = 120;
            // 
            // AppRelations
            // 
            this.AppRelations.HeaderText = "审批关系";
            this.AppRelations.Name = "AppRelations";
            this.AppRelations.Width = 60;
            // 
            // AppRole
            // 
            this.AppRole.HeaderText = "审批角色";
            this.AppRole.Name = "AppRole";
            // 
            // AppPerson
            // 
            this.AppPerson.HeaderText = "审批人";
            this.AppPerson.Name = "AppPerson";
            // 
            // AppDateTime
            // 
            this.AppDateTime.HeaderText = "审批日期";
            this.AppDateTime.Name = "AppDateTime";
            // 
            // AppStatus
            // 
            this.AppStatus.HeaderText = "审批状态";
            this.AppStatus.Name = "AppStatus";
            this.AppStatus.Width = 80;
            // 
            // AppComments
            // 
            this.AppComments.HeaderText = "审批意见";
            this.AppComments.Name = "AppComments";
            this.AppComments.Width = 160;
            // 
            // colDetailNumber
            // 
            this.colDetailNumber.HeaderText = "明细编号";
            this.colDetailNumber.Name = "colDetailNumber";
            this.colDetailNumber.Width = 60;
            // 
            // colGWBSName
            // 
            this.colGWBSName.HeaderText = "工程任务";
            this.colGWBSName.Name = "colGWBSName";
            // 
            // colTaskHandle
            // 
            this.colTaskHandle.HeaderText = "任务承担者";
            this.colTaskHandle.Name = "colTaskHandle";
            this.colTaskHandle.Width = 145;
            // 
            // colPlannedQuantity
            // 
            this.colPlannedQuantity.HeaderText = "计划总工程量";
            this.colPlannedQuantity.Name = "colPlannedQuantity";
            // 
            // colGWBSDetail
            // 
            this.colGWBSDetail.HeaderText = "工程任务明细";
            this.colGWBSDetail.Name = "colGWBSDetail";
            // 
            // colActualCompletedQuantity
            // 
            this.colActualCompletedQuantity.HeaderText = "本次确认工程量";
            this.colActualCompletedQuantity.Name = "colActualCompletedQuantity";
            // 
            // colState
            // 
            this.colState.HeaderText = "核算状态";
            this.colState.Name = "colState";
            this.colState.Width = 80;
            // 
            // colQuantityBeforeConfirm
            // 
            this.colQuantityBeforeConfirm.HeaderText = "确认前累计确认工程量";
            this.colQuantityBeforeConfirm.Name = "colQuantityBeforeConfirm";
            this.colQuantityBeforeConfirm.Width = 130;
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
            // colRealOperationDate
            // 
            this.colRealOperationDate.HeaderText = "业务发生时间";
            this.colRealOperationDate.Name = "colRealOperationDate";
            // 
            // colProjectTaskType
            // 
            this.colProjectTaskType.HeaderText = "工程量类型";
            this.colProjectTaskType.Name = "colProjectTaskType";
            // 
            // VAppOpinionGWBSConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 552);
            this.Name = "VAppOpinionGWBSConfirm";
            this.Text = "审批意见";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FgAppSetpsInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView FgAppSetpsInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn StepOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn StepName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppRelations;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppComments;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private System.Windows.Forms.DataGridViewLinkColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfirmHandlePersonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskHandler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocState;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetailNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskHandle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPlannedQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGWBSDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActualCompletedQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantityBeforeConfirm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskCompletedPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSumCompletedPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescript;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialFeeSettlement;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectTaskType;

    }
}