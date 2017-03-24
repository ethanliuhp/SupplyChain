using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.SystemAspect.Security;
using System.Collections;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using NHibernate.Criterion;
using Application.Resource.CommonClass.Domain;
using NHibernate;
using CommonSearchLib.BillCodeMng.Service;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Service
{
    public class AccountTitleTreeSvr : IAccountTitleTreeSvr
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
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }
        #endregion

        #region 会计科目树(增,删,改)

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveAccountTitleTreeRootNode(IList lst)
        {
            AccountTitleTree node = lst[0] as AccountTitleTree;

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
            oq.AddCriterion(Expression.Eq("Name", "会计科目"));
            IList list = dao.ObjectQuery(typeof(CategoryTree), oq);
            if (list.Count == 0)
            {
                tree = new CategoryTree();
                tree.CreateDate = DateTime.Now;
                tree.Author = author;
                tree.Code = ClassUtil.GetFullNameAndAssembly(node.GetType());
                tree.Name = "会计科目";
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
            Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;

            node.TheTree = tree;

            dao.SaveOrUpdate(node);

            if (!string.IsNullOrEmpty(node.Id))
                node.SysCode = node.Id + ".";

            dao.SaveOrUpdate(node);

            lst[0] = node;

            for (int i = 1; i < lst.Count; i++)
            {
                lst[i] = SaveAccountTitleTree(lst[i] as AccountTitleTree);
            }

            return lst;
        }

        /// <summary>
        /// 添加会计科目树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public AccountTitleTree SaveAccountTitleTree(AccountTitleTree childNode)
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
        public IList SaveCostAccountSubjects(IList lst)
        {
            IList list = new ArrayList();
            foreach (AccountTitleTree var in lst)
            {
                list.Add(SaveAccountTitleTree(var));
            }
            return list;
        }

        /// <summary>
        /// 删除成本核算科目树节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteAccountTitleTree(AccountTitleTree pbs)
        {
            pbs = dao.Get(typeof(AccountTitleTree), pbs.Id) as AccountTitleTree;
            return nodeSrv.DeleteCategoryNode(pbs);
        }

        [TransManager]
        public bool DeleteAccountTitleTree(IList list)
        {
            foreach (AccountTitleTree node in list)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ParentNode.Id", node.Id));
                IList listChild = dao.ObjectQuery(typeof(AccountTitleTree), oq);

                if (listChild.Count > 0)
                {
                    DeleteAccountTitleTree(listChild);
                }

                AccountTitleTree tempNode = dao.Get(typeof(AccountTitleTree), node.Id) as AccountTitleTree;
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
        public AccountTitleTree MoveAccountTitleTree(AccountTitleTree pbs, AccountTitleTree toPbs)
        {
            //nodeSrv.MoveNode(pbs, toPbs);

            //修改原始父节点属性
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", pbs.ParentNode.Id));
            IList list = dao.ObjectQuery(typeof(AccountTitleTree), oq);
            if (list != null && list.Count == 1)
            {
                AccountTitleTree oldParent = dao.Get(typeof(AccountTitleTree), pbs.ParentNode.Id) as AccountTitleTree;
                oldParent.CategoryNodeType = NodeType.LeafNode;

                dao.SaveOrUpdate(oldParent);
            }

            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("Id", toPbs.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            list = dao.ObjectQuery(typeof(AccountTitleTree), oq);
            toPbs = list[0] as AccountTitleTree;

            //修改自身属性
            pbs = dao.Get(typeof(AccountTitleTree), pbs.Id) as AccountTitleTree;
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
            IList listChild = dao.ObjectQuery(typeof(AccountTitleTree), oq);
            if (listChild.Count > 0)
            {
                for (int i = 0; i < listChild.Count; i++)
                {
                    AccountTitleTree childNode = listChild[i] as AccountTitleTree;
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

        private void UpdateChildNodes(AccountTitleTree parentNode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", parentNode.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            parentNode = dao.ObjectQuery(typeof(AccountTitleTree), oq)[0] as AccountTitleTree;
            if (parentNode.ChildNodes.Count > 0)
            {
                IList listChild = parentNode.ChildNodes;

                for (int i = 0; i < listChild.Count; i++)
                {
                    AccountTitleTree childNode = listChild[i] as AccountTitleTree;
                    childNode.Level = parentNode.Level + 1;
                    childNode.SysCode = parentNode.SysCode + childNode.Id + ".";

                    listChild[i] = childNode;
                    //dao.SaveOrUpdate(childNode);
                    UpdateChildNodes(childNode);
                }
            }
        }

        public IDictionary MoveNode(AccountTitleTree fromNode, AccountTitleTree toNode)
        {
            AccountTitleTree cat = MoveAccountTitleTree(fromNode, toNode);
            IDictionary dic = new Hashtable();
            dic.Add(cat.ParentNode.Id.ToString(), cat.ParentNode);
            dic.Add(cat.Id.ToString(), cat);
            IList lstChildNodes = GetALLChildNodes(cat);
            foreach (AccountTitleTree var in lstChildNodes)
            {
                dic.Add(var.Id.ToString(), var);
            }
            return dic;
        }
        /// <summary>
        /// 失效节点 State ,0 无效,1有效
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public IList InvalidateAccountTitleTree(AccountTitleTree pbs)
        {
            nodeSrv.InvalidateNode(pbs);
            IList lstNodes = new ArrayList();
            lstNodes.Add(pbs);
            IList lstChildNodes = nodeSrv.GetALLChildNodes(pbs);
            foreach (AccountTitleTree o in lstChildNodes)
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
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof(AccountTitleTree));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            AccountTitleTree root = new AccountTitleTree();
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
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof(AccountTitleTree));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof(BusinessOperators), aPerson.Id) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            AccountTitleTree root = new AccountTitleTree();
            root.Name = treeName;
            root.TheTree = tree;
            root.CreateDate = System.DateTime.Now;
            nodeSrv.AddRoot(root);

            tree.RootId = root.Id;
            treeService.SaveCategoryTree(tree);
            return tree;
        }
        #endregion

        #region 会计科目树(查询)
        /// <summary>
        /// 获取成本核算科目树节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetAccountTitleTrees(Type t)
        {
            //CategoryTree tree = InitTree("成本核算科目树", typeof(CostAccountSubject));
            ObjectQuery oq = new ObjectQuery();
            //oq.AddFetchMode("TheTree", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;
        }

        /// <summary>
        /// 获取成本核算科目树节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetAccountTitleTreeByInstance()
        {
            //CategoryTree tree = InitTree("成本核算科目树", typeof(CostAccountSubject));

            #region 获取有权限和无权限的节点
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            //oq.AddOrder(Order.Asc("Level"));
            //oq.AddOrder(Order.Asc("OrderNo"));
            //IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(CostAccountSubject), oq);
            #endregion

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            IList list = dao.ObjectQuery(typeof(AccountTitleTree), oq);

            return list;
        }
        public IList GetAccountTitleTreeByInstance(string sRootCode)
        {
            ObjectQuery oQuery = null;
            ObjectQuery oq = new ObjectQuery();
            if (!string.IsNullOrEmpty(sRootCode))
            {
                oQuery=new ObjectQuery();
                oQuery.AddCriterion(Expression.Eq("Code", sRootCode));
                oQuery.AddCriterion(Expression.Eq("State", 1));
                IList lst = Dao.ObjectQuery(typeof(AccountTitleTree), oQuery);
                if (lst != null && lst.Count > 0)
                {
                    oq.AddCriterion(Expression.Like("SysCode", (lst[0] as AccountTitleTree).SysCode, MatchMode.Start));
                }
            }
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            IList list = Dao.ObjectQuery(typeof(AccountTitleTree), oq);

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
        [TransManager]
        public IList SaveAccountTitleTrees(IList lst)
        {
            IList list = new ArrayList();
            foreach (AccountTitleTree var in lst)
            {
                list.Add(SaveAccountTitleTree(var));
            }
            return list;
        }
        /// <summary>
        /// 判断成本核算科目树是否存在,存在返回,不存在添加然后返回
        /// </summary>
        [TransManager]
        public AccountTitleTree GetAccountTitleTree(AccountTitleTree orgCat)
        {
            AccountTitleTree opeOrg = new AccountTitleTree();
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

            IList lst = dao.ObjectQuery(typeof(AccountTitleTree), oq);
            if (lst.Count > 0)
            {
                opeOrg = lst[0] as AccountTitleTree;
                return opeOrg;
            }
            else
            {
                orgCat = SaveAccountTitleTree(orgCat);
                return orgCat;
            }
        }

        /// <summary>
        /// 获取成本核算科目树及岗位节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetAccountTitleTreeAndJobs(Type t)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;
        }

        /// <summary>
        /// 获取成本核算科目树及岗位节点集合(把成本核算科目树的ChildNodes也eager出来)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetAccountTitleTreeAndJobs()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(AccountTitleTree), oq);
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
        /// 获取成本核算科目树及岗位节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetOpeOrgAndJobsByInstance()
        {
            ObjectQuery oq = new ObjectQuery();
            //直接eager岗位，实例权限不起作用
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(AccountTitleTree), oq);
            IList lstInstance = list[1] as IList;
            IList listTree = list[0] as IList;
            return list;
        }

        public IList GetCostAccountSubjects(Type t, ObjectQuery oq)
        {
            return nodeSrv.GetNodesByObjectQuery(t, oq);
        }


        /// <summary>
        /// 获取当前节点下面的所有子节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        public IList GetALLChildNodes(AccountTitleTree pbs)
        {
            return nodeSrv.GetALLChildNodes(pbs);
        }

        /// <summary>
        /// 根据ID获取成本核算科目树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountTitleTree GetAccountTitleTreeById(string id)
        {
            return nodeSrv.GetCategoryNodeById(id, typeof(AccountTitleTree)) as AccountTitleTree;
        }

        public IList GetAccountTitleTreeByQuery(ObjectQuery oQuery)
        {
            return Dao.ObjectQuery(typeof(AccountTitleTree), oQuery);
        }
        #endregion

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now);
        }
        public Hashtable GetBasicAccountTitleTree()
        {
            Hashtable hs = new Hashtable();
            //财务费用 = 6603,短期借款 = 2001, 利润 = 4103,货币上交 = 4104
            string[] arrCode = { "6603", "2001", "4103", "4104" };
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.In("Code", arrCode));
            IList lst = GetAccountTitleTreeByQuery(oQuery);
            string sName = string.Empty;
            foreach (AccountTitleTree oAccountTitleTree in lst)
            {
                if (oAccountTitleTree.Code == arrCode[0])
                {
                    sName = "财务费用";
                }
                else if (oAccountTitleTree.Code == arrCode[1])
                {
                    sName = "短期借款";
                }
                else if (oAccountTitleTree.Code == arrCode[2])
                {
                    sName = "利润上交";
                }
                else if (oAccountTitleTree.Code == arrCode[3])
                {
                    sName = "货币上交";
                }
                else
                {
                    sName = string.Empty;
                }
                if (!string.IsNullOrEmpty(sName))
                {
                    hs.Add(sName, oAccountTitleTree);
                }
            }
            return hs;
        }
    }
    
}
