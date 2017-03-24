using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public enum IndirectCostExecType
    {
        费用信息维护 = 0,
        日现金流统计 = 1,
        关键指标计算 = 2,
        间接费用台账 = 3,
        甲方审量及应收台账 = 4,
        财务账面台账 = 5,
        间接费用汇总台账 = 6,
        票据台账 = 7,
        项目资金计划申请 = 8,
        分公司资金计划申请 = 9,
        资金计划查询 = 10,
        资金计划分配 = 11,
    }

    public class CIndirectCost
    {
        private static IFramework framework = null;
        private string mainViewName = "费用信息维护";
        private static VIndirectCostSearchList searchList;

        public CIndirectCost(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VIndirectCostSearchList(this);
        }

        public void Start(IndirectCostExecType execType)
        {
            Find("空", execType, "空");
        }

        public void Find(string name, IndirectCostExecType execType, string Id)
        {
            mainViewName = "费用信息维护";
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

            VIndirectCost vMainView = framework.GetMainView(mainViewName + "-空") as VIndirectCost;

            if (vMainView == null)
            {
                vMainView = new VIndirectCost();
                //vMainView.MonthlyType = execType;
                vMainView.ViewName = mainViewName;

                //载入查询视图
                vMainView.ViewDeletedEvent +=
                    new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(
                        vSaleBudget_ViewDeletedEvent);
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VIndirectCostSearchCon searchCon = new VIndirectCostSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        private void vSaleBudget_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VIndirectCost vDmand = mv as VIndirectCost;
            if (vDmand != null)
                searchList.RemoveRow(vDmand.CurBillMaster.Id);


            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {

            }
            else
            {
                object o = args[0];
                IndirectCostExecType execType = (IndirectCostExecType) o;
                switch (execType)
                {
                    case IndirectCostExecType.费用信息维护:
                        {
                            Start(execType);
                            break;
                        }
                    case IndirectCostExecType.日现金流统计:
                        string viewName = "日现金流统计";
                        IMainView mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VDayCashReport vmc = new VDayCashReport();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case IndirectCostExecType.票据台账:
                        {
                            string viewName2 = "票据台账";
                            IMainView mv2 = framework.GetMainView(viewName2);
                            if (mv2 != null)
                            {
                                mv2.ViewShow();
                            }
                            else
                            {
                                VAcceptanceBillReport vmc2 = new VAcceptanceBillReport();
                                vmc2.ViewCaption = viewName2;
                                framework.AddMainView(vmc2);
                            }
                            break;
                        }

                    case IndirectCostExecType.间接费用台账:
                        {
                            viewName = "间接费用台账";
                            mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VIndirectCostReport vmc = new VIndirectCostReport(execType);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case IndirectCostExecType.甲方审量及应收台账:
                        {
                            viewName = "甲方审量及应收台账";
                            mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VOwnerQuantityReport vmc = new VOwnerQuantityReport(execType);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case IndirectCostExecType.关键指标计算:
                        string viewName1 = "关键指标计算";
                        IMainView mv1 = framework.GetMainView(viewName1);
                        if (mv1 != null)
                        {
                            mv1.ViewShow();
                        }
                        else
                        {
                            VCompanyIndexGenerate vmc1 = new VCompanyIndexGenerate();
                            vmc1.ViewCaption = viewName1;
                            framework.AddMainView(vmc1);
                        }
                        break;
                    case IndirectCostExecType.财务账面台账:
                        {
                            viewName = "财务账面台账";
                            mv1 = framework.GetMainView(viewName);
                            if (mv1 != null)
                            {
                                mv1.ViewShow();
                            }
                            else
                            {
                                mv1 = new VFinancialMngReporter();
                                mv1.ViewCaption = viewName;
                                framework.AddMainView(mv1);
                            }
                            break;
                        }
                    case IndirectCostExecType.项目资金计划申请:
                        {
                            viewName = "项目资金计划申请";
                            IMainView mv3 = framework.GetMainView(viewName);
                            if (mv3 != null)
                            {
                                mv3.ViewShow();
                            }
                            else
                            {
                                mv3 = new VProjectFundPlan();
                                mv3.ViewCaption = viewName;
                                framework.AddMainView(mv3);
                            }
                            break;
                        }
                    case IndirectCostExecType.分公司资金计划申请:
                        {
                            viewName = "分公司资金计划申请";
                            IMainView mv4 = framework.GetMainView(viewName);
                            if (mv4 != null)
                            {
                                mv4.ViewShow();
                            }
                            else
                            {
                                mv4 = new VFilialeFundPlan();
                                mv4.ViewCaption = viewName;
                                framework.AddMainView(mv4);
                            }
                            break;
                        }
                    case IndirectCostExecType.资金计划查询:
                        {
                            viewName = "资金计划查询";
                            IMainView mv5 = framework.GetMainView(viewName);
                            if (mv5 != null)
                            {
                                mv5.ViewShow();
                            }
                            else
                            {
                                mv5 = new VPlanDeclareQuery();
                                mv5.ViewCaption = viewName;
                                framework.AddMainView(mv5);
                            }
                            break;
                        }
                    case IndirectCostExecType.资金计划分配:
                        {
                            viewName = "资金计划分配";
                            IMainView mv6 = framework.GetMainView(viewName);
                            if (mv6 != null)
                            {
                                mv6.ViewShow();
                            }
                            else
                            {
                                mv6 = new VPlanDeclareAllot();
                                mv6.ViewCaption = viewName;
                                framework.AddMainView(mv6);
                            }
                            break;
                        }
                    default:
                        {
                            Start(execType);
                            break;
                        }
                }
            }
            return null;
        }
    }
}
