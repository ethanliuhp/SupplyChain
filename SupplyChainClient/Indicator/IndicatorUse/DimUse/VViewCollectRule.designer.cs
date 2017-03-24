namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    partial class VViewCollectRule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VViewCollectRule));
            this.customGroupBox1 = new VirtualMachine.Component.WinControls.Controls.CustomGroupBox();
            this.btnSave = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.dgvData = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.btnClose = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.btnClose);
            this.customGroupBox1.Controls.Add(this.btnSave);
            this.customGroupBox1.Controls.Add(this.dgvData);
            this.customGroupBox1.Location = new System.Drawing.Point(18, 12);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(828, 514);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "数据区规则定义";
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSave.Location = new System.Drawing.Point(238, 482);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保　存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // dgvData
            // 
            this.dgvData.AddDefaultMenu = false;
            this.dgvData.AddNoColumn = false;
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvData.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgvData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.CustomBackColor = false;
            this.dgvData.EditCellBackColor = System.Drawing.Color.White;
            this.dgvData.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.NextRow;
            this.dgvData.FreezeFirstRow = false;
            this.dgvData.FreezeLastRow = false;
            this.dgvData.FrontColumnCount = 0;
            this.dgvData.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgvData.IsAllowOrder = true;
            this.dgvData.IsConfirmDelete = true;
            this.dgvData.Location = new System.Drawing.Point(6, 20);
            this.dgvData.Name = "dgvData";
            this.dgvData.PageIndex = 0;
            this.dgvData.PageSize = 0;
            this.dgvData.Query = null;
            this.dgvData.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgvData.ReadOnlyCols")));
            this.dgvData.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvData.RowHeadersWidth = 22;
            this.dgvData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(816, 456);
            this.dgvData.TabIndex = 0;
            this.dgvData.TargetType = null;
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnClose.Location = new System.Drawing.Point(403, 482);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关　闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // VViewCollectRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 529);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "VViewCollectRule";
            this.Text = "报表汇总规则定义";
            this.Load += new System.EventHandler(this.VViewCollectRule_Load);
            this.customGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomGroupBox customGroupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomDataGridView dgvData;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSave;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnClose;
    }
}