namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.CommonControl
{
    partial class UcTenantSelector
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnTenantSelector = new System.Windows.Forms.Button();
            this.txtTenant = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(2, 5);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(47, 12);
            this.customLabel1.TabIndex = 149;
            this.customLabel1.Text = "租赁方:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnTenantSelector
            // 
            this.btnTenantSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTenantSelector.AutoSize = true;
            this.btnTenantSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnTenantSelector.Location = new System.Drawing.Point(184, 0);
            this.btnTenantSelector.Name = "btnTenantSelector";
            this.btnTenantSelector.Size = new System.Drawing.Size(39, 22);
            this.btnTenantSelector.TabIndex = 147;
            this.btnTenantSelector.Text = "选择";
            this.btnTenantSelector.UseVisualStyleBackColor = true;
            // 
            // txtTenant
            // 
            this.txtTenant.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTenant.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTenant.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtTenant.Location = new System.Drawing.Point(56, 1);
            this.txtTenant.Name = "txtTenant";
            this.txtTenant.Size = new System.Drawing.Size(122, 21);
            this.txtTenant.TabIndex = 150;
            // 
            // UcTenantSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnTenantSelector);
            this.Controls.Add(this.txtTenant);
            this.Controls.Add(this.customLabel1);
            this.Name = "UcTenantSelector";
            this.Size = new System.Drawing.Size(223, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.Button btnTenantSelector;
        private System.Windows.Forms.TextBox txtTenant;
    }
}
