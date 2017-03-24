namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    partial class VWaringServerControl
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
            this.components = new System.ComponentModel.Container();
            this.customlabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblState = new System.Windows.Forms.Label();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.lblStartTime);
            this.pnlFloor.Controls.Add(this.lblState);
            this.pnlFloor.Controls.Add(this.btnStart);
            this.pnlFloor.Controls.Add(this.btnStop);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Controls.Add(this.customlabel2);
            this.pnlFloor.Size = new System.Drawing.Size(908, 511);
            this.pnlFloor.Controls.SetChildIndex(this.customlabel2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnStop, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnStart, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblState, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblStartTime, 0);
            // 
            // customlabel2
            // 
            this.customlabel2.AddColonAuto = true;
            this.customlabel2.AutoSize = true;
            this.customlabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customlabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customlabel2.Location = new System.Drawing.Point(101, 126);
            this.customlabel2.Name = "customlabel2";
            this.customlabel2.Size = new System.Drawing.Size(89, 12);
            this.customlabel2.TabIndex = 162;
            this.customlabel2.Text = "当前启动状态：";
            this.customlabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(103, 216);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(132, 23);
            this.btnStop.TabIndex = 163;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(103, 70);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(132, 23);
            this.btnStart.TabIndex = 165;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(101, 175);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(89, 12);
            this.customLabel1.TabIndex = 162;
            this.customLabel1.Text = "最后启动时间：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblState.ForeColor = System.Drawing.Color.Red;
            this.lblState.Location = new System.Drawing.Point(187, 117);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(68, 27);
            this.lblState.TabIndex = 166;
            this.lblState.Text = "状态";
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.ForeColor = System.Drawing.Color.Red;
            this.lblStartTime.Location = new System.Drawing.Point(190, 175);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(53, 12);
            this.lblStartTime.TabIndex = 166;
            this.lblStartTime.Text = "启动时间";
            // 
            // VWaringServerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(908, 511);
            this.Name = "VWaringServerControl";
            this.Text = "预警服务控制台";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private VirtualMachine.Component.WinControls.Controls.CustomLabel customlabel2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Timer timer1;
	}
}