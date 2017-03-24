 

/// <summary>
///SessionInfo 的摘要说明
/// </summary>
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System;
using System.Collections.Generic;
using VirtualMachine.Core;
using System.Collections;
using AuthManagerLib.AuthMng.MenusMng.Domain;
using NHibernate.Criterion;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
 
[Serializable]
public class SessionInfo
{
    StandardPerson oCurrentPerson;
    PersonInfo oCurrentPersinInfo;
    OperationJob   oCurrentJob;
    OperationOrg   oCurrentOrg;
    OperationOrgInfo oCurrentOrgInfo;
    CurrentProjectInfo oCurrentProjectInfo; 

   
    DateTime logTime= DateTime.Now;
    List<AuthRole> _listRoles = new List<AuthRole>();
    List<Menus> _listMenus = new List<Menus>();

 

    public SessionInfo(PersonOnJob oPersonOnJob)
    {
        try
        {
            this.oCurrentPerson = oPersonOnJob.StandardPerson;
            this.oCurrentJob = oPersonOnJob .OperationJob;
            this.oCurrentOrg = oPersonOnJob.OperationJob.OperationOrg ;

             

        }
        catch
        {
        }
    }
    public SessionInfo(string sUserCode, string sPassWord, string sJobID)
    {
        ObjectQuery oQuery = new ObjectQuery();
        oQuery.AddCriterion(Expression.Eq("StandardPerson.Code", sUserCode));
        oQuery.AddCriterion(Expression.Eq("StandardPerson.Password", sPassWord));
        oQuery.AddCriterion(Expression.Eq("OperationJob.Id", sJobID));
        //oQuery.AddFetchMode("OperationJob", FetchMode.Eager);
        //oQuery.AddFetchMode("OperationJob.OperationOrg", FetchMode.Eager);
        //oQuery.AddFetchMode("StandardPerson", FetchMode.Eager);
      IList lstPersonOnJob=  ResourceSvr.PersonOnJobManager.GetOnJobPersonList(oQuery);
      if (lstPersonOnJob != null && lstPersonOnJob.Count > 0)
      {
          PersonOnJob oPersonOnJob = lstPersonOnJob[0] as PersonOnJob;
         
          this.oCurrentPerson = oPersonOnJob.StandardPerson;
          this.oCurrentJob = oPersonOnJob.OperationJob;
          this.oCurrentOrg = oPersonOnJob.OperationJob.OperationOrg;
      }
      else
      { 

      }
    }
    public SessionInfo(StandardPerson oCurrentPerson, OperationJob oCurrentJob, OperationOrg oCurrentOrg )
    {
        this.oCurrentPerson = oCurrentPerson;
        this.oCurrentJob = oCurrentJob;
        this.oCurrentOrg = oCurrentOrg;
 
    }
 
    public StandardPerson CurrentPerson
    {
        get { return oCurrentPerson; }
    }
    public PersonInfo CurrentPersinInfo
    {
        get
        {
            if (oCurrentPersinInfo == null)
            {
                oCurrentPersinInfo = ResourceSvr.PersonManager.GetPersonInfo(oCurrentPerson.Id);
            }
            return oCurrentPersinInfo;
        }
    }
    public OperationOrg CurrentOrg
    {
        get { return oCurrentOrg; }
    }
    public OperationOrgInfo CurrentOrgInfo
    {
        get
        {
            if (oCurrentOrgInfo == null)
            {
                oCurrentOrgInfo = GlobalClass.CommonMethodSrv.GetOperationOrgInfo(CurrentOrg.Id);
            }
            return oCurrentOrgInfo;
        }
    }
    public OperationJob CurrentJob
    {
        get { return      oCurrentJob;}
    }
    public CurrentProjectInfo CurrentProjectInfo
    {
        get
        {
            if (oCurrentProjectInfo == null)
            {
                oCurrentProjectInfo = GlobalClass.CommonMethodSrv.GetProjectInfo( CurrentOrgInfo.SysCode);
            }
            return oCurrentProjectInfo;
        }
    }
    public List<AuthRole> ListRoles
    {
        get { return _listRoles; }
        set { _listRoles = value; }
    }
    public List<Menus> ListMenus
    {
        get { return _listMenus; }
        set { _listMenus = value; }
    }
    public DateTime LogTime
    {
        get { return logTime; }
    }
    public bool Validate()
    {
        bool bFlag = false;
        if (this.oCurrentJob != null && this.oCurrentOrg != null && this.oCurrentPerson != null)
        {
            bFlag = true;
        }
        else
        {
            bFlag = false  ;
        }
        return bFlag;
    }
}
