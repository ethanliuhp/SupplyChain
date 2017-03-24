﻿using System;
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
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Service;
using System.Data.OracleClient;
using Application.Business.Erp.SupplyChain.Base.Service;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Service
{
    /// <summary>
    /// PBS服务
    /// </summary>
    public class PBSTreeSrv : BaseService, IPBSTreeSrv
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

        private IStockInSrv _stockInSrv;

        public IStockInSrv StockInSrv
        {
            get { return _stockInSrv; }
            set { _stockInSrv = value; }
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
            //session.Transaction.Enlist(cmd); 
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            IDataReader dr = cmd.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }


        #endregion

        #region PBS树(增,删,改)

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SavePBSTreeRootNode(IList lst)
        {
            PBSTree node = lst[0] as PBSTree;

            string operId = "";
            if (node.Author == null)
            {
                try
                {
                    operId = SecurityUtil.GetLogOperId();
                }
                catch
                {
                }
            }
            else
            {
                operId = node.Author.Id;
            }
            IBusinessOperators author = Dao.Get(typeof (BusinessOperators), operId) as IBusinessOperators;


            CategoryTree tree = null;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Name", "PBS树"));
            IList list = dao.ObjectQuery(typeof (CategoryTree), oq);
            if (list.Count == 0)
            {
                tree = new CategoryTree();
                tree.CreateDate = DateTime.Now;
                tree.Author = author;
                tree.Code = ClassUtil.GetFullNameAndAssembly(node.GetType());
                tree.Name = "PBS树";
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
                lst[i] = SavePBSTree(lst[i] as PBSTree);
            }

            return lst;
        }

        /// <summary>
        /// 保存PBS树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public PBSTree SavePBSTree(PBSTree childNode)
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
                    catch
                    {
                    }
                }
                else
                {
                    operId = childNode.Author.Id;
                }
                childNode.Author = Dao.Get(typeof (BusinessOperators), operId) as IBusinessOperators;

                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
                childNode.OwnerGUID = login.ThePerson.Id;
                childNode.OwnerName = login.ThePerson.Name;
                childNode.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

                PBSTree parentNode = dao.Get(typeof (PBSTree), childNode.ParentNode.Id) as PBSTree;
                if (parentNode.CategoryNodeType != NodeType.RootNode)
                {
                    parentNode.CategoryNodeType = NodeType.MiddleNode;
                    dao.Update(parentNode);
                }

                childNode.Level = parentNode.Level + 1;
                childNode.CategoryNodeType = NodeType.LeafNode;

                dao.Save(childNode);

                childNode.SysCode = parentNode.SysCode + childNode.Id + ".";

                dao.Update(childNode);
            }
            else
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = "select t1.name,t1.fullpath from thd_pbstree t1 where t1.id='" + childNode.Id + "'";
                command.CommandText = sql;
                IDataReader dr = command.ExecuteReader();
                DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

                string oldName = ds.Tables[0].Rows[0]["name"].ToString();
                string fullpath = ds.Tables[0].Rows[0]["fullpath"].ToString();

                if (oldName != childNode.Name)
                {
                    //修改下属子节点完整路径
                    sql = "update thd_pbstree t1 set t1.fullpath=replace(t1.fullpath,'" + fullpath + "','" +
                          childNode.FullPath + "') where t1.syscode like '" + childNode.SysCode + "%' and t1.id!='" +
                          childNode.Id + "'";

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

                dao.Update(childNode);
            }
            return childNode;
        }

        /// <summary> 
        /// 保存一个节点加上其子节点集合
        /// </summary>
        [TransManager]
        public IList SavePBSTrees(IList list)
        {
            if (list == null || list.Count == 0)
                return list;

            PBSTree targetParentNode = list[0] as PBSTree;
            dao.Update(targetParentNode);

            CurrentProjectInfo projectInfo =
                dao.Get(typeof (CurrentProjectInfo), targetParentNode.TheProjectGUID) as CurrentProjectInfo;

            List<PBSTree> listChild = new List<PBSTree>();

            IBusinessOperators author =
                Dao.Get(typeof (BusinessOperators), (list[1] as PBSTree).OwnerGUID) as IBusinessOperators;
            List<BasicDataOptr> listStructureType =
                StockInSrv.GetBasicDataByParentName("结构类型").OfType<BasicDataOptr>().ToList();

            for (int i = 1; i < list.Count; i++)
            {
                PBSTree pbs = list[i] as PBSTree;

                if (string.IsNullOrEmpty(pbs.Code))
                {
                    //系统生成编码
                    var queryType = from t in listStructureType
                                    where t.Id == pbs.StructTypeGUID
                                    select t;

                    pbs.Code = projectInfo.Code.PadLeft(4, '0') + "-" + queryType.ElementAt(0).BasicCode + "-" +
                               GetCode(typeof (PBSTree));
                }

                pbs.Author = author;
                listChild.Add(pbs);
            }

            dao.Save(listChild);

            var query = from c in listChild
                        where c.ParentNode != null && c.ParentNode.Id == targetParentNode.Id
                        select c;

            foreach (PBSTree g in query)
            {
                g.SysCode = g.ParentNode.SysCode + g.Id + ".";

                updateSyscode(listChild, g);
            }

            dao.Update(listChild);

            return list;
        }

        [TransManager]
        public IList PBSTreeOrder(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (PBSTree wbs in list)
            {
                dis.Add(Expression.Eq("Id", wbs.Id));
            }
            oq.AddCriterion(dis);
            IList listNew = dao.ObjectQuery(typeof (PBSTree), oq);


            IEnumerable<PBSTree> listQuery = list.OfType<PBSTree>();
            foreach (PBSTree wbs in listNew)
            {
                var query = from w in listQuery
                            where w.Id == wbs.Id
                            select w;

                if (query.Count() > 0)
                    wbs.OrderNo = query.ElementAt(0).OrderNo;
            }

            listNew = listNew.OfType<PBSTree>().OrderBy(w => w.OrderNo).ToArray();

            dao.Update(listNew);

            return listNew;

        }

        private void updateSyscode(List<PBSTree> listChild, PBSTree parent)
        {
            var query = from g in listChild
                        where g.ParentNode != null && g.ParentNode.Id == parent.Id
                        select g;

            foreach (PBSTree g in query)
            {
                g.SysCode = g.ParentNode.SysCode + g.Id + ".";

                updateSyscode(listChild, g);
            }
        }

        /// <summary>
        /// 插入（部分属性固定）或修改PBS树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public PBSTree InsertOrUpdatePBSTree(PBSTree childNode)
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
                    catch
                    {
                    }
                }
                else
                {
                    operId = childNode.Author.Id;
                }
                childNode.Author = Dao.Get(typeof (BusinessOperators), operId) as IBusinessOperators;

                childNode.OwnerGUID = operId;
                childNode.OwnerName = childNode.Author.PerName;

                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
                childNode.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

                long orderNo = childNode.OrderNo;

                nodeSrv.AddChildNode(childNode);

                childNode.OrderNo = orderNo; //添加到节点集合后会自动设置一个最大的排序号
                dao.Update(childNode);
            }
            else
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = "select t1.name,t1.fullpath from thd_pbstree t1 where t1.id='" + childNode.Id + "'";
                command.CommandText = sql;
                IDataReader dr = command.ExecuteReader();
                DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

                string oldName = ds.Tables[0].Rows[0]["name"].ToString();
                string fullpath = ds.Tables[0].Rows[0]["fullpath"].ToString();

                if (oldName != childNode.Name)
                {
                    //修改下属子节点完整路径
                    sql = "update thd_pbstree t1 set t1.fullpath=replace(t1.fullpath,'" + fullpath + "','" +
                          childNode.FullPath + "') where t1.syscode like '" + childNode.SysCode + "%' and t1.id!='" +
                          childNode.Id + "'";

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

                dao.Update(childNode);
            }

            return childNode;
        }

        /// <summary> 
        /// 插入（部分属性固定）或修改PBS节点集合
        /// </summary>
        [TransManager]
        public IList InsertOrUpdatePBSTrees(IList lst)
        {
            IList list = new ArrayList();
            foreach (PBSTree var in lst)
            {
                list.Add(InsertOrUpdatePBSTree(var));
            }
            return list;
        }

        /// <summary>
        /// 删除PBS树节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeletePBSTree(PBSTree pbs)
        {
            //pbs = dao.Get(typeof(PBSTree), pbs.Id) as PBSTree;
            //return nodeSrv.DeleteCategoryNode(pbs);

            try
            {
                if (pbs == null || pbs.SysCode == "")
                    return false;

                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                //1.删除WBS节点及其子节点
                string sql = " delete from thd_pbstree t1 where t1.syscode like '" + pbs.SysCode + "%'";

                command.CommandText = sql;
                command.ExecuteNonQuery();

                //2.更新父节点类型信息
                if (pbs.ParentNode != null)
                {
                    sql = "select count(*) childCount from thd_pbstree t1 where t1.parentnodeid='" + pbs.ParentNode.Id +
                          "'";
                    command.CommandText = sql;
                    int childCount = Convert.ToInt32(command.ExecuteScalar());

                    //根节点=0，枝节点=1，叶节点=2
                    if (childCount == 0)
                        sql = "update thd_pbstree t1 set t1.categorynodetype=2 where t1.id='" + pbs.ParentNode.Id + "'";
                    else
                        sql = "update thd_pbstree t1 set t1.categorynodetype=1 where t1.id='" + pbs.ParentNode.Id + "'";

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        [TransManager]
        public bool DeletePBSTree(IList list)
        {
            //foreach (PBSTree node in list)
            //{
            //    ObjectQuery oq = new ObjectQuery();
            //    oq.AddCriterion(Expression.Eq("ParentNode.Id", node.Id));
            //    IList listChild = dao.ObjectQuery(typeof(PBSTree), oq);

            //    if (listChild.Count > 0)
            //    {
            //        DeletePBSTree(listChild);
            //    }

            //    PBSTree tempNode = dao.Get(typeof(PBSTree), node.Id) as PBSTree;
            //    if (tempNode != null)
            //        dao.Delete(tempNode);
            //}

            foreach (PBSTree pbs in list)
            {
                DeletePBSTree(pbs);
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
        public Hashtable MovePBSTree(PBSTree pbs, PBSTree toPbs)
        {

            Hashtable listResult = new Hashtable();

            //修改原始父节点属性
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", pbs.ParentNode.Id));
            IList list = dao.ObjectQuery(typeof (PBSTree), oq);
            if (list != null && list.Count == 1)
            {
                PBSTree oldParent = dao.Get(typeof (PBSTree), pbs.ParentNode.Id) as PBSTree;
                oldParent.CategoryNodeType = NodeType.LeafNode;

                dao.Update(oldParent);

                listResult.Add(oldParent.Id, oldParent);
            }

            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("Id", toPbs.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            list = dao.ObjectQuery(typeof (PBSTree), oq);
            toPbs = list[0] as PBSTree;

            IBusinessOperators author = Dao.Get(typeof (BusinessOperators), pbs.OwnerGUID) as IBusinessOperators;
            string ownerGUID = pbs.OwnerGUID;
            string ownerName = pbs.OwnerName;
            string ownerOrgSysCode = pbs.OwnerOrgSysCode;

            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.AddCriterion(Expression.Like("SysCode", pbs.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Not(Expression.Eq("Id", pbs.Id)));
            List<PBSTree> listChild = dao.ObjectQuery(typeof (PBSTree), oq).OfType<PBSTree>().ToList();

            //修改自身属性
            pbs = dao.Get(typeof (PBSTree), pbs.Id) as PBSTree;
            pbs.ParentNode = toPbs;
            pbs.Level = toPbs.Level + 1;
            pbs.SysCode = toPbs.SysCode + pbs.Id + ".";
            pbs.FullPath = toPbs.FullPath + @"\" + pbs.Name;

            pbs.OrderNo = GetMaxOrderNo(pbs) + 1;
            pbs.OwnerGUID = ownerGUID;
            pbs.OwnerName = ownerName;
            pbs.OwnerOrgSysCode = ownerOrgSysCode;
            pbs.Author = author;

            pbs.TheProjectGUID = toPbs.TheProjectGUID;
            pbs.TheProjectName = toPbs.TheProjectName;

            toPbs.ChildNodes.Add(pbs);

            if (toPbs.CategoryNodeType != NodeType.RootNode) //如果目的节点是叶节点设为中间节点  0、1、2
                toPbs.CategoryNodeType = NodeType.MiddleNode;

            pbs.CategoryNodeType = NodeType.LeafNode;

            dao.SaveOrUpdate(toPbs);

            listResult.Add(toPbs.Id, toPbs);
            listResult.Add(pbs.Id, pbs);

            //更新所有子节点的属性，新的父节点的id变化，下属所有节点的层次码均改变
            if (listChild.Count > 0)
            {
                var query = from c in listChild
                            where c.ParentNode != null && c.ParentNode.Id == pbs.Id
                            select c;

                foreach (PBSTree childNode in query)
                {
                    childNode.SysCode = pbs.SysCode + childNode.Id + ".";
                    childNode.Level = pbs.Level + 1;
                    childNode.FullPath = pbs.FullPath + @"\" + childNode.Name;

                    childNode.OwnerGUID = ownerGUID;
                    childNode.OwnerName = ownerName;
                    childNode.OwnerOrgSysCode = ownerOrgSysCode;
                    childNode.Author = author;

                    childNode.TheProjectGUID = pbs.TheProjectGUID;
                    childNode.TheProjectName = pbs.TheProjectName;

                    childNode.ParentNode.CategoryNodeType = NodeType.MiddleNode;
                    childNode.CategoryNodeType = NodeType.LeafNode;

                    UpdateChildNodes(childNode, listChild);
                }

                dao.Update(listChild);

                foreach (PBSTree childNode in listChild)
                {
                    listResult.Add(childNode.Id, childNode);
                }
            }

            return listResult;
        }

        private void UpdateChildNodes(PBSTree parentNode, List<PBSTree> listChild)
        {
            var query = from c in listChild
                        where c.ParentNode != null && c.ParentNode.Id == parentNode.Id
                        select c;

            foreach (PBSTree childNode in query)
            {
                childNode.SysCode = parentNode.SysCode + childNode.Id + ".";
                childNode.Level = parentNode.Level + 1;
                childNode.FullPath = parentNode.FullPath + @"\" + childNode.Name;
                //childNode.OrderNo = GetMaxOrderNo(pbs, null) + 1;
                childNode.OwnerGUID = parentNode.OwnerGUID;
                childNode.OwnerName = parentNode.OwnerName;
                childNode.OwnerOrgSysCode = parentNode.OwnerOrgSysCode;
                childNode.Author = parentNode.Author;

                childNode.TheProjectGUID = parentNode.TheProjectGUID;
                childNode.TheProjectName = parentNode.TheProjectName;


                childNode.ParentNode.CategoryNodeType = NodeType.MiddleNode;
                childNode.CategoryNodeType = NodeType.LeafNode;

                UpdateChildNodes(childNode, listChild);

            }
        }


        /// <summary>
        /// 失效节点 State ,0 无效,1有效
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public IList InvalidatePBSTree(PBSTree pbs)
        {
            nodeSrv.InvalidateNode(pbs);
            IList lstNodes = new ArrayList();
            lstNodes.Add(pbs);
            IList lstChildNodes = nodeSrv.GetALLChildNodes(pbs);
            foreach (PBSTree o in lstChildNodes)
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
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof (PBSTree));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof (BusinessOperators), 1L) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            PBSTree root = new PBSTree();
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
            CategoryTree tree = treeService.GetCategoryTreeByType(typeof (PBSTree));
            if (tree != null) return tree;
            BusinessOperators op = dao.Get(typeof (BusinessOperators), aPerson.Id) as BusinessOperators;

            tree = new CategoryTree();
            tree.Name = treeName;
            tree.Code = ClassUtil.GetFullNameAndAssembly(treeType);
            tree.MaxLevel = 0;
            tree.Author = op;
            tree.CreateDate = System.DateTime.Now;
            treeService.SaveCategoryTree(tree);

            PBSTree root = new PBSTree();
            root.Name = treeName;
            root.TheTree = tree;
            root.CreateDate = System.DateTime.Now;
            nodeSrv.AddRoot(root);

            tree.RootId = root.Id;
            treeService.SaveCategoryTree(tree);
            return tree;
        }

        /// <summary>
        /// 初始化PBS树规则
        /// </summary>
        /// <returns></returns>
        [TransManager]
        public bool InitRule()
        {
            //CategoryTree tree = InitTree("PBS树", typeof(PBSTree));
            ////CategoryTree tree = treeService.GetCategoryTreeByType(typeof(PBSTree));
            //BusinessOperators op = dao.Get(typeof(BusinessOperators), 1L) as BusinessOperators;

            //CategoryRule rule = new CategoryRule();
            //rule.Name = "销售PBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly(typeof(PBSTree));
            //rule.ChildNodeType =  ClassUtil.GetFullNameAndAssembly(typeof(SellPBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "采购PBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType =  ClassUtil.GetFullNameAndAssembly(typeof(PBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchasePBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "仓储PBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(StoragePBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "销售PBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(SellPBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(SellPBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "采购PBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchasePBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly( typeof(PurchasePBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);

            //rule = new CategoryRule();
            //rule.Name = "仓储PBS树";
            //rule.TheTree = tree;
            //rule.ParentNodeType = ClassUtil.GetFullNameAndAssembly( typeof(StoragePBSTree));
            //rule.ChildNodeType = ClassUtil.GetFullNameAndAssembly(typeof(StoragePBSTree));
            //rule.Author = op;
            //rule.CreateDate = System.DateTime.Today;
            //ruleSrv.SaveCategoryRule(rule);
            throw new Exception("PBS树无规则！");
            return true;
        }

        #endregion

        #region PBS树(查询)

        /// <summary>
        /// 获取PBS树节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetPBSTrees(Type t)
        {
            //CategoryTree tree = InitTree("PBS树", typeof(PBSTree));
            ObjectQuery oq = new ObjectQuery();
            //oq.AddFetchMode("TheTree", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;

        }

        /// <summary>
        /// 获取PBS树节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetPBSTreesByInstance(string projectId)
        {
            //CategoryTree tree = InitTree("PBS树", typeof(PBSTree));

            #region 获取有权限和无权限的节点

            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            ////oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            //oq.AddOrder(Order.Asc("Level"));
            //oq.AddOrder(Order.Asc("OrderNo"));

            //IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(PBSTree), oq);

            #endregion

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddOrder(Order.Asc("SysCode"));
            //oq.AddOrder(Order.Asc("CategoryNodeType"));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            IList list = dao.ObjectQuery(typeof (PBSTree), oq);

            return list;
        }

        /// <summary>
        /// 获取PBS节点集合(返回有权限和无权限的集合)
        /// </summary>
        public DataTable GetPBSTreesInstanceBySql(string projectId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select t1.id,t1.name,t1.syscode,t1.parentnodeid,t1.structtypeguid,t1.structtypename,t1.orderNo,t1.theprojectname,t1.theprojectguid from thd_pbstree t1 where 1=1 and t1.theprojectguid = '" +
                projectId + "' and t1.state = '1'  order by t1.tlevel asc,t1.orderno asc";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet.Tables[0];
        }

        /// <summary>
        /// 获取PBS节点集合
        /// </summary>
        public IList GetPBSTreesByInstance(string projectId, IList listParentPBS)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));

            Disjunction dis = new Disjunction();
            foreach (PBSTree pbs in listParentPBS)
            {
                dis.Add(Expression.Like("SysCode", pbs.SysCode + "%"));
            }
            oq.AddCriterion(dis);

            oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddOrder(Order.Asc("SysCode"));
            //oq.AddOrder(Order.Asc("CategoryNodeType"));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));


            IList list = dao.ObjectQuery(typeof (PBSTree), oq);

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
        /// 判断PBS树是否存在,存在返回,不存在添加然后返回
        /// </summary>
        [TransManager]
        public PBSTree GetPBSTree(PBSTree orgCat)
        {
            PBSTree opeOrg = new PBSTree();
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

            IList lst = dao.ObjectQuery(typeof (PBSTree), oq);
            if (lst.Count > 0)
            {
                opeOrg = lst[0] as PBSTree;
                return opeOrg;
            }
            else
            {
                orgCat = SavePBSTree(orgCat);
                return orgCat;
            }
        }

        /// <summary>
        /// 获取PBS树及岗位节点集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetPBSTreeAndJobs(Type t)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(t, oq);
            return list;
        }

        /// <summary>
        /// 获取PBS树及岗位节点集合(把PBS树的ChildNodes也eager出来)
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IList GetPBSTreeAndJobs()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            IList list = NodeSrv.GetNodesByObjectQuery(typeof (PBSTree), oq);
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
        /// 获取PBS树及岗位节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetOpeOrgAndJobsByInstance()
        {
            ObjectQuery oq = new ObjectQuery();
            //直接eager岗位，实例权限不起作用
            oq.AddFetchMode("OperationJobs", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof (PBSTree), oq);
            IList lstInstance = list[1] as IList;
            IList listTree = list[0] as IList;
            //foreach (PBSTree childNode in listTree)
            //{
            //    childNode.OperationJobs = new ArrayList();
            //    if (Contains(lstInstance, childNode))
            //    {
            //        oq = new ObjectQuery();
            //        oq.AddCriterion(Expression.Eq("PBSTree.Id", childNode.Id));
            //        IList jobList = dao.ObjectQuery(typeof(OperationJob), oq);
            //        (childNode.OperationJobs as ArrayList).AddRange(jobList);
            //    }
            //}
            return list;
        }

        public IList GetPBSTrees(Type t, ObjectQuery oq)
        {
            return nodeSrv.GetNodesByObjectQuery(t, oq);
        }


        /// <summary>
        /// 获取当前节点下面的所有子节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        public IList GetALLChildNodes(PBSTree pbs)
        {
            return nodeSrv.GetALLChildNodes(pbs);
        }

        /// <summary>
        /// 根据ID获取PBS树
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PBSTree GetPBSTreeById(string id)
        {
            return nodeSrv.GetCategoryNodeById(id, typeof (PBSTree)) as PBSTree;
        }

        #endregion

        #region 树规则

        /// <summary>
        /// 根据树类型获取所有规则集合
        /// </summary>
        /// <param name="treeType"></param>
        /// <returns></returns>
        public IList GetPBSTreeRules(Type treeType)
        {
            CategoryTree tree = InitTree("PBS树", typeof (PBSTree));
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
        public IList GetPBSTreeRules(Type treeType, Type parentNodeType)
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
            string sql = "Select MAX(OPGCODE) Code from ResPBSTree";
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

        #region 特性集

        /// <summary>
        /// 保存 修改 特性集 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList SaveOrUpdateFeatureSet(IList list)
        {
            IList versionList = new ArrayList();
            //IList updateListVersion = new ArrayList();
            IList saveListSet = new ArrayList();
            IList UpdateListSet = new ArrayList();
            //IList listSet = dao.ObjectQuery(typeof(FeatureSet), new ObjectQuery());
            foreach (FeatureSet set in list)
            {
                if (set.Id == null || set.Id == "")
                {
                    Versions v = new Versions();
                    v.LastUpdateTime = DateTime.Now;
                    v.ChangeAction = IfcChangeAction.增加;
                    v.SnapValue = set.Name;
                    versionList.Add(v);
                    saveListSet.Add(set);
                }
                else
                {
                    //bool flag = true;
                    //foreach (FeatureSet s in listSet)
                    //{
                    //    if (set == s)
                    //    {
                    //        flag = false;
                    //        break;
                    //    }
                    //}
                    //if (flag)
                    //{
                    UpdateListSet.Add(set);
                    //}
                }
            }
            if (UpdateListSet != null && UpdateListSet.Count > 0)
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (FeatureSet s in UpdateListSet)
                {
                    dis.Add(Expression.Eq("Id", s.VersionId));
                }
                oq.AddCriterion(dis);
                IList updateListVersion = dao.ObjectQuery(typeof (Versions), oq);
                foreach (Versions v in updateListVersion)
                {
                    v.LastUpdateTime = DateTime.Now;
                    v.ChangeAction = IfcChangeAction.更改;
                    versionList.Add(v);
                }
            }

            dao.SaveOrUpdate(versionList);
            foreach (Versions v in versionList)
            {
                foreach (FeatureSet s in saveListSet)
                {
                    if (s.Name == v.SnapValue)
                    {
                        s.VersionId = v.Id;
                        UpdateListSet.Add(s);
                    }
                }
            }
            dao.SaveOrUpdate(UpdateListSet);
            return UpdateListSet;
        }

        public bool Delete(IList list)
        {
            return dao.Delete(list);
        }

        public bool Delete(object obj)
        {
            return dao.Delete(obj);
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteElement(Elements element)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", element.Id));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = dao.ObjectQuery(typeof (Elements), oq);
            return dao.Delete(list);
        }

        #endregion

        public object Save(object obj)
        {
            return dao.SaveOrUpdateCopy(obj);
        }

        public IList Save(IList obj)
        {
            return dao.SaveOrUpdateCopy(obj);
        }

        [TransManager]
        public PBSTree SavePBSTree(PBSTree pbs, string parentSysCode)
        {
            dao.Save(pbs);
            pbs.SysCode = parentSysCode + pbs.Id + ".";
            dao.Save(pbs);
            return pbs;
        }

        public DataSet GetPBSTreesByInstanceSql(string projectId, string sParentID, string sName)
        {
            string sWhere = string.Empty;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                "select t1.name,t1.id,t1.fullpath,t1.code,t1.tlevel from thd_pbstree t1 where  t1.theprojectguid =:projectId and t1.state = '1' {0} order by t1.tlevel asc,t1.orderno asc";

            command.Parameters.Add(new OracleParameter("projectId", projectId));
            if (!string.IsNullOrEmpty(sParentID))
            {
                sWhere = " and t1.syscode like  (select syscode from  thd_pbstree where id=:parentID)||'%' ";
                command.Parameters.Add(new OracleParameter("parentID", sParentID));
            }
            if (!string.IsNullOrEmpty(sName))
            {
                sWhere += " and t1.name like '%'||:Name||'%' ";
                command.Parameters.Add(new OracleParameter("Name", sName));
            }
            sql = string.Format(sql, sWhere);
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        [TransManager]
        public void SaveBatchPBSTreeName(IList lstPBSTree)
        {


            string sSQL = string.Empty;
            //string sOldFullPath = string.Empty;
            // string sNewFullPath = string.Empty;
            string sValue;
            if (lstPBSTree == null || lstPBSTree.Count == 0) throw new Exception("请修改后在保存");

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            IList<string> lstFullPath = new List<string>();
            int iRow = 0;
            session.Transaction.Enlist(command);    
            // session.Transaction.Begin();
            foreach (PBSTree oPBSTree in lstPBSTree)
            {
// regexp_replace(t.fullpath,'^ \\服务系统结构建筑项目',' \\服务系1212统结构建筑项目')
                command.CommandText = "update thd_pbstree t  set t.fullpath=:fullpath,t.name=:name where t.id=:id";
                command.Parameters.Clear();
                command.Parameters.Add(new OracleParameter("fullpath", oPBSTree.FullPath));
                command.Parameters.Add(new OracleParameter("id", oPBSTree.Id));
                command.Parameters.Add(new OracleParameter("name", oPBSTree.Name));
                command.ExecuteNonQuery();
                // sOldFullPath = "^" + oGWBSTree.Describe.Replace("\\", "\\\\");
                // sNewFullPath = oGWBSTree.FullPath.Replace("\\", "\\\\");

                command.CommandText =
                    @"update thd_pbstree t  set  t.fullpath=:newFullPath || substr(t.fullpath,length(:oldFullPath)+1)  where T.id!=:id AND theprojectguid=:projectid and t.state=1 and t.syscode like (select syscode from thd_pbstree where id=:id and rownum=1)||'%'";
                command.Parameters.Clear();
                command.Parameters.Add(new OracleParameter("oldFullPath", oPBSTree.Describe));
                command.Parameters.Add(new OracleParameter("id", oPBSTree.Id));
                command.Parameters.Add(new OracleParameter("newFullPath", oPBSTree.FullPath));
                command.Parameters.Add(new OracleParameter("projectid", oPBSTree.TheProjectGUID));

                iRow = command.ExecuteNonQuery();
                // lstFullPath.Add(oGWBSTree.FullPath);
                //}
            }
            // session.Transaction.Commit();

        }

        [TransManager]
        public DataSet GetCostBudget(string projectid, string sysCode)
        {
            string sql =
                string.Format(@"select ResourceTypeGUID,RESOURCETYPENAME, ResourceTypeSpec,sum(PlanWorkAmount) totalamount,PROJECTAMOUNTUNITNAME from thd_GWBSDetailCostSubject
   where theprojectguid  = '{0}' and
    TheGWBSTreeSyscode like '{1}%'
   group by ResourceTypeGUID,RESOURCETYPENAME, ResourceTypeSpec,PROJECTAMOUNTUNITNAME",
                              projectid, sysCode);
            return QueryDataToDataSet(sql);
        }
    }
}