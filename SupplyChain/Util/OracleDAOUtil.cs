using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Data;
using System.ComponentModel;
using System.Data.OracleClient;
using System.Data.Common;
using System.Data.SqlClient;

namespace Application.Business.Erp.SupplyChain.Util
{
    public class OracleDAOUtil
    {
        public static void SaveSolution(SqlConnection conn,string title, IDictionary<string, byte[]> solutions)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                
                try
                {
                   
                    //删除原方案
                    DeleteOldSolution(conn, title);

                    //新增方案
                    InsertNewSolution(conn, solutions, title);

                    //生成主表关联

                    scope.Complete();
                }
                catch (Exception e)
                {
                    throw e;
                }
              
            }
        }

        private static void DeleteOldSolution(SqlConnection conn, string title)
        {
            try
            {
                string updateSql = "DELETE FROM printsolution WHERE printtitle='" + title + "'";
                IDbCommand command = new SqlCommand(updateSql, conn);

                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        private static void InsertNewSolution(SqlConnection conn, IDictionary<string, byte[]> solutions, string title)
        {
            try
            {


                string insertSql = "insert into printsolution(printtitle,solution,solutiondata) values(@title,@solution,@solutiondata)";

                SqlCommand insertCommand = null;

                foreach (KeyValuePair<string, byte[]> kv in solutions)
                {
                    insertCommand = new SqlCommand(insertSql, conn);
                    insertCommand.Parameters.Add("title", SqlDbType.VarChar).Value = title;
                    insertCommand.Parameters.Add("solution", SqlDbType.VarChar).Value = kv.Key;
                    insertCommand.Parameters.Add("solutiondata", SqlDbType.Image).Value = kv.Value;

                    insertCommand.ExecuteNonQuery();
                }

            }
            catch
            {
                throw;
            }
        }
        
        public static IDictionary<string, byte[]> LoadSolution(SqlConnection conn,string title)
        {         

            try
            {
                IDictionary<string, byte[]> dic = new Dictionary<string, byte[]>();

                string masterSQL = "SELECT printtitle,solution,solutiondata FROM printsolution WHERE printtitle='" + title + "' ORDER BY PRINTSOLID";

                DataSet printDs = new DataSet();
                SqlDataAdapter masterAdapter = new SqlDataAdapter(masterSQL, conn);

                SqlCommand command = new SqlCommand(masterSQL, conn);
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    string solution = reader["solution"].ToString();
                    byte[] data = (byte[])reader["solutiondata"];
                    dic.Add(solution, data);
                }


                return dic;

            }
            catch (Exception e)
            {
                throw e;
            }
           
        }


    }
}
