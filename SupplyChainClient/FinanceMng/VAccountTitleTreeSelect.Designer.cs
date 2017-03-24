namespace Application.Business.Erp.SupplyChain.Client.FinanceMng
{
    partial class VAccountTitleTreeSelect
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
            this.tvTitle = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.pnBottom = new VirtualMachine.Component.WinControls.Controls.CustomPanel();
            this.btnCancel = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.btnOK = new VirtualMachine.Component.WinControls.Controls.CustomButton();
            this.pnBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvTitle
            // 
            this.tvTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTitle.HideSelection = false;
            this.tvTitle.Location = new System.Drawing.Point(0, 0);
            this.tvTitle.Name = "tvTitle";
            this.tvTitle.Size = new System.Drawing.Size(349, 331);
            this.tvTitle.TabIndex = 0;
            // 
            // pnBottom
            // 
            this.pnBottom.Controls.Add(this.btnCancel);
            this.pnBottom.Controls.Add(this.btnOK);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.ExpandBorderColor = System.Drawing.SystemColors.ControlDark;
            this.pnBottom.ExpandBorderStyle = VirtualMachine.Component.WinControls.Controls.ExpandBorder.None;
            this.pnBottom.ExpandBorderWidth = 1F;
            this.pnBottom.Location = new System.Drawing.Point(0, 331);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(349, 49);
            this.pnBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.ButtonType = VirtualMachine.Component.WinControls.Controls.ButtonType.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnCancel.Location = new System.Drawing.Point(198, 18);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(66, 22);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.ButtonType = VirtualMachine.Component.WinControls.Controls.ButtonType.Confirm;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.FontSize = VirtualMachine.Component.WinControls.Style.FontSize.Default;
            this.btnOK.Location = new System.Drawing.Point(86, 18);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(66, 22);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定(&K)";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // VAccountTitleTreeSelect
            // 
            this.AcceptButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(349, 380);
            this.Controls.Add(this.tvTitle);
            this.Controls.Add(this.pnBottom);
            this.Name = "VAccountTitleTreeSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择会计科目";
            this.pnBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomTreeView tvTitle;
        private VirtualMachine.Component.WinControls.Controls.CustomPanel pnBottom;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnCancel;
        private VirtualMachine.Component.WinControls.Controls.CustomButton btnOK;
    }
}
