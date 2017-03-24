using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireBalance.Domain
{
    public enum EnumBillType
    {
        收料 = 0,
        退料 = 1,
        结存 = 2
    }
    /// <summary>
    /// 料具结算明细
    /// </summary>
    [Serializable]
    public class MatHireBalanceDetail : BaseDetail
    {
        private DateTime startDate;
        private DateTime endDate;
        private string balRule;
        private int days;
        private decimal rentalPrice;
        private decimal approachQuantity;
        private decimal exitQuantity;
        private decimal unusedBalQuantity;
        private string balState;
        private string matCollDtlId;

        private string matCollCode;
        private decimal matCollDtlQty;
        private string matReturnCode;
        private decimal matReturnDtlQty;

        private CostAccountSubject subjectGUID;
        private string subjectName;
        private string subjectSysCode;
        private EnumBillType billType;
        /// <summary>
        /// 收料 退料 结存
        /// </summary>
        public virtual EnumBillType BillType
        {
            get { return billType; }
            set { billType = value; }
        }
        /// <summary>
        /// 核算科目
        /// </summary>
        virtual public CostAccountSubject SubjectGUID
        {
            get { return subjectGUID; }
            set { subjectGUID = value; }
        }
        /// <summary>
        /// 核算科目名称
        /// </summary>
        virtual public string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }
        /// <summary>
        /// 核算科目层次码
        /// </summary>
        virtual public string SubjectSysCode
        {
            get { return subjectSysCode; }
            set { subjectSysCode = value; }
        }
        private GWBSTree usedPart;
        private string usedPartName;
        private string usedPartSysCode;
        /// <summary>
        /// 浇筑部位
        /// </summary>
        virtual public GWBSTree UsedPart
        {
            get { return usedPart; }
            set { usedPart = value; }
        }
        /// <summary>
        /// 浇筑部位名称
        /// </summary>
        virtual public string UsedPartName
        {
            get { return usedPartName; }
            set { usedPartName = value; }
        }
        /// <summary>
        /// 浇筑部位层次码
        /// </summary>
        virtual public string UsedPartSysCode
        {
            get { return usedPartSysCode; }
            set { usedPartSysCode = value; }
        }

        /// <summary>
        /// 结算规则
        /// </summary>
        public virtual string BalRule
        {
            get { return balRule; }
            set { balRule = value; }
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        virtual public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        virtual public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        /// <summary>
        /// 计算天数
        /// </summary>
        virtual public int Days
        {
            get { return days; }
            set { days = value; }
        }
        /// <summary>
        /// 租赁单价
        /// </summary>
        virtual public decimal RentalPrice
        {
            get { return rentalPrice; }
            set { rentalPrice = value; }
        }
        /// <summary>
        /// 进场数量
        /// </summary>
        virtual public decimal ApproachQuantity
        {
            get { return approachQuantity; }
            set { approachQuantity = value; }
        }
        /// <summary>
        /// 退场数量
        /// </summary>
        virtual public decimal ExitQuantity
        {
            get { return exitQuantity; }
            set { exitQuantity = value; }
        }
        /// <summary>
        /// 结算状态 (上期结存、上期未结、本期发生)
        /// </summary>
        virtual public string BalState
        {
            get { return balState; }
            set { balState = value; }
        }
        /// <summary>
        /// 结存数量
        /// </summary>
        virtual public decimal UnusedBalQuantity
        {
            get { return unusedBalQuantity; }
            set { unusedBalQuantity = value; }
        }
        /// <summary>
        /// 收料明细GUID
        /// </summary>
        virtual public string MatCollDtlId
        {
            get { return matCollDtlId; }
            set { matCollDtlId = value; }
        }
        /// <summary>
        /// 收料单号
        /// </summary>
        virtual public string MatCollCode
        {
            get { return matCollCode; }
            set { matCollCode = value; }
        }
        /// <summary>
        /// 收料明细数量
        /// </summary>
        virtual public decimal MatCollDtlQty
        {
            get { return matCollDtlQty; }
            set { matCollDtlQty = value; }
        }
        /// <summary>
        /// 退料单单号
        /// </summary>
        virtual public string MatReturnCode
        {
            get { return matReturnCode; }
            set { matReturnCode = value; }
        }
        /// <summary>
        /// 退料明细数量
        /// </summary>
        virtual public decimal MatReturnDtlQty
        {
            get { return matReturnDtlQty; }
            set { matReturnDtlQty = value; }
        }
    }
}
