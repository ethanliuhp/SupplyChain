namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage
{
    partial class VCollectionQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCollectionQuery));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpCreateDateBegin = new System.Windows.Forms.DateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpCreateDateEnd = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGetUnits = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtSendUnits = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmoLettersStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmoSendStyle = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtPlanName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtLettersId = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colLettersId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLettersName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSendStyle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLettersStyle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSendLettersDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSendUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGetUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Size = new System.Drawing.Size(972, 509);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
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
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtGetUnits);
            this.groupBox1.Controls.Add(this.txtSendUnits);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmoLettersStyle);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmoSendStyle);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtPlanName);
            this.groupBox1.Controls.Add(this.txtLettersId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(966, 69);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // dtpCreateDateBegin
            // 
            this.dtpCreateDateBegin.Location = new System.Drawing.Point(518, 41);
            this.dtpCreateDateBegin.Name = "dtpCreateDateBegin";
            this.dtpCreateDateBegin.Size = new System.Drawing.Size(115, 21);
            this.dtpCreateDateBegin.TabIndex = 237;
            this.dtpCreateDateBegin.Value = new System.DateTime(2012, 7, 10, 8, 47, 0, 0);
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(639, 46);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 236;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpCreateDateEnd
            // 
            this.dtpCreateDateEnd.Location = new System.Drawing.Point(656, 41);
            this.dtpCreateDateEnd.Name = "dtpCreateDateEnd";
            this.dtpCreateDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpCreateDateEnd.TabIndex = 235;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(710, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 231;
            this.label6.Text = "收函单位：";
            // 
            // txtGetUnits
            // 
            this.txtGetUnits.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtGetUnits.DrawSelf = false;
            this.txtGetUnits.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtGetUnits.EnterToTab = false;
            this.txtGetUnits.Location = new System.Drawing.Point(781, 16);
            this.txtGetUnits.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtGetUnits.Name = "txtGetUnits";
            this.txtGetUnits.Padding = new System.Windows.Forms.Padding(1);
            this.txtGetUnits.ReadOnly = false;
            this.txtGetUnits.Size = new System.Drawing.Size(86, 16);
            this.txtGetUnits.TabIndex = 230;
            // 
            // txtSendUnits
            // 
            this.txtSendUnits.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSendUnits.DrawSelf = false;
            this.txtSendUnits.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSendUnits.EnterToTab = false;
            this.txtSendUnits.Location = new System.Drawing.Point(518, 17);
            this.txtSendUnits.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSendUnits.Name = "txtSendUnits";
            this.txtSendUnits.Padding = new System.Windows.Forms.Padding(1);
            this.txtSendUnits.ReadOnly = false;
            this.txtSendUnits.Size = new System.Drawing.Size(115, 16);
            this.txtSendUnits.TabIndex = 229;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(447, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 228;
            this.label9.Text = "发函单位：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(435, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 226;
            this.label11.Text = "收发函日期：";
            // 
            // cmoLettersStyle
            // 
            this.cmoLettersStyle.FormattingEnabled = true;
            this.cmoLettersStyle.Location = new System.Drawing.Point(77, 38);
            this.cmoLettersStyle.Name = "cmoLettersStyle";
            this.cmoLettersStyle.Size = new System.Drawing.Size(105, 20);
            this.cmoLettersStyle.TabIndex = 225;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 224;
            this.label2.Text = "函件类型：";
            // 
            // cmoSendStyle
            // 
            this.cmoSendStyle.FormattingEnabled = true;
            this.cmoSendStyle.Location = new System.Drawing.Point(298, 39);
            this.cmoSendStyle.Name = "cmoSendStyle";
            this.cmoSendStyle.Size = new System.Drawing.Size(120, 20);
            this.cmoSendStyle.TabIndex = 218;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(227, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 217;
            this.label3.Text = "收发类型：";
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(877, 38);
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
            this.btnSearch.Location = new System.Drawing.Point(796, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 211;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtPlanName
            // 
            this.txtPlanName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPlanName.DrawSelf = false;
            this.txtPlanName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPlanName.EnterToTab = false;
            this.txtPlanName.Location = new System.Drawing.Point(298, 17);
            this.txtPlanName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPlanName.Name = "txtPlanName";
            this.txtPlanName.Padding = new System.Windows.Forms.Padding(1);
            this.txtPlanName.ReadOnly = false;
            this.txtPlanName.Size = new System.Drawing.Size(120, 16);
            this.txtPlanName.TabIndex = 167;
            // 
            // txtLettersId
            // 
            this.txtLettersId.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtLettersId.DrawSelf = false;
            this.txtLettersId.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtLettersId.EnterToTab = false;
            this.txtLettersId.Location = new System.Drawing.Point(77, 17);
            this.txtLettersId.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtLettersId.Name = "txtLettersId";
            this.txtLettersId.Padding = new System.Windows.Forms.Padding(1);
            this.txtLettersId.ReadOnly = false;
            this.txtLettersId.Size = new System.Drawing.Size(113, 16);
            this.txtLettersId.TabIndex = 161;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "函件编号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(227, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "函件名称：";
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
            this.colLettersId,
            this.colLettersName,
            this.colSendStyle,
            this.colLettersStyle,
            this.colSendLettersDate,
            this.colSendUnits,
            this.colGetUnits,
            this.colRealOperationDate,
            this.colRemark});
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
            this.dgDetail.Location = new System.Drawing.Point(3, 73);
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
            this.dgDetail.Size = new System.Drawing.Size(966, 433);
            this.dgDetail.TabIndex = 6;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colLettersId
            // 
            this.colLettersId.HeaderText = "函件编号";
            this.colLettersId.Name = "colLettersId";
            this.colLettersId.Width = 110;
            // 
            // colLettersName
            // 
            this.colLettersName.HeaderText = "函件名称";
            this.colLettersName.Name = "colLettersName";
            this.colLettersName.Width = 150;
            // 
            // colSendStyle
            // 
            this.colSendStyle.HeaderText = "收发类型";
            this.colSendStyle.Name = "colSendStyle";
            this.colSendStyle.Width = 150;
            // 
            // colLettersStyle
            // 
            this.colLettersStyle.HeaderText = "函件类型";
            this.colLettersStyle.Name = "colLettersStyle";
            this.colLettersStyle.Width = 150;
            // 
            // colSendLettersDate
            // 
            this.colSendLettersDate.HeaderText = "收发时间";
            this.colSendLettersDate.Name = "colSendLettersDate";
            this.colSendLettersDate.Width = 150;
            // 
            // colSendUnits
            // 
            this.colSendUnits.HeaderText = "发函单位";
            this.colSendUnits.Name = "colSendUnits";
            this.colSendUnits.Width = 150;
            // 
            // colGetUnits
            // 
            this.colGetUnits.HeaderText = "收函单位";
            this.colGetUnits.Name = "colGetUnits";
            this.colGetUnits.Width = 150;
            // 
            // colRealOperationDate
            // 
            this.colRealOperationDate.HeaderText = "制单时间";
            this.colRealOperationDate.Name = "colRealOperationDate";
            // 
            // colRemark
            // 
            this.colRemark.HeaderText = "备注";
            this.colRemark.Name = "colRemark";
            this.colRemark.Width = 150;
            // 
            // VCollectionQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 509);
            this.Name = "VCollectionQuery";
            this.Text = "收发函查询";
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
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPlanName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtLettersId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.ComboBox cmoSendStyle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmoLettersStyle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtGetUnits;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSendUnits;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpCreateDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private System.Windows.Forms.DateTimePicker dtpCreateDateEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLettersId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLettersName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSendStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLettersStyle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSendLettersDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSendUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGetUnits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
    }
}