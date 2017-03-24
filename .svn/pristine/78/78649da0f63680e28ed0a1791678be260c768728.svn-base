using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonSearchLib.BillCodeMng.Service;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Data;
using VirtualMachine.Core.DataAccess;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Service
{
    /// <summary>
    /// 目标责任书服务
    /// </summary>
    public class TargetRespBookSrc : BaseService, ITargetRespBookSrc
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
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
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument SaveOrUpdate

(ProObjectRelaDocument item)
        {
            dao.SaveOrUpdate(item);

            return item;
        }


        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }
        #endregion

        /// <summary>
        /// 通过ID目标责任书的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TargetRespBook GetTargetRespBookById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetTargetRespBook(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as TargetRespBook;
            }
            return null;
        }

        /// <summary>
        /// 查询目标责任书信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetTargetRespBook(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("NodeDetails", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("RecordDetails", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(TargetRespBook), objectQuery);
        }

        /// <summary>
        /// 查询目标责任书信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetPersonOnJob(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("StandardPerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(PersonOnJob), objectQuery);
        }

        /// <summary>
        /// 通过Code查询目标责任书信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public TargetRespBook GetWTargetRespBookByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetTargetRespBook(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as TargetRespBook;
            }
            return null;
        }

        /// <summary>
        /// 保存目标责任书信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public TargetRespBook saveTargetRespBook(TargetRespBook obj)
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
            //obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as TargetRespBook;
        }

        /// <summary>
        /// 查询目标节点信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetTargetProgressNode(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(TargetProgressNode), objectQuery);
        }

        /// <summary>
        /// 查询风险抵押金缴纳记录信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetIrpRiskDepositPayRecord(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(IrpRiskDepositPayRecord), objectQuery);
        }

        public DataSet PersonInfoQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select t1.pername, t3.opjname, t4.opgname, t5.projectname from resperson t1 
                              join respersononjob t2 on t1.perid = t2.perid 
                              join resoperationjob t3 on t2.operationjobid = t3.opjid
                              join resoperationorg t4 on t3.opjorgid = t4.opgid
                              join resconfig t5 on t4.opgid = t5.ownerorg";
            sql += " where 1=1 " + condition;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        public DataSet GetProjectInfo(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select t1.Id,t1.ProjectName from ResConfig t1";
            sql += " where 1=1 " + condition;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
    }
}
