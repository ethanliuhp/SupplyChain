using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using NHibernate.Exceptions;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.MaterialResource.Service;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Resource.CommonClass.Attributes;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using System.Data.SqlClient;

namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Service
{
    public class StockRelationSrv : BaseService, IStockRelationSrv
    {
        /// <summary>
        /// 根据资源查询库存
        /// </summary>
        /// <param name="MaterialId"></param>
        /// <returns></returns>
        public IList GetStockRelationByMaterial(string MaterialId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Material", MaterialId));
            oq.AddCriterion(Expression.Gt("RemainQuantity", 0M));
            oq.AddOrder(new Order("SeqCreateDate", true));
            return Dao.ObjectQuery(typeof(StockRelation), oq);
        }

        /// <summary>
        /// 根据资源、专业、项目查询库存
        /// </summary>
        /// <param name="MaterialId"></param>
        /// <param name="special"></param>
        /// <returns></returns>
        public IList GetStockRelationByMaterialAndSpecial(string MaterialId, string special,string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Material", MaterialId));
            oq.AddCriterion(Expression.Gt("RemainQuantity", 0M));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            oq.AddOrder(new Order("SeqCreateDate", true));
            return Dao.ObjectQuery(typeof(StockRelation), oq);
        }

        public IList GetStockRelation(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(StockRelation), oq);
        }

        public IList GetStockRelationByStockInDtlId(string stockInDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("StockInDtlId", stockInDtlId));
            return GetStockRelation(oq);
        }

        public decimal GetRemainQuantityByStockInDtlId(string stockInDtlId)
        {
            IList list = GetStockRelationByStockInDtlId(stockInDtlId);
            if (list == null || list.Count == 0) return 0M;
            decimal quantity = 0M;
            foreach (StockRelation sr in list)
            {
                quantity += sr.RemainQuantity;
            }
            return quantity;
        }

        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet StockRelationQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql = @"SELECT t1.Code,t2.material MaterialID,t3.diagramnumber，t2.OriginalContractNo,t1.SupplierRelationName,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,
                t3.RemainQuantity,t3.Price,t3.RemainMoney,t1.CreateDate,t2.MatStandardUnitName,t2.ProfessionalCategory,t1.MoveOutProjectName,
                t1.StockInManner,t3.IdleQuantity
                FROM THD_StkStockRelation t3 inner join THD_StkStockIn t1 ON t3.StockInId=t1.id
                INNER JOIN THD_StkStockInDtl t2 ON t3.StockInDtlId=t2.id
                WHERE 1=1 AND t3.RemainQuantity>0 and t1.IsTally=1 ";
            sql = sql + condition + " ORDER BY t1.StockInManner,t1.Code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 库存汇总查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet StockRelationSummary(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql = @"SELECT t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,sum(t3.RemainQuantity) as RemainQuantity,"+
                        " sum(t3.RemainMoney) as RemainMoney,t2.MatStandardUnitName,sum(t3.IdleQuantity) as IdleQuantity "+
                          " FROM THD_StkStockRelation t3 inner join THD_StkStockIn t1 ON t3.StockInId = t1.id "+
                          " INNER JOIN THD_StkStockInDtl t2 ON t3.StockInDtlId = t2.id WHERE 1 = 1 AND t3.RemainQuantity > 0 and t1.IsTally = 1";
            sql = sql + condition + " group by t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,t2.ProfessionalCategory ";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        /// <summary>
        /// 库存汇总查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet StockKCQuantity(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql = @"SELECT sum(t3.RemainQuantity) as RemainQuantity,t2.MaterialCode FROM THD_StkStockRelation t3 inner join THD_StkStockIn t1 ON t3.StockInId = t1.id " +
                          " INNER JOIN THD_StkStockInDtl t2 ON t3.StockInDtlId = t2.id WHERE 1 = 1 AND t3.RemainQuantity > 0 and t1.IsTally = 1";
            sql = sql + condition + "group by t2.MaterialCode";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        public DataSet StockKCQuantity(int iYear, int iMonth,string sProjectID, string sMaterialCode)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sSQL = @"select MaterialCode,sum(RemainQuantity) RemainQuantity from (
                            select to_char(t1.materialcode) MaterialCode ,t1.inventoryquantity RemainQuantity from thd_stockinventorymaster t
                            join thd_stockinventorydetail t1 on t.id=t1.parentid   and t1.materialcode='{0}'
                            where  t.projectid='{1}' and t.createyear={2} and t.createmonth={3}
                            union all
                            select t1.materialcode,t1.QUANTITY from thd_stkstockout t 
                            join thd_stkstockoutdtl t1 on t.id=t1.parentid and t1.materialcode='{0}'
                            where  t.projectid='{1}' and t.createyear={2} and t.createmonth={3}
                            )group by MaterialCode";
            sSQL = string.Format(sSQL, sMaterialCode, sProjectID, iYear, iMonth);
            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        /// <summary>
        /// 库存查询(闲置物资设置)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet StockRelationQuery4SetIdleQuantity(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql = @"SELECT t3.id,t1.Code,t2.OriginalContractNo,t1.SupplierRelationName,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,
                t3.RemainQuantity,t3.Price,t3.RemainMoney,t1.CreateDate,t2.MatStandardUnitName,t2.ProfessionalCategory,t1.MoveOutProjectName,
                t1.StockInManner,t3.IdleQuantity,t3.ProjectName
                FROM THD_StkStockRelation t3 inner join THD_StkStockIn t1 ON t3.StockInId=t1.id
                INNER JOIN THD_StkStockInDtl t2 ON t3.StockInDtlId=t2.id
                WHERE 1=1 AND t3.RemainQuantity>0 and t1.IsTally=1 ";
            sql = sql + condition + " ORDER BY t1.StockInManner,t1.Code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 库存查询(闲置物资查询)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet StockRelationQuery4IdleQuantityQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql = @"SELECT t3.id,t1.Code,t2.OriginalContractNo,t1.SupplierRelationName,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,
                t3.RemainQuantity,t3.Price,t3.RemainMoney,t1.CreateDate,t2.MatStandardUnitName,t2.ProfessionalCategory,t1.MoveOutProjectName,
                t1.StockInManner,t3.IdleQuantity,t3.ProjectName
                FROM THD_StkStockRelation t3 inner join THD_StkStockIn t1 ON t3.StockInId=t1.id
                INNER JOIN THD_StkStockInDtl t2 ON t3.StockInDtlId=t2.id
                WHERE 1=1 AND t3.RemainQuantity>0 and t3.IdleQuantity>0 and t1.IsTally=1 ";
            sql = sql + condition + " ORDER BY t1.StockInManner,t1.Code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 设置闲置数量
        /// </summary>
        /// <param name="stockRelationId"></param>
        /// <param name="idleQuantity"></param>
        public void SetIdleQuantity(string stockRelationId, decimal idleQuantity)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;            
            IDbTransaction tx = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            command.Transaction = tx;
            string sql = "";
            if (conn is SqlConnection)
            {
                sql = "update THD_StkStockRelation set IdleQuantity=@IdleQuantity where id=@StockRelationId";
            } else
            {
                sql = "update THD_StkStockRelation set IdleQuantity=:IdleQuantity where id=:StockRelationId";
            }
            command.CommandText = sql;
            IDbDataParameter p_idleQuantity = command.CreateParameter();
            p_idleQuantity.Value = idleQuantity;
            p_idleQuantity.ParameterName = "IdleQuantity";
            command.Parameters.Add(p_idleQuantity);

            IDbDataParameter p_stockRelationId = command.CreateParameter();
            p_stockRelationId.Value = stockRelationId;
            p_stockRelationId.ParameterName = "StockRelationId";
            command.Parameters.Add(p_stockRelationId);

            try
            {
                command.ExecuteNonQuery();
                tx.Commit();
            } catch (Exception ex)
            {
                tx.Rollback();
                throw;
            }
        } 

        /// <summary>
        /// 仓库收发台帐
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet StockSequenceQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql = @"SELECT t1.CreateDate,t1.AccountYear,t1.AccountMonth,t1.BillType,t2.SupplierRelationName,t2.SupplyOrderCode,
                t3.MATCODE,t3.MATNAME,t3.MATSPECIFICATION,t1.Quantity,t4.STANDUNITNAME,t1.StockInOutManner
                FROM THD_StkStockSequence t1 left outer join thd_stkstockin t2 ON t1.BillId=t2.id
                LEFT JOIN ResMaterial t3 ON t1.Material=t3.MATERIALID
                LEFT JOIN RESSTANDUNIT t4 ON t3.StandardUnitID=t4.STANDUNITID ";
            sql = sql+" where 1=1 " + condition + " ORDER BY t1.CreateDate";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        //#region xl
        ///// <summary>
        ///// 根据入库明细的ID查找对应的关系表中库存大于零的记录
        ///// </summary>
        ///// <param name="sStockInDtlID"></param>
        ///// <returns></returns>
        //public IList GetStockRelationByStockInDtlID(string sStockInDtlID)
        //{
        //    ObjectQuery oQuery = new ObjectQuery();
        //    oQuery.AddCriterion(Expression.Eq ("StockInDtlId", sStockInDtlID));
        //    oQuery.AddCriterion(Expression.Gt("RemainQuantity",decimal .Parse ( "0")));
        //    return dao.ObjectQuery(typeof(StockRelation), oQuery);

        //}
        //#endregion
    }
}
