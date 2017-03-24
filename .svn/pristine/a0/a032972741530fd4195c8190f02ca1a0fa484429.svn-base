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
using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain;
using System.Data.OleDb;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Service
{
    /// <summary>
    /// 费用结算单
    /// </summary>
    public class ExpensesSettleSrv : BaseService, IExpensesSettleSrv
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

        #region 费用结算
        /// <summary>
        /// 通过ID查询费用结算信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExpensesSettleMaster GetExpensesSettleById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.AccountCostSubject", NHibernate.FetchMode.Eager);
            IList list = GetExpensesSettle(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ExpensesSettleMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询费用结算信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ExpensesSettleMaster GetExpensesSettleByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.AccountCostSubject", NHibernate.FetchMode.Eager);
            IList list = GetExpensesSettle(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ExpensesSettleMaster;
            }
            return null;
        }

        /// <summary>
        /// 费用结算信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetExpensesSettle(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.ProjectTask", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.AccountCostSubject", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ExpensesSettleMaster), objectQuery);
        }

        /// <summary>
        /// 费用结算信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet ExpensesSettleQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT t1.id,t1.monthlysettlment, t1.Code,t1.State,t1.RealOperationDate,t1.CreatePersonName,t1.CreateDate,t1.CreateMonth,t1.CreateYear,t2.Quantity,t2.Money,t2.Price,t1.Descript,t2.AccountCostName,t2.ProjectTaskName,t2.CostName,t2.MaterialName FROM THD_ExpensesSettleMaster  t1 INNER JOIN THD_ExpensesSettleDetail  t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }


        [TransManager]
        public ExpensesSettleMaster SaveExpensesSettle(ExpensesSettleMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ExpensesSettleMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now; 
            }
            return SaveOrUpdateByDao(obj) as ExpensesSettleMaster;
        }
        /// <summary>
        /// 根据明细Id查询费用结算明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public ExpensesSettleDetail GetExpensesSettleDetailById(string ExpensesSettleDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", ExpensesSettleDtlId));
            IList list = dao.ObjectQuery(typeof(ExpensesSettleDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ExpensesSettleDetail;
            }
            return null;
        }



        [TransManager]
        public IList GetExcel(DataSet ds)
        {
            IList list = new ArrayList();
            Hashtable hashtableSubject = new Hashtable();//成本核算科目
            CostAccountSubject Subject = null;
            ObjectQuery oqSub = new ObjectQuery();
            IList list1 = GetDomainByCondition(typeof(CostAccountSubject), oqSub);
            if (list1 != null && list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    Subject = list1[i] as CostAccountSubject;
                    hashtableSubject.Add(Subject, Subject.Code);
                }
            }

            //DataSet OleDsExcle = OpenExcel(path);
            if (ds.Tables[0].Columns.Count != 0)
            {
                int Columns = ds.Tables[0].Columns.Count;
                if (Columns < 4)
                {
                    return list;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)//循环读取临时表的行
                {
                    ExpensesSettleDetail mDetail = new ExpensesSettleDetail();
                    string strSubjectName = ds.Tables[0].Rows[i][0].ToString();//科目名称
                    string strSubjectCode = ds.Tables[0].Rows[i][1].ToString();//科目编号
                    CostAccountSubject SubjectGUID = new CostAccountSubject();
                    string strMoney = ds.Tables[0].Rows[i][2].ToString();//本期费用
                    if (strSubjectName != "" && strSubjectCode != "" && strMoney != "")//不将空白行插入list
                    {
                        mDetail.Money = Convert.ToDecimal(strMoney);
                        if (strSubjectName != "")//科目不为空
                        {
                            foreach (System.Collections.DictionaryEntry objName in hashtableSubject)
                            {
                                if (objName.Value.ToString().Equals(strSubjectCode))
                                {
                                    SubjectGUID = (CostAccountSubject)objName.Key;
                                    mDetail.AccountCostSysCode = SubjectGUID.SysCode;
                                    mDetail.AccountCostName = SubjectGUID.Name;
                                    mDetail.AccountCostSubject = SubjectGUID;
                                    break;
                                }
                            }
                        }
                        list.Add(mDetail);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 查询基础数据信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable SearchSql(string sql)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            //IDbTransaction transaction = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //command.Transaction = transaction;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dt = new DataTable();
            dt = dataSet.Tables[0];
            return dt;

        }


        #endregion

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
    }





}
