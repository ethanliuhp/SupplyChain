using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class MainPage_MainPageContent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ProjectId"] != null)
        {
            string projectId = Request.QueryString["ProjectId"];
            string ProjectName = Request.QueryString["ProjectName"];
            string projectSyscode = Request.QueryString["ProjectSyscode"];
            string projectType = Request.QueryString["ProjectType"];

            //string src = "MainPageTop.aspx?ProjectId=" + projectId + "&ProjectName=" + ProjectName + "&projectSyscode=" + projectSyscode + "&projectType=" + projectType;
            //frmTop.Attributes.Add("src", src);

            //string src = "MainPageBottom.aspx?ProjectId=" + projectId + "&ProjectName=" + ProjectName + "&projectSyscode=" + projectSyscode + "&projectType=" + projectType;
            //frmContent.Attributes.Add("src", src);

        }

        //frmContentChild.Attributes.Add("src", "MainPageBottom.aspx?rand=" + (new Random()).Next(1, 1000));
    }
}
