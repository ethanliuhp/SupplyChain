using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Iesi.Collections.Generic;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    [Serializable]
    [Entity]
    public class CommercialReportMaster : BaseMaster
    {
        private ISet<BearerIndicatorDtl> biDtl = new HashedSet<BearerIndicatorDtl>();
        private ISet<BearerIndicatorDtl> blwDtl = new HashedSet<BearerIndicatorDtl>();//劳务分包


        private ISet<BureauCostDtl> bcDtl = new HashedSet<BureauCostDtl>();
        private ISet<CostCheckIndicatorDtl> cciDtl = new HashedSet<CostCheckIndicatorDtl>();
        private ISet<DisputeTrackDtl> dtDtl = new HashedSet<DisputeTrackDtl>();
        private ISet<SettlementProgressReportDtl> sprDtl = new HashedSet<SettlementProgressReportDtl>();
        private ISet<SubcontractAuditDtl> saDtl = new HashedSet<SubcontractAuditDtl>();

        private ISet<VisaClamisDtl> vcDtl = new HashedSet<VisaClamisDtl>();


       virtual public ISet<BearerIndicatorDtl> BiDtl
        {
            get { return biDtl; }
            set { biDtl = value; }
        }

       virtual public ISet<BearerIndicatorDtl> BlwDtl
       {
           get { return blwDtl; }
           set { blwDtl = value; }
       }







       virtual public ISet<BureauCostDtl> BcDtl
        {
            get { return bcDtl; }
            set { bcDtl = value; }
        }

       virtual public ISet<CostCheckIndicatorDtl> CciDtl
        {
            get { return cciDtl; }
            set { cciDtl = value; }
        }

       virtual public ISet<DisputeTrackDtl> DtDtl
        {
            get { return dtDtl; }
            set { dtDtl = value; }
        }

       virtual public ISet<SettlementProgressReportDtl> SprDtl
        {
            get { return sprDtl; }
            set { sprDtl = value; }
        }

       virtual public ISet<SubcontractAuditDtl> SaDtl
        {
            get { return saDtl; }
            set { saDtl = value; }
        }

       virtual public ISet<VisaClamisDtl> VcDtl
       {
           get { return vcDtl; }
           set { vcDtl = value; }
       }
      //private decimal _CurrentHourlyTotal = 0;//虽然是随机汇总  
       
    }
}
