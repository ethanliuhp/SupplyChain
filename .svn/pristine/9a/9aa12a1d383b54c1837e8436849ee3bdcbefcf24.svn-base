using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VResourcesDemandPlanManagementSearchCon : BasicUserControl
    {
        private MRollingDemandPlan model = new MRollingDemandPlan();
        private VResourcesDemandPlanManagementSearchList searchList;

        public VResourcesDemandPlanManagementSearchCon()
        {
            InitializeComponent();

            InitForm();
        }

        public VResourcesDemandPlanManagementSearchCon(VResourcesDemandPlanManagementSearchList searchList)
        {
            InitializeComponent();
            this.searchList = searchList;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitForm();

            cbPlanType.Focus();
        }

        private void InitForm()
        {
            label2.Visible = false;
            cbPlanType.Visible = false;

            //生成执行需求计划
            foreach (string s in Enum.GetNames(typeof(RemandPlanType)))
            {
                cbPlanType.Items.Add(s);
            }
            if (cbPlanType.Items.Count > 0)
                cbPlanType.SelectedIndex = 0;

            dtMadeBillStartDate.Value = DateTime.Now.AddMonths(-1);
            dtMadeBillEndDate.Value = DateTime.Now;

            InitialEvents();
        }

        private void InitialEvents()
        {
            btnOK.Click += new EventHandler(btnQuery_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        //查询
        void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                
                string planName = txtPlanName.Text.Trim();

                DateTime madeStartDate = dtMadeBillStartDate.Value.Date;
                DateTime madeEndDate = dtMadeBillEndDate.Value.Date.AddDays(1).AddSeconds(-1);

                if (madeStartDate > madeEndDate)
                {
                    MessageBox.Show("制单起始日期不能大于结束日期！");
                    dtMadeBillEndDate.Focus();
                    return;
                }

                ObjectQuery oq = new ObjectQuery();
                //默认制单人和项目作为查询条件
                PersonInfo loginUser = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (loginUser == null || string.IsNullOrEmpty(loginUser.Id))
                {
                    MessageBox.Show("未获取到登录用户信息，查询终止!");
                    return;
                }
                else if (projectInfo == null || string.IsNullOrEmpty(projectInfo.Id))
                {
                    MessageBox.Show("未获取到当前项目信息，查询终止!");
                    return;
                }

                //oq.AddCriterion(Expression.Eq("HandlePerson.Id", loginUser.Id));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

                oq.AddCriterion(Expression.Ge("CreateDate", madeStartDate));
                oq.AddCriterion(Expression.Le("CreateDate", madeEndDate));

                //oq.AddCriterion(Expression.Eq("PlanType", ExecuteDemandPlanTypeEnum.滚动计划));

                if (!string.IsNullOrEmpty(planName))
                    oq.AddCriterion(Expression.Like("PlanName", planName, MatchMode.Anywhere));

                oq.AddOrder(Order.Desc("CreateDate"));

                IList list = null;
                list = model.ObjectQuery(typeof(ResourceRequireReceipt), oq);

                searchList.RefreshData( list);
                this.btnOK.FindForm().Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //取消  
        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
