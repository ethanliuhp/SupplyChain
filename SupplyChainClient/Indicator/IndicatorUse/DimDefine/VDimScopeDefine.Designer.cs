namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimDefine
{
    partial class VDimScopeDefine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VDimScopeDefine));
            this.dgvScopeDef = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.scopeType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.beginValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.score = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSubmit = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnDel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnClose = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScopeDef)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvScopeDef
            // 
            this.dgvScopeDef.AddDefaultMenu = false;
            this.dgvScopeDef.AddNoColumn = false;
            this.dgvScopeDef.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvScopeDef.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvScopeDef.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvScopeDef.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvScopeDef.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScopeDef.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.scopeType,
            this.beginValue,
            this.endValue,
            this.score});
            this.dgvScopeDef.CustomBackColor = false;
            this.dgvScopeDef.EditCellBackColor = System.Drawing.Color.White;
            this.dgvScopeDef.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextRow;
            this.dgvScopeDef.FreezeFirstRow = false;
            this.dgvScopeDef.FreezeLastRow = false;
            this.dgvScopeDef.FrontColumnCount = 0;
            this.dgvScopeDef.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvScopeDef.IsAllowOrder = true;
            this.dgvScopeDef.IsConfirmDelete = true;
            this.dgvScopeDef.Location = new System.Drawing.Point(39, 23);
            this.dgvScopeDef.Name = "dgvScopeDef";
            this.dgvScopeDef.PageIndex = 0;
            this.dgvScopeDef.PageSize = 0;
            this.dgvScopeDef.Query = null;
            this.dgvScopeDef.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvScopeDef.ReadOnlyCols")));
            this.dgvScopeDef.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvScopeDef.RowHeadersWidth = 22;
            this.dgvScopeDef.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvScopeDef.RowTemplate.Height = 23;
            this.dgvScopeDef.Size = new System.Drawing.Size(469, 303);
            this.dgvScopeDef.TabIndex = 0;
            this.dgvScopeDef.TargetType = null;
            // 
            // scopeType
            // 
            this.scopeType.HeaderText = "区间类型";
            this.scopeType.Name = "scopeType";
            this.scopeType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // beginValue
            // 
            this.beginValue.HeaderText = "开始值";
            this.beginValue.Name = "beginValue";
            // 
            // endValue
            // 
            this.endValue.HeaderText = "结束值";
            this.endValue.Name = "endValue";
            // 
            // score
            // 
            this.score.HeaderText = "分值";
            this.score.Name = "score";
            this.score.Width = 80;
            // 
            // btnSubmit
            // 
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSubmit.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSubmit.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSubmit.Location = new System.Drawing.Point(171, 348);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 27);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "确 定";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnDel
            // 
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnDel.Location = new System.Drawing.Point(67, 348);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 27);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "删 除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnClose.Location = new System.Drawing.Point(315, 348);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 27);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "取 消";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // VDimScopeDefine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 403);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvScopeDef);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnSubmit);
            this.Name = "VDimScopeDefine";
            this.Text = "区间定义界面";
            ((System.ComponentModel.ISupportInitialize)(this.dgvScopeDef)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvScopeDef;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSubmit;
        private System.Windows.Forms.DataGridViewComboBoxColumn scopeType;
        private System.Windows.Forms.DataGridViewTextBoxColumn beginValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn endValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn score;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnDel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnClose;
    }
}