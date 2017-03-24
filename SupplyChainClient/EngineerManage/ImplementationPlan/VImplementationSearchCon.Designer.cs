namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan
{
    partial class VImplementationSearchCon
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
            this.dtpCreateDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpMadeBillDate = new System.Windows.Forms.DateTimePicker();
            this.btnselect = new System.Windows.Forms.Button();
            this.btncancle = new System.Windows.Forms.Button();
            this.floor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // floor
            // 
            this.floor.Controls.Add(this.btncancle);
            this.floor.Controls.Add(this.groupBox1);
            this.floor.Controls.Add(this.btnselect);
            this.floor.Size = new System.Drawing.Size(483, 220);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpCreateDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpMadeBillDate);
            this.groupBox1.Location = new System.Drawing.Point(26, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(377, 110);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // dtpCreateDateEnd
            // 
            this.dtpCreateDateEnd.Location = new System.Drawing.Point(234, 37);
            this.dtpCreateDateEnd.Name = "dtpCreateDateEnd";
            this.dtpCreateDateEnd.Size = new System.Drawing.Size(106, 21);
            this.dtpCreateDateEnd.TabIndex = 203;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(217, 45);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 204;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 27;
            this.label4.Text = "业务日期：";
            // 
            // dtpMadeBillDate
            // 
            this.dtpMadeBillDate.Location = new System.Drawing.Point(95, 37);
            this.dtpMadeBillDate.Name = "dtpMadeBillDate";
            this.dtpMadeBillDate.Size = new System.Drawing.Size(115, 21);
            this.dtpMadeBillDate.TabIndex = 24;
            this.dtpMadeBillDate.Value = new System.DateTime(2012, 7, 10, 8, 47, 0, 0);
            // 
            // btnselect
            // 
            this.btnselect.Location = new System.Drawing.Point(230, 160);
            this.btnselect.Name = "btnselect";
            this.btnselect.Size = new System.Drawing.Size(75, 23);
            this.btnselect.TabIndex = 28;
            this.btnselect.Text = "查询";
            this.btnselect.UseVisualStyleBackColor = true;
            // 
            // btncancle
            // 
            this.btncancle.Location = new System.Drawing.Point(328, 160);
            this.btncancle.Name = "btncancle";
            this.btncancle.Size = new System.Drawing.Size(75, 23);
            this.btncancle.TabIndex = 29;
            this.btncancle.Text = "取消";
            this.btncancle.UseVisualStyleBackColor = true;
            // 
            // VImplementationSearchCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 220);
            this.Name = "VImplementationSearchCon";
            this.Text = "策划查询";
            this.floor.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpMadeBillDate;
        private System.Windows.Forms.Button btncancle;
        private System.Windows.Forms.Button btnselect;
        private System.Windows.Forms.Label label4;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpCreateDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
    }
}