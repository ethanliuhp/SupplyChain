﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using System.Collections;
using NHibernate;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;
using VirtualMachine.Component.Util;
using System.Runtime.Remoting.Messaging;
using System.Data;
using VirtualMachine.Core.DataAccess;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service
{
    public class IndirectCostSvr : IIndirectCostSvr
    {
        string[] timeStrs = { "本日", "本月", "本年", "累计", "日均", "月均", "上年", "月末", "上月" };
        string[] measurementUnitStrs = { "元", "个", "百分比" };
        string[] warntStrs = { "正常", "黄色预警", "橙色预警", "红色预警" };
        #region 公共方法
        private IDao _Dao;

        virtual public IDao Dao
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


        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }
        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }
        /// <summary>
        /// 保存新增数据
        /// </summary>
        /// <param name="obj">要保存的对象</param>
        /// <returns>保存后的对象</returns>
        [TransManager]
        virtual public Object Save(Object obj)
        {
            BaseMaster master = obj as BaseMaster;
            master.Code = GetCode(obj.GetType());
            if (master != null)
            {
                master.RealOperationDate = DateTime.Now;
            }
            Dao.Save(obj);
            return obj;
        }
        /// <summary>
        /// 保存要修改的数据
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <returns>修改后的对象</returns>
        [TransManager]
        virtual public Object Update(object obj, IList movedDtlList)
        {
            if (movedDtlList != null && movedDtlList.Count > 0)
            {
                Dao.Delete(movedDtlList);
            }
            BaseMaster master = obj as BaseMaster;
            //if (master != null)
            //{
            //    master.RealOperationDate = DateTime.Now;
            //}
            Dao.Update(obj);
            return obj;
        }
        [TransManager]
        virtual public Object Update(object obj, IList addDetail, IList movedDtlList)
        {
            if (movedDtlList != null && movedDtlList.Count > 0)
            {
                Dao.Delete(movedDtlList);
            }
            if (addDetail != null && addDetail.Count > 0)
            {
                Dao.Delete(addDetail);
            }
            BaseMaster master = obj as BaseMaster;
            //if (master != null)
            //{
            //    master.RealOperationDate = DateTime.Now;
            //}
            Dao.Update(obj);
            return obj;
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="obj">要删除的对象</param>
        /// <returns>True 删除成功，False 删除失败</returns>
        virtual public bool Delete(Object obj)
        {
            Dao.Delete(obj);
            return true;
        }

        public virtual T GetMasterByID<T>(string id) where T : class
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            oq.AddFetchMode("Details", FetchMode.Eager);
            IList lst = Dao.ObjectQuery(typeof(T), oq);
            return lst == null || lst.Count == 0 ? null : lst[0] as T;
        }


        public virtual object GetMasterByID(Type type, string id)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", id));
            oQuery.AddFetchMode("Details", FetchMode.Eager);

            IList lst = Dao.ObjectQuery(type, oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0];

        }

        public virtual List<T> Query<T>(ObjectQuery oQuery)
        {
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            var list = Dao.ObjectQuery(typeof(T), oQuery);
            var result = new List<T>();
            foreach (T item in list)
            {
                result.Add(item);
            }
            return result;
        }

        public IList QuerySubAndCompanyOrgInfo()
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = "select t1.opgid,t1.opgname,t1.opgsyscode from resoperationorg t1 where t1.opgstate=1 and " +
                                    " t1.opgoperationtype='b' ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    OperationOrgInfo orgInfo = new OperationOrgInfo();
                    orgInfo.Id = TransUtil.ToString(dataRow["opgid"]);
                    orgInfo.Name = TransUtil.ToString(dataRow["opgname"]);
                    orgInfo.SysCode = TransUtil.ToString(dataRow["opgsyscode"]);
                    list.Add(orgInfo);
                }

            }
            return list;
        }

        #endregion

        #region 借款单
        [TransManager]
        public BorrowedOrderMaster SaveBorrowedOrderMaster(BorrowedOrderMaster obj)
        {
            if (obj.Id == null)
            {
                if (obj.ProjectId == null)
                {
                    obj.Code = GetCode(typeof(BorrowedOrderMaster));
                }
                else
                {
                    obj.Code = GetCode(typeof(BorrowedOrderMaster), obj.ProjectId);
                }
            }
            obj.RealOperationDate = DateTime.Now;
            Dao.SaveOrUpdate(obj);
            return obj;
        }
        #endregion

        #region 费用信息
        public IndirectCostMaster GetIndirectCostMasterByID(string strID)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", strID));
            oQuery.AddFetchMode("Details", FetchMode.Eager);

            IList lst = Dao.ObjectQuery(typeof(IndirectCostMaster), oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as IndirectCostMaster;

        }
        public IList QueryIndirectCostMaster(ObjectQuery oQuery)
        {
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(IndirectCostMaster), oQuery);
        }
        public IList Query(Type type, ObjectQuery oQuery)
        {

            return Dao.ObjectQuery(type, oQuery);
        }
        public object Get(Type type, string sID)
        {
            return Dao.Get(type, sID);
        }
        /// <summary>
        /// 在公司和分公司情况下 根据当前所属组织和截止时间获取最近一个月的预算金额
        /// </summary>
        /// <param name="currDate"></param>
        /// <param name="syscode"></param>
        /// <returns></returns>
        public Hashtable GetNextCostBudgetMoney(string syscode)
        {
            string sKey = string.Empty;
            decimal dMoney = 0;
            Hashtable ohTable = new Hashtable();
            string sSQL = "select  nvl(tt.accounttitleid,'') as accounttitleid,nvl(tt.orginfoname,'') as orginfoname ,tt.budgetmoney as money "
                          + " from thd_indirectcostdetail tt "
                          + " join thd_indirectcostmaster tt1 on tt.parentid=tt1.id and tt1.projectid is null  "
                          + " join (select max( t.createdate) as createdate,nvl(t1.accounttitleid,'') as accounttitleid,"
                          + " nvl(t1.orginfoname,'') as orginfoname from thd_indirectcostmaster t "
                          + " join thd_indirectcostdetail t1 on t.id=t1.parentid  "
                          + " where t.createdate <=to_date('{0}','yyyy-mm-dd') and t.opgsyscode like '{1}%'  and t.projectid is null "
                          + " group by nvl(t1.accounttitleid,''),nvl(t1.orginfoname,'') ) tt2 "
                          + " on nvl(tt.accounttitleid,'')=tt2.accounttitleid and nvl(tt.orginfoname,'')=tt2.orginfoname "
                          + " and tt2.createdate=tt1.createdate "
                          + " and tt1.createdate <=to_date('{0}','yyyy-mm-dd') and tt1.opgsyscode like '{1}%' "
                          + " order by tt1.realoperationdate desc";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand command = oConn.CreateCommand();
            command.CommandText = string.Format(sSQL, DateTime.Now.ToString("yyyy-MM-dd"), syscode);
            IDataReader oReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oReader);
            foreach (DataRow oRow in ds.Tables[0].Rows)
            {
                sKey = string.Format("{0}-{1}", TransUtil.ToString(oRow["accounttitleid"]), TransUtil.ToString(oRow["orginfoname"]));
                if (!ohTable.ContainsKey(sKey))
                {
                    dMoney = TransUtil.ToDecimal(oRow["money"]);
                    ohTable.Add(sKey, dMoney);
                }
            }
            return ohTable;
        }
        #endregion

        #region 指标存储
        /// <summary>
        /// 查询公司关键指标情况
        /// </summary>
        public IList QueryCompanyIndexInfoByDate(DateTime currDate)
        {
            IList list = new ArrayList();
            DataDomain domain = new DataDomain();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.projectid,count(*) count from thd_fundmanagebyproject t1 where " +
                                    " t1.createdate=to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') group by t1.projectid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            int projectCount = 0;
            int totalCount = 0;
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    totalCount += TransUtil.ToInt(dataRow["count"]);
                    projectCount++;
                }
            }
            domain.Name1 = TransUtil.ToDateTime(currDate.ToShortDateString());
            domain.Name2 = projectCount + "";
            domain.Name3 = totalCount + "";
            if (projectCount > 0)
            {
                list.Add(domain);
            }
            return list;
        }

        [TransManager]
        public void CalulationProjectState()
        {
            IProjectStateHelper psh = new ProjectStateHelper(DateTime.Now);
            psh.Create();
        }

        [TransManager]
        public void CompanyKeyInfoService(DateTime currDate)
        {

            string[] indexStrs = { "营业收入", "收款", "借款", "保理", "资金存量", "应收账款", "应收未收款", "收现率", "资金占用", "业主审量", "保证金余额", "收保证金", "付保证金" };
            //try
            //{
                //如果已经存在则删除当日的项目和分公司指标
                this.DeleteFundManageAndProject(currDate);

                // 商务指标生成
                IIndex indexCls = new Index(currDate);
                indexCls.Create();

                IList projectFundList = new ArrayList();//项目指标集合
                IList haveTransFundList = new ArrayList();//需要转换成分公司/公司的项目指标集合
                Hashtable subCompanyIndexHt = new Hashtable();//分公司指标集合
                Hashtable companyIndexHt = new Hashtable();//公司指标集合
                //获取时间信息
                //currDate = TransUtil.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString());//当日
                int currYear = currDate.Year;
                int currMonth = currDate.Month;
                int lastYear = 0;
                int lastMonth = 0;
                if (currMonth == 1)
                {
                    lastYear = currYear - 1;
                    lastMonth = 12;
                }
                else
                {
                    lastYear = currYear;
                    lastMonth = currMonth - 1;
                }
                DateTime monthFirstDate = new DateTime(currDate.Year, currDate.Month, 1);//当月第一天
                DateTime monthLastDate = monthFirstDate.AddMonths(1).AddDays(-1);//当月最后一天
                DateTime yearFirstDate = new DateTime(currDate.Year, 1, 1);//当年第一天

                //查询分公司对应的组织信息
                Hashtable basic_ht = this.GetCurrOrgAndProjectInfo("");
                IList opgList = basic_ht["subcompanyinfo"] as ArrayList;
                Hashtable project_ht = basic_ht["projectinfo"] as Hashtable;
                string dayCondition = " and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                string monthCondition = " and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                string yearCondition = " and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                string addCondition = " and  t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";

                #region 营业收入
                //营业收入(分包工程支出+人工费+材料费+机械费+其他直接费+间接费用+合同毛利)
                //上月
                string monthFinanceCondition = " and t1.year=" + lastYear + " and t1.month=" + lastMonth;
                IList monthProductValueList = this.GetProjectProjectValueData(monthFinanceCondition, monthCondition);
                foreach (FinanceMultDataMaster master in monthProductValueList)
                {
                    CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                    FundManagementByProject funByProject = new FundManagementByProject();
                    if (projectInfo != null)
                    {
                        //项目指标
                        funByProject.CreateDate = currDate;
                        funByProject.IndexName = "营业收入";
                        funByProject.MeasurementUnitName = "元";
                        funByProject.NumericalValue = TransUtil.ToDecimal(master.Temp1);
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.OrganizationLevel = "项目";
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        funByProject.TimeName = "本月";
                        funByProject.WarningLevelName = "正常";
                        projectFundList.Add(funByProject);
                        haveTransFundList.Add(funByProject);
                    }
                }
                //本年
                string yearFinanceCondition = " and t1.year=" + currYear;
                IList yearProductValueList = this.GetProjectProjectValueData(yearFinanceCondition, yearCondition);
                foreach (FinanceMultDataMaster master in yearProductValueList)
                {
                    CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                    FundManagementByProject funByProject = new FundManagementByProject();
                    if (projectInfo != null)
                    {
                        //项目指标
                        funByProject.CreateDate = currDate;
                        funByProject.IndexName = "营业收入";
                        funByProject.MeasurementUnitName = "元";
                        funByProject.NumericalValue = TransUtil.ToDecimal(master.Temp1);
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.OrganizationLevel = "项目";
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        funByProject.TimeName = "本年";
                        funByProject.WarningLevelName = "正常";
                        projectFundList.Add(funByProject);
                        haveTransFundList.Add(funByProject);
                    }
                }
                IList addProductValueList = this.GetProjectProjectValueData(addCondition, addCondition);
                foreach (FinanceMultDataMaster master in addProductValueList)
                {
                    CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                    FundManagementByProject funByProject = new FundManagementByProject();
                    if (projectInfo != null)
                    {
                        //项目指标
                        funByProject.CreateDate = currDate;
                        funByProject.IndexName = "营业收入";
                        funByProject.MeasurementUnitName = "元";
                        funByProject.NumericalValue = TransUtil.ToDecimal(master.Temp1);
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.OrganizationLevel = "项目";
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        funByProject.TimeName = "累计";
                        funByProject.WarningLevelName = "正常";
                        projectFundList.Add(funByProject);
                    }
                }
                #endregion

                #region 收款
                //本日项目收款               
                IList dayGatheringList = this.GetProjectGatheringInfo(dayCondition + " and t1.projectid is not null and t1.ifprojectmoney = 0 ");
                foreach (GatheringMaster master in dayGatheringList)
                {
                    CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                    FundManagementByProject funByProject = new FundManagementByProject();
                    if (projectInfo != null)
                    {
                        //项目指标
                        funByProject.CreateDate = currDate;
                        funByProject.IndexName = "收款";
                        funByProject.MeasurementUnitName = "元";
                        funByProject.NumericalValue = TransUtil.ToDecimal(master.Temp1);
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.OrganizationLevel = "项目";
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        funByProject.TimeName = "本日";
                        funByProject.WarningLevelName = "正常";
                        projectFundList.Add(funByProject);
                        haveTransFundList.Add(funByProject);
                    }
                }

                //本月项目收款
                IList monthGatheringList = this.GetProjectGatheringInfo(monthCondition + " and t1.projectid is not null and t1.ifprojectmoney = 0");
                foreach (GatheringMaster master in monthGatheringList)
                {
                    CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                    FundManagementByProject funByProject = new FundManagementByProject();
                    if (projectInfo != null)
                    {
                        //项目指标
                        funByProject.CreateDate = currDate;
                        funByProject.IndexName = "收款";
                        funByProject.MeasurementUnitName = "元";
                        funByProject.NumericalValue = TransUtil.ToDecimal(master.Temp1);
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.OrganizationLevel = "项目";
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        funByProject.TimeName = "本月";
                        funByProject.WarningLevelName = "正常";
                        projectFundList.Add(funByProject);
                        haveTransFundList.Add(funByProject);
                    }
                }
                //本年项目收款
                IList yearGatheringList = this.GetProjectGatheringInfo(yearCondition + " and t1.projectid is not null and t1.ifprojectmoney = 0 ");
                foreach (GatheringMaster master in yearGatheringList)
                {
                    CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                    FundManagementByProject funByProject = new FundManagementByProject();
                    if (projectInfo != null)
                    {
                        //项目指标
                        funByProject.CreateDate = currDate;
                        funByProject.IndexName = "收款";
                        funByProject.MeasurementUnitName = "元";
                        funByProject.NumericalValue = TransUtil.ToDecimal(master.Temp1);
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.OrganizationLevel = "项目";
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        funByProject.TimeName = "本年";
                        funByProject.WarningLevelName = "正常";
                        projectFundList.Add(funByProject);
                        haveTransFundList.Add(funByProject);
                    }
                }
                //累计项目收款
                IList addGatheringList = this.GetProjectGatheringInfo(addCondition + " and t1.projectid is not null and t1.ifprojectmoney = 0 ");
                foreach (GatheringMaster master in yearGatheringList)
                {
                    CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                    FundManagementByProject funByProject = new FundManagementByProject();
                    if (projectInfo != null)
                    {
                        //项目指标
                        funByProject.CreateDate = currDate;
                        funByProject.IndexName = "收款";
                        funByProject.MeasurementUnitName = "元";
                        funByProject.NumericalValue = TransUtil.ToDecimal(master.Temp1);
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.OrganizationLevel = "项目";
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        funByProject.TimeName = "累计";
                        funByProject.WarningLevelName = "正常";
                        projectFundList.Add(funByProject);
                    }
                }
                //获取分公司的初始化收款信息
                IList yearSubGatheringList = this.GetSubGatheringInfo(yearCondition, opgList);
                decimal yearCompGMoney = 0;
                foreach (FundManagement fund in yearSubGatheringList)
                {
                    fund.TimeName = "本年";
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!subCompanyIndexHt.Contains(subStr) && fund.NumericalValue != 0 )
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                    yearCompGMoney += fund.NumericalValue;
                }
                FundManagement yearCompGFund = new FundManagement();
                yearCompGFund.CreateDate = currDate;
                yearCompGFund.OperationOrg = TransUtil.CompanyOpgGUID;
                yearCompGFund.OperOrgName = TransUtil.CompanyOpgName;
                yearCompGFund.OrganizationLevel = "公司";
                yearCompGFund.OpgSysCode = TransUtil.CompanyOpgSyscode;
                yearCompGFund.IndexName = "收款";
                yearCompGFund.MeasurementUnitName = "元";
                yearCompGFund.TimeName = "本年";
                yearCompGFund.WarningLevelName = "正常";
                yearCompGFund.NumericalValue = yearCompGMoney;
                string compStr = yearCompGFund.IndexName + "-" + yearCompGFund.TimeName + "-" + yearCompGFund.MeasurementUnitName + "-" + yearCompGFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr) && yearCompGFund.NumericalValue != 0)
                {
                    companyIndexHt.Add(compStr, yearCompGFund);
                }
                //累计公司/分公司收工程款
                IList addSubGatheringList = this.GetSubGatheringInfo(addCondition, opgList);
                decimal addCompyGMoney = 0;
                foreach (FundManagement fund in addSubGatheringList)
                {
                    fund.TimeName = "累计";
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                    addCompyGMoney += fund.NumericalValue;
                }
                FundManagement addCompGFund = new FundManagement();
                addCompGFund.CreateDate = currDate;
                addCompGFund.OperationOrg = TransUtil.CompanyOpgGUID;
                addCompGFund.OperOrgName = TransUtil.CompanyOpgName;
                addCompGFund.OrganizationLevel = "公司";
                addCompGFund.OpgSysCode = TransUtil.CompanyOpgSyscode;
                addCompGFund.IndexName = "收款";
                addCompGFund.MeasurementUnitName = "元";
                addCompGFund.TimeName = "累计";
                addCompGFund.WarningLevelName = "正常";
                addCompGFund.NumericalValue = addCompyGMoney;
                compStr = addCompGFund.IndexName + "-" + addCompGFund.TimeName + "-" + addCompGFund.MeasurementUnitName + "-" + addCompGFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr))
                {
                    companyIndexHt.Add(compStr, addCompGFund);
                }
                #endregion

                #region 借款/保理
                //到当日为止分公司借款
                IList addSubBorrowList = this.GetSubCompanyBorrowInfo(addCondition, opgList);
                foreach (FundManagement fund in addSubBorrowList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                //公司借款
                FundManagement compBorrowFund = this.GetCompanyBorrowInfo(addCondition, opgList);
                compBorrowFund.CreateDate = currDate;
                compStr = compBorrowFund.OperationOrg + "-" + compBorrowFund.IndexName + "-" + compBorrowFund.TimeName + "-" + compBorrowFund.MeasurementUnitName + "-" + compBorrowFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr))
                {
                    companyIndexHt.Add(compStr, compBorrowFund);
                }
                //公司到本日为止保理
                FundManagement compFactorFund = this.GetCompanyFactoringInfo(addCondition);
                compFactorFund.CreateDate = currDate;
                compStr = compFactorFund.OperationOrg + "-" + compFactorFund.IndexName + "-" + compFactorFund.TimeName + "-" + compFactorFund.MeasurementUnitName + "-" + compFactorFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr))
                {
                    companyIndexHt.Add(compStr, compFactorFund);
                }
                #endregion

                #region 存量/日均/月均
                //项目资金存量/占用(累计结余+借款) 预警信息
                IList addProjectLeftCashList = this.GetProjectLeftCashInfo(addCondition);
                IList addProjectLeftCashWarnList = new ArrayList();//项目资金占用预警
                foreach (FundManagementByProject funByProject in addProjectLeftCashList)
                {
                    CurrentProjectInfo projectInfo = project_ht[funByProject.ProjectId] as CurrentProjectInfo;
                    funByProject.CreateDate = currDate;
                    if (projectInfo != null)
                    {
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;

                        FundManagementByProject currFunByProject = new FundManagementByProject();
                        currFunByProject.CreateDate = currDate;
                        currFunByProject.IndexName = "资金占用";
                        currFunByProject.MeasurementUnitName = "元";
                        currFunByProject.NumericalValue = funByProject.NumericalValue;
                        currFunByProject.OperationOrg = projectInfo.Data1;
                        currFunByProject.OperOrgName = projectInfo.OwnerOrgName;
                        currFunByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        currFunByProject.OrganizationLevel = "项目";
                        currFunByProject.ProjectId = projectInfo.Id;
                        currFunByProject.ProjectName = projectInfo.Name;
                        currFunByProject.TimeName = "累计";
                        currFunByProject.WarningLevelName = "正常";
                        if (funByProject.NumericalValue < 0)
                        {
                            projectFundList.Add(currFunByProject);
                            addProjectLeftCashWarnList.Add(currFunByProject);
                        }

                        if (funByProject.NumericalValue <= -5000000)
                        {
                            FundManagementByProject currFunWarnByProject = new FundManagementByProject();
                            currFunWarnByProject.CreateDate = currDate;
                            currFunWarnByProject.IndexName = "资金占用";
                            currFunWarnByProject.MeasurementUnitName = "元";
                            currFunWarnByProject.NumericalValue = funByProject.NumericalValue;
                            currFunWarnByProject.OperationOrg = projectInfo.Data1;
                            currFunWarnByProject.OperOrgName = projectInfo.OwnerOrgName;
                            currFunWarnByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                            currFunWarnByProject.OrganizationLevel = "项目";
                            currFunWarnByProject.ProjectId = projectInfo.Id;
                            currFunWarnByProject.ProjectName = projectInfo.Name;
                            currFunWarnByProject.TimeName = "累计";
                            if (funByProject.NumericalValue <= -5000000 && funByProject.NumericalValue > -10000000)
                            {
                                currFunWarnByProject.WarningLevelName = "黄色预警";
                            }
                            else if (funByProject.NumericalValue <= -10000000 && funByProject.NumericalValue > -20000000)
                            {
                                currFunWarnByProject.WarningLevelName = "橙色预警";
                            }
                            else if (funByProject.NumericalValue <= -20000000)
                            {
                                currFunWarnByProject.WarningLevelName = "红色预警";
                            }
                            projectFundList.Add(currFunWarnByProject);
                            addProjectLeftCashWarnList.Add(currFunWarnByProject);
                        }
                    }

                }
                //分公司资金占用预警
                IList addSubLeftCashWarnList = this.TransToSubCompanyLeftCashWarn(addProjectLeftCashWarnList, opgList);
                foreach (FundManagement fund in addSubLeftCashWarnList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                //公司资金占用预警
                IList addCompLeftCashList = this.TransToCompanyLeftCashWarn(addSubLeftCashWarnList);
                foreach (FundManagement fund in addCompLeftCashList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!companyIndexHt.Contains(subStr))
                    {
                        companyIndexHt.Add(subStr, fund);
                    }
                }
                //分公司资金存量
                IList addSubLeftCashList = this.GetSubCompanyLeftCashInfo(addCondition, opgList);
                foreach (FundManagement fund in addSubLeftCashList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                //公司资金存量(本部)
                FundManagement compSelfLeftCashFund = this.GetCompanySelfLeftCashInfo(addCondition, opgList);
                compStr = compSelfLeftCashFund.OperationOrg + "-" + compSelfLeftCashFund.IndexName + "-" + compSelfLeftCashFund.TimeName + "-" + compSelfLeftCashFund.MeasurementUnitName + "-" + compSelfLeftCashFund.WarningLevelName;
                if (!subCompanyIndexHt.Contains(compStr))
                {
                    compSelfLeftCashFund.CreateDate = currDate;
                    subCompanyIndexHt.Add(compStr, compSelfLeftCashFund);
                }
                //公司资金存量
                FundManagement compLeftCashFund = this.GetCompanyLeftCashInfo(addCondition, opgList);
                compStr = compLeftCashFund.OperationOrg + "-" + compLeftCashFund.IndexName + "-" + compLeftCashFund.TimeName + "-" + compLeftCashFund.MeasurementUnitName + "-" + compLeftCashFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr))
                {
                    compLeftCashFund.CreateDate = currDate;
                    companyIndexHt.Add(compStr, compLeftCashFund);
                }
                //项目日均存量，本年范围的(累计存量之和)/(累计天数),补上今天的存量
                IList addProjectAvgLeftCashList = this.GetProjectDayAvgMoney(yearCondition, addProjectLeftCashList);
                foreach (FundManagementByProject funByProject in addProjectAvgLeftCashList)
                {
                    CurrentProjectInfo projectInfo = project_ht[funByProject.ProjectId] as CurrentProjectInfo;
                    if (projectInfo != null)
                    {
                        funByProject.CreateDate = currDate;
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        projectFundList.Add(funByProject);
                    }
                }
                //分公司日均存量
                IList addSubAvgLeftCashList = this.GetSubCompanyDayAvgMoney(yearCondition, addSubLeftCashList, opgList);
                foreach (FundManagement fund in addSubAvgLeftCashList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                //公司日均存量(本部)
                FundManagement compSelfAvgMoneyFund = this.GetCompanySelfDayAvgMoney(yearCondition, compSelfLeftCashFund);
                compStr = compSelfAvgMoneyFund.OperationOrg + "-" + compSelfAvgMoneyFund.IndexName + "-" + compSelfAvgMoneyFund.TimeName + "-" + compSelfAvgMoneyFund.MeasurementUnitName + "-" + compSelfAvgMoneyFund.WarningLevelName;
                if (!subCompanyIndexHt.Contains(compStr))
                {
                    compSelfAvgMoneyFund.CreateDate = currDate;
                    subCompanyIndexHt.Add(compStr, compSelfAvgMoneyFund);
                }
                //公司日均存量
                FundManagement compAvgMoneyFund = this.GetCompanyDayAvgMoney(yearCondition, compLeftCashFund);
                compStr = compAvgMoneyFund.OperationOrg + "-" + compAvgMoneyFund.IndexName + "-" + compAvgMoneyFund.TimeName + "-" + compAvgMoneyFund.MeasurementUnitName + "-" + compAvgMoneyFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr))
                {
                    compAvgMoneyFund.CreateDate = currDate;
                    companyIndexHt.Add(compStr, compAvgMoneyFund);
                }
                if (currDate == monthLastDate)
                {
                    //项目月均存量，本年范围的(月均存量之和)/(累计天数),补上今天的存量
                    //补上月末数据
                    foreach (FundManagementByProject funByProject in addProjectLeftCashList)
                    {
                        CurrentProjectInfo projectInfo = project_ht[funByProject.ProjectId] as CurrentProjectInfo;
                        funByProject.CreateDate = currDate;
                        if (projectInfo != null)
                        {
                            FundManagementByProject currFunByProject = new FundManagementByProject();
                            currFunByProject.CreateDate = currDate;
                            currFunByProject.IndexName = "资金存量";
                            currFunByProject.MeasurementUnitName = "元";
                            currFunByProject.NumericalValue = funByProject.NumericalValue;
                            currFunByProject.OperationOrg = projectInfo.Data1;
                            currFunByProject.OperOrgName = projectInfo.OwnerOrgName;
                            currFunByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                            currFunByProject.OrganizationLevel = "项目";
                            currFunByProject.ProjectId = projectInfo.Id;
                            currFunByProject.ProjectName = projectInfo.Name;
                            currFunByProject.TimeName = "月末";
                            currFunByProject.WarningLevelName = "正常";
                            projectFundList.Add(currFunByProject);
                        }
                    }

                    //分公司月末存量
                    foreach (FundManagement fund in addSubLeftCashList)
                    {
                        FundManagement subFun = new FundManagement();
                        subFun.CreateDate = currDate;
                        subFun.OperationOrg = fund.OperationOrg;
                        subFun.OperOrgName = fund.OperOrgName;
                        subFun.OrganizationLevel = "分公司";
                        subFun.OpgSysCode = fund.OpgSysCode;
                        subFun.IndexName = "资金存量";
                        subFun.MeasurementUnitName = "元";
                        subFun.TimeName = "月末";
                        subFun.WarningLevelName = "正常";
                        subFun.NumericalValue = fund.NumericalValue;
                        string subStr = subFun.OperationOrg + "-" + subFun.IndexName + "-" + subFun.TimeName + "-" + subFun.MeasurementUnitName + "-" + subFun.WarningLevelName;
                        if (!subCompanyIndexHt.Contains(subStr))
                        {
                            subCompanyIndexHt.Add(subStr, subFun);
                        }
                    }

                    //公司月末存量
                    FundManagement companyLastFun = new FundManagement();
                    companyLastFun.OperationOrg = TransUtil.CompanyOpgGUID;
                    companyLastFun.OperOrgName = TransUtil.CompanyOpgName;
                    companyLastFun.OrganizationLevel = "公司";
                    companyLastFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
                    companyLastFun.IndexName = "资金存量";
                    companyLastFun.MeasurementUnitName = "元";
                    companyLastFun.TimeName = "月末";
                    companyLastFun.WarningLevelName = "正常";
                    companyLastFun.NumericalValue = compLeftCashFund.NumericalValue;
                    compStr = companyLastFun.OperationOrg + "-" + companyLastFun.IndexName + "-" + companyLastFun.TimeName + "-" + companyLastFun.MeasurementUnitName + "-" + companyLastFun.WarningLevelName;
                    if (!companyIndexHt.Contains(compStr))
                    {
                        companyLastFun.CreateDate = currDate;
                        companyIndexHt.Add(compStr, companyLastFun);
                    }
                }
                #endregion

                #region 应收账款/应收未收款

                IList addProjectGathList = this.GetProjectGatheringInfo(addCondition + " and t1.projectid is not null and t1.ifprojectmoney = 0 ");//累计收款
                string addOwnerQCondition = " and  nvl(t2.confirmdate,t1.createdate) <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                Hashtable addProjectQuantityHt = this.GetProjectOwnerQuantityInfo(addOwnerQCondition);//累计审量信息
                string yearOwnerQCondition = " and  nvl(t2.confirmdate,t1.createdate) <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                            " and  nvl(t2.confirmdate,t1.createdate) >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') ";
                Hashtable yearProjectQuantityHt = this.GetProjectOwnerQuantityInfo(yearOwnerQCondition);//本年审量信息
                foreach (string projectId in yearProjectQuantityHt.Keys)
                {
                    CurrentProjectInfo projectInfo = project_ht[projectId] as CurrentProjectInfo;
                    OwnerQuantityMaster master = yearProjectQuantityHt[projectId] as OwnerQuantityMaster;
                    FundManagementByProject funByProject = new FundManagementByProject();
                    if (projectInfo != null)
                    {
                        //项目指标
                        funByProject.CreateDate = currDate;
                        funByProject.IndexName = "业主审量";
                        funByProject.MeasurementUnitName = "元";
                        funByProject.NumericalValue = TransUtil.ToDecimal(master.Temp1);
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.OrganizationLevel = "项目";
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        funByProject.TimeName = "本年";
                        funByProject.WarningLevelName = "正常";
                        projectFundList.Add(funByProject);
                        haveTransFundList.Add(funByProject);
                    }
                }
                Hashtable addProjectGathHt = new Hashtable();//项目累计收款HT
                foreach (GatheringMaster master in addProjectGathList)
                {
                    if (!addProjectGathHt.Contains(master.ProjectId))
                    {
                        addProjectGathHt.Add(master.ProjectId, master.Temp1);
                    }
                }
                IList addProjectNoGathWarnList = new ArrayList();//应收账款拖欠项目集合
                foreach (string projectId in addProjectQuantityHt.Keys)//有报量无收款的情况
                {
                    if (!addProjectGathHt.Contains(projectId))
                    {
                        GatheringMaster master = new GatheringMaster();
                        master.ProjectId = projectId;
                        master.Temp1 = "0";
                        addProjectGathList.Add(master);
                    }
                }
                foreach (GatheringMaster master in addProjectGathList)
                {
                    CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                    //项目指标: 应收账款总额
                    if (projectInfo != null)
                    {                       
                        FundManagementByProject funByProject = new FundManagementByProject();
                        funByProject.CreateDate = currDate;
                        funByProject.IndexName = "应收账款";
                        funByProject.MeasurementUnitName = "元";
                        funByProject.OperationOrg = projectInfo.Data1;
                        funByProject.OperOrgName = projectInfo.OwnerOrgName;
                        funByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        funByProject.OrganizationLevel = "项目";
                        funByProject.ProjectId = projectInfo.Id;
                        funByProject.ProjectName = projectInfo.Name;
                        funByProject.TimeName = "累计";
                        funByProject.WarningLevelName = "正常";
                        //审量信息
                        if (addProjectQuantityHt.Contains(projectInfo.Id))
                        {
                            OwnerQuantityMaster currMaster = addProjectQuantityHt[projectInfo.Id] as OwnerQuantityMaster;
                            funByProject.NumericalValue = TransUtil.ToDecimal(currMaster.Temp1) - TransUtil.ToDecimal(master.Temp1);
                            //funByProject.NumericalValue = TransUtil.ToDecimal(currMaster.Temp3);
                        }
                        else
                        {
                            funByProject.NumericalValue = -TransUtil.ToDecimal(master.Temp1);
                            //funByProject.NumericalValue = 0;
                        }
                        projectFundList.Add(funByProject);
                        haveTransFundList.Add(funByProject);

                        //应收未收款总额(合同应收工程款-累计收款) 预警信息
                        FundManagementByProject fundNoGatheringByProject = new FundManagementByProject();
                        fundNoGatheringByProject.CreateDate = currDate;
                        fundNoGatheringByProject.IndexName = "应收未收款";
                        fundNoGatheringByProject.MeasurementUnitName = "元";
                        fundNoGatheringByProject.OperationOrg = projectInfo.Data1;
                        fundNoGatheringByProject.OperOrgName = projectInfo.OwnerOrgName;
                        fundNoGatheringByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                        fundNoGatheringByProject.OrganizationLevel = "项目";
                        fundNoGatheringByProject.ProjectId = projectInfo.Id;
                        fundNoGatheringByProject.ProjectName = projectInfo.Name;
                        fundNoGatheringByProject.TimeName = "累计";
                        fundNoGatheringByProject.WarningLevelName = "正常";
                        //审量信息
                        if (addProjectQuantityHt.Contains(projectInfo.Id))
                        {
                            OwnerQuantityMaster currMaster = addProjectQuantityHt[projectInfo.Id] as OwnerQuantityMaster;
                            fundNoGatheringByProject.NumericalValue = TransUtil.ToDecimal(currMaster.Temp3) - TransUtil.ToDecimal(master.Temp1);
                        }
                        else
                        {
                            fundNoGatheringByProject.NumericalValue = -TransUtil.ToDecimal(master.Temp1);
                        }
                        if (fundNoGatheringByProject.NumericalValue > 0)
                        {
                            projectFundList.Add(fundNoGatheringByProject);
                            haveTransFundList.Add(fundNoGatheringByProject);
                        }
                        if (fundNoGatheringByProject.NumericalValue >= 5000000)
                        {
                            addProjectNoGathWarnList.Add(fundNoGatheringByProject);
                        }
                    }
                }
                //应收账款拖欠预警
                IList projectNoGathWarnList = this.GetProjectNoGatheringWarnList(currDate, addProjectNoGathWarnList);
                foreach (FundManagementByProject funByProject in projectNoGathWarnList)
                {
                    projectFundList.Add(funByProject);
                    haveTransFundList.Add(funByProject);
                }
                //应收款拖欠分公司预警(个)
                IList subNoGathWarnList = this.TransToSubCompanyNoGathWarnWarn(projectNoGathWarnList, opgList);
                foreach (FundManagement fund in subNoGathWarnList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                //公司累计收现率预警(个)
                IList compNoGathWarnList = this.TransToCompanyNoGathWarnWarn(subNoGathWarnList);
                foreach (FundManagement fund in compNoGathWarnList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!companyIndexHt.Contains(subStr))
                    {
                        companyIndexHt.Add(subStr, fund);
                    }
                }
                #endregion

                #region 保证金
                Hashtable bailHt = this.GetBailPaymentInfo(currDate, opgList);
                IList subLastBailList = bailHt["分公司上年余额"] as ArrayList;
                foreach (FundManagement fund in subLastBailList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                IList subGBailList = bailHt["分公司本年收"] as ArrayList;
                foreach (FundManagement fund in subGBailList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                IList subPBailList = bailHt["分公司本年付"] as ArrayList;
                foreach (FundManagement fund in subPBailList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                IList subCurrBailList = bailHt["分公司本年余额"] as ArrayList;
                foreach (FundManagement fund in subCurrBailList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                FundManagement compLastBailFund = bailHt["公司上年余额"] as FundManagement;
                compStr = compLastBailFund.OperationOrg + "-" + compLastBailFund.IndexName + "-" + compLastBailFund.TimeName + "-" + compLastBailFund.MeasurementUnitName + "-" + compLastBailFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr))
                {
                    companyIndexHt.Add(compStr, compLastBailFund);
                }
                FundManagement compGBailFund = bailHt["公司本年收"] as FundManagement;
                compStr = compGBailFund.OperationOrg + "-" + compGBailFund.IndexName + "-" + compGBailFund.TimeName + "-" + compGBailFund.MeasurementUnitName + "-" + compGBailFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr))
                {
                    companyIndexHt.Add(compStr, compGBailFund);
                }
                FundManagement compPBailFund = bailHt["公司本年付"] as FundManagement;
                compStr = compPBailFund.OperationOrg + "-" + compPBailFund.IndexName + "-" + compPBailFund.TimeName + "-" + compPBailFund.MeasurementUnitName + "-" + compPBailFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr))
                {
                    companyIndexHt.Add(compStr, compPBailFund);
                }
                FundManagement compCurrBailFund = bailHt["公司本年余额"] as FundManagement;
                compStr = compCurrBailFund.OperationOrg + "-" + compCurrBailFund.IndexName + "-" + compCurrBailFund.TimeName + "-" + compCurrBailFund.MeasurementUnitName + "-" + compCurrBailFund.WarningLevelName;
                if (!companyIndexHt.Contains(compStr))
                {
                    companyIndexHt.Add(compStr, compCurrBailFund);
                }
                IList projectBailList = bailHt["项目数据"] as ArrayList;
                foreach (FundManagementByProject funByProject in projectBailList)
                {
                    funByProject.CreateDate = currDate;
                    funByProject.MeasurementUnitName = "元";
                    funByProject.OrganizationLevel = "项目";
                    funByProject.WarningLevelName = "正常";
                    projectFundList.Add(funByProject);
                }
                #endregion

                #region 预警
                //项目本年收现率(收款/产值)
                Hashtable yearProjectGathHt = new Hashtable();//项目本年收款HT
                decimal sumYearProductValue = 0;//本年公司产值
                decimal sumYearGatherMoney = 0;//本年公司收款
                foreach (GatheringMaster master in yearGatheringList)
                {
                    if (!yearProjectGathHt.Contains(master.ProjectId))
                    {
                        yearProjectGathHt.Add(master.ProjectId, master.Temp1);
                    }
                }
                IList yearSubGathAndProjectList = new ArrayList();
                foreach (OperationOrgInfo orgInfo in opgList)
                {
                    FundManagement fund = new FundManagement();
                    fund.CreateDate = currDate;
                    fund.IndexName = "收现率";
                    fund.MeasurementUnitName = "百分比";
                    fund.OperationOrg = orgInfo.Id;
                    fund.OperOrgName = orgInfo.Name;
                    fund.OpgSysCode = orgInfo.SysCode;
                    fund.OrganizationLevel = "分公司";
                    fund.TimeName = "本年";
                    fund.WarningLevelName = "正常";
                    yearSubGathAndProjectList.Add(fund);
                }
                foreach (FinanceMultDataMaster master in yearProductValueList)
                {
                    decimal yearGatherMoney = TransUtil.ToDecimal(yearProjectGathHt[master.ProjectId]);//本年收款
                    decimal yearProjectValue = TransUtil.ToDecimal(master.Temp1);//本年营业收入
                    sumYearProductValue += yearProjectValue;
                    sumYearGatherMoney += yearGatherMoney;

                    decimal yearGatherRate = 0;//本年收现率
                    if (yearProjectValue != 0)
                    {
                        yearGatherRate = decimal.Round((yearGatherMoney / yearProjectValue) * 100, 2);
                        FundManagementByProject currFunByProject = new FundManagementByProject();
                        CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                        if (projectInfo != null)
                        {
                            foreach (FundManagement fund in yearSubGathAndProjectList)
                            {
                                if (projectInfo.OwnerOrgSysCode.Contains(fund.OpgSysCode))
                                {
                                    fund.Temp1 += yearProjectValue;
                                    fund.Temp2 += yearGatherMoney;
                                }
                            }
                            currFunByProject.CreateDate = currDate;
                            currFunByProject.IndexName = "收现率";
                            currFunByProject.MeasurementUnitName = "百分比";
                            currFunByProject.NumericalValue = yearGatherRate;
                            currFunByProject.OperationOrg = projectInfo.Data1;
                            currFunByProject.OperOrgName = projectInfo.OwnerOrgName;
                            currFunByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                            currFunByProject.OrganizationLevel = "项目";
                            currFunByProject.ProjectId = projectInfo.Id;
                            currFunByProject.ProjectName = projectInfo.Name;
                            currFunByProject.TimeName = "本年";
                            currFunByProject.WarningLevelName = "正常";
                            projectFundList.Add(currFunByProject);
                        }
                    }
                }
                //分公司本年收现率
                foreach (FundManagement fund in yearSubGathAndProjectList)
                {
                    if (fund.Temp1 != 0)
                    {
                        fund.NumericalValue = decimal.Round((fund.Temp2 / fund.Temp1) * 100, 2);
                    }
                    else
                    {
                        fund.NumericalValue = 0;
                    }
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }

                //公司本年收现率
                if (sumYearProductValue != 0)
                {
                    decimal yearCompanyGatherRate = decimal.Round((sumYearGatherMoney / sumYearProductValue) * 100, 2);
                    FundManagement fund = new FundManagement();
                    fund.CreateDate = currDate;
                    fund.IndexName = "收现率";
                    fund.MeasurementUnitName = "百分比";
                    fund.NumericalValue = yearCompanyGatherRate;
                    fund.OperationOrg = TransUtil.CompanyOpgGUID;
                    fund.OperOrgName = TransUtil.CompanyOpgName;
                    fund.OpgSysCode = TransUtil.CompanyOpgSyscode;
                    fund.OrganizationLevel = "公司";
                    fund.TimeName = "本年";
                    fund.WarningLevelName = "正常";
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    if (!companyIndexHt.Contains(subStr))
                    {
                        companyIndexHt.Add(subStr, fund);
                    }
                }

                //累计收现率预警: 累计收款/营业收入 - 合同应收比例
                IList addProjectGatherRateWarnList = new ArrayList();//项目资金占用预警
                
                foreach (FinanceMultDataMaster master in addProductValueList)
                {
                    decimal addGatherMoney = TransUtil.ToDecimal(addProjectGathHt[master.ProjectId]);//累计收款
                    decimal addProjectValue = TransUtil.ToDecimal(master.Temp1);//累计营业收入
                    decimal contractRate = 0;//最大合同比例
                    OwnerQuantityMaster currQMaster = addProjectQuantityHt[master.ProjectId] as OwnerQuantityMaster;
                    if (currQMaster != null)
                    {
                        contractRate = TransUtil.ToDecimal(currQMaster.Temp2);
                    }
                    decimal addGatherRate = 0;//累计收现率
                    if (addProjectValue != 0)
                    {
                        addGatherRate = decimal.Round((addGatherMoney / addProjectValue - contractRate) * 100, 2);
                    }

                    if (addGatherRate < 0)
                    {
                        FundManagementByProject currFunByProject = new FundManagementByProject();
                        CurrentProjectInfo projectInfo = project_ht[master.ProjectId] as CurrentProjectInfo;
                        if (projectInfo != null)
                        {
                            currFunByProject.CreateDate = currDate;
                            currFunByProject.IndexName = "收现率";
                            currFunByProject.MeasurementUnitName = "百分比";
                            currFunByProject.NumericalValue = decimal.Round((addGatherMoney / addProjectValue) * 100, 2);
                            currFunByProject.OperationOrg = projectInfo.Data1;
                            currFunByProject.OperOrgName = projectInfo.OwnerOrgName;
                            currFunByProject.OpgSysCode = projectInfo.OwnerOrgSysCode;
                            currFunByProject.OrganizationLevel = "项目";
                            currFunByProject.ProjectId = projectInfo.Id;
                            currFunByProject.ProjectName = projectInfo.Name;
                            currFunByProject.TimeName = "累计";
                            if (addGatherRate < 0 && addGatherRate > -10)
                            {
                                currFunByProject.WarningLevelName = "黄色预警";
                            }
                            else if (addGatherRate <= -10 && addGatherRate > -15)
                            {
                                currFunByProject.WarningLevelName = "橙色预警";
                            }
                            else if (addGatherRate <= -15)
                            {
                                currFunByProject.WarningLevelName = "红色预警";
                            }
                            projectFundList.Add(currFunByProject);
                            addProjectGatherRateWarnList.Add(currFunByProject);
                        }
                    }

                }
                //分公司累计收现率预警
                IList addSubGatherRateWarnList = this.TransToSubCompanyGathRateWarn(addProjectGatherRateWarnList, opgList);
                foreach (FundManagement fund in addSubGatherRateWarnList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!subCompanyIndexHt.Contains(subStr))
                    {
                        subCompanyIndexHt.Add(subStr, fund);
                    }
                }
                //公司累计收现率预警
                IList addCompGatherRateWarnList = this.TransToCompanyGathRateWarn(addSubGatherRateWarnList);
                foreach (FundManagement fund in addCompGatherRateWarnList)
                {
                    string subStr = fund.OperationOrg + "-" + fund.IndexName + "-" + fund.TimeName + "-" + fund.MeasurementUnitName + "-" + fund.WarningLevelName;
                    fund.CreateDate = currDate;
                    if (!companyIndexHt.Contains(subStr))
                    {
                        companyIndexHt.Add(subStr, fund);
                    }
                }
                #endregion

                #region 写入数据库
                subCompanyIndexHt = this.TransSubCompanyIndexList(subCompanyIndexHt, haveTransFundList, opgList);
                companyIndexHt = this.TransCompanyIndexList(companyIndexHt, haveTransFundList);

                foreach (FundManagementByProject fund in projectFundList)
                {
                    fund.Type = IndexType.Fund;
                }
                //写入项目指标结果表
                this.InsertFundManageByProjectByBatch(projectFundList);
                //写入分公司/公司指标结果表
                IList subList = new ArrayList();
                foreach (FundManagement subFun in subCompanyIndexHt.Values)
                {
                    subFun.Type = IndexType.Fund;
                    subList.Add(subFun);
                }
                this.InsertFundManagementByBatch(subList);
                //获取公司的内部项目本年营业收入和工程款
                decimal inProjectGathMoney = GetCompanyInProjectGatheringMoney(yearCondition);
                string yearInProjectCondition = " and t1.year=" + currYear;
                decimal inProjectValueMoney = this.GetCompanyInProjectProjectValueMoney(yearInProjectCondition, yearCondition);
                IList companyList = new ArrayList();

                decimal yearCompanyProductValue = 0;//本年产值
                decimal yearCompanyGatheringValue = 0;//本年收款
                foreach (FundManagement fund in companyIndexHt.Values)
                {
                    if (fund.IndexName == "营业收入" && fund.TimeName == "本年")
                    {
                        fund.NumericalValue -= inProjectValueMoney;
                        yearCompanyProductValue = fund.NumericalValue;
                    }
                    if (fund.IndexName == "收款" && fund.TimeName == "本年")
                    {
                        fund.NumericalValue -= inProjectGathMoney;
                        yearCompanyGatheringValue = fund.NumericalValue;
                    }
                    companyList.Add(fund);
                }
                foreach (FundManagement fund in companyList)
                {
                    fund.Type = IndexType.Fund;
                    if (fund.IndexName == "收现率" && fund.TimeName == "本年")
                    {
                        if (yearCompanyProductValue != 0)
                        {
                            fund.NumericalValue = decimal.Round((yearCompanyGatheringValue / yearCompanyProductValue) * 100, 2);
                        }
                    }
                }
                this.InsertFundManagementByBatch(companyList);
                #endregion
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            //finally
            //{

            //}
        }
        //转换成分公司集合
        private Hashtable TransSubCompanyIndexList(Hashtable subCompanyIndexHt, IList projectIndexList, IList subOpgList)
        {
            DateTime currDate = TransUtil.ToDateTime("1990-01-01");
            foreach (FundManagementByProject fundProject in projectIndexList)
            {
                if (currDate < TransUtil.ToDateTime("2000-01-01"))
                {
                    currDate = fundProject.CreateDate;
                }
                //查找归属分公司
                OperationOrgInfo subCompany = new OperationOrgInfo();
                bool isExist = false;
                foreach (OperationOrgInfo orgInfo in subOpgList)
                {
                    if (fundProject.OpgSysCode.Contains(orgInfo.Id))
                    {
                        subCompany = orgInfo;
                        isExist = true;
                    }
                }
                //汇总分公司,分公司ID_指标名称_时间名称_计量单位名称_预警名称
                string subStr = subCompany.Id + "-" + fundProject.IndexName + "-" + fundProject.TimeName + "-" + fundProject.MeasurementUnitName + "-" + fundProject.WarningLevelName;
                if (subCompanyIndexHt.ContainsKey(subStr))
                {
                    FundManagement fund = (FundManagement)subCompanyIndexHt[subStr];
                    fund.NumericalValue += fundProject.NumericalValue;
                }
                else
                {
                    FundManagement fund = new FundManagement();
                    fund.OrganizationLevel = "分公司";
                    fund.CreateDate = TransUtil.ToDateTime(currDate.ToShortDateString());
                    fund.IndexID = fundProject.IndexID;
                    fund.IndexName = fundProject.IndexName;
                    fund.MeasurementUnitID = fundProject.MeasurementUnitID;
                    fund.MeasurementUnitName = fundProject.MeasurementUnitName;
                    fund.NumericalValue = fundProject.NumericalValue;
                    fund.OperationOrg = subCompany.Id;
                    fund.OperOrgName = subCompany.Name;
                    fund.OpgSysCode = subCompany.SysCode;
                    fund.TimeID = fundProject.TimeID;
                    fund.TimeName = fundProject.TimeName;
                    fund.WarningLevelID = fundProject.WarningLevelID;
                    fund.WarningLevelName = fundProject.WarningLevelName;
                    subCompanyIndexHt.Add(subStr, fund);
                }
            }
            return subCompanyIndexHt;
        }
        //转换成公司集合
        private Hashtable TransCompanyIndexList(Hashtable companyIndexHt, IList projectIndexList)
        {
            DateTime currDate = TransUtil.ToDateTime("1990-01-01");
            foreach (FundManagementByProject fundProject in projectIndexList)
            {
                if (currDate < TransUtil.ToDateTime("2000-01-01"))
                {
                    currDate = fundProject.CreateDate;
                }
                //汇总公司,指标名称_时间名称_计量单位名称_预警名称
                string companyStr = fundProject.IndexName + "-" + fundProject.TimeName + "-" + fundProject.MeasurementUnitName + "-" + fundProject.WarningLevelName;
                if (companyIndexHt.ContainsKey(companyStr))
                {
                    FundManagement fund = (FundManagement)companyIndexHt[companyStr];
                    fund.NumericalValue += fundProject.NumericalValue;
                }
                else
                {
                    FundManagement fund = new FundManagement();
                    fund.OrganizationLevel = "公司";
                    fund.CreateDate = TransUtil.ToDateTime(currDate.ToShortDateString());
                    fund.IndexID = fundProject.IndexID;
                    fund.IndexName = fundProject.IndexName;
                    fund.MeasurementUnitID = fundProject.MeasurementUnitID;
                    fund.MeasurementUnitName = fundProject.MeasurementUnitName;
                    fund.NumericalValue = fundProject.NumericalValue;
                    fund.OperationOrg = TransUtil.CompanyOpgGUID;
                    fund.OperOrgName = TransUtil.CompanyOpgName;
                    fund.OpgSysCode = TransUtil.CompanyOpgSyscode;
                    fund.TimeID = fundProject.TimeID;
                    fund.TimeName = fundProject.TimeName;
                    fund.WarningLevelID = fundProject.WarningLevelID;
                    fund.WarningLevelName = fundProject.WarningLevelName;
                    companyIndexHt.Add(companyStr, fund);
                }

            }
            return companyIndexHt;
        }
        /// <summary>
        /// 查询分公司和项目信息
        /// </summary>
        private Hashtable GetCurrOrgAndProjectInfo(string projectCondtion)
        {
            Hashtable ht = new Hashtable();
            IList opgList = new ArrayList();//分公司组织集合
            Hashtable project_ht = new Hashtable();//项目集合
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = "select t1.opgid,t1.opgname,t1.opgsyscode from resoperationorg t1 where t1.opgstate=1 and t1.opgoperationtype='b'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    OperationOrgInfo orgInfo = new OperationOrgInfo();
                    orgInfo.Id = TransUtil.ToString(dataRow["opgid"]);
                    orgInfo.Name = TransUtil.ToString(dataRow["opgname"]);
                    orgInfo.SysCode = TransUtil.ToString(dataRow["opgsyscode"]);
                    opgList.Add(orgInfo);
                }
                ht.Add("subcompanyinfo", opgList);
            }
            //项目信息
            command.CommandText = "select t1.id,t1.projectname,t1.projectcode,t1.ownerorg,t1.ownerorgname, t1.ownerorgsyscode " +
                                    " From resconfig t1 where  nvl(t1.projectcurrstate,0) != 20" + projectCondtion;
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CurrentProjectInfo projectInfo = new CurrentProjectInfo();
                    projectInfo.Id = TransUtil.ToString(dataRow["id"]);
                    projectInfo.Name = TransUtil.ToString(dataRow["projectname"]);
                    projectInfo.OwnerOrgName = TransUtil.ToString(dataRow["ownerorgname"]);
                    projectInfo.Data1 = TransUtil.ToString(dataRow["ownerorg"]);//归属组织ID
                    projectInfo.OwnerOrgSysCode = TransUtil.ToString(dataRow["ownerorgsyscode"]);
                    project_ht.Add(projectInfo.Id, projectInfo);
                }
                ht.Add("projectinfo", project_ht);
            }
            return ht;
        }
        /// 分公司资金占用预警
        /// </summary>
        private IList TransToSubCompanyLeftCashWarn(IList addProjectLeftCashWarnList, IList opgList)
        {
            IList subList = new ArrayList();
            Hashtable existHt = new Hashtable();
            foreach (FundManagementByProject funByProject in addProjectLeftCashWarnList)
            {
                foreach (OperationOrgInfo orgInfo in opgList)
                {
                    if (funByProject.OpgSysCode.Contains(orgInfo.SysCode))
                    {
                        string subStrMoney = orgInfo.Id + "-" + funByProject.WarningLevelName + "-元";
                        string subStrCount = orgInfo.Id + "-" + funByProject.WarningLevelName + "-个";
                        if (!existHt.Contains(subStrMoney))
                        {
                            FundManagement subFun = new FundManagement();
                            subFun.OperationOrg = orgInfo.Id;
                            subFun.OperOrgName = orgInfo.Name;
                            subFun.OrganizationLevel = "分公司";
                            subFun.OpgSysCode = orgInfo.SysCode;
                            subFun.IndexName = "资金占用";
                            subFun.MeasurementUnitName = "元";
                            subFun.TimeName = "累计";
                            subFun.WarningLevelName = funByProject.WarningLevelName;
                            subFun.NumericalValue = funByProject.NumericalValue;
                            existHt.Add(subStrMoney, subFun);
                        }
                        else
                        {
                            FundManagement subFun = existHt[subStrMoney] as FundManagement;
                            subFun.NumericalValue += funByProject.NumericalValue;
                        }
                        if (!existHt.Contains(subStrCount))
                        {
                            FundManagement subFun = new FundManagement();
                            subFun.OperationOrg = orgInfo.Id;
                            subFun.OperOrgName = orgInfo.Name;
                            subFun.OrganizationLevel = "分公司";
                            subFun.OpgSysCode = orgInfo.SysCode;
                            subFun.IndexName = "资金占用";
                            subFun.MeasurementUnitName = "个";
                            subFun.TimeName = "累计";
                            subFun.WarningLevelName = funByProject.WarningLevelName;
                            subFun.NumericalValue = 1;
                            existHt.Add(subStrCount, subFun);
                        }
                        else
                        {
                            FundManagement subFun = existHt[subStrCount] as FundManagement;
                            subFun.NumericalValue += 1;
                        }
                    }
                }
            }
            foreach (FundManagement subFun in existHt.Values)
            {
                subList.Add(subFun);
            }
            return subList;
        }
        /// 公司资金占用预警
        /// </summary>
        private IList TransToCompanyLeftCashWarn(IList addSubLeftCashWarnList)
        {
            IList list = new ArrayList();
            Hashtable existHt = new Hashtable();
            foreach (FundManagement subFun in addSubLeftCashWarnList)
            {
                string str = subFun.WarningLevelName + "-" + subFun.MeasurementUnitName;
                if (subFun.MeasurementUnitName == "元")
                {
                    if (!existHt.Contains(str))
                    {
                        FundManagement fund = new FundManagement();
                        fund.OperationOrg = TransUtil.CompanyOpgGUID;
                        fund.OperOrgName = TransUtil.CompanyOpgName;
                        fund.OrganizationLevel = "公司";
                        fund.OpgSysCode = TransUtil.CompanyOpgSyscode;
                        fund.IndexName = "资金占用";
                        fund.MeasurementUnitName = "元";
                        fund.TimeName = "累计";
                        fund.WarningLevelName = subFun.WarningLevelName;
                        fund.NumericalValue = subFun.NumericalValue;
                        existHt.Add(str, fund);
                    }
                    else
                    {
                        FundManagement fund = existHt[str] as FundManagement;
                        fund.NumericalValue += subFun.NumericalValue;
                    }
                }
                if (subFun.MeasurementUnitName == "个")
                {
                    if (!existHt.Contains(str))
                    {
                        FundManagement fund = new FundManagement();
                        fund.OperationOrg = TransUtil.CompanyOpgGUID;
                        fund.OperOrgName = TransUtil.CompanyOpgName;
                        fund.OrganizationLevel = "公司";
                        fund.OpgSysCode = TransUtil.CompanyOpgSyscode;
                        fund.IndexName = "资金占用";
                        fund.MeasurementUnitName = "个";
                        fund.TimeName = "累计";
                        fund.WarningLevelName = subFun.WarningLevelName;
                        fund.NumericalValue = subFun.NumericalValue;
                        existHt.Add(str, fund);
                    }
                    else
                    {
                        FundManagement fund = existHt[str] as FundManagement;
                        fund.NumericalValue += subFun.NumericalValue;
                    }
                }
            }
            foreach (FundManagement fund in existHt.Values)
            {
                list.Add(fund);
            }
            return list;
        }
        /// 分公司累计收现率预警
        /// </summary>
        private IList TransToSubCompanyGathRateWarn(IList addProjectGatherRateWarnList, IList opgList)
        {
            IList subList = new ArrayList();
            Hashtable existHt = new Hashtable();
            foreach (FundManagementByProject funByProject in addProjectGatherRateWarnList)
            {
                foreach (OperationOrgInfo orgInfo in opgList)
                {
                    if (funByProject.OpgSysCode.Contains(orgInfo.SysCode))
                    {
                        string subStrCount = orgInfo.Id + "-" + funByProject.WarningLevelName + "-个";
                        if (!existHt.Contains(subStrCount))
                        {
                            FundManagement subFun = new FundManagement();
                            subFun.OperationOrg = orgInfo.Id;
                            subFun.OperOrgName = orgInfo.Name;
                            subFun.OrganizationLevel = "分公司";
                            subFun.OpgSysCode = orgInfo.SysCode;
                            subFun.IndexName = "收现率";
                            subFun.MeasurementUnitName = "个";
                            subFun.TimeName = "累计";
                            subFun.WarningLevelName = funByProject.WarningLevelName;
                            subFun.NumericalValue = 1;
                            existHt.Add(subStrCount, subFun);
                        }
                        else
                        {
                            FundManagement subFun = existHt[subStrCount] as FundManagement;
                            subFun.NumericalValue += 1;
                        }
                    }
                }
            }
            foreach (FundManagement subFun in existHt.Values)
            {
                subList.Add(subFun);
            }

            return subList;
        }
        /// 公司累计收现率预警
        /// </summary>
        private IList TransToCompanyGathRateWarn(IList addSubGatherRateWarnList)
        {
            IList list = new ArrayList();
            Hashtable existHt = new Hashtable();
            foreach (FundManagement subFun in addSubGatherRateWarnList)
            {
                string strCount = subFun.WarningLevelName + "-个";
                if (!existHt.Contains(strCount))
                {
                    FundManagement fund = new FundManagement();
                    fund.OperationOrg = TransUtil.CompanyOpgGUID;
                    fund.OperOrgName = TransUtil.CompanyOpgName;
                    fund.OrganizationLevel = "公司";
                    fund.OpgSysCode = TransUtil.CompanyOpgSyscode;
                    fund.IndexName = "收现率";
                    fund.MeasurementUnitName = "个";
                    fund.TimeName = "累计";
                    fund.WarningLevelName = subFun.WarningLevelName;
                    fund.NumericalValue = subFun.NumericalValue;
                    existHt.Add(strCount, fund);
                }
                else
                {
                    FundManagement fund = existHt[strCount] as FundManagement;
                    fund.NumericalValue += subFun.NumericalValue;
                }
            }
            foreach (FundManagement fund in existHt.Values)
            {
                list.Add(fund);
            }
            return list;
        }
        /// 分公司应收款拖欠预警
        /// </summary>
        private IList TransToSubCompanyNoGathWarnWarn(IList subNoGathWarnWarnList, IList opgList)
        {
            IList subList = new ArrayList();
            Hashtable existHt = new Hashtable();
            foreach (FundManagementByProject funByProject in subNoGathWarnWarnList)
            {
                foreach (OperationOrgInfo orgInfo in opgList)
                {
                    if (funByProject.OpgSysCode.Contains(orgInfo.SysCode))
                    {
                        string subStrCount = orgInfo.Id + "-" + funByProject.WarningLevelName + "-个";
                        if (!existHt.Contains(subStrCount))
                        {
                            FundManagement subFun = new FundManagement();
                            subFun.OperationOrg = orgInfo.Id;
                            subFun.OperOrgName = orgInfo.Name;
                            subFun.OrganizationLevel = "分公司";
                            subFun.OpgSysCode = orgInfo.SysCode;
                            subFun.IndexName = "应收未收款";
                            subFun.MeasurementUnitName = "个";
                            subFun.TimeName = "累计";
                            subFun.WarningLevelName = funByProject.WarningLevelName;
                            subFun.NumericalValue = 1;
                            existHt.Add(subStrCount, subFun);
                        }
                        else
                        {
                            FundManagement subFun = existHt[subStrCount] as FundManagement;
                            subFun.NumericalValue += 1;
                        }
                    }
                }
            }
            foreach (FundManagement subFun in existHt.Values)
            {
                subList.Add(subFun);
            }

            return subList;
        }
        /// 公司应收款拖欠预警
        /// </summary>
        private IList TransToCompanyNoGathWarnWarn(IList subGatherNoGathWarnList)
        {
            IList list = new ArrayList();
            Hashtable existHt = new Hashtable();
            foreach (FundManagement subFun in subGatherNoGathWarnList)
            {
                string strCount = subFun.WarningLevelName + "-个";
                if (!existHt.Contains(strCount))
                {
                    FundManagement fund = new FundManagement();
                    fund.OperationOrg = TransUtil.CompanyOpgGUID;
                    fund.OperOrgName = TransUtil.CompanyOpgName;
                    fund.OrganizationLevel = "公司";
                    fund.OpgSysCode = TransUtil.CompanyOpgSyscode;
                    fund.IndexName = "应收未收款";
                    fund.MeasurementUnitName = "个";
                    fund.TimeName = "累计";
                    fund.WarningLevelName = subFun.WarningLevelName;
                    fund.NumericalValue = subFun.NumericalValue;
                    existHt.Add(strCount, fund);
                }
                else
                {
                    FundManagement fund = existHt[strCount] as FundManagement;
                    fund.NumericalValue += subFun.NumericalValue;
                }
            }
            foreach (FundManagement fund in existHt.Values)
            {
                list.Add(fund);
            }
            return list;
        }
        /// <summary>
        /// 查询收款信息
        /// </summary>
        private IList GetProjectGatheringInfo(string condition)
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.projectid,sum(t1.summoney) money From thd_gatheringmaster t1 where t1.ifprojectmoney=0 and t1.state=5 " + condition + " group by t1.projectid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GatheringMaster master = new GatheringMaster();
                    master.ProjectId = TransUtil.ToString(dataRow["projectid"]);
                    master.Temp1 = TransUtil.ToString(dataRow["money"]);
                    list.Add(master);
                }
            }
            return list;
        }
        /// <summary>
        /// 查询分公司/公司付款信息(保证金)
        /// </summary>
        private Hashtable GetBailPaymentInfo(DateTime currDate, IList opgList)
        {
            DateTime yearFirstDate = new DateTime(currDate.Year, 1, 1);//当年第一天
            string lastYearCondition = " and t1.createDate < to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd')";//上年累计
            string yearCondition = " and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                    " and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd')";//本年
            Hashtable ht = new Hashtable();
            #region 分公司集合
            IList subGatheringList = new ArrayList();
            IList subPaymentList = new ArrayList();
            IList subLastList = new ArrayList();
            IList subCurrList = new ArrayList();
            foreach (OperationOrgInfo orgInfo in opgList)
            {
                FundManagement subGFun = new FundManagement();
                subGFun.OperationOrg = orgInfo.Id;
                subGFun.OperOrgName = orgInfo.Name;
                subGFun.OrganizationLevel = "分公司";
                subGFun.OpgSysCode = orgInfo.SysCode;
                subGFun.IndexName = "收保证金";
                subGFun.MeasurementUnitName = "元";
                subGFun.TimeName = "本年";
                subGFun.WarningLevelName = "正常";
                subGFun.CreateDate = currDate;
                subGatheringList.Add(subGFun);

                FundManagement subPFun = new FundManagement();
                subPFun.OperationOrg = orgInfo.Id;
                subPFun.OperOrgName = orgInfo.Name;
                subPFun.OrganizationLevel = "分公司";
                subPFun.OpgSysCode = orgInfo.SysCode;
                subPFun.IndexName = "付保证金";
                subPFun.MeasurementUnitName = "元";
                subPFun.TimeName = "本年";
                subPFun.WarningLevelName = "正常";
                subPFun.CreateDate = currDate;
                subPaymentList.Add(subPFun);

                FundManagement subLastFun = new FundManagement();
                subLastFun.OperationOrg = orgInfo.Id;
                subLastFun.OperOrgName = orgInfo.Name;
                subLastFun.OrganizationLevel = "分公司";
                subLastFun.OpgSysCode = orgInfo.SysCode;
                subLastFun.IndexName = "保证金余额";
                subLastFun.MeasurementUnitName = "元";
                subLastFun.TimeName = "上年";
                subLastFun.WarningLevelName = "正常";
                subLastFun.CreateDate = currDate;
                subLastList.Add(subLastFun);

                FundManagement subCurrFun = new FundManagement();
                subCurrFun.OperationOrg = orgInfo.Id;
                subCurrFun.OperOrgName = orgInfo.Name;
                subCurrFun.OrganizationLevel = "分公司";
                subCurrFun.OpgSysCode = orgInfo.SysCode;
                subCurrFun.IndexName = "保证金余额";
                subCurrFun.MeasurementUnitName = "元";
                subCurrFun.TimeName = "累计";
                subCurrFun.WarningLevelName = "正常";
                subCurrFun.CreateDate = currDate;
                subCurrList.Add(subCurrFun);
            }
            #endregion

            #region 公司总部
            FundManagement compSelfGFun = new FundManagement();
            compSelfGFun.OperationOrg = TransUtil.CompanyOpgGUID;
            compSelfGFun.OperOrgName = "公司总部";
            compSelfGFun.OrganizationLevel = "分公司";
            compSelfGFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            compSelfGFun.IndexName = "收保证金";
            compSelfGFun.MeasurementUnitName = "元";
            compSelfGFun.TimeName = "本年";
            compSelfGFun.WarningLevelName = "正常";
            compSelfGFun.CreateDate = currDate;

            FundManagement compSelfPFun = new FundManagement();
            compSelfPFun.OperationOrg = TransUtil.CompanyOpgGUID;
            compSelfPFun.OperOrgName = "公司总部";
            compSelfPFun.OrganizationLevel = "分公司";
            compSelfPFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            compSelfPFun.IndexName = "付保证金";
            compSelfPFun.MeasurementUnitName = "元";
            compSelfPFun.TimeName = "本年";
            compSelfPFun.WarningLevelName = "正常";
            compSelfPFun.CreateDate = currDate;

            FundManagement compSelfLastFun = new FundManagement();
            compSelfLastFun.OperationOrg = TransUtil.CompanyOpgGUID;
            compSelfLastFun.OperOrgName = "公司总部";
            compSelfLastFun.OrganizationLevel = "分公司";
            compSelfLastFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            compSelfLastFun.IndexName = "保证金余额";
            compSelfLastFun.MeasurementUnitName = "元";
            compSelfLastFun.TimeName = "上年";
            compSelfLastFun.WarningLevelName = "正常";
            compSelfLastFun.CreateDate = currDate;

            FundManagement compSelfCurrFun = new FundManagement();
            compSelfCurrFun.OperationOrg = TransUtil.CompanyOpgGUID;
            compSelfCurrFun.OperOrgName = "公司总部";
            compSelfCurrFun.OrganizationLevel = "分公司";
            compSelfCurrFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            compSelfCurrFun.IndexName = "保证金余额";
            compSelfCurrFun.MeasurementUnitName = "元";
            compSelfCurrFun.TimeName = "累计";
            compSelfCurrFun.WarningLevelName = "正常";
            compSelfCurrFun.CreateDate = currDate;
            #endregion

            #region 公司
            FundManagement compGFun = new FundManagement();
            compGFun.OperationOrg = TransUtil.CompanyOpgGUID;
            compGFun.OperOrgName = TransUtil.CompanyOpgName;
            compGFun.OrganizationLevel = "公司";
            compGFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            compGFun.IndexName = "收保证金";
            compGFun.MeasurementUnitName = "元";
            compGFun.TimeName = "本年";
            compGFun.WarningLevelName = "正常";
            compGFun.CreateDate = currDate;

            FundManagement compPFun = new FundManagement();
            compPFun.OperationOrg = TransUtil.CompanyOpgGUID;
            compPFun.OperOrgName = TransUtil.CompanyOpgName;
            compPFun.OrganizationLevel = "公司";
            compPFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            compPFun.IndexName = "付保证金";
            compPFun.MeasurementUnitName = "元";
            compPFun.TimeName = "本年";
            compPFun.WarningLevelName = "正常";
            compPFun.CreateDate = currDate;

            FundManagement compLastFun = new FundManagement();
            compLastFun.OperationOrg = TransUtil.CompanyOpgGUID;
            compLastFun.OperOrgName = TransUtil.CompanyOpgName;
            compLastFun.OrganizationLevel = "公司";
            compLastFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            compLastFun.IndexName = "保证金余额";
            compLastFun.MeasurementUnitName = "元";
            compLastFun.TimeName = "上年";
            compLastFun.WarningLevelName = "正常";
            compLastFun.CreateDate = currDate;

            FundManagement compCurrFun = new FundManagement();
            compCurrFun.OperationOrg = TransUtil.CompanyOpgGUID;
            compCurrFun.OperOrgName = TransUtil.CompanyOpgName;
            compCurrFun.OrganizationLevel = "公司";
            compCurrFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            compCurrFun.IndexName = "保证金余额";
            compCurrFun.MeasurementUnitName = "元";
            compCurrFun.TimeName = "累计";
            compCurrFun.WarningLevelName = "正常";
            compCurrFun.CreateDate = currDate;
            #endregion

            #region 取上年收付数据
            Hashtable projectHt = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select '付' querytype,t1.opgsyscode,sum(t2.money) money, t1.operationorg,t1.operationorgname,t1.thecustomerrelationinfo,t1.thecustomername "+
                                  " From thd_paymentmaster t1,thd_paymentdetail t2 where " +
                                  " t1.state=5 and t1.id=t2.parentid and t1.accounttitlename like '%保证金%' " + lastYearCondition +
                                  " and t1.thecustomerrelationinfo is not null group by t1.operationorg,t1.operationorgname,t1.opgsyscode,t1.thecustomerrelationinfo,t1.thecustomername " +
                                  " union all " +
                                  " select '收' querytype,t1.opgsyscode,sum(t2.money) money, t1.operationorg,t1.operationorgname,t1.thecustomerrelationinfo,t1.thecustomername "+
                                  " From thd_gatheringmaster t1,thd_gatheringdetail t2 where " +
                                  " t1.state=5 and t1.id=t2.parentid and t1.accounttitlename like '%保证金%' " + lastYearCondition +
                                  " and t1.thecustomerrelationinfo is not null group by t1.operationorg,t1.operationorgname,t1.opgsyscode,t1.thecustomerrelationinfo,t1.thecustomername ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string syscode = TransUtil.ToString(dataRow["opgsyscode"]);
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    string queryType = TransUtil.ToString(dataRow["querytype"]);
                    string operationorg = TransUtil.ToString(dataRow["operationorg"]);
                    string operationorgname = TransUtil.ToString(dataRow["operationorgname"]);
                    string thecustomerrelationinfo = TransUtil.ToString(dataRow["thecustomerrelationinfo"]);
                    string thecustomername = TransUtil.ToString(dataRow["thecustomername"]);

                    bool isSub = false;//是否分公司
                    foreach (FundManagement subFun in subLastList)
                    {
                        if (syscode.Contains(subFun.OpgSysCode))
                        {
                            //项目层次指标
                            string str = thecustomerrelationinfo + "-" + subFun.OperationOrg + "-保证金余额" + "-上年";
                            if (!projectHt.Contains(str))
                            {
                                FundManagementByProject funByProject = new FundManagementByProject();
                                funByProject.IndexName = "保证金余额";
                                funByProject.OperationOrg = subFun.OperationOrg;
                                funByProject.OperOrgName = subFun.OperOrgName;
                                funByProject.OpgSysCode = subFun.OpgSysCode;
                                funByProject.ProjectId = thecustomerrelationinfo;
                                funByProject.ProjectName = thecustomername;
                                funByProject.TimeName = "上年";
                                if (queryType == "收")
                                {
                                    funByProject.NumericalValue -= currMoney;
                                }
                                else
                                {
                                    funByProject.NumericalValue += currMoney;
                                }
                                projectHt.Add(str, funByProject);
                            }
                            else
                            {
                                FundManagementByProject funByProject = (FundManagementByProject)projectHt[str];
                                if (queryType == "收")
                                {
                                    funByProject.NumericalValue -= currMoney;
                                }
                                else
                                {
                                    funByProject.NumericalValue += currMoney;
                                }
                            }

                            //分公司层次
                            if (queryType == "收")
                            {
                                subFun.NumericalValue -= currMoney;
                            }
                            else
                            {
                                subFun.NumericalValue += currMoney;
                            }
                            isSub = true;
                        }
                    }

                    if (isSub == false && syscode.Contains(TransUtil.CompanyOpgSyscode))//公司总部上年
                    {
                        string str = thecustomerrelationinfo + "-" + TransUtil.CompanyOpgGUID + "-保证金余额" + "-上年";
                        if (!projectHt.Contains(str))
                        {
                            FundManagementByProject funByProject = new FundManagementByProject();
                            funByProject.IndexName = "保证金余额";
                            funByProject.OperationOrg = TransUtil.CompanyOpgGUID;
                            funByProject.OperOrgName = "公司总部";
                            funByProject.OpgSysCode = TransUtil.CompanyOpgSyscode;
                            funByProject.ProjectId = thecustomerrelationinfo;
                            funByProject.ProjectName = thecustomername;
                            funByProject.TimeName = "上年";
                            if (queryType == "收")
                            {
                                funByProject.NumericalValue -= currMoney;
                            }
                            else
                            {
                                funByProject.NumericalValue += currMoney;
                            }
                            projectHt.Add(str, funByProject);
                        }
                        else
                        {
                            FundManagementByProject funByProject = (FundManagementByProject)projectHt[str];
                            if (queryType == "收")
                            {
                                funByProject.NumericalValue -= currMoney;
                            }
                            else
                            {
                                funByProject.NumericalValue += currMoney;
                            }
                        }

                        //分公司层次
                        if (queryType == "收")
                        {
                            compSelfLastFun.NumericalValue -= currMoney;
                        }
                        else
                        {
                            compSelfLastFun.NumericalValue += currMoney;
                        }
                    }

                    if (queryType == "收")
                    {
                        compLastFun.NumericalValue -= currMoney;
                    }
                    else
                    {
                        compLastFun.NumericalValue += currMoney;
                    }
                }
            }
            #endregion

            #region 取本年收付数据
            command.CommandText = " select '付' querytype,t1.opgsyscode,sum(t2.money) money, t1.operationorg,t1.operationorgname,t1.thecustomerrelationinfo,t1.thecustomername "+
                                  " From thd_paymentmaster t1,thd_paymentdetail t2 where " +
                                  " t1.state=5 and t1.id=t2.parentid and t1.accounttitlename like '%保证金%' " + yearCondition +
                                  " and t1.thecustomerrelationinfo is not null group by t1.operationorg,t1.operationorgname,t1.opgsyscode,t1.thecustomerrelationinfo,t1.thecustomername " +
                                  " union all " +
                                  " select '收' querytype,t1.opgsyscode,sum(t2.money) money, t1.operationorg,t1.operationorgname,t1.thecustomerrelationinfo,t1.thecustomername "+
                                  " From thd_gatheringmaster t1,thd_gatheringdetail t2 where " +
                                  " t1.state=5 and t1.id=t2.parentid and t1.accounttitlename like '%保证金%' " + yearCondition +
                                  " and t1.thecustomerrelationinfo is not null group by t1.operationorg,t1.operationorgname,t1.opgsyscode,t1.thecustomerrelationinfo,t1.thecustomername ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string syscode = TransUtil.ToString(dataRow["opgsyscode"]);
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    string queryType = TransUtil.ToString(dataRow["querytype"]);
                    string operationorg = TransUtil.ToString(dataRow["operationorg"]);
                    string operationorgname = TransUtil.ToString(dataRow["operationorgname"]);
                    string thecustomerrelationinfo = TransUtil.ToString(dataRow["thecustomerrelationinfo"]);
                    string thecustomername = TransUtil.ToString(dataRow["thecustomername"]);

                    bool isSub = false;//是否分公司 付
                    foreach (FundManagement subFun in subGatheringList)
                    {
                        if (syscode.Contains(subFun.OpgSysCode))
                        {
                            if (queryType == "收")
                            {
                                //项目层次指标
                                string str = thecustomerrelationinfo + "-" + subFun.OperationOrg + "-收保证金" + "-本年";
                                if (!projectHt.Contains(str))
                                {
                                    FundManagementByProject funByProject = new FundManagementByProject();
                                    funByProject.IndexName = "收保证金";
                                    funByProject.OperationOrg = subFun.OperationOrg;
                                    funByProject.OperOrgName = subFun.OperOrgName;
                                    funByProject.OpgSysCode = subFun.OpgSysCode;
                                    funByProject.ProjectId = thecustomerrelationinfo;
                                    funByProject.ProjectName = thecustomername;
                                    funByProject.TimeName = "本年";
                                    funByProject.NumericalValue += currMoney;
                                    projectHt.Add(str, funByProject);
                                }
                                else
                                {
                                    FundManagementByProject funByProject = (FundManagementByProject)projectHt[str];
                                    funByProject.NumericalValue += currMoney;
                                }

                                //分公司层次
                                subFun.NumericalValue += currMoney;
                            }
                            isSub = true;
                        }
                    }
                    if (isSub == false && syscode.Contains(TransUtil.CompanyOpgSyscode))//公司总部收
                    {
                        if (queryType == "收")
                        {
                            //项目层次指标
                            string str = thecustomerrelationinfo + "-" + TransUtil.CompanyOpgGUID + "-收保证金" + "-本年";
                            if (!projectHt.Contains(str))
                            {
                                FundManagementByProject funByProject = new FundManagementByProject();
                                funByProject.IndexName = "收保证金";
                                funByProject.OperationOrg = TransUtil.CompanyOpgGUID;
                                funByProject.OperOrgName = "公司总部";
                                funByProject.OpgSysCode = TransUtil.CompanyOpgSyscode;
                                funByProject.ProjectId = thecustomerrelationinfo;
                                funByProject.ProjectName = thecustomername;
                                funByProject.TimeName = "本年";
                                funByProject.NumericalValue += currMoney;
                                projectHt.Add(str, funByProject);
                            }
                            else
                            {
                                FundManagementByProject funByProject = (FundManagementByProject)projectHt[str];
                                funByProject.NumericalValue += currMoney;
                            }
                            //分公司层次
                            compSelfGFun.NumericalValue += currMoney;
                        }
                    }
                    isSub = false;//付
                    foreach (FundManagement subFun in subPaymentList)
                    {
                        if (syscode.Contains(subFun.OpgSysCode))
                        {
                            if (queryType == "付")
                            {
                                //项目层次指标
                                string str = thecustomerrelationinfo + "-" + subFun.OperationOrg + "-付保证金" + "-本年";
                                if (!projectHt.Contains(str))
                                {
                                    FundManagementByProject funByProject = new FundManagementByProject();
                                    funByProject.IndexName = "付保证金";
                                    funByProject.OperationOrg = subFun.OperationOrg;
                                    funByProject.OperOrgName = subFun.OperOrgName;
                                    funByProject.OpgSysCode = subFun.OpgSysCode;
                                    funByProject.ProjectId = thecustomerrelationinfo;
                                    funByProject.ProjectName = thecustomername;
                                    funByProject.TimeName = "本年";
                                    funByProject.NumericalValue += currMoney;
                                    projectHt.Add(str, funByProject);
                                }
                                else
                                {
                                    FundManagementByProject funByProject = (FundManagementByProject)projectHt[str];
                                    funByProject.NumericalValue += currMoney;
                                }
                                //分公司层次
                                subFun.NumericalValue += currMoney;
                            }
                            isSub = true;
                        }
                    }

                    if (isSub == false && syscode.Contains(TransUtil.CompanyOpgSyscode))//公司总部付
                    {
                        if (queryType == "付")
                        {
                            //项目层次指标
                            string str = thecustomerrelationinfo + "-" + TransUtil.CompanyOpgGUID + "-付保证金" + "-本年";
                            if (!projectHt.Contains(str))
                            {
                                FundManagementByProject funByProject = new FundManagementByProject();
                                funByProject.IndexName = "付保证金";
                                funByProject.OperationOrg = TransUtil.CompanyOpgGUID;
                                funByProject.OperOrgName = "公司总部";
                                funByProject.OpgSysCode = TransUtil.CompanyOpgSyscode;
                                funByProject.ProjectId = thecustomerrelationinfo;
                                funByProject.ProjectName = thecustomername;
                                funByProject.TimeName = "本年";
                                funByProject.NumericalValue += currMoney;
                                projectHt.Add(str, funByProject);
                            }
                            else
                            {
                                FundManagementByProject funByProject = (FundManagementByProject)projectHt[str];
                                funByProject.NumericalValue += currMoney;
                            }
                            //分公司层次
                            compSelfPFun.NumericalValue += currMoney;
                        }
                    }

                    if (queryType == "收")
                    {
                        compGFun.NumericalValue += currMoney;
                    }
                    else
                    {
                        compPFun.NumericalValue += currMoney;
                    }
                }
            }
            #endregion

            //取今年余额(分公司层次)
            foreach (FundManagement subCurrFun in subCurrList)
            {
                foreach (FundManagement subGFun in subGatheringList)
                {
                    if (subCurrFun.OperationOrg == subGFun.OperationOrg)
                    {
                        subCurrFun.NumericalValue -= subGFun.NumericalValue;
                    }
                }
                foreach (FundManagement subPFun in subPaymentList)
                {
                    if (subCurrFun.OperationOrg == subPFun.OperationOrg)
                    {
                        subCurrFun.NumericalValue += subPFun.NumericalValue;
                    }
                }
                foreach (FundManagement subLFun in subLastList)
                {
                    if (subCurrFun.OperationOrg == subLFun.OperationOrg)
                    {
                        subCurrFun.NumericalValue += subLFun.NumericalValue;
                    }
                }
            }
            #region 取项目层次保证金余额
            //取今年余额(项目层次)
            Hashtable projectCurrFunHt = new Hashtable();
            foreach (FundManagementByProject funProject in projectHt.Values)
            {
                string str = funProject.ProjectId + "-" + funProject.OperationOrg;
                if (!projectCurrFunHt.Contains(str))
                {
                    FundManagementByProject currFunByProject = new FundManagementByProject();
                    currFunByProject.IndexName = "保证金余额";
                    currFunByProject.OperationOrg = funProject.OperationOrg;
                    currFunByProject.OperOrgName = funProject.OperOrgName;
                    currFunByProject.OpgSysCode = funProject.OpgSysCode;
                    currFunByProject.ProjectId = funProject.ProjectId;
                    currFunByProject.ProjectName = funProject.ProjectName;
                    currFunByProject.TimeName = "累计";
                    projectCurrFunHt.Add(str, currFunByProject);
                }
            }
            foreach (FundManagementByProject currFunByProject in projectCurrFunHt.Values)
            {
                foreach (FundManagementByProject funProject in projectHt.Values)
                {
                    if (currFunByProject.ProjectId == funProject.ProjectId && currFunByProject.OperationOrg == funProject.OperationOrg)
                    {
                        if (funProject.IndexName == "收保证金" && funProject.TimeName == "本年")
                        {
                            currFunByProject.NumericalValue -= funProject.NumericalValue;
                        }
                        else
                        {
                            currFunByProject.NumericalValue += funProject.NumericalValue;
                        }
                    }
                }
            }
            //汇总项目层次数据
            IList projectList = new ArrayList();
            foreach (FundManagementByProject funByProject in projectHt.Values)
            {
                projectList.Add(funByProject);
            }
            foreach (FundManagementByProject funByProject in projectCurrFunHt.Values)
            {
                projectList.Add(funByProject);
            }
            #endregion
            compCurrFun.NumericalValue = compLastFun.NumericalValue + compPFun.NumericalValue - compGFun.NumericalValue;
            compSelfCurrFun.NumericalValue = compSelfLastFun.NumericalValue + compSelfPFun.NumericalValue - compSelfGFun.NumericalValue;
            subLastList.Add(compSelfLastFun);
            subGatheringList.Add(compSelfGFun);
            subPaymentList.Add(compSelfPFun);
            subCurrList.Add(compSelfCurrFun);
            ht.Add("分公司上年余额", subLastList);
            ht.Add("分公司本年收", subGatheringList);
            ht.Add("分公司本年付", subPaymentList);
            ht.Add("分公司本年余额", subCurrList);
            ht.Add("公司上年余额", compLastFun);
            ht.Add("公司本年收", compGFun);
            ht.Add("公司本年付", compPFun);
            ht.Add("公司本年余额", compCurrFun);
            ht.Add("项目数据", projectList);
            return ht;
        }
        /// <summary>
        /// 查询项目账面信息(产值信息)(分包工程支出+人工费+材料费+机械费+其他直接费+合同毛利)
        /// </summary>
        private IList GetProjectProjectValueData(string financeCondition, string indirectCondtion)
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t.projectid, sum(t.money) money from ( " +
                                  " select t1.projectid,sum(t2.SubProjectPayout+t2.PersonCost+t2.MaterialCost+t2.MechanicalCost " +
                                  "   +t2.OtherDirectCost+t2.ContractGrossProfit) money From thd_financemultdatamaster t1,thd_financemultdatadetail t2 " +
                                  "  where t1.id=t2.parentid and t1.state=5 and t1.projectid is not null " + financeCondition + " group by t1.projectid " +
                                  " union all " +
                                  " select t1.projectid,sum(t2.money) money from thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 and t1.projectid is not null and t2.accounttitlecode like '%54010106%'  " + indirectCondtion + " group by t1.projectid " +
                                  " ) t group by t.projectid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FinanceMultDataMaster master = new FinanceMultDataMaster();
                    master.ProjectId = TransUtil.ToString(dataRow["projectid"]);
                    master.Temp1 = TransUtil.ToString(dataRow["money"]);
                    list.Add(master);
                }
            }
            return list;
        }
        /// <summary>
        /// 查询项目间接费用信息
        /// </summary>
        private IList GetProjectIndirectMoney(string condition)
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.projectid,sum(t2.money) money from thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 and t2.accounttitlecode like '%54010106%'  " + condition + " group by t1.projectid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GatheringMaster master = new GatheringMaster();
                    master.ProjectId = TransUtil.ToString(dataRow["projectid"]);
                    master.Temp1 = TransUtil.ToString(dataRow["money"]);
                    list.Add(master);
                }
            }
            return list;
        }

        /// <summary>
        /// 查询项目报量情况
        /// </summary>
        private Hashtable GetProjectOwnerQuantityInfo(string condition)
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.projectid,sum(t2.confirmMoney) money,sum(t2.acctgatheringmoney) acctmoney, nvl(max(t2.gatheringrate),0) grate from thd_ownerquantitymaster t1, " +
                                    " thd_ownerquantitydetail t2 where t1.id=t2.parentid and t1.state=5  " + condition + " group by t1.projectid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    OwnerQuantityMaster master = new OwnerQuantityMaster();
                    string projectID = TransUtil.ToString(dataRow["projectid"]);
                    master.Temp1 = TransUtil.ToString(dataRow["money"]);
                    master.Temp2 = TransUtil.ToString(dataRow["grate"]);
                    master.Temp3 = TransUtil.ToString(dataRow["acctmoney"]);//应收款金额
                    if (!ht.Contains(projectID))
                    {
                        ht.Add(projectID, master);
                    }
                }
            }
            return ht;
        }
        /// <summary>
        /// 查询项目资金占用情况
        /// 收款-付款+借款-费用
        /// </summary>
        private IList GetProjectLeftCashInfo(string condition)
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //项目信息
            command.CommandText = " select t.projectid, sum(t.money) money from ( " +
                                  " select t1.projectid,sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5 " + condition +
                                  " and t1.projectid is not null group by t1.projectid " +
                                  " union all " +
                                  " select t1.projectid,-sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5  " + condition +
                                  " and t1.projectid is not null group by t1.projectid " +
                                  " union all " +
                                  " select t1.projectid,sum(t2.money) money From thd_borrowedordermaster t1,thd_borrowedorderdetail t2 where t1.id=t2.parentid and t1.state=5 " + condition +
                                  " and t1.projectid is not null group by t1.projectid " +
                                  " union all " +
                                  " select t1.projectid,-sum(t1.money) money From thd_indirectcostmaster t1 where t1.state=5  " + condition +
                                  " and t1.projectid is not null group by t1.projectid ) t group by t.projectid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FundManagementByProject funByProject = new FundManagementByProject();
                    funByProject.ProjectId = TransUtil.ToString(dataRow["projectid"]);
                    funByProject.NumericalValue = TransUtil.ToDecimal(dataRow["money"]);
                    funByProject.IndexName = "资金存量";
                    funByProject.MeasurementUnitName = "元";
                    funByProject.OrganizationLevel = "项目";
                    funByProject.TimeName = "累计";
                    funByProject.WarningLevelName = "正常";
                    list.Add(funByProject);
                }
            }
            return list;
        }

        /// <summary>
        /// 查询分公司资金存量情况
        /// 收款-付款+借款(分公司)-费用
        /// </summary>
        private IList GetSubCompanyLeftCashInfo(string condition, IList opgList)
        {
            IList list = new ArrayList();
            foreach (OperationOrgInfo orgInfo in opgList)
            {
                FundManagement subFun = new FundManagement();
                subFun.OperationOrg = orgInfo.Id;
                subFun.OperOrgName = orgInfo.Name;
                subFun.OrganizationLevel = "分公司";
                subFun.OpgSysCode = orgInfo.SysCode;
                subFun.IndexName = "资金存量";
                subFun.MeasurementUnitName = "元";
                subFun.TimeName = "累计";
                subFun.WarningLevelName = "正常";
                list.Add(subFun);
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t.opgsyscode, sum(t.money) money from ( " +
                                  " select t1.opgsyscode,sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5 " + condition +
                                  " group by t1.opgsyscode " +
                                  " union all " +
                                  " select t1.opgsyscode,-sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5   " + condition +
                                  " group by t1.opgsyscode " +
                                  " union all " +
                                  " select t1.opgsyscode,sum(t2.money) money From thd_borrowedordermaster t1,thd_borrowedorderdetail t2 " +
                                  " where t1.id=t2.parentid and t1.projectid is null and t1.state=5 " + condition +
                                  " group by t1.opgsyscode " +
                                  " union all " +
                                  " select t1.opgsyscode,-sum(t1.money) money From thd_indirectcostmaster t1 where t1.state=5  " + condition +
                                  " group by t1.opgsyscode ) t group by t.opgsyscode ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string syscode = TransUtil.ToString(dataRow["opgsyscode"]);
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    foreach (FundManagement subFun in list)
                    {
                        if (syscode.Contains(subFun.OpgSysCode))
                        {
                            subFun.NumericalValue += currMoney;
                        }
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 查询公司资金存量情况
        /// 收款-付款-费用+保理+局借款
        /// </summary>
        private FundManagement GetCompanyLeftCashInfo(string condition, IList opgList)
        {
            decimal money = 0;
            string notInSubCondition = "";//剔除分公司以下层次的单据
            foreach (OperationOrgInfo orgInfo in opgList)
            {
                notInSubCondition += " and instr(t1.opgsyscode,'" + orgInfo.SysCode + "') = 0 ";
            }
            string inSubCondition = "";//分公司的单据
            foreach (OperationOrgInfo orgInfo in opgList)
            {
                if (inSubCondition == "")
                {
                    inSubCondition += " and ( instr(t1.opgsyscode,'" + orgInfo.SysCode + "') > 0 ";
                }
                else
                {
                    inSubCondition += " or instr(t1.opgsyscode,'" + orgInfo.SysCode + "') > 0 ";
                }
            }
            if (opgList != null && opgList.Count > 0)
            {
                inSubCondition += " ) ";
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //公司信息
            command.CommandText = " select sum(money) money from ( " +
                                  " select sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5 " + condition +
                                  " union all " +
                                  " select -sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5   " + condition +
                                  " union all " +
                                  " select -sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 and instr(t2.accounttitlecode,'2001') = 0 " + condition +
                                  " union all " +
                                  " select sum(t1.summoney) money from thd_factoringdatamaster t1  where  t1.state=5 " + condition +
                                  " union all " +
                                  " select sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 and instr(t2.accounttitlecode,'2001') > 0 " + condition + notInSubCondition;
            string inQuery = " union all " +
                                  " select sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 and instr(t2.accounttitlecode,'4104') > 0 " + condition + inSubCondition;
            if (inSubCondition != "")
            {
                command.CommandText += inQuery + " ) ";
            }
            else
            {
                command.CommandText += " ) ";
            }
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    money = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            FundManagement companyFun = new FundManagement();
            companyFun.OperationOrg = TransUtil.CompanyOpgGUID;
            companyFun.OperOrgName = TransUtil.CompanyOpgName;
            companyFun.OrganizationLevel = "公司";
            companyFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            companyFun.IndexName = "资金存量";
            companyFun.MeasurementUnitName = "元";
            companyFun.TimeName = "累计";
            companyFun.WarningLevelName = "正常";
            companyFun.NumericalValue = money;
            return companyFun;
        }
        /// <summary>
        /// 查询公司总部自身的资金存量情况
        /// 收款-付款-费用+保理+局借款-分公司借款+分公司货币上交
        /// </summary>
        private FundManagement GetCompanySelfLeftCashInfo(string condition, IList opgList)
        {
            decimal money = 0;
            string notInSubCondition = "";//排除分公司的单据
            foreach (OperationOrgInfo orgInfo in opgList)
            {
                notInSubCondition += " and instr(t1.opgsyscode,'" + orgInfo.SysCode + "') = 0 ";
            }
            string inSubCondition = "";//分公司的单据
            foreach (OperationOrgInfo orgInfo in opgList)
            {
                if (inSubCondition == "")
                {
                    inSubCondition += " and ( instr(t1.opgsyscode,'" + orgInfo.SysCode + "') > 0 ";
                }
                else
                {
                    inSubCondition += " or instr(t1.opgsyscode,'" + orgInfo.SysCode + "') > 0 ";
                }
            }
            if (opgList != null && opgList.Count > 0)
            {
                inSubCondition += " ) ";
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select sum(money) money from ( " +
                                  " select sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5 and t1.projectid is null " + condition + notInSubCondition +
                                  " union all " +
                                  " select -sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5  and t1.projectid is null " + condition + notInSubCondition +
                                  " union all " +
                                  " select -sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 and instr(t2.accounttitlecode,'2001') = 0 and t1.projectid is null " + condition + notInSubCondition +
                                  " union all " +
                                  " select sum(t1.summoney) money from thd_factoringdatamaster t1  where  t1.state=5 " + condition +
                                  " union all " +
                                  " select sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 and instr(t2.accounttitlecode,'2001') > 0 and t1.projectid is null " + condition + notInSubCondition +//局借款
                                  " union all " +
                                  " select -sum(t2.money) money From thd_borrowedordermaster t1,thd_borrowedorderdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 and t1.projectid is null " + condition + inSubCondition;//分公司借款
            string inQuery = " union all " +
                                  " select sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 and instr(t2.accounttitlecode,'4104') > 0 " + condition + inSubCondition;//分公司货币上交
            if (inSubCondition != "")
            {
                command.CommandText += inQuery + " ) ";
            }
            else
            {
                command.CommandText += " ) ";
            }
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    money = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            FundManagement companyFun = new FundManagement();
            companyFun.OperationOrg = TransUtil.CompanyOpgGUID;
            companyFun.OperOrgName = "公司总部";
            companyFun.OrganizationLevel = "分公司";
            companyFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            companyFun.IndexName = "资金存量";
            companyFun.MeasurementUnitName = "元";
            companyFun.TimeName = "累计";
            companyFun.WarningLevelName = "正常";
            companyFun.NumericalValue = money;
            return companyFun;
        }
        /// <summary>
        /// 查询分公司借款情况
        /// </summary>
        private IList GetSubCompanyBorrowInfo(string condition, IList opgList)
        {
            IList subList = new ArrayList();
            foreach (OperationOrgInfo orgInfo in opgList)
            {
                FundManagement subFun = new FundManagement();
                subFun.OperationOrg = orgInfo.Id;
                subFun.OperOrgName = orgInfo.Name;
                subFun.OrganizationLevel = "分公司";
                subFun.OpgSysCode = orgInfo.SysCode;
                subFun.IndexName = "借款";
                subFun.MeasurementUnitName = "元";
                subFun.TimeName = "累计";
                subFun.WarningLevelName = "正常";
                subList.Add(subFun);
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.opgsyscode,sum(t2.money) money From thd_borrowedordermaster t1,thd_borrowedorderdetail t2 " +
                                  " where t1.id=t2.parentid and t1.projectid is null and t1.state=5 " + condition +
                                  " group by t1.opgsyscode ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string syscode = TransUtil.ToString(dataRow["opgsyscode"]);
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    foreach (FundManagement subFun in subList)
                    {
                        if (syscode.Contains(subFun.OpgSysCode))
                        {
                            subFun.NumericalValue += currMoney;
                        }
                    }
                }
            }
            return subList;
        }

        private IList GetSubGatheringInfo(string condition, IList opgList)
        {
            IList subList = new ArrayList();
            foreach (OperationOrgInfo orgInfo in opgList)
            {
                FundManagement subFun = new FundManagement();
                subFun.OperationOrg = orgInfo.Id;
                subFun.OperOrgName = orgInfo.Name;
                subFun.OrganizationLevel = "分公司";
                subFun.OpgSysCode = orgInfo.SysCode;
                subFun.IndexName = "收款";
                subFun.MeasurementUnitName = "元";
                subFun.WarningLevelName = "正常";
                subList.Add(subFun);
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.opgsyscode,sum(t1.summoney) money From thd_gatheringmaster t1 where t1.ifprojectmoney=0 and t1.projectid is null "+
                                    " and t1.state=5 " + condition + " group by t1.opgsyscode ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string syscode = TransUtil.ToString(dataRow["opgsyscode"]);
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    foreach (FundManagement subFun in subList)
                    {
                        if (syscode.Contains(subFun.OpgSysCode))
                        {
                            subFun.NumericalValue += currMoney;
                        }
                    }
                }
            }
            return subList;
        }
        /// <summary>
        /// 查询公司借款情况
        /// </summary>
        private FundManagement GetCompanyBorrowInfo(string condition, IList opgList)
        {
            decimal money = 0;
            string notInSubCondition = "";//剔除分公司以下层次的单据
            foreach (OperationOrgInfo orgInfo in opgList)
            {
                notInSubCondition += " and instr(t1.opgsyscode,'" + orgInfo.SysCode + "') = 0 ";
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select sum(t2.money) money from thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                    " where t1.id=t2.parentid and t1.state=5 and instr(t2.accounttitlecode,'2001') > 0 " + condition + notInSubCondition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    money = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            FundManagement companyFun = new FundManagement();
            companyFun.OperationOrg = TransUtil.CompanyOpgGUID;
            companyFun.OperOrgName = TransUtil.CompanyOpgName;
            companyFun.OrganizationLevel = "公司";
            companyFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            companyFun.IndexName = "借款";
            companyFun.MeasurementUnitName = "元";
            companyFun.TimeName = "累计";
            companyFun.WarningLevelName = "正常";
            companyFun.NumericalValue = money;
            return companyFun;
        }
        /// <summary>
        /// 查询公司保理情况
        /// </summary>
        private FundManagement GetCompanyFactoringInfo(string condition)
        {
            decimal money = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select sum(t1.summoney) money from thd_factoringdatamaster t1  where  t1.state=5 " + condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    money = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            FundManagement companyFun = new FundManagement();
            companyFun.OperationOrg = TransUtil.CompanyOpgGUID;
            companyFun.OperOrgName = TransUtil.CompanyOpgName;
            companyFun.OrganizationLevel = "公司";
            companyFun.OpgSysCode = TransUtil.CompanyOpgSyscode;
            companyFun.IndexName = "保理";
            companyFun.MeasurementUnitName = "元";
            companyFun.TimeName = "累计";
            companyFun.WarningLevelName = "正常";
            companyFun.NumericalValue = money;
            return companyFun;
        }
        /// <summary>
        /// 查询项目日均资金存量
        /// <param name="addProjectLeftCashList">当日项目资金存量</param>
        /// </summary>
        private IList GetProjectDayAvgMoney(string condition, IList addProjectLeftCashList)
        {
            IList list = new ArrayList();
            Hashtable projectHt = new Hashtable();
            foreach (FundManagementByProject funByProject in addProjectLeftCashList)
            {
                if (!projectHt.Contains(funByProject.ProjectId))
                {
                    projectHt.Add(funByProject.ProjectId, funByProject.NumericalValue);
                }
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //项目信息
            command.CommandText = " select t1.projectid,count(*) count,sum(t1.numericalvalue) money From thd_fundmanagebyproject t1 where " +
                                  " t1.timename='累计' and t1.organizationlevel='项目' and t1.indexname='资金存量' and t1.warninglevelname='正常' " + condition +
                                  " group by t1.projectid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string projectID = TransUtil.ToString(dataRow["projectid"]);
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    decimal currCount = TransUtil.ToDecimal(dataRow["count"]);

                    FundManagementByProject fund = new FundManagementByProject();
                    fund.ProjectId = projectID;
                    fund.OrganizationLevel = "项目";
                    fund.IndexName = "资金存量";
                    fund.MeasurementUnitName = "元";
                    fund.TimeName = "日均";
                    fund.WarningLevelName = "正常";
                    if (projectHt.Contains(projectID))
                    {
                        fund.NumericalValue = decimal.Round((TransUtil.ToDecimal(projectHt[projectID]) + currMoney) / (currCount + 1), 2);
                    }
                    else
                    {
                        fund.NumericalValue = currMoney;
                    }
                    list.Add(fund);
                }
            }
            if (list.Count == 0)
            {
                foreach (FundManagementByProject funByProject in addProjectLeftCashList)
                {
                    FundManagementByProject currFundByProject = new FundManagementByProject();
                    currFundByProject.ProjectId = funByProject.ProjectId;
                    currFundByProject.OrganizationLevel = "项目";
                    currFundByProject.IndexName = "资金存量";
                    currFundByProject.MeasurementUnitName = "元";
                    currFundByProject.TimeName = "日均";
                    currFundByProject.WarningLevelName = "正常";
                    currFundByProject.NumericalValue = funByProject.NumericalValue;
                    list.Add(currFundByProject);
                }
            }
            return list;
        }
        /// <summary>
        /// 查询分公司日均资金存量
        /// </summary>
        private IList GetSubCompanyDayAvgMoney(string condition, IList addSubLeftCashList, IList opgList)
        {
            IList list = new ArrayList();
            Hashtable subHt = new Hashtable();
            foreach (FundManagement fund in addSubLeftCashList)
            {
                if (!subHt.Contains(fund.OperationOrg))
                {
                    subHt.Add(fund.OperationOrg, fund.NumericalValue);
                }
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.OperationOrg,count(*) count,sum(t1.numericalvalue) money From thd_fundmanagement t1 where " +
                                  " t1.timename='累计' and t1.organizationlevel='分公司' and t1.operorgname != '公司总部' and t1.indexname='资金存量' and t1.warninglevelname='正常' " + condition +
                                  " group by t1.OperationOrg ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string operationOrg = TransUtil.ToString(dataRow["OperationOrg"]);
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    decimal currCount = TransUtil.ToDecimal(dataRow["count"]);

                    FundManagement fund = new FundManagement();
                    fund.OperationOrg = operationOrg;
                    fund.OrganizationLevel = "分公司";
                    fund.IndexName = "资金存量";
                    fund.MeasurementUnitName = "元";
                    fund.TimeName = "日均";
                    fund.WarningLevelName = "正常";
                    foreach (OperationOrgInfo orgInfo in opgList)
                    {
                        if (orgInfo.Id == operationOrg)
                        {
                            fund.OperOrgName = orgInfo.Name;
                            fund.OpgSysCode = orgInfo.SysCode;
                        }
                    }
                    if (subHt.Contains(operationOrg))
                    {
                        fund.NumericalValue = decimal.Round((TransUtil.ToDecimal(subHt[operationOrg]) + currMoney) / (currCount + 1), 2);
                    }
                    else
                    {
                        fund.NumericalValue = currMoney;
                    }
                    list.Add(fund);
                }
            }
            if (list.Count == 0)
            {
                foreach (FundManagement fund in addSubLeftCashList)
                {
                    FundManagement currFund = new FundManagement();
                    currFund.OperationOrg = fund.OperationOrg;
                    currFund.OrganizationLevel = "分公司";
                    currFund.IndexName = "资金存量";
                    currFund.MeasurementUnitName = "元";
                    currFund.TimeName = "日均";
                    currFund.WarningLevelName = "正常";
                    currFund.NumericalValue = fund.NumericalValue;
                    currFund.OperOrgName = fund.OperOrgName;
                    currFund.OpgSysCode = fund.OpgSysCode;
                    list.Add(currFund);
                }
            }
            return list;
        }
        /// <summary>
        /// 查询公司日均资金存量
        /// </summary>
        private FundManagement GetCompanyDayAvgMoney(string condition, FundManagement compLeftCashFund)
        {
            FundManagement companyFun = new FundManagement();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select count(*) count,sum(t1.numericalvalue) money From thd_fundmanagement t1 where " +
                                  " t1.timename='累计' and t1.organizationlevel='公司' and t1.indexname='资金存量' and t1.warninglevelname='正常' " + condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    decimal currCount = TransUtil.ToDecimal(dataRow["count"]);

                    companyFun.OperationOrg = compLeftCashFund.OperationOrg;
                    companyFun.OperOrgName = compLeftCashFund.OperOrgName;
                    companyFun.OpgSysCode = compLeftCashFund.OpgSysCode;
                    companyFun.OrganizationLevel = "公司";
                    companyFun.IndexName = "资金存量";
                    companyFun.MeasurementUnitName = "元";
                    companyFun.TimeName = "日均";
                    companyFun.WarningLevelName = "正常";
                    companyFun.NumericalValue = decimal.Round((compLeftCashFund.NumericalValue + currMoney) / (currCount + 1), 2);
                }
            }
            if (companyFun.NumericalValue == 0)
            {
                companyFun.OperationOrg = compLeftCashFund.OperationOrg;
                companyFun.OperOrgName = compLeftCashFund.OperOrgName;
                companyFun.OpgSysCode = compLeftCashFund.OpgSysCode;
                companyFun.OrganizationLevel = "公司";
                companyFun.IndexName = "资金存量";
                companyFun.MeasurementUnitName = "元";
                companyFun.TimeName = "日均";
                companyFun.WarningLevelName = "正常";
                companyFun.NumericalValue = compLeftCashFund.NumericalValue;
            }
            return companyFun;
        }
        /// <summary>
        /// 查询公司(本部)日均资金存量
        /// </summary>
        private FundManagement GetCompanySelfDayAvgMoney(string condition, FundManagement compSelfLeftCashFund)
        {
            FundManagement companyFun = new FundManagement();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select count(*) count,sum(t1.numericalvalue) money From thd_fundmanagement t1 where " +
                                  " t1.timename='累计' and t1.operorgname ='公司总部' and t1.organizationlevel='分公司' and t1.indexname='资金存量' and t1.warninglevelname='正常' " + condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    decimal currCount = TransUtil.ToDecimal(dataRow["count"]);

                    companyFun.OperationOrg = compSelfLeftCashFund.OperationOrg;
                    companyFun.OperOrgName = compSelfLeftCashFund.OperOrgName;
                    companyFun.OpgSysCode = compSelfLeftCashFund.OpgSysCode;
                    companyFun.OrganizationLevel = "分公司";
                    companyFun.IndexName = "资金存量";
                    companyFun.MeasurementUnitName = "元";
                    companyFun.TimeName = "日均";
                    companyFun.WarningLevelName = "正常";
                    companyFun.NumericalValue = decimal.Round((compSelfLeftCashFund.NumericalValue + currMoney) / (currCount + 1), 2);
                }
            }
            if (companyFun.NumericalValue == 0)
            {
                companyFun.OperationOrg = compSelfLeftCashFund.OperationOrg;
                companyFun.OperOrgName = compSelfLeftCashFund.OperOrgName;
                companyFun.OpgSysCode = compSelfLeftCashFund.OpgSysCode;
                companyFun.OrganizationLevel = "分公司";
                companyFun.IndexName = "资金存量";
                companyFun.MeasurementUnitName = "元";
                companyFun.TimeName = "日均";
                companyFun.WarningLevelName = "正常";
                companyFun.NumericalValue = compSelfLeftCashFund.NumericalValue;
            }
            return companyFun;
        }

        /// <summary>
        /// 查询项目月均资金存量
        /// <param name="addProjectLeftCashList">当日项目资金存量</param>
        /// </summary>
        private IList GetProjectMonthAvgMoney(string condition, IList addProjectLeftCashList)
        {
            IList list = new ArrayList();
            Hashtable projectHt = new Hashtable();
            foreach (FundManagementByProject funByProject in addProjectLeftCashList)
            {
                if (!projectHt.Contains(funByProject.ProjectId))
                {
                    projectHt.Add(funByProject.ProjectId, funByProject.NumericalValue);
                }
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //项目信息
            command.CommandText = " select t1.projectid,count(*) count,sum(t1.numericalvalue) money From thd_fundmanagebyproject t1 where " +
                                  " t1.timename='月末' and t1.organizationlevel='项目' and t1.indexname='资金存量' and t1.warninglevelname='正常' " + condition +
                                  " group by t1.projectid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string projectID = TransUtil.ToString(dataRow["projectid"]);
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    decimal currCount = TransUtil.ToDecimal(dataRow["count"]);

                    FundManagementByProject fund = new FundManagementByProject();
                    fund.ProjectId = projectID;
                    fund.OrganizationLevel = "项目";
                    fund.IndexName = "资金存量";
                    fund.MeasurementUnitName = "元";
                    fund.TimeName = "月均";
                    fund.WarningLevelName = "正常";
                    if (projectHt.Contains(projectID))
                    {
                        fund.NumericalValue = decimal.Round((TransUtil.ToDecimal(projectHt[projectID]) + currMoney) / (currCount + 1), 2);
                    }
                    else
                    {
                        fund.NumericalValue = currMoney;
                    }
                    list.Add(fund);
                }
            }
            if (list.Count == 0)
            {
                foreach (FundManagementByProject funByProject in addProjectLeftCashList)
                {
                    FundManagementByProject currFundByProject = new FundManagementByProject();
                    currFundByProject.ProjectId = funByProject.ProjectId;
                    currFundByProject.OrganizationLevel = "项目";
                    currFundByProject.IndexName = "资金存量";
                    currFundByProject.MeasurementUnitName = "元";
                    currFundByProject.TimeName = "月均";
                    currFundByProject.WarningLevelName = "正常";
                    currFundByProject.NumericalValue = funByProject.NumericalValue;
                    list.Add(currFundByProject);
                }
            }
            return list;
        }
        /// <summary>
        /// 查询分公司月均资金存量
        /// </summary>
        private IList GetSubCompanyMonthAvgMoney(string condition, IList addSubLeftCashList, IList opgList)
        {
            IList list = new ArrayList();
            Hashtable subHt = new Hashtable();
            foreach (FundManagement fund in addSubLeftCashList)
            {
                if (!subHt.Contains(fund.OperationOrg))
                {
                    subHt.Add(fund.OperationOrg, fund.NumericalValue);
                }
            }
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.OperationOrg,count(*) count,sum(t1.numericalvalue) money From thd_fundmanagement t1 where " +
                                  " t1.timename='月末' and t1.organizationlevel='分公司' and t1.indexname='资金存量' and t1.warninglevelname='正常' " + condition +
                                  " group by t1.OperationOrg ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string operationOrg = TransUtil.ToString(dataRow["OperationOrg"]);
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    decimal currCount = TransUtil.ToDecimal(dataRow["count"]);

                    FundManagement fund = new FundManagement();
                    fund.OperationOrg = operationOrg;
                    fund.OrganizationLevel = "分公司";
                    fund.IndexName = "资金存量";
                    fund.MeasurementUnitName = "元";
                    fund.TimeName = "月均";
                    fund.WarningLevelName = "正常";
                    foreach (OperationOrgInfo orgInfo in opgList)
                    {
                        if (orgInfo.Id == operationOrg)
                        {
                            fund.OperOrgName = orgInfo.Name;
                            fund.OpgSysCode = orgInfo.SysCode;
                        }
                    }
                    if (subHt.Contains(operationOrg))
                    {
                        fund.NumericalValue = decimal.Round((TransUtil.ToDecimal(subHt[operationOrg]) + currMoney) / (currCount + 1), 2);
                    }
                    else
                    {
                        fund.NumericalValue = currMoney;
                    }
                    list.Add(fund);
                }
            }
            if (list.Count == 0)
            {
                foreach (FundManagement fund in addSubLeftCashList)
                {
                    FundManagement currFund = new FundManagement();
                    currFund.OperationOrg = fund.OperationOrg;
                    currFund.OrganizationLevel = "分公司";
                    currFund.IndexName = "资金存量";
                    currFund.MeasurementUnitName = "元";
                    currFund.TimeName = "月均";
                    currFund.WarningLevelName = "正常";
                    currFund.NumericalValue = fund.NumericalValue;
                    currFund.OperOrgName = fund.OperOrgName;
                    currFund.OpgSysCode = fund.OpgSysCode;
                    list.Add(currFund);
                }
            }
            return list;
        }
        /// <summary>
        /// 查询公司月均资金存量
        /// </summary>
        private FundManagement GetCompanyMonthAvgMoney(string condition, FundManagement compLeftCashFund)
        {
            FundManagement companyFun = new FundManagement();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select count(*) count,sum(t1.numericalvalue) money From thd_fundmanagement t1 where " +
                                  " t1.timename='月末' and t1.organizationlevel='公司' and t1.indexname='资金存量' and t1.warninglevelname='正常' " + condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    decimal currMoney = TransUtil.ToDecimal(dataRow["money"]);
                    decimal currCount = TransUtil.ToDecimal(dataRow["count"]);

                    companyFun.OperationOrg = compLeftCashFund.OperationOrg;
                    companyFun.OperOrgName = compLeftCashFund.OperOrgName;
                    companyFun.OpgSysCode = compLeftCashFund.OpgSysCode;
                    companyFun.OrganizationLevel = "公司";
                    companyFun.IndexName = "资金存量";
                    companyFun.MeasurementUnitName = "元";
                    companyFun.TimeName = "月均";
                    companyFun.WarningLevelName = "正常";
                    companyFun.NumericalValue = decimal.Round((compLeftCashFund.NumericalValue + currMoney) / (currCount + 1), 2);
                }
            }
            if (companyFun.NumericalValue == 0)
            {
                companyFun.OperationOrg = compLeftCashFund.OperationOrg;
                companyFun.OperOrgName = compLeftCashFund.OperOrgName;
                companyFun.OpgSysCode = compLeftCashFund.OpgSysCode;
                companyFun.OrganizationLevel = "公司";
                companyFun.IndexName = "资金存量";
                companyFun.MeasurementUnitName = "元";
                companyFun.TimeName = "月均";
                companyFun.WarningLevelName = "正常";
                companyFun.NumericalValue = compLeftCashFund.NumericalValue;
            }
            return companyFun;
        }
        /// <summary>
        /// 查询项目应收账款拖欠预警信息(作废)
        /// <param name="addProjectNoGathWarnList">应收账款拖欠项目集合</param>
        /// </summary>
        private IList GetProjectNoGathWarnList(DateTime currDate, IList addProjectNoGathWarnList)
        {
            IList list = new ArrayList();
            IList existList = new ArrayList();
            Hashtable smallDateHt = new Hashtable();//小于预警值集合
            Hashtable bigDateHt = new Hashtable();//大于预警值集合
            Hashtable projectValue = new Hashtable();
            string warn500Condition = "('0'";//500万元到1000万元以内
            string warn1000Condition = "('0'";//1000万元2000万元以内
            string warn2000Condition = "('0'";//2000万元及以上
            foreach (FundManagementByProject funByProject in addProjectNoGathWarnList)
            {
                if (!projectValue.Contains(funByProject.ProjectId))
                {
                    projectValue.Add(funByProject.ProjectId, funByProject);
                }
                if (funByProject.NumericalValue >= 5000000)
                {
                    warn500Condition += " ,'" + funByProject.ProjectId + "' ";
                }
                if (funByProject.NumericalValue >= 10000000 && funByProject.NumericalValue < 20000000)
                {
                    warn1000Condition += " ,'" + funByProject.ProjectId + "' ";
                }
                if (funByProject.NumericalValue >= 20000000)
                {
                    warn2000Condition += " ,'" + funByProject.ProjectId + "' ";
                }
            }
            warn500Condition += ")";
            warn1000Condition += ")";
            warn2000Condition += ")";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //500万以上
            command.CommandText = " select 0 querytype,t1.projectid,max(t1.createdate) createdate From thd_fundmanagebyproject t1 where " +
                                  " t1.timename='累计' and t1.organizationlevel='项目' and t1.indexname='应收未收款' and t1.warninglevelname='正常' " +
                                  " and t1.numericalvalue< 5000000 and t1.createdate < to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid in "
                                  + warn500Condition + " group by t1.projectid " +
                                  " union all " +
                                  " select 1 querytype,t1.projectid,min(t1.createdate) createdate From thd_fundmanagebyproject t1 where " +
                                  " t1.timename='累计' and t1.organizationlevel='项目' and t1.indexname='应收未收款' and t1.warninglevelname='正常' " +
                                  " and t1.numericalvalue >= 5000000 and t1.createdate < to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') and t1.projectid in "
                                  + warn500Condition + " group by t1.projectid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string projectID = TransUtil.ToString(dataRow["projectid"]);
                    DateTime maxDate = TransUtil.ToDateTime(dataRow["createdate"]);
                    int queryType = TransUtil.ToInt(dataRow["querytype"]);
                    if (queryType == 0)
                    {
                        smallDateHt.Add(projectID, maxDate);
                    }
                    if (queryType == 1)
                    {
                        bigDateHt.Add(projectID, maxDate);
                    }
                }
            }
            //判断预警范围
            foreach (string projectID in bigDateHt.Keys)
            {
                DateTime maxDate = (DateTime)bigDateHt[projectID];
                if (smallDateHt.Contains(projectID))
                {
                    maxDate = (DateTime)smallDateHt[projectID];
                }
                int days = (currDate - maxDate).Days;
                if (days > 90)
                {
                    FundManagementByProject fund = new FundManagementByProject();
                    FundManagementByProject transFund = projectValue[projectID] as FundManagementByProject;
                    if (transFund != null)
                    {
                        fund.OperationOrg = transFund.OperationOrg;
                        fund.OperOrgName = transFund.OperOrgName;
                        fund.OpgSysCode = transFund.OpgSysCode;
                        fund.ProjectName = transFund.ProjectName;
                        fund.NumericalValue = transFund.NumericalValue;
                    }
                    fund.ProjectId = projectID;
                    fund.OrganizationLevel = "项目";
                    fund.IndexName = "应收未收款";
                    fund.MeasurementUnitName = "元";
                    fund.TimeName = "累计";
                    fund.WarningLevelName = "红色预警";
                    fund.CreateDate = currDate;
                    list.Add(fund);
                    existList.Add(fund.ProjectId);
                }
            }

            bigDateHt.Clear();
            smallDateHt.Clear();
            if (warn2000Condition != "('0')")
            {
                command.CommandText = " select 0 querytype,t1.projectid,max(t1.createdate) createdate From thd_fundmanagebyproject t1 where " +
                                  " t1.timename='累计' and t1.organizationlevel='项目' and t1.indexname='应收未收款' and t1.warninglevelname='正常' " +
                                  " and t1.numericalvalue < 20000000 and t1.createdate < to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                  " and t1.projectid in " + warn2000Condition + " group by t1.projectid " +
                                  " union all " +
                                  " select 1 querytype,t1.projectid,min(t1.createdate) createdate From thd_fundmanagebyproject t1 where " +
                                  " t1.timename='累计' and t1.organizationlevel='项目' and t1.indexname='应收未收款' and t1.warninglevelname='正常' " +
                                  " and t1.numericalvalue >= 20000000 and t1.createdate < to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                  " and t1.projectid in " + warn2000Condition + " group by t1.projectid ";
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        string projectID = TransUtil.ToString(dataRow["projectid"]);
                        DateTime maxDate = TransUtil.ToDateTime(dataRow["createdate"]);
                        int queryType = TransUtil.ToInt(dataRow["querytype"]);
                        if (queryType == 0)
                        {
                            smallDateHt.Add(projectID, maxDate);
                        }
                        if (queryType == 1)
                        {
                            bigDateHt.Add(projectID, maxDate);
                        }
                    }
                }
            }
            //判断预警范围
            foreach (string projectID in bigDateHt.Keys)
            {
                DateTime maxDate = (DateTime)bigDateHt[projectID];
                if (smallDateHt.Contains(projectID))
                {
                    maxDate = (DateTime)smallDateHt[projectID];
                }
                int days = (currDate - maxDate).Days;
                if (days > 30 && existList.Contains(projectID) == false)
                {
                    FundManagementByProject fund = new FundManagementByProject();
                    FundManagementByProject transFund = projectValue[projectID] as FundManagementByProject;
                    if (transFund != null)
                    {
                        fund.OperationOrg = transFund.OperationOrg;
                        fund.OperOrgName = transFund.OperOrgName;
                        fund.OpgSysCode = transFund.OpgSysCode;
                        fund.ProjectName = transFund.ProjectName;
                        fund.NumericalValue = transFund.NumericalValue;
                    }
                    fund.ProjectId = projectID;
                    fund.OrganizationLevel = "项目";
                    fund.IndexName = "应收未收款";
                    fund.MeasurementUnitName = "元";
                    fund.TimeName = "累计";

                    fund.CreateDate = currDate;

                    if (days >= 60)
                    {
                        fund.WarningLevelName = "红色预警";
                    }
                    if (days >= 30 && days < 60)
                    {
                        fund.WarningLevelName = "橙色预警";
                    }
                    list.Add(fund);
                    existList.Add(fund.ProjectId);
                }
            }
            bigDateHt.Clear();
            smallDateHt.Clear();

            if (warn1000Condition != "('0')")
            {
                command.CommandText = " select 0 querytype,t1.projectid,max(t1.createdate) createdate From thd_fundmanagebyproject t1 where " +
                                  " t1.timename='累计' and t1.organizationlevel='项目' and t1.indexname='应收未收款' and t1.warninglevelname='正常' " +
                                  " and (t1.numericalvalue < 10000000 or t1.numericalvalue > 20000000) and t1.createdate < to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                  " and t1.projectid in " + warn1000Condition + " group by t1.projectid " +
                                  " union all " +
                                  " select 1 querytype,t1.projectid,min(t1.createdate) createdate From thd_fundmanagebyproject t1 where " +
                                  " t1.timename='累计' and t1.organizationlevel='项目' and t1.indexname='应收未收款' and t1.warninglevelname='正常' " +
                                  " and t1.numericalvalue >= 10000000 and t1.numericalvalue < 20000000 and t1.createdate < to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                  " and t1.projectid in " + warn1000Condition + " group by t1.projectid ";
                dataReader = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                if (ds != null)
                {
                    DataTable dataTable = ds.Tables[0];
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        string projectID = TransUtil.ToString(dataRow["projectid"]);
                        DateTime maxDate = TransUtil.ToDateTime(dataRow["createdate"]);
                        int queryType = TransUtil.ToInt(dataRow["querytype"]);
                        if (queryType == 0)
                        {
                            smallDateHt.Add(projectID, maxDate);
                        }
                        if (queryType == 1)
                        {
                            bigDateHt.Add(projectID, maxDate);
                        }
                    }
                }
            }
            //判断预警范围
            foreach (string projectID in bigDateHt.Keys)
            {
                DateTime maxDate = (DateTime)bigDateHt[projectID];
                if (smallDateHt.Contains(projectID))
                {
                    maxDate = (DateTime)smallDateHt[projectID];
                }
                int days = (currDate - maxDate).Days;
                if (days > 30 && existList.Contains(projectID) == false)
                {
                    FundManagementByProject fund = new FundManagementByProject();
                    FundManagementByProject transFund = projectValue[projectID] as FundManagementByProject;
                    if (transFund != null)
                    {
                        fund.OperationOrg = transFund.OperationOrg;
                        fund.OperOrgName = transFund.OperOrgName;
                        fund.OpgSysCode = transFund.OpgSysCode;
                        fund.ProjectName = transFund.ProjectName;
                        fund.NumericalValue = transFund.NumericalValue;
                    }
                    fund.ProjectId = projectID;
                    fund.OrganizationLevel = "项目";
                    fund.IndexName = "应收未收款";
                    fund.MeasurementUnitName = "元";
                    fund.TimeName = "累计";

                    fund.CreateDate = currDate;

                    if (days >= 60)
                    {
                        fund.WarningLevelName = "橙色预警";
                    }
                    if (days >= 30 && days < 60)
                    {
                        fund.WarningLevelName = "黄色预警";
                    }
                    list.Add(fund);
                    existList.Add(fund.ProjectId);
                }
            }
            return list;
        }
        /// <summary>
        /// 查询内部项目的收款信息(工程款112201)
        /// </summary>
        private decimal GetCompanyInProjectGatheringMainMoney(string condition)
        {
            decimal inMoney = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select sum(t1.summoney) money From thd_gatheringmaster t1,resconfig t2 where " +
                                  " t1.projectid=t2.id and t2.ifsync=1 and t1.accounttitlecode ='112201' and t1.ifprojectmoney=0 and t1.state=5 " + condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    inMoney = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            return inMoney;
        }
        /// <summary>
        /// 查询内部项目的收款信息
        /// </summary>
        private decimal GetCompanyInProjectGatheringMoney(string condition)
        {
            decimal inMoney = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select sum(t1.summoney) money From thd_gatheringmaster t1,resconfig t2 where " +
                                  " t1.projectid=t2.id and t2.ifsync=1 and t1.ifprojectmoney=0 and t1.state=5 " + condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    inMoney = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            return inMoney;
        }
        /// <summary>
        /// 查询内部项目的账面信息(产值信息)(分包工程支出+人工费+材料费+机械费+其他直接费+合同毛利)
        /// </summary>
        private decimal GetCompanyInProjectProjectValueMoney(string financeCondition, string indirectCondtion)
        {
            decimal inMoney = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select sum(t2.SubProjectPayout+t2.PersonCost+t2.MaterialCost+t2.MechanicalCost " +
                                  "   +t2.OtherDirectCost+t2.ContractGrossProfit) money From thd_financemultdatamaster t1,thd_financemultdatadetail t2,resconfig t3 " +
                                  " where t1.id=t2.parentid and t1.projectid=t3.id and t3.ifsync=1 and t1.state=5 and t1.projectid is not null " + financeCondition +
                                  " union all " +
                                  " select sum(t2.money) money from thd_indirectcostmaster t1,thd_indirectcostdetail t2,resconfig t3 " +
                                  " where t1.id=t2.parentid and t1.projectid=t3.id and t3.ifsync=1 and t1.state=5 and t1.projectid is not null and t2.accounttitlecode like '%54010106%' " + indirectCondtion;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    inMoney += TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            return inMoney;
        }
        /// <summary>
        /// 查询项目应收账款拖欠预警信息
        /// <param name="addProjectNoGathWarnList">应收账款拖欠项目集合</param>
        /// 临界点日期,临界点应收未收款
        /// </summary>
        private IList GetProjectNoGatheringWarnList(DateTime currDate, IList addProjectNoGathWarnList)
        {
            IList list = new ArrayList();
            IEnumerable<OwnerQuantityDetail> ownerDtlEnumList = null;
            IEnumerable<GatheringMaster> gMasterEnumList = null;
            IList ownerList = new ArrayList();
            IList gatherList = new ArrayList();
            DateTime minDate = currDate.AddDays(-100);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //审量信息、合同应收金额
            command.CommandText = " select t1.projectid,t2.confirmdate createdate,sum(t2.acctgatheringmoney) acctmoney, sum(t2.confirmmoney) money,max(t2.gatheringrate) grate " +
                                  " from thd_ownerquantitymaster t1,thd_ownerquantitydetail t2 where t1.id=t2.parentid and t1.state=5 " +
                                  " group by t1.projectid,t2.confirmdate";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    OwnerQuantityDetail gDetail = new OwnerQuantityDetail();
                    gDetail.TempData1 = TransUtil.ToString(dataRow["projectid"]);
                    gDetail.ConfirmDate = TransUtil.ToDateTime(dataRow["createdate"]);
                    gDetail.ConfirmMoney = TransUtil.ToDecimal(dataRow["money"]);
                    gDetail.GatheringRate = TransUtil.ToDecimal(dataRow["grate"]);
                    gDetail.AcctGatheringMoney = TransUtil.ToDecimal(dataRow["acctmoney"]);
                    ownerList.Add(gDetail);
                }
            }
            ownerDtlEnumList = ownerList.OfType<OwnerQuantityDetail>();
            //收款信息
            command.CommandText = " select t1.projectid,t1.createdate,sum(t1.summoney) money " +
                                  " from thd_gatheringmaster t1 where t1.state=5 and t1.projectid is not null and t1.ifprojectmoney = 0 " +
                                  " group by t1.projectid,t1.createdate ";
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GatheringMaster master = new GatheringMaster();
                    master.ProjectId = TransUtil.ToString(dataRow["projectid"]);
                    master.CreateDate = TransUtil.ToDateTime(dataRow["createdate"]);
                    master.Temp1 = TransUtil.ToString(dataRow["money"]);//金额
                    gatherList.Add(master);
                }
            }
            gMasterEnumList = gatherList.OfType<GatheringMaster>();


            foreach (FundManagementByProject funByProject in addProjectNoGathWarnList)
            {
                FundManagementByProject fund = new FundManagementByProject();
                fund.OperationOrg = funByProject.OperationOrg;
                fund.OperOrgName = funByProject.OperOrgName;
                fund.OpgSysCode = funByProject.OpgSysCode;
                fund.ProjectName = funByProject.ProjectName;
                fund.NumericalValue = funByProject.NumericalValue;
                fund.ProjectId = funByProject.ProjectId;
                fund.OrganizationLevel = "项目";
                fund.IndexName = "应收未收款";
                fund.MeasurementUnitName = "元";
                fund.TimeName = "累计";
                fund.CreateDate = currDate;

                decimal lastNoGathMoney = 0;
                DateTime date500 = TransUtil.ToDateTime("2000-01-01");
                DateTime date1000 = TransUtil.ToDateTime("2000-01-01");
                DateTime date2000 = TransUtil.ToDateTime("2000-01-01");
                decimal date500Value = 0;
                decimal date1000Value = 0;
                decimal date2000Value = 0;
                Hashtable warnHt = new Hashtable();
                for (DateTime calDate = currDate.AddDays(-1); calDate >= minDate.AddDays(-1); calDate = calDate.AddDays(-1))
                {
                    var queryOwnerQDetail = from c in ownerDtlEnumList
                                            where c.ConfirmDate <= calDate
                                                && c.TempData1 == funByProject.ProjectId
                                            select c;
                    var queryGatherMaster = from c in gMasterEnumList
                                            where c.CreateDate <= calDate
                                                && c.ProjectId == funByProject.ProjectId
                                            select c;
                    //decimal addOwnerQMoney = 0;
                    //decimal gRate = 0;
                    decimal addAcctGatheringMoney = 0;
                    decimal addGatherMoney = 0;
                    
                    if (queryOwnerQDetail.Count() > 0)
                    {
                        //addOwnerQMoney = queryOwnerQDetail.Sum(c => c.ConfirmMoney);//当前累计审量金额
                        //gRate = queryOwnerQDetail.Max(c => c.GatheringRate);//最大合同比例
                        addAcctGatheringMoney = queryOwnerQDetail.Sum(c => c.AcctGatheringMoney);//当前累计应收金额
                    }
                    if (queryGatherMaster.Count() > 0)
                    {
                        addGatherMoney = queryGatherMaster.Sum(c => TransUtil.ToDecimal(c.Temp1));//当前累计收款金额
                    }

                    //decimal currNoGatherMoney = addOwnerQMoney * gRate - addGatherMoney;//当前拖欠金额
                    decimal currNoGatherMoney = addAcctGatheringMoney - addGatherMoney;//当前拖欠金额
                    //找出三个临界点的日期
                    if (lastNoGathMoney >= 5000000 && currNoGatherMoney < 5000000 && date500 == TransUtil.ToDateTime("2000-01-01"))
                    {
                        date500 = calDate;
                        date500Value = lastNoGathMoney;
                    }
                    if (lastNoGathMoney >= 10000000 && currNoGatherMoney < 10000000 && date1000 == TransUtil.ToDateTime("2000-01-01"))
                    {
                        date1000 = calDate;
                        date1000Value = lastNoGathMoney;
                    }
                    if (lastNoGathMoney >= 20000000 && currNoGatherMoney < 20000000 && date2000 == TransUtil.ToDateTime("2000-01-01"))
                    {
                        date2000 = calDate;
                        date2000Value = lastNoGathMoney;
                    }
                    lastNoGathMoney = currNoGatherMoney;
                }

                if (funByProject.NumericalValue >= 20000000)//累计拖欠款大于2000万
                {
                    if (date500 > TransUtil.ToDateTime("2000-01-01"))
                    {
                        int days = (currDate - date500).Days;
                        if (days >= 90)
                        {
                            fund.WarningLevelName = "红色预警";
                            fund.Descript = "预警原因:[500万元及以上,三个月及以上],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date500.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date500Value + "]";
                            if (!warnHt.ContainsKey("红"))
                            {
                                warnHt.Add("红", fund);
                            }
                        }
                        else
                        {
                            if (!warnHt.ContainsKey("正常"))
                            {
                                warnHt.Add("正常", fund);
                            }
                        }
                    }
                    if (date1000 > TransUtil.ToDateTime("2000-01-01"))
                    {
                        int days = (currDate - date1000).Days;
                        if (days >= 90)
                        {
                            fund.WarningLevelName = "红色预警";
                            fund.Descript = "预警原因:[1000万元-2000万元,三个月以上],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date1000.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date1000Value + "]";
                            if (!warnHt.ContainsKey("红"))
                            {
                                warnHt.Add("红", fund);
                            }
                        }
                        if (days >= 60 && days < 90)
                        {
                            fund.WarningLevelName = "橙色预警";
                            fund.Descript = "预警原因:[1000万元-2000万元,两个月],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date1000.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date1000Value + "]";
                            if (!warnHt.ContainsKey("橙"))
                            {
                                warnHt.Add("橙", fund);
                            }
                        }
                        if (days >= 30 && days < 60)
                        {
                            fund.WarningLevelName = "黄色预警";
                            fund.Descript = "预警原因:[1000万元-2000万元,一个月],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date1000.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date1000Value + "]";
                            warnHt.Add("黄", fund);
                        }
                        if (days < 30)
                        {
                            if (!warnHt.ContainsKey("正常"))
                            {
                                warnHt.Add("正常", fund);
                            }
                        }
                    }
                    if (date2000 > TransUtil.ToDateTime("2000-01-01"))
                    {
                        int days = (currDate - date1000).Days;
                        if (days >= 60)
                        {
                            fund.WarningLevelName = "红色预警";
                            fund.Descript = "预警原因:[2000万元及以上,两个月及以上],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date2000.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date2000Value + "]";
                            if (!warnHt.ContainsKey("红"))
                            {
                                warnHt.Add("红", fund);
                            }
                        }
                        if (days >= 30 && days < 60)
                        {
                            fund.WarningLevelName = "橙色预警";
                            fund.Descript = "预警原因:[2000万元及以上,一个月],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date2000.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date2000Value + "]";
                            if (!warnHt.ContainsKey("橙"))
                            {
                                warnHt.Add("橙", fund);
                            }
                        }
                        if (days < 30)
                        {
                            if (!warnHt.ContainsKey("正常"))
                            {
                                warnHt.Add("正常", fund);
                            }
                        }
                    }
                }
                if (funByProject.NumericalValue >= 10000000 && funByProject.NumericalValue < 20000000)//累计拖欠款在1000万到2000万之间
                {
                    if (date500 > TransUtil.ToDateTime("2000-01-01"))
                    {
                        int days = (currDate - date500).Days;
                        if (days >= 90)
                        {
                            fund.WarningLevelName = "红色预警";
                            fund.Descript = "预警原因:[500万元及以上,三个月及以上],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date500.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date500Value + "]";
                            if (!warnHt.ContainsKey("红"))
                            {
                                warnHt.Add("红", fund);
                            }
                        }
                        else
                        {
                            if (!warnHt.ContainsKey("正常"))
                            {
                                warnHt.Add("正常", fund);
                            }
                        }
                    }
                    if (date1000 > TransUtil.ToDateTime("2000-01-01"))
                    {
                        int days = (currDate - date1000).Days;
                        if (days >= 90)
                        {
                            fund.WarningLevelName = "红色预警";
                            fund.Descript = "预警原因:[1000万元-2000万元,三个月以上],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date1000.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date1000Value + "]";
                            if (!warnHt.ContainsKey("红"))
                            {
                                warnHt.Add("红", fund);
                            }
                        }
                        if (days >= 60 && days < 90)
                        {
                            fund.WarningLevelName = "橙色预警";
                            fund.Descript = "预警原因:[1000万元-2000万元,两个月],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date1000.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date1000Value + "]";
                            if (!warnHt.ContainsKey("橙"))
                            {
                                warnHt.Add("橙", fund);
                            }
                        }
                        if (days >= 30 && days < 60)
                        {
                            fund.WarningLevelName = "黄色预警";
                            fund.Descript = "预警原因:[1000万元-2000万元,一个月],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date1000.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date1000Value + "]";
                            warnHt.Add("黄", fund);
                        }
                        if (days < 30)
                        {
                            if (!warnHt.ContainsKey("正常"))
                            {
                                warnHt.Add("正常", fund);
                            }
                        }
                    }

                }
                if (funByProject.NumericalValue < 10000000)//累计拖欠款在1000万到2000万之间
                {
                    if (date500 > TransUtil.ToDateTime("2000-01-01"))
                    {
                        int days = (currDate - date500).Days;
                        if (days >= 90)
                        {
                            fund.WarningLevelName = "红色预警";
                            fund.Descript = "预警原因:[500万元及以上,三个月及以上],应收欠款时间[" + (days - 1) + "天]," +
                                            "临界欠款日期[" + date500.AddDays(1).ToShortDateString() + "],应收欠款金额[" + date500Value + "]";
                            if (!warnHt.ContainsKey("红"))
                            {
                                warnHt.Add("红", fund);
                            }
                        }
                        else
                        {
                            if (!warnHt.ContainsKey("正常"))
                            {
                                warnHt.Add("正常", fund);
                            }
                        }
                    }

                }
                //取最大的预警级别
                if (warnHt.ContainsKey("红"))
                {
                    FundManagementByProject calFund = (FundManagementByProject)warnHt["红"];
                    list.Add(calFund);
                }
                else if (warnHt.ContainsKey("橙"))
                {
                    FundManagementByProject calFund = (FundManagementByProject)warnHt["橙"];
                    list.Add(calFund);
                }
                else if (warnHt.ContainsKey("黄"))
                {
                    FundManagementByProject calFund = (FundManagementByProject)warnHt["黄"];
                    list.Add(calFund);
                }
                else if (warnHt.ContainsKey("正常"))
                {

                }
                else
                {
                    fund.WarningLevelName = "红色预警";
                    fund.Descript = "预警原因:[应收欠款金额超过500万,时间超过90天],应收欠款时间[100天内未找到临界日期],应收欠款金额[" + funByProject.NumericalValue + "]";
                    list.Add(fund);
                }
            }
            return list;
        }
        //批量插入指标数据(分公司/公司)
        private void InsertFundManagementByBatch(IList list)
        {
            try
            {
                IFCGuidGenerator gen = new IFCGuidGenerator();
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                foreach (FundManagement model in list)
                {
                    string guid = gen.GeneratorIFCGuid();
                    string sql = " insert into thd_fundmanagement " +
                                    " (Id,CreateDate,OperationOrg,OperOrgName,OpgSysCode,OrganizationLevel,IndexName,IndexID,TimeID," +
                                    " TimeName,MeasurementUnitID,MeasurementUnitName,WarningLevelID,WarningLevelName,NumericalValue,Descript,businesstype) values ( '" +
                        guid + "',to_date('" + model.CreateDate.ToShortDateString() + "','yyyy-mm-dd'),'" + model.OperationOrg + "','" + model.OperOrgName + "','"
                        + model.OpgSysCode + "','" + model.OrganizationLevel + "','" + model.IndexName + "','" + model.IndexID + "','"
                        + model.TimeID + "','" + model.TimeName + "','" + model.MeasurementUnitID + "','" + model.MeasurementUnitName + "','"
                        + model.WarningLevelID + "','" + model.WarningLevelName + "'," + model.NumericalValue + ",'" + model.Descript + "'," + (int)model.Type
                        + ")";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //批量插入指标数据(项目)
        private void InsertFundManageByProjectByBatch(IList list)
        {
            try
            {
                IFCGuidGenerator gen = new IFCGuidGenerator();
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                foreach (FundManagementByProject model in list)
                {
                    string guid = gen.GeneratorIFCGuid();
                    string sql = " insert into thd_fundmanagebyproject " +
                                    " (Id,ProjectId,ProjectName,CreateDate,OperationOrg,OperOrgName,OpgSysCode,OrganizationLevel,IndexName,IndexID,TimeID," +
                                    " TimeName,MeasurementUnitID,MeasurementUnitName,WarningLevelID,WarningLevelName,NumericalValue,Descript,businesstype) values ( '" +
                        guid + "','" + model.ProjectId + "','" + model.ProjectName + "',to_date('" + model.CreateDate.ToShortDateString() + "','yyyy-mm-dd'),'" + model.OperationOrg + "','"
                        + model.OperOrgName + "','" + model.OpgSysCode + "','" + model.OrganizationLevel + "','" + model.IndexName + "','" + model.IndexID + "','"
                        + model.TimeID + "','" + model.TimeName + "','" + model.MeasurementUnitID + "','" + model.MeasurementUnitName + "','"
                        + model.WarningLevelID + "','" + model.WarningLevelName + "'," + model.NumericalValue + ",'" + model.Descript + "'," + (int)model.Type
                        + ")";
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //通过日期删除指标数据(项目和分公司)
        private void DeleteFundManageAndProject(DateTime currDate)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                string sql = " delete from thd_fundmanagebyproject t1 where t1.createdate=to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                sql = " delete from thd_fundmanagement t1 where t1.createdate=to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region 日现金流
        /// <summary>
        /// 日现金流量表
        /// </summary>
        /// <param name="currDate">当前日期</param>
        /// <param name="projectID">项目ID</param>
        /// <param name="syscode">分公司/公司的话，组织层次码</param>
        /// 格式{流入,流出} 
        /// {货币上交:,4104}{借款:2001,}{保理金额:4105,}{工程款:112201,220203}{以房(物)抵款:112206,220208}{应交税费:,2221}{投标保证金:12210201,22410101}
        /// {履约保证金:12210202,22410102}{农民工工资保证金:12210204,12210204}
        /// {其他保证金:[安全保证金:12210205,22410106][质量保证金:12210206,22410104][诚信保证金:12210207,22410107][其他保证金:12210299,22410199][预付款保证金:12210203,12210203]}
        /// {押金:[散装水泥押金:12210305,12210305][风险抵押金:22410203,22410203][房租押金:12210303,22410204][水电押金:12210304,22410205][处理废材押金:22410206,22410206]
        /// [食堂押金:22410208,22410208][其他押金:12210310,22410220]}
        /// {备用金:[122101,122101}{政府规费:,224197}{保险费:,224196}{间接费用:,54010106}{管理费用:,6602}{财务费用:,6603}{调入调出租赁费:122194,224194}
        /// {调入调出材料费:122195,224195}{其他:[代业主垫款:12210401,12210401][代分包垫款:12210402,12210402][代职工垫款:12210403,12210403][配合费:122196,]
        /// [罚款:122197,224198][其他应收付款其他:224199,122199]} 
        public IList QueryDayCashFlowReport(DateTime currDate, string projectID, string syscode, bool ifSelf)
        {
            string[] acctTitles = new string[] { "货币上交", "借款", "保理金额", "工程款", "以房(物)抵款", "应交税费", "投标保证金", "履约保证金", "农民工工资保证金", 
                "其他保证金", "押金", "备用金", "政府规费", "保险费", "间接费用", "管理费用", "财务费用", "调入调出租赁费", "调入调出材料费", "其他" };
            string[,] acctTables = new string[20, 8];
            IList list = new ArrayList();
            for (int i = 0; i < acctTitles.Length; i++)
            {
                string titleName = acctTitles[i];
                DataDomain domain = new DataDomain();
                domain.Name1 = titleName;
                list.Add(domain);
            }
            IList subList = new ArrayList();//分公司集合
            #region 条件
            //获取时间信息
            if (currDate == null)
            {
                currDate = TransUtil.ToDateTime(DateTime.Now.ToShortDateString());//当日
            }
            int currYear = currDate.Year;
            int currMonth = currDate.Month;
            DateTime monthFirstDate = new DateTime(currDate.Year, currDate.Month, 1);//当月第一天
            DateTime monthLastDate = monthFirstDate.AddMonths(1).AddDays(-1);//当月最后一天
            DateTime yearFirstDate = new DateTime(currDate.Year, 1, 1);//当年第一天
            //DateTime yearLastDate = yearFirstDate.AddYears(1).AddDays(-1);//当年最后一天

            string dayCondition = " ";//本日条件
            string monthCondition = " ";//本月条件
            string yearCondition = " ";//本年条件
            string addCondition = " ";//累计条件
            string yearInitCondition = "";//本年期初条件
            if (TransUtil.ToString(projectID) != "")
            {
                dayCondition = " and t1.projectid = '" + projectID + "' and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                monthCondition = " and t1.projectid = '" + projectID + "' and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                yearCondition = " and t1.projectid = '" + projectID + "' and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                addCondition = " and t1.projectid = '" + projectID + "' and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                yearInitCondition = " and t1.projectid = '" + projectID + "' and t1.createDate < to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd')  ";
            }
            else
            {
                if (ifSelf == false)
                {
                    dayCondition = " and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    monthCondition = " and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    yearCondition = " and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    addCondition = "  and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    yearInitCondition = " and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate < to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') ";
                }
                else
                {
                    if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true && syscode != "")//公司
                    {
                        subList = this.GetSubCompanySyscodeList();
                        string notInSubCondition = "";//剔除分公司以下层次的单据
                        foreach (string subSyscode in subList)
                        {
                            notInSubCondition += " and instr(t1.opgsyscode,'" + subSyscode + "') = 0 ";
                        }
                        dayCondition = notInSubCondition + " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                        monthCondition = notInSubCondition + " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                        yearCondition = notInSubCondition + " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                        addCondition = notInSubCondition + " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0  and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                        yearInitCondition = notInSubCondition + " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate < to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    }
                    else
                    {//分公司
                        dayCondition = " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                        monthCondition = " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                        yearCondition = " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                        addCondition = " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                        yearInitCondition = " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate < to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    }
                }
            }
            #endregion

            #region 收/付款
            //本日收款
            IList dayGatheringList = this.GetGatheringInfoByCondition(dayCondition);
            acctTables = CreateCashDataDomain(acctTables, dayGatheringList, 0);
            //本月收款
            IList monthGatheringList = this.GetGatheringInfoByCondition(monthCondition);
            acctTables = CreateCashDataDomain(acctTables, monthGatheringList, 1);
            //本年收款
            IList yearGatheringList = this.GetGatheringInfoByCondition(yearCondition);
            acctTables = CreateCashDataDomain(acctTables, yearGatheringList, 2);
            //累计收款
            IList addGatheringList = this.GetGatheringInfoByCondition(addCondition);
            acctTables = CreateCashDataDomain(acctTables, addGatheringList, 3);

            //付款
            //本日付款
            IList dayPaymentList = this.GetPaymentInfoByCondition(dayCondition);
            acctTables = CreateCashDataDomainByPayment(acctTables, dayPaymentList, 4);
            //本月付款
            IList monthPaymentList = this.GetPaymentInfoByCondition(monthCondition);
            acctTables = CreateCashDataDomainByPayment(acctTables, monthPaymentList, 5);
            //本年付款
            IList yearPaymentList = this.GetPaymentInfoByCondition(yearCondition);
            acctTables = CreateCashDataDomainByPayment(acctTables, yearPaymentList, 6);
            //累计付款
            IList addPaymentList = this.GetPaymentInfoByCondition(addCondition);
            acctTables = CreateCashDataDomainByPayment(acctTables, addPaymentList, 7);
            #endregion

            #region 费用信息
            //本日费用信息
            Hashtable dayExpenseHt = this.GetIndirectCostByCondition(dayCondition);
            decimal dayCurrencyMoney = (decimal)dayExpenseHt["4104"];//分公司货币上交
            acctTables[0, 4] = dayCurrencyMoney + "";
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true)//公司
            {
                decimal dayBorrowMoney = (decimal)dayExpenseHt["2001"];//借款
                acctTables[1, 0] = dayBorrowMoney + "";
            }
            decimal dayIndirectMoney = (decimal)dayExpenseHt["54010106"];//间接费用
            acctTables[14, 4] = dayIndirectMoney + "";
            decimal dayManageMoney = (decimal)dayExpenseHt["6602"];//管理费用
            acctTables[15, 4] = dayManageMoney + "";
            decimal dayFinanceMoney = (decimal)dayExpenseHt["6603"];//财务费用
            acctTables[16, 4] = dayFinanceMoney + "";
            //本月费用信息
            Hashtable monthExpenseHt = this.GetIndirectCostByCondition(monthCondition);
            decimal monthCurrencyMoney = (decimal)monthExpenseHt["4104"];//分公司货币上交
            acctTables[0, 5] = monthCurrencyMoney + "";

            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true)//公司
            {
                decimal monthBorrowMoney = (decimal)monthExpenseHt["2001"];//借款
                acctTables[1, 1] = monthBorrowMoney + "";
            }
            decimal monthIndirectMoney = (decimal)monthExpenseHt["54010106"];//间接费用
            acctTables[14, 5] = monthIndirectMoney + "";
            decimal monthManageMoney = (decimal)monthExpenseHt["6602"];//管理费用
            acctTables[15, 5] = monthManageMoney + "";
            decimal monthFinanceMoney = (decimal)monthExpenseHt["6603"];//财务费用
            acctTables[16, 5] = monthFinanceMoney + "";
            //本年费用信息
            Hashtable yearExpenseHt = this.GetIndirectCostByCondition(yearCondition);
            decimal yearCurrencyMoney = (decimal)yearExpenseHt["4104"];//分公司货币上交
            acctTables[0, 6] = yearCurrencyMoney + "";
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true)//公司
            {
                decimal yearBorrowMoney = (decimal)yearExpenseHt["2001"];//借款
                acctTables[1, 2] = yearBorrowMoney + "";
            }
            decimal yearIndirectMoney = (decimal)yearExpenseHt["54010106"];//间接费用
            acctTables[14, 6] = yearIndirectMoney + "";
            decimal yearManageMoney = (decimal)yearExpenseHt["6602"];//管理费用
            acctTables[15, 6] = yearManageMoney + "";
            decimal yearFinanceMoney = (decimal)yearExpenseHt["6603"];//财务费用
            acctTables[16, 6] = yearFinanceMoney + "";
            //累计费用信息
            Hashtable addExpenseHt = this.GetIndirectCostByCondition(addCondition);
            decimal addCurrencyMoney = (decimal)addExpenseHt["4104"];//分公司货币上交
            acctTables[0, 7] = addCurrencyMoney + "";
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true)//公司
            {
                decimal addBorrowMoney = (decimal)addExpenseHt["2001"];//借款
                acctTables[1, 3] = addBorrowMoney + "";
            }
            decimal addIndirectMoney = (decimal)addExpenseHt["54010106"];//间接费用
            acctTables[14, 7] = addIndirectMoney + "";
            decimal addManageMoney = (decimal)addExpenseHt["6602"];//管理费用
            acctTables[15, 7] = addManageMoney + "";
            decimal addFinanceMoney = (decimal)addExpenseHt["6603"];//财务费用
            acctTables[16, 7] = addFinanceMoney + "";

            #endregion

            #region 货币上交
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true && syscode != "")//公司
            {
                if (subList.Count == 0)
                {
                    subList = this.GetSubCompanySyscodeList();
                }
                string inSubCondition = "";//分公司的单据
                foreach (string subSyscode in subList)
                {
                    if (inSubCondition == "")
                    {
                        inSubCondition += " and ( instr(t1.opgsyscode,'" + subSyscode + "') > 0 ";
                    }
                    else
                    {
                        inSubCondition += " or instr(t1.opgsyscode,'" + subSyscode + "') > 0 ";
                    }
                }
                if (subList != null && subList.Count > 0)
                {
                    inSubCondition += " ) ";
                }
                string currencyDayCondition = " and t1.projectid is null and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " + inSubCondition;
                string currencyMonthCondition = " and t1.projectid is null and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " + inSubCondition;
                string currencyYearCondition = " and t1.projectid is null and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " + inSubCondition;
                string currencyAddCondition = " and t1.projectid is null and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " + inSubCondition;

                decimal daySubCurrencyMoney = this.GetCurrencyMoneyByCondition(currencyDayCondition);
                decimal monthSubCurrencyMoney = this.GetCurrencyMoneyByCondition(currencyMonthCondition);
                decimal yearSubCurrencyMoney = this.GetCurrencyMoneyByCondition(currencyYearCondition);
                decimal addSubCurrencyMoney = this.GetCurrencyMoneyByCondition(currencyAddCondition);
                if (ifSelf == true)
                {
                    acctTables[0, 0] = daySubCurrencyMoney + "";
                    acctTables[0, 1] = monthSubCurrencyMoney + "";
                    acctTables[0, 2] = yearSubCurrencyMoney + "";
                    acctTables[0, 3] = addSubCurrencyMoney + "";
                }
                else
                {
                    acctTables[0, 4] = (dayCurrencyMoney - daySubCurrencyMoney) + "";
                    acctTables[0, 5] = (monthCurrencyMoney - monthSubCurrencyMoney) + "";
                    acctTables[0, 6] = (yearCurrencyMoney - yearSubCurrencyMoney) + "";
                    acctTables[0, 7] = (addCurrencyMoney - addSubCurrencyMoney) + "";
                }

            }
            #endregion

            #region 保理
            if (TransUtil.ToString(syscode) != "")
            {
                //本日保理
                decimal dayFactoringMoney = this.GetFactoringDataByCondition(dayCondition);
                acctTables[2, 0] = dayFactoringMoney + "";
                //本月保理
                decimal monthFactoringMoney = this.GetFactoringDataByCondition(monthCondition);
                acctTables[2, 1] = monthFactoringMoney + "";
                //本年保理
                decimal yearFactoringMoney = this.GetFactoringDataByCondition(yearCondition);
                acctTables[2, 2] = yearFactoringMoney + "";
                //累计保理
                decimal addFactoringMoney = this.GetFactoringDataByCondition(addCondition);
                acctTables[2, 3] = addFactoringMoney + "";
            }
            #endregion

            #region 借款
            //借款(项目和分公司流入)
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == false || projectID != "")//非公司
            {
                if (projectID != "")
                {
                    //本日借款
                    decimal dayBorrowMoney = this.GetBorrowOrderByCondition(dayCondition);
                    acctTables[1, 0] = dayBorrowMoney + "";
                    //本月借款
                    decimal monthBorrowMoney = this.GetBorrowOrderByCondition(monthCondition);
                    acctTables[1, 1] = monthBorrowMoney + "";
                    //本年借款
                    decimal yearBorrowMoney = this.GetBorrowOrderByCondition(yearCondition);
                    acctTables[1, 2] = yearBorrowMoney + "";
                    //累计借款
                    decimal addBorrowMoney = this.GetBorrowOrderByCondition(addCondition);
                    acctTables[1, 3] = addBorrowMoney + "";
                }
                else
                {
                    string daySubCondition = " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    string monthSubCondition = " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    string yearSubCondition = " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    string addSubCondition = "  and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    //本日借款
                    decimal dayBorrowMoney = this.GetBorrowOrderByCondition(daySubCondition);
                    acctTables[1, 0] = dayBorrowMoney + "";
                    //本月借款
                    decimal monthBorrowMoney = this.GetBorrowOrderByCondition(monthSubCondition);
                    acctTables[1, 1] = monthBorrowMoney + "";
                    //本年借款
                    decimal yearBorrowMoney = this.GetBorrowOrderByCondition(yearSubCondition);
                    acctTables[1, 2] = yearBorrowMoney + "";
                    //累计借款
                    decimal addBorrowMoney = this.GetBorrowOrderByCondition(addSubCondition);
                    acctTables[1, 3] = addBorrowMoney + "";
                }
            }
            //借款(公司流出: 各分公司借款)
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) && ifSelf == true && syscode != "")//公司
            {
                if (subList.Count == 0)
                {
                    subList = this.GetSubCompanySyscodeList();
                }
                string inSubCondition = "";//分公司的单据
                foreach (string subSyscode in subList)
                {
                    if (inSubCondition == "")
                    {
                        inSubCondition += " and ( instr(t1.opgsyscode,'" + subSyscode + "') > 0 ";
                    }
                    else
                    {
                        inSubCondition += " or instr(t1.opgsyscode,'" + subSyscode + "') > 0 ";
                    }
                }
                if (subList != null && subList.Count > 0)
                {
                    inSubCondition += " ) ";
                }
                string borrowDayCondition = " and t1.projectid is null and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " + inSubCondition;
                string borrowMonthCondition = " and t1.projectid is null and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " + inSubCondition;
                string borrowYearCondition = " and t1.projectid is null and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " + inSubCondition;
                string borrowAddCondition = " and t1.projectid is null and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') " + inSubCondition;

                //本日借款
                decimal dayBorrowMoney = this.GetBorrowOrderByCondition(borrowDayCondition);
                acctTables[1, 4] = dayBorrowMoney + "";
                //本月借款
                decimal monthBorrowMoney = this.GetBorrowOrderByCondition(borrowMonthCondition);
                acctTables[1, 5] = monthBorrowMoney + "";
                //本年借款
                decimal yearBorrowMoney = this.GetBorrowOrderByCondition(borrowYearCondition);
                acctTables[1, 6] = yearBorrowMoney + "";
                //累计借款
                decimal addBorrowMoney = this.GetBorrowOrderByCondition(borrowAddCondition);
                acctTables[1, 7] = addBorrowMoney + "";
            }
            //借款(分公司流出: 各项目借款)
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == false && TransUtil.ToString(projectID) == "" && ifSelf == true)//公司
            {
                string borrowDayCondition = " and t1.projectid is not null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                string borrowMonthCondition = " and t1.projectid is not null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                string borrowYearCondition = " and t1.projectid is not null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                string borrowAddCondition = "  and t1.projectid is not null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                //本日借款
                decimal dayBorrowMoney = this.GetBorrowOrderByCondition(borrowDayCondition);
                acctTables[1, 4] = dayBorrowMoney + "";
                //本月借款
                decimal monthBorrowMoney = this.GetBorrowOrderByCondition(borrowMonthCondition);
                acctTables[1, 5] = monthBorrowMoney + "";
                //本年借款
                decimal yearBorrowMoney = this.GetBorrowOrderByCondition(borrowYearCondition);
                acctTables[1, 6] = yearBorrowMoney + "";
                //累计借款
                decimal addBorrowMoney = this.GetBorrowOrderByCondition(borrowAddCondition);
                acctTables[1, 7] = addBorrowMoney + "";
            }
            #endregion

            #region 期初
            decimal initCalRelMoney = 0;//初始化算上不算下的类型
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == false || projectID != "")//非公司才会有借款
            {
                if (projectID != "")
                {
                    initCalRelMoney = this.GetBorrowOrderByCondition(yearInitCondition);//项目
                }
                else
                {
                    string initYearInitCondition = " and t1.projectid is null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate < to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    initCalRelMoney = this.GetBorrowOrderByCondition(initYearInitCondition);
                }

            }
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true && syscode != "")//公司
            {
                string inSubCondition = "";//分公司的单据
                foreach (string subSyscode in subList)
                {
                    if (inSubCondition == "")
                    {
                        inSubCondition += " and ( instr(t1.opgsyscode,'" + subSyscode + "') > 0 ";
                    }
                    else
                    {
                        inSubCondition += " or instr(t1.opgsyscode,'" + subSyscode + "') > 0 ";
                    }
                }
                if (subList != null && subList.Count > 0)
                {
                    inSubCondition += " ) ";
                }
                string calRelYearInitCondition = " and t1.projectid is null and t1.createDate <= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') " + inSubCondition;
                if (ifSelf == true)
                {
                    initCalRelMoney = -this.GetBorrowOrderByCondition(calRelYearInitCondition);
                }
                initCalRelMoney += this.GetCurrencyMoneyByCondition(calRelYearInitCondition);//公司的货币上交
            }
            //借款(分公司流出: 各项目借款)
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == false && TransUtil.ToString(projectID) == "" && ifSelf == true)//分公司
            {
                string borrowAddCondition = "  and t1.projectid is not null and instr(t1.opgsyscode,'" + syscode + "') > 0 and t1.createDate <= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') ";
                initCalRelMoney -= this.GetBorrowOrderByCondition(borrowAddCondition);
            }

            decimal initMoney = this.GetCurrInitMoneyByCondition(yearInitCondition);//期初

            #endregion

            #region 日均/月均
            list = TransListFromArrays(list, acctTables);
            //计算月均
            decimal currLeftValue = 0;
            foreach (DataDomain domain in list)
            {
                currLeftValue += TransUtil.ToDecimal(domain.Name5) - TransUtil.ToDecimal(domain.Name9);
            }
            currLeftValue += initMoney + initCalRelMoney;

            decimal avgDayMoney = 0;
            decimal avgMonthMoney = 0;
            if (projectID != "")
            {
                string dayAvgCondition = " and t1.timename='日均' and t1.indexname='资金存量' and t1.projectid='" + projectID + "'" +
                                        " and t1.createdate=to_date('" + currDate.AddDays(-1).ToShortDateString() + "','yyyy-mm-dd') ";
                avgDayMoney = this.GetCurrDayAvgMoneyByCondition(dayAvgCondition, 0);

                string monthAvgCondition = " and t1.timename='月末' and t1.indexname='资金存量' and t1.projectid='" + projectID + "'" +
                                        " and t1.createdate>=to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                        " and t1.createdate<to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                avgMonthMoney = this.GetCurrMonthAvgMoneyByCondition(monthAvgCondition, 0, currLeftValue);
            }
            else
            {
                if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true && syscode != "")//公司
                {
                    if (ifSelf == true)
                    {
                        string dayAvgCondition = " and t1.timename='日均' and t1.indexname='资金存量' and t1.operorgname='公司总部' and t1.organizationlevel='分公司' " +
                                            " and t1.createdate=to_date('" + currDate.AddDays(-1).ToShortDateString() + "','yyyy-mm-dd') ";
                        avgDayMoney = this.GetCurrDayAvgMoneyByCondition(dayAvgCondition, 1);
                    }
                    else
                    {
                        string dayAvgCondition = " and t1.timename='日均' and t1.indexname='资金存量' and t1.organizationlevel='公司' " +
                                            " and t1.createdate=to_date('" + currDate.AddDays(-1).ToShortDateString() + "','yyyy-mm-dd') ";
                        avgDayMoney = this.GetCurrDayAvgMoneyByCondition(dayAvgCondition, 1);


                    }
                    string monthAvgCondition = " and t1.timename='月末' and t1.indexname='资金存量' and t1.organizationlevel='公司' " +
                                            " and t1.createdate>=to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                            " and t1.createdate<to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    avgMonthMoney = this.GetCurrMonthAvgMoneyByCondition(monthAvgCondition, 1, currLeftValue);
                }
                else
                {
                    string dayAvgCondition = " and t1.timename='日均' and t1.indexname='资金存量' and t1.organizationlevel='分公司' and instr(t1.opgsyscode,'" + syscode + "') > 0" +
                                        " and t1.createdate=to_date('" + currDate.AddDays(-1).ToShortDateString() + "','yyyy-mm-dd') ";
                    avgDayMoney = this.GetCurrDayAvgMoneyByCondition(dayAvgCondition, 1);

                    string monthAvgCondition = " and t1.timename='月末' and t1.indexname='资金存量' and t1.organizationlevel='分公司' and instr(t1.opgsyscode,'" + syscode + "') > 0" +
                                        " and t1.createdate>=to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                        " and t1.createdate<to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
                    avgMonthMoney = this.GetCurrMonthAvgMoneyByCondition(monthAvgCondition, 1, currLeftValue);
                }
            }
            #endregion

            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true && syscode != "")//公司(剔除内部项目的工程款)
            {
                DataDomain gDomain = (DataDomain)list[3];
                decimal dayInProjectGMoney = this.GetCompanyInProjectGatheringMainMoney(dayCondition);
                decimal monthInProjectGMoney = this.GetCompanyInProjectGatheringMainMoney(monthCondition);
                decimal yearInProjectGMoney = this.GetCompanyInProjectGatheringMainMoney(yearCondition);
                decimal addInProjectGMoney = this.GetCompanyInProjectGatheringMainMoney(addCondition);
                gDomain.Name3 = TransUtil.ToDecimal(gDomain.Name3) - dayInProjectGMoney;
                gDomain.Name4 = TransUtil.ToDecimal(gDomain.Name4) - monthInProjectGMoney;
                gDomain.Name5 = TransUtil.ToDecimal(gDomain.Name5) - yearInProjectGMoney;
                gDomain.Name6 = TransUtil.ToDecimal(gDomain.Name6) - addInProjectGMoney;
            }
            DataDomain initDomain = (DataDomain)list[0];
            initDomain.Name2 = (initMoney + initCalRelMoney) + "";
            initDomain.Name12 = avgDayMoney + "";
            initDomain.Name14 = avgMonthMoney + "";
            return list;
        }
        /// <summary>
        /// 日现金流量详细统计表
        /// </summary>
        /// <param name="currDate">当前日期</param>
        /// <param name="syscode">分公司/公司的话，组织层次码</param>
        public IList QueryDayCashDetailReport(DateTime currDate, string syscode)
        {
            IList list = new ArrayList();
            #region 算法初始化
            int queryType = 0;//查询信息为项目
            if (TransUtil.CompanyOpgSyscode.Contains(syscode) == true && syscode != "")//公司
            {
                queryType = 1;//分公司
            }
            //查询分公司对应的组织信息
            string projectCondition = " and t1.ownerorgsyscode like '%" + syscode + "%' ";
            Hashtable basic_ht = this.GetCurrOrgAndProjectInfo(projectCondition);
            if (queryType == 0)
            {
                Hashtable project_ht = basic_ht["projectinfo"] as Hashtable;
                foreach (CurrentProjectInfo projectInfo in project_ht.Values)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name30 = projectInfo.Id;
                    domain.Name1 = projectInfo.Name;
                    list.Add(domain);
                }
            }
            else
            {
                IList opgList = basic_ht["subcompanyinfo"] as ArrayList;
                foreach (OperationOrgInfo orgInfo in opgList)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name30 = orgInfo.SysCode;
                    domain.Name1 = orgInfo.Name;
                    list.Add(domain);
                }
                //加上公司总部
                //DataDomain compDomain = new DataDomain();
                //compDomain.Name1 = "公司总部";
                //list.Insert(0, compDomain);
            }

            #endregion

            #region 时间条件
            //获取时间信息
            if (currDate == null)
            {
                currDate = TransUtil.ToDateTime(DateTime.Now.ToShortDateString());//当日
            }
            int currYear = currDate.Year;
            int currMonth = currDate.Month;
            DateTime monthFirstDate = new DateTime(currDate.Year, currDate.Month, 1);//当月第一天
            DateTime monthLastDate = monthFirstDate.AddMonths(1).AddDays(-1);//当月最后一天
            DateTime yearFirstDate = new DateTime(currDate.Year, 1, 1);//当年第一天

            string dayCondition = " and t1.createDate = to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
            string monthCondition = " and t1.createDate >= to_date('" + monthFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
            string yearCondition = " and t1.createDate >= to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') and t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
            string addCondition = " and  t1.createDate <= to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') ";
            string yearInitCondition = " and t1.createDate < to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd')  ";
            #endregion

            //期初
            list = this.GetProjectLeftCashInfoByCondition(list, yearInitCondition, syscode, queryType);
            //流入
            list = this.GetCashInByCondition(list, dayCondition, syscode, queryType, 3);
            list = this.GetCashInByCondition(list, monthCondition, syscode, queryType, 4);
            list = this.GetCashInByCondition(list, yearCondition, syscode, queryType, 5);
            list = this.GetCashInByCondition(list, addCondition, syscode, queryType, 6);
            //流出
            list = this.GetCashOutByCondition(list, dayCondition, syscode, queryType, 7);
            list = this.GetCashOutByCondition(list, monthCondition, syscode, queryType, 8);
            list = this.GetCashOutByCondition(list, yearCondition, syscode, queryType, 9);
            list = this.GetCashOutByCondition(list, addCondition, syscode, queryType, 10);
            //日均/月均
            list = this.GetProjectIndexInfoByCondition(list, currDate, queryType);
            IList resultList = new ArrayList();
            foreach (DataDomain domain in list)
            {
                if (TransUtil.ToDecimal(domain.Name2) != 0 || TransUtil.ToDecimal(domain.Name6) != 0 || TransUtil.ToDecimal(domain.Name10) != 0)
                {
                    resultList.Add(domain);
                }
            }
            return resultList;
        }
        //票据台账
        public DataSet QueryAcceptanceBillReport(DateTime start, DateTime end, string projectID, string syscode)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            IList list = new ArrayList();

            string s1 = @"select (select t1.opgname  from resconfig t2
join resoperationorg t1 on t1.opgoperationtype='b'  and instr(t2.ownerorgsyscode,t1.opgid)>0 
where t2.id=t.PROJECTID) as ORGNAME,t.BILLNO,t.GATHERINGMXID,t.PAYMENTMXID,t.PROJECTNAME,t.ACCEPTPERSON,t.BILLTYPE,t.SUMMONEY,t.CREATEDATE,t.EXPIREDATE from THD_AcceptanceBill t";
            string s2 = "";
            if (TransUtil.ToString(projectID) != "")
            {
                s2 = string.Format(" where PROJECTID='{0}' and CREATEDATE >=to_date('{1}','yyyy-mm-dd') and CREATEDATE<=to_date('{2}','yyyy-mm-dd')", projectID, start.Date.ToString("yyyy-MM-dd"), end.Date.ToString("yyyy-MM-dd"));
            }
            else
            {
                s2 = string.Format(" where OPGSYSCODE like '%{0}%' and CREATEDATE >=to_date('{1}','yyyy-mm-dd') and CREATEDATE<=to_date('{2}','yyyy-mm-dd')", syscode, start.Date.ToString("yyyy-MM-dd"), end.Date.ToString("yyyy-MM-dd"));
            }
            string SQL = s1 + s2;
            command.CommandText = SQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }
        //费用信息查询
        public DataSet QueryAccountMng(string SQL)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            command.CommandText = SQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }
        //创建收款单对应的二维数组
        private string[,] CreateCashDataDomain(string[,] acctTables, IList dayGatheringList, int colIndex)
        {
            foreach (GatheringMaster master in dayGatheringList)
            {
                if (master.AccountTitleCode == "112201")//工程款
                {
                    acctTables[3, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "112206")//以房(物)抵款
                {
                    acctTables[4, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "12210201")//投标保证金
                {
                    acctTables[6, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "12210202")//履约保证金
                {
                    acctTables[7, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "12210204")//农民工工资保证金
                {
                    acctTables[8, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "12210205" || master.AccountTitleCode == "12210206" || master.AccountTitleCode == "12210207"
                    || master.AccountTitleCode == "12210299" || master.AccountTitleCode == "12210203")//其他保证金
                {
                    acctTables[9, colIndex] = TransUtil.ToDecimal(acctTables[9, colIndex]) + TransUtil.ToDecimal(master.Temp1) + "";
                }
                else if (master.AccountTitleCode == "12210305" || master.AccountTitleCode == "22410203" || master.AccountTitleCode == "12210303"
                    || master.AccountTitleCode == "12210304" || master.AccountTitleCode == "22410206" || master.AccountTitleCode == "22410208"
                    || master.AccountTitleCode == "12210310")//押金
                {
                    acctTables[10, colIndex] = TransUtil.ToDecimal(acctTables[10, colIndex]) + TransUtil.ToDecimal(master.Temp1) + "";
                }
                else if (master.AccountTitleCode == "122101")//备用金
                {
                    acctTables[11, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "122194")//调入调出租赁费
                {
                    acctTables[17, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "122195")//调入调出材料费
                {
                    acctTables[18, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "12210401" || master.AccountTitleCode == "12210402" || master.AccountTitleCode == "12210403"
                    || master.AccountTitleCode == "122196" || master.AccountTitleCode == "122197" || master.AccountTitleCode == "224199")//其他
                {
                    acctTables[19, colIndex] = TransUtil.ToDecimal(acctTables[19, colIndex]) + TransUtil.ToDecimal(master.Temp1) + "";
                }

            }
            return acctTables;
        }
        //创建付款单对应的二维数组
        private string[,] CreateCashDataDomainByPayment(string[,] acctTables, IList dayPaymentList, int colIndex)
        {
            foreach (PaymentMaster master in dayPaymentList)
            {
                if (master.AccountTitleCode == "220202" || master.AccountTitleCode == "220203"
                    || master.AccountTitleCode == "220204" || master.AccountTitleCode == "220205" || master.AccountTitleCode == "220299")//工程款
                {
                    acctTables[3, colIndex] = TransUtil.ToDecimal(acctTables[3, colIndex]) + TransUtil.ToDecimal(master.Temp1) + "";
                }
                else if (master.AccountTitleCode.Contains("220208"))//以房(物)抵款
                {
                    acctTables[4, colIndex] = TransUtil.ToDecimal(acctTables[4, colIndex]) + TransUtil.ToDecimal(master.Temp1) + "";
                }
                else if (master.AccountTitleCode == "2221")//应交税费
                {
                    acctTables[5, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "22410101")//投标保证金
                {
                    acctTables[6, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "22410102")//履约保证金
                {
                    acctTables[7, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "12210204")//农民工工资保证金
                {
                    acctTables[8, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "22410106" || master.AccountTitleCode == "22410104" || master.AccountTitleCode == "22410107"
                    || master.AccountTitleCode == "22410199" || master.AccountTitleCode == "12210203")//其他保证金
                {
                    acctTables[9, colIndex] = TransUtil.ToDecimal(acctTables[9, colIndex]) + TransUtil.ToDecimal(master.Temp1) + "";
                }
                else if (master.AccountTitleCode == "12210305" || master.AccountTitleCode == "22410203" || master.AccountTitleCode == "22410204"
                    || master.AccountTitleCode == "22410205" || master.AccountTitleCode == "22410206" || master.AccountTitleCode == "22410208"
                    || master.AccountTitleCode == "22410220")//押金
                {
                    acctTables[10, colIndex] = TransUtil.ToDecimal(acctTables[10, colIndex]) + TransUtil.ToDecimal(master.Temp1) + "";
                }
                else if (master.AccountTitleCode == "122101")//备用金
                {
                    acctTables[11, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "224197")//政府规费
                {
                    acctTables[12, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "224196")//保险费
                {
                    acctTables[13, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "224194")//调入调出租赁费
                {
                    acctTables[17, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "224195")//调入调出材料费
                {
                    acctTables[18, colIndex] = master.Temp1;
                }
                else if (master.AccountTitleCode == "12210401" || master.AccountTitleCode == "12210402" || master.AccountTitleCode == "12210403"
                    || master.AccountTitleCode == "224198" || master.AccountTitleCode == "122199")//其他
                {
                    acctTables[19, colIndex] = TransUtil.ToDecimal(acctTables[19, colIndex]) + TransUtil.ToDecimal(master.Temp1) + "";
                }

            }
            return acctTables;
        }
        //把二维数组转换成List
        private IList TransListFromArrays(IList list, string[,] acctTables)
        {
            for (int i = 0; i < acctTables.GetLength(0); i++)
            {
                DataDomain domain = (DataDomain)list[i];
                for (int j = 0; j < acctTables.GetLength(1); j++)
                {
                    switch (j)
                    {
                        case (0):
                            domain.Name3 = acctTables[i, j];
                            break;
                        case (1):
                            domain.Name4 = acctTables[i, j];
                            break;
                        case (2):
                            domain.Name5 = acctTables[i, j];
                            break;
                        case (3):
                            domain.Name6 = acctTables[i, j];
                            break;
                        case (4):
                            domain.Name7 = acctTables[i, j];
                            break;
                        case (5):
                            domain.Name8 = acctTables[i, j];
                            break;
                        case (6):
                            domain.Name9 = acctTables[i, j];
                            break;
                        case (7):
                            domain.Name10 = acctTables[i, j];
                            break;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 通过分公司Syscode集合
        /// </summary>
        private IList GetSubCompanySyscodeList()
        {
            IList list = new ArrayList();//分公司层次码集合
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = "select t1.opgsyscode from resoperationorg t1 where t1.opgstate=1 and t1.opgoperationtype='b'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    list.Add(TransUtil.ToString(dataRow["opgsyscode"]));
                }
            }
            return list;
        }
        /// <summary>
        /// 通过条件查询收款信息
        /// </summary>
        private IList GetGatheringInfoByCondition(string condition)
        {
            IList list = new ArrayList();//分公司组织集合
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.accounttitlecode,sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5 " + condition + " group by t1.accounttitlecode ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GatheringMaster master = new GatheringMaster();
                    master.AccountTitleCode = TransUtil.ToString(dataRow["accounttitlecode"]);
                    master.Temp1 = TransUtil.ToString(dataRow["money"]);
                    list.Add(master);
                }
            }
            return list;
        }
        /// <summary>
        /// 通过条件查询付款信息
        /// </summary>
        private IList GetPaymentInfoByCondition(string condition)
        {
            IList list = new ArrayList();//分公司组织集合
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.accounttitlecode,sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5 " + condition + " group by t1.accounttitlecode ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PaymentMaster master = new PaymentMaster();
                    master.AccountTitleCode = TransUtil.ToString(dataRow["accounttitlecode"]);
                    master.Temp1 = TransUtil.ToString(dataRow["money"]);
                    list.Add(master);
                }
            }
            return list;
        }
        /// <summary>
        /// 通过条件查询费用金额
        /// </summary>
        private Hashtable GetIndirectCostByCondition(string condition)
        {
            Hashtable ht = new Hashtable();
            decimal currencyMoney = 0;//货币上交
            decimal borrowMoney = 0;//借款
            decimal indirectMoney = 0;//间接费用
            decimal manageMoney = 0;//管理费用
            decimal financeMoney = 0;//财务费用
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t2.accounttitlecode, sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                    " where t1.id=t2.parentid and t1.state=5  " +
                                    " and (t2.accounttitlecode like '%4104%' or t2.accounttitlecode like '%2001%' or t2.accounttitlecode like '%54010106%' " +
                                    " or t2.accounttitlecode like '%6602%' or t2.accounttitlecode like '%6603%' ) "
                                    + condition + " group by t2.accounttitlecode ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string titleCode = TransUtil.ToString(dataRow["accounttitlecode"]);
                    if (titleCode.Contains("4104"))
                    {
                        currencyMoney += TransUtil.ToDecimal(dataRow["money"]);
                    }
                    else if (titleCode.Contains("2001"))
                    {
                        borrowMoney += TransUtil.ToDecimal(dataRow["money"]);
                    }
                    else if (titleCode.Contains("54010106"))
                    {
                        indirectMoney += TransUtil.ToDecimal(dataRow["money"]);
                    }
                    else if (titleCode.Contains("6602"))
                    {
                        manageMoney += TransUtil.ToDecimal(dataRow["money"]);
                    }
                    else if (titleCode.Contains("6603"))
                    {
                        financeMoney += TransUtil.ToDecimal(dataRow["money"]);
                    }
                }
            }
            ht.Add("4104", currencyMoney);
            ht.Add("2001", borrowMoney);
            ht.Add("54010106", indirectMoney);
            ht.Add("6602", manageMoney);
            ht.Add("6603", financeMoney);
            return ht;
        }
        /// <summary>
        /// 通过条件查询货币上交金额
        /// </summary>
        private decimal GetCurrencyMoneyByCondition(string condition)
        {
            decimal currencyMoney = 0;//货币上交
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                    " where t1.id=t2.parentid and t1.state=5  " +
                                    " and t2.accounttitlecode like '%4104%'  "
                                    + condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    currencyMoney = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            return currencyMoney;
        }
        /// <summary>
        /// 通过条件查询保理数据
        /// </summary>
        private decimal GetFactoringDataByCondition(string condition)
        {
            decimal sumMoney = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select sum(t1.summoney) money From thd_factoringdatamaster t1  where t1.state=5 " + condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    sumMoney = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            return sumMoney;
        }
        /// <summary>
        /// 通过条件查询借款数据
        /// </summary>
        private decimal GetBorrowOrderByCondition(string condition)
        {
            decimal sumMoney = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select sum(t2.money) money From thd_borrowedordermaster t1,thd_borrowedorderdetail t2 " +
                                  " where t1.id=t2.parentid and t1.state=5 " + condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    sumMoney = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            return sumMoney;
        }
        /// <summary>
        /// 通过条件查询本期的期初金额
        /// </summary>
        private decimal GetCurrInitMoneyByCondition(string condition)
        {
            decimal sumMoney = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            if (condition.Contains("projectid = "))
            {
                command.CommandText = " select -sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                    " where t1.id=t2.parentid and t1.state=5 and (t2.accounttitlecode like '%4104%' " +
                                    " or t2.accounttitlecode like '%54010106%' " +
                                    " or t2.accounttitlecode like '%6602%' or t2.accounttitlecode like '%6603%' ) " + condition +
                                    " union all " +
                                    " select -sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5 " + condition +
                                    " union all " +
                                    " select sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5 " + condition;
            }
            else
            {
                command.CommandText = " select nvl(sum(t1.summoney),0) money From thd_factoringdatamaster t1  where t1.state=5 " + condition +
                                    " union all " +
                                    " select sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                    " where t1.id=t2.parentid and t1.state=5 and t2.accounttitlecode like '%2001%' " + condition +
                                    " union all " +
                                    " select -sum(t2.money) money From thd_indirectcostmaster t1,thd_indirectcostdetail t2 " +
                                    " where t1.id=t2.parentid and t1.state=5 and (t2.accounttitlecode like '%4104%' " +
                                    " or t2.accounttitlecode like '%54010106%' " +
                                    " or t2.accounttitlecode like '%6602%' or t2.accounttitlecode like '%6603%' ) " + condition +
                                    " union all " +
                                    " select -sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5 " + condition +
                                    " union all " +
                                    " select sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5 " + condition;
            }
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    sumMoney += TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            return sumMoney;
        }
        /// <summary>
        /// 查询当天的日均金额
        /// <param name="queryType">0;项目,1;分公司</param>
        /// </summary>
        private decimal GetCurrDayAvgMoneyByCondition(string condition, int queryType)
        {
            decimal money = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            if (queryType == 0)
            {
                command.CommandText = " select t1.numericalvalue from thd_fundmanagebyproject t1 where 1=1 " + condition;
            }
            else
            {
                command.CommandText = " select t1.numericalvalue from thd_fundmanagement t1 where 1=1  " + condition;
            }

            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    money = TransUtil.ToDecimal(dataRow["numericalvalue"]);
                }
            }
            return money;
        }
        /// <summary>
        /// 查询当天的月均金额
        /// <param name="queryType">0;项目,1;分公司</param>
        /// <param name="currValue">当天存量</param>
        /// </summary>
        private decimal GetCurrMonthAvgMoneyByCondition(string condition, int queryType, decimal currValue)
        {
            decimal money = 0;
            decimal sumAvgMoney = 0;
            int count = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            if (queryType == 0)
            {
                command.CommandText = " select t1.numericalvalue from thd_fundmanagebyproject t1 where 1=1 " + condition;
            }
            else
            {
                command.CommandText = " select t1.numericalvalue from thd_fundmanagement t1 where 1=1  " + condition;
            }

            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    sumAvgMoney += TransUtil.ToDecimal(dataRow["numericalvalue"]);
                    count++;
                }
            }
            money = decimal.Round((sumAvgMoney + currValue) / (count + 1), 2);
            return money;
        }
        /// <summary>
        /// 通过组织ID查询项目ID
        /// </summary>
        public string GetProjectIDByOperationOrg(string opgID)
        {
            string projectID = "";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.id from resconfig t1 where t1.ownerorg='" + opgID + "' and nvl(t1.projectcurrstate,0) != 20 and t1.projectcode != '0000'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    projectID = TransUtil.ToString(dataRow["id"]);
                }
            }
            return projectID;
        }

        public CurrentProjectInfo GetProjectByOperationOrg(string opgSysCode)
        {
            if (string.IsNullOrEmpty(opgSysCode))
            {
                return null;
            }

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectCurrState", 0));
            objectQuery.AddCriterion(Expression.Eq("OwnerOrgSysCode", opgSysCode));

            var retList = Dao.ObjectQuery(typeof (CurrentProjectInfo), objectQuery);
            if(retList==null || retList.Count==0)
            {
                return null;
            }

            return retList.OfType<CurrentProjectInfo>().ToList().FirstOrDefault();
        }

        /// <summary>
        /// 查询项目/分公司的日均/月均信息
        /// <param name="queryType">0;项目,1;分公司</param>
        /// </summary>
        private IList GetProjectIndexInfoByCondition(IList list, DateTime currDate, int queryType)
        {
            DateTime yearFirstDate = new DateTime(currDate.Year, 1, 1);//当年第一天
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            if (queryType == 0)
            {
                command.CommandText = " select t1.projectid sysid, t1.numericalvalue money from thd_fundmanagebyproject t1 where 1=1 " +
                                      " and t1.timename='日均' and t1.indexname='资金存量' " +
                                      " and t1.createdate=to_date('" + currDate.AddDays(-1).ToShortDateString() + "','yyyy-mm-dd')";
            }
            else
            {
                command.CommandText = " select t1.opgsyscode sysid,t1.numericalvalue money from thd_fundmanagement t1 where 1=1  " +
                                      " and t1.timename='日均' and t1.indexname='资金存量' and t1.organizationlevel='分公司' " +
                                      " and t1.createdate=to_date('" + currDate.AddDays(-1).ToShortDateString() + "','yyyy-mm-dd')";
            }

            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string sysid = TransUtil.ToString(dataRow["sysid"]);
                    decimal money = TransUtil.ToDecimal(dataRow["money"]);
                    foreach (DataDomain domain in list)
                    {
                        if (queryType == 0)//项目
                        {
                            if (TransUtil.ToString(domain.Name30) == sysid)
                            {
                                domain.Name12 = money;
                            }
                        }
                        else
                        {
                            if (sysid.Contains(TransUtil.ToString(domain.Name30)))
                            {
                                domain.Name12 = money;
                            }
                        }
                    }
                }
            }

            if (queryType == 0)
            {
                command.CommandText = " select t1.projectid sysid, sum(t1.numericalvalue) money,count(*) count from thd_fundmanagebyproject t1 where 1=1 " +
                                     " and t1.timename='月末' and t1.indexname='资金存量' " +
                                        " and t1.createdate>=to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                        " and t1.createdate<to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') group by t1.projectid ";
            }
            else
            {
                command.CommandText = " select t1.opgsyscode sysid,sum(t1.numericalvalue) money,count(*) count from thd_fundmanagement t1 where 1=1  " +
                                      " and t1.timename='月末' and t1.indexname='资金存量' and t1.organizationlevel='分公司' " +
                                        " and t1.createdate>=to_date('" + yearFirstDate.ToShortDateString() + "','yyyy-mm-dd') " +
                                        " and t1.createdate<to_date('" + currDate.ToShortDateString() + "','yyyy-mm-dd') group by t1.opgsyscode";
            }
            dataReader = command.ExecuteReader();
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string sysid = TransUtil.ToString(dataRow["sysid"]);
                    decimal money = TransUtil.ToDecimal(dataRow["money"]);
                    int count = TransUtil.ToInt(dataRow["count"]);
                    foreach (DataDomain domain in list)
                    {
                        if (queryType == 0)//项目
                        {
                            if (TransUtil.ToString(domain.Name30) == sysid)
                            {
                                decimal currLeftMoney = TransUtil.ToDecimal(domain.Name2) + TransUtil.ToDecimal(domain.Name5)
                                                                    - TransUtil.ToDecimal(domain.Name9);
                                domain.Name14 = decimal.Round((currLeftMoney + money) / (count + 1), 2);
                            }
                        }
                        else
                        {
                            if (sysid.Contains(TransUtil.ToString(domain.Name30)))
                            {
                                decimal currLeftMoney = TransUtil.ToDecimal(domain.Name2) + TransUtil.ToDecimal(domain.Name5)
                                                                   - TransUtil.ToDecimal(domain.Name9);
                                domain.Name14 = decimal.Round((currLeftMoney + money) / (count + 1), 2);
                            }
                        }
                    }

                }
            }
            return list;
        }
        /// <summary>
        /// 查询项目资金存量情况(日现金流详细)
        /// 收款-付款+借款-费用
        /// </summary>
        private IList GetProjectLeftCashInfoByCondition(IList list, string condition, string syscode, int queryType)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //项目信息
            if (queryType == 0)//查询项目
            {
                command.CommandText = " select t.projectid sysid, sum(t.money) money from ( " +
                                      " select t1.projectid,sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5 " + condition +
                                      " and t1.projectid is not null group by t1.projectid " +
                                      " union all " +
                                      " select t1.projectid,-sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5  " + condition +
                                      " and t1.projectid is not null group by t1.projectid " +
                                      " union all " +
                                      " select t1.projectid,sum(t2.money) money From thd_borrowedordermaster t1,thd_borrowedorderdetail t2 where t1.id=t2.parentid and t1.state=5 " + condition +
                                      " and t1.projectid is not null group by t1.projectid " +
                                      " union all " +
                                      " select t1.projectid,-sum(t1.money) money From thd_indirectcostmaster t1 where t1.state=5  " + condition +
                                      " and t1.projectid is not null group by t1.projectid ) t group by t.projectid ";
            }
            else
            {//分公司
                command.CommandText = " select t.opgsyscode sysid, sum(t.money) money from ( " +
                                  " select t1.opgsyscode,sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5 " + condition +
                                  " group by t1.opgsyscode " +
                                  " union all " +
                                  " select t1.opgsyscode,-sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5   " + condition +
                                  " group by t1.opgsyscode " +
                                  " union all " +
                                  " select t1.opgsyscode,sum(t2.money) money From thd_borrowedordermaster t1,thd_borrowedorderdetail t2 " +
                                  " where t1.id=t2.parentid and t1.projectid is null and t1.state=5 " + condition +
                                  " group by t1.opgsyscode " +
                                  " union all " +
                                  " select t1.opgsyscode,-sum(t1.money) money From thd_indirectcostmaster t1 where t1.state=5  " + condition +
                                  " group by t1.opgsyscode ) t group by t.opgsyscode ";
            }
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string sysid = TransUtil.ToString(dataRow["sysid"]);
                    decimal money = TransUtil.ToDecimal(dataRow["money"]);
                    foreach (DataDomain domain in list)
                    {
                        if (queryType == 0)//项目
                        {
                            if (TransUtil.ToString(domain.Name30) == sysid)
                            {
                                domain.Name2 = TransUtil.ToDecimal(domain.Name2) + money;
                            }
                        }
                        else
                        {
                            if (sysid.Contains(TransUtil.ToString(domain.Name30)))
                            {
                                domain.Name2 = TransUtil.ToDecimal(domain.Name2) + money;
                            }
                        }
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 通过条件查询日现金流流出信息(付款+费用)
        /// </summary>
        /// <param name="list">返回结果集合</param>
        /// <param name="syscode">组织层次码</param>
        /// <param name="queryType">0:项目,1:分公司</param>
        /// <param name="displayIndex">数据的索引</param>
        private IList GetCashOutByCondition(IList list, string condition, string syscode, int queryType, int displayIndex)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            if (queryType == 0)//查询项目
            {
                command.CommandText = " select t.projectid sysid, sum(t.money) money from ( " +
                                  " select t1.projectid,sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5  " +
                                  " and t1.opgsyscode like '%" + syscode + "%'" + condition +
                                  " and t1.projectid is not null group by t1.projectid " +
                                  " union all " +
                                  " select t1.projectid,sum(t1.money) money From thd_indirectcostmaster t1 where t1.state=5  " +
                                  " and t1.opgsyscode like '%" + syscode + "%'" + condition +
                                  " and t1.projectid is not null group by t1.projectid ) t group by t.projectid ";
            }
            else
            {
                //分公司信息
                command.CommandText = " select t.opgsyscode sysid, sum(t.money) money from ( " +
                                      " select t1.opgsyscode,sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5   " + condition +
                                      " group by t1.opgsyscode " +
                                      " union all " +
                                      " select t1.opgsyscode,sum(t1.money) money From thd_indirectcostmaster t1 where t1.state=5  " + condition +
                                      " group by t1.opgsyscode ) t group by t.opgsyscode ";
            }
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string sysid = TransUtil.ToString(dataRow["sysid"]);
                    decimal money = TransUtil.ToDecimal(dataRow["money"]);
                    foreach (DataDomain domain in list)
                    {
                        if (queryType == 0)//项目
                        {
                            if (TransUtil.ToString(domain.Name30) == sysid)
                            {
                                if (displayIndex == 7)//流出当日
                                {
                                    domain.Name7 = TransUtil.ToDecimal(domain.Name7) + money;
                                }
                                else if (displayIndex == 8)//流出本月
                                {
                                    domain.Name8 = TransUtil.ToDecimal(domain.Name8) + money;
                                }
                                else if (displayIndex == 9)//流出本年
                                {
                                    domain.Name9 = TransUtil.ToDecimal(domain.Name9) + money;
                                }
                                else if (displayIndex == 10)//流出累计
                                {
                                    domain.Name10 = TransUtil.ToDecimal(domain.Name10) + money;
                                }
                            }
                        }
                        else
                        {
                            if (sysid.Contains(TransUtil.ToString(domain.Name30)))
                            {
                                if (displayIndex == 7)//流出当日
                                {
                                    domain.Name7 = TransUtil.ToDecimal(domain.Name7) + money;
                                }
                                else if (displayIndex == 8)//流出本月
                                {
                                    domain.Name8 = TransUtil.ToDecimal(domain.Name8) + money;
                                }
                                else if (displayIndex == 9)//流出本年
                                {
                                    domain.Name9 = TransUtil.ToDecimal(domain.Name9) + money;
                                }
                                else if (displayIndex == 10)//流出累计
                                {
                                    domain.Name10 = TransUtil.ToDecimal(domain.Name10) + money;
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 通过条件查询日现金流流入信息(收款+借款)
        /// </summary>
        /// <param name="list">返回结果集合</param>
        /// <param name="syscode">组织层次码</param>
        /// <param name="queryType">0:项目,1:分公司</param>
        /// <param name="displayIndex">数据的索引</param>
        private IList GetCashInByCondition(IList list, string condition, string syscode, int queryType, int displayIndex)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            if (queryType == 0)//查询项目
            {
                command.CommandText = " select t.projectid sysid, sum(t.money) money from ( " +
                                  " select t1.projectid,sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5  " +
                                  " and t1.opgsyscode like '%" + syscode + "%'" + condition +
                                  " and t1.projectid is not null group by t1.projectid " +
                                  " union all " +
                                  " select t1.projectid,sum(t2.money) money From thd_borrowedordermaster t1,thd_borrowedorderdetail t2 where t1.id=t2.parentid and t1.state=5  " +
                                  " and t1.opgsyscode like '%" + syscode + "%'" + condition +
                                  " and t1.projectid is not null group by t1.projectid ) t group by t.projectid ";
            }
            else
            {
                //分公司信息
                command.CommandText = " select t.opgsyscode sysid, sum(t.money) money from ( " +
                                      " select t1.opgsyscode,sum(t1.summoney) money From thd_gatheringmaster t1 where t1.state=5   " + condition +
                                      " group by t1.opgsyscode " +
                                      " union all " +
                                      " select t1.opgsyscode,sum(t2.money) money From thd_borrowedordermaster t1,thd_borrowedorderdetail t2 " +
                                      " where t1.id=t2.parentid and t1.projectid is null and t1.state=5  " + condition +
                                      " group by t1.opgsyscode ) t group by t.opgsyscode ";
            }
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string sysid = TransUtil.ToString(dataRow["sysid"]);
                    decimal money = TransUtil.ToDecimal(dataRow["money"]);
                    foreach (DataDomain domain in list)
                    {
                        if (queryType == 0)//项目
                        {
                            if (TransUtil.ToString(domain.Name30) == sysid)
                            {
                                if (displayIndex == 3)//流入当日
                                {
                                    domain.Name3 = TransUtil.ToDecimal(domain.Name3) + money;
                                }
                                else if (displayIndex == 4)//流入本月
                                {
                                    domain.Name4 = TransUtil.ToDecimal(domain.Name4) + money;
                                }
                                else if (displayIndex == 5)//流入本年
                                {
                                    domain.Name5 = TransUtil.ToDecimal(domain.Name5) + money;
                                }
                                else if (displayIndex == 6)//流入累计
                                {
                                    domain.Name6 = TransUtil.ToDecimal(domain.Name6) + money;
                                }
                            }
                        }
                        else
                        {
                            if (sysid.Contains(TransUtil.ToString(domain.Name30)))
                            {
                                if (displayIndex == 3)//流入当日
                                {
                                    domain.Name3 = TransUtil.ToDecimal(domain.Name3) + money;
                                }
                                else if (displayIndex == 4)//流入本月
                                {
                                    domain.Name4 = TransUtil.ToDecimal(domain.Name4) + money;
                                }
                                else if (displayIndex == 5)//流入本年
                                {
                                    domain.Name5 = TransUtil.ToDecimal(domain.Name5) + money;
                                }
                                else if (displayIndex == 6)//流入累计
                                {
                                    domain.Name6 = TransUtil.ToDecimal(domain.Name6) + money;
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        #endregion

        #region 报表台账
        public DataSet QueryManageCost(string sOrgSysCode, string sProject, DateTime dStart, DateTime dEnd)
        {
            DataSet ds = new DataSet();
            DataSet dsTemp = null;
            dsTemp = QueryManageCostGroupAccountTitle(sOrgSysCode, sProject, dStart, dEnd);
            ds.Tables.Add(dsTemp == null || dsTemp.Tables.Count == 0 ? new DataTable() : dsTemp.Tables[0]);
            dsTemp = QueryManageCostGroupOrg(sOrgSysCode, sProject, dStart, dEnd);
            ds.Tables.Add(dsTemp == null || dsTemp.Tables.Count == 0 ? new DataTable() : dsTemp.Tables[0]);
            return ds;
        }

        public DataSet QueryManageCostGroupAccountTitle(string sOrgSysCode, string sProjectID, DateTime dStart, DateTime dEnd)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            var subCompanys = GetSubCompanySyscodeList();
            var isSubCompany = subCompanys != null && subCompanys.Contains(sOrgSysCode);
         
            //   
            //EnumCostType.管理费用
            string sSQL = @"select t1.accounttitlename,sum(t1.budgetmoney) budgetmoney,sum(t1.money) money 
from thd_indirectcostmaster t
join thd_indirectcostdetail t1 on t.id=t1.parentid 
where  t.state=5 and t1.costtype={2} and t1.accountsymbol={3}
and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') ";
            sSQL = string.Format(sSQL, dStart.ToString("yyyy-MM-dd"), dEnd.ToString("yyyy-MM-dd"), (int)EnumCostType.管理费用, (int)EnumAccountSymbol.其他);
            if (string.IsNullOrEmpty(sOrgSysCode))
            {
                sSQL = string.Format("{0} and t.projectid = '{1}' group by  t1.accounttitlename", sSQL, sProjectID);
            }
            else if (isSubCompany)
            {
                sSQL = string.Format("{0} and t.opgsyscode like '{1}%'  and t.projectid is null  group by  t1.accounttitlename", sSQL, sOrgSysCode);
            }
            else
            {
                sSQL = string.Format("{0} and t.opgsyscode like '{1}%'  and t.projectid is null and not exists(select 1 from resoperationorg o where o.opgoperationtype = 'b' and instr(t.opgsyscode, o.opgsyscode)>0 )  group by  t1.accounttitlename", sSQL, sOrgSysCode);
            }
            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }

        public DataSet QueryManageCostGroupOrg(string sOrgSysCode, string sProjectID, DateTime dStart, DateTime dEnd)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            var subCompanys = GetSubCompanySyscodeList();
            var isSubCompany = subCompanys != null && subCompanys.Contains(sOrgSysCode);
            string sSQL=string.Empty ;

            //   
            //EnumCostType.管理费用
             
            
            
            if (string.IsNullOrEmpty(sOrgSysCode))
            {
                sSQL = @"select t1.orginfoname,sum(t1.budgetmoney) budgetmoney,sum(t1.money) money 
from thd_indirectcostmaster t
join thd_indirectcostdetail t1 on t.id=t1.parentid 
where t.state=5 and t1.costtype={2} and t1.accountsymbol={3}
and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') ";
                sSQL = string.Format(sSQL, dStart.ToString("yyyy-MM-dd"), dEnd.AddDays(1).ToString("yyyy-MM-dd"), (int)EnumCostType.管理费用, (int)EnumAccountSymbol.其他);
                sSQL = string.Format("{0} and t.projectid = '{1}' group by  t1.orginfoname", sSQL, sProjectID);
            }
            else if (isSubCompany)
            {
                sSQL = @"select t1.orginfoname,sum(t1.budgetmoney) budgetmoney,sum(t1.money) money 
from thd_indirectcostmaster t
join thd_indirectcostdetail t1 on t.id=t1.parentid 
where t.state=5 and t1.costtype={2} and t1.accountsymbol={3}
and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')  and t.projectid is null ";
                sSQL = string.Format(sSQL, dStart.ToString("yyyy-MM-dd"), dEnd.AddDays(1).ToString("yyyy-MM-dd"), (int)EnumCostType.管理费用, (int)EnumAccountSymbol.其他);
                sSQL = string.Format("{0} and t.opgsyscode like '{1}%' group by  t1.orginfoname", sSQL, sOrgSysCode);
            }
            else
            {
               sSQL= @"select t1.orginfoname,sum(t1.budgetmoney) budgetmoney,sum(t1.money) money 
from thd_indirectcostmaster t
join thd_indirectcostdetail t1 on t.id=t1.parentid 
where t.state=5 and t1.costtype={2} and t1.accountsymbol={3}
and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')   and t.opgsyscode like '{4}%'  and t.projectid is null
                      and not exists(
                          select 1 from resoperationorg o 
                          where o.opgoperationtype = 'b' and instr(t.opgsyscode, o.opgsyscode)>0
                      )  group by  t1.orginfoname ";
               sSQL = string.Format(sSQL, dStart.ToString("yyyy-MM-dd"), dEnd.ToString("yyyy-MM-dd"), (int)EnumCostType.管理费用, (int)EnumAccountSymbol.其他, sOrgSysCode);
            }
            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }

        public DataSet QueryIndirectCostGroupAccountTitle(string sOrgSysCode, string sProjectId, DateTime dStart, DateTime dEnd)
        {
            string sSQL = string.Empty;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //   
            //EnumCostType.管理费用
            if (string.IsNullOrEmpty(sOrgSysCode))
            {
                sSQL = @"select t1.accounttitleid,t1.accounttitlename,(select sum(tt1.budgetmoney)  from thd_indirectcostmaster tt
join thd_indirectcostdetail tt1 on tt.id=tt1.parentid 
where  tt.state=5 and tt1.costtype=0 and tt1.accountsymbol=4  and tt1.accounttitleid=t1.accounttitleid
 and tt.createdate between to_date(substr('{1}',1,4)||'-01-01','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
 and tt.projectid  ='{2}' ) budgetmoney,sum(t1.money) money 
from thd_indirectcostmaster t
join thd_indirectcostdetail t1 on t.id=t1.parentid 
where t.state=5 and t1.costtype={3} and t1.accountsymbol={4}
and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') and t.projectid = '{2}'
group by t1.accounttitleid, t1.accounttitlename";
                sSQL = string.Format(sSQL, dStart.ToString("yyyy-MM-dd"), dEnd.ToString("yyyy-MM-dd"), sProjectId, (int)EnumCostType.间接费用, (int)EnumAccountSymbol.其他);
            }
            else
            {
                sSQL = @"select t1.accounttitleid,t1.accounttitlename,(select sum(tt1.budgetmoney)  from thd_indirectcostmaster tt
join thd_indirectcostdetail tt1 on tt.id=tt1.parentid 
where tt.state=5 and tt1.costtype=0 and tt1.accountsymbol=4  and tt1.accounttitleid=t1.accounttitleid
 and tt.createdate between to_date(substr('{1}',1,4)||'-01-01','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
 and tt.opgsyscode like '{2}%' ) budgetmoney,sum(t1.money) money 
from thd_indirectcostmaster t
join thd_indirectcostdetail t1 on t.id=t1.parentid 
where t.state=5 and t1.costtype={3} and t1.accountsymbol={4}
and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') and t.opgsyscode like '{2}%'
group by  t1.accounttitleid,t1.accounttitlename";
                sSQL = string.Format(sSQL, dStart.ToString("yyyy-MM-dd"), dEnd.ToString("yyyy-MM-dd"), sOrgSysCode, (int)EnumCostType.间接费用, (int)EnumAccountSymbol.其他);
            }

            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }

        public DataSet QueryIndirectCostGroupProject(string sOrgSysCode, DateTime dStart, DateTime dEnd)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //   
            //EnumCostType.管理费用
            string sSQL = @"select * from (
select (select tt1.opgname  from resconfig tt 
join resoperationorg tt1 on tt1.opgoperationtype='b'  and instr(tt.ownerorgsyscode,tt1.opgid)>0 
where tt.id=t.projectid) as opgname, t.projectid ,t.projectname,(select sum(tt1.budgetmoney)  from thd_indirectcostmaster tt
join thd_indirectcostdetail tt1 on tt.state=5 and tt.id=tt1.parentid 
where  tt1.costtype=0 and tt1.accountsymbol=4  and tt.projectid=t.projectid
 and tt.createdate between to_date(substr('{1}',1,4)||'-01-01','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
 and tt.opgsyscode like '{2}%' )as budgetmoney,sum(t1.money) money 
from thd_indirectcostmaster t
join thd_indirectcostdetail t1 on t.id=t1.parentid 
where  t.state=5 and t1.costtype={3} and t1.accountsymbol={4}
and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') and t.opgsyscode like '{2}%'
group by  t.projectid ,t.projectname) order by opgname,projectname";
            sSQL = string.Format(sSQL, dStart.ToString("yyyy-MM-dd"), dEnd.ToString("yyyy-MM-dd"), sOrgSysCode, (int)EnumCostType.间接费用, (int)EnumAccountSymbol.其他);
            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }

        public DataSet QueryOwnerQuantityReport(string sProjectID, string sOrgSysCode, DateTime dStart, DateTime dEnd)
        {
            string sSQL = string.Empty;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            if (string.IsNullOrEmpty(sOrgSysCode))
            {
                sSQL = @"select t1.quantitydate,t1.submitquantity,t.projectname,
(case when t.quantitytype='土建' then t1.submitquantity else 0 end ) tjsubmitquantity,
(case when t.quantitytype='安装' then t1.submitquantity else 0 end ) azsubmitquantity,
(case when t.quantitytype='装饰' then t1.submitquantity else 0 end ) zssubmitquantity,
(case when t.quantitytype='土建' then t1.qwbsname else n'' end ) tjsqwbsname,
(case when t.quantitytype='安装' then t1.qwbsname else n'' end ) azqwbsname,
(case when t.quantitytype='装饰' then t1.qwbsname else n'' end ) zsqwbsname,
t1.confirmdate,t1.confirmstartdate,t1.confirmenddate,
(case when t.quantitytype='土建' then t1.confirmmoney else 0 end ) tjconfirmmoney,
(case when t.quantitytype='安装' then t1.confirmmoney else 0 end ) azconfirmmoney,
(case when t.quantitytype='装饰' then t1.confirmmoney else 0 end ) zsconfirmmoney,
t1.confirmmoney,t1.GatheringRate,t1.acctgatheringmoney,t1.Descript
 from thd_ownerquantitymaster t
join thd_ownerquantitydetail t1 on t.id=t1.parentid
where  t.state=5 and  t1.confirmdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
and t.projectid ='{2}' order by t1.quantitydate desc ";
                sSQL = string.Format(sSQL, dStart.ToString("yyyy-MM-dd"), dEnd.ToString("yyyy-MM-dd"), sProjectID);
            }
            else
            {
                sSQL = @"select (select t1.opgname  from resconfig t 
join resoperationorg t1 on t1.opgoperationtype='b'  and instr(t.ownerorgsyscode,t1.opgid)>0 
where t.id=tt.projectid) as opgname,tt.* ,(select max(ttt1.gatheringrate) from thd_ownerquantitymaster ttt 
join thd_ownerquantitydetail ttt1 on ttt.id=ttt1.parentid where  ttt.state=5 and ttt.projectid=tt.projectid and (ttt1.confirmdate between to_date('{0}','yyyy-mm-dd') and tt.confirmdate))  gatheringrate
from(select projectname,projectid,sum(submitquantity),
sum(tjsubmitquantity)tjsubmitquantity,sum(azsubmitquantity)azsubmitquantity,sum(zssubmitquantity)zssubmitquantity,
sum(tjconfirmmoney)tjconfirmmoney,sum(azconfirmmoney)azconfirmmoney,sum(zsconfirmmoney)zsconfirmmoney,
sum(confirmmoney)confirmmoney,max(confirmdate)confirmdate,sum(acctgatheringmoney)acctgatheringmoney
from (
select t1.submitquantity,t.projectname,t.projectid,
(case when t.quantitytype='土建' then t1.submitquantity else 0 end ) tjsubmitquantity,
(case when t.quantitytype='安装' then t1.submitquantity else 0 end ) azsubmitquantity,
(case when t.quantitytype='装饰' then t1.submitquantity else 0 end ) zssubmitquantity,
(case when t.quantitytype='土建' then t1.confirmmoney else 0 end ) tjconfirmmoney,
(case when t.quantitytype='安装' then t1.confirmmoney else 0 end ) azconfirmmoney,
(case when t.quantitytype='装饰' then t1.confirmmoney else 0 end ) zsconfirmmoney,
t1.confirmmoney,t1.GatheringRate,t1.acctgatheringmoney,t1.confirmdate
 from thd_ownerquantitymaster t
join thd_ownerquantitydetail t1 on t.id=t1.parentid
where  t.state=5 and t1.confirmdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')   and 
t.opgsyscode like '{2}%')
group by projectname ,projectid) tt order by  opgname,projectname desc";
                sSQL = string.Format(sSQL, dStart.ToString("yyyy-MM-dd"), dEnd.ToString("yyyy-MM-dd"), sOrgSysCode);
            }

            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }

        public DataSet QueryFinancialAccount(string sProjectID, string sOrgSysCode, DateTime dStart, DateTime dEnd, EnumAccountSymbol AccountSymbol, bool isIncludeProj)
        {
            string sSQL = string.Empty;
            DataSet ds = null;

            var subCompanys = GetSubCompanySyscodeList();
            var isSubCompany = subCompanys != null && subCompanys.Contains(sOrgSysCode);

            switch (AccountSymbol)
            {
                case EnumAccountSymbol.财务费用标志:
                    if (!string.IsNullOrEmpty(sProjectID))
                    {
                        sSQL = string.Format(SQLScript.FinanceCostByProjectSql,
                            dStart.Date.ToString("yyyy-MM-dd"), dEnd.Date.ToString("yyyy-MM-dd"), sProjectID);
                    }
                    else if (isSubCompany)
                    {
                        sSQL = string.Format(SQLScript.SubFinanceCostSql,
                            dStart.Date.ToString("yyyy-MM-dd"), dEnd.Date.ToString("yyyy-MM-dd"),
                            sOrgSysCode, isIncludeProj ? "" : "and t.projectid is null");
                    }
                    else
                    {
                        sSQL = string.Format(SQLScript.FinanceCostSql,
                            dStart.Date.ToString("yyyy-MM-dd"), dEnd.Date.ToString("yyyy-MM-dd"),
                            sOrgSysCode, isIncludeProj ? "" : "and t.projectid is null");
                    }
                    break;
                case EnumAccountSymbol.借款标志:
                    {
                        if (!string.IsNullOrEmpty(sProjectID))
                        {
                            sSQL = string.Format(SQLScript.BorrowMoneyByProjectSql, 
                                dStart.Date.ToString("yyyy-MM-dd"), dEnd.Date.ToString("yyyy-MM-dd"), sProjectID);
                        }
                        else if (isSubCompany)
                        {
                            sSQL = string.Format(SQLScript.SubBorrowMoneySql, 
                                dStart.Date.ToString("yyyy-MM-dd"), dEnd.Date.ToString("yyyy-MM-dd"), sOrgSysCode);
                        }
                        else
                        {
                            sSQL = string.Format(SQLScript.BorrowMoneySql, 
                                dStart.Date.ToString("yyyy-MM-dd"), dEnd.Date.ToString("yyyy-MM-dd"), sOrgSysCode);
                        }
                        break;
                    }
                case EnumAccountSymbol.利润标志:

                    break;
                case EnumAccountSymbol.上交标志:
                    if (!string.IsNullOrEmpty(sProjectID))
                    {
                        sSQL = string.Format(SQLScript.CurrencyExchangeByProjectSql, 
                            dStart.Date.ToString("yyyy-MM-dd"), dEnd.Date.ToString("yyyy-MM-dd"), sProjectID);
                    }
                    if (isSubCompany)
                    {
                        sSQL = string.Format(SQLScript.SubCurrencyExchangeSql, 
                            dStart.Date.ToString("yyyy-MM-dd"), dEnd.Date.ToString("yyyy-MM-dd"), sOrgSysCode);
                    }
                    else
                    {
                        sSQL = string.Format(SQLScript.CurrencyExchangeSql, 
                            dStart.Date.ToString("yyyy-MM-dd"), dEnd.Date.ToString("yyyy-MM-dd"), sOrgSysCode);
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(sSQL))
            {
                ISession oSession = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = oSession.Connection;
                IDbCommand comm = conn.CreateCommand();
                comm.CommandText = sSQL;
                IDataReader reader = comm.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(reader);
            }

            return ds;
        }
        #endregion

        public ProjectFundPlanMaster GetProjectFundPlanMaster(string projctId, int year, int month,string type) 
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectType", type));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projctId));
            objectQuery.AddCriterion(Expression.Eq("CreateYear", year));
            objectQuery.AddCriterion(Expression.Eq("CreateMonth", month));
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = Dao.ObjectQuery(typeof(ProjectFundPlanMaster), objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ProjectFundPlanMaster;
            }
            return null;
        }

        public void SaveProjectFundPlan(ProjectFundPlanMaster obj) 
        {
            if (obj.Id == null)
            {
                //obj.Code = GetCode(typeof(ProjectFundPlanMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            Dao.SaveOrUpdate(obj);
        }
    }
}