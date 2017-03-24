namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    partial class VFundAssessment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VFundAssessment));
            this.txtCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOperationOrg = new System.Windows.Forms.Button();
            this.txtOperationOrg = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.lblPSelect = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGetData = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.txtCreateBy = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCreateTime = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDocState = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.gdAssesscashDetail = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.gdInterestDetail = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.pnlBody.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlFloor.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.gdInterestDetail);
            this.pnlBody.Controls.Add(this.gdAssesscashDetail);
            this.pnlBody.Location = new System.Drawing.Point(0, 37);
            this.pnlBody.Size = new System.Drawing.Size(1008, 504);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.txtDocState);
            this.pnlFooter.Controls.Add(this.label7);
            this.pnlFooter.Controls.Add(this.txtCreateTime);
            this.pnlFooter.Controls.Add(this.label6);
            this.pnlFooter.Controls.Add(this.txtCreateBy);
            this.pnlFooter.Controls.Add(this.label5);
            this.pnlFooter.Location = new System.Drawing.Point(0, 541);
            this.pnlFooter.Size = new System.Drawing.Size(1008, 32);
            // 
            // spTop
            // 
            this.spTop.Size = new System.Drawing.Size(1008, 0);
            // 
            // pnlFloor
            // 
            this.pnlFloor.Size = new System.Drawing.Size(1008, 573);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnGetData);
            this.pnlHeader.Controls.Add(this.label4);
            this.pnlHeader.Controls.Add(this.cmbMonth);
            this.pnlHeader.Controls.Add(this.label3);
            this.pnlHeader.Controls.Add(this.cmbYear);
            this.pnlHeader.Controls.Add(this.label2);
            this.pnlHeader.Controls.Add(this.btnOperationOrg);
            this.pnlHeader.Controls.Add(this.txtOperationOrg);
            this.pnlHeader.Controls.Add(this.lblPSelect);
            this.pnlHeader.Controls.Add(this.txtCode);
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Size = new System.Drawing.Size(1008, 37);
            this.pnlHeader.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlHeader.Controls.SetChildIndex(this.label1, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtCode, 0);
            this.pnlHeader.Controls.SetChildIndex(this.lblPSelect, 0);
            this.pnlHeader.Controls.SetChildIndex(this.txtOperationOrg, 0);
            this.pnlHeader.Controls.SetChildIndex(this.btnOperationOrg, 0);
            this.pnlHeader.Controls.SetChildIndex(this.label2, 0);
            this.pnlHeader.Controls.SetChildIndex(this.cmbYear, 0);
            this.pnlHeader.Controls.SetChildIndex(this.label3, 0);
            this.pnlHeader.Controls.SetChildIndex(this.cmbMonth, 0);
            this.pnlHeader.Controls.SetChildIndex(this.label4, 0);
            this.pnlHeader.Controls.SetChildIndex(this.btnGetData, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Visible = false;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCode.DrawSelf = false;
            this.txtCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCode.EnterToTab = false;
            this.txtCode.Location = new System.Drawing.Point(94, 8);
            this.txtCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCode.Name = "txtCode";
            this.txtCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(208, 16);
            this.txtCode.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "资金考核单号：";
            // 
            // btnOperationOrg
            // 
            this.btnOperationOrg.Location = new System.Drawing.Point(586, 5);
            this.btnOperationOrg.Name = "btnOperationOrg";
            this.btnOperationOrg.Size = new System.Drawing.Size(44, 23);
            this.btnOperationOrg.TabIndex = 315;
            this.btnOperationOrg.Text = "选择";
            this.btnOperationOrg.UseVisualStyleBackColor = true;
            // 
            // txtOperationOrg
            // 
            this.txtOperationOrg.BackColor = System.Drawing.SystemColors.Control;
            this.txtOperationOrg.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtOperationOrg.DrawSelf = false;
            this.txtOperationOrg.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtOperationOrg.EnterToTab = false;
            this.txtOperationOrg.Location = new System.Drawing.Point(360, 8);
            this.txtOperationOrg.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtOperationOrg.Name = "txtOperationOrg";
            this.txtOperationOrg.Padding = new System.Windows.Forms.Padding(1);
            this.txtOperationOrg.ReadOnly = true;
            this.txtOperationOrg.Size = new System.Drawing.Size(225, 16);
            this.txtOperationOrg.TabIndex = 314;
            // 
            // lblPSelect
            // 
            this.lblPSelect.AutoSize = true;
            this.lblPSelect.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPSelect.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblPSelect.Location = new System.Drawing.Point(326, 10);
            this.lblPSelect.Name = "lblPSelect";
            this.lblPSelect.Size = new System.Drawing.Size(41, 12);
            this.lblPSelect.TabIndex = 313;
            this.lblPSelect.Text = "项目：";
            this.lblPSelect.UnderLineColor = System.Drawing.Color.Red;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(869, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 320;
            this.label4.Text = "月";
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.ItemHeight = 12;
            this.cmbMonth.Location = new System.Drawing.Point(798, 7);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(70, 20);
            this.cmbMonth.TabIndex = 319;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(780, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 318;
            this.label3.Text = "年";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.ItemHeight = 12;
            this.cmbYear.Location = new System.Drawing.Point(709, 7);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(70, 20);
            this.cmbYear.TabIndex = 317;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(650, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 316;
            this.label2.Text = "考核期间：";
            // 
            // btnGetData
            // 
            this.btnGetData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGetData.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetData.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnGetData.Location = new System.Drawing.Point(924, 6);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(75, 23);
            this.btnGetData.TabIndex = 321;
            this.btnGetData.Text = "获取数据";
            this.btnGetData.UseVisualStyleBackColor = true;
            // 
            // txtCreateBy
            // 
            this.txtCreateBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreateBy.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreateBy.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreateBy.DrawSelf = false;
            this.txtCreateBy.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreateBy.EnterToTab = false;
            this.txtCreateBy.Location = new System.Drawing.Point(59, 9);
            this.txtCreateBy.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateBy.Name = "txtCreateBy";
            this.txtCreateBy.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateBy.ReadOnly = true;
            this.txtCreateBy.Size = new System.Drawing.Size(150, 16);
            this.txtCreateBy.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "制单人：";
            // 
            // txtCreateTime
            // 
            this.txtCreateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCreateTime.BackColor = System.Drawing.SystemColors.Control;
            this.txtCreateTime.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtCreateTime.DrawSelf = false;
            this.txtCreateTime.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtCreateTime.EnterToTab = false;
            this.txtCreateTime.Location = new System.Drawing.Point(287, 9);
            this.txtCreateTime.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtCreateTime.Name = "txtCreateTime";
            this.txtCreateTime.Padding = new System.Windows.Forms.Padding(1);
            this.txtCreateTime.ReadOnly = true;
            this.txtCreateTime.Size = new System.Drawing.Size(150, 16);
            this.txtCreateTime.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(229, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 25;
            this.label6.Text = "制单时间：";
            // 
            // txtDocState
            // 
            this.txtDocState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtDocState.BackColor = System.Drawing.SystemColors.Control;
            this.txtDocState.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtDocState.DrawSelf = false;
            this.txtDocState.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtDocState.EnterToTab = false;
            this.txtDocState.Location = new System.Drawing.Point(514, 9);
            this.txtDocState.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtDocState.Name = "txtDocState";
            this.txtDocState.Padding = new System.Windows.Forms.Padding(1);
            this.txtDocState.ReadOnly = true;
            this.txtDocState.Size = new System.Drawing.Size(150, 16);
            this.txtDocState.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(456, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 27;
            this.label7.Text = "单据状态：";
            // 
            // gdAssesscashDetail
            // 
            this.gdAssesscashDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gdAssesscashDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdAssesscashDetail.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdAssesscashDetail.CheckedImage")));
            this.gdAssesscashDetail.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdAssesscashDetail.DefaultRowHeight = ((short)(25));
            this.gdAssesscashDetail.DisplayRowArrow = true;
            this.gdAssesscashDetail.DisplayRowNumber = true;
            this.gdAssesscashDetail.EnterKeyMoveTo = FlexCell.MoveToEnum.NextRow;
            this.gdAssesscashDetail.ExtendLastCol = true;
            this.gdAssesscashDetail.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdAssesscashDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdAssesscashDetail.Location = new System.Drawing.Point(6, 2);
            this.gdAssesscashDetail.Name = "gdAssesscashDetail";
            this.gdAssesscashDetail.Size = new System.Drawing.Size(482, 500);
            this.gdAssesscashDetail.TabIndex = 235;
            this.gdAssesscashDetail.Tag = "项目资金策划考核兑现表";
            this.gdAssesscashDetail.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdAssesscashDetail.UncheckedImage")));
            // 
            // gdInterestDetail
            // 
            this.gdInterestDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gdInterestDetail.BackColorBkg = System.Drawing.SystemColors.ButtonFace;
            this.gdInterestDetail.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdInterestDetail.CheckedImage")));
            this.gdInterestDetail.DefaultFont = new System.Drawing.Font("SimSun", 9F);
            this.gdInterestDetail.DefaultRowHeight = ((short)(25));
            this.gdInterestDetail.DisplayRowArrow = true;
            this.gdInterestDetail.DisplayRowNumber = true;
            this.gdInterestDetail.EnterKeyMoveTo = FlexCell.MoveToEnum.NextRow;
            this.gdInterestDetail.ExtendLastCol = true;
            this.gdInterestDetail.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gdInterestDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gdInterestDetail.Location = new System.Drawing.Point(519, 2);
            this.gdInterestDetail.Name = "gdInterestDetail";
            this.gdInterestDetail.Size = new System.Drawing.Size(482, 500);
            this.gdInterestDetail.TabIndex = 236;
            this.gdInterestDetail.Tag = "项目月度资金利息表";
            this.gdInterestDetail.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("gdInterestDetail.UncheckedImage")));
            // 
            // VFundAssessment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 573);
            this.Name = "VFundAssessment";
            this.Text = "资金考核";
            this.pnlBody.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlFloor.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtOperationOrg;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblPSelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label label2;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnGetData;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtDocState;
        private System.Windows.Forms.Label label7;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreateTime;
        private System.Windows.Forms.Label label6;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtCreateBy;
        private System.Windows.Forms.Label label5;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdAssesscashDetail;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid gdInterestDetail;
    }
}