using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Spring.Context;
using IRPServiceModel.Services.PaymentOrder;
using IRPServiceModel.Services.Common;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.MaterialResource.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using IRPServiceModel.Services.CompanyMng;
using AuthManagerLib.AuthMng.AuthConfigMng.Service;
using AuthManagerLib.AuthMng.MenusMng.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;

/// <summary>
/// Summary description for GlobalClass
/// </summary>
public class GlobalClass
{
    public GlobalClass()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    
  
 
    private  static ICommonMethodSrv commonMethodSrv;
    /// <summary>
    /// 公共服务
    /// </summary>
    public static ICommonMethodSrv CommonMethodSrv
    {
        get
        {
            if (commonMethodSrv == null)
            {
                commonMethodSrv = (AppDomain.CurrentDomain.GetData("IRPServiceModel") as IApplicationContext).GetObject("CommonMethodSrv") as ICommonMethodSrv;
            }
            return commonMethodSrv;
        }
    }

    private static IPaymentOrderSvr paymentOrderSvr;
    /// <summary>
    /// 获取付款单服务
    /// </summary>
    public static IPaymentOrderSvr PaymentOrderSvr
    {
        get
        {
            if (paymentOrderSvr == null)
            {
                paymentOrderSvr = (AppDomain.CurrentDomain.GetData("IRPServiceModel") as IApplicationContext).GetObject("PaymentOrderSvr") as IPaymentOrderSvr;
            }
            return paymentOrderSvr;
        }
    }
    private static IIndirectCostSvr indirectCostSvr;
    /// <summary>
    /// 获取公司和分公司财务账面维护服务
    /// </summary>
    public static IIndirectCostSvr IndirectCostSvr
    {
        get
        {
            if (indirectCostSvr == null)
            {
                indirectCostSvr = (AppDomain.CurrentDomain.GetData("SupplyChain") as IApplicationContext).GetObject("IndirectCostSvr") as IIndirectCostSvr;
            }
            return indirectCostSvr;
        }
    }
 private static ICompanyInfoSvr companyInfoSvr;
 public static ICompanyInfoSvr CompanyInfoSvr
 {
     get
     {
         if (companyInfoSvr == null)
         {
             companyInfoSvr = (AppDomain.CurrentDomain.GetData("IRPServiceModel") as IApplicationContext).GetObject("CompanyInfoSvr") as ICompanyInfoSvr;
         }
         return companyInfoSvr;
     }
      
 }




 private static IFinanceMultDataSrv financeMultDataSrv;
 /// <summary>
 /// 获取财务服务
 /// </summary>
 public static IFinanceMultDataSrv FinanceMultDataSrv
 {
     get
     {
         if (financeMultDataSrv == null)
         {
             var a = AppDomain.CurrentDomain.GetData("SupplyChain") as IApplicationContext;
             financeMultDataSrv = a.GetObject("FinanceMultDataSrv") as IFinanceMultDataSrv;
         }
         return financeMultDataSrv;
     }
 }

 private static IAccountTitleTreeSvr accountTitleTree;
    /// <summary>
    /// 会计科目服务
    /// </summary>
 public static IAccountTitleTreeSvr AccountTitleTreeSvr
 {
     get
     {
         if (accountTitleTree == null)
         {
             var a = AppDomain.CurrentDomain.GetData("SupplyChain") as IApplicationContext;
             accountTitleTree = a.GetObject("AccountTitleTreeSvr") as IAccountTitleTreeSvr;
         }
         return accountTitleTree;
     }
 }

}
public class ResourceSvr
{
    private static IOperationOrgService operationOrgService;
    public static IOperationOrgService OperationOrgService
    {
        get
        {
            if (operationOrgService == null)
            {
                operationOrgService = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("OperationOrgService") as IOperationOrgService;
            }
            return operationOrgService;
        }
    }
    private static IOrganizationResSrv organizationResSrv;
    public static IOrganizationResSrv OrganizationResSrv
    {
        get
        {
            if (organizationResSrv == null)
            {
                organizationResSrv = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("OrganizationResSrv") as IOrganizationResSrv;
            }
            return organizationResSrv;
        }
    }
    private static IMaterialCategoryService materialCategoryService;
    public static IMaterialCategoryService MaterialCategoryService
    {
        get
        {
            if (materialCategoryService == null)
            {
                materialCategoryService = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("MaterialCategoryService") as IMaterialCategoryService;
            }
            return materialCategoryService;
        }
    }

    private static IMaterialService materialService;
    public static IMaterialService MaterialService
    {
        get
        {
            if (materialService == null)
            {
                materialService = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("MaterialService") as IMaterialService;
            }
            return materialService;
        }

    }
    private static IPersonManager oPersonManager = null;
    public static IPersonManager PersonManager
    {
        get
        {
            if (oPersonManager == null)
            {
                oPersonManager = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("PersonManager") as IPersonManager;
            }
            return oPersonManager;
        }
    }
    private static IOperationJobManager operationJobManager;
    public static IOperationJobManager OperationJobManager
    {
        get
        {
            if (operationJobManager == null)
            {
                operationJobManager = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("OperationJobManager") as IOperationJobManager;
            }
            return operationJobManager;
        }
        set { operationJobManager = value; }
    }
    private static IPersonOnJobManager personOnJobManager;
    public static IPersonOnJobManager PersonOnJobManager
    {
        get
        {
            if (personOnJobManager == null)
            {
                personOnJobManager = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("PersonOnJobManager") as IPersonOnJobManager;

            }
            return personOnJobManager;
        }
        set { personOnJobManager = value; }
    }
    private static IStandardUnitManager standardUnitManager;
    public static IStandardUnitManager StandardUnitManager
    {
        get
        {
            if (standardUnitManager == null)
            {
                standardUnitManager = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("StandardUnitManager") as IStandardUnitManager;
            }
            return standardUnitManager;
        }
        set { standardUnitManager = value; }
    }
    private static IOrganizationService organizationService;
    public static IOrganizationService OrganizationService
    {
        get
        {
            if (organizationService == null)
            {
                organizationService = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("OrganizationService") as IOrganizationService;
            }
            return organizationService;
        }
    }
}
public class AuthSvr
{
    private static IAuthConfigSrv oAuthConfigSrv = null;
    public static IAuthConfigSrv AuthConfigSrv
    {
        get
        {
            if (oAuthConfigSrv == null)
            {
                oAuthConfigSrv = (AppDomain.CurrentDomain.GetData("AuthManagerLib") as IApplicationContext).GetObject("AuthConfigSrv") as IAuthConfigSrv;
            }
            return oAuthConfigSrv;
        }
    }
    private static IMenusSrv oMenusSrv;
    public static IMenusSrv MenusSrv
    {
        get
        {
            if (oMenusSrv == null)
            {
                oMenusSrv = (AppDomain.CurrentDomain.GetData("AuthManagerLib") as IApplicationContext).GetObject("MenusSrv") as IMenusSrv;
            }

            return oMenusSrv;
        }
    }


}

