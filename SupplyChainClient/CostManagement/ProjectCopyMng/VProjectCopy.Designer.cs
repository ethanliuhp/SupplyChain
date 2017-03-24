namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectCopyMng
{
    partial class VProjectCopy
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
            this.listBoxType = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.listBoxTypeRight = new Application.Business.Erp.ClientSystem.Template.CustomListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCopy = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PBSCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.WBSCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtYProject = new System.Windows.Forms.TextBox();
            this.txtMProject = new System.Windows.Forms.TextBox();
            this.btnYSearch = new System.Windows.Forms.Button();
            this.btnMSearch = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlFloor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnMSearch);
            this.pnlFloor.Controls.Add(this.btnYSearch);
            this.pnlFloor.Controls.Add(this.txtMProject);
            this.pnlFloor.Controls.Add(this.txtYProject);
            this.pnlFloor.Controls.Add(this.label4);
            this.pnlFloor.Controls.Add(this.label3);
            this.pnlFloor.Controls.Add(this.panel2);
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Controls.Add(this.btnCopy);
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.listBoxTypeRight);
            this.pnlFloor.Controls.Add(this.label2);
            this.pnlFloor.Controls.Add(this.listBoxType);
            this.pnlFloor.Size = new System.Drawing.Size(824, 528);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.listBoxType, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.listBoxTypeRight, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCopy, 0);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.panel2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label3, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label4, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtYProject, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtMProject, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnYSearch, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnMSearch, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // listBoxType
            // 
            this.listBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxType.FormattingEnabled = true;
            this.listBoxType.ItemHeight = 12;
            this.listBoxType.Location = new System.Drawing.Point(17, 81);
            this.listBoxType.Name = "listBoxType";
            this.listBoxType.ScrollAlwaysVisible = true;
            this.listBoxType.Size = new System.Drawing.Size(221, 410);
            this.listBoxType.TabIndex = 162;
            // 
            // listBoxTypeRight
            // 
            this.listBoxTypeRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxTypeRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxTypeRight.FormattingEnabled = true;
            this.listBoxTypeRight.ItemHeight = 12;
            this.listBoxTypeRight.Location = new System.Drawing.Point(581, 81);
            this.listBoxTypeRight.Name = "listBoxTypeRight";
            this.listBoxTypeRight.ScrollAlwaysVisible = true;
            this.listBoxTypeRight.Size = new System.Drawing.Size(221, 410);
            this.listBoxTypeRight.TabIndex = 163;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 164;
            this.label1.Text = "源项目";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(581, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 165;
            this.label2.Text = "目标项目";
            // 
            // btnCopy
            // 
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCopy.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCopy.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCopy.Location = new System.Drawing.Point(356, 20);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 28);
            this.btnCopy.TabIndex = 168;
            this.btnCopy.Text = "复制数据";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.PBSCategory);
            this.panel1.Location = new System.Drawing.Point(245, 81);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(435, 226);
            this.panel1.TabIndex = 169;
            // 
            // PBSCategory
            // 
            this.PBSCategory.AllowDrop = true;
            this.PBSCategory.BackColor = System.Drawing.SystemColors.Window;
            this.PBSCategory.CheckBoxes = true;
            this.PBSCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PBSCategory.HideSelection = false;
            this.PBSCategory.Location = new System.Drawing.Point(0, 0);
            this.PBSCategory.Name = "PBSCategory";
            this.PBSCategory.Size = new System.Drawing.Size(435, 226);
            this.PBSCategory.TabIndex = 1;
            // 
            // WBSCategory
            // 
            this.WBSCategory.AllowDrop = true;
            this.WBSCategory.BackColor = System.Drawing.SystemColors.Window;
            this.WBSCategory.CheckBoxes = true;
            this.WBSCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WBSCategory.HideSelection = false;
            this.WBSCategory.Location = new System.Drawing.Point(0, 0);
            this.WBSCategory.Name = "WBSCategory";
            this.WBSCategory.Size = new System.Drawing.Size(435, 245);
            this.WBSCategory.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 171;
            this.label3.Text = "PBS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 310);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 172;
            this.label4.Text = "WBS";
            // 
            // txtYProject
            // 
            this.txtYProject.Location = new System.Drawing.Point(17, 26);
            this.txtYProject.Name = "txtYProject";
            this.txtYProject.Size = new System.Drawing.Size(160, 21);
            this.txtYProject.TabIndex = 173;
            // 
            // txtMProject
            // 
            this.txtMProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMProject.Location = new System.Drawing.Point(542, 26);
            this.txtMProject.Name = "txtMProject";
            this.txtMProject.Size = new System.Drawing.Size(160, 21);
            this.txtMProject.TabIndex = 174;
            // 
            // btnYSearch
            // 
            this.btnYSearch.Location = new System.Drawing.Point(183, 25);
            this.btnYSearch.Name = "btnYSearch";
            this.btnYSearch.Size = new System.Drawing.Size(75, 23);
            this.btnYSearch.TabIndex = 175;
            this.btnYSearch.Text = "源项目查询";
            this.btnYSearch.UseVisualStyleBackColor = true;
            // 
            // btnMSearch
            // 
            this.btnMSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMSearch.Location = new System.Drawing.Point(708, 25);
            this.btnMSearch.Name = "btnMSearch";
            this.btnMSearch.Size = new System.Drawing.Size(94, 23);
            this.btnMSearch.TabIndex = 176;
            this.btnMSearch.Text = "目标项目查询";
            this.btnMSearch.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.WBSCategory);
            this.panel2.Location = new System.Drawing.Point(245, 325);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(435, 245);
            this.panel2.TabIndex = 170;
            // 
            // VProjectCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 528);
            this.Name = "VProjectCopy";
            this.Text = "项目工程复制";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Application.Business.Erp.ClientSystem.Template.CustomListBox listBoxType;
        private Application.Business.Erp.ClientSystem.Template.CustomListBox listBoxTypeRight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCopy;
        private System.Windows.Forms.Panel panel1;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView PBSCategory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private VirtualMachine.Component.WinControls.Controls.CustomTreeView WBSCategory;
        private System.Windows.Forms.Button btnMSearch;
        private System.Windows.Forms.Button btnYSearch;
        private System.Windows.Forms.TextBox txtMProject;
        private System.Windows.Forms.TextBox txtYProject;
        private System.Windows.Forms.Panel panel2;

    }
}