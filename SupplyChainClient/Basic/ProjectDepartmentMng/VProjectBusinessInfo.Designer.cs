namespace Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng
{
    partial class VProjectBusinessInfo
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel9 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnEnterMBP = new System.Windows.Forms.Button();
            this.dgDetail = new System.Windows.Forms.DataGridView();
            this.ColNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProjectCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProManage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProjectType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProStage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProjectCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColtheGroundArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBuildingHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColGroundLayers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnderLayers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbProjectStage = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbProjectType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cmbProjectCurState = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlFloor.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.tabControl1);
            this.pnlFloor.Size = new System.Drawing.Size(1007, 489);
            this.pnlFloor.Controls.SetChildIndex(this.tabControl1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(137, 7);
            this.lblTitle.Size = new System.Drawing.Size(114, 20);
            this.lblTitle.Text = "项目一览表";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1007, 489);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.cmbProjectCurState);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnOperationOrg);
            this.tabPage1.Controls.Add(this.txtOperationOrg);
            this.tabPage1.Controls.Add(this.customLabel9);
            this.tabPage1.Controls.Add(this.btnEnterMBP);
            this.tabPage1.Controls.Add(this.dgDetail);
            this.tabPage1.Controls.Add(this.btnSearch);
            this.tabPage1.Controls.Add(this.cbProjectStage);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtName);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.cbProjectType);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(999, 463);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "项目基本信息";
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(559, 16);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 163;
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
            this.txtOperationOrg.Location = new System.Drawing.Point(447, 19);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(106, 16);
            this.txtOperationOrg.TabIndex = 162;
            // 
            // customLabel9
            // 
            this.customLabel9.AutoSize = true;
            this.customLabel9.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel9.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel9.Location = new System.Drawing.Point(387, 22);
            this.customLabel9.Name = "customLabel9";
            this.customLabel9.Size = new System.Drawing.Size(59, 12);
            this.customLabel9.TabIndex = 161;
            this.customLabel9.Text = "归属组织:";
            this.customLabel9.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnEnterMBP
            // 
            this.btnEnterMBP.Location = new System.Drawing.Point(861, 14);
            this.btnEnterMBP.Name = "btnEnterMBP";
            this.btnEnterMBP.Size = new System.Drawing.Size(113, 23);
            this.btnEnterMBP.TabIndex = 21;
            this.btnEnterMBP.Text = "进入项目部";
            this.btnEnterMBP.UseVisualStyleBackColor = true;
            // 
            // dgDetail
            // 
            this.dgDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColNo,
            this.ColProjectCode,
            this.ColProjectName,
            this.ColProManage,
            this.ColProjectType,
            this.ColProStage,
            this.ColProjectCost,
            this.ColtheGroundArea,
            this.ColBuildingHeight,
            this.ColGroundLayers,
            this.colUnderLayers});
            this.dgDetail.Location = new System.Drawing.Point(6, 45);
            this.dgDetail.Name = "dgDetail";
            this.dgDetail.RowTemplate.Height = 23;
            this.dgDetail.Size = new System.Drawing.Size(985, 410);
            this.dgDetail.TabIndex = 20;
            // 
            // ColNo
            // 
            this.ColNo.HeaderText = "序号";
            this.ColNo.Name = "ColNo";
            this.ColNo.Width = 60;
            // 
            // ColProjectCode
            // 
            this.ColProjectCode.HeaderText = "编码";
            this.ColProjectCode.Name = "ColProjectCode";
            this.ColProjectCode.Width = 70;
            // 
            // ColProjectName
            // 
            this.ColProjectName.HeaderText = "工程名称";
            this.ColProjectName.Name = "ColProjectName";
            // 
            // ColProManage
            // 
            this.ColProManage.HeaderText = "项目经理";
            this.ColProManage.Name = "ColProManage";
            // 
            // ColProjectType
            // 
            this.ColProjectType.HeaderText = "项目类型";
            this.ColProjectType.Name = "ColProjectType";
            // 
            // ColProStage
            // 
            this.ColProStage.HeaderText = "施工阶段";
            this.ColProStage.Name = "ColProStage";
            // 
            // ColProjectCost
            // 
            this.ColProjectCost.HeaderText = "工程造价";
            this.ColProjectCost.Name = "ColProjectCost";
            // 
            // ColtheGroundArea
            // 
            this.ColtheGroundArea.HeaderText = "建筑面积";
            this.ColtheGroundArea.Name = "ColtheGroundArea";
            // 
            // ColBuildingHeight
            // 
            this.ColBuildingHeight.HeaderText = "建筑高度";
            this.ColBuildingHeight.Name = "ColBuildingHeight";
            // 
            // ColGroundLayers
            // 
            this.ColGroundLayers.HeaderText = "地上层数";
            this.ColGroundLayers.Name = "ColGroundLayers";
            // 
            // colUnderLayers
            // 
            this.colUnderLayers.HeaderText = "地下层数";
            this.colUnderLayers.Name = "colUnderLayers";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(780, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 19;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // cbProjectStage
            // 
            this.cbProjectStage.FormattingEnabled = true;
            this.cbProjectStage.Location = new System.Drawing.Point(491, 0);
            this.cbProjectStage.Name = "cbProjectStage";
            this.cbProjectStage.Size = new System.Drawing.Size(77, 20);
            this.cbProjectStage.TabIndex = 18;
            this.cbProjectStage.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(426, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "施工阶段：";
            this.label4.Visible = false;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(93, 16);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(121, 21);
            this.txtName.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "工程名称：";
            // 
            // cbProjectType
            // 
            this.cbProjectType.FormattingEnabled = true;
            this.cbProjectType.Items.AddRange(new object[] {
            "建筑工程",
            "安装工程",
            "市政桥梁",
            "公路工程",
            "装饰装修工程",
            "园林绿化工程",
            "大型土石方工程",
            "钢结构工程",
            "其它"});
            this.cbProjectType.Location = new System.Drawing.Point(290, 17);
            this.cbProjectType.Name = "cbProjectType";
            this.cbProjectType.Size = new System.Drawing.Size(89, 20);
            this.cbProjectType.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(229, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "项目类型:";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(381, 257);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "工程预警";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cmbProjectCurState
            // 
            this.cmbProjectCurState.FormattingEnabled = true;
            this.cmbProjectCurState.Location = new System.Drawing.Point(682, 17);
            this.cmbProjectCurState.Name = "cmbProjectCurState";
            this.cmbProjectCurState.Size = new System.Drawing.Size(77, 20);
            this.cmbProjectCurState.TabIndex = 165;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(617, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 164;
            this.label3.Text = "项目状态：";
            // 
            // VProjectBusinessInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 489);
            this.Name = "VProjectBusinessInfo";
            this.Text = "项目业务信息";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbProjectStage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbProjectType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgDetail;
        private System.Windows.Forms.Button btnEnterMBP;
        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel9;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProjectCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProManage;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProjectType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProStage;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProjectCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColtheGroundArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBuildingHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColGroundLayers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnderLayers;
        private System.Windows.Forms.ComboBox cmbProjectCurState;
        private System.Windows.Forms.Label label3;


    }
}