namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    partial class VScheduleApprove
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtPlanDesc = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnAgree = new System.Windows.Forms.Button();
            this.txtApproveRemark = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProjectName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPlanName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtSubmitBy = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtSubmitTime = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.ucScheduleDetailViewer1 = new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.UcScheduleDetailViewer();
            this.pnlFloor.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.ucScheduleDetailViewer1);
            this.pnlFloor.Controls.Add(this.txtSubmitTime);
            this.pnlFloor.Controls.Add(this.txtSubmitBy);
            this.pnlFloor.Controls.Add(this.txtPlanName);
            this.pnlFloor.Controls.Add(this.label6);
            this.pnlFloor.Controls.Add(this.label5);
            this.pnlFloor.Controls.Add(this.txtProjectName);
            this.pnlFloor.Controls.Add(this.label3);
            this.pnlFloor.Controls.Add(this.groupBox4);
            this.pnlFloor.Controls.Add(this.txtPlanDesc);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.label4);
            this.pnlFloor.Size = new System.Drawing.Size(1045, 622);
            this.pnlFloor.Controls.SetChildIndex(this.label4, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtPlanDesc, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox4, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label3, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtProjectName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label5, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label6, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtPlanName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtSubmitBy, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtSubmitTime, 0);
            this.pnlFloor.Controls.SetChildIndex(this.ucScheduleDetailViewer1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 40);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "计划描述：";
            // 
            // txtPlanDesc
            // 
            this.txtPlanDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPlanDesc.Location = new System.Drawing.Point(74, 39);
            this.txtPlanDesc.Name = "txtPlanDesc";
            this.txtPlanDesc.ReadOnly = true;
            this.txtPlanDesc.Size = new System.Drawing.Size(959, 21);
            this.txtPlanDesc.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.btnReject);
            this.groupBox4.Controls.Add(this.btnAgree);
            this.groupBox4.Controls.Add(this.txtApproveRemark);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(1, 561);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1042, 60);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "审批";
            // 
            // btnReject
            // 
            this.btnReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReject.Location = new System.Drawing.Point(957, 20);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 31);
            this.btnReject.TabIndex = 3;
            this.btnReject.Text = "不同意";
            this.btnReject.UseVisualStyleBackColor = true;
            // 
            // btnAgree
            // 
            this.btnAgree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAgree.Location = new System.Drawing.Point(876, 20);
            this.btnAgree.Name = "btnAgree";
            this.btnAgree.Size = new System.Drawing.Size(75, 31);
            this.btnAgree.TabIndex = 2;
            this.btnAgree.Text = "同意";
            this.btnAgree.UseVisualStyleBackColor = true;
            // 
            // txtApproveRemark
            // 
            this.txtApproveRemark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApproveRemark.Location = new System.Drawing.Point(63, 19);
            this.txtApproveRemark.Multiline = true;
            this.txtApproveRemark.Name = "txtApproveRemark";
            this.txtApproveRemark.Size = new System.Drawing.Size(807, 32);
            this.txtApproveRemark.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "审批意见";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(285, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "计划名称：";
            // 
            // txtProjectName
            // 
            this.txtProjectName.BackColor = System.Drawing.SystemColors.Control;
            this.txtProjectName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProjectName.DrawSelf = false;
            this.txtProjectName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProjectName.EnterToTab = false;
            this.txtProjectName.Location = new System.Drawing.Point(74, 12);
            this.txtProjectName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Padding = new System.Windows.Forms.Padding(1);
            this.txtProjectName.ReadOnly = true;
            this.txtProjectName.Size = new System.Drawing.Size(189, 16);
            this.txtProjectName.TabIndex = 147;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 148;
            this.label4.Text = "项目名称：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(558, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 149;
            this.label5.Text = "提交人：";
            this.label5.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(772, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 150;
            this.label6.Text = "提交时间：";
            this.label6.Visible = false;
            // 
            // txtPlanName
            // 
            this.txtPlanName.BackColor = System.Drawing.SystemColors.Control;
            this.txtPlanName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPlanName.DrawSelf = false;
            this.txtPlanName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPlanName.EnterToTab = false;
            this.txtPlanName.Location = new System.Drawing.Point(345, 12);
            this.txtPlanName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPlanName.Name = "txtPlanName";
            this.txtPlanName.Padding = new System.Windows.Forms.Padding(1);
            this.txtPlanName.ReadOnly = true;
            this.txtPlanName.Size = new System.Drawing.Size(189, 16);
            this.txtPlanName.TabIndex = 151;
            // 
            // txtSubmitBy
            // 
            this.txtSubmitBy.BackColor = System.Drawing.SystemColors.Control;
            this.txtSubmitBy.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSubmitBy.DrawSelf = false;
            this.txtSubmitBy.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSubmitBy.EnterToTab = false;
            this.txtSubmitBy.Location = new System.Drawing.Point(606, 12);
            this.txtSubmitBy.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSubmitBy.Name = "txtSubmitBy";
            this.txtSubmitBy.Padding = new System.Windows.Forms.Padding(1);
            this.txtSubmitBy.ReadOnly = true;
            this.txtSubmitBy.Size = new System.Drawing.Size(138, 16);
            this.txtSubmitBy.TabIndex = 152;
            this.txtSubmitBy.Visible = false;
            // 
            // txtSubmitTime
            // 
            this.txtSubmitTime.BackColor = System.Drawing.SystemColors.Control;
            this.txtSubmitTime.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSubmitTime.DrawSelf = false;
            this.txtSubmitTime.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSubmitTime.EnterToTab = false;
            this.txtSubmitTime.Location = new System.Drawing.Point(831, 12);
            this.txtSubmitTime.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSubmitTime.Name = "txtSubmitTime";
            this.txtSubmitTime.Padding = new System.Windows.Forms.Padding(1);
            this.txtSubmitTime.ReadOnly = true;
            this.txtSubmitTime.Size = new System.Drawing.Size(202, 16);
            this.txtSubmitTime.TabIndex = 153;
            this.txtSubmitTime.Visible = false;
            // 
            // ucScheduleDetailViewer1
            // 
            this.ucScheduleDetailViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucScheduleDetailViewer1.Location = new System.Drawing.Point(1, 64);
            this.ucScheduleDetailViewer1.Name = "ucScheduleDetailViewer1";
            this.ucScheduleDetailViewer1.Size = new System.Drawing.Size(1042, 491);
            this.ucScheduleDetailViewer1.TabIndex = 154;
            // 
            // VScheduleApprove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 622);
            this.Name = "VScheduleApprove";
            this.Text = "总进度计划审批";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtPlanDesc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnAgree;
        private System.Windows.Forms.TextBox txtApproveRemark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProjectName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSubmitTime;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSubmitBy;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPlanName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private UcScheduleDetailViewer ucScheduleDetailViewer1;
    }
}