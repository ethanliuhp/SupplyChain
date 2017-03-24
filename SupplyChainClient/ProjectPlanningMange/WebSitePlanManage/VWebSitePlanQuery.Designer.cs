namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.WebSitePlanManage
{
    partial class VWebSitePlanQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VWebSitePlanQuery));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCreatePerson = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.cmbEvaluationWay = new System.Windows.Forms.ComboBox();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dtpDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel8 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbEnginnerType = new System.Windows.Forms.ComboBox();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtEnginnerName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colEnginnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEvaluationWay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnginnerType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubmitDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDcoState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(1016, 450);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtCreatePerson);
            this.groupBox1.Controls.Add(this.cmbEvaluationWay);
            this.groupBox1.Controls.Add(this.customLabel4);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.dtpDateBegin);
            this.groupBox1.Controls.Add(this.dtpDateEnd);
            this.groupBox1.Controls.Add(this.customLabel9);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.customLabel8);
            this.groupBox1.Controls.Add(this.cmbEnginnerType);
            this.groupBox1.Controls.Add(this.customLabel7);
            this.groupBox1.Controls.Add(this.txtEnginnerName);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Location = new System.Drawing.Point(6, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1005, 69);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "<<专业策划维护";
            // 
            // txtCreatePerson
            // 
            this.txtCreatePerson.AcceptsEscape = false;
            this.txtCreatePerson.AutoSize = false;
            this.txtCreatePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePerson.Code = null;
            this.txtCreatePerson.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtCreatePerson.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtCreatePerson.EnterToTab = false;
            this.txtCreatePerson.Font = new System.Drawing.Font("宋体", 10.5F);
            this.txtCreatePerson.Id = "";
            this.txtCreatePerson.IsAllLoad = true;
            this.txtCreatePerson.Location = new System.Drawing.Point(292, 17);
            this.txtCreatePerson.Name = "txtCreatePerson";
            this.txtCreatePerson.Result = ((System.Collections.IList)(resources.GetObject("txtCreatePerson.Result")));
            this.txtCreatePerson.RightMouse = false;
            this.txtCreatePerson.Size = new System.Drawing.Size(121, 19);
            this.txtCreatePerson.TabIndex = 148;
            this.txtCreatePerson.Tag = null;
            this.txtCreatePerson.Value = "";
            this.txtCreatePerson.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // cmbEvaluationWay
            // 
            this.cmbEvaluationWay.FormattingEnabled = true;
            this.cmbEvaluationWay.Location = new System.Drawing.Point(89, 42);
            this.cmbEvaluationWay.Name = "cmbEvaluationWay";
            this.cmbEvaluationWay.Size = new System.Drawing.Size(121, 20);
            this.cmbEvaluationWay.TabIndex = 213;
            // 
            // customLabel4
            // 
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(233, 23);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(53, 12);
            this.customLabel4.TabIndex = 147;
            this.customLabel4.Text = "责任人：";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(818, 42);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(78, 23);
            this.btnExcel.TabIndex = 212;
            this.btnExcel.Text = "导出Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(734, 42);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(78, 23);
            this.btnSearch.TabIndex = 211;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // dtpDateBegin
            // 
            this.dtpDateBegin.Location = new System.Drawing.Point(518, 18);
            this.dtpDateBegin.Name = "dtpDateBegin";
            this.dtpDateBegin.Size = new System.Drawing.Size(106, 21);
            this.dtpDateBegin.TabIndex = 207;
            this.dtpDateBegin.Value = new System.DateTime(2012, 7, 10, 8, 47, 0, 0);
            // 
            // dtpDateEnd
            // 
            this.dtpDateEnd.Location = new System.Drawing.Point(634, 18);
            this.dtpDateEnd.Name = "dtpDateEnd";
            this.dtpDateEnd.Size = new System.Drawing.Size(106, 21);
            this.dtpDateEnd.TabIndex = 208;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(624, 23);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(11, 12);
            this.customLabel9.TabIndex = 209;
            this.customLabel9.Text = "-";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(447, 23);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 206;
            this.customLabel1.Text = "提交时间：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel8
            // 
            this.customLabel8.AutoSize = true;
            this.customLabel8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel8.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel8.Location = new System.Drawing.Point(18, 45);
            this.customLabel8.Name = "customLabel8";
            this.customLabel8.Size = new System.Drawing.Size(65, 12);
            this.customLabel8.TabIndex = 151;
            this.customLabel8.Text = "评审方式：";
            this.customLabel8.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbEnginnerType
            // 
            this.cmbEnginnerType.FormattingEnabled = true;
            this.cmbEnginnerType.Location = new System.Drawing.Point(866, 18);
            this.cmbEnginnerType.Name = "cmbEnginnerType";
            this.cmbEnginnerType.Size = new System.Drawing.Size(121, 20);
            this.cmbEnginnerType.TabIndex = 150;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(771, 24);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(89, 12);
            this.customLabel7.TabIndex = 149;
            this.customLabel7.Text = "专业策划类型：";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtEnginnerName
            // 
            this.txtEnginnerName.BackColor = System.Drawing.SystemColors.Control;
            this.txtEnginnerName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtEnginnerName.DrawSelf = false;
            this.txtEnginnerName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtEnginnerName.EnterToTab = false;
            this.txtEnginnerName.Location = new System.Drawing.Point(89, 19);
            this.txtEnginnerName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtEnginnerName.Name = "txtEnginnerName";
            this.txtEnginnerName.Padding = new System.Windows.Forms.Padding(1);
            this.txtEnginnerName.ReadOnly = false;
            this.txtEnginnerName.Size = new System.Drawing.Size(121, 16);
            this.txtEnginnerName.TabIndex = 147;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(18, 23);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(65, 12);
            this.customLabel3.TabIndex = 4;
            this.customLabel3.Text = "文档名称：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
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
            this.colEnginnerName,
            this.colEvaluationWay,
            this.colEnginnerType,
            this.colSubmitDate,
            this.colCreatePerson,
            this.colCreateDate,
            this.colRealOperationDate,
            this.colRemark,
            this.colDcoState});
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
            this.dgDetail.Location = new System.Drawing.Point(5, 84);
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
            this.dgDetail.Size = new System.Drawing.Size(1008, 363);
            this.dgDetail.TabIndex = 7;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colEnginnerName
            // 
            this.colEnginnerName.HeaderText = "文档名称";
            this.colEnginnerName.Name = "colEnginnerName";
            this.colEnginnerName.Width = 80;
            // 
            // colEvaluationWay
            // 
            this.colEvaluationWay.HeaderText = "评审方式";
            this.colEvaluationWay.Name = "colEvaluationWay";
            // 
            // colEnginnerType
            // 
            this.colEnginnerType.HeaderText = "专业策划类型";
            this.colEnginnerType.Name = "colEnginnerType";
            // 
            // colSubmitDate
            // 
            this.colSubmitDate.HeaderText = "提交时间";
            this.colSubmitDate.Name = "colSubmitDate";
            this.colSubmitDate.Width = 80;
            // 
            // colCreatePerson
            // 
            this.colCreatePerson.HeaderText = "责任人";
            this.colCreatePerson.Name = "colCreatePerson";
            // 
            // colCreateDate
            // 
            this.colCreateDate.HeaderText = "业务时间";
            this.colCreateDate.Name = "colCreateDate";
            // 
            // colRealOperationDate
            // 
            this.colRealOperationDate.HeaderText = "制单时间";
            this.colRealOperationDate.Name = "colRealOperationDate";
            // 
            // colRemark
            // 
            this.colRemark.HeaderText = "备注说明";
            this.colRemark.Name = "colRemark";
            this.colRemark.Width = 80;
            // 
            // colDcoState
            // 
            this.colDcoState.HeaderText = "状态";
            this.colDcoState.Name = "colDcoState";
            this.colDcoState.Width = 80;
            // 
            // VWebSitePlanQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 450);
            this.Name = "VWebSitePlanQuery";
            this.Text = "专业策划统计查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePerson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbEvaluationWay;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel8;
        private System.Windows.Forms.ComboBox cmbEnginnerType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtEnginnerName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtCreatePerson;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnginnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEvaluationWay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnginnerType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubmitDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemark;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDcoState;
    }
}