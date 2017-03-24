namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport
{
    partial class VConstructionReportQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VConstructionReportQuery));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customLabel14 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.comMngType = new System.Windows.Forms.ComboBox();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtHandlePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.customLabel16 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHandlePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaterialCase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProblem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCompletionSchedule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOtherActivities = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectManage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSafetyControl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeather = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWind = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTemperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHumidity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRecordTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.lblRecordTotal);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(911, 498);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecordTotal, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -8);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.customLabel14);
            this.groupBox1.Controls.Add(this.comMngType);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtHandlePerson);
            this.groupBox1.Controls.Add(this.customLabel16);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel6);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(887, 53);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // customLabel14
            // 
            this.customLabel14.AutoSize = true;
            this.customLabel14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel14.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel14.Location = new System.Drawing.Point(503, 25);
            this.customLabel14.Name = "customLabel14";
            this.customLabel14.Size = new System.Drawing.Size(59, 12);
            this.customLabel14.TabIndex = 185;
            this.customLabel14.Text = "单据状态:";
            this.customLabel14.UnderLineColor = System.Drawing.Color.Red;
            // 
            // comMngType
            // 
            this.comMngType.FormattingEnabled = true;
            this.comMngType.Location = new System.Drawing.Point(564, 18);
            this.comMngType.Name = "comMngType";
            this.comMngType.Size = new System.Drawing.Size(123, 20);
            this.comMngType.TabIndex = 184;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(776, 16);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 12;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(693, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtHandlePerson
            // 
            this.txtHandlePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtHandlePerson.Code = null;
            this.txtHandlePerson.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtHandlePerson.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtHandlePerson.EnterToTab = false;
            this.txtHandlePerson.Id = "";
            this.txtHandlePerson.IsAllLoad = true;
            this.txtHandlePerson.Location = new System.Drawing.Point(373, 18);
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Result = ((System.Collections.IList)(resources.GetObject("txtHandlePerson.Result")));
            this.txtHandlePerson.RightMouse = false;
            this.txtHandlePerson.Size = new System.Drawing.Size(111, 21);
            this.txtHandlePerson.TabIndex = 8;
            this.txtHandlePerson.Tag = null;
            this.txtHandlePerson.Value = "";
            this.txtHandlePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // customLabel16
            // 
            this.customLabel16.AutoSize = true;
            this.customLabel16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel16.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel16.Location = new System.Drawing.Point(326, 23);
            this.customLabel16.Name = "customLabel16";
            this.customLabel16.Size = new System.Drawing.Size(47, 12);
            this.customLabel16.TabIndex = 86;
            this.customLabel16.Text = "责任人:";
            this.customLabel16.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(23, 24);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(35, 12);
            this.customLabel1.TabIndex = 80;
            this.customLabel1.Text = "日期:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(60, 17);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBegin.TabIndex = 3;
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(192, 17);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEnd.TabIndex = 4;
            // 
            // customLabel6
            // 
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(175, 24);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(11, 12);
            this.customLabel6.TabIndex = 83;
            this.customLabel6.Text = "-";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = false;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colPart,
            this.colProject,
            this.colHandlePerson,
            this.colMaterialCase,
            this.colProblem,
            this.colCompletionSchedule,
            this.colOtherActivities,
            this.colProjectManage,
            this.colSafetyControl,
            this.colWeather,
            this.colWind,
            this.colTemperature,
            this.colHumidity,
            this.colCreatePerson,
            this.colCreateDate});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(12, 64);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.PageIndex = 0;
            this.dgDetail.PageSize = 0;
            this.dgDetail.Query = null;
            this.dgDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDetail.ReadOnlyCols")));
            this.dgDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.RowHeadersVisible = false;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDetail.Size = new System.Drawing.Size(887, 403);
            this.dgDetail.TabIndex = 3;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colDate
            // 
            this.colDate.HeaderText = "日期";
            this.colDate.Name = "colDate";
            // 
            // colPart
            // 
            this.colPart.HeaderText = "主要施工部位";
            this.colPart.Name = "colPart";
            // 
            // colProject
            // 
            this.colProject.HeaderText = "归属项目";
            this.colProject.Name = "colProject";
            // 
            // colHandlePerson
            // 
            this.colHandlePerson.HeaderText = "责任人";
            this.colHandlePerson.Name = "colHandlePerson";
            // 
            // colMaterialCase
            // 
            this.colMaterialCase.HeaderText = "材料情况";
            this.colMaterialCase.Name = "colMaterialCase";
            // 
            // colProblem
            // 
            this.colProblem.HeaderText = "存在问题";
            this.colProblem.Name = "colProblem";
            // 
            // colCompletionSchedule
            // 
            this.colCompletionSchedule.HeaderText = "工作内容及完成情况";
            this.colCompletionSchedule.Name = "colCompletionSchedule";
            this.colCompletionSchedule.Width = 150;
            // 
            // colOtherActivities
            // 
            this.colOtherActivities.HeaderText = "其他活动情况";
            this.colOtherActivities.Name = "colOtherActivities";
            // 
            // colProjectManage
            // 
            this.colProjectManage.HeaderText = "项目管理情况";
            this.colProjectManage.Name = "colProjectManage";
            // 
            // colSafetyControl
            // 
            this.colSafetyControl.HeaderText = "生产安全控制情况";
            this.colSafetyControl.Name = "colSafetyControl";
            // 
            // colWeather
            // 
            this.colWeather.HeaderText = "天气状况";
            this.colWeather.Name = "colWeather";
            // 
            // colWind
            // 
            this.colWind.HeaderText = "风力风向";
            this.colWind.Name = "colWind";
            this.colWind.Width = 80;
            // 
            // colTemperature
            // 
            this.colTemperature.HeaderText = "温度";
            this.colTemperature.Name = "colTemperature";
            this.colTemperature.Width = 80;
            // 
            // colHumidity
            // 
            this.colHumidity.HeaderText = "相对湿度";
            this.colHumidity.Name = "colHumidity";
            this.colHumidity.Width = 80;
            // 
            // colCreatePerson
            // 
            this.colCreatePerson.HeaderText = "制单人";
            this.colCreatePerson.Name = "colCreatePerson";
            // 
            // colCreateDate
            // 
            this.colCreateDate.HeaderText = "制单日期";
            this.colCreateDate.Name = "colCreateDate";
            // 
            // lblRecordTotal
            // 
            this.lblRecordTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblRecordTotal.AutoSize = true;
            this.lblRecordTotal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecordTotal.ForeColor = System.Drawing.Color.Red;
            this.lblRecordTotal.Location = new System.Drawing.Point(757, 477);
            this.lblRecordTotal.Name = "lblRecordTotal";
            this.lblRecordTotal.Size = new System.Drawing.Size(83, 12);
            this.lblRecordTotal.TabIndex = 97;
            this.lblRecordTotal.Text = "共【0】条记录";
            this.lblRecordTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VConstructionReportQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 498);
            this.Name = "VConstructionReportQuery";
            this.Text = "日施工情况查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHandlePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtHandlePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel16;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecordTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHandlePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaterialCase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProblem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCompletionSchedule;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOtherActivities;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectManage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSafetyControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeather;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWind;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTemperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHumidity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel14;
        private System.Windows.Forms.ComboBox comMngType;
    }
}