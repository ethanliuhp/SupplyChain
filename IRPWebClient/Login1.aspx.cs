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

public partial class Login1 : Page
{
    private static SysOperator theSysOperator = null;
    private static List<SysRole> listJobs = new List<SysRole>();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string appName = Request.ApplicationPath;
            //string PORTALHOST = System.Configuration.ConfigurationManager.AppSettings["GXTHOST"];
            string PORTALHOST = "http://www.cscec3b.com:8888/Portal";
            
            if (!Page.IsPostBack && Request.UrlReferrer != null && Request.UrlReferrer.AbsoluteUri.IndexOf(PORTALHOST) > -1)
            {
               
                string rawURL = Request.RawUrl;
                string userNameKey = "UserName".ToLower();
                string userName = "";//用户名
                string PortalMenuKey = "PortalMenuName".ToLower();
                string PortalMenuName = "";
                string tokenKey = "token";
                string token = "";//令牌
                //1:获取广迅通点击后的Url
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
                    if (dicQueryStr.ContainsKey(userNameKey))
                        userName = dicQueryStr[userNameKey].ToUpper();
                    if (dicQueryStr.ContainsKey(PortalMenuKey))
                        PortalMenuName = dicQueryStr[PortalMenuKey];
                    if (dicQueryStr.ContainsKey(tokenKey))
                        token = dicQueryStr[tokenKey];
                }

                
                //2:调用Webservice确认
                //GXTSingleLoginService.DataSvc data = new GXTSingleLoginService.DataSvc();
                //GXTSingleLoginService.ReturnData rData = data.GetAllData(token, "usr");
                
                //登录系统
                SysOperator loginUser = MLogin.TheLoginSrv.GetSysOperator(userName);
                if (loginUser == null)
                {
                    Session["LoginError"] = "在项目管理中未获取到用户“" + userName + "”信息！";
                    Server.Transfer("~/LoginError.aspx");
                    return;
                }

                List<SysRole> userJobs = MLogin.GetOperOnRoles(loginUser.Id, DateTime.Now).OfType<SysRole>().ToList();
                if (userJobs == null || userJobs.Count == 0)
                {
                    Session["LoginError"] = "用户“" + userName + "”未配置岗位信息！";
                    Server.Transfer("~/LoginError.aspx");
                    return;
                }

                Application[token + "UserName"] = userName;
                Application[token + "PortalMenuName"] = PortalMenuName;
                txtSessionKeyHidden.Value = token;

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
                //listGroupJob.Height = (Unit)(listGroupJob.Items.Count * 20);
//listGroupJob.Height = 20;
		listGroupJob.Height = (Unit)((listGroupJob.Items.Count>20 ? 20 : listGroupJob.Items.Count) * 20);
                if (listGroupJob.Items.Count == 1)//如果只有一个岗位就直接登录，否则提供选择
                    Page.ClientScript.RegisterStartupScript(this.GetType(), ClientID, "document.getElementById('btnEnter').click();", true);
                else
                    Page.ClientScript.RegisterStartupScript(this.GetType(), ClientID, "$(function() {popForm();});", true);

            }

        }
        catch (Exception exp)
        {
            MessageBox(VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(exp));
        }
    }
    //选择岗位
    protected void btnEnter_Click(object sender, EventArgs e)
    {
       
        //获取票证作为键存储菜单名称
        string token = txtSessionKeyHidden.Value;//门户地址里面的票证，作为唯一键
        string userCode = Application[token + "UserName"].ToString();
        
        
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

            Application[token + "UserName"] = null;
            string URLParam = "";
            string portalMenuName = "ProjectBasicManagement";
            if (Application[token + "PortalMenuName"] != null)
            {
                portalMenuName = Application[token + "PortalMenuName"].ToString();
            }
            string proInfoAuth = "0";
            string proInfoAuthKey = "proInfoAuth";
            if (Application[token + proInfoAuthKey] != null)
                proInfoAuth = Application[token + proInfoAuthKey].ToString();

            URLParam = "?userCode=" + userCode + "&groupId=" + groupId + "&proInfoAuth=" + proInfoAuth + "&portalIntegration=true";

            Application[userCode + "CreateSession"] = true;
            if (portalMenuName == "ProjectBasicManagement" || portalMenuName == "CompanyBusinessManagement")//登录MBP的菜单
            {
                Response.Redirect("~/ForCS/Plugin.aspx" + URLParam);
            }
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
