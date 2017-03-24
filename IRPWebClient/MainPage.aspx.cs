using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Component.Util;

public partial class MainPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            //string targetPageType = "projectWarning";
            //if (Request.QueryString["targetPageType"] != null)
            //    targetPageType = Request.QueryString["targetPageType"];

            string queryStr = GetPageQueryString();

            frmLeft.Attributes.Add("src", "MainPage/MainPageLeft.aspx" + queryStr);
            //frmContent.Attributes.Add("src", "MainPage/MainPageContent.aspx");
        }
    }
    private string GetPageQueryString()
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
}
