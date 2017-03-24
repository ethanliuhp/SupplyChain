using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    [Serializable]
    /// <summary>
    /// 取费模板
    /// </summary>
    public class SelFeeTemplateMaster : BaseMaster
    {
        private string name;
        private Iesi.Collections.Generic.ISet<BaseDetail> selFeeTemplateDetail = new Iesi.Collections.Generic.HashedSet<BaseDetail>();
        private Iesi.Collections.Generic.ISet<BaseDetail> selFeeTempFormula = new Iesi.Collections.Generic.HashedSet<BaseDetail>();
        /// <summary>
        /// 取费明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<BaseDetail> SelFeeTemplateDetails
        {
            get { return selFeeTemplateDetail; }
            set { selFeeTemplateDetail = value; }
        }
        /// <summary>
        /// 添加取费明细
        /// </summary>
        public virtual void AddDetail(SelFeeTemplateDtl oSelFeeTemplateDtl)
        {
            oSelFeeTemplateDtl.Master = this;
            selFeeTemplateDetail.Add(oSelFeeTemplateDtl);
        }
        /// <summary>
        /// 取费公式
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<BaseDetail> SelFeeTempFormulas
        {
            get { return selFeeTempFormula; }
            set { selFeeTempFormula = value; }
        }
        /// <summary>
        /// 添加取费公式
        /// </summary>
        public virtual void AddFormula(SelFeeTempFormula oSelFeeTempFormula)
        {
            oSelFeeTempFormula.Master = this;
            selFeeTempFormula.Add(oSelFeeTempFormula);
        }
        /// <summary>
        /// 模板名称
        /// </summary>
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
