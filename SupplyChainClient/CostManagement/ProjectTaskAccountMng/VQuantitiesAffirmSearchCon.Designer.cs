namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    partial class VQuantitiesAffirmSearchCon
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.customGroupBox1 = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.dtMadeBillStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtMadeBillEndDate = new System.Windows.Forms.DateTimePicker();
            this.txtBillStartCode = new System.Windows.Forms.TextBox();
            this.txtBillEndCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.floor.SuspendLayout();
            this.customGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // floor
            // 
            this.floor.Controls.Add(this.customGroupBox1);
            this.floor.Controls.Add(this.btnCancel);
            this.floor.Controls.Add(this.btnOK);
            this.floor.Size = new System.Drawing.Size(378, 207);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(270, 170);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(180, 170);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "查询";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "制单日期：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "单号：";
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.dtMadeBillStartDate);
            this.customGroupBox1.Controls.Add(this.dtMadeBillEndDate);
            this.customGroupBox1.Controls.Add(this.txtBillStartCode);
            this.customGroupBox1.Controls.Add(this.txtBillEndCode);
            this.customGroupBox1.Controls.Add(this.label5);
            this.customGroupBox1.Controls.Add(this.label2);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Controls.Add(this.label4);
            this.customGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(354, 140);
            this.customGroupBox1.TabIndex = 19;
            this.customGroupBox1.TabStop = false;
            // 
            // dtMadeBillStartDate
            // 
            this.dtMadeBillStartDate.Location = new System.Drawing.Point(77, 86);
            this.dtMadeBillStartDate.Name = "dtMadeBillStartDate";
            this.dtMadeBillStartDate.Size = new System.Drawing.Size(115, 21);
            this.dtMadeBillStartDate.TabIndex = 23;
            // 
            // dtMadeBillEndDate
            // 
            this.dtMadeBillEndDate.Location = new System.Drawing.Point(209, 86);
            this.dtMadeBillEndDate.Name = "dtMadeBillEndDate";
            this.dtMadeBillEndDate.Size = new System.Drawing.Size(115, 21);
            this.dtMadeBillEndDate.TabIndex = 22;
            // 
            // txtBillStartCode
            // 
            this.txtBillStartCode.Location = new System.Drawing.Point(77, 39);
            this.txtBillStartCode.Name = "txtBillStartCode";
            this.txtBillStartCode.Size = new System.Drawing.Size(115, 21);
            this.txtBillStartCode.TabIndex = 18;
            // 
            // txtBillEndCode
            // 
            this.txtBillEndCode.Location = new System.Drawing.Point(209, 39);
            this.txtBillEndCode.Name = "txtBillEndCode";
            this.txtBillEndCode.Size = new System.Drawing.Size(115, 21);
            this.txtBillEndCode.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(195, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "-";
            // 
            // VProjectTaskAccountBillSearchCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 207);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "VProjectTaskAccountBillSearchCon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询进度计划确认单";
            this.floor.ResumeLayout(false);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.DateTimePicker dtMadeBillStartDate;
        private System.Windows.Forms.DateTimePicker dtMadeBillEndDate;
        private System.Windows.Forms.TextBox txtBillStartCode;
        private System.Windows.Forms.TextBox txtBillEndCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;

    }
}