using VirtualMachine.Component.WinControls.Controls;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.Main
{
    partial class VStartPage { 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VStartPage));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.wbServer = new System.Windows.Forms.WebBrowser();
            this.authGo1 = new AuthManager.AuthMng.AuthControlsMng.AuthGo();
            this.grpResult = new VirtualMachine.Component.WinControls.Controls.NavigationBarCtl.NavigationBar.NaviGroup(this.components);
            this.naviGroup1 = new VirtualMachine.Component.WinControls.Controls.NavigationBarCtl.NavigationBar.NaviGroup(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgBill = new VirtualMachine.Component.WinControls.Controls.CustomDataGridView();
            this.collBillName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatePerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstBoxMsg = new System.Windows.Forms.ListBox();
            this.pnlFloor.SuspendLayout();
            this.panel2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.authGo1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.naviGroup1)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgBill)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pnlFloor.Controls.Add(this.panel2);
            this.pnlFloor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlFloor.Size = new System.Drawing.Size(854, 499);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(854, 499);
            this.panel2.TabIndex = 45;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(854, 499);
            this.splitContainer1.SplitterDistance = 327;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.wbServer);
            this.groupBox1.Controls.Add(this.authGo1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(852, 325);
            this.groupBox1.TabIndex = 179;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息系统应用情况排名";
            // 
            // wbServer
            // 
            this.wbServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbServer.Location = new System.Drawing.Point(3, 17);
            this.wbServer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.wbServer.MinimumSize = new System.Drawing.Size(15, 16);
            this.wbServer.Name = "wbServer";
            this.wbServer.Size = new System.Drawing.Size(846, 305);
            this.wbServer.TabIndex = 58;
            // 
            // authGo1
            // 
            this.authGo1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.authGo1.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.authGo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.authGo1.Controls.Add(this.grpResult);
            this.authGo1.Controls.Add(this.naviGroup1);
            this.authGo1.Location = new System.Drawing.Point(647, 112);
            this.authGo1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.authGo1.Name = "authGo1";
            this.authGo1.ShowContent = false;
            this.authGo1.Size = new System.Drawing.Size(194, 143);
            this.authGo1.TabIndex = 57;
            this.authGo1.Visible = false;
            // 
            // grpResult
            // 
            this.grpResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpResult.Caption = "搜索";
            this.grpResult.ExpandedHeight = 254;
            this.grpResult.HeaderContextMenuStrip = null;
            this.grpResult.LayoutStyle = VirtualMachine.Component.WinControls.Controls.NavigationBarCtl.NavigationBar.NaviLayoutStyle.Office2003Blue;
            this.grpResult.Location = new System.Drawing.Point(-1, -1);
            this.grpResult.Name = "grpResult";
            this.grpResult.Padding = new System.Windows.Forms.Padding(1, 22, 1, 1);
            this.grpResult.Size = new System.Drawing.Size(196, 145);
            this.grpResult.TabIndex = 0;
            // 
            // naviGroup1
            // 
            this.naviGroup1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.naviGroup1.Caption = "搜索";
            this.naviGroup1.ExpandedHeight = 254;
            this.naviGroup1.HeaderContextMenuStrip = null;
            this.naviGroup1.LayoutStyle = VirtualMachine.Component.WinControls.Controls.NavigationBarCtl.NavigationBar.NaviLayoutStyle.Office2003Blue;
            this.naviGroup1.Location = new System.Drawing.Point(-1, -1);
            this.naviGroup1.Name = "naviGroup1";
            this.naviGroup1.Padding = new System.Windows.Forms.Padding(1, 22, 1, 1);
            this.naviGroup1.Size = new System.Drawing.Size(65, 36);
            this.naviGroup1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
           // this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer2.Size = new System.Drawing.Size(852, 166);
            this.splitContainer2.SplitterDistance = 500;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgBill);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(500, 166);
            this.groupBox2.TabIndex = 181;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "审批任务";
            // 
            // dgBill
            // 
            this.dgBill.AddDefaultMenu = false;
            this.dgBill.AddNoColumn = false;
            this.dgBill.AllowUserToAddRows = false;
            this.dgBill.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgBill.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgBill.CellBackColor = System.Drawing.SystemColors.Control;
            this.dgBill.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgBill.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgBill.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.collBillName,
            this.colCode,
            this.colCreatePerson,
            this.colCreateDate});
            this.dgBill.CustomBackColor = false;
            this.dgBill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgBill.EditCellBackColor = System.Drawing.Color.White;
            this.dgBill.EnterNext = VirtualMachine.Component.WinControls.Controls.EnterNextType.None;
            this.dgBill.FreezeFirstRow = false;
            this.dgBill.FreezeLastRow = false;
            this.dgBill.FrontColumnCount = 0;
            this.dgBill.GridColor = System.Drawing.SystemColors.WindowText;
            this.dgBill.HScrollOffset = 0;
            this.dgBill.IsAllowOrder = true;
            this.dgBill.IsConfirmDelete = true;
            this.dgBill.Location = new System.Drawing.Point(3, 17);
            this.dgBill.MultiSelect = false;
            this.dgBill.Name = "dgBill";
            this.dgBill.PageIndex = 0;
            this.dgBill.PageSize = 0;
            this.dgBill.Query = null;
            this.dgBill.ReadOnly = true;
            this.dgBill.ReadOnlyCols = ((System.Collections.IList)(resources.GetObject("dgBill.ReadOnlyCols")));
            this.dgBill.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgBill.RowHeadersWidth = 22;
            this.dgBill.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgBill.RowTemplate.Height = 23;
            this.dgBill.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgBill.Size = new System.Drawing.Size(494, 146);
            this.dgBill.TabIndex = 58;
            this.dgBill.TargetType = null;
            this.dgBill.VScrollOffset = 0;
            // 
            // collBillName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            this.collBillName.DefaultCellStyle = dataGridViewCellStyle1;
            this.collBillName.HeaderText = "任务类型";
            this.collBillName.Name = "collBillName";
            this.collBillName.ReadOnly = true;
            this.collBillName.Width = 200;
            // 
            // colCode
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            this.colCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.colCode.HeaderText = "任务号";
            this.colCode.Name = "colCode";
            this.colCode.ReadOnly = true;
            this.colCode.Width = 150;
            // 
            // colCreatePerson
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.colCreatePerson.DefaultCellStyle = dataGridViewCellStyle3;
            this.colCreatePerson.HeaderText = "提交人";
            this.colCreatePerson.Name = "colCreatePerson";
            this.colCreatePerson.ReadOnly = true;
            this.colCreatePerson.Width = 70;
            // 
            // colCreateDate
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            this.colCreateDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.colCreateDate.HeaderText = "提交日期";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.ReadOnly = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstBoxMsg);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(348, 166);
            this.groupBox3.TabIndex = 181;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "消息";
            // 
            // lstBoxMsg
            // 
            this.lstBoxMsg.BackColor = System.Drawing.SystemColors.Control;
            this.lstBoxMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBoxMsg.FormattingEnabled = true;
            this.lstBoxMsg.ItemHeight = 12;
            this.lstBoxMsg.Location = new System.Drawing.Point(3, 17);
            this.lstBoxMsg.Name = "lstBoxMsg";
            this.lstBoxMsg.Size = new System.Drawing.Size(342, 136);
            this.lstBoxMsg.TabIndex = 0;
            this.lstBoxMsg.BorderStyle = BorderStyle.None;
          
            // 
            // VStartPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 499);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "VStartPage";
            this.Text = "个人工作台";
            this.pnlFloor.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.authGo1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.naviGroup1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgBill)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private AuthManager.AuthMng.AuthControlsMng.AuthGo authGo1;
        private VirtualMachine.Component.WinControls.Controls.NavigationBarCtl.NavigationBar.NaviGroup grpResult;
        private VirtualMachine.Component.WinControls.Controls.NavigationBarCtl.NavigationBar.NaviGroup naviGroup1;
        private System.Windows.Forms.WebBrowser wbServer;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private CustomDataGridView dgBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn collBillName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatePerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreateDate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox lstBoxMsg;
    }
}