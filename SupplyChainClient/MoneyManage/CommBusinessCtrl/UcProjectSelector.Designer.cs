﻿namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    partial class UcProjectSelector
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
            this.txtProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnProject = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtProject
            // 
            this.txtProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProject.BackColor = System.Drawing.SystemColors.Control;
            this.txtProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProject.DrawSelf = false;
            this.txtProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProject.EnterToTab = false;
            this.txtProject.Location = new System.Drawing.Point(47, 4);
            this.txtProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProject.Name = "txtProject";
            this.txtProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtProject.ReadOnly = true;
            this.txtProject.Size = new System.Drawing.Size(229, 16);
            this.txtProject.TabIndex = 145;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(3, 6);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(41, 12);
            this.customLabel1.TabIndex = 146;
            this.customLabel1.Text = "项目 :";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnProject
            // 
            this.btnProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProject.AutoSize = true;
            this.btnProject.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnProject.Location = new System.Drawing.Point(278, 1);
            this.btnProject.Name = "btnProject";
            this.btnProject.Size = new System.Drawing.Size(39, 22);
            this.btnProject.TabIndex = 144;
            this.btnProject.Text = "选择";
            this.btnProject.UseVisualStyleBackColor = true;
            // 
            // UcProjectSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.txtProject);
            this.Controls.Add(this.customLabel1);
            this.Controls.Add(this.btnProject);
            this.Name = "UcProjectSelector";
            this.Size = new System.Drawing.Size(320, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.Button btnProject;

    }
}
