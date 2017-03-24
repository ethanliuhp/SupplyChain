using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostBusinessAccount;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement;
using Application.Business.Erp.SupplyChain.Basic.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public enum CCostMonthAccount_ExecType
    {
        MonthAccount,//月度核算月结
        MonthAccountSpecial,//月度核算月结(安装)
        MonthLog,//月结日志
        MonthQuery,//月度核算查询
        MonthReport,//月度成本报表
        MonthReportNew,//新月度成本报表
        EconomyProfit,//经济效益
        CostConsume,//成本消耗
        TechIndicator,//技经指标
        IncomeCost,//收入成本
        ReceiveMoney,//财务收款
        ContractLedger,//合同变化台帐
        BusinessReport,//商务报表
        ProduceReport,//生产报表
        MonthGenQuery,//月度核算综合查询
        SpecialReport,//安装月度成本分析报表
        SpecialBusinessReport,//安装商务分析报表
        OEMChargebackReport,   // 项目结算罚款台帐
        SubcontractsLedger,//分包合同台账
        FineAccountReport,   // 项目结算罚款台帐
        ContractDisclosure, //合同交底样稿
        ContractDisclosureQuery,//合同交底查询
        SubcontractSettlementReport, //项目分包结算台帐
        MaterialRentalSettlementReport,  // 机械租赁结算台账
        SubcontractAmountReport,  // 分包单位工程量台账
        CommercialReport,   //项目商务信息填报
        ConfirmRightReport, //项目业主确权台账

        CommercialReportQuery,   //项目商务信息填报查询
        SceneManageFeelReport,   //现场管理费对比表


        CostMonthCompareReport,//月度成本分析对比表
        MechanicalCostComparisonRpt, //机械费成本分析对比表
        //CommercialReportQuery   //项目商务信息填报查询
        多维度对比表,
        SelFeeTemplate //取费模板定义
    }

    public class CCostMonthAccount
    {
        static IFramework framework;
        string mainViewName = "月度成本核算单";
        string sViewName = "合同交底样稿"; //合同交底视图名称
        private static VContractDisclosureSearchList searchList;
        private static VSelFeeTemplateSearchList searchList1;
        private CCostMonthAccount_ExecType enumType;

        public CCostMonthAccount(IFramework fw)
        {
            if (framework == null)
                framework = fw;
            searchList = new VContractDisclosureSearchList(this);
            searchList1 = new VSelFeeTemplateSearchList(this);
        }

        public void Start(CCostMonthAccount_ExecType enumType)
        {
            Find("空", enumType, "空");
        }

        public void Find(string name, CCostMonthAccount_ExecType enumType, string Id)
        {
            if (enumType == CCostMonthAccount_ExecType.SelFeeTemplate)
            {
                string sViewName1 = "取费模板定义";
                string CName = sViewName1;
                if (name is string)
                {
                    CName = sViewName1 + "-" + name;
                }
                IMainView mv = framework.GetMainView(CName);
                if (mv != null)
                {
                    //如果当前视图已经存在，直接显示
                    mv.ViewShow();
                    return;
                }
                VSelFeeTemplate vMainView = framework.GetMainView(sViewName1 + "-空") as VSelFeeTemplate;
                if (vMainView == null)
                {
                    vMainView = new VSelFeeTemplate();
                    vMainView.ViewName = sViewName1;
                    //vMainView.RegisteViewToSubmit();

                    //载入查询视图
                    //分配辅助视图
                    vMainView.AssistViews.Add(searchList1);
                    VSelFeeTemplateSearchCon searchCon1 = new VSelFeeTemplateSearchCon(searchList1);
                    vMainView.AssistViews.Add(searchCon1);

                    //载入框架
                    framework.AddMainView(vMainView);
                }

                vMainView.ViewCaption = CName;
                vMainView.ViewName = sViewName1;
                vMainView.Start(Id);

                vMainView.ViewShow();
            }
            else
            {
                string captionName = mainViewName;
                string capName = sViewName;
                if (name is string)
                {
                    captionName = this.mainViewName + "-" + name;
                    capName = this.sViewName + "-" + name;
                }
                IMainView mv = framework.GetMainView(captionName);
                IMainView smv = framework.GetMainView(capName);
                if (mv != null)
                {
                    //如果当前视图已经存在，直接显示
                    mv.ViewShow();
                    return;
                }
                if (smv != null)
                {
                    smv.ViewShow();
                    return;
                }
                VContractDisclosure vMainView = framework.GetMainView(sViewName + "-空") as VContractDisclosure;
                if (vMainView == null)
                {
                    vMainView = new VContractDisclosure();
                    vMainView.ViewName = sViewName;

                    //载入查询视图
                    //分配辅助视图
                    vMainView.AssistViews.Add(searchList);
                    VContractDisclosureSearchCon searchCon = new VContractDisclosureSearchCon(searchList);
                    vMainView.AssistViews.Add(searchCon);

                    //载入框架
                    framework.AddMainView(vMainView);
                }

                vMainView.ViewCaption = capName;
                vMainView.ViewName = sViewName;
                vMainView.Start(Id);

                vMainView.ViewShow();
            }
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
                Start(enumType);
            else
            {
                object o = args[0];
                CCostMonthAccount_ExecType executeType = (CCostMonthAccount_ExecType)o;
                string theViewName = string.Empty;
                IMainView mainView = null;
                switch (executeType)
                {
                    case CCostMonthAccount_ExecType.MonthAccount:
                        {
                            string viewName = "月度成本核算维护";
                            IMainView mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VCostMonthAccount vmc = new VCostMonthAccount(0);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case CCostMonthAccount_ExecType.MonthAccountSpecial:
                        string viewName20 = "月度成本核算(安装)";
                        IMainView mv20 = framework.GetMainView(viewName20);
                        if (mv20 != null)
                        {
                            mv20.ViewShow();
                        }
                        else
                        {
                            VCostMonthAccount vmc = new VCostMonthAccount(1);
                            vmc.ViewCaption = viewName20;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.MonthLog:
                        string viewName1 = "月度成本核算日志";
                        IMainView mv1 = framework.GetMainView(viewName1);
                        if (mv1 != null)
                        {
                            mv1.ViewShow();
                        }
                        else
                        {
                            VCostMonthLogQuery vmc = new VCostMonthLogQuery();
                            vmc.ViewCaption = viewName1;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.MonthQuery:
                        string viewName2 = "月度成本核算查询";
                        IMainView mv2 = framework.GetMainView(viewName2);
                        if (mv2 != null)
                        {
                            mv2.ViewShow();
                        }
                        else
                        {
                            VCostMonthAccountQuery vmc = new VCostMonthAccountQuery();
                            vmc.ViewCaption = viewName2;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.MonthReport:
                        {
                            string viewName3 = "月度成本核算报表";
                            IMainView mv3 = framework.GetMainView(viewName3);
                            if (mv3 != null)
                            {
                                mv3.ViewShow();
                            }
                            else
                            {
                                CurrentProjectInfo projectinfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                                if (projectinfo.ProjectInfoState == EnumProjectInfoState.老项目)
                                {
                                    VCostMonthReport vmc = new VCostMonthReport();
                                    vmc.ViewCaption = viewName3;
                                    framework.AddMainView(vmc);
                                }
                                else
                                {
                                    VCostMonthReportNew vmc = new VCostMonthReportNew();
                                    vmc.ViewCaption = viewName3;
                                    framework.AddMainView(vmc);
                                }
                            }
                            break;
                        }
                    case CCostMonthAccount_ExecType.MonthReportNew:
                        {
                            string viewName3 = "新月度成本核算报表";
                            IMainView mv3 = framework.GetMainView(viewName3);
                            if (mv3 != null)
                            {
                                mv3.ViewShow();
                            }
                            else
                            {
                                VCostMonthReportNew vmc = new VCostMonthReportNew();
                                vmc.ViewCaption = viewName3;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case CCostMonthAccount_ExecType.CostMonthCompareReport:
                        {
                            string viewName3 = "月度成本分析对比报表";
                            IMainView mv3 = framework.GetMainView(viewName3);
                            if (mv3 != null)
                            {
                                mv3.ViewShow();
                            }
                            else
                            {
                                VCostMonthCompareReport vmc = new VCostMonthCompareReport();
                                vmc.ViewCaption = viewName3;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case CCostMonthAccount_ExecType.EconomyProfit:
                        string viewName4 = "经济效益统计表";
                        IMainView mv4 = framework.GetMainView(viewName4);
                        if (mv4 != null)
                        {
                            mv4.ViewShow();
                        }
                        else
                        {
                            VCompanyReport vmc = new VCompanyReport(1);
                            vmc.ViewCaption = viewName4;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.CostConsume:
                        string viewName5 = "成本消耗统计表";
                        IMainView mv5 = framework.GetMainView(viewName5);
                        if (mv5 != null)
                        {
                            mv5.ViewShow();
                        }
                        else
                        {
                            VCompanyReport vmc = new VCompanyReport(2);
                            vmc.ViewCaption = viewName5;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.TechIndicator:
                        string viewName6 = "技经指标统计表";
                        IMainView mv6 = framework.GetMainView(viewName6);
                        if (mv6 != null)
                        {
                            mv6.ViewShow();
                        }
                        else
                        {
                            VCompanyReport vmc = new VCompanyReport(3);
                            vmc.ViewCaption = viewName6;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.IncomeCost:
                        string viewName7 = "收入成本统计表";
                        IMainView mv7 = framework.GetMainView(viewName7);
                        if (mv7 != null)
                        {
                            mv7.ViewShow();
                        }
                        else
                        {
                            VCompanyReport vmc = new VCompanyReport(4);
                            vmc.ViewCaption = viewName7;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.ReceiveMoney:
                        string viewName8 = "财务收款统计表";
                        IMainView mv8 = framework.GetMainView(viewName8);
                        if (mv8 != null)
                        {
                            mv8.ViewShow();
                        }
                        else
                        {
                            VCompanyReport vmc = new VCompanyReport(5);
                            vmc.ViewCaption = viewName8;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.ContractLedger:
                        string viewName9 = "签证变更统计表";
                        IMainView mv9 = framework.GetMainView(viewName9);
                        if (mv9 != null)
                        {
                            mv9.ViewShow();
                        }
                        else
                        {
                            VContractLedgerReport vmc = new VContractLedgerReport();
                            vmc.ViewCaption = viewName9;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.BusinessReport:
                        string viewName10 = "商务报表统计";
                        IMainView mv10 = framework.GetMainView(viewName10);
                        if (mv10 != null)
                        {
                            mv10.ViewShow();
                        }
                        else
                        {
                            VCostBusinessReport vmc = new VCostBusinessReport();
                            vmc.ViewCaption = viewName10;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.ProduceReport:
                        string viewName11 = "生产施工统计报表";
                        IMainView mv11 = framework.GetMainView(viewName11);
                        if (mv11 != null)
                        {
                            mv11.ViewShow();
                        }
                        else
                        {
                            VProduceReport vmc = new VProduceReport();
                            vmc.ViewCaption = viewName11;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.MonthGenQuery:
                        string viewName12 = "月度成本核算综合查询";
                        IMainView mv12 = framework.GetMainView(viewName12);
                        if (mv12 != null)
                        {
                            mv12.ViewShow();
                        }
                        else
                        {
                            VCostMonthAcctGenQuery vmc = new VCostMonthAcctGenQuery();
                            vmc.ViewCaption = viewName12;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.SpecialReport:
                        string viewName13 = "月度成本分析报表(安装)";
                        IMainView mv13 = framework.GetMainView(viewName13);
                        if (mv13 != null)
                        {
                            mv13.ViewShow();
                        }
                        else
                        {
                            VSpecialCostReport vmc = new VSpecialCostReport();
                            vmc.ViewCaption = viewName13;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.SpecialBusinessReport:
                        string viewName14 = "商务报表(安装)";
                        IMainView mv14 = framework.GetMainView(viewName14);
                        if (mv14 != null)
                        {
                            mv14.ViewShow();
                        }
                        else
                        {
                            VSpecialBusinessReport vmc = new VSpecialBusinessReport();
                            vmc.ViewCaption = viewName14;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.SubcontractsLedger:
                        string viewName15 = "分包合同台账";
                        IMainView mv15 = framework.GetMainView(viewName15);
                        if (mv15 != null)
                        {
                            mv15.ViewShow();
                        }
                        else
                        {
                            VSubcontractsLedger vmc = new VSubcontractsLedger();
                            vmc.ViewCaption = viewName15;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.OEMChargebackReport:
                        string viewName16 = "项目代工扣款台账";
                        IMainView mv16 = framework.GetMainView(viewName16);
                        if (mv16 != null)
                        {
                            mv16.ViewShow();
                        }
                        else
                        {
                            VOEMChargebackReport vmc = new VOEMChargebackReport();
                            vmc.ViewCaption = viewName16;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.FineAccountReport:
                        string viewName17 = "项目结算罚款台帐";
                        IMainView mv17 = framework.GetMainView(viewName17);
                        if (mv17 != null)
                        {
                            mv17.ViewShow();
                        }
                        else
                        {
                            VFineAccountReport vmc = new VFineAccountReport();
                            vmc.ViewCaption = viewName17;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.ContractDisclosure:
                        {
                            string viewName18 = "合同交底样稿";
                            Start(executeType);
                            break;
                        }
                    case CCostMonthAccount_ExecType.ContractDisclosureQuery:
                        {
                            string viewName = "合同交底查询";
                            IMainView mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VContractDisclosureQuery vmc = new VContractDisclosureQuery();
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case CCostMonthAccount_ExecType.SubcontractSettlementReport:
                        string viewName19 = "项目分包结算台账";
                        IMainView mv19 = framework.GetMainView(viewName19);
                        if (mv19 != null)
                        {
                            mv19.ViewShow();
                        }
                        else
                        {
                            VSubcontractSettlementReport vmc = new VSubcontractSettlementReport();
                            vmc.ViewCaption = viewName19;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.MaterialRentalSettlementReport:
                        string viewName21 = "机械租赁结算台账";
                        IMainView mv21 = framework.GetMainView(viewName21);
                        if (mv21 != null)
                        {
                            mv21.ViewShow();
                        }
                        else
                        {
                            VMaterialRentalSettlementReport vmc = new VMaterialRentalSettlementReport();
                            vmc.ViewCaption = viewName21;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.SubcontractAmountReport:
                        string viewName22 = "分包单位工程量台账";
                        IMainView mv22 = framework.GetMainView(viewName22);
                        if (mv22 != null)
                        {
                            mv22.ViewShow();
                        }
                        else
                        {
                            VSubcontractAmountReport vmc = new VSubcontractAmountReport();
                            vmc.ViewCaption = viewName22;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.CommercialReport:
                        string viewName23 = "商务报表填报";
                        IMainView mv23 = framework.GetMainView(viewName23);
                        if (mv23 != null)
                        {
                            mv23.ViewShow();
                        }
                        else
                        {
                            VCommercialReport vmc = new VCommercialReport();
                            vmc.ViewCaption = viewName23;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.ConfirmRightReport:
                        string viewName24 = "项目业主确权台账";
                        IMainView mv24 = framework.GetMainView(viewName24);
                        if (mv24 != null)
                        {
                            mv24.ViewShow();
                        }
                        else
                        {
                            //VMaterialRentalSettlementReport vmc = new VMaterialRentalSettlementReport();
                            VConfirmRightReport vmc = new VConfirmRightReport();
                            vmc.ViewCaption = viewName24;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.CommercialReportQuery:
                        string viewName25 = "商务报表填报统计";
                        IMainView mv25 = framework.GetMainView(viewName25);
                        if (mv25 != null)
                        {
                            mv25.ViewShow();
                        }
                        else
                        {
                            VCommercialReportQuery vmc = new VCommercialReportQuery();
                            vmc.ViewCaption = viewName25;
                            framework.AddMainView(vmc);
                        }
                        break;

                    case CCostMonthAccount_ExecType.SceneManageFeelReport:
                        string viewName26 = "现场管理费对比表";
                        IMainView mv26 = framework.GetMainView(viewName26);
                        if (mv26 != null)
                        {
                            mv26.ViewShow();
                        }
                        else
                        {
                            VSceneManagementReport vmc = new VSceneManagementReport();
                            vmc.ViewCaption = viewName26;
                            framework.AddMainView(vmc);
                        }
                        break;

                    case CCostMonthAccount_ExecType.MechanicalCostComparisonRpt:
                        string viewName_mccr = "机械费成本分析对比表";
                        IMainView mv_mccr = framework.GetMainView(viewName_mccr);
                        if (mv_mccr != null)
                        {
                            mv_mccr.ViewShow();
                        }
                        else
                        {
                            VMechanicalCostComparisonRpt vmc = new VMechanicalCostComparisonRpt();
                            vmc.ViewCaption = viewName_mccr;
                            framework.AddMainView(vmc);
                        }
                        break;

                    case CCostMonthAccount_ExecType.多维度对比表:
                        theViewName = "多维度对比表";
                        mainView = framework.GetMainView(theViewName);
                        if (mainView != null)
                        {
                            mainView.ViewShow();
                        }
                        else
                        {
                            VMultiDimensionRpt vmc = new VMultiDimensionRpt();
                            vmc.ViewCaption = theViewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case CCostMonthAccount_ExecType.SelFeeTemplate:
                        Find("空", CCostMonthAccount_ExecType.SelFeeTemplate, "空");
                        break;

                }
            }

            return null;
        }
    }
}
