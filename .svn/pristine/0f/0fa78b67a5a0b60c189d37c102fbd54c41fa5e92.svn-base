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
using Application.Business.Erp.SupplyChain.CostManagement.ConstructionDataMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ConstructionDataMng.Service
{
   

    /// <summary>
    /// 施工专业基础表
    /// </summary>
    public class ConstructionDataSrv : BaseService, IConstructionDataSrv
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

        #region 施工专业基础数据

        /// <summary>
        /// 施工专业基础数据信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetConstructionData(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ConstructionData), objectQuery);
        }

        /// <summary>
        /// 通过ID查询施工专业基础表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConstructionData GetConstructionDateById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetConstructionData(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ConstructionData;
            }
            return null;
        }

        /// <summary>
        /// 施工专业基础数据信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public IList GetConstructionDataBySerailNum(int SerailNum)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("SerailNum", SerailNum));
            return Dao.ObjectQuery(typeof(ConstructionData), objectQuery);
        }
        
        [TransManager]
        public ConstructionData SaveConstructionData(ConstructionData obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ConstructionData));
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as ConstructionData;
        }

        #endregion

    }
}
