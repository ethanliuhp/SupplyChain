namespace Application.Business.Erp.SupplyChain.Client.Basic.Template
{
    partial class TBasicToolBarByMobile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TBasicToolBarByMobile));
            this.pnlFloor = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TBtnPageDown = new System.Windows.Forms.ToolStripButton();
            this.TBtnPageUp = new System.Windows.Forms.ToolStripButton();
            this.TBtnContents = new System.Windows.Forms.ToolStripDropDownButton();
            this.功能菜单6Item = new System.Windows.Forms.ToolStripMenuItem();
            this.功能菜单5Item = new System.Windows.Forms.ToolStripMenuItem();
            this.功能菜单4Item = new System.Windows.Forms.ToolStripMenuItem();
            this.功能菜单3Item = new System.Windows.Forms.ToolStripMenuItem();
            this.功能菜单2Item = new System.Windows.Forms.ToolStripMenuItem();
            this.功能菜单1Item = new System.Windows.Forms.ToolStripMenuItem();
            this.返回主菜单Item = new System.Windows.Forms.ToolStripMenuItem();
            this.退出选择Item = new System.Windows.Forms.ToolStripMenuItem();
            this.lblHeaderLine = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.pnlFloor.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlFloor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFloor.Controls.Add(this.toolStrip1);
            this.pnlFloor.Controls.Add(this.lblHeaderLine);
            this.pnlFloor.Controls.Add(this.lblTitle);
            this.pnlFloor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFloor.Location = new System.Drawing.Point(0, 0);
            this.pnlFloor.Name = "pnlFloor";
            this.pnlFloor.Size = new System.Drawing.Size(449, 377);
            this.pnlFloor.TabIndex = 1;
            this.pnlFloor.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFloor_Paint);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.PowderBlue;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TBtnPageDown,
            this.TBtnPageUp,
            this.TBtnContents});
            this.toolStrip1.Location = new System.Drawing.Point(0, 323);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(447, 52);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TBtnPageDown
            // 
            this.TBtnPageDown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TBtnPageDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBtnPageDown.Image = ((System.Drawing.Image)(resources.GetObject("TBtnPageDown.Image")));
            this.TBtnPageDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TBtnPageDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBtnPageDown.Margin = new System.Windows.Forms.Padding(0, 1, 2, 2);
            this.TBtnPageDown.Name = "TBtnPageDown";
            this.TBtnPageDown.Size = new System.Drawing.Size(100, 49);
            this.TBtnPageDown.Text = "下一页";
            // 
            // TBtnPageUp
            // 
            this.TBtnPageUp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.TBtnPageUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBtnPageUp.Image = ((System.Drawing.Image)(resources.GetObject("TBtnPageUp.Image")));
            this.TBtnPageUp.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TBtnPageUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBtnPageUp.Margin = new System.Windows.Forms.Padding(40, 1, 80, 2);
            this.TBtnPageUp.Name = "TBtnPageUp";
            this.TBtnPageUp.Size = new System.Drawing.Size(100, 49);
            this.TBtnPageUp.Text = "上一页";
            // 
            // TBtnContents
            // 
            this.TBtnContents.BackColor = System.Drawing.Color.Transparent;
            this.TBtnContents.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TBtnContents.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.功能菜单6Item,
            this.功能菜单5Item,
            this.功能菜单4Item,
            this.功能菜单3Item,
            this.功能菜单2Item,
            this.功能菜单1Item,
            this.返回主菜单Item,
            this.退出选择Item});
            this.TBtnContents.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBtnContents.Image = ((System.Drawing.Image)(resources.GetObject("TBtnContents.Image")));
            this.TBtnContents.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.TBtnContents.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TBtnContents.Name = "TBtnContents";
            this.TBtnContents.Size = new System.Drawing.Size(109, 49);
            this.TBtnContents.Text = "功能菜单";
            // 
            // 功能菜单6Item
            // 
            this.功能菜单6Item.Font = new System.Drawing.Font("Courier New", 36F, System.Drawing.FontStyle.Bold);
            this.功能菜单6Item.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.功能菜单6Item.Name = "功能菜单6Item";
            this.功能菜单6Item.Size = new System.Drawing.Size(356, 58);
            this.功能菜单6Item.Text = "功能菜单6";
            this.功能菜单6Item.Visible = false;
            this.功能菜单6Item.Click += new System.EventHandler(this.功能菜单6Item_Click);
            // 
            // 功能菜单5Item
            // 
            this.功能菜单5Item.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.功能菜单5Item.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.功能菜单5Item.Name = "功能菜单5Item";
            this.功能菜单5Item.Size = new System.Drawing.Size(356, 58);
            this.功能菜单5Item.Text = "功能菜单5";
            this.功能菜单5Item.Visible = false;
            this.功能菜单5Item.Click += new System.EventHandler(this.功能菜单5Item_Click);
            // 
            // 功能菜单4Item
            // 
            this.功能菜单4Item.BackColor = System.Drawing.Color.Transparent;
            this.功能菜单4Item.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.功能菜单4Item.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.功能菜单4Item.Name = "功能菜单4Item";
            this.功能菜单4Item.Size = new System.Drawing.Size(356, 58);
            this.功能菜单4Item.Text = "功能菜单4";
            this.功能菜单4Item.Visible = false;
            this.功能菜单4Item.Click += new System.EventHandler(this.功能菜单4Item_Click);
            // 
            // 功能菜单3Item
            // 
            this.功能菜单3Item.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.功能菜单3Item.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.功能菜单3Item.Name = "功能菜单3Item";
            this.功能菜单3Item.Size = new System.Drawing.Size(356, 58);
            this.功能菜单3Item.Text = "功能菜单3";
            this.功能菜单3Item.Visible = false;
            this.功能菜单3Item.Click += new System.EventHandler(this.功能菜单3Item_Click);
            // 
            // 功能菜单2Item
            // 
            this.功能菜单2Item.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.功能菜单2Item.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.功能菜单2Item.Name = "功能菜单2Item";
            this.功能菜单2Item.Size = new System.Drawing.Size(356, 58);
            this.功能菜单2Item.Text = "功能菜单2";
            this.功能菜单2Item.Visible = false;
            this.功能菜单2Item.Click += new System.EventHandler(this.功能菜单2Item_Click);
            // 
            // 功能菜单1Item
            // 
            this.功能菜单1Item.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.功能菜单1Item.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.功能菜单1Item.Name = "功能菜单1Item";
            this.功能菜单1Item.Size = new System.Drawing.Size(356, 58);
            this.功能菜单1Item.Text = "功能菜单1";
            this.功能菜单1Item.Visible = false;
            this.功能菜单1Item.Click += new System.EventHandler(this.功能菜单1Item_Click);
            // 
            // 返回主菜单Item
            // 
            this.返回主菜单Item.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.返回主菜单Item.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.返回主菜单Item.Name = "返回主菜单Item";
            this.返回主菜单Item.Size = new System.Drawing.Size(356, 58);
            this.返回主菜单Item.Text = "返回主菜单";
            this.返回主菜单Item.Click += new System.EventHandler(this.返回主菜单Item_Click);
            // 
            // 退出选择Item
            // 
            this.退出选择Item.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.退出选择Item.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.退出选择Item.Name = "退出选择Item";
            this.退出选择Item.Size = new System.Drawing.Size(356, 58);
            this.退出选择Item.Text = "关闭窗口";
            this.退出选择Item.Click += new System.EventHandler(this.退出选择Item_Click);
            // 
            // lblHeaderLine
            // 
            this.lblHeaderLine.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeaderLine.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Bold);
            this.lblHeaderLine.ForeColor = System.Drawing.SystemColors.Window;
            this.lblHeaderLine.Location = new System.Drawing.Point(0, 0);
            this.lblHeaderLine.Name = "lblHeaderLine";
            this.lblHeaderLine.Size = new System.Drawing.Size(447, 40);
            this.lblHeaderLine.TabIndex = 2;
            this.lblHeaderLine.Text = "lblHeaderLine";
            this.lblHeaderLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("楷体_GB2312", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTitle.Location = new System.Drawing.Point(177, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(51, 20);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "标题";
            this.lblTitle.Visible = false;
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // TBasicToolBarByMobile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(449, 377);
            this.Controls.Add(this.pnlFloor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TBasicToolBarByMobile";
            this.Load += new System.EventHandler(this.TBasicDataView_Load);
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton TBtnContents;
        public System.Windows.Forms.ToolStripMenuItem 返回主菜单Item;
        public System.Windows.Forms.ToolStripMenuItem 退出选择Item;
        public System.Windows.Forms.ToolStripMenuItem 功能菜单6Item;
        public System.Windows.Forms.ToolStripMenuItem 功能菜单5Item;
        public System.Windows.Forms.ToolStripMenuItem 功能菜单4Item;
        public System.Windows.Forms.ToolStripMenuItem 功能菜单3Item;
        public System.Windows.Forms.ToolStripMenuItem 功能菜单2Item;
        public System.Windows.Forms.ToolStripMenuItem 功能菜单1Item;
        public System.Windows.Forms.ToolStripButton TBtnPageDown;
        public Sunisoft.IrisSkin.SkinEngine skinEngine1;
        public System.Windows.Forms.ToolStripButton TBtnPageUp;
        public System.Windows.Forms.Label lblHeaderLine;
    }
}