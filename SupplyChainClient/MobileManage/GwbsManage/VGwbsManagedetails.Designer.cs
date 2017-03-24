namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage
{
    partial class VGwbsManagedetails
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGvDetails = new System.Windows.Forms.DataGridView();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDtlName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDtlDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtDtlCostItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResourceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuantitiesCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detailsSearchBtn = new System.Windows.Forms.Button();
            this.detailsTxt = new System.Windows.Forms.TextBox();
            this.pnlFloor.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGvDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.Size = new System.Drawing.Size(310, 40);
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Size = new System.Drawing.Size(312, 446);
            this.pnlFloor.Controls.SetChildIndex(this.lblHeaderLine, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Enabled = false;
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGvDetails);
            this.panel1.Controls.Add(this.detailsSearchBtn);
            this.panel1.Controls.Add(this.detailsTxt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 352);
            this.panel1.TabIndex = 1;
            // 
            // dataGvDetails
            // 
            this.dataGvDetails.BackgroundColor = System.Drawing.Color.White;
            this.dataGvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGvDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.colDtlName,
            this.colDtlDesc,
            this.txtDtlCostItem,
            this.ResourceCol,
            this.QuantitiesCol,
            this.PlanCol,
            this.StateCol});
            this.dataGvDetails.Location = new System.Drawing.Point(17, 50);
            this.dataGvDetails.Name = "dataGvDetails";
            this.dataGvDetails.RowTemplate.Height = 23;
            this.dataGvDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGvDetails.Size = new System.Drawing.Size(275, 292);
            this.dataGvDetails.TabIndex = 2;
            // 
            // code
            // 
            this.code.HeaderText = "编码";
            this.code.Name = "code";
            this.code.Width = 60;
            // 
            // colDtlName
            // 
            this.colDtlName.HeaderText = "明细名称";
            this.colDtlName.Name = "colDtlName";
            // 
            // colDtlDesc
            // 
            this.colDtlDesc.HeaderText = "明细说明";
            this.colDtlDesc.Name = "colDtlDesc";
            // 
            // txtDtlCostItem
            // 
            this.txtDtlCostItem.HeaderText = "成本项";
            this.txtDtlCostItem.Name = "txtDtlCostItem";
            // 
            // ResourceCol
            // 
            this.ResourceCol.HeaderText = "资源类型";
            this.ResourceCol.Name = "ResourceCol";
            // 
            // QuantitiesCol
            // 
            this.QuantitiesCol.HeaderText = "工程量";
            this.QuantitiesCol.Name = "QuantitiesCol";
            // 
            // PlanCol
            // 
            this.PlanCol.HeaderText = "工程进度";
            this.PlanCol.Name = "PlanCol";
            // 
            // StateCol
            // 
            this.StateCol.HeaderText = "状态";
            this.StateCol.Name = "StateCol";
            // 
            // detailsSearchBtn
            // 
            this.detailsSearchBtn.Location = new System.Drawing.Point(235, 21);
            this.detailsSearchBtn.Name = "detailsSearchBtn";
            this.detailsSearchBtn.Size = new System.Drawing.Size(53, 23);
            this.detailsSearchBtn.TabIndex = 1;
            this.detailsSearchBtn.Text = "搜索";
            this.detailsSearchBtn.UseVisualStyleBackColor = true;
            this.detailsSearchBtn.Click += new System.EventHandler(this.detailsSearchBtn_Click);
            // 
            // detailsTxt
            // 
            this.detailsTxt.Location = new System.Drawing.Point(17, 21);
            this.detailsTxt.Name = "detailsTxt";
            this.detailsTxt.Size = new System.Drawing.Size(203, 21);
            this.detailsTxt.TabIndex = 0;
            this.detailsTxt.TextChanged += new System.EventHandler(this.detailsTxt_TextChanged);
            // 
            // VGwbsManagedetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(312, 446);
            this.Name = "VGwbsManagedetails";
            this.Text = "工程任务明细信息";
            this.Load += new System.EventHandler(this.VGwbsManagedetails_Load_1);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGvDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGvDetails;
        private System.Windows.Forms.Button detailsSearchBtn;
        private System.Windows.Forms.TextBox detailsTxt;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtlName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtlDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtDtlCostItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResourceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuantitiesCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlanCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn StateCol;


    }
}