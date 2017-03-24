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
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Resource.CommonClass.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using CommonSearchLib.BillCodeMng.Service;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    /// <summary>
    /// GWBS服务
    /// </summary>
    public class GWBSTreeSrv : IGWBSTreeSrv
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

        #region GWBS树(增,删,改)
        /// <summary>
        /// 添加GWBS树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree SaveGWBSTree(GWBSTree childNode)
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

                childNode.OwnerGUID = operId;

                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
                childNode.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

                nodeSrv.AddChildNode(childNode);
            }
            else
                nodeSrv.UpdateCategoryNode(childNode);
            return childNode;
        }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveGWBSTreeRootNode(IList lst)
        {
            GWBSTree node = lst[0] as GWBSTree;

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
            oq.AddCriterion(Expression.Eq("Name", "工程WBS树"));
            IList list = dao.ObjectQuery(typeof(CategoryTree), oq);
            if (list.Count == 0)
            {
                tree = new CategoryTree();
                tree.CreateDate = DateTime.Now;
                tree.Author = author;
                tree.Code = ClassUtil.GetFullNameAndAssembly(node.GetType());
                tree.Name = "工程WBS树";
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
            node.CategoryNodeType = NodeType.RootNode;
            node.Level = 1;
            node.OrderNo = 0;

            node.Author = author;

            node.OwnerGUID = operId;
            node.OwnerName = node.Author.PerName;

            Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
            node.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

            node.TheTree = tree;

            dao.SaveOrUpdate(node);

            if (!string.IsNullOrEmpty(node.Id))
                node.SysCode = node.Id + ".";

            dao.SaveOrUpdate(node);

            lst[0] = node;

            for (int i = 1; i < lst.Count; i++)
            {
                lst[i] = SaveGWBSTree(lst[i] as GWBSTree);
            }

            return lst;
        }

        /// <summary> 
        /// 保存一个节点加上其子节点集合
        /// </summary>
        [TransManager]
        public IList SaveGWBSTrees(IList lst)
        {
            IList list = new ArrayList();
            foreach (GWBSTree var in lst)
            {
                list.Add(SaveGWBSTree(var));
            }
            return list;

            //IList list = new ArrayList();
            //foreach (GWBSTree childNode in lst)
            //{
            //    childNode.UpdatedDate = DateTime.Now;

            //    if (childNode.Id == null)
            //    {
            //        childNode.CreateDate = DateTime.Now;
            //        string operId = "";
            //        if (childNode.Author == null)
            //        {
            //            try
            //            {
            //                operId = SecurityUtil.GetLogOperId();
            //            }
            //            catch { }
            //        }
            //        else
            //        {
            //            operId = childNode.Author.Id;
            //        }
            //        childNode.Author = Dao.Get(typeof(BusinessOperators), operId) as IBusinessOperators;
            //    }

            //    dao.SaveOrUpdate(childNode);

            //    list.Add(childNode);
            //}
            //return list;
        }

        /// <summary>
        /// 插入（部分属性固定）或修改WBS树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree InsertOrUpdateWBSTree(GWBSTree childNode)
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

                childNode.OwnerGUID = operId;

                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
                childNode.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

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
        /// 插入（部分属性固定）或修改WBS节点集合
        /// </summary>
        [TransManager]
        public IList InsertOrUpdateWBSTrees(IList lst)
        {
            IList list = new ArrayList();
            foreach (GWBSTree var in lst)
            {
                list.Add(InsertOrUpdateWBSTree(var));
            }
            return list;
        }

        /// <summary>
        /// 删除GWBS树节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteGWBSTree(GWBSTree pbs)
        {
            pbs = dao.Get(typeof(GWBSTree), pbs.Id) as GWBSTree;
            return nodeSrv.DeleteCategoryNode(pbs);
        }

        [TransManager]
        public bool DeleteGWBSTree(IList list)
        {
            foreach (GWBSTree node in list)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ParentNode.Id", node.Id));
                IList listChild = dao.ObjectQuery(typeof(GWBSTree), oq);

                if (listChild.Count > 0)
                {
                    DeleteGWBSTree(listChild);
                }

                GWBSTree tempNode = dao.Get(typeof(GWBSTree), node.Id) as GWBSTree;
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
        public GWBSTree MoveGWBSTree(GWBSTree pbs, GWBSTree toPbs)
        {
            //nodeSrv.MoveNode(pbs, toPbs);

            //修改原始父节点属性
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", pbs.ParentNode.Id));
            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (list != null && list.Count == 1)
            {
                GWBSTree oldParent = dao.Get(typeof(GWBSTree), pbs.ParentNode.Id) as GWBSTree;
                oldParent.CategoryNodeType = NodeType.LeafNode;

                dao.SaveOrUpdate(oldParent);
            }

            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("Id", toPbs.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            oq.AddFetchMode("ListRelaPBS", FetchMode.Eager);
            //oq.AddFetchMode("BearOrgGUID", FetchMode.Eager);
            //oq.AddFetchMode("ProjectTaskTypeGUID", FetchMode.Eager);
            list = dao.ObjectQuery(typeof(GWBSTree), oq);
            toPbs = list[0] as GWBSTree;

            //修改自身属性
            pbs = dao.Get(typeof(GWBSTree), pbs.Id) as GWBSTree;
            pbs.ParentNode = toPbs;
            pbs.Level = toPbs.Level + 1;
            pbs.SysCode = toPbs.SysCode + pbs.Id + ".";

            pbs.OrderNo = GetMaxOrderNo(pbs, null) + 1;
            pbs.OwnerGUID = toPbs.OwnerGUID;
            pbs.OwnerName = toPbs.OwnerName;
            pbs.OwnerOrgSysCode = toPbs.OwnerOrgSysCode;
            pbs.BearOrgGUID = toPbs.BearOrgGUID;
            pbs.BearOrgName = toPbs.BearOrgName;
            pbs.ProjectTaskTypeGUID = toPbs.ProjectTaskTypeGUID;
            pbs.ProjectTaskTypeName = toPbs.ProjectTaskTypeName;

            pbs.ListRelaPBS.Clear();
            foreach (GWBSRelaPBS rela in toPbs.ListRelaPBS)
            {
                GWBSRelaPBS newRela = new GWBSRelaPBS();
                newRela.ThePBS = rela.ThePBS;
                newRela.PBSName = rela.PBSName;

                newRela.TheGWBSTree = pbs;
                pbs.ListRelaPBS.Add(newRela);
            }
            pbs.TheProjectGUID = toPbs.TheProjectGUID;
            pbs.TheProjectName = toPbs.TheProjectName;
            pbs.TaskState = GWBSTreeState.编制;
            pbs.TaskStateTime = DateTime.Now;
            pbs.Summary = toPbs.Summary + "," + pbs.Name;


            toPbs.ChildNodes.Add(pbs);


            if (toPbs.CategoryNodeType == NodeType.LeafNode)//如果目的节点是叶节点设为中间节点  0、1、2
            {
                toPbs.CategoryNodeType = NodeType.MiddleNode;
            }
            dao.SaveOrUpdate(toPbs);

            //更新所有子节点的属性
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", pbs.Id));
            IList listChild = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (listChild.Count > 0)
            {
                for (int i = 0; i < listChild.Count; i++)
                {
                    GWBSTree childNode = listChild[i] as GWBSTree;

                    childNode.SysCode = pbs.SysCode + childNode.Id + ".";
                    childNode.Level = pbs.Level + 1;
                    //childNode.OrderNo = GetMaxOrderNo(pbs, null) + 1;
                    childNode.OwnerGUID = pbs.OwnerGUID;
                    childNode.OwnerName = pbs.OwnerName;
                    childNode.OwnerOrgSysCode = pbs.OwnerOrgSysCode;
                    childNode.BearOrgGUID = pbs.BearOrgGUID;
                    childNode.BearOrgName = pbs.BearOrgName;
                    childNode.ProjectTaskTypeGUID = pbs.ProjectTaskTypeGUID;
                    childNode.ProjectTaskTypeName = pbs.ProjectTaskTypeName;

                    childNode.ListRelaPBS.Clear();
                    foreach (GWBSRelaPBS rela in pbs.ListRelaPBS)
                    {
                        GWBSRelaPBS newRela = new GWBSRelaPBS();
                        newRela.ThePBS = rela.ThePBS;
                        newRela.PBSName = rela.PBSName;

                        newRela.TheGWBSTree = childNode;
                        childNode.ListRelaPBS.Add(newRela);
                    }
                    childNode.TheProjectGUID = pbs.TheProjectGUID;
                    childNode.TheProjectName = pbs.TheProjectName;

                    childNode.TaskState = GWBSTreeState.编制;
                    childNode.TaskStateTime = DateTime.Now;
                    childNode.Summary = pbs.Summary + "," + childNode.Name;


                    listChild[i] = childNode;
                    //dao.SaveOrUpdate(childNode);

                    UpdateChildNodes(childNode);
                }

                dao.SaveOrUpdate(listChild);
            }

            return pbs;
        }

        /// <summary>
        /// 移动节点并修改移动节点及子节点契约信息
        /// </summary>
        /// <param name="pbs"></param>
        /// <param name="toOrg"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree MoveGWBSTreeAndUpdateContract(GWBSTree pbs, GWBSTree toPbs, ContractGroup group)
        {
            //修改原始父节点属性
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", pbs.ParentNode.Id));
            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (list != null && list.Count == 1)
            {
                GWBSTree oldParent = dao.Get(typeof(GWBSTree), pbs.ParentNode.Id) as GWBSTree;
                oldParent.CategoryNodeType = NodeType.LeafNode;

                dao.SaveOrUpdate(oldParent);
            }

            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("Id", toPbs.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            list = dao.ObjectQuery(typeof(GWBSTree), oq);
            toPbs = list[0] as GWBSTree;

            //修改自身属性
            pbs = dao.Get(typeof(GWBSTree), pbs.Id) as GWBSTree;
            pbs.ParentNode = toPbs;
            pbs.Level = toPbs.Level + 1;
            pbs.SysCode = toPbs.SysCode + pbs.Id + ".";

            //if (group != null)
            //{
            //    pbs.ContractGroupGUID = group;
            //    pbs.ContractGroupCode = group.Code;
            //}

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
            IList listChild = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (listChild.Count > 0)
            {
                for (int i = 0; i < listChild.Count; i++)
                {
                    GWBSTree childNode = listChild[0] as GWBSTree;
                    childNode.SysCode = pbs.SysCode + childNode.Id + ".";
                    //if (group != null)
                    //{
                    //    pbs.ContractGroupGUID = group;
                    //    pbs.ContractGroupCode = group.Code;
                    //}

                    dao.SaveOrUpdate(childNode);

                    UpdateChildNodes(childNode, group);
                }
            }

            return pbs;
        }

        private void UpdateChildNodes(GWBSTree parentNode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", parentNode.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            parentNode = dao.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;
            if (parentNode.ChildNodes.Count > 0)
            {
                IList listChild = parentNode.ChildNodes;
                for (int i = 0; i < listChild.Count; i++)
                {
                    GWBSTree childNode = listChild[i] as GWBSTree;
                    childNode.SysCode = parentNode.SysCode + childNode.Id + ".";
                    childNode.Level = parentNode.Level + 1;
                    //childNode.OrderNo = GetMaxOrderNo(pbs, null) + 1;
                    childNode.OwnerGUID = parentNode.OwnerGUID;
                    childNode.OwnerName = parentNode.OwnerName;
                    childNode.OwnerOrgSysCode = parentNode.OwnerOrgSysCode;
                    childNode.BearOrgGUID = parentNode.BearOrgGUID;
                    childNode.BearOrgName = parentNode.BearOrgName;
                    childNode.ProjectTaskTypeGUID = parentNode.ProjectTaskTypeGUID;
                    childNode.ProjectTaskTypeName = parentNode.ProjectTaskTypeName;

                    childNode.ListRelaPBS.Clear();
                    foreach (GWBSRelaPBS rela in parentNode.ListRelaPBS)
                    {
                        GWBSRelaPBS newRela = new GWBSRelaPBS();
                        newRela.ThePBS = rela.ThePBS;
                        newRela.PBSName = rela.PBSName;

                        newRela.TheGWBSTree = childNode;
                        childNode.ListRelaPBS.Add(newRela);
                    }
                    childNode.TheProjectGUID = parentNode.TheProjectGUID;
                    childNode.TheProjectName = parentNode.TheProjectName;

                    childNode.TaskState = GWBSTreeState.编制;
                    childNode.TaskStateTime = DateTime.Now;
                    childNode.Summary = parentNode.Summary + "," + childNode.Name;


                    listChild[i] = childNode;
                    //dao.SaveOrUpdate(childNode);

                    UpdateChildNodes(childNode);
                }
            }
        }
        private void UpdateChildNodes(GWBSTree parentNode, ContractGroup group)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", parentNode.Id));
            IList listChild = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (listChild.Count > 0)
            {
                for (int i = 0; i < listChild.Count; i++)
                {
                    GWBSTree childNode = listChild[0] as GWBSTree;
                    childNode.SysCode = parentNode.SysCode + childNode.Id + ".";

                    //if (group != null)
                    //{
                    //    childNode.ContractGroupGUID = group;
                    //    childNode.ContractGroupCode = group.Code;
                    //}

                    dao.SaveOrUpdate(childNode);

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
        public IList InvalidateGWBSTree(GWBSTree pbs)
        {
            nodeSrv.InvalidateNode(pbs);
            IList lstNodes = new ArrayList();
            lstNodes.Add(pbs);
            IList lstChildNodes = nodeSrv.GetALLChildNodes(pbs);
            foreach (GWBSTree o in lstChildNodes)
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
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof(GWBSTree));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            GWBSTree root = new GWBSTree();
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
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof(GWBSTree));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof(BusinessOperators), aPerson.Id) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            GWBSTree root = new GWBSTree();
            root.Name = treeName;
            root.TheTree = tree;
            root.CreateDate = System.DateTime.Now;
            nodeSrv.AddRoot(root);

            tree.RootId = root.Id;
            treeService.SaveCategoryTree(tree);
            return tree;
        }

        /// <summary>
        /// 初始化GWBS树规则
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool InitRule()
        {
            //CategoryTree tree = InitTree("GWBS树", typeof(GWBSTree));
            ////CategoryTree tree = treeService.GetCategoryTreeByType(typeof(GWBSTree));
            //BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            //CategoryRule rule = new CategoryRule();
            //rule.Name = "销售GWBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly(typeof(GWBSTree));
            //rule.ChildNodeType =  ClassUtil.GetFullNameAndAssembly(typeof(SellGWBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "采购GWBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType =  ClassUtil.GetFullNameAndAssembly(typeof(GWBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchaseGWBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "仓储GWBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(GWBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(StorageGWBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "销售GWBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(SellGWBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(SellGWBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "采购GWBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchaseGWBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchaseGWBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "仓储GWBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(StorageGWBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly(typeof(StorageGWBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);
            throw new Exception("GWBS树无规则！");
            return true;
        }
        #endregion

        #region GWBS树(查询)
        /// <summary>
        /// 获取GWBS树节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetGWBSTrees(Type t)
        {
            //CategoryTree tree = InitTree("GWBS树", typeof(GWBSTree));
            ObjectQuery oq = new ObjectQuery();
            //oq.AddFetchMode("TheTree", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;
        }

        /// <summary>
        /// 获取GWBS树节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetGWBSTreesByInstance(string projectId)
        {
            //CategoryTree tree = InitTree("GWBS树", typeof(GWBSTree));

            #region 获取有权限和无权限的节点

            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            ////oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            //oq.AddOrder(Order.Asc("Level"));
            //oq.AddOrder(Order.Asc("OrderNo"));
            //IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(GWBSTree), oq);

            #endregion

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddOrder(Order.Asc("SysCode"));
            //oq.AddOrder(Order.Asc("CategoryNodeType"));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);

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
        /// 判断GWBS树是否存在,存在返回,不存在添加然后返回
        /// </summary>
        [TransManager]
        public GWBSTree GetGWBSTree(GWBSTree orgCat)
        {
            GWBSTree opeOrg = new GWBSTree();
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

            IList lst = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (lst.Count > 0)
            {
                opeOrg = lst[0] as GWBSTree;
                return opeOrg;
            }
            else
            {
                orgCat = SaveGWBSTree(orgCat);
                return orgCat;
            }
        }

        /// <summary>
        /// 获取GWBS树及岗位节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetGWBSTreeAndJobs(Type t)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;
        }

        /// <summary>
        /// 获取GWBS树及岗位节点集合(把GWBS树的ChildNodes也eager出来)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetGWBSTreeAndJobs()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof(GWBSTree), oq);
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
        /// 获取GWBS树及岗位节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetOpeOrgAndJobsByInstance()
        {
            ObjectQuery oq = new ObjectQuery();
            //直接eager岗位，实例权限不起作用
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(GWBSTree), oq);
            IList lstInstance = list[1] as IList;
            IList listTree = list[0] as IList;
            //foreach (GWBSTree childNode in listTree)
            //{
            //    childNode.OperationJobs = new ArrayList();
            //    if (Contains(lstInstance, childNode))
            //    {
            //        oq = new ObjectQuery();
            //        oq.AddCriterion(Expression.Eq("GWBSTree.Id", childNode.Id));
            //        IList jobList = dao.ObjectQuery(typeof(OperationJob), oq);
            //        (childNode.OperationJobs as ArrayList).AddRange(jobList);
            //    }
            //}
            return list;
        }

        public IList GetGWBSTrees(Type t, ObjectQuery oq)
        {
            return nodeSrv.GetNodesByObjectQuery(t, oq);
        }


        /// <summary>
        /// 获取当前节点下面的所有子节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        public IList GetALLChildNodes(GWBSTree pbs)
        {
            return nodeSrv.GetALLChildNodes(pbs);
        }

        /// <summary>
        /// 根据ID获取GWBS树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GWBSTree GetGWBSTreeById(string id)
        {
            return nodeSrv.GetCategoryNodeById(id, typeof(GWBSTree)) as GWBSTree;
        }

        #endregion

        #region 树规则

        /// <summary>
        /// 根据树类型获取所有规则集合
        /// </summary>
        /// <param name="treeType"></param>
        /// <returns></returns>
        public IList GetGWBSTreeRules(Type treeType)
        {
            CategoryTree tree = InitTree("GWBS树", typeof(GWBSTree));
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
        public IList GetGWBSTreeRules(Type treeType, Type parentNodeType)
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
            string sql = "Select MAX(OPGCODE) Code from ResGWBSTree";
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
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return dao.Get(entityType, Id);
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

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 保存或更新工程任务对象集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList SaveOrUpdateGWBSTree(IList list)
        {

            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 保存或更新GWBS,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, IList<GWBSDetailLedger> listLedger)
        {
            dao.SaveOrUpdate(wbs);

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            return wbs;
        }

        /// <summary>
        /// 保存或更新工程WBS集合,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点集合</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateGWBSTree(IList list, IList<GWBSDetailLedger> listLedger)
        {
            dao.SaveOrUpdate(list);

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            return list;
        }

        /// <summary>
        /// 保存或修改工程WBS明细
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <returns></returns>
        [TransManager]
        public GWBSDetail SaveOrUpdateDetail(GWBSDetail dtl)
        {
            dtl.UpdatedDate = DateTime.Now;

            dao.SaveOrUpdate(dtl);

            return dtl;
        }


        /// <summary>
        /// 保存或修改任务明细和父对象
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="parentNode">父对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        [TransManager]
        public GWBSDetail SaveOrUpdateDetail(GWBSDetail dtl, ref GWBSTree parentNode, IList listLedger)
        {

            parentNode.UpdatedDate = DateTime.Now;

            dao.SaveOrUpdate(parentNode);

            if (parentNode != null)
            {
                dtl.UpdatedDate = DateTime.Now;

                dao.SaveOrUpdate(dtl);
            }

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            return dtl;
        }

        /// <summary>
        /// 保存或修改工程WBS明细集合
        /// </summary>
        /// <param name="list">明细集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateDetail(IList list)
        {
            IList lst = new ArrayList();
            foreach (GWBSDetail dtl in list)
            {
                lst.Add(SaveOrUpdateDetail(dtl));
            }
            return lst;
        }

        /// <summary>
        /// 删除工程WBS明细集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteDetail(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (GWBSDetail dtl in list)
            {
                dis.Add(Expression.Eq("Id", dtl.Id));
            }
            oq.AddCriterion(dis);
            list = dao.ObjectQuery(typeof(GWBSDetail), oq);
            return dao.Delete(list);
        }

        /// <summary>
        /// 保存或修改工程WBS明细集合
        /// </summary>
        /// <param name="list">明细集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateCostSubject(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 删除明细分科目成本集合
        /// </summary>
        /// <param name="list">分科目成本集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteCostSubject(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (GWBSDetailCostSubject dtl in list)
            {
                dis.Add(Expression.Eq("Id", dtl.Id));
            }
            oq.AddCriterion(dis);
            list = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
            return dao.Delete(list);
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
    }
}
