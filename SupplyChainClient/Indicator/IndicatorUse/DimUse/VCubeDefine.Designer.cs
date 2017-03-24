namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    partial class VCubeDefine
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
            this.lblCubeName = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.edtCubeName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.gbxCubeTip = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.btnDel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnAdd = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.lstTarDim = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.lstSourDim = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.btnSubmit = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customGroupBox1 = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.lstCubeSel = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.btnCreate = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnRemove = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnFactDefine = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.gbxCubeTip.SuspendLayout();
            this.customGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnFactDefine);
            this.pnlFloor.Controls.Add(this.btnRemove);
            this.pnlFloor.Controls.Add(this.btnCreate);
            this.pnlFloor.Controls.Add(this.customGroupBox1);
            this.pnlFloor.Controls.Add(this.btnSubmit);
            this.pnlFloor.Controls.Add(this.lblCubeName);
            this.pnlFloor.Controls.Add(this.gbxCubeTip);
            this.pnlFloor.Controls.Add(this.edtCubeName);
            this.pnlFloor.Dock = System.Windows.Forms.DockStyle.None;
            this.pnlFloor.Location = new System.Drawing.Point(2, 3);
            this.pnlFloor.Size = new System.Drawing.Size(762, 466);
            this.pnlFloor.Controls.SetChildIndex(this.edtCubeName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.gbxCubeTip, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblCubeName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSubmit, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customGroupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCreate, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnRemove, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnFactDefine, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(355, 20);
            this.lblTitle.Visible = false;
            // 
            // lblCubeName
            // 
            this.lblCubeName.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCubeName.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblCubeName.Location = new System.Drawing.Point(244, 33);
            this.lblCubeName.Name = "lblCubeName";
            this.lblCubeName.Size = new System.Drawing.Size(102, 23);
            this.lblCubeName.TabIndex = 0;
            this.lblCubeName.Text = "主题名称：";
            this.lblCubeName.UnderLineColor = System.Drawing.Color.Red;
            // 
            // edtCubeName
            // 
            this.edtCubeName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.edtCubeName.DrawSelf = false;
            this.edtCubeName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.edtCubeName.EnterToTab = false;
            this.edtCubeName.Location = new System.Drawing.Point(367, 28);
            this.edtCubeName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.edtCubeName.Name = "edtCubeName";
            this.edtCubeName.Padding = new System.Windows.Forms.Padding(1);
            this.edtCubeName.ReadOnly = false;
            this.edtCubeName.Size = new System.Drawing.Size(250, 16);
            this.edtCubeName.TabIndex = 1;
            // 
            // gbxCubeTip
            // 
            this.gbxCubeTip.Controls.Add(this.btnDel);
            this.gbxCubeTip.Controls.Add(this.btnAdd);
            this.gbxCubeTip.Controls.Add(this.lstTarDim);
            this.gbxCubeTip.Controls.Add(this.lstSourDim);
            this.gbxCubeTip.Location = new System.Drawing.Point(246, 85);
            this.gbxCubeTip.Name = "gbxCubeTip";
            this.gbxCubeTip.Size = new System.Drawing.Size(463, 290);
            this.gbxCubeTip.TabIndex = 3;
            this.gbxCubeTip.TabStop = false;
            this.gbxCubeTip.Text = ">>>请选择主题的属性";
            // 
            // btnDel
            // 
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnDel.Location = new System.Drawing.Point(188, 152);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 28);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "<< 移除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnAdd.Location = new System.Drawing.Point(188, 82);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 28);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加 >>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lstTarDim
            // 
            this.lstTarDim.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstTarDim.FormattingEnabled = true;
            this.lstTarDim.ItemHeight = 12;
            this.lstTarDim.Location = new System.Drawing.Point(286, 50);
            this.lstTarDim.Name = "lstTarDim";
            this.lstTarDim.Size = new System.Drawing.Size(135, 182);
            this.lstTarDim.TabIndex = 1;
            // 
            // lstSourDim
            // 
            this.lstSourDim.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSourDim.FormattingEnabled = true;
            this.lstSourDim.ItemHeight = 12;
            this.lstSourDim.Location = new System.Drawing.Point(31, 50);
            this.lstSourDim.Name = "lstSourDim";
            this.lstSourDim.Size = new System.Drawing.Size(135, 182);
            this.lstSourDim.TabIndex = 0;
            // 
            // btnSubmit
            // 
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSubmit.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSubmit.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSubmit.Location = new System.Drawing.Point(434, 394);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 28);
            this.btnSubmit.TabIndex = 4;
            this.btnSubmit.Text = "确　定";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.lstCubeSel);
            this.customGroupBox1.Location = new System.Drawing.Point(24, 33);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(199, 342);
            this.customGroupBox1.TabIndex = 6;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = ">>>主题选择";
            // 
            // lstCubeSel
            // 
            this.lstCubeSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstCubeSel.FormattingEnabled = true;
            this.lstCubeSel.ItemHeight = 12;
            this.lstCubeSel.Location = new System.Drawing.Point(17, 20);
            this.lstCubeSel.Name = "lstCubeSel";
            this.lstCubeSel.ScrollAlwaysVisible = true;
            this.lstCubeSel.Size = new System.Drawing.Size(165, 302);
            this.lstCubeSel.TabIndex = 0;
            this.lstCubeSel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstCubeSel_MouseClick);
            // 
            // btnCreate
            // 
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCreate.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCreate.Location = new System.Drawing.Point(24, 397);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 28);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "新 增";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRemove.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRemove.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnRemove.Location = new System.Drawing.Point(118, 397);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 28);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "删　除";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnFactDefine
            // 
            this.btnFactDefine.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFactDefine.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFactDefine.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnFactDefine.Location = new System.Drawing.Point(515, 394);
            this.btnFactDefine.Name = "btnFactDefine";
            this.btnFactDefine.Size = new System.Drawing.Size(75, 28);
            this.btnFactDefine.TabIndex = 9;
            this.btnFactDefine.Text = "事实定义";
            this.btnFactDefine.UseVisualStyleBackColor = true;
            // 
            // VCubeDefine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 466);
            this.Enabled = false;
            this.Name = "VCubeDefine";
            this.Text = "主题定义和维护界面";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.gbxCubeTip.ResumeLayout(false);
            this.customGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblCubeName;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit edtCubeName;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox gbxCubeTip;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSubmit;
        private Application.Business.Erp.ClientSystem.Template.CustomListBox lstSourDim;
        private Application.Business.Erp.ClientSystem.Template.CustomListBox lstTarDim;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnAdd;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnDel;
        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox customGroupBox1;
        private Application.Business.Erp.ClientSystem.Template.CustomListBox lstCubeSel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnRemove;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCreate;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnFactDefine;
    }
}