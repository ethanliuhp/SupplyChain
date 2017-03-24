using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class TransferPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool flag = false;
        if (Request.QueryString["menuId"] != null)
        {
            string menuId = Request.QueryString["menuId"];
            SessionInfo userInfo = PublicClass.GetUseInfo(Session);

            if (userInfo != null)
            {
                var query = from m in userInfo.ListMenus
                            where m.Id == menuId
                            select m;
                if (query.Count() > 0)
                {
                    flag = true;

                    //Random rand = new Random();
                    //int randValue = rand.Next(Int32.MinValue, Int32.MaxValue);

                    string ticketKey = PublicClass.GetAuthMenuIdEncryptKey();
                    string ticketValue = PublicClass.GetAuthMenuIdEncryptValue(ticketKey);
                    Session[ticketKey] = ticketValue;
                    string sPath = query.ElementAt(0).ExeContent; //~
                      sPath = sPath.IndexOf("~")==0 ? sPath : "~" + sPath;
                      sPath += sPath.IndexOf("?") < 0 ? "?" : "&";
                    Response.Redirect(sPath + "ticketKey=" + ticketKey, true);
                }
            }
            else
            {
                FormsAuthentication.SignOut();
            }
        }

        if (flag == false)
        {
            Response.Redirect("~/AuthenticationFailed.aspx");
        }
    }
}
