using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using System.Collections;
using VirtualMachine.Core.Expression;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VCopyProgress : Form
    {
        public MGWBSTree model = new MGWBSTree();

        private List<GWBSTree> listSelectWBSNode = null;

        private List<GWBSDetail> listCopyNodeDetail = null;

        ContractGroup selectedContractGroup = null;

        public VCopyProgress()
        {
            InitializeComponent();
        }
        public VCopyProgress(List<GWBSTree> listSelectNode, List<GWBSDetail> listNodeDetail, ContractGroup currContractGroup)
        {
            InitializeComponent();

            listSelectWBSNode = listSelectNode;
            listCopyNodeDetail = listNodeDetail;
            selectedContractGroup = currContractGroup;

            this.Shown += new EventHandler(VCopyProgress_Shown);
        }

        void VCopyProgress_Shown(object sender, EventArgs e)
        {
            SaveData(listSelectWBSNode, listCopyNodeDetail);
        }

        private void SaveData(List<GWBSTree> listSelectWBSNode, List<GWBSDetail> listCopyNodeDetail)
        {
            try
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = listSelectWBSNode.Count;
                progressBar1.Value = 0;


                List<GWBSDetailLedger> listLedger = new List<GWBSDetailLedger>();
                DateTime serverTime = model.GetServerTime();

                IList listNode = new ArrayList();
                for (int index = 0; index < listSelectWBSNode.Count; index++)
                {
                    GWBSTree oprNode = listSelectWBSNode[index];
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", oprNode.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList listTemp = model.ObjectQuery(typeof(GWBSTree), oq);
                    oprNode = listTemp[0] as GWBSTree;

                    //复制明细数据到选择的工程WBS节点
                    if (listCopyNodeDetail.Count > 0)
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
                                            code =tempCode+ 1;
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

                        #region 复制数据
                        for (int i = 0; i < listCopyNodeDetail.Count; i++)
                        {
                            GWBSDetail dtl = listCopyNodeDetail[i];

                            GWBSDetail tempDtl = new GWBSDetail();

                            //tempDtl.BearOrgGUID = dtl.BearOrgGUID;
                            //tempDtl.BearOrgName = dtl.BearOrgName;

                            tempDtl.Code = oprNode.Code + "-" + (code + i).ToString().PadLeft(5, '0');
                            tempDtl.OrderNo = startOrderNo + i;
                            tempDtl.ContentDesc = dtl.ContentDesc;

                            tempDtl.ContractGroupCode = selectedContractGroup.Code;
                            tempDtl.ContractGroupGUID = selectedContractGroup.Id;
                            tempDtl.ContractGroupName = selectedContractGroup.ContractName;
                            tempDtl.ContractGroupType = selectedContractGroup.ContractGroupType;

                            tempDtl.ContractPrice = dtl.ContractPrice;
                            tempDtl.ContractProjectQuantity = dtl.ContractProjectQuantity;
                            tempDtl.ContractTotalPrice = dtl.ContractTotalPrice;

                            tempDtl.TheCostItem = dtl.TheCostItem;
                            tempDtl.TheCostItemCateSyscode = dtl.TheCostItemCateSyscode;
                            //tempDtl.CostItemName = dtl.CostItemName;
                            tempDtl.State =  VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit;
                            tempDtl.CurrentStateTime = serverTime;
                            tempDtl.DetailExecuteDesc = dtl.DetailExecuteDesc;
                            //tempDtl.FinishedWorkAmount = dtl.FinishedWorkAmount;

                            tempDtl.Name = dtl.Name;

                            tempDtl.PlanPrice = dtl.PlanPrice;
                            tempDtl.PlanTotalPrice = dtl.PlanTotalPrice;
                            tempDtl.PlanWorkAmount = dtl.PlanWorkAmount;

                            tempDtl.PriceUnitGUID = dtl.PriceUnitGUID;
                            tempDtl.PriceUnitName = dtl.PriceUnitName;

                            tempDtl.ProjectTaskTypeCode = dtl.ProjectTaskTypeCode;

                            tempDtl.ResponsibilitilyPrice = dtl.ResponsibilitilyPrice;
                            tempDtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyTotalPrice;
                            tempDtl.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount;

                            tempDtl.Summary = dtl.Summary;
                            //tempDtl.TaskFinishedPercent = dtl.TaskFinishedPercent;

                            tempDtl.ResponseFlag = dtl.ResponseFlag;
                            tempDtl.CostingFlag = dtl.CostingFlag;
                            tempDtl.ProduceConfirmFlag = dtl.ProduceConfirmFlag;
                            tempDtl.SubContractFeeFlag = dtl.SubContractFeeFlag;

                            //tempDtl.QuantityConfirmed = dtl.QuantityConfirmed;
                            //tempDtl.ProgressConfirmed = dtl.ProgressConfirmed;
                            //tempDtl.AddupAccFigureProgress = dtl.AddupAccFigureProgress;
                            //tempDtl.AddupAccQuantity = dtl.AddupAccQuantity;

                            tempDtl.SubContractStepRate = dtl.SubContractStepRate;
                            tempDtl.TheGWBSSysCode = oprNode.SysCode;

                            tempDtl.MainResourceTypeId = dtl.MainResourceTypeId;
                            tempDtl.MainResourceTypeName = dtl.MainResourceTypeName;
                            tempDtl.MainResourceTypeQuality = dtl.MainResourceTypeQuality;
                            tempDtl.MainResourceTypeSpec = dtl.MainResourceTypeSpec;

                            tempDtl.DiagramNumber = dtl.DiagramNumber;

                            tempDtl.TheGWBS = oprNode;
                            oprNode.Details.Add(tempDtl);

                            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                            if (projectInfo != null)
                            {
                                tempDtl.TheProjectGUID = projectInfo.Id;
                                tempDtl.TheProjectName = projectInfo.Name;
                            }
                            else
                            {
                                tempDtl.TheProjectGUID = dtl.TheProjectGUID;
                                tempDtl.TheProjectName = dtl.TheProjectName;
                            }

                            tempDtl.UpdatedDate = serverTime;
                            tempDtl.WorkAmountUnitGUID = dtl.WorkAmountUnitGUID;
                            tempDtl.WorkAmountUnitName = dtl.WorkAmountUnitName;

                            //tempDtl.WorkMethod = dtl.WorkMethod;
                            //tempDtl.WorkPart = dtl.WorkPart;
                            //tempDtl.WorkUseMaterial = dtl.WorkUseMaterial;


                            ObjectQuery oqCostSub = new ObjectQuery();

                            oqCostSub.AddCriterion(Expression.Eq("TheGWBSDetail.Id", dtl.Id));
                            //oqCostSub.AddCriterion("State", GWBSDetailCostSubjectState.生效);
                            oqCostSub.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                            oqCostSub.AddFetchMode("PriceUnitGUID", NHibernate.FetchMode.Eager);
                            oqCostSub.AddFetchMode("ProjectAmountUnitGUID", NHibernate.FetchMode.Eager);
                            oqCostSub.AddFetchMode("ResourceUsageQuota", NHibernate.FetchMode.Eager);

                            IList listCostSubjectDtl = model.ObjectQuery(typeof(GWBSDetailCostSubject), oqCostSub);


                            //明细分科目成本
                            foreach (GWBSDetailCostSubject subject in listCostSubjectDtl)
                            {
                                GWBSDetailCostSubject tempSubject = new GWBSDetailCostSubject();
                                tempSubject.AddupAccountCost = subject.AddupAccountCost;
                                tempSubject.AddupAccountCostEndTime = subject.AddupAccountCostEndTime;
                                tempSubject.AddupAccountProjectAmount = subject.AddupAccountProjectAmount;
                                tempSubject.AddupBalanceProjectAmount = subject.AddupBalanceProjectAmount;
                                tempSubject.AddupBalanceTotalPrice = subject.AddupBalanceTotalPrice;
                                tempSubject.AssessmentRate = subject.AssessmentRate;

                                tempSubject.ContractQuotaQuantity = subject.ContractQuotaQuantity;

                                tempSubject.ContractBasePrice = subject.ContractBasePrice;
                                tempSubject.ContractPricePercent = subject.ContractPricePercent;
                                tempSubject.ContractQuantityPrice = subject.ContractQuantityPrice;

                                tempSubject.ContractPrice = subject.ContractPrice;
                                tempSubject.ContractProjectAmount = subject.ContractProjectAmount;
                                tempSubject.ContractTotalPrice = subject.ContractTotalPrice;

                                tempSubject.CostAccountSubjectGUID = subject.CostAccountSubjectGUID;
                                tempSubject.CostAccountSubjectName = subject.CostAccountSubjectName;
                                tempSubject.CostAccountSubjectSyscode = subject.CostAccountSubjectSyscode;
                                tempSubject.CreateTime = serverTime;
                                tempSubject.CurrentPeriodAccountCost = subject.CurrentPeriodAccountCost;
                                tempSubject.CurrentPeriodAccountCostEndTime = subject.CurrentPeriodAccountCostEndTime;
                                tempSubject.CurrentPeriodAccountProjectAmount = subject.CurrentPeriodAccountProjectAmount;
                                tempSubject.CurrentPeriodBalanceProjectAmount = subject.CurrentPeriodBalanceProjectAmount;
                                tempSubject.CurrentPeriodBalanceTotalPrice = subject.CurrentPeriodBalanceTotalPrice;
                                tempSubject.Name = subject.Name;

                                tempSubject.PlanQuotaNum = subject.PlanQuotaNum;

                                tempSubject.PlanBasePrice = subject.PlanBasePrice;
                                tempSubject.PlanPricePercent = subject.PlanPricePercent;
                                tempSubject.PlanPrice = subject.PlanPrice;

                                tempSubject.PlanWorkPrice = subject.PlanWorkPrice;
                                tempSubject.PlanWorkAmount = subject.PlanWorkAmount;
                                tempSubject.PlanTotalPrice = subject.PlanTotalPrice;

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
                                tempSubject.ResponsibilitilyWorkAmount = subject.ResponsibilitilyWorkAmount;
                                tempSubject.ResponsibilitilyTotalPrice = subject.ResponsibilitilyTotalPrice;

                                tempSubject.ResourceUsageQuota = subject.ResourceUsageQuota;

                                tempSubject.State = GWBSDetailCostSubjectState.编制;

                                if (projectInfo != null)
                                {
                                    tempSubject.TheProjectGUID = projectInfo.Id;
                                    tempSubject.TheProjectName = projectInfo.Name;
                                }
                                else
                                {
                                    tempSubject.TheProjectGUID = subject.TheProjectGUID;
                                    tempSubject.TheProjectName = subject.TheProjectName;
                                }

                                tempSubject.TheGWBSTree = tempDtl.TheGWBS;
                                tempSubject.TheGWBSTreeName = tempDtl.TheGWBS.Name;
                                tempSubject.TheGWBSTreeSyscode = tempDtl.TheGWBS.SysCode;

                                tempSubject.TheGWBSDetail = tempDtl;

                                tempDtl.ListCostSubjectDetails.Add(tempSubject);
                            }
                        }
                        #endregion
                    }

                    //根据明细设置任务对象的标记以及更新合价信息
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



                    listNode.Add(oprNode);

                    //由于最后有保存操作进度条控制不完全显示
                    progressBar1.Value = index;
                }

                //保存复制的明细
                model.SaveOrUpdateGWBSTree(listNode, listLedger);

                progressBar1.Value = progressBar1.Maximum;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                this.Close();
            }
        }
    }
}
