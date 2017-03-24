namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary
{
    partial class VMeetingQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMeetingQuery));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpCreateDateBegin = new System.Windows.Forms.DateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpCreateDateEnd = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMeetingStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtMeetingTopic = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colMeetingStyle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMeetingDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMeetingTopic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMeetingRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePersonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(902, 503);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpCreateDateBegin);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Controls.Add(this.dtpCreateDateEnd);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtMeetingStyle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtMeetingTopic);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(896, 49);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // dtpCreateDateBegin
            // 
            this.dtpCreateDateBegin.Location = new System.Drawing.Point(462, 17);
            this.dtpCreateDateBegin.Name = "dtpCreateDateBegin";
            this.dtpCreateDateBegin.Size = new System.Drawing.Size(103, 21);
            this.dtpCreateDateBegin.TabIndex = 240;
            this.dtpCreateDateBegin.Value = new System.DateTime(2012, 7, 10, 8, 47, 0, 0);
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(571, 20);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 239;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpCreateDateEnd
            // 
            this.dtpCreateDateEnd.Location = new System.Drawing.Point(588, 16);
            this.dtpCreateDateEnd.Name = "dtpCreateDateEnd";
            this.dtpCreateDateEnd.Size = new System.Drawing.Size(106, 21);
            this.dtpCreateDateEnd.TabIndex = 238;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(400, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 237;
            this.label11.Text = "会议日期：";
            // 
            // txtMeetingStyle
            // 
            this.txtMeetingStyle.FormattingEnabled = true;
            this.txtMeetingStyle.Location = new System.Drawing.Point(283, 18);
            this.txtMeetingStyle.Name = "txtMeetingStyle";
            this.txtMeetingStyle.Size = new System.Drawing.Size(90, 20);
            this.txtMeetingStyle.TabIndex = 236;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 235;
            this.label2.Text = "会议类型：";
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(802, 16);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 234;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(719, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 233;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtMeetingTopic
            // 
            this.txtMeetingTopic.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMeetingTopic.DrawSelf = false;
            this.txtMeetingTopic.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMeetingTopic.EnterToTab = false;
            this.txtMeetingTopic.Location = new System.Drawing.Point(80, 16);
            this.txtMeetingTopic.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMeetingTopic.Name = "txtMeetingTopic";
            this.txtMeetingTopic.Padding = new System.Windows.Forms.Padding(1);
            this.txtMeetingTopic.ReadOnly = false;
            this.txtMeetingTopic.Size = new System.Drawing.Size(120, 16);
            this.txtMeetingTopic.TabIndex = 232;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 231;
            this.label4.Text = "会议主题：";
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = true;
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
            this.colMeetingStyle,
            this.colMeetingDate,
            this.colMeetingTopic,
            this.colMeetingRemark,
            this.colCreatePersonName,
            this.colRealOperationDate});
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
            this.dgDetail.Location = new System.Drawing.Point(3, 55);
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
            this.dgDetail.Size = new System.Drawing.Size(896, 445);
            this.dgDetail.TabIndex = 7;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colMeetingStyle
            // 
            this.colMeetingStyle.HeaderText = "会议类型";
            this.colMeetingStyle.Name = "colMeetingStyle";
            this.colMeetingStyle.Width = 150;
            // 
            // colMeetingDate
            // 
            this.colMeetingDate.HeaderText = "会议时间";
            this.colMeetingDate.Name = "colMeetingDate";
            this.colMeetingDate.Width = 150;
            // 
            // colMeetingTopic
            // 
            this.colMeetingTopic.HeaderText = "会议主题";
            this.colMeetingTopic.Name = "colMeetingTopic";
            this.colMeetingTopic.Width = 150;
            // 
            // colMeetingRemark
            // 
            this.colMeetingRemark.HeaderText = "会议说明";
            this.colMeetingRemark.Name = "colMeetingRemark";
            this.colMeetingRemark.Width = 150;
            // 
            // colCreatePersonName
            // 
            this.colCreatePersonName.HeaderText = "制单人";
            this.colCreatePersonName.Name = "colCreatePersonName";
            this.colCreatePersonName.Width = 150;
            // 
            // colRealOperationDate
            // 
            this.colRealOperationDate.HeaderText = "制单时间";
            this.colRealOperationDate.Name = "colRealOperationDate";
            this.colRealOperationDate.Width = 150;
            // 
            // colId
            // 
            this.colId.HeaderText = "序号";
            this.colId.Name = "colId";
            this.colId.Width = 110;
            // 
            // VMeetingQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 503);
            this.Name = "VMeetingQuery";
            this.Text = "会议纪要查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DateTimePicker dtpCreateDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private System.Windows.Forms.DateTimePicker dtpCreateDateEnd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox txtMeetingStyle;
        private System.Windows.Forms.Label label2;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMeetingTopic;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMeetingStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMeetingDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMeetingTopic;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMeetingRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePersonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperationDate;
    }
}