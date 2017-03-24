﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalContractMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Core;
using System.Collections;
using NHibernate;
using System.Data;
using VirtualMachine.Core.DataAccess;
using System.Runtime.Remoting.Messaging;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using NHibernate.Criterion;
using CommonSearchLib.BillCodeMng.Service;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Service
{
    public class FinanceMultDataSrv : BaseService, IFinanceMultDataSrv
    {
        #region

        private IDao _Dao;

        public virtual IDao Dao
        {
            get { return _Dao; }
            set { _Dao = value; }
        }

        private IBillCodeRuleSrv billCodeRuleSrv;

        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        private IAppSrv refAppSrv;

        /// <summary>
        /// 审批服务
        /// </summary>
        public IAppSrv RefAppSrv
        {
            get
            {
                return refAppSrv;
            }
            set { refAppSrv = value; }
        }

        public virtual string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }

        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }

        /// <summary>
        /// 保存新增数据
        /// </summary>
        /// <param name="obj">要保存的对象</param>
        /// <returns>保存后的对象</returns>
        [TransManager]
        public virtual Object Save(Object obj)
        {
            BaseMaster master = obj as BaseMaster;
            master.Code = GetCode(obj.GetType());
            if (master != null)
            {
                //master.Code = obj.GetType().Name + "-" + DateTime.Now.ToString("yyyyMMddHHssmm");
                if (master != null)
                {
                    master.RealOperationDate = DateTime.Now;
                }
                Dao.Save(obj);
            }
            else
            {
                var model = obj as BaseDetail;
                Dao.Save(model);
            }
            return obj;
        }

        /// <summary>
        /// 保存要修改的数据
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <returns>修改后的对象</returns>
        [TransManager]
        public virtual Object Update(Object obj)
        {
            BaseMaster master = obj as BaseMaster;
            if (master != null)
            {
                master.RealOperationDate = DateTime.Now;
            }
            Dao.Update(obj);
            return obj;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="obj">要删除的对象</param>
        /// <returns>True 删除成功，False 删除失败</returns>
        [TransManager]
        public bool Delete(object obj)
        {
            return Dao.Delete(obj);
        }

        [TransManager]
        public bool DeleteObjects(IList objList)
        {
            return Dao.Delete(objList);
        }

        [TransManager]
        public void SaveList(IList list)
        {
            Dao.Save(list);
        }

        [TransManager]
        public bool SaveOrUpdateList(IList list)
        {
            return Dao.SaveOrUpdate(list);
        }

        #endregion

        #region 公共业务方法

        /// <summary>
        /// 查询分公司信息
        /// </summary>
        public IList QuerySubAndCompanyOrgInfo()
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText =
                "select t1.opgid,t1.opgname,t1.opgsyscode from resoperationorg t1 where t1.opgstate=1 and " +
                " t1.opgoperationtype='b' ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    OperationOrgInfo orgInfo = new OperationOrgInfo();
                    orgInfo.Id = TransUtil.ToString(dataRow["opgid"]);
                    orgInfo.Name = TransUtil.ToString(dataRow["opgname"]);
                    orgInfo.SysCode = TransUtil.ToString(dataRow["opgsyscode"]);
                    list.Add(orgInfo);
                }

            }
            return list;
        }

        #endregion

        #region 财务账面信息

        [TransManager]
        public bool DeleteFinanceMultDataMaster(Object obj)
        {
            return Dao.Delete(obj);
        }

        public IList Query(ObjectQuery oQuery)
        {
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof (FinanceMultDataMaster), oQuery);
        }

        /// <summary>
        /// temp
        /// </summary>
        /// <param name="type"></param>
        /// <param name="oQuery"></param>
        /// <returns></returns>
        public IList Query(Type type, ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(type, oQuery);
        }

        public FinanceMultDataMaster GetMasterByID(string strID)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", strID));
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            IList lst = Dao.ObjectQuery(typeof (FinanceMultDataMaster), oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as FinanceMultDataMaster;

        }

        public FactoringDataMaster GetFactoringDataById(string id)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", id));
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            IList lst = Dao.ObjectQuery(typeof (FactoringDataMaster), oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as FactoringDataMaster;
        }

        public IList QueryDetial(ObjectQuery oQuery)
        {
            IList<FactoringDataDetail> lstResult = new List<FactoringDataDetail>();
            IList lst = Dao.ObjectQuery(typeof (FactoringDataDetail), oQuery);
            return lst;
        }

        #endregion

        #region 收款单

        [TransManager]
        public IList SaveGatheringMaster(IList lstMaster)
        {
            foreach (GatheringMaster oMaster in lstMaster)
            {
                if (oMaster.Id == null)
                {
                    if (oMaster.ProjectId == null)
                    {
                        oMaster.Code = GetCode(typeof (GatheringMaster));
                    }
                    else
                    {
                        oMaster.Code = GetCode(typeof (GatheringMaster), oMaster.ProjectId);
                    }
                }
                oMaster.RealOperationDate = DateTime.Now;
            }
            Dao.SaveOrUpdate(lstMaster);
            return lstMaster;
        }

        [TransManager]
        public GatheringMaster SaveGatheringMaster(GatheringMaster obj)
        {
            if (obj.Id == null)
            {
                if (obj.ProjectId == null)
                {
                    obj.Code = GetCode(typeof (GatheringMaster));
                }
                else
                {
                    obj.Code = GetCode(typeof (GatheringMaster), obj.ProjectId);
                }
            }
            obj.RealOperationDate = DateTime.Now;
            //保存票据信息
            foreach (GatheringDetail detail in obj.Details)
            {
                AcceptanceBill acceptBill = detail.AcceptBillID;
                if (acceptBill != null && TransUtil.ToString(acceptBill.BillNo) != "")
                {
                    if (TransUtil.ToString(acceptBill.Id) == "")
                    {
                        acceptBill.CreatePerson = obj.CreatePerson;
                        acceptBill.CreatePersonName = obj.CreatePersonName;
                        acceptBill.OperOrgInfo = obj.OperOrgInfo;
                        acceptBill.OperOrgInfoName = obj.OperOrgInfoName;
                        acceptBill.OpgSysCode = obj.OpgSysCode;
                        acceptBill.ProjectId = obj.ProjectId;
                        acceptBill.ProjectName = obj.ProjectName;
                        acceptBill.CreateYear = obj.CreateYear;
                        acceptBill.CreateMonth = obj.CreateMonth;
                        acceptBill.DocState = DocumentState.InExecute;
                    }
                    acceptBill.GatheringMxId = detail;
                    acceptBill.RealOperationDate = DateTime.Now;
                    this.SaveAcceptanceBill(acceptBill);
                }
            }
            //补充发票信息
            foreach (GatheringInvoice invoice in obj.ListInvoice)
            {
                if (invoice != null && TransUtil.ToString(invoice.Id) == "")
                {
                    invoice.CreatePerson = obj.CreatePerson;
                    invoice.CreatePersonName = obj.CreatePersonName;
                    invoice.OperOrgInfo = obj.OperOrgInfo;
                    invoice.OperOrgInfoName = obj.OperOrgInfoName;
                    invoice.OpgSysCode = obj.OpgSysCode;
                    invoice.ProjectId = obj.ProjectId;
                    invoice.ProjectName = obj.ProjectName;
                    invoice.CreateYear = obj.CreateYear;
                    invoice.CreateMonth = obj.CreateMonth;
                    invoice.DocState = obj.DocState;
                    invoice.TheCustomerName = obj.TheCustomerName;
                    CustomerRelationInfo customRel = new CustomerRelationInfo();
                    customRel.Id = obj.TheCustomerRelationInfo;
                    customRel.Version = 0;
                    invoice.TheCustomerRelationInfo = customRel;
                }
                invoice.RealOperationDate = DateTime.Now;
            }
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        [TransManager]
        public GatheringMaster SaveWebGatheringMaster(GatheringMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof (GatheringMaster));
            }
            obj.RealOperationDate = DateTime.Now;
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        [TransManager]
        public bool DeleteGatheringMaster(GatheringMaster obj)
        {
            return Dao.Delete(obj);
        }

        //删除无收款明细的票据信息
        private void DeleteNoGatheringAcceptBills(string delAcceptIdStr)
        {
            try
            {
                if (TransUtil.ToString(delAcceptIdStr) == "")
                    return;
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                string sql = " delete from thd_acceptancebill t1 where t1.id in (" + delAcceptIdStr + ")";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 通过收款单ID查询对应的票据ID信息
        /// </summary>
        /// <returns></returns>
        private IList GetAcceptBillByGatheringID(string gatheringID)
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText =
                "select t1.acceptbillid from thd_gatheringdetail t1 where t1.acceptbillid is not null and t1.parentid= '" +
                gatheringID + "'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    list.Add(TransUtil.ToString(dataRow["acceptbillid"]));
                }
            }
            return list;
        }

        [TransManager]
        public AcceptanceBill SaveAcceptanceBill(AcceptanceBill obj)
        {
            obj.RealOperationDate = DateTime.Now;
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        [TransManager]
        public GatheringInvoice SaveGatheringInvoice(GatheringInvoice obj)
        {
            obj.RealOperationDate = DateTime.Now;
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        public AcceptanceBill GetAcceptanceBillById(string id)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", id));
            IList lst = Dao.ObjectQuery(typeof (AcceptanceBill), oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as AcceptanceBill;
        }

        public IList GetGatheringMaster(ObjectQuery oQuery)
        {
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof (GatheringMaster), oQuery);
        }

        public IList GetGatheringInvoice(ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(typeof (GatheringInvoice), oQuery);
        }

        public IList GetAcceptanceBill(ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(typeof (AcceptanceBill), oQuery);
        }

        public GatheringMaster GetGatheringMasterById(string id)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", id));
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            oQuery.AddFetchMode("ListInvoice", FetchMode.Eager);
            oQuery.AddFetchMode("ListRel", FetchMode.Eager);
            oQuery.AddFetchMode("Details.AcceptBillID", FetchMode.Eager);
            IList lst = Dao.ObjectQuery(typeof (GatheringMaster), oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as GatheringMaster;
        }

        /// <summary>
        /// 查询收款相关信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public DataSet QueryGatheringByCondition(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            //string sql = "";
            command.CommandText = condition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        /// <summary>
        /// 查询报量明细信息
        /// </summary>
        /// <param name="ownerMxID">报量明细ID</param>
        /// <returns></returns>
        public DataDomain QueryOwnerQuantityInfoByMxID(string ownerMxID)
        {
            DataDomain domain = new DataDomain();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = "SELECT t1.Code,t2.id dtlID, t2.ConfirmMoney,t2.QWBSName,t2.SubmitQuantity,t2.QuantityDate,t2.ConfirmDate "
                                  +
                                  " FROM THD_OwnerQuantityMaster t1 INNER JOIN THD_OwnerQuantityDetail t2 ON t1.Id = t2.ParentId and t2.id='" +
                                  ownerMxID + "'";

            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    domain.Name1 = TransUtil.ToString(dataRow["dtlID"]);
                    domain.Name2 = TransUtil.ToString(dataRow["Code"]);
                    domain.Name3 = TransUtil.ToString(dataRow["QuantityDate"]);
                    domain.Name4 = TransUtil.ToString(dataRow["SubmitQuantity"]);
                    domain.Name5 = TransUtil.ToString(dataRow["ConfirmMoney"]);
                    domain.Name6 = TransUtil.ToString(dataRow["QWBSName"]);
                }
            }
            return domain;
        }

        #endregion

        #region 收款与报量关系

        [TransManager]
        public GatheringAndQuantityRel SaveGatheringAndQuantityRel(GatheringAndQuantityRel obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        public IList GetGatheringAndQuantityRel(ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(typeof (GatheringAndQuantityRel), oQuery);
        }

        #endregion

        #region 付款单

        [TransManager]
        public IList SavePaymentMaster(IList lstBill)
        {
            foreach (PaymentMaster oMaster in lstBill)
            {
                if (oMaster.Id == null)
                {
                    if (oMaster.ProjectId == null)
                    {
                        oMaster.Code = GetCode(typeof (PaymentMaster));
                    }
                    else
                    {
                        oMaster.Code = GetCode(typeof (PaymentMaster), oMaster.ProjectId);
                    }
                }
                oMaster.RealOperationDate = DateTime.Now;
            }
            Dao.SaveOrUpdate(lstBill);
            return lstBill;
        }

        [TransManager]
        public PaymentMaster SavePaymentMaster(PaymentMaster obj)
        {
            IList oldAcceptBillIdList = new ArrayList(); //付款单对应的票据信息
            if (obj.Id == null)
            {
                if (obj.ProjectId == null)
                {
                    obj.Code = GetCode(typeof (PaymentMaster));
                }
                else
                {
                    obj.Code = GetCode(typeof (PaymentMaster), obj.ProjectId);
                }

                if(!string.IsNullOrEmpty(obj.Temp1))
                {
                    obj.Code = obj.Code.Replace("付款", obj.Temp1);
                }
            }
            else
            {
                oldAcceptBillIdList = this.GetAcceptBillByPaymentID(obj.Id);
                if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
                {
                    string relID = "";
                    if (TransUtil.ToString(obj.TheSupplierRelationInfo) != "")
                    {
                        relID = obj.TheSupplierRelationInfo;
                    }
                    else
                    {
                        relID = obj.TheCustomerRelationInfo;
                    }
                    obj.AddPayMoney = this.GetBusinessMoneyByProject(1, obj.ProjectId, relID);
                    obj.AddInvoiceMoney = this.GetBusinessMoneyByProject(2, obj.ProjectId, relID);
                }
            }
            foreach (PaymentDetail detail in obj.Details)
            {
                AcceptanceBill acceptBill = detail.AcceptBillID;
                if (acceptBill != null && TransUtil.ToString(acceptBill.BillNo) != "")
                {
                    if (TransUtil.ToString(acceptBill.Id) == "")
                    {
                        acceptBill.CreatePerson = obj.CreatePerson;
                        acceptBill.CreatePersonName = obj.CreatePersonName;
                        acceptBill.OperOrgInfo = obj.OperOrgInfo;
                        acceptBill.OperOrgInfoName = obj.OperOrgInfoName;
                        acceptBill.OpgSysCode = obj.OpgSysCode;
                        acceptBill.ProjectId = obj.ProjectId;
                        acceptBill.ProjectName = obj.ProjectName;
                        acceptBill.CreateYear = obj.CreateYear;
                        acceptBill.CreateMonth = obj.CreateMonth;
                    }
                    else
                    {
                        if (oldAcceptBillIdList.Contains(acceptBill.Id))
                        {
                            oldAcceptBillIdList.Remove(acceptBill.Id);
                        }
                    }
                    acceptBill.PaymentMxId = detail;
                    acceptBill.Descript = obj.ProjectName;
                    acceptBill.RealOperationDate = DateTime.Now;
                    this.SaveAcceptanceBill(acceptBill);
                }
            }
            obj.RealOperationDate = DateTime.Now;
            Dao.SaveOrUpdate(obj);

            //清空无付款明细的票据的付款明细ID
            if (oldAcceptBillIdList.Count > 0)
            {
                string oldStr = "'0'";
                foreach (string billId in oldAcceptBillIdList)
                {
                    oldStr += ",'" + billId + "'";
                }
                this.ClearAcceptBillsPaymentMxIdByStr(oldStr);
            }

            if (obj.Code.StartsWith("资金支付") && obj.DocState == DocumentState.InAudit)
            {
                var appBill = new ApproveBill();
                appBill.BillCode = obj.Code;
                appBill.BillCreateDate = obj.CreateDate;
                appBill.BillCreatePerson = obj.CreatePerson;
                appBill.BillCreatePersonName = obj.CreatePersonName;
                appBill.BillId = obj.Id;
                appBill.BillSysCode = obj.OpgSysCode;
                appBill.ProjectId = obj.ProjectId;
                appBill.ProjectName = obj.ProjectName;

                RefAppSrv.SaveApproveBill(appBill, typeof (PaymentMaster));
            }
            return obj;
        }

        [TransManager]
        public PaymentRequest SavePaymentRequest(PaymentRequest obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(PaymentRequest));
            }
            Dao.SaveOrUpdate(obj);

            
            return obj;
        }

        [TransManager]
        public PaymentRequest UpdatePaymentRequest(PaymentRequest obj)
        {
            if (obj.Code.StartsWith("资金支付申请"))
            {
                var appBill = new ApproveBill();
                appBill.BillCode = obj.Code;
                appBill.BillCreateDate = obj.CreateDate;
                appBill.BillCreatePerson = obj.CreatePerson;
                appBill.BillCreatePersonName = obj.CreatePersonName;
                appBill.BillId = obj.Id;
                appBill.BillSysCode = obj.Temp1;
                appBill.ProjectId = obj.ProjectId;
                appBill.ProjectName = obj.ProjectName;

                RefAppSrv.SaveApproveBill(appBill, typeof(PaymentRequest));
            }
            Dao.Update(obj);
            return obj;
        }

        [TransManager]
        public void DeletePaymentRequest(PaymentRequest obj)
        {
            Dao.Delete(obj);
        }

        [TransManager]
        public void DeletePaymentRequestList(IList obj)
        {
            Dao.Delete(obj);
        }

        [TransManager]
        public PaymentMaster SaveWebPaymentMaster(PaymentMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof (PaymentMaster));
            }
            obj.RealOperationDate = DateTime.Now;
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        [TransManager]
        public bool DeletePaymentMaster(PaymentMaster obj)
        {
            //清空此付款单对应的票据信息
            ClearAcceptBillsPaymentMxId(obj.Id);
            return Dao.Delete(obj);
        }

        //清空无付款明细的票据的付款明细ID
        [TransManager]
        private void ClearAcceptBillsPaymentMxId(string paymentID)
        {
            try
            {
                if (TransUtil.ToString(paymentID) == "")
                    return;
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                //收款单入的票据，清除付款明细ID
                string sql = " update thd_acceptancebill t1 set t1.paymentmxid=null " +
                             " where t1.gatheringmxid is not null and t1.paymentmxid in (select id from thd_paymentdetail where parentid='" +
                             paymentID + "')";
                command.CommandText = sql;
                command.ExecuteNonQuery();
                //付款单入的票据删除
                sql = " delete from thd_acceptancebill t1 " +
                      " where t1.gatheringmxid is null and t1.paymentmxid in (select id from thd_paymentdetail where parentid='" +
                      paymentID + "')";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //通过票据ID联接字符串，如果存在收款明细ID，则清空付款明细ID，否则删除此票据信息
        [TransManager]
        private void ClearAcceptBillsPaymentMxIdByStr(string paymentMxIDStr)
        {
            try
            {
                if (TransUtil.ToString(paymentMxIDStr) == "")
                    return;
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                //收款单入的票据，清除付款明细ID
                string sql =
                    " update thd_acceptancebill t1 set t1.paymentmxid=null  where t1.gatheringmxid is not null and t1.id in (" +
                    paymentMxIDStr + ")";
                command.CommandText = sql;
                command.ExecuteNonQuery();

                //付款单入的票据删除
                sql = " delete from thd_acceptancebill t1 where t1.gatheringmxid is null and t1.id in (" +
                      paymentMxIDStr + ")";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PaymentMaster GetPaymentMasterById(string id)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", id));
            oQuery.AddFetchMode("FundPlan", FetchMode.Eager);
            oQuery.AddFetchMode("FundPlan.Master", FetchMode.Eager);
            oQuery.AddFetchMode("Details.AcceptBillID", FetchMode.Eager);

            IList lst = GetPaymentMaster(oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as PaymentMaster;
        }

        public CurrentProjectInfo GetProjectInfoById(string id)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", id));

            IList lst = Dao.ObjectQuery(typeof(CurrentProjectInfo), oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as CurrentProjectInfo;
        }


        public PaymentMaster GetPlanPayMasterById(string id)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", id));
            oQuery.AddFetchMode("FundPlan", FetchMode.Eager);
            oQuery.AddFetchMode("FundPlan.Master", FetchMode.Eager);

            IList lst = GetPaymentMaster(oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as PaymentMaster;
        }

        public IList GetPaymentMasterByRequestBill(PaymentRequest requestBill)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            oQuery.AddCriterion(Expression.Eq("RequestBill", requestBill));
            return Dao.ObjectQuery(typeof(PaymentMaster), oQuery);
        }

        public IList GetPaymentMaster(ObjectQuery oQuery)
        {
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof (PaymentMaster), oQuery);
        }

        public IList GetPaymentRequest(ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(typeof(PaymentRequest), oQuery);
        }

        /// <summary>
        /// 查询本项目未提交的收付款单数
        /// </summary>
        /// <param name="billType">单据类型,0:收款,1:付款</param>
        /// <returns></returns>
        public int GetNoSubmitBillCount(string projectID, int billType)
        {
            int count = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            if (billType == 0)
            {
                command.CommandText =
                    "select count(t1.id) count from thd_gatheringmaster t1 where t1.state=0 and t1.projectid='" +
                    projectID + "'";
            }
            if (billType == 1)
            {
                command.CommandText =
                    "select count(t1.id) count from thd_paymentmaster t1 where t1.state=0 and t1.projectid='" +
                    projectID + "'";
            }
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    count = TransUtil.ToInt(dataRow["count"]);
                }
            }
            return count;
        }

        /// <summary>
        /// 通过付款单ID查询对应的票据ID信息
        /// </summary>
        /// <returns></returns>
        private IList GetAcceptBillByPaymentID(string paymentID)
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText =
                "select id from thd_acceptancebill where paymentmxid in (select id from thd_paymentdetail where parentid='" +
                paymentID + "')";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    list.Add(TransUtil.ToString(dataRow["id"]));
                }
            }
            return list;
        }

        /// <summary>
        /// 查询本项目的累计付款，累计发票，累计结算
        /// </summary>
        /// <param name="queryType">查询类型(1:累计付款 2:累计发票 3:累计结算)</param>
        private decimal GetBusinessMoneyByProject(int queryType, string projectId, string relId)
        {
            decimal sumMoney = 0;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            if (queryType == 1)
            {
                command.CommandText =
                    " select sum(t1.summoney) money From thd_paymentmaster t1 where t1.state=5 and t1.projectid='" +
                    projectId + "' and " +
                    " (t1.supplierrelation='" + relId + "' or t1.thecustomerrelationinfo='" + relId + "') ";
            }
            else if (queryType == 2)
            {
                command.CommandText = " select sum(t1.summoney) money from thd_paymentinvoice t1 where t1.state=5 " +
                                      " and t1.projectid='" + projectId + "' and t1.supplierrelation='" + relId + "' ";
            }
            else if (queryType == 3)
            {
                command.CommandText = " ";
            }
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    sumMoney = TransUtil.ToDecimal(dataRow["money"]);
                }
            }
            return sumMoney;
        }

        #endregion

        #region 付款发票

        public PaymentInvoice GetPaymentInvoiceById(string id)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", id));
            oQuery.AddFetchMode("Settlements", FetchMode.Eager);
            oQuery.AddFetchMode("TheSupplierRelationInfo", FetchMode.Eager);
            oQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", FetchMode.Eager);

            IList lst = Dao.ObjectQuery(typeof (PaymentInvoice), oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as PaymentInvoice;
        }

        public bool IsPaymentInvoiceNoUsed(string invNo, string id)
        {
            if (string.IsNullOrEmpty(invNo))
            {
                return false;
            }

            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("InvoiceNo", invNo));
            if (!string.IsNullOrEmpty(id))
            {
                oQuery.AddCriterion(Expression.Not(Expression.Eq("Id", id)));
            }
            IList lst = Dao.ObjectQuery(typeof (PaymentInvoice), oQuery);

            return lst != null && lst.Count > 0;
        }

        public IList GetPaymentInvoice(ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(typeof (PaymentInvoice), oQuery);
        }

        [TransManager]
        public bool DeletePaymentInvoice(PaymentInvoice obj)
        {
            return Dao.Delete(obj);
        }

        [TransManager]
        public PaymentInvoice SavePaymentInvoice(PaymentInvoice obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof (PaymentInvoice));
            }
            obj.RealOperationDate = DateTime.Now;
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        [TransManager]
        public IList SavePaymentInvoice(IList lst)
        {
            foreach (PaymentInvoice obj in lst)
            {
                if (obj.Id == null)
                {
                    obj.Code = GetCode(typeof (PaymentInvoice));
                }
                obj.RealOperationDate = DateTime.Now;
            }
            Dao.SaveOrUpdate(lst);
            return lst;
        }

        public IList GetPaymentInvoice(string projId, string sysCode, DateTime bgDate, DateTime endDate,
                                       string supplierName, string invoiceType)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = @"select t3.opgname as OpgName,t2.taxregcode as TaxRegCode,t1.* from THD_PAYMENTINVOICE t1"
                         + " inner join RESSUPPLIERRELATION t2 on t1.supplierrelation = t2.suprelid"
                         +
                         " left join resoperationorg t3 on t3.opgoperationtype='b' and instr(t1.opgsyscode,t3.opgsyscode)>0 where 1=1 ";

            if (!string.IsNullOrEmpty(projId))
            {
                sql += " and t1.ProjectId = '" + projId + "'";
            }

            if (!string.IsNullOrEmpty(sysCode))
            {
                sql += " and ( t1.OpgSysCode like '%" + sysCode + "%') ";
            }

            if (!string.IsNullOrEmpty(supplierName))
            {
                sql += " and t1.supplierrelationname = '" + supplierName + "'";
            }
            if (!string.IsNullOrEmpty(invoiceType))
            {
                sql += " and t1.InvoiceType = '" + invoiceType + "'";
            }

            sql += " and t1.CreateDate>=to_date('" + bgDate.ToShortDateString() +
                   "','yyyy-mm-dd') and t1.CreateDate<=to_date('" + endDate.ToShortDateString() + "','yyyy-mm-dd')";

            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            IList list = new ArrayList();
            foreach (DataRow oDataRow in dataSet.Tables[0].Rows) //循环读取临时表的行
            {
                PaymentInvoice pi = new PaymentInvoice();
                pi.Code = oDataRow["Code"].ToString();
                pi.Temp1 = oDataRow["OpgName"].ToString();
                pi.ProjectName = oDataRow["ProjectName"].ToString();
                pi.TheSupplierName = oDataRow["SupplierRelationName"].ToString();
                pi.Temp2 = oDataRow["TaxRegCode"].ToString();
                pi.AccountTitleName = oDataRow["AccountTitleName"].ToString();
                pi.SupplierScale = oDataRow["SupplierScale"].ToString();
                pi.InvoiceType = oDataRow["InvoiceType"].ToString();
                pi.InvoiceCode = oDataRow["InvoiceCode"].ToString();
                pi.InvoiceNo = oDataRow["InvoiceNo"].ToString();
                pi.CreateDate = ClientUtil.ToDateTime(oDataRow["CreateDate"]);
                pi.SumMoney = ClientUtil.ToDecimal(oDataRow["SumMoney"].ToString());
                pi.TaxMoney = ClientUtil.ToDecimal(oDataRow["TaxMoney"].ToString());
                pi.TransferType = oDataRow["TransferType"].ToString();
                pi.TransferTax = ClientUtil.ToDecimal(oDataRow["TransferTax"].ToString());
                pi.IfDeduction = oDataRow["IfDeduction"].ToString();
                pi.DocState = (DocumentState) Enum.Parse(typeof (DocumentState), oDataRow["State"].ToString());
                pi.CreatePersonName = oDataRow["CreatePersonName"].ToString();
                pi.RealOperationDate = ClientUtil.ToDateTime(oDataRow["RealOperationDate"]);
                pi.Descript = oDataRow["Descript"].ToString();

                list.Add(pi);
            }
            return list;

        }

        public List<DataDomain> GetPaymentInvoiceReport(DateTime bgDate, DateTime endDate, string projId, string sysCode,
                                                        bool isSubCompany)
        {
            var sql = string.Empty;
            var resultType = 0;
            if (!string.IsNullOrEmpty(projId))
            {
                sql = string.Format(SQLScript.PaymentInvoiceReportByProjectSql, bgDate.ToString("yyyy-MM-dd"),
                                    endDate.ToString("yyyy-MM-dd"), projId);
                resultType = 1;
            }
            else if (isSubCompany)
            {
                sql = string.Format(SQLScript.PaymentInvoiceReportBySubSql, bgDate.ToString("yyyy-MM-dd"),
                                    endDate.ToString("yyyy-MM-dd"), sysCode);
                resultType = 2;
            }
            else
            {
                sql = string.Format(SQLScript.PaymentInvoiceReportSql, bgDate.ToString("yyyy-MM-dd"),
                                    endDate.ToString("yyyy-MM-dd"));
                resultType = 3;
            }

            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var retList = new List<DataDomain>();
            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var dm = new DataDomain();
                dm.Name1 = i + 1;
                switch (resultType)
                {
                    case 1:
                        dm.Name2 = ds.Tables[0].Rows[i]["supplierrelationname"].ToString();
                        dm.Name3 = ds.Tables[0].Rows[i]["accounttitlename"].ToString();
                        dm.Name4 = ds.Tables[0].Rows[i]["supplierscale"].ToString();
                        dm.Name5 = ds.Tables[0].Rows[i]["invoicetype"].ToString();
                        break;
                    case 2:
                        dm.Name2 = ds.Tables[0].Rows[i]["projectname"].ToString();
                        dm.Name3 = ds.Tables[0].Rows[i]["invoicetype"].ToString();
                        break;
                    case 3:
                        dm.Name2 = ds.Tables[0].Rows[i]["opgname"].ToString();
                        dm.Name3 = ds.Tables[0].Rows[i]["invoicetype"].ToString();
                        break;
                }

                dm.Name6 = ds.Tables[0].Rows[i]["summoney"];
                dm.Name7 = ds.Tables[0].Rows[i]["taxmoney"];
                dm.Name8 = ds.Tables[0].Rows[i]["deductionMoney"];
                dm.Name9 = ds.Tables[0].Rows[i]["unDeductionMoney"];

                retList.Add(dm);
            }

            return retList;
        }

        public List<DataDomain> GetSettlementsByType(DateTime bgDate, DateTime endDate, string projId, string billCode, string billType, string supId)
        {
            var sql = string.Empty;
            if (billType == "分包结算")
            {
                sql = SQLScript.GetUnRelationSubcontractSql;
            }
            else if (billType == "物资结算")
            {
                sql = SQLScript.GetUnRelationMaterialSettleSql;
            }
            else if (billType == "设备结算")
            {
                sql = SQLScript.GetUnRelationMaterialRentalSql;
            }

            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }

            var ds = QueryDataToDataSet(
                string.Format(sql,
                              (int) DocumentState.InExecute,
                              projId,
                              bgDate.ToString("yyyy-MM-dd"),
                              endDate.ToString("yyyy-MM-dd"),
                              string.IsNullOrEmpty(billCode)
                                  ? string.Empty
                                  : string.Format(" and a.Code like '%{0}%'", billCode),
                              supId));
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var list = new List<DataDomain>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var item = new DataDomain();
                item.Name1 = TransUtil.ToString(ds.Tables[0].Rows[i][0]);
                item.Name2 = TransUtil.ToString(ds.Tables[0].Rows[i][1]);
                item.Name3 = TransUtil.ToDecimal(ds.Tables[0].Rows[i][2]);
                item.Name4 = TransUtil.ToDecimal(ds.Tables[0].Rows[i][3]);

                list.Add(item);
            }
            return list;
        }

        #endregion

        #region 供应和客户关系选择

        //选择供应和客户关系大类
        public IList GetRelCategoryList()
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText =
                " select '2' caterel ,t1.catid,t1.catname from rescumrelcategory t1 where t1.catlevel=2 " +
                " union all " +
                " select '1' caterel ,t1.catid,t1.catname from ressuprelcategory t1 where t1.catlevel=2 ";

            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name1 = TransUtil.ToString(dataRow["caterel"]);
                    domain.Name2 = TransUtil.ToString(dataRow["catid"]);
                    domain.Name3 = TransUtil.ToString(dataRow["catname"]);
                    list.Add(domain);
                }
            }
            return list;
        }

        /// <summary>
        /// 通过条件查询供应关系和客户关系信息
        /// </summary>
        /// <param name="supCondition">供应商条件</param>
        /// <param name="cusCondition">客户条件</param>
        /// <returns></returns>
        public IList QueryCusAndSupInfoByCondition(string supCondition, string cusCondition)
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            command.CommandText =
                " select 1 orgtype,t3.catname, t1.suprelid id,t1.code,t2.orgname,t1.address,t1.bankname,t1.bankaccount " +
                " From ressupplierrelation t1,resorganization t2, ressuprelcategory t3 where " +
                " t1.orgid=t2.orgid and t1.supplierrelationcategoryid=t3.catid " + supCondition +
                " union all " +
                " select 2 orgtype, t3.catname,t1.cusrelid id, t1.cusrelcode code,t2.orgname,t1.address,t1.bankname,t1.bankaccount " +
                " From rescusrelation t1,resorganization t2,rescumrelcategory t3 where " +
                " t1.orgid=t2.orgid and t1.cusrelcategoryid=t3.catid " + cusCondition;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name1 = TransUtil.ToInt(dataRow["orgtype"]);
                    domain.Name8 = TransUtil.ToString(dataRow["catname"]);
                    domain.Name2 = TransUtil.ToString(dataRow["id"]);
                    domain.Name3 = TransUtil.ToString(dataRow["code"]);
                    domain.Name4 = TransUtil.ToString(dataRow["orgname"]);
                    domain.Name5 = TransUtil.ToString(dataRow["bankaccount"]);
                    domain.Name6 = TransUtil.ToString(dataRow["bankname"]);
                    domain.Name7 = TransUtil.ToString(dataRow["address"]);
                    list.Add(domain);
                }
            }
            return list;
        }

        public IList QuerySupInfo()
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            command.CommandText =
                " select t3.catname, t1.suprelid id,t1.code,t2.orgname,t1.address,t1.bankname,t1.bankaccount " +
                " From ressupplierrelation t1,resorganization t2, ressuprelcategory t3 where " +
                " t1.orgid=t2.orgid and t1.supplierrelationcategoryid=t3.catid ";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain domain = new DataDomain();
                    //domain.Name1 = TransUtil.ToInt(dataRow["orgtype"]);
                    domain.Name8 = TransUtil.ToString(dataRow["catname"]);
                    domain.Name2 = TransUtil.ToString(dataRow["id"]);
                    domain.Name3 = TransUtil.ToString(dataRow["code"]);
                    domain.Name4 = TransUtil.ToString(dataRow["orgname"]);
                    domain.Name5 = TransUtil.ToString(dataRow["bankaccount"]);
                    domain.Name6 = TransUtil.ToString(dataRow["bankname"]);
                    domain.Name7 = TransUtil.ToString(dataRow["address"]);
                    list.Add(domain);
                }
            }
            return list;
        }

        public IList GetProjectInfo()
        {
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            DataDomain domain = null;
            string sSQL = @"select t.opgid,t.opgsyscode,t.opgname,t1.id,t1.projectname from  resoperationorg t 
join resconfig t1 on nvl(t1.projectcurrstate,0)!=20 and  t.opgid=t1.ownerorg  ";
            //sSQL = string.Format(sSQL,sProjectName);
            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    domain = new DataDomain();
                    domain.Name1 = TransUtil.ToString(dataRow["opgid"]);
                    domain.Name2 = TransUtil.ToString(dataRow["opgsyscode"]);
                    domain.Name3 = TransUtil.ToString(dataRow["opgname"]);
                    domain.Name4 = TransUtil.ToString(dataRow["id"]);
                    domain.Name5 = TransUtil.ToString(dataRow["projectname"]);
                    list.Add(domain);
                }
            }
            return list;
        }

        #endregion

        #region  未收、收款台账

        public IList QueryProjectGatheringAccountReport(string sProjectID, DateTime startDate, DateTime endDate)
        {
            DataRow[] arrDataRow = null;
            IList lstResult = new ArrayList();
            DataDomain oDataDomain = null;
            StringBuilder strDate = null;
            StringBuilder strInvoiceNo = null;
            string sInvoiceNo = string.Empty;
            string strNum = string.Empty;
            string sMasterID = string.Empty;
            string sOldMasterID = string.Empty;
            decimal dMoney = 0;
            Hashtable hsDate = new Hashtable();
            Hashtable hsInvoiceNo = new Hashtable();
            string sDate = string.Empty;
            string sDataSQL =
                @"select t.id masterID,t1.id,t.projectname ProjectName,to_char(t.createdate,'yyyy-mm-dd') CreateDate,
t1.gatheringmoney,t2.id AcceptBillID,t1.WaterElecMoney,t1.PenaltyMoney,t1.OtherMoney,t1.WorkerMoney,
t1.ConcreteMoney,t1.AgreementMoney,t1.OtherItemMoney,t1.Money
 from thd_gatheringmaster t join thd_gatheringdetail t1 on t.id=t1.parentid
left join thd_AcceptanceBill t2 on t1.id=t2.GATHERINGMXID and t2.state=5
where t.state=5 and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') and
t.ifprojectmoney=0 and t.projectid='{2}'";
            sDataSQL = string.Format(sDataSQL, startDate.Date.ToString("yyyy-MM-dd"),
                                     endDate.Date.ToString("yyyy-MM-dd"), sProjectID);
            string sDataInvoiceSQL =
                @"select t.id masterID,to_char(t1.createdate,'yyyy-mm-dd') createdate,t1.SumMoney,t1.invoiceno  
from thd_gatheringmaster t join thd_GatheringInvoice t1 on t.id=t1.gatheringid and t1.state=5
where t.state=5 and t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') and
t.ifprojectmoney=0 and t.projectid='{2}' order by t.id ,t1.createdate asc";
            sDataInvoiceSQL = string.Format(sDataInvoiceSQL, startDate.Date.ToString("yyyy-MM-dd"),
                                            endDate.Date.ToString("yyyy-MM-dd"), sProjectID);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sDataSQL;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);
            oCommand.CommandText = sDataInvoiceSQL;
            oRead = oCommand.ExecuteReader();
            DataSet dsInvoice = DataAccessUtil.ConvertDataReadertoDataSet(oRead);


            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    oDataDomain = new DataDomain()
                                      {
                                          Name1 = ClientUtil.ToString(oRow["Id"]), //明细ID
                                          Name2 = ClientUtil.ToString(oRow["ProjectName"]), //项目名称
                                          Name3 = ClientUtil.ToDateTime(oRow["CreateDate"]), //收款时间
                                          Name4 = ClientUtil.ToDecimal(oRow["GatheringMoney"]), //收款金额
                                          //Name5 = (oDetail.AcceptBillID != null && string.IsNullOrEmpty( oDetail.AcceptBillID.BillType)) ? oDetail.AcceptBillID.SumMoney : 0,//货币
                                          //Name6 = (oDetail.AcceptBillID != null && !string.IsNullOrEmpty(oDetail.AcceptBillID.BillType)) ? oDetail.AcceptBillID.SumMoney : 0,//票据金额

                                          Name5 =
                                              (oRow["AcceptBillID"] == DBNull.Value)
                                                  ? ClientUtil.ToDecimal(oRow["GatheringMoney"])
                                                  : 0, //货币
                                          Name6 =
                                              (oRow["AcceptBillID"] != DBNull.Value)
                                                  ? ClientUtil.ToDecimal(oRow["GatheringMoney"])
                                                  : 0, //票据金额
                                          Name7 = ClientUtil.ToDecimal(oRow["WaterElecMoney"]),
                                          // oDetail.WaterElecMoney,//代扣水费
                                          Name8 = ClientUtil.ToDecimal(oRow["PenaltyMoney"]),
                                          // oDetail.PenaltyMoney,//代扣罚款
                                          Name9 = ClientUtil.ToDecimal(oRow["OtherMoney"]),
                                          //oDetail.OtherMoney,//代扣费用其他
                                          Name10 = ClientUtil.ToDecimal(oRow["WorkerMoney"]),
                                          // oDetail.WorkerMoney,//代扣农民保障金
                                          Name11 = ClientUtil.ToDecimal(oRow["ConcreteMoney"]),
                                          // oDetail.ConcreteMoney,//代扣散装水泥押金
                                          Name12 = ClientUtil.ToDecimal(oRow["AgreementMoney"]),
                                          //oDetail.AgreementMoney,//代扣履约保证金
                                          Name13 = ClientUtil.ToDecimal(oRow["OtherItemMoney"]),
                                          //oDetail.OtherItemMoney,//代扣款项其他			
                                          Name14 = ClientUtil.ToDecimal(oRow["Money"]) // oDetail.Money//合计 
                                      };
                    sMasterID = ClientUtil.ToString(oRow["masterID"]);
                    if (!string.IsNullOrEmpty(sMasterID) && !string.Equals(sMasterID, sOldMasterID))
                    {
                        dMoney = 0;
                        hsDate.Clear();
                        hsInvoiceNo.Clear();
                        if (dsInvoice != null && dsInvoice.Tables.Count > 0 && dsInvoice.Tables[0].Rows.Count > 0)
                        {

                            sOldMasterID = sMasterID;
                            arrDataRow = dsInvoice.Tables[0].Select(string.Format(" masterID='{0}'", sMasterID));
                            if (arrDataRow != null && arrDataRow.Length > 0)
                            {

                                foreach (DataRow oRowTemp in arrDataRow)
                                {
                                    sDate = ClientUtil.ToString(oRowTemp["createdate"]);
                                    if (!hsDate.Contains(sDate))
                                    {
                                        hsDate.Add(sDate, null);
                                    }
                                    sInvoiceNo = ClientUtil.ToString(oRowTemp["invoiceno"]);
                                    if (!hsInvoiceNo.Contains(sInvoiceNo))
                                    {
                                        hsInvoiceNo.Add(sInvoiceNo, null);
                                    }

                                    dMoney += ClientUtil.ToDecimal(oRowTemp["SumMoney"]);

                                }
                            }
                        }
                        oDataDomain.Name15 = hsDate.Keys.Count == 0
                                                 ? ""
                                                 : string.Join("|", hsDate.Keys.OfType<string>().ToArray()); //发票日期
                        oDataDomain.Name16 = dMoney; //发票金额
                        oDataDomain.Name17 = hsInvoiceNo.Keys.Count == 0
                                                 ? ""
                                                 : string.Join("|", hsInvoiceNo.Keys.OfType<string>().ToArray()); //发票号
                    }
                    oDataDomain.Name18 = sMasterID;
                    lstResult.Add(oDataDomain);
                }
            }
            return lstResult;
        }

        public IList QueryProjectGatheringAccountReport1(string sProjectID, DateTime startDate, DateTime endDate)
        {
            IList lstResult = new ArrayList();
            GatheringMaster oMaster = null;
            DataDomain oDataDomain = null;
            string strDate = string.Empty;
            string strNum = string.Empty;
            decimal dMoney = 0;
            //oDetail.Master.ProjectId
            //oDetail.Master.IfProjectMoney
            //oDetail.Master.CreateDate
            //oDetail.Master.Details;明细
            // oDetail.Master.ListRel 业主报量
            //oDetail.Master.ListInvoice 收款发票
            //oDetail.AcceptBillID 关联票据
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddFetchMode("Master", FetchMode.Eager);
            oQuery.AddFetchMode("AcceptBillID", FetchMode.Eager);
            oQuery.AddFetchMode("Master.ListInvoice", FetchMode.Eager);
            oQuery.AddCriterion(Expression.Ge("Master.CreateDate", startDate));
            oQuery.AddCriterion(Expression.Lt("Master.CreateDate", endDate));
            oQuery.AddCriterion(Expression.Eq("Master.IfProjectMoney", 0));

            oQuery.AddCriterion(Expression.Eq("Master.ProjectId", sProjectID));

            IList lstDetail = Dao.ObjectQuery(typeof (GatheringDetail), oQuery);
            if (lstDetail != null)
            {
                foreach (GatheringDetail oDetail in lstDetail)
                {
                    if (oDetail.AcceptBillID != null && ClientUtil.ToString(oDetail.AcceptBillID.Id) != "")
                    {
                    }
                    oDataDomain = new DataDomain()
                                      {

                                          Name1 = oDetail.Id, //明细ID
                                          Name2 = oDetail.Master.ProjectName, //项目名称
                                          Name3 = oDetail.Master.CreateDate, //收款时间
                                          Name4 = oDetail.GatheringMoney, //收款金额
                                          //Name5 = (oDetail.AcceptBillID != null && string.IsNullOrEmpty( oDetail.AcceptBillID.BillType)) ? oDetail.AcceptBillID.SumMoney : 0,//货币
                                          //Name6 = (oDetail.AcceptBillID != null && !string.IsNullOrEmpty(oDetail.AcceptBillID.BillType)) ? oDetail.AcceptBillID.SumMoney : 0,//票据金额

                                          Name5 =
                                              (oDetail.AcceptBillID != null &&
                                               ClientUtil.ToString(oDetail.AcceptBillID.Id) != "")
                                                  ? oDetail.GatheringMoney
                                                  : 0, //货币
                                          Name6 = (oDetail.AcceptBillID != null) ? oDetail.GatheringMoney : 0, //票据金额
                                          Name7 = oDetail.WaterElecMoney, //代扣水费
                                          Name8 = oDetail.PenaltyMoney, //代扣罚款
                                          Name9 = oDetail.OtherMoney, //代扣费用其他
                                          Name10 = oDetail.WorkerMoney, //代扣农民保障金
                                          Name11 = oDetail.ConcreteMoney, //代扣散装水泥押金
                                          Name12 = oDetail.AgreementMoney, //代扣履约保证金
                                          Name13 = oDetail.OtherItemMoney, //代扣款项其他			
                                          Name14 = oDetail.Money //合计 
                                      };
                    oMaster = (oDetail.Master as GatheringMaster);
                    strDate = string.Empty;
                    strNum = string.Empty;
                    dMoney = 0;
                    if (oMaster.ListInvoice != null)
                    {
                        strDate = string.Join("|",
                                              oMaster.ListInvoice.Select(a => a.CreateDate.ToString("yyyy-MM-dd")).
                                                  ToArray<string>());
                        dMoney = oMaster.ListInvoice.Sum(a => a.SumMoney);
                        strNum = string.Join("|", oMaster.ListInvoice.Select(a => a.InvoiceNo).ToArray<string>());
                    }
                    oDataDomain.Name15 = strDate; //发票日期
                    oDataDomain.Name16 = dMoney; //发票金额
                    oDataDomain.Name17 = strNum; //发票号
                    lstResult.Add(oDataDomain);
                }
            }
            //                select   t.id masterID, t1.id id, 
            //decode(t.ifprojectmoney,1,t.projectname,'') projectName,
            //to_char(t.createdate ,'yyyy-mm-dd') as gatherDate,
            //t1.GatheringMoney sureMoney,
            //(case when exists(select 1 from thd_AcceptanceBill tt where tt.gatheringmxid=t1.id ) then 1 else 0 end ) currency ,  
            //(select sum(tt.summoney) from thd_AcceptanceBill tt where tt.gatheringmxid=t1.id ) billSumMoney,
            //t1.WaterElecMoney,t1.PenaltyMoney,t1.OtherMoney,t1.WorkerMoney,t1.ConcreteMoney,t1.AgreementMoney,t1.OtherItemMoney,t1.GatheringMoney sumMoney,
            //'' as InvoiceDate,
            //(select sum(summoney) from thd_GatheringInvoice tt1 where tt1.gatheringid=t.id) as InvoiceMoney,'' as InvoiceNum
            //from thd_gatheringmaster t
            //join thd_gatheringdetail t1 on t.id=t1.parentid;
            return lstResult;
        }

        public DataSet QueryOrgGatheringAccountReport(string sOrgSysCode, DateTime startDate, DateTime endDate)
        {
            //IList lstResult = new ArrayList();
            string sSQL = @"select (select t1.opgname  from resconfig t 
join resoperationorg t1 on t1.opgoperationtype='b'  and instr(t.ownerorgsyscode,t1.opgid)>0 
where t.id=tt.projectid) as opgname,tt.* 
from (select projectname,projectid,sum(gatheringmoney)gatheringmoney,sum(CurreyBillMoney)CurreyBillMoney,
sum(AcceptanceBillMoney)AcceptanceBillMoney,sum(WaterElecMoney) WaterElecMoney,sum(PenaltyMoney)PenaltyMoney,
sum(othermoney)othermoney,sum(WorkerMoney)WorkerMoney,sum(ConcreteMoney)ConcreteMoney,sum(AgreementMoney)AgreementMoney,
sum(OtherItemMoney)OtherItemMoney,sum(summoney)summoney,sum(InvoiceMoney)InvoiceMoney from (
select t.projectid,t.projectname,t.id ,sum(t1.gatheringmoney) gatheringmoney,
sum(case when t2.id is null then t1.gatheringmoney else 0 end  ) CurreyBillMoney,
sum(case when t2.id is null then 0 else t1.gatheringmoney  end  ) AcceptanceBillMoney,
sum(t1.WaterElecMoney) WaterElecMoney,
sum(t1.PenaltyMoney) PenaltyMoney,
sum(t1.othermoney)othermoney,
sum(t1.WorkerMoney )WorkerMoney,
sum(t1.ConcreteMoney) ConcreteMoney,
sum(t1.AgreementMoney) AgreementMoney,
sum(t1.OtherItemMoney) OtherItemMoney,
sum(t1.money) summoney,
nvl((select sum(tt.summoney) from thd_gatheringinvoice tt where tt.state=5 and t.id=tt.gatheringid),0) InvoiceMoney
from thd_gatheringmaster t 
join thd_gatheringdetail t1 on t.id=t1.parentid 
left join thd_AcceptanceBill t2 on t1.id=t2.gatheringmxid and t2.state=5
where t.state=5 and t.ifprojectmoney =0 and t.opgsyscode like '{0}%'  and t.createdate between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd')
group by t.projectid,t.projectname,t.id) group by projectname,projectid) tt
order by opgname,tt.projectname desc";
            sSQL = string.Format(sSQL, sOrgSysCode, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sSQL;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);

            return ds;
        }

        /// <summary>
        ///  土建/安装/装饰审定金额:查询结果为(小于endDate)审定累计之和 MustGathering
        ///  合同比例:获取当前项目的最大比例   RightRate
        ///  累计收款：获取对应项目的查询结果为(小于endDate)收款之和 Gathering 
        ///  合同应收工程款:审定累计之和-累计收款 MustGathering-Gathering
        ///  应收未收工程款:MustGathering*RightRate-累计收款
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet QueryProjectNotGatheringAccountReport(string sProjectID, DateTime startDate, DateTime endDate)
        {
            string sSQL =
                @"select confirmdate,quantitytype,gatheringrate,confirmstartdate,confirmenddate,DelayTime,confirmmoney,acctgatheringmoney,
GatheringDate,gathering from (
select rownum num,tt.* from (
select t1.confirmdate ,t.quantitytype ,
t1.gatheringrate,t1.confirmstartdate,t1.confirmenddate,0 as DelayTime,t1.confirmmoney,t1.acctgatheringmoney
from thd_ownerquantitymaster t
join thd_ownerquantitydetail t1 on t.id=t1.parentid
where t.state=5  and t.projectid='{2}' and t1.confirmdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')
order by t1.confirmdate asc) tt) ttt 
full join (select rownum num,tt.* from (
select  t.createdate GatheringDate,t.SUMMONEY gathering from thd_gatheringmaster t
where t.state=5 and t.IFPROJECTMONEY=0 and t.projectid='{2}' and 
t.createdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')
order by t.createdate asc,t.realoperationdate asc)tt
) ttt1 on ttt.num=ttt1.num";
            sSQL = string.Format(sSQL, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), sProjectID);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sSQL;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);

            return ds;
        }

        /// <summary>
        ///  土建/安装/装饰审定金额:查询结果为(小于endDate)审定累计之和 MustGathering
        ///  合同比例:获取当前项目的最大比例   RightRate
        ///  累计收款：获取对应项目的查询结果为(小于endDate)收款之和 Gathering 
        ///  合同应收工程款:审定累计之和-累计收款 MustGathering-Gathering
        ///  应收未收工程款:MustGathering*RightRate-累计收款
        /// </summary>
        /// <param name="sOrgSysCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet QueryOrgNotGatheringAccountReport(string sOrgSysCode, DateTime startDate, DateTime endDate)
        {
            string sSQL = @"select * from (select 
(select t1.opgname  from resconfig t
join resoperationorg t1 on instr(t.ownerorgsyscode, t1.opgid)>0 and t1.oPGOPERATIONTYPE='b'
where t.id=tt.projectid) orgName,
(select count(0)  from thd_ownerquantitymaster t 
 join thd_ownerquantitydetail t1 on t.id=t1.parentid  and t1.confirmdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')
where  nvl(t.projectid,'-1')=tt.projectid and t.state=5 and t.opgsyscode like '{2}%') flag,
tt.*,(select max(ttt1.gatheringrate) from thd_ownerquantitymaster ttt 
join thd_ownerquantitydetail ttt1 on ttt.id=ttt1.parentid where ttt.state=5 and nvl(ttt.projectid,'1')=tt.projectid and (ttt1.confirmdate between to_date('{0}','yyyy-mm-dd') and tt.confirmdate)  )  gatheringrate
from(select t2.projectid,t2.projectname,
sum(case t.quantitytype when '安装' then t1.confirmmoney else 0 end) azConfrim,
sum(case t.quantitytype when  '土建' then t1.confirmmoney else 0 end) tjConfrim,
sum(t1.acctgatheringmoney) acctgatheringmoney,
max(t1.confirmdate) confirmdate,
(select sum(tt.summoney) from thd_gatheringmaster tt where tt.ifprojectmoney=0 and tt.state=5 and tt.createdate  between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')
 and nvl(tt.projectid,'-1')=t2.projectid) gathering
from (
select to_char(nvl(projectid,'-1')) projectid,to_char(projectname)projectname from thd_ownerquantitymaster t , thd_ownerquantitydetail t1    where t.id=t1.parentid and t.state=5 and t.opgsyscode like '{2}%' and t1.confirmdate  between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')
union  
select nvl(projectid,'-1'),projectname from thd_gatheringmaster  where ifprojectmoney=0 and state=5 and opgsyscode like '{2}%' and createdate  between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd') 
) t2
left join thd_ownerquantitymaster t on nvl(t.projectid,'-1')=t2.projectid and t.state=5 and t.opgsyscode like '{2}%'
left join thd_ownerquantitydetail t1 on t.id=t1.parentid  and t1.confirmdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')
group by t2.projectid,t2.projectname)tt) order by  orgName,projectname desc";
            sSQL = string.Format(sSQL, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), sOrgSysCode);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sSQL;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);

            return ds;
        }

        public DataSet QueryMainBusinessInComeReport(string sProjectID, string sSysCode, int iYear, int iMonth,
                                                     bool isOnlyOne)
        {
            var subCompanys = GetSubCompanySyscodeList();
            var isSubCompay = subCompanys != null && subCompanys.Contains(sSysCode);

            var sSQL = string.Empty;
            if (!string.IsNullOrEmpty(sProjectID))
            {
                sSQL = string.Format(SQLScript.MainBusinessInComeReportByProjectSql,
                                     iYear, iMonth.ToString().PadLeft(2, '0'), sProjectID, isOnlyOne ? "" : "--");
            }
            else if (isSubCompay)
            {
                sSQL = string.Format(SQLScript.MainBusinessInComeReportBySubSql,
                                     iYear, iMonth, sSysCode, iMonth.ToString().PadLeft(2, '0'),
                                     isOnlyOne ? iMonth.ToString().PadLeft(2, '0') : "01",
                                     isOnlyOne ? "=" : "<=");
            }
            else
            {
                sSQL = string.Format(SQLScript.MainBusinessInComeReportByHeadquartersSql,
                                     iYear, iMonth, sSysCode, iMonth.ToString().PadLeft(2, '0'),
                                     isOnlyOne ? iMonth.ToString().PadLeft(2, '0') : "01",
                                     isOnlyOne ? "=" : "<=");
            }

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sSQL;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);
            return ds;
        }

        public IList GetSubCompanySyscodeList()
        {
            IList list = new ArrayList(); //分公司层次码集合
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText =
                "select t1.opgsyscode from resoperationorg t1 where t1.opgstate=1 and t1.opgoperationtype='b'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    list.Add(TransUtil.ToString(dataRow["opgsyscode"]));
                }
            }
            return list;
        }

        public string GetOrgType(string sOrgID)
        {
            string sSQL =
                @"select (case when  exists(select 1  from resconfig t where t.ownerorg='{0}' and t.projectcode!='0000') then '项目'
              when  exists(select 1 from resoperationorg t1 where t1.opgstate=1 and t1.opgoperationtype='b' and t1.opgid='{0}') then '分公司'
              when  exists(select 1 from resoperationorg t1 where t1.opgstate=1 and t1.opgoperationtype='h' and t1.opgid='{0}') then '总部'
              else '其他' end   ) from dual";
            sSQL = string.Format(sSQL, sOrgID);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sSQL;
            return ClientUtil.ToString(command.ExecuteScalar());
        }

        public DataSet QueryCostPaymentReport(string sProjectID, string sSysCode, int iYear, int iMonth)
        {
            var subCompanys = GetSubCompanySyscodeList();
            var isSubCompay = subCompanys != null && subCompanys.Contains(sSysCode);

            var sSQL = string.Empty;
            if (!string.IsNullOrEmpty(sProjectID))
            {
                sSQL = string.Format(SQLScript.CostPaymentReportByProjectSql,
                                     iYear, iMonth, sProjectID, iMonth.ToString().PadLeft(2, '0'));
            }
            else if (isSubCompay)
            {
                sSQL = string.Format(SQLScript.CostPaymentReportBySubSql,
                                     iYear, iMonth, sSysCode, iMonth.ToString().PadLeft(2, '0'));
            }
            else
            {
                sSQL = string.Format(SQLScript.CostPaymentReportByHeadquartersSql,
                                     iYear, iMonth, sSysCode, iMonth.ToString().PadLeft(2, '0'));
            }

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sSQL;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);
            return ds;
        }

        public DataSet QueryGatherAndPayDepositReport(DateTime dEnd, string sProjectID, string sSysCode,
                                                      string sAccountTitleName, string sTheSupplierRelationInfo,
                                                      string sTheCustomerRelationInfo, string sDeptType)
        {

            //(select tt1.opgname  from resoperationorg tt1 where tt1.opgoperationtype='b'  and instr(t.opgsyscode,tt1.opgid)>0)  as opgname,
            string sSQL =
                @" with temp1 as (select nvl(t.opgsyscode,'') opgsyscode,nvl(t.projectname,'')projectname,nvl(nvl(t.supplierrelationname,t.thecustomername),'')deptname,
nvl((case when t.supplierrelationname is not null then '分供商'   when t.thecustomername is not null then '客户' else '' end  ),'') depttype,
nvl(t.accounttitlename,'')accounttitlename,t.summoney paymoney,0 gathermoney,t.createdate
from thd_paymentmaster t  where t.state=5 and  t.createdate<=to_date('{0}','yyyy-mm-dd') {2}
union all
select nvl(t.opgsyscode,'') opgsyscode,nvl(t.projectname,'')projectname,nvl(nvl(t.supplierrelationname,t.thecustomername),'')deptname,
nvl((case when t.supplierrelationname is not null then '分供商'   when t.thecustomername is not null then '客户' else '' end  ),'') depttype,
nvl(t.accounttitlename,'')accounttitlename,0 paymoney,t.summoney gathermoney,t.createdate
from thd_gatheringmaster t where t.state=5 and t.createdate<=to_date('{0}','yyyy-mm-dd') {2} )
select opgname,projectname,depttype,deptname,accounttitlename ,sum(lastremainmoney) lastremainmoney,sum(paymoney)paymoney,sum(gathermoney)gathermoney
　from (select 
nvl((select tt1.opgname  from resoperationorg tt1 where tt1.opgoperationtype='b'  and instr(t.opgsyscode,tt1.opgid)>0),
    (select tt1.opgname  from resoperationorg tt1 where tt1.opgoperationtype='h'  and instr(t.opgsyscode,tt1.opgid)>0) ) as opgname,
nvl(t.projectname,(select tt.opgname from resoperationorg tt where tt.opgsyscode=t.opgsyscode)) as projectname,
t.depttype,t.deptname,accounttitlename,lastremainmoney,paymoney,gathermoney
from (select t.opgsyscode,t.projectname,t.depttype,t.deptname,accounttitlename,
sum(case when t.createdate<to_date('{1}','yyyy-mm-dd') then  t.paymoney-t.gathermoney   else 0 end) lastremainmoney,
sum(case when t.createdate between to_date('{1}','yyyy-mm-dd') and to_date('{0}','yyyy-mm-dd') then t.paymoney   else 0 end) paymoney,
sum(case when t.createdate between to_date('{1}','yyyy-mm-dd') and to_date('{0}','yyyy-mm-dd') then t.gathermoney   else 0 end) gathermoney
from temp1 t
group by t.opgsyscode,t.projectname,t.depttype,t.deptname,t.accounttitlename) t) group by opgname,projectname,depttype,deptname,accounttitlename order by opgname,projectname,depttype,deptname,accounttitlename ";
            string sWhere = string.Empty;
            if (string.IsNullOrEmpty(sSysCode))
            {
                sWhere = string.Format(" and t.projectid='{0}' ", sProjectID);
            }
            else
            {
                sWhere = string.Format(" and t.opgsyscode like '{0}%' ", sSysCode);
            }
            if (!string.IsNullOrEmpty(sAccountTitleName))
            {
                sWhere = string.Format("{0} and t.accounttitlename in({1}) ", sWhere, sAccountTitleName);
            }
            if (!string.IsNullOrEmpty(sTheCustomerRelationInfo))
            {
                sWhere = string.Format("{0} and t.thecustomerrelationinfo='{1}'  ", sWhere, sTheCustomerRelationInfo);
            }
            if (!string.IsNullOrEmpty(sTheSupplierRelationInfo))
            {
                sWhere = string.Format("{0} and t.supplierrelation='{1}'  ", sWhere, sTheSupplierRelationInfo);
            }
            if (!string.IsNullOrEmpty(sDeptType))
            {
                if (sDeptType == "客户")
                {
                    sWhere = string.Format("{0} and t.thecustomerrelationinfo is not null ", sWhere);
                }
                else if (sDeptType == "分供商")
                {
                    sWhere = string.Format("{0} and t.supplierrelation is not null ", sWhere);
                }
                else
                {
                }

            }
            sSQL = string.Format(sSQL, dEnd.ToString("yyyy-MM-dd"), string.Format("{0}-01-01", dEnd.Year), sWhere);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sSQL;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);
            return ds;
        }

        #endregion

        #region 资金结账

        private bool IsCanOperateLockAccount(int year, int mon, string projId, string id)
        {
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", projId));
            if (!string.IsNullOrEmpty(id))
            {
                objQuery.AddCriterion(Expression.Not(Expression.Eq("Id", id)));

            }

            var result = Query(typeof (LockAccountMaster), objQuery);
            if (result == null || result.Count == 0)
            {
                return true;
            }

            var rList = result.OfType<LockAccountMaster>().ToList();
            var maxRes = rList.OrderByDescending(r => r.AccountYear).ThenByDescending(r => r.AccountMonth).First();
            var maxDate = new DateTime(maxRes.AccountYear, maxRes.AccountMonth, 1);
            var opDate = new DateTime(year, mon, 1);

            return maxDate.AddMonths(1) == opDate;
        }

        /// <summary>
        /// 根据资金结账情况，判断当前业务是否允许发生
        /// 有错误信息即表示不允许该业务，否则允许
        /// </summary>
        /// <param name="projId">项目Id</param>
        /// <param name="businessDate">业务日期</param>
        /// <returns></returns>
        public string IsAllowBusinessHappend(string projId, DateTime businessDate)
        {
            try
            {
                ObjectQuery objQuery = new ObjectQuery();
                objQuery.AddCriterion(Expression.Eq("ProjectId", projId));

                var result = Query(typeof (LockAccountMaster), objQuery).OfType<LockAccountMaster>().ToList();
                if (result == null || result.Count == 0)
                {
                    return null;
                }

                var lastLimit = result.OrderByDescending(l => l.AccountEndDate).First();
                if (businessDate.Date <= lastLimit.AccountEndDate)
                {
                    return string.Format("项目[{0}]的结账日期为{1}，业务的发生日期必须大于该日期", lastLimit.ProjectName,
                                         lastLimit.AccountEndDate.ToShortDateString());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return string.Format("判断业务是否可以发生出错：{0}", ex.Message);
            }
        }

        [TransManager]
        public string SaveAccountLockMaster(IList lstLock)
        {
            if (lstLock == null || lstLock.Count == 0)
            {
                return "没有待结账的数据";
            }

            StringBuilder mes = new StringBuilder();
            var sveList = new List<LockAccountMaster>();
            foreach (LockAccountMaster mt in lstLock)
            {
                if (!IsCanOperateLockAccount(mt.AccountYear, mt.AccountMonth, mt.ProjectId, null))
                {
                    mes.AppendLine(mt.ProjectName);
                    continue;
                }

                sveList.Add(mt);
            }

            var rt = Dao.SaveOrUpdate(sveList);
            if (!rt)
            {
                return "保存资金结账信息失败";
            }

            if (mes.Length > 0)
            {
                return string.Format("以下项目的前一个会计期间未结账：\r\n{0}", mes.ToString());
            }

            return null;
        }

        [TransManager]
        public string DeleteAccountLockMaster(IList lstLock)
        {
            if (lstLock == null || lstLock.Count == 0)
            {
                return "没有待反结账的数据";
            }

            StringBuilder mes = new StringBuilder();
            var delList = new List<LockAccountMaster>();

            foreach (LockAccountMaster mt in lstLock)
            {
                if (!IsCanOperateLockAccount(mt.AccountYear, mt.AccountMonth, mt.ProjectId, mt.Id))
                {
                    mes.AppendLine(mt.ProjectName);
                    continue;
                }

                delList.Add(mt);
            }

            var rt = Dao.Delete(delList);
            if (!rt)
            {
                return "保存资金反结账信息失败";
            }

            if (mes.Length > 0)
            {
                return string.Format("以下项目的后一个会计期间未反结账：\r\n{0}", mes.ToString());
            }

            return null;
        }

        #endregion

        #region 时间期间定义、施工节点进度、项目启动确认

        public IList GetDatePeriodDefineByYear(int year, bool isFilterInvalid)
        {
            if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
            {
                return null;
            }

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Like("PeriodCode", year.ToString(), MatchMode.Start));
            if (isFilterInvalid)
            {
                objectQuery.AddCriterion(Expression.Not(Expression.Eq("PeriodType", PeriodType.InValid)));
            }

            return Dao.ObjectQuery(typeof (DatePeriodDefine), objectQuery);
        }

        public IList GetConstructNodeByProject(string projId)
        {
            if (string.IsNullOrEmpty(projId))
            {
                return null;
            }

            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projId));
            objectQuery.AddFetchMode("DatePeriod", FetchMode.Eager);
            objectQuery.AddFetchMode("WBSTree", FetchMode.Eager);

            return Dao.ObjectQuery(typeof (ConstructNode), objectQuery);
        }

        #endregion

        #region 资金策划

        [TransManager]
        public FundPlanningMaster SaveFundScheme(FundPlanningMaster fund)
        {
            if (fund == null)
            {
                return null;
            }

            if (fund.Id == null)
            {
                fund.Code = GetCode(typeof (FundPlanningMaster));
            }

            if (!Dao.SaveOrUpdate(fund))
            {
                return fund;
            }

            if (fund.DocState == DocumentState.InAudit)
            {
                var appBill = new ApproveBill();
                appBill.BillCode = fund.Code;
                appBill.BillCreateDate = fund.CreateDate;
                appBill.BillCreatePerson = fund.CreatePerson;
                appBill.BillCreatePersonName = fund.CreatePersonName;
                appBill.BillId = fund.Id;
                appBill.BillSysCode = fund.OpgSysCode;
                appBill.ProjectId = fund.ProjectId;
                appBill.ProjectName = fund.ProjectName;

                RefAppSrv.SaveApproveBill(appBill, typeof (FundPlanningMaster));
            }
            return fund;
        }

        public FundSchemeEfficiencyMaster SaveSchemeEfficiency(FundSchemeEfficiencyMaster efficiencyMaster)
        {
            if (efficiencyMaster == null)
            {
                return null;
            }

            if (efficiencyMaster.Id == null)
            {
                efficiencyMaster.Code = GetCode(typeof(FundSchemeEfficiencyMaster));
            }

            Dao.SaveOrUpdate(efficiencyMaster);
            return efficiencyMaster;
        }

        public IList GetFundSchemeByEditor(PersonInfo per)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("CreatePerson", per));
            objectQuery.AddFetchMode("CreatePerson", FetchMode.Eager);
            objectQuery.AddFetchMode("OperOrgInfo", FetchMode.Eager);

            return Dao.ObjectQuery(typeof (FundPlanningMaster), objectQuery);
        }

        public FundPlanningMaster GetFundSchemeById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));

            var list = Dao.ObjectQuery(typeof (FundPlanningMaster), objectQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0] as FundPlanningMaster;
        }

        public PaymentMaster GetFundPaymentMasterById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));

            var list = Dao.ObjectQuery(typeof(PaymentMaster), objectQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0] as PaymentMaster;
        }

        public IList GetFundSchemeByProject(string projeId)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projeId));
            objectQuery.AddOrder(new Order("Code", false));

            return Dao.ObjectQuery(typeof (FundPlanningMaster), objectQuery);
        }

        public IList QueryFundSchemeReportAmountByScheme(string schId)
        {
            var scheme = Dao.Get(typeof (FundPlanningMaster), schId) as FundPlanningMaster;
            if (string.IsNullOrEmpty(schId) || scheme == null)
            {
                return null;
            }

            var ds = QueryDataToDataSet(string.Format(SQLScript.QueryFundSchemeReportAmountBySchemeSql, schId));
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var retList = new List<FundSchemeReportAmount>();
            retList.Add(QueryLastYearFundSchemeReportAmount(schId));

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var colIndex = 0;
                var y = Convert.ToInt32(ds.Tables[0].Rows[i][colIndex++]);
                var m = Convert.ToInt32(ds.Tables[0].Rows[i][colIndex++]);
                var item = retList.Find(c => c.Year == y && c.Month == m);
                if (item == null)
                {
                    item = new FundSchemeReportAmount();
                    item.Master = scheme;
                    item.ItemGuid = Guid.NewGuid().ToString();
                    item.Year = y;
                    item.Month = m;
                    retList.Add(item);
                }
                if (string.IsNullOrEmpty(item.JobNameLink))
                {
                    item.JobNameLink = ds.Tables[0].Rows[i][colIndex++].ToString();
                }
                else
                {
                    item.JobNameLink = item.JobNameLink + "、" + ds.Tables[0].Rows[i][colIndex++];
                }
                item.CurrentEngineeringFee += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentMeasureFee += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentInnerSetup += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentSubcontractor += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentOutputTax += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentCommonSpecialCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentLaborCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentSteelCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentConcreteCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentOtherMaterialCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentLeasingCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentUtilitiesCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentOtherEquipmentCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentGovernmentFee += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentOtherDirectCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentIndirectCost += Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex]);
            }

            var totalRows = new List<string>() {"项目竣工验收", "完工结算", "质保期", "合计"};
            foreach (var rw in totalRows)
            {
                var item = new FundSchemeReportAmount();
                item.Master = scheme;
                item.JobNameLink = rw;
                retList.Add(item);
            }

            var rIndex = 1;
            retList.ForEach(r => r.RowIndex = rIndex++);

            return retList;
        }

        private FundSchemeReportAmount QueryLastYearFundSchemeReportAmount(string schId)
        {
            var scheme = Dao.Get(typeof (FundPlanningMaster), schId) as FundPlanningMaster;
            if (string.IsNullOrEmpty(schId) || scheme == null)
            {
                return null;
            }

            FundSchemeReportAmount item = new FundSchemeReportAmount();
            item.JobNameLink = "期初";

            var ds = QueryDataToDataSet(string.Format(SQLScript.QueryLastYearFundSchemeReportAmountSql, schId));
            if (ds == null || ds.Tables.Count == 0)
            {
                return item;
            }

            var colIndex = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                colIndex = 0;
                item.Master = scheme;
                item.ItemGuid = Guid.NewGuid().ToString();
                item.CurrentEngineeringFee = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentMeasureFee = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentInnerSetup = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentSubcontractor = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentOutputTax = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentCommonSpecialCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentLaborCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentSteelCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentConcreteCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentOtherMaterialCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentLeasingCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentUtilitiesCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentOtherEquipmentCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentGovernmentFee = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentOtherDirectCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex++]);
                item.CurrentIndirectCost = Convert.ToDecimal(ds.Tables[0].Rows[i][colIndex]);
            }

            return item;
        }

        [TransManager]
        public bool SaveFundSchemeDetail(List<IList> details)
        {
            if (details == null || details.Count == 0)
            {
                return false;
            }

            foreach (var detail in details)
            {
                Dao.SaveOrUpdate(detail);
            }
            return true;
        }

        [TransManager]
        public FundPlanningMaster ClearFundSchemeDetail(string schId)
        {
            if (string.IsNullOrEmpty(schId))
            {
                return null;
            }

            var scheme = GetFundSchemeById(schId);
            if (scheme == null)
            {
                return null;
            }

            DeleteFundSchemeReportAmount(scheme);

            DeleteFundSchemeGethering(scheme);

            DeleteFundSchemeIndirectTaxRate(scheme);

            DeleteFundSchemePayment(scheme);

            DeleteFundSchemeFinanceFee(scheme);

            DeleteFundSchemeSummary(scheme);

            DeleteFundSchemeContrast(scheme);

            DeleteFundSchemeCashCostRate(scheme);

            return scheme;
        }

        public IList GetFundSchemeDetail(FundPlanningMaster scheme, Type tp)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Master", scheme));
            objectQuery.AddOrder(new Order("RowIndex", true));
            objectQuery.AddFetchMode("Master", FetchMode.Eager);

            return Dao.ObjectQuery(tp, objectQuery);
        }

        private void DeleteFundSchemeReportAmount(FundPlanningMaster scheme)
        {
            var list = GetFundSchemeDetail(scheme, typeof (FundSchemeReportAmount));
            foreach (FundSchemeReportAmount ra in list)
            {
                scheme.CostCalculationDtl.Remove(ra);
                ra.Master = null;
                Dao.Delete(ra);
            }
        }

        private void DeleteFundSchemeGethering(FundPlanningMaster scheme)
        {
            var list = GetFundSchemeDetail(scheme, typeof (FundSchemeGathering));
            foreach (FundSchemeGathering ra in list)
            {
                scheme.GatheringCalculationDtl.Remove(ra);
                ra.Master = null;
                Dao.Delete(ra);
            }
        }

        private void DeleteFundSchemeIndirectTaxRate(FundPlanningMaster scheme)
        {
            var list = GetFundSchemeDetail(scheme, typeof (FundSchemeIndirectTaxRate));
            foreach (FundSchemeIndirectTaxRate ra in list)
            {
                scheme.IndirectInputCalculate.Remove(ra);
                ra.Master = null;
                Dao.Delete(ra);
            }
        }

        private void DeleteFundSchemePayment(FundPlanningMaster scheme)
        {
            var list = GetFundSchemeDetail(scheme, typeof (FundSchemePayment));
            foreach (FundSchemePayment ra in list)
            {
                scheme.PaymentCalculationDtl.Remove(ra);
                ra.Master = null;
                Dao.Delete(ra);
            }
        }

        private void DeleteFundSchemeFinanceFee(FundPlanningMaster scheme)
        {
            var list = GetFundSchemeDetail(scheme, typeof (FundSchemeFinanceFee));
            foreach (FundSchemeFinanceFee ra in list)
            {
                scheme.FinanceFeeCalculate.Remove(ra);
                ra.Master = null;
                Dao.Delete(ra);
            }
        }

        private void DeleteFundSchemeSummary(FundPlanningMaster scheme)
        {
            var list = GetFundSchemeDetail(scheme, typeof (FundSchemeSummary));
            foreach (FundSchemeSummary ra in list)
            {
                scheme.FundSummaryDtl.Remove(ra);
                ra.Master = null;
                Dao.Delete(ra);
            }
        }

        private void DeleteFundSchemeContrast(FundPlanningMaster scheme)
        {
            var list = GetFundSchemeDetail(scheme, typeof (FundSchemeContrast));
            foreach (FundSchemeContrast ra in list)
            {
                scheme.FundCalculateContrastDtl.Remove(ra);
                ra.Master = null;
                Dao.Delete(ra);
            }
        }

        private void DeleteFundSchemeCashCostRate(FundPlanningMaster scheme)
        {
            var list = GetFundSchemeDetail(scheme, typeof (FundSchemeCashCostRate));
            foreach (FundSchemeCashCostRate ra in list)
            {
                scheme.CashCostRateCalculationDtl.Remove(ra);
                ra.Master = null;
                Dao.Delete(ra);
            }
        }

        #endregion

        #region 工程项目收支分析表

        public ProjectBalanceOfPayment SaveOrUpdateProjectBalanceOfPayment(ProjectBalanceOfPayment obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        public bool DeleteProjectBalanceOfPayment(ProjectBalanceOfPayment obj)
        {
            return Dao.Delete(obj);
        }

        public IList QueryProjectBalanceOfPayment(ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(typeof (ProjectBalanceOfPayment), oQuery);
        }

        public IList QueryProjectUnBalanceOfPayment(string sOrgID, string sProjectID, int iYear, int iMonth)
        {
            IList lstResult = new ArrayList();
            ProjectBalanceOfPayment oProjectBalanceOfPayment = null;
            string sSQL = string.Empty;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            DateTime startDate = new DateTime(iYear, iMonth, 2);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);

            sSQL = @"select t.projectname,t1.opgname,t.id from resconfig t 
join resoperationorg t1 on t1.opgoperationtype='b' and instr(t.ownerorgsyscode,t1.opgid)>0 
where t.projectcurrstate!=20 and {0} and t.id  in (SELECT DISTINCT PROJECTID FROM ((select t.projectid from thd_paymentmaster t   where t.state=5 and t.projectid is not null
 union 
select t.projectid from thd_gatheringmaster t where t.state=5 and t.projectid is not null )
minus 
select t.projectid  from thd_projectbalanceofpayment t where t.state=5 and t.createyear={1} and t.createmonth={2})
)
order by t1.opgname ,t.projectname";
            // DateTime date = new DateTime(iYear, iMonth, 2);
            // string sStartTime = string.Format("to_date('{0}', 'yyyy-mm-dd')", date.ToString("yyyy-MM-dd"));
            //string sEndTime = string.Format("to_date('{0} 23:59:59', 'yyyy-mm-dd hh24:mi:ss')", date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"));
            string sWhere = string.IsNullOrEmpty(sProjectID)
                                ? string.Format(" instr( t1.opgsyscode,'{0}')>0 ", sOrgID)
                                : string.Format(" t.id='{0}' ", sProjectID);
            sSQL = string.Format(sSQL, sWhere, iYear, iMonth);


            oCommand.CommandText = sSQL;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oRow in ds.Tables[0].Rows)
                {
                    oProjectBalanceOfPayment = new ProjectBalanceOfPayment()
                                                   {
                                                       Id = "-1",
                                                       SubCompanyName = ClientUtil.ToString(oRow["opgname"]),
                                                       ProjectName = ClientUtil.ToString(oRow["projectname"])
                                                   };
                    lstResult.Add(oProjectBalanceOfPayment);
                }
            }

            return lstResult;
        }

        public IList SaveOrUpdateProjectBalanceOfPayment(IList lst)
        {
            Dao.SaveOrUpdate(lst);
            return lst;
        }

        public ProjectBalanceOfPayment GenerateProjectBalanceOfPayment(string sProjectId, int iYear, int iMonth)
        {
            IList lst = null;
            ProjectBalanceOfPayment oProjectBalanceOfPayment = null, oLastProjectBalanceOfPayment = null;
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("ProjectId", sProjectId));
            oQuery.AddCriterion(Expression.Eq("CreateYear", iYear));
            oQuery.AddCriterion(Expression.Eq("CreateMonth", iMonth));
            lst = QueryProjectBalanceOfPayment(oQuery);
            oProjectBalanceOfPayment = (lst != null && lst.Count > 0)
                                           ? (lst[0] as ProjectBalanceOfPayment)
                                           : new ProjectBalanceOfPayment() {CreateYear = iYear, CreateMonth = iMonth};

            #region 获取上期的 合同收款条款    工程类型 业主类型 工程状态 应付账款 应收欠款当前工程状态合同收款率

            if (string.IsNullOrEmpty(oProjectBalanceOfPayment.Id)) //如果本月单据为非重新生成的收支表 不需要取上个月数据进行填充
            {

                oQuery = new ObjectQuery();
                oQuery.AddCriterion(Expression.Eq("ProjectId", sProjectId));
                oQuery.AddCriterion(Expression.Eq("CreateYear", iMonth == 1 ? iYear - 1 : iYear));
                oQuery.AddCriterion(Expression.Eq("CreateMonth", iMonth == 1 ? iMonth = 12 : iMonth - 1));
                lst = QueryProjectBalanceOfPayment(oQuery);
                if (lst != null && lst.Count > 0)
                {
                    //获取上期的 合同收款条款    工程类型 业主类型 工程状态 应付账款 当前工程状态收款率
                    oLastProjectBalanceOfPayment = lst[0] as ProjectBalanceOfPayment;
                    oProjectBalanceOfPayment.ContractContent = oLastProjectBalanceOfPayment.ContractContent;
                    oProjectBalanceOfPayment.ProjectType = oLastProjectBalanceOfPayment.ProjectType;
                    oProjectBalanceOfPayment.OwnerType = oLastProjectBalanceOfPayment.OwnerType;
                    oProjectBalanceOfPayment.ProjectState = oLastProjectBalanceOfPayment.ProjectState;
                    oProjectBalanceOfPayment.MustPayment = oLastProjectBalanceOfPayment.MustPayment;
                    oProjectBalanceOfPayment.ContractGatheringRate = oLastProjectBalanceOfPayment.ContractGatheringRate;
                }

            }

            #endregion

            #region 生成 工程项目收支分析表

            DateTime date = new DateTime(iYear, iMonth, 2);

            //时间范围 本月2号到下个月1号
            string sStartTime = string.Format("to_date('{0}', 'yyyy-mm-dd')", date.ToString("yyyy-MM-dd"));
            // string sStartTime1 = string.Format("to_date('{0}', 'yyyy-mm-dd')", (new DateTime(iYear, iMonth, 1).ToString("yyyy-MM-dd")));
            string sEndTime = string.Format("to_date('{0} 23:59:59', 'yyyy-mm-dd hh24:mi:ss')",
                                            date.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"));
            //string sEndTime1 = string.Format("to_date('{0} 23:59:59', 'yyyy-mm-dd hh24:mi:ss')", (new DateTime(iYear, iMonth + 1, 1)).AddDays(-1).ToString("yyyy-MM-dd"));
            string sCurYear = string.Format("to_date('{0}', 'yyyy-mm-dd')",
                                            (new DateTime(iYear, 1, 2)).ToString("yyyy-MM-dd"));
            //string sCurYear1 = string.Format("to_date('{0}', 'yyyy-mm-dd')", (new DateTime(iYear, 1, 1)).ToString("yyyy-MM-dd"));
            //0:sStartTime  1:sEndTime  2:sProjectId 3:sCurYear 4:sStartTime1 5:sEndTime1 6:sCurYear1 7:iYear 8:iMonth
            //projectname 项目名称  Projectcost 造价成本 Resproportion 合同上缴比例   
            //FinanBalanceTotal 财务列报结算累计数 FinanBalanceCurrent 财务列报结算本年数 
            //CivilandsetupbalanceTotal 土建安装结算累计数 CivilandsetupbalanceYear 土建安装结算年累计数 CivilandsetupbalanceMonth 土建安装结算月累计数
            //SetupprojectbuildTotal 安装工程施工累计数 SetupprojectbuildYear 安装工程施工年计数 SetupprojectbuildMonth 安装工程施工月计数
            //.setuppayoutTotal 安装资金支出累计数 setuppayoutyear 安装资金支出年计数 setuppayoutmonth 安装资金支出月计数
            //土建装饰结算
            //装饰工程施工
            //项目成本支出t1.projectCostPaymentTotal,t1.projectCostPaymentYear,t1.projectCostPaymentMonth,
            //土建安装付款: civilandsetuppayoutTotal ,  civilandsetuppayoutyear, civilandsetuppayoutmonth,
            //主营业务t1.mainBusinessTotal,t1.mainBusinessYear,t1.mainBusinessMonth,
            //资金存量t2.remainMoneyTotal ,t2.remainMoneyYear,t2.remainMoneyMonth,
            //submitquantity 商务报量 confirmmoney 业主实际确认 AcctGatheringMoney 业主保量合同应收  ConfirmDate 业主审量签字时间 ConfirmEndDate 业主审量确认时间
            //gatheringtotal 收款累计数  gatheringyear 收款年累计数  gatheringmonth 收款月累计数
            //paymenttotal 付款累计数 paymentyear付款本年数 paymentmonth 付款本月数
            //deposit 保证金押金
            string sSQL =
                @"WITH temp1 as (select 0 CivilProjectBalance,0 Civilandsetupbalance,0 Setupprojectbuild,0 setuppayout,0 civilandsetuppayout, t1.money projectCostPayment,t1.money mainBusiness ,t.createdate from thd_indirectcostmaster t join thd_indirectcostdetail t1 on t.id = t1.parentid where  t.state=5 and t1.accounttitlecode like '54010106%' and   t.createdate<={1} and t.projectid='{2}'
                                union all 
                                select  t1.CivilProjectBalance CivilProjectBalance,t1.Civilandsetupbalance,t1.Setupprojectbuild,t1.setuppayout,t1.civilandsetuppayout,
                                (t1.SUBPROJECTPAYOUT + t1.PERSONCOST + t1.MATERIALCOST + t1.MECHANICALCOST + t1.OTHERDIRECTCOST+ t1.MainBusinessTax + t1.MaterialRemain + t1.TempDeviceRemain + t1.LowValueConsumableRemain+ t1.ExchangeMaterialRemain) as  projectCostPayment,
                                (t1.SubProjectPayout+t1.PersonCost+t1.MaterialCost+t1.MechanicalCost+t1.OtherDirectCost+t1.IndirectCost+t1.ContractGrossProfit) as mainBusiness,
                                to_date(t.year||'-'||t.month||'-2','yyyy-mm-dd') createdate  from thd_financemultdatamaster t join thd_financemultdatadetail t1 on t.id = t1.parentid where  t.state=5 and (t.year<{4} OR (t.year={4} and t.month<={5})) and t.projectid='{2}')";
            //项目成本支出 主营业务 财务列报结算 
            sSQL +=
                @",temp2 as (select t.summoney money,t.createdate From thd_gatheringmaster t where  t.state=5 and t.createdate<={1} and t.projectid='{2}'
                              union all select -t.summoney money,t.createdate From thd_paymentmaster t where  t.state=5  and t.createdate<={1} and t.projectid='{2}'
                              union all select t1.money money,t.createdate createdate From thd_borrowedordermaster t,thd_borrowedorderdetail t1 where  t.id=t1.parentid and t.state=5 and t.createdate<={1} and t.projectid='{2}'
                              union all select -t.money money, t.createdate  createdate From thd_indirectcostmaster t where t.state=5 and t.createdate<={1} and t.projectid='{2}')";
                //资金存量
            sSQL +=
                ",temp3 as (select t1.submitquantity ,t1.confirmmoney , t1.AcctGatheringMoney ,t1.ConfirmDate ,t1.ConfirmEndDate   from thd_ownerquantitymaster t join thd_ownerquantitydetail t1 on t.id=t1.parentid where t.state=5 and t1.ConfirmDate<={1} and t.projectid='{2}')";
                //业主报量
            sSQL +=
                "，temp4 as (select  t.createdate ,t.SUMMONEY money  from thd_gatheringmaster t where t.state=5 and t.ifprojectmoney=0 and t.createdate<={1} and t.projectid='{2}')";
                //收款
            sSQL +=
                ",temp5 as (select  t.summoney money,t.createdate from thd_paymentmaster t where t.ifprojectmoney=0 and t.state=5  and t.createdate<={1} and t.projectid='{2}')";
                //付款
            sSQL +=
                ",temp6 as (select t.summoney money,t.createdate  from thd_paymentmaster t where t.state=5 and  (t.accounttitlename like '%保证金%' or t.accounttitlename like '%押金%')  and t.createdate<={1} and t.projectid='{2}')";
                //保证金 押金
            sSQL +=
                @"select (select t1.opgname  from resconfig t join resoperationorg t1 on t1.opgoperationtype='b'  and instr(t.ownerorgsyscode,t1.opgid)>0 where t.id='{2}') as subCompanyname,
                            t.projectname,t.Projectcost,t.Resproportion ,t1.FinanBalanceTotal,t1.FinanBalanceCurrent,
                            t1.CivilandsetupbalanceTotal,t1.CivilandsetupbalanceYear,t1.CivilandsetupbalanceMonth,
                            t1.SetupprojectbuildTotal,t1.SetupprojectbuildYear,t1.SetupprojectbuildMonth,
                            t1.setuppayoutTotal,t1.setuppayoutyear,t1.setuppayoutmonth,
                            t1.civilandsetuppayoutTotal,t1.civilandsetuppayoutyear,t1.civilandsetuppayoutmonth,
                            t1.projectCostPaymentTotal,t1.projectCostPaymentYear,t1.projectCostPaymentMonth,
                            t1.mainBusinessTotal,t1.mainBusinessYear,t1.mainBusinessMonth,
                            t2.remainMoneyTotal ,t2.remainMoneyYear,t2.remainMoneyMonth,
                            t3.confirmmoney,t3.AcctGatheringMoney,t3.submitquantity,t3.ConfirmDate,t3.ConfirmEndDate,
                            t4.gatheringtotal,t4.gatheringyear,t4.gatheringmonth,
                            t5.paymenttotal,t5.paymentyear,t5.paymentmonth,
                            t6.deposit
                            from resconfig t ,
                            ( select  sum(CivilProjectBalance)  FinanBalanceTotal,sum(case when createdate>={3} then CivilProjectBalance else 0 end )FinanBalanceCurrent,
                                sum(Civilandsetupbalance)CivilandsetupbalanceTotal,sum(case when createdate>={3} then Civilandsetupbalance else 0 end)CivilandsetupbalanceYear,sum(case when createdate>={0} then Civilandsetupbalance else 0 end)CivilandsetupbalanceMonth,
                                sum(Setupprojectbuild)SetupprojectbuildTotal,sum(case when createdate>={3} then Setupprojectbuild else 0 end)SetupprojectbuildYear,sum(case when createdate>={0} then Setupprojectbuild else 0 end)SetupprojectbuildMonth,
                                sum(setuppayout) setuppayoutTotal ,sum(case when createdate>={3} then setuppayout else 0 end)setuppayoutyear,sum(case when createdate>={0} then setuppayout else 0 end)setuppayoutmonth,
                                sum(civilandsetuppayout) civilandsetuppayoutTotal ,sum(case when createdate>={3} then civilandsetuppayout else 0 end)civilandsetuppayoutyear,sum(case when createdate>={0} then civilandsetuppayout else 0 end)civilandsetuppayoutmonth,
                                sum(projectCostPayment) projectCostPaymentTotal,sum(case when createdate>={3} then projectCostPayment else 0 end)projectCostPaymentYear,sum(case when createdate>={0} then projectCostPayment else 0 end)projectCostPaymentMonth,
                                sum(mainBusiness)mainBusinessTotal,sum(case when createdate>={3} then mainBusiness else 0 end )mainBusinessYear,sum(case when createdate>={0} then mainBusiness else 0 end)mainBusinessMonth
                            from  temp1) t1,
                            (select sum(money) remainMoneyTotal,sum(case when createdate>={3} then money else 0 end  )remainMoneyYear,sum(case when createdate>={0} then money else 0 end  )remainMoneyMonth from temp2) t2,
                            (select sum(confirmmoney)confirmmoney,sum(submitquantity)submitquantity,sum(AcctGatheringMoney) AcctGatheringMoney,max(ConfirmDate)ConfirmDate,max(ConfirmEndDate)ConfirmEndDate from temp3) t3,
                            (select  sum(money)gatheringtotal,sum(case when createdate>={3} then money else 0 end )gatheringyear,sum(case when createdate>={0} then money else 0 end )gatheringmonth from temp4) t4,
                            (select sum(money)paymenttotal,sum(case when createdate>={3} then money else 0 end )paymentyear,sum(case when createdate>={0} then money else 0 end )paymentmonth from temp5) t5,
                            (select sum(money)deposit from temp6) t6
                            where t.id='{2}'"; //4=0 5=1 6=3 7:4 8:5
            //sSQL = string.Format(sSQL, sStartTime, sEndTime, sProjectId, sCurYear, sStartTime1, sEndTime1, sCurYear1, iYear, iMonth);
            sSQL = string.Format(sSQL, sStartTime, sEndTime, sProjectId, sCurYear, iYear, iMonth);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection oConn = session.Connection;
            IDbCommand oCommand = oConn.CreateCommand();
            oCommand.CommandText = sSQL;
            IDataReader oRead = oCommand.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow oRow = ds.Tables[0].Rows[0];

                oProjectBalanceOfPayment.ProjectId = sProjectId;
                oProjectBalanceOfPayment.SubCompanyName = ClientUtil.ToString(oRow["subCompanyname"]);
                oProjectBalanceOfPayment.ProjectName = ClientUtil.ToString(oRow["projectname"]);
                oProjectBalanceOfPayment.ContractTotal = ClientUtil.ToDecimal(oRow["Projectcost"]); //合同金额
                oProjectBalanceOfPayment.FinanBalanceTotal = ClientUtil.ToDecimal(oRow["FinanBalanceTotal"]); //
                oProjectBalanceOfPayment.FinanBalanceCurrent = ClientUtil.ToDecimal(oRow["FinanBalanceCurrent"]);
                oProjectBalanceOfPayment.OwnerSure = ClientUtil.ToDecimal(oRow["confirmmoney"]);
                //业主确认金额-土建安装结算
                oProjectBalanceOfPayment.OwnerSureTJ = oProjectBalanceOfPayment.OwnerSure -
                                                       ClientUtil.ToDecimal(oRow["CivilandsetupbalanceTotal"]);
                oProjectBalanceOfPayment.OwnerSureLastTime = (oRow["ConfirmDate"] == DBNull.Value ||
                                                              oRow["ConfirmDate"] == null)
                                                                 ? DateTime.MinValue
                                                                 : ClientUtil.ToDateTime(oRow["ConfirmDate"]);
                oProjectBalanceOfPayment.OwnerSureYearMonth = (oRow["ConfirmEndDate"] == DBNull.Value ||
                                                               oRow["ConfirmEndDate"] == null)
                                                                  ? ""
                                                                  : ClientUtil.ToDateTime(oRow["ConfirmEndDate"]).
                                                                        ToString("yyyy年MM月");
                //项目成本支出合计-土建安装结算-土建装饰结算=土建累计成本支出
                oProjectBalanceOfPayment.TJCostPaymentTotal = ClientUtil.ToDecimal(oRow["projectCostPaymentTotal"]) -
                                                              ClientUtil.ToDecimal(oRow["CivilandsetupbalanceTotal"]);
                //项目成本支出合计-土建安装结算-土建装饰结算+安装工程施工+装饰工程施工=总包累计成本支出
                oProjectBalanceOfPayment.CBCostPaymentTotal = ClientUtil.ToDecimal(oRow["projectCostPaymentTotal"]) -
                                                              ClientUtil.ToDecimal(oRow["CivilandsetupbalanceTotal"]) +
                                                              ClientUtil.ToDecimal(oRow["SetupprojectbuildTotal"]);
                //oProjectBalanceOfPayment.CBSureRate = oProjectBalanceOfPayment.CBCostPaymentTotal == 0 ? "-" : Math.Round((oProjectBalanceOfPayment.OwnerSure / oProjectBalanceOfPayment.CBCostPaymentTotal) * 100, 2).ToString() + "%";
                //oProjectBalanceOfPayment.TJSureRate = oProjectBalanceOfPayment.TJCostPaymentTotal == 0 ? "-" : Math.Round((oProjectBalanceOfPayment.OwnerSureTJ / oProjectBalanceOfPayment.TJCostPaymentTotal) * 100, 2).ToString() + "%";
                oProjectBalanceOfPayment.ContractGathering = ClientUtil.ToDecimal(oRow["AcctGatheringMoney"]);
                //oProjectBalanceOfPayment.MustNotGathering = oProjectBalanceOfPayment.ContractGathering-ClientUtil.ToDecimal(oRow["gatheringtotal"]) ;//按合同应收款-收款累计
                oProjectBalanceOfPayment.MainBusinessTotal = ClientUtil.ToDecimal(oRow["mainbusinesstotal"]);
                oProjectBalanceOfPayment.MainBusinessCurrYear = ClientUtil.ToDecimal(oRow["mainbusinessyear"]);
                oProjectBalanceOfPayment.MainBusinessCurrMonth = ClientUtil.ToDecimal(oRow["mainbusinessmonth"]);
                //总  包  收工程  款
                oProjectBalanceOfPayment.CBProjectGatheringTotal = ClientUtil.ToDecimal(oRow["gatheringtotal"]);
                oProjectBalanceOfPayment.CBProjectGatheringCurrYear = ClientUtil.ToDecimal(oRow["gatheringyear"]);
                oProjectBalanceOfPayment.CBProjectGatheringCurrMonth = ClientUtil.ToDecimal(oRow["gatheringmonth"]);
                //收工程款-累计资金存量-土建安装付款+安装资金支出=总包资金支出
                oProjectBalanceOfPayment.CBProjectPaymentTotal = oProjectBalanceOfPayment.CBProjectGatheringTotal -
                                                                 ClientUtil.ToDecimal(oRow["remainMoneyTotal"]) -
                                                                 ClientUtil.ToDecimal(oRow["civilandsetuppayoutTotal"]) +
                                                                 ClientUtil.ToDecimal(oRow["setuppayoutTotal"]);
                    //收工程款-累计资金存量-土建安装付款+安装资金支出
                oProjectBalanceOfPayment.CBProjectPaymentCurrYear =
                    oProjectBalanceOfPayment.CBProjectGatheringCurrYear - ClientUtil.ToDecimal(oRow["remainMoneyYear"]) -
                    ClientUtil.ToDecimal(oRow["civilandsetuppayoutYear"]) +
                    ClientUtil.ToDecimal(oRow["setuppayoutYear"]); //收工程款-累计资金存量-土建安装付款+安装资金支出
                oProjectBalanceOfPayment.CBProjectPaymentCurrMonth =
                    oProjectBalanceOfPayment.CBProjectGatheringCurrMonth -
                    ClientUtil.ToDecimal(oRow["remainMoneyMonth"]) -
                    ClientUtil.ToDecimal(oRow["civilandsetuppayoutMonth"]) +
                    ClientUtil.ToDecimal(oRow["setuppayoutMonth"]); //收工程款-累计资金存量-土建安装付款+安装资金支出
                //总包收工程款-土建安装付款=土建收工程款
                oProjectBalanceOfPayment.TJProjectGatheringTotal = oProjectBalanceOfPayment.CBProjectGatheringTotal -
                                                                   ClientUtil.ToDecimal(oRow["civilandsetuppayoutTotal"]);
                oProjectBalanceOfPayment.TJProjectGatheringCurrYear =
                    oProjectBalanceOfPayment.CBProjectGatheringCurrYear -
                    ClientUtil.ToDecimal(oRow["civilandsetuppayoutYear"]);
                oProjectBalanceOfPayment.TJProjectGatheringCurrMonth =
                    oProjectBalanceOfPayment.CBProjectGatheringCurrMonth -
                    ClientUtil.ToDecimal(oRow["civilandsetuppayoutMonth"]);
                //收工程款-累计资金存量-土建安装付款=土建资金支出
                oProjectBalanceOfPayment.TJProjectPaymentTotal = oProjectBalanceOfPayment.CBProjectGatheringTotal -
                                                                 ClientUtil.ToDecimal(oRow["remainMoneyTotal"]) -
                                                                 ClientUtil.ToDecimal(oRow["civilandsetuppayoutTotal"]);
                oProjectBalanceOfPayment.TJProjectPaymentCurrYear =
                    oProjectBalanceOfPayment.CBProjectGatheringCurrYear - ClientUtil.ToDecimal(oRow["remainMoneyyear"]) -
                    ClientUtil.ToDecimal(oRow["civilandsetuppayoutYear"]);
                oProjectBalanceOfPayment.TJProjectPaymentCurrMonth =
                    oProjectBalanceOfPayment.CBProjectGatheringCurrMonth -
                    ClientUtil.ToDecimal(oRow["remainMoneymonth"]) -
                    ClientUtil.ToDecimal(oRow["civilandsetuppayoutMonth"]);

                //oProjectBalanceOfPayment.CBMoneyRemainTotal = ClientUtil.ToDecimal(oRow["remainMoneyTotal"]) + ClientUtil.ToDecimal(oRow["civilandsetuppayoutTotal"]) - ClientUtil.ToDecimal(oRow["setuppayoutTotal"]);//资金存量+土建安装付款-安装资金支出
                //oProjectBalanceOfPayment.CBMoneyRemainCurrYear = ClientUtil.ToDecimal(oRow["remainMoneyYear"]) + ClientUtil.ToDecimal(oRow["civilandsetuppayoutYear"]) - ClientUtil.ToDecimal(oRow["setuppayoutYear"]);
                //oProjectBalanceOfPayment.CBMoneyRemainCurrMonth = ClientUtil.ToDecimal(oRow["remainMoneyMonth"]) + ClientUtil.ToDecimal(oRow["civilandsetuppayoutMonth"]) - ClientUtil.ToDecimal(oRow["setuppayoutMonth"]);
                //oProjectBalanceOfPayment.TJMoneyRemainTotal = oProjectBalanceOfPayment.TJProjectGatheringTotal - oProjectBalanceOfPayment.TJProjectPaymentTotal;//土建工程收款-土建工程支出
                //oProjectBalanceOfPayment.TJMoneyRemainTotal = oProjectBalanceOfPayment.TJProjectGatheringTotal - oProjectBalanceOfPayment.TJProjectPaymentTotal;
                oProjectBalanceOfPayment.OtherMustPayment = ClientUtil.ToDecimal(oRow["deposit"]);
                oProjectBalanceOfPayment.CBHandUpRate = ClientUtil.ToDecimal(oRow["Resproportion"]);

            }

            #endregion

            return oProjectBalanceOfPayment;
        }

        #endregion

        #region 资金计划
        public DataDomain GetProjectPlanRate(string projId)
        {
            if (string.IsNullOrEmpty(projId))
            {
                return null;
            }

            var ds = QueryDataToDataSet(string.Format(SQLScript.GetProjectPlanRateSql, projId));
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var data = new DataDomain();
            data.Name1 = ds.Tables[0].Rows[0][0];
            data.Name2 = ds.Tables[0].Rows[0][1];
            data.Name3 = ds.Tables[0].Rows[0][2];

            return data;
        }

        public IList GetProjectFundPlanMasterByOQ(ObjectQuery oq)
        {
            oq.AddFetchMode("OtherPayDetails", FetchMode.Eager);
            oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof (ProjectFundPlanMaster), oq);
        }

        //分公司资金计划申报表
        public IList GetFilialeFundPlanMasterByOQ(ObjectQuery oq)
        {
            oq.AddFetchMode("OperOrgInfo", FetchMode.Eager);
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("OfficeFundPlanDetails", FetchMode.Eager);

            return Dao.ObjectQuery(typeof (FilialeFundPlanMaster), oq);
        }

        public ProjectFundPlanMaster GetProjectFundPlanById(string id)
        {
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("Id", id));

            var list = GetProjectFundPlanMasterByOQ(objQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0] as ProjectFundPlanMaster;
        }

        public FilialeFundPlanMaster GetFilialeFundPlanById(string id)
        {
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("Id", id));

            var list = GetFilialeFundPlanMasterByOQ(objQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0] as FilialeFundPlanMaster;
        }

        private DataDomain GetSupplyOrderMasterByStockInBalMaster(StockInBalMaster master)
        {
            if (master == null)
            {
                return null;
            }

            var ds = QueryDataToDataSet(string.Format(SQLScript.GetSupplierOrderSql, master.Id));
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var dt = new DataDomain();
                dt.Name1 = ds.Tables[0].Rows[i]["supplierrelation"].ToString();
                dt.Name2 = ds.Tables[0].Rows[i]["supplierrelationname"].ToString();
                dt.Name3 = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["contractmoney"]);
                dt.Name4 = ds.Tables[0].Rows[i]["balancestyle"].ToString();
                dt.Name5 = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["processpayrate"]);
                dt.Name6 = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["completepayrate"]);
                dt.Name7 = TransUtil.ToDecimal(ds.Tables[0].Rows[i]["warrantypayrate"]);

                return dt;
            }

            return null;
        }

        private MaterialRentalContractMaster GetMaterialRentalContractBySupplier(SupplierRelationInfo supplier)
        {
            if (supplier == null)
            {
                return null;
            }

            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objQuery.AddCriterion(Expression.Eq("TheSupplierRelationInfo", supplier));

            var list = Query(typeof (MaterialRentalContractMaster), objQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }

            return list[0] as MaterialRentalContractMaster;
        }

        private IList CreateProjectFundDetailBySubContractBalanceBill(string projectId, int year, int month)
        {
            //分包结算单
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", projectId));
            objQuery.AddCriterion(Expression.Le("CreateDate", new DateTime(year, month, 1)));
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objQuery.AddFetchMode("Details", FetchMode.Eager);
            objQuery.AddFetchMode("Details.Details", FetchMode.Eager);
            objQuery.AddFetchMode("TheSubContractProject", FetchMode.Eager);

            var subBills = Query(typeof (SubContractBalanceBill), objQuery);
            if (subBills.Count == 0)
            {
                return null;
            }

            var billHash = new Dictionary<SubContractProject, List<SubContractBalanceSubjectDtl>>();
            var planDetails = new List<ProjectFundPlanDetail>();
            var subAccounts = new Dictionary<SubContractType, string>();
            subAccounts.Add(SubContractType.劳务分包, "人工费");
            subAccounts.Add(SubContractType.专业分包, "分包工程");

            foreach (SubContractBalanceBill subBill in subBills)
            {
                foreach (SubContractBalanceDetail detail in subBill.Details)
                {
                    if (billHash.ContainsKey(subBill.TheSubContractProject))
                    {
                        var dtList = billHash[subBill.TheSubContractProject];
                        dtList.AddRange(detail.Details);
                        billHash[subBill.TheSubContractProject] = dtList;
                    }
                    else
                    {
                        billHash.Add(subBill.TheSubContractProject,
                                     detail.Details.OfType<SubContractBalanceSubjectDtl>().ToList());
                    }
                }
            }

            var lastYear = year;
            var lastMonth = month;
            if(month==1)
            {
                lastYear -= 1;
                lastMonth = 11;
            }
            else
            {
                lastMonth -= 2;
            }

            foreach (SubContractProject subContract in billHash.Keys)
            {
                var subDetails = billHash[subContract];
                var planDetail = new ProjectFundPlanDetail();
                planDetail.CreditorUnitLeadingOfficial = subContract.BearerOrgName;
                planDetail.ContractAmount = subContract.ContractSumMoney;
                planDetail.JobContent = subContract.SubPackage;
                planDetail.ContractPaymentRatio = subContract.BalanceStyle == "过程结算"
                                                      ? subContract.ProcessPayRate
                                                      : subContract.BalanceStyle == "末次结算"
                                                            ? subContract.CompletePayRate
                                                            : subContract.WarrantyPayRate;

                planDetail.CumulativeSettlement = subContract.AddupBalanceMoney;
                if (subAccounts.ContainsKey(subContract.ContractType))
                {
                    planDetail.FundPaymentCategory = subAccounts[subContract.ContractType];
                }
                else
                {
                    planDetail.FundPaymentCategory = "其他";
                }
                planDetail.PrecedingMonthSettlement =
                    subDetails.FindAll(a => a.TheBalanceDetail.Master.CreateYear == lastYear && a.TheBalanceDetail.Master.CreateMonth == lastMonth)
                    .Sum(a => a.BalanceTotalPrice);
                planDetail.TempData = Guid.NewGuid().ToString();

                planDetails.Add(planDetail);
            }

            return planDetails;
        }

        private IList CreateProjectFundDetailByMaterials(string projectId, int year, int month)
        {
            //物资验收结算单
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", projectId));
            objQuery.AddCriterion(Expression.Le("CreateDate", new DateTime(year, month, 1)));
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objQuery.AddFetchMode("Details", FetchMode.Eager);
            objQuery.AddFetchMode("Details.ForwardDetails", FetchMode.Eager);

            var stockBills = Query(typeof (StockInBalMaster), objQuery);
            if (stockBills.Count == 0)
            {
                return null;
            }

            var lastYear = year;
            var lastMonth = month;
            if (month == 1)
            {
                lastYear -= 1;
                lastMonth = 11;
            }
            else
            {
                lastMonth -= 2;
            }

            var planDetails = new List<ProjectFundPlanDetail>();
            var contractList = new List<DataDomain>();
            foreach (StockInBalMaster master in stockBills)
            {
                var dt = GetSupplyOrderMasterByStockInBalMaster(master);
                if (dt != null)
                {
                    contractList.Add(dt);
                }
            }

            var bills =
                stockBills.OfType<StockInBalMaster>().GroupBy(a => new { a.TheSupplierRelationInfo, a.TheSupplierName });
            var lastMonthBills = stockBills.OfType<StockInBalMaster>().ToList()
                .FindAll(a => a.CreateYear == lastYear && a.CreateMonth == lastMonth);
            foreach (var gp in bills)
            {
                var planDetail = new ProjectFundPlanDetail();
                planDetail.CreditorUnitLeadingOfficial = gp.Key.TheSupplierName;
                planDetail.JobContent = string.Empty;
                var contract = contractList.Find(c => c.Name2.ToString() == gp.Key.TheSupplierName);
                if (contract != null)
                {
                    planDetail.ContractAmount = TransUtil.ToDecimal(contract.Name3);
                    planDetail.ContractPaymentRatio = TransUtil.ToString(contract.Name4) == "过程结算"
                                                      ? TransUtil.ToDecimal(contract.Name5)
                                                      : TransUtil.ToString(contract.Name4) == "末次结算"
                                                            ? TransUtil.ToDecimal(contract.Name6)
                                                            : TransUtil.ToDecimal(contract.Name7);
                }
                planDetail.CumulativeSettlement = gp.Sum(a => a.SumMoney);
                planDetail.FundPaymentCategory = "材料费";
                planDetail.PrecedingMonthSettlement =
                    lastMonthBills.FindAll(a => a.TheSupplierRelationInfo == gp.Key.TheSupplierRelationInfo)
                        .Sum(a => a.SumMoney);
                planDetail.TempData = Guid.NewGuid().ToString();

                planDetails.Add(planDetail);
            }

            return planDetails;
        }

        private IList CreateProjectFundDetailByMaterialRentalBill(string projectId, int year, int month)
        {
            //机械租赁结算单
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", projectId));
            objQuery.AddCriterion(Expression.Le("CreateDate", new DateTime(year, month, 1)));
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));

            var rentalBills = Query(typeof (MaterialRentalSettlementMaster), objQuery);
            if (rentalBills.Count == 0)
            {
                return null;
            }

            var lastYear = year;
            var lastMonth = month;
            if (month == 1)
            {
                lastYear -= 1;
                lastMonth = 11;
            }
            else
            {
                lastMonth -= 2;
            }
            var planDetails = new List<ProjectFundPlanDetail>();
            var bills =
                rentalBills.OfType<MaterialRentalSettlementMaster>().GroupBy(a => new { a.TheSupplierRelationInfo, a.SupplierName });
            var lastMonthBills = rentalBills.OfType<MaterialRentalSettlementMaster>().ToList()
                .FindAll(a => a.CreateYear == lastYear && a.CreateMonth == lastMonth);
            foreach (var gp in bills)
            {
                var planDetail = new ProjectFundPlanDetail();
                planDetail.CreditorUnitLeadingOfficial = gp.Key.SupplierName;
                planDetail.JobContent = string.Empty;
                var contract = GetMaterialRentalContractBySupplier(gp.Key.TheSupplierRelationInfo);
                if (contract != null)
                {
                    planDetail.ContractAmount = contract.SumMoney;
                    planDetail.ContractPaymentRatio = contract.BalanceStyle == "过程结算"
                                                      ? contract.ProcessPayRate
                                                      : contract.BalanceStyle == "末次结算"
                                                            ? contract.CompletePayRate
                                                            : contract.WarrantyPayRate;
                }

                planDetail.CumulativeSettlement = gp.Sum(a => a.SumMoney);
                planDetail.FundPaymentCategory = "机械费";
                planDetail.PrecedingMonthSettlement =
                    lastMonthBills.FindAll(a => a.SupplierName == gp.Key.SupplierName).Sum(a => a.SumMoney);
                planDetail.TempData = Guid.NewGuid().ToString();

                planDetails.Add(planDetail);
            }

            return planDetails;
        }

        private IList GetPayments(string projectId, int year, int month)
        {
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", projectId));
            objQuery.AddCriterion(Expression.Le("CreateDate", new DateTime(year, month, 1)));//截止到当月
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objQuery.AddFetchMode("Details", FetchMode.Eager);

            var list = Query(typeof (PaymentMaster), objQuery).OfType<PaymentMaster>();

            var supList = QueryCusAndSupInfoByCondition(string.Empty, string.Empty);
            var catDic = new Dictionary<string, string>();
            catDic.Add("劳务分包供应商", "人工费");
            catDic.Add("料具供应商", "材料费");
            catDic.Add("设备出租商", "机械费");
            catDic.Add("设备供应商", "机械费");
            catDic.Add("物资供应商", "材料费");
            catDic.Add("专业分包供应商", "分包工程");

            var retList = new List<PaymentMaster>();
            foreach (var pay in list)
            {
                var sup =
                    supList.OfType<DataDomain>().FirstOrDefault(
                        a => TransUtil.ToString(a.Name2) == pay.TheSupplierRelationInfo
                            || TransUtil.ToString(a.Name2) == pay.TheCustomerRelationInfo);
                if (sup == null)
                {
                    continue;
                }
                var cat = TransUtil.ToString(sup.Name8);
                if(!catDic.ContainsKey(cat))
                {
                    continue;
                }

                pay.AccountTitleName = catDic[cat];

                retList.Add(pay);
            }

            return retList;
        }

        public ProjectFundPlanMaster CreateProjectFundDetail(ProjectFundPlanMaster fundPlan)
        {
            if (fundPlan == null || string.IsNullOrEmpty(fundPlan.ProjectId))
            {
                return null;
            }
            var year = fundPlan.CreateYear;
            var month = fundPlan.CreateMonth;
            if (month == 12)
            {
                year += 1;
                month = 1;
            }
            else
            {
                month += 1;
            }

            var planDetails = new List<ProjectFundPlanDetail>();

            //人工、分包
            var dts = CreateProjectFundDetailBySubContractBalanceBill(fundPlan.ProjectId, year, month);
            if (dts != null)
            {
                planDetails.AddRange(dts.OfType<ProjectFundPlanDetail>());
            }

            //材料
            dts = CreateProjectFundDetailByMaterials(fundPlan.ProjectId, year, month);
            if (dts != null)
            {
                planDetails.AddRange(dts.OfType<ProjectFundPlanDetail>());
            }

            //机械
            dts = CreateProjectFundDetailByMaterialRentalBill(fundPlan.ProjectId, year, month);
            if (dts != null)
            {
                planDetails.AddRange(dts.OfType<ProjectFundPlanDetail>());
            }

            //累计支付
            var payList = GetPayments(fundPlan.ProjectId, year, month);
            var payTotals = from pay in payList.OfType<PaymentMaster>()
                            group pay by new
                                             {
                                                 pay.AccountTitleName,
                                                 pay.TheSupplierName,
                                                 pay.TheCustomerName
                                             }
                            into gps
                            select new
                                       {
                                           SubjectName = gps.Key.AccountTitleName,
                                           SupplierName = !string.IsNullOrEmpty(gps.Key.TheSupplierName)
                                                              ? gps.Key.TheSupplierName
                                                              : gps.Key.TheCustomerName,
                                           TotalMoney = gps.Sum(pay => pay.SumMoney)
                                       };

            fundPlan.Details.Clear();

            foreach (var detail in planDetails)
            {
                var payItem =
                    payTotals.FirstOrDefault(
                        p =>
                        p.SubjectName == detail.FundPaymentCategory &&
                        p.SupplierName == detail.CreditorUnitLeadingOfficial);
                if (payItem != null)
                {
                    detail.CumulativePayment = payItem.TotalMoney;
                }

                detail.Master = fundPlan;
                detail.CumulativeArrears = detail.CumulativeSettlement - detail.CumulativePayment;
                detail.CumulativeExpireDue = detail.CumulativeSettlement*detail.ContractPaymentRatio -
                                             detail.CumulativePayment;
                if (detail.CumulativeSettlement != 0)
                {
                    detail.PaymentRatio = Math.Round(detail.CumulativePayment/detail.CumulativeSettlement, 4);
                    detail.PlanPaymentRatio =
                        Math.Round((detail.CumulativePayment + detail.PlanPayment)/detail.CumulativeSettlement, 4);
                }

                fundPlan.Details.Add(detail);
            }

            fundPlan.CumulativePayment = fundPlan.Details.OfType<ProjectFundPlanDetail>().Sum(a => a.CumulativePayment);

            return fundPlan;
        }

        public IList GetProjectOtherPayPlanDetailByOQ(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof (ProjectOtherPayPlanDetail), oq);
        }

        [TransManager]
        public ProjectFundPlanMaster SaveProjectFundPlan(ProjectFundPlanMaster fundPlan)
        {
            if (string.IsNullOrEmpty(fundPlan.Id))
            {
                fundPlan.Code = GetCode(fundPlan.GetType());
            }

            //与资金策划对比
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", fundPlan.ProjectId));
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objQuery.AddCriterion(Expression.Le("SchemeBeginDate", fundPlan.DeclareDate));
            objQuery.AddCriterion(Expression.Ge("SchemeEndDate", fundPlan.DeclareDate));
            objQuery.AddFetchMode("PaymentCalculationDtl", FetchMode.Eager);

            var schemeList = Query(typeof (FundPlanningMaster), objQuery);
            if (schemeList != null && schemeList.Count > 0)
            {
                var scheme = schemeList[0] as FundPlanningMaster;
                var payScheme = scheme.PaymentCalculationDtl.OfType<FundSchemePayment>().ToList()
                    .Find(a => a.Year == fundPlan.CreateYear && a.Month == fundPlan.CreateMonth);
                if (payScheme != null && payScheme.CurrentPayTotal < fundPlan.PresentMonthPayment)
                {
                    throw new Exception("该资金计划合计支付金额不得大于策划金额" + payScheme.CurrentPayTotal);
                }
            }

            Dao.SaveOrUpdate(fundPlan);

            if (fundPlan.DocState == DocumentState.InAudit)
            {
                var appBill = new ApproveBill();
                appBill.BillCode = fundPlan.Code;
                appBill.BillCreateDate = fundPlan.CreateDate;
                appBill.BillCreatePerson = fundPlan.CreatePerson;
                appBill.BillCreatePersonName = fundPlan.CreatePersonName;
                appBill.BillId = fundPlan.Id;
                appBill.BillSysCode = fundPlan.OpgSysCode;
                appBill.ProjectId = fundPlan.ProjectId;
                appBill.ProjectName = fundPlan.ProjectName;

                RefAppSrv.SaveApproveBill(appBill, typeof (ProjectFundPlanMaster));
            }

            return fundPlan;
        }

        public List<ProjectFundPlanMaster> SaveProjectFundPlans(List<ProjectFundPlanMaster> fundPlans)
        {
            foreach (ProjectFundPlanMaster fundPlan in fundPlans)
            {
                if (string.IsNullOrEmpty(fundPlan.Id))
                {
                    fundPlan.Code = GetCode(fundPlan.GetType());
                }

                Dao.SaveOrUpdate(fundPlan);

                if (fundPlan.DocState == DocumentState.InAudit)
                {
                    var appBill = new ApproveBill();
                    appBill.BillCode = fundPlan.Code;
                    appBill.BillCreateDate = fundPlan.CreateDate;
                    appBill.BillCreatePerson = fundPlan.CreatePerson;
                    appBill.BillCreatePersonName = fundPlan.CreatePersonName;
                    appBill.BillId = fundPlan.Id;
                    appBill.BillSysCode = fundPlan.OpgSysCode;
                    appBill.ProjectId = fundPlan.ProjectId;
                    appBill.ProjectName = fundPlan.ProjectName;

                    RefAppSrv.SaveApproveBill(appBill, typeof (ProjectFundPlanMaster));
                }
            }
            return fundPlans;
        }

        [TransManager]
        public FilialeFundPlanMaster SaveFilialeFundPlan(FilialeFundPlanMaster fundPlan)
        {
            if (string.IsNullOrEmpty(fundPlan.Id))
            {
                fundPlan.Code = GetCode(fundPlan.GetType());
            }

            Dao.SaveOrUpdate(fundPlan);

            if (fundPlan.DocState == DocumentState.InAudit)
            {
                var appBill = new ApproveBill();
                appBill.BillCode = fundPlan.Code;
                appBill.BillCreateDate = fundPlan.CreateDate;
                appBill.BillCreatePerson = fundPlan.CreatePerson;
                appBill.BillCreatePersonName = fundPlan.CreatePersonName;
                appBill.BillId = fundPlan.Id;
                appBill.BillSysCode = fundPlan.OpgSysCode;
                appBill.ProjectId = fundPlan.ProjectId;
                appBill.ProjectName = fundPlan.ProjectName;

                RefAppSrv.SaveApproveBill(appBill, typeof (FilialeFundPlanMaster));
            }
            return fundPlan;
        }

        public ProjectOtherPayPlanDetail SaveOtherPayPlanDetail(ProjectOtherPayPlanDetail otherPlanDetail)
        {
            Dao.SaveOrUpdate(otherPlanDetail);
            return otherPlanDetail;
        }

        public List<ProjectOtherPayPlanDetail> SaveOtherPayPlanDetails(List<ProjectOtherPayPlanDetail> otherPlanDetails)
        {
            Dao.SaveOrUpdate(otherPlanDetails);
            return otherPlanDetails;
        }

        public List<ProjectFundPlanDetail> SaveProjectPlanDetails(List<ProjectFundPlanDetail> projectPlanDetails)
        {
            Dao.SaveOrUpdate(projectPlanDetails);
            return projectPlanDetails;
        }

        public ProjectFundPlanDetail SaveProjectPlanDetail(ProjectFundPlanDetail projectPlanDetail)
        {
            Dao.SaveOrUpdate(projectPlanDetail);
            return projectPlanDetail;
        }

        public OfficeFundPlanPayDetail SaveOfficePlanDetail(OfficeFundPlanPayDetail officePlanDetail)
        {
            Dao.SaveOrUpdate(officePlanDetail);
            return officePlanDetail;
        }

        public List<OfficeFundPlanPayDetail> SaveOfficePlanDetails(List<OfficeFundPlanPayDetail> officePlanDetails)
        {
            Dao.SaveOrUpdate(officePlanDetails);
            return officePlanDetails;
        }

        public FilialeFundPlanDetail SaveFilialePlanDetail(FilialeFundPlanDetail filialePlanDetail)
        {
            Dao.SaveOrUpdate(filialePlanDetail);
            return filialePlanDetail;
        }

        public List<FilialeFundPlanDetail> SaveFilialePlanDetails(List<FilialeFundPlanDetail> filialePlanDetails)
        {
            Dao.SaveOrUpdate(filialePlanDetails);
            return filialePlanDetails;
        }

        public ProjectFundPlanMaster GetProjectFundFlow(ProjectFundPlanMaster fundPlan)
        {
            if (fundPlan == null || string.IsNullOrEmpty(fundPlan.ProjectId))
            {
                return null;
            }

            var year = fundPlan.CreateYear;
            var month = fundPlan.CreateMonth;
            if (month == 12)
            {
                year += 1;
                month = 1;
            }
            else
            {
                month += 1;
            }

            var ds =
                QueryDataToDataSet(string.Format(SQLScript.GetProjectFundFlowSql, fundPlan.ProjectId, year, month));
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            fundPlan.OwnerActualAffirmMeterage = Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
            fundPlan.CumulativeGathering = Convert.ToDecimal(ds.Tables[0].Rows[0][1]);
            fundPlan.CumulativePayment = Convert.ToDecimal(ds.Tables[0].Rows[0][2]);

            return fundPlan;
        }

        public IList GetFilialeFundPlanProjectDetail(FilialeFundPlanMaster filialeFund)
        {
            if (filialeFund == null)
            {
                return null;
            }

            ObjectQuery objQuery = new ObjectQuery();

            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objQuery.AddCriterion(Expression.Eq("CreateYear", filialeFund.CreateYear));
            objQuery.AddCriterion(Expression.Eq("CreateMonth", filialeFund.CreateMonth + 1));

            var list = GetProjectFundPlanMasterByOQ(objQuery).OfType<ProjectFundPlanMaster>().ToList();

            return list.FindAll(a => filialeFund.OperOrgInfo.SysCode.StartsWith(a.AttachBusinessOrg.SysCode));
        }

        public FilialeFundPlanMaster GetFilialeFundPlanFlow(FilialeFundPlanMaster filialeFund)
        {
            var year = filialeFund.CreateYear;
            var month = filialeFund.CreateMonth;
            if (month == 12)
            {
                year += 1;
                month = 1;
            }
            else
            {
                month += 1;
            }

            var ds =
                QueryDataToDataSet(string.Format(SQLScript.GetFilialeFundFlowSql, filialeFund.OpgSysCode, year, month));
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            filialeFund.CumulativeCurrentYearGathering = Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
            filialeFund.CumulativeCurrentYearPayment = Convert.ToDecimal(ds.Tables[0].Rows[0][1]);

            return filialeFund;
        }

        public IList GetPayAccounts()
        {
            var ds = QueryDataToDataSet(SQLScript.GetPayAccountSql);
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var idList = new Disjunction();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                idList.Add(Expression.Eq("Id", ds.Tables[0].Rows[i]["Id"]));
            }

            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(idList);

            var list = Dao.ObjectQuery(typeof (AccountTitleTree), objQuery);
            return list.OfType<AccountTitleTree>().OrderBy(a => a.Code).ToList();
        }

        public List<decimal> GetPayValues(string projId, string supId, string planCode, DateTime billDate)
        {
            var retList = new List<decimal>();
            retList.Add(GetBusinessMoneyByProject(2, projId, supId));

            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", projId));
            objQuery.AddCriterion(Expression.Eq("TheSupplierRelationInfo", supId));
            objQuery.AddCriterion(Expression.Eq("FundPlanCode", planCode));
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objQuery.AddCriterion(Expression.Lt("RealOperationDate", billDate));

            var list = Dao.ObjectQuery(typeof (PaymentMaster), objQuery);
            if (list != null)
            {
                retList.Add(list.OfType<PaymentMaster>().Sum(a => a.SumMoney));
            }
            else
            {
                retList.Add(0);
            }

            return retList;
        }

        public FundAssessmentMaster SaveFundAssessment(FundAssessmentMaster master)
        {
            if (string.IsNullOrEmpty(master.Id))
            {
                //判断是资金利息计算还是考核兑现
                master.Code = GetCode(master.GetType()).Replace("资金考核", master.Temp1);
            }

            Dao.SaveOrUpdate(master);

            return master;
        }

        public FundAssessmentMaster GetFundAssessmentById(string id)
        {
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("Id", id));
            objQuery.AddFetchMode("Details", FetchMode.Eager);
            objQuery.AddFetchMode("AssessCashDetails", FetchMode.Eager);

            var list = Dao.ObjectQuery(typeof (FundAssessmentMaster), objQuery);
            return list.OfType<FundAssessmentMaster>().FirstOrDefault();
        }

        public FundAssessmentMaster GetFundAssessmentData(FundAssessmentMaster initMaster ,int fundAssessmentType)
        {
            if (initMaster == null || string.IsNullOrEmpty(initMaster.ProjectId))
            {
                return initMaster;
            }

            //累计实际支付
            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", initMaster.ProjectId));
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));

            var payList = Dao.ObjectQuery(typeof(PaymentMaster), objQuery).OfType<PaymentMaster>();
            initMaster.CurrentRealPay = payList.Sum(p => p.SumMoney);

            //累计实际收款
            var getList = Dao.ObjectQuery(typeof(GatheringMaster), objQuery).OfType<GatheringMaster>();
            initMaster.CurrentRealGet = getList.Sum(g => g.SumMoney);

            initMaster.CurrentCashBalance = initMaster.CurrentRealGet - initMaster.CurrentRealPay;

            #region 资金考核兑现

            var aDetail = new FundAssessCashDetail();
            aDetail.Master = initMaster;
            initMaster.AssessCashDetails.Clear();
            initMaster.AssessCashDetails.Add(aDetail);

            objQuery.AddCriterion(Expression.Eq("CreateYear", initMaster.CreateYear));
            objQuery.AddCriterion(Expression.Eq("CreateMonth", initMaster.CreateMonth));
            objQuery.AddFetchMode("Details", FetchMode.Eager);
            var planList = Dao.ObjectQuery(typeof(ProjectFundPlanMaster), objQuery).OfType<ProjectFundPlanMaster>();
            var plan = planList.FirstOrDefault();
            if (plan != null)
            {
                var details = plan.Details.OfType<ProjectFundPlanDetail>().ToList();
                aDetail.CentralPurchase = details.FindAll(a => a.FundPaymentCategory == "材料费").Sum(b => b.CumulativeExpireDue);
                aDetail.InnerInstall = 0;
                aDetail.OtherContractPay = details.Sum(b => b.CumulativeExpireDue) - aDetail.CentralPurchase - aDetail.InnerInstall;
            }
            aDetail.OtherAdjust = 0;
            aDetail.RealCashBalance = initMaster.CurrentCashBalance
                                      - (aDetail.CentralPurchase + aDetail.InnerInstall
                                         + aDetail.OtherContractPay +
                                         aDetail.OtherAdjust);
            aDetail.AssessCardinal = initMaster.CurrentCashBalance - initMaster.CurrentSchemeTarget
                                     - aDetail.CentralPurchase - aDetail.InnerInstall
                                     - aDetail.OtherContractPay * 0.5m - aDetail.OtherAdjust;
            if (aDetail.AssessCardinal <= 0)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.0001m;
            }
            else if (aDetail.AssessCardinal < 500)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.0008m;
            }
            else if (aDetail.AssessCardinal < 1000)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.001m;
            }
            else if (aDetail.AssessCardinal < 2000)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.0012m;
            }
            else if (aDetail.AssessCardinal < 5000)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.00125m;
            }
            else if (aDetail.AssessCardinal < 10000)
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.0013m;
            }
            else
            {
                aDetail.CashMoney = aDetail.AssessCardinal * 0.00135m;
            }

            var payAsList = QueryProjectBalanceOfPayment(objQuery);
            ProjectBalanceOfPayment payAsc = null;
            if (payAsList != null)
            {
                payAsc = payAsList.OfType<ProjectBalanceOfPayment>().FirstOrDefault();
                if (payAsc != null)
                {
                    var warnList = new List<string>() { payAsc.WarnMoneyFlow, payAsc.WarnMoneyRemain, payAsc.WarnMustNotGathering };
                    if (warnList.Contains("红色预警"))
                    {
                        aDetail.WarnLevel = "红色预警";
                        aDetail.WarnRate = 1m;
                    }
                    else if (warnList.Contains("橙色预警"))
                    {
                        aDetail.WarnLevel = "橙色预警";
                        aDetail.WarnRate = 0.6m;
                    }
                    else if (warnList.Contains("黄色预警"))
                    {
                        aDetail.WarnLevel = "黄色预警";
                        aDetail.WarnRate = 0.3m;
                    }

                    aDetail.ApprovalRate = payAsc.CBSureRate == "-" ? 0 : decimal.Parse(payAsc.CBSureRate) / 100m;
                }
            }

            if (aDetail.ApprovalRate < 0.9m)
            {
                aDetail.ApprovalDeduction = 1m;
            }
            else if (aDetail.ApprovalRate < 1m)
            {
                aDetail.ApprovalDeduction = 0.6m;
            }
            else if (aDetail.ApprovalRate < 1.05m)
            {
                aDetail.ApprovalDeduction = 0.3m;
            }

            aDetail.DeductionItem = aDetail.AssessCardinal <= 0
                                        ? 0
                                        : aDetail.CashMoney * aDetail.WarnRate > aDetail.ApprovalDeduction
                                              ? aDetail.WarnRate
                                              : aDetail.ApprovalDeduction;
            aDetail.AssessCashMoney = aDetail.AssessCardinal - aDetail.DeductionItem;

            #endregion

            #region 取资金策划目标
            objQuery.Criterions.Clear();
            objQuery.AddCriterion(Expression.Eq("ProjectId", initMaster.ProjectId));
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            var dt = new DateTime(initMaster.CreateYear, initMaster.CreateMonth, 1);
            objQuery.AddCriterion(Expression.Le("SchemeBeginDate", dt));
            objQuery.AddCriterion(Expression.Ge("SchemeEndDate", dt));
            objQuery.AddFetchMode("CostCalculationDtl", FetchMode.Eager);
            objQuery.AddFetchMode("FundSummaryDtl", FetchMode.Eager);

            var schemeList = Dao.ObjectQuery(typeof(FundPlanningMaster), objQuery).OfType<FundPlanningMaster>();
            var scheme = schemeList.FirstOrDefault();
            if (scheme != null)
            {
                FundSchemeReportAmount matchAmount = null;
                if (payAsc == null)
                {
                    matchAmount = scheme.CostCalculationDtl.OfType<FundSchemeReportAmount>()
                        .FirstOrDefault(a => a.Year == initMaster.CreateYear && a.Month == initMaster.CreateMonth);
                }
                else
                {
                    var q = from reportAmount in scheme.CostCalculationDtl.OfType<FundSchemeReportAmount>()
                            select new
                            {
                                reportAmount.Id,
                                reportAmount.Year,
                                reportAmount.Month,
                                Diff = Math.Abs(payAsc.MainBusinessCurrMonth - reportAmount.CurrentProfit)
                            };
                    var minDiff = q.Min(a => a.Diff);
                    matchAmount = scheme.CostCalculationDtl.OfType<FundSchemeReportAmount>()
                        .FirstOrDefault(a => a.Id == q.FirstOrDefault(b => b.Diff == minDiff).Id);

                }
                if (matchAmount != null)
                {
                    var target = scheme.FundSummaryDtl.OfType<FundSchemeSummary>()
                        .FirstOrDefault(a => a.Year == matchAmount.Year && a.Month == matchAmount.Month);
                    initMaster.SchemeTarget = target.Id;
                    initMaster.CurrentSchemeTarget = target.CurrentBalance;
                }
            }

            #endregion

            #region 资金计息

            var iDetail = new FundInterestDetail();
            iDetail.Master = initMaster;
            initMaster.Details.Clear();
            initMaster.Details.Add(iDetail);
            iDetail.CashBalanceInScheme = initMaster.CurrentSchemeTarget <= 0
                                              ? 0
                                              : initMaster.CurrentCashBalance <= 0
                                                    ? 0
                                                    : Math.Min(initMaster.CurrentSchemeTarget,
                                                               initMaster.CurrentCashBalance);
            iDetail.CashBorrowInScheme = (initMaster.CurrentSchemeTarget >= 0
                                              ? 0
                                              : initMaster.CurrentCashBalance >= 0
                                                    ? 0
                                                    : Math.Max(initMaster.CurrentSchemeTarget,
                                                               initMaster.CurrentCashBalance)) * -1;
            iDetail.CashBalanceOutScheme = initMaster.CurrentSchemeTarget < 0
                                               ? initMaster.CurrentCashBalance > 0 ? initMaster.CurrentCashBalance : 0
                                               : Math.Max(
                                                   initMaster.CurrentCashBalance - initMaster.CurrentSchemeTarget, 0);
            iDetail.CashBorrowOutScheme = (initMaster.CurrentSchemeTarget > 0
                                               ? initMaster.CurrentCashBalance < 0 ? initMaster.CurrentCashBalance : 0
                                               : Math.Min(
                                                   initMaster.CurrentCashBalance - initMaster.CurrentSchemeTarget, 0)) *
                                          -1;

            if (initMaster.ProjectState.Contains("在建"))
            {
                iDetail.InterestCost = iDetail.CashBorrowInScheme * 0.005m + iDetail.CashBorrowOutScheme * 0.0125m -
                                       iDetail.CashBalanceInScheme * 0.0025m - iDetail.CashBalanceOutScheme * 0.005m;
            }
            else if (initMaster.ProjectState.Contains("收尾") || initMaster.ProjectState.Contains("完工6月内未办理结算"))
            {
                var tmp = initMaster.CurrentCashBalance - initMaster.CurrentSchemeTarget;
                iDetail.InterestCost = tmp > 0 ? tmp * 0.0025m : tmp * 0.005m;
            }

            iDetail.ReceivableDebt = iDetail.SettlementMoney * initMaster.GatheringRate - initMaster.CurrentRealGet;
            if (initMaster.ProjectState.Contains("完工6个月以上未办结算") || initMaster.ProjectState.Contains("已结算项目"))
            {
                iDetail.CompleteInterestCost = iDetail.ReceivableDebt * -0.0005m;
            }
            #endregion

            if (fundAssessmentType == 1)
            {
                initMaster.AssessCashDetails.Clear();
            }
            else if (fundAssessmentType == 2)
            {

                initMaster.Details.Clear();
            }
            return initMaster;
        }

        #endregion
    }
}
