using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Main;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonForm
{
    public partial class VMessageShow : Form
    {
        public string estimateInfo = "";
        public string ExpenseAccountInfo = "";
        public string PayOutBillInfo = "";

        public VMessageShow()
        {
            InitializeComponent();
        }

        private void VMessageShow_Load(object sender, EventArgs e)
        {
            if (ExpenseAccountInfo != "")
            {
                this.linkExpenseAccount.Text = ExpenseAccountInfo;
            }
            else
            {
                this.linkExpenseAccount.Visible = false;
            }

            if (PayOutBillInfo != "")
            {
                this.linkPayoutBill.Text = PayOutBillInfo;
            }
            else
            {
                this.linkPayoutBill.Visible = false;
            }

            if (estimateInfo != "")
            {
                this.rchEstimate.Text = estimateInfo;
            }
            else
            {
                this.rchEstimate.Visible = false;
            }
        }

        internal void Start()
        {
        }
    }
}