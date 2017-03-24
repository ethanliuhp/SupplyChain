using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
   

    /// <summary>
    /// 采购计划服务
    /// </summary>
    public class SupplyPlanSrv : BaseService, ISupplyPlanSrv
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

        #region 采购计划
        /// <summary>
        /// 通过ID查询采购信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SupplyPlanMaster GetSupplyPlanById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetSupplyPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SupplyPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询采购信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public SupplyPlanMaster GetSupplyPlanByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetSupplyPlan(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SupplyPlanMaster;
            }
            return null;
        }

        /// <summary>
        /// 采购计划信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetSupplyPlan(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(SupplyPlanMaster), objectQuery);
        }

        /// <summary>
        /// 采购计划信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet SupplyPlanQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id, t1.Code,t1.State,t1.PlanName,t1.PrintTimes,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,
t1.CreatePersonName,t1.CreateDate,t1.Descript,t2.UsedPartName,t1.SpecialType,t2.QualityStandard,t2.Quantity,t2.DiagramNumber,t2.TechnologyParameter
FROM THD_SupplyPlanMaster  t1 INNER JOIN THD_SupplyPlanDetail  t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        [TransManager]
        public SupplyPlanMaster SaveSupplyPlan(SupplyPlanMaster obj, IList movedDtlList)
        {

            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                obj.RealOperationDate = System.DateTime.Now;//添加制单时间
                obj.Code = GetCode(typeof(SupplyPlanMaster), obj.ProjectId, obj.SpecialType);
                obj = SaveByDao(obj) as SupplyPlanMaster;
                //新增时修改前续单据的引用数量
                foreach (SupplyPlanDetail dtl in obj.Details)
                {
                    DemandMasterPlanDetail forwardDtl = GetSupplyDetailById(dtl.ForwardDetailId);
                    forwardDtl.SupplyLeftQuantity = forwardDtl.SupplyLeftQuantity + Math.Abs(dtl.Quantity);//RefQuery为引用数量
                    forwardDtl.DemandLeftQuantity = forwardDtl.Quantity - forwardDtl.SupplyLeftQuantity;//需求剩余数量
                    dao.Save(forwardDtl);
                }
            }
            else
            {
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveOrUpdateByDao(obj) as SupplyPlanMaster;
                foreach (SupplyPlanDetail dtl in obj.Details)
                {
                    DemandMasterPlanDetail forwardDtl = GetSupplyDetailById(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        forwardDtl.SupplyLeftQuantity = forwardDtl.SupplyLeftQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                        forwardDtl.DemandLeftQuantity = forwardDtl.Quantity - forwardDtl.SupplyLeftQuantity;//需求剩余数量
                    }
                    else
                    {
                        forwardDtl.SupplyLeftQuantity = forwardDtl.SupplyLeftQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                        forwardDtl.DemandLeftQuantity = forwardDtl.Quantity - forwardDtl.SupplyLeftQuantity;//需求剩余数量
                    }
                    dao.Save(forwardDtl);
                }

                //修改时对于删除的明细 删除引用数量
                foreach (SupplyPlanDetail dtl in movedDtlList)
                {
                    DemandMasterPlanDetail forwardDtl = GetSupplyDetailById(dtl.ForwardDetailId);
                    forwardDtl.SupplyLeftQuantity = forwardDtl.SupplyLeftQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }
            return obj;
        }

        public DemandMasterPlanDetail GetSupplyDetailById(string supplyPlanDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", supplyPlanDtlId));
            IList list = dao.ObjectQuery(typeof(DemandMasterPlanDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as DemandMasterPlanDetail;
            }
            return null;
        }

        [TransManager]
        public bool DeleteSupplyPlan(SupplyPlanMaster obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (SupplyPlanDetail dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    DemandMasterPlanDetail forwardDtl = GetSupplyDetailById(dtl.ForwardDetailId);
                    forwardDtl.SupplyLeftQuantity = forwardDtl.SupplyLeftQuantity - Math.Abs(dtl.Quantity);
                    forwardDtl.DemandLeftQuantity = forwardDtl.Quantity - forwardDtl.SupplyLeftQuantity;
                    dao.Save(forwardDtl);
                }
            }
            return dao.Delete(obj);
        }

        #endregion
    }





}
