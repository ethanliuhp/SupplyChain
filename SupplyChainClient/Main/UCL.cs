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
        /// ��ǰȨ�޲˵�
        /// </summary>
        public static AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu
        {
            get { return UCL._theAuthMenu; }
            set { UCL._theAuthMenu = value; }
        }

        public static object Locate(string control, params object[] args)
        {
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.����Ŀ && (control == "�����ù���ά��" || control == "�����ù�������"
                || control == "����������㵥ά��" || control == "ʩ����λ����ά��" || control == "ʩ�����񻮷�ά��" || control == "����������㵥ά��" || control == "�ֳ���������"))
            {
                MessageBox.Show("�°�2.0������ʹ�ô˹��ܣ�");
                return null;
            }
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.����Ŀ && (control == "PBS�ڵ�ά��" || control == "ʩ�����񻮷�ά��_��"
               || control == "���ֳ���������" || control == "������ȷ�ϵ�"))
            {
                MessageBox.Show("�ϰ�1.0������ʹ�ô˹��ܣ�");
                return null;
            }
            if (projectInfo.Code == CommonUtil.CompanyProjectCode)
            {
                if (control == "��Ŀ������Ϣά��" || control == "ʩ����λ����ά��"
                    || control == "ʩ�����񻮷�ά��" || control == "ʩ�����񻮷�ά��_��" || control == "�嵥���񻮷�ά��"
                    || control.Contains("�ճ�����") || control.Contains("ҵ��������ά��") || control.Equals("�տ") || control.Equals("���"))
                {
                    MessageBox.Show("��½λ��Ϊ[��˾/�ֹ�˾��Ŀ��������],���ܽ������ҵ�������");
                    return null;
                }
            }
            switch (control)
            {
                case "��ʼҳ":
                    new CStartPage(framework).Start();
                    break;

                #region ��Դ����
                case "����ڼ�ά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.CFiscalPeriod(framework).Excute(args);
                case "��Դ����ά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CMaterialCategory(framework).Excute(args);
                case "���Ϻ����ɫά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CAccountRole(framework).Excute(args);
                case "��Դά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CMaterial(framework).Excute(args);
                case "��Դ��ѯ":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CMaterial(framework).Excute(MaterialExcuteType.MaterialSearch);
                case "������λά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CStandardUnit(framework).Excute(args);
                case "������λ����ѡ��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CStandardUnit(framework).Excute(args);
                case "��Աά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource.CPerson(framework).Excute(args);
                case "��Ա�ϸ�ά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource.CPersonOnJob(framework).Excute(args);
                case "��Ա��ְά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource.CEmpOnPost(framework).Excute(args);
                case "ҵ����֯ά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COperationOrgTree(framework).Excute(args);
                case "ҵ���λά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COperationJob(framework).Excute(args);
                case "������֯ά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COrganization(framework).Excute(args);
                case "����ְλά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.CPoliticalPost(framework).Excute(args);
                case "���ʵ��ά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.CPartner(framework).Excute(args);
                case "�ͻ�����ά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement.CCustomerRelationCategory(framework).Excute(args);
                case "�ͻ���ϵά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement.CCustomerRelation(framework).Excute(args);
                case "��Ӧ����ά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.SupplierManagement.CSupplierRelationCategory(framework).Excute(args);
                case "��Ӧ��ϵά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.SupplierManagement.CSupplierRelation(framework).Excute(args);
                case "���ʵ���ѯ":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.CPartner(framework).Excute(PartnerExecuteType.ObjectSearch);
                case "������Ϣά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.BankInfoUI.CBankInfo(framework).Excute(args);
                case "�����ʺ�":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.BankAcctUI.CBankAcct(framework).Excute(args);
                case "֧��ҵ������":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.BusTypeUI.CBusType(framework).Excute(args);
                case "�ʽ��Ŀ����":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.Financial.MoneyVsAcctTitUI.CMoneyVsAcctTit(framework).Excute(control, args);
                case "���ά��":
                    return new Application.Business.Erp.SupplyChain.Client.BasicData.ClassesMng.CClasses(framework).Excute(args);
                case "����ά��":
                    return new Application.Business.Erp.SupplyChain.Client.BasicData.ClassTeamMng.CClassTeam(framework).Excute(args);
                case "��ƿ�Ŀ":
                    //return new Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI.CAccountTitle(framework).Excute(control, args);
                    return new Application.Business.Erp.Financial.Client.FinanceUI.InitialSetting.AccountTitleUI.CAccountTitle(framework).Excute(AccountTitleType.��ƿ�Ŀ��);
                case "������Ŀά��":
                    return new CExpenseItem(framework).Excute(args);
                case "�ɱ���Ŀά��":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI.CCostProject(framework).Excute(control, args);
                case "������Ŀѡ��":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.CostProjectUI.CCostProject(framework).Excute(CostPtojectType.costProjectSelect);
                case "��������ά��":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.basicDataOptr);
                case "��־��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.logDataQuery);
                case "��־ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.logStatReport);
                case "��ɫά��":
                    return new Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COperationJob(framework).Excute(Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource.COperationJob.AccountOrgExecuteType.��ɫά��);

                #endregion

                #region �ֿ����
                case "���ϳ��ⵥ(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI.CStockOut(framework).Excute(EnumStockExecType.��װ);
                case "���ϳ��ⵥ(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI.CStockOut(framework).Excute(EnumStockExecType.����);
                case "���ϳ���쵥(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI.CStockOutRed(framework).Excute(EnumStockExecType.����);
                case "���ϳ���쵥(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI.CStockOutRed(framework).Excute(EnumStockExecType.��װ);
                case "���ϳ��ⵥ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI.CStockOut(framework).Excute(EnumStockExecType.StockOutQuery, TheAuthMenu);
                case "�������ݱ���ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI.CStockOut(framework).Excute(EnumStockExecType.�������ݱ���ͳ��);
                case "����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.StockRelationQuery);


                //return new Application.Business.Erp.SupplyChain.Client.WebMng.CWebMng(framework).Excute("WorkFlowHistory");
                case "��ӯ��(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn.CProfitIn(framework).Execute(EnumStockExecType.��װ);
                case "��ӯ��(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn.CProfitIn(framework).Execute(EnumStockExecType.����);
                //xl
                case "��ӯ����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn.CProfitIn(framework).Execute(Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn.ProfitInExcType.ProfitInQuery);
                case "�̿���(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut.CLossOut(framework).Execute(EnumStockExecType.����);
                case "�̿���(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut.CLossOut(framework).Execute(EnumStockExecType.��װ);
                case "�̿�����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut.CLossOut(framework).Execute(Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut.LossOutExcType.LossOutQuery);
                case "����ʵ�ʺ��ý���":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(args);
                //return new Application.Business.Erp.SupplyChain.Client.WebMng.CWebMng(framework).Excute(args);
                case "���¼��������ļ�":
                    return new Application.Business.Erp.SupplyChain.Client.WebMng.ReloadIrpXML().ShowDialog();
                case "������ⵥ(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.����);
                case "������ⵥ(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.��װ);

                case "�����޸�":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.�����޸�);

                case "�������쵥(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI.CStockInRed(framework).Excute(EnumStockExecType.����);
                case "�������쵥(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI.CStockInRed(framework).Excute(EnumStockExecType.��װ);
                case "������ⵥ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI.CStockIn(framework).Excute(EnumStockExecType.stateSearch, TheAuthMenu);
                case "������ⵥ(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI.CStockMoveIn(framework).Execute(EnumStockExecType.����);
                case "������ⵥ(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI.CStockMoveIn(framework).Execute(EnumStockExecType.��װ);
                case "�������쵥(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI.CStockMoveInRed(framework).Execute(EnumStockExecType.����);
                case "�������쵥(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI.CStockMoveInRed(framework).Execute(EnumStockExecType.��װ);
                case "������ⵥ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI.CStockMoveIn(framework).Execute(EnumStockExecType.StockMoveInQuery, TheAuthMenu);
                case "�������ⵥ(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.��װ);
                case "�������ⵥ(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.����);
                case "��������쵥(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutRedUI.CStockMoveOutRed(framework).Excute(EnumStockExecType.��װ);
                case "��������쵥(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutRedUI.CStockMoveOutRed(framework).Excute(EnumStockExecType.����);
                case "�������ⵥ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.StockMoveOutQuery, TheAuthMenu);
                case "��������ά��":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.��������ά��);
                case "��˾�����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutUI.CStockMoveOut(framework).Excute(EnumStockExecType.��˾�����ѯ);
                case "���ս��㵥":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalUI.CStockInBal(framework).Excute(args);
                case "���ս���쵥":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalRedUI.CStockInBalRed(framework).Excute(args);
                case "���ս��㵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalUI.CStockInBal(framework).Excute(EnumStockExecType.StockInBalQuery, TheAuthMenu);
                case "�ֿ��շ�̨��":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI.CStockInRed(framework).Excute(EnumStockExecType.�ֿ��շ�̨��);
                case "�ֿ��շ����±�":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(EnumStockExecType.�ֿ��շ����±�);
                case "�շ��汨��":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(EnumStockExecType.�ֿ��շ��汨��);
                case "�ɱ��Աȷ�����":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(EnumStockExecType.�ɱ��Աȷ�����);
                //return null;
                #endregion

                #region �Ͼ����޹���
                case "�Ͼ����޺�ͬ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder(framework).Excute(args);
                case "�Ͼ����޺�ͬ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder_ExecType.MaterialRentalOrderQuery);
                case "�Ͼ����޺�ͬ����":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder_ExecType.MaterialRentalOrderCopy);
                case "�Ͼ����޺�ͬ����":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialRentalOrder.CMaterialRentalOrder_ExecType.MaterialRentalOrderRef);
                case "�Ͼ����ϵ�":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(args);
                case "���ϵ�ͳ�Ʋ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialCollectionQuery);
                case "�Ͼ����ϵ�":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn.CMaterialReturn(framework, 1).Excute(1);
                case "�Ͼ����ϵ�(���)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn.CMaterialReturn(framework, 2).Excute(2);
                case "���ϵ�ͳ�Ʋ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn.CMaterialReturn(framework, 0).Excute(CMaterialReturn_ExecType.MaterialReturnQuery);
                case "�Ͼ�������ˮ��":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialRentalCurrentCount);
                case "�Ͼ�����̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialRentalLedgerQuery);
                case "�Ͼ������½�":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialMonthlyBalance);
                case "�Ͼ����޽����":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection.CMaterialCollection(framework).Excute(CMaterialCollection_ExecType.MaterialMonthlyQuery);
                case "�Ͼ������±�":
                    return new Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI.CStockInOut(framework).Excute(EnumStockExecType.�Ͼ�����);
                #endregion

                #region �Ͼ����ʹ���
                case "�Ͼ��������뵥":
                    return new Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialOrder(framework).Excute(args);
                case "�Ͼ����������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialOrder_ExecType.WasteMatApplyQuery);
                case "�Ͼ����ʴ�����":
                    return new Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialHandle(framework).Excute(args);
                case "�Ͼ����ʴ�����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialHandle(framework).Excute(Application.Business.Erp.SupplyChain.Client.WasteMaterialMng.WasteMaterialManage.CWasteMaterialHandle_ExecType.WasteMatHandleQuery);
                #endregion

                #region ��Ʒ�Ź���
                case "������¼��":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConPouringNoteMng.CConPouringNote(framework).Excute(args);
                case "������¼ͳ�Ʋ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConPouringNoteMng.CConPouringNote(framework).Excute(CConPouringNote_ExceType.ConPouringNoteQuery);
                case "�����":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng.CPumpingPounds(framework).Excute(args);
                case "�����ͳ�Ʋ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng.CPumpingPounds(framework).Excute(CPumpingPounds_ExceType.PumpingPoundsQuery);
                case "���˵�":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConCheckingMng.CConCheck(framework).Excute(args);
                case "���˵�ͳ�Ʋ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConCheckingMng.CConCheck(framework).Excute(CConCheck_ExceType.ConPouringNoteQuery);
                case "��Ʒ�Ž��㵥":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.CConBalance(framework).Excute(args);
                case "��Ʒ�Ž���쵥":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.ConBalanceRedUI.CConBalanceRed(framework).Excute(args);
                case "��Ʒ�Ž����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.CConBalance(framework).Excute(CConBalance_ExceType.ConBalanceQuery);
                #endregion

                #region �����ܼƻ�����
                case "�����ܼƻ���(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.��װ);
                case "�����ܼƻ���(����)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.����);
                case "�����ܼƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.demandSearch, TheAuthMenu);
                case "�����ܼƻ���ѯ(��˾)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.companyDemandSearch);
                case "�ɹ��ɱ�ͳ�Ʊ�":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng.CDemandMasterPlanMng(framework).Excute(EnumDemandType.companySupplyPlan);
                #endregion

                #region �¶�����ƻ�����
                case "�¶�����ƻ���(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng.CMonthlyPlanMng(framework).Excute(EnumMonthlyType.��װ);
                case "�¶�����ƻ���(����)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng.CMonthlyPlanMng(framework).Excute(EnumMonthlyType.����);
                case "�¶�����ƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.MonthlyPlanMng.CMonthlyPlanMng(framework).Excute(EnumMonthlyType.monthlySearch, TheAuthMenu);

                #endregion

                #region �ճ�����ƻ�����
                case "�ճ�����ƻ���(����)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng.CDailyPlanMng(framework).Excute(EnumDailyType.����);
                case "�ճ�����ƻ���(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng.CDailyPlanMng(framework).Excute(EnumDailyType.��װ);
                case "�ճ�����ƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.DailyPlanMng.CDailyPlanMng(framework).Excute(EnumDailyType.dailySearch, TheAuthMenu);
                #endregion

                #region �ɹ���ͬ����
                case "�ɹ���ͬ��(����)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng.CSupplyOrderMng(framework).Excute(EnumSupplyType.����);
                case "�ɹ���ͬ��(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng.CSupplyOrderMng(framework).Excute(EnumSupplyType.��װ);
                case "�ɹ���ͬ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMng.CSupplyOrderMng(framework).Excute(EnumSupplyType.supplySearch);
                case "�ɹ���ͬ��(��˾)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMngCompany.CSupplyOrderMngCompany(framework).Excute();
                case "�ɹ���ͬ��ѯ(��˾)":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyOrderMngCompany.CSupplyOrderMngCompany(framework).Excute(EnumSupplyType.supplySearch);
                #endregion

                #region �ɹ���ͬ���۹���
                //case "�ɹ���ͬ���۵�":
                //    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPriceMng(framework).Excute(args);
                case "�ɹ���ͬ����":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPriceMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPrice_ExecType.ContractAdjustPriceQuery);
                case "�ɹ���ͬ���۲�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPriceMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng.CContractAdjustPrice_ExecType.ContractAdujustPriceQueryNew);
                #endregion

                #region �ɹ��ƻ�����
                case "רҵ����ƻ�":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng.CSupplyPlanMng(framework).Excute(args);
                case "רҵ����ƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng.CSupplyPlanMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.SupplyMng.SupplyPlanMng.CSupplyPlanMng_ExecType.SupplyPlanMngQuery);
                #endregion

                #region ���̸��Ĺ���
                case "���̸�����Ϣ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.EngineerChangeMng.CEngineerChangeMng(framework).Excute(args);
                case "���̸��Ĳ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.EngineerChangeMng.CEngineerChangeMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.EngineerChangeMng.CEngineerChangeMng_ExecType.EngineerChangeQuery);
                #endregion

                #region ���
                case "���ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(args);
                case "�����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng_ExecType.PenaltyDeductionQuery);
                case "�ۿ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng_ExecType.PenaltyQuery);
                case "������㵥":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng_ExecType.PenaltyDeductionSelect);
                case "�ݿۿά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement.CPenaltyDeductionMng_ExecType.TempDedit);
                #endregion

                #region ��Ŀ������Ϣ����
                case "��Ŀ������Ϣά��":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectDepartment);
                case "��Ŀ������Ϣ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectDepartmentSearch);
                case "��Ŀҵ����Ϣ":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectBusiInfo);
                case "��Ŀ����״̬ά��":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectBusinessStateMng);
                case "��Ŀ����״̬ά��":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectStateMng);
                case "��Ŀ��������ά��":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectMaterialStateMng);

                case "��Ŀʹ��״̬����":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectStateReport);
                case "��Ŀ�ۺ�����ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.ProjectDataInfo);
                case "��˾��Ŀʹ��״��ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.AllProjectState);
                case "������Ŀά��":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.ProjectDepartmentMng.CProjectDepartment_ExecType.OrgAsProjectInfo);
                #endregion

                #region ��������ƻ�
                case "��������ƻ�":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng.CLaborDemandPlan(framework).Excute(args);
                case "��������ƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng.CLaborDemandPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng.CLaborDemandPlan_ExecType.LaborDemandPlanQuery);
                #endregion

                #region �¶��̵����
                //case "�̵㵥":
                //    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(args);

                case "�̵㵥(����)":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(CStockInventoryMng_ExecType.����);
                case "�̵㵥(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(CStockInventoryMng_ExecType.��װ);
                case "�ֿⱨ��(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(CStockInventoryMng_ExecType.�ֿⱨ��);
                case "�̵㵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng.CStockInventoryMng_ExecType.StockInventoryQuery);
                #endregion

                #region �����ù�����
                case "�����ɹ���ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.�����ɹ�);
                case "�����ù���ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.�ɹ�);
                case "�ְ�ǩ֤��ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.�ְ�ǩ֤);
                case "��ʱ�ɹ���ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.��ʱ�ɹ�);
                case "������ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(EnumLaborType.����);
                case "����������":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborSporadicSelector);
                case "�����ù�������":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborSporadicSelect);
                case "�ְ�ǩ֤�����":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.SubPackageVisaSelect);
                case "��ʱ�ɹ�������":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.TimeDespatching);
                case "�����ù���������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborSporadicSHCQuery);
                case "�����ù��������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborSporadicQuery);
                case "��������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborSporadicMng.CLaborSporadicMng_ExecType.LaborQuery);
                #endregion

                #region ����ִ����
                case "����ִ��":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.DetectionReceiptMng.CDetectionReceiptMng(framework).Excute(args);
                case "����ִ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.StockMng.DetectionReceiptMng.CDetectionReceiptMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.StockMng.DetectionReceiptMng.CDetectionReceiptMng_ExecType.DetectionReceiptQuery);
                #endregion

                #region ������
                case "������":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng.CInspectionLotMng(framework).Excute(args);
                case "��������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng.CInspectionLotMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng.CInspectionLotMng_ExecType.InspectionLotQuery);
                #endregion

                #region ����֪ͨ����
                case "����֪ͨ��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNoticeMng(framework).Excute(args);
                case "����֪ͨ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNoticeMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNotice_ExecType.RectificationNoticeQuery);
                case "���ĵ�����":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNoticeMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng.CRectificationNotice_ExecType.RectificationNoticeSelector);
                #endregion

                #region ���ý������
                case "���ý��㵥ά��":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng.CExpensesSettleMng(framework).Excute(args);
                case "���ý��㵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng.CExpensesSettleMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng.CExpensesSettleMng_ExecType.ExpensesSettleQuery);
                #endregion

                #region ���ʺ��ý������
                case "���Ϻ��ý��㵥ά��":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng.CMaterialSettleMng(framework).Excute(EnumMaterialSettleType.���ʺ��ý��㵥ά��);
                case "�Ͼ����޽��㵥ά��":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng.CMaterialSettleMng(framework).Excute(EnumMaterialSettleType.�Ͼ߽��㵥ά��);
                case "���Ϻ��ý��㵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng.CMaterialSettleMng(framework).Excute(EnumMaterialSettleType.materialSettleQuery);
                case "�Ͼ����޽��㵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng.CMaterialSettleMng(framework).Excute(EnumMaterialSettleType.materialQuery);
                #endregion

                #region ���ռ���¼
                case "���ռ���¼":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng(framework).Excute(args);
                case "���ռ���¼��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng_ExecType.AcceptanceInspectionQuery);
                case "���ռ��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng.CAcceptanceInspectionMng_ExecType.Acceptance);
                #endregion

                #region ��Ŀ���̸���
                case "��Ŀ���̸���":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectCopyMng.CProjectCopy(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectCopyMng.CProjectCopy_ExecType.ProjectCopy);
                #endregion

                #region OBS
                case "����OBS":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OBS.COBS(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.OBS.CDOBS_ExecType.OBSManage);
                case "����OBS":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OBS.COBS(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.OBS.CDOBS_ExecType.OBSService);
                #endregion

                #region ר���������
                case "ר��������õ�":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost.CSpecialCost(framework).Excute(args);
                case "ר��������ò�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost.CSpecialCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost.CSpecialCostOrder_ExecType.SpecialCostQuery);

                case "ר����ý��㵥":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount.CSpecialAccount(framework).Excute(args);
                case "ר����ý����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount.CSpecialAccount(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount.CSpecialAccount_ExecType.SpecialAccountQuery);

                #endregion

                #region �ְ���Ŀ����
                case "�ְ�ִ��״̬ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng.CContractExcuteMng(framework).Excute(args);
                case "�ְ�ִ��״̬��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng.CContractExcuteMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng.CContractExcuteMng_ExecType.ContractExcuteQuery);
                #endregion

                #region ��������ƻ�����
                case "��������ƻ���":
                    return new Application.Business.Erp.SupplyChain.Client.TotalDemandPlan.CTotalDemandPlanMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.TotalDemandPlan.CDemandPlanMng_ExecType.TotalDemandPlanQuery);
                case "��������������":
                    return new Application.Business.Erp.SupplyChain.Client.TotalDemandPlan.CTotalDemandPlanMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.TotalDemandPlan.CDemandPlanMng_ExecType.TotalDemandAnalysis);
                #endregion

                #region ҵ����
                case "ҵ��������ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng(framework).Excute(args);
                case "ҵ����������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng_ExecType.OwnerQuantityQuery);
                case "ҵ������״̬��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng.COwnerQuantityMng_ExecType.OwnerQuantitySearch);
                #endregion

                #region �������
                case "���˽�":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.DelimitIndividualBillMng.CDelimitIndividualBillMng(framework).Excute(args);
                case "���˽���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.DelimitIndividualBillMng.CDelimitIndividualBillMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.DelimitIndividualBillMng.CDelimitIndividualBillMng_ExecType.DelimitIndividualBillQuery);

                case "���û��˵�":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng.CExpensesRowBillMng(framework).Excute(args);
                case "���û��˲�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng.CExpensesRowBillMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng.CExpensesRowBillMng_ExecType.ExpensesRowBillQuery);
                case "���ñ�����":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesSingleBillMng.CExpensesSingleBillMng(framework).Excute(args);
                case "���ñ�����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesSingleBillMng.CExpensesSingleBillMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesSingleBillMng.CExpensesSingleBillMng_ExecType.ExpensesSingleBillQuery);

                case "�ٽ�̯����":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.OverlayAmortizeMng.COverlayAmortizeMng(framework).Excute(args);
                case "�ٽ�̯����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.OverlayAmortizeMng.COverlayAmortizeMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.OverlayAmortizeMng.COverlayAmortizeMng_ExecType.OverlayAmortizeQuery);
                case "����������㵥":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.IncomeSettlementMng.CIncomeSettlementMng(framework).Excute(args);
                case "����������㵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.IncomeSettlementMng.CIncomeSettlementMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.IncomeSettlementMng.CIncomeSettlementMng_ExecType.IncomeSettlementQuery);

                case "���Ͻ��㵥":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.MaterialsettlementMng.CMaterialSettlementMng(framework).Excute(args);
                case "���Ͻ����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.FinanceMng.MaterialsettlementMng.CMaterialSettlementMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.FinanceMng.MaterialsettlementMng.CMaterialSettlementMng_ExecType.MaterialSettlementQuery);
                #endregion

                #region ʩ��רҵ��������
                case "ʩ��רҵ������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ConstructionDataMng.CConstructionData(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ConstructionDataMng.CWasteMaterialHandle_ExecType.ConstructionData);
                #endregion

                #region רҵ����¼
                case "�ճ����":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng.CProInsRecord(framework).Excute(args);
                case "�ճ�����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng.CProInsRecord(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ProfessionInspectionRecordMng.CProInsRecord_ExecType.ProInsRecordQuery);

                #endregion

                #region �������ݹ���
                case "����Ĭ�ϼ�����λ����":
                    return new Application.Business.Erp.SupplyChain.Client.BasicData.UnitMng.CUnitMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.BasicData.UnitMng.CUnitMng_ExecType.UnitMng);
                #endregion

                #region �������ݵ���
                case "�������ݵ���":
                    return new Application.Business.Erp.SupplyChain.Client.ExcelImportMng.CExcelImportMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ExcelImportMng.CExcelImportMng_ExecType.VExcelImport);
                case "�����������":
                    return new Application.Business.Erp.SupplyChain.Client.ExcelImportMng.CExcelImportMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.ExcelImportMng.CExcelImportMng_ExecType.VExcelImportMng);
                #endregion

                #region �豸���޽������
                case "��е���޽��㵥ά��":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng.CMaterialRentalSettlement(framework).Excute(args);
                case "��е���޽��㵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng.CMaterialRentalSettlement(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalSettlementMng.CMaterialRentalSettlementPlan_ExecType.MaterialRentalSettlementQuery);
                case "��е���޺�ͬά��":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalContractMng.CMaterialRentalContract(framework).Excute(args);
                #endregion

                #region ����

                case "�������Ź���ά��":
                    return new CommonSearch.BillCodeRuleMng.VBillCodeRule().ShowDialog();
                case "Ȩ������":
                    //return new VAuthConfig(ConstObject.TheLogin.ThePerson.Code).ShowDialog();
                    return new VAuthConfigByRole(ConstObject.TheLogin.ThePerson.Code).ShowDialog();
                case "����Ȩ������":
                    //return new VAuthConfig(ConstObject.TheLogin.ThePerson.Code).ShowDialog();
                    return new VAuthConfigOrgSysCode().ShowDialog();
                case "��ѯ����":
                    //return new CommonSearchDesign().ShowDialog();
                    return null;
                case "�����ֵ�":
                    //return new VClassDict().ShowDialog();
                    return null;
                case "��������":
                    //return new ApprovalManager.AppDefineMng.VAppDefine().ShowDialog();
                    return null;
                case "����ƽ̨":
                    //new ApprovalManager.AppDataMng.VAppDataView(ConstObject.TheLogin.ThePerson.Code).Show();
                    return null;
                #endregion

                #region ����ά��
                case "ϵͳ����":
                    return new CustomServiceClient.QuestionMng.CQuestion(framework).Excute(35, ConstObject.LoginPersonInfo.Name, args);
                case "ϵͳ�����ѯ":
                    return new CustomServiceClient.QuestionMng.CQuestion(framework).Excute(35, ConstObject.LoginPersonInfo.Name, QuestionType.search);
                #endregion

                #region ����ƽ̨
                case "�������ݶ���":
                    return new CAppTableSet(framework).Excute(control, args);
                case "�������Զ���":
                    return new CAppSolutionSet(framework).Excute(control, CAppSolutionSet_ExecType.AppPropertySet);
                case "��������ά��":
                    return new CAppSolutionSet(framework).Excute(control, CAppSolutionSet_ExecType.AppSolutionSet);
                case "ҵ������ƽ̨":
                    return new CAppPlatform(framework).Excute(control, args);
                case "ҵ�񵥾��޸�":
                    return new CAppPlatform(framework).Excute(control, CAppPlatform_EnumType.SetBill);
                case "����״̬��ѯ":
                    return new CAppStatusQuery(framework).Excute(control, args);
                case "����״̬���ݲ�ѯ":
                    return new CAppStatusQuery(framework).ExcuteByBill(args[0], args[0].ToString());
                case "����������ѯ":
                    return new CAppPlatform(framework).Excute(control, CAppPlatform_EnumType.ApproveQuery);
                #endregion

                #region �ɱ�����/PIM����
                case "ʩ����λ����ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.ʩ����λ����ά��);
                case "PBS�ڵ�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.PBS�ڵ�ά��);
                case "ʩ����λģ��ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.ʩ����λģ��ά��);
                case "ʩ����λ�ṹ����ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.ʩ����λ�ṹ����ά��);
                case "���Լ�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.PBS.CPBSTree(framework).Excute(OperationPBSTreeType.���Լ�ά��);
                case "�ĵ��������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.�ĵ��������);
                case "ʩ����������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.ʩ����������);
                case "��������ģ��ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.��������ģ��ά��);
                case "��ʼ����Ŀ�����ĵ�ģ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.��ʼ����Ŀ�����ĵ�ģ��);
                case "��Լ������Ϣά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng.CWBSContractGroup(framework).Excute(OperationContractGroupType.��Լ������Ϣά��);
                case "ʩ�����񻮷�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.ʩ�����񻮷�ά��);
                case "ʩ�����񻮷�ά��_��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.ʩ�����񻮷�ά��_��);
                case "���̳ɱ�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.���̳ɱ�ά��);
                case "���̳ɱ�����ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.���̳ɱ�����ά��);
                case "ǩ֤���̨�˲�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.ǩ֤���̨�˲�ѯ);
                case "��Ŀ�����ۺϲ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.��Ŀ�����ۺϲ�ѯ);
                case "���̳ɱ�ά���ۺϲ�ѯ"://��������ϼ۲�ѯ
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.���̳ɱ�ά���ۺϲ�ѯ);
                case "�ֳ���������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.�ֳ���������);
                case "�������ռ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.�������ռ��);
                case "���ֳ���������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.���ֳ���������);
                case "�ֳ���������(�ֳ�)":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.�ֳ���������_�ֳ�);
                case "�������ᱨ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.�������ᱨ��ѯ);
                //case "����WBS��ϸ�༭":
                //    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS.CWBSTree(framework).Excute(OperationGWBSType.����WBS��ϸ�༭);

                case "�ɱ������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng.CCostItemCategory(framework).Excute(OperationCostItemCategoryType.�ɱ������);
                case "�ɱ�����ർ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng.CCostItemCategory(framework).Excute(OperationCostItemCategoryType.�ɱ�����ർ��);
                case "�ɱ���ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng.CCostItem(framework).Excute(OperationCostItemType.�ɱ���ά��);
                case "�ɱ����":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng.CCostItem(framework).Excute(OperationCostItemType.�ɱ����);
                case "�ɱ������Ŀ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng.CCostItemCategory(framework).Excute(OperationCostAccountSubjectType.�ɱ������Ŀ);
                case "�嵥���񻮷�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS.CQWBSManagement(framework).Excute(OperationQWBS.�嵥WBS����);
                #endregion

                #region ��������
                case "�ܽ��ȼƻ�":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(args);
                case "���񵥲�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.���񵥲�ѯ);
                case "�ܽ��ȼƻ�����":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.�ܽ��ȼƻ�����);
                case "�ܹ������ȼƻ�":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(EnumScheduleType.�ܹ������ȼƻ�);
                case "���ȼƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(EnumScheduleType.���ȼƻ���ѯ, TheAuthMenu);
                case "���ȼƻ�����":
                    //return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumExecScheduleType.�¶Ƚ��ȼƻ�);
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumExecScheduleType.��Ƚ��ȼƻ�);
                case "�����ܼƻ�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumExecScheduleType.�ܽ��ȼƻ�);
                case "�¼ƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.�¼ƻ���ѯ);
                case "�ܼƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.�ܼƻ���ѯ);
                case "��Ŀ�ܼƻ�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.��Ŀ�ܼƻ�ά��);
                case "����ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.����ά��);
                case "��Ŀ�ܼƻ�ȷ��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI.CWeekSchedule(framework).Excute(EnumProductionMngExecType.�ܼƻ�ȷ��);
                case "������ȷ�ϵ�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI.CGWBSConfirm(framework).Excute();
                case "������ȷ�ϵ�ά��_�Ǽƻ�":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI.CGWBSConfirm(framework).Excute(EnumProductionMngExecType.������ȷ�ϵ�ά��_�Ǽƻ�);
                case "������ȷ�ϵ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI.CGWBSConfirm(framework).Excute(EnumProductionMngExecType.��������ȷ�ϵ���ѯ);
                case "������ƻ�":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(OperationRollingDemandPlanType.������ƻ�);
                case "�ܹ�������ƻ�":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(OperationRollingDemandPlanType.�ܹ�������ƻ�);
                case "����ִ������ƻ�":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(args);
                case "ִ������ƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(OperationRollingDemandPlanType.ִ������ƻ���ѯ);
                case "�ճ�����¼":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng.CInspectionRecord(framework).Excute(CInspectionRecord_ExecType.InspectionRecord);
                case "�������ռ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionRecordMng.CInspectionRecord(framework).Excute(CInspectionRecord_ExecType.InspectionRecordQuery);
                case "��Դ�������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.OperationRollingDemandPlanType.��Դ�������);
                case "��Դ����ƻ�����":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.CRollingDemandPlan(framework, true).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan.OperationRollingDemandPlanType.��Դ����ƻ�����);
                case "�ڼ�����ƻ���ά��":
                    return new Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan.CPeriodRequirePlanMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan.CPeriodRequirePlanMng_ExecType.�ڼ�����ƻ���ά��);
                case "����Ԥ��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(EnumProductionMngExecType.����Ԥ��);
                case "�Ͷ���Ԥ��ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI.CSchedule(framework).Excute(EnumProductionMngExecType.�Ͷ���Ԥ��ͳ��);
                #endregion

                #region �ֳ��������
                case "�����������ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccount(framework).Excute(args);
                case "����������㵥ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccount(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccountExecType.�����������ά��_���⹤��);
                case "������ȷ�ϵ�":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccount(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccountExecType.������ȷ�ϵ�);
                case "������������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccount(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng.CProjectTaskAccountExecType.������������ѯ, TheAuthMenu);
                case "�ְ����㵥ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(args);
                case "�ְ����㵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.�ְ������ѯ, TheAuthMenu);
                case "���ݺ����Ŀ����":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.���ݺ����Ŀ����, TheAuthMenu);
                case "�¶ȳɱ�����ά��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthAccount);
                case "�¶ȳɱ�����(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthAccountSpecial);
                case "�¶ȳɱ�������־":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthLog);
                case "�¶ȳɱ������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthQuery);
                case "�¶Ⱥ����ۺϲ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthGenQuery);
                case "����ʩ��ͳ�Ʊ���":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ProduceReport);
                case "�¶ȳɱ���������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthReport);
                case "���¶ȳɱ���������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MonthReportNew);
                case "�¶ȳɱ������Աȱ���":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.CostMonthCompareReport);
                case "�¶ȳɱ�����(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SpecialReport);
                case "���񱨱�(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SpecialBusinessReport);
                case "����Ч��ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.EconomyProfit);
                case "�ɱ�����ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.CostConsume);
                case "����ָ��ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.TechIndicator);
                case "����ɱ�ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.IncomeCost);
                case "�����տ�ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ReceiveMoney);
                case "ǩ֤���ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ContractLedger);
                case "���񱨱�ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.BusinessReport);
                case "�ְ���̨ͬ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SubcontractsLedger);
                case "��Ŀ���㷣��̨��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.FineAccountReport);
                case "��Ŀ�����ۿ�̨��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.OEMChargebackReport);
                case "��ͬ��������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ContractDisclosure);
                case "��ͬ���ײ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ContractDisclosureQuery);
                case "��Ŀ�ְ�����̨��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SubcontractSettlementReport);
                case "��е���޽���̨��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MaterialRentalSettlementReport);
                case "�ְ���λ������̨��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SubcontractAmountReport);
                case "���񱨱��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.CommercialReport);
                case "��Ŀҵ��ȷȨ̨��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.ConfirmRightReport);
                case "���񱨱��ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.CommercialReportQuery);
                case "��е�ѳɱ������Աȱ�":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.MechanicalCostComparisonRpt);
                case "ȡ��ģ�嶨��":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SelFeeTemplate);
                case "��ά�ȶԱȱ�":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.��ά�ȶԱȱ�);
                case "��Ŀ�ְ���������":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.��Ŀ�ְ���������);
                case "�ֳ������ѷ����Աȱ�"://20160819 HJ
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount.CCostMonthAccount(framework).Excute(CCostMonthAccount_ExecType.SceneManageFeelReport);
                case "�ְ��ս���㵥����"://20160904 HJ
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.�ְ��ս���㵥����);
                case "�ְ��ս���㵥���"://20160904 HJ
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng.CSubContractBalance(framework).Excute(CostManagement.SubContractBalanceMng.CSubContractBalance.CSubContractBalance_ExecType.�ְ��ս���㵥���);
                #endregion

                #region ָ�����
                case "ά��ά��":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.CDimensionCategory(framework).Excute(args);
                case "���ⶨ��":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse.CCubeDefine(framework).Excute(args);
                case "ͨ�ñ�������":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse.CViewDefFree(framework).Excute(args);
                case "ͨ�ñ����ַ�":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse.CViewFreeDistribute(framework).Excute(args);
                case "��������¼��":
                    return new Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse.CViewFreeWrite(framework).Excute(args);
                #endregion

                #region �ļ��ϴ�
                case "�ļ��ϴ�����":
                    return new Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.CWBSManagement(framework).Excute(OperationWBSType.�ļ��ϴ�����);
                case "�ļ��ϴ�":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload.CFilesUpLoad(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload.CFilesUpLoaad_ExecType.VFilesUpLoad);
                #endregion

                #region ���Բ˵�
                case "���Բ˵�":
                    return new CWebMng(framework).Excute("IRPͬ��");
                #endregion

                #region ��ʩ������
                case "������Ϣ":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeather(framework).Excute(args);
                case "������Ϣ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeather(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeatherMng_ExecType.WeatherQuery);
                case "��ά�����":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeather(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.WeatherMng.CWeatherMng_ExecType.QRCode);
                case "������Ա��־":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement.CPersonManage(framework).Excute(args);
                case "������Ա��־��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement.CPersonManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.PersonManagement.CPersonManageMng_ExecType.PersonManageQuery);
                case "ʩ����־":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement.CConstruction(framework).Excute(args);
                case "ʩ����־��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement.CConstruction(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement.CConstructionMng_ExecType.ConstructionQuery);

                case "��ʩ������":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport.CConstructionReport(framework).Excute(args);
                case "��ʩ�������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport.CConstructionReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport.CConstructionReportMng_ExecType.ConstructReportQuery);

                #endregion

                #region ��������
                case "�������㵥":
                    return new Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook.CCompleteMng(framework).Excute(args);
                case "���������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook.CCompleteMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook.CCompleteMng_ExecType.CompleteQuery);
                #endregion

                #region ���ʼ�ά��
                case "��Ϣ�۸�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.Basic.CProgramManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.Basic.CProgramManageMng_ExecType.ProgramManage);

                #endregion

                #region �ܽ��ȼƻ�
                case "�ܽ��ȼƻ�":
                    //return new Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules.CWeekPlanEntry(framework).Excute(EnumWeekPlanMngExecType.�ܽ��ȼƻ�);
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules.CWeekPlanEntry(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules.EnumWeekPlanMngExecType.�ܽ��ȼƻ�);
                #endregion

                #region ʩ��������Ϣ��ѯ
                case "ʩ��������Ϣ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage.CGwbsManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage.CGwbsManage_ExecType.VGwbsManage);
                #endregion

                #region ���ö���ά��
                case "���ö���ά��":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords.COftenWords(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords.COftenWords_ExecType.VOftenWords);
                case "���Խ���":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords.COftenWords(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords.COftenWords_ExecType.VTest);
                #endregion

                #region ��Ŀʵʩ�߻���
                case "��Ŀʵʩ�߻���":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan.CImplementationPlan(framework).Excute(args);
                case "��Ŀʵʩ�߻���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan.CImplementationPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan.CImplementationPlan_ExexType.ImplementtationSearch);
                #endregion

                #region ��ĿĿ��������
                case "Ŀ��������":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng.CTargetRespBook(framework).Excute(args);
                case "Ŀ���������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng.CTargetRespBook(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng.EnumTargetRespBook.TargetRerspBookSearch);
                #endregion

                #region ��Ŀ�ĵ�����
                case "�ĵ�����":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList_ExecType.VDocumentLisBak);
                case "�ĵ��������ǰ׺ӳ������":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList_ExecType.�ĵ��������ǰ׺ӳ������);
                case "�������͹����ĵ���������":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList_ExecType.�������͹����ĵ���������);
                case "�ĵ�����ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory_ExecType.VDocumentCategory);
                case "�ļ�����ϸ��Ϣά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory_ExecType.VDocumentCategoryManage);
                case "�ĵ��ۺϲ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng.CDocumentCategory_ExecType.VDocumentsSelect);

                #endregion
                //#region ��Ŀ�ĵ�����
                //case "�����ĵ�������":
                //    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.ProjectDocumentsMng.CDocumentList_ExecType.VDocumentList);
                //#endregion

                #region �շ�������
                case "�շ�����Ϣ":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage.CCollectionManage(framework).Excute(args);
                case "�շ�����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage.CCollectionManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage.CCollectionManage_ExexType.CollectionManageSearch, TheAuthMenu);
                #endregion

                #region �����Ҫ����
                case "�����Ҫ��Ϣ":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary.CMeetingManage(framework).Excute(args);
                case "�����Ҫ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary.CMeetingManage(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary.CMeetingManage_ExexType.MeetingManageSearch);
                #endregion

                #region �ܿ�ָ���Ԥ��
                case "״̬��鶯��ά��":
                    return new Application.Business.Erp.SupplyChain.Client.PMCAndWarning.CPMCAndWarning(framework).Excute(OperationPMCAndWarningType.״̬��鶯��ά��);
                case "Ԥ����Ϣ���ͷ�ʽ����":
                    return new Application.Business.Erp.SupplyChain.Client.PMCAndWarning.CPMCAndWarning(framework).Excute(OperationPMCAndWarningType.Ԥ����Ϣ���ͷ�ʽ����);
                case "Ԥ���������̨":
                    return new Application.Business.Erp.SupplyChain.Client.PMCAndWarning.CPMCAndWarning(framework).Excute(OperationPMCAndWarningType.Ԥ���������̨);
                case "Ԥ����Ϣ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.PMCAndWarning.CPMCAndWarning(framework).Excute(OperationPMCAndWarningType.Ԥ����Ϣ��ѯ);
                #endregion

                #region ��Ŀ�߻�����
                case "ʩ����֯���ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage.CContructionDesign(framework).Excute(args);
                case "ʩ����֯��Ʋ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage.CContructionDesign(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage.EnumContructionDesign.ContructionDesignSearch);
                case "רҵ�߻�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.WebSitePlanManage.CWebSitePlan(framework).Excute(args);
                case "רҵ�߻���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.WebSitePlanManage.CWebSitePlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.WebSitePlanManage.EnumWebSitePlan.WebSitePlanSearch);
                case "����߻�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage.CBusinessProposal(framework).Excute(args);
                case "����߻���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage.CBusinessProposal(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage.EnumBusinessPropoasal.BusinessProposalSeaech);
                #endregion

                #region �����ĵ���������
                case "�����ĵ�����ά��":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng.CDocApprovalMng(framework).Excute(args);
                case "�����ĵ�������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng.CDocApprovalMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng.CDocApprovalMng_ExexType.DocApprovalManageSearch);
                #endregion

                #region ���ĵ�ȷ��
                case "���ĵ�ȷ��":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection.CDailyCorrection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection.CDailyCorrection_ExecType.VDailyCorrectionMaster);
                #endregion

                #region ���ĵ���ѯ
                case "���ĵ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrectioSearch.CDailyCorrectioSearch(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrectioSearch.CDailyCorrectionSearch_ExecType.VDailyCorrectionSearch);
                #endregion

                #region ��ѯ�ܼƻ�����
                case "�ܼƻ������ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.CProjectTaskQuery(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.EnumProjectTask.WeekScheduleSearch);
                #endregion

                #region רҵ���
                case "רҵ���":
                    return new Application.Business.Erp.SupplyChain.Client.MobileManage.ProInRecord.CProInRecordMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MobileManage.ProInRecord.CProInRecordMng_exectype.VProInRecordMng);
                #endregion

                #region ��ֵ���ܲ�ѯ
                case "��ֵ���ܲ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue.COutPutValue(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue.COutPutValue_ExexType.RealOutputValueQuery);
                case "��ֵ���ܲ�ѯ[���ƻ��ڵ�]":
                    return new Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue.COutPutValue(framework).Excute(Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue.COutPutValue_ExexType.OutPutValueQuery);
                #endregion

                #region ���߰���
                case "���߰���":
                    return new Application.Business.Erp.SupplyChain.Client.HelpOnline.CHelpOnline(framework).Excute(Application.Business.Erp.SupplyChain.Client.HelpOnline.CHelpOnline_ExexType.HelpOnline);
                #endregion

                #region �ʽ����
                case "Ʊ��̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.Ʊ��̨��);
                case "������Ϣ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.������Ϣ��ѯ);
                case "��Ŀ��������ά��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.��Ŀ��������ά��);
                case "��Ŀ��������ά��(��װ)":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.��Ŀ��������ά����װ);
                case "��Ŀ����ɱ�ȷ��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.��Ŀ����ɱ�ȷ��);

                case "��Ŀ��������ά����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.��Ŀ��������ά����ѯ);
                case "��Ӫҵ������̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.��Ӫҵ������̨��);
                case "��Ŀ�ɱ�֧��̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.��Ŀ�ɱ�֧��̨��);
                case "��˾��������ά��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.��˾��������ά��);
                case "��˾��������ά����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.��˾��������ά����ѯ);
                case "�ֹ�˾��������ά��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.�ֹ�˾��������ά��);
                case "�ֹ�˾��������ά����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.�ֹ�˾��������ά����ѯ);
                case "��������̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CAccountMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.EnumAccountMng.��������̨��);
                case "������Ϣά��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.������Ϣά��);
                case "��ӷ���̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.��ӷ���̨��);
                case "�׷�������Ӧ��̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.�׷�������Ӧ��̨��);
                case "�տ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.Gathering);
                case "�տ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.GatheringQuery);
                case "�տ(�ǹ��̿�)":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.GatheringOther);
                case "�տ�̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.�տ�̨��);
                case "Ӧ����Ƿ��̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.Ӧ����Ƿ��̨��);
                case "���":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.Payment);
                case "�������ݳ�ʼ��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.PaymentIntial);
                case "��֤����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.DepositImport);
                case "�����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.PaymentQuery);
                case "���(�ǹ��̿�)":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentMng_ExecType.PaymentOther);
                case "���Ʊά��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentInvoice(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentInvoiceType.PaymentInvoice);
                case "���Ʊ�ֿ�ά��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentInvoice(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CPaymentInvoiceType.���Ʊ�ֿ�ά��);
                case "���Ʊ��ѯ":
                    return new MoneyManage.FinanceMultData.CPaymentInvoice(framework).Excute(MoneyManage.FinanceMultData.CPaymentInvoiceType.PaymentInvoiceQuery);
                case "���Ʊ̨��":
                    return new MoneyManage.FinanceMultData.CPaymentInvoice(framework).Excute(MoneyManage.FinanceMultData.CPaymentInvoiceType.PaymentInvoiceReport);
                case "���ֽ���ͳ��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.���ֽ���ͳ��);
                case "�ؼ�ָ�����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.�ؼ�ָ�����);
                case "��������̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.��������̨��);
                case "��Ŀ�ʽ�ƻ�����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.��Ŀ�ʽ�ƻ�����);
                case "�ֹ�˾�ʽ�ƻ�����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.�ֹ�˾�ʽ�ƻ�����);
                case "�ʽ�ƻ���ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.�ʽ�ƻ���ѯ);
                case "�ʽ�ƻ�����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CIndirectCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.IndirectCostExecType.�ʽ�ƻ�����);

                case "��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CBorrowedOrder(framework).Excute();
                case "����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost.CBorrowedOrder(framework).Excute(EnumBorrowedOrder.����ѯ);

                case "������":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFactoringData(framework).Excute();
                case "����̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFactoringData(framework).Excute("Report");
                case "��Ŀ��֧������":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.������Ŀ��֧������);
                case "�ʽ����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.�ʽ����);
                case "ʱ���ڼ䶨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.ʱ���ڼ䶨��);
                case "��֤��/Ѻ��̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CGatheringMng_ExecType.�ո��֤��Ѻ��̨��);
                case "ʩ���ڵ����ά��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.ʩ���ڵ����ά��);
                case "ʩ���ڵ���Ȳ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.ʩ���ڵ���Ȳ�ѯ);
                case "��Ŀ����ȷ��":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.��Ŀ����ȷ��);
                case "�ʽ�߻���":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.�ʽ�߻���);
                case "�ʽ�߻�����ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.�ʽ�߻�����ѯ);
                case "�ʽ�����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.�ʽ�����);
                case "�ʽ�߻�������":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.�ʽ�߻�������);
                case "��ʵ���������":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.CFinanceMultData(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData.FinanceMultDataExecType.��ʵ���������);
                case "�ʽ�ƻ��걨":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ�ƻ��걨);
                case "�ʽ�ƻ�����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ�ƻ�����);
                case "�ʽ�֧��������":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ�֧��������);
                case "�ʽ�֧�����������":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ�֧�����������);
                case "�ʽ�֧�����뵥����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ�֧�����뵥����);
                case "�ʽ�֧�����뵥���":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ�֧�����뵥���);
                case "�ʽ�֧�����뵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ�֧�����뵥��ѯ);
                case "�ʽ���Ϣ����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ���Ϣ����);
                case "�ʽ𿼺˲�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ𿼺˲�ѯ);
                case "�ʽ𿼺˶���":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ𿼺˶���);
                case "�ʽ�߻���Ч����":
                    return new Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.CFundPlan(framework).Excute(Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan.FundPlanExecType.�ʽ�߻���Ч����);

                #endregion

                #region ���Ͼ�
                case "���Ͼߺ�ͬ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.�Ͼ����޺�ͬ);
                case "���Ͼߺ�ͬ��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.�Ͼ����޺�ͬ��ѯ);
                case "���Ͼ����޺�ͬ����":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.�Ͼ����޸���);
                case "���Ͼ����޺�ͬ����":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.�Ͼ����޺�ͬ����);
                case "�Ͼ߷�浥":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.�Ͼ߷�浥);
                case "�Ͼ߷�浥(�ֹ�)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.�ֹܷ�浥);
                case "�Ͼ߷�浥(���)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.��۷�浥);
                case "�Ͼ߷�浥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.CMaterialHireOrder(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder.EnumMaterialHireOrder_ExecType.�Ͼ߷�浥��ѯ);

                case "���Ͼ߷��ϵ�":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(args);
                case "���Ͼ߷��ϵ�(�ֹ�)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireCollectionGGCheck);
                case "���Ͼ߷��ϵ�(���)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireCollectionWKCheck);
                case "�·��ϵ�ͳ�Ʋ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireCollectionQuery);
                case "���Ͼ������½�":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireMonthlyBalance);
                case "���Ͼ������½��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection.CMaterialHireCollection_ExecType.MaterialHireMonthlyQuery);
                case "���Ͼ����ϵ�":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireReturn);
                case "���Ͼ����ϵ�(���)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireReturnLoss);
                case "���Ͼ����ϵ�(�ֹ�)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireGGReturn);
                case "���Ͼ����ϵ�(�ֹ�)(���)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireGGReturnLoss);
                case "���Ͼ����ϵ�(���)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireWKReturn);
                case "���Ͼ����ϵ�(���)(���)":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireWKReturnLoss);
                case "�����ϵ�ͳ�Ʋ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireReturnQuery);
                case "һ�����¸ֹ����ϲ�ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireReturn.CMaterialHireReturn(framework).Excute(CMaterialHireReturn_ExecType.MaterialHireReturnGGLessOneQuery);
                case "���䵥":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost.CMaterialHireTransportCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost.CMaterialHireTransportCost_ExecType.�����);
                case "���䵥��ѯ":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost.CMaterialHireTransportCost(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost.CMaterialHireTransportCost_ExecType.����Ѳ�ѯ);
               
               
                case "���Ͼ�����̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.�Ͼ�����̨��);
                case "�Ͼ���������":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.�Ͼ���������);
                case "���޽���̨��":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.���޽���̨��);
                case "�ߴ�ֶ�ͳ�Ʊ�":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.�ߴ�ֶ�ͳ�Ʊ�);
                case "�Ͼ߷ֲ�����":
                    return new Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport(framework).Excute(Application.Business.Erp.SupplyChain.Client.MaterialHireMng.Report.CReport_ExecType.�Ͼ߷ֲ�����);
                
                #endregion

                #region
                //case "�ӹ��ܲ���" :
                //    return new Application.Business.Erp.SupplyChain.Client.Test.CTest(framework).Execute("�ӹ��ܲ���");
                #endregion
            }
            return null;
        }
    }
}