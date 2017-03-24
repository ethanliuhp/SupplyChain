using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    [Serializable]
    /// <summary>
    /// 取费模板公式
    /// </summary>
    public class SelFeeTempFormula : BaseDetail
    {

        private CostAccountSubject accountSubject;
        private string accountSubjectName;
        private string accountSubjectCode;
        private string formula;


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
        /// <summary> 公式 </summary>
        public virtual string Formula
        {
            get { return formula; }
            set { formula = value; }
        }

        private string  _formulaCode;
        ///<summary></summary>  
        public virtual string  FormulaCode
        {
            set { this._formulaCode = value; }
            get { return this._formulaCode; }
        }

    }
}
