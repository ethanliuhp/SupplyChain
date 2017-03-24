using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
//using Application.Business.Erp.SupplyChain.CostManagement.EndAccountAuditMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Service
{
    public interface ISubContractBalanceBillSrv
    {

        
        #region 终结分包相关操作   
        EndAccountAuditBill GetEndAccountAuditBillById(string id);
        /// <summary>
        /// 保存或修改终结分包
        /// </summary>
        /// <param name="list">终结分包</param>
        /// <returns></returns>
        IList SaveOrUpdateEndAccountAuditBill(IList list);

        /// <summary>
        /// 保存或修改分包结算单
        /// </summary>
        /// <param name="bill">分包结算单</param>
        /// <returns></returns>
        EndAccountAuditBill SaveOrUpdateEndAccountAuditBill(EndAccountAuditBill bill);


        /// <summary>
        /// 保存或修改分包结算单明细
        /// </summary>
        /// <param name="bill">分包结算单</param>
        /// <returns></returns>
        EndAccountAuditDetail SaveOrUpdateEndAccountAuditBillDetail(EndAccountAuditDetail bill);

        /// <summary>
        /// 保存或修改终结分包
        /// </summary>
        /// <param name="list">终结分包</param>
        /// <returns></returns>
        IList SaveOrUpdateEndAccountAuditBillDetailList(IList list);

        /// <summary>
        /// 删除终结分包
        /// </summary>
        /// <param name="bill">终结分包</param>
        /// <returns></returns>
        bool DeleteEndAccountAuditBill(EndAccountAuditBill bill);


        //IList GenerateEndAccountAuditBill(EndAccountAuditBill subBill, CurrentProjectInfo projectInfo);

        #endregion


        SubContractBalanceBill GetSubContractBalanceMasterById(string id);

        /// <summary>
        /// 保存或修改分包结算单集合
        /// </summary>
        /// <param name="list">分包结算单集合</param>
        /// <returns></returns>
        IList SaveOrUpdateSubContractBalanceBill(IList list);

        /// <summary>
        /// 保存或修改分包结算单
        /// </summary>
        /// <param name="bill">分包结算单</param>
        /// <returns></returns>
        SubContractBalanceBill SaveOrUpdateSubContractBalanceBill(SubContractBalanceBill bill);

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

        SubContractBalanceBill GenSubBalanceBill(SubContractBalanceBill subBill, SubContractProject subProject, CurrentProjectInfo projectInfo);
        SubContractBalanceBill SaveSubContractBalBill(SubContractBalanceBill subBill);
        void DeleteSubContractBalBill(SubContractBalanceBill subBill);
        void UpdateSubContractProjectByAgree(string billId);
        void UpdateSubContractProjectByDisAgree(string billId);

        /// <summary>
        /// 计算分包措施费任务明细的结算明细数据
        /// </summary>
        /// <param name="listMeansureDtl">分包措施费任务明细集</param>
        /// <returns></returns>
        List<SubContractBalanceDetail> BalanceSubContractFeeDtl(SubContractProject subProject, List<GWBSDetail> listMeansureDtl, List<SubContractBalanceDetail> listCurrBalDtl);
        DataSet SubContractBalanceQuery(string condition);
        DataSet GetPerCode(string condition);
        Hashtable GetCostSubjectNameList();
        void UpdateBillSubjectInfo(IList dataList, List<DataDomain> lstMaterial, int billType);

        IList GenerateProjectSubContractBalance(SubContractBalanceBill subBill, CurrentProjectInfo projectInfo);

        bool DeleteProjectSubContractBalance(SubContractBalanceBill bill);

        #region 自动将预算体系上科目与资源与分包结算单耗用进行同步
        int ExeAutoSyn(string sProjectId);
        DataDomain GetAutoSynCount(string sProjectId);
        #endregion
    }
}
