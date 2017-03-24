namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonForm
{
    partial class VMessageShow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.linkExpenseAccount = new System.Windows.Forms.LinkLabel();
            this.rchEstimate = new System.Windows.Forms.RichTextBox();
            this.linkPayoutBill = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // linkExpenseAccount
            // 
            this.linkExpenseAccount.AutoSize = true;
            this.linkExpenseAccount.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkExpenseAccount.Location = new System.Drawing.Point(27, 19);
            this.linkExpenseAccount.Name = "linkExpenseAccount";
            this.linkExpenseAccount.Size = new System.Drawing.Size(160, 16);
            this.linkExpenseAccount.TabIndex = 0;
            this.linkExpenseAccount.TabStop = true;
            this.linkExpenseAccount.Text = "AuditExpenseAccount";
            // 
            // rchEstimate
            // 
            this.rchEstimate.Location = new System.Drawing.Point(12, 97);
            this.rchEstimate.Name = "rchEstimate";
            this.rchEstimate.Size = new System.Drawing.Size(306, 75);
            this.rchEstimate.TabIndex = 1;
            this.rchEstimate.Text = "";
            // 
            // linkPayoutBill
            // 
            this.linkPayoutBill.AutoSize = true;
            this.linkPayoutBill.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkPayoutBill.Location = new System.Drawing.Point(27, 59);
            this.linkPayoutBill.Name = "linkPayoutBill";
            this.linkPayoutBill.Size = new System.Drawing.Size(128, 16);
            this.linkPayoutBill.TabIndex = 2;
            this.linkPayoutBill.TabStop = true;
            this.linkPayoutBill.Text = "AuditPayoutBill";
            // 
            // VMessageShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 195);
            this.Controls.Add(this.linkPayoutBill);
            this.Controls.Add(this.rchEstimate);
            this.Controls.Add(this.linkExpenseAccount);
            this.Name = "VMessageShow";
            this.Text = "系统消息提示";
            this.Load += new System.EventHandler(this.VMessageShow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkExpenseAccount;
        private System.Windows.Forms.RichTextBox rchEstimate;
        private System.Windows.Forms.LinkLabel linkPayoutBill;

    }
}