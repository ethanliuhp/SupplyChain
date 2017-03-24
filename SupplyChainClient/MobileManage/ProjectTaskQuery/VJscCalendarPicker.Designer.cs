namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    partial class VJscCalendarPicker
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DateTimeGrid = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNextMonth = new System.Windows.Forms.Button();
            this.btnLastMonth = new System.Windows.Forms.Button();
            this.btnNextYear = new System.Windows.Forms.Button();
            this.btnLastYear = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cBox_Month = new System.Windows.Forms.ComboBox();
            this.cBox_Year = new System.Windows.Forms.ComboBox();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DateTimeGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Size = new System.Drawing.Size(310, 40);
            this.lblHeaderLine.Text = "选择时间";
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.btnReturn);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.DateTimeGrid);
            this.pnlFloor.Controls.Add(this.label2);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.cBox_Month);
            this.pnlFloor.Controls.Add(this.cBox_Year);
            this.pnlFloor.Size = new System.Drawing.Size(312, 446);
            this.pnlFloor.Controls.SetChildIndex(this.cBox_Year, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cBox_Month, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.DateTimeGrid, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnReturn, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(130, 20);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "月";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "年";
            // 
            // DateTimeGrid
            // 
            this.DateTimeGrid.AllowUserToAddRows = false;
            this.DateTimeGrid.AllowUserToDeleteRows = false;
            this.DateTimeGrid.AllowUserToOrderColumns = true;
            this.DateTimeGrid.AllowUserToResizeColumns = false;
            this.DateTimeGrid.AllowUserToResizeRows = false;
            this.DateTimeGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DateTimeGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DateTimeGrid.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.DateTimeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DateTimeGrid.GridColor = System.Drawing.SystemColors.Control;
            this.DateTimeGrid.Location = new System.Drawing.Point(9, 147);
            this.DateTimeGrid.MultiSelect = false;
            this.DateTimeGrid.Name = "DateTimeGrid";
            this.DateTimeGrid.ReadOnly = true;
            this.DateTimeGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.DateTimeGrid.RowHeadersVisible = false;
            this.DateTimeGrid.RowTemplate.Height = 23;
            this.DateTimeGrid.ShowCellErrors = false;
            this.DateTimeGrid.ShowCellToolTips = false;
            this.DateTimeGrid.ShowEditingIcon = false;
            this.DateTimeGrid.ShowRowErrors = false;
            this.DateTimeGrid.Size = new System.Drawing.Size(292, 210);
            this.DateTimeGrid.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnNextMonth);
            this.groupBox1.Controls.Add(this.btnLastMonth);
            this.groupBox1.Controls.Add(this.btnNextYear);
            this.groupBox1.Controls.Add(this.btnLastYear);
            this.groupBox1.Location = new System.Drawing.Point(7, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 56);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "日历表的控制";
            // 
            // btnNextMonth
            // 
            this.btnNextMonth.Location = new System.Drawing.Point(233, 22);
            this.btnNextMonth.Name = "btnNextMonth";
            this.btnNextMonth.Size = new System.Drawing.Size(53, 23);
            this.btnNextMonth.TabIndex = 3;
            this.btnNextMonth.Text = "下个月";
            this.btnNextMonth.UseVisualStyleBackColor = true;
            // 
            // btnLastMonth
            // 
            this.btnLastMonth.Location = new System.Drawing.Point(158, 22);
            this.btnLastMonth.Name = "btnLastMonth";
            this.btnLastMonth.Size = new System.Drawing.Size(53, 23);
            this.btnLastMonth.TabIndex = 2;
            this.btnLastMonth.Text = "上个月";
            this.btnLastMonth.UseVisualStyleBackColor = true;
            // 
            // btnNextYear
            // 
            this.btnNextYear.Location = new System.Drawing.Point(83, 22);
            this.btnNextYear.Name = "btnNextYear";
            this.btnNextYear.Size = new System.Drawing.Size(53, 23);
            this.btnNextYear.TabIndex = 1;
            this.btnNextYear.Text = "下一年";
            this.btnNextYear.UseVisualStyleBackColor = true;
            // 
            // btnLastYear
            // 
            this.btnLastYear.Location = new System.Drawing.Point(8, 22);
            this.btnLastYear.Name = "btnLastYear";
            this.btnLastYear.Size = new System.Drawing.Size(53, 23);
            this.btnLastYear.TabIndex = 0;
            this.btnLastYear.Text = "上一年";
            this.btnLastYear.UseVisualStyleBackColor = true;
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(67, 366);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(53, 23);
            this.btnReturn.TabIndex = 10;
            this.btnReturn.Text = "恢复";
            this.btnReturn.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(176, 366);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(53, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cBox_Month
            // 
            this.cBox_Month.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Month.FormattingEnabled = true;
            this.cBox_Month.Location = new System.Drawing.Point(115, 50);
            this.cBox_Month.Name = "cBox_Month";
            this.cBox_Month.Size = new System.Drawing.Size(62, 20);
            this.cBox_Month.TabIndex = 5;
            // 
            // cBox_Year
            // 
            this.cBox_Year.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBox_Year.FormattingEnabled = true;
            this.cBox_Year.Location = new System.Drawing.Point(7, 50);
            this.cBox_Year.Name = "cBox_Year";
            this.cBox_Year.Size = new System.Drawing.Size(83, 20);
            this.cBox_Year.TabIndex = 4;
            // 
            // VJscCalendarPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 446);
            this.Name = "VJscCalendarPicker";
            this.Text = "VJscCalendarPicker";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DateTimeGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView DateTimeGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnNextMonth;
        private System.Windows.Forms.Button btnLastMonth;
        private System.Windows.Forms.Button btnNextYear;
        private System.Windows.Forms.Button btnLastYear;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.ComboBox cBox_Month;
        private System.Windows.Forms.ComboBox cBox_Year;
    }
}