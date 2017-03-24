namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm
{
    partial class VGWBSTreeConfirmSelect
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
            this.txtTaskHandle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConstructionSite = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTaskDetailName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Size = new System.Drawing.Size(310, 40);
            this.lblHeaderLine.Text = "工程量确认查询";
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.label3);
            this.pnlFloor.Controls.Add(this.txtTaskDetailName);
            this.pnlFloor.Controls.Add(this.txtConstructionSite);
            this.pnlFloor.Controls.Add(this.txtTaskHandle);
            this.pnlFloor.Controls.Add(this.label9);
            this.pnlFloor.Controls.Add(this.label7);
            this.pnlFloor.Size = new System.Drawing.Size(312, 447);
            this.pnlFloor.Controls.SetChildIndex(this.label7, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label9, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtTaskHandle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtConstructionSite, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtTaskDetailName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label3, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(260, -1);
            // 
            // txtTaskHandle
            // 
            this.txtTaskHandle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTaskHandle.BackColor = System.Drawing.Color.White;
            this.txtTaskHandle.Location = new System.Drawing.Point(104, 60);
            this.txtTaskHandle.Name = "txtTaskHandle";
            this.txtTaskHandle.ReadOnly = true;
            this.txtTaskHandle.Size = new System.Drawing.Size(194, 21);
            this.txtTaskHandle.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 19;
            this.label3.Text = "承担者：";
            // 
            // txtConstructionSite
            // 
            this.txtConstructionSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConstructionSite.BackColor = System.Drawing.Color.White;
            this.txtConstructionSite.Location = new System.Drawing.Point(104, 95);
            this.txtConstructionSite.Name = "txtConstructionSite";
            this.txtConstructionSite.Size = new System.Drawing.Size(194, 21);
            this.txtConstructionSite.TabIndex = 50;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 49;
            this.label7.Text = "工程任务：";
            // 
            // txtTaskDetailName
            // 
            this.txtTaskDetailName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTaskDetailName.BackColor = System.Drawing.Color.White;
            this.txtTaskDetailName.Location = new System.Drawing.Point(104, 130);
            this.txtTaskDetailName.Name = "txtTaskDetailName";
            this.txtTaskDetailName.Size = new System.Drawing.Size(194, 21);
            this.txtTaskDetailName.TabIndex = 52;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 133);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 51;
            this.label9.Text = "任务明细名称：";
            // 
            // VGWBSTreeConfirmSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 447);
            this.Name = "VGWBSTreeConfirmSelect";
            this.Text = "请选择：";
            this.Load += new System.EventHandler(this.VGWBSTreeConfirmSelect_Load);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTaskHandle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTaskDetailName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtConstructionSite;
        private System.Windows.Forms.Label label7;


    }
}