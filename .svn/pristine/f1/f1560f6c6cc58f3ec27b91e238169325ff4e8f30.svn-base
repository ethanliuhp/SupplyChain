using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using VirtualMachine.Core;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using AuthManagerLib.AuthMng.MenusMng.Domain;
using NHibernate.Criterion;


public partial class AuthMsg_Auth_MenuMsg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IntialMenu();
            IntialRole();
            SetState();

        }
    }
    public void IntialRole()
    {
        IList lstRoles = new ArrayList();
        ObjectQuery oq = new ObjectQuery();
        string sMsg = string.Empty;
        try
        {
            lstRoles = AuthSvr.AuthConfigSrv.GetAuthRole(oq);
        }
        catch (Exception ex)
        {
            sMsg = "加载角色出错:" + ex.Message;
        }
        trvGroup.Nodes.Clear();
        if (string.IsNullOrEmpty(sMsg))
        {

            TreeNode oRootNode = new TreeNode();
            oRootNode.Text = "角色";
            oRootNode.Value = "";
            oRootNode.Target = "";
            TreeNode oChildNode = null;
            foreach (AuthRole oAuthRole in lstRoles)
            {
                oChildNode = new TreeNode();
                oChildNode.Text = oAuthRole.RoleName;
                oChildNode.Value = oAuthRole.Id;
                oChildNode.Target = oAuthRole.Id;
                oRootNode.ChildNodes.Add(oChildNode);
            }
            this.trvGroup.Nodes.Add(oRootNode);
            oRootNode.ShowCheckBox = false;
            if (oRootNode != null && oRootNode.ChildNodes.Count > 0)
            {
                oRootNode.Select();
                SelectRoleNode(this.trvGroup.SelectedNode);
            }
        }
        MessageBox(sMsg);
    }
    public void IntialMenu()
    {
        string sMsg = string.Empty;
        IList lstMenu = null;
        TreeNode oRootNode = null;
        Hashtable ht = new Hashtable();
        try
        {
            ObjectQuery oq = new ObjectQuery();
           Menus oRootMenu= PublicClass.GetRootMenu();
            string IdCode=oRootMenu==null ?"%":oRootMenu.Id+"%";
            oq.AddCriterion(Expression.Like("IdCode", IdCode));
            lstMenu = AuthSvr.AuthConfigSrv.GetMenus(oq);
        }
        catch (Exception ex)
        {
            sMsg = "加载权限出错：" + ex.Message;
        }
        this.trvMenu.Nodes.Clear();

        Menus oRootMenus = new Menus();
        oRootMenus.Id = "功能菜单";
        oRootMenus.Name = "功能菜单";
        oRootMenus.Code = "功能菜单";
        oRootMenus.Parent = null;
        oRootNode = new TreeNode();
        oRootNode.Text = string.IsNullOrEmpty(oRootMenus.Code) ? oRootMenus.Name : "[" + oRootMenus.Code + "]" + oRootMenus.Name;
        oRootNode.Value = oRootMenus.Id;
        oRootNode.Target = oRootMenus.Id;
        ht.Add(oRootNode.Target,oRootNode);
        if (lstMenu != null && lstMenu.Count > 0)
        {

            if (string.IsNullOrEmpty(sMsg))
            {
               
                // ht.Add(oRootNode.Target, oRootNode);
                TreeNode oChildNode = null;
                TreeNode oParentNode = null;
                foreach (Menus oMenu in lstMenu)
                {
                    if (oMenu.Parent == null)
                    {
                        oChildNode = new TreeNode();
                        oChildNode.Text = oMenu.Name;
                        oChildNode.Value = oMenu.Id;
                        oChildNode.Target = oMenu.Id;
                        oRootNode.ChildNodes.Add(  oChildNode);
                        ht.Add(oChildNode.Target, oChildNode);
                        oChildNode.ShowCheckBox = true;
                    }
                }
                foreach (Menus oMenu in lstMenu)
                {
                    oChildNode = new TreeNode();
                    oChildNode.Text = oMenu.Name;
                    oChildNode.Value = oMenu.Id;
                    oChildNode.Target = oMenu.Id;
                    oChildNode.ShowCheckBox = true;
                    if (oMenu.Parent == null)
                    {
                        //oRootNode = oChildNode;
                    }
                    else
                    {
                        if (ht.ContainsKey(oMenu.Parent.Id))
                        {
                            oParentNode = ht[oMenu.Parent.Id] as TreeNode;
                            if (oParentNode != null)
                            {
                                oParentNode.ChildNodes.Add(oChildNode);
                            }
                            ht.Add(oChildNode.Target, oChildNode);

                        }
                    }

                }
            }
        }
       //IList lstRemoveKey=new ArrayList();
       //foreach (string sKey in ht.Keys)
       //{
       //    TreeNode oTempNode = null;
       //    if (!string.Equals(sKey, oRootNode.Target) )
       //    {
       //        oTempNode = ht[sKey] as TreeNode;
       //        if (oTempNode != null && oTempNode.Parent == oRootNode && oTempNode.ChildNodes.Count == 0) 
       //        {
       //            lstRemoveKey.Add(sKey);
       //        }
       //    }
       //}
       //foreach (string sKey in lstRemoveKey)
       //{

       //}
       //for(int i=ht.Keys.Count-1;i>-1;i--){
       //    if (!string.Equals(ht.Keys., oRootNode.Target)&&(ht[ht.Keys[i]] as TreeNode).ChildNodes.Count == 0)
       //    {
       //         ht.Remove(ht.Keys[i]);
               
       //    }
       //}
        oRootNode.Select();
        this.trvMenu.Nodes.Add(oRootNode);
        this.trvMenu.ShowCheckBoxes = TreeNodeTypes.All;
        this.trvMenu.Attributes.Add("onclick", "postBackByObject()");
        MessageBox(sMsg);
    }
    public void SelectRoleNode(TreeNode oNode)
    {
        if (oNode == null)
        {

        }
        else
        {

        }
    }
    public void MessageBox(string sMessage)
    {
        string sMsg = string.Empty;
        if (!string.IsNullOrEmpty(sMessage))
        {
            sMsg = string.Format("<script>alert('{0}');</script>", sMessage);
            // Response.Write(sMsg);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", sMsg);
        }
    }
    public void SetState()
    {
        if (this.trvMenu.SelectedNode != null)
        {
            this.hdState.Value = "0";
        }
        else
        {
            this.hdState.Value = "";
        }
    }
    public Menus GetSelectMenu()
    {
        Menus oMenu = null;
        if (this.trvMenu.SelectedNode != null)
        {
            oMenu = GetMenusById(this.trvMenu.SelectedNode.Value);// AuthSvr.MenusSrv.GetMenusById(this.trvMenu.SelectedNode.Value) as Menus;//.GetMenusByID(this.trvMenu.SelectedNode.Value);
        }
        return oMenu;
    }
    public IList GetSelectMenus()
    {
        IList lstMenuIDs = new ArrayList();
        IList lstMenu = null;
        if (this.trvMenu.SelectedNode != null)
        {
            GetMenuID(this.trvMenu.SelectedNode, lstMenuIDs);
        }

        if (lstMenuIDs != null && lstMenuIDs.Count > 0)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.In("Id", lstMenuIDs));

            lstMenu = AuthSvr.AuthConfigSrv.GetMenus(oQuery);
        }
        return lstMenu;
    }
    public void GetMenuID(TreeNode oNode, IList lstMenuIDs)
    {
        if (oNode != null)
        {
            lstMenuIDs.Add(oNode.Value);
            foreach (TreeNode oChildNode in oNode.ChildNodes)
            {
                GetMenuID(oChildNode, lstMenuIDs);
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string sMsg = string.Empty;
        Menus oMenus = null;

        string sState = hdState.Value;
        try
        {
            oMenus = GetSelectMenu();

            if (this.trvMenu.SelectedNode != null)
            {
                if (string.Equals(sState, "0"))
                {
                    if (oMenus != null)
                    {
                        UpdateMenus(oMenus);
                        //PublicClass.WriteLog(oMenus.Id, "菜单", oMenus.Code, "", Session, "[修改]");
                        SetState();
                    }
                    else
                    {
                        sMsg = "不能修改根节点";
                    }
                }
                else if (string.Equals(sState, "1"))
                {
                    AddMenus(oMenus);
                   // PublicClass.WriteLog(oMenus.Id, "菜单", oMenus.Code, "", Session, "[添加]");
                    SetState();
                }
            }
            else
            {
                sMsg = "请选择一个菜单节点";
            }

        }
        catch (Exception ex)
        {
            sMsg = "保存出错:" + ex.Message;
        }
        MessageBox(sMsg);
    }
    public void UpdateMenus(Menus oOldMenus)
    {
        string sMsg = string.Empty;
        if (oOldMenus == null)
        {
            sMsg = "根节点无法保存";
        }
        else
        {
            ViewToModel(oOldMenus);
            oOldMenus = this.SaveOrUpdateMenus(oOldMenus);
           
            sMsg = "修改成功";
            this.trvMenu.SelectedNode.Text = oOldMenus.Name;
        }
        MessageBox(sMsg);
    }
    public void AddMenus(Menus oParentMenus)
    {
        string sMsg = string.Empty;
        Menus oChildMenus = new Menus();
        ViewToModel(oChildMenus);
        oChildMenus.Parent = oParentMenus;
        oChildMenus.Level =oParentMenus==null ?1: oParentMenus.Level + 1;
        oChildMenus.Serial = trvMenu.SelectedNode == null ? 0 : trvMenu.SelectedNode.ChildNodes.Count;
      
        oChildMenus = this.SaveOrUpdateMenus(oChildMenus);//SaveOrUpdateMenus(oChildMenus);
        sMsg = "新增成功";
        TreeNode oChildNode = new TreeNode();
        oChildNode.Text = string.IsNullOrEmpty(oChildMenus.Code) ? oChildMenus.Name : "[" + oChildMenus.Code + "]" + oChildMenus.Name;
        oChildNode.Value = oChildMenus.Id;
        oChildNode.Target = oChildMenus.Id;
        this.trvMenu.SelectedNode.ChildNodes.Add(oChildNode);
        this.trvMenu.SelectedNode.Expanded = true;
        oChildNode.Select();
        MessageBox(sMsg);
    }
    public void ModelToView(Menus oMenus)
    {
        if (oMenus != null)
        {
            this.txtCode.Text = oMenus.Code;
            this.txtName.Text = oMenus.Name;
            txtPagePath.Text = oMenus.ExeContent;
        }
        else
        {
            this.txtCode.Text = "";
            this.txtName.Text ="";
            txtPagePath.Text = "";
        }
    }
    public void ViewToModel(Menus oMenus)
    {
        oMenus.Name = txtName.Text;
        oMenus.Code = txtCode.Text;
        oMenus.ExeContent = txtPagePath.Text.Trim();
        oMenus.Serial = (this.trvMenu.SelectedNode == null || this.trvMenu.SelectedNode.Parent == null) ? 0 : this.trvMenu.SelectedNode.Parent.ChildNodes.IndexOf(this.trvMenu.SelectedNode);
    }
    protected void btnSaveLink_Click(object sender, EventArgs e)
    {
        SetMenuAndRoleLInk();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string sMsg = string.Empty;
        try
        {
            IList lstMenus = GetSelectMenus();
            if (lstMenus != null && lstMenus.Count > 0)
            {
                if (lstMenus.Count > 1)
                {
                    Menus oMenus = lstMenus[0] as Menus;
                    sMsg = string.Format("删除失败:建议请在清除[{0}]节点前，先清除该节点下面的子节点后", this.trvMenu.SelectedNode.Text);
                }
                else
                {
                    bool bFlag = AuthSvr.MenusSrv.Delete(lstMenus);//RemoveMenus(lstMenus);
                    if (bFlag)
                    {
                        foreach (Menus oMenus in lstMenus)
                        {
                            //PublicClass.WriteLog(oMenus.Id, "菜单", oMenus.Code, "", Session, "[删除]");
                        }
                    }
                    sMsg = bFlag ? "删除成功" : "删除失败";
                    if (this.trvMenu.SelectedNode.Parent != null)
                    {
                        this.trvMenu.SelectedNode.Parent.ChildNodes.Remove(this.trvMenu.SelectedNode);
                    }
                }
                //DeleteCheckNode(this.trvMenu.Nodes[0]);
                TreeNode oNode = this.trvMenu.Nodes[0];
                oNode.Select();
                SetState();

            }

            else
            {
                sMsg = "删除失败:请勾选节点";
            }

        }
        catch (Exception ex)
        {
            sMsg = "删除错误:" + ex.Message;
        }
        MessageBox(sMsg);
    }
    public void DeleteCheckNode(TreeNode oCurrentNode)
    {
        if (oCurrentNode.Checked && !string.IsNullOrEmpty(oCurrentNode.Value))
        {
            oCurrentNode.Parent.ChildNodes.Remove(oCurrentNode);
        }
        else
        {
            for (int i = 0; i < oCurrentNode.ChildNodes.Count; i++)
            {
                DeleteCheckNode(oCurrentNode.ChildNodes[i]);
            }
        }
    }
    protected void trvMenu_SelectedNodeChanged(object sender, EventArgs e)
    {
        if (this.trvMenu.SelectedNode != null)
        {
          //  IList lstMenus =  GetMenusById(this.trvMenu.SelectedNode.Value); //AuthSvr.MenusSrv.GetMenusById(this.trvMenu.SelectedNode.Value);//AuthSvr.MenusSrv.GetMenusByID(this.trvMenu.SelectedNode.Value);
          // Menus oMenus = lstMenus != null && lstMenus.Count > 0 ? lstMenus[0] as Menus : null;
            Menus oMenus = GetMenusById(this.trvMenu.SelectedNode.Value);
            if (oMenus == null && string.IsNullOrEmpty(this.trvMenu.SelectedNode.Value))
            {
                oMenus = new Menus();
                oMenus.Name = "功能菜单";
                oMenus.Code = "";
                ModelToView(oMenus);
                SetState();
            }
            else
            {
                ModelToView(oMenus);
                SetState();
            }
        }
    }

    protected void trvGroup_SelectedNodeChanged(object sender, EventArgs e)
    {
        IList lstAuthConfig = null;
        string sRoleID = string.Empty;
        if (this.trvGroup.SelectedNode != null && !string.IsNullOrEmpty(this.trvGroup.SelectedNode.Value))
        {
            sRoleID = this.trvGroup.SelectedNode.Value;
        
             lstAuthConfig = AuthSvr.AuthConfigSrv.GetRoleAuthConfig(sRoleID);//GetAuthConfigByRoleID(sRoleID);

            SetMenu(lstAuthConfig, this.trvMenu.Nodes[0]);

        }
    }
    public AuthRole GetSelectRole()
    {
        AuthRole oAuthRole = null;
        if (this.trvGroup.SelectedNode != null && this.trvGroup.SelectedNode.Value != "")
        {
            string sRoleID = trvGroup.SelectedNode.Value;
            ObjectQuery oQuery = new ObjectQuery();

            //IList lstAuthRole = AuthSvr.AuthConfigSrv.GetRoleAuthConfig(sRoleID);//GetAuthRoleByRoleID(sRoleID);
            //oAuthRole = lstAuthRole != null && lstAuthRole.Count > 0 ? lstAuthRole[0] as AuthRole : null;
            oAuthRole = GetAuthRoleByRoleID(sRoleID);
        }
        return oAuthRole;
    }
    public void SetMenu(IList lstAuthConfig, TreeNode oCurrent)
    {

        if (oCurrent != null && lstAuthConfig != null)
        {
            oCurrent.Checked = false;
            TreeNode oChildNode = null;
            foreach (AuthConfig oAuthConfig in lstAuthConfig)
            {
                if (string.IsNullOrEmpty(oCurrent.Value))
                {
                    break;
                }
                if (string.Equals(oAuthConfig.Menus.Id, oCurrent.Value))
                {
                    oCurrent.Checked = true;
                    break;
                }
            }
            for (int i = 0; i < oCurrent.ChildNodes.Count; i++)
            {
                oChildNode = oCurrent.ChildNodes[i];
                SetMenu(lstAuthConfig, oChildNode);
            }
        }
    }
    public IList GetCheckedMenuIDs()
    {
        IList lstMenuID = new ArrayList();
        //if (oCurrent.Checked&&!string.IsNullOrEmpty ( oCurrent .Value))
        //{
        //    lstMenuID.Add(oCurrent.Value);

        //}
        //foreach (TreeNode oChildNode in oCurrent.ChildNodes)
        //{
        //    GetCheckedMenu(lstMenuID, oChildNode);
        //}
        foreach (TreeNode oNode in this.trvMenu.CheckedNodes)
        {
            lstMenuID.Add(oNode.Value);
        }
        return lstMenuID;

    }
    public void SetMenuAndRoleLInk()
    {
        string sMsg = string.Empty;
        IList lstMenu = new ArrayList();
        //IList lstMenuIDs = new ArrayList();
        IList lstAuthConfig = new ArrayList();
        AuthRole oRole = GetSelectRole();
        string sIDs = string.Empty;
        AuthConfig oAuthConfig = null;
        try
        {
            if (oRole != null)
            {
                lstMenu = GetCheckMenus();
                if (lstMenu != null)
                {
                    foreach (Menus oMenus in lstMenu)
                    {
                        oAuthConfig = new AuthConfig();
                        oAuthConfig.IsHas = true;
                        oAuthConfig.Roles = oRole;
                        oAuthConfig.RoleName = oRole.RoleName;
                        oAuthConfig.Menus = oMenus;
                        oAuthConfig.RoleName = oMenus.Name;
                        lstAuthConfig.Add(oAuthConfig);
                    }
                }
                AuthSvr.AuthConfigSrv.SaveAuthConfig(oRole.Id, lstAuthConfig);
                sMsg = "关联保存成功";
            }
            else
            {
                sMsg = "请选择角色节点";
            }
        }
        catch (Exception ex)
        {
            sMsg = "保存失败:" + ex.Message;
        }
        MessageBox(sMsg);
    }
    public IList GetCheckMenus()
    {
        IList lstMenuIDs = this.GetCheckedMenuIDs();
        IList lstMenu = null;
        if (lstMenuIDs != null && lstMenuIDs.Count > 0)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.In("Id", lstMenuIDs));
            lstMenu = AuthSvr.AuthConfigSrv.GetMenus(oQuery);
        }
        else
        {
        }
        return lstMenu;
    }
    //protected void trvMenu_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    //{
    //    bool bValue = e.Node.Checked;

    //    foreach (TreeNode oNode in e.Node.ChildNodes)
    //    {
    //        SetMenuCheck( oNode ,bValue );
    //    }
    //}
    public void SetMenuCheck(TreeNode oNode, bool bValue)
    {
        oNode.Checked = bValue;
        foreach (TreeNode oChildNode in oNode.ChildNodes)
        {
            SetMenuCheck(oChildNode, bValue);
        }

    }
    protected void btnMoveUp_Click(object sender, EventArgs e)
    {

        string sMsg = string.Empty;
        if (this.trvMenu.SelectedNode != null)
        {
            try
            {
                string sChildID = trvMenu.SelectedNode.Value;
                string sBortherID = string.Empty;
                IList lstMenu = null;
                if (trvMenu.SelectedNode.Parent != null)
                {

                    int index = trvMenu.SelectedNode.Parent.ChildNodes.IndexOf(trvMenu.SelectedNode);
                    if (index == 0)
                    {
                        sMsg = "该节点为首节，无法上移";
                    }
                    else
                    {
                        sBortherID = trvMenu.SelectedNode.Parent.ChildNodes[index - 1].Value;
                        ObjectQuery oQuery = new ObjectQuery();
                        oQuery.AddCriterion(Expression.In("Id", new object[] { sChildID, sBortherID }));
                        lstMenu = AuthSvr.AuthConfigSrv.GetMenus(oQuery);
                        if (lstMenu != null && lstMenu.Count == 2)
                        {
                            Menus oBorther = null;
                            Menus oCurrent = null;
                            foreach (Menus oMenus in lstMenu)
                            {
                                if (string.Equals(oMenus.Id, sChildID))
                                {
                                    oCurrent = oMenus;
                                }
                                else if (string.Equals(oMenus.Id, sBortherID))
                                {
                                    oBorther = oMenus;
                                }
                            }
                            if (oBorther != null && oCurrent != null)
                            {
                                oBorther.Serial += 1;
                                oCurrent.Serial -= 1;
                                AuthSvr.AuthConfigSrv.Update(new ArrayList() { oBorther, oCurrent });
                                TreeNode oCurrentNode = this.trvMenu.SelectedNode;
                                TreeNode oBortherNode = trvMenu.SelectedNode.Parent.ChildNodes[index - 1];
                                TreeNode oParentNode = trvMenu.SelectedNode.Parent;
                                oParentNode.ChildNodes.Remove(oBortherNode);
                                oParentNode.ChildNodes.Remove(oCurrentNode);
                                oParentNode.ChildNodes.AddAt(index - 1, oBortherNode);
                                oParentNode.ChildNodes.AddAt(index - 1, oCurrentNode);
                                // sMsg = "没有找到兄弟节点";
                            }
                        }
                        else
                        {
                            sMsg = "没有找到兄弟节点";
                        }
                    }
                }
                else
                {
                    sMsg = "该节点为根节点";
                }
            }
            catch (Exception ex)
            {
                sMsg = "保存失败：" + ex.Message;
            }
        }
        else
        {
            sMsg = "请选择菜单节点";
        }
        MessageBox(sMsg);
    }
    protected void btnMoveDown_Click(object sender, EventArgs e)
    {
        string sMsg = string.Empty;
        if (this.trvMenu.SelectedNode != null)
        {
            try
            {
                string sChildID = trvMenu.SelectedNode.Value;
                string sBortherID = string.Empty;
                IList lstMenu = null;
                if (trvMenu.SelectedNode.Parent != null)
                {

                    int index = trvMenu.SelectedNode.Parent.ChildNodes.IndexOf(trvMenu.SelectedNode);
                    if (index == trvMenu.SelectedNode.Parent.ChildNodes.Count - 1)
                    {
                        sMsg = "该节点为为末节点，无法下移";
                    }
                    else
                    {
                        sBortherID = trvMenu.SelectedNode.Parent.ChildNodes[index + 1].Value;
                        ObjectQuery oQuery = new ObjectQuery();
                        oQuery.AddCriterion(Expression.In("Id", new object[] { sChildID, sBortherID }));
                        lstMenu = AuthSvr.AuthConfigSrv.GetMenus(oQuery);
                        if (lstMenu != null && lstMenu.Count == 2)
                        {
                            Menus oBorther = null;
                            Menus oCurrent = null;
                            foreach (Menus oMenus in lstMenu)
                            {
                                if (string.Equals(oMenus.Id, sChildID))
                                {
                                    oCurrent = oMenus;
                                }
                                else if (string.Equals(oMenus.Id, sBortherID))
                                {
                                    oBorther = oMenus;
                                }
                            }
                            if (oBorther != null && oCurrent != null)
                            {
                                oBorther.Serial -= 1;
                                oCurrent.Serial += 1;
                                AuthSvr.AuthConfigSrv.Update(new ArrayList() { oBorther, oCurrent });
                                TreeNode oCurrentNode = this.trvMenu.SelectedNode;
                                TreeNode oBortherNode = trvMenu.SelectedNode.Parent.ChildNodes[index + 1];
                                TreeNode oParentNode = trvMenu.SelectedNode.Parent;
                                oParentNode.ChildNodes.Remove(oBortherNode);
                                oParentNode.ChildNodes.Remove(oCurrentNode);
                                oParentNode.ChildNodes.AddAt(index, oCurrentNode);
                                oParentNode.ChildNodes.AddAt(index, oBortherNode);

                                // sMsg = "没有找到兄弟节点";
                            }
                        }
                        else
                        {
                            sMsg = "没有找到兄弟节点";
                        }
                    }
                }
                else
                {
                    sMsg = "该节点为根节点";
                }
            }
            catch (Exception ex)
            {
                sMsg = "保存失败：" + ex.Message;
            }
        }
        else
        {
            sMsg = "请选择菜单节点";
        }
        MessageBox(sMsg);
    }
    #region 对菜单进行增删改
    public Menus SaveOrUpdateMenus(Menus oMenus)
    {
        if (string.IsNullOrEmpty(oMenus.Id))
        {
            oMenus = AuthSvr.MenusSrv.Save(oMenus) as Menus;
        }
        else
        {
            oMenus = AuthSvr.MenusSrv.Update(oMenus) as Menus;
        }
        return oMenus;
    }
    public bool DeleteMenus(IList lstMenus)
    {
        AuthSvr.MenusSrv.Delete(lstMenus);
        DeleteConfig(lstMenus);
        return true;
    }
    public bool DeleteConfig(IList lstMenus)
    {
        bool bFlag = false;
        IList lstIDs = GetMenuIDs(lstMenus);
        if (lstIDs != null && lstIDs.Count > 0)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.In("Menus.Id", lstIDs));
            IList lstAuthConfig = AuthSvr.MenusSrv.GetObjects(typeof(AuthConfig), oQuery);
            if (lstAuthConfig != null && lstAuthConfig.Count > 0)
            {
                AuthSvr.MenusSrv.Delete(lstAuthConfig);
                bFlag = true;
            }
        }
        return bFlag;
    }
    public IList GetMenuIDs(IList lstMenus)
    {
        IList lstID = new ArrayList();
        foreach (Menus oMenus in lstMenus)
        {
            lstID.Add(oMenus.Id);
        }
        return lstID;
    }
    public Menus GetMenusById(string strID)
    {
        ObjectQuery oQuery=new ObjectQuery ();
        oQuery.AddCriterion(Expression.Eq("Id",strID));
        IList lstMenus = AuthSvr.MenusSrv.GetObjects(typeof(Menus), oQuery);
        return lstMenus != null && lstMenus.Count > 0 ? lstMenus[0] as Menus : null;
    }
    #endregion
    #region 权限配置
    public AuthRole GetAuthRoleByRoleID(string sRoleID)
    {
        ObjectQuery oQuery = new ObjectQuery();
        oQuery.AddCriterion(Expression.Eq("Id",sRoleID));
      IList lst=  AuthSvr.AuthConfigSrv.GetAuthRole(oQuery);
      return lst == null || lst.Count == 0 ? null : lst[0] as AuthRole;
    }
    #endregion
}
