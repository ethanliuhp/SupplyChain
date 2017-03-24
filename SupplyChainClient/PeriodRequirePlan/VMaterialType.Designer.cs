namespace Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan
{
    partial class VMaterialType
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
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtMaterial = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel2 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtMaterialSuffer = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel1 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtDiagramNum = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel3 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.txtDiagramNum);
            this.pnlFloor.Controls.Add(this.customLabel3);
            this.pnlFloor.Controls.Add(this.txtMaterialSuffer);
            this.pnlFloor.Controls.Add(this.customLabel1);
            this.pnlFloor.Controls.Add(this.btnCancel);
            this.pnlFloor.Controls.Add(this.btnOK);
            this.pnlFloor.Controls.Add(this.btnSelect);
            this.pnlFloor.Controls.Add(this.txtMaterial);
            this.pnlFloor.Controls.Add(this.customLabel2);
            this.pnlFloor.Size = new System.Drawing.Size(280, 158);
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelect.Location = new System.Drawing.Point(200, 25);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(59, 21);
            this.btnSelect.TabIndex = 162;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // txtMaterial
            // 
            this.txtMaterial.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterial.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMaterial.DrawSelf = false;
            this.txtMaterial.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMaterial.EnterToTab = false;
            this.txtMaterial.Location = new System.Drawing.Point(95, 27);
            this.txtMaterial.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMaterial.Name = "txtMaterial";
            this.txtMaterial.Padding = new System.Windows.Forms.Padding(1);
            this.txtMaterial.ReadOnly = false;
            this.txtMaterial.Size = new System.Drawing.Size(110, 16);
            this.txtMaterial.TabIndex = 161;
            // 
            // customLabel2
            // 
            this.customLabel2.AutoSize = true;
            this.customLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel2.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel2.Location = new System.Drawing.Point(24, 29);
            this.customLabel2.Name = "customLabel2";
            this.customLabel2.Size = new System.Drawing.Size(65, 12);
            this.customLabel2.TabIndex = 160;
            this.customLabel2.Text = "资源类型：";
            this.customLabel2.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(139, 123);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 23);
            this.btnCancel.TabIndex = 164;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(60, 123);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(55, 23);
            this.btnOK.TabIndex = 163;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // txtMaterialSuffer
            // 
            this.txtMaterialSuffer.BackColor = System.Drawing.SystemColors.Control;
            this.txtMaterialSuffer.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtMaterialSuffer.DrawSelf = false;
            this.txtMaterialSuffer.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtMaterialSuffer.EnterToTab = false;
            this.txtMaterialSuffer.Location = new System.Drawing.Point(95, 55);
            this.txtMaterialSuffer.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtMaterialSuffer.Name = "txtMaterialSuffer";
            this.txtMaterialSuffer.Padding = new System.Windows.Forms.Padding(1);
            this.txtMaterialSuffer.ReadOnly = false;
            this.txtMaterialSuffer.Size = new System.Drawing.Size(110, 16);
            this.txtMaterialSuffer.TabIndex = 166;
            // 
            // customLabel1
            // 
            this.customLabel1.AutoSize = true;
            this.customLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel1.Location = new System.Drawing.Point(24, 57);
            this.customLabel1.Name = "customLabel1";
            this.customLabel1.Size = new System.Drawing.Size(65, 12);
            this.customLabel1.TabIndex = 165;
            this.customLabel1.Text = "规格型号：";
            this.customLabel1.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtDiagramNum
            // 
            this.txtDiagramNum.BackColor = System.Drawing.SystemColors.Control;
            this.txtDiagramNum.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDiagramNum.DrawSelf = false;
            this.txtDiagramNum.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDiagramNum.EnterToTab = false;
            this.txtDiagramNum.Location = new System.Drawing.Point(95, 86);
            this.txtDiagramNum.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDiagramNum.Name = "txtDiagramNum";
            this.txtDiagramNum.Padding = new System.Windows.Forms.Padding(1);
            this.txtDiagramNum.ReadOnly = false;
            this.txtDiagramNum.Size = new System.Drawing.Size(110, 16);
            this.txtDiagramNum.TabIndex = 168;
            // 
            // customLabel3
            // 
            this.customLabel3.AutoSize = true;
            this.customLabel3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel3.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel3.Location = new System.Drawing.Point(48, 86);
            this.customLabel3.Name = "customLabel3";
            this.customLabel3.Size = new System.Drawing.Size(41, 12);
            this.customLabel3.TabIndex = 167;
            this.customLabel3.Text = "图号：";
            this.customLabel3.UnderLineColor = System.Drawing.Color.Red;
            // 
            // VMaterialType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 158);
            this.Name = "VMaterialType";
            this.Text = "资源类型过滤";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelect;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMaterial;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDiagramNum;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel3;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtMaterialSuffer;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel1;
    }
}