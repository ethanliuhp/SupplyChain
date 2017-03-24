namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    partial class VChosePersonByRole
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.pnlShow = new System.Windows.Forms.Panel();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.pnlShow);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.btnBack);
            this.pnlFloor.Controls.Add(this.btnNext);
            this.pnlFloor.Size = new System.Drawing.Size(312, 446);
            this.pnlFloor.Controls.SetChildIndex(this.btnNext, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnBack, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            this.pnlFloor.Controls.SetChildIndex(this.pnlShow, 0);
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Size = new System.Drawing.Size(310, 40);
            this.lblHeaderLine.Text = "请选择人员：";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(195, 366);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 23);
            this.btnOK.TabIndex = 49;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBack.Location = new System.Drawing.Point(47, 366);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(60, 23);
            this.btnBack.TabIndex = 48;
            this.btnBack.Text = "上一页";
            this.btnBack.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNext.Location = new System.Drawing.Point(121, 366);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(60, 23);
            this.btnNext.TabIndex = 47;
            this.btnNext.Text = "下一页";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // pnlShow
            // 
            this.pnlShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlShow.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlShow.Location = new System.Drawing.Point(-2, 39);
            this.pnlShow.Name = "pnlShow";
            this.pnlShow.Size = new System.Drawing.Size(313, 322);
            this.pnlShow.TabIndex = 50;
            // 
            // VChosePersonByRole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(312, 446);
            this.Name = "VChosePersonByRole";
            this.Text = "VChosePersonByRole";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel pnlShow;
    }
}