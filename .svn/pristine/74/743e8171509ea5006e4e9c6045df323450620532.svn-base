using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.CompleteSettlementBook.Domain;
using CommonSearchLib.BillCodeMng.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;


namespace Application.Business.Erp.SupplyChain.CompleteSettlementBook.Service
{
   public class CompleteSrv:BaseService,IComplete
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




        #region 竣工表信息
        /// <summary>
        /// 通过ID查询竣工表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompleteInfo GetCompleteById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetComplete(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as CompleteInfo;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询竣工表信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public CompleteInfo GetCompleteByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetComplete(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as CompleteInfo;
            }
            return null;
        }
       /// <summary>
       /// 查询竣工信息
       /// </summary>
       /// <param name="objectQuery"></param>
       /// <returns></returns>
        public IList GetComplete(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Person", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(CompleteInfo), objectQuery);
        }

        /// <summary>
        /// 查询竣工信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        //public IList GetWeather(ObjectQuery objectQuery)
        //{
        //    return Dao.ObjectQuery(typeof(WeatherInfo), objectQuery);
        //}

        [TransManager]
        public CompleteInfo SaveComplete(CompleteInfo obj)
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
            return SaveOrUpdateByDao(obj) as CompleteInfo;
        }
        #endregion
        /// <summary>
        /// 竣工结算查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet CompleteRelationQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql = @"select *  from thd_completeinfo ";
            sql += "where 1=1 "+ condition +"";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
    }
}
