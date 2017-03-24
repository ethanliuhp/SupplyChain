namespace Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules
{
    partial class VWeekPlanExamine
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
            this.btnGiveUp = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtPerfomance = new System.Windows.Forms.TextBox();
            this.txtCumulative = new System.Windows.Forms.TextBox();
            this.txtActualWorkTime = new System.Windows.Forms.TextBox();
            this.lblDateActualB = new System.Windows.Forms.Label();
            this.lblPerfomance = new System.Windows.Forms.Label();
            this.lblCumulative = new System.Windows.Forms.Label();
            this.lblActualWorkTime = new System.Windows.Forms.Label();
            this.lblDateActualE = new System.Windows.Forms.Label();
            this.dtpActualBeginDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpActualEndDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dtpActualEndDate);
            this.pnlFloor.Controls.Add(this.dtpActualBeginDate);
            this.pnlFloor.Controls.Add(this.btnGiveUp);
            this.pnlFloor.Controls.Add(this.btnSave);
            this.pnlFloor.Controls.Add(this.txtPerfomance);
            this.pnlFloor.Controls.Add(this.txtCumulative);
            this.pnlFloor.Controls.Add(this.txtActualWorkTime);
            this.pnlFloor.Controls.Add(this.lblDateActualB);
            this.pnlFloor.Controls.Add(this.lblPerfomance);
            this.pnlFloor.Controls.Add(this.lblCumulative);
            this.pnlFloor.Controls.Add(this.lblActualWorkTime);
            this.pnlFloor.Controls.Add(this.lblDateActualE);
            this.pnlFloor.Size = new System.Drawing.Size(312, 446);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblDateActualE, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblActualWorkTime, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblCumulative, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblPerfomance, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblDateActualB, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtActualWorkTime, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtCumulative, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtPerfomance, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSave, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnGiveUp, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtpActualBeginDate, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtpActualEndDate, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 34);
            // 
            // btnGiveUp
            // 
            this.btnGiveUp.Location = new System.Drawing.Point(173, 289);
            this.btnGiveUp.Name = "btnGiveUp";
            this.btnGiveUp.Size = new System.Drawing.Size(54, 23);
            this.btnGiveUp.TabIndex = 45;
            this.btnGiveUp.Text = "放弃";
            this.btnGiveUp.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(65, 289);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(54, 23);
            this.btnSave.TabIndex = 44;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // txtPerfomance
            // 
            this.txtPerfomance.Location = new System.Drawing.Point(134, 145);
            this.txtPerfomance.Multiline = true;
            this.txtPerfomance.Name = "txtPerfomance";
            this.txtPerfomance.Size = new System.Drawing.Size(137, 129);
            this.txtPerfomance.TabIndex = 43;
            // 
            // txtCumulative
            // 
            this.txtCumulative.Location = new System.Drawing.Point(133, 111);
            this.txtCumulative.Name = "txtCumulative";
            this.txtCumulative.Size = new System.Drawing.Size(138, 21);
            this.txtCumulative.TabIndex = 42;
            // 
            // txtActualWorkTime
            // 
            this.txtActualWorkTime.Location = new System.Drawing.Point(134, 81);
            this.txtActualWorkTime.Name = "txtActualWorkTime";
            this.txtActualWorkTime.Size = new System.Drawing.Size(137, 21);
            this.txtActualWorkTime.TabIndex = 41;
            // 
            // lblDateActualB
            // 
            this.lblDateActualB.AutoSize = true;
            this.lblDateActualB.Location = new System.Drawing.Point(38, 29);
            this.lblDateActualB.Name = "lblDateActualB";
            this.lblDateActualB.Size = new System.Drawing.Size(89, 12);
            this.lblDateActualB.TabIndex = 38;
            this.lblDateActualB.Text = "实际开始日期：";
            // 
            // lblPerfomance
            // 
            this.lblPerfomance.AutoSize = true;
            this.lblPerfomance.Location = new System.Drawing.Point(62, 145);
            this.lblPerfomance.Name = "lblPerfomance";
            this.lblPerfomance.Size = new System.Drawing.Size(65, 12);
            this.lblPerfomance.TabIndex = 37;
            this.lblPerfomance.Text = "完成情况：";
            // 
            // lblCumulative
            // 
            this.lblCumulative.AutoSize = true;
            this.lblCumulative.Location = new System.Drawing.Point(14, 120);
            this.lblCumulative.Name = "lblCumulative";
            this.lblCumulative.Size = new System.Drawing.Size(113, 12);
            this.lblCumulative.TabIndex = 36;
            this.lblCumulative.Text = "之后累积形象进度：";
            // 
            // lblActualWorkTime
            // 
            this.lblActualWorkTime.AutoSize = true;
            this.lblActualWorkTime.Location = new System.Drawing.Point(62, 90);
            this.lblActualWorkTime.Name = "lblActualWorkTime";
            this.lblActualWorkTime.Size = new System.Drawing.Size(65, 12);
            this.lblActualWorkTime.TabIndex = 35;
            this.lblActualWorkTime.Text = "实际工期：";
            // 
            // lblDateActualE
            // 
            this.lblDateActualE.AutoSize = true;
            this.lblDateActualE.Location = new System.Drawing.Point(38, 60);
            this.lblDateActualE.Name = "lblDateActualE";
            this.lblDateActualE.Size = new System.Drawing.Size(89, 12);
            this.lblDateActualE.TabIndex = 34;
            this.lblDateActualE.Text = "实际结束日期：";
            // 
            // dtpActualBeginDate
            // 
            this.dtpActualBeginDate.Location = new System.Drawing.Point(133, 20);
            this.dtpActualBeginDate.Name = "dtpActualBeginDate";
            this.dtpActualBeginDate.Size = new System.Drawing.Size(138, 21);
            this.dtpActualBeginDate.TabIndex = 46;
            // 
            // dtpActualEndDate
            // 
            this.dtpActualEndDate.Location = new System.Drawing.Point(133, 51);
            this.dtpActualEndDate.Name = "dtpActualEndDate";
            this.dtpActualEndDate.Size = new System.Drawing.Size(138, 21);
            this.dtpActualEndDate.TabIndex = 47;
            // 
            // VWeekPlanExamine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(312, 446);
            this.Name = "VWeekPlanExamine";
            this.Text = "周计划进度考核";
            this.Load += new System.EventHandler(this.VWeekPlanExamine_Load);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGiveUp;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtPerfomance;
        private System.Windows.Forms.TextBox txtCumulative;
        private System.Windows.Forms.TextBox txtActualWorkTime;
        private System.Windows.Forms.Label lblDateActualB;
        private System.Windows.Forms.Label lblPerfomance;
        private System.Windows.Forms.Label lblCumulative;
        private System.Windows.Forms.Label lblActualWorkTime;
        private System.Windows.Forms.Label lblDateActualE;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpActualEndDate;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpActualBeginDate;

    }
}