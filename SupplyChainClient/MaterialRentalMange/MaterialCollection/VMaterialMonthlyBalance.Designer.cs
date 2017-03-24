namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection
{
    partial class VMaterialMonthlyBalance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMaterialMonthlyBalance));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMonth = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtYear = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnMatUnReckoning = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnMatReckoning = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpEndDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.txtExtSumMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupply = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier();
            this.lblSupplier = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSelectGWBS = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtAccountRootNode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupply)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(807, 536);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(127, 0);
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.Text = "料具租赁月结";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectGWBS);
            this.groupBox1.Controls.Add(this.txtMonth);
            this.groupBox1.Controls.Add(this.txtYear);
            this.groupBox1.Controls.Add(this.txtAccountRootNode);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.customLabel5);
            this.groupBox1.Controls.Add(this.btnMatUnReckoning);
            this.groupBox1.Controls.Add(this.btnMatReckoning);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.txtExtSumMoney);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.txtSupply);
            this.groupBox1.Controls.Add(this.lblSupplier);
            this.groupBox1.Location = new System.Drawing.Point(75, 164);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(646, 153);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // txtMonth
            // 
            this.txtMonth.BackColor = System.Drawing.SystemColors.Control;
            this.txtMonth.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMonth.DrawSelf = false;
            this.txtMonth.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMonth.EnterToTab = false;
            this.txtMonth.Location = new System.Drawing.Point(400, 65);
            this.txtMonth.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Padding = new System.Windows.Forms.Padding(1);
            this.txtMonth.ReadOnly = false;
            this.txtMonth.Size = new System.Drawing.Size(54, 16);
            this.txtMonth.TabIndex = 139;
            // 
            // txtYear
            // 
            this.txtYear.BackColor = System.Drawing.SystemColors.Control;
            this.txtYear.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtYear.DrawSelf = false;
            this.txtYear.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtYear.EnterToTab = false;
            this.txtYear.Location = new System.Drawing.Point(328, 65);
            this.txtYear.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtYear.Name = "txtYear";
            this.txtYear.Padding = new System.Windows.Forms.Padding(1);
            this.txtYear.ReadOnly = false;
            this.txtYear.Size = new System.Drawing.Size(54, 16);
            this.txtYear.TabIndex = 138;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(277, 69);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(47, 12);
            this.customLabel4.TabIndex = 144;
            this.customLabel4.Text = "会计期:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(383, 69);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(11, 12);
            this.customLabel5.TabIndex = 143;
            this.customLabel5.Text = "-";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnMatUnReckoning
            // 
            this.btnMatUnReckoning.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMatUnReckoning.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMatUnReckoning.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMatUnReckoning.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnMatUnReckoning.Location = new System.Drawing.Point(320, 102);
            this.btnMatUnReckoning.Name = "btnMatUnReckoning";
            this.btnMatUnReckoning.Size = new System.Drawing.Size(107, 35);
            this.btnMatUnReckoning.TabIndex = 142;
            this.btnMatUnReckoning.Text = "料具反结账";
            this.btnMatUnReckoning.UseVisualStyleBackColor = true;
            // 
            // btnMatReckoning
            // 
            this.btnMatReckoning.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMatReckoning.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMatReckoning.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnMatReckoning.Location = new System.Drawing.Point(119, 102);
            this.btnMatReckoning.Name = "btnMatReckoning";
            this.btnMatReckoning.Size = new System.Drawing.Size(107, 35);
            this.btnMatReckoning.TabIndex = 141;
            this.btnMatReckoning.Text = "料具月结账";
            this.btnMatReckoning.UseVisualStyleBackColor = true;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(5, 69);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(83, 12);
            this.customLabel3.TabIndex = 140;
            this.customLabel3.Text = "业务结束日期:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(92, 65);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(142, 21);
            this.dtpEndDate.TabIndex = 139;
            // 
            // txtExtSumMoney
            // 
            this.txtExtSumMoney.BackColor = System.Drawing.SystemColors.Control;
            this.txtExtSumMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtExtSumMoney.DrawSelf = false;
            this.txtExtSumMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtExtSumMoney.EnterToTab = false;
            this.txtExtSumMoney.Location = new System.Drawing.Point(328, 25);
            this.txtExtSumMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtExtSumMoney.Name = "txtExtSumMoney";
            this.txtExtSumMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtExtSumMoney.ReadOnly = false;
            this.txtExtSumMoney.Size = new System.Drawing.Size(84, 16);
            this.txtExtSumMoney.TabIndex = 137;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(267, 28);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 136;
            this.customLabel2.Text = "调整费用:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSupply
            // 
            this.txtSupply.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupply.Code = null;
            this.txtSupply.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtSupply.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtSupply.EnterToTab = false;
            this.txtSupply.Id = "";
            this.txtSupply.Location = new System.Drawing.Point(93, 25);
            this.txtSupply.Name = "txtSupply";
            this.txtSupply.Result = ((System.Collections.IList)(resources.GetObject("txtSupply.Result")));
            this.txtSupply.RightMouse = false;
            this.txtSupply.Size = new System.Drawing.Size(142, 21);
            this.txtSupply.TabIndex = 9;
            this.txtSupply.Tag = null;
            this.txtSupply.Value = "";
            this.txtSupply.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplier.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplier.Location = new System.Drawing.Point(41, 30);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(47, 12);
            this.lblSupplier.TabIndex = 10;
            this.lblSupplier.Text = "出租方:";
            this.lblSupplier.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSelectGWBS
            // 
            this.btnSelectGWBS.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectGWBS.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectGWBS.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSelectGWBS.Location = new System.Drawing.Point(584, 22);
            this.btnSelectGWBS.Name = "btnSelectGWBS";
            this.btnSelectGWBS.Size = new System.Drawing.Size(46, 22);
            this.btnSelectGWBS.TabIndex = 152;
            this.btnSelectGWBS.Text = "选择";
            this.btnSelectGWBS.UseVisualStyleBackColor = true;
            // 
            // txtAccountRootNode
            // 
            this.txtAccountRootNode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtAccountRootNode.DrawSelf = false;
            this.txtAccountRootNode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtAccountRootNode.EnterToTab = false;
            this.txtAccountRootNode.Location = new System.Drawing.Point(472, 26);
            this.txtAccountRootNode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtAccountRootNode.Name = "txtAccountRootNode";
            this.txtAccountRootNode.Padding = new System.Windows.Forms.Padding(1);
            this.txtAccountRootNode.ReadOnly = true;
            this.txtAccountRootNode.Size = new System.Drawing.Size(111, 16);
            this.txtAccountRootNode.TabIndex = 151;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(414, 28);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 150;
            this.customLabel1.Text = "调整部位:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VMaterialMonthlyBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 536);
            this.Name = "VMaterialMonthlyBalance";
            this.Text = "料具月结";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSupply)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnMatUnReckoning;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnMatReckoning;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpEndDate;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtExtSumMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonSupplier txtSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplier;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMonth;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtYear;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSelectGWBS;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtAccountRootNode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;

    }
}