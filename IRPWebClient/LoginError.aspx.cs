using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LoginError : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoginError"] != null)
        {
            string msg = Session["LoginError"].ToString();
            if (msg.IndexOf("正在中止线程") == -1)
            {
                divMessage.InnerText = "未授权，请联系系统管理员！";
                divHiddenMesDtl.InnerText = msg;
            }
        }
    }
}
