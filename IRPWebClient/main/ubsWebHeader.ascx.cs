using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

public partial class Main_ubsWebHeader : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[PublicClass.sUserInfoID] != null)
        {
            pnlUserInfo.Visible = true;
            lblLoginOrExit.InnerText = "退出登录";

            SessionInfo user = Session[PublicClass.sUserInfoID] as SessionInfo;
            if (user != null && user.CurrentPerson != null)
                lblUserInfo.InnerText = "欢迎您," + user.CurrentPerson.Name + "【" + user.CurrentOrg.Name + "-" + user.CurrentJob.Name + "】" + "!";
        }
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
          SessionInfo user = Session[PublicClass.sUserInfoID] as SessionInfo;
       // PersonInfo personInfo = SBasicMng.GetPersonInfoById(PublicClass.GetUseInfo(Session).CurrentPerson.Id);
       // PublicClass.WriteLog(personInfo.Id, "退出", personInfo.Name, "退出", Session, "退出");

          string sPath = string.Format("~/Login/SSOLogin.aspx?UserCode={0}&PassWord={1}", user.CurrentPerson.Code, user.CurrentPerson.Password);
        Session[PublicClass.sUserInfoID] = null;
        FormsAuthentication.SignOut();
        Response.Redirect(sPath);
    }
    protected void btnExitHide_Click(object sender, EventArgs e)
    {
        //SessionInfo user = Session[PublicClass.sUserInfoID] as SessionInfo;
        // personInfo = SBasicMng.GetPersonInfoById(PublicClass.GetUseInfo(Session).CurrentPerson.Id);
        //PublicClass.WriteLog(personInfo.Id, "退出", personInfo.Name, "退出", Session, "退出");

    }
}
