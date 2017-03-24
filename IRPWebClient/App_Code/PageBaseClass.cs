using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using System.Collections.Specialized;
using System.Reflection;

/// <summary>
///PageBaseClass 的摘要说明
/// </summary>
public class PageBaseClass : Page
{
    public PageBaseClass()
    {

    }

    protected virtual void Page_Init(object sender, EventArgs e)
    {

    }

    protected override void OnLoad(EventArgs e)
    {
        if (Page.IsPostBack == false)
        {
            if (Session["UserSession"] == null && Request["userCode"] != null && Request["groupId"] != null && Application[Request["userCode"] + "CreateSession"] != null)
            {
                string user = Request["userCode"];
                string password = "";
                string groupId = Request["groupId"];
                //单点登录时sessin丢失，生成新的session;寻找更好的解决方案中...
                CreateSession(user, password, groupId);
                Application[Request["userCode"] + "CreateSession"] = null;
            }

            if (Session["UserSession"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        base.OnLoad(e);
    }

    public void MessageBox(string sMessage)
    {
        string sMsg = string.Empty;
        if (!string.IsNullOrEmpty(sMessage))
        {
            sMessage = sMessage.Replace("\r", "").Replace("\n", "");
            sMsg = string.Format("alert('{0}');", sMessage);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "myScript", sMsg, true);
        }
    }
    public static string GetMessageStr(string sMessage)
    {
        if (!string.IsNullOrEmpty(sMessage))
        {
            sMessage = sMessage.Replace("\r", "").Replace("\n", "");
        }
        sMessage = string.Format("alert('{0}');", sMessage);

        return sMessage;
    }
    public string GetPageQueryString()
    {
        string queryStr = "";
        for (int i = 0; i < Request.QueryString.Count; i++)
        {
            if (queryStr == "")
                queryStr += "?" + Request.QueryString.Keys[i] + "=" + Request.QueryString[i];
            else
                queryStr += "&" + Request.QueryString.Keys[i] + "=" + Request.QueryString[i];
        }
        return queryStr;
    }

    /// <summary>
    /// 创建session
    /// </summary>
    /// <param name="userCode">用户code</param>
    /// <param name="pwd">用户密码</param>
    /// <param name="jobId">岗位Id</param>
    public void CreateSession(string userCode, string pwd, string jobId)
    {
        if (!string.IsNullOrEmpty(userCode) && !string.IsNullOrEmpty(jobId))
        {
            UserSession user = new UserSession();

            SysOperator loginUser = MLogin.TheLoginSrv.GetSysOperator(userCode);
            List<SysRole> listJobs = MLogin.GetOperOnRoles(loginUser.Id, DateTime.Now).OfType<SysRole>().ToList();

            var loginJob = from j in listJobs
                           where j.Id == jobId
                           select j;
            SysRole theSysRole = listJobs.ElementAt(0);


            Application.Resource.CommonClass.Domain.Login theLogin = new Application.Resource.CommonClass.Domain.Login();

            theLogin.Password = string.IsNullOrEmpty(pwd) ? loginUser.OperPassword : pwd;
            theLogin.LoginDate = DateTime.Now;
            theLogin.TheCurrency = MLogin.TheLoginSrv.GetBasicCurrencyInfo();
            theLogin.TheSysRole = theSysRole;
            theLogin.ThePerson = MLogin.TheLoginSrv.GetPerson(loginUser.Id);
            theLogin.TheBusinessOperators = MLogin.TheLoginSrv.GetAuthor(loginUser.Id);
            theLogin.GroupName = "默认帐套";//Convert.ToString(this.cbbAppContext.SelectedItem);
            theLogin.Supplier = MLogin.SupplierResSrv.GetSelf();
            theLogin.Customer = MLogin.ClientResSrv.GetSelf();
            theLogin.TheRoles = MLogin.ClientResSrv.GetRoles(theSysRole.Id);

            if (theSysRole != null && theSysRole.Id != null)
            {
                theLogin.TheOperationOrgInfo = MLogin.TheLoginSrv.GetOperationOrgInfo(theSysRole.Id);
                if (theLogin.TheOperationOrgInfo != null)
                {
                    theLogin.TheAccountOrgInfo = MLogin.ClientResSrv.GetAccountOrgInfo(theLogin.TheOperationOrgInfo.SysCode);
                }
            }

            //设置会计年月
            //if (theBusinessModule.Id != null)
            //    theLogin.TheComponentPeriod = theMLogin.GetComponentPeriod(theBusinessModule.Id, DateTime.Parse(this.txtLoginDate.Text));
            //if (_FiscalModule.Id != null)
            //    theLogin.FiscalModule = theMLogin.GetComponentPeriod(_FiscalModule.Id, DateTime.Parse(this.txtLoginDate.Text));

            //AppDomain.CurrentDomain.SetData("TheLogin", theLogin);
            //ContextConfig config = AppDomain.CurrentDomain.GetData("ContextConfig") as ContextConfig;
            //config.ExecuteInitailMethod(theLogin.TheSysRole.Id, theLogin.ThePerson.Id, theLogin.LoginDate);

            user.TheJobs = listJobs;
            user.TheLogin = theLogin;
            Session["UserSession"] = user;
        }
    }
}
