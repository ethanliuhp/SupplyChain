using System;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class FundSchemeGathering : Base.Domain.BaseDetail
    {
        private int _year;

        /// <summary>年份</summary>
        public virtual int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        private int _month;

        /// <summary>月份</summary>
        public virtual int Month
        {
            get { return _month; }
            set { _month = value; }
        }

        private string _jobNameLink;

        /// <summary>施工任务名称联接</summary>
        public virtual string JobNameLink
        {
            get { return _jobNameLink; }
            set { _jobNameLink = value; }
        }

        private decimal _currentVoluntarilyAmount;

        /// <summary>本期自行报量</summary>
        public virtual decimal CurrentVoluntarilyAmount
        {
            get { return _currentVoluntarilyAmount; }
            set { _currentVoluntarilyAmount = value; }
        }

        private decimal _currentOptimizeAmount;

        /// <summary>本期优化报量</summary>
        public virtual decimal CurrentOptimizeAmount
        {
            get { return _currentOptimizeAmount; }
            set { _currentOptimizeAmount = value; }
        }

        private decimal _currentInnerSetUp;

        /// <summary>本期内部安装报量</summary>
        public virtual decimal CurrentInnerSetUp
        {
            get { return _currentInnerSetUp; }
            set { _currentInnerSetUp = value; }
        }

        private decimal _currentSubContract;

        /// <summary>本期甲分包报量</summary>
        public virtual decimal CurrentSubContract
        {
            get { return _currentSubContract; }
            set { _currentSubContract = value; }
        }

        private decimal _currentNoTaxAmount;

        /// <summary>本期不含税价报量</summary>
        public virtual decimal CurrentNoTaxAmount
        {
            get { return _currentNoTaxAmount; }
            set { _currentNoTaxAmount = value; }
        }

        private decimal _currentOutputTax;

        /// <summary>本期销项税金报量</summary>
        public virtual decimal CurrentOutputTax
        {
            get { return _currentOutputTax; }
            set { _currentOutputTax = value; }
        }

        private decimal _currentSubtotalAmount;

        /// <summary>本期报量小计</summary>
        public virtual decimal CurrentSubtotalAmount
        {
            get { return _currentSubtotalAmount; }
            set { _currentSubtotalAmount = value; }
        }

        private decimal _totalVoluntarilyAmount;

        /// <summary>累计自行报量</summary>
        public virtual decimal TotalVoluntarilyAmount
        {
            get { return _totalVoluntarilyAmount; }
            set { _totalVoluntarilyAmount = value; }
        }

        private decimal _totalOptimizeAmount;

        /// <summary>累计优化报量</summary>
        public virtual decimal TotalOptimizeAmount
        {
            get { return _totalOptimizeAmount; }
            set { _totalOptimizeAmount = value; }
        }

        private decimal _totalInnerSetUp;

        /// <summary>累计内部安装报量</summary>
        public virtual decimal TotalInnerSetUp
        {
            get { return _totalInnerSetUp; }
            set { _totalInnerSetUp = value; }
        }

        private decimal _totalSubContract;

        /// <summary>累计甲分包报量</summary>
        public virtual decimal TotalSubContract
        {
            get { return _totalSubContract; }
            set { _totalSubContract = value; }
        }

        private decimal _totalOutputTax;

        /// <summary>累计销项税金报量</summary>
        public virtual decimal TotalOutputTax
        {
            get { return _totalOutputTax; }
            set { _totalOutputTax = value; }
        }

        private decimal _totalNoTaxAmount;

        /// <summary>累计不含税价报量</summary>
        public virtual decimal TotalNoTaxAmount
        {
            get { return _totalNoTaxAmount; }
            set { _totalNoTaxAmount = value; }
        }

        private decimal _totalSubtotalAmount;
        /// <summary>累计小计</summary>
        public virtual decimal TotalSubtotalAmount
        {
            get { return _totalSubtotalAmount; }
            set { _totalSubtotalAmount = value; }
        }

        private decimal _currentVoluntarilyGether;

        /// <summary>本期自行收款</summary>
        public virtual decimal CurrentVoluntarilyGether
        {
            get { return _currentVoluntarilyGether; }
            set { _currentVoluntarilyGether = value; }
        }

        private decimal _currentInnerSetUpGether;

        /// <summary>本期内部安装收款</summary>
        public virtual decimal CurrentInnerSetUpGether
        {
            get { return _currentInnerSetUpGether; }
            set { _currentInnerSetUpGether = value; }
        }

        private decimal _currentSubContractGether;

        /// <summary>本期甲分包收款</summary>
        public virtual decimal CurrentSubContractGether
        {
            get { return _currentSubContractGether; }
            set { _currentSubContractGether = value; }
        }

        private decimal _currentOutputTaxGether;

        /// <summary>本期销项税金收款</summary>
        public virtual decimal CurrentOutputTaxGether
        {
            get { return _currentOutputTaxGether; }
            set { _currentOutputTaxGether = value; }
        }

        private decimal _currentGetherTotal;

        /// <summary>本期收款合计</summary>
        public virtual decimal CurrentGetherTotal
        {
            get { return _currentGetherTotal; }
            set { _currentGetherTotal = value; }
        }

        private decimal _totalVoluntarilyGether;

        /// <summary>累计自行收款</summary>
        public virtual decimal TotalVoluntarilyGether
        {
            get { return _totalVoluntarilyGether; }
            set { _totalVoluntarilyGether = value; }
        }

        private decimal _totalInnerSetUpGether;

        /// <summary>累计内部安装收款</summary>
        public virtual decimal TotalInnerSetUpGether
        {
            get { return _totalInnerSetUpGether; }
            set { _totalInnerSetUpGether = value; }
        }

        private decimal _totalSubContractGether;

        /// <summary>累计甲分包收款</summary>0
        public virtual decimal TotalSubContractGether
        {
            get { return _totalSubContractGether; }
            set { _totalSubContractGether = value; }
        }

        private decimal _totalOutputTaxGether;

        /// <summary>累计销项税金收款</summary>
        public virtual decimal TotalOutputTaxGether
        {
            get { return _totalOutputTaxGether; }
            set { _totalOutputTaxGether = value; }
        }

        private decimal _totalGetherWithTax;

        /// <summary>累计收含税价款小计</summary>
        public virtual decimal TotalGetherWithTax
        {
            get { return _totalGetherWithTax; }
            set { _totalGetherWithTax = value; }
        }

        private decimal _contractGetherRate;

        /// <summary>合同约定收款比率</summary>
        public virtual decimal ContractGetherRate
        {
            get { return _contractGetherRate; }
            set { _contractGetherRate = value; }
        }

        private int _rowIndex;
        /// <summary>行序号</summary>
        public virtual int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        private string _itemGuid;
        /// <summary>行标识</summary>
        public virtual string ItemGuid
        {
            get { return _itemGuid; }
            set { _itemGuid = value; }
        }
    }
}
