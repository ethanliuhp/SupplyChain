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
    /// 会计科目
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
        ///// 关联的上一年度的科目
        ///// </summary>
        //virtual public AccountTitle OldTitle
        //{
        //    get { return oldTitle; }
        //    set { oldTitle = value; }
        //}

        /// <summary>
        /// 核算年度
        /// </summary>
        virtual public int FiscalYear
        {
            get { return fiscalYear; }
            set { fiscalYear = value; }
        }

        /// <summary>
        /// 科目编码
        /// </summary>
        virtual public string AccountCode
        {
            get { return accountCode; }
            set { accountCode = value; }
        }

        /// <summary>
        /// 科目分级名称
        /// </summary>
        virtual public string AccLevelName
        {
            get { return accLevelName; }
            set { accLevelName = value; }
        }

        /// <summary>
        /// 助记码
        /// </summary>
        virtual public string AssisCode
        {
            get { return assisCode; }
            set { assisCode = value; }
        }

        /// <summary>
        /// 科目类型
        /// </summary>
        virtual public AccountType AccType
        {
            get { return accType; }
            set { accType = value; }
        }

        /// <summary>
        /// 余额方向 0 借方 1 贷方
        /// </summary>
        virtual public int BalanceDire
        {
            get { return balanceDire; }
            set { balanceDire = value; }
        }

        /// <summary>
        /// 账页格式
        /// </summary>
        virtual public BookStyle ShowStyle
        {
            get { return showStyle; }
            set { showStyle = value; }
        }

        /// <summary>
        /// 现金科目
        /// </summary>
        virtual public CashAccTitle AboutCash
        {
            get { return aboutCash; }
            set { aboutCash = value; }
        }

        /// <summary>
        /// 外币核算
        /// </summary>
        virtual public bool ForeignAccount
        {
            get { return foreignAccount; }
            set { foreignAccount = value; }
        }

        /// <summary>
        /// 核算外币币种
        /// </summary>
        virtual public CurrencyInfo ForeignCurrency
        {
            get { return foreignCurrency; }
            set { foreignCurrency = value; }
        }

        /// <summary>
        /// 数量核算
        /// </summary>
        virtual public bool QuantityAccount
        {
            get { return quantityAccount; }
            set { quantityAccount = value; }
        }

        /// <summary>
        /// 计量单位
        /// </summary>
        virtual public string QuantityUnit
        {
            get { return quantityUnit; }
            set { quantityUnit = value; }
        }

        /// <summary>
        /// 规格型号
        /// </summary>
        virtual public string QuantityDesc
        {
            get { return quantityDesc; }
            set { quantityDesc = value; }
        }

        /// <summary>
        /// 日记账
        /// </summary>
        virtual public bool DailyAccBook
        {
            get { return dailyAccBook; }
            set { dailyAccBook = value; }
        }

        /// <summary>
        /// 银行账
        /// </summary>
        virtual public bool BankAccBook
        {
            get { return bankAccBook; }
            set { bankAccBook = value; }
        }

        /// <summary>
        /// 部门核算
        /// </summary>
        virtual public bool DepartmentAccount
        {
            get { return departmentAccount; }
            set { departmentAccount = value; }
        }

        /// <summary>
        /// 个人核算
        /// </summary>
        virtual public bool PersonAccount
        {
            get { return personAccount; }
            set { personAccount = value; }
        }

        /// <summary>
        /// 伙伴实体核算
        /// </summary>
        virtual public bool PartnerAccount
        {
            get { return partnerAccount; }
            set { partnerAccount = value; }
        }

        /// <summary>
        /// 客户关系核算
        /// </summary>
        virtual public bool ClientAccount
        {
            get { return clientAccount; }
            set { clientAccount = value; }
        }

        /// <summary>
        /// 供应关系核算
        /// </summary>
        virtual public bool SupplierAccount
        {
            get { return supplierAccount; }
            set { supplierAccount = value; }
        }

        /// <summary>
        /// 项目核算
        /// </summary>
        virtual public bool ProjectAccount
        {
            get { return projectAccount; }
            set { projectAccount = value; }
        }

        /// <summary>
        /// 往来管理 暂时不用此属性
        /// </summary>
        virtual public bool AccountCurrent
        {
            get { return accountCurrent; }
            set { accountCurrent = value; }
        }

        /// <summary>
        /// 背书人管理
        /// </summary>
        virtual public bool EndorsementManage
        {
            get { return endorsementManage; }
            set { endorsementManage = value; }
        }

        /// <summary>
        /// 预算控制
        /// </summary>
        virtual public bool BudgetManage
        {
            get { return budgetManage; }
            set { budgetManage = value; }
        }

        /// <summary>
        /// 科目冻结
        /// </summary>
        virtual public bool FreezeAccount
        {
            get { return freezeAccount; }
            set { freezeAccount = value; }
        }

        /// <summary>
        /// 对应台账类别
        /// </summary>
        virtual public DeskAccount DeskAcc
        {
            get { return deskAcc; }
            set { deskAcc = value; }
        }

        /// <summary>
        /// 归属代码
        /// </summary>
        virtual public string BelongCode
        {
            get { return belongCode; }
            set { belongCode = value; }
        }

        /// <summary>
        /// 判断科目5种辅助类型
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
        /// 判断科目辅助类型
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
        /// 科目一级科目名称
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
        /// 科目明细科目名称
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
        /// 构建值对象
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
        /// 科目复制
        /// </summary>
        /// <param name="srcCode"></param>
        /// <param name="srvLevName"></param>
        /// <returns></returns>
        virtual public AccountTitle CopyAccountTitle(string srcCode, string srvLevName, int accLev, string accSysCode, AccountTitle parentAcc)
        {
            //复制科目，还需给Level 和 SysCode 赋值 ParentNode
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
