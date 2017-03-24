namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    partial class VSubcontractSettlementReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSubcontractSettlementReport));
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.tabDetail = new System.Windows.Forms.TabPage();
            this.fGridDetail = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tabCost = new System.Windows.Forms.TabControl();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblPSelect = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.tabDetail.SuspendLayout();
            this.tabCost.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnOperationOrg);
            this.pnlFloor.Controls.Add(this.txtOperationOrg);
            this.pnlFloor.Controls.Add(this.lblPSelect);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Controls.Add(this.cbType);
            this.pnlFloor.Controls.Add(this.cbYear);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.customLabel7);
            this.pnlFloor.Controls.Add(this.btnExcel);
            this.pnlFloor.Controls.Add(this.tabCost);
            this.pnlFloor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlFloor.Size = new System.Drawing.Size(1011, 508);
            this.pnlFloor.Controls.SetChildIndex(this.tabCost, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExcel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel7, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cbYear, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cbType, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblPSelect, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOperationOrg, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(57, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(842, 10);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 14;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // tabDetail
            // 
            this.tabDetail.Controls.Add(this.fGridDetail);
            this.tabDetail.Location = new System.Drawing.Point(4, 22);
            this.tabDetail.Name = "tabDetail";
            this.tabDetail.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabDetail.Size = new System.Drawing.Size(988, 442);
            this.tabDetail.TabIndex = 0;
            this.tabDetail.Text = "明细";
            this.tabDetail.UseVisualStyleBackColor = true;
            // 
            // fGridDetail
            // 
            this.fGridDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGridDetail.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail.CheckedImage")));
            this.fGridDetail.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.fGridDetail.DefaultRowHeight = ((short)(24));
            this.fGridDetail.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGridDetail.Location = new System.Drawing.Point(4, 3);
            this.fGridDetail.Name = "fGridDetail";
            this.fGridDetail.Size = new System.Drawing.Size(978, 433);
            this.fGridDetail.TabIndex = 5;
            this.fGridDetail.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail.UncheckedImage")));
            // 
            // tabCost
            // 
            this.tabCost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCost.Controls.Add(this.tabDetail);
            this.tabCost.Location = new System.Drawing.Point(12, 37);
            this.tabCost.Name = "tabCost";
            this.tabCost.SelectedIndex = 0;
            this.tabCost.Size = new System.Drawing.Size(996, 468);
            this.tabCost.TabIndex = 149;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(236, 18);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 173;
            this.customLabel7.Text = "分包类型:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 174;
            this.label1.Text = "统计年份:";
            // 
            // cbYear
            // 
            this.cbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Location = new System.Drawing.Point(83, 14);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(121, 20);
            this.cbYear.TabIndex = 175;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "劳务分包",
            "专业分包"});
            this.cbType.Location = new System.Drawing.Point(301, 14);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(121, 20);
            this.cbType.TabIndex = 176;
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(749, 10);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 28);
            this.btnQuery.TabIndex = 177;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(672, 13);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 247;
            this.btnOperationOrg.Text = "选择";
            this.btnOperationOrg.UseVisualStyleBackColor = true;
            // 
            // txtOperationOrg
            // 
            this.txtOperationOrg.BackColor = System.Drawing.SystemColors.Control;
            this.txtOperationOrg.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOperationOrg.DrawSelf = false;
            this.txtOperationOrg.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOperationOrg.EnterToTab = false;
            this.txtOperationOrg.Location = new System.Drawing.Point(502, 16);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(175, 16);
            this.txtOperationOrg.TabIndex = 246;
            // 
            // lblPSelect
            // 
            this.lblPSelect.AutoSize = true;
            this.lblPSelect.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPSelect.Location = new System.Drawing.Point(443, 18);
            this.lblPSelect.Name = "lblPSelect";
            this.lblPSelect.Size = new System.Drawing.Size(59, 12);
            this.lblPSelect.TabIndex = 245;
            this.lblPSelect.Text = "范围选择:";
            this.lblPSelect.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VSubcontractSettlementReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 508);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "VSubcontractSettlementReport";
            this.Text = "分包结算台帐";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.tabDetail.ResumeLayout(false);
            this.tabCost.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private System.Windows.Forms.TabControl tabCost;
        private System.Windows.Forms.TabPage tabDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGridDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private System.Windows.Forms.ComboBox cbYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbType;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPSelect;
    }
}