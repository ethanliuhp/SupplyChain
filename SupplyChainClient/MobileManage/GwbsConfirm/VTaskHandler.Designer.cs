namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm
{
    partial class VTaskHandler
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
            this.btnBack = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.pnlShow = new System.Windows.Forms.Panel();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Size = new System.Drawing.Size(310, 40);
            this.lblHeaderLine.Text = "请输入";
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.pnlShow);
            this.pnlFloor.Controls.Add(this.btnBack);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.btnNext);
            this.pnlFloor.Size = new System.Drawing.Size(312, 447);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnNext, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnBack, 0);
            this.pnlFloor.Controls.SetChildIndex(this.pnlShow, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(260, -1);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBack.Location = new System.Drawing.Point(35, 367);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "上一页";
            this.btnBack.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(197, 367);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNext.Location = new System.Drawing.Point(116, 367);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 8;
            this.btnNext.Text = "下一页";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // pnlShow
            // 
            this.pnlShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlShow.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlShow.Location = new System.Drawing.Point(0, 34);
            this.pnlShow.Name = "pnlShow";
            this.pnlShow.Size = new System.Drawing.Size(320, 327);
            this.pnlShow.TabIndex = 10;
            // 
            // VTaskHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 447);
            this.Name = "VTaskHandler";
            this.Text = "请输入：";
            this.Load += new System.EventHandler(this.VTaskHandler_Load);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel pnlShow;


    }
}