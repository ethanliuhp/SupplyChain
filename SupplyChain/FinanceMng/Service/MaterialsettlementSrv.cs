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
    /// 材料结算单服务
    /// </summary>
    public class MaterialSettlementSrv : BaseService, IMaterialSettlementSrv
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

        #region 材料结算单
        /// <summary>
        /// 通过ID查询材料结算单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MaterialSettlementMaster GetMaterialSettlementMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetMaterialSettlementMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialSettlementMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询材料结算单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MaterialSettlementMaster GetMaterialSettlementMasterByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMaterialSettlementMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialSettlementMaster;
            }
            return null;
        }

        /// <summary>
        /// 材料结算单信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMaterialSettlementMaster(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MaterialSettlementMaster), objectQuery);
        }

        /// <summary>
        /// 材料结算单信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet MaterialSettlementMasterQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.Code,t1.State,t1.CreateDate,t1.CreatePersonName,t2.MaterialCode,t2.MaterialName,t1.SupplierName,
                t2.MaterialSpec,t2.MatStandardUnitName,t2.ContainTaxPrice,t2.ContainTaxMoney,t2.Quantity
                FROM THD_MaterialSettlementMaster t1 INNER JOIN THD_MaterialSettlementDetail t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public MaterialSettlementMaster SaveMaterialSettlementMaster(MaterialSettlementMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(MaterialSettlementMaster));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as MaterialSettlementMaster;
        }

        /// <summary>
        /// 根据明细Id查询材料结算单明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public MaterialSettlementDetail GetMaterialSettlementDetailById(string DemandDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", DemandDtlId));
            IList list = dao.ObjectQuery(typeof(MaterialSettlementDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as MaterialSettlementDetail;
            }
            return null;
        }
        #endregion
    }
}
