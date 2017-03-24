namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VDatePeriodDefineSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDatePeriodDefineSelector));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkWeek = new System.Windows.Forms.CheckBox();
            this.chkMonth = new System.Windows.Forms.CheckBox();
            this.chkQuarter = new System.Windows.Forms.CheckBox();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPeriodCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBeginDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.colEndDate = new VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.chkYears = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "年份：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "期间类型：";
            this.label2.Visible = false;
            // 
            // chkWeek
            // 
            this.chkWeek.AutoSize = true;
            this.chkWeek.Location = new System.Drawing.Point(411, 16);
            this.chkWeek.Name = "chkWeek";
            this.chkWeek.Size = new System.Drawing.Size(36, 16);
            this.chkWeek.TabIndex = 9;
            this.chkWeek.Text = "周";
            this.chkWeek.UseVisualStyleBackColor = true;
            this.chkWeek.Visible = false;
            // 
            // chkMonth
            // 
            this.chkMonth.AutoSize = true;
            this.chkMonth.Location = new System.Drawing.Point(355, 16);
            this.chkMonth.Name = "chkMonth";
            this.chkMonth.Size = new System.Drawing.Size(48, 16);
            this.chkMonth.TabIndex = 8;
            this.chkMonth.Text = "月份";
            this.chkMonth.UseVisualStyleBackColor = true;
            this.chkMonth.Visible = false;
            // 
            // chkQuarter
            // 
            this.chkQuarter.AutoSize = true;
            this.chkQuarter.Location = new System.Drawing.Point(299, 16);
            this.chkQuarter.Name = "chkQuarter";
            this.chkQuarter.Size = new System.Drawing.Size(48, 16);
            this.chkQuarter.TabIndex = 7;
            this.chkQuarter.Text = "季度";
            this.chkQuarter.UseVisualStyleBackColor = true;
            this.chkQuarter.Visible = false;
            // 
            // dgMaster
            // 
            this.dgMaster.AddDefaultMenu = false;
            this.dgMaster.AddNoColumn = false;
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgMaster.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheck,
            this.colPeriodCode,
            this.colPeriodName,
            this.colBeginDate,
            this.colEndDate});
            this.dgMaster.CustomBackColor = false;
            this.dgMaster.EditCellBackColor = System.Drawing.Color.White;
            this.dgMaster.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgMaster.FreezeFirstRow = false;
            this.dgMaster.FreezeLastRow = false;
            this.dgMaster.FrontColumnCount = 0;
            this.dgMaster.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgMaster.HScrollOffset = 0;
            this.dgMaster.IsAllowOrder = true;
            this.dgMaster.IsConfirmDelete = true;
            this.dgMaster.Location = new System.Drawing.Point(146, 44);
            this.dgMaster.Name = "dgMaster";
            this.dgMaster.PageIndex = 0;
            this.dgMaster.PageSize = 0;
            this.dgMaster.Query = null;
            this.dgMaster.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgMaster.ReadOnlyCols")));
            this.dgMaster.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.RowHeadersWidth = 22;
            this.dgMaster.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMaster.RowTemplate.Height = 23;
            this.dgMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMaster.Size = new System.Drawing.Size(620, 361);
            this.dgMaster.TabIndex = 185;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colCheck
            // 
            this.colCheck.HeaderText = "";
            this.colCheck.Name = "colCheck";
            this.colCheck.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCheck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCheck.Width = 50;
            // 
            // colPeriodCode
            // 
            this.colPeriodCode.DataPropertyName = "FormattedPeriodCode";
            this.colPeriodCode.HeaderText = "期间编码";
            this.colPeriodCode.Name = "colPeriodCode";
            this.colPeriodCode.ReadOnly = true;
            this.colPeriodCode.Width = 120;
            // 
            // colPeriodName
            // 
            this.colPeriodName.DataPropertyName = "FormattedPeriodName";
            dataGridViewCellStyle4.NullValue = null;
            this.colPeriodName.DefaultCellStyle = dataGridViewCellStyle4;
            this.colPeriodName.HeaderText = "期间名称";
            this.colPeriodName.Name = "colPeriodName";
            this.colPeriodName.ReadOnly = true;
            this.colPeriodName.Width = 200;
            // 
            // colBeginDate
            // 
            this.colBeginDate.DataPropertyName = "BeginDate";
            dataGridViewCellStyle5.Format = "yyyy-MM-dd";
            this.colBeginDate.DefaultCellStyle = dataGridViewCellStyle5;
            this.colBeginDate.HeaderText = "开始日期";
            this.colBeginDate.Name = "colBeginDate";
            this.colBeginDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colBeginDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colBeginDate.Width = 150;
            // 
            // colEndDate
            // 
            this.colEndDate.DataPropertyName = "EndDate";
            dataGridViewCellStyle6.Format = "yyyy-MM-dd";
            dataGridViewCellStyle6.NullValue = null;
            this.colEndDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.colEndDate.HeaderText = "结束日期";
            this.colEndDate.Name = "colEndDate";
            this.colEndDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEndDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colEndDate.Width = 150;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(579, 422);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 186;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(675, 422);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 187;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(146, 411);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(48, 16);
            this.chkSelectAll.TabIndex = 188;
            this.chkSelectAll.Text = "全选";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            // 
            // chkYears
            // 
            this.chkYears.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.chkYears.CheckOnClick = true;
            this.chkYears.FormattingEnabled = true;
            this.chkYears.Location = new System.Drawing.Point(12, 32);
            this.chkYears.Name = "chkYears";
            this.chkYears.Size = new System.Drawing.Size(120, 404);
            this.chkYears.TabIndex = 189;
            // 
            // VDatePeriodDefineSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 457);
            this.Controls.Add(this.chkYears);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dgMaster);
            this.Controls.Add(this.chkWeek);
            this.Controls.Add(this.chkMonth);
            this.Controls.Add(this.chkQuarter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "VDatePeriodDefineSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择时间期间定义";
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkWeek;
        private System.Windows.Forms.CheckBox chkMonth;
        private System.Windows.Forms.CheckBox chkQuarter;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriodCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriodName;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colBeginDate;
        private VirtualMachine.Component.WinControls.Controls.DataGridViewCalendarColumn colEndDate;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.CheckedListBox chkYears;
    }
}