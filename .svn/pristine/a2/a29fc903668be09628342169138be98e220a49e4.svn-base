using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.CostManagement.EngineerChangeMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;

namespace Application.Business.Erp.SupplyChain.CostManagement.Service
{
   

    /// <summary>
    /// 工程更改管理
    /// </summary>
    public class EngineerChangeSrv : BaseService, IEngineerChangeSrv
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

        #region 工程更改管理
        /// <summary>
        /// 通过ID查询工程更改管理信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EngineerChangeMaster GetEngineerChangeById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetEngineerChange(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as EngineerChangeMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询工程更改管理信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public EngineerChangeMaster GetEngineerChangeByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetEngineerChange(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as EngineerChangeMaster;
            }
            return null;
        }

        /// <summary>
        /// 工程更改管理信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetEngineerChange(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(EngineerChangeMaster), objectQuery);
        }

        /// <summary>
        /// 工程更改管理信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet EngineerChangeQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.Code,t1.CreateDate,t1.CreatePersonName,t2.Descript,t2.ChangeHandlePersonName,t1.ContractGroupName
                ,t2.EngineerChangeLink,t2.ChangeDescript,t2.ComplateDate
                FROM THD_EngineerChangeMaster t1 INNER JOIN THD_EngineerChangeDetail t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public EngineerChangeMaster SaveEngineerChange(EngineerChangeMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(EngineerChangeMaster));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as EngineerChangeMaster;
        }
        #endregion
    }





}
