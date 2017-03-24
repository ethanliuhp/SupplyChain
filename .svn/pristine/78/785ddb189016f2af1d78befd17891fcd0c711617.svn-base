using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorDefine
{
    public class MIndicatorDefine
    {
        private IIndicatorDefineService indicatorDefSrv;

        public IIndicatorDefineService IndicatorDefSrv
        {
            get { return indicatorDefSrv; }
            set { indicatorDefSrv = value; }
        }

        public MIndicatorDefine()
        {
            if (indicatorDefSrv == null)
            {
                indicatorDefSrv = ConstMethod.GetService(typeof(IIndicatorDefineService)) as IIndicatorDefineService;
            }
        }
    }
}
