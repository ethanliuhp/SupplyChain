using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Oracle.DataAccess.Client;
using NHibernate;

using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using System.Data;
using NHibernate.Criterion;
using System.Runtime.Remoting.Messaging;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service
{
    public class ViewService : Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service.IViewService
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

        #endregion

        #region 视图定义层

        /// <summary>
        /// 查询注册立方的视图集合
        /// </summary>
        /// <returns></returns>
        public IList GetViewMainByCubeId(CubeRegister obj) 
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("CubeRegId.Id",obj.Id));
            oq.AddFetchMode("ViewStyles",NHibernate.FetchMode.Eager);
            IList retValue=Dao.ObjectQuery(typeof(ViewMain), oq);
            foreach (ViewMain vm in retValue)
            {
                foreach (ViewStyle vs in vm.ViewStyles)
                {
                    dao.InitializeObj(vs.Details);
                }
            }
            return retValue;
        }

        /// <summary>
        /// 通过主题和视图类型查询视图集合
        /// </summary>
        /// <returns></returns>
        public IList GetViewMainByCubeIdAndType(CubeRegister obj,string type)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("CubeRegId.Id", obj.Id));
            if ("3".Equals(type) || "4".Equals(type) || "5".Equals(type))
            {
                oq.AddCriterion(Expression.Eq("ViewTypeCode", type));
            }
            else {
                ICriterion ex_1 = Expression.Eq("ViewTypeCode", "1");
                ICriterion ex_2 = Expression.Eq("ViewTypeCode", "2");
                oq.AddCriterion(Expression.Or(ex_1, ex_2));
            }
            oq.AddOrder(Order.Asc("ViewName"));
            oq.AddFetchMode("ViewStyles", NHibernate.FetchMode.Eager);
            IList retValue = Dao.ObjectQuery(typeof(ViewMain), oq);
            foreach (ViewMain vm in retValue)
            {
                foreach (ViewStyle vs in vm.ViewStyles)
                {
                    dao.InitializeObj(vs.Details);
                }
            }
            return retValue;
        }

        /// <summary>
        /// 通过条件查询视图集合
        /// </summary>
        /// <returns></returns>
        public IList GetViewMainByQuery(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(ViewMain), oq);
        }

        
        /// <summary>
        /// 新增/修改视图主表
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public ViewMain SaveViewMain(ViewMain obj) 
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 删除视图主表
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewMain(ViewMain obj) 
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 通过视图ID查询视图信息
        /// </summary>
        /// <returns></returns>
        public ViewMain GetViewMainById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list= Dao.ObjectQuery(typeof(ViewMain), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ViewMain;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过视图名称查询视图信息
        /// </summary>
        /// <returns></returns>
        public ViewMain GetViewMainByName(string name) 
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ViewName", name));
            IList list = Dao.ObjectQuery(typeof(ViewMain), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ViewMain;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 新增/修改视图录入信息
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public ViewWriteInfo SaveViewWriteInfo(ViewWriteInfo obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 删除视图视图录入信息
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewWriteInfo(ViewWriteInfo obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 根据条件查找视图录入信息
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为视图录入信息的集合</returns>
        public IList GetViewWriteInfoByQuery(ObjectQuery oq)
        {
            return dao.ObjectQuery(typeof(ViewWriteInfo), oq);
        }

        /// <summary>
        /// 批量新增/修改视图规则定义
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public void SaveViewRuleDefs(IList objs)
        {
            foreach (ViewRuleDef obj in objs)
            {
                Dao.SaveOrUpdate(obj);
            }
        }

        // <summary>
        /// 修改规则定义数据
        /// </summary>
        /// <param name="cd"></param>
        [TransManager]
        public ViewRuleDef UpdateViewRuleDefById(ViewRuleDef obj)
        {
            string sql = "";
            if (!"".Equals(obj.CalExpress) && obj.CalExpress != null)
            {
                sql = " update KNVIEWRULEDEF set CELLSIGN = '" + obj.CellSign + "', CALEXPRESS = '" + obj.CalExpress + "' where id = " + obj.Id;
            }
            else if (!"".Equals(obj.DisplayRule) && obj.DisplayRule != null)
            {
                sql = " update KNVIEWRULEDEF set CELLSIGN = '" + obj.CellSign + "', DISPLAYRULE = '" + obj.DisplayRule + "' where id = " + obj.Id;
            }
            else {
                sql = " update KNVIEWRULEDEF set CELLSIGN = '" + obj.CellSign + "', CALEXPRESS = '', DISPLAYRULE = '' where id = " + obj.Id;
            }

            if (!"".Equals(sql))
            {
                DBDao.ExecuteNonQuery(CommandType.Text, null, sql);
            }
            return obj;
        }

        // <summary>
        /// 批量修改规则定义数据
        /// </summary>
        /// <param name="cd"></param>
        public void BatchUpdateViewRuleDefById(IList list)
        {
            if (list == null || list.Count == 0) return;
            string sql_str = "";
            ISession session = CallContext.GetData("nhsession") as ISession;            
            IDbConnection conn = session.Connection;
            IDbTransaction tx = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            command.Transaction = tx;

            foreach (ViewRuleDef obj in list)
            {
                if (!"".Equals(obj.CalExpress) && obj.CalExpress != null)
                {
                    sql_str += "update thd_VIEWRULEDEF set CELLSIGN = '" + obj.CellSign + "', CALEXPRESS = '" + obj.CalExpress + "' where id = '" + obj.Id + "';";
                }
                else if (!"".Equals(obj.DisplayRule) && obj.DisplayRule != null)
                {
                    sql_str += "update thd_VIEWRULEDEF set CELLSIGN = '" + obj.CellSign + "', DISPLAYRULE = '" + obj.DisplayRule + "' where id = '" + obj.Id + "';";
                }
                else
                {
                    sql_str += "update thd_VIEWRULEDEF set CELLSIGN = '" + obj.CellSign + "', CALEXPRESS = '', DISPLAYRULE = '' where id = '" + obj.Id + "';";
                }
            }

            string sql = "begin " + sql_str +"  end;";
            command.CommandText = sql;
            try
            {
                command.ExecuteNonQuery();
                tx.Commit();
            }catch
            {
                tx.Rollback();
                throw;
            }
        }

        // <summary>
        /// 通过存储过程批量修改视图规则定义
        /// </summary>
        /// <param name="type">类型(1:新增,2:修改)</param>
        /// <param name="cd_list">视图规则定义集合</param>
        //[TransManager]
        public void BatchUpdateRuleDefByProc(int type,IList rule_list)
        {
            if (rule_list == null || rule_list.Count == 0) return;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbTransaction tx = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            command.Transaction = tx;
            string sql="begin ";
            VirtualMachine.Component.Util.IFCGuidGenerator ifc=new VirtualMachine.Component.Util.IFCGuidGenerator();
            foreach (ViewRuleDef vrd in rule_list)
            { 
                string guid=ifc.GeneratorIFCGuid();
                string cellSign = string.IsNullOrEmpty(vrd.CellSign) ? "" : vrd.CellSign;
                string calExpress = string.IsNullOrEmpty(vrd.CalExpress) ? "" : vrd.CalExpress;
                string saveExpress = string.IsNullOrEmpty(vrd.SaveExpress) ? "" : vrd.SaveExpress;
                if (type == 1)
                {
                    sql += @"insert into thd_viewruledef (id,version,viewmainid,cellsign,calexpress,saveexpress) 
               values ('" + guid + "',0,'" + vrd.Main.Id + "','" + cellSign + "','" + calExpress + "','" + saveExpress + "');";
                } else
                {
                    string tempStr = string.Format("update thd_viewruledef set viewmainid='{0}',cellsign='{1}',calexpress='{2}',saveexpress='{3}' where id='{4}';", vrd.Main.Id, cellSign, calExpress, saveExpress,vrd.Id);
                    sql += tempStr;
                }
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
        /// 新增/修改视图规则定义
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public ViewRuleDef SaveViewRuleDef(ViewRuleDef obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 批量删除视图规则定义
        /// </summary>
        /// <returns></returns>
        public void DeleteViewRuleDefs(IList objs)
        {
            if (objs == null || objs.Count == 0) return;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbTransaction tx = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            command.Transaction = tx;
            string id_link = "(";
            foreach (string str in objs)
            {
                if ("(".Equals(id_link))
                {
                    id_link += "'"+str+"'";
                }
                else {
                    id_link += "," + str;
                }
            }
            id_link += ")";
            string sql = "delete from THD_VIEWRULEDEF where ID in " + id_link;
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
        /// 删除视图规则定义
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewRuleDef(ViewRuleDef obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 通过视图主表查询视图规则定义明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetViewRuleDefByViewMain(ViewMain obj)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Main.Id", obj.Id));
            oq.AddOrder(Order.Asc("Id"));
            return Dao.ObjectQuery(typeof(ViewRuleDef), oq);
        }

        /// <summary>
        /// 新增/修改视图明细
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public ViewDetail SaveViewDetail(ViewDetail obj) 
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 删除视图明细
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewDetail(ViewDetail obj) 
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 批量删除视图明细
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewDetail(IList objs) 
        {
            return Dao.Delete(objs);
        }

        /// <summary>
        /// 通过视图明细ID查询视图明细信息
        /// </summary>
        /// <returns></returns>
        public ViewDetail GetViewDetailById(string id) 
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(ViewDetail), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ViewDetail;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过视图主表查询视图明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetViewDetailByViewMain(ViewMain obj)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Main.Id", obj.Id));
            return Dao.ObjectQuery(typeof(ViewDetail), oq);
        }

        /// <summary>
        /// 通过视图主表查询视图格式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ordered">是否按指定顺序排序 true排序，false不排序</param>
        /// <returns></returns>
        public IList GetViewStyleByViewMain(ViewMain obj,bool ordered)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Main.Id", obj.Id));
            oq.AddFetchMode("Details", FetchMode.Eager);
            if (ordered)
            {
                oq.AddOrder(Order.Asc("Direction"));//先列、行
                oq.AddOrder(Order.Asc("RangeOrder"));//再顺序
            }
            else
            {
                oq.AddOrder(Order.Asc("RangeOrder"));
            }
            return Dao.ObjectQuery(typeof(ViewStyle), oq);
        }

        /// <summary>
        /// 通过视图主表查询视图分发明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IList GetViewDistributeByViewMain(ViewMain obj)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Main.Id", obj.Id));
            return Dao.ObjectQuery(typeof(ViewDistribute), oq);
        }

        /// <summary>
        /// 删除视图格式表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewStyle(ViewStyle obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 删除视图格式表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewStyle(IList objs)
        {
            return Dao.Delete(objs);
        }

        /// <summary>
        /// 删除视图格式维度表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewStyleDim(ViewStyleDimension obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 删除视图格式维度表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewStyleDim(IList objs)
        {
            return Dao.Delete(objs);
        }

        /// <summary>
        /// 通过视图格式表ID查询视图格式维度表
        /// </summary>
        /// <param name="viewStyleId"></param>
        /// <returns></returns>
        public IList GetViewStyleDimByVS(string viewStyleId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ViewStyleId.Id", viewStyleId));
            oq.AddOrder(Order.Asc("Id"));
            return Dao.ObjectQuery(typeof(ViewStyleDimension), oq);
        }

        /// <summary>
        /// 通过视图格式表ID和维度名称查询视图格式维度表
        /// </summary>
        /// <param name="viewStyleId"></param>
        /// <param name="catName"></param>
        /// <returns></returns>
        public IList GetViewStyleDimByVS(string viewStyleId,string catName)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ViewStyleId.Id", viewStyleId));
            oq.AddCriterion(Expression.Eq("Name", catName));
            return Dao.ObjectQuery(typeof(ViewStyleDimension), oq);
        }
        
        #endregion

        #region 视图分发层

        /// <summary>
        /// 新增/修改视图分发
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public ViewDistribute SaveViewDistribute(ViewDistribute obj) {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        [TransManager]
        public void SaveViewDistribute(IList lst)
        {
            string distributeSerial = GetMaxCode();
            foreach (ViewDistribute vd in lst)
            {
                vd.DistributeSerial = distributeSerial;
                SaveViewDistribute(vd);
            }
        }

        /// <summary>
        /// 删除视图分发
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool DeleteViewDistribute(ViewDistribute obj) {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 根据条件查找分发的视图
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为视图的集合</returns>
        public IList GetViewDistributeByQuery(ObjectQuery oq) {
            return dao.ObjectQuery(typeof(ViewDistribute), oq);
        }

        /// <summary>
        /// 根据ID查找分发的视图
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public ViewDistribute GetViewDistributeById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(ViewDistribute), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ViewDistribute;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取得最大的分发流水号 规则：四位年+两位月+两位日+三位流水
        /// </summary>
        private string GetMaxCode()
        {
            ObjectQuery oq = new ObjectQuery();
            string minId = DateTime.Today.ToString("yyyyMMdd") + "001";
            string maxId = DateTime.Today.ToString("yyyyMMdd") + "999";
            oq.AddCriterion(Expression.Ge("DistributeSerial", minId));
            oq.AddCriterion(Expression.Le("DistributeSerial", maxId));
            string maxCode = (string)Dao.Max1(typeof(ViewDistribute), "DistributeSerial", oq);
            if (string.IsNullOrEmpty(maxCode))
                return minId;
            else
                return (long.Parse(maxCode)+1).ToString();
        }

        #endregion
 
    }
}
