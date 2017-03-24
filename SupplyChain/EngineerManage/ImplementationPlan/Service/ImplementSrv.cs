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
using Application.Resource.CommonClass.Domain;
using Iesi.Collections;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.ImplementationPlan.Domain;
using CommonSearchLib.BillCodeMng.Service;

namespace Application.Business.Erp.SupplyChain.ImplementationPlan.Service
{
    public class ImplementSrv : BaseService, IImplementSrv
    {
   

        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }
        #endregion
        /// <summary>
        /// 通过ID查询项目实施策划书信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ImplementationMaintain GetImpById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetImp(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ImplementationMaintain;
            }
            return null;
        }


        /// <summary>
        /// 通过Code查询项目实施策划
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ImplementationMaintain GetImpByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetImp(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ImplementationMaintain;
            }
            return null;
        }
        /// <summary>
        /// 查询项目实施策划书信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetImp(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ImplementationMaintain), objectQuery);
        }
        [TransManager]
        public ImplementationMaintain saveImp(ImplementationMaintain obj)
        {
            if (obj.Id == null)
            {
                //obj.Code = GetCode(typeof(MonthlyPlanMaster), obj.Special);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            
            return SaveOrUpdateByDao(obj) as ImplementationMaintain;
        }
        public DataSet ImplementationQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.ProName,t1.FileName,t1.CreateDate ,t1.EnGoal,t1.CostObjective,t1.PeriodTarget,t1.ObjectiveName,t1.DofficerName,t1.RealOperationDate
                From thd_ImplementationPlanbook t1 INNER JOIN thd_Relevanproject t2 on t1.Id = t2.ParentId";
             sql += " where 1=1 " + condition;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            bool flag = false;
            foreach (ProObjectRelaDocument cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    dis.Add(Expression.Eq("Id", cg.Id));
                    flag = true;
                }
            }
            if (flag)
            {
                oq.AddCriterion(dis);

                IList listTemp = dao.ObjectQuery(typeof

(ProObjectRelaDocument), oq);
                if (listTemp != null && listTemp.Count > 0)
                    dao.Delete(listTemp);
            }

            return true;
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
        /// 保存或修改项目实施策划书
        /// </summary>
        /// <param name="item">项目实施策划书</param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument SaveOrUpdate

(ProObjectRelaDocument item)
        {
            dao.SaveOrUpdate(item);

            return item;
        }


    }
}
