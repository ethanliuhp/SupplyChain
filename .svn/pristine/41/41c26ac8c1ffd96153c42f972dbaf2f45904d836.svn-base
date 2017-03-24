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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.CommonClass.Domain;
using CommonSearchLib.BillCodeMng.Service;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    /// <summary>
    /// 工程任务类型服务
    /// </summary>
    public class ProjectTaskTypeSrv : IProjectTaskTypeSrv
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

        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
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

        #region 工程任务类型树(增,删,改)

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveProjectTaskTypeRootNode(IList lst)
        {
            ProjectTaskTypeTree node = lst[0] as ProjectTaskTypeTree;

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
            oq.AddCriterion(Expression.Eq("Name", "工程任务类型树"));
            IList list = dao.ObjectQuery(typeof(CategoryTree), oq);
            if (list.Count == 0)
            {
                tree = new CategoryTree();
                tree.CreateDate = DateTime.Now;
                tree.Author = author;
                tree.Code = ClassUtil.GetFullNameAndAssembly(node.GetType());
                tree.Name = "工程任务类型树";
                tree.RootId = "1";
                tree.MaxLevel = 0;
                tree.CurrMaxLevel = 1;

                dao.SaveOrUpdate(tree);
            }
            else
                tree = list[0] as CategoryTree;

            node.CreateDate = DateTime.Now;
            node.UpdatedDate = DateTime.Now;
            node.CategoryNodeType = NodeType.RootNode;
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
                lst[i] = SaveProjectTaskTypeTree(lst[i] as ProjectTaskTypeTree);
            }

            return lst;
        }

        /// <summary>
        /// 添加工程任务类型树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public ProjectTaskTypeTree SaveProjectTaskTypeTree(ProjectTaskTypeTree childNode)
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
        public IList SaveProjectTaskTypeTrees(IList lst)
        {
            IList list = new ArrayList();
            foreach (ProjectTaskTypeTree var in lst)
            {
                list.Add(SaveProjectTaskTypeTree(var));
            }
            return list;
        }

        /// <summary>
        /// 插入（部分属性固定）或修改任务类型树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public ProjectTaskTypeTree InsertOrUpdateTaskTypeTree(ProjectTaskTypeTree childNode)
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

                long orderNo = childNode.OrderNo;

                nodeSrv.AddChildNode(childNode);

                childNode.OrderNo = orderNo;//添加到节点集合后会自动设置一个最大的排序号
                dao.Update(childNode);
            }
            else
                nodeSrv.UpdateCategoryNode(childNode);

            return childNode;
        }

        /// <summary> 
        /// 插入（部分属性固定）或修改工程任务类型节点集合
        /// </summary>
        [TransManager]
        public IList InsertOrUpdateTaskTypeTrees(IList lst)
        {
            IList list = new ArrayList();
            foreach (ProjectTaskTypeTree var in lst)
            {
                list.Add(InsertOrUpdateTaskTypeTree(var));
            }
            return list;
        }

        /// <summary>
        /// 删除工程任务类型树节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteProjectTaskTypeTree(ProjectTaskTypeTree pbs)
        {
            pbs = dao.Get(typeof(ProjectTaskTypeTree), pbs.Id) as ProjectTaskTypeTree;
            return nodeSrv.DeleteCategoryNode(pbs);
        }

        [TransManager]
        public bool DeleteProjectTaskTypeTree(IList list)
        {
            foreach (ProjectTaskTypeTree node in list)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ParentNode.Id", node.Id));
                IList listChild = dao.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

                if (listChild.Count > 0)
                {
                    DeleteProjectTaskTypeTree(listChild);
                }

                ProjectTaskTypeTree tempNode = dao.Get(typeof(ProjectTaskTypeTree), node.Id) as ProjectTaskTypeTree;
                if (tempNode != null)
                    dao.Delete(tempNode);
            }
            return true;
        }


        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="pbs">移动节点</param>
        /// <param name="toPbs">目的节点</param>
        /// <returns></returns>
        [TransManager]
        public ProjectTaskTypeTree MoveProjectTaskTypeTree(ProjectTaskTypeTree pbs, ProjectTaskTypeTree toPbs)
        {
            //nodeSrv.MoveNode(pbs, toPbs);

            //修改原始父节点属性
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", pbs.ParentNode.Id));
            IList list = dao.ObjectQuery(typeof(ProjectTaskTypeTree), oq);
            if (list != null && list.Count == 1)
            {
                ProjectTaskTypeTree oldParent = dao.Get(typeof(ProjectTaskTypeTree), pbs.ParentNode.Id) as ProjectTaskTypeTree;
                oldParent.CategoryNodeType = NodeType.LeafNode;

                dao.SaveOrUpdate(oldParent);
            }

            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("Id", toPbs.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            list = dao.ObjectQuery(typeof(ProjectTaskTypeTree), oq);
            toPbs = list[0] as ProjectTaskTypeTree;

            //修改自身属性
            pbs = dao.Get(typeof(ProjectTaskTypeTree), pbs.Id) as ProjectTaskTypeTree;
            pbs.ParentNode = toPbs;
            pbs.Level = toPbs.Level + 1;
            pbs.SysCode = toPbs.SysCode + pbs.Id + ".";
            pbs.OrderNo = GetMaxOrderNo(pbs) + 1;
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
            IList listChild = dao.ObjectQuery(typeof(ProjectTaskTypeTree), oq);
            if (listChild.Count > 0)
            {
                for (int i = 0; i < listChild.Count; i++)
                {
                    ProjectTaskTypeTree childNode = listChild[i] as ProjectTaskTypeTree;
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

        private void UpdateChildNodes(ProjectTaskTypeTree parentNode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", parentNode.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            parentNode = dao.ObjectQuery(typeof(ProjectTaskTypeTree), oq)[0] as ProjectTaskTypeTree;
            if (parentNode.ChildNodes.Count > 0)
            {
                IList listChild = parentNode.ChildNodes;
                for (int i = 0; i < listChild.Count; i++)
                {
                    ProjectTaskTypeTree childNode = listChild[i] as ProjectTaskTypeTree;
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
        public IList InvalidateProjectTaskTypeTree(ProjectTaskTypeTree pbs)
        {
            nodeSrv.InvalidateNode(pbs);
            IList lstNodes = new ArrayList();
            lstNodes.Add(pbs);
            IList lstChildNodes = nodeSrv.GetALLChildNodes(pbs);
            foreach (ProjectTaskTypeTree o in lstChildNodes)
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
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof(ProjectTaskTypeTree));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            ProjectTaskTypeTree root = new ProjectTaskTypeTree();
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
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof(ProjectTaskTypeTree));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof(BusinessOperators), aPerson.Id) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            ProjectTaskTypeTree root = new ProjectTaskTypeTree();
            root.Name = treeName;
            root.TheTree = tree;
            root.CreateDate = System.DateTime.Now;
            nodeSrv.AddRoot(root);

            tree.RootId = root.Id;
            treeService.SaveCategoryTree(tree);
            return tree;
        }

        /// <summary>
        /// 初始化工程任务类型树规则
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool InitRule()
        {
            //CategoryTree tree = InitTree("工程任务类型树", typeof(ProjectTaskTypeTree));
            ////CategoryTree tree = treeService.GetCategoryTreeByType(typeof(ProjectTaskTypeTree));
            //BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            //CategoryRule rule = new CategoryRule();
            //rule.Name = "销售工程任务类型树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly(typeof(ProjectTaskTypeTree));
            //rule.ChildNodeType =  ClassUtil.GetFullNameAndAssembly(typeof(SellProjectTaskTypeTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "采购工程任务类型树";
            //rule.TheTree = tree;
            //rule.ParentNodeType =  ClassUtil.GetFullNameAndAssembly(typeof(ProjectTaskTypeTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchaseProjectTaskTypeTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "仓储工程任务类型树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(ProjectTaskTypeTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(StorageProjectTaskTypeTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "销售工程任务类型树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(SellProjectTaskTypeTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(SellProjectTaskTypeTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "采购工程任务类型树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchaseProjectTaskTypeTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchaseProjectTaskTypeTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "仓储工程任务类型树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(StorageProjectTaskTypeTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly(typeof(StorageProjectTaskTypeTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);
            throw new Exception("工程任务类型树无规则！");
            return true;
        }
        #endregion

        #region 工程任务类型树(查询)
        /// <summary>
        /// 获取工程任务类型树节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetProjectTaskTypeTrees(Type t)
        {
            //CategoryTree tree = InitTree("工程任务类型树", typeof(ProjectTaskTypeTree));
            ObjectQuery oq = new ObjectQuery();
            //oq.AddFetchMode("TheTree", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;

        }

        /// <summary>
        /// 获取工程任务类型树节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetProjectTaskTypeByInstance(string projectId)
        {
            //CategoryTree tree = InitTree("工程任务类型树", typeof(ProjectTaskTypeTree));

            #region 获取有权限和无权限的节点
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            //oq.AddOrder(Order.Asc("Level"));
            //oq.AddOrder(Order.Asc("OrderNo"));
            //IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(ProjectTaskTypeTree), oq);
            //return list;
            #endregion

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddOrder(Order.Asc("SysCode"));
            //oq.AddOrder(Order.Asc("CategoryNodeType"));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            IList list = dao.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

            return list;
        }
        /// <summary>
        /// 获取工程任务类型树节点集合
        /// </summary>
        public IList GetProjectTaskTypeByInstance(string projectId, IList listParentTaskType)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));

            Disjunction dis = new Disjunction();
            foreach (ProjectTaskTypeTree taskType in listParentTaskType)
            {
                dis.Add(Expression.Like("SysCode", taskType.SysCode + "%"));
            }
            oq.AddCriterion(dis);

            oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddOrder(Order.Asc("SysCode"));
            //oq.AddOrder(Order.Asc("CategoryNodeType"));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            IList list = dao.ObjectQuery(typeof(ProjectTaskTypeTree), oq);

            return list;
        }
        /// <summary>
        /// 返回节点node所在树的当前层次的最大OrderNo
        /// </summary>
        /// <param name="node"></param>
        /// <param name="oq"></param>
        /// <returns></returns>
        public long GetMaxOrderNo(CategoryNode node)
        {
            if (node == null)
                return 0;

            return nodeSrv.GetMaxOrderNo(node, null);
        }

        /// <summary>
        /// 判断工程任务类型树是否存在,存在返回,不存在添加然后返回
        /// </summary>
        [TransManager]
        public ProjectTaskTypeTree GetProjectTaskTypeTree(ProjectTaskTypeTree orgCat)
        {
            ProjectTaskTypeTree opeOrg = new ProjectTaskTypeTree();
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

            IList lst = dao.ObjectQuery(typeof(ProjectTaskTypeTree), oq);
            if (lst.Count > 0)
            {
                opeOrg = lst[0] as ProjectTaskTypeTree;
                return opeOrg;
            }
            else
            {
                orgCat = SaveProjectTaskTypeTree(orgCat);
                return orgCat;
            }
        }

        /// <summary>
        /// 获取工程任务类型树及岗位节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetProjectTaskTypeTreeAndJobs(Type t)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;
        }

        /// <summary>
        /// 获取工程任务类型树及岗位节点集合(把工程任务类型树的ChildNodes也eager出来)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetProjectTaskTypeTreeAndJobs()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(ProjectTaskTypeTree), oq);
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
        /// 获取工程任务类型树及岗位节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetOpeOrgAndJobsByInstance()
        {
            ObjectQuery oq = new ObjectQuery();
            //直接eager岗位，实例权限不起作用
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(ProjectTaskTypeTree), oq);
            IList lstInstance = list[1] as IList;
            IList listTree = list[0] as IList;
            //foreach (ProjectTaskTypeTree childNode in listTree)
            //{
            //    childNode.OperationJobs = new ArrayList();
            //    if (Contains(lstInstance, childNode))
            //    {
            //        oq = new ObjectQuery();
            //        oq.AddCriterion(Expression.Eq("ProjectTaskTypeTree.Id", childNode.Id));
            //        IList jobList = dao.ObjectQuery(typeof(OperationJob), oq);
            //        (childNode.OperationJobs as ArrayList).AddRange(jobList);
            //    }
            //}
            return list;
        }

        public IList GetProjectTaskTypeTrees(Type t, ObjectQuery oq)
        {
            return nodeSrv.GetNodesByObjectQuery(t, oq);
        }


        /// <summary>
        /// 获取当前组织下面的所有子组织
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        public IList GetALLChildNodes(ProjectTaskTypeTree pbs)
        {
            return nodeSrv.GetALLChildNodes(pbs);
        }

        /// <summary>
        /// 根据ID获取工程任务类型树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProjectTaskTypeTree GetProjectTaskTypeTreeById(string id)
        {
            return nodeSrv.GetCategoryNodeById(id, typeof(ProjectTaskTypeTree)) as ProjectTaskTypeTree;
        }

        #endregion

        #region 树规则

        /// <summary>
        /// 根据树类型获取所有规则集合
        /// </summary>
        /// <param name="treeType"></param>
        /// <returns></returns>
        public IList GetProjectTaskTypeTreeRules(Type treeType)
        {
            CategoryTree tree = InitTree("工程任务类型树", typeof(ProjectTaskTypeTree));
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
        public IList GetProjectTaskTypeTreeRules(Type treeType, Type parentNodeType)
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
            string sql = "Select MAX(OPGCODE) Code from ResProjectTaskTypeTree";
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
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }
    }
}
