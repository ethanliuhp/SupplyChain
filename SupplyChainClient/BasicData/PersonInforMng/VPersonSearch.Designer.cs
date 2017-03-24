namespace Application.Business.Erp.SupplyChain.Client.BasicData.PersonInforMng
{
    partial class VPersonSearch
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VPersonSearch));
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dtGrid = new System.Windows.Forms.DataGridView();
            this.btnCancle = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtName = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPerCode = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BantchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList1;
            this.treeView.Location = new System.Drawing.Point(6, 17);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(218, 426);
            this.treeView.TabIndex = 0;
            this.treeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_DrawNode);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "logo.jpg");
            this.imageList1.Images.SetKeyName(1, "bumen.png");
            this.imageList1.Images.SetKeyName(2, "renyuan.png");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 462);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "员工信息";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.btnCancle);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtPerCode);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(249, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(448, 433);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "员工信息检索";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dtGrid);
            this.groupBox3.Location = new System.Drawing.Point(17, 45);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(420, 382);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            // 
            // dtGrid
            // 
            this.dtGrid.AllowUserToAddRows = false;
            this.dtGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dtGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dtGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.PerCode,
            this.PerName,
            this.BantchName,
            this.PerId});
            this.dtGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGrid.GridColor = System.Drawing.SystemColors.WindowText;
            this.dtGrid.Location = new System.Drawing.Point(3, 17);
            this.dtGrid.Name = "dtGrid";
            this.dtGrid.RowTemplate.Height = 23;
            this.dtGrid.Size = new System.Drawing.Size(414, 362);
            this.dtGrid.TabIndex = 3;
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(384, 16);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(53, 23);
            this.btnCancle.TabIndex = 40;
            this.btnCancle.Text = "重置";
            this.btnCancle.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(313, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(53, 23);
            this.button1.TabIndex = 39;
            this.button1.Text = "查 询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtName.BackColor = System.Drawing.SystemColors.Control;
            this.txtName.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtName.DrawSelf = false;
            this.txtName.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtName.EnterToTab = false;
            this.txtName.Location = new System.Drawing.Point(215, 19);
            this.txtName.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtName.Name = "txtName";
            this.txtName.Padding = new System.Windows.Forms.Padding(1);
            this.txtName.ReadOnly = false;
            this.txtName.Size = new System.Drawing.Size(92, 16);
            this.txtName.TabIndex = 38;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 37;
            this.label2.Text = "姓 名:";
            // 
            // txtPerCode
            // 
            this.txtPerCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPerCode.BackColor = System.Drawing.SystemColors.Control;
            this.txtPerCode.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtPerCode.DrawSelf = false;
            this.txtPerCode.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtPerCode.EnterToTab = false;
            this.txtPerCode.Location = new System.Drawing.Point(62, 19);
            this.txtPerCode.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtPerCode.Name = "txtPerCode";
            this.txtPerCode.Padding = new System.Windows.Forms.Padding(1);
            this.txtPerCode.ReadOnly = false;
            this.txtPerCode.Size = new System.Drawing.Size(100, 16);
            this.txtPerCode.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "工 号:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(608, 441);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // Selected
            // 
            this.Selected.HeaderText = "选择";
            this.Selected.Name = "Selected";
            this.Selected.Width = 50;
            // 
            // PerCode
            // 
            this.PerCode.HeaderText = "工号";
            this.PerCode.Name = "PerCode";
            // 
            // PerName
            // 
            this.PerName.HeaderText = "姓名";
            this.PerName.Name = "PerName";
            // 
            // BantchName
            // 
            this.BantchName.HeaderText = "行政部门";
            this.BantchName.Name = "BantchName";
            // 
            // PerId
            // 
            this.PerId.HeaderText = "PerId";
            this.PerId.Name = "PerId";
            this.PerId.Visible = false;
            // 
            // VPersonSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 470);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "VPersonSearch";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtName;
        private System.Windows.Forms.Label label2;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtPerCode;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridView dtGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn PerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BantchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PerId;
    }
}