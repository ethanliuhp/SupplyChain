namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VMainBusinessInComeReportReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMainBusinessInComeReportReport));
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblPSelect = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.reportGrid = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbMonth = new Application.Business.Erp.SupplyChain.Client.Basic.Controls.CommonMonth();
            this.cmbYear = new Application.Business.Erp.SupplyChain.Client.Basic.Controls.CommonYear();
            this.chkOnlyOne = new System.Windows.Forms.CheckBox();
            this.pnlFloor.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.chkOnlyOne);
            this.pnlFloor.Controls.Add(this.cmbYear);
            this.pnlFloor.Controls.Add(this.cmbMonth);
            this.pnlFloor.Controls.Add(this.customLabel2);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.btnOperationOrg);
            this.pnlFloor.Controls.Add(this.txtOperationOrg);
            this.pnlFloor.Controls.Add(this.lblPSelect);
            this.pnlFloor.Controls.Add(this.btnExcel);
            this.pnlFloor.Controls.Add(this.btnSearch);
            this.pnlFloor.Size = new System.Drawing.Size(991, 495);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSearch, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExcel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblPSelect, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cmbMonth, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cmbYear, 0);
            this.pnlFloor.Controls.SetChildIndex(this.chkOnlyOne, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 43);
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(610, 12);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 193;
            this.btnOperationOrg.Text = "选择";
            this.btnOperationOrg.UseVisualStyleBackColor = true;
            // 
            // txtOperationOrg
            // 
            this.txtOperationOrg.BackColor = System.Drawing.SystemColors.Control;
            this.txtOperationOrg.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOperationOrg.DrawSelf = false;
            this.txtOperationOrg.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOperationOrg.EnterToTab = false;
            this.txtOperationOrg.Location = new System.Drawing.Point(485, 15);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(124, 16);
            this.txtOperationOrg.TabIndex = 192;
            // 
            // lblPSelect
            // 
            this.lblPSelect.AutoSize = true;
            this.lblPSelect.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPSelect.Location = new System.Drawing.Point(424, 17);
            this.lblPSelect.Name = "lblPSelect";
            this.lblPSelect.Size = new System.Drawing.Size(59, 12);
            this.lblPSelect.TabIndex = 191;
            this.lblPSelect.Text = "范围选择:";
            this.lblPSelect.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(759, 9);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 190;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(675, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 28);
            this.btnSearch.TabIndex = 189;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.reportGrid);
            this.groupBox2.Location = new System.Drawing.Point(0, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(991, 456);
            this.groupBox2.TabIndex = 194;
            this.groupBox2.TabStop = false;
            // 
            // reportGrid
            // 
            this.reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.reportGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("reportGrid.CheckedImage")));
            this.reportGrid.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.reportGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportGrid.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.reportGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.reportGrid.Location = new System.Drawing.Point(3, 17);
            this.reportGrid.Name = "reportGrid";
            this.reportGrid.Size = new System.Drawing.Size(985, 436);
            this.reportGrid.TabIndex = 10;
            this.reportGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("reportGrid.UncheckedImage")));
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(17, 16);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(35, 12);
            this.customLabel1.TabIndex = 195;
            this.customLabel1.Text = "年份:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(192, 16);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(35, 12);
            this.customLabel2.TabIndex = 196;
            this.customLabel2.Text = "月份:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbMonth
            // 
            this.cmbMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(233, 12);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(88, 20);
            this.cmbMonth.TabIndex = 197;
            // 
            // cmbYear
            // 
            this.cmbYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(58, 12);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(96, 20);
            this.cmbYear.TabIndex = 198;
            // 
            // chkOnlyOne
            // 
            this.chkOnlyOne.AutoSize = true;
            this.chkOnlyOne.Location = new System.Drawing.Point(328, 14);
            this.chkOnlyOne.Name = "chkOnlyOne";
            this.chkOnlyOne.Size = new System.Drawing.Size(48, 16);
            this.chkOnlyOne.TabIndex = 199;
            this.chkOnlyOne.Text = "当月";
            this.chkOnlyOne.UseVisualStyleBackColor = true;
            // 
            // VMainBusinessInComeReportReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 495);
            this.Name = "VMainBusinessInComeReportReport";
            this.Text = "主营业务收入台账";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private Application.Business.Erp.SupplyChain.Client.Basic.Controls.CommonYear cmbYear;
        private Application.Business.Erp.SupplyChain.Client.Basic.Controls.CommonMonth cmbMonth;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid reportGrid;
        private System.Windows.Forms.CheckBox chkOnlyOne;
    }
}