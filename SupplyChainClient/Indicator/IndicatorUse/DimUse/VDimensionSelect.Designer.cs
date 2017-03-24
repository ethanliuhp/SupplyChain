namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse
{
    partial class VDimensionSelect
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("模板属性树");
            this.TVCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.cmsStyleMx = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.upMove = new System.Windows.Forms.ToolStripMenuItem();
            this.downMove = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSelectAll = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnUnSelectAll = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOk = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.cmsStyleMx.SuspendLayout();
            this.SuspendLayout();
            // 
            // TVCategory
            // 
            this.TVCategory.CheckBoxes = true;
            this.TVCategory.ContextMenuStrip = this.cmsStyleMx;
            this.TVCategory.HideSelection = false;
            this.TVCategory.Location = new System.Drawing.Point(12, 12);
            this.TVCategory.Name = "TVCategory";
            treeNode1.Name = "dimRoot";
            treeNode1.Text = "模板属性树";
            this.TVCategory.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.TVCategory.Size = new System.Drawing.Size(318, 310);
            this.TVCategory.TabIndex = 0;
            // 
            // cmsStyleMx
            // 
            this.cmsStyleMx.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upMove,
            this.downMove});
            this.cmsStyleMx.Name = "cmsStyleMx";
            this.cmsStyleMx.Size = new System.Drawing.Size(95, 48);
            // 
            // upMove
            // 
            this.upMove.Name = "upMove";
            this.upMove.Size = new System.Drawing.Size(94, 22);
            this.upMove.Text = "上移";
            // 
            // downMove
            // 
            this.downMove.Name = "downMove";
            this.downMove.Size = new System.Drawing.Size(94, 22);
            this.downMove.Text = "下移";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelectAll.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectAll.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnSelectAll.Location = new System.Drawing.Point(12, 328);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "全 选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            // 
            // btnUnSelectAll
            // 
            this.btnUnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnSelectAll.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUnSelectAll.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnUnSelectAll.Location = new System.Drawing.Point(93, 328);
            this.btnUnSelectAll.Name = "btnUnSelectAll";
            this.btnUnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnUnSelectAll.TabIndex = 2;
            this.btnUnSelectAll.Text = "取消全选";
            this.btnUnSelectAll.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOk.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOk.Location = new System.Drawing.Point(174, 328);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "确 定";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(255, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取 消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // VDimensionSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 361);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnUnSelectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.TVCategory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "VDimensionSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "视图维度选择界面";
            this.cmsStyleMx.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomTreeView TVCategory;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnSelectAll;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnUnSelectAll;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOk;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private System.Windows.Forms.ContextMenuStrip cmsStyleMx;
        private System.Windows.Forms.ToolStripMenuItem upMove;
        private System.Windows.Forms.ToolStripMenuItem downMove;
    }
}