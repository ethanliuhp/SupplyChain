namespace Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload
{
    partial class VFilesUpLoad
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
            this.lblFileURL = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblEncode = new System.Windows.Forms.Label();
            this.lblExplain = new System.Windows.Forms.Label();
            this.lblSort = new System.Windows.Forms.Label();
            this.lblWorkflow = new System.Windows.Forms.Label();
            this.txtFilesURL = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtEncode = new System.Windows.Forms.TextBox();
            this.txtExplain = new System.Windows.Forms.TextBox();
            this.txtSort = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnUpLoad = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.cbWorkflow = new System.Windows.Forms.ComboBox();
            this.pnlFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.cbWorkflow);
            this.pnlFloor.Controls.Add(this.btnQuit);
            this.pnlFloor.Controls.Add(this.btnUpLoad);
            this.pnlFloor.Controls.Add(this.btnBrowse);
            this.pnlFloor.Controls.Add(this.lblSort);
            this.pnlFloor.Controls.Add(this.lblName);
            this.pnlFloor.Controls.Add(this.lblWorkflow);
            this.pnlFloor.Controls.Add(this.txtEncode);
            this.pnlFloor.Controls.Add(this.lblFileURL);
            this.pnlFloor.Controls.Add(this.txtSort);
            this.pnlFloor.Controls.Add(this.txtExplain);
            this.pnlFloor.Controls.Add(this.txtName);
            this.pnlFloor.Controls.Add(this.lblExplain);
            this.pnlFloor.Controls.Add(this.txtFilesURL);
            this.pnlFloor.Controls.Add(this.lblEncode);
            this.pnlFloor.Size = new System.Drawing.Size(312, 446);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblEncode, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtFilesURL, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblExplain, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtExplain, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtSort, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblFileURL, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtEncode, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblWorkflow, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblName, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblSort, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnBrowse, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnUpLoad, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnQuit, 0);
            this.pnlFloor.Controls.SetChildIndex(this.cbWorkflow, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // lblFileURL
            // 
            this.lblFileURL.AutoSize = true;
            this.lblFileURL.Location = new System.Drawing.Point(42, 39);
            this.lblFileURL.Name = "lblFileURL";
            this.lblFileURL.Size = new System.Drawing.Size(41, 12);
            this.lblFileURL.TabIndex = 6;
            this.lblFileURL.Text = "文件：";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(18, 71);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(65, 12);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "文档名称：";
            // 
            // lblEncode
            // 
            this.lblEncode.AutoSize = true;
            this.lblEncode.Location = new System.Drawing.Point(18, 107);
            this.lblEncode.Name = "lblEncode";
            this.lblEncode.Size = new System.Drawing.Size(65, 12);
            this.lblEncode.TabIndex = 8;
            this.lblEncode.Text = "文档编码：";
            // 
            // lblExplain
            // 
            this.lblExplain.AutoSize = true;
            this.lblExplain.Location = new System.Drawing.Point(18, 144);
            this.lblExplain.Name = "lblExplain";
            this.lblExplain.Size = new System.Drawing.Size(65, 12);
            this.lblExplain.TabIndex = 9;
            this.lblExplain.Text = "文档说明：";
            // 
            // lblSort
            // 
            this.lblSort.AutoSize = true;
            this.lblSort.Location = new System.Drawing.Point(18, 331);
            this.lblSort.Name = "lblSort";
            this.lblSort.Size = new System.Drawing.Size(65, 12);
            this.lblSort.TabIndex = 10;
            this.lblSort.Text = "文档分类：";
            // 
            // lblWorkflow
            // 
            this.lblWorkflow.AutoSize = true;
            this.lblWorkflow.Enabled = false;
            this.lblWorkflow.Location = new System.Drawing.Point(6, 367);
            this.lblWorkflow.Name = "lblWorkflow";
            this.lblWorkflow.Size = new System.Drawing.Size(77, 12);
            this.lblWorkflow.TabIndex = 11;
            this.lblWorkflow.Text = "文档工作流：";
            // 
            // txtFilesURL
            // 
            this.txtFilesURL.Location = new System.Drawing.Point(87, 33);
            this.txtFilesURL.Name = "txtFilesURL";
            this.txtFilesURL.Size = new System.Drawing.Size(141, 21);
            this.txtFilesURL.TabIndex = 12;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(87, 68);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(213, 21);
            this.txtName.TabIndex = 13;
            // 
            // txtEncode
            // 
            this.txtEncode.Location = new System.Drawing.Point(87, 104);
            this.txtEncode.Name = "txtEncode";
            this.txtEncode.Size = new System.Drawing.Size(213, 21);
            this.txtEncode.TabIndex = 14;
            // 
            // txtExplain
            // 
            this.txtExplain.Location = new System.Drawing.Point(87, 141);
            this.txtExplain.Multiline = true;
            this.txtExplain.Name = "txtExplain";
            this.txtExplain.Size = new System.Drawing.Size(213, 168);
            this.txtExplain.TabIndex = 15;
            this.txtExplain.DoubleClick += new System.EventHandler(this.txtExplain_DoubleClick);
            // 
            // txtSort
            // 
            this.txtSort.Location = new System.Drawing.Point(87, 328);
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(213, 21);
            this.txtSort.TabIndex = 16;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(233, 31);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(68, 23);
            this.btnBrowse.TabIndex = 17;
            this.btnBrowse.Text = "选择文件";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnUpLoad
            // 
            this.btnUpLoad.Location = new System.Drawing.Point(45, 399);
            this.btnUpLoad.Name = "btnUpLoad";
            this.btnUpLoad.Size = new System.Drawing.Size(75, 23);
            this.btnUpLoad.TabIndex = 18;
            this.btnUpLoad.Text = "上传";
            this.btnUpLoad.UseVisualStyleBackColor = true;
            this.btnUpLoad.Click += new System.EventHandler(this.btnUpLoad_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(180, 399);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 23);
            this.btnQuit.TabIndex = 19;
            this.btnQuit.Text = "放弃";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // cbWorkflow
            // 
            this.cbWorkflow.Enabled = false;
            this.cbWorkflow.FormattingEnabled = true;
            this.cbWorkflow.Location = new System.Drawing.Point(87, 364);
            this.cbWorkflow.Name = "cbWorkflow";
            this.cbWorkflow.Size = new System.Drawing.Size(213, 20);
            this.cbWorkflow.TabIndex = 20;
            // 
            // VFilesUpLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 446);
            this.Name = "VFilesUpLoad";
            this.Text = "文件上传";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFileURL;
        private System.Windows.Forms.Label lblSort;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblWorkflow;
        private System.Windows.Forms.Label lblExplain;
        private System.Windows.Forms.Label lblEncode;
        private System.Windows.Forms.TextBox txtEncode;
        private System.Windows.Forms.TextBox txtSort;
        private System.Windows.Forms.TextBox txtExplain;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtFilesURL;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnUpLoad;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ComboBox cbWorkflow;
    }
}