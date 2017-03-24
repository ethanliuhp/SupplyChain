using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service
{
    internal interface IIndex
    {
        DateTime Date { get; set; }
        Dictionary<string, OperationOrgInfo> OrgList { get; }
        Dictionary<string, CurrentProjectInfo> ProjectList { get; }
        void Create();
        void Create(string index);
    }
}
