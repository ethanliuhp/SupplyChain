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
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;
using Application.Business.Erp.SupplyChain.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Service
{
    /// <summary>
    /// 业主报量
    /// </summary>
    public class OwnerQuantitySrv : BaseService, IOwnerQuantitySrv
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
        /// 根据项目生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }
        #endregion

        #region 业主报量
        /// <summary>
        /// 通过ID查询业主报量信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OwnerQuantityMaster GetOwnerQuantityById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetOwnerQuantity(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as OwnerQuantityMaster;
            }
            return null;
        }

        /// <summary>
        /// 通过Code查询业主报量信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OwnerQuantityMaster GetOwnerQuantityByCode(string code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetOwnerQuantity(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as OwnerQuantityMaster;
            }
            return null;
        }

        /// <summary>
        /// 业主报量查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOwnerQuantity(ObjectQuery objectQuery)
        {
            //objectQuery.AddFetchMode("Details.MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(OwnerQuantityMaster), objectQuery);
        }

        /// <summary>
        /// 业主报量状态查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetOwner(ObjectQuery objectQuery)
        {
            return Dao.ObjectQuery(typeof(OwnerQuantity), objectQuery);
        }

        /// <summary>
        /// 项目业主报量状态查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet OwnerQuantitySearch(string projectId,string qwbsGUID)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "";
            if (qwbsGUID == "")
            {
                sql = @"SELECT max(t1.CreateDate) as CreateDate,t2.PriceUnitName,t1.ProjectName,sum(CollectionSumMoney)as Collect,sum(ConfirmSumMoney) as Confirm,sum(SubmitSumQuantity) as Submit FROM THD_OwnerQuantityMaster t1 INNER JOIN THD_OwnerQuantityDetail t2 ON t1.Id = t2.ParentId";
                sql += " where projectId='" + projectId + "' group by t1.ProjectName,t2.PriceUnitName";
            }
            else {
                sql = @"SELECT max(t1.CreateDate) as CreateDate,t2.PriceUnitName,t1.ProjectName,t2.QWBSName,sum(CollectionSumMoney)as Collect,sum(ConfirmSumMoney) as Confirm,sum(SubmitSumQuantity) as Submit FROM THD_OwnerQuantityMaster t1 INNER JOIN THD_OwnerQuantityDetail t2 ON t1.Id = t2.ParentId";
                sql += " where t1.projectId ='" + projectId + "' and t2.QWBSName ='" + qwbsGUID + "' group by t2.PriceUnitName,t1.ProjectName,t2.QWBSName,t2.QWBS";
            }
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = dataSet.Tables[0];
            
            string createDate = "";
            string sqlDate = " select max(t1.CreateDate) createDate from THD_OwnerQuantityMaster t1 where t1.Projectid  = '" + projectId + "'";
            command.CommandText = sqlDate;
            IDataReader dataReader1 = command.ExecuteReader();
            DataSet dataSet1 = DataAccessUtil.ConvertDataReadertoDataSet(dataReader1);
            if (dataSet1 != null)
            {
                DataTable dataTable1 = dataSet1.Tables[0];
                foreach (DataRow dataRow in dataTable1.Rows)
                {
                    createDate = TransUtil.ToString(dataRow["createDate"]);
                }
            }
            //将两个临时表中的信息融合为一个临时表
            foreach (DataRow dataRow in dataTable.Rows)
            {
                dataRow["CreateDate"] = createDate;
            }

            return dataSet;
        }


        /// <summary>
        /// 业主报量查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public DataSet OwnerQuantity(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT * from THD_OwnerQuantity";
            sql += " where 1=1 " + condition + "";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        public DataSet OwnerQuantityQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT t1.id,t1.Code,t1.State,t1.Descript,t1.AccountSign,t1.QuantityType,t1.CreateDate,t1.CreatePersonName,t1.RealOperationDate,t1.HandlePersonName,t2.id dtlID,"
                + " t2.ConfirmMoney,t2.QWBSCode,t2.QWBSName,t2.confirmStartDate,t2.confirmEndDate,t2.gatheringRate,t2.acctGatheringMoney,t2.SubmitQuantity,t2.QuantityDate,t2.ConfirmDate,t2.Descript DtlDescript "
                +" FROM THD_OwnerQuantityMaster t1 INNER JOIN THD_OwnerQuantityDetail t2 ON t1.Id = t2.ParentId ";
            sql += " where 1=1 " + condition + " order by t1.code";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        [TransManager]
        public OwnerQuantityMaster SaveOwnerQuantity(OwnerQuantityMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(OwnerQuantityMaster), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            obj.LastModifyDate = DateTime.Now;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            obj = SaveOrUpdateByDao(obj) as OwnerQuantityMaster;

            //将信息保存到状态查询中去
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", obj.ProjectId));
            IList listOwner = GetOwner(objectQuery);
            if (listOwner.Count <= 0)
            {
                //直接向数据库添加信息
                OwnerQuantity owner = new OwnerQuantity();
                owner.LastUpdateDate = DateTime.Now;
                owner.ProjectId = obj.ProjectId;
                owner.ProjectName = obj.ProjectName;
                owner.RealCollectionMoney = obj.CollectionSumMoney;//实际收款金额
                owner.SumConfirmMoney = obj.ConfirmSumMoney;//业主确认金额
                //owner.SumContractMoney = curBillMaster;//合同总金额
                owner.SumPayforMoney = obj.SumPayforMoney;//应付累计金额
                owner.SumSubmitMoney = obj.SubmitSumQuantity;//报送金额
                foreach (OwnerQuantityDetail detail in obj.Details)
                {
                    owner.UnitPrice = detail.PriceUnit;
                    owner.UnitPriceName = detail.PriceUnitName;
                    owner.QWBSGUID = detail.QWBS;
                    owner.QWBSName = detail.QWBSName;
                    owner.QWBSSysCode = detail.QWBSCode;
                }

                owner = SaveOwner(owner);
            }
            else
            {
                //修改信息
                OwnerQuantity owner = new OwnerQuantity();
                owner = listOwner[0] as OwnerQuantity;
                if (obj.Id != null)
                {

                    //更新业主报量信息
                    owner.RealCollectionMoney = Convert.ToDecimal(owner.RealCollectionMoney) - Convert.ToDecimal(obj.Temp3) + Convert.ToDecimal(obj.CollectionSumMoney);//实际收款金额
                    owner.SumConfirmMoney = Convert.ToDecimal(owner.SumConfirmMoney) - Convert.ToDecimal(obj.Temp4) + Convert.ToDecimal(obj.ConfirmSumMoney);//业务确认金额
                    owner.SumPayforMoney = Convert.ToDecimal(owner.SumPayforMoney) - Convert.ToDecimal(obj.Temp2) + Convert.ToDecimal(obj.SumPayforMoney);//应付金额
                    owner.SumSubmitMoney = Convert.ToDecimal(owner.SumSubmitMoney) - Convert.ToDecimal(obj.Temp1) + Convert.ToDecimal(obj.SubmitSumQuantity);//报送金额
                    owner = SaveOwner(owner);
                }
                else
                {
                    //新增业主报量信息
                    owner.RealCollectionMoney += obj.CollectionSumMoney;//实际收款金额
                    owner.SumConfirmMoney += obj.ConfirmSumMoney;//业务确认金额
                    owner.SumPayforMoney += obj.SumPayforMoney;//应付金额
                    owner.SumSubmitMoney += obj.SubmitSumQuantity;//报送金额
                    owner = SaveOwner(owner);
                }
            }
            return obj;
        }

        [TransManager]
        public OwnerQuantity SaveOwner(OwnerQuantity obj)
        {
            obj.LastUpdateDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as OwnerQuantity;
        }

        [TransManager]
        public OwnerQuantityMaster UpdateOwnerQuantity(OwnerQuantityMaster obj)
        {
            obj.LastModifyDate = DateTime.Now;
            return UpdateByDao(obj) as OwnerQuantityMaster;
        }
        #endregion
    }





}
