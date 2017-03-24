using System;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using C1.Win.C1FlexGrid;
using System.Collections;
using Application.Resource.BasicData.Domain;
//using Application.Business.Erp.SupplyChain.CostingMng.InitData.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Resource.CommonClass.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI;
using Application.Business.Erp.Secure.GlobalInfo;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Drawing;
using System.Collections.Generic;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.Linq;
using System.Reflection;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.MobileManage;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng
{
    public partial class VAppPlatMng : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        private IList List = new ArrayList();
        private MAppPlatMng Model = new MAppPlatMng();
        private AppTableSet currATset = null;
        IList ListDetail = new ArrayList();
        string Id = null;
        private int pageIndex = 0;
        int pageCount = 0;
        IList ListMaster = new ArrayList();
        IList listMasterDate = new ArrayList();
        IEnumerable<object> listDtl = null;
        /// <summary>
        /// 需要审批的单据
        /// </summary>
        private Hashtable neededAuditBillHash = new Hashtable();
        /// <summary>
        /// 当前单据需要的审批步骤
        /// </summary>
        private Hashtable BillIdAppStepsSetHash = new Hashtable();
        private IList list_AppSolution = null;
        IList list_AppStepsInfo = null;
        public IList list11 = new ArrayList();
        public VAppPlatMng()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitEvents();
            InitData();
            //btnBack.Enabled = false;
            //if (pageCount <= 1)
            //{
            //    btnNext.Enabled = false;
            //}
            if (pageCount <= 0)
            {
                btnApprove.Enabled = false;
                btnAlready.Enabled = false;
                btnDetail.Enabled = false;
            }
        }

        public void InitEvents()
        {
            btnNext.Click += new EventHandler(btnNext_Click);
            btnDetail.Click += new EventHandler(btnDetail_Click);
            btnAlready.Click += new EventHandler(btnAlready_Click);
            btnApprove.Click += new EventHandler(btnApprove_Click);
            btnBack.Click += new EventHandler(btnBack_Click);
            //this.Load += new EventHandler(VAppPlatMng_Load);
        }

        //void VAppPlatMng_Load(object sender, EventArgs e)
        //{
            
        //}
        void InitData()
        {
            QueryData();
            //dtpDateBegin.Value = LoginInfomation.LoginInfo.LoginDate.AddMonths(-1);
            //DateEnd.Value = LoginInfomation.LoginInfo.LoginDate;
            LoginInfomation.LoginInfo = AppDomain.CurrentDomain.GetData("TheLogin") as Login;
            //this.label1.Text = LoginInfomation.LoginInfo.TheSysRole.RoleName;
        }
        //上一项信息
        void btnBack_Click(object sender, EventArgs e)
        {
            pageIndex -= 1;
            if (pageIndex == 0)
            {
                btnBack.Enabled = false;
            }
            else
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }
            label5.Text = "第【" + (pageIndex+1) + "】条";
            label6.Text = "共【" + pageCount + "】条";
            object obj = ListMaster[pageIndex] as object;
            Id = ClientUtil.ToString(obj.GetType().GetProperty("Id").GetValue(obj, null));
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
                    dgBillCreateDate = ClientUtil.ToString(pi.GetValue(obj, null));
                }
            }

            //this.txtName.Text = ClientUtil.ToString(master.TableName);
            this.txtCode.Text = dgBillCode;
            this.txtAdmin.Text = dgBillCreatePerson;
            this.dtpDateBegin.Value = Convert.ToDateTime(dgBillCreateDate);
        }
        //下一项信息
        void btnNext_Click(object sender, EventArgs e)
        {

            pageIndex += 1;

            if (pageIndex == pageCount - 1)
            {
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
                btnBack.Enabled = true;
            }
            if (pageIndex + 1 <= pageCount)
            {
                label5.Text = "第【" + (pageIndex + 1) + "】条";
                label6.Text = "共【" + pageCount + "】条";
                object obj = ListMaster[pageIndex] as object;
                Id = ClientUtil.ToString(obj.GetType().GetProperty("Id").GetValue(obj, null));
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
                        dgBillCreateDate = ClientUtil.ToString(pi.GetValue(obj, null));
                    }
                }

                //this.txtName.Text = ClientUtil.ToString(master.TableName);
                this.txtCode.Text = dgBillCode;
                this.txtAdmin.Text = dgBillCreatePerson;
                this.dtpDateBegin.Value = Convert.ToDateTime(dgBillCreateDate);
            }
        }
        //获取主表数据，MasterData类型数据
        private IList GetAppMasterData()
        {
            listMasterDate = new ArrayList();
            AppMasterData theAppMasterData = new AppMasterData();

            theAppMasterData = new AppMasterData();
            theAppMasterData.PropertyName = "ProjectName";
            theAppMasterData.PropertyValue = txtName.Text;
            theAppMasterData.BillId = Id;
            theAppMasterData.PropertyChineseName = "项目名称";
            theAppMasterData.PropertySerialNumber = 0;
            theAppMasterData.AppDate = DateTime.Now;
            theAppMasterData.AppStatus = 2L;
            theAppMasterData.AppTableSet = currATset.Id;
            listMasterDate.Add(theAppMasterData);

            theAppMasterData = new AppMasterData();
            theAppMasterData.PropertyName = "Code";
            theAppMasterData.PropertyValue = txtCode.Text;
            theAppMasterData.BillId = Id;
            theAppMasterData.PropertyChineseName = "单据号";
            theAppMasterData.PropertySerialNumber = 1;
            theAppMasterData.AppDate = DateTime.Now;
            theAppMasterData.AppStatus = 2L;
            theAppMasterData.AppTableSet = currATset.Id;
            listMasterDate.Add(theAppMasterData);

            theAppMasterData = new AppMasterData();
            theAppMasterData.PropertyName = "TaskHandleName";
            theAppMasterData.PropertyValue = Id;
            theAppMasterData.BillId = Id;
            theAppMasterData.PropertyChineseName = "分承包方";
            theAppMasterData.PropertySerialNumber = 2;
            theAppMasterData.AppDate = DateTime.Now;
            theAppMasterData.AppStatus = 2L;
            theAppMasterData.AppTableSet = currATset.Id;
            listMasterDate.Add(theAppMasterData);

            theAppMasterData.PropertyName = "ConfirmDate";
            theAppMasterData.PropertyValue = dtpDateBegin.Value.ToShortDateString();
            theAppMasterData.BillId = Id;
            theAppMasterData.PropertyChineseName = "制单日期";
            theAppMasterData.PropertySerialNumber = 3;
            theAppMasterData.AppDate = DateTime.Now;
            theAppMasterData.AppStatus = 2L;
            theAppMasterData.AppTableSet = currATset.Id;
            listMasterDate.Add(theAppMasterData);

            theAppMasterData = new AppMasterData();
            theAppMasterData.PropertyName = "ConfirmHandlePersonName";
            theAppMasterData.PropertyValue = txtAdmin.Text;
            theAppMasterData.BillId = Id;
            theAppMasterData.PropertyChineseName = "确认人";
            theAppMasterData.PropertySerialNumber = 4;
            theAppMasterData.AppDate = DateTime.Now;
            theAppMasterData.AppStatus = 2L;
            theAppMasterData.AppTableSet = currATset.Id;
            listMasterDate.Add(theAppMasterData);

            return listMasterDate;
        }

        //显示操作界面
        void btnApprove_Click(object sender, EventArgs e)
        {
            IList List = new ArrayList();
            List.Add(ListMaster[pageIndex]);
            GetAppMasterData();
            //AppTableSet obj1 = ListMaster[PageIndex] as AppTableSet;
            VAppOperation vo = new VAppOperation(Id, BillIdAppStepsSetHash, currATset, List, pageIndex, listMasterDate,list11);
            vo.ShowDialog();
        }
        //显示已审界面
        void btnAlready_Click(object sender, EventArgs e)
        {
            VAppAlready va = new VAppAlready(Id, currATset);
            va.ShowDialog();
        }
        //显示详细界面
        void btnDetail_Click(object sender, EventArgs e)
        {
            if (pageIndex >= ListMaster.Count)
            {
                pageIndex = pageIndex - 1;
            }
            object obj1 = ListMaster[pageIndex] as object;
            VAppDetail vwpd = new VAppDetail(obj1, currATset);
            vwpd.ShowDialog();

            list11 = vwpd.listDetailDataList;
            
        }
        private void QueryData()
        {
            IList appTableSetLst = CurrentUserAppTableSet();
            BillIdAppStepsSetHash.Clear();
            //循环遍历解决方案，注意：不是只有一种解决方案，有不同的单据类型
            foreach (AppTableSet obj in appTableSetLst)
            {
                Query(obj);
            }

        }
        private void Query(AppTableSet master)
        {
            IList ListMasterTemp = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.InAudit));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

            //listMasterTemp注意，不要定义成全局的，会被过滤
            ListMasterTemp = Model.GetDomainByCondition(master.MasterNameSpace, oq);
            ShowDate(master,ListMasterTemp);
          
        }
        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);
            this.FindForm().Close();
        }
        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            base.TBtnPageDown_Click(sender, e);
            VMessageBox v_show = new VMessageBox();
            v_show.txtInformation.Text = "当前是最后一步！";
            v_show.ShowDialog();
            return;
        }
        private void ShowDate(AppTableSet master,IList list)
        {
            IList  ListMasterTemp = list;
            ListMasterTemp = FilterData(ListMasterTemp, master);

            //btnBack.Enabled = false;

            if (ListMasterTemp == null || ListMasterTemp.Count == 0)
            {
                label5.Text = "第【" + (pageIndex + 1) + "】条";
                label6.Text = "共【" + pageCount + "】条";
                btnBack.Visible = false;
                btnNext.Visible = false;
                return;
            }
            foreach (object o in ListMasterTemp)
            {
                ListMaster.Add(o);
            }

            IList masterData = new ArrayList();
            IList detailData = new ArrayList();
            //IList List_DetailProperty = Model.GetAppDetailProperties(master.Id);
            listDtl = ListMaster.OfType<object>();
            pageCount = listDtl.Count();
            if (pageCount <= 1)
            {
                btnBack.Visible = false;
                btnNext.Visible = false;
            }
            label5.Text = "第【" + (pageIndex + 1) + "】条";
            label6.Text = "共【" + pageCount + "】条";
            if (pageCount > 0)
            {
                object obj = ListMaster[pageIndex] as object;
                //int rowIndex = dgMaster.Rows.Add();
                //masterData = new ArrayList();
                Id = ClientUtil.ToString(obj.GetType().GetProperty("Id").GetValue(obj, null));

                //把当前单据放到hashtable中
                if (!neededAuditBillHash.ContainsKey(Id))
                {
                    neededAuditBillHash.Add(Id, obj);
                }
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
                        dgBillCreateDate = ClientUtil.ToString(pi.GetValue(obj, null));
                    }
                }
                this.txtName.Text = ClientUtil.ToString(master.TableName);
                this.txtCode.Text = dgBillCode;
                //this.txtCode.Tag = Id;
                currATset = master;
                this.txtAdmin.Text = dgBillCreatePerson;
                this.dtpDateBegin.Value = Convert.ToDateTime(dgBillCreateDate);
            }
        }
        
        void ShowAppSetpSet(AppTableSet theAppTableSet, string BillId)
        {
            //FgAppSetpsInfo.Rows.Clear();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);

            //当前单据的审批信息
            list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);
            //获取当前单据定义的审批方案和审批步骤
            list_AppSolution = Model.Service.GetAppSolution(theAppTableSet);

            #region 已审批通过的审批步骤信息

            #endregion

            #region 待审批的步骤的缺省信息

            //int rowIndex = FgAppSetpsInfo.Rows.Add();
            AppStepsSet ReadyAppStepsSet = BillIdAppStepsSetHash[BillId] as AppStepsSet;
            OperationRole currentRole = CurrentAppRole(ReadyAppStepsSet);

            AppStepsInfo theAppStepsInfo = new AppStepsInfo();
            theAppStepsInfo.StepOrder = ReadyAppStepsSet.StepOrder;
            theAppStepsInfo.StepsName = ReadyAppStepsSet.StepsName;
            theAppStepsInfo.AppRelations = ReadyAppStepsSet.AppRelations;
            theAppStepsInfo.AppTableSet = ReadyAppStepsSet.AppTableSet;
            theAppStepsInfo.AppRole = currentRole;
            theAppStepsInfo.RoleName = currentRole.RoleName;
            theAppStepsInfo.AppStepsSet = ReadyAppStepsSet;
            #endregion
            //FgAppSetpsInfo_SelectionChanged(null, new EventArgs());
        }

        //当前登录的角色
        private OperationRole CurrentAppRole(AppStepsSet appStepsSet)
        {
            //当前登录用户的角色
            IList userRolelst = GetOperationRoleByJobId(ConstObject.TheSysRole.Id);
            foreach (OperationRole role in userRolelst)
            {
                foreach (AppRoleSet appRoleSet in appStepsSet.AppRoleSets)
                {
                    if (role.Id == appRoleSet.AppRole.Id)
                    {
                        return role;
                    }
                }
            }
            return null;
        }
        private IList GetOperationRoleByJobId(string jobId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OperationJob.Id", jobId));
            oq.AddFetchMode("OperationRole", NHibernate.FetchMode.Eager);
            IList tempLst = Model.Service.GetDomainByCondition(typeof(OperationJobWithRole), oq);
            IList retLst = new ArrayList();
            foreach (OperationJobWithRole obj in tempLst)
            {
                retLst.Add(obj.OperationRole);
            }
            return retLst;
        }
        #region 新
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
            IList lst = Model.Service.CurrentUserTableSet(sql);
            return lst;
        }

        public override void ViewShow()
        {
            base.ViewShow();

            QueryData();

        }
        #endregion
        private IList FilterData(IList lstMaster, AppTableSet theAppTableSet)
        {
            IList retLst = new ArrayList();
            if (lstMaster == null || lstMaster.Count == 0) return retLst;

            //获取当前单据定义的审批方案
            IList lstAppSolution = Model.Service.GetAppSolution(theAppTableSet);
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
            list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);

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
                tempId = tempId + id + ".";
                parentSysCodes.Add(tempId);
                dis.Add(Expression.Eq("SysCode", tempId));
            }
            if (opgIds.Length > 1)
            {
                dis.Add(Expression.Eq("ParentNode.Id", opgIds[opgIds.Length - 2]));
            }
            oq.AddCriterion(dis);
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            //查询单据对应业务部门的所有上层同级业务部门
            IList lstOpg = Model.Service.GetDomainByCondition(typeof(OperationOrg), oq);
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
        private IList GetOperationJobByRoleId(string roleId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OperationRole.Id", roleId));
            oq.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            IList tempLst = Model.Service.GetDomainByCondition(typeof(OperationJobWithRole), oq);
            IList retLst = new ArrayList();
            foreach (OperationJobWithRole obj in tempLst)
            {
                retLst.Add(obj.OperationJob);
            }
            return retLst;
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
                lst = Model.Service.GetDomainByCondition(theAppTableSet.MasterNameSpace, oq);
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

        private void VAppPlatMng_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void txtAdmin_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
