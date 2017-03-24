using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.OutPutValue
{


    public class MOutPutValue
    {
        private ISpecialCostSrv specialCostSrv;

        public ISpecialCostSrv SpecialCostSrv
        {
            get { return specialCostSrv; }
            set { specialCostSrv = value; }
        }
        private IGWBSTreeSrv gwbsSrv;

        public IGWBSTreeSrv GwbsSrv
        {
            get { return gwbsSrv; }
            set { gwbsSrv = value; }
        }

        public MOutPutValue()
        {
            if (specialCostSrv == null)
            {
                specialCostSrv = StaticMethod.GetService("SpecialCostSrv") as ISpecialCostSrv;
            }
            if (gwbsSrv == null)
            {
                gwbsSrv = StaticMethod.GetService("GWBSTreeSrv") as IGWBSTreeSrv;
            }
        }
    }
}