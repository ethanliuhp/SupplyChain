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
    public partial class VGWBSDetailCostEditInputPrice : Form
    {
        OptCostType optionCostType = OptCostType.合同收入;

        public OptSetPriceType SetPriceType = OptSetPriceType.直接输入;

        public bool IsOK = false;

        private decimal quantityPriceValue = 0;
        /// <summary>
        /// （耗用）数量单价值
        /// </summary>
        public decimal QuantityPriceValue
        {
            get { return quantityPriceValue; }
            set { quantityPriceValue = value; }
        }
        private decimal projectAmountPriceValue = 0;
        /// <summary>
        /// （耗用）工程量单价值
        /// </summary>
        public decimal ProjectAmountPriceValue
        {
            get { return projectAmountPriceValue; }
            set { projectAmountPriceValue = value; }
        }

        public VGWBSDetailCostEditInputPrice()
        {
            InitializeComponent();
        }
        public VGWBSDetailCostEditInputPrice(OptCostType optCostType)
            : this()
        {
            optionCostType = optCostType;

            InitForm();
        }

        private void InitForm()
        {
            if (optionCostType == OptCostType.合同收入)
            {
                lblQuantityPrice.Text = "合同数量单价";
                lblProjectAmountPrice.Text = "合同工程量单价";
            }
            else if (optionCostType == OptCostType.责任成本)
            {
                lblQuantityPrice.Text = "责任数量单价";
                lblProjectAmountPrice.Text = "责任工程量单价";
            }
            else if (optionCostType == OptCostType.计划成本)
            {
                lblQuantityPrice.Text = "计划数量单价";
                lblProjectAmountPrice.Text = "计划工程量单价";
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
                    lblQuantityPrice.Text = "合同数量单价";
                    lblProjectAmountPrice.Text = "合同工程量单价";
                }
                else if (optionCostType == OptCostType.责任成本)
                {
                    lblQuantityPrice.Text = "责任数量单价";
                    lblProjectAmountPrice.Text = "责任工程量单价";
                }
                else if (optionCostType == OptCostType.计划成本)
                {
                    lblQuantityPrice.Text = "计划数量单价";
                    lblProjectAmountPrice.Text = "计划工程量单价";
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
                    lblQuantityPrice.Text = "合同数量单价百分比(如：0.85)";
                    lblProjectAmountPrice.Text = "合同工程量单价百分比(如：0.85)";
                }
                else if (optionCostType == OptCostType.责任成本)
                {
                    lblQuantityPrice.Text = "责任数量单价百分比(如：0.85)";
                    lblProjectAmountPrice.Text = "责任工程量单价百分比(如：0.85)";
                }
                else if (optionCostType == OptCostType.计划成本)
                {
                    lblQuantityPrice.Text = "计划数量单价百分比(如：0.85)";
                    lblProjectAmountPrice.Text = "计划工程量单价百分比(如：0.85)";
                }
            }
        }


        void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtQuantityPrice.Text.Trim() != "")
                    quantityPriceValue = Convert.ToDecimal(txtQuantityPrice.Text);
            }
            catch
            {
                MessageBox.Show("数量单价值输入格式不正确！");
                txtQuantityPrice.Focus();
                return;
            }
            try
            {
                if (txtProjectAmountPrice.Text.Trim() != "")
                    projectAmountPriceValue = Convert.ToDecimal(txtProjectAmountPrice.Text);
            }
            catch
            {
                MessageBox.Show("工程量单价值输入格式不正确！");
                txtProjectAmountPrice.Focus();
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
