namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut
{
    partial class VLossOutSthCon
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.customGroupBox1 = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.txtCodeEnd = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.txtCodeBegin = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.optTally = new VirtualMachine.Component.WinControls.Controls.CustomRadioButton();
            this.optNoTally = new VirtualMachine.Component.WinControls.Controls.CustomRadioButton();
            this.optAll = new VirtualMachine.Component.WinControls.Controls.CustomRadioButton();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.customGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // floor
            // 
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.customGroupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(377, 219);
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.txtCodeEnd);
            this.customGroupBox1.Controls.Add(this.txtCodeBegin);
            this.customGroupBox1.Controls.Add(this.customLabel5);
            this.customGroupBox1.Controls.Add(this.customLabel4);
            this.customGroupBox1.Controls.Add(this.optTally);
            this.customGroupBox1.Controls.Add(this.optNoTally);
            this.customGroupBox1.Controls.Add(this.optAll);
            this.customGroupBox1.Controls.Add(this.customLabel3);
            this.customGroupBox1.Controls.Add(this.dtpDateEnd);
            this.customGroupBox1.Controls.Add(this.dtpDateBegin);
            this.customGroupBox1.Controls.Add(this.customLabel1);
            this.customGroupBox1.Location = new System.Drawing.Point(12, 23);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(346, 141);
            this.customGroupBox1.TabIndex = 13;
            this.customGroupBox1.TabStop = false;
            // 
            // txtCodeEnd
            // 
            this.txtCodeEnd.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeEnd.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeEnd.DrawSelf = false;
            this.txtCodeEnd.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeEnd.EnterToTab = false;
            this.txtCodeEnd.Location = new System.Drawing.Point(190, 23);
            this.txtCodeEnd.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeEnd.Name = "txtCodeEnd";
            this.txtCodeEnd.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeEnd.ReadOnly = false;
            this.txtCodeEnd.Size = new System.Drawing.Size(104, 16);
            this.txtCodeEnd.TabIndex = 24;
            // 
            // txtCodeBegin
            // 
            this.txtCodeBegin.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBegin.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBegin.DrawSelf = false;
            this.txtCodeBegin.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBegin.EnterToTab = false;
            this.txtCodeBegin.Location = new System.Drawing.Point(78, 23);
            this.txtCodeBegin.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBegin.Name = "txtCodeBegin";
            this.txtCodeBegin.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBegin.ReadOnly = false;
            this.txtCodeBegin.Size = new System.Drawing.Size(104, 16);
            this.txtCodeBegin.TabIndex = 25;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(179, 27);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(11, 12);
            this.customLabel5.TabIndex = 22;
            this.customLabel5.Text = "-";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(8, 29);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(35, 12);
            this.customLabel4.TabIndex = 23;
            this.customLabel4.Text = "单号:";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // optTally
            // 
            this.optTally.AutoSize = true;
            this.optTally.Location = new System.Drawing.Point(199, 111);
            this.optTally.Name = "optTally";
            this.optTally.Size = new System.Drawing.Size(59, 16);
            this.optTally.TabIndex = 12;
            this.optTally.Text = "已记账";
            this.optTally.UseVisualStyleBackColor = true;
            // 
            // optNoTally
            // 
            this.optNoTally.AutoSize = true;
            this.optNoTally.Location = new System.Drawing.Point(131, 111);
            this.optNoTally.Name = "optNoTally";
            this.optNoTally.Size = new System.Drawing.Size(59, 16);
            this.optNoTally.TabIndex = 14;
            this.optNoTally.Text = "未记账";
            this.optNoTally.UseVisualStyleBackColor = true;
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Checked = true;
            this.optAll.Location = new System.Drawing.Point(78, 111);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(47, 16);
            this.optAll.TabIndex = 13;
            this.optAll.TabStop = true;
            this.optAll.Text = "全部";
            this.optAll.UseVisualStyleBackColor = true;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(8, 113);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 11;
            this.customLabel3.Text = "记账标志:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(199, 45);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 8;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(78, 45);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 6;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(8, 54);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(35, 12);
            this.customLabel1.TabIndex = 7;
            this.customLabel1.Text = "日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(283, 184);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(182, 184);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // VStockMoveSthCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(377, 219);
            this.Name = "VStockMoveSthCon";
            this.pnlFloor.ResumeLayout(false);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox customGroupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private VirtualMachine.Component.WinControls.Controls.CustomRadioButton optTally;
        private VirtualMachine.Component.WinControls.Controls.CustomRadioButton optNoTally;
        private VirtualMachine.Component.WinControls.Controls.CustomRadioButton optAll;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
    }
}
