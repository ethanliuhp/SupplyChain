using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.FinancialResource.RelateClass;
using Application.Business.Erp.Financial.BasicAccount.AssisAccount.Domain;
using Application.Business.Erp.Financial.BasicAccount.DailyOperation.ValueObject;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain
{
    /// <summary>
    /// ��ƿ�Ŀ
    /// </summary>
    [Serializable]
    [Entity]
    public class AccountTitle : CategoryNode
    {
        private int fiscalYear;
        private string accountCode;
        private string accLevelName;
        private string assisCode;
        private AccountType accType;
        private int balanceDire;
        private BookStyle showStyle;
        private CashAccTitle aboutCash;
        private bool foreignAccount = false;
        private CurrencyInfo foreignCurrency;
        private bool quantityAccount = false;
        private string quantityUnit;
        private string quantityDesc;
        private bool dailyAccBook = false;
        private bool bankAccBook = false;
        private bool departmentAccount = false;
        private bool personAccount = false;
        private bool partnerAccount = false;
        private bool clientAccount = false;
        private bool supplierAccount = false;
        private bool projectAccount = false;
        private bool accountCurrent = false;
        private bool endorsementManage = false;
        private bool budgetManage = false;
        private bool freezeAccount = false;
        private DeskAccount deskAcc;
        private string belongCode;
        //private AccountTitle oldTitle;

        ///// <summary>
        ///// ��������һ��ȵĿ�Ŀ
        ///// </summary>
        //virtual public AccountTitle OldTitle
        //{
        //    get { return oldTitle; }
        //    set { oldTitle = value; }
        //}

        /// <summary>
        /// �������
        /// </summary>
        virtual public int FiscalYear
        {
            get { return fiscalYear; }
            set { fiscalYear = value; }
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        virtual public string AccountCode
        {
            get { return accountCode; }
            set { accountCode = value; }
        }

        /// <summary>
        /// ��Ŀ�ּ�����
        /// </summary>
        virtual public string AccLevelName
        {
            get { return accLevelName; }
            set { accLevelName = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        virtual public string AssisCode
        {
            get { return assisCode; }
            set { assisCode = value; }
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        virtual public AccountType AccType
        {
            get { return accType; }
            set { accType = value; }
        }

        /// <summary>
        /// ���� 0 �跽 1 ����
        /// </summary>
        virtual public int BalanceDire
        {
            get { return balanceDire; }
            set { balanceDire = value; }
        }

        /// <summary>
        /// ��ҳ��ʽ
        /// </summary>
        virtual public BookStyle ShowStyle
        {
            get { return showStyle; }
            set { showStyle = value; }
        }

        /// <summary>
        /// �ֽ��Ŀ
        /// </summary>
        virtual public CashAccTitle AboutCash
        {
            get { return aboutCash; }
            set { aboutCash = value; }
        }

        /// <summary>
        /// ��Һ���
        /// </summary>
        virtual public bool ForeignAccount
        {
            get { return foreignAccount; }
            set { foreignAccount = value; }
        }

        /// <summary>
        /// ������ұ���
        /// </summary>
        virtual public CurrencyInfo ForeignCurrency
        {
            get { return foreignCurrency; }
            set { foreignCurrency = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        virtual public bool QuantityAccount
        {
            get { return quantityAccount; }
            set { quantityAccount = value; }
        }

        /// <summary>
        /// ������λ
        /// </summary>
        virtual public string QuantityUnit
        {
            get { return quantityUnit; }
            set { quantityUnit = value; }
        }

        /// <summary>
        /// ����ͺ�
        /// </summary>
        virtual public string QuantityDesc
        {
            get { return quantityDesc; }
            set { quantityDesc = value; }
        }

        /// <summary>
        /// �ռ���
        /// </summary>
        virtual public bool DailyAccBook
        {
            get { return dailyAccBook; }
            set { dailyAccBook = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        virtual public bool BankAccBook
        {
            get { return bankAccBook; }
            set { bankAccBook = value; }
        }

        /// <summary>
        /// ���ź���
        /// </summary>
        virtual public bool DepartmentAccount
        {
            get { return departmentAccount; }
            set { departmentAccount = value; }
        }

        /// <summary>
        /// ���˺���
        /// </summary>
        virtual public bool PersonAccount
        {
            get { return personAccount; }
            set { personAccount = value; }
        }

        /// <summary>
        /// ���ʵ�����
        /// </summary>
        virtual public bool PartnerAccount
        {
            get { return partnerAccount; }
            set { partnerAccount = value; }
        }

        /// <summary>
        /// �ͻ���ϵ����
        /// </summary>
        virtual public bool ClientAccount
        {
            get { return clientAccount; }
            set { clientAccount = value; }
        }

        /// <summary>
        /// ��Ӧ��ϵ����
        /// </summary>
        virtual public bool SupplierAccount
        {
            get { return supplierAccount; }
            set { supplierAccount = value; }
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        virtual public bool ProjectAccount
        {
            get { return projectAccount; }
            set { projectAccount = value; }
        }

        /// <summary>
        /// �������� ��ʱ���ô�����
        /// </summary>
        virtual public bool AccountCurrent
        {
            get { return accountCurrent; }
            set { accountCurrent = value; }
        }

        /// <summary>
        /// �����˹���
        /// </summary>
        virtual public bool EndorsementManage
        {
            get { return endorsementManage; }
            set { endorsementManage = value; }
        }

        /// <summary>
        /// Ԥ�����
        /// </summary>
        virtual public bool BudgetManage
        {
            get { return budgetManage; }
            set { budgetManage = value; }
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        virtual public bool FreezeAccount
        {
            get { return freezeAccount; }
            set { freezeAccount = value; }
        }

        /// <summary>
        /// ��Ӧ̨�����
        /// </summary>
        virtual public DeskAccount DeskAcc
        {
            get { return deskAcc; }
            set { deskAcc = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        virtual public string BelongCode
        {
            get { return belongCode; }
            set { belongCode = value; }
        }

        /// <summary>
        /// �жϿ�Ŀ5�ָ�������
        /// </summary>
        /// <returns></returns>
        virtual public int AssisType()
        {
            string typeStr = "";
            if (DepartmentAccount)
            {
                typeStr = "1";
            }
            if (PersonAccount)
            {
                typeStr += "2";
            }
            if (ClientAccount)
            {
                typeStr += "3";
            }
            if (SupplierAccount)
            {
                typeStr += "4";
            }
            if (ProjectAccount)
            {
                typeStr += "5";
            }

            if (typeStr != "")
            {
                return int.Parse(typeStr);
            }
            return 0;
        }

        /// <summary>
        /// �жϿ�Ŀ��������
        /// </summary>
        /// <returns></returns>
        virtual public int CheckAccTitleAssisType()
        {
            string typeStr = "";
            if (DepartmentAccount)
            {
                typeStr = "1";
            }
            if (PersonAccount)
            {
                typeStr += "2";
            }
            if (ClientAccount)
            {
                typeStr += "3";
            }
            if (SupplierAccount)
            {
                typeStr += "4";
            }
            if (ProjectAccount)
            {
                typeStr += "5";
            }
            if (ForeignAccount)
            {
                typeStr += "8";
            }
            if (QuantityAccount)
            {
                typeStr += "9";
            }

            if (typeStr != string.Empty)
            {
                return int.Parse(typeStr);
            }
            return 0;
        }

        /// <summary>
        /// ��Ŀһ����Ŀ����
        /// </summary>
        virtual public string AccFirName
        {
            get
            {
                if (AccLevelName == null)
                    return null;
                if (AccLevelName.IndexOf("_") <= 0)
                {
                    return Name;
                }
                else
                {
                    return AccLevelName.Substring(0, AccLevelName.IndexOf("_"));
                }
            }
        }

        /// <summary>
        /// ��Ŀ��ϸ��Ŀ����
        /// </summary>
        virtual public string AccDetName
        {
            get
            {
                if (AccLevelName == null)
                    return null;
                if (AccLevelName.IndexOf("_") <= 0)
                {
                    return "";
                }
                else
                {
                    return AccLevelName.Substring(AccLevelName.IndexOf("_") + 1);
                }
            }
        }

        /// <summary>
        /// ����ֵ����
        /// </summary>
        /// <returns></returns>
        virtual public AccTitleVob ConsValueObj()
        {
            AccTitleVob accInfo = new AccTitleVob();
            accInfo.Id = Id;
            accInfo.AccAssisType = CheckAccTitleAssisType();
            accInfo.AccCode = AccountCode;
            accInfo.AccName = Name;
            if (AccLevelName.IndexOf("_") <= 0)
            {
                accInfo.AccFirName = Name;
                accInfo.AccDetName = "";
            }
            else
            {
                int ind = AccLevelName.IndexOf("_");
                accInfo.AccFirName = AccLevelName.Substring(0, ind);
                accInfo.AccDetName = AccLevelName.Substring(ind + 1, AccLevelName.Length - ind - 1);
            }
            accInfo.BankAccBook = BankAccBook;
            accInfo.DeskAccId = DeskAcc.Id;
            return accInfo;
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        /// <param name="srcCode"></param>
        /// <param name="srvLevName"></param>
        /// <returns></returns>
        virtual public AccountTitle CopyAccountTitle(string srcCode, string srvLevName, int accLev, string accSysCode, AccountTitle parentAcc)
        {
            //���ƿ�Ŀ�������Level �� SysCode ��ֵ ParentNode
            AccountTitle rtnAcc = new AccountTitle();
            rtnAcc.AboutCash = AboutCash;
            rtnAcc.AccountCurrent = AccountCurrent;
            rtnAcc.AccType = AccType;
            rtnAcc.AssisCode = "";
            rtnAcc.Author = Author;
            rtnAcc.BalanceDire = BalanceDire;
            rtnAcc.BankAccBook = BankAccBook;
            rtnAcc.BelongCode = BelongCode;
            rtnAcc.BudgetManage = BudgetManage;
            rtnAcc.CategoryNodeType = CategoryNodeType;
            rtnAcc.ClientAccount = ClientAccount;
            rtnAcc.CreateDate = CreateDate;
            rtnAcc.DailyAccBook = DailyAccBook;
            rtnAcc.DepartmentAccount = DepartmentAccount;
            rtnAcc.Describe = Describe;
            rtnAcc.DeskAcc = DeskAcc;
            rtnAcc.EndorsementManage = EndorsementManage;
            rtnAcc.FiscalYear = FiscalYear;
            rtnAcc.ForeignAccount = ForeignAccount;
            rtnAcc.ForeignCurrency = ForeignCurrency;
            rtnAcc.FreezeAccount = false;
            rtnAcc.Name = Name;
            rtnAcc.OrderNo = OrderNo;
            rtnAcc.PartnerAccount = PartnerAccount;
            rtnAcc.PersonAccount = PersonAccount;
            rtnAcc.ProjectAccount = ProjectAccount;
            rtnAcc.QuantityAccount = QuantityAccount;
            rtnAcc.QuantityDesc = QuantityDesc;
            rtnAcc.QuantityUnit = QuantityUnit;
            rtnAcc.ShowStyle = ShowStyle;
            rtnAcc.State = 1;
            rtnAcc.SupplierAccount = SupplierAccount;
            rtnAcc.TheTree = TheTree;

            rtnAcc.Code = srcCode;
            rtnAcc.AccountCode = srcCode;
            rtnAcc.AccLevelName = srvLevName;
            rtnAcc.Level = accLev;
            rtnAcc.SysCode = accSysCode;
            rtnAcc.ParentNode = parentAcc;

            return rtnAcc;
        }
    }
}
