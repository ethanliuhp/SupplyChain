using System;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class ConstructNodeSubject : BaseMaster
    {
        private string _wBSName;
        private GWBSTree _wBSTree;

        public virtual string WbsName
        {
            get { return _wBSName; }
            set { _wBSName = value; }
        }

        public virtual GWBSTree WBSTree
        {
            get { return _wBSTree; }
            set { _wBSTree = value; }
        }

        private CostAccountSubject _subject;

        /// <summary>科目</summary>
        public virtual CostAccountSubject Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private string _subjectCode;

        /// <summary>科目编码</summary>
        public virtual string SubjectCode
        {
            get { return _subjectCode; }
            set { _subjectCode = value; }
        }

        private string _subjectName;

        /// <summary>科目名称</summary>
        public virtual string SubjectName
        {
            get { return _subjectName; }
            set { _subjectName = value; }
        }

        private int year;
        public virtual int Year
        {
            get { return year; }
            set { year = value; }
        }

        private int month;
        public virtual int Month
        {
            get { return month; }
            set { month = value; }
        }

        private DateTime beginDate;
        public virtual DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }

        private DateTime endDate;
        public virtual DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private DatePeriodDefine datePeriod;
        public virtual DatePeriodDefine DatePeriod
        {
            get { return datePeriod; }
            set { datePeriod = value; }
        }

        private decimal _currentRate;

        /// <summary>本期进度</summary>
        public virtual decimal CurrentRate
        {
            get { return _currentRate; }
            set { _currentRate = value; }
        }

        private decimal _rate;

        /// <summary>累计进度</summary>
        public virtual decimal Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }

        private bool isChecked;

        public virtual bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
    }
}
