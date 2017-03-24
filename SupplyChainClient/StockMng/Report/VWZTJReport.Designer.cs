namespace Application.Business.Erp.SupplyChain.Client.StockMng.Report
{
    partial class VWZTJReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VWZTJReport));
            this.cmbProject = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel24 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabCost = new System.Windows.Forms.TabControl();
            this.tabFourCal = new System.Windows.Forms.TabPage();
            this.fGridSupply = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tabPerson = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fGridMaterial = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.lblTo = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblDate = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtMaterialCategory = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial();
            this.lblCat = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.tabCost.SuspendLayout();
            this.tabFourCal.SuspendLayout();
            this.tabPerson.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterialCategory)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.txtMaterialCategory);
            this.pnlFloor.Controls.Add(this.lblCat);
            this.pnlFloor.Controls.Add(this.dtpDateBegin);
            this.pnlFloor.Controls.Add(this.dtpDateEnd);
            this.pnlFloor.Controls.Add(this.lblTo);
            this.pnlFloor.Controls.Add(this.lblDate);
            this.pnlFloor.Controls.Add(this.btnExcel);
            this.pnlFloor.Controls.Add(this.tabCost);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Controls.Add(this.customLabel24);
            this.pnlFloor.Controls.Add(this.cmbProject);
            this.pnlFloor.Size = new System.Drawing.Size(1011, 508);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cmbProject, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel24, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.tabCost, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExcel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblDate, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTo, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtpDateEnd, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtpDateBegin, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblCat, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtMaterialCategory, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 9);
            // 
            // cmbProject
            // 
            this.cmbProject.BackColor = System.Drawing.SystemColors.Control;
            this.cmbProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProject.FormattingEnabled = true;
            this.cmbProject.Location = new System.Drawing.Point(110, 11);
            this.cmbProject.Name = "cmbProject";
            this.cmbProject.Size = new System.Drawing.Size(129, 20);
            this.cmbProject.TabIndex = 4;
            // 
            // customLabel24
            // 
            this.customLabel24.AutoSize = true;
            this.customLabel24.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel24.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel24.Location = new System.Drawing.Point(45, 16);
            this.customLabel24.Name = "customLabel24";
            this.customLabel24.Size = new System.Drawing.Size(59, 12);
            this.customLabel24.TabIndex = 115;
            this.customLabel24.Text = "项目名称:";
            this.customLabel24.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabCost
            // 
            this.tabCost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCost.Controls.Add(this.tabFourCal);
            this.tabCost.Controls.Add(this.tabPerson);
            this.tabCost.Location = new System.Drawing.Point(12, 37);
            this.tabCost.Name = "tabCost";
            this.tabCost.SelectedIndex = 0;
            this.tabCost.Size = new System.Drawing.Size(996, 468);
            this.tabCost.TabIndex = 149;
            // 
            // tabFourCal
            // 
            this.tabFourCal.Controls.Add(this.fGridSupply);
            this.tabFourCal.Location = new System.Drawing.Point(4, 21);
            this.tabFourCal.Name = "tabFourCal";
            this.tabFourCal.Padding = new System.Windows.Forms.Padding(3);
            this.tabFourCal.Size = new System.Drawing.Size(988, 443);
            this.tabFourCal.TabIndex = 0;
            this.tabFourCal.Text = "采购成本对比分析表";
            this.tabFourCal.UseVisualStyleBackColor = true;
            // 
            // fGridSupply
            // 
            this.fGridSupply.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGridSupply.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGridSupply.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridSupply.CheckedImage")));
            this.fGridSupply.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.fGridSupply.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGridSupply.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGridSupply.Location = new System.Drawing.Point(4, 3);
            this.fGridSupply.Name = "fGridSupply";
            this.fGridSupply.Size = new System.Drawing.Size(978, 434);
            this.fGridSupply.TabIndex = 5;
            this.fGridSupply.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridSupply.UncheckedImage")));
            // 
            // tabPerson
            // 
            this.tabPerson.Controls.Add(this.panel1);
            this.tabPerson.Location = new System.Drawing.Point(4, 21);
            this.tabPerson.Name = "tabPerson";
            this.tabPerson.Padding = new System.Windows.Forms.Padding(3);
            this.tabPerson.Size = new System.Drawing.Size(366, 218);
            this.tabPerson.TabIndex = 1;
            this.tabPerson.Text = "物资消耗对比分析表";
            this.tabPerson.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fGridMaterial);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 212);
            this.panel1.TabIndex = 7;
            // 
            // fGridMaterial
            // 
            this.fGridMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGridMaterial.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGridMaterial.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridMaterial.CheckedImage")));
            this.fGridMaterial.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.fGridMaterial.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGridMaterial.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGridMaterial.Location = new System.Drawing.Point(3, 3);
            this.fGridMaterial.Name = "fGridMaterial";
            this.fGridMaterial.Size = new System.Drawing.Size(354, 206);
            this.fGridMaterial.TabIndex = 7;
            this.fGridMaterial.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridMaterial.UncheckedImage")));
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(866, 8);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 14;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(783, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 28);
            this.btnQuery.TabIndex = 13;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(331, 9);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 151;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(463, 9);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 152;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTo.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblTo.Location = new System.Drawing.Point(446, 16);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(11, 12);
            this.lblTo.TabIndex = 153;
            this.lblTo.Text = "-";
            this.lblTo.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDate.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblDate.Location = new System.Drawing.Point(267, 14);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(59, 12);
            this.lblDate.TabIndex = 150;
            this.lblDate.Text = "统计日期:";
            this.lblDate.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtMaterialCategory
            // 
            this.txtMaterialCategory.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterialCategory.Code = null;
            this.txtMaterialCategory.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtMaterialCategory.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtMaterialCategory.EnterToTab = false;
            this.txtMaterialCategory.Id = "";
            this.txtMaterialCategory.IsAllLoad = true;
            this.txtMaterialCategory.IsCheckBox = false;
            this.txtMaterialCategory.Location = new System.Drawing.Point(664, 10);
            this.txtMaterialCategory.Name = "txtMaterialCategory";
            this.txtMaterialCategory.ObjectType = Application.Business.Erp.ResourceManager.Client.Basic.Controls.MaterialSelectType.MaterialCatView;
            this.txtMaterialCategory.Result = ((System.Collections.IList)(resources.GetObject("txtMaterialCategory.Result")));
            this.txtMaterialCategory.RightMouse = false;
            this.txtMaterialCategory.Size = new System.Drawing.Size(96, 21);
            this.txtMaterialCategory.TabIndex = 155;
            this.txtMaterialCategory.Tag = null;
            this.txtMaterialCategory.Value = "";
            this.txtMaterialCategory.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // lblCat
            // 
            this.lblCat.AutoSize = true;
            this.lblCat.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCat.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCat.Location = new System.Drawing.Point(601, 14);
            this.lblCat.Name = "lblCat";
            this.lblCat.Size = new System.Drawing.Size(59, 12);
            this.lblCat.TabIndex = 154;
            this.lblCat.Text = "物资分类:";
            this.lblCat.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VWZTJReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 508);
            this.Name = "VWZTJReport";
            this.Text = "成本对比分析表";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.tabCost.ResumeLayout(false);
            this.tabFourCal.ResumeLayout(false);
            this.tabPerson.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMaterialCategory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cmbProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel24;
        private System.Windows.Forms.TabControl tabCost;
        private System.Windows.Forms.TabPage tabFourCal;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGridSupply;
        private System.Windows.Forms.TabPage tabPerson;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private System.Windows.Forms.Panel panel1;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGridMaterial;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblTo;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblDate;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonMaterial txtMaterialCategory;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCat;
    }
}