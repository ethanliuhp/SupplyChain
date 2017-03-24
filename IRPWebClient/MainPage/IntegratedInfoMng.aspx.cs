using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IntegratedInfoMng_ : PageBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack == false)
            InitPage();
    }

    private void InitPage()
    {
        string MainPageLink = "IntegratedInfoMain.aspx";

        string queryStr = GetPageQueryString();
        if (queryStr != "")
            queryStr += "&rand=" + (new Random()).Next();
        else
            queryStr = "?rand=" + (new Random()).Next();

        if (MainPageLink.IndexOf("?") > -1)
            queryStr = queryStr.Replace("?", "&");

        MainPageLink += queryStr;

        frmContentMain.Attributes.Add("src", MainPageLink);
    }
}
