namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    partial class VTimeSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VTimeSelect));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.dgvTime = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTime)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(137, 273);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOk.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOk.Location = new System.Drawing.Point(33, 273);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(84, 23);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "确定(&K)";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // dgvTime
            // 
            this.dgvTime.AddDefaultMenu = false;
            this.dgvTime.AddNoColumn = false;
            this.dgvTime.AllowUserToAddRows = false;
            this.dgvTime.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTime.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvTime.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTime.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTime.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selected,
            this.name});
            this.dgvTime.CustomBackColor = false;
            this.dgvTime.EditCellBackColor = System.Drawing.Color.White;
            this.dgvTime.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextRow;
            this.dgvTime.FreezeFirstRow = false;
            this.dgvTime.FreezeLastRow = false;
            this.dgvTime.FrontColumnCount = 0;
            this.dgvTime.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvTime.IsAllowOrder = true;
            this.dgvTime.IsConfirmDelete = true;
            this.dgvTime.Location = new System.Drawing.Point(22, 21);
            this.dgvTime.Name = "dgvTime";
            this.dgvTime.PageIndex = 0;
            this.dgvTime.PageSize = 0;
            this.dgvTime.Query = null;
            this.dgvTime.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvTime.ReadOnlyCols")));
            this.dgvTime.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTime.RowHeadersWidth = 22;
            this.dgvTime.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTime.RowTemplate.Height = 23;
            this.dgvTime.Size = new System.Drawing.Size(270, 246);
            this.dgvTime.TabIndex = 22;
            this.dgvTime.TargetType = null;
            // 
            // selected
            // 
            this.selected.HeaderText = "选择";
            this.selected.Name = "selected";
            this.selected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.selected.Width = 60;
            // 
            // name
            // 
            this.name.HeaderText = "时间跨度名";
            this.name.Name = "name";
            this.name.Width = 160;
            // 
            // VTimeSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 301);
            this.Controls.Add(this.dgvTime);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "VTimeSelect";
            this.Text = "时间跨度选择";
            this.Load += new System.EventHandler(this.VTimeSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
    }
}