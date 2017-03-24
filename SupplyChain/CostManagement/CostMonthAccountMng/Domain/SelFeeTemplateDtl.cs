using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    [Serializable]
  public   class SelFeeTemplateDtl : BaseDetail
    {
        private string specialType;
        private CostAccountSubject accountSubject;
        private string accountSubjectName;
        private string accountSubjectCode;
        private decimal rate;
        private string descript;
        private decimal beginMoney;
        private decimal endMoney;
        private CostAccountSubject mainAccSubject;
        private string mainAccSubjectName;
        private string mainAccSubjectCode;
        /// <summary> 专业类型 </summary>
        public virtual string SpecialType
        {
            set { specialType = value; }
            get { return specialType; }
        }
        /// <summary> 科目 </summary>
        public virtual CostAccountSubject AccountSubject
        {
            get { return accountSubject; }
            set { accountSubject = value; }
        }
        /// <summary> 科目名称 </summary>
        public virtual string AccountSubjectName
        {
            get { return accountSubjectName; }
            set { accountSubjectName = value; }
        }
        /// <summary> 科目编码 </summary>
        public virtual string AccountSubjectCode
        {
            get { return accountSubjectCode; }
            set { accountSubjectCode = value; }
        }
        /// <summary> 费率 </summary>
        public virtual decimal Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        /// <summary> 备注 </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
        /// <summary> 开始金额 </summary>
        public virtual decimal BeginMoney
        {
            get { return beginMoney; }
            set { beginMoney = value; }
        }
        /// <summary> 结束金额 </summary>
        public virtual decimal EndMoney
        {
            get { return endMoney; }
            set { endMoney = value; }
        }
        /// <summary> 科目大类 </summary>
        public virtual CostAccountSubject MainAccSubject
        {
            get { return mainAccSubject; }
            set { mainAccSubject = value; }
        }
        /// <summary> 科目大类名称 </summary>
        public virtual string MainAccSubjectName
        {
            get { return mainAccSubjectName; }
            set { mainAccSubjectName = value; }
        }
        /// <summary> 科目大类编码 </summary>
        public virtual string MainAccSubjectCode
        {
            get { return mainAccSubjectCode; }
            set { mainAccSubjectCode = value; }
        }
    }
}
