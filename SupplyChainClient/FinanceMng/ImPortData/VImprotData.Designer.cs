namespace Application.Business.Erp.SupplyChain.Client.FinanceMng
{
    partial class VImprotData
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
            this._tab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._flex = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnExcel = new System.Windows.Forms.Button();
            this._book = new C1.C1Excel.C1XLBook();
            this.btnOK = new System.Windows.Forms.Button();
            this._tab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._flex)).BeginInit();
            this.SuspendLayout();
            // 
            // _tab
            // 
            this._tab.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this._tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tab.Controls.Add(this.tabPage1);
            this._tab.Location = new System.Drawing.Point(0, 51);
            this._tab.Multiline = true;
            this._tab.Name = "_tab";
            this._tab.Padding = new System.Drawing.Point(0, 0);
            this._tab.SelectedIndex = 0;
            this._tab.Size = new System.Drawing.Size(830, 437);
            this._tab.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._flex);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(822, 412);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Sheet 1";
            // 
            // _flex
            // 
            this._flex.AllowAddNew = true;
            this._flex.AllowDelete = true;
            this._flex.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Spill;
            this._flex.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.MultiColumn;
            this._flex.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this._flex.ColumnInfo = "10,1,0,0,0,85,Columns:0{Width:22;}\t";
            this._flex.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flex.Location = new System.Drawing.Point(0, 0);
            this._flex.Name = "_flex";
            this._flex.Rows.DefaultSize = 17;
            this._flex.Size = new System.Drawing.Size(822, 412);
            this._flex.TabIndex = 0;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(23, 17);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(79, 28);
            this.btnExcel.TabIndex = 1;
            this.btnExcel.Text = "选择文件";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(724, 21);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(74, 28);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "导入";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // VImprotData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 488);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this._tab);
            this.Name = "VImprotData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据导入";
            this._tab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._flex)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl _tab;
        private System.Windows.Forms.TabPage tabPage1;
        private C1.Win.C1FlexGrid.C1FlexGrid _flex;
        private System.Windows.Forms.Button btnExcel;
        private C1.C1Excel.C1XLBook _book;
        private System.Windows.Forms.Button btnOK;
    }
}

