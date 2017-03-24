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
using Application.Business.Erp.SupplyChain.StockManage.Base.Service;
using Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using NHibernate.Exceptions;
using VirtualMachine.Core.Expression;
using Application.Resource.FinancialResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using CommonSearchLib.BillCodeMng.Service;
using VirtualMachine.Core.DataAccess;

namespace Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Service
{
    /// <summary>
    /// 盘亏出库
    /// </summary>
    public class LossOutSrv : BaseService, ILossOutSrv
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
        private string GetCode(Type type, string special)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now,special);
        }
        #endregion
        
        private IStockInOutSrv refStockInOutSrv;

        virtual public IStockInOutSrv RefStockInOutSrv
        {
            get { return refStockInOutSrv; }
            set { refStockInOutSrv = value; }
        }

        /// <summary>
        /// 查询盘亏单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetLossOut(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(LossOut), oq);
        }

        /// <summary>
        /// 保存盘亏单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public LossOut SaveLossOut(LossOut obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(LossOut),obj.Special);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            return SaveOrUpdateByDao(obj) as LossOut;
        }

        /// <summary>
        /// 删除盘亏单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteLossOut(LossOut obj)
        {
            return DeleteByDao(obj);
        }

        /// <summary>
        /// 盘亏单记账
        /// </summary>
        /// <param name="hashLst"></param>
        /// <param name="hashCode"></param>
        /// <param name="tallyDate"></param>
        /// <param name="componentPeriod"></param>
        /// <param name="auditPerson"></param>
        /// <returns></returns>
        virtual public Hashtable Tally(Hashtable hashLst, Hashtable hashCode, DateTime tallyDate, ComponentPeriod componentPeriod, PersonInfo auditPerson,string projectId)
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
            //盘亏单记账
            IList lst = hashLst["LossOut"] as IList;
            IList lstCode = hashCode["LossOutCode"] as IList;
            if (lst != null)
            {
                foreach (string var in lst)
                {
                    IDbCommand cmd = cnn.CreateCommand();
                    cmd.CommandText = "thd_StockLossOutTally";
                    cmd.CommandType = CommandType.StoredProcedure;
                    #region 传入参数

                    IDbDataParameter id = cmd.CreateParameter();
                    id.Value = var;
                    id.Direction = ParameterDirection.Input;
                    id.ParameterName = "BillId";
                    cmd.Parameters.Add(id);

                    IDbDataParameter tallyPersonId = cmd.CreateParameter();
                    tallyPersonId.Value = auditPerson.Id;
                    tallyPersonId.Direction = ParameterDirection.Input;
                    tallyPersonId.ParameterName = "TallyPersonId";
                    cmd.Parameters.Add(tallyPersonId);

                    IDbDataParameter tallyPersonName = cmd.CreateParameter();
                    tallyPersonName.Value = auditPerson.Name;
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
                    //TallyMonth.DbType = DbType.UInt64;
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
        /// 
        /// </summary>
        /// <param name="sCondition"></param>
        /// <returns></returns>
        public DataSet LossOutQuery(string sCondition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.id,t1.CreateDate,t1.CreatePersonName,t1.Descript,t2.MaterialCode,t2.MaterialName,t2.MaterialSpec,t2.DiagramNumber,t2.MatStandardUnit,t2.MatStandardUnitName,t1.Code,t1.State,t2.quantity  FROM thd_stklossout t1 INNER JOIN thd_stklossoutdtl t2 ON t1.Id = t2.ParentId";
            sql += " where 1=1 " + sCondition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

    }
}
