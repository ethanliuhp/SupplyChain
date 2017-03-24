namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    partial class VFactDefine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VFactDefine));
            this.dgFactDefine = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.btnAdd = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnSave = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnModify = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.colFactname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgFactDefine)).BeginInit();
            this.SuspendLayout();
            // 
            // dgFactDefine
            // 
            this.dgFactDefine.AddDefaultMenu = false;
            this.dgFactDefine.AddNoColumn = true;
            this.dgFactDefine.AllowUserToAddRows = false;
            this.dgFactDefine.AllowUserToDeleteRows = false;
            this.dgFactDefine.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgFactDefine.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgFactDefine.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgFactDefine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFactDefine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFactname,
            this.colUnit,
            this.colState});
            this.dgFactDefine.CustomBackColor = false;
            this.dgFactDefine.EditCellBackColor = System.Drawing.Color.White;
            this.dgFactDefine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgFactDefine.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextColumn;
            this.dgFactDefine.FreezeFirstRow = false;
            this.dgFactDefine.FreezeLastRow = false;
            this.dgFactDefine.FrontColumnCount = 0;
            this.dgFactDefine.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgFactDefine.IsAllowOrder = true;
            this.dgFactDefine.IsConfirmDelete = true;
            this.dgFactDefine.Location = new System.Drawing.Point(12, 12);
            this.dgFactDefine.Name = "dgFactDefine";
            this.dgFactDefine.PageIndex = 0;
            this.dgFactDefine.PageSize = 0;
            this.dgFactDefine.Query = null;
            this.dgFactDefine.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgFactDefine.ReadOnlyCols")));
            this.dgFactDefine.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgFactDefine.RowHeadersWidth = 22;
            this.dgFactDefine.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgFactDefine.RowTemplate.Height = 23;
            this.dgFactDefine.Size = new System.Drawing.Size(465, 248);
            this.dgFactDefine.TabIndex = 0;
            this.dgFactDefine.TargetType = null;
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnAdd.Location = new System.Drawing.Point(160, 266);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 25);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "增加";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSave.Location = new System.Drawing.Point(322, 266);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 26);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnModify
            // 
            this.btnModify.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnModify.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnModify.Location = new System.Drawing.Point(241, 266);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 25);
            this.btnModify.TabIndex = 3;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            // 
            // colFactname
            // 
            this.colFactname.HeaderText = "事实名称";
            this.colFactname.Name = "colFactname";
            // 
            // colUnit
            // 
            this.colUnit.HeaderText = "计量单位";
            this.colUnit.Name = "colUnit";
            this.colUnit.ReadOnly = true;
            // 
            // colState
            // 
            this.colState.HeaderText = "状态";
            this.colState.Name = "colState";
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnClose.Location = new System.Drawing.Point(402, 266);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 26);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "退出";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // VFactDefine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 301);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgFactDefine);
            this.MaximizeBox = false;
            this.Name = "VFactDefine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "事实定义";
            ((System.ComponentModel.ISupportInitialize)(this.dgFactDefine)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgFactDefine;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnAdd;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSave;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnModify;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFactname;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colState;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnClose;
    }
}