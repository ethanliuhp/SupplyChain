namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    partial class VCommercialReportQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCommercialReportQuery));
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.comYear = new System.Windows.Forms.ComboBox();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.comMonth = new System.Windows.Forms.ComboBox();
            this.lblPSelect = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.selectType = new System.Windows.Forms.ComboBox();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.fGridDetail = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnExl = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnExl);
            this.pnlFloor.Controls.Add(this.fGridDetail);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Controls.Add(this.selectType);
            this.pnlFloor.Controls.Add(this.customLabel7);
            this.pnlFloor.Controls.Add(this.btnOperationOrg);
            this.pnlFloor.Controls.Add(this.txtOperationOrg);
            this.pnlFloor.Controls.Add(this.lblPSelect);
            this.pnlFloor.Controls.Add(this.comMonth);
            this.pnlFloor.Controls.Add(this.customLabel2);
            this.pnlFloor.Controls.Add(this.comYear);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Size = new System.Drawing.Size(1026, 480);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.comYear, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.comMonth, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblPSelect, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel7, 0);
            this.pnlFloor.Controls.SetChildIndex(this.selectType, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.fGridDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExl, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(12, 20);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(41, 12);
            this.customLabel1.TabIndex = 222;
            this.customLabel1.Text = "年份：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // comYear
            // 
            this.comYear.FormattingEnabled = true;
            this.comYear.Location = new System.Drawing.Point(45, 15);
            this.comYear.Name = "comYear";
            this.comYear.Size = new System.Drawing.Size(141, 20);
            this.comYear.TabIndex = 227;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(192, 20);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(41, 12);
            this.customLabel2.TabIndex = 228;
            this.customLabel2.Text = "月份：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // comMonth
            // 
            this.comMonth.FormattingEnabled = true;
            this.comMonth.Location = new System.Drawing.Point(226, 15);
            this.comMonth.Name = "comMonth";
            this.comMonth.Size = new System.Drawing.Size(141, 20);
            this.comMonth.TabIndex = 229;
            // 
            // lblPSelect
            // 
            this.lblPSelect.AutoSize = true;
            this.lblPSelect.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPSelect.Location = new System.Drawing.Point(373, 20);
            this.lblPSelect.Name = "lblPSelect";
            this.lblPSelect.Size = new System.Drawing.Size(59, 12);
            this.lblPSelect.TabIndex = 230;
            this.lblPSelect.Text = "范围选择:";
            this.lblPSelect.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtOperationOrg
            // 
            this.txtOperationOrg.BackColor = System.Drawing.SystemColors.Control;
            this.txtOperationOrg.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOperationOrg.DrawSelf = false;
            this.txtOperationOrg.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOperationOrg.EnterToTab = false;
            this.txtOperationOrg.Location = new System.Drawing.Point(438, 16);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(124, 16);
            this.txtOperationOrg.TabIndex = 231;
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(568, 13);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 232;
            this.btnOperationOrg.Text = "选择";
            this.btnOperationOrg.UseVisualStyleBackColor = true;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(632, 20);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(41, 12);
            this.customLabel7.TabIndex = 221;
            this.customLabel7.Text = "分类：";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // selectType
            // 
            this.selectType.FormattingEnabled = true;
            this.selectType.Location = new System.Drawing.Point(679, 15);
            this.selectType.Name = "selectType";
            this.selectType.Size = new System.Drawing.Size(141, 20);
            this.selectType.TabIndex = 233;
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(826, 10);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 28);
            this.btnQuery.TabIndex = 234;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // fGridDetail
            // 
            this.fGridDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGridDetail.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail.CheckedImage")));
            this.fGridDetail.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.fGridDetail.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGridDetail.Location = new System.Drawing.Point(-1, 60);
            this.fGridDetail.Name = "fGridDetail";
            this.fGridDetail.Size = new System.Drawing.Size(1027, 420);
            this.fGridDetail.TabIndex = 235;
            this.fGridDetail.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail.UncheckedImage")));
            // 
            // btnExl
            // 
            this.btnExl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExl.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExl.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExl.Location = new System.Drawing.Point(907, 10);
            this.btnExl.Name = "btnExl";
            this.btnExl.Size = new System.Drawing.Size(75, 28);
            this.btnExl.TabIndex = 236;
            this.btnExl.Text = "Excel";
            this.btnExl.UseVisualStyleBackColor = true;
            // 
            // VCommercialReportQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 480);
            this.Name = "VCommercialReportQuery";
            this.Text = "VCommercialReportQuery";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.ComboBox comYear;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.ComboBox comMonth;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private System.Windows.Forms.ComboBox selectType;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGridDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExl;
    }
}