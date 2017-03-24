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
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Service
{
    /// <summary>
    /// 施工日志信息服务
    /// </summary>
    public class ConstructionSrv : BaseService, IConstructionSrv
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

        #region 施工日志信息
        /// <summary>
        /// 通过ID查询施工日志信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConstructionManage GetConstructionById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetConstruction(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConstructionManage;
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
        /// 通过Code查询施工日志信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ConstructionManage GetConstructionByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetConstruction(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConstructionManage;
            }
            return null;
        }

        /// <summary>
        /// 查询施工日志信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetConstruction(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("WeatherGlass", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ConstructionManage), objectQuery);
        }

        /// <summary>
        /// 查询施工日志信息List
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        [TransManager]
        public IList GetConstructionList(CurrentProjectInfo projectInfo,DateTime strDate)
        {
            ConstructionManage manage = new ConstructionManage();
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Ge("CreateDate", strDate));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", strDate.AddDays(1).Date));
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objectQuery.AddFetchMode("WeatherGlass", NHibernate.FetchMode.Eager);
            //objectQuery.AddCriterion(Expression.Eq("OperOrgInfo.Id", (CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login).TheOperationOrgInfo.Id));
            //objectQuery.AddCriterion(Expression.Eq("OperOrgInfoName", ConstObject.TheLogin.TheOperationOrgInfo.Name));
            IList SearchList = Dao.ObjectQuery(typeof(PersonManage), objectQuery);
           
            IList ResultList = new ArrayList();
            string strPart = null;
            string strWork = null;
            string strTechnology = null;
            string strQuality = null;
            string strSafty = null;

            string str1 = null;
            string str2 = null;
            string str3 = null;
            string str4 = null;
            string str5 = null;
            string str6 = null;
            string str7 = null;
            string str8 = null;
            foreach (PersonManage person in SearchList)
            {
                //主要施工部位
                if (person.ConstructSite != null)
                {
                    strPart += person.ConstructSite + ";";
                }
                //生产情况
                if (person.Post.Equals("施工员"))
                {
                    if (person.MainWork != null)
                    {
                        strWork += person.MainWork + ";";
                    }
                }
                #region 获得质量安全工作记录
                if (person.Post.Equals("技术员"))
                {
                    if (person.MainWork != null)
                    {
                        str1 += person.MainWork + ";";
                    }
                    if(person.ProjectManage != null)
                    {
                        str1 += person.ProjectManage + ";";
                    }
                    if (person.OtherActivities != null)
                    {
                        str1 += person.OtherActivities + ";";
                    }
                    if (person.Problem != null)
                    {
                        str1 += person.Problem + ";";
                    }
                }
                if (person.Post.Equals("材料员"))
                {
                    if (person.MainWork != null)
                    {
                        str3 += person.MainWork + ";";
                    }
                    if (person.OtherActivities != null)
                    {
                        str3 += person.OtherActivities + ";";
                    }
                    if (person.Problem != null)
                    {
                        str3 += person.Problem + ";";
                    }
                }
                if (person.Post.Equals("质检员"))
                {
                    if (person.MainWork != null)
                    {
                        str4 += person.MainWork + ";";
                    }
                    if (person.OtherActivities != null)
                    {
                        str4 += person.OtherActivities + ";";
                    }
                    if (person.Problem != null)
                    {
                        str4 += person.Problem + ";";
                    }
                }
                if (person.Post.Equals("试验员"))
                {
                    if (person.MainWork != null)
                    {
                        str5 += person.MainWork + ";";
                    }
                    if (person.OtherActivities != null)
                    {
                        str5 += person.OtherActivities + ";";
                    }
                    if (person.Problem != null)
                    {
                        str5 += person.Problem + ";";
                    }
                }
                if (person.Post.Equals("安全员"))
                {
                    if (person.MainWork != null)
                    {
                        str6 += person.MainWork + ";";
                    }
                    if (person.OtherActivities != null)
                    {
                        str6 += person.OtherActivities + ";";
                    }
                    if (person.Problem != null)
                    {
                        str6 += person.Problem + ";";
                    }
                }
                #endregion 
            }
            if (strPart != null)
            {
                manage.ConstructSite = strPart.Substring(0, strPart.Length - 1);//主要施工部位
            }
            if (strWork != null)
            {
                manage.ProductionRecord = strWork.Substring(0, strWork.Length - 1);//生产情况记录
            }
            IList TechnologyLit = new ArrayList();
            IList QualityList = new ArrayList();
            IList SaftyList = new ArrayList();
            //组合技术质量安全工作记录
            if (str1 != null) TechnologyLit.Add(str1);
            if (str3 != null) TechnologyLit.Add(str3);
            if (str4 != null) QualityList.Add(str4);
            if (str5 != null) QualityList.Add(str5);
            if (str6 != null) SaftyList.Add(str6);
            int i = TechnologyLit.Count;
            for(int j = 0; j < i; j ++)
            {
                strTechnology += (j + 1) + "." + TechnologyLit[j].ToString() + "\r\n";
            }
            if (strTechnology != null)
            {
                manage.WorkRecord = strTechnology;//技术工作记录
            }
            int k = QualityList.Count;
            for (int j = 0; j < k; j++)
            {
                strQuality += (j + 1) + "." + QualityList[j].ToString() + "\r\n";
            }
            if (strQuality != null)
            {
                manage.QualityWorkRecord = strQuality;//质量工作记录
            }
            int l = SaftyList.Count;
            for (int j = 0; j < l; j++)
            {
                strSafty += (j + 1) + "." + SaftyList[j].ToString() + "\r\n";
            }
            if (strSafty != null)
            {
                manage.SaftyWorkRecord = strSafty;//技术质量安全工作记录
            }
            ResultList.Add(manage);
            return ResultList;
        }

        [TransManager]
        public ConstructionManage SaveConstruction(ConstructionManage obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ConstructionManage));
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as ConstructionManage;
        }
        #endregion
    }
}
