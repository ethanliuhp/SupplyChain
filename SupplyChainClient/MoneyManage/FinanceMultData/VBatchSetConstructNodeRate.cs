using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VBatchSetConstructNodeRate : Form
    {
        public VBatchSetConstructNodeRate()
        {
            InitializeComponent();

            InitEvents();
        }

        public delegate void AfterClickOkEventHandler(int startRow, int endRow, decimal rate, decimal change);

        public AfterClickOkEventHandler AfterClickOkEvent;

        private void InitEvents()
        {
            txtStartRow.TextChanged += new EventHandler(txtStartRow_TextChanged);
            txtEndRow.TextChanged += new EventHandler(txtEndRow_TextChanged);
            txtRate.TextChanged += new EventHandler(txtRate_TextChanged);
            txtChange.TextChanged += new EventHandler(txtChange_TextChanged);

            btnOk.Click += new EventHandler(btnOk_Click);
            btnClose.Click += new EventHandler(btnClose_Click);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            lbMes.Text = string.Empty;
            int startRow = 0;
            if(!int.TryParse(txtStartRow.Text.Trim(),out startRow))
            {
                errorProvider1.SetError(txtStartRow, "请输入正确的整数");
                return;
            }

            int endRow = 0;
            if (!int.TryParse(txtEndRow.Text.Trim(), out endRow))
            {
                errorProvider1.SetError(txtEndRow, "请输入正确的整数");
                return;
            }

            if (endRow < startRow)
            {
                errorProvider1.SetError(txtEndRow, "终止行必须大于起始行，请重新输入正确的整数");
                return;
            }

            decimal rate = 0;
            if(!decimal.TryParse(txtRate.Text.Trim(),out rate))
            {
                errorProvider1.SetError(txtRate, "请输入1-100间的数值");
                return;
            }

            decimal chage = 0;
            if (!decimal.TryParse(txtChange.Text.Trim(), out chage))
            {
                errorProvider1.SetError(txtChange, "请输入正确的数值");
                return;
            }

            var totalChage = (endRow - startRow) * chage + rate;
            var maxChage = Math.Round((100 - rate) / (endRow - startRow), 4);
            var minChage = Math.Round(rate / (endRow - startRow), 4);
            if (chage != 0 && (totalChage > 100 || totalChage < 0))
            {
                lbMes.Text = string.Format("请输入{0}-{1}之间的数值", minChage, maxChage);
                errorProvider1.SetError(txtChange, lbMes.Text);
                return;
            }

            errorProvider1.Clear();
            if (AfterClickOkEvent != null)
            {
                AfterClickOkEvent(startRow, endRow, rate, chage);
            }

            var interval = endRow - startRow + 1;
            txtStartRow.Text = (startRow + interval).ToString();
            txtEndRow.Text = (endRow + interval).ToString();
        }

        private void txtChange_TextChanged(object sender, EventArgs e)
        {
            decimal tmp = 0;
            if (!decimal.TryParse(txtChange.Text.Trim(), out tmp) || tmp <= 0 || tmp > 100)
            {
                errorProvider1.SetError(txtChange, "请输入0-100间的数值");
            }
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            decimal tmp = 0;
            if (!decimal.TryParse(txtRate.Text.Trim(), out tmp) || tmp <= 0 || tmp > 100)
            {
                txtRate.Text = "0";
                errorProvider1.SetError(txtRate, "请输入1-100间的数值");
                return;
            }

            if(!string.IsNullOrEmpty(txtStartRow.Text.Trim()) 
                && !string.IsNullOrEmpty(txtEndRow.Text.Trim()))
            {
                //btnOk_Click(null, null);
            }
        }

        private void txtEndRow_TextChanged(object sender, EventArgs e)
        {
            int tmp = 0;
            if (!int.TryParse(txtEndRow.Text.Trim(), out tmp) || tmp <= 0)
            {
                txtEndRow.Text = "1";
                errorProvider1.SetError(txtEndRow, "请输入正确的整数");
            }
        }

        private void txtStartRow_TextChanged(object sender, EventArgs e)
        {
            int tmp = 0;
            if (!int.TryParse(txtStartRow.Text.Trim(), out tmp) || tmp <= 0)
            {
                txtStartRow.Text = "1";
                errorProvider1.SetError(txtStartRow, "请输入正大于0的整数");
            }
        }
    }
}
