using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Domain
{
    /// <summary>
    /// 临建摊销单主表
    /// </summary>
    [Serializable]
    public class OverlayAmortizeMaster : BaseMaster
    {
        private Double sumMoney;

        ///<summary>
        ///总金额
        ///</summary>
        
        virtual public Double SumMoney
        {
            get
            {
                Double temMoney = 0;
                //汇总
                foreach (OverlayAmortizeDetail var in Details)
                {
                    temMoney += var.OverlayValue;
                }
                sumMoney = temMoney;
                return sumMoney;
            }
            set
            {
                sumMoney = value;
            }
        }
    }
}
