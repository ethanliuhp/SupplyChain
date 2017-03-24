using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;


namespace Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain
{
    /// <summary>
    /// 服务OBS
    /// </summary>
    [Serializable]
    public class OBSService : BaseMaster
    {
        private SubContractProject supplierId;
        private string supplierName;
        private DateTime beginDate = ClientUtil.ToDateTime("1900-1-1");
        private DateTime endDate = ClientUtil.ToDateTime("1900-1-1");
        private GWBSTree projectTask;
        private string projectTaskName;
        private string projectTaskCode;
        private string serviceState;
        private string serviceType;
        private GWBSDetail wBSDetail;
        private string gWBSDetailName;
        private string materialFeeSettlementFlag;
        private decimal planNedQuantity;
        private string quotaCode;
        private string fullPath;
        ///<summary>
        /// 服务类型
        ///</summary>
        virtual public string ServiceType
        {
            get { return serviceType; }
            set { serviceType = value; }
        }
        ///<summary>
        /// 状态
        ///</summary>
        virtual public string ServiceState
        {
            get { return serviceState; }
            set { serviceState = value; }
        }

        ///<summary>
        ///分包项目
        ///</summary>
        virtual public SubContractProject SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }
        ///<summary>
        ///服务供应商名称
        ///</summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        ///<summary>
        ///开始时间
        ///</summary>
        virtual public DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }
        ///<summary>
        ///结束时间
        ///</summary>
        virtual public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        ///<summary>
        ///工程项目任务
        ///</summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        ///<summary>
        ///工程项目任务名称
        ///</summary>
        virtual public string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        ///<summary>
        ///工程项目任务层次码
        ///</summary>
        virtual public string ProjectTaskCode
        {
            get { return projectTaskCode; }
            set { projectTaskCode = value; }
        }
        /// <summary>
        /// 任务明细
        /// </summary>
        virtual public GWBSDetail WBSDetail
        {
            get { return wBSDetail; }
            set { wBSDetail = value; }
        }
        /// <summary>
        /// 工程任务明细名称
        /// </summary>
        virtual public string GWBSDetailName
        {
            get { return gWBSDetailName; }
            set { gWBSDetailName = value; }
        }
        /// <summary>
        /// 结算标识
        /// </summary>
        virtual public string MaterialFeeSettlementFlag
        {
            get { return materialFeeSettlementFlag; }
            set { materialFeeSettlementFlag = value; }
        }
        /// <summary>
        /// 工程量
        /// </summary>
        virtual public decimal PlanNedQuantity
        {
            get { return planNedQuantity; }
            set { planNedQuantity = value; }
        }
        /// <summary>
        /// 对应定额编号
        /// </summary>
        virtual public string QuotaCode
        {
            get { return quotaCode; }
            set { quotaCode = value; }
        }
        /// <summary>
        /// 完整路径
        /// </summary>
        virtual public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
        }
    }
}
