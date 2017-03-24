using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.HelpOnlineManage.Domain;
using NHibernate;
using System.Data;
using NHibernate.Criterion;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.HelpOnlineManage.Service
{
    public class HelpOnlineSrv : BaseService, IHelpOnlineSrv
    {
        [TransManager]
        public HelpOnlineMng saveImp(HelpOnlineMng obj)
        {
            //if (obj.Id == null)
            //{
            //    obj.CreateDate= DateTime.Now;
            //}
            //if(obj==null)
            dao.SaveOrUpdate(obj);
            return obj;
        }
        public DataSet HelpOnlineQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"SELECT t1.Id ,t1.MenuId,t1.MenuName,t1.MenuDescript,t1.CreatePersonName,t1.LastUpdateDate,t1.CreateDate,t1.DocState
                From thd_helponlinemanage t1";
            sql += " where 1=1 " + condition + "";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
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
    }
}
