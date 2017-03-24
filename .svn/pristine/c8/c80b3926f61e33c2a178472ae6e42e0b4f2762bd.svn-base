using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.DetectionReceiptManage.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
namespace Application.Business.Erp.SupplyChain.StockManage.DetectionReceiptManage.Service
{
   

    /// <summary>
    /// 检测回执单服务
    /// </summary>
    public class DetectionReceiptSrv : BaseService, IDetectionReceiptSrv 
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

        #region 检测回执单
        /// <summary>
        /// 通过ID查询检测回执单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetectionReceiptMaster GetDetectionReceiptById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetDetectionReceipt(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DetectionReceiptMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询检测回执单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DetectionReceiptMaster GetDetectionReceiptByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetDetectionReceipt(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DetectionReceiptMaster;
            }
            return null;
        }

        /// <summary>
        /// 检测回执单信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetDetectionReceipt(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(DetectionReceiptMaster), objectQuery);
        }

        /// <summary>
        /// 检测回执单信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet DetectionReceiptQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.Code,t1.State,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,t2.MatStandardUnit,t1.CreatePersonName,t1.CreateDate, t2.DetectionQuantity,t1.Descript,t2.TastResult,t2.ManuFacturer,t2.SupplyUnit,t2.SupplyUnitName,t2.AppearanceTast,t2.ComeDate FROM THD_DetectionReceiptMaster t1 INNER JOIN THD_DetectionReceiptDetail t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public DetectionReceiptMaster SaveDetectionReceipt(DetectionReceiptMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(DetectionReceiptMaster));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as DetectionReceiptMaster;
        }


        ///// <summary>
        ///// 根据明细Id查询检测回执单明细
        ///// </summary>
        ///// <param name="stockOutDtlId"></param>
        ///// <returns></returns>
        //public DemandMasterPlanDetail GetDemandDetailById(string DemandDtlId)
        //{
        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(Expression.Eq("Id", DemandDtlId));
        //    IList list = dao.ObjectQuery(typeof(DemandMasterPlanDetail), oq);
        //    if (list != null && list.Count > 0)
        //    {
        //        return list[0] as DemandMasterPlanDetail;
        //    }
        //    return null;
        //}

        #endregion
    }





}
