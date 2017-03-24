using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
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

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
   

    /// <summary>
    /// 需求总计划服务
    /// </summary>
    public class DailyPlanSrv : BaseService, IDailyPlanSrv
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
        private string GetCode(Type type, string projectId, string matCatAbb,DateTime oDateTime)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", oDateTime, projectId, matCatAbb);
        }
        #endregion

        #region 日常需求计划
        /// <summary>
        /// 通过ID查询日常需求计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DailyPlanMaster GetDailyPlanById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            IList list = GetDailyPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DailyPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询日常需求计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DailyPlanMaster GetDailyPlanByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetDailyPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DailyPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 日常需求计划信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetDailyPlan(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(DailyPlanMaster), objectQuery);
        }
        /// <summary>
        /// 日常需求计划明细信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public DailyPlanDetail GetDailyPlanDetail(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("id", id));
            objectQuery.AddFetchMode("UsedPart", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);
            IList list = Dao.ObjectQuery(typeof(DailyPlanDetail), objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DailyPlanDetail;
            }
            return null;
        }
        /// <summary>
        /// 日常需求计划信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet DailyPlanQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.State,t1.PrintTimes,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,t1.CreatePersonName,t1.CreateDate,t1.RealOperationDate,t1.OPGSYSCODE,
t1.sumQuantity,t1.sumMoney,t2.Price,t2.Descript,t2.SpecialType,t1.Compilation,t2.UsedRankName,t2.UsedPartName,t2.ApproachDate,t2.Money,t2.Quantity,t2.DiagramNumber
FROM THD_DailyPlanMaster  t1 INNER JOIN THD_DailyPlanDetail  t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        public DataSet DailyAudit(string condition)
        { 
          ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =@"select t1.steporder,t1.rolename,t2.pername from thd_appstepsinfo t1,resperson t2 
where t1.auditperson=t2.perid";
            sql +=" and t1.billid = '" + condition + "' order by t1.steporder";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;    
        }

        [TransManager]
        public DailyPlanMaster SaveDailyPlan(DailyPlanMaster obj)
        {
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(DailyPlanMaster), obj.ProjectId, obj.MaterialCategory.Abbreviation,obj.CreateDate );
                  
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(DailyPlanMaster), obj.ProjectId, obj.SpecialType,obj.CreateDate);
                }
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as DailyPlanMaster;
        }

        [TransManager]
        public DailyPlanMaster SaveDailyPlan(DailyPlanMaster obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(DailyPlanMaster), obj.ProjectId, obj.MaterialCategory.Abbreviation,obj.CreateDate);
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(DailyPlanMaster), obj.ProjectId, obj.SpecialType,obj.CreateDate);
                }
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as DailyPlanMaster;
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
        /// 查询日常需求计划某一物资数量
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sMaterialID"></param>
        /// <returns></returns>
        public decimal DailyPlanQuantity(string sProjectID, string sMaterialID, string sSpecial, string sSpecialType)
        {
            decimal dQuantity=0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select (select nvl(sum(t1.quantity),0) demQuantity from thd_demandmasterplanmaster t
join thd_demandmasterplandetail t1 on t.id=t1.parentid where t.projectid='{0}' and t1.material='{1}' and t.SPECIAL='{2}' and nvl(t.SPECIALTYPE,'-1')=nvl('{3}','-1'))  
-(select nvl(sum(t1.quantity),0) demQuantity from thd_dailyplanmaster t
join thd_dailyplandetail t1 on t.id=t1.parentid where t.projectid='{0}' and t1.material='{1}' and t.SPECIAL='{2}' and nvl(t.SPECIALTYPE,'-1')=nvl('{3}','-1'))  from dual";
            sql = string.Format(sql, sProjectID, sMaterialID, sSpecial, sSpecialType);
            command.CommandText = sql;
            object obj= command.ExecuteScalar();
            if (obj == null&& string.IsNullOrEmpty (obj.ToString ()))
            {
                dQuantity = 0;
            }
            else
            {
                dQuantity = decimal.Parse(obj.ToString());
            }

            return dQuantity;
        }


        #endregion
    }





}
