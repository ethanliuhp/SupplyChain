using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockIn.BasicDomain
{
    /// <summary>
    /// 入库单明细基类
    /// </summary>
    [Serializable]
    public abstract class MatHireBasicStockInDtl : BaseDetail
    {
        private decimal confirmPrice;
        private decimal confirmMoney;
        private string professionalCategory;
        private string materialGrade;
        private string concreteBalDtlId;


        /// <summary>
        /// 成色，物资档次
        /// </summary>
        public virtual string MaterialGrade
        {
            get { return materialGrade; }
            set { materialGrade = value; }
        }

        /// <summary>
        /// 专业分类
        /// </summary>
        public virtual string ProfessionalCategory
        {
            get { return professionalCategory; }
            set { professionalCategory = value; }
        }

        /// <summary>
        /// 认价金额
        /// </summary>
        public virtual decimal ConfirmMoney
        {
            get { return confirmMoney; }
            set { confirmMoney = value; }
        }

        /// <summary>
        /// 认价单价
        /// </summary>
        public virtual decimal ConfirmPrice
        {
            get { return confirmPrice; }
            set { confirmPrice = value; }
        }

        /// <summary>
        /// 商品砼结算明细ID
        /// </summary>
        virtual public string ConcreteBalDtlID
        {
            get { return concreteBalDtlId; }
            set { concreteBalDtlId = value; }
        }
    }
}
