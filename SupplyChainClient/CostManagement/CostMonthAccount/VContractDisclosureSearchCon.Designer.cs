namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    partial class VContractDisclosureSearchCon
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
            this.btncancle = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.dtDisclosureEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.dtDisclosureStart = new System.Windows.Forms.DateTimePicker();
            this.btnselect = new System.Windows.Forms.Button();
            this.floor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // floor
            // 
            this.floor.Controls.Add(this.btncancle);
            this.floor.Controls.Add(this.groupBox1);
            this.floor.Controls.Add(this.btnselect);
            this.floor.Dock = System.Windows.Forms.DockStyle.None;
            this.floor.Size = new System.Drawing.Size(461, 288);
            // 
            // btncancle
            // 
            this.btncancle.Location = new System.Drawing.Point(252, 199);
            this.btncancle.Name = "btncancle";
            this.btncancle.Size = new System.Drawing.Size(75, 23);
            this.btncancle.TabIndex = 32;
            this.btncancle.Text = "取消";
            this.btncancle.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtDisclosureEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtDisclosureStart);
            this.groupBox1.Location = new System.Drawing.Point(42, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(377, 110);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            // 
            // txtProName
            // 
            this.txtProName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtProName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProName.DrawSelf = false;
            this.txtProName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProName.EnterToTab = false;
            this.txtProName.Location = new System.Drawing.Point(92, 34);
            this.txtProName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProName.Name = "txtProName";
            this.txtProName.Padding = new System.Windows.Forms.Padding(1);
            this.txtProName.ReadOnly = false;
            this.txtProName.Size = new System.Drawing.Size(206, 16);
            this.txtProName.TabIndex = 208;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 207;
            this.label1.Text = "项目名称：";
            // 
            // dtDisclosureEnd
            // 
            this.dtDisclosureEnd.Location = new System.Drawing.Point(225, 67);
            this.dtDisclosureEnd.Name = "dtDisclosureEnd";
            this.dtDisclosureEnd.Size = new System.Drawing.Size(106, 21);
            this.dtDisclosureEnd.TabIndex = 203;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(208, 71);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 204;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 27;
            this.label4.Text = "交底日期：";
            // 
            // dtDisclosureStart
            // 
            this.dtDisclosureStart.Location = new System.Drawing.Point(92, 67);
            this.dtDisclosureStart.Name = "dtDisclosureStart";
            this.dtDisclosureStart.Size = new System.Drawing.Size(115, 21);
            this.dtDisclosureStart.TabIndex = 24;
            this.dtDisclosureStart.Value = new System.DateTime(2015, 1, 1, 8, 0, 0, 0);
            // 
            // btnselect
            // 
            this.btnselect.Location = new System.Drawing.Point(134, 199);
            this.btnselect.Name = "btnselect";
            this.btnselect.Size = new System.Drawing.Size(75, 23);
            this.btnselect.TabIndex = 31;
            this.btnselect.Text = "查询";
            this.btnselect.UseVisualStyleBackColor = true;
            // 
            // VContractDisclosureSearchCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 288);
            this.Name = "VContractDisclosureSearchCon";
            this.Text = "查询";
            this.floor.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btncancle;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtDisclosureEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtDisclosureStart;
        private System.Windows.Forms.Button btnselect;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProName;
        private System.Windows.Forms.Label label1;
    }
}