using System;
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
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Service
{
    /// <summary>
    /// 分包结算单服务
    /// </summary>
    public class SubContractBalanceBillSrv : ISubContractBalanceBillSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

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

        /// <summary>
        /// 保存分包结算单
        /// </summary>
        /// <param name="bill">分包结算单</param>
        /// <param name="listAccountDetail">结算的核算明细</param>
        /// <param name="listPenalty">结算的罚扣款单</param>
        /// <param name="listLaborSporadic">结算的零星用工单</param>
        /// <returns></returns>
        [TransManager]
        public SubContractBalanceBill SaveSubContractBalanceBill(SubContractBalanceBill bill, IList listAccountDetail, IList listPenalty, IList listLaborSporadic)
        {

            dao.Save(bill);


            foreach (SubContractBalanceDetail dtl in bill.ListDetails)
            {
                if (dtl.FontBillType == FrontBillType.工程任务核算明细)
                {
                    for (int i = 0; i < listAccountDetail.Count; i++)
                    {
                        ProjectTaskDetailAccount item = listAccountDetail[i] as ProjectTaskDetailAccount;

                        if (item.Id == dtl.FrontBillGUID)
                        {

                            //if (item.ProjectTaskDtlGUID != null && dtl.BalanceTaskDtl != null && item.ProjectTaskDtlGUID.Id == dtl.BalanceTaskDtl.Id)
                            //{
                            //item = dao.Get(typeof(ProjectTaskDetailAccount), item.Id) as ProjectTaskDetailAccount;

                            item.BalanceState = ProjectTaskDetailAccountState.已结算;
                            item.BalanceDtlGUID = dtl.Id;

                            listAccountDetail[i] = item;
                            break;
                        }
                    }
                }
                else if (dtl.FontBillType == FrontBillType.罚扣款单明细)
                {
                    for (int i = 0; i < listPenalty.Count; i++)
                    {
                        PenaltyDeductionDetail item = listPenalty[i] as PenaltyDeductionDetail;
                        if (item.Id == dtl.FrontBillGUID)
                        {

                            //if (item.ProjectTaskDetail != null && dtl.BalanceTaskDtl != null && item.ProjectTaskDetail.Id == dtl.BalanceTaskDtl.Id)
                            //{
                            //    item = dao.Get(typeof(PenaltyDeductionDetail), item.Id) as PenaltyDeductionDetail;

                            item.AccountState = EnumSettlementType.已结算;

                            item.BalanceDtlGUID = dtl.Id;

                            listPenalty[i] = item;
                            break;
                        }
                    }
                }
                else if (dtl.FontBillType == FrontBillType.零星用工明细)
                {
                    for (int i = 0; i < listLaborSporadic.Count; i++)
                    {
                        LaborSporadicDetail item = listLaborSporadic[i] as LaborSporadicDetail;

                        if (item.Id == dtl.FrontBillGUID)
                        {
                            //if (item.ProjectTastDetail != null && dtl.BalanceTaskDtl != null && item.ProjectTastDetail.Id == dtl.BalanceTaskDtl.Id)
                            //{
                            //    item = dao.Get(typeof(LaborSporadicDetail), item.Id) as LaborSporadicDetail;

                            item.SettlementState = EnumSettlementType.已结算;

                            item.BalanceDtlGUID = dtl.Id;

                            listLaborSporadic[i] = item;
                            break;
                        }
                    }
                }
            }

            dao.Update(listAccountDetail);
            dao.Update(listPenalty);
            dao.Update(listLaborSporadic);

            return bill;
        }

        /// <summary>
        /// 删除分包结算单集合
        /// </summary>
        /// <param name="list">分包结算单集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteSubContractBalanceBill(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis1 = new Disjunction();

            for (int i = 0; i < list.Count; i++)
            {
                SubContractBalanceBill bill = list[i] as SubContractBalanceBill;
                if (!string.IsNullOrEmpty(bill.Id))
                {
                    oq.AddCriterion(Expression.Eq("Id", bill.Id));
                    oq.AddFetchMode("ListDetails", FetchMode.Eager);

                    IList listTemp = dao.ObjectQuery(typeof(SubContractBalanceBill), oq);
                    bill = listTemp[0] as SubContractBalanceBill;

                    foreach (SubContractBalanceDetail item in bill.ListDetails)
                    {
                        dis1.Add(Expression.Eq("BalanceDtlGUID", item.Id));
                    }

                    list[i] = bill;
                }
            }

            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.Criterions.Add(dis1);

            IList listAccountDetail = dao.ObjectQuery(typeof(ProjectTaskDetailAccount), oq);
            IList listPenalty = dao.ObjectQuery(typeof(PenaltyDeductionDetail), oq);
            IList listLaborSporadic = dao.ObjectQuery(typeof(LaborSporadicDetail), oq);

            for (int i = 0; i < listAccountDetail.Count; i++)
            {
                ProjectTaskDetailAccount item = listAccountDetail[i] as ProjectTaskDetailAccount;
                item.BalanceState = ProjectTaskDetailAccountState.未结算;
                item.BalanceDtlGUID = null;
                listAccountDetail[i] = item;
            }
            for (int i = 0; i < listPenalty.Count; i++)
            {
                PenaltyDeductionDetail item = listPenalty[i] as PenaltyDeductionDetail;
                item.AccountState = EnumSettlementType.未结算;
                item.BalanceDtlGUID = null;
                listPenalty[i] = item;
            }
            for (int i = 0; i < listLaborSporadic.Count; i++)
            {
                LaborSporadicDetail item = listLaborSporadic[i] as LaborSporadicDetail;
                item.SettlementState = EnumSettlementType.未结算;
                item.BalanceDtlGUID = null;
                listLaborSporadic[i] = item;
            }

            dao.Update(listAccountDetail);
            dao.Update(listPenalty);
            dao.Update(listLaborSporadic);

            dao.Delete(list);

            return true;
        }

        /// <summary>
        /// 删除分包结算单明细集合
        /// </summary>
        /// <param name="list">分包结算单明细集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteSubContractBalanceBillDetail(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis1 = new Disjunction();

            for (int i = 0; i < list.Count; i++)
            {
                SubContractBalanceDetail dtl = list[i] as SubContractBalanceDetail;
                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    dtl = dao.Get(typeof(SubContractBalanceDetail), dtl.Id) as SubContractBalanceDetail;

                    dis1.Add(Expression.Eq("BalanceDtlGUID", dtl.Id));

                    list[i] = dtl;
                }
            }

            oq.Criterions.Add(dis1);
            IList listAccountDetail = dao.ObjectQuery(typeof(ProjectTaskDetailAccount), oq);
            IList listPenalty = dao.ObjectQuery(typeof(PenaltyDeductionDetail), oq);
            IList listLaborSporadic = dao.ObjectQuery(typeof(LaborSporadicDetail), oq);

            for (int i = 0; i < listAccountDetail.Count; i++)
            {
                ProjectTaskDetailAccount item = listAccountDetail[i] as ProjectTaskDetailAccount;
                item.BalanceState = ProjectTaskDetailAccountState.未结算;
                item.BalanceDtlGUID = null;
                listAccountDetail[i] = item;
            }
            for (int i = 0; i < listPenalty.Count; i++)
            {
                PenaltyDeductionDetail item = listPenalty[i] as PenaltyDeductionDetail;
                item.AccountState = EnumSettlementType.未结算;
                item.BalanceDtlGUID = null;
                listPenalty[i] = item;
            }
            for (int i = 0; i < listLaborSporadic.Count; i++)
            {
                LaborSporadicDetail item = listLaborSporadic[i] as LaborSporadicDetail;
                item.SettlementState = EnumSettlementType.未结算;
                item.BalanceDtlGUID = null;
                listLaborSporadic[i] = item;
            }

            dao.Update(listAccountDetail);
            dao.Update(listPenalty);
            dao.Update(listLaborSporadic);

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

    }
}
