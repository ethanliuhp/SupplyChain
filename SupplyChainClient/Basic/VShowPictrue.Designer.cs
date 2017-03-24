namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    partial class VShowPictrue
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
            this.picVirtual = new System.Windows.Forms.PictureBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnOne = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.timerPic = new System.Windows.Forms.Timer(this.components);
            this.pnlFloor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picVirtual)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFloor
            // 
            this.pnlFloor.Controls.Add(this.btnEnd);
            this.pnlFloor.Controls.Add(this.btnOne);
            this.pnlFloor.Controls.Add(this.btnStop);
            this.pnlFloor.Controls.Add(this.btnStart);
            this.pnlFloor.Controls.Add(this.btnNext);
            this.pnlFloor.Controls.Add(this.btnPrevious);
            this.pnlFloor.Controls.Add(this.picVirtual);
            this.pnlFloor.Size = new System.Drawing.Size(723, 545);
            this.pnlFloor.Controls.SetChildIndex(this.picVirtual, 0);
            this.pnlFloor.Controls.SetChildIndex(this.lblTitle, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnPrevious, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnNext, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnStart, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnStop, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnOne, 0);
            this.pnlFloor.Controls.SetChildIndex(this.btnEnd, 0);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(336, 14);
            // 
            // picVirtual
            // 
            this.picVirtual.Location = new System.Drawing.Point(12, 40);
            this.picVirtual.Name = "picVirtual";
            this.picVirtual.Size = new System.Drawing.Size(100, 50);
            this.picVirtual.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picVirtual.TabIndex = 1;
            this.picVirtual.TabStop = false;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(304, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(58, 22);
            this.btnStop.TabIndex = 354;
            this.btnStop.Text = "暂停";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(234, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(58, 22);
            this.btnStart.TabIndex = 356;
            this.btnStart.Text = "播放";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(164, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(58, 22);
            this.btnNext.TabIndex = 355;
            this.btnNext.Text = "下张";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(94, 12);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(58, 22);
            this.btnPrevious.TabIndex = 353;
            this.btnPrevious.Text = "上张";
            this.btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnOne
            // 
            this.btnOne.Location = new System.Drawing.Point(545, 16);
            this.btnOne.Name = "btnOne";
            this.btnOne.Size = new System.Drawing.Size(58, 22);
            this.btnOne.TabIndex = 357;
            this.btnOne.Text = "首张";
            this.btnOne.UseVisualStyleBackColor = true;
            this.btnOne.Visible = false;
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(462, 16);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(58, 22);
            this.btnEnd.TabIndex = 358;
            this.btnEnd.Text = "末张";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Visible = false;
            // 
            // timerPic
            // 
            this.timerPic.Interval = 1000;
            // 
            // VShowPictrue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 545);
            this.Name = "VShowPictrue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VShowPictrue";
            this.pnlFloor.ResumeLayout(false);
            this.pnlFloor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picVirtual)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picVirtual;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnOne;
        private System.Windows.Forms.Timer timerPic;
    }
}