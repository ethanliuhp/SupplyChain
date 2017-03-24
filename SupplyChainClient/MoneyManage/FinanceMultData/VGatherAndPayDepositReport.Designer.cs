namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VGatherAndPayDepositReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VGatherAndPayDepositReport));
            this.reportGrid = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.txtOrgName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.btnSelRel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.lblCat = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbGatheringType = new System.Windows.Forms.ComboBox();
            this.lblGathingType = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblPSelect = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnExcel);
            this.pnlFloor.Controls.Add(this.btnSearch);
            this.pnlFloor.Controls.Add(this.cmbType);
            this.pnlFloor.Controls.Add(this.customLabel2);
            this.pnlFloor.Controls.Add(this.btnOperationOrg);
            this.pnlFloor.Controls.Add(this.txtOperationOrg);
            this.pnlFloor.Controls.Add(this.lblPSelect);
            this.pnlFloor.Controls.Add(this.cmbGatheringType);
            this.pnlFloor.Controls.Add(this.lblGathingType);
            this.pnlFloor.Controls.Add(this.txtOrgName);
            this.pnlFloor.Controls.Add(this.btnSelRel);
            this.pnlFloor.Controls.Add(this.lblCat);
            this.pnlFloor.Controls.Add(this.dtpDateEnd);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Controls.Add(this.reportGrid);
            this.pnlFloor.Size = new System.Drawing.Size(974, 548);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.reportGrid, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtpDateEnd, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblCat, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSelRel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtOrgName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblGathingType, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cmbGatheringType, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblPSelect, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cmbType, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSearch, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExcel, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 62);
            // 
            // reportGrid
            // 
            this.reportGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.reportGrid.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.reportGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("reportGrid.CheckedImage")));
            this.reportGrid.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.reportGrid.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.reportGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.reportGrid.Location = new System.Drawing.Point(3, 73);
            this.reportGrid.Name = "reportGrid";
            this.reportGrid.Size = new System.Drawing.Size(968, 472);
            this.reportGrid.TabIndex = 8;
            this.reportGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("reportGrid.UncheckedImage")));
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(12, 18);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 184;
            this.customLabel1.Text = "结束日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(75, 14);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(185, 21);
            this.dtpDateEnd.TabIndex = 185;
            // 
            // txtOrgName
            // 
            this.txtOrgName.BackColor = System.Drawing.SystemColors.Control;
            this.txtOrgName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOrgName.DrawSelf = false;
            this.txtOrgName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOrgName.EnterToTab = false;
            this.txtOrgName.Location = new System.Drawing.Point(75, 45);
            this.txtOrgName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOrgName.Name = "txtOrgName";
            this.txtOrgName.Padding = new System.Windows.Forms.Padding(1);
            this.txtOrgName.ReadOnly = false;
            this.txtOrgName.Size = new System.Drawing.Size(185, 16);
            this.txtOrgName.TabIndex = 188;
            // 
            // btnSelRel
            // 
            this.btnSelRel.Enabled = false;
            this.btnSelRel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelRel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelRel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSelRel.Location = new System.Drawing.Point(266, 42);
            this.btnSelRel.Name = "btnSelRel";
            this.btnSelRel.Size = new System.Drawing.Size(42, 23);
            this.btnSelRel.TabIndex = 186;
            this.btnSelRel.Text = "选择";
            this.btnSelRel.UseVisualStyleBackColor = true;
            // 
            // lblCat
            // 
            this.lblCat.AutoSize = true;
            this.lblCat.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCat.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCat.Location = new System.Drawing.Point(36, 47);
            this.lblCat.Name = "lblCat";
            this.lblCat.Size = new System.Drawing.Size(35, 12);
            this.lblCat.TabIndex = 187;
            this.lblCat.Text = "单位:";
            this.lblCat.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbGatheringType
            // 
            this.cmbGatheringType.DropDownHeight = 300;
            this.cmbGatheringType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGatheringType.FormattingEnabled = true;
            this.cmbGatheringType.IntegralHeight = false;
            this.cmbGatheringType.Location = new System.Drawing.Point(696, 14);
            this.cmbGatheringType.Name = "cmbGatheringType";
            this.cmbGatheringType.Size = new System.Drawing.Size(126, 20);
            this.cmbGatheringType.TabIndex = 190;
            // 
            // lblGathingType
            // 
            this.lblGathingType.AutoSize = true;
            this.lblGathingType.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGathingType.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblGathingType.Location = new System.Drawing.Point(638, 18);
            this.lblGathingType.Name = "lblGathingType";
            this.lblGathingType.Size = new System.Drawing.Size(59, 12);
            this.lblGathingType.TabIndex = 189;
            this.lblGathingType.Text = "款项类别:";
            this.lblGathingType.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(510, 13);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 193;
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
            this.txtOperationOrg.Location = new System.Drawing.Point(385, 16);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(124, 16);
            this.txtOperationOrg.TabIndex = 192;
            // 
            // lblPSelect
            // 
            this.lblPSelect.AutoSize = true;
            this.lblPSelect.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPSelect.Location = new System.Drawing.Point(324, 18);
            this.lblPSelect.Name = "lblPSelect";
            this.lblPSelect.Size = new System.Drawing.Size(59, 12);
            this.lblPSelect.TabIndex = 191;
            this.lblPSelect.Text = "范围选择:";
            this.lblPSelect.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbType
            // 
            this.cmbType.DropDownHeight = 120;
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.IntegralHeight = false;
            this.cmbType.Items.AddRange(new object[] {
            "",
            "客户",
            "分供商"});
            this.cmbType.Location = new System.Drawing.Point(385, 43);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(126, 20);
            this.cmbType.TabIndex = 195;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(347, 47);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(35, 12);
            this.customLabel2.TabIndex = 194;
            this.customLabel2.Text = "类型:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(747, 39);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 197;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(663, 39);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 28);
            this.btnSearch.TabIndex = 196;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // VGatherAndPayDepositReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 548);
            this.Name = "VGatherAndPayDepositReport";
            this.Text = "保证金/押金台账";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid reportGrid;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOrgName;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSelRel;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCat;
        private System.Windows.Forms.ComboBox cmbGatheringType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblGathingType;
        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPSelect;
        private System.Windows.Forms.ComboBox cmbType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
    }
}