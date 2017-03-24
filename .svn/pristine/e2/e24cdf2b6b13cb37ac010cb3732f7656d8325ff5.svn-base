using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class ConstructNode : BaseMaster
    {
        private int year;
        private int month;
        private DateTime beginDate;
        private DateTime endDate;
        private DatePeriodDefine datePeriod;
        private decimal rate;
        private decimal currentRate;
        private string wBSName;
        private GWBSTree wBSTree;
        private bool isChecked;

        public virtual bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public virtual int Year
        {
            get { return year; }
            set { year = value; }
        }

        public virtual int Month
        {
            get { return month; }
            set { month = value; }
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

        public virtual decimal Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        public virtual decimal CurrentRate
        {
            get { return currentRate; }
            set { currentRate = value; }
        }

        public virtual DatePeriodDefine DatePeriod
        {
            get { return datePeriod; }
            set { datePeriod = value; }
        }

        public virtual string WBSName
        {
            get { return wBSName; }
            set { wBSName = value; }
        }

        public virtual GWBSTree WBSTree
        {
            get { return wBSTree; }
            set { wBSTree = value; }
        }

        public virtual string PeriodCode
        {
            get
            {
                if (datePeriod != null)
                {
                    return datePeriod.PeriodCode;
                }
                else
                {
                    return "<双击设置期间>";
                }
            }
        }

        public virtual string PeriodName
        {
            get
            {
                if (datePeriod != null)
                {
                    return datePeriod.PeriodName;
                }
                else
                {
                    return "<双击设置期间>";
                }
            }
        }
    }
}
