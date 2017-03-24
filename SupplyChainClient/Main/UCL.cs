using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.BasicData.MachineClassMng;
using Application.Business.Erp.SupplyChain.Client.BasicData.ClassTeamMng;
using AuthManager.AuthMng.AuthConfigMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.BasicData.ExpenseItemMng;
using Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.Client.Util;
using CustomServiceClient.QuestionMng;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppTableSetUI;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn;
using Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConPouringNoteMng;
using Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng;
using Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConCheckingMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan;
using Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng;
using Application.Business.Erp.SupplyChain.Client.SupplyMng;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount;
using Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS;
using Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng;
using Application.Business.Erp.SupplyChain.Client.WebMng;
using Application.Business.Erp.SupplyChain.Client.MobileManage;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.PMCAndWarning.Domain;
using Application.Business.Erp.SupplyChain.Client.PMCAndWarning;
using Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection;
using Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng;
using Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn;

namespace Application.Business.Erp.SupplyChain.Client.Main
{
    public class UCL
    {
        static IFramework framework;
        public static IFramework Framework
        {
            get { return framework; }
            set { framework = value; }
        }

        private static AuthManagerLib.AuthMng.MenusMng.Domain.Menus _theAuthMenu = null;
        /// <summary>
        /// 当前权限菜单
        /// </summary>
        public static AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu
        {
            get { return UCL._theAuthMenu; }
            set { UCL._theAuthMenu = value; }
        }

        public static object Locate(string control, params object[] args)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目 && (control == "零星用工单维护" || control == "零星用工单核算"
                || control == "工程任务核算单维护" || control == "施工部位划分维护" || control == "施工任务划分维护" || control == "工程任务核算单维护" || control == "现场生产管理"))
            {
                MessageBox.Show("新版2.0不允许使用此功能！");
                return null;
            }
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.老项目 && (control == "PBS节点维护" || control == "施工任务划分维护_新"
               || control == "新现场生产管理" || control == "工程量确认单"))
            {
                MessageBox.Show("老版1.0不允许使用此功能！");
                return null;
            }
            if (projectInfo.Code == CommonUtil.CompanyProjectCode)
            {
                if (control == "项目基本信息维护" || control == "施工部位划分维护"
                    || control == "施工任务划分维护" || control == "施工任务划分维护_新" || control == "清单任务划分维护"
                    || control.Contains("日常需求") || control.Contains("业主报量单维护") || control.Equals("收款单") || control.Equals("付款单"))
                {
                    MessageBox.Show("登陆位置为[公司/分公司项目基础管理],不能进行相关业务操作！");
                    return null;
                }
            }
            switch (control)
            {
                case "起始页":
                    new CStartPage(framework).Start();
                    break;

                #region 资源管理
                case "会计期间维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.CFiscalPeriod(framework).Excute(args);
                case "资源分类维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CMaterialCategory(framework).Excute(args);
                case "物料核算角色维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CAccountRole(framework).Excute(args);
                case "资源维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CMaterial(framework).Excute(args);
                case "资源查询":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CMaterial(framework).Excute(MaterialExcuteType.MaterialSearch);
                case "计量单位维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CStandardUnit(framework).Excute(args);
                case "计量单位量纲选择":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CStandardUnit(framework).Excute(args);
                case "人员维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource.CPerson(framework).Excute(args);
                case "人员上岗维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource.CPersonOnJob(framework).Excute(args);
                case "人员任职维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource.CEmpOnPost(framework).Excute(args);
                case "业务组织维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COperationOrgTree(framework).Excute(args);
                case "业务岗位维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COperationJob(framework).Excute(args);
                case "行政组织维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COrganization(framework).Excute(args);
                case "行政职位维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.CPoliticalPost(framework).Excute(args);
                case "伙伴实体维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.CPartner(framework).Excute(args);
                case "客户分类维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement.CCustomerRelationCategory(framework).Excute(args);
                case "客户关系维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement.CCustomerRelation(framework).Excute(args);
                case "供应分类维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.SupplierManagement.CSupplierRelationCategory(framework).Excute(args);
                case "供应关系维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.SupplierManagement.CSupplierRelation(framework).Excute(args);
                case "伙伴实体查询":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.CPartner(framework).Excute(PartnerExecuteType.ObjectSearch);
                case "银行信息维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.BankInfoUI.CBankInfo(framework).Excute(args);
                case "银行帐号":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.BankAcctUI.CBankAcct(framework).Excute(args);
                case "支付业务配置":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.BusTypeUI.CBusType(framework).Excute(args);
                case "资金科目配置":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.MoneyVsAcctTitUI.CMoneyVsAcctTit(framework).Excute(control, args);
                case "班别维护":
                    return new Application.Business.Erp.SupplyChain.Client.BasicData.ClassesMng.CClasses(framework).Excute(args);
                case "班组维护":
                    return new Application.Business.Erp.SupplyChain.Client.BasicData.ClassTeamMng.CClassTeam(framework).Excute(args);
                case "会计科目":
                    //return new Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI.CAccountTitle(framework).Excute(control, args);
                    return new Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI.CAccountTitle(framework).Excute(AccountTitleType.会计科目树);
                case "费用项目维护":
                    return new CExpenseItem(framework).Excute(args);
                case "成本项目维护":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI.CCostProject(framework).Excute(control, args);
                case "费用项目选择":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI.CCostProject(framework).Excute(CostPtojectType.costProjectSelect);
                case "基础数据维护":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.basicDataOptr);
                case "日志查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.logDataQuery);
                case "日志统计":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.logStatReport);
                case "角色维护":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COperationJob(framework).Excute(Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COperationJob.AccountOrgExecuteType.角色维护);

                #endregion

                #region 仓库管理
                case "领料出库单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI.CStockOut(framework).Excute(EnumStockExecType.安装);
                case "领料出库单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI.CStockOut(framework).Excute(EnumStockExecType.土建);
                case "领料出库红单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI.CStockOutRed(framework).Excute(EnumStockExecType.土建);
                case "领料出库红单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI.CStockOutRed(framework).Excute(EnumStockExecType.安装);
                case "领料出库单查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI.CStockOut(framework).Excute(EnumStockExecType.StockOutQuery, TheAuthMenu);
                case "辅材数据比例统计":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI.CStockOut(framework).Excute(EnumStockExecType.辅材数据比例统计);
                case "库存查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.StockRelationQuery);


                //return new Application.Business.Erp.SupplyChain.Client.WebMng.CWebMng(framework).Excute("WorkFlowHistory");
                case "盘盈单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn.CProfitIn(framework).Execute(EnumStockExecType.安装);
                case "盘盈单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn.CProfitIn(framework).Execute(EnumStockExecType.土建);
                //xl
                case "盘盈单查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn.CProfitIn(framework).Execute(Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn.ProfitInExcType.ProfitInQuery);
                case "盘亏单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut.CLossOut(framework).Execute(EnumStockExecType.土建);
                case "盘亏单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut.CLossOut(framework).Execute(EnumStockExecType.安装);
                case "盘亏单查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut.CLossOut(framework).Execute(Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut.LossOutExcType.LossOutQuery);
                case "物资实际耗用结算":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(args);
                //return new Application.Business.Erp.SupplyChain.Client.WebMng.CWebMng(framework).Excute(args);
                case "重新加载配置文件":
                    return new Application.Business.Erp.SupplyChain.Client.WebMng.ReloadIrpXML().ShowDialog();
                case "收料入库单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.土建);
                case "收料入库单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.安装);

                case "单据修改":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.单据修改);

                case "收料入库红单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI.CStockInRed(framework).Excute(EnumStockExecType.土建);
                case "收料入库红单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI.CStockInRed(framework).Excute(EnumStockExecType.安装);
                case "收料入库单查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.stateSearch, TheAuthMenu);
                case "调拨入库单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI.CStockMoveIn(framework).Execute(EnumStockExecType.土建);
                case "调拨入库单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI.CStockMoveIn(framework).Execute(EnumStockExecType.安装);
                case "调拨入库红单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI.CStockMoveInRed(framework).Execute(EnumStockExecType.土建);
                case "调拨入库红单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI.CStockMoveInRed(framework).Execute(EnumStockExecType.安装);
                case "调拨入库单查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI.CStockMoveIn(framework).Execute(EnumStockExecType.StockMoveInQuery, TheAuthMenu);
                case "调拨出库单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.安装);
                case "调拨出库单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.土建);
                case "调拨出库红单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutRedUI.CStockMoveOutRed(framework).Excute(EnumStockExecType.安装);
                case "调拨出库红单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutRedUI.CStockMoveOutRed(framework).Excute(EnumStockExecType.土建);
                case "调拨出库单查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.StockMoveOutQuery, TheAuthMenu);
                case "闲置物资维护":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.闲置物资维护);
                case "公司调配查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.公司调配查询);
                case "验收结算单":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalUI.CStockInBal(framework).Excute(args);
                case "验收结算红单":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalRedUI.CStockInBalRed(framework).Excute(args);
                case "验收结算单查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalUI.CStockInBal(framework).Excute(EnumStockExecType.StockInBalQuery, TheAuthMenu);
                case "仓库收发台账":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI.CStockInRed(framework).Excute(EnumStockExecType.仓库收发台账);
                case "仓库收发存月报":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(EnumStockExecType.仓库收发存月报);
                case "收发存报表":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(EnumStockExecType.仓库收发存报表);
                case "成本对比分析表":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(EnumStockExecType.成本对比分析表);
                //return null;
                #endregion

                #region 料具租赁管理
                case "料具租赁合同":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder(framework).Excute(args);
                case "料具租赁合同查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder_ExecType.MaterialRentalOrderQuery);
                case "料具租赁合同复制":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder_ExecType.MaterialRentalOrderCopy);
                case "料具租赁合同引用":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder_ExecType.MaterialRentalOrderRef);
                case "料具收料单":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(args);
                case "收料单统计查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialCollectionQuery);
                case "料具退料单":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn.CMaterialReturn(framework, 1).Excute(1);
                case "料具退料单(损耗)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn.CMaterialReturn(framework, 2).Excute(2);
                case "退料单统计查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn.CMaterialReturn(framework, 0).Excute(CMaterialReturn_ExecType.MaterialReturnQuery);
                case "料具租赁流水账":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialRentalCurrentCount);
                case "料具租赁台账":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialRentalLedgerQuery);
                case "料具租赁月结":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialMonthlyBalance);
                case "料具租赁结算表":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialMonthlyQuery);
                case "料具租赁月报":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(EnumStockExecType.料具租赁);
                #endregion

                #region 废旧物资管理
                case "废旧物资申请单":
                    return new Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialOrder(framework).Excute(args);
                case "废旧物资申请查询":
                    return new Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialOrder_ExecType.WasteMatApplyQuery);
                case "废旧物资处理单":
                    return new Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialHandle(framework).Excute(args);
                case "废旧物资处理查询":
                    return new Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialHandle(framework).Excute(Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialHandle_ExecType.WasteMatHandleQuery);
                #endregion

                #region 商品砼管理
                case "浇筑记录单":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConPouringNoteMng.CConPouringNote(framework).Excute(args);
                case "浇筑记录统计查询":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConPouringNoteMng.CConPouringNote(framework).Excute(CConPouringNote_ExceType.ConPouringNoteQuery);
                case "抽磅单":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng.CPumpingPounds(framework).Excute(args);
                case "抽磅单统计查询":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng.CPumpingPounds(framework).Excute(CPumpingPounds_ExceType.PumpingPoundsQuery);
                case "对账单":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConCheckingMng.CConCheck(framework).Excute(args);
                case "对账单统计查询":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConCheckingMng.CConCheck(framework).Excute(CConCheck_ExceType.ConPouringNoteQuery);
                case "商品砼结算单":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.CConBalance(framework).Excute(args);
                case "商品砼结算红单":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.ConBalanceRedUI.CConBalanceRed(framework).Excute(args);
                case "商品砼结算查询":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.CConBalance(framework).Excute(CConBalance_ExceType.ConBalanceQuery);
                #endregion

                #region 需求总计划管理
                case "需求总计划单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.安装);
                case "需求总计划单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.土建);
                case "需求总计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.demandSearch, TheAuthMenu);
                case "需求总计划查询(公司)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.companyDemandSearch);
                case "采购成本统计表":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.companySupplyPlan);
                #endregion

                #region 月度需求计划管理
                case "月度需求计划单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng.CMonthlyPlanMng(framework).Excute(EnumMonthlyType.安装);
                case "月度需求计划单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng.CMonthlyPlanMng(framework).Excute(EnumMonthlyType.土建);
                case "月度需求计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng.CMonthlyPlanMng(framework).Excute(EnumMonthlyType.monthlySearch, TheAuthMenu);

                #endregion

                #region 日常需求计划管理
                case "日常需求计划单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng.CDailyPlanMng(framework).Excute(EnumDailyType.土建);
                case "日常需求计划单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng.CDailyPlanMng(framework).Excute(EnumDailyType.安装);
                case "日常需求计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng.CDailyPlanMng(framework).Excute(EnumDailyType.dailySearch, TheAuthMenu);
                #endregion

                #region 采购合同管理
                case "采购合同单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng.CSupplyOrderMng(framework).Excute(EnumSupplyType.土建);
                case "采购合同单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng.CSupplyOrderMng(framework).Excute(EnumSupplyType.安装);
                case "采购合同查询":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng.CSupplyOrderMng(framework).Excute(EnumSupplyType.supplySearch);
                case "采购合同单(公司)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMngCompany.CSupplyOrderMngCompany(framework).Excute();
                case "采购合同查询(公司)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMngCompany.CSupplyOrderMngCompany(framework).Excute(EnumSupplyType.supplySearch);
                #endregion

                #region 采购合同调价管理
                //case "采购合同调价单":
                //    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPriceMng(framework).Excute(args);
                case "采购合同调价":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPriceMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPrice_ExecType.ContractAdjustPriceQuery);
                case "采购合同调价查询":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPriceMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPrice_ExecType.ContractAdujustPriceQueryNew);
                #endregion

                #region 采购计划管理
                case "专业需求计划":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng.CSupplyPlanMng(framework).Excute(args);
                case "专业需求计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng.CSupplyPlanMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng.CSupplyPlanMng_ExecType.SupplyPlanMngQuery);
                #endregion

                #region 工程更改管理
                case "工程更改信息":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.EngineerChangeMng.CEngineerChangeMng(framework).Excute(args);
                case "工程更改查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.EngineerChangeMng.CEngineerChangeMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.EngineerChangeMng.CEngineerChangeMng_ExecType.EngineerChangeQuery);
                #endregion

                #region 罚款单
                case "罚款单维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(args);
                case "罚款单查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng_ExecType.PenaltyDeductionQuery);
                case "扣款单查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng_ExecType.PenaltyQuery);
                case "罚款核算单":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng_ExecType.PenaltyDeductionSelect);
                case "暂扣款单维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng_ExecType.TempDedit);
                #endregion

                #region 项目基本信息管理
                case "项目基本信息维护":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectDepartment);
                case "项目基本信息查询":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectDepartmentSearch);
                case "项目业务信息":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectBusiInfo);
                case "项目商务状态维护":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectBusinessStateMng);
                case "项目工程状态维护":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectStateMng);
                case "项目物资排名维护":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectMaterialStateMng);

                case "项目使用状态报告":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectStateReport);
                case "项目综合数据统计":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectDataInfo);
                case "公司项目使用状况统计":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.AllProjectState);
                case "工程项目维护":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.OrgAsProjectInfo);
                #endregion

                #region 劳务需求计划
                case "劳务需求计划":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng.CLaborDemandPlan(framework).Excute(args);
                case "劳务需求计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng.CLaborDemandPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng.CLaborDemandPlan_ExecType.LaborDemandPlanQuery);
                #endregion

                #region 月度盘点管理
                //case "盘点单":
                //    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(args);

                case "盘点单(土建)":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(CStockInventoryMng_ExecType.土建);
                case "盘点单(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(CStockInventoryMng_ExecType.安装);
                case "仓库报表(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(CStockInventoryMng_ExecType.仓库报表);
                case "盘点单查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng_ExecType.StockInventoryQuery);
                #endregion

                #region 零星用工管理
                case "逐日派工单维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.逐日派工);
                case "零星用工单维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.派工);
                case "分包签证单维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.分包签证);
                case "计时派工单维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.计时派工);
                case "代工单维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.代工);
                case "代工单核算":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborSporadicSelector);
                case "零星用工单核算":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborSporadicSelect);
                case "分包签证单审核":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.SubPackageVisaSelect);
                case "计时派工单核算":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.TimeDespatching);
                case "零星用工单生产查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborSporadicSHCQuery);
                case "零星用工单商务查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborSporadicQuery);
                case "代工单查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborQuery);
                #endregion

                #region 检测回执管理
                case "检测回执单":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.DetectionReceiptMng.CDetectionReceiptMng(framework).Excute(args);
                case "检测回执查询":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.DetectionReceiptMng.CDetectionReceiptMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.StockMng.DetectionReceiptMng.CDetectionReceiptMng_ExecType.DetectionReceiptQuery);
                #endregion

                #region 检验批
                case "检验批":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng.CInspectionLotMng(framework).Excute(args);
                case "检验批查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng.CInspectionLotMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng.CInspectionLotMng_ExecType.InspectionLotQuery);
                #endregion

                #region 整改通知管理
                case "整改通知单":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNoticeMng(framework).Excute(args);
                case "整改通知查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNoticeMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNotice_ExecType.RectificationNoticeQuery);
                case "整改单复核":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNoticeMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNotice_ExecType.RectificationNoticeSelector);
                #endregion

                #region 费用结算管理
                case "费用结算单维护":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng.CExpensesSettleMng(framework).Excute(args);
                case "费用结算单查询":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng.CExpensesSettleMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng.CExpensesSettleMng_ExecType.ExpensesSettleQuery);
                #endregion

                #region 物资耗用结算管理
                case "材料耗用结算单维护":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng.CMaterialSettleMng(framework).Excute(EnumMaterialSettleType.物资耗用结算单维护);
                case "料具租赁结算单维护":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng.CMaterialSettleMng(framework).Excute(EnumMaterialSettleType.料具结算单维护);
                case "材料耗用结算单查询":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng.CMaterialSettleMng(framework).Excute(EnumMaterialSettleType.materialSettleQuery);
                case "料具租赁结算单查询":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng.CMaterialSettleMng(framework).Excute(EnumMaterialSettleType.materialQuery);
                #endregion

                #region 验收检查记录
                case "验收检查记录":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng(framework).Excute(args);
                case "验收检查记录查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng_ExecType.AcceptanceInspectionQuery);
                case "验收检查":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng_ExecType.Acceptance);
                #endregion

                #region 项目工程复制
                case "项目工程复制":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectCopyMng.CProjectCopy(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectCopyMng.CProjectCopy_ExecType.ProjectCopy);
                #endregion

                #region OBS
                case "管理OBS":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OBS.COBS(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.OBS.CDOBS_ExecType.OBSManage);
                case "服务OBS":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OBS.COBS(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.OBS.CDOBS_ExecType.OBSService);
                #endregion

                #region 专项管理费用
                case "专项管理费用单":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost.CSpecialCost(framework).Excute(args);
                case "专项管理费用查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost.CSpecialCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost.CSpecialCostOrder_ExecType.SpecialCostQuery);

                case "专项费用结算单":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount.CSpecialAccount(framework).Excute(args);
                case "专项费用结算查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount.CSpecialAccount(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount.CSpecialAccount_ExecType.SpecialAccountQuery);

                #endregion

                #region 分包项目管理
                case "分包执行状态维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng.CContractExcuteMng(framework).Excute(args);
                case "分包执行状态查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng.CContractExcuteMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng.CContractExcuteMng_ExecType.ContractExcuteQuery);
                #endregion

                #region 总量需求计划管理
                case "总量需求计划单":
                    return new Application.Business.Erp.SupplyChain.Client.TotalDemandPlan.CTotalDemandPlanMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.TotalDemandPlan.CDemandPlanMng_ExecType.TotalDemandPlanQuery);
                case "需求总量分析单":
                    return new Application.Business.Erp.SupplyChain.Client.TotalDemandPlan.CTotalDemandPlanMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.TotalDemandPlan.CDemandPlanMng_ExecType.TotalDemandAnalysis);
                #endregion

                #region 业务报量
                case "业主报量单维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng(framework).Excute(args);
                case "业主报量单查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng_ExecType.OwnerQuantityQuery);
                case "业主报量状态查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng_ExecType.OwnerQuantitySearch);
                #endregion

                #region 财务管理
                case "个人借款单":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.DelimitIndividualBillMng.CDelimitIndividualBillMng(framework).Excute(args);
                case "个人借款查询":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.DelimitIndividualBillMng.CDelimitIndividualBillMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.DelimitIndividualBillMng.CDelimitIndividualBillMng_ExecType.DelimitIndividualBillQuery);

                case "费用划账单":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng.CExpensesRowBillMng(framework).Excute(args);
                case "费用划账查询":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng.CExpensesRowBillMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng.CExpensesRowBillMng_ExecType.ExpensesRowBillQuery);
                case "费用报销单":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesSingleBillMng.CExpensesSingleBillMng(framework).Excute(args);
                case "费用报销查询":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesSingleBillMng.CExpensesSingleBillMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesSingleBillMng.CExpensesSingleBillMng_ExecType.ExpensesSingleBillQuery);

                case "临建摊销单":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.OverlayAmortizeMng.COverlayAmortizeMng(framework).Excute(args);
                case "临建摊销查询":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.OverlayAmortizeMng.COverlayAmortizeMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.OverlayAmortizeMng.COverlayAmortizeMng_ExecType.OverlayAmortizeQuery);
                case "当期收益结算单":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.IncomeSettlementMng.CIncomeSettlementMng(framework).Excute(args);
                case "当期收益结算单查询":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.IncomeSettlementMng.CIncomeSettlementMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.IncomeSettlementMng.CIncomeSettlementMng_ExecType.IncomeSettlementQuery);

                case "材料结算单":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.MaterialsettlementMng.CMaterialSettlementMng(framework).Excute(args);
                case "材料结算查询":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.MaterialsettlementMng.CMaterialSettlementMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.MaterialsettlementMng.CMaterialSettlementMng_ExecType.MaterialSettlementQuery);
                #endregion

                #region 施工专业基础数据
                case "施工专业基础表":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ConstructionDataMng.CConstructionData(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ConstructionDataMng.CWasteMaterialHandle_ExecType.ConstructionData);
                #endregion

                #region 专业检查记录
                case "日常检查":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng.CProInsRecord(framework).Excute(args);
                case "日常检查查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng.CProInsRecord(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng.CProInsRecord_ExecType.ProInsRecordQuery);

                #endregion

                #region 基础数据管理
                case "单据默认计量单位配置":
                    return new Application.Business.Erp.SupplyChain.Client.BasicData.UnitMng.CUnitMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.BasicData.UnitMng.CUnitMng_ExecType.UnitMng);
                #endregion

                #region 基础数据导入
                case "基础数据导入":
                    return new Application.Business.Erp.SupplyChain.Client.ExcelImportMng.CExcelImportMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ExcelImportMng.CExcelImportMng_ExecType.VExcelImport);
                case "导入基础数据":
                    return new Application.Business.Erp.SupplyChain.Client.ExcelImportMng.CExcelImportMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ExcelImportMng.CExcelImportMng_ExecType.VExcelImportMng);
                #endregion

                #region 设备租赁结算管理
                case "机械租赁结算单维护":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng.CMaterialRentalSettlement(framework).Excute(args);
                case "机械租赁结算单查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng.CMaterialRentalSettlement(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng.CMaterialRentalSettlementPlan_ExecType.MaterialRentalSettlementQuery);
                case "机械租赁合同维护":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalContractMng.CMaterialRentalContract(framework).Excute(args);
                #endregion

                #region 公用

                case "表单单号规则维护":
                    return new CommonSearch.BillCodeRuleMng.VBillCodeRule().ShowDialog();
                case "权限设置":
                    //return new VAuthConfig(ConstObject.TheLogin.ThePerson.Code).ShowDialog();
                    return new VAuthConfigByRole(ConstObject.TheLogin.ThePerson.Code).ShowDialog();
                case "数据权限设置":
                    //return new VAuthConfig(ConstObject.TheLogin.ThePerson.Code).ShowDialog();
                    return new VAuthConfigOrgSysCode().ShowDialog();
                case "查询设置":
                    //return new CommonSearchDesign().ShowDialog();
                    return null;
                case "数据字典":
                    //return new VClassDict().ShowDialog();
                    return null;
                case "审批定义":
                    //return new ApprovalManager.AppDefineMng.VAppDefine().ShowDialog();
                    return null;
                case "审批平台":
                    //new ApprovalManager.AppDataMng.VAppDataView(ConstObject.TheLogin.ThePerson.Code).Show();
                    return null;
                #endregion

                #region 在线维护
                case "系统问题":
                    return new CustomServiceClient.QuestionMng.CQuestion(framework).Excute(35, ConstObject.LoginPersonInfo.Name, args);
                case "系统问题查询":
                    return new CustomServiceClient.QuestionMng.CQuestion(framework).Excute(35, ConstObject.LoginPersonInfo.Name, QuestionType.search);
                #endregion

                #region 审批平台
                case "审批单据定义":
                    return new CAppTableSet(framework).Excute(control, args);
                case "审批属性定义":
                    return new CAppSolutionSet(framework).Excute(control, CAppSolutionSet_ExecType.AppPropertySet);
                case "审批方案维护":
                    return new CAppSolutionSet(framework).Excute(control, CAppSolutionSet_ExecType.AppSolutionSet);
                case "业务审批平台":
                    return new CAppPlatform(framework).Excute(control, args);
                case "业务单据修改":
                    return new CAppPlatform(framework).Excute(control, CAppPlatform_EnumType.SetBill);
                case "审批状态查询":
                    return new CAppStatusQuery(framework).Excute(control, args);
                case "审批状态单据查询":
                    return new CAppStatusQuery(framework).ExcuteByBill(args[0], args[0].ToString());
                case "单据审批查询":
                    return new CAppPlatform(framework).Excute(control, CAppPlatform_EnumType.ApproveQuery);
                #endregion

                #region 成本核算/PIM管理
                case "施工部位划分维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.施工部位划分维护);
                case "PBS节点维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.PBS节点维护);
                case "施工部位模板维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.施工部位模板维护);
                case "施工部位结构类型维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.施工部位结构类型维护);
                case "特性集维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.特性集维护);
                case "文档对象管理":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.文档对象管理);
                case "施工任务类型":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.施工任务类型);
                case "任务类型模版维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.任务类型模版维护);
                case "初始化项目任务文档模板":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.初始化项目任务文档模板);
                case "合约商务信息维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng.CWBSContractGroup(framework).Excute(OperationContractGroupType.合约商务信息维护);
                case "施工任务划分维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.施工任务划分维护);
                case "施工任务划分维护_新":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.施工任务划分维护_新);
                case "工程成本维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.工程成本维护);
                case "工程成本批量维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.工程成本批量维护);
                case "签证变更台账查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.签证变更台账查询);
                case "项目任务综合查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.项目任务综合查询);
                case "工程成本维护综合查询"://工程任务合价查询
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.工程成本维护综合查询);
                case "现场生产管理":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.现场生产管理);
                case "质量验收检查":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.质量验收检查);
                case "新现场生产管理":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.新现场生产管理);
                case "现场生产管理(现场)":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.现场生产管理_现场);
                case "工程量提报查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.工程量提报查询);
                //case "工程WBS明细编辑":
                //    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.工程WBS明细编辑);

                case "成本项分类":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng.CCostItemCategory(framework).Excute(OperationCostItemCategoryType.成本项分类);
                case "成本项分类导入":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng.CCostItemCategory(framework).Excute(OperationCostItemCategoryType.成本项分类导入);
                case "成本项维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng.CCostItem(framework).Excute(OperationCostItemType.成本项维护);
                case "成本项导入":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng.CCostItem(framework).Excute(OperationCostItemType.成本项导入);
                case "成本核算科目":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng.CCostItemCategory(framework).Excute(OperationCostAccountSubjectType.成本核算科目);
                case "清单任务划分维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS.CQWBSManagement(framework).Excute(OperationQWBS.清单WBS管理);
                #endregion

                #region 生产管理
                case "总进度计划":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(args);
                case "任务单查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.任务单查询);
                case "总进度计划审批":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.总进度计划审批);
                case "总滚动进度计划":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(EnumScheduleType.总滚动进度计划);
                case "进度计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(EnumScheduleType.进度计划查询, TheAuthMenu);
                case "进度计划编制":
                    //return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumExecScheduleType.月度进度计划);
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumExecScheduleType.年度进度计划);
                case "工区周计划维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumExecScheduleType.周进度计划);
                case "月计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.月计划查询);
                case "周计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.周计划查询);
                case "项目周计划维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.项目周计划维护);
                case "任务单维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.任务单维护);
                case "项目周计划确认":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.周计划确认);
                case "工程量确认单维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI.CGWBSConfirm(framework).Excute();
                case "工程量确认单维护_非计划":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI.CGWBSConfirm(framework).Excute(EnumProductionMngExecType.工程量确认单维护_非计划);
                case "工程量确认单查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI.CGWBSConfirm(framework).Excute(EnumProductionMngExecType.工程任务确认单查询);
                case "总需求计划":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(OperationRollingDemandPlanType.总需求计划);
                case "总滚动需求计划":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(OperationRollingDemandPlanType.总滚动需求计划);
                case "生成执行需求计划":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(args);
                case "执行需求计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(OperationRollingDemandPlanType.执行需求计划查询);
                case "日常检查记录":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng.CInspectionRecord(framework).Excute(CInspectionRecord_ExecType.InspectionRecord);
                case "质量验收检查查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng.CInspectionRecord(framework).Excute(CInspectionRecord_ExecType.InspectionRecordQuery);
                case "资源需求管理":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.OperationRollingDemandPlanType.资源需求管理);
                case "资源需求计划管理":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework, true).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.OperationRollingDemandPlanType.资源需求计划管理);
                case "期间需求计划单维护":
                    return new Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan.CPeriodRequirePlanMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan.CPeriodRequirePlanMng_ExecType.期间需求计划单维护);
                case "工期预警":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(EnumProductionMngExecType.工期预警);
                case "劳动力预测统计":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(EnumProductionMngExecType.劳动力预测统计);
                #endregion

                #region 现场商务管理
                case "工程任务核算维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccount(framework).Excute(args);
                case "工程任务核算单维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccount(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccountExecType.工程任务核算维护_虚拟工单);
                case "工程量确认单":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccount(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccountExecType.工程量确认单);
                case "工程任务核算查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccount(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccountExecType.工程任务核算查询, TheAuthMenu);
                case "分包结算单维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(args);
                case "分包结算单查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.分包结算查询, TheAuthMenu);
                case "单据核算科目调整":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.单据核算科目调整, TheAuthMenu);
                case "月度成本核算维护":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthAccount);
                case "月度成本核算(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthAccountSpecial);
                case "月度成本核算日志":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthLog);
                case "月度成本核算查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthQuery);
                case "月度核算综合查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthGenQuery);
                case "生产施工统计报表":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ProduceReport);
                case "月度成本分析报表":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthReport);
                case "新月度成本分析报表":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthReportNew);
                case "月度成本分析对比报表":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.CostMonthCompareReport);
                case "月度成本报表(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SpecialReport);
                case "商务报表(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SpecialBusinessReport);
                case "经济效益统计":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.EconomyProfit);
                case "成本消耗统计":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.CostConsume);
                case "技经指标统计":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.TechIndicator);
                case "收入成本统计":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.IncomeCost);
                case "财务收款统计":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ReceiveMoney);
                case "签证变更统计":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ContractLedger);
                case "商务报表统计":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.BusinessReport);
                case "分包合同台账":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SubcontractsLedger);
                case "项目结算罚款台账":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.FineAccountReport);
                case "项目代工扣款台账":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.OEMChargebackReport);
                case "合同交底样稿":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ContractDisclosure);
                case "合同交底查询":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ContractDisclosureQuery);
                case "项目分包结算台账":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SubcontractSettlementReport);
                case "机械租赁结算台账":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MaterialRentalSettlementReport);
                case "分包单位工程量台账":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SubcontractAmountReport);
                case "商务报表填报":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.CommercialReport);
                case "项目业主确权台账":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ConfirmRightReport);
                case "商务报表填报统计":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.CommercialReportQuery);
                case "机械费成本分析对比表":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MechanicalCostComparisonRpt);
                case "取费模板定义":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SelFeeTemplate);
                case "多维度对比表":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.多维度对比表);
                case "项目分包结算生成":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.项目分包结算生成);
                case "现场管理费分析对比表"://20160819 HJ
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SceneManageFeelReport);
                case "分包终结结算单生成"://20160904 HJ
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.分包终结结算单生成);
                case "分包终结结算单审核"://20160904 HJ
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.分包终结结算单审核);
                #endregion

                #region 指标管理
                case "维度维护":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.CDimensionCategory(framework).Excute(args);
                case "主题定义":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse.CCubeDefine(framework).Excute(args);
                case "通用报表定义":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse.CViewDefFree(framework).Excute(args);
                case "通用报表分发":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse.CViewFreeDistribute(framework).Excute(args);
                case "报表数据录入":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse.CViewFreeWrite(framework).Excute(args);
                #endregion

                #region 文件上传
                case "文件上传测试":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.文件上传测试);
                case "文件上传":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload.CFilesUpLoad(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload.CFilesUpLoaad_ExecType.VFilesUpLoad);
                #endregion

                #region 测试菜单
                case "测试菜单":
                    return new CWebMng(framework).Excute("IRP同步");
                #endregion

                #region 日施工管理
                case "晴雨信息":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeather(framework).Excute(args);
                case "晴雨信息查询":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeather(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeatherMng_ExecType.WeatherQuery);
                case "二维码管理":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeather(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeatherMng_ExecType.QRCode);
                case "管理人员日志":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement.CPersonManage(framework).Excute(args);
                case "管理人员日志查询":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement.CPersonManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement.CPersonManageMng_ExecType.PersonManageQuery);
                case "施工日志":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement.CConstruction(framework).Excute(args);
                case "施工日志查询":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement.CConstruction(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement.CConstructionMng_ExecType.ConstructionQuery);

                case "日施工报告":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport.CConstructionReport(framework).Excute(args);
                case "日施工报告查询":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport.CConstructionReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport.CConstructionReportMng_ExecType.ConstructReportQuery);

                #endregion

                #region 竣工结算
                case "竣工结算单":
                    return new Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook.CCompleteMng(framework).Excute(args);
                case "竣工结算查询":
                    return new Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook.CCompleteMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook.CCompleteMng_ExecType.CompleteQuery);
                #endregion

                #region 物资价维护
                case "信息价格维护":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.CProgramManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.CProgramManageMng_ExecType.ProgramManage);

                #endregion

                #region 周进度计划
                case "周进度计划":
                    //return new Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules.CWeekPlanEntry(framework).Excute(EnumWeekPlanMngExecType.周进度计划);
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules.CWeekPlanEntry(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules.EnumWeekPlanMngExecType.周进度计划);
                #endregion

                #region 施工任务信息查询
                case "施工任务信息查询":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage.CGwbsManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage.CGwbsManage_ExecType.VGwbsManage);
                #endregion

                #region 常用短语维护
                case "常用短语维护":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords.COftenWords(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords.COftenWords_ExecType.VOftenWords);
                case "测试界面":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords.COftenWords(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords.COftenWords_ExecType.VTest);
                #endregion

                #region 项目实施策划书
                case "项目实施策划书":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan.CImplementationPlan(framework).Excute(args);
                case "项目实施策划查询":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan.CImplementationPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan.CImplementationPlan_ExexType.ImplementtationSearch);
                #endregion

                #region 项目目标责任书
                case "目标责任书":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng.CTargetRespBook(framework).Excute(args);
                case "目标责任书查询":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng.CTargetRespBook(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng.EnumTargetRespBook.TargetRerspBookSearch);
                #endregion

                #region 项目文档管理
                case "文档管理":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList_ExecType.VDocumentLisBak);
                case "文档分类编码前缀映射配置":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList_ExecType.文档分类编码前缀映射配置);
                case "对象类型关联文档分类配置":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList_ExecType.对象类型关联文档分类配置);
                case "文档分类维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory_ExecType.VDocumentCategory);
                case "文件柜详细信息维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory_ExecType.VDocumentCategoryManage);
                case "文档综合查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory_ExecType.VDocumentsSelect);

                #endregion
                //#region 项目文档管理
                //case "工程文档审批单":
                //    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList_ExecType.VDocumentList);
                //#endregion

                #region 收发函管理
                case "收发函信息":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage.CCollectionManage(framework).Excute(args);
                case "收发函查询":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage.CCollectionManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage.CCollectionManage_ExexType.CollectionManageSearch, TheAuthMenu);
                #endregion

                #region 会议纪要管理
                case "会议纪要信息":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary.CMeetingManage(framework).Excute(args);
                case "会议纪要查询":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary.CMeetingManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary.CMeetingManage_ExexType.MeetingManageSearch);
                #endregion

                #region 管控指标和预警
                case "状态检查动作维护":
                    return new Application.Business.Erp.SupplyChain.Client.PMCAndWarning.CPMCAndWarning(framework).Excute(OperationPMCAndWarningType.状态检查动作维护);
                case "预警信息推送方式配置":
                    return new Application.Business.Erp.SupplyChain.Client.PMCAndWarning.CPMCAndWarning(framework).Excute(OperationPMCAndWarningType.预警信息推送方式配置);
                case "预警服务控制台":
                    return new Application.Business.Erp.SupplyChain.Client.PMCAndWarning.CPMCAndWarning(framework).Excute(OperationPMCAndWarningType.预警服务控制台);
                case "预警信息查询":
                    return new Application.Business.Erp.SupplyChain.Client.PMCAndWarning.CPMCAndWarning(framework).Excute(OperationPMCAndWarningType.预警信息查询);
                #endregion

                #region 项目策划管理
                case "施工组织设计维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage.CContructionDesign(framework).Excute(args);
                case "施工组织设计查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage.CContructionDesign(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage.EnumContructionDesign.ContructionDesignSearch);
                case "专业策划维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.WebSitePlanManage.CWebSitePlan(framework).Excute(args);
                case "专业策划查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.WebSitePlanManage.CWebSitePlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.WebSitePlanManage.EnumWebSitePlan.WebSitePlanSearch);
                case "商务策划维护":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage.CBusinessProposal(framework).Excute(args);
                case "商务策划查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage.CBusinessProposal(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage.EnumBusinessPropoasal.BusinessProposalSeaech);
                #endregion

                #region 工程文档审批管理
                case "工程文档审批维护":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng.CDocApprovalMng(framework).Excute(args);
                case "工程文档审批查询":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng.CDocApprovalMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng.CDocApprovalMng_ExexType.DocApprovalManageSearch);
                #endregion

                #region 整改单确认
                case "整改单确认":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection.CDailyCorrection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection.CDailyCorrection_ExecType.VDailyCorrectionMaster);
                #endregion

                #region 整改单查询
                case "整改单查询":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrectioSearch.CDailyCorrectioSearch(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrectioSearch.CDailyCorrectionSearch_ExecType.VDailyCorrectionSearch);
                #endregion

                #region 查询周计划任务
                case "周计划任务查询":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.CProjectTaskQuery(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.EnumProjectTask.WeekScheduleSearch);
                #endregion

                #region 专业检查
                case "专业检查":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.ProInRecord.CProInRecordMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.ProInRecord.CProInRecordMng_exectype.VProInRecordMng);
                #endregion

                #region 产值汇总查询
                case "产值汇总查询":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue.COutPutValue(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue.COutPutValue_ExexType.RealOutputValueQuery);
                case "产值汇总查询[到计划节点]":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue.COutPutValue(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue.COutPutValue_ExexType.OutPutValueQuery);
                #endregion

                #region 在线帮助
                case "在线帮助":
                    return new Application.Business.Erp.SupplyChain.Client.HelpOnline.CHelpOnline(framework).Excute(Application.Business.Erp.SupplyChain.Client.HelpOnline.CHelpOnline_ExexType.HelpOnline);
                #endregion

                #region 资金管理
                case "票据台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.票据台账);
                case "费用信息查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.费用信息查询);
                case "项目财务账面维护":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.项目财务账面维护);
                case "项目财务账面维护(安装)":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.项目财务账面维护安装);
                case "项目商务成本确认":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.项目商务成本确认);

                case "项目财务账面维护查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.项目财务账面维护查询);
                case "主营业务收入台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.主营业务收入台账);
                case "项目成本支出台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.项目成本支出台账);
                case "公司财务账面维护":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.公司财务账面维护);
                case "公司财务账面维护查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.公司财务账面维护查询);
                case "分公司财务账面维护":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.分公司财务账面维护);
                case "分公司财务账面维护查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.分公司财务账面维护查询);
                case "管理费用台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.管理费用台账);
                case "费用信息维护":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.费用信息维护);
                case "间接费用台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.间接费用台账);
                case "甲方审量及应收台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.甲方审量及应收台账);
                case "收款单":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.Gathering);
                case "收款单查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.GatheringQuery);
                case "收款单(非工程款)":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.GatheringOther);
                case "收款台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.收款台账);
                case "应收拖欠款台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.应收拖欠款台账);
                case "付款单":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.Payment);
                case "付款数据初始化":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.PaymentIntial);
                case "保证金导入":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.DepositImport);
                case "付款单查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.PaymentQuery);
                case "付款单(非工程款)":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.PaymentOther);
                case "付款发票维护":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentInvoice(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentInvoiceType.PaymentInvoice);
                case "付款发票抵扣维护":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentInvoice(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentInvoiceType.付款发票抵扣维护);
                case "付款发票查询":
                    return new MoneyManage.FinanceMultData.CPaymentInvoice(framework).Excute(MoneyManage.FinanceMultData.CPaymentInvoiceType.PaymentInvoiceQuery);
                case "付款发票台账":
                    return new MoneyManage.FinanceMultData.CPaymentInvoice(framework).Excute(MoneyManage.FinanceMultData.CPaymentInvoiceType.PaymentInvoiceReport);
                case "日现金流统计":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.日现金流统计);
                case "关键指标计算":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.关键指标计算);
                case "财务账面台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.财务账面台账);
                case "项目资金计划申请":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.项目资金计划申请);
                case "分公司资金计划申请":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.分公司资金计划申请);
                case "资金计划查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.资金计划查询);
                case "资金计划分配":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.资金计划分配);

                case "借款单":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CBorrowedOrder(framework).Excute();
                case "借款单查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CBorrowedOrder(framework).Excute(EnumBorrowedOrder.借款单查询);

                case "保理单":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFactoringData(framework).Excute();
                case "保理台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFactoringData(framework).Excute("Report");
                case "项目收支分析表":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.工程项目收支分析表);
                case "资金结账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.资金结账);
                case "时间期间定义":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.时间期间定义);
                case "保证金/押金台账":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.收付款保证金及押金台账);
                case "施工节点进度维护":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.施工节点进度维护);
                case "施工节点进度查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.施工节点进度查询);
                case "项目启动确认":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.项目启动确认);
                case "资金策划表":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.资金策划表);
                case "资金策划表查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.资金策划表查询);
                case "资金测算表":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.资金测算表);
                case "资金策划表审批":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.资金策划表审批);
                case "非实体进度设置":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.非实体进度设置);
                case "资金计划申报":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金计划申报);
                case "资金计划审批":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金计划审批);
                case "资金支付审批单":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金支付审批单);
                case "资金支付审批单审核":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金支付审批单审核);
                case "资金支付申请单编制":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金支付申请单编制);
                case "资金支付申请单审核":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金支付申请单审核);
                case "资金支付申请单查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金支付申请单查询);
                case "资金利息计算":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金利息计算);
                case "资金考核查询":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金考核查询);
                case "资金考核兑现":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金考核兑现);
                case "资金策划成效分析":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.资金策划成效分析);

                #endregion

                #region 新料具
                case "新料具合同":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.料具租赁合同);
                case "新料具合同查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.料具租赁合同查询);
                case "新料具租赁合同复制":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.料具租赁复制);
                case "新料具租赁合同引用":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.料具租赁合同引用);
                case "料具封存单":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.料具封存单);
                case "料具封存单(钢管)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.钢管封存单);
                case "料具封存单(碗扣)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.碗扣封存单);
                case "料具封存单查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.料具封存单查询);

                case "新料具发料单":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(args);
                case "新料具发料单(钢管)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireCollectionGGCheck);
                case "新料具发料单(碗扣)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireCollectionWKCheck);
                case "新发料单统计查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireCollectionQuery);
                case "新料具租赁月结":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireMonthlyBalance);
                case "新料具租赁月结查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireMonthlyQuery);
                case "新料具退料单":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireReturn);
                case "新料具退料单(损耗)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireReturnLoss);
                case "新料具退料单(钢管)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireGGReturn);
                case "新料具退料单(钢管)(损耗)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireGGReturnLoss);
                case "新料具退料单(碗扣)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireWKReturn);
                case "新料具退料单(碗扣)(损耗)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireWKReturnLoss);
                case "新退料单统计查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireReturnQuery);
                case "一米以下钢管退料查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireReturnGGLessOneQuery);
                case "运输单":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost.CMaterialHireTransportCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost.CMaterialHireTransportCost_ExecType.运输费);
                case "运输单查询":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost.CMaterialHireTransportCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost.CMaterialHireTransportCost_ExecType.运输费查询);
               
               
                case "新料具租赁台账":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.料具租赁台账);
                case "料具损耗情况表":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.料具损耗情况表);
                case "租赁结算台帐":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.租赁结算台帐);
                case "尺寸分段统计表":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.尺寸分段统计表);
                case "料具分布报表":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.料具分布报表);
                
                #endregion

                #region
                //case "子功能测试" :
                //    return new Application.Business.Erp.SupplyChain.Client.Test.CTest(framework).Execute("子功能测试");
                #endregion
            }
            return null;
        }
    }
}