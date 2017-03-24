namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    partial class VSetBillProperty
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSetBillProperty));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbBillName = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel24 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cmbProject = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel19 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBeginBill = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEndBill = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel20 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel13 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCreatePersonBill = new Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson();
            this.txtCodeBeginBill = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel15 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cstBillState = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cstTime = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.btnSetBill = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePersonBill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFloor.Controls.Add(this.btnSetBill);
            this.pnlFloor.Controls.Add(this.customLabel3);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.cstTime);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Controls.Add(this.cstBillState);
            this.pnlFloor.Dock = System.Windows.Forms.DockStyle.None;
            this.pnlFloor.Size = new System.Drawing.Size(943, 453);
            this.pnlFloor.Controls.SetChildIndex(this.cstBillState, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cstTime, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel3, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSetBill, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(446, -8);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.cmbBillName);
            this.groupBox1.Controls.Add(this.customLabel24);
            this.groupBox1.Controls.Add(this.cmbProject);
            this.groupBox1.Controls.Add(this.customLabel19);
            this.groupBox1.Controls.Add(this.dtpDateBeginBill);
            this.groupBox1.Controls.Add(this.dtpDateEndBill);
            this.groupBox1.Controls.Add(this.customLabel20);
            this.groupBox1.Controls.Add(this.customLabel13);
            this.groupBox1.Controls.Add(this.txtCreatePersonBill);
            this.groupBox1.Controls.Add(this.txtCodeBeginBill);
            this.groupBox1.Controls.Add(this.customLabel15);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(919, 68);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(224, 19);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 124;
            this.customLabel2.Text = "单据类型:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbBillName
            // 
            this.cmbBillName.BackColor = System.Drawing.SystemColors.Control;
            this.cmbBillName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBillName.FormattingEnabled = true;
            this.cmbBillName.Location = new System.Drawing.Point(289, 11);
            this.cmbBillName.Name = "cmbBillName";
            this.cmbBillName.Size = new System.Drawing.Size(134, 20);
            this.cmbBillName.TabIndex = 123;
            // 
            // customLabel24
            // 
            this.customLabel24.AutoSize = true;
            this.customLabel24.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel24.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel24.Location = new System.Drawing.Point(10, 19);
            this.customLabel24.Name = "customLabel24";
            this.customLabel24.Size = new System.Drawing.Size(59, 12);
            this.customLabel24.TabIndex = 119;
            this.customLabel24.Text = "项目名称:";
            this.customLabel24.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cmbProject
            // 
            this.cmbProject.BackColor = System.Drawing.SystemColors.Control;
            this.cmbProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProject.FormattingEnabled = true;
            this.cmbProject.Location = new System.Drawing.Point(75, 11);
            this.cmbProject.Name = "cmbProject";
            this.cmbProject.Size = new System.Drawing.Size(142, 20);
            this.cmbProject.TabIndex = 118;
            // 
            // customLabel19
            // 
            this.customLabel19.AutoSize = true;
            this.customLabel19.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel19.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel19.Location = new System.Drawing.Point(5, 45);
            this.customLabel19.Name = "customLabel19";
            this.customLabel19.Size = new System.Drawing.Size(59, 12);
            this.customLabel19.TabIndex = 97;
            this.customLabel19.Text = "业务日期:";
            this.customLabel19.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBeginBill
            // 
            this.dtpDateBeginBill.Location = new System.Drawing.Point(64, 41);
            this.dtpDateBeginBill.Name = "dtpDateBeginBill";
            this.dtpDateBeginBill.Size = new System.Drawing.Size(109, 21);
            this.dtpDateBeginBill.TabIndex = 95;
            // 
            // dtpDateEndBill
            // 
            this.dtpDateEndBill.Location = new System.Drawing.Point(189, 41);
            this.dtpDateEndBill.Name = "dtpDateEndBill";
            this.dtpDateEndBill.Size = new System.Drawing.Size(109, 21);
            this.dtpDateEndBill.TabIndex = 96;
            // 
            // customLabel20
            // 
            this.customLabel20.AutoSize = true;
            this.customLabel20.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel20.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel20.Location = new System.Drawing.Point(175, 45);
            this.customLabel20.Name = "customLabel20";
            this.customLabel20.Size = new System.Drawing.Size(11, 12);
            this.customLabel20.TabIndex = 98;
            this.customLabel20.Text = "-";
            this.customLabel20.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel13
            // 
            this.customLabel13.AutoSize = true;
            this.customLabel13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel13.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel13.Location = new System.Drawing.Point(429, 17);
            this.customLabel13.Name = "customLabel13";
            this.customLabel13.Size = new System.Drawing.Size(47, 12);
            this.customLabel13.TabIndex = 94;
            this.customLabel13.Text = "制单人:";
            this.customLabel13.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCreatePersonBill
            // 
            this.txtCreatePersonBill.AcceptsEscape = false;
            this.txtCreatePersonBill.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreatePersonBill.Code = null;
            this.txtCreatePersonBill.CustomBorderStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditBorderStyle.None;
            this.txtCreatePersonBill.EditStyle = Application.Business.Erp.ResourceManager.Client.Basic.CommonClass.EditStyle.Text;
            this.txtCreatePersonBill.EnterToTab = false;
            this.txtCreatePersonBill.Id = "";
            this.txtCreatePersonBill.IsAllLoad = true;
            this.txtCreatePersonBill.Location = new System.Drawing.Point(482, 10);
            this.txtCreatePersonBill.Name = "txtCreatePersonBill";
            this.txtCreatePersonBill.Result = ((System.Collections.IList)(resources.GetObject("txtCreatePersonBill.Result")));
            this.txtCreatePersonBill.RightMouse = false;
            this.txtCreatePersonBill.Size = new System.Drawing.Size(134, 21);
            this.txtCreatePersonBill.TabIndex = 93;
            this.txtCreatePersonBill.Tag = null;
            this.txtCreatePersonBill.Value = "";
            this.txtCreatePersonBill.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // txtCodeBeginBill
            // 
            this.txtCodeBeginBill.BackColor = System.Drawing.SystemColors.Control;
            this.txtCodeBeginBill.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCodeBeginBill.DrawSelf = false;
            this.txtCodeBeginBill.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCodeBeginBill.EnterToTab = false;
            this.txtCodeBeginBill.Location = new System.Drawing.Point(690, 15);
            this.txtCodeBeginBill.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCodeBeginBill.Name = "txtCodeBeginBill";
            this.txtCodeBeginBill.Padding = new System.Windows.Forms.Padding(1);
            this.txtCodeBeginBill.ReadOnly = false;
            this.txtCodeBeginBill.Size = new System.Drawing.Size(161, 16);
            this.txtCodeBeginBill.TabIndex = 91;
            // 
            // customLabel15
            // 
            this.customLabel15.AutoSize = true;
            this.customLabel15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel15.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel15.Location = new System.Drawing.Point(637, 19);
            this.customLabel15.Name = "customLabel15";
            this.customLabel15.Size = new System.Drawing.Size(47, 12);
            this.customLabel15.TabIndex = 92;
            this.customLabel15.Text = "单据号:";
            this.customLabel15.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(710, 37);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
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
            this.dgDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colID,
            this.colCreatePerson,
            this.colCreateTime,
            this.colState});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgDetail.EnableHeadersVisualStyles = false;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(12, 79);
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
            this.dgDetail.Size = new System.Drawing.Size(919, 329);
            this.dgDetail.TabIndex = 3;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colCode
            // 
            this.colCode.FillWeight = 150F;
            this.colCode.HeaderText = "单据号";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colCode.Width = 80;
            // 
            // colID
            // 
            this.colID.HeaderText = "单据ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Visible = false;
            // 
            // colCreatePerson
            // 
            this.colCreatePerson.HeaderText = "制单人";
            this.colCreatePerson.Name = "colCreatePerson";
            this.colCreatePerson.ReadOnly = true;
            // 
            // colCreateTime
            // 
            this.colCreateTime.FillWeight = 150F;
            this.colCreateTime.HeaderText = "业务时间";
            this.colCreateTime.Name = "colCreateTime";
            // 
            // colState
            // 
            this.colState.HeaderText = "单据状态";
            this.colState.Name = "colState";
            this.colState.ReadOnly = true;
            // 
            // customLabel1
            // 
            this.customLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(196, 425);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(59, 12);
            this.customLabel1.TabIndex = 126;
            this.customLabel1.Text = "单据状态:";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cstBillState
            // 
            this.cstBillState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cstBillState.BackColor = System.Drawing.SystemColors.Control;
            this.cstBillState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cstBillState.FormattingEnabled = true;
            this.cstBillState.Location = new System.Drawing.Point(261, 417);
            this.cstBillState.Name = "cstBillState";
            this.cstBillState.Size = new System.Drawing.Size(134, 20);
            this.cstBillState.TabIndex = 125;
            // 
            // customLabel3
            // 
            this.customLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(16, 425);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 128;
            this.customLabel3.Text = "业务日期:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cstTime
            // 
            this.cstTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cstTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.cstTime.Location = new System.Drawing.Point(75, 416);
            this.cstTime.Name = "cstTime";
            this.cstTime.Size = new System.Drawing.Size(109, 21);
            this.cstTime.TabIndex = 127;
            // 
            // btnSetBill
            // 
            this.btnSetBill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetBill.Location = new System.Drawing.Point(413, 414);
            this.btnSetBill.Name = "btnSetBill";
            this.btnSetBill.Size = new System.Drawing.Size(75, 23);
            this.btnSetBill.TabIndex = 129;
            this.btnSetBill.Text = "保存";
            this.btnSetBill.UseVisualStyleBackColor = true;
            // 
            // VSetBillProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 456);
            this.Name = "VSetBillProperty";
            this.Text = "业务单据修改";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCreatePersonBill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCodeBeginBill;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel15;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel13;
        private Application.Business.Erp.ResourceManager.Client.Basic.Controls.CommonPerson txtCreatePersonBill;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel19;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBeginBill;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEndBill;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel20;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel24;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cmbProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cmbBillName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker cstTime;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cstBillState;
        private System.Windows.Forms.DataGridViewLinkColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private System.Windows.Forms.Button btnSetBill;
    }
}