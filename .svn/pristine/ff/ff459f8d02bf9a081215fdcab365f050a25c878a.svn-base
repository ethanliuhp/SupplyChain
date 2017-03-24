using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    [Entity]
    public class ProjectFundPlanMaster : BaseMaster
    {
        private decimal _contractAccountsDue;

        /// <summary>按合同应收款</summary>
        public virtual decimal ContractAccountsDue
        {
            get { return _contractAccountsDue; }
            set { _contractAccountsDue = value; }
        }

        private decimal _presentMonthPayment;

        /// <summary>本月付款</summary>
        public virtual decimal PresentMonthPayment
        {
            get { return _presentMonthPayment; }
            set { _presentMonthPayment = value; }
        }

        private decimal _presentMonthGathering;

        /// <summary>本月收款</summary>
        public virtual decimal PresentMonthGathering
        {
            get { return _presentMonthGathering; }
            set { _presentMonthGathering = value; }
        }

        private decimal _financeConfirmTaxIncome;

        /// <summary>财务确认含税收入</summary>
        public virtual decimal FinanceConfirmTaxIncome
        {
            get { return _financeConfirmTaxIncome; }
            set { _financeConfirmTaxIncome = value; }
        }

        private string _unit;

        /// <summary>单位</summary>
        public virtual string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        private decimal _contractAppointGatheringRatio;

        /// <summary>合同约定收款比率</summary>
        public virtual decimal ContractAppointGatheringRatio
        {
            get { return _contractAppointGatheringRatio; }
            set { _contractAppointGatheringRatio = value; }
        }

        private decimal _cumulativePayment;

        /// <summary>累计付款</summary>
        public virtual decimal CumulativePayment
        {
            get { return _cumulativePayment; }
            set { _cumulativePayment = value; }
        }

        private decimal _cumulativeGathering;

        /// <summary>累计收款</summary>
        public virtual decimal CumulativeGathering
        {
            get { return _cumulativeGathering; }
            set { _cumulativeGathering = value; }
        }

        private decimal _approvalAmount;

        /// <summary>审批额</summary>
        public virtual decimal ApprovalAmount
        {
            get { return _approvalAmount; }
            set { _approvalAmount = value; }
        }

        private decimal _actualGatheringRatio;

        /// <summary>实际收款率</summary>
        public virtual decimal ActualGatheringRatio
        {
            get { return _actualGatheringRatio; }
            set { _actualGatheringRatio = value; }
        }

        private int _isReport;

        /// <summary>是否报分公司</summary>
        public virtual int IsReport
        {
            get { return _isReport; }
            set { _isReport = value; }
        }

        private string _reportUnit;

        /// <summary>填报单位</summary>
        public virtual string ReportUnit
        {
            get { return _reportUnit; }
            set { _reportUnit = value; }
        }

        private DateTime _declareDate;

        /// <summary>申报日期</summary>
        public virtual DateTime DeclareDate
        {
            get { return _declareDate; }
            set { _declareDate = value; }
        }

        private decimal _ownerActualAffirmMeterage;

        /// <summary>业主实际确认计量</summary>
        public virtual decimal OwnerActualAffirmMeterage
        {
            get { return _ownerActualAffirmMeterage; }
            set { _ownerActualAffirmMeterage = value; }
        }

        private decimal _monthEndCumulativeFundStock;

        /// <summary>月末累计资金存量</summary>
        public virtual decimal MonthEndCumulativeFundStock
        {
            get { return _monthEndCumulativeFundStock; }
            set { _monthEndCumulativeFundStock = value; }
        }

        private decimal _fundStock;

        /// <summary>资金存量</summary>
        public virtual decimal FundStock
        {
            get { return _fundStock; }
            set { _fundStock = value; }
        }

        private OperationOrgInfo _attachBusinessOrg;

        /// <summary>
        /// 归属业务组织
        /// </summary>
        public virtual OperationOrgInfo AttachBusinessOrg
        {
            get { return _attachBusinessOrg; }
            set { _attachBusinessOrg = value; }
        }

        private string _attachBusinessOrgName;

        /// <summary>
        /// 归属业务组织名称
        /// </summary>
        public virtual string AttachBusinessOrgName
        {
            get { return _attachBusinessOrgName; }
            set { _attachBusinessOrgName = value; }
        }

        private Iesi.Collections.Generic.ISet<ProjectOtherPayPlanDetail> _otherDetails =
            new Iesi.Collections.Generic.HashedSet<ProjectOtherPayPlanDetail>();

        public virtual Iesi.Collections.Generic.ISet<ProjectOtherPayPlanDetail> OtherPayDetails
        {
            get { return _otherDetails; }
            set { _otherDetails = value; }
        }

        private string _projectState;
        /// <summary>
        /// 项目商务状态
        /// </summary>
        public virtual  string ProjectState
        {
            get { return _projectState; }
            set { _projectState = value; }
        }

        private int _projectType;
        /// <summary>
        /// 项目类型
        /// </summary>
        public virtual int ProjectType
        {
            get { return _projectType; }
            set { _projectType = value; }
        }
    }
}
