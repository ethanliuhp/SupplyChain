namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.DailyInspectionRecord
{
    partial class VDailyInspectionResult
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
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtInspectionContent = new System.Windows.Forms.TextBox();
            this.btnCamera = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblRecordTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblRecord = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnTheLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnTheFirst = new System.Windows.Forms.Button();
            this.txtProjectTask = new System.Windows.Forms.TextBox();
            this.txtTaskFullPath = new System.Windows.Forms.TextBox();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Size = new System.Drawing.Size(310, 38);
            this.lblHeaderLine.Text = "日常检查记录";
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.txtTaskFullPath);
            this.pnlFloor.Controls.Add(this.txtProjectTask);
            this.pnlFloor.Controls.Add(this.btnTheLast);
            this.pnlFloor.Controls.Add(this.btnNext);
            this.pnlFloor.Controls.Add(this.btnBack);
            this.pnlFloor.Controls.Add(this.btnTheFirst);
            this.pnlFloor.Controls.Add(this.lblRecord);
            this.pnlFloor.Controls.Add(this.lblRecordTotal);
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Controls.Add(this.btnCamera);
            this.pnlFloor.Controls.Add(this.txtInspectionContent);
            this.pnlFloor.Controls.Add(this.customLabel3);
            this.pnlFloor.Controls.Add(this.customLabel2);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Size = new System.Drawing.Size(312, 446);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel3, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtInspectionContent, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCamera, 0);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecordTotal, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecord, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnTheFirst, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnBack, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnNext, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnTheLast, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtProjectTask, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtTaskFullPath, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 27);
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(34, 85);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(77, 12);
            this.customLabel2.TabIndex = 18;
            this.customLabel2.Text = "任务全路径：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(46, 51);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 17;
            this.customLabel1.Text = "任务名称：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(34, 145);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(65, 12);
            this.customLabel3.TabIndex = 21;
            this.customLabel3.Text = "检查说明：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtInspectionContent
            // 
            this.txtInspectionContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInspectionContent.Location = new System.Drawing.Point(24, 165);
            this.txtInspectionContent.Multiline = true;
            this.txtInspectionContent.Name = "txtInspectionContent";
            this.txtInspectionContent.Size = new System.Drawing.Size(260, 148);
            this.txtInspectionContent.TabIndex = 22;
            // 
            // btnCamera
            // 
            this.btnCamera.Location = new System.Drawing.Point(209, 323);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(75, 23);
            this.btnCamera.TabIndex = 23;
            this.btnCamera.Text = "拍照";
            this.btnCamera.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(5, 114);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 1);
            this.panel1.TabIndex = 24;
            // 
            // lblRecordTotal
            // 
            this.lblRecordTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblRecordTotal.AutoSize = true;
            this.lblRecordTotal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecordTotal.ForeColor = System.Drawing.Color.Red;
            this.lblRecordTotal.Location = new System.Drawing.Point(240, 118);
            this.lblRecordTotal.Name = "lblRecordTotal";
            this.lblRecordTotal.Size = new System.Drawing.Size(59, 12);
            this.lblRecordTotal.TabIndex = 115;
            this.lblRecordTotal.Text = "共【0】条";
            this.lblRecordTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblRecord
            // 
            this.lblRecord.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblRecord.AutoSize = true;
            this.lblRecord.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecord.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecord.ForeColor = System.Drawing.Color.Red;
            this.lblRecord.Location = new System.Drawing.Point(166, 118);
            this.lblRecord.Name = "lblRecord";
            this.lblRecord.Size = new System.Drawing.Size(59, 12);
            this.lblRecord.TabIndex = 116;
            this.lblRecord.Text = "第【0】条";
            this.lblRecord.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnTheLast
            // 
            this.btnTheLast.Location = new System.Drawing.Point(248, 371);
            this.btnTheLast.Name = "btnTheLast";
            this.btnTheLast.Size = new System.Drawing.Size(55, 20);
            this.btnTheLast.TabIndex = 132;
            this.btnTheLast.Text = "末条";
            this.btnTheLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(171, 371);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(55, 20);
            this.btnNext.TabIndex = 131;
            this.btnNext.Text = "下一条";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(94, 371);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(55, 20);
            this.btnBack.TabIndex = 130;
            this.btnBack.Text = "上一条";
            this.btnBack.UseVisualStyleBackColor = true;
            // 
            // btnTheFirst
            // 
            this.btnTheFirst.Location = new System.Drawing.Point(17, 371);
            this.btnTheFirst.Name = "btnTheFirst";
            this.btnTheFirst.Size = new System.Drawing.Size(55, 20);
            this.btnTheFirst.TabIndex = 129;
            this.btnTheFirst.Text = "首条";
            this.btnTheFirst.UseVisualStyleBackColor = true;
            // 
            // txtProjectTask
            // 
            this.txtProjectTask.Location = new System.Drawing.Point(117, 41);
            this.txtProjectTask.Name = "txtProjectTask";
            this.txtProjectTask.Size = new System.Drawing.Size(161, 21);
            this.txtProjectTask.TabIndex = 159;
            // 
            // txtTaskFullPath
            // 
            this.txtTaskFullPath.Location = new System.Drawing.Point(117, 74);
            this.txtTaskFullPath.Multiline = true;
            this.txtTaskFullPath.Name = "txtTaskFullPath";
            this.txtTaskFullPath.Size = new System.Drawing.Size(161, 29);
            this.txtTaskFullPath.TabIndex = 162;
            // 
            // VDailyInspectionResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(312, 446);
            this.Name = "VDailyInspectionResult";
            this.Text = "日常检查记录";
            this.Load += new System.EventHandler(this.VDailyInspectionResult_Load);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private System.Windows.Forms.TextBox txtInspectionContent;
        private System.Windows.Forms.Button btnCamera;
        private System.Windows.Forms.Panel panel1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecordTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecord;
        private System.Windows.Forms.Button btnTheLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnTheFirst;
        private System.Windows.Forms.TextBox txtProjectTask;
        private System.Windows.Forms.TextBox txtTaskFullPath;
    }
}