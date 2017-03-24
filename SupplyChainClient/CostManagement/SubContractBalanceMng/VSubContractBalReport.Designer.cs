﻿namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    partial class VSubContractBalReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSubContractBalReport));
            this.tabSubBill = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fGridMain = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tabDetail = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.fGridDetail = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnClose = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnPrint = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnPreview = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.tabSubBill.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabDetail.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSubBill
            // 
            this.tabSubBill.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSubBill.Controls.Add(this.tabMain);
            this.tabSubBill.Controls.Add(this.tabDetail);
            this.tabSubBill.Location = new System.Drawing.Point(12, 36);
            this.tabSubBill.Name = "tabSubBill";
            this.tabSubBill.SelectedIndex = 0;
            this.tabSubBill.Size = new System.Drawing.Size(1052, 480);
            this.tabSubBill.TabIndex = 0;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.panel1);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(1044, 454);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "分包结算封面";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fGridMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1038, 448);
            this.panel1.TabIndex = 0;
            // 
            // fGridMain
            // 
            this.fGridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGridMain.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGridMain.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridMain.CheckedImage")));
            this.fGridMain.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.fGridMain.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGridMain.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGridMain.Location = new System.Drawing.Point(3, 3);
            this.fGridMain.Name = "fGridMain";
            this.fGridMain.Rows = 10;
            this.fGridMain.Size = new System.Drawing.Size(1032, 442);
            this.fGridMain.TabIndex = 7;
            this.fGridMain.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridMain.UncheckedImage")));
            // 
            // tabDetail
            // 
            this.tabDetail.Controls.Add(this.panel2);
            this.tabDetail.Location = new System.Drawing.Point(4, 22);
            this.tabDetail.Name = "tabDetail";
            this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabDetail.Size = new System.Drawing.Size(1050, 463);
            this.tabDetail.TabIndex = 1;
            this.tabDetail.Text = "分包结算明细";
            this.tabDetail.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.fGridDetail);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1044, 457);
            this.panel2.TabIndex = 0;
            // 
            // fGridDetail
            // 
            this.fGridDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGridDetail.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail.CheckedImage")));
            this.fGridDetail.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.fGridDetail.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGridDetail.Location = new System.Drawing.Point(3, 3);
            this.fGridDetail.Name = "fGridDetail";
            this.fGridDetail.Rows = 10;
            this.fGridDetail.Size = new System.Drawing.Size(1038, 451);
            this.fGridDetail.TabIndex = 8;
            this.fGridDetail.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail.UncheckedImage")));
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(756, 2);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 15;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnClose.Location = new System.Drawing.Point(837, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 28);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "关 闭";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPrint.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnPrint.Location = new System.Drawing.Point(675, 2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 28);
            this.btnPrint.TabIndex = 17;
            this.btnPrint.Text = "打 印";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnPreview
            // 
            this.btnPreview.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPreview.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnPreview.Location = new System.Drawing.Point(594, 2);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 28);
            this.btnPreview.TabIndex = 18;
            this.btnPreview.Text = "预 览";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Visible = false;
            // 
            // VSubContractBalReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 528);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.tabSubBill);
            this.Name = "VSubContractBalReport";
            this.Text = "分包结算打印报表";
            this.tabSubBill.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabDetail.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabSubBill;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.TabPage tabDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private System.Windows.Forms.Panel panel1;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGridMain;
        private System.Windows.Forms.Panel panel2;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGridDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnClose;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnPrint;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnPreview;
    }
}