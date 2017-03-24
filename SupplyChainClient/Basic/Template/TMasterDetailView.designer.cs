using VirtualMachine.Component.WinMVC.core;
using System;
namespace Application.Business.Erp.SupplyChain.Client.Basic.Template
{
    partial class TMasterDetailView
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
            try
            {
                base.Dispose(disposing);
            }
            catch (Exception e)
            {
 
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlFloor = new System.Windows.Forms.Panel();
            this.pnlWorkSpace = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.spTop = new VirtualMachine.Component.WinMVC.core.SimplePanel();
            this.pnlFloor.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.AutoScroll = true;
            this.pnlFloor.BackColor = System.Drawing.SystemColors.Control;
            //this.pnlFloor.Controls.Add(this.pnlContent);
            this.pnlFloor.Controls.Add(this.pnlBody);
            this.pnlFloor.Controls.Add(this.pnlFooter);
            this.pnlFloor.Controls.Add(this.pnlHeader);
            this.pnlFloor.Controls.Add(this.spTop);
            this.pnlFloor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFloor.Location = new System.Drawing.Point(0, 0);
            this.pnlFloor.Name = "pnlFloor";
            this.pnlFloor.Size = new System.Drawing.Size(634, 325);
            this.pnlFloor.TabIndex = 4;
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.AutoScroll = true;
            this.pnlWorkSpace.BackColor = System.Drawing.Color.LightSteelBlue;
            //this.pnlWorkSpace.Controls.Add(this.pnlContent);
            this.pnlWorkSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWorkSpace.Location = new System.Drawing.Point(0, 0);
            this.pnlWorkSpace.Name = "pnlWorkSpace";
            this.pnlWorkSpace.Size = new System.Drawing.Size(1, 1);
            this.pnlWorkSpace.TabIndex = 5;
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContent.BackColor = System.Drawing.SystemColors.Control;
            this.pnlContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            //this.pnlContent.Controls.Add(this.pnlBody);
            //this.pnlContent.Controls.Add(this.pnlFooter);
            //this.pnlContent.Controls.Add(this.pnlHeader);
            this.pnlContent.Location = new System.Drawing.Point(10, 10);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1, 1);
            this.pnlContent.TabIndex = 3;
            // 
            // pnlBody
            // 
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 101);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.pnlBody.Size = new System.Drawing.Size(610, 167);
            this.pnlBody.TabIndex = 12;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 268);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(610, 38);
            this.pnlFooter.TabIndex = 11;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlHeader.Size = new System.Drawing.Size(610, 101);
            this.pnlHeader.TabIndex = 10;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("楷体_GB2312", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTitle.Location = new System.Drawing.Point(299, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(51, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "标题";
            // 
            // spTop
            // 
            this.spTop.BackColor = System.Drawing.SystemColors.Control;
            this.spTop.BottomLine = true;
            this.spTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.spTop.DrawSelf = true;
            this.spTop.InnerVisible = true;
            this.spTop.Location = new System.Drawing.Point(0, 0);
            this.spTop.Name = "spTop";
            this.spTop.Padding = new System.Windows.Forms.Padding(1);
            this.spTop.Size = new System.Drawing.Size(634, 0);
            this.spTop.TabIndex = 4;
            this.spTop.TopLine = false;
            // 
            // TMasterDetailView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 325);
            this.Controls.Add(this.pnlFloor);
            this.KeyPreview = true;
            this.Name = "TMasterDetailView";
            this.pnlFloor.ResumeLayout(false);
            this.pnlWorkSpace.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel pnlContent;
        protected System.Windows.Forms.Panel pnlBody;
        protected System.Windows.Forms.Panel pnlFooter;
        protected SimplePanel spTop;
        protected System.Windows.Forms.Panel pnlFloor;
        protected System.Windows.Forms.Panel pnlWorkSpace;
        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblTitle;
    }
}
