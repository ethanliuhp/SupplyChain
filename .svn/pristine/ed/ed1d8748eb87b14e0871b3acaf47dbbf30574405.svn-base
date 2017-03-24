using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.SystemAspect.Security.FunctionSecurity.Service;
using System.Windows.Forms;
using System.Collections;
using VirtualMachine.SystemAspect.Security.FunctionSecurity.Domain;
using System.Reflection;
using VirtualMachine.Component.WinMVC.generic;
using Application.Resource.PersonAndOrganization.ClientManagement.Service;
using VirtualMachine.Component.Util;
using VirtualMachine.SystemAspect.Security.InstanceSecurity.Service;


namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class CommonMenu
    {
        private static IFramework theFramework;
        /// <summary>
        /// 系统框架
        /// </summary>
        public static IFramework TheFramework
        {
            get
            {
                if (theFramework == null)
                {
                    throw new Exception("请给TheFramework赋值！");
                }
                return theFramework;
            }
            set { theFramework = value; }
        }

        private static Hashtable hashtableMenu = new Hashtable();
        private static TreeView theTreeView = new TreeView();
        private static ISecurityDao secureDao;
        private static IInstanceConfigure instanceSrv;

        public static void TreeViewGenerateTreeView()
        {
            if (secureDao == null)
                secureDao = StaticMethod.GetService(typeof(ISecurityDao)) as ISecurityDao;
        }


        public static void TreeViewGenerateTreeView(TreeView treeView, long id)
        {

        }
        /// <summary>
        /// 根据模块Id,用户Id,角色产生树
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        public static void TreeViewGenerateTreeView(int moduleId, TreeView treeView, long id, long roleId)
        {

        }
        /// <summary>
        /// 根据Id,角色产生树
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        public static void TreeViewGenerateTreeView(TreeView treeView, string id, string roleId)
        {
            if (secureDao == null)
                secureDao = StaticMethod.GetService("SecurityDaoSrv") as ISecurityDao;
            ConstructMenu(treeView, roleId);
            if (instanceSrv == null)
                instanceSrv = StaticMethod.GetService("InstanceCfg") as IInstanceConfigure;
            instanceSrv.ShowRoleInsPermit("1", "3");
            //theTreeView = treeView;
            //InitEvent();
        }
        private static void InitEvent()
        {
            theTreeView.MouseDoubleClick += new MouseEventHandler(theTreeView_MouseDoubleClick);
        }

        static void theTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SysMenu aa = theTreeView.SelectedNode.Tag as SysMenu;
            if (theTreeView.SelectedNode.Nodes.Count == 0)
                MenuClick(aa.SysLink);
        }
        public static void TreeViewGenerateTreeView(TreeView treeView, long id, long roleId, string moduleName)
        {

        }
        /// <summary>
        /// 菜单点击定位
        /// </summary>
        /// <param name="invokeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object MenuClick(string invokeName, params object[] args)
        {
            if (invokeName == "" || invokeName == null)
                return null;
            string className = invokeName;
            className = invokeName;//"Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource.CMaterial(IFramework fra).Excute(int 3,int 2,string aaa)";


            string methodName = GetInvokeMethodName(className);

            Type objType = GetObjectType(className);
            //Type[] methodType = GetInvokeMethodType(className);
            int constructorNum = GetConstructParamsType(className);


            string methodParams = GetMethodParams(className);
            object[] objValue = GetInvokeMethodValue(methodParams);

            return Locate(objType, constructorNum, methodName, objValue);
        }
        /// <summary>
        /// 调用对象的方法
        /// </summary>
        /// <param name="classType">对象类型</param>
        /// <param name="constructNum">是否有构造参数</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="methodValue">方法值</param>
        /// <returns>调用方法后的返回值</returns>
        private static object Locate(Type classType, int constructNum, string methodName, object[] methodValue)
        {
            Type objType = classType;
            int constructorNum = constructNum;
            object obj = null;
            #region 创建实例
            if (constructorNum == 0)
                try
                {
                    obj = Activator.CreateInstance(objType);
                }
                catch (Exception ee)
                {
                    if (ee.Message.ToUpper() == "No parameterless constructor defined for this object.".ToUpper())
                        throw new Exception("没有默认构造函数！");
                    else
                        throw ee;
                }
            else
                obj = Activator.CreateInstance(objType, TheFramework);
            #endregion

            MethodInfo[] methodInfos = objType.GetMethods();

            object[] objects;
            objects = methodValue;
            foreach (MethodInfo var in methodInfos)
            {
                try
                {
                    if (var.Name == methodName)
                        return var.Invoke(obj, objects);
                }
                catch
                {
                }
            }
            throw new Exception("方法名称[" + methodName + "]不存在或参数错误，请检查参数类型！");

        }
        /// <summary>
        /// 获得方法参数
        /// </summary>
        /// <param name="invokeName">调用全称</param>
        /// <returns></returns>
        private static string GetMethodParams(string invokeName)
        {
            string[] classNameTmp = invokeName.Split('(');
            if (classNameTmp.GetLength(0) < 2)
            {
                throw new Exception("解析类方法错误，请确认是否定义了调用方法！");
            }
            string methodTypeTmp = classNameTmp[2].Trim().Substring(0, classNameTmp[2].Trim().Length - 1);
            return methodTypeTmp;
        }
        private static Type GetObjectType(string invokeName)
        {
            try
            {
                string classNameTmp = invokeName.Split('(')[0].ToString();
                Type typeReturn = Type.GetType(classNameTmp);
                return typeReturn;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(VirtualMachine.Component.Util.ExceptionUtil.ExceptionMessage(ee));
                return null;
            }
        }
        private static string GetInvokeMethodName(string invokeName)
        {
            string[] classNameTmp = invokeName.Split('(');
            if (invokeName.IndexOf('(') < 1)
            {
                throw new Exception("解析类方法错误，请确认是否定义了调用方法！");
            }
            if (classNameTmp.GetLength(0) < 2)
            {
                throw new Exception("解析类方法错误，请确认是否定义了调用方法！");
            }
            string methodNameTmp = classNameTmp[1].Trim().Split('.')[1];
            return methodNameTmp;
        }
        private static Type[] GetInvokeMethodType(string invokeName)
        {
            string[] classNameTmp = invokeName.Split('(');
            if (classNameTmp.GetLength(0) < 2)
            {
                throw new Exception("解析类方法错误，请确认是否定义了调用方法！");
            }
            string methodTypeTmp = classNameTmp[2].Trim().Substring(0, classNameTmp[2].Trim().Length - 1);
            string[] methodType1 = methodTypeTmp.Split(',');
            Type[] typeReturn = new Type[methodType1.GetLength(0)];
            for (int i = 0; i < methodType1.GetLength(0); i++)
            {
                if (methodType1[i].Trim().Substring(0, methodType1[i].Trim().IndexOf(' ')).ToUpper() == "long".ToUpper())
                    typeReturn.SetValue(typeof(long), i);
                else if (methodType1[i].Trim().Substring(0, methodType1[i].Trim().IndexOf(' ')).ToUpper() == "Int32".ToUpper())
                    typeReturn.SetValue(typeof(int), i);
                else if (methodType1[i].Trim().Substring(0, methodType1[i].Trim().IndexOf(' ')).ToUpper() == "Int16".ToUpper())
                    typeReturn.SetValue(typeof(int), i);
                else if (methodType1[i].Trim().Substring(0, methodType1[i].Trim().IndexOf(' ')).ToUpper() == "string".ToUpper())
                    typeReturn.SetValue(typeof(string), i);
                else if (methodType1[i].Trim().Substring(0, methodType1[i].Trim().IndexOf(' ')).ToUpper() == "DateTime".ToUpper())
                    typeReturn.SetValue(typeof(DateTime), i);
            }
            if (typeReturn.Length == 1)
            {
                if (typeReturn[0] == null)
                    typeReturn = new Type[] { };
            }
            return typeReturn;
        }
        private static object[] GetInvokeMethodValue(string paramsValue)
        {
            string[] methodType1 = paramsValue.Split(',');
            object[] typeTmp = new object[methodType1.GetLength(0)];
            object[] typeReturn = new object[1] { typeTmp };

            for (int i = 0; i < methodType1.GetLength(0); i++)
            {
                string[] aaa = methodType1[i].Trim().Split(' ');

                if (aaa[0].Trim().ToUpper() == "long".ToUpper())
                {
                    long lngTmp = 0L;
                    long.TryParse(aaa[1].Trim(), out lngTmp);
                    typeTmp.SetValue(lngTmp, i);
                }
                else if (aaa[0].Trim().ToUpper() == "Int32".ToUpper() || aaa[0].Trim().ToUpper() == "Int16".ToUpper() || aaa[0].Trim().ToUpper() == "Int".ToUpper())
                {
                    int intTmp = 0;
                    int.TryParse(aaa[1].Trim(), out intTmp);
                    typeTmp.SetValue(intTmp, i);
                }
                else if (aaa[0].Trim().ToUpper() == "string".ToUpper())
                {
                    typeTmp.SetValue(aaa[1], i);
                }
                else if (aaa[0].Trim().ToUpper() == "DateTime".ToUpper())
                {
                    DateTime datTmp = new DateTime();
                    DateTime.TryParse(aaa[1], out datTmp);
                    typeTmp.SetValue(datTmp, i);
                }
            }
            if (typeTmp.Length - 1 == 0)
            {
                typeTmp = new object[] { };
            }
            return typeReturn;
        }
        private static int GetConstructParamsType(string invokeName)
        {
            string paramsTmp = invokeName.Split('(')[1].Split(')')[0].ToString();
            string[] paramsType = paramsTmp.Split(' ');
            return paramsType.GetLength(0) - 1;
        }

        private static void ConstructMenu(TreeView treeView, string roleId)
        {
            IList lsMenus = secureDao.RtnAllMenusByRole(roleId);
            if (lsMenus.Count == 0) return;

            //添加的资源根菜单
            IList ls = secureDao.RtnSysMenuByFatherId("0");
            foreach (SysMenu menu in ls)
            {
                TreeNode subNode = new TreeNode();
                subNode.Name = menu.Id.ToString();
                subNode.Text = menu.Name;
                subNode.Tag = menu.Name;
                LoadSubTree(subNode, menu.Id, lsMenus);
                treeView.Nodes.Add(subNode);
            }
        }

        #region 形成树节点
        private static IList ShowAllChildByFather(string fatherId, IList lsMenus)
        {
            IList lsChild = new ArrayList();
            foreach (SysMenu menu in lsMenus)
            {
                if (menu.MenuFather.Id == fatherId)
                    lsChild.Add(menu);
            }
            return lsChild;
        }

        private static void LoadSubTree(TreeNode node, string upid, IList lsMenus)
        {
            IList ls = ShowAllChildByFather(upid, lsMenus);
            if (ls.Count == 0) return;
            foreach (SysMenu objs in ls)
            {
                TreeNode subnode = new TreeNode();
                subnode.Name = objs.Id.ToString();
                subnode.Text = objs.Name;
                subnode.Tag = objs.Name;
                //设置非末级节点的图片
                if (objs.MenuType != 0)
                {
                    subnode.ImageIndex = 1;
                    subnode.SelectedImageIndex = 1;
                }
                node.Nodes.Add(subnode);
                LoadSubTree(subnode, objs.Id, lsMenus);
            }
        }
        #endregion
    }
}
