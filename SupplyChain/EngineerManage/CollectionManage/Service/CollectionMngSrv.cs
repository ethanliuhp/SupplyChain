using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using VirtualMachine.SystemAspect.Security;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data;
using VirtualMachine.Core.DataAccess;
using Application.Resource.FinancialResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.CommonClass.Domain;
using Iesi.Collections;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.EngineerManage.CollectionManage.Domain;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.EngineerManage.CollectionManage.Service;
namespace Application.Business.Erp.SupplyChain.CollectionManage.Service
{
    public class   Collectionmngsrv : BaseService, ICollectionMngSrv
    {

        #region code生成方法
        private IBillCodeRuleSrv billcoderulesrv;
        public IBillCodeRuleSrv Billcoderulesrv
        {
            get { return billcoderulesrv; }
            set { billcoderulesrv = value; }
        }

        private string getcode(Type type)
        {
            return billcoderulesrv.GetBillNoNextValue(type, "code", DateTime.Now);
        }
        #endregion
        /// <summary>
        /// 通过id查询收发函信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ColletionManage GetImpById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetImp(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ColletionManage;
            }
            return null;
        }


        /// <summary>
        /// 通过code查询收发邀请函
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ColletionManage GetImpByCode(string code)
        {  ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Code", code));


            IList list = GetImp(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as ColletionManage;
            }
            return null;
        }
        /// <summary>
        /// 保存或修改收发邀请函
        /// </summary>
        /// <param name="item">收发邀请函</param>
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
        /// 查询收发邀请函信息
        /// </summary>
        /// <param name="objectquery"></param>
        /// <returns></returns>
      public IList GetImp(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ColletionManage), objectQuery);
        }
        [TransManager]
        public ColletionManage saveImp(ColletionManage obj)
        {
            if (obj.Id == null)
            {
                ///obj.Code = GetCode(typeof(MonthlyPlanMaster), obj.Special);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            //obj.LastModifyDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as ColletionManage;
        }
        public DataSet CollectionQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.Id,t1.DocState, t1.CreatePersonName,t1.LettersId,t1.HandOrgLevel,t1.OpgSysCode,t1.LettersName,t1.LettersStyle,t1.SendUnits,t1.GetUnits,t1.Remark,t1.SendLettersDate,t1.SendStyle,t1.RealOperationDate
                From thd_collectionmanage t1";
            sql += " where 1=1 " + condition;
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
    }
}
