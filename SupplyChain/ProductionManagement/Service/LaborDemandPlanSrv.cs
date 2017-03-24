using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
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

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
    /// <summary>
    /// 劳务需求计划服务
    /// </summary>
    public class LaborDemandPlanSrv : BaseService, ILaborDemandPlanSrv
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

        #region 劳务需求计划
        /// <summary>
        /// 通过ID查询劳务需求计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LaborDemandPlanMaster GetLaborDemandPlanById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("Details.WorkerDetails", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            IList list = GetLaborDemandPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as LaborDemandPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询劳务需求计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public LaborDemandPlanMaster GetLaborDemandPlanByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.WorkerDetails", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            IList list = GetLaborDemandPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as LaborDemandPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 劳务需求计划查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetLaborDemandPlan(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.WorkerDetails", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(LaborDemandPlanMaster), objectQuery);
        }

        public IList GetWorkTypeByParentId(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", id));
            return Dao.ObjectQuery(typeof(LaborDemandWorkerType), oq);
        }

        /// <summary>
        /// 劳务需求计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet LaborDemandPlanQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t2.EstimateProjectQuantity,t2.EstimateProjectTimeLimit,t2.MainJobDescript,t1.Descript,t1.RealOperationDate,
                t2.QualitySafetyRequirement,t2.LaborRankInTime,t2.ProjectQuantityUnit,t2.ProjectTimeLimitUnitName,
                t1.CreatePersonName,t1.CreateDate,t2.UsedRankType,t1.PlanName,t2.ProjectTaskName,t2.PlanLaborDemandNumber
                FROM THD_LaborDemandPlanMaster t1 INNER JOIN THD_LaborDemandPlanDetail t2 ON 
                t1.Id = t2.ParentId ";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        [TransManager]
        public LaborDemandPlanMaster AddLaborDemandPlan(LaborDemandPlanMaster obj)
        {
            obj.Code = GetCode(typeof(LaborDemandPlanMaster));
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as LaborDemandPlanMaster;
        }

        [TransManager]
        public LaborDemandPlanMaster SaveLaborDemandPlan(LaborDemandPlanMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(LaborDemandPlanMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as LaborDemandPlanMaster;
        }
        #endregion
    }





}
