using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service
{
    /// <summary>
    /// 专项管理费用
    /// </summary>
    public interface ISpecialCostSrv : IBaseService
    {

        #region 专项管理费用
        /// <summary>
        /// 通过ID查询专项管理费用信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SpecialCostMaster GetSpecialCostById(string id);
        /// <summary>
        /// 通过Code查询专项管理费用信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        SpecialCostMaster GetSpecialCostByCode(string code);
        /// <summary>
        /// 查询专项管理费用信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetSpecialCost(ObjectQuery objectQuery);
        
        [TransManager]
        SpecialCostMaster SaveSpecialCost(SpecialCostMaster obj);
        #endregion

        #region 专项费用结算

        SpeCostSettlementMaster GetSpecialAccountById(string id);
        SpeCostSettlementMaster GetSpecialAccountByCode(string code);
        IList GetSpecialAccount(ObjectQuery objectQuery);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        IList GetSpecialDetailAccount(ObjectQuery objectQuery);
        SpeCostSettlementMaster SaveSpecialAccount(SpeCostSettlementMaster obj);

        #endregion

        #region 产值管理
        
        /// <summary>
        /// 计划产值计算服务
        /// </summary>
        /// <param name="schedulePlan">周/月进度计划</param>
        /// <param name="optProject">操作的项目</param>
        ProduceSelfValueMaster PlanedOutputValueAccount(WeekScheduleMaster schedulePlan, CurrentProjectInfo optProject, OperationOrgInfo org);
        DataSet OutPutValueQuery(string condition);

        bool RecountPlanOutputValue(CurrentProjectInfo project);

        bool RecountRealOutputValue();
        #endregion
    }
}
