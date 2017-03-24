using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.Basic.Controls
{
    public partial class CustomCalculator : System.Windows.Forms.Form
    {
        #region 控件声明

        private System.Windows.Forms.TextBox txtShow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_rev;
        private System.Windows.Forms.Button btn_dot;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_equ;
        private System.Windows.Forms.Button btn_sign;
        private System.Windows.Forms.Button btn_sub;
        private System.Windows.Forms.Button btn_mul;
        private System.Windows.Forms.Button btn_0;
        private System.Windows.Forms.Button btn_3;
        private System.Windows.Forms.Button btn_2;
        private System.Windows.Forms.Button btn_1;
        private System.Windows.Forms.Button btn_6;
        private System.Windows.Forms.Button btn_5;
        private System.Windows.Forms.Button btn_4;
        private System.Windows.Forms.Button btn_sqrt;
        private System.Windows.Forms.Button btn_div;
        private System.Windows.Forms.Button btn_7;
        private System.Windows.Forms.Button btn_8;
        private System.Windows.Forms.Button btn_9;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.Button btn_sqr;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.Button c;
        private System.Windows.Forms.Button ce;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem10;
        private Button btnOk;
        private Button btnCancel;
        private IContainer components;


        public CustomCalculator()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();
            this.btnOk.Click += new EventHandler(btnOk_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            result = this.txtShow.Text;
            this.Close();
        }

        public object OpenCalc()
        {
            this.txtShow.Text = tmp.ToString();
            this.ShowDialog();
            return result;
        }



        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #region 各控件的属性的方法Windows Form Designer generated code
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtShow = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.c = new System.Windows.Forms.Button();
            this.ce = new System.Windows.Forms.Button();
            this.btn_rev = new System.Windows.Forms.Button();
            this.btn_dot = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_equ = new System.Windows.Forms.Button();
            this.btn_sign = new System.Windows.Forms.Button();
            this.btn_sub = new System.Windows.Forms.Button();
            this.btn_mul = new System.Windows.Forms.Button();
            this.btn_0 = new System.Windows.Forms.Button();
            this.btn_3 = new System.Windows.Forms.Button();
            this.btn_2 = new System.Windows.Forms.Button();
            this.btn_1 = new System.Windows.Forms.Button();
            this.btn_6 = new System.Windows.Forms.Button();
            this.btn_5 = new System.Windows.Forms.Button();
            this.btn_4 = new System.Windows.Forms.Button();
            this.btn_sqrt = new System.Windows.Forms.Button();
            this.btn_div = new System.Windows.Forms.Button();
            this.btn_7 = new System.Windows.Forms.Button();
            this.btn_8 = new System.Windows.Forms.Button();
            this.btn_9 = new System.Windows.Forms.Button();
            this.btn_sqr = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtShow
            // 
            this.txtShow.BackColor = System.Drawing.Color.White;
            this.txtShow.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtShow.ForeColor = System.Drawing.Color.Black;
            this.txtShow.Location = new System.Drawing.Point(25, 8);
            this.txtShow.Name = "txtShow";
            this.txtShow.Size = new System.Drawing.Size(228, 23);
            this.txtShow.TabIndex = 1;
            this.txtShow.Text = "0.";
            this.txtShow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.c);
            this.groupBox1.Controls.Add(this.ce);
            this.groupBox1.Controls.Add(this.btn_rev);
            this.groupBox1.Controls.Add(this.btn_dot);
            this.groupBox1.Controls.Add(this.btn_add);
            this.groupBox1.Controls.Add(this.btn_equ);
            this.groupBox1.Controls.Add(this.btn_sign);
            this.groupBox1.Controls.Add(this.btn_sub);
            this.groupBox1.Controls.Add(this.btn_mul);
            this.groupBox1.Controls.Add(this.btn_0);
            this.groupBox1.Controls.Add(this.btn_3);
            this.groupBox1.Controls.Add(this.btn_2);
            this.groupBox1.Controls.Add(this.btn_1);
            this.groupBox1.Controls.Add(this.btn_6);
            this.groupBox1.Controls.Add(this.btn_5);
            this.groupBox1.Controls.Add(this.btn_4);
            this.groupBox1.Controls.Add(this.btn_sqrt);
            this.groupBox1.Controls.Add(this.btn_div);
            this.groupBox1.Controls.Add(this.btn_7);
            this.groupBox1.Controls.Add(this.btn_8);
            this.groupBox1.Controls.Add(this.btn_9);
            this.groupBox1.Controls.Add(this.btn_sqr);
            this.groupBox1.Location = new System.Drawing.Point(24, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 184);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "计算机区";
            // 
            // c
            // 
            this.c.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.c.ForeColor = System.Drawing.Color.Red;
            this.c.Location = new System.Drawing.Point(178, 48);
            this.c.Name = "c";
            this.c.Size = new System.Drawing.Size(36, 61);
            this.c.TabIndex = 41;
            this.c.Text = "C";
            this.c.Click += new System.EventHandler(this.btn_Oper);
            // 
            // ce
            // 
            this.ce.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ce.ForeColor = System.Drawing.Color.Red;
            this.ce.Location = new System.Drawing.Point(138, 16);
            this.ce.Name = "ce";
            this.ce.Size = new System.Drawing.Size(76, 29);
            this.ce.TabIndex = 40;
            this.ce.Text = "CE";
            this.ce.Click += new System.EventHandler(this.btn_Oper);
            // 
            // btn_rev
            // 
            this.btn_rev.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_rev.ForeColor = System.Drawing.Color.Blue;
            this.btn_rev.Location = new System.Drawing.Point(178, 112);
            this.btn_rev.Name = "btn_rev";
            this.btn_rev.Size = new System.Drawing.Size(36, 29);
            this.btn_rev.TabIndex = 39;
            this.btn_rev.Text = "1/x";
            this.btn_rev.Click += new System.EventHandler(this.btn_Oper);
            // 
            // btn_dot
            // 
            this.btn_dot.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_dot.Location = new System.Drawing.Point(98, 144);
            this.btn_dot.Name = "btn_dot";
            this.btn_dot.Size = new System.Drawing.Size(36, 29);
            this.btn_dot.TabIndex = 38;
            this.btn_dot.Tag = "0";
            this.btn_dot.Text = ".";
            this.btn_dot.Click += new System.EventHandler(this.btn_Oper);
            // 
            // btn_add
            // 
            this.btn_add.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_add.ForeColor = System.Drawing.Color.Red;
            this.btn_add.Location = new System.Drawing.Point(138, 144);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(36, 29);
            this.btn_add.TabIndex = 37;
            this.btn_add.Text = "+";
            this.btn_add.Click += new System.EventHandler(this.btn_Oper);
            // 
            // btn_equ
            // 
            this.btn_equ.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_equ.ForeColor = System.Drawing.Color.Red;
            this.btn_equ.Location = new System.Drawing.Point(178, 144);
            this.btn_equ.Name = "btn_equ";
            this.btn_equ.Size = new System.Drawing.Size(36, 29);
            this.btn_equ.TabIndex = 36;
            this.btn_equ.Text = "=";
            this.btn_equ.Click += new System.EventHandler(this.btn_equ_Click);
            // 
            // btn_sign
            // 
            this.btn_sign.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sign.ForeColor = System.Drawing.Color.Blue;
            this.btn_sign.Location = new System.Drawing.Point(58, 144);
            this.btn_sign.Name = "btn_sign";
            this.btn_sign.Size = new System.Drawing.Size(36, 29);
            this.btn_sign.TabIndex = 35;
            this.btn_sign.Text = "+/-";
            this.btn_sign.Click += new System.EventHandler(this.btn_Oper);
            // 
            // btn_sub
            // 
            this.btn_sub.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sub.ForeColor = System.Drawing.Color.Red;
            this.btn_sub.Location = new System.Drawing.Point(138, 112);
            this.btn_sub.Name = "btn_sub";
            this.btn_sub.Size = new System.Drawing.Size(36, 29);
            this.btn_sub.TabIndex = 34;
            this.btn_sub.Text = "-";
            this.btn_sub.Click += new System.EventHandler(this.btn_Oper);
            // 
            // btn_mul
            // 
            this.btn_mul.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_mul.ForeColor = System.Drawing.Color.Red;
            this.btn_mul.Location = new System.Drawing.Point(138, 80);
            this.btn_mul.Name = "btn_mul";
            this.btn_mul.Size = new System.Drawing.Size(36, 29);
            this.btn_mul.TabIndex = 33;
            this.btn_mul.Text = "*";
            this.btn_mul.Click += new System.EventHandler(this.btn_Oper);
            // 
            // btn_0
            // 
            this.btn_0.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_0.ForeColor = System.Drawing.Color.Blue;
            this.btn_0.Location = new System.Drawing.Point(18, 144);
            this.btn_0.Name = "btn_0";
            this.btn_0.Size = new System.Drawing.Size(36, 29);
            this.btn_0.TabIndex = 32;
            this.btn_0.Tag = "0";
            this.btn_0.Text = "0";
            this.btn_0.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_3
            // 
            this.btn_3.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_3.ForeColor = System.Drawing.Color.Blue;
            this.btn_3.Location = new System.Drawing.Point(98, 112);
            this.btn_3.Name = "btn_3";
            this.btn_3.Size = new System.Drawing.Size(36, 29);
            this.btn_3.TabIndex = 31;
            this.btn_3.Tag = "3";
            this.btn_3.Text = "3";
            this.btn_3.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_2
            // 
            this.btn_2.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_2.ForeColor = System.Drawing.Color.Blue;
            this.btn_2.Location = new System.Drawing.Point(58, 112);
            this.btn_2.Name = "btn_2";
            this.btn_2.Size = new System.Drawing.Size(36, 29);
            this.btn_2.TabIndex = 30;
            this.btn_2.Tag = "2";
            this.btn_2.Text = "2";
            this.btn_2.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_1
            // 
            this.btn_1.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_1.ForeColor = System.Drawing.Color.Blue;
            this.btn_1.Location = new System.Drawing.Point(18, 112);
            this.btn_1.Name = "btn_1";
            this.btn_1.Size = new System.Drawing.Size(36, 29);
            this.btn_1.TabIndex = 29;
            this.btn_1.Tag = "1";
            this.btn_1.Text = "1";
            this.btn_1.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_6
            // 
            this.btn_6.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_6.ForeColor = System.Drawing.Color.Blue;
            this.btn_6.Location = new System.Drawing.Point(98, 80);
            this.btn_6.Name = "btn_6";
            this.btn_6.Size = new System.Drawing.Size(36, 29);
            this.btn_6.TabIndex = 28;
            this.btn_6.Tag = "6";
            this.btn_6.Text = "6";
            this.btn_6.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_5
            // 
            this.btn_5.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_5.ForeColor = System.Drawing.Color.Blue;
            this.btn_5.Location = new System.Drawing.Point(58, 80);
            this.btn_5.Name = "btn_5";
            this.btn_5.Size = new System.Drawing.Size(36, 29);
            this.btn_5.TabIndex = 27;
            this.btn_5.Tag = "5";
            this.btn_5.Text = "5";
            this.btn_5.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_4
            // 
            this.btn_4.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_4.ForeColor = System.Drawing.Color.Blue;
            this.btn_4.Location = new System.Drawing.Point(18, 80);
            this.btn_4.Name = "btn_4";
            this.btn_4.Size = new System.Drawing.Size(36, 29);
            this.btn_4.TabIndex = 26;
            this.btn_4.Tag = "4";
            this.btn_4.Text = "4";
            this.btn_4.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_sqrt
            // 
            this.btn_sqrt.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sqrt.ForeColor = System.Drawing.Color.Blue;
            this.btn_sqrt.Location = new System.Drawing.Point(18, 16);
            this.btn_sqrt.Name = "btn_sqrt";
            this.btn_sqrt.Size = new System.Drawing.Size(76, 29);
            this.btn_sqrt.TabIndex = 25;
            this.btn_sqrt.Text = "sqrt";
            this.btn_sqrt.Click += new System.EventHandler(this.btn_Oper);
            // 
            // btn_div
            // 
            this.btn_div.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_div.ForeColor = System.Drawing.Color.Red;
            this.btn_div.Location = new System.Drawing.Point(138, 48);
            this.btn_div.Name = "btn_div";
            this.btn_div.Size = new System.Drawing.Size(36, 29);
            this.btn_div.TabIndex = 24;
            this.btn_div.Text = "/";
            this.btn_div.Click += new System.EventHandler(this.btn_Oper);
            // 
            // btn_7
            // 
            this.btn_7.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_7.ForeColor = System.Drawing.Color.Blue;
            this.btn_7.Location = new System.Drawing.Point(18, 48);
            this.btn_7.Name = "btn_7";
            this.btn_7.Size = new System.Drawing.Size(36, 29);
            this.btn_7.TabIndex = 23;
            this.btn_7.Tag = "7";
            this.btn_7.Text = "7";
            this.btn_7.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_8
            // 
            this.btn_8.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_8.ForeColor = System.Drawing.Color.Blue;
            this.btn_8.Location = new System.Drawing.Point(58, 48);
            this.btn_8.Name = "btn_8";
            this.btn_8.Size = new System.Drawing.Size(36, 29);
            this.btn_8.TabIndex = 22;
            this.btn_8.Tag = "8";
            this.btn_8.Text = "8";
            this.btn_8.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_9
            // 
            this.btn_9.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_9.ForeColor = System.Drawing.Color.Blue;
            this.btn_9.Location = new System.Drawing.Point(98, 48);
            this.btn_9.Name = "btn_9";
            this.btn_9.Size = new System.Drawing.Size(36, 29);
            this.btn_9.TabIndex = 21;
            this.btn_9.Tag = "9";
            this.btn_9.Text = "9";
            this.btn_9.Click += new System.EventHandler(this.btn_0_Click);
            // 
            // btn_sqr
            // 
            this.btn_sqr.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_sqr.ForeColor = System.Drawing.Color.Blue;
            this.btn_sqr.Location = new System.Drawing.Point(98, 16);
            this.btn_sqr.Name = "btn_sqr";
            this.btn_sqr.Size = new System.Drawing.Size(36, 29);
            this.btn_sqr.TabIndex = 19;
            this.btn_sqr.Text = "sqr";
            this.btn_sqr.Click += new System.EventHandler(this.btn_Oper);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem8,
            this.menuItem4});
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 1;
            this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem9,
            this.menuItem10});
            this.menuItem8.Text = "文件(&F)";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 0;
            this.menuItem9.Text = "打开windows计算器(&O)";
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 1;
            this.menuItem10.Text = "退出(&Q)";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3});
            this.menuItem1.Text = "编辑(&E)";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "复制(&C)";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "粘贴(&P)";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5,
            this.menuItem6});
            this.menuItem4.Text = "帮助(&H)";
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.Text = "帮助主题(&H)";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "关于(&A)";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(83, 239);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 22;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(177, 239);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // CustomCalc
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(273, 266);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "CustomCalculator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算器";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region 各变量和常数的声明
        public const int NULL = 0;      // 定义操作码
        public const int ADD = 1;     //表示加
        public const int SUB = 2;     //减
        public const int MUL = 3;     //乘
        public const int DIV = 4;    //除
        public const int SQR = 5;    //求平方
        public const int SQRT = 6;   //求平方根
        public const int NODOT = 0;     // 定义是否点击了小数点,0 为没点
        public const int HASDOT = 1;
        private double res = 0;         // 记录结果数
        public double tmp = 0;         // 当前输入的操作数
        private int opt = NULL;         // 记录操作码
        private int dot = NODOT;    // 记录是否点击了小数点,0为没有点
        private int num = 0;        // 记录输入操作数的个数
        private int dotnum = 0;        // 记录小数点部分的个数        
        string strOper;            //获取操作符
        public object result = null;
        #endregion

        /// <summary>

        #region 获取操作数事件
        //获取操作数事件

        private void btn_0_Click(object sender, System.EventArgs e)
        {
            System.Windows.Forms.Button btnTmp;
            double i;

            btnTmp = sender as System.Windows.Forms.Button;
            if (btnTmp != null)
            {
                if (dot == NODOT)
                {
                    // 没有点击小数点
                    i = double.Parse(btnTmp.Tag.ToString()); //取用户自定义的控件关联数,并转换成double型
                    tmp = tmp * 10 + i;
                    txtShow.Text = tmp.ToString();   //将其放入文本显示屏啊
                }
                else   // 点击了小数点
                {
                    dotnum++;    //记录小数点部分的个数
                    // 生成小数部分的新的数值
                    i = double.Parse(btnTmp.Tag.ToString()) / System.Math.Pow(10, dotnum);
                    tmp = tmp + i;  //将小数点后的值加到当前操做数
                    txtShow.Text = tmp.ToString();
                }
            }
        }
        #endregion

        #region 等于事件和等于运算方法
        //等于事件
        private void btn_equ_Click(object sender, System.EventArgs e)
        {
            calc();
        }

        //等于运算方法
        private void calc()
        {
            // 生成结果
            if (num == 0)    //是否有操作数,没有就返回0
            {
                res = 0;
                tmp = 0;
                txtShow.Text = res.ToString();
                return;
            }

            switch (opt)  //找到对应的运算符进行计算
            {
                // 加法
                case ADD:
                    res = res + tmp;
                    break;
                // 减法
                case SUB:
                    res = res - tmp;
                    break;
                // 乘法
                case MUL:
                    res = res * tmp;
                    break;
                // 除法
                case DIV:
                    res = res / tmp;
                    break;
                // 平方
                case SQR:
                    res = tmp * tmp;
                    break;
                // 平方根
                case SQRT:
                    res = System.Math.Sqrt(tmp);
                    break;
                default:
                    return;
            }
            txtShow.Text = res.ToString();   //结果输出到文本显示屏
            opt = NULL;  //运算符清空
            tmp = 0;
            dot = NODOT;
            //res = 0;
            //num = 0;
        }
        #endregion

        #region 获取操作符运事件
        //获取操作符运事件
        private void btn_Oper(object obj, EventArgs ea)
        {
            Button tmp1 = (Button)obj;
            strOper = tmp1.Text;
            switch (strOper)
            {
                case "/":       //除法运算
                    if (opt != NULL && opt != DIV)
                    {
                        calc();
                    }
                    opt = DIV;
                    if (num != 0)  //判断操作数的个数,如果两个就做二元运算
                    {
                        if (tmp != 0)
                            res = res / tmp;
                    }
                    else
                        res = tmp;
                    num++;
                    tmp = 0;
                    txtShow.Text = res.ToString();
                    dot = NODOT;
                    break;
                case "*":
                    // 乘法运算
                    if (opt != NULL && opt != MUL)
                    {
                        calc();
                    }

                    opt = MUL;

                    if (num != 0)     //判断操作数的个数,如果两个就做二元运算    
                    {
                        if (tmp != 0)
                            res = res * tmp;
                    }
                    else
                        res = tmp;

                    num++;
                    tmp = 0;
                    txtShow.Text = res.ToString();
                    dot = NODOT;
                    break;
                case "+":            //加法运算
                    if (opt != NULL && opt != ADD)
                    {
                        calc();
                    }
                    opt = ADD;
                    if (num != 0)    //判断操作数的个数,如果两个就做二元运算
                        res = res + tmp;
                    else
                        res = tmp;
                    num++;
                    tmp = 0;
                    txtShow.Text = res.ToString();
                    dot = NODOT;
                    break;
                case "-":        //减法运算
                    if (opt != NULL && opt != SUB)
                    {
                        calc();
                    }
                    /*if(opt==ADD)
                    {
                        res=res+tmp;
                        tmp=0;
                    }*/
                    opt = SUB;
                    if (num != 0)    //判断操作数的个数,如果两个就做二元运算
                        res = res - tmp;
                    else
                        res = tmp;
                    num++;
                    tmp = 0;
                    txtShow.Text = res.ToString();
                    dot = NODOT;
                    break;
                case "sqrt":     //平方根运算
                    if (opt != NULL)
                    {
                        calc();
                    }

                    //opt=SQRT;
                    if (tmp > 0)  //要求操作数大于0
                    {
                        res = Math.Sqrt(tmp);
                        //res=tmp;
                    }
                    else if (res > 0)
                        res = Math.Sqrt(res);
                    txtShow.Text = res.ToString();
                    num++;
                    tmp = 0;
                    dot = NODOT;
                    break;
                case "sqr":
                    // 平方运算
                    if (opt != NULL)
                    {
                        calc();
                    }
                    //opt=SQR;
                    if (tmp != 0)
                    {
                        res = tmp * tmp;
                        //res=tmp;
                    }
                    else
                        res = res * res;

                    txtShow.Text = res.ToString();
                    num++;
                    tmp = 0;
                    dot = NODOT;
                    break;
                case "1/x":  //倒数运算
                    if (opt != NULL)
                    {
                        calc();
                    }
                    if (tmp != 0)
                    {
                        res = 1 / tmp;
                        //res=tmp;
                    }
                    else
                        res = 1 / res;
                    txtShow.Text = res.ToString();
                    tmp = 0;
                    dot = NODOT;
                    break;
                case ".":
                    // 点击了小数点
                    if (dot == HASDOT)
                        return;
                    else
                    {
                        dot = HASDOT;
                        dotnum = 0;
                    }
                    break;
                case "+/-":
                    // 点击了符号运算
                    if (tmp != 0)
                    {
                        tmp = -tmp;
                        txtShow.Text = tmp.ToString();
                    }
                    else
                    {
                        res = -res;
                        //res=tmp;
                        txtShow.Text = res.ToString();
                    }
                    dot = NODOT;
                    break;
                case "CE":     //清除运算
                    res = 0;         // 记录结果数
                    tmp = 0;         // 当前输入的操作数
                    opt = NULL;         // 记录操作码
                    dot = NODOT;    // 记录是否点击了小数点
                    num = 0;        // 记录输入操作数的个数
                    dotnum = 0;        // 记录小数点部分的个数
                    txtShow.Text = "";
                    break;
                case "C":         //清除运算
                    res = 0;         // 记录结果数
                    tmp = 0;         // 当前输入的操作数
                    opt = NULL;         // 记录操作码
                    dot = NODOT;    // 记录是否点击了小数点
                    num = 0;        // 记录输入操作数的个数
                    dotnum = 0;        // 记录小数点部分的个数
                    txtShow.Text = "";
                    break;

            }
        }

        #endregion

        #region     主菜单事件
        //打开关于主题---调用windows xp中计算器的帮助
        private void menuItem5_Click(object sender, System.EventArgs e)
        {
            Help.ShowHelp(this, "C:\\WINDOWS\\Help\\calc.chm");
        }



        //打开于我们
        private void menuItem6_Click(object sender, System.EventArgs e)
        {

        }

        //复制
        private void menuItem2_Click(object sender, System.EventArgs e)
        {
            if (txtShow.SelectionLength > 0)
            {
                txtShow.Copy();
            }
        }
        //
        //粘贴
        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            txtShow.Paste();
        }

        //调用windows xp中的计算器
        private void menuItem9_Click(object sender, System.EventArgs e)
        {
            Help.ShowHelp(this, "C:\\WINDOWS\\system32\\calc.exe");
        }

        #endregion
    }
}
