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
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OBS.Service
{
   

    /// <summary>
    /// OBS服务
    /// </summary>
    public class OBSSrv : BaseService, IOBSSrv
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

        #region OBSService服务

        /// <summary>
        /// 通过ProjectTaskName查询服务OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetOBSServiceByName(string Name)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectTaskName", Name));
            return Dao.ObjectQuery(typeof(OBSService), objectQuery);
        }
        /// <summary>
        /// 通过ID查询服务OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OBSService GetOBSServiceById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetOBSService(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as OBSService;
            }
            return null;
        }

        /// <summary>
        /// OBS服务信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOBSService(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(OBSService), objectQuery);
        }


        [TransManager]
        public OBSService SaveOBSService(OBSService obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(OBSService));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as OBSService;
        }


        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }

        #endregion

        #region OBSManage管理

        /// <summary>
        /// 通过ID查询管理OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OBSManage GetOBSManageById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetOBSManage(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as OBSManage;
            }
            return null;
        }

        /// <summary>
        /// 通过ProjectTaskName查询管理OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetOBSManageByName(string Name)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectTaskName", Name));
            return Dao.ObjectQuery(typeof(OBSManage), objectQuery);
        }

        /// <summary>
        /// 通过ProjectTasId查询管理OBS信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetOBSManageByProjectTaskId(string Id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectTask", Id));
            return Dao.ObjectQuery(typeof(OBSManage), objectQuery);
        }


        /// <summary>
        /// OBS管理信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOBSManage(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(OBSManage), objectQuery);
        }



        [TransManager]
        public OBSManage SaveOBSManage(OBSManage obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(OBSManage));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as OBSManage;
        }



        #endregion

    }
}
