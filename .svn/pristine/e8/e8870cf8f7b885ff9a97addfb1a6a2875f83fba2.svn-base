using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ComponentArt.Web.UI;
using System.Text;

public partial class UserControls_WebMenuControl : UserControlBaseClass
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            PortalMenuIntegration();
    }

    /// <summary>
    /// 门户菜单集成
    /// </summary>
    private void PortalMenuIntegration()
    {
        #region 门户菜单集成

        string IRPMenuName = string.Empty;

        if (Request.QueryString["PortalMenuName"] != null)
        {
            IRPMenuName = Request.QueryString["PortalMenuName"].Trim();

            if (IRPMenuName.Equals("MainPageMenu", StringComparison.OrdinalIgnoreCase))//项目综合信息菜单
            {
                List<MenuCategory> listMenu = new List<MenuCategory>();
                MenuCategory cate1 = new MenuCategory();
                cate1.MenuText = "综合信息";
                cate1.MenuURL = Request.ApplicationPath + "/MainPage/IntegratedInfoMng.aspx" + GetPageQueryString().Replace("targetPageType=projectMap", "");

                MenuCategory cate2 = new MenuCategory();
                cate2.MenuText = "项目全景图";
                cate2.MenuURL = Request.ApplicationPath + "/MainPage/IntegratedInfoMng.aspx?targetPageType=projectMap" + GetPageQueryString().Replace("?", "&");

                MenuCategory cate3 = new MenuCategory();
                cate3.MenuText = "项目预警查询";
                cate3.MenuURL = Request.ApplicationPath + "/MainPage/ProjectWarnQuery.aspx" + GetPageQueryString().Replace("targetPageType=projectMap", "");

                //测试
                MenuCategory cate4 = new MenuCategory();
                cate4.MenuText = "付款单信息";
                cate4.MenuURL = Request.ApplicationPath + "/PaymentOrder/PaymentOrderMaster.aspx";

                MenuCategory cate5 = new MenuCategory();
                cate5.MenuText = "公司树信息";
                cate5.MenuURL = Request.ApplicationPath + "/CompanyMng/CompanyMng.aspx";
                listMenu.Add(cate1);
                listMenu.Add(cate2);
                listMenu.Add(cate3);
                //测试
                listMenu.Add(cate4);
                listMenu.Add(cate5);
                foreach (MenuCategory menu in listMenu)
                {
                    ComponentArt.Web.UI.MenuItem item = new ComponentArt.Web.UI.MenuItem();
                    item.Text = menu.MenuText;
                    item.ToolTip = menu.MenuText;
                    item.LookId = "TopItemLook";
                    ubsMenu1.Items.Add(item);

                    string webAction = menu.MenuURL;
                    if (!String.IsNullOrEmpty(webAction))
                    {
                        item.NavigateUrl = webAction;
                    }

                    if (menu.ChildMenus.Count > 0)
                    {
                        LoadChildMenus(item, menu);
                    }

                    ////add a spacer item
                    //ComponentArt.Web.UI.MenuItem itemS = new ComponentArt.Web.UI.MenuItem();
                    //itemS.LookId = "SpacerItemLook";
                    //itemS.Width = Unit.Pixel(1500);
                    //ubsMenu1.Items.Add(itemS);
                }
            }
        }

        if (ubsMenu1.Visible && ubsMenu1.Items.Count == 0)
            ubsMenu1.Visible = false;

        #endregion
    }
    /// <summary>
    /// 加载子菜单
    /// </summary>
    private void LoadChildMenus(ComponentArt.Web.UI.MenuItem parentItem, MenuCategory parentMenu)
    {
        foreach (MenuCategory menu in parentMenu.ChildMenus)
        {
            ComponentArt.Web.UI.MenuItem item = new ComponentArt.Web.UI.MenuItem();
            item.Text = menu.MenuText;
            item.ToolTip = menu.MenuText;
            item.LookId = "TopItemLook";
            parentItem.Items.Add(item);

            string webAction = menu.MenuURL;
            if (!String.IsNullOrEmpty(webAction))
            {
                item.NavigateUrl = webAction;
            }

            if (menu.ChildMenus.Count > 0)
                LoadChildMenus(item, menu);
        }
    }
}
