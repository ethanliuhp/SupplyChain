namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    partial class VMobileDailyWorkMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VMobileDailyWorkMenu));
            this.btnDailyInspection = new System.Windows.Forms.Button();
            this.btnWeekSchedule = new System.Windows.Forms.Button();
            this.btnGWBSConfirm = new System.Windows.Forms.Button();
            this.btnDailyCorrection = new System.Windows.Forms.Button();
            this.txtTaskFullPath = new System.Windows.Forms.TextBox();
            this.txtWeekSchedule = new System.Windows.Forms.TextBox();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Size = new System.Drawing.Size(310, 38);
            this.lblHeaderLine.Text = "选择日常工作";
            this.lblHeaderLine.Visible = false;
            // 
            // pnlFloor
            // 
            this.pnlFloor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlFloor.Controls.Add(this.txtTaskFullPath);
            this.pnlFloor.Controls.Add(this.txtWeekSchedule);
            this.pnlFloor.Controls.Add(this.customLabel4);
            this.pnlFloor.Controls.Add(this.customLabel2);
            this.pnlFloor.Controls.Add(this.btnDailyCorrection);
            this.pnlFloor.Controls.Add(this.btnGWBSConfirm);
            this.pnlFloor.Controls.Add(this.btnWeekSchedule);
            this.pnlFloor.Controls.Add(this.btnDailyInspection);
            this.pnlFloor.Size = new System.Drawing.Size(312, 446);
            this.pnlFloor.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFloor_Paint);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnDailyInspection, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnWeekSchedule, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnGWBSConfirm, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnDailyCorrection, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel4, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtWeekSchedule, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtTaskFullPath, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // btnDailyInspection
            // 
            this.btnDailyInspection.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDailyInspection.BackgroundImage")));
            this.btnDailyInspection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDailyInspection.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnDailyInspection.Location = new System.Drawing.Point(25, 181);
            this.btnDailyInspection.Name = "btnDailyInspection";
            this.btnDailyInspection.Size = new System.Drawing.Size(115, 72);
            this.btnDailyInspection.TabIndex = 60;
            this.btnDailyInspection.Text = "日常检查";
            this.btnDailyInspection.UseVisualStyleBackColor = true;
            // 
            // btnWeekSchedule
            // 
            this.btnWeekSchedule.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnWeekSchedule.BackgroundImage")));
            this.btnWeekSchedule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnWeekSchedule.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnWeekSchedule.Location = new System.Drawing.Point(173, 181);
            this.btnWeekSchedule.Name = "btnWeekSchedule";
            this.btnWeekSchedule.Size = new System.Drawing.Size(115, 72);
            this.btnWeekSchedule.TabIndex = 61;
            this.btnWeekSchedule.Text = "周计划确认";
            this.btnWeekSchedule.UseVisualStyleBackColor = true;
            // 
            // btnGWBSConfirm
            // 
            this.btnGWBSConfirm.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGWBSConfirm.BackgroundImage")));
            this.btnGWBSConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGWBSConfirm.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnGWBSConfirm.Location = new System.Drawing.Point(25, 292);
            this.btnGWBSConfirm.Name = "btnGWBSConfirm";
            this.btnGWBSConfirm.Size = new System.Drawing.Size(115, 72);
            this.btnGWBSConfirm.TabIndex = 62;
            this.btnGWBSConfirm.Text = "工程量确认";
            this.btnGWBSConfirm.UseVisualStyleBackColor = true;
            // 
            // btnDailyCorrection
            // 
            this.btnDailyCorrection.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDailyCorrection.BackgroundImage")));
            this.btnDailyCorrection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDailyCorrection.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnDailyCorrection.Location = new System.Drawing.Point(173, 292);
            this.btnDailyCorrection.Name = "btnDailyCorrection";
            this.btnDailyCorrection.Size = new System.Drawing.Size(115, 72);
            this.btnDailyCorrection.TabIndex = 63;
            this.btnDailyCorrection.Text = "整改单确认";
            this.btnDailyCorrection.UseVisualStyleBackColor = true;
            // 
            // txtTaskFullPath
            // 
            this.txtTaskFullPath.Location = new System.Drawing.Point(122, 112);
            this.txtTaskFullPath.Multiline = true;
            this.txtTaskFullPath.Name = "txtTaskFullPath";
            this.txtTaskFullPath.Size = new System.Drawing.Size(148, 29);
            this.txtTaskFullPath.TabIndex = 155;
            // 
            // txtWeekSchedule
            // 
            this.txtWeekSchedule.Location = new System.Drawing.Point(122, 66);
            this.txtWeekSchedule.Name = "txtWeekSchedule";
            this.txtWeekSchedule.Size = new System.Drawing.Size(148, 21);
            this.txtWeekSchedule.TabIndex = 154;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(39, 115);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(77, 12);
            this.customLabel4.TabIndex = 153;
            this.customLabel4.Text = "任务全路径：";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(51, 69);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(65, 12);
            this.customLabel2.TabIndex = 151;
            this.customLabel2.Text = "计划名称：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VMobileDailyWorkMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(312, 446);
            this.Name = "VMobileDailyWorkMenu";
            this.Text = "选择日常工作";
            this.Load += new System.EventHandler(this.VMobileDailyWorkMenu_Load);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDailyInspection;
        private System.Windows.Forms.Button btnDailyCorrection;
        private System.Windows.Forms.Button btnGWBSConfirm;
        private System.Windows.Forms.Button btnWeekSchedule;
        private System.Windows.Forms.TextBox txtTaskFullPath;
        private System.Windows.Forms.TextBox txtWeekSchedule;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
    }
}