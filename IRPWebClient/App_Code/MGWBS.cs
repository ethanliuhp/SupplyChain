using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRPServiceModel.Services.Document;
using Spring.Context;
using Application.Resource.CommonClass.Service;
using Application.Resource.PersonAndOrganization.SupplierManagement.Service;
using Application.Resource.PersonAndOrganization.ClientManagement.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;

/// <summary>
///MLogin 的摘要说明
/// </summary>
public class MGWBS
{
    private static IGWBSTreeSrv _GWBSSrv;
    private static IOperationOrgService _OperationOrgSrv;

    static MGWBS()
    {
        if (_GWBSSrv == null)
            _GWBSSrv = (AppDomain.CurrentDomain.GetData("SupplyChain") as IApplicationContext).GetObject("GWBSTreeSrv") as IGWBSTreeSrv;
        if (_OperationOrgSrv == null)
            _OperationOrgSrv = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("OperationOrgService") as IOperationOrgService;
    }

    public static IGWBSTreeSrv GWBSSrv
    {
        get
        {
            if (_GWBSSrv == null)
                _GWBSSrv = (AppDomain.CurrentDomain.GetData("SupplyChain") as IApplicationContext).GetObject("GWBSTreeSrv") as IGWBSTreeSrv;

            return MGWBS._GWBSSrv;
        }
        set { MGWBS._GWBSSrv = value; }
    }
    public static IOperationOrgService OperationOrgSrv
    {
        get
        {
            if (_OperationOrgSrv == null)
                _OperationOrgSrv = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("OperationOrgService") as IOperationOrgService;

            return MGWBS._OperationOrgSrv;
        }
        set { MGWBS._OperationOrgSrv = value; }
    }
}
