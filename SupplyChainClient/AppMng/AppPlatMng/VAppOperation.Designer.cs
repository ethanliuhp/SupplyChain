namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng
{
    partial class VAppOperation
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
            this.BtnDisagree = new System.Windows.Forms.Button();
            this.btnAppAgree = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Text = "审批操作";
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.textBox1);
            this.pnlFloor.Controls.Add(this.BtnDisagree);
            this.pnlFloor.Controls.Add(this.btnAppAgree);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Size = new System.Drawing.Size(312, 446);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnAppAgree, 0);
            this.pnlFloor.Controls.SetChildIndex(this.BtnDisagree, 0);
            this.pnlFloor.Controls.SetChildIndex(this.textBox1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            this.lblTitle.Size = new System.Drawing.Size(0, 20);
            this.lblTitle.Text = "";
            // 
            // BtnDisagree
            // 
            this.BtnDisagree.Location = new System.Drawing.Point(195, 360);
            this.BtnDisagree.Name = "BtnDisagree";
            this.BtnDisagree.Size = new System.Drawing.Size(75, 23);
            this.BtnDisagree.TabIndex = 7;
            this.BtnDisagree.Text = "审批不通过";
            this.BtnDisagree.UseVisualStyleBackColor = true;
            // 
            // btnAppAgree
            // 
            this.btnAppAgree.Location = new System.Drawing.Point(49, 360);
            this.btnAppAgree.Name = "btnAppAgree";
            this.btnAppAgree.Size = new System.Drawing.Size(75, 23);
            this.btnAppAgree.TabIndex = 6;
            this.btnAppAgree.Text = "审批通过";
            this.btnAppAgree.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "审批意见：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(76, 50);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(172, 286);
            this.textBox1.TabIndex = 8;
            // 
            // VAppOperation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(312, 446);
            this.Name = "VAppOperation";
            this.Text = "审批操作";
            this.Load += new System.EventHandler(this.VAppOperation_Load);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnDisagree;
        private System.Windows.Forms.Button btnAppAgree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;

    }
}