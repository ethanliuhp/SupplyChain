using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Domain
{
    /// <summary>
    /// 材料结算单主表
    /// </summary>
    [Serializable]
    public class MaterialSettlementMaster : BaseMaster
    {
        private SupplierRelationInfo supplier;
        private string supplierName;
        private decimal sumMoney;

        /// <summary>
        /// 供应商
        /// </summary>
        virtual public SupplierRelationInfo Supplier
        {
            get { return supplier; }
            set { supplier = value; }
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }

        /// <summary>
        /// 金额汇总
        /// </summary>
        virtual public decimal SumMoney
        {
            get
            {
                decimal temMoney = 0;
                //汇总
                foreach (MaterialSettlementDetail var in Details)
                {
                    temMoney += var.ContainTaxMoney;
                }
                sumMoney = temMoney;
                return sumMoney;
            }
            set
            {
                sumMoney = value;
            }
        }
    }
}
