namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    partial class VDataTransfer
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
            this.lblStartDate = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.edtDate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnQr = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.webReports = new System.Windows.Forms.WebBrowser();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.webReports);
            this.pnlFloor.Controls.Add(this.btnQr);
            this.pnlFloor.Controls.Add(this.lblStartDate);
            this.pnlFloor.Controls.Add(this.edtDate);
            this.pnlFloor.Size = new System.Drawing.Size(667, 404);
            // 
            // lblStartDate
            // 
            this.lblStartDate.AddColonAuto = true;
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStartDate.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblStartDate.Location = new System.Drawing.Point(137, 39);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(65, 12);
            this.lblStartDate.TabIndex = 25;
            this.lblStartDate.Text = "选择时间：";
            this.lblStartDate.UnderLineColor = System.Drawing.Color.Red;
            // 
            // edtDate
            // 
            this.edtDate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.edtDate.DrawSelf = false;
            this.edtDate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.DateTime;
            this.edtDate.EnterToTab = false;
            this.edtDate.Location = new System.Drawing.Point(221, 35);
            this.edtDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.DateOnly;
            this.edtDate.Name = "edtDate";
            this.edtDate.Padding = new System.Windows.Forms.Padding(1);
            this.edtDate.ReadOnly = false;
            this.edtDate.Size = new System.Drawing.Size(106, 16);
            this.edtDate.TabIndex = 24;
            // 
            // btnQr
            // 
            this.btnQr.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQr.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQr.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQr.Location = new System.Drawing.Point(401, 28);
            this.btnQr.Name = "btnQr";
            this.btnQr.Size = new System.Drawing.Size(90, 23);
            this.btnQr.TabIndex = 26;
            this.btnQr.Text = "数据确认";
            // 
            // webReports
            // 
            this.webReports.Location = new System.Drawing.Point(77, 78);
            this.webReports.MinimumSize = new System.Drawing.Size(20, 20);
            this.webReports.Name = "webReports";
            this.webReports.Size = new System.Drawing.Size(509, 277);
            this.webReports.TabIndex = 27;
            // 
            // VDataTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 404);
            this.Name = "VDataTransfer";
            this.Text = "报表数据确认";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblStartDate;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit edtDate;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQr;
        private System.Windows.Forms.WebBrowser webReports;
    }
}