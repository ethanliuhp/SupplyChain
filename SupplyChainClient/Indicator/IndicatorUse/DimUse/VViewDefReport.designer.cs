namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse
{
    partial class VViewDefReport
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
            this.btnDel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnAdd = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.gbxViewDefine = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.btnStyle = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.cboType = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.lblType = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.btnModify = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSave = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtViewName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblViewName = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.gbxViewSelect = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.lstViewSel = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.cboCube = new VirtualMachine.Component.WinControls.Controls.CustomComboBox();
            this.pnlFloor.SuspendLayout();
            this.gbxViewDefine.SuspendLayout();
            this.gbxViewSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.gbxViewDefine);
            this.pnlFloor.Controls.Add(this.gbxViewSelect);
            this.pnlFloor.Size = new System.Drawing.Size(805, 451);
            // 
            // btnDel
            // 
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnDel.Location = new System.Drawing.Point(210, 331);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 7;
            this.btnDel.Text = "删 除";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnAdd.Location = new System.Drawing.Point(20, 331);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "新 增";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // gbxViewDefine
            // 
            this.gbxViewDefine.Controls.Add(this.btnStyle);
            this.gbxViewDefine.Controls.Add(this.cboType);
            this.gbxViewDefine.Controls.Add(this.lblType);
            this.gbxViewDefine.Controls.Add(this.btnModify);
            this.gbxViewDefine.Controls.Add(this.btnDel);
            this.gbxViewDefine.Controls.Add(this.btnSave);
            this.gbxViewDefine.Controls.Add(this.btnAdd);
            this.gbxViewDefine.Controls.Add(this.txtViewName);
            this.gbxViewDefine.Controls.Add(this.lblViewName);
            this.gbxViewDefine.Location = new System.Drawing.Point(304, 12);
            this.gbxViewDefine.Name = "gbxViewDefine";
            this.gbxViewDefine.Size = new System.Drawing.Size(493, 427);
            this.gbxViewDefine.TabIndex = 5;
            this.gbxViewDefine.TabStop = false;
            this.gbxViewDefine.Text = ">>>定义报表属性";
            // 
            // btnStyle
            // 
            this.btnStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStyle.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStyle.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnStyle.Location = new System.Drawing.Point(398, 331);
            this.btnStyle.Name = "btnStyle";
            this.btnStyle.Size = new System.Drawing.Size(75, 23);
            this.btnStyle.TabIndex = 14;
            this.btnStyle.Text = "格式定义";
            this.btnStyle.UseVisualStyleBackColor = true;
            // 
            // cboType
            // 
            this.cboType.BackColor = System.Drawing.SystemColors.Control;
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(129, 199);
            this.cboType.Name = "cboType";
            this.cboType.ReadOnly = true;
            this.cboType.Size = new System.Drawing.Size(92, 20);
            this.cboType.TabIndex = 13;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblType.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblType.Location = new System.Drawing.Point(43, 207);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(65, 12);
            this.lblType.TabIndex = 12;
            this.lblType.Text = "模板类型：";
            this.lblType.UnderLineColor = System.Drawing.Color.Red;
            // 
            // btnModify
            // 
            this.btnModify.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnModify.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnModify.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnModify.Location = new System.Drawing.Point(117, 331);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 8;
            this.btnModify.Text = "修 改";
            this.btnModify.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSave.Location = new System.Drawing.Point(306, 331);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保 存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // txtViewName
            // 
            this.txtViewName.BackColor = System.Drawing.SystemColors.Control;
            this.txtViewName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtViewName.DrawSelf = false;
            this.txtViewName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtViewName.EnterToTab = false;
            this.txtViewName.Location = new System.Drawing.Point(119, 136);
            this.txtViewName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtViewName.Name = "txtViewName";
            this.txtViewName.Padding = new System.Windows.Forms.Padding(1);
            this.txtViewName.ReadOnly = false;
            this.txtViewName.Size = new System.Drawing.Size(330, 16);
            this.txtViewName.TabIndex = 1;
            // 
            // lblViewName
            // 
            this.lblViewName.AutoSize = true;
            this.lblViewName.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblViewName.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblViewName.Location = new System.Drawing.Point(43, 140);
            this.lblViewName.Name = "lblViewName";
            this.lblViewName.Size = new System.Drawing.Size(65, 12);
            this.lblViewName.TabIndex = 0;
            this.lblViewName.Text = "模板名称：";
            this.lblViewName.UnderLineColor = System.Drawing.Color.Red;
            // 
            // gbxViewSelect
            // 
            this.gbxViewSelect.Controls.Add(this.lstViewSel);
            this.gbxViewSelect.Controls.Add(this.cboCube);
            this.gbxViewSelect.Location = new System.Drawing.Point(12, 12);
            this.gbxViewSelect.Name = "gbxViewSelect";
            this.gbxViewSelect.Size = new System.Drawing.Size(274, 427);
            this.gbxViewSelect.TabIndex = 4;
            this.gbxViewSelect.TabStop = false;
            this.gbxViewSelect.Text = ">>>选择模板";
            // 
            // lstViewSel
            // 
            this.lstViewSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstViewSel.FormattingEnabled = true;
            this.lstViewSel.HorizontalScrollbar = true;
            this.lstViewSel.ItemHeight = 12;
            this.lstViewSel.Location = new System.Drawing.Point(17, 72);
            this.lstViewSel.Name = "lstViewSel";
            this.lstViewSel.Size = new System.Drawing.Size(239, 338);
            this.lstViewSel.TabIndex = 1;
            // 
            // cboCube
            // 
            this.cboCube.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboCube.FormattingEnabled = true;
            this.cboCube.Location = new System.Drawing.Point(17, 31);
            this.cboCube.Name = "cboCube";
            this.cboCube.Size = new System.Drawing.Size(239, 20);
            this.cboCube.TabIndex = 0;
            // 
            // VViewDefReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 451);
            this.Name = "VViewDefReport";
            this.Text = "统计报表定义";
            this.pnlFloor.ResumeLayout(false);
            this.gbxViewDefine.ResumeLayout(false);
            this.gbxViewDefine.PerformLayout();
            this.gbxViewSelect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomButton btnDel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnAdd;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gbxViewDefine;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSave;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtViewName;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblViewName;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gbxViewSelect;
        private Application.Business.Erp.ClientSystem.Template.CustomListBox lstViewSel;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboCube;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnModify;
        private VirtualMachine.Component.WinControls.Controls.CustomComboBox cboType;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblType;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnStyle;

    }
}