namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.CompleteSettlementBook
{
    partial class VCompleteQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VCompleteQuery));
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.No = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ContractDocName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RealTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubmitMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZhengquMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SureMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BeginMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RealCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Benefit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Benefitlv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Handleperson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealOperationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpCreateDateBegin = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpCreateDateEnd = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtHandlePerson = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProjectName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtContractDocName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSumMoney = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtSumQuantity = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblRecordTotal = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.txtSumMoney);
            this.pnlFloor.Controls.Add(this.customLabel2);
            this.pnlFloor.Controls.Add(this.txtSumQuantity);
            this.pnlFloor.Controls.Add(this.lblRecordTotal);
            this.pnlFloor.Controls.Add(this.customLabel5);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Size = new System.Drawing.Size(987, 501);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.dgDetail, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel5, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblRecordTotal, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtSumQuantity, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtSumMoney, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(194, 0);
            this.lblTitle.Size = new System.Drawing.Size(0, 20);
            this.lblTitle.Text = "";
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
            this.No,
            this.ContractDocName,
            this.RealTime,
            this.SubmitMoney,
            this.ZhengquMoney,
            this.SureMoney,
            this.BeginMoney,
            this.EndMoney,
            this.RealCost,
            this.Benefit,
            this.Benefitlv,
            this.Handleperson,
            this.colRealOperationDate,
            this.CreateTime});
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
            this.dgDetail.Location = new System.Drawing.Point(3, 76);
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
            this.dgDetail.Size = new System.Drawing.Size(981, 396);
            this.dgDetail.TabIndex = 149;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // No
            // 
            this.No.HeaderText = "序号";
            this.No.Name = "No";
            this.No.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.No.Width = 80;
            // 
            // ContractDocName
            // 
            this.ContractDocName.HeaderText = "文档名称";
            this.ContractDocName.Name = "ContractDocName";
            // 
            // RealTime
            // 
            this.RealTime.HeaderText = "实际结算完成时间";
            this.RealTime.Name = "RealTime";
            this.RealTime.Width = 120;
            // 
            // SubmitMoney
            // 
            this.SubmitMoney.HeaderText = "报送总金额";
            this.SubmitMoney.Name = "SubmitMoney";
            // 
            // ZhengquMoney
            // 
            this.ZhengquMoney.HeaderText = "争取结算金额";
            this.ZhengquMoney.Name = "ZhengquMoney";
            // 
            // SureMoney
            // 
            this.SureMoney.HeaderText = "确保结算金额";
            this.SureMoney.Name = "SureMoney";
            // 
            // BeginMoney
            // 
            this.BeginMoney.HeaderText = "初次审定总金额";
            this.BeginMoney.Name = "BeginMoney";
            this.BeginMoney.Width = 120;
            // 
            // EndMoney
            // 
            this.EndMoney.HeaderText = "审定总金额";
            this.EndMoney.Name = "EndMoney";
            // 
            // RealCost
            // 
            this.RealCost.HeaderText = "实际成本";
            this.RealCost.Name = "RealCost";
            // 
            // Benefit
            // 
            this.Benefit.HeaderText = "效益额";
            this.Benefit.Name = "Benefit";
            // 
            // Benefitlv
            // 
            this.Benefitlv.HeaderText = "效益率";
            this.Benefitlv.Name = "Benefitlv";
            // 
            // Handleperson
            // 
            this.Handleperson.HeaderText = "责任人";
            this.Handleperson.Name = "Handleperson";
            this.Handleperson.Width = 80;
            // 
            // colRealOperationDate
            // 
            this.colRealOperationDate.HeaderText = "制单时间";
            this.colRealOperationDate.Name = "colRealOperationDate";
            // 
            // CreateTime
            // 
            this.CreateTime.HeaderText = "业务时间";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.Width = 80;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpCreateDateBegin);
            this.groupBox1.Controls.Add(this.dtpCreateDateEnd);
            this.groupBox1.Controls.Add(this.customLabel9);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtHandlePerson);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtProjectName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtContractDocName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(3, -3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(981, 73);
            this.groupBox1.TabIndex = 150;
            this.groupBox1.TabStop = false;
            // 
            // dtpCreateDateBegin
            // 
            this.dtpCreateDateBegin.Location = new System.Drawing.Point(714, 16);
            this.dtpCreateDateBegin.Name = "dtpCreateDateBegin";
            this.dtpCreateDateBegin.Size = new System.Drawing.Size(106, 21);
            this.dtpCreateDateBegin.TabIndex = 207;
            this.dtpCreateDateBegin.Value = new System.DateTime(2012, 7, 10, 20, 3, 0, 0);
            // 
            // dtpCreateDateEnd
            // 
            this.dtpCreateDateEnd.Location = new System.Drawing.Point(843, 16);
            this.dtpCreateDateEnd.Name = "dtpCreateDateEnd";
            this.dtpCreateDateEnd.Size = new System.Drawing.Size(106, 21);
            this.dtpCreateDateEnd.TabIndex = 208;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(826, 20);
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
            this.customLabel1.Location = new System.Drawing.Point(651, 22);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 206;
            this.customLabel1.Text = "业务时间：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(876, 42);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 172;
            this.btnExcel.Text = "导出Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(788, 43);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 171;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtHandlePerson
            // 
            this.txtHandlePerson.BackColor = System.Drawing.SystemColors.Control;
            this.txtHandlePerson.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtHandlePerson.DrawSelf = false;
            this.txtHandlePerson.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtHandlePerson.EnterToTab = false;
            this.txtHandlePerson.Location = new System.Drawing.Point(511, 19);
            this.txtHandlePerson.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtHandlePerson.Name = "txtHandlePerson";
            this.txtHandlePerson.Padding = new System.Windows.Forms.Padding(1);
            this.txtHandlePerson.ReadOnly = false;
            this.txtHandlePerson.Size = new System.Drawing.Size(117, 16);
            this.txtHandlePerson.TabIndex = 170;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(462, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 169;
            this.label2.Text = "责任人：";
            // 
            // txtProjectName
            // 
            this.txtProjectName.BackColor = System.Drawing.SystemColors.Control;
            this.txtProjectName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProjectName.DrawSelf = false;
            this.txtProjectName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProjectName.EnterToTab = false;
            this.txtProjectName.Location = new System.Drawing.Point(277, 19);
            this.txtProjectName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Padding = new System.Windows.Forms.Padding(1);
            this.txtProjectName.ReadOnly = false;
            this.txtProjectName.Size = new System.Drawing.Size(165, 16);
            this.txtProjectName.TabIndex = 168;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 167;
            this.label1.Text = "项目名称：";
            // 
            // txtContractDocName
            // 
            this.txtContractDocName.BackColor = System.Drawing.SystemColors.Control;
            this.txtContractDocName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtContractDocName.DrawSelf = false;
            this.txtContractDocName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtContractDocName.EnterToTab = false;
            this.txtContractDocName.Location = new System.Drawing.Point(77, 19);
            this.txtContractDocName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtContractDocName.Name = "txtContractDocName";
            this.txtContractDocName.Padding = new System.Windows.Forms.Padding(1);
            this.txtContractDocName.ReadOnly = false;
            this.txtContractDocName.Size = new System.Drawing.Size(117, 16);
            this.txtContractDocName.TabIndex = 166;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 165;
            this.label4.Text = "文档名称：";
            // 
            // txtSumMoney
            // 
            this.txtSumMoney.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtSumMoney.BackColor = System.Drawing.SystemColors.Control;
            this.txtSumMoney.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumMoney.DrawSelf = false;
            this.txtSumMoney.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumMoney.EnterToTab = false;
            this.txtSumMoney.Location = new System.Drawing.Point(871, 478);
            this.txtSumMoney.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumMoney.Name = "txtSumMoney";
            this.txtSumMoney.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumMoney.ReadOnly = true;
            this.txtSumMoney.Size = new System.Drawing.Size(104, 16);
            this.txtSumMoney.TabIndex = 155;
            // 
            // customLabel2
            // 
            this.customLabel2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(806, 482);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(59, 12);
            this.customLabel2.TabIndex = 154;
            this.customLabel2.Text = "合计成本:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtSumQuantity
            // 
            this.txtSumQuantity.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtSumQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.txtSumQuantity.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtSumQuantity.DrawSelf = false;
            this.txtSumQuantity.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtSumQuantity.EnterToTab = false;
            this.txtSumQuantity.Location = new System.Drawing.Point(701, 478);
            this.txtSumQuantity.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtSumQuantity.Name = "txtSumQuantity";
            this.txtSumQuantity.Padding = new System.Windows.Forms.Padding(1);
            this.txtSumQuantity.ReadOnly = true;
            this.txtSumQuantity.Size = new System.Drawing.Size(104, 16);
            this.txtSumQuantity.TabIndex = 153;
            // 
            // lblRecordTotal
            // 
            this.lblRecordTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblRecordTotal.AutoSize = true;
            this.lblRecordTotal.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordTotal.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblRecordTotal.ForeColor = System.Drawing.Color.Red;
            this.lblRecordTotal.Location = new System.Drawing.Point(495, 482);
            this.lblRecordTotal.Name = "lblRecordTotal";
            this.lblRecordTotal.Size = new System.Drawing.Size(83, 12);
            this.lblRecordTotal.TabIndex = 151;
            this.lblRecordTotal.Text = "共【0】条记录";
            this.lblRecordTotal.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel5
            // 
            this.customLabel5.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(596, 482);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(101, 12);
            this.customLabel5.TabIndex = 152;
            this.customLabel5.Text = "审定总金额合计：";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VCompleteQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 501);
            this.Name = "VCompleteQuery";
            this.Text = "竣工结算书查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtHandlePerson;
        private System.Windows.Forms.Label label2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProjectName;
        private System.Windows.Forms.Label label1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtContractDocName;
        private System.Windows.Forms.Label label4;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumMoney;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtSumQuantity;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblRecordTotal;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpCreateDateBegin;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpCreateDateEnd;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.DataGridViewLinkColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContractDocName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RealTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubmitMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZhengquMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn SureMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn BeginMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn RealCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn Benefit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Benefitlv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Handleperson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealOperationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;

    }
}