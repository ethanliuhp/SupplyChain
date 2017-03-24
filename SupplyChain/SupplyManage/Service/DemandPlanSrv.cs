using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
    /// <summary>
    /// 需求总计划服务
    /// </summary>
    public class DemandPlanSrv : BaseService, IDemandPlanSrv
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

        #region 需求总计划
        /// <summary>
        /// 通过ID查询需求总计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DemandMasterPlanMaster GetDemandMasterPlanById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            IList list = GetDemandMasterPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DemandMasterPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询需求总计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DemandMasterPlanMaster GetDemandMasterPlanByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));

            IList list = GetDemandMasterPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DemandMasterPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 需求总计划信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetDemandMasterPlan(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(DemandMasterPlanMaster), objectQuery);
        }

        /// <summary>
        /// 需求总计划信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet DemandMstPlanQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT t1.id, t1.Code,t1.State,t1.PrintTimes,t1.OPGSYSCODE,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,t1.CreatePersonName,t1.CreateDate,t1.RealOperationDate,t2.Quantity,t2.Money,t2.Price,t2.DiagramNumber,t1.Descript,t2.SpecialType,t1.Compilation,t1.MaterialCategoryName,t2.MaterialCategory FROM THD_DemandMasterPlanMaster  t1 INNER JOIN THD_DemandMasterPlanDetail  t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }

        /// <summary>
        /// 需求总计划查询(公司)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet DemandMstPlanQuery4Company(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select t1.projectname,t2.materialcode,t2.materialname,t2.materialspec,t2.matstandardunitname,
                sum(t2.quantity) quantity,sum(t2.money) money
                from thd_demandmasterplanmaster t1 inner join thd_demandmasterplandetail t2 on t1.id=t2.parentid";
            sql += " where 1=1 " + condition + " group by rollup( (t2.materialcode,t2.materialname,t2.materialspec,t2.matstandardunitname),(t1.projectname) ) order by t2.materialcode";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }


        [TransManager]
        public DemandMasterPlanMaster SaveDemandMasterPlan(DemandMasterPlanMaster obj)
        {
            //if (obj.Id == null)
            //{
            //    //obj.Code = GetCode(typeof(DemandMasterPlanMaster));
            //    obj.Code = GetCode(typeof(DemandMasterPlanMaster), obj.Special);
            //    obj.RealOperationDate = DateTime.Now;
            //}
            GetCode(obj);
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as DemandMasterPlanMaster;
        }
        [TransManager]
        public void GetCode(DemandMasterPlanMaster obj)
        {
            if (obj != null && obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(DemandMasterPlanMaster), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(DemandMasterPlanMaster), obj.ProjectId, obj.SpecialType);
                }
                obj.RealOperationDate = DateTime.Now;
            }
        }
        [TransManager]
        public DemandMasterPlanMaster SaveDemandMasterPlan(DemandMasterPlanMaster obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            GetCode(obj);
            //if (obj.Id == null)
            //{
            //    if (obj.Special == "土建")
            //    {
            //        obj.Code = GetCode(typeof(DemandMasterPlanMaster), obj.ProjectId, obj.MaterialCategory.Abbreviation);
            //    }
            //    else if (obj.Special == "安装")
            //    {
            //        obj.Code = GetCode(typeof(DemandMasterPlanMaster), obj.ProjectId, obj.SpecialType);
            //    }
            //    obj.RealOperationDate = DateTime.Now;
            //}
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as DemandMasterPlanMaster;
        }


        /// <summary>
        /// 根据明细Id查询需求总计划明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public DemandMasterPlanDetail GetDemandDetailById(string DemandDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", DemandDtlId));
            IList list = GetDemandDetailPlan(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as DemandMasterPlanDetail;
            }
            return null;
        }

        /// <summary>
        /// 查询需求总计划明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public IList GetDemandDetailPlan(ObjectQuery oq)
        {
            //IList list = dao.ObjectQuery(typeof(DemandMasterPlanDetail), oq);
            ////if (list != null && list.Count > 0)
            ////{
            ////    return list[0] as DemandMasterPlanDetail;
            ////}
            ////return null;
            return Dao.ObjectQuery(typeof(DemandMasterPlanDetail), oq);
        }


        public DataSet Stkstockindtl_RealInQuantity(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select sum(t2.quantity) quantity from thd_stkstockin t1,thd_stkstockindtl t2 where t1.id = t2.parentid";
            sql += condition;
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
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

        #endregion

        //采购成本统计
        /// <summary>
        ///  查询存在验收结算单的项目
        /// </summary>
        /// <returns></returns>
        public IList QuerySupplyProjectInfo()
        {
            IList list = new ArrayList();
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select t1.projectid,t1.projectname from thd_stockinbalmaster t1
                            where t1.special is null group by t1.projectid,t1.projectname order by t1.projectname ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CurrentProjectInfo project = new CurrentProjectInfo();
                    project.Id = TransUtil.ToString(dataRow["projectid"]);
                    project.Name = TransUtil.ToString(dataRow["projectname"]);
                    list.Add(project);
                }
            }
            return list;
        }

        /// <summary>
        ///  查询存在验收结算单[公司]的相关信息
        /// </summary>
        /// <returns></returns>
        public IList QuerySupplyCostInfo(string condition, string startDate, string endDate)
        {
            IList list = new ArrayList();
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select projectid,projectname,material,materialcode,materialname,materialspec,supplierrelationname,
                            sum(currqty) currqty,sum(currmoney) currmoney,sum(ljqty) ljqty,sum(ljmoney) ljmoney from (
                            select t1.projectid,t1.projectname,t2.material,t2.materialcode,t2.materialname,t2.materialspec,t1.supplierrelationname,
                            sum(t2.quantity) currqty,sum(t2.money) currmoney,0 ljqty,0 ljmoney 
                            from thd_stockinbalmaster t1,thd_stockinbaldetail t2 
                            where t1.id=t2.parentid and t1.special is null " + condition + @"
                            and t1.createdate >= to_date('" + startDate + @"','yyyy-mm-dd') and t1.createdate < to_date('" + endDate + @"','yyyy-mm-dd')
                            group by t1.projectid,t1.projectname,t2.material,t2.materialname,t2.materialcode,t2.materialspec,t1.supplierrelationname
                        union all
                            select t1.projectid,t1.projectname,t2.material,t2.materialcode,t2.materialname,t2.materialspec,t1.supplierrelationname,
                            0 currqty,0 currmoney,sum(t2.quantity) ljqty,sum(t2.money) ljmoney 
                            from thd_stockinbalmaster t1,thd_stockinbaldetail t2 
                            where t1.id=t2.parentid and t1.special is null " + condition + @" and t1.createdate < to_date('" + endDate + @"','yyyy-mm-dd')
                            group by t1.projectid,t1.projectname,t2.material,t2.materialcode,t2.materialname,t2.materialspec,t1.supplierrelationname
                        union all
                            select t1.projectid,t1.projectname,t2.material,t2.materialcode,t2.materialname,t2.materialspec,t1.supplierrelationname,
                            0 currqty,0 currmoney,sum(t2.quantity) ljqty,sum(t2.money) ljmoney 
                            from thd_stkstockin t1,thd_stkstockindtl t2 
                            where t1.id=t2.parentid and t1.concretebalid is not null " + condition + @" and t1.createdate < to_date('" + endDate + @"','yyyy-mm-dd')
                            group by t1.projectid,t1.projectname,t2.material,t2.materialcode,t2.materialname,t2.materialspec,t1.supplierrelationname
                        union all
                            select t1.projectid,t1.projectname,t2.material,t2.materialcode,t2.materialname,t2.materialspec,t1.supplierrelationname,
                            sum(t2.quantity) currqty,sum(t2.money) currmoney,0 ljqty,0 ljmoney 
                            from thd_stkstockin t1,thd_stkstockindtl t2 
                            where t1.id=t2.parentid and t1.concretebalid is not null " + condition + @" 
                             and t1.createdate >= to_date('" + startDate + @"','yyyy-mm-dd') and t1.createdate < to_date('" + endDate + @"','yyyy-mm-dd')
                            group by t1.projectid,t1.projectname,t2.material,t2.materialcode,t2.materialname,t2.materialspec,t1.supplierrelationname
                        ) group by projectid,projectname,material,materialcode,materialname,materialspec,supplierrelationname order by projectid,materialcode,supplierrelationname ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                string projectId = "";
                string projectName = "";
                decimal sumCurrQty = 0;
                decimal sumCurrMoney = 0;
                decimal sumLjQty = 0;
                decimal sumLjMoney = 0;
                decimal totalCurrQty = 0;
                decimal totalCurrMoney = 0;
                decimal totalLjQty = 0;
                decimal totalMoney = 0;
                int count = 0;
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    count++;
                    DataDomain domain = new DataDomain();
                    domain.Name1 = TransUtil.ToString(dataRow["projectid"]);
                    domain.Name2 = TransUtil.ToString(dataRow["projectname"]);
                    domain.Name3 = TransUtil.ToString(dataRow["material"]);
                    domain.Name4 = TransUtil.ToString(dataRow["materialname"]);
                    domain.Name5 = TransUtil.ToString(dataRow["materialspec"]);
                    domain.Name6 = TransUtil.ToString(dataRow["supplierrelationname"]);
                    domain.Name7 = TransUtil.ToString(dataRow["currqty"]);
                    domain.Name8 = TransUtil.ToString(dataRow["currmoney"]);
                    domain.Name9 = TransUtil.ToString(dataRow["ljqty"]);
                    domain.Name10 = TransUtil.ToString(dataRow["ljmoney"]);
                    if (projectId == "")
                    {
                        projectId = TransUtil.ToString(domain.Name1);
                    }
                    if (projectId != TransUtil.ToString(domain.Name1))
                    {
                        DataDomain xjDomain = new DataDomain();
                        xjDomain.Name4 = projectName + "小计: ";
                        xjDomain.Name7 = sumCurrQty;
                        xjDomain.Name8 = sumCurrMoney;
                        xjDomain.Name9 = sumLjQty;
                        xjDomain.Name10 = sumLjMoney;
                        sumCurrQty = 0;
                        sumCurrMoney = 0;
                        sumLjQty = 0;
                        sumLjMoney = 0;
                        list.Add(xjDomain);
                    }

                    sumCurrQty += TransUtil.ToDecimal(dataRow["currqty"]);
                    sumCurrMoney += TransUtil.ToDecimal(dataRow["currmoney"]);
                    sumLjQty += TransUtil.ToDecimal(dataRow["ljqty"]);
                    sumLjMoney += TransUtil.ToDecimal(dataRow["ljmoney"]);
                    totalCurrQty += TransUtil.ToDecimal(dataRow["currqty"]);
                    totalCurrMoney += TransUtil.ToDecimal(dataRow["currmoney"]);
                    totalLjQty += TransUtil.ToDecimal(dataRow["ljqty"]);
                    totalMoney += TransUtil.ToDecimal(dataRow["ljmoney"]);
                    projectId = TransUtil.ToString(dataRow["projectid"]);
                    projectName = TransUtil.ToString(dataRow["projectname"]);
                    list.Add(domain);

                    if (count == dataTable.Rows.Count)
                    {
                        DataDomain sumDomain = new DataDomain();
                        sumDomain.Name4 = domain.Name2 + "小计: ";
                        sumDomain.Name7 = sumCurrQty;
                        sumDomain.Name8 = sumCurrMoney;
                        sumDomain.Name9 = sumLjQty;
                        sumDomain.Name10 = sumLjMoney;
                        list.Add(sumDomain);

                        DataDomain totalDomain = new DataDomain();
                        totalDomain.Name4 =  "合计: ";
                        totalDomain.Name7 = totalCurrQty;
                        totalDomain.Name8 = totalCurrMoney;
                        totalDomain.Name9 = totalLjQty;
                        totalDomain.Name10 = totalMoney;
                        list.Insert(0, totalDomain);
                    }
                }
            }
            return list;
        }
    }
}
