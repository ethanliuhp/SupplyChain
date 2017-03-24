using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDetailCostEditInputPriceByDtl : Form
    {
        OptCostType optionCostType = OptCostType.合同收入;

        public OptSetPriceType SetPriceType = OptSetPriceType.直接输入;

        public bool IsOK = false;


        private decimal priceValue = 0;
        /// <summary>
        /// （明细）单价值
        /// </summary>
        public decimal PriceValue
        {
            get { return priceValue; }
            set { priceValue = value; }
        }

        public VGWBSDetailCostEditInputPriceByDtl()
        {
            InitializeComponent();
        }
        public VGWBSDetailCostEditInputPriceByDtl(OptCostType optCostType)
            : this()
        {
            optionCostType = optCostType;

            InitForm();
        }

        private void InitForm()
        {
            if (optionCostType == OptCostType.合同收入)
            {
                lblPrice.Text = "合同单价";
            }
            else if (optionCostType == OptCostType.责任成本)
            {
                lblPrice.Text = "责任单价";
            }
            else if (optionCostType == OptCostType.计划成本)
            {
                lblPrice.Text = "计划单价";
            }

            InitEvents();
        }
        private void InitEvents()
        {
            this.btnEnter.Click += new EventHandler(btnEnter_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            rdInput.Click += new EventHandler(rdInput_Click);
            rdUseRate.Click += new EventHandler(rdUseRate_Click);
        }

        void rdInput_Click(object sender, EventArgs e)
        {
            if (rdInput.Checked)
            {
                SetPriceType = OptSetPriceType.直接输入;

                if (optionCostType == OptCostType.合同收入)
                {
                    lblPrice.Text = "合同单价";
                }
                else if (optionCostType == OptCostType.责任成本)
                {
                    lblPrice.Text = "责任单价";
                }
                else if (optionCostType == OptCostType.计划成本)
                {
                    lblPrice.Text = "计划单价";
                }
            }
        }
        void rdUseRate_Click(object sender, EventArgs e)
        {
            if (rdUseRate.Checked)
            {
                SetPriceType = OptSetPriceType.根据系数调整;

                if (optionCostType == OptCostType.合同收入)
                {
                    lblPrice.Text = "合同单价百分比(如：0.85)";
                }
                else if (optionCostType == OptCostType.责任成本)
                {
                    lblPrice.Text = "责任单价百分比(如：0.85)";
                }
                else if (optionCostType == OptCostType.计划成本)
                {
                    lblPrice.Text = "计划单价百分比(如：0.85)";
                }
            }
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPrice.Text.Trim() != "")
                    priceValue = Convert.ToDecimal(txtPrice.Text);
            }
            catch
            {
                MessageBox.Show("单价值输入格式不正确！");
                txtPrice.Focus();
                return;
            }

            IsOK = true;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            IsOK = false;

            this.Close();
        }
    }
}
