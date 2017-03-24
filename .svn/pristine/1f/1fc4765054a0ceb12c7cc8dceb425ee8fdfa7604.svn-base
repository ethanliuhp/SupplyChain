using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using VirtualMachine.Core;
using NHibernate.Criterion;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.FinancialResource;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public class MGWBSTree
    {
        private static IGWBSTreeSrv model;
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IInspectionLotSrv modelIns;
        private static IProductionManagementSrv modelInspection;
        public MGWBSTree()
        {
            if (model == null)
                model = ConstMethod.GetService("GWBSTreeSrv") as IGWBSTreeSrv;
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (modelIns == null)
                modelIns = ConstMethod.GetService("InspectionLotSrv") as IInspectionLotSrv;
            if (modelInspection == null)
                modelInspection = ConstMethod.GetService("ProductionManagementSrv") as IProductionManagementSrv;
        }
        public GWBSTaskConfirm SaveGWBSTaskConfirm(GWBSTaskConfirm dtl)
        {
            return modelInspection.SaveGWBSTaskConfirm(dtl);
        }
        public IList SaveGWBSTaskConfirm(IList lst)
        {
            return modelInspection.SaveGWBSTaskConfirm(lst);
        }
        public IList SaveGWBSTaskConfirm1(IList lst)
        {
            return modelInspection.SaveGWBSTaskConfirm1(lst);
        }
        public IList SaveGWBSTaskConfirmNow(IList lst)
        {
            return modelInspection.SaveGWBSTaskConfirmNow(lst);
        }
        public DataSet SearchGWBSDetail(string sGWBSTreeSysCode, string sProjectID)
        {
            return modelInspection.SearchGWBSDetail(sGWBSTreeSysCode, sProjectID);
        }
        public DataSet SearchGWBSDetailNow(string sGWBSTreeSysCode, string sProjectID)
        {
            return modelInspection.SearchGWBSDetailNow(sGWBSTreeSysCode, sProjectID);
        }
        public IList SearchGWBSDetail(string strSysCode)
        {
            return modelInspection.SearchGWBSDetail(strSysCode);
        }
        public IList SearchGWBSDetail(GWBSTree oGWBSTree, PersonInfo oLoginPersonInfo, OperationOrgInfo oTheOperationOrgInfo)
        {
            return modelInspection.SearchGWBSDetail(oGWBSTree, oLoginPersonInfo, oTheOperationOrgInfo);
        }

        public List<GWBSDetail> SearchGWBSDetailByContractGroupId(string contractGroupId)
        {
            string sql = "select t2.quotacode,t1.id,t1.name,t1.mainresourcetypename,t1.ContractProjectName," +
                         "t1.mainresourcetypespec,t1.diagramnumber,t1.workamountunitname," +
                         "t1.planworkamount,t1.planprice,t1.plantotalprice," +
                         "t1.RESPONSIBILITILYWORKAMOUNT, t1.RESPONSIBILITILYPRICE, t1.RESPONSIBILITILYTOTALPRICE, " +
                         "t1.CONTRACTPROJECTQUANTITY, t1.CONTRACTPRICE, t1.CONTRACTTOTALPRICE," +
                         "t1.THEGWBSFULLPATH," +
                         "t1.priceunitname,t1.state,t1.contentdesc,t1.orderno,t1.code,t1.changeparentid,T1.CONTRACTGROUPNAME,t1.QuantityConfirmed from thd_gwbsdetail t1 inner join thd_costitem t2 on t1.costitemguid=t2.id where t1.contractgroupguid='" + contractGroupId + "' order by t1.orderno";

            DataSet ds = SearchSQL(sql);

            List<GWBSDetail> listDtl = new List<GWBSDetail>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                GWBSDetail dtl = new GWBSDetail();

                CostItem costItem = new CostItem();
                costItem.QuotaCode = row["quotacode"].ToString();
                dtl.TheCostItem = costItem;

                dtl.Id = row["id"].ToString();
                dtl.Name = row["name"].ToString();
                dtl.OrderNo = ClientUtil.ToInt(row["orderno"]);
                dtl.MainResourceTypeName = row["mainresourcetypename"].ToString();
                dtl.MainResourceTypeSpec = row["mainresourcetypespec"].ToString();
                dtl.DiagramNumber = row["diagramnumber"].ToString();
                dtl.WorkAmountUnitName = row["workamountunitname"].ToString();

                dtl.ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(row["RESPONSIBILITILYWORKAMOUNT"]);
                dtl.ResponsibilitilyPrice = ClientUtil.ToDecimal(row["RESPONSIBILITILYPRICE"]);
                dtl.ResponsibilitilyTotalPrice = ClientUtil.ToDecimal(row["RESPONSIBILITILYTOTALPRICE"]);

                dtl.ContractProjectQuantity = ClientUtil.ToDecimal(row["CONTRACTPROJECTQUANTITY"]);
                dtl.ContractPrice = ClientUtil.ToDecimal(row["CONTRACTPRICE"]);
                dtl.ContractTotalPrice = ClientUtil.ToDecimal(row["CONTRACTTOTALPRICE"]);

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
                dtl.TheGWBSFullPath = row["THEGWBSFULLPATH"] + "";
                listDtl.Add(dtl);
            }

            return listDtl;
        }


        public bool DeleteGWBSTaskConfirm(GWBSTaskConfirm confirm)
        {
            return modelInspection.DeleteGWBSTaskConfirm(confirm);
        }

        public GWBSTaskConfirmMaster SaveGWBSTaskConfirmMaster(GWBSTaskConfirmMaster conMaster)
        {
            return modelInspection.SaveGWBSTaskConfirmMaster(conMaster);
        }

        public bool DeleteInspectionRecord(InspectionRecord obj)
        {
            return modelInspection.DeleteInspectionRecord(obj);
        }
        public IList GetInsRecord(ObjectQuery oq)
        {
            return modelInspection.GetInspectionRecord(oq);
        }
        public InspectionRecord SaveInspectialRecordMaster(InspectionRecord obj)
        {
            return modelInspection.SaveInsMaster(obj);
        }

        public InspectionRecord SaveInspectialRecordMaster(InspectionRecord obj,bool isSubmit)
        {
            return modelInspection.SaveInsMaster(obj, isSubmit);
        }

        public IList GetInspectionRecordQuery(ObjectQuery oq)
        {
            return modelInspection.GetInspectionRecordMaster(oq);
        }
        //通过Id查找检验批信息
        public bool DeleteByDao(InspectionLot obj)
        {
            return modelIns.DeleteByDao(obj);
        }
        //IList GetInspectionLot(ObjectQuery objectQuery)
        //查找检验批信息
        public IList GetInspectionLot(ObjectQuery objectQuery)
        {
            return modelIns.GetInspectionLot(objectQuery);
        }

        //保存检验批
        public InspectionLot SaveInspectionLot(InspectionLot obj)
        {
            return modelIns.SaveInspectionLot(obj);
        }

        //返回节点node所在树的当前层次的最大OrderNo
        public long GetMaxOrderNo(CategoryNode node)
        {
            return model.GetMaxOrderNo(node, null);
        }

        public IList SaveGWBSTreeRootNode(IList list)
        {
            return model.SaveGWBSTreeRootNode(list);
        }

        //保存组织集合
        public IList SaveGWBSTrees(IList lst)
        {
            return model.SaveGWBSTrees(lst);
        }

        public IList GWBSTreeOrder(IList list)
        {
            return model.GWBSTreeOrder(list);
        }

        public List<GWBSDetail> GetWBSDetail(string wbsId, string sqlWhere)
        {
            return model.GetWBSDetail(wbsId, sqlWhere);
        }

        /// <summary>
        /// 保存复制的任务明细到任务节点
        /// </summary>
        /// <param name="wbsNode"></param>
        /// <param name="listDtl"></param>
        /// <returns></returns>
        public IList SaveCopyDetailByNode(ContractGroup selectedContractGroup, GWBSTree wbsNode, List<GWBSDetail> listDtl)
        {
            return model.SaveCopyDetailByNode(selectedContractGroup, wbsNode, listDtl);
        }

        public bool SaveCopyDetailByNode(ContractGroup selectedContractGroup, List<GWBSTree> listWBSNode, List<GWBSDetail> listDtl)
        {
            return model.SaveCopyDetailByNode(selectedContractGroup, listWBSNode, listDtl);
        }

        /// <summary>
        /// 插入（部分属性固定）或修改WBS树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        public GWBSTree InsertOrUpdateWBSTree(GWBSTree childNode)
        {
            return model.InsertOrUpdateWBSTree(childNode);
        }

        /// <summary> 
        /// 插入（部分属性固定）或修改WBS节点集合
        /// </summary>
        public IList InsertOrUpdateWBSTrees(IList lst)
        {
            return model.InsertOrUpdateWBSTrees(lst);
        }

        /// <summary> 
        /// 插入（部分属性固定）或修改WBS节点集合
        /// </summary>
        public IList InsertOrUpdateWBSTrees(IList lst, IList listVerify)
        {
            return model.InsertOrUpdateWBSTrees(lst, listVerify);
        }

        public Hashtable MoveNode(GWBSTree fromNode, GWBSTree toNode)
        {
            return model.MoveGWBSTree(fromNode, toNode);
        }
        /// <summary>
        /// 根据名称查找业务组织
        /// </summary>
        public GWBSTree GetOpeOrgByName(string name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("Name", "%" + name + "%"));
            IList list = model.GetGWBSTrees(typeof(GWBSTree), oq);
            if (list.Count == 1)
                return list[0] as GWBSTree;
            else
                return null;
        }
        /// <summary>
        /// 根据该节点syscode 查找它的父节点是否含有临建类型的工程wbs节点
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sTrreNodeSysCode"></param>
        /// <returns></returns>
        public bool IsTempBuildProjectTypeNode(string sProjectID, string sTrreNodeSysCode)
        {
            return modelInspection.IsTempBuildProjectTypeNode(sProjectID, sTrreNodeSysCode);
        }
        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetGWBSTreesByInstance(string projectId)
        {
            return model.GetGWBSTreesByInstance(projectId);
        }
        public DataTable GetGWBSTree(string sProjectID, string sParentID, int iLevel)
        {
            return model.GetGWBSTrees(sProjectID, sParentID, iLevel);
        }
        public IList GetGWBSTreesByLevel(string projectId, int iLevel, string sParentSysCode)
        {
            return model.GetGWBSTreesByLevel(projectId, iLevel, sParentSysCode);
        }
        public DataSet GetGWBSTreesByInstanceSql(string projectId)
        {
            return model.GetGWBSTreesByInstanceSql(projectId);
        }
        public DataSet GetGWBSTreesByInstanceSql(string projectId, string sParentID, string sName)
        {
            return model.GetGWBSTreesByInstanceSql(projectId, sParentID, sName);
        }
        public void SaveBatchGWBSTreeName(IList lstGWBSTree)
        {
            model.SaveBatchGWBSTreeName(lstGWBSTree);
        }
        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetGWBSTreesByInstance(string projectId, string sSysCode, int iLevel)
        {
            return model.GetGWBSTreesByInstance(projectId, sSysCode, iLevel);
        }
        public IList GetGWBSTrees(Type t)
        {
            return model.GetGWBSTrees(t);
        }
        public IList GetGWBSTreeAndJobs(Type t)
        {
            return model.GetGWBSTreeAndJobs(t);
        }


        public GWBSTree GetGWBSTreeById(string id)
        {
            //GWBSTree org = mm.GetGWBSTreeById(id);
            //return org;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = model.GetGWBSTrees(typeof(GWBSTree), oq);
            if (list.Count == 1)
                return list[0] as GWBSTree;
            else
                return null;
        }


        public GWBSTree SaveGWBSTree(GWBSTree childOrg)
        {
            return model.SaveGWBSTree(childOrg);
        }
        public GWBSTree SaveGWBSTree(GWBSTree childOrg, IList list)
        {
            return model.SaveGWBSTree(childOrg, list);
        }


        public bool DeleteGWBSTree(GWBSTree childOrg)
        {
            return model.DeleteGWBSTree(childOrg);
        }

        public bool DeleteGWBSTree(IList lst)
        {
            return model.DeleteGWBSTree(lst);
        }

        public bool InitRule()
        {
            return model.InitRule();
        }


        public IList GetGWBSTreeRules(Type treeType)
        {
            return model.GetGWBSTreeRules(treeType);
        }


        public IList GetGWBSTreeRules(Type treeType, Type parentNodeType)
        {
            return model.GetGWBSTreeRules(treeType, parentNodeType);
        }


        public string GetNextOPGCode()
        {
            return model.GetNextOPGCode();
        }

        public DataSet SearchSQL(string sql)
        {
            return model.SearchSQL(sql);
        }

        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return model.GetObjectById(entityType, Id);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return model.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 对象查询并加载其完整路径
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetGWBSTreeAndFullPath(ObjectQuery oq)
        {
            return model.GetGWBSTreeAndFullPath(oq);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return model.GetServerTime();
        }

        /// <summary>
        /// 保存或更新GWBS,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        public GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, IList listLedger)
        {
            return model.SaveOrUpdateGWBSTree(wbs, listLedger);
        }

        /// <summary>
        /// 保存或更新工程WBS集合,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点集合</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateGWBSTree(IList list, IList listLedger)
        {
            return model.SaveOrUpdateGWBSTree(list, listLedger);
        }

        #region 任务明细操作

        /// <summary>
        /// 保存或修改工程WBS明细（1.任务明细，2.明细变更台账）
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>1.任务明细，2.明细变更台账</returns>
        public IList SaveOrUpdateDetail(GWBSDetail dtl, IList listLedger)
        {
            return model.SaveOrUpdateDetail(dtl, listLedger);
        }

        /// <summary>
        ///  变更工程WBS明细（1.任务明细，2.明细变更台账）
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>1.任务明细，2.明细变更台账</returns>
        public IList ChangeTaskDetail(GWBSDetail dtl, IList listLedger)
        {
            return model.ChangeTaskDetail(dtl, listLedger);
        }

        /// <summary>
        /// 保存或修改工程WBS明细集,返回对象集合（1.任务明细集，2.资源耗用集）
        /// </summary>
        /// <param name="dtl">明细对象集合</param>
        /// <param name="dtl">明细耗用对象集合</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>返回对象集合（1.任务明细集，2.资源耗用集）</returns>
        public IList SaveOrUpdateDetail(IList listDtl, IList listDtlUsage, IList listLedger)
        {
            return model.SaveOrUpdateDetail(listDtl, listDtlUsage, listLedger);
        }

        /// <summary>
        /// 保存或修改任务明细和父对象
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="parentNode">父对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>返回对象集合（1.任务明细，2.父节点）</returns>
        public IList SaveOrUpdateDetail(GWBSDetail dtl, GWBSTree parentNode, IList listLedger)
        {
            return model.SaveOrUpdateDetail(dtl, parentNode, listLedger);
        }

        /// <summary>
        /// 保存或修改工程GWBS和GWBS明细
        /// </summary>
        /// <param name="dtl">项目任务集</param>
        /// <param name="dtl">任务明细集</param>
        /// <returns>返回对象集合（1.项目任务集,2.任务明细集）</returns>
        public IList SaveOrUpdateDetail(IList listGWBSNode, IList listGWBSDtl, bool isReturnValue)
        {
            return model.SaveOrUpdateDetail(listGWBSNode, listGWBSDtl, isReturnValue);
        }

        /// <summary>
        /// 保存或修改工程WBS明细集
        /// </summary>
        /// <param name="dtl">明细对象集合</param>
        /// <param name="dtl">明细耗用对象集合</param>
        /// <returns>返回对象集合（1.任务明细集，2.资源耗用集）</returns>
        public IList SaveOrUpdateDetailByCostEdit(IList listDtl, IList listLedger, IList listDtlUsage, List<string> listDeleteDtlUsages)
        {
            return model.SaveOrUpdateDetailByCostEdit(listDtl, listLedger, listDtlUsage, listDeleteDtlUsages);
        }
        /// <summary>
        /// 保存或修改工程WBS明细集合
        /// </summary>
        /// <param name="list">明细集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateDetail(IList list)
        {
            return model.SaveOrUpdateDetail(list);
        }

        /// <summary>
        /// 删除工程WBS明细集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteDetail(IList list)
        {
            return model.DeleteDetail(list);
        }
        #endregion

        /// <summary>
        /// 保存或修改工程WBS明细集合
        /// </summary>
        /// <param name="list">明细集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateCostSubject(IList list)
        {
            return model.SaveOrUpdateCostSubject(list);
        }

        /// <summary>
        /// 保存或更新工程WBS集合,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateGWBSTree(IList list)
        {
            return model.SaveOrUpdateGWBSTree(list);
        }

        /// <summary>
        /// 删除明细分科目成本集合
        /// </summary>
        /// <param name="list">分科目成本集合</param>
        /// <returns></returns>
        public bool DeleteCostSubject(IList list)
        {
            return model.DeleteCostSubject(list);
        }

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type)
        {
            return model.GetCode(type);
        }


        #region 文档操作
        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        public IList SaveOrUpdate(IList list)
        {
            return modelDoc.SaveOrUpdate(list);
        }


        ///// <summary>
        ///// 保存或修改对象
        ///// </summary>
        ///// <param name="list">对象集合</param>
        ///// <returns></returns>
        //public object SaveOrUpdate(object obj)
        //{
        //    return modelDoc.SaveOrUpdate(obj);
        //}

        /// <summary>
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        public ProObjectRelaDocument SaveOrUpdateProObjRelaDoc(ProObjectRelaDocument item)
        {
            return modelDoc.SaveOrUpdate(item);
        }

        /// <summary>
        /// 删除工程对象关联文档对象集合
        /// </summary>
        /// <param name="list">工程对象关联文档对象集合</param>
        /// <returns></returns>
        public bool DeleteProObjRelaDoc(IList list)
        {
            return modelDoc.DeleteProObjRelaDoc(list);
        }
        #endregion

        #region 工程任务类型文档模版
        /// <summary>
        /// 保存或修改工程任务类型文档模版
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ProTaskTypeDocumentStencil SaveOrUpDateDocStencil(ProTaskTypeDocumentStencil obj)
        {
            return model.SaveOrUpDateDocStencil(obj);
        }
        #endregion


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Delete(IList list)
        {
            return model.Delete(list);
        }



        /// <summary>
        /// 根据父节点层次码查找其子节点
        /// </summary>
        /// <param name="ParentNodeId"></param>
        /// <returns></returns>
        public IList GetGWBSTreeByParentNodeSyscode(string parentNodeSyscode)
        {
            return model.GetGWBSTreeByParentNodeSyscode(parentNodeSyscode);
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
            return model.GetCostAccDtl(oq, accSubject, matCate, mat, usageName);
        }

        /// <summary>
        /// 计算分包取费任务明细的预算成本
        /// </summary>
        /// <param name="listDtl">分包取费明细集</param>
        /// <returns></returns>
        public IList AccountSubContractFeeDtl(IList listDtl, IList listDtlUsage, DocumentState state)
        {
            return model.AccountSubContractFeeDtl(listDtl, listDtlUsage, state);
        }

        /// <summary>
        /// 计算分包取费任务明细的预算成本
        /// </summary>
        /// <param name="listDtl">分包取费明细集</param>
        /// <returns></returns>
        public IList AccountSubContractFeeDtl(IList listDtl, IList listDtlUsage)
        {
            return model.AccountSubContractFeeDtl(listDtl, listDtlUsage);
        }
        /// <summary>
        /// 根据wbs任务类型得到工程文档验证
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        public IList GetDocumentTemplatesByTaskType(GWBSTree wbs)
        {
            return model.GetDocumentTemplatesByTaskType(wbs);
        }
        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        public IList SaveGWBSTreeRootNode1(IList lst)
        {
            return model.SaveGWBSTreeRootNode1(lst);
        }
        /// <summary>
        /// 保存一个节点加上其子节点集合
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public IList SaveGWBSTrees1(IList lst)
        {
            return model.SaveGWBSTrees1(lst);
        }

        /// <summary>
        /// 校验发布的任务明细的成本项、主资源、图号不能重复
        /// </summary>
        /// <param name="listDtl"></param>
        /// <returns></returns>
        public string CheckGWBSDetail(IList list)
        {
            return model.CheckGWBSDetail(list);
        }

        /// <summary>
        /// 计算项目任务的合同合价、责任合价、计划合价
        /// </summary>
        /// <param name="task">项目任务</param>
        /// <returns></returns>
        public GWBSTree AccountTotalPrice(GWBSTree task)
        {
            return model.AccountTotalPrice(task);
        }

        /// <summary>
        /// 计算项目任务所有下级任务节点的合同合价、责任合价、计划合价
        /// </summary>
        /// <param name="task">项目任务</param>
        /// <returns></returns>
        public GWBSTree AccountTotalPriceByChilds(GWBSTree task)
        {
            return model.AccountTotalPriceByChilds(task);
        }

        /// <summary>
        /// 发布任务节点及其子节点
        /// </summary>
        /// <param name="taskId">父任务</param>
        /// <returns></returns>
        public GWBSTree PublisthTaskNodeAndChilds(GWBSTree task)
        {
            return model.PublisthTaskNodeAndChilds(task);
        }

        /// <summary>
        /// 作废任务节点及其子节点
        /// </summary>
        /// <param name="taskId">父任务</param>
        /// <returns></returns>
        public GWBSTree InvalidTaskNodeAndChilds(GWBSTree task)
        {
            return model.InvalidTaskNodeAndChilds(task);
        }

        public List<GWBSDetail> AccountTaskDtlPrice(List<GWBSDetail> listTargetTaskDtl)
        {
            return model.AccountTaskDtlPrice(listTargetTaskDtl);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int SaveSQL(string sql)
        {
            return model.SaveSQL(sql);
        }

        /// <summary>
        /// 保存拆除的任务明细
        /// </summary>
        /// <param name="dtl"></param>
        /// <returns></returns>
        public GWBSDetail SaveBackoutGWBSDetail(GWBSDetail dtl, string optTypeCode)
        {
            return model.SaveBackoutGWBSDetail(dtl, optTypeCode);
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
            return model.CostItemZoningChange(wbs, cate, info);
        }
        /// <summary>
        /// 删除成本项地域
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public void DeleteCostItemZoning(CostItemsZoning z)
        {
            model.DeleteCostItemZoning(z);
        }

        /// <summary>
        /// 查询 缺省WBS节点及其子节点上面的状态为发布的任务明细个数
        /// </summary>
        /// <param name="sysCode"></param>
        /// <returns></returns>
        public int GetGWBSDetailLikeWBSSysCodeSql(string sysCode)
        {
            return model.GetGWBSDetailLikeWBSSysCodeSql(sysCode);
        }
        public int GetGWBSDetailLikeWBSSysCodeSql(string sysCode, string sProjectID)
        {
            return model.GetGWBSDetailLikeWBSSysCodeSql(sysCode, sProjectID);
        }
        /// <summary>
        /// 导入任务明细检查这些定额编号在成本项里是否有重复
        /// </summary>
        /// <returns></returns>
        public bool CheckQutaCodeIsRepeat(string strSql)
        {
            return model.CheckQutaCodeIsRepeat(strSql);
        }

        /// <summary>
        /// 作废明细前的判断
        /// </summary>
        /// <param name="list"></param>
        /// <param name="wbs"></param>
        /// <returns></returns>
        public string DeleteGWBSDetailBeforeOperat(List<GWBSDetail> list, GWBSTree wbs)
        {
            return model.DeleteGWBSDetailBeforeOperat(list, wbs);
        }

        /// <summary>
        /// 删除明细 连带相应生产明细一起删除
        /// </summary>
        /// <param name="wbs"></param>
        /// <param name="listInvalid"></param>
        /// <returns></returns>
        public GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, List<GWBSDetail> listInvalid)
        {
            return model.SaveOrUpdateGWBSTree(wbs, listInvalid);
        }
        public IList GetWBSDetailNum(string sGWBSTreeID, string sGWBSDetialID)
        {
            return model.GetWBSDetailNum(sGWBSTreeID, sGWBSDetialID);
        }
        public IList GetWBSDetailNum(string sGWBSTreeID, string[] sGWBSDetialIDs)
        {
            return model.GetWBSDetailNum(sGWBSTreeID, sGWBSDetialIDs);
        }
        /// <summary>
        /// 保存或更新GWBS,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <param name="dtl">修改后的明细</param>
        /// <param name="theCostItemId">修改明细前的明细</param>
        /// <returns></returns>
        public GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, IList listLedger, GWBSDetail dtl, GWBSDetail beforeDtl)
        {
            return model.SaveOrUpdateGWBSTree(wbs, listLedger, dtl, beforeDtl);
        }

        /// <summary>
        /// 保存批量生成的任务明细
        /// </summary>
        /// <param name="wbs"></param>
        /// <returns>1.返回修改后的WBS，2.返回最新的任务明细集合</returns>
        public IList SaveBatchDetail(GWBSTree wbs, string sqlWhere)
        {
            return model.SaveBatchDetail(wbs, sqlWhere);
        }

        /// <summary>
        /// 保存并回写滚动计划的状态
        /// </summary>
        /// <param name="wbs"></param>
        /// <returns></returns>
        public bool SaveAndWritebackScrollPlanState(GWBSTree wbs)
        {
            return model.SaveAndWritebackScrollPlanState(wbs);
        }

        public List<GWBSDetailLedger> GWBSDtlLedgerQuery(string sqlCondition, string createStartTime, string createEndTime)
        {
            return model.GWBSDtlLedgerQuery(sqlCondition, createStartTime, createEndTime);
        }

        /// <summary>
        /// 工程任务合价查询
        /// </summary>
        /// <param name="wbs"></param>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        public DataTable SelectGWBSValence(GWBSTree wbs, CurrentProjectInfo projectInfo)
        {
            return model.SelectGWBSValence(wbs, projectInfo);
        }

        public DataTable SelectGWBSValence(string sSysCodes, string sProjectId)
        {
            return model.SelectGWBSValence(sSysCodes, sProjectId);
        }

        #region 工程成本批量维护
        public DataTable GetGWBSDetailCostSubject(string sysCode)
        {
            return model.GetGWBSDetailCostSubject(sysCode).Tables[0];
        }

        public DataTable SelectGWBSResourceCost(GWBSTree wbs, CurrentProjectInfo projectInfo)
        {
            return model.SelectGWBSResourceCost(wbs, projectInfo).Tables[0];
        }

        public DataTable GetSubjectCostQuotaByQuotaCode(string strCodes)
        {
            return model.GetSubjectCostQuotaByQuotaCode(strCodes).Tables[0];
        }
        public DataTable GetSubjectCostQuotas(string id, string sysCode)
        {
            return model.GetSubjectCostQuotas(id, sysCode).Tables[0];
        }
        #endregion

        public bool UpdateGWBSAndScheduleOrderNO(List<GWBSTree> ilst)
        {
            return model.UpdateGWBSAndScheduleOrderNO(ilst);
        }


        public IList SaveOrUpdateShareRateInfo(IList ilistChanged, IList ilistDel)
        {
            return model.SaveOrUpdateShareRateInfo(ilistChanged, ilistDel);
        }
    }
}
