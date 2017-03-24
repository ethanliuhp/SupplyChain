using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_Default : PageBaseClass
{
    PortalIntegration.PortalService srv = new PortalIntegration.PortalService();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {   
        PortalIntegration.ProjectInfo proa1 = new PortalIntegration.ProjectInfo();
        proa1.Name="测试项目001";
        proa1.Code="test001";
        
        srv.AddProjectInfo(proa1);

        PortalIntegration.ProjectInfo proa2 = new PortalIntegration.ProjectInfo();
        proa2.Name = "测试项目002";
        proa2.Code = "test001";

        srv.AddProjectInfo(proa2);

        PortalIntegration.ProjectInfo proa3 = new PortalIntegration.ProjectInfo();
        proa3.Name = "测试项目003";
        proa3.Code = "test001";

        srv.AddProjectInfo(proa3);

        PortalIntegration.ProjectInfo proa4 = new PortalIntegration.ProjectInfo();
        proa4.Name = "测试项目004";
        proa4.Code = "test001";

        srv.AddProjectInfo(proa4);

        PortalIntegration.ProjectInfo proa5 = new PortalIntegration.ProjectInfo();
        proa5.Name = "测试项目005";
        proa5.Code = "测试项目005";

        srv.AddProjectInfo(proa5);

        MessageBox("新建ok");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        PortalIntegration.ProjectInfo[] prou1 =srv.GetProjectInfo("测试项目001");
        PortalIntegration.ProjectInfo a1= prou1.ElementAt(0);
        a1.BuildingHeight = 11;
        a1.ProjectCost = 201;
        
        srv.UpdateProjectInfo(a1);

        PortalIntegration.ProjectInfo[] prou2 = srv.GetProjectInfo("测试项目003");
        PortalIntegration.ProjectInfo a2 = prou2.ElementAt(0);
        a2.BuildingHeight = 13;
        a2.ProjectCost = 203;

        srv.UpdateProjectInfo(a2);

        MessageBox("修改ok");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        PortalIntegration.ProjectInfo[] prod1 = srv.GetProjectInfo("测试项目001");
        PortalIntegration.ProjectInfo d1 = prod1.ElementAt(0);
        
        srv.DeleteProjectInfo(d1);

        PortalIntegration.ProjectInfo[] prod2 = srv.GetProjectInfo("测试项目002");
        PortalIntegration.ProjectInfo d2 = prod2.ElementAt(0);

        srv.DeleteProjectInfo(d2);


        MessageBox("删除ok");
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        PortalIntegration.ProjectInfo proa1 = new PortalIntegration.ProjectInfo();
        proa1.Name = "测试项目001";
        proa1.Code = "test001";

        srv.AddProjectInfo(proa1);

        PortalIntegration.ProjectInfo proa6 = new PortalIntegration.ProjectInfo();
        proa6.Name = "测试项目005";
        proa6.Code = "test001";

        srv.AddProjectInfo(proa6);

        PortalIntegration.ProjectInfo[] prou2 = srv.GetProjectInfo("测试项目002");
        PortalIntegration.ProjectInfo a2 = prou2.ElementAt(0);
        a2.BuildingHeight = 12.0m;
        a2.ProjectCost = 202.0m;

        srv.UpdateProjectInfo(a2);

        PortalIntegration.ProjectInfo[] prou21 = srv.GetProjectInfo("测试项目002");
        PortalIntegration.ProjectInfo a21 = prou2.ElementAt(0);
        a21.BuildingHeight = 121.1m;
        a21.ProjectCost = 202.1m;

        srv.UpdateProjectInfo(a21);

        PortalIntegration.ProjectInfo[] prod3 = srv.GetProjectInfo("测试项目005");
        PortalIntegration.ProjectInfo d3 = prod3.ElementAt(0);

        srv.DeleteProjectInfo(d3);


        MessageBox("ok");
    }
}
