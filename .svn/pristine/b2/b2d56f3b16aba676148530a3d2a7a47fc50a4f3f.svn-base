using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using System.Collections;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using NHibernate.Exceptions;
using NHibernate;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Data.Common;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Resource.CommonClass.Attributes;
using Application.Business.Erp.SupplyChain.Util;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.Component.Util;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Service;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Basic.Service;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Resource.FinancialResource.Domain;
using System.Linq;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Service;
using Application.Resource .PersonAndOrganization .HumanResource .RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;

 
namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service
{
    /// <summary>
    /// 入库单服务
    /// </summary>    
    [ClassDescription("入库单服务")]
    public class StockInSrv : BaseService, IStockInSrv
    {
        private IAppByBusinessSrv refAppByBusinessSrv;

        public IAppByBusinessSrv RefAppByBusinessSrv
        {
            get { return refAppByBusinessSrv; }
            set { refAppByBusinessSrv = value; }
        }
        private ISupplyOrderSrv supplyOrderSrv;

        public ISupplyOrderSrv SupplyOrderSrv
        {
            get { return supplyOrderSrv; }
            set { supplyOrderSrv = value; }
        }
        private IStockRelationSrv stockRelationSrv;
        public IStockRelationSrv StockRelationSrv
        {
            get { return stockRelationSrv; }
            set { stockRelationSrv = value; }
        }
        private IStockMoveSrv stockMoveSrv;
        public IStockMoveSrv StockMoveSrv
        {
            get { return stockMoveSrv; }
            set { stockMoveSrv = value; }
        }
        private ICurrentProjectSrv currentProjectSrv;

        public ICurrentProjectSrv CurrentProjectSrv
        {
            get { return currentProjectSrv; }
            set { currentProjectSrv = value; }
        }

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

        /// <summary>
        /// 根据项目生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
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
        /// <summary>
        /// 修改打印次数字段
        /// </summary>
        /// <param name="billType">单据类型[1:物资验收单 2:调拨入库单 3:调拨出库单 4:商品砼结算单 5:分包结算单 6:罚款单, 7:二维码, 8:付款单 ]</param>
        /// <returns></returns>
        public void UpdateBillPrintTimes(int billType, string billId)
        {
            string billTable = "";
            switch (billType)
            {
                case 1:
                    billTable = "thd_stockinbalmaster";
                    break;
                case 2:
                    billTable = "thd_stkstockin";
                    break;
                case 3:
                    billTable = "thd_stkstockout";
                    break;
                case 4:
                    billTable = "thd_concretebalancemaster";
                    break;
                case 5:
                    billTable = "thd_subcontractbalancebill";
                    break;
                case 6:
                    billTable = "thd_penaltydeductionmaster";
                    break;
                case 7:
                    billTable = "thd_qrcodebill";
                    break;
                case 8:
                    billTable = "thd_paymentmaster";
                    break;
            }
            if (billTable == "")
            {
                return;
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = " update " + billTable + " set printtimes = nvl(printtimes,0) + 1 where id = '" + billId + "'";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 查询打印次数字段
        /// </summary>
        /// <param name="billType">单据类型[1:物资验收单 2:调拨入库单 3:调拨出库单 4:商品砼结算单 5:分包结算单 6:罚款单 ]</param>
        /// <returns></returns>
        public int QueryBillPrintTimes(int billType, string billId)
        {
            int times = 0;
            string billTable = "";
            switch (billType)
            {
                case 1:
                    billTable = "thd_stockinbalmaster";
                    break;
                case 2:
                    billTable = "thd_stkstockin";
                    break;
                case 3:
                    billTable = "thd_stkstockout";
                    break;
                case 4:
                    billTable = "thd_concretebalancemaster";
                    break;
                case 5:
                    billTable = "thd_subcontractbalancebill";
                    break;
                case 6:
                    billTable = "thd_penaltydeductionmaster";
                    break;
            }
            if (billTable == "")
            {
                return -1;
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = " select nvl(printtimes,0) printtimes from " + billTable + " where id = '" + billId + "' ";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                {
                    times = TransUtil.ToInt(oDataRow["printtimes"]);
                }
            }
            return times;
        }

        #region 收料入库单方法

        public IList GetStockIn(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockIn), objectQuery);
        }


        public StockIn GetStockInById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockIn(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockIn;
            }
            return null;
        }

        public StockIn GetStockInByCode(string Code, string special, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", Code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockIn(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockIn;
            }
            return null;
        }
        public StockIn GetStockInByCode(string Code, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", Code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockIn(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockIn;
            }
            return null;
        }
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }


        /// <summary>
        /// 入库查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet StockInQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = @"SELECT t1.id,t1.Code,t1.SupplyOrderCode,t1.SupplierRelationName,t1.STATE,t1.createdate,t1.createpersonname,t1.PrintTimes,t1.Descript,
                            t1.StockInManner,t2.MaterialCode,t2.MaterialName,t1.TheStockInOutKind,t2.id DtlId,t2.DiagramNumber,
                            t2.MaterialSpec,t2.Quantity,t2.Refquantity,t2.Balquantity,t2.Price,t2.MONEY,t2.MatStandardUnitName,t2.ProfessionalCategory,t1.SumQuantity,t1.SumMoney，t2.confirmprice,t2.confirmmoney
                            FROM thd_stkstockin t1 inner join thd_stkstockindtl t2
                            ON t1.id=t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.Code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }

        /// <summary>
        /// 通过验收单明细的前驱获取验收单的验收单的id
        /// </summary>
        /// <param name="sStockInBalForworkID"></param>
        /// <returns></returns>
        public string GetStockInBalDtlID(string sStockInBalForworkID)
        {

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = @"select tt.id from thd_stockinbaldetail tt   ";
            sql += " where 1=1 " + string.Format(" and tt.forwarddetailid='{0}' ", sStockInBalForworkID);
            command.CommandText = sql;
            //dataReader = command.ExecuteReader(CommandBehavior.Default);
            //DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            string sStockInBalDtlID = command.ExecuteScalar() as string;

            return sStockInBalDtlID;
        }
        /// <summary>
        /// 通过验收单的前驱获取验收单的验收单
        /// </summary>
        /// <param name="sStockInBalForworkID"></param>
        /// <returns></returns>
        public DataSet GetStockInBalID(string sStockInBalForworkID)
        {

            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = @"select t1.id ,t1.code from thd_stockinbalmaster t1 join thd_stockinbaldetail t2 on t1.id=t2.parentid join thd_stkstockindtl t3 on t3.id=t2.forwarddetailid and t3.parentid='{0}'";
            sql = string.Format(sql, sStockInBalForworkID);
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }
        /// <summary>
        /// 根据条件统计入库单
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>查询结果</returns>
        [MethodDescription("根据条件统计入库单")]
        public DataSet StockInStateSearch(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            if (cnn is SqlConnection)
            {

            }
            IDbCommand command = cnn.CreateCommand();
            string strSQL = "select t1.rate 成材率,t2.id 明细ID,t2.Brand 品牌," +
                            " t2.bundleno 捆包号,t16.opgname 业务部门, t1.ContractNo 合同号,t1.weighttype 重量类型,t6.code 入库方式编码,t6.name 入库方式," +
                            "       case t1.thestockinoutkind when 0 then '蓝单' when 1  then '红单' end 单据类型,t1.id 序号,t1.createdate createdate,t1.code 单号,t3.code 仓库编码,t3.name 仓库名称,\n" +
                            "       t5.orgcode 供应商编码,t5.orgname 供应商名称,case t1.istally when 0 then '未记帐' when 1  then '记帐' end 记帐,t1.auditdate 审核日期,\n" +
                            "       t7.matcode 物料编码,t7.matname 物料名称,t7.matspecification 规格,t7.stuff 材质,0 冲红量,t15.orgname 原始供应商,t7.materialcategoryid 父分类, \n" +
                //"       (select nvl(sum(reftol),0) from thd_stkstockindtlrlt where forwarddetclsname like '%StockInDtl%' and forwarddetailid=t2.id) 冲红量," +
                            "       t8.standunitname 计量单位,t2.quantity 数量,t2.price 单价,t2.money 金额, t10.pername 制单人,t2.frommanufactory 厂家,t1.DESCRIPT 备注,t12.Pername 经办人,t12.Perid 业务员ID, t2.Other1 长度\n" +
                            "from   thd_stkstockin t1 inner join thd_stkstockinDtl t2\n" +
                            "       on t1.id=t2.parentid\n" +
                            "       left join thd_stkstationcategory t3\n" +
                            "       on t1.thestationcategory=t3.id\n" +
                            "       left join ressupplierrelation t4\n" +
                            "       on t1.supplierrelation=t4.suprelid\n" +
                            "       left join resorganization t5\n" +
                            "       on t4.orgid=t5.orgid\n" +
                            "       left join thd_stkstockinmanner t6\n" +
                            "       on t1.StockInManner=t6.id\n" +
                            "       left join resmaterial t7\n" +
                            "       on t2.material=t7.MATERIALID\n" +
                            "       left join resstandunit t8\n" +
                            "       on t2.MATSTANDARDUNIT=t8.STANDUNITID\n " +
                            "      left join resperson t10 on t1.createperson=t10.perid " +
                //"       left join thd_stkstockinrlt t9\n" +
                //"       on t1.id=t9.backwardid \n" +
                //"       left join thd_spysupplyreceving t11 \n" +
                //"       on t9.forwardid=t11.id \n" +
                            "       left join resperson t12 on t1.handleperson=t12.perid \n" +
                            "       left join ressupplierrelation t14 on t1.initsupplier=t14.suprelid\n" +
                            "       left join resorganization t15 on t14.orgid=t15.orgid\n" +
                            "       left join resoperationorg t16 on t1.handleorg=t16.opgid \n" +
                //"       left join thd_stkstockindtlrlt t13 on t13.forwarddetailid=t2.id and t13.forwarddetclsname like '%StockInDtl%' " +
                            "where 1=1 " + condition + " order by t1.Code";
            command.CommandText = strSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;

        }

        public string GetStockInID(string id)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            string stockInId = "";
            string sql = "";
            IDbConnection conn = session.Connection;
            if (conn is SqlConnection)
            {
                SqlCommand command = ((SqlConnection)conn).CreateCommand();
                sql = "select parentid from thd_stkstockindtl where id=@id";
                command.CommandText = sql;
                command.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
                object objId = command.ExecuteScalar();
                if (objId != null)
                {
                    stockInId = objId.ToString();
                }
            }
            else if (conn is OracleConnection)
            {
                OracleCommand command = ((OracleConnection)conn).CreateCommand();
                sql = "select parentid from thd_stkstockindtl where id=:id";
                command.CommandText = sql;
                command.Parameters.Add("id", OracleDbType.Varchar2).Value = id;
                object objId = command.ExecuteScalar();
                if (objId != null)
                {
                    stockInId = objId.ToString();
                }
            }
            return stockInId;
        }

        public DateTime GetServerDateTime()
        {
            return DateTime.Now;
        }
        public bool CheckServerDateTime()
        {
            //if (DateTime.Now > TransUtil.ToDateTime("2014-01-01"))
            //{
            //    return false;
            //}
            return true;
        }

        public StockInDtl GetStockInDtl(string stockInDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", stockInDtlId));
            oq.AddFetchMode("Master", FetchMode.Eager);
            IList list = Dao.ObjectQuery(typeof(StockInDtl), oq);

            if (list != null && list.Count > 0)
            {
                return list[0] as StockInDtl;
            }
            return null;
        }

        /// <summary>
        /// 查询收料单明细
        /// </summary>
        /// <param name="stockInBalDetailLst">验收结算单明细集合</param>
        /// <returns></returns>
        public IList GetStockInDtlLst(List<StockInBalDetail> stockInBalDetailLst)
        {
            if (stockInBalDetailLst == null || stockInBalDetailLst.Count == 0) return new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = Expression.Disjunction();
            foreach (StockInBalDetail obj in stockInBalDetailLst)
            {
                dis.Add(Expression.Eq("Id", obj.ForwardDetailId));
            }
            oq.AddCriterion(dis);
            if (dis.GetProjections().Length == 0) return new ArrayList();
            return Dao.ObjectQuery(typeof(StockInDtl), oq);
        }

        /// <summary>
        /// 查询收料单明细
        /// </summary>
        /// <param name="stockInBalDetail">验收结算单明细</param>
        /// <returns></returns>
        public IList GetStockInDtlLst(StockInBalDetail stockInBalDetail)
        {
            if (stockInBalDetail == null || stockInBalDetail.ForwardDetails.Count == 0) return new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = Expression.Disjunction();
            foreach (StockInBalDetail_ForwardDetail obj in stockInBalDetail.ForwardDetails)
            {
                dis.Add(Expression.Eq("Id", obj.ForwardDetailId));
            }
            oq.AddCriterion(dis);
            if (dis.ToString() == "()") return new ArrayList();
            return Dao.ObjectQuery(typeof(StockInDtl), oq);
        }

        /// <summary>
        /// 查询入库单明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStockInDtl(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(StockInDtl), oq);
        }

        public IList GetStockInDtlByBal(ObjectQuery oq, string projectId, string supplierId)
        {
            IList list = Dao.ObjectQuery(typeof(StockInDtl), oq);
            #region 1: 取开始和结束时间
            DateTime startDate = TransUtil.ToDateTime("2000-01-01");
            DateTime endDate = TransUtil.ToDateTime("2000-01-01");
            foreach (StockInDtl dtl in list)
            {
                BaseMaster master = dtl.Master;
                if (startDate == TransUtil.ToDateTime("2000-01-01"))
                {
                    startDate = master.RealOperationDate;
                }
                if (master.CreateDate < startDate)
                {
                    startDate = master.RealOperationDate;
                }

                if (endDate == TransUtil.ToDateTime("2000-01-01"))
                {
                    endDate = master.RealOperationDate;
                }
                if (master.CreateDate > endDate)
                {
                    endDate = master.RealOperationDate;
                }
                dtl.TempData = master.RealOperationDate.ToShortDateString();
                dtl.Price = 0;
            }
            #endregion

            #region 2: 取得项目降低率
            decimal downRate = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select t1.rate from thd_ProgramReduceRate t1 where t1.supplyer='" + supplierId + "' and t1.projectid='" + projectId + "' ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    downRate = TransUtil.ToDecimal(dataRow["rate"]);
                }
            }
            #endregion

            #region 3: 按时间取得合同调整价格表里价格
            Hashtable ht_adjust = new Hashtable();
            command.CommandText = " select t1.availabilitydate,t1.material,t1.modifyprice from thd_contractadjustprice t1 " +
                                    "  where t1.projectid='" + projectId + "' and t1.supplierrelation='" + supplierId + "' and state=5 " +
                                    " and t1.availabilitydate>=to_date('" + startDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                    " and t1.availabilitydate<=to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string material = TransUtil.ToString(dataRow["material"]);
                    DataDomain domain = new DataDomain();
                    domain.Name1 = material;
                    domain.Name2 = TransUtil.ToDateTime(dataRow["availabilitydate"]).ToShortDateString();
                    domain.Name3 = TransUtil.ToString(dataRow["modifyprice"]);

                    if (!ht_adjust.Contains(material))
                    {
                        IList temp = new ArrayList();
                        temp.Add(domain);
                        ht_adjust.Add(material, temp);
                    }
                    else
                    {
                        IList temp = (ArrayList)ht_adjust[material];
                        temp.Add(domain);
                        ht_adjust.Remove(material);
                        ht_adjust.Add(material, temp);
                    }
                }
            }
            #endregion

            #region 4: 如果合同调整价格表里无，则取得采购合同价格
            Hashtable ht_order = new Hashtable();
            command.CommandText = " select t2.material,t2.modifyprice,t1.signdate from thd_supplyordermaster t1,thd_supplyorderdetail t2 " +
                                  " where t1.id=t2.parentid and state=5 and t1.projectid ='" + projectId + "' and t1.supplierrelation='" + supplierId + "'";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string material = TransUtil.ToString(dataRow["material"]);
                    DataDomain domain = new DataDomain();
                    domain.Name1 = material;
                    domain.Name2 = TransUtil.ToDateTime(dataRow["signdate"]).ToShortDateString();
                    domain.Name3 = TransUtil.ToString(dataRow["modifyprice"]);

                    if (!ht_order.Contains(material))
                    {
                        IList temp = new ArrayList();
                        temp.Add(domain);
                        ht_order.Add(material, temp);
                    }
                    else
                    {
                        IList temp = (ArrayList)ht_order[material];
                        temp.Add(domain);
                        ht_order.Remove(material);
                        ht_order.Add(material, temp);
                    }
                }
            }
            #endregion

            #region 5: 取得信息价格,并乘降低率
            Hashtable ht_matinter = new Hashtable();
            command.CommandText = " select t1.materialguid,t1.price from thd_matinterprice t1 ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string material = TransUtil.ToString(dataRow["materialguid"]);
                    decimal price = TransUtil.ToDecimal(dataRow["price"]) * (1 - downRate / 100);
                    if (!ht_matinter.Contains(material))
                    {
                        ht_matinter.Add(material, price + "");
                    }
                }
            }
            #endregion

            #region 6: 补充价格
            foreach (StockInDtl dtl in list)
            {
                string material = dtl.MaterialResource.Id;
                DateTime createDate = TransUtil.ToDateTime(dtl.TempData);

                if (ht_adjust.Contains(material))
                {
                    IList temp = (ArrayList)ht_adjust[material];
                    DateTime oldDate = TransUtil.ToDateTime("2000-01-01");
                    foreach (DataDomain domain in temp)
                    {
                        if (TransUtil.ToDateTime(domain.Name2) > oldDate && createDate > TransUtil.ToDateTime(domain.Name2))
                        {
                            oldDate = TransUtil.ToDateTime(domain.Name2);
                            dtl.Price = TransUtil.ToDecimal(domain.Name3);
                        }
                    }
                    if (dtl.Price > 0)
                    {
                        continue;
                    }
                }

                if (ht_order.Contains(material))
                {
                    IList temp = (ArrayList)ht_order[material];
                    DateTime oldDate = TransUtil.ToDateTime("2000-01-01");
                    foreach (DataDomain domain in temp)
                    {
                        if (TransUtil.ToDateTime(domain.Name2) > oldDate)
                        {
                            oldDate = TransUtil.ToDateTime(domain.Name2);
                            dtl.Price = TransUtil.ToDecimal(domain.Name3);
                        }
                    }
                    continue;
                }

                if (ht_matinter.Contains(material))
                {
                    dtl.Price = TransUtil.ToDecimal(ht_matinter[material]);
                }
            }
            #endregion
            return list;
        }

        /// <summary>
        /// 查询入库冲红时序
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStockInDtlSeq(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(StockInDtlSeq), oq);
        }

        /// <summary>
        /// 保存收料入库单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        [TransManager]
        public StockIn SaveStockIn(StockIn obj, IList movedDtlList)
        {
            //操作过磅单材料关联表
            if (movedDtlList != null)
            {
                SaveWeightBillRelation(obj.Details.OfType<StockInDtl>().ToList(), movedDtlList.OfType<StockInDtl>().ToList());
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(StockIn), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(StockIn), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockIn;
                //新增时修改前续单据的引用数量
                foreach (StockInDtl dtl in obj.Details)
                {
                    DailyPlanDetail forwardDtl = GetDailyPlanDetailById(dtl.ForwardDetailId);
                    if (forwardDtl != null)
                    {
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                        dao.SaveOrUpdate(forwardDtl);
                    }
                }
            }
            else
            {
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveOrUpdateByDao(obj) as StockIn;
                foreach (StockInDtl dtl in obj.Details)
                {
                    DailyPlanDetail forwardDtl = GetDailyPlanDetailById(dtl.ForwardDetailId);
                    if (forwardDtl != null)
                    {
                        if (dtl.Id == null)
                        {
                            forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                        }
                        else
                        {
                            //修改时修改前续单据的引用数量为 引用数量+当前冲红数量与上次冲红数量的差值
                            forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                        }
                        dao.SaveOrUpdate(forwardDtl);
                    }
                }

                //修改时对于删除的明细 删除引用数量
                foreach (StockInDtl dtl in movedDtlList)
                {
                    DailyPlanDetail forwardDtl = GetDailyPlanDetailById(dtl.ForwardDetailId);
                    if (forwardDtl != null)
                    {
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                        dao.SaveOrUpdate(forwardDtl);
                    }
                }
            }
            if (obj.Special == "安装")
            {
                //安装直接做验收单
                #region 验收单
                StockInBalMaster oStockInBal = this.CreateStockInBalMaster(obj);
                if (oStockInBal != null)
                {
                    //SaveStockInBalMaster2(oStockInBal, movedDtlList);
                    SaveStockInBalMaster2(oStockInBal, null);
                }
                #endregion
                #region 资源滚动需求计划回写
                ObjectQuery oq1 = new ObjectQuery();
                IList list;
                decimal dQuantity = 0;
                foreach (StockInDtl dtl in obj.Details)
                {
                    if (dtl.ForwardDetailId != null)
                    {
                        oq1.Criterions.Clear();
                        oq1.AddCriterion(Expression.Eq("MaterialResource.Id", dtl.MaterialResource.Id));
                        if (dtl.DiagramNumber == "")
                        {
                            oq1.AddCriterion(Expression.IsNull("DiagramNumber"));
                        }
                        else
                        {
                            oq1.AddCriterion(Expression.Eq("DiagramNumber", dtl.DiagramNumber));
                        }

                        oq1.AddCriterion(Expression.Like("TheGWBSTaskGUID.Id", dtl.UsedPart.Id));
                        list = dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq1);
                        if (list == null || list.Count == 0) continue;
                        foreach (ResourceRequirePlanDetail plan in list)
                        {
                            plan.ExecutedQuantity += (dtl.Quantity - dtl.QuantityTemp);
                        }
                        SaveOrUpdateByDao(list);
                    }
                }
                #endregion
            }
            return obj;
        }
       
        /// <summary>
        /// 删除收料入库单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteStockIn(StockIn obj)
        {
            if (obj.Id == null) return true;
            //删除过磅单材料关联关系
            DeleteWeightBillRelation(obj.Details.OfType<StockInDtl>().ToList());
            //删除明细时 删除引用数量
            foreach (StockInDtl dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    DailyPlanDetail forwardDtl = GetDailyPlanDetailById(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.SaveOrUpdate(forwardDtl);
                }
            }
            if (obj.Special == "安装" && !string.IsNullOrEmpty(obj.Id))
            {
                ObjectQuery oQuery=new ObjectQuery ();
                oQuery .AddCriterion(Expression.Eq("ForwardBillId",obj.Id));
                IList lst = dao.ObjectQuery(typeof(StockInBalMaster), oQuery);
                if (lst != null && lst.Count > 0)
                {
                    dao.Delete(lst[0]);
                }
            }
            return dao.Delete(obj);
        }
        /// <summary>
        /// 删除关联入库单的验收结算单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool DeleteStockInBalMaster(StockIn obj)
        {
            if (obj.Special == "安装" && !string.IsNullOrEmpty(obj.Id))
            {
                ObjectQuery oQuery = new ObjectQuery();
                oQuery.AddCriterion(Expression.Eq("ForwardBillId", obj.Id));
                //oQuery.AddFetchMode("Details", FetchMode.Eager);
                IList lst = dao.ObjectQuery(typeof(StockInBalMaster), oQuery);
                if (lst != null && lst.Count > 0)
                {
                  return   DeleteStockInBalMaster(lst[0] as StockInBalMaster);
                }
            }
            return false;
        }
        #endregion

        #region 收料入库单红单方法
        public IList GetStockInRed(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockInRed), oq);
        }

        public StockInRed GetStockInRedById(string stockInRedId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", stockInRedId));
            IList list = GetStockInRed(oq);
            if (list == null || list.Count == 0) return null;
            return list[0] as StockInRed;
        }

        public StockInRed GetStockInRedByCode(string code, string special, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("Special", special));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockInRed(oq);
            if (list == null || list.Count == 0) return null;
            return list[0] as StockInRed;
        }

        public StockInRed GetStockInRedByCode(string code, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockInRed(oq);
            if (list == null || list.Count == 0) return null;
            return list[0] as StockInRed;
        }

        [TransManager]
        public StockInRed SaveStockInRed(StockInRed obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(StockInRed), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(StockInRed), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockInRed;
                //新增时修改前续单据的引用数量
                foreach (StockInRedDtl dtl in obj.Details)
                {
                    StockInDtl forwardDtl = GetStockInDtl(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);

                    //减少日常需求计划的引用数量
                    DailyPlanDetail dailyPlanDetail = GetDailyPlanDetailById(forwardDtl.ForwardDetailId);
                    if (dailyPlanDetail != null)
                    {
                        dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity - Math.Abs(dtl.Quantity);
                        dao.SaveOrUpdate(dailyPlanDetail);
                    }
                }
            }
            else
            {

                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveOrUpdateByDao(obj) as StockInRed;
                foreach (StockInRedDtl dtl in obj.Details)
                {
                    StockInDtl forwardDtl = GetStockInDtl(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);
                    }
                    else
                    {
                        //修改时修改前续单据的引用数量为 引用数量+当前冲红数量与上次冲红数量的差值
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                    }
                    dao.Save(forwardDtl);

                    //日常需求计划引用数量处理
                    DailyPlanDetail dailyPlanDetail = GetDailyPlanDetailById(forwardDtl.ForwardDetailId);
                    if (dailyPlanDetail != null)
                    {
                        if (dtl.Id == null)
                        {
                            //新增时减少引用数量
                            dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity - Math.Abs(dtl.Quantity);
                        }
                        else
                        {
                            //修改时减少引用数量
                            dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity - (Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp));
                        }
                        dao.SaveOrUpdate(dailyPlanDetail);//保存日常需求计划明细
                    }
                }

                //修改时对于删除的明细 删除引用数量
                foreach (StockInRedDtl dtl in movedDtlList)
                {
                    StockInDtl forwardDtl = GetStockInDtl(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.SaveOrUpdate(forwardDtl);

                    //增加日常需求计划的引用数量
                    DailyPlanDetail dailyPlanDetail = GetDailyPlanDetailById(forwardDtl.ForwardDetailId);
                    if (dailyPlanDetail != null)
                    {
                        dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity + Math.Abs(dtl.Quantity);
                        dao.SaveOrUpdate(dailyPlanDetail);   
                    }
                }
            }
            if (obj.Special == "安装")
            {
                StockInBalRedMaster oStockBalRed = this.CreateStockInBalRedMaster(obj);
                if (oStockBalRed != null)
                {
                    SaveStockInBalRedMaster(oStockBalRed, movedDtlList);
                }
            }
            return obj;
        }
        [TransManager]
        public StockInRed SaveStockInRed(StockInRed obj)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(StockInRed), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(StockInRed), obj.ProjectId, obj.ProfessionCategory);
                }
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockInRed;
                //新增时修改前续单据的引用数量
            }
            return obj;
        }
        [TransManager]
        public bool DeleteStockInRed(StockInRed obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (StockInRedDtl dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    StockInDtl forwardDtl = GetStockInDtl(dtl.ForwardDetailId);
                    forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                    dao.SaveOrUpdate(forwardDtl);

                    //增加日常需求计划的引用数量
                    DailyPlanDetail dailyPlanDetail = GetDailyPlanDetailById(forwardDtl.ForwardDetailId);
                    if (dailyPlanDetail != null)
                    {
                        dailyPlanDetail.RefQuantity = dailyPlanDetail.RefQuantity + Math.Abs(dtl.Quantity);
                        dao.SaveOrUpdate(dailyPlanDetail);
                    }
                }
            }
            return dao.Delete(obj);
        }
        #endregion

        #region 验收结算单方法
        /// <summary>
        /// 验收结算单查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetStockInBal(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            //objectQuery.AddCriterion(Expression.Eq("Details.MaterialResource.Id", "1"));
            //objectQuery.AddCriterion(Expression.Eq("Details.Id", "1"));
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockInBalMaster), objectQuery);
        }

        public IList GetStockInBalMaster(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ForwardDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockInBalMaster), oq);
        }
        private IList GetStockInBalDtl(ObjectQuery oq)
        {
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ForwardDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Master.MaterialCategory", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockInBalDetail), oq);
        }

        public Hashtable GetStockInBal(ObjectQuery oq, CurrentProjectInfo ProjectInfo)
        {
            Hashtable ht = new Hashtable();
            IList list = GetStockInBalDtl(oq);
            if (list != null && list.Count > 0)
            {
                foreach (StockInBalDetail detail in list)
                {
                    string strMaterial = detail.MaterialResource.Id;
                    if (ht.Count == 0)
                    {
                        detail.TempData2 = Convert.ToString(Convert.ToDecimal(detail.TempData2) + 1);
                        ht.Add(strMaterial, detail);
                    }
                    else
                    {
                        if (ht.Contains(strMaterial))
                        {
                            StockInBalDetail dtl = (StockInBalDetail)ht[strMaterial];
                            dtl.Quantity += detail.Quantity;//数量
                            dtl.Price += detail.Price;//验收单价
                            dtl.TempData2 = Convert.ToString(Convert.ToDecimal(dtl.TempData2) + 1);
                            ht.Remove(strMaterial);
                            ht.Add(strMaterial, dtl);
                        }
                        else
                        {
                            detail.TempData2 = Convert.ToString(Convert.ToDecimal(detail.TempData2) + 1);
                            ht.Add(strMaterial, detail);
                        }
                    }
                }
            }

            ObjectQuery objqy = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            IList listSupply = SupplyOrderSrv.GetSupplyOrder(objqy);

            if (listSupply.Count > 0)
            {
                IList listTemp = new ArrayList();
                foreach (DictionaryEntry objSys in ht)
                {
                    decimal dePrice = 0;
                    decimal quantity = 0;
                    string strMaterial = Convert.ToString(objSys.Key);
                    StockInBalDetail dtl = objSys.Value as StockInBalDetail;
                    foreach (SupplyOrderMaster master in listSupply)
                    {
                        foreach (SupplyOrderDetail detail in master.Details)
                        {
                            if (detail.MaterialResource.Id == strMaterial)
                            {
                                dePrice += detail.SupplyPrice;
                                quantity += 1;
                            }
                        }
                    }
                    listTemp.Add(dtl);
                }
                foreach (StockInBalDetail del in listTemp)
                {
                    string strId = Convert.ToString(del.MaterialResource.Id);
                    if (ht.Contains(strId))
                    {
                        ht.Remove(strId);
                        ht.Add(strId, del);
                    }
                }
            }

            //工程量核算单 求合同单价
            //objqy = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("Master.ProjectId", ProjectInfo.Id));
            //IList lstProjectAccount = Dao.ObjectQuery(typeof(ProjectTaskDetailAccountSubject), objqy);

            ObjectQuery oqy = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("State", "1"));
            IList listPrice = CurrentProjectSrv.GetMaterialPrice(oqy);
            if (listPrice.Count > 0)
            {
                foreach (MaterialInterfacePrice matPrice in listPrice)
                {
                    string strMat = matPrice.MaterialGUID.Id;
                    if (ht.Contains(strMat))
                    {
                        StockInBalDetail dtl = ht[strMat] as StockInBalDetail;
                        dtl.TempData1 = Convert.ToString(matPrice.MarketPrice);//市场价
                        ht.Remove(strMat);
                        ht.Add(strMat, dtl);
                    }
                }
            }
            return ht;
        }

        public Hashtable GetStockInBal(string materialCode, DateTime beginDate, DateTime endDate, CurrentProjectInfo ProjectInfo)
        {
            Hashtable ht = new Hashtable();
            ObjectQuery oq = new ObjectQuery();
            if (!string.IsNullOrEmpty(materialCode))
            {
                oq.AddCriterion(Expression.Like("MaterialCode", materialCode + "%"));
            }
            oq.AddCriterion(Expression.Eq("Master.DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.Ge("Master.CreateDate", beginDate.Date));
            oq.AddCriterion(Expression.Lt("Master.CreateDate", endDate.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("Master.ProjectId", ProjectInfo.Id));

            IList list = GetStockInBalDtl(oq);
            if (list != null && list.Count > 0)
            {
                foreach (StockInBalDetail detail in list)
                {
                    string strMaterial = detail.MaterialResource.Id;
                    if (ht.Count == 0)
                    {
                        detail.TempData2 = Convert.ToString(Convert.ToDecimal(detail.TempData2) + 1);
                        ht.Add(strMaterial, detail);
                    }
                    else
                    {
                        if (ht.Contains(strMaterial))
                        {
                            StockInBalDetail dtl = (StockInBalDetail)ht[strMaterial];
                            dtl.Quantity += detail.Quantity;//数量
                            dtl.Price += detail.Price;//验收单价
                            dtl.TempData2 = Convert.ToString(Convert.ToDecimal(dtl.TempData2) + 1);
                            ht.Remove(strMaterial);
                            ht.Add(strMaterial, dtl);
                        }
                        else
                        {
                            detail.TempData2 = Convert.ToString(Convert.ToDecimal(detail.TempData2) + 1);
                            ht.Add(strMaterial, detail);
                        }
                    }
                }
            }

            ObjectQuery objqy = new ObjectQuery();
            objqy.AddCriterion(Expression.Eq("ProjectId", ProjectInfo.Id));
            //if (!string.IsNullOrEmpty(materialCode))
            //{
            //    objqy.AddCriterion(Expression.Like("MaterialCode", materialCode + "%"));
            //}
            IList listSupply = SupplyOrderSrv.GetSupplyOrder(objqy);

            if (listSupply.Count > 0)
            {
                IList listTemp = new ArrayList();
                foreach (DictionaryEntry objSys in ht)
                {
                    decimal dePrice = 0;
                    decimal quantity = 0;
                    string strMaterial = Convert.ToString(objSys.Key);
                    StockInBalDetail dtl = objSys.Value as StockInBalDetail;
                    foreach (SupplyOrderMaster master in listSupply)
                    {
                        foreach (SupplyOrderDetail detail in master.Details)
                        {
                            if (detail.MaterialResource.Id == strMaterial)
                            {
                                dePrice += detail.SupplyPrice;
                                quantity += 1;
                            }
                        }
                    }
                    listTemp.Add(dtl);
                }
                foreach (StockInBalDetail del in listTemp)
                {
                    string strId = Convert.ToString(del.MaterialResource.Id);
                    if (ht.Contains(strId))
                    {
                        ht.Remove(strId);
                        ht.Add(strId, del);
                    }
                }
            }

            #region 工程量核算单 求合同金额
            //工程量核算单 求合同单价
            string con = " and a.theProjectGuid='" + ProjectInfo.Id + "' and a.createdate<to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')+1";
            if (!string.IsNullOrEmpty(materialCode))
            {
                con += " and d.matCode like '" + materialCode + "%'";
            }
            string sql = @"select d.materialid,sum(b.rescontractquantity) htsl,sum(b.contractincometotal) htje              
                from thd_projecttaskaccountbill a,thd_projecttaskdtlacctsubject b,thd_projecttaskdetailaccount c,resmaterial d
                where a.id=c.parentid and c.id=b.parentid and b.resourcetypeguid=d.materialid and a.state=5 " + con + @"
                group by d.materialid";
            DataSet dsHTJE = GetStockInBal_HTJE(sql);
            if (dsHTJE != null && dsHTJE.Tables.Count > 0)
            {
                DataTable dt = dsHTJE.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    IList lstTemp = new ArrayList();
                    foreach (DictionaryEntry objSys in ht)
                    {
                        StockInBalDetail dtl = objSys.Value as StockInBalDetail;
                        foreach (DataRow dr in dt.Rows)
                        {
                            string materialid = dr["materialid"] + "";
                            if (dtl.MaterialResource.Id == materialid)
                            {
                                decimal htsl = ClientUtil.ToDecimal(dr["htsl"]);
                                decimal htje = ClientUtil.ToDecimal(dr["htje"]);
                                if (htsl != 0)
                                {
                                    dtl.TempData = (dtl.Quantity * (htje / htsl)).ToString("####################.##");
                                    lstTemp.Add(dtl);
                                }
                            }

                        }
                    }
                    if (lstTemp.Count > 0)
                    {
                        foreach (StockInBalDetail dtl in lstTemp)
                        {
                            string materialid = dtl.MaterialResource.Id;
                            if (ht.Contains(materialid))
                            {
                                ht.Remove(materialid);
                                ht.Add(materialid, dtl);
                            }
                        }
                    }
                }
            }
            #endregion

            ObjectQuery oqy = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("State", "1"));
            IList listPrice = CurrentProjectSrv.GetMaterialPrice(oqy);
            if (listPrice.Count > 0)
            {

                foreach (MaterialInterfacePrice matPrice in listPrice)
                {
                    string strMat = matPrice.MaterialGUID.Id;
                    if (ht.Contains(strMat))
                    {
                        StockInBalDetail dtl = ht[strMat] as StockInBalDetail;
                        dtl.TempData1 = Convert.ToString(matPrice.MarketPrice);//市场价
                        ht.Remove(strMat);
                        ht.Add(strMat, dtl);
                    }
                }

            }
            return ht;
        }

        private DataSet GetStockInBal_HTJE(string sql)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        public IList GetStockInBalDetail(ObjectQuery oq)
        {
            oq.AddFetchMode("ForwardDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Master.MaterialCategory", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockInBalDetail), oq);
        }

        public StockInBalMaster GetStockInBalMasterByCode(string code, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockInBalMaster(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockInBalMaster;
            }
            return null;
        }

        public StockInBalMaster GetStockInBalMasterById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockInBalMaster(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockInBalMaster;
            }
            return null;
        }

        [TransManager]
        public StockInBalMaster SaveStockInBalMaster(StockInBalMaster obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(StockInBalMaster));
                obj = SaveByDao(obj) as StockInBalMaster;
                //新增时修改前续单据的引用数量
                foreach (StockInBalDetail dtl in obj.Details)
                {
                    StockInDtl forwardDtl = GetStockInDtl(dtl.ForwardDetailId);
                    forwardDtl.BalQuantity = forwardDtl.BalQuantity + Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }
            else
            {
                obj = SaveOrUpdateByDao(obj) as StockInBalMaster;
                foreach (StockInBalDetail dtl in obj.Details)
                {
                    StockInDtl forwardDtl = GetStockInDtl(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        forwardDtl.BalQuantity = forwardDtl.BalQuantity + Math.Abs(dtl.Quantity);
                    }
                    else
                    {
                        //修改时修改前续单据的引用数量为 引用数量+当前冲红数量与上次冲红数量的差值
                        forwardDtl.BalQuantity = forwardDtl.BalQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                    }
                    dao.Save(forwardDtl);
                }

                //修改时对于删除的明细 删除引用数量
                foreach (StockInBalDetail dtl in movedDtlList)
                {
                    StockInDtl forwardDtl = GetStockInDtl(dtl.ForwardDetailId);
                    forwardDtl.BalQuantity = forwardDtl.BalQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }

            return obj;
        }

        /// <summary>
        /// 验收结算汇总时的保存
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        [TransManager]
        public StockInBalMaster SaveStockInBalMaster2(StockInBalMaster obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                string sAbbreviation = string.Empty;
                if (TransUtil.ToString(obj.ProfessionCategory) == "")
                {
                    sAbbreviation = obj.MaterialCategory.Abbreviation;
                }
                else
                {
                    sAbbreviation = obj.ProfessionCategory;
                }
                obj.Code = GetCode(typeof(StockInBalMaster), obj.ProjectId, sAbbreviation);
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                //新增时修改前续单据的引用数量
                foreach (StockInBalDetail dtl in obj.Details)
                {
                    decimal tempQty = dtl.Quantity;
                    foreach (StockInBalDetail_ForwardDetail forwardDetail in dtl.ForwardDetails)
                    {
                        StockInDtl stockInDtl = GetStockInDtl(forwardDetail.ForwardDetailId);
                        decimal canUserQty = stockInDtl.Quantity - stockInDtl.BalQuantity - stockInDtl.RefQuantity;
                        if (canUserQty >= tempQty)
                        {
                            stockInDtl.BalQuantity = stockInDtl.BalQuantity + tempQty;
                            forwardDetail.Quantity = tempQty;
                            tempQty = 0;

                        }
                        else
                        {
                            stockInDtl.BalQuantity = stockInDtl.BalQuantity + canUserQty;
                            forwardDetail.Quantity = canUserQty;
                            tempQty = tempQty - canUserQty;
                        }
                        dao.Save(stockInDtl);

                    }
                }
                obj = SaveByDao(obj) as StockInBalMaster;
            }
            else
            {
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                foreach (StockInBalDetail dtl in obj.Details)
                {
                    if (dtl.Id == null)
                    {
                        decimal tempQty = dtl.Quantity;
                        foreach (StockInBalDetail_ForwardDetail forwardDetail in dtl.ForwardDetails)
                        {
                            StockInDtl stockInDtl = GetStockInDtl(forwardDetail.ForwardDetailId);
                            decimal canUserQty = stockInDtl.Quantity - stockInDtl.BalQuantity - stockInDtl.RefQuantity;
                            if (canUserQty >= tempQty)
                            {
                                stockInDtl.BalQuantity = stockInDtl.BalQuantity + tempQty;
                                forwardDetail.Quantity = tempQty;
                                tempQty = 0;
                            }
                            else
                            {
                                stockInDtl.BalQuantity = stockInDtl.BalQuantity + canUserQty;
                                forwardDetail.Quantity = canUserQty;
                                tempQty = tempQty - canUserQty;
                            }
                            dao.Save(stockInDtl);
                        }
                    }
                    else
                    {
                        decimal diffQty = Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                        decimal tempDiffQty = Math.Abs(diffQty);
                        foreach (StockInBalDetail_ForwardDetail forwardDetail in dtl.ForwardDetails)
                        {
                            StockInDtl stockInDtl = GetStockInDtl(forwardDetail.ForwardDetailId);
                            decimal canUseQty = stockInDtl.Quantity - stockInDtl.BalQuantity - stockInDtl.RefQuantity;
                            if (diffQty >= 0)
                            {
                                //修改时增加了结算数量
                                if (canUseQty >= diffQty)
                                {
                                    stockInDtl.BalQuantity = stockInDtl.BalQuantity + diffQty;
                                    forwardDetail.Quantity = forwardDetail.Quantity + diffQty;
                                    diffQty = 0;
                                }
                                else
                                {
                                    stockInDtl.BalQuantity = stockInDtl.BalQuantity + canUseQty;
                                    forwardDetail.Quantity = forwardDetail.Quantity + canUseQty;
                                    diffQty = diffQty - canUseQty;
                                }
                            }
                            else
                            {
                                //修改时减少了引用数量，从结算数量中减去相应的数量
                                decimal balQty = stockInDtl.BalQuantity;
                                if (balQty >= tempDiffQty)
                                {
                                    stockInDtl.BalQuantity = stockInDtl.BalQuantity - tempDiffQty;
                                    forwardDetail.Quantity = forwardDetail.Quantity - tempDiffQty;
                                    tempDiffQty = 0;
                                }
                                else
                                {
                                    tempDiffQty = tempDiffQty - balQty;
                                    stockInDtl.BalQuantity = 0;
                                    forwardDetail.Quantity = 0;
                                }
                            }
                            dao.Save(stockInDtl);
                        }
                    }

                }
                obj = SaveOrUpdateByDao(obj) as StockInBalMaster;

                //修改时对于删除的明细 删除引用数量
                foreach (StockInBalDetail dtl in movedDtlList)
                {
                    if (dtl.Id != null)
                    {
                        foreach (StockInBalDetail_ForwardDetail forObj in dtl.ForwardDetails)
                        {
                            if (forObj != null)
                            {
                                StockInDtl forwardDtl = GetStockInDtl(forObj.ForwardDetailId);
                                forwardDtl.BalQuantity = forwardDtl.BalQuantity - Math.Abs(forObj.Quantity);
                                dao.Save(forwardDtl);
                            }
                        }
                    }
                }
            }

            return obj;
        }

        #region 验收结算单提交处理
        private IStockOutSrv stockOutSrv;
        public IStockOutSrv StockOutSrv
        {
            get { return stockOutSrv; }
            set { stockOutSrv = value; }
        }

        /// <summary>
        /// 提交验收结算单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        [TransManager]
        public StockInBalMaster SubmitStockInBalMaster(StockInBalMaster obj, IList movedDtlList)
        {
            StockInBalMaster stockInBalMaster = SaveStockInBalMaster(obj, movedDtlList);
            string stockInId = "";
            foreach (StockInBalDetail stockInBalDetail in stockInBalMaster.Details)
            {
                //更新入库单明细的单价、金额
                StockInDtl stockInDtl = GetStockInDtl(stockInBalDetail.ForwardDetailId);
                decimal costMoney = stockInBalDetail.CostMoney;//力资运费
                if (stockInDtl.Quantity != 0)
                {
                    stockInDtl.Money = stockInBalDetail.Price * stockInDtl.Quantity + costMoney;
                    stockInDtl.Price = decimal.Round(stockInDtl.Money / stockInDtl.Quantity, 8);
                }
                Dao.SaveOrUpdate(stockInDtl);

                stockInId = stockInDtl.Master.Id;

                //更新库存单价、金额
                UpdateStockRelation(stockInDtl.Id, stockInDtl.Price);

                //更新出库单单价、金额
                UpdateStockOutBySql(stockInDtl.Id, stockInDtl.Price);
            }

            //更新入库单总金额
            if (stockInId != "")
            {
                StockIn stockIn = GetStockInById(stockInId);
                decimal money = 0M;
                foreach (StockInDtl stockInDtl in stockIn.Details)
                {
                    money += stockInDtl.Money;
                }
                stockIn.SumMoney = money;
                dao.SaveOrUpdate(stockIn);
            }

            return stockInBalMaster;
        }

        /// <summary>
        /// 提交验收结算单 汇总后方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        [TransManager]
        public StockInBalMaster SubmitStockInBalMaster2(StockInBalMaster obj, IList movedDtlList)
        {
            StockInBalMaster stockInBalMaster = SaveStockInBalMaster2(obj, movedDtlList);
            List<string> stockInIdLst = new List<string>();
            foreach (StockInBalDetail stockInBalDetail in stockInBalMaster.Details)
            {
                foreach (StockInBalDetail_ForwardDetail forObj in stockInBalDetail.ForwardDetails)
                {
                    //更新入库单明细的单价、金额
                    StockInDtl stockInDtl = GetStockInDtl(forObj.ForwardDetailId);
                    decimal costMoney = stockInBalDetail.CostMoney;//力资运费
                    if (stockInDtl.Quantity != 0)
                    {
                        stockInDtl.Money = stockInBalDetail.Price * stockInDtl.Quantity + costMoney;
                        stockInDtl.Price = decimal.Round(stockInDtl.Money / stockInDtl.Quantity, 8);
                    }
                    Dao.SaveOrUpdate(stockInDtl);

                    if (!stockInIdLst.Contains(stockInDtl.Master.Id))
                    {
                        stockInIdLst.Add(stockInDtl.Master.Id);
                    }

                    //更新库存单价、金额
                    UpdateStockRelation(stockInDtl.Id, stockInDtl.Price);

                    //更新出库单单价、金额
                    UpdateStockOutBySql(stockInDtl.Id, stockInDtl.Price);
                }
            }

            //更新入库单总金额
            if (stockInIdLst.Count > 0)
            {
                foreach (string stockInId in stockInIdLst)
                {
                    StockIn stockIn = GetStockInById(stockInId);
                    decimal money = 0M;
                    foreach (StockInDtl stockInDtl in stockIn.Details)
                    {
                        money += stockInDtl.Money;
                    }
                    stockIn.SumMoney = money;
                    dao.SaveOrUpdate(stockIn);
                }
            }

            //判断是否隔月，如果隔月需调差
            //1：一笔调差之前的入库和出库 2：入库冲红，重新以新价格入库

            return stockInBalMaster;
        }
        /// <summary>
        /// 提交调价单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <param name="lstStock">存放顺序：入库红单  入库蓝单  出库调价单  入库调价单</param>
        /// <returns></returns>
        [TransManager]
        public StockInBalMaster SubmitStockInBalMaster3(StockInBalMaster obj, IList movedDtlList, IList lstStock)
        {
            StockInBalMaster stockInBalMaster = SaveStockInBalMaster2(obj, movedDtlList);//保存结算单;

            List<string> stockInIdLst = new List<string>();
            StockIn oStockInMasterAdjustPrice = null;
            StockOut oStockOutMasterAdjustPrice = null;
            bool bFirst = true;
            //判断是否隔月，如果隔月需调差
            //1：一笔调差之前的入库和出库 2：入库冲红，重新以新价格入库
            // 隔月出库  ：做一个出库蓝单（金额：差价×出库数，单价：差价 ，数量：出库数）  做一个入库蓝单（金额：差价×出库数，单价：差价 ，数量：出库数）
            //隔月入库：将库存做入库冲红（金额：改前价格×库存数，单价：改前价格 ，数量：库存数）  在做入库蓝单（金额：改后价格×库存数，单价：改后价格 ，数量：库存数）

            foreach (StockInBalDetail stockInBalDetail in stockInBalMaster.Details)
            {
                decimal calBalPrice = 0;
                foreach (StockInBalDetail_ForwardDetail forObj in stockInBalDetail.ForwardDetails)
                {
                    if (stockInBalDetail.Quantity != 0 && stockInBalDetail.CostMoney != 0)
                    {
                        calBalPrice = decimal.Round((stockInBalDetail.Money + stockInBalDetail.CostMoney) / stockInBalDetail.Quantity, 8);
                    }
                    else
                    {
                        calBalPrice = stockInBalDetail.Price;
                    }
                    //更新入库单明细的单价、金额
                    StockInDtl stockInDtl = GetStockInDtl(forObj.ForwardDetailId);
                    if ((stockInDtl.Master.CreateYear != stockInBalMaster.CreateYear) || (stockInDtl.Master.CreateYear == stockInBalMaster.CreateYear && stockInDtl.Master.CreateMonth != stockInBalDetail.Master.CreateMonth))
                    {
                        if (stockInDtl.Price != calBalPrice)//核算价格与入库价格存在差异,一笔调差
                        {
                            if (bFirst)
                            {
                                bFirst = false;
                                oStockInMasterAdjustPrice = CopyToStockInMaster(stockInDtl, stockInBalMaster);
                                oStockInMasterAdjustPrice.Descript = "[系统生成][调价入库单蓝单(为了补调价后出库的价格差做的入库单)]";
                                oStockOutMasterAdjustPrice = CopyToStockOutMaster(stockInDtl, stockInBalMaster);
                                oStockOutMasterAdjustPrice.Descript = "[系统生成][调价出库单蓝单(为了补调价后出库的价格差做的出库蓝单)]";
                            }
                            DiffMonthAdjust(stockInDtl, stockInBalDetail, stockInBalMaster, oStockInMasterAdjustPrice, oStockOutMasterAdjustPrice);//不记账
                        }
                    }
                    else
                    {
                        if (stockInDtl.Quantity != 0)
                        {
                            stockInDtl.Money = decimal.Round(calBalPrice * stockInDtl.Quantity, 2);
                            stockInDtl.Price = calBalPrice;
                        }
                        Dao.SaveOrUpdate(stockInDtl);

                        if (!stockInIdLst.Contains(stockInDtl.Master.Id))
                        {
                            stockInIdLst.Add(stockInDtl.Master.Id);
                        }
                       
                        //更新出库单单价、金额
                        UpdateStockOutBySql(stockInDtl.Id, calBalPrice);
                        //更新库存单价、金额(已经调差的,不再更新库存单价)
                        //UpdateStockRelation(stockInDtl.Id, calBalPrice);
                        UpdateStockRelationByNew(stockInDtl.Id, calBalPrice);
                    }
                }
            }
            //更新入库单总金额
            if (stockInIdLst.Count > 0)
            {
                foreach (string stockInId in stockInIdLst)
                {
                    StockIn stockIn = GetStockInById(stockInId);
                    decimal money = 0M;
                    foreach (StockInDtl stockInDtl in stockIn.Details)
                    {
                        money += stockInDtl.Money;
                    }
                    UpdateStockInSummoney(stockInId, money);
                    //stockIn.SumMoney = money;
                    //dao.SaveOrUpdate(stockIn);
                }
            }
            if (oStockOutMasterAdjustPrice != null)
            {
                oStockOutMasterAdjustPrice = StockOutSrv.SaveStockOut(oStockOutMasterAdjustPrice);
            }
            if (oStockInMasterAdjustPrice != null)
            {
                oStockInMasterAdjustPrice = SaveStockIn(oStockInMasterAdjustPrice, null);
            }
            return stockInBalMaster;
        }
        /// <summary>
        /// 验收结算单审批
        /// </summary>
        [TransManager]
        public void SubmitStockInBalMasterByAudit(StockInBalMaster stockInBalMaster)
        {
            List<string> stockInIdLst = new List<string>();
            StockIn oStockInMasterAdjustPrice = null;
            StockOut oStockOutMasterAdjustPrice = null;
            bool bFirst = true;
            //判断是否隔月，如果隔月需调差
            //1：一笔调差之前的入库和出库 2：入库冲红，重新以新价格入库
            // 隔月出库  ：做一个出库蓝单（金额：差价×出库数，单价：差价 ，数量：出库数）  做一个入库蓝单（金额：差价×出库数，单价：差价 ，数量：出库数）
            //隔月入库：将库存做入库冲红（金额：改前价格×库存数，单价：改前价格 ，数量：库存数）  在做入库蓝单（金额：改后价格×库存数，单价：改后价格 ，数量：库存数）

            foreach (StockInBalDetail stockInBalDetail in stockInBalMaster.Details)
            {
                foreach (StockInBalDetail_ForwardDetail forObj in stockInBalDetail.ForwardDetails)
                {
                    //更新入库单明细的单价、金额
                    StockInDtl stockInDtl = GetStockInDtl(forObj.ForwardDetailId);
                    if ((stockInDtl.Master.CreateYear < stockInBalMaster.CreateYear) || (stockInDtl.Master.CreateYear == stockInBalMaster.CreateYear && stockInDtl.Master.CreateMonth < stockInBalDetail.Master.CreateMonth))
                    {
                        if (stockInDtl.Price != stockInBalDetail.Price)//核算价格与入库价格存在差异
                        {
                            if (bFirst)
                            {
                                bFirst = false;
                                oStockInMasterAdjustPrice = CopyToStockInMaster(stockInDtl, stockInBalMaster);
                                oStockInMasterAdjustPrice.Descript = "[系统生成][调价入库单蓝单(为了补调价后出库的价格差做的入库单)]";
                                oStockOutMasterAdjustPrice = CopyToStockOutMaster(stockInDtl, stockInBalMaster);
                                oStockOutMasterAdjustPrice.Descript = "[系统生成][调价出库单蓝单(为了补调价后出库的价格差做的出库蓝单)]";
                            }
                            DiffMonthAdjust(stockInDtl, stockInBalDetail, stockInBalMaster, oStockInMasterAdjustPrice, oStockOutMasterAdjustPrice);//不记账
                        }
                    }
                    else
                    {
                        decimal costMoney = stockInBalDetail.CostMoney;//力资运费
                        if (stockInDtl.Quantity != 0)
                        {
                            stockInDtl.Money = stockInBalDetail.Price * stockInDtl.Quantity + costMoney;
                            stockInDtl.Price = decimal.Round(stockInDtl.Money / stockInDtl.Quantity, 8);
                        }
                        Dao.SaveOrUpdate(stockInDtl);

                        if (!stockInIdLst.Contains(stockInDtl.Master.Id))
                        {
                            stockInIdLst.Add(stockInDtl.Master.Id);
                        }

                        //更新库存单价、金额
                        UpdateStockRelation(stockInDtl.Id, stockInDtl.Price);
                        //更新出库单单价、金额
                        UpdateStockOutBySql(stockInDtl.Id, stockInDtl.Price);
                    }
                }

                //更新入库单总金额
                if (stockInIdLst.Count > 0)
                {
                    foreach (string stockInId in stockInIdLst)
                    {
                        StockIn stockIn = GetStockInById(stockInId);
                        decimal money = 0M;
                        foreach (StockInDtl stockInDtl in stockIn.Details)
                        {
                            money += stockInDtl.Money;
                        }
                        stockIn.SumMoney = money;
                        dao.SaveOrUpdate(stockIn);
                    }
                }
            }
            if (oStockOutMasterAdjustPrice != null)
            {
                oStockOutMasterAdjustPrice = StockOutSrv.SaveStockOut(oStockOutMasterAdjustPrice);
            }
            if (oStockInMasterAdjustPrice != null)
            {
                oStockInMasterAdjustPrice = SaveStockIn(oStockInMasterAdjustPrice, null);
            }
        }
        /// <summary>
        /// 对于存在价格差的入库单（为结算的单据）做调价（不需要记账）
        /// </summary>
        /// <param name="oStockInDtl"></param>
        /// <param name="oStockInBalDetail"></param>
        /// <param name="stockInBalMaster"></param>
        /// <param name="oStockInMasterAdjustPrice"></param>
        /// <param name="oStockOutMasterAdjustPrice"></param>
        [TransManager]
        public void DiffMonthAdjust(StockInDtl oStockInDtl, StockInBalDetail oStockInBalDetail, StockInBalMaster stockInBalMaster, StockIn oStockInMasterAdjustPrice, StockOut oStockOutMasterAdjustPrice)
        {
            if (oStockInDtl != null && oStockInBalDetail != null)
            {
                string sStockInDtlID = oStockInDtl.Id;
                IList lstStockRelation = StockRelationSrv.GetStockRelationByStockInDtlId(sStockInDtlID);//获取该入库单的关系信息 找出里面大于零的
                if (lstStockRelation != null && lstStockRelation.Count > 0)
                {
                    StockRelation oStockRelation = lstStockRelation[0] as StockRelation;
                    if (oStockRelation != null)
                    {
                        #region 出库调差单
                        //差异单价需加上力资运费
                        decimal balPrice = 0;
                        if (oStockInBalDetail.Quantity != 0)
                        {
                            balPrice = Decimal.Round((oStockInBalDetail.Money + oStockInBalDetail.CostMoney) / oStockInBalDetail.Quantity, 8);
                        }
                        else {
                            balPrice = oStockInBalDetail.Price;
                        }
                        decimal dDiffPrice = balPrice - oStockInDtl.Price;
                        //decimal dQuantity = oStockRelation.Quantity - oStockRelation.RemainQuantity - oStockInDtl.RefQuantity;//入库数-库存数-冲红数
                        //decimal dQuantity = oStockRelation.Quantity - oStockInDtl.RefQuantity;//入库数--冲红数
                        decimal dQuantity = oStockInDtl.BalQuantity;//未结算数 2016-12-16
                        if (dDiffPrice != 0 && dQuantity > 0)
                        {
                            #region 出库蓝单

                            decimal diffMoney = 0;
                            if (dQuantity == oStockInBalDetail.Quantity)
                            {
                                diffMoney = oStockInBalDetail.Money + oStockInBalDetail.CostMoney - oStockInDtl.Money;
                            }
                            else {
                                diffMoney = decimal.Round(dDiffPrice * dQuantity, 2);
                            }
                            //明细
                            StockOutDtl oStockOutDtlHand = CopyToStockOutDtl(oStockInDtl);
                            oStockOutDtlHand.ForwardDetailId = oStockInBalDetail.Id;
                            oStockOutDtlHand.Id = null;
                            oStockOutDtlHand.Price = dDiffPrice;//价格
                            oStockOutDtlHand.Money = diffMoney;//总额
                            oStockOutDtlHand.Quantity = 0;// dQuantity;//总数
                            oStockOutDtlHand.StockOutDtlSeqList.Clear();
                            oStockOutMasterAdjustPrice.AddDetail(oStockOutDtlHand);
                            oStockOutMasterAdjustPrice.SumMoney += diffMoney;
                            #endregion
                            #region 入库蓝单
                            //明细
                            StockInDtl oRemainStockInDtlHand = CopyToStockInDtl(oStockInDtl);
                            oRemainStockInDtlHand.ForwardDetailId = oStockInBalDetail.Id;
                            oRemainStockInDtlHand.Id = null;
                            oRemainStockInDtlHand.Price = dDiffPrice;//价格
                            oRemainStockInDtlHand.ConfirmPrice = dDiffPrice;
                            oRemainStockInDtlHand.Quantity = 0;// dQuantity;// oStockOutDtlHand.Quantity;//数量
                            oRemainStockInDtlHand.RefQuantity = 0;
                            oRemainStockInDtlHand.BalQuantity = 0;
                            oRemainStockInDtlHand.ConfirmMoney = diffMoney;//oStockOutDtlHand.Quantity;
                            oRemainStockInDtlHand.Money = diffMoney;// oStockOutDtlHand.Quantity;//总额
                            oStockInMasterAdjustPrice.AddDetail(oRemainStockInDtlHand);
                            oStockInMasterAdjustPrice.SumConfirmMoney += diffMoney;
                            oStockInMasterAdjustPrice.SumMoney += diffMoney;

                            #endregion
                        }
                        #endregion
                    }
                }
            }
        }
        /// <summary>
        /// 对于存在价格差的入库单（为结算的单据）做调价
        /// </summary>
        /// <param name="oStockInDtl">入库明细</param>
        /// <param name="oStockInBalDetail">结算明细</param>
        /// <param name="stockInBalMaster">结算单</param>
        /// <param name="oStockInMasterTall">库存添加后的入库单</param>
        /// <param name="oStockInRedMasterTall">库存冲红的红单</param>
        /// <param name="oStockInMasterAdjustPrice">出库后的入库调价</param>
        /// <param name="oStockOutMasterAdjustPrice">出库后的出库调价单</param>
        [TransManager]
        public void DiffMonthAdjust(StockInDtl oStockInDtl, StockInBalDetail oStockInBalDetail, StockInBalMaster stockInBalMaster, StockIn oStockInMasterTall, StockInRed oStockInRedMasterTall, StockIn oStockInMasterAdjustPrice, StockOut oStockOutMasterAdjustPrice)
        {
            if (oStockInDtl != null && oStockInBalDetail != null)
            {
                string sStockInDtlID = oStockInDtl.Id;
                IList lstStockRelation = StockRelationSrv.GetStockRelationByStockInDtlId(sStockInDtlID);//获取该入库单的关系信息 找出里面大于零的
                if (lstStockRelation != null && lstStockRelation.Count > 0)
                {
                    StockRelation oStockRelation = lstStockRelation[0] as StockRelation;
                    if (oStockRelation != null)
                    {
                        #region 出库调差单
                        decimal dDiffPrice = oStockInBalDetail.Price - oStockInDtl.Price;
                        decimal dQuantity = oStockRelation.Quantity - oStockRelation.RemainQuantity - oStockInDtl.RefQuantity;//入库数-库存数-冲红数
                        if (dDiffPrice != 0 && dQuantity > 0)
                        {
                            #region 出库蓝单

                            //明细
                            StockOutDtl oStockOutDtlHand = CopyToStockOutDtl(oStockInDtl);
                            oStockOutDtlHand.Id = null;
                            oStockOutDtlHand.Price = dDiffPrice;//价格
                            oStockOutDtlHand.Money = dDiffPrice * dQuantity;//总额
                            oStockOutDtlHand.Quantity = 0;// dQuantity;//总数
                            oStockOutDtlHand.StockOutDtlSeqList.Clear();
                            oStockOutMasterAdjustPrice.AddDetail(oStockOutDtlHand);
                            oStockOutMasterAdjustPrice.SumMoney += dDiffPrice * dQuantity;
                            #endregion
                            #region 入库蓝单
                            //明细
                            StockInDtl oRemainStockInDtlHand = CopyToStockInDtl(oStockInDtl);
                            oRemainStockInDtlHand.Id = null;
                            oRemainStockInDtlHand.Price = dDiffPrice;//价格
                            oRemainStockInDtlHand.ConfirmPrice = dDiffPrice;
                            oRemainStockInDtlHand.Quantity = 0;// dQuantity;// oStockOutDtlHand.Quantity;//数量
                            oRemainStockInDtlHand.RefQuantity = 0;
                            oRemainStockInDtlHand.BalQuantity = 0;
                            oRemainStockInDtlHand.ConfirmMoney = dDiffPrice * dQuantity;//oStockOutDtlHand.Quantity;
                            oRemainStockInDtlHand.Money = dDiffPrice * dQuantity;// oStockOutDtlHand.Quantity;//总额
                            oStockInMasterAdjustPrice.SumConfirmMoney += dQuantity * dDiffPrice;
                            oStockInMasterAdjustPrice.SumMoney += dQuantity * dDiffPrice;
                            oStockInMasterAdjustPrice.AddDetail(oRemainStockInDtlHand);
                            #endregion
                        }
                        #endregion
                        if (oStockRelation.RemainQuantity > 0)
                        {
                            #region 将库存做入库冲红
                            //StockIn oStockIn = oStockInDtl.Master as StockIn;
                            //StockInRed oStockInRedHand = CopyToStockInRed(oStockInDtl);
                            //StockInRedDtl oStockInRedDtlHand = oStockInRedHand.Details.ElementAt(0) as StockInRedDtl;
                            //#region 红单主表
                            //oStockInRedHand.Code = null;
                            //oStockInRedHand.CreateDate = DateTime.Now;
                            //oStockInRedHand.CreateMonth = stockInBalMaster.CreateMonth;//会计月
                            //oStockInRedHand.CreateYear = stockInBalMaster.CreateYear;//会计年 
                            //oStockInRedHand.Descript = oStockIn.Descript;
                            //oStockInRedHand.Id = null;
                            //oStockInRedHand.SumConfirmMoney = (oStockRelation.RemainQuantity * oStockRelation.Price);//计算红单的总价
                            //oStockInRedHand.SumMoney = oStockInRedHand.SumConfirmMoney;// 
                            //oStockInRedHand.SumQuantity = oStockRelation.RemainQuantity;//冲的物资数量
                            //oStockInRedHand.Descript = "[系统生成][调价入库冲红单]";
                            #endregion
                            #region 子表
                            StockInRedDtl oStockInRedDtlHand = CopyToStockInRedDtl(oStockInDtl);
                            oStockInRedDtlHand.ConfirmMoney = oStockRelation.RemainQuantity * oStockRelation.Price;
                            oStockInRedDtlHand.ConfirmPrice = oStockRelation.Price;// oStockInDtl.ConfirmPrice;
                            oStockInRedDtlHand.Id = null;
                            oStockInRedDtlHand.Money = -oStockRelation.RemainQuantity * oStockRelation.Price;
                            oStockInRedDtlHand.NewPrice = oStockRelation.Price;
                            oStockInRedDtlHand.Price = oStockRelation.Price;//价格                          
                            oStockInRedDtlHand.Quantity = -oStockRelation.RemainQuantity;//数量                              
                            oStockInRedDtlHand.RefQuantity = 0;
                            #endregion
                            oStockInRedMasterTall.SumConfirmMoney += (oStockRelation.RemainQuantity * oStockRelation.Price);//计算红单的总价
                            oStockInRedMasterTall.SumMoney = oStockInRedMasterTall.SumConfirmMoney;// 
                            oStockInRedMasterTall.SumQuantity = oStockRelation.RemainQuantity;//冲的物资数量
                            oStockInRedMasterTall.AddDetail(oStockInRedDtlHand);

                            #region 入库蓝单  新价格做蓝单
                            #region 入库蓝单主表
                            // StockIn oStockInHand = oStockInDtlTemp.Master as StockIn;
                            // StockIn oStockInHand = GetStockInByStockInDtlID(sStockInDtlID);
                            //StockIn oStockInHand = CopyToStockIn(oStockInDtl); //oStockInDtl.Master as StockIn;
                            //oStockInHand.CreateDate = DateTime.Now;
                            //oStockInHand.CreateYear = stockInBalMaster.CreateYear;
                            //oStockInHand.CreateMonth = stockInBalMaster.CreateMonth;
                            //oStockInHand.RealOperationDate = DateTime.Now;
                            //oStockInHand.Id = null;
                            //oStockInHand.SumQuantity = oStockRelation.RemainQuantity;
                            //oStockInHand.SumMoney = oStockRelation.RemainQuantity * oStockInBalDetail.Price;
                            //oStockInHand.SumConfirmMoney = oStockInHand.SumMoney;
                            //oStockInHand.Code = null;
                            //oStockInHand.Descript = "[系统生成][调价入库蓝单]";
                            #endregion
                            #region 入库蓝单明细
                            StockInDtl oStockInDtlHand = CopyToStockInDtl(oStockInDtl);

                            oStockInDtlHand.Id = null;
                            oStockInDtlHand.BalQuantity = 0;
                            oStockInDtlHand.ConfirmPrice = oStockInBalDetail.Price;
                            oStockInDtlHand.Price = oStockInBalDetail.Price;
                            oStockInDtlHand.ConfirmMoney = oStockInBalDetail.Price * oStockRelation.RemainQuantity;
                            oStockInDtlHand.Money = oStockInDtlHand.ConfirmMoney;
                            oStockInDtlHand.Quantity = oStockRelation.RemainQuantity;
                            oStockInMasterTall.SumQuantity += oStockRelation.RemainQuantity;
                            oStockInMasterTall.SumMoney += oStockRelation.RemainQuantity * oStockInBalDetail.Price;
                            oStockInMasterTall.SumConfirmMoney = oStockInMasterTall.SumMoney;
                            oStockInMasterTall.AddDetail(oStockInDtlHand);
                            // oStockInHand.Descript = "xulei";
                            //oStockInHand = SaveStockIn(oStockInHand, null);
                            //lstStockInTall.Add(oStockInHand);

                            #endregion
                            #endregion
                        }

                    }
                }
            }
        }
        /// <summary>
        /// 对调价单进行记账
        /// </summary>
        /// <param name="lstStock">调价的集合 入库红单  入库蓝单  出库调价单  入库调价单</param>
        /// <param name="oStockInBalMaster">结算单的单</param>
        /// <param name="oPersonInfo">结算人员</param>
        /// <returns></returns>
        [TransManager]
        public string TallyList(IList lstStock, StockInBalMaster oStockInBalMaster, PersonInfo oPersonInfo)
        {//入库红单  入库蓝单  出库调价单  入库调价单
            // bool bFlag = false;
            string sMsg = string.Empty;
            if (lstStock != null)
            {
                if (lstStock.Count > 0)
                {
                    StockInRed oStockInRed = lstStock[0] as StockInRed;
                    if (oStockInRed != null)
                    {
                        sMsg = Tally("StockInRed", oStockInRed.Id, oStockInRed.Code, oStockInBalMaster.CreateYear, oStockInBalMaster.CreateMonth, oPersonInfo, oStockInBalMaster.ProjectId);
                        if (!string.IsNullOrEmpty(sMsg))
                        {
                            UndoAdjustPrice(lstStock);//记账出现问题删除调价单
                            return sMsg;
                        }
                    }
                }
                if (lstStock.Count > 1)
                {
                    StockIn oStockIn = lstStock[1] as StockIn;
                    if (oStockIn != null)
                    {
                        sMsg = Tally("StockIn", oStockIn.Id, oStockIn.Code, oStockInBalMaster.CreateYear, oStockInBalMaster.CreateMonth, oPersonInfo, oStockInBalMaster.ProjectId);
                        if (!string.IsNullOrEmpty(sMsg))
                        {
                            UndoAdjustPrice(lstStock);//记账出现问题删除调价单
                            return sMsg;
                        }
                    }
                }
            }
            return sMsg;
        }
        /// <summary>
        /// 回滚调价单
        /// </summary>
        /// <param name="lstStock">调价的集合 入库红单  入库蓝单  出库调价单  入库调价单</param>
        [TransManager]
        public void UndoAdjustPrice(IList lstStock)
        {
            //入库红单  入库蓝单  出库调价单  入库调价单
            //回滚顺序  入库蓝单  入库红单 出库调价单 入库调价单
            if (lstStock != null && lstStock.Count > 0)
            {
                if (lstStock.Count > 1)
                {
                    StockIn oStockIn = lstStock[1] as StockIn;
                    if (oStockIn != null)
                    {
                        DeleteStockIn(oStockIn);
                    }
                }
                if (lstStock.Count > 0)
                {
                    StockInRed oStockInRed = lstStock[0] as StockInRed;
                    if (oStockInRed != null)
                    {
                        DeleteStockInRed(oStockInRed);
                    }
                }
                if (lstStock.Count > 2)
                {
                    StockOut oStockOut = lstStock[2] as StockOut;
                    if (oStockOut != null)
                    {
                        stockOutSrv.DeleteByDao(oStockOut);
                    }
                }
                if (lstStock.Count > 3)
                {
                    StockIn oStockIn = lstStock[3] as StockIn;
                    if (oStockIn != null)
                    {
                        DeleteStockIn(oStockIn);
                    }
                }
            }
        }
        /// <summary>
        /// 记账
        /// </summary>
        /// <param name="sBillType">单据类型</param>
        /// <param name="sBillID">单据id</param>
        /// <param name="sBillCode">单据code</param>
        /// <param name="iNowYear">会计年</param>
        /// <param name="iNowMonth">会计月</param>
        /// <param name="oPerson">操作人</param>
        /// <param name="sProjectID">项目id</param>
        /// <returns></returns>
        [TransManager]
        public string Tally(string sBillType, string sBillID, string sBillCode, int iNowYear, int iNowMonth, PersonInfo oPerson, string sProjectID)
        {
            string sMsg = string.Empty;
            try
            {
                IList billIdList = new ArrayList();
                billIdList.Add(sBillID);

                IList billCodeList = new ArrayList();
                //billCodeList.Add(curBillMaster.Code);
                billCodeList.Add(sBillCode);
                Hashtable hashBillId = new Hashtable();
                //hashBillId.Add("StockIn", billIdList);
                hashBillId.Add(sBillType, billIdList);
                Hashtable hashBillCode = new Hashtable();
                hashBillCode.Add(sBillType, billCodeList);

                Hashtable tallyResult = TallyStockIn(hashBillId, hashBillCode, iNowYear, iNowMonth, oPerson.Id, oPerson.Name, sProjectID);
                if (tallyResult != null)
                {
                    sMsg = (string)tallyResult["err"];
                }
            }
            catch (Exception ex)
            {
                sMsg += ex.Message;
            }
            return sMsg;
        }
        /// <summary>
        /// 复制入库单明细到入库红单明细中
        /// </summary>
        /// <param name="oStockInDtl"></param>
        /// <returns></returns>
        public StockInRedDtl CopyToStockInRedDtl(StockInDtl oStockInDtl)
        {
            StockInRedDtl oStockInRedDtlHand = new StockInRedDtl();
            if (oStockInDtl != null)
            {
                #region 子表
                oStockInRedDtlHand.ConcreteBalDtlID = oStockInDtl.ConcreteBalDtlID;
                oStockInRedDtlHand.DiagramNumber = oStockInDtl.DiagramNumber;
                oStockInRedDtlHand.ForwardDetailId = oStockInDtl.ForwardDetailId;
                oStockInRedDtlHand.Id = null;
                oStockInRedDtlHand.Master = null;
                oStockInRedDtlHand.MaterialCode = oStockInDtl.MaterialCode;
                oStockInRedDtlHand.MaterialGrade = oStockInDtl.MaterialGrade;
                oStockInRedDtlHand.MaterialName = oStockInDtl.MaterialName;
                oStockInRedDtlHand.MaterialResource = oStockInDtl.MaterialResource;
                oStockInRedDtlHand.MaterialSpec = oStockInDtl.MaterialSpec;
                oStockInRedDtlHand.MaterialStuff = oStockInDtl.MaterialStuff;
                oStockInRedDtlHand.MatStandardUnit = oStockInDtl.MatStandardUnit;
                oStockInRedDtlHand.MatStandardUnitName = oStockInDtl.MatStandardUnitName;
                oStockInRedDtlHand.ProfessionalCategory = oStockInDtl.ProfessionalCategory;

                //oStockInRedDtlHand.QuantityTemp  = oStockInDtlTemp
                //oStockInRedDtlHand.NewPrice = oStockInDtlTemp.Price;
                oStockInRedDtlHand.RefQuantity = 0;
                oStockInRedDtlHand.TempData = oStockInDtl.TempData;
                oStockInRedDtlHand.TempData1 = oStockInDtl.TempData1;
                oStockInRedDtlHand.TempData2 = oStockInDtl.TempData2;
                oStockInRedDtlHand.UsedPart = oStockInDtl.UsedPart;
                oStockInRedDtlHand.UsedPartName = oStockInDtl.UsedPartName;
                oStockInRedDtlHand.UsedPartSysCode = oStockInDtl.UsedPartSysCode;
                oStockInRedDtlHand.ForwardDetailId = oStockInDtl.Id;
                #endregion
            }

            return oStockInRedDtlHand;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oStockInDtl"></param>
        /// <param name="oStockInBalMaster"></param>
        /// <returns></returns>
        public StockIn CopyToStockInMaster(StockInDtl oStockInDtl, StockInBalMaster oStockInBalMaster)
        {
            StockIn oStockInHand = new StockIn();

            if (oStockInDtl != null)
            {
                StockIn oStockIn = oStockInDtl.Master as StockIn;
                if (oStockIn != null)
                {
                    #region 主表
                    
                    oStockInHand.AuditRoles = oStockIn.AuditRoles;
                    oStockInHand.Audits = oStockIn.Audits;
                    oStockInHand.AuditYear = oStockIn.AuditYear;
                    oStockInHand.ClassifyCode = oStockIn.ClassifyCode;
                    oStockInHand.Code = null;
                    oStockInHand.ContractNo = oStockIn.ContractNo;
                    oStockInHand.CurrencyType = oStockIn.CurrencyType;
                    oStockInHand.Descript = oStockIn.Descript;
                    oStockInHand.DocState = oStockIn.DocState;
                    oStockInHand.ExchangeRate = oStockIn.ExchangeRate;
                    oStockInHand.IsTally = 1;
                    oStockInHand.HandleOrg = oStockIn.HandleOrg;
                    oStockInHand.HandlePerson = oStockIn.HandlePerson;
                    oStockInHand.HandlePersonName = oStockIn.HandlePersonName;
                    oStockInHand.HandOrgLevel = oStockIn.HandOrgLevel;
                    oStockInHand.Id = null;
                    oStockInHand.InvalidateDate = oStockIn.InvalidateDate;
                    oStockInHand.InvalidateMonth = oStockIn.InvalidateMonth;
                    oStockInHand.InvalidatePerson = oStockIn.InvalidatePerson;
                    oStockInHand.InvalidateYear = oStockIn.InvalidateYear;
                    oStockInHand.IsFinished = oStockIn.IsFinished;
                    oStockInHand.IsSelect = oStockIn.IsSelect;
                    //oStockInRedHand.IsTally = oStockIn.IsTally;
                    oStockInHand.JBR = oStockIn.JBR;
                    oStockInHand.LastModifyDate = DateTime.Now;
                    oStockInHand.MatCatName = oStockIn.MatCatName;
                    oStockInHand.MaterialCategory = oStockIn.MaterialCategory;
                    oStockInHand.OperOrgInfo = oStockIn.OperOrgInfo;
                    oStockInHand.OperOrgInfoName = oStockIn.OperOrgInfoName;
                    oStockInHand.OpgSysCode = oStockIn.OpgSysCode;
                    oStockInHand.PrintTimes = oStockIn.PrintTimes;
                    oStockInHand.ProfessionCategory = oStockIn.ProfessionCategory;
                    oStockInHand.ProjectId = oStockIn.ProjectId;
                    oStockInHand.ProjectName = oStockIn.ProjectName;
                    oStockInHand.RealOperationDate = DateTime.Now;
                    oStockInHand.Special = oStockIn.Special;
                    oStockInHand.StockInManner = oStockIn.StockInManner;
                   
                    oStockInHand.SupplyOrderCode = oStockIn.SupplyOrderCode;
                    oStockInHand.TheStationCategory = oStockIn.TheStationCategory;
                    oStockInHand.TheSupplierName = oStockIn.TheSupplierName;
                    oStockInHand.TheSupplierRelationInfo = oStockIn.TheSupplierRelationInfo;
                    oStockInHand.Details.Clear();
                    oStockInHand.ForwardBillCode = oStockInBalMaster.Code;
                    oStockInHand.ForwardBillId = oStockInBalMaster.Id;
                    oStockInHand.SubmitDate = oStockInBalMaster.SubmitDate;
                    oStockInHand.AuditDate = oStockInBalMaster.AuditDate;
                    oStockInHand.AuditMonth = oStockInBalMaster.AuditMonth;
                    oStockInHand.AuditPerson = oStockInBalMaster.AuditPerson;
                    oStockInHand.AuditPersonName = oStockInBalMaster.AuditPersonName;
                    oStockInHand.CreateDate = oStockInBalMaster.CreateDate;
                    oStockInHand.CreateMonth = oStockInBalMaster.CreateMonth;
                    oStockInHand.CreateYear = oStockInBalMaster.CreateYear;
                    oStockInHand.CreatePerson = oStockInBalMaster.CreatePerson;
                    oStockInHand.CreatePersonName = oStockInBalMaster.CreatePersonName;

                    #endregion
                }
            }

            return oStockInHand;
        }
        public StockInRed CopyToStockInRedMaster(StockInDtl oStockInDtl, StockInBalMaster oStockInBalMaster)
        {
            StockInRed oStockInRedHand = new StockInRed();

            if (oStockInDtl != null)
            {
                StockIn oStockIn = oStockInDtl.Master as StockIn;
                if (oStockIn != null)
                {
                    #region 红单主表
                    oStockInRedHand.AuditDate = oStockIn.AuditDate;
                    oStockInRedHand.AuditMonth = oStockIn.AuditMonth;
                    oStockInRedHand.AuditPerson = oStockIn.AuditPerson;
                    oStockInRedHand.AuditPersonName = oStockIn.AuditPersonName;
                    oStockInRedHand.AuditRoles = oStockIn.AuditRoles;
                    oStockInRedHand.Audits = oStockIn.Audits;
                    oStockInRedHand.AuditYear = oStockIn.AuditYear;
                    oStockInRedHand.ClassifyCode = oStockIn.ClassifyCode;
                    oStockInRedHand.Code = null;
                    oStockInRedHand.ContractNo = oStockIn.ContractNo;
                    oStockInRedHand.CreateDate = DateTime.Now;
                    oStockInRedHand.CreatePerson = oStockIn.CreatePerson;
                    oStockInRedHand.CreatePersonName = oStockIn.CreatePersonName;
                    oStockInRedHand.CurrencyType = oStockIn.CurrencyType;
                    oStockInRedHand.Descript = oStockIn.Descript;
                    oStockInRedHand.Details.Clear();
                    oStockInRedHand.DocState = oStockIn.DocState;
                    oStockInRedHand.ExchangeRate = oStockIn.ExchangeRate;
                    oStockInRedHand.ForRedType = EnumForRedType.冲数量;
                    oStockInRedHand.ForwardBillCode = oStockIn.Code;
                    oStockInRedHand.ForwardBillId = oStockIn.Id;
                    oStockInRedHand.ForwardBillType = "StockIn";
                    oStockInRedHand.HandleOrg = oStockIn.HandleOrg;
                    oStockInRedHand.HandlePerson = oStockIn.HandlePerson;
                    oStockInRedHand.HandlePersonName = oStockIn.HandlePersonName;
                    oStockInRedHand.HandOrgLevel = oStockIn.HandOrgLevel;
                    oStockInRedHand.Id = null;
                    oStockInRedHand.InvalidateDate = oStockIn.InvalidateDate;
                    oStockInRedHand.InvalidateMonth = oStockIn.InvalidateMonth;
                    oStockInRedHand.InvalidatePerson = oStockIn.InvalidatePerson;
                    oStockInRedHand.InvalidateYear = oStockIn.InvalidateYear;
                    oStockInRedHand.IsFinished = oStockIn.IsFinished;
                    oStockInRedHand.IsSelect = oStockIn.IsSelect;
                    //oStockInRedHand.IsTally = oStockIn.IsTally;
                    oStockInRedHand.JBR = oStockIn.JBR;
                    oStockInRedHand.LastModifyDate = DateTime.Now;
                    oStockInRedHand.MatCatName = oStockIn.MatCatName;
                    oStockInRedHand.MaterialCategory = oStockIn.MaterialCategory;
                    oStockInRedHand.OperOrgInfo = oStockIn.OperOrgInfo;
                    oStockInRedHand.OperOrgInfoName = oStockIn.OperOrgInfoName;
                    oStockInRedHand.OpgSysCode = oStockIn.OpgSysCode;
                    oStockInRedHand.PrintTimes = oStockIn.PrintTimes;
                    oStockInRedHand.ProfessionCategory = oStockIn.ProfessionCategory;
                    oStockInRedHand.ProjectId = oStockIn.ProjectId;
                    oStockInRedHand.ProjectName = oStockIn.ProjectName;
                    oStockInRedHand.RealOperationDate = DateTime.Now;
                    oStockInRedHand.Special = oStockIn.Special;
                    oStockInRedHand.StockInManner = oStockIn.StockInManner;
                    oStockInRedHand.SubmitDate = DateTime.Now;
                    oStockInRedHand.SumMoney = oStockInRedHand.SumConfirmMoney;// 
                    oStockInRedHand.SupplyOrderCode = oStockIn.SupplyOrderCode;
                    oStockInRedHand.Temp1 = oStockIn.Temp1;
                    oStockInRedHand.Temp2 = oStockIn.Temp2;
                    oStockInRedHand.Temp3 = oStockIn.Temp3;
                    oStockInRedHand.Temp4 = oStockIn.Temp4;
                    oStockInRedHand.Temp5 = oStockIn.Temp5;
                    oStockInRedHand.TheStationCategory = oStockIn.TheStationCategory;
                    oStockInRedHand.TheSupplierName = oStockIn.TheSupplierName;
                    oStockInRedHand.TheSupplierRelationInfo = oStockIn.TheSupplierRelationInfo;
                    oStockInRedHand.Details.Clear();
                    oStockInRedHand.ForwardBillCode = oStockIn.Code;
                    oStockInRedHand.ForwardBillId = oStockIn.Id;
                    oStockInRedHand.ForwardBillType = null;
                    oStockInRedHand.CreateDate = DateTime.Now;
                    oStockInRedHand.CreateMonth = oStockInBalMaster.CreateMonth;
                    oStockInRedHand.CreateYear = oStockInBalMaster.CreateYear;
                    oStockInRedHand.CreatePerson = oStockInBalMaster.CreatePerson;
                    oStockInRedHand.CreatePersonName = oStockInBalMaster.CreatePersonName;
                    #endregion
                }
            }

            return oStockInRedHand;
        }
        public StockInDtl CopyToStockInDtl(StockInDtl oStockInDtl)
        {
            // StockIn oStockInHandle = new StockIn();
            StockInDtl oStockInDtlHandle = new StockInDtl();
            if (oStockInDtl != null)
            {

                oStockInDtlHandle.AppearanceQuality = oStockInDtl.AppearanceQuality;
                oStockInDtlHandle.BalQuantity = oStockInDtl.BalQuantity;
                oStockInDtlHandle.Calculate = oStockInDtl.Calculate;
                oStockInDtlHandle.ConcreteBalDtlID = oStockInDtl.ConcreteBalDtlID;
                oStockInDtlHandle.Descript = oStockInDtl.Descript;
                oStockInDtlHandle.DiagramNumber = oStockInDtl.DiagramNumber;
                oStockInDtlHandle.ForwardDetailId = oStockInDtl.ForwardDetailId;
                oStockInDtlHandle.Id = null;
                oStockInDtlHandle.MaterialCode = oStockInDtl.MaterialCode;
                oStockInDtlHandle.MaterialGrade = oStockInDtl.MaterialGrade;
                oStockInDtlHandle.MaterialName = oStockInDtl.MaterialName;
                oStockInDtlHandle.MaterialResource = oStockInDtl.MaterialResource;
                oStockInDtlHandle.MaterialSpec = oStockInDtl.MaterialSpec;
                oStockInDtlHandle.MaterialStuff = oStockInDtl.MaterialStuff;
                oStockInDtlHandle.MatStandardUnit = oStockInDtl.MatStandardUnit;
                oStockInDtlHandle.MatStandardUnitName = oStockInDtl.MatStandardUnitName;
                oStockInDtlHandle.OriginalContractNo = oStockInDtl.OriginalContractNo;
                oStockInDtlHandle.ProfessionalCategory = oStockInDtl.ProfessionalCategory;
                oStockInDtlHandle.SupplyOrderDetailId = oStockInDtl.SupplyOrderDetailId;
                oStockInDtlHandle.UsedPart = oStockInDtl.UsedPart;
                oStockInDtlHandle.UsedPartName = oStockInDtl.UsedPartName;
                oStockInDtlHandle.UsedPartSysCode = oStockInDtl.UsedPartSysCode;

            }
            //oStockInHandle.AddDetail(oStockInDtlHandle);
            return oStockInDtlHandle;
        }

        public StockOutDtl CopyToStockOutDtl(StockInDtl oStockInDtl)
        {
            StockOutDtl oStockOutDtlHandle = new StockOutDtl();
            if (oStockInDtl != null)
            {
                oStockOutDtlHandle.ConcreteBalDtlID = oStockInDtl.ConcreteBalDtlID;
                oStockOutDtlHandle.Descript = oStockInDtl.Descript;
                oStockOutDtlHandle.DiagramNumber = oStockInDtl.DiagramNumber;
                oStockOutDtlHandle.ForwardDetailId = oStockInDtl.ForwardDetailId;
                oStockOutDtlHandle.IsOver = oStockInDtl.IsOver;
                oStockOutDtlHandle.IsSelect = oStockInDtl.IsSelect;
                oStockOutDtlHandle.MaterialCode = oStockInDtl.MaterialCode;
                oStockOutDtlHandle.MaterialGrade = oStockInDtl.MaterialGrade;
                oStockOutDtlHandle.MaterialName = oStockInDtl.MaterialName;
                oStockOutDtlHandle.MaterialResource = oStockInDtl.MaterialResource;
                oStockOutDtlHandle.MaterialSpec = oStockInDtl.MaterialSpec;
                oStockOutDtlHandle.MaterialStuff = oStockInDtl.MaterialStuff;
                oStockOutDtlHandle.MatStandardUnit = oStockInDtl.MatStandardUnit;
                oStockOutDtlHandle.MatStandardUnitName = oStockInDtl.MatStandardUnitName;
                oStockOutDtlHandle.ProfessionalCategory = oStockInDtl.ProfessionalCategory;
                CostAccountSubject subject = new CostAccountSubject();
                subject.Id = TransUtil.ConStockOutSubjectId;
                subject.Name = TransUtil.ConStockOutSubjectName;
                subject.SysCode = TransUtil.ConStockOutSubjectSyscode;
                oStockOutDtlHandle.SubjectGUID = subject;
                oStockOutDtlHandle.SubjectName = subject.Name;
                oStockOutDtlHandle.SubjectSysCode = subject.SysCode;
                oStockOutDtlHandle.UsedPart = oStockInDtl.UsedPart;
                oStockOutDtlHandle.UsedPartName = oStockInDtl.UsedPartName;
                oStockOutDtlHandle.UsedPartSysCode = oStockInDtl.UsedPartSysCode;
            }
            return oStockOutDtlHandle;
        }

        public StockOut CopyToStockOutMaster(StockInDtl oStockInDtl, StockInBalMaster oStockInBalMaster)
        {
            StockOut oStockOutHandle = new StockOut();

            if (oStockInDtl != null)
            {
                StockIn oStockIn = oStockInDtl.Master as StockIn;
                if (oStockIn != null)
                {                   
                    oStockOutHandle.AuditRoles = oStockIn.AuditRoles;
                    oStockOutHandle.Audits = oStockIn.Audits;
                    oStockOutHandle.ClassifyCode = oStockIn.ClassifyCode;
                    oStockOutHandle.Code = null;
                    oStockOutHandle.ConcreteBalID = oStockIn.ConcreteBalID;
                    oStockOutHandle.CurrencyType = oStockIn.CurrencyType;
                    oStockOutHandle.DocState = oStockIn.DocState;
                    oStockOutHandle.ExchangeRate = oStockIn.ExchangeRate;
                    oStockOutHandle.ForwardBillCode = oStockInBalMaster.Code;
                    oStockOutHandle.ForwardBillId = oStockInBalMaster.Id;
                    oStockOutHandle.HandleOrg = oStockIn.HandleOrg;
                    oStockOutHandle.HandlePerson = oStockIn.HandlePerson;
                    oStockOutHandle.HandlePersonName = oStockIn.HandlePersonName;
                    oStockOutHandle.HandOrgLevel = oStockIn.HandOrgLevel;
                    oStockOutHandle.Id = null;
                    oStockOutHandle.JBR = oStockIn.JBR;
                    oStockOutHandle.MatCatName = oStockIn.MatCatName;
                    oStockOutHandle.MaterialCategory = oStockIn.MaterialCategory;
                    oStockOutHandle.OperOrgInfo = oStockIn.OperOrgInfo;
                    oStockOutHandle.OperOrgInfoName = oStockIn.OperOrgInfoName;
                    oStockOutHandle.IsTally = 1;
                    oStockOutHandle.OpgSysCode = oStockIn.OpgSysCode;
                    oStockOutHandle.ProfessionCategory = oStockIn.ProfessionCategory;
                    oStockOutHandle.ProjectId = oStockIn.ProjectId;
                    oStockOutHandle.ProjectName = oStockIn.ProjectName;
                    oStockOutHandle.RealOperationDate = DateTime.Now;
                    oStockOutHandle.RefQuantity = 0;
                    oStockOutHandle.Special = oStockIn.Special;
                    oStockOutHandle.StockOutManner = EnumStockInOutManner.领料出库;
                    oStockOutHandle.SumMoney = 0;
                    oStockOutHandle.SumQuantity = 0;

                    oStockOutHandle.TheStationCategory = oStockIn.TheStationCategory;
                    oStockOutHandle.TheSupplierName = oStockIn.TheSupplierName;
                    oStockOutHandle.TheSupplierRelationInfo = oStockIn.TheSupplierRelationInfo;
                    oStockOutHandle.AuditDate = oStockInBalMaster.AuditDate;
                    oStockOutHandle.AuditYear = oStockInBalMaster.AuditYear;
                    oStockOutHandle.AuditMonth = oStockInBalMaster.AuditMonth;
                    oStockOutHandle.AuditPerson = oStockInBalMaster.AuditPerson;
                    oStockOutHandle.AuditPersonName = oStockInBalMaster.AuditPersonName;
                    oStockOutHandle.CreateDate = oStockInBalMaster.CreateDate;
                    oStockOutHandle.CreateMonth = oStockInBalMaster.CreateMonth;
                    oStockOutHandle.CreateYear = oStockInBalMaster.CreateYear;
                    oStockOutHandle.CreatePerson = oStockInBalMaster.CreatePerson;
                    oStockOutHandle.CreatePersonName = oStockInBalMaster.CreatePersonName;
                }
            }
            return oStockOutHandle;
        }

        [TransManager]
        /// <summary>
        /// 入库蓝单的记账
        /// </summary>
        // <param name="sBillType">入库单据类型</param>
        // <param name="sID">单据ID</param>
        /// <param name="sCode">单据Code</param>
        /// <param name="iKJYear">会计年</param>
        /// <param name="iKJMonth">会计月</param>
        /// <param name="sPersonID">操作者的ID</param>
        /// <param name="sPersonName">操作者的名字</param>
        /// <param name="sProjectID">会计的项目</param>
        /// <returns></returns>
        public bool TallyStockIn(string sBillType, string sID, string sCode, int iKJYear, int iKJMonth, Application.Resource.PersonAndOrganization.HumanResource.RelateClass.PersonInfo oPerson, string sProjectID)
        {
            bool bFlag = false;
            try
            {
                IList billIdList = new ArrayList();
                billIdList.Add(sID);

                IList billCodeList = new ArrayList();
                billCodeList.Add(sCode);

                Hashtable hashBillId = new Hashtable();
                hashBillId.Add(sBillType, billIdList);

                Hashtable hashBillCode = new Hashtable();
                hashBillCode.Add(sBillType, billCodeList);

                Hashtable tallyResult = TallyStockIn(hashBillId, hashBillCode, iKJYear, iKJMonth, oPerson.Id, oPerson.Name, sProjectID);
                if (tallyResult != null)
                {
                    string errMsg = (string)tallyResult["err"];
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        bFlag = false;
                    }
                    else
                    {
                        // oStockIn.IsTally = 1;
                        bFlag = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                bFlag = false;
            }
            return bFlag;
        }
        [TransManager]
        public IList GetStockInDtlByID(string sStockInDtlID)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", sStockInDtlID));
            //oQuery.AddCriterion( Expression.Eq("Master.TheStockInOutKind", 0));
            //oQuery.AddFetchMode("Master.MaterialCategory", FetchMode.Eager);
            //oQuery.AddFetchMode("Master.MaterialCategory", FetchMode.Eager);

            return dao.ObjectQuery(typeof(StockInDtl), oQuery);

        }
        [TransManager]
        /// <summary>
        /// 根据明细的ID获取入库主表的
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public StockIn GetStockInByStockInDtlID(string sID)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            StockIn oStockIn = null;
            string sql = @"select distinct t.id from thd_stkstockin t join thd_stkstockindtl t1 on t.id=t1.parentid and t1.id='{0}'";
            sql = string.Format(sql, sID);

            command.CommandText = sql;
            string sStockOutID = command.ExecuteScalar() as string;
            if (!string.IsNullOrEmpty(sStockOutID))
            {
                ObjectQuery oQuery = new ObjectQuery();
                oQuery.AddCriterion(Expression.Eq("Id", sStockOutID));
                oQuery.AddFetchMode("MaterialCategory", FetchMode.Eager);
                IList list = dao.ObjectQuery(typeof(StockIn), oQuery);
                if (list != null && list.Count > 0)
                {
                    oStockIn = list[0] as StockIn;
                }
            }
            return oStockIn;

        }

        /// <summary>
        /// 更新出库单
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [TransManager]
        private bool UpdateStockOut(string stockInDtlId, decimal price)
        {
            IList list = StockOutSrv.GetStockOutDtlSeqByStockInDtlId(stockInDtlId);//查询出库时序
            if (list == null || list.Count == 0) return true;

            IList StockOutDtlIdList = new ArrayList();//明细Id集合
            IList StockOutIdList = new ArrayList();//出库主表ID集合
            foreach (StockOutDtlSeq stockOutDtlSeq in list)
            {
                //更新出库时序表的单价
                stockOutDtlSeq.Price = price;
                if (!StockOutDtlIdList.Contains(stockOutDtlSeq.StockOutDtl.Id))
                {
                    StockOutDtlIdList.Add(stockOutDtlSeq.StockOutDtl.Id);
                }
                dao.SaveOrUpdate(stockOutDtlSeq);
            }

            //更新出库明细金额、单价
            if (StockOutDtlIdList.Count > 0)
            {
                foreach (string stockOutDtlId in StockOutDtlIdList)
                {
                    StockOutDtl stockOutDtl = StockOutSrv.GetStockOutDtlByIdWithDetails(stockOutDtlId);
                    if (stockOutDtl == null)
                    {
                        //StockMoveOutDtl stockMoveOut=
                    }
                    if (!StockOutIdList.Contains(stockOutDtl.Master.Id))
                    {
                        StockOutIdList.Add(stockOutDtl.Master.Id);
                    }
                    decimal money = 0M;
                    foreach (StockOutDtlSeq stockOutDtlSeq in stockOutDtl.StockOutDtlSeqList)
                    {
                        money += stockOutDtlSeq.Price * stockOutDtlSeq.Quantity;
                    }
                    stockOutDtl.Money = money;
                    stockOutDtl.Price = stockOutDtl.Money / stockOutDtl.Quantity;
                    dao.SaveOrUpdate(stockOutDtl);
                }
            }

            //更新出库单总金额
            if (StockOutIdList.Count > 0)
            {
                foreach (string stockOutId in StockOutIdList)
                {
                    StockOut stockOut = StockOutSrv.GetStockOutById(stockOutId);
                    decimal money = 0M;
                    foreach (StockOutDtl stockOutDtl in stockOut.Details)
                    {
                        money += stockOutDtl.Money;
                    }
                    stockOut.SumMoney = money;
                    dao.SaveOrUpdate(stockOut);
                }
            }
            return true;
        }

        /// <summary>
        /// 更新相关的入库和出库单
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        private bool UpdateStockOutBySql(string stockInDtlId, decimal price)
        {
            if (string.IsNullOrEmpty(stockInDtlId)) return true;

            ISession session = CallContext.GetData("nhsession") as ISession;
            System.Data.OracleClient.OracleConnection conn = session.Connection as System.Data.OracleClient.OracleConnection;
            System.Data.OracleClient.OracleCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            try
            {
                //更新入库红单明细的单价和金额
                string sql1 = @"update thd_stkstockindtl a set 
                  a.price= :price,
                  a.money=:price*a.quantity
                  where a.forwarddetailid =:stockInDtlId ";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("stockInDtlId", stockInDtlId);
                command.Parameters.AddWithValue("price", price);
                command.CommandText = sql1;
                command.ExecuteNonQuery();

                //更新入库红单的主表金额
                sql1 = @"update thd_stkstockin a set 
                  a.summoney=(select sum(money) from thd_stkstockindtl b where a.id=b.parentid )
                  where a.id in (select parentid from thd_stkstockindtl  where forwarddetailid=:stockInDtlId)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("stockInDtlId", stockInDtlId);
                command.CommandText = sql1;
                command.ExecuteNonQuery();

                //更新出库时序的单价
                sql1 = "update thd_stkstockoutdtlseq set price=:price where stockInDtlId=:stockInDtlId";
                command.Parameters.AddWithValue("price", price);
                command.Parameters.AddWithValue("stockInDtlId", stockInDtlId);
                command.CommandText = sql1;
                command.ExecuteNonQuery();

                //更新出库明细金额、单价
                sql1 = @"update thd_stkstockoutdtl a set 
                  a.money=(select nvl(sum(b.price*b.quantity),0) from thd_stkstockoutdtlseq b where a.id=b.stockoutdtlid),
                  a.price=(select nvl(sum(b.price*b.quantity),0) from thd_stkstockoutdtlseq b where a.id=b.stockoutdtlid)/a.quantity
                  where a.id in (select stockOutDtlId from thd_stkstockoutdtlseq 
                  where stockInDtlId=:stockInDtlId)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("stockInDtlId", stockInDtlId);
                command.CommandText = sql1;
                command.ExecuteNonQuery();

                //更新出库单总金额
                sql1 = @"update thd_stkstockOut a set 
                  a.summoney=(select sum(money) from thd_stkstockOutDtl b where a.id=b.parentid )
                  where a.id in (select parentid from thd_stkstockOutDtl where id in 
                  (select stockOutDtlId from thd_stkstockoutdtlseq 
                  where (stockInDtlId=:stockInDtlId) ))";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("stockInDtlId", stockInDtlId);
                command.CommandText = sql1;
                command.ExecuteNonQuery();

                //更新出库单对应出库红单明细的单价和金额
                sql1 = @"update thd_stkstockoutdtl a set 
                  a.price=(select b.price from thd_stkstockoutdtl b where b.id=a.forwarddetailid),
                  a.money=(select b.price*a.quantity from thd_stkstockoutdtl b where b.id=a.forwarddetailid)
                  where a.forwarddetailid in (select stockOutDtlId from thd_stkstockoutdtlseq 
                  where stockInDtlId=:stockInDtlId)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("stockInDtlId", stockInDtlId);
                command.CommandText = sql1;
                command.ExecuteNonQuery();

                //更新出库红单主表总金额
                sql1 = @"update thd_stkstockOut a set 
                  a.summoney=(select sum(money) from thd_stkstockOutDtl b where a.id=b.parentid )
                  where a.id in (select parentid from thd_stkstockOutDtl  where forwarddetailid in 
                  (select stockOutDtlId from thd_stkstockoutdtlseq 
                  where (stockInDtlId=:stockInDtlId) ))";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("stockInDtlId", stockInDtlId);
                command.CommandText = sql1;
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// 根据入库明细Id更新库存表单价、金额
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <param name="price"></param>
        [TransManager]
        private bool UpdateStockRelation(string stockInDtlId, decimal price)
        {
            IList list = GetStockRelationByStockInDtlId(stockInDtlId);
            if (list == null || list.Count == 0) return true;
            foreach (StockRelation sr in list)
            {
                sr.Price = price;
                sr.Money = sr.Price * sr.Quantity;
                sr.RemainMoney = sr.Price * sr.RemainQuantity;
                return Dao.SaveOrUpdate(sr);
            }
            return false;
        }

        /// <summary>
        /// 根据入库明细Id更新库存表单价、金额(2014-07-21)
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <param name="price"></param>
        [TransManager]
        private bool UpdateStockRelationByNew(string stockInDtlId, decimal price)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = "update thd_stkstockrelation t set t.price = " + price + ",t.money = t.quantity *" + price + ",t.remainmoney = t.remainquantity * "
                        + price + " where t.stockindtlid = '" + stockInDtlId + "'";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            return true;
        }

        /// <summary>
        ///  更新入库单总金额(2014-07-21)
        /// </summary>
        /// <param name="stockInDtlId"></param>
        /// <param name="price"></param>
        [TransManager]
        private bool UpdateStockInSummoney(string stockInId, decimal sumMoney)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = "update thd_stkstockin t1 set t1.summoney = " + sumMoney + "  where t1.id = '" + stockInId + "'";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            return true;
        }

        #endregion

        [TransManager]
        public bool DeleteStockInBalMaster(StockInBalMaster obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (StockInBalDetail dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    foreach (StockInBalDetail_ForwardDetail forObj in dtl.ForwardDetails)
                    {
                        if (forObj.Id != null)
                        {
                            StockInDtl forwardDtl = GetStockInDtl(forObj.ForwardDetailId);
                            forwardDtl.BalQuantity = forwardDtl.BalQuantity - Math.Abs(dtl.Quantity);
                            if (forwardDtl.BalQuantity < 0)
                            {
                                forwardDtl.BalQuantity = 0;
                            }
                            dao.Save(forwardDtl);
                        }
                    }
                }
            }
            return dao.Delete(obj);
        }

        /// <summary>
        /// 根据ID查询验收结算单明细
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        public StockInBalDetail GetStockInBalDetail(string detailId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", detailId));
            oq.AddFetchMode("ForwardDetails", FetchMode.Eager);
            oq.AddFetchMode("Master",FetchMode.Eager);
            IList list = dao.ObjectQuery(typeof(StockInBalDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockInBalDetail;
            }
            return null;
        }
        /// <summary>
        /// 根据 项目名称 单据类型 单据ID获取没有记账的单据
        /// </summary>
        /// <param name="sProjectName"></param>
        /// <param name="sCode"></param>
        /// <returns></returns>
        public DataSet QueryListStockInNotHS(string sProjectName, string sCode)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sSQL="select t.id ,t.code,t.summoney,t.sumquantity,t.createpersonname,t.realoperationdate,to_char(t.createdate,'YYYY-MM-DD')createdate,t.descript ,t.projectid,t.projectname  from thd_stkstockin t where t.projectname='{0}' and t.code='{1}'";
            sSQL = string.Format(sSQL, sProjectName, sCode);
            command.CommandText = sSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }

        public bool UpdateStockInNotHS(string sID, DateTime time, string sDescript, int iYear, int iMonth)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sSQL = "update thd_stkstockin t  set t.createdate =to_date('{1}','YYYY-MM-DD'), t.descript='{2}',t.createyear={3},t.createmonth={4} where t.id='{0}'";
            string sSQLTemp = string.Format(sSQL, sID, time.ToShortDateString(), sDescript, iYear, iMonth);
            command.CommandText = sSQLTemp;
            command.ExecuteNonQuery();
            return true;
        }
        public DataSet QueryListStockInBalNotHS(string sProjectName, string sCode)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sSQL = "select t.id ,t.code,t.summoney,t.sumquantity,t.createpersonname,t.realoperationdate,to_char(t.createdate,'YYYY-MM-DD')createdate,t.descript ,t.projectid,t.projectname  from thd_stockinbalmaster t where t.projectname='{0}' and t.code='{1}'";
            sSQL = string.Format(sSQL, sProjectName, sCode);
            command.CommandText = sSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        public bool UpdateStockInBalNotHS(string sID, DateTime time, string sDescript, int iYear, int iMonth)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sSQL = "update thd_stockinbalmaster t  set t.createdate =to_date('{1}','YYYY-MM-DD'), t.descript='{2}',t.createyear={3},t.createmonth={4} where t.id='{0}'";
            string sSQLTemp = string.Format(sSQL, sID, time.ToShortDateString(), sDescript, iYear, iMonth);
            command.CommandText = sSQLTemp;
            command.ExecuteNonQuery();
            return true;
        }
        /// <summary>
        /// 验收结算单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet StockInBalQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = @"SELECT t1.Id,t1.Code,t1.SupplierRelationName,t1.STATE,t1.createdate,t1.createpersonname,t1.Descript,
                            t2.MaterialCode,t2.MaterialName,t1.TheStockInOutKind,
                            t2.MaterialSpec,t2.Quantity,t2.Price,t2.MONEY,t2.MatStandardUnitName,t2.ProfessionalCategory,t1.SumQuantity,t1.SumMoney,t2.DiagramNumber
                            FROM THD_StockInBalMaster t1 inner join THD_StockInBalDetail t2
                            ON t1.id=t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.Code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }
        #endregion

        #region 验收结算单红单方法

        /// <summary>
        /// 查询验收结算单红单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStockInBalRedMaster(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(StockInBalRedMaster), oq);
        }

        /// <summary>
        /// 根据Code查询验收结算单红单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public StockInBalRedMaster GetStockInBalRedMasterByCode(string code, string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            IList list = GetStockInBalRedMaster(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockInBalRedMaster;
            }
            return null;
        }

        /// <summary>
        /// 根据ID查询验收结算单红单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StockInBalRedMaster GetStockInBalRedMasterById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockInBalRedMaster(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StockInBalRedMaster;
            }
            return null;
        }

        /// <summary>
        /// 保存验收结算单红单
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="movedDtlList"></param>
        /// <returns></returns>
        [TransManager]
        public StockInBalRedMaster SaveStockInBalRedMaster(StockInBalRedMaster obj, IList movedDtlList)
        {
            decimal dCostMoney = 0;
            decimal dQty = 0;
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                string sAbbreviation = string.Empty;
                if (TransUtil.ToString(obj.ProfessionCategory) == "")
                {
                    sAbbreviation = obj.MaterialCategory.Abbreviation;
                }
                else
                {
                    sAbbreviation = obj.ProfessionCategory;
                }
                obj.Code = GetCode(typeof(StockInBalRedMaster), obj.ProjectId, sAbbreviation);
                obj.RealOperationDate = DateTime.Now;
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveByDao(obj) as StockInBalRedMaster;
                //新增时修改前续单据的引用数量
                foreach (StockInBalRedDetail dtl in obj.Details)
                {
                    //增加验收结算单明细的引用数量
                    StockInBalDetail stockInBalDetail = GetStockInBalDetail(dtl.ForwardDetailId);
                    stockInBalDetail.RefQuantity = stockInBalDetail.RefQuantity + Math.Abs(dtl.Quantity);
                    #region 红单明细力资费计算=蓝单明细力资费*(红单数量/蓝单数量)
                    #region  
                    //不回写验收结算单
                    dtl.CostMoney = stockInBalDetail.Quantity == 0 ? 0 : stockInBalDetail.CostMoney * Math.Abs(dtl.Quantity) / stockInBalDetail.Quantity;
                    #endregion
                    #endregion
                    dao.Save(stockInBalDetail);

                    decimal refTempQty = Math.Abs(dtl.Quantity);
                    //减少收料单明细的结算数量
                    foreach (StockInBalDetail_ForwardDetail forObj in stockInBalDetail.ForwardDetails)
                    {
                        StockInDtl forwardDtl = GetStockInDtl(forObj.ForwardDetailId);
                        if (forwardDtl.BalQuantity >= refTempQty)
                        {
                            forwardDtl.BalQuantity = forwardDtl.BalQuantity - refTempQty;
                            refTempQty = 0;
                        }
                        else
                        {
                            refTempQty = refTempQty - forwardDtl.BalQuantity;
                            forwardDtl.BalQuantity = 0;
                        }
                        dao.Save(forwardDtl);
                    }

                }
            }
            else
            {
                if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                foreach (StockInBalRedDetail dtl in obj.Details)
                {
                    //验收结算单明细引用数量处理
                    StockInBalDetail stockInBalDetail = GetStockInBalDetail(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        //新增明细
                        stockInBalDetail.RefQuantity = stockInBalDetail.RefQuantity + Math.Abs(dtl.Quantity);
                    }
                    else
                    {
                        //修改明细
                        stockInBalDetail.RefQuantity = stockInBalDetail.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp); ;
                    }
                    #region 红单明细力资费计算=蓝单明细力资费*(红单数量/蓝单数量)
                
                    #region 不回写
                    dtl.CostMoney = stockInBalDetail.Quantity == 0 ? 0 : stockInBalDetail.CostMoney * Math.Abs(dtl.Quantity) / stockInBalDetail.Quantity;
                    #endregion
                    #endregion
                    dao.SaveOrUpdate(stockInBalDetail);//保存验收结算单明细

                    decimal refTempQty = Math.Abs(dtl.Quantity);
                    decimal diffQty = Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);//修改前后数量差异
                    decimal tempDiffQty = Math.Abs(diffQty);
                    foreach (StockInBalDetail_ForwardDetail forObj in stockInBalDetail.ForwardDetails)
                    {
                        //收料单结算数量处理
                        StockInDtl forwardDtl = GetStockInDtl(forObj.ForwardDetailId);
                        if (dtl.Id == null)
                        {
                            //新增时减少结算数量
                            if (forwardDtl.BalQuantity >= refTempQty)
                            {
                                forwardDtl.BalQuantity = forwardDtl.BalQuantity - refTempQty;
                                refTempQty = 0M;
                            }
                            else
                            {
                                refTempQty = refTempQty - forwardDtl.BalQuantity;
                                forwardDtl.BalQuantity = 0M;
                            }
                        }
                        else
                        {
                            //修改时减少结算数量
                            if (diffQty >= 0)
                            {
                                //修改后数量比修改前要大，减少收料单结算数量
                                if (forwardDtl.BalQuantity >= tempDiffQty)
                                {
                                    forwardDtl.BalQuantity = forwardDtl.BalQuantity - tempDiffQty;
                                    tempDiffQty = 0M;
                                }
                                else
                                {
                                    tempDiffQty = tempDiffQty - forwardDtl.BalQuantity;
                                    forwardDtl.BalQuantity = 0M;
                                }
                            }
                            else
                            {
                                //修改后数量比修改前要小，增加收料单结算数量
                                decimal canUseQty = forwardDtl.Quantity - forwardDtl.BalQuantity - forwardDtl.RefQuantity;
                                if (canUseQty >= tempDiffQty)
                                {
                                    forwardDtl.BalQuantity = forwardDtl.BalQuantity + tempDiffQty;
                                    tempDiffQty = 0M;
                                }
                                else
                                {
                                    tempDiffQty = tempDiffQty - canUseQty;
                                    forwardDtl.BalQuantity = forwardDtl.BalQuantity + canUseQty;
                                }
                            }
                        }
                        dao.SaveOrUpdate(forwardDtl);//保存收料单明细
                    }

                }
                obj = SaveOrUpdateByDao(obj) as StockInBalRedMaster;

                //修改时对于删除的明细 删除引用数量
                foreach (StockInBalRedDetail dtl in movedDtlList)
                {
                    //删除验收结算单明细的引用数量
                    StockInBalDetail stockInBalDetail = GetStockInBalDetail(dtl.ForwardDetailId);
                    stockInBalDetail.RefQuantity = stockInBalDetail.RefQuantity - Math.Abs(dtl.Quantity);
                    #region 红单明细力资费计算=蓝单明细力资费*(红单数量/蓝单数量)
                 
                    //不回写
                    //dtl.CostMoney = stockInBalDetail.Quantity == 0 ? 0 : stockInBalDetail.CostMoney * Math.Abs(dtl.Quantity) / stockInBalDetail.Quantity;
                    
                    #endregion
                    dao.SaveOrUpdate(stockInBalDetail);

                    decimal quantity = Math.Abs(dtl.Quantity);
                    foreach (StockInBalDetail_ForwardDetail forObj in stockInBalDetail.ForwardDetails)
                    {
                        //增加收料单的结算数量
                        StockInDtl forwardDtl = GetStockInDtl(forObj.ForwardDetailId);
                        decimal canUseQty = forwardDtl.Quantity - forwardDtl.BalQuantity - forwardDtl.RefQuantity;
                        if (quantity >= canUseQty)
                        {
                            forwardDtl.BalQuantity = forwardDtl.BalQuantity + canUseQty;
                            quantity = quantity - canUseQty;
                        }
                        else
                        {
                            forwardDtl.BalQuantity = forwardDtl.BalQuantity + quantity;
                            quantity = 0M;
                        }
                        dao.SaveOrUpdate(forwardDtl);
                    }
                }
            }
            if (obj.DocState == DocumentState.InExecute)
            {
                SaveStockInBalRedMasterCreateDiffPriceBill(obj);
            }

            return obj;
        }
     
        public Hashtable GetDiffMonthAdjustByStockInBal(IList lstStockInBalDtlID)
        {
            
            Hashtable ht = new Hashtable();
            string sWhere = string.Empty;
            if (lstStockInBalDtlID != null && lstStockInBalDtlID.Count > 0)
            {
                IList lst = new ArrayList();
                foreach (string  oStockInBalDetailID in lstStockInBalDtlID)
                {
                    if (!string.IsNullOrEmpty(sWhere))
                    {
                        sWhere += ",";
                    }
                    sWhere += "'" + oStockInBalDetailID + "'";
                }
                try
                {
                    ISession session = CallContext.GetData("nhsession") as ISession;
                    IDbConnection conn = session.Connection;
                    IDbCommand command = conn.CreateCommand();
                    command.CommandText = "select  distinct forwarddetailid  from thd_stkstockindtl where forwarddetailid in (" + sWhere + ")";
                    IDataReader dataReader = command.ExecuteReader();
                   
                    while (dataReader.Read())
                    {
                        ht.Add(dataReader.GetString(0), "");
                      
                    }
                    
                }
                catch
                {
                }
            }
            return ht;
        }
        [TransManager]
        public void SaveStockInBalRedMasterCreateDiffPriceBill(StockInBalRedMaster oStockInBalRedMaster)
        {
            string sStockInBalMasterID=string.Empty ;
            
            StockIn oStockIn = null;
            StockOut oStockOut = null;
            
            if (oStockInBalRedMaster != null)
            {
                sStockInBalMasterID = oStockInBalRedMaster.ForwardBillId;//结算红单主表前驱是结算单主表
                oStockIn = GetStockInByForwardID(sStockInBalMasterID);//结算单主表是出入库调差单前驱
                oStockOut = GetStockOutByForwardID(sStockInBalMasterID);
                if (oStockIn != null && oStockOut != null)
                {
                    StockInRed oStockInRed = CreateStockInRed(oStockInBalRedMaster, oStockIn);
                    StockOutRed oStockOutRed = CreateStockOutRed(oStockInBalRedMaster, oStockOut);
                    if (oStockInRed != null)
                    {
                        SaveStockInRed(oStockInRed );
                    }
                    if (oStockOutRed != null)
                    {
                        //dao.Save(oStockOutRed);
                        stockOutSrv.SaveStockOutRed(oStockOutRed );
                    }
                }
            }
            else
            {
            }
        }
        [TransManager]
        public StockInRed CreateStockInRed(StockInBalRedMaster oStockInBalRedMaster, StockIn oStockIn)
        {
            StockInRed oStockInRed = null; 
            StockInRedDtl oStockInRedDtl=null;
            string sStockInBalRedDetailForwardID=string.Empty ;
            if (oStockInBalRedMaster != null && oStockInBalRedMaster.Details.Count >0 && oStockIn != null && oStockIn.Details.Count > 0)
            {
                oStockInRed = CopyStockInRedMaster(oStockInBalRedMaster,oStockIn);
                oStockInRed.ForwardBillCode = oStockInBalRedMaster.Code;
                oStockInRed.ForwardBillId = oStockInBalRedMaster.Id;
                oStockInRed.ForwardBillType = "验收结算红单";
                oStockInRed.SumQuantity=0;
                oStockInRed.Descript = "结算红单对入库调价单做的【入库红单】";
                foreach (StockInBalRedDetail oStockInBalRedDetail in oStockInBalRedMaster.Details)
                {
                    sStockInBalRedDetailForwardID=oStockInBalRedDetail .ForwardDetailId;//结算单红单明细的前驱是结算单明细 结算单明细的也是出入库调价单明细的前驱 
                    foreach (StockInDtl oStockInDtl in oStockIn.Details)
                    {
                        if (string.Equals(oStockInDtl.ForwardDetailId, sStockInBalRedDetailForwardID))
                        {
                             oStockInRedDtl= CopyStockInRedDtl( oStockInDtl);
                             if (oStockInRedDtl != null)
                             {
                                 oStockInRed.Details.Add(oStockInRedDtl);
                                 oStockInRedDtl.Master = oStockInRed;
                                 oStockInRedDtl.ForwardDetailId = oStockInBalRedDetail.Id;
                                 oStockInRedDtl.Quantity = 0;
                                 oStockInRedDtl.Price = oStockInDtl.Price;
                                 oStockInRedDtl.Money = -oStockInDtl.Money; //-oStockInDtl.Price * Math .Abs ( oStockInBalRedDetail.Quantity);
                                 oStockInRedDtl.Descript = "结算红单对入库调价单做的【入库红单】";
                                 oStockInRed.SumMoney += oStockInRedDtl.Money;
                                
                             }
                        }
                    }

                }
            }
            return oStockInRed; 
        }
        [TransManager]
        public StockOutRed CreateStockOutRed(StockInBalRedMaster oStockInBalRedMaster, StockOut oStockOut)
        {
            StockOutRed oStockOutRed = null;
            StockOutRedDtl oStockOutRedDtl = null;
            string sStockInBalRedDetailForwardID = string.Empty;
            if (oStockInBalRedMaster != null && oStockInBalRedMaster.Details.Count > 0 && oStockOut != null && oStockOut.Details.Count > 0)
            {
                oStockOutRed = CopyStockOutRedMaster(oStockInBalRedMaster, oStockOut);
                if (oStockOutRed != null)
                {
                    oStockOutRed.ForwardBillCode = oStockInBalRedMaster.Code;
                    oStockOutRed.ForwardBillId = oStockInBalRedMaster.Id;
                    oStockOutRed.ForwardBillType = "验收结算红单";
                    oStockOutRed.SumQuantity = 0;
                    oStockOutRed.Descript = "结算红单对出库调价单做的【出库红单】";
                    foreach (StockInBalRedDetail oStockInBalRedDetail in oStockInBalRedMaster.Details)
                    {
                        sStockInBalRedDetailForwardID = oStockInBalRedDetail.ForwardDetailId;//结算单红单明细的前驱是结算单明细 结算单明细的也是出入库调价单明细的前驱 
                        foreach (StockOutDtl oStockOutDtl in oStockOut.Details)
                        {
                            if (string.Equals(oStockOutDtl.ForwardDetailId, sStockInBalRedDetailForwardID))
                            {
                                oStockOutRedDtl = CopyStockOutRedDtl(oStockOutDtl);
                                if (oStockOutRedDtl != null)
                                {
                                    oStockOutRed.Details.Add(oStockOutRedDtl);
                                    oStockOutRedDtl.Master = oStockOutRed;
                                    oStockOutRedDtl.ForwardDetailId = oStockInBalRedDetail.Id;
                                    oStockOutRedDtl.Quantity = 0;
                                    oStockOutRedDtl.Price = oStockOutDtl.Price;
                                    oStockOutRedDtl.Money = -oStockOutDtl.Money;//-oStockOutDtl.Price * Math.Abs(oStockInBalRedDetail.Quantity);
                                    oStockOutRedDtl.Descript = "结算红单对出库调价单做的【出库红单】";
                                    oStockOutRed.SumMoney += oStockOutRedDtl.Money;
                                }
                            }
                        }
                    }

                }
            }
            return oStockOutRed;
        }
        [TransManager]
        public StockInRed CopyStockInRedMaster(StockInBalRedMaster oStockInBalRedMaster,StockIn oStockIn)
        {
            StockInRed oStockInRed =null;
            if(oStockIn!=null)
            {
            oStockInRed= new StockInRed();
            oStockInRed.AuditDate = oStockInBalRedMaster.AuditDate;
            oStockInRed.AuditMonth = oStockInBalRedMaster.AuditMonth;
            oStockInRed.AuditPerson = oStockInBalRedMaster.AuditPerson;
            oStockInRed.AuditPersonName = oStockInBalRedMaster.AuditPersonName;
            oStockInRed.AuditRoles = oStockInBalRedMaster.AuditRoles;
            oStockInRed.Audits = oStockInBalRedMaster.Audits;
            oStockInRed.AuditYear = oStockInBalRedMaster.AuditYear;
            oStockInRed.ClassifyCode = oStockIn.ClassifyCode;
            oStockInRed.ContractNo = oStockIn.ContractNo;
            oStockInRed.ConcreteBalID = oStockIn.ConcreteBalID;
            oStockInRed.ContractNo = oStockIn.ContractNo;
            oStockInRed.CreateDate = oStockInBalRedMaster.CreateDate;
            oStockInRed.CreateMonth = oStockInBalRedMaster.CreateMonth;
            oStockInRed.CreatePerson = oStockInBalRedMaster.CreatePerson;
            oStockInRed.CreatePersonName = oStockInBalRedMaster.CreatePersonName;
            oStockInRed.CreateYear = oStockInBalRedMaster.CreateYear;
            oStockInRed.CurrencyType = oStockIn.CurrencyType;
            oStockInRed.Descript = oStockIn.Descript;
            oStockInRed.DocState = oStockIn.DocState;
            oStockInRed.ExchangeRate = oStockIn.ExchangeRate;
            oStockInRed.ForRedType = EnumForRedType.冲单价;
            oStockInRed.HandleOrg = oStockIn.HandleOrg;
            oStockInRed.HandlePerson = oStockIn.HandlePerson;
            oStockInRed.HandlePersonName = oStockIn.HandlePersonName;
            oStockInRed.HandOrgLevel = oStockIn.HandOrgLevel;
            oStockInRed.InvalidateDate = oStockIn.InvalidateDate;
            oStockInRed.InvalidateMonth = oStockIn.InvalidateMonth;
            oStockInRed.InvalidatePerson = oStockIn.InvalidatePerson;
            oStockInRed.InvalidateYear = oStockIn.InvalidateYear;
            oStockInRed.IsFinished = oStockIn.IsFinished;
            oStockInRed.IsTally = oStockIn.IsTally;
            oStockInRed.JBR = oStockIn.JBR;
            oStockInRed.LastModifyDate = oStockIn.LastModifyDate;
            oStockInRed.MatCatName = oStockIn.MatCatName;
            oStockInRed.MaterialCategory = oStockIn.MaterialCategory;
            oStockInRed.OperOrgInfo = oStockIn.OperOrgInfo;
            oStockInRed.OperOrgInfoName = oStockIn.OperOrgInfoName;
            oStockInRed.OpgSysCode = oStockIn.OpgSysCode;
            oStockInRed.PrintTimes = oStockIn.PrintTimes;
            oStockInRed.ProfessionCategory = oStockIn.ProfessionCategory;
            oStockInRed.ProjectId = oStockIn.ProjectId;
            oStockInRed.ProjectName = oStockIn.ProjectName;
            oStockInRed.RealOperationDate = oStockIn.RealOperationDate;
            oStockInRed.Special = oStockIn.Special;
            oStockInRed.StockInManner = EnumStockInOutManner.收料入库;
            oStockInRed.SubmitDate = oStockInBalRedMaster.SubmitDate;
            oStockInRed.SumConfirmMoney = oStockIn.SumConfirmMoney;
            oStockInRed.SumMoney = oStockIn.SumMoney;
            oStockInRed.SumQuantity = oStockIn.SumQuantity;
            oStockInRed.SupplyOrderCode = oStockIn.SupplyOrderCode;
            oStockInRed.TheStationCategory = oStockIn.TheStationCategory;
            oStockInRed.TheStockInOutKind = 1;
            oStockInRed.TheSupplierName = oStockIn.TheSupplierName;
            oStockInRed.TheSupplierRelationInfo = oStockIn.TheSupplierRelationInfo;
            }
            return oStockInRed;
        }
        [TransManager]
        public StockInRedDtl CopyStockInRedDtl(StockInDtl oStockInDtl)
        {
            StockInRedDtl oStockInRedDtl = null;
            if (oStockInDtl != null)
            {
                oStockInRedDtl = new StockInRedDtl();
                oStockInRedDtl.ConcreteBalDtlID = oStockInDtl.ConcreteBalDtlID;
                oStockInRedDtl.ConfirmMoney = oStockInDtl.ConfirmMoney;
                oStockInRedDtl.ConfirmPrice = oStockInDtl.ConfirmPrice;
                oStockInRedDtl.Descript = oStockInDtl.Descript;
                oStockInRedDtl.DiagramNumber = oStockInDtl.DiagramNumber;
                oStockInRedDtl.ForwardDetailId = oStockInDtl.ForwardDetailId;
                oStockInRedDtl.IsOver = oStockInDtl.IsOver;
                oStockInRedDtl.MaterialCode = oStockInDtl.MaterialCode;
                oStockInRedDtl.MaterialCode = oStockInDtl.MaterialCode;
                oStockInRedDtl.MaterialName = oStockInDtl.MaterialName;
                oStockInRedDtl.MaterialResource = oStockInDtl.MaterialResource;
                oStockInRedDtl.MaterialSpec = oStockInDtl.MaterialSpec;
                oStockInRedDtl.MaterialStuff = oStockInDtl.MaterialStuff;
                oStockInRedDtl.MaterialSysCode = oStockInDtl.MaterialSysCode;
                oStockInRedDtl.MatStandardUnit = oStockInDtl.MatStandardUnit;
                oStockInRedDtl.MatStandardUnitName = oStockInDtl.MatStandardUnitName;
                oStockInRedDtl.Money = oStockInDtl.Money;
               // oStockInRedDtl.NewPrice = oStockInDtl.NewPrice;
                oStockInRedDtl.Price = oStockInDtl.Price;
                oStockInRedDtl.ProfessionalCategory = oStockInDtl.ProfessionalCategory;
                oStockInRedDtl.Quantity = oStockInDtl.Quantity;
                oStockInRedDtl.QuantityTemp = oStockInDtl.QuantityTemp;
                oStockInRedDtl.RefQuantity = oStockInDtl.RefQuantity;
                oStockInRedDtl.UsedPart = oStockInDtl.UsedPart;
                oStockInRedDtl.UsedPartName = oStockInDtl.UsedPartName;
                oStockInRedDtl.UsedPartSysCode = oStockInDtl.UsedPartSysCode;
            }
            return oStockInRedDtl;
            
        }
        [TransManager]
        public StockOutRed CopyStockOutRedMaster(StockInBalRedMaster oStockInBalRedMaster,StockOut oStockOut)
        {
            StockOutRed oStockOutRed = null;
            if (oStockOut != null)
            {
                oStockOutRed = new StockOutRed();
                oStockOutRed.AuditDate = oStockInBalRedMaster.AuditDate;
                oStockOutRed.AuditMonth = oStockInBalRedMaster.AuditMonth;
                oStockOutRed.AuditPerson = oStockInBalRedMaster.AuditPerson;
                oStockOutRed.AuditPersonName = oStockInBalRedMaster.AuditPersonName;
                oStockOutRed.AuditRoles = oStockInBalRedMaster.AuditRoles;
                oStockOutRed.Audits = oStockInBalRedMaster.Audits;
                oStockOutRed.AuditYear = oStockInBalRedMaster.AuditYear;
                oStockOutRed.ClassifyCode = oStockOut.ClassifyCode;
                oStockOutRed.ConcreteBalID = oStockOut.ConcreteBalID;
                oStockOutRed.CreateDate = oStockInBalRedMaster.CreateDate;
                oStockOutRed.CreateMonth = oStockInBalRedMaster.CreateMonth;
                oStockOutRed.CreatePerson = oStockInBalRedMaster.CreatePerson;
                oStockOutRed.CreatePersonName = oStockInBalRedMaster.CreatePersonName;
                oStockOutRed.CreateYear = oStockInBalRedMaster.CreateYear;
                oStockOutRed.CurrencyType = oStockOut.CurrencyType;
                oStockOutRed.Descript = oStockOut.Descript;
                oStockOutRed.DocState = oStockOut.DocState;
                oStockOutRed.ExchangeRate = oStockOut.ExchangeRate;
                oStockOutRed.ForwardBillCode = oStockOut.ForwardBillCode;
                oStockOutRed.ForwardBillId = oStockOut.ForwardBillId;
                oStockOutRed.ForwardBillType = oStockOut.ForwardBillType;
                oStockOutRed.HandleOrg = oStockOut.HandleOrg;
                oStockOutRed.HandlePerson = oStockOut.HandlePerson;
                oStockOutRed.HandlePersonName = oStockOut.HandlePersonName;
                oStockOutRed.HandOrgLevel = oStockOut.HandOrgLevel;
                oStockOutRed.InvalidateDate = oStockOut.InvalidateDate;
                oStockOutRed.InvalidateMonth = oStockOut.InvalidateMonth;
                oStockOutRed.InvalidatePerson = oStockOut.InvalidatePerson;
                oStockOutRed.InvalidateYear = oStockOut.InvalidateYear;
                oStockOutRed.IsFinished = oStockOut.IsFinished;
                oStockOutRed.IsLimited = oStockOut.IsLimited;
                oStockOutRed.IsSelect = oStockOut.IsSelect;
                oStockOutRed.IsTally = oStockOut.IsTally;
                oStockOutRed.JBR = oStockOut.JBR;
                oStockOutRed.LastModifyDate = oStockOut.LastModifyDate;

                oStockOutRed.MatCatName = oStockOut.MatCatName;
                oStockOutRed.MaterialCategory = oStockOut.MaterialCategory;
                oStockOutRed.MonthConsumeId = oStockOut.MonthConsumeId;
                oStockOutRed.OperOrgInfo = oStockOut.OperOrgInfo;
                oStockOutRed.OperOrgInfoName = oStockOut.OperOrgInfoName;
                oStockOutRed.OpgSysCode = oStockOut.OpgSysCode;
                oStockOutRed.PrintTimes = oStockOut.PrintTimes;
                oStockOutRed.ProfessionCategory = oStockOut.ProfessionCategory;
                oStockOutRed.ProjectId = oStockOut.ProjectId;
                oStockOutRed.ProjectName = oStockOut.ProjectName;
                oStockOutRed.RealOperationDate = oStockOut.RealOperationDate;
                oStockOutRed.RefQuantity = oStockOut.RefQuantity;
                oStockOutRed.Special = oStockOut.Special;
                oStockOutRed.StockOutManner = EnumStockInOutManner.领料出库;
                oStockOutRed.SubmitDate = oStockOut.SubmitDate;
                oStockOutRed.SumMoney = oStockOut.SumMoney;
                oStockOutRed.SumQuantity = oStockOut.SumQuantity;
                oStockOutRed.TheStationCategory = oStockOut.TheStationCategory;
                oStockOutRed.TheStockInOutKind = 1;
                oStockOutRed.TheSupplierName = oStockOut.TheSupplierName;
                oStockOutRed.TheSupplierRelationInfo = oStockOut.TheSupplierRelationInfo;
                 
            }
            return oStockOutRed;
        }
        [TransManager]
        public StockOutRedDtl CopyStockOutRedDtl(StockOutDtl oStockOutDtl)
        {
            StockOutRedDtl oStockOutRedDtl = null;
            if (oStockOutDtl != null)
            {
                oStockOutRedDtl = new StockOutRedDtl();
                oStockOutRedDtl.ConcreteBalDtlID = oStockOutDtl.ConcreteBalDtlID;
                oStockOutRedDtl.ConfirmMoney = 0;
                oStockOutRedDtl.ConfirmPrice = 0;
                oStockOutRedDtl.Descript = oStockOutDtl.Descript;
                oStockOutRedDtl.DiagramNumber = oStockOutDtl.DiagramNumber;
                oStockOutRedDtl.ForwardDetailId = oStockOutDtl.ForwardDetailId;
                oStockOutRedDtl.IsOver = oStockOutDtl.IsOver;
                oStockOutRedDtl.MaterialCode = oStockOutDtl.MaterialCode;
                oStockOutRedDtl.MaterialCode = oStockOutDtl.MaterialCode;
                oStockOutRedDtl.MaterialName = oStockOutDtl.MaterialName;
                oStockOutRedDtl.MaterialResource = oStockOutDtl.MaterialResource;
                oStockOutRedDtl.MaterialSpec = oStockOutDtl.MaterialSpec;
                oStockOutRedDtl.MaterialStuff = oStockOutDtl.MaterialStuff;
                oStockOutRedDtl.MaterialSysCode = oStockOutDtl.MaterialSysCode;
                oStockOutRedDtl.MatStandardUnit = oStockOutDtl.MatStandardUnit;
                oStockOutRedDtl.MatStandardUnitName = oStockOutDtl.MatStandardUnitName;
                oStockOutRedDtl.Money = oStockOutDtl.Money;
                 
                oStockOutRedDtl.Price = oStockOutDtl.Price;
                oStockOutRedDtl.ProfessionalCategory = oStockOutDtl.ProfessionalCategory;
                oStockOutRedDtl.Quantity = oStockOutDtl.Quantity;
                //oStockOutRedDtl.QuantityTemp = oStockOutDtl.QuantityTemp;
                oStockOutRedDtl.RefQuantity = oStockOutDtl.RefQuantity;
                oStockOutRedDtl.UsedPart = oStockOutDtl.UsedPart;
                oStockOutRedDtl.UsedPartName = oStockOutDtl.UsedPartName;
                oStockOutRedDtl.UsedPartSysCode = oStockOutDtl.UsedPartSysCode;
                oStockOutRedDtl.SubjectGUID = oStockOutDtl.SubjectGUID;
                oStockOutRedDtl.SubjectName = oStockOutDtl.SubjectName;
                oStockOutRedDtl.SubjectSysCode = oStockOutDtl.SubjectSysCode;
            }
            return oStockOutRedDtl;

        }
        private StockIn GetStockInByForwardID(string sForwardBillId)
        {
             
            ObjectQuery oQuery = new ObjectQuery();
            oQuery .AddCriterion (Expression .Eq ("ForwardBillId",sForwardBillId));
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            IList lst = dao.ObjectQuery(typeof(StockIn), oQuery);
            return (lst==null ||lst.Count ==0)?null:lst[0] as StockIn ;
        }
        private StockOut GetStockOutByForwardID(string sForwardBillId)
        {
             
            ObjectQuery oQuery = new ObjectQuery();
            oQuery .AddCriterion (Expression .Eq ("ForwardBillId",sForwardBillId));
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            IList lst = stockOutSrv.GetStockOut(oQuery);
            return (lst == null || lst.Count == 0) ? null : lst[0] as StockOut; 
        }
        /// <summary>
        /// 删除验收结算单红单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteStockInBalRedMaster(StockInBalRedMaster obj)
        {
            decimal dQty = 0, dCostMoney = 0;
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (StockInBalRedDetail dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    //删除验收结算单引用数量
                    StockInBalDetail stockInBalDetail = GetStockInBalDetail(dtl.ForwardDetailId);
                    stockInBalDetail.RefQuantity = stockInBalDetail.RefQuantity - Math.Abs(dtl.Quantity);
                    #region 红单明细力资费计算=蓝单明细力资费*(红单数量/蓝单数量)
                 

                    #endregion
                    dao.SaveOrUpdate(stockInBalDetail);

                    //增加收料单的结算数量
                    decimal quantity = Math.Abs(dtl.Quantity);
                    foreach (StockInBalDetail_ForwardDetail forObj in stockInBalDetail.ForwardDetails)
                    {
                        //增加收料单的结算数量
                        StockInDtl forwardDtl = GetStockInDtl(forObj.ForwardDetailId);
                        decimal canUseQty = forwardDtl.Quantity - forwardDtl.BalQuantity - forwardDtl.RefQuantity;
                        if (quantity >= canUseQty)
                        {
                            forwardDtl.BalQuantity = forwardDtl.BalQuantity + canUseQty;
                            quantity = quantity - canUseQty;
                        }
                        else
                        {
                            forwardDtl.BalQuantity = forwardDtl.BalQuantity + quantity;
                            quantity = 0M;
                        }
                        dao.SaveOrUpdate(forwardDtl);
                    }
                }
            }
            return dao.Delete(obj);
        }
        #endregion

        #region 入库记账方法
        private IStockInOutSrv stockInOutSrv;
        public IStockInOutSrv StockInOutSrv
        {
            get { return stockInOutSrv; }
            set { stockInOutSrv = value; }
        }

        /// <summary>
        /// 入库记账
        /// </summary>
        /// <param name="hashLst">key StockIn;value id List;key StockInRed;value id List;</param>
        /// <param name="hashCode">key StockIn;value Code List;key StockInRed;value Code List;</param>
        /// <param name="year">记账年</param>
        /// <param name="month">记账月</param>
        /// <param name="tallyPersonId">记账人ID</param>
        /// <param name="tallyPersonName">记账人名称</param>
        /// <returns></returns>
        virtual public Hashtable TallyStockIn(Hashtable hashLst, Hashtable hashCode, int year, int month, string tallyPersonId, string tallyPersonName, string projectId)
        {
            //检查上个月是否结帐，上个月必须结帐，本月才能开展记账业务
            string lastDate = year.ToString() + "-" + month.ToString() + "-01";
            int lastYear = Convert.ToDateTime(lastDate).AddMonths(-1).Year;
            int lastMonth = Convert.ToDateTime(lastDate).AddMonths(-1).Month;

            //if (!stockInOutSrv.CheckIfNewProject(projectId))
            //{
                //if (!StockInOutSrv.CheckReckoning(lastYear, lastMonth, projectId))
                //    throw new Exception("会计期【" + lastYear.ToString() + "-" + lastMonth.ToString() + "】未结账,请先进行结账！");

                //if (StockInOutSrv.CheckReckoning(year, month, projectId))
                //    throw new Exception("会计期【" + year.ToString() + "-" + month.ToString() + "】已经结账,不能进行记账！");
            //}

            Hashtable returnValue = new Hashtable();

            if (hashLst != null)
            {
                foreach (string billName in hashLst.Keys)
                {
                    if (billName == "StockIn")
                    {
                        returnValue = Tally(hashLst[billName] as IList, hashCode[billName] as IList, 0, year, month, tallyPersonId, tallyPersonName);
                    }
                    else if (billName == "StockInRed")
                    {
                        returnValue = Tally(hashLst[billName] as IList, hashCode[billName] as IList, 1, year, month, tallyPersonId, tallyPersonName);
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 记账方法
        /// </summary>
        /// <param name="billIdList">ID List</param>
        /// <param name="codeList">Code List</param>
        /// <param name="billType">0表示蓝单；1表示红单</param>
        /// <returns></returns>
        private Hashtable Tally(IList billIdList, IList codeList, int billType, int year, int month, string tallyPersonId, string tallyPersonName)
        {
            Hashtable retValue = new Hashtable();
            if (billIdList == null) return retValue;

            IList listSuccess = new ArrayList();
            string errMsg = "";

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;

            foreach (string billId in billIdList)
            {
                IDbCommand cmd = cnn.CreateCommand();
                session.Transaction.Enlist(cmd);

                cmd.CommandText = "thd_StockInTally";
                cmd.CommandType = CommandType.StoredProcedure;

                #region 传入参数
                //主表ID
                IDbDataParameter id = cmd.CreateParameter();
                id.Value = billId;
                id.Direction = ParameterDirection.Input;
                id.ParameterName = "billid";
                cmd.Parameters.Add(id);

                //记账人ID
                IDbDataParameter v_tallyPersonId = cmd.CreateParameter();
                v_tallyPersonId.Value = tallyPersonId;
                v_tallyPersonId.Direction = ParameterDirection.Input;
                v_tallyPersonId.ParameterName = "tallyPersonId";
                cmd.Parameters.Add(v_tallyPersonId);

                //记账人名称
                IDbDataParameter v_tallyPersonName = cmd.CreateParameter();
                v_tallyPersonName.Value = tallyPersonName;
                v_tallyPersonName.Direction = ParameterDirection.Input;
                v_tallyPersonName.ParameterName = "tallyPersonName";
                cmd.Parameters.Add(v_tallyPersonName);

                //记账日期
                IDbDataParameter v_TallyDate = cmd.CreateParameter();
                v_TallyDate.DbType = DbType.DateTime;
                v_TallyDate.Direction = ParameterDirection.Input;
                v_TallyDate.Value = DateTime.Now;
                v_TallyDate.ParameterName = "TallyDate";
                cmd.Parameters.Add(v_TallyDate);

                //会计年
                IDbDataParameter v_TallyYear = cmd.CreateParameter();
                v_TallyYear.Direction = ParameterDirection.Input;
                v_TallyYear.Value = year;
                v_TallyYear.ParameterName = "TallyYear";
                cmd.Parameters.Add(v_TallyYear);

                //会计月
                IDbDataParameter v_TallyMonth = cmd.CreateParameter();
                v_TallyMonth.Direction = ParameterDirection.Input;
                v_TallyMonth.Value = month;
                v_TallyMonth.ParameterName = "TallyMonth";
                cmd.Parameters.Add(v_TallyMonth);

                //红、蓝单标志
                IDbDataParameter v_BillType = cmd.CreateParameter();
                v_BillType.Direction = ParameterDirection.Input;
                v_BillType.Value = billType;
                v_BillType.ParameterName = "BillType";
                cmd.Parameters.Add(v_BillType);

                IDbDataParameter err = cmd.CreateParameter();
                err.DbType = DbType.AnsiString;
                err.Direction = ParameterDirection.Output;
                err.ParameterName = "errMsg";
                err.Size = 500;
                cmd.Parameters.Add(err);
                #endregion

                cmd.ExecuteNonQuery();
                if (err.Value == null || Convert.ToString(err.Value) == "")
                    listSuccess.Add(billId);
                else
                    errMsg += "单号:" + codeList[billIdList.IndexOf(billId)].ToString() + ":" + err.Value + "\n";
            }
            retValue.Add("err", errMsg);
            retValue.Add("Succ", listSuccess);
            return retValue;
        }
        #endregion

        #region 库存方法
        public IList GetStockRelation(ObjectQuery oq)
        {
            return dao.ObjectQuery(typeof(StockRelation), oq);
        }

        public IList GetStockRelationByStockInDtlId(string stockInDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("StockInDtlId", stockInDtlId));
            return GetStockRelation(oq);
        }

        private decimal GetRemainQuantityByStockInDtlId(string stockInDtlId)
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
        #endregion

        #region 日常需求计划、合同方法
        /// <summary>
        /// 查询日常需求计划
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetDailyPLanMaster(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(DailyPlanMaster), oq);
        }

        /// <summary>
        /// 根据Id查询日常需求计划明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DailyPlanDetail GetDailyPlanDetailById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(DailyPlanDetail), oq);
            if (list != null && list.Count > 0) return list[0] as DailyPlanDetail;
            return null;
        }

        /// <summary>
        /// 查询采购合同主表
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetSupplyOrderMaster(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(SupplyOrderMaster), oq);
        }

        #endregion

        #region 基础数据方法
        /// <summary>
        /// 查询基础数据
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetBasicData(ObjectQuery objectQuery)
        {
            IList list = new ArrayList();
            //list = dao.ObjectQuery(typeof(BasicDataOptr), objectQuery);
            //list = dao.ListAll(typeof(StockIn));
            list = base.GetDomainByCondition(typeof(BasicDataOptr), objectQuery);
            return list;
        }

        /// <summary>
        /// 通过基础数据名称查询基础数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList GetBasicDataByParentName(string name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BasicName", name));
            oq.AddCriterion(Expression.Eq("State", -1));
            IList list = GetBasicData(oq);
            if (list != null && list.Count > 0)
            {
                BasicDataOptr parent = list[0] as BasicDataOptr;
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ParentId", parent.Id));
                oq.AddOrder(new NHibernate.Criterion.Order("BasicCode", true));
                IList listDetail = GetBasicData(oq);
                return listDetail;
            }
            return null;
        }

        /// <summary>
        /// 通过基础数据名称和项目查询基础数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList GetBasicDataByParentNameAndProjectName(string name, string projectName)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BasicName", name));
            oq.AddCriterion(Expression.Eq("State", -1));
            IList list = GetBasicData(oq);
            if (list != null && list.Count > 0)
            {
                BasicDataOptr parent = list[0] as BasicDataOptr;
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ParentId", parent.Id));
                oq.AddCriterion(Expression.Eq("ExtendField1", projectName));
                oq.AddOrder(new NHibernate.Criterion.Order("BasicCode", true));
                IList listDetail = GetBasicData(oq);
                return listDetail;
            }
            return null;
        }

        // <summary>
        /// 通过ID删除基础数据明细
        /// </summary>
        public void DelBasicDataBySql(BasicDataOptr model)
        {
            if (model == null || string.IsNullOrEmpty(model.Id)) return;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            //command.Transaction = session.Transaction.;
            string sql = "";
            if (conn is SqlConnection)
            {
                sql = "delete from THD_BasicDataOptr where id =@id";
                //command.Parameters.Add(
            }
            else
            {
                //oracle
                sql = "delete from THD_BasicDataOptr where id =:id";
            }
            command.CommandText = sql;
            IDataParameter id = command.CreateParameter();
            id.Value = model.Id;
            id.ParameterName = "id";
            command.Parameters.Add(id);

            command.ExecuteNonQuery();

        }

        /// <summary>
        /// 保存基础数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public BasicDataOptr SaveBasicData(BasicDataOptr obj)
        {
            return SaveOrUpdateByDao(obj) as BasicDataOptr;
        }
        #endregion

        #region 日志操作
        /// <summary>
        /// 日志统计
        public IList GetLogStatReport(string condition)
        {
            IList lst = new ArrayList();  
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();

            string projectSql = "select t1.projectname,t1.projectcost,t1.projectlifecycle from resconfig t1 " +
                                    " where t1.projectcost>0 or t1.projectlifecycle>0 ";
            command.CommandText = projectSql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            Hashtable ht = new Hashtable();
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name3 = TransUtil.ToString(oDataRow["projectcost"]);
                    int cycle = TransUtil.ToInt(oDataRow["projectlifecycle"]);
                    if (cycle == 1)
                    {
                        domain.Name2 = "投标阶段";
                    }else if (cycle == 2)
                    {
                        domain.Name2 = "策划阶段";
                    }
                    else if (cycle == 3)
                    {
                        domain.Name2 = "施工阶段";
                    }
                    else if (cycle == 4)
                    {
                        domain.Name2 = "竣工结算阶段";
                    }
                    else if (cycle == 5)
                    {
                        domain.Name2 = "维护阶段";
                    }
                    string projectName = TransUtil.ToString(oDataRow["projectname"]);
                    if (!ht.Contains(projectName))
                    {
                        ht.Add(projectName, domain);
                    }
                }
            }

            string sql = " select t1.projectname,t1.billtype,count(DISTINCT(t1.billid)) billcount from thd_logdata t1 where " +
                " t1.billid is not null and trim(t1.projectname) is not null and instr(t1.opertype,'删除') = 0 "
                + condition + " group by t1.projectname,t1.billtype order by t1.projectname,t1.billtype";
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                {
                    DataDomain domain = new DataDomain();
                    string projectName = TransUtil.ToString(oDataRow["projectname"]);
                    domain.Name1 = projectName;
                    domain.Name4 = TransUtil.ToString(oDataRow["billtype"]);
                    domain.Name5 = TransUtil.ToString(oDataRow["billcount"]);
                    if (ht.Contains(projectName))
                    {
                        DataDomain p_domain = (DataDomain)ht[projectName];
                        domain.Name2 = p_domain.Name2;
                        domain.Name3 = p_domain.Name3;
                    }
                    lst.Add(domain);
                }
            }
            return lst;
        }

        /// <summary>
        /// 日志查询
        public IList GetLogData(string condition)
        {
            IList lst = new ArrayList();  
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();

            string sql = " select t1.operdate,t1.operperson from thd_logdata t1 where 1=1 ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                {
                    LogData log = new LogData();
                    log.OperDate = TransUtil.ToDateTime(oDataRow["operdate"]);
                    log.OperPerson = TransUtil.ToString(oDataRow["operperson"]);
                    lst.Add(log);
                }
            }
            return lst;
        }
        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [TransManager]
        public bool SaveLogData(LogData model)
        {
            model.OperDate = DateTime.Now;
            return Dao.SaveOrUpdate(model);
        }

        /// <summary>
        /// 插入日志 参数传入顺序为 BillId;OperType;Code;OperPerson;BillType;Descript;ProjectName
        /// </summary>
        /// <param name="args">参数传入顺序为 BillId;OperType;Code;OperPerson;BillType;Descript;ProjectName</param>
        /// <returns></returns>
        [TransManager]
        public bool SaveLogData(params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                LogData log = new LogData();
                log.OperDate = DateTime.Now;
                for (int i = 0; i < args.Length; i++)
                {
                    object obj = args[i];
                    if (i == 0)
                    {
                        log.BillId = obj.ToString();
                    }
                    else if (i == 1)
                    {
                        log.OperType = obj.ToString();
                    }
                    else if (i == 2)
                    {
                        log.Code = obj.ToString();
                    }
                    else if (i == 3)
                    {
                        log.OperPerson = obj.ToString();
                    }
                    else if (i == 4)
                    {
                        log.BillType = obj.ToString();

                    }
                    else if (i == 5)
                    {
                        log.Descript = obj.ToString();
                    }
                    else if (i == 6)
                    {
                        log.ProjectName = obj.ToString();
                    }
                    else if (i == 7)
                    {
                        log.ProjectID = obj.ToString();
                    }
                }
                return Dao.SaveOrUpdate(log);
            }
            return false;

        }
        #endregion

        #region 项目信息
        /// <summary>
        /// 查找项目信息
        /// </summary>
        /// <returns></returns>
        public CurrentProjectInfo GetProjectInfo()
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = "select id,projectName,projectCode from resconfig";
            IDataReader dataReader = command.ExecuteReader();
            CurrentProjectInfo projectInfo = new CurrentProjectInfo();
            while (dataReader.Read())
            {
                projectInfo.Id = dataReader.GetString(0);
                projectInfo.Name = dataReader.GetString(1);
                projectInfo.Code = dataReader.GetString(2);
            }
            return projectInfo;
        }

        /// <summary>
        /// 查找项目信息
        /// </summary>
        /// <param name="ownerorgsyscode">组织层次码</param>
        /// <returns></returns>
        public CurrentProjectInfo GetProjectInfo(string ownerorgsyscode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Sql(" instr('" + ownerorgsyscode + "',{alias}.ownerorgsyscode)>0"));
            IList lst = Dao.ObjectQuery(typeof(CurrentProjectInfo), oq);
            if (lst != null && lst.Count > 0)
            {
                foreach (CurrentProjectInfo projectInfo in lst)
                {
                    if (projectInfo.Code != "0000")
                    {
                        return projectInfo;
                    }
                }
            }
            return null;
        }
        public OperationOrgInfo GetSubCompanyOrgInfo(string ownerorgsyscode)
        {
            OperationOrgInfo subOrgInfo = new OperationOrgInfo();
            string sSQL = "select t1.opgid,t1.opgcode,t1.opgname,t1.opgsyscode from resoperationorg t1 where instr('" 
                            + ownerorgsyscode + "', t1.opgsyscode)>0 and t1.opgoperationtype='b' ";
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                {
                    subOrgInfo.Id = ClientUtil.ToString(oDataRow["opgid"]);
                    subOrgInfo.Code = ClientUtil.ToString(oDataRow["opgcode"]);
                    subOrgInfo.Name = ClientUtil.ToString(oDataRow["opgname"]);
                    subOrgInfo.SysCode = ClientUtil.ToString(oDataRow["opgsyscode"]);
                }
            }
            return subOrgInfo;
        }
        /// <summary>
        /// 查找项目信息
        /// </summary>
        /// <param name="ownerorgsyscode">组织层次码</param>
        /// <returns></returns>
        public CurrentProjectInfo GetProjectInfo(ObjectQuery oq)
        {
            IList lst = Dao.ObjectQuery(typeof(CurrentProjectInfo), oq);
            if (lst != null && lst.Count > 0) return lst[0] as CurrentProjectInfo;
            return null;
        }

        /// <summary>
        /// 根据ID查找项目
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns></returns>
        public CurrentProjectInfo GetProjectInfoById(string projectId)
        {
            return Dao.Get(typeof(CurrentProjectInfo), projectId) as CurrentProjectInfo;
        }

        /// <summary>
        /// 保存工程项目信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public CurrentProjectInfo SaveCurrentProjectInfo(CurrentProjectInfo obj)
        {
            bool createStock = false;
            if (obj.Id == null)
            {
                //新建项目的时候，生成一个新的仓库
                createStock = true;
            }
            CurrentProjectInfo temp = SaveOrUpdateByDao(obj) as CurrentProjectInfo;
            if (createStock)
            {
                //AddStationCategory(temp);
            }
            return temp;
        }

        /// <summary>
        /// 删除工程项目信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCurrentProjectInfo(CurrentProjectInfo obj)
        {
            if (obj.Id == null) return true;
            return dao.Delete(obj); ;
        }

        private StationCategory GetRootStationCategory()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
            oq.AddCriterion(Expression.Eq("State", 1));
            IList lst = Dao.ObjectQuery(typeof(StationCategory), oq);
            if (lst != null && lst.Count > 0) return lst[0] as StationCategory;
            return null;
        }

        [TransManager]
        private StationCategory AddStationCategory(CurrentProjectInfo projectInfo)
        {
            StationCategory parent = GetRootStationCategory();
            StationCategory newsg = new StationCategory();
            newsg.CategoryNodeType = NodeType.LeafNode;
            newsg.Name = projectInfo.Name + "仓库";
            newsg.CreateDate = DateTime.Now;
            newsg.State = 1;
            newsg.Level = parent.Level + 1;
            newsg.ParentNode = parent;
            newsg.TheTree = parent.TheTree;
            newsg.StationKind = 0;
            newsg.ProjectId = projectInfo;
            newsg.ProjectName = projectInfo.Name;
            newsg = SaveOrUpdateByDao(newsg) as StationCategory;
            newsg.Code = newsg.Id;
            newsg.SysCode = parent.SysCode + newsg.Id + ".";
            newsg = SaveOrUpdateByDao(newsg) as StationCategory;
            return newsg;
        }

        /// <summary>
        ///判断是否使用SQL Server数据库
        /// </summary>
        /// <returns></returns>
        public bool IsUseSQLServer()
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            return (conn is SqlConnection);
        }
        #endregion

        #region 读取flx模板方法
        public bool IfExistFileInServer(string fileName)
        {
            return TransUtil.IfExistFileInServer(fileName);
        }

        public byte[] ReadTempletByServer(string fileName)
        {
            return TransUtil.ReadTempletByServer(fileName);
        }
        #endregion

        #region 查询仓库

        public StationCategory GetStationCategory(string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Not(Expression.Eq("NodeType",0)));
            oq.AddCriterion(Expression.Eq("ProjectId.Id", projectId));
            IList list = dao.ObjectQuery(typeof(StationCategory), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as StationCategory;
            }
            return null;
        }

        public string GetIRPAddress()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["IRPAddress"];
        }
        #endregion

        public object GetObjectById(Type type, string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList lst = Dao.ObjectQuery(type, oq);
            if (lst != null && lst.Count > 0)
            {
                return lst[0];
            }
            return null;
        }

        #region 验收单
        /// <summary>
        /// 创建验收红单对象
        /// </summary>
        /// <returns></returns>
        public StockInBalRedMaster CreateStockInBalRedMaster(StockInRed curBillMaster)
        {
            StockInBalRedMaster curBillBalMaster = new StockInBalRedMaster();
            try
            {
                //目前问题 验收单据和验收明细的前驱无法确定

                curBillBalMaster.CreatePerson = curBillMaster.CreatePerson;
                curBillBalMaster.CreatePersonName = curBillMaster.CreatePersonName;
                curBillBalMaster.CreateDate = curBillMaster.CreateDate;
                curBillBalMaster.CreateYear = curBillMaster.CreateYear;
                curBillBalMaster.CreateMonth = curBillMaster.CreateMonth;
                curBillBalMaster.OperOrgInfo = curBillMaster.OperOrgInfo;
                curBillBalMaster.OperOrgInfoName = curBillMaster.OperOrgInfoName;
                curBillBalMaster.OpgSysCode = curBillMaster.OpgSysCode;
                curBillBalMaster.HandlePerson = curBillMaster.HandlePerson;
                curBillBalMaster.HandlePersonName = curBillMaster.HandlePersonName;
                curBillBalMaster.DocState = DocumentState.InExecute;
                curBillBalMaster.ProjectId = curBillMaster.ProjectId;
                curBillBalMaster.ProjectName = curBillMaster.ProjectName;



                DataSet oData = GetStockInBalID(curBillMaster.ForwardBillId);
                string sForwardBillId = string.Empty;
                string sForwardBillCode = string.Empty;
                if (oData != null && oData.Tables.Count > 0 && oData.Tables[0].Rows.Count > 0)
                {
                    sForwardBillId = oData.Tables[0].Rows[0]["id"].ToString();
                    sForwardBillCode = oData.Tables[0].Rows[0]["code"].ToString();
                }
                curBillBalMaster.ForwardBillId = sForwardBillId;
                curBillBalMaster.ForwardBillCode = sForwardBillCode;

                curBillBalMaster.TheSupplierRelationInfo = curBillMaster.TheSupplierRelationInfo;
                curBillBalMaster.TheSupplierName = curBillMaster.TheSupplierName;

                curBillBalMaster.RealOperationDate = curBillMaster.RealOperationDate;
                curBillBalMaster.Descript = curBillMaster.Descript;
                curBillBalMaster.SumMoney = curBillMaster.SumMoney;// -ClientUtil.TransToDecimal(txtSumMoney.Text);
                curBillBalMaster.SumQuantity = curBillMaster.SumQuantity; //- ClientUtil.TransToDecimal(txtSumQuantity.Text);
                curBillBalMaster.ProfessionCategory = curBillMaster.ProfessionCategory;
                foreach (BaseDetail o in curBillMaster.Details)
                {
                    StockInRedDtl oDtl = o as StockInRedDtl;
                    if (oDtl != null)
                    {
                        StockInBalRedDetail curBillDtl = new StockInBalRedDetail();
                        curBillDtl.MaterialResource = oDtl.MaterialResource;
                        curBillDtl.MaterialCode = oDtl.MaterialCode;
                        curBillDtl.MaterialName = oDtl.MaterialName;
                        curBillDtl.MaterialSpec = oDtl.MaterialSpec;
                        curBillDtl.MaterialStuff = oDtl.MaterialStuff;
                        curBillDtl.MatStandardUnit = oDtl.MatStandardUnit;
                        curBillDtl.MatStandardUnitName = oDtl.MatStandardUnitName;
                        curBillDtl.Quantity = oDtl.Quantity;
                        curBillDtl.QuantityTemp = oDtl.QuantityTemp;
                        curBillDtl.Price = oDtl.Price;
                        curBillDtl.Money = oDtl.Money;
                        curBillDtl.StockInPrice = oDtl.Price;
                        curBillDtl.StockInMoney = oDtl.Money;
                        curBillDtl.ConfirmMoney = oDtl.ConfirmMoney;
                        curBillDtl.ConfirmPrice = oDtl.ConfirmPrice;
                        curBillDtl.ForwardDetailId = GetStockInBalDtlID(oDtl.ForwardDetailId);
                        curBillDtl.Descript = oDtl.Descript;
                        curBillDtl.DiagramNumber = oDtl.DiagramNumber;
                        curBillDtl.ProfessionalCategory = oDtl.ProfessionalCategory;
                        curBillBalMaster.AddDetail(curBillDtl);
                    }
                }
                curBillBalMaster.DocState = DocumentState.InExecute;

            }
            catch (Exception e)
            {

                throw e;
            }
            return curBillBalMaster;
        }
        #endregion

        #region 验收单
        /// <summary>
        /// 创建验收单对象
        /// </summary>
        /// <returns></returns>
        public StockInBalMaster CreateStockInBalMaster(StockIn curBillMaster)
        {
            StockInBalMaster curBillBalMaster = new StockInBalMaster();
            try
            {
                curBillBalMaster.CreatePerson = curBillMaster.CreatePerson;
                curBillBalMaster.CreatePersonName = curBillMaster.CreatePersonName;
                curBillBalMaster.CreateDate = curBillMaster.CreateDate;
                curBillBalMaster.CreateYear = curBillMaster.CreateYear;
                curBillBalMaster.CreateMonth = curBillMaster.CreateMonth;
                curBillBalMaster.OperOrgInfo = curBillMaster.OperOrgInfo;
                curBillBalMaster.OperOrgInfoName = curBillMaster.OperOrgInfoName;
                curBillBalMaster.OpgSysCode = curBillMaster.OpgSysCode;
                curBillBalMaster.HandlePerson = curBillMaster.HandlePerson;
                curBillBalMaster.HandlePersonName = curBillMaster.HandlePersonName;
                curBillBalMaster.DocState = DocumentState.InExecute;

                curBillBalMaster.ProjectId = curBillMaster.ProjectId;
                curBillBalMaster.ProjectName = curBillMaster.ProjectName;

                curBillBalMaster.ForwardBillId = curBillMaster.Id;
                curBillBalMaster.ForwardBillCode = curBillBalMaster.Code;
                curBillBalMaster.TheSupplierName = curBillMaster.TheSupplierName;
                curBillBalMaster.TheSupplierRelationInfo = curBillMaster.TheSupplierRelationInfo;
                curBillBalMaster.RealOperationDate = curBillMaster.RealOperationDate;
                curBillBalMaster.Descript = string.Empty;
                curBillBalMaster.CostMoney = 0;
                curBillBalMaster.SumMoney = curBillMaster.SumMoney;
                curBillBalMaster.MaterialCategory = curBillMaster.MaterialCategory;
                curBillBalMaster.ProfessionCategory = curBillMaster.ProfessionCategory;

                //添加明细
                foreach (BaseDetail o in curBillMaster.Details)
                {
                    StockInDtl oDtl = o as StockInDtl;
                    StockInBalDetail oBalDtl = new StockInBalDetail();
                    if (oDtl != null)
                    {
                        oBalDtl.MaterialResource = oDtl.MaterialResource;
                        oBalDtl.MaterialCode = oDtl.MaterialCode;
                        oBalDtl.MaterialName = oDtl.MaterialName;
                        oBalDtl.MaterialSpec = oDtl.MaterialSpec;
                        oBalDtl.MaterialStuff = oDtl.MaterialStuff;
                        oBalDtl.MatStandardUnit = oDtl.MatStandardUnit;
                        oBalDtl.MatStandardUnitName = oDtl.MatStandardUnitName;
                        oBalDtl.Quantity = oDtl.Quantity;
                        oBalDtl.QuantityTemp = oDtl.QuantityTemp;
                        oBalDtl.Price = oDtl.Price;
                        oBalDtl.Money = oDtl.Money;
                        oBalDtl.StockInPrice = oDtl.Price;
                        oBalDtl.StockInMoney = oDtl.Money;
                        oBalDtl.CostMoney = 0;
                        oBalDtl.ForwardDetailId = oDtl.Id;
                        oBalDtl.Descript = oDtl.Descript;
                        oBalDtl.DiagramNumber = oDtl.DiagramNumber;
                        oBalDtl.ProfessionalCategory = oDtl.ProfessionalCategory;
                        oBalDtl.ConfirmPrice = oDtl.ConfirmPrice;
                        oBalDtl.ConfirmMoney = oDtl.ConfirmMoney;
                        StockInBalDetail_ForwardDetail forwardDetail = new StockInBalDetail_ForwardDetail();
                        forwardDetail.ForwardDetailId = oDtl.Id;
                        oBalDtl.AddForwardDetail(forwardDetail);

                        curBillBalMaster.AddDetail(oBalDtl);
                    }
                }
                curBillMaster.DocState = DocumentState.InExecute;
            }
            catch (Exception e)
            {
                throw e;

            }
            return curBillBalMaster;
        }
        #endregion

        #region 结帐判断
        public FiscalPeriodDetail GetFiscalPeriodDetailList(string sModleID, int iYear, int iMonth)
        {

            FiscalPeriodDetail oFiscalPeriodDetail = null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("AccPeriod.BusModuleId", sModleID));
            oq.AddCriterion(Expression.Eq("FiscalYear", iYear));
            oq.AddCriterion(Expression.Eq("FiscalMonth", iMonth));

            oq.AddFetchMode("AccPeriod", NHibernate.FetchMode.Eager);
            IList oList = dao.ObjectQuery(typeof(FiscalPeriodDetail), oq);
            if (oList != null && oList.Count > 0)
            {
                oFiscalPeriodDetail = oList[0] as FiscalPeriodDetail;
            }
            return oFiscalPeriodDetail;
        }

        /// <summary>
        /// 判断该业务时间内 该项目是否会计了
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="CreateDate"></param>
        /// <returns>true是已经会计</returns>
        public string CheckAccounted(OperationOrgInfo oAccountOrg, DateTime CreateDate, string sProjectID)
        {
            string sMsg = string.Empty;
            string sModelId = "16";
            if (oAccountOrg == null || oAccountOrg.Id == null || string.IsNullOrEmpty(oAccountOrg.Id))
            {
                sMsg = "无核算组织节点，无法操作";
            }
            else
            {
                CreateDate = DateTime.Parse(CreateDate.ToShortDateString());
                string sSQL = "select fiscalyear,fiscalmonth from (select    nvl(t.fiscalyear,0) as   fiscalyear  ,nvl(t.fiscalmonth ,0)  as fiscalmonth from thd_stkstockinout t where t.projectid='{0}' and instr('{1}',t.accountorgsyscode)>0 order by t.fiscalyear ,t.fiscalmonth desc) where rownum<2 ";
                int iYear = 0;
                int iMonth = 0;
                sSQL = string.Format(sSQL, sProjectID, oAccountOrg.SysCode);
                IDataReader dataReader;
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand command = cnn.CreateCommand();
                command.CommandText = sSQL;
                dataReader = command.ExecuteReader(CommandBehavior.Default);
                //while (dataReader.Read())
                //{
                //    dataReader.GetDecimal(0);
                //}
                DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    iYear = int.Parse(dataSet.Tables[0].Rows[0]["fiscalyear"].ToString());
                    iMonth = int.Parse(dataSet.Tables[0].Rows[0]["fiscalmonth"].ToString());
                    FiscalPeriodDetail oFiscalPeriodDetail = GetFiscalPeriodDetailList(sModelId, iYear, iMonth);
                    if (oFiscalPeriodDetail != null)
                    {
                        if (oFiscalPeriodDetail.EndDate < CreateDate)
                        {
                            sMsg = string.Empty;
                        }
                        else
                        {
                            sMsg = string.Format(" [{0}-{1}]会计期已经结帐,单据业务日期必须大于等于[{2}-{3}-{4}]！", CreateDate.Year, CreateDate.Month, oFiscalPeriodDetail.EndDate.Year, oFiscalPeriodDetail.EndDate.Month, oFiscalPeriodDetail.EndDate.AddDays(1).Day);
                        }
                    }
                    else
                    {
                        sMsg = string.Empty;
                    }
                }
                else
                {
                    sMsg = string.Empty;
                }
            }
            return sMsg;

        }

        /// <summary>
        /// 判断出入库的业务日期，必须在最后的业务日期之后
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="CreateDate"></param>
        /// <param name="materialList">资源ID的集合</param>
        /// <param name="busType">业务类型(0,入库 1 出库)</param>
        /// <returns>提示信息</returns>
        public string CheckBusinessDate(OperationOrgInfo oAccountOrg, DateTime CreateDate, IList materialList, string sProjectID)
        {
            string sMsg = string.Empty;
            return sMsg;
            if (oAccountOrg == null || oAccountOrg.Id == null || string.IsNullOrEmpty(oAccountOrg.Id))
            {
                sMsg = "无核算组织节点，无法操作";
            }
            else
            {
                string sSQL = "select max(t1.createdate) createdate from thd_stkstockin t1,thd_stkstockindtl t2 where t1.id=t2.parentid and t1.state=5 and " +
                          " t1.opgsyscode like '%" + oAccountOrg.Id + "%' and t2.material in ( ";
                foreach (string matrialStr in materialList)
                {
                    sSQL += " '" + matrialStr + "' , ";
                }
                sSQL += " '0' ) ";
                sSQL += " union all ";
                sSQL += "select max(t1.createdate) createdate from thd_stkstockout t1,thd_stkstockoutdtl t2 where t1.id=t2.parentid and t1.state=5  and " +
                          " t1.opgsyscode like '%" + oAccountOrg.Id + "%' and t2.material in ( ";
                foreach (string matrialStr in materialList)
                {
                    sSQL += " '" + matrialStr + "' , ";
                }
                sSQL += " '0' ) ";


                IDataReader dataReader;
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand command = cnn.CreateCommand();
                command.CommandText = sSQL;
                dataReader = command.ExecuteReader(CommandBehavior.Default);

                DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                DateTime maxCreateDate = TransUtil.ToDateTime("2000-01-01");
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                    {

                        if (TransUtil.ToDateTime(oDataRow["createdate"]) > maxCreateDate)
                        {
                            maxCreateDate = TransUtil.ToDateTime(oDataRow["createdate"]);
                        }
                    }
                }
                if (CreateDate < maxCreateDate)
                {
                    sMsg = "业务日期[" + CreateDate.ToShortDateString() + "]必须大于明细物资的最近业务日期[" + maxCreateDate.ToShortDateString() + "]！";
                }
            }
            return sMsg;

        }
        #endregion

        public IList GetFiscalYear()
        {
            IList lstArr = new ArrayList();
            string sSQL = "select distinct t.fiscalyear from RESFISCALPERIODDET t order by fiscalyear asc";
            int iYear = 0;
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                {
                    iYear = int.Parse(oDataRow["fiscalyear"].ToString());
                    lstArr.Add(iYear);
                }
            }
            return lstArr;
        }
        /// <summary>
        /// 查询成本分析
        /// </summary>
        /// <param name="sProfessionCategory"></param>
        /// <param name="sProjectId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public IList QueryStockCost(string sProjectId, int iYear, int iMonth)
        {
            DataTable oTable = null;
            string sWhereCurrentMonth = string.Empty;
            string sWhereTotal = string.Empty;
            IList lstArr = new ArrayList();
            string sSQL = @"select    materialName,type,
                            sum(chkConfirmMoney) chkConfirmMoney,sum(chkOrderMoney)chkOrderMoney ,
                            sum(mvInConfirmMoney)mvInConfirmMoney ,sum(mvInOrderMoney)mvInOrderMoney,
                            sum(mvOutConfirmMoney)mvOutConfirmMoney,sum(mvOutOrderMoney)mvOutOrderMoney,
                            sum( stkInConfirmMoney)stkInConfirmMoney,sum( stkInOrderMoney)stkInOrderMoney,
                            sum( stkOutConfirmMoney)stkOutConfirmMoney,sum( stkOutOrderMoney)stkOutOrderMoney,
                            sum(balConfirmMoney)balConfirmMoney,sum(balOrderMoney) balOrderMoney,
                            sum(invConfirmMoney) invConfirmMoney,sum(invOrderMoney) invOrderMoney
                            from (
                            ---本月
                            select  nvl(t1.special,'其他') materialName, 0 type,
                            confirmmoney  chkConfirmMoney,t.money chkOrderMoney,0 mvInConfirmMoney ,0 mvInOrderMoney,
                            0 mvOutConfirmMoney,0 mvOutOrderMoney,0 stkInConfirmMoney,0 stkInOrderMoney,
                            0 stkOutConfirmMoney,0 stkOutOrderMoney,0 balConfirmMoney,0 balOrderMoney,
                            0  invConfirmMoney,0 invOrderMoney
                            from thd_stockinbaldetail t  join thd_stockinbalmaster t1 on t.parentid=t1.id  and t1.special is not null {0} ---结算 
                            union all
                             select * from (
                                 select  getspecialbysyscode(t1.accounttasknodesyscode)  materialName ,0 type,
                                 0  chkConfirmMoney,0 chkOrderMoney,0 mvInConfirmMoney ,0 mvInOrderMoney,
                                 0 mvOutConfirmMoney,0 mvOutOrderMoney,0 stkInConfirmMoney,0 stkInOrderMoney,
                                 0 stkOutConfirmMoney,0 stkOutOrderMoney,
                                 t2.accounttotalprice balConfirmMoney,t2.contractincometotal balOrderMoney,0  invConfirmMoney,0 invOrderMoney
                                from thd_projecttaskaccountbill t
                                join thd_projecttaskdetailaccount t1 on t.id=t1.parentid   
                                join thd_projecttaskdtlacctsubject t2 on t1.id=t2.parentid   
                                join (select t1.begindate,t1.enddate from  RESFISCALPERIOD t
                                        join resfiscalperioddet t1 on t.fiscalid=t1.fiscalid
                                        where t1.fiscalyear={3} and t1.fiscalmonth={4}
                                      ) t3 on   t.createdate   between t3.begindate and t3. enddate
                                 where t.theprojectguid='{2}')where materialName is not null  ---- in ('水','电','风','智能建筑','电梯','其他')  劳务结算消耗
                              union all
                            select  nvl(t1. professioncategory,'其他') materialName, 0 type,
                            0  chkConfirmMoney,0 chkOrderMoney,
                            (case t1.thestockinoutkind when 3 then t.confirmmoney when 4 then t.confirmmoney else 0 end) mvInConfirmMoney ,
                            (case t1.thestockinoutkind when 3 then t.money when 4 then t.money else 0 end) mvInOrderMoney,
                            0 mvOutConfirmMoney,0 mvOutOrderMoney,
                            (case t1.thestockinoutkind when 0 then t.confirmmoney when 1 then t.confirmmoney else 0 end) stkInConfirmMoney,
                            (case t1.thestockinoutkind when 0 then t.money when 1  then t.money else 0 end) stkInOrderMoney,
                            0 stkOutConfirmMoney,0 stkOutOrderMoney,0 balConfirmMoney,0 balOrderMoney,
                            0 invConfirmMoney,0 invOrderMoney 
                            from thd_stkstockindtl t join thd_stkstockin t1 on t.parentid=t1.id       and t1.special='安装'     {0}      ---入库    
                             union all
                            select  nvl(t1.professioncategory,'其他') materialName ,0 type,
                            0  chkConfirmMoney,0 chkOrderMoney,0 mvInConfirmMoney ,0 mvInOrderMoney,
                            (case t1.thestockinoutkind when 3 then t.confirmmoney when 4 then t.confirmmoney else 0 end) mvOutConfirmMoney,
                            (case t1.thestockinoutkind when 3 then t.money when 4 then t.money else 0 end) mvOutOrderMoney,
                            0 stkInConfirmMoney,0 stkInOrderMoney,
                            (case t1.thestockinoutkind when 0 then t.confirmmoney when 1 then t.confirmmoney else 0 end) stkOutConfirmMoney,
                            (case t1.thestockinoutkind when 0 then t.money when 1  then t.money else 0 end)  stkOutOrderMoney,
                            0 balConfirmMoney,0 balOrderMoney, 0  invConfirmMoney,0  invOrderMoney 
                            from thd_stkstockoutdtl t join thd_stkstockout t1 on t.parentid=t1.id        and t1.special='安装'     {0}     ---出库   
                             union  all
                            select    nvl(t1.professioncategory  ,'其他')     materialName, 0 type,
                            0  chkConfirmMoney,0 chkOrderMoney,0 mvInConfirmMoney ,0 mvInOrderMoney,
                            0 mvOutConfirmMoney,0 mvOutOrderMoney,0 stkInConfirmMoney,0 stkInOrderMoney,
                            0 stkOutConfirmMoney,0 stkOutOrderMoney,0 balConfirmMoney,0 balOrderMoney,
                            t.confirmmoney  invConfirmMoney,t.money invOrderMoney  
                            from thd_stockinventorydetail t join thd_stockinventorymaster t1 on t.parentid=t1.id  and t1.state=5 and t1.special='安装'   {0}   ---月度盘点 
                             union all    ---累计
                            select  nvl(t1.special,'其他') materialName, 1 type,
                            confirmmoney  chkConfirmMoney,t.money chkOrderMoney, 0 mvInConfirmMoney ,0 mvInOrderMoney,
                            0 mvOutConfirmMoney,0 mvOutOrderMoney, 0 stkInConfirmMoney,0 stkInOrderMoney,
                            0 stkOutConfirmMoney,0 stkOutOrderMoney, 0 balConfirmMoney,0 balOrderMoney,
                            0  invConfirmMoney,0 invOrderMoney
                             from thd_stockinbaldetail t  join thd_stockinbalmaster t1 on t.parentid=t1.id   and t1.special is not null {1}    ---结算 
                        union all
                            select * from (
                                 select  getspecialbysyscode(t1.accounttasknodesyscode)   materialName ,1 type,
                                 0  chkConfirmMoney,0 chkOrderMoney,0 mvInConfirmMoney ,0 mvInOrderMoney,
                                 0 mvOutConfirmMoney,0 mvOutOrderMoney,0 stkInConfirmMoney,0 stkInOrderMoney,
                                 0 stkOutConfirmMoney,0 stkOutOrderMoney,
                                 t2.accounttotalprice balConfirmMoney,t2.contractincometotal balOrderMoney,0  invConfirmMoney,0 invOrderMoney
                                from thd_projecttaskaccountbill t
                                join thd_projecttaskdetailaccount t1 on t.id=t1.parentid   
                                join thd_projecttaskdtlacctsubject t2 on t1.id=t2.parentid   
                                 where t.theprojectguid='{2}') where materialName is not null     --- in ('水','电','风','智能建筑','电梯','其他') 劳务结算消耗
                              union all
                            select   nvl(t1.professioncategory,'其他')  materialName, 1 type,
                            0  chkConfirmMoney,0 chkOrderMoney,
                            (case t1.thestockinoutkind when 3 then t.confirmmoney when 4 then t.confirmmoney else 0 end) mvInConfirmMoney ,
                            (case t1.thestockinoutkind when 3 then t.money when 4 then t.money else 0 end) mvInOrderMoney,
                            0 mvOutConfirmMoney,0 mvOutOrderMoney,
                            (case t1.thestockinoutkind when 0 then t.confirmmoney when 1 then t.confirmmoney else 0 end) stkInConfirmMoney,
                            (case t1.thestockinoutkind when 0 then t.money when 1  then t.money else 0 end) stkInOrderMoney,
                            0 stkOutConfirmMoney,0 stkOutOrderMoney,0 balConfirmMoney,0 balOrderMoney,
                            0 invConfirmMoney,0 invOrderMoney 
                            from thd_stkstockindtl t join thd_stkstockin t1 on t.parentid=t1.id    and t1.special='安装'   {1}       ---入库    
                             union all
                            select     nvl(t1.professioncategory,'其他') materialName  ,1 type,
                            0  chkConfirmMoney,0 chkOrderMoney,0 mvInConfirmMoney ,0 mvInOrderMoney,
                            (case t1.thestockinoutkind when 3 then t.confirmmoney when 4 then t.confirmmoney else 0 end) mvOutConfirmMoney,
                            (case t1.thestockinoutkind when 3 then t.money when 4 then t.money else 0 end) mvOutOrderMoney,
                            0 stkInConfirmMoney,0 stkInOrderMoney,
                            (case t1.thestockinoutkind when 0 then t.confirmmoney when 1 then t.confirmmoney else 0 end) stkOutConfirmMoney,
                            (case t1.thestockinoutkind when 0 then t.money when 1  then t.money else 0 end)  stkOutOrderMoney,
                            0 balConfirmMoney,0 balOrderMoney, 0  invConfirmMoney,0  invOrderMoney 
                            from thd_stkstockoutdtl t join thd_stkstockout t1 on t.parentid=t1.id     and t1.special='安装'  {1} ---出库   
                             union  all
                            select   nvl(t1.professioncategory,'其他')  materialName, 1 type,
                            0  chkConfirmMoney,0 chkOrderMoney,0 mvInConfirmMoney ,0 mvInOrderMoney,
                            0 mvOutConfirmMoney,0 mvOutOrderMoney,0 stkInConfirmMoney,0 stkInOrderMoney,
                            0 stkOutConfirmMoney,0 stkOutOrderMoney,0 balConfirmMoney,0 balOrderMoney,
                            t.confirmmoney  invConfirmMoney,t.money invOrderMoney  
                            from thd_stockinventorydetail t join thd_stockinventorymaster t1 on t.parentid=t1.id  and t1.state=5 and t1.special='安装'  {1}   ---月度盘点 
                            ) group by   materialName ,type  order by materialName,type asc
                             ";
            string sSQLCount = @"select sum(chkCount)chkCount,sum(chkRedCount)chkRedCount,sum(stkOutCount)stkOutCount,sum(stkOutRedCount)stkOutRedCount,
                            sum(stkInCount)stkInCount,sum(stkInRedCount)stkInRedCount,sum(mvInCount)mvInCount,sum(mvInRedCount)mvInRedCount,
                            sum(mvOutCount)mvOutCount,sum(mvOutRedCount)mvOutRedCount,sum(sumTotal)sumTotal
                            from (   ---本月
                            select (case t1.thestockinoutkind when 0 then 1 else 0 end) chkCount,
                            (case t1.thestockinoutkind when 1 then 1 else 0 end)  chkRedCount,0 stkOutCount,0 stkOutRedCount,
                            0 stkInCount,0 stkInRedCount,0 mvInCount,0 mvInRedCount,
                            0 mvOutCount,0 mvOutRedCount,1 sumTotal
                             from thd_stockinbalmaster t1 where t1.special is not null  {0}  ---结算   
                              union all
                            select  0 chkCount,0 chkRedCount,0 stkOutCount,0 stkOutRedCount,
                            (case t1.thestockinoutkind when 0 then 1 else 0 end) stkInCount,
                            (case t1.thestockinoutkind when 1 then 1 else 0 end) stkInRedCount,
                            (case t1.thestockinoutkind when 3 then 1 else 0 end) mvInCount,
                            (case t1.thestockinoutkind when 4 then 1 else 0 end) mvInRedCount,
                            0 mvOutCount,0 mvOutRedCount,1 sumTotal
                            from thd_stkstockin t1 where t1.special='安装'  {0}     ---入库    
                             union all
                            select   
                            0 chkCount,0 chkRedCount,
                            (case t1.thestockinoutkind when 0 then 1 else 0 end) stkOutCount,
                            (case t1.thestockinoutkind when 1 then 1 else 0 end) stkOutRedCount,
                            0 stkInCount,0 stkInRedCount,0 mvInCount,0 mvInRedCount,
                            (case t1.thestockinoutkind when 3 then 1 else 0 end) mvOutCount,
                            (case t1.thestockinoutkind when 4 then 1 else 0 end) mvOutRedCount,
                            1 sumTotal
                            from thd_stkstockout t1 where t1.special='安装'  {0}   ---出库   
                             union  all
                            select 0 chkCount,0 chkRedCount,0 stkOutCount,0 stkOutRedCount,
                            0 stkInCount,0 stkInRedCount,0 mvInCount,0 mvInRedCount,
                            0 mvOutCount,0 mvOutRedCount,0 sumTotal
                            from thd_stockinventorymaster t1 where t1.special='安装' and t1.state=5  {0}   ---月度盘点 
                            ) ";
            sWhereCurrentMonth = string.Format("  and   t1.PROJECTID='{0}' and  t1.createyear={1} and t1.createmonth={2}", sProjectId, iYear, iMonth);
            sWhereTotal = string.Format("  where   t1.PROJECTID='{0}' ", sProjectId);
            sSQL = string.Format(sSQL, sWhereCurrentMonth, sWhereTotal, sProjectId, iYear, iMonth);
            sSQLCount = string.Format(sSQLCount, sWhereCurrentMonth);
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            // command.CommandText = sSQL;//获取金额统计信息
            #region 存储过程
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sunfun.GetCostAnaReport";
            // procedure GetCostAnaReport(sProjectId varchar2,iYear int ,iMonth int,dtSunReport out SunFun.DataSet)
            System.Data.OracleClient.OracleParameter oParemeter = null;
            oParemeter = new System.Data.OracleClient.OracleParameter("sProjectId", sProjectId);
            oParemeter.OracleType = System.Data.OracleClient.OracleType.VarChar;
            command.Parameters.Add(oParemeter);
            oParemeter = new System.Data.OracleClient.OracleParameter("iYear", iYear);
            oParemeter.OracleType = System.Data.OracleClient.OracleType.Int16;
            command.Parameters.Add(oParemeter);
            oParemeter = new System.Data.OracleClient.OracleParameter("iMonth", iMonth);
            oParemeter.OracleType = System.Data.OracleClient.OracleType.Int16;
            command.Parameters.Add(oParemeter);
            oParemeter = new System.Data.OracleClient.OracleParameter("dtSunReport", System.Data.OracleClient.OracleType.Cursor);

            oParemeter.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(oParemeter);

            #endregion
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                oTable = dataSet.Tables[0];
            }
            lstArr.Insert(lstArr.Count, oTable);
            command = cnn.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sSQLCount;//获取单据数量统计信息
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                oTable = dataSet.Tables[0];
            }
            lstArr.Insert(lstArr.Count, oTable);
            return lstArr;
        }

        public DataTable QueryStoreInventory(string sProjectID, string sUserPartID,string sAccountTaskSysCode, string sUserRand,string sProfessionCat, int iYear, int iMonth)
        {
            DataTable oTable = null;
            int iLastYear = 0;
            int iLastMonth = 0;
            if (iMonth == 1)
            {
                iLastYear = iYear - 1;
                iLastMonth = 12;
            }
            else
            {
                iLastYear = iYear  ;
                iLastMonth = iMonth-1;
            }
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SunFun.InsertSpecial";
            System.Data.OracleClient.OracleParameter oParameter = new System.Data.OracleClient.OracleParameter("sProjectId", sProjectID);
            oParameter.OracleType = System.Data.OracleClient.OracleType.VarChar;
            command.Parameters.Add(oParameter);
            command.ExecuteNonQuery();
           // sProjectID = "2n70jRll19TeOERC9yD9AX";
            //本月出库+上个月盘点数-本月盘点数=这个月消耗
            //in('水','电','风','智能建筑','电梯','其他') 
            string sSQL = @" select  resourcetypeguid,materialname,diagramnumber,materialspec,matstandardunitname,    money,round(decode(quantity,0,0,null,0,money/quantity),2)  price,
   confirmmoney,round(decode(quantity,0,0,null,0,confirmmoney/quantity),2)confirmprice,
  quantity, ACCUSAGEQNY, 0 ljquantity,0 LJACCUSAGEQNY,jsConfirmMoney,jsOrderMoney,round(decode(quantity+ACCUSAGEQNY,0,0,null,0,(jsConfirmMoney+confirmmoney)/(quantity+ACCUSAGEQNY)),2) totalConfirmPrice,round(decode(quantity+ACCUSAGEQNY,0,0,null,0,(jsOrderMoney+money)/(quantity+ACCUSAGEQNY)),2) totalPrice,price1
 from (
 select resourcetypeguid,materialname,diagramnumber,materialspec,matstandardunitname,sum(money) money,sum(confirmmoney)confirmmoney,
 sum(quantity)quantity,sum(ACCUSAGEQNY)ACCUSAGEQNY,sum(ljquantity)ljquantity,sum(LJACCUSAGEQNY)LJACCUSAGEQNY,sum(jsConfirmMoney) jsConfirmMoney,sum(jsOrderMoney) jsOrderMoney,price1
 from (
select nvl(t.resourcetypeguid,t1.resourcetypeguid)resourcetypeguid,nvl(t.materialname,t1.RESOURCETYPENAME)materialname,nvl(t.diagramnumber,t1.diagramnumber)diagramnumber , nvl( t.materialspec,t1.RESOURCETYPESPEC)materialspec, nvl(t.matstandardunitname,t1.QUANTITYUNITNAME)matstandardunitname,
round(nvl(t.money,0),2)money, round(nvl(t.price,0),2)price, round(nvl(t.confirmmoney,0),2)confirmmoney, 
                 round(nvl(t.confirmprice,0),2)confirmprice,round( nvl(t.quantity,0),2)quantity,round(nvl(t1.ACCUSAGEQNY ,0),2)ACCUSAGEQNY 
,round(nvl(ljquantity,0),2)ljquantity ,round(nvl(LJACCUSAGEQNY,0),2)LJACCUSAGEQNY 
 ,round(nvl( jsConfirmMoney,0),2)jsConfirmMoney,round(nvl(jsOrderMoney,0),2)jsOrderMoney,price1
                from (select  resourcetypeguid,materialname,  materialspec, matstandardunitname,diagramnumber,
                 money,decode(quantity,0,0,round(money/quantity,2))price,
                  confirmmoney,decode(quantity,0,0,round(confirmmoney/quantity,2))confirmprice, quantity ,ljquantity ,price1
                from(
                select  resourcetypeguid, materialname,  materialspec, matstandardunitname, diagramnumber,
                sum( money)money,sum(confirmmoney)confirmmoney,sum(quantity)quantity ,sum(  ljquantity) ljquantity,price1
                from (----本月盘点数
                select t1.material resourcetypeguid,to_char(t1.materialname) materialname, nvl(t1.diagramnumber,'') diagramnumber, to_char(t1.materialspec)materialspec,to_char(t1.matstandardunitname) matstandardunitname,
                -sum(t1.money)money,-sum(t1.confirmmoney)confirmmoney,
                -sum(t1.inventoryquantity)quantity , -sum(t1.inventoryquantity) ljquantity ,t1.price price1
                 from thd_stockinventorymaster t
                join thd_stockinventorydetail t1 on t.id=t1.parentid and t.state=5  and t1.materialcode like 'I%'
                where t.projectid='{0}' and  t.special='安装' 
                and t.createyear={1} and t.createmonth={2} " + (string.IsNullOrEmpty(sUserPartID) ? "  " : "  and IsUsedpartparentAndChild('" + sUserPartID + "',t.userpart)=1  ") + (string.IsNullOrEmpty(sUserRand) ? "  " : "   and t.usedrankid='" + sUserRand + "'  ") + (string.IsNullOrEmpty(sProfessionCat) ? "  " : "   and t.PROFESSIONCATEGORY='" + sProfessionCat + "'  ") +
                @"        group by t1.material,t1.materialname, t1.materialspec,t1.matstandardunitname,t1.diagramnumber ,t1.price
                union all  ----上个月盘点数
                select t1.material resourcetypeguid,to_char(t1.materialname) materialname, nvl(t1.diagramnumber,'') diagramnumber,to_char(t1.materialspec)materialspec,to_char(t1.matstandardunitname) matstandardunitname,
                sum(t1.money)money,sum(t1.confirmmoney)confirmmoney,
                sum(t1.inventoryquantity)quantity ,0 ljquantity ,t1.price price1
                 from thd_stockinventorymaster t
                join thd_stockinventorydetail t1 on t.id=t1.parentid and t.state=5 and t1.materialcode like 'I%'
                where t.projectid='{0}' and  t.special='安装' 
                and t.createyear={3} and t.createmonth={4}     " + (string.IsNullOrEmpty(sUserPartID) ? "  " : " and IsUsedpartparentAndChild('" + sUserPartID + "',t.userpart)=1 ") + (string.IsNullOrEmpty(sUserRand) ? "  " : "   and t.usedrankid='" + sUserRand + "'  ") + (string.IsNullOrEmpty(sProfessionCat) ? "  " : "   and t.PROFESSIONCATEGORY='" + sProfessionCat + "'  ") +
               @"   group by t1.material,t1.materialname, t1.materialspec,t1.matstandardunitname,t1.diagramnumber,t1.price
                union all   ----本月出库
                select t1.material resourcetypeguid,to_char(t1.materialname) materialname, nvl(t1.diagramnumber,'') diagramnumber,to_char(t1.materialspec)materialspec,to_char(t1.matstandardunitname) matstandardunitname,
                sum(t1.money)money,sum(t1.confirmmoney)confirmmoney,
                sum(t1.quantity)quantity ,0 ljquantity ,t1.price price1
                 from thd_stkstockout t
                join thd_stkstockoutdtl t1 on t.id=t1.parentid   and t1.materialcode like 'I%'  " + (string.IsNullOrEmpty(sUserPartID) ? "  " : " and IsUsedpartparentAndChild( '" + sUserPartID + "',t1.usedpart)=1   ") +
                @"where t.projectid='{0}' and  t.special='安装' 
                and t.createyear={1} and t.createmonth={2}  " + (string.IsNullOrEmpty(sUserRand) ? "  " : "   and t.SUPPLIERRELATION='" + sUserRand + "'  ") + (string.IsNullOrEmpty(sProfessionCat) ? "  " : "   and t.PROFESSIONCATEGORY='" + sProfessionCat + "'  ") +
                @"   and t.THESTOCKINOUTKIND in (0,1)
                group by t1.material,t1.materialname, t1.materialspec,t1.matstandardunitname,t1.diagramnumber ,t1.price
                union all   ----累计出库
                select t1.material resourcetypeguid,to_char(t1.materialname) materialname,  nvl(t1.diagramnumber,'') diagramnumber,to_char(t1.materialspec)materialspec,to_char(t1.matstandardunitname) matstandardunitname,
                0 money,0 confirmmoney,  0 quantity
                ,sum(t1.quantity) ljquantity ,t1.price price1
                 from thd_stkstockout t
                join thd_stkstockoutdtl t1 on t.id=t1.parentid   and t1.materialcode like 'I%'  " + (string.IsNullOrEmpty(sUserPartID) ? "  " : " and IsUsedpartparentAndChild( '" + sUserPartID + "',t1.usedpart)=1   ") +
                @"where t.projectid='{0}' and  t.special='安装' 
                and t.createyear*100+t.createmonth<={1}*100+{2}  " + (string.IsNullOrEmpty(sUserRand) ? "  " : "   and t.SUPPLIERRELATION='" + sUserRand + "'  ") + (string.IsNullOrEmpty(sProfessionCat) ? "  " : "   and t.PROFESSIONCATEGORY='" + sProfessionCat + "'  ") +
                @"   and t.THESTOCKINOUTKIND in (0,1)
                group by t1.material,t1.materialname, t1.materialspec,t1.matstandardunitname,t1.diagramnumber,t1.price

) group by resourcetypeguid, materialname,  materialspec, matstandardunitname,diagramnumber,price1)) t

               full join (
 select resourcetypeguid,RESOURCETYPENAME,diagramnumber,RESOURCETYPESPEC, QUANTITYUNITNAME,  ACCUSAGEQNY,
                  decode(ACCUSAGEQNY,0,0,null,0,round(ACCWORKQNYMoney/ACCUSAGEQNY,2))ACCWORKQNYPrice, ACCWORKQNYMoney,
                   decode(ACCUSAGEQNY,0,0,null,0,round(money/ACCUSAGEQNY,2)) price,
                   money, LJACCUSAGEQNY
,   jsConfirmMoney,  jsOrderMoney
from (
 select resourcetypeguid,RESOURCETYPENAME,diagramnumber,RESOURCETYPESPEC, QUANTITYUNITNAME, sum(ACCUSAGEQNY)ACCUSAGEQNY,
                  sum(ACCWORKQNYMoney )ACCWORKQNYMoney,
                 sum( money) money,sum(nvl(LJACCUSAGEQNY,0) )LJACCUSAGEQNY
, sum(jsConfirmMoney) jsConfirmMoney,sum(jsOrderMoney) jsOrderMoney
 from(
select resourcetypeguid,RESOURCETYPENAME,diagramnumber,RESOURCETYPESPEC,QUANTITYUNITNAME, ACCUSAGEQNY,
                 decode(ACCUSAGEQNY,0,0,null,0,round(ACCWORKQNYMoney/ACCUSAGEQNY,2))ACCWORKQNYPrice, ACCWORKQNYMoney ,
                 ttt2.price,ttt2.price*ACCUSAGEQNY money,LJACCUSAGEQNY 
, jsConfirmMoney,jsOrderMoney
                 from (
                 select resourcetypeguid,RESOURCETYPENAME,diagramnumber,RESOURCETYPESPEC,QUANTITYUNITNAME, sum(ACCUSAGEQNY)ACCUSAGEQNY,
                 sum(ACCWORKQNYMoney)ACCWORKQNYMoney ,sum(LJACCUSAGEQNY) LJACCUSAGEQNY 
 , sum(jsConfirmMoney) jsConfirmMoney,sum(jsOrderMoney)jsOrderMoney
                 from ( 
                select t2.resourcetypeguid, nvl(t2.RESOURCETYPENAME,'') RESOURCETYPENAME,nvl(t2.diagramnumber,'')diagramnumber, nvl(t2.RESOURCETYPESPEC,'')RESOURCETYPESPEC,
                nvl(t2.QUANTITYUNITNAME ,'')QUANTITYUNITNAME, nvl(t2.ACCUSAGEQNY,0.00)ACCUSAGEQNY ,
                nvl(t2.ACCUSAGEQNY,0.00)*nvl(t2.CONTRACTQUANTITYPRICE,0.00) ACCWORKQNYMoney,0 LJACCUSAGEQNY 
 ,t2.contractincometotal jsConfirmMoney,t2.accounttotalprice jsOrderMoney
                from thd_projecttaskaccountbill  t
                join thd_projecttaskdetailaccount t1 on t.id=t1.parentid  " + (string.IsNullOrEmpty (sUserPartID)? "  " :"  and  IsUsedpartparentAndChild('"+sUserPartID+"' , t1.ACCOUNTTASKNODEGUID)=1  " )+
                @"join THD_SUBCONTRACTPROJECT t4 on t1.bearerguid=t4.id  " + (string.IsNullOrEmpty(sUserRand) ? "  " : "   and t4.bearerorgguid='" + sUserRand + "'  ") +
                @"join thd_projecttaskdtlacctsubject t2 on t1.id=t2.parentid   " + (string.IsNullOrEmpty(sProfessionCat) ? "   and exists( select 1 from TEMP_SPECIAL1 t  where instr(t1.accounttasknodesyscode,t.syscode)>0 )  " : "   and SunFun.GetSpecial1(t1.accounttasknodesyscode)='" + sProfessionCat + "'  ") +
                @"  JOIN (
                SELECT to_date(to_char(tt1.begindate,'YYYY-MM-DD') ,'YYYY-MM-DD') begindate,to_date(to_char(tt1.enddate,'YYYY-MM-DD'),'YYYY-MM-DD') enddate 
                FROM RESFISCALPERIOD TT JOIN RESFISCALPERIODDET TT1 ON TT.FISCALID=TT1.FISCALID AND TT1.FISCALYEAR={1} AND TT1.FISCALMONTH={2}
                WHERE TT.BUSMODULEID=16) t3 on to_date(to_char(t.createdate,'YYYY-MM-DD') ,'YYYY-MM-DD') between t3.begindate and enddate
                where t.THEPROJECTGUID='{0}' " + (string.IsNullOrEmpty(sAccountTaskSysCode) ? "" : "and t.ACCOUNTTASKSYSCODE like '" + sAccountTaskSysCode + "%'") +
               @" )
                group by resourcetypeguid,RESOURCETYPENAME,RESOURCETYPESPEC,QUANTITYUNITNAME,diagramnumber

                union all
                select resourcetypeguid,RESOURCETYPENAME, diagramnumber,RESOURCETYPESPEC,QUANTITYUNITNAME, 0 ACCUSAGEQNY,
                0 ACCWORKQNYMoney ,SUM(ACCUSAGEQNY ) LJACCUSAGEQNY
,0 jsConfirmMoney,0 jsOrderMoney
                 from ( 
                select t2.resourcetypeguid, nvl(t2.RESOURCETYPENAME,'') RESOURCETYPENAME, nvl(t2.diagramnumber,'')diagramnumber,nvl(t2.RESOURCETYPESPEC,'')RESOURCETYPESPEC,
                nvl(t2.QUANTITYUNITNAME ,'')QUANTITYUNITNAME,   nvl(t2.ACCUSAGEQNY,0.00)ACCUSAGEQNY 
                from thd_projecttaskaccountbill  t
                join thd_projecttaskdetailaccount t1 on t.id=t1.parentid  " + (string.IsNullOrEmpty(sUserPartID) ? "  " : "  and  IsUsedpartparentAndChild('" + sUserPartID + "' , t1.ACCOUNTTASKNODEGUID)=1  ") +
                @"join THD_SUBCONTRACTPROJECT t4 on t1.bearerguid=t4.id  " + (string.IsNullOrEmpty(sUserRand) ? "  " : "   and t4.bearerorgguid='" + sUserRand + "'  ") +
                @"join thd_projecttaskdtlacctsubject t2 on t1.id=t2.parentid   " + (string.IsNullOrEmpty(sProfessionCat) ? "  and exists( select 1 from TEMP_SPECIAL1 t where instr(t1.accounttasknodesyscode,t.syscode)>0 )  " : "   and SunFun.GetSpecial1(t1.accounttasknodesyscode)='" + sProfessionCat + "'  ") +
                @"  JOIN (
                SELECT to_date(to_char(tt1.begindate,'YYYY-MM-DD') ,'YYYY-MM-DD') begindate,to_date(to_char(tt1.enddate,'YYYY-MM-DD'),'YYYY-MM-DD') enddate 
                FROM RESFISCALPERIOD TT JOIN RESFISCALPERIODDET TT1 ON TT.FISCALID=TT1.FISCALID AND TT1.FISCALYEAR={1} AND TT1.FISCALMONTH={2}
                WHERE TT.BUSMODULEID=16) t3 on to_date(to_char(t.createdate,'YYYY-MM-DD') ,'YYYY-MM-DD') <= enddate
                where t.THEPROJECTGUID='{0}' " + (string.IsNullOrEmpty(sAccountTaskSysCode) ? "" : "and t.ACCOUNTTASKSYSCODE like '" + sAccountTaskSysCode + "%'") +
               @" )
                group by resourcetypeguid,RESOURCETYPENAME,RESOURCETYPESPEC,QUANTITYUNITNAME,diagramnumber
                
                ) ttt
                join  resmaterial ttt1 on ttt1.materialid= ttt.resourcetypeguid   and ttt1.matcode like 'I%'
                left join (
                select  material,projectid,decode(quantity,null,0,0,0,round(money/quantity,2)) price
                from (
                select tt1.material,tt.projectid, sum(nvl(tt1.SUPPLYPRICE,0)*tt1.quantity) money,sum(tt1.quantity) quantity 
                from thd_supplyordermaster tt 
                join thd_supplyorderdetail tt1 on tt.id=tt1.parentid 
                where  tt.special='安装'  
                group by tt1.material,tt.projectid )
                ) ttt2 on  ttt1.materialid=ttt2.material  and ttt2.projectid='{0}' ) group by resourcetypeguid,RESOURCETYPENAME,diagramnumber,RESOURCETYPESPEC,QUANTITYUNITNAME)) t1
                on t1.resourcetypeguid=t.resourcetypeguid and t1.QUANTITYUNITNAME=t.matstandardunitname 
                 ) 
                group by resourcetypeguid, materialname,materialspec,matstandardunitname ,diagramnumber, price1)    where  quantity<>0 or  ACCUSAGEQNY <>0";
            command = cnn.CreateCommand();
            command.CommandType = CommandType.Text;
            sSQL = string.Format(sSQL, sProjectID, iYear, iMonth,iLastYear ,iLastMonth );
            command.CommandText = sSQL;//获取金额统计信息
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
               oTable = dataSet.Tables[0];
            }
            return oTable;
        }
        public DataTable QueryStoreBalance(string sProjectID, string sUserPartID, string sUserRand,   int iYear, int iMonth)
        {
            DataTable oTable = null;

            string sSQL = @"   select resourcetypeguid,RESOURCETYPENAME,RESOURCETYPESPEC,QUANTITYUNITNAME,accounttasknodesyscode,ACCUSAGEQNY,
                 decode(ACCUSAGEQNY,0,0,null,0,round(ACCWORKQNYMoney/ACCUSAGEQNY,2))ACCWORKQNYPrice, ACCWORKQNYMoney ,
                 ttt2.price,ttt2.price*ACCUSAGEQNY money
                 from (
                 select resourcetypeguid,RESOURCETYPENAME,RESOURCETYPESPEC,QUANTITYUNITNAME,accounttasknodesyscode,sum(ACCUSAGEQNY)ACCUSAGEQNY,
                 sum(ACCWORKQNYMoney)ACCWORKQNYMoney 
                 from ( 
                select t2.resourcetypeguid, nvl(t2.RESOURCETYPENAME,'') RESOURCETYPENAME, nvl(t2.RESOURCETYPESPEC,'')RESOURCETYPESPEC,
                nvl(t2.QUANTITYUNITNAME ,'')QUANTITYUNITNAME, nvl(t2.ACCUSAGEQNY,0.00)ACCUSAGEQNY ,
                nvl(t2.ACCUSAGEQNY,0.00)*nvl(t2.CONTRACTQUANTITYPRICE,0.00) ACCWORKQNYMoney ,getspecialbysyscode(t1.accounttasknodesyscode)accounttasknodesyscode
                from thd_projecttaskaccountbill  t
                join thd_projecttaskdetailaccount t1 on t.id=t1.parentid     and t1.ACCOUNTTASKNODEGUID='{3}' --and t1.BEARERGUID='1_EwZXkH9Cq8VTHZm39A1y'
                join THD_SUBCONTRACTPROJECT t4 on t1.bearerguid=t4.id and t4.bearerorgguid='{4}'
                join thd_projecttaskdtlacctsubject t2 on t1.id=t2.parentid 
                JOIN (
                SELECT to_date(to_char(tt1.begindate,'YYYY-MM-DD') ,'YYYY-MM-DD') begindate,to_date(to_char(tt1.enddate,'YYYY-MM-DD'),'YYYY-MM-DD') enddate 
                FROM RESFISCALPERIOD TT JOIN RESFISCALPERIODDET TT1 ON TT.FISCALID=TT1.FISCALID AND TT1.FISCALYEAR={1} AND TT1.FISCALMONTH={2}
                WHERE TT.BUSMODULEID=16) t3 on to_date(to_char(t.createdate,'YYYY-MM-DD') ,'YYYY-MM-DD') between t3.begindate and enddate
                where t.THEPROJECTGUID='{0}' 
                )
                group by resourcetypeguid,RESOURCETYPENAME,RESOURCETYPESPEC,QUANTITYUNITNAME,accounttasknodesyscode) ttt
                join  resmaterial ttt1 on ttt1.materialid= ttt.resourcetypeguid  
                 join (
                select  material,projectid,decode(quantity,null,0,0,0,round(money/quantity,2)) price
                from (
                select tt1.material,tt.projectid, sum(nvl(tt1.SUPPLYPRICE,0)*tt1.quantity) money,sum(tt1.quantity) quantity 
                from thd_supplyordermaster tt 
                join thd_supplyorderdetail tt1 on tt.id=tt1.parentid 
                where  tt.special='安装'  
                group by tt1.material,tt.projectid )
                ) ttt2 on  ttt1.materialid=ttt2.material  and ttt2.projectid='{0}'";
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            //sSQL = string.Format(sSQL, sProjectID, iYear, iMonth, sUserRand, sUserPartID, sProfessionCat);
            sSQL = string.Format(sSQL, sProjectID, iYear, iMonth, sUserRand, sUserPartID);
            //sSQL = string.Format(sSQL, sProjectID, iYear, iMonth, sUserPartID, sUserRand, sProfessionCat, iLastYear, iLastMonth);
            command.CommandText = sSQL;//获取金额统计信息
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                oTable = dataSet.Tables[0];
            }
            return oTable;
        }

        public DataTable QueryMaterialAccount(string sProjectID,string sStartDate,string sEndDate)
        {
            DataTable oTable = null;

            string sSQL = @" select * from (
 select  t1.id stkInID,t2.id stkInID1,t4.mvInID,t5.mvOutID,t6.stkOutID, t.professioncategory ,nvl(t.supplierrelationname,nvl(t4.mvinProject,nvl(t5.mvOutProject,''))) supplierrelationname,
                           t1.materialname,t1.materialspec,t1.diagramnumber, t1.matstandardunitname , nvl(t1.confirmprice,0) stkinConfirmPrice,t1.descript stkinDescript,  ---入库单
                           nvl(t2.balQuantity ,0)balQuantity,nvl(t2.balPrice ,0)balPrice,nvl(t2.balMoney,0)balMoney ,t2.balCode,t2.balDescript,t2.balCreatedate ,  ---验收单
                           nvl(t4.mvinQuantity,0.00)mvinQuantity,nvl( t4.mvinPrice,0.00)mvinPrice, nvl(t4.mvinMoney,0.00)mvinMoney,
                           nvl( t4.mvinCode,'')mvinCode, nvl(t4.mvinProject,'')mvinProject, nvl(t4.mvinDescript,'')mvinDescript, ---调入
                           nvl(t5.mvOutQuantity,0.00)mvOutQuantity,nvl(t5.mvOutPrice,0.00)mvOutPrice,nvl(t5.mvOutMoney,0.00)mvOutMoney,
                           nvl(t5.mvOutCode,'')mvOutCode,nvl(t5.mvOutProject,'')mvOutProject, nvl(t5.mvOutDescript,'')mvOutDescript,   ---调出
                           0 lostQuantity,0.00 lostPrice,0.00 lostMoney,'' lostCode ,'' lostDescript, ----报损
                           nvl(t6.stkOutQuantity,0.00)stkOutQuantity ,nvl(t6.stkOutMoney,0.00)stkOutMoney,
                           nvl(t6.stkOutCode,'')stkOutCode,nvl(t6.stkOutSupplierrelationname,'')stkOutSupplierrelationname ,t6.stkOutDescript  ---领料出库
                    from thd_stkstockin t ---收料
                    join thd_stkstockindtl t1 on t.id=t1.parentid  
                    left join ---验收
                    (
                         select (case tt.thestockinoutkind  when 0 then tt1.forwarddetailid  
                            when 1 then (select k1.forwarddetailid from thd_stockinbaldetail k1 where k1.id=tt1.forwarddetailid) end) id, 
                            nvl(tt1.quantity,0.00) balQuantity ,nvl(tt1.price,0.00)balPrice ,nvl(tt1.money,0.00) balMoney ,
                         tt.code balCode,tt1.descript balDescript,to_char(tt.createdate,'YYYY-MM-DD')balCreatedate 
                         from thd_stockinbalmaster tt
                         join thd_stockinbaldetail tt1 on tt1.parentid=tt.id  
                         where  tt.projectid='{0}' and  
                         to_date(to_char(tt.createdate,'YYYY-MM-DD'),'YYYY-MM-DD') between to_date('{1}','YYYY-MM-DD') and to_date('{2}','YYYY-MM-DD')
                    )t2 on t1.id=t2.id
                    left join ---调拨入库    
                    (
                         select  tt1.id, tt1.id mvInID,tt1.quantity mvinQuantity,tt1.price mvinPrice,tt1.money mvinMoney,tt.code mvinCode,
                           tt.moveoutprojectname mvinProject,tt1.descript mvinDescript
                           from thd_stkstockin tt  
                           join thd_stkstockindtl tt1 on tt1.parentid=tt.id  
                           where  tt.thestockinoutkind in(3,4) and   tt.projectid='{0}' and  ---调拨入库    
                           to_date(to_char(tt.createdate,'YYYY-MM-DD'),'YYYY-MM-DD') between to_date('{1}','YYYY-MM-DD') and to_date('{2}','YYYY-MM-DD')
                    ) t4 on  t4.id=t1.id    
                    left join ---调拨出库
                    (
                         select tt2.stockindtlid id, tt1.id mvOutID,tt2.quantity mvOutQuantity,tt1.moveprice mvOutPrice,nvl(tt2.quantity,0)*nvl(tt1.moveprice,0) mvOutMoney,
                           tt.code mvOutCode,tt.moveoutprojectname mvOutProject,tt1.descript mvOutDescript 
                           from thd_stkstockout tt  
                           join thd_stkstockoutdtl tt1 on tt.id=tt1.parentid  
                           join thd_stkstockoutdtlseq tt2 on tt2.stockoutdtlid=tt1.id
                          where  tt.thestockinoutkind in(3,4) and   tt.projectid='{0}' and   
                           to_date(to_char(tt.createdate,'YYYY-MM-DD'),'YYYY-MM-DD') between to_date('{1}','YYYY-MM-DD') and to_date('{2}','YYYY-MM-DD')
                    ) t5 on t5.id=t1.id
                    left join ---领料出库
                    (
                         select tt2.stockindtlid id, tt1.id stkOutID,tt2.quantity stkOutQuantity ,nvl(tt2.quantity,0)*nvl(tt2.price,0) stkOutMoney,tt.code stkOutCode,
                         tt.supplierrelationname stkOutSupplierrelationname ,tt1.descript stkOutDescript
                         from  thd_stkstockout tt 
                         join thd_stkstockoutdtl tt1 on tt1.parentid=tt.id  
                         join thd_stkstockoutdtlseq tt2 on tt1.id=tt2.stockoutdtlid
                         where   tt.projectid='{0}'  and tt.thestockinoutkind in(0,1) and  
                         to_date(to_char(tt.createdate,'YYYY-MM-DD'),'YYYY-MM-DD') between to_date('{1}','YYYY-MM-DD') and to_date('{2}','YYYY-MM-DD')
                    )t6 on t6.id=t1.id
                    where t.projectid='{0}' and trim(t.special)='安装'
                ---  and to_date(to_char(t.createdate,'YYYY-MM-DD'),'YYYY-MM-DD') between to_date('{1}','YYYY-MM-DD') and to_date('{2}','YYYY-MM-DD')
                    order by  t.createdate, t.professioncategory , t.supplierrelationname , t1.materialname,t1.diagramnumber, t1.id)
                    where  (stkInID1 is not null) or  (mvInID is not null) or  
                    (mvOutID is not null) or (stkOutID is not null ) ";
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();

            sSQL = string.Format(sSQL, sProjectID, sStartDate, sEndDate);
            command.CommandText = sSQL;//获取金额统计信息
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                oTable = dataSet.Tables[0];
            }
            return oTable;
        }

        /// <summary>
        /// 根据业务时间计算出会计年月
        /// </summary>
        /// <param name="CreateDate"></param>
        /// <returns></returns>
        public DataTable GetFiscaDate(DateTime  CreateDate)
        {
            DataTable oDataTable = null;
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sCreateDate = CreateDate.ToShortDateString();
            string sSQL = @"select nvl(t.fiscalyear,0) year,nvl(t.fiscalmonth,0) month from RESFISCALPERIODDET   t 
                join RESFISCALPERIOD t1 on t.fiscalid=t1.fiscalid and t1.busmoduleid=16 
                where t.begindate<=to_date('{0}','YYYY-MM-DD') and t.enddate>=to_date('{0}','YYYY-MM-DD')";
            sSQL = string.Format(sSQL, sCreateDate);
            command.CommandText = sSQL;//获取金额统计信息
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                oDataTable = dataSet.Tables[0];
            }
            return oDataTable;
        }

        public decimal GetRemainQty(string sDailyPlanDetialID)
        {
            decimal dQty = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sSQL = "select nvl(t.quantity-t.refquantity,0) from thd_dailyplandetail t where t.id='{0}'";
            sSQL = string.Format(sSQL, sDailyPlanDetialID);
            command.CommandText = sSQL;
            object obj = command.ExecuteScalar();
            if (obj != null)
            {
                dQty =Decimal .Parse ( obj.ToString());
            }
            return dQty;
        }

        #region 过磅单服务方法
        #region 过磅单关系增删操作
        [TransManager]
        public bool DeleteWeightBillRelation(string[] arrWeightBillRelationID)
        {
            if (arrWeightBillRelationID != null && arrWeightBillRelationID.Length > 0)
            {
                ObjectQuery oQuery = new ObjectQuery();
                oQuery.AddCriterion(Expression.In("Id", arrWeightBillRelationID));
                IList lst = Dao.ObjectQuery(typeof(WeightBillRelation), oQuery);
                if (lst != null && lst.Count > 0)
                {
                    return dao.Delete(lst);
                }
            }
            return false;
        }
        [TransManager]
        public bool SaveWeightBillRelation(IList<WeightBillRelation> lstWeightBillRelation)
        {
            if(lstWeightBillRelation!=null && lstWeightBillRelation.Count>0){
                //dao.SaveOrUpdate(lstWeightBillRelation);
                foreach (WeightBillRelation o in lstWeightBillRelation)
                {
                    dao.SaveOrUpdate(o);

                }
            }
            return true;
        }
       
        #endregion
        #region 入库单过磅操作
        [TransManager]
        public bool DeleteWeightBillRelation(List<StockInDtl> lstStockInDtl)
        {
            bool bResult = false;
            if (lstStockInDtl != null && lstStockInDtl.Count>0)
            {
                string[] arrWeightBillRelationID = lstStockInDtl.Where(a => (!string.IsNullOrEmpty(a.WeightBillRelationID))).Select(a => a.WeightBillRelationID).ToArray<string>();
                if (arrWeightBillRelationID != null && arrWeightBillRelationID.Length > 0)
                {
                   bResult=  DeleteWeightBillRelation(arrWeightBillRelationID);
                    foreach(StockInDtl oDetail in lstStockInDtl )
                    {
                        oDetail.WeightBillRelationID=null;
                    }
                }
            }
            return bResult;
        }
        [TransManager]
        public bool SaveWeightBillRelation(List<StockInDtl> lstStockInDtl, List<StockInDtl> movedDtlList)
        {
            bool bResult = false;
            List<WeightBillRelation> lstWeightBillRelation = null;
            List<StockInDtl> lstDetailTemp = null;
            WeightBillRelation oWeightBillRelation = null;
            CurrentProjectInfo oProjectInfo = null;
            DeleteWeightBillRelation(movedDtlList);
            if (lstStockInDtl != null && lstStockInDtl.Count > 0)
            {
                lstDetailTemp = lstStockInDtl.Where(a => (a.WeightBillDetail != null )).ToList();
                if (lstDetailTemp != null && lstDetailTemp.Count > 0)
                {
                    if (string.IsNullOrEmpty(lstDetailTemp[0].Master.ProjectId))
                    {
                        throw new Exception("该单据没有所属项目");
                    }
                    else
                    {
                        //删除被修改后的过磅单材料关联表
                        string[] arrWeightBillRelationID = lstDetailTemp.Where(a => !string.IsNullOrEmpty(a.WeightBillRelationID)).Select(a=>a.WeightBillRelationID).ToArray();
                        DeleteWeightBillRelation(arrWeightBillRelationID);

                        lstWeightBillRelation = new List<WeightBillRelation>();
                        oProjectInfo = GetProjectInfoById(lstDetailTemp[0].Master.ProjectId);
                        foreach (StockInDtl oDetail in lstDetailTemp)
                        {
                            oWeightBillRelation = new WeightBillRelation();
                            oWeightBillRelation.ProjectCode = oProjectInfo.Code;
                            oWeightBillRelation.ProjectId = oProjectInfo.Id;
                            oWeightBillRelation.RefBillType = EnumRefBillType.入库单;
                            oWeightBillRelation.WeightId = oDetail.WeightBillDetail.id;
                            lstWeightBillRelation.Add(oWeightBillRelation);
                        }
                      bResult=  SaveWeightBillRelation(lstWeightBillRelation);
                        foreach (StockInDtl oDetail in lstDetailTemp)
                        {
                            oWeightBillRelation = lstWeightBillRelation.First(a => a.WeightId == oDetail.WeightBillDetail.id);
                            if (oWeightBillRelation != null)
                            {
                                oDetail.WeightBillRelationID = oWeightBillRelation.Id;
                                oDetail.WeightBillDetail = null;
                            }
                        }
                    }
                }
            }
            return bResult;
        }
        #endregion
        #region 过磅单查询服务
        public IList<WeightBillDetail> QueryWeightBill(string projectCode, DateTime dStart, DateTime dEnd,string sSupplyCode,string sMaterialCode)
        {
            IList<WeightBillDetail> lstDetail = QueryAllWeightBill(projectCode, dStart, dEnd);
            IList<WeightBillDetail> lstResult = FilterWeightBill(projectCode,sSupplyCode,sMaterialCode, lstDetail);
            return lstResult;
        }
      
        public IList<WeightBillDetail> FilterWeightBill(string projectCode,string sSupplyCode,string sMaterialCode,  IList<WeightBillDetail> lstDetail)
        {
            IList<WeightBillDetail> lstResult = null;
            IList<WeightBillDetail> lstTemp = null;
            IList<long > lstWeightDetailID=null;
            
            if (lstDetail != null && lstDetail.Count > 0)
            {
                lstTemp = lstDetail;
                //if (!string.IsNullOrEmpty(sSupplyCode))
                //{
                //    lstTemp = lstTemp.Where(a => (a.Master.GYSBM == sSupplyCode)).ToList();//
                //}
                if (!string.IsNullOrEmpty(sMaterialCode))
                {
                    lstTemp = lstTemp.Where(a => (a.CLBM == sMaterialCode)).ToList();
                }
             
                if (lstTemp != null && lstTemp.Count > 0)
                {
                    string[] arrWeightDetailID = lstTemp.Select(a => a.id.ToString()).ToArray<string>();
                    //WeightBillRelation rel;rel.Id
                    if (arrWeightDetailID != null && arrWeightDetailID.Length > 0)
                    {
                        ISession session = CallContext.GetData("nhsession") as ISession;
                        IDbConnection cnn = session.Connection;
                        IDbCommand command = cnn.CreateCommand();
                        string sql = string.Format(" select t.weightid from thd_weightbillrelation t where t.projectcode='{0}' and t.weightid in ({1})", projectCode, string.Join(",", arrWeightDetailID));
                        command.CommandText = sql;
                        IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
                        DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        {
                            lstWeightDetailID = new List<long>();
                            foreach (DataRow oDataRow in dataSet.Tables[0].Rows)
                            {
                                lstWeightDetailID.Add(TransUtil.ToLong(oDataRow["weightid"]));
                            }
                            lstResult = lstTemp.Where(a => (!lstWeightDetailID.Contains(a.id))).ToList();
                        }
                        else
                        {
                            lstResult = lstTemp;
                        }
                    }
                }
            }
            return lstResult;
        }
        public static IList<WeightBillDetail> QueryAllWeightBill(string projectCode, DateTime dStart, DateTime dEnd)
        {
            IList<WeightBillDetail> lstDetail=null;
            //GetGTPWeightBill oGetGTPWeightBillSrv = new GetGTPWeightBill();
            IList<WeightBillMaster> lstMaster = GetGTPWeightBill.QueryWeightBill(EnumServerType.收料磅单, projectCode, dStart, dEnd);
            if (lstMaster != null && lstMaster.Count > 0)
            {
                lstDetail = new List<WeightBillDetail>();
                foreach (WeightBillMaster oMaster in lstMaster)
                {
                    if (oMaster.BDCL != null && oMaster.BDCL.Count>0)
                    {
                        foreach (WeightBillDetail oDetail in oMaster.BDCL)
                        {
                            oDetail.Master = oMaster;
                            lstDetail.Add(oDetail);
                        }
                    }
                    
                }
            }
            return lstDetail;
        }

        #endregion
        #endregion

    }
}
