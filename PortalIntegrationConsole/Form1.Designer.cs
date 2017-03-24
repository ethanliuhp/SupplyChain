namespace PortalIntegrationConsole
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblServerState = new System.Windows.Forms.Label();
            this.gridLog = new System.Windows.Forms.DataGridView();
            this.colContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtReTryNumber = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStartPlan = new System.Windows.Forms.Button();
            this.dateTimePickerExeTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.timerDataSyncPlan = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblTryStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtRepExeTryNumber = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStartRepExePlan = new System.Windows.Forms.Button();
            this.dateTimePickerRepExeTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(254, 43);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(99, 36);
            this.btnStartServer.TabIndex = 0;
            this.btnStartServer.Text = "启动服务";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(17, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "服务当前状态：";
            // 
            // lblServerState
            // 
            this.lblServerState.AutoSize = true;
            this.lblServerState.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblServerState.ForeColor = System.Drawing.Color.Red;
            this.lblServerState.Location = new System.Drawing.Point(139, 52);
            this.lblServerState.Name = "lblServerState";
            this.lblServerState.Size = new System.Drawing.Size(42, 16);
            this.lblServerState.TabIndex = 2;
            this.lblServerState.Text = "停止";
            // 
            // gridLog
            // 
            this.gridLog.AllowUserToAddRows = false;
            this.gridLog.AllowUserToDeleteRows = false;
            this.gridLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colContent});
            this.gridLog.Location = new System.Drawing.Point(12, 125);
            this.gridLog.Name = "gridLog";
            this.gridLog.RowTemplate.Height = 23;
            this.gridLog.Size = new System.Drawing.Size(990, 342);
            this.gridLog.TabIndex = 3;
            // 
            // colContent
            // 
            this.colContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colContent.HeaderText = "日志内容";
            this.colContent.MinimumWidth = 200;
            this.colContent.Name = "colContent";
            this.colContent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtReTryNumber);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnStartPlan);
            this.groupBox1.Controls.Add(this.dateTimePickerExeTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(614, 55);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "计划执行1次";
            // 
            // txtReTryNumber
            // 
            this.txtReTryNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtReTryNumber.FormattingEnabled = true;
            this.txtReTryNumber.Location = new System.Drawing.Point(441, 21);
            this.txtReTryNumber.Name = "txtReTryNumber";
            this.txtReTryNumber.Size = new System.Drawing.Size(60, 20);
            this.txtReTryNumber.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(231, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "执行1次，失败时重试次数：";
            // 
            // btnStartPlan
            // 
            this.btnStartPlan.Location = new System.Drawing.Point(507, 18);
            this.btnStartPlan.Name = "btnStartPlan";
            this.btnStartPlan.Size = new System.Drawing.Size(81, 25);
            this.btnStartPlan.TabIndex = 4;
            this.btnStartPlan.Text = "启动计划";
            this.btnStartPlan.UseVisualStyleBackColor = true;
            this.btnStartPlan.Click += new System.EventHandler(this.btnStartPlan_Click);
            // 
            // dateTimePickerExeTime
            // 
            this.dateTimePickerExeTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerExeTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerExeTime.Location = new System.Drawing.Point(73, 21);
            this.dateTimePickerExeTime.Name = "dateTimePickerExeTime";
            this.dateTimePickerExeTime.Size = new System.Drawing.Size(157, 21);
            this.dateTimePickerExeTime.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(14, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "计划在";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnStartServer);
            this.groupBox2.Controls.Add(this.lblServerState);
            this.groupBox2.Location = new System.Drawing.Point(632, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(370, 116);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "即时执行";
            // 
            // timerDataSyncPlan
            // 
            this.timerDataSyncPlan.Interval = 1000;
            this.timerDataSyncPlan.Tick += new System.EventHandler(this.timerDataSyncPlan_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTryStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 470);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1014, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblTryStatus
            // 
            this.lblTryStatus.Name = "lblTryStatus";
            this.lblTryStatus.Size = new System.Drawing.Size(56, 17);
            this.lblTryStatus.Text = "重试状态";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtRepExeTryNumber);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.btnStartRepExePlan);
            this.groupBox3.Controls.Add(this.dateTimePickerRepExeTime);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(12, 64);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(614, 55);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "计划重复执行";
            // 
            // txtRepExeTryNumber
            // 
            this.txtRepExeTryNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtRepExeTryNumber.FormattingEnabled = true;
            this.txtRepExeTryNumber.Location = new System.Drawing.Point(441, 21);
            this.txtRepExeTryNumber.Name = "txtRepExeTryNumber";
            this.txtRepExeTryNumber.Size = new System.Drawing.Size(60, 20);
            this.txtRepExeTryNumber.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(231, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(195, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "执行，失败时重试次数：";
            // 
            // btnStartRepExePlan
            // 
            this.btnStartRepExePlan.Location = new System.Drawing.Point(507, 18);
            this.btnStartRepExePlan.Name = "btnStartRepExePlan";
            this.btnStartRepExePlan.Size = new System.Drawing.Size(81, 25);
            this.btnStartRepExePlan.TabIndex = 4;
            this.btnStartRepExePlan.Text = "启动计划";
            this.btnStartRepExePlan.UseVisualStyleBackColor = true;
            this.btnStartRepExePlan.Click += new System.EventHandler(this.btnStartRepExePlan_Click);
            // 
            // dateTimePickerRepExeTime
            // 
            this.dateTimePickerRepExeTime.CustomFormat = "";
            this.dateTimePickerRepExeTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerRepExeTime.Location = new System.Drawing.Point(113, 21);
            this.dateTimePickerRepExeTime.Name = "dateTimePickerRepExeTime";
            this.dateTimePickerRepExeTime.Size = new System.Drawing.Size(117, 21);
            this.dateTimePickerRepExeTime.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(14, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "计划在每天";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 492);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gridLog);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "门户集成控制台";
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblServerState;
        private System.Windows.Forms.DataGridView gridLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStartPlan;
        private System.Windows.Forms.DateTimePicker dateTimePickerExeTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer timerDataSyncPlan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox txtReTryNumber;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblTryStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContent;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox txtRepExeTryNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStartRepExePlan;
        private System.Windows.Forms.DateTimePicker dateTimePickerRepExeTime;
        private System.Windows.Forms.Label label5;
    }
}

