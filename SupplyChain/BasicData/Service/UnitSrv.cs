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
using Application.Business.Erp.SupplyChain.BasicData.UnitMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.BasicData.Service
{


    /// <summary>
    /// 计量单位服务
    /// </summary>
    public class UnitSrv : BaseService, IUnitSrv
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

        #region 计量单位
        /// <summary>
        /// 通过ID查询计量单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UnitMaster GetUnitById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetUnit(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as UnitMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过ID查询计量单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UnitMaster GetUnitBillTypeNameById(string BillName)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("BillName", BillName));
            IList list = GetUnit(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as UnitMaster;
            }
            return null;
        }



        /// <summary>
        /// 通过Code查询计量单位信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public UnitMaster GetUnitByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetUnit(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as UnitMaster;
            }
            return null;
        }

        /// <summary>
        /// 计量单位查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetUnit(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(UnitMaster), objectQuery);
        }

        /// <summary>
        /// 计量单位信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet UnitQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select * from THD_UnitMaster";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public UnitMaster SaveUnit(UnitMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(UnitMaster));
            }
            obj.RealOperationDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as UnitMaster;
        }


        #endregion
    }





}
