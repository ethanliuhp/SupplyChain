using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain
{
    /// <summary>
    /// 劳务需求计划主表
    /// </summary>
    [Serializable]
    public class LaborDemandPlanMaster : BaseSupplyMaster
    {
        private DateTime? reportTime;
       
        private string planName;
        
        ///<summary>
        /// 计划名称
        /// </summary>
        virtual public string PlanName
        {
            get { return planName; }
            set { planName = value; }
        }
        /// <summary>
        /// 提报时间
        /// </summary>
        virtual public DateTime? ReportTime
        {
            get { return reportTime; }
            set { reportTime = value; }
        }
    }
}
