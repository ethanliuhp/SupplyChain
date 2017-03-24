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
    /// 当期收益结算单服务
    /// </summary>
    public class IncomeSettlementSrv : BaseService, IIncomeSettlementSrv
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

        #region 当期收益结算单
        /// <summary>
        /// 通过ID查询当期收益结算单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IncomeSettlementMaster GetIncomeSettlementMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetIncomeSettlementMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as IncomeSettlementMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询当期收益结算单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IncomeSettlementMaster GetIncomeSettlementMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetIncomeSettlementMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as IncomeSettlementMaster;
            }
            return null;
        }

        /// <summary>
        ///当期收益结算单信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetIncomeSettlementMaster(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(IncomeSettlementMaster), objectQuery);
        }

        /// <summary>
        /// 当期收益结算单信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet IncomeSettlementMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.Code,t1.State,t1.CreateDate,t1.CreatePersonName,t2.IncomeProject,t2.IncomeCategories,t2.Money,t2.Descript,t1.HandlePersonName,t1.RealOperationDate FROM THD_IncomeSettlementMaster t1 INNER JOIN THD_IncomeSettlementDetail t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public IncomeSettlementMaster SaveIncomeSettlementMaster(IncomeSettlementMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(IncomeSettlementMaster));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as IncomeSettlementMaster;
        }


        /// <summary>
        /// 根据明细Id查询当期收益结算单明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public IncomeSettlementDetail GetIncomeSettlementDetailById(string DemandDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", DemandDtlId));
            IList list = dao.ObjectQuery(typeof(IncomeSettlementDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as IncomeSettlementDetail;
            }
            return null;
        }

        #endregion
    }





}
