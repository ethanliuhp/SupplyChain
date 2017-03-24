using System;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.ComponentModel;
using Application.Resource.FinancialResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Domain
{
    /// <summary>
    /// 业务环节，用于确定费用发生的业务环节
    /// </summary>
    public enum enmOperationTache
    {
        采购环节,
        销售环节,
        通用
    }
    public enum enmFiscalTache
    {
        /// <summary>
        /// 采购费用记录
        /// </summary>
        [Description("采购费用记录")]
        SupplyExpRec = 1,

        /// <summary>
        /// 销售费用记录
        /// </summary>
        [Description("销售费用记录")]
        SaleExpRec = 2,

        /// <summary>
        /// 进项发票
        /// </summary>
        [Description("进项发票")]
        IncomeInvoice = 3,

        /// <summary>
        /// 销项发票
        /// </summary>
        [Description("销项发票")]
        SaleInvoice = 4,

        /// <summary>
        /// 其它销项发票
        /// </summary>
        [Description("其它销项发票")]
        HoistInvoice = 5,

        /// <summary>
        /// 包装发票
        /// </summary>
        [Description("包装发票")]
        PackInvoice = 6

    }
    public enum enmExpItemType
    {
        /// <summary>
        /// 采购货款
        /// </summary>
        [Description("采购货款")]
        stockmoney  = 1,

        /// <summary>
        /// 委外加工费用
        /// </summary>
        [Description("委外加工费用")]
        outMakemoney = 2,

        /// <summary>
        /// 销售货款
        /// </summary>
        [Description("销售货款")]
        salemoney = 3,

        /// <summary>
        /// 来料加工费
        /// </summary>
        [Description("来料加工费")]
        Processingmoney = 4,

        /// <summary>
        /// 销售吊费
        /// </summary>
        [Description("销售吊费")]
        Hoistingmoney = 5,

        /// <summary>
        /// 其他费用
        /// </summary>
        [Description("其他费用")]
        other = 6,

        /// <summary>
        /// 包装费用
        /// </summary>
        [Description("包装费用")]
        Packmoney = 7,
    }

    /// <summary>
    /// 费用项目
    /// </summary>
    [Serializable]
    public class ExpenseItem : BaseBasicData
    {
        private decimal taxRate=0;
        private enmOperationTache operTache;
        private enmExpItemType expItemType;

        private bool _StockLine;
        private bool _SaleLine;
        private bool _InComeExp;
        private bool _OutPutExp;
        private bool _Hoisting;

        private AccountTitleInfo accountTitle;

        /// <summary>
        /// 采购费用环节
        /// </summary>
        virtual public bool StockLine
        {
            get { return _StockLine; }
            set { _StockLine = value; }
        }
        /// <summary>
        /// 销售费用环节
        /// </summary>
        virtual public bool SaleLine
        {
            get { return _SaleLine; }
            set { _SaleLine = value; }
        }
        /// <summary>
        /// 进项发票
        /// </summary>
        virtual public bool InComeExp
        {
            get { return _InComeExp; }
            set { _InComeExp = value; }
        }
        /// <summary>
        /// 销项发票
        /// </summary>
        virtual public bool OutPutExp
        {
            get { return _OutPutExp; }
            set { _OutPutExp = value; }
        }
        /// <summary>
        /// 吊装费
        /// </summary>
        virtual public bool Hoisting 
        {
            get { return _Hoisting; }
            set { _Hoisting = value; }
        }
        /// <summary>
        /// 业务环节
        /// </summary>
        virtual public enmOperationTache OperTache
        {
            get { return operTache; }
            set { operTache = value; }
        }

        /// <summary>
        /// 费用税率
        /// </summary>
        virtual public decimal TaxRate
        {
            get { return taxRate; }
            set { taxRate = value; }
        }
        /// <summary>
        /// 费用类型
        /// </summary>
        virtual public enmExpItemType ExpItemType
        {
            get { return expItemType; }
            set { expItemType = value; }
        }

        /// <summary>
        /// 费用科目
        /// </summary>
        virtual public AccountTitleInfo AccountTitle
        {
            get { return accountTitle; }
            set { accountTitle = value; }
        }

        /// <summary>
        /// 用于下拉显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 根据ID判断相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            ExpenseItem temp = obj as ExpenseItem;
            if (temp == null)
                return false;

            if (this.Id == temp.Id)
                return true;

            return false;
        }

    }
}
