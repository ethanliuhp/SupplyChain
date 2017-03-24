namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    partial class VViewFreeQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VViewFreeQuery));
            this.lblView = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cboView = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnPreview = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dgvWrite = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.cboYwzz = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.lblYwzz = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnFs = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.cboReportType = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.lblReportType = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnTime = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.edtTime = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.edtTime);
            this.pnlFloor.Controls.Add(this.btnTime);
            this.pnlFloor.Controls.Add(this.cboReportType);
            this.pnlFloor.Controls.Add(this.lblReportType);
            this.pnlFloor.Controls.Add(this.btnFs);
            this.pnlFloor.Controls.Add(this.cboYwzz);
            this.pnlFloor.Controls.Add(this.lblYwzz);
            this.pnlFloor.Controls.Add(this.dgvWrite);
            this.pnlFloor.Controls.Add(this.btnExcel);
            this.pnlFloor.Controls.Add(this.btnPreview);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Controls.Add(this.cboView);
            this.pnlFloor.Controls.Add(this.lblView);
            this.pnlFloor.Size = new System.Drawing.Size(992, 554);
            // 
            // lblView
            // 
            this.lblView.AutoSize = true;
            this.lblView.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblView.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblView.Location = new System.Drawing.Point(400, 9);
            this.lblView.Name = "lblView";
            this.lblView.Size = new System.Drawing.Size(65, 12);
            this.lblView.TabIndex = 1;
            this.lblView.Text = "选择模板：";
            this.lblView.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cboView
            // 
            this.cboView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboView.FormattingEnabled = true;
            this.cboView.Location = new System.Drawing.Point(471, 3);
            this.cboView.Name = "cboView";
            this.cboView.Size = new System.Drawing.Size(224, 20);
            this.cboView.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(710, 28);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "查　询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // btnPreview
            // 
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPreview.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnPreview.Location = new System.Drawing.Point(312, 528);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 7;
            this.btnPreview.Text = "预　览";
            this.btnPreview.UseVisualStyleBackColor = true;
            // 
            // btnExcel
            // 
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(416, 528);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 8;
            this.btnExcel.Text = "导出Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // dgvWrite
            // 
            this.dgvWrite.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("dgvWrite.CheckedImage")));
            this.dgvWrite.Cols = 12;
            this.dgvWrite.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.dgvWrite.Font = new System.Drawing.Font("SimSun", 9F);
            this.dgvWrite.Location = new System.Drawing.Point(12, 56);
            this.dgvWrite.Name = "dgvWrite";
            this.dgvWrite.Rows = 25;
            this.dgvWrite.Size = new System.Drawing.Size(977, 466);
            this.dgvWrite.TabIndex = 10;
            this.dgvWrite.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("dgvWrite.UncheckedImage")));
            this.dgvWrite.KeyUp += new FlexCell.Grid.KeyUpEventHandler(this.dgvWrite_KeyUp);
            // 
            // cboYwzz
            // 
            this.cboYwzz.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboYwzz.FormattingEnabled = true;
            this.cboYwzz.Location = new System.Drawing.Point(209, 31);
            this.cboYwzz.Name = "cboYwzz";
            this.cboYwzz.Size = new System.Drawing.Size(160, 20);
            this.cboYwzz.TabIndex = 12;
            // 
            // lblYwzz
            // 
            this.lblYwzz.AutoSize = true;
            this.lblYwzz.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblYwzz.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblYwzz.Location = new System.Drawing.Point(114, 37);
            this.lblYwzz.Name = "lblYwzz";
            this.lblYwzz.Size = new System.Drawing.Size(89, 12);
            this.lblYwzz.TabIndex = 11;
            this.lblYwzz.Text = "选择业务组织：";
            this.lblYwzz.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnFs
            // 
            this.btnFs.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFs.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFs.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnFs.Location = new System.Drawing.Point(804, 26);
            this.btnFs.Name = "btnFs";
            this.btnFs.Size = new System.Drawing.Size(75, 23);
            this.btnFs.TabIndex = 13;
            this.btnFs.Text = "反  审";
            this.btnFs.UseVisualStyleBackColor = true;
            // 
            // cboReportType
            // 
            this.cboReportType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboReportType.FormattingEnabled = true;
            this.cboReportType.Location = new System.Drawing.Point(209, 3);
            this.cboReportType.Name = "cboReportType";
            this.cboReportType.Size = new System.Drawing.Size(160, 20);
            this.cboReportType.TabIndex = 15;
            this.cboReportType.SelectedIndexChanged += new System.EventHandler(this.cboReportType_SelectedIndexChanged);
            // 
            // lblReportType
            // 
            this.lblReportType.AutoSize = true;
            this.lblReportType.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblReportType.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblReportType.Location = new System.Drawing.Point(138, 9);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.Size = new System.Drawing.Size(65, 12);
            this.lblReportType.TabIndex = 14;
            this.lblReportType.Text = "报表类型：";
            this.lblReportType.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnTime
            // 
            this.btnTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTime.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTime.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnTime.Location = new System.Drawing.Point(402, 29);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(75, 23);
            this.btnTime.TabIndex = 16;
            this.btnTime.Text = "时间选择";
            this.btnTime.UseVisualStyleBackColor = true;
            // 
            // edtTime
            // 
            this.edtTime.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.edtTime.DrawSelf = false;
            this.edtTime.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.edtTime.EnterToTab = false;
            this.edtTime.Location = new System.Drawing.Point(500, 34);
            this.edtTime.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.edtTime.Name = "edtTime";
            this.edtTime.Padding = new System.Windows.Forms.Padding(1);
            this.edtTime.ReadOnly = true;
            this.edtTime.Size = new System.Drawing.Size(83, 16);
            this.edtTime.TabIndex = 17;
            // 
            // VViewFreeQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 554);
            this.Name = "VViewFreeQuery";
            this.Text = "通用报表查询界面";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid dgvWrite;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnPreview;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboView;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblView;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboYwzz;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblYwzz;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnFs;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboReportType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblReportType;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnTime;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit edtTime;
    }
}