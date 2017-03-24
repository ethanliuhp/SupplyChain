using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using NHibernate;
using System.Data;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.FinancialResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Service
{
    /// <summary>
    /// 盘盈入库
    /// </summary>
    public class ProfitInSrv : BaseService,IProfitInSrv
    {
        private IStockInOutSrv refStockInOutSrv;

        virtual public IStockInOutSrv RefStockInOutSrv
        {
            get { return refStockInOutSrv; }
            set { refStockInOutSrv = value; }
        }

        public override object GetDomain(Type domain, string id, VirtualMachine.Core.ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("TheStationCategory", NHibernate.FetchMode.Eager);            
            objectQuery.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("HandleOrg", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);

            ProfitIn theProfitIn = new ProfitIn();
            theProfitIn = base.GetDomain(domain, id, objectQuery) as ProfitIn;

            if (theProfitIn != null)
            {
                foreach (ProfitInDtl detail in theProfitIn.Details)
                {
                    if (detail.MaterialResource.ComplexUnits != null && detail.MaterialResource.ComplexUnits.Count > 0)
                        Dao.InitializeObj(detail.MaterialResource.ComplexUnits);                    
                }
            }
            return theProfitIn;
        }

        public override System.Collections.IList GetDomainByCondition(Type objType, VirtualMachine.Core.ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("TheStationCategory", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("HandleOrg", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);

            ProfitIn theProfitIn = new ProfitIn();
            IList list = null;
            try
            {
                list = Dao.ObjectQuery(objType, objectQuery);
            }
            catch (Exception ex)
            {
            }
            //if (list != null && list.Count > 0)
            //{
            //    foreach (ProfitIn sm in list)
            //    {                    
            //        foreach (ProfitInDtl detail in sm.Details)
            //        {                       
            //            Dao.InitializeObj(detail.ForwardBill);
            //            if (detail.MaterialResource.ComplexUnits != null && detail.MaterialResource.ComplexUnits.Count > 0)
            //                Dao.InitializeObj(detail.MaterialResource.ComplexUnits);
            //        }
            //    }
            //}
            return list;
            return null;
        }

        public System.Collections.IList GetDomainByConditionByAcct(Type objType, VirtualMachine.Core.ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("TheCustomerRelationInfo.CustomerInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("HandleOrg", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);

            IList list = null;
            try
            {
                list = Dao.ObjectQuery(objType, objectQuery);
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public override IList GetDetailList(Type objType, ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Master.TheStationCategory", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.CreatePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("HandleOrg", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master.TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MatStandardUnit", NHibernate.FetchMode.Eager);

            IList list = null;
            try
            {
                list = Dao.ObjectQuery(objType, objectQuery); //base.GetDomainByCondition(objType, objectQuery);
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public override System.Collections.IList GetForwardMasterBills(LinkRule linkRule, ObjectQuery objectQuery, bool isEagerDetail)
        {
            objectQuery.AddFetchMode("TheStationCategory", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("CreatePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("HandleOrg", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheSupplierRelationInfo.SupplierInfo", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details.MatStandardUnit", NHibernate.FetchMode.Eager);

            IList masterList = base.GetForwardMasterBills(linkRule, objectQuery, isEagerDetail);

            //foreach (ProfitIn sm in masterList)
            //{
            //    foreach (ProfitInDtl detail in sm.Details)
            //    {
            //        detail.RefQuantity = RefBusinessEssenceQuerySrv.ReferTotal(linkRule, detail.Id);
            //        if (detail.MaterialResource.ComplexUnits != null && detail.MaterialResource.ComplexUnits.Count > 0)
            //            Dao.InitializeObj(detail.MaterialResource.ComplexUnits);
            //    }
            //}

            return masterList;
        }

        /// <summary>
        ///  盘盈单记账
        /// </summary>
        /// <param name="hashLst"></param>
        /// <param name="hashCode"></param>
        /// <param name="tallyDate"></param>
        /// <param name="componentPeriod"></param>
        /// <param name="AuditPerson"></param>
        /// <returns></returns>
        virtual public Hashtable Tally(Hashtable hashLst, Hashtable hashCode, DateTime tallyDate, ComponentPeriod componentPeriod, PersonInfo AuditPerson,string projectId)
        {
            if (!RefStockInOutSrv.CheckIfNewProject(projectId))
            {
                //检查上个月是否结帐，上个月必须结帐，本月才能开展记账业务
                if (!RefStockInOutSrv.CheckReckoning(componentPeriod.LastYear, componentPeriod.LastMonth, projectId))
                    throw new Exception("会计期【" + componentPeriod.LastYear + "-" + componentPeriod.LastMonth + "】未结账,请先进行结账！");

                if (RefStockInOutSrv.CheckReckoning(componentPeriod.NowYear, componentPeriod.NowMonth, projectId))
                    throw new Exception("会计期【" + componentPeriod.NowYear + "-" + componentPeriod.NowMonth + "】已经结账,不能进行记账！");
            }

            Hashtable returnValue = new Hashtable();

            IList list = new ArrayList();
            string errMsg = "";

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            //盘盈单记账
            IList lst = hashLst["ProfitIn"] as IList;
            IList lstCode = hashCode["ProfitInCode"] as IList;
            if (lst != null)
            {
                foreach (string var in lst)
                {
                    IDbCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "thd_StockProfitInTally";
                    cmd.CommandType = CommandType.StoredProcedure;
                    #region 传入参数

                    IDbDataParameter id = cmd.CreateParameter();
                    id.Value = var;
                    id.Direction = ParameterDirection.Input;
                    id.ParameterName = "BillId";
                    cmd.Parameters.Add(id);

                    IDbDataParameter tallyPersonId = cmd.CreateParameter();
                    tallyPersonId.Value = AuditPerson.Id;
                    tallyPersonId.Direction = ParameterDirection.Input;
                    tallyPersonId.ParameterName = "TallyPersonId";
                    cmd.Parameters.Add(tallyPersonId);

                    IDbDataParameter tallyPersonName = cmd.CreateParameter();
                    tallyPersonName.Value = AuditPerson.Name;
                    tallyPersonName.Direction = ParameterDirection.Input;
                    tallyPersonName.ParameterName = "TallyPersonName";
                    cmd.Parameters.Add(tallyPersonName);

                    IDbDataParameter TallyDate = cmd.CreateParameter();
                    TallyDate.DbType = DbType.DateTime;
                    TallyDate.Direction = ParameterDirection.Input;
                    TallyDate.Value = tallyDate;
                    TallyDate.ParameterName = "TallyDate";
                    cmd.Parameters.Add(TallyDate);

                    IDbDataParameter TallyYear = cmd.CreateParameter();
                    TallyYear.Direction = ParameterDirection.Input;
                    TallyYear.Value = componentPeriod.NowYear;
                    TallyYear.ParameterName = "TallyYear";
                    cmd.Parameters.Add(TallyYear);

                    IDbDataParameter TallyMonth = cmd.CreateParameter();
                    TallyMonth.Direction = ParameterDirection.Input;
                    TallyMonth.Value = componentPeriod.NowMonth;
                    TallyMonth.ParameterName = "TallyMonth";
                    cmd.Parameters.Add(TallyMonth);
                    #endregion

                    IDbDataParameter err = cmd.CreateParameter();
                    err.DbType = DbType.AnsiString;
                    err.Direction = ParameterDirection.Output;
                    err.ParameterName = "ErrMsg";
                    err.Size = 500;
                    cmd.Parameters.Add(err);
                    cmd.ExecuteNonQuery();
                    if (err.Value == null || Convert.ToString(err.Value) == "")
                        list.Add(var);
                    else
                        errMsg = errMsg + "单号:" + lstCode[lst.IndexOf(var)].ToString() + ":" + err.Value + "\n";

                }
            }
            returnValue.Add("err", errMsg);
            returnValue.Add("Succ", list);
            return returnValue;
        }

        /// <summary>
        /// 查询是否已经账面结帐
        /// </summary>
        /// <param name="kjn">会计年</param>
        /// <param name="kjy">会计月</param>
        /// <returns></returns>
        public bool QueryStockAcct(int kjn, int kjy)
        {
            bool ifHave = false;
            int last_kjn = 0;
            int last_kjy = 0;
            IDataReader dataReader;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string strSQL = " select max(fiscalyear) kjn_max from thd_accfiscalFlag ";
            command.CommandText = strSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                last_kjn = Int16.Parse(dr["kjn_max"].ToString());
            }

            strSQL = " select max(fiscalmonth) kjy_max from thd_accfiscalFlag where fiscalyear=" + last_kjn;
            command.CommandText = strSQL;
            dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds1 = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                last_kjy = Int16.Parse(dr["kjy_max"].ToString());
            }

            if (kjn < last_kjn)
            {
                ifHave = true;
            }

            if (kjn == last_kjn && kjy <= last_kjy)
            {
                ifHave = true;
            }
            return ifHave;
        }

        //xl
        /// <summary>
        /// 盘盈查询
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
      public   DataSet ProfitInQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.CreateDate,t1.CreatePersonName,t1.Descript,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.DiagramNumber,t2.MatStandardUnit,t2.MatStandardUnitName,t1.Code,t1.State,t2.quantity  FROM thd_stkprofitin t1 INNER JOIN thd_stkprofitindtl t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
         [TransManager]
        public ProfitIn SaveProfitIn(ProfitIn oProfitIn)
        {
            try
            {
                if (oProfitIn.Id == null)
                {
                    oProfitIn.RealOperationDate = DateTime.Now;

                }
                if (oProfitIn.DocState == DocumentState.InExecute || oProfitIn.DocState == DocumentState.InAudit)
                {
                    oProfitIn.SubmitDate = DateTime.Now;
                }
                return SaveByDao(oProfitIn) as ProfitIn;
            }
            catch
            {
                return null;
            }
        }
    }
}
