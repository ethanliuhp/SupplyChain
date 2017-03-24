using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{

    [Serializable]
    public class FactoringDataDetail : BaseDetail
    {
        private string _DepartmentName;
        /// <summary>
        /// 单位名称
        /// </summary>

        public virtual string DepartmentName
        {
            get { return _DepartmentName; }
            set { _DepartmentName = value; }
        }
        private string _projectName;
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                _projectName = value;
            }
        }
        private string _bankName;
        /// <summary>
        /// 银行
        /// </summary>
        public virtual string BankName
        {
            get
            {
                return _bankName;
            }
            set
            {
                _bankName = value;
            }
        }
        private decimal _balance = 0;
        /// <summary>
        /// 余额
        /// </summary>
        public virtual decimal Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;
            }
        }
        private decimal _rate = 0;
        /// <summary>
        /// 费率
        /// </summary>
        public virtual decimal Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
            }
        }
        private DateTime _startDate = StringUtil.StrToDateTime("1900-01-01");
        /// <summary>
        /// 起始日期
        /// </summary>
        public virtual DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }
        private DateTime _endDate = StringUtil.StrToDateTime("1900-01-01");
        /// <summary>
        /// 终止日期
        /// </summary>
        public virtual DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
            }
        }

        private string _payType;
        /// <summary>
        /// 付费方式
        /// </summary>
        public virtual string PayType
        {
            get
            {
                return _payType;
            }
            set
            {
                _payType = value;
            }
        }
        private DateTime _startChargingDate = StringUtil.StrToDateTime("1900-01-01");
        /// <summary>
        /// 计费起始日
        /// </summary>
        public virtual DateTime StartChargingDate
        {
            get
            {
                return _startChargingDate;
            }
            set
            {
                _startChargingDate = value;
            }
        }
        private DateTime _endChargingDate = StringUtil.StrToDateTime("1900-01-01");
        /// <summary>
        /// 计费终止日
        /// </summary>
        public virtual DateTime EndChargingDate
        {
            get
            {
                return _endChargingDate;
            }
            set
            {
                _endChargingDate = value;
            }
        }

        private int _totalDay = 0;
        /// <summary>
        /// 天数
        /// </summary>
        public virtual int TotalDay
        {
            get
            {
                return _totalDay;
            }
            set
            {
                _totalDay = value;
            }
        }

        private decimal _amountPayable = 0;
        /// <summary>
        /// 应付金额
        /// </summary>
        public virtual decimal AmountPayable
        {
            get
            {
                return _amountPayable;
            }
            set
            {
                _amountPayable = value;
            }
        }


    }
}
