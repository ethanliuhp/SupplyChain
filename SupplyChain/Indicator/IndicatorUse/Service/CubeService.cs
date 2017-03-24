using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Oracle.DataAccess.Client;
using VirtualMachine.Core.DataAccess;
using System.Linq;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using System.Data.SqlClient;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Util;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service
{
    public class CubeService : Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service.ICubeService
    {
        #region 注入服务

        private IDao dao;
        virtual public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        private IDataAccess dBDao;

        /// <summary>
        /// 直接数据库操作DAO
        /// </summary>
        public IDataAccess DBDao
        {
            get { return dBDao; }
            set { dBDao = value; }
        }

        private IDimensionDefineService dimDefineSrv;
        public IDimensionDefineService DimDefineSrv
        {
            get { return dimDefineSrv; }
            set { dimDefineSrv = value; }
        }

        #endregion

        public SystemRegister GetSystemRegisterById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(SystemRegister), oq);
            if (list.Count > 0)
            {
                return list[0] as SystemRegister;
            }
            else
            {
                return null;
            }
        }

        #region 立方定义层服务

        /// <summary>
        /// 得到所有立方的集合
        /// </summary>
        /// <returns></returns>
        public IList GetAllCubeRegister()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("CubeAttribute", NHibernate.FetchMode.Eager);
            //oq.AddOrder(Order.Asc("Id"));
            IList list = Dao.ObjectQuery(typeof(CubeRegister), oq);
            return list;
        }

        /// <summary>
        /// 得到注册分系统的立方集合
        /// </summary>
        /// <returns></returns>
        public IList GetPartSystemCubeRegister(SystemRegister obj)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("SysRegister.Id", obj.Id));
            IList list = Dao.ObjectQuery(typeof(CubeRegister), oq);
            return list;
        }

        /// <summary>
        /// 通过立方注册ID获取立方信息
        /// </summary>
        /// <returns></returns>
        public CubeRegister GetCubeRegisterById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(CubeRegister), oq);
            if (list.Count > 0)
            {
                return list[0] as CubeRegister;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过立方注册名称获取立方信息
        /// </summary>
        /// <returns></returns>
        public CubeRegister GetCubeRegisterByName(String name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("CubeName", name));
            IList list = Dao.ObjectQuery(typeof(CubeRegister), oq);
            if (list.Count > 0)
            {
                return list[0] as CubeRegister;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 新增/修改立方注册信息
        /// </summary>
        /// <returns></returns>
        public CubeRegister SaveCubeRegister(CubeRegister obj)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                obj.CubeCode = GetNewCubeCode();
            }
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 删除立方注册信息
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCubeRegister(CubeRegister obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 通过立方ID获取该立方的属性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetCubeAttrByCubeResgisterId(CubeRegister obj)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("CubeRegis.Id", obj.Id));
            oq.AddOrder(Order.Asc("Id"));
            IList list = Dao.ObjectQuery(typeof(CubeAttribute), oq);
            return list;
        }


        /// <summary>
        /// 通过立方属性ID获取属性信息
        /// </summary>
        /// <param name="id">立方属性ID</param>
        /// <returns></returns>
        public CubeAttribute GetCubeAttributeById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(CubeAttribute), oq);
            if (list.Count > 0)
            {
                return list[0] as CubeAttribute;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 新增/修改立方属性信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public CubeAttribute SaveCubeAttribute(CubeAttribute obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 删除立方属性信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCubeAttribute(CubeAttribute obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 获取立方编码
        /// </summary>
        /// 立方编码规则为: kncubedata+最大值+1，例如:kncubedata12
        /// <returns></returns>
        private string GetNewCubeCode()
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string queryString = "select max(cubecode) from thd_cuberegister";            
            command.CommandText = queryString;
            object ret=command.ExecuteScalar();
            if (ret is DBNull || ret == null || ret.ToString().Equals(""))
            {
                return "thd_cubedata1";
            } else
            {
                string cubeCode=ret.ToString().ToUpper();
                string codeNum = cubeCode.Replace("thd_cubedata".ToUpper(), "");
                return "thd_cubedata" + (int.Parse(codeNum)+1);
            }
        }

        /// <summary>
        /// 调用脚本：系统动态生成立方数据表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CallDynamicCreateCubeData(CubeRegister obj)
        {
            IList list = GetCubeAttrByCubeResgisterId(obj);//字段拼接

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();            
            //建立表
            string table_name = obj.CubeCode;//表名
            string script = "";
            if (conn is SqlConnection)
            {
                script = " create table " + table_name + " ( ID varchar(80) NOT NULL primary key,";
                foreach (CubeAttribute attr in list)
                {
                    script += attr.DimensionCode + " varchar(80) NOT NULL , ";
                }
                script += "FactId varchar(80), RESULTVALUE numeric(16,4),PLANVALUE VARCHAR(40),SONVALUE numeric(16,4),MOTHERVALUE numeric(16,4) ) ";
            } else
            {
                script = " create table " + table_name + " ( ID varchar2(80) NOT NULL primary key,";

                foreach (CubeAttribute attr in list)
                {
                    script += attr.DimensionCode + " varchar2(80) NOT NULL , ";
                }
                script += "FactId varchar2(80),RESULTVALUE NUMBER(16,4),PLANVALUE VARCHAR2(40),SONVALUE NUMBER(16,4),MOTHERVALUE NUMBER(16,4) ) ";
            }

            command.CommandText = script;
            command.ExecuteNonQuery();

            return true;
        }

        /// <summary>
        /// 调用脚本：系统动态删除立方数据表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CallDynamicDelCubeData(CubeRegister obj)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string table_name = obj.CubeCode;
            string script = " drop table " + table_name;
            try
            {
                //删除表
                command.CommandText = script;
                command.ExecuteNonQuery();
            } catch { }
            return true;           
        }

        #endregion


        #region 立方数据操作层服务

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        /// <returns></returns>
        public string SetCubeData(CubeRegister cr, CubeData cd)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();

            string id =null;
            IList dimList = cd.DimDataList;

            //查询立方数据表中是否该记录
            string exist_sql = "select ID from " + cr.CubeCode + " where 1=1 ";
            int k = 0;
            if (conn is SqlConnection)
            {
                foreach (CubeAttribute ca in cr.CubeAttribute)
                {
                    //exist_sql += " and " + ca.DimensionCode + "=" + dimList[k];
                    exist_sql += " and " + ca.DimensionCode + "=@" + ca.DimensionCode;
                    IDbDataParameter para = command.CreateParameter();
                    para.Value = dimList[k];
                    para.ParameterName = ca.DimensionCode;
                    command.Parameters.Add(para);
                    k++;
                }
                //加上事实
                exist_sql += " and FactId=@FactId";
                IDbDataParameter paraFactId = command.CreateParameter();
                paraFactId.ParameterName = "FactId";
                paraFactId.Value = dimList[dimList.Count - 1];
                command.Parameters.Add(paraFactId);
            }
            else
            {
                foreach (CubeAttribute ca in cr.CubeAttribute)
                {
                    //exist_sql += " and " + ca.DimensionCode + "=" + dimList[k];
                    exist_sql += " and " + ca.DimensionCode + "=:" + ca.DimensionCode;
                    IDbDataParameter para = command.CreateParameter();
                    para.Value = dimList[k];
                    para.ParameterName = ca.DimensionCode;
                    command.Parameters.Add(para);
                    k++;
                }
                //加上事实
                exist_sql += " and FactId=@FactId";
                IDbDataParameter paraFactId = command.CreateParameter();
                paraFactId.ParameterName = "FactId";
                paraFactId.Value = dimList[dimList.Count - 1];
                command.Parameters.Add(paraFactId);
            }
            command.CommandText = exist_sql;
            object idObj = command.ExecuteScalar();
            if (!(idObj is DBNull) && idObj != null && !idObj.ToString().Equals(""))
            {
                id = idObj.ToString();
            }
            if (string.IsNullOrEmpty(id))
            {
                IFCGuidGenerator guid=new IFCGuidGenerator();
                string newId = guid.GeneratorIFCGuid();
                string sql = "insert into " + cr.CubeCode+"(id";
                foreach (CubeAttribute ca in cr.CubeAttribute)
                {
                      sql += "," + ca.DimensionCode;
                }
                sql += ",FactId,RESULTVALUE,PLANVALUE,SONVALUE,MOTHERVALUE) values('" + newId + "'";
                for (int i = 0; i < cr.CubeAttribute.Count; i++)
                {
                    sql += ",'"+dimList[i]+"'";
                }
                //加上事实
                sql += ",'" + dimList[dimList.Count - 1] + "'";
                sql += "," + cd.Result + ",'" + cd.Plan + "'," + cd.SonValue + "," + cd.MotherValue + ")";
                command = conn.CreateCommand();
                IDbTransaction tx = conn.BeginTransaction();
                command.Transaction = tx;
                command.CommandText = sql;
                try
                {
                    command.ExecuteNonQuery();
                    tx.Commit();
                    id = newId;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
            return id;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        /// <returns></returns>
        public long InsertCubeData(CubeRegister cr, CubeData cd)
        {
            long id = -1;
            IList dimList = cd.DimDataList;

            //得到序列号
            string findId = " select " + cr.CubeCode + "_seq.currval FROM DUAL ";

            int count =cr.CubeAttribute.Count;
            string sql = "insert into " + cr.CubeCode + " ( ID ";
            foreach (CubeAttribute ca in cr.CubeAttribute)
            {
                sql += "," + ca.DimensionCode;

            }
            sql += ", RESULTVALUE, PLANVALUE,SONVALUE,MOTHERVALUE ) values ( " + cr.CubeCode + "_seq.nextval ";
            for (int i = 0; i < count; i++)
            {
                sql += "," + dimList[i];

            }
            sql += " , " + cd.Result + " , '" + cd.Plan + "' , " + cd.SonValue + ", " + cd.MotherValue + ") ";
            DBDao.ExecuteNonQuery(CommandType.Text, null, sql);

            DataSet seqDs = DBDao.OpenQueryDataSet(findId, null);

            if (seqDs != null)
            {
                string id_str = seqDs.Tables[0].Rows[0][0].ToString();
                if (id_str != null && !"".Equals(id_str))
                {
                    id = long.Parse(id_str);
                }

            }
            return id;
        }


        // <summary>
        /// 删除立方数据,提供通过主键ID进行删除
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据,ID不能为空</param>
        public bool DeleteCubeDataById(CubeRegister cr, CubeData cd)
        {            
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn=session.Connection;
            IDbTransaction tx = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            command.Transaction = tx;
            string sql = "";
            if (conn is SqlConnection)
            {
                sql = "delete from " + cr.CubeCode + " where ID =@Id";
            } else
            {
                sql = "delete from " + cr.CubeCode + " where ID =:Id";
            }
            command.CommandText = sql;
            IDbDataParameter p_id = command.CreateParameter();
            p_id.Value = cd.Id;
            p_id.ParameterName = "Id";
            command.Parameters.Add(p_id);
            try
            {
                command.ExecuteNonQuery();
                tx.Commit();
            } catch
            {
                tx.Rollback();
                throw;
            }            
            return true;
        }

        // <summary>
        /// 删除立方数据,提供通过维度集进行删除
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        public bool DeleteCubeDataByDimensions(CubeRegister cr, CubeData cd)
        {
            string sql = " delete from " + cr.CubeCode + " where  1=1 ";
            IList attrList = this.GetCubeAttrByCubeResgisterId(cr);
            IList dimList = cd.DimDataList;
            int k = 0;
            foreach (CubeAttribute ca in attrList)
            {
                sql += "and " + ca.DimensionCode + "=" + dimList[k];
                k++;
            }
            DBDao.ExecuteNonQuery(CommandType.Text, null, sql);
            return true;
        }

        // <summary>
        /// 修改立方数据
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        public CubeData UpdateCubeDataById(CubeRegister cr, CubeData cd)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbTransaction tx = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            string sql = "";
            if (conn is SqlConnection)
            {
                sql = "update " + cr.CubeCode + " set RESULTVALUE =@ResultValue, PLANVALUE =@PlanValue, SONVALUE =@SonValue, MOTHERVALUE =@MotherValue where id =@Id";
            } else
            {
                sql = "update " + cr.CubeCode + " set RESULTVALUE =:ResultValue, PLANVALUE =:PlanValue, SONVALUE =:SonValue, MOTHERVALUE =:MotherValue where id =:Id";
            }
            command.CommandText = sql;
            IDbDataParameter p_resultValue = command.CreateParameter();
            p_resultValue.Value = cd.Result;
            p_resultValue.ParameterName = "ResultValue";
            command.Parameters.Add(p_resultValue);

            IDbDataParameter p_planValue = command.CreateParameter();
            p_planValue.Value = (cd.Plan == null) ? "" : cd.Plan;
            p_planValue.ParameterName = "PlanValue";
            command.Parameters.Add(p_planValue);

            IDbDataParameter p_sonValue = command.CreateParameter();
            p_sonValue.Value = cd.SonValue;
            p_sonValue.ParameterName = "SonValue";
            command.Parameters.Add(p_sonValue);

            IDbDataParameter p_motherValue = command.CreateParameter();
            p_motherValue.Value = cd.MotherValue;
            p_motherValue.ParameterName = "MotherValue";
            command.Parameters.Add(p_motherValue);

            IDbDataParameter p_id = command.CreateParameter();
            p_id.Value = cd.Id;
            p_id.ParameterName = "Id";
            command.Parameters.Add(p_id);

            try
            {
                command.ExecuteNonQuery();
                tx.Commit();
            } catch
            {
                tx.Rollback();
                throw;
            }
            return cd;
        }

        // <summary>
        /// 批量修改立方数据
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd_list">立方数据集合</param>
        //[TransManager]
        public void BatchUpdateCubeDatasById(CubeRegister cr, IList cd_list)
        {
            if (cr == null || cd_list == null || cd_list.Count == 0) return;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbTransaction tx = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            command.Transaction = tx;
            string sql = "begin ";
            string tableName = cr.CubeCode;
            foreach (CubeData cd in cd_list)
            {
                string planValue=(cd.Plan==null)?"":cd.Plan;
                string updateSql = "update " + tableName + " set ResultValue={0},PlanValue='{1}' where id='{2}';";
                updateSql = string.Format(updateSql, cd.Result, planValue, cd.Id);
                sql += updateSql;
            }
            sql += " end;";
            command.CommandText = sql;
            try
            {
                command.ExecuteNonQuery();
                tx.Commit();
            } catch
            {
                tx.Rollback();
                throw;
            }
        }

        // <summary>
        /// 批量新增立方数据
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd_list">立方数据集合</param>
        //[TransManager]
        public void BatchInsertCubeDatas(CubeRegister cr, IList cd_list)
        {
            if (cr == null || cd_list == null || cd_list.Count == 0) return;
            DataSet ds = GetFieldNameByCubeRegisterId(cr.Id);
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbTransaction tx = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            command.Transaction = tx;
            string sql = "begin ";
            string tableName = cr.CubeCode;
            string partSql = "insert into " + tableName+"(id";
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                string fieldStr = dr["DimensionCode"].ToString();
                partSql += "," + fieldStr;
            }
            //维度之后再加上事实
            partSql += ",FactId) values(";
            IFCGuidGenerator ifc=new IFCGuidGenerator();
            foreach (CubeData cd in cd_list)
            {
                string insertSql = partSql;
                string idValue = ifc.GeneratorIFCGuid();
                insertSql += "'" + idValue + "'";
                foreach (string fieldValue in cd.DimDataList)
                {
                    insertSql += ",'" + fieldValue + "'";
                }
                insertSql += ");";
                sql += insertSql;
            }
            sql += " end;";
            command.CommandText = sql;
            try
            {
                command.ExecuteNonQuery();
                tx.Commit();
            } catch
            {
                tx.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 根据维度注册ID查询列名
        /// </summary>
        /// <param name="cubeRegisterId"></param>
        /// <returns></returns>
        private DataSet GetFieldNameByCubeRegisterId(string cubeRegisterId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "";
            if (conn is SqlConnection)
            {
                sql = @"SELECT b.DimensionCode,b.DimensionName
                    FROM THD_ViewStyle a LEFT JOIN THD_CubeAttribute b ON a.CubeAttrId=b.id
                    WHERE b.CubeRegId=@CubeRegisterId ORDER BY a.RangeOrder";
            } else
            {
                sql = @"SELECT b.DimensionCode,b.DimensionName
                    FROM THD_ViewStyle a LEFT JOIN THD_CubeAttribute b ON a.CubeAttrId=b.id
                    WHERE b.CubeRegId=:CubeRegisterId ORDER BY a.RangeOrder";
            }
            command.CommandText = sql;
            IDbDataParameter p_cubeRegisterId = command.CreateParameter();
            p_cubeRegisterId.Value = cubeRegisterId;
            p_cubeRegisterId.ParameterName = "CubeRegisterId";
            command.Parameters.Add(p_cubeRegisterId);
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        // <summary>
        /// 修改立方数据,提供通过维度集进行修改
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        public bool UpdateCubeDataByDimensions(CubeRegister cr, CubeData cd)
        {
            string sql = " update " + cr.CubeCode + " set RESULTVALUE = " + cd.Result + ", PLANVALUE = '" + cd.Plan + "', SONVALUE = " + cd.SonValue + ", MOTHERVALUE = " + cd.MotherValue + " where  1=1 ";
            IList attrList = this.GetCubeAttrByCubeResgisterId(cr);
            IList dimList = cd.DimDataList;
            int k = 0;
            foreach (CubeAttribute ca in attrList)
            {
                sql += " and " + ca.DimensionCode + "=" + dimList[k];
                k++;
            }
            DBDao.ExecuteNonQuery(CommandType.Text, null, sql);
            return true;
        }

        // <summary>
        /// 通过ID查询立方数据中的一条记录
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        public CubeData GetCubeDataById(CubeRegister cr, CubeData cd)
        {
            string sql = " select RESULTVALUE,PLANVALUE,SONVALUE,MOTHERVALUE from " + cr.CubeCode + " where id = " + cd.Id;
            DataSet ds = DBDao.OpenQueryDataSet(sql, null);

            if (ds != null)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string value = dr["RESULTVALUE"].ToString();
                string plan = dr["PLANVALUE"].ToString();
                string sonValue = dr["SONVALUE"].ToString();
                string motherValue = dr["MOTHERVALUE"].ToString();
                if (value != null && !"".Equals(value))
                {
                    cd.Result = double.Parse(value);
                }
                if (plan != null && !"".Equals(plan))
                {
                    cd.Plan = plan;
                }
                if (sonValue != null && !"".Equals(sonValue))
                {
                    cd.SonValue = double.Parse(sonValue);
                }
                if (motherValue != null && !"".Equals(motherValue))
                {
                    cd.MotherValue = double.Parse(motherValue);
                }
            }
            return cd;
        }

        // <summary>
        /// 通过维度集合查询立方数据中的结果
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="cd">立方数据</param>
        public CubeData GetCubeDataByDimensions(CubeRegister cr, CubeData cd)
        {
            string sql = "select RESULTVALUE,PLANVALUE,SONVALUE,MOTHERVALUE from " + cr.CubeCode + " where 1=1 ";
            IList attrList = this.GetCubeAttrByCubeResgisterId(cr);
            IList dimList = cd.DimDataList;
            int k = 0;
            foreach (CubeAttribute ca in attrList)
            {
                sql += " and " + ca.DimensionCode + "=" + dimList[k];
                k++;
            }

            DataSet ds = DBDao.OpenQueryDataSet(sql, null);
            if (ds != null)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string value = dr["RESULTVALUE"].ToString();
                string plan = dr["PLANVALUE"].ToString();
                string sonValue = dr["SONVALUE"].ToString();
                string motherValue = dr["MOTHERVALUE"].ToString();
                if (value != null && !"".Equals(value))
                {
                    cd.Result = double.Parse(value);
                }
                if (plan != null && !"".Equals(plan))
                {
                    cd.Plan = plan;
                }
                if (sonValue != null && !"".Equals(sonValue))
                {
                    cd.SonValue = double.Parse(sonValue);
                }
                if (motherValue != null && !"".Equals(motherValue))
                {
                    cd.MotherValue = double.Parse(motherValue);
                }
            }
            return cd;
        }

        // <summary>
        /// 查询立方数据中的多条记录
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="elements">确定元素的多维组合的集合,为一个二维的集合,长度为立方的维度数</param>
        /// 例如:  new String[][] {new String[] { "a1", "a2" }, new String[] { "b1", "b2" }, new String[] { "c1" }, });
        /// return 2 * 2 * 1 = 4个结果. 对应coordinates("a1", "b1", "c1" ),("a1", "b2", "c1" ),("a2", "b1", "c1" ),("a2", "b2", "c1" )
        /// <returns>CubeData对象的集合</returns>
        public IList GetCubeDataList(CubeRegister cr, IList elements)
        {
            IList resultList = new ArrayList();//结果集
            
            //拼凑SQL,类似于select p1,p2,p3 from x where 1=1 and t1 in (,,,) and t2 in (,,,)
            string sql = " select ID";
            IList attrList = this.GetCubeAttrByCubeResgisterId(cr);
            foreach (CubeAttribute ca in attrList)
            {
                sql += "," + ca.DimensionCode;
            }
            sql += ",FactId,RESULTVALUE,PLANVALUE,SONVALUE,MOTHERVALUE from " + cr.CubeCode + " where 1=1 ";
            int k = 0;
            foreach (CubeAttribute ca in attrList)
            {
                sql += " and " + ca.DimensionCode + " in (";
                ArrayList temp = (ArrayList)elements[k];
                for (int i = 0; i < temp.Count; i++)
                {
                    if (i == 0)
                    {
                        sql += "'"+temp[i]+"'";
                    } else
                    {
                        sql += ",'" + temp[i]+"'";
                    }
                }
                sql += ")";
                k++;
            }
            //添加事实
            IList factList = elements[elements.Count - 1] as IList;
            sql+=" and FactId in ('1'";
            foreach (FactDefine fact in factList)
            {
                sql += ",'" + fact.Id + "'";
            }
            sql += ")";

            //查询数据
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CubeData cd = new CubeData();
                        ArrayList al = new ArrayList();
                        IList dimNameList = new ArrayList();//维度代码集合
                        foreach (CubeAttribute ca in attrList)
                        {
                            al.Add(dr[ca.DimensionCode].ToString());
                            dimNameList.Add(ca.DimensionCode);
                        }
                        //添加事实
                        al.Add(dr["FactId"].ToString());
                        cd.DimDataList = al;
                        cd.DimCodeList = dimNameList;
                        string value = dr["RESULTVALUE"].ToString();
                        string plan = dr["PLANVALUE"].ToString();
                        string sonValue = dr["SONVALUE"].ToString();
                        string motherValue = dr["MOTHERVALUE"].ToString();
                        string id = dr["ID"].ToString();
                        if (value != null && !"".Equals(value))
                        {
                            cd.Result = double.Parse(value);
                        }
                        if (plan != null && !"".Equals(plan))
                        {
                            cd.Plan = plan;
                        }
                        if (sonValue != null && !"".Equals(sonValue))
                        {
                            cd.SonValue = double.Parse(sonValue);
                        }
                        if (motherValue != null && !"".Equals(motherValue))
                        {
                            cd.MotherValue = double.Parse(motherValue);
                        }
                        if (id != null && !"".Equals(id))
                        {
                            cd.Id = id;
                        }
                        resultList.Add(cd);
                    }
                }

            }
            return resultList;
        }

        // <summary>
        /// 查询立方数据中的多条记录(以显示顺序返回记录)
        /// </summary>
        /// <param name="cr">立方注册对象</param>
        /// <param name="elements">确定元素的多维组合的集合,为一个二维的集合,长度为立方的维度数</param>
        /// 例如:  new String[][] {new String[] { "a1", "a2" }, new String[] { "b1", "b2" }, new String[] { "c1" }, });
        /// return 2 * 2 * 1 = 4个结果. 对应coordinates("a1", "b1", "c1" ),("a1", "b2", "c1" ),("a2", "b1", "c1" ),("a2", "b2", "c1" )
        /// <returns>CubeData对象的集合</returns>
        public IList GetCubeDataListByViewStyle(CubeRegister cr, IList elements, IList styleList)
        {
            IList resultList = new ArrayList();//结果集
            //拼凑SQL,类似于select p1,p2,p3 from x where 1=1 and t1 in (,,,) and t2 in (,,,)
            //string sql = " select ID ";
            //IList dimNameList = new ArrayList();//维度代码集合
            //IList attrList = this.GetCubeAttrByCubeResgisterId(cr);
            //IList attrStyleList = this.getCubetributeByStyleOrder(attrList, styleList);
            //string order = "";
            //foreach (CubeAttribute ca in attrStyleList)
            //{
            //    sql += " , " + ca.DimensionCode;
            //    if ("".Equals(order))
            //    {
            //        order += ca.DimensionCode;
            //    }
            //    else
            //    {
            //        order += "," + ca.DimensionCode;
            //    }
            //    dimNameList.Add(ca.DimensionCode);

            //}
            //sql += " ,RESULTVALUE,PLANVALUE,SONVALUE,MOTHERVALUE from " + cr.CubeCode + " where 1=1 ";
            //int k = 0;
            //foreach (CubeAttribute ca in attrList)
            //{
            //    sql += " and " + ca.DimensionCode + " in ( ";
            //    ArrayList temp = (ArrayList)elements[k];
            //    for (int i = 0; i < temp.Count; i++)
            //    {
            //        if (i == 0)
            //        {
            //            sql += temp[i];
            //        }
            //        else
            //        {
            //            sql += "," + temp[i];
            //        }
            //    }
            //    sql += " ) ";
            //    k++;
            //}
            //sql += " order by " + order + " asc ";

            ////查询数据
            //DataSet ds = DBDao.OpenQueryDataSet(sql, null);
            //if (ds != null)
            //{
            //    DataTable dt = ds.Tables[0];
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        CubeData cd = new CubeData();
            //        ArrayList al = new ArrayList();
            //        foreach (CubeAttribute ca in attrStyleList)
            //        {
            //            al.Add(dr[ca.DimensionCode].ToString());
            //        }
            //        cd.DimDataList = al;
            //        string value = dr["RESULTVALUE"].ToString();
            //        string plan = dr["PLANVALUE"].ToString();
            //        string sonValue = dr["SONVALUE"].ToString();
            //        string motherValue = dr["MOTHERVALUE"].ToString();
            //        string id = dr["ID"].ToString();

            //        if (value != null && !"".Equals(value))
            //        {
            //            cd.Result = double.Parse(value);
            //        }
            //        if (plan != null && !"".Equals(plan))
            //        {
            //            cd.Plan = plan;
            //        }
            //        if (id != null && !"".Equals(id))
            //        {
            //            cd.Id = long.Parse(id);
            //        }
            //        if (sonValue != null && !"".Equals(sonValue))
            //        {
            //            cd.SonValue = double.Parse(sonValue);
            //        }
            //        if (motherValue != null && !"".Equals(motherValue))
            //        {
            //            cd.MotherValue = double.Parse(motherValue);
            //        }
            //        cd.DimCodeList = dimNameList;
            //        resultList.Add(cd);
            //    }

            //}
            return resultList;
        }

        //得到查询的维度值的集合 类似于{[ "a1", "a2" ],[ "b1", "b2" ],  [ "c1" ]}
        public IList GetQueryElements(IList caList, IList styleList)
        {
            IList elements = new ArrayList();//维度元素的二维集合
            Hashtable ht_temp = new Hashtable();//暂时存储维度元素
            //生成表格行
            if (styleList != null && styleList.Count > 0)
            {
                foreach (ViewStyle vs in styleList)
                {
                    ArrayList dim_list = new ArrayList();//维度值的集合
                    foreach (ViewStyleDimension vsd in vs.Details)
                    {
                        dim_list.Add(vsd.DimCatId + "");
                    }
                    ht_temp.Add(vs.OldCatRootId + "", dim_list);
                }
                //和立方属性的维度保持顺序一致
                foreach (CubeAttribute ca in caList)
                {
                    ArrayList al = (ArrayList)ht_temp[ca.DimensionId + ""];
                    elements.Add(al);
                }
            }
            return elements;
        }

        //重新按照显示顺序排列立方属性
        private IList getCubetributeByStyleOrder(IList cubeAttr_list, IList viewStyle_list)
        {
            IList newCubeAttr_list = new ArrayList();
            foreach (ViewStyle vs in viewStyle_list)
            {
                foreach (CubeAttribute ca in cubeAttr_list)
                {
                    if (vs.OldCatRootId.Equals(ca.DimensionId))
                    {
                        newCubeAttr_list.Add(ca);
                    }
                }
            }
            return newCubeAttr_list;
        }

        // <summary>
        /// 同立方属性模式的维度值集合转换到显示模式的维度值集合
        /// </summary>
        /// <param name="attrOrder">立方属性维度集合</param>
        /// <param name="styleOrder">显示模式维度集合</param>
        /// <param name="sourceList">立方属性排列的数据</param>
        public IList transOrderToDisplay(IList attrOrder, IList styleOrder, IList sourceList)
        {
            Hashtable ht_order = new Hashtable();
            int k = 0;
            foreach (ViewStyle vs in styleOrder)
            {
                int l = 0;
                int t = 0;
                foreach (CubeAttribute ca in attrOrder)
                {
                    if (vs.OldCatRootId.Equals(ca.DimensionId))
                    {
                        t = l;
                    }
                    l++;
                }
                ht_order.Add(k + "", t + "");
                k++;
            }
            IList tarList = new ArrayList();
            for (int i = 0; i < sourceList.Count; i++)
            {
                tarList.Add(sourceList[Int16.Parse((string)(ht_order[i + ""]))]);
            }
            ht_order.Clear();
            return tarList;
        }


        // <summary>
        /// 同显示模式模式的维度值集合转换到立方属性的维度值集合
        /// </summary>
        /// <param name="attrOrder">显示模式维度集合</param>
        /// <param name="styleOrder">立方属性维度集合</param>
        /// <param name="sourceList">显示模式排列的数据</param>
        public IList transDisplayToOrder(IList styleOrder, IList attrOrder, IList sourceList)
        {
            Hashtable ht_order = new Hashtable();
            int k = 0;
            foreach (CubeAttribute ca in attrOrder)
            {
                int l = 0;
                int t = 0;
                foreach (ViewStyle vs in styleOrder)
                {
                    if (ca.DimensionId.Equals(vs.OldCatRootId))
                    {
                        t = l;
                    }
                    l++;
                }
                ht_order.Add(k + "", t + "");
                k++;
            }
            IList tarList = new ArrayList();
            //由于sourceList中含有"事实"，且是放在sourceList的最后，为了排序，这里做了下处理。
            for (int i = 0; i < sourceList.Count-1; i++)
            {
                tarList.Add(sourceList[Int16.Parse((string)(ht_order[i + ""]))]);
            }
            tarList.Add(sourceList[sourceList.Count - 1]);
            return tarList;
        }

        // <summary>
        /// 取得当前CubeData的分数
        /// </summary>
        /// <param name="cd">CubeData对象</param>
        /// <returns>分值</returns>
        public double getSocreByInput(CubeData cd)
        {
            ObjectQuery oq = new ObjectQuery();
            IList list_all_sysdim = Dao.ObjectQuery(typeof(DimensionRegister), oq);//此部分有待优化

            //获取当前立方数据对象中的有度量属性的维度ID
            IList dimNameList = cd.DimCodeList;//主维度集合
            int k = 0;
            bool ifBreak = false;
            foreach (string dimName in dimNameList)
            {
                foreach (DimensionRegister dr in list_all_sysdim)
                {
                    if (dr.Code.Equals(dimName) && dr.IfMeasure == 1)
                    {
                        ifBreak = true;
                        break;
                    }
                }
                if (ifBreak == true)
                {
                    break;
                }
                k++;
            }
            IList datalist = cd.DimDataList;
            long mensureDimId = long.Parse(datalist[k].ToString());
            return this.getScoreByDimension(mensureDimId, cd.Result);
        }

        // <summary>
        /// 取得输入值的得分
        /// </summary>
        /// <param name="dimensionId">维度ID</param>
        /// <param name="input">输入值</param>
        /// <returns>分值</returns>
        public double getScoreByDimension(long dimensionId, double input)
        {
            double return_score = -1;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Category.Id", dimensionId));
            IList list = Dao.ObjectQuery(typeof(DimensionScope), oq);
            foreach (DimensionScope ds in list)
            {
                return_score = this.calScore(ds.ScopeType, ds.BeginValue, ds.EndValue, ds.Score, input);
                if (return_score != -1)
                {
                    break;
                }
            }
            return return_score;
        }

        // <summary>
        /// 得分算法
        /// </summary>
        /// <param name="calType">计算类型</param>
        /// <param name="beginValue">开始值</param>
        /// <param name="endValue">结束值</param>
        /// <param name="score">分值</param>
        /// <param name="input">输入值</param>
        /// <returns>分值</returns>
        private double calScore(int calType, double beginValue, double endValue, double score, double input)
        {
            double return_score = -1;
            switch (calType)
            {
                //开区间()
                case 1:
                    if (beginValue < input && input < endValue)
                    {
                        return_score = score;
                    }
                    break;
                //闭区间[]
                case 2:
                    if (beginValue <= input && input <= endValue)
                    {
                        return_score = score;
                    }
                    break;
                //左开右闭(]
                case 3:
                    if (beginValue < input && input <= endValue)
                    {
                        return_score = score;
                    }
                    break;
                //左闭右开[)
                case 4:
                    if (beginValue <= input && input < endValue)
                    {
                        return_score = score;
                    }
                    break;
            }

            return return_score;
        }


        // <summary>
        /// 判断传入的立方数据和立方注册字段的合法性
        /// 1：字段和结果是否对应
        /// </summary>
        /// <returns></returns>
        private bool CheckCubeValid(CubeRegister cr, CubeData obj)
        {

            return true;
        }

        /// <summary>
        /// 通过查询SQL获取单个值
        /// <returns></returns>
        public string GetResultBySql(string sql)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            command.CommandText = sql;
            object ret = command.ExecuteReader();
            if (ret is DBNull || ret == null || ret.Equals(""))
            {
                return "";
            } else
            {
                return ret.ToString();
            }
        }

        /// <summary>
        /// 查询分发序号
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public string GetMaxDistributeSerialByViewId(string viewId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "";
            if (conn is SqlConnection)
            {
                sql = "select max(distributeSerial) from thd_viewdistribute where viewMainId =@ViewId";
            } else
            {
                sql = "select max(distributeSerial) from thd_viewdistribute where viewMainId =:ViewId";
            }
            IDbDataParameter p_viewId = command.CreateParameter();
            p_viewId.Value = viewId;
            p_viewId.ParameterName = "ViewId";
            command.Parameters.Add(p_viewId);

            command.CommandText = sql;
            object ret = command.ExecuteScalar();
            if (ret is DBNull || ret == null || ret.Equals(""))
            {
                return "";
            } else
            {
                return ret.ToString();
            }
        }

        /*
        // <summary>
        /// 浏览数据时的汇总算法
        /// </summary>
        /// <param name="obj">立方体对象</param>
        /// <param name="type">汇总类型(1:累计相加;2:累计平均)</param>
        /// <returns>返回的立方体的数据集合</returns>
        public double CalCubeData(CubeData obj,int type)
        {
            double result = 0;
            IList dataList = obj.DimDataList;//维度值集合
            IList codeList = obj.DimCodeList;//维度代码集合
            int measureIndex = obj.MensureIndex;

            return result;
        }*/

        #endregion

        #region 文件操作
        /// <summary>
        /// 判断指定文件是否存在技术管理系统服务器的目录下
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IfExistFileInKnowlodgeServer(string fileName)
        {
            return TransUtil.IfExistFileInServer(fileName);
        }

        /// <summary>
        /// 判断指定文件是否存在服务器的目录下
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IfExistFileInServer(string fileName)
        {
            return TransUtil.IfExistFileInServer(fileName);
        }

        /// <summary>
        /// 判断指定文件是否存在服务器
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IfExistFiles(string filePath)
        {
            return TransUtil.IfExistFiles(filePath);
        }

        /// <summary>
        /// 把模板保存文件到服务器上
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void SaveFileToServer(string fileName, byte[] source)
        {
            TransUtil.SaveFileToServer(fileName, source);
        }

        /// <summary>
        /// 把结果保存文件到服务器上
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void SaveFileToServerByResult(string fileName, byte[] source)
        {
            TransUtil.SaveFileToServerByResult(fileName, source);
        }

        /// <summary>
        /// 从服务器上删除文件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void DeleteFileFromServer(string fileName)
        {
            TransUtil.DeleteFileFromServer(fileName);
        }

        /// <summary>
        /// 修改服务器的目录下的文件名
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void ChangeFileName(string oldFileName, string newFileName)
        {
            TransUtil.ChangeFileName(oldFileName, newFileName);
        }

        #endregion


        #region 事实定义方法
        /// <summary>
        /// 查询事实定义
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetFactDefine(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(FactDefine), oq);
        }

        /// <summary>
        /// 保存事实定义
        /// </summary>
        /// <param name="factDefineList"></param>
        /// <returns></returns>
        [TransManager]
        public IList SaveFactDefine(IList factDefineList)
        {
            Dao.SaveOrUpdate(factDefineList);
            return factDefineList;
        }

        /// <summary>
        /// 通过主题查询事实
        /// </summary>
        /// <param name="cubeRegisterId"></param>
        /// <returns></returns>
        public IList GetFactDefineByCubeRegisterId(string cubeRegisterId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("CubeRegister.Id", cubeRegisterId));
            return GetFactDefine(oq);
        }
        #endregion
    }
}
