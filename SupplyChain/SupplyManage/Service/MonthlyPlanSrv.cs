using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
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
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
   

    /// <summary>
    /// 月度需求计划服务
    /// </summary>
    public class MonthlyPlanSrv : BaseService, IMonthlyPlanSrv
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

        #region 月度需求计划
        /// <summary>
        /// 通过ID查询月度需求计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MonthlyPlanMaster GetMonthlyPlanById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            IList list = GetMonthlyPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MonthlyPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询月度需求计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MonthlyPlanMaster GetMonthlyPlanByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetMonthlyPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MonthlyPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 月度需求计划信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetMonthlyPlan(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MonthlyPlanMaster), objectQuery);
        }
        /// <summary>
        /// 查询审批信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet MonthAudit(string billId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select t1.steporder,t1.rolename,t2.pername,t2.filecabinetid,t2.perid from thd_appstepsinfo t1,resperson t2 
                            where t1.auditperson=t2.perid and t1.billappdate
                            in (select max(k1.billappdate) from thd_appstepsinfo k1 where k1.appstatus=2 and k1.billid ='" + billId + "') ";
            sql += " and t1.billid = '" + billId + "' order by t1.steporder";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
         
        /// <summary>
        /// 月度需求计划信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet MonthlyPlanQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.State,t1.PrintTimes,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,
t1.CreatePersonName,t1.CreateDate,t1.RealOperationDate,t1.sumQuantity,t1.sumMoney,t2.Price,t2.Descript,t2.UsedRankName ,t2.Quantity,t2.Money,t2.UsedPartName,t2.DiagramNumber
FROM THD_MonthlyPlanMaster  t1 INNER JOIN THD_MonthlyPlanDetail  t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }


        [TransManager]
        public MonthlyPlanMaster SaveMonthlyPlan(MonthlyPlanMaster obj)
        {
            //if (obj.Id == null)
            //{
            //    //obj.Code = GetCode(typeof(MonthlyPlanMaster));
            //    obj.Code = GetCode(typeof(MonthlyPlanMaster), obj.Special);
            //    obj.RealOperationDate = DateTime.Now;
            //}
            GetCode(obj);
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MonthlyPlanMaster;
        }

        [TransManager]
        public MonthlyPlanMaster SaveMonthlyPlan(MonthlyPlanMaster obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;
            GetCode(obj);
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MonthlyPlanMaster;
        }
        public void GetCode(MonthlyPlanMaster obj)
        {
            if ( obj!=null && obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(MonthlyPlanMaster), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(MonthlyPlanMaster), obj.ProjectId, obj.SpecialType);
                }
                obj.RealOperationDate = DateTime.Now;
            }
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
    }

}
