namespace Application.Business.Erp.SupplyChain.Client.StockMng.Report
{
    partial class LJReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LJReport));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboFiscalMonth = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.cboFiscalYear = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupplier = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbReportName = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customLabel24 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbProject = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.reportGrid = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.txtUsePart = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnSelectUsePart = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.lblUsePart = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.reportGrid);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(976, 426);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.reportGrid, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtUsePart);
            this.groupBox1.Controls.Add(this.btnSelectUsePart);
            this.groupBox1.Controls.Add(this.lblUsePart);
            this.groupBox1.Controls.Add(this.cboFiscalMonth);
            this.groupBox1.Controls.Add(this.cboFiscalYear);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.txtSupplier);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.cmbReportName);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.customLabel24);
            this.groupBox1.Controls.Add(this.cmbProject);
            this.groupBox1.Location = new System.Drawing.Point(9, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(955, 62);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // cboFiscalMonth
            // 
            this.cboFiscalMonth.BackColor = System.Drawing.SystemColors.Control;
            this.cboFiscalMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFiscalMonth.FormattingEnabled = true;
            this.cboFiscalMonth.Location = new System.Drawing.Point(449, 12);
            this.cboFiscalMonth.Name = "cboFiscalMonth";
            this.cboFiscalMonth.Size = new System.Drawing.Size(41, 20);
            this.cboFiscalMonth.TabIndex = 152;
            // 
            // cboFiscalYear
            // 
            this.cboFiscalYear.BackColor = System.Drawing.SystemColors.Control;
            this.cboFiscalYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFiscalYear.FormattingEnabled = true;
            this.cboFiscalYear.Location = new System.Drawing.Point(379, 11);
            this.cboFiscalYear.Name = "cboFiscalYear";
            this.cboFiscalYear.Size = new System.Drawing.Size(64, 20);
            this.cboFiscalYear.TabIndex = 151;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(326, 16);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(47, 12);
            this.customLabel3.TabIndex = 150;
            this.customLabel3.Text = "会计期:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(501, 15);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(47, 12);
            this.customLabel1.TabIndex = 149;
            this.customLabel1.Text = "出租方:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSupplier
            // 
            this.txtSupplier.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupplier.Code = null;
            this.txtSupplier.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtSupplier.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtSupplier.EnterToTab = false;
            this.txtSupplier.Id = "";
            this.txtSupplier.Location = new System.Drawing.Point(552, 10);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Result = ((System.Collections.IList)(resources.GetObject("txtSupplier.Result")));
            this.txtSupplier.RightMouse = false;
            this.txtSupplier.Size = new System.Drawing.Size(265, 21);
            this.txtSupplier.TabIndex = 148;
            this.txtSupplier.Tag = null;
            this.txtSupplier.Value = "";
            this.txtSupplier.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(6, 43);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 122;
            this.customLabel2.Text = "报表名称:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbReportName
            // 
            this.cmbReportName.BackColor = System.Drawing.SystemColors.Control;
            this.cmbReportName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbReportName.FormattingEnabled = true;
            this.cmbReportName.Location = new System.Drawing.Point(71, 38);
            this.cmbReportName.Name = "cmbReportName";
            this.cmbReportName.Size = new System.Drawing.Size(236, 20);
            this.cmbReportName.TabIndex = 121;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(725, 32);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 120;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(648, 33);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 119;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // customLabel24
            // 
            this.customLabel24.AutoSize = true;
            this.customLabel24.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel24.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel24.Location = new System.Drawing.Point(6, 17);
            this.customLabel24.Name = "customLabel24";
            this.customLabel24.Size = new System.Drawing.Size(59, 12);
            this.customLabel24.TabIndex = 117;
            this.customLabel24.Text = "项目名称:";
            this.customLabel24.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbProject
            // 
            this.cmbProject.BackColor = System.Drawing.SystemColors.Control;
            this.cmbProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProject.FormattingEnabled = true;
            this.cmbProject.Location = new System.Drawing.Point(71, 12);
            this.cmbProject.Name = "cmbProject";
            this.cmbProject.Size = new System.Drawing.Size(236, 20);
            this.cmbProject.TabIndex = 116;
            // 
            // reportGrid
            // 
            this.reportGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.reportGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("reportGrid.CheckedImage")));
            this.reportGrid.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.reportGrid.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.reportGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.reportGrid.Location = new System.Drawing.Point(9, 72);
            this.reportGrid.Name = "reportGrid";
            this.reportGrid.Size = new System.Drawing.Size(955, 342);
            this.reportGrid.TabIndex = 6;
            this.reportGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("reportGrid.UncheckedImage")));
            // 
            // txtUsePart
            // 
            this.txtUsePart.BackColor = System.Drawing.SystemColors.Control;
            this.txtUsePart.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtUsePart.DrawSelf = false;
            this.txtUsePart.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtUsePart.EnterToTab = false;
            this.txtUsePart.Location = new System.Drawing.Point(379, 42);
            this.txtUsePart.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtUsePart.Name = "txtUsePart";
            this.txtUsePart.Padding = new System.Windows.Forms.Padding(1);
            this.txtUsePart.ReadOnly = true;
            this.txtUsePart.Size = new System.Drawing.Size(88, 16);
            this.txtUsePart.TabIndex = 157;
            // 
            // btnSelectUsePart
            // 
            this.btnSelectUsePart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectUsePart.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectUsePart.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSelectUsePart.Location = new System.Drawing.Point(473, 35);
            this.btnSelectUsePart.Name = "btnSelectUsePart";
            this.btnSelectUsePart.Size = new System.Drawing.Size(41, 23);
            this.btnSelectUsePart.TabIndex = 158;
            this.btnSelectUsePart.Text = "选择";
            this.btnSelectUsePart.UseVisualStyleBackColor = true;
            // 
            // lblUsePart
            // 
            this.lblUsePart.AutoSize = true;
            this.lblUsePart.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUsePart.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblUsePart.Location = new System.Drawing.Point(320, 46);
            this.lblUsePart.Name = "lblUsePart";
            this.lblUsePart.Size = new System.Drawing.Size(59, 12);
            this.lblUsePart.TabIndex = 156;
            this.lblUsePart.Text = "使用部位:";
            this.lblUsePart.UnderLineColor = System.Drawing.Color.Red;
            // 
            // LJReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 426);
            this.Name = "LJReport";
            this.Text = "料具租赁月报";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupplier)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel24;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cmbProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cmbReportName;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid reportGrid;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupplier;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboFiscalMonth;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboFiscalYear;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtUsePart;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSelectUsePart;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblUsePart;
    }
}