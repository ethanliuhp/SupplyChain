using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public enum FundPlanExecType
    {
        资金计划申报 = 1,
        资金计划查询 = 2,
        资金计划审批 = 3,
        资金计划分配 = 4,
        资金支付审批单 = 5,
        资金考核 = 6,
        资金考核查询 = 7,
        资金策划成效分析 = 8,
        资金考核兑现 = 9,
        资金利息计算 = 10,
        资金支付审批单审核 = 11,
        资金支付申请单编制 = 12,
        资金支付申请单审核 = 13,
        资金支付申请单查询 = 14
    }

    public class CFundPlan
    {
        private static IFramework framework = null;
        string mainViewName = "资金计划申报";
        private static VPlanDeclareSearchList searchList;
        private static VPlanPaySearchList paySearchList;
        private static VFundAssessmentSearchList assessmentSearchList;

        public CFundPlan()
        {

        }

        public CFundPlan(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VPlanDeclareSearchList(this);
            paySearchList = new VPlanPaySearchList(this);
            assessmentSearchList = new VFundAssessmentSearchList(this);
        }

        public void Start(FundPlanExecType execType)
        {
            if (execType == FundPlanExecType.资金计划申报)
            {
                FindPlanDeclare("空", "空");
            }
            else if (execType == FundPlanExecType.资金支付审批单)
            {
                FindPlanPay("空", "空");
            }
            else if (execType == FundPlanExecType.资金考核)
            {
                FindFundAssessment("空", "空");
            }
            else if (execType == FundPlanExecType.资金考核兑现)
            {
                FindFundAssessmentCash("空", "空");
            }
            else if (execType == FundPlanExecType.资金利息计算)
            {
                FindFundInterestCompute("空", "空");
            }
        }

        public void FindPlanDeclare(string name, string Id)
        {
            mainViewName = "资金计划申报";
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

            VPlanDeclare vMainView = framework.GetMainView(mainViewName + "-空") as VPlanDeclare;
            if (vMainView == null)
            {
                vMainView = new VPlanDeclare();
                vMainView.ViewName = mainViewName;
                vMainView.RegisteViewToSubmit();
                vMainView.AssistViews.Add(searchList);//载入查询视图
                VPlanDeclareCon searchCon = new VPlanDeclareCon(searchList);
                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.Start(Id);
            vMainView.ViewShow();
        }

        public void FindPlanPay(string name, string Id)
        {
            mainViewName = "资金支付审批单维护";
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

            VPlanPay vMainView = framework.GetMainView(mainViewName + "-空") as VPlanPay;
            if (vMainView == null)
            {
                vMainView = new VPlanPay();
                vMainView.ViewName = mainViewName;
                vMainView.RegisteViewToSubmit();
                vMainView.AssistViews.Add(paySearchList);//载入查询视图
                VPlanPayCon searchCon = new VPlanPayCon(paySearchList);
                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.Start(Id);
            vMainView.ViewShow();
        }

        public void FindFundAssessment(string name, string Id)
        {
            mainViewName = "资金考核维护";
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

            VFundAssessment vMainView = framework.GetMainView(mainViewName + "-空") as VFundAssessment;
            if (vMainView == null)
            {
                vMainView = new VFundAssessment();
                vMainView.ViewName = mainViewName;
                vMainView.RegisteViewToSubmit();
                vMainView.AssistViews.Add(assessmentSearchList);//载入查询视图
                VFundAssessmentCon searchCon = new VFundAssessmentCon(assessmentSearchList);
                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.Start(Id);
            vMainView.ViewShow();
        }

        public void FindFundAssessmentCash(string name, string Id)
        {
            mainViewName = "资金考核兑现";
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

            VFundAssessmentCash vMainView = framework.GetMainView(mainViewName + "-空") as VFundAssessmentCash;
            if (vMainView == null)
            {
                vMainView = new VFundAssessmentCash();
                vMainView.ViewName = mainViewName;
                vMainView.RegisteViewToSubmit();
                vMainView.AssistViews.Add(assessmentSearchList);//载入查询视图
                VFundAssessmentCon searchCon = new VFundAssessmentCon(assessmentSearchList, "考核兑现");
                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.Start(Id);
            vMainView.ViewShow();
        }

        public void FindFundInterestCompute(string name, string Id)
        {
            mainViewName = "资金利息计算";
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

            VFundInterestCompute vMainView = framework.GetMainView(mainViewName + "-空") as VFundInterestCompute;
            if (vMainView == null)
            {
                vMainView = new VFundInterestCompute();
                vMainView.ViewName = mainViewName;
                vMainView.RegisteViewToSubmit();
                vMainView.AssistViews.Add(assessmentSearchList);//载入查询视图
                VFundAssessmentCon searchCon = new VFundAssessmentCon(assessmentSearchList, "利息计算");
                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.Start(Id);
            vMainView.ViewShow();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start(0);
            }
            else
            {
                object obj = args[0];
                FundPlanExecType executeType = (FundPlanExecType)obj;
                var captionName = string.Empty;
                switch (executeType)
                {
                    case FundPlanExecType.资金计划申报:
                    case FundPlanExecType.资金支付审批单:
                    case FundPlanExecType.资金考核:
                    case FundPlanExecType.资金考核兑现:
                    case FundPlanExecType.资金利息计算:
                        Start(executeType);
                        break;
                    case FundPlanExecType.资金考核查询:
                        captionName = "资金考核查询";
                        VFundAssessmentQuery queryView = framework.GetMainView(captionName) as VFundAssessmentQuery;
                        if (queryView == null)
                        {
                            queryView = new VFundAssessmentQuery();
                            framework.AddMainView(queryView);
                            queryView.ViewCaption = captionName;
                        }
                        queryView.ViewShow();
                        break;
                    case FundPlanExecType.资金计划审批:
                        captionName = "资金计划审批";
                        VPlanDeclareApprove viewMain = framework.GetMainView(captionName) as VPlanDeclareApprove;
                        if (viewMain == null)
                        {
                            viewMain = new VPlanDeclareApprove();
                            framework.AddMainView(viewMain);
                            viewMain.ViewCaption = captionName;
                        }
                        viewMain.ViewShow();
                        break;
                    case FundPlanExecType.资金支付审批单审核:
                        captionName = "资金支付审批单审核";
                        VPlanPayApprove payApprove = framework.GetMainView(captionName) as VPlanPayApprove;
                        if (payApprove == null)
                        {
                            payApprove = new VPlanPayApprove();
                            framework.AddMainView(payApprove);
                            payApprove.ViewCaption = captionName;
                        }
                        payApprove.ViewShow();
                        break;
                    case FundPlanExecType.资金支付申请单编制:
                        captionName = "资金支付申请单编制";
                        VFundPayApplyFormation payApplyFormation = framework.GetMainView(captionName) as VFundPayApplyFormation;
                        if (payApplyFormation == null)
                        {
                            payApplyFormation = new VFundPayApplyFormation();
                            framework.AddMainView(payApplyFormation);
                            payApplyFormation.ViewCaption = captionName;
                        }
                        payApplyFormation.ViewShow();
                        break;
                    case FundPlanExecType.资金支付申请单审核:
                        captionName = "资金支付申请单审核";
                        VFundPayApplyApprove payApplyApprove = framework.GetMainView(captionName) as VFundPayApplyApprove;
                        if (payApplyApprove == null)
                        {
                            payApplyApprove = new VFundPayApplyApprove();
                            framework.AddMainView(payApplyApprove);
                            payApplyApprove.ViewCaption = captionName;
                        }
                        payApplyApprove.ViewShow();
                        break;
                    case FundPlanExecType.资金支付申请单查询:
                        captionName = "资金支付申请单查询";
                        VFundPayApplyQuery payApplyQuery = framework.GetMainView(captionName) as VFundPayApplyQuery;
                        if (payApplyQuery == null)
                        {
                            payApplyQuery = new VFundPayApplyQuery();
                            framework.AddMainView(payApplyQuery);
                            payApplyQuery.ViewCaption = captionName;
                        }
                        payApplyQuery.ViewShow();
                        break;
                    case FundPlanExecType.资金策划成效分析:
                        captionName = "资金策划成效分析";
                        VFundSchemeEfficiency efficiencyView = framework.GetMainView(captionName) as VFundSchemeEfficiency;
                        if (efficiencyView == null)
                        {
                            efficiencyView = new VFundSchemeEfficiency();
                            framework.AddMainView(efficiencyView);
                            efficiencyView.ViewCaption = captionName;
                        }
                        efficiencyView.ViewShow();
                        break;
                }
            }
            return null;
        }
    }
}
