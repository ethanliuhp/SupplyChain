using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using VirtualMachine.SystemAspect.Security;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data;
using VirtualMachine.Core.DataAccess;
using Application.Resource.FinancialResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.CommonClass.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Service
{
    /// <summary>
    /// 成本项分类服务
    /// </summary>
    public class CostItemCategorySrv : ICostItemCategorySrv
    {
        #region 注入服务
        private IDao dao;
        public IDao Dao
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

        private IOPGManagementManager theOpgManage;
        public IOPGManagementManager TheOpgManage
        {
            get { return theOpgManage; }
            set { theOpgManage = value; }
        }

        #endregion

        #region 调用Ado保存
        public int SaveSQL(string sql)
        {
            int result = 0;
            IDbTransaction tra;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            tra = cnn.BeginTransaction();
            try
            {
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tra;
                result = cmd.ExecuteNonQuery();
                tra.Commit();
            }
            catch
            {
                tra.Rollback();
                throw;
            }
            finally
            {

            }
            return result;
        }

        public DataSet SearchSQL(string sql)
        {
            DataSet ds = new DataSet();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;

            IDbCommand cmd = cnn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            IDataReader dr = cmd.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }


        #endregion

        #region 成本项分类树(增,删,改)

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveCostItemCategoryRootNode(IList lst)
        {
            CostItemCategory node = lst[0] as CostItemCategory;

            string operId = "";
            if (node.Author == null)
            {
                try
                {
                    operId = SecurityUtil.GetLogOperId();
                }
                catch { }
            }
            else
            {
                operId = node.Author.Id;
            }
            IBusinessOperators author = Dao.Get(typeof(BusinessOperators), operId) as IBusinessOperators;


            CategoryTree tree = null;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name", "成本项分类树"));
            IList list = dao.ObjectQuery(typeof(CategoryTree), oq);
            if (list.Count == 0)
            {
                tree = new CategoryTree();
                tree.CreateDate = DateTime.Now;
                tree.Author = author;
                tree.Code = ClassUtil.GetFullNameAndAssembly(node.GetType());
                tree.Name = "成本项分类树";
                tree.RootId = "1";
                tree.MaxLevel = 0;
                tree.CurrMaxLevel = 1;

                dao.SaveOrUpdate(tree);
            }
            else
                tree = list[0] as CategoryTree;

            node.CreateDate = DateTime.Now;
            node.UpdatedDate = DateTime.Now;
            node.State = 1;
            node.Level = 1;
            node.OrderNo = 0;

            node.Author = author;

            node.TheTree = tree;

            dao.SaveOrUpdate(node);

            if (!string.IsNullOrEmpty(node.Id))
                node.SysCode = node.Id + ".";

            dao.SaveOrUpdate(node);

            lst[0] = node;

            for (int i = 1; i < lst.Count; i++)
            {
                lst[i] = SaveCostItemCategory(lst[i] as CostItemCategory);
            }

            return lst;
        }

        /// <summary>
        /// 添加成本项分类树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public CostItemCategory SaveCostItemCategory(CostItemCategory childNode)
        {
            childNode.UpdatedDate = DateTime.Now;
            if (childNode.Id == null)
            {
                childNode.CreateDate = DateTime.Now;
                string operId = "";
                if (childNode.Author == null)
                {
                    try
                    {
                        operId = SecurityUtil.GetLogOperId();
                    }
                    catch { }
                }
                else
                {
                    operId = childNode.Author.Id;
                }
                childNode.Author = Dao.Get(typeof(BusinessOperators), operId) as IBusinessOperators;

                nodeSrv.AddChildNode(childNode);
            }
            else
                nodeSrv.UpdateCategoryNode(childNode);
            return childNode;
        }

        /// <summary> 
        /// 保存一个节点加上其子节点集合
        /// </summary>
        [TransManager]
        public IList SaveCostItemCategorys(IList lst)
        {
            IList list = new ArrayList();
            foreach (CostItemCategory var in lst)
            {
                list.Add(SaveCostItemCategory(var));
            }
            return list;
        }

        /// <summary>
        /// 删除成本项分类树节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCostItemCategory(CostItemCategory cate)
        {
            try
            {
                if (cate == null || cate.SysCode == "")
                    return false;

                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                //1.删除WBS节点及其子节点
                string sql = " delete from thd_costitemcategory t1 where t1.syscode like '" + cate.SysCode + "%'";

                command.CommandText = sql;
                command.ExecuteNonQuery();

                //2.更新父节点类型信息
                if (cate.ParentNode != null)
                {
                    sql = "select count(*) childCount from thd_costitemcategory t1 where t1.parentnodeid='" + cate.ParentNode.Id + "'";
                    command.CommandText = sql;
                    int childCount = Convert.ToInt32(command.ExecuteScalar());

                    //根节点=0，枝节点=1，叶节点=2
                    if (childCount == 0)
                        sql = "update thd_costitemcategory t1 set t1.categorynodetype=2 where t1.id='" + cate.ParentNode.Id + "'";
                    else
                        sql = "update thd_costitemcategory t1 set t1.categorynodetype=1 where t1.id='" + cate.ParentNode.Id + "'";

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;

            //pbs = dao.Get(typeof(CostItemCategory), pbs.Id) as CostItemCategory;
            //return nodeSrv.DeleteCategoryNode(pbs);
        }

        [TransManager]
        public bool DeleteCostItemCategory(IList list)
        {
            foreach (CostItemCategory node in list)
            {
                DeleteCostItemCategory(node);
            }

            //foreach (CostItemCategory node in list)
            //{
            //    ObjectQuery oq = new ObjectQuery();
            //    oq.AddCriterion(Expression.Eq("ParentNode.Id", node.Id));
            //    IList listChild = dao.ObjectQuery(typeof(CostItemCategory), oq);

            //    if (listChild.Count > 0)
            //    {
            //        DeleteCostItemCategory(listChild);
            //    }

            //    CostItemCategory tempNode = dao.Get(typeof(CostItemCategory), node.Id) as CostItemCategory;
            //    if (tempNode != null)
            //        dao.Delete(tempNode);
            //}
            return true;
        }


        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="pbs">移动节点</param>
        /// <param name="toPbs">目的节点</param>
        /// <returns></returns>
        [TransManager]
        public CostItemCategory MoveCostItemCategory(CostItemCategory pbs, CostItemCategory toPbs)
        {
            //nodeSrv.MoveNode(pbs, toPbs);

            //修改原始父节点属性
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", pbs.ParentNode.Id));
            IList list = dao.ObjectQuery(typeof(CostItemCategory), oq);
            if (list != null && list.Count == 1)
            {
                CostItemCategory oldParent = dao.Get(typeof(CostItemCategory), pbs.ParentNode.Id) as CostItemCategory;
                oldParent.CategoryNodeType = NodeType.LeafNode;

                dao.SaveOrUpdate(oldParent);
            }

            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("Id", toPbs.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            list = dao.ObjectQuery(typeof(CostItemCategory), oq);
            toPbs = list[0] as CostItemCategory;

            //修改自身属性
            pbs = dao.Get(typeof(CostItemCategory), pbs.Id) as CostItemCategory;
            pbs.ParentNode = toPbs;
            pbs.Level = toPbs.Level + 1;
            pbs.SysCode = toPbs.SysCode + pbs.Id + ".";
            pbs.OrderNo = GetMaxOrderNo(pbs, null) + 1;
            toPbs.ChildNodes.Add(pbs);

            if (toPbs.CategoryNodeType == NodeType.LeafNode)//如果目的节点是叶节点设为中间节点  0、1、2
            {
                toPbs.CategoryNodeType = NodeType.MiddleNode;
            }
            dao.SaveOrUpdate(toPbs);

            //更新所有子节点的层次码
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", pbs.Id));
            IList listChild = dao.ObjectQuery(typeof(CostItemCategory), oq);
            if (listChild.Count > 0)
            {
                for (int i = 0; i < listChild.Count; i++)
                {
                    CostItemCategory childNode = listChild[i] as CostItemCategory;
                    childNode.SysCode = pbs.SysCode + childNode.Id + ".";
                    childNode.Level = pbs.Level + 1;

                    listChild[i] = childNode;

                    //dao.SaveOrUpdate(childNode);

                    UpdateChildNodes(childNode);
                }

                dao.SaveOrUpdate(listChild);
            }

            return pbs;
        }

        private void UpdateChildNodes(CostItemCategory parentNode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", parentNode.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            parentNode = dao.ObjectQuery(typeof(CostItemCategory), oq)[0] as CostItemCategory;
            if (parentNode.ChildNodes.Count > 0)
            {
                IList listChild = parentNode.ChildNodes;
                for (int i = 0; i < listChild.Count; i++)
                {
                    CostItemCategory childNode = listChild[i] as CostItemCategory;
                    childNode.SysCode = parentNode.SysCode + childNode.Id + ".";
                    childNode.Level = parentNode.Level + 1;

                    listChild[i] = childNode;

                    //dao.SaveOrUpdate(childNode);

                    UpdateChildNodes(childNode);
                }
            }
        }

        /// <summary>
        /// 失效节点 State ,0 无效,1有效
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public IList InvalidateCostItemCategory(CostItemCategory pbs)
        {
            nodeSrv.InvalidateNode(pbs);
            IList lstNodes = new ArrayList();
            lstNodes.Add(pbs);
            IList lstChildNodes = nodeSrv.GetALLChildNodes(pbs);
            foreach (CostItemCategory o in lstChildNodes)
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
        [TransManager]
        public CategoryTree InitTree(string treeName, Type treeType)
        {
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof(CostItemCategory));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            CostItemCategory root = new CostItemCategory();
            root.Name = treeName;
            root.TheTree = tree;
            root.CreateDate = System.DateTime.Now;
            nodeSrv.AddRoot(root);

            tree.RootId = root.Id;
            treeService.SaveCategoryTree(tree);
            return tree;
        }
        public CategoryTree InitTree(string treeName, Type treeType, StandardPerson aPerson)
        {
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof(CostItemCategory));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof(BusinessOperators), aPerson.Id) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            CostItemCategory root = new CostItemCategory();
            root.Name = treeName;
            root.TheTree = tree;
            root.CreateDate = System.DateTime.Now;
            nodeSrv.AddRoot(root);

            tree.RootId = root.Id;
            treeService.SaveCategoryTree(tree);
            return tree;
        }

        /// <summary>
        /// 初始化成本项分类树规则
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool InitRule()
        {
            //CategoryTree tree = InitTree("成本项分类树", typeof(CostItemCategory));
            ////CategoryTree tree = treeService.GetCategoryTreeByType(typeof(CostItemCategory));
            //BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            //CategoryRule rule = new CategoryRule();
            //rule.Name = "销售成本项分类树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly(typeof(CostItemCategory));
            //rule.ChildNodeType =  ClassUtil.GetFullNameAndAssembly(typeof(SellCostItemCategory));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "采购成本项分类树";
            //rule.TheTree = tree;
            //rule.ParentNodeType =  ClassUtil.GetFullNameAndAssembly(typeof(CostItemCategory));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchaseCostItemCategory));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "仓储成本项分类树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(CostItemCategory));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(StorageCostItemCategory));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "销售成本项分类树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(SellCostItemCategory));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(SellCostItemCategory));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "采购成本项分类树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchaseCostItemCategory));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchaseCostItemCategory));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "仓储成本项分类树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(StorageCostItemCategory));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly(typeof(StorageCostItemCategory));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);
            throw new Exception("成本项分类树无规则！");
            return true;
        }
        #endregion

        #region 成本项分类树(查询)
        /// <summary>
        /// 获取成本项分类树节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetCostItemCategorys(Type t)
        {
            //CategoryTree tree = InitTree("成本项分类树", typeof(CostItemCategory));
            ObjectQuery oq = new ObjectQuery();
            //oq.AddFetchMode("TheTree", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;

        }

        /// <summary>
        /// 获取成本项分类树节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetCostItemCategoryByInstance()
        {
            //CategoryTree tree = InitTree("成本项分类树", typeof(CostItemCategory));

            #region 获取有权限和无权限的节点
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            //oq.AddOrder(Order.Asc("Level"));
            //oq.AddOrder(Order.Asc("OrderNo"));
            //IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(CostItemCategory), oq);
            #endregion

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            IList list = dao.ObjectQuery(typeof(CostItemCategory), oq);
            return list;
        }

        /// <summary>
        /// 返回节点node所在树的当前层次的最大OrderNo
        /// </summary>
        /// <param name="node"></param>
        /// <param name="oq"></param>
        /// <returns></returns>
        public long GetMaxOrderNo(CategoryNode node, ObjectQuery oq)
        {
            return nodeSrv.GetMaxOrderNo(node, null);
        }

        /// <summary>
        /// 判断成本项分类树是否存在,存在返回,不存在添加然后返回
        /// </summary>
        [TransManager]
        public CostItemCategory GetCostItemCategory(CostItemCategory orgCat)
        {
            CostItemCategory opeOrg = new CostItemCategory();
            if (orgCat.ParentNode == null)
            {
                throw new Exception("父节点不能为空!");
            }
            else if (orgCat.ParentNode.Id == null)
            {
                throw new Exception("父节点的Id不能为空!");
            }
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", orgCat.ParentNode.Id));
            oq.AddCriterion(Expression.Eq("Name", orgCat.Name));
            oq.AddFetchMode("TheTree.Rules", NHibernate.FetchMode.Eager);

            IList lst = dao.ObjectQuery(typeof(CostItemCategory), oq);
            if (lst.Count > 0)
            {
                opeOrg = lst[0] as CostItemCategory;
                return opeOrg;
            }
            else
            {
                orgCat = SaveCostItemCategory(orgCat);
                return orgCat;
            }
        }

        /// <summary>
        /// 获取成本项分类树及岗位节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetCostItemCategoryAndJobs(Type t)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;
        }

        /// <summary>
        /// 获取成本项分类树及岗位节点集合(把成本项分类树的ChildNodes也eager出来)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetCostItemCategoryAndJobs()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(CostItemCategory), oq);
            return list;
        }

        /// <summary>
        /// 确定分类树节点是否有权限操作
        /// </summary>
        private bool Contains(IList lstInstance, CategoryNode node)
        {
            foreach (CategoryNode n in lstInstance)
            {
                if (n.Id == node.Id)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取成本项分类树及岗位节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetOpeOrgAndJobsByInstance()
        {
            ObjectQuery oq = new ObjectQuery();
            //直接eager岗位，实例权限不起作用
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(CostItemCategory), oq);
            IList lstInstance = list[1] as IList;
            IList listTree = list[0] as IList;
            //foreach (CostItemCategory childNode in listTree)
            //{
            //    childNode.OperationJobs = new ArrayList();
            //    if (Contains(lstInstance, childNode))
            //    {
            //        oq = new ObjectQuery();
            //        oq.AddCriterion(Expression.Eq("CostItemCategory.Id", childNode.Id));
            //        IList jobList = dao.ObjectQuery(typeof(OperationJob), oq);
            //        (childNode.OperationJobs as ArrayList).AddRange(jobList);
            //    }
            //}
            return list;
        }

        public IList GetCostItemCategorys(Type t, ObjectQuery oq)
        {
            return nodeSrv.GetNodesByObjectQuery(t, oq);
        }


        /// <summary>
        /// 获取当前节点下面的所有子节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        public IList GetALLChildNodes(CostItemCategory pbs)
        {
            return nodeSrv.GetALLChildNodes(pbs);
        }

        /// <summary>
        /// 根据ID获取成本项分类树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CostItemCategory GetCostItemCategoryById(string id)
        {
            return nodeSrv.GetCategoryNodeById(id, typeof(CostItemCategory)) as CostItemCategory;
        }

        #endregion

        #region 树规则

        /// <summary>
        /// 根据树类型获取所有规则集合
        /// </summary>
        /// <param name="treeType"></param>
        /// <returns></returns>
        public IList GetCostItemCategoryRules(Type treeType)
        {
            CategoryTree tree = InitTree("成本项分类树", typeof(CostItemCategory));
            IList list = new ArrayList();
            list = ruleSrv.GetCategoryRules(treeType);
            if (list.Count == 0)
            {
                this.InitRule();
                list = ruleSrv.GetCategoryRules(treeType);
            }
            return list;
        }

        /// <summary>
        /// 根据树类型和父节点类型获取父节点的所有规则集合
        /// </summary>
        /// <param name="treeType"></param>
        /// <param name="parentNodeType"></param>
        /// <returns></returns>
        public IList GetCostItemCategoryRules(Type treeType, Type parentNodeType)
        {
            return ruleSrv.GetCategoryRules(treeType, parentNodeType);
        }


        #endregion

        /// <summary>
        /// 取出当前的最大编码号
        /// </summary>
        /// <returns></returns>
        public string GetNextOPGCode()
        {
            string MaxCode = "";
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = "Select MAX(OPGCODE) Code from ResCostItemCategory";
            command.CommandText = sql;

            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = ds.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                MaxCode = row["Code"].ToString();
            }
            int maxNum = Convert.ToInt32(MaxCode.Substring(1, MaxCode.Length - 1)) + 1;
            MaxCode = "Z" + string.Format("{0:D4}", maxNum);
            return MaxCode;
        }
        /// <summary>
        /// 保存成本项分类
        /// </summary>
        /// <param name="cateList"></param>
        /// <returns></returns>
        [TransManager]
        public bool SaveCostItemCate(List<CostItemCategory> cateList)
        {
            string operId = SecurityUtil.GetLogOperId();
            IBusinessOperators author = Dao.Get(typeof(BusinessOperators), operId) as IBusinessOperators;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name", "成本项分类树"));
            IList listTree = dao.ObjectQuery(typeof(CategoryTree), oq);
            CategoryTree tree = listTree[0] as CategoryTree;

            for (int i = 0; i < cateList.Count; i++)
            {
                CostItemCategory cate = cateList[i];
                cate.Author = author;
                cate.TheTree = tree;
            }
            return dao.Save(cateList);
        }

        /// <summary>
        /// 保存成本项分类
        /// </summary>
        /// <param name="cateList"></param>
        /// <param name="defaultCate"></param>
        /// <returns></returns>
        [TransManager]
        public bool SaveCostItemCate(List<CostItemCategory> cateList, CostItemCategory defaultCate)
        {
            string operId = SecurityUtil.GetLogOperId();
            IBusinessOperators author = Dao.Get(typeof(BusinessOperators), operId) as IBusinessOperators;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name", "成本项分类树"));
            IList listTree = dao.ObjectQuery(typeof(CategoryTree), oq);
            CategoryTree tree = listTree[0] as CategoryTree;

            for (int i = 0; i < cateList.Count; i++)
            {
                CostItemCategory cate = cateList[i];
                cate.Author = author;
                cate.TheTree = tree;
            }
            dao.Save(cateList);

            var query = from c in cateList
                        where c.ParentNode != null && c.ParentNode.Id == defaultCate.Id
                        select c;
            foreach (CostItemCategory cate in query)
            {
                cate.SysCode = cate.ParentNode.SysCode + cate.Id + ".";
                UpdateChildNodes(cate, cateList);
            }
            return dao.SaveOrUpdate(cateList);
        }

        private void UpdateChildNodes(CostItemCategory parentNode, List<CostItemCategory> cateList)
        {
            var query = from c in cateList
                        where c.ParentNode != null && c.ParentNode.Id == parentNode.Id
                        select c;

            foreach (CostItemCategory childNode in query)
            {
                childNode.SysCode = parentNode.SysCode + childNode.Id + ".";
                UpdateChildNodes(childNode, cateList);
            }
        }
    }
}
