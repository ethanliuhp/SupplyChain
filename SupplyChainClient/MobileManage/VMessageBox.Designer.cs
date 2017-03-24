namespace Application.Business.Erp.SupplyChain.Client.MobileManage
{
    partial class VMessageBox
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
            this.btnExit = new System.Windows.Forms.Button();
            this.txtInformation = new System.Windows.Forms.TextBox();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Size = new System.Drawing.Size(579, 40);
            this.lblHeaderLine.Visible = false;
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnExit);
            this.pnlFloor.Controls.Add(this.txtInformation);
            this.pnlFloor.Size = new System.Drawing.Size(581, 370);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtInformation, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExit, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(278, 20);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(374, 278);
            this.btnExit.Margin = new System.Windows.Forms.Padding(5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(127, 46);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "关   闭";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // txtInformation
            // 
            this.txtInformation.BackColor = System.Drawing.SystemColors.Window;
            this.txtInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInformation.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInformation.Location = new System.Drawing.Point(1, 38);
            this.txtInformation.Margin = new System.Windows.Forms.Padding(5);
            this.txtInformation.Multiline = true;
            this.txtInformation.Name = "txtInformation";
            this.txtInformation.Size = new System.Drawing.Size(577, 230);
            this.txtInformation.TabIndex = 4;
            // 
            // VMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 370);
            this.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "VMessageBox";
            this.Text = "信息提示框";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        public System.Windows.Forms.TextBox txtInformation;

    }
}