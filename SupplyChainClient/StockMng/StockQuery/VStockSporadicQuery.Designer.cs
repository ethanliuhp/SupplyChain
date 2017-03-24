namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockQuery
{
    partial class VStockSporadicQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VStockSporadicQuery));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblPSelect = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtProject = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel24 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel36 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.dtpDateBeginMv = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.dtpDateEndMv = new VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker();
            this.customLabel37 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSearch = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colTotalMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSporadicMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNetMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colElectMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSporadicRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNetRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colElecRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.dgDetail);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(972, 498);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
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
            this.groupBox1.Controls.Add(this.btnOperationOrg);
            this.groupBox1.Controls.Add(this.txtOperationOrg);
            this.groupBox1.Controls.Add(this.lblPSelect);
            this.groupBox1.Controls.Add(this.txtProject);
            this.groupBox1.Controls.Add(this.customLabel24);
            this.groupBox1.Controls.Add(this.customLabel36);
            this.groupBox1.Controls.Add(this.dtpDateBeginMv);
            this.groupBox1.Controls.Add(this.dtpDateEndMv);
            this.groupBox1.Controls.Add(this.customLabel37);
            this.groupBox1.Controls.Add(this.btnExcel);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(948, 46);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(697, 12);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 169;
            this.btnOperationOrg.Text = "选择";
            this.btnOperationOrg.UseVisualStyleBackColor = true;
            this.btnOperationOrg.Click += new System.EventHandler(this.btnOperationOrg_Click);
            // 
            // txtOperationOrg
            // 
            this.txtOperationOrg.BackColor = System.Drawing.SystemColors.Control;
            this.txtOperationOrg.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOperationOrg.DrawSelf = false;
            this.txtOperationOrg.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOperationOrg.EnterToTab = false;
            this.txtOperationOrg.Location = new System.Drawing.Point(572, 15);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(124, 16);
            this.txtOperationOrg.TabIndex = 168;
            // 
            // lblPSelect
            // 
            this.lblPSelect.AutoSize = true;
            this.lblPSelect.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPSelect.Location = new System.Drawing.Point(511, 18);
            this.lblPSelect.Name = "lblPSelect";
            this.lblPSelect.Size = new System.Drawing.Size(59, 12);
            this.lblPSelect.TabIndex = 167;
            this.lblPSelect.Text = "项目选择:";
            this.lblPSelect.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtProject
            // 
            this.txtProject.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtProject.DrawSelf = false;
            this.txtProject.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtProject.EnterToTab = false;
            this.txtProject.Location = new System.Drawing.Point(63, 15);
            this.txtProject.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtProject.Name = "txtProject";
            this.txtProject.Padding = new System.Windows.Forms.Padding(1);
            this.txtProject.ReadOnly = true;
            this.txtProject.Size = new System.Drawing.Size(141, 16);
            this.txtProject.TabIndex = 158;
            // 
            // customLabel24
            // 
            this.customLabel24.AutoSize = true;
            this.customLabel24.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel24.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel24.Location = new System.Drawing.Point(1, 19);
            this.customLabel24.Name = "customLabel24";
            this.customLabel24.Size = new System.Drawing.Size(59, 12);
            this.customLabel24.TabIndex = 157;
            this.customLabel24.Text = "项目名称:";
            this.customLabel24.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel36
            // 
            this.customLabel36.AutoSize = true;
            this.customLabel36.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel36.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel36.Location = new System.Drawing.Point(211, 18);
            this.customLabel36.Name = "customLabel36";
            this.customLabel36.Size = new System.Drawing.Size(59, 12);
            this.customLabel36.TabIndex = 86;
            this.customLabel36.Text = "业务日期:";
            this.customLabel36.UnderLineColor = System.Drawing.Color.Red;
            // 
            // dtpDateBeginMv
            // 
            this.dtpDateBeginMv.Location = new System.Drawing.Point(271, 13);
            this.dtpDateBeginMv.Name = "dtpDateBeginMv";
            this.dtpDateBeginMv.Size = new System.Drawing.Size(110, 21);
            this.dtpDateBeginMv.TabIndex = 84;
            // 
            // dtpDateEndMv
            // 
            this.dtpDateEndMv.Location = new System.Drawing.Point(394, 13);
            this.dtpDateEndMv.Name = "dtpDateEndMv";
            this.dtpDateEndMv.Size = new System.Drawing.Size(110, 21);
            this.dtpDateEndMv.TabIndex = 85;
            // 
            // customLabel37
            // 
            this.customLabel37.AutoSize = true;
            this.customLabel37.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel37.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel37.Location = new System.Drawing.Point(382, 17);
            this.customLabel37.Name = "customLabel37";
            this.customLabel37.Size = new System.Drawing.Size(11, 12);
            this.customLabel37.TabIndex = 87;
            this.customLabel37.Text = "-";
            this.customLabel37.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(847, 12);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExcel.TabIndex = 12;
            this.btnExcel.Text = "导出到Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSearch.Location = new System.Drawing.Point(770, 12);
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
            this.dgDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeight = 24;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTotalMoney,
            this.colSporadicMoney,
            this.colNetMoney,
            this.colElectMaterial,
            this.colSporadicRate,
            this.colNetRate,
            this.colElecRate});
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
            this.dgDetail.Location = new System.Drawing.Point(12, 57);
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
            this.dgDetail.Size = new System.Drawing.Size(948, 429);
            this.dgDetail.TabIndex = 3;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colTotalMoney
            // 
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = "0";
            this.colTotalMoney.DefaultCellStyle = dataGridViewCellStyle1;
            this.colTotalMoney.HeaderText = "物资消耗总金额";
            this.colTotalMoney.Name = "colTotalMoney";
            this.colTotalMoney.Width = 150;
            // 
            // colSporadicMoney
            // 
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = "0";
            this.colSporadicMoney.DefaultCellStyle = dataGridViewCellStyle2;
            this.colSporadicMoney.HeaderText = "零星材料消耗金额";
            this.colSporadicMoney.Name = "colSporadicMoney";
            this.colSporadicMoney.Width = 150;
            // 
            // colNetMoney
            // 
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = "0";
            this.colNetMoney.DefaultCellStyle = dataGridViewCellStyle3;
            this.colNetMoney.HeaderText = "安全网消耗金额";
            this.colNetMoney.Name = "colNetMoney";
            this.colNetMoney.Width = 120;
            // 
            // colElectMaterial
            // 
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = "0";
            this.colElectMaterial.DefaultCellStyle = dataGridViewCellStyle4;
            this.colElectMaterial.HeaderText = "电料消耗金额";
            this.colElectMaterial.Name = "colElectMaterial";
            // 
            // colSporadicRate
            // 
            this.colSporadicRate.HeaderText = "零星材料比例(%)";
            this.colSporadicRate.Name = "colSporadicRate";
            // 
            // colNetRate
            // 
            this.colNetRate.HeaderText = "安全网比例(%)";
            this.colNetRate.Name = "colNetRate";
            // 
            // colElecRate
            // 
            this.colElecRate.HeaderText = "电料比例(%)";
            this.colElecRate.Name = "colElecRate";
            // 
            // VStockSporadicQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 498);
            this.Name = "VStockSporadicQuery";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel36;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateBeginMv;
        private VirtualMachine.Component.WinControls.Controls.CustomDateTimePicker dtpDateEndMv;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel37;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtProject;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel24;
        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSporadicMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn colElectMaterial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSporadicRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNetRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colElecRate;
    }
}