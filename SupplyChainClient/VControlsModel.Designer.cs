namespace Application.Business.Erp.SupplyChain.Client
{
    partial class VControlsModel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VControlsModel));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node2", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            this.btnOK = new System.Windows.Forms.Button();
            this.rdoOut = new System.Windows.Forms.RadioButton();
            this.rdoInner = new System.Windows.Forms.RadioButton();
            this.dtpDeliveryDate = new System.Windows.Forms.DateTimePicker();
            this.chkOut = new System.Windows.Forms.CheckBox();
            this.txtClassTeam = new Application.Business.Erp.SupplyChain.Client.Basic.Controls.CommonClassTeam();
            this.txtRemark = new VirtualMachine.Component.WinControls.Controls.CustomEdit();
            this.customLabel5 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.customLabel12 = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.lblSupplyContract = new VirtualMachine.Component.WinControls.Controls.CustomLabel();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.grdDtl = new VirtualMachine.Component.WinControls.Controls.CustomCFlexGrid(this.components);
            this.cboKind = new System.Windows.Forms.ComboBox();
            this.PicHeader = new System.Windows.Forms.PictureBox();
            this.treGroup = new System.Windows.Forms.TreeView();
            this.lnkUp = new System.Windows.Forms.LinkLabel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tabPage = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grpMain = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtClassTeam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDtl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicHeader)).BeginInit();
            this.tabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(47, 97);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "按钮";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // rdoOut
            // 
            this.rdoOut.AutoSize = true;
            this.rdoOut.Location = new System.Drawing.Point(426, 47);
            this.rdoOut.Name = "rdoOut";
            this.rdoOut.Size = new System.Drawing.Size(47, 16);
            this.rdoOut.TabIndex = 24;
            this.rdoOut.Text = "厂外";
            this.rdoOut.UseVisualStyleBackColor = true;
            // 
            // rdoInner
            // 
            this.rdoInner.AutoSize = true;
            this.rdoInner.Checked = true;
            this.rdoInner.Location = new System.Drawing.Point(373, 46);
            this.rdoInner.Name = "rdoInner";
            this.rdoInner.Size = new System.Drawing.Size(47, 16);
            this.rdoInner.TabIndex = 25;
            this.rdoInner.TabStop = true;
            this.rdoInner.Text = "厂内";
            this.rdoInner.UseVisualStyleBackColor = true;
            // 
            // dtpDeliveryDate
            // 
            this.dtpDeliveryDate.Location = new System.Drawing.Point(553, 44);
            this.dtpDeliveryDate.Name = "dtpDeliveryDate";
            this.dtpDeliveryDate.Size = new System.Drawing.Size(109, 21);
            this.dtpDeliveryDate.TabIndex = 23;
            // 
            // chkOut
            // 
            this.chkOut.AutoSize = true;
            this.chkOut.Location = new System.Drawing.Point(319, 47);
            this.chkOut.Name = "chkOut";
            this.chkOut.Size = new System.Drawing.Size(48, 16);
            this.chkOut.TabIndex = 22;
            this.chkOut.Text = "委外";
            this.chkOut.UseVisualStyleBackColor = true;
            // 
            // txtClassTeam
            // 
            this.txtClassTeam.Location = new System.Drawing.Point(200, 42);
            this.txtClassTeam.Name = "txtClassTeam";
            this.txtClassTeam.Result = ((System.Collections.IList)(resources.GetObject("txtClassTeam.Result")));
            this.txtClassTeam.Size = new System.Drawing.Size(116, 21);
            this.txtClassTeam.TabIndex = 16;
            this.txtClassTeam.Tag = null;
            this.txtClassTeam.Value = "";
            this.txtClassTeam.VisibleButtons = C1.Win.C1Input.DropDownControlButtonFlags.Modal;
            // 
            // txtRemark
            // 
            this.txtRemark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemark.BackColor = System.Drawing.SystemColors.Control;
            this.txtRemark.CustomBorderStyle = VirtualMachine.Component.WinControls.Controls.EditBorderStyle.None;
            this.txtRemark.DrawSelf = false;
            this.txtRemark.EditStyle = VirtualMachine.Component.WinControls.Controls.EditStyle.Text;
            this.txtRemark.EnterToTab = false;
            this.txtRemark.Location = new System.Drawing.Point(47, 75);
            this.txtRemark.Mask = VirtualMachine.Component.WinControls.Controls.MaskStyle.None;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Padding = new System.Windows.Forms.Padding(1);
            this.txtRemark.ReadOnly = false;
            this.txtRemark.Size = new System.Drawing.Size(618, 16);
            this.txtRemark.TabIndex = 20;
            // 
            // customLabel5
            // 
            this.customLabel5.AutoSize = true;
            this.customLabel5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel5.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel5.Location = new System.Drawing.Point(479, 50);
            this.customLabel5.Name = "customLabel5";
            this.customLabel5.Size = new System.Drawing.Size(59, 12);
            this.customLabel5.TabIndex = 18;
            this.customLabel5.Text = "交货日期:";
            this.customLabel5.UnderLineColor = System.Drawing.Color.Red;
            // 
            // customLabel12
            // 
            this.customLabel12.AutoSize = true;
            this.customLabel12.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.customLabel12.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.customLabel12.Location = new System.Drawing.Point(167, 51);
            this.customLabel12.Name = "customLabel12";
            this.customLabel12.Size = new System.Drawing.Size(35, 12);
            this.customLabel12.TabIndex = 19;
            this.customLabel12.Text = "班组:";
            this.customLabel12.UnderLineColor = System.Drawing.Color.Red;
            // 
            // lblSupplyContract
            // 
            this.lblSupplyContract.AutoSize = true;
            this.lblSupplyContract.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSupplyContract.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.lblSupplyContract.Location = new System.Drawing.Point(6, 51);
            this.lblSupplyContract.Name = "lblSupplyContract";
            this.lblSupplyContract.Size = new System.Drawing.Size(35, 12);
            this.lblSupplyContract.TabIndex = 17;
            this.lblSupplyContract.Text = "单号:";
            this.lblSupplyContract.UnderLineColor = System.Drawing.Color.Red;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(47, 42);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(117, 21);
            this.txtCode.TabIndex = 27;
            // 
            // grdDtl
            // 
            this.grdDtl.AllowAddNew = true;
            this.grdDtl.AllowDelete = true;
            this.grdDtl.ColumnInfo = resources.GetString("grdDtl.ColumnInfo");
            this.grdDtl.EnalbeSerial = true;
            this.grdDtl.ExtendLastCol = true;
            this.grdDtl.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grdDtl.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.grdDtl.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.grdDtl.Location = new System.Drawing.Point(47, 126);
            this.grdDtl.LockColumnBackColor = System.Drawing.Color.White;
            this.grdDtl.LockColumnIndex = 0;
            this.grdDtl.LockColumnName = null;
            this.grdDtl.LockColumnsIndex = null;
            this.grdDtl.LockColumnsName = null;
            this.grdDtl.Name = "grdDtl";
            this.grdDtl.Rows.Count = 1;
            this.grdDtl.Rows.DefaultSize = 18;
            this.grdDtl.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.grdDtl.ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WhenEditing;
            this.grdDtl.ShowCellLabels = true;
            this.grdDtl.Size = new System.Drawing.Size(359, 178);
            this.grdDtl.StyleInfo = resources.GetString("grdDtl.StyleInfo");
            this.grdDtl.TabIndex = 28;
            // 
            // cboKind
            // 
            this.cboKind.FormattingEnabled = true;
            this.cboKind.Location = new System.Drawing.Point(508, 133);
            this.cboKind.Name = "cboKind";
            this.cboKind.Size = new System.Drawing.Size(121, 20);
            this.cboKind.TabIndex = 29;
            // 
            // PicHeader
            // 
            this.PicHeader.Location = new System.Drawing.Point(508, 159);
            this.PicHeader.Name = "PicHeader";
            this.PicHeader.Size = new System.Drawing.Size(162, 79);
            this.PicHeader.TabIndex = 30;
            this.PicHeader.TabStop = false;
            // 
            // treGroup
            // 
            this.treGroup.Location = new System.Drawing.Point(676, 133);
            this.treGroup.Name = "treGroup";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Node1";
            treeNode2.Name = "Node0";
            treeNode2.Text = "Node0";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Node3";
            treeNode4.Name = "Node4";
            treeNode4.Text = "Node4";
            treeNode5.Name = "Node2";
            treeNode5.Text = "Node2";
            this.treGroup.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode5});
            this.treGroup.Size = new System.Drawing.Size(121, 97);
            this.treGroup.TabIndex = 31;
            // 
            // lnkUp
            // 
            this.lnkUp.AutoSize = true;
            this.lnkUp.Location = new System.Drawing.Point(128, 108);
            this.lnkUp.Name = "lnkUp";
            this.lnkUp.Size = new System.Drawing.Size(29, 12);
            this.lnkUp.TabIndex = 32;
            this.lnkUp.TabStop = true;
            this.lnkUp.Text = "上移";
            // 
            // pnlMain
            // 
            this.pnlMain.Location = new System.Drawing.Point(47, 310);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(200, 100);
            this.pnlMain.TabIndex = 33;
            // 
            // tabPage
            // 
            this.tabPage.Controls.Add(this.tabPage1);
            this.tabPage.Controls.Add(this.tabPage2);
            this.tabPage.Location = new System.Drawing.Point(263, 310);
            this.tabPage.Name = "tabPage";
            this.tabPage.SelectedIndex = 0;
            this.tabPage.Size = new System.Drawing.Size(200, 100);
            this.tabPage.TabIndex = 34;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(192, 75);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 75);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grpMain
            // 
            this.grpMain.Location = new System.Drawing.Point(470, 310);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(200, 100);
            this.grpMain.TabIndex = 35;
            this.grpMain.TabStop = false;
            this.grpMain.Text = "groupBox1";
            // 
            // VControlsModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 435);
            this.Controls.Add(this.grpMain);
            this.Controls.Add(this.tabPage);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.lnkUp);
            this.Controls.Add(this.treGroup);
            this.Controls.Add(this.PicHeader);
            this.Controls.Add(this.cboKind);
            this.Controls.Add(this.grdDtl);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.rdoOut);
            this.Controls.Add(this.rdoInner);
            this.Controls.Add(this.dtpDeliveryDate);
            this.Controls.Add(this.chkOut);
            this.Controls.Add(this.txtClassTeam);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.customLabel5);
            this.Controls.Add(this.customLabel12);
            this.Controls.Add(this.lblSupplyContract);
            this.Name = "VControlsModel";
            this.Text = "VControlsModel";
            ((System.ComponentModel.ISupportInitialize)(this.txtClassTeam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDtl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicHeader)).EndInit();
            this.tabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rdoOut;
        private System.Windows.Forms.RadioButton rdoInner;
        private System.Windows.Forms.DateTimePicker dtpDeliveryDate;
        private System.Windows.Forms.CheckBox chkOut;
        private Application.Business.Erp.SupplyChain.Client.Basic.Controls.CommonClassTeam txtClassTeam;
        private VirtualMachine.Component.WinControls.Controls.CustomEdit txtRemark;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel5;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel customLabel12;
        private VirtualMachine.Component.WinControls.Controls.CustomLabel lblSupplyContract;
        private System.Windows.Forms.TextBox txtCode;
        private VirtualMachine.Component.WinControls.Controls.CustomCFlexGrid grdDtl;
        private System.Windows.Forms.ComboBox cboKind;
        private System.Windows.Forms.PictureBox PicHeader;
        private System.Windows.Forms.TreeView treGroup;
        private System.Windows.Forms.LinkLabel lnkUp;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TabControl tabPage;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox grpMain;
    }
}