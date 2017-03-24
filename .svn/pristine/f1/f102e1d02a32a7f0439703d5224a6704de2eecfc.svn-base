using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_ubsWebHeader : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserSession"] != null)
        {
            pnlUserInfo.Visible = true;
            lblLoginOrExit.InnerText = "注销";

            UserSession user = Session["UserSession"] as UserSession;
            if (user.TheLogin != null)
                lblUserInfo.InnerText = "[" + user.TheLogin.ThePerson.Name + ",欢迎您!]";
        }
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Session["UserSession"] = null;
        Response.Redirect("~/Login.aspx");
    }
}
