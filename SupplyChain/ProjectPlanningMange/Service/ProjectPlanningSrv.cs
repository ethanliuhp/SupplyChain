using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.ProjectPlanningMange.Domain;
using System.Collections;
using CommonSearchLib.BillCodeMng.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.ProjectPlanningMange.Service
{
    /// <summary>
    /// 项目专业策划
    /// </summary>
    public class ProjectPlanningSrv : BaseService, IProjectPlanningSrv
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
        /// 通过ID查询项目专业策划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SpecialityProposal GetSpecialityProposalById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetSpecialityProposal(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SpecialityProposal;
            }
            return null;
        }

        /// <summary>
        /// 项目专业策划信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetSpecialityProposal(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(SpecialityProposal), objectQuery);
        }

        [TransManager]
        public SpecialityProposal SaveSpecialityProposal(SpecialityProposal obj)
        {
            if (obj.Id == null)
            {
                ///obj.Code = GetCode(typeof(MonthlyPlanMaster), obj.Special);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as SpecialityProposal;
        }

        /// <summary>
        /// 项目专业策划信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet SpecialityProposalQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.ProjectName,t1.EnginnerName,t1.DocState,t1.SubmitDate,t1.CreateDate,t1.CreatePersonName,t1.Descript,t1.EnginnerType,t1.RealOperationDate,
                t1.EvaluationWay, t1.PlanningLevel From Thd_SpecialityProposal t1";
            sql += " where 1=1 " + condition;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        /// <summary>
        /// 通过ID查询项目专业商务策划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BusinessProposal GetBusinessProposalById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.IrpDetails", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            IList list = GetBusinessProposal(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as BusinessProposal;
            }
            return null;
        }

        /// <summary>
        /// 商务策划信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetBusinessProposal(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.IrpDetails", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(BusinessProposal), objectQuery);
        }

        [TransManager]
        public BusinessProposal SaveBusinessProposal(BusinessProposal obj)
        {
            if (obj.Id == null)
            {
                ///obj.Code = GetCode(typeof(MonthlyPlanMaster), obj.Special);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            //obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as BusinessProposal;
        }

        /// <summary>
        /// 商务策划信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet BusinessProposalQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.EnginnerName,t1.DocState,t1.SubmitDate,t1.CreateDate,t1.RealOperationDate,t1.CreatePersonName,t1.Descript From Thd_BusinessProposal t1";
            sql += " where 1=1 " + condition;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 通过ID查询商务策划点信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BusinessProposalItem GetBusinessProposalItemById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetBusinessProposalItem(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as BusinessProposalItem;
            }
            return null;
        }

        /// <summary>
        /// 商务策划点信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetBusinessProposalItem(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(BusinessProposalItem), objectQuery);
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
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item)
        {
            dao.SaveOrUpdate(item);

            return item;
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

                IList listTemp = dao.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (listTemp != null && listTemp.Count > 0)
                    dao.Delete(listTemp);
            }

            return true;
        }
    }
}
