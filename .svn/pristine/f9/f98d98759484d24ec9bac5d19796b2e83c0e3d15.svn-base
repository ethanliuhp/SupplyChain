using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using VirtualMachine.Core;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
 
using System.Collections.Generic;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using AuthManagerLib.AuthMng.MenusMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using NHibernate.Criterion;
 

public partial class Login_SSOLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        divBottom.InnerText = "";
        if (!IsPostBack)
        {
            txtUserName.Text = Request.QueryString["UserCode"];
            txtPwd.Text = Request.QueryString["PassWord"];
            
        }
        if (IsSingleLogin)
        {
            btnComeIn.Visible = false;
            DataBind(this.UserCode, this.PassWord);
        }
        
    }
    private void DataBind(string sUserCode, string sPassWord)
    {
         
        Check(sUserCode, sPassWord, true);
    }
    private bool Check(string sUserCode, string sPassWord, bool bUpdateJob)
    {
        bool bResult = false;
        IList lstPersonOnJob = GlobalClass.CommonMethodSrv.GetPersonOnJob(sUserCode, sPassWord);
        if (lstPersonOnJob != null && lstPersonOnJob.Count > 0)
        {
            btnComeIn.Visible = true;
            if (bUpdateJob)
            {
                dpLst.Items.Clear();
                foreach (PersonOnJob oPersonOnJob in lstPersonOnJob)
                {
                    if (oPersonOnJob.OperationJob != null)
                    {
                        dpLst.Items.Add(new ListItem(oPersonOnJob.OperationJob.Name, oPersonOnJob.OperationJob.Id));
                    }
                }
            }
        }
        else
        {
            ShowMessage("该用户为非法用户");
        }
        return bResult;
    }
    private string UserCode
    {
        get { return txtUserName.Text; }
    }
    private string PassWord
    {
        get { return txtPwd.Text; }
    }
    private void ShowMessage(string sMsg)
    {
     
        divBottom.InnerText = sMsg;
       // Page.ClientScript.RegisterClientScriptBlock(this.GetType(),this.GetType().ToString(),string.Format(" showMessage('{0}')",sMsg),true);
    }
     
    public bool IsSingleLogin 
    {
        get {

            return !string.IsNullOrEmpty(Request.QueryString["UserCode"]);
        }
    }
    protected void btnComeIn_Click(object sender, EventArgs e)
    {
        try
        {
            if (dpLst.SelectedIndex > -1 && dpLst.SelectedIndex < dpLst.Items.Count)
            {
                IList lstPersonOnJob = GlobalClass.CommonMethodSrv.GetPersonOnJob(this.UserCode, this.PassWord,dpLst.SelectedValue);
                if (lstPersonOnJob != null && lstPersonOnJob.Count > 0)
                {
                    PersonOnJob oPersonOnJob = lstPersonOnJob[0] as PersonOnJob;
                    SessionInfo oSessionInfo = new SessionInfo(oPersonOnJob);
                    if (oSessionInfo.Validate())
                    {
                        IList lstAuthConfigs = null;
                        if (oSessionInfo.CurrentPerson != null && string.Equals(oSessionInfo.CurrentPerson.Name, PublicClass.GetAdminName()))
                        {//管理员
                            lstAuthConfigs = GlobalClass.CommonMethodSrv.GetAuthConfigByOperationJobID("",PublicClass.GetMenuRootCode());
                            foreach (AuthConfig oAuthConfig in lstAuthConfigs)
                            {
                               
                                if (!oSessionInfo.ListRoles.Contains(oAuthConfig.Roles))
                                {
                                    oSessionInfo.ListRoles.Add(oAuthConfig.Roles);
                                }
                            }
                            oSessionInfo.ListMenus = GlobalClass.CommonMethodSrv.GetAdminMenus(PublicClass.GetMenuRootCode()).OfType<Menus>().ToList();
                        }
                        else
                        {
                            lstAuthConfigs = GlobalClass.CommonMethodSrv.GetAuthConfigByOperationJobID(oPersonOnJob.OperationJob.Id, PublicClass.GetMenuRootCode());
                            foreach (AuthConfig oAuthConfig in lstAuthConfigs)
                            {
                                if (!oSessionInfo.ListMenus.Contains(oAuthConfig.Menus))
                                {
                                    oSessionInfo.ListMenus.Add(oAuthConfig.Menus);
                                }
                                if (!oSessionInfo.ListRoles.Contains(oAuthConfig.Roles))
                                {
                                    oSessionInfo.ListRoles.Add(oAuthConfig.Roles);
                                }
                            }
                        }
                        

                        PublicClass.SaveUseInfo(oSessionInfo, Session);
                        FormsAuthentication.SetAuthCookie(oPersonOnJob.StandardPerson.Code, false);
                        Response.Redirect("~/Main/main.aspx");
                    }
                    else
                    {
                        throw new Exception("登录失败");
                    }
                }
                else
                {
                    throw new Exception("登录失败:请输入正确用户名和密码");
                }
            }
            else
            {
                throw new Exception("请选择对应的岗位");
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }
    public List<Menus> GetMenus(SessionInfo oSessionInfo)
    {
        List<Menus> lstMenu = null;
        if (oSessionInfo.CurrentPerson != null && string.Equals(oSessionInfo.CurrentPerson.Name, PublicClass.GetAdminName()))
        {
            ObjectQuery oQuery = new ObjectQuery();
            //oQuery.AddCriterion(Expression.Sql(" (ExeContent is not null or ExeContent<>'' )"));
            lstMenu = AuthSvr.AuthConfigSrv.GetMenus(oQuery).OfType<Menus>().ToList();
        }
        else
        {
            IList listAuthConfig = AuthSvr.AuthConfigSrv.GetAuthConfig(oSessionInfo.CurrentJob.Id);
            lstMenu = GetMenusFromAuthConfig(listAuthConfig);
        }
        return lstMenu;
    }
    private List<Menus> GetMenusFromAuthConfig(IList authConfigLst)
    {
 
        List<Menus> lst = new List<Menus>();
        if (authConfigLst == null || authConfigLst.Count == 0) return lst;
        foreach (AuthConfig authConfig in authConfigLst)
        {
            Menus menu = authConfig.Menus;
           
            if (!lst.Contains(menu))
            {
                lst.Add(menu);
            }
        }
        //排序
        IEnumerable<Menus> MenusSort =
        from tmpMenus in lst
        orderby tmpMenus.Level, tmpMenus.Serial
        select tmpMenus;
        lst = MenusSort.ToList<Menus>();
        return lst;
    }



 
    protected void btnGetJob_Click(object sender, EventArgs e)
    {
        bool bResult = false;
        IList lstPersonOnJob = GlobalClass.CommonMethodSrv.GetPersonOnJob(this.UserCode);
        if (lstPersonOnJob != null && lstPersonOnJob.Count > 0)
        {
            btnComeIn.Visible = true;

            dpLst.Items.Clear();
            foreach (PersonOnJob oPersonOnJob in lstPersonOnJob)
            {
                if (oPersonOnJob.OperationJob != null)
                {
                    dpLst.Items.Add(new ListItem(oPersonOnJob.OperationJob.Name, oPersonOnJob.OperationJob.Id));
                }
            }
            
        }
        else
        {
            ShowMessage("该用户为非法用户");
        }
    }
    protected void txtUserName_TextChanged(object sender, EventArgs e)
    {
        bool bResult = false;
        IList lstPersonOnJob = GlobalClass.CommonMethodSrv.GetPersonOnJob(this.UserCode);
        if (lstPersonOnJob != null && lstPersonOnJob.Count > 0)
        {
            btnComeIn.Visible = true;

            dpLst.Items.Clear();
            foreach (PersonOnJob oPersonOnJob in lstPersonOnJob)
            {
                if (oPersonOnJob.OperationJob != null)
                {
                    dpLst.Items.Add(new ListItem(oPersonOnJob.OperationJob.Name, oPersonOnJob.OperationJob.Id));
                }
            }

        }
        else
        {
            ShowMessage("该用户为非法用户");
            dpLst.Items.Clear();
            btnComeIn.Visible = false;
        }
    }
    protected void txtPwd_TextChanged(object sender, EventArgs e)
    {

    }
}
