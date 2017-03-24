using System;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    [Entity]
    public class FundAssessmentMaster : Base.Domain.BaseMaster
    {
        private string _projectState;

        /// <summary>项目状态</summary>
        public virtual string ProjectState
        {
            get { return _projectState; }
            set { _projectState = value; }
        }

        private decimal _gatheringRate;
        /// <summary>收款比例</summary>
        public virtual decimal GatheringRate
        {
            get { return _gatheringRate; }
            set { _gatheringRate = value; }
        }

        private decimal _currentRealGet;

        /// <summary>累计实际资金收款</summary>
        public virtual decimal CurrentRealGet
        {
            get { return _currentRealGet; }
            set { _currentRealGet = value; }
        }

        private decimal _currentRealPay;

        /// <summary>累计实际资金付款</summary>
        public virtual decimal CurrentRealPay
        {
            get { return _currentRealPay; }
            set { _currentRealPay = value; }
        }

        private decimal _currentSchemeTarget;

        /// <summary>当月资金策划目标</summary>
        public virtual decimal CurrentSchemeTarget
        {
            get { return _currentSchemeTarget; }
            set { _currentSchemeTarget = value; }
        }

        private decimal _currentCashBalance;

        /// <summary>当月实际资金结存</summary>
        public virtual decimal CurrentCashBalance
        {
            get { return _currentCashBalance; }
            set { _currentCashBalance = value; }
        }

        private Iesi.Collections.Generic.ISet<FundAssessCashDetail> assessCashDetails = new Iesi.Collections.Generic.HashedSet<FundAssessCashDetail>();
        /// <summary>
        /// 资金策划考核兑现明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<FundAssessCashDetail> AssessCashDetails
        {
            get { return assessCashDetails; }
            set { assessCashDetails = value; }
        }

        private DateTime _queryDate;

        public virtual DateTime QueryDate
        {
            get { return _queryDate; }
            set { _queryDate = value; }
        }

        private string _schemeTarget;

        /// <summary>匹配资金策划记录</summary>
        public virtual string SchemeTarget
        {
            get { return _schemeTarget; }
            set { _schemeTarget = value; }
        }
    }
}
