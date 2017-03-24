using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireSupply.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using CommonSearchLib.BillCodeMng.Service;
using NHibernate;
using VirtualMachine.Component.Util;
using VirtualMachine.Core.DataAccess;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Util;
using NHibernate.Criterion;
using System.Runtime.Remoting.Messaging;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.Service
{
    /// <summary>
    /// 采购合同服务
    /// </summary>
    public class MaterialHireSupplyOrderSrv : BaseService, IMaterialHireSupplyOrderSrv
    {
        private IAppByBusinessSrv refAppByBusinessSrv;

        public IAppByBusinessSrv RefAppByBusinessSrv
        {
            get { return refAppByBusinessSrv; }
            set { refAppByBusinessSrv = value; }
        }

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
        private string GetCode(Type type, string specail)
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
        ///获取确认单价
        /// </summary>
        /// <param name="sAccountSysCode">所属组织的核算的节点的syscode</param>
        /// <param name="sProjectID">当前项目的ID</param>
        /// <param name="sDiagram">图号</param>
        /// <param name="sMaterialID">物资ID</param>
        /// <returns></returns>
        public decimal GetConfirmPrice(string sAccountSysCode, string sProjectID, string sDiagram, string sMaterialID)
        {

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            decimal dPrice = 0;
            string sql =
                @"select decode(TotalCount,0,0,round(TotalSum/TotalCount,4))from (
                select nvl(sum(t2.contractprojectamount),0) TotalCount, nvl(sum(t2.CONTRACTTOTALPRICE),0) TotalSum from thd_gwbstree t
                join thd_gwbsdetail t1 on t.id=t1.parentid 
                join THD_GWBSDETAILCOSTSUBJECT t2 on t1.id=t2.GWBSDETAILID
                where instr(t.syscode ,GetProjectTaskSysCode('{0}','{1}'))=1 and   {2} 
                and t2.RESOURCETYPEGUID='{3}' and t2.CONTRACTTOTALPRICE!=0)";
            sql = string.Format(sql, sAccountSysCode, sProjectID, string.IsNullOrEmpty(sDiagram) ? " t2.diagramnumber is null " : " t2.diagramnumber='" + sDiagram + "'  ", sMaterialID);
            command.CommandText = sql;
            object obj = command.ExecuteScalar();
            if (obj != null)
            {
                dPrice = ClientUtil.ToDecimal(obj);
            }
            //DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            //return dataSet;
            return dPrice;
            // return 0;
        }
        public decimal GetConfirmPrice(string sDemandPlanDetailID)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            decimal dPrice = 0;
            string sql =
                @"select nvl(t.price,0)  price from thd_demandmasterplandetail t where id='{0}'";
            sql = string.Format(sql, sDemandPlanDetailID);
            command.CommandText = sql;
            object obj = command.ExecuteScalar();
            if (obj != null)
            {
                dPrice = ClientUtil.ToDecimal(obj);
            }
            //DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            //return dataSet;
            return dPrice;
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

        #region 采购合同(公司)
        /// <summary>
        /// 通过ID查询采购合同信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MatHireSupplyOrderMaster GetSupplyOrderByIdCompany(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetSupplyOrderCompany(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireSupplyOrderMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询采购合同信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MatHireSupplyOrderMaster GetSupplyOrderByCodeCompany(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));

            IList list = GetSupplyOrderCompany(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireSupplyOrderMaster;
            }
            return null;
        }

        /// <summary>
        /// 采购合同信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetSupplyOrderCompany(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("PaymentDetails", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("ProjectDetails", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MatHireSupplyOrderMaster), objectQuery);
        }

        /// <summary>
        /// 采购合同信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet SupplyOrderQueryCompany(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.State,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,t2.SpecialType,
                t1.CreatePersonName,t1.CreateDate,t1.OldContractNum,t2.Quantity,t2.SupplyPrice,t2.Money,t1.Descript,t1.SupplierName,t1.ContractMoney,t2.confirmprice
                FROM THD_SupplyOrderMaster  t1 INNER JOIN THD_SupplyOrderDetail  t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 保存采购合同(公司)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public MatHireSupplyOrderMaster SaveSupplyOrderCompany(MatHireSupplyOrderMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(MatHireSupplyOrderMaster));
                //obj.Code = GetCode(typeof(SupplyOrderMaster), obj.Special);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as MatHireSupplyOrderMaster;
        }

        /// <summary>
        /// 删除采购合同(公司)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteSupplyOrderCompany(MatHireSupplyOrderMaster obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            return dao.Delete(obj);
        }
        #endregion


        #region 采购合同
        /// <summary>
        /// 通过ID查询采购合同信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MatHireSupplyOrderMaster GetSupplyOrderById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetSupplyOrder(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireSupplyOrderMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询采购合同信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MatHireSupplyOrderMaster GetSupplyOrderByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));

            IList list = GetSupplyOrder(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireSupplyOrderMaster;
            }
            return null;
        }

        /// <summary>
        /// 采购合同信息查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetSupplyOrder(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("PaymentDetails", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MatHireSupplyOrderMaster), objectQuery);
        }

        /// <summary>
        /// 采购合同信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet SupplyOrderQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.Code,t1.State,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.MatStandardUnitName,t2.SpecialType,
                t1.CreatePersonName,t1.CreateDate,t1.RealOperationDate,t1.OldContractNum,t2.Quantity,t2.SupplyPrice,t2.Money,t1.Descript,t1.SupplierName,t1.ContractMoney,t2.confirmprice,t2.DiagramNumber,t2.BRAND,t2.TECHNOLOGYPARAMETER  
                FROM THD_SupplyOrderMaster  t1 INNER JOIN THD_SupplyOrderDetail  t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        [TransManager]
        public MatHireSupplyOrderMaster AddSupplyOrder(MatHireSupplyOrderMaster obj, IList movedDtlList)
        {
            obj.LastModifyDate = DateTime.Now;

            if (obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(MatHireSupplyOrderMaster), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                    obj = SaveByDao(obj) as MatHireSupplyOrderMaster;
                    foreach (MatHireSupplyOrderDetail dtl in obj.Details)
                    {
                        if (TransUtil.ToString(dtl.ForwardDetailId) != "")
                        {
                            DemandMasterPlanDetail forwardDtl = GetDemandDetailById(dtl.ForwardDetailId);
                            forwardDtl.SupplyLeftQuantity = forwardDtl.SupplyLeftQuantity - Math.Abs(dtl.Quantity);//剩余数量
                            dao.Save(forwardDtl);
                        }
                    }
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(MatHireSupplyOrderMaster), obj.ProjectId, obj.Special);
                    obj = SaveByDao(obj) as MatHireSupplyOrderMaster;
                    //新增时修改前续单据的引用数量
                    foreach (MatHireSupplyOrderDetail dtl in obj.Details)
                    {
                        if (TransUtil.ToString(dtl.ForwardDetailId) != "")
                        {
                            SupplyPlanDetail forwardDtl = GetSupplyDetailById(dtl.ForwardDetailId);
                            forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity);//引用数量
                            forwardDtl.LeftQuantity = forwardDtl.Quantity - forwardDtl.RefQuantity;//剩余数量
                            dao.Save(forwardDtl);
                        }
                    }
                }
                obj.RealOperationDate = DateTime.Now;
            }
            else
            {
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    obj.SubmitDate = DateTime.Now;
                }
                obj = SaveOrUpdateByDao(obj) as MatHireSupplyOrderMaster;
                if (obj.Special == "土建")
                {
                    foreach (MatHireSupplyOrderDetail dtl in obj.Details)
                    {
                        DemandMasterPlanDetail forwardDtl = GetDemandDetailById(dtl.ForwardDetailId);
                        if (forwardDtl == null) continue;
                        forwardDtl.SupplyLeftQuantity = forwardDtl.SupplyLeftQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                        //forwardDtl.SupplyLeftQuantity = Math.Abs(dtl.Quantity) - forwardDtl.SupplyLeftQuantity + Math.Abs(dtl.QuantityTemp);
                        dao.Save(forwardDtl);
                    }

                    //修改时对于删除的明细 删除引用数量
                    foreach (DemandMasterPlanDetail dtl in movedDtlList)
                    {
                        DemandMasterPlanDetail forwardDtl = GetDemandDetailById(dtl.ForwardDetailId);
                        forwardDtl.SupplyLeftQuantity = forwardDtl.SupplyLeftQuantity - Math.Abs(dtl.Quantity);
                        //forwardDtl.LeftQuantity = forwardDtl.Quantity + Math.Abs(dtl.Quantity);
                        dao.Save(forwardDtl);
                    }
                }
                else if (obj.Special == "安装")
                {
                    foreach (MatHireSupplyOrderDetail dtl in obj.Details)
                    {
                        SupplyPlanDetail forwardDtl = GetSupplyDetailById(dtl.ForwardDetailId);
                        if (forwardDtl == null) continue;
                        //SupplyPlanDetail forwardDtl = GetSupplyDetailById(obj.ForwardBillId);
                        if (dtl.Id == null)
                        {
                            forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                            forwardDtl.LeftQuantity = forwardDtl.Quantity - forwardDtl.RefQuantity;//剩余数量
                        }
                        else
                        {
                            forwardDtl.RefQuantity = forwardDtl.RefQuantity + Math.Abs(dtl.Quantity) - Math.Abs(dtl.QuantityTemp);
                            forwardDtl.LeftQuantity = forwardDtl.Quantity - forwardDtl.RefQuantity;//剩余数量
                        }
                        dao.Save(forwardDtl);
                    }

                    //修改时对于删除的明细 删除引用数量
                    foreach (SupplyPlanDetail dtl in movedDtlList)
                    {
                        SupplyPlanDetail forwardDtl = GetSupplyDetailById(dtl.ForwardDetailId);
                        forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                        forwardDtl.LeftQuantity = forwardDtl.Quantity + Math.Abs(dtl.Quantity);
                        dao.Save(forwardDtl);
                    }
                }
            }
            return obj;
        }

        [TransManager]
        public MatHireSupplyOrderMaster SaveSupplyOrder(MatHireSupplyOrderMaster obj)
        {
            if (obj.Id == null)
            {
                // obj.Code = GetCode(typeof(SupplyOrderMaster));
                GetCode(obj);
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MatHireSupplyOrderMaster;
        }

        public SupplyPlanDetail GetSupplyDetailById(string supplyPlanDtlId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", supplyPlanDtlId));
            IList list = dao.ObjectQuery(typeof(SupplyPlanDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as SupplyPlanDetail;
            }
            return null;
        }

        public DemandMasterPlanDetail GetDemandDetailById(string supplyPlanDtlId)
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

        //[TransManager]
        //public SupplyOrderMaster SaveSupplyOrder(SupplyOrderMaster obj)
        //{
        //    if (obj.Id == null)
        //    {
        //        //obj.Code = GetCode(typeof(SupplyOrderMaster));
        //        obj.Code = GetCode(typeof(SupplyOrderMaster), obj.Special);
        //    }
        //    obj.LastModifyDate = DateTime.Now;
        //    return SaveOrUpdateByDao(obj) as SupplyOrderMaster;
        //}

        [TransManager]
        public MatHireSupplyOrderMaster SaveSupplyOrder(MatHireSupplyOrderMaster obj, IList movedDtlList)
        {
            GetCode(obj);
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MatHireSupplyOrderMaster;
        }
        [TransManager]
        public void GetCode(MatHireSupplyOrderMaster obj)
        {
            if (obj != null && obj.Id == null)
            {
                if (obj.Special == "土建")
                {
                    obj.Code = GetCode(typeof(MatHireSupplyOrderMaster), obj.ProjectId, obj.MaterialCategory.Abbreviation);
                }
                else if (obj.Special == "安装")
                {
                    obj.Code = GetCode(typeof(MatHireSupplyOrderMaster), obj.ProjectId, obj.SpecialType);
                }
                obj.RealOperationDate = DateTime.Now;
            }
        }

        [TransManager]
        public bool DeleteSupplyOrder(MatHireSupplyOrderMaster obj)
        {
            if (obj.Id == null) return true;
            //删除明细时 删除引用数量
            foreach (MatHireSupplyOrderDetail dtl in obj.Details)
            {
                if (dtl.Id != null)
                {
                    if (obj.Special == "土建")
                    {
                        if (TransUtil.ToString(dtl.ForwardDetailId) != "")
                        {
                            DemandMasterPlanDetail forwardDtl = GetDemandDetailById(dtl.ForwardDetailId);
                            forwardDtl.SupplyLeftQuantity = forwardDtl.SupplyLeftQuantity + Math.Abs(dtl.Quantity);
                            //forwardDtl.DemandLeftQuantity = forwardDtl.DemandLeftQuantity + Math.Abs(dtl.Quantity);
                            //forwardDtl.LeftQuantity = forwardDtl.Quantity - forwardDtl.RefQuantity;
                            dao.Save(forwardDtl);
                        }
                    }
                    else
                    {
                        if (TransUtil.ToString(dtl.ForwardDetailId) != "")
                        {
                            SupplyPlanDetail forwardDtl = GetSupplyDetailById(dtl.ForwardDetailId);
                            forwardDtl.RefQuantity = forwardDtl.RefQuantity - Math.Abs(dtl.Quantity);
                            forwardDtl.LeftQuantity = forwardDtl.Quantity - forwardDtl.RefQuantity;
                            dao.Save(forwardDtl);
                        }
                    }
                }
            }
            return dao.Delete(obj);
        }

        /// <summary>
        /// 得到同一项目，同一供应商的合同签订物资情况
        /// </summary>
        /// <returns></returns>
        public Hashtable GetOrderMaterialInfo(string projectId, string supRelId)
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = " select t2.material,t1.id from thd_supplyordermaster t1,thd_supplyorderdetail t2 where t1.id=t2.parentid " +
                                    " and t1.projectid='" + projectId + "' and t1.supplierrelation='" + supRelId + "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (!ht.Contains(TransUtil.ToString(dataRow["material"])))
                    {
                        IList list = new ArrayList();
                        list.Add(TransUtil.ToString(dataRow["id"]));
                        ht.Add(TransUtil.ToString(dataRow["material"]), list);
                    }
                    else
                    {
                        IList list = (ArrayList)ht[TransUtil.ToString(dataRow["material"])];
                        list.Add(TransUtil.ToString(dataRow["id"]));
                        ht.Remove(TransUtil.ToString(dataRow["material"]));
                        ht.Add(TransUtil.ToString(dataRow["material"]), list);
                    }
                }
            }
            return ht;
        }
        #endregion

        #region 封存单据
        /// <summary>
        /// 保存封存管理单据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public MatHireStockBlockMaster SaveStockBlockMasterOrder(MatHireStockBlockMaster obj)
        {
            if (obj.Id == null)
            {
                if (obj.MatHireType == EnumMatHireType.普通料具)
                {
                    obj.Code = GetCode(typeof(MatHireStockBlockMaster), obj.ProjectId);
                }
                else
                {
                    obj.Code = GetCode(typeof(MatHireStockBlockMaster), obj.ProjectId, obj.MatHireType == EnumMatHireType.钢管 ? "钢管" : "碗扣");
                }
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InExecute || obj.DocState == DocumentState.InAudit)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as MatHireStockBlockMaster;
        }

        /// <summary>
        /// 通过ID查询料具封存单据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MatHireStockBlockMaster GetStockBlockMasterOrderById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetStockBlockMasterOrder(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireStockBlockMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询料具封存单据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MatHireStockBlockMaster GetStockBlockMasterOrderCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));
            IList list = GetStockBlockMasterOrder(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as MatHireStockBlockMaster;
            }
            return null;
        }

        /// <summary>
        /// 查询料具封存单据
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetStockBlockMasterOrder(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("TheSupplierRelationInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.UsedPart", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.SubjectGUID", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.BorrowUnit", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MatHireStockBlockMaster), objectQuery);
        }

        /// <summary>
        /// 查询料具封存单据明细信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetStockBlockMasterOrderDetail(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(MatHireStockBlockDetail), objectQuery);
        }

        /// <summary>
        /// 料具封存单据查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet StockBlockMasterOrderQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
//            string sql = @"SELECT t1.id,t1.code,t1.realoperationdate,t1.createdate,t1.originalContractNo,t5.ORGNAME,t1.docstate state,t7.MATCODE,t7.MATNAME,t7.MATSPECIFICATION,
//                t2.Quantity,t2.price,t1.balRule,t8.STANDUNITNAME,t2.Descript,t1.PrintTimes
//                FROM thd_MaterialHireOrderMaster t1 Left JOIN thd_MaterialHireOrderDetail t2 ON t1.id=t2.parentId
//                LEFT JOIN ResMaterial t7 ON t7.MATERIALID=t2.Material
//                left join ressupplierrelation t4 on t1.supplierrelation=t4.suprelid
//                left join resorganization t5 on t4.orgid=t5.orgid
//                left join resstandunit t8 on t2.MATSTANDARDUNIT=t8.STANDUNITID";
//            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = "";//sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        /// <summary>
        /// 校验当前料具商是否已经签订租赁合同
        /// </summary>
        /// <param name="theSupplier"></param>
        /// <returns></returns>
        public bool VerifyCurrSupplierOrder(SupplierRelationInfo theSupplier, MatHireOrderMaster Master)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "";
            //if (Master.Id != null)
            //{
            //    sql = "SELECT * FROM thd_MaterialHireOrderMaster WHERE SupplierRelation ='" + theSupplier.Id + "'and Id !='" + Master.Id + "' and projectid='" + Master.ProjectId + "'";

            //}
            //else
            //{
            //    sql = "SELECT * FROM thd_MaterialHireOrderMaster WHERE SupplierRelation ='" + theSupplier.Id + "' and projectid='" + Master.ProjectId + "'";
            //}
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable table = dataSet.Tables[0];
            int count = table.Rows.Count;
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }

}
