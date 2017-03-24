using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using System.Collections;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using NHibernate.Exceptions;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.StockManage.Base.Service
{
    public class StationCategorySrv : Application.Business.Erp.SupplyChain.StockManage.Base.Service.IStationCategorySrv
    {
        private IDao dao;
        virtual public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }
        #region 注入服务
        private ICategoryNodeService nodeSrv;
        public ICategoryNodeService NodeSrv
        {
            get { return nodeSrv; }
            set { nodeSrv = value; }
        }

        private ICategoryRuleService ruleSrv;
        public ICategoryRuleService RuleSrv
        {
            get { return ruleSrv; }
            set { ruleSrv = value; }
        }

        private ICategoryTreeService treeService;
        public ICategoryTreeService TreeService
        {
            get { return treeService; }
            set { treeService = value; }
        }

        #endregion

        #region  增,删,改,移动,实效

        [TransManager]
        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <param name="obj">节点</param>
        /// <returns></returns>
        public StationCategory SaveOrUpdate(StationCategory obj)
        {
            if (obj.Id == "")
            {
                obj.CreateDate = DateTime.Today;
                obj.UpdateDate = DateTime.Today;
                nodeSrv.AddChildNode(obj);
            }
            else
            {
                obj.UpdateDate = DateTime.Today;
                nodeSrv.UpdateCategoryNode(obj);
            }
            return obj;
        }

        /// <summary> 保存一个节点加上其子节点集合，返回该节点的父节点连同这个集合，给界面节点复制操作使用
        /// </summary>
        [TransManager]
        public IList SaveOrUpdate(IList lst)
        {
            IList list = new ArrayList();
            foreach (StationCategory var in lst)
            {
                list.Add(SaveOrUpdate(var));
            }
            return list;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(StationCategory obj)
        {
            return nodeSrv.DeleteCategoryNode(obj);
        }
        [TransManager]
        public bool Delete(IList lst)
        {
            foreach (object var in lst)
            {
                Delete((StationCategory)var);
            }
            return true;
        }

        /// <summary> 移动节点,返回被移动节点加上它的所有子节点的集合
        /// </summary>
        /// <param name="org">移动节点</param>
        /// <param name="toOrg">目的节点</param>
        /// <returns></returns>
        [TransManager]
        public IList Move(StationCategory obj, StationCategory toObj)
        {
            nodeSrv.MoveNode(obj, toObj);
            IList lstNodes = new ArrayList();
            IList lstChildNodes = GetALLChildNodes(obj);
            lstNodes.Add(obj);
            foreach (StationCategory o in lstChildNodes)
            {
                if (obj.Id != o.Id)
                    lstNodes.Add(o);
            }
            return lstNodes;
        }

        /// <summary>
        /// 失效节点 State ,0 无效,1有效
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        [TransManager]
        public IList Invalidate(StationCategory obj)
        {
            nodeSrv.InvalidateNode(obj);
            IList lstNodes = new ArrayList();
            lstNodes.Add(obj);
            IList lstChildNodes = nodeSrv.GetALLChildNodes(obj);
            foreach (StationCategory o in lstChildNodes)
            {
                lstNodes.Add(o);
            }
            return lstNodes;
        }

        /// <summary>
        /// 创建树
        /// </summary>
        /// <param name="treeName">树名称</param>
        /// <param name="treeType">树类型</param>
        /// <returns></returns>
        private CategoryTree InitTree(string treeName, Type treeType)
        {
            CategoryTree tree = treeService.GetCategoryTreeByType(treeType);
            if (tree != null)
                return tree;
            BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Today;
            treeService.SaveCategoryTree(tree);

            StationCategory root = new StationCategory();
            root.Name = treeName;
            root.TheTree = tree;
            root.CreateDate = System.DateTime.Today;
            nodeSrv.AddRoot(root);

            tree.RootId = root.Id;
            treeService.SaveCategoryTree(tree);
            return tree;
        }

        #endregion
        #region 查询
        /// <summary>
        /// 获取物料分类集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetAllObjects(Type t)
        {

            InitTree("仓库编码维护", typeof(StationCategory));
            ObjectQuery oq = new ObjectQuery();

            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("Name"));
            oq.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
            IList list = dao.ObjectQuery(t, oq);
            return list;
        }
        /// <summary>
        /// 通过条件查询仓库
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetObjectsByCondition(Type t, ObjectQuery oq)
        {

            InitTree("仓库编码维护", typeof(StationCategory));
            oq.AddOrder(Order.Asc("Name"));
            IList list = dao.ObjectQuery(t, oq);
            return list;
        }
        /// <summary>
        /// 获得物料分类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public StationCategory GetStationCategory(Type type, string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = NodeSrv.GetNodesByObjectQuery(type, oq);
            if (list.Count > 0)
                return list[0] as StationCategory;
            else
                return null;

        }
        public IList GetStationCategory(Type type, ObjectQuery oq)
        {
            IList list = NodeSrv.GetNodesByObjectQuery(type, oq);
            return list;
        }

        /// <summary>
        /// 获取当前节点下的所有子节点,包括当前节点
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        public IList GetALLChildNodes(StationCategory obj)
        {
            //保证ThePracticalityStateControlRule等属性不被lazy
            dao.FlushClearSession();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode", obj.SysCode + "%"));
            oq.AddOrder(Order.Asc("SysCode"));
            IList list = NodeSrv.GetALLChildNodes(obj, oq);
            return list;
        }

        /// <summary>
        /// 根据ID获取货位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StationCategory GetStationCategoryById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(StationCategory), oq);
            if (list.Count > 0)
                return list[0] as StationCategory;
            else
                return null;
        }

        #endregion

        /// <summary>
        /// 判断物料分类是否存在,存在返回,不存在添加然后返回
        /// </summary>
        /// <param name="matCat">待判断物料分类</param>
        /// <returns>物料分类</returns>
        [TransManager]
        public StationCategory GetStationCategory(StationCategory matCat)
        {
            StationCategory materialCategory = new StationCategory();
            if (matCat.ParentNode == null)
            {
                throw new Exception("父节点不能为空!");
            }
            else if (matCat.ParentNode.Id == "")
            {
                throw new Exception("父节点的Id不能为-1!");
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", matCat.ParentNode.Id));
            oq.AddCriterion(Expression.Eq("Name", matCat.Name));
            oq.AddFetchMode("TheTree.Rules", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);

            IList lst = dao.ObjectQuery(typeof(StationCategory), oq);
            if (lst.Count > 0)
            {
                materialCategory = lst[0] as StationCategory;
                return materialCategory;
            }
            else
            {
                matCat = SaveOrUpdate(matCat);
                return matCat;
            }
        }
    }
}
