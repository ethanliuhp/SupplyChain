using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain
{
    /// <summary>
    /// 劳务需求计划明细
    /// </summary>
    [Serializable]
    public class LaborDemandPlanDetail : BaseDetail
    {
        private StandardUnit projectQuantityUnit;
        private string projectQuantityUnitName;
        private StandardUnit projectTimeLimitUnit;
        private string projectTimeLimitUnitName;
        private DateTime? laborRankInTime;
        private decimal estimateProjectQuantity;
        private decimal estimateProjectTimeLimit;
        private string qualitySafetyRequirement;
        private string mainJobDescript;
        //private string laborRankType;
        private string correspondGWBS;
        private string usedRankType;
        //private string usedRankTypeName;
        private GWBSTree projectTask;
        private string projectTaskName;
        private string projectTaskSysCode;
        private Iesi.Collections.Generic.ISet<LaborDemandWorkerType> workerDetails = new Iesi.Collections.Generic.HashedSet<LaborDemandWorkerType>();
        private decimal planLaborDemandNumber;

        virtual public Iesi.Collections.Generic.ISet<LaborDemandWorkerType> WorkerDetails
        {
            get { return workerDetails; }
            set { workerDetails = value; }
        }

        /// <summary>
        /// 增加工种信息
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddWoker(LaborDemandWorkerType wokerDetail)
        {
            wokerDetail.Master = this;
            WorkerDetails.Add(wokerDetail);
        }

        ///<summary>
        /// 工程任务
        /// </summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        ///<summary>
        /// 工程任务名称
        /// </summary>
        virtual public string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        ///<summary>
        /// 工程任务层次码
        /// </summary>
        virtual public string ProjectTaskSysCode
        {
            get { return projectTaskSysCode; }
            set { projectTaskSysCode = value; }
        }
        ///<summary>
        /// 劳务队伍类型
        /// </summary>
        virtual public string UsedRankType
        {
            get { return usedRankType; }
            set { usedRankType = value; }
        }
        /////<summary>
        ///// 劳务队伍类型名称
        ///// </summary>
        //virtual public string UsedRankTypeName
        //{
        //    get { return usedRankTypeName; }
        //    set { usedRankTypeName = value; }
        //}
        /// <summary>
        /// 工程量计量单位
        /// </summary>
        virtual public StandardUnit ProjectQuantityUnit
        {
            get { return projectQuantityUnit; }
            set { projectQuantityUnit = value; }
        }
        /// <summary>
        /// 工程计量单位名称
        /// </summary>
        virtual public string ProjectQuantityUnitName
        {
            get { return projectQuantityUnitName; }
            set { projectQuantityUnitName = value; }
        }
        /// <summary>
        /// 工期计量单位
        /// </summary>
        virtual public StandardUnit ProjectTimeLimitUnit
        {
            get { return projectTimeLimitUnit; }
            set { projectTimeLimitUnit = value; }
        }
        /// <summary>
        /// 工期计量单位名称
        /// </summary>
        virtual public string ProjectTimeLimitUnitName
        {
            get { return projectTimeLimitUnitName; }
            set { projectTimeLimitUnitName = value; }
        }
        /// <summary>
        /// 劳务队伍进场时间
        /// </summary>
        virtual public DateTime? LaborRankInTime
        {
            get { return laborRankInTime; }
            set { laborRankInTime = value; }
        }
        /// <summary>
        /// 预计工程量
        /// </summary>
        virtual public decimal EstimateProjectQuantity
        {
            get { return estimateProjectQuantity; }
            set { estimateProjectQuantity = value; }
        }
        /// <summary>
        /// 预计工期
        /// </summary>
        virtual public decimal EstimateProjectTimeLimit
        {
            get { return estimateProjectTimeLimit; }
            set { estimateProjectTimeLimit = value; }
        }
        /// <summary>
        /// 质量安全专业要求
        /// </summary>
        virtual public string QualitySafetyRequirement
        {
            get { return qualitySafetyRequirement; }
            set { qualitySafetyRequirement = value; }
        }
        /// <summary>
        /// 主要工作内容描述
        /// </summary>
        virtual public string MainJobDescript
        {
            get { return mainJobDescript; }
            set { mainJobDescript = value; }
        }
        ///// <summary>
        ///// 劳务队伍类型
        ///// </summary>
        //virtual public string LaborRankType
        //{
        //    get { return laborRankType; }
        //    set { laborRankType = value; }
        //}
        /// <summary>
        /// 针对的GWBS
        /// </summary>
        virtual public string CorrespondGWBS
        {
            get { return correspondGWBS; }
            set { correspondGWBS = value; }
        }
        /// <summary>
        /// 计划劳动力需求数量
        /// </summary>
        virtual public decimal PlanLaborDemandNumber
        {
            get { return planLaborDemandNumber; }
            set { planLaborDemandNumber = value; }
        }
    }
}
