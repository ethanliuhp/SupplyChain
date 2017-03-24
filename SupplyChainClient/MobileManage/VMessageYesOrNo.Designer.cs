namespace Application.Business.Erp.SupplyChain.Client.MobileManage
{
    partial class VMessageYesOrNo
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
            this.btnNo = new System.Windows.Forms.Button();
            this.txtInformation = new System.Windows.Forms.TextBox();
            this.btnYes = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Size = new System.Drawing.Size(310, 18);
            this.lblHeaderLine.Text = "信息提示";
            this.lblHeaderLine.Visible = false;
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnYes);
            this.pnlFloor.Controls.Add(this.btnNo);
            this.pnlFloor.Controls.Add(this.txtInformation);
            this.pnlFloor.Size = new System.Drawing.Size(312, 296);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtInformation, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnNo, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnYes, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(130, 20);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(178, 221);
            this.btnNo.Margin = new System.Windows.Forms.Padding(5);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(127, 57);
            this.btnNo.TabIndex = 5;
            this.btnNo.Text = "取   消";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // txtInformation
            // 
            this.txtInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInformation.BackColor = System.Drawing.SystemColors.Window;
            this.txtInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInformation.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInformation.Location = new System.Drawing.Point(8, 35);
            this.txtInformation.Margin = new System.Windows.Forms.Padding(5);
            this.txtInformation.Multiline = true;
            this.txtInformation.Name = "txtInformation";
            this.txtInformation.Size = new System.Drawing.Size(297, 176);
            this.txtInformation.TabIndex = 4;
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(8, 221);
            this.btnYes.Margin = new System.Windows.Forms.Padding(5);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(127, 57);
            this.btnYes.TabIndex = 6;
            this.btnYes.Text = "确   定";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // VMessageYesOrNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 296);
            this.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "VMessageYesOrNo";
            this.Text = "信息提示框";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNo;
        public System.Windows.Forms.TextBox txtInformation;
        private System.Windows.Forms.Button btnYes;

    }
}