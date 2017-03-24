using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.core;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using VirtualMachine.Component;
using VirtualMachine.SystemAspect.Security.FunctionSecurity.Domain;

using System.Configuration;
using System.Threading;
using Application.Business.Erp.Secure.Client;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using AuthManagerLib.AuthMng.MenusMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.Main
{
    public partial class Navigate : BasicUserControl
    {
        IDictionary<string, IList> treeMenu = new Dictionary<string, IList>();
        public Menus selectedMenus;
        private static IFramework framework;
        public static IFramework Framework
        {
            get { return framework; }
            set { framework = value; }
        }
        public Menus menus;

        public Navigate(IFramework framework)
        {
          
            Navigate.framework = framework;
            InitializeComponent();
            this.ViewType = ViewType.Navigate;
            this.ViewCaption = "功能向导";

            tbPanel.Dock = DockStyle.Fill;
            this.AuthMenus.IsDistribute = true;
            this.AuthMenus.AfterMenuSelect += new AuthManager.AuthMng.AuthControlsMng.AfterMenuSelect(AuthMenus_AfterMenuSelect);
            this.AuthMenus.MenuNodeSelect += new AuthManager.AuthMng.AuthControlsMng.MenuNodeSelect(AuthMenus_MenuNodeSelect);
            //用户Code
            this.AuthMenus.TheUsersCode = ConstObject.TheLogin.ThePerson.Code;
            //一级菜单名称
            this.AuthMenus.IRPMenuName = ConstObject.IRPMenuName;
            //this.AuthMenus.IRPMenuName = "项目资源管理";
            //岗位ID
            this.AuthMenus.OperationJobId = ConstObject.TheSysRole.Id;
            if (StaticMethod.GetProjectInfo().Code == CommonUtil.CompanyProjectCode)//菜单业务归属(0:项目 1:公司)
            {
                this.AuthMenus.MenuBusinessType = 1;
            }
            VStartPage.MenuTreeView = this.AuthMenus.MenuView;
        }
        void AuthMenus_MenuNodeSelect(AuthManagerLib.AuthMng.MenusMng.Domain.Menus aMenus, TreeNode aTreeNode)
        {
            selectedMenus = aMenus;
        }

        void AuthMenus_AfterMenuSelect(AuthManagerLib.AuthMng.MenusMng.Domain.Menus aMenus, TreeNode aTreeNode)
        {
            if (aTreeNode.Nodes.Count == 0)
            {
                aMenus.Exe();
                if (aMenus.Code != null && aMenus.Code.StartsWith("Search"))
                {
                    //new CommonSearch.CommonSearchMng.ClassCommonSearchMng.VClassCommonSearchCon(aMenus.Name, ConstObject.TheLogin.ThePerson.Code).ShowDialog();
                }
                else
                {
                    UCL.TheAuthMenu = aMenus;
                    UCL.Locate(aMenus.Name);
                }
            }
        }
    }
}

