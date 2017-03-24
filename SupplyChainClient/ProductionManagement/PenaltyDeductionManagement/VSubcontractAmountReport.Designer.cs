namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    partial class VSubcontractAmountReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VSubcontractAmountReport));
            this.btnExcel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnQuery = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.lblDate = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.tabDetail = new System.Windows.Forms.TabPage();
            this.fGridDetail = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.tabCost = new System.Windows.Forms.TabControl();
            this.customLabel7 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtPenaltyRank = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.pnlFloor.SuspendLayout();
            this.tabDetail.SuspendLayout();
            this.tabCost.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnSearch);
            this.pnlFloor.Controls.Add(this.txtPenaltyRank);
            this.pnlFloor.Controls.Add(this.cbYear);
            this.pnlFloor.Controls.Add(this.customLabel7);
            this.pnlFloor.Controls.Add(this.btnExcel);
            this.pnlFloor.Controls.Add(this.lblDate);
            this.pnlFloor.Controls.Add(this.tabCost);
            this.pnlFloor.Controls.Add(this.btnQuery);
            this.pnlFloor.Margin = new System.Windows.Forms.Padding(4);
            this.pnlFloor.Size = new System.Drawing.Size(1011, 508);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuery, 0);
            this.pnlFloor.Controls.SetChildIndex(this.tabCost, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblDate, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnExcel, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.customLabel7, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cbYear, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtPenaltyRank, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSearch, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            // 
            // btnExcel
            // 
            this.btnExcel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcel.Location = new System.Drawing.Point(741, 8);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(75, 28);
            this.btnExcel.TabIndex = 14;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            // 
            // btnQuery
            // 
            this.btnQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnQuery.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQuery.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnQuery.Location = new System.Drawing.Point(647, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 28);
            this.btnQuery.TabIndex = 13;
            this.btnQuery.Text = "�� ѯ";
            this.btnQuery.UseVisualStyleBackColor = true;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDate.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblDate.Location = new System.Drawing.Point(75, 15);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(59, 12);
            this.lblDate.TabIndex = 150;
            this.lblDate.Text = "ͳ������:";
            this.lblDate.UnderLineColor = System.Drawing.Color.Red;
            // 
            // tabDetail
            // 
            this.tabDetail.Controls.Add(this.fGridDetail);
            this.tabDetail.Location = new System.Drawing.Point(4, 22);
            this.tabDetail.Name = "tabDetail";
            this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabDetail.Size = new System.Drawing.Size(988, 442);
            this.tabDetail.TabIndex = 0;
            this.tabDetail.Text = "������ϸ";
            this.tabDetail.UseVisualStyleBackColor = true;
            // 
            // fGridDetail
            // 
            this.fGridDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fGridDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.fGridDetail.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail.CheckedImage")));
            this.fGridDetail.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.fGridDetail.DefaultRowHeight = ((short)(24));
            this.fGridDetail.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fGridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.fGridDetail.Location = new System.Drawing.Point(4, 3);
            this.fGridDetail.Name = "fGridDetail";
            this.fGridDetail.Size = new System.Drawing.Size(978, 433);
            this.fGridDetail.TabIndex = 5;
            this.fGridDetail.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("fGridDetail.UncheckedImage")));
            // 
            // tabCost
            // 
            this.tabCost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCost.Controls.Add(this.tabDetail);
            this.tabCost.Location = new System.Drawing.Point(12, 37);
            this.tabCost.Name = "tabCost";
            this.tabCost.SelectedIndex = 0;
            this.tabCost.Size = new System.Drawing.Size(996, 468);
            this.tabCost.TabIndex = 149;
            // 
            // customLabel7
            // 
            this.customLabel7.AutoSize = true;
            this.customLabel7.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel7.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel7.Location = new System.Drawing.Point(270, 17);
            this.customLabel7.Name = "customLabel7";
            this.customLabel7.Size = new System.Drawing.Size(59, 12);
            this.customLabel7.TabIndex = 173;
            this.customLabel7.Text = "���㵥λ:";
            this.customLabel7.UnderLineColor = System.Drawing.Color.Red;
            // 
            // cbYear
            // 
            this.cbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Location = new System.Drawing.Point(137, 12);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(121, 20);
            this.cbYear.TabIndex = 176;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(540, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(48, 23);
            this.btnSearch.TabIndex = 178;
            this.btnSearch.Text = "ѡ��";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtPenaltyRank
            // 
            this.txtPenaltyRank.BackColor = System.Drawing.SystemColors.Control;
            this.txtPenaltyRank.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPenaltyRank.DrawSelf = false;
            this.txtPenaltyRank.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPenaltyRank.EnterToTab = false;
            this.txtPenaltyRank.Location = new System.Drawing.Point(330, 12);
            this.txtPenaltyRank.Margin = new System.Windows.Forms.Padding(4);
            this.txtPenaltyRank.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPenaltyRank.Name = "txtPenaltyRank";
            this.txtPenaltyRank.Padding = new System.Windows.Forms.Padding(1);
            this.txtPenaltyRank.ReadOnly = true;
            this.txtPenaltyRank.Size = new System.Drawing.Size(210, 16);
            this.txtPenaltyRank.TabIndex = 177;
            // 
            // VSubcontractAmountReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 508);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "VSubcontractAmountReport";
            this.Text = "�ְ���λ������̨��";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.tabDetail.ResumeLayout(false);
            this.tabCost.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnQuery;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblDate;
        private System.Windows.Forms.TabControl tabCost;
        private System.Windows.Forms.TabPage tabDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid fGridDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel7;
        private System.Windows.Forms.ComboBox cbYear;
        private System.Windows.Forms.Button btnSearch;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPenaltyRank;
    }
}