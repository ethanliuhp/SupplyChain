using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using System.Data;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.Util;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.OracleClient;
using System.Data.SqlClient;
namespace Application.Business.Erp.SupplyChain.Base.Service
{
    [Service("PrintSolutionAccess")]
    public class PrintSolutionAccess : IPrintSolutionAccess
    {       
        /// <summary>
        /// 保存打印方案
        /// </summary>
        /// <param name="title"></param>
        /// <param name="solutions"></param>
        virtual public void Save(string title, IDictionary<string, byte[]> solutions)
        {
            
            OracleDAOUtil.SaveSolution(GetDbConnection(), title, solutions);
        }

        virtual public void Save(string title, string menuStr, IDictionary<string, byte[]> solutions)
        {
            
        }

        public IDictionary<string, byte[]> Load(string title)
        {
            return OracleDAOUtil.LoadSolution(GetDbConnection(),title);
        }

        public IDictionary<string, byte[]> Load(string title, string menuStr)
        {
            throw null;
        }


        private SqlConnection GetDbConnection()
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            return session.Connection as SqlConnection;
        }
    }
}
