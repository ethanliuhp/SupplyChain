namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng
{
    partial class VSeclectCommonPerson
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSeclectCommonPerson));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEnter = new System.Windows.Forms.Button();
            this.lblSelectCount = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtPersonOnJob = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtOperationorg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cbAllSelect = new System.Windows.Forms.CheckBox();
            this.dgDetail = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPersonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPersonOnJob = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOperationOrg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(924, 453);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnEnter);
            this.groupBox1.Controls.Add(this.lblSelectCount);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.txtPersonOnJob);
            this.groupBox1.Controls.Add(this.customLabel2);
            this.groupBox1.Controls.Add(this.txtOperationorg);
            this.groupBox1.Controls.Add(this.customLabel1);
            this.groupBox1.Controls.Add(this.cbAllSelect);
            this.groupBox1.Controls.Add(this.dgDetail);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(924, 453);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(673, 426);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 21);
            this.btnCancel.TabIndex = 175;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnEnter
            // 
            this.btnEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnter.Location = new System.Drawing.Point(548, 425);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(91, 22);
            this.btnEnter.TabIndex = 176;
            this.btnEnter.Text = "确定";
            this.btnEnter.UseVisualStyleBackColor = true;
            // 
            // lblSelectCount
            // 
            this.lblSelectCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectCount.AutoSize = true;
            this.lblSelectCount.ForeColor = System.Drawing.Color.Red;
            this.lblSelectCount.Location = new System.Drawing.Point(427, 431);
            this.lblSelectCount.Name = "lblSelectCount";
            this.lblSelectCount.Size = new System.Drawing.Size(71, 12);
            this.lblSelectCount.TabIndex = 174;
            this.lblSelectCount.Text = "共[0]条记录";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(780, 18);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 173;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // txtPersonOnJob
            // 
            this.txtPersonOnJob.BackColor = System.Drawing.SystemColors.Control;
            this.txtPersonOnJob.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPersonOnJob.DrawSelf = false;
            this.txtPersonOnJob.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPersonOnJob.EnterToTab = false;
            this.txtPersonOnJob.Location = new System.Drawing.Point(381, 20);
            this.txtPersonOnJob.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPersonOnJob.Name = "txtPersonOnJob";
            this.txtPersonOnJob.Padding = new System.Windows.Forms.Padding(1);
            this.txtPersonOnJob.ReadOnly = false;
            this.txtPersonOnJob.Size = new System.Drawing.Size(78, 16);
            this.txtPersonOnJob.TabIndex = 172;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(310, 23);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(65, 12);
            this.customLabel2.TabIndex = 171;
            this.customLabel2.Text = "岗位名称：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtOperationorg
            // 
            this.txtOperationorg.BackColor = System.Drawing.SystemColors.Control;
            this.txtOperationorg.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOperationorg.DrawSelf = false;
            this.txtOperationorg.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOperationorg.EnterToTab = false;
            this.txtOperationorg.Location = new System.Drawing.Point(105, 19);
            this.txtOperationorg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationorg.Name = "txtOperationorg";
            this.txtOperationorg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationorg.ReadOnly = false;
            this.txtOperationorg.Size = new System.Drawing.Size(141, 16);
            this.txtOperationorg.TabIndex = 170;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(34, 23);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 169;
            this.customLabel1.Text = "所属组织：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cbAllSelect
            // 
            this.cbAllSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbAllSelect.AutoSize = true;
            this.cbAllSelect.Location = new System.Drawing.Point(19, 430);
            this.cbAllSelect.Name = "cbAllSelect";
            this.cbAllSelect.Size = new System.Drawing.Size(102, 16);
            this.cbAllSelect.TabIndex = 165;
            this.cbAllSelect.Text = "全选/取消全选";
            this.cbAllSelect.UseVisualStyleBackColor = true;
            // 
            // dgDetail
            // 
            this.dgDetail.AddDefaultMenu = false;
            this.dgDetail.AddNoColumn = true;
            this.dgDetail.AllowUserToAddRows = false;
            this.dgDetail.AllowUserToDeleteRows = false;
            this.dgDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDetail.BackgroundColor = System.Drawing.Color.White;
            this.dgDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgDetail.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelect,
            this.colPersonName,
            this.colPersonOnJob,
            this.colOperationOrg,
            this.colProjectName});
            this.dgDetail.CustomBackColor = false;
            this.dgDetail.EditCellBackColor = System.Drawing.Color.White;
            this.dgDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgDetail.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgDetail.FreezeFirstRow = false;
            this.dgDetail.FreezeLastRow = false;
            this.dgDetail.FrontColumnCount = 0;
            this.dgDetail.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgDetail.HScrollOffset = 0;
            this.dgDetail.IsAllowOrder = true;
            this.dgDetail.IsConfirmDelete = true;
            this.dgDetail.Location = new System.Drawing.Point(4, 56);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.PageIndex = 0;
            this.dgDetail.PageSize = 0;
            this.dgDetail.Query = null;
            this.dgDetail.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgDetail.ReadOnlyCols")));
            this.dgDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgDetail.RowHeadersWidth = 22;
            this.dgDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgDetail.Size = new System.Drawing.Size(914, 363);
            this.dgDetail.TabIndex = 164;
            this.dgDetail.TargetType = null;
            this.dgDetail.VScrollOffset = 0;
            // 
            // colSelect
            // 
            this.colSelect.HeaderText = "选择";
            this.colSelect.Name = "colSelect";
            this.colSelect.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colSelect.Width = 55;
            // 
            // colPersonName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.colPersonName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPersonName.HeaderText = "姓名";
            this.colPersonName.Name = "colPersonName";
            this.colPersonName.ReadOnly = true;
            this.colPersonName.Width = 80;
            // 
            // colPersonOnJob
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            this.colPersonOnJob.DefaultCellStyle = dataGridViewCellStyle2;
            this.colPersonOnJob.HeaderText = "岗位";
            this.colPersonOnJob.Name = "colPersonOnJob";
            this.colPersonOnJob.ReadOnly = true;
            this.colPersonOnJob.Width = 80;
            // 
            // colOperationOrg
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.colOperationOrg.DefaultCellStyle = dataGridViewCellStyle3;
            this.colOperationOrg.HeaderText = "业务组织";
            this.colOperationOrg.Name = "colOperationOrg";
            this.colOperationOrg.Width = 120;
            // 
            // colProjectName
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.colProjectName.DefaultCellStyle = dataGridViewCellStyle4;
            this.colProjectName.FillWeight = 80F;
            this.colProjectName.HeaderText = "所属项目";
            this.colProjectName.Name = "colProjectName";
            this.colProjectName.ReadOnly = true;
            this.colProjectName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colProjectName.Width = 120;
            // 
            // VSeclectCommonPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 453);
            this.Name = "VSeclectCommonPerson";
            this.Text = "人员查询";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbAllSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgDetail;
        private System.Windows.Forms.Button btnQuery;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPersonOnJob;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationorg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Label lblSelectCount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPersonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPersonOnJob;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOperationOrg;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProjectName;


    }
}