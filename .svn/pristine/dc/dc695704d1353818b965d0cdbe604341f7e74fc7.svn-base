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
using Iesi.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    /// <summary>
    /// 工程任务类型服务
    /// </summary>
    public class ResourceRequirePlanSrv : IResourceRequirePlanSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        private IBillCodeRuleSrv billCodeRuleSrv;
        public IBillCodeRuleSrv BillCodeRuleSrv
        {
            get { return billCodeRuleSrv; }
            set { billCodeRuleSrv = value; }
        }

        /// <summary>
        /// 保存或修改资源需求计划集合
        /// </summary>
        /// <param name="list">资源需求计划集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateResourceRequirePlan(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 保存或修改资源需求计划
        /// </summary>
        /// <param name="group">资源需求计划</param>
        /// <returns></returns>
        [TransManager]
        public ResourceRequirePlan SaveOrUpdateResourceRequirePlan(ResourceRequirePlan plan)
        {
            dao.SaveOrUpdate(plan);
            return plan;
        }

        /// <summary>
        /// 删除资源需求计划集合
        /// </summary>
        /// <param name="list">资源需求计划集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteResourceRequirePlan(IList list)
        {
            foreach (ResourceRequirePlan cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    object obj = dao.Get(typeof(ResourceRequirePlan), cg.Id);
                    if (obj != null)
                        dao.Delete(obj);
                }
            }
            return true;
        }

        /// <summary>
        /// 保存或修改滚动资源需求计划和计划明细
        /// </summary>
        /// <param name="plan">滚动资源需求计划</param>
        /// <param name="list">滚动资源需求计划明细集合</param>
        /// <param name="isPublish">是否发布</param>
        /// <param name="deleteList">删除</param>
        /// <returns></returns>
        [TransManager]
        public Hashtable SaveOrUpdateResourcePlanAndDetail(ResourceRequirePlan plan, IList list, bool isPublish, IList deleteList)
        {
            Hashtable ht = new Hashtable();
            ResourceRequirePlan resultPlan = plan;
            //保存主表
            IList resultDtlList = new ArrayList();
            if (plan.Id != null)
            {
                ResourceRequirePlan rrp = dao.Get(typeof(ResourceRequirePlan), plan.Id) as ResourceRequirePlan;
                if (rrp != null)
                {
                    rrp.RequirePlanVersion = plan.RequirePlanVersion;
                    if (rrp.State == ResourceRequirePlanState.发布 || plan.State == ResourceRequirePlanState.发布)
                    {
                        rrp.State = ResourceRequirePlanState.发布;
                    }
                    resultPlan = rrp;
                }
            }
            if (list != null && list.Count > 0)
            {
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();

                for (int i = 0; i < list.Count; i++)
                {
                    ResourceRequirePlanDetail dtl = list[i] as ResourceRequirePlanDetail;
                    //if (isPublish)
                    if (resultPlan.State == ResourceRequirePlanState.发布 && dtl.State == ResourceRequirePlanDetailState.编制)
                    {
                        dtl.State = ResourceRequirePlanDetailState.发布;
                    }
                    if (dtl.TheResourceRequirePlan == null || dtl.TheResourceRequirePlan.Id == null)
                    {
                        dtl.TheResourceRequirePlan = resultPlan;
                    }

                }
                oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
                oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", plan.Id));
                IList dtlNewList = dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
                if (dtlNewList != null && dtlNewList.Count > 0)
                {
                    foreach (ResourceRequirePlanDetail dtlOld in list)
                    {
                        bool flag = false;
                        foreach (ResourceRequirePlanDetail dtlNew in dtlNewList)
                        {
                            if (dtlNew.Id == dtlOld.Id)
                            {
                                dtlNew.TheGWBSTaskGUID.Id = dtlOld.TheGWBSTaskGUID.Id;
                                dtlNew.TheGWBSTaskName = dtlOld.TheGWBSTaskName;
                                dtlNew.TheGWBSSysCode = dtlOld.TheGWBSSysCode;
                                dtlNew.PlanInRequireQuantity = dtlOld.PlanInRequireQuantity;
                                dtlNew.PlanOutRequireQuantity = dtlOld.PlanOutRequireQuantity;
                                dtlNew.TechnicalParameters = dtlOld.TechnicalParameters;
                                dtlNew.QualityStandards = dtlOld.QualityStandards;
                                dtlNew.SupplyPlanPublishQuantity = dtlOld.SupplyPlanPublishQuantity;
                                dtlNew.Descript = dtlOld.Descript;
                                if (dtlOld.State == ResourceRequirePlanDetailState.作废 || dtlNew.State == ResourceRequirePlanDetailState.作废)
                                {
                                    dtlNew.State = ResourceRequirePlanDetailState.作废;
                                }
                                else if (dtlOld.State == ResourceRequirePlanDetailState.发布 || dtlNew.State == ResourceRequirePlanDetailState.发布)
                                {
                                    dtlNew.State = ResourceRequirePlanDetailState.发布;
                                }
                                dtlNew.QuantityUnitGUID = dtlOld.QuantityUnitGUID;
                                dtlNew.QuantityUnitName = dtlOld.QuantityUnitName;
                                flag = true;
                                resultDtlList.Add(dtlNew);
                            }
                        }
                        if (!flag)
                        {
                            resultDtlList.Add(dtlOld);
                        }
                    }
                }
                else
                {
                    resultDtlList = list;
                }
            }
            resultPlan.Details.Clear();
            foreach (ResourceRequirePlanDetail dtl in resultDtlList)
            {
                dtl.TheResourceRequirePlan = resultPlan;
                resultPlan.Details.Add(dtl);
            }
            dao.SaveOrUpdate(resultPlan);
            ht.Add("plan", resultPlan);
            ht.Add("list", resultDtlList);
            return ht;
        }

        /// <summary>
        /// 保存或修改计划明细T
        /// </summary>
        /// <param name="dtl">计划明细</param>
        /// <returns></returns>
        [TransManager]
        public ResourceRequirePlanDetail SaveOrUpdateResourcePlanDetail(ResourceRequirePlanDetail dtl)
        {
            dao.SaveOrUpdate(dtl);

            return dtl;
        }

        /// <summary>
        /// 保存或修改计划明细集合
        /// </summary>
        /// <param name="list">计划明细集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateResourcePlanDetail(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 资源需求计划明细集合
        /// </summary>
        /// <param name="list">资源需求计划明细集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteResourceRequirePlanDetail(IList list)
        {
            foreach (ResourceRequirePlanDetail dtl in list)
            {
                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    ResourceRequirePlanDetail tempDtl = dao.Get(typeof(ResourceRequirePlanDetail), dtl.Id) as ResourceRequirePlanDetail;
                    if (tempDtl != null)
                        dao.Delete(tempDtl);
                }
            }
            return true;
        }

        /// <summary>
        /// 保存或修改资源需求计划单
        /// </summary>
        /// <param name="rrr"></param>
        /// <returns></returns>
        [TransManager]
        public ResourceRequireReceipt SaveOrUpdateResourceRequireReceipt(ResourceRequireReceipt rrr)
        {
            dao.SaveOrUpdate(rrr);
            return rrr;
        }

        /// <summary>
        /// 删除资源需求计划单
        /// </summary>
        /// <param name="rrr"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteResourceRequireReceipt(ResourceRequireReceipt rrr)
        {
            dao.Delete(rrr);
            return true;
        }
        /// <summary>
        /// 删除资源需求计划单集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteResourceRequireReceiptList(IList list)
        {
            foreach (ResourceRequireReceipt rrr in list)
            {
                dao.Delete(rrr);
            }
            return true;
        }
        /// <summary>
        /// 删除资源需求计划单明细
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteResourceRequireReceiptDtlList(IList list)
        {
            foreach (ResourceRequireReceiptDetail rrr in list)
            {
                dao.Delete(rrr);
            }
            return true;
        }
        /// <summary>
        /// 保存或修改资源需求计划单明细集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateResourceRequireReceiptDetail(IList list)
        {
            dao.SaveOrUpdate(list);
            return list;
        }

        /// <summary>
        /// 保存或修改资源需求计划单和其明细集合
        /// </summary>
        /// <param name="rrr"></param>
        /// <param name="rrrd"></param>
        /// <returns></returns>
        [TransManager]
        public ResourceRequireReceipt SaveResourceRequireReceiptAndDetail(ResourceRequireReceipt rrr, IList list)
        {
            dao.SaveOrUpdate(rrr);
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ResourceRequireReceiptDetail rrrd = list[i] as ResourceRequireReceiptDetail;
                    rrrd.TheResReceipt = rrr;
                }
            }
            dao.SaveOrUpdate(list);
            return rrr;
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

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type, string projectId)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId);
        }

        /// <summary>
        /// 根据项目 物资分类(专业分类) 生成Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectId"></param>
        /// <param name="matCatAbb"></param>
        /// <returns></returns>
        private string GetCode(Type type, string projectId, string matCatAbb)
        {
            return BillCodeRuleSrv.GetBillNoNextValue(type, "Code", DateTime.Now, projectId, matCatAbb);
        }

        public bool GenerateSupplyResourcePlanBak(RemandPlanType planType, string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));

            if (planType == RemandPlanType.日常需求计划)
            {
                #region 日常需求计划

                IList list = dao.ObjectQuery(typeof(DailyPlanMaster), oq);
                if (list.Count > 0)
                {
                    DailyPlanMaster theDailyPlanMaster = list[0] as DailyPlanMaster;

                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.Eq("Master.Id", theDailyPlanMaster.Id));
                    oq.AddFetchMode("MaterialCategory", FetchMode.Eager);

                    IList listDtlTemp = dao.ObjectQuery(typeof(DailyPlanDetail), oq);

                    IEnumerable<DailyPlanDetail> listDtl = listDtlTemp.OfType<DailyPlanDetail>();

                    var queryGroup = from d in listDtl
                                     where d.MaterialCategory != null
                                     group d by new { d.MaterialCategory.Id }
                                         into g
                                         select new { g.Key.Id };

                    IList listNewMasterPlan = new ArrayList();
                    foreach (var o in queryGroup)
                    {
                        var queryDtl = from d in listDtl
                                       where d.MaterialCategory.Id == o.Id
                                       select d;

                        DailyPlanDetail newDtl = queryDtl.ElementAt(0);
                        DailyPlanMaster master = new DailyPlanMaster();
                        master.ClassifyCode = newDtl.MaterialCategory.Code;
                        master.Code = GetCode(typeof(DailyPlanMaster), theDailyPlanMaster.ProjectId, newDtl.MaterialCategory.Name);
                        //master.Code = theDailyPlanMaster.Code;

                        master.Compilation = theDailyPlanMaster.Compilation;
                        master.CreateDate = theDailyPlanMaster.CreateDate;
                        master.CreateMonth = theDailyPlanMaster.CreateMonth;
                        master.CreatePerson = theDailyPlanMaster.CreatePerson;
                        master.CreatePersonName = theDailyPlanMaster.CreatePersonName;
                        master.CreateYear = theDailyPlanMaster.CreateYear;
                        master.CurrencyType = theDailyPlanMaster.CurrencyType;
                        master.Descript = theDailyPlanMaster.Descript;

                        master.DocState = DocumentState.Edit;

                        master.ExchangeRate = theDailyPlanMaster.ExchangeRate;
                        master.ForwardBillCode = theDailyPlanMaster.ForwardBillCode;
                        master.ForwardBillId = theDailyPlanMaster.ForwardBillId;
                        master.ForwardBillType = theDailyPlanMaster.ForwardBillType;
                        master.HandleOrg = theDailyPlanMaster.HandleOrg;
                        master.HandlePerson = theDailyPlanMaster.HandlePerson;
                        master.HandlePersonName = theDailyPlanMaster.HandlePersonName;

                        master.IsFinished = theDailyPlanMaster.IsFinished;
                        master.JBR = theDailyPlanMaster.JBR;
                        master.LastModifyDate = theDailyPlanMaster.LastModifyDate;

                        master.MaterialCategory = newDtl.MaterialCategory;
                        master.MaterialCategoryCode = newDtl.MaterialCategory.Code;
                        master.MaterialCategoryName = newDtl.MaterialCategoryName;

                        master.OperOrgInfo = theDailyPlanMaster.OperOrgInfo;
                        master.OperOrgInfoName = theDailyPlanMaster.OperOrgInfoName;
                        master.OpgSysCode = theDailyPlanMaster.OpgSysCode;

                        master.PlanName = theDailyPlanMaster.PlanName;
                        master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;

                        master.ProjectId = theDailyPlanMaster.ProjectId;
                        master.ProjectName = theDailyPlanMaster.ProjectName;

                        master.RealOperationDate = DateTime.Now;
                        master.Special = theDailyPlanMaster.Special;
                        master.SpecialType = theDailyPlanMaster.SpecialType;

                        //master.SumMoney = theDailyPlanMaster.SumMoney;
                        //master.SumQuantity = theDailyPlanMaster.SumQuantity;

                        foreach (DailyPlanDetail tempDtl in queryDtl)
                        {
                            DailyPlanDetail dtl = new DailyPlanDetail();
                            dtl.Master = master;
                            master.Details.Add(dtl);

                            dtl.ApproachDate = tempDtl.ApproachDate;
                            dtl.Descript = tempDtl.Descript;
                            dtl.ForwardDetailId = tempDtl.ForwardDetailId;
                            dtl.IsOver = tempDtl.IsOver;
                            dtl.LeftQuantity = tempDtl.LeftQuantity;
                            dtl.Manufacturer = tempDtl.Manufacturer;

                            dtl.MaterialCategory = tempDtl.MaterialCategory;
                            dtl.MaterialCategoryName = tempDtl.MaterialCategoryName;
                            dtl.MaterialCode = tempDtl.MaterialCode;
                            dtl.MaterialName = tempDtl.MaterialName;
                            dtl.MaterialResource = tempDtl.MaterialResource;
                            dtl.MaterialSpec = tempDtl.MaterialSpec;
                            dtl.MaterialStuff = tempDtl.MaterialStuff;
                            dtl.MatStandardUnit = tempDtl.MatStandardUnit;
                            dtl.MatStandardUnitName = tempDtl.MatStandardUnitName;
                            dtl.Money = tempDtl.Money;
                            dtl.Price = tempDtl.Price;
                            dtl.ProjectTask = tempDtl.ProjectTask;
                            dtl.ProjectTaskName = tempDtl.ProjectTaskName;
                            dtl.ProjectTaskSysCode = tempDtl.ProjectTaskSysCode;
                            dtl.QualityStandard = tempDtl.QualityStandard;
                            dtl.Quantity = tempDtl.Quantity;
                            dtl.RefQuantity = tempDtl.RefQuantity;
                            dtl.SendBackQuantity = tempDtl.SendBackQuantity;
                            dtl.SpecialType = tempDtl.SpecialType;

                            dtl.SupplyOrderDetail = tempDtl.SupplyOrderDetail;
                            dtl.SupplyQuantity = tempDtl.SupplyQuantity;
                            dtl.UsedPart = tempDtl.UsedPart;
                            dtl.UsedPartName = tempDtl.UsedPartName;
                            dtl.UsedRank = tempDtl.UsedRank;
                            dtl.UsedRankName = tempDtl.UsedRankName;
                        }

                        listNewMasterPlan.Add(master);
                    }
                    dao.SaveOrUpdate(listNewMasterPlan);
                }
                #endregion
            }
            else if (planType == RemandPlanType.月度需求计划 || planType == RemandPlanType.节点需求计划)
            {
                #region 月度需求计划或节点计划

                IList list = dao.ObjectQuery(typeof(MonthlyPlanMaster), oq);
                if (list.Count > 0)
                {
                    MonthlyPlanMaster theMonthPlanMaster = list[0] as MonthlyPlanMaster;

                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.Eq("Master.Id", theMonthPlanMaster.Id));
                    oq.AddFetchMode("MaterialCategory", FetchMode.Eager);

                    IList listDtlTemp = dao.ObjectQuery(typeof(MonthlyPlanDetail), oq);

                    IEnumerable<MonthlyPlanDetail> listDtl = listDtlTemp.OfType<MonthlyPlanDetail>();

                    var queryGroup = from d in listDtl
                                     where d.MaterialCategory != null
                                     group d by new { d.MaterialCategory.Id }
                                         into g
                                         select new { g.Key.Id };

                    IList listNewMasterPlan = new ArrayList();
                    foreach (var o in queryGroup)
                    {
                        var queryDtl = from d in listDtl
                                       where d.MaterialCategory.Id == o.Id
                                       select d;

                        MonthlyPlanDetail newDtl = queryDtl.ElementAt(0);

                        MonthlyPlanMaster master = new MonthlyPlanMaster();

                        master.MonthePlanType = theMonthPlanMaster.MonthePlanType;

                        master.ClassifyCode = newDtl.MaterialCategory.Code;
                        master.Code = GetCode(typeof(MonthlyPlanMaster), theMonthPlanMaster.ProjectId, newDtl.MaterialCategory.Name);
                        //master.Code = theMonthPlanMaster.Code;

                        master.Compilation = theMonthPlanMaster.Compilation;
                        master.CreateDate = theMonthPlanMaster.CreateDate;
                        master.CreateMonth = theMonthPlanMaster.CreateMonth;
                        master.CreatePerson = theMonthPlanMaster.CreatePerson;
                        master.CreatePersonName = theMonthPlanMaster.CreatePersonName;
                        master.CreateYear = theMonthPlanMaster.CreateYear;
                        master.CurrencyType = theMonthPlanMaster.CurrencyType;
                        master.Descript = theMonthPlanMaster.Descript;

                        master.DocState = DocumentState.InExecute;

                        master.ExchangeRate = theMonthPlanMaster.ExchangeRate;
                        master.ForwardBillCode = theMonthPlanMaster.ForwardBillCode;
                        master.ForwardBillId = theMonthPlanMaster.ForwardBillId;
                        master.ForwardBillType = theMonthPlanMaster.ForwardBillType;
                        master.HandleOrg = theMonthPlanMaster.HandleOrg;
                        master.HandlePerson = theMonthPlanMaster.HandlePerson;
                        master.HandlePersonName = theMonthPlanMaster.HandlePersonName;


                        master.IsFinished = theMonthPlanMaster.IsFinished;
                        master.JBR = theMonthPlanMaster.JBR;
                        master.LastModifyDate = theMonthPlanMaster.LastModifyDate;


                        master.MaterialCategory = newDtl.MaterialCategory;
                        master.MaterialCategoryCode = newDtl.MaterialCategory.Code;
                        master.MaterialCategoryName = newDtl.MaterialCategoryName;

                        master.OperOrgInfo = theMonthPlanMaster.OperOrgInfo;
                        master.OperOrgInfoName = theMonthPlanMaster.OperOrgInfoName;
                        master.OpgSysCode = theMonthPlanMaster.OpgSysCode;

                        master.PlanName = theMonthPlanMaster.PlanName;
                        master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;

                        master.ProjectId = theMonthPlanMaster.ProjectId;
                        master.ProjectName = theMonthPlanMaster.ProjectName;

                        master.RealOperationDate = DateTime.Now;
                        master.Special = theMonthPlanMaster.Special;
                        master.SpecialType = theMonthPlanMaster.SpecialType;


                        foreach (MonthlyPlanDetail tempDtl in queryDtl)
                        {
                            MonthlyPlanDetail dtl = new MonthlyPlanDetail();

                            dtl.Master = master;
                            master.Details.Add(dtl);


                            dtl.Descript = tempDtl.Descript;
                            dtl.ForwardDetailId = tempDtl.ForwardDetailId;
                            dtl.IsOver = tempDtl.IsOver;
                            dtl.LeftQuantity = tempDtl.LeftQuantity;
                            dtl.Manufacturer = tempDtl.Manufacturer;

                            dtl.MaterialCategory = tempDtl.MaterialCategory;
                            dtl.MaterialCategoryName = tempDtl.MaterialCategoryName;
                            dtl.MaterialCode = tempDtl.MaterialCode;
                            dtl.MaterialName = tempDtl.MaterialName;
                            dtl.MaterialResource = tempDtl.MaterialResource;
                            dtl.MaterialSpec = tempDtl.MaterialSpec;
                            dtl.MaterialStuff = tempDtl.MaterialStuff;
                            dtl.MatStandardUnit = tempDtl.MatStandardUnit;
                            dtl.MatStandardUnitName = tempDtl.MatStandardUnitName;
                            dtl.Money = tempDtl.Money;
                            dtl.Price = tempDtl.Price;
                            dtl.ProjectTask = tempDtl.ProjectTask;
                            dtl.ProjectTaskName = tempDtl.ProjectTaskName;
                            dtl.ProjectTaskSysCode = tempDtl.ProjectTaskSysCode;
                            dtl.QualityStandard = tempDtl.QualityStandard;
                            dtl.Quantity = tempDtl.Quantity;
                            dtl.RefQuantity = tempDtl.RefQuantity;

                            dtl.SpecialType = tempDtl.SpecialType;

                            dtl.UsedPart = tempDtl.UsedPart;
                            dtl.UsedPartName = tempDtl.UsedPartName;
                            dtl.UsedRank = tempDtl.UsedRank;
                            dtl.UsedRankName = tempDtl.UsedRankName;


                        }

                        listNewMasterPlan.Add(master);
                    }

                    dao.SaveOrUpdate(listNewMasterPlan);

                }

                #endregion
            }
            else if (planType == RemandPlanType.需求总计划)
            {
                #region 需求总计划

                IList list = dao.ObjectQuery(typeof(DemandMasterPlanMaster), oq);
                if (list.Count > 0)
                {
                    DemandMasterPlanMaster theMasterPlanMaster = list[0] as DemandMasterPlanMaster;

                    oq.Criterions.Clear();
                    oq.AddCriterion(Expression.Eq("Master.Id", theMasterPlanMaster.Id));
                    oq.AddFetchMode("MaterialCategory", FetchMode.Eager);

                    IList listDtlTemp = dao.ObjectQuery(typeof(DemandMasterPlanDetail), oq);

                    IEnumerable<DemandMasterPlanDetail> listDtl = listDtlTemp.OfType<DemandMasterPlanDetail>();

                    var queryGroup = from d in listDtl
                                     where d.MaterialCategory != null
                                     group d by new { d.MaterialCategory.Id }
                                         into g
                                         select new { g.Key.Id };

                    IList listNewMasterPlan = new ArrayList();
                    foreach (var o in queryGroup)
                    {
                        var queryDtl = from d in listDtl
                                       where d.MaterialCategory.Id == o.Id
                                       select d;

                        DemandMasterPlanDetail newDtl = queryDtl.ElementAt(0);

                        DemandMasterPlanMaster master = new DemandMasterPlanMaster();

                        master.ClassifyCode = newDtl.MaterialCategory.Code;

                        master.Code = GetCode(typeof(DemandMasterPlanMaster), theMasterPlanMaster.ProjectId, newDtl.MaterialCategory.Name);
                        //master.Code = theMasterPlanMaster.Code;

                        master.Compilation = theMasterPlanMaster.Compilation;
                        master.CreateDate = theMasterPlanMaster.CreateDate;
                        master.CreateMonth = theMasterPlanMaster.CreateMonth;
                        master.CreatePerson = theMasterPlanMaster.CreatePerson;
                        master.CreatePersonName = theMasterPlanMaster.CreatePersonName;
                        master.CreateYear = theMasterPlanMaster.CreateYear;
                        master.CurrencyType = theMasterPlanMaster.CurrencyType;
                        master.Descript = theMasterPlanMaster.Descript;

                        master.DocState = DocumentState.InExecute;

                        master.ExchangeRate = theMasterPlanMaster.ExchangeRate;
                        master.ForwardBillCode = theMasterPlanMaster.ForwardBillCode;
                        master.ForwardBillId = theMasterPlanMaster.ForwardBillId;
                        master.ForwardBillType = theMasterPlanMaster.ForwardBillType;
                        master.HandleOrg = theMasterPlanMaster.HandleOrg;
                        master.HandlePerson = theMasterPlanMaster.HandlePerson;
                        master.HandlePersonName = theMasterPlanMaster.HandlePersonName;

                        master.IsFinished = theMasterPlanMaster.IsFinished;
                        master.JBR = theMasterPlanMaster.JBR;
                        master.LastModifyDate = theMasterPlanMaster.LastModifyDate;

                        master.MaterialCategory = newDtl.MaterialCategory;
                        master.MaterialCategoryCode = newDtl.MaterialCategory.Code;
                        master.MaterialCategoryName = newDtl.MaterialCategoryName;

                        master.OperOrgInfo = theMasterPlanMaster.OperOrgInfo;
                        master.OperOrgInfoName = theMasterPlanMaster.OperOrgInfoName;
                        master.OpgSysCode = theMasterPlanMaster.OpgSysCode;

                        master.PlanName = theMasterPlanMaster.PlanName;
                        master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;

                        master.ProjectId = theMasterPlanMaster.ProjectId;
                        master.ProjectName = theMasterPlanMaster.ProjectName;

                        master.RealOperationDate = DateTime.Now;
                        master.Special = theMasterPlanMaster.Special;
                        master.SpecialType = theMasterPlanMaster.SpecialType;

                        foreach (DemandMasterPlanDetail tempDtl in queryDtl)
                        {
                            DemandMasterPlanDetail dtl = new DemandMasterPlanDetail();
                            dtl.Master = master;
                            master.Details.Add(dtl);

                            dtl.Descript = tempDtl.Descript;
                            dtl.ForwardDetailId = tempDtl.ForwardDetailId;
                            dtl.IsOver = tempDtl.IsOver;
                            dtl.Manufacturer = tempDtl.Manufacturer;

                            dtl.MaterialCategory = tempDtl.MaterialCategory;
                            dtl.MaterialCategoryName = tempDtl.MaterialCategoryName;
                            dtl.MaterialCode = tempDtl.MaterialCode;
                            dtl.MaterialName = tempDtl.MaterialName;
                            dtl.MaterialResource = tempDtl.MaterialResource;
                            dtl.MaterialSpec = tempDtl.MaterialSpec;
                            dtl.MaterialStuff = tempDtl.MaterialStuff;
                            dtl.MatStandardUnit = tempDtl.MatStandardUnit;
                            dtl.MatStandardUnitName = tempDtl.MatStandardUnitName;
                            dtl.Money = tempDtl.Money;
                            dtl.Price = tempDtl.Price;

                            dtl.QualityStandard = tempDtl.QualityStandard;
                            dtl.Quantity = tempDtl.Quantity;
                            dtl.RefQuantity = tempDtl.RefQuantity;

                            dtl.SpecialType = tempDtl.SpecialType;

                            dtl.UsedPart = tempDtl.UsedPart;
                            dtl.UsedPartName = tempDtl.UsedPartName;
                            dtl.UsedRank = tempDtl.UsedRank;
                            dtl.UsedRankName = tempDtl.UsedRankName;


                        }

                        listNewMasterPlan.Add(master);
                    }

                    dao.SaveOrUpdate(listNewMasterPlan);
                }

                #endregion
            }
            return true;
        }

        /// <summary>
        /// 查询资源需求管理生成执行计划明细
        /// </summary>
        public Hashtable GetFirstMatInfo()
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql =
                @"select id,name,code from resmaterialcategory  where numlevel=3";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            GWBSDetailLedger detailleger = new GWBSDetailLedger();
            Hashtable ht = new Hashtable();
            if (dataSet != null)
            {
                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DataDomain domain = new DataDomain();
                    domain.Name1 = TransUtil.ToString(dataRow["id"]);
                    domain.Name2 = TransUtil.ToString(dataRow["name"]);
                    ht.Add(TransUtil.ToString(dataRow["code"]), domain);
                }
            }
            return ht;
        }


        #region 总量需求计划
        /// <summary>
        /// 通过滚动需求计划查询滚动计划明细
        /// </summary>
        /// <param name="strPlan"></param>
        /// <returns></returns>
        [TransManager]
        public Hashtable GetTotalPlan(string strPlan)
        {
            Hashtable ht = new Hashtable();
            //查询滚动明细并汇总
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", strPlan));
            oq.AddFetchMode("TheResourceRequirePlan", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("MaterialGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ResourceCategory", NHibernate.FetchMode.Eager);
            IList listPlan = ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
            if (listPlan.Count > 0)
            {
                foreach (ResourceRequirePlanDetail detailPlan in listPlan)
                {
                    if (ht.Count == 0)
                    {
                        ht.Add(detailPlan, detailPlan.MaterialName);
                    }
                    else
                    {
                        if (ht.ContainsValue(detailPlan.MaterialName))
                        {
                            ResourceRequirePlanDetail detail = new ResourceRequirePlanDetail();
                            foreach (System.Collections.DictionaryEntry objht in ht)
                            {
                                if (objht.Value.ToString().Equals(detailPlan.MaterialName))
                                {
                                    detail = objht.Key as ResourceRequirePlanDetail;
                                    break;
                                }
                            }
                            if (detail.DiagramNumber == detailPlan.DiagramNumber)
                            {
                                //图号相同
                                detailPlan.PlanInRequireQuantity += detail.PlanInRequireQuantity;
                                detailPlan.PlanOutRequireQuantity += detail.PlanOutRequireQuantity;
                                detailPlan.MonthPlanPublishQuantity += detail.MonthPlanPublishQuantity;
                                detailPlan.DailyPlanPublishQuantity += detail.DailyPlanPublishQuantity;
                                detailPlan.SupplyPlanPublishQuantity += detail.SupplyPlanPublishQuantity;
                                detailPlan.ExecutedQuantity += detail.ExecutedQuantity;
                                ht.Remove(detail);
                                ht.Add(detailPlan, detailPlan.MaterialName);

                            }
                            else
                            {
                                ht.Add(detailPlan, detailPlan.MaterialName);
                            }
                        }
                        else
                        {
                            ht.Add(detailPlan, detailPlan.MaterialName);
                        }
                    }
                }
            }


            return ht;

        }
        /// <summary>
        /// 通过滚动需求计划查询所属滚动计划的所有总量需求计划明细
        /// </summary>
        /// <param name="strPlan"></param>
        /// <returns></returns>
        [TransManager]
        public Hashtable GetTotalReceipt(string strPlan)
        {
            Hashtable ht = new Hashtable();
            ObjectQuery objectQuery = new ObjectQuery();
            objectQuery.AddCriterion(Expression.Eq("TheResReceipt.ResourceRequirePlan.Id", strPlan));
            objectQuery.AddFetchMode("TheResReceipt.ResourceCategory", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheResReceipt.ResourceRequirePlan", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            objectQuery.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
            IList listReceipt = ObjectQuery(typeof(ResourceRequireReceiptDetail), objectQuery);
            if (listReceipt.Count > 0)
            {
                foreach (ResourceRequireReceiptDetail detailReceipt in listReceipt)
                {
                    if (ht.Count == 0)
                    {
                        ht.Add(detailReceipt, detailReceipt.MaterialName);
                    }
                    else
                    {
                        if (ht.ContainsValue(detailReceipt.MaterialName))
                        {
                            ResourceRequireReceiptDetail detail = new ResourceRequireReceiptDetail();
                            foreach (System.Collections.DictionaryEntry objht in ht)
                            {
                                if (objht.Value.ToString().Equals(detailReceipt.MaterialName))
                                {
                                    detail = objht.Key as ResourceRequireReceiptDetail;
                                    break;
                                }
                            }
                            if (detail.DiagramNumber == detailReceipt.DiagramNumber)
                            {
                                //图号相同
                                detailReceipt.PlanInRequireQuantity += detail.PlanInRequireQuantity;
                                detailReceipt.PlanOutRequireQuantity += detail.PlanOutRequireQuantity;
                                ht.Remove(detail);
                                ht.Add(detailReceipt, detailReceipt.MaterialName);
                            }
                            else
                            {
                                ht.Add(detailReceipt, detailReceipt.MaterialName);
                            }
                        }
                        else
                        {
                            ht.Add(detailReceipt, detailReceipt.MaterialName);
                        }
                    }
                }
            }
            return ht;
        }
        /// <summary>
        /// 总量需求计划查询
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        public IList GetResReceipt(ObjectQuery objectQuery)
        {
            objectQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            return Dao.ObjectQuery(typeof(ResourceRequireReceipt), objectQuery);
        }


        [TransManager]
        public ResourceRequireReceipt SaveResReceipt(ResourceRequireReceipt obj)
        {
            if (obj.Id == null)
            {
                obj.Code = GetCode(typeof(ResourceRequireReceipt), obj.ProjectId);
                obj.RealOperationDate = DateTime.Now;
            }
            if (obj.State == ResourceRequirePlanState.提交)
            {
                obj.SubmitDate = DateTime.Now;
            }
            dao.SaveOrUpdate(obj);

            return obj;
        }
        [TransManager]
        public ResourceRequireReceiptDetail SaveResReceiptDetail(ResourceRequireReceiptDetail obj)
        {
            dao.SaveOrUpdate(obj);

            return obj;
        }
        // dao.Delete(tempNode);
        public bool DeleteResReceipt(ResourceRequireReceipt obj)
        {
            if (obj.Id != null)
            {
                dao.Delete(obj);
                return true;
            }
            return false;
        }

        public bool DeleteResReceiptDetail(ResourceRequireReceiptDetail obj)
        {
            if (obj.Id != null)
            {
                dao.Delete(obj);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 通过主表ID查询明细
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(ObjectQuery oq)
        {
            oq.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
            return dao.ObjectQuery(typeof(ResourceRequireReceiptDetail), oq);
        }
        ///// <summary>
        ///// 通过主表ID查询滚动资源需求计划明细
        ///// </summary>
        ///// <param name="entityType">实体类型</param>
        ///// <param name="oq">查询条件</param>
        ///// <returns></returns>
        //public IList GetResPlanDetail(ObjectQuery oq)
        //{
        //    oq.AddFetchMode("Material", NHibernate.FetchMode.Eager);
        //    oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
        //    oq.AddFetchMode("ResourceRequirePlan", NHibernate.FetchMode.Eager);

        //    return dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
        //}
        /// <summary>
        /// 通过主表ID查询滚动资源需求计划明细
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public Hashtable GetResPlanDetail(ObjectQuery oq)
        {
            Hashtable ht = new Hashtable();
            oq.AddFetchMode("Material", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ResourceRequirePlan", NHibernate.FetchMode.Eager);

            IList listDtl = dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
            if (listDtl.Count > 0)
            {
                foreach (ResourceRequirePlanDetail dtl in listDtl)
                {
                    //通过管理OBS过滤信息
                    ObjectQuery objectQuery = new ObjectQuery();
                    //objectQuery.AddCriterion(Expression.Like("OpgSysCode", "%" + (txtOperationOrg.Tag as OperationOrg).SysCode + "%"));
                    string[] sysCodes = dtl.TheGWBSTaskGUID.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    Disjunction dis = new Disjunction();
                    for (int i = 0; i < sysCodes.Length; i++)
                    {
                        string sysCode = "";
                        for (int j = 0; j <= i; j++)
                        {
                            sysCode += sysCodes[j] + ".";
                        }
                        dis.Add(Expression.Eq("ProjectTask.SysCode", sysCode));
                    }
                    objectQuery.AddCriterion(dis);
                    //objectQuery.AddCriterion(Expression.Like("ProjectTask.SysCode", "%" + dtl.TheGWBSTaskGUID.SysCode + "%"));

                    objectQuery.AddFetchMode("ProjectTask", NHibernate.FetchMode.Eager);
                    IList obslist = ObjectQuery(typeof(OBSManage), objectQuery);
                    if (obslist.Count > 0)
                    {
                        foreach (OBSManage manage in obslist)
                        {
                            if (dtl.TheGWBSTaskGUID.SysCode.IndexOf(manage.ProjectTask.SysCode) > -1)
                            {
                                if (ht.Count == 0)
                                {
                                    ht.Add(dtl, dtl.MaterialResource.Id);
                                }
                                else
                                {
                                    if (ht.ContainsValue(dtl.MaterialResource.Id))
                                    {
                                        ResourceRequirePlanDetail detail = new ResourceRequirePlanDetail();
                                        bool flag = false;
                                        foreach (System.Collections.DictionaryEntry objht in ht)
                                        {
                                            if (objht.Value.ToString().Equals(dtl.MaterialResource.Id))
                                            {
                                                detail = objht.Key as ResourceRequirePlanDetail;
                                                if (detail.DiagramNumber == dtl.DiagramNumber)
                                                {
                                                    detail.ResponsibilityCostQuantity += dtl.ResponsibilityCostQuantity;//责任成本量
                                                    detail.PlanInRequireQuantity += dtl.PlanInRequireQuantity;//计划内
                                                    detail.PlannedCostQuantity += dtl.PlannedCostQuantity;//计划成本量
                                                    detail.PlanOutRequireQuantity += dtl.PlanOutRequireQuantity;//计划外
                                                    detail.FirstOfferRequireQuantity += dtl.FirstOfferRequireQuantity;//甲供
                                                    detail.SupplyPlanPublishQuantity += dtl.SupplyPlanPublishQuantity;
                                                    detail.TechnicalParameters += dtl.TechnicalParameters;
                                                    ht.Remove(detail);
                                                    ht.Add(detail, detail.MaterialResource.Id);
                                                    flag = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (!flag)
                                        {
                                            ht.Add(dtl, dtl.MaterialResource.Id);
                                        }
                                    }
                                    else
                                    {
                                        ht.Add(dtl, dtl.MaterialResource.Id);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                if (ht.Count == 0)
                {
                    foreach (ResourceRequirePlanDetail dtl in listDtl)
                    {
                        ht.Add(dtl, dtl.MaterialResource.Id);
                    }
                }
            }

            return ht;

        }

        /// <summary>
        /// 通过主表ID查询主表信息
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public ResourceRequireReceipt GetResourceRequireReceipt(ObjectQuery oq)
        {
            oq.AddFetchMode("ResourceCategory", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.MaterialGUID", NHibernate.FetchMode.Eager);
            IList list = dao.ObjectQuery(typeof(ResourceRequireReceipt), oq);
            if (list != null && list.Count > 0)
            {
                return list[0] as ResourceRequireReceipt;
            }
            return null;
        }
        #endregion

        #region 计算GWBS滚动资源需求数据
        /// <summary>
        /// 计算GWBS滚动资源需求数据
        /// </summary>
        /// <param name="rrp">所依据的{滚动资源需求计划}</param>
        /// <param name="wbs">指定的{工程项目任务}节点</param>
        /// <param name="mat">指定的资源类型</param>
        /// <param name="diagramNumber">指定资源类型的图号</param>
        /// <returns></returns>
        [TransManager]
        public ResourceRequirePlanDetail CalculateGWBSRollingResourceDemandData(ResourceRequirePlan rrp, GWBSTree wbs, Material mat, string diagramNumber, PlanRequireType requireType)
        {
            if (rrp == null || wbs == null || mat == null)
            {
                return null;
            }
            else
            {
                //确定<操作｛滚动资源需求计划明细｝>
                //ResourceRequirePlanDetail d = new ResourceRequirePlanDetail();
                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", rrp.Id));
                oq.AddCriterion(Expression.Eq("MaterialResource.Id", mat.Id));
                if (string.IsNullOrEmpty(diagramNumber))
                {
                    oq.AddCriterion(Expression.IsNull("DiagramNumber"));
                }
                else
                {
                    oq.AddCriterion(Expression.Eq("DiagramNumber", diagramNumber));
                }
                oq.AddCriterion(Expression.Eq("State", ResourceRequirePlanDetailState.发布));
                dis.Add(Expression.Like("TheGWBSSysCode", wbs.SysCode, MatchMode.Anywhere));
                oq.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
                string[] sysCodes = wbs.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
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
                IList planDtlList = dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
                IList listDtl = new ArrayList();
                IList listDtl1 = new ArrayList();
                bool flag = false;
                foreach (ResourceRequirePlanDetail pdtl in planDtlList)
                {
                    if (pdtl.RequireType == requireType)
                    {
                        listDtl.Add(pdtl);
                        flag = true;
                    }
                }

                if (flag)
                {
                    bool sameObjectFlag = false;
                    bool parentObjectFlag = false;
                    List<ResourceRequirePlanDetail> childObjectList = new List<ResourceRequirePlanDetail>();
                    List<ResourceRequirePlanDetail> parentObjectList = new List<ResourceRequirePlanDetail>();
                    ResourceRequirePlanDetail samePlanDtl = new ResourceRequirePlanDetail();
                    ResourceRequirePlanDetail parentObject = new ResourceRequirePlanDetail();
                    ResourceRequirePlanDetail result = new ResourceRequirePlanDetail();
                    //确定节点关系
                    foreach (ResourceRequirePlanDetail dtl in listDtl)
                    {
                        if (dtl.TheGWBSSysCode == wbs.SysCode)//需求计划节点
                        {
                            sameObjectFlag = true;
                            samePlanDtl = dtl;
                        }
                        else if (dtl.TheGWBSSysCode.Contains(wbs.SysCode))//需求计划节点(子节点)
                        {
                            childObjectList.Add(dtl);
                        }
                        else if (wbs.SysCode.Contains(dtl.TheGWBSSysCode))//需求计划节点(父节点)dtl.TheGWBSSysCode.Contains(wbs.SysCode
                        {
                            parentObjectList.Add(dtl);
                            parentObjectFlag = true;
                        }
                    }

                    if (sameObjectFlag)
                    {
                        #region  同级
                        result.TheGWBSTaskGUID = wbs;
                        result.TheGWBSSysCode = wbs.SysCode;
                        result.TheGWBSTaskName = wbs.Name;

                        result.CreateTime = samePlanDtl.CreateTime;
                        result.DailyPlanPublishQuantity = samePlanDtl.DailyPlanPublishQuantity;
                        result.DiagramNumber = samePlanDtl.DiagramNumber;
                        result.ExecutedQuantity = samePlanDtl.ExecutedQuantity;
                        result.FirstOfferRequireQuantity = samePlanDtl.FirstOfferRequireQuantity;
                        result.MaterialCode = samePlanDtl.MaterialCode;
                        result.MaterialResource = samePlanDtl.MaterialResource;
                        result.MaterialName = samePlanDtl.MaterialName;
                        result.MaterialStuff = samePlanDtl.MaterialStuff;
                        result.MaterialSpec = samePlanDtl.MaterialSpec;
                        result.MonthPlanPublishQuantity = samePlanDtl.MonthPlanPublishQuantity;
                        result.SupplyPlanPublishQuantity = samePlanDtl.SupplyPlanPublishQuantity;
                        result.PlanInRequireQuantity = samePlanDtl.PlanInRequireQuantity;
                        result.PlannedCostQuantity = samePlanDtl.PlannedCostQuantity;
                        result.PlanOutRequireQuantity = samePlanDtl.PlanOutRequireQuantity;
                        result.QuantityUnitGUID = samePlanDtl.QuantityUnitGUID;
                        result.QuantityUnitName = samePlanDtl.QuantityUnitName;
                        result.RequireType = samePlanDtl.RequireType;
                        result.ResourceCategory = samePlanDtl.ResourceCategory;
                        result.ResourceTypeClassification = samePlanDtl.ResourceTypeClassification;
                        result.ResponsibilityCostQuantity = samePlanDtl.ResponsibilityCostQuantity;
                        result.State = samePlanDtl.State;
                        result.StateUpdateTime = DateTime.Now;
                        result.Summary = samePlanDtl.Summary;
                        result.TheProjectGUID = samePlanDtl.TheProjectGUID;
                        result.TheProjectName = samePlanDtl.TheProjectName;
                        result.TheResourceRequirePlan = samePlanDtl.TheResourceRequirePlan;
                        result.QualityStandards = samePlanDtl.QualityStandards;
                        result.TechnicalParameters = samePlanDtl.TechnicalParameters;
                        #endregion
                    }
                    else if (parentObjectFlag)
                    {
                        #region  分摊
                        var query = parentObjectList.OrderBy(p => p.TheGWBSSysCode.Length).ToList<ResourceRequirePlanDetail>();
                        parentObject = query[0];
                        decimal planCostAmount = DemandTotalSharingCoefficient(parentObject, wbs);//指定GWBS节点的计划成本量
                        if (planCostAmount == 0) return null;
                        decimal shareFactor = 0;
                        if (parentObject.PlannedCostQuantity != 0)
                        {
                            shareFactor = planCostAmount / parentObject.PlannedCostQuantity;//预算量分摊因子
                        }
                        //decimal transformFactor = 0;
                        //if (parentObject.PlannedCostQuantity != 0)
                        //{
                        //    transformFactor = parentObject.PlanInRequireQuantity / parentObject.PlannedCostQuantity;//预算to需求变换因子
                        //}

                        result.TheGWBSSysCode = wbs.SysCode;
                        result.TheGWBSTaskName = wbs.Name;
                        result.TheGWBSTaskGUID = wbs;
                        result.CreateTime = parentObject.CreateTime;
                        result.DiagramNumber = parentObject.DiagramNumber;
                        result.MaterialCode = parentObject.MaterialCode;
                        result.MaterialResource = parentObject.MaterialResource;
                        result.MaterialName = parentObject.MaterialName;
                        result.MaterialStuff = parentObject.MaterialStuff;
                        result.MaterialSpec = parentObject.MaterialSpec;
                        result.QuantityUnitGUID = parentObject.QuantityUnitGUID;
                        result.QuantityUnitName = parentObject.QuantityUnitName;
                        result.RequireType = parentObject.RequireType;
                        result.ResourceCategory = parentObject.ResourceCategory;
                        result.ResourceTypeClassification = parentObject.ResourceTypeClassification;
                        result.State = parentObject.State;
                        result.StateUpdateTime = DateTime.Now;
                        result.Summary = parentObject.Summary;
                        result.TheProjectGUID = parentObject.TheProjectGUID;
                        result.TheProjectName = parentObject.TheProjectName;
                        result.TheResourceRequirePlan = parentObject.TheResourceRequirePlan;
                        result.QualityStandards = parentObject.QualityStandards;
                        result.TechnicalParameters = parentObject.TechnicalParameters;

                        //result.ResponsibilityCostQuantity = shareFactor * parentObject.ResponsibilityCostQuantity;
                        //result.ExecutedQuantity = shareFactor * transformFactor * parentObject.ExecutedQuantity;
                        //result.FirstOfferRequireQuantity = shareFactor * parentObject.FirstOfferRequireQuantity;
                        //result.DailyPlanPublishQuantity = shareFactor * transformFactor * parentObject.DailyPlanPublishQuantity;
                        //result.MonthPlanPublishQuantity = shareFactor * transformFactor * parentObject.MonthPlanPublishQuantity;
                        //result.SupplyPlanPublishQuantity = shareFactor * transformFactor * parentObject.SupplyPlanPublishQuantity;
                        //result.PlanInRequireQuantity = shareFactor * parentObject.PlanInRequireQuantity;
                        //result.PlannedCostQuantity = shareFactor * parentObject.PlannedCostQuantity;
                        //result.PlanOutRequireQuantity = shareFactor * parentObject.PlanOutRequireQuantity;

                        result.MonthPlanPublishQuantity = parentObject.MonthPlanPublishQuantity;
                        result.DailyPlanPublishQuantity = parentObject.DailyPlanPublishQuantity;
                        result.ExecutedQuantity = parentObject.ExecutedQuantity;
                        result.PlanInRequireQuantity = shareFactor * parentObject.PlanInRequireQuantity;
                        result.PlanOutRequireQuantity = shareFactor * parentObject.PlanOutRequireQuantity;

                        #endregion
                    }
                    else
                    {
                        #region 汇总
                        if (childObjectList.Count > 0)
                        {
                            ResourceRequirePlanDetail oneChildObject = childObjectList[0];
                            result.TheGWBSTaskGUID = wbs;
                            result.TheGWBSSysCode = wbs.SysCode;
                            result.TheGWBSTaskName = wbs.Name;
                            result.CreateTime = oneChildObject.CreateTime;
                            result.DiagramNumber = oneChildObject.DiagramNumber;
                            result.MaterialCode = oneChildObject.MaterialCode;
                            result.MaterialResource = oneChildObject.MaterialResource;
                            result.MaterialName = oneChildObject.MaterialName;
                            result.MaterialStuff = oneChildObject.MaterialStuff;
                            result.MaterialSpec = oneChildObject.MaterialSpec;
                            result.QuantityUnitGUID = oneChildObject.QuantityUnitGUID;
                            result.QuantityUnitName = oneChildObject.QuantityUnitName;
                            result.RequireType = oneChildObject.RequireType;
                            result.ResourceCategory = oneChildObject.ResourceCategory;
                            result.ResourceTypeClassification = oneChildObject.ResourceTypeClassification;
                            result.State = oneChildObject.State;
                            result.StateUpdateTime = DateTime.Now;
                            result.Summary = oneChildObject.Summary;
                            result.TheProjectGUID = oneChildObject.TheProjectGUID;
                            result.TheProjectName = oneChildObject.TheProjectName;
                            result.TheResourceRequirePlan = oneChildObject.TheResourceRequirePlan;
                            result.QualityStandards = oneChildObject.QualityStandards;
                            result.TechnicalParameters = oneChildObject.TechnicalParameters;

                            foreach (ResourceRequirePlanDetail childObject in childObjectList)
                            {
                                result.ResponsibilityCostQuantity += childObject.ResponsibilityCostQuantity;
                                result.ExecutedQuantity += childObject.ExecutedQuantity;
                                result.FirstOfferRequireQuantity += childObject.FirstOfferRequireQuantity;
                                result.DailyPlanPublishQuantity += childObject.DailyPlanPublishQuantity;
                                result.MonthPlanPublishQuantity += childObject.MonthPlanPublishQuantity;
                                result.SupplyPlanPublishQuantity += childObject.SupplyPlanPublishQuantity;
                                result.PlanInRequireQuantity += childObject.PlanInRequireQuantity;
                                result.PlannedCostQuantity += childObject.PlannedCostQuantity;
                                result.PlanOutRequireQuantity += childObject.PlanOutRequireQuantity;
                            }
                        }
                        else
                        {
                            return null;
                        }
                        #endregion
                    }
                    return result;
                }
                else
                {
                    return null;
                }

            }
        }
        #endregion

        #region 需求总量分摊系数
        /// <summary>
        /// 需求总量分摊系数
        /// </summary>
        /// <param name="rrpd">被分摊滚动需求计划明细对象</param>
        /// <param name="wbs">需要计算分摊系数的GWBS节点，该节点是<滚动资源需求计划明细>_【所属工程项目任务】的子节点</param>
        /// <returns></returns>
        [TransManager]
        public decimal DemandTotalSharingCoefficient(ResourceRequirePlanDetail rrpd, GWBSTree wbs)
        {
            decimal planCostAmount = 0;//计划成本量
            #region  确定<核算{工程任务明细}集>
            IList accountingGWBSDetailList = new ArrayList();
            IList listGWBSDetailCostSubject = new ArrayList();
            //GWBSDetail d = new GWBSDetail();
            //d.TheGWBSSysCode
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();
            oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
            //oq.AddCriterion(Expression.Eq("CostingFlag", 1));
            dis.Add(Expression.Like("TheGWBSSysCode", wbs.SysCode, MatchMode.Start));
            string[] sysCodes = wbs.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
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
            IList dtlList = dao.ObjectQuery(typeof(GWBSDetail), oq);
            if (dtlList == null || dtlList.Count <= 0) return 0;
            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Eq("ResourceTypeGUID", rrpd.MaterialResource.Id));
            if (!string.IsNullOrEmpty(rrpd.DiagramNumber))
            {
                oq.AddCriterion(Expression.Eq("DiagramNumber", rrpd.DiagramNumber));
            }
            else
            {
                oq.AddCriterion(Expression.IsNull("DiagramNumber"));
            }
            IList costSubjectList = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
            if (costSubjectList == null || costSubjectList.Count <= 0) return 0;
            foreach (GWBSDetail d in dtlList)
            {
                foreach (GWBSDetailCostSubject c in costSubjectList)
                {
                    if (d.Id == c.TheGWBSDetail.Id && d.CostingFlag == 1)
                    {
                        accountingGWBSDetailList.Add(d);
                        break;
                    }
                }
            }
            #endregion
            //计算单位定额消耗
            foreach (GWBSDetail d in accountingGWBSDetailList)
            {
                decimal planCostSum = 0;//计划耗用数量之和
                decimal unitFixedConsumption = 0; //单位定额耗用
                foreach (GWBSDetailCostSubject c in costSubjectList)
                {
                    if (d.Id == c.TheGWBSDetail.Id)
                    {
                        planCostSum += c.PlanWorkAmount;
                    }
                }
                if (d.PlanWorkAmount != 0)
                {
                    unitFixedConsumption = planCostSum / d.PlanWorkAmount;
                }
                //计算计划成本量
                oq.Criterions.Clear();
                oq.AddCriterion(Expression.Like("TheGWBSSysCode", wbs.SysCode, MatchMode.Start));
                oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
                oq.AddCriterion(Expression.Eq("ContractGroupGUID", d.ContractGroupGUID));
                oq.AddCriterion(Expression.Eq("MainResourceTypeId", d.MainResourceTypeId));
                oq.AddCriterion(Expression.Eq("TheCostItem.Id", d.TheCostItem.Id));
                if (!string.IsNullOrEmpty(d.DiagramNumber))
                {
                    oq.AddCriterion(Expression.Eq("DiagramNumber", d.DiagramNumber));
                }
                else
                {
                    oq.AddCriterion(Expression.IsNull("DiagramNumber"));
                }
                oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
                IList produceGWBSDetailList = dao.ObjectQuery(typeof(GWBSDetail), oq);
                if (produceGWBSDetailList != null && produceGWBSDetailList.Count > 0)
                {
                    foreach (GWBSDetail p in produceGWBSDetailList)
                    {
                        planCostAmount += unitFixedConsumption * p.PlanWorkAmount;
                    }
                }
            }
            return planCostAmount;
            //return planCostAmount / rrpd.PlannedCostQuantity * rrpd.PlanInRequireQuantity;
        }
        //public decimal DemandTotalSharingCoefficient(ResourceRequirePlanDetail rrpd, GWBSTree wbs)
        //{
        //    decimal planCostAmount = 0;//计划成本量
        //    //GWBSDetailCostSubject cs = new GWBSDetailCostSubject();
        //    //<核算{工程资源耗用明细}集>
        //    ObjectQuery oq = new ObjectQuery();
        //    oq.AddCriterion(Expression.Eq("TheGWBSTreeSyscode", rrpd.TheGWBSSysCode));
        //    oq.AddCriterion(Expression.Eq("ResourceTypeGUID", rrpd.MaterialResource.Id));
        //    oq.AddCriterion(Expression.Eq("DiagramNumber", rrpd.DiagramNumber));
        //    oq.AddCriterion(Expression.Eq("TheGWBSDetail.State", GWBSDetailState.有效));
        //    IList costSubjectList = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq);

        //    //GWBSDetail d = new GWBSDetail();

        //    //<指定节点计划工程量集>
        //    IList appointNodeList = new ArrayList();
        //    oq.Criterions.Clear();
        //    oq.AddCriterion(Expression.Like("TheGWBSSysCode", wbs.SysCode, MatchMode.Anywhere));
        //    oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
        //    oq.AddCriterion(Expression.Eq("State", GWBSDetailState.有效));
        //    IList wbsDetailList = dao.ObjectQuery(typeof(GWBSDetail), oq);
        //    if (wbsDetailList != null && wbsDetailList.Count > 0)
        //    {
        //        #region
        //        for (int i = 0; i < wbsDetailList.Count; i++)
        //        {
        //            GWBSDetail dtl = wbsDetailList[i] as GWBSDetail;
        //            if (i == 0)
        //            {
        //                GWBSDetail appointNode = new GWBSDetail();
        //                appointNode.TheGWBS = dtl.TheGWBS;
        //                appointNode.TheGWBSSysCode = dtl.TheGWBSSysCode;
        //                appointNode.ContractGroupGUID = dtl.ContractGroupGUID;
        //                appointNode.MainResourceTypeId = dtl.MainResourceTypeId;
        //                appointNode.DiagramNumber = dtl.DiagramNumber;
        //                appointNode.TheCostItem = dtl.TheCostItem;
        //                appointNode.PlanWorkAmount = dtl.PlanWorkAmount;
        //                appointNodeList.Add(dtl);
        //            }
        //            else
        //            {
        //                bool flag = true;
        //                for (int j = 0; j < appointNodeList.Count; j++)
        //                {
        //                    GWBSDetail node = appointNodeList[j] as GWBSDetail;
        //                    if (node.ContractGroupGUID == dtl.ContractGroupGUID && node.MainResourceTypeId == dtl.MainResourceTypeId && node.DiagramNumber == dtl.DiagramNumber && node.TheCostItem == dtl.TheCostItem)
        //                    {
        //                        node.PlanWorkAmount += dtl.PlanWorkAmount;
        //                        flag = false;
        //                        break;
        //                    }
        //                }
        //                if (flag)
        //                {
        //                    GWBSDetail appointNode = new GWBSDetail();
        //                    appointNode.TheGWBS = dtl.TheGWBS;
        //                    appointNode.TheGWBSSysCode = dtl.TheGWBSSysCode;
        //                    appointNode.ContractGroupGUID = dtl.ContractGroupGUID;
        //                    appointNode.MainResourceTypeId = dtl.MainResourceTypeId;
        //                    appointNode.DiagramNumber = dtl.DiagramNumber;
        //                    appointNode.TheCostItem = dtl.TheCostItem;
        //                    appointNode.PlanWorkAmount = dtl.PlanWorkAmount;
        //                    appointNodeList.Add(dtl);
        //                }
        //            }
        //        }
        //        #endregion
        //    }

        //    if (costSubjectList != null && costSubjectList.Count > 0)
        //    {
        //        foreach (GWBSDetailCostSubject cs in costSubjectList)
        //        {
        //            foreach (GWBSDetail n in appointNodeList)
        //            {
        //                if (n.ContractGroupGUID == cs.TheGWBSDetail.ContractGroupGUID
        //                    && n.MainResourceTypeId == cs.TheGWBSDetail.MainResourceTypeId
        //                    && n.DiagramNumber == cs.TheGWBSDetail.DiagramNumber
        //                    && n.TheCostItem == cs.TheGWBSDetail.TheCostItem)
        //                {
        //                    planCostAmount += cs.PlanWorkAmount * n.PlanWorkAmount / cs.TheGWBSDetail.PlanWorkAmount;
        //                }
        //            }
        //        }
        //    }
        //    return planCostAmount;
        //}

        #endregion

        #region  获取预算资源需求量

        /// <summary>
        /// 获取预算资源需求量(更改)
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        [TransManager]
        public IList GetBudgetResourcesDemand(ResourceRequirePlan plan, CurrentProjectInfo projectInfo, GWBSTree wbs)
        {
            #region 1、得到工程资源耗用明细
            List<GWBSDetailCostSubject> OpCostSubjectList = new List<GWBSDetailCostSubject>();
            //IList updateList = new ArrayList();
            IList addList = new ArrayList();
            //IList addOrUpdateList = new ArrayList();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
            oq.AddCriterion(Expression.Eq("CostingFlag", 1));
            if (wbs.Id != null)
            {
                oq.AddCriterion(Expression.Like("TheGWBSSysCode", wbs.SysCode, MatchMode.Start));
            }
            oq.AddFetchMode("ListCostSubjectDetails", FetchMode.Eager);
            IList wbsDetailList = dao.ObjectQuery(typeof(GWBSDetail), oq);
            if (wbsDetailList == null || wbsDetailList.Count == 0)
            {
                return null;
            }
            foreach (GWBSDetail dtl in wbsDetailList)
            {
                OpCostSubjectList.AddRange(dtl.ListCostSubjectDetails);
            }
            //List<GWBSDetailCostSubject> OpCostSubjectList = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq).OfType<GWBSDetailCostSubject>().ToList<GWBSDetailCostSubject>();
            if (OpCostSubjectList == null || OpCostSubjectList.Count <= 0)
            {
                return null;
            }
            //根据层次码长短升序排列 父节点就在前面
            var query = OpCostSubjectList.OrderBy(p => p.TheGWBSTreeSyscode.Length).ToList<GWBSDetailCostSubject>();
            #endregion
            //int first = 1;//点击“获取预算资源需求量”按钮 要先清空计划明细上面责任成本量、计划成本量。
            #region 2、根据层次码、物料、图号汇总工程资源耗用明细相应数据 并得到滚动资源需求计划明细
            Disjunction dis = new Disjunction();
            foreach (GWBSDetailCostSubject cs in query)
            {
                bool isAddFlag = true;
                #region
                if (addList.Count > 0)
                {
                    for (int i = 0; i < addList.Count; i++)
                    {
                        ResourceRequirePlanDetail check = addList[i] as ResourceRequirePlanDetail;
                        if (cs.TheGWBSTreeSyscode.Contains(check.TheGWBSSysCode) && check.DiagramNumber == cs.DiagramNumber)
                        {
                            bool flag = false;
                            if (check.MaterialResource == null)
                            {
                                if (cs.ResourceTypeGUID == null)
                                {
                                    flag = true;
                                }
                            }
                            else
                            {
                                if (check.MaterialResource.Id == cs.ResourceTypeGUID)
                                {
                                    flag = true;
                                }
                            }
                            if (flag)
                            {
                                check.ResponsibilityCostQuantity += cs.ResponsibilitilyWorkAmount;
                                check.PlannedCostQuantity += cs.PlanWorkAmount;
                                check.PlanInRequireQuantity = check.PlannedCostQuantity;

                                isAddFlag = false;
                            }
                        }
                    }
                }
                if (isAddFlag)
                {
                    ResourceRequirePlanDetail rrpd = AddResourceRequirePlanDetail(cs, plan, projectInfo);
                    addList.Add(rrpd);
                    //dis.Add(Expression.Eq("Id", cs.ResourceTypeGUID));
                }
                #endregion
            }
            #endregion
            #region 更新加载滚动资源需求计划明细的物料相应数据
            oq.Criterions.Clear();
            
            oq.AddFetchMode("Category", FetchMode.Eager);
            oq.AddCriterion(Expression.Sql(" MaterialId in (select resourcetypeguid from thd_gwbsdetailcostsubject where theprojectguid = '" + projectInfo.Id + "') "));
            //oq.AddCriterion(dis);
            IList list = dao.ObjectQuery(typeof(Material), oq);
            foreach (Material m in list)
            {
                for (int i = 0; i < addList.Count; i++)
                {
                    ResourceRequirePlanDetail d = addList[i] as ResourceRequirePlanDetail;

                    if (d.MaterialResource == null || d.MaterialResource.Id == null)
                    {
                        d.MaterialResource = null;
                        continue;
                    }
                    if (m.Id == d.MaterialResource.Id)
                    {
                        d.MaterialResource = m;
                        d.MaterialCode = m.Code;
                        d.MaterialName = m.Name;
                        d.ResourceCategory = m.Category;
                        d.ResourceTypeClassification = m.Category.Name;
                    }
                }
            }
            #endregion
            return addList;

        }

        /// <summary>
        /// 新建滚动资源需求计划明细
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [TransManager]
        public ResourceRequirePlanDetail AddResourceRequirePlanDetail(GWBSDetailCostSubject cs, ResourceRequirePlan plan, CurrentProjectInfo projectInfo)
        {
            //ObjectQuery oq = new ObjectQuery();
            //oq.AddCriterion(Expression.Eq("Id", cs.ResourceTypeGUID));
            //oq.AddFetchMode("Category", NHibernate.FetchMode.Eager);
            //Material m = dao.ObjectQuery(typeof(Material), oq)[0] as Material;

            ResourceRequirePlanDetail rrpd = new ResourceRequirePlanDetail();
            rrpd.TheResourceRequirePlan = plan;
            rrpd.TheProjectGUID = projectInfo.Id;
            rrpd.TheProjectName = projectInfo.Name;
            rrpd.TheGWBSTaskGUID = cs.TheGWBSTree;
            rrpd.TheGWBSTaskName = cs.TheGWBSTreeName;
            rrpd.TheGWBSSysCode = cs.TheGWBSTreeSyscode;
            if (cs.ResourceTypeGUID != null)
            {
                rrpd.MaterialResource = new Material();
                rrpd.MaterialResource.Id = cs.ResourceTypeGUID;
            }
            else
            {
                rrpd.MaterialResource = null;
            }

            //rrpd.MaterialCode = m.Code;
            //rrpd.MaterialName = m.Name;
            rrpd.MaterialStuff = cs.ResourceTypeQuality;
            rrpd.MaterialSpec = cs.ResourceTypeSpec;
            rrpd.DiagramNumber = cs.DiagramNumber;
            //rrpd.ResourceCategory = m.Category;
            //rrpd.ResourceTypeClassification = m.Category.Name;
            //rrpd.Summary = null;
            rrpd.State = ResourceRequirePlanDetailState.编制;
            rrpd.StateUpdateTime = DateTime.Now;
            rrpd.FirstOfferRequireQuantity = 0;
            rrpd.ResponsibilityCostQuantity = cs.ResponsibilitilyWorkAmount;
            rrpd.PlannedCostQuantity = cs.PlanWorkAmount;
            rrpd.PlanInRequireQuantity = rrpd.PlannedCostQuantity;
            rrpd.PlanOutRequireQuantity = 0;
            rrpd.MonthPlanPublishQuantity = 0;
            rrpd.DailyPlanPublishQuantity = 0;
            rrpd.SupplyPlanPublishQuantity = 0;
            rrpd.ExecutedQuantity = 0;
            rrpd.QuantityUnitGUID = cs.ProjectAmountUnitGUID;
            rrpd.QuantityUnitName = cs.ProjectAmountUnitName;
            rrpd.RequireType = PlanRequireType.计划内需求;
            rrpd.CreateTime = DateTime.Now;
            rrpd.Price = cs.ContractBasePrice;
            return rrpd;
        }
        #endregion

        #region  获取预算资源需求量重载2

        /// <summary>
        /// 获取预算资源需求量(更改)
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        [TransManager]
        public IList GetBudgetResourcesDemand(ResourceRequireReceipt plan,CurrentProjectInfo projectInfo, GWBSTree wbs, DateTime beginDate, DateTime endDate,ResourceTpye rt,PlanType st)
        {
            #region 1、得到工程资源耗用明细
            List<GWBSDetailCostSubject> OpCostSubjectList = new List<GWBSDetailCostSubject>();
            //IList updateList = new ArrayList();
            IList addList = new ArrayList();
            //IList addOrUpdateList = new ArrayList();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));
            oq.AddCriterion(Expression.Eq("CostingFlag", 1));
            if (wbs.Id != null)
            {
                oq.AddCriterion(Expression.Like("TheGWBSSysCode", wbs.SysCode, MatchMode.Start));
            }
            oq.AddFetchMode("ListCostSubjectDetails", FetchMode.Eager);

            //资源类型
            //if (rt != null && rt != ResourceTpye.资源)
            //{
            //    oq.AddCriterion(Expression.Like("ListCostSubjectDetails.ResourceCateSyscode", rt));
             
            //}
            
            //开始结束时间 
            oq.AddCriterion(Expression.Not(Expression.Eq("TheGWBS.TaskPlanStartTime", new DateTime(1900, 1, 1))));
            oq.AddCriterion(Expression.Not(Expression.Eq("TheGWBS.TaskPlanEndTime", new DateTime(1900, 1, 1))));

            Disjunction dis_date = new Disjunction();
            dis_date.Add(Expression.And(Expression.Le("TheGWBS.TaskPlanStartTime", beginDate), Expression.Ge("TheGWBS.TaskPlanEndTime", beginDate)));
            dis_date.Add(Expression.And(Expression.Le("TheGWBS.TaskPlanStartTime", endDate), Expression.Ge("TheGWBS.TaskPlanEndTime", endDate)));
            dis_date.Add(Expression.And(Expression.Gt("TheGWBS.TaskPlanStartTime", beginDate), Expression.Lt("TheGWBS.TaskPlanEndTime" ,endDate)));

            if (st != PlanType.总体计划)
                oq.AddCriterion(dis_date);

            IList wbsDetailList = dao.ObjectQuery(typeof(GWBSDetail), oq);
            if (wbsDetailList == null || wbsDetailList.Count == 0)
            {
                return null;
            }
            foreach (GWBSDetail dtl in wbsDetailList)
            {
                OpCostSubjectList.AddRange(dtl.ListCostSubjectDetails);
            }
            //List<GWBSDetailCostSubject> OpCostSubjectList = dao.ObjectQuery(typeof(GWBSDetailCostSubject), oq).OfType<GWBSDetailCostSubject>().ToList<GWBSDetailCostSubject>();
            if (OpCostSubjectList == null || OpCostSubjectList.Count <= 0)
            {
                return null;
            }

            //资源类别
            string resourceSyscode = "";
            switch (rt)
            {
                case ResourceTpye.资源:
                    break;
                case ResourceTpye.分包资源:
                    resourceSyscode = TransUtil.SubcontractResourceCode;
                    break;
                case ResourceTpye.物资资源:
                    resourceSyscode = TransUtil.MaterialResourceSysCode;
                    break;
                case ResourceTpye.机械资源:
                    resourceSyscode = TransUtil.MechanicalResourceCode;
                    break;
                default:
                    break;
            }
            var needOpCostSubjectList = OpCostSubjectList.Where<GWBSDetailCostSubject>(p => p.ResourceCateSyscode.IndexOf(resourceSyscode) > -1);
          

            //根据层次码长短升序排列 父节点就在前面
            var query = needOpCostSubjectList.OrderBy(p => p.TheGWBSTreeSyscode.Length).ToList<GWBSDetailCostSubject>();
            #endregion
            //int first = 1;//点击“获取预算资源需求量”按钮 要先清空计划明细上面责任成本量、计划成本量。
            #region 2、根据层次码、物料、图号汇总工程资源耗用明细相应数据 并得到滚动资源需求计划明细
            Disjunction dis = new Disjunction();
            foreach (GWBSDetailCostSubject cs in query)
            {
                bool isAddFlag = true;
                #region
                if (addList.Count > 0)
                {
                    for (int i = 0; i < addList.Count; i++)
                    {
                        ResourceRequireReceiptDetail check = addList[i] as ResourceRequireReceiptDetail;
                        if (cs.TheGWBSTreeSyscode.Contains(check.TheGWBSSysCode) && check.DiagramNumber == cs.DiagramNumber)
                        {
                            bool flag = false;
                            if (check.MaterialResource == null)
                            {
                                if (cs.ResourceTypeGUID == null)
                                {
                                    flag = true;
                                }
                            }
                            else
                            {
                                if (check.MaterialResource.Id == cs.ResourceTypeGUID)
                                {
                                    flag = true;
                                }
                            }
                            if (flag)
                            {
                                check.ResponsibilityCostQuantity += cs.ResponsibilitilyWorkAmount;
                                check.PlannedCostQuantity += cs.PlanWorkAmount;
                                check.PlanInRequireQuantity = check.PlannedCostQuantity;

                                isAddFlag = false;
                            }
                        }
                    }
                }
                if (isAddFlag)
                {
                    ResourceRequireReceiptDetail rrpd = AddResourceRequireReceiptDetail(cs, plan, projectInfo);
                    addList.Add(rrpd);
                    //dis.Add(Expression.Eq("Id", cs.ResourceTypeGUID));
                }
                #endregion
            }
            #endregion

            #region 更新加载滚动资源需求计划明细的物料相应数据
            oq.Criterions.Clear();

            oq.AddFetchMode("Category", FetchMode.Eager);
            oq.AddCriterion(Expression.Sql(" MaterialId in (select resourcetypeguid from thd_gwbsdetailcostsubject where theprojectguid = '" + projectInfo.Id + "') "));
            //oq.AddCriterion(dis);
            IList list = dao.ObjectQuery(typeof(Material), oq);
            foreach (Material m in list)
            {
                for (int i = 0; i < addList.Count; i++)
                {
                    ResourceRequireReceiptDetail d = addList[i] as ResourceRequireReceiptDetail;

                    if (d.MaterialResource == null || d.MaterialResource.Id == null)
                    {
                        d.MaterialResource = null;
                        continue;
                    }
                    if (m.Id == d.MaterialResource.Id)
                    {
                        d.MaterialResource = m;
                        d.MaterialCode = m.Code;
                        d.MaterialName = m.Name;
                        d.MaterialCategory = m.Category;
                        d.MaterialCategoryName = m.Category.Name;
                    }
                }
            }
            #endregion
            return addList;

        }

        /// <summary>
        /// 新建 资源需求计划明细
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        [TransManager]
        public ResourceRequireReceiptDetail AddResourceRequireReceiptDetail(GWBSDetailCostSubject cs, ResourceRequireReceipt plan, CurrentProjectInfo projectInfo)
        {

            ResourceRequireReceiptDetail rrpd = new ResourceRequireReceiptDetail();

            rrpd.Master = plan;
            //rrpd.TheProjectGUID = projectInfo.Id;
            //rrpd.TheProjectName = projectInfo.Name;
            rrpd.TheGWBSTaskGUID = cs.TheGWBSTree;
            rrpd.TheGWBSTaskName = cs.TheGWBSTreeName;
            rrpd.TheGWBSSysCode = cs.TheGWBSTreeSyscode;
            if (cs.ResourceTypeGUID != null)
            {
                rrpd.MaterialResource = new Material();
                rrpd.MaterialResource.Id = cs.ResourceTypeGUID;
            }
            else
            {
                rrpd.MaterialResource = null;
            }
            rrpd.MaterialStuff = cs.ResourceTypeQuality;
            rrpd.MaterialSpec = cs.ResourceTypeSpec;
            rrpd.DiagramNumber = cs.DiagramNumber;
            rrpd.State = ResourceRequireReceiptDetailState.执行完毕;
            //rrpd.StateUpdateTime = DateTime.Now;
            rrpd.FirstOfferRequireQuantity = 0;
            rrpd.ResponsibilityCostQuantity = cs.ResponsibilitilyWorkAmount;
            rrpd.PlannedCostQuantity = cs.PlanWorkAmount;
            rrpd.PlanInRequireQuantity = rrpd.PlannedCostQuantity;
            rrpd.PlanOutRequireQuantity = 0;
            //rrpd.MonthPlanPublishQuantity = 0;
            rrpd.DailyPlanPublishQuantity = 0;
            rrpd.SupplyPlanPublishQuantity = 0;
            //rrpd.ExecutedQuantity = 0;
            rrpd.QuantityUnitGUID = cs.ProjectAmountUnitGUID;
            rrpd.QuantityUnitName = cs.ProjectAmountUnitName;
            rrpd.RequireType = PlanRequireType.计划内需求;
            //rrpd.CreateTime = DateTime.Now;
            rrpd.Price = cs.ContractBasePrice;
            return rrpd;

        }
        #endregion

        #region 计算GWBS计划产值
        /// <summary>
        /// 计算GWBS计划产值
        /// </summary>
        /// <param name="schedulePlan"></param>
        /// <param name="gwbs"></param>
        /// <returns></returns>
        [TransManager]
        public IList GWBSPlanValue(WeekScheduleMaster schedulePlan, GWBSTree wbs)
        {
            try
            {
                IList lists = new ArrayList();
                ObjectQuery oq = new ObjectQuery();

                oq.AddCriterion(Expression.Eq("Master.Id", schedulePlan.Id));
                oq.AddFetchMode("GWBSTree", FetchMode.Eager);
                IList listSchedulePlanDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq);

                oq.Criterions.Clear();
                oq.FetchModes.Clear();

                oq.AddCriterion(Expression.Eq("TheProjectGUID", schedulePlan.ProjectId));
                oq.AddCriterion(Expression.Eq("CostAccFlag", true));
                Disjunction dis = new Disjunction();
                foreach (WeekScheduleDetail dtl in listSchedulePlanDtl)
                {
                    dis.Add(Expression.Like("SysCode", dtl.GWBSTree.SysCode, MatchMode.Start));
                }
                oq.AddCriterion(dis);

                IEnumerable<GWBSTree> listAllGWBS = dao.ObjectQuery(typeof(GWBSTree), oq).OfType<GWBSTree>();

                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                dis = new Disjunction();


                List<string> listGWBSIds = new List<string>();
                foreach (GWBSTree g in listAllGWBS)
                {
                    listGWBSIds.Add(g.Id);
                }

                //循环1：针对<操作{周（月）进度计 划明细}集>中个个对象处理
                foreach (WeekScheduleDetail dtl in listSchedulePlanDtl)
                {
                    var queryGWBS = from g in listAllGWBS
                                    where g.SysCode.IndexOf(dtl.GWBSTree.SysCode) > -1
                                    select g;

                    if (queryGWBS.Count() > 0)
                    {
                        foreach (GWBSTree AccountWbs in queryGWBS)
                        {

                            int planDays = 0;//本期计划工期
                            if (dtl.NodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)//如果是叶节点
                            {
                                planDays = (dtl.PlannedEndDate - dtl.PlannedBeginDate).Days + 1;
                            }
                            else
                            {
                                //取下属子节点最晚计划结束实际和最早计划起始时间的工期
                                oq.Criterions.Clear();
                                oq.FetchModes.Clear();

                                oq.AddCriterion(Expression.Eq("Master.Id", dtl.Master.Id));
                                oq.AddCriterion(Expression.Like("GWBSTreeSysCode", dtl.GWBSTreeSysCode, MatchMode.Start));
                                IEnumerable<WeekScheduleDetail> listWeekDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq).OfType<WeekScheduleDetail>();

                                var queryWeekDtlMinBeginTime = from w in listWeekDtl
                                                               where w.PlannedBeginDate == listWeekDtl.Min(t => w.PlannedBeginDate)
                                                               select w;

                                var queryWeekDtlMaxEndTime = from w in listWeekDtl
                                                             where w.PlannedEndDate == listWeekDtl.Max(t => w.PlannedEndDate)
                                                             select w;

                                planDays = (queryWeekDtlMaxEndTime.ElementAt(0).PlannedEndDate - queryWeekDtlMinBeginTime.ElementAt(0).PlannedBeginDate).Days + 1;
                            }

                            int planCountDays = 0;//总工期

                            oq.Criterions.Clear();
                            oq.FetchModes.Clear();

                            oq.AddCriterion(Expression.Eq("Master.Id", schedulePlan.ForwardBillId));
                            oq.AddCriterion(Expression.Eq("GWBSTree.Id", AccountWbs.Id));

                            IList listPlanDtl = dao.ObjectQuery(typeof(ProductionScheduleDetail), oq);

                            if (listPlanDtl == null || listPlanDtl.Count == 0)
                                continue;

                            ProductionScheduleDetail schedulePlanDtl = listPlanDtl[0] as ProductionScheduleDetail;

                            if (schedulePlanDtl.GWBSNodeType == VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode)//如果是叶节点
                                planCountDays = (schedulePlanDtl.PlannedEndDate - schedulePlanDtl.PlannedBeginDate).Days + 1;
                            else
                            {
                                //取下属子节点最晚计划结束实际和最早计划起始时间的工期
                                oq.Criterions.Clear();
                                oq.FetchModes.Clear();

                                oq.AddCriterion(Expression.Eq("Master.Id", schedulePlanDtl.Master.Id));
                                oq.AddCriterion(Expression.Like("GWBSTreeSysCode", schedulePlanDtl.GWBSTreeSysCode, MatchMode.Start));
                                IEnumerable<WeekScheduleDetail> listChildPlanDtl = dao.ObjectQuery(typeof(WeekScheduleDetail), oq).OfType<WeekScheduleDetail>();

                                var queryPlanDtlMinBeginTime = from w in listChildPlanDtl
                                                               where w.PlannedBeginDate == listChildPlanDtl.Min(t => w.PlannedBeginDate)
                                                               select w;

                                var queryPlanDtlMaxEndTime = from w in listChildPlanDtl
                                                             where w.PlannedEndDate == listChildPlanDtl.Max(t => w.PlannedEndDate)
                                                             select w;

                                planCountDays = (queryPlanDtlMaxEndTime.ElementAt(0).PlannedEndDate - queryPlanDtlMinBeginTime.ElementAt(0).PlannedBeginDate).Days + 1;
                            }

                            int periodPercent = planDays / planCountDays;
                            decimal dc = periodPercent * AccountWbs.ContractTotalPrice;
                            IList lst = new ArrayList();
                            lst.Add(dtl.TaskCompletedPercent);
                            lst.Add(periodPercent);
                            lists.Add(lst);

                            oq.Criterions.Clear();
                            oq.FetchModes.Clear();

                            //oq.AddCriterion(Expression)
                        }
                    }
                }
                return lists;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region 生成期间需求计划单明细
        /// <summary>
        /// 生成期间需求计划单明细
        /// </summary>
        /// <param name="resPlan"></param>
        /// <param name="resReceipt"></param>
        /// <param name="ht"></param>
        /// <returns></returns>
        public IList GetPeriodResPlanDetail(ResourceRequirePlan resPlan, ResourceRequireReceipt resReceipt, List<GWBSTree> list, int a)
        {
            IList listPeriod = new ArrayList();
            if (list != null && list.Count > 0)
            {
                //List<GWBSTree> list = new List<GWBSTree>();
                PlanRequireType requireType = new PlanRequireType();
                if (a == 1)
                {
                    requireType = PlanRequireType.计划内需求;
                }
                if (a == 2)
                {
                    requireType = PlanRequireType.计划外需求;
                }

                //foreach (System.Collections.DictionaryEntry obj in ht)
                //{
                //    TreeNode tn = obj.Key as TreeNode;
                //    GWBSTree g = tn.Tag as GWBSTree;
                //    list.Add(g);
                //}

                var query = from q in list
                            orderby q.SysCode ascending
                            select q;

                ObjectQuery oq1 = new ObjectQuery();
                oq1.AddOrder(NHibernate.Criterion.Order.Desc("ProjectTaskCode"));
                oq1.AddFetchMode("SupplierId", NHibernate.FetchMode.Eager);
                IList OBSlist = dao.ObjectQuery(typeof(OBSService), oq1);

                foreach (GWBSTree gt in query)
                {
                    #region 查询得到滚动资源需求计划明细
                    //oq.Orders.Clear();
                    //oq.FetchModes.Clear();
                    ObjectQuery oq = new ObjectQuery();
                    Disjunction dis = new Disjunction();
                    oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", resPlan.Id));
                    oq.AddCriterion(Expression.Eq("State", ResourceRequirePlanDetailState.发布));
                    if (a != 3)
                    {
                        oq.AddCriterion(Expression.Eq("RequireType", requireType));
                    }
                    oq.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("TheResourceRequirePlan", NHibernate.FetchMode.Eager);
                    string[] sysCodes = gt.SysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < sysCodes.Length - 1; i++)
                    {
                        string sysCode = "";
                        for (int j = 0; j <= i; j++)
                        {
                            sysCode += sysCodes[j] + ".";
                        }
                        dis.Add(Expression.Eq("TheGWBSSysCode", sysCode));
                    }
                    dis.Add(Expression.Like("TheGWBSSysCode", gt.SysCode, MatchMode.Start)); //向下查
                    oq.AddCriterion(dis);
                    oq.AddCriterion(Expression.Like("ResourceCategory.SysCode", "%" + resReceipt.ResourceCategorySysCode + "%"));
                    IList listPlanDtl = Dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
                    #endregion

                    #region 追加资源需求计划单明细
                    if (listPlanDtl.Count > 0)
                    {
                        foreach (ResourceRequirePlanDetail planDtl in listPlanDtl)
                        {
                            bool flag = true;
                            ResourceRequirePlanDetail planDetail = CalculateGWBSRollingResourceDemandData(planDtl.TheResourceRequirePlan, gt, planDtl.MaterialResource, planDtl.DiagramNumber, planDtl.RequireType);
                            if (planDetail != null)
                            {
                                for (int i = 0; i < listPeriod.Count; i++)
                                {
                                    ResourceRequireReceiptDetail rrrDtl = listPeriod[i] as ResourceRequireReceiptDetail;
                                    if (rrrDtl.MaterialResource.Id == planDetail.MaterialResource.Id && rrrDtl.DiagramNumber == planDetail.DiagramNumber && planDetail.TheGWBSSysCode.Contains(rrrDtl.TheGWBSSysCode) && planDetail.RequireType == rrrDtl.RequireType)
                                    {
                                        flag = false;
                                        if (ClientUtil.ToInt(rrrDtl.PlannedCostQuantity) == ClientUtil.ToInt(planDetail.PlannedCostQuantity))
                                        {
                                            rrrDtl.TheGWBSSysCode = planDetail.TheGWBSSysCode;
                                            rrrDtl.TheGWBSTaskGUID = planDetail.TheGWBSTaskGUID;
                                            rrrDtl.TheGWBSTaskName = planDetail.TheGWBSTaskName;
                                        }
                                    }
                                }
                                if (flag)
                                {
                                    ResourceRequireReceiptDetail rDtl = new ResourceRequireReceiptDetail();
                                    rDtl.TheResReceipt = resReceipt;
                                    rDtl.TheGWBSTaskGUID = planDetail.TheGWBSTaskGUID;
                                    rDtl.TheGWBSTaskName = planDetail.TheGWBSTaskName;
                                    rDtl.TheGWBSSysCode = planDetail.TheGWBSSysCode;
                                    rDtl.MaterialResource = planDetail.MaterialResource;
                                    rDtl.MaterialName = planDetail.MaterialName;
                                    rDtl.MaterialCode = planDetail.MaterialCode;
                                    rDtl.MaterialSpec = planDetail.MaterialSpec;
                                    rDtl.MaterialStuff = planDetail.MaterialStuff;
                                    rDtl.MaterialCategory = planDetail.ResourceCategory;
                                    rDtl.MaterialCategoryName = planDetail.ResourceTypeClassification;
                                    rDtl.DiagramNumber = planDetail.DiagramNumber;
                                    rDtl.QuantityUnitGUID = planDetail.QuantityUnitGUID;
                                    rDtl.QuantityUnitName = planDetail.QuantityUnitName;
                                    rDtl.FirstOfferRequireQuantity = planDetail.FirstOfferRequireQuantity;
                                    rDtl.ResponsibilityCostQuantity = planDetail.ResponsibilityCostQuantity;
                                    rDtl.PlannedCostQuantity = planDetail.PlannedCostQuantity;
                                    rDtl.PlanInRequireQuantity = planDetail.PlanInRequireQuantity;
                                    rDtl.PlanOutRequireQuantity = planDetail.PlanOutRequireQuantity;
                                    rDtl.PeriodQuantity = 0;
                                    rDtl.CostQuantity = 0;
                                    rDtl.DailyPlanPublishQuantity = 0;
                                    rDtl.State = ResourceRequireReceiptDetailState.有效;
                                    rDtl.RequireType = planDetail.RequireType;
                                    if (resReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "1")
                                    {
                                        rDtl.Price = 0;
                                        if (OBSlist != null && OBSlist.Count > 0)
                                        {
                                            foreach (OBSService os in OBSlist)
                                            {
                                                if (rDtl.TheGWBSSysCode.Contains(os.ProjectTaskCode))
                                                {
                                                    rDtl.UsedRank = os.SupplierId;
                                                    rDtl.UsedRankName = os.SupplierName;
                                                }
                                            }
                                        }
                                    }
                                    if (resReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "2")
                                    {
                                        rDtl.Price = 0;
                                        if (OBSlist != null && OBSlist.Count > 0)
                                        {
                                            foreach (OBSService os in OBSlist)
                                            {
                                                if (rDtl.TheGWBSSysCode.Contains(os.ProjectTaskCode))
                                                {
                                                    rDtl.UsedRank = os.SupplierId;
                                                    rDtl.UsedRankName = os.SupplierName;
                                                }
                                            }
                                        }
                                    }
                                    if (resReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "3")
                                    {
                                        rDtl.Price = planDetail.Price;
                                    }
                                    rDtl.TechnicalParameters = planDetail.TechnicalParameters;
                                    rDtl.QualityStandards = planDetail.QualityStandards;
                                    listPeriod.Add(rDtl);
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            return listPeriod;
        }
        #endregion

        #region 根据期间需求计划生成物资资源需求计划
        /// <summary>
        /// 根据期间需求计划生成物资资源需求计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TransManager]
        public IList GenerateSupplyResourcePlan(string id)
        {
            ResourceRequireReceipt thePlanMaster = null;
            #region 资源类型分类(一级分类)
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Level", 3));
            IList listMaterial = dao.ObjectQuery(typeof(MaterialCategory), oq);
            #endregion

            #region 期间需求计划单
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.AddCriterion(Expression.Eq("Id", id));
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("Details.ResourceCategory", FetchMode.Eager);
            oq.AddFetchMode("Details.TheGWBSTaskGUID", FetchMode.Eager);
            IList lists = dao.ObjectQuery(typeof(ResourceRequireReceipt), oq);
            if (lists.Count > 0)
            {
                thePlanMaster = lists[0] as ResourceRequireReceipt;
            }
            string code = thePlanMaster.ResourceRequirePlanTypeCode;
            #endregion

            #region 工程任务(专业分类)
            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Not(Expression.Eq("SpecialType", "土建")));
            oq.AddCriterion(Expression.Not(Expression.Eq("SpecialType", "钢结构")));
            oq.AddCriterion(Expression.Not(Expression.Eq("SpecialType", "精装修")));
            oq.AddCriterion(Expression.IsNotNull("SpecialType"));
            //oq.AddOrder(NHibernate.Criterion.Order.Desc("Level"));
            oq.AddCriterion(Expression.Eq("TheProjectGUID", thePlanMaster.ProjectId));
            IList listGWBSTree = dao.ObjectQuery(typeof(GWBSTree), oq);
            #endregion

            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.AddCriterion(Expression.Eq("CostAccFlag", true));
            oq.AddOrder(NHibernate.Criterion.Order.Desc("Level"));
            oq.AddCriterion(Expression.Eq("TheProjectGUID", thePlanMaster.ProjectId));
            IList CostList = dao.ObjectQuery(typeof(GWBSTree), oq);

            string errMsg = "";
            IList lst = new ArrayList();
            IList listMsg = new ArrayList();
            listMsg.Add(errMsg);
            listMsg.Add(lst);

            #region 总量需求计划
            if (code.Substring(0, 1) != "3" && code.Substring(2, 1) == "0")
            {
                List<DemandMasterPlanMaster> listDemand = new List<DemandMasterPlanMaster>();
                foreach (ResourceRequireReceiptDetail dtl in thePlanMaster.Details)
                {
                    bool flag = false; //资源分类中是否只有物资分类
                    GWBSTree specialGwbs = new GWBSTree();//判断工程任务是否有专业分类
                    int slength = 0;
                    foreach (GWBSTree gt in listGWBSTree)
                    {
                        if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                        {
                            if (gt.SysCode.Length > slength)
                            {
                                specialGwbs = gt;
                                slength = gt.SysCode.Length;
                            }

                        }
                    }
                    foreach (MaterialCategory mat in listMaterial)
                    {
                        if (dtl.MaterialCategory.SysCode.Contains(mat.SysCode))
                        {
                            if (mat.ParentNode.Code == "01")
                            {
                                flag = true;
                            }
                        }
                    }
                    if (flag)
                    {
                        DemandMasterPlanDetail detail = new DemandMasterPlanDetail();
                        detail.IsOver = dtl.IsOver;
                        detail.MaterialCategory = dtl.MaterialCategory;
                        detail.MaterialCategoryName = dtl.MaterialCategoryName;
                        detail.MaterialCode = dtl.MaterialCode;
                        detail.MaterialName = dtl.MaterialName;
                        detail.MaterialResource = dtl.MaterialResource;
                        detail.MaterialSpec = dtl.MaterialSpec;
                        detail.MaterialStuff = dtl.MaterialStuff;
                        detail.MatStandardUnit = dtl.QuantityUnitGUID;
                        detail.MatStandardUnitName = dtl.QuantityUnitName;
                        detail.Money = dtl.Money;
                        detail.Price = dtl.Price;
                        detail.UsedPart = dtl.TheGWBSTaskGUID;
                        detail.UsedPartName = dtl.TheGWBSTaskName;
                        detail.UsedPartSysCode = dtl.TheGWBSSysCode;

                        foreach (GWBSTree gt in CostList)
                        {
                            if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                            {
                                detail.UsedPart = gt;
                                detail.UsedPartName = gt.Name;
                            }
                        }

                        detail.DiagramNumber = dtl.DiagramNumber;
                        detail.Quantity = dtl.PlanInRequireQuantity + dtl.PlanOutRequireQuantity;
                        detail.QualityStandard = dtl.QualityStandards;
                        detail.TechnologyParameter = dtl.TechnicalParameters;
                        detail.ForwardDetailId = dtl.Id;
                        detail.Descript = dtl.ApproachRequestDesc;
                        if (listDemand != null && listDemand.Count > 0)
                        {
                            bool dflag = false;
                            for (int i = 0; i < listDemand.Count; i++)
                            {
                                DemandMasterPlanMaster master = listDemand[i] as DemandMasterPlanMaster;
                                foreach (DemandMasterPlanDetail ddtl in listDemand[i].Details)
                                {
                                    if (specialGwbs.SpecialType == "")
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(master.MaterialCategory.SysCode))
                                        {
                                            if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource == ddtl.MaterialResource && detail.DiagramNumber == ddtl.DiagramNumber)
                                            {
                                                ddtl.Quantity += detail.Quantity;
                                                dflag = true;
                                                break;
                                            }
                                            else
                                            {
                                                detail.Master = master;
                                                master.Details.Add(detail);
                                                dflag = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (specialGwbs.SpecialType == master.SpecialType)
                                        {
                                            if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource == ddtl.MaterialResource && detail.DiagramNumber == ddtl.DiagramNumber)
                                            {
                                                ddtl.Quantity += detail.Quantity;
                                                dflag = true;
                                                break;
                                            }
                                            else
                                            {
                                                detail.Master = master;
                                                detail.SpecialType = master.SpecialType;
                                                master.Details.Add(detail);
                                                dflag = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!dflag)
                            {
                                DemandMasterPlanMaster master = new DemandMasterPlanMaster();
                                master.CreateDate = DateTime.Now;
                                master.CreateMonth = DateTime.Now.Month;
                                master.CreateYear = DateTime.Now.Year;
                                master.CreatePerson = thePlanMaster.HandlePerson;
                                master.CreatePersonName = thePlanMaster.HandlePersonName;
                                master.CurrencyType = thePlanMaster.CurrencyType;
                                master.Descript = thePlanMaster.Descript;
                                master.DocState = DocumentState.InAudit;
                                master.ExchangeRate = thePlanMaster.ExchangeRate;
                                master.ForwardBillCode = thePlanMaster.ForwardBillCode;
                                master.ForwardBillId = thePlanMaster.ForwardBillId;
                                master.ForwardBillType = thePlanMaster.ForwardBillType;
                                master.HandleOrg = thePlanMaster.HandleOrg;
                                master.HandlePerson = thePlanMaster.HandlePerson;
                                master.HandlePersonName = thePlanMaster.HandlePersonName;
                                master.IsFinished = thePlanMaster.IsFinished;
                                master.JBR = thePlanMaster.JBR;
                                master.LastModifyDate = thePlanMaster.LastModifyDate;
                                master.MaterialCategory = dtl.MaterialCategory;
                                master.MaterialCategoryCode = dtl.MaterialCategory.Code;
                                master.MaterialCategoryName = dtl.MaterialCategoryName;
                                master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                                master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                                master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                                master.PlanName = thePlanMaster.ReceiptName;
                                master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                                master.ProjectId = thePlanMaster.ProjectId;
                                master.ProjectName = thePlanMaster.ProjectName;
                                master.RealOperationDate = DateTime.Now;
                                master.SubmitDate = DateTime.Now;
                                if (specialGwbs.SpecialType == "")
                                {
                                    foreach (MaterialCategory m in listMaterial)
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(m.SysCode))
                                        {
                                            master.MaterialCategory = m;
                                            master.MaterialCategory.SysCode = m.SysCode;
                                            master.MaterialCategoryName = m.Name;
                                            break;
                                        }
                                    }
                                    master.ClassifyCode = master.MaterialCategory.Code;
                                    master.Code = GetCode(typeof(DemandMasterPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                    master.Special = "土建";
                                }
                                else
                                {
                                    master.ClassifyCode = specialGwbs.SysCode;
                                    master.Code = GetCode(typeof(DemandMasterPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                    master.Special = "安装";
                                    master.SpecialType = specialGwbs.SpecialType;
                                }
                                detail.Master = master;
                                master.Details.Add(detail);
                                listDemand.Add(master);
                            }
                        }
                        else
                        {
                            DemandMasterPlanMaster master = new DemandMasterPlanMaster();
                            master.CreateDate = DateTime.Now;
                            master.RealOperationDate = DateTime.Now;
                            master.CreateMonth = DateTime.Now.Month;
                            master.CreateYear = DateTime.Now.Year;
                            master.CreatePerson = thePlanMaster.HandlePerson;
                            master.CreatePersonName = thePlanMaster.HandlePersonName;
                            master.CurrencyType = thePlanMaster.CurrencyType;
                            master.Descript = thePlanMaster.Descript;
                            master.DocState = DocumentState.InAudit;
                            master.ExchangeRate = thePlanMaster.ExchangeRate;
                            master.ForwardBillCode = thePlanMaster.Code;
                            master.ForwardBillId = thePlanMaster.Id;
                            master.HandleOrg = thePlanMaster.HandleOrg;
                            master.HandlePerson = thePlanMaster.HandlePerson;
                            master.HandlePersonName = thePlanMaster.HandlePersonName;
                            master.IsFinished = thePlanMaster.IsFinished;
                            master.JBR = thePlanMaster.JBR;
                            master.LastModifyDate = thePlanMaster.LastModifyDate;
                            master.MaterialCategory = dtl.MaterialCategory;
                            master.MaterialCategoryCode = dtl.MaterialCategory.Code;
                            master.MaterialCategoryName = dtl.MaterialCategoryName;
                            master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                            master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                            master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                            master.PlanName = thePlanMaster.ReceiptName;
                            master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                            master.ProjectId = thePlanMaster.ProjectId;
                            master.ProjectName = thePlanMaster.ProjectName;
                            master.SubmitDate = DateTime.Now;
                            if (specialGwbs.SpecialType == "")
                            {
                                foreach (MaterialCategory m in listMaterial)
                                {
                                    if (detail.MaterialCategory.SysCode.Contains(m.SysCode))
                                    {
                                        master.MaterialCategory = m;
                                        master.MaterialCategory.SysCode = m.SysCode;
                                        master.MaterialCategoryName = m.Name;
                                        break;
                                    }
                                }
                                master.ClassifyCode = master.MaterialCategory.Code;
                                master.Code = GetCode(typeof(DemandMasterPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                master.Special = "土建";
                            }
                            else
                            {
                                master.ClassifyCode = specialGwbs.SysCode;
                                master.Code = GetCode(typeof(DemandMasterPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                master.Special = "安装";
                                master.SpecialType = specialGwbs.SpecialType;
                            }
                            detail.Master = master;
                            master.Details.Add(detail);
                            listDemand.Add(master);
                        }
                    }
                }
                if (listDemand != null && listDemand.Count > 0)
                {
                    bool mflag = false;
                    foreach (DemandMasterPlanMaster mat in listDemand)
                    {
                        dao.SaveOrUpdate(mat);
                        mflag = true;
                    }
                    if (mflag)
                    {
                        listMsg[1] = listDemand;
                    }
                }
                else
                {
                    errMsg = "明细中的资源类型不属于物资资源一级分类下的资源类型，生成失败";
                    listMsg[0] = errMsg;
                    return listMsg;
                }
            }
            #endregion

            #region 日常需求计划
            else if (code.Substring(2, 1) == "1")
            {
                List<DailyPlanMaster> listDaily = new List<DailyPlanMaster>();
                foreach (ResourceRequireReceiptDetail dtl in thePlanMaster.Details)
                {
                    bool flag = false; //判断资源类型是否属于物资资源分类的一级分类
                    GWBSTree specialGwbs = new GWBSTree();//判断工程任务是否有专业分类
                    int slength = 0;
                    foreach (GWBSTree gt in listGWBSTree)
                    {
                        if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                        {
                            if (gt.SysCode.Length > slength)
                            {
                                specialGwbs = gt;
                                slength = gt.SysCode.Length;
                            }
                            
                        }
                    }
                    foreach (MaterialCategory mat in listMaterial)
                    {
                        if (dtl.MaterialCategory.SysCode.Contains(mat.SysCode))
                        {
                            if (mat.ParentNode.Code == "01")
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        #region 添加明细信息
                        DailyPlanDetail detail = new DailyPlanDetail();
                        detail.Descript = dtl.ApproachRequestDesc;
                        detail.IsOver = dtl.IsOver;
                        detail.MaterialCategory = dtl.MaterialCategory;
                        detail.MaterialCategoryName = dtl.MaterialCategoryName;
                        detail.MaterialCode = dtl.MaterialCode;
                        detail.MaterialName = dtl.MaterialName;
                        detail.MaterialResource = dtl.MaterialResource;
                        detail.MaterialSpec = dtl.MaterialSpec;
                        detail.MaterialStuff = dtl.MaterialStuff;
                        detail.UsedPart = dtl.TheGWBSTaskGUID;
                        detail.UsedPartName = dtl.TheGWBSTaskName;
                        foreach (GWBSTree gwbs in CostList)
                        {
                            if (detail.UsedPart.SysCode.Contains(gwbs.SysCode))
                            {
                                detail.UsedPart = gwbs;
                                detail.UsedPartName = gwbs.Name;
                                detail.ProjectTask = gwbs;
                                detail.ProjectTaskName = gwbs.Name;
                                detail.ProjectTaskSysCode = gwbs.SysCode;
                                break;
                            }
                        }
                        detail.Quantity = dtl.PeriodQuantity;
                        detail.LeftQuantity = dtl.PeriodQuantity;
                        detail.Price = dtl.Price;
                        detail.DiagramNumber = dtl.DiagramNumber;
                        detail.MatStandardUnit = dtl.QuantityUnitGUID;
                        detail.MatStandardUnitName = dtl.QuantityUnitName;
                        detail.SpecialType = dtl.TheGWBSTaskGUID.SpecialType;
                        detail.QualityStandard = dtl.QualityStandards;
                        if (dtl.UsedRank != null)
                        {
                            detail.UsedRankName = dtl.UsedRankName;
                            detail.UsedRank = dtl.UsedRank.BearerOrg;
                        }
                        detail.ForwardDetailId = dtl.Id;
                        detail.Descript = dtl.ApproachRequestDesc;
                        detail.ApproachDate = dtl.ApproachDate;
                        #endregion
                        #region 判断是否有相同的一级分类
                        if (listDaily != null && listDaily.Count > 0)
                        {
                            bool dflag = false; //判断是否有相同的一级分类
                            for (int i = 0; i < listDaily.Count; i++)
                            {
                                if (!dflag)
                                {
                                    DailyPlanMaster master = listDaily[i] as DailyPlanMaster;
                                    foreach (DailyPlanDetail ddtl in listDaily[i].Details)
                                    {
                                        if (specialGwbs.SpecialType == "")
                                        {
                                            if (detail.MaterialCategory.SysCode.Contains(master.MaterialCategory.SysCode))
                                            {
                                                if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource == ddtl.MaterialResource && detail.DiagramNumber == ddtl.DiagramNumber)
                                                {
                                                    ddtl.Quantity += detail.Quantity;
                                                    ddtl.LeftQuantity += detail.LeftQuantity;
                                                    dflag = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    detail.Master = master;
                                                    master.Details.Add(detail);
                                                    dflag = true;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (specialGwbs.SpecialType == master.SpecialType)
                                            {
                                                if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource.Id == ddtl.MaterialResource.Id && detail.DiagramNumber == ddtl.DiagramNumber)
                                                {
                                                    ddtl.Quantity += detail.Quantity;
                                                    ddtl.LeftQuantity += detail.LeftQuantity;
                                                    dflag = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    detail.Master = master;
                                                    detail.SpecialType = master.SpecialType;
                                                    master.Details.Add(detail);
                                                    dflag = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (!dflag)
                            {
                                DailyPlanMaster master = new DailyPlanMaster();
                                master.CreateDate = DateTime.Now;
                                master.RealOperationDate = DateTime.Now;
                                master.CreateMonth = DateTime.Now.Month;
                                master.CreateYear = DateTime.Now.Year;
                                master.CreatePerson = thePlanMaster.HandlePerson;
                                master.CreatePersonName = thePlanMaster.HandlePersonName;
                                master.Descript = thePlanMaster.Descript;
                                master.DocState = DocumentState.InAudit;
                                master.HandlePerson = thePlanMaster.HandlePerson;
                                master.HandlePersonName = thePlanMaster.HandlePersonName;
                                master.IsFinished = thePlanMaster.IsFinished;
                                master.JBR = thePlanMaster.JBR;
                                master.LastModifyDate = thePlanMaster.LastModifyDate;
                                master.MaterialCategory = detail.MaterialCategory;
                                master.MaterialCategoryCode = detail.MaterialCategory.Code;
                                master.MaterialCategoryName = detail.MaterialCategoryName;
                                master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                                master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                                master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                                master.PlanName = thePlanMaster.ReceiptName;
                                master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                                master.SpecialType = detail.SpecialType;
                                master.ProjectId = thePlanMaster.ProjectId;
                                master.ProjectName = thePlanMaster.ProjectName;
                                master.ForwardBillId = thePlanMaster.Id;
                                master.ForwardBillCode = thePlanMaster.Code;
                                master.SubmitDate = DateTime.Now;
                                if (specialGwbs.SpecialType != "")
                                {
                                    //安装
                                    master.ClassifyCode = specialGwbs.SysCode;
                                    master.Code = GetCode(typeof(DailyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                    master.Special = "安装";
                                    master.SpecialType = specialGwbs.SpecialType;
                                }
                                else
                                {
                                    foreach (MaterialCategory mat in listMaterial)
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(mat.SysCode))
                                        {
                                            master.MaterialCategory = mat;
                                            master.MaterialCategoryCode = mat.SysCode;
                                            master.MaterialCategoryName = mat.Name;
                                            break;
                                        }
                                    }
                                    master.ClassifyCode = detail.MaterialCategory.Code;
                                    master.Code = GetCode(typeof(DailyPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                    master.Special = "土建";
                                }
                                detail.Master = master;
                                master.Details.Add(detail);
                                listDaily.Add(master);
                            }
                        }
                        else
                        {
                            DailyPlanMaster master = new DailyPlanMaster();
                            master.CreateDate = DateTime.Now;
                            master.RealOperationDate = DateTime.Now;
                            master.CreateMonth = DateTime.Now.Month;
                            master.CreateYear = DateTime.Now.Year;
                            master.CreatePerson = thePlanMaster.HandlePerson;
                            master.CreatePersonName = thePlanMaster.HandlePersonName;
                            master.Descript = thePlanMaster.Descript;
                            master.DocState = DocumentState.InAudit;
                            master.HandlePerson = thePlanMaster.HandlePerson;
                            master.HandlePersonName = thePlanMaster.HandlePersonName;
                            master.IsFinished = thePlanMaster.IsFinished;
                            master.MaterialCategory = detail.MaterialCategory;
                            master.MaterialCategoryCode = detail.MaterialCategory.Code;
                            master.MaterialCategoryName = detail.MaterialCategoryName;
                            master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                            master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                            master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                            master.PlanName = thePlanMaster.ReceiptName;
                            master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                            master.ProjectId = thePlanMaster.ProjectId;
                            master.ProjectName = thePlanMaster.ProjectName;
                            master.ForwardBillId = thePlanMaster.Id;
                            master.ForwardBillCode = thePlanMaster.Code;
                            master.SubmitDate = DateTime.Now;
                            if (specialGwbs.SpecialType != "")
                            {
                                master.ClassifyCode = specialGwbs.SysCode;
                                master.Code = GetCode(typeof(DailyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                master.Special = "安装";
                                master.SpecialType = specialGwbs.SpecialType;
                            }
                            else
                            {
                                foreach (MaterialCategory mat in listMaterial)
                                {
                                    if (detail.MaterialCategory.SysCode.Contains(mat.SysCode))
                                    {
                                        master.MaterialCategory = mat;
                                        master.MaterialCategoryCode = mat.SysCode;
                                        master.MaterialCategoryName = mat.Name;
                                        break;
                                    }
                                }
                                master.ClassifyCode = master.MaterialCategory.Code;
                                master.Code = GetCode(typeof(DailyPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                master.Special = "土建";

                            }
                            detail.Master = master;
                            master.Details.Add(detail);
                            listDaily.Add(master);
                        }
                        #endregion
                    }
                }

                if (listDaily != null && listDaily.Count > 0)
                {
                    bool mflag = false;
                    foreach (DailyPlanMaster mat in listDaily)
                    {
                        dao.SaveOrUpdate(mat);
                        mflag = true;
                    }
                    if (mflag)
                    {
                        listMsg[1] = listDaily;
                    }

                }
                else
                {
                    errMsg = "明细中的资源类型不属于物资资源一级分类下的资源类型，生成失败";
                    listMsg[0] = errMsg;
                    return listMsg;
                }
            }
            #endregion

            #region 月度需求计划
            else if (code.Substring(2, 1) == "2")
            {
                bool mflag = false;
                List<MonthlyPlanMaster> listMonth = new List<MonthlyPlanMaster>();
                foreach (ResourceRequireReceiptDetail dtl in thePlanMaster.Details)
                {
                    GWBSTree specialGwbs = new GWBSTree();//判断工程任务是否有专业分类
                    int slength = 0;
                    foreach (GWBSTree gt in listGWBSTree)
                    {
                        if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                        {
                            if (gt.SysCode.Length > slength)
                            {
                                specialGwbs = gt;
                                slength = gt.SysCode.Length;
                            }

                        }
                    }
                    foreach (MaterialCategory mat in listMaterial)
                    {
                        if (dtl.MaterialCategory.SysCode.Contains(mat.SysCode))
                        {
                            if (mat.ParentNode.Code == "01")
                            {
                                mflag = true;
                            }
                        }
                    }
                    if (mflag)
                    {
                        MonthlyPlanDetail detail = new MonthlyPlanDetail();
                        //detail.ProjectTask = dtl.TheGWBSTaskGUID;
                        //detail.ProjectTaskName = dtl.TheGWBSTaskName;
                        //detail.ProjectTaskSysCode = dtl.TheGWBSSysCode;
                        detail.MaterialCategory = dtl.MaterialCategory;
                        detail.MaterialCategoryName = dtl.MaterialCategoryName;
                        detail.MaterialCode = dtl.MaterialCode;
                        detail.MaterialName = dtl.MaterialName;
                        detail.MaterialResource = dtl.MaterialResource;
                        detail.MaterialSpec = dtl.MaterialSpec;
                        detail.MaterialStuff = dtl.MaterialStuff;
                        detail.MatStandardUnit = dtl.QuantityUnitGUID;
                        detail.MatStandardUnitName = dtl.QuantityUnitName;
                        detail.UsedPart = dtl.TheGWBSTaskGUID;
                        detail.UsedPartName = dtl.TheGWBSTaskName;
                        foreach (GWBSTree gt in CostList)
                        {
                            if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                            {
                                detail.UsedPart = gt;
                                detail.UsedPartName = gt.Name;
                                detail.ProjectTask = gt;
                                detail.ProjectTaskName = gt.Name;
                                detail.ProjectTaskSysCode = gt.SysCode;
                                break;
                            }
                        }
                        detail.Quantity = dtl.PeriodQuantity;
                        detail.NeedQuantity = dtl.PeriodQuantity;
                        detail.LeftQuantity = dtl.PeriodQuantity;
                        detail.DiagramNumber = dtl.DiagramNumber;
                        detail.ForwardDetailId = dtl.TheResReceipt.Id;
                        detail.SpecialType = dtl.TheGWBSTaskGUID.SpecialType;
                        detail.QualityStandard = dtl.QualityStandards;
                        detail.ForwardDetailId = dtl.Id;
                        if (dtl.UsedRank != null)
                        {
                            detail.UsedRankName = dtl.UsedRankName;
                            detail.UsedRank = dtl.UsedRank.BearerOrg;
                        }
                        detail.Descript = dtl.ApproachRequestDesc;
                        if (listMonth != null && listMonth.Count > 0)
                        {
                            bool flag = false;
                            for (int i = 0; i < listMonth.Count; i++)
                            {
                                MonthlyPlanMaster master = listMonth[i] as MonthlyPlanMaster;
                                List<MonthlyPlanDetail> ddtlList = listMonth[i].Details.OfType<MonthlyPlanDetail>().ToList<MonthlyPlanDetail>();
                                foreach (MonthlyPlanDetail ddtl in ddtlList)
                                {
                                    if (specialGwbs.SpecialType == "")
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(master.MaterialCategory.SysCode))
                                        {
                                            if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource.Id == ddtl.MaterialResource.Id && detail.DiagramNumber == ddtl.DiagramNumber)
                                            {
                                                detail.Quantity += ddtl.Quantity;
                                                detail.LeftQuantity += ddtl.LeftQuantity;
                                                detail.NeedQuantity += ddtl.NeedQuantity;
                                                flag = true;
                                                break;
                                            }
                                            else
                                            {
                                                detail.Master = master;
                                                master.Details.Add(detail);
                                                flag = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (specialGwbs.SpecialType == master.SpecialType)
                                        {
                                            if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource.Id == ddtl.MaterialResource.Id && detail.DiagramNumber == ddtl.DiagramNumber)
                                            {
                                                detail.Quantity += ddtl.Quantity;
                                                detail.LeftQuantity += ddtl.LeftQuantity;
                                                detail.NeedQuantity += ddtl.NeedQuantity;
                                                flag = true;
                                                break;
                                            }
                                            else
                                            {
                                                detail.Master = master;
                                                detail.SpecialType = master.SpecialType;
                                                master.Details.Add(detail);
                                                flag = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!flag)
                            {
                                MonthlyPlanMaster master = new MonthlyPlanMaster();
                                master.MonthePlanType = thePlanMaster.ResourceRequirePlanTypeWord;
                                master.CreateDate = DateTime.Now;
                                master.RealOperationDate = DateTime.Now;
                                master.CreateMonth = DateTime.Now.Month;
                                master.CreateYear = DateTime.Now.Year;
                                master.CreatePerson = thePlanMaster.HandlePerson;
                                master.CreatePersonName = thePlanMaster.HandlePersonName;
                                master.Descript = thePlanMaster.Descript;
                                master.DocState = DocumentState.InAudit;
                                master.ExchangeRate = thePlanMaster.ExchangeRate;
                                master.ForwardBillCode = thePlanMaster.Code;
                                master.ForwardBillId = thePlanMaster.Id;
                                master.HandlePerson = thePlanMaster.HandlePerson;
                                master.HandlePersonName = thePlanMaster.HandlePersonName;
                                master.MaterialCategory = dtl.MaterialCategory;
                                master.MaterialCategoryCode = dtl.MaterialCategory.Code;
                                master.MaterialCategoryName = dtl.MaterialCategoryName;
                                master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                                master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                                master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                                master.PlanName = thePlanMaster.ReceiptName;
                                master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                                master.ProjectId = thePlanMaster.ProjectId;
                                master.ProjectName = thePlanMaster.ProjectName;
                                master.SubmitDate = DateTime.Now;
                                if (specialGwbs.SpecialType != "")
                                {
                                    master.ClassifyCode = specialGwbs.SysCode;
                                    master.Code = GetCode(typeof(MonthlyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                    master.Special = "安装";
                                    master.SpecialType = specialGwbs.SpecialType;
                                }
                                else
                                {
                                    foreach (MaterialCategory mat in listMaterial)
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(mat.SysCode))
                                        {
                                            master.MaterialCategory = mat;
                                            master.MaterialCategoryCode = mat.SysCode;
                                            master.MaterialCategoryName = mat.Name;
                                            break;
                                        }
                                    }
                                    master.ClassifyCode = master.MaterialCategory.Code;
                                    master.Code = GetCode(typeof(MonthlyPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                    master.Special = "土建";
                                }
                                detail.Master = master;
                                master.Details.Add(detail);
                                listMonth.Add(master);
                            }
                        }
                        else
                        {
                            MonthlyPlanMaster master = new MonthlyPlanMaster();
                            master.MonthePlanType = thePlanMaster.ResourceRequirePlanTypeWord;
                            master.CreateDate = DateTime.Now;
                            master.RealOperationDate = DateTime.Now;
                            master.CreateMonth = DateTime.Now.Month;
                            master.CreateYear = DateTime.Now.Year;
                            master.CreatePerson = thePlanMaster.HandlePerson;
                            master.CreatePersonName = thePlanMaster.HandlePersonName;
                            master.Descript = thePlanMaster.Descript;
                            master.DocState = DocumentState.InAudit;
                            master.HandlePerson = thePlanMaster.HandlePerson;
                            master.HandlePersonName = thePlanMaster.HandlePersonName;
                            master.MaterialCategory = dtl.MaterialCategory;
                            master.MaterialCategoryCode = dtl.MaterialCategory.Code;
                            master.MaterialCategoryName = dtl.MaterialCategoryName;
                            master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                            master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                            master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                            master.PlanName = thePlanMaster.ReceiptName;
                            master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                            master.ProjectId = thePlanMaster.ProjectId;
                            master.ProjectName = thePlanMaster.ProjectName;
                            master.SubmitDate = DateTime.Now;
                            if (specialGwbs.SpecialType != "")
                            {
                                master.ClassifyCode = specialGwbs.SysCode;
                                master.Code = GetCode(typeof(MonthlyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                master.Special = "安装";
                                master.SpecialType = specialGwbs.SpecialType;
                            }
                            else
                            {
                                foreach (MaterialCategory mat in listMaterial)
                                {
                                    if (detail.MaterialCategory.SysCode.Contains(mat.SysCode))
                                    {
                                        master.MaterialCategory = mat;
                                        master.MaterialCategoryCode = mat.SysCode;
                                        master.MaterialCategoryName = mat.Name;
                                        break;
                                    }
                                }
                                master.ClassifyCode = master.MaterialCategory.Code;
                                master.Code = GetCode(typeof(MonthlyPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                master.Special = "土建";
                            }
                            detail.Master = master;
                            master.Details.Add(detail);
                            listMonth.Add(master);
                        }
                    }
                }
                if (listMonth != null && listMonth.Count > 0)
                {
                    bool monflag = false;
                    foreach (MonthlyPlanMaster mat in listMonth)
                    {
                        dao.SaveOrUpdate(mat);
                        monflag = true;
                    }
                    if (monflag)
                    {
                        listMsg[1] = listMonth;
                    }
                }
                else
                {
                    errMsg = "明细中的资源类型不属于物资资源一级分类下的资源类型，生成失败";
                    listMsg[0] = errMsg;
                    return listMsg;
                }
            }
            #endregion

            #region 专业需求计划
            else if (code.Substring(0, 1) == "3")
            {
                List<SupplyPlanMaster> listSupply = new List<SupplyPlanMaster>();
                foreach (ResourceRequireReceiptDetail dtl in thePlanMaster.Details)
                {
                    GWBSTree specialGwbs = new GWBSTree();//判断工程任务是否有专业分类
                    int slength = 0;
                    foreach (GWBSTree gt in listGWBSTree)
                    {
                        if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                        {
                            if (gt.SysCode.Length > slength)
                            {
                                specialGwbs = gt;
                                slength = gt.SysCode.Length;
                            }

                        }
                    }
                    if (specialGwbs.SpecialType != "")
                    {
                        SupplyPlanDetail detail = new SupplyPlanDetail();
                        detail.MaterialResource = dtl.MaterialResource;
                        detail.MaterialName = dtl.MaterialName;
                        detail.MaterialCode = dtl.MaterialCode;
                        detail.MaterialSpec = dtl.MaterialSpec;
                        detail.MaterialStuff = dtl.MaterialStuff;
                        detail.DiagramNumber = dtl.DiagramNumber;
                        detail.UsedPart = dtl.TheGWBSTaskGUID;
                        detail.UsedPartName = dtl.TheGWBSTaskName;
                        foreach (GWBSTree gt in CostList)
                        {
                            if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                            {
                                detail.UsedPart = gt;
                                detail.UsedPartName = gt.Name;
                                detail.SysCode = gt.SysCode;
                                break;
                            }
                        }
                        detail.ForwardDetailId = dtl.Id;
                        detail.MatStandardUnit = dtl.QuantityUnitGUID;
                        detail.MatStandardUnitName = dtl.QuantityUnitName;
                        detail.Price = dtl.Price;
                        detail.SpecialType = dtl.TheGWBSTaskGUID.SpecialType;
                        detail.Quantity = dtl.PlanOutRequireQuantity + dtl.PlanInRequireQuantity;
                        detail.LeftQuantity = dtl.PlanOutRequireQuantity + dtl.PlanInRequireQuantity;
                        detail.QualityStandard = dtl.QualityStandards;
                        detail.TechnologyParameter = dtl.TechnicalParameters;
                        if (listSupply != null && listSupply.Count > 0)
                        {
                            bool flag = false;
                            for (int i = 0; i < listSupply.Count; i++)
                            {
                                SupplyPlanMaster master = listSupply[i] as SupplyPlanMaster;
                                foreach (SupplyPlanDetail ddtl in master.Details)
                                {
                                    if (specialGwbs.SpecialType == master.SpecialType)
                                    {
                                        if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource.Id == ddtl.MaterialResource.Id && detail.DiagramNumber == ddtl.DiagramNumber)
                                        {
                                            detail.Quantity += ddtl.Quantity;
                                            detail.LeftQuantity += ddtl.LeftQuantity;
                                            flag = true;
                                            break;
                                        }
                                        else
                                        {
                                            detail.Master = master;
                                            detail.SpecialType = master.SpecialType;
                                            master.Details.Add(detail);
                                            flag = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (!flag)
                            {
                                SupplyPlanMaster master = new SupplyPlanMaster();
                                master.DocState = DocumentState.InExecute;
                                master.CreateDate = DateTime.Now;
                                master.RealOperationDate = DateTime.Now;
                                master.CreateMonth = DateTime.Now.Month;
                                master.CreateYear = DateTime.Now.Year;
                                master.CreatePerson = thePlanMaster.CreatePerson;
                                master.CreatePersonName = thePlanMaster.CreatePersonName;
                                master.HandlePerson = thePlanMaster.HandlePerson;
                                master.HandlePersonName = thePlanMaster.HandlePersonName;
                                master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                                master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                                master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                                master.MaterialCategory = thePlanMaster.ResourceCategory;
                                master.MaterialCategoryName = thePlanMaster.ResourceCategory.Name;
                                master.MaterialCategoryCode = thePlanMaster.ResourceCategorySysCode;
                                master.ProjectId = thePlanMaster.ProjectId;
                                master.ProjectName = thePlanMaster.ProjectName;
                                master.ForwardBillId = thePlanMaster.Id;
                                master.ForwardBillCode = thePlanMaster.Code;
                                master.ClassifyCode = specialGwbs.SysCode;
                                master.CreatePerson = thePlanMaster.HandlePerson;
                                master.CreatePersonName = thePlanMaster.HandlePersonName;
                                master.PlanName = thePlanMaster.ReceiptName;
                                master.Code = GetCode(typeof(SupplyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                master.SpecialType = specialGwbs.SpecialType;
                                detail.SpecialType = specialGwbs.SpecialType;
                                detail.Master = master;
                                master.Details.Add(detail);
                                listSupply.Add(master);
                            }
                        }
                        else
                        {
                            SupplyPlanMaster master = new SupplyPlanMaster();
                            master.DocState = DocumentState.InAudit;
                            master.CreateDate = DateTime.Now;
                            master.RealOperationDate = DateTime.Now;
                            master.CreateMonth = DateTime.Now.Month;
                            master.CreateYear = DateTime.Now.Year;
                            master.CreatePerson = thePlanMaster.CreatePerson;
                            master.CreatePersonName = thePlanMaster.CreatePersonName;
                            master.HandlePerson = thePlanMaster.HandlePerson;
                            master.HandlePersonName = thePlanMaster.HandlePersonName;
                            master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                            master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                            master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                            master.MaterialCategory = thePlanMaster.ResourceCategory;
                            master.MaterialCategoryName = thePlanMaster.ResourceCategory.Name;
                            master.MaterialCategoryCode = thePlanMaster.ResourceCategorySysCode;
                            master.ProjectId = thePlanMaster.ProjectId;
                            master.ProjectName = thePlanMaster.ProjectName;
                            master.ForwardBillId = thePlanMaster.Id;
                            master.ForwardBillCode = thePlanMaster.Code;
                            master.CreatePerson = thePlanMaster.HandlePerson;
                            master.CreatePersonName = thePlanMaster.HandlePersonName;
                            master.PlanName = thePlanMaster.ReceiptName;
                            master.Code = GetCode(typeof(SupplyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                            master.SpecialType = specialGwbs.SpecialType;
                            detail.SpecialType = specialGwbs.SpecialType;
                            master.ClassifyCode = specialGwbs.SysCode;
                            detail.Master = master;
                            master.Details.Add(detail);
                            listSupply.Add(master);
                        }
                    }
                }
                if (listSupply != null && listSupply.Count > 0)
                {
                    bool supflag = false;
                    foreach (SupplyPlanMaster mat in listSupply)
                    {
                        dao.SaveOrUpdate(mat);
                        supflag = true;
                    }
                    if (supflag)
                    {
                        listMsg[1] = listSupply;
                    }
                }
                else
                {
                    errMsg = "明细上的工程任务或该工程任务的父节点的专业分类不属于安装专业分类，无法生成！";
                    listMsg[0] = errMsg;
                    return listMsg;
                }

            }
            #endregion
            return listMsg;
        }

        /// <summary>
        /// 根据期间需求计划生成物资资源需求计划  VResourcesDemandPlanManagement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TransManager]
        public IList GenerateSupplyResourcePlanNew(string id)
        {
            ResourceRequireReceipt thePlanMaster = null;
            #region 资源类型分类(一级分类)
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Level", 3));
            IList listMaterial = dao.ObjectQuery(typeof(MaterialCategory), oq);
            #endregion

            #region 期间需求计划单
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.AddCriterion(Expression.Eq("Id", id));
            oq.AddFetchMode("Details", FetchMode.Eager);
            oq.AddFetchMode("Details.ResourceCategory", FetchMode.Eager);
            oq.AddFetchMode("Details.TheGWBSTaskGUID", FetchMode.Eager);
            IList lists = dao.ObjectQuery(typeof(ResourceRequireReceipt), oq);
            if (lists.Count > 0)
            {
                thePlanMaster = lists[0] as ResourceRequireReceipt;
            }
            //string code = thePlanMaster.ResourceRequirePlanTypeCode;

            #endregion

            #region 工程任务(专业分类)
            oq.Criterions.Clear();
            oq.AddCriterion(Expression.Not(Expression.Eq("SpecialType", "土建")));
            oq.AddCriterion(Expression.Not(Expression.Eq("SpecialType", "钢结构")));
            oq.AddCriterion(Expression.Not(Expression.Eq("SpecialType", "精装修")));
            oq.AddCriterion(Expression.IsNotNull("SpecialType"));
            //oq.AddOrder(NHibernate.Criterion.Order.Desc("Level"));
            oq.AddCriterion(Expression.Eq("TheProjectGUID", thePlanMaster.ProjectId));
            IList listGWBSTree = dao.ObjectQuery(typeof(GWBSTree), oq);
            #endregion

            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            oq.AddCriterion(Expression.Eq("CostAccFlag", true));
            oq.AddOrder(NHibernate.Criterion.Order.Desc("Level"));
            oq.AddCriterion(Expression.Eq("TheProjectGUID", thePlanMaster.ProjectId));
            IList CostList = dao.ObjectQuery(typeof(GWBSTree), oq);

            string errMsg = "";
            IList lst = new ArrayList();
            IList listMsg = new ArrayList();
            listMsg.Add(errMsg);
            listMsg.Add(lst);

            #region 总量需求计划
            //if (code.Substring(0, 1) != "3" && code.Substring(2, 1) == "0")
            if(thePlanMaster.StagePlanType == PlanType.总体计划)
            {
                List<DemandMasterPlanMaster> listDemand = new List<DemandMasterPlanMaster>();
                foreach (ResourceRequireReceiptDetail dtl in thePlanMaster.Details)
                {
                    bool flag = false; //资源分类中是否只有物资分类
                    GWBSTree specialGwbs = new GWBSTree();//判断工程任务是否有专业分类
                    int slength = 0;
                    foreach (GWBSTree gt in listGWBSTree)
                    {
                        if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                        {
                            if (gt.SysCode.Length > slength)
                            {
                                specialGwbs = gt;
                                slength = gt.SysCode.Length;
                            }

                        }
                    }
                    foreach (MaterialCategory mat in listMaterial)
                    {
                        if (dtl.MaterialCategory.SysCode.Contains(mat.SysCode))
                        {
                            if (mat.ParentNode.Code == "01")
                            {
                                flag = true;
                            }
                        }
                    }
                    if (flag)
                    {
                        DemandMasterPlanDetail detail = new DemandMasterPlanDetail();
                        detail.IsOver = dtl.IsOver;
                        detail.MaterialCategory = dtl.MaterialCategory;
                        detail.MaterialCategoryName = dtl.MaterialCategoryName;
                        detail.MaterialCode = dtl.MaterialCode;
                        detail.MaterialName = dtl.MaterialName;
                        detail.MaterialResource = dtl.MaterialResource;
                        detail.MaterialSpec = dtl.MaterialSpec;
                        detail.MaterialStuff = dtl.MaterialStuff;
                        detail.MatStandardUnit = dtl.QuantityUnitGUID;
                        detail.MatStandardUnitName = dtl.QuantityUnitName;
                        detail.Money = dtl.Money;
                        detail.Price = dtl.Price;
                        detail.UsedPart = dtl.TheGWBSTaskGUID;
                        detail.UsedPartName = dtl.TheGWBSTaskName;
                        detail.UsedPartSysCode = dtl.TheGWBSSysCode;

                        foreach (GWBSTree gt in CostList)
                        {
                            if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                            {
                                detail.UsedPart = gt;
                                detail.UsedPartName = gt.Name;
                            }
                        }

                        detail.DiagramNumber = dtl.DiagramNumber;
                        detail.Quantity = dtl.PlanInRequireQuantity + dtl.PlanOutRequireQuantity;
                        detail.QualityStandard = dtl.QualityStandards;
                        detail.TechnologyParameter = dtl.TechnicalParameters;
                        detail.ForwardDetailId = dtl.Id;
                        detail.Descript = dtl.ApproachRequestDesc;
                        if (listDemand != null && listDemand.Count > 0)
                        {
                            bool dflag = false;
                            for (int i = 0; i < listDemand.Count; i++)
                            {
                                DemandMasterPlanMaster master = listDemand[i] as DemandMasterPlanMaster;
                                foreach (DemandMasterPlanDetail ddtl in listDemand[i].Details)
                                {
                                    if (specialGwbs.SpecialType == "")
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(master.MaterialCategory.SysCode))
                                        {
                                            if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource == ddtl.MaterialResource && detail.DiagramNumber == ddtl.DiagramNumber)
                                            {
                                                ddtl.Quantity += detail.Quantity;
                                                dflag = true;
                                                break;
                                            }
                                            else
                                            {
                                                detail.Master = master;
                                                master.Details.Add(detail);
                                                dflag = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (specialGwbs.SpecialType == master.SpecialType)
                                        {
                                            if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource == ddtl.MaterialResource && detail.DiagramNumber == ddtl.DiagramNumber)
                                            {
                                                ddtl.Quantity += detail.Quantity;
                                                dflag = true;
                                                break;
                                            }
                                            else
                                            {
                                                detail.Master = master;
                                                detail.SpecialType = master.SpecialType;
                                                master.Details.Add(detail);
                                                dflag = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!dflag)
                            {
                                DemandMasterPlanMaster master = new DemandMasterPlanMaster();
                                master.CreateDate = DateTime.Now;
                                master.CreateMonth = DateTime.Now.Month;
                                master.CreateYear = DateTime.Now.Year;
                                master.CreatePerson = thePlanMaster.HandlePerson;
                                master.CreatePersonName = thePlanMaster.HandlePersonName;
                                master.CurrencyType = thePlanMaster.CurrencyType;
                                master.Descript = thePlanMaster.Descript;
                                master.DocState = DocumentState.InAudit;
                                master.ExchangeRate = thePlanMaster.ExchangeRate;
                                master.ForwardBillCode = thePlanMaster.ForwardBillCode;
                                master.ForwardBillId = thePlanMaster.ForwardBillId;
                                master.ForwardBillType = thePlanMaster.ForwardBillType;
                                master.HandleOrg = thePlanMaster.HandleOrg;
                                master.HandlePerson = thePlanMaster.HandlePerson;
                                master.HandlePersonName = thePlanMaster.HandlePersonName;
                                master.IsFinished = thePlanMaster.IsFinished;
                                master.JBR = thePlanMaster.JBR;
                                master.LastModifyDate = thePlanMaster.LastModifyDate;
                                master.MaterialCategory = dtl.MaterialCategory;
                                master.MaterialCategoryCode = dtl.MaterialCategory.Code;
                                master.MaterialCategoryName = dtl.MaterialCategoryName;
                                master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                                master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                                master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                                master.PlanName = thePlanMaster.ReceiptName;
                                master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                                master.ProjectId = thePlanMaster.ProjectId;
                                master.ProjectName = thePlanMaster.ProjectName;
                                master.RealOperationDate = DateTime.Now;
                                master.SubmitDate = DateTime.Now;
                                if (specialGwbs.SpecialType == "")
                                {
                                    foreach (MaterialCategory m in listMaterial)
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(m.SysCode))
                                        {
                                            master.MaterialCategory = m;
                                            master.MaterialCategory.SysCode = m.SysCode;
                                            master.MaterialCategoryName = m.Name;
                                            break;
                                        }
                                    }
                                    master.ClassifyCode = master.MaterialCategory.Code;
                                    master.Code = GetCode(typeof(DemandMasterPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                    master.Special = "土建";
                                }
                                else
                                {
                                    master.ClassifyCode = specialGwbs.SysCode;
                                    master.Code = GetCode(typeof(DemandMasterPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                    master.Special = "安装";
                                    master.SpecialType = specialGwbs.SpecialType;
                                }
                                detail.Master = master;
                                master.Details.Add(detail);
                                listDemand.Add(master);
                            }
                        }
                        else
                        {
                            DemandMasterPlanMaster master = new DemandMasterPlanMaster();
                            master.CreateDate = DateTime.Now;
                            master.RealOperationDate = DateTime.Now;
                            master.CreateMonth = DateTime.Now.Month;
                            master.CreateYear = DateTime.Now.Year;
                            master.CreatePerson = thePlanMaster.HandlePerson;
                            master.CreatePersonName = thePlanMaster.HandlePersonName;
                            master.CurrencyType = thePlanMaster.CurrencyType;
                            master.Descript = thePlanMaster.Descript;
                            master.DocState = DocumentState.InAudit;
                            master.ExchangeRate = thePlanMaster.ExchangeRate;
                            master.ForwardBillCode = thePlanMaster.Code;
                            master.ForwardBillId = thePlanMaster.Id;
                            master.HandleOrg = thePlanMaster.HandleOrg;
                            master.HandlePerson = thePlanMaster.HandlePerson;
                            master.HandlePersonName = thePlanMaster.HandlePersonName;
                            master.IsFinished = thePlanMaster.IsFinished;
                            master.JBR = thePlanMaster.JBR;
                            master.LastModifyDate = thePlanMaster.LastModifyDate;
                            master.MaterialCategory = dtl.MaterialCategory;
                            master.MaterialCategoryCode = dtl.MaterialCategory.Code;
                            master.MaterialCategoryName = dtl.MaterialCategoryName;
                            master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                            master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                            master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                            master.PlanName = thePlanMaster.ReceiptName;
                            master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                            master.ProjectId = thePlanMaster.ProjectId;
                            master.ProjectName = thePlanMaster.ProjectName;
                            master.SubmitDate = DateTime.Now;
                            if (specialGwbs.SpecialType == "")
                            {
                                foreach (MaterialCategory m in listMaterial)
                                {
                                    if (detail.MaterialCategory.SysCode.Contains(m.SysCode))
                                    {
                                        master.MaterialCategory = m;
                                        master.MaterialCategory.SysCode = m.SysCode;
                                        master.MaterialCategoryName = m.Name;
                                        break;
                                    }
                                }
                                master.ClassifyCode = master.MaterialCategory.Code;
                                master.Code = GetCode(typeof(DemandMasterPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                master.Special = "土建";
                            }
                            else
                            {
                                master.ClassifyCode = specialGwbs.SysCode;
                                master.Code = GetCode(typeof(DemandMasterPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                master.Special = "安装";
                                master.SpecialType = specialGwbs.SpecialType;
                            }
                            detail.Master = master;
                            master.Details.Add(detail);
                            listDemand.Add(master);
                        }
                    }
                }
                if (listDemand != null && listDemand.Count > 0)
                {
                    bool mflag = false;
                    foreach (DemandMasterPlanMaster mat in listDemand)
                    {
                        dao.SaveOrUpdate(mat);
                        mflag = true;
                    }
                    if (mflag)
                    {
                        listMsg[1] = listDemand;
                    }
                }
                else
                {
                    errMsg = "明细中的资源类型不属于物资资源一级分类下的资源类型，生成失败";
                    listMsg[0] = errMsg;
                    return listMsg;
                }
            }
            #endregion

            #region 日常需求计划
            //else if (code.Substring(2, 1) == "1")
            else if(thePlanMaster.StagePlanType==PlanType.日常计划)
            {
                List<DailyPlanMaster> listDaily = new List<DailyPlanMaster>();
                foreach (ResourceRequireReceiptDetail dtl in thePlanMaster.Details)
                {
                    bool flag = false; //判断资源类型是否属于物资资源分类的一级分类
                    GWBSTree specialGwbs = new GWBSTree();//判断工程任务是否有专业分类
                    int slength = 0;
                    foreach (GWBSTree gt in listGWBSTree)
                    {
                        if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                        {
                            if (gt.SysCode.Length > slength)
                            {
                                specialGwbs = gt;
                                slength = gt.SysCode.Length;
                            }

                        }
                    }
                    foreach (MaterialCategory mat in listMaterial)
                    {
                        if (dtl.MaterialCategory.SysCode.Contains(mat.SysCode))
                        {
                            if (mat.ParentNode.Code == "01")
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        #region 添加明细信息
                        DailyPlanDetail detail = new DailyPlanDetail();
                        detail.Descript = dtl.ApproachRequestDesc;
                        detail.IsOver = dtl.IsOver;
                        detail.MaterialCategory = dtl.MaterialCategory;
                        detail.MaterialCategoryName = dtl.MaterialCategoryName;
                        detail.MaterialCode = dtl.MaterialCode;
                        detail.MaterialName = dtl.MaterialName;
                        detail.MaterialResource = dtl.MaterialResource;
                        detail.MaterialSpec = dtl.MaterialSpec;
                        detail.MaterialStuff = dtl.MaterialStuff;
                        detail.UsedPart = dtl.TheGWBSTaskGUID;
                        detail.UsedPartName = dtl.TheGWBSTaskName;
                        foreach (GWBSTree gwbs in CostList)
                        {
                            if (detail.UsedPart.SysCode.Contains(gwbs.SysCode))
                            {
                                detail.UsedPart = gwbs;
                                detail.UsedPartName = gwbs.Name;
                                detail.ProjectTask = gwbs;
                                detail.ProjectTaskName = gwbs.Name;
                                detail.ProjectTaskSysCode = gwbs.SysCode;
                                break;
                            }
                        }
                        detail.Quantity = dtl.PeriodQuantity;
                        detail.LeftQuantity = dtl.PeriodQuantity;
                        detail.Price = dtl.Price;
                        detail.DiagramNumber = dtl.DiagramNumber;
                        detail.MatStandardUnit = dtl.QuantityUnitGUID;
                        detail.MatStandardUnitName = dtl.QuantityUnitName;
                        detail.SpecialType = dtl.TheGWBSTaskGUID.SpecialType;
                        detail.QualityStandard = dtl.QualityStandards;
                        if (dtl.UsedRank != null)
                        {
                            detail.UsedRankName = dtl.UsedRankName;
                            detail.UsedRank = dtl.UsedRank.BearerOrg;
                        }
                        detail.ForwardDetailId = dtl.Id;
                        detail.Descript = dtl.ApproachRequestDesc;
                        detail.ApproachDate = dtl.ApproachDate;
                        #endregion
                        #region 判断是否有相同的一级分类
                        if (listDaily != null && listDaily.Count > 0)
                        {
                            bool dflag = false; //判断是否有相同的一级分类
                            for (int i = 0; i < listDaily.Count; i++)
                            {
                                if (!dflag)
                                {
                                    DailyPlanMaster master = listDaily[i] as DailyPlanMaster;
                                    foreach (DailyPlanDetail ddtl in listDaily[i].Details)
                                    {
                                        if (specialGwbs.SpecialType == "")
                                        {
                                            if (detail.MaterialCategory.SysCode.Contains(master.MaterialCategory.SysCode))
                                            {
                                                if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource == ddtl.MaterialResource && detail.DiagramNumber == ddtl.DiagramNumber)
                                                {
                                                    ddtl.Quantity += detail.Quantity;
                                                    ddtl.LeftQuantity += detail.LeftQuantity;
                                                    dflag = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    detail.Master = master;
                                                    master.Details.Add(detail);
                                                    dflag = true;
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (specialGwbs.SpecialType == master.SpecialType)
                                            {
                                                if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource.Id == ddtl.MaterialResource.Id && detail.DiagramNumber == ddtl.DiagramNumber)
                                                {
                                                    ddtl.Quantity += detail.Quantity;
                                                    ddtl.LeftQuantity += detail.LeftQuantity;
                                                    dflag = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    detail.Master = master;
                                                    detail.SpecialType = master.SpecialType;
                                                    master.Details.Add(detail);
                                                    dflag = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (!dflag)
                            {
                                DailyPlanMaster master = new DailyPlanMaster();
                                master.CreateDate = DateTime.Now;
                                master.RealOperationDate = DateTime.Now;
                                master.CreateMonth = DateTime.Now.Month;
                                master.CreateYear = DateTime.Now.Year;
                                master.CreatePerson = thePlanMaster.HandlePerson;
                                master.CreatePersonName = thePlanMaster.HandlePersonName;
                                master.Descript = thePlanMaster.Descript;
                                master.DocState = DocumentState.InAudit;
                                master.HandlePerson = thePlanMaster.HandlePerson;
                                master.HandlePersonName = thePlanMaster.HandlePersonName;
                                master.IsFinished = thePlanMaster.IsFinished;
                                master.JBR = thePlanMaster.JBR;
                                master.LastModifyDate = thePlanMaster.LastModifyDate;
                                master.MaterialCategory = detail.MaterialCategory;
                                master.MaterialCategoryCode = detail.MaterialCategory.Code;
                                master.MaterialCategoryName = detail.MaterialCategoryName;
                                master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                                master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                                master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                                master.PlanName = thePlanMaster.ReceiptName;
                                master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                                master.SpecialType = detail.SpecialType;
                                master.ProjectId = thePlanMaster.ProjectId;
                                master.ProjectName = thePlanMaster.ProjectName;
                                master.ForwardBillId = thePlanMaster.Id;
                                master.ForwardBillCode = thePlanMaster.Code;
                                master.SubmitDate = DateTime.Now;
                                if (specialGwbs.SpecialType != "")
                                {
                                    //安装
                                    master.ClassifyCode = specialGwbs.SysCode;
                                    master.Code = GetCode(typeof(DailyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                    master.Special = "安装";
                                    master.SpecialType = specialGwbs.SpecialType;
                                }
                                else
                                {
                                    foreach (MaterialCategory mat in listMaterial)
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(mat.SysCode))
                                        {
                                            master.MaterialCategory = mat;
                                            master.MaterialCategoryCode = mat.SysCode;
                                            master.MaterialCategoryName = mat.Name;
                                            break;
                                        }
                                    }
                                    master.ClassifyCode = detail.MaterialCategory.Code;
                                    master.Code = GetCode(typeof(DailyPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                    master.Special = "土建";
                                }
                                detail.Master = master;
                                master.Details.Add(detail);
                                listDaily.Add(master);
                            }
                        }
                        else
                        {
                            DailyPlanMaster master = new DailyPlanMaster();
                            master.CreateDate = DateTime.Now;
                            master.RealOperationDate = DateTime.Now;
                            master.CreateMonth = DateTime.Now.Month;
                            master.CreateYear = DateTime.Now.Year;
                            master.CreatePerson = thePlanMaster.HandlePerson;
                            master.CreatePersonName = thePlanMaster.HandlePersonName;
                            master.Descript = thePlanMaster.Descript;
                            master.DocState = DocumentState.InAudit;
                            master.HandlePerson = thePlanMaster.HandlePerson;
                            master.HandlePersonName = thePlanMaster.HandlePersonName;
                            master.IsFinished = thePlanMaster.IsFinished;
                            master.MaterialCategory = detail.MaterialCategory;
                            master.MaterialCategoryCode = detail.MaterialCategory.Code;
                            master.MaterialCategoryName = detail.MaterialCategoryName;
                            master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                            master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                            master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                            master.PlanName = thePlanMaster.ReceiptName;
                            master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                            master.ProjectId = thePlanMaster.ProjectId;
                            master.ProjectName = thePlanMaster.ProjectName;
                            master.ForwardBillId = thePlanMaster.Id;
                            master.ForwardBillCode = thePlanMaster.Code;
                            master.SubmitDate = DateTime.Now;
                            if (specialGwbs.SpecialType != "")
                            {
                                master.ClassifyCode = specialGwbs.SysCode;
                                master.Code = GetCode(typeof(DailyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                master.Special = "安装";
                                master.SpecialType = specialGwbs.SpecialType;
                            }
                            else
                            {
                                foreach (MaterialCategory mat in listMaterial)
                                {
                                    if (detail.MaterialCategory.SysCode.Contains(mat.SysCode))
                                    {
                                        master.MaterialCategory = mat;
                                        master.MaterialCategoryCode = mat.SysCode;
                                        master.MaterialCategoryName = mat.Name;
                                        break;
                                    }
                                }
                                master.ClassifyCode = master.MaterialCategory.Code;
                                master.Code = GetCode(typeof(DailyPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                master.Special = "土建";

                            }
                            detail.Master = master;
                            master.Details.Add(detail);
                            listDaily.Add(master);
                        }
                        #endregion
                    }
                }

                if (listDaily != null && listDaily.Count > 0)
                {
                    bool mflag = false;
                    foreach (DailyPlanMaster mat in listDaily)
                    {
                        dao.SaveOrUpdate(mat);
                        mflag = true;
                    }
                    if (mflag)
                    {
                        listMsg[1] = listDaily;
                    }

                }
                else
                {
                    errMsg = "明细中的资源类型不属于物资资源一级分类下的资源类型，生成失败";
                    listMsg[0] = errMsg;
                    return listMsg;
                }
            }
            #endregion

            #region 月度需求计划
            //else if (code.Substring(2, 1) == "2")
            else if(thePlanMaster.StagePlanType==PlanType.月度计划)
            {
                bool mflag = false;
                List<MonthlyPlanMaster> listMonth = new List<MonthlyPlanMaster>();
                foreach (ResourceRequireReceiptDetail dtl in thePlanMaster.Details)
                {
                    GWBSTree specialGwbs = new GWBSTree();//判断工程任务是否有专业分类
                    int slength = 0;
                    foreach (GWBSTree gt in listGWBSTree)
                    {
                        if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                        {
                            if (gt.SysCode.Length > slength)
                            {
                                specialGwbs = gt;
                                slength = gt.SysCode.Length;
                            }

                        }
                    }
                    foreach (MaterialCategory mat in listMaterial)
                    {
                        if (dtl.MaterialCategory.SysCode.Contains(mat.SysCode))
                        {
                            if (mat.ParentNode.Code == "01")
                            {
                                mflag = true;
                            }
                        }
                    }
                    if (mflag)
                    {
                        MonthlyPlanDetail detail = new MonthlyPlanDetail();
                        //detail.ProjectTask = dtl.TheGWBSTaskGUID;
                        //detail.ProjectTaskName = dtl.TheGWBSTaskName;
                        //detail.ProjectTaskSysCode = dtl.TheGWBSSysCode;
                        detail.MaterialCategory = dtl.MaterialCategory;
                        detail.MaterialCategoryName = dtl.MaterialCategoryName;
                        detail.MaterialCode = dtl.MaterialCode;
                        detail.MaterialName = dtl.MaterialName;
                        detail.MaterialResource = dtl.MaterialResource;
                        detail.MaterialSpec = dtl.MaterialSpec;
                        detail.MaterialStuff = dtl.MaterialStuff;
                        detail.MatStandardUnit = dtl.QuantityUnitGUID;
                        detail.MatStandardUnitName = dtl.QuantityUnitName;
                        detail.UsedPart = dtl.TheGWBSTaskGUID;
                        detail.UsedPartName = dtl.TheGWBSTaskName;
                        #region
                        foreach (GWBSTree gt in CostList)
                        {
                            if (dtl.TheGWBSTaskGUID.SysCode.Contains(gt.SysCode))
                            {
                                detail.UsedPart = gt;
                                detail.UsedPartName = gt.Name;
                                detail.ProjectTask = gt;
                                detail.ProjectTaskName = gt.Name;
                                detail.ProjectTaskSysCode = gt.SysCode;
                                break;
                            }
                        }
                        #endregion
                        detail.Quantity = dtl.PeriodQuantity;
                        detail.NeedQuantity = dtl.PeriodQuantity;
                        detail.LeftQuantity = dtl.PeriodQuantity;
                        detail.DiagramNumber = dtl.DiagramNumber;
                        detail.ForwardDetailId = dtl.TheResReceipt.Id;
                        detail.SpecialType = dtl.TheGWBSTaskGUID.SpecialType;
                        detail.QualityStandard = dtl.QualityStandards;
                        detail.ForwardDetailId = dtl.Id;
                        if (dtl.UsedRank != null)
                        {
                            detail.UsedRankName = dtl.UsedRankName;
                            detail.UsedRank = dtl.UsedRank.BearerOrg;
                        }
                        detail.Descript = dtl.ApproachRequestDesc;
                        if (listMonth != null && listMonth.Count > 0)
                        {
                            bool flag = false;
                            for (int i = 0; i < listMonth.Count; i++)
                            {
                                MonthlyPlanMaster master = listMonth[i] as MonthlyPlanMaster;
                                List<MonthlyPlanDetail> ddtlList = listMonth[i].Details.OfType<MonthlyPlanDetail>().ToList<MonthlyPlanDetail>();
                                foreach (MonthlyPlanDetail ddtl in ddtlList)
                                {
                                    if (specialGwbs.SpecialType == "")
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(master.MaterialCategory.SysCode))
                                        {
                                            if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource.Id == ddtl.MaterialResource.Id && detail.DiagramNumber == ddtl.DiagramNumber)
                                            {
                                                detail.Quantity += ddtl.Quantity;
                                                detail.LeftQuantity += ddtl.LeftQuantity;
                                                detail.NeedQuantity += ddtl.NeedQuantity;
                                                flag = true;
                                                break;
                                            }
                                            else
                                            {
                                                detail.Master = master;
                                                master.Details.Add(detail);
                                                flag = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (specialGwbs.SpecialType == master.SpecialType)
                                        {
                                            if (detail.UsedPart.Id == ddtl.UsedPart.Id && detail.MaterialResource.Id == ddtl.MaterialResource.Id && detail.DiagramNumber == ddtl.DiagramNumber)
                                            {
                                                detail.Quantity += ddtl.Quantity;
                                                detail.LeftQuantity += ddtl.LeftQuantity;
                                                detail.NeedQuantity += ddtl.NeedQuantity;
                                                flag = true;
                                                break;
                                            }
                                            else
                                            {
                                                detail.Master = master;
                                                detail.SpecialType = master.SpecialType;
                                                master.Details.Add(detail);
                                                flag = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!flag)
                            {
                                MonthlyPlanMaster master = new MonthlyPlanMaster();
                                master.MonthePlanType = thePlanMaster.ResourceRequirePlanTypeWord;
                                master.CreateDate = DateTime.Now;
                                master.RealOperationDate = DateTime.Now;
                                master.CreateMonth = DateTime.Now.Month;
                                master.CreateYear = DateTime.Now.Year;
                                master.CreatePerson = thePlanMaster.HandlePerson;
                                master.CreatePersonName = thePlanMaster.HandlePersonName;
                                master.Descript = thePlanMaster.Descript;
                                master.DocState = DocumentState.InAudit;
                                master.ExchangeRate = thePlanMaster.ExchangeRate;
                                master.ForwardBillCode = thePlanMaster.Code;
                                master.ForwardBillId = thePlanMaster.Id;
                                master.HandlePerson = thePlanMaster.HandlePerson;
                                master.HandlePersonName = thePlanMaster.HandlePersonName;
                                master.MaterialCategory = dtl.MaterialCategory;
                                master.MaterialCategoryCode = dtl.MaterialCategory.Code;
                                master.MaterialCategoryName = dtl.MaterialCategoryName;
                                master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                                master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                                master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                                master.PlanName = thePlanMaster.ReceiptName;
                                master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                                master.ProjectId = thePlanMaster.ProjectId;
                                master.ProjectName = thePlanMaster.ProjectName;
                                master.SubmitDate = DateTime.Now;
                                if (specialGwbs.SpecialType != "")
                                {
                                    master.ClassifyCode = specialGwbs.SysCode;
                                    master.Code = GetCode(typeof(MonthlyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                    master.Special = "安装";
                                    master.SpecialType = specialGwbs.SpecialType;
                                }
                                else
                                {
                                    foreach (MaterialCategory mat in listMaterial)
                                    {
                                        if (detail.MaterialCategory.SysCode.Contains(mat.SysCode))
                                        {
                                            master.MaterialCategory = mat;
                                            master.MaterialCategoryCode = mat.SysCode;
                                            master.MaterialCategoryName = mat.Name;
                                            break;
                                        }
                                    }
                                    master.ClassifyCode = master.MaterialCategory.Code;
                                    master.Code = GetCode(typeof(MonthlyPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                    master.Special = "土建";
                                }
                                detail.Master = master;
                                master.Details.Add(detail);
                                listMonth.Add(master);
                            }
                        }
                        else
                        {
                            MonthlyPlanMaster master = new MonthlyPlanMaster();
                            master.MonthePlanType = thePlanMaster.ResourceRequirePlanTypeWord;
                            master.CreateDate = DateTime.Now;
                            master.RealOperationDate = DateTime.Now;
                            master.CreateMonth = DateTime.Now.Month;
                            master.CreateYear = DateTime.Now.Year;
                            master.CreatePerson = thePlanMaster.HandlePerson;
                            master.CreatePersonName = thePlanMaster.HandlePersonName;
                            master.Descript = thePlanMaster.Descript;
                            master.DocState = DocumentState.InAudit;
                            master.HandlePerson = thePlanMaster.HandlePerson;
                            master.HandlePersonName = thePlanMaster.HandlePersonName;
                            master.MaterialCategory = dtl.MaterialCategory;
                            master.MaterialCategoryCode = dtl.MaterialCategory.Code;
                            master.MaterialCategoryName = dtl.MaterialCategoryName;
                            master.OperOrgInfo = thePlanMaster.OperOrgInfo;
                            master.OperOrgInfoName = thePlanMaster.OperOrgInfoName;
                            master.OpgSysCode = thePlanMaster.OperOrgInfo.SysCode;
                            master.PlanName = thePlanMaster.ReceiptName;
                            master.PlanType = ExecuteDemandPlanTypeEnum.物资计划;
                            master.ProjectId = thePlanMaster.ProjectId;
                            master.ProjectName = thePlanMaster.ProjectName;
                            master.SubmitDate = DateTime.Now;
                            if (specialGwbs.SpecialType != "")
                            {
                                master.ClassifyCode = specialGwbs.SysCode;
                                master.Code = GetCode(typeof(MonthlyPlanMaster), thePlanMaster.ProjectId, specialGwbs.SpecialType);
                                master.Special = "安装";
                                master.SpecialType = specialGwbs.SpecialType;
                            }
                            else
                            {
                                foreach (MaterialCategory mat in listMaterial)
                                {
                                    if (detail.MaterialCategory.SysCode.Contains(mat.SysCode))
                                    {
                                        master.MaterialCategory = mat;
                                        master.MaterialCategoryCode = mat.SysCode;
                                        master.MaterialCategoryName = mat.Name;
                                        break;
                                    }
                                }
                                master.ClassifyCode = master.MaterialCategory.Code;
                                master.Code = GetCode(typeof(MonthlyPlanMaster), thePlanMaster.ProjectId, master.MaterialCategory.Abbreviation);
                                master.Special = "土建";
                            }
                            detail.Master = master;
                            master.Details.Add(detail);
                            listMonth.Add(master);
                        }
                    }
                }
                if (listMonth != null && listMonth.Count > 0)
                {
                    bool monflag = false;
                    foreach (MonthlyPlanMaster mat in listMonth)
                    {
                        dao.SaveOrUpdate(mat);
                        monflag = true;
                    }
                    if (monflag)
                    {
                        listMsg[1] = listMonth;
                    }
                }
                else
                {
                    errMsg = "明细中的资源类型不属于物资资源一级分类下的资源类型，生成失败";
                    listMsg[0] = errMsg;
                    return listMsg;
                }
            }
            #endregion

            return listMsg;
        }
        #endregion


        public DataSet GWBSAccountingNodes(string condition)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            string sql = @"select t.Id, t.Name, t.SysCode from THD_GWBSTree t where t.CostAccFlag = 1 ";
            sql += "and t.TheProjectGUID ='" + condition + "'" + " order by t.SysCode desc";
            command.CommandText = sql;
            IDataReader dataReader = command.ExecuteReader();
            DataSet dataSet = DataAccessUtil.ConvertDataReadertoDataSet(dataReader);
            return dataSet;
        }

        // <summary>
        /// 修改滚动资源需求计划明细状态
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public ResourceRequirePlan modifyPlanDetailState(ResourceRequirePlan plan)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", plan.Id));
            IList list = dao.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);

            foreach (ResourceRequirePlanDetail dtl in list)
            {
                dtl.State = ResourceRequirePlanDetailState.作废;
            }
            dao.SaveOrUpdate(plan);
            return plan;
        }
    }
}
