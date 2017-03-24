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
    /// 费用划账单
    /// </summary>
    public class ExpensesRowBillSrv : BaseService, IExpensesRowBillSrv
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

        #region 费用划账单
        /// <summary>
        /// 通过ID查询费用划账单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExpensesRowBill GetExpensesRowBillById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetExpensesRowBill(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ExpensesRowBill;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询费用划账单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ExpensesRowBill GetExpensesRowBillByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetExpensesRowBill(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ExpensesRowBill;
            }
            return null;
        }

        /// <summary>
        /// 查询费用划账单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetExpensesRowBill(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(ExpensesRowBill), objectQuery);
        }

        /// <summary>
        /// 费用划账单信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet ExpensesRowBillQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT * from thd_ExpensesRowBill";
            sql += " where 1=1 " + condition + " order by code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public ExpensesRowBill SaveExpensesRowBill(ExpensesRowBill obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ExpensesRowBill));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as ExpensesRowBill;
        }

        #endregion
    }
}
