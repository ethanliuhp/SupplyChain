namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VSubjectAndDateSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSubjectAndDateSelector));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSubjectList = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvPeriods = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.colRowCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colWbsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radBtnWbsPeriod = new System.Windows.Forms.RadioButton();
            this.radBtnPeriodWbs = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeriods)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.chkSubjectList);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 451);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "科目列表";
            // 
            // chkSubjectList
            // 
            this.chkSubjectList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSubjectList.CheckOnClick = true;
            this.chkSubjectList.FormattingEnabled = true;
            this.chkSubjectList.Location = new System.Drawing.Point(6, 20);
            this.chkSubjectList.Name = "chkSubjectList";
            this.chkSubjectList.Size = new System.Drawing.Size(188, 420);
            this.chkSubjectList.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvPeriods);
            this.groupBox2.Location = new System.Drawing.Point(218, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(519, 451);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "任务期间列表";
            // 
            // dgvPeriods
            // 
            this.dgvPeriods.AddDefaultMenu = false;
            this.dgvPeriods.AddNoColumn = true;
            this.dgvPeriods.AllowUserToAddRows = false;
            this.dgvPeriods.AllowUserToDeleteRows = false;
            this.dgvPeriods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPeriods.BackgroundColor = System.Drawing.Color.White;
            this.dgvPeriods.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPeriods.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvPeriods.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvPeriods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPeriods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRowCheck,
            this.colWbsName,
            this.colPeriodName,
            this.colRate});
            this.dgvPeriods.CustomBackColor = false;
            this.dgvPeriods.EditCellBackColor = System.Drawing.Color.White;
            this.dgvPeriods.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvPeriods.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgvPeriods.FreezeFirstRow = false;
            this.dgvPeriods.FreezeLastRow = false;
            this.dgvPeriods.FrontColumnCount = 0;
            this.dgvPeriods.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvPeriods.HScrollOffset = 0;
            this.dgvPeriods.IsAllowOrder = true;
            this.dgvPeriods.IsConfirmDelete = true;
            this.dgvPeriods.Location = new System.Drawing.Point(6, 20);
            this.dgvPeriods.MultiSelect = false;
            this.dgvPeriods.Name = "dgvPeriods";
            this.dgvPeriods.PageIndex = 0;
            this.dgvPeriods.PageSize = 0;
            this.dgvPeriods.Query = null;
            this.dgvPeriods.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvPeriods.ReadOnlyCols")));
            this.dgvPeriods.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvPeriods.RowHeadersWidth = 22;
            this.dgvPeriods.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvPeriods.RowTemplate.Height = 23;
            this.dgvPeriods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPeriods.Size = new System.Drawing.Size(507, 425);
            this.dgvPeriods.TabIndex = 196;
            this.dgvPeriods.TargetType = null;
            this.dgvPeriods.VScrollOffset = 0;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(662, 472);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(567, 472);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "重新选择";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(224, 468);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(48, 16);
            this.chkSelectAll.TabIndex = 5;
            this.chkSelectAll.Text = "全选";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            // 
            // colRowCheck
            // 
            this.colRowCheck.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
            this.colRowCheck.HeaderText = "";
            this.colRowCheck.MinimumWidth = 30;
            this.colRowCheck.Name = "colRowCheck";
            this.colRowCheck.Width = 30;
            // 
            // colWbsName
            // 
            this.colWbsName.DataPropertyName = "WBSName";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.colWbsName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colWbsName.HeaderText = "任务名称";
            this.colWbsName.Name = "colWbsName";
            this.colWbsName.ReadOnly = true;
            this.colWbsName.Width = 180;
            // 
            // colPeriodName
            // 
            this.colPeriodName.DataPropertyName = "PeriodName";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.colPeriodName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colPeriodName.HeaderText = "期间名称";
            this.colPeriodName.Name = "colPeriodName";
            this.colPeriodName.ReadOnly = true;
            this.colPeriodName.Width = 130;
            // 
            // colRate
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colRate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colRate.HeaderText = "累计进度(%)";
            this.colRate.Name = "colRate";
            // 
            // radBtnWbsPeriod
            // 
            this.radBtnWbsPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radBtnWbsPeriod.AutoSize = true;
            this.radBtnWbsPeriod.Location = new System.Drawing.Point(286, 468);
            this.radBtnWbsPeriod.Name = "radBtnWbsPeriod";
            this.radBtnWbsPeriod.Size = new System.Drawing.Size(119, 16);
            this.radBtnWbsPeriod.TabIndex = 6;
            this.radBtnWbsPeriod.TabStop = true;
            this.radBtnWbsPeriod.Text = "按任务->期间排序";
            this.radBtnWbsPeriod.UseVisualStyleBackColor = true;
            // 
            // radBtnPeriodWbs
            // 
            this.radBtnPeriodWbs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radBtnPeriodWbs.AutoSize = true;
            this.radBtnPeriodWbs.Location = new System.Drawing.Point(419, 468);
            this.radBtnPeriodWbs.Name = "radBtnPeriodWbs";
            this.radBtnPeriodWbs.Size = new System.Drawing.Size(119, 16);
            this.radBtnPeriodWbs.TabIndex = 7;
            this.radBtnPeriodWbs.TabStop = true;
            this.radBtnPeriodWbs.Text = "按期间->任务排序";
            this.radBtnPeriodWbs.UseVisualStyleBackColor = true;
            // 
            // VSubjectAndDateSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 507);
            this.Controls.Add(this.radBtnPeriodWbs);
            this.Controls.Add(this.radBtnWbsPeriod);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "VSubjectAndDateSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "科目及时间设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeriods)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOk;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvPeriods;
        private System.Windows.Forms.CheckedListBox chkSubjectList;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colRowCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWbsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRate;
        private System.Windows.Forms.RadioButton radBtnWbsPeriod;
        private System.Windows.Forms.RadioButton radBtnPeriodWbs;
    }
}