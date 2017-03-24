using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using System.Data;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using VirtualMachine.Core.DataAccess;
using CommonSearchLib.BillCodeMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Util;
using System.Text.RegularExpressions;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectCopyMng.Service
{
    /// <summary>
    /// 工程项目复制
    /// </summary>
    public class ProjectCopySrv : BaseService, IProjectCopySrv
    {
        #region 注入服务
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

        #region 工程项目复制
        [TransManager]
        public void DeleteCopy(CurrentProjectInfo ProjectInfoLeft, CurrentProjectInfo ProjectInfoRight, IList listPBS, IList listWBS)
        {
            //删除原有的信息
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);

            string strsql = "delete  thd_pbstree where TheProjectGUID = '" + ProjectInfoRight.Id + "'";
            command.CommandText = strsql;
            int res = command.ExecuteNonQuery();

            string strsql3 = "delete  THD_GWBSDetailCostSubject where TheProjectGUID = '" + ProjectInfoRight.Id + "'";
            command.CommandText = strsql3;
            int res3 = command.ExecuteNonQuery();

            string strsql2 = "delete  THD_GWBSDetail where TheProjectGUID = '" + ProjectInfoRight.Id + "'";
            command.CommandText = strsql2;
            int res2 = command.ExecuteNonQuery();

            string strsql1 = "delete  THD_GWBSTree where TheProjectGUID = '" + ProjectInfoRight.Id + "'";
            command.CommandText = strsql1;
            int res1 = command.ExecuteNonQuery();


            SaveCopy(ProjectInfoLeft, ProjectInfoRight, listPBS, listWBS);
        }

        [TransManager]
        public void SaveCopy(CurrentProjectInfo ProjectInfoLeft, CurrentProjectInfo ProjectInfoRight, IList listPBS, IList listWBS)
        {
            //复制PBS信息
            Hashtable ht_oldToNew = new Hashtable();
            PBSTree PBSTree = new PBSTree();
            IList list_update = new ArrayList();
            foreach (PBSTree oldTree in listPBS)
            {
                oldTree.TempGUID = oldTree.Id;
                PBSTree newTree = this.PbsTreeCopy(oldTree, ProjectInfoRight);
                newTree.TheProjectGUID = ProjectInfoRight.Id;
                newTree.TheProjectName = ProjectInfoRight.Name;
                PBSTree = SavePBSTree(newTree, ProjectInfoRight);

                ht_oldToNew.Add(oldTree.TempGUID, PBSTree);
                string syscode = TransUtil.ToString(PBSTree.SysCode);
                CategoryNode newParent = PBSTree.ParentNode;
                if (newParent != null)
                {
                    foreach (System.Collections.DictionaryEntry objSys in ht_oldToNew)
                    {
                        if (objSys.Key.ToString().Equals(TransUtil.ToString(newParent.Id)))
                        {
                            newParent = (CategoryNode)objSys.Value;
                            break;
                        }
                    }
                }
                string newSyscode = "";
                string[] str = syscode.Split('.');
                for (int t = 0; t < str.Length; t++)
                {
                    string temp = TransUtil.ToString(str[t]);
                    if (temp.Equals(""))
                    { }
                    else
                    {
                        string newStr = TransUtil.ToString(((PBSTree)ht_oldToNew[temp]).Id);
                        newSyscode += newStr + ".";
                    }
                }
                PBSTree.SysCode = newSyscode;
                PBSTree.ParentNode = newParent;
                list_update.Add(PBSTree);
            }
            this.UpdatePbsTree(list_update);
            //复制GWBS信息
            Hashtable ht_WBSoldetonew = new Hashtable();
            GWBSTree gwbsTree = new GWBSTree();
            IList list_gwbsupdate = new ArrayList();
            foreach (GWBSTree oldTree in listWBS)
            {
                oldTree.TempGUID = oldTree.Id;
                GWBSTree newTree = this.GwbsTreeCopy(oldTree, ProjectInfoRight);

                newTree.TheProjectGUID = ProjectInfoRight.Id;
                newTree.TheProjectName = ProjectInfoRight.Name;
                gwbsTree = SaveGWBSTree(newTree, ProjectInfoRight);

                ht_WBSoldetonew.Add(oldTree.TempGUID, gwbsTree);
                string syscode = TransUtil.ToString(gwbsTree.SysCode);
                CategoryNode newParent = gwbsTree.ParentNode;
                if (newParent != null)
                {
                    foreach (System.Collections.DictionaryEntry objSys in ht_WBSoldetonew)
                    {
                        if (objSys.Key.ToString().Equals(TransUtil.ToString(newParent.Id)))
                        {
                            newParent = (CategoryNode)objSys.Value;
                        }
                    }
                }
                string newSyscode = "";
                string[] str = syscode.Split('.');
                for (int t = 0; t < str.Length; t++)
                {
                    string temp = TransUtil.ToString(str[t]);
                    if (temp.Equals(""))
                    {
                    }
                    else
                    {
                        string newStr = TransUtil.ToString(((GWBSTree)ht_WBSoldetonew[temp]).Id);
                        newSyscode += newStr + ".";
                    }
                }
                gwbsTree.SysCode = newSyscode;
                gwbsTree.ParentNode = newParent;
                list_gwbsupdate.Add(gwbsTree);
            }
            this.UpdateGwbsTree(list_gwbsupdate);

        }

        /// <summary>
        /// 更新PBS的syscode和parentnode
        /// </summary>
        /// <param name="list"></param>
        [TransManager]
        private void UpdatePbsTree(IList list)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            foreach (PBSTree pbs in list)
            {
                string parentNodeId = "";
                if (pbs.ParentNode != null)
                {
                    parentNodeId = pbs.ParentNode.Id;
                }
                string strsql = "update thd_pbstree set SysCode = '" + pbs.SysCode + "',ParentNodeID = '" + parentNodeId + "' where Id = '" + pbs.Id + "'";
                command.CommandText = strsql;
                int res = command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 更新GWBS的syscode和parentnode
        /// </summary>
        /// <param name="list"></param>
        [TransManager]
        private void UpdateGwbsTree(IList list)
        {
            ISession session = CallContext.GetData("nhsession") as ISession;
            IDbConnection conn = session.Connection;
            IDbCommand command = conn.CreateCommand();
            session.Transaction.Enlist(command);
            foreach (GWBSTree pbs in list)
            {
                string parentNodeId = "";
                if (pbs.ParentNode != null)
                {
                    parentNodeId = pbs.ParentNode.Id;
                }
                string strsql = "update thd_gwbstree set SysCode = '" + pbs.SysCode + "',ParentNodeID = '" + parentNodeId + "' where Id = '" + pbs.Id + "'";
                command.CommandText = strsql;
                int res = command.ExecuteNonQuery();
            }
        }
        private PBSTree PbsTreeCopy(PBSTree oldTree, CurrentProjectInfo ProjectInfoRight)
        {
            PBSTree newTree = new PBSTree();
            newTree.CategoryNodeType = oldTree.CategoryNodeType;
            string strcode = Convert.ToString(oldTree.Code);

            string strnewCode = ProjectInfoRight.Code.PadLeft(4, '0');
            string str = "-";
            Regex r = new Regex(str);
            Match m = r.Match(strcode);
            if (m.Success)
            {//含有“-”
                string[] sArray = strcode.Split('-');
                for (int t = 1; t < sArray.Length; t++)
                {
                    string temp = Convert.ToString(sArray[t]);
                    if (temp.Equals(""))
                    { }
                    else
                    {
                        strnewCode += "-" + temp;
                    }
                }
            }
            else
            {
                strnewCode += "-" + strcode;
            }
            newTree.Code = strnewCode;
            newTree.Describe = oldTree.Describe;
            newTree.DocumentModelGUID = oldTree.DocumentModelGUID;
            newTree.FullPath = oldTree.FullPath;
            newTree.Level = oldTree.Level;
            newTree.Name = oldTree.Name;
            newTree.NodeDesc = oldTree.NodeDesc;
            newTree.OrderNo = oldTree.OrderNo;
            newTree.OwnerGUID = oldTree.OwnerGUID;
            newTree.OwnerName = oldTree.OwnerName;
            newTree.OwnerOrgSysCode = oldTree.OwnerOrgSysCode;
            newTree.ParentNode = oldTree.ParentNode;
            newTree.State = oldTree.State;
            newTree.StructTypeGUID = oldTree.StructTypeGUID;
            newTree.StructTypeName = oldTree.StructTypeName;
            newTree.SysCode = oldTree.SysCode;
            newTree.CreateDate = DateTime.Now;
            return newTree;
        }

        private GWBSTree GwbsTreeCopy(GWBSTree oldTree, CurrentProjectInfo ProjectInfoRight)
        {
            GWBSTree newTree = new GWBSTree();
            //newTree.AddUpFigureProgress = oldTree.AddUpFigureProgress;
            //newTree.Author = oldTree.Author;
            newTree.CategoryNodeType = oldTree.CategoryNodeType;
            //newTree.CheckBatchNumber = oldTree.CheckBatchNumber;
            //newTree.CheckRequire = oldTree.CheckRequire;
            //newTree.ChildNodes = oldTree.ChildNodes;

            string strcode = Convert.ToString(oldTree.Code);
            string strnewCode = ProjectInfoRight.Code.PadLeft(4, '0');
            string str = "-";
            Regex r = new Regex(str);
            Match m = r.Match(strcode);
            if (m.Success)
            {//含有“-”
                string[] sArray = strcode.Split('-');
                for (int t = 1; t < sArray.Length; t++)
                {
                    string temp = TransUtil.ToString(sArray[t]);
                    if (temp.Equals(""))
                    { }
                    else
                    {
                        strnewCode += "-" + temp;
                    }
                }
            }
            else
            {
                strnewCode += "-" + strcode;
            }
            newTree.Code = strnewCode;
            //newTree.ContractTotalPrice = oldTree.ContractTotalPrice;
            //newTree.CostAccFlag = oldTree.CostAccFlag;
            //newTree.CreateDate = DateTime.Now;
            newTree.Describe = oldTree.Describe;
            //newTree.Details = oldTree.Details;
            //newTree.IsAccountNode = oldTree.IsAccountNode;
            newTree.Level = oldTree.Level;
            //newTree.ListRelaPBS = oldTree.ListRelaPBS;
            newTree.Name = oldTree.Name;
            //newTree.NGUID = oldTree.NGUID;
            newTree.NodeType = oldTree.NodeType;
            newTree.OrderNo = oldTree.OrderNo;
            //newTree.OwnerGUID = oldTree.OwnerGUID;
            //newTree.OwnerName = oldTree.OwnerName;
            //newTree.OwnerOrgSysCode = oldTree.OwnerOrgSysCode;
            newTree.ParentNode = oldTree.ParentNode;
            //newTree.PlanTotalPrice = oldTree.PlanTotalPrice;
            //newTree.PriceAmountUnitGUID = oldTree.PriceAmountUnitGUID;
            //newTree.PriceAmountUnitName = oldTree.PriceAmountUnitName;
            //newTree.ProductConfirmFlag = oldTree.ProductConfirmFlag;
            //newTree.ProjectTaskTypeGUID = oldTree.ProjectTaskTypeGUID;
            //newTree.ProjectTaskTypeName = oldTree.ProjectTaskTypeName;
            //newTree.ResponsibilityTotalPrice = oldTree.ResponsibilityTotalPrice;
            //newTree.ResponsibleAccFlag = oldTree.ResponsibleAccFlag;
            //newTree.State = oldTree.State;
            //newTree.SubContractFeeFlag = oldTree.SubContractFeeFlag;
            //newTree.Summary = oldTree.Summary;
            newTree.SysCode = oldTree.SysCode;
            //newTree.TaskPlanEndTime = oldTree.TaskPlanEndTime;
            //newTree.TaskPlanStartTime = oldTree.TaskPlanStartTime;
            //newTree.TaskState = oldTree.TaskState;
            //newTree.TaskStateTime = oldTree.TaskStateTime;
            newTree.TheTree = oldTree.TheTree;
            newTree.Version = oldTree.Version;
            //查询GWBS下的明细
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddCriterion(Expression.Eq("Id", oldTree.Id));
            IList GWBSlist = Dao.ObjectQuery(typeof(GWBSTree), oq);
            GWBSTree GWBSSearch = GWBSlist[0] as GWBSTree;

            foreach (GWBSDetail detail in GWBSSearch.Details)
            {
                GWBSDetail newDetail = new GWBSDetail();
                newDetail.AddupAccFigureProgress = detail.AddupAccFigureProgress;
                newDetail.AddupAccQuantity = detail.AddupAccQuantity;
                newDetail.BearOrgGUID = detail.BearOrgGUID;
                newDetail.BearOrgName = detail.BearOrgName;

                string strcode1 = Convert.ToString(detail.Code);
                string strnewCode1 = ProjectInfoRight.Code.PadLeft(4, '0');
                string str1 = "-";
                Regex r1 = new Regex(str1);
                Match m1 = r1.Match(strcode1);
                if (m1.Success)
                {//含有“-”
                    string[] sArray1 = strcode1.Split('-');
                    for (int t1 = 1; t1 < sArray1.Length; t1++)
                    {
                        string temp1 = TransUtil.ToString(sArray1[t1]);
                        if (temp1.Equals(""))
                        { }
                        else
                        {
                            strnewCode1 += "-" + temp1;
                        }
                    }
                }
                else
                {
                    strnewCode1 += "-" + strcode1;
                }
                newDetail.Code = strnewCode1;

                newDetail.ContentDesc = detail.ContentDesc;
                newDetail.ContractGroupCode = detail.ContractGroupCode;
                newDetail.ContractGroupGUID = detail.ContractGroupGUID;
                newDetail.ContractGroupName = detail.ContractGroupName;
                newDetail.ContractGroupType = detail.ContractGroupType;
                newDetail.ContractPrice = detail.ContractPrice;
                newDetail.ContractProjectQuantity = detail.ContractProjectQuantity;
                newDetail.ContractTotalPrice = detail.ContractTotalPrice;
                newDetail.CostingFlag = detail.CostingFlag;
                newDetail.CreateTime = DateTime.Now;
                newDetail.CurrentStateTime = detail.CurrentStateTime;
                newDetail.DetailExecuteDesc = detail.DetailExecuteDesc;
                newDetail.FinishedWorkAmount = detail.FinishedWorkAmount;
                //newDetail.ListCostSubjectDetails = detail.ListCostSubjectDetails;

                //查询GWBS明细下的科目
                ObjectQuery oqSubject = new ObjectQuery();
                oqSubject.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
                oqSubject.AddCriterion(Expression.Eq("Id", detail.Id));
                IList GSubjectList = Dao.ObjectQuery(typeof(GWBSDetail), oqSubject);
                GWBSDetail GWBSDtl = GSubjectList[0] as GWBSDetail;

                foreach (GWBSDetailCostSubject subject in GWBSDtl.ListCostSubjectDetails)
                {
                    GWBSDetailCostSubject gSubject = new GWBSDetailCostSubject();
                    gSubject.AddupAccountCost = subject.AddupAccountCost;
                    gSubject.AddupAccountCostEndTime = subject.AddupAccountCostEndTime;
                    gSubject.AddupAccountProjectAmount = subject.AddupAccountProjectAmount;
                    gSubject.AddupBalanceProjectAmount = subject.AddupBalanceProjectAmount;
                    gSubject.AddupBalanceTotalPrice = subject.AddupBalanceTotalPrice;
                    gSubject.AssessmentRate = subject.AssessmentRate;
                    gSubject.ContractPrice = subject.ContractPrice;
                    gSubject.ContractProjectAmount = subject.ContractProjectAmount;

                    gSubject.ContractBasePrice = subject.ContractBasePrice;
                    gSubject.ContractPricePercent = subject.ContractPricePercent;
                    gSubject.ContractQuantityPrice = subject.ContractQuantityPrice;

                    gSubject.ContractQuotaQuantity = subject.ContractQuotaQuantity;
                    gSubject.ContractTotalPrice = subject.ContractTotalPrice;
                    gSubject.CostAccountSubjectGUID = subject.CostAccountSubjectGUID;
                    gSubject.CostAccountSubjectName = subject.CostAccountSubjectName;
                    gSubject.CostAccountSubjectSyscode = subject.CostAccountSubjectSyscode;
                    gSubject.CreateTime = DateTime.Now;
                    gSubject.CurrentPeriodAccountCost = subject.CurrentPeriodAccountCost;
                    gSubject.CurrentPeriodAccountCostEndTime = subject.CurrentPeriodAccountCostEndTime;
                    gSubject.CurrentPeriodAccountProjectAmount = subject.CurrentPeriodAccountProjectAmount;
                    gSubject.CurrentPeriodBalanceProjectAmount = subject.CurrentPeriodBalanceProjectAmount;
                    gSubject.CurrentPeriodBalanceTotalPrice = subject.CurrentPeriodBalanceTotalPrice;
                    gSubject.IsCategoryResource = subject.IsCategoryResource;
                    gSubject.MainResTypeFlag = subject.MainResTypeFlag;
                    gSubject.Name = subject.Name;
                    gSubject.TheProjectGUID = ProjectInfoRight.Id;
                    gSubject.TheProjectName = ProjectInfoRight.Name;

                    gSubject.PlanBasePrice = subject.PlanBasePrice;
                    gSubject.PlanPricePercent = subject.PlanPricePercent;
                    gSubject.PlanPrice = subject.PlanPrice;
                    gSubject.PlanQuotaNum = subject.PlanQuotaNum;
                    gSubject.PlanTotalPrice = subject.PlanTotalPrice;
                    gSubject.PlanWorkAmount = subject.PlanWorkAmount;
                    gSubject.PlanWorkPrice = subject.PlanWorkPrice;

                    gSubject.PriceUnitGUID = subject.PriceUnitGUID;
                    gSubject.PriceUnitName = subject.PriceUnitName;
                    gSubject.ProjectAmountUnitGUID = subject.ProjectAmountUnitGUID;
                    gSubject.ProjectAmountUnitName = subject.ProjectAmountUnitName;
                    gSubject.ProjectAmountWasta = subject.ProjectAmountWasta;
                    gSubject.ResourceTypeGUID = subject.ResourceTypeGUID;
                    gSubject.ResourceTypeCode = subject.ResourceTypeCode;
                    gSubject.ResourceTypeName = subject.ResourceTypeName;
                    gSubject.ResourceTypeQuality = subject.ResourceTypeQuality;
                    gSubject.ResourceTypeSpec = subject.ResourceTypeSpec;
                    gSubject.ResourceCateSyscode = subject.ResourceCateSyscode;
                    gSubject.ResourceUsageQuota = subject.ResourceUsageQuota;

                    gSubject.ResponsibleBasePrice = subject.ResponsibleBasePrice;
                    gSubject.ResponsiblePricePercent = subject.ResponsiblePricePercent;
                    gSubject.ResponsibilitilyPrice = subject.ResponsibilitilyPrice;

                    gSubject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyTotalPrice;
                    gSubject.ResponsibilitilyWorkAmount = subject.ResponsibilitilyWorkAmount;
                    gSubject.ResponsibleQuotaNum = subject.ResponsibleQuotaNum;
                    gSubject.ResponsibleWorkPrice = subject.ResponsibleWorkPrice;
                    gSubject.State = subject.State;
                    gSubject.TheGWBSDetail = newDetail;

                    gSubject.TheGWBSTree = newTree;
                    gSubject.TheGWBSTreeName = newTree.Name;
                    gSubject.TheGWBSTreeSyscode = newTree.SysCode;

                    //gSubject.TheGWBSDetail = subject.TheGWBSDetail;
                    gSubject.Version = subject.Version;
                    newDetail.ListCostSubjectDetails.Add(gSubject);//将科目成本添加到工程明细信息中
                }
                newDetail.MainResourceTypeId = detail.MainResourceTypeId;
                newDetail.MainResourceTypeName = detail.MainResourceTypeName;
                newDetail.MainResourceTypeQuality = detail.MainResourceTypeQuality;
                newDetail.MainResourceTypeSpec = detail.MainResourceTypeSpec;
                newDetail.DiagramNumber = detail.DiagramNumber;
                newDetail.Name = detail.Name;
                newDetail.NGUID = detail.NGUID;
                newDetail.TheProjectGUID = ProjectInfoRight.Id;
                newDetail.TheProjectName = ProjectInfoRight.Name;
                newDetail.PlanPrice = detail.PlanPrice;
                newDetail.PlanTotalPrice = detail.PlanTotalPrice;
                newDetail.PlanWorkAmount = detail.PlanWorkAmount;
                newDetail.PriceUnitGUID = detail.PriceUnitGUID;
                newDetail.PriceUnitName = detail.PriceUnitName;
                newDetail.ProduceConfirmFlag = detail.ProduceConfirmFlag;
                newDetail.ProgressConfirmed = detail.ProgressConfirmed;
                newDetail.ProjectTaskTypeCode = detail.ProjectTaskTypeCode;
                newDetail.QuantityConfirmed = detail.QuantityConfirmed;
                newDetail.ResponseFlag = detail.ResponseFlag;
                newDetail.ResponsibilitilyPrice = detail.ResponsibilitilyPrice;
                newDetail.ResponsibilitilyTotalPrice = detail.ResponsibilitilyTotalPrice;
                newDetail.ResponsibilitilyWorkAmount = detail.ResponsibilitilyWorkAmount;
                newDetail.State = detail.State;
                newDetail.SubContractFeeFlag = detail.SubContractFeeFlag;
                newDetail.SubContractStepRate = detail.SubContractStepRate;
                newDetail.Summary = detail.Summary;
                newDetail.TaskFinishedPercent = detail.TaskFinishedPercent;
                newDetail.TheCostItem = detail.TheCostItem;
                newDetail.TheCostItemCateSyscode = detail.TheCostItemCateSyscode;
                newDetail.TheGWBS = newTree;
                newDetail.TheGWBSSysCode = newTree.SysCode;
                //newDetail.TheGWBS = detail.TheGWBS;
                //newDetail.TheGWBSSysCode = detail.TheGWBSSysCode;
                newDetail.Version = detail.Version;
                newDetail.WeekScheduleDetail = detail.WeekScheduleDetail;
                newDetail.WorkAmountUnitGUID = detail.WorkAmountUnitGUID;
                newDetail.WorkAmountUnitName = detail.WorkAmountUnitName;
                newDetail.WorkMethod = detail.WorkMethod;
                newDetail.WorkPart = detail.WorkPart;
                newDetail.WorkUseMaterial = detail.WorkUseMaterial;
                newTree.Details.Add(newDetail);//将明细信息添加到主表信息中
            }

            return newTree;
        }

        [TransManager]
        public PBSTree SavePBSTree(PBSTree obj, CurrentProjectInfo ProjectInfoRight)
        {
            if (obj.Id == null)
            {
                //if (ProjectInfoRight != null)
                //{
                //    obj.Code = ProjectInfoRight.Code.PadLeft(4, '0') + "-" + obj.BasicCode + "-" + model.GetCode(typeof(PBSTree));
                //}
            }
            obj.CreateDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as PBSTree;
        }

        [TransManager]
        public GWBSTree SaveGWBSTree(GWBSTree obj, CurrentProjectInfo ProjectInfoRight)
        {
            if (obj.Id == null)
            {
                //if (ProjectInfoRight != null)
                //{
                //    obj.Code = ProjectInfoRight.Code.PadLeft(4, '0') + "-" + obj.BasicCode + "-" + model.GetCode(typeof(PBSTree));
                //}
            }
            obj.CreateDate = DateTime.Now;
            return SaveOrUpdateByDao(obj) as GWBSTree;
        }

        #endregion
    }
}
