using System.Collections;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
//using Application.Business.Erp.SupplyChain.ReceivingManage.CusMoneyMng.Domain;
//using Application.Business.Erp.SupplyChain.ReceivingManage.ProcessingInvoiceMng.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using NHibernate;
using NC = NHibernate.Criterion;
using VirtualMachine.Core.Expression;
using VirtualMachine.Core;
using VirtualMachine.Core.DataAccess;
using System;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Resource.CommonClass.Domain;
using System.Reflection;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using System.Collections.Generic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using System.Data.OracleClient;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.ConcreteManage.Service;
using FetchMode = NHibernate.FetchMode;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Service
{
    public class AppSrv : BaseService, IAppSrv
    {
        private ICategoryNodeService nodeSrv;
        public ICategoryNodeService NodeSrv
        {
            get { return nodeSrv; }
            set { nodeSrv = value; }
        }

        private IResourceRequirePlanSrv planSrv;
        public IResourceRequirePlanSrv PlanSrv
        {
            get { return planSrv; }
            set { planSrv = value; }
        }

        private IProjectTaskAccountSrv accSrv;
        /// <summary>
        /// 核算单服务
        /// </summary>
        public IProjectTaskAccountSrv AccSrv
        {
            get { return accSrv; }
            set { accSrv = value; }
        }

        private ISubContractBalanceBillSrv subBalSrv;
        public ISubContractBalanceBillSrv SubBalSrv
        {
            get { return subBalSrv; }
            set { subBalSrv = value; }
        }
        private IConcreteManSrv concreteSrv;
        public IConcreteManSrv ConcreteSrv
        {
            get { return concreteSrv; }
            set { concreteSrv = value; }
        }
        public bool IfSendMessage()
        {
            string ifMessage = System.Configuration.ConfigurationSettings.AppSettings["IfSendMessage"];
            if (ifMessage == null || ifMessage.ToUpper() == "TRUE")
            {
                return true;
            }
            return false;
        }

        #region 基本方法
        public DateTime GetServerDateTime()
        {
            return DateTime.Now;
        }
        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetOpeOrgsByInstance()
        {
            //CategoryTree tree = InitTree("业务组织", typeof(OperationOrg));
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(NHibernate.Criterion.Order.Asc("Level"));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));
            IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(OperationOrg), oq);
            return list;

        }

        public object GetDomain(Type t, ObjectQuery o)
        {
            IList ls = base.GetDomainByCondition(t, o);
            if (ls.Count > 0)
            {
                return ls[0];
            }
            else
            {
                return null;
            }
        }
        [TransManager]
        public object Save(object o)
        {
            return base.SaveOrUpdateByDao(o);
        }
        [TransManager]
        public IList Save(IList l)
        {
            return base.SaveOrUpdateByDao(l);
        }
        [TransManager]
        public bool Delete(object o)
        {
            try
            {
                base.DeleteByDao(o);

            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        [TransManager]
        public AppSolutionSet DeleteAppStep(IList lstDel, AppSolutionSet oAppSolutionSet)
        {
            bool bFlag = false;
            try
            {
                foreach (AppStepsSet oDelStep in lstDel)
                {
                    if (oDelStep != null)
                    {
                        oAppSolutionSet.AppStepsSets.Remove(oDelStep);
                        foreach (AppStepsSet oStep in oAppSolutionSet.AppStepsSets)
                        {
                            if (oDelStep.StepOrder < oStep.StepOrder)
                            {
                                oStep.StepOrder -= 1;
                            }
                        }
                    }
                }

                bFlag = dao.SaveOrUpdate(oAppSolutionSet);

            }
            catch
            {
            }
            return bFlag ? oAppSolutionSet : null;
        }
        [TransManager]
        public bool Delete(IList l)
        {
            try
            {
                base.dao.Delete(l);

            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 返回SQL
        /// </summary>
        /// <param name="_SQL"></param>
        /// <returns></returns>
        public DataSet GetStockInRefier(string _SQL)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;

            IDbCommand cmd = cnn.CreateCommand();

            //ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            //tran.Enlist(cmd);
            cmd.CommandText = _SQL;
            cmd.CommandTimeout = 100;
            IDataReader dataReader = cmd.ExecuteReader(CommandBehavior.Default);
            return DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
        }
        #endregion

        #region 查询方法
        public IList GetMasterProperties(string MasterNameSpace)
        {
            Assembly dll = Assembly.LoadFrom(System.Windows.Forms.Application.StartupPath + "\\SupplyChain.dll");
            Type type = dll.GetType(MasterNameSpace);
            IList list = new ArrayList();
            if (type != null)
            {
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    list.Add(propertyInfo);
                }
            }
            return list;
        }

        public IList GetDetailProperties(string DetailNameSpace)
        {
            Assembly dll = Assembly.LoadFrom(System.Windows.Forms.Application.StartupPath + "\\SupplyChain.dll");
            Type type = dll.GetType(DetailNameSpace);
            IList list = new ArrayList();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                list.Add(propertyInfo);
            }
            return list;
        }

        public IList GetDomainByCondition(string ClassName, ObjectQuery oq)
        {
            Assembly dll = Assembly.LoadFrom(System.Windows.Forms.Application.StartupPath + "\\SupplyChain.dll");
            Type type = dll.GetType(ClassName);
            IList list = new ArrayList();
            list = GetDomainByCondition(type, oq);
            return list;
        }
        /// <summary>
        /// 获取主表属性定义
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IList GetAppMasterProperties(string parentId)
        {
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentId.Id", parentId));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("SerialNumber"));

            list = base.GetDomainByCondition(typeof(AppMasterPropertySet), oq);
            return list;
        }
        /// <summary>
        /// 获取明细属性定义
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IList GetAppDetailProperties(string parentId)
        {
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentId.Id", parentId));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("SerialNumber"));

            list = base.GetDomainByCondition(typeof(AppDetailPropertySet), oq);
            return list;
        }
        /// <summary>
        /// 根据审批单据获取审批方案
        /// </summary>
        /// <returns></returns>
        public IList GetAppSolution(AppTableSet AppTable)
        {
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("AppStepsSets", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppStepsSets.AppRoleSets", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppStepsSets.AppRoleSets.AppRole", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ParentId", NHibernate.FetchMode.Eager);
            oq.AddCriterion(Expression.Eq("ParentId.Id", AppTable.Id));
            list = base.GetDomainByCondition(typeof(AppSolutionSet), oq);
            return list;
        }
        /// <summary>
        /// 根据审批单据获取审批方案
        /// </summary>
        /// <returns></returns>
        public AppSolutionSet GetAppSolution(string sSolutionID)
        {
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("AppStepsSets", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppStepsSets.AppRoleSets", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppStepsSets.AppRoleSets.AppRole", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ParentId", NHibernate.FetchMode.Eager);
            oq.AddCriterion(Expression.Eq("Id", sSolutionID));
            list = base.GetDomainByCondition(typeof(AppSolutionSet), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as AppSolutionSet;
            }
            else
            {
                return null;
            }
        }
        public IList GetAppStepsInfo(ObjectQuery oq)
        {
            IList list = new ArrayList();
            oq.AddFetchMode("AppTableSet", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppStepsSet", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppStepsSet.AppRoleSets", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            list = base.GetDomainByCondition(typeof(AppStepsInfo), oq);
            return list;
        }

        public IList GetAppData(Type type, ObjectQuery oq)
        {
            IList list = new ArrayList();
            list = base.GetDomainByCondition(type, oq);

            if (list.Count > 0)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询当前用户的审批表单
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList CurrentUserTableSet(string sql)
        {
            IList ret = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand cmd = cnn.CreateCommand();
            cmd.CommandText = sql;
            IDataReader dr = cmd.ExecuteReader();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Status", 0));
            Disjunction dis = Expression.Disjunction();
            while (dr.Read())
            {
                string id = dr.GetString(0);
                dis.Add(Expression.Eq("Id", id));
            }
            oq.AddCriterion(dis);
            if (dis.Criteria.Count == 0) return ret;
            ret = Dao.ObjectQuery(typeof(AppTableSet), oq);
            return ret;
        }

        public Assembly GetSuppluChainAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
        #endregion

        #region 更新数据方法
        /// <summary>
        /// 更新原始单据主表信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="MasterList"></param>
        private void UpdateBillMaster(AppMasterPropertySet MasterProperty, string PhysicsMasterName)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            string sql = "";
            IDbCommand cmd = cnn.CreateCommand();
            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(cmd);
            if (MasterProperty.DataType == "String" || MasterProperty.DataType == "Number")
            {
                sql = "update " + PhysicsMasterName + " set " + MasterProperty.DBFieldName + "= '" + MasterProperty.TempValue + "' where id='" + MasterProperty.TempBillId + "'";
            }
            else if (MasterProperty.DataType == "DateTime")
            {
                sql = "update " + PhysicsMasterName + " set " + MasterProperty.DBFieldName + "= to_date('" + Convert.ToDateTime(MasterProperty.TempValue).ToShortDateString() + "','yyyy-mm-dd') where id='" + MasterProperty.TempBillId + "'";
            }

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 更新原始单据明细信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="DetailList"></param>
        private void UpdateBillDetail(AppDetailPropertySet DetailProperty, string PhysicsDetailName)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            string sql = "";
            IDbCommand cmd = cnn.CreateCommand();
            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(cmd);
            if (DetailProperty.DataType == "String" || DetailProperty.DataType == "Number")
            {
                sql = "update " + PhysicsDetailName + " set " + DetailProperty.DBFieldName + "='" + DetailProperty.TempValue + "' where id='" + DetailProperty.TempBillId + "'";
            }
            else if (DetailProperty.DataType == "DataTime")
            {
                sql = "update " + PhysicsDetailName + " set " + DetailProperty.DBFieldName + "= to_date('" + Convert.ToDateTime(DetailProperty.TempValue).ToShortDateString() + "','yyyy-mm-dd') where id='" + DetailProperty.TempBillId + "'";
            }
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 更新审批主表数据
        /// </summary>
        /// <param name="MasterProperty"></param>
        private void UpdateMasterData(AppMasterPropertySet MasterProperty)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            string sql = "";
            IDbCommand cmd = cnn.CreateCommand();
            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(cmd);
            sql = "update thd_appmasterdata set propertyvalue='" + MasterProperty.TempValue + "' where billid='" + MasterProperty.TempBillId +
                    "' and AppState=2 and propertyname='" + MasterProperty.MasterPropertyName + "'";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 更新审批明细数据
        /// </summary>
        /// <param name="DetailProperty"></param>
        private void UpdateDetailData(AppDetailPropertySet DetailProperty)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            string sql = "";
            IDbCommand cmd = cnn.CreateCommand();
            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(cmd);
            sql = "update thd_appdetaildata set propertyvalue='" + DetailProperty.TempValue + "' where billdtlid ='" + DetailProperty.TempBillId +
                  "' and AppState=2 and propertyname='" + DetailProperty.DetailPropertyName + "'";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        #endregion

        /// <summary>
        /// 用于审批通过时判断 是否己完成
        /// </summary>
        /// <param name="currentSteps"></param>
        /// <returns></returns>
        private bool AllRolePassed(AppStepsInfo currentSteps)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", currentSteps.BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);

            //当前单据的审批信息
            IList list_AppStepsInfo = this.GetAppStepsInfo(oq);
            if (list_AppStepsInfo == null)
            {
                list_AppStepsInfo = new ArrayList();
            }
            list_AppStepsInfo.Add(currentSteps);

            return this.AllRolePassed(list_AppStepsInfo, currentSteps.AppStepsSet);
        }

        /// <summary>
        /// 当审批步骤的关系为与（1）时 判断审批步骤的角色是否已经全部完成审批 全部完成返回true
        /// </summary>
        /// <param name="list_AppStepsInfo"></param>
        /// <param name="appStepsSet"></param>
        /// <returns></returns>
        private bool AllRolePassed(IList list_AppStepsInfo, AppStepsSet appStepsSet)
        {
            bool allPassed = true;

            foreach (AppRoleSet appRoleSet in appStepsSet.AppRoleSets)
            {
                bool currentPassed = false;
                foreach (AppStepsInfo stepInfo in list_AppStepsInfo)
                {
                    if (stepInfo.AppRole.Id == appRoleSet.AppRole.Id)
                    {
                        currentPassed = true;
                        break;
                    }
                }
                if (currentPassed == false)
                {
                    return false;
                }
            }
            return allPassed;
        }

        /// <summary>
        /// </summary>
        /// <param name="curAppStepsInfo">审批信息</param>
        /// <param name="_AppComments">审批意见</param>
        /// <param name="BillId">审批单据主表ID</param>
        /// <returns></returns>
        [TransManager]
        public void AppAgree(AppTableSet tableSet, AppSolutionSet theAppSolutionSet, AppStepsInfo curAppStepsInfo, string _AppComments, string BillId, IList AppMasterList, IList AppDetailList, IList AppMasteDataModify, IList AppDetailDataModify, string sPersonID)
        {
            #region 查询审批信息 用于判断该单据是否已经做过审批
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", 2L));
            oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddCriterion(Expression.Eq("StepOrder", 1L));
            IList appStepsInfoLst = base.GetDomainByCondition(typeof(AppStepsInfo), oq);
            #endregion

            //读取主表的审批日期（以审批日期为时间戳来区分主表和明细的关系）
            DateTime AppDate = ClientUtil.ToDateTime("1900-01-01");
            if (appStepsInfoLst == null || appStepsInfoLst.Count == 0)
            {
                //保存审批单据的信息（主表和明细）
                SaveOrUpdateByDao(AppMasterList);

                foreach (AppMasterData master in AppMasterList)
                {
                    AppDate = master.AppDate;
                    break;
                }
                //明细审批日期赋值
                foreach (AppDetailData detail in AppDetailList)
                {
                    detail.AppDate = AppDate;
                }
                SaveOrUpdateByDao(AppDetailList);
            }

           // Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
            PersonInfo oPersonInfo = GetCurrentPerson(sPersonID);
            curAppStepsInfo.AuditPerson = oPersonInfo;//login.ThePerson;
            curAppStepsInfo.AppDate = DateTime.Now;
            if (appStepsInfoLst == null || appStepsInfoLst.Count == 0)
            {
                curAppStepsInfo.BillAppDate = AppDate;
            }
            else
            {
                AppStepsInfo theAppStepsInfo = appStepsInfoLst[0] as AppStepsInfo;
                curAppStepsInfo.BillAppDate = theAppStepsInfo.BillAppDate;
            }
            curAppStepsInfo.AppComments = _AppComments;
            curAppStepsInfo.AppStatus = 2;//2: 通过 1：未通过
            curAppStepsInfo.State = 1;
            curAppStepsInfo.BillId = BillId;


            //判断当前审批步骤是否是最后一步
            bool IsFinish = true;
            //获取审批的最后一步
            long MaxStepOrder = 1;
            foreach (AppStepsSet item in theAppSolutionSet.AppStepsSets)
            {
                if (item.StepOrder >= MaxStepOrder)
                {
                    MaxStepOrder = item.StepOrder;
                }
            }
            if (curAppStepsInfo.AppRelations == 0)
            {
                //或关系
                if (curAppStepsInfo.StepOrder >= MaxStepOrder)
                {
                    IsFinish = true;
                }
                else
                {
                    IsFinish = false;
                }
            }
            else if (curAppStepsInfo.AppRelations == 1)
            {
                //与关系 必须是所有角色审批完成后才能代表完成
                if (AllRolePassed(curAppStepsInfo))
                {
                    if (curAppStepsInfo.StepOrder >= MaxStepOrder)
                    {
                        IsFinish = true;
                    }
                    else
                    {
                        IsFinish = false;
                    }
                }
                else
                {
                    IsFinish = false;
                }
            }

            string sql = "";
            //当前步骤审批完成，判断是否是最后一步，如果是最后一步 则更新审批状态表，同时更新原始单据
            string className = tableSet.ClassName;
            if (IsFinish == true && className.Equals("ConcreteBalanceMaster") == false)
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand cmd = cnn.CreateCommand();
                ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
                tran.Enlist(cmd);
                sql = "update " + curAppStepsInfo.AppTableSet.PhysicsName + " Set " + curAppStepsInfo.AppTableSet.StatusName + "=" + curAppStepsInfo.AppTableSet.StatusValueAgr + " WHERE Id='" + BillId + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            //if (AppMasteDataModify.Count > 0)
            //{
            //    //更新原始单据信息(主表和明细)和更新审批数据信息(主表和明细)
            //    foreach (AppMasterPropertySet MasterProperty in AppMasteDataModify)
            //    {
            //        this.UpdateBillMaster(MasterProperty, curAppStepsInfo.AppTableSet.PhysicsName);
            //        if (curAppStepsInfo.StepOrder > 1)
            //        {
            //            this.UpdateMasterData(MasterProperty);
            //        }
            //    }
            //}
            //if (AppDetailDataModify.Count > 0)
            //{
            //    foreach (AppDetailPropertySet DetailProperty in AppDetailDataModify)
            //    {
            //        this.UpdateBillDetail(DetailProperty, curAppStepsInfo.AppTableSet.DetailPhysicsName);
            //        if (curAppStepsInfo.StepOrder > 1)
            //        {
            //            this.UpdateDetailData(DetailProperty);
            //        }
            //    }
            //}

            //加入日志内容
            curAppStepsInfo.TempLogData = "定义表ID[" + tableSet.Id + "]审批方案ID[" + theAppSolutionSet.Id + "]最大步骤["
                + MaxStepOrder + "]关系[" + curAppStepsInfo.AppRelations + "]总步数[" + theAppSolutionSet.AppStepsSets.Count
                + "]是否最后一步[" + IsFinish + "]SQL语句[" + sql + "]";
            Dao.SaveOrUpdate(curAppStepsInfo);
            //写业务相关逻辑
            if (IsFinish == true)
            {
                AppAgreeByBusiness(tableSet, BillId);
            }
        }

        [TransManager]
        public void AppDisAgree(AppTableSet tableSet, AppStepsInfo curAppStepsInfo, string _AppComments, string BillId, IList AppMasterList, IList AppDetailList, IList AppMasteDataModify, IList AppDetailDataModify,string sPersonID)
        {
            #region 查询审批信息 用于判断该单据是否已经做过审批
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("BillId", BillId));
            objectQuery.AddCriterion(Expression.Eq("AppStatus", 2L));
            objectQuery.AddCriterion(Expression.Eq("State", 1));
            //objectQuery.AddCriterion(Expression.Eq("StepOrder", 1L));
            IList appStepsInfoLst = base.GetDomainByCondition(typeof(AppStepsInfo), objectQuery);
            #endregion

            //读取主表的审批日期（以审批日期为时间戳来区分主表和明细的关系）
            DateTime AppDate = ClientUtil.ToDateTime("1900-01-01");
            IList AppMaster = new ArrayList();
            IList AppDetail = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            string sql = "";
            IDbCommand cmd = cnn.CreateCommand();
            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;

            //如果是审批的第一步(直接生成)
            if (appStepsInfoLst == null || appStepsInfoLst.Count == 0)
            {
                //保存审批单据的信息（主表和明细）
                SaveOrUpdateByDao(AppMasterList);
                foreach (AppMasterData master in AppMasterList)
                {
                    AppDate = master.AppDate;
                    break;
                }
                //明细审批日期赋值
                foreach (AppDetailData detail in AppDetailList)
                {
                    detail.AppDate = AppDate;
                }
                SaveOrUpdateByDao(AppDetailList);
            }
            //如果不是审批的第一步(查询第一步生成的审批单据信息)
            else
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("BillId", BillId));
                oq.AddCriterion(Expression.Eq("AppStatus", 2L));

                tran.Enlist(cmd);
                sql = "update thd_appmasterdata t1 Set t1.appstate=1 where t1.appstate = 2 and t1.billid= '" + BillId + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                tran.Enlist(cmd);
                sql = "update thd_appdetaildata t1 Set t1.appstate=1 where t1.appstate = 2 and t1.billid= '" + BillId + "'";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                tran.Enlist(cmd);
                sql = string.Format("UPDATE  THD_APPSTEPSINFO T SET T.STATE=2 WHERE T.BILLID='{0}' AND T.STATE=1", BillId);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                //AppMaster = base.GetDomainByCondition(typeof(AppMasterData), oq);
                //AppDetail = base.GetDomainByCondition(typeof(AppDetailData), oq);

                //foreach (AppMasterData master in AppMaster)
                //{
                //    master.AppStatus = ClientUtil.ToLong(1);
                //    //UpdateByDao(master);
                //}
                //foreach (AppDetailData detail in AppDetail)
                //{
                //    detail.AppStatus = ClientUtil.ToLong(1);
                //    //UpdateByDao(detail);
                //}

                //foreach (AppStepsInfo StepsInfo in appStepsInfoLst)
                //{
                //    StepsInfo.State = 2;
                //    UpdateByDao(StepsInfo);
                //}
            }

            //如不同意 则直接更改单据状态
           // Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
            PersonInfo oPersonInfo = GetCurrentPerson(sPersonID);
            curAppStepsInfo.AuditPerson = oPersonInfo;
            if (appStepsInfoLst == null || appStepsInfoLst.Count == 0)
            {
                curAppStepsInfo.BillAppDate = AppDate;
            }
            else
            {
                AppStepsInfo theAppStepsInfo = appStepsInfoLst[0] as AppStepsInfo;
                curAppStepsInfo.BillAppDate = theAppStepsInfo.BillAppDate;
            }
            curAppStepsInfo.AppDate = DateTime.Now;
            curAppStepsInfo.AppComments = _AppComments;
            curAppStepsInfo.AppStatus = 1;//2: 通过 1：未通过
            curAppStepsInfo.BillId = BillId;
            curAppStepsInfo.State = 2;
            Dao.SaveOrUpdate(curAppStepsInfo);

            //更新业务表单状态
            session = CallContext.GetData("nhsession") as ISession;
            cnn = session.Connection;
            sql = "";
            cmd = cnn.CreateCommand();
            tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(cmd);
            sql = "update " + curAppStepsInfo.AppTableSet.PhysicsName + " Set " + curAppStepsInfo.AppTableSet.StatusName + "=" + curAppStepsInfo.AppTableSet.StatusValueDis + "  WHERE Id='" + BillId + "'";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();


            ////更新原始单据信息(主表和明细)和更新审批数据信息(主表和明细)
            //if (AppMasteDataModify.Count > 0)
            //{
            //    foreach (AppMasterPropertySet MasterProperty in AppMasteDataModify)
            //    {
            //        this.UpdateBillMaster(MasterProperty, curAppStepsInfo.AppTableSet.PhysicsName);
            //        if (curAppStepsInfo.StepOrder > 1)
            //        {
            //            this.UpdateMasterData(MasterProperty);
            //        }
            //    }
            //}
            //if (AppDetailDataModify.Count > 0)
            //{
            //    foreach (AppDetailPropertySet DetailProperty in AppDetailDataModify)
            //    {
            //        this.UpdateBillDetail(DetailProperty, curAppStepsInfo.AppTableSet.DetailPhysicsName);
            //        if (curAppStepsInfo.StepOrder > 1)
            //        {
            //            this.UpdateDetailData(DetailProperty);
            //        }
            //    }
            //}

            AppDisAgreeByBusiness(tableSet, BillId);
            //
        }

        #region 手机端
        [TransManager]
        public void SubmitApprove(string sSolutionID, AppStepsInfo oAppStepsInfo,string sProjectId,string sJobId)
        {
            IList lstTemp = null;
            AppTableSet oAppTableSet = null;
            AppSolutionSet oSolutionSet = null;
            AppStepsSet oCurrAppStepsSet = null;
            ObjectQuery oQuery = new ObjectQuery();
            AppStepsInfo theAppStepsInfo = null;
            OperationRole oOperationRole = null;
            oQuery.AddFetchMode("ParentId",FetchMode.Eager);
            oQuery.AddFetchMode("AppStepsSets", FetchMode.Eager);
            oQuery.AddCriterion(Expression.Eq("Id",sSolutionID));
            lstTemp = dao.ObjectQuery(typeof(AppSolutionSet), oQuery);
            
            if (lstTemp == null || lstTemp.Count==0)
            {
                throw new Exception("未找到对应的审批方案");
            }
             oSolutionSet = lstTemp[0] as AppSolutionSet;
             oAppTableSet = oSolutionSet.ParentId;
            //获取当前审批步骤
             oCurrAppStepsSet = oSolutionSet.AppStepsSets.FirstOrDefault(a => a.Id == oAppStepsInfo.AppStepsSet.Id);
             if (oCurrAppStepsSet==null) { throw new Exception("获取当前审批步骤信息"); }
             oOperationRole = GetOperationRole(sJobId, oCurrAppStepsSet, oAppStepsInfo.BillId);
             if (oOperationRole == null) throw new Exception("当前用户无法权限审批");
             theAppStepsInfo = new AppStepsInfo() ;
             theAppStepsInfo.StepOrder = oCurrAppStepsSet.StepOrder;
             theAppStepsInfo.StepsName = oCurrAppStepsSet.StepsName;
             theAppStepsInfo.AppRelations = oCurrAppStepsSet.AppRelations;
             theAppStepsInfo.AppTableSet = oAppTableSet;
             theAppStepsInfo.AppRole = oOperationRole;
             theAppStepsInfo.RoleName = oOperationRole.RoleName;
             theAppStepsInfo.AppStepsSet = oCurrAppStepsSet;
            theAppStepsInfo.AppComments=oAppStepsInfo.AppComments;
            theAppStepsInfo.State=oAppStepsInfo.State;
            theAppStepsInfo.BillId=oAppStepsInfo.BillId;
            theAppStepsInfo.AuditPerson=oAppStepsInfo.AuditPerson;
            lstTemp = GetAppMasterDetailData(theAppStepsInfo.BillId, oAppTableSet, sProjectId);
            IList lstMaster = lstTemp[0] as IList;
            IList lstDetail = lstTemp[1] as IList;
            if (theAppStepsInfo.State == 1)
            {
                AppAgree(oAppTableSet, oSolutionSet, theAppStepsInfo, theAppStepsInfo.AppComments, theAppStepsInfo.BillId, lstMaster, lstDetail, null, null, theAppStepsInfo.AuditPerson.Id);
            }
            else
            {
                AppDisAgree(oAppTableSet, theAppStepsInfo, theAppStepsInfo.AppComments, theAppStepsInfo.BillId, lstMaster, lstDetail, null, null, theAppStepsInfo.AuditPerson.Id);
            }
             
        }
        public OperationRole GetOperationRole(string sJobId, AppStepsSet oCurrAppStepsSet, string sBillId)
        {
            OperationRole oRole = null;
            string sSQL = @"select t.operationrole from resoperationjobwithrole t
where t.operationjob='{0}' and t.operationrole in (
select tt1.approle from thd_appstepsset tt
join thd_approleset tt1 on tt.id=tt1.parentid and tt1.approle not in (
select ttt.approle from thd_appstepsinfo ttt where ttt.AppStatus=2 and ttt.state=1 and ttt.steporder={3} AND ttt.billid='{2}'   
)
where tt.id='{1}'
)";
                DataTable oTable=GetData(string.Format(sSQL,sJobId,oCurrAppStepsSet.Id,sBillId,oCurrAppStepsSet.StepOrder));
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    oRole = Dao.Get(typeof(OperationRole), ClientUtil.ToString(oTable.Rows[0]["operationrole"])) as OperationRole;
                }
                return oRole;
               
        }
        public PersonInfo GetCurrentPerson(string sPersonId)
        {
            PersonInfo oPersonInfo = null;
            if (string.IsNullOrEmpty(sPersonId))
            {
                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
                oPersonInfo = login.ThePerson;
            }
            else
            {
                oPersonInfo = dao.Get(typeof(PersonInfo), sPersonId) as PersonInfo;
            }
            return oPersonInfo;
        }
        /// <summary>
        /// 根据表单定义 和单据ID 获取对应的审批主表和明细信息
        /// </summary>
        /// <param name="sBillID"></param>
        /// <param name="oAppTableSet"></param>
        /// <param name="sProjectId"></param>
        /// <returns></returns>
        [TransManager]
        public IList GetAppMasterDetailData(string sBillID, AppTableSet oAppTableSet, string sProjectId)
        {
            IList lstResult = new ArrayList();
            string sMasterSQL = "select t.masterpropertyname,t.masterppropertychinesename,serialnumber from thd_appmasterpropertyset t where t.parentid='{0}'  order by t.serialnumber asc";
            string sDetialSQL = "select t.detailpropertyname,t.detailpropertychinesename,serialnumber from thd_appdetailpropertyset t where t.parentid='{0}' order by t.serialnumber asc ";
            List<AppMasterData> lstMaster = new List<AppMasterData>();
            AppMasterData theAppMasterData = null;
            List<AppDetailData> lstDetail = new List<AppDetailData>();
            AppDetailData theAppDetailData = null;
            string sPropertyName = string.Empty;
            PropertyInfo oPropertyInfo = null;
            DataTable oTable = null;
            Type type = null;
            object oBill = null;
            IEnumerable Detials = null;
            lstResult.Insert(0, null);
            lstResult.Insert(0, null);
            DateTime date = System.DateTime.Now;
            string sDetailID = string.Empty;
            if (!IsHasStepInfo(sBillID))
            {
                oBill = GetBillById(oAppTableSet, sBillID);
                #region 主表
                oTable = GetData(string.Format(sMasterSQL, oAppTableSet.Id));
                if (oTable != null)
                {
                    type = oBill.GetType();
                    foreach (DataRow oRow in oTable.Rows)
                    {
                        sPropertyName = ClientUtil.ToString(oRow["masterpropertyname"]);
                        oPropertyInfo = type.GetProperty(sPropertyName);
                        if (oPropertyInfo != null)
                        {
                            theAppMasterData = new AppMasterData();
                            theAppMasterData.PropertyName = sPropertyName;
                            theAppMasterData.PropertyValue = ClientUtil.ToString(oPropertyInfo.GetValue(oBill, null));
                            theAppMasterData.BillId = sBillID;
                            theAppMasterData.PropertyChineseName = ClientUtil.ToString(oRow["masterppropertychinesename"]);
                            theAppMasterData.PropertySerialNumber = ClientUtil.ToInt(oRow["serialnumber"]);
                            theAppMasterData.AppDate = date;
                            theAppMasterData.ProjectId = sProjectId;
                            theAppMasterData.AppStatus = 2L;
                            theAppMasterData.AppTableSet = oAppTableSet.Id;
                            lstMaster.Add(theAppMasterData);
                        }
                    }
                }
                #endregion
                #region 明细
                oTable = GetData(string.Format(sDetialSQL, oAppTableSet.Id));
                if (oTable != null && oTable.Rows.Count > 0)
                {
                    Detials = type.GetProperty("Details").GetValue(oBill, null) as IEnumerable;
                    foreach (object oDetial in Detials)
                    {
                        type = oDetial.GetType();
                        oPropertyInfo= type.GetProperty("Id");
                       
                        sDetailID= oPropertyInfo==null?"":ClientUtil.ToString(oPropertyInfo.GetValue(oDetial, null));
                        foreach (DataRow oRow in oTable.Rows)
                        {
                            sPropertyName = ClientUtil.ToString(oRow["detailpropertyname"]);
                            oPropertyInfo = type.GetProperty(sPropertyName);
                            if (oPropertyInfo != null)
                            {
                                theAppDetailData = new AppDetailData();
                                theAppDetailData.PropertyName = sPropertyName;
                                theAppDetailData.PropertyValue = ClientUtil.ToString(oPropertyInfo.GetValue(oDetial, null));
                                theAppDetailData.BillId = sBillID;
                                theAppDetailData.PropertyChineseName = ClientUtil.ToString(oRow["detailpropertychinesename"]);
                                theAppDetailData.PropertySerialNumber = ClientUtil.ToInt(oRow["serialnumber"]);
                                theAppDetailData.AppDate = date;
                                theAppDetailData.AppTableSet = oAppTableSet.Id;
                                theAppDetailData.BillDtlId=sDetailID;
                                //theAppDetailData.ProjectId = sProjectId;
                                theAppDetailData.AppStatus = 2L;
                                lstDetail.Add(theAppDetailData);
                            }
                        }
                    }
                }
                #endregion
            }
            lstResult[0] = lstMaster;
            lstResult[1] = lstDetail;
            return lstResult;
        }
        [TransManager]
        public bool IsHasStepInfo(string sBuildID)
        {
            IList lst = GetStepInfos(sBuildID);
            return lst != null && lst.Count > 0;
        }
        [TransManager]
        public IList GetStepInfos(string sBuildID)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", sBuildID));
            oq.AddCriterion(Expression.Eq("AppStatus", 2L));
            oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddCriterion(Expression.Eq("StepOrder", 1L));
            return  base.GetDomainByCondition(typeof(AppStepsInfo), oq);
        }
        public DataTable GetData(string sSQL)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand oCommand = conn.CreateCommand();
            ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            tran.Enlist(oCommand);
            oCommand.CommandType = CommandType.Text;
            oCommand.CommandText = sSQL;
            IDataReader dataReader = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds.Tables[0];
            
        }
        [TransManager]
        public object GetBillById(AppTableSet oAppTableSet, string sBillID)
        {
            object oBill = null;
            Type type = Type.GetType(oAppTableSet.MasterNameSpace + ",SupplyChain");
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", sBillID));
            oQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList lst = Dao.ObjectQuery(type, oQuery);
            if (lst != null && lst.Count > 0)
            {
                oBill = lst[0];
            }
            else
            {
                throw new Exception("未找到对应审批单据");
            }
            return oBill;
        }
        #endregion

        #region 短信
        /// <summary>
        /// 根据单据的类名获取表单集合
        /// </summary>
        /// <param name="sMasterClassName">主类名</param>
        /// <returns></returns>
        public IList GetAppTable(string sMasterClassName)
        {
            IList list_AppTable = new ArrayList();// 表单信息 步骤信息 角色信息
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("MasterNameSpace", sMasterClassName));
            oq.AddCriterion(Expression.Eq("Status", 0));
            list_AppTable = base.GetDomainByCondition(typeof(AppTableSet), oq);
            return list_AppTable;
            //oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            ////oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("AppStepsSet", NHibernate.FetchMode.Eager);
            ////oq.AddFetchMode("AppStepsSet.AppRoleSets", NHibernate.FetchMode.Eager);
            //list_AppStepsInfo = GetAppStepsInfo(oq);
            //return list_AppStepsInfo;
        }
        /// <summary>
        /// 获取审批方案
        /// </summary>
        /// <param name="list_AppTable"></param>
        /// <returns></returns>
        public AppSolutionSet GetAppSolution(IList list_AppTable, string sBillID)
        {


            if (list_AppTable != null && list_AppTable.Count > 0)
            {
                foreach (AppTableSet oTable in list_AppTable)
                {
                    IList list_Solution = new ArrayList();
                    ObjectQuery oq = new ObjectQuery();

                    oq.AddCriterion(Expression.Eq("ParentId.Id", oTable.Id));
                    list_Solution = base.GetDomainByCondition(typeof(AppSolutionSet), oq);
                    foreach (AppSolutionSet oSolu in list_Solution)
                    {
                        if (IsExistBill(oTable, oSolu, sBillID))
                        {
                            return oSolu;
                        }
                    }
                }

            }
            return null;
        }
        /// <summary>
        /// 判断该单据是否存在
        /// </summary>
        /// <param name="oTable"></param>
        /// <param name="oSolution"></param>
        /// <param name="sBillID"></param>
        /// <returns></returns>
        public bool IsExistBill(AppTableSet oTable, AppSolutionSet oSolution, string sBillID)
        {
            bool bFlag = false;
            if (oSolution != null && oTable != null)
            {

                IList ret = new ArrayList();
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand cmd = cnn.CreateCommand();
                string sSQL = "select count(*) from {0} t where t.id='{1}' {2}";
                string sCondition = oSolution.Conditions == null ? string.Empty : oSolution.Conditions.Trim();
                if (!string.IsNullOrEmpty(sCondition))
                {
                    sCondition = "  and " + sCondition;
                }
                sSQL = string.Format(sSQL, oTable.PhysicsName, sBillID, sCondition);
                cmd.CommandText = sSQL;
                IDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    bFlag = true;
                }
                else
                {
                    bFlag = false;
                }



            }
            return bFlag;
        }

        /// <summary>
        /// 获取当前单据审核到了那一步 如果没有审批 返回1 如果审批了返回当前步+1;
        /// </summary>
        /// <param name="sBillId"></param>
        /// <returns></returns>
        public Int64 GetCurrStep(string sBillId)
        {
            Int64 iStep = 0;//起步
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", sBillId));
            oq.AddCriterion(Expression.Eq("AppStatus", 2L));
            oq.AddCriterion(Expression.Eq("State", 1));

            //oq.AddCriterion(Expression.Eq("StepOrder", 1L));
            IList appStepsInfoLst = base.GetDomainByCondition(typeof(AppStepsInfo), oq);
            foreach (AppStepsInfo oAppStepsInfo in appStepsInfoLst)
            {
                if (iStep < oAppStepsInfo.StepOrder)
                {
                    iStep = oAppStepsInfo.StepOrder;
                }
            }
            iStep++;
            return iStep;
        }
        /// <summary>
        /// 根据方案的ID获取对应的角色
        /// </summary>
        /// <param name="sSolutionID"></param>
        /// <returns></returns>
        /// 
        public IList GetAppRoleBySolutionID(string sSolutionID, Int64 iStep)
        {


            IList listAppRole = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentId.Id", sSolutionID));
            oq.AddCriterion(Expression.Eq("StepOrder", iStep));//审批下一步
            oq.AddFetchMode("AppRoleSets", NHibernate.FetchMode.Eager);//AppRole
            oq.AddFetchMode("AppRoleSets.AppRole", NHibernate.FetchMode.Eager);
            IList listStep = base.GetDomainByCondition(typeof(AppStepsSet), oq);
            if (listStep != null && listStep.Count > 0)
            {
                foreach (AppStepsSet oStep in listStep)
                {
                    foreach (AppRoleSet oRole in oStep.AppRoleSets)
                    {
                        listAppRole.Add(oRole);
                    }
                }
            }
            return listAppRole;
        }
        /// <summary>
        /// 获取当前单据 审批第一步角色集合
        /// </summary>
        /// <param name="sClassName">类名</param>
        /// <param name="sBillID">单据ID</param>
        /// <returns></returns>
        public IList GetCurrBillRole(string sClassName, string sBillID)
        {
            IList list_AppTable = null;
            IList list_AppRole = null;
            AppSolutionSet oSolution = null;
            Int64 iStep = 0;
            //获取审批表单
            list_AppTable = GetAppTable(sClassName);
            //获取审批方案
            oSolution = GetAppSolution(list_AppTable, sBillID);//获取当前单据所对应的方案

            if (oSolution != null)
            {
                iStep = GetCurrStep(sBillID);
                list_AppRole = GetAppRoleBySolutionID(oSolution.Id, iStep);
            }
            return list_AppRole;
        }
        /// <summary>
        /// 根据角色找岗位
        /// </summary>
        /// <param name="listRole"></param>
        /// <returns></returns>
        public IList GetJobByRoles(IList listRole)
        {
            IList list_OprJob = new ArrayList();
            IList list_OprRoleWintJob = null;
            List<string> lstRoleID = new List<string>();
            ObjectQuery oq = new ObjectQuery();
            foreach (AppRoleSet oRole in listRole)
            {
                if (oRole.AppRole != null)
                {
                    lstRoleID.Add(oRole.AppRole.Id);
                }
            }
            oq.AddCriterion(Expression.In("OperationRole.Id", lstRoleID));
            oq.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            list_OprRoleWintJob = base.GetDomainByCondition(typeof(OperationJobWithRole), oq);

            foreach (OperationJobWithRole o in list_OprRoleWintJob)
            {
                list_OprJob.Add(o.OperationJob);
            }
            return list_OprJob;
        }

        /// <summary>
        /// 根据部门查找
        /// </summary>
        /// <param name="listJob"></param>
        /// <param name="sSysCode"></param>
        /// <returns></returns>
        public IList GetOprJobBySysCode(IList listJob, string sSysCode)
        {
            IList list_OprJob = new ArrayList();
            if (listJob != null && listJob.Count > 0)
            {
                List<string> lstJobID = new List<string>();
                foreach (OperationJob oJob in listJob)
                {
                    lstJobID.Add(oJob.Id);
                }
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.In("Id", lstJobID));
                oq.AddCriterion(Expression.Eq("OperationOrg.SysCode", sSysCode));
                list_OprJob = base.GetDomainByCondition(typeof(OperationJob), oq);
            }
            return list_OprJob;
        }
        /// <summary>
        /// 根据岗位 和层次码获取岗位
        /// </summary>
        /// <param name="list_Job"></param>
        /// <param name="sOrgSysCode"></param>
        /// <returns></returns>
        public IList GetOperationJobByJobIDAndOrgID(IList list_Job, string sOrgSysCode)
        {
            List<string> lstJobs = new List<string>();
            IList retLst = new ArrayList();

            foreach (OperationJob oJob in list_Job)//获取岗位ID集合
            {
                lstJobs.Add(oJob.Id);
            }
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("Id", sRoleID));
            oq.AddCriterion(Expression.Eq("OperationOrg.SysCode", sOrgSysCode));
            oq.AddCriterion(Expression.In("Id", lstJobs));
            //oq.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            IList tempLst = GetDomainByCondition(typeof(OperationJob), oq);

            foreach (OperationJob obj in tempLst)
            {
                retLst.Add(obj);
            }
            return retLst;
        }
        private List<OperationOrg> GetChildOpg(string parentId, IList lstOpg)
        {
            //可以在本方法中把检索过的数据清除 以避免后续再循环 未处理
            List<OperationOrg> retLst = new List<OperationOrg>();
            if (lstOpg == null) return retLst;
            foreach (OperationOrg org in lstOpg)
            {
                if (org.ParentNode == null) continue;
                if (org.ParentNode.Id == parentId)
                {
                    retLst.Add(org);
                }
            }
            return retLst;
        }
        public IList CheckOpg(string billOpgSysCode, IList appJobList)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            string[] opgIds = billOpgSysCode.Split('.');
            if (opgIds == null || opgIds.Length == 0) return null;

            string tempId = "";
            List<string> parentSysCodes = new List<string>();
            foreach (string id in opgIds)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    tempId = tempId + id + ".";
                    parentSysCodes.Add(tempId);
                    dis.Add(Expression.Eq("SysCode", tempId));
                }
            }
            if (opgIds.Length > 1)
            {
                dis.Add(Expression.Eq("ParentNode.Id", opgIds[opgIds.Length - 2]));
            }

            oq.AddCriterion(dis);
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            //查询单据对应业务部门的所有上层同级业务部门
            IList lstOpg = GetDomainByCondition(typeof(OperationOrg), oq);
            //最近上级部门的情况
            for (int j = parentSysCodes.Count - 1; j >= 0; j--)
            {
                string parentSysCode = parentSysCodes[j];
                if (parentSysCode == billOpgSysCode) continue;
                //if (ConstObject.TheOperationOrg.SysCode == parentSysCode && CheckRole(appRoleLst, userRolelst))

                IList list_tempJob = GetOperationJobByJobIDAndOrgID(appJobList, parentSysCodes[j]);//通过当前的组织结构code筛选岗位集合
                if (list_tempJob != null && list_tempJob.Count > 0)
                {
                    return list_tempJob;
                }

            }
            /*****
             * 最近的兄弟部门
             * **/
            if (opgIds.Length > 1)
            {
                for (int k = opgIds.Length - 2; k >= 0; k--)
                {
                    string parentId = opgIds[k];
                    List<OperationOrg> childs = GetChildOpg(parentId, lstOpg);
                    foreach (OperationOrg org in childs)
                    {

                        IList list_tempJob = GetOperationJobByJobIDAndOrgID(appJobList, org.SysCode);//通过当前的组织结构code筛选岗位集合
                        if (list_tempJob != null && list_tempJob.Count > 0)
                        {
                            return list_tempJob;
                        }

                    }
                }
            }
            return null;
        }
        public IList GetPersonByJob(IList listJob)
        {
            List<string> lstJobs = new List<string>();
            IList retLst = new ArrayList();
            if (listJob != null && listJob.Count > 0)
            {
                foreach (OperationJob oJob in listJob)//获取岗位ID集合
                {
                    lstJobs.Add(oJob.Id);
                }
                ObjectQuery oq = new ObjectQuery();


                oq.AddCriterion(Expression.In("OperationJob.Id", lstJobs));
                oq.AddFetchMode("StandardPerson", NHibernate.FetchMode.Eager);
                IList tempLst = GetDomainByCondition(typeof(PersonOnJob), oq);

                foreach (PersonOnJob obj in tempLst)
                {
                    retLst.Add(obj.StandardPerson);
                }
            }
            return retLst;
        }
        #endregion

        #region 审批回调业务方法
        /// <summary>业务单据审批提交接口
        /// </summary>ConstObject.TheSysRole.Id=sUserJobID
        /// <param name="obj">业务单据信息</param>
        /// <returns>错误和成功信息</returns>
        public string AppCommitBusiness(Object obj)
        {
            string str = "";
            string BillId = ClientUtil.ToString(obj.GetType().GetProperty("Id").GetValue(obj, null));
            string opgSysCode = obj.GetType().GetProperty("OpgSysCode").GetValue(obj, null) + "";//部门层次码
            string sMasterClassName = obj.ToString();
            IList list_AppRole = null;
            IList list_Job = null;
            IList list_ResultJob = null;
            //1: 获取审批平台的审批方案信息 

            //  //2: 根据业务单据信息获取到该业务单据的审批方案信息 thd_apptableset.id  = thd_appsolutionset.parentid一个类名可能对多个

            //  //3: 根据审批方案信息中第一个审批步骤获取角色信息thd_appsolutionset.id=thd_appstepset.parentid   thd_appstepset.steporder=1 thd_appstepset.id=thd_approleset.parentid
            list_AppRole = GetCurrBillRole(sMasterClassName, BillId);


            //  //4: 根据审批数据权限，通过角色获取到该角色对应的岗位信息
            #region 获取岗位
            list_Job = GetJobByRoles(list_AppRole);//根据角色获取岗位
            #endregion
            //4.1: 如果该角色只对应一个岗位,则为此岗位
            if (list_Job.Count == 1)
            {
                list_ResultJob = list_Job;
            }
            else//如该角色查处有多个 那需要通过组织筛选
            {
                //4.2: 判断本部门 角色、组织是否相同
                #region 是否相同
                //IList userRolelst = GetOperationRoleByJobId(sJobID);
                //IList list_TempRole = null;
                //if (sOrgID == opgSysCode)
                //{
                //      list_TempRole = CheckRole(list_AppRole, userRolelst);
                //}

                IList list_TempJob = null;
                list_TempJob = GetOprJobBySysCode(list_Job, opgSysCode);//根据岗位和当前业务单据所属的组织层次码
                // list_TempJob.Clear();

                #endregion
                if (list_TempJob != null && list_TempJob.Count > 0)
                {
                    list_ResultJob = list_TempJob;
                }
                else
                {
                    //4.3: 最近上级部门和兄弟部门
                    list_ResultJob = CheckOpg(opgSysCode, list_Job);

                }
            }
            //  //5: 根据该岗位获取到人员信息
            IList list_Person = GetPersonByJob(list_ResultJob);

            //  //6: 给相关人员发送短消息
            foreach (StandardPerson oPerson in list_Person)
            {
                str += oPerson.Name + oPerson.Photo + " | ";
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 3);
            }
            return str;
        }

        //通用调用方法[审批通过]
        private void AppAgreeByBusiness(AppTableSet tableSet, string billId)
        {
            string className = tableSet.ClassName;
            switch (className)
            {
                case "ResourceRequireReceipt":
                    GenerateSupplyResourcePlan(billId);
                    break;
                case "SubContractBalanceBill":
                    UpdateSubContractProjectByAgree(billId);
                    break;
                case "ProjectTaskAccountBill":
                    WriteBackAccTaskDtlQnyAndProgress(billId);
                    break;
                case "DailyPlanMaster":
                    DailyBackResourcePlan(billId);
                    break;
                case "MonthlyPlanMaster":
                    MonthlyBackResourcePlan(billId);
                    break;
                case "SupplyPlanMaster":
                    SupplyBackResourcePlan(billId);
                    break;
                case "ConcreteBalanceMaster":
                    UpdateConcreteManByAgree(billId);
                    break;
                case "PaymentMaster":
                    UpdateCumulativeExcutePay(billId);
                    break;
            }
        }

        //通用调用方法[审批不通过]
        private void AppDisAgreeByBusiness(AppTableSet tableSet, string billId)
        {
            string className = tableSet.ClassName;
            switch (className)
            {
                case "SubContractBalanceBill":
                    UpdateSubContractProjectByDisAgree(billId);
                    break;
            }
        }

        //分包结算单逻辑[审批通过]
        private void UpdateSubContractProjectByAgree(string billId)
        {
            SubBalSrv.UpdateSubContractProjectByAgree(billId);
        }

        //分包结算单逻辑[审批不通过]
        private void UpdateSubContractProjectByDisAgree(string billId)
        {
            SubBalSrv.UpdateSubContractProjectByDisAgree(billId);
        }

        //业务相关逻辑方法
        private void GenerateSupplyResourcePlan(string billId)
        {
            PlanSrv.GenerateSupplyResourcePlan(billId);
        }

        //商品砼结算单[审批通过]
        private void UpdateConcreteManByAgree(string billId)
        {
            ConcreteSrv.TallyConcreteBalanceByApproval(billId);
        }

        /// <summary>
        /// 根据审批通过的核算单回写前驱核算任务明细的核算工程量和形象进度
        /// </summary>
        /// <param name="accBillId"></param>
        private void WriteBackAccTaskDtlQnyAndProgress(string accBillId)
        {
            ProjectTaskAccountBill bill = dao.Get(typeof(ProjectTaskAccountBill), accBillId) as ProjectTaskAccountBill;
            if (bill != null && bill.DocState == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InExecute)
            {
                AccSrv.WriteBackAccTaskDtlQnyAndProgress(accBillId);//回写确认单的状态
            }
        }

        //资金支付审批单通过回写
        [TransManager]
        private void UpdateCumulativeExcutePay(string billId)
        {
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("Id", billId));
            objQuery.AddCriterion(Expression.IsNotNull("FundPlan"));
            objQuery.AddFetchMode("FundPlan", FetchMode.Eager);

            var list = Dao.ObjectQuery(typeof (PaymentMaster), objQuery);
            if (list == null || list.Count == 0)
            {
                return;
            }

            var pay = list[0] as PaymentMaster;
            var planDetail = pay.FundPlan;
            planDetail.CumulativeExcutePay += pay.SumMoney;

            Dao.SaveOrUpdate(planDetail);
        }

        #endregion

        #region 审批平台加速版

        public DataTable GetAppBill(string sOpjID, string sSysCode, string sProjectID, DateTime dateBegin, DateTime dateEnd)
        {
            DataTable oBillTable = CreateEmptyBillDataTable();//单据表
            DataTable oSolutionTable = null;//审批方案表
            DataTable oCurrUserRole = null;//当前用户角色
            // DataTable oAllRoleTable = null;//所有角色
            DataTable OnlyOneRole = null;
            DataTable oAllBillTable = null;
            string sTableName = string.Empty;
            string sSolutionID = string.Empty;
            string sSolutionName = string.Empty;
            string sCondition = string.Empty;
            string sTableID = string.Empty;
            string sSQLs = string.Empty;
            string sSQL = string.Empty;
            try
            {
                oSolutionTable = QueryCanAduditSolutionByOpJobID(sOpjID);//获取当前用户所能审批的解决方案    tableID, physicsname , solutionID, t2.solutionname  ,t2.conditions
                if (oSolutionTable != null && oSolutionTable.Rows.Count > 0)
                {
                    oCurrUserRole = QueryRoleByOpJobID(sOpjID);//获取当前用户的角色
                    if (oCurrUserRole != null && oCurrUserRole.Rows.Count > 0)
                    {
                        //oAllRoleTable = QueryAllRole();//获取所有权限
                        OnlyOneRole = QueryOnlyOneRole();//获取所有权限

                        //oOrgAllTable = QueryAllOrg();//获取所有组织
                        //if (oOrgAllTable != null && oOrgAllTable.Rows.Count > 0)
                        //{
                        foreach (DataRow oRowSolution in oSolutionTable.Rows)
                        {
                            if (oRowSolution != null)
                            {
                                sTableID = oRowSolution["tableID"].ToString();
                                sTableName = oRowSolution["physicsname"].ToString();
                                sSolutionID = oRowSolution["solutionID"].ToString();
                                sSolutionName = oRowSolution["SolutionName"].ToString();
                                sCondition = oRowSolution["conditions"].ToString();
                                //GetBill(oBillTable, oCurrUserRole, oAllRoleTable, sSysCode, sOpjID, sSolutionName, sSolutionID, sTableName, sCondition, sProjectID, dateBegin, dateEnd);
                                sSQL = QueryBillSQL(sTableID, sSolutionID, sSolutionName, sTableName, sCondition, sProjectID, sOpjID, dateBegin, dateEnd);
                                if (!string.IsNullOrEmpty(sSQL))
                                {
                                    sSQLs += (string.IsNullOrEmpty(sSQLs) ? sSQL : " union " + sSQL);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(sSQLs))
                        {
                            oAllBillTable = QueryDataTable(sSQLs);
                            GetBill(oBillTable, oCurrUserRole, OnlyOneRole, oAllBillTable, sSysCode, sOpjID);
                        }
                    }
                }
            }
            catch
            {
            }
            return oBillTable;
        }

        public DataTable QueryDataTable(string sSQL)
        {
            DataTable oTable = null;
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandText = sSQL;
                IDataReader dataReader = cmd.ExecuteReader();
                DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                oTable = ds.Tables[0];
            }
            catch
            {
            }
            return oTable;
        }
        public void GetTestSQL(string sOpjID, string sSysCode, string sProjectID, DateTime dateBegin, DateTime dateEnd)
        {
            string sPLSQL = @"declare
       vOpJobID varchar2(100):='{0}';
       sOrgSysCode varchar2(300):='{1}';
       sDateEnd varchar2(20):='{2}';
       sDateBegin varchar2(20):='{3}';
       sProjectID varchar2(100):='{4}';
       dtBill   appfun.DataSet;
begin
        
       appfun.GetBill(vOpJobID => vOpJobID,sOrgSysCode => sOrgSysCode,
                sDateEnd => sDateEnd,sDateBegin => sDateBegin,
                sProjectID => sProjectID,dtBill => dtBill);
end;";
            sPLSQL = string.Format(sPLSQL, sOpjID, sSysCode, dateEnd.ToShortDateString(), dateBegin.ToShortDateString(), sProjectID);
        }
        public DataTable GetAppBillByProc(string sOpjID, string sSysCode, string sProjectID, DateTime dateBegin, DateTime dateEnd, ref  string sErrMsg)
        {
            DataTable oTable = null;
            try
            {

                //GetTestSQL(sOpjID,   sSysCode,   sProjectID,   dateBegin,   dateEnd);
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AppFun.GetBill";
                // GetBill(vOpJobID varchar2,sOrgSysCode varchar2,sDateEnd varchar2,sDateBegin varchar2,sProjectID varchar2 ,dtBill out DataSet)
                //vOpJobID varchar2
                OracleParameter paramOpJobID = new OracleParameter("vOpJobID", sOpjID);
                paramOpJobID.OracleType = OracleType.VarChar;
                cmd.Parameters.Add(paramOpJobID);
                //sOrgSysCode varchar2
                OracleParameter paramOrgSysCode = new OracleParameter("sOrgSysCode", sSysCode);
                paramOrgSysCode.OracleType = OracleType.VarChar;
                cmd.Parameters.Add(paramOrgSysCode);
                //sDateEnd varchar2
                OracleParameter paramDateEnde = new OracleParameter("sDateEnd", dateEnd.ToString("yyyy-MM-dd"));
                paramDateEnde.OracleType = OracleType.VarChar;
                cmd.Parameters.Add(paramDateEnde);
                //sDateBegin varchar2
                OracleParameter paramDateBegin = new OracleParameter("sDateBegin", dateBegin.ToString("yyyy-MM-dd"));
                paramDateBegin.OracleType = OracleType.VarChar;
                cmd.Parameters.Add(paramDateBegin);
                //sProjectID varchar2
                OracleParameter paramProjectID = new OracleParameter("sProjectID", sProjectID);
                paramProjectID.OracleType = OracleType.VarChar;
                cmd.Parameters.Add(paramProjectID);

                OracleParameter paramDT = new OracleParameter("dtBill", OracleType.Cursor);
                paramDT.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramDT);
                IDataReader dataReader = cmd.ExecuteReader();
                DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                oTable = ds.Tables[0];
            }
            catch (Exception e)
            {
                sErrMsg = e.Message;
            }
            return oTable;
        }
        public IList GetAppBillPerNameByProc(string sBillID)
        {
            IList lst = null;
            try
            {

                //procedure GetAppNextPName(sBillID varchar2, sStepName in out varchar2, sStepPersons in out varchar2, sErrMsg in out varchar2);
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "appFunGetPersonName.GetAppNextPName";
                string sStepName = string.Empty;
                string sStepPersons = string.Empty;
                string sErrMsg = string.Empty;
                OracleParameter paramBillID = new OracleParameter("sBillID", sBillID);
                paramBillID.OracleType = OracleType.VarChar;
                cmd.Parameters.Add(paramBillID);

                OracleParameter paramStepName = new OracleParameter("sStepName", sStepName);
                paramStepName.OracleType = OracleType.VarChar;
                paramStepName.Size = 100;
                paramStepName.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(paramStepName);

                OracleParameter paramStepPersons = new OracleParameter("sStepPersons", sStepPersons);
                paramStepPersons.OracleType = OracleType.VarChar;
                paramStepPersons.Size = 4000;
                paramStepPersons.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(paramStepPersons);

                OracleParameter paramsErrMsg = new OracleParameter("sErrMsg", sErrMsg);
                paramsErrMsg.OracleType = OracleType.VarChar;
                paramsErrMsg.Direction = ParameterDirection.InputOutput;
                paramsErrMsg.Size = 1000;
                cmd.Parameters.Add(paramsErrMsg);
                cmd.ExecuteNonQuery();
                if (paramsErrMsg.Value == null || string.IsNullOrEmpty(paramsErrMsg.Value.ToString()))
                {
                    lst = new ArrayList();
                    if (paramStepName.Value == null || string.IsNullOrEmpty(paramStepName.Value.ToString()))
                    {
                        lst = null;
                    }
                    else
                    {
                        lst.Insert(lst.Count, paramStepName.Value.ToString());
                        lst.Insert(lst.Count, paramStepPersons.Value.ToString());
                    }
                }
                else
                {
                    throw new Exception(paramsErrMsg.Value.ToString());
                }


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return lst;
        }
        public DataSet GetSubmitBillPerson(string sBillID, string sClassName)
        {
            DataSet ds = null;
            try
            {

                //procedure GetPerson(sBillID varchar2 ,sClassName varchar2,sErrMsg out varchar2,Persons out appfun_GetPersonByName.DataSet)
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "appfun_GetPersonByName.GetPerson";
                string sStepName = string.Empty;
                string sStepPersons = string.Empty;
                string sErrMsg = string.Empty;
                OracleParameter paramBillID = new OracleParameter("sBillID", sBillID);
                paramBillID.OracleType = OracleType.VarChar;
                paramBillID.Size = 200;
                cmd.Parameters.Add(paramBillID);

                OracleParameter paramStepName = new OracleParameter("sClassName", sClassName);
                paramStepName.OracleType = OracleType.VarChar;
                paramStepName.Size = 200;
                paramStepName.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(paramStepName);

                OracleParameter paramsErrMsg = new OracleParameter("sErrMsg", sErrMsg);
                paramsErrMsg.OracleType = OracleType.VarChar;
                paramsErrMsg.Direction = ParameterDirection.InputOutput;
                paramsErrMsg.Size = 1000;
                cmd.Parameters.Add(paramsErrMsg);


                OracleParameter paramDT = new OracleParameter("Persons", OracleType.Cursor);
                paramDT.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramDT);
                IDataReader dataReader = cmd.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

                if (paramsErrMsg.Value == null || string.IsNullOrEmpty(paramsErrMsg.Value.ToString()))
                {

                }
                else
                {
                    throw new Exception(paramsErrMsg.Value.ToString());
                }


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }
        public string GetSubmitBillPersonByName(string sBillID, string sClassName)
        {
            string sPersonNames = string.Empty;
            try
            {

                //appfungetpersonname.GetPerson(sBillID => sBillID,sClassName => sClassName,sErrMsg =>sErrMsg ,PersonName =>PersonName )
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "appfungetpersonname.GetPerson";
                string sStepName = string.Empty;
                string sStepPersons = string.Empty;
                string sErrMsg = string.Empty;
                OracleParameter paramBillID = new OracleParameter("sBillID", sBillID);
                paramBillID.OracleType = OracleType.VarChar;
                paramBillID.Size = 200;
                cmd.Parameters.Add(paramBillID);

                OracleParameter paramStepName = new OracleParameter("sClassName", sClassName);
                paramStepName.OracleType = OracleType.VarChar;
                paramStepName.Size = 200;
                paramStepName.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(paramStepName);

                OracleParameter paramsErrMsg = new OracleParameter("sErrMsg", sErrMsg);
                paramsErrMsg.OracleType = OracleType.VarChar;
                paramsErrMsg.Direction = ParameterDirection.InputOutput;
                paramsErrMsg.Size = 1000;
                cmd.Parameters.Add(paramsErrMsg);


                OracleParameter paramDT = new OracleParameter("PersonName", sPersonNames);
                paramDT.OracleType = OracleType.VarChar;
                paramDT.Size = 4000;
                paramDT.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramDT);
                IDataReader dataReader = cmd.ExecuteReader();
                sPersonNames = paramDT.Value.ToString();

                if (paramsErrMsg.Value == null || string.IsNullOrEmpty(paramsErrMsg.Value.ToString()))
                {

                }
                else
                {
                    throw new Exception(paramsErrMsg.Value.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return sPersonNames;
        }
        public DataSet GetPassBillID(string sTableSetID, string sBeginTime, string sEndTime, string sProjectID)
        {
            DataSet ds = null;
            try
            {

                //getPassedApp(sTableSetID varchar2,sBeginTime varchar2,sEndTime varchar2,sProjectID varchar2,sErrMsg out varchar2,ds out appfun.DataSet)
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "getpassedapp";

                string sErrMsg = string.Empty;
                OracleParameter paramTableSetID = new OracleParameter("sTableSetID", sTableSetID);
                paramTableSetID.OracleType = OracleType.VarChar;
                paramTableSetID.Size = 200;
                cmd.Parameters.Add(paramTableSetID);

                OracleParameter paramBeginTime = new OracleParameter("sBeginTime", sBeginTime);
                paramBeginTime.OracleType = OracleType.VarChar;
                paramBeginTime.Size = 200;
                cmd.Parameters.Add(paramBeginTime);

                OracleParameter paramEndTime = new OracleParameter("sEndTime", sEndTime);
                paramEndTime.OracleType = OracleType.VarChar;
                paramEndTime.Size = 200;
                cmd.Parameters.Add(paramEndTime);

                OracleParameter paramProjectID = new OracleParameter("sProjectID", sProjectID);
                paramProjectID.OracleType = OracleType.VarChar;
                paramProjectID.Size = 200;
                cmd.Parameters.Add(paramProjectID);

                OracleParameter paramsErrMsg = new OracleParameter("sErrMsg", sErrMsg);
                paramsErrMsg.OracleType = OracleType.VarChar;
                paramsErrMsg.Direction = ParameterDirection.InputOutput;
                paramsErrMsg.Size = 1000;
                cmd.Parameters.Add(paramsErrMsg);



                OracleParameter paramDT = new OracleParameter("ds", OracleType.Cursor);
                paramDT.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramDT);
                IDataReader dataReader = cmd.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

                if (paramsErrMsg.Value == null || string.IsNullOrEmpty(paramsErrMsg.Value.ToString()))
                {

                }
                else
                {
                    throw new Exception(paramsErrMsg.Value.ToString());
                }


            }
            catch (Exception e)
            {
                //throw new Exception(e.Message);
            }
            return ds;
        }
        public DataSet GetAppingBillPerson(string sBillID)
        {
            return GetSubmitBillPerson(sBillID, "");
        }

        public string QueryDataOneFieldValue(string sSQL)
        {
            string sValue = string.Empty;
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection cnn = session.Connection;
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandText = sSQL;
                object obj = cmd.ExecuteScalar();
                sValue = obj == null ? string.Empty : obj.ToString();
            }
            catch
            {
            }
            return sValue;
        }

        /// <summary>
        /// 更具岗位ID获取 ：审批配置表ID 主表  条件  解决方案ID 审批方案名
        /// </summary>
        /// <param name="sOpJobID"></param>
        /// <returns></returns>
        public DataTable QueryCanAduditSolutionByOpJobID(string sOpJobID)
        {
            DataTable oTable = null;
            string sSQL = @"  select distinct t3.id tableID,nvl(t3.physicsname,'')physicsname ,t2.id solutionID, nvl(t2.solutionname ,'')solutionname ,nvl(t2.conditions,'')conditions  from  thd_approleset t1
                            join thd_appstepsset t on t1.parentid=t.id
                            join thd_appsolutionset t2 on t.parentid=t2.id
                            join thd_apptableset t3 on t2.parentid=t3.id and t3.status=0
                            where t1.approle in(
                                select t1.operationrole  from resoperationjobwithrole t1  
                                 where t1.operationjob= '{0}')";
            sSQL = string.Format(sSQL, sOpJobID);
            oTable = QueryDataTable(sSQL);
            return oTable;
        }
        /// <summary>
        /// 根据岗位ID获取角色信息
        /// </summary>
        /// <param name="sOpJobID"></param>
        /// <returns></returns>
        public DataTable QueryRoleByOpJobID(string sOpJobID)
        {
            DataTable oTable = null;
            string sSQL = @"  select t.id stepID,t.steporder, t2.operationrole roleID from thd_appstepsset t
                                    join thd_approleset t1 on t.id=t1.parentid
                                    join resoperationjobwithrole t2 on t2.operationrole=t1.approle and 
                                    t2.operationjob='{0}'";
            sSQL = string.Format(sSQL, sOpJobID);
            oTable = QueryDataTable(sSQL);
            return oTable;
        }
        /// <summary>
        /// 获取所有的权限 审批步骤、审批权限(只有一个角色对应岗位) 2013-4-26
        /// </summary>
        /// <returns></returns>
        public DataTable QueryAllRole()
        {
            DataTable oTable = null;
            string sSQL = @" select distinct t.id stepID, nvl(t2.operationjob,'') jobID  from thd_appstepsset t
                                            join thd_approleset t1 on t.id=t1.parentid
                                            join resoperationjobwithrole t2 on t2.operationrole=t1.approle  ";
            oTable = QueryDataTable(sSQL);
            return oTable;
        }
        private DataTable QueryOnlyOneRole()
        {
            DataTable oTable = null;
            //string sSQL = @" select t1.operationrole from resoperationjobwithrole t1 group by t1.operationrole having count(*)=1 ";
            string sSQL = @" select distinct t.id stepID, nvl(t2.operationjob,'') jobID  from thd_appstepsset t
                                            join thd_approleset t1 on t.id=t1.parentid
                                            join resoperationjobwithrole t2 on t2.operationrole=t1.approle 
                                            and t2.operationrole in(select t1.operationrole from resoperationjobwithrole t1 group by t1.operationrole having count(*)=1)";
            oTable = QueryDataTable(sSQL);
            return oTable;
        }
        /// <summary>
        /// 获取所有组织
        /// </summary>
        /// <returns></returns>
        public DataTable QueryAllOrg()
        {
            DataTable oTable = null;
            string sSQL = "select t.opgid,t.opgsyscode from resoperationorg t";
            oTable = QueryDataTable(sSQL);
            return oTable;
        }
        /// <summary>
        /// 获取提交人的字段名字
        /// </summary>
        /// <param name="sTableName"></param>
        /// <returns></returns>
        public string QueryFieldName(string sTableName, string[] arrColumnNames)
        {
            string sFieldName = string.Empty;
            string sSQL = @"select  t.COLUMN_NAME from user_tab_columns  t where t.TABLE_NAME=upper('{0}') 
                        and  ({1}  )";
            string sFields = string.Empty;
            foreach (string sName in arrColumnNames)
            {
                if (!string.IsNullOrEmpty(sName))
                {
                    if (string.IsNullOrEmpty(sFields))
                    {
                        sFields = string.Format(" COLUMN_NAME=upper('{0}')  ", sName);
                    }
                    else
                    {
                        sFields += string.Format(" or COLUMN_NAME=upper('{0}')  ", sName);
                    }
                }

            }
            sSQL = string.Format(sSQL, sTableName, sFields);
            sFieldName = QueryDataOneFieldValue(sSQL);
            return sFieldName;
        }
        public string QuerySQLFieldName(string sTableName, string[] arrColumnNames)
        {
            string sFieldName = string.Empty;
            string sSQL = @"select  t.COLUMN_NAME from user_tab_columns  t where  rownum=1 and t.TABLE_NAME=upper('{0}') 
                        and  ({1}  )";
            string sFields = string.Empty;
            foreach (string sName in arrColumnNames)
            {
                if (!string.IsNullOrEmpty(sName))
                {
                    if (string.IsNullOrEmpty(sFields))
                    {
                        sFields = string.Format(" COLUMN_NAME=upper('{0}')  ", sName);
                    }
                    else
                    {
                        sFields += string.Format(" or COLUMN_NAME=upper('{0}')  ", sName);
                    }
                }
            }
            sSQL = string.Format(sSQL, sTableName, sFields);

            return sSQL;
        }
        public void QueryFieldName(string sTableName, ref  string sPersonNameFieldName, ref  string sProjectIDFieldName, ref  string sOrgSysCodeFieldName, ref  string sCreateDateFieldName, ref  string sCodeFieldName, ref  string sStateFieldName)
        {
            string sSQL = "select ({0}) as PersonNameFieldName ,({1}) as ProjectIDFieldName ,({2}) as OrgSysCodeFieldName,({3}) as CreateDateFieldName,nvl(({4}),'') as CodeFieldName,({5}) as StateFieldName from dual";
            sSQL = string.Format(sSQL, QuerySQLFieldName(sTableName, arrPersonFieldName), QuerySQLFieldName(sTableName, arrProjectIDFieldName), QuerySQLFieldName(sTableName, arrOrgSysCode), QuerySQLFieldName(sTableName, arrCreateDate), QuerySQLFieldName(sTableName, arrCodes), QuerySQLFieldName(sTableName, arrState));
            DataTable oTable = QueryDataTable(sSQL);
            if (oTable != null && oTable.Rows.Count > 0)
            {
                sPersonNameFieldName = oTable.Rows[0]["PersonNameFieldName"].ToString();
                sProjectIDFieldName = oTable.Rows[0]["ProjectIDFieldName"].ToString();
                sOrgSysCodeFieldName = oTable.Rows[0]["OrgSysCodeFieldName"].ToString();
                sCreateDateFieldName = oTable.Rows[0]["CreateDateFieldName"].ToString();
                sCodeFieldName = oTable.Rows[0]["CodeFieldName"].ToString();
                sStateFieldName = oTable.Rows[0]["StateFieldName"].ToString();
                sCodeFieldName = string.IsNullOrEmpty(sCodeFieldName) ? "''" : "t." + sCodeFieldName;

            }
        }
        string[] arrPersonFieldName = new string[] { "ACCOUNTPERSONNAME", "createpersonname", "CONFIRMHANDLEPERSONNAME", "HANDLEPERSONNAME" };
        string[] arrProjectIDFieldName = new string[] { "theprojectguid", "projectid" };
        string[] arrOrgSysCode = new string[] { "opgsyscode", "ACCOUNTPERSONORGSYSCODE", "HANDLEORG", "ORGSYSCODE" };
        string[] arrCreateDate = new string[] { "createdate", "createtime" };
        string[] arrCodes = new string[] { "code" };
        string[] arrState = new string[] { "DOCSTATE", "state" };
        public string QueryBillSQL(string sTableID, string sSolutionID, string sSolutionName, string sTableName, string sCondition, string sProjectID, string sOpjID, DateTime oDateBegin, DateTime oDateEnd)
        {

            string sPersonNameFieldName = string.Empty;
            string sProjectIDFieldName = string.Empty;
            string sOrgSysCodeFieldName = string.Empty;
            string sCreateDateFieldName = string.Empty;
            string sCodeFieldName = string.Empty;
            string sStateFieldName = string.Empty;
            string sSQL = string.Empty;

            QueryFieldName(sTableName, ref sPersonNameFieldName, ref sProjectIDFieldName, ref sOrgSysCodeFieldName, ref sCreateDateFieldName, ref sCodeFieldName, ref sStateFieldName);
            if (!string.IsNullOrEmpty(sPersonNameFieldName) && !string.IsNullOrEmpty(sProjectIDFieldName) && !string.IsNullOrEmpty(sOrgSysCodeFieldName) && !string.IsNullOrEmpty(sCreateDateFieldName) && !string.IsNullOrEmpty(sStateFieldName))
            {
                sSQL = @"(select   '{0}' as  SolutionName, to_char({12})  as billCode,to_char(t.id) as  billID,to_char(t.{10}) as billSysCode ,to_char(nvl(t.{11},sysdate),'YYYY-MM-DD') billCreateDate,'{1}'  solutionID,  GetAppNextStepOrgSyscode(t.id,'{1}',t.{10},'{15}') OrgSysCodes,to_char(t.{2}) as billCreatePerson  ,'{14}' as  TableID from {3} t
	                    where   t.{13}={4} and   t.{11} between to_date('{5}','YYYY-MM-DD') and to_date('{6} 23:59:59','YYYY-MM-DD HH24:MI:SS') and t.{7}='{8}' and {9} )";
                sSQL = string.Format(sSQL, sSolutionName, sSolutionID, sPersonNameFieldName, sTableName, 3, oDateBegin.AddMonths(-12).ToShortDateString(), oDateEnd.ToShortDateString(), sProjectIDFieldName, sProjectID, string.IsNullOrEmpty(sCondition) ? "1=1" : sCondition, sOrgSysCodeFieldName, sCreateDateFieldName, sCodeFieldName, sStateFieldName, sTableID, sOpjID);
                // sSQL = string.Format(sSQL, sSolutionID, sFieldName, oDateBegin.AddMonths(-12).ToShortDateString(), oDateEnd.ToShortDateString(), sSolutionName, sProjectID, 3, string.IsNullOrEmpty(sCondition)? "1=1" :sCondition,sTableName );
                //oTable = QueryDataTable(sSQL);


            }
            else
            {
                sSQL = string.Empty;
            }
            return sSQL;
        }

        /// <summary>
        /// 过滤数据
        /// </summary>
        /// <param name="oSourceTable"></param>
        /// <param name="sCondition"></param>
        /// <returns></returns>
        public DataTable SelectDataTable(DataTable oSourceTable, string sCondition)
        {
            DataTable oDestTable = null;
            try
            {
                if (oSourceTable == null || oSourceTable.Rows.Count == 0)
                {
                    oDestTable = null;
                }
                else
                {
                    DataRow[] oRows = oSourceTable.Select(sCondition);
                    if (oRows.Length == 0)
                    {
                        oDestTable = null;
                    }
                    else
                    {
                        oDestTable = new DataTable();
                        oDestTable = oSourceTable.Clone();
                        foreach (DataRow oRow in oRows)
                        {
                            oDestTable.ImportRow(oRow);
                        }
                    }
                }
            }
            catch
            {
                oDestTable = null;
            }
            return oDestTable;
        }

        /// <summary>
        /// 获取审批单据
        /// </summary>
        /// <param name="oTotalTable"></param>
        /// <param name="oCurrUserRole"></param>
        /// <param name="oOrgAllTable"></param>
        /// <param name="sOrgSysCode"></param>
        /// <param name="sSolutionName"></param>
        /// <param name="sSolutionID"></param>
        /// <param name="sTableName"></param>
        /// <param name="sCondition"></param>
        /// <param name="sProjectID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        public void GetBill(DataTable oTotalTable, DataTable oCurrUserRole, DataTable OnlyOneRole, DataTable oBillTableTemp, string sOrgSysCode, string sOpjID)
        {
            try
            {
                if (oCurrUserRole != null && oCurrUserRole.Rows.Count > 0 && oBillTableTemp != null && oBillTableTemp.Rows.Count > 0)
                {
                    DataTable oBillTable = SelectDataTable(oBillTableTemp, " OrgSysCodes is not null ");
                    string sAppOrgSysCodes = string.Empty;
                    string sBillStepID = string.Empty;
                    string sBillSysCode = string.Empty;
                    bool IsAdd = false;
                    bool IsAduit = false;

                    foreach (DataRow oRow in oBillTable.Rows)
                    {
                        IsAdd = false;
                        IsAduit = false;
                        string ss = oRow["SolutionName"].ToString();
                        if (ss.StartsWith("分包结算单审批"))
                        {
                            int iii = 0;
                        }
                        sAppOrgSysCodes = oRow["OrgSysCodes"].ToString();//审批步骤id - 最近审核它的一级syscode

                        string sName = oRow["billID"].ToString();
                        string sID = oRow["billCode"].ToString();
                        if (!string.IsNullOrEmpty(sAppOrgSysCodes))
                        {
                            string[] ArrSysCode = sAppOrgSysCodes.Split('-');
                            if (ArrSysCode.Length > 1)
                            {
                                sBillStepID = ArrSysCode[0];
                                // sAppOrgSysCode = ArrSysCode[1];//第一个能审核它的
                                if (!string.IsNullOrEmpty(sBillStepID))
                                {
                                    sBillSysCode = oRow["billSysCode"].ToString();
                                    //拥有审批角色
                                    IsAduit = IsHaveGrant(sBillStepID, oCurrUserRole);
                                    if (IsAduit)
                                    {
                                        if (!string.IsNullOrEmpty(sBillSysCode) && !string.IsNullOrEmpty(sOrgSysCode))
                                        {
                                            if (IsContrain(sOrgSysCode, ArrSysCode))//当前用户所属的组织是 能审核该单据的上一级第一个组织
                                            {
                                                IsAdd = true;
                                            }
                                            else
                                            {
                                                //三、审批角色只有对应一个岗位时 如果与登录岗位匹配则返回（无的情况）
                                                IsAdd = IsOneJobAndSame(sOpjID, sBillStepID, OnlyOneRole);
                                            }
                                            ////同一部门
                                            //if (IsSameDepartBill(sBillSysCode, sOrgSysCode))//单据就属于该部门
                                            //{
                                            //    IsAdd = true;
                                            //}
                                            //else
                                            //{
                                            //    //最近上级
                                            //    if (IsOverLevel(sOrgSysCode, ArrSysCode))//当前用户所属的组织是 能审核该单据的上一级第一个组织
                                            //    {
                                            //        IsAdd = true;
                                            //    }
                                            //    else
                                            //    {
                                            //        if (IsBrother(sBillSysCode, sOrgSysCode))//是否是兄弟节点
                                            //        {
                                            //            IsAdd = true;
                                            //        }
                                            //        else
                                            //        {
                                            //            //三、审批角色只有对应一个岗位时 如果与登录岗位匹配则返回（无的情况）
                                            //            IsAdd = IsOneJobAndSame(sOpjID, sBillStepID, OnlyOneRole);
                                            //        }
                                            //    }
                                            //}
                                        }
                                        if (IsAdd)
                                        {
                                            oTotalTable.ImportRow(oRow);
                                        }
                                    }
                                    else
                                    {
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 是否存在上级
        /// </summary>
        /// <param name="sOrgSysCode"></param>
        /// <param name="sBillSysCode"></param>
        /// <returns></returns>
        public bool IsOverLevel(string sOrgSysCode, string[] ArrSysCode)
        {
            bool bFlag = false;
            for (int i = ArrSysCode.Length - 1; i > 0; i--)
            {

                if (string.Equals(ArrSysCode[i], sOrgSysCode))
                {
                    bFlag = true;
                }
            }
            return bFlag;
        }
        public bool IsContrain(string sOrgSysCode, string[] ArrSysCode)
        {
            bool bFlag = false;
            for (int i = ArrSysCode.Length - 1; i > 0; i--)
            {

                if (string.Equals(ArrSysCode[i], sOrgSysCode))
                {
                    bFlag = true;
                }
            }
            return bFlag;
        }
        public bool IsOneJobAndSame(string sOpjID, string sStepID, DataTable OnlyOneRole)
        {
            bool bFlag = false;
            try
            {
                if (OnlyOneRole != null && OnlyOneRole.Rows.Count > 0)//stepID jobID
                {// t.id stepID,t.steporder, t2.operationrole roleID ,nvl(t2.operationjob,'') jobID 
                    string sCondition = "stepID='{0}'   ";
                    string sOpjIDTemp = string.Empty;
                    sCondition = string.Format(sCondition, sStepID);
                    DataRow[] oRows = OnlyOneRole.Select(sCondition);
                    if (oRows.Length > 0)
                    {
                        foreach (DataRow oRow in oRows)
                        {
                            sOpjIDTemp = oRow["jobID"].ToString();
                            if (string.Equals(sOpjIDTemp, sOpjID))
                            {
                                bFlag = true;
                            }
                            else
                            {
                                bFlag = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        bFlag = false;
                    }
                }
            }
            catch
            {
                bFlag = false;
            }
            return bFlag;
        }
        /// <summary>
        /// 当前用户是该单据的上一级    
        /// </summary>
        /// <param name="sBillSysCode"></param>
        /// <param name="sOrgSysCode"></param>
        /// <returns></returns>
        public bool IsParent(string sBillSysCode, string sOrgSysCode)
        {
            bool bFlag = false;

            bFlag = sBillSysCode.StartsWith(sOrgSysCode);

            return bFlag;
        }

        public bool IsBrother(string sBillSysCode, string sOrgSysCode)
        {
            bool bFlag = false;
            try
            {
                string sBillSysCodeTemp = sBillSysCode.Substring(0, sBillSysCode.LastIndexOf("."));
                sBillSysCodeTemp = sBillSysCode.Substring(0, sBillSysCodeTemp.LastIndexOf("."));
                string sOrgSysCodeTemp = sOrgSysCode.Substring(0, sOrgSysCode.LastIndexOf("."));
                sOrgSysCodeTemp = sOrgSysCode.Substring(0, sOrgSysCodeTemp.LastIndexOf("."));
                if (string.Equals(sBillSysCodeTemp, sOrgSysCodeTemp))
                {
                    bFlag = true;
                }
            }
            catch
            {
                bFlag = false;
            }
            return bFlag;


        }
        /// <summary>
        /// 检查该单据是否是该部门 
        /// </summary>
        /// <param name="sBillStepID"></param>
        /// <param name="sBillOrgSysCode"></param>
        /// <param name="oCurrUserRole"></param>
        /// <param name="sOrgSysCode"></param>
        /// <returns></returns>
        public bool IsSameDepartBill(string sBillOrgSysCode, string sOrgSysCode)
        {
            bool bFlag = false;
            try
            {
                if (string.Equals(sOrgSysCode, sBillOrgSysCode))
                {
                    bFlag = true;
                }

            }
            catch
            {
                bFlag = false;
            }
            return bFlag;
        }
        /// <summary>
        /// 并且该审批步骤当前登录人可以审批
        /// </summary>
        /// <param name="sBillStepID"></param>
        /// <param name="oCurrUserRole"></param>
        /// <returns></returns>
        public bool IsHaveGrant(string sBillStepID, DataTable oCurrUserRole)
        {
            bool bFlag = false;
            try
            {
                string sCondition = " stepID='" + sBillStepID + "'";
                if (oCurrUserRole.Select(sCondition).Length > 0)
                {
                    bFlag = true;
                }
            }
            catch
            {
                bFlag = false;
            }
            return bFlag;
        }

        public DataTable CreateEmptyBillDataTable()
        {
            //  SolutionName   billCode  billID  billSysCode  billCreateDate  solutionID,    OrgSysCodes  billCreatePerson
            string[] arrFieldName = new string[] { "billCode", "billSysCode", "billCreateDate", "OrgSysCodes", "billCreatePerson", "solutionID", "SolutionName", "billID", "TableID" };
            DataTable oTable = new DataTable();
            foreach (string sFieldName in arrFieldName)
            {
                oTable.Columns.Add(sFieldName);
            }
            return oTable;
        }
        /// <summary>
        /// 根据审批配置表和单据ID获取单据数据
        /// </summary>
        /// <param name="sTableSetID"></param>
        /// <param name="sBillID"></param>
        /// <returns>[0]审批配置表 [1] 单据</returns>
        public IList GetBill(string sTableSetID, string sBillID)
        {
            string sClassName = string.Empty;
            AppTableSet oAppTableSet = null;
            IList lstObj = null;
            try
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", sTableSetID));
                lstObj = dao.ObjectQuery(typeof(AppTableSet), oq);
                if (lstObj != null && lstObj.Count > 0)
                {
                    oAppTableSet = lstObj[0] as AppTableSet;
                    //GetDomainByCondition(string ClassName, ObjectQuery oq)
                    if (oAppTableSet != null)
                    {
                        oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Id", sBillID));
                        oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                        sClassName = oAppTableSet.MasterNameSpace;
                        IList list = GetDomainByCondition(sClassName, oq);
                        if (list != null && list.Count > 0)
                        {
                            lstObj.Insert(1, list[0]);
                        }
                    }
                }
            }
            catch
            {
            }
            return lstObj;
        }
        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="oAppTableSet"></param>
        /// <returns></returns>
        public IList GetBill(AppTableSet oAppTableSet, string sProjectID)
        {
            string sClassName = string.Empty;
            IList lstObj = null;

            if (oAppTableSet != null)
            {
                ObjectQuery oq = new ObjectQuery();
                
                //oq.AddCriterion(Expression.Eq(oAppTableSet.StatusName , 3));
                oq.AddCriterion(NHibernate.Criterion.Expression.Sql("this_." + oAppTableSet.StatusName + "=3"));
                oq.AddCriterion(Expression.Eq("ProjectId", sProjectID));
                //oq.AddCriterion(Expression.Sql(oAppTableSet.StatusName + "=3"));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                sClassName = oAppTableSet.MasterNameSpace;
                string sSQL = GetConditionByAppTableSet(oAppTableSet);
                if (!string.IsNullOrEmpty(sSQL))
                {
                    oq.AddCriterion(NHibernate.Criterion.Expression.Sql(sSQL));
                }
                lstObj = GetDomainByCondition(sClassName, oq);

            }

            return lstObj;
        }
       public  IList GetBill(AppTableSet oAppTableSet, string sProjectID, DateTime dBeginTime, DateTime dEndTime)
        {
            string sClassName = string.Empty;
            IList lstObj = null;

            if (oAppTableSet != null)
            {
                ObjectQuery oq = new ObjectQuery();
                string sSQL1 = string.Format(" this_.{0}={1}   and not exists (select 1 from thd_appmasterdata t1 where   this_.id=t1.billid and t1.apptableset='{2}' and t1.projectid='{3}'  AND T1.APPSTATE=2) ", oAppTableSet.StatusName, 3, oAppTableSet.Id, sProjectID);
                //oq.AddCriterion(Expression.Eq(oAppTableSet.StatusName , 3));
               // oq.AddCriterion(NHibernate.Criterion.Expression.Sql("this_." + oAppTableSet.StatusName + "=3"));
                oq.AddCriterion(NHibernate.Criterion.Expression.Sql(sSQL1));
                oq.AddCriterion(Expression.Eq("ProjectId", sProjectID));
                //oq.AddCriterion(Expression.Sql(oAppTableSet.StatusName + "=3"));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                sClassName = oAppTableSet.MasterNameSpace;
                string sSQL = GetConditionByAppTableSet(oAppTableSet);
                if (!string.IsNullOrEmpty(sSQL))
                {
                    oq.AddCriterion(NHibernate.Criterion.Expression.Sql(sSQL));
                }
                lstObj = GetDomainByCondition(sClassName, oq);

            }

            return lstObj;
        }
        public string GetConditionByAppTableSet(AppTableSet oAppTableSet)
        {
            string sSQL=string.Empty ;
             
            ObjectQuery oObjectQuery = new ObjectQuery();
            oObjectQuery.AddCriterion(Expression.Eq("ParentId.Id",oAppTableSet.Id));
            IList lst=dao.ObjectQuery(typeof(AppSolutionSet),oObjectQuery);
            if(lst!=null )
            {
                foreach(AppSolutionSet oAppSolutionSet in lst )
                {
                    if(!string.IsNullOrEmpty(sSQL))
                    {
                        sSQL+=" or  ";
                    }
                    sSQL+=oAppSolutionSet.Conditions;
                }
                if(!string.IsNullOrEmpty(sSQL))
                    {
                        sSQL=" ( "+sSQL+")";
                    }
            }
            return sSQL;

        }
        /// <summary>
        /// 通过步骤ID获取审批步骤
        /// </summary>
        /// <param name="sStepID"></param>
        /// <returns></returns>
        public AppStepsSet GetAppStepSetByStepID(string sStepID)
        {
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();

            oq.AddFetchMode("AppRoleSets", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRoleSets.AppRole", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppTableSet", NHibernate.FetchMode.Eager);
            oq.AddCriterion(Expression.Eq("Id", sStepID));
            list = base.GetDomainByCondition(typeof(AppStepsSet), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as AppStepsSet;
            }
            else
            {
                return null;
            }
        }

        public AppTableSet GetAppTableSetByID(string sTableSteID)
        {
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", sTableSteID));
            list = base.GetDomainByCondition(typeof(AppTableSet), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as AppTableSet;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region  新审批平台
        private AppTableSet GetAppTableSetByType(Type billType)
        {
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ClassName", billType.Name));
            objQuery.AddCriterion(Expression.Eq("MasterNameSpace", billType.FullName));
            objQuery.AddCriterion(Expression.Eq("Status", 0));

            var list = Dao.ObjectQuery(typeof (AppTableSet), objQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0] as AppTableSet;
        }

        private IList GetAppSolutionByTableDefine(AppTableSet tableSet)
        {
            if (tableSet == null)
            {
                return null;
            }

            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ParentId", tableSet));

            var list = Dao.ObjectQuery(typeof (AppSolutionSet), objQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list;
        }

        private bool IsBillInSolution(string billId, Type billType, string souCondition)
        {
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("Id", billId));
            objQuery.AddCriterion(NHibernate.Criterion.Expression.Sql(souCondition));

            var list = Dao.ObjectQuery(billType, objQuery);
            return list != null && list.Count > 0;
        }

        private void GetApproveNextStep(ApproveBill bill)
        {
            if (bill == null)
            {
                return;
            }

            //获取该单据该步骤的配置信息
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("AppTableSet", bill.AppTableDefine));
            objQuery.AddCriterion(Expression.Eq("ParentId", bill.AppSolution));
            objQuery.AddFetchMode("AppRoleSets", FetchMode.Eager);

            var list = Dao.ObjectQuery(typeof (AppStepsSet), objQuery).OfType<AppStepsSet>().ToList();
            if (list.Count == 0)
            {
                return;
            }
            var curStep = list.Find(s => s.StepOrder == bill.NextStep);

            //获取该单据的所有审批步骤信息
            var stepInfos = GetAppStepsInfoByBill(bill).FindAll(s=>s.AppStepsSet == curStep);
            var nextStep = bill.NextStep;
            if (curStep.AppRelations == 0)
            {
                //或关系，该步骤下定义的角色有任意审批记录即可进入下一步
                if (stepInfos.Exists(s => s.State == 1 && s.AppStatus == 2 && curStep.AppRoleSets.Any(r => r.AppRole == s.AppRole)))
                {
                    nextStep = bill.NextStep + 1;
                }
            }
            else
            {
                //与关系，该步骤下定义的角色有任意没有审批记录的继续当前步骤
                if (!curStep.AppRoleSets.Any(r => !stepInfos.Exists(s => s.State == 1 && s.AppRole == r.AppRole)))
                {
                    nextStep = bill.NextStep + 1;
                }
            }

            if (nextStep > list.Max(s => s.StepOrder))
            {
                bill.IsDone = true;
                bill.NextStep = 0;
            }
            else
            {
                bill.IsDone = false;
                bill.NextStep = nextStep;
            }
        }

        private AppStepsSet GetAppStepsSetByBill(ApproveBill bill)
        {
            if (bill == null)
            {
                return null;
            }

            //获取该单据该步骤的配置信息
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("AppTableSet", bill.AppTableDefine));
            objQuery.AddCriterion(Expression.Eq("ParentId", bill.AppSolution));
            objQuery.AddFetchMode("AppRoleSets", FetchMode.Eager);

            var list = Dao.ObjectQuery(typeof(AppStepsSet), objQuery).OfType<AppStepsSet>().ToList();
            if (list.Count == 0)
            {
                return null;
            }
            var curStep = list.Find(s => s.StepOrder == bill.NextStep);
            return curStep;
        }

        public List<AppStepsInfo> GetAppStepsInfoByBill(ApproveBill bill)
        {
            if (bill == null)
            {
                return null;
            }

            //获取该单据该步骤的配置信息
            var objQuery = new ObjectQuery();
            objQuery.Criterions.Clear();
            objQuery.AddCriterion(Expression.Eq("BillId", bill.BillId));
            objQuery.AddCriterion(Expression.Eq("AppTableSet", bill.AppTableDefine));
            objQuery.AddCriterion(Expression.Eq("State", 1));
            objQuery.AddFetchMode("AppStepsSet", FetchMode.Eager);
            objQuery.AddFetchMode("AppRole", FetchMode.Eager);
            objQuery.AddFetchMode("AuditPerson", FetchMode.Eager);

            return Dao.ObjectQuery(typeof(AppStepsInfo), objQuery).OfType<AppStepsInfo>().ToList();
        }

        private List<OperationJobWithRole> GetOperationRoleByJob(string jobId)
        {
            if(string.IsNullOrEmpty(jobId))
            {
                return null;
            }

            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("OperationJob.Id", jobId));

            return Dao.ObjectQuery(typeof(OperationJobWithRole), objQuery).OfType<OperationJobWithRole>().ToList();
        }

        private bool UpdateBillState(AppStepsInfo result)
        {
            if (result == null)
            {
                return false;
            }

            string sql = string.Format("update {0} set {1} = {2} where Id = '{3}'",
                                       result.AppTableSet.PhysicsName,
                                       result.AppTableSet.StatusName,
                                       (int)(result.AppStatus == 2 ? DocumentState.InExecute : DocumentState.Edit),
                                       result.BillId);
            return ExecuteSql(sql) > 0;
        }

        private void UpdateBillData(AppStepsInfo result)
        {
            if (result == null || result.TempData == null)
            {
                return;
            }

            switch (result.AppTableSet.ClassName)
            {
                case "ProjectFundPlanMaster":
                    var plan = Dao.Get(typeof (ProjectFundPlanMaster), result.BillId) as ProjectFundPlanMaster;
                    if (plan != null && result.TempData.Name1 != null)
                    {
                        plan.ApprovalAmount = (decimal) result.TempData.Name1;
                    }
                    Dao.SaveOrUpdate(plan);
                    break;
                case "FilialeFundPlanMaster":
                    var fiPlan = Dao.Get(typeof(FilialeFundPlanMaster), result.BillId) as FilialeFundPlanMaster;
                    if (fiPlan != null && result.TempData.Name1 != null)
                    {
                        fiPlan.Approval = (decimal)result.TempData.Name1;
                    }
                    Dao.SaveOrUpdate(fiPlan);
                    break;
            }
        }

        private bool UnValidAppStepInfo(ApproveBill bill)
        {
            if (bill == null)
            {
                return false;
            }

            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("BillId", bill.BillId));

            var list = Dao.ObjectQuery(typeof (AppStepsInfo), objQuery);
            foreach (AppStepsInfo stepsInfo in list)
            {
                stepsInfo.State = 2;
            }

            return Dao.SaveOrUpdate(list);
        }

        [TransManager]
        public ApproveBill SaveApproveBill(ApproveBill bill, Type billType)
        {
            var appTableSet = GetAppTableSetByType(billType);
            if (appTableSet == null)
            {
                throw new Exception("该单据的没有可用的审批表单定义");
            }
            bill.AppTableDefine = appTableSet;

            var souList = GetAppSolutionByTableDefine(appTableSet);
            if (souList == null || souList.Count == 0)
            {
                throw new Exception("该单据没有配置审批方案，请先配置审批方案");
            }

            var billList = new List<ApproveBill>();
            foreach (AppSolutionSet solution in souList)
            {
                if (!string.IsNullOrEmpty(solution.Conditions)
                    && !IsBillInSolution(bill.BillId, billType, solution.Conditions))
                {
                    continue;
                }

                var nBill = new ApproveBill();
                nBill.BillCode = bill.BillCode;
                nBill.BillCreateDate = bill.BillCreateDate;
                nBill.BillCreatePerson = bill.BillCreatePerson;
                nBill.BillCreatePersonName = bill.BillCreatePersonName;
                nBill.BillId = bill.BillId;
                nBill.BillSysCode = bill.BillSysCode;
                nBill.AppTableDefine = appTableSet;
                nBill.AppSolution = solution;
                nBill.AppSolutionName = solution.SolutionName;
                nBill.IsDone = false;
                nBill.LastModifTime = DateTime.Now;
                nBill.LastModifyBy = bill.BillCreatePersonName;
                nBill.ProjectId = bill.ProjectId;
                nBill.ProjectName = bill.ProjectName;

                nBill.NextStep = 1;

                billList.Add(nBill);
            }

            var res = Dao.SaveOrUpdate(billList);

            return bill;
        }

        public IList GetApprovingBills(string orgJobId, string personId, string projId)
        {
            var ds = QueryDataToDataSet(
                string.Format(SqlScript.GetApprovingBillSql,
                              orgJobId, personId,
                              string.IsNullOrEmpty(projId)
                                  ? string.Empty
                                  : string.Format(" and a.projectid = '{0}' ", projId)));
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var idConstions = new Disjunction();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                idConstions.Add(Expression.Eq("Id", ds.Tables[0].Rows[i]["Id"]));
            }

            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(idConstions);

            return Dao.ObjectQuery(typeof (ApproveBill), objQuery);
        }

        public ApproveBill GetApproveBillById(string id)
        {
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("Id", id));
            objQuery.AddFetchMode("BillCreatePerson", FetchMode.Eager);
            objQuery.AddFetchMode("AppTableDefine", FetchMode.Eager);
            objQuery.AddFetchMode("AppSolution", FetchMode.Eager);

            var list = Dao.ObjectQuery(typeof (ApproveBill), objQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }
            else
            {
                return list[0] as ApproveBill;
            }
        }

        [TransManager]
        public bool SubmitApprove(AppStepsInfo result, ApproveBill bill)
        {
            if (result == null || bill == null)
            {
                throw new Exception("审批失败：审批单据及审批结果不能为空");
            }

            var newBill = GetApproveBillById(bill.Id);
            if (newBill.Version != bill.Version)
            {
                throw new Exception(string.Format("审批失败：单据{0}已经被修改，请刷新后再试", bill.BillCode));
            }

            newBill.ApproveJob = bill.ApproveJob;

            //补全审批步骤信息
            var curStep = GetAppStepsSetByBill(newBill);
            if (curStep == null)
            {
                throw new Exception("审批失败：没有获取到当前审批步骤信息");
            }

            result.AppTableSet = newBill.AppTableDefine;
            result.State = 1;
            result.AppDate = DateTime.Now;
            result.AppRelations = curStep.AppRelations;
            result.AppStepsSet = curStep;
            result.StepsName = curStep.StepsName;

            var stepInfos = GetAppStepsInfoByBill(newBill);
            if (stepInfos == null || stepInfos.Count == 0)
            {
                result.BillAppDate = result.AppDate;
            }
            else
            {
                result.BillAppDate = stepInfos.Min(s => s.AppDate);
            }

            var jobRoles = GetOperationRoleByJob(newBill.ApproveJob);
            if (jobRoles == null || jobRoles.Count == 0)
            {
                throw new Exception("审批失败：没有获取到当前岗位对应的角色信息");
            }

            var appRole = curStep.AppRoleSets.FirstOrDefault(r => jobRoles.Exists(j => j.OperationRole == r.AppRole));
            if (appRole == null)
            {
                throw new Exception("审批失败：没有获取到当前审批步骤的角色信息");
            }

            result.AppRole = appRole.AppRole;
            result.RoleName = appRole.RoleName;
            
            //保存审批结果
            if (!Dao.SaveOrUpdate(result))
            {
                return false;
            }

            //计算审批下一步
            if (result.AppStatus == 2)//通过
            {
                GetApproveNextStep(newBill);
                newBill.LastModifTime = DateTime.Now;
                newBill.LastModifyBy = result.AuditPerson.Name;

                if (!Dao.SaveOrUpdate(newBill))
                {
                    return false;
                }

                UpdateBillData(result);

                if (newBill.IsDone)
                {
                    UpdateBillState(result);

                    AppAgreeByBusiness(newBill.AppTableDefine, newBill.BillId);
                }
            }
            else//不通过
            {
                UpdateBillData(result);

                UpdateBillState(result);

                UnValidAppStepInfo(newBill);

                Dao.Delete(newBill);
            }

            return true;
        }

        public IList GetApprovingSets(string tName, string sysCode)
        {
            string sql = @"
            select a.apptableset,b.tablename
            from thd_approvebill a
            join thd_apptableset b on b.id = a.apptableset
            where 1 = 1 {0}
            group by a.apptableset,b.tablename
            order by b.tablename";

            StringBuilder condition = new StringBuilder();
            if(!string.IsNullOrEmpty(tName))
            {
                condition.Append(" and b.tablename like '%" + tName + "%' ");
            }
            if(!string.IsNullOrEmpty(sysCode))
            {
                condition.Append(" and a.BILLSYSCODE like '" + sysCode + "%' ");
            }

            var ds = QueryDataToDataSet(string.Format(sql, condition));
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var list = new List<AppTableSet>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var tbSet = new AppTableSet();
                tbSet.Id = ds.Tables[0].Rows[i][0].ToString();
                tbSet.TableName = ds.Tables[0].Rows[i][1].ToString();

                list.Add(tbSet);
            }

            return list;
        }

        public IList GetApproveBillByType(string appSet, string billCode, string sysCode, DateTime bgDate, DateTime endDate)
        {
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Ge("BillCreateDate", bgDate));
            objQuery.AddCriterion(Expression.Le("BillCreateDate", endDate));
            if(!string.IsNullOrEmpty(appSet))
            {
                objQuery.AddCriterion(Expression.Eq("AppTableDefine.Id", appSet));
            }
            if(!string.IsNullOrEmpty( billCode))
            {
                objQuery.AddCriterion(Expression.Like("BillCode", billCode, MatchMode.Anywhere));
            }
            if (!string.IsNullOrEmpty(sysCode))
            {
                objQuery.AddCriterion(Expression.Like("BillSysCode", sysCode, MatchMode.Start));
            }

            objQuery.AddFetchMode("AppSolution", FetchMode.Eager);
            objQuery.AddOrder(new NC.Order("IsDone", true));
            objQuery.AddOrder(new NC.Order("BillCode", true));

            return Dao.ObjectQuery(typeof (ApproveBill), objQuery);
        }

        public IList GetDefineStepsBySoulution(string souId)
        {
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ParentId.Id", souId));
            objQuery.AddFetchMode("AppRoleSets", FetchMode.Eager);
            objQuery.AddOrder(new NC.Order("StepOrder", true));
            
            return Dao.ObjectQuery(typeof(AppStepsSet), objQuery);
        }

        #endregion

        #region 修改单据
        /// <summary>
        /// 获取所有项目信息
        /// </summary>
        /// <returns></returns>
        public IList GetAllProject()
        {
            IList list = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddOrder(NHibernate.Criterion.Order.Asc("Name"));
            list = Dao.ObjectQuery(typeof(CurrentProjectInfo), oq);
            return list;
        }
        public DataTable GetSetBillData(string sSQL)
        {
            DataTable oTable = this.QueryDataTable(sSQL);



            return oTable;
        }
        public bool SetBillData(string sSQL)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand cmd = cnn.CreateCommand();
            cmd.CommandText = sSQL;
            return cmd.ExecuteNonQuery() > 0;
        }
        #endregion

        public void DailyBackResourcePlan(string billId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", billId));
            IList listDaily = dao.ObjectQuery(typeof(DailyPlanDetail), oq);

            foreach (DailyPlanDetail dtl in listDaily)
            {
                if (dtl.ForwardDetailId != null)
                {
                    ObjectQuery oq1 = new ObjectQuery();
                    oq1.AddCriterion(Expression.Eq("MaterialResource.Id", dtl.MaterialResource.Id));
                    if (dtl.DiagramNumber == null)
                    {
                        oq1.AddCriterion(Expression.IsNull("DiagramNumber"));
                    }
                    else
                    {
                        oq1.AddCriterion(Expression.Eq("DiagramNumber", dtl.DiagramNumber));
                    }

                    oq1.AddCriterion(Expression.Like("TheGWBSSysCode", dtl.ProjectTaskSysCode, MatchMode.Start));
                    IList list = dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq1);
                    if (list == null || list.Count == 0) return;
                    foreach (ResourceRequirePlanDetail plan in list)
                    {
                        plan.DailyPlanPublishQuantity += dtl.Quantity;
                    }
                    SaveOrUpdateByDao(list);
                }
            }
        }

        public void MonthlyBackResourcePlan(string billId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", billId));
            IList listDaily = dao.ObjectQuery(typeof(MonthlyPlanDetail), oq);

            foreach (MonthlyPlanDetail dtl in listDaily)
            {
                if (dtl.ForwardDetailId != null)
                {
                    ObjectQuery oq1 = new ObjectQuery();
                    oq1.AddCriterion(Expression.Eq("MaterialResource.Id", dtl.MaterialResource.Id));
                    if (dtl.DiagramNumber == null)
                    {
                        oq1.AddCriterion(Expression.IsNull("DiagramNumber"));
                    }
                    else
                    {
                        oq1.AddCriterion(Expression.Eq("DiagramNumber", dtl.DiagramNumber));
                    }

                    oq1.AddCriterion(Expression.Like("TheGWBSSysCode", dtl.ProjectTaskSysCode, MatchMode.Start));
                    IList list = dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq1);
                    if (list == null || list.Count == 0) return;
                    foreach (ResourceRequirePlanDetail plan in list)
                    {
                        plan.MonthPlanPublishQuantity += dtl.Quantity;
                    }
                    SaveOrUpdateByDao(list);
                }
            }
        }

        public void SupplyBackResourcePlan(string billId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", billId));
            IList listDaily = dao.ObjectQuery(typeof(SupplyPlanDetail), oq);

            foreach (SupplyPlanDetail dtl in listDaily)
            {
                if (dtl.ForwardDetailId != null)
                {
                    ObjectQuery oq1 = new ObjectQuery();
                    oq1.AddCriterion(Expression.Eq("MaterialResource.Id", dtl.MaterialResource.Id));
                    if (dtl.DiagramNumber == null)
                    {
                        oq1.AddCriterion(Expression.IsNull("DiagramNumber"));
                    }
                    else
                    {
                        oq1.AddCriterion(Expression.Eq("DiagramNumber", dtl.DiagramNumber));
                    }

                    oq1.AddCriterion(Expression.Like("TheGWBSSysCode", dtl.SysCode, MatchMode.Start));
                    IList list = dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq1);
                    if (list == null || list.Count == 0) return;
                    foreach (ResourceRequirePlanDetail plan in list)
                    {
                        plan.SupplyPlanPublishQuantity += dtl.Quantity;
                    }
                    SaveOrUpdateByDao(list);
                }
            }
        }
    }
}