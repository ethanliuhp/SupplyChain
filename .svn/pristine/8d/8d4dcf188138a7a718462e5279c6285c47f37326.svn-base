using System;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    [Serializable]
    [Entity]
    public class DisclosureMaster : BaseMaster
    {
        private string contractName;    //合同名称
        private SupplierRelationInfo bearerOrg;     //分包组织
        private string bearerOrgName;   //分包单位名称
        /// <summary>
        /// 合同名称
        /// </summary>
        virtual public string ContractName
        {
            get { return contractName; }
            set { contractName = value; }
        }
        /// <summary>
        /// 分包组织
        /// </summary>
        virtual public SupplierRelationInfo BearerOrg
        {
            get { return bearerOrg; }
            set { bearerOrg = value; }
        }
        /// <summary>
        /// 分包单位名称
        /// </summary>
        virtual public string BearerOrgName
        {
            get { return bearerOrgName; }
            set { bearerOrgName = value; }
        }
    }
}
