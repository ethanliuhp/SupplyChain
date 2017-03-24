using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using System.Xml;
using VirtualMachine.Core;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;

namespace Application.Business.Erp.SupplyChain.Util
{
    public class UpdateIRPXml
    {
        private static string strXmlFile = @"F:\IRP6\ubsww\Config\Custom\UsersDirectory.xml";
        private static string strXmlFileDictionary = @"F:\IRP6\ubsww\Config\Custom\UsersDirectoryDictionary.xml";
        
        public static void UpdateJob(IStockInSrv stockInSrv)
        {
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Like("Code", "P101000600060018", MatchMode.Start));
            oq.AddFetchMode("OperationOrg", NHibernate.FetchMode.Eager);
            IList lst = stockInSrv.GetObjects(typeof(OperationJob), oq);

            ObjectQuery opgOq = new ObjectQuery();
            IList operationOrgLst = stockInSrv.GetObjects(typeof(OperationOrg), opgOq);

            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(strXmlFile);

            XmlDocument objXmlDocDictionary = new XmlDocument();
            objXmlDocDictionary.Load(strXmlFileDictionary);

            XmlNode theXmlNode = objXmlDoc.SelectSingleNode("UsersDirectorySerializer");
            foreach (OperationJob job in lst)
            {
                XmlElement theXMlElement = objXmlDoc.CreateElement("Group");
                theXMlElement.SetAttribute("Name", job.Code);
                theXMlElement.SetAttribute("EMail", "");
                theXMlElement.SetAttribute("GroupId", job.Id);
                theXmlNode.AppendChild(theXMlElement);
                string dictionaryName = GetDictionaryName(job.Name, job.OperationOrg, operationOrgLst);
                UpdateDictionary(objXmlDocDictionary, job.Code, dictionaryName);
            }
            objXmlDoc.Save(strXmlFile);
            objXmlDocDictionary.Save(strXmlFileDictionary);
        }

        private static string GetDictionaryName(string jobName, OperationOrg operationOrgOfJob, IList operationOrgLst)
        {
            if(string.IsNullOrEmpty(operationOrgOfJob.OperationType))
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
            for (int j = parentIds.Length-2; j >= 0; j--)
            {
                string parentId = parentIds[j];
                OperationOrg org = GetOrgById(parentId, operationOrgLst);
                if (IsMatch(org))
                {
                    tempName =org.Name + "_"+ tempName + jobName;
                    return tempName;
                }
                else
                {
                    tempName = org.Name + "_" + tempName;
                }
            }
            return tempName+"_"+jobName;
        }

        private static OperationOrg GetOrgById(string orgId,IList operationOrgLst)
        {
            foreach (OperationOrg org in operationOrgLst)
            {
                if (org.Id == orgId)
                {
                    return org;
                }
            }
            return null;
        }

        private static bool IsMatch(OperationOrg operationOrg)
        {
            if (operationOrg.OperationType.Equals("fgsxmb", StringComparison.OrdinalIgnoreCase)
                    || operationOrg.OperationType.Equals("zgxmb", StringComparison.OrdinalIgnoreCase)
                    || operationOrg.OperationType.Equals("h", StringComparison.OrdinalIgnoreCase)
                    || operationOrg.OperationType.Equals("b", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private static void UpdateDictionary(XmlDocument objXmlDocDictionary,String nodeName,string nodeZhsName)
        {
            //头部根节点
            XmlNode HeaderXmlNode = objXmlDocDictionary.SelectSingleNode("DictionarySerializer").FirstChild;
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
            XmlNode CaptionXmlNode = objXmlDocDictionary.SelectSingleNode("DictionarySerializer").LastChild;
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
        }

        public static void UpdateRole(IStockInSrv stockInSrv)
        {
            ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Like("Code", "P101000600060018", MatchMode.Start));
            //oq.AddFetchMode("OperationRole", NHibernate.FetchMode.Eager);
            IList lst = stockInSrv.GetObjects(typeof(OperationRole), oq);

            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(strXmlFile);

            XmlDocument objXmlDocDictionary = new XmlDocument();
            objXmlDocDictionary.Load(strXmlFileDictionary);

            XmlNode theXmlNode = objXmlDoc.SelectSingleNode("UsersDirectorySerializer");
            foreach (OperationRole role in lst)
            {
                XmlElement theXMlElement = objXmlDoc.CreateElement("Role");
                theXMlElement.SetAttribute("Name", role.RoleName);
                theXMlElement.SetAttribute("EMail", "");
                theXMlElement.SetAttribute("RoleId", role.Id);
                theXmlNode.AppendChild(theXMlElement);

                UpdateDictionary(objXmlDocDictionary, role.RoleName, role.RoleName);
            }
            objXmlDoc.Save(strXmlFile);
            objXmlDocDictionary.Save(strXmlFileDictionary);
        }

        public static void UpdateUser(IStockInSrv stockInSrv)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("ConnMethod", NHibernate.FetchMode.Eager);
            IList lst = stockInSrv.GetObjects(typeof(StandardPerson), oq);

            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(strXmlFile);

            XmlDocument objXmlDocDictionary = new XmlDocument();
            objXmlDocDictionary.Load(strXmlFileDictionary);

            XmlNode theXmlNode = objXmlDoc.SelectSingleNode("UsersDirectorySerializer");
            foreach (StandardPerson theUser in lst)
            {
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
                theXmlElement.SetAttribute("Local", "C:\\temp\\VOB");
                theXmlElement.SetAttribute("PrivateWorkspace", "C:\\temp\\PWS");
                theXmlElement.SetAttribute("UserId", theUser.Id);

                foreach (ContactMethod theContactMethod in theUser.ConnMethod)
                {
                    theXmlElement.SetAttribute("EMail", theContactMethod.Email);
                    theXmlElement.SetAttribute("Phone", theContactMethod.Mobile);
                    theXmlElement.SetAttribute("Address", theContactMethod.Address);
                    theXmlElement.SetAttribute("ZipCode", theContactMethod.Fax);
                }


                theXmlNode.AppendChild(theXmlElement);
                UpdateDictionary(objXmlDocDictionary, theUser.Code, theUser.Name);
            }
            objXmlDoc.Save(strXmlFile);
            objXmlDocDictionary.Save(strXmlFileDictionary);
        }

        public static void PersonOnJob(IStockInSrv stockInSrv)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("OperationJob.JobWithRole", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("StandardPerson", NHibernate.FetchMode.Eager);
            IList lst = stockInSrv.GetObjects(typeof(PersonOnJob), oq);

            XmlDocument objXmlDoc = new XmlDocument();
            objXmlDoc.Load(strXmlFile);

            //XmlNode theXmlNode = objXmlDoc.SelectSingleNode("UsersDirectorySerializer");
            XmlNodeList theXmlNodeList = objXmlDoc.GetElementsByTagName("User");
            foreach (XmlNode theXmlNode in theXmlNodeList)
            {
                XmlElement theXmlElement = (XmlElement)theXmlNode;
                string userCode = theXmlElement.GetAttribute("Name");
                IList userJobsLst = MatchJobs(userCode, lst);
                if (userJobsLst.Count > 0)
                {
                    foreach (OperationJob job in userJobsLst)
                    {
                        XmlElement theJobXmlElement = objXmlDoc.CreateElement("GroupRef");
                        theJobXmlElement.SetAttribute("Name", job.Code);
                        if (job.JobWithRole.Count > 0)
                        {
                            foreach (OperationJobWithRole jobWithRole in job.JobWithRole)
                            {
                                XmlElement theRoleXmlElement = objXmlDoc.CreateElement("RoleRef");
                                theRoleXmlElement.SetAttribute("Name", jobWithRole.OperationRoleName);
                                theJobXmlElement.AppendChild(theRoleXmlElement);
                            }
                            theXmlNode.AppendChild(theJobXmlElement);
                        }
                        
                    }
                }
            }
            objXmlDoc.Save(strXmlFile);
        }

        private static IList MatchJobs(string personCode,IList personOnJobLst)
        {
            IList lst = new ArrayList();
            foreach (PersonOnJob obj in personOnJobLst)
            {
                if (obj.StandardPerson.Code == personCode)
                {
                    lst.Add(obj.OperationJob);
                }
            }
            return lst;
        }
    }
}
