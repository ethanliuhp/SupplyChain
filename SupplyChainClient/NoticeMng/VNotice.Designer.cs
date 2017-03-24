namespace Application.Business.Erp.SupplyChain.Client.NoticeMng
{
    partial class VNotice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VNotice));
            this.lnkDelete = new System.Windows.Forms.LinkLabel();
            this.lnkEdit = new System.Windows.Forms.LinkLabel();
            this.lnkAdd = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.grdEdiDept = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grdEdiDept)).BeginInit();
            this.SuspendLayout();
            // 
            // lnkDelete
            // 
            this.lnkDelete.AutoSize = true;
            this.lnkDelete.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lnkDelete.LinkColor = System.Drawing.Color.Blue;
            this.lnkDelete.Location = new System.Drawing.Point(110, 52);
            this.lnkDelete.Name = "lnkDelete";
            this.lnkDelete.Size = new System.Drawing.Size(29, 12);
            this.lnkDelete.TabIndex = 3;
            this.lnkDelete.TabStop = true;
            this.lnkDelete.Text = "删除";
            // 
            // lnkEdit
            // 
            this.lnkEdit.AutoSize = true;
            this.lnkEdit.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lnkEdit.LinkArea = new System.Windows.Forms.LinkArea(0, 2);
            this.lnkEdit.LinkColor = System.Drawing.Color.Blue;
            this.lnkEdit.Location = new System.Drawing.Point(72, 52);
            this.lnkEdit.Name = "lnkEdit";
            this.lnkEdit.Size = new System.Drawing.Size(29, 12);
            this.lnkEdit.TabIndex = 2;
            this.lnkEdit.TabStop = true;
            this.lnkEdit.Text = "编辑";
            // 
            // lnkAdd
            // 
            this.lnkAdd.AutoSize = true;
            this.lnkAdd.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lnkAdd.LinkColor = System.Drawing.Color.Blue;
            this.lnkAdd.Location = new System.Drawing.Point(34, 52);
            this.lnkAdd.Name = "lnkAdd";
            this.lnkAdd.Size = new System.Drawing.Size(29, 12);
            this.lnkAdd.TabIndex = 1;
            this.lnkAdd.TabStop = true;
            this.lnkAdd.Text = "增加";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("KaiTi_GB2312", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.Location = new System.Drawing.Point(229, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 19);
            this.label4.TabIndex = 1;
            this.label4.Text = "系统公告平台";
            // 
            // grdEdiDept
            // 
            this.grdEdiDept.AllowDelete = true;
            this.grdEdiDept.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.Both;
            this.grdEdiDept.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdEdiDept.AutoClipboard = true;
            this.grdEdiDept.ColumnInfo = resources.GetString("grdEdiDept.ColumnInfo");
            this.grdEdiDept.ExtendLastCol = true;
            this.grdEdiDept.Location = new System.Drawing.Point(15, 80);
            this.grdEdiDept.Name = "grdEdiDept";
            this.grdEdiDept.Rows.Count = 1;
            this.grdEdiDept.Rows.DefaultSize = 18;
            this.grdEdiDept.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.grdEdiDept.Size = new System.Drawing.Size(998, 546);
            this.grdEdiDept.StyleInfo = resources.GetString("grdEdiDept.StyleInfo");
            this.grdEdiDept.TabIndex = 6;
            // 
            // VNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 638);
            this.Controls.Add(this.grdEdiDept);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lnkDelete);
            this.Controls.Add(this.lnkEdit);
            this.Controls.Add(this.lnkAdd);
            this.Name = "VNotice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统公告信息维护";
            ((System.ComponentModel.ISupportInitialize)(this.grdEdiDept)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkDelete;
        private System.Windows.Forms.LinkLabel lnkEdit;
        private System.Windows.Forms.LinkLabel lnkAdd;
        private System.Windows.Forms.Label label4;
        private C1.Win.C1FlexGrid.C1FlexGrid grdEdiDept;
    }
}