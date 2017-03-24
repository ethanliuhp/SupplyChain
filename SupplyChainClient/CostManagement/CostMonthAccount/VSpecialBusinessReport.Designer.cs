namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostBusinessAccount
{
    partial class VSpecialBusinessReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSpecialBusinessReport));
            this.cmbProject = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel24 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabBusinessReport = new System.Windows.Forms.TabControl();
            this.tabOrder = new System.Windows.Forms.TabPage();
            this.fGrid_Order = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tabIndicate = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fGrid_Indicate = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.cmbType = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.cboFiscalMonth = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblMonth = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSelectGWBS = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtAccountRootNode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.pnlFloor.SuspendLayout();
            this.tabBusinessReport.SuspendLayout();
            this.tabOrder.SuspendLayout();
            this.tabIndicate.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Controls.Add(this.btnSelectGWBS);
            this.pnlFloor.Controls.Add(this.txtAccountRootNode);
            this.pnlFloor.Controls.Add(this.lblMonth);
            this.pnlFloor.Controls.Add(this.customLabel4);
            this.pnlFloor.Controls.Add(this.cmbYear);
            this.pnlFloor.Controls.Add(this.cboFiscalMonth);
            this.pnlFloor.Controls.Add(this.cmbType);
            this.pnlFloor.Controls.Add(this.btnExcel);
            this.pnlFloor.Controls.Add(this.tabBusinessReport);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Controls.Add(this.customLabel24);
            this.pnlFloor.Controls.Add(this.cmbProject);
            this.pnlFloor.Size = new System.Drawing.Size(1050, 509);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cmbProject, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel24, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.tabBusinessReport, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExcel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cmbType, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cboFiscalMonth, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cmbYear, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel4, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblMonth, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtAccountRootNode, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSelectGWBS, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 12);
            // 
            // cmbProject
            // 
            this.cmbProject.BackColor = System.Drawing.SystemColors.Control;
            this.cmbProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProject.FormattingEnabled = true;
            this.cmbProject.Location = new System.Drawing.Point(76, 13);
            this.cmbProject.Name = "cmbProject";
            this.cmbProject.Size = new System.Drawing.Size(173, 20);
            this.cmbProject.TabIndex = 4;
            // 
            // customLabel24
            // 
            this.customLabel24.AutoSize = true;
            this.customLabel24.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel24.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel24.Location = new System.Drawing.Point(16, 16);
            this.customLabel24.Name = "customLabel24";
            this.customLabel24.Size = new System.Drawing.Size(59, 12);
            this.customLabel24.TabIndex = 115;
            this.customLabel24.Text = "项目名称:";
            this.customLabel24.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabBusinessReport
            // 
            this.tabBusinessReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabBusinessReport.Controls.Add(this.tabOrder);
            this.tabBusinessReport.Controls.Add(this.tabIndicate);
            this.tabBusinessReport.Location = new System.Drawing.Point(12, 37);
            this.tabBusinessReport.Name = "tabBusinessReport";
            this.tabBusinessReport.SelectedIndex = 0;
            this.tabBusinessReport.Size = new System.Drawing.Size(1035, 469);
            this.tabBusinessReport.TabIndex = 149;
            // 
            // tabOrder
            // 
            this.tabOrder.Controls.Add(this.fGrid_Order);
            this.tabOrder.Location = new System.Drawing.Point(4, 21);
            this.tabOrder.Name = "tabOrder";
            this.tabOrder.Padding = new System.Windows.Forms.Padding(3);
            this.tabOrder.Size = new System.Drawing.Size(1027, 444);
            this.tabOrder.TabIndex = 0;
            this.tabOrder.Text = "合同签报情况表";
            this.tabOrder.UseVisualStyleBackColor = true;
            // 
            // fGrid_Order
            // 
            this.fGrid_Order.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGrid_Order.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGrid_Order.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGrid_Order.CheckedImage")));
            this.fGrid_Order.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.fGrid_Order.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGrid_Order.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGrid_Order.Location = new System.Drawing.Point(3, 1);
            this.fGrid_Order.Name = "fGrid_Order";
            this.fGrid_Order.Size = new System.Drawing.Size(1021, 441);
            this.fGrid_Order.TabIndex = 8;
            this.fGrid_Order.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGrid_Order.UncheckedImage")));
            // 
            // tabIndicate
            // 
            this.tabIndicate.Controls.Add(this.panel1);
            this.tabIndicate.Location = new System.Drawing.Point(4, 21);
            this.tabIndicate.Name = "tabIndicate";
            this.tabIndicate.Padding = new System.Windows.Forms.Padding(3);
            this.tabIndicate.Size = new System.Drawing.Size(366, 218);
            this.tabIndicate.TabIndex = 1;
            this.tabIndicate.Text = "消耗指标情况统计表";
            this.tabIndicate.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fGrid_Indicate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 212);
            this.panel1.TabIndex = 7;
            // 
            // fGrid_Indicate
            // 
            this.fGrid_Indicate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGrid_Indicate.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGrid_Indicate.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGrid_Indicate.CheckedImage")));
            this.fGrid_Indicate.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.fGrid_Indicate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGrid_Indicate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGrid_Indicate.Location = new System.Drawing.Point(0, 0);
            this.fGrid_Indicate.Name = "fGrid_Indicate";
            this.fGrid_Indicate.Size = new System.Drawing.Size(360, 213);
            this.fGrid_Indicate.TabIndex = 8;
            this.fGrid_Indicate.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGrid_Indicate.UncheckedImage")));
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(806, 9);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 14;
            this.btnExcel.Text = "导出Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(723, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 28);
            this.btnQuery.TabIndex = 13;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // cmbType
            // 
            this.cmbType.BackColor = System.Drawing.SystemColors.Control;
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "",
            "合同签报情况表",
            "项目责任成本消耗指标情况表"});
            this.cmbType.Location = new System.Drawing.Point(255, 0);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(127, 20);
            this.cmbType.TabIndex = 151;
            this.cmbType.Visible = false;
            // 
            // cmbYear
            // 
            this.cmbYear.BackColor = System.Drawing.SystemColors.Control;
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(304, 13);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(66, 20);
            this.cmbYear.TabIndex = 174;
            // 
            // cboFiscalMonth
            // 
            this.cboFiscalMonth.BackColor = System.Drawing.SystemColors.Control;
            this.cboFiscalMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiscalMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFiscalMonth.FormattingEnabled = true;
            this.cboFiscalMonth.Location = new System.Drawing.Point(408, 13);
            this.cboFiscalMonth.Name = "cboFiscalMonth";
            this.cboFiscalMonth.Size = new System.Drawing.Size(41, 20);
            this.cboFiscalMonth.TabIndex = 173;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(268, 18);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(35, 12);
            this.customLabel4.TabIndex = 175;
            this.customLabel4.Text = "年份:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMonth.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblMonth.Location = new System.Drawing.Point(371, 17);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(35, 12);
            this.lblMonth.TabIndex = 176;
            this.lblMonth.Text = "月份:";
            this.lblMonth.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(457, 19);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 177;
            this.customLabel1.Text = "核算节点:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSelectGWBS
            // 
            this.btnSelectGWBS.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectGWBS.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectGWBS.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSelectGWBS.Location = new System.Drawing.Point(657, 12);
            this.btnSelectGWBS.Name = "btnSelectGWBS";
            this.btnSelectGWBS.Size = new System.Drawing.Size(46, 22);
            this.btnSelectGWBS.TabIndex = 179;
            this.btnSelectGWBS.Text = "选择";
            this.btnSelectGWBS.UseVisualStyleBackColor = true;
            // 
            // txtAccountRootNode
            // 
            this.txtAccountRootNode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtAccountRootNode.DrawSelf = false;
            this.txtAccountRootNode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtAccountRootNode.EnterToTab = false;
            this.txtAccountRootNode.Location = new System.Drawing.Point(514, 15);
            this.txtAccountRootNode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtAccountRootNode.Name = "txtAccountRootNode";
            this.txtAccountRootNode.Padding = new System.Windows.Forms.Padding(1);
            this.txtAccountRootNode.ReadOnly = true;
            this.txtAccountRootNode.Size = new System.Drawing.Size(142, 16);
            this.txtAccountRootNode.TabIndex = 178;
            // 
            // VSpecialBusinessReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 509);
            this.Name = "VSpecialBusinessReport";
            this.Text = "安装商务报表";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.tabBusinessReport.ResumeLayout(false);
            this.tabOrder.ResumeLayout(false);
            this.tabIndicate.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cmbProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel24;
        private System.Windows.Forms.TabControl tabBusinessReport;
        private System.Windows.Forms.TabPage tabIndicate;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabOrder;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGrid_Order;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGrid_Indicate;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cmbType;
        private System.Windows.Forms.ComboBox cmbYear;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboFiscalMonth;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblMonth;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSelectGWBS;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtAccountRootNode;
    }
}