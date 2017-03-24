using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Remoting.Messaging;
using NHibernate;
using VirtualMachine.Core.DataAccess;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.SystemAspect.Security;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using System.Data.SqlClient;
using AuthManagerLib.AuthMng.MenusMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Util;
namespace IRPServiceModel.Services.Common
{
    public class CommonMethodSrv : ICommonMethodSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }
        public IList Query(Type type, ObjectQuery oQuery)
        {
            return dao.ObjectQuery(type, oQuery);
        }
        public object QueryById(Type type, string sID)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", sID));
            IList lst = dao.ObjectQuery(type, oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0];
        }
        public DataSet GetDataAndRowCount(string sDataSQL, int iCurrentPage, int iPageSize, ref int iRowCount)
        {
            string sQueryData = string.Format("select * from ({0}) t where   t.num between {1} and {2}", sDataSQL, (iCurrentPage - 1) * iPageSize + 1, iCurrentPage * iPageSize);
            string sQueryDataCount = string.Format("select count(*)  from ({0}) t", sDataSQL);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sQueryData;
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            command.CommandText = sQueryDataCount;
            iRowCount = int.Parse(command.ExecuteScalar().ToString());

            return ds;
        }
        public DataSet GetData(string sSQL)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return ds;
        }
        public void InsertData(string sql)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
        #region 登录信息 权限 岗位 角色查询
        public IList GetPersonOnJob(string sUserCode, string sPassWord)
        {

            ObjectQuery oQuery = new ObjectQuery();
            string sTempPassWord = string.Empty;
            if (string.IsNullOrEmpty(sPassWord))
            {
                sPassWord = "";
            }
                sTempPassWord = CryptoString.Encrypt(sPassWord, sPassWord);
 
            // oQuery.AddCriterion(Expression.Eq("StandardPerson.Password", sPassWord));
            oQuery.AddCriterion(Expression.Eq("StandardPerson.Code", sUserCode));
            oQuery.AddCriterion(Expression.Eq("State", 1));
            oQuery.AddFetchMode("OperationJob", FetchMode.Eager);
            oQuery.AddFetchMode("StandardPerson", FetchMode.Eager);
            oQuery.AddFetchMode("OperationJob.OperationOrg", FetchMode.Eager);
            IList lstResult = dao.ObjectQuery(typeof(PersonOnJob), oQuery);
            for (int i = lstResult.Count - 1; i > -1; i--)
            {
                PersonOnJob oPersonOnJob = lstResult[i] as PersonOnJob;
                if (oPersonOnJob.StandardPerson != null)
                {
                    if (string.Equals(oPersonOnJob.StandardPerson.Password, sTempPassWord) || string.Equals(oPersonOnJob.StandardPerson.Password, sPassWord))
                    {
                        continue;
                    }
                    else if (string.IsNullOrEmpty(oPersonOnJob.StandardPerson.Password) && string.IsNullOrEmpty(sTempPassWord))
                    {
                        continue;
                    }
                    else
                    {
                        lstResult.RemoveAt(i);
                    }
                }
            }
            return lstResult;
        }
        public IList GetPersonOnJob(string sUserCode)
        {

            ObjectQuery oQuery = new ObjectQuery();
     
            oQuery.AddCriterion(Expression.Eq("StandardPerson.Code", sUserCode));
            oQuery.AddCriterion(Expression.Eq("State", 1));
            oQuery.AddFetchMode("OperationJob", FetchMode.Eager);
            //oQuery.AddFetchMode("StandardPerson", FetchMode.Eager);
            oQuery.AddFetchMode("OperationJob.OperationOrg", FetchMode.Eager);
            IList lstResult = dao.ObjectQuery(typeof(PersonOnJob), oQuery);
            
            return lstResult;
        }
        public IList GetPersonOnJob(string sUserCode, string sPassWord, string sOperationJobId)
        {
            PersonOnJob o;

            ObjectQuery oQuery = new ObjectQuery();
            string sTempPassWord = string.Empty;
            if (string.IsNullOrEmpty(sPassWord))
            {
                sPassWord = "";
            }
            sTempPassWord = CryptoString.Encrypt(sPassWord, sPassWord);
            //oQuery.AddCriterion(Expression.Eq("StandardPerson.Password", sPassWord));
            oQuery.AddCriterion(Expression.Eq("StandardPerson.Code", sUserCode));
            oQuery.AddCriterion(Expression.Eq("OperationJob.Id", sOperationJobId));
            oQuery.AddCriterion(Expression.Eq("State", 1));
            oQuery.AddFetchMode("OperationJob", FetchMode.Eager);
            oQuery.AddFetchMode("StandardPerson", FetchMode.Eager);
            oQuery.AddFetchMode("OperationJob.OperationOrg", FetchMode.Eager);
            IList lstResult = dao.ObjectQuery(typeof(PersonOnJob), oQuery);
            for (int i = lstResult.Count - 1; i > -1; i--)
            {
                PersonOnJob oPersonOnJob = lstResult[i] as PersonOnJob;
                if (oPersonOnJob.StandardPerson != null)
                {
                    if (string.Equals(oPersonOnJob.StandardPerson.Password, sTempPassWord) || string.Equals(oPersonOnJob.StandardPerson.Password, sPassWord))
                    {
                        continue;
                    }
                    else if (string.IsNullOrEmpty(oPersonOnJob.StandardPerson.Password) && string.IsNullOrEmpty(sTempPassWord))
                    {
                        continue;
                    }
                    else
                    {
                        lstResult.RemoveAt(i);
                    }
                }
            }
            return lstResult;
        }
        public IList GetAuthConfigByOperationJobID(string sOperationJobID, string sRootMenuCode)
        {
            IList lstAuthConfig = null;
            Menus oRootMenus = GetMenus(sRootMenuCode);
            ObjectQuery oq = new ObjectQuery();
            if (!string.IsNullOrEmpty(sOperationJobID))
            {
                List<string> roleIdLst = GetRoleIdByJobId(sOperationJobID);
                if (roleIdLst == null || roleIdLst.Count == 0) return new ArrayList();

                Disjunction dis = Expression.Disjunction();
                foreach (string id in roleIdLst)
                {
                    dis.Add(Expression.Eq("Roles.Id", id));
                }
                oq.AddCriterion(dis);
            }
            string IdCode = oRootMenus == null ? "%" : oRootMenus.Id + "%";
            oq.AddCriterion(Expression.Like("Menus.IdCode", IdCode));
            oq.AddOrder(Order.Asc("Menus.Level"));
            oq.AddOrder(Order.Asc("Menus.Serial"));
            lstAuthConfig = GetAuthConfig(oq);

            return lstAuthConfig;
        }
        public Menus GetMenus(string sCode)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Code", sCode));
            IList lstMenus = dao.ObjectQuery(typeof(Menus), oQuery);
            return lstMenus == null || lstMenus.Count == 0 ? null : lstMenus[0] as Menus;
        }
        /// <summary>
        /// 根据岗位查询角色ID集合
        /// </summary>
        /// <param name="operationJobId"></param>
        /// <returns></returns>
        private List<string> GetRoleIdByJobId(string operationJobId)
        {
            List<string> roleIdLst = new List<string>();
            if (string.IsNullOrEmpty(operationJobId)) return roleIdLst;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select distinct operationRole from resOperationJobWithRole where operationJob=:operationJobId";
            if (conn is SqlConnection)
            {
                sql = @"select distinct operationRole from resOperationJobWithRole where operationJob=@operationJobId";
            }
            command.CommandText = sql;
            IDbDataParameter p_operationJobId = command.CreateParameter();
            p_operationJobId.ParameterName = "operationJobId";
            p_operationJobId.Value = operationJobId;
            command.Parameters.Add(p_operationJobId);
            using (IDataReader dr = command.ExecuteReader())
            {
                while (dr.Read())
                {
                    roleIdLst.Add(dr.GetString(0));
                }
            }
            return roleIdLst;
        }
        /// <summary>
        /// 查询权限菜单配置集合
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetAuthConfig(ObjectQuery oq)
        {
            oq.AddFetchMode("Menus", FetchMode.Eager);
            oq.AddFetchMode("Roles", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(AuthConfig), oq);
        }
        public IList GetAuthConfigByMenuIDCode(string sRootMenuCode)
        {
            IList lstAuthConfig = null;
            Menus oRootMenus = GetMenus(sRootMenuCode);
            ObjectQuery oq = new ObjectQuery();
            string IdCode = oRootMenus == null ? "%" : oRootMenus.Id + "%";
            oq.AddCriterion(Expression.Like("Menus.IdCode", IdCode));
            lstAuthConfig = GetAuthConfig(oq);
            return lstAuthConfig;
        }
        public IList GetAdminMenus(string sRootMenuCode)
        {
            IList lstMenu = null;
            Menus oRootMenus = GetMenus(sRootMenuCode);
            ObjectQuery oq = new ObjectQuery();
            string IdCode = oRootMenus == null ? "%" : oRootMenus.Id + "%";
            oq.AddCriterion(Expression.Like("IdCode", IdCode));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("Serial"));
            lstMenu = dao.ObjectQuery(typeof(Menus), oq);
            return lstMenu;
        }
        /// <summary>
        /// 获取组织信息
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public OperationOrgInfo  GetOperationOrgInfo(string sID)
        {
            return dao.Get(typeof(OperationOrgInfo), sID) as OperationOrgInfo;

        }
        #endregion

        #region
        /// <summary>
        /// 查找项目信息
        /// </summary>
        /// <param name="ownerorgsyscode">组织层次码</param>
        /// <returns></returns>
        public CurrentProjectInfo GetProjectInfo(string ownerorgsyscode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Sql(" instr('" + ownerorgsyscode + "',{alias}.ownerorgsyscode)>0"));
            IList lst = Dao.ObjectQuery(typeof(CurrentProjectInfo), oq);
            if (lst != null && lst.Count > 0)
            {
                foreach (CurrentProjectInfo projectInfo in lst)
                {
                    if (projectInfo.Code != "0000")
                    {
                        return projectInfo;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 查找项目信息
        /// </summary>
        /// <param name="ownerorgsyscode">组织层次码</param>
        /// <returns></returns>
        public CurrentProjectInfo GetProjectInfo(ObjectQuery oq)
        {
            IList lst = Dao.ObjectQuery(typeof(CurrentProjectInfo), oq);
            if (lst != null && lst.Count > 0) return lst[0] as CurrentProjectInfo;
            return null;
        }

        /// <summary>
        /// 根据ID查找项目
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns></returns>
        public CurrentProjectInfo GetProjectInfoById(string projectId)
        {
            return Dao.Get(typeof(CurrentProjectInfo), projectId) as CurrentProjectInfo;
        }
        /// <summary>
        /// 通过组织ID查询项目ID
        /// </summary>
        public string GetProjectIDByOperationOrg(string opgID)
        {
            string projectID = "";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            //分公司信息
            command.CommandText = " select t1.id from resconfig t1 where t1.ownerorg='" + opgID + "' and nvl(t1.projectcurrstate,0) != 20 and t1.projectcode != '0000'";
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dataTable = ds.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    projectID = TransUtil.ToString(dataRow["id"]);
                }
            }
            return projectID;
        }
        #endregion

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="sqlTable">完整的SQL语句，做tableName用，如：select * from ( sqlTable ) t where t.</param>
        /// <param name="iCurrentPage">当前页</param>
        /// <param name="iPageSize">分页大小</param>
        /// <param name="iRowCount">总记录数</param>
        /// <returns></returns>
        public DataSet GetDataByPage(string sqlTable, int iCurrentPage, int iPageSize, ref int iRowCount)
        {
            string sQueryData = string.Format(@"select *
  from (select ROWNUM as num,tab0.* from ({0}) tab0) tab1
 where tab1.num between {1} and {2}", sqlTable, (iCurrentPage - 1) * iPageSize + 1, iCurrentPage * iPageSize);
            string sQueryDataCount = string.Format("select count(*)  from ({0}) t", sqlTable);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            command.CommandText = sQueryData;
            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            command.CommandText = sQueryDataCount;
            iRowCount = int.Parse(command.ExecuteScalar().ToString());

            return ds;
        }
    }
}
