namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    partial class VPlanDeclare
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VPlanDeclare));
            this.txtState = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tspMenuInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.tspMenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tspMenuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.tPOfficeExpend = new System.Windows.Forms.TabPage();
            this.gdOfficeExpend = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpOtherExpend = new System.Windows.Forms.TabPage();
            this.gdOtherExpend = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpProjectExpend = new System.Windows.Forms.TabPage();
            this.gdProjectExpend = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpTotalExpend1 = new System.Windows.Forms.TabPage();
            this.gdTotalExpend1 = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpTotalExpend2 = new System.Windows.Forms.TabPage();
            this.gdTotalExpend2 = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpProjectReport = new System.Windows.Forms.TabPage();
            this.gdProjectReport = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tpCompanyReport = new System.Windows.Forms.TabPage();
            this.gdCompanyReport = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnGetData = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlBody.SuspendLayout();
            this.pnlFloor.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tPOfficeExpend.SuspendLayout();
            this.tpOtherExpend.SuspendLayout();
            this.tpProjectExpend.SuspendLayout();
            this.tpTotalExpend1.SuspendLayout();
            this.tpTotalExpend2.SuspendLayout();
            this.tpProjectReport.SuspendLayout();
            this.tpCompanyReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.tabControl);
            this.pnlBody.Location = new System.Drawing.Point(0, 40);
            this.pnlBody.Size = new System.Drawing.Size(917, 414);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Location = new System.Drawing.Point(0, 454);
            this.pnlFooter.Size = new System.Drawing.Size(917, 38);
            this.pnlFooter.Visible = false;
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(917, 0);
            // 
            // pnlFloor
            // 
            this.pnlFloor.Size = new System.Drawing.Size(917, 492);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnGetData);
            this.pnlHeader.Controls.Add(this.txtState);
            this.pnlHeader.Controls.Add(this.label5);
            this.pnlHeader.Controls.Add(this.label4);
            this.pnlHeader.Controls.Add(this.cmbMonth);
            this.pnlHeader.Controls.Add(this.label3);
            this.pnlHeader.Controls.Add(this.cmbYear);
            this.pnlHeader.Controls.Add(this.label2);
            this.pnlHeader.Controls.Add(this.txtCode);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Size = new System.Drawing.Size(917, 40);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.label1, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtCode, 0);
            this.pnlHeader.Controls.SetChildIndex(this.label2, 0);
            this.pnlHeader.Controls.SetChildIndex(this.cmbYear, 0);
            this.pnlHeader.Controls.SetChildIndex(this.label3, 0);
            this.pnlHeader.Controls.SetChildIndex(this.cmbMonth, 0);
            this.pnlHeader.Controls.SetChildIndex(this.label4, 0);
            this.pnlHeader.Controls.SetChildIndex(this.label5, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtState, 0);
            this.pnlHeader.Controls.SetChildIndex(this.btnGetData, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 4);
            this.lblTitle.Visible = false;
            // 
            // txtState
            // 
            this.txtState.BackColor = System.Drawing.SystemColors.Control;
            this.txtState.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtState.DrawSelf = false;
            this.txtState.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtState.EnterToTab = false;
            this.txtState.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtState.ForeColor = System.Drawing.Color.Red;
            this.txtState.Location = new System.Drawing.Point(627, 12);
            this.txtState.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtState.Name = "txtState";
            this.txtState.Padding = new System.Windows.Forms.Padding(1);
            this.txtState.ReadOnly = true;
            this.txtState.Size = new System.Drawing.Size(117, 16);
            this.txtState.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(568, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 25;
            this.label5.Text = "计划状态：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(540, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "月";
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.ItemHeight = 12;
            this.cmbMonth.Location = new System.Drawing.Point(469, 10);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(70, 20);
            this.cmbMonth.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(451, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "年";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.ItemHeight = 12;
            this.cmbYear.Location = new System.Drawing.Point(380, 10);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(70, 20);
            this.cmbYear.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(321, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "计划期间：";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(99, 12);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(208, 16);
            this.txtCode.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "资金计划单号：";
            // 
            // tabControl
            // 
            this.tabControl.ContextMenuStrip = this.contextMenuStrip1;
            this.tabControl.Controls.Add(this.tPOfficeExpend);
            this.tabControl.Controls.Add(this.tpOtherExpend);
            this.tabControl.Controls.Add(this.tpProjectExpend);
            this.tabControl.Controls.Add(this.tpTotalExpend1);
            this.tabControl.Controls.Add(this.tpTotalExpend2);
            this.tabControl.Controls.Add(this.tpProjectReport);
            this.tabControl.Controls.Add(this.tpCompanyReport);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(6, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(905, 414);
            this.tabControl.TabIndex = 19;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspMenuInsert,
            this.tspMenuDelete,
            this.tspMenuRefresh});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(130, 70);
            // 
            // tspMenuInsert
            // 
            this.tspMenuInsert.Name = "tspMenuInsert";
            this.tspMenuInsert.Size = new System.Drawing.Size(129, 22);
            this.tspMenuInsert.Text = "插入行(&I)";
            this.tspMenuInsert.ToolTipText = "在焦点单元格上插入选择行数的行";
            // 
            // tspMenuDelete
            // 
            this.tspMenuDelete.Name = "tspMenuDelete";
            this.tspMenuDelete.Size = new System.Drawing.Size(129, 22);
            this.tspMenuDelete.Text = "删除行(&D)";
            this.tspMenuDelete.ToolTipText = "删除所有选择的行";
            // 
            // tspMenuRefresh
            // 
            this.tspMenuRefresh.Name = "tspMenuRefresh";
            this.tspMenuRefresh.Size = new System.Drawing.Size(129, 22);
            this.tspMenuRefresh.Text = "刷新(&R)";
            // 
            // tPOfficeExpend
            // 
            this.tPOfficeExpend.Controls.Add(this.gdOfficeExpend);
            this.tPOfficeExpend.Location = new System.Drawing.Point(4, 22);
            this.tPOfficeExpend.Name = "tPOfficeExpend";
            this.tPOfficeExpend.Size = new System.Drawing.Size(897, 388);
            this.tPOfficeExpend.TabIndex = 3;
            this.tPOfficeExpend.Tag = "机关资金计划支出明细表";
            this.tPOfficeExpend.Text = "机关资金计划支出明细表";
            this.tPOfficeExpend.UseVisualStyleBackColor = true;
            // 
            // gdOfficeExpend
            // 
            this.gdOfficeExpend.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdOfficeExpend.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdOfficeExpend.CheckedImage")));
            this.gdOfficeExpend.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdOfficeExpend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdOfficeExpend.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdOfficeExpend.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdOfficeExpend.Location = new System.Drawing.Point(0, 0);
            this.gdOfficeExpend.Name = "gdOfficeExpend";
            this.gdOfficeExpend.Size = new System.Drawing.Size(897, 388);
            this.gdOfficeExpend.TabIndex = 234;
            this.gdOfficeExpend.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdOfficeExpend.UncheckedImage")));
            // 
            // tpOtherExpend
            // 
            this.tpOtherExpend.Controls.Add(this.gdOtherExpend);
            this.tpOtherExpend.Location = new System.Drawing.Point(4, 22);
            this.tpOtherExpend.Name = "tpOtherExpend";
            this.tpOtherExpend.Padding = new System.Windows.Forms.Padding(3);
            this.tpOtherExpend.Size = new System.Drawing.Size(614, 221);
            this.tpOtherExpend.TabIndex = 0;
            this.tpOtherExpend.Tag = "项目资金支付计划申报其他支出明细表";
            this.tpOtherExpend.Text = "项目资金计划其他支出明细表";
            this.tpOtherExpend.UseVisualStyleBackColor = true;
            // 
            // gdOtherExpend
            // 
            this.gdOtherExpend.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdOtherExpend.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdOtherExpend.CheckedImage")));
            this.gdOtherExpend.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdOtherExpend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdOtherExpend.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdOtherExpend.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdOtherExpend.Location = new System.Drawing.Point(3, 3);
            this.gdOtherExpend.Name = "gdOtherExpend";
            this.gdOtherExpend.Size = new System.Drawing.Size(608, 215);
            this.gdOtherExpend.TabIndex = 235;
            this.gdOtherExpend.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdOtherExpend.UncheckedImage")));
            // 
            // tpProjectExpend
            // 
            this.tpProjectExpend.Controls.Add(this.gdProjectExpend);
            this.tpProjectExpend.Location = new System.Drawing.Point(4, 22);
            this.tpProjectExpend.Name = "tpProjectExpend";
            this.tpProjectExpend.Padding = new System.Windows.Forms.Padding(3);
            this.tpProjectExpend.Size = new System.Drawing.Size(897, 388);
            this.tpProjectExpend.TabIndex = 1;
            this.tpProjectExpend.Tag = "项目资金支付计划申报明细表";
            this.tpProjectExpend.Text = "资金计划申报明细表";
            this.tpProjectExpend.UseVisualStyleBackColor = true;
            // 
            // gdProjectExpend
            // 
            this.gdProjectExpend.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdProjectExpend.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectExpend.CheckedImage")));
            this.gdProjectExpend.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdProjectExpend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdProjectExpend.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdProjectExpend.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdProjectExpend.Location = new System.Drawing.Point(3, 3);
            this.gdProjectExpend.Name = "gdProjectExpend";
            this.gdProjectExpend.Size = new System.Drawing.Size(891, 382);
            this.gdProjectExpend.TabIndex = 235;
            this.gdProjectExpend.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectExpend.UncheckedImage")));
            // 
            // tpTotalExpend1
            // 
            this.tpTotalExpend1.Controls.Add(this.gdTotalExpend1);
            this.tpTotalExpend1.Location = new System.Drawing.Point(4, 22);
            this.tpTotalExpend1.Name = "tpTotalExpend1";
            this.tpTotalExpend1.Size = new System.Drawing.Size(614, 221);
            this.tpTotalExpend1.TabIndex = 2;
            this.tpTotalExpend1.Tag = "单位资金支付计划申报汇总表";
            this.tpTotalExpend1.Text = "资金计划申报汇总表1";
            this.tpTotalExpend1.UseVisualStyleBackColor = true;
            // 
            // gdTotalExpend1
            // 
            this.gdTotalExpend1.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdTotalExpend1.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdTotalExpend1.CheckedImage")));
            this.gdTotalExpend1.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdTotalExpend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdTotalExpend1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdTotalExpend1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdTotalExpend1.Location = new System.Drawing.Point(0, 0);
            this.gdTotalExpend1.Name = "gdTotalExpend1";
            this.gdTotalExpend1.Size = new System.Drawing.Size(614, 221);
            this.gdTotalExpend1.TabIndex = 235;
            this.gdTotalExpend1.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdTotalExpend1.UncheckedImage")));
            // 
            // tpTotalExpend2
            // 
            this.tpTotalExpend2.Controls.Add(this.gdTotalExpend2);
            this.tpTotalExpend2.Location = new System.Drawing.Point(4, 22);
            this.tpTotalExpend2.Name = "tpTotalExpend2";
            this.tpTotalExpend2.Size = new System.Drawing.Size(614, 221);
            this.tpTotalExpend2.TabIndex = 4;
            this.tpTotalExpend2.Tag = "月资金计划项目申报汇总表";
            this.tpTotalExpend2.Text = "资金计划申报汇总表2";
            this.tpTotalExpend2.UseVisualStyleBackColor = true;
            // 
            // gdTotalExpend2
            // 
            this.gdTotalExpend2.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdTotalExpend2.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdTotalExpend2.CheckedImage")));
            this.gdTotalExpend2.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdTotalExpend2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdTotalExpend2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdTotalExpend2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdTotalExpend2.Location = new System.Drawing.Point(0, 0);
            this.gdTotalExpend2.Name = "gdTotalExpend2";
            this.gdTotalExpend2.Size = new System.Drawing.Size(614, 221);
            this.gdTotalExpend2.TabIndex = 235;
            this.gdTotalExpend2.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdTotalExpend2.UncheckedImage")));
            // 
            // tpProjectReport
            // 
            this.tpProjectReport.Controls.Add(this.gdProjectReport);
            this.tpProjectReport.Location = new System.Drawing.Point(4, 22);
            this.tpProjectReport.Name = "tpProjectReport";
            this.tpProjectReport.Size = new System.Drawing.Size(897, 388);
            this.tpProjectReport.TabIndex = 5;
            this.tpProjectReport.Tag = "项目资金支付计划申报表";
            this.tpProjectReport.Text = "项目资金支付计划申报表";
            this.tpProjectReport.UseVisualStyleBackColor = true;
            // 
            // gdProjectReport
            // 
            this.gdProjectReport.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdProjectReport.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectReport.CheckedImage")));
            this.gdProjectReport.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdProjectReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdProjectReport.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdProjectReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdProjectReport.Location = new System.Drawing.Point(0, 0);
            this.gdProjectReport.Name = "gdProjectReport";
            this.gdProjectReport.Size = new System.Drawing.Size(897, 388);
            this.gdProjectReport.TabIndex = 235;
            this.gdProjectReport.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdProjectReport.UncheckedImage")));
            // 
            // tpCompanyReport
            // 
            this.tpCompanyReport.Controls.Add(this.gdCompanyReport);
            this.tpCompanyReport.Location = new System.Drawing.Point(4, 22);
            this.tpCompanyReport.Name = "tpCompanyReport";
            this.tpCompanyReport.Size = new System.Drawing.Size(614, 221);
            this.tpCompanyReport.TabIndex = 6;
            this.tpCompanyReport.Tag = "分公司资金支付计划申报表";
            this.tpCompanyReport.Text = "分公司资金支付计划申报表";
            this.tpCompanyReport.UseVisualStyleBackColor = true;
            // 
            // gdCompanyReport
            // 
            this.gdCompanyReport.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdCompanyReport.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdCompanyReport.CheckedImage")));
            this.gdCompanyReport.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdCompanyReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdCompanyReport.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdCompanyReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdCompanyReport.Location = new System.Drawing.Point(0, 0);
            this.gdCompanyReport.Name = "gdCompanyReport";
            this.gdCompanyReport.Size = new System.Drawing.Size(614, 221);
            this.gdCompanyReport.TabIndex = 235;
            this.gdCompanyReport.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdCompanyReport.UncheckedImage")));
            // 
            // btnGetData
            // 
            this.btnGetData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGetData.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetData.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnGetData.Location = new System.Drawing.Point(770, 9);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(75, 23);
            this.btnGetData.TabIndex = 8;
            this.btnGetData.Text = "获取数据";
            this.btnGetData.UseVisualStyleBackColor = true;
            // 
            // VPlanDeclare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 492);
            this.Name = "VPlanDeclare";
            this.Text = "资金计划申报";
            this.Controls.SetChildIndex(this.pnlFloor, 0);
            this.pnlBody.ResumeLayout(false);
            this.pnlFloor.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tPOfficeExpend.ResumeLayout(false);
            this.tpOtherExpend.ResumeLayout(false);
            this.tpProjectExpend.ResumeLayout(false);
            this.tpTotalExpend1.ResumeLayout(false);
            this.tpTotalExpend2.ResumeLayout(false);
            this.tpProjectReport.ResumeLayout(false);
            this.tpCompanyReport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label label2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tPOfficeExpend;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdOfficeExpend;
        private System.Windows.Forms.TabPage tpOtherExpend;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdOtherExpend;
        private System.Windows.Forms.TabPage tpProjectExpend;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdProjectExpend;
        private System.Windows.Forms.TabPage tpTotalExpend1;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdTotalExpend1;
        private System.Windows.Forms.TabPage tpTotalExpend2;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdTotalExpend2;
        private System.Windows.Forms.TabPage tpProjectReport;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdProjectReport;
        private System.Windows.Forms.TabPage tpCompanyReport;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdCompanyReport;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tspMenuInsert;
        private System.Windows.Forms.ToolStripMenuItem tspMenuDelete;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnGetData;
        private System.Windows.Forms.ToolStripMenuItem tspMenuRefresh;

    }
}