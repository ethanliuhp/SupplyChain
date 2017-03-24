using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockInventory.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInventory.Service
{
   

    /// <summary>
    /// 月度盘点服务
    /// </summary>
    public class StockInventorySrv : BaseService, IStockInventorySrv
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
        private string GetCode(Type type, string specail)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, specail);
        }
        /// <summary>
        /// 根据项目 物资分类(专业分类) 生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <param name="matCatAbb"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId, string matCatAbb)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId, matCatAbb);
        }
        #endregion

        #region 月度盘点
        /// <summary>
        /// 通过ID查询月度盘点信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StockInventoryMaster GetStockInventoryById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("UsedRank", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("UserPart", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("UsedRank.SupplierInfo", NHibernate.FetchMode.Eager);
            IList list = GetStockInventory(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockInventoryMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询月度盘点信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public StockInventoryMaster GetStockInventoryByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetStockInventory(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockInventoryMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询废旧物料申请信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetStockInventory(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockInventoryMaster), objectQuery);
        }


        /// <summary>
        /// 废旧物料申请信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet StockInventoryQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.CreateDate,t1.CreatePersonName,t1.Descript,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,
                t2.MatStandardUnit,t2.MatStandardUnitName,t2.StockQuantity,t2.SpecialType,t1.Code,t1.State,t2.DiagramNumber
                FROM THD_StockInventoryMaster t1 INNER JOIN THD_StockInventoryDetail t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        //[TransManager]
        //public StockInventoryMaster SaveStockInventory(StockInventoryMaster obj)
        //{
        //    if (obj.Id == null)
        //    {
        //        obj.Code = GetCode(typeof(StockInventoryMaster));
        //    }
        //    obj.LastModifyDate = DateTime.Now;
        //    return SaveOrUpdateByDao(obj) as StockInventoryMaster;
        //}
    


        [TransManager]
        public StockInventoryMaster SaveStockInventory(StockInventoryMaster obj, IList movedDtlList)
        {
            if (obj.Id == null)
            {
                if (obj.MaterialCategory != null && obj.MaterialCategory.Abbreviation != null)
                {
                    obj.Code = GetCode(typeof(StockInventoryMaster), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else
                {
                    obj.Code = GetCode(typeof(StockInventoryMaster), obj.ProjectId );
                }
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as StockInventoryMaster;
        }
        #endregion
        /// <summary>
        /// 获取安装的月度盘点报表数据
        /// </summary>
        /// <param name="sProfessionCategory">专业分类</param>
        /// <param name="sProjectId">项目ID</param>
        /// <param name="sUsedRankId">劳务队伍ID</param>
        /// <returns></returns>
        public DataTable GetInventoryReport(string sProfessionCategory, string sProjectId, string sUsedRankId,int iYear,int iMonth)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sSQL = " select t.materialname,t.MATERIALSPEC, t.MATSTANDARDUNITNAME，t.diagramnumber ,t.INVENTORYQUANTITY,t.confirmprice,t.confirmmoney,t.price,t.money,t.descript,t1.createdate  from thd_stockinventorydetail t join thd_stockinventorymaster t1 on  t1.special='安装'   {0}    and t1.projectid='{1}' and  t1.id=t.parentid   {2}  and t1.createyear={3} and  t1.createmonth={4}";
            if (!string.IsNullOrEmpty(sProfessionCategory))
            {
                sProfessionCategory = string.Format("  and t1.professioncategory='{0}'  ", sProfessionCategory);
            }
            if (!string.IsNullOrEmpty(sUsedRankId))
            {
                sUsedRankId = string.Format (" and t1.usedrankid='{0}' ", sUsedRankId);
            }
            sSQL = string.Format(sSQL, sProfessionCategory, sProjectId, sUsedRankId, iYear, iMonth);
            command.CommandText = sSQL;

            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return dataSet.Tables[0];
            }
            return null;
            
        }

        public DataTable GetAvgPrice(string sMaterialID, string sProjectID)
        {
            DataTable oTable = null;

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sSQL = @" select nvl(round(decode(quantity,0,0,money/quantity),6),0) price,nvl(round(decode(quantity,0,0,confirmmoney/quantity),6),0)    confirmprice
from (select sum(t1.quantity) quantity ,sum(t1.money) money,sum(t1.confirmmoney)confirmmoney from thd_stkstockout t
join thd_stkstockoutdtl t1 on t.id=t1.parentid and  t1.material='{0}' and   t.projectid='{1}')";
            sSQL = string.Format(sSQL, sMaterialID, sProjectID );
            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                oTable=dataSet.Tables[0];
            }
            return oTable;
        }
        public DataTable GetSubject(string sMaterialID, string sProjectID)
        {
            DataTable oTable = null;
            DataRow oRow = null;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sSQL = @" select subjectguid|| '-'||subjectsyscode id, subjectname name from (select t1.subjectguid,t1.subjectname,t1.subjectsyscode,count(1) TotalTime from thd_stkstockoutdtl t1
join thd_stkstockout t on t.id=t1.parentid and  t1.material='{0}' and   t.projectid='{1}' and t1.subjectguid is not null   group by t1.subjectguid,t1.subjectname,t1.subjectsyscode order by TotalTime desc)";
            sSQL = string.Format(sSQL, sMaterialID, sProjectID);
            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                oTable = dataSet.Tables[0];
                 
            }

            return oTable;
        }
    }





}
