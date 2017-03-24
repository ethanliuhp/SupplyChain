using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VGenerateRequirePlanSetYearMonth : Form
    {
        private int _accountYear;
        /// <summary>
        /// 会计年
        /// </summary>
        public int AccountYear
        {
            get { return _accountYear; }
            set { _accountYear = value; }
        }
        private int _accountMonth;
        /// <summary>
        /// 会计月
        /// </summary>
        public int AccountMonth
        {
            get { return _accountMonth; }
            set { _accountMonth = value; }
        }

        private bool _IsOk = false;
        /// <summary>
        /// 是否点击确定
        /// </summary>
        public bool IsOk
        {
            get { return _IsOk; }
            set { _IsOk = value; }
        }

        public VGenerateRequirePlanSetYearMonth()
        {
            InitializeComponent();
            InitialForm();
        }
        private void InitialForm()
        {
            //AccountYear = DateTime.Now.Year;
            //AccountMonth = DateTime.Now.Month;
            AccountYear = ConstObject.TheLogin.TheComponentPeriod.NowYear;
            AccountMonth = ConstObject.TheLogin.TheComponentPeriod.NowMonth;

            txtYear.Text = AccountYear.ToString();
            txtMonth.Text = AccountMonth.ToString();

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtYear.Text.Trim() == "")
            {
                MessageBox.Show("会计年不能为空！");
                txtYear.Focus();
                return;
            }
            else if (txtMonth.Text.Trim() == "")
            {
                MessageBox.Show("会计月不能为空！");
                txtMonth.Focus();
                return;
            }

            try
            {
                AccountYear = Convert.ToInt32(txtYear.Text);
            }
            catch
            {
                MessageBox.Show("会计年输入格式不正确！");
                txtYear.Focus();
                return;
            }
            try
            {
                AccountMonth = Convert.ToInt32(txtMonth.Text);
            }
            catch
            {
                MessageBox.Show("会计月输入格式不正确！");
                txtMonth.Focus();
                return;
            }

            IsOk = true;

            this.Close();
        }

    }
}
