using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    [Serializable]
    [Entity]
    public class SpeCostSettlementMaster : BaseMaster
    {
        private string subcontractUnitName;
        private SubContractProject subcontractProjectId;
        private DateTime settlementDate = new DateTime(1900, 1, 1);
        private DateTime submitDate = new DateTime(1900, 1, 1);    
        /// <summary>
        /// 分包单位名称
        /// </summary>
        virtual public string SubcontractUnitName
        {
            get { return subcontractUnitName; }
            set { subcontractUnitName = value; }
        }
       /// <summary>
        /// 分包项目
       /// </summary>
        virtual public SubContractProject SubcontractProjectId
        {
            get { return subcontractProjectId; }
            set { subcontractProjectId = value; }
        }      
       /// <summary>
        /// 结算日期
       /// </summary>
        virtual public DateTime SettlementDate
        {
            get { return settlementDate; }
            set { settlementDate = value; }
        }        
       /// <summary>
        /// 提交时间
       /// </summary>
        virtual public DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
        }
    }
}
