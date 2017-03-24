using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;
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
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.WasteMaterialManage.Service
{
   

    /// <summary>
    /// 废旧物资申请服务
    /// </summary>
    public class WasteMatSrv : BaseService, IWasteMatSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private string GetCode(Type type,string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }
        #endregion

        #region 废旧物料申请
        /// <summary>
        /// 通过ID查询废旧物料申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WasteMatApplyMaster GetWasteMatApplyById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetWasteMatApply(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as WasteMatApplyMaster;
            }
            return null;
        }


        /// <summary>
        /// 通过Code查询废旧物料申请信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WasteMatApplyMaster GetWasteMatApplyByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetWasteMatApply(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as WasteMatApplyMaster;
            }
            return null;
        }


        public IList GetWasteMaterialProcessHandle(ObjectQuery oq)
        {
            oq.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(WasteMatProcessMaster), oq);
        }


        /// <summary>
        /// 通过Id查询废旧物料处理信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public WasteMatProcessMaster GetWasteMatHandleById(string Id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", Id));


            IList list = GetWasteMaterialProcessHandle(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as WasteMatProcessMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询废旧物料处理信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WasteMatProcessMaster GetWasteMatHandleByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetWasteMatHandle(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as WasteMatProcessMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询废旧物料申请信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetWasteMatApply(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(WasteMatApplyMaster), objectQuery);
        }

        /// <summary>
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item)
        {
            dao.SaveOrUpdate(item);

            return item;
        }

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            bool flag = false;
            foreach (ProObjectRelaDocument cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    dis.Add(Expression.Eq("Id", cg.Id));
                    flag = true;
                }
            }
            if (flag)
            {
                oq.AddCriterion(dis);

                IList listTemp = dao.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (listTemp != null && listTemp.Count > 0)
                    dao.Delete(listTemp);
            }

            return true;
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

        /// <summary>
        /// 查询废旧物料申请处理信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetWasteMatHandle(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(WasteMatProcessMaster), objectQuery);
        }

        /// <summary>
        /// 废旧物料申请信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet WasteMatApplyQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.code,t1.State,t1.PrintTimes,t1.RealOperationDate,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.Quantity,t2.MatStandardUnitName,t2.Grade,t2.ApplyDate,t2.Descript,t1.CreatePersonName,t1.CreateDate,t2.UsedPartName,t2.DiagramNumber FROM THD_WasteMatApplyDetail t2 INNER JOIN THD_WasteMatApplyMaster t1 ON t2.ParentId = t1.Id";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 废旧物料处理信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet WasteMatHandleQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.State,t1.PrintTimes,t1.Descript,t1.RealOperationDate,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.NetWeight,t2.GrossWeight,t2.TareWeight,
t2.ProcessPrice,t2.TotalValue,t2.PlateNumber,t2.ReceiptCode,t1.CreatePersonName,t1.CreateDate,t1.RecycleUnitName,t2.UsedPartName,t2.DiagramNumber
FROM THD_WasteMatProcessMaster t1 INNER JOIN THD_WasteMatProcessDetail t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }



        [TransManager]
        public WasteMatApplyMaster saveWasteMatApply(WasteMatApplyMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(WasteMatApplyMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as WasteMatApplyMaster;
        }

        //[TransManager]
        //public WasteMatProcessMaster saveWasteMatHandle(WasteMatProcessMaster obj)
        //{
        //    if (obj.Id == null)
        //    {
        //        obj.Code = GetCode(typeof(WasteMatProcessMaster));
        //    }
        //    obj.LastModifyDate = DateTime.Now;
        //    return SaveOrUpdateByDao(obj) as WasteMatProcessMaster;
        //}

        [TransManager]
        public WasteMatProcessMaster saveWasteMatProcess(WasteMatProcessMaster obj, IList movedDtlList)
        {

            obj.LastModifyDate = DateTime.Now;
            if (obj.Id == null)
            {
                obj.RealOperationDate = DateTime.Now;
                obj.Code = GetCode(typeof(WasteMatProcessMaster), obj.ProjectId);
                obj = SaveByDao(obj) as WasteMatProcessMaster;
                //新增时修改前续单据的引用数量
                foreach (WasteMatProcessDetail dtl in obj.Details)
                {
                    WasteMatApplyDetail forwardDtl = GetWasteProcessDetailById(dtl.ForwardDetailId);
                    forwardDtl.LeftQuantity = forwardDtl.LeftQuantity + dtl.NetWeight;//RefQuery为引用数量

                    dao.Save(forwardDtl);
                }
            }
            else
            {
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveOrUpdateByDao(obj) as WasteMatProcessMaster;
                foreach (WasteMatProcessDetail dtl in obj.Details)
                {
                    WasteMatApplyDetail forwardDtl = GetWasteProcessDetailById(dtl.ForwardDetailId);
                    if (dtl.Id == null)
                    {
                        forwardDtl.LeftQuantity = forwardDtl.LeftQuantity + dtl.NetWeight - dtl.QuantityTemp;
                    }
                    else
                    {
                        forwardDtl.LeftQuantity = forwardDtl.LeftQuantity + dtl.NetWeight - dtl.QuantityTemp;
                    }
                    dao.Save(forwardDtl);
                }

                //修改时对于删除的明细 删除引用数量
                foreach (WasteMatProcessDetail dtl in movedDtlList)
                {
                    WasteMatApplyDetail forwardDtl = GetWasteProcessDetailById(dtl.ForwardDetailId);
                    forwardDtl.LeftQuantity = forwardDtl.LeftQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }
            return obj;
        }

        /// <summary>
        /// 根据明细Id查询废旧物资申请单明细
        /// </summary>
        /// <param name="stockOutDtlId"></param>
        /// <returns></returns>
        public WasteMatApplyDetail GetWasteProcessDetailById(string wasteProcessDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", wasteProcessDtlId));
            IList list = dao.ObjectQuery(typeof(WasteMatApplyDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as WasteMatApplyDetail;
            }
            return null;
        }


        [TransManager]
        public bool DeleteWasteMaterialProcessMaster(WasteMatProcessMaster obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (WasteMatProcessDetail dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    WasteMatApplyDetail forwardDtl = GetWasteProcessDetailById(dtl.ForwardDetailId);
                    forwardDtl.LeftQuantity = forwardDtl.LeftQuantity - Math.Abs(dtl.Quantity);
                    dao.Save(forwardDtl);
                }
            }
            return dao.Delete(obj);
        }

        #endregion

        /// <summary>
        /// 得到处理申请单制单日期
        /// </summary>
        /// <returns></returns>
        public DateTime GetWasterSQCreateDate(string id)
        {
            DateTime dt = TransUtil.ToDateTime("2000-01-01");
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select t1.createdate from thd_wastematapplymaster t1 where t1.id='" + id + "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    dt = TransUtil.ToDateTime(dataRow["createdate"]);
                }
            }
            return dt;
        }
    }


}
