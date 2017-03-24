﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using VirtualMachine.Core;
using NHibernate;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using NHibernate.Criterion;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.Runtime.Remoting.Messaging;
using System.Data;
using CommonSearchLib.BillCodeMng.Service;
using VirtualMachine.Core.DataAccess;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Data.SqlClient;
using System.Data.OracleClient;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
    /// <summary>
    /// 生产管理服务
    /// </summary>
    public class ProductionManagementSrv : BaseService, IProductionManagementSrv
    {
        #region Code生成方法
        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }
        private ISpecialCostSrv _proValSrv;

        public ISpecialCostSrv ProValSrv
        {
            get { return _proValSrv; }
            set { _proValSrv = value; }
        }


        private string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }
        #endregion

        #region 进度计划方法
        /// <summary>
        /// 查询进度计划
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetSchedules(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddOrder(Order.Asc("Id"));
            return Dao.ObjectQuery(typeof(ProductionScheduleMaster), oq);
        }

        /// <summary>
        /// 根据ID查询进度计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductionScheduleMaster GetSchedulesById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetSchedules(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ProductionScheduleMaster;
            }
            return null;
        }

        [TransManager]
        public ProductionScheduleMaster NewSchedule(ProductionScheduleMaster master)
        {
            master = SaveOrUpdateByDao(master) as ProductionScheduleMaster;

            ProductionScheduleDetail detail = new ProductionScheduleDetail();
            detail.Level = 1;
            detail.Master = master;
            detail.State = EnumScheduleDetailState.有效;
            detail.GWBSNodeType = VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode;
            detail = SaveOrUpdateByDao(detail) as ProductionScheduleDetail;

            detail.SysCode = detail.Id + ".";
            detail.OrderNo = 0;
            detail.ScheduleUnit = "天";

            master.AddDetail(detail);

            return SaveOrUpdateByDao(master) as ProductionScheduleMaster;
        }

        /// <summary>
        /// 根据进度计划类型查询进度计划
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public ProductionScheduleMaster GetSchedulesByType(ObjectQuery oq)
        {
            IList list = GetSchedules(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ProductionScheduleMaster;
            }
            return null;
        }

        public IList GetChilds(ProductionScheduleMaster master)
        {
            ArrayList list = new ArrayList();

            if (master == null)
                return list;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));

            IEnumerable<ProductionScheduleDetail> listDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

            var queryDtl = from d in listDtl
                           where d.ParentNode == null && d.Level == 1
                           orderby d.Level, d.OrderNo ascending
                           select d;


            list.AddRange(queryDtl.ToArray());

            getChildDtl(queryDtl.ElementAt(0), listDtl, ref list);

            return list;
        }

        private void getChildDtl(ProductionScheduleDetail parentDtl, IEnumerable<ProductionScheduleDetail> listDtl, ref ArrayList listResult)
        {
            var queryDtl = from d in listDtl
                           where d.ParentNode == parentDtl
                           orderby d.OrderNo ascending
                           orderby d.Level ascending
                           select d;
            parentDtl.ChildCount = queryDtl.Count();

            foreach (ProductionScheduleDetail dtl in queryDtl)
            {
                listResult.Add(dtl);

                getChildDtl(dtl, listDtl, ref listResult);
            }
        }

        public IList GetChildsOrdered(ProductionScheduleDetail parentNode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode", parentNode));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            return dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
        }

        public List<ProductionScheduleDetail> aa(IList list)
        {
            List<ProductionScheduleDetail> retList = new List<ProductionScheduleDetail>();
            foreach (ProductionScheduleDetail detail in list)
            {
                retList.Add(detail);
                IList tempList = GetChildsOrdered(detail);
                List<ProductionScheduleDetail> tempRetList = aa(tempList);
                retList.AddRange(tempRetList);
            }
            return retList;
        }

        /// <summary>
        /// 保存进度计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public ProductionScheduleMaster SaveSchedule(ProductionScheduleMaster obj)
        {
            ProductionScheduleMaster master = SaveOrUpdateByDao(obj) as ProductionScheduleMaster;
            return master;
        }

        /// <summary>
        /// 保存进度计划
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        [TransManager]
        public ProductionScheduleMaster SaveSchedule(ProductionScheduleMaster obj, IList detailList)
        {
            foreach (ProductionScheduleDetail detail in detailList)
            {
                obj.AddDetail(detail);
            }
            ProductionScheduleMaster master = SaveOrUpdateByDao(obj) as ProductionScheduleMaster;
            return master;
        }

        /// <summary>
        /// 查找树节点的最后一级子节点
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        private IList GetLastLevelNode(TreeNode treeNode)
        {
            ArrayList list = new ArrayList();
            if (treeNode.Nodes.Count == 0)
            {
                list.Add(treeNode);
            }
            else
            {
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    IList tempList = GetLastLevelNode(tn);
                    if (tempList != null && tempList.Count > 0)
                    {
                        list.AddRange(tempList);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 查询节点的子节点数
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public int CountChildNodes(ProductionScheduleDetail parentNode)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode", parentNode));
            oq.AddCriterion(Expression.Not(Expression.Eq("State", EnumScheduleDetailState.失效)));
            return Dao.Count(typeof(ProductionScheduleDetail), oq);
        }

        /// <summary>
        /// 查询节点下的所有子节点数
        /// </summary>
        /// <param name="parentNode">ProductionScheduleDetail</param>
        /// <returns></returns>
        public int CountAllChildNodes(string parentNodeId)
        {
            int result = 0;

            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("ParentNode.Id", parentNodeId));
            //oq.AddCriterion(Expression.Not(Expression.Eq("State", EnumScheduleDetailState.失效)));
            //IList list = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
            //if (list != null && list.Count > 0)
            //{
            //    result = list.Count;
            //    foreach (ProductionScheduleDetail detail in list)
            //    {
            //        result += CountAllChildNodes(detail.Id);
            //    }
            //}

            result = GetAllChilds(parentNodeId).Count;

            return result;
        }

        /// <summary>
        /// 查询节点的子节点数
        /// </summary>
        /// <param name="parentNode">ProductionScheduleDetail</param>
        /// <returns></returns>
        public int CountChildNodes(string parentNodeId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ParentNode.Id", parentNodeId));
            oq.AddCriterion(Expression.Not(Expression.Eq("State", EnumScheduleDetailState.失效)));
            return Dao.Count(typeof(ProductionScheduleDetail), oq);
        }

        /// <summary>
        /// 删除一条进度计划明细 并删除其下的所有子节点
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="errMsg">异常信息</param>
        public IList DeleteScheduleDetail(ProductionScheduleDetail detail, int childCount, string errMsg)
        {
            childCount = 0;
            errMsg = "";

            IList list = new ArrayList();
            list.Add(errMsg);
            list.Add(childCount);

            //IList lst = new ArrayList();

            if (detail == null || string.IsNullOrEmpty(detail.Id))
            {
                errMsg = "要删除的计划节点为空,请指定要删除的计划节点.";
                list[0] = errMsg;
                return list;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode", detail.SysCode, MatchMode.Start));
            //oq.AddCriterion(Expression.Eq("State",EnumScheduleDetailState.有效));

            IList listdtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);

            foreach (ProductionScheduleDetail d in listdtl)
            {
                if (d.State == EnumScheduleDetailState.有效 || d.State == EnumScheduleDetailState.失效)
                {
                    errMsg = "要删除的计划节点或下属计划节点存在“有效”或“失效”状态的值,当前只能删除“编辑”状态的计划节点.";
                    list[0] = errMsg;
                    return list;
                }
            }

            childCount = listdtl.Count - 1;

            //lst.Add(childCount);
            list[1] = childCount;

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbTransaction tr = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            try
            {
                command.Transaction = tr;

                //if (conn is SqlConnection)
                //{
                //    command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=@detailId";
                //}
                //else
                //{
                //    command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=:detailId";
                //}
                //IDbDataParameter p_detailId = command.CreateParameter();
                //p_detailId.Value = detail.Id;
                //p_detailId.ParameterName = "detailId";
                //command.Parameters.Add(p_detailId);
                //command.ExecuteNonQuery();

                if (listdtl != null && listdtl.Count > 0)
                {
                    foreach (ProductionScheduleDetail tempDetail in listdtl)
                    {
                        command.Parameters.Clear();
                        if (conn is SqlConnection)
                        {
                            command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=@Id";
                        }
                        else
                        {
                            command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=:Id";
                        }
                        IDbDataParameter p_Id = command.CreateParameter();
                        p_Id.Value = tempDetail.Id;
                        p_Id.ParameterName = "Id";
                        command.Parameters.Add(p_Id);
                        command.ExecuteNonQuery();
                    }
                }
                tr.Commit();
                return list;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 发布一条进度计划明细 并发布其下的所有子节点
        /// </summary>
        /// <param name="detailId"></param>
        public void PublishScheduleDetail(string detailId)
        {
            UpdateScheduleState(detailId, 11);
        }

        private void UpdateScheduleState(string detailId, int state)
        {
            IList list = GetAllChilds(detailId);

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbTransaction tr = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            try
            {
                command.Transaction = tr;
                if (conn is SqlConnection)
                {
                    command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=@state WHERE Id=@detailId";
                }
                else
                {
                    command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=:state WHERE Id=:detailId";
                }
                IDbDataParameter p_state = command.CreateParameter();
                p_state.ParameterName = "state";
                p_state.Value = state;
                command.Parameters.Add(p_state);

                IDbDataParameter p_detailId = command.CreateParameter();
                p_detailId.Value = detailId;
                p_detailId.ParameterName = "detailId";
                command.Parameters.Add(p_detailId);
                command.ExecuteNonQuery();
                if (list != null && list.Count > 0)
                {
                    foreach (ProductionScheduleDetail tempDetail in list)
                    {
                        command.Parameters.Clear();
                        if (conn is SqlConnection)
                        {
                            command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=@state WHERE Id=@Id";
                        }
                        else
                        {
                            command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=:state WHERE Id=:Id";
                        }
                        IDbDataParameter p_tempState = command.CreateParameter();
                        p_tempState.Value = state;
                        p_tempState.ParameterName = "state";
                        command.Parameters.Add(p_tempState);
                        IDbDataParameter p_tempId = command.CreateParameter();
                        p_tempId.Value = tempDetail.Id;
                        p_tempId.ParameterName = "Id";
                        command.Parameters.Add(p_tempId);
                        command.ExecuteNonQuery();
                    }
                }
                tr.Commit();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 查询节点下的所有子节点
        /// </summary>
        /// <param name="parentNodeId">父计划明细ID</param>
        /// <returns></returns>
        public IList GetAllChilds(string parentNodeId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", parentNodeId));
            IList list = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);

            if (list != null && list.Count > 0)
            {
                ProductionScheduleDetail dtl = list[0] as ProductionScheduleDetail;

                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Eq("Master.ScheduleType", dtl.Master.ScheduleType));
                oq.AddCriterion(Expression.Like("SysCode", dtl.SysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Not(Expression.Eq("SysCode", dtl.SysCode)));

                list = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);

            }

            return list;
        }

        public IList GetAllChildsId(string parentNodeId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            ArrayList result = new ArrayList();
            command.CommandText = string.Format("SELECT Id FROM THD_ProductionScheduleDetail tpsd WHERE parentNode='{0}'", parentNodeId);
            IDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                string detailId = dr.GetString(0);
                IList tempList = GetAllChildsId(detailId);
                if (tempList != null && tempList.Count > 0)
                {
                    result.AddRange(tempList);
                }
            }
            return result;
        }

        /// <summary>
        /// 滚动计划引用
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetProductionScheduleDetailForQuery(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
        }

        /// <summary>
        /// 保存滚动进度计划明细（生成时调用）
        /// </summary>
        /// <param name="listPlanDtl"></param>
        /// <returns></returns>
        [TransManager]
        public ProductionScheduleMaster SaveScrollPlanDtl(IList listPlanDtl)
        {
            if (listPlanDtl == null || listPlanDtl.Count == 0)
                return null;

            dao.Save(listPlanDtl);

            var query11 = from d in listPlanDtl.OfType<ProductionScheduleDetail>()
                          where d.Id == "3_6RIOZa9Egf5ikMOzQ1Pa"
                          select d;

            var query12 = from d in listPlanDtl.OfType<ProductionScheduleDetail>()
                          where d.ParentNode.Id == "3_6RIOZa9Egf5ikMOzQ1Pa"
                          select d;

            //设置明细的Syscode
            IEnumerable<ProductionScheduleDetail> queryPlanDtl = listPlanDtl.OfType<ProductionScheduleDetail>();

            int level = queryPlanDtl.Min(d => d.Level);

            queryPlanDtl = from d in queryPlanDtl
                           where d.Level == level
                           select d;

            ProductionScheduleDetail dtl = queryPlanDtl.ElementAt(0) as ProductionScheduleDetail;

            foreach (ProductionScheduleDetail d in listPlanDtl)
            {
                if (d.ParentNode.Id == dtl.ParentNode.Id)
                {
                    d.SysCode = dtl.ParentNode.SysCode + d.Id + ".";

                    SetPlanDtlSysCode(d, listPlanDtl);
                }
            }

            dao.Update(listPlanDtl);

            //生成syscode
            //UpdateScrollPlanDtlSyscode(dtl.Master.Id);


            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", dtl.Master.Id));
            oq.AddFetchMode("Details", FetchMode.Eager);

            ProductionScheduleMaster master = dao.ObjectQuery(typeof(ProductionScheduleMaster), oq)[0] as ProductionScheduleMaster;

            return master;

        }

        private void SetPlanDtlSysCode(ProductionScheduleDetail parentDtl, IList listPlanDtl)
        {
            foreach (ProductionScheduleDetail d in listPlanDtl)
            {
                if (d.ParentNode.Id == parentDtl.Id)
                {
                    d.SysCode = parentDtl.SysCode + d.Id + ".";

                    SetPlanDtlSysCode(d, listPlanDtl);
                }
            }
        }

        public void UpdateScrollPlanDtlSyscode(string masterId)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();

            session.Transaction.Enlist(command);

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "UP_UpdateScrollPlanDtlSyscode";

            OracleParameter param1 = new OracleParameter("masterId", masterId);
            param1.Direction = ParameterDirection.Input;
            command.Parameters.Add(param1);

            command.ExecuteNonQuery();

        }

        #endregion

        #region 周进度计划方法

        [TransManager]
        public void SaveWeekPlanDtl(IList listPlanDtl, IList list_Del)
        {
            if (list_Del != null && list_Del.Count > 0)
                dao.Delete(list_Del);
            SaveWeekPlanDtl(listPlanDtl);
        }

        [TransManager]
        public void SaveWeekPlanDtl(IList listPlanDtl)
        {
            if (listPlanDtl == null || listPlanDtl.Count == 0)
                return;

            dao.SaveOrUpdate(listPlanDtl);

            //设置明细的Syscode
            IEnumerable<WeekScheduleDetail> queryPlanDtl = listPlanDtl.OfType<WeekScheduleDetail>();
            int level = queryPlanDtl.Min(d => d.Level);
            queryPlanDtl = from d in queryPlanDtl
                           where d.Level == level
                           select d;

            //保存根节点的 SysCode
            WeekScheduleDetail dtl = queryPlanDtl.ElementAt(0);
            dtl.SysCode = dtl.Id + ".";
            dao.Update(dtl);

            //维护枝叶节点的SysCode
            SetWeekPlanDtlSysCode(dtl, listPlanDtl);

            dao.Update(listPlanDtl);
        }

        [TransManager]
        public void SaveWeekPlanDtl(IList listPlanDtl, IList list_Add, IList list_Del)
        {
            if (list_Del != null && list_Del.Count > 0)
                dao.Delete(list_Del);
            SaveWeekPlanDtl(listPlanDtl,list_Add,true);
        }

        [TransManager]
        public void SaveWeekPlanDtl(IList listPlanDtl,IList list_Add ,bool isflag )
        {
            if (list_Add == null || list_Add.Count == 0)
                return;

            dao.SaveOrUpdate(list_Add);

            //设置明细的Syscode
            IEnumerable<WeekScheduleDetail> queryPlanDtl = listPlanDtl.OfType<WeekScheduleDetail>();
            int level = queryPlanDtl.Min(d => d.Level);
            queryPlanDtl = from d in queryPlanDtl
                           where d.Level == level
                           select d;

            //保存根节点的 SysCode
            WeekScheduleDetail dtl = queryPlanDtl.ElementAt(0);
            dtl.SysCode = dtl.Id + ".";
            dao.Update(dtl);

            //维护枝叶节点的SysCode
            SetWeekPlanDtlSysCode(dtl, listPlanDtl);

            dao.Update(listPlanDtl);

            //var master = GetWeekScheduleMasterById(dtl.Master.Id);
            //if (master == null)
            //{
            //    return master;
            //}

            //var defaultDate = new DateTime(1900, 1, 1);
            //var isChanged = false;
            //var validPlanDtl = queryPlanDtl.TakeWhile(a => a.PlannedBeginDate != defaultDate);
            //if (validPlanDtl.Count() > 0)
            //{
            //    var newBeginDate = validPlanDtl.Min(b => b.PlannedBeginDate);
            //    if (master.PlannedBeginDate == defaultDate || newBeginDate < master.PlannedBeginDate)
            //    {
            //        master.PlannedBeginDate = newBeginDate;
            //        isChanged = true;
            //    }
            //}
            //validPlanDtl = queryPlanDtl.TakeWhile(a => a.PlannedEndDate != defaultDate);
            //if (validPlanDtl.Count() > 0)
            //{
            //    var newEndDate = validPlanDtl.Max(b => b.PlannedEndDate);
            //    if (master.PlannedEndDate == defaultDate || newEndDate > master.PlannedEndDate)
            //    {
            //        master.PlannedEndDate = newEndDate;
            //        isChanged = true;
            //    }
            //}

            //if (isChanged)
            //{
            //    dao.Update(master);
            //}

            //return master;
        }

        [TransManager]
        public void SaveUpdateWeekPlanDtl(IList listPlanDtl)
        {
            if (listPlanDtl == null || listPlanDtl.Count == 0)
                return;

            dao.SaveOrUpdate(listPlanDtl);
        }

        [TransManager]
        public IList SaveUpdateWeekPlanDtl(IList listPlanDtl, IList listPlanDtlRalation)
        {
            if (listPlanDtl == null || listPlanDtl.Count == 0)
                return listPlanDtl;
            if (listPlanDtlRalation != null && listPlanDtlRalation.Count > 0)
                DeleteByIList(listPlanDtlRalation);

             dao.SaveOrUpdate(listPlanDtl);
            return listPlanDtl;
        }

        private void SetWeekPlanDtlSysCode(WeekScheduleDetail parentDtl, IList listPlanDtl)
        {
            foreach (WeekScheduleDetail d in listPlanDtl)
            {
                if (d.ParentNode != null && d.ParentNode.Id == parentDtl.Id)
                {
                    d.SysCode = parentDtl.SysCode + d.Id + ".";

                    SetWeekPlanDtlSysCode(d, listPlanDtl);
                }
            }
        }

        [TransManager]
        public WeekScheduleMaster NewWeekSchedule(WeekScheduleMaster master)
        {
            if (string.IsNullOrEmpty(master.Id))
            {
                master.Code = GetCode(typeof(WeekScheduleMaster), master.ProjectId);
            }
            master = SaveOrUpdateByDao(master) as WeekScheduleMaster;

            //GWBSTree GWBSTree_root= null;
            //IList list = GetGWBSTreesRoot(master.ProjectId);
            //if (list != null && list.Count > 0)
            //    GWBSTree_root = list[0] as GWBSTree;


            //WeekScheduleDetail detail = new WeekScheduleDetail();
            //detail.GWBSTree = GWBSTree_root;
            //if (detail.GWBSTree != null)
            //{
            //    detail.GWBSTreeName = detail.GWBSTree.Name;
            //    detail.GWBSTreeSysCode = detail.GWBSTree.SysCode;
            //    detail.NodeType = detail.GWBSTree.CategoryNodeType;
            //    detail.OrderNo = (int)detail.GWBSTree.OrderNo;
            //}
            //detail.Level = 1;
            //detail.Master = master;
            //detail.State = DocumentState.InExecute;
            //detail.NodeType = VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode;
            //detail.ProjectId = master.ProjectId;
            //detail.ProjectName = master.ProjectName;
            //detail.GWBSTreeSysCode = string.Empty;
            //detail = SaveOrUpdateByDao(detail) as WeekScheduleDetail;

            //detail.SysCode = detail.Id + ".";
            //detail.OrderNo = 0;
            //detail.ScheduleUnit = "天";


            //master.AddDetail(detail);

            return SaveOrUpdateByDao(master) as WeekScheduleMaster;
        }

        /// <summary>
        /// 查询周进度计划
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetWeekScheduleMaster(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("Details.Details", FetchMode.Eager);
            oq.AddFetchMode("Details.RalationDetails", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(WeekScheduleMaster), oq);
        }


        /// <summary>
        /// 查询周进度计划
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetWeekScheduleMasterOQ(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(WeekScheduleMaster), oq);
        }

        /// <summary>
        /// 查询周进度计划明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetWeekDetail(ObjectQuery oq)
        {
            oq.AddFetchMode("Master", FetchMode.Eager);
            oq.AddFetchMode("GWBSTree", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(WeekScheduleDetail), oq);
        }

        /// <summary>
        /// 根据ID查询周进度计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WeekScheduleMaster GetWeekScheduleMasterById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetWeekScheduleMaster(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as WeekScheduleMaster;
            }
            return null;
        }

        /// <summary>
        /// 根据Code查询周进度计划
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WeekScheduleMaster GetWeekScheduleMasterByCode(ObjectQuery oq)
        {
            IList list = GetWeekScheduleMaster(oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as WeekScheduleMaster;
            }
            return null;
        }

        /// <summary>
        /// 保存周进度计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public WeekScheduleMaster SaveWeekScheduleMaster(WeekScheduleMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(WeekScheduleMaster), obj.ProjectId);
            }

            SaveOrUpdateByDao(obj);

            #region chenf 2016-09-28 经确认暂时屏蔽生成计划产值
            //if (obj.DocState == DocumentState.InExecute && (obj.ExecScheduleType == EnumExecScheduleType.月度进度计划 || obj.ExecScheduleType == EnumExecScheduleType.季度进度计划))
            //{
            //    CurrentProjectInfo projectInfo = dao.Get(typeof(CurrentProjectInfo), obj.ProjectId) as CurrentProjectInfo;
            //    ProValSrv.PlanedOutputValueAccount(obj, projectInfo, obj.HandleOrg);
            //}
            #endregion
            return obj;
        }

        /// <summary>
        /// 执行进度计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet WeekScheduleQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT t1.Code,t2.GWBSTreeName,t2.PBSTreeName,t1.PlannedBeginDate,t1.PlannedEndDate,t2.PlannedDuration,
                t2.PlannedWrokload,t2.ActualBeginDate,t2.ActualEndDate,t2.ActualDuration,t2.TaskCompletedPercent,
                t2.MainTaskContent,t2.CompletionAnalysis,t2.PlanConformity,t2.TaskCheckState,t1.State
                FROM THD_WeekScheduleMaster t1 INNER JOIN THD_WeekScheduleDetail t2 ON t1.id=t2.parentid";
            sql += " where 1=1 " + condition + " order by t1.Code";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        /// <summary>
        /// 删除 汇总周计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteSummaryWeekScheduleMaster(WeekScheduleMaster obj)
        {
            List<string> detailIdList = new List<string>();
            foreach (WeekScheduleDetail detial in obj.Details)
            {
                if (!detailIdList.Contains(detial.ForwardBillDtlId))
                {
                    detailIdList.Add(detial.ForwardBillDtlId);
                }
            }
            UpdateWeekScheduleSummaryStatus((int)EnumSummaryStatus.未汇总, detailIdList);
            return DeleteByDao(obj);
        }

        /// <summary>
        /// 保存 汇总周计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public WeekScheduleMaster SaveSummaryWeekScheduleMaster(WeekScheduleMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(WeekScheduleMaster), obj.ProjectId);
            }

            WeekScheduleMaster ret = SaveOrUpdateByDao(obj) as WeekScheduleMaster;

            List<string> detailIdList = new List<string>();
            foreach (WeekScheduleDetail detial in ret.Details)
            {
                if (!detailIdList.Contains(detial.ForwardBillDtlId))
                {
                    detailIdList.Add(detial.ForwardBillDtlId);
                }
            }
            UpdateWeekScheduleSummaryStatus((int)EnumSummaryStatus.己汇总, detailIdList);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="summaryStatus"></param>
        /// <param name="detailIdList"></param>
        [TransManager]
        private void UpdateWeekScheduleSummaryStatus(int summaryStatus, List<string> detailIdList)
        {
            if (detailIdList == null || detailIdList.Count == 0) return;

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            string idCon = "('0'";
            foreach (string id in detailIdList)
            {
                idCon += ",'" + id + "'";
            }
            idCon += ")";
            string sql = "update thd_weekschedulemaster set SummaryStatus=" + summaryStatus +
                " where id in (select distinct parentid from thd_weekscheduledetail where id in " + idCon + " )";
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 查询执行进度计划明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetWeekScheduleDetail(ObjectQuery oq)
        {
            oq.AddFetchMode("GWBSTree", FetchMode.Eager);
            oq.AddFetchMode("GWBSTree.ListRelaPBS", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(WeekScheduleDetail), oq);
        }

        /// <summary>
        /// 根据总进度计划复制一个新的总滚动进度计划
        /// </summary>
        /// <param name="targetPlan">源计划（要复制的计划）</param>
        /// <returns></returns>
        [TransManager]
        public WeekScheduleMaster CopyNewSchdulePlan(WeekScheduleMaster targetPlan)
        {
            WeekScheduleMaster newPlan = new WeekScheduleMaster();
            newPlan.ProjectId = targetPlan.ProjectId;
            newPlan.ProjectName = targetPlan.ProjectName;
            newPlan.CreateDate = DateTime.Now;
            newPlan.Code = GetCode(typeof(WeekScheduleMaster), newPlan.ProjectId);
            newPlan.PlannedBeginDate = targetPlan.PlannedBeginDate;
            newPlan.PlannedEndDate = targetPlan.PlannedEndDate;

            Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
            newPlan.HandlePerson = login.ThePerson;
            newPlan.HandlePersonName = login.ThePerson.Name;
            newPlan.CreatePerson = login.ThePerson;
            newPlan.CreatePersonName = login.ThePerson.Name;
            newPlan.HandleOrg = login.TheOperationOrgInfo;
            newPlan.HandlePersonSyscode = login.TheOperationOrgInfo.SysCode;
            newPlan.DocState = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
            newPlan.ExecScheduleType = targetPlan.ExecScheduleType;
            newPlan.PlanName = targetPlan.PlanName;

            var queryDtl = from d in targetPlan.Details
                           where d.ParentNode == null
                           select d;

            WeekScheduleDetail dtl = queryDtl.ElementAt(0);

            WeekScheduleDetail dtlNew = CopyToNewDetail(dtl);
            dtlNew.Master = newPlan;
            dtlNew.ParentNode = null;
            dtlNew.Level = 1;
            dtlNew.State = DocumentState.InExecute;
            dtlNew.OrderNo = 0;

            newPlan.Details.Add(dtlNew);
            newPlan = SaveOrUpdateByDao(newPlan) as WeekScheduleMaster;

            dtlNew.SysCode = dtlNew.Id + ".";
            dtlNew = SaveOrUpdateByDao(dtlNew) as WeekScheduleDetail;

            CopyNewSchdulePlanDetail(dtl, targetPlan, dtlNew, newPlan);

            return newPlan;
        }

        private void CopyNewSchdulePlanDetail(WeekScheduleDetail targetParentDtl, WeekScheduleMaster targetPlan, WeekScheduleDetail newParentDtl, WeekScheduleMaster newPlan)
        {
            var query = from d in targetPlan.Details
                        orderby d.OrderNo ascending
                        where d.ParentNode != null && d.ParentNode.Id == targetParentDtl.Id
                        select d;

            foreach (WeekScheduleDetail dtl in query)
            {
                WeekScheduleDetail dtlNew = CopyToNewDetail(dtl);
                dtlNew.Master = newPlan;
                dtlNew.ParentNode = newParentDtl;
                dtlNew.Level = newParentDtl.Level + 1;
                dtlNew.State = DocumentState.Edit;
                dao.Save(dtlNew);

                dtlNew.SysCode = newParentDtl.SysCode + dtlNew.Id + ".";
                dao.Update(dtlNew);

                newPlan.Details.Add(dtlNew);

                CopyNewSchdulePlanDetail(dtl, targetPlan, dtlNew, newPlan);
            }
        }

        /// <summary>
        /// 保存子计划
        /// </summary>
        /// <param name="newPlan">新计划</param>
        /// <param name="selDetails">筛选的明细</param>
        /// <returns></returns>
        [TransManager]
        public WeekScheduleMaster CreateSubSchdulePlan(WeekScheduleMaster newPlan, IList selDetails)
        {
            if (newPlan == null || selDetails == null || selDetails.Count == 0)
            {
                return null;
            }

            newPlan.Code = GetCode(typeof(WeekScheduleMaster), newPlan.ProjectId);
            newPlan = SaveOrUpdateByDao(newPlan) as WeekScheduleMaster;

            var rootNode = selDetails.OfType<WeekScheduleDetail>().First(a => a.ParentNode == null);
            var newRootNode = CopyToNewDetail(rootNode);
            newRootNode.Master = newPlan;
            newRootNode.Level = 1;
            dao.Save(newRootNode);

            newRootNode.SysCode = newRootNode.Id + ".";
            dao.Update(newRootNode);

            newPlan.Details.Add(newRootNode);

            AddNewDetails(rootNode, selDetails, newRootNode, newPlan);

            CreateWeekScheduleTask(newPlan);

            return newPlan;
        }

        private void CreateWeekScheduleTask(WeekScheduleMaster plan)
        {
            if (plan == null || plan.ExecScheduleType != EnumExecScheduleType.周进度计划)
            {
                return;
            }

            var dts = from dt in plan.Details
                      where dt.NodeType == NodeType.LeafNode
                      select dt;

            foreach (var dt in dts)
            {
                var nodeDetails = GetGwbsTreeDetails(dt.GWBSTree);
                if (nodeDetails == null)
                {
                    continue;
                }

                foreach (GWBSDetail nDetail in nodeDetails)
                {
                    var wst = new WeekScheduleTask();
                    wst.Master = dt;
                    wst.CreateTime = DateTime.Now;
                    wst.GwbsName = dt.GWBSTreeName;
                    wst.GwbsTree = dt.GWBSTree;
                    wst.PlanBeginDate = dt.PlannedBeginDate;
                    wst.PlanEndDate = dt.PlannedEndDate;
                    wst.PlanTime = dt.PlannedDuration;
                    wst.ProjectId = plan.ProjectId;
                    wst.ProjectName = plan.ProjectName;
                    wst.RealBeginDate = wst.PlanBeginDate;
                    wst.RealEndDate = wst.PlanEndDate;
                    wst.SubContractProject = nDetail.ContractProject;
                    wst.SubContractProjectName = nDetail.ContractProjectName;
                    wst.Task = nDetail;
                    wst.TaskName = nDetail.Name;

                    dt.Details.Add(wst);
                }

                Dao.SaveOrUpdate(dt);
            }
        }

        private IList GetGwbsTreeDetails(GWBSTree treeNode)
        {
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("TheGWBS", treeNode));
            objQuery.AddCriterion(Expression.Eq("State", DocumentState.InExecute));

            return Dao.ObjectQuery(typeof(GWBSDetail), objQuery);
        }

        private WeekScheduleDetail CopyToNewDetail(WeekScheduleDetail dtl)
        {
            var dtlNew = new WeekScheduleDetail();
            dtlNew.GWBSTree = dtl.GWBSTree;
            dtlNew.GWBSTreeName = dtl.GWBSTreeName;
            dtlNew.GWBSTreeSysCode = dtl.GWBSTreeSysCode;
            dtlNew.NodeType = dtl.NodeType;
            dtlNew.OrderNo = dtl.OrderNo;
            dtlNew.State = dtl.State;
            dtlNew.PlannedBeginDate = dtl.PlannedBeginDate;
            dtlNew.PlannedEndDate = dtl.PlannedEndDate;
            dtlNew.PlannedDuration = dtl.PlannedDuration;
            dtlNew.ScheduleUnit = dtl.ScheduleUnit;
            dtlNew.Descript = dtl.Descript;
            dtlNew.ForwardBillDtlId = dtl.Id;

            return dtlNew;
        }

        private void AddNewDetails(WeekScheduleDetail targetParentDtl, IList selDetails, WeekScheduleDetail newParentDtl, WeekScheduleMaster newPlan)
        {
            var query = from d in selDetails.OfType<WeekScheduleDetail>()
                        orderby d.OrderNo ascending
                        where d.ParentNode != null && d.ParentNode.Id == targetParentDtl.Id
                        select d;

            foreach (WeekScheduleDetail dtl in query)
            {
                WeekScheduleDetail dtlNew = CopyToNewDetail(dtl);
                dtlNew.Master = newPlan;
                dtlNew.ParentNode = newParentDtl;
                dtlNew.Level = newParentDtl.Level + 1;
                dao.Save(dtlNew);

                dtlNew.SysCode = newParentDtl.SysCode + dtlNew.Id + ".";
                dao.Update(dtlNew);

                newPlan.Details.Add(dtlNew);

                AddNewDetails(dtl, selDetails, dtlNew, newPlan);
            }
        }

        [TransManager]
        public int DeleteWeekPlan(WeekScheduleMaster weekPlan)
        {
            if (weekPlan == null || weekPlan.ExecScheduleType != EnumExecScheduleType.周进度计划)
            {
                return -1;
            }

            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("WeekSchedule", weekPlan.Id));
            var assList = GetAssignWorkerOrderMasterByOQ(objQuery).OfType<AssignWorkerOrderMaster>();
            if (assList.Any(a => a.PrintCount > 0))
            {
                return -2;
            }

            if (Dao.Delete(weekPlan))
            {
                foreach (var orderMaster in assList)
                {
                    Dao.Delete(orderMaster);
                }
            }

            return 1;
        }

        #endregion

        #region 派工单

        public IList GetAssignWorkerOrderMasterByOQ(ObjectQuery oq, bool isNeedShowDetail)
        {
            if (isNeedShowDetail)
                oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(AssignWorkerOrderMaster), oq);
        }

        [TransManager]
        public int CreateAssignWorkerOrderByPlan(WeekScheduleMaster weekPlan)
        {
            if (weekPlan == null || weekPlan.ExecScheduleType != EnumExecScheduleType.周进度计划)
            {
                return -1;
            }

            weekPlan = GetWeekScheduleMasterById(weekPlan.Id);
            if (weekPlan == null)
            {
                return -1;
            }

            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("WeekSchedule", weekPlan.Id));
            var assBills = GetAssignWorkerOrderMasterByOQ(objQuery);
            if (assBills != null && assBills.Count > 0)
            {
                return -2;
            }

            Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation");
            var taskList = new List<WeekScheduleTask>();
            foreach (var detail in weekPlan.Details)
            {
                if (detail.Details != null)
                {
                    taskList.AddRange(detail.Details);
                }
            }

            var teams = from tsk in taskList
                        group tsk by new { tsk.SubContractProject, tsk.SubContractProjectName }
                            into gps
                            select new
                                       {
                                           gps.Key.SubContractProject,
                                           gps.Key.SubContractProjectName
                                       };

            foreach (var tm in teams)
            {
                var bill = new AssignWorkerOrderMaster();
                bill.CreateDate = DateTime.Now;
                bill.CreatePerson = login.ThePerson;
                bill.CreatePersonName = login.ThePerson.Name;
                bill.ProjectId = weekPlan.ProjectId;
                bill.ProjectName = weekPlan.ProjectName;
                bill.WeekSchedule = weekPlan.Id;
                bill.WeekScheduleName = weekPlan.PlanName;
                bill.Code = GetCode(typeof(AssignWorkerOrderMaster), weekPlan.ProjectId);
                bill.OrgSysCode = login.TheOperationOrgInfo.SysCode;

                if (tm.SubContractProject != null)
                {
                    bill.AssignTeam = tm.SubContractProject.Id;
                    bill.AssignTeamName = tm.SubContractProjectName;
                }

                var tasks = taskList.FindAll(t => t.SubContractProject == tm.SubContractProject);
                foreach (var task in tasks)
                {
                    var billDetail = new AssignWorkerOrderDetail();
                    billDetail.Master = bill;
                    //billDetail.ActualBenginDate = task.RealBeginDate;
                    //billDetail.ActualEndDate = task.RealEndDate;
                    billDetail.GWBSDetail = task.Task;
                    billDetail.GWBSDetailName = task.TaskName;
                    billDetail.GWBSTree = task.GwbsTree;
                    billDetail.GWBSTreeName = task.GwbsName;
                    billDetail.PlanBeginDate = task.PlanBeginDate;
                    billDetail.PlanEndDate = task.PlanEndDate;
                    billDetail.PlanWorkDays = task.PlanTime;

                    bill.Details.Add(billDetail);
                }

                SaveAssignWorkerOrderMaster(bill);
            }

            return 1;
        }

        public IList GetAssignWorkerOrderMasterByOQ(ObjectQuery oq)
        {

            oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(AssignWorkerOrderMaster), oq);
        }

        public AssignWorkerOrderMaster SaveAssignWorkerOrderMaster(AssignWorkerOrderMaster obj)
        {
            bool flag = dao.SaveOrUpdate(obj);
            return flag ? obj : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isMaintainWSDActualDate">是否反写进度计划明细的时间开始时间</param>
        /// <returns></returns>
        [TransManager]
        public AssignWorkerOrderMaster SaveAssignWorkerOrderMaster(AssignWorkerOrderMaster obj, bool isMaintainWSDActualDate)
        {
            if (isMaintainWSDActualDate)
            {
                List<WeekScheduleDetail>  list_WSD = new List<WeekScheduleDetail>();
                Hashtable hash = new Hashtable();
                DateTime defaultDate = new DateTime(1900,1,1);
                foreach (var item in obj.Details)
                {
                    if (!hash.Contains(item.GWBSTree.Id))
                    {
                        hash.Add(item.GWBSTree.Id, item.ActualBenginDate);
                    }
                    else 
                    {
                        DateTime hash_dt =ClientUtil.ToDateTime(hash[item.GWBSTree.Id]);
                        if (hash_dt != defaultDate && item.ActualBenginDate > defaultDate)
                            hash[item.GWBSTree.Id] = item.ActualBenginDate < hash_dt ? item.ActualBenginDate : hash_dt;
                    }
                        
                }

                foreach (string  key in hash.Keys)
                {
                    WeekScheduleDetail wsd = GetWSDByGWBSTreeId(key,obj.ProjectId);
                    if (wsd == null || (wsd.ActualBeginDate != null && wsd.ActualBeginDate > new DateTime(1900,1,1) && wsd.ActualBeginDate <= ClientUtil.ToDateTime(hash[key])))
                        continue;
                    wsd.ActualBeginDate = ClientUtil.ToDateTime(hash[key]);
                    list_WSD.Add(wsd);
                }

                bool isSaveWsd = dao.Save(list_WSD);
                if (!isSaveWsd)
                    return null;
            }
            

            bool flag = dao.SaveOrUpdate(obj);
            return flag ? obj : null;
        }

        private WeekScheduleDetail GetWSDByGWBSTreeId(string id, string strProjectid)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("GWBSTree.Id",id));
            oq.AddCriterion(Expression.Eq("ProjectId", strProjectid));
            IList lst = Dao.ObjectQuery(typeof(WeekScheduleDetail), oq);
            if (lst != null && lst.Count > 0)
                return lst[0] as WeekScheduleDetail;
            else
                return null;
        }

        public AssignWorkerOrderMaster GetAssignWorkerOrderMasterById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetAssignWorkerOrderMasterByOQ(oq, true);
            if (list != null && list.Count > 0)
            {
                return list[0] as AssignWorkerOrderMaster;
            }
            return null;
        }

        #endregion

        #region 工程任务确认单方法
        /// <summary>
        /// 查询工程任务确认单
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetGWBSTaskConfirmMaster(ObjectQuery oq)
        {
            oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(GWBSTaskConfirmMaster), oq);
        }

        /// <summary>
        /// 根据Id查询工程任务确认单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GWBSTaskConfirmMaster GetGWBSTaskConfirmMasterById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = GetGWBSTaskConfirmMaster(oq);
            if (list != null && list.Count > 0) return list[0] as GWBSTaskConfirmMaster;
            return null;
        }

        /// <summary>
        /// 根据Code查询工程任务确认单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public GWBSTaskConfirmMaster GetGWBSTaskConfirmMasterByCode(ObjectQuery oq)
        {
            IList list = GetGWBSTaskConfirmMaster(oq);
            if (list != null && list.Count > 0) return list[0] as GWBSTaskConfirmMaster;
            return null;
        }

        /// <summary>
        /// 保存工程任务确认单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public GWBSTaskConfirmMaster SaveGWBSTaskConfirmMaster(GWBSTaskConfirmMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(GWBSTaskConfirmMaster), obj.ProjectId);
            }

            if (obj.DocState == VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Valid)
            {
                UpdateWeekScheduleDetail(obj, 0);

                obj.ConfirmDate = DateTime.Now;

                UpdateGWBSDetail(obj, 0);
            }

            GWBSTaskConfirmMaster ret = SaveOrUpdateByDao(obj) as GWBSTaskConfirmMaster;



            return ret;
        }

        /// <summary>
        /// 保存工程任务确认单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public GWBSTaskConfirmMaster SaveGWBSTaskConfirmMaster2(GWBSTaskConfirmMaster obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(GWBSTaskConfirmMaster), obj.ProjectId);
            }

            UpdateGWBSDetail(obj, 0);

            GWBSTaskConfirmMaster ret = SaveOrUpdateByDao(obj) as GWBSTaskConfirmMaster;

            return ret;
        }

        /// <summary>
        /// 保存工程任务确认单明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public GWBSTaskConfirm SaveGWBSTaskConfirm(GWBSTaskConfirm confirmDtl)
        {
            GWBSDetail dtl = dao.Get(typeof(GWBSDetail), confirmDtl.GWBSDetail.Id) as GWBSDetail;
            if (dtl != null)
            {

                dtl.QuantityConfirmed -= ClientUtil.ToDecimal(confirmDtl.TempData);
                dtl.QuantityConfirmed += confirmDtl.ActualCompletedQuantity;
                if (dtl.PlanWorkAmount != 0)
                {
                    dtl.ProgressConfirmed = dtl.QuantityConfirmed / dtl.PlanWorkAmount * 100;
                }
                dao.Update(dtl);
            }
            confirmDtl.RealOperationDate = DateTime.Now;
            GWBSTaskConfirm ret = SaveOrUpdateByDao(confirmDtl) as GWBSTaskConfirm;
            return ret;
        }
        [TransManager]
        public IList SaveGWBSTaskConfirm(IList lstConfirmDtl)
        {

            IList lstGWBSDetail = null;
            ObjectQuery oQuery = new ObjectQuery();
            GWBSTaskConfirm oConfirm = null, oTempConfirm = null;
            StringBuilder oBuilder = new StringBuilder();
            string[] arrIDs;
            if (lstConfirmDtl != null && lstConfirmDtl.Count > 0)
            {
                #region 将累计形象进度汇总到 工程WBS明细上
                oQuery = new ObjectQuery();
                arrIDs = lstConfirmDtl.OfType<GWBSTaskConfirm>().Select(a => a.GWBSDetail.Id).ToArray<string>();
                oQuery.AddCriterion(Expression.In("Id", arrIDs));
                lstGWBSDetail = Dao.ObjectQuery(typeof(GWBSDetail), oQuery);
                if (lstGWBSDetail != null && lstGWBSDetail.Count > 0)
                {
                    foreach (GWBSDetail oGWBSDetail in lstGWBSDetail)
                    {
                        var var = lstConfirmDtl.OfType<GWBSTaskConfirm>().Where(a => a.GWBSDetail.Id == oGWBSDetail.Id);
                        if (var != null && var.Count() > 0)
                        {
                            oConfirm = var.ElementAt(0);
                            oGWBSDetail.QuantityConfirmed -= ClientUtil.ToDecimal(oConfirm.TempData);
                            oGWBSDetail.QuantityConfirmed += oConfirm.ActualCompletedQuantity;
                            //oConfirm.GWBSDetail = oGWBSDetail;
                            if (oGWBSDetail.PlanWorkAmount != 0)
                            {
                                oGWBSDetail.ProgressConfirmed = oGWBSDetail.QuantityConfirmed / oGWBSDetail.PlanWorkAmount * 100;
                            }
                        }
                    }
                    Dao.SaveOrUpdate(lstGWBSDetail);//回写工程WBS明细累计确认量和累计形象进度
                }
                #endregion
                Dao.SaveOrUpdate(lstConfirmDtl);
            }
            return lstConfirmDtl;
        }
        /// <summary>
        ///批量填报
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        [TransManager]
        public IList SaveGWBSTaskConfirm1(IList lst)
        {
            IEnumerable<GWBSTaskConfirm> _lstConfirmDtl;
            IEnumerable<GWBSDetail> _lstGWBSDetail;
            IEnumerable<OBSService> _lstOBSService;
            StringBuilder oBuilder = new StringBuilder();
            IList lstGWBSDetail = null;
            IList lstOBSService = null;
            ObjectQuery oQuery;
            GWBSTaskConfirm oConfirm = null, oTempConfirm = null;
            GWBSDetail oGWBSDetail = null;
            OBSService oOBSService = null;
            string[] arrIDs;
            IList lstConfirmSave = new ArrayList();
            if (lst != null && lst.Count > 0)
            {
                _lstConfirmDtl = lst.OfType<GWBSTaskConfirm>();

                #region 获取工程确认明细集合 进行数据更新
                arrIDs = _lstConfirmDtl.Where(a => !string.IsNullOrEmpty(a.Id)).Select(a => a.Id).Distinct().ToArray();
                if (arrIDs != null && arrIDs.Count() > 0)
                {
                    oQuery = new ObjectQuery();
                    oQuery.AddCriterion(Expression.In("Id", arrIDs));
                    oQuery.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.虚拟工单));//【状态】=9.虚拟工程任务确认单
                    oQuery.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
                    //oQuery.AddFetchMode("TaskHandler", NHibernate.FetchMode.Eager);
                    lstConfirmSave = Dao.ObjectQuery(typeof(GWBSTaskConfirm), oQuery);//查找已经存在的工程确认单
                    if (lstConfirmSave != null)
                    {
                        foreach (GWBSTaskConfirm oTemp in lstConfirmSave)
                        {
                            oConfirm = _lstConfirmDtl.Where(a => a.Id == oTemp.Id).FirstOrDefault();
                            #region 验证
                            if (oTemp.AccountingState == EnumGWBSTaskConfirmAccountingState.己核算)
                            {
                                oBuilder.AppendFormat("\n[{0}]此提报明细正处于商务核算过程中！", oTemp.GWBSDetailName);
                                continue;
                            }
                            if (oTemp.QuantityBeforeConfirm != oConfirm.QuantityBeforeConfirm)
                            {
                                oBuilder.AppendFormat("\n[{0}]提报明细商务已核算，请重新刷新提报！", oTemp.GWBSDetailName);
                                continue;
                            }
                            if (oBuilder.Length > 0)
                            {
                                continue;
                            }
                            #endregion
                            oTemp.TempData = oConfirm.TempData;
                            oTemp.CompletedPercent = oConfirm.CompletedPercent;
                            oTemp.ActualCompletedQuantity = oConfirm.ActualCompletedQuantity;
                            oTemp.QuantiyAfterConfirm = oConfirm.QuantiyAfterConfirm;
                            oTemp.ProgressAfterConfirm = oConfirm.ProgressAfterConfirm;
                            oTemp.Descript = oConfirm.Descript;
                            oTemp.RealOperationDate = oConfirm.RealOperationDate;
                        }
                    }
                }
                if (oBuilder.Length != 0) throw new Exception(oBuilder.ToString());
                #endregion
                #region 新建工程确认单
                _lstConfirmDtl = _lstConfirmDtl.Where(a => string.IsNullOrEmpty(a.Id) && !string.IsNullOrEmpty(a.TempData3));
                arrIDs = _lstConfirmDtl.Select(a => a.GWBSDetail.Id).Distinct().ToArray();
                if (arrIDs != null && arrIDs.Count() > 0)//获取新建工程任务确认明细对应的工程任务明细
                {
                    #region 获取新建工程任务确认明细对应的工程任务明细集合
                    oQuery = new ObjectQuery();
                    oQuery.AddCriterion(Expression.In("Id", arrIDs));
                    oQuery.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
                    oQuery.AddCriterion(Expression.Gt("PlanWorkAmount", 0M));
                    oQuery.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                    oQuery.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                    oQuery.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
                    lstGWBSDetail = dao.ObjectQuery(typeof(GWBSDetail), oQuery);
                    #endregion
                    #region 获取新建工程任务确认明细对应的服务obs
                    oQuery = new ObjectQuery();
                    arrIDs = _lstConfirmDtl.Select(a => a.TempData3).ToArray();
                    if (arrIDs != null && arrIDs.Count() > 0)
                    {
                        oQuery.AddCriterion(Expression.In("Id", arrIDs.ToArray()));
                        oQuery.AddFetchMode("SupplierId", NHibernate.FetchMode.Eager);
                        oQuery.AddFetchMode("SupplierId.TheContractGroup", NHibernate.FetchMode.Eager);
                        lstOBSService = dao.ObjectQuery(typeof(OBSService), oQuery);
                    }
                    #endregion
                    if (lstGWBSDetail != null && lstGWBSDetail.Count > 0 && lstOBSService != null && lstOBSService.Count > 0)
                    {
                        _lstGWBSDetail = lstGWBSDetail.OfType<GWBSDetail>();
                        _lstOBSService = lstOBSService.OfType<OBSService>();
                        #region 新建工程任务确认明细与 工程任务明细 承办者进行关联
                        foreach (GWBSTaskConfirm oTemp in _lstConfirmDtl)
                        {
                            oGWBSDetail = _lstGWBSDetail.Where(a => string.Equals(a.Id, oTemp.GWBSDetail.Id)).FirstOrDefault();
                            oOBSService = _lstOBSService.Where(a => string.Equals(a.Id, oTemp.TempData3 as string)).FirstOrDefault();
                            if (oGWBSDetail != null && oOBSService != null)
                            {
                                lstConfirmSave.Add(oTemp);
                                oTemp.TaskHandler = oOBSService.SupplierId;
                                oTemp.TaskHandlerName = oOBSService.SupplierName;
                                oTemp.GWBSDetail = oGWBSDetail;
                                oTemp.GWBSDetailName = oGWBSDetail.Name;
                                oTemp.GWBSTree = oGWBSDetail.TheGWBS;//
                                oTemp.GWBSTreeName = oGWBSDetail.TheGWBS.Name;
                                oTemp.GwbsSysCode = oGWBSDetail.TheGWBS.SysCode;
                                oTemp.WorkAmountUnitId = oGWBSDetail.WorkAmountUnitGUID;//
                                oTemp.WorkAmountUnitName = oGWBSDetail.PriceUnitName;
                                oTemp.CostItem = oGWBSDetail.TheCostItem;
                                oTemp.CostItemName = oGWBSDetail.TheCostItem.Name;
                                oTemp.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                                oTemp.RealOperationDate = DateTime.Now;
                                oGWBSDetail.GWBSTaskConfirmDetail = oTemp;
                            }
                            else
                            {
                                oBuilder.Length = 0;
                                if (oGWBSDetail == null) oBuilder.AppendFormat("工程任务明细为[{0}]上的任务量提报未找到对应的工程任务明细[{1}],请刷新工程wbs树", oTemp.GWBSDetailName, oTemp.GWBSDetailName);
                                if (oOBSService == null) oBuilder.AppendFormat("工程任务明细为[{0}]上的任务量提报未找到对应的服务OBS,请刷新工程wbs树", oTemp.GWBSDetailName);
                                throw new Exception(oBuilder.ToString());
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 异常分析
                        if (lstGWBSDetail == null || lstGWBSDetail.Count == 0)
                        {
                            throw new Exception("请刷新工程任务树,再进行任务量提报");
                        }
                        else
                        {
                            throw new Exception(string.Format("[{0}]工程任务明细的工程任务提报未找到对应服务OBS", string.Join("、", _lstConfirmDtl.Select(a => a.GWBSDetailName).ToArray())));
                        }
                        #endregion
                    }
                }
                #endregion
                #region 将累计形象进度汇总到 工程WBS明细上
                lstGWBSDetail = lstConfirmSave.OfType<GWBSTaskConfirm>().Select(a => a.GWBSDetail).ToList();
                if (lstGWBSDetail != null && lstGWBSDetail.Count > 0)
                {
                    foreach (GWBSDetail oTemp in lstGWBSDetail)
                    {
                        var var = lstConfirmSave.OfType<GWBSTaskConfirm>().Where(a => a.GWBSDetail.Id == oTemp.Id);
                        if (var != null && var.Count() > 0)
                        {
                            oConfirm = var.ElementAt(0);
                            oTemp.QuantityConfirmed -= ClientUtil.ToDecimal(oConfirm.TempData);
                            oTemp.QuantityConfirmed += oConfirm.ActualCompletedQuantity;
                            //oConfirm.GWBSDetail = oGWBSDetail;
                            if (oTemp.PlanWorkAmount != 0)
                            {
                                oTemp.ProgressConfirmed = oTemp.QuantityConfirmed / oTemp.PlanWorkAmount * 100;
                            }
                        }
                    }
                    Dao.SaveOrUpdate(lstGWBSDetail);//回写工程WBS明细累计确认量和累计形象进度
                }
                #endregion
                Dao.SaveOrUpdate(lstConfirmSave);
            }
            return lstConfirmSave;
        }
        /// <summary>
        /// 现场生产提报方法
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        [TransManager]
        public IList SaveGWBSTaskConfirmNow(IList lst)
        {
            IEnumerable<GWBSTaskConfirm> _lstConfirmDtl;
            IEnumerable<GWBSDetail> _lstGWBSDetail;
            IEnumerable<OBSService> _lstOBSService;
            string sErrMsg = string.Empty;
            StringBuilder oBuilder = new StringBuilder();
            IList lstGWBSDetail = null;
            IList lstOBSService = null;
            ObjectQuery oQuery;
            GWBSTaskConfirm oConfirm = null, oTempConfirm = null;
            GWBSTree oGWBSTree = null;
            GWBSDetail oGWBSDetail = null;
            OBSService oOBSService = null;
            string[] arrIDs;
            IList lstConfirmSave = new ArrayList();
            if (lst != null && lst.Count > 0)
            {
                _lstConfirmDtl = lst.OfType<GWBSTaskConfirm>();

                #region 获取工程确认明细集合 进行数据更新
                arrIDs = _lstConfirmDtl.Where(a => !string.IsNullOrEmpty(a.Id)).Select(a => a.Id).Distinct().ToArray();
                if (arrIDs != null && arrIDs.Count() > 0)
                {
                    oQuery = new ObjectQuery();
                    oQuery.AddCriterion(Expression.In("Id", arrIDs));
                    oQuery.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.虚拟工单));//【状态】=9.虚拟工程任务确认单
                    oQuery.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
                    //oQuery.AddFetchMode("TaskHandler", NHibernate.FetchMode.Eager);
                    lstConfirmSave = Dao.ObjectQuery(typeof(GWBSTaskConfirm), oQuery);//查找已经存在的工程确认单
                    if (lstConfirmSave != null)
                    {
                        foreach (GWBSTaskConfirm oTemp in lstConfirmSave)
                        {
                            oConfirm = _lstConfirmDtl.Where(a => a.Id == oTemp.Id).FirstOrDefault();
                            #region 验证
                            if (oTemp.AccountingState == EnumGWBSTaskConfirmAccountingState.己核算)
                            {
                                oBuilder.AppendFormat("\n[0]此提报明细正处于商务核算过程中！", oTemp.GWBSDetailName);
                                continue;
                            }
                            if (oTemp.QuantityBeforeConfirm != oConfirm.QuantityBeforeConfirm)
                            {
                                oBuilder.AppendFormat("\n[0]提报明细商务已核算，请重新刷新提报！", oTemp.GWBSDetailName);
                                continue;
                            }
                            if (oBuilder.Length > 0)
                            {
                                continue;
                            }
                            #endregion
                            oTemp.TempData = oConfirm.TempData;
                            oTemp.CompletedPercent = oConfirm.CompletedPercent;
                            oTemp.ActualCompletedQuantity = oConfirm.ActualCompletedQuantity;
                            oTemp.QuantiyAfterConfirm = oConfirm.QuantiyAfterConfirm;
                            oTemp.ProgressAfterConfirm = oConfirm.ProgressAfterConfirm;
                            oTemp.Descript = oConfirm.Descript;
                        }
                    }
                }
                if (oBuilder.Length != 0) throw new Exception(oBuilder.ToString());
                #endregion
                #region 新建工程确认单
                _lstConfirmDtl = _lstConfirmDtl.Where(a => string.IsNullOrEmpty(a.Id) && !string.IsNullOrEmpty(a.TempData3));
                arrIDs = _lstConfirmDtl.Select(a => a.GWBSDetail.Id).Distinct().ToArray();
                if (arrIDs != null && arrIDs.Count() > 0)//获取新建工程任务确认明细对应的工程任务明细
                {
                    #region 获取新建工程任务确认明细对应的工程任务明细集合
                    oQuery = new ObjectQuery();
                    oQuery.AddCriterion(Expression.In("Id", arrIDs));
                    oQuery.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
                    oQuery.AddCriterion(Expression.Gt("PlanWorkAmount", 0M));
                    oQuery.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                    oQuery.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                    oQuery.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
                    lstGWBSDetail = dao.ObjectQuery(typeof(GWBSDetail), oQuery);
                    #endregion
                    #region 获取新建工程任务确认明细对应的服务obs
                    oQuery = new ObjectQuery();
                    arrIDs = _lstConfirmDtl.Select(a => a.TempData3).ToArray();
                    if (arrIDs != null && arrIDs.Count() > 0)
                    {
                        oQuery.AddCriterion(Expression.In("Id", arrIDs.ToArray()));
                        oQuery.AddFetchMode("SupplierId", NHibernate.FetchMode.Eager);
                        oQuery.AddFetchMode("SupplierId.TheContractGroup", NHibernate.FetchMode.Eager);
                        lstOBSService = dao.ObjectQuery(typeof(OBSService), oQuery);
                    }
                    #endregion
                    #region 获取新建工程任务确认明细对应的工程wbs树  因为在现场做提报时  提报的工程wbs树与提报工程任务明细不一定存在父子关系 并且实际操作中只有一个工程wbs树节点
                    oGWBSTree = _lstConfirmDtl.FirstOrDefault().GWBSTree;
                    oGWBSTree = dao.Get(typeof(GWBSTree), oGWBSTree.Id) as GWBSTree;
                    #endregion
                    #region 新建工程任务确认明细与工程任务树、工程任务明细 承办者进行关联
                    if (lstGWBSDetail != null && lstGWBSDetail.Count > 0 && lstOBSService != null && lstOBSService.Count > 0 && oGWBSTree != null)
                    {
                        _lstGWBSDetail = lstGWBSDetail.OfType<GWBSDetail>();
                        _lstOBSService = lstOBSService.OfType<OBSService>();
                        foreach (GWBSTaskConfirm oTemp in _lstConfirmDtl)
                        {
                            // oGWBSTree = _lstGWBSTree.Where(a => string.Equals(a.Id, oTemp.GWBSTree.Id)).FirstOrDefault();
                            oGWBSDetail = _lstGWBSDetail.Where(a => string.Equals(a.Id, oTemp.GWBSDetail.Id)).FirstOrDefault();
                            oOBSService = _lstOBSService.Where(a => string.Equals(a.Id, oTemp.TempData3 as string)).FirstOrDefault();
                            if (oGWBSDetail != null && oOBSService != null && oGWBSTree != null)
                            {
                                lstConfirmSave.Add(oTemp);
                                oTemp.TaskHandler = oOBSService.SupplierId;
                                oTemp.TaskHandlerName = oOBSService.SupplierName;
                                oTemp.GWBSDetail = oGWBSDetail;
                                oTemp.GWBSDetailName = oGWBSDetail.Name;
                                oTemp.GWBSTree = oGWBSTree;//
                                oTemp.GWBSTreeName = oGWBSTree.Name;
                                oTemp.GwbsSysCode = oGWBSTree.SysCode;
                                oTemp.WorkAmountUnitId = oGWBSDetail.WorkAmountUnitGUID;//
                                oTemp.WorkAmountUnitName = oGWBSDetail.PriceUnitName;
                                oTemp.CostItem = oGWBSDetail.TheCostItem;
                                oTemp.CostItemName = oGWBSDetail.TheCostItem.Name;
                                oTemp.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                                oGWBSDetail.GWBSTaskConfirmDetail = oTemp;
                            }
                            else
                            {
                                oBuilder.Length = 0;
                                if (oGWBSTree == null) oBuilder.AppendFormat("工程任务明细[{0}]上的任务量提报未找到对应的工程WBS树[{1}],请刷新工程wbs树", oTemp.GWBSDetailName, oTemp.GWBSTreeName);
                                if (oGWBSDetail == null) oBuilder.AppendFormat("工程任务明细为[{0}]上的任务量提报未找到对应的工程任务明细[{1}],请刷新工程wbs树", oTemp.GWBSDetailName, oTemp.GWBSDetailName);
                                if (oOBSService == null) oBuilder.AppendFormat("工程任务明细为[{0}]上的任务量提报未找到对应的服务OBS,请刷新工程wbs树", oTemp.GWBSDetailName);
                                throw new Exception(oBuilder.ToString());
                            }
                        }
                    }
                    else
                    {
                        #region 异常分析
                        if (oGWBSTree == null || lstGWBSDetail == null || lstGWBSDetail.Count == 0)
                        {
                            throw new Exception("请刷新工程任务树,再进行任务量提报");
                        }
                        else
                        {
                            throw new Exception(string.Format("[{0}]工程任务明细的工程任务提报未找到对应服务OBS", string.Join("、", _lstConfirmDtl.Select(a => a.GWBSDetailName).ToArray())));
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion


                #region 将累计形象进度汇总到 工程WBS明细上
                lstGWBSDetail = lstConfirmSave.OfType<GWBSTaskConfirm>().Select(a => a.GWBSDetail).ToList();
                if (lstGWBSDetail != null && lstGWBSDetail.Count > 0)
                {
                    foreach (GWBSDetail oTemp in lstGWBSDetail)
                    {
                        var var = lstConfirmSave.OfType<GWBSTaskConfirm>().Where(a => a.GWBSDetail.Id == oTemp.Id);
                        if (var != null && var.Count() > 0)
                        {
                            oConfirm = var.ElementAt(0);
                            oTemp.QuantityConfirmed -= ClientUtil.ToDecimal(oConfirm.TempData);
                            oTemp.QuantityConfirmed += oConfirm.ActualCompletedQuantity;
                            //oConfirm.GWBSDetail = oGWBSDetail;
                            if (oTemp.PlanWorkAmount != 0)
                            {
                                oTemp.ProgressConfirmed = oTemp.QuantityConfirmed / oTemp.PlanWorkAmount * 100;
                            }
                        }
                    }
                    Dao.SaveOrUpdate(lstGWBSDetail);//回写工程WBS明细累计确认量和累计形象进度
                }
                #endregion
                Dao.SaveOrUpdate(lstConfirmSave);
            }
            return lstConfirmSave;
        }
        [TransManager]
        public void BackSchedule(IList lstConfirm)
        {
            if (lstConfirm != null && lstConfirm.Count > 0)
            {
                GWBSTaskConfirm oGWBSTaskConfirm = null;
                IList listPro = new ArrayList();
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.In("GWBSTree.Id", lstConfirm.OfType<GWBSTaskConfirm>().Select(a => a.GWBSTree.Id).ToArray()));
                oq.AddFetchMode("GWBSTree", FetchMode.Eager);
                IList list = Dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                if (list != null && list.Count > 0)
                {
                    foreach (ProductionScheduleDetail oProductionScheduleDetail in list)
                    {
                        var var = lstConfirm.OfType<GWBSTaskConfirm>().Where(a => a.GWBSTree.Id == oProductionScheduleDetail.GWBSTree.Id);
                        if (var != null && var.Count() > 0)
                        {
                            oGWBSTaskConfirm = var.ElementAt<GWBSTaskConfirm>(0);
                            if (oProductionScheduleDetail.GWBSTree.Id == oGWBSTaskConfirm.GWBSTree.Id)
                            {
                                oProductionScheduleDetail.ActualBeginDate = oGWBSTaskConfirm.RealOperationDate;
                                if (oGWBSTaskConfirm.GWBSTree.AddUpFigureProgress > 101)
                                {
                                    oProductionScheduleDetail.ActualEndDate = ClientUtil.ToDateTime(oGWBSTaskConfirm.GWBSTree.RealEndDate);
                                    oProductionScheduleDetail.ActualDuration = (oProductionScheduleDetail.ActualEndDate - oProductionScheduleDetail.ActualBeginDate).Days;
                                }
                                listPro.Add(oProductionScheduleDetail);
                            }
                        }
                    }
                    if (listPro.Count > 0)
                    {
                        SaveOrUpdate(listPro);
                    }
                }
            }
        }
        /// <summary>
        /// 保存工程任务明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public GWBSDetail SaveGWBSDetail(GWBSDetail obj)
        {
            GWBSDetail ret = SaveOrUpdateByDao(obj) as GWBSDetail;
            return ret;
        }

        /// <summary>
        /// 查询工程任务明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public IList SearchGWBSDetail(string strSysCode)
        {
            IList gwbsList = new ArrayList();
            //通过层次码查询非生产节点下的任务明细
            ObjectQuery oq = new ObjectQuery();
            //TheGWBSSysCode明细所属GWBS节点层次码
            oq.AddCriterion(Expression.Like("TheGWBSSysCode", strSysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);
            oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));
            IEnumerable<GWBSDetail> dtlList = ObjectQuery(typeof(GWBSDetail), oq).OfType<GWBSDetail>();
            var queryConfirmSum = from p in dtlList
                                  group p by new
                                  {
                                      costItem = p.TheCostItem,
                                      resourceTypeId = p.MainResourceTypeId,
                                      resourceTypeName = p.MainResourceTypeName,
                                      resourceTypeSpe = p.MainResourceTypeSpec,
                                      diagramNumber = p.DiagramNumber
                                  }
                                      into g
                                      select new
                                      {
                                          g.Key.costItem,
                                          g.Key.resourceTypeId,
                                          g.Key.resourceTypeName,
                                          g.Key.resourceTypeSpe,
                                          g.Key.diagramNumber,
                                          planWorkAmount = g.Sum(o => o.PlanWorkAmount),
                                          quantityConfirmed = g.Sum(o => o.QuantityConfirmed),
                                          count = g.Count()
                                      };
            foreach (var obj in queryConfirmSum)
            {
                var query = from g in dtlList
                            where g.TheCostItem != null && g.TheCostItem.Id == obj.costItem.Id
                            && g.MainResourceTypeId == obj.resourceTypeId
                            && g.DiagramNumber == obj.diagramNumber
                            select g;

                GWBSDetail dtl = new GWBSDetail();

                dtl.Name = query.ElementAt(0).Name;

                dtl.TheCostItem = obj.costItem;
                dtl.MainResourceTypeId = obj.resourceTypeId;
                dtl.MainResourceTypeName = obj.resourceTypeName;
                dtl.MainResourceTypeSpec = obj.resourceTypeSpe;
                dtl.DiagramNumber = obj.diagramNumber;
                dtl.PlanWorkAmount = obj.planWorkAmount;
                dtl.QuantityConfirmed = obj.quantityConfirmed;
                gwbsList.Add(dtl);
            }
            return gwbsList;
        }

        [TransManager]
        public IList SearchGWBSDetailNew(string sGWBSTreeSysCode, string sProjectID)
        {
            IList gwbsList = new ArrayList();
            string sSQL = @"select tt.*,tt1.id obsid,tt1.SupplierName from (
select t.id gwbsdetailid ,t.name  gwbsdetailname,t.MainResourceTypeName,t.MainResourceTypeSpec,t.DiagramNumber,t.State,t.WorkAmountUnitName,t.ContractGroupName,t.ContractGroupGUID,t.OrderNo,t.PlanWorkAmount,
                t1.id confirmDetialID,t1.CreatePersonName,t1.TaskHandlerName,t1.MaterialFeeSettlementFlag,t1.CompletedPercent,t1.ProgressBeforeConfirm,t1.ProgressAfterConfirm,t1.RealOperationDate,t1.Descript,t1.AccountingState,t1.QuantityBeforeConfirm,t1.QuantiyAfterConfirm,t1.ActualCompletedQuantity,
                t2.id costItemID,t2.QuotaCode 
                from thd_gwbsdetail t
                left join thd_gwbstaskconfirmdetail t1 on t.id=t1.gwbsdetail and T1.PARENTID IN (select TT.ID from thd_gwbstaskconfirmmaster tt where tt.id=t1.parentid and tt.BillType=1 AND tT.Projectid=:projectid )
                left join thd_costitem t2 on t.costitemguid=t2.id
                where t.thegwbssyscode like :SysCode||'%' and t.produceconfirmflag=1 and t.planworkamount>0 and t.state=5 and t.theprojectguid=:projectid) tt
                left join thd_obsservice tt1 on tt.confirmDetialID is null and  tt.gwbsdetailid=tt1.gwbsdetail  AND TT1.PROJECTID=:projectid";

            return gwbsList;
        }
        public DataTable GetData(string sSQL)
        {
            ISession oSession = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = oSession.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();
            command.CommandText = sSQL;
            IDataReader oRead = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
        }
        string TempBuildProjectTypeCodes = "'040102003'";//临建wbs节点类型的code
        public bool IsTempBuildProjectTypeNode(string sProjectID, string sTrreNodeSysCode)
        {
            bool bResult = false;
            string sSQL = @"select 0 from thd_gwbstree t
join  thd_projecttasktypetree t2 on t2.code in ({0}) where t.theprojectguid=:sProjectID and instr(:sTrreNodeSysCode,t.id)>0
and exists(select 1 from thd_projecttasktypetree t1 where  t.projecttasktypeguid=t1.id and instr(t1.syscode,t2.id)>0 )";
            sSQL = string.Format(sSQL, TempBuildProjectTypeCodes);
            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();
            command.CommandText = sSQL;
            command.Parameters.Add(new OracleParameter("sProjectID", sProjectID));
            command.Parameters.Add(new OracleParameter("sTrreNodeSysCode", sTrreNodeSysCode));
            IDataReader oReader = command.ExecuteReader();
            DataSet ds = DataAccessUtil.ConvertDataReadertoDataSet(oReader);
            return ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0 ? false : true;
        }
        public DataSet SearchGWBSDetail(string sGWBSTreeSyCode, string sProjectID)
        {
            IList lstResult = new ArrayList();
            string sSQL = string.Empty;
            DataTable oTable = null;
            GWBSDetail oDetail = null;
            DataSet ds = null;
            try
            {
                if (string.IsNullOrEmpty(sGWBSTreeSyCode)) throw new Exception("工程WBS树为空");
                bool IsTempBuildTypeNode = this.IsTempBuildProjectTypeNode(sProjectID, sGWBSTreeSyCode);
                #region 获取工程wbs明细
                string sParam1 = string.Empty, sParam2 = string.Empty;
                sSQL = @"{0}select tt.*,tt1.id obsid,tt1.SupplierName,tt1.MATERIALFEESETTLEMENTFLAG ISSETTLEMENT from (
select t.id gwbsdetailid ,t.name  gwbsdetailname,t.MainResourceTypeName,t.MainResourceTypeSpec,t.DiagramNumber,t.State,t.WorkAmountUnitName,t.ContractGroupName,t.ContractGroupGUID,t.OrderNo,t.PlanWorkAmount,
                t1.id confirmDetialID,t1.CreatePersonName,t1.TaskHandlerName,t1.MaterialFeeSettlementFlag,t1.CompletedPercent,t1.ProgressBeforeConfirm,t1.ProgressAfterConfirm,t1.RealOperationDate,t1.Descript,t1.AccountingState,t1.QuantityBeforeConfirm,t1.QuantiyAfterConfirm,t1.ActualCompletedQuantity,
                t2.id costItemID,t2.QuotaCode from thd_gwbsdetail t 
                inner join  thd_gwbstree tree on tree.id=t.PARENTID and tree.Categorynodetype = 2
                left join thd_gwbstaskconfirmdetail t1 on t.id=t1.gwbsdetail and T1.PARENTID IN (select TT.ID from thd_gwbstaskconfirmmaster tt where tt.id=t1.parentid and tt.BillType=1 AND tT.Projectid=:projectid )
                left join thd_costitem t2 on t.costitemguid=t2.id where t.thegwbssyscode like :SysCode||'%'  and t.planworkamount>0 and t.state=5 and t.PRODUCECONFIRMFLAG=1 and t.theprojectguid=:projectid {1}) tt
                left join thd_obsservice tt1 on tt.confirmDetialID is null and tt.gwbsdetailid=tt1.gwbsdetail  AND TT1.PROJECTID=:projectid order by tt.orderno asc ";
                //sSQL = string.Format(sSQL, sGWBSTreeSyCode, sProjectID, (int)DocumentState.InExecute, (int)EnumConfirmBillType.虚拟工单);
                if (!IsTempBuildTypeNode)
                {
                    sParam1 = @"with temp as (select tt2.id from thd_projecttasktypetree tt join thd_projecttasktypetree tt1 on instr( tt1.syscode,tt.id)>0 join thd_gwbstree tt2 on tt1.id=tt2.projecttasktypeguid and tt2.theprojectguid=:projectid where tt.code in ('040102003'))";
                    sParam2 = @"and not exists(select 1 from temp a where instr(t.thegwbssyscode,a.id )>0)";
                }
                sSQL = string.Format(sSQL, sParam1, sParam2);
                ISession oSession = CallContext.GetData("nhsession") as ISession;
                OracleConnection conn = oSession.Connection as OracleConnection;
                OracleCommand command = conn.CreateCommand();
                command.CommandText = sSQL;
                command.Parameters.Add(new OracleParameter("SysCode", sGWBSTreeSyCode));
                command.Parameters.Add(new OracleParameter("projectid", sProjectID));
                //command.Parameters.Add(new OracleParameter("projectid2", sProjectID));
                //command.Parameters.Add(new OracleParameter("projectid2", sProjectID));
                IDataReader oRead = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("工程任务明细查询失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
            return ds;
        }
        /// <summary>
        /// 主要针对现场生产 根据工程wbs树，找到该节点及父节点最近的工程任务明细
        /// </summary>
        /// <param name="sGWBSTreeSyCode"></param>
        /// <param name="sProjectID"></param>
        /// <returns></returns>
        public DataSet SearchGWBSDetailNow(string sGWBSTreeSyCode, string sProjectID)
        {
            IList lstResult = new ArrayList();
            string sSQL = string.Empty;
            DataTable oTable = null;
            GWBSDetail oDetail = null;
            DataSet ds = null;
            try
            {
                if (string.IsNullOrEmpty(sGWBSTreeSyCode)) throw new Exception("工程WBS树为空");
                #region 获取工程wbs明细

                sSQL = @"select tt.*,tt1.id obsid,tt1.SupplierName from (
select t.id gwbsdetailid ,t.name  gwbsdetailname,t.MainResourceTypeName,t.MainResourceTypeSpec,t.DiagramNumber,t.State,t.WorkAmountUnitName,t.ContractGroupName,t.ContractGroupGUID,t.OrderNo,t.PlanWorkAmount,
                t1.id confirmDetialID,t1.CreatePersonName,t1.TaskHandlerName,t1.MaterialFeeSettlementFlag,t1.CompletedPercent,t1.ProgressBeforeConfirm,t1.ProgressAfterConfirm,t1.RealOperationDate,t1.Descript,t1.AccountingState,t1.QuantityBeforeConfirm,t1.QuantiyAfterConfirm,t1.ActualCompletedQuantity,
                t2.id costItemID,t2.QuotaCode 
                from thd_gwbsdetail t
                left join thd_gwbstaskconfirmdetail t1 on t.id=t1.gwbsdetail and T1.PARENTID IN (select TT.ID from thd_gwbstaskconfirmmaster tt where tt.id=t1.parentid and tt.BillType=1 AND tT.Projectid=:projectid )
                left join thd_costitem t2 on t.costitemguid=t2.id and t2.theprojectguid=:projectid
                where t.parentid=(select id from  (select t.id,t.tlevel,t.theprojectguid from thd_gwbstree t 
                  where t.state=1 and t.theprojectguid=:projectid and  instr(:SysCode,t.id)>0
                  and exists(select 1 from thd_gwbsdetail tt where tt.parentid=t.id and tt.theprojectguid=:projectid and   tt.produceconfirmflag=1 and tt.planworkamount>0 and tt.state=5)
                  order by t.tlevel desc) where rownum=1) and t.produceconfirmflag=1 and t.planworkamount>0 and t.state=5 and t.theprojectguid=:projectid) tt
                left join thd_obsservice tt1 on tt.confirmDetialID is null and  tt.gwbsdetailid=tt1.gwbsdetail  AND TT1.PROJECTID=:projectid";
                //sSQL = string.Format(sSQL, sGWBSTreeSyCode, sProjectID, (int)DocumentState.InExecute, (int)EnumConfirmBillType.虚拟工单);
                ISession oSession = CallContext.GetData("nhsession") as ISession;
                OracleConnection conn = oSession.Connection as OracleConnection;
                OracleCommand command = conn.CreateCommand();
                command.CommandText = sSQL;
                command.Parameters.Add(new OracleParameter("SysCode", sGWBSTreeSyCode));
                command.Parameters.Add(new OracleParameter("projectid", sProjectID));
                //command.Parameters.Add(new OracleParameter("projectid2", sProjectID));
                //command.Parameters.Add(new OracleParameter("projectid2", sProjectID));
                IDataReader oRead = command.ExecuteReader();
                ds = DataAccessUtil.ConvertDataReadertoDataSet(oRead);
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("工程任务明细查询失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
            return ds;
        }
        [TransManager]
        public IList SearchGWBSDetail(GWBSTree oGWBSTree, PersonInfo oLoginPersonInfo, OperationOrgInfo oTheOperationOrgInfo)
        {
            IList lstResult = null;
            if (oGWBSTree != null)
            {
                ObjectQuery oq = new ObjectQuery();
                #region 查询工程wbs明细
                //TheGWBSSysCode明细所属GWBS节点层次码
                oq.AddCriterion(Expression.Like("TheGWBSSysCode", oGWBSTree.SysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
                oq.AddCriterion(Expression.Gt("PlanWorkAmount", 0M));
                oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("WorkAmountUnitGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);
                // oq.AddFetchMode("WeekScheduleDetail", NHibernate.FetchMode.Eager);
                // oq.AddFetchMode("WeekScheduleDetail.SubContractProject", NHibernate.FetchMode.Eager);

                oq.AddOrder(NHibernate.Criterion.Order.Asc("OrderNo"));
                lstResult = Dao.ObjectQuery(typeof(GWBSDetail), oq);
                #endregion
                #region 查询工程任务确认明细
                oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("Master.Id", conMaster.Id));
                oq.AddCriterion(Expression.Like("GWBSDetail.TheGWBSSysCode", oGWBSTree.SysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("Master.BillType", EnumConfirmBillType.虚拟工单));//【状态】=9.虚拟工程任务确认单
                oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TaskHandler", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);

                //oq.AddOrder(Order.Asc("RealOperationDate"));
                IList lstConfirm = Dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
                #endregion
                IList<GWBSDetail> lstTemp = new List<GWBSDetail>();//临时存储没有工程任务确认明细的 wbs明细
                #region 将查询到工程任务明细绑定到wbs明细上
                foreach (GWBSDetail oGWBSDetail in lstResult)
                {
                    var var = lstConfirm.OfType<GWBSTaskConfirm>().Where(a => (a.GWBSDetail != null && a.GWBSDetail.Id == oGWBSDetail.Id));
                    if (var != null && var.Count() > 0)
                    {
                        oGWBSDetail.GWBSTaskConfirmDetail = var.ElementAt(0);
                        lstConfirm.Remove(oGWBSDetail.GWBSTaskConfirmDetail);
                    }
                    else //if (oGWBSDetail.WeekScheduleDetail != null && oGWBSDetail.WeekScheduleDetail.SubContractProject != null)//没有工程任务确认明细
                    {
                        lstTemp.Add(oGWBSDetail);
                    }
                }
                #endregion
                #region 针对存在没有工程任务确认明细的wbs明细
                if (lstTemp.Count > 0)
                {
                    oq = new ObjectQuery();
                    oq.AddCriterion(Expression.In("WBSDetail", lstTemp.ToArray()));
                    oq.AddFetchMode("SupplierId", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("SupplierId.TheContractGroup", NHibernate.FetchMode.Eager);
                    IList lstOBS = dao.ObjectQuery(typeof(OBSService), oq);
                    //if (lstOBS != null && lstOBS.Count > 0)
                    // {
                    GWBSTaskConfirm confirm = null;
                    OBSService oOBSService = null;
                    foreach (GWBSDetail oGWBSDetail in lstTemp)
                    {
                        confirm = new GWBSTaskConfirm();
                        //if (oGWBSDetail.WeekScheduleDetail != null)//明细中没有承担者
                        //{

                        //    confirm.TaskHandler = oGWBSDetail.WeekScheduleDetail.SubContractProject;
                        //    confirm.TaskHandlerName = oGWBSDetail.WeekScheduleDetail.SupplierName;
                        //}
                        //else//找obs
                        //{
                        if (lstOBS != null && lstOBS.Count > 0)
                        {
                            var var = lstOBS.OfType<OBSService>().Where(a => a.WBSDetail == oGWBSDetail);
                            if (var != null && var.Count() > 0)
                            {
                                oOBSService = var.ElementAt(0);
                                confirm.TaskHandler = oOBSService.SupplierId;
                                confirm.TaskHandlerName = oOBSService.SupplierName;
                            }
                            else
                            {
                                confirm = null;
                            }
                        }
                        else
                        {
                            confirm = null;
                        }
                        //}
                        if (confirm != null)
                        {
                            confirm.GWBSDetail = oGWBSDetail;
                            confirm.GWBSDetailName = oGWBSDetail.Name;
                            confirm.GWBSTree = oGWBSDetail.TheGWBS;//
                            confirm.GWBSTreeName = oGWBSDetail.TheGWBS.Name;
                            confirm.GwbsSysCode = oGWBSDetail.TheGWBS.SysCode;
                            confirm.WorkAmountUnitId = oGWBSDetail.WorkAmountUnitGUID;//
                            confirm.WorkAmountUnitName = oGWBSDetail.PriceUnitName;
                            confirm.CostItem = oGWBSDetail.TheCostItem;
                            confirm.CostItemName = oGWBSDetail.TheCostItem.Name;
                            confirm.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                            confirm.RealOperationDate = DateTime.Now;
                            confirm.CreatePerson = oLoginPersonInfo;//制单人编号
                            confirm.CreatePersonName = oLoginPersonInfo.Name;//制单人名称
                            confirm.OperOrgInfo = oTheOperationOrgInfo;//登录业务组织
                            confirm.OperOrgInfoName = oTheOperationOrgInfo.Name;//业务组织名称
                            confirm.OpgSysCode = oTheOperationOrgInfo.SysCode;//业务组织编号
                            oGWBSDetail.GWBSTaskConfirmDetail = confirm;
                        }
                    }
                    // }
                }
                #endregion
            }
            return lstResult;
        }
        /// <summary>
        /// 更新工程任务明细表
        /// </summary>
        /// <param name="_GWBSTaskConfirmMaster"></param>
        /// <param name="listOldDtl"></param>
        /// <param name="action">0 修改或保存；1 删除</param>
        [TransManager]
        private void UpdateGWBSDetail(GWBSTaskConfirmMaster _GWBSTaskConfirmMaster, int action)
        {
            if (_GWBSTaskConfirmMaster == null && _GWBSTaskConfirmMaster.Details == null)
                return;

            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = "";
            if (action == 0)
            {
                //                sql = @"update thd_gwbsdetail a set (quantityConfirmed,progressConfirmed)=
                //                    (select b.quantiyAfterConfirm,b.progressAfterConfirm from thd_gwbstaskconfirmdetail b 
                //                    where a.id=b.gwbsdetail and b.parentid=:Id)
                //                    where a.id in (select b.gwbsdetail from thd_gwbstaskconfirmdetail b 
                //                    where b.parentid=:Id2)";


                //ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("Master.Id", _GWBSTaskConfirmMaster.Id));

                //IEnumerable<GWBSTaskConfirm> listOldDtl = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq).OfType<GWBSTaskConfirm>();//查询保存前所有的确认明细


                List<string> listOldDtl = new List<string>();

                sql = "select distinct GWBSDetail from THD_GWBSTaskConfirmDetail where ParentId='" + _GWBSTaskConfirmMaster.Id + "'";

                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                IDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    listOldDtl.Add(dr.GetValue(0).ToString());
                }

                //1.修改工程任务明细的确认工程量和形象进度为当前确认的工程量和形象进度(删除的任务明细)，需要先修改因为一个任务明细可能有多个确认明细
                if (listOldDtl != null && listOldDtl.Count > 0)
                {
                    string deleteDtlIds = string.Empty;//删除的任务明细ID集
                    if (_GWBSTaskConfirmMaster.Details.Count == 0)
                    {
                        foreach (string oldDtlId in listOldDtl)
                        {
                            deleteDtlIds += "'" + oldDtlId + "',";
                        }
                    }
                    else if (_GWBSTaskConfirmMaster.Details.Count > 0)
                    {
                        foreach (string oldDtlId in listOldDtl)
                        {
                            var query = from d in _GWBSTaskConfirmMaster.Details.OfType<GWBSTaskConfirm>()
                                        where d.GWBSDetail.Id == oldDtlId
                                        select d;

                            if (query.Count() == 0)
                                deleteDtlIds += "'" + oldDtlId + "',";
                        }
                    }

                    if (!string.IsNullOrEmpty(deleteDtlIds))
                    {
                        deleteDtlIds = deleteDtlIds.Substring(0, deleteDtlIds.Length - 1);

                        sql = @"update thd_gwbsdetail a set (quantityConfirmed,progressConfirmed)=
                    (select max(b.quantityBeforeConfirm),max(b.progressBeforeConfirm) from thd_gwbstaskconfirmdetail b 
                    where b.gwbsdetail=a.id and b.parentid=:Id)
                    where a.id in (" + deleteDtlIds + ")";
                        command.CommandText = sql;
                        command.Parameters.Clear();
                        command.Parameters.Add("Id", OracleType.VarChar).Value = _GWBSTaskConfirmMaster.Id;
                        command.ExecuteNonQuery();
                    }
                }

                //2.修改工程任务明细的确认工程量和形象进度为当前确认的工程量和形象进度(已有)
                string currDtlIds = string.Empty;
                foreach (GWBSTaskConfirm confirmDetail in _GWBSTaskConfirmMaster.Details)
                {
                    sql = @"update thd_gwbsdetail set quantityConfirmed=:Quantity,progressConfirmed=:Progress where id=:DetailId";
                    command.CommandText = sql;
                    command.Parameters.Clear();
                    command.Parameters.Add("Quantity", OracleType.Double).Value = confirmDetail.QuantiyAfterConfirm;
                    command.Parameters.Add("Progress", OracleType.Double).Value = confirmDetail.ProgressAfterConfirm;
                    command.Parameters.Add("DetailId", OracleType.VarChar).Value = confirmDetail.GWBSDetail.Id;
                    command.ExecuteNonQuery();
                }
            }
            else if (action == 1)
            {
                //修改工程任务明细的确认工程量和形象进度为上一次确认的工程量和形象进度

                sql = @"update thd_gwbsdetail a set (quantityConfirmed,progressConfirmed)=
                    (select max(b.quantityBeforeConfirm),max(b.progressBeforeConfirm) from thd_gwbstaskconfirmdetail b 
                    where b.gwbsdetail=a.id and b.parentid=:Id)
                    where a.id in (select b.gwbsdetail from thd_gwbstaskconfirmdetail b 
                    where b.parentid=:Id2)";
                command.CommandText = sql;
                command.Parameters.Add("Id", OracleType.VarChar).Value = _GWBSTaskConfirmMaster.Id;
                command.Parameters.Add("Id2", OracleType.VarChar).Value = _GWBSTaskConfirmMaster.Id;
                command.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// 更新周计划明细表
        /// </summary>
        /// <param name="_GWBSTaskConfirmMaster"></param>
        /// <param name="action">0 修改或保存；1 删除</param>
        [TransManager]
        private void UpdateWeekScheduleDetail(GWBSTaskConfirmMaster _GWBSTaskConfirmMaster, int action)
        {
            if (_GWBSTaskConfirmMaster == null && (_GWBSTaskConfirmMaster.Details == null || _GWBSTaskConfirmMaster.Details.Count == 0))
                return;

            ISession session = CallContext.GetData("nhsession") as ISession;
            OracleConnection conn = session.Connection as OracleConnection;
            OracleCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            IList weekScheduleDetailIdList = new ArrayList();
            string confirmIds = string.Empty;
            foreach (GWBSTaskConfirm confirmDetail in _GWBSTaskConfirmMaster.Details)
            {
                if (confirmDetail.Id != null)
                    confirmIds += "'" + confirmDetail.Id + "',";

                WeekScheduleDetail weekDtl = confirmDetail.WeekScheduleDetailGUID;
                if (!weekScheduleDetailIdList.Contains(weekDtl.Id))
                {
                    weekScheduleDetailIdList.Add(weekDtl.Id);
                }
            }

            //            if (!string.IsNullOrEmpty(confirmIds))
            //            {
            //                confirmIds = confirmIds.Substring(0, confirmIds.Length - 1);

            //                string deleteWeekDtlIds = string.Empty;//存被删除的确认明细对应的前驱周进度计划明细id集

            //                string sql = "select distinct WeekScheduleDetailGUID from THD_GWBSTaskConfirmDetail where ParentId='" + _GWBSTaskConfirmMaster.Id + "' and id not in (" + confirmIds + ")";

            //                command.CommandText = sql;
            //                command.CommandType = CommandType.Text;
            //                IDataReader dr = command.ExecuteReader();
            //                while (dr.Read())
            //                {
            //                    deleteWeekDtlIds += "'" + dr.GetValue(0).ToString() + "',";
            //                }


            //                //1.修改删除的确认明细的前驱周计划明细的确认标志和确认时间
            //                if (string.IsNullOrEmpty(deleteWeekDtlIds) == false)
            //                {
            //                    deleteWeekDtlIds = deleteWeekDtlIds.Substring(0, deleteWeekDtlIds.Length - 1);

            //                    sql = @"update thd_weekscheduledetail g set g.GWBSConfirmFlag=:GWBSConfirmFlag,g.GWBSConfirmDate=:GWBSConfirmDate 
            //                               where g.id in (" + deleteWeekDtlIds + ")";

            //                    command.CommandText = sql;
            //                    command.Parameters.Clear();
            //                    command.Parameters.Add("GWBSConfirmFlag", OracleType.Int32).Value = 0;
            //                    command.Parameters.Add("GWBSConfirmDate", OracleType.DateTime).Value = (new DateTime(1900, 1, 1)).Date;
            //                    command.ExecuteNonQuery();
            //                }
            //            }

            //2.修改当前确认明细的前驱周计划明细的确认状态和确认时间
            int GWBSConfirmFlag = 1;
            DateTime GWBSConfirmDate = DateTime.Now;

            if (action == 1)
            {
                GWBSConfirmFlag = 0;
                GWBSConfirmDate = (new DateTime(1900, 1, 1)).Date;
            }
            foreach (string weekScheduleDetailId in weekScheduleDetailIdList)
            {
                command.Parameters.Clear();

                //修改前驱周计划明细（同一项目任务且形象进度小于等于当前确认周计划明细的形象进度，且已确认过周计划形象进度和未确认工程量）的确认状态和确认时间，第二次设计调整前
                //                string sql = @"update thd_weekscheduledetail g set g.GWBSConfirmFlag=:GWBSConfirmFlag,g.GWBSConfirmDate=:GWBSConfirmDate
                //                                    where g.ScheduleConfirmFlag=1 and g.GWBSTree=(select gwbstree from thd_weekscheduledetail where id=:Id) 
                //                                    and g.taskcompletedpercent<=(select taskcompletedpercent from thd_weekscheduledetail where id=:Id2)";


                //修改前驱周计划明细的确认状态和确认时间
                string sql = @"update thd_weekscheduledetail g set g.GWBSConfirmFlag=:GWBSConfirmFlag,g.GWBSConfirmDate=:GWBSConfirmDate 
                               where g.id=:Id";

                command.CommandText = sql;
                command.Parameters.Add("GWBSConfirmFlag", OracleType.Int32).Value = GWBSConfirmFlag;
                command.Parameters.Add("GWBSConfirmDate", OracleType.DateTime).Value = GWBSConfirmDate;
                command.Parameters.Add("Id", OracleType.VarChar).Value = weekScheduleDetailId;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 删除工程任务确认
        /// </summary>
        /// <param name="_GWBSTaskConfirmMaster"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteGWBSTaskConfirmMaster(GWBSTaskConfirmMaster _GWBSTaskConfirmMaster)
        {
            //UpdateWeekScheduleDetail(_GWBSTaskConfirmMaster, 1);//提交时才修改前驱周计划明细，删除时只针对编辑状态的确认单，所以不需修改前驱周计划明细的状态

            UpdateGWBSDetail(_GWBSTaskConfirmMaster, 1);
            return DeleteByDao(_GWBSTaskConfirmMaster);
        }

        /// <summary>
        /// 删除工程任务确认
        /// </summary>
        /// <param name="_GWBSTaskConfirmMaster"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteGWBSTaskConfirmMaster2(GWBSTaskConfirmMaster _GWBSTaskConfirmMaster)
        {
            UpdateGWBSDetail(_GWBSTaskConfirmMaster, 1);
            return DeleteByDao(_GWBSTaskConfirmMaster);
        }
        /// <summary>
        /// 删除工程任务确认明细
        /// </summary>
        /// <param name="_GWBSTaskConfirmMaster"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteGWBSTaskConfirm(GWBSTaskConfirm confirm)
        {
            GWBSDetail dtl = dao.Get(typeof(GWBSDetail), confirm.GWBSDetail.Id) as GWBSDetail;
            dtl.QuantityConfirmed -= confirm.QuantiyAfterConfirm;
            dtl.ProgressConfirmed = dtl.QuantityConfirmed / dtl.PlanWorkAmount * 100;
            dao.Update(dtl);
            return DeleteByDao(confirm);
        }

        /// <summary>
        /// 查询工程任务确认明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetGWBSTaskConfirm(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
        }

        /// <summary>
        /// 根据工程任务Id查询工程任务明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GetGWBSDetailByParentId(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheGWBS.Id", id));

            oq.AddFetchMode("TheCostItem", FetchMode.Eager);

            return Dao.ObjectQuery(typeof(GWBSDetail), oq);
        }

        /// <summary>
        /// 查询工程任务明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetGWBSDetail(ObjectQuery oq)
        {
            oq.AddFetchMode("TheCostItem", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(GWBSDetail), oq);
        }

        /// <summary>
        /// 查询当前节点的父节点和所有的子节点
        /// </summary>
        /// <param name="currentNode"></param>
        /// <returns></returns>
        public List<GWBSTree> GetGWBSTree(GWBSTree currentNode)
        {
            List<GWBSTree> list = new List<GWBSTree>();
            if (currentNode == null) return list;
            List<GWBSTree> parnetNodes = GetParentNode(currentNode);
            list.AddRange(parnetNodes);
            List<GWBSTree> childNodes = GetChildNodes(currentNode);
            list.AddRange(childNodes);

            return list;
        }

        private List<GWBSTree> GetParentNode(GWBSTree currentNode)
        {
            List<GWBSTree> list = new List<GWBSTree>();
            if (currentNode.ParentNode != null)
            {
                GWBSTree tree = GetGWBSTreeById(((GWBSTree)currentNode.ParentNode).Id);
                if (tree != null)
                {
                    list.Add(tree);
                    List<GWBSTree> tempList = GetParentNode(tree);
                    list.AddRange(tempList);
                }
            }
            return list;
        }

        private GWBSTree GetGWBSTreeById(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));

            IList list = Dao.ObjectQuery(typeof(GWBSTree), oq);
            if (list != null && list.Count > 0) return list[0] as GWBSTree;
            return null;
        }

        private List<GWBSTree> GetChildNodes(GWBSTree currentNode)
        {
            List<GWBSTree> retList = new List<GWBSTree>();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode", currentNode.SysCode, MatchMode.Start));

            IList list = Dao.ObjectQuery(typeof(GWBSTree), oq);
            if (list == null || list.Count == 0) return retList;
            foreach (GWBSTree tree in list)
            {
                retList.Add(tree);
            }
            return retList;
        }

        /// <summary>
        /// 查询生产节点
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetGWBSTaskConfirmNode(ObjectQuery oq)
        {
            return Dao.ObjectQuery(typeof(GWBSTaskConfirmNode), oq);
        }

        /// <summary>
        /// 根据工程任务层次码 查询周计划明细 用于生成生产节点
        /// </summary>
        /// <param name="gwbsSysCode"></param>
        /// <returns></returns>
        public DataSet QueryNode(string gwbsSysCode)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            DataSet ds = new DataSet();
            string sql = @"select id detailId,gwbsTree,gwbsTreeName,taskCompletedPercent
                from thd_weekscheduledetail where scheduleConfirmFlag=1 and gwbsTree in (select id from thd_gwbstree where 
                syscode like :SysCode)";
            command.CommandText = sql;
            IDbDataParameter p_sysCode = command.CreateParameter();
            p_sysCode.DbType = DbType.String;
            p_sysCode.ParameterName = "SysCode";
            p_sysCode.Value = gwbsSysCode + "%";
            command.Parameters.Add(p_sysCode);
            IDataReader dataReader = command.ExecuteReader();
            ds.Load(dataReader, LoadOption.PreserveChanges, new string[] { "table1" });
            return ds;
        }

        /// <summary>
        /// 判断工程任务明细是否需要做结算
        /// </summary>
        /// <param name="_GWBSDetailId"></param>
        /// <returns></returns>
        public bool IfSubBalance(string _GWBSDetailId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheGWBSDetail.Id", _GWBSDetailId));
            oq.AddFetchMode("CostAccountSubject", FetchMode.Eager);
            IList lst = Dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
            //只有一条工程资源耗用明细时，不用判断是否做结算
            if (lst == null || lst.Count == 0 || lst.Count == 1) return false;
            foreach (GWBSDetailCostSubject obj in lst)
            {
                if (obj.CostAccountSubjectGUID.IfSubBalanceFlag == 3)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 检查记录单
        /// <summary>
        /// 根据ID查询检查记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InspectionRecord GetInspectionRecordById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetInspectionRecord(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as InspectionRecord;
            }
            return null;
        }

        #region
        /// <summary>
        /// 杜孟佳
        /// 根据parentId查询检查记录
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetInspectionRecordMaster(ObjectQuery oq)
        {
            //oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(InspectionRecord), oq);
        }

        /// <summary>
        /// 根据parentId查询文档
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetSitePictureVideoMaster(ObjectQuery oq)
        {
            //oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(SitePictureVideo), oq);
        }

        /// <summary>
        /// 根据parentId到基表中查询检查专业
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetInspectialSpecialMaster(ObjectQuery oq)
        {
            //oq.AddFetchMode("Details", FetchMode.Eager);
            return Dao.ObjectQuery(typeof(BasicDataOptr), oq);
        }

        /// <summary>
        /// 保存检查记录对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SaveInspectialRecordMaster(InspectionRecord obj)
        {
            string msg = "success";
            try
            {
                InspectionRecord ir = SaveOrUpdateByDao(obj) as InspectionRecord;
            }
            catch (Exception e)
            {
                msg = "fail";
            }
            return msg;
        }

        /// <summary>
        /// 保存检查记录对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public InspectionRecord SaveInsMaster(InspectionRecord obj)
        {
            if (obj.Id == null)
            {
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            InspectionRecord inspectionRecord = SaveOrUpdateByDao(obj) as InspectionRecord;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                //提交的时候将信息反写给GWBSTree
                string strSpecail = ClientUtil.ToString(inspectionRecord.InspectionSpecialCode);
                string strGWBS = ClientUtil.ToString(obj.GWBSTree.CheckRequire);
                GWBSTree gwbs = obj.GWBSTree;
                if (strSpecail.Equals("001"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = "2" + strGWBS.Substring(1, 11);
                        }
                        else
                        {
                            gwbs.CheckRequire = "1" + strGWBS.Substring(1, 11);
                        }
                    }
                }
                else if (strSpecail.Equals("002"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 1) + "2" + strGWBS.Substring(2, 10);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 1) + "1" + strGWBS.Substring(2, 10);
                        }
                    }
                }
                else if (strSpecail.Equals("003"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 2) + "2" + strGWBS.Substring(3, 9);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 2) + "1" + strGWBS.Substring(3, 9);
                        }
                    }
                }
                else if (strSpecail.Equals("004"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 3) + "2" + strGWBS.Substring(4, 8);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 3) + "1" + strGWBS.Substring(4, 8);
                        }
                    }
                }
                else if (strSpecail.Equals("005"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 4) + "2" + strGWBS.Substring(5, 7);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 4) + "1" + strGWBS.Substring(5, 7);
                        }
                    }
                }
                else if (strSpecail.Equals("006"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 5) + "2" + strGWBS.Substring(6, 6);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 5) + "1" + strGWBS.Substring(6, 6);
                        }
                    }
                }
                else if (strSpecail.Equals("007"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 6) + "2" + strGWBS.Substring(7, 5);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 6) + "1" + strGWBS.Substring(7, 5);
                        }
                    }
                }
                gwbs = SaveOrUpdateByDao(gwbs) as GWBSTree;
            }
            return inspectionRecord;
        }


        [TransManager]
        public InspectionRecord SaveInsMaster(InspectionRecord obj, bool isSubmit)
        {
            InspectionRecord rtn_ir = SaveInsMaster(obj);
            if (isSubmit)
            { 
                //提交质量检查 将WBS节点形象进度，以及总进度计划　进度设置为　１００％
                string str_wbs = obj.GWBSTree.Id;
                string sql = string.Format(@"update thd_gwbstree  set addupfigureprogress = 100 where id = '{0}' ",str_wbs);
                ExecuteSql(sql);

//                sql = string.Format(@"update thd_weekscheduledetail t1
//                                        set t1.taskcompletedpercent = 100
//                                      where exists  (select 1
//                                               from thd_weekschedulemaster tm where  tm.id = t1.parentid
//                                                                                  and tm.execscheduletype = 40)
//                                        and t1.gwbstree = '{0}'",str_wbs);
//                ExecuteSql(sql);

            }
            if (obj.Id == null)
            {
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            InspectionRecord inspectionRecord = SaveOrUpdateByDao(obj) as InspectionRecord;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                //提交的时候将信息反写给GWBSTree
                string strSpecail = ClientUtil.ToString(inspectionRecord.InspectionSpecialCode);
                string strGWBS = ClientUtil.ToString(obj.GWBSTree.CheckRequire);
                GWBSTree gwbs = obj.GWBSTree;
                if (strSpecail.Equals("001"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = "2" + strGWBS.Substring(1, 11);
                        }
                        else
                        {
                            gwbs.CheckRequire = "1" + strGWBS.Substring(1, 11);
                        }
                    }
                }
                else if (strSpecail.Equals("002"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 1) + "2" + strGWBS.Substring(2, 10);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 1) + "1" + strGWBS.Substring(2, 10);
                        }
                    }
                }
                else if (strSpecail.Equals("003"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 2) + "2" + strGWBS.Substring(3, 9);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 2) + "1" + strGWBS.Substring(3, 9);
                        }
                    }
                }
                else if (strSpecail.Equals("004"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 3) + "2" + strGWBS.Substring(4, 8);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 3) + "1" + strGWBS.Substring(4, 8);
                        }
                    }
                }
                else if (strSpecail.Equals("005"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 4) + "2" + strGWBS.Substring(5, 7);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 4) + "1" + strGWBS.Substring(5, 7);
                        }
                    }
                }
                else if (strSpecail.Equals("006"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 5) + "2" + strGWBS.Substring(6, 6);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 5) + "1" + strGWBS.Substring(6, 6);
                        }
                    }
                }
                else if (strSpecail.Equals("007"))
                {
                    if (gwbs.CheckRequire != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 6) + "2" + strGWBS.Substring(7, 5);
                        }
                        else
                        {
                            gwbs.CheckRequire = strGWBS.Substring(0, 6) + "1" + strGWBS.Substring(7, 5);
                        }
                    }
                }
                if (inspectionRecord.InspectionConclusion == "通过")
                {
                    gwbs.AddUpFigureProgress = 100;
                }

                gwbs = SaveOrUpdateByDao(gwbs) as GWBSTree;
            }
            return inspectionRecord;
    
            
        }

        /// <summary>
        /// 保存检查记录对象同时更新周进度计划信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public InspectionRecord SaveInspectialRecordMaster(InspectionRecord obj, WeekScheduleDetail weekDetail)
        {
            if (obj.Id == null)
            {
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                obj.SubmitDate = DateTime.Now;
            }
            InspectionRecord inspectionRecord = SaveOrUpdateByDao(obj) as InspectionRecord;
            if (obj.DocState == DocumentState.InAudit || obj.DocState == DocumentState.InExecute)
            {
                //weekDetail.ScheduleConfirmDate = DateTime.Now;//当前时间(周计划的形象进度确认是在周计划确认里不在日常检查里)
                string strCheckRequtre = ClientUtil.ToString(inspectionRecord.GWBSTree.CheckRequire);

                string strSpecail = ClientUtil.ToString(inspectionRecord.InspectionSpecialCode);
                if (strSpecail.Equals("001"))
                {
                    if (weekDetail.TaskCheckState != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = "2" + strWeek.Substring(1, 11);
                        }
                        else
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = "1" + strWeek.Substring(1, 11);
                        }
                    }
                    else
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            weekDetail.TaskCheckState = "2" + strCheckRequtre.Substring(1, 11);
                        }
                        else
                        {
                            weekDetail.TaskCheckState = strCheckRequtre;
                        }

                    }
                }
                else if (strSpecail.Equals("002"))
                {
                    if (weekDetail.TaskCheckState != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 1) + "2" + strWeek.Substring(2, 10);
                        }
                        else
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 1) + "1" + strWeek.Substring(2, 10);
                        }
                    }
                    else
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            weekDetail.TaskCheckState = strCheckRequtre.Substring(0, 1) + "2" + strCheckRequtre.Substring(2, 10);
                        }
                        else
                        {
                            weekDetail.TaskCheckState = strCheckRequtre;
                        }
                    }
                }
                else if (strSpecail.Equals("003"))
                {
                    if (weekDetail.TaskCheckState != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 2) + "2" + strWeek.Substring(3, 9);
                        }
                        else
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 2) + "1" + strWeek.Substring(3, 9);
                        }
                    }
                    else
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            weekDetail.TaskCheckState = strCheckRequtre.Substring(0, 2) + "2" + strCheckRequtre.Substring(3, 9);
                        }
                        else
                        {
                            weekDetail.TaskCheckState = strCheckRequtre;
                        }
                    }
                }
                else if (strSpecail.Equals("004"))
                {
                    if (weekDetail.TaskCheckState != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 3) + "2" + strWeek.Substring(4, 8);
                        }
                        else
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 3) + "1" + strWeek.Substring(4, 8);
                        }
                    }
                    else
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            weekDetail.TaskCheckState = strCheckRequtre.Substring(0, 3) + "2" + strCheckRequtre.Substring(4, 8);
                        }
                        else
                        {
                            weekDetail.TaskCheckState = strCheckRequtre;
                        }
                    }
                }
                else if (strSpecail.Equals("005"))
                {
                    if (weekDetail.TaskCheckState != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 4) + "2" + strWeek.Substring(5, 7);
                        }
                        else
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 4) + "1" + strWeek.Substring(5, 7);
                        }
                    }
                    else
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            weekDetail.TaskCheckState = strCheckRequtre.Substring(0, 4) + "2" + strCheckRequtre.Substring(5, 7);
                        }
                        else
                        {
                            weekDetail.TaskCheckState = strCheckRequtre;
                        }
                    }
                }
                else if (strSpecail.Equals("006"))
                {
                    if (weekDetail.TaskCheckState != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 5) + "2" + strWeek.Substring(6, 6);
                        }
                        else
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 5) + "1" + strWeek.Substring(6, 6);
                        }
                    }
                    else
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            weekDetail.TaskCheckState = strCheckRequtre.Substring(0, 5) + "2" + strCheckRequtre.Substring(6, 6);
                        }
                        else
                        {
                            weekDetail.TaskCheckState = strCheckRequtre;
                        }
                    }
                }
                else if (strSpecail.Equals("007"))
                {
                    if (weekDetail.TaskCheckState != null)
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 6) + "2" + strWeek.Substring(7, 5);
                        }
                        else
                        {
                            string strWeek = ClientUtil.ToString(weekDetail.TaskCheckState);
                            weekDetail.TaskCheckState = strWeek.Substring(0, 6) + "1" + strWeek.Substring(7, 5);
                        }
                    }
                    else
                    {
                        if (inspectionRecord.InspectionConclusion == "通过")
                        {
                            weekDetail.TaskCheckState = strCheckRequtre.Substring(0, 6) + "2" + strCheckRequtre.Substring(7, 5);
                        }
                        else
                        {
                            weekDetail.TaskCheckState = strCheckRequtre;
                        }
                    }
                }
                weekDetail = UpdateWeekScheduleDetail(weekDetail);
            }
            return inspectionRecord;
        }

        /// <summary>
        /// 根据ID查询一条文档信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SitePictureVideo GetSitePictureVideoById(string id)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("Id", id));
            IList list = GetSitePictureVideoMaster(objectQuery);
            if (list != null && list.Count > 0)
            {
                return list[0] as SitePictureVideo;
            }
            return null;
        }

        #endregion
        /// <summary>
        /// 查询检查记录单
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetInspectionRecord(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("PBSTree", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("HandlePerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("InspectionPerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("RectificationNotices", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("RectificationNotices.SubjectOrganization", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("RectificationNotices.SupjectOrgPerson", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("SitePictureVideos", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("WeekScheduleDetail", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("WeekScheduleDetail.GWBSTree", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(InspectionRecord), objectQuery);
        }

        /// <summary>
        /// 执行进度计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataSet GetInspectionRecordQuery(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            string sql = @"SELECT GWBSTreeName,PBSTreeName,InspectionSpecial,InspectionConclusion,InspectionContent,InspectionStatus,InspectionDate,RealOperationDate,
                           State,InspectionPersonName,t2.RectificationConclusion,BearTeamName,CorrectiveSign
                           FROM THD_InspectionRecord t1 LEFT JOIN THD_RectificationNotice t2 ON t2.ParentId=t1.Id";
            sql += " where 1=1 " + condition + " order by t1.InspectionDate";
            command.CommandText = sql;
            IDataReader dr = command.ExecuteReader();
            return DataAccessUtil.ConvertDataReadertoDataSet(dr);
        }

        #endregion

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
        /// 通过WeekScheduleDetail查找InspectionRecord
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IList GetByWeekDetail(WeekScheduleDetail code)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("WeekScheduleDetail", code));
            return Dao.ObjectQuery(typeof(InspectionRecord), objectQuery);
        }

        /// <summary>
        /// 修改周计划明细并更新wbs相关检查状态以及确认单检查状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public WeekScheduleDetail UpdateWeekScheduleDetail(WeekScheduleDetail obj)
        {

            dao.SaveOrUpdate(obj);

            GWBSTree wbs = dao.Get(typeof(GWBSTree), obj.GWBSTree.Id) as GWBSTree;
            wbs.DailyCheckState = obj.TaskCheckState;

            bool passFlag = GetCheckStatePassStr(wbs.DailyCheckState);
            if (passFlag == true && wbs.AddUpFigureProgress == 100)//形象进度为100，且日常检查状态全部通过，则设置检验检查状态为“已通过”
            {
                wbs.AcceptanceCheckState = AcceptanceCheckStateEnum.已通过;
            }

            dao.Update(wbs);

            //修改确认单检查状态
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("GWBSTree.Id", wbs.Id));
            oq.AddCriterion(Expression.Eq("WeekScheduleDetailGUID.Id", obj.Id));
            oq.AddCriterion(Expression.Eq("Master.DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Valid));
            IList listConfirm = dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);

            if (listConfirm.Count > 0)
            {

                passFlag = false;

                for (int i = 0; i < listConfirm.Count; i++)
                {
                    GWBSTaskConfirm confirm = listConfirm[i] as GWBSTaskConfirm;

                    confirm.DailyCheckState = !string.IsNullOrEmpty(obj.TaskCheckState) ? ("2" + obj.TaskCheckState.Substring(1)) : "";//工长质检(标志第一位)缺省为通过

                    if (passFlag == false && GetCheckStatePassStr(confirm.DailyCheckState))
                        passFlag = true;
                }
                dao.Update(listConfirm);



                //如果下属确认单明细都通过检查则将确认单提交审批
                if (passFlag)
                {

                    var queryGroupMaster = from c in listConfirm.OfType<GWBSTaskConfirm>()
                                           group c by new { masterId = c.Master.Id } into g
                                           select new { g.Key.masterId };


                    oq.Criterions.Clear();
                    Disjunction dis = new Disjunction();
                    foreach (var masterObj in queryGroupMaster)
                    {
                        dis.Add(Expression.Eq("Id", masterObj.masterId));
                    }
                    oq.AddCriterion(dis);
                    oq.AddFetchMode("Details", FetchMode.Eager);
                    IList listConfirmMaster = dao.ObjectQuery(typeof(GWBSTaskConfirmMaster), oq);


                    List<GWBSTaskConfirmMaster> listUpdateMaster = new List<GWBSTaskConfirmMaster>();
                    for (int i = 0; i < listConfirmMaster.Count; i++)
                    {
                        GWBSTaskConfirmMaster master = listConfirmMaster[i] as GWBSTaskConfirmMaster;

                        var query = from cf in master.Details.OfType<GWBSTaskConfirm>()
                                    where GetCheckStatePassStr(cf.DailyCheckState) == false
                                    select cf;

                        if (query.Count() == 0)//下属明细检查状态均通过
                        {
                            master.DocState = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.InAudit;
                            listUpdateMaster.Add(master);
                        }
                    }

                    if (listUpdateMaster.Count > 0)
                        dao.Update(listConfirmMaster);
                }
            }

            return obj;
        }

        /// <summary>
        /// 本队伍已确认累计工程量
        /// </summary>
        /// <param name="info">当前操作项目</param>
        /// <param name="dtl">(操作{工程任务确认明细})_【被确认工程任务明细】</param>
        /// <param name="sub">(操作{工程任务确认明细})_【承担者】</param>
        /// <returns></returns>
        public decimal GetTheTeamQuantityConfirmed(CurrentProjectInfo info, GWBSDetail dtl, SubContractProject sub)
        {
            decimal num = 0;
            if (info == null || dtl == null || sub == null) return num;
            ObjectQuery oq = new ObjectQuery();
            //GWBSTaskConfirm c = new GWBSTaskConfirm();
            //c.
            oq.AddCriterion(Expression.Eq("Master.ProjectId", info.Id));
            oq.AddCriterion(Expression.Eq("GWBSDetail.Id", dtl.Id));
            oq.AddCriterion(Expression.Eq("TaskHandler.Id", sub.Id));
            IList list = Dao.ObjectQuery(typeof(GWBSTaskConfirm), oq);
            if (list != null && list.Count > 0)
            {
                foreach (GWBSTaskConfirm con in list)
                {
                    num += con.ActualCompletedQuantity;
                }
            }
            return num;
        }

        /// <summary>
        /// 更新周计划明细和滚动进度计划
        /// </summary>
        /// <param name="weekPlanDtl"></param>
        /// <returns></returns>
        [TransManager]
        public WeekScheduleDetail UpdateWeekScheduleDetailAndScrollPlan(WeekScheduleDetail weekPlanDtl)
        {
            dao.Update(weekPlanDtl);

            //回写实际开始时间和形象进度到前驱滚动计划明细
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", weekPlanDtl.ForwardBillDtlId));
            IList listFrontPlanDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq);
            if (listFrontPlanDtl.Count > 0)//获取前驱工区周计划
            {
                WeekScheduleDetail frontPlanDtl = listFrontPlanDtl[0] as WeekScheduleDetail;

                oq.Criterions.Clear();

                oq.AddCriterion(Expression.Eq("Id", frontPlanDtl.ForwardBillDtlId));

                IList listFrontScrollPlanDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                if (listFrontScrollPlanDtl.Count > 0)//获取前驱滚动计划
                {
                    ProductionScheduleDetail frontScrollPlanDtl = listFrontScrollPlanDtl[0] as ProductionScheduleDetail;

                    if (frontScrollPlanDtl.ActualBeginDate == DateTime.Parse("1900-1-1") || frontScrollPlanDtl.ActualBeginDate > weekPlanDtl.ActualBeginDate)
                    {
                        frontScrollPlanDtl.ActualBeginDate = weekPlanDtl.ActualBeginDate;
                    }

                    frontScrollPlanDtl.AddupFigureProgress = weekPlanDtl.TaskCompletedPercent;

                    if (frontScrollPlanDtl.AddupFigureProgress == 100)
                        frontScrollPlanDtl.ActualEndDate = weekPlanDtl.ActualEndDate;

                    dao.Update(frontScrollPlanDtl);
                }
            }

            //回写累计形象进度到项目任务
            oq.Criterions.Clear();

            GWBSTree wbs = dao.Get(typeof(GWBSTree), weekPlanDtl.GWBSTree.Id) as GWBSTree;
            wbs.AddUpFigureProgress = weekPlanDtl.TaskCompletedPercent;

            //形象进度为100，且日常检查状态全部通过，则设置检验检查状态为“已通过”
            if (wbs.AddUpFigureProgress == 100 && GetCheckStatePassStr(wbs.DailyCheckState) == true)
                wbs.AcceptanceCheckState = AcceptanceCheckStateEnum.已通过;

            dao.Update(wbs);

            return weekPlanDtl;
        }

        /// <summary>
        /// 判断日常检查是否通过(检查标志：0.未检查，1.检查未通过，2.检查通过)
        /// </summary>
        /// <param name="checkState"></param>
        /// <returns></returns>
        public bool GetCheckStatePassStr(string checkState)
        {
            if (string.IsNullOrEmpty(checkState))
                return true;
            else if (checkState.Length == 12)//第12位为工程量确认标志，前面为检查标志
                checkState = checkState.Substring(0, 11);

            return (checkState.IndexOf("0") == -1 && checkState.IndexOf("1") == -1) ? true : false;
        }

        [TransManager]
        public bool DeleteInspectionRecord(InspectionRecord obj)
        {
            if (obj.Id == null) return true;
            return dao.Delete(obj);
        }

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public bool Delete(IList list)
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            bool flag = false;
            foreach (ProObjectRelaDocument cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    dis.Add(Expression.Eq("Id", cg.Id));
                    flag = true;
                }
            }
            if (flag)
            {
                oq.AddCriterion(dis);

                IList listTemp = dao.ObjectQuery(typeof(ProObjectRelaDocument), oq);
                if (listTemp != null && listTemp.Count > 0)
                    dao.Delete(listTemp);
            }

            return true;
        }

        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdate(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        [TransManager]
        public ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item)
        {
            dao.SaveOrUpdate(item);

            return item;
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

        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 根据总进度计划复制一个新的总进度计划
        /// </summary>
        /// <param name="targetPlan">源计划（要复制的计划）</param>
        /// <returns></returns>
        [TransManager]
        public ProductionScheduleMaster CopyNewSchdulePlan(ProductionScheduleMaster targetPlan)
        {
            ProductionScheduleMaster newPlan = new ProductionScheduleMaster();

            newPlan.ProjectId = targetPlan.ProjectId;
            newPlan.ProjectName = targetPlan.ProjectName;
            newPlan.CreateDate = DateTime.Now;

            Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;
            newPlan.HandlePerson = login.ThePerson;
            newPlan.HandlePersonName = login.ThePerson.Name;
            newPlan.OperOrgInfo = login.TheOperationOrgInfo;
            newPlan.OperOrgInfoName = login.TheOperationOrgInfo.Name;
            newPlan.OpgSysCode = login.TheOperationOrgInfo.SysCode;

            newPlan.DocState = DocumentState.Edit;
            newPlan.ScheduleType = targetPlan.ScheduleType;
            newPlan.ScheduleCaliber = targetPlan.ScheduleCaliber;
            newPlan.ScheduleTypeDetail = targetPlan.ScheduleTypeDetail;
            newPlan.ScheduleName = targetPlan.ScheduleName;
            newPlan.ScheduleVersion = targetPlan.ScheduleVersion;

            var queryDtl = from d in targetPlan.Details
                           where d.ParentNode == null
                           select d;

            ProductionScheduleDetail dtl = queryDtl.ElementAt(0);

            ProductionScheduleDetail dtlNew = new ProductionScheduleDetail();
            dtlNew.Master = newPlan;
            newPlan.Details.Add(dtlNew);

            dtlNew.GWBSTree = dtl.GWBSTree;
            dtlNew.GWBSTreeName = dtl.GWBSTreeName;
            dtlNew.GWBSTreeSysCode = dtl.GWBSTreeSysCode;
            dtlNew.GWBSNodeType = dtl.GWBSNodeType;
            dtlNew.ParentNode = null;
            dtlNew.Level = 1;
            dtlNew.State = EnumScheduleDetailState.有效;
            dtlNew.OrderNo = 0;
            dtlNew.ScheduleUnit = "天";
            dtlNew.PlannedBeginDate = dtl.PlannedBeginDate;
            dtlNew.PlannedEndDate = dtl.PlannedEndDate;
            dtlNew.PlannedDuration = dtl.PlannedDuration;

            newPlan = SaveOrUpdateByDao(newPlan) as ProductionScheduleMaster;

            dtlNew.SysCode = dtlNew.Id + ".";

            dtlNew = SaveOrUpdateByDao(dtlNew) as ProductionScheduleDetail;

            CopyPlanDetail(dtl, targetPlan, dtlNew, newPlan);

            return newPlan;

        }

        private void CopyPlanDetail(ProductionScheduleDetail targetParentDtl, ProductionScheduleMaster targetPlan, ProductionScheduleDetail newParentDtl, ProductionScheduleMaster newPlan)
        {
            var query = from d in targetPlan.Details
                        orderby d.OrderNo ascending
                        where d.ParentNode != null && d.ParentNode.Id == targetParentDtl.Id
                        select d;

            foreach (ProductionScheduleDetail dtl in query)
            {
                ProductionScheduleDetail dtlNew = new ProductionScheduleDetail();
                dtlNew.Master = newPlan;
                newPlan.Details.Add(dtlNew);

                dtlNew.GWBSTree = dtl.GWBSTree;
                dtlNew.GWBSTreeName = dtl.GWBSTreeName;
                dtlNew.GWBSTreeSysCode = dtl.GWBSTreeSysCode;
                dtlNew.GWBSNodeType = dtl.GWBSNodeType;
                dtlNew.OrderNo = dtl.OrderNo;

                dtlNew.ParentNode = newParentDtl;
                dtlNew.Level = newParentDtl.Level + 1;
                dtlNew.State = EnumScheduleDetailState.编辑;

                dtlNew.PlannedBeginDate = dtl.PlannedBeginDate;
                dtlNew.PlannedEndDate = dtl.PlannedEndDate;
                dtlNew.PlannedDuration = dtl.PlannedDuration;
                dtlNew.ScheduleUnit = dtl.ScheduleUnit;
                dtlNew.TaskDescript = dtl.TaskDescript;

                dao.Save(dtlNew);
                dtlNew.SysCode = newParentDtl.SysCode + dtlNew.Id + ".";
                dao.Update(dtlNew);

                CopyPlanDetail(dtl, targetPlan, dtlNew, newPlan);
            }
        }

        /// <summary>
        /// 根据总进度计划复制一个新的总滚动进度计划
        /// </summary>
        /// <param name="targetPlan">源计划（要复制的计划）</param>
        /// <returns></returns>
        public ProductionScheduleMaster CopyNewScrollSchdulePlan(ProductionScheduleMaster targetPlan)
        {
            ProductionScheduleMaster newPlan = new ProductionScheduleMaster();

            newPlan.ProjectId = targetPlan.ProjectId;
            newPlan.ProjectName = targetPlan.ProjectName;

            newPlan.CreateDate = DateTime.Now;

            Login login = CallContextUtil.LogicalGetData<Login>("LoginInformation") as Login;

            newPlan.HandlePerson = login.ThePerson;
            newPlan.HandlePersonName = login.ThePerson.Name;
            newPlan.OperOrgInfo = login.TheOperationOrgInfo;
            newPlan.OperOrgInfoName = login.TheOperationOrgInfo.Name;
            newPlan.OpgSysCode = login.TheOperationOrgInfo.SysCode;

            newPlan.DocState = VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
            newPlan.ScheduleType = targetPlan.ScheduleType;
            newPlan.ScheduleCaliber = targetPlan.ScheduleCaliber;
            newPlan.ScheduleTypeDetail = targetPlan.ScheduleTypeDetail;
            newPlan.ScheduleName = targetPlan.ScheduleName;

            var queryDtl = from d in targetPlan.Details
                           where d.ParentNode == null
                           select d;

            ProductionScheduleDetail dtl = queryDtl.ElementAt(0);

            ProductionScheduleDetail dtlNew = new ProductionScheduleDetail();
            dtlNew.Master = newPlan;
            newPlan.Details.Add(dtlNew);

            dtlNew.GWBSTree = dtl.GWBSTree;
            dtlNew.GWBSTreeName = dtl.GWBSTreeName;
            dtlNew.GWBSTreeSysCode = dtl.GWBSTreeSysCode;
            dtlNew.GWBSNodeType = dtl.GWBSNodeType;

            dtlNew.ParentNode = null;
            dtlNew.Level = 1;
            dtlNew.State = EnumScheduleDetailState.有效;
            dtlNew.OrderNo = 0;

            newPlan = SaveOrUpdateByDao(newPlan) as ProductionScheduleMaster;

            dtlNew.SysCode = dtlNew.Id + ".";

            dtlNew = SaveOrUpdateByDao(dtlNew) as ProductionScheduleDetail;

            CopyPlanDetail(dtl, targetPlan, dtlNew, newPlan);

            return newPlan;
        }

        /// <summary>
        /// 作废滚动进度计划明细
        /// </summary>
        /// <param name="planDtl">计划明细</param>
        /// <returns></returns>
        public bool InvalidScrollSchdulePlanDtl(ProductionScheduleDetail detail, out IList listChilds)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbTransaction tr = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();

            try
            {
                command.Transaction = tr;

                if (conn is SqlConnection)
                {
                    command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=12 WHERE SysCode like '" + detail.SysCode + "%' and state=11";
                }
                else
                {
                    command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=12 WHERE SysCode like '" + detail.SysCode + "%' and state=11";
                }

                command.ExecuteNonQuery();

                tr.Commit();

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Like("SysCode", detail.SysCode, MatchMode.Start));
                //oq.AddCriterion(Expression.Eq("State",EnumScheduleDetailState.有效));

                IEnumerable<ProductionScheduleDetail> listdtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

                listChilds = listdtl.ToList();
            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw new Exception(ex.Message);
            }

            return true;
        }

        /// <summary>
        /// 启用（设置生效）滚动进度计划明细
        /// </summary>
        /// <param name="detail">计划明细</param>
        /// <returns></returns>
        [TransManager]
        public bool EffectScrollSchdulePlanDtl(ProductionScheduleDetail detail, IList listEditDtl, out IList listChilds)
        {

            ArrayList listReturn = new ArrayList();//用于更新界面显示等

            DateTime baseTime = new DateTime(1900, 1, 1);
            for (int i = 0; i < listEditDtl.Count; i++)
            {
                ProductionScheduleDetail dtl = listEditDtl[i] as ProductionScheduleDetail;
                if (dtl.PlannedBeginDate == baseTime || dtl.PlannedEndDate == baseTime)
                    continue;

                dtl.State = EnumScheduleDetailState.有效;
            }

            dao.Update(listEditDtl);
            listReturn.AddRange(listEditDtl);



            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("SysCode", detail.SysCode, MatchMode.Start));
            oq.AddCriterion(Expression.Not(Expression.Eq("State", EnumScheduleDetailState.编辑)));

            IEnumerable<ProductionScheduleDetail> listdtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

            listReturn.AddRange(listdtl.ToList());

            var queryUpdate = from d in listdtl
                              where d.State == EnumScheduleDetailState.失效
                              select d;

            IList listUpdate = queryUpdate.ToList();
            for (int i = 0; i < listUpdate.Count; i++)
            {
                ProductionScheduleDetail dtl = listUpdate[i] as ProductionScheduleDetail;
                dtl.State = EnumScheduleDetailState.有效;
            }
            dao.Update(listUpdate);


            listChilds = listReturn;

            //ISession session = CallContext.GetData("nhsession") as ISession;
            //IDbConnection conn = session.Connection;
            //IDbTransaction tr = conn.BeginTransaction();
            //IDbCommand command = conn.CreateCommand();
            //try
            //{
            //    command.Transaction = tr;

            //    if (conn is SqlConnection)
            //    {
            //        //command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=11 WHERE SysCode like '" + detail.SysCode + "%' and (state=12 or (state=10 and plannedbegindate !='1900-1-1' and plannedenddate !='1900-1-1'))";
            //        command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=11 WHERE SysCode like '" + detail.SysCode + "%' and (state=12 or (state=10 and plannedbegindate !='1900-1-1' and plannedenddate !='1900-1-1'))";
            //    }
            //    else
            //    {
            //        //command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=11 WHERE SysCode like '" + detail.SysCode + "%' and (state=12 or (state=10 and plannedbegindate !=to_date('1900-1-1','yyyy-mm-dd') and plannedenddate !=to_date('1900-1-1','yyyy-mm-dd')))";
            //        command.CommandText = "UPDATE THD_ProductionScheduleDetail SET STATE=11 WHERE SysCode like '" + detail.SysCode + "%' and (state=12 or state=10)";
            //    }

            //    command.ExecuteNonQuery();

            //    tr.Commit();

            //    ObjectQuery oq = new ObjectQuery();
            //    oq.AddCriterion(Expression.Like("SysCode", detail.SysCode, MatchMode.Start));
            //    //oq.AddCriterion(Expression.Eq("State",EnumScheduleDetailState.有效));

            //    IEnumerable<ProductionScheduleDetail> listdtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq).OfType<ProductionScheduleDetail>();

            //    listChilds = listdtl.ToList();
            //}
            //catch (Exception ex)
            //{
            //    tr.Rollback();
            //    throw new Exception(ex.Message);
            //}

            return true;
        }

        //通过project修改计划明细
        public void UpdateSchdulePlanDtl(List<ProductionScheduleDetail> list)
        {
            try
            {
                ISession session = CallContext.GetData("nhsession") as ISession;
                IDbConnection conn = session.Connection;
                IDbCommand command = conn.CreateCommand();
                session.Transaction.Enlist(command);
                string sSQL = " update THD_ProductionScheduleDetail t set t.plannedbegindate=to_date('{0}','YYYY-MM -DD HH24:MI:SS'), t.plannedenddate=to_date('{1}','YYYY-MM -DD HH24:MI:SS') , t.plannedduration={2}   where t.id='{3}'";
                string sql = string.Empty;
                foreach (ProductionScheduleDetail detail in list)
                {
                    sql = string.Format(sSQL, detail.PlannedBeginDate.ToString("G"), detail.PlannedEndDate.ToString("G"), detail.PlannedDuration.ToString(), detail.Id);
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 查询滚动进度计划明细
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetScrollPlanDtl(ObjectQuery oq, string projectId)
        {
            IList listDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);

            if (listDtl.Count > 0)
            {
                //string sql = "select Id,Name,ParentNodeID,SysCode from THD_GWBSTree where TheProjectGUID='" + projectId + "' and state=1";
                //DataSet ds = SearchSQL(sql);

                //List<GWBSTree> list = new List<GWBSTree>();
                //foreach (DataRow row in ds.Tables[0].Rows)
                //{
                //    GWBSTree wbs = new GWBSTree();
                //    wbs.Id = row["Id"].ToString();
                //    wbs.Name = row["Name"].ToString();

                //    if (!string.IsNullOrEmpty(row["ParentNodeID"].ToString()))
                //    {
                //        wbs.ParentNode = new GWBSTree();
                //        wbs.ParentNode.Id = row["ParentNodeID"].ToString();
                //    }
                //    wbs.SysCode = row["SysCode"].ToString();

                //    list.Add(wbs);
                //}

                //for (int i = 0; i < listDtl.Count; i++)
                //{
                //    ProductionScheduleDetail dtl = listDtl[i] as ProductionScheduleDetail;
                //    dtl.GwbsFullPath = GetCategorTreeFullPath(typeof(GWBSTree), dtl.GWBSTreeName, dtl.GWBSTreeSysCode);// GetGWBSFullPath(list, dtl.GWBSTree.Id, dtl.GWBSTreeName, dtl.GWBSTreeSysCode);
                //}
            }
            return listDtl;
        }
        private string GetGWBSFullPath(List<GWBSTree> list, string wbsId, string wbsName, string gwbsSysCode)
        {
            IEnumerable<GWBSTree> listTemp = from g in list
                                             where gwbsSysCode.IndexOf(g.SysCode) > -1
                                             select g;

            string fullPath = wbsName;

            var query = from g in listTemp
                        where g.Id == wbsId
                        select g;

            GWBSTree wbs = null;
            if (query.Count() > 0)//找父节点
            {
                wbs = query.ElementAt(0);

                query = from g in listTemp
                        where wbs.ParentNode != null && g.Id == wbs.ParentNode.Id
                        select g;

                if (query.Count() > 0)
                    wbs = query.ElementAt(0);
                else
                    wbs = null;
            }

            while (wbs != null)
            {
                fullPath = wbs.Name + "\\" + fullPath;

                query = from g in listTemp
                        where wbs.ParentNode != null && g.Id == wbs.ParentNode.Id
                        select g;


                if (query.Count() > 0)
                    wbs = query.ElementAt(0);
                else
                    wbs = null;

            }

            return fullPath;
        }

        /// <summary>
        /// 获取分类对象的完整路径
        /// </summary>
        /// <param name="cateEntityType"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeSysCode"></param>
        /// <returns></returns>
        private string GetCategorTreeFullPath(Type cateEntityType, string nodeName, string nodeSysCode)
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
        /// 更新滚动计划  先删除删除节点
        /// </summary>
        /// <param name="oMaster"></param>
        /// <param name="listDtl"></param>
        /// <param name="oDelDtl"></param>
        /// <returns></returns>
        [TransManager]
        public ProductionScheduleMaster UpdateScrollSchedule(ProductionScheduleMaster oMaster, IList listDtl, ProductionScheduleDetail oDelDtl)
        {
            bool bFlag = true;

            try
            {
                //IList lstDelDtl = new ArrayList();
                RemoveScrollSchedule(oMaster, oDelDtl);
                // bFlag= dao.Delete(lstDelDtl);
                if (bFlag)
                {
                    if (listDtl == null || listDtl.Count == 0)
                        return null;

                    dao.Save(listDtl);


                    //设置明细的Syscode
                    IEnumerable<ProductionScheduleDetail> queryPlanDtl = listDtl.OfType<ProductionScheduleDetail>();

                    int level = queryPlanDtl.Min(d => d.Level);

                    queryPlanDtl = from d in queryPlanDtl
                                   where d.Level == level
                                   select d;

                    ProductionScheduleDetail dtl = queryPlanDtl.ElementAt(0) as ProductionScheduleDetail;
                    foreach (ProductionScheduleDetail d in listDtl)
                    {
                        if (d.ParentNode.Id == dtl.ParentNode.Id)
                        {
                            d.SysCode = dtl.ParentNode.SysCode + d.Id + ".";

                            SetPlanDtlSysCode(d, listDtl);
                        }
                    }
                    dao.Update(listDtl);
                    //生成syscode
                    //UpdateScrollPlanDtlSyscode(dtl.Master.Id);
                    //if (oDelDtl != null)
                    //{
                    //    ObjectQuery oq1 = new ObjectQuery();
                    //    oq1.AddCriterion(Expression.Like("SysCode", oDelDtl.SysCode, MatchMode.Start));
                    //    IList listdtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq1);
                    //    Dao.Delete(listdtl);       
                    //}
                    //return oMaster;

                }
            }
            catch (System.Exception ex)
            {

            }
            return oMaster;
        }

        public void RemoveScrollSchedule(ProductionScheduleMaster oMaster, ProductionScheduleDetail oDelDtl)
        {
            if (oDelDtl != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Like("SysCode", oDelDtl.SysCode, MatchMode.Start));
                //oq.AddCriterion(Expression.Eq("State", EnumScheduleDetailState.有效));

                IList listdtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                Dao.Delete(listdtl);
                //ISession session = CallContext.GetData("nhsession") as ISession;
                //IDbConnection conn = session.Connection;
                //// IDbTransaction tr = conn.BeginTransaction();
                //IDbCommand command = conn.CreateCommand();
                //string sSQL = "Delete FROM THD_ProductionScheduleDetail WHERE syscode like '{0}%'";
                //sSQL = string.Format(sSQL, oDelDtl.SysCode);
                //command.CommandText = sSQL;
                //command.ExecuteNonQuery();
            }
        }

        public IList GetProChilds(IList list)
        {

            ArrayList listPro = new ArrayList();

            IEnumerable<ProductionScheduleDetail> listDtl = list.OfType<ProductionScheduleDetail>();

            var queryDtl = from d in listDtl
                           where d.ParentNode == null && d.Level == 1
                           orderby d.Level, d.OrderNo ascending
                           select d;


            listPro.AddRange(queryDtl.ToArray());

            getChildDtl(queryDtl.ElementAt(0), listDtl, ref listPro);

            return list;
        }

        private void getWeekChildDtl(WeekScheduleDetail parentDtl, IEnumerable<WeekScheduleDetail> listDtl, ref ArrayList listResult)
        {
            var queryDtl = from d in listDtl
                           where d.ParentNode == parentDtl
                           orderby d.OrderNo ascending
                           orderby d.Level ascending
                           select d;
            parentDtl.ChildCount = queryDtl.Count();

            foreach (WeekScheduleDetail dtl in queryDtl)
            {
                listResult.Add(dtl);

                getWeekChildDtl(dtl, listDtl, ref listResult);
            }
        }

        public IList GetWeekChilds(WeekScheduleMaster master)
        {
            ArrayList list = new ArrayList();

            if (master == null)
                return list;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));

            var listDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq).OfType<WeekScheduleDetail>();

            var queryDtl = from d in listDtl
                           where d.ParentNode == null && d.Level == 1
                           orderby d.Level, d.OrderNo ascending
                           select d;

            list.AddRange(queryDtl.ToArray());

            getWeekChildDtl(queryDtl.ElementAt(0), listDtl, ref list);

            return list;
        }

        public void DeleteWeekScheduleDetail(WeekScheduleMaster master)
        {

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            IList listdtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq);
            if (listdtl.Count == 0) return;
            dao.Delete(listdtl);

            //childCount = listdtl.Count - 1;

            //ISession session = CallContext.GetData("nhsession") as ISession;
            //IDbConnection conn = session.Connection;
            //IDbTransaction tr = conn.BeginTransaction();
            //IDbCommand command = conn.CreateCommand();
            //try
            //{
            //    command.Transaction = tr;

            //    //if (conn is SqlConnection)
            //    //{
            //    //    command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=@detailId";
            //    //}
            //    //else
            //    //{
            //    //    command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=:detailId";
            //    //}
            //    //IDbDataParameter p_detailId = command.CreateParameter();
            //    //p_detailId.Value = detail.Id;
            //    //p_detailId.ParameterName = "detailId";
            //    //command.Parameters.Add(p_detailId);
            //    //command.ExecuteNonQuery();

            //    if (listdtl != null && listdtl.Count > 0)
            //    {
            //        foreach (ProductionScheduleDetail tempDetail in listdtl)
            //        {
            //            command.Parameters.Clear();
            //            if (conn is SqlConnection)
            //            {
            //                command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=@Id";
            //            }
            //            else
            //            {
            //                command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=:Id";
            //            }
            //            IDbDataParameter p_Id = command.CreateParameter();
            //            p_Id.Value = tempDetail.Id;
            //            p_Id.ParameterName = "Id";
            //            command.Parameters.Add(p_Id);
            //            command.ExecuteNonQuery();
            //        }
            //    }
            //    tr.Commit();
            //}
            //catch (Exception ex)
            //{
            //    tr.Rollback();
            //    throw new Exception(ex.Message);
            //}
        }

        /// <summary>
        /// 删除一条进度计划明细 并删除其下的所有子节点
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="errMsg">异常信息</param>
        public IList DeleteWeekScheduleDetail(WeekScheduleDetail detail, int childCount, string errMsg)
        {
            childCount = 0;
            errMsg = "";

            IList list = new ArrayList();
            list.Add(errMsg);
            list.Add(childCount);

            //IList lst = new ArrayList();

            if (detail == null || string.IsNullOrEmpty(detail.Id))
            {
                errMsg = "要删除的计划节点为空,请指定要删除的计划节点.";
                list[0] = errMsg;
                return list;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", detail.Master.Id));
            oq.AddCriterion(Expression.Like("SysCode", detail.SysCode, MatchMode.Start));
            IList listdtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq);

            childCount = listdtl.Count - 1;

            //lst.Add(childCount);
            list[1] = childCount;

            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbTransaction tr = conn.BeginTransaction();
            IDbCommand command = conn.CreateCommand();
            try
            {
                command.Transaction = tr;

                //if (conn is SqlConnection)
                //{
                //    command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=@detailId";
                //}
                //else
                //{
                //    command.CommandText = "Delete FROM THD_ProductionScheduleDetail WHERE Id=:detailId";
                //}
                //IDbDataParameter p_detailId = command.CreateParameter();
                //p_detailId.Value = detail.Id;
                //p_detailId.ParameterName = "detailId";
                //command.Parameters.Add(p_detailId);
                //command.ExecuteNonQuery();

                if (listdtl != null && listdtl.Count > 0)
                {
                    foreach (WeekScheduleDetail tempDetail in listdtl)
                    {
                        command.Parameters.Clear();
                        if (conn is SqlConnection)
                        {
                            command.CommandText = "Delete FROM THD_WeekScheduleDetail WHERE Id=@Id";
                        }
                        else
                        {
                            command.CommandText = "Delete FROM THD_WeekScheduleDetail WHERE Id=:Id";
                        }
                        IDbDataParameter p_Id = command.CreateParameter();
                        p_Id.Value = tempDetail.Id;
                        p_Id.ParameterName = "Id";
                        command.Parameters.Add(p_Id);
                        command.ExecuteNonQuery();
                    }
                }
                tr.Commit();
                return list;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 返回1.核算明细集，2.确认单明细
        /// </summary>
        /// <param name="oqAccount"></param>
        /// <param name="oqConfirmMaster"></param>
        /// <param name="oqConfirm"></param>
        /// <returns></returns>
        public IList GetTaskConfirmInfo(ObjectQuery oqAccountMaster, ObjectQuery oqAccountDtl, ObjectQuery oqConfirmMaster, ObjectQuery oqConfirm)
        {
            IList listResult = new ArrayList();

            IList listConfirmMaster = dao.ObjectQuery(typeof(GWBSTaskConfirmMaster), oqConfirmMaster);
            if (listConfirmMaster.Count == 0)
                return listResult;

            GWBSTaskConfirmMaster master = listConfirmMaster[0] as GWBSTaskConfirmMaster;
            oqConfirm.AddCriterion(Expression.Eq("Master.Id", master.Id));

            IList listConfirmDtl = dao.ObjectQuery(typeof(GWBSTaskConfirm), oqConfirm);


            IList listAccountMaster = dao.ObjectQuery(typeof(ProjectTaskAccountBill), oqAccountMaster);
            Disjunction disAccountDtl = new Disjunction();
            foreach (ProjectTaskAccountBill bill in listAccountMaster)
            {
                disAccountDtl.Add(Expression.Eq("TheAccountBill.Id", bill.Id));
            }
            oqAccountDtl.AddCriterion(disAccountDtl);

            IList listAccountDtl = dao.ObjectQuery(typeof(ProjectTaskDetailAccount), oqAccountDtl);


            listResult.Add(listAccountDtl);
            listResult.Add(listConfirmDtl);
            return listResult;
        }

        public IList GetFrontSchedule(ObjectQuery oq, string projectId)
        {
            IList listDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq);
            return listDtl;
        }


        public void DeleteProjectDelayInfo(string projId)
        {
            #region   old code 不知道为毛删不掉
            //ObjectQuery objectQuery = new ObjectQuery();
            //objectQuery.AddCriterion(Expression.Eq("CreateDate", DateTime.Now.Date));
            //if (!string.IsNullOrEmpty(projId))
            //{
            //    objectQuery.AddCriterion(Expression.Eq("ProjectId", projId));
            //}

            //var list = Dao.ObjectQuery(typeof(DurationDelayWarn), objectQuery);
            //if (list != null && list.Count > 0)
            //{
            //    DeleteByDao(list);
            //}
            #endregion
            string sqlwhereProId = "";
            if (!string.IsNullOrEmpty(projId))
            {
                sqlwhereProId += string.Format(" and t1.ProjectId = '{0}' ", projId);
            }
            string sql = string.Format("delete from THD_DurationDelayWarn t1 where t1.createdate = to_date('{0}','yyyy-MM-dd') {1}  ", DateTime.Now.Date.ToString("yyyy-MM-dd"), sqlwhereProId);
            ExecuteSql(sql);

        }
        [TransManager]
        public bool CreateProjectDelayDays(string projId)
        {
            DeleteProjectDelayInfo(projId);
            #region old code
            //var sql =
            //    string.Format(SqlScript.GetProjectDelaysSql,
            //                  string.IsNullOrEmpty(projId) ? string.Empty : " and t3.theprojectguid = '" + projId + "' ");

            //// 计算明细延期数据
            //var ds = QueryDataToDataSet(sql);
            //if (ds == null || ds.Tables.Count == 0)
            //{
            //    return false;
            //}

            //var insertList = new List<DurationDelayWarn>();
            //var dt = ds.Tables[0];
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    var item = new DurationDelayWarn();
            //    item.CostDetail = dt.Rows[i]["costRemark"].ToString();
            //    item.CreateDate = Convert.ToDateTime(dt.Rows[i]["createDate"]);
            //    item.DelayCosts =
            //        dt.Rows[i]["totalPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["totalPrice"]);
            //    item.DelayDays =
            //        dt.Rows[i]["delayDays"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["delayDays"]);
            //    item.ModifyTime = DateTime.Now;
            //    item.OrgSyscode = dt.Rows[i]["ownerorgsyscode"].ToString();
            //    item.OwnerOrg = dt.Rows[i]["opgname"].ToString();
            //    item.PlanBeginDate = Convert.ToDateTime(dt.Rows[i]["taskplanstarttime"]);
            //    item.PlanEndDate = Convert.ToDateTime(dt.Rows[i]["taskplanendtime"]);
            //    item.PlanRate =
            //        dt.Rows[i]["planRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["planRate"]);
            //    item.PlanTime =
            //        dt.Rows[i]["palntime"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["palntime"]);
            //    //item.ProjectDelayDays =
            //    //     dt.Rows[i]["projectDelayDays"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["projectDelayDays"]);
            //    item.ProjectId = dt.Rows[i]["theprojectguid"].ToString();
            //    item.ProjectName = dt.Rows[i]["theprojectname"].ToString();
            //    if (dt.Rows[i]["realstartdate"] != DBNull.Value)
            //    {
            //        item.RealBeginDate = Convert.ToDateTime(dt.Rows[i]["realstartdate"]);
            //    }

            //    item.RealRate =
            //        dt.Rows[i]["addupfigureprogress"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["addupfigureprogress"]);
            //    item.Task = GetObjectById(typeof(GWBSTree), dt.Rows[i]["taskId"].ToString()) as GWBSTree;
            //    item.TaskFullPath = dt.Rows[i]["fullpath"].ToString();
            //    item.TaskName = dt.Rows[i]["taskName"].ToString();
            //    item.WarnLevel = dt.Rows[i]["WarnLevel"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["WarnLevel"]);
            //    item.IsProjectDelay = false;
            //    insertList.Add(item);
            //}
            #endregion

            //增加项目延期数据
            ObjectQuery oq = new ObjectQuery();

            if (!string.IsNullOrEmpty(projId))
                oq.AddCriterion(Expression.Eq("ProjectId", projId));
            oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.总体进度计划));
            oq.AddOrder(Order.Desc("CreateDate"));

            oq.AddFetchMode("Details", FetchMode.Eager);
            //oq.AddFetchMode("Details.Details", FetchMode.Eager);
            oq.AddFetchMode("Details.RalationDetails", FetchMode.Eager);

            IList listMaster = Dao.ObjectQuery(typeof(WeekScheduleMaster), oq);
            
            var insertList = new List<DurationDelayWarn>();
            List<DurationDelayWarn> listProjectDelayinfo = (new DurationDelayHelper()).GetProjectDelayInfo(listMaster, projId);

            if (listProjectDelayinfo != null && listProjectDelayinfo.Count > 0)
            {
                foreach (var item in listProjectDelayinfo)
                {
                    insertList.Add(item);
                }
            }
          
            return Dao.Save(insertList);
        }

        public IList GetProjectTotalDelayDays(string projId, DateTime beginDate, DateTime endDate)
        {
            var sql = string.Format(SqlScript.GetProjectTotalDelaysSql
                                    , projId
                                    , beginDate.ToString("yyyy-MM-dd")
                                    , endDate.ToString("yyyy-MM-dd"));

            var ds = QueryDataToDataSet(sql);
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }

            var dt = ds.Tables[0];
            var list = new List<DurationDelayWarn>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new DurationDelayWarn();
                item.ProjectId = dt.Rows[i]["projectid"].ToString();
                item.ProjectName = dt.Rows[i]["projectname"].ToString();
                item.CreateDate = Convert.ToDateTime(dt.Rows[i]["CreateDate"]);
                item.ProjectDelayDays = Convert.ToDecimal(dt.Rows[i]["projectdelaydays"]);
                item.DelayCosts = Convert.ToDecimal(dt.Rows[i]["delaycosts"]);

                list.Add(item);
            }

            return list;
        }

        public IList GetGWBSTreesByInstance(string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddOrder(Order.Asc("Level"));
            oq.AddOrder(Order.Asc("OrderNo"));
            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);

            return list;
        }

        public IList GetGWBSTreesOQ(ObjectQuery oq)
        {
            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);
            return list;
        }

        public IList GetGWBSTreesRoot(string projectId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectId));
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddCriterion(Expression.Eq("Level", 1));
            IList list = dao.ObjectQuery(typeof(GWBSTree), oq);

            return list;
        }

        public IList GetShowWeekScheduleDetails(ObjectQuery oq)
        {
            oq.AddFetchMode("RalationDetails", FetchMode.Eager);
            IList listDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq);
            return listDtl;
        }


        public bool DeleteByIList(IList lstObj)
        {
            return DeleteByDao(lstObj);
        }

        public void UpdateWeekScheduleMasterState(DocumentState dc,WeekScheduleMaster wsm)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            string sql =string.Format( @"update thd_weekschedulemaster t1 
                              set t1.state={0}
                            where t1.id = '{1}' ",ClientUtil.ToInt(dc), wsm.Id);
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        public string GetAssignTeamLinkMan(string assignTeam)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            string sql = string.Format(@"select t3.linkman
  from thd_subcontractproject t1 
       left join ressupplierrelation t2 on t1.bearerorgguid = t2.suprelid
       left join resorganization t3 on t2.orgid = t3.orgid
       where t1.id = '{0}';", assignTeam);
            command.CommandText = sql;
           object obj =  command.ExecuteScalar();

           return (obj == null || obj == DBNull.Value) ? "" : ClientUtil.ToString(obj);
        }
    }
}
