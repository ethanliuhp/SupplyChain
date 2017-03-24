namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppProcessUI
{
    partial class VAppOpinion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VAppOpinion));
            this.FgAppSetpsInfo = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.StepOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StepName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppRelations = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppComments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FgAppSetpsInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.FgAppSetpsInfo);
            this.pnlFloor.Size = new System.Drawing.Size(898, 272);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.FgAppSetpsInfo, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // FgAppSetpsInfo
            // 
            this.FgAppSetpsInfo.AddDefaultMenu = false;
            this.FgAppSetpsInfo.AddNoColumn = false;
            this.FgAppSetpsInfo.AllowUserToAddRows = false;
            this.FgAppSetpsInfo.AllowUserToDeleteRows = false;
            this.FgAppSetpsInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.FgAppSetpsInfo.Location = new System.Drawing.Point(12, 12);
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
            this.FgAppSetpsInfo.Size = new System.Drawing.Size(874, 248);
            this.FgAppSetpsInfo.TabIndex = 7;
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
            // VAppOpinion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 272);
            this.Name = "VAppOpinion";
            this.Text = "审批意见";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FgAppSetpsInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView FgAppSetpsInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn StepOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn StepName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppRelations;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppComments;
    }
}