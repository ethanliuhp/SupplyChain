namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    partial class VMaterialHireMonthlyBalance
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMonth = new System.Windows.Forms.ComboBox();
            this.txtYear = new System.Windows.Forms.ComboBox();
            this.txtChangeMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnForward = new System.Windows.Forms.Button();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtContract = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtProjectName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSupply = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSupplier = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnMatUnReckoning = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnMatReckoning = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpEndDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMonth);
            this.groupBox1.Controls.Add(this.txtYear);
            this.groupBox1.Controls.Add(this.txtChangeMoney);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Controls.Add(this.btnForward);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.txtContract);
            this.groupBox1.Controls.Add(this.txtProjectName);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.txtSupply);
            this.groupBox1.Controls.Add(this.lblSupplier);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.customLabel5);
            this.groupBox1.Controls.Add(this.btnMatUnReckoning);
            this.groupBox1.Controls.Add(this.btnMatReckoning);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Location = new System.Drawing.Point(31, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(724, 258);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // txtMonth
            // 
            this.txtMonth.FormattingEnabled = true;
            this.txtMonth.Location = new System.Drawing.Point(167, 78);
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(46, 20);
            this.txtMonth.TabIndex = 166;
            // 
            // txtYear
            // 
            this.txtYear.FormattingEnabled = true;
            this.txtYear.Location = new System.Drawing.Point(92, 79);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(60, 20);
            this.txtYear.TabIndex = 165;
            // 
            // txtChangeMoney
            // 
            this.txtChangeMoney.BackColor = System.Drawing.SystemColors.Control;
            this.txtChangeMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtChangeMoney.DrawSelf = false;
            this.txtChangeMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtChangeMoney.EnterToTab = false;
            this.txtChangeMoney.Location = new System.Drawing.Point(573, 81);
            this.txtChangeMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtChangeMoney.Name = "txtChangeMoney";
            this.txtChangeMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtChangeMoney.ReadOnly = false;
            this.txtChangeMoney.Size = new System.Drawing.Size(132, 16);
            this.txtChangeMoney.TabIndex = 164;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(522, 84);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(35, 12);
            this.customLabel6.TabIndex = 163;
            this.customLabel6.Text = "调整:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnForward
            // 
            this.btnForward.Image = global::Application.Business.Erp.SupplyChain.Client.Properties.Resources.dot;
            this.btnForward.Location = new System.Drawing.Point(246, 51);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(21, 18);
            this.btnForward.TabIndex = 162;
            this.btnForward.UseVisualStyleBackColor = true;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(27, 58);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 161;
            this.customLabel1.Text = "租赁合同:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtContract
            // 
            this.txtContract.BackColor = System.Drawing.SystemColors.Control;
            this.txtContract.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtContract.DrawSelf = false;
            this.txtContract.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtContract.EnterToTab = false;
            this.txtContract.Location = new System.Drawing.Point(91, 53);
            this.txtContract.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtContract.Name = "txtContract";
            this.txtContract.Padding = new System.Windows.Forms.Padding(1);
            this.txtContract.ReadOnly = true;
            this.txtContract.Size = new System.Drawing.Size(149, 16);
            this.txtContract.TabIndex = 160;
            // 
            // txtProjectName
            // 
            this.txtProjectName.BackColor = System.Drawing.SystemColors.Control;
            this.txtProjectName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProjectName.DrawSelf = false;
            this.txtProjectName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProjectName.EnterToTab = false;
            this.txtProjectName.Location = new System.Drawing.Point(574, 54);
            this.txtProjectName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Padding = new System.Windows.Forms.Padding(1);
            this.txtProjectName.ReadOnly = true;
            this.txtProjectName.Size = new System.Drawing.Size(132, 16);
            this.txtProjectName.TabIndex = 159;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(521, 57);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(47, 12);
            this.customLabel2.TabIndex = 158;
            this.customLabel2.Text = "租赁方:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSupply
            // 
            this.txtSupply.BackColor = System.Drawing.SystemColors.Control;
            this.txtSupply.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSupply.DrawSelf = false;
            this.txtSupply.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSupply.EnterToTab = false;
            this.txtSupply.Location = new System.Drawing.Point(376, 55);
            this.txtSupply.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSupply.Name = "txtSupply";
            this.txtSupply.Padding = new System.Windows.Forms.Padding(1);
            this.txtSupply.ReadOnly = true;
            this.txtSupply.Size = new System.Drawing.Size(120, 16);
            this.txtSupply.TabIndex = 157;
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplier.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplier.Location = new System.Drawing.Point(323, 58);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(47, 12);
            this.lblSupplier.TabIndex = 156;
            this.lblSupplier.Text = "出租方:";
            this.lblSupplier.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(34, 83);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(47, 12);
            this.customLabel4.TabIndex = 144;
            this.customLabel4.Text = "会计期:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(154, 83);
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
            this.btnMatUnReckoning.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMatUnReckoning.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnMatUnReckoning.Location = new System.Drawing.Point(444, 118);
            this.btnMatUnReckoning.Name = "btnMatUnReckoning";
            this.btnMatUnReckoning.Size = new System.Drawing.Size(107, 35);
            this.btnMatUnReckoning.TabIndex = 142;
            this.btnMatUnReckoning.Text = "料具反结账";
            this.btnMatUnReckoning.UseVisualStyleBackColor = true;
            // 
            // btnMatReckoning
            // 
            this.btnMatReckoning.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnMatReckoning.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMatReckoning.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnMatReckoning.Location = new System.Drawing.Point(128, 118);
            this.btnMatReckoning.Name = "btnMatReckoning";
            this.btnMatReckoning.Size = new System.Drawing.Size(107, 35);
            this.btnMatReckoning.TabIndex = 141;
            this.btnMatReckoning.Text = "料具月结账";
            this.btnMatReckoning.UseVisualStyleBackColor = true;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(286, 81);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(83, 12);
            this.customLabel3.TabIndex = 140;
            this.customLabel3.Text = "业务结束日期:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(376, 77);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(133, 21);
            this.dtpEndDate.TabIndex = 139;
            // 
            // VMaterialHireMonthlyBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 536);
            this.Name = "VMaterialHireMonthlyBalance";
            this.Text = "料具月结";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnMatUnReckoning;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnMatReckoning;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpEndDate;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProjectName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSupply;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplier;
        private System.Windows.Forms.Button btnForward;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtContract;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtChangeMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private System.Windows.Forms.ComboBox txtMonth;
        private System.Windows.Forms.ComboBox txtYear;
    }
}