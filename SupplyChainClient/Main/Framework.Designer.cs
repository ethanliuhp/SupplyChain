using VirtualMachine.Core.AppHost;
using System;
namespace Application.Business.Erp.SupplyChain.Client.Main
{
    partial class Framework
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Framework));
            this.wm = new VirtualMachine.Component.WinMVC.core.WindowManager();
            this.StateTool = new System.Windows.Forms.StatusStrip();
            this.ProjectName = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblJobName = new System.Windows.Forms.ToolStripStatusLabel();
            this.LoginUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.personSign = new System.Windows.Forms.ToolStripStatusLabel();
            this.LoginDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.LoginYear = new System.Windows.Forms.ToolStripStatusLabel();
            this.LoginMonth = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblConnection = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblOrgName = new System.Windows.Forms.ToolStripStatusLabel();
            this.SupplyCompany = new System.Windows.Forms.ToolStripStatusLabel();
            this.LabScrollingMessage = new System.Windows.Forms.Label();
            this.StateTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // wm
            // 
            this.wm.AutoSize = true;
            this.wm.BackColor = System.Drawing.SystemColors.Control;
            this.wm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wm.Location = new System.Drawing.Point(0, 0);
            this.wm.Name = "wm";
            this.wm.Size = new System.Drawing.Size(747, 407);
            this.wm.TabIndex = 0;
            // 
            // StateTool
            // 
            this.StateTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProjectName,
            this.lblJobName,
            this.LoginUser,
            this.personSign,
            this.LoginDate,
            this.LoginYear,
            this.LoginMonth,
            this.lblConnection,
            this.lblOrgName,
            this.SupplyCompany});
            this.StateTool.Location = new System.Drawing.Point(0, 385);
            this.StateTool.Name = "StateTool";
            this.StateTool.Size = new System.Drawing.Size(747, 22);
            this.StateTool.TabIndex = 1;
            // 
            // ProjectName
            // 
            this.ProjectName.ForeColor = System.Drawing.Color.Red;
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(53, 17);
            this.ProjectName.Text = "登录项目";
            // 
            // lblJobName
            // 
            this.lblJobName.ForeColor = System.Drawing.Color.Red;
            this.lblJobName.Name = "lblJobName";
            this.lblJobName.Size = new System.Drawing.Size(53, 17);
            this.lblJobName.Text = "当前岗位";
            // 
            // LoginUser
            // 
            this.LoginUser.Name = "LoginUser";
            this.LoginUser.Size = new System.Drawing.Size(41, 17);
            this.LoginUser.Text = "登录人";
            // 
            // personSign
            // 
            this.personSign.DoubleClickEnabled = true;
            this.personSign.Name = "personSign";
            this.personSign.Size = new System.Drawing.Size(41, 17);
            this.personSign.Text = "签名照";
            this.personSign.ToolTipText = "请鼠标双击";
            this.personSign.DoubleClick += new System.EventHandler(this.personSign_DoubleClick);
            // 
            // LoginDate
            // 
            this.LoginDate.Name = "LoginDate";
            this.LoginDate.Size = new System.Drawing.Size(53, 17);
            this.LoginDate.Text = "登录日期";
            this.LoginDate.Visible = false;
            // 
            // LoginYear
            // 
            this.LoginYear.Name = "LoginYear";
            this.LoginYear.Size = new System.Drawing.Size(41, 17);
            this.LoginYear.Text = "会计年";
            this.LoginYear.Visible = false;
            // 
            // LoginMonth
            // 
            this.LoginMonth.BackColor = System.Drawing.Color.Transparent;
            this.LoginMonth.Name = "LoginMonth";
            this.LoginMonth.Size = new System.Drawing.Size(41, 17);
            this.LoginMonth.Text = "会计月";
            this.LoginMonth.Visible = false;
            // 
            // lblConnection
            // 
            this.lblConnection.ForeColor = System.Drawing.Color.Red;
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(53, 17);
            this.lblConnection.Text = "联接状态";
            // 
            // lblOrgName
            // 
            this.lblOrgName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblOrgName.Name = "lblOrgName";
            this.lblOrgName.Size = new System.Drawing.Size(29, 17);
            this.lblOrgName.Text = "版本";
            // 
            // SupplyCompany
            // 
            this.SupplyCompany.Margin = new System.Windows.Forms.Padding(1000, 3, 0, 2);
            this.SupplyCompany.Name = "SupplyCompany";
            this.SupplyCompany.Size = new System.Drawing.Size(23, 12);
            this.SupplyCompany.Text = "111";
            // 
            // LabScrollingMessage
            // 
            this.LabScrollingMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LabScrollingMessage.Location = new System.Drawing.Point(472, 9);
            this.LabScrollingMessage.Name = "LabScrollingMessage";
            this.LabScrollingMessage.Size = new System.Drawing.Size(206, 21);
            this.LabScrollingMessage.TabIndex = 2;
            this.LabScrollingMessage.Visible = false;
            // 
            // Framework
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 407);
            this.Controls.Add(this.LabScrollingMessage);
            this.Controls.Add(this.StateTool);
            this.Controls.Add(this.wm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Framework";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Framework";
            this.StateTool.ResumeLayout(false);
            this.StateTool.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VirtualMachine.Component.WinMVC.core.WindowManager wm;
        private System.Windows.Forms.StatusStrip StateTool;
        private System.Windows.Forms.ToolStripStatusLabel LoginUser;
        private System.Windows.Forms.ToolStripStatusLabel LoginDate;
        private System.Windows.Forms.ToolStripStatusLabel LoginYear;
        private System.Windows.Forms.ToolStripStatusLabel LoginMonth;
        private System.Windows.Forms.ToolStripStatusLabel SupplyCompany;
        private System.Windows.Forms.Label LabScrollingMessage;
        private System.Windows.Forms.ToolStripStatusLabel ProjectName;
        private System.Windows.Forms.ToolStripStatusLabel lblOrgName;
        private System.Windows.Forms.ToolStripStatusLabel lblJobName;
        private System.Windows.Forms.ToolStripStatusLabel lblConnection;
        private System.Windows.Forms.ToolStripStatusLabel personSign;
    }
}