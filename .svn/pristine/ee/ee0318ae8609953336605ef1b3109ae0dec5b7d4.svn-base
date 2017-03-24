using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    [Entity]
    public class FilialeFundPlanDetail : BaseDetail
    {
        private decimal _thisMonthInstallFilialePayment;

        /// <summary>本月安装分公司付款</summary>
        public virtual decimal ThisMonthInstallFilialePayment
        {
            get { return _thisMonthInstallFilialePayment; }
            set { _thisMonthInstallFilialePayment = value; }
        }

        private decimal _presentMonthPlanPayment;

        /// <summary>本月计划付款</summary>
        public virtual decimal PresentMonthPlanPayment
        {
            get { return _presentMonthPlanPayment; }
            set { _presentMonthPlanPayment = value; }
        }

        private decimal _quota;

        /// <summary>分配额</summary>
        public virtual decimal Quota
        {
            get { return _quota; }
            set { _quota = value; }
        }

        private decimal _cumulativeExpireDue;

        /// <summary>累计到期应付款</summary>
        public virtual decimal CumulativeExpireDue
        {
            get { return _cumulativeExpireDue; }
            set { _cumulativeExpireDue = value; }
        }

        private decimal _cumulativePayment;

        /// <summary>累计付款额</summary>
        public virtual decimal CumulativePayment
        {
            get { return _cumulativePayment; }
            set { _cumulativePayment = value; }
        }

        private decimal _cumulativeSettlement;

        /// <summary>累计结算额</summary>
        public virtual decimal CumulativeSettlement
        {
            get { return _cumulativeSettlement; }
            set { _cumulativeSettlement = value; }
        }

        private decimal _cumulativeArrears;

        /// <summary>累计欠款额</summary>
        public virtual decimal CumulativeArrears
        {
            get { return _cumulativeArrears; }
            set { _cumulativeArrears = value; }
        }

        private string _projectCategory;

        /// <summary>项目类别</summary>
        public virtual string ProjectCategory
        {
            get { return _projectCategory; }
            set { _projectCategory = value; }
        }
    }
}
