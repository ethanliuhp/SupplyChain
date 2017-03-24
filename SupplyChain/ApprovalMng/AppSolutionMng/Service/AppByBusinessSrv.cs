using System.Collections;
using System.Data;
using System.Runtime.Remoting.Messaging;
using Application.Business.Erp.SupplyChain.Base.Service;
using NHibernate;
using VirtualMachine.Core;
using VirtualMachine.Core.DataAccess;
using VirtualMachine.Core.Expression;
using System;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;

namespace Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Service
{
    public class AppByBusinessSrv : BaseService,IAppByBusinessSrv
    {
        private ICategoryNodeService nodeSrv;
        public ICategoryNodeService NodeSrv
        {
            get { return nodeSrv; }
            set { nodeSrv = value; }
        }

        public object GetDomain(Type t, ObjectQuery o)
        {
            IList ls = base.GetDomainByCondition(t, o);
            if (ls.Count > 0)
            {
                return ls[0];
            }
            else
            {
                return null;
            }
        }
        public AppSolutionSet GetExecAppSolution(Type t, decimal _money)
        {
            
            //查找审批表单类
            ObjectQuery OQ=new ObjectQuery();
            OQ.AddCriterion(Expression.Eq("ClassName",t.Name));
            AppTableSet curAppTableSet = GetDomain(typeof(AppTableSet), OQ) as AppTableSet;
            if (curAppTableSet!=null)
            {
                ObjectQuery OS = new ObjectQuery();
                OS.AddCriterion(Expression.Eq("ParentId", curAppTableSet));
                //OS.AddCriterion(Expression.Eq("Status", 1L));
                IList lsSolutionSet = GetObjects(typeof(AppSolutionSet), OS);
                //如果存在默认方案 则返回
                foreach (AppSolutionSet item in lsSolutionSet)
                {
                    if (item.Status==1)
                    {
                        return item;
                    }
                }
                //如果没有默认方案 则根据条件查找审批方案
                foreach (AppSolutionSet item in lsSolutionSet)
                {
                    try
                    {
                        string[] sArray = item.Conditions.Split(';');
                        decimal je1 = ClientUtil.ToDecimal(sArray[0]);
                        decimal je2 = ClientUtil.ToDecimal(sArray[1]);

                        if (_money >= je1 && _money <= je2)
                        {
                            return item;
                        }

                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        [TransManager]
        public object SaveSubmittedApp(object o)
        {
            return base.SaveOrUpdateByDao(o);
        }
        [TransManager]
        public IList SaveSubmittedApp(IList l)
        {
            return base.SaveOrUpdateByDao(l);
        }
        [TransManager]
        public bool Delete(object o)
        {
            try
            {
                base.DeleteByDao(o);

            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        [TransManager]
        public bool Delete(IList l)
        {
            try
            {
                base.dao.Delete(l);

            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 返回SQL
        /// </summary>
        /// <param name="_SQL"></param>
        /// <returns></returns>
        public DataSet GetStockInRefier(string _SQL)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;

            IDbCommand cmd = cnn.CreateCommand();

            //ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
            //tran.Enlist(cmd);
            cmd.CommandText = _SQL;
            cmd.CommandTimeout = 100;
            IDataReader dataReader = cmd.ExecuteReader(CommandBehavior.Default);
            return DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
        }
  
    }
}
