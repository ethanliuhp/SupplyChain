using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Service
{
    /// <summary>
    /// 需求总计划服务
    /// </summary>
    public class DelimitIndividualBillSrv : BaseService, IDelimitIndividualBillSrv
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

        #region 需求总计划
        /// <summary>
        /// 通过ID查询个人借款单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DelimitIndividualBill GetDelimitIndividualBillById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetDelimitIndividualBill(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DelimitIndividualBill;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询个人借款单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DelimitIndividualBill GetDelimitIndividualBillByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetDelimitIndividualBill(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DelimitIndividualBill;
            }
            return null;
        }

        /// <summary>
        /// 查询个人借款单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetDelimitIndividualBill(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(DelimitIndividualBill), objectQuery);
        }

        /// <summary>
        /// 个人借款单信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet DelimitIndividualBillQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT * from thd_DelimitIndividualBill";
            sql += " where 1=1 " + condition + " order by code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public DelimitIndividualBill SaveDelimitIndividualBill(DelimitIndividualBill obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(DelimitIndividualBill));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as DelimitIndividualBill;
        }

        #endregion
    }
}
