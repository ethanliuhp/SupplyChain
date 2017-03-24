﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using VirtualMachine.SystemAspect.Security;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data;
using VirtualMachine.Core.DataAccess;
using Application.Resource.FinancialResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.CommonClass.Domain;
using Iesi.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.MaterialResource.Domain;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Service;
using System.Data.OracleClient;
//using Application.Business.Erp.SupplyChain.CostManagement.EndAccountAuditMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Service
{
    /// <summary>
    /// 分包结算单服务
    /// </summary>
    public class SubContractBalanceBillSrv : ISubContractBalanceBillSrv
    {
        private IProjectTaskAccountSrv accSrv;
        /// <summary>
        /// 核算单服务
        /// </summary>
        public IProjectTaskAccountSrv ProjectTaskAccountSrv
        {
            get { return accSrv; }
            set { accSrv = value; }
        }

        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

       
       #region 终结分包相关操作   
    public   EndAccountAuditBill GetEndAccountAuditBillById(string id)
       {
           ObjectQuery objectQuery = new ObjectQuery();
           objectQuery.AddCriterion(Expression.Eq("Id", id));
           objectQuery.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
           objectQuery.AddFetchMode("TheSubContractProject", NHibernate.FetchMode.Eager);
           IList list = ObjectQuery(typeof(EndAccountAuditBill), objectQuery);
           if (list != null && list.Count > 0)
           {
               return list[0] as EndAccountAuditBill;
           }
           return null;
       }
       /// <summary>
       /// 保存或修改终结分包
       /// </summary>
       /// <param name="list">终结分包</param>
       /// <returns></returns>
    public IList SaveOrUpdateEndAccountAuditBill(IList list)
    {
        dao.SaveOrUpdate(list);
        return list;
    }

       /// <summary>
       /// 保存或修改分包结算单明细
       /// </summary>
       /// <param name="bill">分包结算单明细</param>
       /// <returns></returns>
    public EndAccountAuditDetail SaveOrUpdateEndAccountAuditBillDetail(EndAccountAuditDetail bill)
    {
        dao.SaveOrUpdate(bill);
        return bill;
    }

    /// <summary>
    /// 保存或修改分包结算单
    /// </summary>
    /// <param name="bill">分包结算单</param>
    /// <returns></returns>
    public EndAccountAuditBill SaveOrUpdateEndAccountAuditBill(EndAccountAuditBill bill)
    {
        dao.SaveOrUpdate(bill);
        return bill;
    }

    /// <summary>
    /// 保存或修改终结分包
    /// </summary>
    /// <param name="list">终结分包</param>
    /// <returns></returns>
    public IList SaveOrUpdateEndAccountAuditBillDetailList(IList list)
    {
        dao.SaveOrUpdate(list);
        return list;
    }

       /// <summary>
       /// 删除终结分包
       /// </summary>
       /// <param name="bill">终结分包</param>
       /// <returns></returns>
    public bool DeleteEndAccountAuditBill(EndAccountAuditBill bill)
    {
        return dao.Delete(bill);
    }

    //[TransManager]
    //public IList GenerateEndAccountAuditBill(EndAccountAuditBill subBill, CurrentProjectInfo projectInfo)  //这个函数需要重新检查，目前代码是COPY的
    //{
    //    if (subBill == null || projectInfo == null)
    //    {
    //        return null;
    //    }

    //    //var curBillMaster = new ProjectTaskAccountBill();
    //    //curBillMaster.CreateDate = subBill.CreateDate;
    //    //curBillMaster.DocState = DocumentState.Edit;
    //    //curBillMaster.AccountPersonGUID = subBill.CreatePerson;
    //    //curBillMaster.AccountPersonName = subBill.CreatePersonName;
    //    //curBillMaster.OperOrgInfo = subBill.OperOrgInfo;
    //    //curBillMaster.OperOrgInfoName = subBill.OperOrgInfoName;
    //    //curBillMaster.OpgSysCode = subBill.OpgSysCode;
    //    //curBillMaster.ProjectId = projectInfo.Id;
    //    //curBillMaster.ProjectName = projectInfo.Name;
    //    //curBillMaster.AccountRange = subBill.BalanceRange;
    //    //curBillMaster.AccountTaskSyscode = subBill.BalanceTaskSyscode;
    //    //curBillMaster.AccountTaskName = subBill.BalanceTaskName;
    //    //curBillMaster.BeginTime = subBill.BeginTime;
    //    //curBillMaster.EndTime = subBill.EndTime;

    //    //var list = ProjectTaskAccountSrv.GenAccountBillByVirConfirmBill(curBillMaster);
    //    //var confirmTasks = list[1] as List<GWBSTaskConfirm>;
    //    //var taskAccountBill = list[0] as ProjectTaskAccountBill;
    //    //var batchNo = string.Format("{0:yyyyMMddHHmmss}", GetServerTime());
    //    //List<SubContractProject> subProjects = new List<SubContractProject>();
    //    //if (taskAccountBill != null && confirmTasks != null && confirmTasks.Count > 0)
    //    //{
    //    //    taskAccountBill.AuditDate = DateTime.Now;
    //    //    taskAccountBill.AuditPerson = taskAccountBill.CreatePerson;
    //    //    taskAccountBill.AuditPersonName = taskAccountBill.CreatePersonName;
    //    //    taskAccountBill.DocState = DocumentState.InExecute;
    //    //    taskAccountBill.CreateBatchNo = batchNo;
    //    //    taskAccountBill.Descript = "批量生成";
    //    //    taskAccountBill.SubmitDate = GetServerTime();
    //    //    taskAccountBill.CreateYear = taskAccountBill.SubmitDate.Year;
    //    //    taskAccountBill.CreateMonth = taskAccountBill.SubmitDate.Month;
    //    //    ProjectTaskAccountSrv.SaveAccBillAndSetCfmStateByVirCfmBill(taskAccountBill, confirmTasks);

    //    //    foreach (var cfmTask in confirmTasks)
    //    //    {
    //    //        if (!subProjects.Contains(cfmTask.TaskHandler))
    //    //        {
    //    //            subProjects.Add(cfmTask.TaskHandler);
    //    //        }
    //    //    }
    //    //}

    //    //subProjects.AddRange(GetCanSettlementSubContractProject(subBill));
    //    //subProjects = subProjects.Distinct().ToList();
    //    //var bananceList = new List<EndAccountAuditBill>();
    //    //foreach (SubContractProject subProj in subProjects)
    //    //{
    //    //    var newSubBill = GenSubBalanceBill(BuildGenerateBillArgs(subBill, subProj), subProj, projectInfo);
    //    //    newSubBill.DocState = DocumentState.Edit;
    //    //    newSubBill.CreateBatchNo = batchNo;
    //    //    newSubBill.Descript = "批量生成";
    //    //    newSubBill = SaveSubContractBalBill(newSubBill);

    //    //    bananceList.Add(newSubBill);
    //    //}

    //    //return bananceList;
    //}



       #endregion


        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        /// <summary>
        /// 通过ID查询分包结算单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubContractBalanceBill GetSubContractBalanceMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSubContractProject", NHibernate.FetchMode.Eager);
            IList list = ObjectQuery(typeof(SubContractBalanceBill), objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SubContractBalanceBill;
            }
            return null;
        }
        /// <summary>
        /// 根据项目生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }
        #endregion

        #region 通用方法
        /// <summary>
        /// 保存或修改分包结算单集合
        /// </summary>
        /// <param name="list">分包结算单集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateSubContractBalanceBill(IList list)
        {
            dao.SaveOrUpdate(list);
            return list;
        }

        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return dao.Get(entityType, Id);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }
        #endregion

        /// <summary>
        /// 保存或修改分包结算单
        /// </summary>
        /// <param name="bill">分包结算单</param>
        /// <returns></returns>
        [TransManager]
        public SubContractBalanceBill SaveOrUpdateSubContractBalanceBill(SubContractBalanceBill bill)
        {
            dao.SaveOrUpdate(bill);
            return bill;
        }

        #region 分包结算单算法
        /// <summary>
        /// 生成分包结算单算法
        /// </summary>
        /// <param name="subBill">分包结算模型数据</param>
        /// <param name="subProject">分包项目[合同]</param>
        /// <param name="projectInfo">归属项目</param>
        /// <returns></returns>
        public SubContractBalanceBill GenSubBalanceBill(SubContractBalanceBill subBill, SubContractProject subProject, CurrentProjectInfo projectInfo)
        {
            IList list = new ArrayList(); 
            decimal sumBalMoney = 0;//累计结算金额

            #region 1:取工程任务核算单数据
            //1.1 过滤条件[时间][结算节点包含关系][承担者][状态][未结算]
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateDate", subBill.BeginTime));
            oq.AddCriterion(Expression.Le("CreateDate", subBill.EndTime));
            oq.AddCriterion(Expression.Eq("ProjectId", subBill.ProjectId));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.GWBSDetail", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_PTaskAccount = dao.ObjectQuery(typeof(ProjectTaskAccountBill), oq);

            //1.2 查询成本核算科目标志
            Hashtable ht_costsubjectflag = this.GetCostSubjectFlagList();

            //1.3 结算工程任务核算明细
            foreach (ProjectTaskAccountBill model in list_PTaskAccount)
            {
                //工程任务核算明细信息
                foreach (ProjectTaskDetailAccount detail in model.Details)
                {
                    if (detail.BearerGUID.Id == subProject.Id && TransUtil.ToString(detail.BalanceDtlGUID) == ""
                        && TransUtil.ToString(detail.AccountTaskNodeSyscode).Contains(subBill.BalanceTaskSyscode))
                    {
                        decimal sumDtlBalancePrice = 0;
                        decimal sumDtlBalanceMoney = 0;
                        SubContractBalanceDetail billDtl = new SubContractBalanceDetail();
                        billDtl.BalanceTask = detail.AccountTaskNodeGUID;
                        billDtl.BalanceTaskName = detail.AccountTaskNodeName;
                        billDtl.BalanceTaskSyscode = detail.AccountTaskNodeSyscode;
                        billDtl.BalanceTaskDtl = detail.ProjectTaskDtlGUID;
                        billDtl.BalanceTaskDtlName = detail.ProjectTaskDtlName;
                        billDtl.ConfirmQuantity = detail.ConfirmQuantity;
                        billDtl.FontBillType = FrontBillType.工程任务核算;
                        billDtl.FrontBillGUID = detail.Id;
                        billDtl.QuantityUnit = detail.QuantityUnitGUID;
                        billDtl.QuantityUnitName = detail.QuantityUnitName;
                        billDtl.PriceUnit = detail.PriceUnitGUID;
                        billDtl.PriceUnitName = detail.PriceUnitName;
                        billDtl.UseDescript += " " + detail.Descript;
                        billDtl.Remarks = detail.Remark;
                        billDtl.HandlePerson = detail.ResponsiblePerson;
                        billDtl.HandlePersonName = detail.ResponsiblePersonName;

                        //工程任务资源耗用信息
                        foreach (ProjectTaskDetailAccountSubject subject in detail.Details)
                        {
                            if (subject.CostingSubjectGUID == null)//无成本核算科目
                            {
                                continue;
                            }
                            bool ifHaveBal = false;

                            #region 结算方式

                            ifHaveBal = subject.IsBalance;

                            CostAccountSubject temp = (CostAccountSubject)ht_costsubjectflag[subject.CostingSubjectGUID.Id];

                            #endregion

                            if (ifHaveBal == false)
                                continue;

                            //string costSubjectGUID = subject.CostingSubjectGUID.Id;//成本核算科目GUID

                            SubContractBalanceSubjectDtl balanceSubject = new SubContractBalanceSubjectDtl();
                            balanceSubject.CostName = subject.BestaetigtCostSubjectName;
                            //balanceSubject.BalanceQuantity = subject.AccUsageQny;
                            balanceSubject.BalanceQuantity = detail.AccountProjectAmount;
                            balanceSubject.BalancePrice = subject.AccWorkQnyPrice;
                            balanceSubject.BalanceTotalPrice = Math.Round(balanceSubject.BalanceQuantity * balanceSubject.BalancePrice, 2);

                            sumDtlBalancePrice += balanceSubject.BalancePrice;
                            sumDtlBalanceMoney += balanceSubject.BalanceTotalPrice;

                            balanceSubject.QuantityUnit = subject.QuantityUnitGUID;
                            balanceSubject.QuantityUnitName = subject.QuantityUnitName;
                            balanceSubject.PriceUnit = subject.PriceUnitGUID;
                            balanceSubject.PriceUnitName = subject.PriceUnitName;
                            balanceSubject.BalanceSubjectGUID = subject.CostingSubjectGUID;
                            balanceSubject.BalanceSubjectCode = temp.Code;
                            balanceSubject.BalanceSubjectName = subject.CostingSubjectName;
                            balanceSubject.BalanceSubjectSyscode = temp.SysCode;
                            balanceSubject.ResourceTypeGUID = subject.ResourceTypeGUID;
                            balanceSubject.ResourceTypeName = subject.ResourceTypeName;
                            balanceSubject.ResourceTypeSpec = subject.ResourceTypeSpec;
                            balanceSubject.ResourceTypeStuff = subject.ResourceTypeQuality;
                            balanceSubject.ResourceSyscode = subject.ResourceCategorySysCode;
                            balanceSubject.DiagramNumber = subject.DiagramNumber;
                            balanceSubject.FrontBillGUID = subject.Id;
                            balanceSubject.TheBalanceDetail = billDtl;

                            billDtl.Details.Add(balanceSubject);
                        }
                        if (TransUtil.ToString(detail.ProjectTaskDtlGUID.ContractGroupType).Contains("设计变更"))
                        {
                            billDtl.BalanceBase = "设计变更";
                        }
                        else if (TransUtil.ToString(detail.ProjectTaskDtlGUID.ContractGroupType).Contains("签证"))
                        {
                            billDtl.BalanceBase = "签证变更";
                        }
                        else
                        {
                            billDtl.BalanceBase = "合同内";
                        }
                        billDtl.BalacneQuantity = detail.AccountProjectAmount;
                        billDtl.BalanceTotalPrice = sumDtlBalanceMoney;
                        billDtl.BalancePrice = sumDtlBalancePrice;
                        //billDtl.BalanceTotalPrice = Math.Round(billDtl.BalancePrice * billDtl.BalacneQuantity, 2);
                        sumBalMoney += billDtl.BalanceTotalPrice;
                        billDtl.Master = subBill;
                        subBill.Details.Add(billDtl);
                    }
                }
            }

            #endregion

            #region 2:取罚扣款单数据
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateDate", subBill.BeginTime));
            oq.AddCriterion(Expression.Le("CreateDate", subBill.EndTime));
            oq.AddCriterion(Expression.Eq("ProjectId", subBill.ProjectId));
            oq.AddCriterion(Expression.Eq("PenaltyDeductionRant.Id", subProject.Id));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list_PenaltyDeduction = dao.ObjectQuery(typeof(PenaltyDeductionMaster), oq);
            foreach (PenaltyDeductionMaster model in list_PenaltyDeduction)
            {
                //罚扣款单明细信息
                foreach (PenaltyDeductionDetail detail in model.Details)
                {
                    if (TransUtil.ToString(detail.BalanceDtlGUID) == "" && subBill.TheSubContractProject.Id == model.PenaltyDeductionRant.Id
                        && TransUtil.ToString(detail.ProjectTaskSyscode).Contains(subBill.BalanceTaskSyscode))
                    {
                        SubContractBalanceDetail billDtl = new SubContractBalanceDetail();
                        billDtl.BalanceTask = detail.ProjectTask;
                        billDtl.BalanceTaskName = detail.ProjectTaskName;
                        billDtl.BalanceTaskSyscode = detail.ProjectTaskSyscode;
                        billDtl.BalanceTaskDtl = detail.ProjectTaskDetail;
                        billDtl.BalanceTaskDtlName = detail.TaskDetailName;
                        billDtl.BalanceBase = "罚款";
                        //billDtl.UseDescript += " " + detail.Cause;
                        billDtl.Remarks += " " + detail.Cause;

                        if (model.PenaltyType != PenaltyDeductionType.代工扣款)
                        {
                            billDtl.BalacneQuantity = 1;
                            billDtl.BalanceTotalPrice = -(detail.PenaltyMoney);
                            billDtl.BalancePrice = -(detail.PenaltyMoney);
                        }
                        else
                        {
                            billDtl.BalacneQuantity = detail.AccountQuantity;
                            billDtl.BalanceTotalPrice = -(detail.AccountMoney);
                            billDtl.BalancePrice = -(detail.AccountPrice);
                            billDtl.BalanceBase = "代工";
                        }
                        billDtl.FrontBillGUID = detail.Id;
                        billDtl.QuantityUnit = detail.ProductUnit;
                        billDtl.QuantityUnitName = detail.ProductUnitName;
                        billDtl.PriceUnit = detail.MoneyUnit;
                        billDtl.PriceUnitName = detail.MoneyUnitName;

                        //sumBalMoney += billDtl.BalanceTotalPrice;
                        if (model.PenaltyType != PenaltyDeductionType.代工扣款)
                        {
                            if (model.PenaltyType == PenaltyDeductionType.暂扣款 || model.PenaltyType == PenaltyDeductionType.暂扣款红单)
                            {
                                billDtl.FontBillType = FrontBillType.暂扣款单;
                            }
                            else
                            {
                                billDtl.FontBillType = FrontBillType.罚款单;

                            }
                        }
                        else
                        {
                            billDtl.FontBillType = FrontBillType.扣款单;
                        }
                        CostAccountSubject temp = (CostAccountSubject)ht_costsubjectflag[detail.PenaltySubjectGUID.Id];
                        if (temp.IfSubBalanceFlag != 2)
                        {   
                            //生成物资耗用明细
                            SubContractBalanceSubjectDtl balanceSubject = new SubContractBalanceSubjectDtl();
                            balanceSubject.CostName = detail.PenaltySubject;
                            balanceSubject.BalanceQuantity = billDtl.BalacneQuantity;
                            balanceSubject.BalancePrice = billDtl.BalancePrice;
                            balanceSubject.BalanceTotalPrice = billDtl.BalanceTotalPrice;
                            balanceSubject.QuantityUnit = detail.ProductUnit;
                            balanceSubject.QuantityUnitName = detail.ProductUnitName;
                            balanceSubject.PriceUnit = detail.MoneyUnit;
                            balanceSubject.PriceUnitName = detail.MoneyUnitName;
                            balanceSubject.BalanceSubjectGUID = detail.PenaltySubjectGUID;
                            balanceSubject.BalanceSubjectName = detail.PenaltySubject;
                            balanceSubject.BalanceSubjectCode = temp.Code;
                            balanceSubject.BalanceSubjectSyscode = detail.PenaltySysCode;
                            balanceSubject.ResourceTypeGUID = detail.ResourceType.Id;
                            balanceSubject.ResourceTypeName = detail.ResourceTypeName;
                            balanceSubject.ResourceTypeSpec = detail.ResourceTypeSpec;
                            balanceSubject.ResourceTypeStuff = detail.ResourceTypeStuff;
                            balanceSubject.ResourceSyscode = detail.ResourceSysCode;
                            balanceSubject.DiagramNumber = detail.DiagramNumber;
                            balanceSubject.FrontBillGUID = detail.Id;
                            balanceSubject.TheBalanceDetail = billDtl;
                            billDtl.Details.Add(balanceSubject);
                        }
                        billDtl.Master = subBill;
                        subBill.Details.Add(billDtl);
                    }
                }
            }
            #endregion

            #region 3:取零星用工单数据
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateDate", subBill.BeginTime));
            oq.AddCriterion(Expression.Le("CreateDate", subBill.EndTime));
            oq.AddCriterion(Expression.Eq("ProjectId", subBill.ProjectId));
            oq.AddCriterion(Expression.Eq("BearTeam.Id", subProject.Id));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list_LaborSporadic = dao.ObjectQuery(typeof(LaborSporadicMaster), oq);
            foreach (LaborSporadicMaster model in list_LaborSporadic)
            {
                //零星用工单明细信息
                foreach (LaborSporadicDetail detail in model.Details)
                {
                    if (TransUtil.ToString(detail.BalanceDtlGUID) == "" && subBill.TheSubContractProject.Id == model.BearTeam.Id
                            && TransUtil.ToString(detail.ProjectTaskSyscode).Contains(subBill.BalanceTaskSyscode))
                    {
                        SubContractBalanceDetail billDtl = new SubContractBalanceDetail();
                        billDtl.BalanceTask = detail.ProjectTast;
                        billDtl.BalanceTaskName = detail.ProjectTastName;
                        billDtl.BalanceTaskSyscode = detail.ProjectTaskSyscode;
                        billDtl.BalanceTaskDtl = detail.ProjectTastDetail;
                        billDtl.BalanceTaskDtlName = detail.ProjectTastDetailName;
                        billDtl.BalacneQuantity = detail.AccountLaborNum;
                        billDtl.ConfirmQuantity = detail.RealLaborNum;
                        billDtl.BalancePrice = detail.AccountPrice;
                        billDtl.BalanceTotalPrice = detail.AccountSumMoney;
                        billDtl.FrontBillGUID = detail.Id;
                        billDtl.QuantityUnit = detail.QuantityUnit;
                        billDtl.QuantityUnitName = detail.QuantityUnitName;
                        billDtl.PriceUnit = detail.PriceUnit;
                        billDtl.PriceUnitName = detail.PriceUnitName;
                        //billDtl.UseDescript += " " + detail.LaborDescript;
                        billDtl.HandlePerson = model.HandlePerson;
                        billDtl.HandlePersonName = model.HandlePersonName;
                        if (model.LaborState != null && model.LaborState.IndexOf("计时") != -1)
                        {
                            billDtl.FontBillType = FrontBillType.计时派工;
                            billDtl.BalanceBase = "计时工"; 
                        }
                        else if (model.LaborState != null && model.LaborState.IndexOf("签证") != -1)
                        {
                            billDtl.FontBillType = FrontBillType.分包签证;
                            billDtl.BalanceBase = "分包签证";
                        }
                        else if (model.LaborState != null && model.LaborState.IndexOf("代工") != -1)
                        {
                            billDtl.FontBillType = FrontBillType.代工单;
                            billDtl.BalanceBase = "代工";
                        }
                        else
                        {
                            billDtl.FontBillType = FrontBillType.零星用工单;
                        }
                        sumBalMoney += billDtl.BalanceTotalPrice;

                        CostAccountSubject temp = (CostAccountSubject)ht_costsubjectflag[detail.LaborSubject.Id];
                        if (temp.IfSubBalanceFlag != 2)
                        {
                            //生成物资耗用明细
                            SubContractBalanceSubjectDtl balanceSubject = new SubContractBalanceSubjectDtl();
                            balanceSubject.CostName = detail.LaborSubjectName;
                            balanceSubject.BalanceQuantity = detail.AccountLaborNum;
                            balanceSubject.BalancePrice = detail.AccountPrice;
                            balanceSubject.BalanceTotalPrice = detail.AccountSumMoney;
                            balanceSubject.QuantityUnit = detail.QuantityUnit;
                            balanceSubject.QuantityUnitName = detail.QuantityUnitName;
                            balanceSubject.PriceUnit = detail.PriceUnit;
                            balanceSubject.PriceUnitName = detail.PriceUnitName;
                            balanceSubject.BalanceSubjectGUID = detail.LaborSubject;
                            balanceSubject.BalanceSubjectCode = temp.Code;
                            balanceSubject.BalanceSubjectName = detail.LaborSubjectName;
                            balanceSubject.BalanceSubjectSyscode = detail.LaborSubjectSysCode;
                            if (detail.ResourceType != null)
                            {
                                balanceSubject.ResourceTypeGUID = detail.ResourceType.Id;
                            }
                            balanceSubject.ResourceTypeName = detail.ResourceTypeName;
                            balanceSubject.ResourceTypeSpec = detail.ResourceTypeSpec;
                            balanceSubject.ResourceTypeStuff = detail.ResourceTypeStuff;
                            balanceSubject.ResourceSyscode = detail.ResourceSysCode;
                            balanceSubject.DiagramNumber = detail.DiagramNumber;
                            balanceSubject.FrontBillGUID = detail.Id;
                            balanceSubject.TheBalanceDetail = billDtl;
                            billDtl.Details.Add(balanceSubject);
                        }

                        billDtl.Remarks = detail.LaborDescript;
                        billDtl.Master = subBill;
                        subBill.Details.Add(billDtl);
                    }
                }
            }
            #endregion

            if (subBill.Details.Count > 0)
            {
                Hashtable ht_basic = this.GetSubBalBasicData(subBill.BalanceTaskSyscode);

                #region 4:税金
                if (subProject.LaborMoneyType != ManagementRememberMethod.不计取)
                {
                    //4.1 构造税金的分包结算明细信息
                    SubContractBalanceDetail billDtl = new SubContractBalanceDetail();
                    billDtl.FontBillType = FrontBillType.税金;

                    if (ht_basic.Contains("BC15-5"))//成本项: 劳务税金
                    {
                        GWBSTree task = (GWBSTree)ht_basic["BC15-5"];

                        billDtl.BalanceTask = task;
                        billDtl.BalanceTaskName = billDtl.BalanceTask.Name;
                        billDtl.BalanceTaskSyscode = billDtl.BalanceTask.SysCode;
                        billDtl.BalanceTaskDtl = task.Details.ElementAt(0);
                        billDtl.BalanceTaskDtlName = billDtl.BalanceTaskDtl.Name;
                    }
                    else
                    {
                        billDtl.BalanceTask = subBill.BalanceRange;
                        billDtl.BalanceTaskName = billDtl.BalanceTask.Name;
                        billDtl.BalanceTaskSyscode = billDtl.BalanceTask.SysCode;
                    }
                    if (TransUtil.ToString(billDtl.BalanceTaskDtlName) == "")
                    {
                        billDtl.BalanceTaskDtlName = "劳务税金";
                    }

                    if (ht_basic.Contains("项"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["项"];
                        billDtl.QuantityUnitName = unit.Name;
                        billDtl.QuantityUnit = unit;
                    }
                    if (ht_basic.Contains("元"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["元"];
                        billDtl.PriceUnit = unit;
                        billDtl.PriceUnitName = unit.Name;
                    }

                    billDtl.BalacneQuantity = 1;
                    if (subProject.LaborMoneyType == ManagementRememberMethod.按费率计取)
                    {
                        billDtl.BalancePrice = sumBalMoney * subProject.LaobrRace;
                        billDtl.BalanceTotalPrice = sumBalMoney * subProject.LaobrRace;
                    }
                    else if (subProject.ManagementRemMethod == ManagementRememberMethod.据实计取)
                    {
                        billDtl.BalancePrice = 0;
                        billDtl.BalanceTotalPrice = 0;
                    }

                    sumBalMoney += billDtl.BalanceTotalPrice;

                    SubContractBalanceSubjectDtl balanceSubject = new SubContractBalanceSubjectDtl();
                    balanceSubject.BalanceQuantity = 1;
                    balanceSubject.BalancePrice = billDtl.BalancePrice;
                    balanceSubject.BalanceTotalPrice = balanceSubject.BalancePrice;
                    if (ht_basic.Contains("项"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["项"];
                        balanceSubject.QuantityUnitName = unit.Name;
                        balanceSubject.QuantityUnit = unit;
                    }
                    if (ht_basic.Contains("元"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["元"];
                        balanceSubject.PriceUnit = unit;
                        balanceSubject.PriceUnitName = unit.Name;
                    }

                    if (ht_basic.Contains("C5110125"))//成本核算科目: 税金
                    {
                        balanceSubject.BalanceSubjectGUID = (CostAccountSubject)ht_basic["C5110125"];
                        balanceSubject.BalanceSubjectName = balanceSubject.BalanceSubjectGUID.Name;
                        balanceSubject.BalanceSubjectSyscode = balanceSubject.BalanceSubjectGUID.SysCode;
                        balanceSubject.BalanceSubjectCode = balanceSubject.BalanceSubjectGUID.Code;
                        balanceSubject.CostName = balanceSubject.BalanceSubjectName;
                    }

                    if (ht_basic.Contains("R30100000"))//物资资源:管理资源
                    {
                        Material material = (Material)ht_basic["R30100000"];
                        balanceSubject.ResourceTypeGUID = material.Id;
                        balanceSubject.ResourceTypeName = material.Name;
                        balanceSubject.ResourceTypeSpec = material.Specification;
                        balanceSubject.ResourceTypeStuff = material.Stuff;
                        balanceSubject.ResourceSyscode = material.TheSyscode;
                    }

                    balanceSubject.TheBalanceDetail = billDtl;
                    billDtl.Details.Add(balanceSubject);

                    billDtl.Master = subBill;
                    subBill.Details.Add(billDtl);
                }
                #endregion

                #region 5:代缴水电费
                if (subProject.UtilitiesRemMethod != UtilitiesRememberMethod.不计取)
                {
                    //5.1 构造代缴水电费的分包结算明细信息
                    SubContractBalanceDetail billDtl = new SubContractBalanceDetail();
                    if (ht_basic.Contains("K4-8"))//[成本项.定额编号]分包扣水电费
                    {
                        billDtl.BalanceTask = (GWBSTree)ht_basic["K4-8"];
                        billDtl.BalanceTaskName = billDtl.BalanceTask.Name;
                        billDtl.BalanceTaskSyscode = billDtl.BalanceTask.SysCode;
                        billDtl.BalanceTaskDtl = billDtl.BalanceTask.Details.ElementAt<GWBSDetail>(0);
                        billDtl.BalanceTaskDtlName = billDtl.BalanceTaskDtl.Name;
                    }
                    else
                    {
                        billDtl.BalanceTask = subBill.BalanceRange;
                        billDtl.BalanceTaskName = subBill.BalanceTaskName;
                        billDtl.BalanceTaskSyscode = subBill.BalanceTaskSyscode;
                    }
                    if (TransUtil.ToString(billDtl.BalanceTaskDtlName) == "")
                    {
                        billDtl.BalanceTaskDtlName = "水电费";
                    }
                    billDtl.FontBillType = FrontBillType.水电;
                    billDtl.BalacneQuantity = 1;

                    if (ht_basic.Contains("项"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["项"];
                        billDtl.QuantityUnitName = unit.Name;
                        billDtl.QuantityUnit = unit;
                    }
                    if (ht_basic.Contains("元"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["元"];
                        billDtl.PriceUnit = unit;
                        billDtl.PriceUnitName = unit.Name;
                    }

                    if (subProject.UtilitiesRemMethod == UtilitiesRememberMethod.按费率计取)
                    {
                        billDtl.BalancePrice = -(sumBalMoney * subProject.UtilitiesRate);
                        billDtl.BalanceTotalPrice = -(sumBalMoney * subProject.UtilitiesRate);
                    }
                    else if (subProject.UtilitiesRemMethod == UtilitiesRememberMethod.据实计取)
                    {
                        billDtl.BalancePrice = 0;
                        billDtl.BalanceTotalPrice = 0;
                    }
                    //5.2 构造代缴水电费的分包结算分科目信息
                    SubContractBalanceSubjectDtl balanceSubject = new SubContractBalanceSubjectDtl();
                    balanceSubject.BalanceQuantity = 1;
                    balanceSubject.BalancePrice = billDtl.BalancePrice;
                    balanceSubject.BalanceTotalPrice = billDtl.BalanceTotalPrice;
                    if (ht_basic.Contains("项"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["项"];
                        balanceSubject.QuantityUnitName = unit.Name;
                        balanceSubject.QuantityUnit = unit;
                    }
                    if (ht_basic.Contains("元"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["元"];
                        balanceSubject.PriceUnit = unit;
                        balanceSubject.PriceUnitName = unit.Name;
                    }

                    if (ht_basic.Contains("C5110306"))//成本核算科目: 施工用电费
                    {
                        balanceSubject.BalanceSubjectGUID = (CostAccountSubject)ht_basic["C5110306"];
                        balanceSubject.BalanceSubjectName = balanceSubject.BalanceSubjectGUID.Name;
                        balanceSubject.BalanceSubjectCode = balanceSubject.BalanceSubjectGUID.Code;
                        balanceSubject.BalanceSubjectSyscode = balanceSubject.BalanceSubjectGUID.SysCode;
                        balanceSubject.CostName = balanceSubject.BalanceSubjectName;
                    }

                    Material elcRMaterial = GetElectResourceMaterial(TransUtil.ElectResourceMaterial);
                    if (elcRMaterial != null)
                    {
                        balanceSubject.ResourceTypeGUID = elcRMaterial.Id;
                        balanceSubject.ResourceTypeName = elcRMaterial.Name;
                        balanceSubject.ResourceTypeSpec = elcRMaterial.Specification;
                        balanceSubject.ResourceTypeStuff = elcRMaterial.Stuff;
                        balanceSubject.ResourceSyscode = elcRMaterial.TheSyscode;
                    }
                    //if (ht_basic.Contains("I19800000"))//物资资源: 水电资源
                    //{
                    //    Material material = (Material)ht_basic["I19800000"];
                    //    balanceSubject.ResourceTypeGUID = material.Id;
                    //    balanceSubject.ResourceTypeName = material.Name;
                    //    balanceSubject.ResourceTypeSpec = material.Specification;
                    //    balanceSubject.ResourceTypeStuff = material.Stuff;
                    //    balanceSubject.ResourceSyscode = material.TheSyscode;
                    //}

                    balanceSubject.TheBalanceDetail = billDtl;
                    billDtl.Details.Add(balanceSubject);
                    billDtl.Master = subBill;
                    subBill.Details.Add(billDtl);
                }
                #endregion

                #region 6:代缴建设管理费
                if (subProject.ManagementRemMethod != ManagementRememberMethod.不计取)
                {
                    //6.1 构造代缴建设管理费的分包结算明细信息
                    SubContractBalanceDetail billDtl = new SubContractBalanceDetail();
                    if (ht_basic.Contains("K4-7"))//[成本项.定额编号]分包代扣建管费
                    {
                        billDtl.BalanceTask = (GWBSTree)ht_basic["K4-7"];
                        billDtl.BalanceTaskName = billDtl.BalanceTask.Name;
                        billDtl.BalanceTaskSyscode = billDtl.BalanceTask.SysCode;
                        billDtl.BalanceTaskDtl = billDtl.BalanceTask.Details.ElementAt<GWBSDetail>(0);
                        billDtl.BalanceTaskDtlName = billDtl.BalanceTaskDtl.Name;
                    }
                    else
                    {
                        billDtl.BalanceTask = subBill.BalanceRange;
                        billDtl.BalanceTaskName = subBill.BalanceTaskName;
                        billDtl.BalanceTaskSyscode = subBill.BalanceTaskSyscode;
                    }
                    if (TransUtil.ToString(billDtl.BalanceTaskDtlName) == "")
                    {
                        billDtl.BalanceTaskDtlName = "建管费";
                    }
                    billDtl.FontBillType = FrontBillType.建管;
                    billDtl.BalacneQuantity = 1;
                    if (ht_basic.Contains("项"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["项"];
                        billDtl.QuantityUnitName = unit.Name;
                        billDtl.QuantityUnit = unit;
                    }
                    if (ht_basic.Contains("元"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["元"];
                        billDtl.PriceUnit = unit;
                        billDtl.PriceUnitName = unit.Name;
                    }
                    if (subProject.ManagementRemMethod == ManagementRememberMethod.按费率计取)
                    {
                        billDtl.BalancePrice = -(sumBalMoney * subProject.ManagementRate);
                        billDtl.BalanceTotalPrice = -(sumBalMoney * subProject.ManagementRate);
                    }
                    else if (subProject.ManagementRemMethod == ManagementRememberMethod.据实计取)
                    {
                        billDtl.BalancePrice = 0;
                        billDtl.BalanceTotalPrice = 0;
                    }
                    //6.2 构造代缴建设管理费的分包结算分科目信息
                    SubContractBalanceSubjectDtl balanceSubject = new SubContractBalanceSubjectDtl();
                    balanceSubject.BalanceQuantity = 1;
                    balanceSubject.BalancePrice = billDtl.BalancePrice;
                    balanceSubject.BalanceTotalPrice = billDtl.BalanceTotalPrice;
                    if (ht_basic.Contains("项"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["项"];
                        balanceSubject.QuantityUnitName = unit.Name;
                        balanceSubject.QuantityUnit = unit;
                    }
                    if (ht_basic.Contains("元"))
                    {
                        StandardUnit unit = (StandardUnit)ht_basic["元"];
                        balanceSubject.PriceUnit = unit;
                        balanceSubject.PriceUnitName = unit.Name;
                    }
                    if (ht_basic.Contains("C515"))//成本核算科目: 税金
                    {
                        balanceSubject.BalanceSubjectGUID = (CostAccountSubject)ht_basic["C515"];
                        balanceSubject.BalanceSubjectName = balanceSubject.BalanceSubjectGUID.Name;
                        balanceSubject.BalanceSubjectSyscode = balanceSubject.BalanceSubjectGUID.SysCode;
                        balanceSubject.BalanceSubjectCode = balanceSubject.BalanceSubjectGUID.Code;
                        balanceSubject.CostName = balanceSubject.BalanceSubjectName;
                    }

                    if (ht_basic.Contains("R30100000"))//物资资源:管理资源
                    {
                        Material material = (Material)ht_basic["R30100000"];
                        balanceSubject.ResourceTypeGUID = material.Id;
                        balanceSubject.ResourceTypeName = material.Name;
                        balanceSubject.ResourceTypeSpec = material.Specification;
                        balanceSubject.ResourceTypeStuff = material.Stuff;
                        balanceSubject.ResourceSyscode = material.TheSyscode;
                    }

                    balanceSubject.TheBalanceDetail = billDtl;
                    billDtl.Details.Add(balanceSubject);
                    billDtl.Master = subBill;
                    subBill.Details.Add(billDtl);
                }
                #endregion
            }

            //加载成本项（界面显示用）
            Disjunction disCostItem = new Disjunction();
            foreach (SubContractBalanceDetail billDtl in subBill.Details)
            {
                if (billDtl.BalanceTaskDtl != null && billDtl.BalanceTaskDtl.TheCostItem != null)
                {
                    disCostItem.Add(Expression.Eq("Id", billDtl.BalanceTaskDtl.TheCostItem.Id));
                }
            }
            if (disCostItem.ToString() != "()")
            {
                ObjectQuery oqCostItem = new ObjectQuery();
                oqCostItem.AddCriterion(disCostItem);

                var queryCostItem = dao.ObjectQuery(typeof(CostItem), oqCostItem).OfType<CostItem>();

                foreach (SubContractBalanceDetail billDtl in subBill.Details)
                {
                    if (billDtl.BalanceTaskDtl != null && billDtl.BalanceTaskDtl.TheCostItem != null)
                    {
                        var query = from c in queryCostItem
                                    where c.Id == billDtl.BalanceTaskDtl.TheCostItem.Id
                                    select c;

                        if (query.Count() > 0)
                            billDtl.BalanceTaskDtl.TheCostItem = query.ElementAt(0);
                    }
                }
            }
            //写入任务明细的计划工程量、计划金额、累计结算工程量、累计结算金额
            string dtlStr = "'1'";
            foreach (SubContractBalanceDetail billDtl in subBill.Details)
            {
                if (billDtl.BalanceTaskDtl != null && dtlStr.Contains(billDtl.BalanceTaskDtl.Id) == false)
                {
                    dtlStr += ",'" + billDtl.BalanceTaskDtl.Id + "'";
                }
            }
            
            Hashtable ht_taskMxInfo = this.GetSubBalMxPlanInfo(dtlStr, subBill.ProjectId);
            if (ht_taskMxInfo.Count > 0)
            {
                foreach (SubContractBalanceDetail billDtl in subBill.Details)
                {
                    if (billDtl.BalanceTaskDtl != null)
                    {
                        string taskMxID = billDtl.BalanceTaskDtl.Id;
                        if (ht_taskMxInfo.Contains(taskMxID))
                        {
                            DataDomain domain = (DataDomain)ht_taskMxInfo[taskMxID];
                            billDtl.PlanWorkAmount = TransUtil.ToDecimal(domain.Name2);
                            billDtl.PlanTotalprice = TransUtil.ToDecimal(domain.Name3);
                            billDtl.AddBalanceQuantity = TransUtil.ToDecimal(domain.Name4) + billDtl.BalacneQuantity;
                            billDtl.AddBalanceMoney = TransUtil.ToDecimal(domain.Name5) + billDtl.BalanceTotalPrice;
                        }
                    }
                }
            }
            //统计结算金额
            subBill.BalanceMoney = subBill.Details.OfType<SubContractBalanceDetail>().Sum(p => p.BalanceTotalPrice);
            subBill.Details.OfType<SubContractBalanceDetail>().OrderBy(dtl => dtl.BalanceBase);
           //ValidateBill(subBill);
            return subBill;
        }
//        工程任务核算 计划量<累计结算量  
//其他   计划金额<累计金额
        public void ValidateBill(SubContractBalanceBill subBill)
        {
            StringBuilder oBuilder = new StringBuilder();
            if (subBill != null)
            {
                foreach (SubContractBalanceDetail oDetail in subBill.Details)
                {
                    switch (oDetail.FontBillType)
                    {
                        case FrontBillType.工程任务核算:
                            {
                                if (oDetail.PlanWorkAmount < oDetail.AddBalanceQuantity)
                                {
                                    if (oBuilder.Length> 0)
                                    {
                                        oBuilder.Append("\n");
                                    }
                                    oBuilder.Append(string.Format("[{3}节点下:{0}]类型:[工程任务核算],[计划工程量({1})]小于[累计结算工程量({2})]", oDetail.BalanceTaskDtlName, oDetail.PlanWorkAmount,oDetail.AddBalanceQuantity,oDetail.BalanceTaskName));
                                }
                                break;
                            }
                        default:
                            {
                                if (oDetail.PlanTotalprice < oDetail.AddBalanceMoney)
                                {
                                    if (oBuilder.Length > 0)
                                    {
                                        oBuilder.Append("\n");
                                    }
                                    oBuilder.Append(string.Format("[{4}节点下:{0}]类型:[{3}],[计划金额({1})]小于[累计结算金额({2})]", oDetail.BalanceTaskDtlName, oDetail.PlanTotalprice, oDetail.AddBalanceMoney, Enum.GetName(typeof(FrontBillType), oDetail.FontBillType),oDetail.BalanceTaskName));
                                }
                                break;
                            }
                    }
                }
            }
            if (oBuilder.Length > 0)
            {
                throw new Exception("生成分包结算单失败:" + oBuilder.ToString());
            }
        }
        /// <summary>
        /// 查询分包结算明细对应的计划成本信息,累计结算信息
        /// </summary>
        /// <returns></returns>
        private Hashtable GetSubBalMxPlanInfo(string taskDtlStr, string projectID)
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select t1.id, t1.planworkamount,t1.plantotalprice From thd_gwbsdetail t1 where t1.id in ( " + taskDtlStr + " ) ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name1 = TransUtil.ToString(dataRow["id"]);
                    domain.Name2 = TransUtil.ToString(dataRow["planworkamount"]);
                    domain.Name3 = TransUtil.ToString(dataRow["plantotalprice"]);
                    if (!ht.Contains(TransUtil.ToString(domain.Name1)))
                    {
                        ht.Add(TransUtil.ToString(TransUtil.ToString(domain.Name1)), domain);
                    }
                }
            }
            command.CommandText = "select t2.balancetaskdtlguid,sum(t2.balacnequantity) sumBalQuantity,sum(t2.balancetotalprice) sumBalMoney "+
                                  " from thd_subcontractbalancebill t1,thd_subcontractbalancedetail t2 "+
                                  " where t1.id=t2.parentid and t1.state=5 and t1.projectid='" + projectID + "' and t2.balancetaskdtlguid in (" + taskDtlStr + ") " +
                                  " group by t2.balancetaskdtlguid ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string taskMxID = TransUtil.ToString(dataRow["balancetaskdtlguid"]);
                    if (ht.Contains(taskMxID))
                    {
                        DataDomain domain = (DataDomain)ht[taskMxID];
                        domain.Name4 = TransUtil.ToString(dataRow["sumBalQuantity"]);
                        domain.Name5 = TransUtil.ToString(dataRow["sumBalMoney"]);
                    }
                    else
                    {
                        DataDomain domain = new DataDomain();
                        domain.Name1 = taskMxID;
                        domain.Name4 = TransUtil.ToString(dataRow["sumBalQuantity"]);
                        domain.Name5 = TransUtil.ToString(dataRow["sumBalMoney"]);
                        ht.Add(TransUtil.ToString(TransUtil.ToString(domain.Name1)), domain);
                    }
                }
            }
            return ht;
        }

        [TransManager]
        public SubContractBalanceBill SaveSubContractBalBill(SubContractBalanceBill subBill)
        {
            bool ifNew = false;
            Hashtable ht_costsubjectflag = this.GetCostSubjectFlagList();
            //1 保存分包结算单
            foreach (SubContractBalanceDetail detail in subBill.Details)
            {
                //detail.BalancePrice = detail.BalancePrice;
                //detail.BalanceTotalPrice = detail.BalanceTotalPrice;
                foreach (SubContractBalanceSubjectDtl subject in detail.Details)
                {
                    if (detail.FontBillType == FrontBillType.水电 || detail.FontBillType == FrontBillType.建管)
                    {
                        subject.BalancePrice = detail.BalancePrice;
                        subject.BalanceTotalPrice = detail.BalanceTotalPrice;
                    }
                    else
                    {
                        subject.BalanceQuantity = detail.BalacneQuantity;
                        subject.BalanceTotalPrice = detail.BalacneQuantity * subject.BalancePrice;
                    }
                    //补充科目CODE
                    CostAccountSubject temp = (CostAccountSubject) ht_costsubjectflag[subject.BalanceSubjectGUID.Id];
                    if (temp != null)
                    {
                        subject.BalanceSubjectCode = temp.Code;
                    }
                }
            }
            if (subBill.Id == null)
            {
                subBill.Code = GetCode(typeof(SubContractBalanceBill), subBill.ProjectId);
                ifNew = true;
            }
            dao.SaveOrUpdate(subBill);

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = "";

            #region 回写结算明细对应的前驱单据
            //如果为修改,先清除之前状态
            if (ifNew == false)
            {
                //前驱工程任务核算单
                sql = "update thd_projecttaskdetailaccount t1 set t1.balancedtlguid='', t1.balancestate = 2 where t1.balancedtlguid in " +
                        " ( select k1.id from thd_subcontractbalancedetail k1 where k1.parentid='" + subBill.Id + "')";
                command.CommandText = sql;
                command.ExecuteNonQuery();
                //前驱罚扣款单
                sql = "update thd_penaltydeductiondetail t1 set t1.balancedtlguid='' where t1.balancedtlguid in " +
                        " ( select k1.id from thd_subcontractbalancedetail k1 where k1.parentid='" + subBill.Id + "')";
                command.CommandText = sql;
                command.ExecuteNonQuery();
                //前驱零星用工单
                sql = "update thd_laborsporadicdetail t1 set t1.balancedtlguid='' where t1.balancedtlguid in " +
                        " ( select k1.id from thd_subcontractbalancedetail k1 where k1.parentid='" + subBill.Id + "')";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }

            foreach (SubContractBalanceDetail detail in subBill.Details)
            {
                //2 回写前驱工程任务核算单
                if (detail.FontBillType == FrontBillType.工程任务核算)
                {
                    sql = " update THD_ProjectTaskDetailAccount t1 set t1.balancedtlguid='" + detail.Id + "',t1.balancestate = 3 where id = '" + detail.FrontBillGUID + "'";

                }//3 回写前驱罚扣款单
                else if (detail.FontBillType == FrontBillType.罚款单 || detail.FontBillType == FrontBillType.扣款单 || detail.FontBillType == FrontBillType.暂扣款单)
                {
                    sql = " update thd_penaltydeductiondetail t1 set t1.balancedtlguid='" + detail.Id + "' where id = '" + detail.FrontBillGUID + "'";

                } //4 回写前驱零星用工单
                else if (detail.FontBillType == FrontBillType.零星用工单 || detail.FontBillType == FrontBillType.代工单 
                    || detail.FontBillType == FrontBillType.计时派工 || detail.FontBillType == FrontBillType.分包签证)
                {
                    sql = " update thd_laborsporadicdetail t1 set t1.balancedtlguid='" + detail.Id + "' where id = '" + detail.FrontBillGUID + "'";

                }
                if (sql != "")
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
            #endregion

            //5 回写分包项目金额
            if (subBill.DocState == DocumentState.InAudit)
            {
                if (subBill.TheSubContractProject != null)
                {
                    sql = " update thd_subcontractproject t1 set t1.addupwaitapprovebalmoney = t1.addupwaitapprovebalmoney+" + subBill.BalanceMoney +
                            " where t1.id = '" + subBill.TheSubContractProject.Id + "'";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }

            return subBill;
        }

        /// <summary>
        /// 反写分包项目累积量[审批平台通过用]
        /// </summary>
        /// <param name="billId">结算单ID</param>
        [TransManager]
        public void UpdateSubContractProjectByAgree(string billId)
        {
            string subContractId = "";
            decimal balMoney = 0;

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(command);

            command.CommandText = " select t1.subcontractprojectid,t1.balancemoney from thd_subcontractbalancebill t1 where t1.id='" + billId + "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    subContractId = TransUtil.ToString(dataRow["subcontractprojectid"]);
                    balMoney = TransUtil.ToDecimal(dataRow["balancemoney"]);
                }
            }

            string sql = " update thd_subcontractproject t1 set t1.addupbalancemoney = t1.addupbalancemoney+" + balMoney +
                            "  ,t1.addupwaitapprovebalmoney = t1.addupwaitapprovebalmoney - " + balMoney +
                            " where t1.id = '" + subContractId + "'";

            command.CommandText = sql;
            command.ExecuteNonQuery();


            #region 2017-02-13 chenf
            //更新提报单 累计报量、本次提报量、 清除工程量提报单上的工程量确认单Id 
            sql = string.Format(@"update thd_gwbstaskconfirmdetail t1
   set t1.quantitybeforeconfirm  =
       (select t3.addbalancequantity - t3.balacnequantity
          from thd_projecttaskdetailaccount t2
          join thd_subcontractbalancedetail t3
            on t2.balancedtlguid = t3.id
          join thd_subcontractbalancebill t6
            on t3.parentid = t6.id
         where t6.id = '{0}'
           and t1.gwbsdetail = t2.projecttaskdtlguid
           and rownum = 1) +
       (select sum(t3.balacnequantity)
          from thd_projecttaskdetailaccount t2
          join thd_subcontractbalancedetail t3
            on t2.balancedtlguid = t3.id
          join thd_subcontractbalancebill t6
            on t3.parentid = t6.id
         where t6.id = '{0}'
           and t1.gwbsdetail = t2.projecttaskdtlguid),
       t1.actualcompletedquantity = 0,
       t1.accountingstate         = 0,
       t1.accounttime             = null
 where t1.accountingdetailid in
       (select t4.id
          from thd_projecttaskdetailaccount t4
          join thd_subcontractbalancedetail t5
            on t4.balancedtlguid = t5.id
          join thd_subcontractbalancebill t6
            on t5.parentid = t6.id
         where t6.id = '{0}'
           and t5.fontbilltype = 1)", billId);
            command.CommandText = sql;
            command.ExecuteNonQuery();

            //更新GWBSDetail 累计形象进度
            sql = string.Format(@"update thd_gwbsdetail t1
   set t1.addupaccquantity       = 
    (select t3.addbalancequantity - t3.balacnequantity
          from thd_projecttaskdetailaccount t2
          join thd_subcontractbalancedetail t3
            on t2.balancedtlguid = t3.id
          join thd_subcontractbalancebill t6
            on t3.parentid = t6.id
         where t6.id = '{0}'
           and t1.id = t2.projecttaskdtlguid
           and rownum = 1) 
           +
            (select sum(t3.balacnequantity)
          from thd_projecttaskdetailaccount t2
          join thd_subcontractbalancedetail t3
            on t2.balancedtlguid = t3.id
          join thd_subcontractbalancebill t6
            on t3.parentid = t6.id
         where t6.id = '{0}'
           and t1.id = t2.projecttaskdtlguid),
           
                                  
       t1.addupaccfigureprogress =
       ((select t3.addbalancequantity - t3.balacnequantity
          from thd_projecttaskdetailaccount t2
          join thd_subcontractbalancedetail t3
            on t2.balancedtlguid = t3.id
          join thd_subcontractbalancebill t6
            on t3.parentid = t6.id
         where t6.id = '{0}'
           and t1.id = t2.projecttaskdtlguid
           and rownum = 1) 
           +
            (select sum(t3.balacnequantity)
          from thd_projecttaskdetailaccount t2
          join thd_subcontractbalancedetail t3
            on t2.balancedtlguid = t3.id
          join thd_subcontractbalancebill t6
            on t3.parentid = t6.id
         where t6.id = '{0}'
           and t1.id = t2.projecttaskdtlguid)) / t1.planworkamount * 100
         where t1.id in (select t3.balancetaskdtlguid
                   from thd_subcontractbalancedetail t3
                   join thd_subcontractbalancebill t2
                     on t2.id = t3.parentid
                  where t2.id = '{0}')", billId);
            command.CommandText = sql;
            command.ExecuteNonQuery();

            //更新总体进度计划 形象进度、实际结束时间
            sql = string.Format(@"update thd_weekscheduledetail a
   set taskcompletedpercent = (case (select sum(d.planworkamount * d.Planprice)
                                  from thd_gwbsdetail d
                                 where d.parentid = a.gwbstree)
                                when 0 then
                                 100
                                else
                                 case
                                   when (((select sum(tt.detailaddbalancemoney)
                                        from (select (t3.addbalancemoney -
                                                     t3.balancetotalprice) as detailaddbalancemoney,
                                                     e.parentid as gwbsid,
                                                     e.id as detailid,
                                                     row_number() OVER(PARTITION BY e.id order by e.id) rn
                                                from thd_projecttaskdetailaccount t2
                                                join thd_subcontractbalancedetail t3
                                                  on t2.balancedtlguid = t3.id
                                                join thd_subcontractbalancebill t6
                                                  on t6.id = t3.parentid
                                                join thd_gwbsdetail e
                                                  on e.id = t2.projecttaskdtlguid
                                               where t6.id = '{0}') tt
                                       where tt.rn = 1
                                         and tt.gwbsid = a.gwbstree) +
                                        (select sum(t3.balancetotalprice)
                                             from thd_projecttaskdetailaccount t2
                                             join thd_subcontractbalancedetail t3
                                               on t2.balancedtlguid = t3.id
                                             join thd_subcontractbalancebill t6
                                               on t3.parentid = t6.id
                                             join thd_gwbsdetail d
                                               on d.id = t2.projecttaskdtlguid
                                            where t6.id = '{0}'
                                              and d.parentid = a.gwbstree)) * 100 /
                                        (select sum(d.planworkamount *
                                                     d.Planprice)
                                            from thd_gwbsdetail d
                                           where d.parentid = a.gwbstree)) > 100 then
                                    100
                                   else
                                    (((select sum(tt.detailaddbalancemoney)
                                        from (select (t3.addbalancemoney -
                                                     t3.balancetotalprice) as detailaddbalancemoney,
                                                     e.parentid as gwbsid,
                                                     e.id as detailid,
                                                     row_number() OVER(PARTITION BY e.id order by e.id) rn
                                                from thd_projecttaskdetailaccount t2
                                                join thd_subcontractbalancedetail t3
                                                  on t2.balancedtlguid = t3.id
                                                join thd_subcontractbalancebill t6
                                                  on t6.id = t3.parentid
                                                join thd_gwbsdetail e
                                                  on e.id = t2.projecttaskdtlguid
                                               where t6.id = '{0}') tt
                                       where tt.rn = 1
                                         and tt.gwbsid = a.gwbstree)
                                    
                                    + (select sum(t3.balancetotalprice)
                                          from thd_projecttaskdetailaccount t2
                                          join thd_subcontractbalancedetail t3
                                            on t2.balancedtlguid = t3.id
                                          join thd_subcontractbalancebill t6
                                            on t3.parentid = t6.id
                                          join thd_gwbsdetail c
                                            on c.id = t2.projecttaskdtlguid
                                         where t6.id = '{0}'
                                           and c.parentid = a.gwbstree)) * 100 /
                                    (select sum(d.planworkamount * d.Planprice)
                                       from thd_gwbsdetail d
                                      where d.parentid = a.gwbstree)) end
                              end), actualenddate = (SELECT max(b.realoperationdate)
                                                       FROM Thd_Gwbstaskconfirmdetail B
                                                      WHERE A.gwbstree =
                                                            B.gwbstree)

 WHERE a.parentid in (select id
                        from thd_weekschedulemaster w
                       where w.id = a.parentid
                         and w.execscheduletype = 40)
   and a.gwbstree IN (select c.parentid
                        FROM thd_subcontractbalancedetail td
                        join thd_subcontractbalancebill tm
                          on tm.id = td.parentid
                       inner join thd_gwbsdetail c
                          on td.balancetaskdtlguid = c.id
                       where tm.id = '{0}')", billId);
            command.CommandText = sql;
            command.ExecuteNonQuery();

            #endregion 
        }

        /// <summary>
        /// 反写分包项目累积量[审批平台不通过用]
        /// </summary>
        public void UpdateSubContractProjectByDisAgree(string billId)
        {
            string subContractId = "";
            decimal balMoney = 0;

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(command);

            command.CommandText = " select t1.subcontractprojectid,t1.balancemoney from thd_subcontractbalancebill t1 where t1.id='" + billId + "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    subContractId = TransUtil.ToString(dataRow["subcontractprojectid"]);
                    balMoney = TransUtil.ToDecimal(dataRow["balancemoney"]);
                }
            }

            string sql = " update thd_subcontractproject t1 set t1.addupwaitapprovebalmoney = t1.addupwaitapprovebalmoney - " + balMoney +
                            " where t1.id = '" + subContractId + "'";

            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        [TransManager]
        public void DeleteSubContractBalBill(SubContractBalanceBill subBill)
        {

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = "";

            foreach (SubContractBalanceDetail detail in subBill.Details)
            {
                //1 回写前驱工程任务核算单
                if (detail.FontBillType == FrontBillType.工程任务核算)
                {
                    sql = " update THD_ProjectTaskDetailAccount t1 set  t1.balancedtlguid='', t1.balancestate = 2  where t1.balancedtlguid='" + detail.Id + "'";

                }//2 回写前驱罚扣款单
                else if (detail.FontBillType == FrontBillType.罚款单 || detail.FontBillType == FrontBillType.扣款单)
                {
                    sql = " update thd_penaltydeductiondetail t1 set t1.balancedtlguid='' where t1.balancedtlguid='" + detail.Id + "'";
                } //3 回写前驱零星用工单
                else if (detail.FontBillType == FrontBillType.零星用工单 || detail.FontBillType == FrontBillType.代工单 
                    || detail.FontBillType == FrontBillType.计时派工 || detail.FontBillType == FrontBillType.分包签证)
                {
                    sql = " update thd_laborsporadicdetail t1 set t1.balancedtlguid='' where t1.balancedtlguid='" + detail.Id + "'";
                }
                if (sql != "")
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }

            //4 回写分包项目金额
            //sql = " update thd_subcontractproject t1 set t1.addupwaitapprovebalmoney = t1.addupwaitapprovebalmoney -" + subBill.BalanceMoney;
            //command.CommandText = sql;
            //command.ExecuteNonQuery();

            //5 删除分包结算单
            subBill = dao.Get(typeof(SubContractBalanceBill), subBill.Id) as SubContractBalanceBill;
            dao.Delete(subBill);
        }

        /// <summary>
        /// 查询成本核算科目标志
        /// </summary>
        /// <returns></returns>
        private Hashtable GetCostSubjectFlagList()
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select t1.id,t1.code,t1.syscode,t1.name,t1.IfSubBalanceFlag from THD_CostAccountSubject t1 ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CostAccountSubject subject = new CostAccountSubject();
                    subject.Id = TransUtil.ToString(dataRow["id"]);
                    subject.Code = TransUtil.ToString(dataRow["code"]);
                    subject.SysCode = TransUtil.ToString(dataRow["syscode"]);
                    subject.Name = TransUtil.ToString(dataRow["name"]);
                    subject.IfSubBalanceFlag = TransUtil.ToInt(dataRow["IfSubBalanceFlag"]);
                    ht.Add(subject.Id, subject);
                }
            }
            return ht;
        }

        /// <summary>
        /// 查询成本核算科目名称集合
        /// </summary>
        /// <returns></returns>
        public Hashtable GetCostSubjectNameList()
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select t1.id,t1.name from THD_CostAccountSubject t1 ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string temp = TransUtil.ToString(dataRow["name"]);
                    if (!ht.Contains(temp))
                    {
                        ht.Add(temp, TransUtil.ToString(dataRow["id"]));
                    }
                }
            }
            return ht;
        }

        /// <summary>
        /// 查询分包结算的基础数据
        /// </summary>
        /// <returns></returns>
        private Hashtable GetSubBalBasicData(string balGWBSSyscode)
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = "select t1.id treeid,t1.name treename,t1.syscode,t3.quotacode,t2.id,t2.name from thd_gwbstree t1,thd_gwbsdetail t2,thd_costitem t3 " +
                " where t1.id=t2.parentid and t2.costitemguid=t3.id and t3.quotacode in ('K4-8','K4-7','BC15-5') and t1.syscode like '" + balGWBSSyscode + "%'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    IList list = new ArrayList();
                    GWBSTree tree = new GWBSTree();
                    tree.Id = TransUtil.ToString(dataRow["treeid"]);
                    tree.Version = 1;
                    tree.Name = TransUtil.ToString(dataRow["treename"]);
                    tree.SysCode = TransUtil.ToString(dataRow["syscode"]);
                    GWBSDetail detail = new GWBSDetail();
                    detail.Id = TransUtil.ToString(dataRow["id"]);
                    detail.Name = TransUtil.ToString(dataRow["name"]);
                    detail.TheGWBS = tree;
                    detail.Version = 1;
                    tree.Details.Add(detail);
                    if (!ht.Contains(dataRow["quotacode"]))
                    {
                        ht.Add(TransUtil.ToString(dataRow["quotacode"]), tree);
                    }
                }
            }
            command.CommandText = "select t1.code,t1.id,t1.name,t1.syscode From thd_costaccountsubject t1 where t1.code in ('C5110306','C515','C5110125') ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CostAccountSubject model = new CostAccountSubject();
                    model.Id = TransUtil.ToString(dataRow["id"]);
                    model.Name = TransUtil.ToString(dataRow["name"]);
                    model.SysCode = TransUtil.ToString(dataRow["syscode"]);
                    if (!ht.Contains(dataRow["code"]))
                    {
                        ht.Add(TransUtil.ToString(dataRow["code"]), model);
                    }
                }
            }
            command.CommandText = "select t1.materialid,t1.matcode,t1.matname,t1.stuff,t1.matspecification,t1.thesyscode " +
                    " from resmaterial t1 where t1.matcode in ('I19800000','R30100000')";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    Material model = new Material();
                    model.Id = TransUtil.ToString(dataRow["materialid"]);
                    model.Name = TransUtil.ToString(dataRow["matname"]);
                    model.TheSyscode = TransUtil.ToString(dataRow["thesyscode"]);
                    model.Specification = TransUtil.ToString(dataRow["matspecification"]);
                    model.Stuff = TransUtil.ToString(dataRow["stuff"]);
                    if (!ht.Contains(TransUtil.ToString(dataRow["matcode"])))
                    {
                        ht.Add(TransUtil.ToString(dataRow["matcode"]), model);
                    }
                }
            }
            command.CommandText = " select t1.standunitid,t1.standunitname from resstandunit t1 where t1.standunitname in ('项','元')";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    StandardUnit unit = new StandardUnit();
                    unit.Version = 1;
                    unit.Code = "";
                    unit.Id = TransUtil.ToString(dataRow["standunitid"]);
                    unit.Name = TransUtil.ToString(dataRow["standunitname"]);
                    if (!ht.Contains(TransUtil.ToString(dataRow["standunitname"])))
                    {
                        ht.Add(TransUtil.ToString(dataRow["standunitname"]), unit);
                    }
                }
            }
            return ht;
        }

        /// <summary>
        /// 计算分包措施费任务明细的结算明细数据
        /// </summary>
        /// <param name="listMeansureDtl">分包措施费任务明细集</param>
        /// <returns></returns>
        public List<SubContractBalanceDetail> BalanceSubContractFeeDtl(SubContractProject subProject, List<GWBSDetail> listMeansureDtl, List<SubContractBalanceDetail> listCurrBalDtl)
        {
            List<SubContractBalanceDetail> listResult = new List<SubContractBalanceDetail>();

            //加载要结算的措施费明细下的耗用
            ObjectQuery oq = new ObjectQuery();
            Disjunction disDtl = new Disjunction();
            foreach (GWBSDetail dtl in listMeansureDtl)
            {
                disDtl.Add(Expression.Eq("Id", dtl.Id));
            }
            oq.AddCriterion(disDtl);
            oq.AddFetchMode("ListCostSubjectDetails", FetchMode.Eager);

            listMeansureDtl = dao.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>().ToList();

            oq.Criterions.Clear();

            foreach (GWBSDetail dtl in listMeansureDtl)//根据措施费明细生成结算明细
            {
                oq.Criterions.Clear();

                //1.过滤纳入分包取费范围内的分包结算明细
                oq.AddCriterion(Expression.Like("BalanceTaskSyscode", dtl.TheGWBSSysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("FontBillType", FrontBillType.工程任务核算));

                #region 按成本项分类过滤（各个分类之间OR关系）

                //如果未设置分类过滤条件则查询所有
                Disjunction dis1 = new Disjunction();
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode1))
                {
                    dis1.Add(Expression.Like("BalanceTaskDtl.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode1, MatchMode.Start));
                }
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode2))
                {
                    dis1.Add(Expression.Like("BalanceTaskDtl.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode2, MatchMode.Start));
                }
                string term = dis1.ToString();
                if (term != "()")//不加条件时为()
                    oq.AddCriterion(dis1);

                #endregion

                oq.AddFetchMode("Details", FetchMode.Eager);
                List<SubContractBalanceDetail> listBalanceDtl = dao.ObjectQuery(typeof(SubContractBalanceDetail), oq).OfType<SubContractBalanceDetail>().ToList();

                if (listCurrBalDtl != null && listCurrBalDtl.Count > 0)
                    listBalanceDtl.AddRange(listCurrBalDtl);

                if (listBalanceDtl.Count() > 0)//过滤核算科目
                {
                    List<SubContractBalanceSubjectDtl> listMeansureFeeSubDtl = new List<SubContractBalanceSubjectDtl>();

                    foreach (SubContractBalanceDetail balDtl in listBalanceDtl)
                    {
                        if (dtl.TheCostItem.SubjectCateFilter1 != null || dtl.TheCostItem.SubjectCateFilter2 != null || dtl.TheCostItem.SubjectCateFilter3 != null)
                        {
                            if (dtl.TheCostItem.SubjectCateFilter1 != null)
                            {
                                listMeansureFeeSubDtl.AddRange(from d in balDtl.Details
                                                               where d.BalanceSubjectGUID.Id == dtl.TheCostItem.SubjectCateFilter1.Id
                                                               select d);
                            }
                            if (dtl.TheCostItem.SubjectCateFilter2 != null)
                            {
                                listMeansureFeeSubDtl.AddRange(from d in balDtl.Details
                                                               where d.BalanceSubjectGUID.Id == dtl.TheCostItem.SubjectCateFilter2.Id
                                                               select d);
                            }
                            if (dtl.TheCostItem.SubjectCateFilter3 != null)
                            {
                                listMeansureFeeSubDtl.AddRange(from d in balDtl.Details
                                                               where d.BalanceSubjectGUID.Id == dtl.TheCostItem.SubjectCateFilter3.Id
                                                               select d);
                            }
                        }
                        else
                        {
                            listMeansureFeeSubDtl.AddRange(balDtl.Details);
                        }
                    }

                    SubContractBalanceDetail newBalDtl = new SubContractBalanceDetail();
                    newBalDtl.FontBillType = FrontBillType.措施;
                    newBalDtl.ForwardDetailId = dtl.Id;
                    newBalDtl.FrontBillGUID = dtl.Id;
                    newBalDtl.BalanceTask = dtl.TheGWBS;
                    newBalDtl.BalanceTaskName = dtl.TheGWBS.Name;
                    newBalDtl.BalanceTaskSyscode = dtl.TheGWBSSysCode;
                    newBalDtl.BalanceTaskDtl = dtl;
                    newBalDtl.BalanceTaskDtlName = dtl.Name;
                    newBalDtl.BalacneQuantity = 1;
                    newBalDtl.BalancePrice = listMeansureFeeSubDtl.Sum(d => d.BalanceTotalPrice) * dtl.SubContractStepRate;
                    newBalDtl.BalanceTotalPrice = newBalDtl.BalacneQuantity * newBalDtl.BalancePrice;
                    //扣税
                    if (subProject.LaborMoneyType == ManagementRememberMethod.按费率计取)
                    {
                        newBalDtl.BalancePrice = newBalDtl.BalancePrice - (newBalDtl.BalancePrice * subProject.LaobrRace);
                        newBalDtl.BalanceTotalPrice = newBalDtl.BalanceTotalPrice - (newBalDtl.BalanceTotalPrice * subProject.LaobrRace);
                    }
                    newBalDtl.QuantityUnit = dtl.WorkAmountUnitGUID;
                    newBalDtl.QuantityUnitName = dtl.WorkAmountUnitName;
                    newBalDtl.PriceUnit = dtl.PriceUnitGUID;
                    newBalDtl.PriceUnitName = dtl.PriceUnitName;

                    foreach (GWBSDetailCostSubject dtlUsage in dtl.ListCostSubjectDetails)
                    {
                        SubContractBalanceSubjectDtl newBalSubDtl = new SubContractBalanceSubjectDtl();
                        newBalSubDtl.BalanceSubjectGUID = dtlUsage.CostAccountSubjectGUID;
                        newBalSubDtl.BalanceSubjectName = dtlUsage.CostAccountSubjectName;
                        newBalSubDtl.BalanceSubjectSyscode = dtlUsage.CostAccountSubjectSyscode;

                        newBalSubDtl.ResourceTypeGUID = dtlUsage.ResourceTypeGUID;
                        newBalSubDtl.ResourceTypeName = dtlUsage.ResourceTypeName;
                        newBalSubDtl.ResourceTypeSpec = dtlUsage.ResourceTypeSpec;
                        newBalSubDtl.ResourceTypeStuff = dtlUsage.ResourceTypeQuality;
                        newBalSubDtl.ResourceSyscode = dtlUsage.ResourceCateSyscode;
                        newBalSubDtl.DiagramNumber = dtlUsage.DiagramNumber;
                        newBalSubDtl.BalanceQuantity = 1;
                        newBalSubDtl.BalancePrice = newBalDtl.BalancePrice * dtlUsage.AssessmentRate;
                        newBalSubDtl.BalanceTotalPrice = newBalSubDtl.BalanceQuantity * newBalSubDtl.BalancePrice;
                        newBalSubDtl.CostName = dtlUsage.CostAccountSubjectName;
                        newBalSubDtl.FrontBillGUID = dtlUsage.Id;
                        newBalSubDtl.QuantityUnit = dtlUsage.ProjectAmountUnitGUID;
                        newBalSubDtl.QuantityUnitName = dtlUsage.ProjectAmountUnitName;
                        newBalSubDtl.PriceUnit = dtlUsage.PriceUnitGUID;
                        newBalSubDtl.PriceUnitName = dtlUsage.PriceUnitName;
                        newBalSubDtl.MonthBalanceFlag = MonthBalanceSuccessFlag.未结算;

                        newBalSubDtl.TheBalanceDetail = newBalDtl;
                        newBalDtl.Details.Add(newBalSubDtl);
                    }

                    listResult.Add(newBalDtl);
                }
            }

            return listResult;
        }
        #endregion

        /// <summary>
        /// 分包结算信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet SubContractBalanceQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select t3.id dtlsubjectid,t1.id, t1.code, t3.balancesubjectcode,t1.monthaccbillid,t1.subcontractunitname, t1.balancemoney, t1.balancetaskname, t1.createpersonname, t1.createdate, t1.begintime, 
                        t1.endtime, t1.state, t2.balancetaskname dtltaskname,t2.balancetaskdtlname, t3.balancetotalprice, t3.balanceprice, t3.balancequantity,
                        t2.fontbilltype, t2.quantityunitname, t2.priceunitname,t2.remarks, t2.usedescript,t2.HandlePersonName, t3.costname, t3.resourcetypename, t3.resourcetypespec,
                        t3.diagramnumber, t3.balancesubjectname,t2.balancebase
                        from thd_subcontractbalancebill t1 inner join thd_subcontractbalancedetail t2 on t1.id = t2.parentid left join 
                        thd_subcontractbalsubjectdtl t3 on t2.id = t3.parentid";
            sql += " where 1=1 " + condition;
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 得到人员Code
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet GetPerCode(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select t2.auditperson,t2.rolename, t3.percode from thd_subcontractbalancebill t1 join thd_appstepsinfo t2 on t1.id = t2.billid join resperson t3 on t3.perid = t2.auditperson where t2.billid = '" + condition +
                         "' union all select t4.createpersonname, t5.pername,t5.percode from thd_subcontractbalancebill t4 join resperson t5 on t4.createperson = t5.perid where t4.id = '" + condition + "'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        public void CreateParameter(IDbCommand oCommand, string sParameterName, object oValue, DbType type)
        {

            IDbDataParameter oParameter = oCommand.CreateParameter();
            oParameter.ParameterName = sParameterName;
            if (oValue == null)
            {
                oParameter.Value = DBNull.Value;
            }
            else
            {
                oParameter.Value = oValue;
            }
            oParameter.DbType = type;
            oCommand.Parameters.Add(oParameter);
        }
        /// <summary>
        /// 调整单据的核算科目信息
        /// </summary>
        /// <param name="billType">1:分包结算,2:物资消耗</param>
        [TransManager]
        public void UpdateBillSubjectInfo(IList dataList,List<DataDomain> lstMaterial, int billType)
        {
            Hashtable subject_ht = this.GetCostSubjectFlagList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = "";
            if (billType == 1)
            {
                foreach (DataDomain domain in dataList)
                {
                    string dtlsubjectid = domain.Name1.ToString();
                    string subjectid = domain.Name2.ToString();
                    if (subject_ht.Contains(subjectid))
                    {
                        CostAccountSubject subject = (CostAccountSubject)subject_ht[subjectid];
                        sql = " update thd_subcontractbalsubjectdtl t1 set t1.balancesubjectguid='" + subject.Id + "',t1.balancesubjectname='" + subject .Name+ "', " +
                                " t1.balancesubjectcode='" + subject.Code + "',t1.balancesubjectsyscode='" + subject.SysCode+ "' where t1.id = '" + dtlsubjectid + "'";
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
                if (lstMaterial != null && lstMaterial.Count > 0)
                {
                    IDbDataParameter oParameter =null;
                    string sSQL = @"update thd_subcontractbalsubjectdtl t set 
t.resourcetypeguid=:resourcetypeguid,
t.resourcetypename=:resourcetypename,
t.resourcetypestuff=:resourcetypestuff,
t.resourcetypespec=:resourcetypespec,
t.resourcesyscode=:resourcesyscode 
where t.id=:id";
                    foreach (DataDomain domain in lstMaterial)
                    {
                        
                         //Name1 = oRow .Tag, Name2 = oMaterial.Id,Name3=oMaterial.Name, Name4=oMaterial.Stuff,Name5=oMaterial.TheSyscode
                        command.CommandText = sSQL;
                        command.Parameters.Clear();
                      
                       
                        CreateParameter(command,"id", ClientUtil.ToString(domain.Name1),DbType.String);
                        CreateParameter(command,"resourcetypeguid", ClientUtil.ToString(domain.Name2),DbType.String);
                       CreateParameter(command,"resourcetypename", ClientUtil.ToString(domain.Name3),DbType.String);
                        CreateParameter(command,"resourcetypestuff", ClientUtil.ToString(domain.Name4),DbType.String);
                       CreateParameter(command,"resourcetypespec", ClientUtil.ToString(domain.Name5),DbType.String);
                       CreateParameter(command, "resourcesyscode", ClientUtil.ToString(domain.Name6), DbType.String);
                        command.ExecuteNonQuery();
                    }
                }
            }
            if (billType == 2)
            {
                foreach (DataDomain domain in dataList)
                {
                    string dtlsubjectid = domain.Name1.ToString();
                    string subjectid = domain.Name2.ToString();
                    if (subject_ht.Contains(subjectid))
                    {
                        CostAccountSubject subject = (CostAccountSubject)subject_ht[subjectid];
                        sql = " update thd_stkstockoutdtl t1 set t1.subjectguid='" + subject.Id + "',t1.subjectname='" + subject.Name + "', " +
                                " t1.subjectsyscode='" + subject.SysCode + "' where t1.id = '" + dtlsubjectid + "'";
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private SubContractBalanceBill BuildGenerateBillArgs(SubContractBalanceBill srcBill,SubContractProject subProj)
        {
            var subBillArg = new SubContractBalanceBill();
            subBillArg.CreateDate = srcBill.CreateDate;
            subBillArg.CreatePerson = srcBill.CreatePerson;
            subBillArg.CreatePersonName = srcBill.CreatePersonName;
            subBillArg.OpgSysCode = srcBill.OpgSysCode;
            subBillArg.CreateYear = srcBill.CreateYear;
            subBillArg.CreateMonth = srcBill.CreateMonth;

            subBillArg.OperOrgInfo = srcBill.OperOrgInfo;
            subBillArg.OperOrgInfoName = srcBill.OperOrgInfoName;
            subBillArg.OpgSysCode = srcBill.OpgSysCode;
            subBillArg.HandlePersonName = srcBill.HandlePersonName;
            subBillArg.ProjectId = srcBill.ProjectId;
            subBillArg.ProjectName = srcBill.ProjectName;
            subBillArg.BalanceRange = srcBill.BalanceRange;
            subBillArg.BalanceTaskName = srcBill.BalanceTaskName;
            subBillArg.BalanceTaskSyscode = srcBill.BalanceTaskSyscode;
            subBillArg.BeginTime = srcBill.BeginTime;
            subBillArg.EndTime = srcBill.EndTime;
            subBillArg.Descript = srcBill.Descript;

            subBillArg.TheSubContractProject = subProj;
            subBillArg.SubContractUnitGUID = subProj.BearerOrg;
            subBillArg.SubContractUnitName = subProj.BearerOrgName;
            subBillArg.CumulativeMoney = subProj.AddupBalanceMoney;
            subBillArg.SubmitDate = GetServerTime();

            return subBillArg; 
        }

        private List<SubContractProject> GetCanSettlementSubContractProject(SubContractBalanceBill subBill)
        {
            List<SubContractProject> subProjects = new List<SubContractProject>();

            //取工程核算单的分包项目
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateDate", subBill.BeginTime));
            oq.AddCriterion(Expression.Le("CreateDate", subBill.EndTime));
            oq.AddCriterion(Expression.Eq("ProjectId", subBill.ProjectId));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddFetchMode("Details", FetchMode.Eager);
            var list_PTaskAccount =
                dao.ObjectQuery(typeof(ProjectTaskAccountBill), oq).OfType<ProjectTaskAccountBill>().ToList();
            if (list_PTaskAccount != null)
            {
                foreach (var pBill in list_PTaskAccount)
                {
                    var subs = from d in pBill.Details.OfType<ProjectTaskDetailAccount>().ToList()
                               where string.IsNullOrEmpty(d.BalanceDtlGUID)
                               select d.BearerGUID;
                    if (subs.Any())
                    {
                        subProjects.AddRange(subs.ToList());
                    }
                }
            }

            //取罚款单可以结算的分包项目
            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Ge("CreateDate", subBill.BeginTime));
            oq.AddCriterion(Expression.Le("CreateDate", subBill.EndTime));
            oq.AddCriterion(Expression.Eq("ProjectId", subBill.ProjectId));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            var list_PenaltyDeduction =
                dao.ObjectQuery(typeof (PenaltyDeductionMaster), oq).OfType<PenaltyDeductionMaster>().ToList();
            if (list_PenaltyDeduction != null)
            {
                var q =
                    from p in list_PenaltyDeduction
                    where
                        p.Details.OfType<PenaltyDeductionDetail>().ToList().Exists(
                            d => string.IsNullOrEmpty(d.BalanceDtlGUID))
                    select p.PenaltyDeductionRant;

                if (q.Any())
                {
                    subProjects.AddRange(q.ToList());
                }
            }

            //取零星用工单可以结算的分包项目
            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Ge("CreateDate", subBill.BeginTime));
            oq.AddCriterion(Expression.Le("CreateDate", subBill.EndTime));
            oq.AddCriterion(Expression.Eq("ProjectId", subBill.ProjectId));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            var list_LaborSporadic =
                dao.ObjectQuery(typeof (LaborSporadicMaster), oq).OfType<LaborSporadicMaster>().ToList();
            if (list_LaborSporadic != null)
            {
                var q =
                    from p in list_LaborSporadic
                    where
                        p.Details.OfType<LaborSporadicDetail>().ToList().Exists(
                            d => string.IsNullOrEmpty(d.BalanceDtlGUID))
                    select p.BearTeam;

                if (q.Any())
                {
                    subProjects.AddRange(q.ToList());
                }
            }

            return subProjects.Distinct().ToList();
        }

        [TransManager]
        public IList GenerateProjectSubContractBalance(SubContractBalanceBill subBill, CurrentProjectInfo projectInfo)
        {
            if (subBill == null || projectInfo == null)
            {
                return null;
            }

            var curBillMaster = new ProjectTaskAccountBill();
            curBillMaster.CreateDate = subBill.CreateDate;
            curBillMaster.DocState = DocumentState.Edit;
            curBillMaster.AccountPersonGUID = subBill.CreatePerson;
            curBillMaster.AccountPersonName = subBill.CreatePersonName;
            curBillMaster.OperOrgInfo = subBill.OperOrgInfo;
            curBillMaster.OperOrgInfoName = subBill.OperOrgInfoName;
            curBillMaster.OpgSysCode = subBill.OpgSysCode;
            curBillMaster.ProjectId = projectInfo.Id;
            curBillMaster.ProjectName = projectInfo.Name;
            curBillMaster.AccountRange = subBill.BalanceRange;
            curBillMaster.AccountTaskSyscode = subBill.BalanceTaskSyscode;
            curBillMaster.AccountTaskName = subBill.BalanceTaskName;
            curBillMaster.BeginTime = subBill.BeginTime;
            curBillMaster.EndTime = subBill.EndTime;
            if (subBill.TheSubContractProject != null)
            {
                curBillMaster.SubContractProjectID = subBill.TheSubContractProject.Id;
            }
            var batchNo = string.Format("{0:yyyyMMddHHmmss}", GetServerTime());

            #region 工程任务核算单生产经理确认生成，取消自动生成
            //1、生成工程任务核算单
            //var list = ProjectTaskAccountSrv.GenAccountBillByVirConfirmBill(curBillMaster);
            //var confirmTasks = list[1] as List<GWBSTaskConfirm>;
            //var taskAccountBill = list[0] as ProjectTaskAccountBill;
            List<SubContractProject> subProjects = new List<SubContractProject>();
            //if (taskAccountBill != null && confirmTasks != null && confirmTasks.Count > 0)
            //{
            //    //分包结算单生成日期源自UI可改，不能作为工程任务核算单的生成日期，否则可能导致该核算单不在选取的范围内。
            //    taskAccountBill.CreateDate = DateTime.Now;
            //    taskAccountBill.AuditDate = DateTime.Now;
            //    taskAccountBill.AuditPerson = taskAccountBill.CreatePerson;
            //    taskAccountBill.AuditPersonName = taskAccountBill.CreatePersonName;
            //    taskAccountBill.DocState = DocumentState.InExecute;
            //    taskAccountBill.CreateBatchNo = batchNo;
            //    taskAccountBill.Descript = "批量生成";
            //    taskAccountBill.SubmitDate = GetServerTime();
            //    taskAccountBill.CreateYear = taskAccountBill.SubmitDate.Year;
            //    taskAccountBill.CreateMonth = taskAccountBill.SubmitDate.Month;
            //    ProjectTaskAccountSrv.SaveAccBillAndSetCfmStateByVirCfmBill(taskAccountBill, confirmTasks);

            //    foreach (var cfmTask in confirmTasks)
            //    {
            //        if (!subProjects.Contains(cfmTask.TaskHandler))
            //        {
            //            subProjects.Add(cfmTask.TaskHandler);
            //        }
            //    }
            //}
            #endregion

            if (subBill.TheSubContractProject == null)
            {
                subProjects.AddRange(GetCanSettlementSubContractProject(subBill));
            }
            else
            {
                subProjects.Add(subBill.TheSubContractProject);
            }
            subProjects = subProjects.Distinct().ToList();
            var bananceList = new List<SubContractBalanceBill>();
            //按分包商生成结算单
            foreach (SubContractProject subProj in subProjects)
            {
                var newSubBill = GenSubBalanceBill(BuildGenerateBillArgs(subBill, subProj), subProj, projectInfo);
                if (newSubBill.Details == null || newSubBill.Details.Count == 0)
                {
                    continue;
                }
                newSubBill.DocState = DocumentState.Edit;
                newSubBill.CreateBatchNo = batchNo;
                newSubBill.Descript = "批量生成";
                newSubBill = SaveSubContractBalBill(newSubBill);

                bananceList.Add(newSubBill);
            }

            return bananceList;
        }

        [TransManager]
        public bool DeleteProjectSubContractBalance(SubContractBalanceBill bill)
        {
            if (bill == null || string.IsNullOrEmpty(bill.CreateBatchNo))
            {
                return false;
            }

            #region 工程任务核算单生产经理确认，删除结算单不回滚报量、不删除核算单
            /*
            Disjunction disAccountBillDetail = new Disjunction();
            foreach (SubContractBalanceDetail detail in bill.Details)
            {
                if(detail.FontBillType == FrontBillType.工程任务核算)
                {
                    disAccountBillDetail.Add(Expression.Eq("Id", detail.FrontBillGUID));
                }
            }

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(disAccountBillDetail);
            var taskList = Dao.ObjectQuery(typeof(ProjectTaskDetailAccount), objectQuery);
            if (taskList != null && taskList.Count > 0)
            {
                ProjectTaskAccountSrv.RollbackAccTaskDtlQnyAndProgress(taskList);

                var accBillId = taskList.OfType<ProjectTaskDetailAccount>().First().TheAccountBill.Id;
                objectQuery.Criterions.Clear();
                objectQuery.AddCriterion(Expression.Eq("Id", accBillId));
                objectQuery.AddFetchMode("Details", FetchMode.Eager);
                var accBillList = Dao.ObjectQuery(typeof (ProjectTaskAccountBill), objectQuery);
                if (accBillList != null && accBillList.Count == 1)
                {
                    var accBill = accBillList.OfType<ProjectTaskAccountBill>().First();
                    if (accBill.Details.Count == 0)
                    {
                        ProjectTaskAccountSrv.DeleteProjectTaskAccount(accBillList);
                    }
                }
            }
            */
            #endregion

            DeleteSubContractBalBill(bill);

            return true;
        }

        #region 自动将预算体系上科目与资源与分包结算单耗用进行同步
        public DataDomain GetAutoSynCount(string sProjectId)
        {
            int iCount = 0, iResouceCount = 0, iSubjectCount = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AutoSynSubSubjectCost.query";

            IDbDataParameter Parameter = command.CreateParameter(); Parameter.ParameterName = "sProjectId"; Parameter.DbType = DbType.String; Parameter.Value = sProjectId; command.Parameters.Add(Parameter);

            IDbDataParameter Parameter1 = command.CreateParameter(); Parameter1.ParameterName = "iCount"; Parameter1.DbType = DbType.Int32; Parameter1.Direction = ParameterDirection.Output; command.Parameters.Add(Parameter1);
            IDbDataParameter Parameter2 = command.CreateParameter(); Parameter2.ParameterName = "iResouceCount"; Parameter2.DbType = DbType.Int32; Parameter2.Direction = ParameterDirection.Output; command.Parameters.Add(Parameter2);
            IDbDataParameter Parameter3 = command.CreateParameter(); Parameter3.ParameterName = "iSubjectCount"; Parameter3.DbType = DbType.Int32; Parameter3.Direction = ParameterDirection.Output; command.Parameters.Add(Parameter3);
            IDataReader dataReader = command.ExecuteReader();
            //DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataDomain data = new DataDomain();
            data.Name1 = Parameter1.Value;
            data.Name2 = Parameter2.Value;
            data.Name3 = Parameter3.Value;
            return data;
        }
        public int  ExeAutoSyn(string sProjectId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AutoSynSubSubjectCost.execute";
            IDbDataParameter Parameter;
            Parameter = command.CreateParameter(); Parameter.ParameterName = "sProjectId"; Parameter.DbType = DbType.String; Parameter.Value = sProjectId; command.Parameters.Add(Parameter);
           return  command.ExecuteNonQuery();
        }
        #endregion

        private Material GetElectResourceMaterial(string strCode)
        {
            var oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", strCode));
            IList list_Material = Dao.ObjectQuery(typeof(Material), oq);
            if (list_Material == null)
            {
                return list_Material[0] as Material;
            }
            return null;
        }
    }
}