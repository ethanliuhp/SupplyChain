using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using Application.Resource.CommonClass.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Service
{
    /// <summary>
    /// 日施工情况
    /// </summary>
    public class ConstructionReportSrv : BaseService, IConstructionReportSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }
        #endregion

        #region 日施工情况
        /// <summary>
        /// 通过ID查询日施工情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConstructReport GetConstructReportById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetConstructReport(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConstructReport;
            }
            return null;
        }
        /// <summary>
        /// 查询晴雨信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetWeather(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(WeatherInfo), objectQuery);
        }

        /// <summary>
        /// 通过Code查询日施工情况
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ConstructReport GetConstructReportByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetConstructReport(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConstructReport;
            }
            return null;
        }

        /// <summary>
        /// 查询日施工情况
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetConstructReport(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("WeatherGlass", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ConstructReport), objectQuery);
        }

        /// <summary>
        /// 查询日施工情况List
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        [TransManager]
        public IList GetConstructReportList(CurrentProjectInfo projectInfo ,DateTime dt)
        {
            ConstructReport manage = new ConstructReport();
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Ge("CreateDate", dt));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", dt.AddDays(1).Date));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objectQuery.AddFetchMode("WeatherGlass", NHibernate.FetchMode.Eager);
            objectQuery.AddCriterion(Expression.Eq("OperOrgInfo.Id", (CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login).TheOperationOrgInfo.Id));
            IList SearchList = Dao.ObjectQuery(typeof(PersonManage), objectQuery);
            IList ResultList = new ArrayList();
            if (SearchList.Count > 0)
            {
                string strPart = null;//主要施工部位
                string strWork = null;//工作内容及进度完成情况
                string strManage = null;//项目管理活动
                string strMaterial = null;//材料相关情况
                string strSafty = null;//生产安全控制情况
                string strActive = null;//其它活动情况
                string strProblem = null;//存在的问题

                foreach (PersonManage person in SearchList)
                {
                    if (person.ConstructSite != null)
                    {
                        strPart += person.ConstructSite + ";";
                    }
                    if (person.Post.Equals("施工员"))
                    {
                        if (person.MainWork != null)
                        {
                            strWork += person.MainWork + ";";
                        }
                    }
                    if (person.Post.Equals("技术员"))
                    {
                        if (person.ProjectManage != null)
                        {
                            strManage += person.ProjectManage + ";";
                        }
                    }
                    if (person.Post.Equals("材料员"))
                    {
                        if (person.MainWork != null)
                        {
                            strMaterial += person.MainWork + ";";
                        }
                    }
                    if (person.Post.Equals("安全员"))
                    {
                        if (person.MainWork != null)
                        {
                            strSafty += person.MainWork + ";";
                        }
                    }
                    if (person.OtherActivities != null)
                    {
                        strActive += person.OtherActivities + ";";
                    }
                    if (person.Problem != null)
                    {
                        strProblem += person.Problem + ";";
                    }
                }
                if (strPart != null)
                {
                    manage.ConstructSite = strPart.Substring(0, strPart.Length - 1);//主要施工部位
                }
                if (strWork != null)
                {
                    manage.CompletionSchedule = strWork.Substring(0, strWork.Length - 1);//工作内容及进度完成情况
                }
                if (strManage != null)
                {
                    manage.ProjectManage = strManage.Substring(0, strManage.Length - 1);//项目管理活动
                }
                if (strMaterial != null)
                {
                    manage.MaterialCase = strMaterial.Substring(0, strMaterial.Length - 1);//材料相关情况
                }
                if (strSafty != null)
                {
                    manage.SafetyControl = strSafty.Substring(0, strSafty.Length - 1);//生产安全控制情况
                }
                if (strActive != null)
                {
                    manage.OtherActivities = strActive.Substring(0, strActive.Length - 1);//其它活动情况
                }
                if (strProblem != null)
                {
                    manage.Problem = strProblem.Substring(0, strProblem.Length - 1);//存在的问题
                }
                ResultList.Add(manage);
            }
            return ResultList;
                
        }

        [TransManager]
        public ConstructReport SaveConstructReport(ConstructReport obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ConstructReport));
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as ConstructReport;
        }
        #endregion
    }
}
