using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ExcelImportMng.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service
{


    /// <summary>
    /// 专项管理费用
    /// </summary>
    public class SpecialCostSrv : BaseService, ISpecialCostSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }
        private IExcelImportSrv excelImportSrv;
        public IExcelImportSrv ExcelImportSrv
        {
            get { return excelImportSrv; }
            set { excelImportSrv = value; }
        }
        private IStockInSrv _theStockInSrv;
        public IStockInSrv TheStockInSrv
        {
            get { return _theStockInSrv; }
            set { _theStockInSrv = value; }
        }

        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }
        #endregion

        #region 专项管理费用
        /// <summary>
        /// 通过ID查询专项管理费用信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SpecialCostMaster GetSpecialCostById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("EngTaskId", FetchMode.Eager);
            IList list = GetSpecialCost(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SpecialCostMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询专项管理费用信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public SpecialCostMaster GetSpecialCostByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetSpecialCost(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SpecialCostMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询专项管理费用信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetSpecialCost(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.EngTaskId", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(SpecialCostMaster), objectQuery);
        }


        [TransManager]
        public SpecialCostMaster SaveSpecialCost(SpecialCostMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(SpecialCostMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as SpecialCostMaster;
        }

        #endregion

        #region 专项费用结算

        /// <summary>
        /// 通过ID查询专项管理费用信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SpeCostSettlementMaster GetSpecialAccountById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetSpecialAccount(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SpeCostSettlementMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询专项管理费用信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public SpeCostSettlementMaster GetSpecialAccountByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetSpecialAccount(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SpeCostSettlementMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询专项管理费用信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetSpecialAccount(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.EngTaskId", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("SubcontractProjectId", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.SpeCostMngId", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.SpeCostMngId.Details", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(SpeCostSettlementMaster), objectQuery);
        }

        /// <summary>
        /// 查询专项管理费用明细信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetSpecialDetailAccount(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("EngTaskId", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.SubcontractProjectId", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("SpeCostMngId", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("SpeCostMngId.Details", NHibernate.FetchMode.Eager);
            //objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(SpeCostSettlementDetail), objectQuery);
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

        [TransManager]
        public SpeCostSettlementMaster SaveSpecialAccount(SpeCostSettlementMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(SpeCostSettlementMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            //else
            //{
            ////提交的时候才更新专项管理费用信息
            //if (obj.DocState == DocumentState.InExecute)
            //{
            //    //保存专项管理费用明细
            //    foreach (SpeCostSettlementDetail dtl in obj.Details)
            //    {
            //        SpecialCostDetail detail = new SpecialCostDetail();
            //        ObjectQuery oq = new ObjectQuery();
            //        oq.AddCriterion(Expression.Eq("AccountingYear", obj.CreateDate.Year));
            //        oq.AddCriterion(Expression.Eq("AccountingMonth", obj.CreateDate.Month));
            //        oq.AddCriterion(Expression.Eq("AccountingStyle", "0"));
            //        oq.AddFetchMode("EngTaskId", NHibernate.FetchMode.Eager);
            //        IList list = Dao.ObjectQuery(typeof(SpecialCostDetail), oq);
            //        if (list.Count > 0)
            //        {
            //            foreach (SpecialCostDetail costDetail in list)
            //            {
            //                detail = costDetail;
            //                detail.CurrentRealPay += dtl.SettlementMoney;//当期实际支出
            //                detail.CurrentRealProgress = detail.CurrentRealPay / dtl.SpeCostMngId.ContractTotalIPay;//当期实际核算形象进度
            //                detail.CurrentRealIncome += dtl.SpeCostMngId.ContractTotalIncome * detail.CurrentRealProgress;//当期实际收入
            //                //dtl.SpeCostMngId.Details.Remove(costDetail);
            //                //detail.Id = null;
            //                //dtl.SpeCostMngId.AddDetail(detail);
            //                SpecialCostDetail saveDetail = SaveOrUpdateByDao(detail) as SpecialCostDetail;
            //                ISession session = CallContext.GetData("nhsession") as ISession;
            //                session.Refresh(dtl);
            //            }
            //        }
            //        else
            //        {
            //            detail.AccountingYear = obj.CreateDate.Year;
            //            detail.AccountingMonth = obj.CreateDate.Month;
            //            detail.AccountingStyle = "0";//0月度，1季度

            //            DateTime strStart = DateTime.Now;
            //            DateTime strEnd = DateTime.Now;
            //            string strCostType = "select BEGINDATE,ENDDATE from resfiscalperioddet where FISCALYEAR = '" + obj.CreateDate.Year + "' and FISCALMONTH = '" + obj.CreateDate.Month + "'";
            //            DataTable dtCostType = SearchSql(strCostType);
            //            for (int i = 0; i < dtCostType.Rows.Count; i++)
            //            {
            //                strStart = Convert.ToDateTime(dtCostType.Rows[i][0].ToString());
            //                strEnd = Convert.ToDateTime(dtCostType.Rows[i][1].ToString());
            //            }

            //            detail.AccountingStartDate = strStart;//会计期间开始时间
            //            detail.AccountingEndDate = strEnd;//会计期间结束时间                

            //            detail.CurrentRealPay += dtl.SettlementMoney;//当期实际支出
            //            detail.CurrentRealProgress = detail.CurrentRealPay / dtl.SpeCostMngId.ContractTotalIPay;//当期实际核算形象进度
            //            detail.CurrentRealIncome += dtl.SpeCostMngId.ContractTotalIncome * detail.CurrentRealProgress;//当期实际收入
            //            dtl.SpeCostMngId.AddDetail(detail);
            //        }


            //        ObjectQuery oqy = new ObjectQuery();
            //        oq.AddCriterion(Expression.Eq("AccountingYear", obj.CreateDate.Year));
            //        int month = obj.CreateDate.Month;
            //        if (month == 1 || month == 2 || month == 3)
            //        {
            //            oq.AddCriterion(Expression.Eq("AccountingMonth", 1));
            //        }
            //        if (month == 4 || month == 5 || month == 6)
            //        {
            //            oq.AddCriterion(Expression.Eq("AccountingMonth", 4));
            //        }
            //        if (month == 7 || month == 8 || month == 9)
            //        {
            //            oq.AddCriterion(Expression.Eq("AccountingMonth", 7));
            //        }
            //        if (month == 10 || month == 11 || month == 12)
            //        {
            //            oq.AddCriterion(Expression.Eq("AccountingMonth", 10));
            //        }
            //        oqy.AddCriterion(Expression.Eq("AccountingStyle", "1"));
            //        oqy.AddFetchMode("EngTaskId", NHibernate.FetchMode.Eager);
            //        IList lists = Dao.ObjectQuery(typeof(SpecialCostDetail), oqy);
            //        if (lists.Count > 0)
            //        {
            //            foreach (SpecialCostDetail costDetail in lists)
            //            {
            //                //更新专项费用管理单信息
            //                //dtl.SpeCostMngId.RealPay += dtl.SettlementMoney;//累计实际支出
            //                //dtl.SpeCostMngId.AccountingProgress += dtl.SpeCostMngId.RealPay / dtl.SpeCostMngId.ContractTotalIPay * 100;//累计核算形象进度
            //                //dtl.SpeCostMngId.RealIncome += dtl.SpeCostMngId.ContractTotalIncome * dtl.SpeCostMngId.AccountingProgress / 100;//累计实际收入
            //                SpecialCostDetail detail1 = new SpecialCostDetail();
            //                detail1 = costDetail;
            //                detail1.AccountingYear = obj.CreateDate.Year;
            //                int months = obj.CreateDate.Month;
            //                if (months == 1 || months == 2 || months == 3)
            //                {
            //                    detail1.AccountingMonth = 1;
            //                }
            //                if (months == 4 || months == 5 || months == 6)
            //                {
            //                    detail1.AccountingMonth = 4;
            //                }
            //                if (months == 7 || months == 8 || months == 9)
            //                {
            //                    detail1.AccountingMonth = 7;
            //                }
            //                if (months == 10 || months == 11 || months == 12)
            //                {
            //                    detail1.AccountingMonth = 10;
            //                }
            //                //detail1.AccountingMonth = obj.CreateDate.Month;
            //                detail1.AccountingStyle = "1";//0月度，1季度

            //                //detail1.AccountingStartDate = strStart;//会计期间开始时间
            //                //detail1.AccountingEndDate = strEnd;//会计期间结束时间

            //                detail1.CurrentRealPay += dtl.SettlementMoney;//当期实际支出
            //                detail1.CurrentRealProgress = detail.CurrentRealPay / dtl.SpeCostMngId.ContractTotalIPay;//当期实际核算形象进度
            //                detail1.CurrentRealIncome += dtl.SpeCostMngId.ContractTotalIncome * detail.CurrentRealProgress;//当期实际收入
            //                //dtl.SpeCostMngId.Details.Remove(costDetail);
            //                //detail1.Id = null;
            //                //dtl.SpeCostMngId.AddDetail(detail1);
            //                SpecialCostDetail saveDetail = SaveOrUpdateByDao(detail1) as SpecialCostDetail;
            //                ISession session = CallContext.GetData("nhsession") as ISession;
            //                session.Refresh(dtl);
            //            }
            //        }
            //        else
            //        {
            //            //更新专项费用管理单信息
            //            //dtl.SpeCostMngId.RealPay += dtl.SettlementMoney;//累计实际支出
            //            //dtl.SpeCostMngId.AccountingProgress = dtl.SpeCostMngId.RealPay / dtl.SpeCostMngId.ContractTotalIPay * 100;//累计核算形象进度
            //            //dtl.SpeCostMngId.RealIncome += dtl.SpeCostMngId.ContractTotalIncome * dtl.SpeCostMngId.AccountingProgress / 100;//累计实际收入
            //            SpecialCostDetail detail1 = new SpecialCostDetail();
            //            detail1.AccountingYear = obj.CreateDate.Year;
            //            int months = obj.CreateDate.Month;
            //            if (months == 1 || months == 2 || months == 3)
            //            {
            //                detail1.AccountingMonth = 1;
            //            }
            //            if (months == 4 || months == 5 || months == 6)
            //            {
            //                detail1.AccountingMonth = 4;
            //            }
            //            if (months == 7 || months == 8 || months == 9)
            //            {
            //                detail1.AccountingMonth = 7;
            //            }
            //            if (months == 10 || months == 11 || months == 12)
            //            {
            //                detail1.AccountingMonth = 10;
            //            }
            //            detail1.AccountingStyle = "1";//0月度，1季度
            //            detail1.CurrentRealPay += dtl.SettlementMoney;//当期实际支出
            //            detail1.CurrentRealProgress = detail.CurrentRealPay / dtl.SpeCostMngId.ContractTotalIPay;//当期实际核算形象进度
            //            detail1.CurrentRealIncome += dtl.SpeCostMngId.ContractTotalIncome * detail.CurrentRealProgress;//当期实际收入
            //            dtl.SpeCostMngId.AddDetail(detail1);
            //        }


            //        //更新专项费用管理单信息
            //        dtl.SpeCostMngId.RealPay += dtl.SettlementMoney;//累计实际支出
            //        dtl.SpeCostMngId.AccountingProgress = dtl.SpeCostMngId.RealPay / dtl.SpeCostMngId.ContractTotalIPay * 100;//累计核算形象进度
            //        dtl.SpeCostMngId.RealIncome += dtl.SpeCostMngId.ContractTotalIncome * dtl.SpeCostMngId.AccountingProgress / 100;//累计实际收入
            //        //ISession session = CallContext.GetData("nhsession") as ISession;
            //        //session.Refresh(dtl);
            //        dtl.SpeCostMngId = SaveOrUpdateByDao(dtl.SpeCostMngId) as SpecialCostMaster;
            //    }
            //}
            //}
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as SpeCostSettlementMaster;
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
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dt = new DataTable();
            dt = dataSet.Tables[0];
            return dt;

        }
        #endregion

        #region 产值管理
        //自行产值查询
        public DataSet OutPutValueQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.ProjectName,t1.CreateYear ,t1.CreateMonth,t2.PlanValue,t2.RealValue,t2.GWBSTreeName,t2.GWBSTreeSysCode,t1.AccountYear, t1.AccountMonth
                From thd_produceselfvaluemaster t1 inner join thd_produceselfvaluedetail t2 on t1.id=t2.parentid ";
            sql += " where 1=1 " + condition;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 计划产值计算服务
        /// </summary>
        /// <param name="schedulePlan">周/月进度计划</param>
        /// <param name="optProject">操作的项目</param>
        [TransManager]
        public ProduceSelfValueMaster PlanedOutputValueAccountbak(WeekScheduleMaster schedulePlan, CurrentProjectInfo optProject, OperationOrgInfo org)
        {
            try
            {
                ProduceSelfValueMaster optProduceValueMaster = null;//操作{自行产值}

                //1确定操作自行产值
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", optProject.Id));
                oq.AddCriterion(Expression.Eq("SchedualGUID", schedulePlan.Id));
                oq.AddFetchMode("Details", FetchMode.Eager);

                IList listProduceValueMaster = dao.ObjectQuery(typeof(ProduceSelfValueMaster), oq);

                if (listProduceValueMaster != null && listProduceValueMaster.Count > 0)
                    optProduceValueMaster = listProduceValueMaster[0] as ProduceSelfValueMaster;
                else
                {
                    optProduceValueMaster = new ProduceSelfValueMaster();
                    optProduceValueMaster.ProjectId = optProject.Id;
                    optProduceValueMaster.ProjectName = optProject.Name;

                    optProduceValueMaster.OperOrgInfo = org;
                    optProduceValueMaster.OperOrgInfoName = org.Name;
                    optProduceValueMaster.OperOrgSysCode = org.SysCode;

                    optProduceValueMaster.CreateYear = DateTime.Now.Year;
                    optProduceValueMaster.CreateMonth = DateTime.Now.Month;

                    if (schedulePlan.ExecScheduleType == EnumExecScheduleType.月度进度计划)
                        optProduceValueMaster.AccountType = ProduceSelfValueMasterAccountType.月度核算;
                    else if (schedulePlan.ExecScheduleType == EnumExecScheduleType.季度进度计划)
                        optProduceValueMaster.AccountType = ProduceSelfValueMasterAccountType.季度核算;

                    //会计年月
                    optProduceValueMaster.AccountYear = schedulePlan.AccountYear;
                    optProduceValueMaster.AccountMonth = schedulePlan.AccountMonth;

                    if (optProduceValueMaster.AccountYear > 0 && optProduceValueMaster.AccountMonth > 0)
                    {
                        optProduceValueMaster.BeginDate = DateTime.Parse(optProduceValueMaster.AccountYear + "-" + optProduceValueMaster.AccountMonth + "-1");
                        optProduceValueMaster.EndDate = DateTime.Parse(optProduceValueMaster.AccountYear + "-" + optProduceValueMaster.AccountMonth + "-1").AddMonths(1).AddDays(-1);
                    }
                }

                oq.Criterions.Clear();
                oq.FetchModes.Clear();

                oq.AddCriterion(Expression.Eq("Master.Id", schedulePlan.Id));
                oq.AddFetchMode("GWBSTree", FetchMode.Eager);

                IList listSchedulePlanDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq);

                oq.Criterions.Clear();
                oq.FetchModes.Clear();


                oq.AddCriterion(Expression.Eq("TheProjectGUID", schedulePlan.ProjectId));
                oq.AddCriterion(Expression.Eq("CostAccFlag", true));
                Disjunction dis = new Disjunction();
                foreach (WeekScheduleDetail dtl in listSchedulePlanDtl)
                {
                    string[] sysCodes = dtl.GWBSTree.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < sysCodes.Length - 1; i++)
                    {
                        string sysCode = "";
                        for (int j = 0; j <= i; j++)
                        {
                            sysCode += sysCodes[j] + ".";
                        }
                        dis.Add(Expression.Eq("SysCode", sysCode));
                    }
                }
                oq.AddCriterion(dis);
                oq.AddOrder(NHibernate.Criterion.Order.Desc("SysCode"));

                IList lista = dao.ObjectQuery(typeof(GWBSTree), oq);

                IEnumerable<GWBSTree> listAllGWBS = dao.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>();

                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                dis = new Disjunction();


                List<string> listGWBSIds = new List<string>();
                foreach (GWBSTree g in listAllGWBS)
                {
                    listGWBSIds.Add(g.Id);
                }


                IEnumerable<ProduceSelfValueDetail> listAllProduceValueDtl = from d in optProduceValueMaster.Details.OfType<ProduceSelfValueDetail>()
                                                                             where listGWBSIds.Contains(d.GWBSTree.Id)
                                                                             select d;


                //判断进度计划明细中工程任务是否存在隶属关系
                IList listScheduleDtl = new ArrayList();
                foreach (WeekScheduleDetail sDtl in listSchedulePlanDtl)
                {
                    if (listScheduleDtl == null || listScheduleDtl.Count == 0)
                    {
                        listScheduleDtl.Add(sDtl);
                    }
                    else
                    {
                        bool flag = false;
                        WeekScheduleDetail d = new WeekScheduleDetail();
                        foreach (WeekScheduleDetail w in listScheduleDtl)
                        {
                            if (sDtl.GWBSTreeSysCode.Contains(w.GWBSTreeSysCode))
                            {
                                d = w;
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            listScheduleDtl.Remove(d);
                            listScheduleDtl.Add(sDtl);
                        }

                    }
                }


                //循环1：针对<操作{周（月）进度计 划明细}集>中个个对象处理
                foreach (WeekScheduleDetail dtl in listScheduleDtl)
                {
                    bool flag = false;
                    IList listGWBSTree = new ArrayList();
                    foreach (GWBSTree gt in listAllGWBS)
                    {
                        if (dtl.GWBSTree.SysCode.Contains(gt.SysCode))
                        {
                            listGWBSTree.Add(gt);
                            flag = true;
                            break;
                        }
                    }

                    //循环2：针对<产值核算节点 集>中每个对象处理
                    if (flag)
                    {
                        foreach (GWBSTree AccountWbs in listGWBSTree)
                        {
                            var queryProduceValueDtl = from d in listAllProduceValueDtl
                                                       where d.GWBSTree.Id == AccountWbs.Id
                                                       select d;

                            ObjectQuery oq1 = new ObjectQuery();
                            oq1.AddCriterion(Expression.Eq("TheGWBS.Id", AccountWbs.Id));
                            IList listGWBSDtl = dao.ObjectQuery(typeof(GWBSDetail), oq1);


                            ProduceSelfValueDetail optDtl = null;//<操作{自行产值明细}
                            if (queryProduceValueDtl.Count() == 0)
                            {
                                optDtl = new ProduceSelfValueDetail();
                                optDtl.Master = optProduceValueMaster;
                                optProduceValueMaster.Details.Add(optDtl);

                                optDtl.GWBSTree = AccountWbs;
                                optDtl.GWBSTreeName = AccountWbs.Name;
                                optDtl.GWBSTreeSysCode = AccountWbs.SysCode;

                                optDtl.PlanValue = 0;
                                optDtl.RealValue = 0;

                                int planDays = 0;//本期计划工期
                                if (dtl.NodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)//如果是叶节点
                                {
                                    planDays = (dtl.PlannedEndDate - dtl.PlannedBeginDate).Days + 1;
                                }
                                else
                                {
                                    //取下属子节点最晚计划结束实际和最早计划起始时间的工期
                                    oq.Criterions.Clear();
                                    oq.FetchModes.Clear();
                                    oq.Orders.Clear();


                                    oq.AddCriterion(Expression.Eq("Master.Id", dtl.Master.Id));
                                    oq.AddCriterion(Expression.Like("GWBSTreeSysCode", dtl.GWBSTreeSysCode, MatchMode.Start));
                                    IEnumerable<WeekScheduleDetail> listWeekDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq).OfType<WeekScheduleDetail>();

                                    DateTime minDateTime = DateTime.MaxValue;
                                    DateTime maxDateTime = DateTime.MinValue;
                                    for (int i = 0; i < listWeekDtl.Count(); i++)
                                    {
                                        DateTime mintime = listWeekDtl.ElementAt(i).PlannedBeginDate;
                                        DateTime maxtime = listWeekDtl.ElementAt(i).PlannedEndDate;
                                        if (mintime == ClientUtil.ToDateTime("1900-1-1") && maxtime == ClientUtil.ToDateTime("1900-1-1"))
                                            continue;
                                        if (mintime < minDateTime)
                                        {
                                            minDateTime = mintime;
                                        }
                                        if (maxtime > maxDateTime)
                                        {
                                            maxDateTime = maxtime;
                                        }
                                    }
                                    planDays = (maxDateTime - minDateTime).Days + 1;
                                }


                                int planCountDays = 0;//总工期

                                oq.Criterions.Clear();
                                oq.FetchModes.Clear();
                                oq.Orders.Clear();

                                oq.AddCriterion(Expression.Eq("Master.Id", schedulePlan.ForwardBillId));
                                oq.AddCriterion(Expression.Eq("GWBSTree.Id", AccountWbs.Id));


                                IList listPlanDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);

                                if (listPlanDtl == null || listPlanDtl.Count == 0)
                                    continue;

                                ProductionScheduleDetail schedulePlanDtl = listPlanDtl[0] as ProductionScheduleDetail;


                                if (schedulePlanDtl.GWBSNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)//如果是叶节点
                                    planCountDays = (schedulePlanDtl.PlannedEndDate - schedulePlanDtl.PlannedBeginDate).Days + 1;
                                else
                                {
                                    //取下属子节点最晚计划结束实际和最早计划起始时间的工期
                                    oq.Criterions.Clear();
                                    oq.FetchModes.Clear();
                                    oq.Orders.Clear();

                                    oq.AddCriterion(Expression.Eq("Master.Id", schedulePlanDtl.Master.Id));
                                    //oq.AddCriterion(Expression.Like("GWBSTreeSysCode", schedulePlanDtl.GWBSTreeSysCode, MatchMode.Start));
                                    IEnumerable<ProductionScheduleDetail> listChildPlanDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();


                                    if (listChildPlanDtl.Count() == 0)
                                        continue;

                                    DateTime minDateTime = DateTime.MaxValue;
                                    DateTime maxDateTime = DateTime.MinValue;
                                    for (int i = 0; i < listChildPlanDtl.Count(); i++)
                                    {
                                        DateTime mintime = listChildPlanDtl.ElementAt(i).PlannedBeginDate;
                                        DateTime maxtime = listChildPlanDtl.ElementAt(i).PlannedEndDate;
                                        if (mintime == ClientUtil.ToDateTime("1900-1-1") && maxtime == ClientUtil.ToDateTime("1900-1-1"))
                                            continue;
                                        if (mintime < minDateTime)
                                        {
                                            minDateTime = mintime;
                                        }
                                        if (maxtime > maxDateTime)
                                        {
                                            maxDateTime = maxtime;
                                        }
                                    }
                                    planCountDays = (maxDateTime - minDateTime).Days + 1;
                                }

                                optDtl.PlanProgress = (decimal)planDays / (decimal)planCountDays;
                                optDtl.RealProgress = 0;

                            }
                            else
                            {
                                optDtl = queryProduceValueDtl.ElementAt(0);

                                int planDays = 0;//本期计划工期
                                if (dtl.NodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)//如果是叶节点
                                {
                                    planDays = (dtl.PlannedEndDate - dtl.PlannedBeginDate).Days + 1;
                                }
                                else
                                {
                                    //取下属子节点最晚计划结束实际和最早计划起始时间的工期
                                    oq.Criterions.Clear();
                                    oq.FetchModes.Clear();
                                    oq.Orders.Clear();

                                    oq.AddCriterion(Expression.Eq("Master.Id", dtl.Master.Id));
                                    oq.AddCriterion(Expression.Like("GWBSTreeSysCode", dtl.GWBSTreeSysCode, MatchMode.Start));
                                    IEnumerable<WeekScheduleDetail> listWeekDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq).OfType<WeekScheduleDetail>();

                                    DateTime minDateTime = DateTime.MaxValue;
                                    DateTime maxDateTime = DateTime.MinValue;
                                    for (int i = 0; i < listWeekDtl.Count(); i++)
                                    {
                                        DateTime mintime = listWeekDtl.ElementAt(i).PlannedBeginDate;
                                        DateTime maxtime = listWeekDtl.ElementAt(i).PlannedEndDate;
                                        if (mintime == ClientUtil.ToDateTime("1900-1-1") && maxtime == ClientUtil.ToDateTime("1900-1-1"))
                                            continue;
                                        if (mintime < minDateTime)
                                        {
                                            minDateTime = mintime;
                                        }
                                        if (maxtime > maxDateTime)
                                        {
                                            maxDateTime = maxtime;
                                        }
                                    }
                                    planDays = (maxDateTime - minDateTime).Days + 1;
                                }


                                int planCountDays = 0;//总工期

                                oq.Criterions.Clear();
                                oq.FetchModes.Clear();
                                oq.Orders.Clear();

                                oq.AddCriterion(Expression.Eq("Master.Id", schedulePlan.ForwardBillId));
                                oq.AddCriterion(Expression.Eq("GWBSTree.Id", AccountWbs.Id));


                                IList listPlanDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);

                                if (listPlanDtl == null || listPlanDtl.Count == 0)
                                    continue;

                                ProductionScheduleDetail schedulePlanDtl = listPlanDtl[0] as ProductionScheduleDetail;


                                if (schedulePlanDtl.GWBSNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)//如果是叶节点
                                    planCountDays = (schedulePlanDtl.PlannedEndDate - schedulePlanDtl.PlannedBeginDate).Days + 1;
                                else
                                {
                                    //取下属子节点最晚计划结束实际和最早计划起始时间的工期
                                    oq.Criterions.Clear();
                                    oq.FetchModes.Clear();
                                    oq.Orders.Clear();

                                    oq.AddCriterion(Expression.Eq("Master.Id", schedulePlanDtl.Master.Id));
                                    //oq.AddCriterion(Expression.Like("GWBSTreeSysCode", schedulePlanDtl.GWBSTreeSysCode, MatchMode.Start));
                                    IEnumerable<ProductionScheduleDetail> listChildPlanDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

                                    if (listChildPlanDtl.Count() == 0)
                                        continue;
                                    DateTime minDateTime = DateTime.MaxValue;
                                    DateTime maxDateTime = DateTime.MinValue;
                                    for (int i = 0; i < listChildPlanDtl.Count(); i++)
                                    {
                                        DateTime mintime = listChildPlanDtl.ElementAt(i).PlannedBeginDate;
                                        DateTime maxtime = listChildPlanDtl.ElementAt(i).PlannedEndDate;
                                        if (mintime == ClientUtil.ToDateTime("1900-1-1") && maxtime == ClientUtil.ToDateTime("1900-1-1"))
                                            continue;
                                        if (mintime < minDateTime)
                                        {
                                            minDateTime = mintime;
                                        }
                                        if (maxtime > maxDateTime)
                                        {
                                            maxDateTime = maxtime;
                                        }
                                    }
                                    planCountDays = (maxDateTime - minDateTime).Days + 1;
                                }

                                optDtl.PlanProgress = (decimal)planDays / (decimal)planCountDays;
                                optDtl.RealProgress = 0;
                            }

                            oq.Criterions.Clear();
                            oq.FetchModes.Clear();
                            oq.Orders.Clear();

                            IList listGWBSDetails = new ArrayList();

                            if (dtl.NodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)
                            {
                                oq.AddCriterion(Expression.Like("TheGWBSSysCode", dtl.GWBSTreeSysCode, MatchMode.Start));
                                oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                                IEnumerable<GWBSDetail> listWBSDtl = dao.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>();
                                foreach (GWBSDetail wbsDtl in listWBSDtl)
                                {
                                    listGWBSDetails.Add(wbsDtl);
                                }
                            }
                            else if (dtl.NodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.MiddleNode)
                            {
                                oq.AddCriterion(Expression.Like("SysCode", dtl.GWBSTree.SysCode, MatchMode.Start));
                                oq.AddCriterion(Expression.Eq("CostAccFlag", false));
                                IEnumerable<GWBSTree> listWBS = dao.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>();
                                foreach (GWBSTree gwbs in listWBS)
                                {
                                    ObjectQuery oq2 = new ObjectQuery();
                                    oq2.AddCriterion(Expression.Eq("TheGWBS.Id", gwbs.Id));
                                    oq2.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                                    IEnumerable<GWBSDetail> listWBSDtl = dao.ObjectQuery(typeof(GWBSDetail), oq2).OfType<GWBSDetail>();
                                    foreach (GWBSDetail wbsDtl in listWBSDtl)
                                    {
                                        listGWBSDetails.Add(wbsDtl);
                                    }
                                }
                            }

                            if (listGWBSDetails.Count > 0)
                            {
                                for (int i = 0; i < listGWBSDetails.Count; i++)
                                {
                                    GWBSDetail wbsdtl = listGWBSDetails[i] as GWBSDetail;

                                    for (int j = 0; j < listGWBSDtl.Count; j++)
                                    {
                                        GWBSDetail gwbsDtl = listGWBSDtl[j] as GWBSDetail;

                                        if (wbsdtl.TheCostItem.Id == gwbsDtl.TheCostItem.Id && wbsdtl.MainResourceTypeId == gwbsDtl.MainResourceTypeId && wbsdtl.DiagramNumber == gwbsDtl.DiagramNumber)
                                        {
                                            optDtl.PlanValue += optDtl.PlanProgress * wbsdtl.ContractProjectQuantity * gwbsDtl.ContractPrice;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                optProduceValueMaster.PlanDate = DateTime.Now;
                optProduceValueMaster.State = ProduceSelfValueMasterState.计划产值已生成;
                dao.SaveOrUpdate(optProduceValueMaster);
                return optProduceValueMaster;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TransManager]
        public ProduceSelfValueMaster PlanedOutputValueAccount(WeekScheduleMaster schedulePlan, CurrentProjectInfo optProject, OperationOrgInfo org)
        {
            try
            {
                ProduceSelfValueMaster optProduceValueMaster = null;//操作{自行产值}

                //1确定操作自行产值
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", optProject.Id));
                oq.AddCriterion(Expression.Eq("SchedualGUID", schedulePlan.Id));
                oq.AddFetchMode("Details", FetchMode.Eager);

                IList listProduceValueMaster = dao.ObjectQuery(typeof(ProduceSelfValueMaster), oq);

                if (listProduceValueMaster != null && listProduceValueMaster.Count > 0)
                    optProduceValueMaster = listProduceValueMaster[0] as ProduceSelfValueMaster;
                else
                {
                    optProduceValueMaster = new ProduceSelfValueMaster();
                    optProduceValueMaster.ProjectId = optProject.Id;
                    optProduceValueMaster.ProjectName = optProject.Name;

                    optProduceValueMaster.SchedualGUID = schedulePlan.Id;

                    optProduceValueMaster.OperOrgInfo = org;
                    optProduceValueMaster.OperOrgInfoName = org.Name;
                    optProduceValueMaster.OperOrgSysCode = org.SysCode;

                    optProduceValueMaster.CreateYear = DateTime.Now.Year;
                    optProduceValueMaster.CreateMonth = DateTime.Now.Month;

                    //会计年月
                    optProduceValueMaster.AccountYear = schedulePlan.AccountYear;
                    optProduceValueMaster.AccountMonth = schedulePlan.AccountMonth;

                    //缺省核算时间
                    int startDate = 21;
                    int endDate = 20;
                    string basicName = "月进度计划默认时间段";

                    IList listDateArea = TheStockInSrv.GetBasicDataByParentName(basicName);

                    if (listDateArea != null && listDateArea.Count > 0)
                    {
                        BasicDataOptr basicData = listDateArea[0] as BasicDataOptr;
                        if (basicData != null)
                        {
                            try
                            {
                                startDate = Convert.ToInt32(basicData.BasicCode);
                                endDate = Convert.ToInt32(basicData.BasicName);
                            }
                            catch { }
                        }
                    }

                    if (schedulePlan.ExecScheduleType == EnumExecScheduleType.月度进度计划)
                    {
                        optProduceValueMaster.AccountType = ProduceSelfValueMasterAccountType.月度核算;

                        if (optProduceValueMaster.AccountYear > 0 && optProduceValueMaster.AccountMonth > 0)
                        {
                            optProduceValueMaster.BeginDate = ClientUtil.ToDateTime(optProduceValueMaster.AccountYear + "-" + optProduceValueMaster.AccountMonth + "-" + startDate).AddMonths(-1);
                            optProduceValueMaster.EndDate = ClientUtil.ToDateTime(optProduceValueMaster.AccountYear + "-" + optProduceValueMaster.AccountMonth + "-" + endDate);
                        }
                    }
                    else if (schedulePlan.ExecScheduleType == EnumExecScheduleType.季度进度计划)
                    {
                        optProduceValueMaster.AccountType = ProduceSelfValueMasterAccountType.季度核算;

                        if (optProduceValueMaster.AccountYear > 0 && optProduceValueMaster.AccountMonth > 0)
                        {
                            optProduceValueMaster.BeginDate = ClientUtil.ToDateTime(optProduceValueMaster.AccountYear + "-" + optProduceValueMaster.AccountMonth + "-" + startDate).AddMonths(-3);
                            optProduceValueMaster.EndDate = ClientUtil.ToDateTime(optProduceValueMaster.AccountYear + "-" + optProduceValueMaster.AccountMonth + "-" + endDate);
                        }
                    }
                }

                oq.Criterions.Clear();
                oq.FetchModes.Clear();

                oq.AddCriterion(Expression.Eq("Master.Id", schedulePlan.Id));
                oq.AddFetchMode("GWBSTree", FetchMode.Eager);
                IList listSchedulePlanDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq);

                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                oq.Orders.Clear();

                oq.AddCriterion(Expression.Eq("TheProjectGUID", schedulePlan.ProjectId));
                oq.AddCriterion(Expression.Eq("CostAccFlag", true));
                Disjunction dis = new Disjunction();
                foreach (WeekScheduleDetail dtl in listSchedulePlanDtl)
                {
                    string[] sysCodes = dtl.GWBSTree.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < sysCodes.Length; i++)
                    {
                        string sysCode = "";
                        for (int j = 0; j <= i; j++)
                        {
                            sysCode += sysCodes[j] + ".";
                        }
                        dis.Add(Expression.Eq("SysCode", sysCode));
                    }
                }
                oq.AddCriterion(dis);
                oq.AddOrder(NHibernate.Criterion.Order.Desc("SysCode"));

                IEnumerable<GWBSTree> listAllGWBS = dao.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>();//所有的核算节点

                if (listAllGWBS.Count() == 0)
                    return optProduceValueMaster;

                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                dis = new Disjunction();


                List<string> listAccGWBSIds = new List<string>();
                foreach (GWBSTree g in listAllGWBS)
                {
                    listAccGWBSIds.Add(g.Id);
                }

                //取进度计划的末级节点
                listSchedulePlanDtl = (from d in listSchedulePlanDtl.OfType<WeekScheduleDetail>()
                                       orderby d.GWBSTreeSysCode ascending
                                       select d).ToList();
                IList listScheduleDtl = new ArrayList();
                foreach (WeekScheduleDetail sDtl in listSchedulePlanDtl)
                {
                    if (listScheduleDtl.Count == 0)
                    {
                        listScheduleDtl.Add(sDtl);
                    }
                    else
                    {
                        var query = from d in listScheduleDtl.OfType<WeekScheduleDetail>()
                                    where sDtl.GWBSTreeSysCode.IndexOf(d.GWBSTreeSysCode) > -1
                                    select d;

                        if (query.Count() > 0)
                        {
                            listScheduleDtl.Remove(query.ElementAt(0));
                            listScheduleDtl.Add(sDtl);
                        }
                        else
                        {
                            var query2 = from d in listScheduleDtl.OfType<WeekScheduleDetail>()
                                         where d.GWBSTreeSysCode.IndexOf(sDtl.GWBSTreeSysCode) > -1
                                         select d;

                            if (query2.Count() == 0)//没有任何隶属关系的也要加
                                listScheduleDtl.Add(sDtl);
                        }
                    }
                }

                Dictionary<string, IList> listAccountNodes = new Dictionary<string, IList>();//存储核算节点id,如果第一次计算就清除数据，之后累加
                //循环1：针对<操作{周（月）进度计 划明细}集>中个个对象处理
                foreach (WeekScheduleDetail dtl in listScheduleDtl)
                {
                    GWBSTree AccountWbs = null;//取最近核算节点
                    foreach (GWBSTree gt in listAllGWBS)
                    {
                        if (dtl.GWBSTree.SysCode.IndexOf(gt.SysCode) > -1)
                        {
                            AccountWbs = gt;
                            break;
                        }
                    }

                    //循环2：针对<产值核算节点 集>中每个对象处理
                    if (AccountWbs != null)
                    {
                        var queryProduceValueDtl = from d in optProduceValueMaster.Details.OfType<ProduceSelfValueDetail>()
                                                   where d.GWBSTree != null && d.GWBSTree.Id == AccountWbs.Id
                                                   select d;

                        ObjectQuery oq1 = new ObjectQuery();
                        oq1.AddCriterion(Expression.Eq("TheGWBS.Id", AccountWbs.Id));
                        oq1.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));
                        IList listAccountDtl = dao.ObjectQuery(typeof(GWBSDetail), oq1);


                        ProduceSelfValueDetail optDtl = null;//<操作{自行产值明细}
                        if (queryProduceValueDtl.Count() == 0)
                        {
                            optDtl = new ProduceSelfValueDetail();
                            optDtl.Master = optProduceValueMaster;
                            optProduceValueMaster.Details.Add(optDtl);

                            optDtl.GWBSTree = AccountWbs;
                            optDtl.GWBSTreeName = AccountWbs.Name;
                            optDtl.GWBSTreeSysCode = AccountWbs.SysCode;

                            optDtl.PlanValue = 0;
                            optDtl.RealValue = 0;

                            optDtl.PlanProgress = 0;
                            optDtl.RealProgress = 0;
                        }
                        else
                        {
                            optDtl = queryProduceValueDtl.ElementAt(0);
                        }

                        if (listAccountNodes.Keys.Contains(AccountWbs.Id) == false)
                        {
                            optDtl.PlanProgress = 0;
                            optDtl.PlanValue = 0;
                        }

                        //取下属子节点最晚计划结束时间和最早计划起始时间的工期
                        oq.Criterions.Clear();
                        oq.FetchModes.Clear();
                        oq.Orders.Clear();

                        oq.AddCriterion(Expression.Eq("Master.Id", dtl.Master.Id));
                        oq.AddCriterion(Expression.Like("GWBSTreeSysCode", dtl.GWBSTreeSysCode, MatchMode.Start));
                        IEnumerable<WeekScheduleDetail> listWeekDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq).OfType<WeekScheduleDetail>();

                        DateTime minDateTime = listWeekDtl.Min(w => w.PlannedBeginDate);
                        DateTime maxDateTime = listWeekDtl.Max(w => w.PlannedEndDate);
                        int planDays = (maxDateTime - minDateTime).Days + 1;

                        //从滚动进度计划中取总工期
                        oq.Criterions.Clear();
                        oq.FetchModes.Clear();
                        oq.Orders.Clear();

                        //根据月计划里的任务到总滚动进度计划里取核算节点的计划总工期
                        oq.AddCriterion(Expression.Eq("Master.Id", schedulePlan.ForwardBillId));
                        oq.AddCriterion(Expression.Like("GWBSTreeSysCode", dtl.GWBSTreeSysCode, MatchMode.Start));
                        IEnumerable<ProductionScheduleDetail> listPlanDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

                        if (listPlanDtl.Count() == 0)
                            continue;

                        minDateTime = listPlanDtl.Min(w => w.PlannedBeginDate);
                        maxDateTime = listPlanDtl.Max(w => w.PlannedEndDate);
                        int planCountDays = (maxDateTime - minDateTime).Days + 1;

                        //查询核算节点下所有非无效的明细和耗用（耗用在后续计算分包取费明细的计划产值使用）
                        oq.Criterions.Clear();
                        oq.FetchModes.Clear();
                        oq.Orders.Clear();

                        List<GWBSDetail> listGWBSDetails = new List<GWBSDetail>();
                        List<GWBSDetailCostSubject> listGWBSCostSubject = new List<GWBSDetailCostSubject>();
                        if (listAccountNodes.Keys.Contains(AccountWbs.Id) == false)
                        {
                            IList listDtlAndUsage = new ArrayList();

                            oq.AddCriterion(Expression.Like("TheGWBSSysCode", AccountWbs.SysCode, MatchMode.Start));
                            oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));
                            listGWBSDetails = dao.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>().ToList();

                            oq.Criterions.Clear();
                            dis = new Disjunction();
                            foreach (GWBSDetail dtlItem in listGWBSDetails)
                            {
                                dis.Add(Expression.Eq("TheGWBSDetail.Id", dtlItem.Id));
                            }
                            oq.AddCriterion(dis);
                            oq.AddFetchMode("ListCostSubjectDetails", FetchMode.Eager);
                            listGWBSCostSubject = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq).OfType<GWBSDetailCostSubject>().ToList();

                            listDtlAndUsage.Add(listGWBSDetails);
                            listDtlAndUsage.Add(listGWBSCostSubject);
                            listAccountNodes.Add(AccountWbs.Id, listDtlAndUsage);
                        }
                        else
                        {
                            IList listDtlAndUsage = listAccountNodes[AccountWbs.Id];
                            listGWBSDetails = listDtlAndUsage[0] as List<GWBSDetail>;
                            listGWBSCostSubject = listDtlAndUsage[1] as List<GWBSDetailCostSubject>;
                        }

                        //根据工程量权重算形象进度
                        decimal currPlanProgress = 0;
                        decimal productDtlContractTotalPrice = 0;
                        decimal accountDtlContractTotalPrice = 0;

                        var queryProductDtl = from d in listGWBSDetails.OfType<GWBSDetail>()
                                              where d.TheGWBS.Id == dtl.GWBSTree.Id && d.ProduceConfirmFlag == 1
                                              select d;

                        foreach (GWBSDetail proDtl in queryProductDtl)
                        {
                            var queryAccDtl = from d in listAccountDtl.OfType<GWBSDetail>()
                                              where ((d.TheCostItem == null && proDtl.TheCostItem == null) || (d.TheCostItem.Id == proDtl.TheCostItem.Id))
                                                   && d.MainResourceTypeId == proDtl.MainResourceTypeId
                                                   && d.DiagramNumber == proDtl.DiagramNumber
                                              select d;

                            productDtlContractTotalPrice += proDtl.ContractTotalPrice;
                            accountDtlContractTotalPrice += queryAccDtl.Sum(d => d.ContractTotalPrice);
                        }

                        if (planCountDays > 0 && accountDtlContractTotalPrice > 0)
                        {
                            decimal perDur = (decimal)planDays / (decimal)planCountDays;
                            if (perDur > 1)
                                perDur = 1;
                            decimal perPrice = (productDtlContractTotalPrice / accountDtlContractTotalPrice);

                            currPlanProgress = perDur * perPrice;
                        }

                        //根据合同收入计算计划产值
                        optDtl.PlanProgress += currPlanProgress;
                        optDtl.PlanValue += currPlanProgress * accountDtlContractTotalPrice;

                        #region 分包取费计划产值

                        IList listSubContractFeeDtl = new ArrayList();
                        IList listSubContractFeeDtlUsage = new ArrayList();

                        foreach (GWBSDetail ddtl in listGWBSDetails)
                        {
                            if (ddtl.SubContractFeeFlag == true && ((ddtl.ResponseFlag == 1 || ddtl.CostingFlag == 1)))
                            {
                                bool flag1 = false; //判断是否有耗用
                                foreach (GWBSDetailCostSubject cost in listGWBSCostSubject)
                                {
                                    if (cost.TheGWBSDetail.Id == ddtl.Id)
                                    {
                                        flag1 = true;
                                        listSubContractFeeDtlUsage.Add(cost);
                                    }
                                }
                                if (flag1)
                                {
                                    listSubContractFeeDtl.Add(ddtl);
                                }
                            }
                        }
                        IList listResult = AccountSubContractFeeDtl(listSubContractFeeDtl, listSubContractFeeDtlUsage);
                        listSubContractFeeDtl = listResult[0] as IList;
                        listSubContractFeeDtlUsage = listResult[1] as IList;

                        decimal ContractTotalPrice = 0;
                        foreach (GWBSDetail gdtl in listSubContractFeeDtl)
                        {
                            ContractTotalPrice += gdtl.ContractTotalPrice;
                        }

                        //此次完成的计划分包取费合同合价=计划产值的形象进度*(核算明细合同合价/核算节点总合同合价)*分包取费合同合价
                        decimal sumAccContractTotalPrice = (from d in listAccountDtl.OfType<GWBSDetail>()
                                                            where d.SubContractFeeFlag == false
                                                            select d).Sum(p => p.ContractTotalPrice);
                        if (sumAccContractTotalPrice > 0)
                        {
                            decimal perAccContractTotalPrice = accountDtlContractTotalPrice / sumAccContractTotalPrice;
                            optDtl.PlanValue += optDtl.PlanProgress * perAccContractTotalPrice * ContractTotalPrice;
                        }
                        #endregion

                    }
                }

                optProduceValueMaster.PlanDate = DateTime.Now;
                optProduceValueMaster.State = ProduceSelfValueMasterState.计划产值已生成;
                dao.SaveOrUpdate(optProduceValueMaster);
                return optProduceValueMaster;
            }
            catch (Exception ex)
            {
                string msg = "计划产值生成失败，" + ExceptionUtil.ExceptionMessage(ex);
                throw new Exception(msg);
            }
        }

        [TransManager]
        private ProduceSelfValueMaster RealOutputValueAccount(ProduceSelfValueMaster master)
        {
            #region 写入自行产值数据

            //查询当月自行产值数据
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", master.ProjectId));
            oq.AddCriterion(Expression.Eq("AccountOrgGUID", master.OperOrgInfo.Id));

            if (master.AccountType == ProduceSelfValueMasterAccountType.季度核算)
            {
                if (master.AccountMonth <= 10)
                {
                    oq.AddCriterion(Expression.Eq("Kjn", master.AccountYear));
                    oq.AddCriterion(Expression.And(Expression.Ge("Kjy", master.AccountMonth), Expression.Le("Kjy", master.AccountMonth + 2)));
                }
                else
                {
                    Disjunction dis = new Disjunction();
                    dis.Add(Expression.And(Expression.Eq("Kjn", master.AccountYear),
                        Expression.And(Expression.Ge("Kjy", master.AccountMonth), Expression.Le("Kjy", 12))));
                    dis.Add(Expression.And(Expression.Eq("Kjn", master.AccountYear + 1),
                        Expression.And(Expression.Ge("Kjy", 1), Expression.Le("Kjy", (master.AccountMonth + 2) - 12))));
                    oq.AddCriterion(dis);
                }
            }
            else if (master.AccountType == ProduceSelfValueMasterAccountType.月度核算)
            {
                oq.AddCriterion(Expression.Eq("Kjn", master.AccountYear));
                oq.AddCriterion(Expression.Eq("Kjy", master.AccountMonth));
            }
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

            IList list_MonthAccount = Dao.ObjectQuery(typeof(CostMonthAccountBill), oq);
            if (list_MonthAccount.Count == 0)
                return master;

            foreach (ProduceSelfValueDetail outputValDetail in master.Details)//核算节点
            {
                foreach (CostMonthAccountBill monAccBill in list_MonthAccount)
                {
                    foreach (CostMonthAccountDtl monAccDtl in monAccBill.Details)
                    {
                        if (!string.IsNullOrEmpty(monAccDtl.AccountTaskNodeSyscode) && monAccDtl.AccountTaskNodeSyscode.IndexOf(outputValDetail.GWBSTreeSysCode) > -1)
                        {
                            outputValDetail.RealValue += monAccDtl.CurrIncomeTotalPrice;
                        }
                    }
                }
            }

            master.State = ProduceSelfValueMasterState.实际产值已生成;

            return master;

            #endregion
        }

        /// <summary>
        /// 计算分包取费任务明细的预算成本
        /// </summary>
        /// <param name="listDtl">分包取费明细集</param>
        /// <returns></returns>
        public IList AccountSubContractFeeDtl(IList listDtl, IList listDtlUsage)
        {
            IList listResult = new ArrayList();

            IEnumerable<GWBSDetailCostSubject> listDtlUsageEdit = listDtlUsage.OfType<GWBSDetailCostSubject>();

            ObjectQuery oq = new ObjectQuery();
            foreach (GWBSDetail dtl in listDtl)
            {
                oq.Criterions.Clear();

                //1.根据分包措施明细的过滤条件查询下属实体任务明细的资源耗用

                oq.AddCriterion(Expression.Like("TheGWBSSysCode", dtl.TheGWBSSysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("SubContractFeeFlag", false));
                //oq.AddCriterion(Expression.Eq("State", GWBSDetailState.有效));
                //oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));
                //if (state != DocumentState.Invalid)
                //{
                //    oq.AddCriterion(Expression.Eq("State", state));
                //}
                //else
                //{
                //    oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));
                //}

                Disjunction dis = new Disjunction();//使用Disjunction 如果分包取费明细不包含 责任核算标记和成本核算标记 时则不查出任何核素任务明细数据
                if (dtl.ResponseFlag == 1 && dtl.CostingFlag == 1)
                {
                    dis.Add(Expression.Eq("ResponseFlag", 1));
                    dis.Add(Expression.Eq("CostingFlag", 1));
                }
                else if (dtl.ResponseFlag == 1)
                {
                    dis.Add(Expression.Eq("ResponseFlag", 1));
                }
                else if (dtl.CostingFlag == 1)
                {
                    dis.Add(Expression.Eq("CostingFlag", 1));
                }
                oq.AddCriterion(dis);


                #region 按成本项分类过滤（AND关系，各个分类之间OR关系）

                //如果未设置分类过滤条件则查询所有
                Disjunction dis1 = new Disjunction();
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode1))
                {
                    dis1.Add(Expression.Like("TheCostItem.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode1, MatchMode.Start));
                }
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode2))
                {
                    dis1.Add(Expression.Like("TheCostItem.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode2, MatchMode.Start));
                }
                string term = dis1.ToString();
                if (term != "()")//不加条件时为()
                    oq.AddCriterion(dis1);

                #endregion

                IEnumerable<GWBSDetail> listAccountDtl = dao.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>();


                if (listAccountDtl.Count() > 0
                    && (dtl.TheCostItem.SubjectCateFilter1 != null || dtl.TheCostItem.SubjectCateFilter2 != null || dtl.TheCostItem.SubjectCateFilter3 != null)
                    )//过滤核算科目
                {
                    #region 下属资源耗用按核算科目过滤（AND关系，各个科目之间OR关系）

                    ObjectQuery oqUsage = new ObjectQuery();

                    Disjunction disUsageDtl = new Disjunction();
                    foreach (GWBSDetail item in listAccountDtl)
                    {
                        disUsageDtl.Add(Expression.Eq("TheGWBSDetail.Id", item.Id));
                    }
                    oqUsage.AddCriterion(disUsageDtl);


                    Disjunction disUsage = new Disjunction();
                    if (dtl.TheCostItem.SubjectCateFilter1 != null)
                    {
                        disUsage.Add(Expression.Eq("CostAccountSubjectGUID.Id", dtl.TheCostItem.SubjectCateFilter1.Id));
                    }
                    if (dtl.TheCostItem.SubjectCateFilter2 != null)
                    {
                        disUsage.Add(Expression.Eq("CostAccountSubjectGUID.Id", dtl.TheCostItem.SubjectCateFilter2.Id));
                    }
                    if (dtl.TheCostItem.SubjectCateFilter3 != null)
                    {
                        disUsage.Add(Expression.Eq("CostAccountSubjectGUID.Id", dtl.TheCostItem.SubjectCateFilter3.Id));
                    }
                    oqUsage.AddCriterion(disUsage);

                    #endregion

                    IEnumerable<GWBSDetailCostSubject> listAccountUsage = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oqUsage).OfType<GWBSDetailCostSubject>();

                    if (listAccountUsage.Count() > 0)
                    {
                        #region 计算措施费明细的量价
                        //计算合同预算量价
                        if (true)
                        {
                            decimal sumResponsibleTotalPrice = listAccountUsage.Sum(p => p.ContractTotalPrice) * dtl.TheCostItem.PricingRate;

                            dtl.ContractProjectQuantity = 1;
                            dtl.ContractPrice = sumResponsibleTotalPrice;
                            dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;

                            var queryUsage = from d in listDtlUsageEdit
                                             where d.TheGWBSDetail.Id == dtl.Id
                                             select d;

                            foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                            {
                                dtlUsage.ContractQuotaQuantity = 1;
                                dtlUsage.ContractQuantityPrice = dtl.ContractTotalPrice;
                                if (dtlUsage.ResourceUsageQuota != null)
                                    dtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                                dtlUsage.ContractPricePercent = 1;
                                dtlUsage.ContractBasePrice = dtlUsage.ContractQuantityPrice;

                                dtlUsage.ContractPrice = dtlUsage.ContractQuotaQuantity * dtlUsage.ContractQuantityPrice;
                                dtlUsage.ContractProjectAmount = dtl.ContractProjectQuantity * dtlUsage.ContractQuotaQuantity;
                                dtlUsage.ContractTotalPrice = dtlUsage.ContractProjectAmount * dtlUsage.ContractQuantityPrice;
                            }
                        }

                        //预算类型为责任成本
                        if (dtl.ResponseFlag == 1)
                        {
                            IEnumerable<GWBSDetail> listResponsibleDtl = from d in listAccountDtl
                                                                         where d.ResponseFlag == 1
                                                                         select d;
                            List<string> listResponsibleDtlId = new List<string>();
                            foreach (GWBSDetail d in listResponsibleDtl)
                            {
                                listResponsibleDtlId.Add(d.Id);
                            }

                            IEnumerable<GWBSDetailCostSubject> queryResponsible = from u in listAccountUsage
                                                                                  where listResponsibleDtlId.Contains(u.TheGWBSDetail.Id)
                                                                                  select u;

                            decimal sumResponsibleTotalPrice = queryResponsible.Sum(p => p.ResponsibilitilyTotalPrice) * dtl.TheCostItem.PricingRate;

                            dtl.ResponsibilitilyWorkAmount = 1;
                            dtl.ResponsibilitilyPrice = sumResponsibleTotalPrice;
                            dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                            var queryUsage = from d in listDtlUsageEdit
                                             where d.TheGWBSDetail.Id == dtl.Id
                                             select d;

                            foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                            {
                                dtlUsage.ResponsibleQuotaNum = 1;
                                dtlUsage.ResponsibilitilyPrice = dtl.ResponsibilitilyTotalPrice;
                                if (dtlUsage.ResourceUsageQuota != null)
                                    dtlUsage.ResponsibilitilyPrice = dtlUsage.ResponsibilitilyPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                                dtlUsage.ResponsiblePricePercent = 1;
                                dtlUsage.ResponsibleBasePrice = dtl.ResponsibilitilyPrice;

                                dtlUsage.ResponsibleWorkPrice = dtlUsage.ResponsibleQuotaNum * dtlUsage.ResponsibilitilyPrice;
                                dtlUsage.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleQuotaNum;
                                dtlUsage.ResponsibilitilyTotalPrice = dtlUsage.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleWorkPrice;
                            }
                        }

                        //预算类型为计划成本
                        if (dtl.CostingFlag == 1)
                        {
                            IEnumerable<GWBSDetail> listPlanDtl = from d in listAccountDtl
                                                                  where d.CostingFlag == 1
                                                                  select d;
                            List<string> listPlanDtlId = new List<string>();
                            foreach (GWBSDetail d in listPlanDtl)
                            {
                                listPlanDtlId.Add(d.Id);
                            }

                            IEnumerable<GWBSDetailCostSubject> queryPlan = from u in listAccountUsage
                                                                           where listPlanDtlId.Contains(u.TheGWBSDetail.Id)
                                                                           select u;

                            decimal sumPlanTotalPrice = queryPlan.Sum(p => p.PlanTotalPrice) * dtl.TheCostItem.PricingRate;

                            dtl.PlanWorkAmount = 1;
                            dtl.PlanPrice = sumPlanTotalPrice;
                            dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                            var queryUsage = from d in listDtlUsageEdit
                                             where d.TheGWBSDetail.Id == dtl.Id
                                             select d;

                            foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                            {
                                dtlUsage.PlanQuotaNum = 1;
                                dtlUsage.PlanPrice = dtl.PlanTotalPrice;
                                if (dtlUsage.ResourceUsageQuota != null)
                                    dtlUsage.PlanPrice = dtlUsage.PlanPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                                dtlUsage.PlanPricePercent = 1;
                                dtlUsage.PlanBasePrice = dtlUsage.PlanPrice;

                                dtlUsage.PlanWorkPrice = dtlUsage.PlanQuotaNum * dtlUsage.PlanPrice;
                                dtlUsage.PlanWorkAmount = dtl.PlanWorkAmount * dtlUsage.PlanQuotaNum;
                                dtlUsage.PlanTotalPrice = dtlUsage.PlanWorkAmount * dtlUsage.PlanWorkPrice;
                            }
                        }
                        #endregion
                    }
                }
                else if (listAccountDtl.Count() > 0)
                {
                    #region 计算措施费明细的量价
                    //计算合同预算量价
                    if (true)
                    {
                        decimal sumResponsibleTotalPrice = listAccountDtl.Sum(p => p.ContractTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.ContractProjectQuantity = 1;
                        dtl.ContractPrice = sumResponsibleTotalPrice;
                        dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;

                        var queryUsage = from d in listDtlUsageEdit
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.ContractQuotaQuantity = 1;
                            dtlUsage.ContractQuantityPrice = dtl.ContractTotalPrice;
                            if (dtlUsage.ResourceUsageQuota != null)
                                dtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                            dtlUsage.ContractPricePercent = 1;
                            dtlUsage.ContractBasePrice = dtlUsage.ContractQuantityPrice;

                            dtlUsage.ContractPrice = dtlUsage.ContractQuotaQuantity * dtlUsage.ContractQuantityPrice;
                            dtlUsage.ContractProjectAmount = dtl.ContractProjectQuantity * dtlUsage.ContractQuotaQuantity;
                            dtlUsage.ContractTotalPrice = dtlUsage.ContractProjectAmount * dtlUsage.ContractPrice;
                        }
                    }

                    //预算类型为责任成本
                    if (dtl.ResponseFlag == 1)
                    {
                        IEnumerable<GWBSDetail> listResponsibleDtl = from d in listAccountDtl
                                                                     where d.ResponseFlag == 1
                                                                     select d;

                        decimal sumResponsibleTotalPrice = listResponsibleDtl.Sum(p => p.ResponsibilitilyTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.ResponsibilitilyWorkAmount = 1;
                        dtl.ResponsibilitilyPrice = sumResponsibleTotalPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        var queryUsage = from d in listDtlUsageEdit
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.ResponsibleQuotaNum = 1;
                            dtlUsage.ResponsibilitilyPrice = dtl.ResponsibilitilyTotalPrice;
                            if (dtlUsage.ResourceUsageQuota != null)
                                dtlUsage.ResponsibilitilyPrice = dtlUsage.ResponsibilitilyPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                            dtlUsage.ResponsiblePricePercent = 1;
                            dtlUsage.ResponsibleBasePrice = dtl.ResponsibilitilyPrice;

                            dtlUsage.ResponsibleWorkPrice = dtlUsage.ResponsibleQuotaNum * dtlUsage.ResponsibilitilyPrice;
                            dtlUsage.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleQuotaNum;
                            dtlUsage.ResponsibilitilyTotalPrice = dtlUsage.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleWorkPrice;
                        }
                    }

                    //预算类型为计划成本
                    if (dtl.CostingFlag == 1)
                    {
                        IEnumerable<GWBSDetail> listPlanDtl = from d in listAccountDtl
                                                              where d.CostingFlag == 1
                                                              select d;

                        decimal sumPlanTotalPrice = listPlanDtl.Sum(p => p.PlanTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.PlanWorkAmount = 1;
                        dtl.PlanPrice = sumPlanTotalPrice;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        var queryUsage = from d in listDtlUsageEdit
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.PlanQuotaNum = 1;
                            dtlUsage.PlanPrice = dtl.PlanTotalPrice;
                            if (dtlUsage.ResourceUsageQuota != null)
                                dtlUsage.PlanPrice = dtlUsage.PlanPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                            dtlUsage.PlanPricePercent = 1;
                            dtlUsage.PlanBasePrice = dtlUsage.PlanPrice;

                            dtlUsage.PlanWorkPrice = dtlUsage.PlanQuotaNum * dtlUsage.PlanPrice;
                            dtlUsage.PlanWorkAmount = dtl.PlanWorkAmount * dtlUsage.PlanQuotaNum;
                            dtlUsage.PlanTotalPrice = dtlUsage.PlanWorkAmount * dtlUsage.PlanWorkPrice;
                        }
                    }
                    #endregion
                }
            }

            listResult.Add(listDtl);
            listResult.Add(listDtlUsage);

            return listResult;
        }

        public bool RecountPlanOutputValue(CurrentProjectInfo project)
        {
            ObjectQuery oq = new ObjectQuery();

            if (project != null)
                oq.AddCriterion(Expression.Eq("ProjectId", project.Id));

            //调试
            //oq.AddCriterion(Expression.Eq("CreatePersonName", "admin"));
            //oq.AddCriterion(Expression.Eq("AccountYear", ClientUtil.ToInt(2013)));
            //oq.AddCriterion(Expression.Eq("AccountMonth", ClientUtil.ToInt(8)));

            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.Or(Expression.Eq("ExecScheduleType", EnumExecScheduleType.季度进度计划), Expression.Eq("ExecScheduleType", EnumExecScheduleType.月度进度计划)));
            oq.AddOrder(Order.Asc("CreateDate"));
            IList listWeekPlan = dao.ObjectQuery(typeof(WeekScheduleMaster), oq);
            for (int i = 0; i < listWeekPlan.Count; i++)
            {
                WeekScheduleMaster master = listWeekPlan[i] as WeekScheduleMaster;
                CurrentProjectInfo projectInfo = dao.Get(typeof(CurrentProjectInfo), master.ProjectId) as CurrentProjectInfo;
                PlanedOutputValueAccount(master, projectInfo, master.HandleOrg);
            }

            return true;
        }

        public bool RecountRealOutputValue()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddOrder(Order.Asc("CreateYear"));
            oq.AddOrder(Order.Asc("CreateMonth"));

            IList listProValue = dao.ObjectQuery(typeof(ProduceSelfValueMaster), oq);
            for (int i = 0; i < listProValue.Count; i++)
            {
                ProduceSelfValueMaster master = listProValue[i] as ProduceSelfValueMaster;
                RealOutputValueAccount(master);
            }

            return true;
        }

        #endregion
    }
}
