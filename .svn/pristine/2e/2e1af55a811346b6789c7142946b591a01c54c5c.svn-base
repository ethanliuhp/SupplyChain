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
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

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

        /// <summary>
        /// 保存或修改工程任务核算单
        /// </summary>
        /// <param name="bill">工程任务核算单</param>
        /// <returns></returns>
        [TransManager]
        public ProjectTaskAccountBill SaveOrUpdateProjectTaskAccount(ProjectTaskAccountBill bill)
        {
            dao.SaveOrUpdate(bill);
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
            //回写确认明细中核算信息
            IList listUpdateConfirmDetail = new ArrayList();
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

            listUpdateConfirmDetail = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
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
            dao.Save(bill);

            IEnumerable<GWBSTaskConfirm> listTempConfirmDetail = listConfirmDetail.OfType<GWBSTaskConfirm>();

            IList listUpdateConfirmDetail = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = null;

            foreach (ProjectTaskDetailAccount dtl in bill.Details)
            {
                var query = from d in listTempConfirmDetail
                            where d.Master.ConfirmHandlePerson == dtl.ResponsiblePerson
                            && d.Master.SubContractProject == dtl.BearerGUID
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
    }
}
