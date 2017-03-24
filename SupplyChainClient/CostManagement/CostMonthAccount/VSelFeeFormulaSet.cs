using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using System.Text.RegularExpressions;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VSelFeeFormulaSet : TBasicDataView
    {

        MCostMonthAccount model = new MCostMonthAccount();
        private string[] _strInput;
        ///<summary></summary>
        public  string[] StrInput
        {
            set { 
                if(value.Length > 1 && !string.IsNullOrEmpty(value[0]))
                this._strInput = value;
                this.txtCalExp.Text = value[0];
                this.txtCalExp.Tag = value[1];
            }
        }
        private string[] _strOutput;
        ///<summary></summary>
        public  string[] strOutput
        {
            get { return this._strOutput; }
        }

        public VSelFeeFormulaSet()
        {
            InitializeComponent();
            InitEvents();
            InitData();
        }

        private void InitData()
        {

        }
        void InitEvents()
        {
            #region 数字、运算符事件处理
            this.btnAdd.Click += new EventHandler(NumOrSymbol_Click); //+
            this.btnMinus.Click += new EventHandler(NumOrSymbol_Click);//-
            this.btnMulti.Click += new EventHandler(NumOrSymbol_Click);//*
            this.btnDiv.Click += new EventHandler(NumOrSymbol_Click);// /
            this.btnLeftBracket.Click += new EventHandler(NumOrSymbol_Click);//(
            this.btnRightBracket.Click += new EventHandler(NumOrSymbol_Click);//)
            this.btnZero.Click += new EventHandler(NumOrSymbol_Click);
            this.btnOne.Click += new EventHandler(NumOrSymbol_Click);
            this.btnTwo.Click += new EventHandler(NumOrSymbol_Click);
            this.btnThree.Click += new EventHandler(NumOrSymbol_Click);
            this.btnFour.Click += new EventHandler(NumOrSymbol_Click);
            this.btnFive.Click += new EventHandler(NumOrSymbol_Click);
            this.btnSix.Click += new EventHandler(NumOrSymbol_Click);
            this.btnSeven.Click += new EventHandler(NumOrSymbol_Click);
            this.btnEight.Click += new EventHandler(NumOrSymbol_Click);
            this.btnNine.Click += new EventHandler(NumOrSymbol_Click);
            this.btnDot.Click += new EventHandler(NumOrSymbol_Click);
            this.btnMod.Click += new EventHandler(NumOrSymbol_Click);// %
            #endregion

            this.btnInsert.Click+=new EventHandler(btnInsert_Click);
            this.btnOk.Click+=new EventHandler(btnOk_Click);
            this.btnClearExp.Click+=new EventHandler(btnClearExp_Click);
            this.btnCancle.Click+=new EventHandler(btnCancle_Click);
            this.txtAccountSubject.DoubleClick += new EventHandler(txtAccountSubject_DoubleClick);
        }

        void txtAccountSubject_DoubleClick(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.IsLeafSelect = false;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                this.txtAccountSubject.Text = cost.Name;
                this.txtAccountSubject.Tag = cost.Code;
            }
        }
        void btnInsert_Click(object sender, EventArgs e)
        {
            if (cbPriceType.Text != "")
            {
                AddButtonText("[" + this.cbPriceType.Text + "]", "[" + this.cbPriceType.Text + "]");
            }
            if (txtAccountSubject.Text != "")
            {
                string tag = this.txtAccountSubject.Tag as string;
                if (string.IsNullOrEmpty(tag)) tag = "";
                AddButtonText("[" + this.txtAccountSubject.Text + "]", "[" + tag + "]");
            }

            this.txtAccountSubject.Text = "";
            this.txtAccountSubject.Tag = null;
            this.cbPriceType.Text = "";
        }
  
        void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOk_Click(object sender, EventArgs e)
        {
            string sExpression = this.txtCalExp.Text;
            if (!string.IsNullOrEmpty(sExpression))
            {
                #region 正则表达式验证 弊端 []里面的东西是否合法验证不到
                MatchCollection mc = Regex.Matches(sExpression, @"\[([^\]]*)\]");
                foreach (Match m in mc)
                {
                    sExpression = sExpression.Replace("[" + m.Groups[1].Value + "]", "(1+1)"); //替换 "newString" 是你要查回的名称
                }
                #endregion

                //foreach (string sItem in cbPriceType.Items)
                //{
                //    sExpression = sExpression.Replace(string.Format("[{0}]", sItem), "(1+1)");
                //}
                //Hashtable ht = model.CostMonthAccSrv.GetCostSubjectNameList();
                //foreach (string sItem in ht.Values)
                //{
                //    sExpression = sExpression.Replace(string.Format("[{0}]", sItem), "(1+1)");
                //}
               
                try
                {
                    DataTable oTable = new DataTable();
                    if (oTable.Compute(sExpression, "") == null)
                    {
                        throw new Exception("公式异常");
                    }
                }
                catch
                {
                    MessageBox.Show(string.Format("公式【{0}】不合法，请重新输入", this.txtCalExp.Text), "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }

            string txt = this.txtCalExp.Text.Trim();
            string tag = this.txtCalExp.Tag == null ?"": this.txtCalExp.Tag as string;

            this._strOutput = new string[] { txt, tag };
            this.Close();
        }
        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnClearExp_Click(object sender, EventArgs e)
        {
            txtCalExp.Text = "";
            txtCalExp.Tag = null;
        }

        //private void UpdateTxtTag(string text)
        //{
        //    string calExp = txtCalExp.Tag as string;
        //    if (calExp == null) calExp = "";
        //    calExp += text;
        //    txtCalExp.Tag = calExp;
        //}

        #region 数字、运算符事件处理
        private void AddButtonText(String text,string tag)
        {
            txtCalExp.AppendText(text);

            string txtCalExpTag = this.txtCalExp.Tag as string;
            if (string.IsNullOrEmpty(txtCalExpTag)) txtCalExpTag = "";
            txtCalExpTag += tag;
            this.txtCalExp.Tag = txtCalExpTag;

        }

        private void NumOrSymbol_Click(object sender, EventArgs e)
        {
            AddButtonText(((Button)sender).Text, ((Button)sender).Text);
        }
        #endregion
    }
}
