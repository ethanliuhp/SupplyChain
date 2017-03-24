namespace Application.Business.Erp.SupplyChain.Client.Basic.Template
{
    partial class TSystemView
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
            this.pnlFloor = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFloor.Location = new System.Drawing.Point(0, 0);
            this.pnlFloor.Name = "pnlFloor";
            this.pnlFloor.Size = new System.Drawing.Size(390, 266);
            this.pnlFloor.TabIndex = 1;
            // 
            // TSystemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 266);
            this.Controls.Add(this.pnlFloor);
            this.Name = "TSystemView";
            this.Text = "TSystemView";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnlFloor;
    }
}