using System;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class DatePeriodDefine : BaseMaster
    {
        private string periodCode;
        private string periodName;
        private DateTime beginDate;
        private DateTime endDate;
        private DatePeriodDefine parentPeriod;
        private PeriodType periodType;

        public virtual string PeriodCode
        {
            get { return periodCode; }
            set { periodCode = value; }
        }

        public virtual string PeriodName
        {
            get { return periodName; }
            set { periodName = value; }
        }

        public virtual DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }

        public virtual DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public virtual PeriodType PeriodType
        {
            get { return periodType; }
            set { periodType = value; }
        }

        public virtual DatePeriodDefine ParentPeriod
        {
            get { return parentPeriod; }
            set { parentPeriod = value; }
        }

        public virtual string FormattedPeriodCode
        {
            get
            {
                return PeriodCode.PadLeft(PeriodCode.Length + (int)PeriodType, ' ');
            }
        }

        public virtual string FormattedPeriodName
        {
            get
            {
                return PeriodName.PadLeft(PeriodName.Length + (int)PeriodType, ' ');
            }
        }
    }

    public enum PeriodType
    {
        InValid = -1,
        Year = 0,
        Quarter = 1,
        Month = 2,
        Week = 3,
        Day = 4
    }
}
