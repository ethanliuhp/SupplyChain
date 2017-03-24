namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    partial class AccMonthPicker
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
            this.monthbox = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.SuspendLayout();
            // 
            // monthbox
            // 
            this.monthbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monthbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.monthbox.FormattingEnabled = true;
            this.monthbox.Location = new System.Drawing.Point(0, 0);
            this.monthbox.Name = "monthbox";
            this.monthbox.Size = new System.Drawing.Size(121, 20);
            this.monthbox.TabIndex = 0;
            // 
            // AccMonthPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.monthbox);
            this.Name = "AccMonthPicker";
            this.Size = new System.Drawing.Size(121, 20);
            this.ResumeLayout(false);

        }

        #endregion
        public VirtualMachine.Component.WinControls.Controls.CustomComboBox monthbox;
    }
}
