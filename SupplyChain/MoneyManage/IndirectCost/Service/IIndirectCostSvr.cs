using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.Data;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service
{
    public interface IIndirectCostSvr
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        bool Delete(object obj);

        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        object Save(object obj);
        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        object Update(object obj, IList movedDtlList);
        /// <summary>
        /// 通过ID查询财务综合数据主表
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        IndirectCostMaster GetIndirectCostMasterByID(string strID);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="oQuery"></param>
        /// <returns></returns>
        IList QueryIndirectCostMaster(ObjectQuery oQuery);
        IList Query(Type type, ObjectQuery oQuery);
        BorrowedOrderMaster SaveBorrowedOrderMaster(BorrowedOrderMaster obj);
        /// <summary>
        /// 根据ID获取对应的对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sID"></param>
        /// <returns></returns>
        object Get(Type type, string sID);
        void CalulationProjectState();
        void CompanyKeyInfoService(DateTime currDate);
        // 取分公司信息
        IList QuerySubAndCompanyOrgInfo();
        //取票据台账
        DataSet QueryAcceptanceBillReport(DateTime start, DateTime end, string projectID, string syscode);
        //取费用信息
        DataSet QueryAccountMng(string SQL);
        IList QueryDayCashDetailReport(DateTime currDate, string syscode);
        IList QueryDayCashFlowReport(DateTime currDate, string projectID, string syscode, bool ifSelf);
        IList QueryCompanyIndexInfoByDate(DateTime currDate);
        string GetProjectIDByOperationOrg(string opgID);
        CurrentProjectInfo GetProjectByOperationOrg(string opgSysCode);

        T GetMasterByID<T>(string id) where T : class;
        List<T> Query<T>(ObjectQuery oQuery);
        object GetMasterByID(Type type, string id);
        /// <summary>
        /// 在公司和分公司情况下 根据当前所属组织和截止时间获取最近一个月的预算金额
        /// </summary>
        /// <param name="currDate"></param>
        /// <param name="syscode"></param>
        /// <returns></returns>
        Hashtable GetNextCostBudgetMoney(string syscode);
        DataSet QueryManageCostGroupAccountTitle(string sOrgSysCode, string sProjectID, DateTime dStart, DateTime dEnd);
        DataSet QueryManageCostGroupOrg(string sOrgSysCode,string sProjectID,  DateTime dStart, DateTime dEnd);
        DataSet QueryIndirectCostGroupAccountTitle(string sOrgSysCode, string sProjectId, DateTime dStart, DateTime dEnd);
        DataSet QueryIndirectCostGroupProject(string sOrgSysCode, DateTime dStart, DateTime dEnd);
        DataSet QueryOwnerQuantityReport(string sProjectID, string sOrgSysCode, DateTime dStart, DateTime dEnd);
        //查询 "局借款台账", "财务费用台账", "利润上交、货币上交台帐" 数据
        DataSet QueryFinancialAccount(string sProjectID, string sOrgSysCode, DateTime dStart, DateTime dEnd, EnumAccountSymbol AccountSymbol, bool isIncludeProj);

        ProjectFundPlanMaster GetProjectFundPlanMaster(string projctId, int year, int month,string type);

        void SaveProjectFundPlan(ProjectFundPlanMaster fpMaster);
    }
}
