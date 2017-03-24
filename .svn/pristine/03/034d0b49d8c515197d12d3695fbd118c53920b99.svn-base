using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using AuthManagerLib.AuthMng.MenusMng.Domain;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;

public partial class Main_Menu : System.Web.UI.Page
{
    SessionInfo oSessionInfo = null;
    string _MenuName = "功能菜单";
    string _JosnString = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IntialTree();

            //if (!string.IsNullOrEmpty(_JosnString))
            //{
            //    this.hdMenuArr.Value = "[" + _JosnString + "]";
            //}
        }
    }
    public void IntialTree()
    {
        try
        {
            this.trvMenu.Nodes.Clear();
            oSessionInfo = PublicClass.GetUseInfo(Session);
            if (oSessionInfo.Validate())
            {
                //IList lst = GetMenu();

                //TreeNode oRootNode = CreateTreeNode(oSessionInfo.ListMenus);
                //if (oRootNode != null)
                //{
                    //this.trvMenu.Nodes.Add(oRootNode);
                //}
                Hashtable ht=CreateTreeNode(oSessionInfo.ListMenus);
                TreeNode oNode = null;
                foreach (string sKey in ht.Keys)
                {
                    oNode = ht[sKey] as TreeNode;
                    if (oNode != null && oNode.Parent == null)
                    {
                        this.trvMenu.Nodes.Add(oNode);
                    }
                }
            }
            else
            {
                throw new Exception("Session 丢失");
            }
            // trvMenu.Attributes.Add("onclick", "return ClickNode(event);");
        }
        catch (Exception ex)
        {

        }
    }

    public Hashtable CreateTreeNode(IList lstMenu)
    {
        TreeNode oRootNode = new TreeNode();
        TreeNode oChildNode = null;
        TreeNode oParentNode = null;
        Hashtable hsTable = new Hashtable();
        
        foreach (Menus oMenus in lstMenu)
        {
            if (!oMenus.IsValide) { continue; }
            if (oMenus.MenusKind != MenusKindEnm.Menu) { continue; }
            oChildNode = new TreeNode();
            oChildNode.Value = oMenus.Id;
            oChildNode.Text = oMenus.Name;
            if (!string.IsNullOrEmpty(oMenus.ExeContent))
                oChildNode.NavigateUrl = "../TransferPage.aspx?menuId=" + oMenus.Id;

            //GetJosnString(oMenus.Name);

            if (oMenus.Parent == null)
            {
                oParentNode = null;
                oRootNode = oChildNode;
            }
            else
            {
                if (hsTable.ContainsKey(oMenus.Parent.Id))
                    oParentNode = hsTable[oMenus.Parent.Id] as TreeNode;
                else
                    oParentNode = null;
            }

            if (oParentNode != null)
                oParentNode.ChildNodes.Add(oChildNode);

            hsTable.Add(oChildNode.Value, oChildNode);
        }
        return hsTable;
    }
    //public void GetJosnString(string sValue)
    //{
    //    UrlInfo oUrlInfo = Url.GetUrl(sValue, null);
    //    if (!string.IsNullOrEmpty(_JosnString))
    //    {
    //        _JosnString += ",";
    //    }
    //    _JosnString += oUrlInfo.ToJosnString();
    //}

    protected void trvMenu_SelectedNodeChanged(object sender, EventArgs e)
    {

    }
}
