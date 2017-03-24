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
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Service
{
    /// <summary>
    /// 管理人员日志信息服务
    /// </summary>
    public class PersonManageSrv : BaseService, IPersonManageSrv
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

        #region 管理人员日志信息
        /// <summary>
        /// 通过ID查询管理人员日志信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PersonManage GetPersonManageById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetPersonManage(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as PersonManage;
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
        /// 通过Code查询管理人员日志信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public PersonManage GetPersonManageByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetPersonManage(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as PersonManage;
            }
            return null;
        }

        /// <summary>
        /// 查询管理人员日志信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetPersonManage(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("WeatherGlass", NHibernate.FetchMode.Eager);
            objectQuery.AddOrder(Order.Desc("CreateDate"));
            return Dao.ObjectQuery(typeof(PersonManage), objectQuery);
        }

        [TransManager]
        public PersonManage SavePersonManage(PersonManage obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(PersonManage));
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as PersonManage;
        }
        #endregion
    }
}
