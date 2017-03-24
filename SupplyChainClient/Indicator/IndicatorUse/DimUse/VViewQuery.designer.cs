namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    partial class VViewQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VViewQuery));
            this.gbxViewSelect = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.lstViewSel = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.gbxDataWrite = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.lblTime = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnPreview = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dgvCubeData = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.edtSerial = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblSerial = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.edtDistributeDate = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblDistributeDate = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cboTime = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.pnlFloor.SuspendLayout();
            this.gbxViewSelect.SuspendLayout();
            this.gbxDataWrite.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.gbxDataWrite);
            this.pnlFloor.Controls.Add(this.gbxViewSelect);
            this.pnlFloor.Size = new System.Drawing.Size(992, 554);
            // 
            // gbxViewSelect
            // 
            this.gbxViewSelect.Controls.Add(this.lstViewSel);
            this.gbxViewSelect.Location = new System.Drawing.Point(12, 21);
            this.gbxViewSelect.Name = "gbxViewSelect";
            this.gbxViewSelect.Size = new System.Drawing.Size(191, 513);
            this.gbxViewSelect.TabIndex = 0;
            this.gbxViewSelect.TabStop = false;
            this.gbxViewSelect.Text = ">>>查询模板选择";
            // 
            // lstViewSel
            // 
            this.lstViewSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstViewSel.FormattingEnabled = true;
            this.lstViewSel.ItemHeight = 12;
            this.lstViewSel.Location = new System.Drawing.Point(21, 23);
            this.lstViewSel.Name = "lstViewSel";
            this.lstViewSel.Size = new System.Drawing.Size(150, 470);
            this.lstViewSel.TabIndex = 0;
            this.lstViewSel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstViewSel_MouseClick);
            // 
            // gbxDataWrite
            // 
            this.gbxDataWrite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gbxDataWrite.Controls.Add(this.cboTime);
            this.gbxDataWrite.Controls.Add(this.btnQuery);
            this.gbxDataWrite.Controls.Add(this.lblTime);
            this.gbxDataWrite.Controls.Add(this.btnExcel);
            this.gbxDataWrite.Controls.Add(this.btnPreview);
            this.gbxDataWrite.Controls.Add(this.dgvCubeData);
            this.gbxDataWrite.Controls.Add(this.edtSerial);
            this.gbxDataWrite.Controls.Add(this.lblSerial);
            this.gbxDataWrite.Controls.Add(this.edtDistributeDate);
            this.gbxDataWrite.Controls.Add(this.lblDistributeDate);
            this.gbxDataWrite.Location = new System.Drawing.Point(209, 21);
            this.gbxDataWrite.Name = "gbxDataWrite";
            this.gbxDataWrite.Size = new System.Drawing.Size(780, 513);
            this.gbxDataWrite.TabIndex = 1;
            this.gbxDataWrite.TabStop = false;
            this.gbxDataWrite.Text = ">>>查询数据列表";
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(637, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 12;
            this.btnQuery.Text = "查  询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTime.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblTime.Location = new System.Drawing.Point(446, 17);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(40, 13);
            this.lblTime.TabIndex = 10;
            this.lblTime.Text = "时间:";
            this.lblTime.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnExcel
            // 
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(362, 470);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 9;
            this.btnExcel.Text = "导出Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPreview.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreview.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnPreview.Location = new System.Drawing.Point(204, 470);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 8;
            this.btnPreview.Text = "预 览 ";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // dgvCubeData
            // 
            this.dgvCubeData.AutoSize = true;
            this.dgvCubeData.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("dgvCubeData.CheckedImage")));
            this.dgvCubeData.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.dgvCubeData.Font = new System.Drawing.Font("SimSun", 9F);
            this.dgvCubeData.Location = new System.Drawing.Point(6, 33);
            this.dgvCubeData.MultiSelect = false;
            this.dgvCubeData.Name = "dgvCubeData";
            this.dgvCubeData.Rows = 1;
            this.dgvCubeData.Size = new System.Drawing.Size(774, 431);
            this.dgvCubeData.TabIndex = 7;
            this.dgvCubeData.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("dgvCubeData.UncheckedImage")));
            // 
            // edtSerial
            // 
            this.edtSerial.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.edtSerial.DrawSelf = false;
            this.edtSerial.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.edtSerial.EnterToTab = false;
            this.edtSerial.Location = new System.Drawing.Point(261, 11);
            this.edtSerial.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.edtSerial.Name = "edtSerial";
            this.edtSerial.Padding = new System.Windows.Forms.Padding(1);
            this.edtSerial.ReadOnly = true;
            this.edtSerial.Size = new System.Drawing.Size(99, 16);
            this.edtSerial.TabIndex = 3;
            // 
            // lblSerial
            // 
            this.lblSerial.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSerial.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSerial.Location = new System.Drawing.Point(206, 17);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(49, 13);
            this.lblSerial.TabIndex = 2;
            this.lblSerial.Text = "分发号:";
            this.lblSerial.UnderLineColor = System.Drawing.Color.Red;
            // 
            // edtDistributeDate
            // 
            this.edtDistributeDate.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.edtDistributeDate.DrawSelf = false;
            this.edtDistributeDate.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.edtDistributeDate.EnterToTab = false;
            this.edtDistributeDate.Location = new System.Drawing.Point(93, 11);
            this.edtDistributeDate.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.edtDistributeDate.Name = "edtDistributeDate";
            this.edtDistributeDate.Padding = new System.Windows.Forms.Padding(1);
            this.edtDistributeDate.ReadOnly = true;
            this.edtDistributeDate.Size = new System.Drawing.Size(85, 16);
            this.edtDistributeDate.TabIndex = 1;
            // 
            // lblDistributeDate
            // 
            this.lblDistributeDate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDistributeDate.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblDistributeDate.Location = new System.Drawing.Point(22, 17);
            this.lblDistributeDate.Name = "lblDistributeDate";
            this.lblDistributeDate.Size = new System.Drawing.Size(65, 13);
            this.lblDistributeDate.TabIndex = 0;
            this.lblDistributeDate.Text = "分发日期:";
            this.lblDistributeDate.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cboTime
            // 
            this.cboTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTime.FormattingEnabled = true;
            this.cboTime.Location = new System.Drawing.Point(492, 11);
            this.cboTime.Name = "cboTime";
            this.cboTime.Size = new System.Drawing.Size(121, 20);
            this.cboTime.TabIndex = 13;
            // 
            // VViewQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 554);
            this.Name = "VViewQuery";
            this.Text = "数据查询界面";
            this.pnlFloor.ResumeLayout(false);
            this.gbxViewSelect.ResumeLayout(false);
            this.gbxDataWrite.ResumeLayout(false);
            this.gbxDataWrite.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gbxViewSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gbxDataWrite;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit edtDistributeDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblDistributeDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSerial;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit edtSerial;
        private Application.Business.Erp.ClientSystem.Template.CustomListBox lstViewSel;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid dgvCubeData;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnPreview;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblTime;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboTime;
    }
}