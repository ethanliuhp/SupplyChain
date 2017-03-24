using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain
{
    /// <summary>
    /// 商品砼结算单基类
    /// </summary>
    [Serializable]
    public class BaseConcreteBalanceMaster : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private decimal sumVolumeQuantity;
        private decimal addSumMoney;

        /// <summary>
        /// 累计结算金额
        /// </summary>
        virtual public decimal AddSumMoney
        {
            get { return addSumMoney; }
            set { addSumMoney = value; }
        }
        /// <summary>
        /// 供应商
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        private string supplierName;
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }

        /// <summary>
        /// 总方量
        /// </summary>
        virtual public decimal SumVolumeQuantity
        {
            get { return sumVolumeQuantity; }
            set { sumVolumeQuantity = value; }
        }
    }
}
