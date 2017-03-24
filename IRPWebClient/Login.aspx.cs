using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirtualMachine.Core;
using NHibernate.Criterion;
using IRPServiceModel.Domain.Document;
using System.Collections;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.Service;
using Application.Resource.PersonAndOrganization.ClientManagement.Service;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.ContextConfigure;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

using System.Net;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;

public partial class Login : Page
{
    private static SysOperator theSysOperator = null;
    private static List<SysRole> listJobs = new List<SysRole>();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            string appName = Request.ApplicationPath;
            string PORTALHOST = System.Configuration.ConfigurationManager.AppSettings["PORTALHOST"];
            string SSOHOST = System.Configuration.ConfigurationManager.AppSettings["SSOHOST"];

            //门户菜单集成，点击门户菜单时请求两次，第一次传过来菜单，第二次传过来票证
            if (!Page.IsPostBack && Request.RawUrl != null && Request.RawUrl.Equals(appName + "/Login.aspx", StringComparison.OrdinalIgnoreCase) == false) // && Request.RawUrl.IndexOf("ticket") > -1  //&& Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.IndexOf(PORTALHOST) > -1
            {
                #region 单点登录集成CS

                tbMain.Visible = false;

                //获取票证作为键存储菜单名称
                string ticketKey = "ticket";
                string ticket = string.Empty;
                string PortalMenuKey = "PortalMenuName".ToLower();
                string PortalMenuName = string.Empty;
                string proInfoAuthKey = "proInfoAuth".ToLower();
                string proInfoAuth = string.Empty;

                string rawURL = Request.RawUrl;

                Dictionary<string, string> dicQueryStr = new Dictionary<string, string>();
                string queryStr = "";
                if (rawURL.IndexOf("?") > -1)
                {
                    queryStr = rawURL.Substring(rawURL.IndexOf("?") + 1);
                    string[] str = queryStr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in str)
                    {
                        string[] KeyValue = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        dicQueryStr.Add(KeyValue[0].ToLower(), KeyValue[1]);
                    }
                }

                if (dicQueryStr.Count > 0)
                {
                    if (dicQueryStr.ContainsKey(ticketKey))
                        ticket = dicQueryStr[ticketKey];
                    if (dicQueryStr.ContainsKey(PortalMenuKey))
                        PortalMenuName = dicQueryStr[PortalMenuKey];
                    if (dicQueryStr.ContainsKey(proInfoAuthKey))
                    {
                        proInfoAuth = dicQueryStr[proInfoAuthKey];
                        if (!string.IsNullOrEmpty(PortalMenuName))
                            Application[PortalMenuName + "proInfoAuth"] = proInfoAuth;
                    }
                }


                #region 票证校验和获取用户信息

                string url = string.Empty;//选择岗位页面URL
                try
                {
                    System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();

                    string service = Request.Url.GetLeftPart(UriPartial.Path);
                    service += "?" + PortalMenuKey + "=" + PortalMenuName;

                    if (ticket == null || ticket.Length == 0)
                    {
                        string redir = PORTALHOST + "cas.login?" +
                          "service=" + service;
                        Response.Redirect(redir);
                        return;
                    }

                    string validateurl = SSOHOST + "serviceValidate?" +
                      "ticket=" + ticket + "&" +
                      "service=" + service;
                    StreamReader Reader = new StreamReader(new WebClient().OpenRead(validateurl));
                    string resp = Reader.ReadToEnd();

                    NameTable nt = new NameTable();
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
                    XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
                    XmlTextReader reader = new XmlTextReader(resp, XmlNodeType.Element, context);

                    string netid = null;

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            string tag = reader.LocalName;
                            if (tag == "user")
                                netid = reader.ReadString();
                        }
                    }

                    reader.Close();

                    if (netid == null)
                    {
                        Session["LoginError"] = "身份验证失败，未获取到登录用户信息！";
                        Server.Transfer("~/LoginError.aspx");
                    }
                    else
                    {
                        //访问的菜单名不能为空
                        if (string.IsNullOrEmpty(PortalMenuName))
                        {
                            Session["LoginError"] = "请在门户URL参数中指定一个要访问的菜单！";
                            Server.Transfer("~/LoginError.aspx");
                            return;
                        }

                        SysOperator loginUser = MLogin.TheLoginSrv.GetSysOperator(netid);
                        if (loginUser == null)
                        {
                            Session["LoginError"] = "在项目管理中未获取到用户“" + netid + "”信息！";
                            Server.Transfer("~/LoginError.aspx");
                            return;
                        }

                        List<SysRole> userJobs = MLogin.GetOperOnRoles(loginUser.Id, DateTime.Now).OfType<SysRole>().ToList();
                        if (userJobs == null || userJobs.Count == 0)
                        {
                            Session["LoginError"] = "用户“" + netid + "”未配置岗位信息！";
                            Server.Transfer("~/LoginError.aspx");
                            return;
                        }

                        Application[ticket + "UserName"] = netid;
                        Application[ticket + "URLParam"] = queryStr;
                        Application[ticket + "proInfoAuth"] = Application[PortalMenuName + "proInfoAuth"];
                        txtSessionKeyHidden.Value = ticket;//回传后Request.UrlReferrer.AbsoluteUri的ticket丢失，在点击确定按钮时需要用到该键

                        listGroupJob.Items.Clear();

                        foreach (SysRole group in userJobs)
                        {
                            ListItem li = new ListItem();
                            li.Text = group.RoleName;
                            li.Value = group.Id;

                            listGroupJob.Items.Add(li);
                        }

                        if (listGroupJob.Items.Count > 0)
                            listGroupJob.SelectedIndex = 0;

                        listGroupJob.Height = (Unit)(listGroupJob.Items.Count * 20);
                    }
                }
                catch (Exception ex)
                {
                    Session["LoginError"] = VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(ex);
                    Server.Transfer("~/LoginError.aspx");
                }

                #endregion

                if (listGroupJob.Items.Count == 1)//如果只有一个岗位就直接登录，否则提供选择
                    Page.ClientScript.RegisterStartupScript(this.GetType(), ClientID, "document.getElementById('btnEnter').click();", true);
                else
                    Page.ClientScript.RegisterStartupScript(this.GetType(), ClientID, "$(function() {popForm();});", true);

                #endregion
            }
            else
            {
                #region 单点登录集成BS

                //门户菜单集成，点击门户菜单时请求两次，第一次传过来菜单，第二次传过来票证
                if (!Page.IsPostBack && Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.IndexOf(PORTALHOST) > -1)
                {
                    tbMain.Visible = false;

                    //获取票证作为键存储菜单名称
                    string ticketKey = "ticket";
                    string ticket = string.Empty;
                    string PortalMenuKey = "PortalMenuName".ToLower();
                    string PortalMenuName = string.Empty;
                    string proInfoAuthKey = "proInfoAuth".ToLower();
                    string proInfoAuth = string.Empty;

                    string rawURL = Request.RawUrl;

                    Dictionary<string, string> dicQueryStr = new Dictionary<string, string>();
                    string queryStr = "";
                    if (rawURL.IndexOf("?") > -1)
                    {
                        queryStr = rawURL.Substring(rawURL.IndexOf("?") + 1);
                        string[] str = queryStr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in str)
                        {
                            string[] KeyValue = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            dicQueryStr.Add(KeyValue[0].ToLower(), KeyValue[1]);
                        }
                    }

                    if (dicQueryStr.Count > 0)
                    {
                        if (dicQueryStr.ContainsKey(ticketKey))
                            ticket = dicQueryStr[ticketKey];
                        if (dicQueryStr.ContainsKey(PortalMenuKey))
                            PortalMenuName = dicQueryStr[PortalMenuKey];
                        if (dicQueryStr.ContainsKey(proInfoAuthKey))
                            proInfoAuth = dicQueryStr[proInfoAuthKey];
                    }

                    if (string.IsNullOrEmpty(ticket))
                    {
                        //string sessionKey = string.Empty;//门户地址里面的票证，作为唯一键

                        string refURL = Request.UrlReferrer.AbsoluteUri;

                        queryStr = "";
                        dicQueryStr.Clear();
                        if (refURL.IndexOf("?") > -1)
                        {
                            queryStr = refURL.Substring(refURL.IndexOf("?") + 1);
                            string[] str = queryStr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string s in str)
                            {
                                string[] KeyValue = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                                dicQueryStr.Add(KeyValue[0].ToLower(), KeyValue[1]);
                            }
                        }

                        if (dicQueryStr.Count > 0)
                        {
                            if (dicQueryStr.ContainsKey(ticketKey))
                                ticket = dicQueryStr[ticketKey];
                            if (dicQueryStr.ContainsKey(PortalMenuKey))
                                PortalMenuName = dicQueryStr[PortalMenuKey];
                            if (dicQueryStr.ContainsKey(proInfoAuthKey))
                                proInfoAuth = dicQueryStr[proInfoAuthKey];
                        }
                    }

                    #region 票证校验和获取用户信息

                    string url = string.Empty;//选择岗位页面URL
                    try
                    {
                        System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();

                        string service = Request.Url.GetLeftPart(UriPartial.Path);

                        if (ticket == null || ticket.Length == 0)
                        {
                            string redir = PORTALHOST + "cas.login?" +
                              "service=" + service;
                            Response.Redirect(redir);
                            return;
                        }

                        string validateurl = SSOHOST + "serviceValidate?" +
                          "ticket=" + ticket + "&" +
                          "service=" + service;
                        StreamReader Reader = new StreamReader(new WebClient().OpenRead(validateurl));
                        string resp = Reader.ReadToEnd();

                        NameTable nt = new NameTable();
                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
                        XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
                        XmlTextReader reader = new XmlTextReader(resp, XmlNodeType.Element, context);

                        string netid = null;

                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                string tag = reader.LocalName;
                                if (tag == "user")
                                    netid = reader.ReadString();
                            }
                        }

                        reader.Close();

                        if (netid == null)
                        {
                            Session["LoginError"] = "身份验证失败，未获取到登录用户信息！";
                            Server.Transfer("~/LoginError.aspx");
                        }
                        else
                        {
                            //访问的菜单名不能为空
                            if (string.IsNullOrEmpty(PortalMenuName))
                            {
                                Session["LoginError"] = "请在门户URL参数中指定一个要访问的菜单！";
                                Server.Transfer("~/LoginError.aspx");
                                return;
                            }

                            SysOperator loginUser = MLogin.TheLoginSrv.GetSysOperator(netid);
                            if (loginUser == null)
                            {
                                Session["LoginError"] = "在项目管理中未获取到用户“" + netid + "”信息！";
                                Server.Transfer("~/LoginError.aspx");
                                return;
                            }

                            List<SysRole> userJobs = MLogin.GetOperOnRoles(theSysOperator.Id, DateTime.Now).OfType<SysRole>().ToList();
                            if (userJobs == null || userJobs.Count == 0)
                            {
                                Session["LoginError"] = "用户“" + netid + "”未配置岗位信息！";
                                Server.Transfer("~/LoginError.aspx");
                                return;
                            }

                            //string sessionKey = string.Empty;//门户地址里面的票证，作为唯一键

                            //string refURL = Request.UrlReferrer.AbsoluteUri;

                            //dicQueryStr.Clear();
                            //if (refURL.IndexOf("?") > -1)
                            //{
                            //    string queryStr = refURL.Substring(refURL.IndexOf("?") + 1);
                            //    string[] str = queryStr.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                            //    foreach (string s in str)
                            //    {
                            //        string[] KeyValue = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            //        dicQueryStr.Add(KeyValue[0].ToLower(), KeyValue[1]);
                            //    }
                            //}

                            //if (dicQueryStr.Count > 0)
                            //{
                            //    if (dicQueryStr.ContainsKey(ticketKey))
                            //        sessionKey = dicQueryStr[ticketKey];
                            //}

                            Application[ticket + "UserName"] = netid;
                            Application[ticket + "URLParam"] = queryStr;
                            txtSessionKeyHidden.Value = ticket;//回传后Request.UrlReferrer.AbsoluteUri的ticket丢失，在点击确定按钮时需要用到该键

                            listGroupJob.Items.Clear();
                            foreach (SysRole group in userJobs)
                            {
                                ListItem li = new ListItem();
                                li.Text = group.RoleName;
                                li.Value = group.Id;

                                listGroupJob.Items.Add(li);
                            }
                            if (listGroupJob.Items.Count > 0)
                                listGroupJob.SelectedIndex = 0;

                            listGroupJob.Height = (Unit)(listGroupJob.Items.Count * 20);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        System.Exception ex1 = ex.InnerException;
                        while (ex1 != null)
                        {
                            message += ex1.Message;

                            ex1 = ex1.InnerException;
                        }

                        Session["LoginError"] = message;
                        Server.Transfer("~/LoginError.aspx");
                    }

                    #endregion

                    if (listGroupJob.Items.Count == 1)//如果只有一个岗位就直接登录，否则提供选择
                        Page.ClientScript.RegisterStartupScript(this.GetType(), ClientID, "document.getElementById('btnEnter').click();", true);
                    else
                        Page.ClientScript.RegisterStartupScript(this.GetType(), ClientID, "$(function() {popForm();});", true);

                }
                //else if (!IsPostBack &&
                //       Request.RawUrl != null && Request.RawUrl.Equals(appName + "/Login.aspx", StringComparison.OrdinalIgnoreCase) == false
                //       && Request.RawUrl.ToLower().IndexOf("ticket=") == -1)//超时跳到门户登录
                //{
                //    Response.Redirect("~/LoginPortal.aspx");
                //}

                #endregion 单点登录集成广迅通
            }
        }
        catch (Exception exp)
        {
            MessageBox(VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(exp));
        }
    }

    //登录
    protected void cmdSubmitButton_click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        string userName = txtUserName.Text.Trim();
        string password = txtPassword.Text;

        if (userName == "")
        {
            lblMessage.Text = "请输入用户名！";
            lblMessage.Style.Add("color", "red");
            txtUserName.Focus();
            return;
        }
        else if (password == "")
        {
            lblMessage.Text = "请输入密码！";
            lblMessage.Style.Add("color", "red");
            txtPassword.Focus();
            return;
        }
        else if (cbUserJobs.Items.Count == 0)
        {
            lblMessage.Text = "用户‘" + txtUserName.Text + "’无上岗信息！";
            lblMessage.Style.Add("color", "red");
            cbUserJobs.Focus();
            return;
        }
        else if (!MLogin.TheLoginSrv.CheckLoginPwd(theSysOperator.Id, password))
        {
            lblMessage.Text = "密码不正确！";
            lblMessage.Style.Add("color", "red");
            txtPassword.Focus();
            return;
        }

        if (SetLoginInfo() == false)
            return;

       // Response.Redirect("~/Default.aspx?PortalMenuName=MainPageMenu");
        Response.Redirect("~/Default.aspx");
    }

    private bool SetLoginInfo()
    {
        try
        {
            UserSession user = new UserSession();


            var loginJob = from j in listJobs
                           where j.Id == cbUserJobs.SelectedValue
                           select j;
            SysRole theSysRole = listJobs.ElementAt(0);


            Application.Resource.CommonClass.Domain.Login theLogin = new Application.Resource.CommonClass.Domain.Login();

            string perId = theSysOperator.Id;
            string pwd = txtPassword.Text;

            theLogin.Password = pwd;
            theLogin.LoginDate = DateTime.Now;
            theLogin.TheCurrency = MLogin.TheLoginSrv.GetBasicCurrencyInfo();
            theLogin.TheSysRole = theSysRole;
            theLogin.ThePerson = MLogin.TheLoginSrv.GetPerson(perId);
            theLogin.TheBusinessOperators = MLogin.TheLoginSrv.GetAuthor(perId);
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
        catch (Exception ee)
        {
            lblMessage.Text = ExceptionUtil.ExceptionMessage(ee);
            lblMessage.Style.Add("color", "red");

            return false;
        }
        return true;
    }

    protected void btnChangeGroupRole_Click(object sender, EventArgs e)
    {
        lblMessage.Text.Trim();
        cbUserJobs.Items.Clear();
        listGroupJob.Items.Clear();

        string perCode = txtUserName.Text.Trim();
        if (perCode == "")
        {
            lblMessage.Text = "请输入用户名！";
            lblMessage.Style.Add("color", "red");
            txtUserName.Focus();
            return;
        }

        theSysOperator = MLogin.TheLoginSrv.GetSysOperator(perCode);

        if (theSysOperator == null || theSysOperator.Id == null)
        {
            lblMessage.Text = "账号错误！";
            lblMessage.Style.Add("color", "red");
            return;
        }

        listJobs = MLogin.GetOperOnRoles(theSysOperator.Id, DateTime.Now).OfType<SysRole>().ToList();
        if (listJobs != null && listJobs.Count > 0)
        {
            foreach (SysRole job in listJobs)
            {
                ListItem li = new ListItem();
                li.Text = job.RoleName;
                li.Value = job.Id;

                cbUserJobs.Items.Add(li);
                listGroupJob.Items.Add(li);
            }
            cbUserJobs.SelectedIndex = 0;
            listGroupJob.SelectedIndex = 0;
        }

        lblMessage.Text = "用户信息正确,请输入密码！";
        lblMessage.Style.Add("color", "blue");
    }

    //选择岗位
    protected void btnEnter_Click(object sender, EventArgs e)
    {
        //获取票证作为键存储菜单名称
        string sessionKey = txtSessionKeyHidden.Value;//门户地址里面的票证，作为唯一键
        string userCode = Application[sessionKey + "UserName"].ToString();

        try
        {
            //登录验证并跳转页面
            string groupId = listGroupJob.SelectedValue;
            if (string.IsNullOrEmpty(groupId))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), ClientID, "<script type='text/javascript'>alert('请选择登录岗位！');</script>");
                listGroupJob.Focus();
                return;
            }

            //SysOperator loginUser = MLogin.TheLoginSrv.GetSysOperator(userCode);
            //SysRole theSysRole = new SysRole();
            //theSysRole.Id = groupId;

            //UserSession user = new UserSession();
            //Application.Resource.CommonClass.Domain.Login theLogin = new Application.Resource.CommonClass.Domain.Login();
            //theLogin.LoginDate = DateTime.Now;
            //theLogin.TheSysRole = theSysRole;
            //theLogin.ThePerson = MLogin.TheLoginSrv.GetPerson(loginUser.Id);

            //user.TheLogin = theLogin;
            //Session["123456"] = user;

            Application[sessionKey + "UserName"] = null;
            string URLParam = Application[sessionKey + "URLParam"].ToString();
            string portalMenuName = "";
            string PortalMenuKey = "PortalMenuName";
            string proInfoAuth = "0";
            string proInfoAuthKey = "proInfoAuth";
            if (Application[sessionKey + proInfoAuthKey] != null)
                proInfoAuth = Application[sessionKey + proInfoAuthKey].ToString();

            if (string.IsNullOrEmpty(URLParam) == false)
            {
                bool addProInfoAuth = true;
                string[] str = URLParam.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in str)
                {
                    string[] KeyValue = s.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (PortalMenuKey.Equals(KeyValue[0], StringComparison.OrdinalIgnoreCase))
                        portalMenuName = KeyValue[1];
                    if (proInfoAuthKey.Equals(KeyValue[0], StringComparison.OrdinalIgnoreCase))
                        addProInfoAuth = false;
                }

                URLParam = "?" + URLParam + "&userCode=" + userCode + "&groupId=" + groupId + "&portalIntegration=true" + (addProInfoAuth ? "&" + proInfoAuthKey + "=" + proInfoAuth : "");
            }
            else
                URLParam = "?userCode=" + userCode + "&groupId=" + groupId + "&proInfoAuth=" + proInfoAuth + "&portalIntegration=true";

            Application[userCode + "CreateSession"] = true;

            if (portalMenuName == "ProjectBasicManagement" || portalMenuName == "CompanyBusinessManagement")//登录MBP的菜单
                Response.Redirect("~/ForCS/Plugin.aspx" + URLParam);
            else if (portalMenuName == "MainPageMenu")//项目预警信息的菜单
            {
                Response.Redirect("~/MainPage/IntegratedInfoMng.aspx" + URLParam);
            }
        }
        catch (Exception exc)
        {
            MessageBox(ExceptionUtil.ExceptionMessage(exc));
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtUserName.Text = "";
        txtPassword.Text = "";
        cbUserJobs.Items.Clear();

        //txtUserName.Focus();
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
}
