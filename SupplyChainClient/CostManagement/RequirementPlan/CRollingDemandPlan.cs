using System;
using System.Collections.Generic;
using System.Text;

using VirtualMachine.Component.WinMVC.core;
using VirtualMachine.Component.WinMVC.generic;

using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.MaterialResource.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Business.Erp.ResourceManager.Client.Main;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public enum OperationRollingDemandPlanType
    {
        总需求计划 = 0,
        总滚动需求计划 = 1,
        生成执行需求计划 = 2,
        执行需求计划查询 = 3,
        资源需求管理 = 4,
        资源需求计划管理 = 5
    }

    public class CRollingDemandPlan
    {
        private static IFramework framework = null;
        string mainViewName = "生成执行需求计划";
        private static VGenerateExecDemandPlanSearchList searchList;

        private static VResourcesDemandPlanManagementSearchList searchList_flag;

        public CRollingDemandPlan(IFramework fw)
        {
            if (framework == null)
                framework = fw;

            searchList = new VGenerateExecDemandPlanSearchList(this);
        }
        public CRollingDemandPlan(IFramework fw,bool flag)
        {
            if (framework == null)
                framework = fw;
            searchList_flag = new VResourcesDemandPlanManagementSearchList(this);
        }
        public void Start()
        {
            Find("空");
        }
        public void Find(string name)
        {
            string captionName = mainViewName;
            if (name is string)
            {
                captionName = this.mainViewName + "-" + name;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VGenerateExecDemandPlan vMainView = framework.GetMainView(mainViewName + "-空") as VGenerateExecDemandPlan;

            if (vMainView == null)
            {
                vMainView = new VGenerateExecDemandPlan(new MRollingDemandPlan());
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VGenerateExecDemandPlanSearchCon searchCon = new VGenerateExecDemandPlanSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(name);

            vMainView.ViewShow();
        }
        public void Find(RemandPlanType planType, string code, string GUID)
        {
            string captionName = this.mainViewName + "-" + code;

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VGenerateExecDemandPlan vMainView = new VGenerateExecDemandPlan(new MRollingDemandPlan());

            //载入查询视图
            //分配辅助视图
            vMainView.AssistViews.Add(searchList);

            VGenerateExecDemandPlanSearchCon searchCon = new VGenerateExecDemandPlanSearchCon(searchList);
            vMainView.AssistViews.Add(searchCon);

            //载入框架
            framework.AddMainView(vMainView);


            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(planType, code, GUID);

            vMainView.ViewShow();
        }


        public void Find(string code, string GUID)
        {
            string viewName = "资源需求计划管理";

            string captionName = viewName;
            if (GUID is string)
            {
                captionName += ("-" + code);
            }



            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VResourcesDemandPlanManagement vMainView = framework.GetMainView(viewName + "-空") as VResourcesDemandPlanManagement;

            if (vMainView == null)
            {
                vMainView = new VResourcesDemandPlanManagement();
                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList_flag);

                VResourcesDemandPlanManagementSearchCon searchCon = new VResourcesDemandPlanManagementSearchCon(searchList_flag);
                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }


            vMainView.ViewCaption = captionName;
            vMainView.ViewName = viewName;
            vMainView.Start(GUID);

            vMainView.ViewShow();
        }
 
        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object o = args[0];
                OperationRollingDemandPlanType executeType = (OperationRollingDemandPlanType)o;

                string viewName = "";
                IMainView mv = null;
                switch (executeType)
                {
                    case OperationRollingDemandPlanType.总需求计划:
                        viewName = "总需求计划";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MRollingDemandPlan mmc = new MRollingDemandPlan();
                            VResourceDemandPlan vmc = new VResourceDemandPlan(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationRollingDemandPlanType.总滚动需求计划:
                        viewName = "总滚动需求计划";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MRollingDemandPlan mmc = new MRollingDemandPlan();
                            VRollingDemandPlan vmc = new VRollingDemandPlan(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationRollingDemandPlanType.生成执行需求计划:
                        viewName = "生成执行需求计划";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MRollingDemandPlan mmc = new MRollingDemandPlan();
                            VGenerateExecDemandPlan vmc = new VGenerateExecDemandPlan(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationRollingDemandPlanType.执行需求计划查询:
                        viewName = "执行需求计划查询";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MRollingDemandPlan mmc = new MRollingDemandPlan();
                            VGenerateExecDemandPlanSelect vmc = new VGenerateExecDemandPlanSelect(mmc);
                            vmc.ViewCaption = viewName;
                            vmc.ViewName = mainViewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationRollingDemandPlanType.资源需求管理:
                        viewName = "资源需求管理";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MRollingDemandPlan mmc = new MRollingDemandPlan();
                            VResourcesDemandManagement vrdm = new VResourcesDemandManagement();
                            vrdm.ViewCaption = viewName;
                            vrdm.ViewName = mainViewName;
                            framework.AddMainView(vrdm);
                        }
                        break;
                    case OperationRollingDemandPlanType.资源需求计划管理:
                        viewName = "资源需求计划管理";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                            mv.ViewShow();
                        else
                        {
                            MRollingDemandPlan mmc = new MRollingDemandPlan();
                            VResourcesDemandPlanManagement vdpm = new VResourcesDemandPlanManagement();
                            //载入查询视图
                            //分配辅助视图
                            vdpm.AssistViews.Add(searchList_flag);
                            VResourcesDemandPlanManagementSearchCon searchCon = new VResourcesDemandPlanManagementSearchCon(searchList_flag);

                            vdpm.AssistViews.Add(searchCon);

                            //载入框架
                            vdpm.ViewCaption = viewName;
                            vdpm.ViewName = viewName;
                            framework.AddMainView(vdpm);
                        }
                        break;
                       
                }
            }

            return null;
        }
    }
}
