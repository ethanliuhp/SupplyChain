using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain;
using System.Collections;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core.DataAccess;
using NHibernate.Criterion;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Service
{
    public class DimensionDefineService : IDimensionDefineService
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
        /// 直接数据库操作
        /// </summary>
        public IDataAccess DBDao
        {
            get { return dBDao; }
            set { dBDao = value; }
        }

        private ICategoryNodeService nodeSrv;
        public ICategoryNodeService NodeSrv
        {
            get { return nodeSrv; }
            set { nodeSrv = value; }
        }

        private ICategoryTreeService treeService;
        public ICategoryTreeService TreeService
        {
            get { return treeService; }
            set { treeService = value; }
        }

        #endregion

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(object obj)
        {
            return Dao.Delete(obj);
        }

        #region 系统注册操作

        /// <summary>
        /// 通过系统名称查询系统代码
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        public SystemRegister GetSysteCodeByName(String systemName)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("SystemName", systemName));
            IList list = Dao.ObjectQuery(typeof(SystemRegister), oq);
            if (list == null || list.Count == 0) return null;
            return list[0] as SystemRegister;
        }

        #endregion

        #region 维度分类操作

        /// <summary>
        /// 保存维度分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public DimensionCategory SaveDimensionCategory(DimensionCategory obj)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                obj.CreateDate = DateTime.Today;
                NodeSrv.AddChildNode(obj);
            }
            else
            {
                NodeSrv.UpdateCategoryNode(obj);
            }
            return obj;
        }

        /// <summary>
        /// 保存维度分类集合
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        [TransManager]
        public IList SaveDimensionCategorys(IList lst)
        {
            IList list = new ArrayList();
            foreach (DimensionCategory var in lst)
            {
                list.Add(SaveDimensionCategory(var));
            }
            return list;
        }

        /// <summary>`
        /// 删除维度分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteDimensionCategory(DimensionCategory obj)
        {
            return NodeSrv.DeleteCategoryNode(obj);
        }

        // <summary>
        /// 修改维度值的节点类型
        /// </summary>
        /// <param name="dimId">维度值ID</param>
        /// <param name="nodetype">节点类型</param>
        [TransManager]
        public bool UpdateDimNodeType(string dimId, int nodetype)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbCommand command = session.Connection.CreateCommand();
            string sql = "";
            if (session.Connection is SqlConnection)
            {
                sql ="update thd_dimensioncategory set nodetype =@NodeType where catid =@CatId";
            } else
            {
                sql ="update thd_dimensioncategory set nodetype =:NodeType where catid =:CatId";
            }
            command.CommandText = sql;
            IDbDataParameter p_nodeType = command.CreateParameter();
            p_nodeType.Value = nodetype;
            p_nodeType.ParameterName = "NodeType";
            command.Parameters.Add(p_nodeType);

            IDbDataParameter p_catId = command.CreateParameter();
            p_catId.Value = dimId;
            p_catId.ParameterName = "CatId";
            command.Parameters.Add(p_catId);

            command.ExecuteNonQuery();
            return true;
        }

        /// <summary>
        /// 创建树
        /// </summary>
        /// <param name="treeName">树名称</param>
        /// <param name="treeType">树类型</param>
        /// <returns></returns>
        [TransManager]
        private CategoryTree InitTree(string treeName, Type treeType)
        {
            CategoryTree tree = TreeService.GetCategoryTreeByType(treeType);
            if (tree != null)
                return tree;

            BusinessOperators op = Dao.Get(typeof(BusinessOperators), VirtualMachine.SystemAspect.Security.SecurityUtil.GetLogOperId()) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = DateTime.Now;
            TreeService.SaveCategoryTree(tree);
            Dao.SaveOrUpdate(tree);

            DimensionCategory root = new DimensionCategory();
            root.Name = treeName;
            root.TheTree = tree;
            root.Code = treeName;
            root.CreateDate = DateTime.Today;
            NodeSrv.AddRoot(root);

            tree.RootId = root.Id;
            TreeService.SaveCategoryTree(tree);
            return tree;
        }

        /// <summary>
        /// 获取维度分类节点集合
        /// </summary>
        /// <returns></returns>
        public IList GetDimensionCategorys()
        {
            this.InitTree("维度分类树", typeof(DimensionCategory));
            ObjectQuery oq = new ObjectQuery();
            oq.AddOrder(Order.Asc("SysCode"));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(DimensionCategory), oq);
            return list;
        }

        /// <summary>
        /// 通过维度CODE和系统登录ID查询维度ID
        /// </summary>
        /// <returns></returns>
        public long GetDimensionIdByCode(string code,string registerId)
        {
            long catid = -1;
            string sql = " select CATID from kndimensioncategory where code = '" + code + "' and dimregid = " + registerId;
            DataSet ds = DBDao.OpenQueryDataSet(sql, null);

            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    catid = long.Parse(dr["CATID"].ToString());
                    break;
                }

            }
            return catid;
        }

        /// <summary>
        /// 获取所有维度的ID和名称的对应
        /// </summary>
        /// <returns></returns>
        public Hashtable GetDimensionCategorysBySql()
        {
            Hashtable ht = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "select ID,CATNAME from thd_dimensioncategory";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                string id = dr.GetString(0);
                string catName = dr.GetString(1);
                ht.Add(id, catName);
            }
            return ht;
        }

        /// <summary>
        /// 获取登录子系统的维度分类节点集合
        /// </summary>
        /// <param name="systemId">分系统ID</param>
        /// <returns></returns>
        public IList GetPartDimensionCategorys(string systemId)
        {
            IList dim_list = this.GetDimensionRegisterByRights(systemId);
            this.InitTree("维度分类树", typeof(DimensionCategory));
            Disjunction dis = Expression.Disjunction();
            ObjectQuery oq = new ObjectQuery();
            dis.Add(Expression.Eq("CategoryNodeType", NodeType.RootNode));
            //ICriterion exp2 = Expression.isEq("SysCode", "100.");
            foreach (DimensionRegister dr in dim_list)
            {   
                dis.Add(Expression.Like("SysCode", "%." + dr.Id + ".%"));
            }
            oq.AddCriterion(dis);
            oq.AddOrder(Order.Asc("SysCode"));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(DimensionCategory), oq);
            return list;
        }

        /// <summary>
        /// 获取某个节点的所有子节点
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public IList GetAllChildNodes(CategoryNode cat)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode","%"+cat.Id+".%"));
            oq.AddOrder(Order.Asc("CategoryNodeType"));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(DimensionCategory), oq);
            if (list != null && list.Count > 0)
            {
                list.RemoveAt(0);
            }
            return list;
            //return NodeSrv.GetALLChildNodes(cat);
        }

        /// <summary>
        /// 根据分类名称查找所有子节点
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        public IList GetAllChildNodesByName(String catName)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name",catName));
            IList cats= NodeSrv.GetNodesByObjectQuery(typeof(DimensionCategory), oq);
            if (cats == null || cats.Count == 0) return null;

            return NodeSrv.GetALLChildNodes((DimensionCategory)cats[0]);
        }

        /// <summary>
        /// 通过维度分类的SysCode查找所有子节点,指标采集专用
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        public IList GetAllChildNodesBySysCodeByDBDao(String sysCode)
        {
            IList childList = new ArrayList();
            string sql = " select CATID from kndimensioncategory where syscode like '" + sysCode + "%'";
            DataSet ds = DBDao.OpenQueryDataSet(sql, null);

            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    DimensionCategory dc = new DimensionCategory();
                    long id = long.Parse(dr["CATID"].ToString());
                    dc.Id = id + "";
                    childList.Add(dc);
                }              

            }
            return childList;
        }

        /// <summary>
        /// 通过维度分类的SysCode查找所有子节点
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        public IList GetAllChildNodesBySysCode(String sysCode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("SysCode", sysCode));
            IList cats = NodeSrv.GetNodesByObjectQuery(typeof(DimensionCategory), oq);
            if (cats == null || cats.Count == 0) return null;

            return NodeSrv.GetALLChildNodes((DimensionCategory)cats[0]);
        }

        /// <summary>
        /// 根据ID查找维度分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DimensionCategory GetDimensionCategoryById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id",id));
            IList list=Dao.ObjectQuery(typeof(DimensionCategory), oq);
            if (list == null || list.Count == 0) return null;
            return list[0] as DimensionCategory;
        }

        /// <summary>
        /// 根据条件查找维度分类
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为维度分类的集合</returns>
        public IList GetDimensionCategoryByQuery(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(DimensionCategory), oq);
        }

        #endregion

        #region 维度操作
        /// <summary>
        /// 保存维度
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public DimensionDefine SaveDimensionDefine(DimensionDefine obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 根据ID查找维度
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DimensionDefine GetDimensionDefineById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(DimensionDefine), oq);
            if (list == null || list.Count == 0) return null;
            return list[0] as DimensionDefine;
        }

        /// <summary>
        /// 根据维度分类节点查找维度值
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IList GetDimensionDefineByCategory(DimensionCategory category)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Category.Id", category.Id));
            oq.AddFetchMode("Category", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(DimensionDefine), oq);
        }

        /// <summary>
        /// 根据条件查找维度
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为维度值的集合</returns>
        public IList GetDimensionDefineByQuery(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(DimensionDefine), oq);
        }

        #endregion

        #region 维度注册表操作

        /// <summary>
        /// 根据ID查找维度注册表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DimensionRegister GetDimensionRegisterById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(DimensionRegister), oq);
            if (list == null || list.Count == 0) return null;
            return list[0] as DimensionRegister;
        }

        /// <summary>
        /// 根据维度注册编码查询维度注册表对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DimensionRegister GetDimensionRegisterByCode(string code)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Code", code));
            IList list = Dao.ObjectQuery(typeof(DimensionRegister), oq);
            if (list == null || list.Count == 0) return null;
            return list[0] as DimensionRegister;
        }

        /// <summary>
        /// 通过分系统权限查找维度注册表
        /// </summary>
        /// <param name="rights">分系统的ID</param>
        /// <returns></returns>
        public IList GetDimensionRegisterByRights(string rights)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("DimRights", "&"+rights));
            IList list = Dao.ObjectQuery(typeof(DimensionRegister), oq);
            return list;
        }

        /// <summary>
        /// 求所有己注册的维度
        /// </summary>
        /// <returns></returns>
        public IList GetAllDimensionRegister()
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = Dao.ObjectQuery(typeof(DimensionRegister), oq);
            return list;
        }

        #endregion

        #region 维度评分区间定义
        /// <summary>
        /// 保存纬度区间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public DimensionScope SaveDimensionScope(DimensionScope obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 删除立方注册信息
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool DeleteDimensionScope(DimensionScope obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 根据ID查找评分区间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DimensionScope GetDimensionScopeById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(DimensionScope), oq);
            if (list == null || list.Count == 0) return null;
            return list[0] as DimensionScope;
        }

        /// <summary>
        /// 根据条件查找维度评分区间
        /// </summary>
        /// <param name="oq"></param>
        /// <returns>IList为维度评分区间的集合</returns>
        public IList GetDimensionScopeByQuery(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(DimensionScope), oq);
        }

        #endregion
    }
}
