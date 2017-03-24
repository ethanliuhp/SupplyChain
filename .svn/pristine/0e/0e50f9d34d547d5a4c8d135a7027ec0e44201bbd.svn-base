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
using Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Service
{
   

    /// <summary>
    /// 验收检查记录
    /// </summary>
    public class AcceptanceInspectionSrv : BaseService, IAcceptanceInspectionSrv
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

        private string GetCode(Type type,string specail)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, specail);
        }

        /// <summary>
        /// 根据项目 物资分类(专业分类) 生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <param name="matCatAbb"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId, string matCatAbb)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId, matCatAbb);
        }
        #endregion

        #region 验收检查记录
        /// <summary>
        /// 通过ID查询验收检查记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AcceptanceInspection GetAcceptanceInspectionById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetAcceptanceInspection(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as AcceptanceInspection;
            }
            return null;
        }


        /// <summary>
        /// 通过检验批GUID查询验收检查记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetAcceptanceInspectionByInsLotGUID(InspectionLot id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("InsLotGUID", id));
            IList list = GetAcceptanceInspection(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询验收检查记录信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AcceptanceInspection GetAcceptanceInspectionByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetAcceptanceInspection(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as AcceptanceInspection;
            }
            return null;
        }

        /// <summary>
        /// 验收检查记录信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetAcceptanceInspection(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(AcceptanceInspection), objectQuery);
        }

        /// <summary>
        /// 验收检查记录信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet AcceptanceInspectionQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT * FROM THD_AcceptanceInspection t1";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }


        [TransManager]
        public AcceptanceInspection SaveAcceptanceInspection(AcceptanceInspection obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(AcceptanceInspection));
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            //保存检验批中的验收状态
            InspectionLot InsDtl = obj.InsLotGUID;
            InsDtl.AccountStatus = obj.InsTemp;
            dao.SaveOrUpdate(InsDtl);
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as AcceptanceInspection;
        }

        #endregion
    }





}
