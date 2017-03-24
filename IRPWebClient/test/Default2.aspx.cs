using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_Default2 : PageBaseClass
{
    PortalIntegration104Srv.PortalService srv = new PortalIntegration104Srv.PortalService();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        for (int i = 1; i <= 10; i++)
        {
            PortalIntegration104Srv.ProjectInfo pro = new PortalIntegration104Srv.ProjectInfo();
            pro.Name = "测试项目000" + i;
            pro.Code = null;
            PortalIntegration104Srv.RetOb result = srv.AddProjectInfo(pro);
        }
        MessageBox("ok");
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        PortalIntegration104Srv.ProjectInfo[] pros = srv.GetProjectInfoByName("测试项目000");
        foreach (PortalIntegration104Srv.ProjectInfo pro in pros)
        {
            PortalIntegration104Srv.RetOb result = srv.DeleteProjectInfo(pro);
        }
        MessageBox("ok");
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {

    }
    protected void btnAll_Click(object sender, EventArgs e)
    {

    }
}
