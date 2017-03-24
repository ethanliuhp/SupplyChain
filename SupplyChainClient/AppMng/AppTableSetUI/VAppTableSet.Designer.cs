namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppTableSetUI
{
    partial class VAppTableSet
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VAppTableSet));
            this.Dgv = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnDelete);
            this.pnlFloor.Controls.Add(this.btnSave);
            this.pnlFloor.Controls.Add(this.Dgv);
            this.pnlFloor.Size = new System.Drawing.Size(1216, 410);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.Dgv, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSave, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnDelete, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(127, 11);
            this.lblTitle.Size = new System.Drawing.Size(135, 20);
            this.lblTitle.Text = "审批表单定义";
            // 
            // Dgv
            // 
            this.Dgv.AllowAddNew = true;
            this.Dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Dgv.ColumnInfo = resources.GetString("Dgv.ColumnInfo");
            this.Dgv.Location = new System.Drawing.Point(12, 29);
            this.Dgv.Name = "Dgv";
            this.Dgv.Rows.Count = 1;
            this.Dgv.Rows.DefaultSize = 18;
            this.Dgv.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.Dgv.Size = new System.Drawing.Size(1187, 378);
            this.Dgv.TabIndex = 12;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(783, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 21;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(638, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // VAppTableSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1216, 410);
            this.Name = "VAppTableSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "审批表单定义";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid Dgv;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;

    }
}