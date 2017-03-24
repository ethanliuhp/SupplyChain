using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRPServiceModel.Services.Document;
using Spring.Context;
using Application.Resource.CommonClass.Service;
using Application.Resource.PersonAndOrganization.SupplierManagement.Service;
using Application.Resource.PersonAndOrganization.ClientManagement.Service;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using AuthManagerLib.AuthMng.AuthConfigMng.Service;
using AuthManagerLib.AuthMng.MenusMng.Service;

/// <summary>
///MLogin 的摘要说明
/// </summary>
public class MLogin
{
    private static IDocumentSrv _DocSrv;
    private static ILoginSrv _TheLoginSrv;
    private static ISupplierResSrv _SupplierResSrv;
    private static IClientResSrv _ClientResSrv;
    private static IPersonManager _PersonManager = null;

    static MLogin()
    {
       
        if (_DocSrv == null)
            //_DocSrv = (AppDomain.CurrentDomain.GetData("IRPServiceModel") as IApplicationContext).GetObject("DocumentSrv") as IDocumentSrv;
        if (_TheLoginSrv == null)
            _TheLoginSrv = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("LoginSrv") as ILoginSrv;
        if (_SupplierResSrv == null)
            _SupplierResSrv = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("SupplierResSrv") as ISupplierResSrv;
        if (_ClientResSrv == null)
            _ClientResSrv = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("ClientResSrv") as IClientResSrv;
    }
    public static IPersonManager PersonManager
    {
        get
        {
            if (_PersonManager == null)
            {
                _PersonManager = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("PersonManager") as IPersonManager;
            }
            return _PersonManager;
        }
    }

    public static IDocumentSrv DocSrv
    {
        get
        {
            if (_DocSrv == null)
                _DocSrv = (AppDomain.CurrentDomain.GetData("IRPServiceModel") as IApplicationContext).GetObject("DocumentSrv") as IDocumentSrv;
            return MLogin._DocSrv;
        }
        set { MLogin._DocSrv = value; }
    }

    public static ILoginSrv TheLoginSrv
    {
        get
        {
            if (_TheLoginSrv == null)
                _TheLoginSrv = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("LoginSrv") as ILoginSrv;
            return MLogin._TheLoginSrv;
        }
        set { MLogin._TheLoginSrv = value; }
    }

    public static ISupplierResSrv SupplierResSrv
    {
        get
        {
            if (_SupplierResSrv == null)
                _SupplierResSrv = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("SupplierResSrv") as ISupplierResSrv;
            return MLogin._SupplierResSrv;
        }
        set { MLogin._SupplierResSrv = value; }
    }


    public static IClientResSrv ClientResSrv
    {
        get
        {
            if (_ClientResSrv == null)
                _ClientResSrv = (AppDomain.CurrentDomain.GetData("ResourceManager") as IApplicationContext).GetObject("ClientResSrv") as IClientResSrv;
            return MLogin._ClientResSrv;
        }
        set { MLogin._ClientResSrv = value; }
    }

    public static IList GetOperOnRoles(string operId, DateTime loginDate)
    {
        IList listSysRole = _TheLoginSrv.GetOperOnRoles(operId, loginDate);

        if (listSysRole != null && listSysRole.Count > 0)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            Dictionary<string, string> dicOrg = new Dictionary<string, string>();//存储岗位路径上所有的组织ID和对应的名称
            foreach (SysRole group in listSysRole)
            {
                if (!string.IsNullOrEmpty(group.SysCode) && group.SysCode.Length > 1)
                {
                    //取机构ID
                    string orgId = group.SysCode.Substring(0, group.SysCode.Length - 1);
                    if (orgId.IndexOf(".") > -1)
                    {
                        orgId = orgId.Substring(0, orgId.LastIndexOf("."));//移除岗位ID
                    }
                    string[] listOrgId = orgId.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    if (listOrgId.Length > 1)
                    {
                        for (int i = 1; i < listOrgId.Length; i++)//移除根组织ID
                        {
                            if (dicOrg.ContainsKey(listOrgId[i]) == false)
                            {
                                dicOrg.Add(listOrgId[i], "");
                            }
                        }
                    }
                }
            }
            foreach (string orgId in dicOrg.Keys)
            {
                dis.Add(Expression.Eq("Id", orgId));
            }

            oq.AddCriterion(dis);
            IList listOrg = MLogin.DocSrv.ObjectQuery(typeof(OperationOrg), oq);
            if (listOrg != null && listOrg.Count > 0)
            {
                //设置组织名称
                foreach (OperationOrg org in listOrg)
                {
                    foreach (string orgId in dicOrg.Keys)
                    {
                        if (orgId == org.Id)
                        {
                            dicOrg[orgId] = org.Name;
                            break;
                        }
                    }
                }

                foreach (SysRole group in listSysRole)
                {
                    if (string.IsNullOrEmpty(group.SysCode) || group.SysCode.Length <= 1)
                        continue;

                    string orgId = group.SysCode.Substring(0, group.SysCode.Length - 1);
                    if (orgId.IndexOf(".") > -1)
                    {
                        orgId = orgId.Substring(0, orgId.LastIndexOf(".") + 1);//移除岗位ID
                        orgId = orgId.Substring(orgId.IndexOf(".") + 1);//移除根组织ID
                    }

                    string orgFullName = orgId;
                    foreach (string orgIdKey in dicOrg.Keys)
                    {
                        orgFullName = orgFullName.Replace(orgIdKey + ".", dicOrg[orgIdKey] + "_");
                    }

                    group.RoleName = orgFullName + group.RoleName;
                }
            }
        }

        return listSysRole;
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

}


