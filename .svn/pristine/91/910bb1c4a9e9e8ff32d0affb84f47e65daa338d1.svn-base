using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.SupplyManage.ContractAdjustPriceManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
   

    /// <summary>
    /// 合同调价单
    /// </summary>
    public class ContractAdjustPriceSrv : BaseService, IContractAdjustPriceSrv
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

        #region 合同调价单
        /// <summary>
        /// 通过ID查询合同调价单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContractAdjustPrice GetContractAdjustPriceById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetContractAdjustPrice(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ContractAdjustPrice;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询合同调价单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ContractAdjustPrice GetContractAdjustPriceByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetContractAdjustPrice(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ContractAdjustPrice;
            }
            return null;
        }

        /// <summary>
        /// 合同调价单信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetContractAdjustPrice(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ContractAdjustPrice), objectQuery);
        }

        /// <summary>
        /// 合同调价单信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet ContractAdjustPriceQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.code,t1.State,t1.Supplierrelation,t1.SupplierName,t2.Material,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,t2.id detailId,t2.ModifyPrice
                ,t2.Quantity,t2.Money,t2.SupplyPrice,t2.ContractAdjustPrice,t2.DiagramNumber FROM THD_SupplyOrderMaster t1 INNER JOIN THD_SupplyOrderDetail t2
                ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        //public int SubmitContractAdjustPrice(string forwordId,Decimal ContractPrice,string ID)
        //{
        //    ISession session = CallContext.GetData("nhsession") as ISession;
        //    IDbConnection conn = session.Connection;
        //    IDbCommand command = conn.CreateCommand();
        //    string sql =
        //        @"UPDATE THD_SupplyOrderDetail SET ForwardDetailId = '"+ forwordId +"' and ContractAdjustPrice = '"+ ContractPrice +"' where Id = '"+ ID +"'";
           
        //    command.CommandText = sql;

        //    int res = command.ExecuteNonQuery();
        //    return res;
        //    //IDataReader dataReader = command.ExecuteReader();
        //    //DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
        //}

        public DataSet SelectContractAdjustPrice(string ContractNo,string MaterialCode)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT * FROM THD_ContractAdjustPrice where SupplyOrderCode = '"+ ContractNo +"' and  MaterialCode = '"+ MaterialCode +"' order by code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        [TransManager]
        public ContractAdjustPrice SaveContractAdjustPrice(ContractAdjustPrice obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ContractAdjustPrice));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as ContractAdjustPrice;
        }
        //[TransManager]
        //public ContractAdjustPrice UpdateContractAdjustPrice(ContractAdjustPrice obj)
        //{
        //    obj.Code = GetCode(typeof(ContractAdjustPrice));
        //    obj.LastModifyDate = DateTime.Now;
        //    return SaveByDao(obj) as ContractAdjustPrice;
        //}

        public SupplyOrderDetail GetSupplyOrderDetail(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id",id));
            IList list = dao.ObjectQuery(typeof(SupplyOrderDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as SupplyOrderDetail;
            }
            return null;
        }

        [TransManager]
        public ContractAdjustPrice saveaa(ContractAdjustPrice master, SupplyOrderDetail supplyOrderDetail)
        {
            UpdateByDao(supplyOrderDetail);
            return UpdateByDao(master) as ContractAdjustPrice;
        }

        #endregion
    }





}
