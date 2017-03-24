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
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.Windows.Documents;
using Application.Business.Erp.SupplyChain.Client.MobileManage;


namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng
{

    public partial class VAppAlready : TBasicToolBarByMobile
    {

        object currMaster = new object();
        AppTableSet FgStep = new AppTableSet();//接受传过来的变量
        AppStepsInfo appsteps = new AppStepsInfo();

        public AppTableSet OptAppTable = null;// 定义一个全局变量

        private AutomaticSize automaticSize = new AutomaticSize();
        IEnumerable<object> listDtl = null;
        private MAppPlatMng Model = new MAppPlatMng();
        private IList list= null;
        string Id = null;
        int pageIndex = 0;
        int pageCount = 0;
        public VAppAlready(string id, AppTableSet appTSet)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            //currMaster = master;
            FgStep = appTSet;
            Id = id;
            InitEvent();
        }
        private void InitEvent()
        {
            ShowAppSetpSet(FgStep,Id);
            btnBack.Click += new EventHandler(btnBack_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            pageIndex += 1;

            if (pageIndex == pageCount - 1)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
                btnBack.Enabled = true;
            }
            AppStepsInfo list1 = list[pageIndex] as AppStepsInfo;

            label8.Text = "第【" + (pageIndex + 1) + "】条";
            label9.Text = "共【" + pageCount + "】条";
                    txtName.Text =list1.StepsName;//审批步骤名称

                    if (list1.AppRelations == 0)
                    {
                        txtRetion.Text = "或";
                    }
                    else
                    {
                        txtRetion.Text = "与";
                    }
                    txtRole.Text = list1.RoleName;//角色
                    //状态
                    switch (list1.AppStatus)
                    {
                        case -1:
                            txtState.Text = "已撤单";
                            break;
                        case 0:
                            txtState.Text = "审批中";
                            break;
                        case 1:
                            txtState.Text = "未通过";
                            break;
                        case 2:
                            txtState.Text = "已通过";
                            break;
                        default:
                            txtState.Text = "未审批！";
                            break;
                    }
                    txtMen.Text = ClientUtil.ToString((list1.AuditPerson as PersonInfo).Name);//角色名称
                    txtIdea.Text = list1.AppComments;//意见
                    dtpDate.Value = list1.AppDate;// 时间
                //}
           

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
        void btnBack_Click(object sender, EventArgs e)
        {
            pageIndex -= 1;
            if (pageIndex == 0)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }
            else
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }
            AppStepsInfo list1 = list[pageIndex] as AppStepsInfo;
            label8.Text = "第【" + (pageIndex + 1) + "】条";
            label9.Text = "共【" + pageCount + "】条";
            if (list != null && list.Count > 0)
            {
                txtName.Text = list1.StepsName;//审批步骤名称

                if (list1.AppRelations == 0)
                {
                    txtRetion.Text = "或";
                }
                else
                {
                    txtRetion.Text = "与";
                }
                txtRole.Text = list1.RoleName;//角色
                //状态
                switch (list1.AppStatus)
                {
                    case -1:
                        txtState.Text = "已撤单";
                        break;
                    case 0:
                        txtState.Text = "审批中";
                        break;
                    case 1:
                        txtState.Text = "未通过";
                        break;
                    case 2:
                        txtState.Text = "已通过";
                        break;
                    default:
                        txtState.Text = "未审批！";
                        break;
                }
                txtMen.Text = ClientUtil.ToString((list1.AuditPerson as PersonInfo).Name);//角色名称
                txtIdea.Text = list1.AppComments;//意见
                dtpDate.Value = list1.AppDate;// 时间
            }
        }
        //取数据
         void ShowAppSetpSet(AppTableSet theAppTableSet,string BillId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BillId", BillId));
            oq.AddCriterion(Expression.Eq("AppStatus", ClientUtil.ToLong(2)));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddFetchMode("AuditPerson", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("AppRole", NHibernate.FetchMode.Eager);

            //当前单据的审批信息
            list = Model.Service.GetAppStepsInfo(oq);
            //list_AppSolution = Model.Service.GetAppSolution(theAppTableSet);
            listDtl = list.OfType<object>();
            pageCount = listDtl.Count();
            btnBack.Enabled = false;
            if (pageCount <= 1)
            {
                btnBack.Visible = false;
                btnNext.Visible = false;
            }
             if(list.Count>0)
             {
            AppStepsInfo list1 = list[pageIndex] as AppStepsInfo;
            label8.Text = "第【" + (pageIndex + 1) + "】条";
            label9.Text = "共【" + pageCount + "】条";
          
              if (list != null && list.Count > 0)
               {
                   txtName.Text = list1.StepsName;//审批步骤名称

                   if (list1.AppRelations == 0)
                   {
                       txtRetion.Text = "或";
                   }
                   else
                   {
                       txtRetion.Text = "与";
                   }
                   txtRole.Text = list1.RoleName;//角色
                   //状态
                   switch (list1.AppStatus)
                   {
                       case -1:
                           txtState.Text = "已撤单";
                           break;
                       case 0:
                           txtState.Text = "审批中";
                           break;
                       case 1:
                           txtState.Text = "未通过";
                           break;
                       case 2:
                           txtState.Text = "已通过";
                           break;
                       default:
                           txtState.Text = "未审批！";
                           break;
                   }
                   txtMen.Text = ClientUtil.ToString((list1.AuditPerson as PersonInfo).Name);//角色名称
                   txtIdea.Text = list1.AppComments;//意见
                   dtpDate.Value = list1.AppDate;// 时间
            }
             }
        }

         private void VAppAlready_Load(object sender, EventArgs e)
         {
             this.WindowState = FormWindowState.Maximized;
         }
    }
}
