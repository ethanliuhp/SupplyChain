namespace Application.Business.Erp.SupplyChain.Client.ExcelImportMng
{
    partial class VFlexCellImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VFlexCellImport));
            this.lblText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRowCount = new System.Windows.Forms.TextBox();
            this.btnInsertRows = new System.Windows.Forms.Button();
            this.btnCancel1 = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.flexGrid = new VirtualMachine.Component.WinControls.Controls.CustomFlexGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(5, 9);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(17, 12);
            this.lblText.TabIndex = 19;
            this.lblText.Text = ">>";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 493);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "行";
            // 
            // txtRowCount
            // 
            this.txtRowCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRowCount.Location = new System.Drawing.Point(51, 489);
            this.txtRowCount.Name = "txtRowCount";
            this.txtRowCount.Size = new System.Drawing.Size(68, 21);
            this.txtRowCount.TabIndex = 17;
            // 
            // btnInsertRows
            // 
            this.btnInsertRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInsertRows.Location = new System.Drawing.Point(166, 488);
            this.btnInsertRows.Name = "btnInsertRows";
            this.btnInsertRows.Size = new System.Drawing.Size(75, 23);
            this.btnInsertRows.TabIndex = 16;
            this.btnInsertRows.Text = "添加";
            this.btnInsertRows.UseVisualStyleBackColor = true;
            // 
            // btnCancel1
            // 
            this.btnCancel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel1.Location = new System.Drawing.Point(606, 488);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size(75, 23);
            this.btnCancel1.TabIndex = 15;
            this.btnCancel1.Text = "取消";
            this.btnCancel1.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(247, 488);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(505, 488);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // flexGrid
            // 
            this.flexGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flexGrid.CheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid.CheckedImage")));
            this.flexGrid.Cols = 25;
            this.flexGrid.DefaultFont = new System.Drawing.Font("宋体", 9F);
            this.flexGrid.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flexGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.flexGrid.Location = new System.Drawing.Point(7, 24);
            this.flexGrid.Name = "flexGrid";
            this.flexGrid.Size = new System.Drawing.Size(980, 456);
            this.flexGrid.TabIndex = 20;
            this.flexGrid.UncheckedImage = ((System.Drawing.Bitmap)(resources.GetObject("flexGrid.UncheckedImage")));
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 493);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "添加";
            // 
            // VFlexCellImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 522);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flexGrid);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRowCount);
            this.Controls.Add(this.btnInsertRows);
            this.Controls.Add(this.btnCancel1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Name = "VFlexCellImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量添加";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRowCount;
        private System.Windows.Forms.Button btnInsertRows;
        private System.Windows.Forms.Button btnCancel1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private VirtualMachine.Component.WinControls.Controls.CustomFlexGrid flexGrid;
        private System.Windows.Forms.Label label2;
    }
}