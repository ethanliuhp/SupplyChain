﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;
using System.Data.SqlClient;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Util;
using System.Data.OracleClient;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Service
{
    /// <summary>
    /// 工程任务核算单服务
    /// </summary>
    public class ProjectTaskAccountSrv : IProjectTaskAccountSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
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
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns></returns>
        public string GetProjectTaskAccountCode()
        {
            return "QY-" + string.Format("{0:yyyyMMdd}", DateTime.Now);
        }

        /// <summary>
        /// 获取工程任务核算单明细编号
        /// </summary>
        /// <returns></returns>
        public string GetProjectTaskAccountDetailCode(string contractGroupCode, int detailNum)
        {
            return contractGroupCode + detailNum.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// 保存或修改工程任务核算单集合
        /// </summary>
        /// <param name="list">工程任务核算单集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateProjectTaskAccount(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }
        [TransManager]
        public IList SaveOrUpdate(IList lst)
        {
            dao.SaveOrUpdate(lst);
            return lst;
        }
        /// <summary>
        /// 保存或修改工程任务核算单
        /// </summary>
        /// <param name="bill">工程任务核算单</param>
        /// <returns></returns>
        [TransManager]
        public ProjectTaskAccountBill SaveOrUpdateProjectTaskAccount(ProjectTaskAccountBill bill)
        {
            if (bill.Id == null)
            {
                bill.Code = GetCode(typeof(ProjectTaskAccountBill), bill.ProjectId);
                bill.RealOperationDate = DateTime.Now;
                bill.SubmitDate = DateTime.Now;
            }
            if (bill.DocState == DocumentState.InAudit || bill.DocState == DocumentState.InExecute)
            {
                bill.SubmitDate = DateTime.Now;
            }
            dao.SaveOrUpdate(bill);

            if (bill.DocState == DocumentState.InExecute)
            {
                WriteBackAccTaskDtlQnyAndProgress(bill);
            }

            updateWeekScheduleDetail(bill);
            return bill;
        }

        /// <summary>
        /// 删除工程任务核算单集合
        /// </summary>
        /// <param name="list">工程任务核算单集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteProjectTaskAccount(IList list)
        {
            //回写确认单明细的核算信息
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();

            for (int i = 0; i < list.Count; i++)
            {
                ProjectTaskAccountBill cg = list[i] as ProjectTaskAccountBill;
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    ObjectQuery oqChild = new ObjectQuery();
                    oqChild.AddCriterion(Expression.Eq("Id", cg.Id));
                    oqChild.AddFetchMode("Details", FetchMode.Eager);
                    IList listBillTemp = dao.ObjectQuery(typeof(ProjectTaskAccountBill), oqChild);
                    if (listBillTemp.Count > 0)
                    {
                        cg = listBillTemp[0] as ProjectTaskAccountBill;
                        list[i] = cg;

                        foreach (ProjectTaskDetailAccount dtl in cg.Details)
                        {
                            dis.Add(Expression.Eq("AccountingDetailId", dtl.Id));
                        }

                    }
                }
            }
            oq.AddCriterion(dis);

            IList listUpdateConfirmDetail = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
            for (int i = 0; i < listUpdateConfirmDetail.Count; i++)
            {
                GWBSTaskConfirm c = listUpdateConfirmDetail[i] as GWBSTaskConfirm;
                c.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                c.AccountingDetailId = null;
            }
            dao.Update(listUpdateConfirmDetail);

            dao.Delete(list);
            return true;
        }

        /// <summary>
        /// 工程任务核算单明细集合
        /// </summary>
        /// <param name="list">工程任务核算单明细集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteProjectTaskAccountDetail(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();

            for (int i = 0; i < list.Count; i++)
            {
                ProjectTaskDetailAccount dtl = list[i] as ProjectTaskDetailAccount;
                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dtl = dao.Get(typeof(ProjectTaskDetailAccount), dtl.Id) as ProjectTaskDetailAccount;

                    dis.Add(Expression.Eq("AccountingDetailId", dtl.Id));

                    list[i] = dtl;
                }
            }

            IList listUpdateConfirmDetail = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
            for (int i = 0; i < listUpdateConfirmDetail.Count; i++)
            {
                GWBSTaskConfirm c = listUpdateConfirmDetail[i] as GWBSTaskConfirm;
                c.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                c.AccountingDetailId = null;
            }
            dao.Update(listUpdateConfirmDetail);

            dao.Delete(list);

            return true;
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

        /// <summary>
        /// 保存工程任务明细核算和汇总信息
        /// </summary>
        /// <param name="dtl">工程任务明细核算</param>
        /// <param name="sum">工程任务明细汇总</param>
        /// <returns></returns>
        [TransManager]
        public bool SaveProjectTaskDetailAccountAndSummary(ProjectTaskDetailAccount dtl, ProjectTaskDetailAccountSummary sum)
        {
            dao.SaveOrUpdate(dtl);
            dao.SaveOrUpdate(sum);
            return true;
        }

        /// <summary>
        /// 保存核算明细并反写确认明细数据
        /// </summary>
        /// <param name="bill">核算单</param>
        /// <param name="listConfigDetail">确认明细集合</param>
        /// <returns></returns>
        [TransManager]
        public ProjectTaskAccountBill SaveProjectTaskAndSetConfirmState(ProjectTaskAccountBill bill, IList listConfirmDetail)
        {
            if (bill.Id == null)
            {
                bill.Code = GetCode(typeof(ProjectTaskAccountBill), bill.ProjectId);
            }
            bill.SubmitDate = DateTime.Now;
            dao.SaveOrUpdate(bill);

            IEnumerable<GWBSTaskConfirm> listTempConfirmDetail = listConfirmDetail.OfType<GWBSTaskConfirm>();

            foreach (ProjectTaskDetailAccount dtl in bill.Details)
            {
                var query = from d in listTempConfirmDetail
                            where d.GwbsSysCode.IndexOf(dtl.ProjectTaskDtlGUID.TheGWBSSysCode) > -1
                            && d.CostItem.Id == dtl.ProjectTaskDtlGUID.TheCostItem.Id
                            && d.GWBSDetail.MainResourceTypeId == dtl.ProjectTaskDtlGUID.MainResourceTypeId
                            && d.GWBSDetail.DiagramNumber == dtl.ProjectTaskDtlGUID.DiagramNumber
                            && d.Master.ConfirmHandlePerson.Id == dtl.ResponsiblePerson.Id
                            && d.TaskHandler.Id == dtl.BearerGUID.Id
                            && d.MaterialFeeSettlementFlag == dtl.MatFeeBlanceFlag
                            select d;

                foreach (GWBSTaskConfirm c in query)
                {
                    c.AccountingState = EnumGWBSTaskConfirmAccountingState.己核算;
                    c.AccountingDetailId = dtl.Id;
                    c.AccountTime = DateTime.Now;
                }
            }

            IList listUpdateConfirmDetail = listTempConfirmDetail.ToList();

            dao.Update(listUpdateConfirmDetail);

            if (bill.DocState == DocumentState.InExecute)
            {
                WriteBackAccTaskDtlQnyAndProgress(bill);
            }

            return bill;
        }
        /// <summary>
        /// 保存核算单并反写确认明细数据（包括虚拟工单）
        /// </summary>
        /// <param name="bill">核算单</param>
        /// <param name="listConfigDetail">确认明细集合</param>
        /// <returns></returns>
        [TransManager]
        public ProjectTaskAccountBill SaveAccBillAndSetCfmStateByVirCfmBill(ProjectTaskAccountBill bill, IList listConfirmDetail)
        {
            if (bill.Id == null)
            {
                bill.Code = GetCode(typeof(ProjectTaskAccountBill), bill.ProjectId);
                if (TransUtil.ToString(bill.Temp1) == "1")
                {
                    bill.Code = bill.Code.Replace("任务核算", "任务确认");
                }
            }
            dao.SaveOrUpdate(bill);

            IEnumerable<GWBSTaskConfirm> listTempConfirmDetail = listConfirmDetail.OfType<GWBSTaskConfirm>();

            foreach (ProjectTaskDetailAccount dtl in bill.Details)
            {
                var query = from d in listTempConfirmDetail
                            where d.AccountingDetailTempId == dtl.TempId
                            select d;

                foreach (GWBSTaskConfirm c in query)
                {
                    c.AccountingState = EnumGWBSTaskConfirmAccountingState.己核算;
                    c.AccountingDetailId = dtl.Id;
                    c.AccountTime = DateTime.Now;
                }
            }

            IList listUpdateConfirmDetail = listTempConfirmDetail.ToList();

            dao.Update(listUpdateConfirmDetail);

            #region 2017-02-13 chenf 将形象进度、核算工程量、进度计划完成百分比  移到结算单审结
            //
            if (bill.DocState == DocumentState.InExecute)
            {
                WriteBackAccTaskDtlQnyAndProgress(bill);
            }

            updateWeekScheduleDetail(bill);
            #endregion

            return bill;
        }

        /// <summary>
        /// 算量：根据节点下【所有明细完成量*100/所有明细计划量】得出节点的完成百分比，并更新到进度计划表中
        /// </summary>
        /// <param name="bill"></param>
        private void updateWeekScheduleDetail(ProjectTaskAccountBill bill)
        {
          
            string updateSql = string.Format(@"update thd_weekscheduledetail a
   set taskcompletedpercent =
       (select case sum(c.planworkamount * c.planprice)
                 when 0 then
                  100
                 else
                  case
                    when (sum(b.quantiyafterconfirm * c.planprice) * 100 /
                         sum(c.planworkamount * c.planprice)) > 100 then
                     100
                    else
                     (sum(b.quantiyafterconfirm * c.planprice) * 100 /
                     sum(c.planworkamount * c.planprice))
                  end
               end as prosspercent
          from thd_gwbsdetail c
          left join thd_gwbstaskconfirmdetail b
            on b.gwbsdetail = c.id
           and b.gwbstree = c.parentid
          left join thd_weekscheduledetail e
            on e.gwbstree = b.gwbstree
          left join thd_weekschedulemaster d
            on d.execscheduletype = 40
           and d.id = e.parentid
         where a.gwbstree = c.parentid
           and c.state = '5'),
       actualenddate       =
       (select max(b.realoperationdate)
          from thd_gwbstaskconfirmdetail b
         where a.gwbstree = b.gwbstree)
 where gwbstree in (select b.gwbstree
                      from thd_gwbstaskconfirmdetail b
                     where b.accountingdetailid in
                           (select tt.id
                              from thd_projecttaskdetailaccount tt
                             where tt.parentid = '{0}'))", bill.Id);

            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = updateSql;
            command.Parameters.Clear();
            command.ExecuteNonQuery();
        }

      

        private ProjectTaskAccountBill SaveProjectTaskAndSetConfirmStateBak20121206(ProjectTaskAccountBill bill, IList listConfirmDetail)
        {
            if (bill.Id == null)
            {
                bill.Code = GetCode(typeof(ProjectTaskAccountBill), bill.ProjectId);
            }
            dao.Save(bill);

            IEnumerable<GWBSTaskConfirm> listTempConfirmDetail = listConfirmDetail.OfType<GWBSTaskConfirm>();

            IList listUpdateConfirmDetail = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = null;

            foreach (ProjectTaskDetailAccount dtl in bill.Details)
            {
                var query = from d in listTempConfirmDetail
                            where d.Master.ConfirmHandlePerson == dtl.ResponsiblePerson
                            && d.TaskHandler == dtl.BearerGUID
                            select d;

                oq.Criterions.Clear();
                dis = new Disjunction();
                foreach (GWBSTaskConfirm c in query)
                {
                    dis.Add(Expression.Eq("Id", c.Id));
                }
                oq.Criterions.Add(dis);


                IList listTemp = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                if (listTemp != null && listTemp.Count > 0)
                {
                    for (int i = 0; i < listTemp.Count; i++)
                    {
                        GWBSTaskConfirm c = listTemp[i] as GWBSTaskConfirm;
                        c.AccountingState = EnumGWBSTaskConfirmAccountingState.己核算;
                        c.AccountingDetailId = dtl.Id;
                        c.AccountTime = DateTime.Now;

                        listUpdateConfirmDetail.Add(c);
                    }

                }
            }

            dao.Update(listUpdateConfirmDetail);

            return bill;
        }

        /// <summary>
        /// 生成工程任务核算单
        /// </summary>
        /// <param name="curBillMaster"></param>
        /// <returns>1.核算单，2.要回写数据的确认单明细，3.异常/提示消息</returns>
        [TransManager]
        public IList GenAccountBill(ProjectTaskAccountBill curBillMaster)
        {
            IList listResult = new ArrayList();

            List<GWBSTaskConfirm> listGWBSTaskConfirms = new List<GWBSTaskConfirm>();// 需要反写数据的确认明细集

            string errMsg = "";
            GWBSTree AccountRange = curBillMaster.AccountRange;//核算范围

            IList listAccountTaskDtlAll = null;//核算{工程任务明细}集
            IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirmTemp = null;//工程任务确认明细集

            ObjectQuery oq = new ObjectQuery();//查询类

            //1.设置工程任务核算单主表数据(客户端已设置)

            #region 2.确定<核算{工程任务明细}集>

            oq.AddCriterion(Expression.Like("TheGWBSSysCode", AccountRange.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("CostingFlag", 1));
            oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
            //oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            listAccountTaskDtlAll = dao.ObjectQuery(typeof(GWBSDetail), oq);
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            #endregion

            if (listAccountTaskDtlAll == null || listAccountTaskDtlAll.Count == 0)
            {
                errMsg = "选择核算范围内没有核算任务明细信息！";

                listResult.Add(curBillMaster);
                listResult.Add(listGWBSTaskConfirms);
                listResult.Add(errMsg);

                return listResult;
            }

            #region 3.查询任务确认明细集

            Disjunction disConfirm = new Disjunction();

            oq.AddCriterion(Expression.Like("GwbsSysCode", AccountRange.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("AccountingState", EnumGWBSTaskConfirmAccountingState.未核算));
            //添加成本项过滤条件（核算明细成本项的合集）
            var queryCostItemGroup = from d in listAccountTaskDtlAll.OfType<GWBSDetail>()
                                     group d by new { d.TheCostItem.Id } into g
                                     select new { g.Key.Id };

            foreach (var obj in queryCostItemGroup)
            {
                disConfirm.Add(Expression.Eq("CostItem.Id", obj.Id));
            }
            oq.AddCriterion(disConfirm);

            //在循环中根据核算任务明细的主资源过滤
            //oq.AddCriterion(Expression.Eq("GWBSDetail.MainResourceTypeId", mainResourceTypeId));
            oq.AddCriterion(Expression.Eq("Master.DocState", DocumentState.InExecute));//审批通过
            oq.AddCriterion(Expression.Ge("Master.CreateDate", curBillMaster.BeginTime));
            oq.AddCriterion(Expression.Le("Master.CreateDate", curBillMaster.EndTime));

            oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("CostItem", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("TaskHandler", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("Master.ConfirmHandlePerson", NHibernate.FetchMode.Eager);


            IList listConfirm = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
            oq.Criterions.Clear();
            oq.FetchModes.Clear();

            if (listConfirm == null || listConfirm.Count == 0)
            {
                errMsg = "选择核算范围和时间段内没有需要核算的工单信息,请检查！";

                listResult.Add(curBillMaster);
                listResult.Add(listGWBSTaskConfirms);
                listResult.Add(errMsg);

                return listResult;
            }


            listGWBSTaskConfirmTemp = listConfirm.OfType<GWBSTaskConfirm>();
            #endregion

            decimal planQuantity = 0;//计划工程量
            decimal planPrice = 0;//计划单价
            CostItem AccountCostItem = null;//核算成本项
            string mainResourceTypeId = null;//主资源类型Id
            string diagramNumber = null;//图号
            decimal addupFigureProgress = 0;//累计形象进度
            DateTime serverTime = DateTime.Now;//服务时间

            decimal confirmAddupFirgureProgress = 0;//工长确认累计形象进度
            decimal confirmQuantitySum = 0; //工长确认工程量汇总
            decimal AccountTotalPriceSum = 0;//核算合价汇总

            Dictionary<GWBSDetail, decimal> listAccountTaskDtl = new Dictionary<GWBSDetail, decimal>();//做了确认单的核算明细（此次需要核算的核算明细）

            foreach (GWBSDetail dtl in listAccountTaskDtlAll)
            {
                //数据设置
                planQuantity = dtl.PlanWorkAmount;
                planPrice = dtl.PlanPrice;
                AccountCostItem = dtl.TheCostItem;
                mainResourceTypeId = dtl.MainResourceTypeId;
                diagramNumber = dtl.DiagramNumber;
                addupFigureProgress = dtl.AddupAccFigureProgress;

                bool flag = false;//是否按计划（根据前驱项目周计划）做的确认单（true表示计划，false表示非计划）

                #region 4.汇集<确认工程量汇总集>,计算工长确认累计形象进度

                #region 调试用
                IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirm1 = from c in listGWBSTaskConfirmTemp
                                                                    where c.GwbsSysCode.IndexOf(dtl.TheGWBSSysCode) > -1
                                                                    select c;
                IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirm2 = from c in listGWBSTaskConfirmTemp
                                                                    where c.CostItem.Id == AccountCostItem.Id
                                                                    && c.GwbsSysCode.IndexOf(dtl.TheGWBSSysCode) > -1
                                                                    select c;
                #endregion

                IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirm = from c in listGWBSTaskConfirmTemp
                                                                   where c.CostItem.Id == AccountCostItem.Id
                                                                   && c.GWBSDetail.MainResourceTypeId == mainResourceTypeId
                                                                   && c.GWBSDetail.DiagramNumber == diagramNumber
                                                                   && c.GwbsSysCode.IndexOf(dtl.TheGWBSSysCode) > -1
                                                                   select c;


                if (listGWBSTaskConfirm == null || listGWBSTaskConfirm.Count() == 0)
                {
                    continue;
                }

                listGWBSTaskConfirms.AddRange(listGWBSTaskConfirm);//需要回写状态的确认单明细


                #region 过滤确认单中(同一任务明细、同一确认人、同一承担队伍、相同料费结算标志)重复确认的生产确认明细,取形象进度最大的

                //var queryConfirmDtlGroup = from p in listGWBSTaskConfirm
                //                           group p by new
                //                           {
                //                               proDtlId = p.GWBSDetail.Id,
                //                               bearId = p.TaskHandler.Id,
                //                               confirmPersonId = p.Master.ConfirmHandlePerson.Id,
                //                               p.MaterialFeeSettlementFlag
                //                           }
                //                               into g
                //                               select new
                //                               {
                //                                   g.Key.proDtlId,
                //                                   g.Key.bearId,
                //                                   g.Key.confirmPersonId,
                //                                   g.Key.MaterialFeeSettlementFlag
                //                               };

                //List<GWBSTaskConfirm> listGWBSTaskConfirmValid = new List<GWBSTaskConfirm>();

                //foreach (var obj in queryConfirmDtlGroup)
                //{
                //    var query = from c in listGWBSTaskConfirm
                //                where c.GWBSDetail.Id == obj.proDtlId
                //                && c.TaskHandler.Id == obj.bearId
                //                && c.Master.ConfirmHandlePerson.Id == obj.confirmPersonId
                //                && c.MaterialFeeSettlementFlag == obj.MaterialFeeSettlementFlag
                //                select c;

                //    if (query.Count() > 1)
                //    {
                //        decimal maxProgress = query.Max(q => q.ProgressAfterConfirm);

                //        query = from q in query
                //                where q.ProgressAfterConfirm == maxProgress
                //                select q;
                //    }
                //    listGWBSTaskConfirmValid.Add(query.ElementAt(0));
                //}

                //listGWBSTaskConfirm = listGWBSTaskConfirmValid;

                #endregion


                /* 
                 *  说明：确定前驱工程量确认单的类型（计划、非计划、二者皆有）
                 *  当为非计划确认单时(此时确认单直接针对核算明细)设置<操作{工程任务确认明细}集>中【确认后累积工长确认形象进度】最大值为<工长确认累积形象进度>
                 *  当为计划确认单时取本次确认形象进度的加权平均值+核算明细的已核算形象进度
                */

                var queryUnPlanConfirm = from c in listGWBSTaskConfirm
                                         where c.GWBSDetail.Id == dtl.Id
                                         select c;

                decimal currAccProgress = 0;
                if (queryUnPlanConfirm.Count() == listGWBSTaskConfirm.Count())//非计划
                {
                    flag = false;

                    confirmAddupFirgureProgress = listGWBSTaskConfirm.Max(t => t.ProgressAfterConfirm);

                }
                else if (queryUnPlanConfirm.Count() == 0)//计划
                {
                    flag = true;
                    currAccProgress = GetCurrConfirmProgress(dtl.PlanWorkAmount, listGWBSTaskConfirm);
                    confirmAddupFirgureProgress = currAccProgress + dtl.AddupAccFigureProgress;
                }
                else if (queryUnPlanConfirm.Count() > 0 && queryUnPlanConfirm.Count() < listGWBSTaskConfirm.Count())
                {
                    //当计划和非计划工程量确认单都存在时取形象进度值较大的确认单做为核算工单

                    confirmAddupFirgureProgress = queryUnPlanConfirm.Max(t => t.ProgressAfterConfirm);//非计划


                    IEnumerable<GWBSTaskConfirm> queryPlanConfirm = from c in listGWBSTaskConfirm
                                                                    where c.GWBSDetail.Id != dtl.Id
                                                                    select c;

                    currAccProgress = GetCurrConfirmProgress(dtl.PlanWorkAmount, queryPlanConfirm);
                    decimal planMaxConfirmProgress = currAccProgress + dtl.AddupAccFigureProgress;


                    if (confirmAddupFirgureProgress > planMaxConfirmProgress)//非计划
                    {
                        listGWBSTaskConfirm = queryUnPlanConfirm;
                        flag = false;
                    }
                    else
                    {
                        listGWBSTaskConfirm = queryPlanConfirm;
                        confirmAddupFirgureProgress = planMaxConfirmProgress;
                        flag = true;
                    }
                }



                //C.对<操作{工程任务确认明细}集>中的对象按照【任务承担者】、【确认责任者】、【料费结算标记】对属性【本次工长确认工程量】进行统计，统计结果为<确认工程量汇总集>。
                var queryConfirmSum = from p in listGWBSTaskConfirm
                                      group p by new
                                      {
                                          bear = p.TaskHandler,
                                          bearName = p.TaskHandlerName,
                                          confirmPerson = p.Master.ConfirmHandlePerson,
                                          confirmPersonName = p.Master.ConfirmHandlePersonName,
                                          p.MaterialFeeSettlementFlag
                                      }
                                          into g
                                          select new
                                          {
                                              g.Key.bear,
                                              g.Key.bearName,
                                              g.Key.confirmPerson,
                                              g.Key.confirmPersonName,
                                              g.Key.MaterialFeeSettlementFlag,
                                              QuantityTotal = g.Sum(o => o.ActualCompletedQuantity),
                                              count = g.Count()
                                          };

                #endregion

                #region 5.生成工程任务明细核算
                foreach (var obj in queryConfirmSum)
                {
                    //confirmQuantitySum += obj.QuantityTotal;//汇总<确认工程量汇总集>的【本次工长确认工程量】，设置为<工长确认工程量汇总>

                    ProjectTaskDetailAccount billDtl = new ProjectTaskDetailAccount();
                    billDtl.TheAccountBill = curBillMaster;
                    curBillMaster.Details.Add(billDtl);

                    billDtl.TheProjectGUID = curBillMaster.ProjectId;
                    billDtl.TheProjectName = curBillMaster.ProjectName;

                    billDtl.AccountTaskNodeGUID = dtl.TheGWBS;
                    billDtl.AccountTaskNodeName = dtl.TheGWBS.Name;
                    billDtl.AccountTaskNodeSyscode = dtl.TheGWBS.SysCode;

                    billDtl.ProjectTaskDtlGUID = dtl;
                    billDtl.ProjectTaskDtlName = dtl.Name;

                    billDtl.BearerGUID = obj.bear;
                    billDtl.BearerName = obj.bearName;

                    billDtl.ResponsiblePerson = obj.confirmPerson;
                    billDtl.ResponsiblePersonName = obj.confirmPersonName;

                    billDtl.BalanceState = ProjectTaskDetailAccountState.未结算;

                    billDtl.ContractQuantity = dtl.ContractProjectQuantity;
                    billDtl.ContractPrice = dtl.ContractPrice;
                    billDtl.ContractTotalPrice = dtl.ContractTotalPrice;

                    billDtl.ResponsibleQuantity = dtl.ResponsibilitilyWorkAmount;
                    billDtl.ResponsiblePrice = dtl.ResponsibilitilyPrice;
                    billDtl.ResponsibleTotalPrice = dtl.ResponsibilitilyTotalPrice;

                    billDtl.PlanQuantity = dtl.PlanWorkAmount;
                    billDtl.PlanPrice = dtl.PlanPrice;
                    billDtl.PlanTotalPrice = dtl.PlanTotalPrice;

                    billDtl.ConfirmQuantity = obj.QuantityTotal;//【确认工程量】：=被处理<确认工程量汇总集>_【工程量实际确认数量】；
                    billDtl.AccountProjectAmount = billDtl.ConfirmQuantity;
                    billDtl.PlanPrice = planPrice;
                    billDtl.AccountPrice = 0;
                    billDtl.AccountTotalPrice = 0;

                    if (flag)//计划
                    {
                        billDtl.CurrAccFigureProgress = currAccProgress / 100;
                    }
                    else if (billDtl.AccountProjectAmount != 0 && billDtl.PlanQuantity != 0)//非计划
                        billDtl.CurrAccFigureProgress = billDtl.AccountProjectAmount / billDtl.PlanQuantity;
                    else
                        billDtl.CurrAccFigureProgress = 0;

                    billDtl.CurrAccEV = billDtl.AccountProjectAmount * billDtl.PlanPrice;
                    billDtl.CurrContractIncomeQny = billDtl.CurrAccFigureProgress * billDtl.ContractQuantity;
                    billDtl.CurrContractIncomeTotal = billDtl.CurrContractIncomeQny * billDtl.ContractPrice;
                    billDtl.CurrResponsibleCostQny = billDtl.CurrAccFigureProgress * billDtl.ResponsibleQuantity;
                    billDtl.CurrResponsibleCostTotal = billDtl.CurrResponsibleCostQny * billDtl.ResponsiblePrice;

                    billDtl.MatFeeBlanceFlag = obj.MaterialFeeSettlementFlag;

                    billDtl.QuantityUnitGUID = dtl.WorkAmountUnitGUID;
                    billDtl.QuantityUnitName = dtl.WorkAmountUnitName;

                    billDtl.PriceUnitGUID = dtl.PriceUnitGUID;
                    billDtl.PriceUnitName = dtl.PriceUnitName;

                }
                #endregion 生成工程任务明细核算

                listAccountTaskDtl.Add(dtl, confirmAddupFirgureProgress);
            }

            if (listAccountTaskDtl.Count == 0)
            {
                errMsg = "在核算任务明细中没有找符合核算规则的工单信息！";
            }
            else
            {
                #region 6.生成资源耗用核算(放在循环里外)

                //1.查询核算明细集下的所有明细资源耗用
                ObjectQuery oqSubject = new ObjectQuery();//关联的对象，维护界面编辑数据的时候用到
                Disjunction disSubject = new Disjunction();
                foreach (GWBSDetail accDtl in listAccountTaskDtl.Keys)
                {
                    disSubject.Add(Expression.Eq("TheGWBSDetail.Id", accDtl.Id));
                }
                oqSubject.AddCriterion(disSubject);

                oqSubject.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                oqSubject.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);

                IEnumerable<GWBSDetailCostSubject> listCostSubjectAll = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oqSubject).OfType<GWBSDetailCostSubject>();

                List<string> listMaterialId = new List<string>();//存所有要用的物料对象ID

                //2.生成资源耗用核算
                foreach (ProjectTaskDetailAccount billDtl in curBillMaster.Details)
                {
                    decimal AccountProjectAmountPriceSum = 0;//核算工程量单价汇总

                    //获取指定核算明细下的资源耗用明细
                    IEnumerable<GWBSDetailCostSubject> listCostSubjectAccDtl = from s in listCostSubjectAll
                                                                               where s.TheGWBSDetail.Id == billDtl.ProjectTaskDtlGUID.Id
                                                                               && s.CostAccountSubjectGUID != null
                                                                               select s;

                    //过滤掉不需纳入结算的科目对应的资源耗用
                    listCostSubjectAccDtl = from s in listCostSubjectAccDtl
                                            where s.CostAccountSubjectGUID.IfSubBalanceFlag != 2
                                            select s;

                    //过滤掉工长确定不需纳入结算的资源耗用
                    if (billDtl.MatFeeBlanceFlag == EnumMaterialFeeSettlementFlag.不结算)
                    {
                        //根据项目中定义的模板木枋单价确定[模板木枋材料费]、[地下主体]、[地上主体]、[二次构件] 等科目是否纳入核算
                        List<string> listAccSubjectId = new List<string>();

                        CurrentProjectInfo projectInfo = dao.Get(typeof(CurrentProjectInfo), curBillMaster.ProjectId) as CurrentProjectInfo;

                        var queryAccSubject = from s in listCostSubjectAccDtl
                                              where (s.CostAccountSubjectGUID.Code == "C512080201" && projectInfo.BigModualGroundDownPrice != 0)
                                              && (s.CostAccountSubjectGUID.Code == "C512080202" && projectInfo.BigModualGroundUpPrice != 0)
                                              && (s.CostAccountSubjectGUID.Code == "C512080203" && (projectInfo.BigModualGroundUpPrice != 0 || projectInfo.BigModualGroundDownPrice != 0))
                                              select s;

                        foreach (GWBSDetailCostSubject item in queryAccSubject)
                        {
                            listAccSubjectId.Add(item.CostAccountSubjectGUID.Id);
                        }

                        //过滤出所有需要核算的资源耗用
                        listCostSubjectAccDtl = from s in listCostSubjectAccDtl
                                                where s.CostAccountSubjectGUID.IfSubBalanceFlag != 3 || listAccSubjectId.Contains(s.CostAccountSubjectGUID.Id)
                                                select s;
                    }

                    foreach (GWBSDetailCostSubject dtlUsage in listCostSubjectAccDtl)
                    {
                        ProjectTaskDetailAccountSubject billDtlUsage = new ProjectTaskDetailAccountSubject();
                        billDtlUsage.TheAccountDetail = billDtl;
                        billDtl.Details.Add(billDtlUsage);

                        billDtlUsage.TheProjectGUID = curBillMaster.ProjectId;
                        billDtlUsage.TheProjectName = curBillMaster.ProjectName;

                        billDtlUsage.BestaetigtCostSubjectGUID = dtlUsage;
                        billDtlUsage.BestaetigtCostSubjectName = dtlUsage.Name;
                        billDtlUsage.CostingSubjectGUID = dtlUsage.CostAccountSubjectGUID;
                        billDtlUsage.CostingSubjectName = dtlUsage.CostAccountSubjectName;
                        billDtlUsage.AccountCostSysCode = dtlUsage.CostAccountSubjectSyscode;
                        if (dtlUsage.ResourceTypeGUID == null || dtlUsage.ResourceTypeName == "")
                        {
                            errMsg = "核算节点[" + billDtl.AccountTaskNodeName + "]任务明细[" + billDtl.ProjectTaskDtlName + "]中存在耗用资源为空的情况！";
                        }
                        billDtlUsage.ResourceTypeGUID = dtlUsage.ResourceTypeGUID;
                        billDtlUsage.ResourceTypeName = dtlUsage.ResourceTypeName;
                        billDtlUsage.ResourceTypeQuality = dtlUsage.ResourceTypeQuality;
                        billDtlUsage.ResourceTypeSpec = dtlUsage.ResourceTypeSpec;

                        if (!string.IsNullOrEmpty(billDtlUsage.ResourceTypeGUID) && listMaterialId.Contains(billDtlUsage.ResourceTypeGUID) == false)
                        {
                            listMaterialId.Add(billDtlUsage.ResourceTypeGUID);
                        }

                        billDtlUsage.QuantityUnitGUID = dtlUsage.ProjectAmountUnitGUID;
                        billDtlUsage.QuantityUnitName = dtlUsage.ProjectAmountUnitName;
                        billDtlUsage.PriceUnitGUID = billDtl.PriceUnitGUID;
                        billDtlUsage.PriceUnitName = billDtl.PriceUnitName;

                        billDtlUsage.ContractQuotaNum = dtlUsage.ContractQuotaQuantity;
                        billDtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice;
                        billDtlUsage.ContractProjectAmountPrice = dtlUsage.ContractPrice;
                        billDtlUsage.ResContractQuantity = dtlUsage.ContractProjectAmount;
                        billDtlUsage.ContractIncomeTotal = dtlUsage.ContractTotalPrice;

                        billDtlUsage.ResponsibleQuantity = dtlUsage.ResponsibleQuotaNum;
                        billDtlUsage.ResponsibleQnyPrice = dtlUsage.ResponsibilitilyPrice;
                        billDtlUsage.ResponsibleWorkQnyPrice = dtlUsage.ResponsibleWorkPrice;
                        billDtlUsage.ResponsibleUsageQny = dtlUsage.ResponsibilitilyWorkAmount;
                        billDtlUsage.ResponsibleUsageTotal = dtlUsage.ResponsibilitilyTotalPrice;

                        billDtlUsage.PlanQuantity = dtlUsage.PlanQuotaNum;
                        billDtlUsage.PlanQnyPrice = dtlUsage.PlanPrice;
                        billDtlUsage.PlanWorkQnyPrice = dtlUsage.PlanWorkPrice;
                        billDtlUsage.PlanUsageQny = dtlUsage.PlanWorkAmount;
                        billDtlUsage.PlanUsageTotal = dtlUsage.PlanTotalPrice;

                        billDtlUsage.AccountQuantity = billDtlUsage.PlanQuantity;
                        billDtlUsage.AccountPrice = billDtlUsage.PlanQnyPrice;
                        billDtlUsage.AccWorkQnyPrice = billDtlUsage.PlanWorkQnyPrice;
                        billDtlUsage.AccUsageQny = billDtl.AccountProjectAmount * billDtlUsage.AccountQuantity;
                        billDtlUsage.AccountTotalPrice = billDtlUsage.AccUsageQny * billDtlUsage.AccountPrice;

                        billDtlUsage.CurrContractIncomeQny = billDtl.CurrAccFigureProgress * billDtlUsage.ResContractQuantity;
                        billDtlUsage.CurrContractIncomeTotal = billDtlUsage.CurrContractIncomeQny * billDtlUsage.ContractQuantityPrice;
                        billDtlUsage.CurrResponsibleCostQny = billDtl.CurrAccFigureProgress * billDtlUsage.ResponsibleUsageQny;
                        billDtlUsage.CurrResponsibleCostTotal = billDtlUsage.CurrResponsibleCostQny * billDtlUsage.ResponsibleQnyPrice;

                        AccountProjectAmountPriceSum += billDtlUsage.AccWorkQnyPrice;
                    }

                    billDtl.AccountPrice = AccountProjectAmountPriceSum;
                    billDtl.AccountTotalPrice = billDtl.AccountPrice * billDtl.AccountProjectAmount;
                }

                #region 更新耗用的资源分类信息
                if (listMaterialId.Count > 0)
                {
                    ObjectQuery oqMat = new ObjectQuery();
                    Disjunction disMat = new Disjunction();

                    foreach (string matId in listMaterialId)
                    {
                        disMat.Add(Expression.Eq("Id", matId));
                    }
                    oqMat.AddCriterion(disMat);

                    IEnumerable<Material> listMat = dao.ObjectQuery(typeof(Material), oqMat).OfType<Material>();

                    foreach (Material mat in listMat)
                    {
                        foreach (ProjectTaskDetailAccount accDtl in curBillMaster.Details)
                        {
                            var query = from d in accDtl.Details
                                        where d.ResourceTypeGUID == mat.Id
                                        select d;

                            if (query.Count() > 0)
                            {
                                foreach (ProjectTaskDetailAccountSubject item in query)
                                {
                                    item.ResourceCategorySysCode = mat.TheSyscode;
                                }
                            }
                        }
                    }
                }
                #endregion 结束 更新耗用的资源类型信息

                #endregion 结束 生成资源耗用核算

                #region 7.生成{工程任务明细核算汇总}
                foreach (GWBSDetail dtl in listAccountTaskDtl.Keys)
                {
                    IEnumerable<ProjectTaskDetailAccount> queryAccDtl = from d in curBillMaster.Details.OfType<ProjectTaskDetailAccount>()
                                                                        where d.ProjectTaskDtlGUID.Id == dtl.Id
                                                                        select d;

                    confirmQuantitySum = queryAccDtl.Sum(d => d.ConfirmQuantity);//汇总任务核算明细的确认工程量为<工长确认工程量汇总>

                    AccountTotalPriceSum = queryAccDtl.Sum(d => d.AccountTotalPrice);//汇总任务核算明细的核算合价为<核算合价汇总>

                    confirmAddupFirgureProgress = listAccountTaskDtl[dtl];//核算任务明细的累计确认形象进度

                    ProjectTaskDetailAccountSummary billSum = new ProjectTaskDetailAccountSummary();
                    billSum.TheAccountBill = curBillMaster;
                    curBillMaster.ListSummary.Add(billSum);

                    billSum.TheProjectGUID = dtl.TheProjectGUID;
                    billSum.TheProjectName = dtl.TheProjectName;

                    billSum.AccountNodeGUID = dtl.TheGWBS.Id;
                    billSum.AccountNodeName = dtl.TheGWBS.Name;
                    billSum.AccountNodeSysCode = dtl.TheGWBS.SysCode;

                    billSum.ProjectTaskDtlGUID = dtl;
                    billSum.ProjectTaskDtlName = dtl.Name;

                    billSum.AccountProjectAmount = confirmQuantitySum;
                    billSum.CurrAccFigureProgress = confirmAddupFirgureProgress - dtl.AddupAccFigureProgress;

                    billSum.AddupAccQuantity = dtl.AddupAccQuantity + billSum.AccountProjectAmount;
                    billSum.AddupAccFigureProgress = confirmAddupFirgureProgress;

                    billSum.AccountTotalPrice = AccountTotalPriceSum;

                    billSum.ProjectAmountUnitGUID = dtl.WorkAmountUnitGUID;
                    billSum.ProjectAmountUnitName = dtl.WorkAmountUnitName;

                    billSum.PriceUnitGUID = dtl.PriceUnitGUID;
                    billSum.PriceUnitName = dtl.PriceUnitName;

                    billSum.State = CollectState.已汇集;
                    billSum.Remark = "";
                }
                #endregion 结束 7.生成{工程任务明细核算汇总}

                //8.在保存核算单时反写 前驱{工程任务确认明细} 的核算标志（否则会反复生成相同任务明细的核算单）

                //9.在审批通过时反写 前驱{工程任务明细} 的【累积核算工程量】、【累积核算形象进度】
            }

            listResult.Add(curBillMaster);
            listResult.Add(listGWBSTaskConfirms);
            listResult.Add(errMsg);

            return listResult;
        }

        /// <summary>
        /// 生成工程任务核算单(包括虚拟工单)
        /// </summary>
        /// <param name="curBillMaster"></param>
        /// <returns>1.核算单，2.要回写数据的确认单明细，3.异常/提示消息</returns>
        [TransManager]
        public IList GenAccountBillByVirConfirmBill(ProjectTaskAccountBill curBillMaster)
        {
            IList listResult = new ArrayList();

            List<GWBSTaskConfirm> listGWBSTaskConfirms = new List<GWBSTaskConfirm>();// 需要反写数据的确认明细集

            string errMsg = "";
            GWBSTree AccountRange = curBillMaster.AccountRange;//核算范围

            IList listAccountTaskDtlAll = null;//核算{工程任务明细}集
            IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirmTemp = null;//工程任务确认明细集

            ObjectQuery oq = new ObjectQuery();//查询类
            #region 2.确定<核算{工程任务明细}集>

            oq.AddCriterion(Expression.Like("TheGWBSSysCode", AccountRange.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("CostingFlag", 1));
            oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
            //oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            listAccountTaskDtlAll = dao.ObjectQuery(typeof(GWBSDetail), oq);
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            #endregion
            if (listAccountTaskDtlAll == null || listAccountTaskDtlAll.Count == 0)
            {
                errMsg = "选择核算范围内没有核算任务明细信息！";

                listResult.Add(curBillMaster);
                listResult.Add(listGWBSTaskConfirms);
                listResult.Add(errMsg);

                return listResult;
            }

            #region 3.查询任务确认明细集

            IList listConfirm = null;//确认单明细集

            EnumConfirmBillType frontBillType = EnumConfirmBillType.虚拟工单;

            //1.查找虚拟工单
            oq.AddCriterion(Expression.Like("GwbsSysCode", AccountRange.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("AccountingState", EnumGWBSTaskConfirmAccountingState.未核算));
            oq.AddCriterion(Expression.Not(Expression.Eq("ActualCompletedQuantity", Convert.ToDecimal(0))));
            oq.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.虚拟工单));
            oq.AddCriterion(Expression.Eq("GWBSDetail.State", DocumentState.InExecute));
            if (!string.IsNullOrEmpty(curBillMaster.SubContractProjectID))
            {
                oq.AddCriterion(Expression.Eq("TaskHandler.Id", curBillMaster.SubContractProjectID));
            }
            oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TaskHandler", NHibernate.FetchMode.Eager);

            listConfirm = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
            oq.Criterions.Clear();
            oq.FetchModes.Clear();

            listGWBSTaskConfirmTemp = listConfirm.OfType<GWBSTaskConfirm>();

            #endregion

            decimal planQuantity = 0;//计划工程量
            decimal planPrice = 0;//计划单价
            CostItem AccountCostItem = null;//核算成本项
            string mainResourceTypeId = null;//主资源类型Id
            string diagramNumber = null;//图号
            decimal addupFigureProgress = 0;//累计形象进度
            DateTime serverTime = DateTime.Now;//服务时间

            decimal confirmAddupFirgureProgress = 0;//工长确认累计形象进度
            decimal confirmQuantitySum = 0; //工长确认工程量汇总
            decimal AccountTotalPriceSum = 0;//核算合价汇总

            Dictionary<GWBSDetail, decimal> listAccountTaskDtl = new Dictionary<GWBSDetail, decimal>();//做了确认单的核算明细（此次需要核算的核算明细）
            VirtualMachine.Component.Util.IFCGuidGenerator genId = new IFCGuidGenerator();//用于生成临时GUID

            foreach (GWBSDetail accDtl in listAccountTaskDtlAll)
            {
                //数据设置
                planQuantity = accDtl.PlanWorkAmount;
                planPrice = accDtl.PlanPrice;
                AccountCostItem = accDtl.TheCostItem;
                mainResourceTypeId = accDtl.MainResourceTypeId;
                diagramNumber = accDtl.DiagramNumber;
                addupFigureProgress = accDtl.AddupAccFigureProgress;

                #region 4.汇集<确认工程量汇总集>,计算工长确认累计形象进度

                IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirm = from c in listGWBSTaskConfirmTemp
                                                                   where c.GWBSDetail.TheCostItem.Id == AccountCostItem.Id
                                                                   && c.GWBSDetail.MainResourceTypeId == mainResourceTypeId
                                                                   && c.GWBSDetail.DiagramNumber == diagramNumber
                                                                   && c.GwbsSysCode.IndexOf(accDtl.TheGWBSSysCode) > -1
                                                                   select c;


                if (listGWBSTaskConfirm == null || listGWBSTaskConfirm.Count() == 0)
                {
                    continue;
                }

                //C.对<操作{工程任务确认明细}集>中的对象按照【任务承担者】、【确认责任者】、【料费结算标记】对属性【本次工长确认工程量】进行统计，统计结果为<确认工程量汇总集>。

                var queryConfirmSum = from p in listGWBSTaskConfirm
                                      group p by new
                                      {
                                          bear = p.TaskHandler,
                                          bearName = p.TaskHandlerName,
                                          confirmPerson = p.CreatePerson,
                                          confirmPersonName = p.CreatePersonName,
                                          p.MaterialFeeSettlementFlag
                                      }
                                          into g
                                          let remarks = g.Select(b => b.Descript).ToArray()
                                          select new
                                          {
                                              g.Key.bear,
                                              g.Key.bearName,
                                              g.Key.confirmPerson,
                                              g.Key.confirmPersonName,
                                              g.Key.MaterialFeeSettlementFlag,
                                              QuantityTotal = g.Sum(o => o.ActualCompletedQuantity),
                                              count = g.Count(),
                                              Remarks = string.Join("；", remarks)
                                          };

                #endregion

                //计算本次核算形象进度和累计核算形象进度
                decimal sumConfirmQuantity = queryConfirmSum.Sum(p => p.QuantityTotal);
                decimal currAccProgress = 0;
                if (accDtl.PlanWorkAmount != 0)
                {
                    currAccProgress = sumConfirmQuantity / accDtl.PlanWorkAmount;
                }
                confirmAddupFirgureProgress = currAccProgress * 100 + accDtl.AddupAccFigureProgress;

                #region 5.生成工程任务明细核算
                foreach (var obj in queryConfirmSum)
                {
                    ProjectTaskDetailAccount billDtl = new ProjectTaskDetailAccount();
                    billDtl.TheAccountBill = curBillMaster;
                    curBillMaster.Details.Add(billDtl);

                    billDtl.TheProjectGUID = curBillMaster.ProjectId;
                    billDtl.TheProjectName = curBillMaster.ProjectName;

                    billDtl.AccountTaskNodeGUID = accDtl.TheGWBS;
                    billDtl.AccountTaskNodeName = accDtl.TheGWBS.Name;
                    billDtl.AccountTaskNodeSyscode = accDtl.TheGWBS.SysCode;

                    billDtl.ProjectTaskDtlGUID = accDtl;
                    billDtl.ProjectTaskDtlName = accDtl.Name;

                    billDtl.BearerGUID = obj.bear;
                    billDtl.BearerName = obj.bearName;

                    billDtl.ResponsiblePerson = obj.confirmPerson;
                    billDtl.ResponsiblePersonName = obj.confirmPersonName;

                    billDtl.BalanceState = ProjectTaskDetailAccountState.未结算;

                    billDtl.ContractQuantity = accDtl.ContractProjectQuantity;
                    billDtl.ContractPrice = accDtl.ContractPrice;
                    billDtl.ContractTotalPrice = accDtl.ContractTotalPrice;

                    billDtl.ResponsibleQuantity = accDtl.ResponsibilitilyWorkAmount;
                    billDtl.ResponsiblePrice = accDtl.ResponsibilitilyPrice;
                    billDtl.ResponsibleTotalPrice = accDtl.ResponsibilitilyTotalPrice;

                    billDtl.PlanQuantity = accDtl.PlanWorkAmount;
                    billDtl.PlanPrice = accDtl.PlanPrice;
                    billDtl.PlanTotalPrice = accDtl.PlanTotalPrice;

                    billDtl.ConfirmQuantity = obj.QuantityTotal;//【汇总本次确认工程量】
                    billDtl.AccountProjectAmount = billDtl.ConfirmQuantity;
                    billDtl.PlanPrice = planPrice;
                    billDtl.AccountPrice = 0;
                    billDtl.AccountTotalPrice = 0;

                    if (billDtl.PlanQuantity != 0)
                        billDtl.CurrAccFigureProgress = billDtl.AccountProjectAmount / billDtl.PlanQuantity;
                    else
                        billDtl.CurrAccFigureProgress = 0;

                    billDtl.CurrAccEV = billDtl.AccountProjectAmount * billDtl.PlanPrice;
                    billDtl.CurrContractIncomeQny = billDtl.CurrAccFigureProgress * billDtl.ContractQuantity;
                    billDtl.CurrContractIncomeTotal = billDtl.CurrContractIncomeQny * billDtl.ContractPrice;
                    billDtl.CurrResponsibleCostQny = billDtl.CurrAccFigureProgress * billDtl.ResponsibleQuantity;
                    billDtl.CurrResponsibleCostTotal = billDtl.CurrResponsibleCostQny * billDtl.ResponsiblePrice;

                    billDtl.MatFeeBlanceFlag = obj.MaterialFeeSettlementFlag;
                    billDtl.QuantityUnitGUID = accDtl.WorkAmountUnitGUID;
                    billDtl.QuantityUnitName = accDtl.WorkAmountUnitName;
                    billDtl.PriceUnitGUID = accDtl.PriceUnitGUID;
                    billDtl.PriceUnitName = accDtl.PriceUnitName;
                    billDtl.Remark = obj.Remarks;

                    billDtl.TempId = genId.GeneratorIFCGuid();

                    var query = from c in listGWBSTaskConfirm
                                where c.TaskHandler.Id == obj.bear.Id
                                && c.CreatePerson.Id == obj.confirmPerson.Id
                                && c.MaterialFeeSettlementFlag == obj.MaterialFeeSettlementFlag
                                select c;

                    decimal addupAccQuantity = query.Sum(p => p.QuantityBeforeConfirm);

                    billDtl.AddupAccountQuantity = addupAccQuantity;
                    if (billDtl.PlanQuantity != 0)
                        billDtl.AddupAccountProgress = (addupAccQuantity / billDtl.PlanQuantity) * 100;
                    else
                        billDtl.AddupAccountProgress = 0;

                    foreach (GWBSTaskConfirm item in query)
                    {
                        item.AccountingDetailTempId = billDtl.TempId;
                    }

                    listGWBSTaskConfirms.AddRange(query);//需要回写状态的确认单明细
                }
                #endregion 生成工程任务明细核算

                listAccountTaskDtl.Add(accDtl, confirmAddupFirgureProgress);
            }

            if (listAccountTaskDtl.Count == 0)
            {
                errMsg = "在核算任务明细中没有找符合核算规则的工单信息！";
            }
            else
            {
                #region 6.生成资源耗用核算(放在循环外)

                //1.查询核算明细集下的所有明细资源耗用
                ObjectQuery oqSubject = new ObjectQuery();//关联的对象，维护界面编辑数据的时候用到
                Disjunction disSubject = new Disjunction();

                ObjectQuery oqSumAccDtl = new ObjectQuery();//核算明细汇总查询条件
                Disjunction disSumAccDtl = new Disjunction();
                foreach (GWBSDetail accDtl in listAccountTaskDtl.Keys)
                {
                    disSubject.Add(Expression.Eq("TheGWBSDetail.Id", accDtl.Id));
                    disSumAccDtl.Add(Expression.Eq("ProjectTaskDtlGUID.Id", accDtl.Id));
                }
                oqSubject.AddCriterion(disSubject);
                oqSubject.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                oqSubject.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);

                IEnumerable<GWBSDetailCostSubject> listCostSubjectAll = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oqSubject).OfType<GWBSDetailCostSubject>();

                oqSumAccDtl.AddCriterion(disSumAccDtl);
                IEnumerable<ProjectTaskDetailAccountSummary> listSumAccDtl = dao.ObjectQuery(typeof(ProjectTaskDetailAccountSummary), oqSumAccDtl).OfType<ProjectTaskDetailAccountSummary>();

                List<string> listMaterialId = new List<string>();//存所有要用的物料对象ID

                //2.生成资源耗用核算
                foreach (ProjectTaskDetailAccount billDtl in curBillMaster.Details)
                {
                    decimal AccountProjectAmountPriceSum = 0;//核算工程量单价汇总

                    //获取指定核算明细下的资源耗用明细
                    IEnumerable<GWBSDetailCostSubject> listCostSubjectAccDtl = from s in listCostSubjectAll
                                                                               where s.TheGWBSDetail.Id == billDtl.ProjectTaskDtlGUID.Id
                                                                               && s.CostAccountSubjectGUID != null
                                                                               select s;

                    #region 方式二:根据工长确认的结算耗用标记来确认耗用是否结算，如果结算则都结算，如果不结除必须结算的科目外所有都不结

                    List<GWBSDetailCostSubject> listBalanceUsage = listCostSubjectAccDtl.ToList();//要结算的耗用
                    if (billDtl.MatFeeBlanceFlag == EnumMaterialFeeSettlementFlag.不结算)
                    {
                        //过滤掉不需纳入结算的科目对应的资源耗用(主资源标志为否) 2016-9-26
                        //listBalanceUsage = (from s in listCostSubjectAccDtl
                        //                    where s.CostAccountSubjectGUID.IfSubBalanceFlag == 1
                        //                    select s).ToList();
                        listBalanceUsage = (from s in listCostSubjectAccDtl
                                            where s.MainResTypeFlag == false
                                            select s).ToList();
                    }

                    #endregion

                    foreach (GWBSDetailCostSubject dtlUsage in listCostSubjectAccDtl)
                    {
                        ProjectTaskDetailAccountSubject billDtlUsage = new ProjectTaskDetailAccountSubject();
                        billDtlUsage.TheAccountDetail = billDtl;
                        billDtl.Details.Add(billDtlUsage);

                        billDtlUsage.TheProjectGUID = curBillMaster.ProjectId;
                        billDtlUsage.TheProjectName = curBillMaster.ProjectName;

                        billDtlUsage.BestaetigtCostSubjectGUID = dtlUsage;
                        billDtlUsage.BestaetigtCostSubjectName = dtlUsage.Name;
                        billDtlUsage.CostingSubjectGUID = dtlUsage.CostAccountSubjectGUID;
                        billDtlUsage.CostingSubjectName = dtlUsage.CostAccountSubjectName;
                        billDtlUsage.AccountCostSysCode = dtlUsage.CostAccountSubjectSyscode;
                        billDtlUsage.ResourceTypeGUID = dtlUsage.ResourceTypeGUID;
                        billDtlUsage.ResourceTypeName = dtlUsage.ResourceTypeName;
                        billDtlUsage.ResourceTypeQuality = dtlUsage.ResourceTypeQuality;
                        billDtlUsage.ResourceTypeSpec = dtlUsage.ResourceTypeSpec;
                        billDtlUsage.ResourceCategorySysCode = dtlUsage.ResourceCateSyscode;

                        if (dtlUsage.ResourceTypeGUID == null || dtlUsage.ResourceTypeName == "")
                        {
                            errMsg = "核算节点[" + billDtl.AccountTaskNodeName + "]任务明细[" + billDtl.ProjectTaskDtlName + "]中存在耗用资源为空的情况！";
                        }
                        billDtlUsage.QuantityUnitGUID = dtlUsage.ProjectAmountUnitGUID;
                        billDtlUsage.QuantityUnitName = dtlUsage.ProjectAmountUnitName;
                        billDtlUsage.PriceUnitGUID = billDtl.PriceUnitGUID;
                        billDtlUsage.PriceUnitName = billDtl.PriceUnitName;


                        billDtlUsage.ContractQuotaNum = dtlUsage.ContractQuotaQuantity;
                        billDtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice;
                        billDtlUsage.ContractProjectAmountPrice = dtlUsage.ContractPrice;
                        billDtlUsage.ResContractQuantity = dtlUsage.ContractProjectAmount;
                        billDtlUsage.ContractIncomeTotal = dtlUsage.ContractTotalPrice;

                        billDtlUsage.ResponsibleQuantity = dtlUsage.ResponsibleQuotaNum;
                        billDtlUsage.ResponsibleQnyPrice = dtlUsage.ResponsibilitilyPrice;
                        billDtlUsage.ResponsibleWorkQnyPrice = dtlUsage.ResponsibleWorkPrice;
                        billDtlUsage.ResponsibleUsageQny = dtlUsage.ResponsibilitilyWorkAmount;
                        billDtlUsage.ResponsibleUsageTotal = dtlUsage.ResponsibilitilyTotalPrice;

                        billDtlUsage.PlanQuantity = dtlUsage.PlanQuotaNum;
                        billDtlUsage.PlanQnyPrice = dtlUsage.PlanPrice;
                        billDtlUsage.PlanWorkQnyPrice = dtlUsage.PlanWorkPrice;
                        billDtlUsage.PlanUsageQny = dtlUsage.PlanWorkAmount;
                        billDtlUsage.PlanUsageTotal = dtlUsage.PlanTotalPrice;


                        billDtlUsage.AccountQuantity = billDtlUsage.PlanQuantity;
                        billDtlUsage.AccountPrice = billDtlUsage.PlanQnyPrice;
                        billDtlUsage.AccWorkQnyPrice = billDtlUsage.PlanWorkQnyPrice;
                        billDtlUsage.AccUsageQny = billDtl.AccountProjectAmount * billDtlUsage.AccountQuantity;
                        billDtlUsage.AccountTotalPrice = billDtlUsage.AccUsageQny * billDtlUsage.AccountPrice;

                        //合同收入和责任成本超过100%后不纳入核算 2014-09-18
                        decimal totalProgress = 0;
                        if (billDtl.PlanQuantity != 0)
                            totalProgress = decimal.Round(billDtl.AddupAccountQuantity / billDtl.PlanQuantity, 2);
                        //var querySumAccDtl = from s in listSumAccDtl
                        //                     where s.ProjectTaskDtlGUID.Id == billDtl.ProjectTaskDtlGUID.Id
                        //                     select s;

                        if (totalProgress > 1)
                        {
                            //decimal currAccFigureProgress = 0;
                            //if (querySumAccDtl.Count() > 0)
                            //{
                            //    decimal maxProgress = querySumAccDtl.Max(p => p.AddupAccFigureProgress);
                            //    currAccFigureProgress = 1 - (maxProgress / 100);

                            //    if (currAccFigureProgress < 0)
                            //        currAccFigureProgress = 0;

                            //    var queryMaxProgress = from s in querySumAccDtl
                            //                           where s.AddupAccFigureProgress == maxProgress
                            //                           select s;
                            //    queryMaxProgress.ElementAt(0).AddupAccFigureProgress = 100;
                            //}
                            //else
                            //    currAccFigureProgress = 1;

                            decimal currAccFigureProgress = 1;
                            billDtlUsage.CurrContractIncomeQny = currAccFigureProgress * billDtlUsage.ResContractQuantity;
                            billDtlUsage.CurrResponsibleCostQny = currAccFigureProgress * billDtlUsage.ResponsibleUsageQny;
                        }
                        else
                        {
                            billDtlUsage.CurrContractIncomeQny = billDtl.CurrAccFigureProgress * billDtlUsage.ResContractQuantity;
                            billDtlUsage.CurrResponsibleCostQny = billDtl.CurrAccFigureProgress * billDtlUsage.ResponsibleUsageQny;
                        }

                        billDtlUsage.CurrContractIncomeTotal = billDtlUsage.CurrContractIncomeQny * billDtlUsage.ContractQuantityPrice;
                        billDtlUsage.CurrResponsibleCostTotal = billDtlUsage.CurrResponsibleCostQny * billDtlUsage.ResponsibleQnyPrice;

                        //确认结算标记
                        var query = from b in listBalanceUsage
                                    where b.Id == dtlUsage.Id
                                    select b;
                        //安装的两个科目(安装设备费/安装主材费)特殊处理 2016-12-13
                        if (dtlUsage.CostAccountSubjectGUID != null && (dtlUsage.CostAccountSubjectGUID.Id == TransUtil.ConAZSBSubjectId 
                            || dtlUsage.CostAccountSubjectGUID.Id == TransUtil.ConAZZCSubjectId))
                        {//两个科目的工程任务核算单中的耗用是否结算是由工长报量单明细决定的 2017-3-7
                            //billDtlUsage.IsBalance = false;
                            billDtlUsage.IsBalance = billDtlUsage.TheAccountDetail.MatFeeBlanceFlag == EnumMaterialFeeSettlementFlag.结算;
                            if (billDtlUsage.IsBalance)//如果需要结算
                            {
                                AccountProjectAmountPriceSum += billDtlUsage.AccWorkQnyPrice;
                            }
                        }
                        else
                        {
                            if (query.Count() > 0)
                            {
                                billDtlUsage.IsBalance = true;
                                AccountProjectAmountPriceSum += billDtlUsage.AccWorkQnyPrice;
                            }
                            else
                            {
                                billDtlUsage.IsBalance = false;
                            }
                        }
                    }

                    billDtl.AccountPrice = AccountProjectAmountPriceSum;
                    billDtl.AccountTotalPrice = billDtl.AccountPrice * billDtl.AccountProjectAmount;
                }

                #endregion 结束 生成资源耗用核算

                #region 7.生成{工程任务明细核算汇总}
                foreach (GWBSDetail dtl in listAccountTaskDtl.Keys)
                {
                    IEnumerable<ProjectTaskDetailAccount> queryAccDtl = from d in curBillMaster.Details.OfType<ProjectTaskDetailAccount>()
                                                                        where d.ProjectTaskDtlGUID.Id == dtl.Id
                                                                        select d;

                    confirmQuantitySum = queryAccDtl.Sum(d => d.ConfirmQuantity);//汇总任务核算明细的确认工程量为<工长确认工程量汇总>

                    AccountTotalPriceSum = queryAccDtl.Sum(d => d.AccountTotalPrice);//汇总任务核算明细的核算合价为<核算合价汇总>

                    confirmAddupFirgureProgress = listAccountTaskDtl[dtl];//核算任务明细的累计确认形象进度

                    ProjectTaskDetailAccountSummary billSum = new ProjectTaskDetailAccountSummary();
                    billSum.TheAccountBill = curBillMaster;
                    curBillMaster.ListSummary.Add(billSum);

                    billSum.TheProjectGUID = dtl.TheProjectGUID;
                    billSum.TheProjectName = dtl.TheProjectName;

                    billSum.AccountNodeGUID = dtl.TheGWBS.Id;
                    billSum.AccountNodeName = dtl.TheGWBS.Name;
                    billSum.AccountNodeSysCode = dtl.TheGWBS.SysCode;

                    billSum.ProjectTaskDtlGUID = dtl;
                    billSum.ProjectTaskDtlName = dtl.Name;

                    billSum.AccountProjectAmount = confirmQuantitySum;
                    billSum.CurrAccFigureProgress = confirmAddupFirgureProgress - dtl.AddupAccFigureProgress;

                    billSum.AddupAccQuantity = dtl.AddupAccQuantity + billSum.AccountProjectAmount;
                    //billSum.AddupAccFigureProgress = confirmAddupFirgureProgress;
                    if (dtl.PlanWorkAmount != 0)
                    {
                        billSum.AddupAccFigureProgress = decimal.Round((dtl.AddupAccQuantity + billSum.AccountProjectAmount) * 100 / dtl.PlanWorkAmount, 2);//2014-09-15
                    }
                    //else
                    //{
                    //    throw new Exception("[" + dtl.TheGWBSFullPath + "]的计划工程量为0！");
                    //}

                    billSum.AccountTotalPrice = AccountTotalPriceSum;

                    billSum.ProjectAmountUnitGUID = dtl.WorkAmountUnitGUID;
                    billSum.ProjectAmountUnitName = dtl.WorkAmountUnitName;

                    billSum.PriceUnitGUID = dtl.PriceUnitGUID;
                    billSum.PriceUnitName = dtl.PriceUnitName;

                    billSum.State = CollectState.已汇集;
                    billSum.Remark = "";
                }
                #endregion 结束 7.生成{工程任务明细核算汇总}
            }

            curBillMaster.FrontConfirmBillType = frontBillType;

            listResult.Add(curBillMaster);
            listResult.Add(listGWBSTaskConfirms);
            listResult.Add(errMsg);

            return listResult;
        }

        public DataSet SearchSQL(string sql)
        {
            DataSet ds = new DataSet();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;

            IDbCommand cmd = cnn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            IDataReader dr = cmd.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        private ProjectTaskAccountBill GenAccountBillBak20121206(ProjectTaskAccountBill curBillMaster, out List<GWBSTaskConfirm> listGWBSTaskConfirms, out string errMsg)
        {
            listGWBSTaskConfirms = new List<GWBSTaskConfirm>();// 需要反写数据的确认明细集

            errMsg = "";

            GWBSTree AccountRange = curBillMaster.AccountRange;//核算范围


            IList listAccountTaskDtl = null;//核算{工程任务明细}集
            IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirmTemp = null;//工程任务确认明细集

            ObjectQuery oq = new ObjectQuery();//查询类


            //1.设置工程任务核算单主表数据(客户端已设置)


            #region 2.确定<核算{工程任务明细}集>

            oq.AddCriterion(Expression.Like("TheGWBSSysCode", AccountRange.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("CostingFlag", 1));
            oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
            listAccountTaskDtl = dao.ObjectQuery(typeof(GWBSDetail), oq);
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            #endregion

            if (listAccountTaskDtl == null || listAccountTaskDtl.Count == 0)
            {
                errMsg = "选择核算范围内没有要核算明细信息！";
                return curBillMaster;
            }

            decimal planQuantity = 0;//计划工程量
            decimal planPrice = 0;//计划单价
            CostItem AccountCostItem = null;//核算成本项
            string mainResourceTypeId = null;//主资源类型Id
            decimal addupFigureProgress = 0;//累计形象进度
            DateTime serverTime = DateTime.Now;//服务时间

            decimal confirmAddupFirgureProgress = 0;//工长确认累计形象进度
            decimal confirmQuantitySum = 0; //工长确认工程量汇总



            //查询任务确认明细集
            GWBSTaskConfirm task = new GWBSTaskConfirm();
            oq.AddCriterion(Expression.Like("GwbsSysCode", AccountRange.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("AccountingState", EnumGWBSTaskConfirmAccountingState.未核算));
            oq.AddCriterion(Expression.Eq("Master.DocState", DocumentState.InExecute));//审批通过

            //在循环中根据核算任务明细过滤
            //oq.AddCriterion(Expression.Eq("CostItem.Id", AccountCostItem.Id));
            //oq.AddCriterion(Expression.Eq("GWBSDetail.MainResourceTypeId", mainResourceTypeId));
            oq.AddCriterion(Expression.Gt("Master.CreateDate", curBillMaster.BeginTime));
            oq.AddCriterion(Expression.Le("Master.CreateDate", curBillMaster.EndTime));

            oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("CostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TaskHandler", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Master.ConfirmHandlePerson", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("Master.SubContractProject", NHibernate.FetchMode.Eager);


            IList listConfirm = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
            oq.Criterions.Clear();
            oq.FetchModes.Clear();

            if (listConfirm == null || listConfirm.Count == 0)
            {
                errMsg = "选择根节点和核算时间段内没有需要核算的数据,请检查！";
                return curBillMaster;
            }

            listGWBSTaskConfirmTemp = listConfirm.OfType<GWBSTaskConfirm>();

            listGWBSTaskConfirms.Clear();
            listGWBSTaskConfirms.AddRange(listGWBSTaskConfirmTemp);

            List<string> listMaterialId = new List<string>();//存所有要用的物料对象ID

            foreach (GWBSDetail dtl in listAccountTaskDtl)
            {

                //3.数据设置

                planQuantity = dtl.PlanWorkAmount;
                planPrice = dtl.PlanPrice;
                AccountCostItem = dtl.TheCostItem;
                mainResourceTypeId = dtl.MainResourceTypeId;
                addupFigureProgress = dtl.AddupAccFigureProgress;

                confirmQuantitySum = 0;


                bool flag = false;//是否按计划（根据前驱项目周计划）做的确认单（true表示计划，false表示非计划）



                #region 4.汇集<确认工程量汇总集> 、生成工程任务明细核算、生成资源耗用核算

                //调试用
                //IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirm1 = from c in listGWBSTaskConfirmTemp
                //                                                   where c.GwbsSysCode.IndexOf(dtl.TheGWBSSysCode) > -1
                //                                                   select c;
                //IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirm2 = from c in listGWBSTaskConfirmTemp
                //                                                   where c.CostItem.Id == AccountCostItem.Id
                //                                                   && c.GwbsSysCode.IndexOf(dtl.TheGWBSSysCode) > -1
                //                                                   select c;

                IEnumerable<GWBSTaskConfirm> listGWBSTaskConfirm = from c in listGWBSTaskConfirmTemp
                                                                   where c.CostItem.Id == AccountCostItem.Id
                                                                   && c.GWBSDetail.MainResourceTypeId == mainResourceTypeId
                                                                   && c.GwbsSysCode.IndexOf(dtl.TheGWBSSysCode) > -1
                                                                   select c;

                if (listGWBSTaskConfirm == null || listGWBSTaskConfirm.Count() == 0)
                {
                    continue;
                }


                /* 
                 *  说明：确定前驱工程量确认单的类型（计划、非计划、二者皆有）
                 *  当为非计划确认单时(此时确认单直接针对核算明细)设置<操作{工程任务确认明细}集>中【确认后累积工长确认形象进度】最大值为<工长确认累积形象进度>
                 *  当为计划确认单时取本次确认形象进度的加权平均值+核算明细的已核算形象进度
                */

                var queryUnPlanConfirm = from c in listGWBSTaskConfirm
                                         where c.GWBSDetail.Id == dtl.Id
                                         select c;

                decimal currAccProgress = 0;
                if (queryUnPlanConfirm.Count() == listGWBSTaskConfirm.Count())//非计划
                {
                    flag = false;

                    confirmAddupFirgureProgress = listGWBSTaskConfirm.Max(t => t.ProgressAfterConfirm);
                }
                else if (queryUnPlanConfirm.Count() == 0)//计划
                {
                    flag = true;
                    currAccProgress = GetCurrConfirmProgress(dtl.PlanWorkAmount, listGWBSTaskConfirm);
                    confirmAddupFirgureProgress = currAccProgress + dtl.AddupAccFigureProgress;
                }
                else if (queryUnPlanConfirm.Count() > 0 && queryUnPlanConfirm.Count() < listGWBSTaskConfirm.Count())
                {
                    //当计划和非计划工程量确认单都存在时取形象进度值较大的确认单做为核算工单

                    confirmAddupFirgureProgress = queryUnPlanConfirm.Max(t => t.ProgressAfterConfirm);//非计划


                    IEnumerable<GWBSTaskConfirm> queryPlanConfirm = from c in listGWBSTaskConfirm
                                                                    where c.GWBSDetail.Id != dtl.Id
                                                                    select c;

                    currAccProgress = GetCurrConfirmProgress(dtl.PlanWorkAmount, queryPlanConfirm);
                    decimal planMaxConfirmProgress = currAccProgress + dtl.AddupAccFigureProgress;

                    if (confirmAddupFirgureProgress > planMaxConfirmProgress)//非计划
                    {
                        listGWBSTaskConfirm = queryUnPlanConfirm;
                        flag = false;
                    }
                    else
                    {
                        listGWBSTaskConfirm = queryPlanConfirm;
                        confirmAddupFirgureProgress = planMaxConfirmProgress;
                        flag = true;
                    }
                }


                decimal AccountTotalPriceSum = 0;//核算合价汇总

                //C.对<操作{工程任务确认明细}集>中的对象按照【任务承担者】、【确认责任者】、【料费结算标记】对属性【本次工长确认工程量】进行统计，统计结果为<确认工程量汇总集>。
                var queryConfirmSum = from p in listGWBSTaskConfirm
                                      group p by new
                                      {
                                          bear = p.TaskHandler,
                                          confirmPerson = p.Master.ConfirmHandlePerson,
                                          p.MaterialFeeSettlementFlag
                                      }
                                          into g
                                          select new
                                          {
                                              g.Key.bear,
                                              g.Key.confirmPerson,
                                              g.Key.MaterialFeeSettlementFlag,
                                              QuantityTotal = g.Sum(o => o.ActualCompletedQuantity),
                                              count = g.Count()
                                          };



                //D.汇总<确认工程量汇总集>的【本次工长确认工程量】，设置为<工长确认工程量汇总>
                foreach (var obj in queryConfirmSum)
                {
                    confirmQuantitySum += obj.QuantityTotal;

                    #region 5.生成工程任务明细核算
                    ProjectTaskDetailAccount billDtl = new ProjectTaskDetailAccount();
                    billDtl.TheAccountBill = curBillMaster;
                    curBillMaster.Details.Add(billDtl);

                    billDtl.TheProjectGUID = curBillMaster.ProjectId;
                    billDtl.TheProjectName = curBillMaster.ProjectName;

                    billDtl.AccountTaskNodeGUID = dtl.TheGWBS;
                    billDtl.AccountTaskNodeName = dtl.TheGWBS.Name;
                    billDtl.AccountTaskNodeSyscode = dtl.TheGWBS.SysCode;

                    billDtl.ProjectTaskDtlGUID = dtl;
                    billDtl.ProjectTaskDtlName = dtl.Name;

                    billDtl.BearerGUID = obj.bear;
                    billDtl.BearerName = obj.bear.BearerOrgName;

                    billDtl.ResponsiblePerson = obj.confirmPerson;
                    billDtl.ResponsiblePersonName = obj.confirmPerson.Name;

                    billDtl.BalanceState = ProjectTaskDetailAccountState.未结算;

                    billDtl.ContractQuantity = dtl.ContractProjectQuantity;
                    billDtl.ContractPrice = dtl.ContractPrice;
                    billDtl.ContractTotalPrice = dtl.ContractTotalPrice;

                    billDtl.ResponsibleQuantity = dtl.ResponsibilitilyWorkAmount;
                    billDtl.ResponsiblePrice = dtl.ResponsibilitilyPrice;
                    billDtl.ResponsibleTotalPrice = dtl.ResponsibilitilyTotalPrice;

                    billDtl.PlanQuantity = dtl.PlanWorkAmount;
                    billDtl.PlanPrice = dtl.PlanPrice;
                    billDtl.PlanTotalPrice = dtl.PlanTotalPrice;

                    billDtl.ConfirmQuantity = obj.QuantityTotal;//【确认工程量】：=被处理<确认工程量汇总集>_【工程量实际确认数量】；
                    billDtl.AccountProjectAmount = billDtl.ConfirmQuantity;
                    billDtl.PlanPrice = planPrice;
                    billDtl.AccountPrice = 0;
                    billDtl.AccountTotalPrice = 0;

                    if (flag)//计划
                    {
                        billDtl.CurrAccFigureProgress = currAccProgress / 100;
                    }
                    else if (billDtl.AccountProjectAmount != 0 && billDtl.PlanQuantity != 0)//非计划
                        billDtl.CurrAccFigureProgress = billDtl.AccountProjectAmount / billDtl.PlanQuantity;
                    else
                        billDtl.CurrAccFigureProgress = 0;

                    billDtl.CurrAccEV = billDtl.AccountProjectAmount * billDtl.PlanPrice;
                    billDtl.CurrContractIncomeQny = billDtl.CurrAccFigureProgress * billDtl.ContractQuantity;
                    billDtl.CurrContractIncomeTotal = billDtl.CurrContractIncomeQny * billDtl.ContractPrice;
                    billDtl.CurrResponsibleCostQny = billDtl.CurrAccFigureProgress * billDtl.ResponsibleQuantity;
                    billDtl.CurrResponsibleCostTotal = billDtl.CurrResponsibleCostQny * billDtl.ResponsiblePrice;

                    billDtl.MatFeeBlanceFlag = obj.MaterialFeeSettlementFlag;

                    billDtl.QuantityUnitGUID = dtl.WorkAmountUnitGUID;
                    billDtl.QuantityUnitName = dtl.WorkAmountUnitName;

                    billDtl.PriceUnitGUID = dtl.PriceUnitGUID;
                    billDtl.PriceUnitName = dtl.PriceUnitName;

                    decimal AccountProjectAmountPriceSum = 0;//核算工程量单价汇总

                    #region 6.生成资源耗用核算

                    //if (obj.MaterialFeeSettlementFlag == EnumMaterialFeeSettlementFlag.结算)
                    //{

                    ObjectQuery oqSubject = new ObjectQuery();//关联对象，编辑的时候用到
                    oqSubject.AddCriterion(Expression.Eq("TheGWBSDetail.Id", dtl.Id));
                    oqSubject.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                    oqSubject.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                    IList listCostSubject = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oqSubject);

                    foreach (GWBSDetailCostSubject dtlUsage in listCostSubject)
                    {
                        ProjectTaskDetailAccountSubject billDtlUsage = new ProjectTaskDetailAccountSubject();
                        billDtlUsage.TheAccountDetail = billDtl;
                        billDtl.Details.Add(billDtlUsage);

                        billDtlUsage.TheProjectGUID = curBillMaster.ProjectId;
                        billDtlUsage.TheProjectName = curBillMaster.ProjectName;

                        billDtlUsage.BestaetigtCostSubjectGUID = dtlUsage;
                        billDtlUsage.BestaetigtCostSubjectName = dtlUsage.Name;
                        billDtlUsage.CostingSubjectGUID = dtlUsage.CostAccountSubjectGUID;
                        billDtlUsage.CostingSubjectName = dtlUsage.CostAccountSubjectName;
                        billDtlUsage.ResourceTypeGUID = dtlUsage.ResourceTypeGUID;
                        billDtlUsage.ResourceTypeName = dtlUsage.ResourceTypeName;
                        billDtlUsage.ResourceTypeQuality = dtlUsage.ResourceTypeQuality;
                        billDtlUsage.ResourceTypeSpec = dtlUsage.ResourceTypeSpec;

                        if (!string.IsNullOrEmpty(billDtlUsage.ResourceTypeGUID) && listMaterialId.Contains(billDtlUsage.ResourceTypeGUID) == false)
                        {
                            listMaterialId.Add(billDtlUsage.ResourceTypeGUID);

                            //Material mat = dao.Get(typeof(Material), billDtlUsage.ResourceTypeGUID) as Material;
                            //if (mat != null)
                            //    billDtlUsage.ResourceCategorySysCode = mat.TheSyscode;
                        }


                        billDtlUsage.QuantityUnitGUID = dtlUsage.ProjectAmountUnitGUID;
                        billDtlUsage.QuantityUnitName = dtlUsage.ProjectAmountUnitName;
                        billDtlUsage.PriceUnitGUID = dtl.PriceUnitGUID;
                        billDtlUsage.PriceUnitName = dtl.PriceUnitName;


                        billDtlUsage.ContractQuotaNum = dtlUsage.ContractQuotaQuantity;
                        billDtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice;
                        billDtlUsage.ContractProjectAmountPrice = dtlUsage.ContractPrice;
                        billDtlUsage.ResContractQuantity = dtlUsage.ContractProjectAmount;
                        billDtlUsage.ContractIncomeTotal = dtlUsage.ContractTotalPrice;

                        billDtlUsage.ResponsibleQuantity = dtlUsage.ResponsibleQuotaNum;
                        billDtlUsage.ResponsibleQnyPrice = dtlUsage.ResponsibilitilyPrice;
                        billDtlUsage.ResponsibleWorkQnyPrice = dtlUsage.ResponsibleWorkPrice;
                        billDtlUsage.ResponsibleUsageQny = dtlUsage.ResponsibilitilyWorkAmount;
                        billDtlUsage.ResponsibleUsageTotal = dtlUsage.ResponsibilitilyTotalPrice;

                        billDtlUsage.PlanQuantity = dtlUsage.PlanQuotaNum;
                        billDtlUsage.PlanQnyPrice = dtlUsage.PlanPrice;
                        billDtlUsage.PlanWorkQnyPrice = dtlUsage.PlanWorkPrice;
                        billDtlUsage.PlanUsageQny = dtlUsage.PlanWorkAmount;
                        billDtlUsage.PlanUsageTotal = dtlUsage.PlanTotalPrice;


                        billDtlUsage.AccountQuantity = billDtlUsage.PlanQuantity;
                        billDtlUsage.AccountPrice = billDtlUsage.PlanQnyPrice;
                        billDtlUsage.AccWorkQnyPrice = billDtlUsage.PlanWorkQnyPrice;
                        billDtlUsage.AccUsageQny = billDtl.AccountProjectAmount * billDtlUsage.AccountQuantity;
                        billDtlUsage.AccountTotalPrice = billDtlUsage.AccUsageQny * billDtlUsage.AccountPrice;

                        billDtlUsage.CurrContractIncomeQny = billDtl.CurrAccFigureProgress * billDtlUsage.ResContractQuantity;
                        billDtlUsage.CurrContractIncomeTotal = billDtlUsage.CurrContractIncomeQny * billDtlUsage.ContractProjectAmountPrice;
                        billDtlUsage.CurrResponsibleCostQny = billDtl.CurrAccFigureProgress * billDtlUsage.ResponsibleUsageQny;
                        billDtlUsage.CurrResponsibleCostTotal = billDtlUsage.CurrResponsibleCostQny * billDtlUsage.ResponsibleQnyPrice;

                        AccountProjectAmountPriceSum += billDtlUsage.AccWorkQnyPrice;
                    }

                    //}

                    #endregion 生成资源耗用核算

                    billDtl.AccountPrice = AccountProjectAmountPriceSum;
                    billDtl.AccountTotalPrice = billDtl.AccountPrice * billDtl.AccountProjectAmount;

                    #endregion 生成工程任务明细核算

                    AccountTotalPriceSum += billDtl.AccountTotalPrice;
                }
                #endregion

                #region 7.生成{工程任务明细核算汇总}

                ProjectTaskDetailAccountSummary billSum = new ProjectTaskDetailAccountSummary();
                billSum.TheAccountBill = curBillMaster;
                curBillMaster.ListSummary.Add(billSum);

                billSum.TheProjectGUID = dtl.TheProjectGUID;
                billSum.TheProjectName = dtl.TheProjectName;

                billSum.AccountNodeGUID = dtl.TheGWBS.Id;
                billSum.AccountNodeName = dtl.TheGWBS.Name;
                billSum.AccountNodeSysCode = dtl.TheGWBS.SysCode;

                billSum.ProjectTaskDtlGUID = dtl;
                billSum.ProjectTaskDtlName = dtl.Name;

                billSum.AccountProjectAmount = confirmQuantitySum;
                billSum.CurrAccFigureProgress = confirmAddupFirgureProgress - dtl.AddupAccFigureProgress;

                billSum.AddupAccQuantity = dtl.AddupAccQuantity + billSum.AccountProjectAmount;
                billSum.AddupAccFigureProgress = confirmAddupFirgureProgress;

                billSum.AccountTotalPrice = AccountTotalPriceSum;

                billSum.ProjectAmountUnitGUID = dtl.WorkAmountUnitGUID;
                billSum.ProjectAmountUnitName = dtl.WorkAmountUnitName;

                billSum.PriceUnitGUID = dtl.PriceUnitGUID;
                billSum.PriceUnitName = dtl.PriceUnitName;

                billSum.State = CollectState.已汇集;
                billSum.Remark = "";

                #endregion

                //8.在保存核算单时反写 前驱{工程任务确认明细} 的核算标志

                //9.在审批通过时反写 前驱{工程任务明细} 的【累积核算工程量】、【累积核算形象进度】

            }

            //更新耗用的资源类型信息
            if (listMaterialId.Count > 0)
            {
                ObjectQuery oqMat = new ObjectQuery();
                Disjunction disMat = new Disjunction();

                foreach (string matId in listMaterialId)
                {
                    disMat.Add(Expression.Eq("Id", matId));
                }
                oqMat.AddCriterion(disMat);

                IEnumerable<Material> listMat = dao.ObjectQuery(typeof(Material), oqMat).OfType<Material>();

                foreach (Material mat in listMat)
                {
                    foreach (ProjectTaskDetailAccount accDtl in curBillMaster.Details)
                    {
                        var query = from d in accDtl.Details
                                    where d.ResourceTypeGUID == mat.Id
                                    select d;

                        if (query.Count() > 0)
                        {
                            foreach (ProjectTaskDetailAccountSubject item in query)
                            {
                                item.ResourceCategorySysCode = mat.TheSyscode;
                            }
                        }
                    }
                }
            }

            return curBillMaster;
        }

        /// <summary>
        /// 获取工程量确认单的本次确认形象进度（按工程量算加权平均）
        /// </summary>
        /// <param name="accDtlPlanQuantity">核算明细的计划工程量</param>
        /// <param name="list">核算明细下属工程量确认单集</param>
        /// <returns></returns>
        private decimal GetCurrConfirmProgress(decimal accDtlPlanQuantity, IEnumerable<GWBSTaskConfirm> list)
        {
            decimal confirmProgress = 0;
            if (accDtlPlanQuantity != 0)
            {
                foreach (GWBSTaskConfirm c in list)
                {
                    decimal percent = c.GWBSDetail.PlanWorkAmount / accDtlPlanQuantity;//以本次确认的工程量和总工程量的比作为权重
                    confirmProgress += (c.ProgressAfterConfirm - c.ProgressBeforeConfirm) * percent;
                }
            }

            return confirmProgress;
        }

        /// <summary>
        /// 根据审批通过的核算单回写前驱核算任务明细的核算工程量和形象进度
        /// </summary>
        /// <param name="bill"></param>
        [TransManager]
        public void WriteBackAccTaskDtlQnyAndProgress(ProjectTaskAccountBill bill)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            //回写核算任务明细的累计核算工程量和累计核算形象进度
            //foreach (ProjectTaskDetailAccountSummary item in bill.ListSummary)//2015-09-25
            //{
            //    dis.Add(Expression.Eq("Id", item.ProjectTaskDtlGUID.Id));
            //}
            foreach (ProjectTaskDetailAccount item in bill.Details)
            {
                dis.Add(Expression.Eq("Id", item.ProjectTaskDtlGUID.Id));
            }
            oq.AddCriterion(dis);
            IList listDtl = dao.ObjectQuery(typeof(GWBSDetail), oq);

            #region chenf 2017-02-14 chenf  去掉更新形象进度以及累计核算工程量 将其移到结算单审核
            
            //foreach (GWBSDetail dtl in listDtl)
            //{
            //    /*var query = from a in bill.ListSummary
            //                where a.ProjectTaskDtlGUID.Id == dtl.Id
            //                select a;
            //    ProjectTaskDetailAccountSummary sum = query.ElementAt(0);
            //    dtl.AddupAccQuantity = sum.AddupAccQuantity;*/
            //    //2015-09-25
            //    decimal sumAcctQty = 0;//本次累计核算工程量
            //    foreach (ProjectTaskDetailAccount item in bill.Details)
            //    {
            //        if (dtl.Id == item.ProjectTaskDtlGUID.Id)
            //        {
            //            sumAcctQty += item.AccountProjectAmount;
            //        }
            //    }

            //    dtl.AddupAccQuantity += sumAcctQty;
            //    if (dtl.PlanWorkAmount != 0)
            //    {
            //        dtl.AddupAccFigureProgress = decimal.Round(dtl.AddupAccQuantity * 100 / dtl.PlanWorkAmount, 2);
            //    }
            //}

            //dao.Update(listDtl);
        
            #endregion

            oq.Criterions.Clear();
            dis = new Disjunction();
            //回写虚拟工单的确认前累计工程量等于确认后累计工程量，本次确认工程量为0,核算状态为未核算
            foreach (ProjectTaskDetailAccount item in bill.Details)
            {
                dis.Add(Expression.Eq("AccountingDetailId", item.Id));
            }
            oq.AddCriterion(dis);
            oq.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.虚拟工单));
            IList listVirConfirmBillDtl = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);

            decimal sumQtyQR = 0;
            int i = 1;
            int count = listVirConfirmBillDtl.Count;
            foreach (GWBSTaskConfirm dtl in listVirConfirmBillDtl)
            {
                decimal acctQty = 0;//本次核算工程量
                decimal acctQty1 = 0;//本次提报工程量
                foreach (ProjectTaskDetailAccount item in bill.Details)
                {
                    if (dtl.GWBSDetail.TheCostItem.Id == item.ProjectTaskDtlGUID.TheCostItem.Id
                       && dtl.GWBSDetail.MainResourceTypeId == item.ProjectTaskDtlGUID.MainResourceTypeId
                       && dtl.GWBSDetail.DiagramNumber == item.ProjectTaskDtlGUID.DiagramNumber
                       && dtl.GwbsSysCode.IndexOf(item.ProjectTaskDtlGUID.TheGWBSSysCode) > -1)
                    {
                        acctQty += item.AccountProjectAmount;
                        acctQty1 += item.ConfirmQuantity;
                    }
                }
                if (i == count)
                {
                    dtl.QuantityBeforeConfirm += (acctQty - sumQtyQR);
                }
                else
                {
                    dtl.QuantityBeforeConfirm += acctQty*(dtl.ActualCompletedQuantity/acctQty1);
                    sumQtyQR+=acctQty*(dtl.ActualCompletedQuantity/acctQty1);
                }
                i++;
                //dtl.QuantityBeforeConfirm += acctQty;
                dtl.ActualCompletedQuantity = 0;
                decimal planWorkAmount = 0;
                foreach (GWBSDetail gdtl in listDtl)
                {
                    if (gdtl.Id == dtl.GWBSDetail.Id)
                    {
                        planWorkAmount = gdtl.PlanWorkAmount;
                    }
                }
                if (planWorkAmount != 0)
                {
                    dtl.ProgressBeforeConfirm = decimal.Round(dtl.QuantityBeforeConfirm * 100 / planWorkAmount, 2);
                }
                else
                {
                    dtl.ProgressBeforeConfirm = 0;
                }
                //dtl.ProgressBeforeConfirm += dtl.CompletedPercent;
                dtl.CompletedPercent = 0;
                dtl.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                dtl.AccountTime = null;
            }
            dao.Update(listVirConfirmBillDtl);
        }
        /// <summary>
        /// 根据审批通过的核算单回写前驱核算任务明细的核算工程量和形象进度
        /// </summary>
        /// <param name="accBillId"></param>
        [TransManager]
        public void WriteBackAccTaskDtlQnyAndProgress(string accBillId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", accBillId));
            oq.AddFetchMode("Details", FetchMode.Eager);
            //oq.AddFetchMode("ListSummary", FetchMode.Eager);
            IList list = dao.ObjectQuery(typeof(ProjectTaskAccountBill), oq);

            ProjectTaskAccountBill bill = list[0] as ProjectTaskAccountBill;
            WriteBackAccTaskDtlQnyAndProgress(bill);
        }

        /// <summary>
        /// 删除工程任务耗用核算
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteProjectTaskAccountDtlUsage(IList list)
        {
            return dao.Delete(list);
        }

        public void RollbackAccTaskDtlQnyAndProgress(IList billDetails)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (ProjectTaskDetailAccount item in billDetails)
            {
                dis.Add(Expression.Eq("Id", item.ProjectTaskDtlGUID.Id));
            }
            oq.AddCriterion(dis);
            IList listDtl = dao.ObjectQuery(typeof(GWBSDetail), oq);

            foreach (GWBSDetail dtl in listDtl)
            {
                /*var query = from a in bill.ListSummary
                            where a.ProjectTaskDtlGUID.Id == dtl.Id
                            select a;
                ProjectTaskDetailAccountSummary sum = query.ElementAt(0);
                dtl.AddupAccQuantity = sum.AddupAccQuantity;*/
                //2015-09-25
                decimal sumAcctQty = 0;//本次累计核算工程量
                foreach (ProjectTaskDetailAccount item in billDetails)
                {
                    if (dtl.Id == item.ProjectTaskDtlGUID.Id)
                    {
                        sumAcctQty += item.AccountProjectAmount;
                    }
                }

                dtl.AddupAccQuantity -= sumAcctQty;
                if (dtl.PlanWorkAmount != 0)
                {
                    dtl.AddupAccFigureProgress = decimal.Round(dtl.AddupAccQuantity * 100 / dtl.PlanWorkAmount, 2);
                }
            }

            dao.Update(listDtl);

            oq.Criterions.Clear();
            dis = new Disjunction();
            //回写虚拟工单的确认前累计工程量等于确认后累计工程量，本次确认工程量为0,核算状态为未核算
            foreach (ProjectTaskDetailAccount item in billDetails)
            {
                dis.Add(Expression.Eq("AccountingDetailId", item.Id));
            }
            oq.AddCriterion(dis);
            oq.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.虚拟工单));
            IList listVirConfirmBillDtl = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);

            foreach (GWBSTaskConfirm dtl in listVirConfirmBillDtl)
            {
                decimal acctQty = 0;//本次核算工程量
                foreach (ProjectTaskDetailAccount item in billDetails)
                {
                    if (dtl.GWBSDetail.Id == item.ProjectTaskDtlGUID.Id && dtl.CreatePerson.Id == item.ResponsiblePerson.Id
                        && dtl.MaterialFeeSettlementFlag == item.MatFeeBlanceFlag)
                    {
                        acctQty += item.AccountProjectAmount;
                    }
                }
                dtl.QuantityBeforeConfirm -= acctQty;
                dtl.ActualCompletedQuantity = 0;
                decimal planWorkAmount = 0;
                foreach (GWBSDetail gdtl in listDtl)
                {
                    if (gdtl.Id == dtl.GWBSDetail.Id)
                    {
                        planWorkAmount = gdtl.PlanWorkAmount;
                    }
                }
                if (planWorkAmount != 0)
                {
                    dtl.ProgressBeforeConfirm = decimal.Round(dtl.QuantityBeforeConfirm * 100 / planWorkAmount, 2);
                    dtl.CompletedPercent = decimal.Round(dtl.ActualCompletedQuantity * 100 / planWorkAmount, 2);
                }
                else
                {
                    dtl.ProgressBeforeConfirm = 0;
                    dtl.CompletedPercent = 0;
                }

                dtl.AccountingDetailId = null;
                dtl.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                dtl.AccountTime = null;
            }
            dao.Update(listVirConfirmBillDtl);
        }
        public bool IsNeedProjectTaskAccount(string sProjectId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = "select t.projectname from thd_gwbstaskconfirmmaster t where t.projectid=:ProjectId and exists(select 1 from  thd_gwbstaskconfirmdetail t1 where t.id=t1.parentid and t1.actualcompletedquantity<>0 and t1.accountingstate=0) and rownum=1";
            command.Parameters.Add(new OracleParameter("ProjectId", sProjectId));
            IDataReader reader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(reader);
            return (ds!=null && ds.Tables.Count>0&& ds.Tables[0].Rows.Count>0);
        }
    }
}