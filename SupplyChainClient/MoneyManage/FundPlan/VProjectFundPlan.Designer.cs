namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    partial class VProjectFundPlan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VProjectFundPlan));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.fGridDetail = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnCreate = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.comMonth = new System.Windows.Forms.ComboBox();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.comYear = new System.Windows.Forms.ComboBox();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDate = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSubmit1 = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSave1 = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.fGridDetail1 = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnCreate1 = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnQuery1 = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.comMonth1 = new System.Windows.Forms.ComboBox();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.comYear1 = new System.Windows.Forms.ComboBox();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDate1 = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSave = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSubmit = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.tabControl);
            this.pnlFloor.Size = new System.Drawing.Size(849, 541);
            this.pnlFloor.Controls.SetChildIndex(this.tabControl, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Location = new System.Drawing.Point(3, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(846, 538);
            this.tabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(838, 512);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "公司直管项目汇总";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSubmit);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.fGridDetail);
            this.panel2.Controls.Add(this.btnCreate);
            this.panel2.Controls.Add(this.btnQuery);
            this.panel2.Controls.Add(this.comMonth);
            this.panel2.Controls.Add(this.customLabel1);
            this.panel2.Controls.Add(this.comYear);
            this.panel2.Controls.Add(this.customLabel2);
            this.panel2.Controls.Add(this.dtpDate);
            this.panel2.Controls.Add(this.customLabel6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(832, 506);
            this.panel2.TabIndex = 1;
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
            this.fGridDetail.Location = new System.Drawing.Point(7, 50);
            this.fGridDetail.Name = "fGridDetail";
            this.fGridDetail.Size = new System.Drawing.Size(824, 409);
            this.fGridDetail.TabIndex = 233;
            this.fGridDetail.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail.UncheckedImage")));
            // 
            // btnCreate
            // 
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCreate.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCreate.Location = new System.Drawing.Point(651, 10);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 28);
            this.btnCreate.TabIndex = 232;
            this.btnCreate.Text = "生 成";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(559, 10);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 28);
            this.btnQuery.TabIndex = 231;
            this.btnQuery.Text = "查 询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // comMonth
            // 
            this.comMonth.FormattingEnabled = true;
            this.comMonth.Location = new System.Drawing.Point(400, 15);
            this.comMonth.Name = "comMonth";
            this.comMonth.Size = new System.Drawing.Size(141, 20);
            this.comMonth.TabIndex = 230;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(366, 20);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(41, 12);
            this.customLabel1.TabIndex = 229;
            this.customLabel1.Text = "月份：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // comYear
            // 
            this.comYear.FormattingEnabled = true;
            this.comYear.Location = new System.Drawing.Point(219, 15);
            this.comYear.Name = "comYear";
            this.comYear.Size = new System.Drawing.Size(141, 20);
            this.comYear.TabIndex = 228;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(180, 20);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(41, 12);
            this.customLabel2.TabIndex = 223;
            this.customLabel2.Text = "年份：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(70, 15);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(109, 21);
            this.dtpDate.TabIndex = 155;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(6, 20);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(59, 12);
            this.customLabel6.TabIndex = 154;
            this.customLabel6.Text = "申报日期:";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(838, 512);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "项目资金使用计划表";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSubmit1);
            this.panel1.Controls.Add(this.btnSave1);
            this.panel1.Controls.Add(this.fGridDetail1);
            this.panel1.Controls.Add(this.btnCreate1);
            this.panel1.Controls.Add(this.btnQuery1);
            this.panel1.Controls.Add(this.comMonth1);
            this.panel1.Controls.Add(this.customLabel5);
            this.panel1.Controls.Add(this.comYear1);
            this.panel1.Controls.Add(this.customLabel4);
            this.panel1.Controls.Add(this.dtpDate1);
            this.panel1.Controls.Add(this.customLabel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(832, 506);
            this.panel1.TabIndex = 0;
            // 
            // btnSubmit1
            // 
            this.btnSubmit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSubmit1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSubmit1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSubmit1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSubmit1.Location = new System.Drawing.Point(455, 475);
            this.btnSubmit1.Name = "btnSubmit1";
            this.btnSubmit1.Size = new System.Drawing.Size(75, 28);
            this.btnSubmit1.TabIndex = 235;
            this.btnSubmit1.Text = "提 交";
            this.btnSubmit1.UseVisualStyleBackColor = true;
            // 
            // btnSave1
            // 
            this.btnSave1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSave1.Location = new System.Drawing.Point(332, 475);
            this.btnSave1.Name = "btnSave1";
            this.btnSave1.Size = new System.Drawing.Size(75, 28);
            this.btnSave1.TabIndex = 234;
            this.btnSave1.Text = "保 存";
            this.btnSave1.UseVisualStyleBackColor = true;
            // 
            // fGridDetail1
            // 
            this.fGridDetail1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGridDetail1.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGridDetail1.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail1.CheckedImage")));
            this.fGridDetail1.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.fGridDetail1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGridDetail1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGridDetail1.Location = new System.Drawing.Point(7, 50);
            this.fGridDetail1.Name = "fGridDetail1";
            this.fGridDetail1.Size = new System.Drawing.Size(824, 409);
            this.fGridDetail1.TabIndex = 233;
            this.fGridDetail1.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail1.UncheckedImage")));
            // 
            // btnCreate1
            // 
            this.btnCreate1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreate1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCreate1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCreate1.Location = new System.Drawing.Point(651, 10);
            this.btnCreate1.Name = "btnCreate1";
            this.btnCreate1.Size = new System.Drawing.Size(75, 28);
            this.btnCreate1.TabIndex = 232;
            this.btnCreate1.Text = "生 成";
            this.btnCreate1.UseVisualStyleBackColor = true;
            // 
            // btnQuery1
            // 
            this.btnQuery1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery1.Location = new System.Drawing.Point(559, 10);
            this.btnQuery1.Name = "btnQuery1";
            this.btnQuery1.Size = new System.Drawing.Size(75, 28);
            this.btnQuery1.TabIndex = 231;
            this.btnQuery1.Text = "查 询";
            this.btnQuery1.UseVisualStyleBackColor = true;
            // 
            // comMonth1
            // 
            this.comMonth1.FormattingEnabled = true;
            this.comMonth1.Location = new System.Drawing.Point(400, 15);
            this.comMonth1.Name = "comMonth1";
            this.comMonth1.Size = new System.Drawing.Size(141, 20);
            this.comMonth1.TabIndex = 230;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(366, 20);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(41, 12);
            this.customLabel5.TabIndex = 229;
            this.customLabel5.Text = "月份：";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // comYear1
            // 
            this.comYear1.FormattingEnabled = true;
            this.comYear1.Location = new System.Drawing.Point(219, 15);
            this.comYear1.Name = "comYear1";
            this.comYear1.Size = new System.Drawing.Size(141, 20);
            this.comYear1.TabIndex = 228;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(180, 20);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(41, 12);
            this.customLabel4.TabIndex = 223;
            this.customLabel4.Text = "年份：";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDate1
            // 
            this.dtpDate1.Location = new System.Drawing.Point(70, 15);
            this.dtpDate1.Name = "dtpDate1";
            this.dtpDate1.Size = new System.Drawing.Size(109, 21);
            this.dtpDate1.TabIndex = 155;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(6, 20);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 154;
            this.customLabel3.Text = "申报日期:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSave.Location = new System.Drawing.Point(332, 475);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 235;
            this.btnSave.Text = "保 存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSubmit.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSubmit.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSubmit.Location = new System.Drawing.Point(455, 475);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 28);
            this.btnSubmit.TabIndex = 236;
            this.btnSubmit.Text = "提 交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // VProjectFundPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 541);
            this.Name = "VProjectFundPlan";
            this.Text = "VProjectFundPlan";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDate1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private System.Windows.Forms.ComboBox comYear1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private System.Windows.Forms.ComboBox comMonth1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCreate1;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGridDetail1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSave1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSubmit1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel2;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGridDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCreate;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private System.Windows.Forms.ComboBox comMonth;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.ComboBox comYear;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSave;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSubmit;
    }
}