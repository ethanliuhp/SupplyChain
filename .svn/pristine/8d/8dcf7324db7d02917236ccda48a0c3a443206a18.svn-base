using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;

namespace Application.Business.Erp.SupplyChain.Client.TotalDemandPlan
{
    public partial class VRace : TBasicDataView
    {
        private decimal result = 0;
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public decimal Result
        {
            get { return result; }
            set { result = value; }
        }
        public VRace()
        {
            InitializeComponent();
            InnerEvent();
        }
        private void InnerEvent()
        {
            btnOK.Click +=new EventHandler(btnOK_Click);
            btnCancel.Click +=new EventHandler(btnCancel_Click);
        }

        void btnOK_Click(object sender,EventArgs e)
        {
            bool validity = true;
            string temp_quantity = txtRace.Text.ToString();
            validity = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.CommonMethod.VeryValid(temp_quantity);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                return;
            }
            result = Convert.ToDecimal(this.txtRace.Text);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender,EventArgs e)
        {
            this.btnCancel.FindForm().Close();
        }
    }
}
