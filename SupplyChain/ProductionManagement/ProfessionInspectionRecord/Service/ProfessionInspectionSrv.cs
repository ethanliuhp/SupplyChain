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
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Service
{
   

    /// <summary>
    /// 专业检查记录
    /// </summary>
    public class ProfessionInspectionSrv : BaseService, IProfessionInspectionSrv
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

        #region 专业检查记录
        /// <summary>
        /// 通过ID查询专业检查记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProfessionInspectionRecordMaster GetProfessionInspectionRecordById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetProfessionInspectionRecordPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ProfessionInspectionRecordMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询专业检查记录信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ProfessionInspectionRecordMaster GetProfessionInspectionRecordByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetProfessionInspectionRecordPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ProfessionInspectionRecordMaster;
            }
            return null;
        }

        /// <summary>
        /// 专业检查记录信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetProfessionInspectionRecordPlan(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ProfessionInspectionRecordMaster), objectQuery);
        }

        /// <summary>
        /// 专业检查记录信息引用查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetProfessionInspection(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ProfessionInspectionRecordDetail), objectQuery);
        }

        /// <summary>
        /// 专业检查记录信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet ProfessionInspectionRecordQuery(string condition)
        {
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT t1.Id,t1.Code,t1.State,t1.CreatePersonName,t1.RealOperationDate,t1.InspectionDate,t1.InspectionSpecail,t1.Descript,
t2.InspectionContent,t2.InspectionSituation,t2.InspectionSupplierName,t2.InspectionPersonName,t2.DangerLevel,t2.DangerPart,t2.DangerType,
t2.InspectionConclusion,t2.MeasureRequired,t2.CorrectiveSign FROM THD_ProfessionInspectionMaster  t1 INNER JOIN THD_ProfessionInspectionDetail
t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            dataReader = command.ExecuteReader();
                DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }


        [TransManager]
        public ProfessionInspectionRecordMaster SaveProfessionInspectionRecordPlan(ProfessionInspectionRecordMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ProfessionInspectionRecordMaster));
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as ProfessionInspectionRecordMaster;
        }

        /// <summary>
        /// 根据明细Id查询专业检查记录明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public ProfessionInspectionRecordDetail GetProfessionInspectionRecordDetailById(string ProDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", ProDtlId));
            IList list = dao.ObjectQuery(typeof(ProfessionInspectionRecordDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ProfessionInspectionRecordDetail;
            }
            return null;
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
    }





}
