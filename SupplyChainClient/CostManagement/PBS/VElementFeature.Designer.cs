namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    partial class VElementFeature
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VElementFeature));
            this.dgElementFeature = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.FeatureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureLable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureDes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureValueFormat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtFSet = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel4 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtFName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtFUnit = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtFLable = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtFValue = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtFValueFormat = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel6 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtFDes = new System.Windows.Forms.TextBox();
            this.btnUnit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbFeature = new System.Windows.Forms.GroupBox();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgElementFeature)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.gbFeature.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.gbFeature);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(681, 407);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.gbFeature, 0);
            // 
            // dgElementFeature
            // 
            this.dgElementFeature.AddDefaultMenu = false;
            this.dgElementFeature.AddNoColumn = true;
            this.dgElementFeature.AllowUserToAddRows = false;
            this.dgElementFeature.AllowUserToDeleteRows = false;
            this.dgElementFeature.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgElementFeature.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgElementFeature.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgElementFeature.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgElementFeature.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgElementFeature.ColumnHeadersHeight = 24;
            this.dgElementFeature.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgElementFeature.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FeatureName,
            this.FeatureSet,
            this.FeatureUnit,
            this.FeatureLable,
            this.FeatureDes,
            this.FeatureValue,
            this.FeatureValueFormat});
            this.dgElementFeature.CustomBackColor = false;
            this.dgElementFeature.EditCellBackColor = System.Drawing.Color.White;
            this.dgElementFeature.EnableHeadersVisualStyles = false;
            this.dgElementFeature.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgElementFeature.FreezeFirstRow = false;
            this.dgElementFeature.FreezeLastRow = false;
            this.dgElementFeature.FrontColumnCount = 0;
            this.dgElementFeature.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgElementFeature.HScrollOffset = 0;
            this.dgElementFeature.IsAllowOrder = true;
            this.dgElementFeature.IsConfirmDelete = true;
            this.dgElementFeature.Location = new System.Drawing.Point(6, 20);
            this.dgElementFeature.MultiSelect = false;
            this.dgElementFeature.Name = "dgElementFeature";
            this.dgElementFeature.PageIndex = 0;
            this.dgElementFeature.PageSize = 0;
            this.dgElementFeature.Query = null;
            this.dgElementFeature.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgElementFeature.ReadOnlyCols")));
            this.dgElementFeature.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgElementFeature.RowHeadersVisible = false;
            this.dgElementFeature.RowHeadersWidth = 22;
            this.dgElementFeature.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgElementFeature.RowTemplate.Height = 23;
            this.dgElementFeature.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgElementFeature.Size = new System.Drawing.Size(663, 159);
            this.dgElementFeature.TabIndex = 13;
            this.dgElementFeature.TargetType = null;
            this.dgElementFeature.VScrollOffset = 0;
            // 
            // FeatureName
            // 
            this.FeatureName.HeaderText = "IFC特性名";
            this.FeatureName.Name = "FeatureName";
            // 
            // FeatureSet
            // 
            this.FeatureSet.HeaderText = "IFC特性集";
            this.FeatureSet.Name = "FeatureSet";
            // 
            // FeatureUnit
            // 
            this.FeatureUnit.HeaderText = "单位";
            this.FeatureUnit.Name = "FeatureUnit";
            this.FeatureUnit.Width = 60;
            // 
            // FeatureLable
            // 
            this.FeatureLable.HeaderText = "特性标签";
            this.FeatureLable.Name = "FeatureLable";
            // 
            // FeatureDes
            // 
            this.FeatureDes.HeaderText = "特性描述";
            this.FeatureDes.Name = "FeatureDes";
            // 
            // FeatureValue
            // 
            this.FeatureValue.HeaderText = "特性值";
            this.FeatureValue.Name = "FeatureValue";
            // 
            // FeatureValueFormat
            // 
            this.FeatureValueFormat.HeaderText = "特性值格式";
            this.FeatureValueFormat.Name = "FeatureValueFormat";
            // 
            // txtFSet
            // 
            this.txtFSet.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtFSet.DrawSelf = false;
            this.txtFSet.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtFSet.EnterToTab = false;
            this.txtFSet.Location = new System.Drawing.Point(457, 22);
            this.txtFSet.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtFSet.Name = "txtFSet";
            this.txtFSet.Padding = new System.Windows.Forms.Padding(1);
            this.txtFSet.ReadOnly = false;
            this.txtFSet.Size = new System.Drawing.Size(200, 16);
            this.txtFSet.TabIndex = 18;
            // 
            // customLabel4
            // 
            this.customLabel4.AddColonAuto = true;
            this.customLabel4.AutoSize = true;
            this.customLabel4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel4.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel4.Location = new System.Drawing.Point(388, 24);
            this.customLabel4.Name = "customLabel4";
            this.customLabel4.Size = new System.Drawing.Size(71, 12);
            this.customLabel4.TabIndex = 19;
            this.customLabel4.Text = "IFC特性集：";
            this.customLabel4.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtFName
            // 
            this.txtFName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtFName.DrawSelf = false;
            this.txtFName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtFName.EnterToTab = false;
            this.txtFName.Location = new System.Drawing.Point(77, 20);
            this.txtFName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtFName.Name = "txtFName";
            this.txtFName.Padding = new System.Windows.Forms.Padding(1);
            this.txtFName.ReadOnly = false;
            this.txtFName.Size = new System.Drawing.Size(120, 16);
            this.txtFName.TabIndex = 20;
            // 
            // customLabel1
            // 
            this.customLabel1.AddColonAuto = true;
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(6, 22);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(71, 12);
            this.customLabel1.TabIndex = 21;
            this.customLabel1.Text = "IFC特性名：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtFUnit
            // 
            this.txtFUnit.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtFUnit.DrawSelf = false;
            this.txtFUnit.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtFUnit.EnterToTab = false;
            this.txtFUnit.Location = new System.Drawing.Point(254, 22);
            this.txtFUnit.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtFUnit.Name = "txtFUnit";
            this.txtFUnit.Padding = new System.Windows.Forms.Padding(1);
            this.txtFUnit.ReadOnly = true;
            this.txtFUnit.Size = new System.Drawing.Size(80, 16);
            this.txtFUnit.TabIndex = 22;
            // 
            // customLabel2
            // 
            this.customLabel2.AddColonAuto = true;
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(213, 24);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(41, 12);
            this.customLabel2.TabIndex = 23;
            this.customLabel2.Text = "单位：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtFLable
            // 
            this.txtFLable.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtFLable.DrawSelf = false;
            this.txtFLable.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtFLable.EnterToTab = false;
            this.txtFLable.Location = new System.Drawing.Point(77, 48);
            this.txtFLable.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtFLable.Name = "txtFLable";
            this.txtFLable.Padding = new System.Windows.Forms.Padding(1);
            this.txtFLable.ReadOnly = false;
            this.txtFLable.Size = new System.Drawing.Size(120, 16);
            this.txtFLable.TabIndex = 22;
            // 
            // customLabel3
            // 
            this.customLabel3.AddColonAuto = true;
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(12, 50);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(65, 12);
            this.customLabel3.TabIndex = 23;
            this.customLabel3.Text = "特性标签：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtFValue
            // 
            this.txtFValue.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtFValue.DrawSelf = false;
            this.txtFValue.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtFValue.EnterToTab = false;
            this.txtFValue.Location = new System.Drawing.Point(254, 48);
            this.txtFValue.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtFValue.Name = "txtFValue";
            this.txtFValue.Padding = new System.Windows.Forms.Padding(1);
            this.txtFValue.ReadOnly = false;
            this.txtFValue.Size = new System.Drawing.Size(120, 16);
            this.txtFValue.TabIndex = 26;
            // 
            // customLabel5
            // 
            this.customLabel5.AddColonAuto = true;
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(201, 50);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(53, 12);
            this.customLabel5.TabIndex = 27;
            this.customLabel5.Text = "特性值：";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtFValueFormat
            // 
            this.txtFValueFormat.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtFValueFormat.DrawSelf = false;
            this.txtFValueFormat.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtFValueFormat.EnterToTab = false;
            this.txtFValueFormat.Location = new System.Drawing.Point(457, 48);
            this.txtFValueFormat.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtFValueFormat.Name = "txtFValueFormat";
            this.txtFValueFormat.Padding = new System.Windows.Forms.Padding(1);
            this.txtFValueFormat.ReadOnly = false;
            this.txtFValueFormat.Size = new System.Drawing.Size(200, 16);
            this.txtFValueFormat.TabIndex = 28;
            // 
            // customLabel6
            // 
            this.customLabel6.AddColonAuto = true;
            this.customLabel6.AutoSize = true;
            this.customLabel6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel6.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel6.Location = new System.Drawing.Point(382, 50);
            this.customLabel6.Name = "customLabel6";
            this.customLabel6.Size = new System.Drawing.Size(77, 12);
            this.customLabel6.TabIndex = 29;
            this.customLabel6.Text = "特性值格式：";
            this.customLabel6.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel7
            // 
            this.customLabel7.AddColonAuto = true;
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(12, 77);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(65, 12);
            this.customLabel7.TabIndex = 31;
            this.customLabel7.Text = "特性描述：";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtFDes
            // 
            this.txtFDes.Location = new System.Drawing.Point(77, 74);
            this.txtFDes.Multiline = true;
            this.txtFDes.Name = "txtFDes";
            this.txtFDes.Size = new System.Drawing.Size(580, 38);
            this.txtFDes.TabIndex = 32;
            // 
            // btnUnit
            // 
            this.btnUnit.Location = new System.Drawing.Point(337, 19);
            this.btnUnit.Name = "btnUnit";
            this.btnUnit.Size = new System.Drawing.Size(37, 23);
            this.btnUnit.TabIndex = 33;
            this.btnUnit.Text = "选择";
            this.btnUnit.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(9, 185);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 34;
            this.btnAdd.Text = "新增";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdate.Location = new System.Drawing.Point(90, 185);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 35;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(171, 185);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 36;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(582, 118);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 37;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(501, 118);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dgElementFeature);
            this.groupBox1.Controls.Add(this.btnUpdate);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Location = new System.Drawing.Point(3, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(675, 214);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "元素特性列表";
            // 
            // gbFeature
            // 
            this.gbFeature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFeature.Controls.Add(this.txtFName);
            this.gbFeature.Controls.Add(this.customLabel4);
            this.gbFeature.Controls.Add(this.btnCancel);
            this.gbFeature.Controls.Add(this.btnSave);
            this.gbFeature.Controls.Add(this.txtFSet);
            this.gbFeature.Controls.Add(this.customLabel1);
            this.gbFeature.Controls.Add(this.customLabel2);
            this.gbFeature.Controls.Add(this.txtFUnit);
            this.gbFeature.Controls.Add(this.customLabel3);
            this.gbFeature.Controls.Add(this.btnUnit);
            this.gbFeature.Controls.Add(this.txtFLable);
            this.gbFeature.Controls.Add(this.txtFDes);
            this.gbFeature.Controls.Add(this.customLabel5);
            this.gbFeature.Controls.Add(this.customLabel7);
            this.gbFeature.Controls.Add(this.txtFValue);
            this.gbFeature.Controls.Add(this.txtFValueFormat);
            this.gbFeature.Controls.Add(this.customLabel6);
            this.gbFeature.Location = new System.Drawing.Point(3, 248);
            this.gbFeature.Name = "gbFeature";
            this.gbFeature.Size = new System.Drawing.Size(674, 156);
            this.gbFeature.TabIndex = 40;
            this.gbFeature.TabStop = false;
            this.gbFeature.Text = "元素特性信息";
            // 
            // VElementFeature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 407);
            this.Name = "VElementFeature";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "元素特性";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgElementFeature)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.gbFeature.ResumeLayout(false);
            this.gbFeature.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgElementFeature;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureLable;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureDes;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureValueFormat;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtFValueFormat;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtFValue;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtFLable;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtFUnit;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtFName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtFSet;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel4;
        private System.Windows.Forms.TextBox txtFDes;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUnit;
        private System.Windows.Forms.GroupBox gbFeature;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}