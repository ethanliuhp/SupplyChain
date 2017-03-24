using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireSupply.Domain
{
    /// <summary>
    /// 采购合同付款方式
    /// </summary>
    [Serializable]
    public class MatHireSupplyOrderPayment
    {
        private string id;
        private decimal paymentProportion;
        private string paymentState;
        private decimal factPaymentProportion;
        private string projectState;
        private MatHireSupplyOrderMaster master;
        private string descript;
        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 付款比例
        /// </summary>
        virtual public decimal PaymentProportion
        {
            get { return paymentProportion; }
            set { paymentProportion = value; }
        }
        /// <summary>
        /// 实际付款比例
        /// </summary>
        virtual public decimal FactPaymentProportion
        {
            get { return factPaymentProportion; }
            set { factPaymentProportion = value; }
        }
        /// <summary>
        /// 付款状态
        /// </summary>
        virtual public string PaymentState
        {
            get { return paymentState; }
            set { paymentState = value; }
        }
        /// <summary>
        /// 项目工程状态
        /// </summary>
        virtual public string ProjectState
        {
            get { return projectState; }
            set { projectState = value; }
        }
        virtual public MatHireSupplyOrderMaster Master
        {
            get { return master; }
            set { master = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
    }
}
