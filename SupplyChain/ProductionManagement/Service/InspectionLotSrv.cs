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
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
    /// <summary>
    /// 检验批服务
    /// </summary>
    public class InspectionLotSrv : BaseService, IInspectionLotSrv
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

        #region 检验批
        /// <summary>
        /// 通过ID查询检验批信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InspectionLot GetInspectionLotById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetInspectionLot(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as InspectionLot;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询检验批信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public InspectionLot GetInspectionLotByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetInspectionLot(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as InspectionLot;
            }
            return null;
        }

        /// <summary>
        /// 检验批查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetInspectionLot(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(InspectionLot), objectQuery);
        }

        /// <summary>
        /// 检验批查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet InspectionLotQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT * FROM THD_InspectionLot t1 ";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public InspectionLot SaveInspectionLot(InspectionLot obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(InspectionLot));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as InspectionLot;
        }
        #endregion
    }





}
