namespace Application.Business.Erp.SupplyChain.Client.ExcelImportMng
{
    partial class VExcelImport
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtfileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_fileView = new System.Windows.Forms.Button();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnExcelImport = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtfileName1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_fileView1 = new System.Windows.Forms.Button();
            this.btnCancel1 = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnExcelImport1 = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtProjectType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnProjectType = new System.Windows.Forms.Button();
            this.btnProjectTypeCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnProjectTypeIn = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnlFloor.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.groupBox3);
            this.pnlFloor.Controls.Add(this.groupBox2);
            this.pnlFloor.Controls.Add(this.groupBox1);
            this.pnlFloor.Size = new System.Drawing.Size(843, 513);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox2, 0);
            this.pnlFloor.Controls.SetChildIndex(this.groupBox3, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, -8);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtfileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_fileView);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnExcelImport);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 144);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "物料分类基础数据导入";
            // 
            // txtfileName
            // 
            this.txtfileName.Location = new System.Drawing.Point(66, 46);
            this.txtfileName.Name = "txtfileName";
            this.txtfileName.ReadOnly = true;
            this.txtfileName.Size = new System.Drawing.Size(268, 21);
            this.txtfileName.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "文件名:";
            // 
            // btn_fileView
            // 
            this.btn_fileView.Location = new System.Drawing.Point(341, 44);
            this.btn_fileView.Name = "btn_fileView";
            this.btn_fileView.Size = new System.Drawing.Size(75, 23);
            this.btn_fileView.TabIndex = 15;
            this.btn_fileView.Text = "浏览文件";
            this.btn_fileView.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(224, 98);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnExcelImport
            // 
            this.btnExcelImport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcelImport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcelImport.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcelImport.Location = new System.Drawing.Point(138, 98);
            this.btnExcelImport.Name = "btnExcelImport";
            this.btnExcelImport.Size = new System.Drawing.Size(75, 23);
            this.btnExcelImport.TabIndex = 11;
            this.btnExcelImport.Text = "导入";
            this.btnExcelImport.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtfileName1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btn_fileView1);
            this.groupBox2.Controls.Add(this.btnCancel1);
            this.groupBox2.Controls.Add(this.btnExcelImport1);
            this.groupBox2.Location = new System.Drawing.Point(12, 167);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(464, 144);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "基础数据导入1";
            // 
            // txtfileName1
            // 
            this.txtfileName1.Location = new System.Drawing.Point(66, 46);
            this.txtfileName1.Name = "txtfileName1";
            this.txtfileName1.ReadOnly = true;
            this.txtfileName1.Size = new System.Drawing.Size(268, 21);
            this.txtfileName1.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "文件名:";
            // 
            // btn_fileView1
            // 
            this.btn_fileView1.Location = new System.Drawing.Point(341, 44);
            this.btn_fileView1.Name = "btn_fileView1";
            this.btn_fileView1.Size = new System.Drawing.Size(75, 23);
            this.btn_fileView1.TabIndex = 15;
            this.btn_fileView1.Text = "浏览文件";
            this.btn_fileView1.UseVisualStyleBackColor = true;
            // 
            // btnCancel1
            // 
            this.btnCancel1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel1.Location = new System.Drawing.Point(224, 98);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size(75, 23);
            this.btnCancel1.TabIndex = 12;
            this.btnCancel1.Text = "取消";
            this.btnCancel1.UseVisualStyleBackColor = true;
            // 
            // btnExcelImport1
            // 
            this.btnExcelImport1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExcelImport1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExcelImport1.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnExcelImport1.Location = new System.Drawing.Point(138, 98);
            this.btnExcelImport1.Name = "btnExcelImport1";
            this.btnExcelImport1.Size = new System.Drawing.Size(75, 23);
            this.btnExcelImport1.TabIndex = 11;
            this.btnExcelImport1.Text = "导入";
            this.btnExcelImport1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtProjectType);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.btnProjectType);
            this.groupBox3.Controls.Add(this.btnProjectTypeCancel);
            this.groupBox3.Controls.Add(this.btnProjectTypeIn);
            this.groupBox3.Location = new System.Drawing.Point(14, 328);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(464, 144);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "工程WBS任务类型";
            // 
            // txtProjectType
            // 
            this.txtProjectType.Location = new System.Drawing.Point(66, 46);
            this.txtProjectType.Name = "txtProjectType";
            this.txtProjectType.ReadOnly = true;
            this.txtProjectType.Size = new System.Drawing.Size(268, 21);
            this.txtProjectType.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "文件名:";
            // 
            // btnProjectType
            // 
            this.btnProjectType.Location = new System.Drawing.Point(341, 44);
            this.btnProjectType.Name = "btnProjectType";
            this.btnProjectType.Size = new System.Drawing.Size(75, 23);
            this.btnProjectType.TabIndex = 15;
            this.btnProjectType.Text = "浏览文件";
            this.btnProjectType.UseVisualStyleBackColor = true;
            // 
            // btnProjectTypeCancel
            // 
            this.btnProjectTypeCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnProjectTypeCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnProjectTypeCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProjectTypeCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnProjectTypeCancel.Location = new System.Drawing.Point(224, 98);
            this.btnProjectTypeCancel.Name = "btnProjectTypeCancel";
            this.btnProjectTypeCancel.Size = new System.Drawing.Size(75, 23);
            this.btnProjectTypeCancel.TabIndex = 12;
            this.btnProjectTypeCancel.Text = "取消";
            this.btnProjectTypeCancel.UseVisualStyleBackColor = true;
            // 
            // btnProjectTypeIn
            // 
            this.btnProjectTypeIn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnProjectTypeIn.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProjectTypeIn.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnProjectTypeIn.Location = new System.Drawing.Point(138, 98);
            this.btnProjectTypeIn.Name = "btnProjectTypeIn";
            this.btnProjectTypeIn.Size = new System.Drawing.Size(75, 23);
            this.btnProjectTypeIn.TabIndex = 11;
            this.btnProjectTypeIn.Text = "导入";
            this.btnProjectTypeIn.UseVisualStyleBackColor = true;
            // 
            // VExcelImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 513);
            this.Name = "VExcelImport";
            this.Text = "VMaterialRentalOrderQuery";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcelImport;
        private System.Windows.Forms.TextBox txtfileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_fileView;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtfileName1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_fileView1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel1;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnExcelImport1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtProjectType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnProjectType;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnProjectTypeCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnProjectTypeIn;
    }
}