using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteCheckingMng.Domain
{
    /// <summary>
    /// 商品砼对账单明细
    /// </summary>
    [Serializable]
    public class ConcreteCheckingDetail : BaseDetail
    {
        private decimal conversionVolume;
        private decimal deductionVolume;
        private decimal lessPumpVolume;
        private bool isPump;
        private decimal ticketVolume;
        private decimal balVolume;
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

      
        private string subjectName;

        private string subjectSysCode;
        /// <summary>
        /// 核算科目
        /// </summary>
       virtual public CostAccountSubject SubjectGUID
        {
            get { return subjectGUID; }
            set { subjectGUID = value; }
        }
        /// <summary>
        /// 层次码
        /// </summary>
       virtual public string SubjectSysCode
        {
            get { return subjectSysCode; }
            set { subjectSysCode = value; }
        }
        //核算科目名称
       virtual public string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }
        /// <summary>
        /// 换算抽磅方量
        /// </summary>
        virtual public decimal ConversionVolume
        {
            get { return conversionVolume; }
            set { conversionVolume = value; }
        }
        /// <summary>
        /// 其他扣减
        /// </summary>
        virtual public decimal DeductionVolume
        {
            get { return deductionVolume; }
            set { deductionVolume = value; }
        }
        /// <summary>
        /// 抽磅扣减
        /// </summary>
        virtual public decimal LessPumpVolume
        {
            get { return lessPumpVolume; }
            set { lessPumpVolume = value; }
        }
        /// <summary>
        /// 是否泵送
        /// </summary>
        virtual public bool IsPump
        {
            get { return isPump; }
            set { isPump = value; }
        }
        /// <summary>
        /// 小票方量
        /// </summary>
        virtual public decimal TicketVolume
        {
            get { return ticketVolume; }
            set { ticketVolume = value; }
        }
        /// <summary>
        /// 应结方量
        /// </summary>
        virtual public decimal BalVolume
        {
            get { return balVolume; }
            set { balVolume = value; }
        }
    }
}
