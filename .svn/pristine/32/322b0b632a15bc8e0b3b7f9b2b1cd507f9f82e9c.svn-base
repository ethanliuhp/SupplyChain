using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Domain;
using System.Collections;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Service
{
    public class IndicatorDefineService : IIndicatorDefineService
    {
        #region 注入服务

        private IDao dao;
        virtual public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
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
        /// 保存指标分类
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public IndicatorCategory SaveCategory(IndicatorCategory obj)
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
        /// 保存指标分类集合
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        [TransManager]
        public IList SaveCategorys(IList lst)
        {
            IList list = new ArrayList();
            foreach (IndicatorCategory var in lst)
            {
                list.Add(SaveCategory(var));
            }
            return list;
        }

        /// <summary>
        /// 删除指标分类节点
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCategory(IndicatorCategory obj)
        {
            return NodeSrv.DeleteCategoryNode(obj);
        }

        /// <summary>
        /// 移动节点，返回目标节点
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="toObj"></param>
        /// <returns></returns>
        [TransManager]
        public IndicatorCategory MoveCategory(IndicatorCategory obj, IndicatorCategory toObj)
        {
            NodeSrv.MoveNode(obj, toObj);
            return obj;
        }

        /// <summary> 
        /// 失效节点 State,0无效,1有效,返回被失效节点加上它的所有子节点的集合
        /// </summary>
        [TransManager]
        public IList InvalidateCategory(IndicatorCategory obj)
        {
            NodeSrv.InvalidateNode(obj);
            ArrayList lstNodes = new ArrayList();
            lstNodes.Add(obj);
            IList lstChildNodes = NodeSrv.GetALLChildNodes(obj);
            lstNodes.AddRange(lstChildNodes);
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
            CategoryTree tree = TreeService.GetCategoryTreeByType(treeType);
            if (tree != null)
                return tree;

            BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = DateTime.Now;
            TreeService.SaveCategoryTree(tree);

            IndicatorCategory root = new IndicatorCategory();
            root.Name = treeName;
            root.TheTree = tree;
            root.Code = "指标树";
            root.CreateDate = DateTime.Today;
            NodeSrv.AddRoot(root);

            tree.RootId = root.Id;
            TreeService.SaveCategoryTree(tree);
            return tree;
        }

        #region 指标分类(查询)

        /// <summary> 
        /// 获取指标分类节点集合
        /// </summary>
        [TransManager]
        public IList GetCategorys()
        {
            this.InitTree("指标树", typeof(IndicatorCategory));
            ObjectQuery oq = new ObjectQuery();
            oq.AddOrder(Order.Asc("SysCode"));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(IndicatorCategory), oq);
            return list;
        }

        /// <summary> 
        /// 根据ID获取指标分类
        /// </summary>
        public IndicatorCategory GetCategoryById(string id)
        {
            return dao.Get(typeof(IndicatorCategory), id) as IndicatorCategory;
        }
        #endregion

        #region 指标定义相关服务
        /// <summary>
        /// 保存指标定义
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public IndicatorDefinition SaveIndicatorDefinition(IndicatorDefinition obj)
        {
            if (string.IsNullOrEmpty(obj.Id))
            {
                obj.CreatedDate = DateTime.Now;
            }
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 删除指标定义
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteIndicatorDefinition(IndicatorDefinition obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 根据指标分类查找指标
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IList GetIndicatorDefinitionByCategory(IndicatorCategory category)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Category.Id", category.Id));
            oq.AddFetchMode("Category", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Author", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheOpeOrg", NHibernate.FetchMode.Eager);
            IList list = Dao.ObjectQuery(typeof(IndicatorDefinition), oq);
            return list;
        }

        /// <summary>
        /// 根据指标ID查找指标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IndicatorDefinition GetIndicatorDefinitionById(long id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = Dao.ObjectQuery(typeof(IndicatorDefinition), oq);
            if (list.Count > 0)
            {
                return list[0] as IndicatorDefinition;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据条件获得指标
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetIndicatorDefinitionByQuery(ObjectQuery oq)
        {
            oq.AddFetchMode("Category", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Author", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheOpeOrg", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(IndicatorDefinition), oq);
        }
        #endregion
    }
}
