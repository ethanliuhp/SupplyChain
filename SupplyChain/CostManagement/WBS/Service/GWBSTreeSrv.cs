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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using CommonSearchLib.BillCodeMng.Service;
using Application.Resource.MaterialResource.Domain;
using System.Data.OracleClient;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Drawing;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using System.Data.Common;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Linq;

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

                Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
                childNode.OwnerGUID = login.ThePerson.Id;
                childNode.OwnerName = login.ThePerson.Name;
                childNode.OwnerOrgSysCode = login.TheOperationOrgInfo.SysCode;

                GWBSTree parentNode = dao.Get(typeof(GWBSTree), childNode.ParentNode.Id) as GWBSTree;
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

                string sql = "select t1.name,t1.fullpath from thd_gwbstree t1 where t1.id='" + childNode.Id + "'";
                command.CommandText = sql;
                IDataReader dr = command.ExecuteReader();
                DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

                string oldName = ds.Tables[0].Rows[0]["name"].ToString();
                string fullpath = ds.Tables[0].Rows[0]["fullpath"].ToString();

                if (oldName != childNode.Name)
                {
                    //修改下属子节点完整路径
                    sql = "update thd_gwbstree t1 set t1.fullpath=replace(t1.fullpath,'" + fullpath + "','" + childNode.FullPath + "') where t1.syscode like '" + childNode.SysCode + "%' and t1.id!='" + childNode.Id + "'";

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

                dao.Update(childNode);
            }

            return childNode;
        }

        [TransManager]
        public GWBSTree SaveGWBSTree(GWBSTree childNode, IList listVerify)
        {

            childNode.CreateDate = DateTime.Now;
            childNode.UpdatedDate = DateTime.Now;
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


            for (int i = 0; i < listVerify.Count; i++)
            {
                ProjectDocumentVerify docVerify = listVerify[i] as ProjectDocumentVerify;

                childNode.ListDocVerify.Add(docVerify);
                docVerify.ProjectTask = childNode;

                docVerify.TaskSyscode = childNode.SysCode;
            }

            dao.SaveOrUpdate(childNode);

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
        }

        [TransManager]
        public IList GWBSTreeOrder(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (GWBSTree wbs in list)
            {
                dis.Add(Expression.Eq("Id", wbs.Id));
            }
            oq.AddCriterion(dis);
            IList listNew = dao.ObjectQuery(typeof(GWBSTree), oq);


            IEnumerable<GWBSTree> listQuery = list.OfType<GWBSTree>();
            foreach (GWBSTree wbs in listNew)
            {
                var query = from w in listQuery
                            where w.Id == wbs.Id
                            select w;

                if (query.Count() > 0)
                    wbs.OrderNo = query.ElementAt(0).OrderNo;
            }

            listNew = listNew.OfType<GWBSTree>().OrderBy(w => w.OrderNo).ToArray();

            dao.Update(listNew);

            return listNew;
        }

        /// <summary>
        /// 保存节点和其工程文档验证
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        //[TransManager]
        //public IList SaveGWBSTreesAndVerify(IList lst)
        //{
        //    IList list = new ArrayList();
        //    //foreach (GWBSTree var in lst)
        //    //{
        //    //    list.Add(SaveGWBSTree(var));
        //    //}
        //    //return list;
        //    for (int i = 0; i < lst.Count; i++)
        //    {
        //        GWBSTree wbsTree = lst[i] as GWBSTree;
        //        IList listVerify = GetDocumentTemplatesByTaskType(wbsTree.ProjectTaskTypeGUID.Id);
        //        if (listVerify != null && listVerify.Count > 0)
        //        {
        //            for (int j = 0; j < listVerify.Count; j++)
        //            {
        //                ProjectDocumentVerify docVerify = listVerify[i] as ProjectDocumentVerify;

        //                wbsTree.ListDocVerify.Add(docVerify);
        //                docVerify.ProjectTask = wbsTree;

        //                docVerify.TaskSyscode = wbsTree.SysCode;
        //            }
        //        }
        //    }
        //}


        public List<GWBSDetail> GetWBSDetail(string wbsId, string sqlWhere)
        {
            string sql = "select t2.id as costid,t2.quotacode,t1.id,t1.name,t1.mainresourcetypeid,t1.mainresourcetypename,t1.ContractProjectName," +
                         "t1.mainresourcetypespec,t1.diagramnumber,t1.workamountunitname,t1.planworkamount,t1.planprice,t1.plantotalprice,t1.priceunitname," +
                         "t1.state,t1.contentdesc,t1.orderno,t1.code,t1.changeparentid,T1.CONTRACTGROUPNAME,t1.QuantityConfirmed from thd_gwbsdetail t1 inner join thd_costitem t2 on t1.costitemguid=t2.id where t1.parentid='" + wbsId + "'" + sqlWhere + " order by t1.orderno";

            DataSet ds = SearchSQL(sql);

            List<GWBSDetail> listDtl = new List<GWBSDetail>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                GWBSDetail dtl = new GWBSDetail();
                CostItem costItem = new CostItem();
                costItem.QuotaCode = row["quotacode"].ToString();
                costItem.Id = row["costid"].ToString();

                dtl.TheCostItem = costItem;
                dtl.Id = row["id"].ToString();
                dtl.Name = row["name"].ToString();
                dtl.OrderNo = ClientUtil.ToInt(row["orderno"]);
                dtl.MainResourceTypeId = row["mainresourcetypeid"].ToString();
                dtl.MainResourceTypeName = row["mainresourcetypename"].ToString();
                dtl.MainResourceTypeSpec = row["mainresourcetypespec"].ToString();
                dtl.DiagramNumber = row["diagramnumber"].ToString();
                dtl.WorkAmountUnitName = row["workamountunitname"].ToString();
                dtl.PlanWorkAmount = ClientUtil.ToDecimal(row["planworkamount"]);
                dtl.PlanPrice = ClientUtil.ToDecimal(row["planprice"]);
                dtl.PlanTotalPrice = ClientUtil.ToDecimal(row["plantotalprice"]);
                dtl.PriceUnitName = row["priceunitname"].ToString();
                dtl.State = (DocumentState)ClientUtil.ToInt(row["state"]);
                dtl.ContentDesc = row["contentdesc"].ToString();
                dtl.ContractProjectName = row["ContractProjectName"] + "";
                dtl.Code = ClientUtil.ToString(row["code"]);
                dtl.ChangeParentID = ClientUtil.ToString(row["changeparentid"]);
                dtl.ContractGroupName = ClientUtil.ToString(row["CONTRACTGROUPNAME"]);
                dtl.QuantityConfirmed = Convert.ToDecimal(row["QuantityConfirmed"]);
                listDtl.Add(dtl);
            }

            return listDtl;
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
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = "select t1.name,t1.fullpath from thd_gwbstree t1 where t1.id='" + childNode.Id + "'";
                command.CommandText = sql;
                IDataReader dr = command.ExecuteReader();
                DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

                string oldName = ds.Tables[0].Rows[0]["name"].ToString();
                string fullpath = ds.Tables[0].Rows[0]["fullpath"].ToString();

                if (oldName != childNode.Name)
                {
                    //修改下属子节点完整路径
                    sql = "update thd_gwbstree t1 set t1.fullpath=replace(t1.fullpath,'" + fullpath + "','" + childNode.FullPath + "') where t1.syscode like '" + childNode.SysCode + "%' and t1.id!='" + childNode.Id + "'";

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

                dao.Update(childNode);
            }
            return childNode;

        }

        [TransManager]
        public GWBSTree InsertOrUpdateWBSTree(GWBSTree childNode, IList listVerify)
        {
            childNode.UpdatedDate = DateTime.Now;

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

            if (listVerify != null && listVerify.Count > 0)
            {
                for (int i = 0; i < listVerify.Count; i++)
                {
                    ProjectDocumentVerify docVerify = listVerify[i] as ProjectDocumentVerify;

                    childNode.ListDocVerify.Add(docVerify);
                    docVerify.ProjectTask = childNode;

                    docVerify.TaskSyscode = childNode.SysCode;
                }
            }

            dao.SaveOrUpdate(childNode);


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
        /// 插入（部分属性固定）或修改WBS节点集合
        /// </summary>
        [TransManager]
        public IList InsertOrUpdateWBSTrees(IList lst, IList listVerify)
        {
            IList list = new ArrayList();
            foreach (GWBSTree var in lst)
            {
                if (var.Id == null)
                    list.Add(InsertOrUpdateWBSTree(var, listVerify));
                else
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
        public bool DeleteGWBSTreeOld(GWBSTree wbs)
        {
            try
            {
                if (wbs == null || wbs.SysCode == "")
                    return false;

                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                //1.删除WBS节点及其子节点
                //string sql = " delete from thd_gwbstree t1 where t1.syscode like '" + wbs.SysCode + "%'";
                string sql = " delete from thd_gwbstree t1 where t1.id =:id ";//因为是级联删除
                command.Parameters.Clear();
                command.Parameters.Add(new OracleParameter("id", wbs.Id));
                command.CommandText = sql;
                command.ExecuteNonQuery();

                //2.更新父节点类型信息
                if (wbs.ParentNode != null)
                {
                    //sql = "select count(*) childCount from thd_gwbstree t1 where t1.parentnodeid='" + wbs.ParentNode.Id + "'";
                    sql = "select count(*) childCount from thd_gwbstree t1 where t1.parentnodeid=:parentnodeid";
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter("parentnodeid", wbs.ParentNode.Id));
                    command.CommandText = sql;
                    int childCount = Convert.ToInt32(command.ExecuteScalar());

                    //根节点=0，枝节点=1，叶节点=2
                    if (childCount == 0)
                        // sql = "update thd_gwbstree t1 set t1.categorynodetype=2 where t1.id='" + wbs.ParentNode.Id + "'";
                        sql = "update thd_gwbstree t1 set t1.categorynodetype=2 where t1.id=:id";
                    else
                        // sql = "update thd_gwbstree t1 set t1.categorynodetype=1 where t1.id='" + wbs.ParentNode.Id + "'";
                        sql = "update thd_gwbstree t1 set t1.categorynodetype=1 where t1.id=:id";

                    command.CommandText = sql;
                    command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter("id", wbs.ParentNode.Id));
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
            //pbs = dao.Get(typeof(GWBSTree), pbs.Id) as GWBSTree;
            //return nodeSrv.DeleteCategoryNode(pbs);
        }
        /// <summary>
        /// 删除GWBS树节点
        /// </summary>
        /// <param name="pbs"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteGWBSTree(GWBSTree wbs)
        {
            try
            {
                if (wbs == null || wbs.SysCode == "")
                    return false;

                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                #region 开启级联删除
                //1.删除WBS节点及其子节点
                //string sql = " delete from thd_gwbstree t1 where t1.syscode like '" + wbs.SysCode + "%'";
                string sql = "BEGIN delete  thd_gwbstree t1 where t1.id =:id ;";//因为是级联删除
                command.Parameters.Add(new OracleParameter("id", wbs.Id));
                //command.CommandText = sql;
                //command.ExecuteNonQuery();
                //2.更新父节点类型信息
                if (wbs.ParentNode != null)
                {//decode(count(*),0,2,1)
                    sql += "update thd_gwbstree t2 set t2.categorynodetype= decode((select 1 from dual where exists(select 1 childCount from thd_gwbstree t1 where t1.parentnodeid=:parentnodeid)),1,1,2)   where t2.id=:parentnodeid;";
                    // command.Parameters.Clear();
                    command.Parameters.Add(new OracleParameter("parentnodeid", wbs.ParentNode.Id));

                }
                sql += " end;";
                command.CommandText = sql;
                command.ExecuteNonQuery();
                //session.Transaction.Commit();
                #endregion
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
        [TransManager]
        public bool DeleteGWBSTree(IList list)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);

                string sql = " begin ";
                foreach (GWBSTree item in list)
                {
                    if (item == null || item.SysCode == "")
                    {
                        return false;
                    }

                    //1.删除WBS节点及其子节点                    
                    sql += string.Format(" delete  thd_gwbstree t1 where t1.id ='{0}'; ", item.Id);//因为是级联删除

                    //2.更新父节点类型信息
                    if (item.ParentNode != null)
                    {
                        sql += string.Format("update thd_gwbstree t2 set t2.categorynodetype= decode((select 1 from dual where exists(select 1 childCount from thd_gwbstree t1 where t1.parentnodeid='{0}')),1,1,2)   where t2.id='{0}';", item.ParentNode.Id);
                    }
                }
                sql += " end;";
                command.CommandText = sql;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="pbs">移动节点</param>
        /// <param name="toPbs">目的节点</param>
        /// <returns></returns>
        [TransManager]
        public Hashtable MoveGWBSTree(GWBSTree wbs, GWBSTree toWbs)
        {
            Hashtable listResult = new Hashtable();

            //修改原始父节点属性
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", wbs.ParentNode.Id));
            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (list != null && list.Count == 1)
            {
                GWBSTree oldParent = dao.Get(typeof(GWBSTree), wbs.ParentNode.Id) as GWBSTree;
                oldParent.CategoryNodeType = NodeType.LeafNode;

                dao.Update(oldParent);

                listResult.Add(oldParent.Id, oldParent);
            }

            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("Id", toWbs.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            oq.AddFetchMode("ListRelaPBS", FetchMode.Eager);
            oq.AddFetchMode("ProjectTaskTypeGUID", FetchMode.Eager);
            list = dao.ObjectQuery(typeof(GWBSTree), oq);
            toWbs = list[0] as GWBSTree;

            IBusinessOperators author = Dao.Get(typeof(BusinessOperators), wbs.OwnerGUID) as IBusinessOperators;
            string ownerGUID = wbs.OwnerGUID;
            string ownerName = wbs.OwnerName;
            string ownerOrgSysCode = wbs.OwnerOrgSysCode;

            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.AddCriterion(Expression.Like("SysCode", wbs.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Not(Expression.Eq("Id", wbs.Id)));
            List<GWBSTree> listChild = dao.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>().ToList();

            //修改自身属性
            wbs = dao.Get(typeof(GWBSTree), wbs.Id) as GWBSTree;
            wbs.ParentNode = toWbs;
            wbs.Level = toWbs.Level + 1;
            wbs.SysCode = toWbs.SysCode + wbs.Id + ".";
            wbs.FullPath = toWbs.FullPath + @"\" + wbs.Name;

            wbs.OrderNo = GetMaxOrderNo(wbs, null) + 1;
            wbs.OwnerGUID = ownerGUID;
            wbs.OwnerName = ownerName;
            wbs.OwnerOrgSysCode = ownerOrgSysCode;
            wbs.Author = author;
            wbs.ProjectTaskTypeGUID = toWbs.ProjectTaskTypeGUID;
            wbs.ProjectTaskTypeName = toWbs.ProjectTaskTypeName;
            wbs.CheckRequire = toWbs.ProjectTaskTypeGUID.CheckRequire.PadRight(11, 'X') + "0";
            wbs.DailyCheckState = wbs.CheckRequire;
            wbs.OverOrUndergroundFlag = toWbs.OverOrUndergroundFlag;

            wbs.ListRelaPBS.Clear();
            foreach (GWBSRelaPBS rela in toWbs.ListRelaPBS)
            {
                GWBSRelaPBS newRela = new GWBSRelaPBS();
                newRela.ThePBS = rela.ThePBS;
                newRela.PBSName = rela.PBSName;

                newRela.TheGWBSTree = wbs;
                wbs.ListRelaPBS.Add(newRela);
            }
            wbs.TheProjectGUID = toWbs.TheProjectGUID;
            wbs.TheProjectName = toWbs.TheProjectName;
            wbs.TaskState = DocumentState.Edit;
            wbs.TaskStateTime = DateTime.Now;
            wbs.Summary = toWbs.Summary + "," + wbs.Name;


            toWbs.ChildNodes.Add(wbs);


            if (toWbs.CategoryNodeType != NodeType.RootNode)//如果目的节点是叶节点设为中间节点  0、1、2
                toWbs.CategoryNodeType = NodeType.MiddleNode;

            wbs.CategoryNodeType = NodeType.LeafNode;

            dao.SaveOrUpdate(toWbs);

            listResult.Add(toWbs.Id, toWbs);
            listResult.Add(wbs.Id, wbs);

            //更新所有子节点的属性，新的父节点的id变化，下属所有节点的层次码均改变
            if (listChild.Count > 0)
            {
                var query = from c in listChild
                            where c.ParentNode != null && c.ParentNode.Id == wbs.Id
                            select c;

                foreach (GWBSTree childNode in query)
                {
                    childNode.SysCode = wbs.SysCode + childNode.Id + ".";
                    childNode.Level = wbs.Level + 1;
                    childNode.FullPath = wbs.FullPath + @"\" + childNode.Name;
                    //childNode.OrderNo = GetMaxOrderNo(pbs, null) + 1;
                    childNode.OwnerGUID = ownerGUID;
                    childNode.OwnerName = ownerName;
                    childNode.OwnerOrgSysCode = ownerOrgSysCode;
                    childNode.Author = author;
                    childNode.ProjectTaskTypeGUID = wbs.ProjectTaskTypeGUID;
                    childNode.ProjectTaskTypeName = wbs.ProjectTaskTypeName;
                    childNode.CheckRequire = wbs.ProjectTaskTypeGUID.CheckRequire.PadRight(11, 'X') + "0";
                    childNode.DailyCheckState = childNode.CheckRequire;
                    childNode.OverOrUndergroundFlag = wbs.OverOrUndergroundFlag;

                    childNode.ListRelaPBS.Clear();
                    foreach (GWBSRelaPBS rela in wbs.ListRelaPBS)
                    {
                        GWBSRelaPBS newRela = new GWBSRelaPBS();
                        newRela.ThePBS = rela.ThePBS;
                        newRela.PBSName = rela.PBSName;

                        newRela.TheGWBSTree = childNode;
                        childNode.ListRelaPBS.Add(newRela);
                    }
                    childNode.TheProjectGUID = wbs.TheProjectGUID;
                    childNode.TheProjectName = wbs.TheProjectName;

                    childNode.TaskState = DocumentState.Edit;
                    childNode.TaskStateTime = DateTime.Now;
                    childNode.Summary = wbs.Summary + "," + childNode.Name;
                    if (childNode.ParentNode.CategoryNodeType != NodeType.RootNode)
                    {
                        childNode.ParentNode.CategoryNodeType = NodeType.MiddleNode;
                    }
                    childNode.CategoryNodeType = NodeType.LeafNode;

                    UpdateChildNodes(childNode, listChild);
                }

                dao.Update(listChild);

                foreach (GWBSTree childNode in listChild)
                {
                    listResult.Add(childNode.Id, childNode);
                }
            }

            return listResult;
        }

        /// <summary>
        /// 移动节点并修改移动节点及子节点契约信息
        /// </summary>
        /// <param name="pbs"></param>
        /// <param name="toOrg"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree MoveGWBSTreeAndUpdateContract(GWBSTree wbs, GWBSTree toWbs, ContractGroup group)
        {
            //修改原始父节点属性
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", wbs.ParentNode.Id));
            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (list != null && list.Count == 1)
            {
                GWBSTree oldParent = dao.Get(typeof(GWBSTree), wbs.ParentNode.Id) as GWBSTree;
                oldParent.CategoryNodeType = NodeType.LeafNode;

                dao.SaveOrUpdate(oldParent);
            }

            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("Id", toWbs.Id));
            oq.AddFetchMode("ChildNodes", FetchMode.Eager);
            list = dao.ObjectQuery(typeof(GWBSTree), oq);
            toWbs = list[0] as GWBSTree;

            //修改自身属性
            wbs = dao.Get(typeof(GWBSTree), wbs.Id) as GWBSTree;
            wbs.ParentNode = toWbs;
            wbs.Level = toWbs.Level + 1;
            wbs.SysCode = toWbs.SysCode + wbs.Id + ".";

            //if (group != null)
            //{
            //    pbs.ContractGroupGUID = group;
            //    pbs.ContractGroupCode = group.Code;
            //}

            toWbs.ChildNodes.Add(wbs);

            if (toWbs.CategoryNodeType == NodeType.LeafNode)//如果目的节点是叶节点设为中间节点  0、1、2
            {
                toWbs.CategoryNodeType = NodeType.MiddleNode;
            }
            dao.SaveOrUpdate(toWbs);

            //更新所有子节点的层次码
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", wbs.Id));
            IList listChild = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (listChild.Count > 0)
            {
                for (int i = 0; i < listChild.Count; i++)
                {
                    GWBSTree childNode = listChild[0] as GWBSTree;
                    childNode.SysCode = wbs.SysCode + childNode.Id + ".";
                    //if (group != null)
                    //{
                    //    pbs.ContractGroupGUID = group;
                    //    pbs.ContractGroupCode = group.Code;
                    //}

                    dao.SaveOrUpdate(childNode);

                    UpdateChildNodes(childNode, group);
                }
            }

            return wbs;
        }

        private void UpdateChildNodes(GWBSTree parentNode, List<GWBSTree> listChild)
        {
            var query = from c in listChild
                        where c.ParentNode != null && c.ParentNode.Id == parentNode.Id
                        select c;

            foreach (GWBSTree childNode in query)
            {
                childNode.SysCode = parentNode.SysCode + childNode.Id + ".";
                childNode.Level = parentNode.Level + 1;
                childNode.FullPath = parentNode.FullPath + @"\" + childNode.Name;
                //childNode.OrderNo = GetMaxOrderNo(pbs, null) + 1;
                childNode.OwnerGUID = parentNode.OwnerGUID;
                childNode.OwnerName = parentNode.OwnerName;
                childNode.OwnerOrgSysCode = parentNode.OwnerOrgSysCode;
                childNode.Author = parentNode.Author;
                childNode.ProjectTaskTypeGUID = parentNode.ProjectTaskTypeGUID;
                childNode.ProjectTaskTypeName = parentNode.ProjectTaskTypeName;
                childNode.CheckRequire = parentNode.ProjectTaskTypeGUID.CheckRequire.PadRight(11, 'X') + "0";
                childNode.DailyCheckState = childNode.CheckRequire;
                childNode.OverOrUndergroundFlag = parentNode.OverOrUndergroundFlag;

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

                childNode.TaskState = DocumentState.Edit;
                childNode.TaskStateTime = DateTime.Now;
                childNode.Summary = parentNode.Summary + "," + childNode.Name;

                if (childNode.ParentNode.CategoryNodeType != NodeType.RootNode)
                {
                    childNode.ParentNode.CategoryNodeType = NodeType.MiddleNode;
                }
                childNode.CategoryNodeType = NodeType.LeafNode;

                UpdateChildNodes(childNode, listChild);

            }
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
                    childNode.FullPath = parentNode.FullPath + @"\" + childNode.Name;
                    //childNode.OrderNo = GetMaxOrderNo(pbs, null) + 1;
                    childNode.OwnerGUID = parentNode.OwnerGUID;
                    childNode.OwnerName = parentNode.OwnerName;
                    childNode.OwnerOrgSysCode = parentNode.OwnerOrgSysCode;
                    childNode.ProjectTaskTypeGUID = parentNode.ProjectTaskTypeGUID;
                    childNode.ProjectTaskTypeName = parentNode.ProjectTaskTypeName;
                    childNode.CheckRequire = parentNode.ProjectTaskTypeGUID.CheckRequire.PadRight(11, 'X') + "0";
                    childNode.DailyCheckState = childNode.CheckRequire;
                    childNode.OverOrUndergroundFlag = parentNode.OverOrUndergroundFlag;

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

                    childNode.TaskState = DocumentState.Edit;
                    childNode.TaskStateTime = DateTime.Now;
                    childNode.Summary = parentNode.Summary + "," + childNode.Name;
                    if (childNode.ParentNode.CategoryNodeType != NodeType.RootNode)
                    {
                        childNode.ParentNode.CategoryNodeType = NodeType.MiddleNode;
                    }
                    childNode.CategoryNodeType = NodeType.LeafNode;

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
            //Disjunction dis = new Disjunction();
            //List<string> syscodeList = new List<string>();
            //#region 根据业务组织得到对应的工程任务节点层次码
            //Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
            ////得业务组织
            //oq.AddCriterion(Expression.Like("SysCode", login.TheOperationOrgInfo.SysCode, MatchMode.Start));
            //IList orgList = dao.ObjectQuery(typeof(OperationOrg), oq);
            //if (orgList == null || orgList.Count == 0)
            //{
            //    return null;
            //}
            ////得管理OBS
            //oq.Criterions.Clear();
            //foreach (OperationOrg org in orgList)
            //{
            //    dis.Add(Expression.Eq("OrpJob.Id", org.Id));
            //}
            //oq.AddCriterion(dis);
            //IList listOBS = dao.ObjectQuery(typeof(OBSManage), oq);
            //if (listOBS == null || listOBS.Count == 0)
            //{
            //    return null;
            //}
            ////得管理OBS上关联的工程任务层次吗
            //foreach (OBSManage obs in listOBS)
            //{
            //    if (syscodeList.Count == 0)
            //    {
            //        syscodeList.Add(obs.ProjectTaskSysCode);
            //    }
            //    else
            //    {
            //        bool changFlag = false;
            //        bool addFlag = false;
            //        int index = -1;
            //        for (int i = 0 ;i<syscodeList.Count;i++)
            //        {
            //            string syscode = syscodeList[i];
            //            if (obs.ProjectTaskSysCode.Contains(syscode) || obs.ProjectTaskSysCode == syscode)
            //            {
            //                break; 
            //            }
            //            else if (syscode.Contains(obs.ProjectTaskSysCode))
            //            {
            //                changFlag = true;
            //                index = i;
            //                break;
            //            }
            //            else
            //            {
            //                addFlag = true;
            //                break;
            //            }
            //        }
            //        if (changFlag)
            //        {
            //            syscodeList[index] = obs.ProjectTaskSysCode;
            //        }
            //        if (addFlag)
            //        {
            //            syscodeList.Add(obs.ProjectTaskSysCode);
            //        }
            //    }
            //}
            //#endregion

            //if (syscodeList.Count == 0)
            //{
            //    return null;
            //}
            ////得GWBSTree
            //oq.Criterions.Clear();
            //dis = new Disjunction();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            oq.AddCriterion(Expression.Eq("State", 1));
            //oq.AddOrder(Order.Asc("SysCode"));
            //oq.AddOrder(Order.Asc("CategoryNodeType"));
            //foreach (string s in syscodeList)
            //{
            //    dis.Add(Expression.Like("SysCode",s,MatchMode.Start));
            //}
            //dis.Add(Expression.Eq("Level", 1));
            //oq.AddCriterion(dis);
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);
            return list;
        }
        public DataSet GetGWBSTreesByWhere(string sWhere)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sSQL = string.Empty;
            if (!string.IsNullOrEmpty(sWhere))
            {
                sWhere = " and " + sWhere + "  ";
            }
            //select * from thd_gwbstree t1 where t1.parentnodeid=:parentnodeid and t1.tlevel=:tlevel and t1.theprojectguid=:theprojectguid

            sSQL = "select  t1.id,t1.name,t1.code,t1.taskplanstarttime ,t1.taskplanendtime   from thd_gwbstree t1 where   t1.State=1 " + sWhere + " order by t1.orderno asc";
            command.CommandText = sSQL;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        public DataSet GetGWBSTrees(string sProjectID, string sName, DateTime dStartDate, DateTime dEndDate)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sSQL = string.Empty;
            IDbDataParameter oPara = null;
            //select * from thd_gwbstree t1 where t1.parentnodeid=:parentnodeid and t1.tlevel=:tlevel and t1.theprojectguid=:theprojectguid

            sSQL = "select  t1.id,t1.name,t1.code,t1.taskplanstarttime ,t1.taskplanendtime   from thd_gwbstree t1 where   t1.theprojectguid=:theprojectguid  and t1.State=1 and t1.name like '%" + sName + "%' order by t1.orderno asc";


            oPara = command.CreateParameter();
            oPara.ParameterName = "theprojectguid";
            oPara.Value = sProjectID;
            command.Parameters.Add(oPara);

            oPara = command.CreateParameter();
            oPara.ParameterName = "taskplanstarttime";
            oPara.Value = dStartDate.ToShortDateString();
            oPara.DbType = DbType.Date;
            command.Parameters.Add(oPara);

            oPara = command.CreateParameter();
            oPara.ParameterName = "taskplanendtime";
            oPara.Value = dEndDate.ToShortDateString();
            oPara.DbType = DbType.Date;
            command.Parameters.Add(oPara);
            oPara = command.CreateParameter();
            oPara.ParameterName = "taskplanendtime";
            oPara.Value = dEndDate.ToShortDateString();
            oPara.DbType = DbType.Date;
            command.Parameters.Add(oPara);
            command.CommandText = sSQL;

            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet;
        }
        public DataTable GetGWBSTrees(string sProjectID, string sParentID, int iLevel)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sSQL = string.Empty;
            IDbDataParameter oPara = null;
            //select * from thd_gwbstree t1 where t1.parentnodeid=:parentnodeid and t1.tlevel=:tlevel and t1.theprojectguid=:theprojectguid
            if (string.IsNullOrEmpty(sParentID))
            {
                sSQL = "select t1.*, GetChildCount(t1.id) childCount from thd_gwbstree t1 where t1.parentnodeid is null and  t1.tlevel=:tlevel and t1.theprojectguid=:theprojectguid  and t1.State=1 order by t1.orderno asc";
                oPara = command.CreateParameter();
                oPara.ParameterName = "tlevel";
                oPara.Value = iLevel;
                command.Parameters.Add(oPara);

                oPara = command.CreateParameter();
                oPara.ParameterName = "theprojectguid";
                oPara.Value = sProjectID;
                command.Parameters.Add(oPara);

            }
            else
            {
                sSQL = "select  t1.*, GetChildCount(t1.id) childCount  from thd_gwbstree t1 where t1.parentnodeid=:parentnodeid and t1.tlevel=:tlevel and t1.theprojectguid=:theprojectguid and t1.State=1 order by t1.orderno asc";
                oPara = command.CreateParameter();
                oPara.ParameterName = "tlevel";
                oPara.Value = iLevel;
                command.Parameters.Add(oPara);

                oPara = command.CreateParameter();
                oPara.ParameterName = "theprojectguid";
                oPara.Value = sProjectID;
                command.Parameters.Add(oPara);

                oPara = command.CreateParameter();
                oPara.ParameterName = "parentnodeid";
                oPara.Value = sParentID;
                command.Parameters.Add(oPara);

            }
            command.CommandText = sSQL;

            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return dataSet.Tables[0];
        }

        /// <summary>
        /// 根据项目ID和所需要的节点的层号的子节点 （大于当前层号）
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="iLevel"></param>
        /// <returns></returns>
        public IList GetGWBSTreesByLevel(string projectId, int iLevel, string sParentSysCode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddCriterion(Expression.Gt("Level", iLevel));
            oq.AddCriterion(Expression.Like("SysCode", sParentSysCode, MatchMode.Start));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);
            return list;
        }
        public DataSet GetGWBSTreesByInstanceSql(string projectId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select t1.id,t1.name,t1.syscode,t1.parentnodeid,t1.orderNo,t1.specialtype,T1.TASKSTATE from thd_gwbstree t1 where 1=1 and t1.theprojectguid = '" + projectId + "' and t1.state = '1'  order by t1.tlevel asc,t1.orderno asc";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }
        public DataSet GetGWBSTreesByInstanceSql(string projectId, string sParentID, string sName)
        {
            string sWhere = string.Empty;
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = "select t1.name,t1.id,t1.fullpath,t1.code,t1.tlevel from thd_gwbstree t1 where  t1.theprojectguid =:projectId and t1.state = '1' {0} order by t1.tlevel asc,t1.orderno asc";

            command.Parameters.Add(new OracleParameter("projectId", projectId));
            if (!string.IsNullOrEmpty(sParentID))
            {
                sWhere = " and t1.syscode like  (select syscode from  thd_gwbstree where id=:parentID)||'%' ";
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
        public void SaveBatchGWBSTreeName(IList lstGWBSTree)
        {


            string sSQL = string.Empty;
            //string sOldFullPath = string.Empty;
            // string sNewFullPath = string.Empty;
            string sValue;
            if (lstGWBSTree == null || lstGWBSTree.Count == 0) throw new Exception("请修改后在保存");

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            IList<string> lstFullPath = new List<string>();
            int iRow = 0;
            session.Transaction.Enlist(command);
            // session.Transaction.Begin();
            foreach (GWBSTree oGWBSTree in lstGWBSTree)
            {// regexp_replace(t.fullpath,'^ \\服务系统结构建筑项目',' \\服务系1212统结构建筑项目')
                command.CommandText = "update thd_gwbstree t  set t.fullpath=:fullpath,t.name=:name where t.id=:id";
                command.Parameters.Clear();
                command.Parameters.Add(new OracleParameter("fullpath", oGWBSTree.FullPath));
                command.Parameters.Add(new OracleParameter("id", oGWBSTree.Id));
                command.Parameters.Add(new OracleParameter("name", oGWBSTree.Name));
                command.ExecuteNonQuery();
                // sOldFullPath = "^" + oGWBSTree.Describe.Replace("\\", "\\\\");
                // sNewFullPath = oGWBSTree.FullPath.Replace("\\", "\\\\");

                command.CommandText = @"update thd_gwbstree t  set  t.fullpath=:newFullPath || substr(t.fullpath,length(:oldFullPath)+1)  where T.id!=:id AND theprojectguid=:projectid and t.state=1 and t.syscode like (select syscode from thd_gwbstree where id=:id and rownum=1)||'%'";
                command.Parameters.Clear();
                command.Parameters.Add(new OracleParameter("oldFullPath", oGWBSTree.Describe));
                command.Parameters.Add(new OracleParameter("id", oGWBSTree.Id));
                command.Parameters.Add(new OracleParameter("newFullPath", oGWBSTree.FullPath));
                command.Parameters.Add(new OracleParameter("projectid", oGWBSTree.TheProjectGUID));

                iRow = command.ExecuteNonQuery();
                // lstFullPath.Add(oGWBSTree.FullPath);
                //}
            }
            // session.Transaction.Commit();

        }
        /// <summary>
        /// 获取GWBS树节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetGWBSTreesByInstance(string projectId, string sSysCode, int iLevel)
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
            //oq.AddCriterion(Expression.Lt("Level", 3));
            oq.AddCriterion(Expression.Eq("Level", iLevel));
            if (!string.IsNullOrEmpty(sSysCode))
            {
                oq.AddCriterion(Expression.Like("SysCode", sSysCode + "%"));
            }
            //oq.AddOrder(Order.Asc("SysCode"));
            //oq.AddOrder(Order.Asc("CategoryNodeType"));


            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));

            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);

            return list;
        }
        ///// <summary>
        ///// 获取GWBS树节点集合(返回有权限和无权限的集合)始终加载当前节点的下一层
        ///// </summary>
        //public IList GetGWBSTreesByInstance(string projectId,int iLevel,string sSysCode)
        //{
        //    //CategoryTree tree = InitTree("GWBS树", typeof(GWBSTree));

        //    #region 获取有权限和无权限的节点

        //    //ObjectQuery oq = new ObjectQuery();
        //    //oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
        //    ////oq.AddFetchMode("ChildNodes", NHibernate.FetchMode.Eager);
        //    //oq.AddOrder(Order.Asc("Level"));
        //    //oq.AddOrder(Order.Asc("OrderNo"));
        //    //IList list = NodeSrv.GetInstanceNodesByObjectQuery(typeof(GWBSTree), oq);

        //    #endregion

        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
        //    oq.AddCriterion(Expression.Eq("State", 1));
        //    oq.AddCriterion(Expression.Lt("Level", 3));
        //    //oq.AddOrder(Order.Asc("SysCode"));
        //    //oq.AddOrder(Order.Asc("CategoryNodeType"));
        //    oq.AddOrder(Order.Asc("Level"));
        //    oq.AddOrder(Order.Asc("OrderNo"));

        //    IList list = dao.ObjectQuery(typeof(GWBSTree), oq);

        //    return list;
        //}
        /// <summary>
        /// 根据父节点层次码查找其子节点
        /// </summary>
        /// <param name="parentNodeSyscode">层次码</param>
        /// <returns></returns>
        public IList GetGWBSTreeByParentNodeSyscode(string parentNodeSyscode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode", parentNodeSyscode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("State", 1));
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
        public IList GetWBSDetailNum(string sGWBSTreeID, string sGWBSDetialID)
        {
            IList lstResult = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sql = @"select max( cast(substr(t.code,instr(t.code,'-',-1)+1) as number)) code ,max(t.orderno) orderno,
   SUM(case when t.changeparentid='{1}' then 1 else 0 end ) len from thd_gwbsdetail t  where t.parentid ='{0}'";
            sql = string.Format(sql, sGWBSTreeID, sGWBSDetialID);
            command.CommandText = sql;

            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = ds.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                lstResult.Insert(0, ClientUtil.ToInt(row["code"]));//最大code
                lstResult.Insert(1, ClientUtil.ToInt(row["orderno"]));//最大orderno
                lstResult.Insert(2, ClientUtil.ToInt(row["len"]));//最大orderno
                break;
            }
            return lstResult;
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
        /// 对象查询并加载其完整路径
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetGWBSTreeAndFullPath(ObjectQuery oq)
        {
            IList list = new ArrayList();
            list = dao.ObjectQuery(typeof(GWBSTree), oq);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    GWBSTree wbs = list[i] as GWBSTree;
                    wbs.FullPath = GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
                }
            }
            return list;
        }
        //private string GetCategorTreeFullPath(Type cateEntityType, string nodeName, string nodeSysCode)
        //{
        //    string path = string.Empty;

        //    path = nodeName;

        //    if (string.IsNullOrEmpty(nodeSysCode))
        //        return path;

        //    string[] sysCodes = nodeSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

        //    ObjectQuery oq = new ObjectQuery();
        //    Disjunction dis = new Disjunction();
        //    for (int i = 0; i < sysCodes.Length - 1; i++)
        //    {
        //        string sysCode = "";
        //        for (int j = 0; j <= i; j++)
        //        {
        //            sysCode += sysCodes[j] + ".";

        //        }

        //        dis.Add(Expression.Eq("SysCode", sysCode));
        //    }
        //    oq.AddCriterion(dis);
        //    IList list = dao.ObjectQuery(cateEntityType, oq);

        //    if (list.Count > 0)
        //    {
        //        IEnumerable<VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode> queryParent = list.OfType<VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode>();
        //        queryParent = from p in queryParent
        //                      orderby p.SysCode.Length descending
        //                      select p;

        //        foreach (VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode cate in queryParent)
        //        {
        //            if (string.IsNullOrEmpty(cate.Name) && cateEntityType.GetProperty("TaskName") != null)
        //            {
        //                string taskName = cateEntityType.GetProperty("TaskName").GetValue(cate, null).ToString();
        //                path = taskName + "\\" + path;
        //            }
        //            else
        //            {
        //                path = cate.Name + "\\" + path;
        //            }
        //        }
        //    }

        //    return path;
        //}
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
        public GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, IList listLedger)
        {
            dao.SaveOrUpdate(wbs);

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            //根据分摊比例更新子节点信息、形象进度信息，总进度计划信息
            UpdateShareRateAfterSourseChanged(wbs);
            return wbs;
        }

        /// <summary>
        /// 保存或更新GWBS,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <param name="dtl">修改后的明细</param>
        /// <param name="theCostItemId">修改明细前的明细</param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, IList listLedger, GWBSDetail dtl, GWBSDetail beforeDtl)
        {
            dao.SaveOrUpdate(wbs);

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            #region 核算明细的成本项 主资源 图号修改 其下的生产明细一样修改
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("TheGWBSSysCode", wbs.SysCode, MatchMode.Start));
            if (!string.IsNullOrEmpty(beforeDtl.MainResourceTypeId))
            {
                if (!string.IsNullOrEmpty(beforeDtl.DiagramNumber))
                {
                    oq.AddCriterion(Expression.And(Expression.And(Expression.Eq("MainResourceTypeId", beforeDtl.MainResourceTypeId), Expression.Eq("TheCostItem.Id", beforeDtl.TheCostItem.Id)), Expression.Eq("DiagramNumber", beforeDtl.DiagramNumber)));
                }
                else
                {
                    oq.AddCriterion(Expression.And(Expression.And(Expression.Eq("MainResourceTypeId", beforeDtl.MainResourceTypeId), Expression.Eq("TheCostItem.Id", beforeDtl.TheCostItem.Id)), Expression.IsNull("DiagramNumber")));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(beforeDtl.DiagramNumber))
                {
                    oq.AddCriterion(Expression.And(Expression.And(Expression.Eq("TheCostItem.Id", beforeDtl.TheCostItem.Id), Expression.Eq("DiagramNumber", beforeDtl.DiagramNumber)), Expression.IsNull("MainResourceTypeId")));
                }
                else
                {
                    oq.AddCriterion(Expression.And(Expression.And(Expression.Eq("TheCostItem.Id", beforeDtl.TheCostItem.Id), Expression.IsNull("DiagramNumber")), Expression.IsNull("MainResourceTypeId")));
                }
            }
            oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
            oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
            IList produceDetailList = dao.ObjectQuery(typeof(GWBSDetail), oq);
            if (produceDetailList != null && produceDetailList.Count > 0)
            {
                foreach (GWBSDetail d in produceDetailList)
                {
                    d.TheCostItem = dtl.TheCostItem;
                    d.TheCostItemCateSyscode = dtl.TheCostItemCateSyscode;
                    d.MainResourceTypeId = dtl.MainResourceTypeId;
                    d.MainResourceTypeName = dtl.MainResourceTypeName;
                    d.MainResourceTypeQuality = dtl.MainResourceTypeQuality;
                    d.MainResourceTypeSpec = dtl.MainResourceTypeSpec;
                    d.DiagramNumber = dtl.DiagramNumber;
                }
                dao.SaveOrUpdate(produceDetailList);
            }

            #endregion
            //根据分摊比例更新子节点信息、形象进度信息，总进度计划信息
            UpdateShareRateAfterSourseChanged(wbs);

            return wbs;
        }

        /// <summary>
        /// 保存或更新工程WBS集合,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点集合</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateGWBSTree(IList list, IList listLedger)
        {
            dao.SaveOrUpdate(list);

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            return list;
        }

        /// <summary>
        /// 保存复制的任务明细到任务节点
        /// </summary>
        /// <param name="wbsNode"></param>
        /// <param name="listDtl"></param>
        /// <returns></returns>
        public IList SaveCopyDetailByNode(ContractGroup selectedContractGroup, GWBSTree oprNode, List<GWBSDetail> listDtl)
        {
            IList listResult = new ArrayList();
            List<GWBSDetail> listNewDtl = new List<GWBSDetail>();

            if (listDtl.Count == 0)
                return listResult;

            ObjectQuery oqDtl = new ObjectQuery();
            Disjunction disDtl = new Disjunction();
            foreach (GWBSDetail dtl in listDtl)
            {
                disDtl.Add(Expression.Eq("Id", dtl.Id));
            }
            oqDtl.AddCriterion(disDtl);
            oqDtl.AddFetchMode("TheCostItem", FetchMode.Eager);//界面显示成本项名称和定额
            oqDtl.AddFetchMode("ListCostSubjectDetails", FetchMode.Eager);
            oqDtl.AddOrder(Order.Asc("OrderNo"));
            IList listCopyNodeDetail = dao.ObjectQuery(typeof(GWBSDetail), oqDtl);


            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", oprNode.Id));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

            IList listNode = dao.ObjectQuery(typeof(GWBSTree), oq);//加载明细设置节点的成本核算标记、生产确认标记以及统计各项合价
            oprNode = listNode[0] as GWBSTree;


            List<int> listAddDtlIndex = new List<int>();

            int code = oprNode.Details.Count + 1;
            int startOrderNo = 1;

            //获取父对象下最大明细号
            for (int i = 0; i < oprNode.Details.Count; i++)
            {
                GWBSDetail dtl = oprNode.Details.ElementAt(i);

                if (dtl != null && !string.IsNullOrEmpty(dtl.Code))
                {
                    try
                    {
                        if (dtl.Code.IndexOf("-") > -1)
                        {
                            int tempCode = Convert.ToInt32(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1));
                            if (tempCode >= code)
                                code = tempCode + 1;
                        }
                    }
                    catch
                    {

                    }
                }

                if (dtl.OrderNo >= startOrderNo)
                {
                    startOrderNo = dtl.OrderNo + 1;
                }
            }

            List<GWBSDetailLedger> listLedger = new List<GWBSDetailLedger>();
            DateTime serverTime = DateTime.Now;

            #region 复制任务明细、耗用明细数据

            for (int i = 0; i < listCopyNodeDetail.Count; i++)
            {
                GWBSDetail dtl = listCopyNodeDetail[i] as GWBSDetail;

                GWBSDetail tempDtl = new GWBSDetail();
                tempDtl.Code = oprNode.Code + "-" + (code + i).ToString().PadLeft(5, '0');
                tempDtl.OrderNo = startOrderNo + i;
                tempDtl.ContentDesc = dtl.ContentDesc;

                tempDtl.ContractGroupCode = selectedContractGroup.Code;
                tempDtl.ContractGroupGUID = selectedContractGroup.Id;
                tempDtl.ContractGroupName = selectedContractGroup.ContractName;
                tempDtl.ContractGroupType = selectedContractGroup.ContractGroupType;

                tempDtl.ContractPrice = dtl.ContractPrice;
                //tempDtl.ContractProjectQuantity = dtl.ContractProjectQuantity;
                //tempDtl.ContractTotalPrice = dtl.ContractTotalPrice;

                tempDtl.TheCostItem = dtl.TheCostItem;
                tempDtl.TheCostItemCateSyscode = dtl.TheCostItemCateSyscode;
                tempDtl.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
                tempDtl.CurrentStateTime = serverTime;
                tempDtl.Name = dtl.Name;

                tempDtl.PlanPrice = dtl.PlanPrice;
                //tempDtl.PlanTotalPrice = dtl.PlanTotalPrice;
                //tempDtl.PlanWorkAmount = dtl.PlanWorkAmount;

                tempDtl.PriceUnitGUID = dtl.PriceUnitGUID;
                tempDtl.PriceUnitName = dtl.PriceUnitName;

                tempDtl.ProjectTaskTypeCode = dtl.ProjectTaskTypeCode;

                tempDtl.ResponsibilitilyPrice = dtl.ResponsibilitilyPrice;
                //tempDtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyTotalPrice;
                //tempDtl.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount;

                tempDtl.Summary = dtl.Summary;

                tempDtl.ResponseFlag = dtl.ResponseFlag;
                tempDtl.CostingFlag = dtl.CostingFlag;
                tempDtl.ProduceConfirmFlag = dtl.ProduceConfirmFlag;
                tempDtl.SubContractFeeFlag = dtl.SubContractFeeFlag;

                tempDtl.SubContractStepRate = dtl.SubContractStepRate;
                tempDtl.TheGWBSSysCode = oprNode.SysCode;

                tempDtl.MainResourceTypeId = dtl.MainResourceTypeId;
                tempDtl.MainResourceTypeName = dtl.MainResourceTypeName;
                tempDtl.MainResourceTypeQuality = dtl.MainResourceTypeQuality;
                tempDtl.MainResourceTypeSpec = dtl.MainResourceTypeSpec;

                tempDtl.DiagramNumber = dtl.DiagramNumber;

                tempDtl.TheProjectGUID = oprNode.TheProjectGUID;
                tempDtl.TheProjectName = oprNode.TheProjectName;

                tempDtl.UpdatedDate = serverTime;
                tempDtl.WorkAmountUnitGUID = dtl.WorkAmountUnitGUID;
                tempDtl.WorkAmountUnitName = dtl.WorkAmountUnitName;

                tempDtl.TheGWBS = oprNode;
                tempDtl.TheGWBSFullPath = oprNode.FullPath;
                oprNode.Details.Add(tempDtl);

                listAddDtlIndex.Add(oprNode.Details.Count - 1);


                //明细分科目成本
                foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
                {
                    GWBSDetailCostSubject tempSubject = new GWBSDetailCostSubject();
                    //tempSubject.AddupAccountCost = subject.AddupAccountCost;
                    //tempSubject.AddupAccountCostEndTime = subject.AddupAccountCostEndTime;
                    //tempSubject.AddupAccountProjectAmount = subject.AddupAccountProjectAmount;
                    //tempSubject.AddupBalanceProjectAmount = subject.AddupBalanceProjectAmount;
                    //tempSubject.AddupBalanceTotalPrice = subject.AddupBalanceTotalPrice;
                    //tempSubject.AssessmentRate = subject.AssessmentRate;

                    tempSubject.ContractQuotaQuantity = subject.ContractQuotaQuantity;

                    tempSubject.ContractBasePrice = subject.ContractBasePrice;
                    tempSubject.ContractPricePercent = subject.ContractPricePercent;
                    tempSubject.ContractQuantityPrice = subject.ContractQuantityPrice;

                    tempSubject.ContractPrice = subject.ContractPrice;
                    //tempSubject.ContractProjectAmount = subject.ContractProjectAmount;
                    //tempSubject.ContractTotalPrice = subject.ContractTotalPrice;

                    tempSubject.CostAccountSubjectGUID = subject.CostAccountSubjectGUID;
                    tempSubject.CostAccountSubjectName = subject.CostAccountSubjectName;
                    tempSubject.CostAccountSubjectSyscode = subject.CostAccountSubjectSyscode;
                    tempSubject.CreateTime = serverTime;
                    //tempSubject.CurrentPeriodAccountCost = subject.CurrentPeriodAccountCost;
                    //tempSubject.CurrentPeriodAccountCostEndTime = subject.CurrentPeriodAccountCostEndTime;
                    //tempSubject.CurrentPeriodAccountProjectAmount = subject.CurrentPeriodAccountProjectAmount;
                    //tempSubject.CurrentPeriodBalanceProjectAmount = subject.CurrentPeriodBalanceProjectAmount;
                    //tempSubject.CurrentPeriodBalanceTotalPrice = subject.CurrentPeriodBalanceTotalPrice;
                    tempSubject.Name = subject.Name;

                    tempSubject.PlanQuotaNum = subject.PlanQuotaNum;

                    tempSubject.PlanBasePrice = subject.PlanBasePrice;
                    tempSubject.PlanPricePercent = subject.PlanPricePercent;
                    tempSubject.PlanPrice = subject.PlanPrice;

                    tempSubject.PlanWorkPrice = subject.PlanWorkPrice;
                    //tempSubject.PlanWorkAmount = subject.PlanWorkAmount;
                    //tempSubject.PlanTotalPrice = subject.PlanTotalPrice;

                    tempSubject.PriceUnitGUID = subject.PriceUnitGUID;
                    tempSubject.PriceUnitName = subject.PriceUnitName;
                    tempSubject.ProjectAmountUnitGUID = subject.ProjectAmountUnitGUID;
                    tempSubject.ProjectAmountUnitName = subject.ProjectAmountUnitName;
                    tempSubject.ProjectAmountWasta = subject.ProjectAmountWasta;

                    tempSubject.MainResTypeFlag = subject.MainResTypeFlag;
                    tempSubject.ResourceTypeGUID = subject.ResourceTypeGUID;
                    tempSubject.ResourceTypeCode = subject.ResourceTypeCode;
                    tempSubject.ResourceTypeName = subject.ResourceTypeName;
                    tempSubject.ResourceTypeQuality = subject.ResourceTypeQuality;
                    tempSubject.ResourceTypeSpec = subject.ResourceTypeSpec;
                    tempSubject.ResourceCateSyscode = subject.ResourceCateSyscode;

                    tempSubject.ResponsibleQuotaNum = subject.ResponsibleQuotaNum;

                    tempSubject.ResponsibleBasePrice = subject.ResponsibleBasePrice;
                    tempSubject.ResponsiblePricePercent = subject.ResponsiblePricePercent;
                    tempSubject.ResponsibilitilyPrice = subject.ResponsibilitilyPrice;

                    tempSubject.ResponsibleWorkPrice = subject.ResponsibleWorkPrice;
                    //tempSubject.ResponsibilitilyWorkAmount = subject.ResponsibilitilyWorkAmount;
                    //tempSubject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyTotalPrice;

                    tempSubject.ResourceUsageQuota = subject.ResourceUsageQuota;

                    tempSubject.State = GWBSDetailCostSubjectState.编制;

                    tempSubject.TheProjectGUID = oprNode.TheProjectGUID;
                    tempSubject.TheProjectName = oprNode.TheProjectName;

                    tempSubject.TheGWBSTree = oprNode;
                    tempSubject.TheGWBSTreeName = oprNode.Name;
                    tempSubject.TheGWBSTreeSyscode = oprNode.SysCode;

                    tempSubject.TheGWBSDetail = tempDtl;
                    tempDtl.ListCostSubjectDetails.Add(tempSubject);
                }
            }
            #endregion

            //根据明细设置任务对象的标记
            bool taskResponsibleFlag = false;
            bool taskCostAccFlag = false;
            bool taskProductConfirmFlag = false;
            bool taskSubContractFeeFlag = false;

            foreach (GWBSDetail dtl in oprNode.Details)
            {
                if (dtl.ResponseFlag == 1)
                    taskResponsibleFlag = true;
                if (dtl.CostingFlag == 1)
                    taskCostAccFlag = true;
                if (dtl.ProduceConfirmFlag == 1)
                    taskProductConfirmFlag = true;
                if (dtl.SubContractFeeFlag)
                    taskSubContractFeeFlag = true;
            }

            oprNode.ResponsibleAccFlag = taskResponsibleFlag;
            oprNode.CostAccFlag = taskCostAccFlag;
            oprNode.ProductConfirmFlag = taskProductConfirmFlag;
            oprNode.SubContractFeeFlag = taskSubContractFeeFlag;


            decimal contractTotalPrice = 0;
            decimal responsibleTotalPrice = 0;
            decimal planTotalPrice = 0;
            foreach (GWBSDetail dtl in oprNode.Details)
            {
                contractTotalPrice += dtl.ContractTotalPrice;
                responsibleTotalPrice += dtl.ResponsibilitilyTotalPrice;
                planTotalPrice += dtl.PlanTotalPrice;
            }
            oprNode.ContractTotalPrice = contractTotalPrice;
            oprNode.ResponsibilityTotalPrice = responsibleTotalPrice;
            oprNode.PlanTotalPrice = planTotalPrice;

            SaveOrUpdateGWBSTree(oprNode, listLedger);

            //添加新增的明细到Grid
            foreach (int index in listAddDtlIndex)
            {
                GWBSDetail dtl = oprNode.Details.ElementAt(index);

                listNewDtl.Add(dtl);
            }

            oprNode.Details.Clear();
            listResult.Add(oprNode);
            listResult.Add(listNewDtl);

            return listResult;
        }

        public bool SaveCopyDetailByNode(ContractGroup selectedContractGroup, List<GWBSTree> listWBSNode, List<GWBSDetail> listDtl)
        {

            ObjectQuery oqNode = new ObjectQuery();
            Disjunction disNode = new Disjunction();
            foreach (GWBSTree dtl in listWBSNode)
            {
                disNode.Add(Expression.Eq("Id", dtl.Id));
            }
            oqNode.AddCriterion(disNode);
            oqNode.AddFetchMode("Details", FetchMode.Eager);
            listWBSNode = dao.ObjectQuery(typeof(GWBSTree), oqNode).OfType<GWBSTree>().ToList();


            ObjectQuery oqDtl = new ObjectQuery();
            Disjunction disDtl = new Disjunction();
            foreach (GWBSDetail dtl in listDtl)
            {
                disDtl.Add(Expression.Eq("Id", dtl.Id));
            }
            oqDtl.AddCriterion(disDtl);
            oqDtl.AddFetchMode("ListCostSubjectDetails", FetchMode.Eager);
            oqDtl.AddOrder(Order.Asc("OrderNo"));
            listDtl = dao.ObjectQuery(typeof(GWBSDetail), oqDtl).OfType<GWBSDetail>().ToList();

            List<GWBSDetailLedger> listLedger = new List<GWBSDetailLedger>();
            DateTime serverTime = DateTime.Now;

            foreach (GWBSTree oprNode in listWBSNode)
            {
                int code = oprNode.Details.Count + 1;
                int startOrderNo = 1;

                //获取父对象下最大明细号
                for (int i = 0; i < oprNode.Details.Count; i++)
                {
                    GWBSDetail dtl = oprNode.Details.ElementAt(i);

                    if (dtl != null && !string.IsNullOrEmpty(dtl.Code))
                    {
                        try
                        {
                            if (dtl.Code.IndexOf("-") > -1)
                            {
                                int tempCode = Convert.ToInt32(dtl.Code.Substring(dtl.Code.LastIndexOf("-") + 1));
                                if (tempCode >= code)
                                    code = tempCode + 1;
                            }
                        }
                        catch
                        {

                        }
                    }

                    if (dtl.OrderNo >= startOrderNo)
                    {
                        startOrderNo = dtl.OrderNo + 1;
                    }
                }

                #region 复制任务明细、耗用明细数据

                for (int i = 0; i < listDtl.Count; i++)
                {
                    GWBSDetail dtl = listDtl[i] as GWBSDetail;

                    GWBSDetail tempDtl = new GWBSDetail();

                    tempDtl.Code = oprNode.Code + "-" + (code + i).ToString().PadLeft(5, '0');
                    tempDtl.OrderNo = startOrderNo + i;
                    tempDtl.ContentDesc = dtl.ContentDesc;

                    tempDtl.ContractGroupCode = selectedContractGroup.Code;
                    tempDtl.ContractGroupGUID = selectedContractGroup.Id;
                    tempDtl.ContractGroupName = selectedContractGroup.ContractName;
                    tempDtl.ContractGroupType = selectedContractGroup.ContractGroupType;

                    tempDtl.ContractPrice = dtl.ContractPrice;
                    //tempDtl.ContractProjectQuantity = dtl.ContractProjectQuantity;
                    //tempDtl.ContractTotalPrice = dtl.ContractTotalPrice;

                    tempDtl.TheCostItem = dtl.TheCostItem;
                    tempDtl.TheCostItemCateSyscode = dtl.TheCostItemCateSyscode;
                    tempDtl.State = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
                    tempDtl.CurrentStateTime = serverTime;
                    tempDtl.Name = dtl.Name;

                    tempDtl.PlanPrice = dtl.PlanPrice;
                    //tempDtl.PlanTotalPrice = dtl.PlanTotalPrice;
                    //tempDtl.PlanWorkAmount = dtl.PlanWorkAmount;

                    tempDtl.PriceUnitGUID = dtl.PriceUnitGUID;
                    tempDtl.PriceUnitName = dtl.PriceUnitName;

                    tempDtl.ProjectTaskTypeCode = dtl.ProjectTaskTypeCode;

                    tempDtl.ResponsibilitilyPrice = dtl.ResponsibilitilyPrice;
                    //tempDtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyTotalPrice;
                    //tempDtl.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount;

                    tempDtl.Summary = dtl.Summary;

                    tempDtl.ResponseFlag = dtl.ResponseFlag;
                    tempDtl.CostingFlag = dtl.CostingFlag;
                    tempDtl.ProduceConfirmFlag = dtl.ProduceConfirmFlag;
                    tempDtl.SubContractFeeFlag = dtl.SubContractFeeFlag;

                    tempDtl.SubContractStepRate = dtl.SubContractStepRate;
                    tempDtl.TheGWBSSysCode = oprNode.SysCode;

                    tempDtl.MainResourceTypeId = dtl.MainResourceTypeId;
                    tempDtl.MainResourceTypeName = dtl.MainResourceTypeName;
                    tempDtl.MainResourceTypeQuality = dtl.MainResourceTypeQuality;
                    tempDtl.MainResourceTypeSpec = dtl.MainResourceTypeSpec;

                    tempDtl.DiagramNumber = dtl.DiagramNumber;

                    tempDtl.TheProjectGUID = oprNode.TheProjectGUID;
                    tempDtl.TheProjectName = oprNode.TheProjectName;

                    tempDtl.UpdatedDate = serverTime;
                    tempDtl.WorkAmountUnitGUID = dtl.WorkAmountUnitGUID;
                    tempDtl.WorkAmountUnitName = dtl.WorkAmountUnitName;

                    tempDtl.TheGWBS = oprNode;
                    tempDtl.TheGWBSFullPath = oprNode.FullPath;
                    oprNode.Details.Add(tempDtl);


                    //明细分科目成本
                    foreach (GWBSDetailCostSubject subject in dtl.ListCostSubjectDetails)
                    {
                        GWBSDetailCostSubject tempSubject = new GWBSDetailCostSubject();
                        //tempSubject.AddupAccountCost = subject.AddupAccountCost;
                        //tempSubject.AddupAccountCostEndTime = subject.AddupAccountCostEndTime;
                        //tempSubject.AddupAccountProjectAmount = subject.AddupAccountProjectAmount;
                        //tempSubject.AddupBalanceProjectAmount = subject.AddupBalanceProjectAmount;
                        //tempSubject.AddupBalanceTotalPrice = subject.AddupBalanceTotalPrice;
                        //tempSubject.AssessmentRate = subject.AssessmentRate;

                        tempSubject.ContractQuotaQuantity = subject.ContractQuotaQuantity;

                        tempSubject.ContractBasePrice = subject.ContractBasePrice;
                        tempSubject.ContractPricePercent = subject.ContractPricePercent;
                        tempSubject.ContractQuantityPrice = subject.ContractQuantityPrice;

                        tempSubject.ContractPrice = subject.ContractPrice;
                        //tempSubject.ContractProjectAmount = subject.ContractProjectAmount;
                        //tempSubject.ContractTotalPrice = subject.ContractTotalPrice;

                        tempSubject.CostAccountSubjectGUID = subject.CostAccountSubjectGUID;
                        tempSubject.CostAccountSubjectName = subject.CostAccountSubjectName;
                        tempSubject.CostAccountSubjectSyscode = subject.CostAccountSubjectSyscode;
                        tempSubject.CreateTime = serverTime;
                        //tempSubject.CurrentPeriodAccountCost = subject.CurrentPeriodAccountCost;
                        //tempSubject.CurrentPeriodAccountCostEndTime = subject.CurrentPeriodAccountCostEndTime;
                        //tempSubject.CurrentPeriodAccountProjectAmount = subject.CurrentPeriodAccountProjectAmount;
                        //tempSubject.CurrentPeriodBalanceProjectAmount = subject.CurrentPeriodBalanceProjectAmount;
                        //tempSubject.CurrentPeriodBalanceTotalPrice = subject.CurrentPeriodBalanceTotalPrice;
                        tempSubject.Name = subject.Name;

                        tempSubject.PlanQuotaNum = subject.PlanQuotaNum;

                        tempSubject.PlanBasePrice = subject.PlanBasePrice;
                        tempSubject.PlanPricePercent = subject.PlanPricePercent;
                        tempSubject.PlanPrice = subject.PlanPrice;

                        tempSubject.PlanWorkPrice = subject.PlanWorkPrice;
                        //tempSubject.PlanWorkAmount = subject.PlanWorkAmount;
                        //tempSubject.PlanTotalPrice = subject.PlanTotalPrice;

                        tempSubject.PriceUnitGUID = subject.PriceUnitGUID;
                        tempSubject.PriceUnitName = subject.PriceUnitName;
                        tempSubject.ProjectAmountUnitGUID = subject.ProjectAmountUnitGUID;
                        tempSubject.ProjectAmountUnitName = subject.ProjectAmountUnitName;
                        tempSubject.ProjectAmountWasta = subject.ProjectAmountWasta;

                        tempSubject.MainResTypeFlag = subject.MainResTypeFlag;
                        tempSubject.ResourceTypeGUID = subject.ResourceTypeGUID;
                        tempSubject.ResourceTypeCode = subject.ResourceTypeCode;
                        tempSubject.ResourceTypeName = subject.ResourceTypeName;
                        tempSubject.ResourceTypeQuality = subject.ResourceTypeQuality;
                        tempSubject.ResourceTypeSpec = subject.ResourceTypeSpec;
                        tempSubject.ResourceCateSyscode = subject.ResourceCateSyscode;

                        tempSubject.ResponsibleQuotaNum = subject.ResponsibleQuotaNum;

                        tempSubject.ResponsibleBasePrice = subject.ResponsibleBasePrice;
                        tempSubject.ResponsiblePricePercent = subject.ResponsiblePricePercent;
                        tempSubject.ResponsibilitilyPrice = subject.ResponsibilitilyPrice;

                        tempSubject.ResponsibleWorkPrice = subject.ResponsibleWorkPrice;
                        //tempSubject.ResponsibilitilyWorkAmount = subject.ResponsibilitilyWorkAmount;
                        //tempSubject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyTotalPrice;

                        tempSubject.ResourceUsageQuota = subject.ResourceUsageQuota;

                        tempSubject.State = GWBSDetailCostSubjectState.编制;

                        tempSubject.TheProjectGUID = oprNode.TheProjectGUID;
                        tempSubject.TheProjectName = oprNode.TheProjectName;

                        tempSubject.TheGWBSTree = oprNode;
                        tempSubject.TheGWBSTreeName = oprNode.Name;
                        tempSubject.TheGWBSTreeSyscode = oprNode.SysCode;

                        tempSubject.TheGWBSDetail = tempDtl;
                        tempDtl.ListCostSubjectDetails.Add(tempSubject);
                    }
                }
                #endregion

                //根据明细设置任务对象的标记
                bool taskResponsibleFlag = false;
                bool taskCostAccFlag = false;
                bool taskProductConfirmFlag = false;
                bool taskSubContractFeeFlag = false;

                foreach (GWBSDetail dtl in oprNode.Details)
                {
                    if (dtl.ResponseFlag == 1)
                        taskResponsibleFlag = true;
                    if (dtl.CostingFlag == 1)
                        taskCostAccFlag = true;
                    if (dtl.ProduceConfirmFlag == 1)
                        taskProductConfirmFlag = true;
                    if (dtl.SubContractFeeFlag)
                        taskSubContractFeeFlag = true;
                }

                oprNode.ResponsibleAccFlag = taskResponsibleFlag;
                oprNode.CostAccFlag = taskCostAccFlag;
                oprNode.ProductConfirmFlag = taskProductConfirmFlag;
                oprNode.SubContractFeeFlag = taskSubContractFeeFlag;


                decimal contractTotalPrice = 0;
                decimal responsibleTotalPrice = 0;
                decimal planTotalPrice = 0;
                foreach (GWBSDetail dtl in oprNode.Details)
                {
                    contractTotalPrice += dtl.ContractTotalPrice;
                    responsibleTotalPrice += dtl.ResponsibilitilyTotalPrice;
                    planTotalPrice += dtl.PlanTotalPrice;
                }
                oprNode.ContractTotalPrice = contractTotalPrice;
                oprNode.ResponsibilityTotalPrice = responsibleTotalPrice;
                oprNode.PlanTotalPrice = planTotalPrice;

            }

            dao.SaveOrUpdate(listWBSNode);

            dao.Save(listLedger);

            return true;
        }

        /// <summary>
        /// 保存或修改工程WBS明细（1.任务明细，2.明细变更台账）
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>1.任务明细，2.明细变更台账</returns>
        [TransManager]
        public IList SaveOrUpdateDetail(GWBSDetail dtl, IList listLedger)
        {
            dtl.UpdatedDate = DateTime.Now;

            dao.SaveOrUpdate(dtl);

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            IList listReturnValue = new ArrayList();
            listReturnValue.Add(dtl);
            listReturnValue.Add(listLedger);

            return listReturnValue;
        }

        /// <summary>
        ///  变更工程WBS明细（1.任务明细，2.明细变更台账）
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>1.任务明细，2.明细变更台账</returns>
        [TransManager]
        public IList ChangeTaskDetail(GWBSDetail dtl, IList listLedger)
        {
            dtl.UpdatedDate = DateTime.Now;

            dao.SaveOrUpdate(dtl);

            GWBSTree task = dao.Get(typeof(GWBSTree), dtl.TheGWBS.Id) as GWBSTree;
            task = AccountTotalPrice(task);
            dao.Update(task);

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            IList listReturnValue = new ArrayList();
            listReturnValue.Add(dtl);
            listReturnValue.Add(listLedger);

            return listReturnValue;
        }

        /// <summary>
        /// 保存或修改工程WBS明细集
        /// </summary>
        /// <param name="dtl">明细对象集合</param>
        /// <param name="dtl">明细耗用对象集合</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>返回对象集合（1.任务明细集，2.资源耗用集）</returns>
        [TransManager]
        public IList SaveOrUpdateDetail(IList listDtl, IList listDtlUsage, IList listLedger)
        {
            if (listDtlUsage != null && listDtlUsage.Count > 0)
            {
                for (int i = 0; i < listDtlUsage.Count; i++)
                {
                    GWBSDetailCostSubject dtl = listDtlUsage[i] as GWBSDetailCostSubject;
                    dtl.UpdateTime = DateTime.Now;
                    listDtlUsage[i] = dtl;
                }
                dao.SaveOrUpdate(listDtlUsage);
            }

            if (listDtl != null && listDtl.Count > 0)
            {

                for (int i = 0; i < listDtl.Count; i++)
                {
                    GWBSDetail dtl = listDtl[i] as GWBSDetail;
                    dtl.UpdatedDate = DateTime.Now;
                    dtl.TheGWBSSysCode = dtl.TheGWBS.SysCode;
                    listDtl[i] = dtl;

                }
                dao.SaveOrUpdate(listDtl);


                IList listUsage = new ArrayList();

                IEnumerable<GWBSDetail> queryDtl = listDtl.OfType<GWBSDetail>();
                var query = from d in queryDtl
                            where d.State == DocumentState.Invalid
                            select d;
                if (query.Count() > 0)
                {
                    ObjectQuery oq = new ObjectQuery();
                    Disjunction dis = new Disjunction();
                    foreach (GWBSDetail dtl in query)
                    {
                        dis.Add(Expression.Eq("TheGWBSDetail.Id", dtl.Id));
                    }
                    oq.AddCriterion(dis);
                    IList list = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
                    foreach (GWBSDetailCostSubject usage in list)
                    {
                        usage.State = GWBSDetailCostSubjectState.作废;
                        listUsage.Add(usage);
                    }
                }

                query = from d in queryDtl
                        where d.State == DocumentState.InExecute
                        select d;
                if (query.Count() > 0)
                {
                    ObjectQuery oq = new ObjectQuery();
                    Disjunction dis = new Disjunction();
                    foreach (GWBSDetail dtl in query)
                    {
                        dis.Add(Expression.Eq("TheGWBSDetail.Id", dtl.Id));
                    }
                    oq.AddCriterion(dis);
                    IList list = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
                    foreach (GWBSDetailCostSubject usage in list)
                    {
                        usage.State = GWBSDetailCostSubjectState.生效;
                        listUsage.Add(usage);
                    }
                }
                if (listUsage.Count > 0)
                    dao.Update(listUsage);

            }

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            IList listReturnValue = new ArrayList();
            listReturnValue.Add(listDtl);
            listReturnValue.Add(listDtlUsage);

            return listReturnValue;
        }

        /// <summary>
        /// 保存或修改工程GWBS和GWBS明细
        /// </summary>
        /// <param name="dtl">项目任务集</param>
        /// <param name="dtl">任务明细集</param>
        /// <returns>返回对象集合（1.项目任务集,2.任务明细集）</returns>
        [TransManager]
        public IList SaveOrUpdateDetail(IList listGWBSNode, IList listGWBSDtl, bool isReturnValue)
        {
            if (listGWBSNode != null && listGWBSNode.Count > 0)
                dao.SaveOrUpdate(listGWBSNode);

            if (listGWBSDtl != null && listGWBSDtl.Count > 0)
                dao.SaveOrUpdate(listGWBSDtl);

            IList listReturnValue = null;
            if (isReturnValue)
            {
                listReturnValue = new ArrayList();
                listReturnValue.Add(listGWBSNode);
                listReturnValue.Add(listGWBSDtl);
            }

            return listReturnValue;
        }

        /// <summary>
        /// 保存或修改工程WBS明细集
        /// </summary>
        /// <param name="dtl">明细对象集合</param>
        /// <param name="dtl">明细耗用对象集合</param>
        /// <returns>返回对象集合（1.任务明细集，2.资源耗用集）</returns>
        [TransManager]
        public IList SaveOrUpdateDetailByCostEdit(IList listDtl, IList listLedger, IList listDtlUsage, List<string> listDeleteDtlUsages)
        {
            if (listDtlUsage != null && listDtlUsage.Count > 0)
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (GWBSDetailCostSubject dtl in listDtlUsage)
                {
                    dtl.UpdateTime = DateTime.Now;
                    dis.Add(Expression.Eq("Id", dtl.Id));
                }
                dao.SaveOrUpdate(listDtlUsage);

                //oq.AddCriterion(dis);
                //oq.AddFetchMode("ResourceUsageQuota", FetchMode.Eager);
                //listDtlUsage = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
            }

            if (listDeleteDtlUsages != null && listDeleteDtlUsages.Count > 0)
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (string usageId in listDeleteDtlUsages)
                {
                    dis.Add(Expression.Eq("Id", usageId));
                }
                oq.AddCriterion(dis);

                IList listDeleteUsages = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);

                if (listDeleteUsages.Count > 0)
                    dao.Delete(listDeleteUsages);
            }

            if (listDtl != null && listDtl.Count > 0)
            {
                foreach (GWBSDetail dtl in listDtl)
                {
                    dtl.UpdatedDate = DateTime.Now;
                }
                dao.SaveOrUpdate(listDtl);


                //更新项目任务的合同、责任、计划合价
                var queryGroup = from d in listDtl.OfType<GWBSDetail>()
                                 group d by new { d.TheGWBS.Id, d.TheGWBSSysCode } into g
                                 select new { g.Key.Id, g.Key.TheGWBSSysCode };

                ObjectQuery oqTask = new ObjectQuery();
                Disjunction disTask = new Disjunction();

                ObjectQuery oqDtl = new ObjectQuery();
                Disjunction disDtl = new Disjunction();

                foreach (var obj in queryGroup)
                {
                    disTask.Add(Expression.Eq("Id", obj.Id));

                    disDtl.Add(Expression.Like("TheGWBSSysCode", obj.TheGWBSSysCode, MatchMode.Start));
                }
                oqTask.AddCriterion(disTask);

                oqDtl.AddCriterion(disDtl);
                oqDtl.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));

                IList listTask = dao.ObjectQuery(typeof(GWBSTree), oqTask);
                IEnumerable<GWBSDetail> listTaskDtl = dao.ObjectQuery(typeof(GWBSDetail), oqDtl).OfType<GWBSDetail>();

                foreach (GWBSTree oprNode in listTask)
                {
                    var queryDtl = from d in listTaskDtl
                                   where d.TheGWBSSysCode.IndexOf(oprNode.SysCode) > -1
                                   select d;

                    decimal contractTotalPrice = 0;
                    decimal responsibleTotalPrice = 0;
                    decimal planTotalPrice = 0;
                    foreach (GWBSDetail dtl in queryDtl)
                    {
                        contractTotalPrice += dtl.ContractTotalPrice;
                        if (dtl.ResponseFlag == 1)
                            responsibleTotalPrice += dtl.ResponsibilitilyTotalPrice;
                        if (dtl.CostingFlag == 1)
                            planTotalPrice += dtl.PlanTotalPrice;
                    }

                    var queryDtlFlag = from d in queryDtl
                                       where d.TheGWBS.Id == oprNode.Id && d.ResponseFlag == 1
                                       select d;
                    oprNode.ResponsibleAccFlag = queryDtlFlag.Count() > 0 ? true : false;

                    queryDtlFlag = from d in queryDtl
                                   where d.TheGWBS.Id == oprNode.Id && d.CostingFlag == 1
                                   select d;
                    oprNode.CostAccFlag = queryDtlFlag.Count() > 0 ? true : false;

                    oprNode.ContractTotalPrice = contractTotalPrice;
                    oprNode.ResponsibilityTotalPrice = responsibleTotalPrice;
                    oprNode.PlanTotalPrice = planTotalPrice;
                }

                dao.Update(listTask);
            }

            if (listLedger != null && listLedger.Count > 0)
            {
                dao.Save(listLedger);
            }

            IList listReturnValue = new ArrayList();

            listReturnValue.Add(listDtl);
            listReturnValue.Add(listDtlUsage);

            return listReturnValue;
        }

        /// <summary>
        /// 保存或修改任务明细和父对象
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="parentNode">父对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>返回对象集合（1.任务明细，2.父节点）</returns>
        [TransManager]
        public IList SaveOrUpdateDetail(GWBSDetail dtl, GWBSTree parentNode, IList listLedger)
        {

            parentNode.UpdatedDate = DateTime.Now;

            dao.SaveOrUpdate(parentNode);

            if (dtl != null)
            {
                dtl.UpdatedDate = DateTime.Now;

                dao.SaveOrUpdate(dtl);
            }

            if (listLedger != null && listLedger.Count > 0)
                dao.Save(listLedger);

            IList listReturnValue = new ArrayList();
            listReturnValue.Add(dtl);
            listReturnValue.Add(parentNode);

            return listReturnValue;
        }

        /// <summary>
        /// 保存或修改工程WBS明细集合
        /// </summary>
        /// <param name="list">明细集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateDetail(IList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                GWBSDetail dtl = list[i] as GWBSDetail;
                dtl.UpdatedDate = DateTime.Now;

                dao.SaveOrUpdate(dtl);

                list[i] = dtl;
            }

            return list;
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

        #region 工程任务类型文档模版
        /// <summary>
        /// 保存或修改工程任务类型文档模版
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public ProTaskTypeDocumentStencil SaveOrUpDateDocStencil(ProTaskTypeDocumentStencil obj)
        {
            if (dao.SaveOrUpdate(obj))
            {
                return obj;
            }
            else
            {
                return null;
            }

        }
        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(IList list)
        {
            return dao.Delete(list);
        }

        /// <summary>
        /// 查询成本核算任务明细
        /// </summary>
        /// <param name="project"></param>
        /// <param name="gwbs"></param>
        /// <param name="taskType"></param>
        /// <param name="costItem"></param>
        /// <param name="accSubject"></param>
        /// <param name="mat"></param>
        /// <param name="usageName"></param>
        /// <returns>（1.任务明细集，2.任务明细资源耗用集）</returns>
        public IList GetCostAccDtl(ObjectQuery oq, CostAccountSubject accSubject, MaterialCategory matCate, Material mat, string usageName)
        {
            IList listResult = new ArrayList();
            IList listTaskDtl = new ArrayList();
            IList listDtlUsage = new ArrayList();

            oq.AddFetchMode("WorkAmountUnitGUID", FetchMode.Eager);
            listTaskDtl = dao.ObjectQuery(typeof(GWBSDetail), oq);

            if (listTaskDtl != null && listTaskDtl.Count > 0)
            {
                oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (GWBSDetail dtl in listTaskDtl)
                {
                    dis.Add(Expression.Eq("TheGWBSDetail.Id", dtl.Id));
                }
                oq.AddCriterion(dis);

                if (mat != null)
                    oq.AddCriterion(Expression.Eq("ResourceTypeGUID", mat.Id));

                if (accSubject != null)
                    oq.AddCriterion(Expression.Like("CostAccountSubjectSyscode", accSubject.SysCode, MatchMode.Start));

                if (matCate != null)
                    oq.AddCriterion(Expression.Like("ResourceCateSyscode", matCate.SysCode, MatchMode.Start));

                if (!string.IsNullOrEmpty(usageName))
                    oq.AddCriterion(Expression.Like("Name", usageName, MatchMode.Anywhere));

                listDtlUsage = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);

                //加载耗用关联的成本库定额
                var queryQuotaGroup = from s in listDtlUsage.OfType<GWBSDetailCostSubject>()
                                      where s.ResourceUsageQuota != null
                                      group s by new { s.ResourceUsageQuota.Id } into g
                                      select g.Key.Id;

                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                dis = new Disjunction();
                foreach (var quotaId in queryQuotaGroup)
                {
                    dis.Add(Expression.Eq("Id", quotaId));
                }
                oq.AddCriterion(dis);
                IEnumerable<SubjectCostQuota> listQuota = dao.ObjectQuery(typeof(SubjectCostQuota), oq).OfType<SubjectCostQuota>();

                foreach (GWBSDetailCostSubject s in listDtlUsage)
                {
                    var query = from q in listQuota
                                where s.ResourceUsageQuota != null && q.Id == s.ResourceUsageQuota.Id
                                select q;
                    if (query.Count() > 0)
                        s.ResourceUsageQuota = query.ElementAt(0);
                }

                //过滤没有耗用的任务明细
                IEnumerable<GWBSDetailCostSubject> listTempDtlUsage = listDtlUsage.OfType<GWBSDetailCostSubject>();
                var queryGroup = from u in listTempDtlUsage
                                 group u by new { u.TheGWBSDetail.Id } into g
                                 select g;

                for (int i = listTaskDtl.Count - 1; i > -1; i--)
                {
                    GWBSDetail dtl = listTaskDtl[i] as GWBSDetail;

                    var query = from d in queryGroup
                                where d.Key.Id == dtl.Id
                                select d;

                    if (query.Count() == 0)
                    {
                        listTaskDtl.RemoveAt(i);
                    }
                }

                //更新耗用上的任务明细
                foreach (GWBSDetailCostSubject s in listDtlUsage)
                {
                    var q = from d in listTaskDtl.OfType<GWBSDetail>()
                            where d.Id == s.TheGWBSDetail.Id
                            select d;

                    s.TheGWBSDetail = q.ElementAt(0);
                }
            }

            //foreach (GWBSDetail dtl in listTaskDtl)
            //{
            //    dtl.TheGWBSFullPath = GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBS.Name, dtl.TheGWBSSysCode);
            //}

            listResult.Add(listTaskDtl);
            listResult.Add(listDtlUsage);

            return listResult;

        }

        /// <summary>
        /// 获取分类树节点的完整路径
        /// </summary>
        /// <param name="cateEntityType"></param>
        /// <param name="nodeObj"></param>
        /// <returns></returns>
        public string GetCategorTreeFullPath(Type cateEntityType, VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode nodeObj)
        {
            string path = string.Empty;

            string[] sysCodes = nodeObj.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            for (int i = 0; i < sysCodes.Length; i++)
            {
                string sysCode = "";
                for (int j = 0; j <= i; j++)
                {
                    sysCode += sysCodes[j] + ".";

                }

                dis.Add(Expression.Eq("SysCode", sysCode));
            }
            oq.AddCriterion(dis);
            IList list = dao.ObjectQuery(cateEntityType, oq);

            if (list.Count > 0)
            {
                IEnumerable<VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode> queryParent = list.OfType<VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode>();
                queryParent = from p in queryParent
                              orderby p.SysCode.Length descending
                              select p;

                foreach (VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode cate in queryParent)
                {
                    if (string.IsNullOrEmpty(cate.Name) && cateEntityType.GetProperty("TaskName") != null)
                    {
                        string taskName = cateEntityType.GetProperty("TaskName").GetValue(cate, null).ToString();
                        path = taskName + "\\" + path;
                    }
                    else
                    {
                        path = cate.Name + "\\" + path;
                    }
                }
            }


            return path;
        }

        /// <summary>
        /// 获取分类树节点的完整路径
        /// </summary>
        /// <param name="cateEntityType"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeSysCode"></param>
        /// <returns></returns>
        public string GetCategorTreeFullPath(Type cateEntityType, string nodeName, string nodeSysCode)
        {
            string path = string.Empty;

            path = nodeName;

            if (string.IsNullOrEmpty(nodeSysCode))
                return path;

            string[] sysCodes = nodeSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            for (int i = 0; i < sysCodes.Length - 1; i++)
            {
                string sysCode = "";
                for (int j = 0; j <= i; j++)
                {
                    sysCode += sysCodes[j] + ".";

                }

                dis.Add(Expression.Eq("SysCode", sysCode));
            }
            oq.AddCriterion(dis);
            IList list = dao.ObjectQuery(cateEntityType, oq);

            if (list.Count > 0)
            {
                IEnumerable<VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode> queryParent = list.OfType<VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode>();
                queryParent = from p in queryParent
                              orderby p.SysCode.Length descending
                              select p;

                foreach (VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode cate in queryParent)
                {
                    if (string.IsNullOrEmpty(cate.Name) && cateEntityType.GetProperty("TaskName") != null)
                    {
                        string taskName = cateEntityType.GetProperty("TaskName").GetValue(cate, null).ToString();
                        path = taskName + "\\" + path;
                    }
                    else
                    {
                        path = cate.Name + "\\" + path;
                    }
                }
            }

            return path;
        }


        /// <summary>
        /// 计算分包取费任务明细的预算成本
        /// </summary>
        /// <param name="listDtl">分包取费明细集</param>
        /// <returns></returns>
        public IList AccountSubContractFeeDtl(IList listDtl, IList listDtlUsage, DocumentState state)
        {
            IList listResult = new ArrayList();

            IEnumerable<GWBSDetailCostSubject> listDtlUsageEdit = listDtlUsage.OfType<GWBSDetailCostSubject>();

            ObjectQuery oq = new ObjectQuery();
            foreach (GWBSDetail dtl in listDtl)
            {
                oq.Criterions.Clear();

                //1.根据分包措施明细的过滤条件查询下属实体任务明细的资源耗用

                oq.AddCriterion(Expression.Like("TheGWBSSysCode", dtl.TheGWBSSysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("SubContractFeeFlag", false));
                //oq.AddCriterion(Expression.Eq("State", GWBSDetailState.有效));
                //oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));
                if (state != DocumentState.Invalid)
                {
                    oq.AddCriterion(Expression.Eq("State", state));
                }
                else
                {
                    oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));
                }

                Disjunction dis = new Disjunction();//使用Disjunction 如果分包取费明细不包含 责任核算标记和成本核算标记 时则不查出任何核素任务明细数据
                if (dtl.ResponseFlag == 1 && dtl.CostingFlag == 1)
                {
                    dis.Add(Expression.Eq("ResponseFlag", 1));
                    dis.Add(Expression.Eq("CostingFlag", 1));
                }
                else if (dtl.ResponseFlag == 1)
                {
                    dis.Add(Expression.Eq("ResponseFlag", 1));
                }
                else if (dtl.CostingFlag == 1)
                {
                    dis.Add(Expression.Eq("CostingFlag", 1));
                }
                oq.AddCriterion(dis);


                #region 按成本项分类过滤（AND关系，各个分类之间OR关系）

                //如果未设置分类过滤条件则查询所有
                Disjunction dis1 = new Disjunction();
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode1))
                {
                    dis1.Add(Expression.Like("TheCostItem.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode1, MatchMode.Start));
                }
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode2))
                {
                    dis1.Add(Expression.Like("TheCostItem.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode2, MatchMode.Start));
                }
                string term = dis1.ToString();
                if (term != "()")//不加条件时为()
                    oq.AddCriterion(dis1);

                #endregion

                IEnumerable<GWBSDetail> listAccountDtl = dao.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>();


                if (listAccountDtl.Count() > 0
                    && (dtl.TheCostItem.SubjectCateFilter1 != null || dtl.TheCostItem.SubjectCateFilter2 != null || dtl.TheCostItem.SubjectCateFilter3 != null)
                    )//过滤核算科目
                {
                    #region 下属资源耗用按核算科目过滤（AND关系，各个科目之间OR关系）

                    ObjectQuery oqUsage = new ObjectQuery();

                    Disjunction disUsageDtl = new Disjunction();
                    foreach (GWBSDetail item in listAccountDtl)
                    {
                        disUsageDtl.Add(Expression.Eq("TheGWBSDetail.Id", item.Id));
                    }
                    oqUsage.AddCriterion(disUsageDtl);


                    Disjunction disUsage = new Disjunction();
                    if (dtl.TheCostItem.SubjectCateFilter1 != null)
                    {
                        disUsage.Add(Expression.Eq("CostAccountSubjectGUID.Id", dtl.TheCostItem.SubjectCateFilter1.Id));
                    }
                    if (dtl.TheCostItem.SubjectCateFilter2 != null)
                    {
                        disUsage.Add(Expression.Eq("CostAccountSubjectGUID.Id", dtl.TheCostItem.SubjectCateFilter2.Id));
                    }
                    if (dtl.TheCostItem.SubjectCateFilter3 != null)
                    {
                        disUsage.Add(Expression.Eq("CostAccountSubjectGUID.Id", dtl.TheCostItem.SubjectCateFilter3.Id));
                    }
                    oqUsage.AddCriterion(disUsage);

                    #endregion

                    IEnumerable<GWBSDetailCostSubject> listAccountUsage = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oqUsage).OfType<GWBSDetailCostSubject>();

                    if (listAccountUsage.Count() > 0)
                    {
                        #region 计算措施费明细的量价
                        //计算合同预算量价
                        if (true)
                        {
                            decimal sumResponsibleTotalPrice = listAccountUsage.Sum(p => p.ContractTotalPrice) * dtl.TheCostItem.PricingRate;

                            dtl.ContractProjectQuantity = 1;
                            dtl.ContractPrice = sumResponsibleTotalPrice;
                            dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;

                            var queryUsage = from d in listDtlUsageEdit
                                             where d.TheGWBSDetail.Id == dtl.Id
                                             select d;

                            foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                            {
                                dtlUsage.ContractQuotaQuantity = 1;
                                dtlUsage.ContractQuantityPrice = dtl.ContractTotalPrice;
                                if (dtlUsage.ResourceUsageQuota != null)
                                    dtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                                dtlUsage.ContractPricePercent = 1;
                                dtlUsage.ContractBasePrice = dtlUsage.ContractQuantityPrice;

                                dtlUsage.ContractPrice = dtlUsage.ContractQuotaQuantity * dtlUsage.ContractQuantityPrice;
                                dtlUsage.ContractProjectAmount = dtl.ContractProjectQuantity * dtlUsage.ContractQuotaQuantity;
                                dtlUsage.ContractTotalPrice = dtlUsage.ContractProjectAmount * dtlUsage.ContractQuantityPrice;
                            }
                        }

                        //预算类型为责任成本
                        if (dtl.ResponseFlag == 1)
                        {
                            IEnumerable<GWBSDetail> listResponsibleDtl = from d in listAccountDtl
                                                                         where d.ResponseFlag == 1
                                                                         select d;
                            List<string> listResponsibleDtlId = new List<string>();
                            foreach (GWBSDetail d in listResponsibleDtl)
                            {
                                listResponsibleDtlId.Add(d.Id);
                            }

                            IEnumerable<GWBSDetailCostSubject> queryResponsible = from u in listAccountUsage
                                                                                  where listResponsibleDtlId.Contains(u.TheGWBSDetail.Id)
                                                                                  select u;

                            decimal sumResponsibleTotalPrice = queryResponsible.Sum(p => p.ResponsibilitilyTotalPrice) * dtl.TheCostItem.PricingRate;

                            dtl.ResponsibilitilyWorkAmount = 1;
                            dtl.ResponsibilitilyPrice = sumResponsibleTotalPrice;
                            dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                            var queryUsage = from d in listDtlUsageEdit
                                             where d.TheGWBSDetail.Id == dtl.Id
                                             select d;

                            foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                            {
                                dtlUsage.ResponsibleQuotaNum = 1;
                                dtlUsage.ResponsibilitilyPrice = dtl.ResponsibilitilyTotalPrice;
                                if (dtlUsage.ResourceUsageQuota != null)
                                    dtlUsage.ResponsibilitilyPrice = dtlUsage.ResponsibilitilyPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                                dtlUsage.ResponsiblePricePercent = 1;
                                dtlUsage.ResponsibleBasePrice = dtl.ResponsibilitilyPrice;

                                dtlUsage.ResponsibleWorkPrice = dtlUsage.ResponsibleQuotaNum * dtlUsage.ResponsibilitilyPrice;
                                dtlUsage.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleQuotaNum;
                                dtlUsage.ResponsibilitilyTotalPrice = dtlUsage.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleWorkPrice;
                            }
                        }

                        //预算类型为计划成本
                        if (dtl.CostingFlag == 1)
                        {
                            IEnumerable<GWBSDetail> listPlanDtl = from d in listAccountDtl
                                                                  where d.CostingFlag == 1
                                                                  select d;
                            List<string> listPlanDtlId = new List<string>();
                            foreach (GWBSDetail d in listPlanDtl)
                            {
                                listPlanDtlId.Add(d.Id);
                            }

                            IEnumerable<GWBSDetailCostSubject> queryPlan = from u in listAccountUsage
                                                                           where listPlanDtlId.Contains(u.TheGWBSDetail.Id)
                                                                           select u;

                            decimal sumPlanTotalPrice = queryPlan.Sum(p => p.PlanTotalPrice) * dtl.TheCostItem.PricingRate;

                            dtl.PlanWorkAmount = 1;
                            dtl.PlanPrice = sumPlanTotalPrice;
                            dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                            var queryUsage = from d in listDtlUsageEdit
                                             where d.TheGWBSDetail.Id == dtl.Id
                                             select d;

                            foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                            {
                                dtlUsage.PlanQuotaNum = 1;
                                dtlUsage.PlanPrice = dtl.PlanTotalPrice;
                                if (dtlUsage.ResourceUsageQuota != null)
                                    dtlUsage.PlanPrice = dtlUsage.PlanPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                                dtlUsage.PlanPricePercent = 1;
                                dtlUsage.PlanBasePrice = dtlUsage.PlanPrice;

                                dtlUsage.PlanWorkPrice = dtlUsage.PlanQuotaNum * dtlUsage.PlanPrice;
                                dtlUsage.PlanWorkAmount = dtl.PlanWorkAmount * dtlUsage.PlanQuotaNum;
                                dtlUsage.PlanTotalPrice = dtlUsage.PlanWorkAmount * dtlUsage.PlanWorkPrice;
                            }
                        }
                        #endregion
                    }
                }
                else if (listAccountDtl.Count() > 0)
                {
                    #region 计算措施费明细的量价
                    //计算合同预算量价
                    if (true)
                    {
                        decimal sumResponsibleTotalPrice = listAccountDtl.Sum(p => p.ContractTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.ContractProjectQuantity = 1;
                        dtl.ContractPrice = sumResponsibleTotalPrice;
                        dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;

                        var queryUsage = from d in listDtlUsageEdit
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.ContractQuotaQuantity = 1;
                            dtlUsage.ContractQuantityPrice = dtl.ContractTotalPrice;
                            if (dtlUsage.ResourceUsageQuota != null)
                                dtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                            dtlUsage.ContractPricePercent = 1;
                            dtlUsage.ContractBasePrice = dtlUsage.ContractQuantityPrice;

                            dtlUsage.ContractPrice = dtlUsage.ContractQuotaQuantity * dtlUsage.ContractQuantityPrice;
                            dtlUsage.ContractProjectAmount = dtl.ContractProjectQuantity * dtlUsage.ContractQuotaQuantity;
                            dtlUsage.ContractTotalPrice = dtlUsage.ContractProjectAmount * dtlUsage.ContractPrice;
                        }
                    }

                    //预算类型为责任成本
                    if (dtl.ResponseFlag == 1)
                    {
                        IEnumerable<GWBSDetail> listResponsibleDtl = from d in listAccountDtl
                                                                     where d.ResponseFlag == 1
                                                                     select d;

                        decimal sumResponsibleTotalPrice = listResponsibleDtl.Sum(p => p.ResponsibilitilyTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.ResponsibilitilyWorkAmount = 1;
                        dtl.ResponsibilitilyPrice = sumResponsibleTotalPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        var queryUsage = from d in listDtlUsageEdit
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.ResponsibleQuotaNum = 1;
                            dtlUsage.ResponsibilitilyPrice = dtl.ResponsibilitilyTotalPrice;
                            if (dtlUsage.ResourceUsageQuota != null)
                                dtlUsage.ResponsibilitilyPrice = dtlUsage.ResponsibilitilyPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                            dtlUsage.ResponsiblePricePercent = 1;
                            dtlUsage.ResponsibleBasePrice = dtl.ResponsibilitilyPrice;

                            dtlUsage.ResponsibleWorkPrice = dtlUsage.ResponsibleQuotaNum * dtlUsage.ResponsibilitilyPrice;
                            dtlUsage.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleQuotaNum;
                            dtlUsage.ResponsibilitilyTotalPrice = dtlUsage.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleWorkPrice;
                        }
                    }

                    //预算类型为计划成本
                    if (dtl.CostingFlag == 1)
                    {
                        IEnumerable<GWBSDetail> listPlanDtl = from d in listAccountDtl
                                                              where d.CostingFlag == 1
                                                              select d;

                        decimal sumPlanTotalPrice = listPlanDtl.Sum(p => p.PlanTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.PlanWorkAmount = 1;
                        dtl.PlanPrice = sumPlanTotalPrice;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        var queryUsage = from d in listDtlUsageEdit
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.PlanQuotaNum = 1;
                            dtlUsage.PlanPrice = dtl.PlanTotalPrice;
                            if (dtlUsage.ResourceUsageQuota != null)
                                dtlUsage.PlanPrice = dtlUsage.PlanPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                            dtlUsage.PlanPricePercent = 1;
                            dtlUsage.PlanBasePrice = dtlUsage.PlanPrice;

                            dtlUsage.PlanWorkPrice = dtlUsage.PlanQuotaNum * dtlUsage.PlanPrice;
                            dtlUsage.PlanWorkAmount = dtl.PlanWorkAmount * dtlUsage.PlanQuotaNum;
                            dtlUsage.PlanTotalPrice = dtlUsage.PlanWorkAmount * dtlUsage.PlanWorkPrice;
                        }
                    }
                    #endregion
                }
            }

            listResult.Add(listDtl);
            listResult.Add(listDtlUsage);

            return listResult;
        }

        /// <summary>
        /// 计算分包取费任务明细的预算成本
        /// </summary>
        /// <param name="listDtl">分包取费明细集</param>
        /// <returns></returns>
        public IList AccountSubContractFeeDtl(IList listDtl, IList listDtlUsage)
        {
            IList listResult = new ArrayList();

            IEnumerable<GWBSDetailCostSubject> listDtlUsageEdit = listDtlUsage.OfType<GWBSDetailCostSubject>();

            ObjectQuery oq = new ObjectQuery();
            foreach (GWBSDetail dtl in listDtl)
            {
                oq.Criterions.Clear();

                //1.根据分包措施明细的过滤条件查询下属实体任务明细的资源耗用

                oq.AddCriterion(Expression.Like("TheGWBSSysCode", dtl.TheGWBSSysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("SubContractFeeFlag", false));
                //oq.AddCriterion(Expression.Eq("State", GWBSDetailState.有效));
                oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));

                Disjunction dis = new Disjunction();//使用Disjunction 如果分包取费明细不包含 责任核算标记和成本核算标记 时则不查出任何核素任务明细数据
                if (dtl.ResponseFlag == 1 && dtl.CostingFlag == 1)
                {
                    dis.Add(Expression.Eq("ResponseFlag", 1));
                    dis.Add(Expression.Eq("CostingFlag", 1));
                }
                else if (dtl.ResponseFlag == 1)
                {
                    dis.Add(Expression.Eq("ResponseFlag", 1));
                }
                else if (dtl.CostingFlag == 1)
                {
                    dis.Add(Expression.Eq("CostingFlag", 1));
                }
                oq.AddCriterion(dis);


                #region 按成本项分类过滤（AND关系，各个分类之间OR关系）

                //如果未设置分类过滤条件则查询所有
                Disjunction dis1 = new Disjunction();
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode1))
                {
                    dis1.Add(Expression.Like("TheCostItem.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode1, MatchMode.Start));
                }
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode2))
                {
                    dis1.Add(Expression.Like("TheCostItem.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode2, MatchMode.Start));
                }
                string term = dis1.ToString();
                if (term != "()")//不加条件时为()
                    oq.AddCriterion(dis1);

                #endregion

                IEnumerable<GWBSDetail> listAccountDtl = dao.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>();


                if (listAccountDtl.Count() > 0
                    && (dtl.TheCostItem.SubjectCateFilter1 != null || dtl.TheCostItem.SubjectCateFilter2 != null || dtl.TheCostItem.SubjectCateFilter3 != null)
                    )//过滤核算科目
                {
                    #region 下属资源耗用按核算科目过滤（AND关系，各个科目之间OR关系）

                    ObjectQuery oqUsage = new ObjectQuery();

                    Disjunction disUsageDtl = new Disjunction();
                    foreach (GWBSDetail item in listAccountDtl)
                    {
                        disUsageDtl.Add(Expression.Eq("TheGWBSDetail.Id", item.Id));
                    }
                    oqUsage.AddCriterion(disUsageDtl);


                    Disjunction disUsage = new Disjunction();
                    if (dtl.TheCostItem.SubjectCateFilter1 != null)
                    {
                        disUsage.Add(Expression.Eq("CostAccountSubjectGUID.Id", dtl.TheCostItem.SubjectCateFilter1.Id));
                    }
                    if (dtl.TheCostItem.SubjectCateFilter2 != null)
                    {
                        disUsage.Add(Expression.Eq("CostAccountSubjectGUID.Id", dtl.TheCostItem.SubjectCateFilter2.Id));
                    }
                    if (dtl.TheCostItem.SubjectCateFilter3 != null)
                    {
                        disUsage.Add(Expression.Eq("CostAccountSubjectGUID.Id", dtl.TheCostItem.SubjectCateFilter3.Id));
                    }
                    oqUsage.AddCriterion(disUsage);

                    #endregion

                    IEnumerable<GWBSDetailCostSubject> listAccountUsage = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oqUsage).OfType<GWBSDetailCostSubject>();

                    if (listAccountUsage.Count() > 0)
                    {
                        #region 计算措施费明细的量价
                        //计算合同预算量价
                        if (true)
                        {
                            decimal sumResponsibleTotalPrice = listAccountUsage.Sum(p => p.ContractTotalPrice) * dtl.TheCostItem.PricingRate;

                            dtl.ContractProjectQuantity = 1;
                            dtl.ContractPrice = sumResponsibleTotalPrice;
                            dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;

                            var queryUsage = from d in listDtlUsageEdit
                                             where d.TheGWBSDetail.Id == dtl.Id
                                             select d;

                            foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                            {
                                dtlUsage.ContractQuotaQuantity = 1;
                                dtlUsage.ContractQuantityPrice = dtl.ContractTotalPrice;
                                if (dtlUsage.ResourceUsageQuota != null)
                                    dtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                                dtlUsage.ContractPricePercent = 1;
                                dtlUsage.ContractBasePrice = dtlUsage.ContractQuantityPrice;

                                dtlUsage.ContractPrice = dtlUsage.ContractQuotaQuantity * dtlUsage.ContractQuantityPrice;
                                dtlUsage.ContractProjectAmount = dtl.ContractProjectQuantity * dtlUsage.ContractQuotaQuantity;
                                dtlUsage.ContractTotalPrice = dtlUsage.ContractProjectAmount * dtlUsage.ContractQuantityPrice;
                            }
                        }

                        //预算类型为责任成本
                        if (dtl.ResponseFlag == 1)
                        {
                            IEnumerable<GWBSDetail> listResponsibleDtl = from d in listAccountDtl
                                                                         where d.ResponseFlag == 1
                                                                         select d;
                            List<string> listResponsibleDtlId = new List<string>();
                            foreach (GWBSDetail d in listResponsibleDtl)
                            {
                                listResponsibleDtlId.Add(d.Id);
                            }

                            IEnumerable<GWBSDetailCostSubject> queryResponsible = from u in listAccountUsage
                                                                                  where listResponsibleDtlId.Contains(u.TheGWBSDetail.Id)
                                                                                  select u;

                            decimal sumResponsibleTotalPrice = queryResponsible.Sum(p => p.ResponsibilitilyTotalPrice) * dtl.TheCostItem.PricingRate;

                            dtl.ResponsibilitilyWorkAmount = 1;
                            dtl.ResponsibilitilyPrice = sumResponsibleTotalPrice;
                            dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                            var queryUsage = from d in listDtlUsageEdit
                                             where d.TheGWBSDetail.Id == dtl.Id
                                             select d;

                            foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                            {
                                dtlUsage.ResponsibleQuotaNum = 1;
                                dtlUsage.ResponsibilitilyPrice = dtl.ResponsibilitilyTotalPrice;
                                if (dtlUsage.ResourceUsageQuota != null)
                                    dtlUsage.ResponsibilitilyPrice = dtlUsage.ResponsibilitilyPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                                dtlUsage.ResponsiblePricePercent = 1;
                                dtlUsage.ResponsibleBasePrice = dtl.ResponsibilitilyPrice;

                                dtlUsage.ResponsibleWorkPrice = dtlUsage.ResponsibleQuotaNum * dtlUsage.ResponsibilitilyPrice;
                                dtlUsage.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleQuotaNum;
                                dtlUsage.ResponsibilitilyTotalPrice = dtlUsage.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleWorkPrice;
                            }
                        }

                        //预算类型为计划成本
                        if (dtl.CostingFlag == 1)
                        {
                            IEnumerable<GWBSDetail> listPlanDtl = from d in listAccountDtl
                                                                  where d.CostingFlag == 1
                                                                  select d;
                            List<string> listPlanDtlId = new List<string>();
                            foreach (GWBSDetail d in listPlanDtl)
                            {
                                listPlanDtlId.Add(d.Id);
                            }

                            IEnumerable<GWBSDetailCostSubject> queryPlan = from u in listAccountUsage
                                                                           where listPlanDtlId.Contains(u.TheGWBSDetail.Id)
                                                                           select u;

                            decimal sumPlanTotalPrice = queryPlan.Sum(p => p.PlanTotalPrice) * dtl.TheCostItem.PricingRate;

                            dtl.PlanWorkAmount = 1;
                            dtl.PlanPrice = sumPlanTotalPrice;
                            dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                            var queryUsage = from d in listDtlUsageEdit
                                             where d.TheGWBSDetail.Id == dtl.Id
                                             select d;

                            foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                            {
                                dtlUsage.PlanQuotaNum = 1;
                                dtlUsage.PlanPrice = dtl.PlanTotalPrice;
                                if (dtlUsage.ResourceUsageQuota != null)
                                    dtlUsage.PlanPrice = dtlUsage.PlanPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                                dtlUsage.PlanPricePercent = 1;
                                dtlUsage.PlanBasePrice = dtlUsage.PlanPrice;

                                dtlUsage.PlanWorkPrice = dtlUsage.PlanQuotaNum * dtlUsage.PlanPrice;
                                dtlUsage.PlanWorkAmount = dtl.PlanWorkAmount * dtlUsage.PlanQuotaNum;
                                dtlUsage.PlanTotalPrice = dtlUsage.PlanWorkAmount * dtlUsage.PlanWorkPrice;
                            }
                        }
                        #endregion
                    }
                }
                else if (listAccountDtl.Count() > 0)
                {
                    #region 计算措施费明细的量价
                    //计算合同预算量价
                    if (true)
                    {
                        decimal sumResponsibleTotalPrice = listAccountDtl.Sum(p => p.ContractTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.ContractProjectQuantity = 1;
                        dtl.ContractPrice = sumResponsibleTotalPrice;
                        dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;

                        var queryUsage = from d in listDtlUsageEdit
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.ContractQuotaQuantity = 1;
                            dtlUsage.ContractQuantityPrice = dtl.ContractTotalPrice;
                            if (dtlUsage.ResourceUsageQuota != null)
                                dtlUsage.ContractQuantityPrice = dtlUsage.ContractQuantityPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                            dtlUsage.ContractPricePercent = 1;
                            dtlUsage.ContractBasePrice = dtlUsage.ContractQuantityPrice;

                            dtlUsage.ContractPrice = dtlUsage.ContractQuotaQuantity * dtlUsage.ContractQuantityPrice;
                            dtlUsage.ContractProjectAmount = dtl.ContractProjectQuantity * dtlUsage.ContractQuotaQuantity;
                            dtlUsage.ContractTotalPrice = dtlUsage.ContractProjectAmount * dtlUsage.ContractPrice;
                        }
                    }

                    //预算类型为责任成本
                    if (dtl.ResponseFlag == 1)
                    {
                        IEnumerable<GWBSDetail> listResponsibleDtl = from d in listAccountDtl
                                                                     where d.ResponseFlag == 1
                                                                     select d;

                        decimal sumResponsibleTotalPrice = listResponsibleDtl.Sum(p => p.ResponsibilitilyTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.ResponsibilitilyWorkAmount = 1;
                        dtl.ResponsibilitilyPrice = sumResponsibleTotalPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        var queryUsage = from d in listDtlUsageEdit
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.ResponsibleQuotaNum = 1;
                            dtlUsage.ResponsibilitilyPrice = dtl.ResponsibilitilyTotalPrice;
                            if (dtlUsage.ResourceUsageQuota != null)
                                dtlUsage.ResponsibilitilyPrice = dtlUsage.ResponsibilitilyPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                            dtlUsage.ResponsiblePricePercent = 1;
                            dtlUsage.ResponsibleBasePrice = dtl.ResponsibilitilyPrice;

                            dtlUsage.ResponsibleWorkPrice = dtlUsage.ResponsibleQuotaNum * dtlUsage.ResponsibilitilyPrice;
                            dtlUsage.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleQuotaNum;
                            dtlUsage.ResponsibilitilyTotalPrice = dtlUsage.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleWorkPrice;
                        }
                    }

                    //预算类型为计划成本
                    if (dtl.CostingFlag == 1)
                    {
                        IEnumerable<GWBSDetail> listPlanDtl = from d in listAccountDtl
                                                              where d.CostingFlag == 1
                                                              select d;

                        decimal sumPlanTotalPrice = listPlanDtl.Sum(p => p.PlanTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.PlanWorkAmount = 1;
                        dtl.PlanPrice = sumPlanTotalPrice;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        var queryUsage = from d in listDtlUsageEdit
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.PlanQuotaNum = 1;
                            dtlUsage.PlanPrice = dtl.PlanTotalPrice;
                            if (dtlUsage.ResourceUsageQuota != null)
                                dtlUsage.PlanPrice = dtlUsage.PlanPrice * dtlUsage.ResourceUsageQuota.AssessmentRate;

                            dtlUsage.PlanPricePercent = 1;
                            dtlUsage.PlanBasePrice = dtlUsage.PlanPrice;

                            dtlUsage.PlanWorkPrice = dtlUsage.PlanQuotaNum * dtlUsage.PlanPrice;
                            dtlUsage.PlanWorkAmount = dtl.PlanWorkAmount * dtlUsage.PlanQuotaNum;
                            dtlUsage.PlanTotalPrice = dtlUsage.PlanWorkAmount * dtlUsage.PlanWorkPrice;
                        }
                    }
                    #endregion
                }
            }

            listResult.Add(listDtl);
            listResult.Add(listDtlUsage);

            return listResult;
        }

        /// <summary>
        /// 计算分包取费任务明细的预算成本(修改算法前)
        /// </summary>
        /// <param name="listDtl">分包取费明细集</param>
        /// <returns></returns>
        private IList AccountSubContractFeeDtlBak(IList listDtl, IList listDtlUsage)
        {
            IList listResult = new ArrayList();

            IEnumerable<GWBSDetailCostSubject> listTempUsage = listDtlUsage.OfType<GWBSDetailCostSubject>();

            //A.查询下属实体任务明细：使用以下条件查询{工程任务明细}：
            //【所属GWBS结构层次码】包含<取费{工程任务明细}>_【所属GWBS结构层次码】（包括等于）
            //AND 【责任核算标志】=1（当<预算类型>=“责任成本”时）、或者【成本核算标志】=1（当<预算类型>=“责任成本”时）
            //AND 【分包取费标志】=0
            //AND 【状态】=有效
            //AND 【成本项】关联{成本项}_【所属成本项分类】属于<取费{工程任务明细}>_【成本项】关联{成本项}_【基数成本项分类过滤】、或者【基数成本项分类过滤二】（包括等于，特化查询）
            //AND 【下属资源耗用】_【成本核算科目】属于<取费{工程任务明细}>_【成本项】关联{成本项}_【基数成本科目分类过滤1】、或者【基数成本科目分类过滤2】、或者【基数成本科目分类过滤3】（包括等于，特化查询）
            //设置查得结果为<实体{工程任务明细}集>。

            ObjectQuery oq = new ObjectQuery();
            foreach (GWBSDetail dtl in listDtl)
            {
                oq.Criterions.Clear();

                oq.AddCriterion(Expression.Like("TheGWBSSysCode", dtl.TheGWBSSysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("SubContractFeeFlag", false));
                oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));

                Disjunction dis = new Disjunction();//使用Disjunction 如果分包取费明细不包含 责任核算标记和成本核算标记 时则不查出任何核素任务明细数据
                if (dtl.ResponseFlag == 1 && dtl.CostingFlag == 1)
                {
                    dis.Add(Expression.Eq("ResponseFlag", 1));
                    dis.Add(Expression.Eq("CostingFlag", 1));
                }
                else if (dtl.ResponseFlag == 1)
                {
                    dis.Add(Expression.Eq("ResponseFlag", 1));
                }
                else if (dtl.CostingFlag == 1)
                {
                    dis.Add(Expression.Eq("CostingFlag", 1));
                }
                oq.AddCriterion(dis);


                #region 按成本项分类过滤（AND关系，各个分类之间OR关系）

                //如果未设置分类过滤条件则查询所有
                Disjunction dis1 = new Disjunction();
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode1))
                {
                    dis1.Add(Expression.Like("TheCostItem.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode1, MatchMode.Start));
                }
                if (!string.IsNullOrEmpty(dtl.TheCostItem.CateFilterSysCode2))
                {
                    dis1.Add(Expression.Like("TheCostItem.TheCostItemCateSyscode", dtl.TheCostItem.CateFilterSysCode2, MatchMode.Start));
                }
                string term = dis1.ToString();
                if (term != "()")//不加条件时为()
                    oq.AddCriterion(dis1);

                #endregion

                IEnumerable<GWBSDetail> listAccountDtl = dao.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>();

                #region 下属资源耗用按核算科目过滤（AND关系，各个科目之间OR关系）

                //if (listAccountDtl.Count() > 0)
                //{
                //    string where = "";

                //    if (dtl.TheCostItem.SubjectCateFilter1 != null)
                //    {
                //        where += "CostAccountSubjectGUID='" + dtl.TheCostItem.SubjectCateFilter1.Id + "' or ";
                //    }
                //    if (dtl.TheCostItem.SubjectCateFilter2 != null)
                //    {
                //        where += "CostAccountSubjectGUID='" + dtl.TheCostItem.SubjectCateFilter2.Id + "' or ";
                //    }
                //    if (dtl.TheCostItem.SubjectCateFilter3 != null)
                //    {
                //        where += "CostAccountSubjectGUID='" + dtl.TheCostItem.SubjectCateFilter3.Id + "' or ";
                //    }
                //    if (where != "")
                //    {
                //        where = where.Substring(0, where.Length - 3);

                //        string whereIn = "";
                //        foreach (GWBSDetail item in listAccountDtl)
                //        {
                //            whereIn += "'" + item.Id + "',";
                //        }
                //        whereIn = whereIn.Substring(0, whereIn.Length - 1);

                //        string sql = "select distinct GWBSDetailId from THD_GWBSDetailCostSubject where GWBSDetailId in(" + whereIn + ") and (" + where + ")";

                //        DataSet ds = SearchSQL(sql);
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            List<string> listDtlId = new List<string>();
                //            foreach (DataRow row in ds.Tables[0].Rows)
                //            {
                //                listDtlId.Add(row["GWBSDetailId"].ToString());
                //            }

                //            listAccountDtl = from d in listAccountDtl
                //                             where listDtlId.Contains(d.Id)
                //                             select d;
                //        }
                //    }
                //}
                #endregion

                #region 计算措施费明细的量价

                if (listAccountDtl.Count() > 0)
                {
                    //计算合同预算量价
                    if (true)
                    {
                        decimal sumResponsibleTotalPrice = listAccountDtl.Sum(p => p.ContractTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.ContractProjectQuantity = 1;
                        dtl.ContractPrice = sumResponsibleTotalPrice;
                        dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;

                        var queryUsage = from d in listTempUsage
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.ContractQuotaQuantity = 1;
                            dtlUsage.ContractQuantityPrice = dtl.ContractTotalPrice;
                            dtlUsage.ContractPrice = dtlUsage.ContractQuotaQuantity * dtlUsage.ContractQuantityPrice;
                            dtlUsage.ContractProjectAmount = dtl.ContractProjectQuantity * dtlUsage.ContractQuotaQuantity;
                            dtlUsage.ContractTotalPrice = dtlUsage.ContractProjectAmount * dtlUsage.ContractPrice;
                        }
                    }

                    //预算类型为责任成本
                    if (dtl.ResponseFlag == 1)
                    {
                        decimal sumResponsibleTotalPrice = listAccountDtl.Sum(p => p.ResponsibilitilyTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.ResponsibilitilyWorkAmount = 1;
                        dtl.ResponsibilitilyPrice = sumResponsibleTotalPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        var queryUsage = from d in listTempUsage
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.ResponsibleQuotaNum = 1;
                            dtlUsage.ResponsibilitilyPrice = dtl.ResponsibilitilyTotalPrice;
                            dtlUsage.ResponsibleWorkPrice = dtlUsage.ResponsibleQuotaNum * dtlUsage.ResponsibilitilyPrice;
                            dtlUsage.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleQuotaNum;
                            dtlUsage.ResponsibilitilyTotalPrice = dtlUsage.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleWorkPrice;
                        }
                    }

                    //预算类型为计划成本
                    if (dtl.CostingFlag == 1)
                    {
                        decimal sumPlanTotalPrice = listAccountDtl.Sum(p => p.PlanTotalPrice) * dtl.TheCostItem.PricingRate;

                        dtl.PlanWorkAmount = 1;
                        dtl.PlanPrice = sumPlanTotalPrice;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        var queryUsage = from d in listTempUsage
                                         where d.TheGWBSDetail.Id == dtl.Id
                                         select d;

                        foreach (GWBSDetailCostSubject dtlUsage in queryUsage)
                        {
                            dtlUsage.PlanQuotaNum = 1;
                            dtlUsage.PlanPrice = dtl.PlanTotalPrice;
                            dtlUsage.PlanWorkPrice = dtlUsage.PlanQuotaNum * dtlUsage.PlanPrice;
                            dtlUsage.PlanWorkAmount = dtl.PlanWorkAmount * dtlUsage.PlanQuotaNum;
                            dtlUsage.PlanTotalPrice = dtlUsage.PlanWorkAmount * dtlUsage.PlanWorkPrice;
                        }
                    }
                }
                #endregion
            }

            listResult.Add(listDtl);
            listResult.Add(listDtlUsage);

            return listResult;
        }

        /// <summary>
        /// 根据工程任务保存其工程文档验证
        /// </summary>
        /// <param name="childNode"></param>
        /// <returns></returns>
        [TransManager]
        public bool SaveProjectDocumentVerif(GWBSTree childNode)
        {
            IList listVerify = GetDocumentTemplatesByTaskType(childNode);
            if (listVerify != null && listVerify.Count > 0)
            {
                for (int i = 0; i < listVerify.Count; i++)
                {
                    ProjectDocumentVerify docVerify = listVerify[i] as ProjectDocumentVerify;
                    docVerify.ProjectTask = childNode;
                    docVerify.TaskSyscode = childNode.SysCode;
                }
            }
            return dao.SaveOrUpdate(listVerify);
        }
        /// <summary>
        /// 根据wbs任务类型得到工程文档验证
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public IList GetDocumentTemplatesByTaskType(GWBSTree wbs)
        {
            IList docVerifyList = new List<ProjectDocumentVerify>();
            if (wbs.ProjectTaskTypeGUID == null || wbs.ProjectTaskTypeGUID.Id == null)
                return null;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(NHibernate.Criterion.Expression.Eq("ProTaskType.Id", wbs.ProjectTaskTypeGUID.Id));
            IList listDocStencil = dao.ObjectQuery(typeof(ProTaskTypeDocumentStencil), oq);

            foreach (ProTaskTypeDocumentStencil docStencil in listDocStencil)
            {
                //生成工程文档验证
                ProjectDocumentVerify docVerify = NewProjectDocumentVerify(docStencil, wbs);

                docVerifyList.Add(docVerify);
            }
            return docVerifyList;
        }

        /// <summary>
        /// 新建工程文档验证
        /// </summary>
        /// <param name="docStencil"></param>
        /// <returns></returns>
        public ProjectDocumentVerify NewProjectDocumentVerify(ProTaskTypeDocumentStencil docStencil, GWBSTree wbs)
        {
            ProjectDocumentVerify docVerify = new ProjectDocumentVerify();

            docVerify.ProjectTaskName = wbs.Name;
            docVerify.TaskSyscode = wbs.SysCode;
            if (docStencil.InspectionMark == ProjectTaskTypeCheckFlag.针对项目任务节点)
            {
                docVerify.AssociateLevel = ProjectDocumentAssociateLevel.GWBS;
            }
            if (docStencil.InspectionMark == ProjectTaskTypeCheckFlag.针对检验批)
            {
                docVerify.AssociateLevel = ProjectDocumentAssociateLevel.检验批;
            }
            docVerify.DocuemntID = docStencil.ProDocumentMasterID;
            docVerify.DocumentCode = docStencil.StencilCode;
            docVerify.DocumentName = docStencil.StencilName;
            docVerify.DocumentDesc = docStencil.StencilDescription;
            docVerify.FileSourceURl = null;
            docVerify.DocumentCategoryCode = docStencil.DocumentCateCode;
            docVerify.DocumentCategoryName = docStencil.DocumentCateName;
            docVerify.DocumentWorkflowName = docStencil.ControlWorkflowName;

            docVerify.AlterMode = docStencil.AlarmMode;

            docVerify.SubmitState = ProjectDocumentSubmitState.未提交;
            docVerify.VerifySwitch = ProjectDocumentVerifySwitch.不验证;

            docVerify.ProjectID = wbs.TheProjectGUID;
            docVerify.ProjectName = wbs.TheProjectName;
            docVerify.ProjectCode = wbs.ProjectTaskTypeGUID.Code;

            return docVerify;

        }


        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveGWBSTreeRootNode1(IList list)
        {
            GWBSTree node = list[0] as GWBSTree;
            CurrentProjectInfo projectInfo = dao.Get(typeof(CurrentProjectInfo), node.TheProjectGUID) as CurrentProjectInfo;

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
            IList listTree = dao.ObjectQuery(typeof(CategoryTree), oq);
            if (listTree.Count == 0)
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
                tree = listTree[0] as CategoryTree;

            node.TheTree = tree;

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

            list[0] = node;

            List<ProjectDocumentVerify> listVerify = new List<ProjectDocumentVerify>();
            foreach (GWBSTree wbs in list)
            {
                if (string.IsNullOrEmpty(wbs.Code))
                    wbs.Code = projectInfo.Code.PadLeft(4, '0') + "-" + GetCode(typeof(GWBSTree));//服务端生成
                wbs.TaskStateTime = DateTime.Now;

                List<ProjectDocumentVerify> listVerifyTemp = GetDocumentTemplatesByTaskType(wbs).OfType<ProjectDocumentVerify>().ToList();
                if (listVerifyTemp != null && listVerifyTemp.Count > 0)
                {
                    for (int i = 0; i < listVerifyTemp.Count; i++)
                    {
                        ProjectDocumentVerify docVerify = listVerifyTemp[i] as ProjectDocumentVerify;
                        docVerify.ProjectTask = wbs;
                        docVerify.TaskSyscode = wbs.SysCode;
                    }

                    listVerify.AddRange(listVerifyTemp);
                }
            }

            dao.SaveOrUpdate(list);
            foreach (GWBSTree wbs in list)
            {
                if (wbs.SysCode == null && wbs.Level == 1)
                {
                    wbs.SysCode = wbs.Id + ".";
                }
                else
                {
                    wbs.SysCode = string.Format("{0}{1}.", wbs.ParentNode.SysCode, wbs.Id);
                }

                dao.SaveOrUpdate(wbs);
            }

            dao.SaveOrUpdate(listVerify);

            return list;
        }

        /// <summary> 
        /// 保存一个节点加上其子节点集合
        /// </summary>
        [TransManager]
        public IList SaveGWBSTrees1(IList list)
        {
            if (list == null || list.Count == 0)
                return list;

            GWBSTree targetParentNode = list[0] as GWBSTree;
            dao.Update(targetParentNode);

            CurrentProjectInfo projectInfo = dao.Get(typeof(CurrentProjectInfo), targetParentNode.TheProjectGUID) as CurrentProjectInfo;

            List<GWBSTree> listChild = new List<GWBSTree>();

            IBusinessOperators author = Dao.Get(typeof(BusinessOperators), (list[1] as GWBSTree).OwnerGUID) as IBusinessOperators;

            for (int i = 1; i < list.Count; i++)
            {
                GWBSTree wbs = list[i] as GWBSTree;

                if (string.IsNullOrEmpty(wbs.Code))
                    wbs.Code = projectInfo.Code.PadLeft(4, '0') + "-" + GetCode(typeof(GWBSTree));//服务端生成
                wbs.TaskStateTime = DateTime.Now;
                wbs.Author = author;
                listChild.Add(wbs);
            }

            dao.Save(listChild);

            var query = from c in listChild
                        where c.ParentNode != null && c.ParentNode.Id == targetParentNode.Id
                        select c;

            foreach (GWBSTree g in query)
            {
                g.SysCode = g.ParentNode.SysCode + g.Id + ".";

                updateSyscode(listChild, g);
            }

            dao.Update(listChild);

            return list;
        }

        private void updateSyscode(List<GWBSTree> listChild, GWBSTree parent)
        {
            var query = from g in listChild
                        where g.ParentNode != null && g.ParentNode.Id == parent.Id
                        select g;

            foreach (GWBSTree g in query)
            {
                g.SysCode = g.ParentNode.SysCode + g.Id + ".";

                updateSyscode(listChild, g);
            }
        }

        /// <summary>
        /// 添加GWBS树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree SaveGWBSTree1(GWBSTree childNode)
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

            SaveProjectDocumentVerif(childNode);

            return childNode;
        }

        /// <summary>
        /// 校验发布的任务明细的成本项、主资源、图号不能重复
        /// </summary>
        /// <param name="listDtl"></param>
        /// <returns></returns>
        public string CheckGWBSDetail(IList list)
        {
            string errorMsg = null;
            string gwbsSysCode = (list[0] as GWBSDetail).TheGWBSSysCode;

            if (list.Count > 0)
            {
                #region 在已发布明细中验重

                foreach (GWBSDetail item in list)
                {
                    if (item.CostingFlag == 1)
                    {
                        ObjectQuery oq = new ObjectQuery();

                        oq.AddCriterion(Expression.Eq("TheCostItem.Id", item.TheCostItem.Id));

                        if (!string.IsNullOrEmpty(item.MainResourceTypeId))
                            oq.AddCriterion(Expression.Eq("MainResourceTypeId", item.MainResourceTypeId));
                        else
                            oq.AddCriterion(Expression.IsNull("MainResourceTypeId"));

                        oq.AddCriterion(Expression.Eq("CostingFlag", 1));
                        oq.AddCriterion(Expression.Eq("SubContractFeeFlag", false));
                        oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));

                        if (!string.IsNullOrEmpty(item.DiagramNumber))
                            oq.AddCriterion(Expression.Eq("DiagramNumber", item.DiagramNumber));
                        else
                            oq.AddCriterion(Expression.IsNull("DiagramNumber"));


                        //所有父节点（直接和间接）
                        Disjunction dis = new Disjunction();

                        //特化查询
                        dis.Add(Expression.Like("TheGWBSSysCode", gwbsSysCode, MatchMode.Start));

                        //泛化查询
                        string[] sysCodes = gwbsSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < sysCodes.Length - 1; i++)
                        {
                            string sysCode = "";
                            for (int j = 0; j <= i; j++)
                            {
                                sysCode += sysCodes[j] + ".";
                            }
                            dis.Add(Expression.Eq("TheGWBSSysCode", sysCode));
                        }
                        oq.AddCriterion(dis);

                        IList listTemp = dao.ObjectQuery(typeof(GWBSDetail), oq);

                        if (listTemp.Count > 0)
                        {
                            GWBSDetail detail = listTemp[0] as GWBSDetail;
                            if (detail.TheGWBS.Id == item.TheGWBS.Id)
                            {
                                errorMsg = "当前节点下已存在与任务明细“" + item.Name + "”的成本项、主资源类型、图号相同且“有效”的“成本核算明细”，请检查！";
                            }
                            else
                            {
                                string fullPath = GetCategorTreeFullPath(typeof(GWBSTree), detail.TheGWBS.Name, detail.TheGWBSSysCode);
                                errorMsg = "“" + fullPath + "”节点下已存在成本项、主资源类型、图号与任务明细“" + item.Name + "”相同且“有效”的“成本核算明细”，请检查！";
                            }

                            return errorMsg;
                        }
                    }
                }
                #endregion
            }

            return errorMsg;
        }

        /// <summary>
        /// 计算项目任务的合同合价、责任合价、计划合价
        /// </summary>
        /// <param name="task">项目任务</param>
        /// <returns></returns>
        public GWBSTree AccountTotalPrice(GWBSTree task)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("TheGWBSSysCode", task.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));

            IEnumerable<GWBSDetail> list = dao.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>();

            decimal contractTotalPrice = 0;
            decimal responsibilitilyTotalPrice = 0;
            decimal planTotalPrice = 0;

            if (list != null && list.Count() > 0)
            {
                var query = from d in list
                            where d.CostingFlag == 1
                            select d;

                if (query.Count() == 0)//如果不包含成本核算明细，取下面所有生产明细的计划和责任明细的合价
                {
                    foreach (GWBSDetail dtl in list)
                    {
                        contractTotalPrice += dtl.ContractTotalPrice;
                        responsibilitilyTotalPrice += dtl.ResponsibilitilyTotalPrice;
                        planTotalPrice += dtl.PlanTotalPrice;
                    }
                }
                else//如果包含成本核算明细，则取成本核算明细的计划和责任明细的合价
                {
                    foreach (GWBSDetail dtl in query)
                    {
                        contractTotalPrice += dtl.ContractTotalPrice;
                        responsibilitilyTotalPrice += dtl.ResponsibilitilyTotalPrice;
                        planTotalPrice += dtl.PlanTotalPrice;
                    }
                }
            }

            task.ContractTotalPrice = contractTotalPrice;
            task.ResponsibilityTotalPrice = responsibilitilyTotalPrice;
            task.PlanTotalPrice = planTotalPrice;

            return task;
        }

        /// <summary>
        /// 计算项目任务所有下级任务节点的合同合价、责任合价、计划合价
        /// </summary>
        /// <param name="task">项目任务</param>
        /// <returns></returns>
        public GWBSTree AccountTotalPriceByChilds(GWBSTree task)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("TheGWBSSysCode", task.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Not(Expression.Eq("TheGWBS.Id", task.Id)));
            //oq.AddCriterion(Expression.Eq("CostingFlag", 1));
            oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));

            IEnumerable<GWBSDetail> list = dao.ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>();

            decimal contractTotalPrice = 0;
            decimal responsibilitilyTotalPrice = 0;
            decimal planTotalPrice = 0;

            if (list != null && list.Count() > 0)
            {
                var query = from d in list
                            where d.CostingFlag == 1
                            select d;

                if (query.Count() == 0)//如果不包含成本核算明细，取下面所有生产明细的计划和责任明细的合价
                {
                    foreach (GWBSDetail dtl in list)
                    {
                        contractTotalPrice += dtl.ContractTotalPrice;
                        responsibilitilyTotalPrice += dtl.ResponsibilitilyTotalPrice;
                        planTotalPrice += dtl.PlanTotalPrice;
                    }
                }
                else//如果包含成本核算明细，则取成本核算明细的计划和责任明细的合价
                {
                    foreach (GWBSDetail dtl in query)
                    {
                        contractTotalPrice += dtl.ContractTotalPrice;
                        responsibilitilyTotalPrice += dtl.ResponsibilitilyTotalPrice;
                        planTotalPrice += dtl.PlanTotalPrice;
                    }
                }
            }

            task.ContractTotalPrice = contractTotalPrice;
            task.ResponsibilityTotalPrice = responsibilitilyTotalPrice;
            task.PlanTotalPrice = planTotalPrice;

            return task;
        }

        /// <summary>
        /// 发布任务节点及其子节点
        /// </summary>
        /// <param name="taskId">父任务</param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree PublisthTaskNodeAndChilds(GWBSTree task)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode", task.SysCode, MatchMode.Start));
            Disjunction dis = new Disjunction();
            dis.Add(Expression.Eq("TaskState", DocumentState.Edit));
            dis.Add(Expression.Eq("TaskState", DocumentState.InAudit));
            dis.Add(Expression.Eq("TaskState", DocumentState.Valid));
            oq.AddCriterion(dis);

            IEnumerable<GWBSTree> list = dao.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>();

            foreach (GWBSTree item in list)
            {
                item.TaskState = DocumentState.InExecute;
                item.TaskStateTime = DateTime.Now;
            }

            var query = from g in list
                        where g.Id == task.Id
                        select g;

            return query.ElementAt(0);
        }

        /// <summary>
        /// 作废任务节点及其子节点
        /// </summary>
        /// <param name="taskId">父任务</param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree InvalidTaskNodeAndChilds(GWBSTree task)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode", task.SysCode, MatchMode.Start));
            Disjunction dis = new Disjunction();
            dis.Add(Expression.Eq("TaskState", DocumentState.Edit));
            dis.Add(Expression.Eq("TaskState", DocumentState.InAudit));
            dis.Add(Expression.Eq("TaskState", DocumentState.Valid));
            dis.Add(Expression.Eq("TaskState", DocumentState.InExecute));
            dis.Add(Expression.Eq("TaskState", DocumentState.Freeze));
            dis.Add(Expression.Eq("TaskState", DocumentState.Suspend));
            oq.AddCriterion(dis);

            IEnumerable<GWBSTree> list = dao.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>();

            foreach (GWBSTree item in list)
            {
                item.TaskState = DocumentState.Invalid;
                item.TaskStateTime = DateTime.Now;
            }

            var query = from g in list
                        where g.Id == task.Id
                        select g;

            return query.ElementAt(0);
        }

        public List<GWBSDetail> AccountTaskDtlPrice(List<GWBSDetail> listTargetTaskDtl)
        {
            DataSet ds = new DataSet();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;

            IDbCommand cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;

            foreach (GWBSDetail dtl in listTargetTaskDtl)
            {
                string usageIds = "";
                foreach (GWBSDetailCostSubject usage in dtl.ListCostSubjectDetails)
                {
                    usageIds += "'" + usage.Id + "',";
                }
                if (usageIds != "")
                    usageIds = usageIds.Substring(0, usageIds.Length - 1);

                string sql = "select sum(t1.contractprice) contractprice,sum(t1.responsibleworkprice) responsibleprice,sum(t1.planworkprice) planprice " +
                             "from thd_gwbsdetailcostsubject t1 where t1.gwbsdetailid='" + dtl.Id + "' and t1.id not in(" + usageIds + ") ";

                cmd.CommandText = sql;
                IDataReader dr = cmd.ExecuteReader();

                ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dtl.ContractPrice += ClientUtil.ToDecimal(row["contractprice"]);
                    dtl.ResponsibilitilyPrice += ClientUtil.ToDecimal(row["responsibleprice"]);
                    dtl.PlanPrice += ClientUtil.ToDecimal(row["planprice"]);
                }
                dtl.ListCostSubjectDetails.Clear();
            }

            return listTargetTaskDtl;
        }

        /// <summary>
        /// 保存拆除的任务明细
        /// </summary>
        /// <param name="dtl"></param>
        /// <returns></returns>
        [TransManager]
        public GWBSDetail SaveBackoutGWBSDetail(GWBSDetail dtl, string optTypeCode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("QuotaCode", dtl.TheCostItem.QuotaCode + optTypeCode));
            oq.AddFetchMode("ProjectUnitGUID", FetchMode.Eager);
            oq.AddFetchMode("PriceUnitGUID", FetchMode.Eager);
            IList listCostItem = dao.ObjectQuery(typeof(CostItem), oq);
            oq.Criterions.Clear();

            IList listQuota = null;
            if (listCostItem.Count > 0)
            {
                CostItem optCostItem = listCostItem[0] as CostItem;

                UpdateDtlCostItem(dtl, optCostItem, true);
            }
            else
            {
                oq.AddCriterion(Expression.Eq("TheCostItem.Id", dtl.TheCostItem.Id));
                oq.AddFetchMode("ListResources", FetchMode.Eager);
                listQuota = dao.ObjectQuery(typeof(SubjectCostQuota), oq);


                ObjectQuery oqCostItem = new ObjectQuery();
                oqCostItem.AddCriterion(Expression.Eq("Id", dtl.TheCostItem.Id));
                oqCostItem.AddFetchMode("ProjectUnitGUID", FetchMode.Eager);
                oqCostItem.AddFetchMode("PriceUnitGUID", FetchMode.Eager);
                oqCostItem.AddFetchMode("TheCostItemCategory", FetchMode.Eager);
                IList list = dao.ObjectQuery(typeof(CostItem), oqCostItem);
                CostItem optCostItem = dao.ObjectQuery(typeof(CostItem), oqCostItem)[0] as CostItem;

                CostItem newCostItem = new CostItem();

                #region 生成一个新的成本项
                newCostItem.ApplyLevel = optCostItem.ApplyLevel;
                newCostItem.CateFilter1 = optCostItem.CateFilter1;
                newCostItem.CateFilter2 = optCostItem.CateFilter2;
                newCostItem.CateFilterName1 = optCostItem.CateFilterName1;
                newCostItem.CateFilterName2 = optCostItem.CateFilterName2;
                newCostItem.CateFilterSysCode1 = optCostItem.CateFilterSysCode1;
                newCostItem.CateFilterSysCode2 = optCostItem.CateFilterSysCode2;

                int costItemCode = GetMaxCostItemCodeByCate(optCostItem.TheCostItemCategory.Id);
                newCostItem.Code = optCostItem.TheCostItemCategory.Code + "-" + costItemCode.ToString().PadLeft(5, '0');

                newCostItem.ContentType = optCostItem.ContentType;
                newCostItem.CostDesc = optCostItem.CostDesc;
                newCostItem.CostItemVersion = optCostItem.CostItemVersion;
                newCostItem.ItemState = CostItemState.发布;
                newCostItem.ManagementMode = optCostItem.ManagementMode;
                newCostItem.ManagementModeName = optCostItem.ManagementModeName;
                newCostItem.Name = optCostItem.Name;
                newCostItem.Price = optCostItem.Price;
                newCostItem.PriceNumber = optCostItem.PriceNumber;
                newCostItem.PriceUnitGUID = optCostItem.PriceUnitGUID;
                newCostItem.PriceUnitName = optCostItem.PriceUnitName;
                newCostItem.PricingRate = optCostItem.PricingRate;
                newCostItem.PricingType = optCostItem.PricingType;
                newCostItem.ProjectUnitGUID = optCostItem.ProjectUnitGUID;
                newCostItem.ProjectUnitName = optCostItem.ProjectUnitName;
                newCostItem.QuotaCode = optCostItem.QuotaCode + optTypeCode;
                newCostItem.CostItemType = CostItemType.拆除;
                newCostItem.ResourceTypeGUID = optCostItem.ResourceTypeGUID;
                newCostItem.ResourceTypeName = optCostItem.ResourceTypeName;
                newCostItem.SubContractPrice = optCostItem.SubContractPrice;
                newCostItem.SubjectCateFilter1 = optCostItem.SubjectCateFilter1;
                newCostItem.SubjectCateFilter2 = optCostItem.SubjectCateFilter2;
                newCostItem.SubjectCateFilter3 = optCostItem.SubjectCateFilter3;
                newCostItem.SubjectCateFilterName1 = optCostItem.SubjectCateFilterName1;
                newCostItem.SubjectCateFilterName2 = optCostItem.SubjectCateFilterName2;
                newCostItem.SubjectCateFilterName3 = optCostItem.SubjectCateFilterName3;
                newCostItem.SubjectCateFilterSyscode1 = optCostItem.SubjectCateFilterSyscode1;
                newCostItem.SubjectCateFilterSyscode2 = optCostItem.SubjectCateFilterSyscode2;
                newCostItem.SubjectCateFilterSyscode3 = optCostItem.SubjectCateFilterSyscode3;
                newCostItem.Summary = optCostItem.Summary;
                newCostItem.TheCostItemCategory = optCostItem.TheCostItemCategory;
                newCostItem.TheCostItemCateSyscode = optCostItem.TheCostItemCateSyscode;
                newCostItem.TheProjectGUID = optCostItem.TheProjectGUID;
                newCostItem.TheProjectName = optCostItem.TheProjectName;

                foreach (SubjectCostQuota quota in listQuota)
                {
                    SubjectCostQuota newQuota = new SubjectCostQuota();
                    newCostItem.ListQuotas.Add(newQuota);
                    newQuota.TheCostItem = newCostItem;

                    newQuota.AssessmentRate = quota.AssessmentRate;
                    newQuota.Code = "";
                    newQuota.CostAccountSubjectGUID = quota.CostAccountSubjectGUID;
                    newQuota.CostAccountSubjectName = quota.CostAccountSubjectName;
                    newQuota.MainResourceFlag = quota.MainResourceFlag;
                    newQuota.Name = quota.Name;
                    newQuota.PriceResponsibleOrgGUID = quota.PriceResponsibleOrgGUID;
                    newQuota.PriceResponsibleOrgName = quota.PriceResponsibleOrgName;
                    newQuota.PriceUnitGUID = quota.PriceUnitGUID;
                    newQuota.PriceUnitName = quota.PriceUnitName;
                    newQuota.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
                    newQuota.ProjectAmountUnitName = quota.ProjectAmountUnitName;
                    newQuota.QuantityResponsibleOrgGUID = quota.QuantityResponsibleOrgGUID;
                    newQuota.QuantityResponsibleOrgName = quota.QuantityResponsibleOrgName;
                    newQuota.QuotaMoney = quota.QuotaMoney;
                    newQuota.QuotaPrice = quota.QuotaPrice;
                    newQuota.QuotaProjectAmount = quota.QuotaProjectAmount;
                    newQuota.ResourceTypeGUID = quota.ResourceTypeGUID;
                    newQuota.ResourceTypeName = quota.ResourceTypeName;
                    newQuota.State = quota.State;
                    newQuota.TheProjectGUID = quota.TheProjectGUID;
                    newQuota.TheProjectName = quota.TheProjectName;
                    newQuota.Wastage = quota.Wastage;

                    foreach (ResourceGroup item in quota.ListResources)
                    {
                        ResourceGroup rg = new ResourceGroup();
                        rg.TheCostQuota = newQuota;
                        newQuota.ListResources.Add(rg);

                        rg.Description = item.Description;
                        rg.DiagramNumber = item.DiagramNumber;
                        rg.IsCateResource = item.IsCateResource;
                        rg.ResourceCateId = item.ResourceCateId;
                        rg.ResourceCateSyscode = item.ResourceCateSyscode;
                        rg.ResourceTypeCode = item.ResourceTypeCode;
                        rg.ResourceTypeGUID = item.ResourceTypeGUID;
                        rg.ResourceTypeName = item.ResourceTypeName;
                        rg.ResourceTypeQuality = item.ResourceTypeQuality;
                        rg.ResourceTypeSpec = item.ResourceTypeSpec;
                    }
                }

                #endregion

                dao.Save(newCostItem);

                dtl.TheCostItem = newCostItem;

                UpdateDtlCostItem(dtl, newCostItem, false);
            }



            return dtl;
        }

        private void UpdateDtlCostItem(GWBSDetail oprDtl, CostItem item, bool isLoadDtl)
        {
            IList listQuota = null;
            if (isLoadDtl)
            {
                ObjectQuery oqQuota = new ObjectQuery();
                oqQuota.AddCriterion(Expression.Eq("TheCostItem.Id", item.Id));
                oqQuota.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                oqQuota.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);

                listQuota = dao.ObjectQuery(typeof(SubjectCostQuota), oqQuota);
            }
            else
            {
                listQuota = item.ListQuotas.ToList();
            }

            foreach (SubjectCostQuota quota in listQuota)
            {
                AddResourceUsageInTaskDetail(oprDtl, quota);
            }

            //更新分包措施费率
            if (item.ContentType == CostItemContentType.分包取费)
            {
                oprDtl.SubContractFeeFlag = true;
                oprDtl.SubContractStepRate = item.PricingRate;
            }
            else
            {
                oprDtl.SubContractFeeFlag = false;
                oprDtl.SubContractStepRate = 0;
            }

            oprDtl.TheCostItem = item;
            oprDtl.TheCostItemCateSyscode = item.TheCostItemCateSyscode;

            oprDtl.WorkAmountUnitGUID = item.ProjectUnitGUID;
            oprDtl.WorkAmountUnitName = item.ProjectUnitName;
            oprDtl.PriceUnitGUID = item.PriceUnitGUID;
            oprDtl.PriceUnitName = item.PriceUnitName;
        }

        private void AddResourceUsageInTaskDetail(GWBSDetail oprDtl, SubjectCostQuota quota)
        {
            GWBSDetailCostSubject subject = new GWBSDetailCostSubject();
            subject.Name = quota.Name;
            subject.MainResTypeFlag = quota.MainResourceFlag;

            subject.ProjectAmountUnitGUID = quota.ProjectAmountUnitGUID;
            subject.ProjectAmountUnitName = quota.ProjectAmountUnitName;

            subject.PriceUnitGUID = quota.PriceUnitGUID;
            subject.PriceUnitName = quota.PriceUnitName;

            //合同
            subject.ContractQuotaQuantity = quota.QuotaProjectAmount;

            subject.ContractBasePrice = quota.QuotaPrice;
            subject.ContractPricePercent = 1;
            subject.ContractQuantityPrice = subject.ContractBasePrice * subject.ContractPricePercent;

            subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;
            subject.ContractProjectAmount = oprDtl.ContractProjectQuantity * subject.ContractQuotaQuantity;
            subject.ContractTotalPrice = subject.ContractProjectAmount * subject.ContractQuantityPrice;

            //责任
            subject.ResponsibleQuotaNum = quota.QuotaProjectAmount;

            subject.ResponsibleBasePrice = quota.QuotaPrice;
            subject.ResponsiblePricePercent = 1;
            subject.ResponsibilitilyPrice = subject.ResponsibleBasePrice * subject.ResponsiblePricePercent;

            subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;
            subject.ResponsibilitilyWorkAmount = oprDtl.ResponsibilitilyWorkAmount * subject.ResponsibleQuotaNum;
            subject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyWorkAmount * subject.ResponsibilitilyPrice;//责任包干单价未知

            //计划
            subject.PlanQuotaNum = quota.QuotaProjectAmount;

            subject.PlanBasePrice = quota.QuotaPrice;
            subject.PlanPricePercent = 1;
            subject.PlanPrice = subject.PlanBasePrice * subject.PlanPricePercent;

            subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;
            subject.PlanWorkAmount = oprDtl.PlanWorkAmount * subject.PlanQuotaNum;
            subject.PlanTotalPrice = subject.PlanWorkAmount * subject.PlanPrice;//计划包干单价未知

            subject.AssessmentRate = quota.AssessmentRate;

            subject.ResourceUsageQuota = quota;

            if (quota.ListResources.Count > 0)
            {
                ResourceGroup itemResource = quota.ListResources.ElementAt(0);
                subject.ResourceTypeGUID = itemResource.ResourceTypeGUID;
                subject.ResourceTypeCode = itemResource.ResourceTypeCode;
                subject.ResourceTypeName = itemResource.ResourceTypeName;
                subject.ResourceTypeQuality = itemResource.ResourceTypeQuality;
                subject.ResourceTypeSpec = itemResource.ResourceTypeSpec;
                subject.ResourceCateSyscode = itemResource.ResourceCateSyscode;
                subject.DiagramNumber = itemResource.DiagramNumber;
            }

            subject.CostAccountSubjectGUID = quota.CostAccountSubjectGUID;
            subject.CostAccountSubjectName = quota.CostAccountSubjectName;
            if (quota.CostAccountSubjectGUID != null)
                subject.CostAccountSubjectSyscode = quota.CostAccountSubjectGUID.SysCode;

            subject.ProjectAmountWasta = quota.Wastage;

            subject.State = GWBSDetailCostSubjectState.编制;
            subject.TheProjectGUID = oprDtl.TheProjectGUID;
            subject.TheProjectName = oprDtl.TheProjectName;
            subject.TheGWBSDetail = oprDtl;
            subject.TheGWBSTree = oprDtl.TheGWBS;
            subject.TheGWBSTreeName = oprDtl.TheGWBS.Name;
            subject.TheGWBSTreeSyscode = oprDtl.TheGWBS.SysCode;

            oprDtl.ListCostSubjectDetails.Add(subject);
        }

        private int GetMaxCostItemCodeByCate(string cateId)
        {
            int code = 1;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheCostItemCategory.Id", cateId));
            IList listCostItem = dao.ObjectQuery(typeof(CostItem), oq);

            if (listCostItem.Count > 0)
            {
                //获取父对象下最大明细号
                foreach (CostItem item in listCostItem)
                {
                    if (item != null && !string.IsNullOrEmpty(item.Code) && item.Code.IndexOf("-") > -1)
                    {
                        try
                        {
                            int tempCode = Convert.ToInt32(item.Code.Substring(item.Code.LastIndexOf("-") + 1));
                            if (tempCode >= code)
                                code = tempCode + 1;
                        }
                        catch
                        {

                        }
                    }
                }
            }
            return code;
        }

        /// <summary>
        /// 成本项分类划分区域的改变
        /// </summary>
        /// <param name="wbs"></param>
        /// <param name="cate"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public CostItemsZoning CostItemZoningChange(GWBSTree wbs, CostItemCategory cate, CurrentProjectInfo info)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("GwbsSyscode", wbs.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("ProjectId", info.Id));
            IList list = dao.ObjectQuery(typeof(CostItemsZoning), oq);
            if (list != null && list.Count > 0)
            {
                dao.Delete(list);
            }
            CostItemsZoning z = new CostItemsZoning();
            z.GwbsId = wbs.Id;
            z.GwbsName = wbs.Name;
            z.GwbsSyscode = wbs.SysCode;
            z.ProjectId = info.Id;
            z.CostItemsCategoryId = cate.Id;
            z.CostItemsCateName = cate.Name;
            z.CostItemsCateSyscode = cate.SysCode;
            //IList saveList = new ArrayList();
            //saveList.Add(z);
            dao.Save(z);
            return z;
        }

        /// <summary>
        /// 删除成本项地域
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public void DeleteCostItemZoning(CostItemsZoning z)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("TheCostItem.TheCostItemCateSyscode", z.CostItemsCateSyscode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("TheProjectGUID", z.ProjectId));
            oq.AddCriterion(Expression.Like("TheGWBSSysCode", z.GwbsSyscode, MatchMode.Start));
            IList list = dao.ObjectQuery(typeof(GWBSDetail), oq);
            if (list != null && list.Count > 0)
            {
                dao.Delete(list);
            }
            dao.Delete(z);
        }

        #region 项目或公司、分公司预警及统计服务

        public List<OperationOrg> GetOperationOrg()
        {
            List<OperationOrg> listOperationOrg = new List<OperationOrg>();

            //机构类型 h 总公司；hd 总公司部门；b 分公司；bd 分公司部门；zgxmb 直管项目部；fgsxmb 分公司项目部

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("State", 1));

            Disjunction dis2 = new Disjunction();
            dis2.Add(Expression.Eq("OperationType", "h"));
            dis2.Add(Expression.Eq("OperationType", "b"));
            dis2.Add(Expression.Eq("OperationType", "zgxmb"));
            dis2.Add(Expression.Eq("OperationType", "fgsxmb"));
            oq.AddCriterion(dis2);

            IEnumerable<OperationOrg> listOrg = dao.ObjectQuery(typeof(OperationOrg), oq).OfType<OperationOrg>();

            listOrg = from o in listOrg
                      orderby o.Name ascending
                      orderby o.Level ascending
                      select o;

            listOperationOrg = listOrg.ToList();

            return listOperationOrg;
        }

        public List<List<OperationOrg>> GetOperationOrgByUser(string userCode)
        {
            List<List<OperationOrg>> listOperationOrg = new List<List<OperationOrg>>();

            //机构类型 h 总公司；hd 总公司部门；b 分公司；bd 分公司部门；zgxmb 直管项目部；fgsxmb 分公司项目部

            //1.查询当前用户拥有所在的所有组织层次码
            string sql = "select t1.opjsyscode from resoperationjob t1 where t1.opjid" +
                        " in" +
                        "(" +
                        " select t2.operationjobid from respersononjob t2 where t2.perid in" +
                        "(" +
                        " select t3.perid from resperson t3 where t3.percode='" + userCode + "'" +
                        ")" +
                        ")";
            DataSet ds = SearchSQL(sql);

            List<string> listOrgSyscode = new List<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                listOrgSyscode.Add(row["opjsyscode"].ToString());
            }
            listOrgSyscode = removeRepeat(listOrgSyscode);

            //2.根据第一步查询出来的用户所在组织找到所在的最近的（总公司、分公司、直管项目部、分公司项目部）组织
            List<string> listZhiShuOrgSyscode = new List<string>();
            foreach (string opjsyscode in listOrgSyscode)
            {
                string[] opjIds = opjsyscode.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                string orgIdStr = "";
                foreach (string id in opjIds)
                {
                    orgIdStr += "'" + id + "',";
                }

                orgIdStr = orgIdStr.Substring(0, orgIdStr.Length - 1);

                string sqlOrg = "select * from (" +
                                " select t1.opgsyscode from resoperationorg t1 where t1.opgid" +
                                " in" +
                                " (" +
                                orgIdStr +
                                " ) and t1.opgstate=1 and t1.opgoperationtype in ('h','b','zgxmb','fgsxmb')" +
                                " order by t1.opglevel desc" +
                                " )temp where rownum<2 ";

                DataSet dsZhiShuOrg = SearchSQL(sqlOrg);
                if (dsZhiShuOrg.Tables[0].Rows.Count == 0)
                    continue;

                string zhishuOrgSysCode = dsZhiShuOrg.Tables[0].Rows[0][0].ToString();

                listZhiShuOrgSyscode.Add(zhishuOrgSysCode);

            }

            listZhiShuOrgSyscode = removeRepeat(listZhiShuOrgSyscode);

            foreach (string zhishuOrgSysCode in listZhiShuOrgSyscode)
            {
                //3.根据用户所在的最近的（总公司、分公司、直管项目部、分公司项目部）组织加载下属所有导航显示的组织

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("State", 1));
                oq.AddCriterion(Expression.Like("SysCode", zhishuOrgSysCode, MatchMode.Start));

                Disjunction dis2 = new Disjunction();
                dis2.Add(Expression.Eq("OperationType", "h"));
                dis2.Add(Expression.Eq("OperationType", "b"));
                dis2.Add(Expression.Eq("OperationType", "zgxmb"));
                dis2.Add(Expression.Eq("OperationType", "fgsxmb"));
                oq.AddCriterion(dis2);

                IEnumerable<OperationOrg> listOrg = dao.ObjectQuery(typeof(OperationOrg), oq).OfType<OperationOrg>();

                listOrg = from o in listOrg
                          orderby o.Name ascending
                          orderby o.Level ascending
                          select o;

                listOperationOrg.Add(listOrg.ToList());
            }

            //存在多个根组织时排序
            listOperationOrg = listOperationOrg.OrderBy(o => o.ElementAt(0).Name).ToList();

            return listOperationOrg;
        }

        public List<string> GetOperationOrgSyscodeByUser(string userCode)
        {
            List<string> listZhiShuOrgSyscode = new List<string>();

            //机构类型 h 总公司；hd 总公司部门；b 分公司；bd 分公司部门；zgxmb 直管项目部；fgsxmb 分公司项目部

            //1.查询当前用户拥有所在的所有组织层次码
            string sql = "select t1.opjsyscode from resoperationjob t1 where t1.opjid" +
                        " in" +
                        "(" +
                        " select t2.operationjobid from respersononjob t2 where t2.perid in" +
                        "(" +
                        " select t3.perid from resperson t3 where t3.percode='" + userCode + "'" +
                        ")" +
                        ")";
            DataSet ds = SearchSQL(sql);

            List<string> listOrgSyscode = new List<string>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                listOrgSyscode.Add(row["opjsyscode"].ToString());
            }
            listOrgSyscode = removeRepeat(listOrgSyscode);

            //2.根据第一步查询出来的用户所在组织找到所在的最近的（总公司、分公司、直管项目部、分公司项目部）组织

            foreach (string opjsyscode in listOrgSyscode)
            {
                string[] opjIds = opjsyscode.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                string orgIdStr = "";
                foreach (string id in opjIds)
                {
                    orgIdStr += "'" + id + "',";
                }

                orgIdStr = orgIdStr.Substring(0, orgIdStr.Length - 1);

                string sqlOrg = "select * from (" +
                                " select t1.opgsyscode from resoperationorg t1 where t1.opgid" +
                                " in" +
                                " (" +
                                orgIdStr +
                                " ) and t1.opgstate=1 and t1.opgoperationtype in ('h','b','zgxmb','fgsxmb')" +
                                " order by t1.opglevel desc" +
                                " )temp where rownum<2 ";

                DataSet dsZhiShuOrg = SearchSQL(sqlOrg);
                if (dsZhiShuOrg.Tables[0].Rows.Count == 0)
                    continue;

                string zhishuOrgSysCode = dsZhiShuOrg.Tables[0].Rows[0][0].ToString();

                listZhiShuOrgSyscode.Add(zhishuOrgSysCode);

            }

            listZhiShuOrgSyscode = removeRepeat(listZhiShuOrgSyscode);

            return listZhiShuOrgSyscode;
        }

        private List<string> removeRepeat(List<string> list)
        {

            //去除具有包含关系的组织
            for (int i = list.Count - 1; i > -1; i--)
            {
                string syscode = list[i];

                for (int j = 0; j < i; j++)
                {
                    string s = list[j];
                    if (syscode.IndexOf(s) > -1)
                    {
                        list.RemoveAt(i);
                        break;
                    }
                }
            }

            return list;
        }



        /// <summary>
        /// 获取项目成本预警指标信息（未用）
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="projectSyscode"></param>
        /// <param name="projectType"></param>
        /// <returns></returns>
        public decimal GetProjectCostWarningInfo(string projectId, string projectSyscode, string projectType)
        {

            decimal warningTarget = 0;

            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();

            session.Transaction.Enlist(command);

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "UP_CostWarning";

            OracleParameter paramProjectId = new OracleParameter("projectId", projectId);
            paramProjectId.Direction = ParameterDirection.Input;
            command.Parameters.Add(paramProjectId);

            OracleParameter paramProjectSyscode = new OracleParameter("projectSyscode", projectSyscode);
            paramProjectSyscode.Direction = ParameterDirection.Input;
            command.Parameters.Add(paramProjectSyscode);

            OracleParameter paramProjectType = new OracleParameter("projectType", projectType);
            paramProjectType.Direction = ParameterDirection.Input;
            command.Parameters.Add(paramProjectType);

            OracleParameter paramWarningTarget = new OracleParameter("warningTarget", OracleType.Number);
            paramWarningTarget.Direction = ParameterDirection.Output;
            command.Parameters.Add(paramWarningTarget);

            command.ExecuteNonQuery();

            if (paramWarningTarget.Value.GetType() != typeof(DBNull))
                warningTarget = Convert.ToDecimal(paramWarningTarget.Value);

            return warningTarget;
        }

        /// <summary>
        /// 获取项目安全、质量、成本、工期预警指标信息（1.安全、质量、成本、工期预警颜色，2.在建项目数）
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="projectSyscode"></param>
        /// <param name="projectType"></param>
        /// <returns></returns>
        public IList GetProjectWarningTargetInfo(string projectId, string projectSyscode, string projectType)
        {
            IList listResult = new ArrayList();

            //项目隐患信息：项目ID，隐患级别，隐患数量
            Dictionary<string, Dictionary<string, int>> dicProjectAndDangerLevel = new Dictionary<string, Dictionary<string, int>>();
            //项目叶节点总数
            Dictionary<string, int> dicProjectAndLeafNodeNum = new Dictionary<string, int>();
            //项目总数  1.准备 2.基础 3.预埋 4.结构 5.安装 6.装修 7.联调 8.收尾
            int projectCountNum = 1;
            int readyProjectCount = 0;//准备项目总个数
            int basicProjectCount = 0;//基础项目总个数
            int precastProjectCount = 0;//预埋
            int structProjectCount = 0;//结构项目总个数
            int installProjectCount = 0;//安装
            int fitupProjectCount = 0;//装修项目总个数
            int joinDebuggerProjectCount = 0;//联调项目总个数
            int endingProjectCount = 0;//收尾项目总个数

            //项目施工阶段
            string projectConstractstage = "";

            decimal warningTargetBySafe = 0;
            decimal warningTargetByQuality = 0;
            decimal warningTargetByDuration = 0;
            decimal warningTargetByCostRealProfit = 0;//实际平均利润率：（总合同收入-总实际成本）/总合同收入
            decimal warningTargetByCostResTurnedOver = 0;//平均目标上缴指标=∑（项目合同收入*责任上缴比例）/总合同收入
            decimal warningTargetByCostSumProfit = 0;//利润额 = 合同收入-实际成本
            decimal warningTargetByCostProfitRate = 0;//利润率 = 利润额/合同收入
            decimal warningTargetByCostSumLower = 0;//超成本降低额 = 责任成本-实际成本
            decimal warningTargetByCostRateLower = 0;//超成本降低率 = 超成本降低额/责任成本


            Color warningTargetBySafeColor = Color.Green;
            Color warningTargetByQualityColor = Color.Green;
            Color warningTargetByDurationColor = Color.Green;
            Color warningTargetByCostColor = Color.Green;

            bool projectHasData = false;//选择项目或组织下是否有数据

            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();

            string sqlStr = "";

            //查询项目下的叶节点总数
            sqlStr = "select t1.TheProjectGUID projectid, count(*) leafNodeNum from thd_gwbstree t1 where t1.taskstate!=2 and t1.categoryNodeType=2" +
                        " and t1.TheProjectGUID in" +
                        " (" +
                        "    select tproject.id from resconfig tproject" +
                        "    where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                        " ) group by t1.TheProjectGUID";


            command.CommandText = sqlStr;
            IDataReader dr = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string projectid = row["projectid"].ToString();
                Int32 leafNodeNum = Convert.ToInt32(row["leafNodeNum"]);

                if (dicProjectAndLeafNodeNum.ContainsKey(projectid) == false)
                {
                    dicProjectAndLeafNodeNum.Add(projectid, leafNodeNum);
                }
            }

            projectType = projectType.ToLower();
            if (projectType == "h" || projectType == "b")//总公司或分公司
            {
                //项目总数

                sqlStr = "select tproject.constractstage,count(*) constractstageNum from resconfig tproject" +
                            " inner join resoperationorg tOrg on tproject.ownerorg=tOrg.Opgid " +
                            " where tproject.ownerorgsyscode like '" + projectSyscode + "%' and tOrg.Opgstate=1 group by tproject.constractstage";

                command.CommandText = sqlStr;
                dr = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
                /*  
                    1.准备 
                    2.基础
                    3.预埋
                    4.结构
                    5.安装
                    6.装修
                    7.联调
                    8.收尾
                 */
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string constractstage = ClientUtil.ToString(row[0]);
                    int constractstageNum = ClientUtil.ToInt(row[1]);
                    if (constractstage == "准备")
                    {
                        readyProjectCount += constractstageNum;
                    }
                    else if (constractstage == "基础")
                    {
                        basicProjectCount += constractstageNum;
                    }
                    else if (constractstage == "预埋")
                    {
                        precastProjectCount += constractstageNum;
                    }
                    else if (constractstage == "结构")
                    {
                        structProjectCount += constractstageNum;
                    }
                    else if (constractstage == "安装")
                    {
                        installProjectCount += constractstageNum;
                    }
                    else if (constractstage == "装修")
                    {
                        fitupProjectCount += constractstageNum;
                    }
                    else if (constractstage == "联调")
                    {
                        joinDebuggerProjectCount += constractstageNum;
                    }
                    else if (constractstage == "收尾")
                    {
                        endingProjectCount += constractstageNum;
                    }

                    projectCountNum += constractstageNum;
                }

                if (projectCountNum != 0)
                {
                    #region 安全预警
                    //对项目和隐患级别的分组查询,结果集为：项目，隐患级别，隐患数
                    sqlStr = "select t3.projectid,t2.dangerlevel,count(t2.dangerlevel) dangerlevelCount from thd_professioninspectiondetail t2" +
                              " inner join THD_ProfessionInspectionMaster t3 on t2.parentid=t3.id" +
                              " where t2.parentid in" +
                              " (" +
                              "  select t1.id from THD_ProfessionInspectionMaster t1" +
                              "  where t1.projectid in" +
                              "  (" +
                              "    select tproject.id from resconfig tproject" +
                              "    where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                              "  )" +
                              " )" +
                              " and t2.dangerlevel in ('一般','重要','紧急') group by t3.projectid,t2.dangerlevel";

                    command.CommandText = sqlStr;
                    dr = command.ExecuteReader();
                    ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string theProjectId = row["projectid"].ToString();
                        string dangerlevel = row["dangerlevel"].ToString();
                        Int32 dangerlevelCount = Convert.ToInt32(row["dangerlevelCount"]);

                        if (dicProjectAndDangerLevel.ContainsKey(theProjectId) == false)
                        {
                            Dictionary<string, int> dicDangerLevel = new Dictionary<string, int>();
                            dicDangerLevel.Add(dangerlevel, dangerlevelCount);
                            dicProjectAndDangerLevel.Add(theProjectId, dicDangerLevel);
                        }
                        else
                        {
                            Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[theProjectId];
                            dicDangerLevel.Add(dangerlevel, dangerlevelCount);
                            dicProjectAndDangerLevel[theProjectId] = dicDangerLevel;
                        }
                    }

                    //项目预警指标：1×黄色预警记录数/叶节点任务总数+2×橙色预警记录数/叶节点任务总数+3×红色预警记录数/叶节点任务总数
                    //公司或分公司预警指标：1×黄色预警项目数/项目总个数+2×橙色预警项目数/项目总个数+3×红色预警项目数/项目总个数

                    decimal projectGeneralWarningNum = 0;//一般预警项目数
                    decimal projectImportantWarningNum = 0;//重要预警项目数
                    decimal projectEmergencyWarningNum = 0;//紧急预警项目数
                    foreach (string keyProjectId in dicProjectAndDangerLevel.Keys)
                    {
                        if (dicProjectAndLeafNodeNum.ContainsKey(keyProjectId) == false)
                            continue;

                        decimal projectWaringTarget = 0;

                        Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[keyProjectId];
                        if (dicDangerLevel.ContainsKey("一般"))
                        {
                            projectWaringTarget += Convert.ToDecimal(dicDangerLevel["一般"]) / dicProjectAndLeafNodeNum[keyProjectId];
                        }
                        if (dicDangerLevel.ContainsKey("重要"))
                        {
                            projectWaringTarget += 2 * Convert.ToDecimal(dicDangerLevel["重要"]) / dicProjectAndLeafNodeNum[keyProjectId];
                        }
                        if (dicDangerLevel.ContainsKey("紧急"))
                        {
                            projectWaringTarget += 3 * Convert.ToDecimal(dicDangerLevel["紧急"]) / dicProjectAndLeafNodeNum[keyProjectId];
                        }

                        //统计各种情况的项目预警数
                        //if (0 <= projectWaringTarget && projectWaringTarget < Convert.ToDecimal(0.05))//正常
                        //{

                        //}
                        //else 
                        if (Convert.ToDecimal(0.05) <= projectWaringTarget && projectWaringTarget < Convert.ToDecimal(0.2))//一般
                        {
                            projectGeneralWarningNum += 1;
                        }
                        else if (Convert.ToDecimal(0.2) <= projectWaringTarget && projectWaringTarget < Convert.ToDecimal(0.4))//重要
                        {
                            projectImportantWarningNum += 1;
                        }
                        else if (Convert.ToDecimal(0.4) <= projectWaringTarget)//紧急
                        {
                            projectEmergencyWarningNum += 1;
                        }
                    }

                    warningTargetBySafe = projectGeneralWarningNum / projectCountNum
                                                       + 2 * projectImportantWarningNum / projectCountNum
                                                       + 3 * projectEmergencyWarningNum / projectCountNum;


                    //设置预警颜色
                    if (0 <= warningTargetBySafe && warningTargetBySafe < Convert.ToDecimal(0.05))
                    {
                        warningTargetBySafeColor = System.Drawing.Color.Green;
                    }
                    else if (Convert.ToDecimal(0.05) <= warningTargetBySafe && warningTargetBySafe < Convert.ToDecimal(0.15))
                    {
                        warningTargetBySafeColor = System.Drawing.Color.Yellow;
                    }
                    else if (Convert.ToDecimal(0.15) <= warningTargetBySafe && warningTargetBySafe < Convert.ToDecimal(0.3))
                    {
                        warningTargetBySafeColor = System.Drawing.Color.Orange;
                    }
                    else if (Convert.ToDecimal(0.3) <= warningTargetBySafe)
                    {
                        warningTargetBySafeColor = System.Drawing.Color.Red;
                    }

                    dicProjectAndDangerLevel.Clear();

                    #endregion

                    #region 质量预警

                    sqlStr = "select t1.projectid,t1.inspectionconclusion,count(*) recordNum from THD_InspectionRecord t1 where t1.projectid in" +
                               " (" +
                               "     select tproject.id from resconfig tproject" +
                               "     where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                               " )" +
                               " and t1.inspectionconclusion='不通过'" +
                               " group by t1.projectid,t1.inspectionconclusion";

                    command.CommandText = sqlStr;
                    dr = command.ExecuteReader();
                    ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string theProjectId = row["projectid"].ToString();
                        string inspectionconclusion = row["inspectionconclusion"].ToString();//项目中不通过的结论
                        Int32 recordNum = Convert.ToInt32(row["recordNum"]);

                        if (dicProjectAndDangerLevel.ContainsKey(theProjectId) == false)
                        {
                            Dictionary<string, int> dicDangerLevel = new Dictionary<string, int>();
                            dicDangerLevel.Add(inspectionconclusion, recordNum);
                            dicProjectAndDangerLevel.Add(theProjectId, dicDangerLevel);
                        }
                        else
                        {
                            Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[theProjectId];
                            dicDangerLevel.Add(inspectionconclusion, recordNum);
                            dicProjectAndDangerLevel[theProjectId] = dicDangerLevel;
                        }
                    }

                    //项目预警指标：1×日常检查（质检、工长）不通过记录个数/叶节点任务总数
                    //公司或分公司预警指标：1×黄色预警项目数/项目总个数+2×橙色预警项目数/项目总个数+3×红色预警项目数/项目总个数
                    foreach (string keyProjectId in dicProjectAndDangerLevel.Keys)
                    {
                        if (dicProjectAndLeafNodeNum.ContainsKey(keyProjectId) == false)
                            continue;

                        decimal projectWaringTarget = 0;

                        Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[keyProjectId];
                        if (dicDangerLevel.ContainsKey("不通过"))
                        {
                            projectWaringTarget += Convert.ToDecimal(dicDangerLevel["不通过"]) / dicProjectAndLeafNodeNum[keyProjectId];
                        }

                        //统计各种情况的项目预警数
                        //if (0 <= projectWaringTarget && projectWaringTarget < Convert.ToDecimal(0.05))//正常
                        //{

                        //}
                        //else 
                        if (Convert.ToDecimal(0.05) <= projectWaringTarget && projectWaringTarget < Convert.ToDecimal(0.2))//一般
                        {
                            projectGeneralWarningNum += 1;
                        }
                        else if (Convert.ToDecimal(0.2) <= projectWaringTarget && projectWaringTarget < Convert.ToDecimal(0.4))//重要
                        {
                            projectImportantWarningNum += 1;
                        }
                        else if (Convert.ToDecimal(0.4) <= projectWaringTarget)//紧急
                        {
                            projectEmergencyWarningNum += 1;
                        }
                    }

                    warningTargetByQuality = projectGeneralWarningNum / projectCountNum
                                                       + 2 * projectImportantWarningNum / projectCountNum
                                                       + 3 * projectEmergencyWarningNum / projectCountNum;


                    //设置预警颜色
                    if (0 <= warningTargetByQuality && warningTargetByQuality < Convert.ToDecimal(0.05))
                    {
                        warningTargetByQualityColor = System.Drawing.Color.Green;
                    }
                    else if (Convert.ToDecimal(0.05) <= warningTargetByQuality && warningTargetByQuality < Convert.ToDecimal(0.15))
                    {
                        warningTargetByQualityColor = System.Drawing.Color.Yellow;
                    }
                    else if (Convert.ToDecimal(0.15) <= warningTargetByQuality && warningTargetByQuality < Convert.ToDecimal(0.3))
                    {
                        warningTargetByQualityColor = System.Drawing.Color.Orange;
                    }
                    else if (Convert.ToDecimal(0.3) <= warningTargetByQuality)
                    {
                        warningTargetByQualityColor = System.Drawing.Color.Red;
                    }

                    dicProjectAndDangerLevel.Clear();

                    #endregion

                    #region 工期预警

                    sqlStr = "select ProjectId,Delays,count(Delays) DelaysCount" +
                              " from(" +
                              " select ProjectId," +
                              " (" +
                              "   case " +
                              "   when Delays>=3 and Delays<5 then '正常延误'" +
                              "   when Delays>=5 and Delays<7 then '一般延误'" +
                              "   when Delays>=7 then '严重延误'" +
                              "   else '正常'" +
                              "   end" +
                              "  ) Delays" +
                              "  from" +
                              "  (" +
                              "    select t2.theprojectguid ProjectId," +
                              "    (" +
                              "     case" +
                              "       when t2.realenddate is null or t2.realenddate=to_date('0001-1-1','yyyy-mm-dd') " +
                              "       then (trunc(sysdate)-trunc(t2.taskplanendtime))+1" +
                              "     else " +
                              "       (trunc(t2.realenddate)-trunc(t2.taskplanendtime))+1" +
                              "     end" +
                              "    ) Delays" +
                              "    from thd_gwbstree t2" +
                              "    where t2.theprojectguid in" +
                              "    (" +
                              "        select tproject.id from resconfig tproject" +
                              "        where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                              "    )" +
                              "    and t2.CategoryNodetype=2 " +
                              "    and t2.taskplanendtime is not null " +
                              "    and t2.taskplanendtime!=to_date('0001-1-1','yyyy-mm-dd')" +
                              "  )" +
                              ")group by ProjectId,Delays";

                    command.CommandText = sqlStr;
                    dr = command.ExecuteReader();
                    ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string theProjectId = row["ProjectId"].ToString();
                        string Delays = row["Delays"].ToString();//项目中叶节点的进度情况(正常，正常延误，一般延误，严重延误)
                        Int32 DelaysCount = Convert.ToInt32(row["DelaysCount"]);

                        if (dicProjectAndDangerLevel.ContainsKey(theProjectId) == false)
                        {
                            Dictionary<string, int> dicDangerLevel = new Dictionary<string, int>();
                            dicDangerLevel.Add(Delays, DelaysCount);
                            dicProjectAndDangerLevel.Add(theProjectId, dicDangerLevel);
                        }
                        else
                        {
                            Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[theProjectId];
                            dicDangerLevel.Add(Delays, DelaysCount);
                            dicProjectAndDangerLevel[theProjectId] = dicDangerLevel;
                        }
                    }

                    //项目预警指标：1×黄色预警任务数/叶节点任务总数+2×橙色预警任务数/叶节点任务总数+3×红色预警任务数/叶节点任务总数
                    //公司或分公司预警指标：1×黄色预警项目数/项目总个数+2×橙色预警项目数/项目总个数+3×红色预警项目数/项目总个数
                    foreach (string keyProjectId in dicProjectAndDangerLevel.Keys)
                    {
                        if (dicProjectAndLeafNodeNum.ContainsKey(keyProjectId) == false)
                            continue;

                        decimal projectWaringTarget = 0;

                        Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[keyProjectId];
                        if (dicDangerLevel.ContainsKey("正常延误"))
                        {
                            projectWaringTarget += Convert.ToDecimal(dicDangerLevel["正常延误"]) / dicProjectAndLeafNodeNum[keyProjectId];
                        }
                        if (dicDangerLevel.ContainsKey("一般延误"))
                        {
                            projectWaringTarget += 2 * Convert.ToDecimal(dicDangerLevel["一般延误"]) / dicProjectAndLeafNodeNum[keyProjectId];
                        }
                        if (dicDangerLevel.ContainsKey("严重延误"))
                        {
                            projectWaringTarget += 3 * Convert.ToDecimal(dicDangerLevel["严重延误"]) / dicProjectAndLeafNodeNum[keyProjectId];
                        }

                        //统计各种情况的项目预警数
                        //if (0 <= projectWaringTarget && projectWaringTarget < Convert.ToDecimal(0.05))//正常
                        //{

                        //}
                        //else 
                        if (Convert.ToDecimal(0.05) <= projectWaringTarget && projectWaringTarget < Convert.ToDecimal(0.15))//一般
                        {
                            projectGeneralWarningNum += 1;
                        }
                        else if (Convert.ToDecimal(0.15) <= projectWaringTarget && projectWaringTarget < Convert.ToDecimal(0.3))//重要
                        {
                            projectImportantWarningNum += 1;
                        }
                        else if (Convert.ToDecimal(0.3) <= projectWaringTarget)//紧急
                        {
                            projectEmergencyWarningNum += 1;
                        }
                    }

                    warningTargetByDuration = projectGeneralWarningNum / projectCountNum
                                                       + 2 * projectImportantWarningNum / projectCountNum
                                                       + 3 * projectEmergencyWarningNum / projectCountNum;


                    //设置预警颜色
                    if (0 <= warningTargetByDuration && warningTargetByDuration < Convert.ToDecimal(0.05))
                    {
                        warningTargetByDurationColor = System.Drawing.Color.Green;
                    }
                    else if (Convert.ToDecimal(0.05) <= warningTargetByDuration && warningTargetByDuration < Convert.ToDecimal(0.15))
                    {
                        warningTargetByDurationColor = System.Drawing.Color.Yellow;
                    }
                    else if (Convert.ToDecimal(0.15) <= warningTargetByDuration && warningTargetByDuration < Convert.ToDecimal(0.3))
                    {
                        warningTargetByDurationColor = System.Drawing.Color.Orange;
                    }
                    else if (Convert.ToDecimal(0.3) <= warningTargetByDuration)
                    {
                        warningTargetByDurationColor = System.Drawing.Color.Red;
                    }

                    dicProjectAndDangerLevel.Clear();

                    #endregion
                }

                #region 成本预警

                sqlStr = "select theprojectguid ProjectId" +
                          ",sum(currincometotalprice) currincometotalprice" +
                          ",sum(currresponsitotalprice) currresponsitotalprice" +
                          ",sum(currrealtotalprice) currrealtotalprice" +
                          " from" +
                          " (" +
                          "   select t3.theprojectguid,t1.currincometotalprice,t1.currresponsitotalprice,t1.currrealtotalprice" +
                          "   from thd_costmonthaccountdtl t1" +
                          "   inner join thd_costmonthaccount t3 on t1.parentid=t3.id" +
                          "   where t1.parentid in" +
                          "   (" +
                          "     select t2.id from thd_costmonthaccount t2" +
                          "     where t2.theprojectguid in" +
                          "     (" +
                          "       select tproject.id from resconfig tproject" +
                          "       where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                          "     )" +
                          "   )" +
                          " )" +
                          " group by theprojectguid";

                command.CommandText = sqlStr;
                dr = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

                decimal SumContractTotalPrice = 0;//总合同收入
                decimal SumResponsibleTotalPrice = 0;//总责任成本
                decimal SumRealTotalPrice = 0;//总实际成本

                if (ds.Tables[0].Rows.Count > 0)
                    projectHasData = true;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SumContractTotalPrice += Convert.ToDecimal(row["currincometotalprice"]);
                    SumResponsibleTotalPrice += Convert.ToDecimal(row["currresponsitotalprice"]);
                    SumRealTotalPrice += Convert.ToDecimal(row["currrealtotalprice"]);
                }

                if (SumContractTotalPrice != 0)
                {
                    warningTargetByCostRealProfit = (SumContractTotalPrice - SumRealTotalPrice) / SumContractTotalPrice;
                    warningTargetByCostResTurnedOver = (SumContractTotalPrice - SumResponsibleTotalPrice) / SumContractTotalPrice;

                    warningTargetByCostSumProfit = SumContractTotalPrice - SumRealTotalPrice;
                    warningTargetByCostProfitRate = warningTargetByCostSumProfit / SumContractTotalPrice;
                    warningTargetByCostSumLower = SumResponsibleTotalPrice - SumRealTotalPrice;
                    warningTargetByCostRateLower = warningTargetByCostSumLower / SumResponsibleTotalPrice;
                }


                //项目预警指标：实际利润率A=（合同收入-实际成本）/合同收入，目标上缴指标B=责任上缴比例
                // 公司或分公司预警判断指标：实际平均利润率A=（总合同收入-总实际成本）/总合同收入，平均目标上缴指标B=∑（项目合同收入*责任上缴比例）/总合同收入；
                /* 
                 预警规则：
                 绿色	正常	    A>B 
                 黄色	相对亏损	0<A≤B
                 红色	绝对亏损	A≤0
                 */


                //设置预警颜色
                if (warningTargetByCostRealProfit > warningTargetByCostResTurnedOver)
                {
                    warningTargetByCostColor = System.Drawing.Color.Green;
                }
                else if (warningTargetByCostRealProfit > 0 && warningTargetByCostRealProfit <= warningTargetByCostResTurnedOver)
                {
                    warningTargetByCostColor = System.Drawing.Color.Yellow;
                }
                else if (warningTargetByCostRealProfit <= 0)
                {
                    warningTargetByCostColor = System.Drawing.Color.Red;
                }
                #endregion
            }
            else if (projectType == "zgxmb" || projectType == "fgsxmb")//直管项目部或分公司项目部
            {
                //查询项目的施工阶段
                sqlStr = "select t1.id,t1.constractstage from resconfig t1 where t1.ownerOrg='" + projectId + "'";

                command.CommandText = sqlStr;
                dr = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    projectId = ds.Tables[0].Rows[0][0].ToString();
                    projectConstractstage = ds.Tables[0].Rows[0][1].ToString();
                }

                #region 安全预警
                //对项目和隐患级别的分组查询,结果集为：项目，隐患级别，隐患数
                sqlStr = "select t3.projectid,t2.dangerlevel,count(t2.dangerlevel) dangerlevelCount from thd_professioninspectiondetail t2" +
                          " inner join THD_ProfessionInspectionMaster t3 on t2.parentid=t3.id" +
                          " where t2.parentid in" +
                          " (" +
                          "  select t1.id from THD_ProfessionInspectionMaster t1" +
                          "  where t1.projectid = '" + projectId + "'" +
                          " )" +
                          " and t2.dangerlevel in ('一般','重要','紧急') group by t3.projectid,t2.dangerlevel";

                command.CommandText = sqlStr;
                dr = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string theProjectId = row["projectid"].ToString();
                    string dangerlevel = row["dangerlevel"].ToString();
                    Int32 dangerlevelCount = Convert.ToInt32(row["dangerlevelCount"]);

                    if (dicProjectAndDangerLevel.ContainsKey(theProjectId) == false)
                    {
                        Dictionary<string, int> dicDangerLevel = new Dictionary<string, int>();
                        dicDangerLevel.Add(dangerlevel, dangerlevelCount);
                        dicProjectAndDangerLevel.Add(theProjectId, dicDangerLevel);
                    }
                    else
                    {
                        Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[theProjectId];
                        dicDangerLevel.Add(dangerlevel, dangerlevelCount);
                        dicProjectAndDangerLevel[theProjectId] = dicDangerLevel;
                    }
                }

                //项目预警指标：1×黄色预警记录数/叶节点任务总数+2×橙色预警记录数/叶节点任务总数+3×红色预警记录数/叶节点任务总数
                //公司或分公司预警指标：1×黄色预警项目数/项目总个数+2×橙色预警项目数/项目总个数+3×红色预警项目数/项目总个数

                foreach (string keyProjectId in dicProjectAndDangerLevel.Keys)
                {
                    if (dicProjectAndLeafNodeNum.ContainsKey(keyProjectId) == false)
                        continue;
                    Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[keyProjectId];
                    if (dicDangerLevel.ContainsKey("一般"))
                    {
                        warningTargetBySafe += Convert.ToDecimal(dicDangerLevel["一般"]) / dicProjectAndLeafNodeNum[keyProjectId];
                    }
                    if (dicDangerLevel.ContainsKey("重要"))
                    {
                        warningTargetBySafe += 2 * Convert.ToDecimal(dicDangerLevel["重要"]) / dicProjectAndLeafNodeNum[keyProjectId];
                    }
                    if (dicDangerLevel.ContainsKey("紧急"))
                    {
                        warningTargetBySafe += 3 * Convert.ToDecimal(dicDangerLevel["紧急"]) / dicProjectAndLeafNodeNum[keyProjectId];
                    }
                }

                //设置预警颜色
                if (0 <= warningTargetBySafe && warningTargetBySafe < Convert.ToDecimal(0.05))
                {
                    warningTargetBySafeColor = System.Drawing.Color.Green;
                }
                else if (Convert.ToDecimal(0.05) <= warningTargetBySafe && warningTargetBySafe < Convert.ToDecimal(0.2))
                {
                    warningTargetBySafeColor = System.Drawing.Color.Yellow;
                }
                else if (Convert.ToDecimal(0.2) <= warningTargetBySafe && warningTargetBySafe < Convert.ToDecimal(0.4))
                {
                    warningTargetBySafeColor = System.Drawing.Color.Orange;
                }
                else if (Convert.ToDecimal(0.4) <= warningTargetBySafe)
                {
                    warningTargetBySafeColor = System.Drawing.Color.Red;
                }

                dicProjectAndDangerLevel.Clear();

                #endregion

                #region 质量预警

                sqlStr = "select t1.projectid,t1.inspectionconclusion,count(*) recordNum from THD_InspectionRecord t1 where t1.projectid ='" + projectId + "'" +
                           " and t1.inspectionconclusion='不通过'" +
                           " group by t1.projectid,t1.inspectionconclusion";

                command.CommandText = sqlStr;
                dr = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string theProjectId = row["projectid"].ToString();
                    string inspectionconclusion = row["inspectionconclusion"].ToString();//项目中不通过的结论
                    Int32 recordNum = Convert.ToInt32(row["recordNum"]);

                    if (dicProjectAndDangerLevel.ContainsKey(theProjectId) == false)
                    {
                        Dictionary<string, int> dicDangerLevel = new Dictionary<string, int>();
                        dicDangerLevel.Add(inspectionconclusion, recordNum);
                        dicProjectAndDangerLevel.Add(theProjectId, dicDangerLevel);
                    }
                    else
                    {
                        Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[theProjectId];
                        dicDangerLevel.Add(inspectionconclusion, recordNum);
                        dicProjectAndDangerLevel[theProjectId] = dicDangerLevel;
                    }
                }

                //项目预警指标：1×日常检查（质检、工长）不通过记录个数/叶节点任务总数
                //公司或分公司预警指标：1×黄色预警项目数/项目总个数+2×橙色预警项目数/项目总个数+3×红色预警项目数/项目总个数
                foreach (string keyProjectId in dicProjectAndDangerLevel.Keys)
                {
                    if (dicProjectAndLeafNodeNum.ContainsKey(keyProjectId) == false)
                        continue;
                    Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[keyProjectId];
                    if (dicDangerLevel.ContainsKey("不通过"))
                    {
                        warningTargetByQuality += Convert.ToDecimal(dicDangerLevel["不通过"]) / dicProjectAndLeafNodeNum[keyProjectId];
                    }
                }

                //设置预警颜色
                if (0 <= warningTargetByQuality && warningTargetByQuality < Convert.ToDecimal(0.05))
                {
                    warningTargetByQualityColor = System.Drawing.Color.Green;
                }
                else if (Convert.ToDecimal(0.05) <= warningTargetByQuality && warningTargetByQuality < Convert.ToDecimal(0.2))
                {
                    warningTargetByQualityColor = System.Drawing.Color.Yellow;
                }
                else if (Convert.ToDecimal(0.2) <= warningTargetByQuality && warningTargetByQuality < Convert.ToDecimal(0.4))
                {
                    warningTargetByQualityColor = System.Drawing.Color.Orange;
                }
                else if (Convert.ToDecimal(0.4) <= warningTargetByQuality)
                {
                    warningTargetByQualityColor = System.Drawing.Color.Red;
                }

                dicProjectAndDangerLevel.Clear();

                #endregion

                #region 工期预警

                sqlStr = "select ProjectId,Delays,count(Delays) DelaysCount" +
                          " from(" +
                          " select ProjectId," +
                          " (" +
                          "   case " +
                          "   when Delays>=0 and Delays<3 then '正常'" +
                          "   when Delays>=3 and Delays<5 then '正常延误'" +
                          "   when Delays>=5 and Delays<7 then '一般延误'" +
                          "   else '严重延误'" +
                          "   end" +
                          "  ) Delays" +
                          "  from" +
                          "  (" +
                          "    select t2.theprojectguid ProjectId," +
                          "    (" +
                          "     case" +
                          "       when t2.realenddate is null or t2.realenddate=to_date('0001-1-1','yyyy-mm-dd') " +
                          "       then (trunc(sysdate)-trunc(t2.taskplanendtime))+1" +
                          "     else " +
                          "       (trunc(t2.realenddate)-trunc(t2.taskplanendtime))+1" +
                          "     end" +
                          "    ) Delays" +
                          "    from thd_gwbstree t2" +
                          "    where t2.theprojectguid ='" + projectId + "'" +
                          "    and t2.CategoryNodetype=2 " +
                          "    and t2.taskplanendtime is not null " +
                          "    and t2.taskplanendtime!=to_date('0001-1-1','yyyy-mm-dd')" +
                          "  )" +
                          ")group by ProjectId,Delays";

                command.CommandText = sqlStr;
                dr = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string theProjectId = row["ProjectId"].ToString();
                    string Delays = row["Delays"].ToString();//项目中叶节点的进度情况(正常，正常延误，一般延误，严重延误)
                    Int32 DelaysCount = Convert.ToInt32(row["DelaysCount"]);

                    if (dicProjectAndDangerLevel.ContainsKey(theProjectId) == false)
                    {
                        Dictionary<string, int> dicDangerLevel = new Dictionary<string, int>();
                        dicDangerLevel.Add(Delays, DelaysCount);
                        dicProjectAndDangerLevel.Add(theProjectId, dicDangerLevel);
                    }
                    else
                    {
                        Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[theProjectId];
                        dicDangerLevel.Add(Delays, DelaysCount);
                        dicProjectAndDangerLevel[theProjectId] = dicDangerLevel;
                    }
                }

                //项目预警指标：1×黄色预警任务数/叶节点任务总数+2×橙色预警任务数/叶节点任务总数+3×红色预警任务数/叶节点任务总数
                //公司或分公司预警指标：1×黄色预警项目数/项目总个数+2×橙色预警项目数/项目总个数+3×红色预警项目数/项目总个数
                foreach (string keyProjectId in dicProjectAndDangerLevel.Keys)
                {
                    if (dicProjectAndLeafNodeNum.ContainsKey(keyProjectId) == false)
                        continue;
                    Dictionary<string, int> dicDangerLevel = dicProjectAndDangerLevel[keyProjectId];
                    if (dicDangerLevel.ContainsKey("正常延误"))
                    {
                        warningTargetByDuration += Convert.ToDecimal(dicDangerLevel["正常延误"]) / dicProjectAndLeafNodeNum[keyProjectId];
                    }
                    if (dicDangerLevel.ContainsKey("一般延误"))
                    {
                        warningTargetByDuration += 2 * Convert.ToDecimal(dicDangerLevel["一般延误"]) / dicProjectAndLeafNodeNum[keyProjectId];
                    }
                    if (dicDangerLevel.ContainsKey("严重延误"))
                    {
                        warningTargetByDuration += 3 * Convert.ToDecimal(dicDangerLevel["严重延误"]) / dicProjectAndLeafNodeNum[keyProjectId];
                    }
                }


                //设置预警颜色
                if (0 <= warningTargetByDuration && warningTargetByDuration < Convert.ToDecimal(0.05))
                {
                    warningTargetByDurationColor = System.Drawing.Color.Green;
                }
                else if (Convert.ToDecimal(0.05) <= warningTargetByDuration && warningTargetByDuration < Convert.ToDecimal(0.15))
                {
                    warningTargetByDurationColor = System.Drawing.Color.Yellow;
                }
                else if (Convert.ToDecimal(0.15) <= warningTargetByDuration && warningTargetByDuration < Convert.ToDecimal(0.3))
                {
                    warningTargetByDurationColor = System.Drawing.Color.Orange;
                }
                else if (Convert.ToDecimal(0.3) <= warningTargetByDuration)
                {
                    warningTargetByDurationColor = System.Drawing.Color.Red;
                }

                dicProjectAndDangerLevel.Clear();

                #endregion

                #region 成本预警

                sqlStr = "select theprojectguid ProjectId" +
                          ",sum(currincometotalprice) currincometotalprice" +
                          ",sum(currresponsitotalprice) currresponsitotalprice" +
                          ",sum(currrealtotalprice) currrealtotalprice" +
                          " from" +
                          " (" +
                          "   select t3.theprojectguid,t1.currincometotalprice,t1.currresponsitotalprice,t1.currrealtotalprice" +
                          "   from thd_costmonthaccountdtl t1" +
                          "   inner join thd_costmonthaccount t3 on t1.parentid=t3.id" +
                          "   where t1.parentid in" +
                          "   (" +
                          "     select t2.id from thd_costmonthaccount t2" +
                          "     where t2.theprojectguid ='" + projectId + "'" +
                          "   )" +
                          " )" +
                          " group by theprojectguid";

                command.CommandText = sqlStr;
                dr = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

                decimal SumContractTotalPrice = 0;//总合同收入
                decimal SumResponsibleTotalPrice = 0;//总责任成本
                decimal SumRealTotalPrice = 0;//总实际成本

                if (ds.Tables[0].Rows.Count > 0)
                    projectHasData = true;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SumContractTotalPrice += Convert.ToDecimal(row["currincometotalprice"]);
                    SumResponsibleTotalPrice += Convert.ToDecimal(row["currresponsitotalprice"]);
                    SumRealTotalPrice += Convert.ToDecimal(row["currrealtotalprice"]);
                }

                if (SumContractTotalPrice != 0)
                {
                    warningTargetByCostRealProfit = (SumContractTotalPrice - SumRealTotalPrice) / SumContractTotalPrice;
                    warningTargetByCostResTurnedOver = (SumContractTotalPrice - SumResponsibleTotalPrice) / SumContractTotalPrice;

                    warningTargetByCostSumProfit = SumContractTotalPrice - SumRealTotalPrice;
                    warningTargetByCostProfitRate = warningTargetByCostSumProfit / SumContractTotalPrice;
                    warningTargetByCostSumLower = SumResponsibleTotalPrice - SumRealTotalPrice;
                    warningTargetByCostRateLower = warningTargetByCostSumLower / SumResponsibleTotalPrice;
                }


                //项目预警指标：实际利润率A=（合同收入-实际成本）/合同收入，目标上缴指标B=责任上缴比例
                // 公司或分公司预警判断指标：实际平均利润率A=（总合同收入-总实际成本）/总合同收入，平均目标上缴指标B=∑（项目合同收入*责任上缴比例）/总合同收入；
                /* 
                 预警规则：
                 绿色	正常	    A>B 
                 黄色	相对亏损	0<A≤B
                 红色	绝对亏损	A≤0
                 */


                //设置预警颜色
                if (warningTargetByCostRealProfit > warningTargetByCostResTurnedOver)
                {
                    warningTargetByCostColor = System.Drawing.Color.Green;
                }
                else if (warningTargetByCostRealProfit > 0 && warningTargetByCostRealProfit <= warningTargetByCostResTurnedOver)
                {
                    warningTargetByCostColor = System.Drawing.Color.Yellow;
                }
                else if (warningTargetByCostRealProfit <= 0)
                {
                    warningTargetByCostColor = System.Drawing.Color.Red;
                }
                #endregion
            }

            if (projectHasData == false)//没数据时显示绿色
                warningTargetByCostColor = Color.Green;

            //预警颜色
            listResult.Add(warningTargetBySafeColor);//0
            listResult.Add(warningTargetByQualityColor);//1
            listResult.Add(warningTargetByDurationColor);//2
            listResult.Add(warningTargetByCostColor);//3

            //预警指标值
            listResult.Add(warningTargetBySafe);//4
            listResult.Add(warningTargetByQuality);//5
            listResult.Add(warningTargetByDuration);//6
            listResult.Add(warningTargetByCostRealProfit);//7
            listResult.Add(warningTargetByCostResTurnedOver);//8

            //其它信息
            listResult.Add(projectCountNum);//9
            listResult.Add(projectConstractstage);//10
            listResult.Add(projectHasData);//11

            listResult.Add(readyProjectCount);//12
            listResult.Add(basicProjectCount);//13
            listResult.Add(structProjectCount);//14
            listResult.Add(fitupProjectCount);//15
            listResult.Add(endingProjectCount);//16

            listResult.Add(warningTargetByCostSumProfit);//17
            listResult.Add(warningTargetByCostProfitRate);//18
            listResult.Add(warningTargetByCostSumLower);//19
            listResult.Add(warningTargetByCostRateLower);//20

            //2014.6.18
            listResult.Add(precastProjectCount);//21
            listResult.Add(installProjectCount);//22
            listResult.Add(joinDebuggerProjectCount);//23

            return listResult;
        }

        /// <summary>
        /// 获取起始日期开始的项目或公司/分公司各月合同收入、责任成本、实际成本信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="projectSyscode"></param>
        /// <returns></returns>
        public DataTable[] GetProjectCostData(DateTime startDate, string projectSyscode)
        {
            List<DataTable> listResult = new List<DataTable>();

            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();

            string startDateStr = startDate.Year + "-" + startDate.Month;

            //从月度成本核算表里查询从指定月份开始指定项目或公司/分公司下所有项目的合同收入、责任成本、实际成本累计值
            string sqlStr = "select accountDate" +
                            ",sum(sumincometotalprice) sumincometotalprice" +
                            ",sum(sumresponsitotalprice) sumresponsitotalprice" +
                            ",sum(sumrealtotalprice) sumrealtotalprice" +
                            " from" +
                            " (" +
                            "  select to_char(t3.kjn)||'-'||to_char(t3.kjy) accountDate" +
                            "  ,t1.sumincometotalprice,t1.sumresponsitotalprice,t1.sumrealtotalprice" +
                            "  from thd_costmonthaccountdtl t1" +
                            "  inner join thd_costmonthaccount t3 on t1.parentid=t3.id" +
                            "  where t1.parentid in" +
                            "  (" +
                            "    select t2.id from thd_costmonthaccount t2" +
                            "    where t2.theprojectguid in" +
                            "    (" +
                            "       select tproject.id from resconfig tproject" +
                            "       where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                            "    )" +
                            "    and to_date(to_char(t2.kjn)||'-'||to_char(t2.kjy),'yyyy-mm')>=to_date('" + startDateStr + "','yyyy-mm')" +
                            "  )" +
                            " )" +
                            " group by accountDate";

            command.CommandText = sqlStr;
            IDataReader dr = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

            sqlStr = "select accountDate,sum(collectionsummoney) collectionsummoney" +
                    " from" +
                    " (" +
                    "    select to_char(t1.createDate,'yyyy-mm') accountDate,t1.collectionsummoney" +
                    "    from thd_ownerquantitymaster t1" +
                    "    where t1.projectid in" +
                    "    (" +
                    "        select tproject.id from resconfig tproject" +
                    "        where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                    "    )" +
                    "    and to_date(to_char(t1.createDate,'yyyy-mm'),'yyyy-mm')>=to_date('" + startDateStr + "','yyyy-mm')" +
                    " )" +
                    " group by accountDate";

            command.CommandText = sqlStr;
            dr = command.ExecuteReader();
            DataSet ds2 = DataAccessUtil.ConvertDataReadertoDataSet(dr);

            listResult.Add(ds.Tables[0]);
            listResult.Add(ds2.Tables[0]);

            return listResult.ToArray();
        }

        /// <summary>
        /// 获取起始日期开始的项目或公司/分公司各月合同收入、责任成本、实际成本信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="projectSyscode"></param>
        /// <returns></returns>
        public DataTable[] GetProjectCostDataByYear(DateTime startDate, string projectSyscode)
        {
            List<DataTable> listResult = new List<DataTable>();

            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();

            string startDateStr = startDate.Year + "-" + startDate.Month;

            //从月度成本核算表里查询从指定月份开始指定项目或公司/分公司下所有项目的合同收入、责任成本、实际成本累计值
            string sqlStr = "select accountDate" +
                            ",sum(currincometotalprice) currincometotalprice" +
                            ",sum(currresponsitotalprice) currresponsitotalprice" +
                            ",sum(currrealtotalprice) currrealtotalprice" +
                            " from" +
                            " (" +
                            "  select to_char(t3.kjn)||'-'||to_char(t3.kjy) accountDate" +
                            "  ,t1.currincometotalprice,t1.currresponsitotalprice,t1.currrealtotalprice" +
                            "  from thd_costmonthaccountdtl t1" +
                            "  inner join thd_costmonthaccount t3 on t1.parentid=t3.id" +
                            "  where t1.parentid in" +
                            "  (" +
                            "    select t2.id from thd_costmonthaccount t2" +
                            "    where t2.theprojectguid in" +
                            "    (" +
                            "       select tproject.id from resconfig tproject" +
                            "       where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                            "    )" +
                            "    and to_date(to_char(t2.kjn)||'-'||to_char(t2.kjy),'yyyy-mm')>=to_date('" + startDateStr + "','yyyy-mm')" +
                            "  )" +
                            " )" +
                            " group by accountDate";

            command.CommandText = sqlStr;
            IDataReader dr = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

            //汇总年度每个月的累计值
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DateTime accountDate = ClientUtil.ToDateTime(row["accountDate"]);
                foreach (DataRow row2 in ds.Tables[0].Rows)
                {
                    DateTime accountDate2 = ClientUtil.ToDateTime(row2["accountDate"]);
                    if (accountDate2 < accountDate && accountDate.Year == accountDate2.Year)
                    {
                        row["currincometotalprice"] = ClientUtil.ToDecimal(row["currincometotalprice"]) + ClientUtil.ToDecimal(row2["currincometotalprice"]);
                        row["currresponsitotalprice"] = ClientUtil.ToDecimal(row["currresponsitotalprice"]) + ClientUtil.ToDecimal(row2["currresponsitotalprice"]);
                        row["currrealtotalprice"] = ClientUtil.ToDecimal(row["currrealtotalprice"]) + ClientUtil.ToDecimal(row2["currrealtotalprice"]);
                    }
                }
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DateTime accountDate = ClientUtil.ToDateTime(row["accountDate"]);
                decimal currincometotalprice = ClientUtil.ToDecimal(row["currincometotalprice"]);
                decimal currresponsitotalprice = ClientUtil.ToDecimal(row["currresponsitotalprice"]);
                decimal currrealtotalprice = ClientUtil.ToDecimal(row["currrealtotalprice"]);
            }

            sqlStr = "select accountDate,sum(collectionsummoney) collectionsummoney" +
                    " from" +
                    " (" +
                    "    select to_char(t1.createDate,'yyyy-mm') accountDate,t1.collectionsummoney" +
                    "    from thd_ownerquantitymaster t1" +
                    "    where t1.projectid in" +
                    "    (" +
                    "        select tproject.id from resconfig tproject" +
                    "        where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                    "    )" +
                    "    and to_date(to_char(t1.createDate,'yyyy-mm'),'yyyy-mm')>=to_date('" + startDateStr + "','yyyy-mm')" +
                    " )" +
                    " group by accountDate";

            command.CommandText = sqlStr;
            dr = command.ExecuteReader();
            DataSet ds2 = DataAccessUtil.ConvertDataReadertoDataSet(dr);

            listResult.Add(ds.Tables[0]);
            listResult.Add(ds2.Tables[0]);

            return listResult.ToArray();
        }

        /// <summary>
        /// 获取起始日期开始的项目或公司/分公司各月利润信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="projectSyscode"></param>
        /// <returns></returns>
        public DataSet GetProjectProfitData(DateTime startDate, string projectSyscode)
        {

            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();

            string startDateStr = startDate.Year + "-" + startDate.Month;

            string sqlStr = "select accountDate,sum(profitTotalPrice) profitTotalPrice" +
                            " from" +
                            " (" +
                            "   select to_char(t3.kjn)||'-'||to_char(t3.kjy) accountDate" +
                            "   ,(t1.sumincometotalprice-t1.sumrealtotalprice) profitTotalPrice" +
                            "   from thd_costmonthaccountdtl t1" +
                            "   inner join thd_costmonthaccount t3 on t1.parentid=t3.id" +
                            "   where t1.parentid in" +
                            "   (" +
                            "     select t2.id from thd_costmonthaccount t2" +
                            "     where t2.theprojectguid in" +
                            "     (" +
                            "       select tproject.id from resconfig tproject" +
                            "       where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                            "     )" +
                            "     and to_date(to_char(t2.kjn)||'-'||to_char(t2.kjy),'yyyy-mm')>=to_date('" + startDateStr + "','yyyy-mm')" +
                            "   )" +
                            " )" +
                            " group by accountDate";

            command.CommandText = sqlStr;
            IDataReader dr = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

            return ds;
        }

        /// <summary>
        /// 获取起始日期开始的项目或公司/分公司各月合同收入和收款信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="projectSyscode"></param>
        /// <returns></returns>
        public DataTable[] GetProjectCollectionsummoneyData(DateTime startDate, string projectSyscode)
        {
            List<DataTable> listResult = new List<DataTable>();

            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();

            string startDateStr = startDate.Year + "-" + startDate.Month;

            //--1.统计月度合同金额
            string sqlStr = "select accountDate,sum(sumincometotalprice) sumincometotalprice" +
                             " from" +
                             " (" +
                             "    select to_char(t3.kjn)||'-'||to_char(t3.kjy) accountDate,t1.sumincometotalprice" +
                             "    from thd_costmonthaccountdtl t1" +
                             "    inner join thd_costmonthaccount t3 on t1.parentid=t3.id" +
                             "    where t1.parentid in" +
                             "    (" +
                             "      select t2.id from thd_costmonthaccount t2" +
                             "      where t2.theprojectguid in" +
                             "      (" +
                             "        select tproject.id from resconfig tproject" +
                             "        where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                             "      )" +
                             "      and to_date(to_char(t2.kjn)||'-'||to_char(t2.kjy),'yyyy-mm')>=to_date('" + startDateStr + "','yyyy-mm')" +
                             "    )" +
                             " )" +
                             " group by accountDate";

            command.CommandText = sqlStr;
            IDataReader dr = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dr);

            sqlStr = "select accountDate,sum(collectionsummoney) collectionsummoney" +
                     " from" +
                     " (" +
                     "    select to_char(t1.createDate,'yyyy-mm') accountDate,t1.collectionsummoney" +
                     "    from thd_ownerquantitymaster t1" +
                     "    where t1.projectid in" +
                     "    (" +
                     "        select tproject.id from resconfig tproject" +
                     "        where tproject.ownerorgsyscode like '" + projectSyscode + "%'" +
                     "    )" +
                     "    and to_date(to_char(t1.createDate,'yyyy-mm'),'yyyy-mm')>=to_date('" + startDateStr + "','yyyy-mm')" +
                     " )" +
                     " group by accountDate";

            command.CommandText = sqlStr;
            dr = command.ExecuteReader();
            DataSet ds2 = DataAccessUtil.ConvertDataReadertoDataSet(dr);

            listResult.Add(ds.Tables[0]);
            listResult.Add(ds2.Tables[0]);

            return listResult.ToArray();
        }

        /// <summary>
        /// 查询项目预警信息
        /// </summary>
        /// <param name="oqProject"></param>
        /// <returns></returns>
        public Dictionary<CurrentProjectInfo, IList> QueryProjectWarnInfo(ObjectQuery oqProject)
        {
            Dictionary<CurrentProjectInfo, IList> dicResult = new Dictionary<CurrentProjectInfo, IList>();

            oqProject.AddFetchMode("OperationOrg", FetchMode.Eager);
            IEnumerable<CurrentProjectInfo> listProject = dao.ObjectQuery(typeof(CurrentProjectInfo), oqProject).OfType<CurrentProjectInfo>();

            listProject = from p in listProject
                          orderby p.Name ascending
                          select p;

            foreach (CurrentProjectInfo project in listProject)
            {
                if (!string.IsNullOrEmpty(project.OwnerOrg.OperationType))
                    dicResult.Add(project, GetProjectWarningTargetInfo(project.OwnerOrg.Id, project.OwnerOrg.SysCode, project.OwnerOrg.OperationType));
            }

            return dicResult;
        }

        #endregion  项目或公司、分公司预警及统计服务

        [TransManager]
        public void Test(GWBSTree wbs)
        {
            dao.Update(wbs);
        }

        /// <summary>
        /// 查询 缺省WBS节点及其子节点上面的状态为发布的任务明细个数
        /// </summary>
        /// <param name="sysCode"></param>
        /// <returns></returns>
        public int GetGWBSDetailLikeWBSSysCodeSql(string sysCode)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select count(*) from thd_gwbsdetail t1 where 1=1 and t1.thegwbssyscode like '" + sysCode + "%' and t1.state = 5";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return Convert.ToInt32(dataSet.Tables[0].Rows[0][0].ToString());
        }
        /// <summary>
        /// 查询 缺省WBS节点及其子节点上面的状态为发布的任务明细个数
        /// </summary>
        /// <param name="sysCode"></param>
        /// <returns></returns>
        public int GetGWBSDetailLikeWBSSysCodeSql(string sysCode, string sProjectId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"select count(*) from thd_gwbsdetail t1 where 1=1 and t1.theprojectguid=:ProjectId and t1.thegwbssyscode like :sysCode ||'%' and t1.state = 5";
            command.CommandText = sql;
            command.Parameters.Add(new OracleParameter("ProjectId", sProjectId));
            command.Parameters.Add(new OracleParameter("sysCode", sysCode));
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);

            return Convert.ToInt32(dataSet.Tables[0].Rows[0][0].ToString());
        }
        /// <summary>
        /// 导入任务明细检查这些定额编号在成本项里是否有重复
        /// </summary>
        /// <returns></returns>
        public bool CheckQutaCodeIsRepeat(string strSql)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql =
                @"select count(*) from thd_costitem t1 where t1.quotacode in (" + strSql + ") group by t1.quotacode having count(*)>1";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 作废明细前的判断
        /// </summary>
        /// <param name="list"></param>
        /// <param name="wbs"></param>
        /// <returns></returns>
        public string DeleteGWBSDetailBeforeOperat(List<GWBSDetail> list, GWBSTree wbs)
        {
            string errorMess = "";
            List<GWBSDetail> listCostingFlag = new List<GWBSDetail>();
            List<GWBSDetail> listProduceFlag = new List<GWBSDetail>();
            string sqlCostingFlag = "select t1.projecttaskdtlguid, t1.projecttaskdtlname from thd_projecttaskdetailaccount t1 where t1.projecttaskdtlguid in (";
            string sqlProduceFlag = "select t1.gwbsdetail, t1.gwbsdetailname from thd_gwbstaskconfirmdetail t1 where t1.gwbsdetail in (";

            foreach (GWBSDetail dtl in list)
            {
                if (dtl.CostingFlag == 1)
                {
                    sqlCostingFlag += "'" + dtl.Id + "',";
                    listCostingFlag.Add(dtl);
                }
                if (dtl.ProduceConfirmFlag == 1)
                {
                    sqlProduceFlag += "'" + dtl.Id + "',";
                    listProduceFlag.Add(dtl);
                }
            }
            sqlCostingFlag = sqlCostingFlag.Substring(0, sqlCostingFlag.Length - 1);
            sqlCostingFlag += ")";
            sqlProduceFlag = sqlProduceFlag.Substring(0, sqlProduceFlag.Length - 1);
            sqlProduceFlag += ")";
            if ((listCostingFlag != null && listCostingFlag.Count > 0) || (listProduceFlag != null && listProduceFlag.Count > 0))
            {
                if (listCostingFlag != null && listCostingFlag.Count > 0)
                {
                    #region 核算明细做了核算单的不能删除
                    ISession session = CallContext.GetData("nhsession") as ISession;
                    IDbConnection conn = session.Connection;
                    IDbCommand command = conn.CreateCommand();
                    command.CommandText = sqlCostingFlag;
                    IDataReader dataReader = command.ExecuteReader();
                    DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                    DataTable table = dataSet.Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            errorMess += "核算明细：【" + row["Projecttaskdtlname"].ToString() + "】";
                        }
                        errorMess += "已生成核算单，不能删除，请检查！";
                    }
                    else
                    {
                        #region 核算明细下的生产明细做了工程量提报不能删除
                        ObjectQuery oq = new ObjectQuery();
                        Disjunction dis = new Disjunction();
                        oq.AddCriterion(Expression.Like("TheGWBSSysCode", wbs.SysCode, MatchMode.Start));
                        foreach (GWBSDetail detail in listCostingFlag)
                        {
                            if (detail.MainResourceTypeId != null)
                            {
                                if (!string.IsNullOrEmpty(detail.DiagramNumber))
                                {
                                    dis.Add(Expression.And(Expression.And(Expression.Eq("MainResourceTypeId", detail.MainResourceTypeId), Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id)), Expression.Eq("DiagramNumber", detail.DiagramNumber)));
                                }
                                else
                                {
                                    dis.Add(Expression.And(Expression.And(Expression.Eq("MainResourceTypeId", detail.MainResourceTypeId), Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id)), Expression.IsNull("DiagramNumber")));
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(detail.DiagramNumber))
                                {
                                    dis.Add(Expression.And(Expression.And(Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id), Expression.Eq("DiagramNumber", detail.DiagramNumber)), Expression.IsNull("MainResourceTypeId")));
                                }
                                else
                                {
                                    dis.Add(Expression.And(Expression.And(Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id), Expression.IsNull("DiagramNumber")), Expression.IsNull("MainResourceTypeId")));
                                }
                            }

                        }
                        oq.AddCriterion(dis);
                        oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                        oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
                        oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);
                        IList produceDetailList = dao.ObjectQuery(typeof(GWBSDetail), oq);
                        if (produceDetailList != null && produceDetailList.Count > 0)
                        {
                            string sqlProduce = "select t1.gwbsdetail, t1.gwbsdetailname from thd_gwbstaskconfirmdetail t1 where t1.gwbsdetail in (";
                            foreach (GWBSDetail d in produceDetailList)
                            {
                                sqlProduce += "'" + d.Id + "',";
                            }
                            sqlProduce = sqlProduce.Substring(0, sqlProduce.Length - 1);
                            sqlProduce += ")";
                            ISession session1 = CallContext.GetData("nhsession") as ISession;
                            IDbConnection conn1 = session1.Connection;
                            IDbCommand command1 = conn1.CreateCommand();
                            command1.CommandText = sqlProduce;
                            IDataReader dataReader1 = command1.ExecuteReader();
                            DataSet dataSet1 = DataAccessUtil.ConvertDataReadertoDataSet(dataReader1);
                            DataTable table1 = dataSet1.Tables[0];
                            if (table1.Rows.Count > 0)
                            {
                                if (errorMess != "")
                                {
                                    errorMess += "\r\n";
                                }
                                foreach (DataRow row1 in table1.Rows)
                                {
                                    foreach (GWBSDetail p in produceDetailList)
                                    {
                                        if (row1["Gwbsdetail"].ToString() == p.Id)
                                        {
                                            errorMess += "“" + p.TheGWBS.FullPath + "“下的生产明细：【" + row1["Gwbsdetailname"].ToString() + "】 ";
                                        }
                                    }
                                }
                                errorMess += "已提报工程量，请检查！";
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                if (listProduceFlag != null && listProduceFlag.Count > 0)
                {
                    #region 生产明细做了提报工程量的不能删除
                    ISession session = CallContext.GetData("nhsession") as ISession;
                    IDbConnection conn = session.Connection;
                    IDbCommand command = conn.CreateCommand();
                    command.CommandText = sqlProduceFlag;
                    IDataReader dataReader = command.ExecuteReader();
                    DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
                    DataTable table = dataSet.Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        if (errorMess != "")
                        {
                            errorMess += "\r\n";
                        }
                        foreach (DataRow row in table.Rows)
                        {
                            errorMess += "生产明细：【" + row["Gwbsdetailname"].ToString() + "】";
                        }
                        errorMess += "已提报工程量，请检查！";
                    }
                    #endregion
                }
            }
            return errorMess;
        }

        /// <summary>
        /// 删除明细 连带相应生产明细一起删除
        /// </summary>
        /// <param name="wbs"></param>
        /// <param name="listInvalid"></param>
        /// <returns></returns>
        [TransManager]
        public GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, List<GWBSDetail> listInvalid)
        {
            dao.SaveOrUpdate(wbs);
            if (listInvalid != null && listInvalid.Count > 0)
            {
                #region 核算明细下的生产明细一样作废
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                oq.AddCriterion(Expression.Like("TheGWBSSysCode", wbs.SysCode, MatchMode.Start));
                foreach (GWBSDetail detail in listInvalid)
                {
                    if (detail.MainResourceTypeId != null)
                    {
                        if (!string.IsNullOrEmpty(detail.DiagramNumber))
                        {
                            dis.Add(Expression.And(Expression.And(Expression.Eq("MainResourceTypeId", detail.MainResourceTypeId), Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id)), Expression.Eq("DiagramNumber", detail.DiagramNumber)));
                        }
                        else
                        {
                            dis.Add(Expression.And(Expression.And(Expression.Eq("MainResourceTypeId", detail.MainResourceTypeId), Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id)), Expression.IsNull("DiagramNumber")));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(detail.DiagramNumber))
                        {
                            dis.Add(Expression.And(Expression.And(Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id), Expression.Eq("DiagramNumber", detail.DiagramNumber)), Expression.IsNull("MainResourceTypeId")));
                        }
                        else
                        {
                            dis.Add(Expression.And(Expression.And(Expression.Eq("TheCostItem.Id", detail.TheCostItem.Id), Expression.IsNull("DiagramNumber")), Expression.IsNull("MainResourceTypeId")));
                        }
                    }

                }
                oq.AddCriterion(dis);
                oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
                IList produceDetailList = dao.ObjectQuery(typeof(GWBSDetail), oq);
                if (produceDetailList != null && produceDetailList.Count > 0)
                {
                    string sqlProduce = "update thd_gwbsdetail t1 set t1.state = 2 where t1.id in (";
                    foreach (GWBSDetail d in produceDetailList)
                    {
                        sqlProduce += "'" + d.Id + "',";
                    }
                    sqlProduce = sqlProduce.Substring(0, sqlProduce.Length - 1);
                    sqlProduce += ")";

                    ISession session = CallContext.GetData("nhsession") as ISession;
                    IDbConnection conn = session.Connection;
                    IDbCommand command = conn.CreateCommand();
                    ITransaction tran = CallContext.GetData("ntransaction") as ITransaction;
                    tran.Enlist(command);

                    command.CommandText = sqlProduce;
                    command.ExecuteNonQuery();
                }
                #endregion
            }
            return wbs;
        }

        public IList SaveBatchDetail(GWBSTree wbs, string sqlWhere)
        {
            IList listResult = new ArrayList();

            dao.SaveOrUpdate(wbs);

            wbs.Details.Clear();

            List<GWBSDetail> listDtl = GetWBSDetail(wbs.Id, sqlWhere);

            listResult.Add(wbs);
            listResult.Add(listDtl);

            return listResult;
        }

        /// <summary>
        /// 保存并回写滚动计划的状态
        /// </summary>
        /// <param name="wbs"></param>
        /// <returns></returns>
        [TransManager]
        public bool SaveAndWritebackScrollPlanState(GWBSTree wbs)
        {
            dao.SaveOrUpdate(wbs);

            if (wbs.AddUpFigureProgress == 100)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("GWBSTree.Id", wbs.Id));
                oq.AddCriterion(Expression.Eq("State", EnumScheduleDetailState.有效));
                IList listPlanDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                foreach (ProductionScheduleDetail dtl in listPlanDtl)
                {
                    dtl.ActualEndDate = DateTime.Now;
                    if (dtl.ActualBeginDate == new DateTime(1900, 1, 1))
                        dtl.ActualBeginDate = DateTime.Now;

                    dtl.ActualDuration = (dtl.ActualEndDate - dtl.ActualBeginDate).Days + 1;
                }

                dao.Update(listPlanDtl);
            }

            return true;
        }

        public List<GWBSDetailLedger> GWBSDtlLedgerQuery(string sqlCondition, string createStartTime, string createEndTime)
        {
            List<GWBSDetailLedger> listResult = new List<GWBSDetailLedger>();

            if (!string.IsNullOrEmpty(createStartTime) || !string.IsNullOrEmpty(createEndTime))
            {
                string sqlQueryGroup = "select t1.projecttaskdtlid from thd_gwbsdetailledger t1" +
                                      " inner join thd_gwbsdetail t2 on t1.projecttaskdtlid=t2.id" +
                                      " inner join thd_costitem t3 on t2.costitemguid=t3.id" +
                                      " inner join thd_contractgroup t4 on t1.contractgroupid=t4.id" +
                                        sqlCondition +
                                      " group by t1.projecttaskdtlid";



                sqlQueryGroup = sqlQueryGroup.Replace("t1", "t5");
                sqlQueryGroup = sqlQueryGroup.Replace("t2", "t6");
                sqlQueryGroup = sqlQueryGroup.Replace("t3", "t7");
                sqlQueryGroup = sqlQueryGroup.Replace("t4", "t8");

                sqlCondition = " where t1.projecttaskdtlid in (" + sqlQueryGroup + ")";

            }

            string sqlQuery = "select t1.projecttaskname," +
                                   " t1.projecttaskdtlid," +
                                   " t1.projecttaskdtlname," +
                                   " t1.createtime," +
                                   " t1.contractchangemode," +
                                   " t1.contractworkamount," +
                                   " t1.contractprice," +
                                   " t1.contracttotalprice," +
                                   " t1.responsiblecostchangemode," +
                                   " t1.responsibleworkamount," +
                                   " t1.responsibleprice," +
                                   " t1.responsibletotalprice," +
                                   " t1.plancostchangemode," +
                                   " t1.planworkamount," +
                                   " t1.planprice," +
                                   " t1.plantotalprice," +
                                   " t1.workamountunitname," +
                                   " t1.priceunitname," +
                                   " t3.quotacode," +
                                   " t2.mainresourcetypename," +
                                   " t2.mainresourcetypespec," +
                                   " t2.mainresourcetypequality," +
                                   " t4.code," +
                                   " t4.contractname" +
                                   " from thd_gwbsdetailledger t1" +
                                   " inner join thd_gwbsdetail t2 on t1.projecttaskdtlid = t2.id" +
                                   " inner join thd_costitem t3 on t2.costitemguid = t3.id" +
                                   " inner join thd_contractgroup t4 on t1.contractgroupid = t4.id" + sqlCondition;

            DataSet dsDtl = SearchSQL(sqlQuery);
            foreach (DataRow row in dsDtl.Tables[0].Rows)
            {
                GWBSDetailLedger dtl = new GWBSDetailLedger();
                dtl.ProjectTaskName = row["projecttaskname"].ToString();
                dtl.ProjectTaskDtlID = row["projecttaskdtlid"].ToString();
                dtl.ProjectTaskDtlName = row["projecttaskdtlname"].ToString();
                dtl.CreateTime = ClientUtil.ToDateTime(row["createtime"].ToString());
                dtl.ContractChangeMode = (ContractIncomeChangeModeEnum)ClientUtil.ToInt(row["contractchangemode"]);
                dtl.ContractWorkAmount = ClientUtil.ToDecimal(row["contractworkamount"].ToString());
                dtl.ContractPrice = ClientUtil.ToDecimal(row["contractprice"].ToString());
                dtl.ContractTotalPrice = ClientUtil.ToDecimal(row["contracttotalprice"].ToString());
                dtl.ResponsibleCostChangeMode = (ResponsibleCostChangeModeEnum)ClientUtil.ToInt(row["contractchangemode"]);
                dtl.ResponsibleWorkAmount = ClientUtil.ToDecimal(row["responsibleworkamount"].ToString());
                dtl.ResponsiblePrice = ClientUtil.ToDecimal(row["responsibleprice"].ToString());
                dtl.ResponsibleTotalPrice = ClientUtil.ToDecimal(row["responsibletotalprice"].ToString());
                dtl.PlanCostChangeMode = (PlanCostChangeModeEnum)ClientUtil.ToInt(row["plancostchangemode"]);
                dtl.PlanWorkAmount = ClientUtil.ToDecimal(row["planworkamount"].ToString());
                dtl.PlanPrice = ClientUtil.ToDecimal(row["planprice"].ToString());
                dtl.PlanTotalPrice = ClientUtil.ToDecimal(row["plantotalprice"].ToString());
                dtl.WorkAmountUnitName = row["workamountunitname"].ToString();
                dtl.PriceUnitName = row["priceunitname"].ToString();

                dtl.Temp_WBSDtl = new GWBSDetail();
                dtl.Temp_WBSDtl.TheCostItem = new CostItem();
                dtl.TheContractGroup = new ContractGroup();
                dtl.Temp_WBSDtl.TheCostItem.QuotaCode = row["quotacode"].ToString();
                dtl.Temp_WBSDtl.MainResourceTypeName = row["mainresourcetypename"].ToString();
                dtl.Temp_WBSDtl.MainResourceTypeSpec = row["mainresourcetypespec"].ToString();
                dtl.Temp_WBSDtl.MainResourceTypeQuality = row["mainresourcetypequality"].ToString();
                dtl.TheContractGroup.Code = row["code"].ToString();
                dtl.TheContractGroup.ContractName = row["contractname"].ToString();

                listResult.Add(dtl);
            }

            return listResult;
        }

        /// <summary>
        /// 工程任务合价查询
        /// </summary>
        /// <param name="wbs"></param>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        public DataTable SelectGWBSValence(GWBSTree wbs, CurrentProjectInfo projectInfo)
        {
            string sqlQuery = "select t1.name,t1.Code,t1.id,t1.syscode,t1.parentnodeid,t1.categorynodetype,t1.tlevel,t1.orderno," +
                "t2.name as dName,t2.workamountunitname,t2.orderno as dOrderno,t2.planworkamount,t2.planprice,t2.plantotalprice," +
                "t2.contractprojectquantity,t2.contractprice,t2.contracttotalprice," +
                "t2.responsibilitilyworkamount,t2.responsibilitilyprice,t2.responsibilitilytotalprice ,t2.id detailID " +
                "from thd_gwbstree t1 left join thd_gwbsdetail t2 on t1.id = t2.parentid " +
                "where t1.theprojectguid = '" + projectInfo.Id + "' and t1.syscode like '" + wbs.SysCode + "%' order by t1.tlevel";
            DataSet dsDtl = SearchSQL(sqlQuery);
            return dsDtl.Tables[0];
        }
        public DataTable SelectGWBSValence(string sSysCodes, string sProjectId)
        {

            string sqlQuery = @"select t1.name,t1.Code,t1.id,t1.syscode,t1.parentnodeid,t1.categorynodetype,t1.tlevel,t1.orderno,
                 t2.name as dName,t2.workamountunitname,t2.orderno as dOrderno,t2.planworkamount,t2.planprice,t2.plantotalprice, 
                 t2.contractprojectquantity,t2.contractprice,t2.contracttotalprice, 
                 t2.responsibilitilyworkamount,t2.responsibilitilyprice,t2.responsibilitilytotalprice,t2.id detailID   
                 from thd_gwbstree t1 
                 join (select distinct tt.syscode from thd_gwbstree tt 
                        join thd_gwbsdetail tt1 on tt.id=tt1.parentid
                        join thd_gwbsdetailcostsubject tt2 on tt1.id=tt2.gwbsdetailid 
                            and tt2.resourcecatesyscode like '{0}'
                        where tt.theprojectguid='{1}') t3 on instr(t3.syscode,t1.syscode)>0 
                 left join thd_gwbsdetail t2 on t1.id = t2.parentid 
                 AND T2.ID IN(SELECT distinct gwbsdetailid FROM THD_GWBSDETAILCOSTSUBJECT WHERE theprojectguid='{1}' AND resourcecatesyscode like '{0}')
                 where t1.theprojectguid = '{1}'  order by t1.tlevel,t1.orderno";
            //  
            sqlQuery = string.Format(sqlQuery, sSysCodes + "%", sProjectId);
            DataSet dsDtl = SearchSQL(sqlQuery);
            return dsDtl.Tables[0];
        }
        public IList GetWBSDetailNum(string sGWBSTreeID, string[] sGWBSDetialIDs)
        {
            if (string.IsNullOrEmpty(sGWBSTreeID) || sGWBSDetialIDs == null || sGWBSDetialIDs.Length == 0) throw new Exception("GWBS明细不能为空");
            IList lstResult = new ArrayList();
            Hashtable htTable = new Hashtable();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection cnn = session.Connection;
            IDbCommand command = cnn.CreateCommand();
            string sTempIDs = string.Join("','", sGWBSDetialIDs);
            sTempIDs = string.Format("'{0}'", sTempIDs);
            string sql = @"select max( cast(substr(t.code,instr(t.code,'-',-1)+1) as number)) code ,max(t.orderno) orderno from thd_gwbsdetail t  where t.parentid ='{0}'";
            sql = string.Format(sql, sGWBSTreeID, sGWBSDetialIDs);
            command.CommandText = sql;

            IDataReader dataReader = command.ExecuteReader(CommandBehavior.Default);
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            DataTable dataTable = ds.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                lstResult.Insert(0, ClientUtil.ToInt(row["code"]));//最大code
                lstResult.Insert(1, ClientUtil.ToInt(row["orderno"]));//最大orderno
                lstResult.Insert(2, GetServerTime());//最大orderno
                lstResult.Insert(3, htTable);
                break;
            }
            sql = "select changeparentid,count(1) len  from thd_gwbsdetail where changeparentid in ({0}) and parentid='{1}' GROUP BY changeparentid";
            sql = string.Format(sql, sTempIDs, sGWBSTreeID);
            command.CommandText = sql;

            dataReader = command.ExecuteReader(CommandBehavior.Default);
            ds = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            dataTable = ds.Tables[0];
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    htTable.Add(ClientUtil.ToString(row["changeparentid"]), ClientUtil.ToInt(row["len"]));
                }
            }
            return lstResult;
        }

        #region 工程成本批量维护
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysCode"></param>
        /// <returns></returns>
        public DataSet GetGWBSDetailCostSubject(string sysCode)
        {
            string SQL = string.Format(@"SELECT T2.MAINRESTYPEFLAG,
       t2.CONTRACTBASEPRICE,t1.DiagramNumber,
       t2.RESPONSIBLEBASEPRICE,
       t2.PLANBASEPRICE,
       t2.ContractQuotaQuantity,
       t2.ResponsibleQuotaNum,
       t2.PlanQuotaNum,
       t2.GWBSDETAILID
  from thd_gwbsdetail t1, thd_gwbsdetailcostsubject t2
 where t1.id = t2.gwbsdetailid
   and t1.thegwbssyscode like '{0}%'", sysCode);
            return SearchSQL(SQL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wbs"></param>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        public DataSet SelectGWBSResourceCost(GWBSTree wbs, CurrentProjectInfo projectInfo)
        {
            string sqlQuery = "select t2.DiagramNumber,t2.costitemguid,t1.name,t1.Code,t1.id,t1.syscode,t1.parentnodeid,t1.categorynodetype,t1.tlevel,t1.orderno," +
                "t2.name as dName,t2.workamountunitname,t2.orderno as dOrderno,t2.planworkamount,t2.planprice,t2.plantotalprice," +
                "t2.contractprojectquantity,t2.contractprice,t2.contracttotalprice, t3.quotacode," +
                "t2.responsibilitilyworkamount,t2.responsibilitilyprice,t2.responsibilitilytotalprice ,t2.id detailID " +
                "from thd_gwbstree t1 left join thd_gwbsdetail t2 on t1.id = t2.parentid left join thd_costitem t3 on  t2.costitemguid=t3.id" +
                " where t1.theprojectguid = '" + projectInfo.Id + "' and t1.syscode like '" + wbs.SysCode + "%' order by t1.tlevel";

            return SearchSQL(sqlQuery);
        }

        public DataSet GetSubjectCostQuotaByQuotaCode(string codes)
        {
            string sqlQuery = string.Format(@"select s.*,c.quotacode,c.id as ItemID
  from thd_subjectcostquota s
 inner join thd_CostItem c
    on c.id = s.costitemid
 where s.STATE =2 and c.itemState=2 and c.quotacode in ('{0}')", codes);

            return SearchSQL(sqlQuery);
        }

        public DataSet GetSubjectCostQuotas(string id, string sysCode)
        {
            string sqlQuery = string.Format(@"select s.*, c.quotacode, c.id as ItemID
  from thd_subjectcostquota s
 inner join thd_CostItem c
    on c.id = s.costitemid
 inner join (select t3.quotacode
               from thd_gwbstree t1
               left join thd_gwbsdetail t2
                 on t1.id = t2.parentid
               left join thd_costitem t3
                 on t2.costitemguid = t3.id
              where t1.theprojectguid = '{0}'
                and t1.syscode like '{1}%'
             union
             select code as quotacode
               from thd_GWBSDetailImport
              where PROJECTID = '{0}') t
    on t.quotacode = c.quotacode
 where s.STATE = 2
   and c.itemState = 2", id, sysCode);

            return SearchSQL(sqlQuery);
        }


        #endregion

        //[TransManager]
        public bool UpdateGWBSAndScheduleOrderNO(List<GWBSTree> lst)
        {
            try
            {
                //dao.SaveOrUpdateCopy(ilst);
                string sql = "BEGIN ";
                foreach (var item in lst)
                {
                    sql += string.Format(@" update thd_gwbstree set orderno ={0} where  id = '{1}' ;", item.OrderNo, item.Id);
                }
                string strIds = string.Join("','", lst.Select(p => p.Id).ToArray());

                sql += string.Format((@"UPDATE thd_WeekScheduleDetail A
   SET (orderno) =
       (SELECT orderno FROM thd_gwbstree B WHERE A.GWBSTREE = B.ID)
 WHERE GWBSTREE IN (SELECT B.ID FROM thd_gwbstree B WHERE B.ID in（ '" + strIds + "' ）);"));

                sql += "END;";
                return SaveSQL(sql) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IList SaveOrUpdateShareRateInfo(IList ilistChanged, IList ilistDel)
        {
            var lstGWBSid = SaveOrUpdateShareRate(ilistChanged, ilistDel);

            UpdateScheduleProssPercent(lstGWBSid);

            return ilistChanged;
        }

        [TransManager]
        private List<string> SaveOrUpdateShareRate(IList ilistChanged, IList ilistDel)
        {
            ObjectQuery oq = new ObjectQuery();
            List<string> lstGWBSid = new List<string>();
            IList listLedger = new List<GWBSDetailLedger>();
            foreach (GWBSTreeDetailRelationship item in ilistChanged)
            {
                lstGWBSid.Add(item.TargetGWBSTreeID);
                if (item.TargetGWBSDetail != null)
                {
                    dao.SaveOrUpdate(item.TargetGWBSDetail);

                    #region 工程量提报信息中计划量更新
                    //工长提报量，
                    oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("GWBSDetail.Id", item.TargetGWBSDetail.Id));
                    oq.AddFetchMode("GWBSTree", FetchMode.Eager);
                    var ilstConfirm = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);

                    //如果工长提报量的数据则不做删除，只将计划值设置为0，否则删除
                    if (ilstConfirm != null && ilstConfirm.Count > 0)
                    {
                        GWBSTaskConfirm confirmDetail = ilstConfirm.OfType<GWBSTaskConfirm>().ElementAt(0);
                        confirmDetail.PlannedQuantity = item.TargetGWBSDetail.PlanWorkAmount;
                        confirmDetail.ProgressAfterConfirm = confirmDetail.PlannedQuantity == 0 ? 100 : Math.Round(confirmDetail.ProgressAfterConfirm / confirmDetail.PlannedQuantity, 4) * 100;
                        confirmDetail.ProgressBeforeConfirm = confirmDetail.PlannedQuantity == 0 ? 100 : Math.Round(confirmDetail.ProgressBeforeConfirm / confirmDetail.PlannedQuantity, 4) * 100;
                        confirmDetail.CompletedPercent = confirmDetail.PlannedQuantity == 0 ? 100 : Math.Round(confirmDetail.CompletedPercent / confirmDetail.PlannedQuantity, 4) * 100;
                        dao.SaveOrUpdate(confirmDetail);
                    }
                    #endregion
                }

                item.TargetGWBSDetailID = item.TargetGWBSDetail.Id;

                dao.SaveOrUpdate(item);

                #region 记录明细台账
                oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectTaskDtlID", item.TargetGWBSDetail.Id));
                var ilst = dao.ObjectQuery(typeof(GWBSDetailLedger), oq);
                GWBSDetailLedger led = (ilst == null || ilst.Count == 0) ? new GWBSDetailLedger() : ilst.OfType<GWBSDetailLedger>().ElementAt(0);

                led.ProjectTaskID = item.TargetGWBSDetail.TheGWBS.Id;
                led.ProjectTaskName = item.TargetGWBSDetail.TheGWBS.Name;
                led.TheProjectTaskSysCode = item.TargetGWBSDetail.TheGWBS.SysCode;

                led.ProjectTaskDtlID = item.TargetGWBSDetail.Id;
                led.ProjectTaskDtlName = item.TargetGWBSDetail.Name;

                led.ContractWorkAmount = item.TargetGWBSDetail.ContractProjectQuantity;
                led.ContractPrice = item.TargetGWBSDetail.ContractPrice;
                led.ContractTotalPrice = item.TargetGWBSDetail.ContractTotalPrice;

                led.ResponsiblePrice = item.TargetGWBSDetail.ResponsibilitilyPrice;
                led.ResponsibleWorkAmount = item.TargetGWBSDetail.ResponsibilitilyWorkAmount;
                led.ResponsibleTotalPrice = item.TargetGWBSDetail.ResponsibilitilyTotalPrice;

                led.PlanPrice = item.TargetGWBSDetail.PlanPrice;
                led.PlanWorkAmount = item.TargetGWBSDetail.PlanWorkAmount;
                led.PlanTotalPrice = item.TargetGWBSDetail.PlanTotalPrice;

                led.WorkAmountUnit = item.TargetGWBSDetail.WorkAmountUnitGUID;
                led.WorkAmountUnitName = item.TargetGWBSDetail.WorkAmountUnitName;

                led.PriceUnit = item.TargetGWBSDetail.PriceUnitGUID;
                led.PriceUnitName = item.TargetGWBSDetail.PriceUnitName;

                led.TheContractGroup = GetObjectById(typeof(ContractGroup), item.TargetGWBSDetail.ContractGroupGUID) as ContractGroup;

                led.TheProjectGUID = item.TargetGWBSDetail.TheProjectGUID;
                led.TheProjectName = item.TargetGWBSDetail.TheProjectName;
                listLedger.Add(led);
                //dao.SaveOrUpdate(led);
                #endregion
            }
            #region 删除分摊明细处理
            foreach (GWBSTreeDetailRelationship item in ilistDel)
            {
                lstGWBSid.Add(item.TargetGWBSTreeID);
                if (item.TargetGWBSDetail != null && item.TargetGWBSDetail.Id != null)
                {
                    oq = new ObjectQuery();
                    //明细
                    oq.AddCriterion(Expression.Eq("Id", item.TargetGWBSDetail.Id));
                    oq.AddFetchMode("TheGWBS", FetchMode.Eager);
                    oq.AddFetchMode("ListCostSubjectDetails", FetchMode.Eager);
                    var ilstDetail = dao.ObjectQuery(typeof(GWBSDetail), oq); //
                    GWBSDetail detail = null;
                    //GWBSTaskConfirm confirmDetail = null; ;
                    if (ilstDetail != null && ilstDetail.Count > 0)
                    {
                        detail = ilstDetail.OfType<GWBSDetail>().ElementAt(0);
                        //confirmDetail = detail.GWBSTaskConfirmDetail;
                        detail.ContractProjectQuantity = 0;
                        detail.ContractTotalPrice = 0;
                        detail.PlanWorkAmount = 0;
                        detail.PlanTotalPrice = 0;
                        detail.ResponsibilitilyWorkAmount = 0;
                        detail.ResponsibilitilyTotalPrice = 0;
                        //detail.AddupAccFigureProgress = 100;
                        detail.ProgressConfirmed = 100;
                        foreach (GWBSDetailCostSubject detailcost in detail.ListCostSubjectDetails)
                        {
                            detailcost.ContractProjectAmount = 0;
                            detailcost.ContractTotalPrice = 0;
                            detailcost.PlanWorkAmount = 0;
                            detailcost.PlanTotalPrice = 0;
                            detailcost.ResponsibilitilyWorkAmount = 0;
                            detailcost.ResponsibilitilyTotalPrice = 0;
                        }
                        dao.SaveOrUpdate(detail);
                    }
                    else
                    {
                        continue;
                    }

                    //工长提报量，
                    oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("GWBSDetail.Id", item.TargetGWBSDetail.Id));
                    oq.AddFetchMode("GWBSTree", FetchMode.Eager);
                    var ilstConfirm = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);

                    //如果工长提报量的数据则不做删除，只将计划值设置为0，否则删除
                    if (ilstConfirm != null && ilstConfirm.Count > 0)
                    {
                        item.Rate = 0;
                        dao.SaveOrUpdate(item);

                        GWBSTaskConfirm confirmDetail = ilstConfirm.OfType<GWBSTaskConfirm>().ElementAt(0);
                        confirmDetail.PlannedQuantity = 0;
                        confirmDetail.ProgressAfterConfirm = 100;
                        confirmDetail.ProgressBeforeConfirm = 100;
                        dao.SaveOrUpdate(confirmDetail);
                    }
                    else
                    {
                        //明细台账
                        oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("ProjectTaskDtlID", item.TargetGWBSDetail.Id));
                        var ilst = dao.ObjectQuery(typeof(GWBSDetailLedger), oq); //
                        if (ilst != null && ilst.Count > 0)
                        {
                            GWBSDetailLedger led = ilst.OfType<GWBSDetailLedger>().ElementAt(0);
                            dao.Delete(led);
                        }

                        if (detail != null)
                        {
                            dao.Delete(detail);
                        }
                        dao.Delete(item);
                    }
                }
            }
            #endregion

            #region 根据明细的明细类型，更新节点的节点类型
            var lst = lstGWBSid.Distinct().ToList();
            oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (string wbsId in lst)
            {
                dis.Add(Expression.Eq("Id", wbsId));
            }
            oq.AddCriterion(dis);
            oq.AddFetchMode("Details", FetchMode.Eager);
            var ilstGWBSTree = dao.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>().ToList();
            foreach (GWBSTree item in ilstGWBSTree)
            {
                //如果明细为空，则节点的4个标志均为0,否则只要有一条明细是1，节点的相应标志便为1
                if (item.Details != null && item.Details.Count != 0)
                {
                    item.ResponsibleAccFlag = item.Details.Any(p => p.ResponseFlag == 1);
                    item.CostAccFlag = item.Details.Any(p => p.CostingFlag == 1);
                    item.ProductConfirmFlag = item.Details.Any(p => p.ProduceConfirmFlag == 1);
                    item.SubContractFeeFlag = item.Details.Any(p => p.SubContractFeeFlag);
                }
                else
                {
                    item.ResponsibleAccFlag = false;
                    item.CostAccFlag = false;
                    item.ProductConfirmFlag = false;
                    item.SubContractFeeFlag = false;
                }
            }
            dao.Update(ilstGWBSTree);

            #endregion

            dao.SaveOrUpdate(listLedger);

            return lstGWBSid.Distinct().ToList();
        }
        /// <summary>
        /// 更新形象进度计划的完成百分比，最后提报时间
        /// </summary>
        /// <param name="lstid"></param>
        private void UpdateScheduleProssPercent(List<string> lstid)
        {
            if (lstid == null || lstid.Count == 0)
            {
                return;
            }

            //更新进度计划，袋盖
            string strIds = "'" + string.Join("','", lstid.ToArray()) + "'";
            string sql = string.Format(@"update thd_weekscheduledetail a
   set taskcompletedpercent =
       (select case sum(c.planworkamount * c.planprice)
                 when 0 then
                  100
                 else
                  case
                    when (sum(b.quantiyafterconfirm * c.planprice) * 100 /
                         sum(c.planworkamount * c.planprice)) > 100 then
                     100
                    else
                     (sum(b.quantiyafterconfirm * c.planprice) * 100 /
                     sum(c.planworkamount * c.planprice))
                  end
               end as prosspercent
          from thd_gwbsdetail c
          left join thd_gwbstaskconfirmdetail b
            on b.gwbsdetail = c.id
           and b.gwbstree = c.parentid
          left join thd_weekscheduledetail e
            on e.gwbstree = b.gwbstree
          left join thd_weekschedulemaster d
            on d.execscheduletype = 40
           and d.id = e.parentid
         where a.gwbstree = c.parentid
           and c.state = '5'),
       actualenddate       =
       (select max(b.realoperationdate)
          from thd_gwbstaskconfirmdetail b
         where a.gwbstree = b.gwbstree)
 where gwbstree in ({0})", strIds);
            IList list = new ArrayList();
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        #region 节点明细变更时，根据分摊比例变更子节点明细，报量进度，总计划进度
        /// <summary>
        /// 根据分摊比例更新子节点信息、形象进度信息，总进度计划信息
        /// </summary>
        /// <param name="gWBSTree"></param>
        private void UpdateShareRateAfterSourseChanged(GWBSTree gWBSTree)
        {
            //主节点明细变动时修改，子节点变，形象进度表，进度计划中的进度%
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            //1,获取节点下的子节点及明细，资源耗用等
            dis = new Disjunction();
            dis.Add(Expression.And(Expression.Eq("CategoryNodeType", NodeType.LeafNode), Expression.Like("SysCode", gWBSTree.SysCode, MatchMode.Start)));

            oq = new ObjectQuery();
            oq.AddCriterion(dis);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            List<GWBSTree> lstLeafGWBSTree = dao.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>().ToList();
            if (lstLeafGWBSTree == null || lstLeafGWBSTree.Count == 0)
            {
                return;
            }

            //2,获取节点下明细相关的分摊比例关系
            oq = new ObjectQuery();
            dis = new Disjunction();
            foreach (GWBSDetail item in gWBSTree.Details)
            {
                dis.Add(Expression.Eq("SourseGWBSDetailID", item.Id));
            }
            oq.AddCriterion(dis);
            oq.AddCriterion(Expression.Eq("SourseGWBSTreeID", gWBSTree.Id));
            List<GWBSTreeDetailRelationship> lstRelation = dao.ObjectQuery(typeof(GWBSTreeDetailRelationship), oq).OfType<GWBSTreeDetailRelationship>().ToList();

            //3，获取相关形象进度信息
            oq = new ObjectQuery();
            dis = new Disjunction();
            foreach (GWBSTree leafGWBS in lstLeafGWBSTree)
            {
                dis.Add(Expression.Eq("GWBSTree.Id", leafGWBS.Id));
            }
            oq.AddCriterion(dis);
            List<GWBSTaskConfirm> lstTaskConfirm = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq).OfType<GWBSTaskConfirm>().ToList();

            //4，获取总进度计划信息
            oq = new ObjectQuery();
            dis = new Disjunction();
            foreach (GWBSTree leafGWBS in lstLeafGWBSTree)
            {
                dis.Add(Expression.Eq("GWBSTree.Id", leafGWBS.Id));
            }
            oq.AddCriterion(dis);
            List<WeekScheduleDetail> lstWeekSchedule = dao.ObjectQuery(typeof(WeekScheduleDetail), oq).OfType<WeekScheduleDetail>().ToList();

            //变更的明细
            var lstGWBSDeatailChanged = new List<GWBSDetail>();
            //
            var lstTaskConfirmChanged = new List<GWBSTaskConfirm>();
            //
            var lstWeekScheduleChanged = new List<WeekScheduleDetail>();
            //待删除的资源耗用
            List<GWBSDetailCostSubject> ilstRemoveCostSub = new List<GWBSDetailCostSubject>();
            foreach (GWBSDetail sourseDetail in gWBSTree.Details)
            {
                var lstCurDetailRelation = lstRelation.Where(p => p.SourseGWBSTreeID == gWBSTree.Id && p.SourseGWBSDetailID == sourseDetail.Id).ToList();
                //如果存在分摊比例关系，则根据比例关系更新明细表数据
                if (lstCurDetailRelation != null && lstCurDetailRelation.Count != 0)
                {
                    foreach (var CurDetailRelation in lstCurDetailRelation)
                    {
                        var curGWBS = lstLeafGWBSTree.FirstOrDefault(p => p.Id == CurDetailRelation.TargetGWBSTreeID);
                        if (curGWBS != null)
                        {
                            //叶节点明细处理
                            var curDetail = curGWBS.Details.FirstOrDefault(p => p.Id == CurDetailRelation.TargetGWBSDetailID) as GWBSDetail;

                            UpdateDetail(curDetail, sourseDetail, CurDetailRelation.Rate, ilstRemoveCostSub);
                            lstGWBSDeatailChanged.Add(curDetail);
                            if (lstTaskConfirm == null || lstTaskConfirm.Count == 0)
                            {
                                continue;
                            }
                            //当前节点下的所有任务进度
                            var lstCurGWBSDetailConfirm = lstTaskConfirm.Where(p => p.GWBSTree.Id == curGWBS.Id).ToList();
                            if (lstCurGWBSDetailConfirm == null || lstCurGWBSDetailConfirm.Count == 0)
                            {
                                continue;
                            }
                            //形象进度进度处理
                            var curDetailConfirm = lstCurGWBSDetailConfirm.FirstOrDefault(p => p.GWBSDetail.Id == curDetail.Id);
                            if (curDetailConfirm == null)
                            {
                                continue;
                            }
                            //【原计划量】，【新计划量】百分比 （【现有%】=【原计划量】*【各种%】/【新计划量】）
                            var changeRate = curDetail.PlanWorkAmount == 0 ? 0 : Math.Round(curDetailConfirm.PlannedQuantity / curDetail.PlanWorkAmount, 4);
                            curDetailConfirm.PlannedQuantity = curDetail.PlanWorkAmount;
                            curDetailConfirm.CompletedPercent *= changeRate;
                            curDetailConfirm.ProgressAfterConfirm *= changeRate;
                            curDetailConfirm.ProgressBeforeConfirm *= changeRate;
                            lstTaskConfirmChanged.Add(curDetailConfirm);

                            //总进度计划进度处理
                            var curWeekSchedule = lstWeekSchedule.FirstOrDefault(p => p.GWBSTree.Id == curGWBS.Id);
                            if (curWeekSchedule != null)
                            {
                                var sumPlannedQuantity = lstCurGWBSDetailConfirm.Sum(p => p.PlannedQuantity);
                                curWeekSchedule.TaskCompletedPercent = sumPlannedQuantity == 0 ? 100 : Math.Round(lstCurGWBSDetailConfirm.Sum(p => p.QuantiyAfterConfirm) / sumPlannedQuantity, 4);
                                lstWeekScheduleChanged.Add(curWeekSchedule);
                            }

                        }
                    }
                }
            }
            dao.SaveOrUpdate((IList)lstGWBSDeatailChanged);
            dao.SaveOrUpdate((IList)lstTaskConfirmChanged);
            dao.SaveOrUpdate((IList)lstWeekScheduleChanged);
            dao.Delete(ilstRemoveCostSub);
        }

        private void UpdateDetail(GWBSDetail targetdetail, GWBSDetail soursedetail, decimal rate, List<GWBSDetailCostSubject> ilstRemoveCostSub)
        {
            targetdetail.ContractProjectQuantity = soursedetail.ContractProjectQuantity * rate;// 0; 
            targetdetail.ContractTotalPrice = soursedetail.ContractTotalPrice * rate;// 0;
            targetdetail.PlanWorkAmount = soursedetail.PlanWorkAmount * rate;// 0; 
            targetdetail.PlanTotalPrice = soursedetail.PlanTotalPrice * rate;//0;
            targetdetail.ResponsibilitilyWorkAmount = soursedetail.ResponsibilitilyWorkAmount * rate;//0; 
            targetdetail.ResponsibilitilyTotalPrice = soursedetail.ResponsibilitilyTotalPrice * rate;//0targetdetail 
            targetdetail.AddupAccFigureProgress = GetProgressPercent(targetdetail.AddupAccQuantity, targetdetail.PlanWorkAmount);//targetdetail.PlanWorkAmount == 0 ? 0 : Math.Round(targetdetail.AddupAccQuantity / targetdetail.PlanWorkAmount, 4) * 100;
            targetdetail.ProgressConfirmed = GetProgressPercent(targetdetail.QuantityConfirmed, targetdetail.PlanWorkAmount);//targetdetail.PlanWorkAmount == 0 ? 0 : Math.Round(targetdetail.QuantityConfirmed / targetdetail.PlanWorkAmount, 4) * 100;
            targetdetail.TaskFinishedPercent = soursedetail.TaskFinishedPercent * rate;//任务完成百分比

            //targetdetail.AddupAccQuantity = soursedetail.AddupAccQuantity;//累计核算工程量    
            //targetdetail.QuantityConfirmed = soursedetail.QuantityConfirmed;//累计工长确认工程量   
            targetdetail.ResponsibilitilyPrice = soursedetail.ResponsibilitilyPrice;//责任单价 
            targetdetail.ContractPrice = soursedetail.ContractPrice;//合同单价 
            targetdetail.PlanPrice = soursedetail.PlanPrice;//计划单价 
            targetdetail.Name = soursedetail.Name;//      
            targetdetail.TheCostItem = soursedetail.TheCostItem; //成本项                                   
            targetdetail.Summary = soursedetail.Summary;//	//摘要                        
            targetdetail.ContentDesc = soursedetail.ContentDesc;//	// 	内容说明     
            //targetdetail.FinishedWorkAmount = soursedetail.FinishedWorkAmount;//完工工程量
            targetdetail.WorkAmountUnitGUID = soursedetail.WorkAmountUnitGUID;//工程量计量单位guid
            targetdetail.WorkAmountUnitName = soursedetail.WorkAmountUnitName;//工程量计量单位
            targetdetail.PriceUnitGUID = soursedetail.PriceUnitGUID;//单价单位guid
            targetdetail.PriceUnitName = soursedetail.PriceUnitName;//单价单位 
            //targetdetail.DetailExecuteDesc = soursedetail.DetailExecuteDesc;//明细执行说明
            targetdetail.ContractGroupType = soursedetail.ContractGroupType;//契约组类型
            targetdetail.State = soursedetail.State;//
            targetdetail.CurrentStateTime = soursedetail.CurrentStateTime;//
            targetdetail.ContractGroupGUID = soursedetail.ContractGroupGUID;//契约组guid
            targetdetail.ContractGroupCode = soursedetail.ContractGroupCode;//契约组代码   
            //targetdetail.WorkPart = soursedetail.WorkPart;//部位
            targetdetail.WorkUseMaterial = soursedetail.WorkUseMaterial;//材料
            targetdetail.WorkMethod = soursedetail.WorkMethod;//做法
            targetdetail.TheProjectGUID = soursedetail.TheProjectGUID;//所属项目guid   
            targetdetail.TheProjectName = soursedetail.TheProjectName;//所属项目名称  
            targetdetail.UpdatedDate = soursedetail.UpdatedDate;//    
            targetdetail.MainResourceTypeId = soursedetail.MainResourceTypeId;//
            targetdetail.MainResourceTypeName = soursedetail.MainResourceTypeName;//
            targetdetail.MainResourceTypeSpec = soursedetail.MainResourceTypeSpec;//
            targetdetail.MainResourceTypeQuality = soursedetail.MainResourceTypeQuality;//
            targetdetail.OrderNo = soursedetail.OrderNo;//
            targetdetail.DiagramNumber = soursedetail.DiagramNumber;//
            targetdetail.TheCostItemCateSyscode = soursedetail.TheCostItemCateSyscode;//	成本项分类层次码              
            targetdetail.ContractGroupName = soursedetail.ContractGroupName;//契约组名称  径                
            //targetdetail.ChangeParentID = soursedetail.Id;////签证变更后父节点(依据那条gwbs明细变更)    

            #region 不用重新复制的字段
            //targetdetail.NGUID = soursedetail.NGUID;//
            //targetdetail.temp1 = soursedetail.temp1;//
            //targetdetail.ProjectTaskTypeCode = soursedetail.ProjectTaskTeypeCode;////工程任务类型代码  
            //targetdetail.BearOrgGUID = soursedetail.BearOrgGUID;//// 承担组织guid
            //targetdetail.BearOrgName = soursedetail.BearOrgName;//     
            //targetdetail.Id = soursedetail.id;//
            //targetdetail.Version = soursedetail.version;//
            //targetdetail.Code = soursedetail.code;//
            //targetdetail.parentid = soursedetail.parentid;//所属工程wbs      
            //targetdetail.CreateTime = soursedetail.CreateTime;//    
            //targetdetail.ContractProject = soursedetail.ContractProject;////分包队伍                                    
            //targetdetail.ContractProjectName = soursedetail.ContractProjectName;//分包队伍名称    
            //targetdetail.TheGWBSFullPath = soursedetail.TheGWBSFullPath;//	所属工程任务完整路
            //targetdetail.TheGWBSSysCode = soursedetail.TheGWBSSysCode;//
            //targetdetail.ResponseFlag = soursedetail.ResponseFlag;//	责任核算标志                    
            //targetdetail.ProduceConfirmFlag = soursedetail.ProduceConfirmFlag;//生产确认标志                  
            //targetdetail.CostingFlag = soursedetail.CostingFlag;//	成本核算标志                       
            //targetdetail.SubContractStepRate = soursedetail.SubContractStepRate;//   
            //targetdetail.SubContractFeeFlag = soursedetail.SubContractFeeFlag;//             
            #endregion
            if (soursedetail.ListCostSubjectDetails == null || soursedetail.ListCostSubjectDetails.Count == 0)
            {
                ilstRemoveCostSub.AddRange(targetdetail.ListCostSubjectDetails);
                targetdetail.ListCostSubjectDetails.Clear();
                return;
            }

            if (targetdetail.ListCostSubjectDetails == null || targetdetail.ListCostSubjectDetails.Count == 0)
            {
                var contractGroup = new ContractGroup()
                {
                    Code = targetdetail.ContractGroupCode,
                    Id = targetdetail.ContractGroupGUID,
                    ContractName = targetdetail.ContractGroupName,
                    ContractGroupType = targetdetail.ContractGroupType
                };
                targetdetail = soursedetail.CloneByRate(targetdetail.TheGWBS, contractGroup, rate);
                return;
            }
            //如果soursedetail中存在,源targetdetail中不存在则新增
            foreach (GWBSDetailCostSubject sourseCost in soursedetail.ListCostSubjectDetails)
            {
                GWBSDetailCostSubject targetCost = targetdetail.ListCostSubjectDetails.FirstOrDefault(p => p.ForwardCostSubjectId == sourseCost.Id); ;
                //由于前期未加字段ForwardCostSubjectId,故保留原有的匹配方式
                if (targetCost == null)
                {
                    targetCost = targetdetail.ListCostSubjectDetails.FirstOrDefault(p => sourseCost.ResourceTypeGUID == p.ResourceTypeGUID
                        && sourseCost.CostAccountSubjectGUID == p.CostAccountSubjectGUID
                        && sourseCost.Name == p.Name
                        && sourseCost.DiagramNumber == p.DiagramNumber);
                }

                //如果存在则更新,否则新增
                if (targetCost == null)
                {
                    targetCost = sourseCost.CloneByRate(targetdetail.TheGWBS, targetdetail, rate);
                    targetdetail.ListCostSubjectDetails.Add(targetCost);
                }
                ////源targetdetail中也存在,则更新,
                //else
                //{
                //    UpdateDetailCostSubject(targetCost, sourseCost, rate);
                //}
            }
            //如果targetdetail中存在,源soursedetail中也存在,则更新,源soursedetail中不存在则删除
            foreach (GWBSDetailCostSubject targetCost in targetdetail.ListCostSubjectDetails)
            {
                GWBSDetailCostSubject sourseCost = null;
                //由于前期未加字段ForwardCostSubjectId,故保留原有的匹配方式
                if (!string.IsNullOrEmpty(targetCost.ForwardCostSubjectId))
                {
                    sourseCost = soursedetail.ListCostSubjectDetails.FirstOrDefault(p => p.Id == targetCost.ForwardCostSubjectId);
                }
                else
                {
                    sourseCost = soursedetail.ListCostSubjectDetails.FirstOrDefault(p => targetCost.ResourceTypeGUID == p.ResourceTypeGUID
                        && targetCost.CostAccountSubjectGUID == p.CostAccountSubjectGUID
                        && targetCost.Name == p.Name
                        && targetCost.DiagramNumber == p.DiagramNumber);
                }

                //如果存在则更新,否则删除
                if (sourseCost == null)
                {
                    ilstRemoveCostSub.Add(targetCost);
                }
                else
                {
                    UpdateDetailCostSubject(targetCost, sourseCost, rate);
                }
            }

            foreach (var item in ilstRemoveCostSub)
            {
                var obj = targetdetail.ListCostSubjectDetails.FirstOrDefault(p => p.Id == item.Id);
                if (obj != null)
                {
                    targetdetail.ListCostSubjectDetails.Remove(item);
                }
            }
        }

        private void UpdateDetailCostSubject(GWBSDetailCostSubject targetCost, GWBSDetailCostSubject sourseCost, decimal rate)
        {
            targetCost.ContractProjectAmount = sourseCost.ContractProjectAmount * rate;
            targetCost.ContractTotalPrice = sourseCost.ContractTotalPrice * rate; ;
            targetCost.PlanTotalPrice = sourseCost.PlanTotalPrice * rate;
            targetCost.PlanWorkAmount = sourseCost.PlanWorkAmount * rate;
            targetCost.ResponsibilitilyTotalPrice = sourseCost.ResponsibilitilyTotalPrice * rate;
            targetCost.ResponsibilitilyWorkAmount = sourseCost.ResponsibilitilyWorkAmount * rate;

            targetCost.ResponsibleBasePrice = sourseCost.ResponsibleBasePrice;
            targetCost.PlanBasePrice = sourseCost.PlanBasePrice;
            targetCost.ContractBasePrice = sourseCost.ContractBasePrice;
            targetCost.ContractQuantityPrice = sourseCost.ContractQuantityPrice;
            targetCost.PlanWorkPrice = sourseCost.PlanWorkPrice;
            targetCost.ResponsibleWorkPrice = sourseCost.ResponsibleWorkPrice;
            targetCost.ContractPricePercent = sourseCost.ContractPricePercent;
            targetCost.ResponsiblePricePercent = sourseCost.ResponsiblePricePercent;
            targetCost.PlanPricePercent = sourseCost.PlanPricePercent;
            targetCost.PlanQuotaNum = sourseCost.PlanQuotaNum;
            targetCost.ResponsibleQuotaNum = sourseCost.ResponsibleQuotaNum;
            targetCost.ContractQuotaQuantity = sourseCost.ContractQuotaQuantity;
            targetCost.ResponsibilitilyPrice = sourseCost.ResponsibilitilyPrice;
            targetCost.PlanPrice = sourseCost.PlanPrice;
            targetCost.ContractPrice = sourseCost.ContractPrice;
            targetCost.ResourceTypeQuality = sourseCost.ResourceTypeQuality;
            targetCost.Name = sourseCost.Name; ;
            targetCost.ProjectAmountUnitGUID = sourseCost.ProjectAmountUnitGUID;
            targetCost.ProjectAmountUnitName = sourseCost.ProjectAmountUnitName;
            targetCost.PriceUnitGUID = sourseCost.PriceUnitGUID;
            targetCost.PriceUnitName = sourseCost.PriceUnitName;
            targetCost.AssessmentRate = sourseCost.AssessmentRate;
            targetCost.ResourceTypeGUID = sourseCost.ResourceTypeGUID;
            targetCost.ResourceTypeName = sourseCost.ResourceTypeName;
            targetCost.CostAccountSubjectGUID = sourseCost.CostAccountSubjectGUID;
            targetCost.CostAccountSubjectName = sourseCost.CostAccountSubjectName;
            targetCost.State = sourseCost.State; ;
            targetCost.MainResTypeFlag = sourseCost.MainResTypeFlag;
            targetCost.IsCategoryResource = sourseCost.IsCategoryResource;
            targetCost.ResourceTypeSpec = sourseCost.ResourceTypeSpec;
            targetCost.TechnicalParam = sourseCost.TechnicalParam;
            targetCost.ResourceTypeCode = sourseCost.ResourceTypeCode;
            targetCost.ResourceCateSyscode = sourseCost.ResourceCateSyscode;
            targetCost.CostAccountSubjectSyscode = sourseCost.CostAccountSubjectSyscode;
            targetCost.DiagramNumber = sourseCost.DiagramNumber;//图号 
            targetCost.ProfessionalType = sourseCost.ProfessionalType;

            targetCost.ListGWBSDtlCostSubRate.Clear();
            foreach (var item in sourseCost.ListGWBSDtlCostSubRate)
            {
                targetCost.ListGWBSDtlCostSubRate.Add(item.Clone(sourseCost));
            }

            #region 不用重新复制的字段
            //targetCost.ProjectAmountWasta = sourseCost.ProjectAmountWasta * rate;// 0;
            //targetCost.CurrentPeriodAccountCost = sourseCost.CurrentPeriodAccountCost * rate;// 0;
            //targetCost.CurrentPeriodAccountProjectAmount = sourseCost.CurrentPeriodAccountProjectAmount * rate;// 0;
            //targetCost.CurrentPeriodBalanceProjectAmount = sourseCost.CurrentPeriodBalanceProjectAmount * rate;// 0;
            //targetCost.CurrentPeriodBalanceTotalPrice = sourseCost.CurrentPeriodBalanceTotalPrice * rate;// 0;
            //targetCost.createtime　	     =   sourseCost.createtime　	                                ;
            //targetCost.theprojectguid      =   sourseCost.theprojectguid                                 ;
            //targetCost.theprojectname      =   sourseCost.theprojectname                                 ;
            //targetCost.gwbsdetailid        =   sourseCost.gwbsdetailid       
            //targetCost.temp1 = sourseCost.temp1;
            //targetCost.costitemquotaid = sourseCost.costitemquotaid;
            //targetCost.UpdateTime = sourseCost.updatetime; 
            //targetCost.addupaccountprojectamount =   sourseCost.addupaccountprojectamount 累计核算工程量       ;
            //targetCost.addupaccountcost =   sourseCost.addupaccountcost 累计核算成本                  ;
            //targetCost.addupaccountcostendtime =  sourseCost.addupaccountcostendtime 累计核算成本截止日期   ;
            //targetCost.currperiodacctprojectamount =  sourseCost.currperiodacctprojectamount 当期核算工程量     ;
            //targetCost.currentperiodaccountcost=  sourseCost.currentperiodaccountcost 当期核算成本          ;
            //targetCost.currperiodacctcostendtime=  sourseCost.currperiodacctcostendtime当期核算成本截止时间  ;
            //targetCost.projectamountwasta=   sourseCost.projectamountwasta 工程量损耗                  ;
            //targetCost.addupbalanceprojectamount=   sourseCost.addupbalanceprojectamount 累计结算工程量       ;
            //targetCost.currperiodbalprojectamount =   sourseCost.currperiodbalprojectamount 当期结算工程量      ;
            //targetCost.currperiodbaltotalprice=   sourseCost.currperiodbaltotalprice	 当期结算合价       ;
            //targetCost.addupbalancetotalprice	=   sourseCost.addupbalancetotalprice	 累计结算合价 
            //targetCost.ForwardCostSubjectId = sourseCost.ForwardCostSubjectId;
            //targetCost.thegwbstree=   sourseCost.thegwbstree　所属工程项目任务                    ;
            //targetCost.thegwbstreename =   sourseCost.thegwbstreename　所属工程项目任务名称            ;
            //targetCost.thegwbstreesyscode =   sourseCost.thegwbstreesyscode　所属工程项目任务层次码  
            #endregion
        }

        private decimal GetProgressPercent(decimal accQuantity, decimal planQuantity)
        {
            return planQuantity == 0 ? 0 : Math.Round(accQuantity / planQuantity, 4) * 100;
        }
        #endregion
    }
}
