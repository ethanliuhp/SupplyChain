using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain
{
    /// <summary>
    /// 财务账面盘赢盘亏单
    /// </summary>
    [Serializable]
    public class AcctLoseAndProfit : BaseBillMaster
    {
        private CustomerRelationInfo theCustomerRelationInfo;
        private SupplierRelationInfo theSupplierRelationInfo;
        private int businessType;
        private string businessTypeName;

        /// <summary>
        /// 供应商
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }

        /// <summary>
        /// 客户
        /// </summary>
        virtual public CustomerRelationInfo TheCustomerRelationInfo
        {
            get { return theCustomerRelationInfo; }
            set { theCustomerRelationInfo = value; }
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        virtual public int BusinessType
        {
            get { return businessType; }
            set { businessType = value; }
        }

        /// <summary>
        /// 业务类型名称(报帐，结算)
        /// </summary>
        virtual public string BusinessTypeName
        {
            get { return businessTypeName; }
            set { businessTypeName = value; }
        }
    }
}
