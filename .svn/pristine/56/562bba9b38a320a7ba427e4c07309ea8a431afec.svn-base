using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain;
using Application.Business.Erp.SupplyChain.Client.MobileManage;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng
{
    public partial class VAppOperation : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        private MAppPlatMng Model = new MAppPlatMng();
        private IList list_AppSolution = null; 
        IList ListMasterDatalist = new ArrayList();
        AppTableSet theAppTableSet = new AppTableSet();//接受传过来的变量
        /// 当前单据对应的审批步骤
        private Hashtable AppStepsSetHash = new Hashtable();
        //AppTableSet currATSet = new AppTableSet();
        private OperationRole currentRole = null;
        IList list_AppStepsInfo = null;
        string Id = null;
        int Index = 0;
        IList currMaster = new ArrayList();
        AppMasterData theAppMasterData = new AppMasterData();
        IList DetailData = new ArrayList();
        public VAppOperation(string id, Hashtable BillIdAppStepsSetHash, AppTableSet appTSet, IList list, int PageIndex, IList ListMasterDate,IList lisDateilDatalist)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            Index = PageIndex;
            currMaster=list;
            theAppTableSet = appTSet;
            Id = id;
            AppStepsSetHash = BillIdAppStepsSetHash;
            ListMasterDatalist = ListMasterDate;
            DetailData = lisDateilDatalist;
            InitEvents();   
        }
        public void InitEvents()
        {
            btnAppAgree.Click += new EventHandler(btnAppAgree_Click);
            BtnDisagree.Click += new EventHandler(BtnDisagree_Click);
            ShowAppSetpSet(Id, AppStepsSetHash);
        }

        void BtnDisagree_Click(object sender, EventArgs e)
        {
            IList AppMasterDataList = new ArrayList();
            IList AppDetailDataList = new ArrayList();

            IList AppMasterDataModify = new ArrayList();
            IList AppDetailDataModify = new ArrayList();

            #region 待审批的步骤的缺省信息
            AppStepsSet ReadyAppStepsSet = AppStepsSetHash[Id] as AppStepsSet;
            currentRole = CurrentAppRole(ReadyAppStepsSet,Id);

            AppStepsInfo theAppStepsInfo = new AppStepsInfo();
            theAppStepsInfo.StepOrder = ReadyAppStepsSet.StepOrder;
            theAppStepsInfo.StepsName = ReadyAppStepsSet.StepsName;
            theAppStepsInfo.AppRelations = ReadyAppStepsSet.AppRelations;
            theAppStepsInfo.AppTableSet = ReadyAppStepsSet.AppTableSet;
            theAppStepsInfo.AppRole = currentRole;
            theAppStepsInfo.RoleName = currentRole.RoleName;
            theAppStepsInfo.AppStepsSet = ReadyAppStepsSet;
            #endregion

            AppStepsInfo currentSteps = theAppStepsInfo;
            AppDetailDataList = DetailData;
            //取主表
            AppMasterDataList = ListMasterDatalist;

            AppTableSet tableSet = theAppTableSet;

            string BillId = Id;
            Model.Service.AppDisAgree(tableSet, currentSteps, textBox1.Text, BillId, AppMasterDataList, AppDetailDataList, AppMasterDataModify, AppDetailDataModify,null);

            //MessageBox.Show("审批完成！");
            VMessageBox v_show = new VMessageBox();
            v_show.txtInformation.Text = "审批完成！";
            v_show.ShowDialog();
            Clear();


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
        void btnAppAgree_Click(object sender, EventArgs e)
        {
            IList AppMasterDataList = new ArrayList();
            IList AppDetailDataList = new ArrayList();
            IList AppMasterDataModify = new ArrayList();
            IList AppDetailDataModify = new ArrayList();
            
            //获取当前单据定义的审批方案和审批步骤
            list_AppSolution = Model.Service.GetAppSolution(theAppTableSet);
            //判断当前审批步骤是否是最后一步
            bool IsFinish = true;
            AppSolutionSet theAppSolutionSet = list_AppSolution[0] as AppSolutionSet;
            
                //获取审批的最后一步
                long MaxStepOrder = 1;
                foreach (AppStepsSet item in theAppSolutionSet.AppStepsSets)
                {
                    if (item.StepOrder >= MaxStepOrder)
                    {
                        MaxStepOrder = item.StepOrder;
                    }
                }
                #region 待审批的步骤的缺省信息
                AppStepsSet ReadyAppStepsSet = AppStepsSetHash[Id] as AppStepsSet;
                currentRole = CurrentAppRole(ReadyAppStepsSet,Id);

                AppStepsInfo theAppStepsInfo = new AppStepsInfo();
                theAppStepsInfo.StepOrder = ReadyAppStepsSet.StepOrder;
                theAppStepsInfo.StepsName = ReadyAppStepsSet.StepsName;
                theAppStepsInfo.AppRelations = ReadyAppStepsSet.AppRelations;
                theAppStepsInfo.AppTableSet = ReadyAppStepsSet.AppTableSet;
                theAppStepsInfo.AppRole = currentRole;
                theAppStepsInfo.RoleName = currentRole.RoleName;
                theAppStepsInfo.AppStepsSet = ReadyAppStepsSet;
                #endregion
                if (theAppStepsInfo == null) return;

             AppStepsInfo currentSteps = theAppStepsInfo;
            if (currentSteps == null) return;
            string BillId = Id;
            currentRole = CurrentAppRole(currentSteps.AppStepsSet,Id);
            currentSteps.AppRole = currentRole;
            currentSteps.RoleName = currentRole.RoleName;
            currentSteps.BillId = Id;
            if (currentSteps.AppRelations == 0)
            {
                //或关系
                if (currentSteps.StepOrder >= MaxStepOrder)
                {
                    IsFinish = true;
                }
                else
                {
                    IsFinish = false;
                }
            }
            else if (currentSteps.AppRelations == 1)
            {
                //与关系 必须是所有角色审批完成后才能代表完成
                if (AllRolePassed(currentSteps))
                {
                    if (currentSteps.StepOrder >= MaxStepOrder)
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
            //AppMasterDataList = GetBillMasterMess();
            //AppDetailDataList = GetBillDetailMess();
            AppDetailDataList = DetailData;
            AppMasterDataList = ListMasterDatalist;

            //DataGridViewRow thdCurrentRow = dgBill.SelectedRows[0];
            AppTableSet tableSet = theAppTableSet;

            //Model.Service.AppAgree(tableSet, currentSteps, textBox1.Text, BillId, IsFinish, AppMasterDataList, AppDetailDataList, AppMasterDataModify, AppDetailDataModify);
           // MessageBox.Show();
            VMessageBox v_show = new VMessageBox();
            v_show.txtInformation.Text = "审批完成！";
            v_show.ShowDialog();
            Clear();
        }
       

        //获取审批步骤
        void ShowAppSetpSet(string BillId, Hashtable AppStepsSetHash)
        {
            #region 待审批的步骤的缺省信息
            
            AppStepsSet ReadyAppStepsSet = AppStepsSetHash[BillId] as AppStepsSet;
            OperationRole currentRole = CurrentAppRole(ReadyAppStepsSet,Id);

            if (currentRole == null)
            {
                btnAppAgree.Enabled = false;
                BtnDisagree.Enabled = false;
                //MessageBox.Show();
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "当前步骤的审批已完成！";
                v_show.ShowDialog();
            }
            else
            {
                AppStepsInfo theAppStepsInfo = new AppStepsInfo();
                theAppStepsInfo.StepOrder = ReadyAppStepsSet.StepOrder;
                theAppStepsInfo.StepsName = ReadyAppStepsSet.StepsName;
                theAppStepsInfo.AppRelations = ReadyAppStepsSet.AppRelations;
                theAppStepsInfo.AppTableSet = ReadyAppStepsSet.AppTableSet;
                theAppStepsInfo.AppRole = currentRole;
                theAppStepsInfo.RoleName = currentRole.RoleName;
                theAppStepsInfo.AppStepsSet = ReadyAppStepsSet;
           
            #endregion
            
            IList userRolelst = GetOperationRoleByJobId(ConstObject.TheSysRole.Id);

            foreach (OperationRole role in userRolelst)
            {
                if (theAppStepsInfo.AppRole.Id == role.Id)
                {
                    btnAppAgree.Enabled = true;
                    BtnDisagree.Enabled = true;
                    return;
                }
                else
                {
                    btnAppAgree.Enabled = false;
                    BtnDisagree.Enabled = false;
                }
            }
        }
        }
        private void Clear()
        
       {
           textBox1.Text = "";
            btnAppAgree.Enabled = false;
            BtnDisagree.Enabled = false;
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
       /// <summary>
       /// 当前审批的角色
       /// </summary>
       /// <param name="appStepsSet"></param>
       /// <returns></returns>
       private OperationRole CurrentAppRole(AppStepsSet appStepsSet,string BillId)
       {
           //当前登录用户的角色
           IList userRolelst = GetOperationRoleByJobId(ConstObject.TheSysRole.Id);
           foreach (OperationRole role in userRolelst)
           {

               ObjectQuery oq = new ObjectQuery();
               oq.AddCriterion(Expression.Eq("BillId", BillId));
               oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
               oq.AddCriterion(Expression.Eq("State", 1));
               oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
               oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);
               //当前单据的审批信息
               list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);
               bool audited = false;
               if (list_AppStepsInfo.Count> 0)
               {
               
                   foreach (AppStepsInfo master in list_AppStepsInfo)
                   {
                       if (master.AppRole.Id == role.Id)
                       {
                           audited = true;
                           break;
                       }
                       else
                       {
                           continue;
                       }
                   }
               }
               if (audited) continue;
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
           oq.AddFetchMode("AppStepsSet", NHibernate.FetchMode.Eager);

           //当前单据的审批信息
           list_AppStepsInfo = Model.Service.GetAppStepsInfo(oq);
           if (list_AppStepsInfo == null)
           {
               list_AppStepsInfo = new ArrayList();
           }
           list_AppStepsInfo.Add(currentSteps);

           return AllRolePassed(list_AppStepsInfo, currentSteps.AppStepsSet);
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

       private void VAppOperation_Load(object sender, EventArgs e)
       {
           this.WindowState = FormWindowState.Maximized;
       }
    }
}
