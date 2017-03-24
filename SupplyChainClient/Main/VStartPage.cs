using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using MessageClient.UserManager;
using MessageClient;
using System.Configuration;
using AuthManager.AuthMng.AuthControlsMng;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Linq;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using Application.Business.Erp.SupplyChain.Client.PMCAndWarning.MaterialWarning;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.Main
{
    public partial class VStartPage : TSystemView
    {
        public static MainForm MsgMainForm = null;
        public static TreeView MenuTreeView = null;
        private Timer oTimerMsg = null;
        private static IPMCAndWarningSrv warnSrv = null;
        private static IAppSrv appSrv = null;
        private static CurrentProjectInfo oProjectInfo = null;
        public VStartPage()
        {
            InitializeComponent();
            InitialEvents();
            oProjectInfo = StaticMethod.GetProjectInfo();
        }
        static ILaborSporadicSrv laborSporadicSrv;
        private ILaborSporadicSrv LaborSporadicSrv{
            get
            {
                if (laborSporadicSrv == null)
                {
                    laborSporadicSrv = StaticMethod.GetService("LaborSporadicSrv") as ILaborSporadicSrv;
                }
                return laborSporadicSrv;
            }
        }
        static IProjectTaskAccountSrv projectTaskAccountSrv;
        private IProjectTaskAccountSrv ProjectTaskAccountSrv
        {
            get
            {
                if (projectTaskAccountSrv == null) { projectTaskAccountSrv = StaticMethod.GetService("ProjectTaskAccountSrv") as IProjectTaskAccountSrv; }
                return projectTaskAccountSrv;
            }
        }
        private void InitialEvents()
        {

        }

        public void Start()
        {
            try
            {
                try
                {
                    if (MenuTreeView != null)
                    {
                        authGo1.SetDataSource(MenuTreeView);
                        authGo1.AuthGo_Menu += new AuthGo_Menu(authGo1_AuthGo_Menu);
                    }
                }
                catch { }
                try
                {
                    //this.pnlFloor.BackgroundImage = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"Images\StartBack.jpg");
                }
                catch { }

                try
                {
                    //this.label1.Text = System.Configuration.ConfigurationManager.AppSettings["AppName"];
                }
                catch { }


                #region 审批任务
                ShowAuditTask();
                #endregion

                ObjectLock.Lock(this.pnlFloor, true);

                if (!System.Diagnostics.Debugger.IsAttached)
                {

                    if (ConfigurationManager.AppSettings["MsgEnable"].Equals("True", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            VLogin VLogin = new VLogin();
                            MainForm MainForm = VLogin.DoLogin(ConstObject.TheLogin.ThePerson.Code, ConstObject.TheLogin.ThePerson.Name,
                                                                ConfigurationManager.AppSettings["MsgChannel"], ConfigurationManager.AppSettings["MsgIp"], ConfigurationManager.AppSettings["MsgPort"]);
                            MainForm.TopMost = true;
                            MainForm.Show();
                            MsgMainForm = MainForm;

                        }
                        catch { }
                    }
                }
                var rankingUrl = ConfigurationManager.AppSettings["RankingSysUrl"];
                wbServer.Url = new Uri(rankingUrl);
                #region 消息窗体
                if (oProjectInfo != null && oProjectInfo.Code != CommonUtil.CompanyProjectCode)
                {
                    oTimerMsg = new Timer() { Enabled=true, Interval=10*60*1000 };

                    //lstBoxMsg.Font = new Font("宋体",9,FontStyle.Bold);
                   // lstBoxMsg.ForeColor = System.Drawing.SystemColors.Control;
                   
         
                    oTimerMsg.Tick += new EventHandler(TimerMsg_Tick);
                    TimerMsg_Tick(null,null);
                }
                 
                #endregion

            }
            catch (Exception e)
            {
                MessageBox.Show("视图初始化出错：" + VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(e));
            }
        }
        #region 消息窗体
        private void TimerMsg_Tick(object sender, EventArgs e)
        {
            try
            {
                lstBoxMsg.Items.Clear();
                DataSet ds = LaborSporadicSrv.GetAuthedLaborSporadic(oProjectInfo.Id);
               // lstBoxMsg.Items.Clear();
                string sValue = string.Empty;

                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    sValue = string.Format("有{1}个{0}单需要复核", ClientUtil.ToString(oRow["laborstate"]), ClientUtil.ToString(oRow["cnt"]));
                    lstBoxMsg.Items.Insert(lstBoxMsg.Items.Count, sValue);
                }
                bool isNeedProjectTaskAccount = ProjectTaskAccountSrv.IsNeedProjectTaskAccount(oProjectInfo.Id);
                if (isNeedProjectTaskAccount)
                {
                    sValue = "本项目有工程量提报需要确认";
                    lstBoxMsg.Items.Add(sValue);
                }
            }
            catch
            {
                oTimerMsg.Enabled = false;
            }
        }
        #endregion
        #region 审批方法

        IList list_AppStepsInfo = null;
        /// <summary>
        /// 需要审批的单据
        /// </summary>
        private Hashtable neededAuditBillHash = new Hashtable();

        /// <summary>
        /// 当前单据对应的审批步骤
        /// </summary>
        private Hashtable BillIdAppStepsSetHash = new Hashtable();

        private void ShowAuditTask()
        {
            try
            {
                if (appSrv == null)
                    appSrv = StaticMethod.GetService("RefAppSrv") as IAppSrv;

                dgBill.Rows.Clear();
                bool bFlag = false;
                #region 以前的
                if (bFlag)
                {
                    IList appTableSetLst = CurrentUserAppTableSet();
                    BillIdAppStepsSetHash.Clear();
                    foreach (AppTableSet obj in appTableSetLst)
                    {
                        Query(obj);
                        //dgMaster_SelectionChanged(dgMaster, new EventArgs());
                    }
                }
                #endregion
                else
                {
                    string sErrMsg = string.Empty;
                    DataTable tbBill = appSrv.GetAppBillByProc(ConstObject.TheSysRole.Id, ConstObject.TheOperationOrg.SysCode, StaticMethod.GetProjectInfo().Id, DateTime.Parse("2010-1-1"), DateTime.Now, ref sErrMsg);
                    if (!string.IsNullOrEmpty(sErrMsg))
                    {
                        MessageBox.Show(sErrMsg);
                        return;
                    }
                    DgBillAddRow(tbBill);
                }
                colCode.Width = 200;

            }
            catch
            {
            }
        }

        private void Query(AppTableSet master)
        {
            IList ListMaster = new ArrayList();
            IList ListDetail = new ArrayList();

            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Ge("CreateDate", LoginInfomation.LoginInfo.LoginDate.AddMonths(-1).Date));
            //oq.AddCriterion(Expression.Lt("CreateDate", LoginInfomation.LoginInfo.LoginDate.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InAudit));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

            ListMaster = appSrv.GetDomainByCondition(master.MasterNameSpace, oq);
            ListMaster = FilterData(ListMaster, master);
            IList masterData = new ArrayList();
            IList detailData = new ArrayList();
            foreach (object obj in ListMaster)
            {
                //int rowIndex = dgMaster.Rows.Add();
                //masterData = new ArrayList();
                string Id = ClientUtil.ToString(obj.GetType().GetProperty("Id").GetValue(obj, null));

                //把当前单据放到hashtable中
                if (!neededAuditBillHash.ContainsKey(Id))
                {
                    neededAuditBillHash.Add(Id, obj);
                }

                //dgMaster["colMasterId", rowIndex].Value = Id;
                //dgMaster["colMasterId", rowIndex].Tag = Id;
                //parentIdStr = parentIdStr + Id + ",";
                string dgBillCode = "";
                string dgBillCreatePerson = "";
                string dgBillCreateDate = "";
                foreach (System.Reflection.PropertyInfo pi in obj.GetType().GetProperties())
                {
                    if (pi.Name == "CreatePersonName" || pi.Name == "ConfirmHandlePersonName")
                    {
                        dgBillCreatePerson = ClientUtil.ToString(pi.GetValue(obj, null));
                    }
                    if (pi.Name == "Code")
                    {
                        dgBillCode = ClientUtil.ToString(pi.GetValue(obj, null));
                    }
                    if (pi.Name == "CreateDate")
                    {
                        dgBillCreateDate = ClientUtil.ToDateTime(pi.GetValue(obj, null)).ToShortDateString();
                    }
                }
                DgBillAddRow(master, dgBillCode, dgBillCreatePerson, dgBillCreateDate, Id);
            }
            //dgBill.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }
        private void DgBillAddRow(DataTable tbBill)
        {
            if (tbBill != null && tbBill.Rows.Count > 0)
            {
                string sSolutionName = string.Empty;
                string sBillCode = string.Empty;
                string sBillCreatePerson = string.Empty;
                string sBillCreateDate = string.Empty;
                string sBillID = string.Empty;
                string sTableID = string.Empty;
                int i = 0;
                foreach (DataRow oRow in tbBill.Rows)
                {
                    sSolutionName = oRow["SolutionName"].ToString();
                    sBillCode = oRow["billCode"].ToString();
                    sBillCreatePerson = oRow["billCreatePerson"].ToString();
                    sBillCreateDate = oRow["billCreateDate"].ToString();
                    sBillID = oRow["billID"].ToString();
                    sTableID = oRow["TableID"].ToString();

                    i = dgBill.Rows.Add();
                    DataGridViewRow dr = dgBill.Rows[i];
                    dr.Tag = sTableID;
                    dr.Cells[colCode.Name].Value = sBillCode;
                    dr.Cells[colCode.Name].Tag = sBillID;
                    dr.Cells[colCreatePerson.Name].Value = sBillCreatePerson;
                    dr.Cells[colCreateDate.Name].Value = sBillCreateDate;
                    dr.Cells[collBillName.Name].Value = sSolutionName;

                }
            }

        }
        private void DgBillAddRow(AppTableSet theAppTableSet, string code, string createPerson, string createDate, string id)
        {
            int i = dgBill.Rows.Add();
            DataGridViewRow dr = dgBill.Rows[i];
            dr.Tag = theAppTableSet;
            dr.Cells[colCode.Name].Value = code;
            dr.Cells[colCode.Name].Tag = id;
            dr.Cells[colCreatePerson.Name].Value = createPerson;
            dr.Cells[colCreateDate.Name].Value = createDate;
            dr.Cells[collBillName.Name].Value = theAppTableSet.TableName;
        }

        private IList CurrentUserAppTableSet()
        {
            //当前登录用户的角色
            IList userRolelst = GetOperationRoleByJobId(ConstObject.TheSysRole.Id);
            string roleConStr = " and c.appRole in('1'";
            foreach (OperationRole userRole in userRolelst)
            {
                roleConStr = roleConStr + ",'" + userRole.Id + "'";
            }
            roleConStr += ")";

            string sql = @"select distinct a.parentid appTableSetId from thd_appsolutionset a join thd_appstepsset b on b.parentid=a.id
                join thd_approleset c on c.parentid=b.id where 1=1 " + roleConStr;
            IList lst = appSrv.CurrentUserTableSet(sql);
            return lst;
        }

        private IList GetOperationRoleByJobId(string jobId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OperationJob.Id", jobId));
            oq.AddFetchMode("OperationRole", NHibernate.FetchMode.Eager);
            IList tempLst = appSrv.GetDomainByCondition(typeof(OperationJobWithRole), oq);
            IList retLst = new ArrayList();
            foreach (OperationJobWithRole obj in tempLst)
            {
                retLst.Add(obj.OperationRole);
            }
            return retLst;
        }

        private IList GetOperationJobByRoleId(string roleId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OperationRole.Id", roleId));
            oq.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            IList tempLst = appSrv.GetDomainByCondition(typeof(OperationJobWithRole), oq);
            IList retLst = new ArrayList();
            foreach (OperationJobWithRole obj in tempLst)
            {
                retLst.Add(obj.OperationJob);
            }
            return retLst;
        }

        private IList FilterData(IList lstMaster, AppTableSet theAppTableSet)
        {
            IList retLst = new ArrayList();
            if (lstMaster == null || lstMaster.Count == 0) return retLst;

            //获取当前单据定义的审批方案
            IList lstAppSolution = appSrv.GetAppSolution(theAppTableSet);
            if (lstAppSolution == null || lstAppSolution.Count == 0) return retLst;

            #region 判断审批方案配置是否完整
            IList lstSuitedAppSolution = new ArrayList();
            foreach (AppSolutionSet appSolutionSet in lstAppSolution)
            {
                if (appSolutionSet.AppStepsSets == null || appSolutionSet.AppStepsSets.Count == 0)
                {
                    continue;
                }
                bool roleConfiged = false;//是否配置审批角色
                foreach (AppStepsSet appStepsSet in appSolutionSet.AppStepsSets)
                {
                    if (appStepsSet.AppRoleSets.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        roleConfiged = true;
                        break;
                    }
                }
                if (roleConfiged)
                {
                    lstSuitedAppSolution.Add(appSolutionSet);
                }
            }
            if (lstSuitedAppSolution.Count == 0)
            {
                //审批方案配置不完整 直接返回
                return retLst;
            }
            #endregion


            foreach (object obj in lstMaster)
            {
                string Id = ClientUtil.ToString(obj.GetType().GetProperty("Id").GetValue(obj, null));
                //判断这条数据是否有符合的审批方案
                IList tempAppSolutionSetLst = GetSuitedAppSolutionSet(theAppTableSet, Id, lstSuitedAppSolution);
                if (tempAppSolutionSetLst == null || tempAppSolutionSetLst.Count == 0)
                {
                    //没有合适的审批方案，这条数据不显示
                    continue;
                }
                AppSolutionSet currentAppSolutionSet = tempAppSolutionSetLst[0] as AppSolutionSet;
                if (!SuitedRole(theAppTableSet, obj, currentAppSolutionSet))
                {
                    continue;
                }
                retLst.Add(obj);
            }

            return retLst;
        }

        private bool SuitedRole(AppTableSet theAppTableSet, object objBillInfo, AppSolutionSet currentAppSolutionSet)
        {
            bool result = false;
            int CurrStepOrder = 1;
            int ReadyStepOrder = 1;
            AppStepsSet ReadyAppStepsSet = null;

            string BillId = ClientUtil.ToString(objBillInfo.GetType().GetProperty("Id").GetValue(objBillInfo, null));
            string opgSysCode = objBillInfo.GetType().GetProperty("OpgSysCode").GetValue(objBillInfo, null) + "";
            //ConstObject.TheOperationOrg

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);

            //当前单据的审批信息
            list_AppStepsInfo = appSrv.GetAppStepsInfo(oq);

            /* 1：如果存在审批信息，然后确定待审步骤的岗位和当前登录人岗位对比
             * (1)如果能匹配到则显示审批步骤的缺省信息和之前审批通过的信息
             * (2)如果不匹配到则不显示任何信息
             * 
             * 2：如果不存审批信息，说明待审步骤是第一步
             * (1)如果当前登录人的岗位和第一步审批岗位匹配就显示该审批的缺省信息
             * (2)如果不匹配到则不显示任何信息
             */

            if (list_AppStepsInfo.Count > 0)
            {

                //确定待审批的步骤
                foreach (AppStepsInfo StepsInfo in list_AppStepsInfo)
                {
                    if (StepsInfo.StepOrder > CurrStepOrder)
                    {
                        CurrStepOrder = (int)StepsInfo.StepOrder;
                    }
                }
                ReadyStepOrder = CurrStepOrder;//待审步骤
                foreach (AppStepsSet item in currentAppSolutionSet.AppStepsSets)
                {
                    if (item.StepOrder < CurrStepOrder) continue;
                    if (item.StepOrder == CurrStepOrder)
                    {
                        if (item.AppRelations == 0)
                        {
                            //或关系 跳到下一步骤
                            ReadyStepOrder = ReadyStepOrder + 1;
                            break;
                        }
                        else
                        {
                            //与关系
                            if (AllRolePassed(list_AppStepsInfo, item))
                            {
                                //所有与关系角色已经审批 跳到下一步骤
                                ReadyStepOrder = ReadyStepOrder + 1;
                                break;
                            }
                            else
                            {
                                //还是执行当前步骤
                                ReadyStepOrder = CurrStepOrder;
                                break;
                            }
                        }
                    }
                }
                ReadyAppStepsSet = GetAppStepsSet(ReadyStepOrder, currentAppSolutionSet.AppStepsSets.ToList());

                if (ReadyAppStepsSet != null)
                {
                    bool SFPP = false;//是否匹配标志

                    List<OperationRole> appRoleLst = new List<OperationRole>();
                    foreach (AppRoleSet RoleSet in ReadyAppStepsSet.AppRoleSets)
                    {
                        //或关系把审批步骤的所角色作对比，与关系则排除已经作了审批的角色
                        if (ReadyAppStepsSet.AppRelations == 0)
                        {
                            appRoleLst.Add(RoleSet.AppRole);
                        }
                        else
                        {
                            bool hasAudit = false;
                            foreach (AppStepsInfo stepInfo in list_AppStepsInfo)
                            {
                                if (stepInfo.AppRole.Id == RoleSet.AppRole.Id)
                                {
                                    hasAudit = true;
                                    break;
                                }
                            }
                            if (hasAudit == false)
                            {
                                appRoleLst.Add(RoleSet.AppRole);
                            }
                        }
                        //
                    }
                    SFPP = CheckRoleAndOpg(opgSysCode, appRoleLst);
                    //待审批步骤的定义的岗位和当前登录人的岗位匹配
                    if (SFPP == true)
                    {
                        BillIdAppStepsSetHash.Add(BillId, ReadyAppStepsSet);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (currentAppSolutionSet != null)
                {
                    //确定待审步骤(第一步)
                    foreach (AppStepsSet item in currentAppSolutionSet.AppStepsSets)
                    {
                        //待审批步骤
                        if (item.StepOrder == 1)
                        {
                            ReadyAppStepsSet = item;
                        }
                    }
                    if (ReadyAppStepsSet != null)
                    {
                        bool SFPP = false;//是否匹配标志
                        List<OperationRole> appRoleLst = new List<OperationRole>();
                        foreach (AppRoleSet RoleSet in ReadyAppStepsSet.AppRoleSets)
                        {
                            appRoleLst.Add(RoleSet.AppRole);
                        }
                        SFPP = CheckRoleAndOpg(opgSysCode, appRoleLst);
                        //待审批步骤的定义的岗位和当前登录人的岗位匹配
                        if (SFPP == true)
                        {
                            BillIdAppStepsSetHash.Add(BillId, ReadyAppStepsSet);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return result;
        }

        private bool CheckRoleAndOpg(string billOpgSysCode, List<OperationRole> appRoleLst)
        {
            //ConstObject..TheOperationOrg

            //当前登录用户的角色
            IList userRolelst = GetOperationRoleByJobId(ConstObject.TheSysRole.Id);
            //一、判断本部门 角色、组织是否相同
            if (ConstObject.TheOperationOrg.SysCode == billOpgSysCode && CheckRole(appRoleLst, userRolelst))
            {
                return true;
            }
            //二、最近上级部门和兄弟部门
            if (CheckOpg(billOpgSysCode, userRolelst, appRoleLst))
            {
                return true;
            }

            //三、审批角色只有对应一个岗位时 如果与登录岗位匹配则返回（无的情况）
            if (appRoleLst.Count > 0)
            {
                OperationRole appRole = appRoleLst[0] as OperationRole;
                IList appJobLst = GetOperationJobByRoleId(appRole.Id);
                if (appJobLst != null && appJobLst.Count > 0)
                {
                    if ((appJobLst[0] as OperationJob).Id == ConstObject.TheSysRole.Id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckOpg(string billOpgSysCode, IList userRolelst, List<OperationRole> appRoleLst)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            string[] opgIds = billOpgSysCode.Split('.');
            if (opgIds == null || opgIds.Length == 0) return false;

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
            if (opgIds.Length > 2)
            {
                dis.Add(Expression.Eq("ParentNode.Id", opgIds[opgIds.Length - 3]));
            }

            oq.AddCriterion(dis);
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            //查询单据对应业务部门的所有上层同级业务部门
            IList lstOpg = appSrv.GetDomainByCondition(typeof(OperationOrg), oq);
            //最近上级部门的情况
            for (int j = parentSysCodes.Count - 1; j >= 0; j--)
            {
                string parentSysCode = parentSysCodes[j];
                if (parentSysCode == billOpgSysCode) continue;
                if (ConstObject.TheOperationOrg.SysCode == parentSysCode && CheckRole(appRoleLst, userRolelst))
                {
                    return true;
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
                        if (ConstObject.TheOperationOrg.SysCode == org.SysCode && CheckRole(appRoleLst, userRolelst))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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

        private bool CheckRole(List<OperationRole> appRoleLst, IList userRolelst)
        {
            foreach (OperationRole appRole in appRoleLst)
            {
                foreach (OperationRole userRole in userRolelst)
                {
                    if (appRole.Id == userRole.Id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 根据审批顺序确定
        /// </summary>
        /// <param name="stepOrder"></param>
        /// <param name="AppStepsSets"></param>
        /// <returns></returns>
        private AppStepsSet GetAppStepsSet(int stepOrder, IList AppStepsSets)
        {
            AppStepsSet appStepsSet = null;
            foreach (AppStepsSet item in AppStepsSets)
            {
                if (item.StepOrder == stepOrder)
                {
                    appStepsSet = item;
                    break;
                }
            }
            return appStepsSet;
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

        private List<AppSolutionSet> GetSuitedAppSolutionSet(AppTableSet theAppTableSet, string masterId, IList lstSuitedAppSolution)
        {
            List<AppSolutionSet> ret = new List<AppSolutionSet>();
            foreach (AppSolutionSet appSolutionSet in lstSuitedAppSolution)
            {
                if (appSolutionSet.Conditions == null || appSolutionSet.Conditions.Trim().Equals(""))
                {
                    //执行条件为空时，直接返回当前这个解决方案
                    ret.Add(appSolutionSet);
                    break;
                }

                if (SuitedCondition(theAppTableSet, masterId, appSolutionSet))
                {
                    ret.Add(appSolutionSet);
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// 有执行条件时，判断这个数据是否符合条件
        /// </summary>
        /// <param name="theAppTableSet"></param>
        /// <param name="masterId"></param>
        /// <param name="appSolutionSet"></param>
        /// <returns></returns>
        private bool SuitedCondition(AppTableSet theAppTableSet, string masterId, AppSolutionSet appSolutionSet)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", masterId));
            oq.AddCriterion(Expression.Sql(appSolutionSet.Conditions));
            IList lst = null;
            try
            {
                lst = appSrv.GetDomainByCondition(theAppTableSet.MasterNameSpace, oq);
            }
            catch (Exception ex)
            {

            }
            if (lst != null && lst.Count > 0)
            {
                return true;
            }

            return false;
        }

        #endregion

        void authGo1_AuthGo_Menu(AuthManagerLib.AuthMng.MenusMng.Domain.Menus aMenus)
        {
            try
            {
                aMenus.Exe();
            }
            catch { }
            //此处需要进行扩展（权限下一步增加一个属性来进行标识是通用查询,现在先以代码为Search开头的为标识）
            if (aMenus.Code != null && aMenus.Code.StartsWith("Search"))
            {
                //new CommonSearch.CommonSearchMng.ClassCommonSearchMng.VClassCommonSearchCon(aMenus.Name, ConstObject.TheLogin.ThePerson.Code).ShowDialog();
            }
            else
            {
                UCL.Locate(aMenus.Name);
            }
        }
    }
}