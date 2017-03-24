namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VFundSchemeFormulaCheck
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
            this.lvCheckItems = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.btnStartCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvCheckItems
            // 
            this.lvCheckItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCheckItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvCheckItems.FullRowSelect = true;
            this.lvCheckItems.GridLines = true;
            this.lvCheckItems.Location = new System.Drawing.Point(12, 12);
            this.lvCheckItems.Name = "lvCheckItems";
            this.lvCheckItems.Size = new System.Drawing.Size(800, 352);
            this.lvCheckItems.TabIndex = 0;
            this.lvCheckItems.UseCompatibleStateImageBehavior = false;
            this.lvCheckItems.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "公式";
            this.columnHeader1.Width = 560;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "检查结果";
            this.columnHeader2.Width = 200;
            // 
            // btnStartCheck
            // 
            this.btnStartCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStartCheck.Location = new System.Drawing.Point(362, 370);
            this.btnStartCheck.Name = "btnStartCheck";
            this.btnStartCheck.Size = new System.Drawing.Size(100, 23);
            this.btnStartCheck.TabIndex = 9;
            this.btnStartCheck.Text = "开始检查";
            this.btnStartCheck.UseVisualStyleBackColor = true;
            // 
            // VFundSchemeFormulaCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 401);
            this.Controls.Add(this.btnStartCheck);
            this.Controls.Add(this.lvCheckItems);
            this.Name = "VFundSchemeFormulaCheck";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "资金策划数据公式检查";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvCheckItems;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnStartCheck;
    }
}