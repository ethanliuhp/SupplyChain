namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    partial class VGWBSDetailCostEditInputPriceByDtl
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
            this.btnEnter = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblPrice = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.rdInput = new System.Windows.Forms.RadioButton();
            this.rdUseRate = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(54, 138);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(75, 23);
            this.btnEnter.TabIndex = 1;
            this.btnEnter.Text = "确定";
            this.btnEnter.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(169, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(54, 89);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(190, 21);
            this.txtPrice.TabIndex = 0;
            // 
            // lblPrice
            // 
            this.lblPrice.AddColonAuto = true;
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPrice.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPrice.Location = new System.Drawing.Point(52, 74);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(65, 12);
            this.lblPrice.TabIndex = 108;
            this.lblPrice.Text = "合同单价：";
            this.lblPrice.UnderLineColor = System.Drawing.Color.Red;
            // 
            // rdInput
            // 
            this.rdInput.AutoSize = true;
            this.rdInput.Checked = true;
            this.rdInput.Location = new System.Drawing.Point(54, 36);
            this.rdInput.Name = "rdInput";
            this.rdInput.Size = new System.Drawing.Size(95, 16);
            this.rdInput.TabIndex = 109;
            this.rdInput.TabStop = true;
            this.rdInput.Text = "直接设置单价";
            this.rdInput.UseVisualStyleBackColor = true;
            // 
            // rdUseRate
            // 
            this.rdUseRate.AutoSize = true;
            this.rdUseRate.Location = new System.Drawing.Point(155, 36);
            this.rdUseRate.Name = "rdUseRate";
            this.rdUseRate.Size = new System.Drawing.Size(107, 16);
            this.rdUseRate.TabIndex = 109;
            this.rdUseRate.Text = "按系数调整单价";
            this.rdUseRate.UseVisualStyleBackColor = true;
            // 
            // VGWBSDetailCostEditInputPriceByDtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 215);
            this.Controls.Add(this.rdUseRate);
            this.Controls.Add(this.rdInput);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnEnter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "VGWBSDetailCostEditInputPriceByDtl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "单价输入";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtPrice;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPrice;
        private System.Windows.Forms.RadioButton rdInput;
        private System.Windows.Forms.RadioButton rdUseRate;
    }
}