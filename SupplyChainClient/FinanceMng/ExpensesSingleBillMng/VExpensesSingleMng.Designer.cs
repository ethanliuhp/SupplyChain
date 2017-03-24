namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesSingleBillMng
{
    partial class VExpensesSingleMng
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VExpensesSingleMng));
            this.groupSupply = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.txtExpensesType = new System.Windows.Forms.ComboBox();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCHJKBig = new System.Windows.Forms.TextBox();
            this.txtCHJKMoney = new System.Windows.Forms.TextBox();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtExpensesPerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.txtDigest = new System.Windows.Forms.TextBox();
            this.txtMoneyBig = new System.Windows.Forms.TextBox();
            this.txtExpensesMoney = new System.Windows.Forms.TextBox();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtExpensesDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblSupplier = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmsDg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiDel = new System.Windows.Forms.ToolStripMenuItem();
            this.txtCreateDate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlContent.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupSupply.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpensesPerson)).BeginInit();
            this.cmsDg.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Size = new System.Drawing.Size(879, 441);
            // 
            // pnlBody
            // 
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.None;
            this.pnlBody.Location = new System.Drawing.Point(0, 328);
            this.pnlBody.Size = new System.Drawing.Size(855, 10);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtProject);
            this.pnlFooter.Controls.Add(this.customLabel7);
            this.pnlFooter.Controls.Add(this.txtCreateDate);
            this.pnlFooter.Controls.Add(this.customLabel6);
            this.pnlFooter.Controls.Add(this.txtCreatePerson);
            this.pnlFooter.Controls.Add(this.customLabel5);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFooter.Location = new System.Drawing.Point(0, 328);
            this.pnlFooter.Size = new System.Drawing.Size(877, 111);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(879, 0);
            this.spTop.Visible = false;
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Size = new System.Drawing.Size(879, 441);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.groupSupply);
            this.pnlHeader.Size = new System.Drawing.Size(877, 328);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.groupSupply, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Visible = false;
            // 
            // groupSupply
            // 
            this.groupSupply.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSupply.Controls.Add(this.txtExpensesType);
            this.groupSupply.Controls.Add(this.customLabel4);
            this.groupSupply.Controls.Add(this.txtCHJKBig);
            this.groupSupply.Controls.Add(this.txtCHJKMoney);
            this.groupSupply.Controls.Add(this.customLabel8);
            this.groupSupply.Controls.Add(this.txtExpensesPerson);
            this.groupSupply.Controls.Add(this.txtDigest);
            this.groupSupply.Controls.Add(this.txtMoneyBig);
            this.groupSupply.Controls.Add(this.txtExpensesMoney);
            this.groupSupply.Controls.Add(this.customLabel2);
            this.groupSupply.Controls.Add(this.customLabel1);
            this.groupSupply.Controls.Add(this.txtExpensesDate);
            this.groupSupply.Controls.Add(this.customLabel3);
            this.groupSupply.Controls.Add(this.lblSupplier);
            this.groupSupply.Controls.Add(this.txtCode);
            this.groupSupply.Controls.Add(this.lblSupplyContract);
            this.groupSupply.Location = new System.Drawing.Point(3, 3);
            this.groupSupply.Name = "groupSupply";
            this.groupSupply.Size = new System.Drawing.Size(698, 319);
            this.groupSupply.TabIndex = 2;
            this.groupSupply.TabStop = false;
            this.groupSupply.Text = ">>单据信息";
            // 
            // txtExpensesType
            // 
            this.txtExpensesType.FormattingEnabled = true;
            this.txtExpensesType.Location = new System.Drawing.Point(106, 109);
            this.txtExpensesType.Name = "txtExpensesType";
            this.txtExpensesType.Size = new System.Drawing.Size(135, 20);
            this.txtExpensesType.TabIndex = 155;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(41, 114);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(59, 12);
            this.customLabel4.TabIndex = 154;
            this.customLabel4.Text = "费用类型:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCHJKBig
            // 
            this.txtCHJKBig.Location = new System.Drawing.Point(244, 207);
            this.txtCHJKBig.Name = "txtCHJKBig";
            this.txtCHJKBig.ReadOnly = true;
            this.txtCHJKBig.Size = new System.Drawing.Size(195, 21);
            this.txtCHJKBig.TabIndex = 153;
            this.txtCHJKBig.Text = "金额大写";
            // 
            // txtCHJKMoney
            // 
            this.txtCHJKMoney.Location = new System.Drawing.Point(106, 207);
            this.txtCHJKMoney.Name = "txtCHJKMoney";
            this.txtCHJKMoney.Size = new System.Drawing.Size(135, 21);
            this.txtCHJKMoney.TabIndex = 152;
            this.txtCHJKMoney.TextChanged += new System.EventHandler(this.txtCHJKMoney_tbTextChanged);
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(29, 214);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(71, 12);
            this.customLabel8.TabIndex = 151;
            this.customLabel8.Text = "冲借款金额:";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtExpensesPerson
            // 
            this.txtExpensesPerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtExpensesPerson.Code = null;
            this.txtExpensesPerson.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtExpensesPerson.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtExpensesPerson.EnterToTab = false;
            this.txtExpensesPerson.Id = "";
            this.txtExpensesPerson.Location = new System.Drawing.Point(315, 25);
            this.txtExpensesPerson.Name = "txtExpensesPerson";
            this.txtExpensesPerson.Result = ((System.Collections.IList)(resources.GetObject("txtExpensesPerson.Result")));
            this.txtExpensesPerson.RightMouse = false;
            this.txtExpensesPerson.Size = new System.Drawing.Size(145, 21);
            this.txtExpensesPerson.TabIndex = 148;
            this.txtExpensesPerson.Tag = null;
            this.txtExpensesPerson.Value = "";
            this.txtExpensesPerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // txtDigest
            // 
            this.txtDigest.Location = new System.Drawing.Point(106, 266);
            this.txtDigest.Name = "txtDigest";
            this.txtDigest.Size = new System.Drawing.Size(430, 21);
            this.txtDigest.TabIndex = 141;
            // 
            // txtMoneyBig
            // 
            this.txtMoneyBig.Location = new System.Drawing.Point(244, 149);
            this.txtMoneyBig.Name = "txtMoneyBig";
            this.txtMoneyBig.ReadOnly = true;
            this.txtMoneyBig.Size = new System.Drawing.Size(195, 21);
            this.txtMoneyBig.TabIndex = 139;
            this.txtMoneyBig.Text = "金额大写";
            // 
            // txtExpensesMoney
            // 
            this.txtExpensesMoney.Location = new System.Drawing.Point(106, 149);
            this.txtExpensesMoney.Name = "txtExpensesMoney";
            this.txtExpensesMoney.Size = new System.Drawing.Size(135, 21);
            this.txtExpensesMoney.TabIndex = 138;
            this.txtExpensesMoney.TextChanged += new System.EventHandler(this.txtExpensesMoney_tbTextChanged);
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(41, 156);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 137;
            this.customLabel2.Text = "报销金额:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(41, 72);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 135;
            this.customLabel1.Text = "报销日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtExpensesDate
            // 
            this.txtExpensesDate.Location = new System.Drawing.Point(106, 68);
            this.txtExpensesDate.Name = "txtExpensesDate";
            this.txtExpensesDate.Size = new System.Drawing.Size(135, 21);
            this.txtExpensesDate.TabIndex = 134;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(41, 270);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 9;
            this.customLabel3.Text = "费用说明:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplier.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplier.Location = new System.Drawing.Point(256, 28);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(53, 12);
            this.lblSupplier.TabIndex = 7;
            this.lblSupplier.Text = "报销人：";
            this.lblSupplier.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(106, 25);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(121, 16);
            this.txtCode.TabIndex = 1;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(53, 28);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(47, 12);
            this.lblSupplyContract.TabIndex = 0;
            this.lblSupplyContract.Text = "单据号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmsDg
            // 
            this.cmsDg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDel});
            this.cmsDg.Name = "cmsDg";
            this.cmsDg.Size = new System.Drawing.Size(95, 26);
            // 
            // tsmiDel
            // 
            this.tsmiDel.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.删除;
            this.tsmiDel.Name = "tsmiDel";
            this.tsmiDel.Size = new System.Drawing.Size(94, 22);
            this.tsmiDel.Text = "删除";
            this.tsmiDel.ToolTipText = "删除当前选中的记录";
            // 
            // txtCreateDate
            // 
            this.txtCreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreateDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreateDate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreateDate.DrawSelf = false;
            this.txtCreateDate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreateDate.EnterToTab = false;
            this.txtCreateDate.Location = new System.Drawing.Point(354, 85);
            this.txtCreateDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateDate.ReadOnly = true;
            this.txtCreateDate.Size = new System.Drawing.Size(129, 16);
            this.txtCreateDate.TabIndex = 9;
            // 
            // customLabel6
            // 
            this.customLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(293, 88);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 27;
            this.customLabel6.Text = "制单日期:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreatePerson.DrawSelf = false;
            this.txtCreatePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Location = new System.Drawing.Point(70, 84);
            this.txtCreatePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreatePerson.ReadOnly = true;
            this.txtCreatePerson.Size = new System.Drawing.Size(135, 16);
            this.txtCreatePerson.TabIndex = 8;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(19, 88);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(47, 12);
            this.customLabel5.TabIndex = 25;
            this.customLabel5.Text = "制单人:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProject
            // 
            this.txtProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProject.DrawSelf = false;
            this.txtProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProject.EnterToTab = false;
            this.txtProject.Location = new System.Drawing.Point(619, 85);
            this.txtProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProject.Name = "txtProject";
            this.txtProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtProject.ReadOnly = true;
            this.txtProject.Size = new System.Drawing.Size(175, 16);
            this.txtProject.TabIndex = 34;
            // 
            // customLabel7
            // 
            this.customLabel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(559, 89);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 35;
            this.customLabel7.Text = "归属项目:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VExpensesSingleMng
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(879, 441);
            this.Name = "VExpensesSingleMng";
            this.Text = "费用报销单";
            this.pnlContent.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlWorkSpace.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupSupply.ResumeLayout(false);
            this.groupSupply.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtExpensesPerson)).EndInit();
            this.cmsDg.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox groupSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplier;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid1;
        private System.Windows.Forms.ContextMenuStrip cmsDg;
        private System.Windows.Forms.ToolStripMenuItem tsmiDel;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker txtExpensesDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.TextBox txtDigest;
        private System.Windows.Forms.TextBox txtMoneyBig;
        private System.Windows.Forms.TextBox txtExpensesMoney;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtExpensesPerson;
        private System.Windows.Forms.TextBox txtCHJKBig;
        private System.Windows.Forms.TextBox txtCHJKMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private System.Windows.Forms.ComboBox txtExpensesType;
    }
}