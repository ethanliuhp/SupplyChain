using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain
{
    /// <summary>
    /// 商品砼结算明细基类
    /// </summary>
    [Serializable]
    public class BaseConcreteBalanceDetail : BaseDetail
    {
        private decimal checkingVolume;
        private decimal banlanceVolume;
        private bool isPump;
        private decimal planQuantity;
        /// <summary>
        /// 计划数量
        /// </summary>
        virtual public decimal PlanQuantity
        {
            get { return planQuantity; }
            set { planQuantity = value; }
        }
        private CostAccountSubject subjectGUID;
        /// <summary>
        /// 核算科目
        /// </summary>
        virtual public CostAccountSubject SubjectGUID
        {
            get { return subjectGUID; }
            set { subjectGUID = value; }
        }
        
        private string subjectName;
        /// <summary>
        /// 核算科目名称
        /// </summary>
      virtual  public string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }
        private string subjectSysCode;
        /// <summary>
        /// 层次码
        /// </summary>
       virtual public string SubjectSysCode
        {
            get { return subjectSysCode; }
            set { subjectSysCode = value; }
        }
        /// <summary>
        /// 对账单方量
        /// </summary>
        virtual public decimal CheckingVolume
        {
            get { return checkingVolume; }
            set { checkingVolume = value; }
        }
        /// <summary>
        /// 结算方量
        /// </summary>
        virtual public decimal BalanceVolume
        {
            get { return banlanceVolume; }
            set { banlanceVolume = value; }
        }
        /// <summary>
        /// 是否泵送
        /// </summary>
        virtual public bool IsPump
        {
            get { return isPump; }
            set { isPump = value; }
        }
    }
}
