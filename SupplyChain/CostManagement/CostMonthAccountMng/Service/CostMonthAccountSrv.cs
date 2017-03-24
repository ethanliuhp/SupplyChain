﻿﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Messaging;
﻿using System.Text;
﻿using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;
﻿using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
﻿using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.MaterialResource.Domain;
using NHibernate;
using NHibernate.Criterion;
using Oracle.DataAccess.Client;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using VirtualMachine.Core.DataAccess;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using CommonSearchLib.BillCodeMng.Service;
using Iesi.Collections.Generic;

using System.Linq;


namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Service
{
    /// <summary>
    /// 月度成本核算服务
    /// </summary>
    public class CostMonthAccountSrv : BaseService, ICostMonthAccountSrv
    {
        private IDao _Dao;

        public virtual IDao Dao
        {
            get { return _Dao; }
            set { _Dao = value; }
        }

        private IBillCodeRuleSrv billCodeRuleSrv;

        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        #region 通用功能

        /// <summary>
        /// 月度成本核算综合查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet CostMonthAcctGeneralQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql =
                @"select t1.kjn,t1.kjy,t1.theprojectname,t2.accounttasknodename,t2.projecttaskdtlname,t2.costitemname, " +
                " t3.costingsubjectname,t3.resourcetypename,t3.resourcetypespec,t3.rationunitname,t3.currrealconsumequantity, " +
                " t3.currrealconsumetotalprice,t3.currrealconsumeplanquantity,t3.currrealconsumeplantotalprice, " +
                " t3.currincomequantity,t3.currincometotalprice,t3.currresponsiconsumequantity,t3.currresponsiconsumetotalprice, " +
                " t3.sumrealconsumequantity,t3.sumrealconsumetotalprice,t3.sumrealconsumeplanquantity,t3.sumrealconsumeplantotalprice, " +
                " t3.sumincomequantity,t3.sumincometotalprice,t3.sumresponsiconsumequantity,t3.sumresponsiconsumetotalprice " +
                " from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 " +
                " where t1.id=t2.parentid and t2.id=t3.parentid " + condition;
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }

        /// <summary>
        /// 月度成本核算主表查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet QueryCostMonthAcctBill(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql =
                @" select t1.id,t1.accountrange,t1.accounttasksyscode,t1.createtime,t1.accountpersonname,t1.accounttaskname,t1.endtime,t1.accountorgname " +
                " from thd_costmonthaccount t1  where 1=1 " + condition;
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }

        /// <summary>
        /// 月度成本核算查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetCostMonthAccountBill(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof (CostMonthAccountBill), objectQuery);
        }

        /// <summary>
        /// 通过GWBS节点查询管理OBS对应的业务组织
        /// </summary>
        /// <returns></returns>
        public OBSManage GetOBSManageByTaskNode(string taskNodeSyscode, string projectId)
        {
            OBSManage mOBS = new OBSManage();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText =
                "select t1.orpjob,t1.orgjobname,t1.orgjobsyscode from THD_OBSMANAGE t1 where instr('" + taskNodeSyscode +
                "',t1.projecttasksyscode)>0 and t1.projectid='" + projectId + "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    mOBS.OrpJob =
                        new Application.Resource.PersonAndOrganization.OrganizationResource.Domain.OperationOrg();
                    mOBS.OrpJob.Id = TransUtil.ToString(dataRow["orpjob"]);
                    mOBS.OrgJobName = TransUtil.ToString(dataRow["orgjobname"]);
                    mOBS.OpgSysCode = TransUtil.ToString(dataRow["orgjobsyscode"]);
                }
            }
            return mOBS;
        }

        /// <summary>
        /// 成本核算对应业务单据查询
        /// </summary> 
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet QuerytCostMonthAccountBill(string costAccountGUID)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql = @" select '工程任务核算' billtype,t1.code,0 summoney,t1.accountpersonname personname,t1.createdate from thd_projecttaskaccountbill t1
                              where t1.monthaccountbill='" + costAccountGUID + @"'
                            union all
                            select '分包结算' billtype,t2.code,t2.balancemoney summoney,t2.createpersonname personname,t2.createdate from thd_subcontractbalancebill t2
                              where t2.monthaccbillid='" +
                         costAccountGUID + @"'
                            union all
                            select '材料耗用结算' billtype,t3.code,t3.summoney,t3.createpersonname personname,t3.createdate from thd_materialsettlemaster t3 
                              where t3.settlestate='materialConsume' and t3.monthaccountbill='" +
                         costAccountGUID + @"'
                            union all
                            select '料具租赁结算' billtype,t4.code,t4.summoney,t4.createpersonname personname,t4.createdate from thd_materialsettlemaster t4 
                              where t4.settlestate='materialQuery' and t4.monthaccountbill='" +
                         costAccountGUID + @"'
                            union all
                            select '机械租赁结算' billtype,t5.code,t5.summoney,t5.createpersonname personname,t5.createdate from THD_MaterialRentelSetMaster t5
                              where t5.monthaccountbillid='" +
                         costAccountGUID + @"'
                            union all
                            select '财务费用结算' billtype,t6.code,t6.summoney,t6.createpersonname personname,t6.createdate from thd_expensessettlemaster t6 
                              where t6.monthlysettlment='" + costAccountGUID +
                         @"'";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        public DataSet QueryGenCostMonthInfo(string projectId, int kjn, int kjy, string materialGuid,
                                             string materialCatGuid, string subjectGuid, string taskGuid)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = " select id from thd_costmonthaccount t1 where t1.theprojectguid='" + projectId +
                                  "' and t1.kjn=" + kjn + " and t1.kjy=" + kjy;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            string costAccountIdStr = "";
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (costAccountIdStr == "")
                    {
                        costAccountIdStr = "'" + TransUtil.ToString(dataRow["id"]) + "'";
                    }
                    else
                    {
                        costAccountIdStr += ",'" + TransUtil.ToString(dataRow["id"]) + "'";
                    }
                }
            }

            if (costAccountIdStr == "")
                return ds;
            string taskAcctBillSql = " and t1.monthaccountbill in (" + costAccountIdStr + ")";
            string subBalBillSql = " and t1.monthaccbillid in (" + costAccountIdStr + ")";
            string materialBillSql = " and t1.monthaccountbill in (" + costAccountIdStr + ")";
            string rentalBillSql = " and t1.monthaccountbill in (" + costAccountIdStr + ")";
            string expenseBillSql = " and t1.monthlysettlment in (" + costAccountIdStr + ")";
            string machineBillSql = " and t1.monthaccountbillid in (" + costAccountIdStr + ")";
            if (TransUtil.ToString(materialGuid) != "")
            {
                taskAcctBillSql += " and t3.resourcetypeguid = '" + materialGuid + "'";
                subBalBillSql += " and t3.resourcetypeguid = '" + materialGuid + "'";
                materialBillSql += " and t2.material = '" + materialGuid + "'";
                rentalBillSql += " and t2.material = '" + materialGuid + "'";
                expenseBillSql += " and t2.materialresource = '" + materialGuid + "'";
                machineBillSql += " and t2.material = '" + materialGuid + "'";
            }
            if (TransUtil.ToString(materialCatGuid) != "")
            {
                taskAcctBillSql += " and t3.resourcecategorysyscode like '%" + materialCatGuid + "%'";
                subBalBillSql += " and t3.resourcesyscode like '%" + materialCatGuid + "%'";
                materialBillSql += " and t2.materialsyscode like '%" + materialCatGuid + "%'";
                rentalBillSql += " and t2.materialsyscode like '%" + materialCatGuid + "%'";
                expenseBillSql += " and t2.materialsyscode like '%" + materialCatGuid + "%'";
                machineBillSql += " and t2.materialsyscode like '%" + materialCatGuid + "%'";
            }
            if (TransUtil.ToString(subjectGuid) != "")
            {
                taskAcctBillSql += " and t3.accountcostsyscode like '%" + subjectGuid + "%'";
                subBalBillSql += " and t3.balancesubjectsyscode like '%" + subjectGuid + "%'";
                materialBillSql += " and t2.accountcostcode like '%" + subjectGuid + "%'";
                rentalBillSql += " and t2.accountcostcode like '%" + subjectGuid + "%'";
                expenseBillSql += " and t2.accountcostsyscode like '%" + subjectGuid + "%'";
                machineBillSql += " and t3.settlesubjectsyscode like '%" + subjectGuid + "%'";
            }
            if (TransUtil.ToString(taskGuid) != "")
            {
                taskAcctBillSql += " and t2.accounttasknodesyscode like '%" + taskGuid + "%'";
                subBalBillSql += " and t2.balancetasksyscode like '%" + taskGuid + "%'";
                materialBillSql += " and t2.projecttaskcode like '%" + taskGuid + "%'";
                rentalBillSql += " and t2.projecttaskcode like '%" + taskGuid + "%'";
                expenseBillSql += " and t2.projecttasksyscode like '%" + taskGuid + "%'";
                machineBillSql += " and t2.usedpartsyscode like '%" + taskGuid + "%'";
            }

            string sql =
                "select t3.resourcetypename,t3.resourcetypespec,t3.resourcetypequality,t2.accounttasknodename,t3.bestaetigtcostsubjectname," +
                " 0 realqty,0 realmoney,t1.code,'工程任务核算' billtype,t3.accusageqny,t3.accounttotalprice," +
                " t3.currcontractincomeqny,t3.currcontractincometotal,t3.currresponsiblecostqny,t3.currresponsiblecosttotal " +
                " from thd_projecttaskaccountbill t1,thd_projecttaskdetailaccount t2,thd_projecttaskdtlacctsubject t3 " +
                " where t1.id=t2.parentid and t2.id=t3.parentid " + taskAcctBillSql +
                " union " +
                " select t3.resourcetypename,t3.resourcetypespec,t3.resourcetypestuff,t2.balancetaskname,t3.balancesubjectname, " +
                " t3.balancequantity,t3.balancetotalprice,t1.code,'分包结算',0,0,0,0,0,0 " +
                " from thd_subcontractbalancebill t1,thd_subcontractbalancedetail t2,thd_subcontractbalsubjectdtl t3 " +
                " where t1.id=t2.parentid and t2.id=t3.parentid " + subBalBillSql +
                " union " +
                " select t2.materialname,t2.materialspec,t2.materialstuff,t2.projecttaskname,t2.accountcostname, " +
                " t2.quantity,t2.money,t1.code,'财务费用结算' ,0,0,0,0,0,0 " +
                " from thd_expensessettlemaster t1,thd_expensessettledetail t2 where t1.id=t2.parentid " +
                expenseBillSql +
                " union " +
                " select t2.materialname,t2.materialspec,t2.materialstuff,t2.projecttaskname,t2.accountcostname, " +
                " t2.quantity,t2.money,t1.code,'材料消耗结算' ,0,0,0,0,0,0 " +
                " from thd_materialsettlemaster t1,thd_materialsettledetail t2  " +
                " where t1.id=t2.parentid and t1.settlestate='materialConsume' " + materialBillSql +
                " union " +
                " select t2.materialname,t2.materialspec,t2.materialstuff,t2.projecttaskname,t2.accountcostname, " +
                " t2.quantity,t2.money,t1.code,'料具租赁结算' ,0,0,0,0,0,0 " +
                " from thd_materialsettlemaster t1,thd_materialsettledetail t2  " +
                " where t1.id=t2.parentid and t1.settlestate='materialQuery' " + rentalBillSql +
                " union " +
                " select t2.materialname,t2.materialspec,t2.materialstuff,t2.usedpartname,t3.settlesubjectname, " +
                " t3.settlequantity,t3.settlemoney,t1.code,'机械租赁结算' ,0,0,0,0,0,0 " +
                " from thd_materialrentelsetmaster t1,thd_materialrentalsetdetail t2,thd_materialsubjectdetail t3 " +
                " where t1.id=t2.parentid and t2.id=t3.parentid" + machineBillSql;
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        #endregion

        #region 月度成本核算优化算法(分开计算节点、资源、科目)

        /// <summary>
        /// 月度成本核算被调用主程序
        /// </summary>
        [TransManager]
        private string CostMonthAccountBySubjectAndResource(CostMonthAccountBill costBill,
                                                            CurrentProjectInfo projectInfo)
        {
            string msgstr = "";
            Hashtable ht_monthdtl = new Hashtable(); //月度成本核算明细
            Hashtable ht_monthdtlconsume = new Hashtable(); //根据【核算科目ID】对任务明细下资源耗用生成月度成本耗用明细 记录计划、合同、责任价格和责任合价
            Hashtable ht_sumSubject = new Hashtable(); //根据科目构造预算数据
            //获取科目编码信息
            Hashtable ht_subject = this.GetCostSubjectList(); //成本核算科目集合

            #region 1：构造本月成本核算信息, 核算范围节点构造<核算节点集合><工程任务核算明细集合><核算明细资源耗用集合>

            #region 获取核算范围内所有工程节点

            //1.1 从GWBS树中查询到核算节点下的所有核算节点(包括本身)
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", costBill.ProjectId));
            oq.AddCriterion(Expression.Eq("CostAccFlag", true));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails.CostingSubjectGUID", NHibernate.FetchMode.Eager);
            IList list_GWBS = Dao.ObjectQuery(typeof (GWBSTree), oq);

            #endregion

            //1.2 构造<核算节点集合><工程任务核算明细集合><核算明细资源耗用集合>
            CostMonthAccountDtl costFirstDtl = new CostMonthAccountDtl();
            foreach (GWBSTree model in list_GWBS)
            {
                foreach (GWBSDetail detail in model.Details)
                {
                    if (detail.CostingFlag == 0)
                        continue;
                    //生成一条月度成本核算明细
                    if (ht_monthdtl.Count == 0)
                    {
                        costFirstDtl.AccountTaskNodeGUID = model;
                        costFirstDtl.AccountTaskNodeName = model.Name;
                        costFirstDtl.AccountTaskNodeSyscode = model.SysCode;
                        costFirstDtl.ProjectTaskDtlGUID = detail;
                        costFirstDtl.ProjectTaskDtlName = detail.Name;
                        ht_monthdtl.Add(model.SysCode + "-" + detail.Id, costFirstDtl);
                    }

                    foreach (GWBSDetailCostSubject sonDetail in detail.ListCostSubjectDetails)
                    {
                        if (sonDetail.CostAccountSubjectGUID == null)
                        {
                            continue;
                        }

                        #region 实体部分根据资源类型+成本核算科目进行合同、责任、计划量和合价的汇总

                        //实体：形象进度>100 为100 否则就是当前进度
                        string dtlConsumeLinkStr = sonDetail.ResourceTypeGUID + "-" +
                                                   sonDetail.CostAccountSubjectGUID.Id;
                        decimal progress = detail.AddupAccFigureProgress;
                        if (progress > 100)
                        {
                            progress = 100;
                        }
                        string subjectCode = ht_subject[sonDetail.CostAccountSubjectGUID.Id] as string;
                        if (subjectCode != null && subjectCode != "" && ifMoneyCal(subjectCode) == false)
                        {
                            if (!ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                            {
                                CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                                dtlConsume.CostingSubjectName = sonDetail.CostAccountSubjectName;
                                dtlConsume.CostSubjectCode = sonDetail.CostAccountSubjectGUID.Code;
                                dtlConsume.CostingSubjectGUID = sonDetail.CostAccountSubjectGUID;
                                dtlConsume.CurrIncomePrice = sonDetail.ContractQuantityPrice;
                                dtlConsume.CurrRealConsumePlanPrice = sonDetail.PlanPrice;
                                dtlConsume.CurrResponsiConsumePrice = sonDetail.ResponsibilitilyPrice;
                                dtlConsume.CalType = 0;
                                dtlConsume.ResourceTypeGUID = sonDetail.ResourceTypeGUID;
                                dtlConsume.ResourceTypeName = sonDetail.ResourceTypeName;
                                dtlConsume.RationUnitGUID = sonDetail.ProjectAmountUnitGUID;
                                dtlConsume.RationUnitName = sonDetail.ProjectAmountUnitName;
                                dtlConsume.TheAccountDetail = costFirstDtl;
                                dtlConsume.Data1 = sonDetail.PlanTotalPrice + ""; //计划耗用合价
                                dtlConsume.SumIncomeQuantity =
                                    decimal.Round(sonDetail.ContractProjectAmount*progress/100, 4);
                                dtlConsume.SumIncomeTotalPrice = decimal.Round(
                                    sonDetail.ContractTotalPrice*progress/100, 2);
                                dtlConsume.SumRealConsumePlanQuantity =
                                    decimal.Round(sonDetail.PlanWorkAmount*progress/100, 4);
                                dtlConsume.SumRealConsumePlanTotalPrice =
                                    decimal.Round(sonDetail.PlanTotalPrice*progress/100, 2);
                                dtlConsume.SumResponsiConsumeQuantity =
                                    decimal.Round(sonDetail.ResponsibilitilyWorkAmount*progress/100, 4);
                                dtlConsume.SumResponsiConsumeTotalPrice =
                                    decimal.Round(sonDetail.ResponsibilitilyTotalPrice*progress/100, 2);
                                ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                            }
                            else
                            {
                                CostMonthAccDtlConsume dtlConsume =
                                    (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                                dtlConsume.SumIncomeQuantity +=
                                    decimal.Round(sonDetail.ContractProjectAmount*progress/100, 4);
                                dtlConsume.SumIncomeTotalPrice +=
                                    decimal.Round(sonDetail.ContractTotalPrice*progress/100, 2);
                                dtlConsume.SumRealConsumePlanQuantity +=
                                    decimal.Round(sonDetail.PlanWorkAmount*progress/100, 4);
                                dtlConsume.SumRealConsumePlanTotalPrice +=
                                    decimal.Round(sonDetail.PlanTotalPrice*progress/100, 2);
                                dtlConsume.SumResponsiConsumeQuantity +=
                                    decimal.Round(sonDetail.ResponsibilitilyWorkAmount*progress/100, 4);
                                dtlConsume.SumResponsiConsumeTotalPrice +=
                                    decimal.Round(sonDetail.ResponsibilitilyTotalPrice*progress/100, 2);
                            }
                        }

                        string dtlWBSStr = sonDetail.CostAccountSubjectGUID.Id;
                        if (!ht_sumSubject.Contains(dtlWBSStr)) //科目汇总
                        {
                            DataDomain dDomain = new DataDomain();
                            dDomain.Name1 = sonDetail.ContractQuantityPrice;
                            dDomain.Name2 = sonDetail.PlanPrice;
                            dDomain.Name3 = sonDetail.ResponsibilitilyPrice;
                            dDomain.Name4 = sonDetail.ContractProjectAmount;
                            dDomain.Name5 = sonDetail.PlanWorkAmount;
                            dDomain.Name6 = sonDetail.ResponsibilitilyWorkAmount;
                            dDomain.Name7 = sonDetail.ContractTotalPrice;
                            dDomain.Name8 = sonDetail.PlanTotalPrice;
                            dDomain.Name9 = sonDetail.ResponsibilitilyTotalPrice;
                            dDomain.Name10 = sonDetail.PlanTotalPrice;
                            dDomain.Name27 = sonDetail.CostAccountSubjectName;
                            ht_sumSubject.Add(dtlWBSStr, dDomain);
                        }
                        else
                        {
                            DataDomain dDomain = (DataDomain) ht_sumSubject[dtlWBSStr];
                            dDomain.Name4 = TransUtil.ToDecimal(dDomain.Name4) + sonDetail.ContractProjectAmount;
                            dDomain.Name5 = TransUtil.ToDecimal(dDomain.Name5) + sonDetail.PlanWorkAmount;
                            dDomain.Name6 = TransUtil.ToDecimal(dDomain.Name6) + sonDetail.ResponsibilitilyWorkAmount;
                            dDomain.Name7 = TransUtil.ToDecimal(dDomain.Name7) + sonDetail.ContractTotalPrice;
                            dDomain.Name8 = TransUtil.ToDecimal(dDomain.Name8) + sonDetail.PlanTotalPrice;
                            dDomain.Name9 = TransUtil.ToDecimal(dDomain.Name9) + sonDetail.ResponsibilitilyTotalPrice;
                            dDomain.Name10 = TransUtil.ToDecimal(dDomain.Name10) + sonDetail.PlanTotalPrice;
                        }

                        #endregion
                    }
                }
            }

            #endregion

            #region 2：工程任务核算单的月度汇总

            #region 查询工程任务核算单

            //2.1 查询工程任务核算信息
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime.AddDays(1)));
            oq.AddCriterion(Expression.IsNull("MonthAccountBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_PTaskAccount = Dao.ObjectQuery(typeof (ProjectTaskAccountBill), oq);

            #endregion

            //2.2汇总工程任务核算信息
            foreach (ProjectTaskAccountBill model in list_PTaskAccount)
            {
                //工程任务核算明细信息
                foreach (ProjectTaskDetailAccount dtl in model.Details)
                {
                    //工程任务资源耗用信息
                    foreach (ProjectTaskDetailAccountSubject subject in dtl.Details)
                    {
                        #region 根据【核算科目ID】和【资源】对【工程资源耗用单】进行汇总消耗量、核算合价、合同、责任量和合价、

                        if (subject.CostingSubjectGUID == null)
                            continue;
                        string dtlConsumeLinkStr = subject.ResourceTypeGUID + "-" + subject.CostingSubjectGUID.Id;
                        string subjectCode = ht_subject[subject.CostingSubjectGUID.Id] as string;
                        if (ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                        {
                            if (subjectCode != null && subjectCode != "" && ifMoneyCal(subjectCode) == false)
                            {
                                CostMonthAccDtlConsume dtlConsume =
                                    (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                                dtlConsume.CurrIncomeQuantity += subject.CurrContractIncomeQny;
                                dtlConsume.CurrIncomeTotalPrice += subject.CurrContractIncomeTotal;
                                dtlConsume.CurrRealConsumePlanQuantity += subject.AccUsageQny;
                                dtlConsume.CurrRealConsumePlanTotalPrice += subject.AccountTotalPrice;
                                dtlConsume.CurrResponsiConsumeQuantity += subject.CurrResponsibleCostQny;
                                dtlConsume.CurrResponsiConsumeTotalPrice += subject.CurrResponsibleCostTotal;
                            }
                        }
                        else
                        {
//新生成一个
                            CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                            dtlConsume.CalType = 0;
                            dtlConsume.CostingSubjectGUID = subject.CostingSubjectGUID;
                            dtlConsume.CostingSubjectName = subject.CostingSubjectName;
                            dtlConsume.CostSubjectCode = subject.CostingSubjectGUID.Code;
                            dtlConsume.ResourceTypeGUID = subject.ResourceTypeGUID;
                            dtlConsume.ResourceTypeName = subject.ResourceTypeName;
                            dtlConsume.ResourceTypeSpec = subject.ResourceTypeSpec;
                            dtlConsume.ResourceTypeStuff = subject.ResourceTypeQuality;
                            dtlConsume.ResourceSyscode = subject.ResourceCategorySysCode;
                            if (subjectCode != null && subjectCode != "" && ifMoneyCal(subjectCode) == false)
                            {
                                dtlConsume.CurrIncomeQuantity = subject.CurrContractIncomeQny;
                                dtlConsume.CurrIncomeTotalPrice = subject.CurrContractIncomeTotal;
                                dtlConsume.CurrRealConsumePlanQuantity = subject.AccUsageQny;
                                dtlConsume.CurrRealConsumePlanTotalPrice = subject.AccountTotalPrice;
                                dtlConsume.CurrResponsiConsumeQuantity = subject.CurrResponsibleCostQny;
                                dtlConsume.CurrResponsiConsumeTotalPrice = subject.CurrResponsibleCostTotal;
                            }
                            dtlConsume.TheAccountDetail = costFirstDtl;
                            ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                        }

                        #endregion
                    }
                }
            }

            #endregion

            #region 3：分包结算单的月度汇总

            //3.1 查询分包结算信息
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.IsNull("MonthAccBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_SubBalance = Dao.ObjectQuery(typeof (SubContractBalanceBill), oq);

            //3.2 分包结算信息进行月度结算
            Hashtable ht_sumConsumeDomain = new Hashtable(); //分包结算单消耗汇总 根据核算科目ID对【分包结算单】上面资源耗用数量和金额进行汇总            
            foreach (SubContractBalanceBill model in list_SubBalance)
            {
                foreach (SubContractBalanceDetail dtl in model.Details)
                {
                    foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                    {
                        string dtlConsumeLinkStr = subject.ResourceTypeGUID + "-" + subject.BalanceSubjectGUID.Id;
                        if (ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                        {
                            CostMonthAccDtlConsume tempDtlConsume =
                                (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                            tempDtlConsume.CurrRealConsumeQuantity += subject.BalanceQuantity;
                            tempDtlConsume.CurrRealConsumeTotalPrice += subject.BalanceTotalPrice;
                        }
                        else
                        {
                            CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                            dtlConsume.CalType = 0;
                            dtlConsume.CostingSubjectGUID = subject.BalanceSubjectGUID;
                            dtlConsume.CostingSubjectName = subject.BalanceSubjectName;
                            dtlConsume.CostSubjectCode = subject.BalanceSubjectGUID.Code;
                            dtlConsume.ResourceTypeGUID = subject.ResourceTypeGUID;
                            dtlConsume.ResourceTypeName = subject.ResourceTypeName;
                            dtlConsume.ResourceTypeSpec = subject.ResourceTypeSpec;
                            dtlConsume.ResourceTypeStuff = subject.ResourceTypeStuff;
                            dtlConsume.ResourceSyscode = subject.ResourceSyscode;
                            dtlConsume.CurrRealConsumeQuantity = subject.BalanceQuantity;
                            dtlConsume.CurrRealConsumeTotalPrice = subject.BalanceTotalPrice;
                            dtlConsume.CurrRealConsumePrice = subject.BalancePrice;
                            dtlConsume.TheAccountDetail = costFirstDtl;
                            ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                        }
                    }
                }
            }

            #endregion

            #region 4：物资耗用/料具租赁结算单的月度汇总

            //4.1 查询物资耗用结算信息和料具结算单
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.IsNull("MonthAccountBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list_resourceConsume = Dao.ObjectQuery(typeof (MaterialSettleMaster), oq);
            //4.2 汇总物资耗用结算
            foreach (MaterialSettleMaster model in list_resourceConsume)
            {
                foreach (MaterialSettleDetail subject in model.Details)
                {
                    if (subject.MaterialResource == null)
                    {
                        msgstr = "物资耗用结算中存在物资为空的情况！";
                        return msgstr;
                    }
                    if (subject.AccountCostSubject == null)
                    {
                        msgstr = "物资耗用结算中存在核算科目为空的情况！";
                        return msgstr;
                    }
                    string dtlConsumeLinkStr = subject.MaterialResource.Id + "-" + subject.AccountCostSubject.Id;
                    if (ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                    {
                        CostMonthAccDtlConsume tempDtlConsume =
                            (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                        tempDtlConsume.CurrRealConsumeQuantity += subject.Quantity;
                        tempDtlConsume.CurrRealConsumeTotalPrice += subject.Money;
                    }
                    else
                    {
                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        dtlConsume.CalType = 0;
                        dtlConsume.CostingSubjectGUID = subject.AccountCostSubject;
                        dtlConsume.CostingSubjectName = subject.AccountCostName;
                        dtlConsume.CostSubjectCode = subject.AccountCostCode;
                        dtlConsume.ResourceTypeGUID = subject.MaterialResource.Id;
                        dtlConsume.ResourceTypeName = subject.MaterialName;
                        dtlConsume.ResourceTypeSpec = subject.MaterialSpec;
                        dtlConsume.ResourceTypeStuff = subject.MaterialStuff;
                        dtlConsume.ResourceSyscode = subject.MaterialSysCode;
                        dtlConsume.CurrRealConsumeQuantity = subject.Quantity;
                        dtlConsume.CurrRealConsumeTotalPrice = subject.Money;
                        dtlConsume.CurrRealConsumePrice = subject.Price;
                        dtlConsume.TheAccountDetail = costFirstDtl;
                        ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                    }
                }
            }

            #endregion

            #region 5：设备租赁结算单的月度汇总

            // 查询设备租赁结算单
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.IsNull("MonthAccountBillId"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialSubjectDetails", NHibernate.FetchMode.Eager);
            IList list_machineConsume = Dao.ObjectQuery(typeof (MaterialRentalSettlementMaster), oq);
            // 汇总设备租赁结算
            foreach (MaterialRentalSettlementMaster model in list_machineConsume)
            {
                foreach (MaterialRentalSettlementDetail dtl in model.Details)
                {
                    foreach (MaterialSubjectDetail subject in dtl.MaterialSubjectDetails)
                    {
                        string dtlConsumeLinkStr = subject.SettleSubject.Id;
                        if (ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                        {
                            CostMonthAccDtlConsume tempDtlConsume =
                                (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                            tempDtlConsume.CurrRealConsumeQuantity += subject.SettleQuantity;
                            tempDtlConsume.CurrRealConsumeTotalPrice += subject.SettleMoney;
                        }
                        else
                        {
                            CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                            dtlConsume.CalType = 0;
                            dtlConsume.CostingSubjectGUID = subject.SettleSubject;
                            dtlConsume.CostingSubjectName = subject.SettleSubjectName;
                            dtlConsume.ResourceTypeGUID = subject.MaterialType.Id;
                            dtlConsume.ResourceTypeName = subject.MaterialTypeName;
                            dtlConsume.ResourceTypeSpec = subject.MaterialTypeSpec;
                            dtlConsume.ResourceTypeStuff = subject.MaterialTypeStuff;
                            dtlConsume.ResourceSyscode = subject.MaterialSysCode;
                            dtlConsume.CurrRealConsumeQuantity = subject.SettleQuantity;
                            dtlConsume.CurrRealConsumeTotalPrice = subject.SettleMoney;
                            dtlConsume.TheAccountDetail = costFirstDtl;
                            ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                        }
                    }
                }
            }

            #endregion

            #region 6：费用结算单的月度汇总

            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.IsNull("MonthlySettlment"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddFetchMode("Details.AccountCostSubject", NHibernate.FetchMode.Eager);
            IList list_expenseConsume = Dao.ObjectQuery(typeof (ExpensesSettleMaster), oq);
            // 汇总费用结算结算
            foreach (ExpensesSettleMaster model in list_expenseConsume)
            {
                foreach (ExpensesSettleDetail subject in model.Details)
                {
                    //string dtlConsumeLinkStr = dtl.ProjectTaskSysCode + "-" + dtl.MaterialResource.Id + "-" + dtl.AccountCostSubject.Id;
                    string dtlConsumeLinkStr = subject.MaterialResource.Id + "-" + subject.AccountCostSubject.Id;
                    if (ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                    {
                        CostMonthAccDtlConsume tempDtlConsume =
                            (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                        tempDtlConsume.CurrRealConsumeQuantity += subject.Quantity;
                        tempDtlConsume.CurrRealConsumeTotalPrice += subject.Money;
                    }
                    else
                    {
                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        dtlConsume.CalType = 0;
                        dtlConsume.CostingSubjectGUID = subject.AccountCostSubject;
                        dtlConsume.CostingSubjectName = subject.AccountCostName;
                        dtlConsume.ResourceTypeGUID = subject.MaterialResource.Id;
                        dtlConsume.ResourceTypeName = subject.MaterialName;
                        dtlConsume.ResourceTypeSpec = subject.MaterialSpec;
                        dtlConsume.ResourceTypeStuff = subject.MaterialStuff;
                        dtlConsume.ResourceSyscode = subject.MaterialSysCode;
                        dtlConsume.CurrRealConsumeQuantity = subject.Quantity;
                        dtlConsume.CurrRealConsumeTotalPrice = subject.Money;
                        dtlConsume.TheAccountDetail = costFirstDtl;
                        ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                    }
                }
            }
            //补充科目编码
            foreach (string dtlStrLink in ht_monthdtlconsume.Keys)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlStrLink];
                string subjectCode = TransUtil.ToString(ht_subject[dtlConsume.CostingSubjectGUID.Id]);
                dtlConsume.CostSubjectCode = subjectCode;
            }

            #endregion

            #region 7：计算实际值和累计值等

            //7.1 更新本月累计值
            foreach (CostMonthAccDtlConsume sonDetail in ht_monthdtlconsume.Values)
            {
                this.CalCostMonthAcctDtlConsumeSumValue(sonDetail, new CostMonthAccDtlConsume());
            }
            foreach (CostMonthAccountDtl detail in ht_monthdtl.Values)
            {
                this.CalCostMonthAcctDtlSumValue(detail, new CostMonthAccountDtl());
            }

            //7.2 查询上期的月度成本核算
            int lastYear = TransUtil.GetLastYear(costBill.Kjn, costBill.Kjy);
            int lastMonth = TransUtil.GetLastMonth(costBill.Kjn, costBill.Kjy);
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheAccountBill.Kjn", lastYear));
            oq.AddCriterion(Expression.Eq("TheAccountBill.Kjy", lastMonth));
            oq.AddCriterion(Expression.Eq("TheAccountBill.ProjectId", costBill.ProjectId));
            oq.AddFetchMode("Details.Details", FetchMode.Eager);
            IList list_lastCostMonthAcc = Dao.ObjectQuery(typeof (CostMonthAccountDtl), oq);
            //IList list_lastCostMonthAcc = new ArrayList();
            Hashtable last_monthdtl = new Hashtable();
            //构造月度核算明细和月度核算资源耗用Hashtable
            foreach (CostMonthAccountDtl detail in list_lastCostMonthAcc)
            {
                foreach (CostMonthAccDtlConsume sonDetail in detail.Details)
                {
//资源ID+成本核算科目ID
                    string linkStr = sonDetail.ResourceTypeGUID + "-" + sonDetail.CostingSubjectGUID.Id;
                    if (!ht_monthdtlconsume.Contains(linkStr)) //找到上月同一[工程任务明细+资源类型+成本科目]
                    {
                        CostMonthAccDtlConsume newDtlConsume = this.TransCostMonthAccDtlConsume(sonDetail);
                        newDtlConsume.TheAccountDetail = costFirstDtl;
                        newDtlConsume.Data3 = "Last";
                        ht_monthdtlconsume.Add(linkStr, newDtlConsume);
                    }
                    else
                    {
                        CostMonthAccDtlConsume currDtlConsume = (CostMonthAccDtlConsume) ht_monthdtlconsume[linkStr];
                        this.CalLastCostMonthAcctDtlConsumeSumValue(currDtlConsume, sonDetail);
                    }
                }
            }

            #endregion

            #region 8：数据库处理

            //8.1 写入月度成本核算信息
            foreach (string str in ht_monthdtlconsume.Keys)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume) ht_monthdtlconsume[str];
                if (dtlConsume.CostingSubjectGUID.Id != null && TransUtil.ToString(dtlConsume.CostSubjectCode) == "")
                {
                    dtlConsume.CostSubjectCode = TransUtil.ToString(ht_subject[dtlConsume.CostingSubjectGUID.Id]);
                }
            }
            //构造耗用明细的HT
            Hashtable ht_dtlid = new Hashtable();
            foreach (CostMonthAccDtlConsume dtlConsume in ht_monthdtlconsume.Values)
            {
                dtlConsume.TheAccountDetail = costFirstDtl;
                costFirstDtl.Details.Add(dtlConsume);
            }
            costFirstDtl.TheAccountBill = costBill;
            costBill.Details.Add(costFirstDtl);

            //重新配价_安装专业(累计值的单价取当前预算体系的价格)
            if (costBill.Temp5 == "1" || 1 == 1)
            {
                IList sumSameList = new ArrayList(); //把相同科目、资源、节点的累计
                IList sumTwoList = new ArrayList(); //把相同科目、节点的累计
                foreach (CostMonthAccountDtl detail in costBill.Details)
                {
                    foreach (CostMonthAccDtlConsume dtlConsume in detail.Details)
                    {
                        string subjectCode = ht_subject[dtlConsume.CostingSubjectGUID.Id] as string;
                        //1：当科目为按金额计算时
                        if (subjectCode != null && subjectCode != "" && ifMoneyCal(subjectCode) == true)
                        {
                            //先清除累计值
                            dtlConsume.SumIncomeQuantity = 0;
                            dtlConsume.SumRealConsumePlanQuantity = 0;
                            dtlConsume.SumResponsiConsumeQuantity = 0;
                            dtlConsume.SumIncomeTotalPrice = 0;
                            dtlConsume.SumRealConsumePlanTotalPrice = 0;
                            dtlConsume.SumResponsiConsumeTotalPrice = 0;

                            decimal sumRealConsumeQty = dtlConsume.SumRealConsumeQuantity;
                            decimal sumRealConsumeMoney = dtlConsume.SumRealConsumeTotalPrice;
                            decimal currRealConsumeQty = dtlConsume.CurrRealConsumeQuantity;
                            decimal currRealConsumeMoney = dtlConsume.CurrRealConsumeTotalPrice;

                            decimal sumcalplanTotalPrice = 0;
                            decimal sumcalcontractQuantity = 0;
                            decimal sumcalplanQuantity = 0;
                            decimal sumcalresponsiQuantity = 0;
                            decimal sumcalcontractTotalPrice = 0;
                            decimal sumcalresponsiTotalPrice = 0;
                            foreach (string subjectID in ht_sumSubject.Keys)
                            {
                                string tempsubjectcode = ht_subject[subjectID] as string;
                                DataDomain dDomain = (DataDomain) (ht_sumSubject[subjectID]);
                                if (tempsubjectcode.Contains(subjectCode) == true ||
                                    subjectCode.Contains(tempsubjectcode) == true)
                                {
                                    sumcalcontractQuantity += TransUtil.ToDecimal(dDomain.Name4);
                                    sumcalplanQuantity += TransUtil.ToDecimal(dDomain.Name5);
                                    sumcalresponsiQuantity += TransUtil.ToDecimal(dDomain.Name6);
                                    sumcalcontractTotalPrice += TransUtil.ToDecimal(dDomain.Name7);
                                    sumcalplanTotalPrice += TransUtil.ToDecimal(dDomain.Name8);
                                    sumcalresponsiTotalPrice += TransUtil.ToDecimal(dDomain.Name9);
                                }
                            }
                            if (sumcalplanTotalPrice > 0)
                            {
                                //累计合同收入等
                                if (sumRealConsumeMoney != 0)
                                {
                                    decimal addProgress = decimal.Round(sumRealConsumeMoney/sumcalplanTotalPrice, 2);
                                    decimal currProgress = decimal.Round(currRealConsumeMoney/sumcalplanTotalPrice, 4);
                                    if (addProgress > 1)
                                    {
                                        addProgress = 1;
                                    }
                                    if (currProgress > 1)
                                    {
                                        currProgress = 1;
                                    }
                                    dtlConsume.SumIncomeQuantity = decimal.Round(sumcalcontractQuantity*addProgress, 4);
                                    dtlConsume.SumIncomeTotalPrice = decimal.Round(
                                        sumcalcontractTotalPrice*addProgress, 2);
                                    dtlConsume.SumRealConsumePlanQuantity = decimal.Round(
                                        sumcalplanQuantity*addProgress, 4);
                                    dtlConsume.SumRealConsumePlanTotalPrice =
                                        decimal.Round(sumcalplanTotalPrice*addProgress, 2);
                                    dtlConsume.SumResponsiConsumeQuantity =
                                        decimal.Round(sumcalresponsiQuantity*addProgress, 4);
                                    dtlConsume.SumResponsiConsumeTotalPrice =
                                        decimal.Round(sumcalresponsiTotalPrice*addProgress, 2);

                                    dtlConsume.CurrIncomeQuantity = decimal.Round(sumcalcontractQuantity*currProgress, 4);
                                    dtlConsume.CurrIncomeTotalPrice =
                                        decimal.Round(sumcalcontractTotalPrice*currProgress, 2);
                                    dtlConsume.CurrRealConsumePlanQuantity =
                                        decimal.Round(sumcalplanQuantity*currProgress, 4);
                                    dtlConsume.CurrRealConsumePlanTotalPrice =
                                        decimal.Round(sumcalplanTotalPrice*currProgress, 2);
                                    dtlConsume.CurrResponsiConsumeQuantity =
                                        decimal.Round(sumcalresponsiQuantity*currProgress, 4);
                                    dtlConsume.CurrResponsiConsumeTotalPrice =
                                        decimal.Round(sumcalresponsiTotalPrice*currProgress, 2);
                                }
                                else
                                {
                                    dtlConsume.SumIncomeQuantity = sumRealConsumeQty;
                                    dtlConsume.SumIncomeTotalPrice = sumRealConsumeMoney;
                                    dtlConsume.SumRealConsumePlanQuantity = sumRealConsumeQty;
                                    dtlConsume.SumRealConsumePlanTotalPrice = sumRealConsumeMoney;
                                    dtlConsume.SumResponsiConsumeQuantity = sumRealConsumeQty;
                                    dtlConsume.SumResponsiConsumeTotalPrice = sumRealConsumeMoney;

                                    dtlConsume.CurrIncomeQuantity = currRealConsumeQty;
                                    dtlConsume.CurrIncomeTotalPrice = currRealConsumeMoney;
                                    dtlConsume.CurrRealConsumePlanQuantity = currRealConsumeQty;
                                    dtlConsume.CurrRealConsumePlanTotalPrice = currRealConsumeMoney;
                                    dtlConsume.CurrResponsiConsumeQuantity = currRealConsumeQty;
                                    dtlConsume.CurrResponsiConsumeTotalPrice = currRealConsumeMoney;
                                }
                            }
                            else
                            {
                                //如果没找到科目的计划金额，则取实际成本
                                dtlConsume.SumIncomeQuantity = sumRealConsumeQty;
                                dtlConsume.SumIncomeTotalPrice = sumRealConsumeMoney;
                                dtlConsume.SumRealConsumePlanQuantity = sumRealConsumeQty;
                                dtlConsume.SumRealConsumePlanTotalPrice = sumRealConsumeMoney;
                                dtlConsume.SumResponsiConsumeQuantity = sumRealConsumeQty;
                                dtlConsume.SumResponsiConsumeTotalPrice = sumRealConsumeMoney;

                                dtlConsume.CurrIncomeQuantity = currRealConsumeQty;
                                dtlConsume.CurrIncomeTotalPrice = currRealConsumeMoney;
                                dtlConsume.CurrRealConsumePlanQuantity = currRealConsumeQty;
                                dtlConsume.CurrRealConsumePlanTotalPrice = currRealConsumeMoney;
                                dtlConsume.CurrResponsiConsumeQuantity = currRealConsumeQty;
                                dtlConsume.CurrResponsiConsumeTotalPrice = currRealConsumeMoney;
                            }
                        }
                    }
                }
            }

            //剔除无效数据
            IList delList = new ArrayList();
            foreach (CostMonthAccountDtl detail in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in detail.Details)
                {
                    if (dtlConsume.CurrRealConsumeTotalPrice == 0 && dtlConsume.CurrIncomeTotalPrice == 0 &&
                        dtlConsume.SumRealConsumeTotalPrice == 0
                        && dtlConsume.SumResponsiConsumeTotalPrice == 0 && dtlConsume.SumRealConsumePlanTotalPrice == 0 &&
                        dtlConsume.SumIncomeTotalPrice == 0)
                    {
                        delList.Add(dtlConsume);
                    }
                }
            }
            foreach (CostMonthAccDtlConsume dtlConsume in delList)
            {
                foreach (CostMonthAccountDtl detail in costBill.Details)
                {
                    detail.Details.Remove(dtlConsume);
                }
            }

            //保存方法
            Hashtable ht_forwardbill = new Hashtable();
            ht_forwardbill.Add("0", list_PTaskAccount);
            ht_forwardbill.Add("1", list_SubBalance);
            ht_forwardbill.Add("2", list_resourceConsume);
            ht_forwardbill.Add("3", list_machineConsume);
            ht_forwardbill.Add("5", list_expenseConsume);
            this.InsertCostMonthAccountNew(costBill, ht_forwardbill);

            #endregion

            return "";
        }
        /// <summary>
        /// 取累计合同收入、计划成本、责任成本
        /// </summary>
        private List<CostMonthAccDtlConsume> GetCostMonthAccountDetailFromGwbsTree(CostMonthAccountBill costBill,
                                                                                   Hashtable ht_subject)
        {
            if (costBill == null || ht_subject == null)
            {
                return null;
            }

            IList<MachineCostParame> lstMcp = GetMachineCostParameList(costBill.ProjectId).OfType<MachineCostParame>().ToList<MachineCostParame>();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", costBill.ProjectId));
            oq.AddCriterion(Expression.Eq("CostAccFlag", true));
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails", FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails.CostingSubjectGUID", FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails.ListGWBSDtlCostSubRate", FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails.SelFeelDtl", FetchMode.Eager);
            
            IList gwbsList = Dao.ObjectQuery(typeof (GWBSTree), oq);
            if (gwbsList == null)
            {
                return null;
            }

            var retList = new List<CostMonthAccDtlConsume>();
            var costList = new List<GWBSDetailCostSubject>();
            foreach (GWBSTree treeNode in gwbsList)
            {
                foreach (GWBSDetail gwbsDetail in treeNode.Details)
                {
                    decimal progress = gwbsDetail.AddupAccFigureProgress;
                    //if (progress<=0)
                    //{
                    //    continue;
                    //}
                    //else if (progress > 100)
                    //{
                    //    progress = 100;
                    //}
                    if (progress <= 0)
                        progress = 0;
                    else if (progress > 100)
                        progress = 100;

                    foreach (GWBSDetailCostSubject sonDetail in gwbsDetail.ListCostSubjectDetails)
                    {
                        string subjectCode = ht_subject[sonDetail.CostAccountSubjectGUID.Id] as string;
                        if (string.IsNullOrEmpty(subjectCode) || ifMoneyCal(subjectCode))
                        {
                            continue;
                        }
                        costList.Add(sonDetail);
                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        dtlConsume.AccountTaskNodeGUID = treeNode.Id;
                        dtlConsume.AccountTaskNodeName = treeNode.Name;
                        dtlConsume.AccountTaskNodeSyscode = treeNode.SysCode;
                        dtlConsume.CostingSubjectName = sonDetail.CostAccountSubjectName;
                        dtlConsume.CostSubjectCode = sonDetail.CostAccountSubjectGUID.Code;
                        dtlConsume.CostingSubjectGUID = sonDetail.CostAccountSubjectGUID;
                        dtlConsume.CostSubjectSyscode = sonDetail.CostAccountSubjectGUID.SysCode;
                        dtlConsume.CalType = 0;
                        dtlConsume.DiagramNumber = gwbsDetail.DiagramNumber;
                        dtlConsume.ProjectTaskDtlGUID = gwbsDetail.Id;
                        dtlConsume.ProjectTaskDtlName = gwbsDetail.Name;
                        dtlConsume.ResourceTypeGUID = sonDetail.ResourceTypeGUID;
                        dtlConsume.ResourceTypeName = sonDetail.ResourceTypeName;
                        dtlConsume.ResourceSyscode = sonDetail.ResourceCateSyscode;
                        dtlConsume.ResourceTypeCode = sonDetail.ResourceTypeCode;
                        dtlConsume.ResourceTypeSpec = sonDetail.ResourceTypeSpec;
                        dtlConsume.RationUnitGUID = sonDetail.ProjectAmountUnitGUID;
                        dtlConsume.RationUnitName = sonDetail.ProjectAmountUnitName;
                        dtlConsume.SourceType = sonDetail.GetType().FullName;
                        dtlConsume.SourceId = sonDetail.Id;

                        dtlConsume.CurrIncomePrice = sonDetail.ContractQuantityPrice;
                        dtlConsume.CurrRealConsumePlanPrice = sonDetail.PlanPrice;
                        dtlConsume.CurrResponsiConsumePrice = sonDetail.ResponsibilitilyPrice;
                        dtlConsume.Data1 = sonDetail.PlanTotalPrice + ""; //计划耗用合价
                        if (IsCalculateByAreaParam(subjectCode,1))
                        {
                            decimal rate = ClientUtil.ToDecimal(costBill.TempCalculateArea / costBill.TempProjectArea);
                            dtlConsume.SumIncomeQuantity = decimal.Round(sonDetail.ContractProjectAmount * rate, 4);
                            dtlConsume.SumIncomeTotalPrice = decimal.Round(sonDetail.ContractTotalPrice * rate, 2);
                        }
                        else
                        {
                            dtlConsume.SumIncomeQuantity = decimal.Round(sonDetail.ContractProjectAmount * progress / 100, 4);
                            dtlConsume.SumIncomeTotalPrice = decimal.Round(sonDetail.ContractTotalPrice * progress / 100, 2);
                        }
                       
                        dtlConsume.SumRealConsumePlanQuantity = decimal.Round(sonDetail.PlanWorkAmount*progress/100, 4);
                        dtlConsume.SumRealConsumePlanTotalPrice = decimal.Round(sonDetail.PlanTotalPrice*progress/100, 2);
                        if(IsCalculateByAreaParam(subjectCode,2))
                        {
                            decimal rate = ClientUtil.ToDecimal(costBill.TempCalculateArea / costBill.TempProjectArea);
                            dtlConsume.SumResponsiConsumeQuantity = decimal.Round(sonDetail.ResponsibilitilyWorkAmount * rate, 4);
                            dtlConsume.SumResponsiConsumeTotalPrice = decimal.Round(sonDetail.ResponsibilitilyTotalPrice * rate, 2);
                        }
                        else if(IsCalculateByDateParam(subjectCode,2))
                        {
                            decimal rate = GetDateParame(lstMcp,subjectCode);
                            dtlConsume.SumResponsiConsumeQuantity = decimal.Round(sonDetail.ResponsibilitilyWorkAmount * rate, 4);
                            dtlConsume.SumResponsiConsumeTotalPrice = decimal.Round(sonDetail.ResponsibilitilyTotalPrice * rate, 2);
                        }
                        else if (IsCalculateByFixedRate(subjectCode, 2))
                        {
                            decimal rate = GetFixedRateParam(costBill, subjectCode);
                            dtlConsume.SumResponsiConsumeQuantity = decimal.Round(sonDetail.ContractProjectAmount * rate, 4);
                            dtlConsume.SumResponsiConsumeTotalPrice = decimal.Round(sonDetail.ContractTotalPrice * rate, 2);
                        }
                        else
                        {
                            dtlConsume.SumResponsiConsumeQuantity = decimal.Round(sonDetail.ResponsibilitilyWorkAmount * progress / 100, 4);
                            dtlConsume.SumResponsiConsumeTotalPrice = decimal.Round(sonDetail.ResponsibilitilyTotalPrice * progress / 100, 2);
                        }



                        retList.Add(dtlConsume);
                    }
                }
            }

            //重算非实体合同收入
            var nonEntityList = ComputeNonentity(costList, costBill.ProjectId);
            if (nonEntityList != null)
            {
                retList.AddRange(nonEntityList);
            }

            return retList;
        }

        /// <summary>
        /// 从核算单取当期合同收入、实际成本、责任成本(以确认形象进度计算的，作废)
        /// </summary>
        private List<CostMonthAccDtlConsume> GetCostMonthAccountDetailFromTaskAccount(CostMonthAccountBill costBill,
                                                                                      Hashtable ht_forwardbill)
        {
            if (costBill == null)
            {
                return null;
            }

            var oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime.AddDays(1)));
            oq.AddCriterion(Expression.IsNull("MonthAccountBill"));
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("Details.Details", FetchMode.Eager);
            IList list_PTaskAccount = Dao.ObjectQuery(typeof (ProjectTaskAccountBill), oq);
            if (list_PTaskAccount == null)
            {
                return null;
            }

            ht_forwardbill.Add("0", list_PTaskAccount);
            var retList = new List<CostMonthAccDtlConsume>();
            foreach (ProjectTaskAccountBill model in list_PTaskAccount)
            {
                //工程任务核算明细信息
                foreach (ProjectTaskDetailAccount dtl in model.Details)
                {
                    //工程任务资源耗用信息
                    foreach (ProjectTaskDetailAccountSubject sonDetail in dtl.Details)
                    {
                        if (sonDetail.CostingSubjectGUID == null || ifMoneyCal(sonDetail.CostingSubjectGUID.Code))
                            continue;

                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        dtlConsume.AccountTaskNodeGUID = dtl.AccountTaskNodeGUID.Id;
                        dtlConsume.AccountTaskNodeName = dtl.AccountTaskNodeGUID.Name;
                        dtlConsume.AccountTaskNodeSyscode = dtl.AccountTaskNodeGUID.SysCode;
                        dtlConsume.CostingSubjectName = sonDetail.CostingSubjectGUID.Name;
                        dtlConsume.CostSubjectCode = sonDetail.CostingSubjectGUID.Code;
                        dtlConsume.CostingSubjectGUID = sonDetail.CostingSubjectGUID;
                        dtlConsume.CostSubjectSyscode = sonDetail.CostingSubjectGUID.SysCode;
                        dtlConsume.CalType = 0;
                        dtlConsume.DiagramNumber = dtl.ProjectTaskDtlGUID.DiagramNumber;
                        dtlConsume.ProjectTaskDtlGUID = dtl.ProjectTaskDtlGUID.Id;
                        dtlConsume.ProjectTaskDtlName = dtl.ProjectTaskDtlGUID.Name;
                        dtlConsume.ResourceTypeGUID = sonDetail.ResourceTypeGUID;
                        dtlConsume.ResourceTypeName = sonDetail.ResourceTypeName;
                        dtlConsume.ResourceTypeSpec = sonDetail.ResourceTypeSpec;
                        dtlConsume.ResourceTypeStuff = sonDetail.ResourceTypeQuality;
                        dtlConsume.ResourceSyscode = sonDetail.ResourceCategorySysCode;
                        dtlConsume.RationUnitGUID = sonDetail.QuantityUnitGUID;
                        dtlConsume.RationUnitName = sonDetail.QuantityUnitName;
                        dtlConsume.SourceType = sonDetail.GetType().FullName;
                        dtlConsume.SourceId = sonDetail.Id;

                        dtlConsume.CurrIncomeQuantity = sonDetail.CurrContractIncomeQny;
                        dtlConsume.CurrIncomeTotalPrice = sonDetail.CurrContractIncomeTotal;
                        dtlConsume.CurrRealConsumePlanQuantity = sonDetail.AccUsageQny;
                        dtlConsume.CurrRealConsumePlanTotalPrice = sonDetail.AccountTotalPrice;
                        dtlConsume.CurrResponsiConsumeQuantity = sonDetail.CurrResponsibleCostQny;
                        dtlConsume.CurrResponsiConsumeTotalPrice = sonDetail.CurrResponsibleCostTotal;

                        retList.Add(dtlConsume);
                    }
                }
            }
            return retList;
        }
        /// <summary>
        /// 从分包结算单取当期实际成本
        /// </summary>
        private List<CostMonthAccDtlConsume> GetCostMonthAccountDetailFromSubContract(CostMonthAccountBill costBill,
                                                                                      Hashtable ht_forwardbill)
        {
            if (costBill == null)
            {
                return null;
            }

            Hashtable ht = GetSubBalBasicData();
            var oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.IsNull("MonthAccBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_SubBalance = Dao.ObjectQuery(typeof (SubContractBalanceBill), oq);
            if (list_SubBalance == null)
            {
                return null;
            }

            ht_forwardbill.Add("1", list_SubBalance);
            var retList = new List<CostMonthAccDtlConsume>();
            foreach (SubContractBalanceBill model in list_SubBalance)
            {
                foreach (SubContractBalanceDetail dtl in model.Details)
                {
                    

                    foreach (SubContractBalanceSubjectDtl sonDetail in dtl.Details)
                    {
                        if (sonDetail.BalanceSubjectGUID == null)
                            continue;

                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        dtlConsume.AccountTaskNodeGUID = dtl.BalanceTask.Id;
                        dtlConsume.AccountTaskNodeName = dtl.BalanceTask.Name;
                        dtlConsume.AccountTaskNodeSyscode = dtl.BalanceTask.SysCode;
                        dtlConsume.CostingSubjectName = sonDetail.BalanceSubjectGUID.Name;
                        if (dtl.FontBillType == FrontBillType.建管)
                        {
                            CostAccountSubject fee_jg = ht["C514"] as CostAccountSubject;
                            dtlConsume.CostSubjectCode = fee_jg.Code;
                            dtlConsume.CostingSubjectGUID =fee_jg;
                            dtlConsume.CostSubjectSyscode =fee_jg.SysCode;
                        }
                        //else if (dtl.FontBillType == FrontBillType.水电)
                        //{
                        //    dtlConsume.CostSubjectCode = sonDetail.BalanceSubjectGUID.Code;
                        //    dtlConsume.CostingSubjectGUID = sonDetail.BalanceSubjectGUID;
                        //    dtlConsume.CostSubjectSyscode = sonDetail.BalanceSubjectGUID.SysCode;
                        //}
                        else
                        {
                            dtlConsume.CostSubjectCode = sonDetail.BalanceSubjectGUID.Code;
                            dtlConsume.CostingSubjectGUID = sonDetail.BalanceSubjectGUID;
                            dtlConsume.CostSubjectSyscode = sonDetail.BalanceSubjectGUID.SysCode;
                        }

                        
                        dtlConsume.CalType = 0;
                        if (dtl.BalanceTaskDtl != null)
                        {
                            dtlConsume.DiagramNumber = dtl.BalanceTaskDtl.DiagramNumber;
                            dtlConsume.ProjectTaskDtlGUID = dtl.BalanceTaskDtl.Id;
                            dtlConsume.ProjectTaskDtlName = dtl.BalanceTaskDtl.Name;
                        }
                        dtlConsume.ResourceTypeGUID = sonDetail.ResourceTypeGUID;
                        dtlConsume.ResourceTypeName = sonDetail.ResourceTypeName;
                        dtlConsume.ResourceTypeSpec = sonDetail.ResourceTypeSpec;
                        dtlConsume.ResourceTypeStuff = sonDetail.ResourceTypeStuff;
                        dtlConsume.ResourceSyscode = sonDetail.ResourceSyscode;
                        dtlConsume.RationUnitGUID = sonDetail.QuantityUnit;
                        dtlConsume.RationUnitName = sonDetail.QuantityUnitName;
                        dtlConsume.SourceType = sonDetail.GetType().FullName;
                        dtlConsume.SourceId = sonDetail.Id;

                        dtlConsume.CurrRealConsumeQuantity = sonDetail.BalanceQuantity;
                        dtlConsume.CurrRealConsumeTotalPrice = sonDetail.BalanceTotalPrice;
                        dtlConsume.CurrRealConsumePrice = sonDetail.BalancePrice;

                        retList.Add(dtlConsume);
                    }

                }
            }

            return retList;
        }
        /// <summary>
        /// 从物资耗用结算单取当期实际成本
        /// </summary>
        private List<CostMonthAccDtlConsume> GetCostMonthAccountDetailFromMaterialSettle(CostMonthAccountBill costBill,
                                                                                         Hashtable ht_forwardbill)
        {
            if (costBill == null)
            {
                return null;
            }

            var oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.IsNull("MonthAccountBill"));
            oq.AddFetchMode("Details", FetchMode.Eager);
            IList list_resourceConsume = Dao.ObjectQuery(typeof (MaterialSettleMaster), oq);
            if (list_resourceConsume == null)
            {
                return null;
            }

            ht_forwardbill.Add("2", list_resourceConsume);
            var retList = new List<CostMonthAccDtlConsume>();
            foreach (MaterialSettleMaster model in list_resourceConsume)
            {
                foreach (MaterialSettleDetail sonDetail in model.Details)
                {
                    if (sonDetail.MaterialResource == null)
                    {
                        throw new Exception("物资耗用结算中存在物资为空的情况！");
                    }
                    if (sonDetail.AccountCostSubject == null)
                    {
                        throw new Exception("物资耗用结算中存在核算科目为空的情况！");
                    }

                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    dtlConsume.AccountTaskNodeGUID = sonDetail.ProjectTask.Id;
                    dtlConsume.AccountTaskNodeName = sonDetail.ProjectTask.Name;
                    dtlConsume.AccountTaskNodeSyscode = sonDetail.ProjectTask.SysCode;
                    dtlConsume.CostingSubjectName = sonDetail.AccountCostSubject.Name;
                    dtlConsume.CostSubjectCode = sonDetail.AccountCostSubject.Code;
                    dtlConsume.CostingSubjectGUID = sonDetail.AccountCostSubject;
                    dtlConsume.CostSubjectSyscode = sonDetail.AccountCostSubject.SysCode;
                    dtlConsume.CalType = 0;
                    dtlConsume.DiagramNumber = string.Empty;
                    dtlConsume.ProjectTaskDtlGUID = string.Empty;
                    dtlConsume.ProjectTaskDtlName = string.Empty;
                    dtlConsume.ResourceTypeGUID = sonDetail.MaterialResource.Id;
                    dtlConsume.ResourceTypeName = sonDetail.MaterialName;
                    dtlConsume.ResourceTypeSpec = sonDetail.MaterialSpec;
                    dtlConsume.ResourceTypeStuff = sonDetail.MaterialStuff;
                    dtlConsume.ResourceSyscode = sonDetail.MaterialSysCode;
                    dtlConsume.RationUnitGUID = sonDetail.QuantityUnit;
                    dtlConsume.RationUnitName = sonDetail.QuantityUnitName;
                    dtlConsume.SourceType = sonDetail.GetType().FullName;
                    dtlConsume.SourceId = sonDetail.Id;

                    dtlConsume.CurrRealConsumeQuantity = sonDetail.Quantity;
                    dtlConsume.CurrRealConsumeTotalPrice = sonDetail.Money;
                    dtlConsume.CurrRealConsumePrice = sonDetail.Price;

                    retList.Add(dtlConsume);
                }
            }
            return retList;
        }

        /// <summary>
        /// 从设备租赁结算单取当期实际成本
        /// </summary>
        private List<CostMonthAccDtlConsume> GetCostMonthAccountDetailFromMaterialRental(CostMonthAccountBill costBill,
                                                                                         Hashtable ht_forwardbill)
        {
            if (costBill == null)
            {
                return null;
            }

            var oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.IsNull("MonthAccountBillId"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialSubjectDetails", NHibernate.FetchMode.Eager);
            IList list_machineConsume = Dao.ObjectQuery(typeof (MaterialRentalSettlementMaster), oq);
            if (list_machineConsume == null)
            {
                return null;
            }

            ht_forwardbill.Add("3", list_machineConsume);
            var retList = new List<CostMonthAccDtlConsume>();
            foreach (MaterialRentalSettlementMaster model in list_machineConsume)
            {
                foreach (MaterialRentalSettlementDetail dtl in model.Details)
                {
                    foreach (MaterialSubjectDetail sonDetail in dtl.MaterialSubjectDetails)
                    {
                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        if (sonDetail.UsedPart != null)
                        {
                            dtlConsume.AccountTaskNodeGUID = sonDetail.UsedPart.Id;
                            dtlConsume.AccountTaskNodeName = sonDetail.UsedPart.Name;
                            dtlConsume.AccountTaskNodeSyscode = sonDetail.UsedPart.SysCode;
                        }
                        dtlConsume.CostingSubjectName = sonDetail.SettleSubject.Name;
                        dtlConsume.CostSubjectCode = sonDetail.SettleSubject.Code;
                        dtlConsume.CostingSubjectGUID = sonDetail.SettleSubject;
                        dtlConsume.CostSubjectSyscode = sonDetail.SettleSubject.SysCode;
                        dtlConsume.CalType = 0;
                        dtlConsume.DiagramNumber = string.Empty;
                        dtlConsume.ProjectTaskDtlGUID = string.Empty;
                        dtlConsume.ProjectTaskDtlName = string.Empty;
                        if (sonDetail.MaterialResource != null)
                        {
                            dtlConsume.ResourceTypeGUID = sonDetail.MaterialResource.Id;
                        }
                        dtlConsume.ResourceTypeName = sonDetail.MaterialName;
                        dtlConsume.ResourceTypeSpec = sonDetail.MaterialSpec;
                        dtlConsume.ResourceTypeStuff = sonDetail.MaterialStuff;
                        dtlConsume.ResourceSyscode = sonDetail.MaterialSysCode;
                        dtlConsume.RationUnitGUID = sonDetail.QuantityUnit;
                        dtlConsume.RationUnitName = sonDetail.QuantityUnitName;
                        dtlConsume.SourceType = sonDetail.GetType().FullName;
                        dtlConsume.SourceId = sonDetail.Id;

                        dtlConsume.CurrRealConsumeQuantity = sonDetail.SettleQuantity;
                        dtlConsume.CurrRealConsumeTotalPrice = sonDetail.SettleMoney;

                        retList.Add(dtlConsume);
                    }
                }
            }
            return retList;
        }

        /// <summary>
        /// 从财务费用结算单取当期实际成本
        /// </summary>
        private List<CostMonthAccDtlConsume> GetCostMonthAccountDetailFromExpenses(CostMonthAccountBill costBill,
                                                                                   Hashtable ht_forwardbill)
        {
            if (costBill == null)
            {
                return null;
            }

            var oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.IsNull("MonthlySettlment"));
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("Details.AccountCostSubject", FetchMode.Eager);
            IList list_expenseConsume = Dao.ObjectQuery(typeof (ExpensesSettleMaster), oq);
            if (list_expenseConsume == null)
            {
                return null;
            }

            ht_forwardbill.Add("5", list_expenseConsume);
            var retList = new List<CostMonthAccDtlConsume>();
            foreach (ExpensesSettleMaster model in list_expenseConsume)
            {
                foreach (ExpensesSettleDetail sonDetail in model.Details)
                {
                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    if (sonDetail.UsedPart != null)
                    {
                        dtlConsume.AccountTaskNodeGUID = sonDetail.UsedPart.Id;
                        dtlConsume.AccountTaskNodeName = sonDetail.UsedPart.Name;
                        dtlConsume.AccountTaskNodeSyscode = sonDetail.UsedPart.SysCode;
                    }
                    dtlConsume.CostingSubjectName = sonDetail.AccountCostSubject.Name;
                    dtlConsume.CostSubjectCode = sonDetail.AccountCostSubject.Code;
                    dtlConsume.CostingSubjectGUID = sonDetail.AccountCostSubject;
                    dtlConsume.CostSubjectSyscode = sonDetail.AccountCostSubject.SysCode;
                    dtlConsume.CalType = 0;
                    dtlConsume.DiagramNumber = string.Empty;
                    dtlConsume.ProjectTaskDtlGUID = string.Empty;
                    dtlConsume.ProjectTaskDtlName = string.Empty;
                    dtlConsume.ResourceTypeGUID = sonDetail.MaterialResource.Id;
                    dtlConsume.ResourceTypeName = sonDetail.MaterialName;
                    dtlConsume.ResourceTypeSpec = sonDetail.MaterialSpec;
                    dtlConsume.ResourceTypeStuff = sonDetail.MaterialStuff;
                    dtlConsume.ResourceSyscode = sonDetail.MaterialSysCode;
                    dtlConsume.RationUnitGUID = sonDetail.QuantityUnit;
                    dtlConsume.RationUnitName = sonDetail.QuantityUnitName;
                    dtlConsume.SourceType = sonDetail.GetType().FullName;
                    dtlConsume.SourceId = sonDetail.Id;

                    dtlConsume.CurrRealConsumeQuantity = sonDetail.Quantity;
                    dtlConsume.CurrRealConsumeTotalPrice = sonDetail.Money;

                    retList.Add(dtlConsume);
                }
            }

            return retList;
        }

        /// <summary>
        /// 从计时派工结算单取当期实际成本
        /// </summary>
        private List<CostMonthAccDtlConsume> GetCostMonthAccountDetailFromLabor(CostMonthAccountBill costBill,
                                                                                Hashtable ht_forwardbill)
        {
            if (costBill == null)
            {
                return null;
            }

            var oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.Eq("LaborState", "计时派工"));
            oq.AddCriterion(Expression.IsNull("MonthlySettlment"));
            oq.AddCriterion(Expression.IsNull("PenaltyDeductionMaster"));
            oq.AddFetchMode("Details", FetchMode.Eager);
            IList list_laborConsume = Dao.ObjectQuery(typeof (LaborSporadicMaster), oq);
            if (list_laborConsume == null)
            {
                return null;
            }

            ht_forwardbill.Add("6", list_laborConsume);
            var retList = new List<CostMonthAccDtlConsume>();
            foreach (LaborSporadicMaster model in list_laborConsume)
            {
                foreach (LaborSporadicDetail sonDetail in model.Details)
                {
                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    dtlConsume.AccountTaskNodeGUID = sonDetail.ProjectTast.Id;
                    dtlConsume.AccountTaskNodeName = sonDetail.ProjectTast.Name;
                    dtlConsume.AccountTaskNodeSyscode = sonDetail.ProjectTast.SysCode;
                    dtlConsume.CostingSubjectName = sonDetail.LaborSubject.Name;
                    dtlConsume.CostSubjectCode = sonDetail.LaborSubject.Code;
                    dtlConsume.CostingSubjectGUID = sonDetail.LaborSubject;
                    dtlConsume.CostSubjectSyscode = sonDetail.LaborSubject.SysCode;
                    dtlConsume.CalType = 0;
                    dtlConsume.DiagramNumber = sonDetail.ProjectTastDetail.DiagramNumber;
                    dtlConsume.ProjectTaskDtlGUID = sonDetail.ProjectTastDetail.Id;
                    dtlConsume.ProjectTaskDtlName = sonDetail.ProjectTastDetail.Name;
                    dtlConsume.ResourceTypeGUID = sonDetail.ResourceType.Id;
                    dtlConsume.ResourceTypeName = sonDetail.ResourceTypeName;
                    dtlConsume.ResourceTypeSpec = sonDetail.ResourceTypeSpec;
                    dtlConsume.ResourceTypeStuff = sonDetail.ResourceTypeStuff;
                    dtlConsume.ResourceSyscode = sonDetail.ResourceSysCode;
                    dtlConsume.RationUnitGUID = sonDetail.QuantityUnit;
                    dtlConsume.RationUnitName = sonDetail.QuantityUnitName;
                    dtlConsume.SourceType = sonDetail.GetType().FullName;
                    dtlConsume.SourceId = sonDetail.Id;

                    dtlConsume.CurrRealConsumeQuantity = sonDetail.AccountLaborNum;
                    dtlConsume.CurrRealConsumeTotalPrice = sonDetail.AccountSumMoney;

                    retList.Add(dtlConsume);
                }
            }
            return retList;
        }

        private CostMonthAccDtlConsume CreateNewCostMonthAccDtlConsume(GWBSDetailCostSubject sonDetail)
        {
            var dtlConsume = new CostMonthAccDtlConsume();
            dtlConsume.AccountTaskNodeGUID = sonDetail.TheGWBSTree.Id;
            dtlConsume.AccountTaskNodeName = sonDetail.TheGWBSTree.Name;
            dtlConsume.AccountTaskNodeSyscode = sonDetail.TheGWBSTree.SysCode;
            dtlConsume.CalType = 0;
            dtlConsume.DiagramNumber = sonDetail.TheGWBSDetail.DiagramNumber;
            dtlConsume.ProjectTaskDtlGUID = sonDetail.TheGWBSDetail.Id;
            dtlConsume.ProjectTaskDtlName = sonDetail.TheGWBSDetail.Name;
            dtlConsume.ResourceTypeGUID = sonDetail.ResourceTypeGUID;
            dtlConsume.ResourceTypeName = sonDetail.ResourceTypeName;
            dtlConsume.ResourceSyscode = sonDetail.ResourceCateSyscode;
            dtlConsume.ResourceTypeCode = sonDetail.ResourceTypeCode;
            dtlConsume.ResourceTypeSpec = sonDetail.ResourceTypeSpec;
            dtlConsume.RationUnitGUID = sonDetail.ProjectAmountUnitGUID;
            dtlConsume.RationUnitName = sonDetail.ProjectAmountUnitName;
            dtlConsume.SourceType = sonDetail.GetType().FullName;
            dtlConsume.SourceId = sonDetail.Id;

            decimal progress = sonDetail.TheGWBSDetail.AddupAccFigureProgress;
            if (progress > 100)
            {
                progress = 100;
            }
            dtlConsume.SumIncomeQuantity = Math.Round(sonDetail.ContractProjectAmount * progress / 100, 4);

            return dtlConsume;
        }

        private void ReplaceFormulaExpression(CostMonthAccDtlConsume deskConsume, List<CostMonthAccDtlConsume> srcList)
        {
            var startIndex = deskConsume.Data2.IndexOf('[');
            if (startIndex < 0)
            {
                return;
            }

            var endIndex = deskConsume.Data2.IndexOf(']');
            if (endIndex < 0 || endIndex < startIndex)
            {
                return;
            }

            var exp = deskConsume.Data2.Substring(startIndex + 1, endIndex - startIndex - 1);
            var srcs = srcList.FindAll(s => s.CostSubjectCode.StartsWith(exp));
            if(srcs.Exists(e => e.Data2.IndexOf('[') >= 0 || e.Data2.IndexOf(']') >= 0))
            {
                return;
            }

            deskConsume.Data2 = deskConsume.Data2.Replace("[" + exp + "]", srcs.Sum(s => s.CurrIncomePrice).ToString());
            if (deskConsume.Data2.IndexOf('[') < 0 && deskConsume.Data2.IndexOf(']') < 0)
            {
                deskConsume.CurrIncomePrice =
                    TransUtil.ToDecimal(deskConsume.Data1) * TransUtil.ToDecimal(new DataTable().Compute(deskConsume.Data2, ""));
                deskConsume.SumIncomeTotalPrice = deskConsume.SumIncomeQuantity * deskConsume.CurrIncomePrice;
            }

            ReplaceFormulaExpression(deskConsume, srcList);
        }

        /// <summary>
        /// 计算非实体合同收入
        /// </summary>
        private List<CostMonthAccDtlConsume> ComputeNonentity(List<GWBSDetailCostSubject> costSubjects, string projId)
        {
            if (costSubjects == null || costSubjects.Count == 0 || string.IsNullOrEmpty(projId))
            {
                return null;
            }

            var entityLists = costSubjects.FindAll(s => !string.IsNullOrEmpty(s.ProfessionalType));
            if (entityLists.Count == 0)
            {
                return null;
            }

            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("Id", projId));
            objQuery.AddFetchMode("SelFeeDetails", FetchMode.Eager);
            objQuery.AddFetchMode("SelFeeFormulas", FetchMode.Eager);

            var list = Dao.ObjectQuery(typeof(CurrentProjectInfo), objQuery).OfType<CurrentProjectInfo>();
            if (list.Count() == 0)
            {
                return null;
            }

            var proj = list.ElementAt(0);
            var selFeeDetails = proj.SelFeeDetails.OfType<SelFeeDtl>().ToList();
            var selFeeFormulas = proj.SelFeeFormulas.OfType<SelFeeFormula>().ToList();

            var dt = new DataTable();
            var nonEntityList = new List<CostMonthAccDtlConsume>();
            foreach (var enty in entityLists)
            {
                var fDetails = selFeeDetails.FindAll(s => s.SpecialType == enty.ProfessionalType);
                var entFDetails = enty.ListGWBSDtlCostSubRate.OfType<GWBSDtlCostSubRate>().ToList();
                foreach (var selFeeDtl in fDetails)
                {
                    var fFormula = selFeeFormulas.Find(f => f.AccountSubjectCode == selFeeDtl.MainAccSubjectCode);
                    if (fFormula == null)
                    {
                        continue;
                    }
                    var fNewRate = entFDetails.Find(e => e.SelFeelDtl == selFeeDtl);
                    var rate = (fNewRate != null ? fNewRate.Rate : selFeeDtl.Rate) / 100m;

                    var dtlConsume = CreateNewCostMonthAccDtlConsume(enty);
                    var subj = selFeeDtl.AccountSubject ?? selFeeDtl.MainAccSubject;
                    dtlConsume.CostingSubjectName = subj.Name;
                    dtlConsume.CostSubjectCode = subj.Code;
                    dtlConsume.CostingSubjectGUID = subj;
                    dtlConsume.CostSubjectSyscode = subj.SysCode;
                    dtlConsume.Data1 = rate.ToString();
                    dtlConsume.Data2 = fFormula.FormulaCode.Replace("[人工+机械]", TransUtil.ToString(enty.LaborMachineBasePrice))
                        .Replace("[人工+机械+材料]", TransUtil.ToString(enty.LaborMachineBasePrice + enty.MaterialBasePrice));
                    dtlConsume.Data3 = enty.Id;

                    if (dtlConsume.Data2.IndexOf('[') < 0 && dtlConsume.Data2.IndexOf(']') < 0)
                    {
                        dtlConsume.CurrIncomePrice =
                            TransUtil.ToDecimal(dtlConsume.Data1) * TransUtil.ToDecimal(dt.Compute(dtlConsume.Data2, ""));
                        dtlConsume.SumIncomeTotalPrice = dtlConsume.SumIncomeQuantity * dtlConsume.CurrIncomePrice;
                    }

                    nonEntityList.Add(dtlConsume);
                }
            }

            var unComputeList = nonEntityList;
            while (unComputeList.Count > 0)
            {
                foreach (var dtlConsume in unComputeList)
                {
                    if (dtlConsume.Data2.IndexOf('[') < 0 && dtlConsume.Data2.IndexOf(']') < 0)
                    {
                        dtlConsume.CurrIncomePrice = 
                            TransUtil.ToDecimal(dtlConsume.Data1)* TransUtil.ToDecimal(dt.Compute(dtlConsume.Data2, ""));
                        dtlConsume.SumIncomeTotalPrice = dtlConsume.SumIncomeQuantity*dtlConsume.CurrIncomePrice;
                    }
                    else
                    {
                        var relaLst = nonEntityList.FindAll(e => e.Data3 == dtlConsume.Data3);
                        ReplaceFormulaExpression(dtlConsume, relaLst);
                    }
                }

                unComputeList = nonEntityList.FindAll(e => e.Data2.IndexOf('[') >= 0 || e.Data2.IndexOf(']') >= 0);
            }

            return nonEntityList;
        }

        [TransManager]
        private bool CreateCostMonthAccount(CostMonthAccountBill costBill)
        {
            Hashtable ht_subject = GetCostSubjectList(true);
            var sumeList = GetCostMonthAccountDetailFromGwbsTree(costBill, ht_subject);
            if (sumeList == null)
            {
                sumeList = new List<CostMonthAccDtlConsume>();
            }

            Hashtable ht_forwardbill = new Hashtable();
            //var tmpList = GetCostMonthAccountDetailFromTaskAccount(costBill, ht_forwardbill);
            //if (tmpList != null && tmpList.Count > 0)
            //{
            //    sumeList.AddRange(tmpList);
            //}

            var tmpList = GetCostMonthAccountDetailFromSubContract(costBill, ht_forwardbill);
            if (tmpList != null && tmpList.Count > 0)
            {
                sumeList.AddRange(tmpList);
            }

            tmpList = GetCostMonthAccountDetailFromMaterialSettle(costBill, ht_forwardbill);
            if (tmpList != null && tmpList.Count > 0)
            {
                sumeList.AddRange(tmpList);
            }

            tmpList = GetCostMonthAccountDetailFromMaterialRental(costBill, ht_forwardbill);
            if (tmpList != null && tmpList.Count > 0)
            {
                sumeList.AddRange(tmpList);
            }

            tmpList = GetCostMonthAccountDetailFromExpenses(costBill, ht_forwardbill);
            if (tmpList != null && tmpList.Count > 0)
            {
                sumeList.AddRange(tmpList);
            }

            //tmpList = GetCostMonthAccountDetailFromLabor(costBill, ht_forwardbill);
            //if (tmpList != null && tmpList.Count > 0)
            //{
            //    sumeList.AddRange(tmpList);
            //}

            var emptyList = sumeList.FindAll(a => string.IsNullOrEmpty(a.CostSubjectCode));
            foreach (var sume in emptyList)
            {
                if (ht_subject.ContainsKey(sume.CostingSubjectGUID.Id))
                {
                    sume.CostSubjectCode = TransUtil.ToString(ht_subject[sume.CostingSubjectGUID.Id]);
                }
            }

            var costDetail = new CostMonthAccountDtl();
            costDetail.CurrIncomeQuantity = sumeList.Sum(a => a.CurrIncomeQuantity);
            costDetail.CurrIncomeTotalPrice = sumeList.Sum(a => a.CurrIncomeTotalPrice);
            costDetail.CurrRealPrice = sumeList.Sum(a => a.CurrIncomeQuantity);
            costDetail.CurrRealQuantity = sumeList.Sum(a => a.CurrRealConsumeQuantity);
            costDetail.CurrRealTotalPrice = sumeList.Sum(a => a.CurrRealConsumeTotalPrice);
            costDetail.CurrResponsiQuantity = sumeList.Sum(a => a.CurrResponsiConsumeQuantity);
            costDetail.CurrResponsiTotalPrice = sumeList.Sum(a => a.CurrResponsiConsumeTotalPrice);
            costDetail.DudgetContractQuantity = sumeList.Sum(a => a.DudgetContractQuantity);
            costDetail.DudgetContractTotalPrice = sumeList.Sum(a => a.DudgetContractTotalPrice);
            costDetail.DudgetPlanQuantity = sumeList.Sum(a => a.DudgetPlanQuantity);
            costDetail.DudgetPlanTotalPrice = sumeList.Sum(a => a.DudgetPlanTotalPrice);
            costDetail.DudgetRespQuantity = sumeList.Sum(a => a.DudgetRespQuantity);
            costDetail.DudgetRespTotalPrice = sumeList.Sum(a => a.DudgetRespTotalPrice);
            costDetail.TheAccountBill = costBill;
            costBill.Details.Add(costDetail);

            foreach (CostMonthAccDtlConsume dtlConsume in sumeList)
            {
                if (dtlConsume.CurrRealConsumeTotalPrice == 0 && dtlConsume.CurrIncomeTotalPrice == 0 &&
                    dtlConsume.SumRealConsumeTotalPrice == 0 && dtlConsume.SumResponsiConsumeTotalPrice == 0 
                    && dtlConsume.SumRealConsumePlanTotalPrice == 0 && dtlConsume.SumIncomeTotalPrice == 0)
                {
                    continue;
                }

                dtlConsume.TheAccountDetail = costDetail;
                costDetail.Details.Add(dtlConsume);
            }

            InsertCostMonthAccountNew(costBill, ht_forwardbill);

            return true;
        }

        #endregion

        #region 月度成本核算优化算法

        private void HandleData()
        {
            IList sameList = new ArrayList();
            string id = "";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = "select t1.id,t2.accounttasknodeguid,t3.costingsubjectguid," +
                                  " t3.resourcetypeguid,t3.sumincometotalprice,t3.sumresponsiconsumetotalprice from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 " +
                                  "  where t1.id=t2.parentid and t2.id=t3.parentid and t3.sumincometotalprice != 0 group by t1.id,t2.accounttasknodeguid,t3.costingsubjectguid, " +
                                  "  t3.resourcetypeguid,t3.sumincometotalprice,t3.sumresponsiconsumetotalprice " +
                                  "  having count(*)>1";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string str = TransUtil.ToString(dataRow["id"]) + "-" +
                                 TransUtil.ToString(dataRow["accounttasknodeguid"]) + "-"
                                 + TransUtil.ToString(dataRow["costingsubjectguid"]) + "-" +
                                 TransUtil.ToString(dataRow["resourcetypeguid"])
                                 + "-" + TransUtil.ToDecimal(dataRow["sumincometotalprice"]) + "-" +
                                 TransUtil.ToDecimal(dataRow["sumresponsiconsumetotalprice"]);
                    sameList.Add(str);
                }
            }
            IList existList = new ArrayList();
            session.Transaction.Enlist(command);
            command.CommandText = "select t3.id mxid,t1.id,t2.accounttasknodeguid,t3.costingsubjectguid," +
                                  " t3.resourcetypeguid,t3.sumincometotalprice,t3.sumresponsiconsumetotalprice from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 " +
                                  "  where t1.id=t2.parentid and t2.id=t3.parentid";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string str = TransUtil.ToString(dataRow["id"]) + "-" +
                                 TransUtil.ToString(dataRow["accounttasknodeguid"]) + "-"
                                 + TransUtil.ToString(dataRow["costingsubjectguid"]) + "-" +
                                 TransUtil.ToString(dataRow["resourcetypeguid"])
                                 + "-" + TransUtil.ToDecimal(dataRow["sumincometotalprice"]) + "-" +
                                 TransUtil.ToDecimal(dataRow["sumresponsiconsumetotalprice"]);
                    string mxid = TransUtil.ToString(dataRow["mxid"]);
                    if (sameList.Contains(str))
                    {
                        if (!existList.Contains(str))
                        {
                            existList.Add(str);
                        }
                        else
                        {
                            string sql =
                                " update thd_costmonthaccdtlconsume k1 set k1.sumrealconsumeplanquantity=0,k1.sumrealconsumeplantotalprice=0," +
                                " k1.sumincomequantity=0,k1.sumincometotalprice=0,k1.sumresponsiconsumequantity=0,k1.sumresponsiconsumetotalprice=0 " +
                                "  where k1.id='" + mxid + "'";
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }

        }

        //按照实际金额/预算计划金额来计算合同收入等
        private bool ifMoneyCal(string subjectCode)
        {
            //优化意见:
            //1、subjectStr可以放在方法体外面，并且定义为静态
            //2、用linq来查询匹配可能快一些

            bool ifMoney = false;
            string[] subjectStr =
                {
                    "C5110105", "C5110106", "C5110201", "C5110202", "C5110206", "C5110209", "C5110210", "C511021001","C511021002", "C5110211"
                  //, "C51103"
                    , "C51201"
                  //, "C51202"
                  //, "C51203"
                  , "C51204", "C51205", "C51206", "C51207", "C512080204", "C512080205", "C5120803"
                  //, "C51209"
                    , "C51210", "C51211"
                    ,"C51212", "C51213" 
                  //  ,"C51301", "C51302", "C51303", "C51304", "C51305", "C51306", "C51399"
                    ,"C515"
                    //,"C513"
                    //,"C514"
                };
            //for (int i = 0; i < subjectStr.Length; i++)
            //{
            //    string str = subjectStr[i];
            //    if (subjectCode.Contains(str))
            //    {
            //        ifMoney = true;
            //        return ifMoney;
            //    }
            //}

            ifMoney = subjectStr.Any(a => subjectCode.Contains(a));

            return ifMoney;
        }
        /// <summary>
        /// 费用编码对应的计算方式是否按照建筑面积分摊
        /// </summary>
        /// <param name="subjectCode">费用编码</param>
        /// <param name="calculateType">1：合同收入 2：责任成本 3：实际成本</param>
        /// <returns></returns>
        private bool IsCalculateByAreaParam(string subjectCode,int calculateType)
        {
            //@@@@
            return (subjectCode.StartsWith("C51103") && calculateType == 1)        //机械费 合同收入
                || (subjectCode.StartsWith("C51209") && calculateType == 1)        //外架 合同收入
                || (subjectCode.StartsWith("C51203") && calculateType == 2)        //安全施工费 责任成本
                || (subjectCode.StartsWith("C51202") && calculateType == 2)        //文明施工费 责任成本
                || (subjectCode.StartsWith("C51209") && calculateType == 2)        //外架 责任成本
                ;
        }
        /// <summary>
        /// 费用编码对应的计算方式是否按照测定比值分摊
        /// </summary>
        /// <param name="subjectCode">费用编码</param>
        /// <param name="calculateType">1：合同收入 2：责任成本 3：实际成本</param>
        /// <returns></returns>
        private bool IsCalculateByFixedRate(string subjectCode, int calculateType)
        {
            //@@@@
            return (subjectCode.StartsWith("C51299")
                        //&& !subjectCode.StartsWith("C51203") //刨除 安全施工费 责任成本
                        //&& !subjectCode.StartsWith("C51202") //刨除 文明施工费 责任成本
                        //&& !subjectCode.StartsWith("C51209") //刨除 外架 责任成本
                        && calculateType == 2)                                   //其他总价措施费 责任成本
                || (subjectCode.StartsWith("C514") && calculateType == 2)        //规费 责任成本
                || (subjectCode.StartsWith("C513") && calculateType == 2)        //管理费 责任成本
                ;
        }
        private decimal GetFixedRateParam(CostMonthAccountBill costBill,string subjectCode)
        {
            if (!IsCalculateByFixedRate(subjectCode, 2))
                return 0;
            if (subjectCode.StartsWith("C512"))
                return costBill.TempMeasuresFeeRatio;
            if (subjectCode.StartsWith("C514"))
                return costBill.TempFeesRatio;
            if (subjectCode.StartsWith("C513"))
                return costBill.TempManagementFeeRatio;
            return 0;
        }
        /// <summary>
        /// 费用编码对应的计算方式是否按照工期分摊
        /// </summary>
        /// <param name="subjectCode">费用编码</param>
        /// <param name="calculateType">1：合同收入 2：责任成本 3：实际成本</param>
        /// <returns></returns>
        private bool IsCalculateByDateParam(string subjectCode, int calculateType)
        {
            //@@@@
            return (subjectCode.StartsWith("C51103") && calculateType == 2)        //机械费 责任成本
                ;
        }
        private IList GetMachineCostParameList(string ProjectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", ProjectId));
            return  Dao.ObjectQuery(typeof(MachineCostParame), oq);
        }

        private decimal GetDateParame(IList<MachineCostParame> lstMcp, string subjectCode)
        {
            if (lstMcp == null || lstMcp.Count == 0)
                return 0m;
            MachineCostParame mcp = lstMcp.ToList<MachineCostParame>().Find(a => a.SubjectCode == subjectCode);
            if (mcp == null)
                return 0m;
            if ((DateTime.Now - mcp.ActualentryDate).Days < 0)
                return 0m;
            if ((DateTime.Now - mcp.ActualentryDate).Days > mcp.Duration)
                return 1m;
            else
                return ClientUtil.ToDecimal(((DateTime.Now - mcp.ActualentryDate).Days * 1m) / mcp.Duration);
        }

        /// <summary>
        /// 月度成本核算被调用主程序
        /// </summary>
        [TransManager]
        private string CostMonthAccountSimpleCalByCall(CostMonthAccountBill costBill, CurrentProjectInfo projectInfo)
        {
            //HandleData();
            //return "";
            string msgstr = "";
            Hashtable ht_monthdtl = new Hashtable(); //月度成本核算明细
            Hashtable ht_monthdtlconsume = new Hashtable();
            //根据【工程syscode】_【任务明细id】_【资源类型id】_【核算科目ID】对任务明细下资源耗用生成月度成本耗用明细 记录计划、合同、责任价格和责任合价
            Hashtable ht_price = new Hashtable(); //根据任务节点+科目+资源类构造
            Hashtable ht_priceByOne = new Hashtable(); //根据非实体科目id对任务明细资源耗用责任、计划、合同合价和量汇总
            Hashtable ht_sumTaskAccDtl = new Hashtable(); //根据工程syscode和任务明细id将工程任务核算单进行汇总，计算出当前核算量、核算合价、当前合同、责任收入和量
            Hashtable ht_sumDtlSubject = new Hashtable();
            //根据工程syscode+任务明细ID对【工程任务核算单明细】汇总消耗量、核算合价、合同、责任量和合价 生成【工程任务核算单明细】
            Hashtable ht_sumConsumeDomain = new Hashtable();
            //分包结算单消耗汇总 根据工程syscode+资源类型ID+核算科目ID对【分包结算单】上面资源耗用数量和金额进行汇总
            //获取科目编码信息
            Hashtable ht_subject = this.GetCostSubjectList();

            #region 1：构造本月成本核算信息, 核算范围节点构造<核算节点集合><工程任务核算明细集合><核算明细资源耗用集合>

            #region 获取核算范围内所有工程节点

            //1.1 从GWBS树中查询到核算节点下的所有核算节点(包括本身)
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", costBill.ProjectId));
            oq.AddCriterion(Expression.Like("SysCode", costBill.AccountTaskSysCode + "%"));
            oq.AddCriterion(Expression.Eq("CostAccFlag", true));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails.CostingSubjectGUID", NHibernate.FetchMode.Eager);
            IList list_GWBS = Dao.ObjectQuery(typeof (GWBSTree), oq);

            #endregion

            //1.2 构造<核算节点集合><工程任务核算明细集合><核算明细资源耗用集合>
            foreach (GWBSTree model in list_GWBS)
            {
                foreach (GWBSDetail detail in model.Details)
                {
                    if (detail.CostingFlag == 0)
                        continue;

                    #region 主要根据【工程任务明细】生成【月度成本核算明细】

                    CostMonthAccountDtl costDtl = new CostMonthAccountDtl();
                    costDtl.AccountTaskNodeGUID = model;
                    costDtl.AccountTaskNodeName = model.Name;
                    costDtl.AccountTaskNodeSyscode = model.SysCode;
                    costDtl.ProjectTaskDtlGUID = detail;
                    costDtl.ProjectTaskDtlName = detail.Name;
                    costDtl.TheCostItem = detail.TheCostItem;
                    costDtl.CostItemName = detail.TheCostItem.Name;
                    costDtl.QuantityUnitGUID = detail.WorkAmountUnitGUID;
                    costDtl.QuantityUnitName = detail.WorkAmountUnitName;
                    costDtl.PriceUnitGUID = detail.PriceUnitGUID;
                    costDtl.PriceUnitName = detail.PriceUnitName;
                    ht_monthdtl.Add(model.SysCode + "-" + detail.Id, costDtl);

                    #endregion

                    foreach (GWBSDetailCostSubject sonDetail in detail.ListCostSubjectDetails)
                    {
                        #region 根据任务明细资源耗用生成月度成本耗用明细

                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        if (sonDetail.CostAccountSubjectGUID != null)
                        {
                            dtlConsume.CostingSubjectName = sonDetail.CostAccountSubjectName;
                            dtlConsume.CostSubjectSyscode = sonDetail.CostAccountSubjectGUID.SysCode;
                            dtlConsume.CostSubjectCode = sonDetail.CostAccountSubjectGUID.Code;
                            dtlConsume.CostingSubjectGUID = sonDetail.CostAccountSubjectGUID;
                        }
                        else
                        {
                            continue;
                        }
                        dtlConsume.CurrIncomePrice = sonDetail.ContractQuantityPrice;
                        dtlConsume.CurrRealConsumePlanPrice = sonDetail.PlanPrice;
                        dtlConsume.CurrResponsiConsumePrice = sonDetail.ResponsibilitilyPrice;

                        #region 主要根据工程+资源类型+成本核算科目进行合同、责任、计划量和合价的汇总

                        //非实体：形象进度为100
                        //实体：形象进度>100 为100 否则就是当前进度
                        string dtlWBSStr = model.Id + "-" + sonDetail.ResourceTypeGUID + "-" +
                                           sonDetail.CostAccountSubjectGUID.Id;
                        decimal progress = detail.AddupAccFigureProgress;
                        if (progress > 100)
                        {
                            progress = 100;
                        }
                        string subjectCode = ht_subject[sonDetail.CostAccountSubjectGUID.Id] as string;
                        if (subjectCode != null && subjectCode != "" && ifMoneyCal(subjectCode) == true)
                        {
                            progress = 100;
                        }
                        if (progress > 0)
                        {
                            if (!ht_price.Contains(dtlWBSStr))
                            {
                                DataDomain dDomain = new DataDomain();
                                dDomain.Name1 = sonDetail.ContractQuantityPrice;
                                dDomain.Name2 = sonDetail.PlanPrice;
                                dDomain.Name3 = sonDetail.ResponsibilitilyPrice;
                                dDomain.Name4 = sonDetail.ContractProjectAmount*progress/100;
                                dDomain.Name5 = sonDetail.PlanWorkAmount*progress/100;
                                dDomain.Name6 = sonDetail.ResponsibilitilyWorkAmount*progress/100;
                                dDomain.Name7 = sonDetail.ContractTotalPrice*progress/100;
                                dDomain.Name8 = sonDetail.PlanTotalPrice*progress/100;
                                dDomain.Name9 = sonDetail.ResponsibilitilyTotalPrice*progress/100;
                                dDomain.Name10 = sonDetail.PlanTotalPrice*progress/100;
                                dDomain.Name20 = model.Name;
                                dDomain.Name21 = model.SysCode;
                                dDomain.Name22 = sonDetail.ResourceTypeCode;
                                dDomain.Name23 = sonDetail.ResourceTypeName;
                                dDomain.Name24 = sonDetail.ResourceTypeQuality;
                                dDomain.Name25 = sonDetail.ResourceTypeSpec;
                                dDomain.Name26 = sonDetail.ResourceCateSyscode;
                                dDomain.Name27 = sonDetail.CostAccountSubjectName;
                                dDomain.Name28 = sonDetail.CostAccountSubjectSyscode;

                                ht_price.Add(dtlWBSStr, dDomain);
                            }
                            else
                            {
                                DataDomain dDomain = (DataDomain) ht_price[dtlWBSStr];
                                dDomain.Name4 = TransUtil.ToDecimal(dDomain.Name4) +
                                                sonDetail.ContractProjectAmount*progress/100;
                                dDomain.Name5 = TransUtil.ToDecimal(dDomain.Name5) +
                                                sonDetail.PlanWorkAmount*progress/100;
                                dDomain.Name6 = TransUtil.ToDecimal(dDomain.Name6) +
                                                sonDetail.ResponsibilitilyWorkAmount*progress/100;
                                dDomain.Name7 = TransUtil.ToDecimal(dDomain.Name7) +
                                                sonDetail.ContractTotalPrice*progress/100;
                                dDomain.Name8 = TransUtil.ToDecimal(dDomain.Name8) +
                                                sonDetail.PlanTotalPrice*progress/100;
                                dDomain.Name9 = TransUtil.ToDecimal(dDomain.Name9) +
                                                sonDetail.ResponsibilitilyTotalPrice*progress/100;
                                dDomain.Name10 = TransUtil.ToDecimal(dDomain.Name10) +
                                                 sonDetail.PlanTotalPrice*progress/100;
                                ht_price.Remove(dtlWBSStr); //不需要删除再插入 因为是引用类型
                                ht_price.Add(dtlWBSStr, dDomain);
                            }
                        }

                        #endregion

                        //按科目构造 2015-03-13

                        #region 对非实体的合同、计划、责任量和合价进行汇总  根据成本核算科目id

                        if (subjectCode != null && subjectCode != "" && ifMoneyCal(subjectCode) == true)
                        {
                            string dtlWBSStrByOne = sonDetail.CostAccountSubjectGUID.Id;
                            if (!ht_priceByOne.Contains(dtlWBSStrByOne))
                            {
                                DataDomain dDomain = new DataDomain();
                                dDomain.Name4 = sonDetail.ContractProjectAmount;
                                dDomain.Name5 = sonDetail.PlanWorkAmount;
                                dDomain.Name6 = sonDetail.ResponsibilitilyWorkAmount;
                                dDomain.Name7 = sonDetail.ContractTotalPrice;
                                dDomain.Name8 = sonDetail.PlanTotalPrice;
                                dDomain.Name9 = sonDetail.ResponsibilitilyTotalPrice;

                                ht_priceByOne.Add(dtlWBSStrByOne, dDomain);
                            }
                            else
                            {
                                DataDomain dDomain = (DataDomain) ht_price[dtlWBSStr];
                                dDomain.Name4 = TransUtil.ToDecimal(dDomain.Name4) + sonDetail.ContractProjectAmount;
                                dDomain.Name5 = TransUtil.ToDecimal(dDomain.Name5) + sonDetail.PlanWorkAmount;
                                dDomain.Name6 = TransUtil.ToDecimal(dDomain.Name6) +
                                                sonDetail.ResponsibilitilyWorkAmount;
                                dDomain.Name7 = TransUtil.ToDecimal(dDomain.Name7) + sonDetail.ContractTotalPrice;
                                dDomain.Name8 = TransUtil.ToDecimal(dDomain.Name8) + sonDetail.PlanTotalPrice;
                                dDomain.Name9 = TransUtil.ToDecimal(dDomain.Name9) +
                                                sonDetail.ResponsibilitilyTotalPrice;
                                ht_priceByOne.Remove(dtlWBSStrByOne);
                                ht_priceByOne.Add(dtlWBSStrByOne, dDomain);
                            }
                        }

                        #endregion

                        dtlConsume.ResourceTypeGUID = sonDetail.ResourceTypeGUID;
                        dtlConsume.ResourceTypeName = sonDetail.ResourceTypeName;
                        dtlConsume.RationUnitGUID = sonDetail.ProjectAmountUnitGUID;
                        dtlConsume.RationUnitName = sonDetail.ProjectAmountUnitName;
                        dtlConsume.ProjectTaskDtlGUID = detail.Id;
                        dtlConsume.ProjectTaskDtlName = detail.Name;
                        dtlConsume.TheAccountDetail = costDtl;
                        dtlConsume.Data1 = sonDetail.PlanTotalPrice + ""; //计划耗用合价
                        string dtlConsumeLinkStr = model.SysCode + "-" + detail.Id + "-" + sonDetail.ResourceTypeGUID +
                                                   "-" + sonDetail.CostAccountSubjectGUID.Id;
                        if (!ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                        {
                            ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                        }

                        #endregion
                    }
                }
            }

            #endregion

            #region 2：工程任务核算单的月度汇总

            #region 查询工程任务核算单

            //2.1 查询工程任务核算信息
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime.AddDays(1)));
            oq.AddCriterion(Expression.IsNull("MonthAccountBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_PTaskAccount = Dao.ObjectQuery(typeof (ProjectTaskAccountBill), oq);

            #endregion

            //2.2汇总工程任务核算信息

            foreach (ProjectTaskAccountBill model in list_PTaskAccount)
            {
                //工程任务核算明细信息
                foreach (ProjectTaskDetailAccount dtl in model.Details)
                {
                    #region 根据【工程syscode】+【任务明细ID】对【工程任务核算单】进行汇总生成【工程任务核算单】

                    if (ht_sumTaskAccDtl.Contains(dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id))
                    {
                        ProjectTaskDetailAccount pDtl =
                            (ProjectTaskDetailAccount)
                            ht_sumTaskAccDtl[dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id];
                        pDtl.AccountProjectAmount += dtl.AccountProjectAmount;
                        pDtl.AccountTotalPrice += dtl.AccountTotalPrice;
                        pDtl.CurrAccFigureProgress += dtl.CurrAccFigureProgress;
                        pDtl.CurrContractIncomeQny += dtl.CurrContractIncomeQny;
                        pDtl.CurrContractIncomeTotal += dtl.CurrContractIncomeTotal;
                        pDtl.CurrResponsibleCostQny += dtl.CurrResponsibleCostQny;
                        pDtl.CurrResponsibleCostTotal += dtl.CurrResponsibleCostTotal;
                        pDtl.CurrAccEV += dtl.CurrAccEV;
                        ht_sumTaskAccDtl.Remove(dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id);
                        ht_sumTaskAccDtl.Add(dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id, pDtl);
                    }
                    else
                    {
                        ProjectTaskDetailAccount pDtl = new ProjectTaskDetailAccount();
                        pDtl.AccountProjectAmount = dtl.AccountProjectAmount;
                        pDtl.AccountTotalPrice = dtl.AccountTotalPrice;
                        pDtl.CurrAccFigureProgress = dtl.CurrAccFigureProgress;
                        pDtl.CurrContractIncomeQny = dtl.CurrContractIncomeQny;
                        pDtl.CurrContractIncomeTotal = dtl.CurrContractIncomeTotal;
                        pDtl.CurrResponsibleCostQny = dtl.CurrResponsibleCostQny;
                        pDtl.CurrResponsibleCostTotal = dtl.CurrResponsibleCostTotal;
                        pDtl.CurrAccEV = dtl.CurrAccEV;
                        ht_sumTaskAccDtl.Add(dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id, pDtl);
                    }

                    #endregion

                    //工程任务资源耗用信息
                    foreach (ProjectTaskDetailAccountSubject subject in dtl.Details)
                    {
                        #region 根据【工程syscode】+【工程任务明细ID】+【资源类型ID】+【核算科目ID】对【工程资源耗用单】进行汇总消耗量、核算合价、合同、责任量和合价、

                        if (subject.CostingSubjectGUID == null)
                            continue;
                        string linkStr = dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id + "-" +
                                         subject.ResourceTypeGUID + "-" + subject.CostingSubjectGUID.Id;
                        if (ht_sumDtlSubject.Contains(linkStr))
                        {
                            ProjectTaskDetailAccountSubject pSubject =
                                (ProjectTaskDetailAccountSubject) ht_sumDtlSubject[linkStr];
                            pSubject.AccUsageQny += subject.AccUsageQny;
                            pSubject.AccountTotalPrice += subject.AccountTotalPrice;
                            pSubject.CurrContractIncomeQny += subject.CurrContractIncomeQny;
                            pSubject.CurrContractIncomeTotal += subject.CurrContractIncomeTotal;
                            pSubject.CurrResponsibleCostQny += subject.CurrResponsibleCostQny;
                            pSubject.CurrResponsibleCostTotal += subject.CurrResponsibleCostTotal;
                            ht_sumDtlSubject.Remove(linkStr);
                            ht_sumDtlSubject.Add(linkStr, pSubject);
                        }
                        else
                        {
                            ProjectTaskDetailAccountSubject pSubject = new ProjectTaskDetailAccountSubject();
                            pSubject.TheProjectGUID = dtl.ProjectTaskDtlGUID.Id; //借用
                            pSubject.CostingSubjectGUID = subject.CostingSubjectGUID;
                            pSubject.CostingSubjectName = subject.CostingSubjectName;
                            pSubject.ResourceTypeGUID = subject.ResourceTypeGUID;
                            pSubject.ResourceTypeName = subject.ResourceTypeName;
                            pSubject.ResourceTypeSpec = subject.ResourceTypeSpec;
                            pSubject.ResourceTypeQuality = subject.ResourceTypeQuality;
                            pSubject.QuantityUnitGUID = subject.QuantityUnitGUID;
                            pSubject.QuantityUnitName = subject.QuantityUnitName;
                            pSubject.AccUsageQny = subject.AccUsageQny;
                            pSubject.AccountTotalPrice = subject.AccountTotalPrice;
                            pSubject.CurrContractIncomeQny = subject.CurrContractIncomeQny;
                            pSubject.CurrContractIncomeTotal = subject.CurrContractIncomeTotal;
                            pSubject.CurrResponsibleCostQny = subject.CurrResponsibleCostQny;
                            pSubject.CurrResponsibleCostTotal = subject.CurrResponsibleCostTotal;
                            pSubject.TheAccountDetail = dtl;

                            ht_sumDtlSubject.Add(linkStr, pSubject);
                        }

                        #endregion

                    }
                }
            }

            #region 根据【工程syscode】_【任务明细ID】的【工程任务核算单明细】汇总到对应【月度成本核算明细】

            //2.3工程任务明细月度核算
            IList list_notGWBSMx = new ArrayList(); //在GWBS没找到对应的工程任务明细
            foreach (string dtlLinkStr in ht_sumTaskAccDtl.Keys)
            {
                if (ht_monthdtl.Contains(dtlLinkStr))
                {
                    CostMonthAccountDtl costDtl = (CostMonthAccountDtl) ht_monthdtl[dtlLinkStr];
                    ProjectTaskDetailAccount pDtl = (ProjectTaskDetailAccount) ht_sumTaskAccDtl[dtlLinkStr];
                    costDtl.CurrRealQuantity += pDtl.AccountProjectAmount;
                    costDtl.CurrEarnValue += pDtl.CurrAccEV;
                    costDtl.CurrCompletedPercent += pDtl.CurrAccFigureProgress;
                    costDtl.CurrIncomeQuantity += pDtl.CurrContractIncomeQny;
                    costDtl.CurrIncomeTotalPrice += pDtl.CurrContractIncomeTotal;
                    costDtl.CurrResponsiQuantity += pDtl.CurrResponsibleCostQny;
                    costDtl.CurrResponsiTotalPrice += pDtl.CurrResponsibleCostTotal;
                    costDtl.IfTaskAcctMx = 1; //写入工程任务核算明细标志
                    ht_monthdtl.Remove(dtlLinkStr);
                    ht_monthdtl.Add(dtlLinkStr, costDtl);
                }
                else
                {
                    list_notGWBSMx.Add((ProjectTaskDetailAccount) ht_sumTaskAccDtl[dtlLinkStr]);
                }
            }

            #endregion

            //是否抛出错误信息
            if (list_notGWBSMx.Count > 0)
            {
                //throw new Exception("");
            }
            //核算{月度资源耗用明细核算}数据

            #region 根据【工程syscode】_【任务明细id】_【资源类型ID】_【成本核算科目ID】将【工程任务核算单资源消耗明细】汇总到【月度成本核算资源消耗明细】上

            foreach (string dtlConsumeLinkStr in ht_sumDtlSubject.Keys)
            {
                ProjectTaskDetailAccountSubject dtlAccount =
                    (ProjectTaskDetailAccountSubject) ht_sumDtlSubject[dtlConsumeLinkStr];
                if (ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                {
                    CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                    dtlConsume.CurrIncomeQuantity += dtlAccount.CurrContractIncomeQny;
                    dtlConsume.CurrIncomeTotalPrice += dtlAccount.CurrContractIncomeTotal;
                    dtlConsume.CurrRealConsumePlanQuantity += dtlAccount.AccUsageQny;
                    dtlConsume.CurrRealConsumePlanTotalPrice += dtlAccount.AccountTotalPrice;
                    dtlConsume.CurrResponsiConsumeQuantity += dtlAccount.CurrResponsibleCostQny;
                    dtlConsume.CurrResponsiConsumeTotalPrice += dtlAccount.CurrResponsibleCostTotal;
                    ht_monthdtlconsume.Remove(dtlConsumeLinkStr);
                    ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                }
                else
                {
//新生成一个
                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    dtlConsume.ProjectTaskDtlGUID = dtlAccount.TheProjectGUID;
                    dtlConsume.CostingSubjectGUID = dtlAccount.CostingSubjectGUID;
                    dtlConsume.CostingSubjectName = dtlAccount.CostingSubjectName;
                    dtlConsume.CostSubjectCode = dtlAccount.CostingSubjectGUID.Code;
                    dtlConsume.CostSubjectSyscode = dtlAccount.CostingSubjectGUID.SysCode;
                    dtlConsume.ResourceTypeGUID = dtlAccount.ResourceTypeGUID;
                    dtlConsume.ResourceTypeName = dtlAccount.ResourceTypeName;
                    dtlConsume.ResourceTypeSpec = dtlAccount.ResourceTypeSpec;
                    dtlConsume.ResourceTypeStuff = dtlAccount.ResourceTypeQuality;
                    dtlConsume.ResourceSyscode = dtlAccount.ResourceCategorySysCode;
                    dtlConsume.CurrIncomeQuantity = dtlAccount.CurrContractIncomeQny;
                    dtlConsume.CurrIncomeTotalPrice = dtlAccount.CurrContractIncomeTotal;
                    dtlConsume.CurrRealConsumePlanQuantity = dtlAccount.AccUsageQny;
                    dtlConsume.CurrRealConsumePlanTotalPrice = dtlAccount.AccountTotalPrice;
                    dtlConsume.CurrResponsiConsumeQuantity = dtlAccount.CurrResponsibleCostQny;
                    dtlConsume.CurrResponsiConsumeTotalPrice = dtlAccount.CurrResponsibleCostTotal;
                    //写入物资耗用对应的月度成本核算明细
                    foreach (CostMonthAccountDtl acctDtl in ht_monthdtl.Values)
                    {
                        if (dtlConsume.ProjectTaskDtlGUID == acctDtl.ProjectTaskDtlGUID.Id)
                        {
                            dtlConsume.TheAccountDetail = acctDtl;
                            break;
                        }
                    }
                    ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                }
            }

            #endregion

            #endregion

            #region 3：分包结算单的月度汇总

            //3.1 查询分包结算信息

            #region 分包结算查询

            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.IsNull("MonthAccBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_SubBalance = Dao.ObjectQuery(typeof (SubContractBalanceBill), oq);
            //IList list_SubBalance = new ArrayList();

            #endregion

            #region 根据工程syscode+资源类型ID+核算科目ID对资源耗用进行数量和金额汇总

            //3.2 分包结算信息进行月度结算
            foreach (SubContractBalanceBill model in list_SubBalance)
            {
                foreach (SubContractBalanceDetail dtl in model.Details)
                {
                    foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                    {
                        string dtlConsumeLinkStr = dtl.BalanceTaskSyscode + "-" + subject.ResourceTypeGUID + "-" +
                                                   subject.BalanceSubjectGUID.Id;
                        if (ht_sumConsumeDomain.Contains(dtlConsumeLinkStr))
                        {
                            DataDomain tempDataDomain = (DataDomain) ht_sumConsumeDomain[dtlConsumeLinkStr];
                            tempDataDomain.Name1 = (TransUtil.ToDecimal(tempDataDomain.Name1) + subject.BalanceQuantity) +
                                                   "";
                            tempDataDomain.Name2 = (TransUtil.ToDecimal(tempDataDomain.Name2) +
                                                    subject.BalanceTotalPrice) + "";
                            ht_sumConsumeDomain.Remove(dtlConsumeLinkStr);
                            ht_sumConsumeDomain.Add(dtlConsumeLinkStr, tempDataDomain);
                        }
                        else
                        {
                            DataDomain consumeDataDomain = new DataDomain();
                            consumeDataDomain.Name1 = TransUtil.ToString(subject.BalanceQuantity);
                            consumeDataDomain.Name2 = TransUtil.ToString(subject.BalanceTotalPrice);
                            consumeDataDomain.Name3 = TransUtil.ToString(subject.ResourceTypeGUID);
                            consumeDataDomain.Name4 = TransUtil.ToString(subject.ResourceTypeName);
                            consumeDataDomain.Name5 = TransUtil.ToString(subject.ResourceSyscode);
                            consumeDataDomain.Name6 = (StandardUnit) subject.QuantityUnit;
                            consumeDataDomain.Name7 = (CostAccountSubject) subject.BalanceSubjectGUID;
                            consumeDataDomain.Name8 = TransUtil.ToString(subject.BalanceSubjectName);
                            consumeDataDomain.Name9 = TransUtil.ToString(subject.BalanceSubjectSyscode);
                            consumeDataDomain.Name20 = dtl.BalanceTaskName;
                            if (dtl.BalanceTaskDtl != null)
                            {
                                consumeDataDomain.Name21 = dtl.BalanceTaskDtl.Id;
                            }
                            ht_sumConsumeDomain.Add(dtlConsumeLinkStr, consumeDataDomain);
                        }
                    }
                }
            }

            #endregion

            #endregion

            #region 4：物资耗用/料具租赁结算单的月度汇总

            //4.1 查询物资耗用结算信息和料具结算单
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.IsNull("MonthAccountBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list_resourceConsume = Dao.ObjectQuery(typeof (MaterialSettleMaster), oq);
            //IList list_resourceConsume = new ArrayList();

            //4.2 汇总物资耗用结算
            foreach (MaterialSettleMaster model in list_resourceConsume)
            {
                foreach (MaterialSettleDetail dtl in model.Details)
                {
                    if (dtl.MaterialResource == null)
                    {
                        msgstr = "物资耗用结算中存在物资为空的情况！";
                        return msgstr;
                    }
                    if (dtl.AccountCostSubject == null)
                    {
                        msgstr = "物资耗用结算中存在核算科目为空的情况！";
                        return msgstr;
                    }
                    string dtlConsumeLinkStr = dtl.ProjectTaskCode + "-" + dtl.MaterialResource.Id + "-" +
                                               dtl.AccountCostSubject.Id;
                    if (ht_sumConsumeDomain.Contains(dtlConsumeLinkStr))
                    {
                        DataDomain tempDataDomain = (DataDomain) ht_sumConsumeDomain[dtlConsumeLinkStr];
                        tempDataDomain.Name1 = (TransUtil.ToDecimal(tempDataDomain.Name1) + dtl.Quantity) + "";
                        tempDataDomain.Name2 = (TransUtil.ToDecimal(tempDataDomain.Name2) + dtl.Money) + "";
                        ht_sumConsumeDomain.Remove(dtlConsumeLinkStr);
                        ht_sumConsumeDomain.Add(dtlConsumeLinkStr, tempDataDomain);
                    }
                    else
                    {
                        DataDomain consumeDataDomain = new DataDomain();
                        consumeDataDomain.Name1 = TransUtil.ToString(dtl.Quantity);
                        consumeDataDomain.Name2 = TransUtil.ToString(dtl.Money);
                        consumeDataDomain.Name3 = TransUtil.ToString(dtl.MaterialResource.Id);
                        consumeDataDomain.Name4 = TransUtil.ToString(dtl.MaterialName);
                        consumeDataDomain.Name5 = TransUtil.ToString(dtl.MaterialSysCode);
                        consumeDataDomain.Name6 = (StandardUnit) dtl.QuantityUnit;
                        consumeDataDomain.Name7 = (CostAccountSubject) dtl.AccountCostSubject;
                        consumeDataDomain.Name8 = TransUtil.ToString(dtl.AccountCostName);
                        consumeDataDomain.Name9 = TransUtil.ToString(dtl.AccountCostCode);
                        consumeDataDomain.Name20 = dtl.ProjectTaskName;
                        ht_sumConsumeDomain.Add(dtlConsumeLinkStr, consumeDataDomain);
                    }
                }
            }

            #endregion

            #region 5：设备租赁结算单的月度汇总

            // 查询设备租赁结算单
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.IsNull("MonthAccountBillId"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialSubjectDetails", NHibernate.FetchMode.Eager);
            IList list_machineConsume = Dao.ObjectQuery(typeof (MaterialRentalSettlementMaster), oq);
            //IList list_machineConsume = new ArrayList();
            // 汇总设备租赁结算
            foreach (MaterialRentalSettlementMaster model in list_machineConsume)
            {
                foreach (MaterialRentalSettlementDetail dtl in model.Details)
                {
                    foreach (MaterialSubjectDetail subject in dtl.MaterialSubjectDetails)
                    {
                        string dtlConsumeLinkStr = dtl.UsedPartSysCode + "-" + dtl.MaterialResource.Id + "-" +
                                                   subject.SettleSubject.Id;
                        if (ht_sumConsumeDomain.Contains(dtlConsumeLinkStr))
                        {
                            DataDomain tempDataDomain = (DataDomain) ht_sumConsumeDomain[dtlConsumeLinkStr];
                            tempDataDomain.Name1 = (TransUtil.ToDecimal(tempDataDomain.Name1) + subject.SettleQuantity) +
                                                   "";
                            tempDataDomain.Name2 = (TransUtil.ToDecimal(tempDataDomain.Name2) + subject.SettleMoney) +
                                                   "";
                            ht_sumConsumeDomain.Remove(dtlConsumeLinkStr);
                            ht_sumConsumeDomain.Add(dtlConsumeLinkStr, tempDataDomain);
                        }
                        else
                        {
                            DataDomain consumeDataDomain = new DataDomain();
                            consumeDataDomain.Name1 = TransUtil.ToString(subject.SettleQuantity);
                            consumeDataDomain.Name2 = TransUtil.ToString(subject.SettleMoney);
                            consumeDataDomain.Name3 = TransUtil.ToString(subject.MaterialType.Id);
                            consumeDataDomain.Name4 = TransUtil.ToString(subject.MaterialTypeName);
                            consumeDataDomain.Name5 = TransUtil.ToString(subject.MaterialStuff);
                            consumeDataDomain.Name6 = (StandardUnit) subject.QuantityUnit;
                            consumeDataDomain.Name7 = (CostAccountSubject) subject.SettleSubject;
                            consumeDataDomain.Name8 = TransUtil.ToString(subject.SettleSubjectName);
                            consumeDataDomain.Name9 = TransUtil.ToString(subject.SettleSubjectSyscode);
                            consumeDataDomain.Name20 = dtl.UsedPartName;
                            ht_sumConsumeDomain.Add(dtlConsumeLinkStr, consumeDataDomain);
                        }
                    }
                }
            }

            #endregion

            #region 6：费用结算单的月度汇总

            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.IsNull("MonthlySettlment"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddFetchMode("Details.AccountCostSubject", NHibernate.FetchMode.Eager);
            IList list_expenseConsume = Dao.ObjectQuery(typeof (ExpensesSettleMaster), oq);
            //IList list_expenseConsume = new ArrayList();
            // 汇总费用结算结算
            foreach (ExpensesSettleMaster model in list_expenseConsume)
            {
                foreach (ExpensesSettleDetail dtl in model.Details)
                {
                    string dtlConsumeLinkStr = dtl.ProjectTaskSysCode + "-" + dtl.MaterialResource.Id + "-" +
                                               dtl.AccountCostSubject.Id;
                    if (ht_sumConsumeDomain.Contains(dtlConsumeLinkStr))
                    {
                        DataDomain tempDataDomain = (DataDomain) ht_sumConsumeDomain[dtlConsumeLinkStr];
                        tempDataDomain.Name1 = (TransUtil.ToDecimal(tempDataDomain.Name1) + dtl.Quantity) + "";
                        tempDataDomain.Name2 = (TransUtil.ToDecimal(tempDataDomain.Name2) + dtl.Money) + "";
                        ht_sumConsumeDomain.Remove(dtlConsumeLinkStr);
                        ht_sumConsumeDomain.Add(dtlConsumeLinkStr, tempDataDomain);
                    }
                    else
                    {
                        DataDomain consumeDataDomain = new DataDomain();
                        consumeDataDomain.Name1 = TransUtil.ToString(dtl.Quantity);
                        consumeDataDomain.Name2 = TransUtil.ToString(dtl.Money);
                        consumeDataDomain.Name3 = TransUtil.ToString(dtl.MaterialResource.Id);
                        consumeDataDomain.Name4 = TransUtil.ToString(dtl.MaterialName);
                        consumeDataDomain.Name5 = TransUtil.ToString(dtl.MaterialSysCode);
                        consumeDataDomain.Name6 = (StandardUnit) dtl.QuantityUnit;
                        consumeDataDomain.Name7 = (CostAccountSubject) dtl.AccountCostSubject;
                        consumeDataDomain.Name8 = TransUtil.ToString(dtl.AccountCostName);
                        consumeDataDomain.Name9 = TransUtil.ToString(dtl.AccountCostSysCode);
                        consumeDataDomain.Name20 = dtl.ProjectTaskName;
                        ht_sumConsumeDomain.Add(dtlConsumeLinkStr, consumeDataDomain);
                    }
                }
            }
            //补充科目编码
            foreach (string dtlStrLink in ht_monthdtlconsume.Keys)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlStrLink];
                string subjectCode = TransUtil.ToString(ht_subject[dtlConsume.CostingSubjectGUID.Id]);
                dtlConsume.CostSubjectCode = subjectCode;
            }
            ht_monthdtlconsume = this.RealConsumeSimpleCal(costBill, ht_monthdtl, ht_monthdtlconsume,
                                                           ht_sumConsumeDomain, projectInfo, list_GWBS, ht_subject);
            if (ht_monthdtlconsume.ContainsKey("error"))
            {
                msgstr = ht_monthdtlconsume["error"].ToString();
                return msgstr;
            }

            #endregion

            #region 7：计算实际值和累计值等

            //7.1 更新本月累计值
            foreach (CostMonthAccDtlConsume sonDetail in ht_monthdtlconsume.Values)
            {
                this.CalCostMonthAcctDtlConsumeSumValue(sonDetail, new CostMonthAccDtlConsume());
            }
            foreach (CostMonthAccountDtl detail in ht_monthdtl.Values)
            {
                this.CalCostMonthAcctDtlSumValue(detail, new CostMonthAccountDtl());
            }

            //7.2 查询上期的月度成本核算
            int lastYear = TransUtil.GetLastYear(costBill.Kjn, costBill.Kjy);
            int lastMonth = TransUtil.GetLastMonth(costBill.Kjn, costBill.Kjy);

            #region 新的累计算法

            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheAccountBill.Kjn", lastYear));
            oq.AddCriterion(Expression.Eq("TheAccountBill.Kjy", lastMonth));
            oq.AddCriterion(Expression.Eq("TheAccountBill.ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Like("AccountTaskNodeSyscode", "%" + costBill.AccountRange.Id + "%"));
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_lastCostMonthAcc = Dao.ObjectQuery(typeof (CostMonthAccountDtl), oq);
            //IList list_lastCostMonthAcc = new ArrayList();
            Hashtable last_monthdtl = new Hashtable();
            //构造月度核算明细和月度核算资源耗用Hashtable
            foreach (CostMonthAccountDtl detail in list_lastCostMonthAcc)
            {
                foreach (CostMonthAccDtlConsume sonDetail in detail.Details)
                {
//应该是工程syscode+任务明细ID+资源ID+成本核算科目ID
                    string linkStr = sonDetail.ProjectTaskDtlGUID + "-" + sonDetail.ResourceTypeGUID + "-" +
                                     sonDetail.CostingSubjectGUID.Id;
                    if (ht_monthdtlconsume.Contains(linkStr)) //找到上月同一[工程任务明细+资源类型+成本科目]
                    {
                        CostMonthAccDtlConsume currDtlConsume = (CostMonthAccDtlConsume) ht_monthdtlconsume[linkStr];
                        if (currDtlConsume.Data3 == "Last") //currDtlConsume为上月数据
                        {
                            this.CalLastCostMonthAcctDtlConsumeSumValue(currDtlConsume, sonDetail);
                        }
                        else
                        {
                            this.CalCostMonthAcctDtlConsumeSumValue(currDtlConsume, sonDetail);
                        }
                    }
                    else
                    {
                        CostMonthAccDtlConsume newDtlConsume = this.TransCostMonthAccDtlConsume(sonDetail);
                        string dtlLink = detail.AccountTaskNodeSyscode + "-" + detail.ProjectTaskDtlGUID.Id;
                        if (ht_monthdtl.Contains(dtlLink)) //存在工程任务明细记录
                        {
                            CostMonthAccountDtl currDetail = (CostMonthAccountDtl) ht_monthdtl[dtlLink];
                            currDetail.TheAccountBill = costBill;
                            newDtlConsume.TheAccountDetail = currDetail;
                            newDtlConsume.Data3 = "Last";
                            ht_monthdtlconsume.Add(linkStr, newDtlConsume);
                        }
                        else
                        {
                            if (last_monthdtl.Contains(dtlLink))
                            {
                                CostMonthAccountDtl newDtl = (CostMonthAccountDtl) last_monthdtl[dtlLink];
                                newDtlConsume.TheAccountDetail = newDtl;
                                newDtl.Details.Add(newDtlConsume);
                                ht_monthdtlconsume.Add(linkStr, newDtlConsume);
                            }
                            else
                            {
                                CostMonthAccountDtl newDtl = this.TransCostMonthAccountDtl(detail);
                                newDtlConsume.TheAccountDetail = newDtl;
                                newDtl.Details.Add(newDtlConsume);
                                last_monthdtl.Add(dtlLink, newDtl);
                                ht_monthdtlconsume.Add(linkStr, newDtlConsume);
                            }
                        }
                    }
                }
            }

            #endregion

            #endregion

            #region 8：数据库处理

            //8.1 写入月度成本核算信息
            foreach (string str in ht_monthdtlconsume.Keys)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume) ht_monthdtlconsume[str];
                if (dtlConsume.CostingSubjectGUID.Id != null && TransUtil.ToString(dtlConsume.CostSubjectCode) == "")
                {
                    dtlConsume.CostSubjectCode = TransUtil.ToString(ht_subject[dtlConsume.CostingSubjectGUID.Id]);
                }
            }
            //构造耗用明细的HT
            Hashtable ht_dtlid = new Hashtable();
            foreach (CostMonthAccDtlConsume dtlConsume in ht_monthdtlconsume.Values)
            {
                if (!ht_dtlid.Contains(dtlConsume.ProjectTaskDtlGUID))
                {
                    IList temp = new ArrayList();
                    temp.Add(dtlConsume);
                    ht_dtlid.Add(dtlConsume.ProjectTaskDtlGUID, temp);
                }
                else
                {
                    IList temp = (ArrayList) ht_dtlid[dtlConsume.ProjectTaskDtlGUID];
                    temp.Add(dtlConsume);
                }
            }
            //核算明细挂接耗用明细
            foreach (CostMonthAccountDtl detail in ht_monthdtl.Values)
            {
                if (detail.ProjectTaskDtlGUID != null && detail.ProjectTaskDtlGUID.Id == "0")
                {
                    detail.ProjectTaskDtlGUID = null;
                }

                bool ifMx = false;
                if (ht_dtlid.Contains(detail.ProjectTaskDtlGUID.Id))
                {
                    IList temp = (ArrayList) ht_dtlid[detail.ProjectTaskDtlGUID.Id];
                    foreach (CostMonthAccDtlConsume dtlConsume in temp)
                    {
                        dtlConsume.TheAccountDetail = detail;
                        ifMx = true;
                        detail.Details.Add(dtlConsume);
                    }

                }
                if (ifMx == true)
                {
                    detail.TheAccountBill = costBill;
                    costBill.Details.Add(detail);
                }
            }
            foreach (CostMonthAccountDtl dtl in last_monthdtl.Values) //加入上月数据
            {
                dtl.TheAccountBill = costBill;
                costBill.Details.Add(dtl);
            }
            if (costBill.Details.Count == 0)
            {
                msgstr = "选择时间段内无月度核算信息产生！";
                return msgstr;
            }

            Hashtable ht_Two = new Hashtable(); //实际耗用中“任务节点+科目对应的累计实际值”
            foreach (CostMonthAccountDtl detail in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in detail.Details)
                {
                    string consumeStrByTwo = detail.AccountTaskNodeGUID.Id + "-" + dtlConsume.CostingSubjectGUID.Id;
                    if (!ht_Two.Contains(consumeStrByTwo))
                    {
                        decimal sumRealValue = dtlConsume.SumRealConsumeTotalPrice;
                        ht_Two.Add(consumeStrByTwo, sumRealValue);
                    }
                    else
                    {
                        decimal currValue = TransUtil.ToDecimal(ht_Two[consumeStrByTwo]);
                        decimal sumRealValue = dtlConsume.SumRealConsumeTotalPrice + currValue;
                        ht_Two.Remove(consumeStrByTwo);
                        ht_Two.Add(consumeStrByTwo, sumRealValue);
                    }

                }
            }

            //重新配价_安装专业(累计值的单价取当前预算体系的价格)
            if (costBill.Temp5 == "1" || 1 == 1)
            {
                IList sumSameList = new ArrayList(); //把相同科目、资源、节点的累计
                IList sumTwoList = new ArrayList(); //把相同科目、节点的累计
                foreach (CostMonthAccountDtl detail in costBill.Details)
                {
                    foreach (CostMonthAccDtlConsume dtlConsume in detail.Details)
                    {
                        //先清除累计值
                        dtlConsume.SumIncomeQuantity = 0;
                        dtlConsume.SumRealConsumePlanQuantity = 0;
                        dtlConsume.SumResponsiConsumeQuantity = 0;
                        dtlConsume.SumIncomeTotalPrice = 0;
                        dtlConsume.SumRealConsumePlanTotalPrice = 0;
                        dtlConsume.SumResponsiConsumeTotalPrice = 0;

                        decimal sumRealConsumeQty = dtlConsume.SumRealConsumeQuantity;
                        decimal sumRealConsumeMoney = dtlConsume.SumRealConsumeTotalPrice;
                        decimal currRealConsumeQty = dtlConsume.CurrRealConsumeQuantity;
                        decimal currRealConsumeMoney = dtlConsume.CurrRealConsumeTotalPrice;
                        string subjectCode = ht_subject[dtlConsume.CostingSubjectGUID.Id] as string;
                        //1：当科目为按金额计算时
                        if (subjectCode != null && subjectCode != "" && ifMoneyCal(subjectCode) == true)
                        {
                            decimal sumcalplanTotalPrice = 0;
                            decimal sumcalcontractQuantity = 0;
                            decimal sumcalplanQuantity = 0;
                            decimal sumcalresponsiQuantity = 0;
                            decimal sumcalcontractTotalPrice = 0;
                            decimal sumcalresponsiTotalPrice = 0;
                            foreach (string subjectID in ht_priceByOne.Keys)
                            {
                                string tempsubjectcode = ht_subject[subjectID] as string;
                                DataDomain dDomain = (DataDomain) (ht_priceByOne[subjectID]);
                                if (tempsubjectcode.Contains(subjectCode) == true ||
                                    subjectCode.Contains(tempsubjectcode) == true)
                                {
                                    sumcalcontractQuantity += TransUtil.ToDecimal(dDomain.Name4);
                                    sumcalplanQuantity += TransUtil.ToDecimal(dDomain.Name5);
                                    sumcalresponsiQuantity += TransUtil.ToDecimal(dDomain.Name6);
                                    sumcalcontractTotalPrice += TransUtil.ToDecimal(dDomain.Name7);
                                    sumcalplanTotalPrice += TransUtil.ToDecimal(dDomain.Name8);
                                    sumcalresponsiTotalPrice += TransUtil.ToDecimal(dDomain.Name9);
                                }
                            }
                            if (sumcalplanTotalPrice > 0)
                            {
                                //累计合同收入等
                                if (sumRealConsumeMoney != 0)
                                {
                                    decimal progress = decimal.Round(sumRealConsumeMoney/sumcalplanTotalPrice, 2);
                                    if (progress > 1)
                                    {
                                        progress = 1;
                                    }
                                    dtlConsume.SumIncomeQuantity = decimal.Round(sumcalcontractQuantity*progress, 4);
                                    dtlConsume.SumIncomeTotalPrice = decimal.Round(sumcalcontractTotalPrice*progress, 2);
                                    dtlConsume.SumRealConsumePlanQuantity = decimal.Round(sumcalplanQuantity*progress, 4);
                                    dtlConsume.SumRealConsumePlanTotalPrice =
                                        decimal.Round(sumcalplanTotalPrice*progress, 2);
                                    dtlConsume.SumResponsiConsumeQuantity =
                                        decimal.Round(sumcalresponsiQuantity*progress, 4);
                                    dtlConsume.SumResponsiConsumeTotalPrice =
                                        decimal.Round(sumcalresponsiTotalPrice*progress, 2);
                                }
                                else
                                {
                                    dtlConsume.SumIncomeQuantity = sumRealConsumeQty;
                                    dtlConsume.SumIncomeTotalPrice = sumRealConsumeMoney;
                                    dtlConsume.SumRealConsumePlanQuantity = sumRealConsumeQty;
                                    dtlConsume.SumRealConsumePlanTotalPrice = sumRealConsumeMoney;
                                    dtlConsume.SumResponsiConsumeQuantity = sumRealConsumeQty;
                                    dtlConsume.SumResponsiConsumeTotalPrice = sumRealConsumeMoney;
                                }
                            }
                            else
                            {
                                //如果没找到科目的计划金额，则取实际成本
                                dtlConsume.SumIncomeQuantity = sumRealConsumeQty;
                                dtlConsume.SumIncomeTotalPrice = sumRealConsumeMoney;
                                dtlConsume.SumRealConsumePlanQuantity = sumRealConsumeQty;
                                dtlConsume.SumRealConsumePlanTotalPrice = sumRealConsumeMoney;
                                dtlConsume.SumResponsiConsumeQuantity = sumRealConsumeQty;
                                dtlConsume.SumResponsiConsumeTotalPrice = sumRealConsumeMoney;
                            }
                        }
                        else //2：当其他情况，则从成本预算体系中取
                        {
                            string consumeStr = detail.AccountTaskNodeGUID.Id + "-" + dtlConsume.ResourceTypeGUID + "-" +
                                                dtlConsume.CostingSubjectGUID.Id;
                            if (ht_price.Contains(consumeStr)) //通过相同科目、资源、节点匹配,补过一次就去掉
                            {

                                DataDomain dDomain = (DataDomain) (ht_price[consumeStr]);
                                decimal contractPrice = TransUtil.ToDecimal(dDomain.Name1);
                                decimal planPrice = TransUtil.ToDecimal(dDomain.Name2);
                                decimal responsiPrice = TransUtil.ToDecimal(dDomain.Name3);
                                decimal contractQuantity = TransUtil.ToDecimal(dDomain.Name4); //计划工程量
                                decimal planQuantity = TransUtil.ToDecimal(dDomain.Name5);
                                decimal responsiQuantity = TransUtil.ToDecimal(dDomain.Name6);
                                decimal contractTotalPrice = TransUtil.ToDecimal(dDomain.Name7);
                                decimal planTotalPrice = TransUtil.ToDecimal(dDomain.Name8);
                                decimal responsiTotalPrice = TransUtil.ToDecimal(dDomain.Name9);

                                dtlConsume.SumIncomeQuantity = decimal.Round(contractQuantity, 4);
                                dtlConsume.SumIncomeTotalPrice = decimal.Round(contractTotalPrice, 2);
                                dtlConsume.SumRealConsumePlanQuantity = decimal.Round(planQuantity, 4);
                                dtlConsume.SumRealConsumePlanTotalPrice = decimal.Round(planTotalPrice, 2);
                                dtlConsume.SumResponsiConsumeQuantity = decimal.Round(responsiQuantity, 4);
                                dtlConsume.SumResponsiConsumeTotalPrice = decimal.Round(responsiTotalPrice, 2);
                                ht_price.Remove(consumeStr);
                            }
                        }
                    }
                }
                //3：取ht_price中的剩余值
                foreach (string str in ht_price.Keys)
                {
                    string taskID = str.Split('-')[0];
                    string resourceID = str.Split('-')[1];
                    string subjectID = str.Split('-')[2];
                    string subjectCode = ht_subject[subjectID] as string;
                    if (ifMoneyCal(subjectCode) == true)
                    {
                        continue;
                    }
                    DataDomain dDomain = (DataDomain) ht_price[str];
                    CostMonthAccountDtl detail = new CostMonthAccountDtl();
                    GWBSTree gwbs = new GWBSTree();
                    gwbs.Version = 0;
                    gwbs.Id = taskID;
                    detail.AccountTaskNodeGUID = gwbs;
                    detail.AccountTaskNodeName = TransUtil.ToString(dDomain.Name20);
                    detail.AccountTaskNodeSyscode = TransUtil.ToString(dDomain.Name21);
                    CostAccountSubject subject = new CostAccountSubject();
                    subject.Id = subjectID;
                    subject.Version = 0;
                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    dtlConsume.ResourceTypeGUID = resourceID;
                    dtlConsume.ResourceTypeCode = TransUtil.ToString(dDomain.Name22);
                    dtlConsume.ResourceTypeName = TransUtil.ToString(dDomain.Name23);
                    dtlConsume.ResourceTypeStuff = TransUtil.ToString(dDomain.Name24);
                    dtlConsume.ResourceTypeSpec = TransUtil.ToString(dDomain.Name25);
                    dtlConsume.ResourceSyscode = TransUtil.ToString(dDomain.Name26);
                    dtlConsume.CostingSubjectGUID = subject;
                    dtlConsume.CostSubjectCode = ht_subject[subjectID] as string;
                    dtlConsume.CostingSubjectName = TransUtil.ToString(dDomain.Name27);
                    dtlConsume.CostSubjectSyscode = TransUtil.ToString(dDomain.Name28);

                    dtlConsume.SumIncomeQuantity = decimal.Round(TransUtil.ToDecimal(dDomain.Name4), 4);
                    dtlConsume.SumIncomeTotalPrice = decimal.Round(TransUtil.ToDecimal(dDomain.Name7), 2);
                    dtlConsume.SumRealConsumePlanQuantity = decimal.Round(TransUtil.ToDecimal(dDomain.Name5), 4);
                    dtlConsume.SumRealConsumePlanTotalPrice = decimal.Round(TransUtil.ToDecimal(dDomain.Name8), 2);
                    dtlConsume.SumResponsiConsumeQuantity = decimal.Round(TransUtil.ToDecimal(dDomain.Name6), 4);
                    dtlConsume.SumResponsiConsumeTotalPrice = decimal.Round(TransUtil.ToDecimal(dDomain.Name9), 2);

                    dtlConsume.TheAccountDetail = detail;
                    detail.Details.Add(dtlConsume);
                    detail.TheAccountBill = costBill;
                    costBill.Details.Add(detail);
                }
            }

            //剔除无效数据
            IList delList = new ArrayList();
            foreach (CostMonthAccountDtl detail in costBill.Details)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in detail.Details)
                {
                    if (dtlConsume.CurrRealConsumeTotalPrice == 0 && dtlConsume.CurrIncomeTotalPrice == 0 &&
                        dtlConsume.SumRealConsumeTotalPrice == 0
                        && dtlConsume.SumResponsiConsumeTotalPrice == 0 && dtlConsume.SumRealConsumePlanTotalPrice == 0 &&
                        dtlConsume.SumIncomeTotalPrice == 0)
                    {
                        delList.Add(dtlConsume);
                    }
                }
            }
            foreach (CostMonthAccDtlConsume dtlConsume in delList)
            {
                foreach (CostMonthAccountDtl detail in costBill.Details)
                {
                    detail.Details.Remove(dtlConsume);
                }
            }
            //8.2 更新本月相关数据
            foreach (CostMonthAccountDtl detail in costBill.Details)
            {
                decimal sumCurrRealTotalPrice = 0;
                decimal sumCurrRealQuantity = 0;
                decimal sumCurrResponsiConsumeQuantity = 0;
                decimal sumCurrResponsiConsumeTotalPrice = 0;
                decimal sumCurrIncomeQuantity = 0;
                decimal sumCurrIncomeTotalPrice = 0;
                decimal sumAddRealTotalPrice = 0;
                decimal sumAddRealQuantity = 0;
                decimal sumAddResponsiConsumeQuantity = 0;
                decimal sumAddResponsiConsumeTotalPrice = 0;
                decimal sumAddIncomeQuantity = 0;
                decimal sumAddIncomeTotalPrice = 0;
                foreach (CostMonthAccDtlConsume dtlConsume in detail.Details)
                {
                    sumCurrRealTotalPrice += dtlConsume.CurrRealConsumeTotalPrice;
                    sumCurrRealQuantity += dtlConsume.CurrRealConsumeQuantity;
                    sumCurrResponsiConsumeQuantity += dtlConsume.CurrResponsiConsumeQuantity;
                    sumCurrResponsiConsumeTotalPrice += dtlConsume.CurrResponsiConsumeTotalPrice;
                    sumCurrIncomeQuantity += dtlConsume.CurrIncomeQuantity;
                    sumCurrIncomeTotalPrice += dtlConsume.CurrIncomeTotalPrice;
                    sumAddRealTotalPrice += dtlConsume.SumRealConsumeTotalPrice;
                    sumAddRealQuantity += dtlConsume.SumRealConsumeQuantity;
                    sumAddResponsiConsumeQuantity += dtlConsume.SumResponsiConsumeQuantity;
                    sumAddResponsiConsumeTotalPrice += dtlConsume.SumResponsiConsumeTotalPrice;
                    sumAddIncomeQuantity += dtlConsume.SumIncomeQuantity;
                    sumAddIncomeTotalPrice += dtlConsume.SumIncomeTotalPrice;
                }
                detail.CurrRealTotalPrice = sumCurrRealTotalPrice;
                detail.CurrRealQuantity = sumCurrRealQuantity;
                detail.CurrResponsiQuantity = sumCurrResponsiConsumeQuantity;
                detail.CurrResponsiTotalPrice = sumCurrResponsiConsumeTotalPrice;
                detail.CurrIncomeQuantity = sumCurrIncomeQuantity;
                detail.CurrIncomeTotalPrice = sumCurrIncomeTotalPrice;
                if (detail.CurrRealQuantity != 0)
                {
                    detail.CurrRealPrice = decimal.Round(detail.CurrRealTotalPrice/detail.CurrRealQuantity, 4);
                }
                detail.SumIncomeQuantity = sumAddIncomeQuantity;
                detail.SumIncomeTotalPrice = sumAddIncomeTotalPrice;
                detail.SumRealQuantity = sumAddRealQuantity;
                detail.SumRealTotalPrice = sumAddRealTotalPrice;
                detail.SumResponsiQuantity = sumAddResponsiConsumeQuantity;
                detail.SumResponsiTotalPrice = sumAddResponsiConsumeTotalPrice;
            }

            #region old保存方法

            //Dao.Save(costBill);

            ////8.2 回写工程任务核算/分包结算/设备租赁结算/料具结算/费用结算的标志
            //Hashtable ht_forwardbill = new Hashtable();
            //ht_forwardbill.Add("0", list_PTaskAccount);
            //ht_forwardbill.Add("1", list_SubBalance);
            //ht_forwardbill.Add("2", list_resourceConsume);
            //ht_forwardbill.Add("3", list_machineConsume);
            //ht_forwardbill.Add("5", list_expenseConsume);
            //this.WriteForwardBillFlag(ht_forwardbill, costBill.Id);

            #endregion

            #region new保存方法

            Hashtable ht_forwardbill = new Hashtable();
            ht_forwardbill.Add("0", list_PTaskAccount);
            ht_forwardbill.Add("1", list_SubBalance);
            ht_forwardbill.Add("2", list_resourceConsume);
            ht_forwardbill.Add("3", list_machineConsume);
            ht_forwardbill.Add("5", list_expenseConsume);
            this.InsertCostMonthAccountNew(costBill, ht_forwardbill);
            //costBill = Dao.Get(typeof(CostMonthAccountBill), costBill.Id) as CostMonthAccountBill;//未定是否需要重新查询一次

            #endregion

            #endregion

            return "";
        }

        //简化实际耗用算法
        private Hashtable RealConsumeSimpleCal(CostMonthAccountBill costBill, Hashtable ht_monthdtl,
                                               Hashtable ht_monthdtlconsume,
                                               Hashtable ht_sumConsumeDomain, CurrentProjectInfo projectInfo,
                                               IList list_GWBS, Hashtable ht_subject)
        {
            //对汇总后的每一条物资耗用对象
            /*
             * 1、如果是分包结单单：找本节点或者最近父节点 分包结算单任务明细与月度成本消耗任务明细相同、资源ID和科目相同 分包结算单所属工程是当前月度资源耗用所属工程本节点或者最近的父节点 
             * 2、找本节点或者最近父节点 
             * 3、如果是分包结算单：找子节点   分包结算单任务明细与月度成本消耗任务明细相同、资源ID和科目相同  分包结单所属工程是月度资源耗用所属工程子节点(最近) 
             * 4、找子节点
             */
            foreach (string dtlConsumeLinkStr in ht_sumConsumeDomain.Keys)
            {
                try
                {
                    //1: 取汇总对象中的相关数据
                    DataDomain consumeDataDomain = (DataDomain) ht_sumConsumeDomain[dtlConsumeLinkStr]; //数量+"-"+合价
                    CostAccountSubject costSubject = (CostAccountSubject) consumeDataDomain.Name7;
                    decimal addConsumeMoney = TransUtil.ToDecimal(consumeDataDomain.Name2);
                    decimal addConsumeQty = TransUtil.ToDecimal(consumeDataDomain.Name1);
                    bool queryFlag = false;

                    //2: 通过[工程任务节点+资源+科目]查询最近节点进行挂接
                    string currSyscode = this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 1); //当前物资耗用的GWBS层次码
                    int syscodeLength = 0;
                    string temp_linkStr = "";
                    //如果是分包结算单，要求工程任务明细相同
                    if (TransUtil.ToString(consumeDataDomain.Name21) != "")
                    {
                        foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
                        {
                            CostMonthAccDtlConsume dtlConsume =
                                (CostMonthAccDtlConsume) ht_monthdtlconsume[month_dtConsumelinkStr];
                            string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
                            int tempLength = monthSyscode.Length;
                            //2.1 先找本节点和父节点
                            if (currSyscode.Contains(monthSyscode)
                                &&
                                this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 3).Equals(
                                    this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 2))
                                && dtlConsume.ProjectTaskDtlGUID == TransUtil.ToString(consumeDataDomain.Name21))
                            {
                                if (syscodeLength == 0 || tempLength > syscodeLength)
                                {
                                    syscodeLength = monthSyscode.Length;
                                    temp_linkStr = month_dtConsumelinkStr;
                                }
                                queryFlag = true;
                            }
                        }
                    }

                    if (queryFlag == false)
                    {
                        foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
                        {
                            CostMonthAccDtlConsume dtlConsume =
                                (CostMonthAccDtlConsume) ht_monthdtlconsume[month_dtConsumelinkStr];
                            string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
                            int tempLength = monthSyscode.Length;
                            //2.1 先找本节点和父节点
                            if (currSyscode.Contains(monthSyscode)
                                &&
                                this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 3).Equals(
                                    this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 2)))
                            {
                                if (syscodeLength == 0 || tempLength > syscodeLength)
                                {
                                    syscodeLength = monthSyscode.Length;
                                    temp_linkStr = month_dtConsumelinkStr;
                                }
                                queryFlag = true;
                            }
                        }
                    }

                    //2.2 再找子节点
                    if (queryFlag == false)
                    {
                        //如果是分包结算单，要求工程任务明细相同
                        if (TransUtil.ToString(consumeDataDomain.Name21) != "")
                        {
                            foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
                            {
                                CostMonthAccDtlConsume dtlConsume =
                                    (CostMonthAccDtlConsume) ht_monthdtlconsume[month_dtConsumelinkStr];
                                string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
                                int tempLength = monthSyscode.Length;
                                if (monthSyscode.Contains(currSyscode)
                                    &&
                                    this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 3).Equals(
                                        this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 2))
                                    && dtlConsume.ProjectTaskDtlGUID == TransUtil.ToString(consumeDataDomain.Name21))
                                {
                                    if (syscodeLength == 0 || tempLength < syscodeLength)
                                    {
                                        syscodeLength = monthSyscode.Length;
                                        temp_linkStr = month_dtConsumelinkStr;
                                    }
                                    queryFlag = true;
                                }
                            }
                        }
                        if (queryFlag == false)
                        {
                            foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
                            {
                                CostMonthAccDtlConsume dtlConsume =
                                    (CostMonthAccDtlConsume) ht_monthdtlconsume[month_dtConsumelinkStr];
                                string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
                                int tempLength = monthSyscode.Length;
                                if (monthSyscode.Contains(currSyscode)
                                    &&
                                    this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 3).Equals(
                                        this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 2)))
                                {
                                    if (syscodeLength == 0 || tempLength < syscodeLength)
                                    {
                                        syscodeLength = monthSyscode.Length;
                                        temp_linkStr = month_dtConsumelinkStr;
                                    }
                                    queryFlag = true;
                                }
                            }
                        }
                    }
                    //2.3 已经找到相同的[工程任务节点+资源+科目]信息
                    if (queryFlag == true)
                    {
                        CostMonthAccDtlConsume curr_dtlConsume =
                            (CostMonthAccDtlConsume) ht_monthdtlconsume[temp_linkStr];
                        curr_dtlConsume = this.TransThirdCalValueByConsume(costSubject, currSyscode, curr_dtlConsume,
                                                                           list_GWBS, addConsumeQty, addConsumeMoney);
                        curr_dtlConsume.CurrRealConsumeQuantity += addConsumeQty;
                        curr_dtlConsume.CurrRealConsumeTotalPrice += addConsumeMoney;
                        if (curr_dtlConsume.CurrRealConsumeQuantity != 0)
                        {
                            curr_dtlConsume.CurrRealConsumePrice =
                                decimal.Round(
                                    curr_dtlConsume.CurrRealConsumeTotalPrice/curr_dtlConsume.CurrRealConsumeQuantity, 4);
                        }
                    }
                    //3: 如果[工程任务节点+资源+科目]未查到,则新建记录挂接到最近节点(包括当前核算节点)
                    if (queryFlag == false)
                    {
                        syscodeLength = 0;
                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        CostMonthAccountDtl dtl = new CostMonthAccountDtl();
                        string temp_comsumeLinkStr = "";
                        //3.1 查找本节点和父节点
                        foreach (string month_dtlLinkStr in ht_monthdtl.Keys)
                        {
                            dtl = (CostMonthAccountDtl) ht_monthdtl[month_dtlLinkStr];
                            string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtlLinkStr, 1);
                            int tempLength = monthSyscode.Length;
                            if (currSyscode.Contains(monthSyscode)) //&& dtl.CurrEarnValue != 0
                            {
                                if (syscodeLength == 0 || tempLength > syscodeLength)
                                {
                                    syscodeLength = monthSyscode.Length;
                                    dtlConsume.ProjectTaskDtlGUID = dtl.ProjectTaskDtlGUID.Id;
                                    dtlConsume.CurrRealConsumeQuantity = addConsumeQty;
                                    dtlConsume.CurrRealConsumeTotalPrice = addConsumeMoney;
                                    if (dtlConsume.CurrRealConsumeQuantity != 0)
                                    {
                                        dtlConsume.CurrRealConsumePrice =
                                            decimal.Round(
                                                dtlConsume.CurrRealConsumeTotalPrice/dtlConsume.CurrRealConsumeQuantity,
                                                4);
                                    }
                                    dtlConsume.ResourceTypeGUID = TransUtil.ToString(consumeDataDomain.Name3);
                                    dtlConsume.ResourceTypeName = TransUtil.ToString(consumeDataDomain.Name4);
                                    dtlConsume.ResourceSyscode = TransUtil.ToString(consumeDataDomain.Name5);
                                    dtlConsume.RationUnitGUID =
                                        (Application.Resource.MaterialResource.Domain.StandardUnit)
                                        consumeDataDomain.Name6;
                                    if (dtlConsume.RationUnitGUID != null)
                                    {
                                        dtlConsume.RationUnitName = dtlConsume.RationUnitGUID.Name;
                                    }
                                    dtlConsume.CostingSubjectGUID =
                                        (
                                        Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain.
                                            CostAccountSubject) consumeDataDomain.Name7;
                                    dtlConsume.CostingSubjectName = TransUtil.ToString(consumeDataDomain.Name8);
                                    dtlConsume.CostSubjectSyscode = TransUtil.ToString(consumeDataDomain.Name9);
                                    dtlConsume.TheAccountDetail = dtl;
                                    temp_comsumeLinkStr = dtl.AccountTaskNodeSyscode + "-" + dtlConsume.ResourceTypeGUID +
                                                          "-" + dtlConsume.CostingSubjectGUID.Id;
                                }
                                queryFlag = true;
                            }
                        }
                        if (queryFlag == false)
                        {
                            syscodeLength = 0;
                            //3.2 子节点
                            foreach (string month_dtlLinkStr in ht_monthdtl.Keys)
                            {
                                dtl = (CostMonthAccountDtl) ht_monthdtl[month_dtlLinkStr];
                                string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtlLinkStr, 1);
                                int tempLength = monthSyscode.Length;
                                if (monthSyscode.Contains(currSyscode)) //&& dtl.CurrEarnValue != 0
                                {
                                    if (syscodeLength == 0 || tempLength < syscodeLength)
                                    {
                                        syscodeLength = monthSyscode.Length;
                                        dtlConsume.ProjectTaskDtlGUID = dtl.ProjectTaskDtlGUID.Id;
                                        dtlConsume.CurrRealConsumeQuantity = addConsumeQty;
                                        dtlConsume.CurrRealConsumeTotalPrice = addConsumeMoney;
                                        if (dtlConsume.CurrRealConsumeQuantity != 0)
                                        {
                                            dtlConsume.CurrRealConsumePrice =
                                                decimal.Round(
                                                    dtlConsume.CurrRealConsumeTotalPrice/
                                                    dtlConsume.CurrRealConsumeQuantity, 4);
                                        }
                                        dtlConsume.ResourceTypeGUID = TransUtil.ToString(consumeDataDomain.Name3);
                                        dtlConsume.ResourceTypeName = TransUtil.ToString(consumeDataDomain.Name4);
                                        dtlConsume.ResourceSyscode = TransUtil.ToString(consumeDataDomain.Name5);
                                        dtlConsume.RationUnitGUID =
                                            (Application.Resource.MaterialResource.Domain.StandardUnit)
                                            consumeDataDomain.Name6;
                                        if (dtlConsume.RationUnitGUID != null)
                                        {
                                            dtlConsume.RationUnitName = dtlConsume.RationUnitGUID.Name;
                                        }
                                        dtlConsume.CostingSubjectGUID =
                                            (
                                            Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain.
                                                CostAccountSubject) consumeDataDomain.Name7;
                                        dtlConsume.CostingSubjectName = TransUtil.ToString(consumeDataDomain.Name8);
                                        dtlConsume.CostSubjectSyscode = TransUtil.ToString(consumeDataDomain.Name9);
                                        dtlConsume.TheAccountDetail = dtl;
                                        temp_comsumeLinkStr = dtl.AccountTaskNodeSyscode + "-" +
                                                              dtlConsume.ResourceTypeGUID + "-" +
                                                              dtlConsume.CostingSubjectGUID.Id;
                                    }
                                    queryFlag = true;
                                }
                            }
                        }
                        if (queryFlag == false && currSyscode.Contains(costBill.AccountTaskSysCode) == true)
                        {
                            Hashtable ht_str = new Hashtable();
                            string str = "[工程任务名称: " + consumeDataDomain.Name20 + "][资源: " + consumeDataDomain.Name4 +
                                         "][科目: " + consumeDataDomain.Name8 + "]未找到其归属的核算节点信息,请先在预算体系中设置！";
                            ht_str.Add("error", str);
                            return ht_str;
                        }
                        if (queryFlag == true)
                        {
                            dtlConsume = this.TransThirdCalValueByConsume(costSubject, currSyscode, dtlConsume,
                                                                          list_GWBS, addConsumeQty, addConsumeMoney);
                            if (ht_monthdtlconsume.Contains(temp_comsumeLinkStr))
                            {
                                CostMonthAccDtlConsume curr_dtlConsume =
                                    (CostMonthAccDtlConsume) ht_monthdtlconsume[temp_comsumeLinkStr];
                                curr_dtlConsume.CurrRealConsumeQuantity += addConsumeQty;
                                curr_dtlConsume.CurrRealConsumeTotalPrice += addConsumeMoney;
                                if (curr_dtlConsume.CurrRealConsumeQuantity != 0)
                                {
                                    curr_dtlConsume.CurrRealConsumePrice =
                                        decimal.Round(
                                            curr_dtlConsume.CurrRealConsumeTotalPrice/
                                            curr_dtlConsume.CurrRealConsumeQuantity, 4);
                                }
                                curr_dtlConsume.CurrIncomeQuantity += dtlConsume.CurrIncomeQuantity;
                                curr_dtlConsume.CurrIncomeTotalPrice += dtlConsume.CurrIncomeTotalPrice;
                                curr_dtlConsume.CurrRealConsumePlanQuantity += dtlConsume.CurrRealConsumePlanQuantity;
                                curr_dtlConsume.CurrRealConsumePlanTotalPrice +=
                                    dtlConsume.CurrRealConsumePlanTotalPrice;
                                curr_dtlConsume.CurrResponsiConsumeQuantity += dtlConsume.CurrResponsiConsumeQuantity;
                                curr_dtlConsume.CurrResponsiConsumeTotalPrice +=
                                    dtlConsume.CurrResponsiConsumeTotalPrice;
                            }
                            else
                            {
                                ht_monthdtlconsume.Add(temp_comsumeLinkStr, dtlConsume);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
            return ht_monthdtlconsume;
        }

        //确定非实体的形象进度和过程量
        private CostMonthAccDtlConsume TransThirdCalValueByConsume(CostAccountSubject costSubject, string currSyscode,
                                                                   CostMonthAccDtlConsume dtlConsume, IList list_GWBS,
                                                                   decimal addConsumeQty, decimal addConsumeMoney)
        {
            //C5120302安全施工材料费C51202文明施工费C5110124安装措施费C5120803支架材料费C5120902脚手架材料费C513现场管理费C514	规费C515	税金C5110125	劳务税金C51103	施工机械费
            if (costSubject.Code.IndexOf("C513") != -1 || costSubject.Code.IndexOf("C514") != -1 ||
                costSubject.Code.IndexOf("C515") != -1 || costSubject.Code.IndexOf("C51103") != -1
                || costSubject.Code.IndexOf("C5120803") != -1 || costSubject.Code.IndexOf("C5120902") != -1 ||
                costSubject.Code.IndexOf("C5120302") != -1
                || costSubject.Code.IndexOf("C5110124") != -1 || costSubject.Code.IndexOf("C5110125") != -1 ||
                costSubject.Code.IndexOf("C51202") != -1)
            {
                decimal sumPlanQty = 0;
                decimal sumPlanMny = 0;
                decimal sumIncomeQty = 0;
                decimal sumIncomeMny = 0;
                decimal sumResponsiQty = 0;
                decimal sumResponsiMny = 0;

                foreach (GWBSTree gwbs in list_GWBS)
                {
                    if (gwbs.SysCode.Contains(currSyscode))
                    {
                        foreach (GWBSDetail dtl in gwbs.Details)
                        {
                            foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
                            {
                                if (subject.CostAccountSubjectGUID != null &&
                                    subject.CostAccountSubjectGUID.Code.IndexOf(costSubject.Code) != -1)
                                {
                                    sumPlanQty += subject.PlanWorkAmount;
                                    sumPlanMny += subject.PlanTotalPrice;
                                    sumIncomeQty += subject.ContractProjectAmount;
                                    sumIncomeMny += subject.ContractTotalPrice;
                                    sumResponsiQty += subject.ResponsibilitilyWorkAmount;
                                    sumResponsiMny += subject.ResponsibilitilyTotalPrice;
                                }
                            }
                        }
                    }
                }
                decimal percent = 0;
                if (costSubject.Code.IndexOf("C5120803") != -1 || costSubject.Code.IndexOf("C5120902") != -1)
                {
                    if (sumPlanQty != 0)
                    {
                        percent = decimal.Round(addConsumeQty/sumPlanQty, 4);
                    }

                }
                else
                {
                    if (sumPlanMny != 0)
                    {
                        percent = decimal.Round(addConsumeMoney/sumPlanMny, 4);
                    }
                }
                if (percent > 1)
                {
                    percent = 1;
                }
                else if (percent < -1)
                {
                    percent = -1;
                }
                if (sumPlanMny == 0 && percent == 0)
                    //if (costSubject.Code.IndexOf("C514") != -1 || (sumPlanMny == 0  && percent == 0))
                {
                    dtlConsume.CurrRealConsumePlanQuantity += addConsumeQty;
                    dtlConsume.CurrRealConsumePlanTotalPrice += addConsumeMoney;
                    dtlConsume.CurrResponsiConsumeQuantity += addConsumeQty;
                    dtlConsume.CurrResponsiConsumeTotalPrice += addConsumeMoney;
                    dtlConsume.CurrIncomeQuantity += addConsumeQty;
                    dtlConsume.CurrIncomeTotalPrice += addConsumeMoney;
                }
                else
                {
                    dtlConsume.CurrRealConsumePlanQuantity += sumPlanQty*percent;
                    dtlConsume.CurrRealConsumePlanTotalPrice += sumPlanMny*percent;
                    dtlConsume.CurrResponsiConsumeQuantity += sumResponsiQty*percent;
                    dtlConsume.CurrResponsiConsumeTotalPrice += sumResponsiMny*percent;
                    dtlConsume.CurrIncomeQuantity += sumIncomeQty*percent;
                    dtlConsume.CurrIncomeTotalPrice += sumIncomeMny*percent;
                }

            }
            return dtlConsume;
        }

        private CostMonthAccDtlConsume TransCostMonthAccDtlConsume(CostMonthAccDtlConsume dtlConsume)
        {
            CostMonthAccDtlConsume newDtlConsume = new CostMonthAccDtlConsume();
            newDtlConsume.CostingSubjectGUID = dtlConsume.CostingSubjectGUID;
            newDtlConsume.CostingSubjectName = dtlConsume.CostingSubjectName;
            newDtlConsume.CostSubjectCode = dtlConsume.CostSubjectCode;
            newDtlConsume.CostSubjectSyscode = dtlConsume.CostSubjectSyscode;
            //newDtlConsume.CurrIncomeQuantity = dtlConsume.CurrIncomeQuantity;
            //newDtlConsume.CurrIncomeTotalPrice = dtlConsume.CurrIncomeTotalPrice;
            //newDtlConsume.CurrRealConsumePlanQuantity = dtlConsume.CurrRealConsumePlanQuantity;
            //newDtlConsume.CurrRealConsumePlanTotalPrice = dtlConsume.CurrRealConsumePlanTotalPrice;
            //newDtlConsume.CurrRealConsumePrice = dtlConsume.CurrRealConsumePrice;
            //newDtlConsume.CurrRealConsumeQuantity = dtlConsume.CurrRealConsumeQuantity;
            //newDtlConsume.CurrRealConsumeTotalPrice = dtlConsume.CurrRealConsumeTotalPrice;
            //newDtlConsume.CurrResponsiConsumeQuantity = dtlConsume.CurrResponsiConsumeQuantity;
            //newDtlConsume.CurrResponsiConsumeTotalPrice = dtlConsume.CurrResponsiConsumeTotalPrice;
            newDtlConsume.ProjectTaskDtlGUID = dtlConsume.ProjectTaskDtlGUID;
            newDtlConsume.ProjectTaskDtlName = dtlConsume.ProjectTaskDtlName;
            newDtlConsume.RationUnitGUID = dtlConsume.RationUnitGUID;
            newDtlConsume.RationUnitName = dtlConsume.RationUnitName;
            newDtlConsume.ResourceSyscode = dtlConsume.ResourceSyscode;
            newDtlConsume.ResourceTypeCode = dtlConsume.ResourceTypeCode;
            newDtlConsume.ResourceTypeGUID = dtlConsume.ResourceTypeGUID;
            newDtlConsume.ResourceTypeName = dtlConsume.ResourceTypeName;
            newDtlConsume.ResourceTypeSpec = dtlConsume.ResourceTypeSpec;
            newDtlConsume.ResourceTypeStuff = dtlConsume.ResourceTypeStuff;
            newDtlConsume.SumIncomeQuantity = dtlConsume.SumIncomeQuantity;
            newDtlConsume.SumIncomeTotalPrice = dtlConsume.SumIncomeTotalPrice;
            newDtlConsume.SumRealConsumePlanQuantity = dtlConsume.SumRealConsumePlanQuantity;
            newDtlConsume.SumRealConsumePlanTotalPrice = dtlConsume.SumRealConsumePlanTotalPrice;
            newDtlConsume.SumRealConsumeQuantity = dtlConsume.SumRealConsumeQuantity;
            newDtlConsume.SumRealConsumeTotalPrice = dtlConsume.SumRealConsumeTotalPrice;
            newDtlConsume.SumResponsiConsumeQuantity = dtlConsume.SumResponsiConsumeQuantity;
            newDtlConsume.SumResponsiConsumeTotalPrice = dtlConsume.SumResponsiConsumeTotalPrice;

            return newDtlConsume;
        }

        private CostMonthAccountDtl TransCostMonthAccountDtl(CostMonthAccountDtl dtl)
        {
            CostMonthAccountDtl newDtl = new CostMonthAccountDtl();
            newDtl.AccountTaskNodeGUID = dtl.AccountTaskNodeGUID;
            newDtl.AccountTaskNodeName = dtl.AccountTaskNodeName;
            newDtl.AccountTaskNodeSyscode = dtl.AccountTaskNodeSyscode;
            newDtl.CostItemName = dtl.CostItemName;
            //newDtl.CurrCompletedPercent = dtl.CurrCompletedPercent;
            //newDtl.CurrEarnValue = dtl.CurrEarnValue;
            //newDtl.CurrIncomeQuantity = dtl.CurrIncomeQuantity;
            //newDtl.CurrIncomeTotalPrice = dtl.CurrIncomeTotalPrice;
            //newDtl.CurrRealPrice = dtl.CurrRealPrice;
            //newDtl.CurrRealQuantity = dtl.CurrRealQuantity;
            //newDtl.CurrRealTotalPrice = dtl.CurrRealTotalPrice;
            //newDtl.CurrResponsiQuantity = dtl.CurrResponsiQuantity;
            //newDtl.CurrResponsiTotalPrice = dtl.CurrResponsiTotalPrice;
            newDtl.IfTaskAcctMx = dtl.IfTaskAcctMx;
            newDtl.PriceUnitGUID = dtl.PriceUnitGUID;
            newDtl.PriceUnitName = dtl.PriceUnitName;
            newDtl.ProjectTaskDtlGUID = dtl.ProjectTaskDtlGUID;
            newDtl.ProjectTaskDtlName = dtl.ProjectTaskDtlName;
            newDtl.QuantityUnitGUID = dtl.QuantityUnitGUID;
            newDtl.QuantityUnitName = dtl.QuantityUnitName;
            newDtl.SumCompletedPercent = dtl.SumCompletedPercent;
            newDtl.SumEarnValue = dtl.SumEarnValue;
            newDtl.SumIncomeQuantity = dtl.SumIncomeQuantity;
            newDtl.SumIncomeTotalPrice = dtl.SumIncomeTotalPrice;
            newDtl.SumRealQuantity = dtl.SumRealQuantity;
            newDtl.SumRealTotalPrice = dtl.SumRealTotalPrice;
            newDtl.SumResponsiQuantity = dtl.SumResponsiQuantity;
            newDtl.SumResponsiTotalPrice = dtl.SumResponsiTotalPrice;
            newDtl.TheCostItem = dtl.TheCostItem;

            return newDtl;
        }

        #endregion

        #region 月度成本核算算法

        /// <summary>
        /// 月度成本核算被调用主程序
        /// </summary>
        [TransManager]
        private void CostMonthAccountCalByCall(CostMonthAccountBill costBill, CurrentProjectInfo projectInfo)
        {
            Hashtable ht_monthdtl = new Hashtable();
            Hashtable ht_monthdtlconsume = new Hashtable();
            IList list_log = new ArrayList(); //日志集合

            #region 1：构造本月成本核算主表

            //获取科目编码信息
            Hashtable ht_subject = this.GetCostSubjectList();

            #endregion

            #region 2：通过核算范围节点构造<核算节点集合><工程任务核算明细集合><核算明细资源耗用集合>

            //2.1 从GWBS树中查询到核算节点下的所有核算节点(包括本身)
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", costBill.ProjectId));
            oq.AddCriterion(Expression.Like("SysCode", costBill.AccountTaskSysCode + "%"));
            oq.AddCriterion(Expression.Eq("CostAccFlag", true));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails.CostingSubjectGUID", NHibernate.FetchMode.Eager);
            IList list_GWBS = Dao.ObjectQuery(typeof (GWBSTree), oq);
            //Hashtable

            //2.2 构造<核算节点集合><工程任务核算明细集合><核算明细资源耗用集合>
            foreach (GWBSTree model in list_GWBS)
            {
                foreach (GWBSDetail detail in model.Details)
                {
                    if (detail.CostingFlag == 0)
                        continue;
                    CostMonthAccountDtl costDtl = new CostMonthAccountDtl();
                    costDtl.AccountTaskNodeGUID = model;
                    costDtl.AccountTaskNodeName = model.Name;
                    costDtl.AccountTaskNodeSyscode = model.SysCode;
                    costDtl.ProjectTaskDtlGUID = detail;
                    costDtl.ProjectTaskDtlName = detail.Name;
                    costDtl.TheCostItem = detail.TheCostItem;
                    costDtl.CostItemName = detail.TheCostItem.Name;
                    ht_monthdtl.Add(model.SysCode + "-" + detail.Id, costDtl);

                    foreach (GWBSDetailCostSubject sonDetail in detail.ListCostSubjectDetails)
                    {
                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        if (sonDetail.CostAccountSubjectGUID != null)
                        {
                            dtlConsume.CostingSubjectName = sonDetail.CostAccountSubjectName;
                            dtlConsume.CostSubjectSyscode = sonDetail.CostAccountSubjectGUID.SysCode;
                            dtlConsume.CostSubjectCode = sonDetail.CostAccountSubjectGUID.Code;
                            dtlConsume.CostingSubjectGUID = sonDetail.CostAccountSubjectGUID;
                        }
                        else
                        {
                            continue;
                        }
                        dtlConsume.ResourceTypeGUID = sonDetail.ResourceTypeGUID;
                        dtlConsume.ResourceTypeName = sonDetail.ResourceTypeName;
                        dtlConsume.RationUnitGUID = sonDetail.ProjectAmountUnitGUID;
                        dtlConsume.RationUnitName = sonDetail.ProjectAmountUnitName;
                        dtlConsume.ProjectTaskDtlGUID = detail.Id;
                        dtlConsume.ProjectTaskDtlName = detail.Name;
                        dtlConsume.TheAccountDetail = costDtl;
                        dtlConsume.Data1 = sonDetail.PlanTotalPrice + ""; //计划耗用合价
                        string dtlConsumeLinkStr = model.SysCode + "-" + detail.Id + "-" + sonDetail.ResourceTypeGUID +
                                                   "-" + sonDetail.CostAccountSubjectGUID.Id;
                        if (!ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                        {
                            ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                        }
                    }
                }
            }

            #endregion

            #region 3：工程任务核算单的月度汇总

            //3.1 查询工程任务核算信息
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime.AddDays(1)));
            oq.AddCriterion(Expression.IsNull("MonthAccountBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_PTaskAccount = Dao.ObjectQuery(typeof (ProjectTaskAccountBill), oq);

            //3.2汇总工程任务核算信息
            Hashtable ht_sumTaskAccDtl = new Hashtable();
            Hashtable ht_sumDtlSubject = new Hashtable();
            foreach (ProjectTaskAccountBill model in list_PTaskAccount)
            {
                //工程任务核算明细信息
                foreach (ProjectTaskDetailAccount dtl in model.Details)
                {
                    if (ht_sumTaskAccDtl.Contains(dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id))
                    {
                        ProjectTaskDetailAccount pDtl =
                            (ProjectTaskDetailAccount)
                            ht_sumTaskAccDtl[dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id];
                        pDtl.AccountProjectAmount += dtl.AccountProjectAmount;
                        pDtl.AccountTotalPrice += dtl.AccountTotalPrice;
                        pDtl.CurrAccFigureProgress += dtl.CurrAccFigureProgress;
                        pDtl.CurrContractIncomeQny += dtl.CurrContractIncomeQny;
                        pDtl.CurrContractIncomeTotal += dtl.CurrContractIncomeTotal;
                        pDtl.CurrResponsibleCostQny += dtl.CurrResponsibleCostQny;
                        pDtl.CurrResponsibleCostTotal += dtl.CurrResponsibleCostTotal;
                        pDtl.CurrAccEV += dtl.CurrAccEV;
                        ht_sumTaskAccDtl.Remove(dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id);
                        ht_sumTaskAccDtl.Add(dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id, pDtl);
                    }
                    else
                    {
                        ProjectTaskDetailAccount pDtl = new ProjectTaskDetailAccount();
                        pDtl.AccountProjectAmount = dtl.AccountProjectAmount;
                        pDtl.AccountTotalPrice = dtl.AccountTotalPrice;
                        pDtl.CurrAccFigureProgress = dtl.CurrAccFigureProgress;
                        pDtl.CurrContractIncomeQny = dtl.CurrContractIncomeQny;
                        pDtl.CurrContractIncomeTotal = dtl.CurrContractIncomeTotal;
                        pDtl.CurrResponsibleCostQny = dtl.CurrResponsibleCostQny;
                        pDtl.CurrResponsibleCostTotal = dtl.CurrResponsibleCostTotal;
                        pDtl.CurrAccEV = dtl.CurrAccEV;
                        ht_sumTaskAccDtl.Add(dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id, pDtl);
                    }

                    //工程任务资源耗用信息
                    foreach (ProjectTaskDetailAccountSubject subject in dtl.Details)
                    {
                        if (subject.CostingSubjectGUID == null)
                            continue;
                        string linkStr = dtl.AccountTaskNodeSyscode + "-" + dtl.ProjectTaskDtlGUID.Id + "-" +
                                         subject.ResourceTypeGUID + "-" + subject.CostingSubjectGUID.Id;
                        if (ht_sumDtlSubject.Contains(linkStr))
                        {
                            ProjectTaskDetailAccountSubject pSubject =
                                (ProjectTaskDetailAccountSubject) ht_sumDtlSubject[linkStr];
                            pSubject.AccUsageQny += subject.AccUsageQny;
                            pSubject.AccountTotalPrice += subject.AccountTotalPrice;
                            pSubject.CurrContractIncomeQny += subject.CurrContractIncomeQny;
                            pSubject.CurrContractIncomeTotal += subject.CurrContractIncomeTotal;
                            pSubject.CurrResponsibleCostQny += subject.CurrResponsibleCostQny;
                            pSubject.CurrResponsibleCostTotal += subject.CurrResponsibleCostTotal;
                            ht_sumDtlSubject.Remove(linkStr);
                            ht_sumDtlSubject.Add(linkStr, pSubject);
                        }
                        else
                        {
                            ProjectTaskDetailAccountSubject pSubject = new ProjectTaskDetailAccountSubject();
                            pSubject.TheProjectGUID = dtl.ProjectTaskDtlGUID.Id; //借用
                            pSubject.CostingSubjectGUID = subject.CostingSubjectGUID;
                            pSubject.CostingSubjectName = subject.CostingSubjectName;
                            pSubject.ResourceTypeGUID = subject.ResourceTypeGUID;
                            pSubject.ResourceTypeName = subject.ResourceTypeName;
                            pSubject.ResourceTypeSpec = subject.ResourceTypeSpec;
                            pSubject.ResourceTypeQuality = subject.ResourceTypeQuality;
                            pSubject.QuantityUnitGUID = subject.QuantityUnitGUID;
                            pSubject.QuantityUnitName = subject.QuantityUnitName;
                            pSubject.AccUsageQny = subject.AccUsageQny;
                            pSubject.AccountTotalPrice = subject.AccountTotalPrice;
                            pSubject.CurrContractIncomeQny = subject.CurrContractIncomeQny;
                            pSubject.CurrContractIncomeTotal = subject.CurrContractIncomeTotal;
                            pSubject.CurrResponsibleCostQny = subject.CurrResponsibleCostQny;
                            pSubject.CurrResponsibleCostTotal = subject.CurrResponsibleCostTotal;
                            pSubject.TheAccountDetail = dtl;

                            ht_sumDtlSubject.Add(linkStr, pSubject);
                        }

                    }
                }
            }

            //3.3工程任务明细月度核算
            IList list_notGWBSMx = new ArrayList(); //在GWBS没找到对应的工程任务明细
            foreach (string dtlLinkStr in ht_sumTaskAccDtl.Keys)
            {
                if (ht_monthdtl.Contains(dtlLinkStr))
                {
                    CostMonthAccountDtl costDtl = (CostMonthAccountDtl) ht_monthdtl[dtlLinkStr];
                    ProjectTaskDetailAccount pDtl = (ProjectTaskDetailAccount) ht_sumTaskAccDtl[dtlLinkStr];
                    costDtl.CurrRealQuantity += pDtl.AccountProjectAmount;
                    costDtl.CurrEarnValue += pDtl.CurrAccEV;
                    costDtl.CurrCompletedPercent += pDtl.CurrAccFigureProgress;
                    costDtl.CurrIncomeQuantity += pDtl.CurrContractIncomeQny;
                    costDtl.CurrIncomeTotalPrice += pDtl.CurrContractIncomeTotal;
                    costDtl.CurrResponsiQuantity += pDtl.CurrResponsibleCostQny;
                    costDtl.CurrResponsiTotalPrice += pDtl.CurrResponsibleCostTotal;
                    costDtl.IfTaskAcctMx = 1; //写入工程任务核算明细标志
                    ht_monthdtl.Remove(dtlLinkStr);
                    ht_monthdtl.Add(dtlLinkStr, costDtl);
                }
                else
                {
                    list_notGWBSMx.Add((ProjectTaskDetailAccount) ht_sumTaskAccDtl[dtlLinkStr]);
                }
            }
            //是否抛出错误信息
            if (list_notGWBSMx.Count > 0)
            {
                //throw new Exception("");
            }
            //核算{月度资源耗用明细核算}数据
            foreach (string dtlConsumeLinkStr in ht_sumDtlSubject.Keys)
            {
                ProjectTaskDetailAccountSubject dtlAccount =
                    (ProjectTaskDetailAccountSubject) ht_sumDtlSubject[dtlConsumeLinkStr];
                if (ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                {
                    CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                    dtlConsume.CurrIncomeQuantity += dtlAccount.CurrContractIncomeQny;
                    dtlConsume.CurrIncomeTotalPrice += dtlAccount.CurrContractIncomeTotal;
                    dtlConsume.CurrRealConsumePlanQuantity += dtlAccount.AccUsageQny;
                    dtlConsume.CurrRealConsumePlanTotalPrice += dtlAccount.AccountTotalPrice;
                    dtlConsume.CurrResponsiConsumeQuantity += dtlAccount.CurrResponsibleCostQny;
                    dtlConsume.CurrResponsiConsumeTotalPrice += dtlAccount.CurrResponsibleCostTotal;
                    ht_monthdtlconsume.Remove(dtlConsumeLinkStr);
                    ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                }
                else
                {
//新生成一个
                    CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                    dtlConsume.ProjectTaskDtlGUID = dtlAccount.TheProjectGUID;
                    dtlConsume.CostingSubjectGUID = dtlAccount.CostingSubjectGUID;
                    dtlConsume.CostingSubjectName = dtlAccount.CostingSubjectName;
                    dtlConsume.CostSubjectCode = dtlAccount.CostingSubjectGUID.Code;
                    dtlConsume.CostSubjectSyscode = dtlAccount.CostingSubjectGUID.SysCode;
                    dtlConsume.ResourceTypeGUID = dtlAccount.ResourceTypeGUID;
                    dtlConsume.ResourceTypeName = dtlAccount.ResourceTypeName;
                    dtlConsume.ResourceTypeSpec = dtlAccount.ResourceTypeSpec;
                    dtlConsume.ResourceTypeStuff = dtlAccount.ResourceTypeQuality;
                    dtlConsume.ResourceSyscode = dtlAccount.ResourceCategorySysCode;
                    dtlConsume.CurrIncomeQuantity = dtlAccount.CurrContractIncomeQny;
                    dtlConsume.CurrIncomeTotalPrice = dtlAccount.CurrContractIncomeTotal;
                    dtlConsume.CurrRealConsumePlanQuantity = dtlAccount.AccUsageQny;
                    dtlConsume.CurrRealConsumePlanTotalPrice = dtlAccount.AccountTotalPrice;
                    dtlConsume.CurrResponsiConsumeQuantity = dtlAccount.CurrResponsibleCostQny;
                    dtlConsume.CurrResponsiConsumeTotalPrice = dtlAccount.CurrResponsibleCostTotal;
                    //写入物资耗用对应的月度成本核算明细
                    foreach (CostMonthAccountDtl acctDtl in ht_monthdtl.Values)
                    {
                        if (dtlConsume.ProjectTaskDtlGUID == acctDtl.ProjectTaskDtlGUID.Id)
                        {
                            dtlConsume.TheAccountDetail = acctDtl;
                            break;
                        }
                    }
                    ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                }
            }

            #endregion

            #region 4：分包结算单的月度汇总

            //4.1 查询分包结算信息
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            oq.AddCriterion(Expression.IsNull("MonthAccBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_SubBalance = Dao.ObjectQuery(typeof (SubContractBalanceBill), oq);
            //IList list_SubBalance = new ArrayList();
            Hashtable ht_sumConsumeDomain = new Hashtable();

            //4.2 分包结算信息进行月度结算
            foreach (SubContractBalanceBill model in list_SubBalance)
            {
                foreach (SubContractBalanceDetail dtl in model.Details)
                {
                    //分包结算中部分明细无工程任务明细，如水电费等
                    if (dtl.BalanceTaskDtl == null || TransUtil.ToString(dtl.BalanceTaskDtl.Id) == "0")
                    {
                        GWBSDetail taskDtl = new GWBSDetail();
                        taskDtl.Id = "0";
                        dtl.BalanceTaskDtl = taskDtl;
                    }
                    string dtlLinkStr = dtl.BalanceTaskSyscode + "-" + dtl.BalanceTaskDtl.Id;
                    string currSyscode = dtl.BalanceTaskSyscode;
                    // 分包结算明细匹配月度成本核算明细
                    if (ht_monthdtl.Contains(dtlLinkStr))
                    {
                        CostMonthAccountDtl costDtl = (CostMonthAccountDtl) ht_monthdtl[dtlLinkStr];
                        //<1>如果匹配, 将{分包资源耗用结算}核 算到<操作{月度成本核算物资耗用}>下
                        foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                        {
                            string dtlConsumeLinkStr = dtl.BalanceTaskSyscode + "-" + dtl.BalanceTaskDtl.Id + "-" +
                                                       subject.ResourceTypeGUID + "-" + subject.BalanceSubjectGUID.Id;
                            if (ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                            {
                                CostMonthAccDtlConsume dtlComsume =
                                    (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                                dtlComsume.CurrRealConsumeQuantity += subject.BalanceQuantity;
                                dtlComsume.CurrRealConsumeTotalPrice += subject.BalanceTotalPrice;
                                if (dtlComsume.CurrRealConsumeQuantity != 0)
                                {
                                    dtlComsume.CurrRealConsumePrice =
                                        decimal.Round(
                                            dtlComsume.CurrRealConsumeTotalPrice/dtlComsume.CurrRealConsumeQuantity, 4);
                                }
                            }
                            else
                            {
                                CostMonthAccDtlConsume dtlComsume = new CostMonthAccDtlConsume();
                                dtlComsume.ResourceTypeGUID = subject.ResourceTypeGUID;
                                dtlComsume.ResourceTypeName = subject.ResourceTypeName;
                                dtlComsume.ResourceTypeSpec = subject.ResourceTypeSpec;
                                dtlComsume.ResourceTypeStuff = subject.ResourceTypeStuff;
                                dtlComsume.ResourceSyscode = subject.ResourceSyscode;
                                dtlComsume.CostingSubjectGUID = subject.BalanceSubjectGUID;
                                dtlComsume.CostingSubjectName = subject.BalanceSubjectName;
                                dtlComsume.CostSubjectSyscode = subject.BalanceSubjectSyscode;
                                dtlComsume.RationUnitGUID = subject.QuantityUnit;
                                dtlComsume.RationUnitName = subject.QuantityUnitName;
                                if (costDtl.ProjectTaskDtlGUID != null)
                                {
                                    dtlComsume.ProjectTaskDtlGUID = costDtl.ProjectTaskDtlGUID.Id;
                                }
                                dtlComsume.TheAccountDetail = costDtl;
                                if (costDtl.CurrRealQuantity == 0)
                                {
                                    costDtl.CurrRealQuantity = subject.BalanceQuantity;
                                }
                                if (costDtl.CurrRealTotalPrice == 0)
                                {
                                    costDtl.CurrRealTotalPrice = subject.BalanceTotalPrice;
                                }
                                dtlComsume.CurrRealConsumeQuantity = subject.BalanceQuantity;
                                dtlComsume.CurrRealConsumeTotalPrice = subject.BalanceTotalPrice;
                                if (dtlComsume.CurrRealConsumeQuantity != 0)
                                {
                                    dtlComsume.CurrRealConsumePrice =
                                        decimal.Round(
                                            dtlComsume.CurrRealConsumeTotalPrice/dtlComsume.CurrRealConsumeQuantity, 4);
                                }
                                costDtl.Details.Add(dtlComsume);
                                ht_monthdtl.Remove(dtlLinkStr);
                                ht_monthdtl.Add(dtlLinkStr, costDtl);
                                ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlComsume);
                            }
                        }
                    }
                    else
                    {
//<2> 未匹配 
                        // 匹配月度成本物资耗用[父节点]                    
                        bool queryFlag = false;
                        IList list_curr_monthdtlconsume = new ArrayList();
                        foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                        {
                            string dtlConsumeLinkStr = dtl.BalanceTaskSyscode + "-" + dtl.BalanceTaskDtl.Id + "-" +
                                                       subject.ResourceTypeGUID + "-" + subject.BalanceSubjectGUID.Id;
                            //string currSyscode = this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 1);//当前物资耗用的GWBS层次码
                            foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
                            {
                                CostMonthAccDtlConsume dtlConsume =
                                    (CostMonthAccDtlConsume) ht_monthdtlconsume[month_dtConsumelinkStr];
                                string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
                                //<2.1> 如果匹配, 将<实际耗用汇总>分摊到<操作 {月度资源耗用明细核算}集>
                                string dtlLinkStr1 = this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 2);
                                string monthLinkStr = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 2);
                                if (currSyscode.Contains(monthSyscode)
                                    &&
                                    this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 2).Equals(
                                        this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 2)))
                                {
                                    list_curr_monthdtlconsume.Add(dtlConsume);
                                    queryFlag = true;
                                }

                            }
                        }
                        if (queryFlag == false)
                        {
                            //匹配月度成本物资耗用[子节点]
                            foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                            {
                                string dtlConsumeLinkStr = dtl.BalanceTaskSyscode + "-" + dtl.BalanceTaskDtl.Id + "-" +
                                                           subject.ResourceTypeGUID + "-" +
                                                           subject.BalanceSubjectGUID.Id;
                                //string currSyscode = this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 1);//当前物资耗用的GWBS层次码
                                foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
                                {
                                    CostMonthAccDtlConsume dtlConsume =
                                        (CostMonthAccDtlConsume) ht_monthdtlconsume[month_dtConsumelinkStr];
                                    string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
                                    //<2.2> 如果匹配, 将<实际耗用汇总>分摊到<操作 {月度资源耗用明细核算}集>
                                    if (monthSyscode.Contains(currSyscode)
                                        &&
                                        this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 2).Equals(
                                            this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 2)))
                                    {
                                        list_curr_monthdtlconsume.Add(dtlConsume);
                                        queryFlag = true;
                                    }
                                }
                            }
                        }

                        if (queryFlag == true)
                        {
                            decimal sumConsumePlanQty = 0; //当期实际耗用计划量汇总
                            decimal quantityRatio = 1; //数量权重
                            decimal totalPriceRatio = 1; //合价权重
                            decimal sumConsumeQty = 0;
                            decimal sumConsumeMoney = 0;
                            foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                            {
                                sumConsumeQty += subject.BalanceQuantity;
                                sumConsumeMoney += subject.BalanceTotalPrice;
                            }
                            foreach (CostMonthAccDtlConsume dtlConsume in list_curr_monthdtlconsume)
                            {
                                sumConsumePlanQty += dtlConsume.CurrRealConsumePlanQuantity;
                            }
                            if (sumConsumePlanQty > 0)
                            {
                                quantityRatio = decimal.Round(sumConsumeQty/sumConsumePlanQty, 4);
                                //总价系数=总价/∑实际计划量
                                totalPriceRatio = decimal.Round(sumConsumeMoney/sumConsumePlanQty, 4);
                            }
                            int maxCount = list_curr_monthdtlconsume.Count; //处理分摊尾数问题,最后一条特殊处理
                            int t = 0;
                            decimal sumCurrConsumeQty = 0;
                            decimal sumCurrConsumeMoney = 0;
                            foreach (CostMonthAccDtlConsume dtlConsume in list_curr_monthdtlconsume)
                            {
                                string dtlConsumeLinkStr = dtl.BalanceTaskSyscode + "-" + dtl.BalanceTaskDtl.Id + "-" +
                                                           dtlConsume.ResourceTypeGUID + "-" +
                                                           dtlConsume.CostingSubjectGUID.Id;
                                t++;
                                if (t == maxCount)
                                {
                                    dtlConsume.CurrRealConsumeQuantity = sumConsumeQty - sumCurrConsumeQty;
                                    dtlConsume.CurrRealConsumeTotalPrice = sumConsumeMoney - sumCurrConsumeMoney;
                                    if (dtlConsume.CurrRealConsumeQuantity != 0)
                                    {
                                        dtlConsume.CurrRealConsumePrice =
                                            decimal.Round(
                                                dtlConsume.CurrRealConsumeTotalPrice/dtlConsume.CurrRealConsumeQuantity,
                                                4);
                                    }
                                }
                                else
                                {
                                    dtlConsume.CurrRealConsumeQuantity = dtlConsume.CurrRealConsumePlanQuantity*
                                                                         quantityRatio;
                                    dtlConsume.CurrRealConsumeTotalPrice = dtlConsume.CurrRealConsumePlanQuantity*
                                                                           totalPriceRatio; //价系数*实际计划量
                                    if (dtlConsume.CurrRealConsumeQuantity != 0)
                                    {
                                        dtlConsume.CurrRealConsumePrice =
                                            decimal.Round(
                                                dtlConsume.CurrRealConsumeTotalPrice/dtlConsume.CurrRealConsumeQuantity,
                                                4);
                                    }
                                    sumCurrConsumeQty += dtlConsume.CurrRealConsumeQuantity;
                                    sumCurrConsumeMoney += dtlConsume.CurrRealConsumeTotalPrice;
                                }
                                ht_monthdtlconsume.Remove(dtlConsumeLinkStr);
                                ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                            }
                        }
                        else
                        {
                            //<2.2> 如果未匹配
                            //<2.2.1> 先查[月度工程任务明细核算]本节点和父节点,通过<工程任务节点>
                            IList list_curr_monthdtl = new ArrayList();
                            int syscodeLength = 0;
                            CostMonthAccountDtl last_dtl = new CostMonthAccountDtl();
                            foreach (string month_dtlLinkStr in ht_monthdtl.Keys) //找最近的节点2013-01-07
                            {
                                CostMonthAccountDtl costDtl = (CostMonthAccountDtl) ht_monthdtl[month_dtlLinkStr];
                                string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtlLinkStr, 1);
                                int tempLength = monthSyscode.Length;
                                if (currSyscode.Contains(monthSyscode))
                                {
                                    if (syscodeLength == 0 || tempLength > syscodeLength)
                                    {
                                        syscodeLength = monthSyscode.Length;
                                        last_dtl = costDtl;
                                    }
                                    queryFlag = true;
                                }
                            }
                            if (queryFlag == true)
                            {
                                list_curr_monthdtl.Add(last_dtl);
                            }
                            //<2.2.2> 再查[月度工程任务明细核算]子节点,通过<工程任务节点>
                            if (queryFlag == false)
                            {
                                last_dtl = new CostMonthAccountDtl();
                                foreach (string month_dtlLinkStr in ht_monthdtl.Keys)
                                {
                                    CostMonthAccountDtl costDtl = (CostMonthAccountDtl) ht_monthdtl[month_dtlLinkStr];
                                    string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtlLinkStr, 1);
                                    int tempLength = monthSyscode.Length;
                                    if (monthSyscode.Contains(currSyscode))
                                    {
                                        if (syscodeLength == 0 || tempLength < syscodeLength)
                                        {
                                            syscodeLength = monthSyscode.Length;
                                            last_dtl = costDtl;
                                        }
                                        queryFlag = true;
                                    }
                                }
                                if (queryFlag == true)
                                {
                                    list_curr_monthdtl.Add(last_dtl);
                                }
                            }

                            //<2.2.3> 将<实际耗用汇总>分摊到<操作 {月度资源耗用明细核算}集>
                            if (queryFlag == true)
                            {
                                decimal sumEarnValue = 0; //当期挣值
                                decimal quantityRatio = 1; //数量权重
                                decimal totalPriceRatio = 1; //合价权重
                                decimal sumConsumeQty = 0;
                                decimal sumConsumeMoney = 0;
                                foreach (CostMonthAccountDtl costDtl in list_curr_monthdtl)
                                {
                                    sumEarnValue += costDtl.CurrEarnValue;
                                }
                                if (sumEarnValue > 0)
                                {
                                    quantityRatio = decimal.Round(sumConsumeQty/sumEarnValue, 4);
                                    //总价系数=总价/∑实际计划量
                                    totalPriceRatio = decimal.Round(sumConsumeMoney/sumEarnValue, 4);
                                }
                                int maxCount = list_curr_monthdtl.Count; //处理分摊尾数问题,最后一条特殊处理
                                decimal sumCurrConsumeQty = 0;
                                decimal sumCurrConsumeMoney = 0;
                                foreach (SubContractBalanceSubjectDtl subject in dtl.Details)
                                {
                                    if (sumEarnValue > 0)
                                    {
                                        quantityRatio = decimal.Round(subject.BalanceQuantity/sumEarnValue, 4);
                                        //总价系数=总价/∑实际计划量
                                        totalPriceRatio = decimal.Round(subject.BalanceTotalPrice/sumEarnValue, 4);
                                    }
                                    int t = 0;
                                    foreach (CostMonthAccountDtl costDtl in list_curr_monthdtl)
                                    {
                                        t++;
                                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                                        dtlConsume.ProjectTaskDtlGUID = costDtl.ProjectTaskDtlGUID.Id;
                                        dtlConsume.ProjectTaskDtlName = costDtl.ProjectTaskDtlName;
                                        dtlConsume.CostingSubjectGUID = subject.BalanceSubjectGUID;
                                        dtlConsume.CostingSubjectName = subject.BalanceSubjectName;
                                        dtlConsume.CostSubjectSyscode = subject.BalanceSubjectSyscode;
                                        dtlConsume.ResourceTypeGUID = subject.ResourceTypeGUID;
                                        dtlConsume.ResourceTypeName = subject.ResourceTypeName;
                                        dtlConsume.ResourceTypeSpec = subject.ResourceTypeSpec;
                                        dtlConsume.ResourceTypeStuff = subject.ResourceTypeStuff;
                                        dtlConsume.ResourceSyscode = subject.ResourceSyscode;
                                        dtlConsume.RationUnitGUID = subject.QuantityUnit;
                                        dtlConsume.RationUnitName = subject.QuantityUnitName;
                                        if (t == maxCount)
                                        {
                                            dtlConsume.CurrRealConsumeQuantity = subject.BalanceQuantity -
                                                                                 sumCurrConsumeQty;
                                            dtlConsume.CurrRealConsumeTotalPrice = subject.BalanceTotalPrice -
                                                                                   sumCurrConsumeMoney;
                                            if (dtlConsume.CurrRealConsumeQuantity != 0)
                                            {
                                                dtlConsume.CurrRealConsumePrice =
                                                    decimal.Round(
                                                        dtlConsume.CurrRealConsumeTotalPrice/
                                                        dtlConsume.CurrRealConsumeQuantity, 4);
                                            }
                                        }
                                        else
                                        {
                                            dtlConsume.CurrRealConsumeQuantity =
                                                decimal.Round(costDtl.CurrEarnValue*quantityRatio, 4);
                                            sumCurrConsumeQty += dtlConsume.CurrRealConsumeQuantity;
                                            dtlConsume.CurrRealConsumeTotalPrice =
                                                decimal.Round(costDtl.CurrEarnValue*totalPriceRatio, 4);
                                            sumCurrConsumeMoney += dtlConsume.CurrRealConsumeTotalPrice;
                                            if (dtlConsume.CurrRealConsumeQuantity != 0)
                                            {
                                                dtlConsume.CurrRealConsumePrice =
                                                    decimal.Round(
                                                        dtlConsume.CurrRealConsumeTotalPrice/
                                                        dtlConsume.CurrRealConsumeQuantity, 4);
                                            }
                                        }
                                        //加入现有的月度物资消耗集合中
                                        string dtlConsumeLinkStr = dtl.BalanceTaskSyscode + "-" + dtl.BalanceTaskDtl.Id +
                                                                   "-" + dtlConsume.ResourceTypeGUID + "-" +
                                                                   dtlConsume.CostingSubjectGUID.Id;
                                        if (!ht_monthdtlconsume.Contains(dtlConsumeLinkStr))
                                        {
                                            ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                                        }
                                        else
                                        {
                                            CostMonthAccDtlConsume temp =
                                                (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlConsumeLinkStr];
                                            temp.CurrRealConsumeQuantity += dtlConsume.CurrRealConsumeQuantity;
                                            temp.CurrRealConsumeTotalPrice += dtlConsume.CurrRealConsumeTotalPrice;
                                            if (temp.CurrRealConsumeQuantity != 0)
                                            {
                                                temp.CurrRealConsumePrice =
                                                    decimal.Round(
                                                        temp.CurrRealConsumeTotalPrice/temp.CurrRealConsumeQuantity, 4);
                                            }
                                            ht_monthdtlconsume.Remove(dtlConsumeLinkStr);
                                            ht_monthdtlconsume.Add(dtlConsumeLinkStr, dtlConsume);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //throw new Exception("");
                            }
                        }
                    }
                }
            }

            #endregion

            #region 5：物资耗用/料具租赁结算单的月度汇总

            ht_sumConsumeDomain.Clear();
            //5.1 查询物资耗用结算信息和料具结算单
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.IsNull("MonthAccountBill"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list_resourceConsume = Dao.ObjectQuery(typeof (MaterialSettleMaster), oq);
            //IList list_resourceConsume = new ArrayList();

            //5.2 汇总物资耗用结算
            foreach (MaterialSettleMaster model in list_resourceConsume)
            {
                foreach (MaterialSettleDetail dtl in model.Details)
                {
                    if (dtl.MaterialResource == null)
                    {
                        throw new Exception("物资耗用结算中存在物资为空的情况！");
                    }
                    if (dtl.AccountCostSubject == null)
                    {
                        throw new Exception("物资耗用结算中存在核算科目为空的情况！");
                    }
                    string dtlConsumeLinkStr = dtl.ProjectTaskCode + "-" + dtl.MaterialResource.Id + "-" +
                                               dtl.AccountCostSubject.Id;
                    if (ht_sumConsumeDomain.Contains(dtlConsumeLinkStr))
                    {
                        DataDomain tempDataDomain = (DataDomain) ht_sumConsumeDomain[dtlConsumeLinkStr];
                        tempDataDomain.Name1 = (TransUtil.ToDecimal(tempDataDomain.Name1) + dtl.Quantity) + "";
                        tempDataDomain.Name2 = (TransUtil.ToDecimal(tempDataDomain.Name2) + dtl.Money) + "";
                        ht_sumConsumeDomain.Remove(dtlConsumeLinkStr);
                        ht_sumConsumeDomain.Add(dtlConsumeLinkStr, tempDataDomain);
                    }
                    else
                    {
                        DataDomain consumeDataDomain = new DataDomain();
                        consumeDataDomain.Name1 = TransUtil.ToString(dtl.Quantity);
                        consumeDataDomain.Name2 = TransUtil.ToString(dtl.Money);
                        consumeDataDomain.Name3 = TransUtil.ToString(dtl.MaterialResource.Id);
                        consumeDataDomain.Name4 = TransUtil.ToString(dtl.MaterialName);
                        consumeDataDomain.Name5 = TransUtil.ToString(dtl.MaterialSysCode);
                        consumeDataDomain.Name6 = (StandardUnit) dtl.QuantityUnit;
                        consumeDataDomain.Name7 = (CostAccountSubject) dtl.AccountCostSubject;
                        consumeDataDomain.Name8 = TransUtil.ToString(dtl.AccountCostName);
                        consumeDataDomain.Name9 = TransUtil.ToString(dtl.AccountCostCode);
                        consumeDataDomain.Name20 = dtl.ProjectTaskName;
                        ht_sumConsumeDomain.Add(dtlConsumeLinkStr, consumeDataDomain);
                    }
                }
            }
            //5.3 把物资耗用分科目对象转成标准的物资耗用对象
            ht_monthdtlconsume = this.RealConsumeShare(costBill, ht_monthdtl, ht_monthdtlconsume, ht_sumConsumeDomain,
                                                       projectInfo, list_GWBS, ht_subject);

            #endregion

            #region 6：设备租赁结算单的月度汇总

            ht_sumConsumeDomain.Clear();
            // 查询设备租赁结算单
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.IsNull("MonthAccountBillId"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialSubjectDetails", NHibernate.FetchMode.Eager);
            IList list_machineConsume = Dao.ObjectQuery(typeof (MaterialRentalSettlementMaster), oq);
            //IList list_machineConsume = new ArrayList();
            // 汇总设备租赁结算
            foreach (MaterialRentalSettlementMaster model in list_machineConsume)
            {
                foreach (MaterialRentalSettlementDetail dtl in model.Details)
                {
                    foreach (MaterialSubjectDetail subject in dtl.MaterialSubjectDetails)
                    {
                        string dtlConsumeLinkStr = dtl.UsedPartSysCode + "-" + dtl.MaterialResource.Id + "-" +
                                                   subject.SettleSubject.Id;
                        if (ht_sumConsumeDomain.Contains(dtlConsumeLinkStr))
                        {
                            DataDomain tempDataDomain = (DataDomain) ht_sumConsumeDomain[dtlConsumeLinkStr];
                            tempDataDomain.Name1 = (TransUtil.ToDecimal(tempDataDomain.Name1) + subject.SettleQuantity) +
                                                   "";
                            tempDataDomain.Name2 = (TransUtil.ToDecimal(tempDataDomain.Name2) + subject.SettleMoney) +
                                                   "";
                            ht_sumConsumeDomain.Remove(dtlConsumeLinkStr);
                            ht_sumConsumeDomain.Add(dtlConsumeLinkStr, tempDataDomain);
                        }
                        else
                        {
                            DataDomain consumeDataDomain = new DataDomain();
                            consumeDataDomain.Name1 = TransUtil.ToString(subject.SettleQuantity);
                            consumeDataDomain.Name2 = TransUtil.ToString(subject.SettleMoney);
                            consumeDataDomain.Name3 = TransUtil.ToString(subject.MaterialType.Id);
                            consumeDataDomain.Name4 = TransUtil.ToString(subject.MaterialTypeName);
                            consumeDataDomain.Name5 = TransUtil.ToString(subject.MaterialStuff);
                            consumeDataDomain.Name6 = (StandardUnit) subject.QuantityUnit;
                            consumeDataDomain.Name7 = (CostAccountSubject) subject.SettleSubject;
                            consumeDataDomain.Name8 = TransUtil.ToString(subject.SettleSubjectName);
                            consumeDataDomain.Name9 = TransUtil.ToString(subject.SettleSubjectSyscode);
                            consumeDataDomain.Name20 = dtl.UsedPartName;
                            ht_sumConsumeDomain.Add(dtlConsumeLinkStr, consumeDataDomain);
                        }
                    }
                }
            }
            ht_monthdtlconsume = this.RealConsumeShare(costBill, ht_monthdtl, ht_monthdtlconsume, ht_sumConsumeDomain,
                                                       projectInfo, list_GWBS, ht_subject);

            #endregion

            #region 7：费用结算单的月度汇总

            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            if (TransUtil.ToString(costBill.AccountOrgGUID) != "")
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", costBill.AccountOrgGUID, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Le("CreateDate", costBill.EndTime));
            oq.AddCriterion(Expression.IsNull("MonthlySettlment"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.AccountCostSubject", NHibernate.FetchMode.Eager);
            IList list_expenseConsume = Dao.ObjectQuery(typeof (ExpensesSettleMaster), oq);
            //IList list_expenseConsume = new ArrayList();
            // 汇总费用结算结算
            foreach (ExpensesSettleMaster model in list_expenseConsume)
            {
                foreach (ExpensesSettleDetail dtl in model.Details)
                {
                    string dtlConsumeLinkStr = dtl.ProjectTaskSysCode + "-" + dtl.MaterialResource.Id + "-" +
                                               dtl.AccountCostSubject.Id;
                    if (ht_sumConsumeDomain.Contains(dtlConsumeLinkStr))
                    {
                        DataDomain tempDataDomain = (DataDomain) ht_sumConsumeDomain[dtlConsumeLinkStr];
                        tempDataDomain.Name1 = (TransUtil.ToDecimal(tempDataDomain.Name1) + dtl.Quantity) + "";
                        tempDataDomain.Name2 = (TransUtil.ToDecimal(tempDataDomain.Name2) + dtl.Money) + "";
                        ht_sumConsumeDomain.Remove(dtlConsumeLinkStr);
                        ht_sumConsumeDomain.Add(dtlConsumeLinkStr, tempDataDomain);
                    }
                    else
                    {
                        DataDomain consumeDataDomain = new DataDomain();
                        consumeDataDomain.Name1 = TransUtil.ToString(dtl.Quantity);
                        consumeDataDomain.Name2 = TransUtil.ToString(dtl.Money);
                        consumeDataDomain.Name3 = TransUtil.ToString(dtl.MaterialResource.Id);
                        consumeDataDomain.Name4 = TransUtil.ToString(dtl.MaterialName);
                        consumeDataDomain.Name5 = TransUtil.ToString(dtl.MaterialSysCode);
                        consumeDataDomain.Name6 = (StandardUnit) dtl.QuantityUnit;
                        consumeDataDomain.Name7 = (CostAccountSubject) dtl.AccountCostSubject;
                        consumeDataDomain.Name8 = TransUtil.ToString(dtl.AccountCostName);
                        consumeDataDomain.Name9 = TransUtil.ToString(dtl.AccountCostSysCode);
                        consumeDataDomain.Name20 = dtl.ProjectTaskName;
                        ht_sumConsumeDomain.Add(dtlConsumeLinkStr, consumeDataDomain);
                    }
                }
            }
            //补充科目编码
            foreach (string dtlStrLink in ht_monthdtlconsume.Keys)
            {
                CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume) ht_monthdtlconsume[dtlStrLink];
                string subjectCode = TransUtil.ToString(ht_subject[dtlConsume.CostingSubjectGUID.Id]);
                dtlConsume.CostSubjectCode = subjectCode;
            }
            ht_monthdtlconsume = this.RealConsumeShare(costBill, ht_monthdtl, ht_monthdtlconsume, ht_sumConsumeDomain,
                                                       projectInfo, list_GWBS, ht_subject);

            #endregion

            #region 8：计算实际值和累计值等

            //10.1 更新本月实际成本
            foreach (CostMonthAccountDtl detail in ht_monthdtl.Values)
            {
                decimal sumCurrRealTotalPrice = 0;
                decimal sumCurrRealQuantity = 0;
                decimal sumCurrResponsiConsumeQuantity = 0;
                decimal sumCurrResponsiConsumeTotalPrice = 0;
                decimal sumCurrIncomeQuantity = 0;
                decimal sumCurrIncomeTotalPrice = 0;
                foreach (CostMonthAccDtlConsume dtlConsume in ht_monthdtlconsume.Values)
                {
                    if (detail.ProjectTaskDtlGUID.Id == dtlConsume.ProjectTaskDtlGUID)
                    {
                        sumCurrRealTotalPrice += dtlConsume.CurrRealConsumeTotalPrice;
                        sumCurrRealQuantity += dtlConsume.CurrRealConsumeQuantity;
                        sumCurrResponsiConsumeQuantity += dtlConsume.CurrResponsiConsumeQuantity;
                        sumCurrResponsiConsumeTotalPrice += dtlConsume.CurrResponsiConsumeTotalPrice;
                        sumCurrIncomeQuantity += dtlConsume.CurrIncomeQuantity;
                        sumCurrIncomeTotalPrice += dtlConsume.CurrIncomeTotalPrice;

                    }
                }
                detail.CurrRealTotalPrice = sumCurrRealTotalPrice;
                detail.CurrRealQuantity = sumCurrRealQuantity;
                detail.CurrResponsiQuantity = sumCurrResponsiConsumeQuantity;
                detail.CurrResponsiTotalPrice = sumCurrResponsiConsumeTotalPrice;
                detail.CurrIncomeQuantity = sumCurrIncomeQuantity;
                detail.CurrIncomeTotalPrice = sumCurrIncomeTotalPrice;
                if (detail.CurrRealQuantity != 0)
                {
                    detail.CurrRealPrice = decimal.Round(detail.CurrRealTotalPrice/detail.CurrRealQuantity, 4);
                }
            }
            //10.2 查询上期的月度成本核算
            int lastYear = TransUtil.GetLastYear(costBill.Kjn, costBill.Kjy);
            int lastMonth = TransUtil.GetLastMonth(costBill.Kjn, costBill.Kjy);
            oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Kjn", lastYear));
            oq.AddCriterion(Expression.Eq("Kjy", lastMonth));
            oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            IList list_lastCostMonthAcc = Dao.ObjectQuery(typeof (CostMonthAccountBill), oq);
            //构造月度核算明细和月度核算资源耗用Hashtable
            Hashtable ht_last_monthdtl = new Hashtable();
            Hashtable ht_last_monthdtlconsume = new Hashtable();
            foreach (CostMonthAccountBill model in list_lastCostMonthAcc)
            {
                foreach (CostMonthAccountDtl detail in model.Details)
                {
                    if (!ht_last_monthdtl.Contains(detail.ProjectTaskDtlGUID))
                    {
                        ht_last_monthdtl.Add(detail.ProjectTaskDtlGUID, detail);
                    }
                    foreach (CostMonthAccDtlConsume sonDetail in detail.Details)
                    {
                        string linkStr = sonDetail.ProjectTaskDtlGUID + "-" + sonDetail.ResourceTypeGUID + "-" +
                                         sonDetail.CostingSubjectGUID;
                        if (!ht_last_monthdtlconsume.Contains(linkStr))
                        {
                            ht_last_monthdtlconsume.Add(linkStr, sonDetail);
                        }
                    }
                }
            }

            //10.3 根据上月数据更新本月累计值
            if (list_lastCostMonthAcc != null && list_lastCostMonthAcc.Count > 0)
            {
                foreach (CostMonthAccDtlConsume dtlConsume in ht_monthdtlconsume.Values)
                {
                    string linkStr = dtlConsume.ProjectTaskDtlGUID + "-" + dtlConsume.ResourceTypeGUID + "-" +
                                     dtlConsume.CostingSubjectGUID;
                    if (ht_last_monthdtlconsume.Contains(linkStr)) //找到上月同一[工程任务明细+资源类型+成本科目]
                    {
                        CostMonthAccDtlConsume lastSonDetail = (CostMonthAccDtlConsume) ht_last_monthdtlconsume[linkStr];
                        this.CalCostMonthAcctDtlConsumeSumValue(dtlConsume, lastSonDetail);
                    }
                    else
                    {
                        CostMonthAccDtlConsume lastSonDetail =
                            this.QueryLastCostMonthAcctDtlConsumeData(dtlConsume.ProjectTaskDtlGUID,
                                                                      dtlConsume.ResourceTypeGUID,
                                                                      dtlConsume.CostingSubjectGUID.Id);
                        if (TransUtil.ToString(lastSonDetail.Id) == "" || TransUtil.ToString(lastSonDetail.Id) == "0")
                        {
                            this.CalCostMonthAcctDtlConsumeSumValue(dtlConsume, new CostMonthAccDtlConsume());
                        }
                        else
                        {
                            this.CalCostMonthAcctDtlConsumeSumValue(dtlConsume, lastSonDetail);
                        }
                    }
                }
                foreach (CostMonthAccountDtl detail in ht_monthdtl.Values)
                {
                    if (ht_last_monthdtl.Contains(detail.ProjectTaskDtlGUID.Id)) //找到上月同一工程任务明细
                    {
                        CostMonthAccountDtl lastDetail =
                            (CostMonthAccountDtl) ht_last_monthdtl[detail.ProjectTaskDtlGUID.Id];
                        this.CalCostMonthAcctDtlSumValue(detail, lastDetail);
                    }
                    else
                    {
                        CostMonthAccountDtl lastDetail = this.QueryLastCostMonthAcctDtlData(detail.ProjectTaskDtlGUID.Id);
                        if (TransUtil.ToString(lastDetail.Id) == "0" || TransUtil.ToString(lastDetail.Id) == "")
                        {
                            this.CalCostMonthAcctDtlSumValue(detail, new CostMonthAccountDtl());
                        }
                        else
                        {
                            this.CalCostMonthAcctDtlSumValue(detail, lastDetail);
                        }
                    }
                }
            }
            else
            {
                //无上月数据
                foreach (CostMonthAccDtlConsume sonDetail in ht_monthdtlconsume.Values)
                {
                    this.CalCostMonthAcctDtlConsumeSumValue(sonDetail, new CostMonthAccDtlConsume());
                }
                foreach (CostMonthAccountDtl detail in ht_monthdtl.Values)
                {
                    this.CalCostMonthAcctDtlSumValue(detail, new CostMonthAccountDtl());
                }
            }

            #endregion

            #region 9:写入自行产值数据

            ////查询当月自行产值数据
            //oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("ProjectId", costBill.ProjectId));
            //oq.AddCriterion(Expression.Ge("AccountYear", costBill.Kjn));
            //oq.AddCriterion(Expression.Le("AccountMonth", costBill.Kjy));
            //oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            //IList list_selfValue = Dao.ObjectQuery(typeof(ProduceSelfValueMaster), oq);
            ////循环修改工程任务
            //foreach (ProduceSelfValueMaster selfMaster in list_selfValue)
            //{
            //    foreach (ProduceSelfValueDetail selfDetail in selfMaster.Details)
            //    {
            //        foreach (CostMonthAccountDtl detail in ht_monthdtl.Values)
            //        {
            //            if (detail.AccountTaskNodeSyscode != null && detail.AccountTaskNodeSyscode.IndexOf(selfDetail.GWBSTreeSyscode) != -1)
            //            {
            //                selfDetail.RealValue += detail.CurrIncomeTotalPrice;
            //            }
            //        }
            //    }
            //    selfMaster.State = ProduceSelfValueMasterState.实际产值已生成;
            //    Dao.SaveOrUpdate(selfMaster);
            //}

            #endregion

            #region 10：数据库处理

            //10.1 写入月度成本核算信息
            foreach (CostMonthAccountDtl detail in ht_monthdtl.Values)
            {
                if (detail.CurrRealTotalPrice != 0 || detail.CurrIncomeTotalPrice != 0)
                {
                    if (detail.ProjectTaskDtlGUID != null && detail.ProjectTaskDtlGUID.Id == "0")
                    {
                        detail.ProjectTaskDtlGUID = null;
                    }
                    detail.TheAccountBill = costBill;
                    foreach (CostMonthAccDtlConsume dtlConsume in ht_monthdtlconsume.Values)
                    {
                        if (dtlConsume.CurrRealConsumeTotalPrice != 0 || dtlConsume.CurrIncomeTotalPrice != 0)
                        {
                            if (dtlConsume.CostingSubjectGUID.Id != null &&
                                TransUtil.ToString(dtlConsume.CostSubjectCode) == "")
                            {
                                dtlConsume.CostSubjectCode =
                                    TransUtil.ToString(ht_subject[dtlConsume.CostingSubjectGUID.Id]);
                            }
                            if (dtlConsume.ProjectTaskDtlGUID == detail.ProjectTaskDtlGUID.Id)
                            {
                                detail.Details.Add(dtlConsume);
                                dtlConsume.TheAccountDetail = detail;
                            }
                        }
                    }
                    costBill.Details.Add(detail);
                }
                else
                {
                    //写入删除的过程日志

                }
            }
            if (costBill.Details.Count == 0)
            {
                throw new Exception("选择时间段内无月度核算信息产生！");
            }

            Dao.SaveOrUpdate(costBill);

            //10.2 写入成本核算过程日志
            //this.InsertCostMonthAccountLogByBatch(list_log);
            //10.3 回写工程任务核算/分包结算/设备租赁结算/料具结算/费用结算的标志
            Hashtable ht_forwardbill = new Hashtable();
            ht_forwardbill.Add("0", list_PTaskAccount);
            ht_forwardbill.Add("1", list_SubBalance);
            ht_forwardbill.Add("2", list_resourceConsume);
            ht_forwardbill.Add("3", list_machineConsume);
            ht_forwardbill.Add("5", list_expenseConsume);
            this.WriteForwardBillFlag(ht_forwardbill, costBill.Id);

            #endregion
        }

        /// <summary>
        /// 月度成本核算入口主程序
        /// </summary>
        public string CostMonthAccountCal(CostMonthAccountBill costBill, CurrentProjectInfo projectInfo)
        {
            string msgStr = "";
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.老项目)
                msgStr = this.CostMonthAccountBySubjectAndResource(costBill, projectInfo);
            else
            {
                try
                {
                    if (!CreateCostMonthAccount(costBill))
                    {
                        msgStr = "生成月度成本核算数据失败";
                    }
                }
                catch (Exception ex)
                {
                    msgStr = ex.Message;
                }
            }

            //if (projectInfo.ProjectInfoState == EnumProjectInfoState.老项目)
            //{
            //    msgStr = this.CostMonthAccountSimpleCalByCall(costBill, projectInfo);
            //}
            //else
            //{
            //    msgStr = this.CostMonthAccountBySubjectAndResource(costBill, projectInfo);
            //}
            //修改资源信息
            if (msgStr == "")
            {
                UpdateResourceInfo();
            }
            return msgStr;
        }

        #region 实际耗用核算(算法统一)

        /// <summary>
        /// 对本期的分包结算单、物资耗用结算单、设备租赁结算单、料具结算单、费用结算单的耗用数量和耗用合价进行分摊
        /// <param name="costBill">月度成本核算主表</param>
        /// <param name="ht_monthdtl">月度成本核算明细的集合</param>
        /// <param name="ht_monthdtlconsume">月度成本物资耗用的集合</param>
        /// <param name="ht_sumConsumeDomain">实际耗用的DataDomain对象</param>
        /// <param name="projectInfo">项目信息</param>
        /// <param name="list_GWBS">本次核算的GWBS集合</param>
        ///  [Name1:数量][Name2:合价][Name3:物资类型GUID][Name4:资源名称][Name5:资源层次码][Name6:定额计量单位对象]
        ///  [Name7:成本科目对象][Name8:成本科目名称][Name9:成本科目层次玛]
        /// </summary>
        private Hashtable RealConsumeShare(CostMonthAccountBill costBill, Hashtable ht_monthdtl,
                                           Hashtable ht_monthdtlconsume,
                                           Hashtable ht_sumConsumeDomain, CurrentProjectInfo projectInfo,
                                           IList list_GWBS, Hashtable ht_subject)
        {
            //对汇总后的每一条物资耗用对象
            foreach (string dtlConsumeLinkStr in ht_sumConsumeDomain.Keys)
            {
                try
                {
                    DataDomain consumeDataDomain = (DataDomain) ht_sumConsumeDomain[dtlConsumeLinkStr]; //数量+"-"+合价
                    CostAccountSubject costSubject = (CostAccountSubject) consumeDataDomain.Name7;
                    decimal sumConsumeMoney = TransUtil.ToDecimal(consumeDataDomain.Name2);
                    decimal sumConsumeQty = TransUtil.ToDecimal(consumeDataDomain.Name1);

                    IList list_curr_monthdtlconsume = new ArrayList(); //符合条件的月度资源耗用明细核算集合
                    IList list_curr_monthdtl = new ArrayList(); //符合条件的月度成本核算明细
                    IList list_curr_gwbs_subject = new ArrayList(); //符合条件的GWBS的工程资源耗用明细
                    bool queryFlag = false;
                    string shareRatioType = ""; //分摊权重类型
                    bool processFlag = false; //过程量计量标志

                    //4.3.1 先查本节点和父节点的[月度工程资源耗用明细]<工程任务节点+资源类型+科目>
                    string currSyscode = this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 1); //当前物资耗用的GWBS层次码
                    foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
                    {
                        CostMonthAccDtlConsume dtlConsume =
                            (CostMonthAccDtlConsume) ht_monthdtlconsume[month_dtConsumelinkStr];
                        string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
                        if (currSyscode.Contains(monthSyscode)
                            &&
                            this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 3).Equals(
                                this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 2)))
                        {
                            list_curr_monthdtlconsume.Add(dtlConsume);
                            queryFlag = true;
                            processFlag = true;
                        }
                    }

                    //4.3.2 查询子节点的[月度工程资源耗用明细]<工程任务节点+资源类型+科目>
                    if (queryFlag == false)
                    {
                        list_curr_monthdtlconsume.Clear();
                        foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
                        {
                            CostMonthAccDtlConsume dtlConsume =
                                (CostMonthAccDtlConsume) ht_monthdtlconsume[month_dtConsumelinkStr];
                            string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
                            if (monthSyscode.Contains(currSyscode)
                                &&
                                this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 3).Equals(
                                    this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 2)))
                            {
                                //if (dtlConsume.CurrRealConsumePlanQuantity != 0)
                                //{
                                list_curr_monthdtlconsume.Add(dtlConsume);
                                queryFlag = true;
                                processFlag = true;
                                //}
                            }
                        }
                    }
                    if (queryFlag == true)
                    {
                        shareRatioType = "当期实际耗用计划量";
                    }
                    //4.3.4 先查[月度工程任务明细核算]本节点和父节点,通过<工程任务节点> 2012-12-18 找最近的核算节点
                    if (queryFlag == false)
                    {
                        list_curr_monthdtlconsume.Clear();
                        int syscodeLength = 0;
                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        CostMonthAccountDtl dtl = new CostMonthAccountDtl();
                        foreach (string month_dtlLinkStr in ht_monthdtl.Keys)
                        {
                            dtl = (CostMonthAccountDtl) ht_monthdtl[month_dtlLinkStr];
                            string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtlLinkStr, 1);
                            int tempLength = monthSyscode.Length;
                            if (currSyscode.Contains(monthSyscode)) //&& dtl.CurrEarnValue != 0
                            {
                                if (syscodeLength == 0 || tempLength > syscodeLength)
                                {
                                    syscodeLength = monthSyscode.Length;
                                    dtlConsume.ProjectTaskDtlGUID = dtl.ProjectTaskDtlGUID.Id;
                                    dtlConsume.ResourceTypeGUID = TransUtil.ToString(consumeDataDomain.Name3);
                                    dtlConsume.ResourceTypeName = TransUtil.ToString(consumeDataDomain.Name4);
                                    dtlConsume.ResourceSyscode = TransUtil.ToString(consumeDataDomain.Name5);
                                    dtlConsume.RationUnitGUID =
                                        (Application.Resource.MaterialResource.Domain.StandardUnit)
                                        consumeDataDomain.Name6;
                                    dtlConsume.CostingSubjectGUID =
                                        (
                                        Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain.
                                            CostAccountSubject) consumeDataDomain.Name7;
                                    dtlConsume.CostingSubjectName = TransUtil.ToString(consumeDataDomain.Name8);
                                    dtlConsume.CostSubjectSyscode = TransUtil.ToString(consumeDataDomain.Name9);
                                    //dtlConsume.CurrRealConsumePlanQuantity = dtl.CurrEarnValue;
                                    dtlConsume.TheAccountDetail = dtl;
                                }
                                queryFlag = true;
                            }
                        }
                        if (queryFlag == true)
                        {
                            list_curr_monthdtl.Add(dtl);
                            list_curr_monthdtlconsume.Add(dtlConsume);
                            shareRatioType = "当期挣值";
                        }
                    }
                    //4.3.5 再查[月度工程任务明细核算]子节点,通过<工程任务节点>  2012-12-18 找最近的核算节点
                    if (queryFlag == false)
                    {
                        list_curr_monthdtlconsume.Clear();
                        int syscodeLength = 0;
                        CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
                        CostMonthAccountDtl dtl = new CostMonthAccountDtl();
                        foreach (string month_dtlLinkStr in ht_monthdtl.Keys)
                        {
                            dtl = (CostMonthAccountDtl) ht_monthdtl[month_dtlLinkStr];
                            string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtlLinkStr, 1);
                            int tempLength = monthSyscode.Length;
                            if (monthSyscode.Contains(currSyscode)) // && dtl.CurrEarnValue != 0
                            {
                                if (syscodeLength == 0 || tempLength < syscodeLength)
                                {
                                    syscodeLength = monthSyscode.Length;
                                    dtlConsume = new CostMonthAccDtlConsume();
                                    dtlConsume.ProjectTaskDtlGUID = dtl.ProjectTaskDtlGUID.Id;
                                    dtlConsume.ResourceTypeGUID = TransUtil.ToString(consumeDataDomain.Name3);
                                    dtlConsume.ResourceTypeName = TransUtil.ToString(consumeDataDomain.Name4);
                                    dtlConsume.ResourceSyscode = TransUtil.ToString(consumeDataDomain.Name5);
                                    dtlConsume.RationUnitGUID =
                                        (Application.Resource.MaterialResource.Domain.StandardUnit)
                                        consumeDataDomain.Name6;
                                    dtlConsume.CostingSubjectGUID =
                                        (
                                        Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain.
                                            CostAccountSubject) consumeDataDomain.Name7;
                                    dtlConsume.CostingSubjectName = TransUtil.ToString(consumeDataDomain.Name8);
                                    dtlConsume.CostSubjectSyscode = TransUtil.ToString(consumeDataDomain.Name9);
                                    //dtlConsume.CurrRealConsumePlanQuantity = dtl.CurrEarnValue;
                                    dtlConsume.TheAccountDetail = dtl;
                                }
                                queryFlag = true;
                            }
                        }
                        if (queryFlag == true)
                        {
                            list_curr_monthdtl.Add(dtl);
                            list_curr_monthdtlconsume.Add(dtlConsume);
                            shareRatioType = "当期挣值";
                        }
                    }

                    //4.3.6 无法找到抛出错误
                    if (queryFlag == false)
                    {
                        //throw new Exception("实际耗用中的部位[" + consumeDataDomain.Name20 + "]科目[" + consumeDataDomain.Name8 + "]物资[" + consumeDataDomain.Name4 + "]无法核算其成本！");
                    }

                    #region 4.3.6 确定形象进度和过程量

                    decimal contractPercent = 0; //合同形象进度
                    decimal expensePercent = 0; //费用形象进度

                    if (processFlag == true)
                    {
                        if (costSubject.Code.IndexOf("C513") != -1 || costSubject.Code == "C5120803" ||
                            costSubject.Code == "C5120902")
                        {
//[现场管理费][支架材料费][脚手架材料费]
                            decimal sumResIncomeTotalPrice = 0; //资源合同收入合价小计
                            decimal sumCurrRealIncomeTotalPrice = 0; //当期实现合同收入合价小计
                            foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
                            {
                                CostMonthAccDtlConsume dtlConsume =
                                    (CostMonthAccDtlConsume) ht_monthdtlconsume[month_dtConsumelinkStr];
                                string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
                                string monthSubjectCode = dtlConsume.CostSubjectCode;
                                if (monthSubjectCode == null || monthSubjectCode == "")
                                {
                                    monthSubjectCode = TransUtil.ToString(ht_subject[dtlConsume.CostingSubjectGUID.Id]);
                                }
                                if (monthSyscode.Contains(currSyscode))
                                {
                                    if (costSubject.Code.IndexOf("C513") != -1)
                                    {
                                        if (TransUtil.ToString(monthSubjectCode).IndexOf("C513") == -1 &&
                                            TransUtil.ToString(monthSubjectCode).IndexOf("C514") == -1
                                            && TransUtil.ToString(monthSubjectCode).IndexOf("C515") == -1)
                                        {
                                            sumCurrRealIncomeTotalPrice += dtlConsume.CurrIncomeTotalPrice;
                                        }
                                    }
                                    else if (costSubject.Code == "C5120803")
                                    {
                                        if (monthSubjectCode == "C5120803")
                                        {
                                            sumCurrRealIncomeTotalPrice += dtlConsume.CurrRealConsumeQuantity;
                                        }
                                    }
                                    else if (costSubject.Code == "C5120902")
                                    {
                                        if (monthSubjectCode == "C5120902")
                                        {
                                            sumCurrRealIncomeTotalPrice += dtlConsume.CurrRealConsumeQuantity;
                                        }
                                    }
                                }
                            }
                            //foreach (CostMonthAccDtlConsume dtlConsume in list_curr_monthdtlconsume)
                            //{
                            //    sumCurrRealIncomeTotalPrice += dtlConsume.CurrIncomeTotalPrice;
                            //}
                            foreach (GWBSTree gwbs in list_GWBS)
                            {
                                if (gwbs.SysCode.Contains(currSyscode))
                                {
                                    foreach (GWBSDetail dtl in gwbs.Details)
                                    {
                                        foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
                                        {
                                            if (costSubject.Code.IndexOf("C513") != -1)
                                            {
                                                if (subject.CostAccountSubjectGUID != null &&
                                                    TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf(
                                                        "C513") == -1
                                                    &&
                                                    TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf(
                                                        "C514") == -1
                                                    &&
                                                    TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf(
                                                        "C515") == -1)
                                                {
                                                    sumResIncomeTotalPrice += subject.ContractTotalPrice;
                                                }
                                            }
                                            else if (costSubject.Code == "C5120803")
                                            {
                                                if (subject.CostAccountSubjectGUID.Code == "C5120803")
                                                {
                                                    sumCurrRealIncomeTotalPrice += subject.PlanWorkAmount;
                                                }
                                            }
                                            else if (costSubject.Code == "C5120902")
                                            {
                                                if (subject.CostAccountSubjectGUID != null &&
                                                    subject.CostAccountSubjectGUID.Code == "C5120902")
                                                {
                                                    sumCurrRealIncomeTotalPrice += subject.PlanWorkAmount;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (costSubject.Code.IndexOf("C513") != -1)
                            {
                                if (sumResIncomeTotalPrice != 0)
                                {
                                    contractPercent = decimal.Round(sumCurrRealIncomeTotalPrice/sumResIncomeTotalPrice,
                                                                    4);
                                }
                                decimal upDays = costBill.EndTime.Subtract(projectInfo.BeginDate).Days;
                                decimal downDays = projectInfo.EndDate.Subtract(projectInfo.BeginDate).Days;
                                expensePercent = decimal.Round(upDays/downDays, 4);
                            }
                            if (costSubject.Code == "C5120803" || costSubject.Code == "C5120902")
                            {
                                if (sumResIncomeTotalPrice != 0)
                                {
                                    expensePercent = decimal.Round(sumCurrRealIncomeTotalPrice/sumResIncomeTotalPrice, 4);
                                }
                            }

                        }
                        else if (costSubject.Code.IndexOf("C514") != -1) //规费
                        {
                            expensePercent = 1;
                        }
                        else if (costSubject.Code.IndexOf("C51103") != -1 || costSubject.Code == "C5120302")
                            //[施工机械费][安全施工材料费]
                        {
                            decimal planConsumeTotalPrice = 0; //计划耗用合价小计
                            foreach (GWBSTree gwbs in list_GWBS)
                            {
                                if (gwbs.SysCode.Contains(currSyscode))
                                {
                                    foreach (GWBSDetail dtl in gwbs.Details)
                                    {
                                        foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
                                        {
                                            if (costSubject.Code.IndexOf("C51103") != -1)
                                            {
                                                if (subject.CostAccountSubjectGUID != null &&
                                                    TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf(
                                                        "C51103") != -1)
                                                {
                                                    planConsumeTotalPrice += subject.PlanTotalPrice;
                                                }
                                            }
                                            if (costSubject.Code == "C5120302")
                                            {
                                                if (subject.CostAccountSubjectGUID != null &&
                                                    subject.CostAccountSubjectGUID.Code == "C5120302")
                                                {
                                                    planConsumeTotalPrice += subject.PlanTotalPrice;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (planConsumeTotalPrice != 0)
                            {
                                expensePercent = decimal.Round(sumConsumeMoney/planConsumeTotalPrice, 4);
                            }
                        }

                        //确定过程量
                        if (costSubject.Code.IndexOf("C515") != -1) //税金
                        {
                            shareRatioType = "当期实际耗用量";
                        }
                        if (costSubject.Code.IndexOf("C513") != -1 || costSubject.Code.IndexOf("C514") != -1 ||
                            costSubject.Code.IndexOf("C51103") != -1
                            || costSubject.Code == "C5120803" || costSubject.Code == "C5120902" ||
                            costSubject.Code == "C5120302")
                        {
                            shareRatioType = "当期实际耗用计划量";

                            foreach (CostMonthAccDtlConsume dtlConsume in list_curr_monthdtlconsume)
                            {
                                foreach (GWBSTree gwbs in list_GWBS)
                                {
                                    if (gwbs.SysCode.Contains(currSyscode))
                                    {
                                        foreach (GWBSDetail dtl in gwbs.Details)
                                        {
                                            foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
                                            {
                                                if (subject.CostAccountSubjectGUID != null &&
                                                    subject.CostAccountSubjectGUID.Code.IndexOf(costSubject.Code) != -1)
                                                {
                                                    dtlConsume.CurrRealConsumePlanQuantity += subject.PlanWorkAmount*
                                                                                              expensePercent;
                                                    dtlConsume.CurrRealConsumePlanTotalPrice += subject.PlanTotalPrice*
                                                                                                expensePercent;
                                                    dtlConsume.CurrResponsiConsumeQuantity +=
                                                        subject.ResponsibilitilyWorkAmount*expensePercent;
                                                    dtlConsume.CurrResponsiConsumeTotalPrice +=
                                                        subject.ResponsibilitilyTotalPrice*expensePercent;
                                                    if (costSubject.Code.IndexOf("C513") != -1)
                                                    {
                                                        dtlConsume.CurrIncomeQuantity += subject.ContractProjectAmount*
                                                                                         contractPercent;
                                                        dtlConsume.CurrIncomeTotalPrice += subject.ContractTotalPrice*
                                                                                           contractPercent;
                                                    }
                                                    else
                                                    {
                                                        dtlConsume.CurrIncomeQuantity += subject.ContractProjectAmount*
                                                                                         expensePercent;
                                                        dtlConsume.CurrIncomeTotalPrice += subject.ContractTotalPrice*
                                                                                           expensePercent;
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }


                    #endregion

                    //4.3.7  将<实际耗用汇总>分摊到<操作 {月度资源耗用明细核算}集>
                    if (shareRatioType == "当期实际耗用计划量")
                    {
                        ht_monthdtlconsume = this.ShareRealConsumeSumToMonthDtlConsume(sumConsumeQty, sumConsumeMoney,
                                                                                       dtlConsumeLinkStr,
                                                                                       ht_monthdtlconsume, "当期实际耗用计划量",
                                                                                       list_curr_monthdtlconsume,
                                                                                       list_curr_monthdtl, list_GWBS);
                    }
                    else if (shareRatioType == "当期挣值")
                    {
                        ht_monthdtlconsume = this.ShareRealConsumeSumToMonthDtlConsume(sumConsumeQty, sumConsumeMoney,
                                                                                       dtlConsumeLinkStr,
                                                                                       ht_monthdtlconsume, "当期挣值",
                                                                                       list_curr_monthdtlconsume,
                                                                                       list_curr_monthdtl, list_GWBS);
                    }
                    else if (shareRatioType == "当期实际耗用量")
                    {
                        ht_monthdtlconsume = this.ShareRealConsumeSumToMonthDtlConsume(sumConsumeQty, sumConsumeMoney,
                                                                                       dtlConsumeLinkStr,
                                                                                       ht_monthdtlconsume, "当期实际耗用量",
                                                                                       list_curr_monthdtlconsume,
                                                                                       list_curr_monthdtl, list_GWBS);
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }

            return ht_monthdtlconsume;
        }

        #endregion

        #region 成本核算辅助方法

        //将<实际耗用汇总>分摊到<操作 {月度资源耗用明细核算}集>
        private Hashtable ShareRealConsumeSumToMonthDtlConsume(decimal sumConsumeQty, decimal sumConsumeMoney,
                                                               string dtlConsumeLinkStr,
                                                               Hashtable ht_monthdtlconsume, string consumeType,
                                                               IList list_curr_monthdtlconsume, IList list_curr_monthdtl,
                                                               IList list_GWBS)
        {
            string currSyscode = this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 1); //当前物资耗用的GWBS层次码
            decimal quantityRatio = 1; //数量权重
            decimal totalPriceRatio = 1; //合价权重
            decimal sumCalConsumeQty = 0; //当期计算耗用汇总
            Hashtable ht_consumevalue = new Hashtable();

            //1 计算权重
            if (consumeType == "当期实际耗用计划量")
            {
                foreach (CostMonthAccDtlConsume dtlConsume in list_curr_monthdtlconsume)
                {
                    sumCalConsumeQty += dtlConsume.CurrRealConsumePlanQuantity;
                }
            }
            else if (consumeType == "当期挣值")
            {
                foreach (CostMonthAccountDtl costDtl in list_curr_monthdtl)
                {
                    sumCalConsumeQty += costDtl.CurrEarnValue;
                }
            }
            else if (consumeType == "当期实际耗用量")
            {
                foreach (CostMonthAccDtlConsume dtlConsume in list_curr_monthdtlconsume)
                {
                    decimal sumTemp = 0;
                    foreach (GWBSTree gwbs in list_GWBS)
                    {
                        if (gwbs.SysCode.Contains(currSyscode))
                        {
                            foreach (GWBSDetail dtl in gwbs.Details)
                            {
                                foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
                                {
                                    if (subject.CostAccountSubjectGUID.Code == dtlConsume.CostSubjectCode)
                                    {
                                        sumCalConsumeQty += subject.PlanTotalPrice;
                                        sumTemp += subject.PlanTotalPrice;
                                    }
                                }
                            }
                        }
                    }
                    ht_consumevalue.Add(dtlConsume.Id, sumTemp);
                }
            }
            if (sumCalConsumeQty > 0)
            {
                quantityRatio = decimal.Round(sumConsumeQty/sumCalConsumeQty, 8);
                //总价系数=总价/∑实际计划量
                totalPriceRatio = decimal.Round(sumConsumeMoney/sumCalConsumeQty, 8);
            }

            //2 进行分摊
            int maxCount = list_curr_monthdtlconsume.Count; //处理分摊尾数问题,最后一条特殊处理
            int t = 0;
            decimal sumCurrConsumeQty = 0;
            decimal sumCurrConsumeMoney = 0;
            bool ifBreak = false;
            foreach (CostMonthAccDtlConsume dtlConsume in list_curr_monthdtlconsume)
            {
                if (ifBreak == true)
                {
                    break;
                }
                string taskSyscode = this.GetGWBSSyscodeFromLinkStr(dtlConsumeLinkStr, 1);
                string temp_linkStr = taskSyscode + "-" + dtlConsume.ProjectTaskDtlGUID + "-" +
                                      dtlConsume.ResourceTypeGUID + "-" + dtlConsume.CostingSubjectGUID.Id;
                t++;
                if (t == maxCount)
                {
                    //实际成本数据有正有负
                    if ((sumConsumeQty > 0 && sumConsumeQty - sumCurrConsumeQty > 0) ||
                        (sumConsumeQty < 0 && sumConsumeQty - sumCurrConsumeQty < 0))
                    {
                        dtlConsume.CurrRealConsumeQuantity += sumConsumeQty - sumCurrConsumeQty;
                    }
                    if ((sumConsumeMoney > 0 && sumConsumeMoney - sumCurrConsumeMoney > 0) ||
                        (sumConsumeMoney < 0 && sumConsumeMoney - sumCurrConsumeMoney < 0))
                    {
                        dtlConsume.CurrRealConsumeTotalPrice += sumConsumeMoney - sumCurrConsumeMoney;
                    }

                    if (dtlConsume.CurrRealConsumeQuantity != 0)
                    {
                        dtlConsume.CurrRealConsumePrice +=
                            decimal.Round(dtlConsume.CurrRealConsumeTotalPrice/dtlConsume.CurrRealConsumeQuantity, 4);
                    }

                }
                else
                {
                    decimal currRealConsumeValue = 0;
                    if (consumeType == "当期实际耗用计划量")
                    {
                        currRealConsumeValue = dtlConsume.CurrRealConsumePlanQuantity;
                    }
                    else if (consumeType == "当期挣值")
                    {
                        currRealConsumeValue = dtlConsume.TheAccountDetail.CurrEarnValue;
                    }
                    else if (consumeType == "当期实际耗用量")
                    {
                        currRealConsumeValue = TransUtil.ToDecimal(ht_consumevalue[dtlConsume.Id]);
                    }
                    sumCurrConsumeQty += decimal.Round(currRealConsumeValue*quantityRatio, 4);
                    sumCurrConsumeMoney += decimal.Round(currRealConsumeValue*totalPriceRatio, 2);

                    if ((sumConsumeQty > 0 && sumConsumeQty - sumCurrConsumeQty < 0) ||
                        (sumConsumeMoney > 0 && sumConsumeMoney - sumCurrConsumeMoney < 0)
                        || (sumConsumeQty < 0 && sumConsumeQty - sumCurrConsumeQty > 0) ||
                        (sumConsumeMoney < 0 && sumConsumeMoney - sumCurrConsumeMoney > 0))
                    {
                        dtlConsume.CurrRealConsumeQuantity += sumConsumeQty -
                                                              (sumCurrConsumeQty -
                                                               decimal.Round(currRealConsumeValue*quantityRatio, 4));
                        dtlConsume.CurrRealConsumeTotalPrice += sumConsumeMoney -
                                                                (sumCurrConsumeMoney -
                                                                 decimal.Round(currRealConsumeValue*totalPriceRatio, 2));

                        if (dtlConsume.CurrRealConsumeQuantity != 0)
                        {
                            dtlConsume.CurrRealConsumePrice +=
                                decimal.Round(dtlConsume.CurrRealConsumeTotalPrice/dtlConsume.CurrRealConsumeQuantity, 4);
                        }
                        ifBreak = true;
                    }
                    else
                    {
                        dtlConsume.CurrRealConsumeQuantity += decimal.Round(currRealConsumeValue*quantityRatio, 4);
                        dtlConsume.CurrRealConsumeTotalPrice += decimal.Round(currRealConsumeValue*totalPriceRatio, 2);
                        //价系数*实际计划量
                    }
                    if (dtlConsume.CurrRealConsumeQuantity != 0)
                    {
                        dtlConsume.CurrRealConsumePrice +=
                            decimal.Round(dtlConsume.CurrRealConsumeTotalPrice/dtlConsume.CurrRealConsumeQuantity, 4);
                    }
                }

                if (consumeType == "当期实际耗用量")
                {
                    dtlConsume.CurrRealConsumePlanQuantity += dtlConsume.CurrRealConsumeQuantity;
                    dtlConsume.CurrIncomeQuantity += dtlConsume.CurrRealConsumeQuantity;
                    dtlConsume.CurrResponsiConsumeQuantity += dtlConsume.CurrRealConsumeQuantity;
                    dtlConsume.CurrRealConsumePlanTotalPrice += dtlConsume.CurrRealConsumeTotalPrice;
                    dtlConsume.CurrIncomeTotalPrice += dtlConsume.CurrRealConsumeTotalPrice;
                    dtlConsume.CurrResponsiConsumeTotalPrice += dtlConsume.CurrRealConsumeTotalPrice;
                }
                if (ht_monthdtlconsume.Contains(temp_linkStr))
                {
                    ht_monthdtlconsume.Remove(temp_linkStr);
                    ht_monthdtlconsume.Add(temp_linkStr, dtlConsume);
                }
                else
                {
                    ht_monthdtlconsume.Add(temp_linkStr, dtlConsume);
                }
            }
            return ht_monthdtlconsume;
        }

        //计算月度核算明细的累计值
        private CostMonthAccountDtl CalCostMonthAcctDtlSumValue(CostMonthAccountDtl currDetail,
                                                                CostMonthAccountDtl lastDetail)
        {
            if (TransUtil.ToString(lastDetail.Id) == "")
            {
                currDetail.SumCompletedPercent = currDetail.CurrCompletedPercent;
                currDetail.SumEarnValue = currDetail.CurrEarnValue;
                currDetail.SumIncomeQuantity = currDetail.CurrIncomeQuantity;
                currDetail.SumIncomeTotalPrice = currDetail.CurrIncomeTotalPrice;
                currDetail.SumRealQuantity = currDetail.CurrRealQuantity;
                currDetail.SumRealTotalPrice = currDetail.CurrRealTotalPrice;
                currDetail.SumResponsiQuantity = currDetail.CurrResponsiQuantity;
                currDetail.SumResponsiTotalPrice = currDetail.CurrResponsiTotalPrice;
            }
            else
            {
                currDetail.SumCompletedPercent = currDetail.CurrCompletedPercent + lastDetail.SumCompletedPercent;
                currDetail.SumEarnValue = currDetail.CurrEarnValue + lastDetail.SumEarnValue;
                currDetail.SumIncomeQuantity = currDetail.CurrIncomeQuantity + lastDetail.SumIncomeQuantity;
                currDetail.SumIncomeTotalPrice = currDetail.CurrIncomeTotalPrice + lastDetail.SumIncomeTotalPrice;
                currDetail.SumRealQuantity = currDetail.CurrRealQuantity + lastDetail.SumRealQuantity;
                currDetail.SumRealTotalPrice = currDetail.CurrRealTotalPrice + lastDetail.SumRealTotalPrice;
                currDetail.SumResponsiQuantity = currDetail.CurrResponsiQuantity + lastDetail.SumResponsiQuantity;
                currDetail.SumResponsiTotalPrice = currDetail.CurrResponsiTotalPrice + lastDetail.SumResponsiTotalPrice;
            }
            return currDetail;
        }

        //计算月度核算明细物资耗用的累计值(计算本月和上月累计时，如果currDtlConsume也为上月的数据)
        private CostMonthAccDtlConsume CalLastCostMonthAcctDtlConsumeSumValue(CostMonthAccDtlConsume currDtlConsume,
                                                                              CostMonthAccDtlConsume lastDtlConsume)
        {
            if (TransUtil.ToString(lastDtlConsume.Id) == "")
            {
                currDtlConsume.SumIncomeQuantity = currDtlConsume.SumIncomeQuantity;
                currDtlConsume.SumIncomeTotalPrice = currDtlConsume.SumIncomeTotalPrice;
                currDtlConsume.SumRealConsumePlanQuantity = currDtlConsume.SumRealConsumePlanQuantity;
                currDtlConsume.SumRealConsumePlanTotalPrice = currDtlConsume.SumRealConsumePlanTotalPrice;
                currDtlConsume.SumRealConsumeQuantity = currDtlConsume.SumRealConsumeQuantity;
                currDtlConsume.SumRealConsumeTotalPrice = currDtlConsume.SumRealConsumeTotalPrice;
                currDtlConsume.SumResponsiConsumeQuantity = currDtlConsume.SumResponsiConsumeQuantity;
                currDtlConsume.SumResponsiConsumeTotalPrice = currDtlConsume.SumResponsiConsumeTotalPrice;
            }
            else
            {
                currDtlConsume.SumIncomeQuantity = currDtlConsume.SumIncomeQuantity + lastDtlConsume.SumIncomeQuantity;
                currDtlConsume.SumIncomeTotalPrice = currDtlConsume.SumIncomeTotalPrice +
                                                     lastDtlConsume.SumIncomeTotalPrice;
                currDtlConsume.SumRealConsumePlanQuantity = currDtlConsume.SumRealConsumePlanQuantity +
                                                            lastDtlConsume.SumRealConsumePlanQuantity;
                currDtlConsume.SumRealConsumePlanTotalPrice = currDtlConsume.SumRealConsumePlanTotalPrice +
                                                              lastDtlConsume.SumRealConsumePlanTotalPrice;
                currDtlConsume.SumRealConsumeQuantity = currDtlConsume.SumRealConsumeQuantity +
                                                        lastDtlConsume.SumRealConsumeQuantity;
                currDtlConsume.SumRealConsumeTotalPrice = currDtlConsume.SumRealConsumeTotalPrice +
                                                          lastDtlConsume.SumRealConsumeTotalPrice;
                currDtlConsume.SumResponsiConsumeQuantity = currDtlConsume.SumResponsiConsumeQuantity +
                                                            lastDtlConsume.SumResponsiConsumeQuantity;
                currDtlConsume.SumResponsiConsumeTotalPrice = currDtlConsume.SumResponsiConsumeTotalPrice +
                                                              lastDtlConsume.SumResponsiConsumeTotalPrice;
            }
            return currDtlConsume;
        }

        //计算月度核算明细物资耗用的累计值
        private CostMonthAccDtlConsume CalCostMonthAcctDtlConsumeSumValue(CostMonthAccDtlConsume currDtlConsume,
                                                                          CostMonthAccDtlConsume lastDtlConsume)
        {
            if (TransUtil.ToString(lastDtlConsume.Id) == "")
            {
                currDtlConsume.SumIncomeQuantity = currDtlConsume.CurrIncomeQuantity;
                currDtlConsume.SumIncomeTotalPrice = currDtlConsume.CurrIncomeTotalPrice;
                currDtlConsume.SumRealConsumePlanQuantity = currDtlConsume.CurrRealConsumePlanQuantity;
                currDtlConsume.SumRealConsumePlanTotalPrice = currDtlConsume.CurrRealConsumePlanTotalPrice;
                currDtlConsume.SumRealConsumeQuantity = currDtlConsume.CurrRealConsumeQuantity;
                currDtlConsume.SumRealConsumeTotalPrice = currDtlConsume.CurrRealConsumeTotalPrice;
                currDtlConsume.SumResponsiConsumeQuantity = currDtlConsume.CurrResponsiConsumeQuantity;
                currDtlConsume.SumResponsiConsumeTotalPrice = currDtlConsume.CurrResponsiConsumeTotalPrice;
            }
            else
            {
                currDtlConsume.SumIncomeQuantity = currDtlConsume.CurrIncomeQuantity + lastDtlConsume.SumIncomeQuantity;
                currDtlConsume.SumIncomeTotalPrice = currDtlConsume.CurrIncomeTotalPrice +
                                                     lastDtlConsume.SumIncomeTotalPrice;
                currDtlConsume.SumRealConsumePlanQuantity = currDtlConsume.CurrRealConsumePlanQuantity +
                                                            lastDtlConsume.SumRealConsumePlanQuantity;
                currDtlConsume.SumRealConsumePlanTotalPrice = currDtlConsume.CurrRealConsumePlanTotalPrice +
                                                              lastDtlConsume.SumRealConsumePlanTotalPrice;
                currDtlConsume.SumRealConsumeQuantity = currDtlConsume.CurrRealConsumeQuantity +
                                                        lastDtlConsume.SumRealConsumeQuantity;
                currDtlConsume.SumRealConsumeTotalPrice = currDtlConsume.CurrRealConsumeTotalPrice +
                                                          lastDtlConsume.SumRealConsumeTotalPrice;
                currDtlConsume.SumResponsiConsumeQuantity = currDtlConsume.CurrResponsiConsumeQuantity +
                                                            lastDtlConsume.SumResponsiConsumeQuantity;
                currDtlConsume.SumResponsiConsumeTotalPrice = currDtlConsume.CurrResponsiConsumeTotalPrice +
                                                              lastDtlConsume.SumResponsiConsumeTotalPrice;
            }
            return currDtlConsume;
        }

        // <summary>
        /// 把对象转换成日志对象
        /// <param name="oDomain">成本核算关联对象</param>
        /// <param name="costBill">成本核算主表</param>
        /// <param name="doType">步骤类型</param>
        /// </summary>
        private CostMonthAccountLog TransMonthAccountToLog(Object oDomain, CostMonthAccountBill costBill)
        {
            CostMonthAccountLog log = new CostMonthAccountLog();
            log.Kjn = costBill.Kjn;
            log.Kjy = costBill.Kjy;
            log.AccountTaskName = costBill.AccountTaskName;
            log.TheProjectName = costBill.ProjectName;

            if (oDomain.GetType().ToString().IndexOf("CostMonthAccountBill") != -1)
            {
                CostMonthAccountBill model = (CostMonthAccountBill) oDomain;
                log.LogType = "新建月度成本核算主表";
            }
            if (oDomain.GetType().ToString().IndexOf("CostMonthAccountDtl") != -1)
            {
                CostMonthAccountDtl model = (CostMonthAccountDtl) oDomain;
                log.Descripts = "[工程任务名称:" + model.AccountTaskNodeName + "]" + "[工程任务明细名称:" + model.ProjectTaskDtlName +
                                "]"
                                + "[成本项名称:" + model.CostItemName + "]" + "[数量单位名称:" + model.QuantityUnitName + "]"
                                + "[价格单位名称:" + model.PriceUnitName + "]" + "[当前实际工程量:" + model.CurrRealQuantity + "]"
                                + "[当期合同收入实现量:" + model.CurrIncomeQuantity + "]" + "[当期合同收入合价:" +
                                model.CurrIncomeTotalPrice + "]"
                                + "[当期责任成本实现量:" + model.CurrResponsiQuantity + "]" + "[当期责任成本合价:" +
                                model.CurrResponsiTotalPrice + "]"
                                + "[当期挣值:" + model.CurrEarnValue + "]" + "[当期核算形象进度:" + model.CurrCompletedPercent + "]";
                log.LogType = "新建月度工程任务明细核算";
            }
            if (oDomain.GetType().ToString().IndexOf("CostMonthAccDtlConsume") != -1)
            {
                CostMonthAccDtlConsume model = (CostMonthAccDtlConsume) oDomain;
                log.Descripts = "[工程任务明细名称:" + model.ProjectTaskDtlName + "]"
                                + "[定额计量单位:" + model.RationUnitName + "]" + "[核算科目名称:" + model.CostingSubjectName + "]"
                                + "[资源类型名称:" + model.ResourceTypeName + "]" + "[当期实际耗用数量:" +
                                model.CurrRealConsumeQuantity + "]"
                                + "[当期实际耗用单价:" + model.CurrRealConsumePrice + "]" + "[当期实际耗用合价:" +
                                model.CurrRealConsumeTotalPrice + "]"
                                + "[当期实际耗用计划量:" + model.CurrRealConsumePlanQuantity + "]" + "[当期实际耗用计划合价:" +
                                model.CurrRealConsumePlanTotalPrice + "]"
                                + "[当期实现合同收入量:" + model.CurrIncomeQuantity + "]" + "[当期实现合同收入合价:" +
                                model.CurrIncomeTotalPrice + "]"
                                + "[当期责任耗用数量:" + model.CurrResponsiConsumeQuantity + "]" + "[当期责任耗用合价:" +
                                model.CurrResponsiConsumeTotalPrice + "]";
                log.LogType = "新建月度资源耗用明细核算";
            }
            if (oDomain.GetType().ToString().IndexOf("SubContractBalanceSubjectDtl") != -1)
            {
                SubContractBalanceSubjectDtl model = (SubContractBalanceSubjectDtl) oDomain;
                log.Descripts = "[工程任务明细名称:" + model.QuantityUnitName + "]"
                                + "[资源类型名称:" + model.ResourceTypeName + "]" + "[核算科目名称:" + model.BalanceSubjectName +
                                "]"
                                + "[分包结算数量:" + model.BalanceQuantity + "]" + "[分包结算金额:" + model.BalanceTotalPrice + "]";
                log.LogType = "分包结算物资耗用";
            }
            if (oDomain.GetType().ToString().IndexOf("MaterialSettleDetail") != -1)
            {
                MaterialSettleDetail model = (MaterialSettleDetail) oDomain;
                log.Descripts = "[工程任务名称:" + model.ProjectTaskName + "]"
                                + "[资源类型名称:" + model.MaterialName + "]" + "[核算科目名称:" + model.AccountCostName + "]"
                                + "[分包结算数量:" + model.Quantity + "]" + "[分包结算金额:" + model.Money + "]";
                log.LogType = "汇总物资耗用结算单";
            }
            return log;
        }

        /// <summary>
        /// 取得连接字符串的相关信息
        /// 1:取得GWBS节点的层次码
        /// 2:取得[资源类型GUID+"_"+科目GUID]字符串
        /// 3:取得[科目]
        /// </summary>
        private string GetGWBSSyscodeFromLinkStr(string linkStr, int subType)
        {
            string str = "";
            if (subType == 1)
            {
                int index = linkStr.IndexOf("-");
                str = linkStr.Substring(0, index);
            }
            else if (subType == 2)
            {
                str = TransUtil.GetStrByIndex(linkStr, '-', 2, 4);
            }
            else if (subType == 3)
            {
                str = TransUtil.GetStrByIndex(linkStr, '-', 1, 4);
            }
            return str;
        }

        /// <summary>
        /// 本月是否允许月度成本结算,如果上月未测算，或者下月已计算，则本月不允许计算
        /// 0: 可以结算
        /// 1: 上月没结算
        /// 2: 下月已经结算
        /// 3: 本月已经结算
        /// 4: 本月核算节点范围未包容上月范围
        /// </summary>
        /// 
        public int IfHaveAccount(int kjn, int kjy, string projectId, string accountTaskGUID, string accountTaskSyscode)
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
            string sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId +
                         "' and (accounttasksyscode like '%" + accountTaskGUID + "%' or INSTR('" + accountTaskSyscode +
                         "',accounttasksyscode)>0) "; //本项目数据
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

            sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId + "'and kjn= " +
                  kjn
                  + " and kjy=" + kjy + " and (accounttasksyscode like '%" + accountTaskGUID + "%' or INSTR('" +
                  accountTaskSyscode + "',accounttasksyscode)>0) "; //本月数据
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
            //判断本月核算范围是否包容上月的核算范围
            sql = "select  accounttasksyscode,accounttaskname from thd_costmonthaccount where THEPROJECTGUID='" +
                  projectId + "' and kjn= " + last_kjn
                  + " and kjy=" + last_kjy + " and (accounttasksyscode like '%" + accountTaskGUID + "%' or INSTR('" +
                  accountTaskSyscode + "',accounttasksyscode)>0) "; //上月数据
            command.CommandText = sql;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            dataTable = ds.Tables[0];
            string lastTaskSyscode = "";
            string lastTaskName = "";
            foreach (DataRow dataRow in dataTable.Rows)
            {
                lastTaskSyscode = TransUtil.ToString(dataRow["accounttasksyscode"]);
                lastTaskName = TransUtil.ToString(dataRow["accounttaskname"]);
            }
            //无核算
            if (lastTaskSyscode == "")
            {
                return 1;
            }
            else if (!lastTaskSyscode.Contains(accountTaskSyscode))
            {
                return 4;
            }

            sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId + "'and kjn= " +
                  next_kjn
                  + " and kjy=" + next_kjy + " and (accounttasksyscode like '%" + accountTaskGUID + "%' or INSTR('" +
                  accountTaskSyscode + "',accounttasksyscode)>0) "; //下月数据
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

            return ifHave;
        }

        /// <summary>
        /// 本月是否允许反结月度成本,如果上月未测算，或者下月已计算，则本月不允许计算
        /// 0: 可以反结
        /// 1: 下月已结算
        /// 2: 本月未结算
        /// </summary>
        /// 
        public int IfHaveUnAccount(int kjn, int kjy, string projectId, string accountTaskGUID, string accountTaskSyscode)
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

            string sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId +
                         "' and kjn= " + next_kjn
                         + " and kjy=" + next_kjy + " and (accounttasksyscode like '%" + accountTaskGUID +
                         "%' or INSTR('" + accountTaskSyscode + "',accounttasksyscode)>0) "; //下月数据
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

            sql = "select count(*) count from thd_costmonthaccount where THEPROJECTGUID='" + projectId + "' and kjn= " +
                  kjn
                  + " and kjy=" + kjy + " and (accounttasksyscode like '%" + accountTaskGUID + "%' or INSTR('" +
                  accountTaskSyscode + "',accounttasksyscode)>0) "; //本月数据
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

        //补入资源相关信息
        private void UpdateResourceInfo()
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                string sql =
                    " update thd_costmonthaccdtlconsume t1 set (t1.resourcesyscode,t1.resourcetypecode,t1.resourcetypestuff,t1.resourcetypespec)= " +
                    " (select k1.thesyscode||'.'||k1.materialid,k1.matcode,k1.stuff,k1.matspecification from resmaterial k1 where t1.resourcetypeguid=k1.materialid)  " +
                    " where t1.resourcetypecode is null ";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //当反结时清除前驱单据的结算标志
        private void ClearForwardBillFlag(string costMonthAccGUID)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                //工程任务核算单
                string sql =
                    " update thd_projecttaskaccountbill t1 set t1.monthaccountbill='' where t1.monthaccountbill ='" +
                    costMonthAccGUID + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                //分包结算单
                sql = " update thd_subcontractbalancebill t1 set t1.MonthAccBillid='' where t1.MonthAccBillid ='" +
                      costMonthAccGUID + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                //物资耗用单/料具结算单
                sql = " update thd_materialsettlemaster t1 set t1.monthaccountbill='' where t1.monthaccountbill ='" +
                      costMonthAccGUID + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                //设备租赁结算单
                sql =
                    " update THD_MaterialRentelSetMaster t1 set t1.monthaccountbillid='' where t1.monthaccountbillid ='" +
                    costMonthAccGUID + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                //费用结算单
                sql = " update thd_expensessettlemaster t1 set t1.monthlysettlment='' where t1.monthlysettlment ='" +
                      costMonthAccGUID + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                //计时代工
                sql = " update THD_LABORSPORADICMASTER t1 set t1.monthlysettlment='' where t1.monthlysettlment ='" +
                      costMonthAccGUID + "'";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //回写前驱单据的结算标志
        private void WriteForwardBillFlag(Hashtable ht_forwardbill, string costMonthAccGUID)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                string sql = "";
                foreach (string billType in ht_forwardbill.Keys)
                {
                    if (billType == "0") //工程任务核算单
                    {
                        IList billList = (ArrayList) ht_forwardbill[billType];
                        foreach (ProjectTaskAccountBill model in billList)
                        {
                            sql = " update thd_projecttaskaccountbill t1 set t1.monthaccountbill='" + costMonthAccGUID +
                                  "' where t1.id ='" + model.Id + "'";
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                    }
                    if (billType == "1") //分包结算单
                    {
                        IList billList = (ArrayList) ht_forwardbill[billType];
                        foreach (SubContractBalanceBill model in billList)
                        {
                            sql = " update thd_subcontractbalancebill t1 set t1.MonthAccBillid='" + costMonthAccGUID +
                                  "' where t1.id ='" + model.Id + "'";
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                    }
                    if (billType == "2") //物资耗用单
                    {
                        IList billList = (ArrayList) ht_forwardbill[billType];
                        foreach (MaterialSettleMaster model in billList)
                        {
                            sql = " update thd_materialsettlemaster t1 set t1.monthaccountbill='" + costMonthAccGUID +
                                  "' where t1.id ='" + model.Id + "'";
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                    }
                    if (billType == "3") //设备租赁结算单
                    {
                        IList billList = (ArrayList) ht_forwardbill[billType];
                        foreach (MaterialRentalSettlementMaster model in billList)
                        {
                            sql = " update THD_MaterialRentelSetMaster t1 set t1.monthaccountbillid='" +
                                  costMonthAccGUID + "' where t1.id ='" + model.Id + "'";
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                    }
                    if (billType == "4") //料具结算单
                    {
                        sql = " ";
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                    if (billType == "5") //费用结算单
                    {
                        IList billList = (ArrayList) ht_forwardbill[billType];
                        foreach (ExpensesSettleMaster model in billList)
                        {
                            sql = " update thd_expensessettlemaster t1 set t1.monthlysettlment='" + costMonthAccGUID +
                                  "' where t1.id ='" + model.Id + "'";
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void DeleteCostMonthAccount(string condition)
        {
            try
            {
                if (condition == "")
                    return;
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = " delete from thd_costmonthaccount t1 where 1=1 " + condition;

                command.CommandText = sql;
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 保存月度成本核算单据数据和回写相关单据 用纯SQL语句写
        /// </summary>
        /// <param name="oMaster"></param>
        /// <param name="ht_forwardbill"></param>
        private void InsertCostMonthAccountNew(CostMonthAccountBill oMaster, Hashtable ht_forwardbill)
        {
            IDbTransaction oTransaction = null;
            try
            {
                if (oMaster == null)
                {
                    throw new Exception("月度成本核算主表为空,无法保存");
                }
                else
                {
                    IDbDataParameter oParameter = null;
                    IFCGuidGenerator gen = new IFCGuidGenerator();
                    ISession session = CallContext.GetData("nhsession") as ISession;
                    IDbConnection conn = session.Connection;
                    oTransaction = conn.BeginTransaction();
                    IDbCommand oCommand = conn.CreateCommand();
                    oCommand.Transaction = oTransaction;
                    oCommand.CommandType = CommandType.Text;
                    string sInsertCostMonthAccountSQL =
                        "insert into thd_costmonthaccount (ID,VERSION,KJN,KJY,THEPROJECTGUID,THEPROJECTNAME,CREATETIME,ACCOUNTPERSONGUID,ACCOUNTPERSONNAME,ACCOUNTPERSONORGSYSCODE,THEORGNAME,ACCOUNTRANGE,ACCOUNTTASKNAME,BEGINTIME,ENDTIME,REMARK,EXCHANGERATE, ACCOUNTTASKSYSCODE, STATE, ACCOUNTORGGUID, ACCOUNTORGNAME)values(:p1,:p2,:p3,:p4,:p5,:p6,:p7,:p8,:p9,:p10,:p11,:p12,:p13,:p14,:p15,:p16,:p17,:p18,:p19,:p20,:p21)";
                    string sInsertCostMonthAccountDtlSQL =
                        "insert into thd_costmonthaccountdtl (ID, ACCOUNTTASKNODEGUID, ACCOUNTTASKNODENAME, ACCOUNTTASKNODESYSCODE, PROJECTTASKDTLGUID, PROJECTTASKDTLNAME, THECOSTITEM, COSTITEMNAME, QUANTITYUNITGUID, QUANTITYUNITNAME, PRICEUNITGUID, PRICEUNITNAME, REMARK, PARENTID, CURRREALQUANTITY, CURRREALPRICE, CURRREALTOTALPRICE, CURRINCOMEQUANTITY, CURRINCOMETOTALPRICE, CURRRESPONSIQUANTITY, CURRRESPONSITOTALPRICE, CURREARNVALUE, CURRCOMPLETEDPERCENT, SUMREALQUANTITY, SUMREALTOTALPRICE, SUMINCOMEQUANTITY, SUMINCOMETOTALPRICE, SUMRESPONSIQUANTITY, SUMRESPONSITOTALPRICE, SUMEARNVALUE, SUMCOMPLETEDPERCENT, IFTASKACCTMX, DUDGETCONTRACTQUANTITY, DUDGETCONTRACTTOTALPRICE, DUDGETRESPQUANTITY, DUDGETRESPTOTALPRICE, DUDGETPLANQUANTITY, DUDGETPLANTOTALPRICE)values(:p1,:p2,:p3,:p4,:p5,:p6,:p7,:p8,:p9,:p10,:p11,:p12,:p13,:p14,:p15,:p16,:p17,:p18,:p19,:p20,:p21,:p22,:p23,:p24,:p25,:p26,:p27,:p28,:p29,:p30,:p31,:p32,:p33,:p34,:p35,:p36,:p37,:p38)";
                    string sInsertCostMonthAccountDtlConsumeSQL =
                        "insert into thd_costmonthaccdtlconsume (ID, RATIONUNITGUID, RATIONUNITNAME, COSTSUBJECTSYSCODE, COSTINGSUBJECTGUID, COSTINGSUBJECTNAME, RESOURCETYPEGUID, RESOURCETYPENAME, RESOURCESYSCODE, COSTMONTHDTLID, CURRREALCONSUMEQUANTITY, CURRREALCONSUMEPRICE, CURRREALCONSUMETOTALPRICE, CURRREALCONSUMEPLANQUANTITY, CURRREALCONSUMEPLANTOTALPRICE, CURRINCOMEQUANTITY, CURRINCOMETOTALPRICE, CURRRESPONSICONSUMEQUANTITY, CURRRESPONSICONSUMETOTALPRICE, SUMREALCONSUMEQUANTITY, SUMREALCONSUMETOTALPRICE, SUMREALCONSUMEPLANQUANTITY, SUMREALCONSUMEPLANTOTALPRICE, SUMINCOMEQUANTITY, SUMINCOMETOTALPRICE, SUMRESPONSICONSUMEQUANTITY, SUMRESPONSICONSUMETOTALPRICE, PARENTID, PROJECTTASKDTLGUID, RESOURCETYPESTUFF, RESOURCETYPESPEC, COSTSUBJECTCODE, RESOURCETYPECODE, DIAGRAMNUMBER, DUDGETCONTRACTQUANTITY, DUDGETCONTRACTTOTALPRICE, DUDGETRESPQUANTITY, DUDGETRESPTOTALPRICE, DUDGETPLANQUANTITY, DUDGETPLANTOTALPRICE,ACCOUNTTASKNODEGUID,ACCOUNTTASKNODENAME,ACCOUNTTASKNODESYSCODE,CALTYPE,SOURCETYPE,SOURCEID)values (:p1,:p2,:p3,:p4,:p5,:p6,:p7,:p8,:p9,:p10,:p11,:p12,:p13,:p14,:p15,:p16,:p17,:p18,:p19,:p20,:p21,:p22,:p23,:p24,:p25,:p26,:p27,:p28,:p29,:p30,:p31,:p32,:p33,:p34,:p35,:p36,:p37,:p38,:p39,:p40,:p41,:p42,:p43,:p44,:p45,:p46)";
                    string sUpdateProjectTaskAccountBill =
                        "update thd_projecttaskaccountbill t1 set t1.monthaccountbill=:CostMonthAccountID where t1.id =:Id";
                    string sUpdateSubcontractBalanceBill =
                        "update thd_subcontractbalancebill t1 set t1.MonthAccBillid=:CostMonthAccountID where t1.id =:Id";
                    string sUpdateMaterialSettleMasterBill =
                        "update thd_materialsettlemaster t1 set t1.monthaccountbill=:CostMonthAccountID where t1.id =:Id";
                    string sUpdateMaterialRentelSetMaster =
                        "update THD_MaterialRentelSetMaster t1 set t1.monthaccountbillid=:CostMonthAccountID where t1.id =:Id";
                    string sUpdateExpensesSettleMaster =
                        "update thd_expensessettlemaster t1 set t1.monthlysettlment=:CostMonthAccountID where t1.id =:Id";
                    string sUpdateLaborMaster =
                        "update THD_LABORSPORADICMASTER t1 set t1.monthlysettlment=:CostMonthAccountID where t1.id =:Id";
                    ArrayList billList = null;

                    #region 保存月度成本核算记录

                    #region 月度成本主表

                    oCommand.CommandText = sInsertCostMonthAccountSQL;
                    oCommand.Parameters.Clear();
                    oMaster.Id = gen.GeneratorIFCGuid();
                    CreateParameter(oCommand, "p1", oMaster.Id, DbType.String);
                    CreateParameter(oCommand, "p2", oMaster.Version, DbType.Int64);
                    CreateParameter(oCommand, "p3", oMaster.Kjn, DbType.Int32);
                    CreateParameter(oCommand, "p4", oMaster.Kjy, DbType.Int32);
                    CreateParameter(oCommand, "p5", oMaster.ProjectId, DbType.String);
                    CreateParameter(oCommand, "p6", oMaster.ProjectName, DbType.String);
                    CreateParameter(oCommand, "p7", oMaster.CreateDate, DbType.Date);
                    CreateParameter(oCommand, "p8",
                                    oMaster.AccountPersonGUID == null ? null : oMaster.AccountPersonGUID.Id,
                                    DbType.String);
                    CreateParameter(oCommand, "p9", oMaster.AccountPersonName, DbType.String);
                    CreateParameter(oCommand, "p10", oMaster.AccountPersonOrgSysCode, DbType.String);
                    CreateParameter(oCommand, "p11", oMaster.TheOrgName, DbType.String);
                    CreateParameter(oCommand, "p12", oMaster.AccountRange == null ? null : oMaster.AccountRange.Id,
                                    DbType.String);
                    CreateParameter(oCommand, "p13", oMaster.AccountTaskName, DbType.String);
                    CreateParameter(oCommand, "p14", oMaster.BeginTime, DbType.Date);
                    CreateParameter(oCommand, "p15", oMaster.EndTime, DbType.Date);
                    CreateParameter(oCommand, "p16", oMaster.Remark, DbType.String); //REMARK
                    CreateParameter(oCommand, "p17", oMaster.ExchangeRate, DbType.Decimal); //,EXCHANGERATE,
                    CreateParameter(oCommand, "p18", oMaster.AccountTaskSysCode, DbType.String); // ACCOUNTTASKSYSCODE,
                    CreateParameter(oCommand, "p19", oMaster.DocState, DbType.Int16); // STATE
                    CreateParameter(oCommand, "p20", oMaster.AccountOrgGUID, DbType.String); //, ACCOUNTORGGUID,
                    CreateParameter(oCommand, "p21", oMaster.AccountOrgName, DbType.String); // ACCOUNTORGNAME
                    oCommand.ExecuteNonQuery();

                    #endregion

                    #region 月度成本明细

                    oCommand.CommandText = sInsertCostMonthAccountDtlSQL;
                    foreach (CostMonthAccountDtl oDetail in oMaster.Details)
                    {
                        oCommand.Parameters.Clear();
                        oDetail.Id = gen.GeneratorIFCGuid();
                        CreateParameter(oCommand, "p1", oDetail.Id, DbType.String);
                        CreateParameter(oCommand, "p2",
                                        oDetail.AccountTaskNodeGUID == null ? null : oDetail.AccountTaskNodeGUID.Id,
                                        DbType.String);
                        CreateParameter(oCommand, "p3", oDetail.AccountTaskNodeName, DbType.String);
                        CreateParameter(oCommand, "p4", oDetail.AccountTaskNodeSyscode, DbType.String);
                        CreateParameter(oCommand, "p5",
                                        oDetail.ProjectTaskDtlGUID == null ? null : oDetail.ProjectTaskDtlGUID.Id,
                                        DbType.String);
                        CreateParameter(oCommand, "p6", oDetail.ProjectTaskDtlName, DbType.String);
                        CreateParameter(oCommand, "p7", oDetail.TheCostItem == null ? null : oDetail.TheCostItem.Id,
                                        DbType.String);
                        CreateParameter(oCommand, "p8", oDetail.CostItemName, DbType.String);
                        CreateParameter(oCommand, "p9",
                                        oDetail.QuantityUnitGUID == null ? null : oDetail.QuantityUnitGUID.Id,
                                        DbType.String);
                        CreateParameter(oCommand, "p10", oDetail.QuantityUnitName, DbType.String);
                        CreateParameter(oCommand, "p11", oDetail.PriceUnitGUID == null ? null : oDetail.PriceUnitGUID.Id,
                                        DbType.String);
                        CreateParameter(oCommand, "p12", oDetail.PriceUnitName, DbType.String);
                        CreateParameter(oCommand, "p13", oDetail.Remark, DbType.String);
                        CreateParameter(oCommand, "p14", oDetail.TheAccountBill.Id, DbType.String);
                        CreateParameter(oCommand, "p15", oDetail.CurrRealQuantity, DbType.String);
                        CreateParameter(oCommand, "p16", oDetail.CurrRealPrice, DbType.String);
                        CreateParameter(oCommand, "p17", oDetail.CurrRealTotalPrice, DbType.String);
                        CreateParameter(oCommand, "p18", oDetail.CurrIncomeQuantity, DbType.String);
                        CreateParameter(oCommand, "p19", oDetail.CurrIncomeTotalPrice, DbType.String);
                        CreateParameter(oCommand, "p20", oDetail.CurrResponsiQuantity, DbType.String);
                        CreateParameter(oCommand, "p21", oDetail.CurrResponsiTotalPrice, DbType.String);
                        CreateParameter(oCommand, "p22", oDetail.CurrEarnValue, DbType.String);
                        CreateParameter(oCommand, "p23", oDetail.CurrCompletedPercent, DbType.String);
                        CreateParameter(oCommand, "p24", oDetail.SumRealQuantity, DbType.String);
                        CreateParameter(oCommand, "p25", oDetail.SumRealTotalPrice, DbType.String);
                        CreateParameter(oCommand, "p26", oDetail.SumIncomeQuantity, DbType.String);
                        CreateParameter(oCommand, "p27", oDetail.SumIncomeTotalPrice, DbType.String);
                        CreateParameter(oCommand, "p28", oDetail.SumResponsiQuantity, DbType.String);
                        CreateParameter(oCommand, "p29", oDetail.SumResponsiTotalPrice, DbType.String);
                        CreateParameter(oCommand, "p30", oDetail.SumEarnValue, DbType.String);
                        CreateParameter(oCommand, "p31", oDetail.SumCompletedPercent, DbType.String);
                        CreateParameter(oCommand, "p32", oDetail.IfTaskAcctMx, DbType.String);
                        CreateParameter(oCommand, "p33", oDetail.DudgetContractQuantity, DbType.String);
                        CreateParameter(oCommand, "p34", oDetail.DudgetContractTotalPrice, DbType.String);
                        CreateParameter(oCommand, "p35", oDetail.DudgetRespQuantity, DbType.String);
                        CreateParameter(oCommand, "p36", oDetail.DudgetRespTotalPrice, DbType.String);
                        CreateParameter(oCommand, "p37", oDetail.DudgetPlanQuantity, DbType.String);
                        CreateParameter(oCommand, "p38", oDetail.DudgetPlanTotalPrice, DbType.String);
                        oCommand.ExecuteNonQuery();
                    }

                    #endregion

                    #region 月度成本明细

                    oCommand.CommandText = sInsertCostMonthAccountDtlConsumeSQL;

                    foreach (CostMonthAccountDtl oDetail in oMaster.Details)
                    {
                        foreach (CostMonthAccDtlConsume oConsume in oDetail.Details)
                        {

                            oCommand.Parameters.Clear();
                            oConsume.Id = gen.GeneratorIFCGuid();
                            CreateParameter(oCommand, "p1", oConsume.Id, DbType.String);
                            CreateParameter(oCommand, "p2",
                                            oConsume.RationUnitGUID == null ? null : oConsume.RationUnitGUID.Id,
                                            DbType.String);
                            CreateParameter(oCommand, "p3", oConsume.RationUnitName, DbType.String);
                            CreateParameter(oCommand, "p4", oConsume.CostSubjectSyscode, DbType.String);
                            CreateParameter(oCommand, "p5",
                                            oConsume.CostingSubjectGUID == null ? null : oConsume.CostingSubjectGUID.Id,
                                            DbType.String);
                            CreateParameter(oCommand, "p6", oConsume.CostingSubjectName, DbType.String);
                            CreateParameter(oCommand, "p7", oConsume.ResourceTypeGUID, DbType.String);
                            CreateParameter(oCommand, "p8", oConsume.ResourceTypeName, DbType.String);
                            CreateParameter(oCommand, "p9", oConsume.ResourceSyscode, DbType.String);
                            CreateParameter(oCommand, "p10", null, DbType.String);
                            CreateParameter(oCommand, "p11", oConsume.CurrRealConsumeQuantity, DbType.String);
                            CreateParameter(oCommand, "p12", oConsume.CurrRealConsumePrice, DbType.String);
                            CreateParameter(oCommand, "p13", oConsume.CurrRealConsumeTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p14", oConsume.CurrRealConsumePlanQuantity, DbType.String);
                            CreateParameter(oCommand, "p15", oConsume.CurrRealConsumePlanTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p16", oConsume.CurrIncomeQuantity, DbType.String);
                            CreateParameter(oCommand, "p17", oConsume.CurrIncomeTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p18", oConsume.CurrResponsiConsumeQuantity, DbType.String);
                            CreateParameter(oCommand, "p19", oConsume.CurrResponsiConsumeTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p20", oConsume.SumRealConsumeQuantity, DbType.String);
                            CreateParameter(oCommand, "p21", oConsume.SumRealConsumeTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p22", oConsume.SumRealConsumePlanQuantity, DbType.String);
                            CreateParameter(oCommand, "p23", oConsume.SumRealConsumePlanTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p24", oConsume.SumIncomeQuantity, DbType.String);
                            CreateParameter(oCommand, "p25", oConsume.SumIncomeTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p26", oConsume.SumResponsiConsumeQuantity, DbType.String);
                            CreateParameter(oCommand, "p27", oConsume.SumResponsiConsumeTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p28", oConsume.TheAccountDetail.Id, DbType.String);
                            CreateParameter(oCommand, "p29", oConsume.ProjectTaskDtlGUID, DbType.String);
                            CreateParameter(oCommand, "p30", oConsume.ResourceTypeStuff, DbType.String);
                            CreateParameter(oCommand, "p31", oConsume.ResourceTypeSpec, DbType.String);
                            CreateParameter(oCommand, "p32", oConsume.CostSubjectCode, DbType.String);
                            CreateParameter(oCommand, "p33", oConsume.ResourceTypeCode, DbType.String);
                            CreateParameter(oCommand, "p34", oConsume.DiagramNumber, DbType.String);
                            CreateParameter(oCommand, "p35", oConsume.DudgetContractQuantity, DbType.String);
                            CreateParameter(oCommand, "p36", oConsume.DudgetContractTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p37", oConsume.DudgetRespQuantity, DbType.String);
                            CreateParameter(oCommand, "p38", oConsume.DudgetRespTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p39", oConsume.DudgetPlanQuantity, DbType.String);
                            CreateParameter(oCommand, "p40", oConsume.DudgetPlanTotalPrice, DbType.String);
                            CreateParameter(oCommand, "p41", oConsume.AccountTaskNodeGUID, DbType.String);
                            CreateParameter(oCommand, "p42", oConsume.AccountTaskNodeName, DbType.String);
                            CreateParameter(oCommand, "p43", oConsume.AccountTaskNodeSyscode, DbType.String);
                            CreateParameter(oCommand, "p44", oConsume.CalType, DbType.String);
                            CreateParameter(oCommand, "p45", oConsume.SourceType, DbType.String);
                            CreateParameter(oCommand, "p46", oConsume.SourceId, DbType.String);
                            oCommand.ExecuteNonQuery();

                        }
                    }

                    #endregion

                    #endregion

                    #region 回写前驱单据的结算标志

                    foreach (string billType in ht_forwardbill.Keys)
                    {
                        billList = (ArrayList) ht_forwardbill[billType];
                        if (billList != null && billList.Count > 0)
                        {
                            if (billType == "0") //工程任务核算单
                            {
                                oCommand.CommandText = sUpdateProjectTaskAccountBill;
                                foreach (ProjectTaskAccountBill oBill in billList)
                                {
                                    oCommand.Parameters.Clear();
                                    CreateParameter(oCommand, "CostMonthAccountID", oMaster.Id, DbType.String);
                                    CreateParameter(oCommand, "Id", oBill.Id, DbType.String);
                                    oCommand.ExecuteNonQuery();
                                }
                            }
                            if (billType == "1") //分包结算单
                            {
                                oCommand.CommandText = sUpdateSubcontractBalanceBill;
                                foreach (SubContractBalanceBill oBill in billList)
                                {
                                    oCommand.Parameters.Clear();
                                    CreateParameter(oCommand, "CostMonthAccountID", oMaster.Id, DbType.String);
                                    CreateParameter(oCommand, "Id", oBill.Id, DbType.String);
                                    oCommand.ExecuteNonQuery();
                                }
                            }
                            if (billType == "2") //物资耗用单
                            {
                                oCommand.CommandText = sUpdateMaterialSettleMasterBill;
                                foreach (MaterialSettleMaster oBill in billList)
                                {
                                    //sql = " update thd_materialsettlemaster t1 set t1.monthaccountbill='" + costMonthAccGUID + "' where t1.id ='" + model.Id + "'";
                                    oCommand.Parameters.Clear();
                                    CreateParameter(oCommand, "CostMonthAccountID", oMaster.Id, DbType.String);
                                    CreateParameter(oCommand, "Id", oBill.Id, DbType.String);
                                    oCommand.ExecuteNonQuery();
                                }
                            }
                            if (billType == "3") //设备租赁结算单
                            {
                                oCommand.CommandText = sUpdateMaterialRentelSetMaster;
                                foreach (MaterialRentalSettlementMaster oBill in billList)
                                {
                                    //sql = " update THD_MaterialRentelSetMaster t1 set t1.monthaccountbillid='" + costMonthAccGUID + "' where t1.id ='" + model.Id + "'";
                                    oCommand.Parameters.Clear();
                                    CreateParameter(oCommand, "CostMonthAccountID", oMaster.Id, DbType.String);
                                    CreateParameter(oCommand, "Id", oBill.Id, DbType.String);
                                    oCommand.ExecuteNonQuery();
                                }
                            }
                            if (billType == "4") //料具结算单
                            {
                                //sql = " ";
                                //command.CommandText = sql;
                                //command.ExecuteNonQuery();
                            }
                            if (billType == "5") //费用结算单
                            {
                                oCommand.CommandText = sUpdateExpensesSettleMaster;
                                foreach (ExpensesSettleMaster oBill in billList)
                                {
                                    oCommand.Parameters.Clear();
                                    CreateParameter(oCommand, "CostMonthAccountID", oMaster.Id, DbType.String);
                                    CreateParameter(oCommand, "Id", oBill.Id, DbType.String);
                                    oCommand.ExecuteNonQuery();
                                }
                            }
                            if (billType == "6")
                            {
                                oCommand.CommandText = sUpdateLaborMaster;
                                foreach (LaborSporadicMaster oBill in billList)
                                {
                                    oCommand.Parameters.Clear();
                                    CreateParameter(oCommand, "CostMonthAccountID", oMaster.Id, DbType.String);
                                    CreateParameter(oCommand, "Id", oBill.Id, DbType.String);
                                    oCommand.ExecuteNonQuery();
                                }

                            }
                        }
                    }

                    #endregion

                    oTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                if (oTransaction != null)
                {
                    oTransaction.Rollback();
                }
                throw new Exception(string.Format("保存月度成本核算失败:{0}", ex.Message));
            }

        }

        public void CreateParameter(IDbCommand oCommand, string sParameterName, object oValue, DbType type)
        {

            IDbDataParameter oParameter = oCommand.CreateParameter();
            oParameter.ParameterName = sParameterName;
            if (oValue == null)
            {
                oParameter.Value = DBNull.Value;
            }
            else
            {
                oParameter.Value = oValue;
            }
            oParameter.DbType = type;
            oCommand.Parameters.Add(oParameter);
        }

        [TransManager]
        private void InsertCostMonthAccount(CostMonthAccountBill model)
        {
            try
            {
                IFCGuidGenerator gen = new IFCGuidGenerator();
                string guid = gen.GeneratorIFCGuid();

                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();

                string sql =
                    " insert into thd_costmonthaccount (ID,VERSION,KJN,KJY,THEPROJECTGUID,THEPROJECTNAME,CREATETIME,ACCOUNTPERSONGUID,ACCOUNTPERSONNAME, " +
                    " ACCOUNTPERSONORGSYSCODE,THEORGNAME,ACCOUNTRANGE,ACCOUNTTASKNAME,BEGINTIME,ENDTIME,REMARK,EXCHANGERATE,ACCOUNTTASKSYSCODE) values " +
                    "('" + guid + "',0," + model.Kjn + "," + model.Kjy + ",'" + model.ProjectId + "','" +
                    model.ProjectName + "',to_date('" + model.CreateDate.ToShortDateString() + "','yyyy-mm-dd')," +
                    " '" + model.AccountPersonGUID + "','" + model.AccountPersonName + "','" +
                    model.AccountPersonOrgSysCode + "','" + model.TheOrgName + "'," +
                    " '" + model.AccountRange + "','" + model.AccountTaskName + "',to_date('" +
                    model.BeginTime.ToShortDateString() + "','yyyy-mm-dd')," +
                    " to_date('" + model.EndTime.ToShortDateString() + "','yyyy-mm-dd'),'" + model.Remark + "'," +
                    model.ExchangeRate + ",'" + model.AccountTaskSysCode + "' ) ";

                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private Hashtable GetSubBalBasicData()
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = "select t1.code,t1.id,t1.name,t1.syscode From thd_costaccountsubject t1 where t1.code in ('C514') ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CostAccountSubject model = new CostAccountSubject();
                    model.Id = TransUtil.ToString(dataRow["id"]);
                    model.Name = TransUtil.ToString(dataRow["name"]);
                    model.SysCode = TransUtil.ToString(dataRow["syscode"]);
                    if (!ht.Contains(dataRow["code"]))
                    {
                        ht.Add(TransUtil.ToString(dataRow["code"]), model);
                    }
                }
            }
            return ht;
        }

        [TransManager]
        private void InsertCostMonthAccountDtlByBatch(IList list)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                foreach (CostMonthAccountDtl model in list)
                {
                    string sql = " ";

                    command.CommandText = sql;
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
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                foreach (CostMonthAccDtlConsume model in list)
                {
                    string sql = " ";

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //批量插入核算过程日志
        private void InsertCostMonthAccountLogByBatch(IList list)
        {
            try
            {
                IFCGuidGenerator gen = new IFCGuidGenerator();
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                foreach (CostMonthAccountLog model in list)
                {
                    string guid = gen.GeneratorIFCGuid();
                    string sql =
                        " insert into thd_CostMonthAccountLog (Id,TheProjectName,Kjn,Kjy,SerialNum,AccountTaskName,LogType,Descripts) values ( '" +
                        guid + "','" + model.TheProjectName + "'," + model.Kjn + "," + model.Kjy + "," + model.SerialNum +
                        ",'" + model.AccountTaskName + "','"
                        + model.LogType + "','" + model.Descripts + "')";

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //批量插入核算过程日志
        private void InsertTemp2()
        {
            try
            {
                IFCGuidGenerator gen = new IFCGuidGenerator();
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                for (int t = 0; t < 20000; t++)
                {
                    string guid = gen.GeneratorIFCGuid();
                    string sql = " insert into temp_2 (var2) values ( '" + guid + "')";

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 成本核算日志查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet QuerytCostMonthAccountLog(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string sql =
                @"select Id,TheProjectName,Kjn,Kjy,SerialNum,AccountTaskName,LogType,Descripts from thd_CostMonthAccountLog t1 where 1=1  ";
            sql = sql + condition + " ORDER BY t1.SerialNum asc ";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 删除成本核算日志
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private void DeleteCostMonthAccountLog(string condition)
        {
            try
            {
                if (condition == "")
                    return;
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = " delete from thd_costmonthaccountlog t1 where 1=1 " + condition;

                command.CommandText = sql;
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 查找月度成本核算主表GUID
        /// </summary>
        /// <returns></returns>
        private string GetCostMonthAccountGuid(int kjn, int kjy, string projectId)
        {
            string id = "";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = "select t1.id from thd_costmonthaccount t1 where t1.kjn=" + kjn + " and t1.kjy=" + kjy +
                                  " and t1.theprojectguid='" + projectId + "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    id = TransUtil.ToString(dataRow["id"]);
                }
            }
            return id;
        }

        /// <summary>
        /// 查询科目信息
        /// </summary>
        /// <returns></returns>
        private Hashtable GetCostSubjectList()
        {
            return GetCostSubjectList(false);
        }

        private Hashtable GetCostSubjectList(bool isOnlyOnHide)
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            if (isOnlyOnHide)
                command.CommandText = "select t1.id,t1.code From thd_costaccountsubject t1 where t1.state = 1 ";
            else
                command.CommandText = "select t1.id,t1.code From thd_costaccountsubject t1  ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (!ht.Contains(TransUtil.ToString(dataRow["id"])))
                    {
                        ht.Add(TransUtil.ToString(dataRow["id"]), TransUtil.ToString(dataRow["code"]));
                    }
                }
            }
            return ht;
        }

        public Hashtable GetCostSubjectNameList()
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = "select t1.id,t1.name From thd_costaccountsubject t1";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (!ht.Contains(TransUtil.ToString(dataRow["id"])))
                    {
                        ht.Add(TransUtil.ToString(dataRow["id"]), TransUtil.ToString(dataRow["name"]));
                    }
                }
            }
            return ht;
        }
        /// <summary>
        /// 通过工程任务明细ID查询上期的月度核算明细累计值信息
        /// </summary>
        /// <returns></returns>
        private CostMonthAccountDtl QueryLastCostMonthAcctDtlData(string projectTaskDtlGUID)
        {
            string maxCreateDate = "";
            CostMonthAccountDtl dtl = new CostMonthAccountDtl();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText =
                "select max(t1.createtime) createtime from thd_costmonthaccount t1,thd_costmonthaccountdtl t2 where t1.id=t2.parentid " +
                " and t2.projecttaskdtlguid='" + projectTaskDtlGUID + "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    maxCreateDate = TransUtil.ToDateTime(dataRow["createtime"]).ToShortDateString();
                }
            }

            command.CommandText =
                "select t2.sumrealquantity,t2.sumrealtotalprice,t2.sumincomequantity,t2.sumincometotalprice," +
                " t2.sumresponsiquantity,t2.sumresponsitotalprice,t2.sumearnvalue,t2.sumcompletedpercent " +
                " from thd_costmonthaccount t1,thd_costmonthaccountdtl t2 where t1.id=t2.parentid " +
                " and t2.projecttaskdtlguid='" + projectTaskDtlGUID + "' and t1.createtime = to_date('" + maxCreateDate +
                "','yyyy-mm-dd')";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    dtl.SumCompletedPercent = TransUtil.ToDecimal(dataRow["sumcompletedpercent"]);
                    dtl.SumEarnValue = TransUtil.ToDecimal(dataRow["sumearnvalue"]);
                    dtl.SumIncomeQuantity = TransUtil.ToDecimal(dataRow["sumincomequantity"]);
                    dtl.SumIncomeTotalPrice = TransUtil.ToDecimal(dataRow["sumincometotalprice"]);
                    dtl.SumRealQuantity = TransUtil.ToDecimal(dataRow["sumrealquantity"]);
                    dtl.SumRealTotalPrice = TransUtil.ToDecimal(dataRow["sumrealtotalprice"]);
                    dtl.SumResponsiQuantity = TransUtil.ToDecimal(dataRow["sumresponsiquantity"]);
                    dtl.SumResponsiTotalPrice = TransUtil.ToDecimal(dataRow["sumresponsitotalprice"]);
                }
            }
            return dtl;
        }

        /// <summary>
        /// 通过[工程任务明细ID+资源类型ID+成本科目ID]查询上期的物资耗用累计值信息
        /// </summary>
        /// <returns></returns>
        private CostMonthAccDtlConsume QueryLastCostMonthAcctDtlConsumeData(string projectTaskDtlGUID,
                                                                            string resTypeGUID, string costSubjectGUID)
        {
            string maxCreateDate = "";
            CostMonthAccDtlConsume dtlConsume = new CostMonthAccDtlConsume();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText =
                "select max(t1.createtime) createtime from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3 " +
                " where t1.id=t2.parentid and t2.id =t3.parentid and t3.projecttaskdtlguid='" + projectTaskDtlGUID +
                "' and t3.costingsubjectguid='" + costSubjectGUID + "' " + " and t3.resourcetypeguid='" + resTypeGUID +
                "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    maxCreateDate = TransUtil.ToString(dataRow["createtime"]);
                }
            }

            command.CommandText =
                "select t3.sumrealconsumequantity,t3.sumrealconsumetotalprice,t3.sumrealconsumeplanquantity," +
                " t3.sumrealconsumeplantotalprice,t3.sumincomequantity,t3.sumincometotalprice,t3.sumresponsiconsumequantity," +
                " t3.sumresponsiconsumetotalprice from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3" +
                " where t1.id=t2.parentid and t2.id =t3.parentid and t3.projecttaskdtlguid='" + projectTaskDtlGUID +
                "' and t3.costingsubjectguid='" + costSubjectGUID + "' " + " and t3.resourcetypeguid='" + resTypeGUID +
                "'" +
                " and t1.createtime = to_date('" + maxCreateDate + "','yyyy-mm-dd')";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    dtlConsume.SumIncomeQuantity = TransUtil.ToDecimal(dataRow["sumincomequantity"]);
                    dtlConsume.SumIncomeTotalPrice = TransUtil.ToDecimal(dataRow["sumincometotalprice"]);
                    dtlConsume.SumRealConsumePlanQuantity = TransUtil.ToDecimal(dataRow["sumrealconsumeplanquantity"]);
                    dtlConsume.SumRealConsumePlanTotalPrice =
                        TransUtil.ToDecimal(dataRow["sumrealconsumeplantotalprice"]);
                    dtlConsume.SumRealConsumeQuantity = TransUtil.ToDecimal(dataRow["sumrealconsumequantity"]);
                    dtlConsume.SumRealConsumeTotalPrice = TransUtil.ToDecimal(dataRow["sumrealconsumetotalprice"]);
                    dtlConsume.SumResponsiConsumeQuantity = TransUtil.ToDecimal(dataRow["sumresponsiconsumequantity"]);
                    dtlConsume.SumResponsiConsumeTotalPrice =
                        TransUtil.ToDecimal(dataRow["sumresponsiconsumetotalprice"]);
                }
            }
            return dtlConsume;
        }

        #endregion

        /// <summary>
        /// 月度成本核算反结
        /// </summary>
        [TransManager]
        public void UnCostMonthAccountCal(string costMonthAcctGUID)
        {
            //删除本月数据
            string condition = " and t1.id='" + costMonthAcctGUID + "'";
            this.DeleteCostMonthAccount(condition);

            ////清除前驱单据标志
            this.ClearForwardBillFlag(costMonthAcctGUID);

            //删除本月过程日志
            //condition = " and t1.kjn=" + kjn + " and t1.kjy=" + kjy + " and t1.theprojectname='" + projectInfo.Name + "'";
            //this.DeleteCostMonthAccountLog(condition);
        }

        #endregion

        #region 工程任务明细台账

        /// <summary>
        /// 查询工程任务明细台账信息
        /// </summary>
        [TransManager]
        public IList SearchGWBSDetailLedger(CurrentProjectInfo projectInfo, DateTime dtStartDate, DateTime dtEndDate)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql =
                @"select t3.*,t4.CostItemName,t4.workamountunitname,t4.contractprojectquantity from ( " +
                " select t1.ContractWorkAmount,t1.ContractPrice,t1.ContractTotalPrice,t1.ResponsibleWorkAmount,t1.ResponsiblePrice,t1.ResponsibleTotalPrice," +
                " t1.PlanWorkAmount,t1.PlanPrice,t1.PlanTotalPrice,t2.ContractGroupType,t2.contractnumber,t1.ProjectTaskDtlID,t1.ProjectTaskDtlName,t1.createtime " +
                " from thd_gwbsdetailledger t1 inner join thd_contractgroup t2 on t1.contractgroupid = t2.id) t3 " +
                " left join thd_gwbsdetail t4 on t3.ProjectTaskDtlID = t4.id";
            sql += " where TheProjectGUID = '" + projectInfo.Id + "' and t3.CreateTime>=to_date('" + dtStartDate +
                   "','yyyy-mm-dd hh24:mi:ss') " +
                   " and t3.CreateTime<to_date('" + dtEndDate +
                   "','yyyy-mm-dd hh24:mi:ss') order by t3.ContractGroupType ";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            IList list = new ArrayList();
            string strGroupType = null;
            string strUnit = null;
            string strCostItemName = null;
            decimal sumquantity1 = 0;
            decimal summoney1 = 0;
            decimal sumquantity2 = 0;
            decimal summoney2 = 0;
            decimal sumquantity3 = 0;
            decimal summoney3 = 0;
            int insert = 0;
            int index = 0;
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++) //循环读取临时表的行
            {
                GWBSDetailLedger detailleger = new GWBSDetailLedger();
                detailleger.ContractWorkAmount =
                    Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractWorkAmount"].ToString());
                detailleger.ContractPrice = Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractPrice"].ToString());
                detailleger.ContractTotalPrice =
                    Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractTotalPrice"].ToString());

                detailleger.ResponsibleWorkAmount =
                    Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsibleWorkAmount"].ToString());
                detailleger.ResponsiblePrice =
                    Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsiblePrice"].ToString());
                detailleger.ResponsibleTotalPrice =
                    Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsibleTotalPrice"].ToString());

                detailleger.PlanWorkAmount = Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanWorkAmount"].ToString());
                detailleger.PlanPrice = Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanPrice"].ToString());
                detailleger.PlanTotalPrice = Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanTotalPrice"].ToString());

                detailleger.Temp5 = Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractTotalPrice"].ToString()) -
                                    Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanTotalPrice"].ToString());
                if (Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractTotalPrice"].ToString()) != 0)
                {
                    detailleger.Temp4 = detailleger.Temp5/
                                        Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractTotalPrice"].ToString());
                }
                else
                {
                    detailleger.Temp4 = 0;
                }
                detailleger.Temp3 = dataSet.Tables[0].Rows[i]["contractnumber"].ToString();
                detailleger.ProjectTaskDtlID = dataSet.Tables[0].Rows[i]["ProjectTaskDtlID"].ToString();
                detailleger.ProjectTaskDtlName = dataSet.Tables[0].Rows[i]["ProjectTaskDtlName"].ToString();
                detailleger.Temp2 = dataSet.Tables[0].Rows[i]["CostItemName"].ToString();
                detailleger.WorkAmountUnitName = dataSet.Tables[0].Rows[i]["WorkAmountUnitName"].ToString();
                detailleger.Temp1 = dataSet.Tables[0].Rows[i]["ContractGroupType"].ToString();
                if (strGroupType != null)
                {
                    if (strGroupType.Equals(dataSet.Tables[0].Rows[i]["ContractGroupType"].ToString())) //类型相同，数量金额相加
                    {
                        if (i < dataSet.Tables[0].Rows.Count - 1)
                        {
                            //类型相同
                            sumquantity1 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractWorkAmount"].ToString());
                            summoney1 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractTotalPrice"].ToString());
                            sumquantity2 +=
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsibleWorkAmount"].ToString());
                            summoney2 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsibleTotalPrice"].ToString());
                            sumquantity3 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanWorkAmount"].ToString());
                            summoney3 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanTotalPrice"].ToString());
                            list.Add(detailleger);
                            index += 1;
                        }
                        else
                        {
                            //类型相同也是最后一条
                            sumquantity1 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractWorkAmount"].ToString());
                            summoney1 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractTotalPrice"].ToString());
                            sumquantity2 +=
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsibleWorkAmount"].ToString());
                            summoney2 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsibleTotalPrice"].ToString());
                            sumquantity3 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanWorkAmount"].ToString());
                            summoney3 += Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanTotalPrice"].ToString());
                            list.Add(detailleger);
                            detailleger = new GWBSDetailLedger();
                            detailleger.ContractWorkAmount = sumquantity1;
                            detailleger.ContractPrice =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractPrice"].ToString());
                            detailleger.ContractTotalPrice = summoney1;

                            detailleger.ResponsibleWorkAmount = sumquantity2;
                            detailleger.ResponsiblePrice =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsiblePrice"].ToString());
                            detailleger.ResponsibleTotalPrice = summoney2;

                            detailleger.PlanWorkAmount = sumquantity3;
                            detailleger.PlanPrice = Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanPrice"].ToString());
                            detailleger.PlanTotalPrice = summoney3;

                            detailleger.Temp5 = summoney1 - summoney3;
                            if (summoney1 != 0)
                            {
                                detailleger.Temp4 = detailleger.Temp5/summoney1;
                            }
                            else
                            {
                                detailleger.Temp4 = 0;
                            }
                            detailleger.Temp3 = dataSet.Tables[0].Rows[i]["ContractGroupType"].ToString();
                            detailleger.ProjectTaskDtlID = dataSet.Tables[0].Rows[i]["ProjectTaskDtlID"].ToString();
                            detailleger.ProjectTaskDtlName = dataSet.Tables[0].Rows[i]["ProjectTaskDtlName"].ToString();
                            detailleger.Temp2 = dataSet.Tables[0].Rows[i]["CostItemName"].ToString();
                            detailleger.WorkAmountUnitName = dataSet.Tables[0].Rows[i]["WorkAmountUnitName"].ToString();
                            list.Insert(insert, detailleger);
                        }
                    }
                    else //类型不同，list添加一条信息，设置新的信息
                    {
                        list.Add(detailleger);
                        index += 1;
                        if (i < dataSet.Tables[0].Rows.Count - 1)
                        {
                            //类型不同
                            detailleger = new GWBSDetailLedger();
                            detailleger.ContractWorkAmount = sumquantity1;
                            detailleger.ContractPrice =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractPrice"].ToString());
                            detailleger.ContractTotalPrice = summoney1;

                            detailleger.ResponsibleWorkAmount = sumquantity2;
                            detailleger.ResponsiblePrice =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsiblePrice"].ToString());
                            detailleger.ResponsibleTotalPrice = summoney2;

                            detailleger.PlanWorkAmount = sumquantity3;
                            detailleger.PlanPrice = Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanPrice"].ToString());
                            detailleger.PlanTotalPrice = summoney3;

                            detailleger.Temp5 = summoney1 - summoney3;
                            if (summoney1 != 0)
                            {
                                detailleger.Temp4 = detailleger.Temp5/summoney1;
                            }
                            else
                            {
                                detailleger.Temp4 = 0;
                            }
                            detailleger.Temp3 = strGroupType;
                            detailleger.ProjectTaskDtlID = dataSet.Tables[0].Rows[i]["ProjectTaskDtlID"].ToString();
                            detailleger.ProjectTaskDtlName = dataSet.Tables[0].Rows[i]["ProjectTaskDtlName"].ToString();
                            detailleger.Temp2 = strCostItemName;
                            detailleger.WorkAmountUnitName = strUnit;
                            list.Insert(insert, detailleger);
                            index += 1;
                            insert = index;
                            sumquantity1 = 0;
                            summoney1 = 0;
                            sumquantity2 = 0;
                            summoney2 = 0;
                            sumquantity3 = 0;
                            summoney3 = 0;
                            strGroupType = dataSet.Tables[0].Rows[i]["ContractGroupType"].ToString();
                            strCostItemName = dataSet.Tables[0].Rows[i]["CostItemName"].ToString();
                            strUnit = dataSet.Tables[0].Rows[i]["WorkAmountUnitName"].ToString();
                        }
                        else
                        {
                            //类型不同也是最后一个
                            insert = index - 1;
                            detailleger = new GWBSDetailLedger();
                            detailleger.ContractWorkAmount =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractWorkAmount"].ToString());
                            detailleger.ContractPrice =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractPrice"].ToString());
                            detailleger.ContractTotalPrice =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractTotalPrice"].ToString());

                            detailleger.ResponsibleWorkAmount =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsibleWorkAmount"].ToString());
                            detailleger.ResponsiblePrice =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsiblePrice"].ToString());
                            detailleger.ResponsibleTotalPrice =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ResponsibleTotalPrice"].ToString());

                            detailleger.PlanWorkAmount =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanWorkAmount"].ToString());
                            detailleger.PlanPrice = Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanPrice"].ToString());
                            detailleger.PlanTotalPrice =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanTotalPrice"].ToString());

                            detailleger.Temp5 =
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["ContractTotalPrice"].ToString()) -
                                Convert.ToDecimal(dataSet.Tables[0].Rows[i]["PlanTotalPrice"].ToString());
                            if (summoney1 != 0)
                            {
                                detailleger.Temp4 = detailleger.Temp5/
                                                    Convert.ToDecimal(
                                                        dataSet.Tables[0].Rows[i]["ContractTotalPrice"].ToString());
                            }
                            else
                            {
                                detailleger.Temp4 = 0;
                            }
                            detailleger.Temp3 = strGroupType;
                            detailleger.ProjectTaskDtlID = dataSet.Tables[0].Rows[i]["ProjectTaskDtlID"].ToString();
                            detailleger.ProjectTaskDtlName = dataSet.Tables[0].Rows[i]["ProjectTaskDtlName"].ToString();
                            detailleger.Temp2 = strCostItemName;
                            detailleger.WorkAmountUnitName = strUnit;
                            list.Insert(index, detailleger);
                        }
                    }
                }
                else
                {
                    list.Add(detailleger);
                    strGroupType = dataSet.Tables[0].Rows[i]["ContractGroupType"].ToString();
                    strCostItemName = dataSet.Tables[0].Rows[i]["CostItemName"].ToString();
                    strUnit = dataSet.Tables[0].Rows[i]["WorkAmountUnitName"].ToString();
                    insert = 0;
                    index = 0;
                }
            }
            return list;
        }

        /// <summary>
        /// 查询工程任务明细台账统计后的信息
        /// </summary>
        [TransManager]
        public IList SearchGWBSDetail(CurrentProjectInfo projectInfo)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql =
                @"select t3.ContractGroupType,t4.costitemname from (select t2.ContractGroupType,t2.contractnumber,t1.ProjectTaskDtlID,t1.ProjectTaskDtlName from thd_gwbsdetailledger t1 inner join thd_contractgroup t2 on t1.contractgroupid = t2.id) t3 left join thd_gwbsdetail t4 on t3.ProjectTaskDtlID = t4.id ";
            sql += " where TheProjectGUID = '" + projectInfo.Id + "' group by t3.ContractGroupType,t4.costitemname";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            GWBSDetailLedger detailleger = new GWBSDetailLedger();
            IList list = new ArrayList();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++) //循环读取临时表的行
            {
                detailleger.Temp1 = dataSet.Tables[0].Rows[i][0].ToString();
                detailleger.Temp2 = dataSet.Tables[0].Rows[i][1].ToString();

                list.Add(detailleger);
            }
            return list;
        }

        #endregion

        #region 公司报表方法

        /// <summary>
        /// 得到所有项目信息
        /// </summary>
        /// <returns></returns>
        public IList GetAllProjectList()
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = "select t1.id,t1.projectname,t2.opgoperationtype,t1.resproportion, " +
                                  " (select k1.opgname from resoperationorg k1 where k1.opgid=t2.parentnodeid) parentOrgname " +
                                  " From resconfig t1,resoperationorg t2 where t1.ownerorg=t2.opgid order by parentOrgname,t1.projectname ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CurrentProjectInfo project = new CurrentProjectInfo();
                    project.Id = TransUtil.ToString(dataRow["id"]);
                    project.Name = TransUtil.ToString(dataRow["projectname"]);
                    project.OwnerOrgName = TransUtil.ToString(dataRow["parentOrgname"]);
                    project.Data1 = TransUtil.ToString(dataRow["opgoperationtype"]);
                    project.ResProportion = TransUtil.ToDecimal(dataRow["resproportion"]);
                    list.Add(project);
                }
            }
            return list;
        }

        public Hashtable GetCompanyReportData(int kjn, int kjy)
        {
            Hashtable ht = new Hashtable();

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText =
                " select t1.id,t1.resproportion,t1.thegroundarea,t1.undergroundarea,t1.groundlayers, " +
                " t1.undergroundlayers,t1.buildingheight,t1.baseform,t1.projectlocationprovince, " +
                " t1.contractcollectratio,t1.projectlocationcity,t1.begindate,t1.enddate " +
                " From resconfig t1 ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            Hashtable ht_project = new Hashtable();

            #region 工程项目信息

            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CurrentProjectInfo projectInfo = new CurrentProjectInfo();
                    projectInfo.Id = TransUtil.ToString(dataRow["id"]);
                    projectInfo.TheGroundArea = TransUtil.ToDecimal(dataRow["thegroundarea"]);
                    projectInfo.UnderGroundArea = TransUtil.ToDecimal(dataRow["undergroundarea"]);
                    projectInfo.GroundLayers = TransUtil.ToDecimal(dataRow["groundlayers"]);
                    projectInfo.UnderGroundLayers = TransUtil.ToDecimal(dataRow["undergroundlayers"]);
                    projectInfo.BuildingHeight = TransUtil.ToDecimal(dataRow["buildingheight"]);
                    projectInfo.ResProportion = TransUtil.ToDecimal(dataRow["resproportion"]);
                    projectInfo.ContractCollectRatio = TransUtil.ToDecimal(dataRow["contractcollectratio"]);
                    projectInfo.ProjectLocationProvince = TransUtil.ToString(dataRow["projectlocationprovince"]);
                    projectInfo.ProjectLocationCity = TransUtil.ToString(dataRow["projectlocationcity"]);
                    projectInfo.BaseForm = TransUtil.ToString(dataRow["baseform"]);
                    projectInfo.BeginDate = TransUtil.ToDateTime(dataRow["begindate"]);
                    projectInfo.EndDate = TransUtil.ToDateTime(dataRow["enddate"]);
                    ht_project.Add(projectInfo.Id, projectInfo);
                }
            }

            #endregion

            #region 工程任务信息

            Hashtable ht_ground = new Hashtable();
            command.CommandText = " select t1.theprojectguid,t1.overorundergroundflag,t1.tlevel,t1.id " +
                                  " from thd_gwbstree t1 where t1.overorundergroundflag in (1,2) ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain g_domain = new DataDomain();
                    g_domain.Name1 = TransUtil.ToString(dataRow["theprojectguid"]);
                    g_domain.Name2 = TransUtil.ToString(dataRow["id"]);
                    g_domain.Name3 = TransUtil.ToString(dataRow["tlevel"]);
                    g_domain.Name4 = TransUtil.ToString(dataRow["overorundergroundflag"]);
                    if (!ht_ground.Contains(g_domain.Name1 + "_" + g_domain.Name4))
                    {
                        ht_ground.Add(g_domain.Name1 + "_" + g_domain.Name4, g_domain);
                    }
                    else
                    {
                        DataDomain t_domain = (DataDomain) ht_ground[g_domain.Name1 + "_" + g_domain.Name4];
                        if (ClientUtil.ToInt(g_domain.Name3) < ClientUtil.ToInt(t_domain.Name3))
                        {
                            ht_ground.Remove(g_domain.Name1 + "_" + g_domain.Name4);
                            ht_ground.Add(g_domain.Name1 + "_" + g_domain.Name4, g_domain);
                        }
                    }
                }
            }

            #endregion

            #region 业主报量状态信息

            command.CommandText =
                " select t1.projectid,max(t1.confirmsummoney) confirmsummoney,max(t1.collectionsummoney) collectionsummoney " +
                " from thd_ownerquantitymaster t1 group by t1.projectid ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            Hashtable ht_ownerQty = new Hashtable();
            //工程项目信息
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    OwnerQuantityMaster owner = new OwnerQuantityMaster();
                    owner.ProjectId = TransUtil.ToString(dataRow["projectid"]);
                    owner.ConfirmSumMoney = TransUtil.ToDecimal(dataRow["confirmsummoney"]);
                    owner.CollectionSumMoney = TransUtil.ToDecimal(dataRow["collectionsummoney"]);
                    ht_ownerQty.Add(owner.ProjectId, owner);
                }
            }

            #endregion

            #region 签证变更信息

            command.CommandText =
                " select t1.theprojectguid,sum(t1.contracttotalprice) contracttotalprice,sum(t1.plantotalprice) plantotalprice " +
                " from thd_gwbsdetailledger t1 group by t1.theprojectguid ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            Hashtable ht_gwbsLedger = new Hashtable();
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GWBSDetailLedger ledger = new GWBSDetailLedger();
                    ledger.TheProjectGUID = TransUtil.ToString(dataRow["theprojectguid"]);
                    ledger.ContractTotalPrice = TransUtil.ToDecimal(dataRow["contracttotalprice"]);
                    ledger.PlanTotalPrice = TransUtil.ToDecimal(dataRow["plantotalprice"]);
                    ht_gwbsLedger.Add(ledger.TheProjectGUID, ledger);
                }
            }

            #endregion

            #region 月度成本核算信息

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Kjn", kjn));
            oq.AddCriterion(Expression.Eq("Kjy", kjy));
            IList list_costBill = Dao.ObjectQuery(typeof (CostMonthAccountBill), oq); //有待优化 2012-11-27
            Hashtable ht_sumData = new Hashtable();
            Hashtable ht_subject = new Hashtable(); //占比
            Hashtable ht_indicator = new Hashtable(); //技经指标
            foreach (CostMonthAccountBill costBill in list_costBill)
            {
                DataDomain domain_sum = new DataDomain(); //累计值
                DataDomain domain_subject = new DataDomain(); //按科目
                DataDomain domain_indicator = new DataDomain(); //技经指标
                decimal sumRealTotalPrice = 0;
                foreach (CostMonthAccountDtl dtl in costBill.Details)
                {
                    domain_sum.Name1 = TransUtil.ToDecimal(domain_sum.Name1) + dtl.SumIncomeTotalPrice;
                    domain_sum.Name2 = TransUtil.ToDecimal(domain_sum.Name2) + dtl.SumRealTotalPrice;
                    foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                    {
                        #region 占比信息

                        sumRealTotalPrice += dtlConsume.SumRealConsumeTotalPrice;
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C51101") != -1)
                        {
                            domain_subject.Name1 = TransUtil.ToDecimal(domain_subject.Name1) +
                                                   dtlConsume.SumRealConsumeTotalPrice;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C51102") != -1)
                        {
                            domain_subject.Name2 = TransUtil.ToDecimal(domain_subject.Name2) +
                                                   dtlConsume.SumRealConsumeTotalPrice;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C51103") != -1)
                        {
                            domain_subject.Name3 = TransUtil.ToDecimal(domain_subject.Name3) +
                                                   dtlConsume.SumRealConsumeTotalPrice;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C51202") != -1)
                        {
                            domain_subject.Name4 = TransUtil.ToDecimal(domain_subject.Name4) +
                                                   dtlConsume.SumRealConsumeTotalPrice;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C51203") != -1)
                        {
                            domain_subject.Name5 = TransUtil.ToDecimal(domain_subject.Name5) +
                                                   dtlConsume.SumRealConsumeTotalPrice;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C51204") != -1)
                        {
                            domain_subject.Name6 = TransUtil.ToDecimal(domain_subject.Name6) +
                                                   dtlConsume.SumRealConsumeTotalPrice;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C513") != -1)
                        {
                            domain_subject.Name7 = TransUtil.ToDecimal(domain_subject.Name7) +
                                                   dtlConsume.SumRealConsumeTotalPrice;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5110106") != -1)
                        {
                            domain_subject.Name8 = TransUtil.ToDecimal(domain_subject.Name8) +
                                                   dtlConsume.SumRealConsumeTotalPrice;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5110203") != -1)
                        {
                            if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                            {
                                domain_subject.Name9 = TransUtil.ToDecimal(domain_subject.Name9) +
                                                       decimal.Round(
                                                           (dtlConsume.SumRealConsumeTotalPrice -
                                                            dtlConsume.SumResponsiConsumeTotalPrice)/
                                                           dtlConsume.SumResponsiConsumeTotalPrice, 2);
                            }
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5110204") != -1)
                        {
                            if (dtlConsume.SumResponsiConsumeTotalPrice != 0)
                            {
                                domain_subject.Name10 = TransUtil.ToDecimal(domain_subject.Name10) +
                                                        decimal.Round(
                                                            (dtlConsume.SumRealConsumeTotalPrice -
                                                             dtlConsume.SumResponsiConsumeTotalPrice)/
                                                            dtlConsume.SumResponsiConsumeTotalPrice, 2);
                            }
                        }

                        #endregion

                        #region 技经指标

                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C511020401") != -1)
                        {
                            domain_indicator.Name1 = TransUtil.ToDecimal(domain_subject.Name1) +
                                                     dtlConsume.SumRealConsumePlanQuantity; //地下混凝土
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C511020402") != -1)
                        {
                            domain_indicator.Name3 = TransUtil.ToDecimal(domain_subject.Name3) +
                                                     dtlConsume.SumRealConsumePlanQuantity;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C511020301") != -1)
                        {
                            domain_indicator.Name7 = TransUtil.ToDecimal(domain_subject.Name7) +
                                                     dtlConsume.SumRealConsumePlanQuantity;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C511020302") != -1)
                        {
                            domain_indicator.Name10 = TransUtil.ToDecimal(domain_subject.Name10) +
                                                      dtlConsume.SumRealConsumePlanQuantity;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C512080101") != -1 ||
                            TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C512080102") != -1)
                        {
                            domain_indicator.Name16 = TransUtil.ToDecimal(domain_subject.Name16) +
                                                      dtlConsume.SumRealConsumePlanQuantity; //模板
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C511040401") != -1)
                        {
                            domain_indicator.Name17 = TransUtil.ToDecimal(domain_subject.Name17) +
                                                      dtlConsume.SumRealConsumePlanQuantity; //砌体
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C511040403") != -1)
                        {
                            domain_indicator.Name18 = TransUtil.ToDecimal(domain_subject.Name18) +
                                                      dtlConsume.SumRealConsumePlanQuantity; //外墙墙面面积
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C511040402") != -1)
                        {
                            domain_indicator.Name19 = TransUtil.ToDecimal(domain_subject.Name19) +
                                                      dtlConsume.SumRealConsumePlanQuantity; //内墙抹灰面积
                        }

                        #endregion
                    }
                    if (ht_ground.Contains(costBill.ProjectId + "_1"))
                    {
                        DataDomain g_domain = (DataDomain) ht_ground[costBill.ProjectId + "_1"];
                        string upGwbsId = TransUtil.ToString(g_domain.Name2);
                        if (dtl.AccountTaskNodeSyscode.Contains(upGwbsId + "."))
                        {
                            domain_indicator.Name20 = (TransUtil.ToDecimal(domain_indicator.Name20) +
                                                       dtl.SumIncomeTotalPrice) + ""; //地下收入
                            domain_indicator.Name23 = (TransUtil.ToDecimal(domain_indicator.Name23) +
                                                       dtl.SumRealTotalPrice) + ""; //地下成本
                        }
                    }
                    if (ht_ground.Contains(costBill.ProjectId + "_2"))
                    {
                        DataDomain g_domain = (DataDomain) ht_ground[costBill.ProjectId + "_2"];
                        string upGwbsId = TransUtil.ToString(g_domain.Name2);
                        if (dtl.AccountTaskNodeSyscode.Contains(upGwbsId + "."))
                        {
                            domain_indicator.Name21 = (TransUtil.ToDecimal(domain_indicator.Name21) +
                                                       dtl.SumIncomeTotalPrice) + ""; //地上收入
                            domain_indicator.Name24 = (TransUtil.ToDecimal(domain_indicator.Name24) +
                                                       dtl.SumRealTotalPrice) + ""; //地上成本
                        }
                    }
                }

                #region 占比信息

                if (sumRealTotalPrice != 0)
                {
                    domain_subject.Name1 = decimal.Round(
                        TransUtil.ToDecimal(domain_subject.Name1)*100/sumRealTotalPrice, 2);
                    domain_subject.Name2 = decimal.Round(
                        TransUtil.ToDecimal(domain_subject.Name2)*100/sumRealTotalPrice, 2);
                    domain_subject.Name3 = decimal.Round(
                        TransUtil.ToDecimal(domain_subject.Name3)*100/sumRealTotalPrice, 2);
                    domain_subject.Name4 = decimal.Round(
                        TransUtil.ToDecimal(domain_subject.Name4)*100/sumRealTotalPrice, 2);
                    domain_subject.Name5 = decimal.Round(
                        TransUtil.ToDecimal(domain_subject.Name5)*100/sumRealTotalPrice, 2);
                    domain_subject.Name6 = decimal.Round(
                        TransUtil.ToDecimal(domain_subject.Name6)*100/sumRealTotalPrice, 2);
                    domain_subject.Name7 = decimal.Round(
                        TransUtil.ToDecimal(domain_subject.Name7)*100/sumRealTotalPrice, 2);
                    domain_subject.Name8 = decimal.Round(
                        TransUtil.ToDecimal(domain_subject.Name8)*100/sumRealTotalPrice, 2);
                    if (!ht_subject.Contains(costBill.ProjectId))
                    {
                        ht_subject.Add(costBill.ProjectId, domain_subject);
                    }
                }

                #endregion

                #region 技经指标

                if (ht_project.Contains(costBill.ProjectId))
                {
                    CurrentProjectInfo t_project = (CurrentProjectInfo) ht_project[costBill.ProjectId];
                    if (t_project.UnderGroundArea != 0)
                    {
                        domain_indicator.Name2 =
                            decimal.Round(TransUtil.ToDecimal(domain_indicator.Name1)/t_project.UnderGroundArea, 2);
                    }
                    if (t_project.TheGroundArea != 0)
                    {
                        domain_indicator.Name4 =
                            decimal.Round(TransUtil.ToDecimal(domain_indicator.Name3)/t_project.TheGroundArea, 2);
                    }
                    domain_indicator.Name5 = (TransUtil.ToDecimal(domain_indicator.Name1) +
                                              TransUtil.ToDecimal(domain_indicator.Name3)) + "";
                    if (t_project.UnderGroundArea != 0 || t_project.TheGroundArea != 0)
                    {
                        domain_indicator.Name6 =
                            decimal.Round(
                                TransUtil.ToDecimal(domain_indicator.Name5)/
                                (t_project.UnderGroundArea + t_project.TheGroundArea), 2);
                    }
                    if (t_project.UnderGroundArea != 0)
                    {
                        domain_indicator.Name8 =
                            decimal.Round(TransUtil.ToDecimal(domain_indicator.Name7)*1000/t_project.UnderGroundArea, 2);
                    }
                    if (TransUtil.ToDecimal(domain_indicator.Name1) != 0)
                    {
                        domain_indicator.Name9 =
                            decimal.Round(
                                TransUtil.ToDecimal(domain_indicator.Name7)*1000/
                                TransUtil.ToDecimal(domain_indicator.Name1), 2);
                    }
                    if (t_project.TheGroundArea != 0)
                    {
                        domain_indicator.Name11 =
                            decimal.Round(TransUtil.ToDecimal(domain_indicator.Name10)*1000/t_project.TheGroundArea, 2);
                        //地上钢筋
                    }
                    if (TransUtil.ToDecimal(domain_indicator.Name3) != 0)
                    {
                        domain_indicator.Name12 =
                            decimal.Round(
                                TransUtil.ToDecimal(domain_indicator.Name10)*1000/
                                TransUtil.ToDecimal(domain_indicator.Name3), 2);
                    }
                    domain_indicator.Name13 = (TransUtil.ToDecimal(domain_indicator.Name7) +
                                               TransUtil.ToDecimal(domain_indicator.Name10)) + "";
                    if (t_project.UnderGroundArea != 0 || t_project.TheGroundArea != 0)
                    {
                        domain_indicator.Name14 =
                            decimal.Round(
                                TransUtil.ToDecimal(domain_indicator.Name13)*1000/
                                (t_project.UnderGroundArea + t_project.TheGroundArea), 2);
                    }
                    if (TransUtil.ToDecimal(domain_indicator.Name5) != 0)
                    {
                        domain_indicator.Name15 =
                            decimal.Round(
                                TransUtil.ToDecimal(domain_indicator.Name13)*1000/
                                TransUtil.ToDecimal(domain_indicator.Name5), 2); //钢筋
                    }
                    decimal sumIncomeMoney = TransUtil.ToDecimal(domain_indicator.Name20) +
                                             TransUtil.ToDecimal(domain_indicator.Name21);
                    decimal sumRealMoney = TransUtil.ToDecimal(domain_indicator.Name23) +
                                           TransUtil.ToDecimal(domain_indicator.Name24);
                    if (t_project.UnderGroundArea != 0)
                    {
                        domain_indicator.Name20 =
                            decimal.Round(TransUtil.ToDecimal(domain_indicator.Name20)/t_project.UnderGroundArea, 2);
                        domain_indicator.Name23 =
                            decimal.Round(TransUtil.ToDecimal(domain_indicator.Name23)/t_project.UnderGroundArea, 2);
                    }
                    if (t_project.TheGroundArea != 0)
                    {
                        domain_indicator.Name21 =
                            decimal.Round(TransUtil.ToDecimal(domain_indicator.Name21)/t_project.TheGroundArea, 2);
                        domain_indicator.Name24 =
                            decimal.Round(TransUtil.ToDecimal(domain_indicator.Name24)/t_project.TheGroundArea, 2);
                    }
                    if (t_project.UnderGroundArea != 0 || t_project.TheGroundArea != 0)
                    {
                        domain_indicator.Name22 =
                            decimal.Round(sumIncomeMoney/(t_project.UnderGroundArea + t_project.TheGroundArea), 2);
                        domain_indicator.Name25 =
                            decimal.Round(sumRealMoney/(t_project.UnderGroundArea + t_project.TheGroundArea), 2);
                    }
                    ht_indicator.Add(costBill.ProjectId, domain_indicator);
                }

                #endregion

                if (!ht_sumData.Contains(costBill.ProjectId))
                {
                    ht_sumData.Add(costBill.ProjectId, domain_sum);
                }

            }

            #endregion

            ht.Add("1", ht_project);
            ht.Add("2", ht_ownerQty);
            ht.Add("3", ht_gwbsLedger);
            ht.Add("4", ht_sumData);
            ht.Add("5", ht_subject);
            ht.Add("6", ht_indicator);

            return ht;
        }

        #endregion

        #region 商务报表

        public Hashtable GetBusinessReportData(int kjn, int kjy, string projectId, string acctOrgGUID, IList taskList)
        {
            Hashtable ht = new Hashtable();
            DataDomain cost_domain = new DataDomain(); //项目成本情况

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            #region 2:取会计期的开始和结束日期

            string startDate = "";
            string endDate = "";
            string sql = @"select t1.begindate,(t1.enddate+1) enddate from resfiscalperioddet t1 where t1.fiscalyear=" +
                         kjn + " and t1.fiscalmonth= " + kjy;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    endDate = TransUtil.ToDateTime(dataRow["enddate"]).ToShortDateString();
                    startDate = TransUtil.ToDateTime(dataRow["begindate"]).ToShortDateString();
                }
            }

            #endregion

            command.CommandText =
                " select t1.id,t1.buildingarea,t1.civilcontractmoney,t1.resproportion,t1.wallprojectarea,t1.bigmodualgroundupprice," +
                " t1.bigmodualgrounddownprice,t1.responscost,t1.contractincome,t1.risklimits,t1.inproportion " +
                " From resconfig t1 where t1.id = '" + projectId + "'";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            CurrentProjectInfo projectInfo = new CurrentProjectInfo();

            string sqlTaskDtlStr = ""; //工程任务明细SQL
            string sqlLedgerStr = ""; //签证变更SQL
            if (taskList.Count > 0)
            {
                foreach (string accountTaskGUID in taskList)
                {
                    if (sqlTaskDtlStr == "")
                    {
                        sqlTaskDtlStr = " and ( t1.thegwbssyscode like '%" + accountTaskGUID + "%' ";
                    }
                    else
                    {
                        sqlTaskDtlStr += " or t1.thegwbssyscode like '%" + accountTaskGUID + "%' ";
                    }
                }
                sqlTaskDtlStr += " ) ";

                foreach (string accountTaskGUID in taskList)
                {
                    if (sqlLedgerStr == "")
                    {
                        sqlLedgerStr = " and ( t1.theprojecttasksyscode like '%" + accountTaskGUID + "%' ";
                    }
                    else
                    {
                        sqlLedgerStr += " or t1.theprojecttasksyscode like '%" + accountTaskGUID + "%' ";
                    }
                }
                sqlLedgerStr += " ) ";
            }



            #region 工程项目信息

            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    projectInfo = new CurrentProjectInfo();
                    projectInfo.Id = TransUtil.ToString(dataRow["id"]);
                    projectInfo.BuildingArea = TransUtil.ToDecimal(dataRow["buildingarea"]);
                    projectInfo.CivilContractMoney = TransUtil.ToDecimal(dataRow["civilcontractmoney"]);
                    projectInfo.WallProjectArea = TransUtil.ToDecimal(dataRow["wallprojectarea"]);
                    projectInfo.ResProportion = TransUtil.ToDecimal(dataRow["resproportion"]);
                    projectInfo.BigModualGroundUpPrice = TransUtil.ToDecimal(dataRow["bigmodualgrounddownprice"]);
                    projectInfo.BigModualGroundDownPrice = TransUtil.ToDecimal(dataRow["bigmodualgroundupprice"]);
                    projectInfo.ResponsCost = TransUtil.ToDecimal(dataRow["responscost"]);
                    projectInfo.ContractIncome = TransUtil.ToDecimal(dataRow["contractincome"]);
                    projectInfo.RiskLimits = TransUtil.ToDecimal(dataRow["risklimits"]);
                    projectInfo.Inproportion = TransUtil.ToDecimal(dataRow["inproportion"]);
                }
            }

            #endregion

            #region 工程任务明细

            decimal sumPlanTotalPrice = 0; //工程任务明细计划合价之和
            command.CommandText =
                " select sum(t1.plantotalprice) plantotalprice From thd_gwbsdetail t1 where t1.theprojectguid= '" +
                projectId + "'" + sqlTaskDtlStr + @"";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    sumPlanTotalPrice = TransUtil.ToDecimal(dataRow["plantotalprice"]);
                }
            }

            #endregion

            #region 签证变更信息 表三

            command.CommandText =
                " select t2.contractgrouptype,t2.contractname,t1.projecttaskname,t1.projecttaskdtlname,t1.contracttotalprice, " +
                " t1.plantotalprice,t1.responsibletotalprice from thd_gwbsdetailledger t1,thd_contractgroup t2 " +
                " where t1.contractgroupid=t2.id and t1.createtime >= to_date('" + startDate + "','yyyy-mm-dd')" +
                " and t1.createtime < to_date('" + endDate + "','yyyy-mm-dd')" +
                " and t1.theprojectguid='" + projectId + "'" + sqlLedgerStr + @" order by t2.contractgrouptype ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            IList list_ledger = new ArrayList();
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                string contractType = "";
                decimal sumContractTotalPrice = 0; //小计和
                decimal sumLPlanTotalPrice = 0;
                decimal sumResponsibleTotalPrice = 0;
                decimal totalContractTotalPrice = 0; //合计和
                decimal totalLPlanTotalPrice = 0;
                decimal totalResponsibleTotalPrice = 0;
                int count = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    count++;
                    GWBSDetailLedger ledger = new GWBSDetailLedger();
                    ledger.Temp = TransUtil.ToString(dataRow["contractgrouptype"]); //契约类型
                    ledger.Temp1 = TransUtil.ToString(dataRow["contractname"]); //契约名称
                    if (contractType == "")
                    {
                        contractType = ledger.Temp;
                    }
                    if (contractType != ledger.Temp)
                    {
                        GWBSDetailLedger sumLedger = new GWBSDetailLedger();
                        sumLedger.Temp = "小计: ";
                        sumLedger.Temp1 = "";
                        sumLedger.ContractTotalPrice = sumContractTotalPrice;
                        sumLedger.PlanTotalPrice = sumLPlanTotalPrice;
                        sumLedger.ResponsibleTotalPrice = sumResponsibleTotalPrice;
                        sumContractTotalPrice = 0;
                        sumLPlanTotalPrice = 0;
                        sumResponsibleTotalPrice = 0;
                        list_ledger.Add(sumLedger);
                    }

                    ledger.ContractTotalPrice = TransUtil.ToDecimal(dataRow["contracttotalprice"]);
                    ledger.PlanTotalPrice = TransUtil.ToDecimal(dataRow["plantotalprice"]);
                    ledger.ResponsibleTotalPrice = TransUtil.ToDecimal(dataRow["responsibletotalprice"]);

                    sumContractTotalPrice += ledger.ContractTotalPrice;
                    sumLPlanTotalPrice += ledger.PlanTotalPrice;
                    sumResponsibleTotalPrice += ledger.ResponsibleTotalPrice;
                    totalContractTotalPrice += ledger.ContractTotalPrice;
                    totalLPlanTotalPrice += ledger.PlanTotalPrice;
                    totalResponsibleTotalPrice += ledger.ResponsibleTotalPrice;

                    ledger.ProjectTaskName = TransUtil.ToString(dataRow["projecttaskname"]);
                    ledger.ProjectTaskDtlName = TransUtil.ToString(dataRow["projecttaskdtlname"]);

                    contractType = TransUtil.ToString(dataRow["contractgrouptype"]);
                    list_ledger.Add(ledger);

                    if (count == dataTable.Rows.Count)
                    {
                        GWBSDetailLedger sumLedger = new GWBSDetailLedger();
                        sumLedger.Temp = "小计: ";
                        sumLedger.Temp1 = "";
                        sumLedger.ContractTotalPrice = sumContractTotalPrice;
                        sumLedger.PlanTotalPrice = sumLPlanTotalPrice;
                        sumLedger.ResponsibleTotalPrice = sumResponsibleTotalPrice;
                        list_ledger.Add(sumLedger);

                        GWBSDetailLedger totalLedger = new GWBSDetailLedger();
                        totalLedger.Temp = "合计: ";
                        totalLedger.Temp1 = "";
                        totalLedger.ContractTotalPrice = totalContractTotalPrice;
                        totalLedger.PlanTotalPrice = totalLPlanTotalPrice;
                        totalLedger.ResponsibleTotalPrice = totalResponsibleTotalPrice;
                        list_ledger.Insert(0, totalLedger);
                    }

                    //构造表一数据
                    if (TransUtil.ToString(ledger.Temp).IndexOf("工程合同") != -1)
                    {
                        //cost_domain.Name1 = (TransUtil.ToDecimal(cost_domain.Name1) + ledger.ContractTotalPrice) + "";//合同收入
                        //cost_domain.Name2 = (TransUtil.ToDecimal(cost_domain.Name2) + ledger.ResponsibleTotalPrice) + "";//责任成本
                    }
                    else
                    {
                        cost_domain.Name5 = (TransUtil.ToDecimal(cost_domain.Name5) + ledger.ContractTotalPrice) + "";
                        //合同收入调整
                        cost_domain.Name7 = (TransUtil.ToDecimal(cost_domain.Name7) + ledger.ResponsibleTotalPrice) + "";
                        //责任成本调整
                    }
                }
            }

            #endregion

            #region 分包结算信息 表四

            command.CommandText = " select t1.id from thd_subcontractproject t1 where t1.projectid='" + projectId +
                                  "' and t1.balancestyle = '末次结算' ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            IList list_lastBal = new ArrayList();
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string subId = TransUtil.ToString(dataRow["id"]);
                    list_lastBal.Add(subId);
                }
            }

            string subSql = "";
            if (TransUtil.ToString(acctOrgGUID) != "")
            {
                subSql = " and t1.ownerorgsyscode like '%" + acctOrgGUID + "%'";
            }
            command.CommandText =
                " select t1.id,t1.bearerorgname,max(t1.addupbalancemoney) balmoney,max(t1.contractinterimmoney) contractmny,min(t1.subpackage) subpackage " +
                " from thd_subcontractproject t1 where t1.projectid='" + projectId + "' " + subSql +
                " group by t1.bearerorgname,t1.id ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            IList list_subBal = new ArrayList();
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SubContractProject subProject = new SubContractProject();
                    subProject.SubPackage = TransUtil.ToString(dataRow["subpackage"]);
                    subProject.BearerOrgName = TransUtil.ToString(dataRow["bearerorgname"]);
                    subProject.AddupBalanceMoney = TransUtil.ToDecimal(dataRow["balmoney"]);
                    subProject.ContractInterimMoney = TransUtil.ToDecimal(dataRow["contractmny"]);
                    subProject.Id = TransUtil.ToString(dataRow["id"]);
                    if (list_lastBal.Contains(subProject.Id))
                    {
                        subProject.BalanceStyle = "是";
                    }
                    list_subBal.Add(subProject);
                }
            }

            #endregion

            #region 月度成本核算信息(项目各项消耗指标 表二)

            decimal sumEarnValue = 0; //累计挣值得
            decimal sumRealTotalPrice = 0; //累计实际成本
            decimal sumIncomeTotalPrice = 0; //累计合同收入
            decimal sumRespTotalPrice = 0; //累计责任合价
            decimal sumSubManageMoney = 0; //整体分包管理费
            decimal sumServiceMoney = 0; //总承包服务费
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectId));
            oq.AddCriterion(Expression.Eq("Kjn", kjn));
            oq.AddCriterion(Expression.Eq("Kjy", kjy));
            IList list_cost = this.GetCostMonthAccountBill(oq);
            //CostMonthAccountBill costBill = new CostMonthAccountBill();
            //if (list_cost.Count == 1)
            //{
            //    costBill = (CostMonthAccountBill)list_cost[0];
            //}
            DataDomain consume_domain = new DataDomain();
            decimal sumModualMoney = 0;
            foreach (CostMonthAccountBill costBill in list_cost)
            {
                foreach (CostMonthAccountDtl dtl in costBill.Details)
                {
                    bool ifContain = false;
                    foreach (string accountTaskGUID in taskList)
                    {
                        if (dtl.AccountTaskNodeSyscode.Contains(accountTaskGUID))
                        {
                            ifContain = true;
                        }
                    }
                    if (ifContain == false)
                        break;
                    sumEarnValue += dtl.SumEarnValue;
                    consume_domain.Name1 = (TransUtil.ToDecimal(consume_domain.Name1) + dtl.SumIncomeTotalPrice) + "";
                    //自营产值(万元）
                    foreach (CostMonthAccDtlConsume dtlConsume in dtl.Details)
                    {
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C513") != -1)
                        {
                            consume_domain.Name2 = (TransUtil.ToDecimal(consume_domain.Name2) +
                                                    dtlConsume.SumRealConsumeTotalPrice) + ""; //现场管理费(万元）
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C51204") != -1)
                        {
                            consume_domain.Name4 = (TransUtil.ToDecimal(consume_domain.Name4) +
                                                    dtlConsume.SumRealConsumeTotalPrice) + ""; //已发生(万元）
                            consume_domain.Name5 = ""; //预计尚需发生(万元）
                        }
                        //商品砼
                        if (TransUtil.ToString(dtlConsume.ResourceTypeCode).IndexOf("I11201") != -1)
                        {
                            consume_domain.Name8 = (TransUtil.ToDecimal(consume_domain.Name8) +
                                                    dtlConsume.SumResponsiConsumeTotalPrice) + ""; //图纸计算量（m3）
                            consume_domain.Name9 = (TransUtil.ToDecimal(consume_domain.Name9) +
                                                    dtlConsume.SumRealConsumeTotalPrice) + ""; //实耗量（m3）
                        }
                        //钢筋
                        if (TransUtil.ToString(dtlConsume.ResourceTypeCode).IndexOf("I11001") != -1)
                        {
                            consume_domain.Name11 = (TransUtil.ToDecimal(consume_domain.Name11) +
                                                     dtlConsume.SumResponsiConsumeTotalPrice) + ""; //翻样量(t)
                            consume_domain.Name12 = (TransUtil.ToDecimal(consume_domain.Name12) +
                                                     dtlConsume.SumRealConsumeTotalPrice) + ""; //实耗量(t)
                        }
                        //料具
                        if (TransUtil.ToString(dtlConsume.ResourceTypeCode).IndexOf("I142") != -1 &&
                            TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5120302") != -1)
                        {
                            consume_domain.Name18 = (TransUtil.ToDecimal(consume_domain.Name18) +
                                                     dtlConsume.SumRealConsumeTotalPrice) + ""; //租赁和损耗的合计产生费用
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C512080101") != -1 ||
                            TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C512080102") != -1)
                        {
                            consume_domain.Name21 = (TransUtil.ToDecimal(consume_domain.Name21) +
                                                     dtlConsume.SumRealConsumeQuantity) + ""; //结构已施工面积(m2)
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5120803") != -1)
                        {
                            consume_domain.Name22 = (TransUtil.ToDecimal(consume_domain.Name22) +
                                                     dtlConsume.SumRealConsumeTotalPrice) + ""; //支撑架租赁费用(万元）
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C512080101") != -1 ||
                            TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C512080102") != -1)
                        {
                            consume_domain.Name26 = (TransUtil.ToDecimal(consume_domain.Name21) +
                                                     dtlConsume.SumRealConsumeQuantity) + ""; //模板总接触面积（m2）
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C512080204") != -1 ||
                            TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C512080205") != -1)
                        {
                            consume_domain.Name29 = (TransUtil.ToDecimal(consume_domain.Name29) +
                                                     dtlConsume.SumRealConsumeTotalPrice) + ""; //已发生模板木枋费用(元）
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5120802") != -1)
                        {
                            sumModualMoney = sumModualMoney + dtlConsume.SumRealConsumeTotalPrice; //总模板木枋实际发生费用
                        }

                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5120405") != -1)
                        {
                            consume_domain.Name32 = (TransUtil.ToDecimal(consume_domain.Name22) +
                                                     dtlConsume.SumRealConsumeTotalPrice) + ""; //生活水电（万元）
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5110201") != -1 ||
                            TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5110306") != -1)
                        {
                            consume_domain.Name33 = (TransUtil.ToDecimal(consume_domain.Name21) +
                                                     dtlConsume.SumRealConsumeQuantity) + ""; //施工水电（万元）
                        }

                        //表一数据
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5160301") != -1)
                        {
                            sumSubManageMoney += dtlConsume.SumRealConsumeTotalPrice;
                        }
                        if (TransUtil.ToString(dtlConsume.CostSubjectCode).IndexOf("C5160302") != -1)
                        {
                            sumServiceMoney += dtlConsume.SumRealConsumeTotalPrice;
                        }
                    }
                }
            }
            if (TransUtil.ToDecimal(consume_domain.Name1) != 0)
            {
                consume_domain.Name3 =
                    decimal.Round(
                        TransUtil.ToDecimal(consume_domain.Name2)*100/TransUtil.ToDecimal(consume_domain.Name1), 2);
                //占累计自营产值比例（%）
            }
            consume_domain.Name6 = ""; //合计(万元）
            consume_domain.Name7 = ""; //占自营预计总产值比例（%）
            if (TransUtil.ToDecimal(consume_domain.Name8) != 0)
            {
                consume_domain.Name10 =
                    decimal.Round(
                        (TransUtil.ToDecimal(consume_domain.Name8) - TransUtil.ToDecimal(consume_domain.Name9))*100/
                        TransUtil.ToDecimal(consume_domain.Name8), 2); //损耗率（%）
            }
            if (TransUtil.ToDecimal(consume_domain.Name11) != 0)
            {
                consume_domain.Name13 =
                    decimal.Round(
                        (TransUtil.ToDecimal(consume_domain.Name12) - TransUtil.ToDecimal(consume_domain.Name11))*100/
                        TransUtil.ToDecimal(consume_domain.Name11), 2); //节超率（%）
            }
            consume_domain.Name14 = ""; //废材处理(t)
            consume_domain.Name15 = ""; //废材率（%）
            consume_domain.Name16 = ""; //手工填入
            consume_domain.Name17 = ""; //手工填入
            if (projectInfo.WallProjectArea != 0)
            {
                consume_domain.Name19 =
                    decimal.Round(TransUtil.ToDecimal(consume_domain.Name18)/projectInfo.WallProjectArea, 2);
                //按外墙投影面积单方造价（元/m2）
            }
            if (projectInfo.BuildingArea != 0)
            {
                consume_domain.Name20 =
                    decimal.Round(TransUtil.ToDecimal(consume_domain.Name18)/projectInfo.BuildingArea, 2);
                //按建筑面积单方造价（元/m2)
            }
            consume_domain.Name23 = ""; //手工填入(建筑面积单方造价（元/m2）)
            if (TransUtil.ToDecimal(consume_domain.Name21) != 0)
            {
                consume_domain.Name24 =
                    decimal.Round(
                        TransUtil.ToDecimal(consume_domain.Name22)/TransUtil.ToDecimal(consume_domain.Name21), 2);
                //接触面积单方造价（元/m2）
            }
            consume_domain.Name25 = (projectInfo.BigModualGroundUpPrice + projectInfo.BigModualGroundDownPrice) + "";
            //接触面积单方造价（元/m2）
            if (TransUtil.ToDecimal(consume_domain.Name26) != 0)
            {
                consume_domain.Name27 = decimal.Round(sumModualMoney/TransUtil.ToDecimal(consume_domain.Name26), 2) + "";
                //单方造价（元/m2）
            }
            consume_domain.Name28 = consume_domain.Name26;
            consume_domain.Name30 = "100"; //摊销比例（%）
            if (TransUtil.ToDecimal(consume_domain.Name28) != 0)
            {
                consume_domain.Name31 =
                    decimal.Round(
                        TransUtil.ToDecimal(consume_domain.Name29)*TransUtil.ToDecimal(consume_domain.Name30)/
                        TransUtil.ToDecimal(consume_domain.Name28), 2) + ""; //模板木枋单方造价（元/m2）
            }

            #endregion

            #region 成本状况统计(表一)

            cost_domain.Name1 = projectInfo.ContractIncome + "";
            cost_domain.Name2 = projectInfo.ResponsCost + "";
            cost_domain.Name3 = projectInfo.RiskLimits + "";
            cost_domain.Name4 = projectInfo.Inproportion + "";
            //cost_domain.Name3 = (TransUtil.ToDecimal(cost_domain.Name1) * projectInfo.ResProportion - (TransUtil.ToDecimal(cost_domain.Name1) - TransUtil.ToDecimal(cost_domain.Name2))) + "";//风险额度
            //cost_domain.Name4 = projectInfo.ResProportion + "";//责任上缴比例（%）
            cost_domain.Name6 = (TransUtil.ToDecimal(cost_domain.Name1) + TransUtil.ToDecimal(cost_domain.Name5)) + "";
            //预计合同总收入
            cost_domain.Name8 = (TransUtil.ToDecimal(cost_domain.Name2) + TransUtil.ToDecimal(cost_domain.Name7)) + "";
            //责任成本考核目标
            cost_domain.Name9 = (sumPlanTotalPrice - sumEarnValue) + ""; //预计尚需发生的成本
            cost_domain.Name10 = (TransUtil.ToDecimal(cost_domain.Name9) + TransUtil.ToDecimal(cost_domain.Name15)) + "";
            //预计总成本
            if (TransUtil.ToDecimal(cost_domain.Name6) != 0)
            {
                cost_domain.Name11 =
                    decimal.Round(
                        (TransUtil.ToDecimal(cost_domain.Name6) - TransUtil.ToDecimal(cost_domain.Name10))*100/
                        TransUtil.ToDecimal(cost_domain.Name6), 2) + ""; //预计工程利润率（%）
            }
            if (TransUtil.ToDecimal(cost_domain.Name11) != 0)
            {
                cost_domain.Name12 = decimal.Round(sumRealTotalPrice*100/(1 - TransUtil.ToDecimal(cost_domain.Name11)),
                                                   2);
            }

            cost_domain.Name13 = sumIncomeTotalPrice + "";
            cost_domain.Name14 = sumRespTotalPrice + "";
            cost_domain.Name15 = sumRealTotalPrice + "";
            if (TransUtil.ToDecimal(cost_domain.Name13) != 0)
            {
                cost_domain.Name16 =
                    decimal.Round(
                        (TransUtil.ToDecimal(cost_domain.Name13) - TransUtil.ToDecimal(cost_domain.Name15))*100/
                        TransUtil.ToDecimal(cost_domain.Name13), 2) + ""; //利润率（%）
            }
            if (TransUtil.ToDecimal(cost_domain.Name14) != 0)
            {
                cost_domain.Name17 =
                    decimal.Round(
                        (TransUtil.ToDecimal(cost_domain.Name14) - TransUtil.ToDecimal(cost_domain.Name15))*100/
                        TransUtil.ToDecimal(cost_domain.Name14), 2) + ""; //超成本降低率（%）
            }
            cost_domain.Name18 = sumSubManageMoney + ""; //整体分包部分管理费
            cost_domain.Name19 = sumServiceMoney + ""; //配合费和总包管理费

            #endregion

            ht.Add("1", cost_domain);
            ht.Add("2", consume_domain);
            ht.Add("3", list_ledger);
            ht.Add("4", list_subBal);
            return ht;
        }

        public IList GetConfirmRight(DateTime start, DateTime end, string projectId, string sysCode)
        {
            IList list = new ArrayList();
            decimal totalConfirmMoney = 0;
            decimal busCostSure = 0;
            decimal fiscalCharges = 0;
            DataSet ds1 = null;
            DataSet ds2 = null;
            string condition1 = @"select t3.createdate,t3.submitsumquantity,t4.confirmdate,t4.confirmmoney,t3.descript
 from thd_ownerquantitymaster t3
  inner join thd_ownerquantitydetail t4 on t3.id = t4.parentid
  where t3.state = 5 ";
            string condition2 = @"select t2.buscostsure,t2.mainbusinesstax,t2.materialremain,t2.tempdeviceremain,
t2.lowvalueconsumableremain,t2.exchangematerialremain,t2.subprojectpayout,
t2.personcost,t2.materialcost,t2.mechanicalcost,t2.otherdirectcost,t2.indirectcost,t2.contractgrossprofit
  from thd_financemultdatamaster t1
 inner join thd_financemultdatadetail t2 on t1.id = t2.parentid
 where";
            if (string.IsNullOrEmpty(projectId) && string.IsNullOrEmpty(sysCode))
            {
                return null;
            }
            else if (!string.IsNullOrEmpty(projectId) && string.IsNullOrEmpty(sysCode))
            {
                condition1 += " and t3.projectid ='" + projectId + "'";
                condition2 += " t1.projectid='" + projectId + "'";
            }
            else if (string.IsNullOrEmpty(projectId) && !string.IsNullOrEmpty(sysCode))
            {
                condition1 += " and t3.opgsyscode like '%" + sysCode + "%'";
                condition2 += " t1.opgsyscode like '%" + sysCode + "%'";
            }
            condition1 +=
                string.Format(" and t3.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')",
                              start.ToShortDateString(), end.ToShortDateString());
            condition2 +=
                string.Format(" and t1.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')",
                              start.ToShortDateString(), end.ToShortDateString());
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = condition1;
            IDataReader dataReader = command.ExecuteReader();
            ds1 = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            command.CommandText = condition2;
            dataReader = command.ExecuteReader();
            ds2 = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds1 == null || ds2 == null)
            {
                return null;
            }
            DataTable dt1 = ds1.Tables[0];
            DataTable dt2 = ds2.Tables[0];
            foreach (DataRow dr2 in dt2.Rows)
            {
                busCostSure += ClientUtil.ToDecimal(dr2["buscostsure"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["mainbusinesstax"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["materialremain"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["tempdeviceremain"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["lowvalueconsumableremain"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["exchangematerialremain"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["personcost"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["materialcost"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["mechanicalcost"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["otherdirectcost"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["indirectcost"]);
                fiscalCharges += ClientUtil.ToDecimal(dr2["contractgrossprofit"]);
            }
            foreach (DataRow dr1 in dt1.Rows)
            {
                totalConfirmMoney += ClientUtil.ToDecimal(dr1["confirmmoney"]);
            }
            foreach (DataRow dr in dt1.Rows)
            {
                DataDomain dd = new DataDomain();
                dd.Name1 = ClientUtil.ToDateTime(dr["createdate"]).ToShortDateString();
                dd.Name2 = ClientUtil.ToDecimal(dr["submitsumquantity"]).ToString("N2");
                dd.Name3 = ClientUtil.ToDateTime(dr["confirmdate"]).ToShortDateString();
                dd.Name4 = ClientUtil.ToDecimal(dr["confirmmoney"]).ToString("N2");
                dd.Name5 = totalConfirmMoney.ToString("N2");
                dd.Name6 = fiscalCharges.ToString("N2");
                dd.Name7 = busCostSure.ToString("N2");
                dd.Name8 = dr["descript"].ToString();
                list.Add(dd);
            }
            return list;
        }

        #endregion

        #region 生产报表

        public Hashtable GetProduceReportData(int kjn, int kjy, string projectId, string taskSyscode)
        {
            Hashtable ht = new Hashtable();
            int nextYear = TransUtil.GetNextYear(kjn, kjy);
            int nextMonth = TransUtil.GetNextMonth(kjn, kjy);
            int beginSeasonMonth = TransUtil.GetSeasonBeginMonth(kjy);
            int endSeasonMonth = TransUtil.GetSeasonEndMonth(kjy);
            IList list_season = new ArrayList();
            IList list_monthProduce = new ArrayList();
            IList list_task = new ArrayList();
            IList list_value = new ArrayList();
            IList list_building = new ArrayList();
            DataDomain season_domain = new DataDomain();
            DataDomain monthProduce_domain = new DataDomain();
            DataDomain task_domain = new DataDomain();
            DataDomain value_domain = new DataDomain();
            DataDomain building_domain = new DataDomain();

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText =
                " select t1.id,t1.projectname,t1.structureType,t1.buildingarea,t1.groundlayers,t1.undergroundlayers,t1.begindate,t1.enddate," +
                " t1.constractstage,t1.contractcollectratio,t1.buildingheight,t1.realkgdate, " +
                " t1.projectcost,t1.managerdepart,t1.handlepersonname From resconfig t1 where t1.id = '" + projectId +
                "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            CurrentProjectInfo projectInfo = new CurrentProjectInfo();

            #region 1: 工程项目信息

            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    projectInfo = new CurrentProjectInfo();
                    projectInfo.Id = TransUtil.ToString(dataRow["id"]);
                    projectInfo.Name = TransUtil.ToString(dataRow["projectname"]);
                    projectInfo.HandlePersonName = TransUtil.ToString(dataRow["handlepersonname"]);
                    projectInfo.ConstractStage = TransUtil.ToString(dataRow["constractstage"]);
                    projectInfo.ManagerDepart = TransUtil.ToString(dataRow["managerdepart"]);
                    projectInfo.StructureType = TransUtil.ToInt(dataRow["structureType"]);
                    projectInfo.BuildingArea = TransUtil.ToDecimal(dataRow["buildingarea"]);
                    projectInfo.GroundLayers = TransUtil.ToDecimal(dataRow["groundlayers"]);
                    projectInfo.UnderGroundLayers = TransUtil.ToDecimal(dataRow["undergroundlayers"]);
                    projectInfo.BeginDate = TransUtil.ToDateTime(dataRow["begindate"]);
                    projectInfo.EndDate = TransUtil.ToDateTime(dataRow["enddate"]);
                    projectInfo.RealKGDate = TransUtil.ToDateTime(dataRow["realkgdate"]);
                    projectInfo.BuildingHeight = TransUtil.ToDecimal(dataRow["buildingheight"]);
                    projectInfo.ProjectCost = TransUtil.ToDecimal(dataRow["projectcost"]);
                    projectInfo.ContractCollectRatio = TransUtil.ToDecimal(dataRow["contractcollectratio"]);
                }
            }

            season_domain.Name1 = projectInfo.Name;
            season_domain.Name2 = Enum.GetName(typeof (EnumStructureType), projectInfo.StructureType);
            season_domain.Name3 = projectInfo.GroundLayers + projectInfo.UnderGroundLayers;
            season_domain.Name4 = projectInfo.BuildingArea + "";

            monthProduce_domain.Name1 = projectInfo.Name;
            monthProduce_domain.Name2 = Enum.GetName(typeof (EnumStructureType), projectInfo.StructureType);
            monthProduce_domain.Name3 = projectInfo.GroundLayers + projectInfo.UnderGroundLayers;
            monthProduce_domain.Name4 = projectInfo.BuildingArea + "";

            value_domain.Name1 = projectInfo.Name;
            value_domain.Name2 = Enum.GetName(typeof (EnumStructureType), projectInfo.StructureType);
            if (projectInfo.BeginDate != null)
            {
                value_domain.Name3 = projectInfo.BeginDate.ToShortDateString();
            }
            if (projectInfo.EndDate != null)
            {
                value_domain.Name4 = projectInfo.EndDate.ToShortDateString();
            }
            value_domain.Name5 = projectInfo.BuildingArea + "";
            value_domain.Name6 = projectInfo.BuildingHeight + "";

            task_domain.Name1 = projectInfo.Name;
            task_domain.Name2 = projectInfo.ProjectCost/10000 + "";
            task_domain.Name3 = Enum.GetName(typeof (EnumStructureType), projectInfo.StructureType);
            task_domain.Name4 = projectInfo.GroundLayers + "/-" + projectInfo.UnderGroundLayers;
            task_domain.Name5 = projectInfo.BuildingHeight + "";
            if (projectInfo.BeginDate != null)
            {
                task_domain.Name6 = projectInfo.BeginDate.ToShortDateString();
            }
            if (projectInfo.EndDate != null)
            {
                task_domain.Name7 = projectInfo.EndDate.ToShortDateString();
            }
            task_domain.Name8 = projectInfo.BuildingArea + "";
            if (projectInfo.RealKGDate != null &&
                projectInfo.RealKGDate.ToShortDateString().StartsWith(kjn + "") == true)
            {
                task_domain.Name9 = projectInfo.BuildingArea + "";
            }
            task_domain.Name18 = projectInfo.ConstractStage;

            building_domain.Name1 = projectInfo.Name;
            if (projectInfo.BeginDate != null)
            {
                building_domain.Name2 = projectInfo.BeginDate.ToShortDateString();
            }
            if (projectInfo.RealKGDate != null)
            {
                building_domain.Name3 = projectInfo.RealKGDate.ToShortDateString();
            }
            if (projectInfo.EndDate != null)
            {
                building_domain.Name6 = projectInfo.EndDate.ToShortDateString();
            }
            building_domain.Name9 = projectInfo.ConstractStage;
            building_domain.Name21 = projectInfo.ContractCollectRatio + "";

            #endregion

            #region 2: 契约

            command.CommandText =
                " select t1.projectvisa,t1.submitmoney,t1.confirmmoney from thd_contractgroup t1 where t1.contractgrouptype='工程签证索赔' ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            ContractGroup cGroup = new ContractGroup();
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    cGroup.ProjectVisa = TransUtil.ToDecimal(dataRow["projectvisa"]);
                    cGroup.SubmitMoney = TransUtil.ToDecimal(dataRow["submitmoney"]);
                    cGroup.ConfirmMoney = TransUtil.ToDecimal(dataRow["confirmmoney"]);
                }
            }

            building_domain.Name8 = cGroup.ProjectVisa;
            building_domain.Name14 = cGroup.SubmitMoney + "";
            building_domain.Name15 = cGroup.ConfirmMoney + "";

            #endregion

            #region 3: 业主报量单

            command.CommandText =
                " select t1.createyear,t1.createmonth,t1.auditmanage,t1.projectrecovery,t1.ownerbreach," +
                " round(t1.submitsumquantity/10000,4) submitsumquantity,round(t1.confirmsummoney/10000,4) confirmsummoney, " +
                " round(t1.collectionsummoney/10000,4) collectionsummoney,round(t1.sumpayformoney/10000,4) sumpayformoney " +
                " from thd_ownerquantitymaster t1 where t1.projectid='" + projectId + "' ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            OwnerQuantityMaster ownerQuntity = new OwnerQuantityMaster();
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ownerQuntity.SubmitSumQuantity = TransUtil.ToDecimal(dataRow["submitsumquantity"]);
                    ownerQuntity.ConfirmSumMoney = TransUtil.ToDecimal(dataRow["confirmsummoney"]);
                    ownerQuntity.CollectionSumMoney = TransUtil.ToDecimal(dataRow["collectionsummoney"]);
                    ownerQuntity.SumPayforMoney = TransUtil.ToDecimal(dataRow["sumpayformoney"]);
                    ownerQuntity.CreateYear = TransUtil.ToInt(dataRow["createyear"]);
                    ownerQuntity.CreateMonth = TransUtil.ToInt(dataRow["createmonth"]);
                    ownerQuntity.AuditManage = TransUtil.ToString(dataRow["auditmanage"]);
                    ownerQuntity.ProjectRecovery = TransUtil.ToString(dataRow["projectrecovery"]);
                    ownerQuntity.OwnerBreach = TransUtil.ToString(dataRow["ownerbreach"]);

                    if (ownerQuntity.CreateYear == kjn && ownerQuntity.CreateMonth == kjy)
                    {
                        building_domain.Name10 = (TransUtil.ToDecimal(building_domain.Name10) +
                                                  ownerQuntity.SubmitSumQuantity) + ""; //取自【业主报量管理】上月{报送金额}
                        building_domain.Name11 = (TransUtil.ToDecimal(building_domain.Name11) +
                                                  ownerQuntity.ConfirmSumMoney) + ""; //取自【业主报量管理】上月{确认金额}
                        building_domain.Name17 = (TransUtil.ToDecimal(building_domain.Name17) +
                                                  ownerQuntity.SumPayforMoney) + ""; //业主报量单上月应付数据
                        building_domain.Name18 = (TransUtil.ToDecimal(building_domain.Name18) +
                                                  ownerQuntity.CollectionSumMoney) + ""; //取自【业主报量管理】上月{收款金额}
                    }
                    building_domain.Name12 = (TransUtil.ToDecimal(building_domain.Name12) +
                                              ownerQuntity.SubmitSumQuantity) + ""; //取自【业主报量管理】累计{报送金额}
                    building_domain.Name13 = (TransUtil.ToDecimal(building_domain.Name13) + ownerQuntity.ConfirmSumMoney) +
                                             ""; //取自【业主报量管理】累计{确认金额}
                    building_domain.Name16 += ownerQuntity.AuditManage; //对项目报量的审核”属性
                    building_domain.Name19 = (TransUtil.ToDecimal(building_domain.Name19) + ownerQuntity.SumPayforMoney) +
                                             ""; //取自业主报量累计应付数据
                    building_domain.Name20 = (TransUtil.ToDecimal(building_domain.Name20) +
                                              ownerQuntity.CollectionSumMoney) + ""; //取自【业主报量管理】累计{收款金额}
                    building_domain.Name23 += ownerQuntity.ProjectRecovery; //工程款回收情况备注
                    building_domain.Name24 += ownerQuntity.OwnerBreach; //业主违约备注
                }
            }
            if (TransUtil.ToDecimal(building_domain.Name19) != 0)
            {
                building_domain.Name22 =
                    decimal.Round(
                        TransUtil.ToDecimal(building_domain.Name20)*100/TransUtil.ToDecimal(building_domain.Name19), 2);
                //实际付款比例
            }

            #endregion

            #region 4: 自行产值

            command.CommandText =
                " select t1.accountyear,t1.accountmonth, round(sum(t2.planvalue)/10000,4) planvalue " +
                " From thd_produceselfvaluemaster t1,thd_produceselfvaluedetail t2 " +
                " where t1.id=t2.parentid and t1.projectid='" + projectId + "' and t2.gwbstreesyscode like '" +
                taskSyscode + "%'"
                + " group by t1.accountyear,t1.accountmonth ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int createYear = TransUtil.ToInt(dataRow["accountyear"]);
                    int createMonth = TransUtil.ToInt(dataRow["accountmonth"]);
                    if (createYear == kjn && createMonth >= beginSeasonMonth && createMonth <= endSeasonMonth)
                    {
                        value_domain.Name13 = (TransUtil.ToDecimal(value_domain.Name13) +
                                               TransUtil.ToDecimal(dataRow["planvalue"])) + ""; //自季初[计划值]
                    }
                    if (createYear == kjn && createMonth == kjy)
                    {
                        value_domain.Name19 = (TransUtil.ToDecimal(value_domain.Name19) +
                                               TransUtil.ToDecimal(dataRow["planvalue"])) + ""; //本月[计划值]
                    }
                }
            }

            //实际值
            command.CommandText =
                " select t1.kjn,t1.kjy,round(sum(t2.currincometotalprice)/10000,4) realvalue from thd_costmonthaccount t1" +
                " inner join thd_costmonthaccountdtl t2 on t1.id=t2.parentid " +
                " where t1.theprojectguid = '" + projectId + "' and t2.accounttasknodesyscode like '" + taskSyscode +
                "%'" +
                " group by t1.kjn,t1.kjy ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int createYear = TransUtil.ToInt(dataRow["kjn"]);
                    int createMonth = TransUtil.ToInt(dataRow["kjy"]);
                    if (createYear < kjn || (createYear == kjn && createMonth <= kjy))
                    {
                        task_domain.Name15 = (TransUtil.ToDecimal(task_domain.Name15) +
                                              TransUtil.ToDecimal(dataRow["realvalue"])) + ""; //自开工
                    }
                    if (createYear == kjn && createMonth <= kjy)
                    {
                        task_domain.Name16 = (TransUtil.ToDecimal(task_domain.Name16) +
                                              TransUtil.ToDecimal(dataRow["realvalue"])) + ""; //自年初
                    }
                    if (createYear == kjn && createMonth == kjy)
                    {
                        task_domain.Name17 = (TransUtil.ToDecimal(task_domain.Name17) +
                                              TransUtil.ToDecimal(dataRow["realvalue"])) + ""; //本月
                    }
                    //施工产值[自行]
                    if (createYear < kjn || (createYear == kjn && createMonth <= kjy))
                    {
                        value_domain.Name8 = (TransUtil.ToDecimal(value_domain.Name8) +
                                              TransUtil.ToDecimal(dataRow["realvalue"])) + ""; //自开工
                    }
                    if (createYear == kjn && createMonth <= kjy)
                    {
                        value_domain.Name11 = (TransUtil.ToDecimal(value_domain.Name11) +
                                               TransUtil.ToDecimal(dataRow["realvalue"])) + ""; //自年初
                    }
                    if (createYear == kjn && createMonth >= beginSeasonMonth && createMonth <= kjy)
                    {
                        value_domain.Name15 = (TransUtil.ToDecimal(value_domain.Name15) +
                                               TransUtil.ToDecimal(dataRow["realvalue"])) + ""; //自季初
                    }
                    if (createYear == kjn && createMonth == kjy)
                    {
                        value_domain.Name21 = (TransUtil.ToDecimal(value_domain.Name21) +
                                               TransUtil.ToDecimal(dataRow["realvalue"])) + ""; //本月
                    }
                }
            }

            #endregion

            #region 5: 专项管理费用

            command.CommandText =
                "select t1.audityear,t1.auditmonth,round(sum(t2.money)/10000,4)  currentrealincome from thd_specostsettlementmaster t1, " +
                " thd_specostsettlementdetail t2 where t1.id=t2.parentid and t1.projectid='" + projectId + "'" +
                " and t1.audityear > 0 and t2.engtasksyscode like '%" + taskSyscode +
                "%' group by t1.audityear,t1.auditmonth";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            decimal specSeasonPlanValue = 0;
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int createYear = TransUtil.ToInt(dataRow["audityear"]);
                    int createMonth = TransUtil.ToInt(dataRow["auditmonth"]);
                    if (createYear < kjn || (createYear == kjn && createMonth <= kjy))
                    {
                        task_domain.Name12 = (TransUtil.ToDecimal(task_domain.Name12) +
                                              TransUtil.ToDecimal(dataRow["currentrealincome"])) + ""; //自开工
                    }
                    if (createYear == kjn && createMonth <= kjy)
                    {
                        task_domain.Name13 = (TransUtil.ToDecimal(task_domain.Name13) +
                                              TransUtil.ToDecimal(dataRow["currentrealincome"])) + ""; //自年初
                    }
                    if (createYear == kjn && createMonth == kjy)
                    {
                        task_domain.Name14 = (TransUtil.ToDecimal(task_domain.Name14) +
                                              TransUtil.ToDecimal(dataRow["currentrealincome"])) + ""; //本月
                    }
                    //施工产值[总包]
                    if (createYear < kjn || (createYear == kjn && createMonth <= kjy))
                    {
                        value_domain.Name9 = (TransUtil.ToDecimal(value_domain.Name7) +
                                              TransUtil.ToDecimal(dataRow["currentrealincome"])) + ""; //自开工
                    }
                    if (createYear == kjn && createMonth <= kjy)
                    {
                        value_domain.Name12 = (TransUtil.ToDecimal(value_domain.Name10) +
                                               TransUtil.ToDecimal(dataRow["currentrealincome"])) + ""; //自年初
                    }
                    if (createYear == kjn && createMonth >= beginSeasonMonth && createMonth <= kjy)
                    {
                        value_domain.Name16 = (TransUtil.ToDecimal(value_domain.Name14) +
                                               TransUtil.ToDecimal(dataRow["currentrealincome"])) + ""; //自季初
                        //specSeasonPlanValue += TransUtil.ToDecimal(dataRow["currentplanincome"]);//季度计划值[分包]
                    }
                    if (createYear == kjn && createMonth == kjy)
                    {
                        value_domain.Name22 = (TransUtil.ToDecimal(value_domain.Name20) +
                                               TransUtil.ToDecimal(dataRow["currentrealincome"])) + ""; //本月
                        //specMonthPlanValue += TransUtil.ToDecimal(dataRow["currentplanincome"]);//月计划值[分包]
                    }
                }
            }

            //构造计算数据[产值完成情况]
            value_domain.Name7 = (TransUtil.ToDecimal(value_domain.Name8) + TransUtil.ToDecimal(value_domain.Name9)) +
                                 ""; //分包完成[自开工]
            value_domain.Name10 = (TransUtil.ToDecimal(value_domain.Name11) + TransUtil.ToDecimal(value_domain.Name12)) +
                                  ""; //分包完成[自年初]
            value_domain.Name14 = (TransUtil.ToDecimal(value_domain.Name15) + TransUtil.ToDecimal(value_domain.Name16)) +
                                  ""; //分包完成[自季初]
            value_domain.Name20 = (TransUtil.ToDecimal(value_domain.Name21) + TransUtil.ToDecimal(value_domain.Name22)) +
                                  ""; //分包完成[本月]
            //value_domain.Name18 = (TransUtil.ToDecimal(value_domain.Name19) + specMonthPlanValue) + "";//本月分包[计划值]
            if (TransUtil.ToDecimal(value_domain.Name13) != 0)
            {
                value_domain.Name17 =
                    decimal.Round(
                        TransUtil.ToDecimal(value_domain.Name15)*100/TransUtil.ToDecimal(value_domain.Name13), 2) + "";
                //自行产值完成率[自季初]
            }
            if (TransUtil.ToDecimal(value_domain.Name19) != 0)
            {
                value_domain.Name23 =
                    decimal.Round(
                        TransUtil.ToDecimal(value_domain.Name21)*100/TransUtil.ToDecimal(value_domain.Name19), 2) + "";
                //自行产值完成率[本月]
            }

            //构造计算数据[月度生产计划]
            monthProduce_domain.Name5 = value_domain.Name18;
            monthProduce_domain.Name6 = value_domain.Name19; //计划自行完成产值（万元）

            //构造计算数据[季度生产计划]
            season_domain.Name5 = (specSeasonPlanValue + TransUtil.ToDecimal(value_domain.Name13)) + "";
            season_domain.Name6 = value_domain.Name13; //计划自行完成产值（万元）

            #endregion

            #region 6: 进度计划

            /*//6.1 取核算节点信息
            command.CommandText = " select t1.syscode,t1.name from thd_gwbstree t1 where t1.costaccflag=1 and t1.theprojectguid='" + projectId + "' ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            Hashtable ht_accountNode = new Hashtable();
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (!ht_accountNode.Contains(TransUtil.ToString(dataRow["syscode"])))
                    {
                        ht_accountNode.Add(TransUtil.ToString(dataRow["syscode"]), TransUtil.ToString(dataRow["name"]));
                    }
                }
            }

            //6.2 月度进度计划
            string progressStr = "";
            Hashtable ht_monthSchedual = new Hashtable();
            command.CommandText = " select t1.accountyear,t1.accountmonth,t2.gwbstreesyscode,t2.gwbstreename,t2.plannedenddate," +
                                   " (t2.plannedenddate-t3.plannedbegindate) plandiff,(t3.plannedenddate -t3.plannedbegindate) rplandiff "+
                                   " from thd_weekschedulemaster t1,thd_weekscheduledetail t2,THD_ProductionScheduleDetail t3 " +
                                   " where t1.id=t2.parentid and t2.forwardbilldtlid=t3.id and t1.execscheduletype in (20) and " +
                                   " t1.projectid='" + projectId + "' and t1.accountyear=" + nextYear + " and t1.accountmonth=" + nextMonth;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int createYear = TransUtil.ToInt(dataRow["accountingyear"]);
                    int createMonth = TransUtil.ToInt(dataRow["accountingmonth"]);
                    string gwbstreename = TransUtil.ToString(dataRow["gwbstreename"]);
                    if (createYear == nextYear && createMonth == nextMonth)
                    {
                        int plandiff = TransUtil.ToInt(dataRow["plandiff"]);//月进度计划任务结束时间-总滚动进度计划中计划开始时间
                        int rplandiff = TransUtil.ToInt(dataRow["rplandiff"]);//总滚动进度计划中计划结束时间-总滚动进度计划中计划开始时间

                        if (rplandiff > 0)
                        {
                            decimal progress = decimal.Round(plandiff / rplandiff, 2);
                            progressStr += "[" + gwbstreename + ": " + progress + "]";
                        }
                        
                        string syscode = TransUtil.ToString(dataRow["gwbstreesyscode"]);
                        foreach (string accountSyscode in ht_accountNode.Keys)
                        {
                            string accountName = TransUtil.ToString(ht_accountNode[accountSyscode]);
                            if (syscode.Contains(accountSyscode) && !ht_monthSchedual.Contains(accountSyscode))
                            {
                                ht_monthSchedual.Add(accountSyscode, accountName);
                            }
                        }
                    }
                }
            }
            monthProduce_domain.Name7 = progressStr;//本月要求达到形象进度

            //6.3 取本月成本核算的明细数据
            decimal sumMonthIncome = 0;
            decimal sumSeasonIncome = 0;
            int lastSeasonYear = TransUtil.GetLastSeasonYear(kjn, kjy);
            int lastSeasonEndMonth = TransUtil.GetLastSeasonEndMonth(kjn, kjy);
            command.CommandText = " select t1.kjn,t1.kjy,t2.accounttasknodesyscode,t2.sumcompletedpercent,t2.sumincometotalprice "+
                                    " from thd_costmonthaccount t1,thd_costmonthaccountdtl t2 where t1.id=t2.parentid and " +
                                    " t1.theprojectguid='" + projectId + "' and ((t1.kjn=" + kjn + " and t1.kjy=" + kjy + ") " +
                                    " or (t1.kjn=" + lastSeasonYear + " and t1.kjy=" + lastSeasonEndMonth + ")) ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            IList list_monthdomain = new ArrayList();
            IList list_seasondomain = new ArrayList();
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int currKjn = TransUtil.ToInt(dataRow["kjn"]);
                    int currKjy = TransUtil.ToInt(dataRow["kjy"]);
                    DataDomain domain = new DataDomain();
                    domain.Name1 = TransUtil.ToString(dataRow["accounttasknodesyscode"]);
                    domain.Name2 = TransUtil.ToString(dataRow["sumcompletedpercent"]);
                    domain.Name3 = TransUtil.ToString(dataRow["sumincometotalprice"]);
                    bool isOk = false;
                    foreach (string accountSyscode in ht_monthSchedual.Keys)
                    {
                        if (TransUtil.ToString(domain.Name1).Contains(accountSyscode))
                        {
                            isOk = true;
                        }
                    }
                    if (isOk == true && currKjn == kjn && currKjy == kjy)
                    {
                        list_monthdomain.Add(domain);
                        sumMonthIncome += TransUtil.ToDecimal(domain.Name3);
                    }
                    else {
                        list_seasondomain.Add(domain);
                        sumSeasonIncome += TransUtil.ToDecimal(domain.Name3);
                    }
                }
            }
            //6.4 取核算形象进度
            string monthPercentStr = "";
            if (sumMonthIncome > 0)
            {
                foreach (string accountSyscode in ht_monthSchedual.Keys)
                {
                    decimal currPercent = 0;
                    string accountName = TransUtil.ToString(ht_accountNode[accountSyscode]);
                    foreach (DataDomain domain in list_monthdomain)
                    {
                        if (TransUtil.ToString(domain.Name1).Contains(accountSyscode))
                        {
                            currPercent += decimal.Round(TransUtil.ToDecimal(domain.Name2) * TransUtil.ToDecimal(domain.Name3) / sumMonthIncome, 2);
                        }
                    }
                    monthPercentStr += "[" + accountName + ":" + currPercent + "%]";
                }
                monthProduce_domain.Name8 = monthPercentStr;//上月计划完成情况
            }*/

            #endregion

            list_season.Add(season_domain);
            list_monthProduce.Add(monthProduce_domain);
            list_task.Add(task_domain);
            list_value.Add(value_domain);
            list_building.Add(building_domain);
            ht.Add("1", list_season);
            ht.Add("2", list_monthProduce);
            ht.Add("3", list_task);
            ht.Add("4", list_value);
            ht.Add("5", list_building);
            return ht;
        }

        #endregion

        #region 安装月度成本分析报表

        public IList GetSpecialCollectReportData(int kjn, int kjy, string projectId, string accountOrgGUID,
                                                 IList taskList, string qwbsGUID)
        {
            #region SQL条件设置

            string sqlMonthTaskStr = ""; //月度成本核算SQL
            foreach (string accountTaskGUID in taskList)
            {
                if (sqlMonthTaskStr == "")
                {
                    sqlMonthTaskStr = " and ( t2.accounttasknodesyscode like '%" + accountTaskGUID + "%' ";
                }
                else
                {
                    sqlMonthTaskStr += " or t2.accounttasknodesyscode like '%" + accountTaskGUID + "%' ";
                }
            }
            sqlMonthTaskStr += " ) ";

            string sqlSubTaskStr = ""; //分包结算SQL
            foreach (string accountTaskGUID in taskList)
            {
                if (sqlSubTaskStr == "")
                {
                    sqlSubTaskStr = " and ( t2.balancetasksyscode like '%" + accountTaskGUID + "%' ";
                }
                else
                {
                    sqlSubTaskStr += " or t2.balancetasksyscode like '%" + accountTaskGUID + "%' ";
                }
            }
            sqlSubTaskStr += " ) ";

            string sqlProjectTaskStr = ""; //工程任务SQL
            foreach (string accountTaskGUID in taskList)
            {
                if (sqlProjectTaskStr == "")
                {
                    sqlProjectTaskStr = " and ( t2.accounttasknodesyscode like '%" + accountTaskGUID + "%' ";
                }
                else
                {
                    sqlProjectTaskStr += " or t2.accounttasknodesyscode like '%" + accountTaskGUID + "%' ";
                }
            }
            sqlProjectTaskStr += " ) ";

            string sqlStockTaskStr = ""; //出库SQL
            foreach (string accountTaskGUID in taskList)
            {
                if (sqlStockTaskStr == "")
                {
                    sqlStockTaskStr = " and ( t2.UsedPartSysCode like '%" + accountTaskGUID + "%' ";
                }
                else
                {
                    sqlStockTaskStr += " or t2.UsedPartSysCode like '%" + accountTaskGUID + "%' ";
                }
            }
            sqlStockTaskStr += " ) ";

            string sqlInventoryTaskStr = ""; //盘点SQL
            foreach (string accountTaskGUID in taskList)
            {
                if (sqlInventoryTaskStr == "")
                {
                    sqlInventoryTaskStr = " and ( t1.UserPartSysCode like '%" + accountTaskGUID + "%' ";
                }
                else
                {
                    sqlInventoryTaskStr += " or t1.UserPartSysCode like '%" + accountTaskGUID + "%' ";
                }
            }
            sqlInventoryTaskStr += " ) ";

            #endregion;

            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            string sqlInventory =
                @"select t1.createyear,t1.createmonth,nvl(sum(t2.money)/10000,0) supplymoney, nvl(sum(t2.confirmmoney)/10000,0) confirmmoney
                        from thd_stockinventorymaster t1,thd_stockinventorydetail t2 where t1.id=t2.parentid " +
                sqlInventoryTaskStr + @" group by t1.createyear,t1.createmonth";
            command.CommandText = sqlInventory;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dsInventory = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            Hashtable lastInventory_ht = new Hashtable(); //上月盘点数据
            if (dsInventory != null)
            {
                DataTable dataTable = dsInventory.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain data_domain = new DataDomain();
                    data_domain.Name1 = TransUtil.ToString(dataRow["createyear"]); //会计年
                    data_domain.Name2 = TransUtil.ToString(dataRow["createmonth"]); //会计月
                    data_domain.Name13 = TransUtil.ToString(dataRow["supplymoney"]); //库存盘点消耗量(采购)
                    data_domain.Name14 = TransUtil.ToString(dataRow["confirmmoney"]); //库存盘点消耗量(认价)
                    lastInventory_ht.Add(data_domain.Name1 + "_" + data_domain.Name2, data_domain);
                }
            }
            //取核算科目编码
            string azzcSubjectGUID = ""; //安装主材
            string azsbSubjectGUID = ""; //安装设备
            string subjectSql =
                "select t1.code,t1.id from thd_costaccountsubject t1 where t1.code in ('C5110222','C5110223')";
            command.CommandText = subjectSql;
            dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (TransUtil.ToString(dataRow["code"]) == "C5110222")
                    {
                        azzcSubjectGUID = TransUtil.ToString(dataRow["id"]);
                    }
                    if (TransUtil.ToString(dataRow["code"]) == "C5110223")
                    {
                        azsbSubjectGUID = TransUtil.ToString(dataRow["id"]);
                    }
                }
            }

            string qwbsSql = "";
            if (TransUtil.ToString(qwbsGUID) != "")
            {
                qwbsSql = "and t2.qwbscode like '%" + qwbsGUID + "%'";
            }
            string sql =
                @" select createyear,createmonth,sum(submitsumquantity) submitsumquantity,sum(affirmsummoney) affirmsummoney,sum(sumpayformoney) sumpayformoney,
                            sum(indirectmoney) indirectmoney,sum(reportmoney) reportmoney,sum(reportmaterial) reportmaterial,sum(reportequip) reportequip,
                            sum(submoney) submoney,sum(submaterial) submaterial,sum(subequip) subequip,sum(supplymoney) supplymoney,sum(confirmmoney) confirmmoney
                            from ( ";

            #region 1:业主报量数据

            sql += @" select t1.createyear,t1.createmonth,sum(round(t2.submitquantity/10000,4)) submitsumquantity,sum(round(t2.confirmmoney/10000,4)) affirmsummoney,
                            sum(round(t2.payformoney/10000,4)) sumpayformoney,0 indirectmoney,0 reportmoney,0 reportmaterial,0 reportequip,0 submoney,
                            0 submaterial,0 subequip,0 supplymoney,0 confirmmoney
                            from thd_ownerquantitymaster t1,thd_ownerquantitydetail t2 where t1.id=t2.parentid and 
                            t1.projectid='" + projectId + "' " + qwbsSql +
                   "  group by t1.createyear,t1.createmonth ";

            #endregion;

            #region 2:实际耗用间接费/总产值/主材合同收入/设备产值数据

            sql +=
                @"  union all select t1.kjn createyear,t1.kjy createmonth,0 submitsumquantity,0 affirmsummoney,0 sumpayformoney,
                        sum(round(t3.currrealconsumetotalprice/10000,4)) indirectmoney,0 reportmoney,0 reportmaterial,0 reportequip,0 submoney,
                        0 submaterial,0 subequip,0 supplymoney,0 confirmmoney
                        from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3
                        where t1.id=t2.parentid and t2.id=t3.parentid and t3.costsubjectcode like 'C513%' " +
                sqlMonthTaskStr + @" group by t1.kjn,t1.kjy
                        union all
                        select t1.kjn createyear,t1.kjy createmonth,0 submitsumquantity,0 affirmsummoney,0 sumpayformoney,
                        0 indirectmoney,sum(round(t3.currincometotalprice/10000,4)) reportmoney,0 reportmaterial,0 reportequip,0 submoney,
                        0 submaterial,0 subequip,0 supplymoney,0 confirmmoney
                        from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3
                        where t1.id=t2.parentid and t2.id=t3.parentid  " +
                sqlMonthTaskStr + @" group by t1.kjn,t1.kjy
                        union all
                        select t1.kjn createyear,t1.kjy createmonth,0 submitsumquantity,0 affirmsummoney,0 sumpayformoney,
                        0 indirectmoney,0 reportmoney,sum(round(t3.currrealconsumetotalprice/10000,4)) reportmaterial,0 reportequip,0 submoney,
                        0 submaterial,0 subequip,0 supplymoney,0 confirmmoney  
                        from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3
                        where t1.id=t2.parentid and t2.id=t3.parentid and t3.costsubjectcode like 'C5110222%' " +
                sqlMonthTaskStr + @"  group by t1.kjn,t1.kjy
                        union all
                        select t1.kjn createyear,t1.kjy createmonth,0 submitsumquantity,0 affirmsummoney,0 sumpayformoney,
                        0 indirectmoney,0 reportmoney,0 reportmaterial,sum(round(t3.currrealconsumetotalprice/10000,4)) reportequip,0 submoney,
                        0 submaterial,0 subequip,0 supplymoney,0 confirmmoney   
                        from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3
                        where t1.id=t2.parentid and t2.id=t3.parentid and t3.costsubjectcode like 'C5110223%' " +
                sqlMonthTaskStr + @"  group by t1.kjn,t1.kjy ";

            #endregion;

            #region 3:分包结算数据

            sql += @"   union all select t1.createyear,t1.createmonth,0 submitsumquantity,0 affirmsummoney,0 sumpayformoney,
                        0 indirectmoney,0 reportmoney,0 reportmaterial,0 reportequip,sum(round(t3.balancetotalprice/10000,4)) submoney,
                        0 submaterial,0 subequip,0 supplymoney,0 confirmmoney   
                        from thd_subcontractbalancebill t1,thd_subcontractbalancedetail t2,thd_subcontractbalsubjectdtl t3
                        where t1.id=t2.parentid and t1.state=5 and t2.id=t3.parentid " +
                   sqlSubTaskStr + @" group by t1.createyear,t1.createmonth
                        union all
                        select t1.createyear,t1.createmonth,0 submitsumquantity,0 affirmsummoney,0 sumpayformoney,
                        0 indirectmoney,0 reportmoney,0 reportmaterial,0 reportequip,0 submoney,
                        sum(round(t3.accounttotalprice/10000,4)) submaterial,0 subequip,0 supplymoney,0 confirmmoney
                        from thd_projecttaskaccountbill t1,thd_projecttaskdetailaccount t2,thd_projecttaskdtlacctsubject t3 
                        where t1.id=t2.parentid and t1.state=5 and t2.id=t3.parentid and t3.accountcostsyscode like '%" +
                   azzcSubjectGUID + "%' " + sqlProjectTaskStr + @"  group by t1.createyear,t1.createmonth 
                        union all 
                        select t1.createyear,t1.createmonth,0 submitsumquantity,0 affirmsummoney,0 sumpayformoney,
                        0 indirectmoney,0 reportmoney,0 reportmaterial,0 reportequip,0 submoney,
                        0 submaterial,sum(round(t3.accounttotalprice/10000,4)) subequip,0 supplymoney,0 confirmmoney
                        from thd_projecttaskaccountbill t1,thd_projecttaskdetailaccount t2,thd_projecttaskdtlacctsubject t3 
                        where t1.id=t2.parentid and t1.state=5 and t2.id=t3.parentid and t3.accountcostsyscode like '%" +
                   azsbSubjectGUID + "%' " + sqlProjectTaskStr + @"  group by t1.createyear,t1.createmonth";

            #endregion;

            #region 4:库存盘点消耗数据(上期盘点+本期出库-本期盘点)

            sql += @"   union all select t1.createyear,t1.createmonth,0 submitsumquantity,0 affirmsummoney,0 sumpayformoney,
                        0 indirectmoney,0 reportmoney,0 reportmaterial,0 reportequip,0 submoney,
                        0 submaterial,0 subequip,round(sum(t2.price*t2.quantity)/10000,2) supplymoney, round(sum(t2.confirmprice*t2.quantity)/10000,2) confirmmoney
                        from thd_stkstockout t1,thd_stkstockoutdtl t2  where t1.id=t2.parentid " +
                   sqlStockTaskStr + @"   group by t1.createyear,t1.createmonth
                        union all 
                        select t1.createyear,t1.createmonth,0 submitsumquantity,0 affirmsummoney,0 sumpayformoney,
                        0 indirectmoney,0 reportmoney,0 reportmaterial,0 reportequip,0 submoney,
                        0 submaterial,0 subequip,-nvl(sum(t2.money)/10000,0) supplymoney, -nvl(sum(t2.confirmmoney)/10000,0) confirmmoney
                        from thd_stockinventorymaster t1,thd_stockinventorydetail t2 where t1.id=t2.parentid  " +
                   sqlInventoryTaskStr + @"  group by t1.createyear,t1.createmonth";

            #endregion;

            sql += @" ) group by createyear,createmonth order by createyear,createmonth ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain dataCollect_domain = new DataDomain();
                    dataCollect_domain.Name1 = TransUtil.ToString(dataRow["createyear"]); //会计年
                    dataCollect_domain.Name2 = TransUtil.ToString(dataRow["createmonth"]); //会计月
                    dataCollect_domain.Name3 = TransUtil.ToString(dataRow["submitsumquantity"]); //报送金额
                    dataCollect_domain.Name4 = TransUtil.ToString(dataRow["affirmsummoney"]); //确认金额
                    dataCollect_domain.Name5 = TransUtil.ToString(dataRow["sumpayformoney"]); //付款金额
                    dataCollect_domain.Name6 = TransUtil.ToString(dataRow["indirectmoney"]); //间接费
                    dataCollect_domain.Name7 = TransUtil.ToString(dataRow["reportmoney"]); //公司报量产值
                    dataCollect_domain.Name8 = TransUtil.ToString(dataRow["reportmaterial"]); //公司报量主材
                    dataCollect_domain.Name9 = TransUtil.ToString(dataRow["reportequip"]); //公司报量设备
                    dataCollect_domain.Name10 = TransUtil.ToString(dataRow["submoney"]); //分包结算额
                    dataCollect_domain.Name11 = TransUtil.ToString(dataRow["submaterial"]); //分包结算主材
                    dataCollect_domain.Name12 = TransUtil.ToString(dataRow["subequip"]); //分包结算设备
                    //取得上月盘点数据
                    int lastYear = 0, lastMonth = 0;
                    decimal lastSupplyMoney = 0;
                    decimal lastConfirmmoney = 0;
                    //上个会计期
                    if (TransUtil.ToInt(dataCollect_domain.Name2) == 12)
                    {
                        lastYear = TransUtil.ToInt(dataCollect_domain.Name1) - 1;
                        lastMonth = 1;
                    }
                    else
                    {
                        lastYear = TransUtil.ToInt(dataCollect_domain.Name1);
                        lastMonth = TransUtil.ToInt(dataCollect_domain.Name2) - 1;
                    }
                    if (lastInventory_ht.Contains(lastYear + "_" + lastMonth))
                    {
                        DataDomain lastDomain = (DataDomain) lastInventory_ht[lastYear + "_" + lastMonth];
                        lastSupplyMoney = TransUtil.ToDecimal(lastDomain.Name13);
                        lastConfirmmoney = TransUtil.ToDecimal(lastDomain.Name14);
                    }
                    dataCollect_domain.Name13 = (lastSupplyMoney + TransUtil.ToDecimal(dataRow["supplymoney"])) + "";
                    //库存盘点消耗量(采购)
                    dataCollect_domain.Name14 = (lastConfirmmoney + TransUtil.ToDecimal(dataRow["confirmmoney"])) + "";
                    //库存盘点消耗量(认价)
                    list.Add(dataCollect_domain);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取预计整体数（安装报表）
        /// </summary>
        /// <param name="accountTaskGUID">核算节点</param>
        /// <param name="accountOrgGUID">归属核算组织</param>
        /// <returns></returns>
        public IList GetWholePlanData(int kjn, int kjy, string projectId, string accountTaskGUID, string accountOrgGUID)
        {
            decimal buildingArea = 0; //项目建筑面积
            DateTime endDate = System.DateTime.Now;

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select t1.buildingarea From resconfig t1 where t1.id = '" + projectId + "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    buildingArea = TransUtil.ToDecimal(dataRow["buildingarea"]);
                }
            }

            #region 1:取成本核算科目信息

            Hashtable ht_subject = new Hashtable();
            string sql = @"select t1.id,t1.code from thd_costaccountsubject t1 ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ht_subject.Add(TransUtil.ToString(dataRow["id"]), TransUtil.ToString(dataRow["code"]));
                }
            }

            #endregion

            #region 2:取会计期的结束日期

            /*sql = @"select t1.enddate from resfiscalperioddet t1 where t1.fiscalyear=" + kjn + " and t1.fiscalmonth= " + kjy;
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    endDate = TransUtil.ToDateTime(dataRow["enddate"]);
                }
            }*/

            #endregion

            #region 3:取核算节点下的预算信息

            IList list_gwbs = new ArrayList();
            sql = @" select round(sum(t1.contracttotalprice)/10000,4) contracttotalprice,round(sum(t1.responsibilitilytotalprice)/10000,4) responsibilitilytotalprice,
                            sum(t1.planworkamount) planworkamount,round(sum(t1.plantotalprice)/10000,4) plantotalprice,costaccountsubjectguid
                            from thd_gwbsdetailcostsubject t1,thd_gwbsdetail t2 where t1.gwbsdetailid=t2.id and t2.costingflag=1 and
                            t1.thegwbstreesyscode like '%" + accountTaskGUID +
                  "%' group by t1.costaccountsubjectguid ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name1 = TransUtil.ToString(dataRow["costaccountsubjectguid"]);
                    domain.Name2 = TransUtil.ToString(ht_subject[domain.Name1]);
                    domain.Name3 = TransUtil.ToString(dataRow["contracttotalprice"]); //合同收入
                    domain.Name4 = TransUtil.ToString(dataRow["responsibilitilytotalprice"]); //责任成本
                    domain.Name5 = TransUtil.ToString(dataRow["planworkamount"]); //计划工程量
                    domain.Name6 = TransUtil.ToString(dataRow["plantotalprice"]); //计划合价
                    if (TransUtil.ToDecimal(domain.Name5) != 0)
                    {
                        domain.Name7 =
                            decimal.Round(TransUtil.ToDecimal(domain.Name6)/TransUtil.ToDecimal(domain.Name5), 4) + "";
                        //计划单价
                    }
                    list_gwbs.Add(domain);
                }
            }

            #endregion

            #region 4:取领料出库中的消耗信息

            /*IList list_stockout = new ArrayList();
            sql = @"select sum(t2.quantity) quantity,round(sum(t2.money)/10000,4) money,t2.subjectguid from thd_stkstockout t1,thd_stkstockoutdtl t2 where t1.id=t2.parentid and t1.stockoutmanner = 20
                    and t1.createdate<to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd') and t1.opgsyscode like '%" + accountOrgGUID + "%' group by t2.subjectguid ";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name1 = TransUtil.ToString(dataRow["subjectguid"]);
                    domain.Name2 = TransUtil.ToString(ht_subject[domain.Name1]);
                    domain.Name3 = TransUtil.ToString(dataRow["quantity"]);//实际消耗数量
                    domain.Name4 = TransUtil.ToString(dataRow["money"]);//实际消耗金额
                    if (TransUtil.ToDecimal(domain.Name3) != 0)
                    {
                        domain.Name5 = decimal.Round(TransUtil.ToDecimal(domain.Name4) / TransUtil.ToDecimal(domain.Name3), 4) + "";
                    }
                    list_stockout.Add(domain);
                }
            }*/

            #endregion

            #region 5:通过算法构造整体预计信息

            //5.1 计算实际合价
            /*foreach (DataDomain gwbsDomain in list_gwbs)
            {
                foreach (DataDomain stockDomain in list_stockout)
                {
                    if (TransUtil.ToString(stockDomain.Name2).Contains(TransUtil.ToString(gwbsDomain.Name2)))
                    {
                        decimal planQty = TransUtil.ToDecimal(gwbsDomain.Name5);
                        decimal stockQty = TransUtil.ToDecimal(stockDomain.Name3);
                        if (stockQty >= planQty)
                        {
                            gwbsDomain.Name6 = decimal.Round(planQty * TransUtil.ToDecimal(stockDomain.Name5), 4) + "";
                        }
                        else
                        {
                            gwbsDomain.Name6 = decimal.Round((stockQty * TransUtil.ToDecimal(stockDomain.Name5) + (planQty - stockQty) * TransUtil.ToDecimal(gwbsDomain.Name7)), 4) + "";
                        }
                        break;
                    }
                }
            }*/
            //4.2 计算其他值
            //foreach (DataDomain gwbsDomain in list_gwbs)
            //{
            //    gwbsDomain.Name8 = (TransUtil.ToDecimal(gwbsDomain.Name3) - TransUtil.ToDecimal(gwbsDomain.Name6)) + "";//利润（万元）
            //    if (TransUtil.ToDecimal(gwbsDomain.Name3) != 0)
            //    {
            //        gwbsDomain.Name9 = decimal.Round(TransUtil.ToDecimal(gwbsDomain.Name8) * 100 / TransUtil.ToDecimal(gwbsDomain.Name3), 4);//利润率(%)
            //    }
            //    gwbsDomain.Name10 = (TransUtil.ToDecimal(gwbsDomain.Name4) - TransUtil.ToDecimal(gwbsDomain.Name6)) + "";//超成本降低(万元)
            //    if (TransUtil.ToDecimal(gwbsDomain.Name3) != 0)
            //    {
            //        gwbsDomain.Name11 = decimal.Round(TransUtil.ToDecimal(gwbsDomain.Name10) * 100 / TransUtil.ToDecimal(gwbsDomain.Name4), 4);//超成本降低率(%)
            //    }
            //    if (buildingArea != 0)
            //    {
            //        gwbsDomain.Name12 = decimal.Round(TransUtil.ToDecimal(gwbsDomain.Name3) * 10000 / buildingArea, 4);//预计每平米造价(元/平方米) 合同收入
            //        gwbsDomain.Name13 = decimal.Round(TransUtil.ToDecimal(gwbsDomain.Name4) * 10000 / buildingArea, 4);
            //        gwbsDomain.Name14 = decimal.Round(TransUtil.ToDecimal(gwbsDomain.Name6) * 10000 / buildingArea, 4);
            //    }
            //}

            #endregion

            return list_gwbs;
        }

        #endregion

        #region 安装商务报表

        /// <summary>
        /// 安装商务报表的信息
        /// </summary>
        /// <param name="accountTaskGUID">核算节点</param>
        /// <returns></returns>
        public Hashtable GetSpecialBusinessData(int kjn, int kjy, string projectId, string accountTaskGUID)
        {
            Hashtable ht = new Hashtable();
            DateTime endDate = System.DateTime.Now;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            #region 1:取会计期的结束日期

            string sql = @"select t1.enddate from resfiscalperioddet t1 where t1.fiscalyear=" + kjn +
                         " and t1.fiscalmonth= " + kjy;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    endDate = TransUtil.ToDateTime(dataRow["enddate"]).AddDays(1);
                }
            }

            #endregion

            #region 合同签定变更情况

            IList list_order = new ArrayList();
            sql = @"select t2.contractgrouptype,t2.code,t2.contractname,sum(t1.plantotalprice) plantotalprice
                      from THD_GWBSDETAILLEDGER t1,THD_CONTRACTGROUP t2 where t1.theprojectguid='" +
                  projectId + @"'
                      and t1.contractgroupid=t2.id and t1.createtime<to_date('" +
                  endDate.ToShortDateString() + @"','yyyy-mm-dd') 
                      and t1.theprojecttasksyscode like '%" +
                  accountTaskGUID + @".%' group by t2.contractgrouptype,t2.code,t2.contractname
                      order by t2.contractgrouptype,t2.code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                string groupStr = "";
                decimal sumPlanMoney = 0;
                int count = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    count++;
                    DataDomain order_domain = new DataDomain();
                    string tempStr = "";
                    order_domain.Name1 = TransUtil.ToString(dataRow["contractgrouptype"]);
                    order_domain.Name2 = TransUtil.ToString(dataRow["code"]);
                    order_domain.Name3 = TransUtil.ToString(dataRow["contractname"]);
                    order_domain.Name4 = TransUtil.ToString(dataRow["plantotalprice"]);
                    sumPlanMoney += TransUtil.ToDecimal(order_domain.Name4);

                    //tempStr = order_domain.Name1 + "_" + order_domain.Name2;
                    //if (groupStr == "")
                    //{
                    //    groupStr = order_domain.Name1 + "_" + order_domain.Name2;
                    //}
                    //else {
                    //    if (groupStr != tempStr || count == dataTable.Rows.Count)//增加一行小计
                    //    {
                    //        DataDomain t_domain = new DataDomain();
                    //        t_domain.Name1 = "小计：";
                    //        t_domain.Name4 = sumPlanMoney + "";
                    //        groupStr = order_domain.Name1 + "_" + order_domain.Name2;
                    //        sumPlanMoney = 0;
                    //        if (count == dataTable.Rows.Count)//最后一条记录
                    //        {
                    //            list_order.Add(order_domain);
                    //        }
                    //        list_order.Add(t_domain);
                    //    }
                    //}
                    //if (count < dataTable.Rows.Count)
                    //{
                    //    list_order.Add(order_domain);
                    //}
                    list_order.Add(order_domain);
                }
                DataDomain t_domain = new DataDomain();
                t_domain.Name1 = "合计：";
                t_domain.Name4 = sumPlanMoney + "";
                list_order.Add(t_domain);
            }
            ht.Add("1", list_order);

            #endregion

            #region 项目责任成本消耗指标情况

            DataDomain indicator_domain = new DataDomain();
            sql = @"select t3.sumrealconsumetotalprice,t3.sumincometotalprice,t3.costsubjectcode
                       from thd_costmonthaccount t1,thd_costmonthaccountdtl t2,thd_costmonthaccdtlconsume t3
                       where t1.id=t2.parentid and t2.id=t3.parentid and t1.kjn=" + kjn +
                  @" and t1.kjy=" + kjy + @" and t1.theprojectguid='" + projectId + @"'
                       and t2.accounttasknodesyscode like '%" +
                  accountTaskGUID + @".%'";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string subjectCode = TransUtil.ToString(dataRow["costsubjectcode"]);
                    if (subjectCode.StartsWith("C51101"))
                    {
                        indicator_domain.Name1 = (TransUtil.ToDecimal(indicator_domain.Name1) +
                                                  TransUtil.ToDecimal(dataRow["sumincometotalprice"])) + "";
                        //累计（合同收入C51101）
                        indicator_domain.Name2 = (TransUtil.ToDecimal(indicator_domain.Name2) +
                                                  TransUtil.ToDecimal(dataRow["sumrealconsumetotalprice"])) + "";
                        //累计（劳务实际成本C51101）
                    }
                    if (subjectCode.StartsWith("C513"))
                    {
                        indicator_domain.Name3 = (TransUtil.ToDecimal(indicator_domain.Name3) +
                                                  TransUtil.ToDecimal(dataRow["sumrealconsumetotalprice"])) + "";
                        //累计（实际成本C513）
                    }
                    //buildingArea = TransUtil.ToDecimal(dataRow["buildingarea"]);
                }
            }
            ht.Add("2", indicator_domain);

            #endregion

            return ht;
        }

        #endregion

        #region 废弃保留备份算法

        private void CalRealPercent()
        {
            //#region 4.3.6 确定形象进度和过程量
            //decimal contractPercent = 0;//合同形象进度
            //decimal expensePercent = 0;//费用形象进度
            //if (costSubject.Code.IndexOf("C513") != -1 || costSubject.Code == "C5120803" || costSubject.Code == "C5120902")
            //{//[现场管理费][支架材料费][脚手架材料费]
            //    decimal sumResIncomeTotalPrice = 0;//资源合同收入合价小计
            //    decimal sumCurrRealIncomeTotalPrice = 0;//当期实现合同收入合价小计
            //    foreach (string month_dtConsumelinkStr in ht_monthdtlconsume.Keys)
            //    {
            //        CostMonthAccDtlConsume dtlConsume = (CostMonthAccDtlConsume)ht_monthdtlconsume[month_dtConsumelinkStr];
            //        string monthSyscode = this.GetGWBSSyscodeFromLinkStr(month_dtConsumelinkStr, 1);
            //        string monthSubjectCode = dtlConsume.CostSubjectCode;
            //        if (monthSubjectCode == null || monthSubjectCode == "")
            //        {
            //            monthSubjectCode = TransUtil.ToString(ht_subject[dtlConsume.CostingSubjectGUID.Id]);
            //        }
            //        if (monthSyscode.Contains(currSyscode))
            //        {
            //            if (costSubject.Code.IndexOf("C513") != -1)
            //            {
            //                if (TransUtil.ToString(monthSubjectCode).IndexOf("C513") == -1 && TransUtil.ToString(monthSubjectCode).IndexOf("C514") == -1
            //                    && TransUtil.ToString(monthSubjectCode).IndexOf("C515") == -1)
            //                {
            //                    sumCurrRealIncomeTotalPrice += dtlConsume.CurrIncomeTotalPrice;
            //                }
            //            }
            //            else if (costSubject.Code == "C5120803")
            //            {
            //                if (monthSubjectCode == "C5120803")
            //                {
            //                    sumCurrRealIncomeTotalPrice += dtlConsume.CurrRealConsumeQuantity;
            //                }
            //            }
            //            else if (costSubject.Code == "C5120902")
            //            {
            //                if (monthSubjectCode == "C5120902")
            //                {
            //                    sumCurrRealIncomeTotalPrice += dtlConsume.CurrRealConsumeQuantity;
            //                }
            //            }
            //        }
            //    }
            //    foreach (GWBSTree gwbs in list_GWBS)
            //    {
            //        if (gwbs.SysCode.Contains(currSyscode))
            //        {
            //            foreach (GWBSDetail dtl in gwbs.Details)
            //            {
            //                foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
            //                {
            //                    if (costSubject.Code.IndexOf("C513") != -1)
            //                    {
            //                        if (subject.CostAccountSubjectGUID != null &&
            //                            TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf("C513") == -1
            //                            && TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf("C514") == -1
            //                            && TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf("C515") == -1)
            //                        {
            //                            sumResIncomeTotalPrice += subject.ContractTotalPrice;
            //                        }
            //                    }
            //                    else if (costSubject.Code == "C5120803")
            //                    {
            //                        if (subject.CostAccountSubjectGUID.Code == "C5120803")
            //                        {
            //                            sumCurrRealIncomeTotalPrice += subject.PlanWorkAmount;
            //                        }
            //                    }
            //                    else if (costSubject.Code == "C5120902")
            //                    {
            //                        if (subject.CostAccountSubjectGUID != null && subject.CostAccountSubjectGUID.Code == "C5120902")
            //                        {
            //                            sumCurrRealIncomeTotalPrice += subject.PlanWorkAmount;
            //                        }
            //                    }
            //                }
            //            }

            //        }
            //    }
            //    if (costSubject.Code.IndexOf("C513") != -1)
            //    {
            //        if (sumResIncomeTotalPrice != 0)
            //        {
            //            contractPercent = decimal.Round(sumCurrRealIncomeTotalPrice / sumResIncomeTotalPrice, 4);
            //        }
            //        expensePercent = decimal.Round(costBill.EndTime.Subtract(costBill.BeginTime).Days / projectInfo.EndDate.Subtract(projectInfo.BeginDate).Days, 4);
            //    }
            //    if (costSubject.Code == "C5120803" || costSubject.Code == "C5120902")
            //    {
            //        if (sumResIncomeTotalPrice != 0)
            //        {
            //            expensePercent = decimal.Round(sumCurrRealIncomeTotalPrice / sumResIncomeTotalPrice, 4);
            //        }
            //    }

            //}
            //else if (costSubject.Code.IndexOf("C514") != -1)//规费
            //{
            //    expensePercent = 1;
            //}
            //else if (costSubject.Code.IndexOf("C51103") != -1 || costSubject.Code == "C5120302")//[施工机械费][安全施工材料费]
            //{
            //    decimal planConsumeTotalPrice = 0;//计划耗用合价小计
            //    foreach (GWBSTree gwbs in list_GWBS)
            //    {
            //        if (gwbs.SysCode.Contains(currSyscode))
            //        {
            //            foreach (GWBSDetail dtl in gwbs.Details)
            //            {
            //                foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
            //                {
            //                    if (costSubject.Code.IndexOf("C51103") != -1)
            //                    {
            //                        if (subject.CostAccountSubjectGUID != null && TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf("C51103") != -1)
            //                        {
            //                            planConsumeTotalPrice += subject.PlanTotalPrice;
            //                        }
            //                    }
            //                    if (costSubject.Code == "C5120302")
            //                    {
            //                        if (subject.CostAccountSubjectGUID != null && subject.CostAccountSubjectGUID.Code == "C5120302")
            //                        {
            //                            planConsumeTotalPrice += subject.PlanTotalPrice;
            //                        }
            //                    }
            //                }
            //            }

            //        }
            //    }
            //    if (planConsumeTotalPrice != 0)
            //    {
            //        expensePercent = decimal.Round(sumConsumeMoney / planConsumeTotalPrice, 4);
            //    }
            //}

            ////确定过程量
            //if (costSubject.Code.IndexOf("C515") != -1)//税金
            //{
            //    shareRatioType = "当期实际耗用量";
            //}
            //if (costSubject.Code.IndexOf("C513") != -1 || costSubject.Code.IndexOf("C514") != -1 || costSubject.Code.IndexOf("C51103") != -1
            //      || costSubject.Code == "C5120803" || costSubject.Code == "C5120902" || costSubject.Code == "C5120302")
            //{
            //    shareRatioType = "当期实际耗用计划量";

            //    foreach (CostMonthAccDtlConsume dtlConsume in list_curr_monthdtlconsume)
            //    {
            //        foreach (GWBSTree gwbs in list_GWBS)
            //        {
            //            if (gwbs.SysCode.Contains(currSyscode))
            //            {
            //                foreach (GWBSDetail dtl in gwbs.Details)
            //                {
            //                    foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
            //                    {
            //                        dtlConsume.CurrRealConsumePlanQuantity = subject.PlanWorkAmount * expensePercent;
            //                        dtlConsume.CurrRealConsumePlanTotalPrice = subject.PlanTotalPrice * expensePercent;
            //                        dtlConsume.CurrResponsiConsumeQuantity = subject.ResponsibilitilyWorkAmount * expensePercent;
            //                        dtlConsume.CurrResponsiConsumeTotalPrice = subject.ResponsibilitilyTotalPrice * expensePercent;
            //                        if (costSubject.Code.IndexOf("C513") != -1)//现场管理费
            //                        {
            //                            if (subject.CostAccountSubjectGUID != null && TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf("C513") == -1
            //                                && TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf("C514") == -1
            //                                && TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf("C515") == -1)
            //                            {
            //                                //dtlConsume.CurrIncomeQuantity = subject.ContractProjectAmount * contractPercent;
            //                                //dtlConsume.CurrIncomeTotalPrice = subject.ContractTotalPrice * contractPercent;

            //                            }
            //                        }
            //                        else
            //                        {
            //                            if (subject.CostAccountSubjectGUID != null)
            //                            {
            //                                if ((subject.CostAccountSubjectGUID != null && costSubject.Code.IndexOf("C513") != -1
            //                                    && costSubject.Code.IndexOf("C514") != -1)
            //                                    || (costSubject.Code.IndexOf("C51103") != -1 && TransUtil.ToString(subject.CostAccountSubjectGUID.Code).IndexOf("C51103") != -1)
            //                                    || (costSubject.Code == "C5120803" && subject.CostAccountSubjectGUID.Code == "C5120803")
            //                                    || (costSubject.Code == "C5120902" && subject.CostAccountSubjectGUID.Code == "C5120902")
            //                                    || (costSubject.Code == "C5120302" && subject.CostAccountSubjectGUID.Code == "C5120302"))
            //                                {
            //                                    //dtlConsume.CurrIncomeQuantity = subject.ContractProjectAmount * expensePercent;
            //                                    //dtlConsume.CurrIncomeTotalPrice = subject.ContractTotalPrice * expensePercent;
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //#endregion
        }

        #endregion

        #region 成本报表配置

        [TransManager]
        public IList SaveCostReporterConfig(IList listBill, IList listMoveBill)
        {
            IList list = new ArrayList();
            try
            {
                if (listBill != null && listBill.Count > 0)
                {
                    // SaveByDao(listBill);
                    Dao.SaveOrUpdate(listBill);
                    //list = SaveOrUpdateByDao(listBill);

                }
                if (listMoveBill != null && listMoveBill.Count > 0)
                {
                    Dao.Delete(listMoveBill);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public IList QueryCostReporterConfig(string sProjectID, string sType)
        {
            ObjectQuery oObjectQuery = new ObjectQuery();
            oObjectQuery.AddCriterion(Expression.Eq("CategoryType", sType));
            oObjectQuery.AddCriterion(Expression.Eq("Project.Id", sProjectID));
            oObjectQuery.AddOrder(Order.Asc("OrderNo"));
            oObjectQuery.AddFetchMode("Project", NHibernate.FetchMode.Eager);

            //oObjectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);

            IList list = Dao.ObjectQuery(typeof (CostReporterConfig), oObjectQuery);
            return list;
        }

        public IList QueryCostReporterConfig(string sType)
        {
            ObjectQuery oObjectQuery = new ObjectQuery();
            oObjectQuery.AddCriterion(Expression.Eq("CategoryType", sType));
            // oObjectQuery.AddCriterion(Expression.Eq("Project.Id", sProjectID));
            oObjectQuery.AddOrder(Order.Asc("OrderNo"));
            oObjectQuery.AddFetchMode("Project", NHibernate.FetchMode.Eager);

            //oObjectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);

            IList list = Dao.ObjectQuery(typeof (CostReporterConfig), oObjectQuery);
            return list;
        }

        public DataTable GetConfigSetProjectInfo()
        {
            DataTable oTable = null;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText =
                "select distinct nvl(t.projectid,'') projectid,nvl(t.projectname,'')projectname from thd_costreporterconfig t ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null && ds.Tables.Count > 0)
            {
                oTable = ds.Tables[0];
            }
            return oTable;
        }

        public Hashtable GetConfigSet(string sProjectID)
        {
            Hashtable oHashTable = new Hashtable();
            string sCateTypeName = "资源分类";
            string sCheckTypeName = "核算科目";
            IList list = QueryCostReporterConfig(sProjectID, sCateTypeName);
            oHashTable.Add(sCateTypeName, list);
            list = QueryCostReporterConfig(sProjectID, sCheckTypeName);
            oHashTable.Add(sCheckTypeName, list);
            return oHashTable;

        }

        #endregion

        #region 商务台账

        public IList QuerySubcontractsLedger(string condition, int type)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            IList list = new ArrayList();
            String sql = null;
            if (type == 3)
            {
                sql = "select t3.CONTRACTNAME,t1.contractnumber,t1.contractname as mcontractname,t2.bearerorgname ,t3.changemoney,t1.BEARRANGE,t2.createtime,t1.CONTRACTDESC from THD_CONTRACTGROUP t1 inner join thd_subcontractchangeitem t3"
                      +
                      " on t1.id=t3.contractgroupid inner join THD_SUBCONTRACTPROJECT t2 on t2.id=t3.TheProject where 1=1 " +
                      condition + " order by t1.SINGDATE desc";
            }
            else if (type == 2)
            {
                sql = "select CONTRACTNAME,CONTRACTNUMBER,BEARERORGNAME,CONTRACTINTERIMMONEY,BEARRANGE,SINGDATE,CONTRACTDESC from THD_CONTRACTGROUP t1 left join THD_SUBCONTRACTPROJECT t2"
                      + " on t1.ID = t2.CONTRACTGROUPID where 1=1 " + condition + " order by t1.SINGDATE desc";
            }
            else
            {
                sql =
                    "select CONTRACTNAME,CODE,BEARERORGNAME,CREATEDATE,OTHERDESC from THD_DISCLOSUREMASTER t1 join THD_DISCLOSUREDETAIL t2 on t1.ID = t2.PARENTID where 1=1 " +
                    condition + " order by t1.CREATEDATE desc";
            }


            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                if (type == 3)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        DataDomain dd = new DataDomain();
                        dd.Name1 = dataRow["CONTRACTNAME"];
                        dd.Name2 = dataRow["contractnumber"];
                        dd.Name3 = dataRow["mcontractname"];
                        dd.Name4 = dataRow["CHANGEMONEY"];
                        dd.Name5 = dataRow["BEARRANGE"];
                        dd.Name6 = dataRow["CREATETIME"];
                        dd.Name8 = dataRow["CONTRACTDESC"];
                        dd.Name9 = dataRow["bearerorgname"];
                        list.Add(dd);
                    }
                }
                else if (type == 2)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        DataDomain dd = new DataDomain();
                        dd.Name1 = dataRow["CONTRACTNAME"];
                        dd.Name2 = dataRow["CONTRACTNUMBER"];
                        dd.Name3 = dataRow["BEARERORGNAME"];
                        dd.Name4 = dataRow["CONTRACTINTERIMMONEY"];
                        dd.Name5 = dataRow["BEARRANGE"];
                        dd.Name6 = dataRow["SINGDATE"];
                        dd.Name8 = dataRow["CONTRACTDESC"];
                        list.Add(dd);
                    }
                }
                else
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        DataDomain dd = new DataDomain();
                        dd.Name1 = dataRow["CONTRACTNAME"];
                        dd.Name2 = dataRow["CODE"];
                        dd.Name3 = dataRow["BEARERORGNAME"];
                        dd.Name4 = dataRow["CREATEDATE"];
                        dd.Name5 = dataRow["OTHERDESC"];
                        list.Add(dd);
                    }
                }

            }
            return list;
        }

        [TransManager]
        public DisclosureMaster SaveContractDisclosure(DisclosureMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof (DisclosureMaster));
                obj.RealOperationDate = DateTime.Now;
            }

            Dao.SaveOrUpdate(obj);
            return obj;
        }

        public IList Query(Type type, ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(type, oQuery);
        }

        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }

        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }

        public DisclosureMaster GetContractDisclosureById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = GetContractDisclosure(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as DisclosureMaster;
            }
            return null;
        }

        public IList GetContractDisclosure(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof (DisclosureMaster), objectQuery);
        }

        public CommercialReportMaster GetCommercialMaster(string projctId, int year, int month, string type)
        {

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projctId));
            objectQuery.AddCriterion(Expression.Eq("CreateYear", year));
            objectQuery.AddCriterion(Expression.Eq("CreateMonth", month));

            if (type == "成本检查指标统计表")
            {
                objectQuery.AddFetchMode("CciDtl", NHibernate.FetchMode.Eager);
            }
            if (type == "局成本报表")
            {
                objectQuery.AddFetchMode("BcDtl", NHibernate.FetchMode.Eager);
            }

            #region   专业分包和劳务分包的区分标记，专业分包为0，劳务分包为1

            if ((type == "专业分包指标统计表") || (type == "劳务分包指标统计表"))
            {

                objectQuery.AddFetchMode("BiDtl", NHibernate.FetchMode.Eager);

            }

            #endregion

            if (type == "分包争议统计表")
            {
                objectQuery.AddFetchMode("DtDtl", NHibernate.FetchMode.Eager);
            }
            if (type == "分包终结报审统计表")
            {
                objectQuery.AddFetchMode("SaDtl", NHibernate.FetchMode.Eager);
            }
            if (type == "公司结算进展月报")
            {
                objectQuery.AddFetchMode("SprDtl", NHibernate.FetchMode.Eager);
            }

            #region

            if (type == "签证索赔情况表")
            {
                objectQuery.AddFetchMode("VcDtl", NHibernate.FetchMode.Eager);
            }

            #endregion



            IList list = Dao.ObjectQuery(typeof (CommercialReportMaster), objectQuery);

            //#region  20160825 为子表增加查询条件而添加
            CommercialReportMaster o;
            IList<BearerIndicatorDtl> lstTemp = null;
            IList<BearerIndicatorDtl> lstTempLW = null;
            string sFlag = "1";

            if (list != null && list.Count > 0)
            {
                //#region  20160825 为子表增加查询条件而添加

                //if ((type == "专业分包指标统计表") || (type == "劳务分包指标统计表"))
                //{

                //    if (type == "专业分包指标统计表")
                //    {
                //        sFlag = "0";
                //        //专业分包为0,劳务分包为1

                //    }
                //    if (type == "劳务分包指标统计表")
                //    {
                //        sFlag = "1";
                //    }

                //    CommercialReportMaster oCommercialReportMaster = list[0] as CommercialReportMaster;
                //    lstTemp = oCommercialReportMaster.BiDtl.OfType<BearerIndicatorDtl>().Where(a => a.FLAG == sFlag).ToList();
                //    oCommercialReportMaster.BiDtl.Clear();
                //    oCommercialReportMaster.BiDtl.AddAll(lstTemp);

                //    lstTempLW = oCommercialReportMaster.BlwDtl.OfType<BearerIndicatorDtl>().Where(a => a.FLAG == sFlag).ToList();
                //    oCommercialReportMaster.BlwDtl.Clear();//20160830 add  
                //    oCommercialReportMaster.BlwDtl.AddAll(lstTempLW);//20160830 add 

                //    return oCommercialReportMaster;
                //}

                //#endregion

                //else
                //{

                return list[0] as CommercialReportMaster;
                //}


            }
            return null;
        }

        #endregion

        public void SaveMasterAndDetail(CommercialReportMaster obj)
        {
            if (obj.Id == null)
            {
                //obj.Code = GetCode(typeof(CommercialReportMaster));
                obj.RealOperationDate = DateTime.Now;
            }

            Dao.SaveOrUpdate(obj);
        }

        public IList QueryCommercialReport(int year, int month, string type, string sOrgSysCode, string sProjectId)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreateYear", year));
            objectQuery.AddCriterion(Expression.Eq("CreateMonth", month));
            objectQuery.AddOrder(Order.Asc("ProjectName"));
            if (type == "成本检查指标统计表")
            {
                objectQuery.AddFetchMode("CciDtl", NHibernate.FetchMode.Eager);
            }
            if (type == "局成本报表")
            {
                objectQuery.AddFetchMode("BcDtl", NHibernate.FetchMode.Eager);
            }


            //if ((type == "专业分包指标统计表") || (type == "劳务分包指标统计表"))
            //{
            //    objectQuery.AddFetchMode("BiDtl", NHibernate.FetchMode.Eager);
            //}
            if (type == "分包指标统计表")
            {
                objectQuery.AddFetchMode("BiDtl", NHibernate.FetchMode.Eager);
            }

            if (type == "分包争议统计表")
            {
                objectQuery.AddFetchMode("DtDtl", NHibernate.FetchMode.Eager);
            }
            if (type == "分包终结报审统计表")
            {
                objectQuery.AddFetchMode("SaDtl", NHibernate.FetchMode.Eager);
            }
            if (type == "公司结算进展月报")
            {
                objectQuery.AddFetchMode("SprDtl", NHibernate.FetchMode.Eager);
            }

            #region

            if (type == "签证索赔情况表") //20160829
            {
                objectQuery.AddFetchMode("VcDtl", NHibernate.FetchMode.Eager);
            }

            #endregion



            if (string.IsNullOrEmpty(sProjectId))
            {
                objectQuery.AddCriterion(Expression.Like("OpgSysCode", "%" + sOrgSysCode + "%"));
            }
            else
            {
                objectQuery.AddCriterion(Expression.Eq("ProjectId", sProjectId));
            }

            IList list = Dao.ObjectQuery(typeof (CommercialReportMaster), objectQuery);
            if (list != null && list.Count > 0)
            {

                return list;
            }
            return null;
        }

        public IList QueryCostCompareReportData(string sProject, string sGwbsTreeID, int iYear, int iMonth)
        {
            IList lstResult = new ArrayList();
            string sCostMonthSQL =
                @"select tt.parentnodeid parentid, tt.syscode,tt.id, tt.name gwbsTreeName, tt.tlevel,tt.OrderNo treeOrderNo, tt1.*  from thd_gwbstree tt
left join (select t1.accounttasknodeguid ,t1.projecttaskdtlguid dtlid,t1.projecttaskdtlname dtlName, t2.id consumeID, t2.resourcetypeguid,t2.costingsubjectguid, 
t1.dudgetcontractquantity,t4.contractprice,t1.dudgetrespquantity,t4.responsibilitilyprice,t1.dudgetplanquantity, 
t2.currrealconsumetotalprice,(case when instr(t3.matcode,'I')>0 then t2.currrealconsumetotalprice else 0 end   ) currmaterialTotalPrice,
t2.currresponsiconsumequantity,t2.currresponsiconsumetotalprice,t2.currincomequantity,t2.currincometotalprice,
t2.sumrealconsumequantity,t2.sumrealconsumetotalprice,(case when instr(t3.matcode,'I')>0 then t2.sumrealconsumetotalprice else 0 end   ) summaterialTotalPrice,
t2.sumresponsiconsumequantity,t2.sumresponsiconsumetotalprice ,t2.sumincomequantity,t2.sumincometotalprice,t4.OrderNo detailOrderNo
from thd_costmonthaccount t
join thd_costmonthaccountdtl t1 on t.id=t1.parentid and instr(t1.accounttasknodesyscode,:sGwbsTreeID)>0
join thd_costmonthaccdtlconsume t2 on t1.id=t2.parentid
join resmaterial t3 on t2.resourcetypeguid=t3.materialid
left join thd_gwbsdetail t4 on t4.id=t1.projecttaskdtlguid  and t4.theprojectguid=:sProject
where t.kjn=:iYear and t.kjy=:iMonth and t.theprojectguid=:sProject)tt1 on   tt.id=tt1.accounttasknodeguid
where tt.theprojectguid=:sProject and instr(tt.syscode,:sGwbsTreeID)>0 order by tt.tlevel asc";

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sCostMonthSQL;
            oCommand.Parameters.Clear();
            IDbDataParameter oDataParameter = oCommand.CreateParameter();
            oDataParameter.ParameterName = "sProject";
            oDataParameter.Value = sProject;
            oCommand.Parameters.Add(oDataParameter);
            oDataParameter = oCommand.CreateParameter();
            oDataParameter.ParameterName = "sGwbsTreeID";
            oDataParameter.Value = sGwbsTreeID;
            oCommand.Parameters.Add(oDataParameter);
            oDataParameter = oCommand.CreateParameter();
            oDataParameter.ParameterName = "iYear";
            oDataParameter.Value = iYear;
            oCommand.Parameters.Add(oDataParameter);
            oDataParameter = oCommand.CreateParameter();
            oDataParameter.ParameterName = "iMonth";
            oDataParameter.Value = iMonth;
            oCommand.Parameters.Add(oDataParameter);
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                lstResult.Insert(0, ds);
                string sSubContractSQL =
                    @"select t2.balancetaskguid gwbtreeid,t2.balancetaskdtlguid dtlid,t2.balancetasksyscode syscode,t3.resourcetypeguid,t3.balancesubjectguid,
sum(case when (t.kjn=:iYear and t.kjy=:iMonth and t2.fontbilltype=1) then  t3.balancetotalprice else 0 end ) curTotalPrice,
sum(case when (t.kjn=:iYear and t.kjy=:iMonth and t2.fontbilltype=1) then  t3.balancequantity else 0 end ) curTotalQty,
sum(case when (t.kjn=:iYear and t.kjy=:iMonth and t2.fontbilltype!=1) then t3.balancetotalprice else 0 end ) curOtherTotalPrice,
sum(case when t2.fontbilltype=1 then t3.balancetotalprice else 0 end) sumTotalPrice,
sum(case when t2.fontbilltype=1 then t3.balancequantity else 0 end ) sumQty,
sum(case when t2.fontbilltype!=1 then t3.balancetotalprice else 0 end) sumOtherTotalPrice
from thd_costmonthaccount t
join thd_subcontractbalancebill t1 on t.id=t1.monthaccbillid and t1.projectid=:sProject
join thd_subcontractbalancedetail t2 on t1.id=t2.parentid and instr(t2.balancetasksyscode,:sGwbsTreeID)>0
join thd_subcontractbalsubjectdtl t3 on t2.id=t3.parentid
where( t.kjn<:iYear or(t.kjn=:iYear and t.kjy<=:iMonth)) and t.theprojectguid=:sProject
group by  t2.balancetaskguid,t2.balancetaskdtlguid,t2.balancetasksyscode,t3.resourcetypeguid,t3.balancesubjectguid";
                oCommand = oConn.CreateCommand();
                oCommand.CommandText = sSubContractSQL;
                oCommand.Parameters.Clear();
                oDataParameter = oCommand.CreateParameter();
                oDataParameter.ParameterName = "sProject";
                oDataParameter.Value = sProject;
                oCommand.Parameters.Add(oDataParameter);
                oDataParameter = oCommand.CreateParameter();
                oDataParameter.ParameterName = "sGwbsTreeID";
                oDataParameter.Value = sGwbsTreeID;
                oCommand.Parameters.Add(oDataParameter);
                oDataParameter = oCommand.CreateParameter();
                oDataParameter.ParameterName = "iYear";
                oDataParameter.Value = iYear;
                oCommand.Parameters.Add(oDataParameter);
                oDataParameter = oCommand.CreateParameter();
                oDataParameter.ParameterName = "iMonth";
                oDataParameter.Value = iMonth;
                oCommand.Parameters.Add(oDataParameter);
                oRead = oCommand.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lstResult.Insert(1, ds);
                }
            }
            return lstResult;
        }

        public List<CostMonthAccDtlConsume> GetRealCostConsume(string projId, List<string> nodes, int year, int month)
        {
            string sql = @"
            select sum(c.CURRREALCONSUMEQUANTITY) as totalRealQUANTITY,
                   sum(c.CURRREALCONSUMETOTALPRICE) as TOTALRealPRICE,
                   sum(c.CURRINCOMEQUANTITY) as totalContractQUANTITY,
                   sum(c.CURRINCOMETOTALPRICE) as TOTALContractPRICE,
                   sum(case when to_date(a.kjn || '-' || a.kjy || '-01','yyyy-mm-dd') = to_date('{1}','yyyy-mm-dd') then c.SUMRESPONSICONSUMEQUANTITY else 0 end) as totalRESPONSQUANTITY,
                   sum(case when to_date(a.kjn || '-' || a.kjy || '-01','yyyy-mm-dd') = to_date('{1}','yyyy-mm-dd') then c.SUMRESPONSICONSUMETOTALPRICE else 0 end) as TOTALRESPONSPRICE,
                   sum(case when to_date(a.kjn || '-' || a.kjy || '-01','yyyy-mm-dd') = to_date('{1}','yyyy-mm-dd') then c.sumincomequantity else 0 end) as TOTALincomequantity,
                   sum(case when to_date(a.kjn || '-' || a.kjy || '-01','yyyy-mm-dd') = to_date('{1}','yyyy-mm-dd') then c.sumincometotalprice else 0 end) as TOTALincometotalprice,
                   nvl(c.costsubjectcode,'') as costsubjectcode,
                   nvl(f.syscode,'') as costsubjectsyscode,
                   nvl(c.resourcetypeguid,'') as resourcetypeguid,
                   nvl(d.matname,'') as matname,
                   nvl(d.matspecification,'') as matspecification,
                   nvl(e.standunitname,'') as standunitname,
                   case when c.sourcetype = '{3}' then 1 else 0 end as isTime
            from thd_costmonthaccount a
            join thd_costmonthaccountdtl b on b.parentid = a.id
            join thd_costmonthaccdtlconsume c on c.parentid = b.id
            left join resmaterial d on d.materialid = c.resourcetypeguid
            left join RESSTANDUNIT e on e.standunitid = c.rationunitguid
            left join thd_costaccountsubject f on c.costsubjectcode = f.code
            where a.theprojectguid = '{0}' 
                  and to_date(a.kjn || '-' || a.kjy || '-01','yyyy-mm-dd') <= to_date('{1}','yyyy-mm-dd')
                  {2}
            group by nvl(c.costsubjectcode,''),
                  nvl(f.syscode,''),
                  nvl(c.resourcetypeguid,''),
                  nvl(d.matname,''),
                  nvl(d.matspecification,''),
                  nvl(e.standunitname,''),
                  case when c.sourcetype = '{3}' then 1 else 0 end";

            var condition = new StringBuilder();
            //condition.AppendLine("and (");
            //foreach (var node in nodes)
            //{
            //    condition.AppendLine(
            //        string.Format("nvl(b.accounttasknodesyscode,c.accounttasknodesyscode) like '{0}%' or ", node));
            //}
            //condition.AppendLine(" 1=2)");

            var ds = QueryDataToDataSet(
                string.Format(sql, projId, 
                new DateTime(year, month, 1).ToString("yyyy-MM-dd"), 
                condition,
                typeof(LaborSporadicDetail).FullName));
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var list = new List<CostMonthAccDtlConsume>();
            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var item = new CostMonthAccDtlConsume();
                item.CostSubjectCode = ds.Tables[0].Rows[i]["costsubjectcode"].ToString();
                item.CostSubjectSyscode = TransUtil.ToString(ds.Tables[0].Rows[i]["costsubjectsyscode"]);
                item.ResourceTypeGUID = ds.Tables[0].Rows[i]["resourcetypeguid"].ToString();
                item.ResourceTypeName = ds.Tables[0].Rows[i]["matname"].ToString();
                item.ResourceTypeSpec = ds.Tables[0].Rows[i]["matspecification"].ToString();
                item.RationUnitName = ds.Tables[0].Rows[i]["standunitname"].ToString();
                item.SumRealConsumeQuantity = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["totalRealQUANTITY"]);
                item.SumRealConsumeTotalPrice = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["TOTALRealPRICE"]);
                item.SumResponsiConsumeQuantity = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["totalRESPONSQUANTITY"]);
                item.SumResponsiConsumeTotalPrice = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["TOTALRESPONSPRICE"]);
                item.Data1 = TransUtil.ToString(ds.Tables[0].Rows[i]["isTime"]);
                item.CurrIncomeQuantity = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["totalContractQUANTITY"]);
                item.CurrIncomeTotalPrice = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["TOTALContractPRICE"]);
                item.SumIncomeQuantity = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["TOTALincomequantity"]);
                item.SumIncomeTotalPrice = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["TOTALincometotalprice"]);
                item.Data3 = TransUtil.ToString(ds.Tables[0].Rows[i]["costsubjectsyscode"]);
                
                list.Add(item);
            }

            return list;
        }

        #region 取费模板
        [TransManager]
        public SelFeeTemplateMaster SaveOrUpdateSelFeeTemplateMaster(SelFeeTemplateMaster master)
        {
            if (string.IsNullOrEmpty(master.Id))
            {
                master.Code = GetCode(typeof(SelFeeTemplateMaster));
                master.RealOperationDate = DateTime.Now;
            }
            Dao.SaveOrUpdate(master);
            return master;
        }
        [TransManager]
        public bool DeleteSelFeeTemplateMaster(SelFeeTemplateMaster master)
        {
            return Dao.Delete(master);
        }

        public IList GetSelFeeTemplateMasterList(ObjectQuery oQuery)
        {

            oQuery.AddFetchMode("SelFeeTempFormulas", FetchMode.Eager);
            oQuery.AddFetchMode("SelFeeTemplateDetails", FetchMode.Eager);
            IList lst = QuerySelFeeTemplateMaster(oQuery);
            return lst;
        }

        public SelFeeTemplateMaster GetSelFeeTemplateMaster(ObjectQuery oQuery)
        {
            IList lst = GetSelFeeTemplateMasterList(oQuery);
            return lst == null || lst.Count == 0 ? null : (lst[0] as SelFeeTemplateMaster);
        }
        public IList QuerySelFeeTemplateMaster(ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(typeof(SelFeeTemplateMaster), oQuery);
        }

        public SelFeeTemplateMaster GetSelFeeTemplateMasterById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            oq.AddFetchMode("SelFeeTemplateDetails.MainAccSubject", FetchMode.Eager);
            oq.AddFetchMode("SelFeeTemplateDetails.AccountSubject", FetchMode.Eager);
            oq.AddFetchMode("SelFeeTempFormulas.AccountSubject", FetchMode.Eager);

            return GetSelFeeTemplateMaster(oq);
        }
        #endregion

        public SumAreaParame SaveSumAreaParame(SumAreaParame obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        public string GetDefaultPeriod(string projectid)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql =
                @"select max(kjn||'|'|| kjy ) from thd_costmonthaccount where theprojectguid = '" + projectid + "'";
            command.CommandText = sql;
            object obj =  command.ExecuteScalar();
            if (obj == null || obj == DBNull.Value)
                return "";
            else
                return ClientUtil.ToString(obj);
        }
    }
}
