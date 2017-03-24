using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core;
using NHibernate;
using Oracle.DataAccess.Client;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Service
{
    /// <summary>
    /// 月度成本核算服务
    /// </summary>
    public class CostMonthAccountSrv : ICostMonthAccountSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// 月度成本核算主程序
        /// </summary>
        public void CostMonthAccountCal(CostMonthAccountBill model)
        {
            Hashtable ht_monthdtl = new Hashtable();
            Hashtable ht_monthdtlconsume = new Hashtable();
            IList list_log = new ArrayList();//日志集合
            #region 1：构造本月成本核算主表

            #endregion

            #region 2：通过核算范围节点构造<核算节点集合><工程任务核算明细集合><核算明细资源耗用集合>
            //2.1 从GWBS树中查询到核算节点下的所有核算节点(包括本身)
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", model.TheProjectGUID));
            oq.AddCriterion(Expression.Like("SysCode", model.AccountTaskSysCode + "%"));
            oq.AddCriterion(Expression.Eq("IsAccountNode", true));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            IList list_GWBS = dao.ObjectQuery(typeof(GWBSTree), oq);

            //2.2 构造<核算节点集合><工程任务核算明细集合><核算明细资源耗用集合>
            foreach (GWBSTree tree in list_GWBS)
            {
                foreach (GWBSDetail detail in tree.Details)
                {
                    CostMonthAccountDtl costDtl = new CostMonthAccountDtl();
                    foreach (GWBSDetailCostSubject costSubject in detail.ListCostSubjectDetails)
                    {
                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    }
                }
            }

            #endregion

            #region 3：工程任务核算单的月度汇总
            IList list_pTaskAcctCal = new ArrayList();//已经核算的工程任务核算信息
            //3.1 查询工程任务核算信息
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateTime", model.BeginTime));
            oq.AddCriterion(Expression.Le("CreateTime", model.EndTime));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_PTaskAccount = dao.ObjectQuery(typeof(ProjectTaskAccountBill), oq);

            //3.2汇总工程任务核算信息
            Hashtable ht_sumTaskAccDtl = new Hashtable();
            Hashtable ht_sumDtlSubject = new Hashtable();
            foreach (ProjectTaskAccountBill bill in list_PTaskAccount)
            {
                //工程任务核算明细信息
                foreach (ProjectTaskDetailAccount dtl in bill.Details)
                {
                    if (ht_sumTaskAccDtl.Contains(dtl.ProjectTaskDtlGUID))
                    {
                        ProjectTaskDetailAccount pDtl = (ProjectTaskDetailAccount)ht_sumTaskAccDtl[dtl.ProjectTaskDtlGUID];
                        pDtl.AccountProjectAmount += dtl.AccountProjectAmount;
                        ht_sumTaskAccDtl.Remove(dtl.ProjectTaskDtlGUID);
                        ht_sumTaskAccDtl.Add(dtl.ProjectTaskDtlGUID, pDtl);
                    }
                    else {
                        ProjectTaskDetailAccount pDtl = new ProjectTaskDetailAccount();
                        pDtl.AccountProjectAmount = dtl.AccountProjectAmount;
                        ht_sumTaskAccDtl.Add(dtl.ProjectTaskDtlGUID, pDtl);
                    }
                    //工程任务资源耗用信息
                    foreach (ProjectTaskDetailAccountSubject subject in dtl.Details)
                    {
                        string linkStr = dtl.ProjectTaskDtlGUID + "_" + subject.ResourceTypeGUID + "_" + subject.BestaetigtCostSubjectGUID;
                        if (ht_sumDtlSubject.Contains(linkStr))
                        {
                            ProjectTaskDetailAccountSubject pSubject = (ProjectTaskDetailAccountSubject)ht_sumDtlSubject[linkStr];
                            pSubject.AccountQuantity += subject.AccountQuantity;
                            ht_sumDtlSubject.Remove(linkStr);
                            ht_sumDtlSubject.Add(linkStr, pSubject);
                        }
                        else {
                            ProjectTaskDetailAccountSubject pSubject = new ProjectTaskDetailAccountSubject();
                            pSubject.AccountQuantity = subject.AccountQuantity;
                            ht_sumDtlSubject.Add(linkStr, pSubject);
                        }
                    }
                }
            }

            //3.3工程任务明细月度核算
            foreach (string pTaskMxGUID in ht_sumTaskAccDtl.Keys)
            { 
                
            }
            foreach (string linkStr in ht_sumDtlSubject.Keys)
            { 
                
            }
            //3.4 回写工程任务核算标志
            
            #endregion

            #region 4：分包结算单的月度汇总
            //4.1 查询分包结算信息
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateDate", model.BeginTime));
            oq.AddCriterion(Expression.Le("CreateDate", model.EndTime));
            oq.AddFetchMode("ListDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ListDetails.Details", NHibernate.FetchMode.Eager);
            IList list_SubBalance = dao.ObjectQuery(typeof(SubContractBalanceBill), oq);
            Hashtable ht_sumSubBalSubject = new Hashtable();

            //4.2 汇总分包结算信息
            foreach (SubContractBalanceBill bill in list_SubBalance)
            {
                foreach (SubContractBalanceDetail dtl in bill.ListDetails)
                {
                    foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                    {
                        string linkStr = dtl.BalanceTaskDtl + "_" + subject.ResourceTypeGUID + "_" + subject.BalanceSubjectGUID;
                        if (ht_sumSubBalSubject.Contains(linkStr))
                        {

                        }
                        else { 
                            
                        }
                    }
                }
            }

            //4.3 分包结算的月度物资耗用
            foreach (string linkStr in ht_sumSubBalSubject.Keys)
            {
                bool queryFlag = false;
                //4.3.1 先查本节点和父节点的工程资源耗用明细(工程任务明细GUID+资源类型+科目)

                //4.3.2 如果未查到查询子节点的工程资源耗用明细

                //4.3.3 

                //4.3.4 
            }

            //4.4 回写分包结算单据的标志


            #endregion

            #region 5：物资耗用结算单的月度汇总

            #endregion

            #region 6：设备租赁结算单的月度汇总

            #endregion

            #region 7：料具结算单的月度汇总

            #endregion

            #region 8：费用结算单的月度汇总

            #endregion

            #region 9：料具结算单的月度汇总

            #endregion

            #region 10：计算实际值和累计值等
            //10.1 查询上期的月度成本核算


            //10.2 根据上月数据更新本月累计值

            #endregion

            #region 11：数据库处理
            //写入成本核算过程日志
            #endregion

        }

        

        #region 成本核算辅助方法
        /// <summary>
        /// 查询传入节点的父子集合
        /// 1:查询本节点和父节点的集合
        /// 2:查询子节点集合
        /// </summary>
        private IList GetNodeList (IList nodeList,string nodeGUID,int queryType)
        {
            IList list = new ArrayList();

            return list;
        }
        /// <summary>
        /// 月度成本核算反结
        /// </summary>
        private void UnCostMonthAccountCal(int kjn, int kjy)
        {
            //删除本月数据


            //清除前驱单据标志

        }

        /// <summary>
        /// 本月是否允许月度成本结算,如果上月未测算，或者下月已计算，则本月不允许计算
        /// 0: 可以结算
        /// 1: 上月没结算
        /// 2: 下月已经结算
        /// 3: 本月已经结算
        /// </summary>
        /// 
        public int IfHaveAccount(int kjn, int kjy, string projectId)
        {
            int ifHave = 0;
            int last_kjn = kjn;
            int last_kjy = kjy - 1;
            int next_kjn = kjn;
            int next_kjy = kjy + 1;
            int count = 0;

            if (kjy == 1)
            {
                last_kjn = kjn - 1;
                last_kjy = 12;
                next_kjn = kjn;
                next_kjy = 2;
            }

            if (kjy == 12)
            {
                last_kjn = kjn;
                last_kjy = 11;
                next_kjn = kjn + 1;
                next_kjy = 1;
            }

            //判断是不是新项目
            string sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId + "'";//本项目数据
            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = ds.Tables[0];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                count = TransUtil.ToInt(dataRow["count"]);
            }

            if (count == 0)
            {
                return 0;
            }

            sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId + "' and kjn= " + last_kjn + " and kjy=" + last_kjy;//上月数据
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            dataTable = ds.Tables[0];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                count = TransUtil.ToInt(dataRow["count"]);
            }

            if (count == 0)
            {
                return 1;
            }

            sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId + "'and kjn= " + next_kjn + " and kjy=" + next_kjy;//上月数据
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            dataTable = ds.Tables[0];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                count = TransUtil.ToInt(dataRow["count"]);
            }

            if (count > 0)
            {
                return 2;
            }

            sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId + "'and kjn= " + kjn + " and kjy=" + kjy;//本月数据
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            dataTable = ds.Tables[0];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                count = TransUtil.ToInt(dataRow["count"]);
            }

            if (count > 0)
            {
                return 3;
            }
            return ifHave;
        }

        /// <summary>
        /// 本月是否允许反结月度成本,如果上月未测算，或者下月已计算，则本月不允许计算
        /// 0: 可以反结
        /// 1: 下月已结算
        /// 2: 本月未结算
        /// </summary>
        /// 
        public int IfHaveUnAccount(int kjn, int kjy, string projectId)
        {
            int ifHave = 0;
            int next_kjn = kjn;
            int next_kjy = kjy + 1;
            int count = 0;

            if (kjy == 1)
            {
                next_kjn = kjn;
                next_kjy = 2;
            }

            if (kjy == 12)
            {
                next_kjn = kjn + 1;
                next_kjy = 1;
            }

            string sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId + "' and kjn= " + next_kjn + " and kjy=" + next_kjy;//下月数据
            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = ds.Tables[0];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                count = TransUtil.ToInt(dataRow["count"]);
            }

            if (count > 0)
            {
                return 1;
            }

            sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId + "' and kjn= " + kjn + " and kjy=" + kjy;//本月数据
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            dataTable = ds.Tables[0];

            foreach (DataRow dataRow in dataTable.Rows)
            {
                count = TransUtil.ToInt(dataRow["count"]);
            }

            if (count == 0)
            {
                return 2;
            }
            return ifHave;
        }

        #endregion

        #region 数据库操作
        //当反结时清除前驱单据的结算标志
        [TransManager]
        private void ClearForwardBillFlag(string costMonthAccGUID)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                OracleConnection conn = session.Connection as OracleConnection;
                //工程任务核算单
                string sql = " ";
                IDbCommand command = new OracleCommand(sql, conn);
                command.ExecuteNonQuery();

                //分包结算单
                sql = " ";
                command = new OracleCommand(sql, conn);
                command.ExecuteNonQuery();

                //物资耗用单


                //设备租赁结算单


                //料具结算单


                //费用结算单
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //回写前驱单据的结算标志
        [TransManager]
        private void WriteForwardBillFlag(IList billList,int billType)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                OracleConnection conn = session.Connection as OracleConnection;
                //工程任务核算单
                string sql = " ";
                IDbCommand command = new OracleCommand(sql, conn);
                command.ExecuteNonQuery();

                //分包结算单
                sql = " ";
                command = new OracleCommand(sql, conn);
                command.ExecuteNonQuery();

                //物资耗用单


                //设备租赁结算单


                //料具结算单


                //费用结算单
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [TransManager]
        private void DeleteCostMonthAccount(string condition)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                OracleConnection conn = session.Connection as OracleConnection;

                string sql = " delete from thd_costmonthaccount where 1=1 " + condition;

                IDbCommand command = new OracleCommand(sql, conn);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [TransManager]
        private void InsertCostMonthAccount(CostMonthAccountBill model)
        {
            try
            {
                IFCGuidGenerator gen = new IFCGuidGenerator();
                string guid = gen.GeneratorIFCGuid();

                ISession session = CallContext.GetData("nhsession") as ISession;
                OracleConnection conn = session.Connection as OracleConnection;

                string sql = " insert into thd_costmonthaccount (ID,VERSION,KJN,KJY,THEPROJECTGUID,THEPROJECTNAME,CREATETIME,ACCOUNTPERSONGUID,ACCOUNTPERSONNAME, " +
                                " ACCOUNTPERSONORGSYSCODE,THEORGNAME,ACCOUNTRANGE,ACCOUNTTASKNAME,BEGINTIME,ENDTIME,REMARK,EXCHANGERATE,ACCOUNTTASKSYSCODE) values " +
                                "('" + guid + "',0," + model.Kjn + "," + model.Kjy + ",'" + model.TheProjectGUID + "','" + model.TheProjectName + "',to_date('" + model.CreateTime.ToShortDateString() + "','yyyy-mm-dd')," +
                                " '" + model.AccountPersonGUID + "','" + model.AccountPersonName + "','" + model.AccountPersonOrgSysCode + "','" + model.TheOrgName + "'," +
                                " '" + model.AccountRange + "','" + model.AccountTaskName + "',to_date('" + model.BeginTime.ToShortDateString() + "','yyyy-mm-dd')," +
                                " to_date('" + model.EndTime.ToShortDateString() + "','yyyy-mm-dd'),'" + model.Remark + "'," + model.ExchangeRate + ",'" + model.AccountTaskSysCode + "' ) ";

                IDbCommand command = new OracleCommand(sql, conn);
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [TransManager]
        private void InsertCostMonthAccountDtlByBatch(IList list)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                OracleConnection conn = session.Connection as OracleConnection;
                foreach (CostMonthAccountBill model in list)
                {
                    string sql = " ";

                    IDbCommand command = new OracleCommand(sql, conn);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [TransManager]
        private void InsertCostMonthAccountDtlConsumeByBatch(IList list)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                OracleConnection conn = session.Connection as OracleConnection;
                foreach (CostMonthAccountBill model in list)
                {
                    string sql = " ";

                    IDbCommand command = new OracleCommand(sql, conn);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [TransManager]
        private void InsertCostMonthAccountLogByBatch(IList list)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                OracleConnection conn = session.Connection as OracleConnection;
                foreach (CostMonthAccountLog model in list)
                {
                    string sql = " ";

                    IDbCommand command = new OracleCommand(sql, conn);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
    }
}
