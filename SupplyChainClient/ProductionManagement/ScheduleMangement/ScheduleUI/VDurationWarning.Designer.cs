namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI
{
    partial class VDurationWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDurationWarning));
            this.flexGridTotal = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblPSelect = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageTotal = new System.Windows.Forms.TabPage();
            this.tabPageDetail = new System.Windows.Forms.TabPage();
            this.flexGridDetail = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tabPageChart = new System.Windows.Forms.TabPage();
            this.gdChart = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.btnSave = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.pnlFloor.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageTotal.SuspendLayout();
            this.tabPageDetail.SuspendLayout();
            this.tabPageChart.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dtpDate);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.btnSave);
            this.pnlFloor.Controls.Add(this.tabControl1);
            this.pnlFloor.Controls.Add(this.btnSearch);
            this.pnlFloor.Controls.Add(this.btnOperationOrg);
            this.pnlFloor.Controls.Add(this.txtOperationOrg);
            this.pnlFloor.Controls.Add(this.lblPSelect);
            this.pnlFloor.Size = new System.Drawing.Size(968, 603);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblPSelect, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOperationOrg, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSearch, 0);
            this.pnlFloor.Controls.SetChildIndex(this.tabControl1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSave, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dtpDate, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // flexGridTotal
            // 
            this.flexGridTotal.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGridTotal.CheckedImage")));
            this.flexGridTotal.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.flexGridTotal.DefaultRowHeight = ((short)(21));
            this.flexGridTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flexGridTotal.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGridTotal.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGridTotal.Location = new System.Drawing.Point(3, 3);
            this.flexGridTotal.Name = "flexGridTotal";
            this.flexGridTotal.Size = new System.Drawing.Size(948, 525);
            this.flexGridTotal.TabIndex = 159;
            this.flexGridTotal.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGridTotal.UncheckedImage")));
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(338, 12);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 196;
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
            this.txtOperationOrg.Location = new System.Drawing.Point(71, 15);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(261, 16);
            this.txtOperationOrg.TabIndex = 195;
            // 
            // lblPSelect
            // 
            this.lblPSelect.AutoSize = true;
            this.lblPSelect.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPSelect.Location = new System.Drawing.Point(10, 17);
            this.lblPSelect.Name = "lblPSelect";
            this.lblPSelect.Size = new System.Drawing.Size(65, 12);
            this.lblPSelect.TabIndex = 194;
            this.lblPSelect.Text = "范围选择：";
            this.lblPSelect.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(630, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 28);
            this.btnSearch.TabIndex = 197;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageTotal);
            this.tabControl1.Controls.Add(this.tabPageDetail);
            this.tabControl1.Controls.Add(this.tabPageChart);
            this.tabControl1.Location = new System.Drawing.Point(3, 43);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(962, 557);
            this.tabControl1.TabIndex = 199;
            // 
            // tabPageTotal
            // 
            this.tabPageTotal.Controls.Add(this.flexGridTotal);
            this.tabPageTotal.Location = new System.Drawing.Point(4, 22);
            this.tabPageTotal.Name = "tabPageTotal";
            this.tabPageTotal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTotal.Size = new System.Drawing.Size(954, 531);
            this.tabPageTotal.TabIndex = 0;
            this.tabPageTotal.Text = "按项目统计";
            this.tabPageTotal.UseVisualStyleBackColor = true;
            // 
            // tabPageDetail
            // 
            this.tabPageDetail.Controls.Add(this.flexGridDetail);
            this.tabPageDetail.Location = new System.Drawing.Point(4, 22);
            this.tabPageDetail.Name = "tabPageDetail";
            this.tabPageDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDetail.Size = new System.Drawing.Size(375, 211);
            this.tabPageDetail.TabIndex = 1;
            this.tabPageDetail.Text = "按项目任务统计";
            this.tabPageDetail.UseVisualStyleBackColor = true;
            // 
            // flexGridDetail
            // 
            this.flexGridDetail.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGridDetail.CheckedImage")));
            this.flexGridDetail.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.flexGridDetail.DefaultRowHeight = ((short)(21));
            this.flexGridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flexGridDetail.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGridDetail.Location = new System.Drawing.Point(3, 3);
            this.flexGridDetail.Name = "flexGridDetail";
            this.flexGridDetail.Size = new System.Drawing.Size(369, 205);
            this.flexGridDetail.TabIndex = 160;
            this.flexGridDetail.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGridDetail.UncheckedImage")));
            // 
            // tabPageChart
            // 
            this.tabPageChart.Controls.Add(this.gdChart);
            this.tabPageChart.Location = new System.Drawing.Point(4, 22);
            this.tabPageChart.Name = "tabPageChart";
            this.tabPageChart.Size = new System.Drawing.Size(375, 211);
            this.tabPageChart.TabIndex = 2;
            this.tabPageChart.Text = "统计图表";
            this.tabPageChart.UseVisualStyleBackColor = true;
            // 
            // gdChart
            // 
            this.gdChart.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdChart.CheckedImage")));
            this.gdChart.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdChart.DefaultRowHeight = ((short)(21));
            this.gdChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdChart.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdChart.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdChart.Location = new System.Drawing.Point(0, 0);
            this.gdChart.Name = "gdChart";
            this.gdChart.Size = new System.Drawing.Size(375, 211);
            this.gdChart.TabIndex = 161;
            this.gdChart.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdChart.UncheckedImage")));
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSave.Location = new System.Drawing.Point(729, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 198;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(404, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 200;
            this.label1.Text = "日期：";
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(442, 13);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(123, 21);
            this.dtpDate.TabIndex = 201;
            // 
            // VDurationWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 603);
            this.Name = "VDurationWarning";
            this.Text = "项目工期预警";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageTotal.ResumeLayout(false);
            this.tabPageDetail.ResumeLayout(false);
            this.tabPageChart.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGridTotal;
        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageTotal;
        private System.Windows.Forms.TabPage tabPageDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGridDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSave;
        private System.Windows.Forms.TabPage tabPageChart;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdChart;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label1;
    }
}