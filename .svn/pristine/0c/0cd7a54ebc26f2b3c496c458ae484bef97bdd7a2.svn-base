using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Basic.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Service;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public class MFinanceMultData
    {
        private static IIndirectCostSvr indirectCostSvr;
        private static IFinanceMultDataSrv financeMultDataSrv;
        private static IOrganizationResSrv organizationResSrv;
        private IPersonManager oPersonManager = null;
        private ICurrentProjectSrv currentProjectSrv;

        public IFinanceMultDataSrv FinanceMultDataSrv
        {
            get
            {
                if (financeMultDataSrv == null)
                {
                    financeMultDataSrv = StaticMethod.GetService("FinanceMultDataSrv") as IFinanceMultDataSrv;
                }
                return financeMultDataSrv;
            }
        }

        public IIndirectCostSvr IndirectCostSvr
        {
            get
            {
                if (indirectCostSvr == null)
                {
                    indirectCostSvr = StaticMethod.GetService("IndirectCostSvr") as IIndirectCostSvr;
                }
                return indirectCostSvr;
            }
        }

        public IOrganizationResSrv OrganizationResSrv
        {
            get
            {
                if (organizationResSrv == null)
                {
                    organizationResSrv = StaticMethod.GetService("OrganizationResSrv") as IOrganizationResSrv;
                }
                return organizationResSrv;
            }
        }

        public IPersonManager PersonManager
        {
            get
            {
                if (oPersonManager == null)
                {
                    oPersonManager = StaticMethod.GetService("PersonManager") as IPersonManager;
                }
                return oPersonManager;
            }
        }

        public ICurrentProjectSrv CurrentProjectSrv
        {
            get
            {
                if (currentProjectSrv == null)
                {
                    currentProjectSrv = StaticMethod.GetService("CurrentProjectSrv") as ICurrentProjectSrv;
                }
                return currentProjectSrv;
            }
        }
    }
}
