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
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Service
{
    /// <summary>
    /// 分包项目
    /// </summary>
    public class ContractExcuteSrv : BaseService, IContractExcuteSrv
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

        private string GetCode(Type type,string specail)
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

        #region 分包项目
        /// <summary>
        /// 通过ID查询分包项目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SubContractProject GetContractExcuteById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetContractExcute(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SubContractProject;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询分包项目信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public SubContractProject GetContractExcuteByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetContractExcute(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SubContractProject;
            }
            return null;
        }

        /// <summary>
        /// 分包项目信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetContractExcute(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("ProfessDetails", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("LaborDetails", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheContractGroup", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(SubContractProject), objectQuery);
        }

        /// <summary>
        /// 分包项目信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet ContractExcuteQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT t1.id,t1.Code,t1.BearerOrgName,t2.ChangeDesc,t1.contracttype mainContractType,t1.CreateDate,t1.OWNERNAME,t2.ChangeMoney,t2.ContractCode," +
                    " t2.ContractName,t1.ManagementRate,t1.ManagementRemMethod,t2.ContractType,t1.UtilitiesRate,t1.UtilitiesRemMethod" +
                      " FROM THD_SubContractProject  t1 INNER JOIN Thd_Subcontractchangeitem  t2 ON t1.ID = t2.TheProject";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }


        [TransManager]
        public SubContractProject SaveContractExcute(SubContractProject obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(SubContractProject));
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as SubContractProject;
        }


        /// <summary>
        /// 根据明细Id查询分包项目明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public IList GetContractDetailById(string ContractDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProject", ContractDtlId));
            IList list = dao.ObjectQuery(typeof(SubContractChangeItem), oq);
            if (list != null && list.Count > 0)
            {
                return list;
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

        #endregion
    }





}
