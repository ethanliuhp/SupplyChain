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
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Service
{
    /// <summary>
    /// 晴雨表信息服务
    /// </summary>
    public class WeatherSrv : BaseService, IWeatherSrv
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

        #region 晴雨表信息
        /// <summary>
        /// 通过ID查询晴雨表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WeatherInfo GetWeatherById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetWeather(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as WeatherInfo;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询晴雨表信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WeatherInfo GetWeatherByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetWeather(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as WeatherInfo;
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

        [TransManager]
        public WeatherInfo SaveWeather(WeatherInfo obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(WeatherInfo));
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as WeatherInfo;
        }
        #endregion

        #region 二维码

        #endregion
    }
}
