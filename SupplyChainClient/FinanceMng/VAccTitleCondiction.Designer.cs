namespace Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI
{
    partial class VAccTitleCondiction
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
            this.pnMain = new VirtualMachine.Component.WinControls.Controls.CustomPanel();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOk = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customGroupBox1 = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.txtTo = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtFrom = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cbbAccType = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnMain.SuspendLayout();
            this.customGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnMain
            // 
            this.pnMain.Controls.Add(this.btnCancel);
            this.pnMain.Controls.Add(this.btnOk);
            this.pnMain.Controls.Add(this.customGroupBox1);
            this.pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMain.Location = new System.Drawing.Point(0, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(302, 145);
            this.pnMain.TabIndex = 0;
            this.pnMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnMain_Paint);
            // 
            // btnCancel
            // 
            this.btnCancel.ButtonType = VirtualMachine.Component.WinControls.Controls.ButtonType.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(219, 116);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 22);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.ButtonType = VirtualMachine.Component.WinControls.Controls.ButtonType.Confirm;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(118, 116);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 22);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定(&K)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.txtTo);
            this.customGroupBox1.Controls.Add(this.customLabel3);
            this.customGroupBox1.Controls.Add(this.txtFrom);
            this.customGroupBox1.Controls.Add(this.customLabel2);
            this.customGroupBox1.Controls.Add(this.cbbAccType);
            this.customGroupBox1.Controls.Add(this.customLabel1);
            this.customGroupBox1.Location = new System.Drawing.Point(6, 3);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(290, 107);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            // 
            // txtTo
            // 
            this.txtTo.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtTo.DrawSelf = false;
            this.txtTo.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.ComboBox;
            this.txtTo.EnterToTab = false;
            this.txtTo.Location = new System.Drawing.Point(198, 59);
            this.txtTo.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtTo.Name = "txtTo";
            this.txtTo.Padding = new System.Windows.Forms.Padding(1);
            this.txtTo.ReadOnly = false;
            this.txtTo.Size = new System.Drawing.Size(74, 16);
            this.txtTo.TabIndex = 5;
            this.txtTo.ArrowClick += new System.EventHandler(this.txtTo_ArrowClick);
            this.txtTo.Load += new System.EventHandler(this.txtTo_Load);
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Location = new System.Drawing.Point(175, 63);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(17, 12);
            this.customLabel3.TabIndex = 4;
            this.customLabel3.Text = "至";
            // 
            // txtFrom
            // 
            this.txtFrom.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtFrom.DrawSelf = false;
            this.txtFrom.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.ComboBox;
            this.txtFrom.EnterToTab = false;
            this.txtFrom.Location = new System.Drawing.Point(95, 59);
            this.txtFrom.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Padding = new System.Windows.Forms.Padding(1);
            this.txtFrom.ReadOnly = false;
            this.txtFrom.Size = new System.Drawing.Size(74, 16);
            this.txtFrom.TabIndex = 3;
            this.txtFrom.ArrowClick += new System.EventHandler(this.txtFrom_ArrowClick);
            this.txtFrom.Load += new System.EventHandler(this.txtFrom_Load);
            // 
            // customLabel2
            // 
            this.customLabel2.AddColonAuto = true;
            this.customLabel2.AutoSize = true;
            this.customLabel2.Location = new System.Drawing.Point(18, 63);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(65, 12);
            this.customLabel2.TabIndex = 2;
            this.customLabel2.Text = "科目范围：";
            // 
            // cbbAccType
            // 
            this.cbbAccType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbAccType.FormattingEnabled = true;
            this.cbbAccType.Location = new System.Drawing.Point(95, 26);
            this.cbbAccType.Name = "cbbAccType";
            this.cbbAccType.Size = new System.Drawing.Size(177, 20);
            this.cbbAccType.TabIndex = 1;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Location = new System.Drawing.Point(18, 29);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 0;
            this.customLabel1.Text = "科目属性：";
            // 
            // VAccTitleCondiction
            // 
            this.ClientSize = new System.Drawing.Size(302, 145);
            this.Controls.Add(this.pnMain);
            this.Name = "VAccTitleCondiction";
            this.Text = "选择条件";
            this.Load += new System.EventHandler(this.VAccTitleCondiction_Load);
            this.pnMain.ResumeLayout(false);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomPanel pnMain;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox customGroupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtTo;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtFrom;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cbbAccType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOk;
    }
}
