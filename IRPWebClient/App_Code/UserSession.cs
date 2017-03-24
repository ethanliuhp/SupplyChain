using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

/// <summary>
///UserSession 的摘要说明
/// </summary>
[Serializable]
public class UserSession
{
    public UserSession()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    private Login theLogin = null;
    private List<SysRole> theJobs = null;

    public Login TheLogin
    {
        get
        {
            return theLogin;
        }
        set
        {
            theLogin = value;
        }
    }
    /// <summary>
    /// 拥有的岗位
    /// </summary>
    public List<SysRole> TheJobs
    {
        get
        {
            return theJobs;
        }
        set { theJobs = value; }
    }

}
