using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
    /// <summary>
    /// 生产管理服务接口
    /// </summary>
    public interface IProductionManagementSrv : IBaseService
    {
        #region 进度计划方法
        /// <summary>
        /// 查询进度计划
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetSchedules(ObjectQuery oq);

        /// <summary>
        /// 根据ID查询进度计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProductionScheduleMaster GetSchedulesById(string id);

        /// <summary>
        /// 根据进度计划类型查询进度计划
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        ProductionScheduleMaster GetSchedulesByType(ObjectQuery oq);

        /// <summary>
        /// 保存进度计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ProductionScheduleMaster SaveSchedule(ProductionScheduleMaster obj);

        /// <summary>
        /// 保存进度计划
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        ProductionScheduleMaster SaveSchedule(ProductionScheduleMaster obj, IList detailList);

        /// <summary>
        /// 查询节点的子节点数
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        int CountChildNodes(ProductionScheduleDetail parentNode);

        /// <summary>
        /// 查询节点的子节点数
        /// </summary>
        /// <param name="parentNode">ProductionScheduleDetail</param>
        /// <returns></returns>
        int CountChildNodes(string parentNodeId);

        /// <summary>
        /// 查询节点下的所有子节点数
        /// </summary>
        /// <param name="parentNode">ProductionScheduleDetail</param>
        /// <returns></returns>
        int CountAllChildNodes(string parentNodeId);

        ProductionScheduleMaster NewSchedule(ProductionScheduleMaster master);

        IList GetChilds(ProductionScheduleMaster master);

        /// <summary>
        /// 删除一条进度计划明细 并删除其下的所有子节点
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="childCount">子节点总数（输出参数）</param>
        /// <param name="errMsg">异常信息</param>
        IList DeleteScheduleDetail(ProductionScheduleDetail detail, int childCount, string errMsg);


        /// <summary>
        /// 发布一条进度计划明细 并发布其下的所有子节点
        /// </summary>
        /// <param name="detailId"></param>
        void PublishScheduleDetail(string detailId);

        /// <summary>
        /// 查询节点下的所有子节点
        /// </summary>
        /// <param name="parentNodeId">ProductionScheduleDetail</param>
        /// <returns></returns>
        IList GetAllChilds(string parentNodeId);

        /// <summary>
        /// 滚动计划引用
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetProductionScheduleDetailForQuery(ObjectQuery oq);

        /// <summary>
        /// 保存滚动进度计划明细（生成时调用）
        /// </summary>
        /// <param name="listPlanDtl"></param>
        /// <returns></returns>
        ProductionScheduleMaster SaveScrollPlanDtl(IList listPlanDtl);

        void UpdateScrollPlanDtlSyscode(string masterId);

        #endregion

        #region 周进度计划方法

        WeekScheduleMaster NewWeekSchedule(WeekScheduleMaster master);

        WeekScheduleMaster CopyNewSchdulePlan(WeekScheduleMaster targetPlan);

        void SaveWeekPlanDtl(IList listPlanDtl, IList list_Del);

        void SaveWeekPlanDtl(IList listPlanDtl);

        void SaveWeekPlanDtl(IList listPlanDtl, IList list_Add, IList list_Del);

        void SaveWeekPlanDtl(IList listPlanDtl,IList list_Add,bool flag);

        void SaveUpdateWeekPlanDtl(IList listPlanDtl);

        IList SaveUpdateWeekPlanDtl(IList listPlanDtl, IList listPlanDtlRalation);

        WeekScheduleMaster CreateSubSchdulePlan(WeekScheduleMaster newPlan, IList selDetails);

        /// <summary>
        /// 查询周进度计划
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetWeekScheduleMaster(ObjectQuery oq);

        IList GetWeekScheduleMasterOQ(ObjectQuery oq);
        IList GetWeekDetail(ObjectQuery oq);
        /// <summary>
        /// 根据ID查询周进度计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WeekScheduleMaster GetWeekScheduleMasterById(string id);

        /// <summary>
        /// 根据Code查询周进度计划
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        WeekScheduleMaster GetWeekScheduleMasterByCode(ObjectQuery oq);

        /// <summary>
        /// 保存周进度计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        WeekScheduleMaster SaveWeekScheduleMaster(WeekScheduleMaster obj);


        /// <summary>
        /// 执行进度计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet WeekScheduleQuery(string condition);

        /// <summary>
        /// 查询执行进度计划明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetWeekScheduleDetail(ObjectQuery oq);

        /// <summary>
        /// 删除 汇总周计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteSummaryWeekScheduleMaster(WeekScheduleMaster obj);

        /// <summary>
        /// 保存 汇总周计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        WeekScheduleMaster SaveSummaryWeekScheduleMaster(WeekScheduleMaster obj);

        int DeleteWeekPlan(WeekScheduleMaster weekPlan);
        #endregion

        #region 工程任务确认单方法
        /// <summary>
        /// 查询工程任务确认单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetGWBSTaskConfirmMaster(ObjectQuery oq);

        /// <summary>
        /// 根据Id查询工程任务确认单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GWBSTaskConfirmMaster GetGWBSTaskConfirmMasterById(string id);
        /// <summary>
        /// 保存工程任务确认单明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        GWBSTaskConfirm SaveGWBSTaskConfirm(GWBSTaskConfirm obj);
        IList  SaveGWBSTaskConfirm(IList  lst);
        IList SaveGWBSTaskConfirm1(IList lst);
        IList SaveGWBSTaskConfirmNow(IList lst);
        /// <summary>
        /// 根据Code查询工程任务确认单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        GWBSTaskConfirmMaster GetGWBSTaskConfirmMasterByCode(ObjectQuery oq);
        IList SearchGWBSDetail(string strSysCode);
        DataSet SearchGWBSDetail(string sGWBSTreeSyCode, string sProjectID);
        /// <summary>
        /// 根据该节点syscode 查找它的父节点是否含有临建类型的工程wbs节点
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sTrreNodeSysCode"></param>
        /// <returns></returns>
        bool IsTempBuildProjectTypeNode(string sProjectID, string sTrreNodeSysCode);
        /// <summary>
        /// 针对现场生产的 根据该工程wbs树找到该节点的隶属关系(往上找)最近的工程任务明细的集合
        /// </summary>
        /// <param name="sGWBSTreeSyCode"></param>
        /// <param name="sProjectID"></param>
        /// <returns></returns>
        DataSet SearchGWBSDetailNow(string sGWBSTreeSyCode, string sProjectID);
        IList SearchGWBSDetail(GWBSTree oGWBSTree, PersonInfo oLoginPersonInfo, OperationOrgInfo oTheOperationOrgInfo);
        /// <summary>
        /// 保存工程任务确认单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        GWBSTaskConfirmMaster SaveGWBSTaskConfirmMaster(GWBSTaskConfirmMaster obj);

        /// <summary>
        /// 查询工程任务确认明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetGWBSTaskConfirm(ObjectQuery oq);

        /// <summary>
        /// 根据工程任务Id查询工程任务明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList GetGWBSDetailByParentId(string id);

        /// <summary>
        /// 查询工程任务明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetGWBSDetail(ObjectQuery oq);

        /// <summary>
        /// 查询当前节点的父节点和所有的子节点
        /// </summary>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        List<GWBSTree> GetGWBSTree(GWBSTree currentNode);

        /// <summary>
        /// 查询生产节点
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetGWBSTaskConfirmNode(ObjectQuery oq);

        /// <summary>
        /// 删除工程任务确认
        /// </summary>
        /// <param name="_GWBSTaskConfirmMaster"></param>
        /// <returns></returns>
        bool DeleteGWBSTaskConfirmMaster(GWBSTaskConfirmMaster _GWBSTaskConfirmMaster);
        /// <summary>
        /// 删除工程任务确认明细
        /// </summary>
        /// <param name="_GWBSTaskConfirmMaster"></param>
        /// <returns></returns>
        bool DeleteGWBSTaskConfirm(GWBSTaskConfirm confirm);
        /// <summary>
        /// 保存工程任务确认单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        GWBSTaskConfirmMaster SaveGWBSTaskConfirmMaster2(GWBSTaskConfirmMaster obj);

        /// <summary>
        /// 删除工程任务确认
        /// </summary>
        /// <param name="_GWBSTaskConfirmMaster"></param>
        /// <returns></returns>
        bool DeleteGWBSTaskConfirmMaster2(GWBSTaskConfirmMaster _GWBSTaskConfirmMaster);

        /// <summary>
        /// 根据工程任务层次码 查询周计划明细 用于生成生产节点
        /// </summary>
        /// <param name="gwbsSysCode"></param>
        /// <returns></returns>
        DataSet QueryNode(string gwbsSysCode);

        /// <summary>
        /// 判断工程任务明细是否需要做结算
        /// </summary>
        /// <param name="_GWBSDetailId"></param>
        /// <returns></returns>
        bool IfSubBalance(string _GWBSDetailId);

        /// <summary>
        /// 本队伍已确认累计工程量
        /// </summary>
        /// <param name="info">当前操作项目</param>
        /// <param name="dtl">(操作{工程任务确认明细})_【被确认工程任务明细】</param>
        /// <param name="sub">(操作{工程任务确认明细})_【承担者】</param>
        /// <returns></returns>
        decimal GetTheTeamQuantityConfirmed(CurrentProjectInfo info, GWBSDetail dtl, SubContractProject sub);
        #endregion

        #region 检查记录方法
        InspectionRecord GetInspectionRecordById(string id);
        IList GetInspectionRecord(ObjectQuery objectQuery);
        DataSet GetInspectionRecordQuery(string condition);
        InspectionRecord SaveInsMaster(InspectionRecord obj);
        InspectionRecord SaveInsMaster(InspectionRecord obj,bool isSubmit);
        InspectionRecord SaveInspectialRecordMaster(InspectionRecord obj, WeekScheduleDetail weekDetail);
        bool DeleteInspectionRecord(InspectionRecord obj);
        #endregion

        #region
        /// <summary>
        /// 查询检查记录根据parentId(周计划明细)
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetInspectionRecordMaster(ObjectQuery oq);

        /// <summary>
        /// 查询相关文档根据parentId(检查记录id)
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetSitePictureVideoMaster(ObjectQuery oq);

        /// <summary>
        /// 根据parentId获取符合条件的检查专业
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetInspectialSpecialMaster(ObjectQuery oq);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ir"></param>
        /// <returns></returns>
        string SaveInspectialRecordMaster(InspectionRecord ir);

        /// <summary>
        /// 通过文档id获取一条文档记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SitePictureVideo GetSitePictureVideoById(string id);
        #endregion

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
        IList GetByWeekDetail(WeekScheduleDetail code);
        /// <summary>
        /// 修改周计划明细并更新wbs相关检查状态以及确认单检查状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        WeekScheduleDetail UpdateWeekScheduleDetail(WeekScheduleDetail obj);
        /// <summary>
        /// 更新周计划明细和滚动进度计划
        /// </summary>
        /// <param name="weekPlanDtl"></param>
        /// <returns></returns>
        WeekScheduleDetail UpdateWeekScheduleDetailAndScrollPlan(WeekScheduleDetail weekPlanDtl);
        IList SaveOrUpdate(IList list);
        DataSet SearchSQL(string sql);
        bool Delete(IList list);
        DateTime GetServerTime();
        ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item);
        /// <summary>
        /// 根据总进度计划复制一个新的总进度计划
        /// </summary>
        /// <param name="targetPlan">源计划（要复制的计划）</param>
        /// <returns></returns>
        ProductionScheduleMaster CopyNewSchdulePlan(ProductionScheduleMaster targetPlan);

        /// <summary>
        /// 根据总进度计划复制一个新的总滚动进度计划
        /// </summary>
        /// <param name="targetPlan">源计划（要复制的计划）</param>
        /// <returns></returns>
        ProductionScheduleMaster CopyNewScrollSchdulePlan(ProductionScheduleMaster targetPlan);

        /// <summary>
        /// 作废滚动进度计划明细
        /// </summary>
        /// <param name="planDtl">计划明细</param>
        /// <returns></returns>
        bool InvalidScrollSchdulePlanDtl(ProductionScheduleDetail detail, out IList listChilds);

        /// <summary>
        /// 启用（设置生效）滚动进度计划明细
        /// </summary>
        /// <param name="detail">计划明细</param>
        /// <returns></returns>
        bool EffectScrollSchdulePlanDtl(ProductionScheduleDetail detail, IList listEditDtl, out IList listChilds);
        /// <summary>
        /// 通过project修改进度计划明细
        /// </summary>
        /// <param name="list">明细列表</param>
        void UpdateSchdulePlanDtl(List<ProductionScheduleDetail> list);

        /// <summary>
        /// 查询滚动进度计划明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetScrollPlanDtl(ObjectQuery oq, string projectId);

        /// <summary>
        /// 更新滚动进度计划
        /// </summary>
        /// <param name="oMaster"></param>
        /// <param name="listDtl">需要添加的节点集合</param>
        /// <param name="oDelDtl">需要删除的节点</param>
        /// <returns></returns>
        ProductionScheduleMaster UpdateScrollSchedule(ProductionScheduleMaster oMaster, IList listDtl, ProductionScheduleDetail oDelDtl);

        IList GetProChilds(IList list);
        IList GetWeekChilds(WeekScheduleMaster master);
        void DeleteWeekScheduleDetail(WeekScheduleMaster master);
        IList DeleteWeekScheduleDetail(WeekScheduleDetail detail, int childCount, string errMsg);

        /// <summary>
        /// 返回1.核算明细集，2.确认单明细
        /// </summary>
        /// <param name="oqAccount"></param>
        /// <param name="oqConfirmMaster"></param>
        /// <param name="oqConfirm"></param>
        /// <returns></returns>
        IList GetTaskConfirmInfo(ObjectQuery oqAccountMaster, ObjectQuery oqAccountDtl, ObjectQuery oqConfirmMaster, ObjectQuery oqConfirm);

        IList GetFrontSchedule(ObjectQuery oq, string projectId);

        void DeleteProjectDelayInfo(string proId);
        bool CreateProjectDelayDays(string projId);

        IList GetProjectTotalDelayDays(string projId, DateTime beginDate, DateTime endDate);

        IList GetAssignWorkerOrderMasterByOQ(ObjectQuery oq, bool isNeedShowDetail);

        IList GetAssignWorkerOrderMasterByOQ(ObjectQuery oq);

        AssignWorkerOrderMaster SaveAssignWorkerOrderMaster(AssignWorkerOrderMaster curBillMaster);

        AssignWorkerOrderMaster SaveAssignWorkerOrderMaster(AssignWorkerOrderMaster obj, bool isMaintainWSDActualDate);

        AssignWorkerOrderMaster GetAssignWorkerOrderMasterById(string id);

        int CreateAssignWorkerOrderByPlan(WeekScheduleMaster weekPlan);

        IList GetGWBSTreesByInstance(string projectId);
        IList GetGWBSTreesOQ(ObjectQuery oq);

        IList GetGWBSTreesRoot(string projectId);

        IList GetShowWeekScheduleDetails(ObjectQuery oq);

        bool DeleteByIList(IList lstObj);

        void UpdateWeekScheduleMasterState(DocumentState dc, WeekScheduleMaster wsm);
        DataTable GetData(string sSQL);
        string GetAssignTeamLinkMan(string assignTeam);
    }
}
