namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    partial class VPaymentMngIntial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VPaymentMngIntial));
            this.panel1 = new System.Windows.Forms.Panel();
            this.flexGrid = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRowCount = new System.Windows.Forms.TextBox();
            this.btnInsertRows = new System.Windows.Forms.Button();
            this.btnCancel1 = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.pnlFloor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.label1);
            this.pnlFloor.Controls.Add(this.txtRowCount);
            this.pnlFloor.Controls.Add(this.btnInsertRows);
            this.pnlFloor.Controls.Add(this.btnCancel1);
            this.pnlFloor.Controls.Add(this.btnDelete);
            this.pnlFloor.Controls.Add(this.btnAdd);
            this.pnlFloor.Controls.Add(this.btnSure);
            this.pnlFloor.Controls.Add(this.panel1);
            this.pnlFloor.Size = new System.Drawing.Size(1035, 499);
            this.pnlFloor.Controls.SetChildIndex(this.panel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnSure, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnAdd, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnDelete, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnCancel1, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnInsertRows, 0);
            this.pnlFloor.Controls.SetChildIndex(this.txtRowCount, 0);
            this.pnlFloor.Controls.SetChildIndex(this.label1, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(169, 20);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.flexGrid);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1020, 457);
            this.panel1.TabIndex = 16;
            // 
            // flexGrid
            // 
            this.flexGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid.CheckedImage")));
            this.flexGrid.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.flexGrid.DisplayRowNumber = true;
            this.flexGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flexGrid.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGrid.Location = new System.Drawing.Point(0, 0);
            this.flexGrid.Name = "flexGrid";
            this.flexGrid.Size = new System.Drawing.Size(1020, 457);
            this.flexGrid.TabIndex = 6;
            this.flexGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid.UncheckedImage")));
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 480);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "行";
            // 
            // txtRowCount
            // 
            this.txtRowCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtRowCount.Location = new System.Drawing.Point(85, 475);
            this.txtRowCount.Name = "txtRowCount";
            this.txtRowCount.Size = new System.Drawing.Size(33, 21);
            this.txtRowCount.TabIndex = 22;
            // 
            // btnInsertRows
            // 
            this.btnInsertRows.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnInsertRows.Location = new System.Drawing.Point(47, 473);
            this.btnInsertRows.Name = "btnInsertRows";
            this.btnInsertRows.Size = new System.Drawing.Size(37, 23);
            this.btnInsertRows.TabIndex = 21;
            this.btnInsertRows.Text = "插入";
            this.btnInsertRows.UseVisualStyleBackColor = true;
            // 
            // btnCancel1
            // 
            this.btnCancel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel1.Location = new System.Drawing.Point(463, 473);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size(75, 23);
            this.btnCancel1.TabIndex = 20;
            this.btnCancel1.Text = "取消";
            this.btnCancel1.UseVisualStyleBackColor = true;
            this.btnCancel1.Visible = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.Location = new System.Drawing.Point(251, 473);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 19;
            this.btnDelete.Text = "删除单行";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAdd.Location = new System.Drawing.Point(152, 473);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "添加单行";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnSure
            // 
            this.btnSure.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSure.Location = new System.Drawing.Point(368, 473);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 17;
            this.btnSure.Text = "确定导入";
            this.btnSure.UseVisualStyleBackColor = true;
            // 
            // VPaymentMngIntial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 499);
            this.Name = "VPaymentMngIntial";
            this.Text = "付款单(工程款)初始化";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRowCount;
        private System.Windows.Forms.Button btnInsertRows;
        private System.Windows.Forms.Button btnCancel1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSure;

    }
}