using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.PortalIntegration.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Business.Erp.PortalIntegration.CommonClass;
using System.Collections;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.SystemAspect.Security;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.PortalIntegration.Service
{
    /// <summary>
    /// 门户集成服务
    /// </summary>
    public class PortalService : IPortalService
    {
        #region 服务
        private static IOperationOrgService operationOrgService;
        /// <summary>
        /// 组织服务
        /// </summary>
        /// <returns></returns>
        private IOperationOrgService GetOperationOrgService()
        {
            if (operationOrgService == null)
            {
                operationOrgService = StaticMethod.GetService("ResourceManager", "OperationOrgService") as IOperationOrgService;
            }
            return operationOrgService;
        }

        private static IStockInSrv stockInSrv;
        /// <summary>
        /// 入库服务 用于保存项目信息
        /// </summary>
        /// <returns></returns>
        private IStockInSrv GetStockInSrv()
        {
            if (stockInSrv == null) stockInSrv = StaticMethod.GetService("SupplyChain", "StockInSrv") as IStockInSrv;
            return stockInSrv;
        }

        private static IOperationJobManager operationJobManager;
        /// <summary>
        /// 岗位服务
        /// </summary>
        /// <returns></returns>
        private IOperationJobManager GetOperationJobService()
        {
            if (operationJobManager == null)
            {
                operationJobManager = StaticMethod.GetService("ResourceManager", "OperationJobManager") as IOperationJobManager;
            }
            return operationJobManager;
        }

        private static IPersonManager personManager;
        /// <summary>
        /// 人员服务
        /// </summary>
        /// <returns></returns>
        private IPersonManager GetPersonService()
        {
            if (personManager == null) personManager = StaticMethod.GetService("ResourceManager", "PersonManager") as IPersonManager;
            return personManager;
        }

        private static IPersonOnJobManager personOnJobManager;
        /// <summary>
        /// 人员上岗服务
        /// </summary>
        /// <returns></returns>
        private IPersonOnJobManager GetPersonOnJobService()
        {
            if (personOnJobManager == null) personOnJobManager = StaticMethod.GetService("ResourceManager", "PersonOnJobManager") as IPersonOnJobManager;
            return personOnJobManager;
        }
        #endregion

        private static object userLock = new object();
        private static object projectLock = new object();
        private static long printOutCount = 1;

        /// <summary>
        /// 返回失败对象
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private RetOb RetFailer(string result)
        {
            RetOb retOb = new RetOb();
            retOb.Out0 = 0;
            retOb.Result = result;
            PrintMessage("结束调用。" + result);
            return retOb;
        }

        private void PrintMessage(string message)
        {
            string printMessage = System.Configuration.ConfigurationSettings.AppSettings["PrintMessage"];
            if (printMessage != null && printMessage.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                if (printOutCount % 300 == 0)//消息日志满300清理缓冲区
                    Console.Clear();

                printOutCount += 1;
                Console.WriteLine(DateTime.Now.ToString() + "：" + message);
            }
        }

        private void PrintEmptyLine()
        {
            Console.WriteLine();
        }

        protected string getMessage(Exception e)
        {
            if (e.InnerException == null)
                return e.Message;
            else
                return e.Message + Environment.NewLine + "InnerException:" + getMessage(e.InnerException);
        }

        #region 组织

        /// <summary>
        /// 增加组织机构
        /// </summary>
        /// <param name="parentOrgCode">父机构编码</param>
        /// <param name="org">机构实体</param>
        /// <returns></returns>
        public RetOb AddOrg(string parentOrgCode, OpeOrg org)
        {
            PrintMessage("开始调用AddOrg()方法");
            RetOb retOb = new RetOb();
            if (org == null)
            {
                return RetFailer("组织机构实体为空。");
            }
            if (org.OrgCode == null || org.OrgCode == "" || org.OrgName == null || org.OrgName == "")
            {
                return RetFailer("组织机构编码或名称为空。");
            }
            PrintMessage("机构名称：" + org.OrgName + "，机构代码：" + org.OrgCode);

            try
            {
                OperationOrg temporg = GetOperationOrgByCode(org.OrgCode);
                if (temporg == null)
                {
                    //return RetFailer("组织机构【" + org.OrgCode + "】已经存在。");

                    OperationOrg parentOperationOrg = null;

                    if (parentOrgCode != null && !(parentOrgCode == ""))
                    {
                        parentOperationOrg = GetOperationOrgByCode(parentOrgCode);
                        if (parentOperationOrg == null)
                        {
                            return RetFailer("根据父机构编码【" + parentOrgCode + "】未找到组织机构。");
                        }
                    }
                    else
                    {
                        parentOperationOrg = GetRootOperationOrg();
                    }
                    if (org.OrgType != null && (org.OrgType.Equals("zgxmb", StringComparison.OrdinalIgnoreCase) || org.OrgType.Equals("fgsxmb", StringComparison.OrdinalIgnoreCase)))
                    {
                        //如果机构类型为项目部，新生成一个工程项目对象
                        OperationOrg tempOrg = OrgToOperationOrg(parentOperationOrg, org);
                        tempOrg.IsAccountOrg = true;
                        SaveOperationOrgAndProjectInfo(tempOrg);
                    }
                    else
                    {
                        OperationOrg tempOrg = OrgToOperationOrg(parentOperationOrg, org);
                        GetOperationOrgService().SaveOperationOrg(tempOrg);
                    }
                }
                else
                {
                    retOb = UpdateOrg(org);
                }
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用AddOrg()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用AddOrg()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 更新组织机构
        /// </summary>
        /// <param name="org">组织机构实体</param>
        /// <returns></returns>
        public RetOb UpdateOrg(OpeOrg org)
        {
            PrintMessage("开始调用UpdateOrg()方法");
            RetOb retOb = new RetOb();
            if (org == null)
            {
                return RetFailer("组织机构实体为空。");
            }
            if (org.OrgCode == null || org.OrgCode == "" || org.OrgName == null || org.OrgName == "")
            {
                return RetFailer("组织机构编码或名称为空。");
            }
            PrintMessage("机构名称：" + org.OrgName + "，机构代码：" + org.OrgCode);

            try
            {
                OperationOrg temporg = GetOperationOrgByCode(org.OrgCode);
                if (temporg == null)
                {
                    retOb = AddOrg(org.ParentOrgCode, org);

                    //return RetFailer("通过机构编码【" + org.OrgCode + "】未找到组织机构。");
                }
                else
                {
                    temporg.Name = org.OrgName;
                    temporg.OperationType = org.OrgType;
                    if (org.OrgType != null && (org.OrgType.Equals("zgxmb", StringComparison.OrdinalIgnoreCase) || org.OrgType.Equals("fgsxmb", StringComparison.OrdinalIgnoreCase)))
                    {
                        //如果机构类型为项目部，新生成一个工程项目对象
                        SaveOperationOrgAndProjectInfo(temporg);
                    }
                    else
                    {
                        GetOperationOrgService().SaveOperationOrg(temporg);
                    }
                }
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用UpdateOrg()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用UpdateOrg()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="org">组织机构实体</param>
        /// <returns></returns>
        public RetOb DeleteOrg(OpeOrg org)
        {
            PrintMessage("开始调用DeleteOrg()方法");
            RetOb retOb = new RetOb();
            if (org == null)
            {
                return RetFailer("组织机构实体为空。");
            }
            if (org.OrgCode == null)
            {
                return RetFailer("组织机构编码为空。");
            }
            PrintMessage("机构代码：" + org.OrgCode);

            try
            {
                OperationOrg tempOrg = GetOperationOrgByCode(org.OrgCode);
                if (tempOrg == null)
                {
                    return RetFailer("通过机构编码【" + org.OrgCode + "】未找到组织。");
                }
                GetOperationOrgService().InvalidateOperationOrg(tempOrg);
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用DeleteOrg()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用DeleteOrg()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 查询orgCode对应的机构
        /// </summary>
        /// <param name="orgCode">机构编码</param>
        /// <returns></returns>
        public OpeOrg GetOrgByCode(string orgCode)
        {
            PrintMessage("开始调用GetOrgByCode()方法");
            try
            {
                OperationOrg tempOrg = GetOperationOrgByCode(orgCode);
                if (tempOrg == null)
                {
                    return null;
                }
                OpeOrg org = new OpeOrg();
                org.OrgCode = orgCode;
                org.OrgName = tempOrg.Name;
                org.OrgType = tempOrg.OperationType;
                org.ParentOrgCode = tempOrg.ParentNode.Code;
                return org;
            }
            catch
            { }
            PrintMessage("结束调用GetOrgByCode()方法");
            return null;
        }

        private OperationOrg OrgToOperationOrg(OperationOrg parentOperationOrg, OpeOrg org)
        {
            OperationOrg tempOrg = new OperationOrg();
            tempOrg.Code = org.OrgCode;
            tempOrg.Name = org.OrgName;
            tempOrg.OperationType = org.OrgType;
            tempOrg.ParentNode = parentOperationOrg;
            return tempOrg;
        }

        /// <summary>
        /// 保存业务组织和工程项目信息
        /// </summary>
        [TransManager]
        private void SaveOperationOrgAndProjectInfo(OperationOrg operationOrg)
        {
            CurrentProjectInfo projectInfo = new CurrentProjectInfo();
            if (!string.IsNullOrEmpty(operationOrg.Id))//修改
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("OwnerOrg.Id", operationOrg.Id));
                IList list = GetStockInSrv().ObjectQuery(typeof(CurrentProjectInfo), oq);
                if (list != null && list.Count > 0)
                {
                    projectInfo = list[0] as CurrentProjectInfo;
                }
            }
            OperationOrg tempOrg = GetOperationOrgService().SaveOperationOrg(operationOrg);

            projectInfo.Name = tempOrg.Name;
            projectInfo.Code = tempOrg.Code;
            projectInfo.OwnerOrg = tempOrg;
            projectInfo.OwnerOrgName = tempOrg.Name;
            projectInfo.OwnerOrgSysCode = tempOrg.SysCode;

            GetStockInSrv().SaveCurrentProjectInfo(projectInfo);
        }

        /// <summary>
        /// 查询业务组织根节点
        /// </summary>
        /// <returns></returns>
        private OperationOrg GetRootOperationOrg()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
            oq.AddCriterion(Expression.Eq("State", 1));//有效
            IList lst = GetOperationOrgService().GetOperationOrgs(typeof(OperationOrg), oq);
            if (lst != null && lst.Count > 0)
            {
                return lst[0] as OperationOrg;
            }
            return null;
        }

        /// <summary>
        /// 根据机构代码查询业务组织
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        private OperationOrg GetOperationOrgByCode(string orgCode)
        {
            if (orgCode == null || orgCode.Equals("")) return null;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", orgCode));
            oq.AddFetchMode("ParentNode", NHibernate.FetchMode.Eager);
            IList lst = GetOperationOrgService().GetOperationOrgs(typeof(OperationOrg), oq);
            if (lst != null && lst.Count > 0)
            {
                return lst[0] as OperationOrg;
            }
            return null;
        }

        /// <summary>
        /// 设置机构隐藏显示
        /// </summary>
        /// <param name="orgCode">组织机构代码</param>
        /// <param name="flag">隐藏显示标记（0=隐藏,1=显示）</param>
        /// <returns></returns>
        public RetOb SetOrgInfo(string orgCode, int flag)
        {
            PrintMessage("开始调用SetOrgInfo()方法");
            RetOb retOb = new RetOb();
            if (string.IsNullOrEmpty(orgCode))
            {
                return RetFailer("机构代码为空。");
            }
            PrintMessage("机构代码：" + orgCode);

            try
            {
                OperationOrg temporg = GetOperationOrgByCode(orgCode);
                if (temporg == null)
                {
                    return RetFailer("通过机构编码【" + orgCode + "】未找到组织机构。");
                }

                temporg.State = flag;
                GetOperationOrgService().SaveOperationOrg(temporg);
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用SetOrgInfo()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用SetOrgInfo()方法");
            return retOb;
        }
        #endregion

        #region 用户
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public RetOb AddUser(User user)
        {
            lock (userLock)
            {
                PrintMessage("开始调用AddUser()方法");
                RetOb retOb = new RetOb();
                if (user == null || user.UserCode == null || user.UserCode.Equals("") || user.UserName == null || user.UserName.Equals(""))
                {
                    return RetFailer("用户编码或用户名称为空。");
                }
                if (user.Password == null || user.Password.Equals(""))
                {
                    return RetFailer("用户[" + user.UserCode + "," + user.UserName + "]密码为空");
                }
                PrintMessage("用户名称：" + user.UserName + "，用户代码：" + user.UserCode);
                try
                {
                    StandardPerson person = GetStandardPersonByCode(user.UserCode);
                    if (person == null)
                    {
                        //return RetFailer("用户【" + user.UserCode + "】已经存在。");

                        person = UserToStandardPerson(user);
                        person.AddState = 1;

                        GetPersonService().SavePerson(person);//如果存在更新数据库和XML
                    }
                }
                catch (Exception ex)
                {
                    return RetFailer("结束调用AddUser()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                }
                PrintMessage("结束调用AddUser()方法," + getResultValue(retOb));
                return retOb;
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public RetOb UpdateUser(User user)
        {
            PrintMessage("开始调用UpdateUser()方法");
            RetOb retOb = new RetOb();
            if (user == null || user.UserCode == null || user.UserCode.Equals("") || user.UserName == null || user.UserName.Equals(""))
            {
                return RetFailer("用户编码或用户名称为空。");
            }
            if (user.Password == null || user.Password.Equals(""))
            {
                return RetFailer("用户密码为空。");
            }
            PrintMessage("用户名称：" + user.UserName + "，用户代码：" + user.UserCode);

            try
            {
                StandardPerson person = GetStandardPersonByCode(user.UserCode);
                if (person == null)
                {
                    retOb = AddUser(user);
                    //return RetFailer("根据用户编码【" + user.UserCode + "】未发现用户。");
                }
                else
                {
                    person.Name = user.UserName;
                    string newPass = CryptoString.Encrypt(user.Password, user.Password);
                    if (newPass != person.Password)
                    {
                        person.Password = newPass;
                    }
                    person.UpdateState = 1;
                    GetPersonService().SavePerson(person);
                }
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用UpdateUser()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用UpdateUser()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        public RetOb DeleteUser(string userCode)
        {
            PrintMessage("开始调用DeleteUser()方法");
            RetOb retOb = new RetOb();
            if (userCode == null || userCode.Equals(""))
            {
                return RetFailer("用户编码为空。");
            }
            PrintMessage("用户代码：" + userCode);
            try
            {
                StandardPerson person = GetStandardPersonByCode(userCode);
                if (person == null)
                {
                    return RetFailer("根据用户编码【" + userCode + "】未发现用户。");
                }
                person.State = 0;
                person.DelState = 1;
                GetPersonService().SavePerson(person);
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用DeleteUser()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用DeleteUser()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 根据用户编码查询用户信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        public User GetUserByCode(string userCode)
        {
            PrintMessage("开始调用GetUserByCode()方法");
            if (userCode == null || userCode.Equals(""))
            {
                PrintMessage("结束调用GetUserByCode()方法");
                return null;
            }
            try
            {
                StandardPerson person = GetStandardPersonByCode(userCode);

                if (person == null)
                {
                    PrintMessage("根据用户代码：" + userCode + ",未获取到用户信息");
                    return null;
                }

                User user = new User();
                user.Password = person.Password;
                user.UserCode = person.Code;
                user.UserName = person.Name;
                return user;
            }
            catch (Exception ex)
            {
                PrintMessage("异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用GetUserByCode()方法");
            return null;
        }

        /// <summary>
        /// 修改用户拥有的岗位
        /// </summary>
        /// <param name="emps">岗位编码列表 code1,code2</param>
        /// <param name="userCode">用户编码</param>
        /// <returns>用户拥有的岗位编码列表 code1,code2</returns>
        public string UpdateEmpPost(string emps, string userCode)
        {
            PrintMessage("开始调用UpdateEmpPost()方法 emps:" + emps + " userCode:" + userCode);
            if (userCode == null || userCode.Equals(""))
            {
                PrintMessage("结束调用UpdateEmpPost()方法 用户编码为空。");
                return null;
            }

            StandardPerson person = GetStandardPersonByCode(userCode);
            if (person == null)
            {
                PrintMessage("结束调用UpdateEmpPost()方法 根据用户编码【" + userCode + "】未找到用户信息");
                return null;
            }

            try
            {
                //把[人员]的[关系标志]置为1 2014-06-01
                person.RelState = 1;
                GetPersonService().SavePerson(person);

                IList oldPeronOnJobLst = GetPersonOnJobByUserCode(userCode);

                //岗位列表为空，删除用户关联的岗位
                if (emps == null || emps == "")
                {
                    GetPersonOnJobService().DeletePersonOnJob(oldPeronOnJobLst);
                    PrintMessage("结束调用UpdateEmpPost()方法 ");
                    return null;
                }
                char[] ch = { ',' };
                string[] empsArray = emps.Split(ch, StringSplitOptions.RemoveEmptyEntries);
                if (empsArray == null || empsArray.Length == 0)
                {
                    GetPersonOnJobService().DeletePersonOnJob(oldPeronOnJobLst);
                    PrintMessage("结束调用UpdateEmpPost()方法 ");
                    return null;
                }

                ///添加的岗位编码
                IList canAddJobCodeLst = new ArrayList();
                IList updateJobCodeList = new ArrayList();//要修改的岗位编码
                foreach (string jobCode in empsArray)
                {
                    bool canAdd = true;
                    foreach (PersonOnJob perOnJob in oldPeronOnJobLst)
                    {
                        if (perOnJob.OperationJob.Code == jobCode)
                        {
                            canAdd = false;
                            updateJobCodeList.Add(perOnJob);
                            break;
                        }
                    }
                    if (canAdd)
                    {
                        canAddJobCodeLst.Add(jobCode);
                    }
                }

                //要删除的人员上岗
                IList removePersonOnJobLst = new ArrayList();
                foreach (PersonOnJob perOnJob in oldPeronOnJobLst)
                {
                    bool canRemove = true;
                    foreach (string jobCode in empsArray)
                    {
                        if (jobCode == perOnJob.OperationJob.Code)
                        {
                            canRemove = false;
                            break;
                        }
                    }
                    if (canRemove)
                    {
                        removePersonOnJobLst.Add(perOnJob);
                    }
                }
                return UpdatePersonOnJob(removePersonOnJobLst, canAddJobCodeLst, updateJobCodeList, person);
            }
            catch (Exception ex)
            {

                PrintMessage("结束调用UpdateEmpPost()方法，返回值：null，操作异常：" + ExceptionUtil.ExceptionMessage(ex));
                return null;
            }
        }

        [TransManager]
        private string UpdatePersonOnJob(IList removePersonOnJobLst, IList canAddJobCodeLst, IList updateJobCodeList, StandardPerson person)
        {
            GetPersonOnJobService().DeletePersonOnJob(removePersonOnJobLst);
            foreach (string jobCode in canAddJobCodeLst)
            {
                OperationJob opeJob = GetOperationJobByCode(jobCode);
                if (opeJob == null) continue;
                PersonOnJob perOnJob = new PersonOnJob();
                perOnJob.BeginDate = DateTime.Now;
                perOnJob.CreatedDate = DateTime.Now;
                perOnJob.EndDate = DateTime.Now.AddYears(20);
                perOnJob.OperationJob = opeJob;
                perOnJob.StandardPerson = person;
                perOnJob.State = 1;
                perOnJob.UpdatedDate = DateTime.Now;
                GetPersonOnJobService().SavePersonOnJob(perOnJob);
            }
            foreach (PersonOnJob perOnJob in updateJobCodeList)
            {
                GetPersonOnJobService().SavePersonOnJob(perOnJob);
            }
            IList lst = GetPersonOnJobByUserCode(person.Code);
            string retStr = "";

            foreach (PersonOnJob personOnJob in lst)
            {
                retStr += personOnJob.OperationJob.Code + ",";
            }
            if (retStr.IndexOf(",") > 0)
            {
                retStr = retStr.Substring(0, retStr.LastIndexOf(","));
            }
            PrintMessage("结束调用UpdateEmpPost()方法 retStr:" + retStr);
            return retStr;
        }

        /// <summary>
        /// 根据用户编码查询用户岗位
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        private IList GetPersonOnJobByUserCode(string userCode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("StandardPerson.Code", userCode));
            oq.AddCriterion(Expression.Eq("StandardPerson.State", 1));

            oq.AddFetchMode("OperationJob", NHibernate.FetchMode.Eager);
            return GetStockInSrv().GetObjects(typeof(PersonOnJob), oq);
        }

        private StandardPerson UserToStandardPerson(User user)
        {
            Employee person = new Employee();
            person.Code = user.UserCode;
            person.CreatedDate = DateTime.Now;
            person.Name = user.UserName;
            person.State = 1;
            person.Password = CryptoString.Encrypt(user.Password, user.Password);
            return person;
        }

        private IList GetProjectMngAndIRPStandardPerson(string personCode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", personCode));
            oq.AddCriterion(Expression.Eq("State", 1));
            return GetPersonService().GetProjectMngAndIRPPerson(typeof(Employee), oq);
        }

        private StandardPerson GetStandardPersonByCode(string personCode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", personCode));
            oq.AddCriterion(Expression.Eq("State", 1));
            IList lst = GetPersonService().GetPerson(typeof(Employee), oq);
            if (lst != null && lst.Count > 0)
            {
                return lst[0] as StandardPerson;
            }
            return null;
        }

        #endregion

        #region 岗位
        /// <summary>
        /// 增加岗位
        /// </summary>
        /// <param name="orgCode">岗位所属机构编码</param>
        /// <param name="post">岗位实体</param>
        /// <returns></returns>
        public RetOb AddPost(string orgCode, Post post)
        {
            PrintMessage("开始调用AddPost()方法");
            RetOb retOb = new RetOb();
            if (orgCode == null || orgCode.Equals(""))
            {
                return RetFailer("岗位所属机构编码为空。");
            }
            if (post == null || post.PosiCode == null || post.PosiCode.Equals("") || post.PosiName == null || post.PosiName.Equals(""))
            {
                return RetFailer("岗位编码或岗位名称为空。");
            }
            PrintMessage("机构代码：" + orgCode + "岗位名称：" + post.PosiName + "，岗位代码：" + post.PosiCode);

            try
            {
                //判断岗位是否已经存在
                OperationJob operationJob = GetOperationJobByCode(post.PosiCode);
                if (operationJob == null)
                {
                    OperationOrg operationOrg = GetOperationOrgByCode(orgCode);
                    if (operationOrg == null)
                    {
                        return RetFailer("结束调用AddPost()方法,根据机构编码【" + orgCode + "】未找到组织。");
                    }

                    OperationJob job = PostToOperationJob(post);
                    job.OperationOrg = operationOrg;
                    job.AddState = 1;
                    GetOperationJobService().SaveOperationJob(job);
                }
                //else//岗位存在，可能需要修改XML
                //{
                //    operationJob.UpdateState = 1;
                //    GetOperationJobService().SaveOperationJob(operationJob);
                //}
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用AddPost()方法" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用AddPost()方法," + getResultValue(retOb));
            return retOb;
        }

        private string getResultValue(RetOb retOb)
        {
            string message = "";
            if (retOb != null)
            {
                message = "返回值标记：" + retOb.Out0 + ",返回值结果：" + retOb.Result;
            }
            else
                message = "返回值对象为NULL";

            return message;
        }

        /// <summary>
        /// 修改岗位
        /// </summary>
        /// <param name="post">岗位实体</param>
        /// <returns></returns>
        public RetOb UpdatePost(Post post)
        {
            PrintMessage("开始调用UpdatePost()方法");
            RetOb retOb = new RetOb();
            if (post == null || post.PosiCode == null || post.PosiCode.Equals("") || post.PosiName == null || post.PosiName.Equals(""))
            {
                return RetFailer("岗位编码或岗位名称为空。");
            }
            PrintMessage("岗位代码：" + post.PosiCode);

            try
            {
                //判断岗位是否已经存在
                OperationJob operationJob = GetOperationJobByCode(post.PosiCode);
                if (operationJob == null)
                {
                    retOb = AddPost(post.OrgCode, post);
                }
                else
                {
                    operationJob.Name = post.PosiName;
                    operationJob.OrderNo = post.OrderNo;
                    operationJob.UpdateState = 1;
                    GetOperationJobService().SaveOperationJob(operationJob);
                }
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用UpdatePost()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用UpdatePost()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="post">岗位实体</param>
        /// <returns></returns>
        public RetOb DeletePost(Post post)
        {
            PrintMessage("开始调用DeletePost()方法");
            RetOb retOb = new RetOb();
            if (post == null || post.PosiCode == null || post.PosiCode.Equals(""))
            {
                return RetFailer("岗位编码为空。");
            }
            PrintMessage("岗位代码：" + post.PosiCode);

            //判断岗位是否已经存在
            OperationJob operationJob = GetOperationJobByCode(post.PosiCode);
            if (operationJob == null)
            {
                return RetFailer("根据岗位编码【" + post.PosiCode + "】未发现岗位。");
            }

            try
            {
                operationJob.State = 0;
                operationJob.DelState = 1;
                GetOperationJobService().SaveOperationJob(operationJob);
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用DeletePost()方法,异常信息" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用DeletePost()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 根据岗位编码查询岗位
        /// </summary>
        /// <param name="postCode">岗位编码</param>
        /// <returns></returns>
        public Post GetPostByCode(string postCode)
        {
            PrintMessage("开始调用GetPostByCode()方法");
            if (postCode == null || postCode.Equals("")) return null;
            try
            {
                OperationJob operationJob = GetOperationJobByCode(postCode);

                if (operationJob == null)
                {
                    PrintMessage("根据岗位代码：" + postCode + ",未获取到岗位信息");
                    return null;
                }

                Post post = new Post();
                post.OrgCode = operationJob.OperationOrg.Code;
                post.PosiCode = operationJob.Code;
                post.PosiName = operationJob.Name;
                post.OrderNo = (int)operationJob.OrderNo;
                return post;
            }
            catch (Exception ex)
            {
                PrintMessage("异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用GetPostByCode()方法");
            return null;
        }

        /// <summary>
        /// 岗位关联角色
        /// </summary>
        /// <param name="emps">角色编码列表 形如code1,code2</param>
        /// <param name="posiCode">岗位编码</param>
        /// <returns>角色编码列表 形如code1,code2</returns>
        public string UpdateEmpRole(string emps, string posiCode)
        {
            string retStr = "";
            PrintMessage("开始调用UpdateEmpRole()方法 emps:" + emps + " posiCode:" + posiCode);
            if (posiCode == null || posiCode.Equals(""))
            {
                //throw new Exception("岗位编码为空。");
                PrintMessage("结束调用UpdateEmpRole()方法 岗位编码为空。");
                return null;
            }
            try
            {
                OperationJob operationJob = GetOperationJobByCodeWithRole(posiCode);
                if (operationJob == null)
                {
                    //throw new Exception("根据岗位编码【" + posiCode + "】未发现对应岗位");
                    PrintMessage("结束调用UpdateEmpRole()方法 根据岗位编码【" + posiCode + "】未发现对应岗位");
                    return null;
                }

                operationJob.RelState = 1;
                if (emps == null || emps == "")
                {
                    if (operationJob.JobWithRole.Count > 0)
                    {
                        operationJob.JobWithRole.Clear();
                        GetOperationJobService().SaveOperationJob(operationJob);
                        PrintMessage("结束调用UpdateEmpRole()方法");
                        return null;
                    }
                }

                char[] ch = { ',' };
                string[] empsArray = emps.Split(ch, StringSplitOptions.RemoveEmptyEntries);
                if (empsArray == null || empsArray.Length == 0)
                {
                    if (operationJob.JobWithRole.Count > 0)
                    {
                        operationJob.JobWithRole.Clear();
                        GetOperationJobService().SaveOperationJob(operationJob);
                        PrintMessage("结束调用UpdateEmpRole()方法");
                        return null;
                    }
                }

                //获取要删除的列表
                IList<OperationJobWithRole> removeLst = new List<OperationJobWithRole>();
                foreach (OperationJobWithRole jobWithRole in operationJob.JobWithRole)
                {
                    bool canRemove = true;
                    foreach (string roleCode in empsArray)
                    {
                        if (jobWithRole.OperationRole.RoleCode == roleCode)
                        {
                            canRemove = false;
                            break;
                        }
                    }
                    if (canRemove)
                    {
                        removeLst.Add(jobWithRole);
                    }
                }

                //获取要添加的列表
                IList<string> addEmpCodeLst = new List<string>();
                foreach (string roleCode in empsArray)
                {
                    bool canAdd = true;
                    foreach (OperationJobWithRole jobWithRole in operationJob.JobWithRole)
                    {
                        if (jobWithRole.OperationRole.RoleCode == roleCode)
                        {
                            canAdd = false;
                            break;
                        }
                    }
                    if (canAdd)
                    {
                        addEmpCodeLst.Add(roleCode);
                    }
                }

                //删除 删除列表
                operationJob.JobWithRole.RemoveAll(removeLst);
                //添加 添加列表
                foreach (string roleCode in addEmpCodeLst)
                {
                    OperationRole linkRole = GetOperationRole(roleCode);
                    if (linkRole == null) continue;
                    OperationJobWithRole detail = new OperationJobWithRole();
                    detail.OperationJob = operationJob;
                    detail.OperationJobName = operationJob.Name;
                    detail.OperationRole = linkRole;
                    detail.OperationRoleName = linkRole.RoleName;
                    operationJob.AddJobWithRole(detail);
                }

                operationJob = GetOperationJobService().SaveOperationJob(operationJob);

                foreach (OperationJobWithRole jobWithRole in operationJob.JobWithRole)
                {
                    retStr += jobWithRole.OperationRole.RoleCode + ",";
                }
                if (retStr.IndexOf(",") > 0)
                {
                    retStr = retStr.Substring(0, retStr.LastIndexOf(","));
                }
            }
            catch (Exception ex)
            {
                PrintMessage("结束调用UpdateEmpRole()方法，返回值：null，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                return null;
            }

            PrintMessage("结束调用UpdateEmpRole()方法 retStr:" + retStr);
            return retStr;
        }

        private OperationJob PostToOperationJob(Post post)
        {
            OperationJob job = new OperationJob();
            job.Code = post.PosiCode;
            job.CreatedDate = DateTime.Now;
            job.Name = post.PosiName;
            job.State = 1;
            job.OrderNo = post.OrderNo;
            return job;
        }

        /// <summary>
        /// 根据条件获取State为1的岗位集合（且IRP中也存在该岗位,当项目管理中存在IRP中不存在返回集合中第一个对象为null）
        /// </summary>
        private IList GetProjectMngAndIRPJob(string jobCode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", jobCode));
            oq.AddFetchMode("OperationOrg", NHibernate.FetchMode.Eager);
            return GetOperationJobService().GetProjectMngAndIRPJob(oq);
        }

        /// <summary>
        /// 根据岗位编码查询MBP岗位
        /// </summary>
        /// <param name="jobCode"></param>
        /// <returns></returns>
        private OperationJob GetOperationJobByCode(string jobCode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", jobCode));
            oq.AddFetchMode("OperationOrg", NHibernate.FetchMode.Eager);
            IList lst = GetOperationJobService().GetJobByQuery(oq);
            if (lst != null && lst.Count > 0)
            {
                return lst[0] as OperationJob;
            }
            return null;
        }

        /// <summary>
        /// 根据code查询岗位，关联岗位对应的角色
        /// </summary>
        /// <param name="jobCode"></param>
        /// <returns></returns>
        private OperationJob GetOperationJobByCodeWithRole(string jobCode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", jobCode));
            //oq.AddFetchMode("OperationOrg", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("JobWithRole", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("JobWithRole.OperationRole", NHibernate.FetchMode.Eager);
            IList lst = GetOperationJobService().GetJobByQuery(oq);
            if (lst != null && lst.Count > 0)
            {
                return lst[0] as OperationJob;
            }
            return null;
        }
        #endregion

        #region 角色
        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <returns></returns>
        public RetOb AddRole(Role role)
        {
            PrintMessage("开始调用AddRole()方法");
            RetOb retOb = new RetOb();
            if (role == null || role.RoleCode == null || role.RoleCode.Equals("") || role.RoleName == null || role.RoleName.Equals(""))
            {
                return RetFailer("角色名称或角色编码为空。");
            }
            PrintMessage("角色名称：" + role.RoleName + "，角色代码：" + role.RoleCode);

            try
            {
                OperationRole operationRole = GetOperationRole(role.RoleName);
                if (operationRole == null)
                {
                    return RetFailer("根据角色名称【" + role.RoleName + "】未找到相应的角色。");
                }

                operationRole.UpdateState = 1;
                operationRole.RoleCode = role.RoleCode;
                operationRole.State = 1;
                GetOperationJobService().SaveOperationRole(operationRole);
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用AddRole()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用AddRole()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <returns></returns>
        public RetOb DeleteRole(Role role)
        {
            PrintMessage("开始调用DeleteRole()方法");
            RetOb retOb = new RetOb();
            if (role == null || role.RoleCode == null || role.RoleCode.Equals(""))
            {
                return RetFailer("角色编码为空。");
            }
            PrintMessage("角色代码：" + role.RoleCode);

            OperationRole operationRole = GetOperationRole(role.RoleCode);
            if (operationRole == null)
            {
                return RetFailer("根据角色编码【" + role.RoleCode + "】未找到相应的角色。");
            }
            operationRole.State = 0;
            operationRole.DelState = 1;
            try
            {
                GetOperationJobService().SaveOperationRole(operationRole);
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用DeleteRole()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用DeleteRole()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 根据角色编码查询角色
        /// </summary>
        /// <param name="roleCode">角色编码</param>
        /// <returns></returns>
        public Role GetRoleByCode(string roleCode)
        {
            PrintMessage("开始调用GetRoleByCode()方法，角色代码：" + roleCode);
            if (roleCode == null || roleCode.Equals(""))
            {
                PrintMessage("角色名称/代码不能为空");
                return null;
            }

            try
            {
                OperationRole operationRole = GetOperationRole(roleCode);

                if (operationRole == null)
                {
                    PrintMessage("根据角色名称/代码：" + roleCode + ",未获取到角色信息");
                    return null;
                }

                Role role = new Role();
                role.RoleCode = roleCode;
                role.RoleName = operationRole.RoleName;
                return role;
            }
            catch (Exception ex)
            {
                PrintMessage("异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用GetRoleByCode()方法");
            return null;
        }

        /// <summary>
        /// 获取角色（且IRP中也存在该角色,当项目管理中存在IRP中不存在返回集合中第一个对象为null）
        /// </summary>
        private IList GetProjectMngAndIRPRole(string roleCodeOrRoleName)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            dis.Add(Expression.Eq("RoleName", roleCodeOrRoleName));
            dis.Add(Expression.Eq("RoleCode", roleCodeOrRoleName));
            oq.AddCriterion(dis);
            return GetOperationJobService().GetProjectMngAndIRPRole(oq);
        }

        private OperationRole GetOperationRole(string roleCodeOrRoleName)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            dis.Add(Expression.Eq("RoleName", roleCodeOrRoleName));
            dis.Add(Expression.Eq("RoleCode", roleCodeOrRoleName));
            oq.AddCriterion(dis);
            //oq.AddCriterion(Expression.Eq("State", 1));
            IList lst = GetOperationJobService().GetOperationRole(oq);
            if (lst != null && lst.Count > 0)
            {
                return lst[0] as OperationRole;
            }
            return null;
        }
        #endregion

        #region 工程项目
        /// <summary>
        /// 增加工程项目
        /// </summary>
        /// <param name="model">工程项目实体</param>
        /// <returns></returns>
        public RetOb AddProjectInfo(ProjectInfo model)
        {
            lock (projectLock)//加锁防止并发操作“项目名称+项目代码”或项目代码重复
            {
                PrintEmptyLine();
                PrintMessage("开始调用AddProjectInfo()方法");
                RetOb retOb = new RetOb();
                if (model == null)
                {
                    return RetFailer("工程项目实体为空。");
                }
                else if (model.Name == null || model.Name.Trim().Equals(""))
                {
                    return RetFailer("工程项目名称为空。");
                }
                PrintMessage("工程项目名称：" + model.Name + "，工程项目代码：" + model.Code);

                try
                {
                    model.Name = model.Name.Trim();
                    if (model.Code != null)
                        model.Code = model.Code.Trim();

                    ObjectQuery oq = new ObjectQuery();
                    if (String.IsNullOrEmpty(model.Code))
                    {
                        oq.AddCriterion(Expression.Eq("Name", model.Name));
                        oq.AddCriterion(Expression.Or(Expression.IsNull("Code"), Expression.Eq("Code", "")));
                    }
                    else
                    {
                        oq.AddCriterion(Expression.Eq("Code", model.Code));//代码必须唯一
                    }
                    IList list = GetStockInSrv().ObjectQuery(typeof(CurrentProjectInfo), oq);
                    if (list == null || list.Count == 0)
                    {
                        CurrentProjectInfo currProject = this.projectToStandardProjectInfo(model, new CurrentProjectInfo());
                        GetStockInSrv().SaveCurrentProjectInfo(currProject);
                    }
                    else if (string.IsNullOrEmpty(model.Code))
                    {
                        return RetFailer("无项目部代码的项目名称必须唯一。");
                    }
                    else if (string.IsNullOrEmpty(model.Code) == false)
                    {
                        return RetFailer("项目代码必须唯一。");
                    }
                }
                catch (Exception ex)
                {
                    return RetFailer("结束调用AddProjectInfo()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                }
                PrintMessage("结束调用AddProjectInfo()方法," + getResultValue(retOb));
                return retOb;
            }
        }

        /// <summary>
        /// 更新工程项目
        /// </summary>
        /// <param name="model">工程项目实体</param>
        /// <returns></returns>
        public RetOb UpdateProjectInfo(ProjectInfo model)
        {
            PrintEmptyLine();
            PrintMessage("开始调用UpdateProjectInfo()方法");
            RetOb retOb = new RetOb();
            if (model == null)
            {
                return RetFailer("工程项目实体为空。");
            }
            if (model.Name == null || model.Name.Trim().Equals(""))
            {
                return RetFailer("工程项目名称为空。");
            }
            PrintMessage("工程项目名称：" + model.Name + "，工程项目代码：" + model.Code);

            try
            {
                if (model.Id == null || model.Id.Trim() == "")//如果没有ID直接新增
                {
                    PrintMessage("无项目ID,调用AddProjectInfo()方法添加项目。");
                    retOb = AddProjectInfo(model);
                    PrintMessage("结束调用UpdateProjectInfo()方法," + getResultValue(retOb));
                    return retOb;
                }
                else
                {
                    if (model.Code != null)
                        model.Code = model.Code.Trim();

                    CurrentProjectInfo updateProject = null;

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", model.Id));
                    IList list = GetStockInSrv().ObjectQuery(typeof(CurrentProjectInfo), oq);
                    if (list != null && list.Count > 0)
                        updateProject = list[0] as CurrentProjectInfo;
                    else
                        return RetFailer("根据项目ID没找到要修改的工程项目实体,该项目可能已被删除或者不存在。");

                    oq.Criterions.Clear();
                    //代码为null的情况下，如果修改了项目名称需要验证名称在没有代码的情况下唯一性，如果修改了项目代码为null也需要验证名称的唯一性
                    if ((updateProject.Name != model.Name || updateProject.Code != model.Code) && string.IsNullOrEmpty(model.Code))
                    {
                        oq.AddCriterion(Expression.Eq("Name", model.Name));
                        oq.AddCriterion(Expression.Or(Expression.IsNull("Code"), Expression.Eq("Code", "")));

                        list = GetStockInSrv().ObjectQuery(typeof(CurrentProjectInfo), oq);
                        if (list != null && list.Count > 0)
                            return RetFailer("无项目部代码的项目名称必须唯一。");
                    }
                    else if (updateProject.Code != model.Code && !string.IsNullOrEmpty(model.Code))//如果修改了代码且不为null需要验证项目代码的唯一性
                    {
                        oq.AddCriterion(Expression.Eq("Code", model.Code));//代码必须唯一

                        list = GetStockInSrv().ObjectQuery(typeof(CurrentProjectInfo), oq);
                        if (list != null && list.Count > 0)
                            return RetFailer("项目代码必须唯一。");
                    }

                    CurrentProjectInfo info = this.projectToStandardProjectInfo(model, updateProject);
                    GetStockInSrv().SaveCurrentProjectInfo(info);
                }
            }
            catch (Exception ex)
            {
                return RetFailer("结束调用UpdateProjectInfo()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用UpdateProjectInfo()方法," + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 删除工程项目
        /// </summary>
        /// <param name="model">工程项目实体</param>
        /// <returns></returns>
        public RetOb DeleteProjectInfo(ProjectInfo model)
        {
            PrintEmptyLine();
            PrintMessage("开始调用DeleteProjectInfo()方法");
            RetOb retOb = new RetOb();
            if (model == null)
            {
                return RetFailer("工程项目实体为空。");
            }
            if (string.IsNullOrEmpty(model.Id) && string.IsNullOrEmpty(model.Code))
            {
                return RetFailer("工程项目ID和代码必须保证一个不为空。");
            }

            PrintMessage("工程项目名称：" + model.Name + "，工程项目编码：" + model.Code);

            try
            {
                if (model.Id != null)
                    model.Id = model.Id.Trim();
                if (model.Code != null)
                    model.Code = model.Code.Trim();

                ObjectQuery oq = new ObjectQuery();
                if (!string.IsNullOrEmpty(model.Id))//如果ID不为空
                {
                    oq.AddCriterion(Expression.Eq("Id", model.Id));
                }
                else if (!string.IsNullOrEmpty(model.Id))//如果Code不为空
                {
                    oq.AddCriterion(Expression.Eq("Code", model.Code));
                }

                IList list = GetStockInSrv().ObjectQuery(typeof(CurrentProjectInfo), oq);

                if (list == null || list.Count == 0)
                {
                    return RetFailer("通过项目名称【" + model.Name + "】或项目代码【" + model.Code + "】未找到工程项目，该项目可能已被删除或者不存在。");
                }

                CurrentProjectInfo delProject = list[0] as CurrentProjectInfo;
                GetStockInSrv().DeleteCurrentProjectInfo(delProject);

            }
            catch (Exception ex)
            {
                return RetFailer("结束调用DeleteProjectInfo()方法，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用DeleteProjectInfo()方法，" + getResultValue(retOb));
            return retOb;
        }

        /// <summary>
        /// 根据工程项目名称模糊查询工程项目
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <returns></returns>
        public List<ProjectInfo> GetProjectInfoByName(string name)
        {
            List<ProjectInfo> rList = new List<ProjectInfo>();
            PrintEmptyLine();
            PrintMessage("开始调用GetProjectInfoByName()方法。");
            PrintMessage("工程项目名称：" + name);
            if (string.IsNullOrEmpty(name))
            {
                return rList;
            }

            try
            {
                name = name.Trim();
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Like("Name", name, MatchMode.Anywhere));

                IList list = GetStockInSrv().ObjectQuery(typeof(CurrentProjectInfo), oq);
                if (list != null)
                {
                    foreach (CurrentProjectInfo info in list)
                    {
                        ProjectInfo model = this.standProjectInfoToProject(info);
                        rList.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessage("结束调用GetProjectInfoByName()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用GetProjectInfoByName()方法，获取工程项目个数：" + rList.Count);
            return rList;
        }

        /// <summary>
        /// 根据工程项目代码查询工程项目
        /// </summary>
        /// <param name="code">项目代码</param>
        /// <returns></returns>
        public ProjectInfo GetProjectInfoByCode(string code)
        {
            ProjectInfo model = null;
            PrintEmptyLine();
            PrintMessage("开始调用GetProjectInfoByCode()方法。");
            PrintMessage("工程项目代码：" + code);
            if (string.IsNullOrEmpty(code))
            {
                return model;
            }

            try
            {
                code = code.Trim();
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Code", code));

                IList list = GetStockInSrv().ObjectQuery(typeof(CurrentProjectInfo), oq);
                if (list != null && list.Count > 0)
                {
                    CurrentProjectInfo info = list[0] as CurrentProjectInfo;
                    model = this.standProjectInfoToProject(info);
                }
            }
            catch (Exception ex)
            {
                PrintMessage("结束调用GetProjectInfoByCode()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用GetProjectInfoByCode()方法，获取项目对象：" + (model == null ? "NULL" : code));
            return model;
        }

        /// <summary>
        /// 根据工程项目名称和代码查询工程项目
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="code">项目代码</param>
        /// <returns></returns>
        public ProjectInfo GetProjectInfo(string name, string code)
        {
            ProjectInfo model = null;
            PrintEmptyLine();
            PrintMessage("开始调用GetProjectInfo()方法。");
            PrintMessage("工程项目名称：" + name + "，工程项目代码：" + code);
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(code))
            {
                return model;
            }


            try
            {
                ObjectQuery oq = new ObjectQuery();
                if (!string.IsNullOrEmpty(name))
                {
                    name = name.Trim();
                    oq.AddCriterion(Expression.Eq("Name", name));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    code = code.Trim();
                    oq.AddCriterion(Expression.Eq("Code", code));
                }
                else
                {
                    oq.AddCriterion(Expression.Or(Expression.IsNull("Code"), Expression.Eq("Code", "")));
                }

                IList list = GetStockInSrv().ObjectQuery(typeof(CurrentProjectInfo), oq);
                if (list != null && list.Count > 0)
                {
                    CurrentProjectInfo info = list[0] as CurrentProjectInfo;
                    model = this.standProjectInfoToProject(info);
                }
            }
            catch (Exception ex)
            {
                PrintMessage("结束调用GetProjectInfo()方法,异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            PrintMessage("结束调用GetProjectInfo()方法，获取项目对象：" + (model == null ? "NULL" : name));
            return model;
        }

        /// <summary>
        /// 项目管理[工程项目]转接口[工程项目]
        /// </summary>
        /// <returns></returns>
        private ProjectInfo standProjectInfoToProject(CurrentProjectInfo currProject)
        {
            ProjectInfo model = new ProjectInfo();
            model.Id = currProject.Id;
            model.Code = currProject.Code;
            model.Name = currProject.Name;
            model.CreateDate = currProject.CreateDate;
            model.ContractArea = currProject.ContractArea;
            model.ProjectLocationDescript = currProject.ProjectLocationDescript;
            model.ProjectLocationCity = currProject.ProjectLocationCity;
            model.ProjectLocationProvince = currProject.ProjectLocationProvince;
            model.UnderGroundLayers = currProject.UnderGroundLayers;
            model.GroundLayers = currProject.GroundLayers;
            model.BuildingArea = currProject.BuildingArea;
            model.BuildingHeight = currProject.BuildingHeight;
            model.HandlePersonName = currProject.HandlePersonName;
            model.OwnerOrgName = currProject.OwnerOrgName;
            model.ManagerDepart = currProject.ManagerDepart;
            model.QuanlityTarget = currProject.QuanlityTarget;
            model.SaftyTarget = currProject.SaftyTarget;
            model.AprroachDate = currProject.AprroachDate;
            model.BeginDate = currProject.BeginDate;
            model.EndDate = currProject.EndDate;
            model.ProjectCost = currProject.ProjectCost;
            return model;
        }
        /// <summary>
        /// 接口[工程项目]转项目管理[工程项目]
        /// </summary>
        /// <returns></returns>
        private CurrentProjectInfo projectToStandardProjectInfo(ProjectInfo model, CurrentProjectInfo currProject)
        {
            currProject.Name = model.Name;

            if (string.IsNullOrEmpty(model.Code))
            {
                currProject.OwnerOrg = null;
                currProject.OwnerOrgName = "";
                currProject.OwnerOrgSysCode = "";
            }
            else if (currProject.Code != model.Code)
            {
                OperationOrg ownOrg = GetOperationOrgByCode(model.Code);
                if (ownOrg != null)
                {
                    currProject.OwnerOrg = ownOrg;
                    currProject.OwnerOrgName = ownOrg.Name;
                    currProject.OwnerOrgSysCode = ownOrg.SysCode;
                }
            }
            currProject.Code = model.Code;

            //if (model.ContractArea != null && model.ContractArea != "")
            //{
            currProject.ContractArea = model.ContractArea;
            //}
            //if (model.ProjectLocationDescript != null && model.ProjectLocationDescript != "")
            //{
            currProject.ProjectLocationDescript = model.ProjectLocationDescript;
            //}
            //if (model.ProjectLocationCity != null && model.ProjectLocationCity != "")
            //{
            currProject.ProjectLocationCity = model.ProjectLocationCity;
            //}
            //if (model.ProjectLocationProvince != null && model.ProjectLocationProvince != "")
            //{
            currProject.ProjectLocationProvince = model.ProjectLocationProvince;
            //}
            //if (model.UnderGroundLayers != 0)
            //{
            currProject.UnderGroundLayers = model.UnderGroundLayers;
            //}
            //if (model.GroundLayers != 0)
            //{
            currProject.GroundLayers = model.GroundLayers;
            //}
            //if (model.BuildingArea != 0)
            //{
            currProject.BuildingArea = model.BuildingArea;
            //}
            //if (model.BuildingHeight != 0)
            //{
            currProject.BuildingHeight = model.BuildingHeight;
            //}
            //if (model.HandlePersonName != null && model.HandlePersonName != "")
            //{
            currProject.HandlePersonName = model.HandlePersonName;
            //}
            //if (model.ManagerDepart != null && model.ManagerDepart != "")
            //{
            currProject.ManagerDepart = model.ManagerDepart;
            //}
            //if (model.QuanlityTarget != null && model.QuanlityTarget != "")
            //{
            currProject.QuanlityTarget = model.QuanlityTarget;
            //}
            //if (model.SaftyTarget != null && model.SaftyTarget != "")
            //{
            currProject.SaftyTarget = model.SaftyTarget;
            //}
            if (model.AprroachDate != null && model.AprroachDate > DateTime.Parse("2000-01-01"))
            {
                currProject.AprroachDate = model.AprroachDate;
            }
            if (model.BeginDate != null && model.BeginDate > DateTime.Parse("2000-01-01"))
            {
                currProject.BeginDate = model.BeginDate;
            }
            if (model.EndDate != null && model.EndDate > DateTime.Parse("2000-01-01"))
            {
                currProject.EndDate = model.EndDate;
            }
            //if (model.ProjectCost != 0)
            //{
            currProject.ProjectCost = model.ProjectCost;
            //}


            return currProject;
        }
        #endregion

        /// <summary>
        /// 重新加载数据（xml到内存）
        /// </summary>
        /// <param name="validName"></param>
        /// <param name="validPwd"></param>
        /// <returns></returns>
        public bool SyncXMLData()
        {
            //if (validName != "administratorSystem" && validPwd != "1qaz@WSX")
            //    return false;

            return GetOperationJobService().ReloadIRPXML();
        }

        /// <summary>
        /// 获取是否自动同步XML数据到内存
        /// </summary>
        /// <returns></returns>
        public bool GetIsAutoSyncXMLData()
        {
            //三层，使用控制台的配置
            return GetOperationJobService().GetIsAutoSyncXMLData();
        }

        /// <summary>
        /// 设置是否自动同步XML数据到内存开关
        /// </summary>
        /// <param name="IsAutoSyncFlag"></param>
        /// <returns></returns>
        public bool SetAutoSyncXMLDataFlag(bool IsAutoSyncFlag)
        {
            return GetOperationJobService().SetAutoSyncXMLDataFlag(IsAutoSyncFlag);
        }

        //测试
        public void CallAppPlat(string stepGUID, string auditPersonName, string auditOpinion, string auditDate)
        {
            Console.WriteLine("stepGUID:  " + stepGUID + "---auditPersonName:  " + auditPersonName);
        }
    }
}
