using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;

namespace Application.Business.Erp.SupplyChain.PMCAndWarning.Service
{

    public interface IPMCAndWarningSrv
    {
        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        Object GetObjectById(Type entityType, string Id);

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        DateTime GetServerTime();

        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IList SaveOrUpdate(IList list);

        /// <summary>
        /// 保存或修改对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        object SaveOrUpdate(object obj);

        /// <summary>
        /// 删除对象或对象集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteObjList(IList list);

        /// <summary>
        /// 预警信息写入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IList WriteWarningInfo(IList listWarnInfo, StateCheckAction checkAction, CurrentProjectInfo projectInfo);

        /// <summary>
        /// 获取预警信息
        /// </summary>
        /// <param name="userJob">用户的岗位(当操作的工程项目为NULL时使用)</param>
        /// <param name="optProject">操作的工程项目；公司层面操作多个项目时，该值设置为NULL</param>
        /// <param name="listRole">用户所具有的角色集合</param>
        /// <param name="pushMode">推送方式</param>
        /// <returns></returns>
        List<WarningInfo> GetWarningInfoByUserInfo(OperationJob userJob, CurrentProjectInfo optProject, List<OperationRole> listRole, WarningPushModeEnum pushMode);

        /// <summary>
        /// 获取预警信息
        /// </summary>
        /// <param name="userOrgSysCode">用户所属组织层次码(当操作的工程项目为NULL时使用)</param>
        /// <param name="optProject">操作的工程项目；公司层面操作多个项目时，该值设置为NULL</param>
        /// <param name="listRole">用户所具有的角色集合</param>
        /// <param name="pushMode">推送方式</param>
        /// <returns></returns>
        List<WarningInfo> GetWarningInfoByUserInfo(string userOrgSysCode, CurrentProjectInfo optProject, List<OperationRole> listRole, WarningPushModeEnum pushMode);

        /// <summary>
        /// 启动预警服务
        /// </summary>
        /// <returns></returns>
        bool StartWarningServer();

        /// <summary>
        /// 停止预警服务
        /// </summary>
        /// <returns></returns>
        bool StopWarningServer();

        /// <summary>
        /// 工期状态检查
        /// </summary>
        /// <param name="projectIds">项目Id集</param>
        /// <param name="isShowFlag">显示标记</param>
        /// <param name="isBuildWarningFlag">预警标记</param>
        /// <param name="currProject">当前操作的项目</param>
        /// <returns></returns>
        List<StateCheckActionValueObject> CheckDurationState(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);

        /// <summary>
        /// 资料状态检查
        /// </summary>
        /// <param name="projectIds">项目Id集</param>
        /// <param name="isShowFlag">显示标记</param>
        /// <param name="isBuildWarningFlag">预警标记</param>
        /// <param name="currProject">当前操作的项目</param>
        /// <returns></returns>
        List<StateCheckActionValueObjectOnMeans> CheckInformationState(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);



        #region 商务综合指标检查

        /// <summary>
        /// 任务收款状态检查
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        /// <returns></returns>
        List<OwnerQuantity> CheckTaskProceedsState(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);

        /// <summary>
        /// 业主报量指标检查
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        /// <returns></returns>
        List<OwnerQuantity> CheckOwnerQuantityTarget(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);

        #endregion

        #region 物资预警
        /// <summary>
        /// 获取采购合同预警数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        DataTable GetSupplyOrderWarningInfoDetail(string projectId);

        /// <summary>
        /// 物资采购合同
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void CheckDurationSupplyOrder(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);

        /// <summary>
        /// 获取物资收料预警数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        DataTable GetStockInWarningInfoDetail(string projectId);

        /// <summary>
        /// 物资收料管理
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void CheckDurationStockIn(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);

        /// <summary>
        /// 获取物资月度需求计划预警数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        DataTable GetMonthPlanWarningInfoDetail(string projectId);

        /// <summary>
        /// 获取物资日常需求计划预警数据
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        DataTable GetDailyPlanWarningInfoDetail(string projectId);

        /// <summary>
        /// 物资日常需求计划
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void CheckDurationDailyPlan(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);

        /// <summary>
        /// 物资月度需求计划
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void CheckDurationMonthPlan(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);
        #endregion

        /// <summary>
        /// 获取上一个周日的日期
        /// </summary>
        /// <returns></returns>
        DateTime GetPreviousSundayDate();

        #region 整改单预警
        /// <summary>
        /// 整改单 预警
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void CheckRectificatNoticeMaster(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);
        #endregion

        #region 专业检查 预警
        /// <summary>
        /// 专业检查 预警
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void CheckProfessionInspectionRecordMaster(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);
        #endregion

        /// <summary>
        /// 工单商务复核
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void WorkOrderBusinessReview(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);
        /// <summary>
        /// 设备租赁费用结算
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void RentalCostClear(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);
        /// <summary>
        /// 费用结算
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void CostClear(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);
         /// <summary>
        /// 成本核算
        /// </summary>
        /// <param name="projectIds"></param>
        /// <param name="isShowFlag"></param>
        /// <param name="isBuildWarningFlag"></param>
        /// <param name="currProject"></param>
        void Costing(List<string> projectIds, bool isShowFlag, bool isBuildWarningFlag, CurrentProjectInfo currProject);
    }
}
