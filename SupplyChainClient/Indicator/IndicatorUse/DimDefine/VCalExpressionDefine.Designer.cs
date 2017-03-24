using System.Windows.Forms;
namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimDefine
{
    partial class VCalExpressionDefine
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
            this.TVCategory = new VirtualMachine.Component.WinControls.Controls.CustomTreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCalExp = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnRightBracket = new System.Windows.Forms.Button();
            this.btnLeftBracket = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnMod = new System.Windows.Forms.Button();
            this.btnDot = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnMulti = new System.Windows.Forms.Button();
            this.btnDiv = new System.Windows.Forms.Button();
            this.btnThree = new System.Windows.Forms.Button();
            this.btnZero = new System.Windows.Forms.Button();
            this.btnOne = new System.Windows.Forms.Button();
            this.btnTwo = new System.Windows.Forms.Button();
            this.btnSix = new System.Windows.Forms.Button();
            this.btnEight = new System.Windows.Forms.Button();
            this.btnNine = new System.Windows.Forms.Button();
            this.btnFour = new System.Windows.Forms.Button();
            this.btnFive = new System.Windows.Forms.Button();
            this.btnSeven = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnValidateExp = new System.Windows.Forms.Button();
            this.btnClearExp = new System.Windows.Forms.Button();
            this.btnCalExpAddDim = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TVCategory
            // 
            this.TVCategory.HideSelection = false;
            this.TVCategory.Location = new System.Drawing.Point(12, 12);
            this.TVCategory.Name = "TVCategory";
            this.TVCategory.Size = new System.Drawing.Size(166, 264);
            this.TVCategory.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCalExp);
            this.groupBox1.Location = new System.Drawing.Point(184, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 135);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = ">>>表达式";
            // 
            // txtCalExp
            // 
            this.txtCalExp.AcceptsReturn = true;
            this.txtCalExp.BackColor = System.Drawing.SystemColors.Window;
            this.txtCalExp.Enabled = false;
            this.txtCalExp.Location = new System.Drawing.Point(7, 15);
            this.txtCalExp.Multiline = true;
            this.txtCalExp.Name = "txtCalExp";
            this.txtCalExp.ReadOnly = true;
            this.txtCalExp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCalExp.Size = new System.Drawing.Size(323, 114);
            this.txtCalExp.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnRightBracket);
            this.groupBox2.Controls.Add(this.btnLeftBracket);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.btnMod);
            this.groupBox2.Controls.Add(this.btnDot);
            this.groupBox2.Controls.Add(this.btnMinus);
            this.groupBox2.Controls.Add(this.btnMulti);
            this.groupBox2.Controls.Add(this.btnDiv);
            this.groupBox2.Controls.Add(this.btnThree);
            this.groupBox2.Controls.Add(this.btnZero);
            this.groupBox2.Controls.Add(this.btnOne);
            this.groupBox2.Controls.Add(this.btnTwo);
            this.groupBox2.Controls.Add(this.btnSix);
            this.groupBox2.Controls.Add(this.btnEight);
            this.groupBox2.Controls.Add(this.btnNine);
            this.groupBox2.Controls.Add(this.btnFour);
            this.groupBox2.Controls.Add(this.btnFive);
            this.groupBox2.Controls.Add(this.btnSeven);
            this.groupBox2.Location = new System.Drawing.Point(184, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 128);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // btnRightBracket
            // 
            this.btnRightBracket.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRightBracket.ForeColor = System.Drawing.Color.Red;
            this.btnRightBracket.Location = new System.Drawing.Point(161, 42);
            this.btnRightBracket.Name = "btnRightBracket";
            this.btnRightBracket.Size = new System.Drawing.Size(32, 23);
            this.btnRightBracket.TabIndex = 17;
            this.btnRightBracket.Text = ")";
            this.btnRightBracket.UseVisualStyleBackColor = true;
            this.btnRightBracket.Click += new System.EventHandler(this.btnRightBracket_Click);
            // 
            // btnLeftBracket
            // 
            this.btnLeftBracket.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLeftBracket.ForeColor = System.Drawing.Color.Red;
            this.btnLeftBracket.Location = new System.Drawing.Point(161, 13);
            this.btnLeftBracket.Name = "btnLeftBracket";
            this.btnLeftBracket.Size = new System.Drawing.Size(32, 23);
            this.btnLeftBracket.TabIndex = 16;
            this.btnLeftBracket.Text = "(";
            this.btnLeftBracket.UseVisualStyleBackColor = true;
            this.btnLeftBracket.Click += new System.EventHandler(this.btnLeftBracket_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.ForeColor = System.Drawing.Color.Red;
            this.btnAdd.Location = new System.Drawing.Point(123, 100);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(32, 23);
            this.btnAdd.TabIndex = 15;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnMod
            // 
            this.btnMod.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMod.ForeColor = System.Drawing.Color.Blue;
            this.btnMod.Location = new System.Drawing.Point(85, 100);
            this.btnMod.Name = "btnMod";
            this.btnMod.Size = new System.Drawing.Size(32, 23);
            this.btnMod.TabIndex = 14;
            this.btnMod.Text = "%";
            this.btnMod.UseVisualStyleBackColor = true;
            this.btnMod.Click += new System.EventHandler(this.btnMod_Click);
            // 
            // btnDot
            // 
            this.btnDot.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDot.ForeColor = System.Drawing.Color.Blue;
            this.btnDot.Location = new System.Drawing.Point(47, 100);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(32, 23);
            this.btnDot.TabIndex = 13;
            this.btnDot.Text = ".";
            this.btnDot.UseVisualStyleBackColor = true;
            this.btnDot.Click += new System.EventHandler(this.btnDot_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMinus.ForeColor = System.Drawing.Color.Red;
            this.btnMinus.Location = new System.Drawing.Point(123, 71);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(32, 23);
            this.btnMinus.TabIndex = 12;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnMulti
            // 
            this.btnMulti.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMulti.ForeColor = System.Drawing.Color.Red;
            this.btnMulti.Location = new System.Drawing.Point(123, 42);
            this.btnMulti.Name = "btnMulti";
            this.btnMulti.Size = new System.Drawing.Size(32, 23);
            this.btnMulti.TabIndex = 11;
            this.btnMulti.Text = "*";
            this.btnMulti.UseVisualStyleBackColor = true;
            this.btnMulti.Click += new System.EventHandler(this.btnMulti_Click);
            // 
            // btnDiv
            // 
            this.btnDiv.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDiv.ForeColor = System.Drawing.Color.Red;
            this.btnDiv.Location = new System.Drawing.Point(123, 13);
            this.btnDiv.Name = "btnDiv";
            this.btnDiv.Size = new System.Drawing.Size(32, 23);
            this.btnDiv.TabIndex = 10;
            this.btnDiv.Text = "/";
            this.btnDiv.UseVisualStyleBackColor = true;
            this.btnDiv.Click += new System.EventHandler(this.btnDiv_Click);
            // 
            // btnThree
            // 
            this.btnThree.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnThree.ForeColor = System.Drawing.Color.Blue;
            this.btnThree.Location = new System.Drawing.Point(85, 71);
            this.btnThree.Name = "btnThree";
            this.btnThree.Size = new System.Drawing.Size(32, 23);
            this.btnThree.TabIndex = 9;
            this.btnThree.Text = "3";
            this.btnThree.UseVisualStyleBackColor = true;
            this.btnThree.Click += new System.EventHandler(this.btnThree_Click);
            // 
            // btnZero
            // 
            this.btnZero.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnZero.ForeColor = System.Drawing.Color.Blue;
            this.btnZero.Location = new System.Drawing.Point(9, 100);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(32, 23);
            this.btnZero.TabIndex = 8;
            this.btnZero.Text = "0";
            this.btnZero.UseVisualStyleBackColor = true;
            this.btnZero.Click += new System.EventHandler(this.btnZero_Click);
            // 
            // btnOne
            // 
            this.btnOne.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOne.ForeColor = System.Drawing.Color.Blue;
            this.btnOne.Location = new System.Drawing.Point(9, 71);
            this.btnOne.Name = "btnOne";
            this.btnOne.Size = new System.Drawing.Size(32, 23);
            this.btnOne.TabIndex = 7;
            this.btnOne.Text = "1";
            this.btnOne.UseVisualStyleBackColor = true;
            this.btnOne.Click += new System.EventHandler(this.btnOne_Click);
            // 
            // btnTwo
            // 
            this.btnTwo.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTwo.ForeColor = System.Drawing.Color.Blue;
            this.btnTwo.Location = new System.Drawing.Point(47, 71);
            this.btnTwo.Name = "btnTwo";
            this.btnTwo.Size = new System.Drawing.Size(32, 23);
            this.btnTwo.TabIndex = 6;
            this.btnTwo.Text = "2";
            this.btnTwo.UseVisualStyleBackColor = true;
            this.btnTwo.Click += new System.EventHandler(this.btnTwo_Click);
            // 
            // btnSix
            // 
            this.btnSix.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSix.ForeColor = System.Drawing.Color.Blue;
            this.btnSix.Location = new System.Drawing.Point(85, 42);
            this.btnSix.Name = "btnSix";
            this.btnSix.Size = new System.Drawing.Size(32, 23);
            this.btnSix.TabIndex = 5;
            this.btnSix.Text = "6";
            this.btnSix.UseVisualStyleBackColor = true;
            this.btnSix.Click += new System.EventHandler(this.btnSix_Click);
            // 
            // btnEight
            // 
            this.btnEight.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEight.ForeColor = System.Drawing.Color.Blue;
            this.btnEight.Location = new System.Drawing.Point(47, 13);
            this.btnEight.Name = "btnEight";
            this.btnEight.Size = new System.Drawing.Size(32, 23);
            this.btnEight.TabIndex = 4;
            this.btnEight.Text = "8";
            this.btnEight.UseVisualStyleBackColor = true;
            this.btnEight.Click += new System.EventHandler(this.btnEight_Click);
            // 
            // btnNine
            // 
            this.btnNine.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNine.ForeColor = System.Drawing.Color.Blue;
            this.btnNine.Location = new System.Drawing.Point(85, 13);
            this.btnNine.Name = "btnNine";
            this.btnNine.Size = new System.Drawing.Size(32, 23);
            this.btnNine.TabIndex = 3;
            this.btnNine.Text = "9";
            this.btnNine.UseVisualStyleBackColor = true;
            this.btnNine.Click += new System.EventHandler(this.btnNine_Click);
            // 
            // btnFour
            // 
            this.btnFour.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFour.ForeColor = System.Drawing.Color.Blue;
            this.btnFour.Location = new System.Drawing.Point(9, 42);
            this.btnFour.Name = "btnFour";
            this.btnFour.Size = new System.Drawing.Size(32, 23);
            this.btnFour.TabIndex = 2;
            this.btnFour.Text = "4";
            this.btnFour.UseVisualStyleBackColor = true;
            this.btnFour.Click += new System.EventHandler(this.btnFour_Click);
            // 
            // btnFive
            // 
            this.btnFive.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFive.ForeColor = System.Drawing.Color.Blue;
            this.btnFive.Location = new System.Drawing.Point(47, 42);
            this.btnFive.Name = "btnFive";
            this.btnFive.Size = new System.Drawing.Size(32, 23);
            this.btnFive.TabIndex = 1;
            this.btnFive.Text = "5";
            this.btnFive.UseVisualStyleBackColor = true;
            this.btnFive.Click += new System.EventHandler(this.btnFive_Click);
            // 
            // btnSeven
            // 
            this.btnSeven.Font = new System.Drawing.Font("SimSun", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSeven.ForeColor = System.Drawing.Color.Blue;
            this.btnSeven.Location = new System.Drawing.Point(9, 13);
            this.btnSeven.Name = "btnSeven";
            this.btnSeven.Size = new System.Drawing.Size(32, 23);
            this.btnSeven.TabIndex = 0;
            this.btnSeven.Text = "7";
            this.btnSeven.UseVisualStyleBackColor = true;
            this.btnSeven.Click += new System.EventHandler(this.btnSeven_Click);
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOk.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOk.Location = new System.Drawing.Point(320, 282);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(57, 23);
            this.btnOk.TabIndex = 18;
            this.btnOk.Text = "确定(&K)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(383, 282);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(57, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnValidateExp
            // 
            this.btnValidateExp.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnValidateExp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnValidateExp.Location = new System.Drawing.Point(257, 282);
            this.btnValidateExp.Name = "btnValidateExp";
            this.btnValidateExp.Size = new System.Drawing.Size(57, 23);
            this.btnValidateExp.TabIndex = 20;
            this.btnValidateExp.Text = "校验";
            this.btnValidateExp.UseVisualStyleBackColor = true;
            this.btnValidateExp.Click += new System.EventHandler(this.btnValidateExp_Click);
            // 
            // btnClearExp
            // 
            this.btnClearExp.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClearExp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnClearExp.Location = new System.Drawing.Point(193, 282);
            this.btnClearExp.Name = "btnClearExp";
            this.btnClearExp.Size = new System.Drawing.Size(57, 23);
            this.btnClearExp.TabIndex = 21;
            this.btnClearExp.Text = "清除";
            this.btnClearExp.UseVisualStyleBackColor = true;
            this.btnClearExp.Click += new System.EventHandler(this.btnClearExp_Click);
            // 
            // btnCalExpAddDim
            // 
            this.btnCalExpAddDim.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCalExpAddDim.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCalExpAddDim.Location = new System.Drawing.Point(119, 282);
            this.btnCalExpAddDim.Name = "btnCalExpAddDim";
            this.btnCalExpAddDim.Size = new System.Drawing.Size(68, 23);
            this.btnCalExpAddDim.TabIndex = 22;
            this.btnCalExpAddDim.Text = "添加维度";
            this.btnCalExpAddDim.UseVisualStyleBackColor = true;
            this.btnCalExpAddDim.Click += new System.EventHandler(this.btnCalExpAddDim_Click);
            // 
            // VCalExpressionDefine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 312);
            this.Controls.Add(this.btnCalExpAddDim);
            this.Controls.Add(this.btnClearExp);
            this.Controls.Add(this.btnValidateExp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TVCategory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "VCalExpressionDefine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "构造计算表达式";
            this.Load += new System.EventHandler(this.VCalExpressionDefine_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private VirtualMachine.Component.WinControls.Controls.CustomTreeView TVCategory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCalExp;
        private System.Windows.Forms.GroupBox groupBox2;
        private Button btnThree;
        private Button btnZero;
        private Button btnOne;
        private Button btnTwo;
        private Button btnSix;
        private Button btnEight;
        private Button btnNine;
        private Button btnFour;
        private Button btnFive;
        private Button btnSeven;
        private Button btnAdd;
        private Button btnMod;
        private Button btnDot;
        private Button btnMinus;
        private Button btnMulti;
        private Button btnDiv;
        private Button btnRightBracket;
        private Button btnLeftBracket;
        private Button btnOk;
        private Button btnValidateExp;
        private Button btnClearExp;
        private Button btnCalExpAddDim;
        private Button btnCancel;
    }
}