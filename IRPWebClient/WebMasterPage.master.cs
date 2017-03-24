using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;

public partial class WebMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack == false)
        {
            //门户单点登录时隐藏头部信息
            if (Request.QueryString["portalIntegration"] != null && Request.QueryString["portalIntegration"].ToLower() == "true".ToLower())
            {
                trWebHeader.Visible = false;
            }

            listGroupJob.Items.Clear();

            if (Session["UserSession"] != null)
            {
                UserSession user = Session["UserSession"] as UserSession;
                //加载岗位
                if (user.TheJobs != null && user.TheJobs.Count > 0)
                {
                    foreach (SysRole job in user.TheJobs)
                    {
                        ListItem li = new ListItem();
                        li.Text = job.RoleName;
                        li.Value = job.Id;

                        listGroupJob.Items.Add(li);
                    }
                }

                if (user.TheLogin != null)
                {
                    lblCurrJobName.Text = user.TheLogin.TheSysRole.RoleName;

                    for (int i = 0; i < listGroupJob.Items.Count; i++)
                    {
                        ListItem item = listGroupJob.Items[i];
                        if (item.Value == user.TheLogin.TheSysRole.Id)
                        {
                            listGroupJob.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }
    }
    protected void btnEnter_Click(object sender, EventArgs e)
    {
        string jobName = listGroupJob.SelectedItem.Text;
        string jobId = listGroupJob.SelectedValue;
        lblCurrJobName.Text = jobName;

        if (Session["UserSession"] != null)
        {
            UserSession user = Session["UserSession"] as UserSession;

            if (user.TheJobs != null)
            {
                var optJob = from j in user.TheJobs
                             where j.Id == jobId
                             select j;

                user.TheLogin.TheSysRole = optJob.ElementAt(0);
                Session["UserSession"] = user;
            }
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), ClientID, "<script type='text/javascript'>document.getElementById('popupContactClose').click();</script>");//window.location.replace('" + txtCurrLocation.Value + "');
    }
}
