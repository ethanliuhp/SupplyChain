using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Domain
{
    /// <summary>
    /// 材料结算单明细
    /// </summary>
    [Serializable]
    public class MaterialSettlementDetail : BaseDetail
    {
        private decimal notContainTaxPrice;
        private decimal containTaxPrice;
        private decimal notContainTaxMoney;
        private decimal containTaxMoney;
        private decimal tax;

        /// <summary>
        /// 税额
        /// </summary>
        virtual public decimal Tax
        {
            get { return tax; }
            set { tax = value; }
        }
        /// <summary>
        /// 不含税单价
        /// </summary>
        virtual public decimal NotContainTaxPrice
        {
            get { return notContainTaxPrice; }
            set { notContainTaxPrice = value; }
        }
        /// <summary>
        /// 含税单价
        /// </summary>
        virtual public decimal ContainTaxPrice
        {
            get { return containTaxPrice; }
            set { containTaxPrice = value; }
        }
        /// <summary>
        /// 不含税金额
        /// </summary>
        virtual public decimal NotContainTaxMoney
        {
            get { return notContainTaxMoney; }
            set { notContainTaxMoney = value; }
        }
        /// <summary>
        /// 含税金额
        /// </summary>
        virtual public decimal ContainTaxMoney
        {
            get { return containTaxMoney; }
            set { containTaxMoney = value; }
        }
    }
}
