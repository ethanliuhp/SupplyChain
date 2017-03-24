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
    /// 临建摊销单服务
    /// </summary>
    public class OverlayAmortizeSrv : BaseService, IOverlayAmortizeSrv
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

        #region 临建摊销单
        /// <summary>
        /// 通过ID查询临建摊销单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OverlayAmortizeMaster GetOverlayAmortizeMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetOverlayAmortizeMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as OverlayAmortizeMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询临建摊销单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OverlayAmortizeMaster GetOverlayAmortizeMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetOverlayAmortizeMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as OverlayAmortizeMaster;
            }
            return null;
        }

        /// <summary>
        /// 临建摊销单信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOverlayAmortizeMaster(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(OverlayAmortizeMaster), objectQuery);
        }

        /// <summary>
        /// 临建摊销单信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet OverlayAmortizeMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.Code,t1.State,t1.CreateDate,t1.CreatePersonName,t2.PeriodAmortize,t2.TotalAmortize,t2.OverlayValue,t2.OverlayProject,t2.AmortizeTime,t2.Descript,t2.OrgValue FROM THD_OverlayAmortizeMaster t1 INNER JOIN THD_OverlayAmortizeDetail t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public OverlayAmortizeMaster SaveOverlayAmortizeMaster(OverlayAmortizeMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(OverlayAmortizeMaster));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as OverlayAmortizeMaster;
        }


        /// <summary>
        /// 根据明细Id查询临建摊销单明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public OverlayAmortizeDetail GetOverlayAmortizeDetailById(string DemandDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", DemandDtlId));
            IList list = dao.ObjectQuery(typeof(OverlayAmortizeDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as OverlayAmortizeDetail;
            }
            return null;
        }

        #endregion
    }
}
