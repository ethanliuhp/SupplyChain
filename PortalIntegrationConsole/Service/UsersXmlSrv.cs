using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.FinancialResource;
using System.Collections;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.BasicData.Domain;
using System.Data;
using System.Runtime.Remoting.Messaging;
using NHibernate;
using VirtualMachine.Core.DataAccess;
using Application.Resource.Util;

namespace PortalIntegrationConsole.Service
{
    public class OrgUsersXmlSrv : IOrgUsersXmlSrv
    {
        private static bool ifSync;
        /// <summary>
        /// 判断是否已经查询配置文件路径 避免重复查询
        /// </summary>
        private static bool hasGetConfigFile = false;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string ConfigPath;

        private string UsersDirectoryNodeStr = "UsersDirectorySerializer";
        private string UsersDirectoryDictionaryNodeStr = "DictionarySerializer";

        private static Object lockDirNode = new object();
        private static Object lockDirDicNode = new object();

        private IDao dao;

        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        static OrgUsersXmlSrv()
        {
            ConfigPath = System.Configuration.ConfigurationSettings.AppSettings["IRPAddress"];
        }

        #region 岗位操作

        /// <summary>
        /// 插入岗位(插入到配置文件中的Group中)
        /// </summary>
        /// <param name="operationJob">岗位</param>
        public void InsertOperationJobNode(OperationJob operationJob)
        {
            if (IfSync() == false) return;

            if (JobIsExists(operationJob.Code) == false)
                InsertOperationJob(operationJob);

            string dicName = GetDictionaryName(operationJob.Name, operationJob.OperationOrg);
            if (JobDictionaryIsExists(operationJob.Code))
            {
                ModifyDictionaryNode(operationJob.Code, dicName);
            }
            else
            {
                InsertDictionaryNode(operationJob.Code, dicName);
            }

            if (IsReLoadOrgUser()) ReloadIRPXml.ReloadUserXml();
        }

        private void InsertOperationJob(OperationJob operationJob)
        {
            lock (lockDirNode)
            {
                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                XmlNode theXmlNode = objXmlDoc.SelectSingleNode("UsersDirectorySerializer");
                XmlElement theXMlElement = objXmlDoc.CreateElement("Group");
                theXMlElement.SetAttribute("Name", operationJob.Code);
                theXMlElement.SetAttribute("EMail", "");
                theXMlElement.SetAttribute("GroupId", operationJob.Id);
                theXmlNode.AppendChild(theXMlElement);
                objXmlDoc.Save(strXmlFile);
            }
        }

        public bool JobIsExists(string jobCode)
        {
            lock (lockDirNode)
            {
                bool flag = false;

                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                XmlNode rootNode = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr);

                XmlNodeList groupLst = objXmlDoc.GetElementsByTagName("Group");
                foreach (XmlNode node in groupLst)
                {
                    XmlElement XmlElement = (XmlElement)node;
                    if (XmlElement.GetAttribute("Name") == jobCode)
                    {
                        flag = true;
                        break;
                    }
                }

                return flag;
            }
        }

        private bool JobDictionaryIsExists(string jobCode)
        {
            lock (lockDirDicNode)
            {
                bool flag = false;

                string Node = "DictionarySerializer";
                string strXmlFileDictionary = ConfigPath + "\\Config\\Custom\\UsersDirectoryDictionary.xml";
                XmlDocument objXmlDocDictionary = new XmlDocument();
                objXmlDocDictionary.Load(strXmlFileDictionary);

                XmlNodeList HeaderXmlNodeList = objXmlDocDictionary.SelectSingleNode(Node).FirstChild.ChildNodes;
                foreach (XmlNode theXmlNode in HeaderXmlNodeList)
                {
                    XmlElement theElement = (XmlElement)theXmlNode;
                    if (theElement.GetAttribute("Name").ToString() == jobCode)
                    {
                        flag = true;
                        break;
                    }
                }

                return flag;
            }
        }

        /// <summary>
        /// 修改岗位节点
        /// </summary>
        /// <param name="operationJob"></param>
        public void ModifyOperationJobNode(OperationJob operationJob)
        {
            if (IfSync() == false) return;

            if (JobIsExists(operationJob.Code) == false)
            {
                InsertOperationJob(operationJob);
            }

            string dicName = GetDictionaryName(operationJob.Name, operationJob.OperationOrg);
            if (JobDictionaryIsExists(operationJob.Code) == false)
            {
                InsertDictionaryNode(operationJob.Code, dicName);
            }
            else
            {
                ModifyDictionaryNode(operationJob.Code, dicName);
            }

            if (IsReLoadOrgUser())
                ReloadIRPXml.ReloadUserXml();
        }

        private string GetDictionaryName(string jobName, OperationOrg operationOrgOfJob)
        {
            if (string.IsNullOrEmpty(operationOrgOfJob.OperationType))
            {
                return operationOrgOfJob.Name + "_" + jobName;
            }

            if (IsMatch(operationOrgOfJob))
            {
                return operationOrgOfJob.Name + "_" + jobName;
            }

            string[] parentIds = operationOrgOfJob.SysCode.Split('.');
            if (parentIds == null || parentIds.Length == 0 || parentIds.Length == 1 || parentIds.Length == 2 || parentIds.Length == 3)
            {
                return operationOrgOfJob.Name + "_" + jobName;
            }

            string tempName = "";
            for (int j = parentIds.Length - 2; j >= 0; j--)
            {
                string parentId = parentIds[j];
                OperationOrg org = GetOrgById(parentId);
                if (IsMatch(org))
                {
                    tempName = org.Name + "_" + tempName + jobName;
                    return tempName;
                }
                else
                {
                    tempName = org.Name + "_" + tempName;
                }
            }
            return tempName + "_" + jobName;
        }

        private OperationOrg GetOrgById(string orgId)
        {
            ObjectQuery oq = new ObjectQuery();
            IList operationOrgLst = Dao.ObjectQuery(typeof(OperationOrg), oq);

            foreach (OperationOrg org in operationOrgLst)
            {
                if (org.Id == orgId)
                {
                    return org;
                }
            }
            return null;
        }

        private bool IsMatch(OperationOrg operationOrg)
        {
            if (operationOrg != null && !string.IsNullOrEmpty(operationOrg.OperationType) 
                && (operationOrg.OperationType.Equals("fgsxmb", StringComparison.OrdinalIgnoreCase)
                    || operationOrg.OperationType.Equals("zgxmb", StringComparison.OrdinalIgnoreCase)
                    || operationOrg.OperationType.Equals("h", StringComparison.OrdinalIgnoreCase)
                    || operationOrg.OperationType.Equals("b", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// //删除岗位节点，首先判断当前组织是否被引用，如果引用需删除此用户的岗位信息
        /// </summary>
        /// <param name="operationJob"></param>
        public void DeleteOperationJobNode(OperationJob operationJob)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;
                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                XmlNode rootNode = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr);

                bool flag = false;

                XmlNodeList groupLst = objXmlDoc.GetElementsByTagName("Group");
                foreach (XmlNode node in groupLst)
                {
                    XmlElement XmlElement = (XmlElement)node;
                    if (XmlElement.GetAttribute("Name") == operationJob.Code)
                    {
                        rootNode.RemoveChild(node);
                        flag = true;
                        break;
                    }
                }

                XmlNodeList groupRefLst = objXmlDoc.GetElementsByTagName("GroupRef");
                for (var i = groupRefLst.Count - 1; i > -1; i--)
                {
                    XmlNode node = groupRefLst[i];
                    XmlElement XmlElement = (XmlElement)node;
                    XmlNode parentNode = node.ParentNode;
                    if (XmlElement.GetAttribute("Name") == operationJob.Code)
                    {
                        parentNode.RemoveChild(node);
                        flag = true;
                    }
                }

                if (flag)
                {
                    objXmlDoc.Save(strXmlFile);

                    this.DeleteDictionaryNode(operationJob.Code);

                    if (IsReLoadOrgUser())
                        ReloadIRPXml.ReloadUserXml();
                }
            }
        }
        #endregion

        #region 角色操作

        /// <summary>
        /// 角色是否存在
        /// </summary>
        /// <param name="jobCode">角色名称</param>
        /// <returns></returns>
        public bool RoleIsExists(string roleName)
        {
            lock (lockDirNode)
            {
                bool flag = false;

                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                XmlNode rootNode = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr);

                XmlNodeList roleLst = objXmlDoc.GetElementsByTagName("Role");
                foreach (XmlNode node in roleLst)
                {
                    XmlElement XmlElement = (XmlElement)node;
                    if (XmlElement.GetAttribute("Name") == roleName)
                    {
                        flag = true;
                        break;
                    }
                }

                return flag;
            }
        }

        /// <summary>
        /// 插入角色节点
        /// </summary>
        /// <param name="operationRole">角色</param>
        public void InsertRoleNode(OperationRole operationRole)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;

                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                bool hasFlag = false;

                XmlNodeList theXmlNodeList = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr).ChildNodes;
                foreach (XmlNode theXmlNode in theXmlNodeList)
                {
                    XmlElement theXmlElement = (XmlElement)theXmlNode;
                    if (theXmlElement.GetAttribute("Name").ToString() == operationRole.RoleName)
                    {
                        hasFlag = true;
                        break;
                    }
                }

                if (hasFlag == false)
                {
                    XmlNode theXmlNode = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr);
                    XmlElement theXMlElement = objXmlDoc.CreateElement("Role");

                    theXMlElement.SetAttribute("Name", operationRole.RoleName);
                    theXMlElement.SetAttribute("EMail", "");
                    theXMlElement.SetAttribute("RoleId", operationRole.Id);
                    theXmlNode.AppendChild(theXMlElement);
                    objXmlDoc.Save(strXmlFile);

                    this.InsertDictionaryNode(operationRole.RoleName, operationRole.RoleName);

                    if (IsReLoadOrgUser())
                        ReloadIRPXml.ReloadUserXml();
                }
            }
        }

        /// <summary>
        /// 删除角色节点，首先判断当前角色是否被引用，如果引用需删除此用户的角色信息
        /// </summary>
        /// <param name="operationRole"></param>
        public void DeleteRoleNode(OperationRole operationRole)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;
                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                string RoleName = operationRole.RoleName;
                bool flag = false;
                XmlNodeList theXmlNodList = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr).ChildNodes;
                foreach (XmlNode XmlNode in theXmlNodList)
                {
                    XmlElement XmlElement = (XmlElement)XmlNode;
                    if (XmlElement.Name == "User")
                    {
                        XmlNodeList GroupXmlNodeList = XmlElement.ChildNodes;
                        foreach (XmlNode GroupXmlNode in GroupXmlNodeList)
                        {
                            XmlElement GroupXmlElement = (XmlElement)GroupXmlNode;
                            XmlNodeList RoleXmlNodeList = GroupXmlElement.ChildNodes;

                            for (var i = RoleXmlNodeList.Count - 1; i > -1; i--)
                            {
                                XmlNode RoleXmlNode = RoleXmlNodeList[i];
                                XmlElement RoleXmlElement = (XmlElement)RoleXmlNode;
                                if (RoleXmlElement.GetAttribute("Name").ToString() == RoleName)
                                {
                                    flag = true;
                                    GroupXmlElement.RemoveChild(RoleXmlNode);
                                }
                            }
                        }
                    }
                }

                if (flag)
                    objXmlDoc.Save(strXmlFile);


                this.Delete("UsersDirectorySerializer/" + "Role" + "[@Name='" + RoleName + "']", RoleName);
                this.DeleteDictionaryNode(RoleName);

                if (IsReLoadOrgUser())
                    ReloadIRPXml.ReloadUserXml();
            }
        }

        #endregion

        #region 用户操作
        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="userCode">用户代码</param>
        /// <returns></returns>
        public bool UserIsExists(string userCode)
        {
            lock (lockDirNode)
            {
                bool flag = false;

                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                XmlNode rootNode = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr);

                XmlNodeList userLst = objXmlDoc.GetElementsByTagName("User");
                foreach (XmlNode node in userLst)
                {
                    XmlElement XmlElement = (XmlElement)node;
                    if (XmlElement.GetAttribute("Name") == userCode)
                    {
                        flag = true;
                        break;
                    }
                }

                return flag;
            }
        }

        /// <summary>
        /// 插入用户节点
        /// </summary>
        /// <param name="theUser"></param>
        public void InsertUserNode(StandardPerson theUser)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;

                if (UserIsExists(theUser.Code))
                    return;

                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                XmlNode UserXmlNode = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr);
                XmlElement theXmlElement = objXmlDoc.CreateElement("User");

                theXmlElement.SetAttribute("Name", theUser.Code);
                theXmlElement.SetAttribute("Password", theUser.Password);
                if (theUser.Sex == 0)
                {
                    theXmlElement.SetAttribute("Gender", "male");
                }
                else if (theUser.Sex == 1)
                {
                    theXmlElement.SetAttribute("Gender", "female");
                }
                theXmlElement.SetAttribute("Department", "");//部门
                theXmlElement.SetAttribute("AcademicRecord", theUser.PartLv.ToString());//学历    
                theXmlElement.SetAttribute("Language", "zhs");
                theXmlElement.SetAttribute("TimeZone", "+08:00");
                theXmlElement.SetAttribute("ExpireDate", "31/12/2099 00:00:00");
                theXmlElement.SetAttribute("PasswordExpireDate", "31/12/2099 00:00:00");
                theXmlElement.SetAttribute("Authentication", "Internal");
                theXmlElement.SetAttribute("Timeout", "30");
                theXmlElement.SetAttribute("Location", "wuhan");
                theXmlElement.SetAttribute("Local", "c:\\temp\\VOB");
                theXmlElement.SetAttribute("PrivateWorkspace", "c:\\temp\\PWS");
                theXmlElement.SetAttribute("UserId", theUser.Id);

                foreach (ContactMethod theContactMethod in theUser.ConnMethod)
                {
                    theXmlElement.SetAttribute("EMail", theContactMethod.Email);
                    theXmlElement.SetAttribute("Phone", theContactMethod.Mobile);
                    theXmlElement.SetAttribute("Address", theContactMethod.Address);
                    theXmlElement.SetAttribute("ZipCode", theContactMethod.Fax);
                }

                //XmlElement theGroupXmlElement = objXmlDoc.CreateElement("GroupRef");
                //theGroupXmlElement.SetAttribute("Name", "COMPANY");

                //XmlElement theRoleXmlElement = objXmlDoc.CreateElement("RoleRef");
                //theRoleXmlElement.SetAttribute("Name", "USER");
                //theGroupXmlElement.AppendChild(theRoleXmlElement);
                //theXmlElement.AppendChild(theGroupXmlElement);

                //添加用户节点
                UserXmlNode.AppendChild(theXmlElement);

                objXmlDoc.Save(strXmlFile);
                this.InsertDictionaryNode(theUser.Code, theUser.Name);
                if (IsReLoadOrgUser()) ReloadIRPXml.ReloadUserXml();
            }
        }

        /// <summary>
        /// 修改用户节点
        /// </summary>
        /// <param name="theUser"></param>
        public void ModifyUserNode(StandardPerson theUser)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;
                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                bool flag = false;

                XmlNodeList theXmlNodeList = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr).ChildNodes;
                foreach (XmlNode theXmlNode in theXmlNodeList)
                {
                    XmlElement theXmlElement = (XmlElement)theXmlNode;
                    if (theXmlElement.GetAttribute("Name").ToString() == theUser.Code)
                    {
                        theXmlElement.SetAttribute("Name", theUser.Code);
                        theXmlElement.SetAttribute("Password", theUser.Password);
                        if (theUser.Sex == 0)
                        {
                            theXmlElement.SetAttribute("Gender", "male");
                        }
                        else if (theUser.Sex == 1)
                        {
                            theXmlElement.SetAttribute("Gender", "female");
                        }
                        theXmlElement.SetAttribute("Department", "");//部门
                        theXmlElement.SetAttribute("AcademicRecord", theUser.PartLv.ToString());//学历    
                        theXmlElement.SetAttribute("Language", "zhs");
                        theXmlElement.SetAttribute("TimeZone", "+08:00");
                        theXmlElement.SetAttribute("ExpireDate", "31/12/2099 00:00:00");
                        theXmlElement.SetAttribute("PasswordExpireDate", "31/12/2099 00:00:00");
                        theXmlElement.SetAttribute("Authentication", "Internal");
                        theXmlElement.SetAttribute("Timeout", "30");
                        theXmlElement.SetAttribute("Location", "wuhan");
                        theXmlElement.SetAttribute("Local", "c:\\temp\\VOB");
                        theXmlElement.SetAttribute("PrivateWorkspace", "c:\\temp\\PWS");

                        foreach (ContactMethod theContactMethod in theUser.ConnMethod)
                        {
                            theXmlElement.SetAttribute("EMail", theContactMethod.Email);
                            theXmlElement.SetAttribute("Phone", theContactMethod.Mobile);
                            theXmlElement.SetAttribute("Address", theContactMethod.Address);
                            theXmlElement.SetAttribute("ZipCode", theContactMethod.Fax);
                        }
                        theXmlElement.SetAttribute("UserId", theUser.Id);

                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                {
                    InsertUserNode(theUser);
                }
                else
                {
                    objXmlDoc.Save(strXmlFile);

                    this.ModifyDictionaryNode(theUser.Code, theUser.Name);

                    if (IsReLoadOrgUser()) ReloadIRPXml.ReloadUserXml();
                }
            }
        }

        /// <summary>
        /// 删除用户节点
        /// </summary>
        /// <param name="theUser"></param>
        public void DeleteUserNode(StandardPerson theUser)
        {
            this.Delete("UsersDirectorySerializer/" + "User" + "[@Name='" + theUser.Code + "']", theUser.Code);
            this.DeleteDictionaryNode(theUser.Code);
            if (IsReLoadOrgUser()) ReloadIRPXml.ReloadUserXml();
        }
        #endregion

        //删除节点
        private void Delete(string Node, string NodeName)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;
                int count = 0;
                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                string mainCode = Node.Substring(0, Node.LastIndexOf("/"));
                XmlNodeList theXmlNodeList = objXmlDoc.SelectSingleNode(mainCode).ChildNodes;
                foreach (XmlNode theXmlNode in theXmlNodeList)
                {
                    XmlElement XmlElement = (XmlElement)theXmlNode;
                    if (XmlElement.GetAttribute("Name") == NodeName)
                    {
                        count++;
                        break;
                    }
                }
                if (count > 0)
                {
                    objXmlDoc.SelectSingleNode(mainCode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
                    objXmlDoc.Save(strXmlFile);
                }
            }
        }


        #region 人员上岗维护对应的IRP用户的组织和角色的信息的增删
        /// <summary>
        /// 人员上岗
        /// </summary>
        /// <param name="perOnJob"></param>
        public void AddJobAndRole(PersonOnJob perOnJob)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;
                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                if (perOnJob != null)
                {
                    string UserName = perOnJob.StandardPerson.Code;
                    string jobName = perOnJob.OperationJob.Code;
                    //string RoleName = perOnJob.OperationJob.Code;
                    bool hasJob = false;

                    XmlNodeList theXmlNodeList = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr).ChildNodes;
                    foreach (XmlNode theXmlNode in theXmlNodeList)
                    {
                        XmlElement theXmlElement = (XmlElement)theXmlNode;
                        if (theXmlElement.GetAttribute("Name").ToString() == UserName)
                        {
                            XmlNodeList GroupXmlNodeList = theXmlElement.ChildNodes;
                            foreach (XmlNode GroupXmlNode in GroupXmlNodeList)
                            {
                                XmlElement GroupXmlElement = (XmlElement)GroupXmlNode;
                                if (GroupXmlElement.GetAttribute("Name").ToString() == jobName)
                                {
                                    hasJob = true;
                                    //如果该用户已经存在该岗位，先删除岗位下的所有角色
                                    for (int g = GroupXmlElement.ChildNodes.Count - 1; g > -1; g--)
                                    {
                                        XmlNode childNode = GroupXmlElement.ChildNodes[g];
                                        GroupXmlElement.RemoveChild(childNode);
                                    }
                                    //添加角色
                                    AddRole(objXmlDoc, theXmlElement, GroupXmlElement, perOnJob.OperationJob);
                                    break;
                                }
                            }

                            if (hasJob == false && JobIsExists(jobName))
                            {
                                hasJob = true;

                                //如果当前用户没有当前的岗位
                                XmlElement theGroupXmlElement = objXmlDoc.CreateElement("GroupRef");
                                theGroupXmlElement.SetAttribute("Name", jobName);
                                //添加角色
                                AddRole(objXmlDoc, theXmlElement, theGroupXmlElement, perOnJob.OperationJob);
                            }
                            break;
                        }
                    }

                    if (hasJob)
                    {
                        objXmlDoc.Save(strXmlFile);
                        if (IsReLoadOrgUser()) ReloadIRPXml.ReloadUserXml();
                    }
                }
            }
        }

        /// <summary>
        /// 根据岗位添加角色
        /// </summary>
        /// <param name="objXmlDoc"></param>
        /// <param name="userXmlElement"></param>
        /// <param name="groupXmlElement"></param>
        /// <param name="operationJob"></param>
        private void AddRole(XmlDocument objXmlDoc, XmlElement userXmlElement, XmlElement groupXmlElement, OperationJob operationJob)
        {
            List<OperationRole> roleLst = GetOperationRoleByJobId(operationJob.Id);
            foreach (OperationRole role in roleLst)
            {
                if (RoleIsExists(role.RoleName))
                {
                    XmlElement RoleXmlElement = objXmlDoc.CreateElement("RoleRef");
                    RoleXmlElement.SetAttribute("Name", role.RoleName);
                    groupXmlElement.AppendChild(RoleXmlElement);
                }
            }
            userXmlElement.AppendChild(groupXmlElement);
        }

        /// <summary>
        /// 修改用户的岗位
        /// </summary>
        /// <param name="oldUser"></param>
        /// <param name="personJob"></param>
        public void ModifyUserJob(StandardPerson oldUser, PersonOnJob personJob)
        {
            if (IfSync() == false) return;
            //删除旧用户的岗位
            DeleteUserJob(oldUser, personJob.OperationJob);
            //给用户增加岗位
            AddJobAndRole(personJob);
        }

        /// <summary>
        /// 删除用户的岗位
        /// </summary>
        /// <param name="user"></param>
        /// <param name="operationJob"></param>
        public void DeleteUserJob(StandardPerson user, OperationJob operationJob)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;
                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                XmlNodeList theXmlNodList = objXmlDoc.SelectSingleNode(this.UsersDirectoryNodeStr).ChildNodes;
                foreach (XmlNode theXmlNode in theXmlNodList)
                {
                    XmlElement theXmlElement = (XmlElement)theXmlNode;
                    if (theXmlElement.GetAttribute("Name").ToString() == user.Code)
                    {
                        XmlNodeList GroupXmlNodeList = theXmlElement.ChildNodes;//岗位 集合
                        foreach (XmlNode theGroupXmlNode in GroupXmlNodeList)
                        {
                            XmlElement GroupXmlElment = (XmlElement)theGroupXmlNode;
                            if (GroupXmlElment.GetAttribute("Name").ToString() == operationJob.Code)
                            {
                                //岗位存在 删除岗位
                                theXmlElement.RemoveChild(theGroupXmlNode);
                                objXmlDoc.Save(strXmlFile);
                                if (IsReLoadOrgUser()) ReloadIRPXml.ReloadUserXml();
                                break;
                            }
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 人员上岗
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="listJob"></param>
        public void PersonOnJob(string userCode, List<OperationJob> listJob)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;

                if (string.IsNullOrEmpty(userCode))
                    return;

                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);


                XmlNodeList theXmlNodeList = objXmlDoc.GetElementsByTagName("User");
                foreach (XmlNode theXmlNode in theXmlNodeList)
                {
                    XmlElement theXmlElement = (XmlElement)theXmlNode;
                    if (theXmlElement.GetAttribute("Name").ToString() == userCode)
                    {
                        XmlNodeList GroupXmlNodeList = theXmlElement.ChildNodes;
                        foreach (OperationJob job in listJob)
                        {
                            string jobCode = job.Code;
                            bool hasJob = false;
                            foreach (XmlNode GroupXmlNode in GroupXmlNodeList)
                            {
                                XmlElement GroupXmlElement = (XmlElement)GroupXmlNode;
                                if (GroupXmlElement.GetAttribute("Name").ToString() == jobCode)
                                {
                                    hasJob = true;
                                    //如果该用户已经存在该岗位，先删除岗位下的所有角色
                                    for (int g = GroupXmlElement.ChildNodes.Count - 1; g > -1; g--)
                                    {
                                        XmlNode childNode = GroupXmlElement.ChildNodes[g];
                                        GroupXmlElement.RemoveChild(childNode);
                                    }
                                    //添加角色
                                    AddRole(objXmlDoc, theXmlElement, GroupXmlElement, job);
                                    break;
                                }
                            }

                            if (hasJob == false && JobIsExists(jobCode))
                            {
                                hasJob = true;

                                //如果当前用户没有当前的岗位
                                XmlElement theGroupXmlElement = objXmlDoc.CreateElement("GroupRef");
                                theGroupXmlElement.SetAttribute("Name", jobCode);
                                //添加角色
                                AddRole(objXmlDoc, theXmlElement, theGroupXmlElement, job);
                            }
                        }
                        break;
                    }
                }

                objXmlDoc.Save(strXmlFile);
            }
        }

        #endregion

        #region 字典文件的操作
        /// <summary>
        /// 删除字典文件节点
        /// </summary>
        /// <param name="CurrentNodeName">节点名称(Code)</param>
        private void DeleteDictionaryNode(string CurrentNodeName)
        {
            lock (lockDirDicNode)
            {
                if (IfSync() == false) return;
                string Node = "DictionarySerializer";
                string strXmlFileDictionary = ConfigPath + "\\Config\\Custom\\UsersDirectoryDictionary.xml";
                XmlDocument objXmlDocDictionary = new XmlDocument();
                objXmlDocDictionary.Load(strXmlFileDictionary);

                bool flag = false;

                //头部字典文件节点删除
                XmlNodeList HeaderXmlNodeList = objXmlDocDictionary.SelectSingleNode(Node).FirstChild.ChildNodes;
                for (int i = HeaderXmlNodeList.Count - 1; i > -1; i--)
                {
                    XmlNode theXmlNode = HeaderXmlNodeList[i];
                    XmlElement theElement = (XmlElement)theXmlNode;
                    if (theElement.GetAttribute("Name").ToString() == CurrentNodeName)
                    {
                        objXmlDocDictionary.SelectSingleNode(Node).FirstChild.RemoveChild(theXmlNode);
                        flag = true;
                    }
                }

                //标题部分字典文件节点删除
                XmlNodeList CaptionXmlNodeList = objXmlDocDictionary.SelectSingleNode(Node).LastChild.ChildNodes;
                for (int i = CaptionXmlNodeList.Count - 1; i > -1; i--)
                {
                    XmlNode theXmlNode = CaptionXmlNodeList[i];
                    XmlElement theElement = (XmlElement)theXmlNode;
                    if (theElement.GetAttribute("Name").ToString() == CurrentNodeName)
                    {
                        objXmlDocDictionary.SelectSingleNode(Node).LastChild.RemoveChild(theXmlNode);
                        flag = true;
                    }
                }

                if (flag)
                    objXmlDocDictionary.Save(strXmlFileDictionary);
            }

        }

        /// <summary>
        /// 插入字典文件节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="nodeZhsName"></param>
        private void InsertDictionaryNode(string nodeName, string nodeZhsName)
        {
            lock (lockDirDicNode)
            {
                if (IfSync() == false) return;
                string Node = "DictionarySerializer";
                string strXmlFileDictionary = ConfigPath + "\\Config\\Custom\\UsersDirectoryDictionary.xml";
                XmlDocument objXmlDocDictionary = new XmlDocument();
                objXmlDocDictionary.Load(strXmlFileDictionary);

                //头部根节点
                XmlNode HeaderXmlNode = objXmlDocDictionary.SelectSingleNode(Node).FirstChild;
                XmlElement HeaderXmlElement = objXmlDocDictionary.CreateElement("Label");
                HeaderXmlElement.SetAttribute("Name", nodeName);

                XmlElement HeaderEnXmlElement = objXmlDocDictionary.CreateElement("LocalizedLabel");
                HeaderEnXmlElement.SetAttribute("LanguageRef", "en");
                HeaderEnXmlElement.InnerText = nodeName;
                HeaderXmlElement.AppendChild(HeaderEnXmlElement);

                XmlElement HeaderItXmlElement = objXmlDocDictionary.CreateElement("LocalizedLabel");
                HeaderItXmlElement.SetAttribute("LanguageRef", "it");
                HeaderItXmlElement.InnerText = nodeName;
                HeaderXmlElement.AppendChild(HeaderItXmlElement);

                XmlElement HeaderZhsXmlElement = objXmlDocDictionary.CreateElement("LocalizedLabel");
                HeaderZhsXmlElement.SetAttribute("LanguageRef", "zhs");
                HeaderZhsXmlElement.InnerText = nodeZhsName;
                HeaderXmlElement.AppendChild(HeaderZhsXmlElement);
                HeaderXmlNode.AppendChild(HeaderXmlElement);

                //标题根节点
                XmlNode CaptionXmlNode = objXmlDocDictionary.SelectSingleNode(Node).LastChild;
                XmlElement CaptionXmlElement = objXmlDocDictionary.CreateElement("Label");
                CaptionXmlElement.SetAttribute("Name", nodeName);

                XmlElement CaptionEnXmlElement = objXmlDocDictionary.CreateElement("LocalizedLabel");
                CaptionEnXmlElement.SetAttribute("LanguageRef", "en");
                CaptionEnXmlElement.InnerText = nodeName;
                CaptionXmlElement.AppendChild(CaptionEnXmlElement);

                XmlElement CaptionItXmlElement = objXmlDocDictionary.CreateElement("LocalizedLabel");
                CaptionItXmlElement.SetAttribute("LanguageRef", "it");
                CaptionItXmlElement.InnerText = nodeName;
                CaptionXmlElement.AppendChild(CaptionItXmlElement);

                XmlElement CaptionZhsXmlElement = objXmlDocDictionary.CreateElement("LocalizedLabel");
                CaptionZhsXmlElement.SetAttribute("LanguageRef", "zhs");
                CaptionZhsXmlElement.InnerText = nodeZhsName;
                CaptionXmlElement.AppendChild(CaptionZhsXmlElement);
                CaptionXmlNode.AppendChild(CaptionXmlElement);

                objXmlDocDictionary.Save(strXmlFileDictionary);
            }
        }

        /// <summary>
        /// 修改字典文件节点
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeZhsName">节点中文名称</param>
        private void ModifyDictionaryNode(string nodeName, string nodeZhsName)
        {
            lock (lockDirDicNode)
            {
                if (IfSync() == false) return;
                string Node = "DictionarySerializer";
                string strXmlFileDictionary = ConfigPath + "\\Config\\Custom\\UsersDirectoryDictionary.xml";
                XmlDocument objXmlDocDictionary = new XmlDocument();
                objXmlDocDictionary.Load(strXmlFileDictionary);

                bool flag = false;
                //更新头部
                XmlNodeList HeaderXmlNodeList = objXmlDocDictionary.SelectSingleNode(Node).FirstChild.ChildNodes;
                foreach (XmlNode theXmlNode in HeaderXmlNodeList)
                {
                    XmlElement theElement = (XmlElement)theXmlNode;
                    if (theElement.GetAttribute("Name").ToString() == nodeName)
                    {
                        XmlNodeList XmlNodeList = theElement.ChildNodes;
                        foreach (XmlNode XmlNode in XmlNodeList)
                        {
                            string OuterXml = XmlNode.OuterXml;
                            XmlElement XmlElement = (XmlElement)XmlNode;
                            if (OuterXml.Contains("zhs"))
                            {
                                XmlElement.InnerText = nodeZhsName;
                            }
                        }
                        flag = true;
                        break;
                    }
                }
                //更新标题
                XmlNodeList CaptionXmlNodeList = objXmlDocDictionary.SelectSingleNode(Node).LastChild.ChildNodes;
                foreach (XmlNode theXmlNode in CaptionXmlNodeList)
                {
                    XmlElement theElement = (XmlElement)theXmlNode;
                    if (theElement.GetAttribute("Name").ToString() == nodeName)
                    {
                        XmlNodeList XmlNodeList = theElement.ChildNodes;
                        foreach (XmlNode XmlNode in XmlNodeList)
                        {
                            string OuterXml = XmlNode.OuterXml;
                            XmlElement XmlElement = (XmlElement)XmlNode;
                            if (OuterXml.Contains("zhs"))
                            {
                                XmlElement.InnerText = nodeZhsName;
                            }
                        }
                        flag = true;
                        break;
                    }
                }

                if (flag)
                    objXmlDocDictionary.Save(strXmlFileDictionary);
            }
        }
        #endregion

        #region 岗位关联角色操作
        /// <summary>
        /// 岗位关联角色
        /// </summary>
        /// <param name="operationJob"></param>
        public void JobLinkRole(OperationJob operationJob)
        {
            lock (lockDirNode)
            {
                if (IfSync() == false) return;
                string strXmlFile = ConfigPath + "\\Config\\Custom\\UsersDirectory.xml";
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(strXmlFile);

                bool userHasJob = false;

                XmlNodeList nodeList = objXmlDoc.GetElementsByTagName("GroupRef");
                foreach (XmlNode groupXmlNode in nodeList)
                {
                    XmlElement xmlElement = (XmlElement)groupXmlNode;
                    if (xmlElement.Attributes["Name"].Value == operationJob.Code)
                    {
                        userHasJob = true;
                        XmlNode userXmlNode = groupXmlNode.ParentNode;

                        //先删除这个岗位下的角色
                        for (int g = groupXmlNode.ChildNodes.Count - 1; g > -1; g--)
                        {
                            XmlNode roleXmlNode = groupXmlNode.ChildNodes[g];
                            groupXmlNode.RemoveChild(roleXmlNode);
                        }
                        //是需要修改的岗位节点
                        if (operationJob.JobWithRole.Count > 0)
                        {
                            //添加角色
                            foreach (OperationJobWithRole obj in operationJob.JobWithRole)
                            {
                                if (RoleIsExists(obj.OperationRole.RoleName))
                                {
                                    XmlElement RoleXmlElement = objXmlDoc.CreateElement("RoleRef");
                                    RoleXmlElement.SetAttribute("Name", obj.OperationRole.RoleName);
                                    groupXmlNode.AppendChild(RoleXmlElement);
                                }
                            }
                            //userXmlNode.AppendChild(groupXmlNode);
                        }
                    }
                }
                if (userHasJob)
                {
                    objXmlDoc.Save(strXmlFile);
                    if (IsReLoadOrgUser())
                        ReloadIRPXml.ReloadUserXml();
                }
            }
        }
        #endregion

        /// <summary>
        /// 查询岗位关联的角色
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        private List<OperationRole> GetOperationRoleByJobId(string jobId)
        {
            List<OperationRole> retLst = new List<OperationRole>();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("OperationJob.Id", jobId));
            oq.AddFetchMode("OperationRole", FetchMode.Eager);
            IList lst = Dao.ObjectQuery(typeof(OperationJobWithRole), oq);
            if (lst != null && lst.Count > 0)
            {
                foreach (OperationJobWithRole obj in lst)
                {
                    retLst.Add(obj.OperationRole);
                }
            }
            return retLst;
        }

        /// <summary>
        /// 判断是否需要同步 true同步
        /// </summary>
        /// <returns></returns>
        private bool IfSync()
        {
            return true;
        }

        /// <summary>
        /// 是否需要重新加载组织用户（xml到内存）
        /// </summary>
        /// <returns></returns>
        private bool IsReLoadOrgUser()
        {
            return false;
        }
    }
}