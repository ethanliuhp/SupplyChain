using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain
{
    /// <summary>
    /// 出库单明细基类
    /// </summary>
    [Entity]
    [Serializable]
    public abstract class BasicStockOutDtl : BaseDetail
    {
        private string professionalCategory;
        private string materialGrade;


        private decimal confirmMoney;
        private decimal confirmPrice;

        /// <summary>
        /// 确认价格
        /// </summary>
        public virtual decimal ConfirmPrice
        {
            get { return confirmPrice; }
            set { confirmPrice = value; }
        }
        /// <summary>
        /// 确认金额
        /// </summary>
        public virtual decimal ConfirmMoney
        {
            get { return confirmMoney; }
            set { confirmMoney = value; }
        }
        public virtual string MaterialGrade
        {
            get { return materialGrade; }
            set { materialGrade = value; }
        }

        /// <summary>
        /// 专业分类
        /// </summary>
        virtual public string ProfessionalCategory
        {
            get { return professionalCategory; }
            set { professionalCategory = value; }
        }

        private string concreteBalDtlId;

        /// <summary>
        /// 商品砼结算明细ID
        /// </summary>
        virtual public string ConcreteBalDtlID
        {
            get { return concreteBalDtlId; }
            set { concreteBalDtlId = value; }
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
    }
}
