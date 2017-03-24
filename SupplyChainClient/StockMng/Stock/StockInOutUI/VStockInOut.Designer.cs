namespace Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI
{
    partial class VStockInOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VStockInOut));
            this.btnReckoning = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnUnReckoning = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboFiscalMonth = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.btnSelectGWBS = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtAccountRootNode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgMaster = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colTaskNode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAccountOrg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cboFiscalYear = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.pnlContent.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlWorkSpace.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Size = new System.Drawing.Size(662, 430);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.groupBox2);
            this.pnlBody.Controls.Add(this.groupBox1);
            this.pnlBody.Location = new System.Drawing.Point(0, 0);
            this.pnlBody.Size = new System.Drawing.Size(660, 428);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFooter.Location = new System.Drawing.Point(0, 0);
            this.pnlFooter.Size = new System.Drawing.Size(660, 428);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(662, 10);
            this.spTop.Visible = false;
            // 
            // pnlWorkSpace
            // 
            this.pnlWorkSpace.Location = new System.Drawing.Point(0, 10);
            this.pnlWorkSpace.Size = new System.Drawing.Size(662, 430);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.None;
            this.pnlHeader.Size = new System.Drawing.Size(868, 74);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(308, 30);
            this.lblTitle.Size = new System.Drawing.Size(177, 20);
            this.lblTitle.Text = "物资实际耗用结算";
            // 
            // btnReckoning
            // 
            this.btnReckoning.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReckoning.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReckoning.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReckoning.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnReckoning.Location = new System.Drawing.Point(254, 79);
            this.btnReckoning.Name = "btnReckoning";
            this.btnReckoning.Size = new System.Drawing.Size(90, 28);
            this.btnReckoning.TabIndex = 34;
            this.btnReckoning.Text = "物资耗用结算";
            this.btnReckoning.UseVisualStyleBackColor = true;
            // 
            // btnUnReckoning
            // 
            this.btnUnReckoning.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnUnReckoning.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnReckoning.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUnReckoning.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnUnReckoning.Location = new System.Drawing.Point(263, 220);
            this.btnUnReckoning.Name = "btnUnReckoning";
            this.btnUnReckoning.Size = new System.Drawing.Size(92, 28);
            this.btnUnReckoning.TabIndex = 34;
            this.btnUnReckoning.Text = "物资耗用反结";
            this.btnUnReckoning.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboFiscalYear);
            this.groupBox1.Controls.Add(this.cboFiscalMonth);
            this.groupBox1.Controls.Add(this.btnSelectGWBS);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.customLabel5);
            this.groupBox1.Controls.Add(this.txtAccountRootNode);
            this.groupBox1.Controls.Add(this.btnReckoning);
            this.groupBox1.Controls.Add(this.customLabel3);
            this.groupBox1.Location = new System.Drawing.Point(40, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(612, 123);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "物资耗用结算";
            // 
            // cboFiscalMonth
            // 
            this.cboFiscalMonth.BackColor = System.Drawing.SystemColors.Control;
            this.cboFiscalMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFiscalMonth.FormattingEnabled = true;
            this.cboFiscalMonth.Location = new System.Drawing.Point(137, 34);
            this.cboFiscalMonth.Name = "cboFiscalMonth";
            this.cboFiscalMonth.Size = new System.Drawing.Size(41, 20);
            this.cboFiscalMonth.TabIndex = 169;
            // 
            // btnSelectGWBS
            // 
            this.btnSelectGWBS.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectGWBS.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectGWBS.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSelectGWBS.Location = new System.Drawing.Point(388, 34);
            this.btnSelectGWBS.Name = "btnSelectGWBS";
            this.btnSelectGWBS.Size = new System.Drawing.Size(57, 22);
            this.btnSelectGWBS.TabIndex = 149;
            this.btnSelectGWBS.Text = "选择";
            this.btnSelectGWBS.UseVisualStyleBackColor = true;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(13, 37);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(47, 12);
            this.customLabel2.TabIndex = 144;
            this.customLabel2.Text = "会计期:";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(122, 39);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(11, 12);
            this.customLabel5.TabIndex = 143;
            this.customLabel5.Text = "-";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtAccountRootNode
            // 
            this.txtAccountRootNode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtAccountRootNode.DrawSelf = false;
            this.txtAccountRootNode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtAccountRootNode.EnterToTab = false;
            this.txtAccountRootNode.Location = new System.Drawing.Point(278, 38);
            this.txtAccountRootNode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtAccountRootNode.Name = "txtAccountRootNode";
            this.txtAccountRootNode.Padding = new System.Windows.Forms.Padding(1);
            this.txtAccountRootNode.ReadOnly = true;
            this.txtAccountRootNode.Size = new System.Drawing.Size(111, 16);
            this.txtAccountRootNode.TabIndex = 148;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(219, 40);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(59, 12);
            this.customLabel3.TabIndex = 147;
            this.customLabel3.Text = "核算范围:";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgMaster);
            this.groupBox2.Controls.Add(this.btnUnReckoning);
            this.groupBox2.Location = new System.Drawing.Point(40, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(612, 261);
            this.groupBox2.TabIndex = 148;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "物资耗用反结";
            // 
            // dgMaster
            // 
            this.dgMaster.AddDefaultMenu = false;
            this.dgMaster.AddNoColumn = false;
            this.dgMaster.AllowUserToAddRows = false;
            this.dgMaster.AllowUserToDeleteRows = false;
            this.dgMaster.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgMaster.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgMaster.CausesValidation = false;
            this.dgMaster.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgMaster.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.ColumnHeadersHeight = 24;
            this.dgMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTaskNode,
            this.colAccountOrg,
            this.colEndDate,
            this.colPerson,
            this.colOperDate,
            this.colState});
            this.dgMaster.CustomBackColor = false;
            this.dgMaster.EditCellBackColor = System.Drawing.Color.White;
            this.dgMaster.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgMaster.EnableHeadersVisualStyles = false;
            this.dgMaster.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgMaster.FreezeFirstRow = false;
            this.dgMaster.FreezeLastRow = false;
            this.dgMaster.FrontColumnCount = 0;
            this.dgMaster.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgMaster.HScrollOffset = 0;
            this.dgMaster.IsAllowOrder = true;
            this.dgMaster.IsConfirmDelete = true;
            this.dgMaster.Location = new System.Drawing.Point(2, 15);
            this.dgMaster.Name = "dgMaster";
            this.dgMaster.PageIndex = 0;
            this.dgMaster.PageSize = 0;
            this.dgMaster.Query = null;
            this.dgMaster.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgMaster.ReadOnlyCols")));
            this.dgMaster.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgMaster.RowHeadersVisible = false;
            this.dgMaster.RowHeadersWidth = 22;
            this.dgMaster.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMaster.RowTemplate.Height = 23;
            this.dgMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMaster.Size = new System.Drawing.Size(606, 199);
            this.dgMaster.TabIndex = 148;
            this.dgMaster.TargetType = null;
            this.dgMaster.VScrollOffset = 0;
            // 
            // colTaskNode
            // 
            this.colTaskNode.HeaderText = "核算节点";
            this.colTaskNode.Name = "colTaskNode";
            this.colTaskNode.Width = 150;
            // 
            // colAccountOrg
            // 
            this.colAccountOrg.HeaderText = "核算组织";
            this.colAccountOrg.Name = "colAccountOrg";
            this.colAccountOrg.Width = 150;
            // 
            // colEndDate
            // 
            this.colEndDate.HeaderText = "结束日期";
            this.colEndDate.Name = "colEndDate";
            // 
            // colPerson
            // 
            this.colPerson.HeaderText = "制单人";
            this.colPerson.Name = "colPerson";
            // 
            // colOperDate
            // 
            this.colOperDate.HeaderText = "制单日期";
            this.colOperDate.Name = "colOperDate";
            // 
            // colState
            // 
            this.colState.HeaderText = "状态";
            this.colState.Name = "colState";
            this.colState.Visible = false;
            this.colState.Width = 80;
            // 
            // cboFiscalYear
            // 
            this.cboFiscalYear.BackColor = System.Drawing.SystemColors.Control;
            this.cboFiscalYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFiscalYear.FormattingEnabled = true;
            this.cboFiscalYear.Location = new System.Drawing.Point(56, 34);
            this.cboFiscalYear.Name = "cboFiscalYear";
            this.cboFiscalYear.Size = new System.Drawing.Size(66, 20);
            this.cboFiscalYear.TabIndex = 170;
            // 
            // VStockInOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 440);
            this.Name = "VStockInOut";
            this.Text = "物资实际耗用结算";
            this.pnlContent.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlWorkSpace.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomButton btnReckoning;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnUnReckoning;
        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboFiscalMonth;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSelectGWBS;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtAccountRootNode;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskNode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAccountOrg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboFiscalYear;
    }
}