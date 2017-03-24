using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDetailCostEditAndUsageEdit : TBasicDataView
    {
        /// <summary>
        /// 初始任务类型
        /// </summary>
        public TreeNode DefaultGWBSTreeNode = null;

        CurrentProjectInfo projectInfo = null;

        private OptCostType OptionCostType = OptCostType.合同收入;

        Color selectRowBackColor = ColorTranslator.FromHtml("#D7E8FE");//淡蓝色
        Color unSelectRowBackColor_Control = ColorTranslator.FromHtml("#ECE9D8");//控件颜色
        Color unSelectRowBackColor_White = ColorTranslator.FromHtml("#FFFFFF");//白颜色

        private List<string> listDeleteDtlUsages = new List<string>();
        private GWBSDetail addUsageTaskDtl = null;//要添加耗用的任务明细
        private GWBSDetailCostSubject copyDtlUsage = null;//被复制的资源耗用

        private MGWBSTree model = new MGWBSTree();

        public VGWBSDetailCostEditAndUsageEdit()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            usageIsMainResource.Items.Add("是");
            usageIsMainResource.Items.Add("否");

            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
            li.Text = StaticMethod.GetWBSTaskStateText(DocumentState.Edit);
            li.Value = ((int)DocumentState.Edit).ToString();

            System.Web.UI.WebControls.ListItem li2 = new System.Web.UI.WebControls.ListItem();
            li2.Text = StaticMethod.GetWBSTaskStateText(DocumentState.InExecute);
            li2.Value = ((int)DocumentState.InExecute).ToString();

            cbStateBySearch.Items.Add(li);
            cbStateBySearch.Items.Add(li2);
            cbStateBySearch.SelectedIndex = 0;

            cbEditPricePercent_Click(cbEditPricePercent, new EventArgs());
            btnTabPlan_Click(btnTabPlan, new EventArgs());
            //btnAccountCostData_Click(btnAccountCostData, new EventArgs());
        }

        private void InitEvents()
        {
            btnSelectGWBS.Click += new EventHandler(btnSelectGWBS_Click);

            txtCostItemBySearch.LostFocus += new EventHandler(txtCostItemBySearch_LostFocus);

            btnSelectTaskTypeBySearch.Click += new EventHandler(btnSelectGWBSBySearch_Click);
            btnSelectCostItemCate.Click += new EventHandler(btnSelectCostItemCate_Click);
            btnSelectCostItemBySearch.Click += new EventHandler(btnSelectCostItemBySearch_Click);
            btnSelectAccountSubject.Click += new EventHandler(btnSelectAccountSubject_Click);
            btnSelectResourceType.Click += new EventHandler(btnSelectResourceType_Click);
            btnSelectContract.Click += new EventHandler(btnSelectContract_Click);

            btnQuery.Click += new EventHandler(btnQuery_Click);

            btnTabContract.Click += new EventHandler(btnTabContract_Click);
            btnTabResponsible.Click += new EventHandler(btnTabResponsible_Click);
            btnTabPlan.Click += new EventHandler(btnTabPlan_Click);

            cbTaskDtlSelect.CheckedChanged += new EventHandler(cbTaskDtlSelect_CheckedChanged);
            cbUsageSelect.CheckedChanged += new EventHandler(cbUsageSelect_CheckedChanged);

            linkRemoveDtl.Click += new EventHandler(linkRemoveDtl_Click);
            linkRemoveDtlUsage.Click += new EventHandler(linkRemoveDtlUsage_Click);

            btnCopyPlanToResponsible.Click += new EventHandler(btnCopyPlanToResponsible_Click);
            btnCopyResponsibleToPlan.Click += new EventHandler(btnCopyResponsibleToPlan_Click);
            btnAccountCostData.Click += new EventHandler(btnAccountCostData_Click);
            btnSubcontractFeeAcc.Click += new EventHandler(btnSubcontractFeeAcc_Click);
            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveAndExit.Click += new EventHandler(btnSaveAndExit_Click);
            btnGiveup.Click += new EventHandler(btnGiveup_Click);

            gridGWBDetail.CellContentClick += new DataGridViewCellEventHandler(gridGWBDetail_CellContentClick);
            gridGWBDetail.CellMouseDown += new DataGridViewCellMouseEventHandler(gridGWBDetail_CellMouseDown);

            gridGWBDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridGWBDetail_CellValidating);
            gridGWBDetail.CellEndEdit += new DataGridViewCellEventHandler(gridGWBDetail_CellEndEdit);

            gridDtlUsage.CellValidating += new DataGridViewCellValidatingEventHandler(gridDtlUsage_CellValidating);
            gridDtlUsage.CellEndEdit += new DataGridViewCellEventHandler(gridDtlUsage_CellEndEdit);
            gridDtlUsage.CellDoubleClick += new DataGridViewCellEventHandler(gridDtlUsage_CellDoubleClick);

            gridDtlUsage.CellMouseDown += new DataGridViewCellMouseEventHandler(gridDtlUsage_CellMouseDown);
            gridDtlUsage.MouseDown += new MouseEventHandler(gridDtlUsage_MouseDown);
            gridDtlUsage.CellContentClick += new DataGridViewCellEventHandler(gridDtlUsage_CellContentClick);


            contextMenuQueryElsePriceDtl.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuQueryElsePriceDtl_ItemClicked);
            contextMenuQueryElsePriceDtlUsage.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuQueryElsePriceDtlUsage_ItemClicked);

            cbEditPricePercent.Click += new EventHandler(cbEditPricePercent_Click);

            this.Load += new EventHandler(VGWBSDetailCostEdit_Load);
        }

        //选择项目任务
        void btnSelectGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                DefaultGWBSTreeNode = frm.SelectResult[0];
                txtCurrentPath.Text = DefaultGWBSTreeNode.FullPath;
            }
        }
        //任务明细快捷菜单
        void contextMenuQueryElsePriceDtl_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuQueryElsePriceDtl.Hide();

            if (e.ClickedItem.Name == 查询其他项目单价ToolStripMenuItem.Name)
            {
                #region 查询其他项目单价

                GWBSDetail optDtl = gridGWBDetail.Rows[gridGWBDetail.CurrentCell.RowIndex].Tag as GWBSDetail;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", optDtl.Id));
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                optDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;

                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                oq.AddCriterion(Expression.Not(Expression.Eq("TheProjectGUID", projectInfo.Id)));
                oq.AddCriterion(Expression.Eq("TheCostItem.Id", optDtl.TheCostItem.Id));
                oq.AddCriterion(Expression.Eq("MainResourceTypeId", optDtl.MainResourceTypeId));
                oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));

                IList listElsePriceDtl = model.ObjectQuery(typeof(GWBSDetail), oq);

                VElseProTaskDtlPricePreview frm = new VElseProTaskDtlPricePreview(OptionCostType);
                frm.DefaultListTaskDtl = listElsePriceDtl;
                frm.ShowDialog();
                #endregion
            }
            else if (e.ClickedItem.Name == 批量设置单价ToolStripMenuItem.Name)
            {
                #region 批量设置单价

                VGWBSDetailCostEditInputPriceByDtl frm = new VGWBSDetailCostEditInputPriceByDtl(OptionCostType);
                frm.ShowDialog();
                decimal priceValue = frm.PriceValue;

                if (frm.IsOK == false)
                    return;

                if (frm.SetPriceType == OptSetPriceType.直接输入)
                {
                    foreach (DataGridViewRow row in gridGWBDetail.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[colTaskDtlSelect.Name].Value))
                        {
                            GWBSDetail dtl = row.Tag as GWBSDetail;

                            if (OptionCostType == OptCostType.合同收入)
                            {
                                dtl.ContractPrice = priceValue;

                                row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
                            }
                            else if (OptionCostType == OptCostType.责任成本)
                            {
                                dtl.ResponsibilitilyPrice = priceValue;

                                row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
                            }
                            else if (OptionCostType == OptCostType.计划成本)
                            {
                                dtl.PlanPrice = priceValue;

                                row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
                            }

                            row.Tag = dtl;
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in gridGWBDetail.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[colTaskDtlSelect.Name].Value))
                        {
                            GWBSDetail dtl = row.Tag as GWBSDetail;

                            if (OptionCostType == OptCostType.合同收入)
                            {
                                //dtl.ContractPricePercent = priceValue;
                                dtl.ContractPrice = dtl.ContractPrice * priceValue;

                                //row.Cells[colContractPricePercent.Name].Value = dtl.ContractPricePercent;
                                row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
                            }
                            else if (OptionCostType == OptCostType.责任成本)
                            {
                                //dtl.ResponsiblePricePercent = priceValue;
                                dtl.ResponsibilitilyPrice = dtl.ResponsibilitilyPrice * priceValue;

                                //row.Cells[colResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
                                row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
                            }
                            else if (OptionCostType == OptCostType.计划成本)
                            {
                                //dtl.PlanPricePercent = priceValue;
                                dtl.PlanPrice = dtl.PlanPrice * priceValue;

                                //row.Cells[colPlanPricePercent.Name].Value = dtl.PlanPricePercent;
                                row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
                            }

                            row.Tag = dtl;
                        }
                    }
                }
                #endregion
            }
            else if (e.ClickedItem.Name == 设置勾选行为责任明细ToolStripMenuItem.Name)
            {
                foreach (DataGridViewRow row in gridGWBDetail.Rows)
                {
                    bool isSelect = false;
                    object value = row.Cells[colTaskDtlSelect.Name].EditedFormattedValue;
                    if (value != null)
                    {
                        isSelect = Convert.ToBoolean(value);
                    }
                    if (isSelect == false)
                        continue;

                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    dtl.ResponseFlag = 1;
                    row.Tag = dtl;

                    row.Cells[colResponsibleAccFlag.Name].Value = "是";
                }
            }
            else if (e.ClickedItem.Name == 设置勾选行为计划明细ToolStripMenuItem.Name)
            {
                foreach (DataGridViewRow row in gridGWBDetail.Rows)
                {
                    bool isSelect = false;
                    object value = row.Cells[colTaskDtlSelect.Name].EditedFormattedValue;
                    if (value != null)
                    {
                        isSelect = Convert.ToBoolean(value);
                    }
                    if (isSelect == false)
                        continue;

                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    dtl.CostingFlag = 1;
                    row.Tag = dtl;

                    row.Cells[colCostAccFlag.Name].Value = "是";
                }
            }
        }
        //资源耗用快捷菜单
        void contextMenuQueryElsePriceDtlUsage_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuQueryElsePriceDtlUsage.Hide();

            if (e.ClickedItem.Name == 查询其他项目单价ToolStripMenuItem1.Name)
            {
                #region 查询其他项目单价

                GWBSDetailCostSubject optDtl = gridDtlUsage.Rows[gridDtlUsage.CurrentCell.RowIndex].Tag as GWBSDetailCostSubject;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", optDtl.Id));
                oq.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheGWBSDetail", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheGWBSDetail.TheCostItem", NHibernate.FetchMode.Eager);
                optDtl = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq)[0] as GWBSDetailCostSubject;


                oq.Criterions.Clear();
                oq.FetchModes.Clear();
                oq.AddCriterion(Expression.Not(Expression.Eq("TheProjectGUID", projectInfo.Id)));
                oq.AddCriterion(Expression.Eq("State", GWBSDetailCostSubjectState.生效));
                oq.AddCriterion(Expression.Eq("ResourceTypeGUID", optDtl.ResourceTypeGUID));
                oq.AddCriterion(Expression.Eq("CostAccountSubjectGUID.Id", optDtl.CostAccountSubjectGUID.Id));
                oq.AddCriterion(Expression.Eq("TheGWBSDetail.MainResourceTypeId", optDtl.TheGWBSDetail.MainResourceTypeId));
                oq.AddCriterion(Expression.Eq("TheGWBSDetail.TheCostItem.Id", optDtl.TheGWBSDetail.TheCostItem.Id));
                oq.AddFetchMode("TheGWBSDetail.TheGWBS", NHibernate.FetchMode.Eager);


                IList listElsePriceDtlUsage = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);

                VElseProTaskDtlUsagePricePreview frm = new VElseProTaskDtlUsagePricePreview(OptionCostType);
                frm.DefaultListTaskDtlUsage = listElsePriceDtlUsage;
                frm.DefaultTaskDtlUsage = optDtl;
                frm.ShowDialog();
                #endregion
            }
            else if (e.ClickedItem.Name == 批量设置单价ToolStripMenuItem1.Name)
            {
                #region 批量设置单价
                VGWBSDetailCostEditInputPrice frm = new VGWBSDetailCostEditInputPrice(OptionCostType);
                frm.ShowDialog();
                decimal quantityPriceValue = frm.QuantityPriceValue;
                decimal projectAmountPriceValue = frm.ProjectAmountPriceValue;

                if (frm.IsOK == false)
                    return;

                if (frm.SetPriceType == OptSetPriceType.直接输入)
                {
                    #region 直接输入单价

                    foreach (DataGridViewRow row in gridDtlUsage.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
                        {
                            GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;

                            if (OptionCostType == OptCostType.合同收入)
                            {
                                subject.ContractBasePrice = quantityPriceValue;
                                subject.ContractQuantityPrice = subject.ContractBasePrice * subject.ContractPricePercent;

                                subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;

                                row.Cells[usageContractQuantityPrice.Name].Value = subject.ContractBasePrice;
                                row.Cells[usageContractQnyPriceResult.Name].Value = subject.ContractQuantityPrice;
                                row.Cells[usageContractWorkAmountPrice.Name].Value = subject.ContractPrice;

                            }
                            else if (OptionCostType == OptCostType.责任成本)
                            {
                                subject.ResponsibleBasePrice = quantityPriceValue;
                                subject.ResponsibilitilyPrice = subject.ResponsibleBasePrice * subject.ResponsiblePricePercent;

                                subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;

                                row.Cells[usageResponsibleQuantityPrice.Name].Value = subject.ResponsibleBasePrice;
                                row.Cells[usageResponsibleQnyPriceResult.Name].Value = subject.ResponsibilitilyPrice;
                                row.Cells[usageResponsibleWorkAmountPrice.Name].Value = subject.ResponsibleWorkPrice;
                            }
                            else if (OptionCostType == OptCostType.计划成本)
                            {
                                subject.PlanBasePrice = quantityPriceValue;
                                subject.PlanPrice = subject.PlanBasePrice * subject.PlanPricePercent;

                                subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;

                                row.Cells[usagePlanQuantityPrice.Name].Value = subject.PlanBasePrice;
                                row.Cells[usagePlanQnyPriceResult.Name].Value = subject.PlanPrice;
                                row.Cells[usagePlanWorkAmountPrice.Name].Value = subject.PlanWorkPrice;
                            }

                            row.Tag = subject;
                        }
                    }

                    #region 未使用，所以projectAmountPriceValue始终为0
                    //if (projectAmountPriceValue != 0)
                    //{
                    //    //修改后联动计算【责任数量单价】=【责任工程量单价】，【责任定额数量】=1，【数量计量单位】=所属{工程任务明细}_【工程量计量单位】,合同收入和计划成本算法相似
                    //    foreach (DataGridViewRow row in gridDtlUsage.Rows)
                    //    {
                    //        if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
                    //        {
                    //            GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;

                    //            if (OptionCostType == OptCostType.合同收入)
                    //            {
                    //                subject.ContractPrice = projectAmountPriceValue;
                    //                subject.ContractQuantityPrice = subject.ContractPrice;
                    //                subject.ContractQuotaQuantity = 1;

                    //                subject.ProjectAmountUnitGUID = subject.TheGWBSDetail.WorkAmountUnitGUID;
                    //                subject.ProjectAmountUnitName = subject.TheGWBSDetail.WorkAmountUnitName;

                    //                row.Cells[usageContractWorkAmountPrice.Name].Value = subject.ContractPrice;
                    //                row.Cells[usageContractQuantityPrice.Name].Value = subject.ContractQuantityPrice;
                    //                row.Cells[usageContractQuotaNum.Name].Value = subject.ContractQuotaQuantity;
                    //            }
                    //            else if (OptionCostType == OptCostType.责任成本)
                    //            {
                    //                subject.ResponsibleWorkPrice = projectAmountPriceValue;
                    //                subject.ResponsibilitilyPrice = subject.ResponsibleWorkPrice;
                    //                subject.ResponsibleQuotaNum = 1;

                    //                subject.ProjectAmountUnitGUID = subject.TheGWBSDetail.WorkAmountUnitGUID;
                    //                subject.ProjectAmountUnitName = subject.TheGWBSDetail.WorkAmountUnitName;

                    //                row.Cells[usageResponsibleWorkAmountPrice.Name].Value = subject.ResponsibleWorkPrice;
                    //                row.Cells[usageResponsibleQuantityPrice.Name].Value = subject.ResponsibilitilyPrice;
                    //                row.Cells[usageResponsibleQuotaNum.Name].Value = subject.ResponsibleQuotaNum;
                    //            }
                    //            else if (OptionCostType == OptCostType.计划成本)
                    //            {
                    //                subject.PlanWorkPrice = projectAmountPriceValue;
                    //                subject.PlanPrice = subject.PlanWorkPrice;
                    //                subject.PlanQuotaNum = 1;

                    //                subject.ProjectAmountUnitGUID = subject.TheGWBSDetail.WorkAmountUnitGUID;
                    //                subject.ProjectAmountUnitName = subject.TheGWBSDetail.WorkAmountUnitName;

                    //                row.Cells[usagePlanWorkAmountPrice.Name].Value = subject.PlanWorkPrice;
                    //                row.Cells[usagePlanQuantityPrice.Name].Value = subject.PlanPrice;
                    //                row.Cells[usagePlanQuotaNum.Name].Value = subject.PlanQuotaNum;
                    //            }

                    //            row.Tag = subject;
                    //        }
                    //    }
                    //}
                    #endregion

                    #endregion
                }
                else
                {
                    #region 根据系数调整单价

                    foreach (DataGridViewRow row in gridDtlUsage.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
                        {
                            GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;

                            if (OptionCostType == OptCostType.合同收入)
                            {
                                subject.ContractPricePercent = quantityPriceValue;//系数值
                                subject.ContractQuantityPrice = subject.ContractBasePrice * subject.ContractPricePercent;

                                subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;

                                row.Cells[usageContractQnyPricePercent.Name].Value = subject.ContractPricePercent;
                                row.Cells[usageContractQnyPriceResult.Name].Value = subject.ContractQuantityPrice;
                                row.Cells[usageContractWorkAmountPrice.Name].Value = subject.ContractPrice;
                            }
                            else if (OptionCostType == OptCostType.责任成本)
                            {
                                subject.ResponsiblePricePercent = quantityPriceValue;//系数值
                                subject.ResponsibilitilyPrice = subject.ResponsibleBasePrice * subject.ResponsiblePricePercent;
                                subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;

                                row.Cells[usageResponsibleQnyPricePercent.Name].Value = subject.ResponsiblePricePercent;
                                row.Cells[usageResponsibleQnyPriceResult.Name].Value = subject.ResponsibilitilyPrice;
                                row.Cells[usageResponsibleWorkAmountPrice.Name].Value = subject.ResponsibleWorkPrice;
                            }
                            else if (OptionCostType == OptCostType.计划成本)
                            {
                                subject.PlanPricePercent = quantityPriceValue;//系数值
                                subject.PlanPrice = subject.PlanBasePrice * subject.PlanPricePercent;
                                subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;

                                row.Cells[usagePlanQnyPricePercent.Name].Value = subject.PlanPricePercent;
                                row.Cells[usagePlanQnyPriceResult.Name].Value = subject.PlanPrice;
                                row.Cells[usagePlanWorkAmountPrice.Name].Value = subject.PlanWorkPrice;
                            }

                            row.Tag = subject;
                        }
                    }

                    #endregion
                }

                UpdateDetailUsageQnyPriceBackColor();

                #endregion
            }
            else if (e.ClickedItem.Name == 复制资源类型ToolStripMenuItem1.Name)
            {
                #region 复制资源类型

                if (gridDtlUsage.CurrentRow != null)
                {
                    copyDtlUsage = gridDtlUsage.CurrentRow.Tag as GWBSDetailCostSubject;
                }

                #endregion
            }
            else if (e.ClickedItem.Name == 粘贴资源类型到当前行ToolStripMenuItem1.Name)
            {
                #region 粘贴资源类型到当前行

                if (gridDtlUsage.CurrentRow != null)
                {
                    GWBSDetailCostSubject tempDtlUsage = gridDtlUsage.CurrentRow.Tag as GWBSDetailCostSubject;
                    if (tempDtlUsage.ResourceTypeGUID != copyDtlUsage.ResourceTypeGUID)
                    {
                        tempDtlUsage.ResourceCateSyscode = copyDtlUsage.ResourceCateSyscode;
                        tempDtlUsage.ResourceTypeCode = copyDtlUsage.ResourceTypeCode;
                        tempDtlUsage.ResourceTypeGUID = copyDtlUsage.ResourceTypeGUID;
                        tempDtlUsage.ResourceTypeName = copyDtlUsage.ResourceTypeName;
                        tempDtlUsage.ResourceTypeQuality = copyDtlUsage.ResourceTypeQuality;
                        tempDtlUsage.ResourceTypeSpec = copyDtlUsage.ResourceTypeSpec;

                        tempDtlUsage.IsCategoryResource = copyDtlUsage.IsCategoryResource;

                        gridDtlUsage.CurrentRow.Cells[usageResourceType.Name].Value = tempDtlUsage.ResourceTypeName;
                        gridDtlUsage.CurrentRow.Cells[usageSpec.Name].Value = tempDtlUsage.ResourceTypeSpec;

                        gridDtlUsage.CurrentRow.Tag = tempDtlUsage;

                        if (tempDtlUsage.MainResTypeFlag)
                        {
                            UpdateTaskDtlMainResource(tempDtlUsage, false);
                        }
                    }
                }

                #endregion
            }
            else if (e.ClickedItem.Name == 粘贴资源类型到勾选行ToolStripMenuItem1.Name)
            {
                #region 粘贴资源类型到勾选行

                foreach (DataGridViewRow row in gridDtlUsage.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value) == true)
                    {
                        GWBSDetailCostSubject tempDtlUsage = row.Tag as GWBSDetailCostSubject;
                        if (tempDtlUsage.ResourceTypeGUID != copyDtlUsage.ResourceTypeGUID)
                        {
                            tempDtlUsage.ResourceCateSyscode = copyDtlUsage.ResourceCateSyscode;
                            tempDtlUsage.ResourceTypeCode = copyDtlUsage.ResourceTypeCode;
                            tempDtlUsage.ResourceTypeGUID = copyDtlUsage.ResourceTypeGUID;
                            tempDtlUsage.ResourceTypeName = copyDtlUsage.ResourceTypeName;
                            tempDtlUsage.ResourceTypeQuality = copyDtlUsage.ResourceTypeQuality;
                            tempDtlUsage.ResourceTypeSpec = copyDtlUsage.ResourceTypeSpec;

                            row.Cells[usageResourceType.Name].Value = tempDtlUsage.ResourceTypeName;
                            row.Cells[usageSpec.Name].Value = tempDtlUsage.ResourceTypeSpec;

                            row.Tag = tempDtlUsage;

                            if (tempDtlUsage.MainResTypeFlag)
                            {
                                UpdateTaskDtlMainResource(tempDtlUsage, false);
                            }
                        }
                    }
                }

                #endregion
            }
            else if (e.ClickedItem.Name == 添加资源耗用ToolStripMenuItem1.Name)
            {
                #region 添加资源耗用
                GWBSDetailCostSubject dtlUsage = new GWBSDetailCostSubject();
                if (gridDtlUsage.Rows.Count > 0)
                {
                    GWBSDetailCostSubject tempUsage = null;

                    foreach (DataGridViewRow row in gridDtlUsage.Rows)
                    {
                        if (row.Visible == false)
                            continue;

                        GWBSDetailCostSubject temp = row.Tag as GWBSDetailCostSubject;
                        if (temp.CostAccountSubjectName.IndexOf("主材") > -1)//默取认任务明细下的主材耗用的核算科目，不用id和编码判断，如果应用到其他系统的话还需要调整程序
                        {
                            tempUsage = temp;
                            break;
                        }
                    }

                    if (tempUsage == null)
                    {
                        foreach (DataGridViewRow row in gridDtlUsage.Rows)
                        {
                            if (row.Visible == false)
                                continue;
                            tempUsage = row.Tag as GWBSDetailCostSubject;
                            break;
                        }
                    }
                    else
                    {
                        dtlUsage.CostAccountSubjectGUID = tempUsage.CostAccountSubjectGUID;
                        dtlUsage.CostAccountSubjectName = tempUsage.CostAccountSubjectName;
                        dtlUsage.CostAccountSubjectSyscode = tempUsage.CostAccountSubjectSyscode;
                    }

                    dtlUsage.ProjectAmountUnitGUID = tempUsage.ProjectAmountUnitGUID;
                    dtlUsage.ProjectAmountUnitName = tempUsage.ProjectAmountUnitName;

                    dtlUsage.PriceUnitGUID = tempUsage.PriceUnitGUID;
                    dtlUsage.PriceUnitName = tempUsage.PriceUnitName;
                }

                if (dtlUsage.PriceUnitGUID == null)
                {
                    dtlUsage.PriceUnitGUID = addUsageTaskDtl.PriceUnitGUID;
                    dtlUsage.PriceUnitName = addUsageTaskDtl.PriceUnitName;

                    if (dtlUsage.PriceUnitGUID == null)
                    {
                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("Name", "元"));
                        oq.AddCriterion(Expression.Eq("State", 1));
                        IList listUnit = model.ObjectQuery(typeof(StandardUnit), oq);
                        if (listUnit.Count > 0)
                        {
                            dtlUsage.PriceUnitGUID = listUnit[0] as StandardUnit;
                            dtlUsage.PriceUnitName = dtlUsage.PriceUnitGUID.Name;
                        }
                    }
                }

                dtlUsage.State = GWBSDetailCostSubjectState.编制;
                dtlUsage.TheGWBSDetail = addUsageTaskDtl;
                dtlUsage.TheGWBSTree = addUsageTaskDtl.TheGWBS;
                dtlUsage.TheGWBSTreeName = addUsageTaskDtl.TheGWBS.Name;
                dtlUsage.TheGWBSTreeSyscode = addUsageTaskDtl.TheGWBSSysCode;
                dtlUsage.TheProjectGUID = addUsageTaskDtl.TheProjectGUID;
                dtlUsage.TheProjectName = addUsageTaskDtl.TheProjectName;
                dtlUsage.MainResTypeFlag = false;

                AddDetailUsageInfoInGrid(dtlUsage, true);

                #endregion
            }
            else if (e.ClickedItem.Name == 删除资源耗用ToolStripMenuItem1.Name)
            {
                #region 删除资源耗用

                DataGridViewRow row = gridDtlUsage.Rows[gridDtlUsage.CurrentCell.RowIndex];
                GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;

                if (MessageBox.Show("确认要删除资源耗用“" + dtl.Name + "”吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                if (string.IsNullOrEmpty(dtl.Id) == false)
                {
                    listDeleteDtlUsages.Add(dtl.Id);
                }

                gridDtlUsage.Rows.RemoveAt(row.Index);

                #endregion
            }
            else if (e.ClickedItem.Name == 拷贝责任成本到计划成本ToolStripMenuItem.Name)
            {
                if (MessageBox.Show("确认要拷贝勾选行的责任成本到计划成本吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                btnCopyResponsibleToPlan_Click(btnCopyResponsibleToPlan, new EventArgs());

            }
            else if (e.ClickedItem.Name == 拷贝计划成本到责任成本ToolStripMenuItem.Name)
            {
                if (MessageBox.Show("确认要拷贝勾选行的计划成本到责任成本吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                btnCopyPlanToResponsible_Click(btnCopyPlanToResponsible, new EventArgs());
            }
            else if (e.ClickedItem.Name == 拷贝计划成本到合同收入ToolStripMenuItem.Name)
            {
                if (MessageBox.Show("确认要拷贝勾选行的计划成本到合同收入吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                CopyPlanToContractIncome();
            }
        }
        //编辑单价调整系数
        void cbEditPricePercent_Click(object sender, EventArgs e)
        {
            gridDtlUsage.Columns[usageContractQnyPricePercent.Name].ReadOnly = !cbEditPricePercent.Checked;
            gridDtlUsage.Columns[usageResponsibleQnyPricePercent.Name].ReadOnly = !cbEditPricePercent.Checked;
            gridDtlUsage.Columns[usagePlanQnyPricePercent.Name].ReadOnly = !cbEditPricePercent.Checked;
        }

        //全选（任务明细 ）
        void cbTaskDtlSelect_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                row.Cells[colTaskDtlSelect.Name].Value = cbTaskDtlSelect.Checked;

                if (cbTaskDtlSelect.Checked)
                    row.Cells[colTaskDtlSelect.Name].Style.BackColor = selectRowBackColor;
                else
                    row.Cells[colTaskDtlSelect.Name].Style.BackColor = unSelectRowBackColor_White;

                SetDtlUsageVisable(row.Tag as GWBSDetail, cbTaskDtlSelect.Checked);

            }

            cbUsageSelect.Checked = cbTaskDtlSelect.Checked;
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                row.Cells[usageSelect.Name].Value = cbUsageSelect.Checked;

                if (cbUsageSelect.Checked)
                    row.Cells[usageSelect.Name].Style.BackColor = selectRowBackColor;
                else
                    row.Cells[usageSelect.Name].Style.BackColor = unSelectRowBackColor_White;
            }

            GetDtlUsageSumQnyAndPrice();
        }
        //全选（明细资源耗用 ）
        void cbUsageSelect_CheckedChanged(object sender, EventArgs e)
        {
            cbTaskDtlSelect.CheckedChanged -= new EventHandler(cbTaskDtlSelect_CheckedChanged);
            bool isChecked = cbUsageSelect.Checked;
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                row.Cells[usageSelect.Name].Value = isChecked;

                if (isChecked)
                    row.Cells[usageSelect.Name].Style.BackColor = selectRowBackColor;
                else
                    row.Cells[usageSelect.Name].Style.BackColor = unSelectRowBackColor_White;
            }

            cbTaskDtlSelect.Checked = isChecked;

            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                row.Cells[colTaskDtlSelect.Name].Value = isChecked;

                if (isChecked)
                    row.Cells[colTaskDtlSelect.Name].Style.BackColor = selectRowBackColor;
                else
                    row.Cells[colTaskDtlSelect.Name].Style.BackColor = unSelectRowBackColor_White;

            }
            cbTaskDtlSelect.CheckedChanged += new EventHandler(cbTaskDtlSelect_CheckedChanged);

            GetDtlUsageSumQnyAndPrice();
        }

        //复选框选择处理(任务明细)
        void gridGWBDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                string colName = gridGWBDetail.Columns[e.ColumnIndex].Name;
                if (colName == colTaskDtlSelect.Name)
                {
                    object value = gridGWBDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
                    if (value != null)
                    {
                        bool isSelect = Convert.ToBoolean(value);
                        DataGridViewRow row = gridGWBDetail.Rows[e.RowIndex];
                        if (isSelect)
                        {
                            row.Cells[colTaskDtlSelect.Name].Style.BackColor = selectRowBackColor;
                        }
                        else
                        {
                            row.Cells[colTaskDtlSelect.Name].Style.BackColor = unSelectRowBackColor_White;
                        }


                        GWBSDetail dtl = row.Tag as GWBSDetail;

                        SetDtlUsageChecked(dtl, isSelect);

                        SetDtlUsageVisable(dtl, isSelect);
                    }

                    GetDtlUsageSumQnyAndPrice();
                }
            }
        }
        void gridGWBDetail_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                gridGWBDetail.Rows[e.RowIndex].Selected = true;
                gridGWBDetail.CurrentCell = gridGWBDetail.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }
        void gridGWBDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridGWBDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                //object value = gridGWBDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue;
                object tempValue = e.FormattedValue;
                if (tempValue != null)
                {
                    string value = "";
                    if (tempValue != null)
                        value = tempValue.ToString().Trim();
                    try
                    {
                        string colName = gridGWBDetail.Columns[e.ColumnIndex].Name;

                        //数据格式校验
                        if (colName == colContractQuantity.Name || colName == colContractPrice.Name)//合同
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                        if (colName == colResponsibleQuantity.Name || colName == colResponsiblePrice.Name)//责任
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                        if (colName == colPlanQuantity.Name || colName == colPlanPrice.Name)//计划
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("输入格式不正确！");
                        e.Cancel = true;
                    }
                }
            }
        }
        void gridGWBDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridGWBDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                object tempValue = gridGWBDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                DataGridViewRow currEditRow = gridGWBDetail.Rows[e.RowIndex];
                GWBSDetail dtl = gridGWBDetail.Rows[e.RowIndex].Tag as GWBSDetail;

                string colName = gridGWBDetail.Columns[e.ColumnIndex].Name;

                //耗用数据
                if (OptionCostType == OptCostType.合同收入)
                {
                    if (colName == colContractQuantity.Name)
                    {
                        decimal ContractProjectQuantity = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractProjectQuantity = ClientUtil.ToDecimal(value);

                        dtl.ContractProjectQuantity = ContractProjectQuantity;
                    }
                    else if (colName == colContractPrice.Name)
                    {
                        decimal ContractPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractPrice = ClientUtil.ToDecimal(value);

                        dtl.ContractPrice = ContractPrice;
                    }
                }
                else if (OptionCostType == OptCostType.责任成本)
                {
                    if (colName == colResponsibleQuantity.Name)
                    {
                        decimal ResponsibilitilyWorkAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(value);

                        dtl.ResponsibilitilyWorkAmount = ResponsibilitilyWorkAmount;
                    }
                    else if (colName == colResponsiblePrice.Name)
                    {
                        decimal ResponsibilitilyPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyPrice = ClientUtil.ToDecimal(value);

                        dtl.ResponsibilitilyPrice = ResponsibilitilyPrice;

                    }

                }
                else if (OptionCostType == OptCostType.计划成本)
                {
                    if (colName == colPlanQuantity.Name)
                    {
                        decimal PlanWorkAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanWorkAmount = ClientUtil.ToDecimal(value);

                        dtl.PlanWorkAmount = PlanWorkAmount;
                    }
                    else if (colName == colPlanPrice.Name)
                    {
                        decimal PlanPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanPrice = ClientUtil.ToDecimal(value);

                        dtl.PlanPrice = PlanPrice;
                    }
                }

                currEditRow.Tag = dtl;
            }
        }

        //复选框选择处理(明细耗用)
        void gridDtlUsage_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridDtlUsage.Rows[e.RowIndex].ReadOnly == false)
            {
                object tempValue = e.FormattedValue;
                if (tempValue != null)
                {
                    string value = "";
                    if (tempValue != null)
                        value = tempValue.ToString().Trim();
                    try
                    {
                        GWBSDetailCostSubject dtlUsage = gridDtlUsage.Rows[e.RowIndex].Tag as GWBSDetailCostSubject;

                        string colName = gridDtlUsage.Columns[e.ColumnIndex].Name;

                        decimal editValue = 0;

                        //数据格式校验
                        if (colName == usageContractQuotaNum.Name || colName == usageContractQuantityPrice.Name)//合同耗用
                        {
                            if (value.ToString() != "")
                                editValue = ClientUtil.ToDecimal(value);
                        }
                        else if (colName == usageResponsibleQuotaNum.Name || colName == usageResponsibleQuantityPrice.Name)//责任耗用
                        {
                            if (value.ToString() != "")
                                editValue = ClientUtil.ToDecimal(value);
                        }
                        else if (colName == usagePlanQuotaNum.Name || colName == usagePlanQuantityPrice.Name)//计划耗用
                        {
                            if (value.ToString() != "")
                                editValue = ClientUtil.ToDecimal(value);
                        }
                        else if (colName == usageIsMainResource.Name)
                        {
                            bool flag = value == "是";
                            if (flag)
                            {
                                if (ValidateMainResourceFlag(dtlUsage) == false)
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                        else if (colName == usageContractQnyPricePercent.Name || colName == usageResponsibleQnyPricePercent.Name || colName == usagePlanQnyPricePercent.Name)//调整系数
                        {
                            if (value.ToString() != "")
                                editValue = ClientUtil.ToDecimal(value);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("输入格式不正确！");
                        e.Cancel = true;
                    }
                }
            }
        }
        void gridDtlUsage_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridDtlUsage.Rows[e.RowIndex].ReadOnly == false)
            {
                object tempValue = gridDtlUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                DataGridViewRow currEditRow = gridDtlUsage.Rows[e.RowIndex];
                GWBSDetailCostSubject dtl = gridDtlUsage.Rows[e.RowIndex].Tag as GWBSDetailCostSubject;

                string colName = gridDtlUsage.Columns[e.ColumnIndex].Name;

                #region 修改耗用的预算量、价及更新单元格显示
                if (colName == usageName.Name)
                {
                    dtl.Name = value;
                }
                else if (colName == usageDiagramNumber.Name)
                {
                    if (dtl.DiagramNumber != value)
                    {
                        dtl.DiagramNumber = value;

                        if (dtl.MainResTypeFlag)
                            UpdateTaskDtlDiagramNumber(dtl);
                    }
                }
                else if (colName == usageIsMainResource.Name)
                {
                    bool flag = value == "是";
                    if (dtl.MainResTypeFlag != flag)
                    {
                        dtl.MainResTypeFlag = flag;

                        UpdateTaskDtlMainResource(dtl, !flag);
                    }
                }
                //耗用成本数据
                else if (OptionCostType == OptCostType.合同收入)
                {
                    if (colName == usageContractQuotaNum.Name)
                    {
                        decimal ContractQuotaQuantity = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractQuotaQuantity = ClientUtil.ToDecimal(value);

                        dtl.ContractQuotaQuantity = ContractQuotaQuantity;
                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;

                        currEditRow.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
                    }
                    else if (colName == usageContractQuantityPrice.Name)
                    {
                        decimal ContractQuantityPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractQuantityPrice = ClientUtil.ToDecimal(value);

                        dtl.ContractBasePrice = ContractQuantityPrice;
                        dtl.ContractQuantityPrice = ContractQuantityPrice * dtl.ContractPricePercent;//数量单价结果

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;//合同耗用数量包括定额数量值，合同工程量单价包括定额数量值，故此处用耗用数量*数量单价，防止重复计算定额数量

                        currEditRow.Cells[usageContractQnyPriceResult.Name].Value = dtl.ContractQuantityPrice;
                        currEditRow.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
                        currEditRow.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

                    }
                    else if (colName == usageContractQnyPricePercent.Name)
                    {
                        decimal ContractPricePercent = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractPricePercent = ClientUtil.ToDecimal(value);

                        dtl.ContractPricePercent = ContractPricePercent;
                        dtl.ContractQuantityPrice = ContractPricePercent * dtl.ContractBasePrice;//数量单价系数

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;//合同耗用数量包括定额数量值，合同工程量单价包括定额数量值，故此处用耗用数量*数量单价，防止重复计算定额数量

                        currEditRow.Cells[usageContractQnyPriceResult.Name].Value = dtl.ContractQuantityPrice;
                        currEditRow.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
                        currEditRow.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

                    }
                }
                else if (OptionCostType == OptCostType.责任成本)
                {
                    if (colName == usageResponsibleQuotaNum.Name)
                    {
                        decimal ResponsibleQuotaNum = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibleQuotaNum = ClientUtil.ToDecimal(value);

                        dtl.ResponsibleQuotaNum = ResponsibleQuotaNum;

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;

                        currEditRow.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                    }
                    else if (colName == usageResponsibleQuantityPrice.Name)
                    {
                        decimal ResponsibilitilyPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyPrice = ClientUtil.ToDecimal(value);

                        dtl.ResponsibleBasePrice = ResponsibilitilyPrice;
                        dtl.ResponsibilitilyPrice = ResponsibilitilyPrice * dtl.ResponsiblePricePercent;//数量单价结果

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyPrice * dtl.ResponsibilitilyWorkAmount;

                        currEditRow.Cells[usageResponsibleQnyPriceResult.Name].Value = dtl.ResponsibilitilyPrice;
                        currEditRow.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                        currEditRow.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    }
                    else if (colName == usageResponsibleQnyPricePercent.Name)
                    {
                        decimal ResponsibilitilyPricePercent = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyPricePercent = ClientUtil.ToDecimal(value);

                        dtl.ResponsiblePricePercent = ResponsibilitilyPricePercent;
                        dtl.ResponsibilitilyPrice = ResponsibilitilyPricePercent * dtl.ResponsibleBasePrice;//数量单价结果

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyPrice * dtl.ResponsibilitilyWorkAmount;

                        currEditRow.Cells[usageResponsibleQnyPriceResult.Name].Value = dtl.ResponsibilitilyPrice;
                        currEditRow.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                        currEditRow.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    }
                }
                else if (OptionCostType == OptCostType.计划成本)
                {
                    if (colName == usagePlanQuotaNum.Name)
                    {
                        decimal PlanQuotaNum = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanQuotaNum = ClientUtil.ToDecimal(value);

                        dtl.PlanQuotaNum = PlanQuotaNum;

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;

                        currEditRow.Cells[usagePlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
                    }
                    else if (colName == usagePlanQuantityPrice.Name)
                    {
                        decimal PlanPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanPrice = ClientUtil.ToDecimal(value);

                        dtl.PlanBasePrice = PlanPrice;
                        dtl.PlanPrice = PlanPrice * dtl.PlanPricePercent;//数量单价结果

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;
                        dtl.PlanTotalPrice = dtl.PlanPrice * dtl.PlanWorkAmount;

                        currEditRow.Cells[usagePlanQnyPriceResult.Name].Value = dtl.PlanPrice;
                        currEditRow.Cells[usagePlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
                        currEditRow.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
                    }
                    else if (colName == usagePlanQnyPricePercent.Name)
                    {
                        decimal PlanPricePercent = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanPricePercent = ClientUtil.ToDecimal(value);

                        dtl.PlanPricePercent = PlanPricePercent;
                        dtl.PlanPrice = PlanPricePercent * dtl.PlanBasePrice;//数量单价结果

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;
                        dtl.PlanTotalPrice = dtl.PlanPrice * dtl.PlanWorkAmount;

                        currEditRow.Cells[usagePlanQnyPriceResult.Name].Value = dtl.PlanPrice;
                        currEditRow.Cells[usagePlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
                        currEditRow.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
                    }
                }
                #endregion

                #region 设置定额数量和数量单价单元格背景颜色（当和成本库不一致的时候显示蓝色）
                if (dtl.ResourceUsageQuota != null)
                {
                    //和成本项定额数量、单价对比
                    if (colName == usageContractQuotaNum.Name)
                    {
                        if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.ContractQuotaQuantity)
                            currEditRow.Cells[usageContractQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        else
                            currEditRow.Cells[usageContractQuotaNum.Name].Style.BackColor = unSelectRowBackColor_White;
                    }
                    else if (colName == usageResponsibleQuotaNum.Name)
                    {
                        if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.ResponsibleQuotaNum)
                            currEditRow.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        else
                            currEditRow.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = unSelectRowBackColor_White;
                    }
                    else if (colName == usagePlanQuotaNum.Name)
                    {
                        if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.PlanQuotaNum)
                            currEditRow.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        else
                            currEditRow.Cells[usagePlanQuotaNum.Name].Style.BackColor = unSelectRowBackColor_White;
                    }
                    else if (colName == usageContractQuantityPrice.Name)//修改基准单价
                    {
                        if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractBasePrice)
                            currEditRow.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        else
                            currEditRow.Cells[usageContractQuantityPrice.Name].Style.BackColor = unSelectRowBackColor_White;
                    }
                    //else if (colName == usageContractQuantityPrice.Name || colName == usageContractQnyPricePercent.Name)
                    //{
                    //    if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractQuantityPrice)
                    //        currEditRow.Cells[usageContractQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
                    //    else
                    //        currEditRow.Cells[usageContractQnyPriceResult.Name].Style.BackColor = unSelectRowBackColor_Control;
                    //}
                    else if (colName == usageResponsibleQuantityPrice.Name)//修改基准单价
                    {
                        if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibleBasePrice)
                            currEditRow.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        else
                            currEditRow.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = unSelectRowBackColor_White;
                    }
                    //else if (colName == usageResponsibleQuantityPrice.Name || colName == usageResponsibleQnyPricePercent.Name)
                    //{
                    //    if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibilitilyPrice)
                    //        currEditRow.Cells[usageResponsibleQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
                    //    else
                    //        currEditRow.Cells[usageResponsibleQnyPriceResult.Name].Style.BackColor = unSelectRowBackColor_Control;
                    //}
                    else if (colName == usagePlanQuantityPrice.Name)//修改基准单价
                    {
                        if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanBasePrice)
                            currEditRow.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        else
                            currEditRow.Cells[usagePlanQuantityPrice.Name].Style.BackColor = unSelectRowBackColor_White;
                    }
                    //else if (colName == usagePlanQuantityPrice.Name || colName == usagePlanQnyPricePercent.Name)
                    //{
                    //    if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanPrice)
                    //        currEditRow.Cells[usagePlanQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
                    //    else
                    //        currEditRow.Cells[usagePlanQnyPriceResult.Name].Style.BackColor = unSelectRowBackColor_Control;
                    //}
                }
                else
                {
                    //和成本项定额数量、单价对比
                    if (colName == usageContractQuotaNum.Name)
                    {
                        currEditRow.Cells[usageContractQuotaNum.Name].Style.BackColor = selectRowBackColor;
                    }
                    else if (colName == usageResponsibleQuotaNum.Name)
                    {
                        currEditRow.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                    }
                    else if (colName == usagePlanQuotaNum.Name)
                    {
                        currEditRow.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                    }
                    else if (colName == usageContractQuantityPrice.Name)//修改基准单价
                    {
                        currEditRow.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                    }
                    else if (colName == usageResponsibleQuantityPrice.Name)//修改基准单价
                    {
                        currEditRow.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                    }
                    else if (colName == usagePlanQuantityPrice.Name)//修改基准单价
                    {
                        currEditRow.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                    }
                }
                #endregion

                currEditRow.Tag = dtl;
            }
        }
        void gridDtlUsage_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
                return;

            GWBSDetailCostSubject dtlUsage = gridDtlUsage.Rows[e.RowIndex].Tag as GWBSDetailCostSubject;
            DataGridViewRow row = gridDtlUsage.Rows[e.RowIndex];
            string colName = gridDtlUsage.Columns[e.ColumnIndex].Name;

            if (colName == usageResourceType.Name || colName == usageSpec.Name)//资源类型
            {
                Material mat = null;
                if (dtlUsage.ResourceUsageQuota != null)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", dtlUsage.ResourceUsageQuota.Id));
                    oq.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);
                    IList list = model.ObjectQuery(typeof(SubjectCostQuota), oq);
                    if (list.Count > 0)
                    {
                        SubjectCostQuota tempSubjectCostQuota = list[0] as SubjectCostQuota;
                        VSelectResourceTypeByUsage frm = new VSelectResourceTypeByUsage();
                        frm.ListResourceGroup = tempSubjectCostQuota.ListResources.ToList();
                        frm.DefaultSelectedMaterilId = dtlUsage.ResourceTypeGUID;
                        frm.ShowDialog();

                        if (frm.isOK)
                        {
                            mat = frm.SelectedMateril;
                        }
                    }
                }
                else
                {
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.OpenSelect();

                    IList list = materialSelector.Result;
                    if (list != null && list.Count > 0)
                    {
                        mat = list[0] as Material;
                    }
                }

                if (mat != null && mat.Id != dtlUsage.ResourceTypeGUID)
                {
                    dtlUsage.ResourceTypeGUID = mat.Id;
                    dtlUsage.ResourceTypeCode = mat.Code;
                    dtlUsage.ResourceTypeName = mat.Name;
                    dtlUsage.ResourceTypeQuality = mat.Quality;
                    dtlUsage.ResourceTypeSpec = mat.Specification;
                    dtlUsage.ResourceCateSyscode = mat.TheSyscode;

                    dtlUsage.IsCategoryResource = mat.IfCatResource == 1;

                    dtlUsage.ProjectAmountUnitGUID = mat.BasicUnit;
                    dtlUsage.ProjectAmountUnitName = mat.BasicUnit != null ? mat.BasicUnit.Name : "";

                    row.Cells[usageResourceType.Name].Value = dtlUsage.ResourceTypeName;
                    row.Cells[usageSpec.Name].Value = dtlUsage.ResourceTypeSpec;
                    row.Cells[usageProjectQuantityUnit.Name].Value = dtlUsage.ProjectAmountUnitName;

                    row.Tag = dtlUsage;

                    if (dtlUsage.MainResTypeFlag)
                    {
                        UpdateTaskDtlMainResource(dtlUsage, false);
                    }
                }
            }
            else if (colName == usageAccountSubject.Name)//核算科目
            {
                VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
                frm.DefaultSelectedAccountSubject = dtlUsage.CostAccountSubjectGUID;
                frm.ShowDialog();
                if (frm.isOK && frm.SelectAccountSubject != null)
                {
                    row.Cells[usageAccountSubject.Name].Value = frm.SelectAccountSubject.Name;
                    dtlUsage.CostAccountSubjectGUID = frm.SelectAccountSubject;
                    dtlUsage.CostAccountSubjectName = dtlUsage.CostAccountSubjectGUID.Name;
                    dtlUsage.CostAccountSubjectSyscode = dtlUsage.CostAccountSubjectGUID.SysCode;
                    row.Tag = dtlUsage;
                }
            }
            else if (colName == usageProjectQuantityUnit.Name)//工程量单位
            {
                StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                if (su != null)
                {
                    dtlUsage.ProjectAmountUnitGUID = su;
                    dtlUsage.ProjectAmountUnitName = su.Name;
                    row.Cells[usageProjectQuantityUnit.Name].Value = dtlUsage.ProjectAmountUnitName;
                    row.Tag = dtlUsage;
                }
            }
        }
        void gridDtlUsage_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    gridDtlUsage.Rows[e.RowIndex].Selected = true;
                    gridDtlUsage.CurrentCell = gridDtlUsage.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    if (copyDtlUsage != null)//复制资源类型
                    {
                        string resourceTypeStr = (string.IsNullOrEmpty(copyDtlUsage.ResourceTypeSpec) ? "" : copyDtlUsage.ResourceTypeSpec + ".") +
                            copyDtlUsage.ResourceTypeName +
                            (string.IsNullOrEmpty(copyDtlUsage.DiagramNumber) ? "" : "." + copyDtlUsage.DiagramNumber);

                        粘贴资源类型到当前行ToolStripMenuItem1.Text = "粘贴资源类型[" + resourceTypeStr + "]到当前行";

                        粘贴资源类型到勾选行ToolStripMenuItem1.Text = "粘贴资源类型[" + resourceTypeStr + "]到勾选行";

                        粘贴资源类型到当前行ToolStripMenuItem1.Enabled = true;
                        粘贴资源类型到勾选行ToolStripMenuItem1.Enabled = true;
                    }
                    else
                    {
                        粘贴资源类型到当前行ToolStripMenuItem1.Text = "粘贴资源类型到当前行";
                        粘贴资源类型到勾选行ToolStripMenuItem1.Text = "粘贴资源类型到勾选行";

                        粘贴资源类型到当前行ToolStripMenuItem1.Enabled = false;
                        粘贴资源类型到勾选行ToolStripMenuItem1.Enabled = false;
                    }

                    复制资源类型ToolStripMenuItem1.Enabled = true;
                    删除资源耗用ToolStripMenuItem1.Enabled = true;
                }
                else
                {
                    删除资源耗用ToolStripMenuItem1.Enabled = false;
                }
            }
        }
        void gridDtlUsage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                复制资源类型ToolStripMenuItem1.Enabled = false;
                粘贴资源类型到当前行ToolStripMenuItem1.Enabled = false;
                粘贴资源类型到勾选行ToolStripMenuItem1.Enabled = copyDtlUsage != null;
                删除资源耗用ToolStripMenuItem1.Enabled = false;

                List<DataGridViewRow> listTaskDtlSelectRows = GetTaskDtlSelectRows();
                if (listTaskDtlSelectRows.Count == 1)
                {
                    addUsageTaskDtl = listTaskDtlSelectRows[0].Tag as GWBSDetail;

                    添加资源耗用ToolStripMenuItem1.Enabled = true;
                }
                else
                {
                    addUsageTaskDtl = null;

                    添加资源耗用ToolStripMenuItem1.Enabled = false;
                }
            }
        }
        void gridDtlUsage_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                string colName = gridDtlUsage.Columns[e.ColumnIndex].Name;
                if (colName == usageSelect.Name)
                {
                    object value = gridDtlUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
                    if (value != null)
                    {
                        bool isSelect = Convert.ToBoolean(value);

                        if (isSelect)
                            gridDtlUsage.Rows[e.RowIndex].Cells[usageSelect.Name].Style.BackColor = selectRowBackColor;
                        else
                            gridDtlUsage.Rows[e.RowIndex].Cells[usageSelect.Name].Style.BackColor = unSelectRowBackColor_White;


                        SetTaskDtlChecked(gridDtlUsage.Rows[e.RowIndex], isSelect);
                    }

                    GetDtlUsageSumQnyAndPrice();
                }
            }
        }

        private bool ValidateMainResourceFlag(GWBSDetailCostSubject dtlUsage)
        {
            List<GWBSDetailCostSubject> listUsage = GetTaskDtlUsageByDtlId(dtlUsage.TheGWBSDetail.Id);
            var queryUsage = from u in listUsage
                             where u.Id != dtlUsage.Id && u.MainResTypeFlag == true
                             select u;

            if (queryUsage.Count() > 0)
            {
                MessageBox.Show("资源耗用“" + queryUsage.ElementAt(0).Name + "”已设置主资源类型，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string whereEditUsages = "";
            foreach (GWBSDetailCostSubject item in listUsage)
            {
                whereEditUsages += "'" + item.Id + "',";
            }
            whereEditUsages = whereEditUsages.Substring(0, whereEditUsages.Length - 1);


            string sql = "select name from thd_gwbsdetailcostsubject t1 where t1.gwbsdetailid='" + dtlUsage.TheGWBSDetail.Id
                     + "' and t1.mainrestypeflag=1 and t1.id not in (" + whereEditUsages + ")";

            DataSet ds = model.SearchSQL(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("资源耗用“" + ds.Tables[0].Rows[0][0].ToString() + "”已设置主资源类型，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SetDtlUsageShowOrRemove(GWBSDetail dtl, bool isShow)
        {
            if (isShow)
            {

            }
            else
            {
                for (int i = gridDtlUsage.Rows.Count - 1; i > -1; i--)
                {
                    DataGridViewRow row = gridDtlUsage.Rows[i];
                    GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                    if (item.TheGWBSDetail.Id == dtl.Id)
                    {
                        gridDtlUsage.Rows.RemoveAt(row.Index);
                    }
                }
            }
        }
        private void SetDtlUsageVisable(GWBSDetail dtl, bool isVisible)
        {
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                if (item.TheGWBSDetail.Id == dtl.Id)
                {
                    row.Visible = isVisible;
                }
            }
        }
        private void SetDtlUsageChecked(GWBSDetail dtl, bool isChecked)
        {
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                if (item.TheGWBSDetail.Id == dtl.Id)
                {
                    row.Cells[usageSelect.Name].Value = isChecked;

                    //gridDtlUsage.CurrentCell = row.Cells[usageSelect.Name];


                    if (isChecked)
                        row.Cells[usageSelect.Name].Style.BackColor = selectRowBackColor;
                    else
                        row.Cells[usageSelect.Name].Style.BackColor = unSelectRowBackColor_White;

                }
            }
        }
        private void SetTaskDtlChecked(DataGridViewRow usageRow, bool isSelect)
        {
            GWBSDetailCostSubject dtlUsage = usageRow.Tag as GWBSDetailCostSubject;
            string dtlId = dtlUsage.TheGWBSDetail.Id;

            bool isChecked = isSelect;//同一任务明细下的耗用是否有选中，如果有则选中所属任务明细，没有则取消所属任务明细的选中

            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                if (row.Index != usageRow.Index && item.TheGWBSDetail.Id == dtlId && Convert.ToBoolean(row.Cells[usageSelect.Name].Value) == true)
                {
                    isChecked = true;
                    break;
                }
            }


            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                GWBSDetail dtl = row.Tag as GWBSDetail;
                if (dtl.Id == dtlId)
                {
                    row.Cells[colTaskDtlSelect.Name].Value = isChecked;

                    if (isChecked)
                        row.Cells[colTaskDtlSelect.Name].Style.BackColor = selectRowBackColor;
                    else
                        row.Cells[colTaskDtlSelect.Name].Style.BackColor = unSelectRowBackColor_White;

                    gridGWBDetail.CurrentCell = row.Cells[colTaskDtlSelect.Name];

                    break;
                }
            }

        }

        void VGWBSDetailCostEdit_Load(object sender, EventArgs e)
        {
            if (DefaultGWBSTreeNode != null)
                txtCurrentPath.Text = DefaultGWBSTreeNode.FullPath;

            SetVisibleColumn();

            this.WindowState = FormWindowState.Maximized;
        }


        //选择工程任务类型
        void btnSelectGWBSBySearch_Click(object sender, EventArgs e)
        {
            VSelectWBSProjectTaskType frm = new VSelectWBSProjectTaskType(new MWBSManagement());
            if (txtTaskTypeNameBySearch.Text.Trim() != "" && txtTaskTypeNameBySearch.Tag != null)
            {
                frm.DefaultSelectedTaskType = txtTaskTypeNameBySearch.Tag as ProjectTaskTypeTree;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                ProjectTaskTypeTree task = root.Tag as ProjectTaskTypeTree;
                if (task != null)
                {
                    txtTaskTypeNameBySearch.Text = task.Name;
                    txtTaskTypeNameBySearch.Tag = task;
                }
            }
        }
        void btnSelectCostItemCate_Click(object sender, EventArgs e)
        {
            VSelectCostItemCategory frm = new VSelectCostItemCategory();
            frm.ShowDialog();
            if (frm.IsOK)
            {
                CostItemCategory cate = frm.SelectCategory;
                txtCostItemCateBySearch.Text = cate.Name;
                txtCostItemCateBySearch.Tag = cate;
            }
        }
        void btnSelectCostItemBySearch_Click(object sender, EventArgs e)
        {
            VSelectCostItem frm = new VSelectCostItem(new MCostItem());
            if (txtCostItemBySearch.Text.Trim() != "" && txtCostItemBySearch.Tag != null)
            {
                frm.DefaultSelectedCostItem = txtCostItemBySearch.Tag as CostItem;
            }
            frm.ShowDialog();
            if (frm.SelectCostItem != null)
            {
                if (!string.IsNullOrEmpty(frm.SelectCostItem.QuotaCode))
                    txtCostItemBySearch.Text = frm.SelectCostItem.QuotaCode;
                else
                    txtCostItemBySearch.Text = frm.SelectCostItem.Name;

                txtCostItemBySearch.Tag = frm.SelectCostItem;
            }
        }
        void btnSelectAccountSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            if (txtAccountSubjectBySearch.Text.Trim() != "" && txtAccountSubjectBySearch.Tag != null)
            {
                frm.DefaultSelectedAccountSubject = txtAccountSubjectBySearch.Tag as CostAccountSubject;
            }
            frm.ShowDialog();
            if (frm.SelectAccountSubject != null)
            {
                txtAccountSubjectBySearch.Text = frm.SelectAccountSubject.Name;
                txtAccountSubjectBySearch.Tag = frm.SelectAccountSubject;
            }
        }
        void btnSelectResourceType_Click(object sender, EventArgs e)
        {
            CommonMaterial frm = new CommonMaterial();
            //if (txtResourceTypeBySearch.Text.Trim() != "" && txtResourceTypeBySearch.Tag != null)
            //{
            //    Material mat = txtResourceTypeBySearch.Tag as Material;
            //    DataSet ds = model.SearchSQL("select t1.code from resmaterialcategory t1 where t1.id='" + mat.Category.Id + "'");
            //    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != null)
            //    {
            //        string cateCode = ds.Tables[0].Rows[0][0].ToString();
            //        frm.materialCatCode = cateCode;
            //    }
            //}
            frm.OpenSelect();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                Material mat = frm.Result[0] as Material;
                txtResourceTypeBySearch.Text = mat.Name + (string.IsNullOrEmpty(mat.Specification) ? "" : "." + mat.Specification);
                txtResourceTypeBySearch.Tag = mat;
            }
        }

        void btnSelectContract_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup());
            if (txtContractBySearch.Text.Trim() != null)
                frm.DefaultSelectedContract = txtContractBySearch.Tag as ContractGroup;
            frm.ShowDialog();
            if (frm.isOK)
            {
                ContractGroup cg = frm.SelectResult[0];
                txtContractBySearch.Text = cg.ContractName;
                txtContractBySearch.Tag = cg;
            }
        }

        void txtCostItemBySearch_LostFocus(object sender, EventArgs e)
        {
            string costItemCode = txtCostItemBySearch.Text.Trim();
            if (string.IsNullOrEmpty(costItemCode))
            {
                txtCostItemBySearch.Tag = null;
                //MessageBox.Show("请输入成本项编号或定额编号，或选择一个成本项！");
                return;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Or(Expression.Eq("Code", costItemCode), Expression.Eq("QuotaCode", costItemCode)));
            IList list = model.ObjectQuery(typeof(CostItem), oq);
            if (list != null && list.Count > 0)
            {
                CostItem item = list[0] as CostItem;
                //txtCostItemBySearch.Text = item.Code;
                txtCostItemBySearch.Tag = item;
            }
            else
            {
                txtCostItemBySearch.Tag = null;
                MessageBox.Show("请检查输入的成本项编号或定额编号，未找到相关成本项！");
            }
        }


        void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbResponsibleAccFlag.Checked == false && cbCostAccFlag.Checked == false && cbSubContractFlag.Checked == false)
                {
                    MessageBox.Show("请勾选任务明细的类型（责任核算明细、成本核算明细、分包取费明细）！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbResponsibleAccFlag.Focus();
                    return;
                }

                FlashScreen.Show("正在查询加载数据,请稍候......");

                gridGWBDetail.Rows.Clear();
                gridDtlUsage.Rows.Clear();
                listDeleteDtlUsages.Clear();

                QueryGWBSDetailInGrid();

                cbTaskDtlSelect.Checked = true;
                cbUsageSelect.Checked = true;
                //cbTaskDtlSelect_CheckedChanged(cbTaskDtlSelect, new EventArgs());

                GetDtlUsageSumQnyAndPrice();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void QueryGWBSDetailInGrid()
        {
            GWBSTree gwbs = null;
            ProjectTaskTypeTree taskType = null;
            CostItemCategory costItemCate = null;
            CostItem costItem = null;
            CostAccountSubject subject = null;
            MaterialCategory matCate = null;
            Material mat = null;
            ContractGroup cg = null;

            if (DefaultGWBSTreeNode != null && DefaultGWBSTreeNode.Tag != null)
                gwbs = DefaultGWBSTreeNode.Tag as GWBSTree;
            if (txtTaskTypeNameBySearch.Text.Trim() != "")
                taskType = txtTaskTypeNameBySearch.Tag as ProjectTaskTypeTree;
            if (txtCostItemCateBySearch.Text.Trim() != "")
                costItemCate = txtCostItemCateBySearch.Tag as CostItemCategory;
            if (txtCostItemBySearch.Text.Trim() != "")
                costItem = txtCostItemBySearch.Tag as CostItem;
            if (txtAccountSubjectBySearch.Text.Trim() != "")
                subject = txtAccountSubjectBySearch.Tag as CostAccountSubject;
            if (txtResourceCateBySearch.Text.Trim() != "" && txtResourceCateBySearch.Result != null && txtResourceCateBySearch.Result.Count > 0)
            {
                matCate = txtResourceCateBySearch.Result[0] as MaterialCategory;
            }
            if (txtResourceTypeBySearch.Text.Trim() != "")
                mat = txtResourceTypeBySearch.Tag as Material;

            if (txtContractBySearch.Text.Trim() != "")
                cg = txtContractBySearch.Tag as ContractGroup;

            string usageName = txtResourceUsageName.Text.Trim();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));

            if (cbStateBySearch.Text.Trim() != "" && cbStateBySearch.SelectedItem != null)
            {
                System.Web.UI.WebControls.ListItem li = cbStateBySearch.SelectedItem as System.Web.UI.WebControls.ListItem;
                int stateValue = Convert.ToInt32(li.Value);
                DocumentState state = (DocumentState)stateValue;
                oq.AddCriterion(Expression.Eq("State", state));
            }
            else
                oq.AddCriterion(Expression.Not(Expression.Eq("State", DocumentState.Invalid)));

            if (gwbs != null)
                oq.AddCriterion(Expression.Like("TheGWBSSysCode", gwbs.SysCode, MatchMode.Start));


            if (cbResponsibleAccFlag.Checked)
                oq.AddCriterion(Expression.Eq("ResponseFlag", 1));//责任核算标记
            if (cbCostAccFlag.Checked)
                oq.AddCriterion(Expression.Eq("CostingFlag", 1));//成本核算标记
            if (cbSubContractFlag.Checked)
                oq.AddCriterion(Expression.Eq("SubContractFeeFlag", true));//分包取费标记


            if (taskType != null)
                oq.AddCriterion(Expression.Eq("ProjectTaskTypeCode", taskType.Code));//代码唯一

            if (costItem != null)
                oq.AddCriterion(Expression.Eq("TheCostItem.Id", costItem.Id));

            if (cg != null)
                oq.AddCriterion(Expression.Eq("ContractGroupGUID", cg.Id));

            if (costItemCate != null)
                oq.AddCriterion(Expression.Like("TheCostItemCateSyscode", costItemCate.SysCode, MatchMode.Start));

            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

            oq.AddOrder(NHibernate.Criterion.Order.Asc("TheGWBSSysCode"));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("ContractGroupName"));
            oq.AddOrder(NHibernate.Criterion.Order.Asc("TheCostItem.Id"));

            IList listDtl = model.GetCostAccDtl(oq, subject, matCate, mat, usageName);

            IEnumerable<GWBSDetail> listTempDtl = (listDtl[0] as IList).OfType<GWBSDetail>();
            IEnumerable<GWBSDetailCostSubject> listTempDtlUsage = (listDtl[1] as IList).OfType<GWBSDetailCostSubject>();

            listTempDtl = from dtl in listTempDtl
                          orderby dtl.OrderNo ascending
                          orderby dtl.TheGWBS.Name ascending
                          select dtl;

            listTempDtlUsage = from dtl in listTempDtlUsage
                               orderby dtl.TheGWBSDetail.Name ascending
                               orderby dtl.TheGWBSTreeName ascending
                               select dtl;

            //加载明细数据
            foreach (GWBSDetail dtl in listTempDtl)
            {
                AddDetailInfoInGrid(dtl, true);
            }
            gridGWBDetail.ClearSelection();

            //加载明细耗用数据
            if (listTempDtlUsage != null)
            {
                foreach (GWBSDetailCostSubject dtl in listTempDtlUsage)
                {
                    AddDetailUsageInfoInGrid(dtl, true);
                }
            }
            gridDtlUsage.ClearSelection();
        }

        private void QueryGWBSDetailInGrid111()
        {

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));

            oq.AddCriterion(Expression.Eq("CostingFlag", 1));//成本核算编辑
            oq.AddCriterion(Expression.Eq("State", DocumentState.Edit));

            if (DefaultGWBSTreeNode != null && DefaultGWBSTreeNode.Tag != null)
                oq.AddCriterion(Expression.Like("TheGWBSSysCode", (DefaultGWBSTreeNode.Tag as GWBSTree).SysCode, MatchMode.Start));

            if (txtTaskTypeNameBySearch.Text.Trim() != "" && txtTaskTypeNameBySearch.Tag != null)
            {
                ProjectTaskTypeTree type = txtTaskTypeNameBySearch.Tag as ProjectTaskTypeTree;
                if (type != null)
                {
                    oq.AddCriterion(Expression.Eq("ProjectTaskTypeCode", type.Code));//代码唯一
                }
            }

            if (txtCostItemBySearch.Text.Trim() != "" && txtCostItemBySearch.Tag != null)
                oq.AddCriterion(Expression.Eq("TheCostItem.Id", (txtCostItemBySearch.Tag as CostItem).Id));

            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

            IList listDtl = model.ObjectQuery(typeof(GWBSDetail), oq);

            IEnumerable<GWBSDetail> listTempDtl = listDtl.OfType<GWBSDetail>().ToList();

            listTempDtl = from dtl in listTempDtl
                          orderby dtl.OrderNo ascending
                          orderby dtl.TheGWBS.Name ascending
                          select dtl;


            Disjunction disUsage = new Disjunction();
            gridGWBDetail.Rows.Clear();
            foreach (GWBSDetail dtl in listTempDtl)
            {
                disUsage.Add(Expression.Eq("TheGWBSDetail.Id", dtl.Id));
            }

            IEnumerable<GWBSDetailCostSubject> listTempDtlUsage = null;
            if (listDtl != null && listDtl.Count > 0)
            {
                oq = new ObjectQuery();
                oq.AddCriterion(disUsage);


                if (txtAccountSubjectBySearch.Text.Trim() != "" && txtAccountSubjectBySearch.Tag != null)
                {
                    CostAccountSubject subject = txtAccountSubjectBySearch.Tag as CostAccountSubject;
                    oq.AddCriterion(Expression.Eq("CostAccountSubjectGUID.Id", subject.Id));
                }

                if (txtResourceTypeBySearch.Text.Trim() != "" && txtResourceTypeBySearch.Tag != null)
                {
                    Material mat = txtResourceTypeBySearch.Tag as Material;
                    oq.AddCriterion(Expression.Eq("ResourceTypeGUID", mat.Id));
                }

                if (txtResourceUsageName.Text.Trim() != "")
                {
                    oq.AddCriterion(Expression.Like("Name", txtResourceUsageName.Text.Trim(), MatchMode.Anywhere));
                }

                oq.AddFetchMode("TheGWBSDetail.WorkAmountUnitGUID", NHibernate.FetchMode.Eager);

                IList listDtlUsage = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);

                listTempDtlUsage = listDtlUsage.OfType<GWBSDetailCostSubject>();
                listTempDtlUsage = from dtl in listTempDtlUsage
                                   orderby dtl.TheGWBSDetail.Name ascending
                                   orderby dtl.TheGWBSDetail.TheGWBS.Name ascending
                                   select dtl;
            }


            //过滤任务明细
            if (listTempDtlUsage == null)
                return;

            var queryGroup = from u in listTempDtlUsage
                             group u by new { u.TheGWBSDetail.Id } into g
                             select g;

            List<GWBSDetail> listDtlResult = listTempDtl.ToList();
            for (int i = listDtlResult.Count - 1; i > -1; i--)
            {
                GWBSDetail dtl = listDtlResult[i];

                var query = from d in queryGroup
                            where d.Key.Id == dtl.Id
                            select d;

                if (query.Count() == 0)
                {
                    listDtlResult.RemoveAt(i);
                }
            }

            //加载明细数据
            foreach (GWBSDetail dtl in listDtlResult)
            {
                AddDetailInfoInGrid(dtl, true);
            }
            gridGWBDetail.ClearSelection();

            //加载明细耗用数据
            if (listTempDtlUsage != null)
            {
                foreach (GWBSDetailCostSubject dtl in listTempDtlUsage)
                {
                    AddDetailUsageInfoInGrid(dtl, true);
                }
            }
            gridDtlUsage.ClearSelection();

        }

        void btnTabContract_Click(object sender, EventArgs e)
        {
            btnTabContract.ForeColor = Color.Blue;
            btnTabResponsible.ForeColor = SystemColors.ControlText;
            btnTabPlan.ForeColor = SystemColors.ControlText;

            OptionCostType = OptCostType.合同收入;

            SetVisibleColumn();

            GetDtlUsageSumQnyAndPrice();
        }
        void btnTabResponsible_Click(object sender, EventArgs e)
        {
            btnTabContract.ForeColor = SystemColors.ControlText;
            btnTabResponsible.ForeColor = Color.Blue;
            btnTabPlan.ForeColor = SystemColors.ControlText;

            OptionCostType = OptCostType.责任成本;

            SetVisibleColumn();

            GetDtlUsageSumQnyAndPrice();
        }
        void btnTabPlan_Click(object sender, EventArgs e)
        {
            btnTabContract.ForeColor = SystemColors.ControlText;
            btnTabResponsible.ForeColor = SystemColors.ControlText;
            btnTabPlan.ForeColor = Color.Blue;

            OptionCostType = OptCostType.计划成本;

            SetVisibleColumn();

            GetDtlUsageSumQnyAndPrice();
        }

        private void SetVisibleColumn()
        {
            bool contractFlag = false;
            bool responsibleFlag = false;
            bool planFlag = false;

            if (OptionCostType == OptCostType.合同收入) contractFlag = true;
            else if (OptionCostType == OptCostType.责任成本) responsibleFlag = true;
            else if (OptionCostType == OptCostType.计划成本) planFlag = true;


            gridGWBDetail.Columns[colContractPrice.Name].Visible = contractFlag;
            //gridGWBDetail.Columns[colContractPricePercent.Name].Visible = contractFlag;
            //gridGWBDetail.Columns[colContractPriceResult.Name].Visible = contractFlag;
            gridGWBDetail.Columns[colContractQuantity.Name].Visible = contractFlag;
            gridGWBDetail.Columns[colContractTotalPrice.Name].Visible = contractFlag;

            gridGWBDetail.Columns[colResponsiblePrice.Name].Visible = responsibleFlag;
            //gridGWBDetail.Columns[colResponsiblePricePercent.Name].Visible = responsibleFlag;
            //gridGWBDetail.Columns[colResponsiblePriceResult.Name].Visible = responsibleFlag;
            gridGWBDetail.Columns[colResponsibleQuantity.Name].Visible = responsibleFlag;
            gridGWBDetail.Columns[colResponsibleTotalPrice.Name].Visible = responsibleFlag;

            gridGWBDetail.Columns[colPlanPrice.Name].Visible = planFlag;
            //gridGWBDetail.Columns[colPlanPricePercent.Name].Visible = planFlag;
            //gridGWBDetail.Columns[colPlanPriceResult.Name].Visible = planFlag;
            gridGWBDetail.Columns[colPlanQuantity.Name].Visible = planFlag;
            gridGWBDetail.Columns[colPlanTotalPrice.Name].Visible = planFlag;



            gridDtlUsage.Columns[usageContractQuotaNum.Name].Visible = contractFlag;
            gridDtlUsage.Columns[usageContractQuantityPrice.Name].Visible = contractFlag;
            gridDtlUsage.Columns[usageContractQnyPricePercent.Name].Visible = contractFlag;
            gridDtlUsage.Columns[usageContractQnyPriceResult.Name].Visible = contractFlag;
            //gridDtlUsage.Columns[usageContractWorkAmountPrice.Name].Visible = contractFlag;
            gridDtlUsage.Columns[usageContractWorkAmount.Name].Visible = contractFlag;
            gridDtlUsage.Columns[usageContractTotalPrice.Name].Visible = contractFlag;

            gridDtlUsage.Columns[usageResponsibleQuotaNum.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[usageResponsibleQuantityPrice.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[usageResponsibleQnyPricePercent.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[usageResponsibleQnyPriceResult.Name].Visible = responsibleFlag;
            //gridDtlUsage.Columns[usageResponsibleWorkAmountPrice.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[usageResponsibleQuantity.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[usageResponsibleTotalPrice.Name].Visible = responsibleFlag;

            gridDtlUsage.Columns[usagePlanQuotaNum.Name].Visible = planFlag;
            gridDtlUsage.Columns[usagePlanQuantityPrice.Name].Visible = planFlag;
            gridDtlUsage.Columns[usagePlanQnyPricePercent.Name].Visible = planFlag;
            gridDtlUsage.Columns[usagePlanQnyPriceResult.Name].Visible = planFlag;
            //gridDtlUsage.Columns[usagePlanWorkAmountPrice.Name].Visible = planFlag;
            gridDtlUsage.Columns[usagePlanQuantity.Name].Visible = planFlag;
            gridDtlUsage.Columns[usagePlanTotalPrice.Name].Visible = planFlag;
        }
        private void SetEnabledEditColumn()
        {
            //设计界面已设置
            //foreach(DataGridViewColumn col in gridGWBDetail.Columns)
            //{

            //}
        }

        //剔除选择任务明细
        void linkRemoveDtl_Click(object sender, EventArgs e)
        {
            for (int i = gridGWBDetail.Rows.Count - 1; i > -1; i--)
            {
                DataGridViewRow row = gridGWBDetail.Rows[i];

                object value = row.Cells[colTaskDtlSelect.Name].Value;
                if (value != null && Convert.ToBoolean(value))
                {
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    if (dtl != null)
                    {
                        for (int j = gridDtlUsage.Rows.Count - 1; j > -1; j--)
                        {
                            DataGridViewRow row1 = gridDtlUsage.Rows[j];
                            GWBSDetailCostSubject sub = row1.Tag as GWBSDetailCostSubject;
                            if (sub.TheGWBSDetail.Id == dtl.Id)
                            {
                                gridDtlUsage.Rows.RemoveAt(j);
                            }
                        }
                    }

                    gridGWBDetail.Rows.RemoveAt(i);
                }
            }
        }
        //剔除选择明细耗用
        void linkRemoveDtlUsage_Click(object sender, EventArgs e)
        {
            for (int i = gridDtlUsage.Rows.Count - 1; i > -1; i--)
            {
                DataGridViewRow row = gridDtlUsage.Rows[i];

                object value = row.Cells[usageSelect.Name].Value;
                if (value != null && Convert.ToBoolean(value))
                {
                    gridDtlUsage.Rows.RemoveAt(i);
                }
            }
            GetDtlUsageSumQnyAndPrice();
        }

        //拷贝计划成本到责任成本
        void btnCopyPlanToResponsible_Click(object sender, EventArgs e)
        {
            //A.对“工程任务明细：”列表框中选中的对象进行如下操作：
            //【责任工程量】=【计划工程量】
            //【责任单价】=【计划单价】
            //【责任合价】=【计划合价】
            //B.对“工程资源耗用明细：”列表框中选中的对象进行如下操作：
            //【责任定额数量】=【计划定额数量】
            //【责任数量单价】=【计划数量单价】
            //【责任工程量单价】=【计划工程量单价】
            //【责任工程量】=【计划工程量】
            //【责任合价】=【计划合价】

            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                if (Convert.ToBoolean(row.Cells[colTaskDtlSelect.Name].Value))
                {
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    dtl.ResponsibilitilyWorkAmount = dtl.PlanWorkAmount;
                    dtl.ResponsibilitilyPrice = dtl.PlanPrice;
                    dtl.ResponsibilitilyTotalPrice = dtl.PlanTotalPrice;

                    row.Cells[colResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
                    row.Cells[colResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    row.Tag = dtl;
                }
            }

            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    dtl.ResponsibleQuotaNum = dtl.PlanQuotaNum;
                    dtl.ResponsibleBasePrice = dtl.PlanBasePrice;
                    dtl.ResponsiblePricePercent = dtl.PlanPricePercent;
                    dtl.ResponsibilitilyPrice = dtl.PlanPrice;
                    dtl.ResponsibleWorkPrice = dtl.PlanWorkPrice;
                    dtl.ResponsibilitilyWorkAmount = dtl.PlanWorkAmount;
                    dtl.ResponsibilitilyTotalPrice = dtl.PlanTotalPrice;

                    row.Cells[usageResponsibleQuotaNum.Name].Value = dtl.ResponsibleQuotaNum;
                    row.Cells[usageResponsibleQuantityPrice.Name].Value = dtl.ResponsibleBasePrice;
                    row.Cells[usageResponsibleQnyPricePercent.Name].Value = dtl.ResponsiblePricePercent;
                    row.Cells[usageResponsibleQnyPriceResult.Name].Value = dtl.ResponsibilitilyPrice;
                    row.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                    row.Cells[usageResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    row.Tag = dtl;
                }
            }

            GetDtlUsageSumQnyAndPrice();

            UpdateDetailUsageQnyPriceBackColor();
        }
        //拷贝计划成本到合同收入
        private void CopyPlanToContractIncome()
        {

            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                if (Convert.ToBoolean(row.Cells[colTaskDtlSelect.Name].Value))
                {
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    dtl.ContractProjectQuantity = dtl.PlanWorkAmount;
                    dtl.ContractPrice = dtl.PlanPrice;
                    dtl.ContractTotalPrice = dtl.PlanTotalPrice;

                    row.Cells[colContractQuantity.Name].Value = dtl.ContractProjectQuantity;
                    row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
                    row.Cells[colContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

                    row.Tag = dtl;
                }
            }

            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    dtl.ContractQuotaQuantity = dtl.PlanQuotaNum;
                    dtl.ContractBasePrice = dtl.PlanBasePrice;
                    dtl.ContractPricePercent = dtl.PlanPricePercent;
                    dtl.ContractQuantityPrice = dtl.PlanPrice;
                    dtl.ContractPrice = dtl.PlanWorkPrice;
                    dtl.ContractProjectAmount = dtl.PlanWorkAmount;
                    dtl.ContractTotalPrice = dtl.PlanTotalPrice;

                    row.Cells[usageContractQuotaNum.Name].Value = dtl.ContractQuotaQuantity;
                    row.Cells[usageContractQuantityPrice.Name].Value = dtl.ContractBasePrice;
                    row.Cells[usageContractQnyPricePercent.Name].Value = dtl.ContractPricePercent;
                    row.Cells[usageContractQnyPriceResult.Name].Value = dtl.ContractQuantityPrice;
                    row.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
                    row.Cells[usageContractWorkAmount.Name].Value = dtl.ContractProjectAmount;
                    row.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

                    row.Tag = dtl;
                }
            }

            GetDtlUsageSumQnyAndPrice();

            UpdateDetailUsageQnyPriceBackColor();
        }
        //拷贝责任成本到计划成本
        void btnCopyResponsibleToPlan_Click(object sender, EventArgs e)
        {

            //A.对“工程任务明细：”列表框中选中的对象进行如下操作：
            //【计划工程量】=【责任工程量】
            //【计划单价】=【责任单价】
            //【计划合价】=【责任合价】
            //B.对“工程资源耗用明细：”列表框中选中的对象进行如下操作：
            //【计划定额数量】=【责任定额数量】
            //【计划数量单价】=【责任数量单价】
            //【计划工程量单价】=【责任工程量单价】
            //【计划工程量】=【责任工程量】
            //【计划合价】=【责任合价】

            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                if (Convert.ToBoolean(row.Cells[colTaskDtlSelect.Name].Value))
                {
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    dtl.PlanWorkAmount = dtl.ResponsibilitilyWorkAmount;
                    dtl.PlanPrice = dtl.ResponsibilitilyPrice;
                    dtl.PlanTotalPrice = dtl.ResponsibilitilyTotalPrice;

                    row.Cells[colPlanQuantity.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
                    row.Cells[colPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

                    row.Tag = dtl;
                }
            }

            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    dtl.PlanQuotaNum = dtl.ResponsibleQuotaNum;
                    dtl.PlanBasePrice = dtl.ResponsibleBasePrice;
                    dtl.PlanPricePercent = dtl.ResponsiblePricePercent;
                    dtl.PlanPrice = dtl.ResponsibilitilyPrice;
                    dtl.PlanWorkPrice = dtl.ResponsibleWorkPrice;
                    dtl.PlanWorkAmount = dtl.ResponsibilitilyWorkAmount;
                    dtl.PlanTotalPrice = dtl.ResponsibilitilyTotalPrice;

                    row.Cells[usagePlanQuotaNum.Name].Value = dtl.PlanQuotaNum;
                    row.Cells[usagePlanQuantityPrice.Name].Value = dtl.PlanBasePrice;
                    row.Cells[usagePlanQnyPricePercent.Name].Value = dtl.PlanPricePercent;
                    row.Cells[usagePlanQnyPriceResult.Name].Value = dtl.PlanPrice;
                    row.Cells[usagePlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
                    row.Cells[usagePlanQuantity.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

                    row.Tag = dtl;
                }
            }

            GetDtlUsageSumQnyAndPrice();

            UpdateDetailUsageQnyPriceBackColor();
        }

        //成本数据计算
        void btnAccountCostData_Click(object sender, EventArgs e)
        {
            bool flag = false;//是否勾选任务明细或资源耗用
            bool flag2 = false;//是否选择了有效的任务明细
            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                if (Convert.ToBoolean(row.Cells[colTaskDtlSelect.Name].Value))
                {
                    flag = true;

                    //GWBSDetail dtl = row.Tag as GWBSDetail;
                    //if (dtl.SubContractFeeFlag == false)
                    //    flag2 = true;
                }

                if (flag)//&& flag2
                    break;
            }

            if (flag == false)
            {
                MessageBox.Show("请勾选任务明细或资源耗用！");
                gridGWBDetail.Focus();
                return;
            }
            //if (flag2 == false)
            //{
            //    MessageBox.Show("勾选的任务明细不存在“责任核算明细”或“成本核算明细”,请检查！");
            //    gridGWBDetail.Focus();
            //    return;
            //}

            AccountCostData();
        }

        private void AccountCostData()
        {
            //计算耗用数据
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                bool isSelect = false;
                object value = row.Cells[usageSelect.Name].EditedFormattedValue;
                if (value != null)
                {
                    isSelect = Convert.ToBoolean(value);
                }

                if (isSelect == false)
                    continue;

                GWBSDetailCostSubject dtlUsage = row.Tag as GWBSDetailCostSubject;

                GWBSDetail taskDtl = getTaskDtl(dtlUsage);
                if (taskDtl == null)
                    taskDtl = dtlUsage.TheGWBSDetail;

                //if (taskDtl.SubContractFeeFlag == true)//成本计算时不计算分包取费明细的预算量（算法不一致）
                //    continue;

                if (OptionCostType == OptCostType.合同收入)
                {
                    //i）如果【成本核算科目】为“人工费”，则【资源合同工程量单价】等于所属{工程任务明细}_【合同单价】减去其余兄弟{工程资源耗用明细}_【资源合同工程量单价】之和；
                    //ii）【资源合同工程量】：=所属{工程任务明细}_【合同工程量】*【合同定额数量】；
                    //iii）【资源合同合价】：=【资源合同工程量】*【资源合同工程量单价】；

                    //if (!string.IsNullOrEmpty(dtlUsage.CostAccountSubjectName) && dtlUsage.CostAccountSubjectName.Trim() == "人工费")
                    //{
                    //    decimal brotherProjectAmountPrice = 0;//兄弟合同工程量单价之和
                    //    foreach (DataGridViewRow rowTemp in gridDtlUsage.Rows)
                    //    {
                    //        GWBSDetailCostSubject tempDtl = rowTemp.Tag as GWBSDetailCostSubject;
                    //        if (tempDtl.TheGWBSDetail.Id == taskDtl.Id && tempDtl.Id != dtlUsage.Id)
                    //        {
                    //            brotherProjectAmountPrice += tempDtl.ContractPrice;
                    //            //brotherProjectAmountPrice += tempDtl.ContractQuotaQuantity * tempDtl.ContractQuantityPrice;
                    //        }
                    //    }

                    //    dtlUsage.ContractPrice = taskDtl.ContractPrice - brotherProjectAmountPrice;
                    //}

                    dtlUsage.ContractProjectAmount = taskDtl.ContractProjectQuantity * dtlUsage.ContractQuotaQuantity;
                    dtlUsage.ContractTotalPrice = dtlUsage.ContractProjectAmount * dtlUsage.ContractQuantityPrice;

                    //row.Cells[usageContractWorkAmountPrice.Name].Value = dtlUsage.ContractPrice;
                    row.Cells[usageContractWorkAmount.Name].Value = dtlUsage.ContractProjectAmount;
                    row.Cells[usageContractTotalPrice.Name].Value = dtlUsage.ContractTotalPrice;
                }
                else if (OptionCostType == OptCostType.责任成本)
                {
                    //a）针对<操作{工程资源耗用明细}集>在列表框中显示的每一个对象进行如下操作：
                    //【责任耗用数量】：=所属{工程任务明细}_【责任工程量】*【责任定额数量】；
                    //【责任耗用合价】：=【责任耗用数量】*【责任工程量单价】；

                    dtlUsage.ResponsibilitilyWorkAmount = taskDtl.ResponsibilitilyWorkAmount * dtlUsage.ResponsibleQuotaNum;
                    dtlUsage.ResponsibilitilyTotalPrice = dtlUsage.ResponsibilitilyWorkAmount * dtlUsage.ResponsibilitilyPrice;

                    row.Cells[usageResponsibleQuantity.Name].Value = dtlUsage.ResponsibilitilyWorkAmount;
                    row.Cells[usageResponsibleTotalPrice.Name].Value = dtlUsage.ResponsibilitilyTotalPrice;
                }
                else if (OptionCostType == OptCostType.计划成本)
                {
                    //a）针对<操作{工程资源耗用明细}集>在列表框中显示的每一个对象进行如下操作：
                    //【计划耗用数量】：=所属{工程任务明细}_【计划工程量】*【计划定额数量】；
                    //【计划耗用合价】：=【计划耗用数量】*【计划工程量单价】；

                    dtlUsage.PlanWorkAmount = taskDtl.PlanWorkAmount * dtlUsage.PlanQuotaNum;
                    dtlUsage.PlanTotalPrice = dtlUsage.PlanWorkAmount * dtlUsage.PlanPrice;

                    row.Cells[usagePlanQuantity.Name].Value = dtlUsage.PlanWorkAmount;
                    row.Cells[usagePlanTotalPrice.Name].Value = dtlUsage.PlanTotalPrice;
                }

                row.Tag = dtlUsage;

            }

            //计算任务明细数据
            List<GWBSDetail> listTempDtl = new List<GWBSDetail>();//存任务明细除耗用表格里以外的耗用信息，与表格中的耗用数据一起统计任务明细的单价信息
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                bool isSelect = false;
                object value = row.Cells[colTaskDtlSelect.Name].EditedFormattedValue;
                if (value != null)
                {
                    isSelect = Convert.ToBoolean(value);
                }
                if (isSelect == false)
                    continue;

                GWBSDetail dtl = row.Tag as GWBSDetail;
                List<GWBSDetailCostSubject> listTempUsage = new List<GWBSDetailCostSubject>();//耗用集
                listRowIndex.Add(row.Index);

                if (OptionCostType == OptCostType.合同收入)
                {
                    //【合同单价】：=下属{工程资源耗用明细}_【合同工程量单价】
                    //【合同合价】：=【合同工程量】*【合同单价】

                    decimal dtlUsageProjectAmountPrice = 0;
                    foreach (DataGridViewRow rowTemp in gridDtlUsage.Rows)
                    {
                        GWBSDetailCostSubject subject = rowTemp.Tag as GWBSDetailCostSubject;
                        if (subject.TheGWBSDetail.Id == dtl.Id)
                        {
                            dtlUsageProjectAmountPrice += subject.ContractPrice;

                            GWBSDetailCostSubject tempSubject = new GWBSDetailCostSubject();
                            tempSubject.Id = subject.Id;
                            listTempUsage.Add(tempSubject);
                        }
                    }
                    dtl.ContractPrice = dtlUsageProjectAmountPrice;
                    dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;

                    row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
                    row.Cells[colContractTotalPrice.Name].Value = dtl.ContractTotalPrice;
                }
                else if (OptionCostType == OptCostType.责任成本)
                {
                    //【责任单价】：=下属{工程资源耗用明细}_【责任工程量单价】
                    //【责任合价】：=【责任工程量】*【责任单价】

                    decimal dtlUsageProjectAmountPrice = 0;
                    foreach (DataGridViewRow rowTemp in gridDtlUsage.Rows)
                    {
                        GWBSDetailCostSubject subject = rowTemp.Tag as GWBSDetailCostSubject;
                        if (subject.TheGWBSDetail.Id == dtl.Id)
                        {
                            dtlUsageProjectAmountPrice += subject.ResponsibleWorkPrice;

                            GWBSDetailCostSubject tempSubject = new GWBSDetailCostSubject();
                            tempSubject.Id = subject.Id;
                            listTempUsage.Add(tempSubject);
                        }
                    }
                    dtl.ResponsibilitilyPrice = dtlUsageProjectAmountPrice;
                    dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                    row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
                    row.Cells[colResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;
                }
                else if (OptionCostType == OptCostType.计划成本)
                {
                    //【计划单价】：=下属{工程资源耗用明细}_【计划工程量单价】
                    //【计划合价】：=【计划工程量】*【计划单价】

                    decimal dtlUsageProjectAmountPrice = 0;
                    foreach (DataGridViewRow rowTemp in gridDtlUsage.Rows)
                    {
                        GWBSDetailCostSubject subject = rowTemp.Tag as GWBSDetailCostSubject;
                        if (subject.TheGWBSDetail.Id == dtl.Id)
                        {
                            dtlUsageProjectAmountPrice += subject.PlanWorkPrice;

                            GWBSDetailCostSubject tempSubject = new GWBSDetailCostSubject();
                            tempSubject.Id = subject.Id;
                            listTempUsage.Add(tempSubject);
                        }
                    }
                    dtl.PlanPrice = dtlUsageProjectAmountPrice;
                    dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                    row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
                    row.Cells[colPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
                }

                GWBSDetail tempDtl = new GWBSDetail();
                tempDtl.Id = dtl.Id;
                tempDtl.ListCostSubjectDetails = new HashedSet<GWBSDetailCostSubject>();
                tempDtl.ListCostSubjectDetails.AddAll(listTempUsage);
                listTempDtl.Add(tempDtl);

                row.Tag = dtl;
            }

            if (listTempDtl.Count > 0)
            {
                listTempDtl = model.AccountTaskDtlPrice(listTempDtl);
                foreach (int rowIndex in listRowIndex)
                {
                    DataGridViewRow row = gridGWBDetail.Rows[rowIndex];
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    var query = from d in listTempDtl
                                where d.Id == dtl.Id
                                select d;
                    if (query.Count() > 0)
                    {
                        GWBSDetail tempDtl = query.ElementAt(0);
                        if (OptionCostType == OptCostType.合同收入)
                        {
                            dtl.ContractPrice += tempDtl.ContractPrice;
                            dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;
                            row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
                            row.Cells[colContractTotalPrice.Name].Value = dtl.ContractTotalPrice;
                        }
                        else if (OptionCostType == OptCostType.责任成本)
                        {
                            dtl.ResponsibilitilyPrice += tempDtl.ResponsibilitilyPrice;
                            dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;
                            row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
                            row.Cells[colResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;
                        }
                        else if (OptionCostType == OptCostType.计划成本)
                        {
                            dtl.PlanPrice += tempDtl.PlanPrice;
                            dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;
                            row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
                            row.Cells[colPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
                        }
                        row.Tag = dtl;
                    }
                }
            }

            GetDtlUsageSumQnyAndPrice();
        }

        private GWBSDetail getTaskDtl(GWBSDetailCostSubject subject)
        {
            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                GWBSDetail dtl = row.Tag as GWBSDetail;
                if (dtl.Id == subject.TheGWBSDetail.Id)
                {
                    return dtl;
                }
            }
            return null;
        }

        //分包取费计算
        void btnSubcontractFeeAcc_Click(object sender, EventArgs e)
        {
            bool flag = false;//是否勾选任务明细或资源耗用

            IList listSubContractFeeDtl = new ArrayList();
            IList listSubContractFeeDtlUsage = new ArrayList();

            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                GWBSDetail dtl = row.Tag as GWBSDetail;
                if (Convert.ToBoolean(row.Cells[colTaskDtlSelect.Name].Value) && dtl.SubContractFeeFlag == true)
                {
                    flag = true;

                    if (dtl.ResponseFlag == 1 || dtl.CostingFlag == 1)//只有标记了要计算的类型（责任明细或成本明细）的分包取费明细才有效
                    {
                        bool flag3 = false;//判断是否有耗用（可能被踢出剔除）
                        foreach (DataGridViewRow rowUsage in gridDtlUsage.Rows)
                        {
                            GWBSDetailCostSubject usage = rowUsage.Tag as GWBSDetailCostSubject;
                            if (usage.TheGWBSDetail.Id == dtl.Id)
                            {
                                flag3 = true;

                                listSubContractFeeDtlUsage.Add(usage);
                            }
                        }
                        if (flag3)
                        {
                            listSubContractFeeDtl.Add(dtl);
                        }
                    }
                }
            }

            if (flag == false)
            {
                MessageBox.Show("请勾选分包取费明细！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridGWBDetail.Focus();
                return;
            }
            if (listSubContractFeeDtl.Count == 0)
            {
                MessageBox.Show("选择的分包取费任务明细中数据不完整（未标记“责任核算明细”或“成本核算明细”）,请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridGWBDetail.Focus();
                return;
            }

            try
            {
                FlashScreen.Show("正在计算,请稍候......");
                DocumentState state = DocumentState.Edit;
                if (cbStateBySearch.Text.Trim() != "" && cbStateBySearch.SelectedItem != null)
                {
                    System.Web.UI.WebControls.ListItem li = cbStateBySearch.SelectedItem as System.Web.UI.WebControls.ListItem;
                    int stateValue = Convert.ToInt32(li.Value);
                    state = (DocumentState)stateValue;
                }
                else
                    state = DocumentState.Invalid;
                IList listResult = model.AccountSubContractFeeDtl(listSubContractFeeDtl, listSubContractFeeDtlUsage, state);
                listSubContractFeeDtl = listResult[0] as IList;
                listSubContractFeeDtlUsage = listResult[1] as IList;

                foreach (GWBSDetail dtl in listSubContractFeeDtl)
                {
                    UpdateDetailInfoInGrid(dtl);
                }

                foreach (GWBSDetailCostSubject item in listSubContractFeeDtlUsage)
                {
                    UpdateDetailUsageInfoInGrid(item);
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                FlashScreen.Close();
            }

            GetDtlUsageSumQnyAndPrice();

        }

        //批量输入单价（无效）
        void btnBatchInputPrice_Click(object sender, EventArgs e)
        {
            //VGWBSDetailCostEditInputPrice frm = new VGWBSDetailCostEditInputPrice(OptionCostType);
            //frm.ShowDialog();
            //decimal priceValue = frm.PriceValue;
            //decimal quantityPriceValue = frm.QuantityPriceValue;
            //decimal projectAmountPriceValue = frm.ProjectAmountPriceValue;

            //if (priceValue != 0)
            //{
            //    foreach (DataGridViewRow row in gridGWBDetail.Rows)
            //    {
            //        if (Convert.ToBoolean(row.Cells[colTaskDtlSelect.Name].Value))
            //        {
            //            GWBSDetail dtl = row.Tag as GWBSDetail;

            //            if (OptionCostType == OptCostType.合同收入)
            //            {
            //                dtl.ContractPrice = priceValue;

            //                row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
            //            }
            //            else if (OptionCostType == OptCostType.责任成本)
            //            {
            //                dtl.ResponsibilitilyPrice = priceValue;

            //                row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
            //            }
            //            else if (OptionCostType == OptCostType.计划成本)
            //            {
            //                dtl.PlanPrice = priceValue;

            //                row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
            //            }

            //            row.Tag = dtl;
            //        }
            //    }
            //}

            //if (quantityPriceValue != 0)
            //{
            //    foreach (DataGridViewRow row in gridDtlUsage.Rows)
            //    {
            //        if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
            //        {
            //            GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;

            //            if (OptionCostType == OptCostType.合同收入)
            //            {
            //                subject.ContractQuantityPrice = quantityPriceValue;
            //                subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;

            //                row.Cells[usageContractQuantityPrice.Name].Value = subject.ContractQuantityPrice;
            //                row.Cells[usageContractWorkAmountPrice.Name].Value = subject.ContractPrice;
            //            }
            //            else if (OptionCostType == OptCostType.责任成本)
            //            {
            //                subject.ResponsibilitilyPrice = quantityPriceValue;
            //                subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;

            //                row.Cells[usageResponsibleQuantityPrice.Name].Value = subject.ResponsibilitilyPrice;
            //                row.Cells[usageResponsibleWorkAmountPrice.Name].Value = subject.ResponsibleWorkPrice;
            //            }
            //            else if (OptionCostType == OptCostType.计划成本)
            //            {
            //                subject.PlanPrice = quantityPriceValue;
            //                subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;

            //                row.Cells[usagePlanQuantityPrice.Name].Value = subject.PlanPrice;
            //                row.Cells[usagePlanWorkAmountPrice.Name].Value = subject.PlanWorkPrice;
            //            }

            //            row.Tag = subject;
            //        }
            //    }
            //}
            //if (projectAmountPriceValue != 0)
            //{
            //    //修改后联动计算【责任数量单价】=【责任工程量单价】，【责任定额数量】=1，【数量计量单位】=所属{工程任务明细}_【工程量计量单位】,合同收入和计划成本算法相似
            //    foreach (DataGridViewRow row in gridDtlUsage.Rows)
            //    {
            //        if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
            //        {
            //            GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;

            //            if (OptionCostType == OptCostType.合同收入)
            //            {
            //                subject.ContractPrice = projectAmountPriceValue;
            //                subject.ContractQuantityPrice = subject.ContractPrice;
            //                subject.ContractQuotaQuantity = 1;

            //                subject.ProjectAmountUnitGUID = subject.TheGWBSDetail.WorkAmountUnitGUID;
            //                subject.ProjectAmountUnitName = subject.TheGWBSDetail.WorkAmountUnitName;

            //                row.Cells[usageContractWorkAmountPrice.Name].Value = subject.ContractPrice;
            //                row.Cells[usageContractQuantityPrice.Name].Value = subject.ContractQuantityPrice;
            //                row.Cells[usageContractQuotaNum.Name].Value = subject.ContractQuotaQuantity;
            //            }
            //            else if (OptionCostType == OptCostType.责任成本)
            //            {
            //                subject.ResponsibleWorkPrice = projectAmountPriceValue;
            //                subject.ResponsibilitilyPrice = subject.ResponsibleWorkPrice;
            //                subject.ResponsibleQuotaNum = 1;

            //                subject.ProjectAmountUnitGUID = subject.TheGWBSDetail.WorkAmountUnitGUID;
            //                subject.ProjectAmountUnitName = subject.TheGWBSDetail.WorkAmountUnitName;

            //                row.Cells[usageResponsibleWorkAmountPrice.Name].Value = subject.ResponsibleWorkPrice;
            //                row.Cells[usageResponsibleQuantityPrice.Name].Value = subject.ResponsibilitilyPrice;
            //                row.Cells[usageResponsibleQuotaNum.Name].Value = subject.ResponsibleQuotaNum;
            //            }
            //            else if (OptionCostType == OptCostType.计划成本)
            //            {
            //                subject.PlanWorkPrice = projectAmountPriceValue;
            //                subject.PlanPrice = subject.PlanWorkPrice;
            //                subject.PlanQuotaNum = 1;

            //                subject.ProjectAmountUnitGUID = subject.TheGWBSDetail.WorkAmountUnitGUID;
            //                subject.ProjectAmountUnitName = subject.TheGWBSDetail.WorkAmountUnitName;

            //                row.Cells[usagePlanWorkAmountPrice.Name].Value = subject.PlanWorkPrice;
            //                row.Cells[usagePlanQuantityPrice.Name].Value = subject.PlanPrice;
            //                row.Cells[usagePlanQuotaNum.Name].Value = subject.PlanQuotaNum;
            //            }

            //            row.Tag = subject;
            //        }
            //    }
            //}
        }
        //选择契约组
        void btnSelectContractGroup_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtContractGroupName.Tag != null)
            {
                frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
            }
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                txtContractGroupName.Text = cg.ContractName;
                txtContractGroupName.Tag = cg;
                txtContractGroupType.Text = cg.ContractGroupType;
            }
        }
        private bool valideContractGroup()
        {
            if (txtContractGroupName.Text.Trim() == "" || txtContractGroupName.Tag == null)
            {
                MessageBox.Show("保存的任务明细中有状态为“执行中”的任务明细，请选择变更契约！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
                if (txtContractGroupName.Tag != null)
                {
                    frm.DefaultSelectedContract = txtContractGroupName.Tag as ContractGroup;
                }
                frm.ShowDialog();
                if (frm.SelectResult.Count > 0)
                {
                    ContractGroup cg = frm.SelectResult[0] as ContractGroup;
                    txtContractGroupName.Text = cg.ContractName;
                    txtContractGroupType.Text = cg.ContractGroupType;
                    txtContractGroupName.Tag = cg;

                    return true;
                }
                return false;
            }
            return true;
        }
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateSave())
            {
                MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //保存并退出
        void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            if (ValidateSave())
                this.Close();
        }
        private bool ValidateSave()
        {
            try
            {
                #region 数据校验
                bool flag = false;//是否有发布状态的任务明细
                foreach (DataGridViewRow row in gridGWBDetail.Rows)
                {
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    if (dtl.State == DocumentState.InExecute)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag && !valideContractGroup())
                {
                    return false;
                }

                foreach (DataGridViewRow row in gridDtlUsage.Rows)
                {
                    GWBSDetailCostSubject dtlUsage = row.Tag as GWBSDetailCostSubject;
                    if (dtlUsage.CostAccountSubjectGUID == null)
                    {
                        MessageBox.Show("请选择(" + (row.Index + 1) + "行)资源耗用“" + dtlUsage.Name + "”的核算科目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (row.Visible)
                            gridDtlUsage.CurrentCell = row.Cells[usageAccountSubject.Name];
                        return false;
                    }
                    else if (OptionCostType == OptCostType.合同收入 && dtlUsage.ContractQuotaQuantity == 0)
                    {
                        MessageBox.Show("请输入(" + (row.Index + 1) + "行)资源耗用“" + dtlUsage.Name + "”的合同定额数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (row.Visible)
                            gridDtlUsage.CurrentCell = row.Cells[usageContractQuotaNum.Name];
                        return false;
                    }
                    else if (OptionCostType == OptCostType.责任成本 && dtlUsage.ResponsibleQuotaNum == 0)
                    {
                        MessageBox.Show("请输入(" + (row.Index + 1) + "行)资源耗用“" + dtlUsage.Name + "”的责任定额数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (row.Visible)
                            gridDtlUsage.CurrentCell = row.Cells[usageResponsibleQuotaNum.Name];
                        return false;
                    }
                    else if (OptionCostType == OptCostType.计划成本 && dtlUsage.PlanQuotaNum == 0)
                    {
                        MessageBox.Show("请输入(" + (row.Index + 1) + "行)资源耗用“" + dtlUsage.Name + "”的计划定额数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (row.Visible)
                            gridDtlUsage.CurrentCell = row.Cells[usagePlanQuotaNum.Name];
                        return false;
                    }
                }
                #endregion

                AccountCostData();

                IList listDtl = new ArrayList();
                IList listDtlUsage = new ArrayList();

                ObjectQuery oq = new ObjectQuery();
                Disjunction dis = new Disjunction();
                foreach (DataGridViewRow row in gridGWBDetail.Rows)
                {
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    dis.Add(Expression.Eq("Id", dtl.Id));
                }
                oq.AddCriterion(dis);
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                listDtl = model.ObjectQuery(typeof(GWBSDetail), oq);
                oq.Criterions.Clear();

                IList listLedger = null;
                for (int i = 0; i < listDtl.Count; i++)
                {
                    GWBSDetail dtl = listDtl[i] as GWBSDetail;
                    foreach (DataGridViewRow row in gridGWBDetail.Rows)
                    {
                        GWBSDetail currDtl = row.Tag as GWBSDetail;
                        if (dtl.Id == currDtl.Id)
                        {
                            //记录变更台帐信息
                            if (dtl.State == DocumentState.InExecute)
                            {
                                if (listLedger == null)
                                    listLedger = new ArrayList();

                                #region 记录明细台账
                                if (dtl.ContractProjectQuantity != currDtl.ContractProjectQuantity ||
                                    dtl.ResponsibilitilyWorkAmount != currDtl.ResponsibilitilyWorkAmount ||
                                    dtl.PlanWorkAmount != currDtl.PlanWorkAmount)
                                {
                                    GWBSDetailLedger led = new GWBSDetailLedger();

                                    led.ProjectTaskID = dtl.TheGWBS.Id;
                                    led.ProjectTaskName = dtl.TheGWBS.Name;
                                    led.TheProjectTaskSysCode = dtl.TheGWBS.SysCode;

                                    led.ProjectTaskDtlID = dtl.Id;
                                    led.ProjectTaskDtlName = dtl.Name;

                                    if (dtl.ContractProjectQuantity != currDtl.ContractProjectQuantity)
                                    {
                                        led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入工程量变化;
                                        led.ContractWorkAmount = currDtl.ContractProjectQuantity - dtl.ContractProjectQuantity;
                                        led.ContractPrice = dtl.ContractPrice;
                                        led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;
                                    }
                                    else
                                    {
                                        led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;
                                        led.ContractWorkAmount = 0;
                                        led.ContractPrice = 0;
                                        led.ContractTotalPrice = 0;
                                    }

                                    if (dtl.ResponsibilitilyWorkAmount != currDtl.ResponsibilitilyWorkAmount)
                                    {
                                        led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任工程量变化;
                                        led.ResponsibleWorkAmount = currDtl.ResponsibilitilyWorkAmount - dtl.ResponsibilitilyWorkAmount;
                                        led.ResponsiblePrice = dtl.ResponsibilitilyPrice;
                                        led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;
                                    }
                                    else
                                    {
                                        led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;
                                        led.ResponsiblePrice = 0;
                                        led.ResponsibleWorkAmount = 0;
                                        led.ResponsibleTotalPrice = 0;
                                    }


                                    if (dtl.PlanWorkAmount != currDtl.PlanWorkAmount)
                                    {
                                        led.PlanCostChangeMode = PlanCostChangeModeEnum.计划工程量变化;
                                        led.PlanWorkAmount = currDtl.PlanWorkAmount - dtl.PlanWorkAmount;
                                        led.PlanPrice = dtl.PlanPrice;
                                        led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;
                                    }
                                    else
                                    {
                                        led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;
                                        led.PlanWorkAmount = 0;
                                        led.PlanPrice = 0;
                                        led.PlanTotalPrice = 0;
                                    }

                                    led.WorkAmountUnit = dtl.WorkAmountUnitGUID;
                                    led.WorkAmountUnitName = dtl.WorkAmountUnitName;
                                    led.PriceUnit = dtl.PriceUnitGUID;
                                    led.PriceUnitName = dtl.PriceUnitName;
                                    led.TheContractGroup = txtContractGroupName.Tag as ContractGroup;
                                    led.TheProjectGUID = dtl.TheProjectGUID;
                                    led.TheProjectName = dtl.TheProjectName;

                                    listLedger.Add(led);
                                }

                                dtl.ContractProjectQuantity = currDtl.ContractProjectQuantity;
                                dtl.ResponsibilitilyWorkAmount = currDtl.ResponsibilitilyWorkAmount;
                                dtl.PlanWorkAmount = currDtl.PlanWorkAmount;

                                if (dtl.ContractPrice != currDtl.ContractPrice ||
                                     dtl.ResponsibilitilyPrice != currDtl.ResponsibilitilyPrice ||
                                     dtl.PlanPrice != currDtl.PlanPrice)
                                {
                                    GWBSDetailLedger led = new GWBSDetailLedger();

                                    led.ProjectTaskID = dtl.TheGWBS.Id;
                                    led.ProjectTaskName = dtl.TheGWBS.Name;
                                    led.TheProjectTaskSysCode = dtl.TheGWBS.SysCode;

                                    led.ProjectTaskDtlID = dtl.Id;
                                    led.ProjectTaskDtlName = dtl.Name;

                                    if (dtl.ContractPrice != currDtl.ContractPrice)
                                    {
                                        led.ContractChangeMode = ContractIncomeChangeModeEnum.合同单价变化;
                                        led.ContractPrice = currDtl.ContractPrice - dtl.ContractPrice;
                                        led.ContractWorkAmount = dtl.ContractProjectQuantity;
                                        led.ContractTotalPrice = led.ContractWorkAmount * led.ContractPrice;
                                    }
                                    else
                                    {
                                        led.ContractChangeMode = ContractIncomeChangeModeEnum.合同收入无变化;
                                        led.ContractWorkAmount = 0;
                                        led.ContractPrice = 0;
                                        led.ContractTotalPrice = 0;
                                    }

                                    if (dtl.ResponsibilitilyPrice != currDtl.ResponsibilitilyPrice)
                                    {
                                        led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任单价变化;
                                        led.ResponsiblePrice = currDtl.ResponsibilitilyPrice - dtl.ResponsibilitilyPrice;
                                        led.ResponsibleWorkAmount = dtl.ResponsibilitilyWorkAmount;
                                        led.ResponsibleTotalPrice = led.ResponsibleWorkAmount * led.ResponsiblePrice;
                                    }
                                    else
                                    {
                                        led.ResponsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本无变化;
                                        led.ResponsiblePrice = 0;
                                        led.ResponsibleWorkAmount = 0;
                                        led.ResponsibleTotalPrice = 0;
                                    }

                                    if (dtl.PlanPrice != currDtl.PlanPrice)
                                    {
                                        led.PlanCostChangeMode = PlanCostChangeModeEnum.计划单价变化;
                                        led.PlanPrice = currDtl.PlanPrice - dtl.PlanPrice;
                                        led.PlanWorkAmount = dtl.PlanWorkAmount;
                                        led.PlanTotalPrice = led.PlanWorkAmount * led.PlanPrice;
                                    }
                                    else
                                    {
                                        led.PlanCostChangeMode = PlanCostChangeModeEnum.计划成本无变化;
                                        led.PlanWorkAmount = 0;
                                        led.PlanPrice = 0;
                                        led.PlanTotalPrice = 0;
                                    }

                                    led.WorkAmountUnit = dtl.WorkAmountUnitGUID;
                                    led.WorkAmountUnitName = dtl.WorkAmountUnitName;
                                    led.PriceUnit = dtl.PriceUnitGUID;
                                    led.PriceUnitName = dtl.PriceUnitName;
                                    led.TheContractGroup = txtContractGroupName.Tag as ContractGroup;
                                    led.TheProjectGUID = dtl.TheProjectGUID;
                                    led.TheProjectName = dtl.TheProjectName;

                                    listLedger.Add(led);
                                }
                                #endregion
                            }
                            else
                            {
                                dtl.ContractProjectQuantity = currDtl.ContractProjectQuantity;
                                dtl.ResponsibilitilyWorkAmount = currDtl.ResponsibilitilyWorkAmount;
                                dtl.PlanWorkAmount = currDtl.PlanWorkAmount;
                            }

                            dtl.MainResourceTypeId = currDtl.MainResourceTypeId;
                            dtl.MainResourceTypeName = currDtl.MainResourceTypeName;
                            dtl.MainResourceTypeQuality = currDtl.MainResourceTypeQuality;
                            dtl.MainResourceTypeSpec = currDtl.MainResourceTypeSpec;
                            dtl.DiagramNumber = currDtl.DiagramNumber;

                            dtl.ContractPrice = currDtl.ContractPrice;
                            dtl.ContractTotalPrice = currDtl.ContractTotalPrice;

                            dtl.ResponsibilitilyPrice = currDtl.ResponsibilitilyPrice;
                            dtl.ResponsibilitilyTotalPrice = currDtl.ResponsibilitilyTotalPrice;

                            dtl.PlanPrice = currDtl.PlanPrice;
                            dtl.PlanTotalPrice = currDtl.PlanTotalPrice;

                            dtl.ResponseFlag = currDtl.ResponseFlag;
                            dtl.CostingFlag = currDtl.CostingFlag;

                            break;
                        }
                    }
                }


                foreach (DataGridViewRow row in gridDtlUsage.Rows)
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    if (!string.IsNullOrEmpty(dtl.Id))
                        dis.Add(Expression.Eq("Id", dtl.Id));
                }
                oq.AddCriterion(dis);
                oq.AddFetchMode("ResourceUsageQuota", NHibernate.FetchMode.Eager);
                listDtlUsage = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);

                foreach (DataGridViewRow row in gridDtlUsage.Rows)
                {
                    GWBSDetailCostSubject currDtl = row.Tag as GWBSDetailCostSubject;
                    if (string.IsNullOrEmpty(currDtl.Id))
                        listDtlUsage.Add(currDtl);
                    else
                    {
                        for (int i = 0; i < listDtlUsage.Count; i++)
                        {
                            GWBSDetailCostSubject dtl = listDtlUsage[i] as GWBSDetailCostSubject;
                            if (dtl.Id == currDtl.Id)
                            {
                                dtl.Name = currDtl.Name;
                                dtl.MainResTypeFlag = currDtl.MainResTypeFlag;
                                dtl.IsCategoryResource = currDtl.IsCategoryResource;
                                dtl.ResourceTypeGUID = currDtl.ResourceTypeGUID;
                                dtl.ResourceTypeCode = currDtl.ResourceTypeCode;
                                dtl.ResourceTypeName = currDtl.ResourceTypeName;
                                dtl.ResourceTypeQuality = currDtl.ResourceTypeQuality;
                                dtl.ResourceTypeSpec = currDtl.ResourceTypeSpec;
                                dtl.ResourceCateSyscode = currDtl.ResourceCateSyscode;
                                dtl.DiagramNumber = currDtl.DiagramNumber;

                                dtl.CostAccountSubjectGUID = currDtl.CostAccountSubjectGUID;
                                dtl.CostAccountSubjectName = currDtl.CostAccountSubjectName;
                                dtl.CostAccountSubjectSyscode = currDtl.CostAccountSubjectSyscode;

                                dtl.ContractQuotaQuantity = currDtl.ContractQuotaQuantity;
                                dtl.ContractBasePrice = currDtl.ContractBasePrice;
                                dtl.ContractPricePercent = currDtl.ContractPricePercent;
                                dtl.ContractQuantityPrice = currDtl.ContractBasePrice * currDtl.ContractPricePercent;
                                dtl.ContractPrice = currDtl.ContractPrice;
                                dtl.ContractProjectAmount = currDtl.ContractProjectAmount;
                                dtl.ContractTotalPrice = currDtl.ContractTotalPrice;

                                dtl.ResponsibleQuotaNum = currDtl.ResponsibleQuotaNum;
                                dtl.ResponsibleBasePrice = currDtl.ResponsibleBasePrice;
                                dtl.ResponsiblePricePercent = currDtl.ResponsiblePricePercent;
                                dtl.ResponsibilitilyPrice = currDtl.ResponsibleBasePrice * currDtl.ResponsiblePricePercent;
                                dtl.ResponsibleWorkPrice = currDtl.ResponsibleWorkPrice;
                                dtl.ResponsibilitilyWorkAmount = currDtl.ResponsibilitilyWorkAmount;
                                dtl.ResponsibilitilyTotalPrice = currDtl.ResponsibilitilyTotalPrice;

                                dtl.PlanQuotaNum = currDtl.PlanQuotaNum;
                                dtl.PlanBasePrice = currDtl.PlanBasePrice;
                                dtl.PlanPricePercent = currDtl.PlanPricePercent;
                                dtl.PlanPrice = currDtl.PlanBasePrice * currDtl.PlanPricePercent;
                                dtl.PlanWorkPrice = currDtl.PlanWorkPrice;
                                dtl.PlanWorkAmount = currDtl.PlanWorkAmount;
                                dtl.PlanTotalPrice = currDtl.PlanTotalPrice;

                                dtl.ProjectAmountUnitGUID = currDtl.ProjectAmountUnitGUID;
                                dtl.ProjectAmountUnitName = currDtl.ProjectAmountUnitName;
                                break;
                            }
                        }
                    }
                }

                FlashScreen.Show("正在保存,请稍候......");

                IList listResult = model.SaveOrUpdateDetailByCostEdit(listDtl, listLedger, listDtlUsage, listDeleteDtlUsages);
                listDtl = listResult[0] as IList;
                listDtlUsage = listResult[1] as IList;

                //更新datagridview

                IEnumerable<GWBSDetail> listDtlNew = listDtl.OfType<GWBSDetail>();
                foreach (DataGridViewRow row in gridGWBDetail.Rows)
                {
                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    var query = from d in listDtlNew
                                where d.Id == dtl.Id
                                select d;
                    if (query.Count() > 0)
                        dtl = query.ElementAt(0);
                    row.Tag = dtl;
                }

                IEnumerable<GWBSDetailCostSubject> listDtlUsageNew = listDtlUsage.OfType<GWBSDetailCostSubject>();
                foreach (DataGridViewRow row in gridDtlUsage.Rows)
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;

                    var query = from d in listDtlUsageNew
                                where d.Id == dtl.Id
                                select d;
                    if (query.Count() > 0)
                        dtl = query.ElementAt(0);

                    row.Tag = dtl;
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
            finally
            {
                FlashScreen.Close();
            }

            return true;
        }
        //放弃
        void btnGiveup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddDetailInfoInGrid(GWBSDetail dtl, bool isSelected)
        {
            int index = gridGWBDetail.Rows.Add();
            DataGridViewRow row = gridGWBDetail.Rows[index];

            row.Cells[colProjectTaskName.Name].Value = dtl.TheGWBS.Name;
            row.Cells[colProjectTaskName.Name].ToolTipText = dtl.TheGWBS.FullPath;// Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBS);

            row.Cells[colProjectTaskDtlName.Name].Value = dtl.Name;

            row.Cells[colResponsibleAccFlag.Name].Value = dtl.ResponseFlag == 1 ? "是" : "否";
            row.Cells[colCostAccFlag.Name].Value = dtl.CostingFlag == 1 ? "是" : "否";
            row.Cells[colSubContractFlag.Name].Value = dtl.SubContractFeeFlag == true ? "是" : "否";

            row.Cells[colContractName.Name].Value = dtl.ContractGroupName;
            if (dtl.TheCostItem != null)
            {
                row.Cells[colCostItemName.Name].Value = dtl.TheCostItem.Name;
                row.Cells[colCostItemQuotaCode.Name].Value = dtl.TheCostItem.QuotaCode;
            }
            row.Cells[colMainResourceName.Name].Value = dtl.MainResourceTypeName;
            row.Cells[colSpec.Name].Value = dtl.MainResourceTypeSpec;
            row.Cells[colDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Cells[colContractQuantity.Name].Value = dtl.ContractProjectQuantity;
            row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
            //row.Cells[colContractPricePercent.Name].Value = dtl.ContractPricePercent;
            //row.Cells[colContractPriceResult.Name].Value = dtl.ContractPrice * dtl.ContractPricePercent;
            row.Cells[colContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

            row.Cells[colResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
            //row.Cells[colResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
            //row.Cells[colResponsiblePriceResult.Name].Value = dtl.ResponsibilitilyPrice * dtl.ResponsiblePricePercent;
            row.Cells[colResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[colPlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
            //row.Cells[colPlanPricePercent.Name].Value = dtl.PlanPricePercent;
            //row.Cells[colPlanPriceResult.Name].Value = dtl.PlanPrice * dtl.PlanPricePercent;
            row.Cells[colPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

            row.Cells[colQuantityUnit.Name].Value = dtl.WorkAmountUnitName;
            row.Cells[colPriceUnit.Name].Value = dtl.PriceUnitName;

            //if (dtl.TheCostItem != null)
            //    row.Cells[colCostItemBasePrice.Name].Value = dtl.TheCostItem.Price;

            row.Cells[colTaskDtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);

            row.Tag = dtl;

            if (isSelected)
            {
                row.Cells[colTaskDtlSelect.Name].Value = true;
                row.Cells[colTaskDtlSelect.Name].Style.BackColor = selectRowBackColor;
            }
        }
        private void UpdateDetailInfoInGrid(GWBSDetail dtl)
        {
            for (int i = 0; i < gridGWBDetail.Rows.Count; i++)
            {
                DataGridViewRow row = gridGWBDetail.Rows[i];
                GWBSDetail d = row.Tag as GWBSDetail;
                if (d.Id == dtl.Id)
                {
                    row.Cells[colProjectTaskName.Name].Value = dtl.TheGWBS.Name;
                    row.Cells[colProjectTaskName.Name].ToolTipText = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBS);

                    row.Cells[colProjectTaskDtlName.Name].Value = dtl.Name;

                    row.Cells[colResponsibleAccFlag.Name].Value = dtl.ResponseFlag == 1 ? "是" : "否";
                    row.Cells[colCostAccFlag.Name].Value = dtl.CostingFlag == 1 ? "是" : "否";
                    row.Cells[colSubContractFlag.Name].Value = dtl.SubContractFeeFlag == true ? "是" : "否";

                    row.Cells[colContractName.Name].Value = dtl.ContractGroupName;
                    if (dtl.TheCostItem != null)
                    {
                        row.Cells[colCostItemName.Name].Value = dtl.TheCostItem.Name;
                        row.Cells[colCostItemQuotaCode.Name].Value = dtl.TheCostItem.QuotaCode;
                    }
                    row.Cells[colMainResourceName.Name].Value = dtl.MainResourceTypeName;
                    row.Cells[colSpec.Name].Value = dtl.MainResourceTypeSpec;
                    row.Cells[colDiagramNumber.Name].Value = dtl.DiagramNumber;

                    row.Cells[colContractQuantity.Name].Value = dtl.ContractProjectQuantity;
                    row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
                    //row.Cells[colContractPricePercent.Name].Value = dtl.ContractPricePercent;
                    //row.Cells[colContractPriceResult.Name].Value = dtl.ContractPrice * dtl.ContractPricePercent;
                    row.Cells[colContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

                    row.Cells[colResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
                    //row.Cells[colResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
                    //row.Cells[colResponsiblePriceResult.Name].Value = dtl.ResponsibilitilyPrice * dtl.ResponsiblePricePercent;
                    row.Cells[colResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    row.Cells[colPlanQuantity.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
                    //row.Cells[colPlanPricePercent.Name].Value = dtl.PlanPricePercent;
                    //row.Cells[colPlanPriceResult.Name].Value = dtl.PlanPrice * dtl.PlanPricePercent;
                    row.Cells[colPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

                    row.Cells[colQuantityUnit.Name].Value = dtl.WorkAmountUnitName;
                    row.Cells[colPriceUnit.Name].Value = dtl.PriceUnitName;

                    //if (dtl.TheCostItem != null)
                    //    row.Cells[colCostItemBasePrice.Name].Value = dtl.TheCostItem.Price;

                    row.Cells[colTaskDtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);

                    row.Tag = dtl;

                    gridGWBDetail.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void AddDetailUsageInfoInGrid(GWBSDetailCostSubject dtl, bool isSelected)
        {
            int index = gridDtlUsage.Rows.Add();
            DataGridViewRow row = gridDtlUsage.Rows[index];
            if (string.IsNullOrEmpty(dtl.Name))
            {
                row.Cells[usageName.Name].Value = "耗用名称";
            }
            else
            {
                row.Cells[usageName.Name].Value = dtl.Name;
            }
            row.Cells[usageResourceType.Name].Value = dtl.ResourceTypeName;
            row.Cells[usageSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[usageDiagramNumber.Name].Value = dtl.DiagramNumber;
            row.Cells[usageIsMainResource.Name].Value = dtl.MainResTypeFlag ? "是" : "否";
            row.Cells[usageAccountSubject.Name].Value = dtl.CostAccountSubjectName;

            row.Cells[usageProjectQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;

            row.Cells[usageContractQuotaNum.Name].Value = dtl.ContractQuotaQuantity;
            row.Cells[usageContractQuantityPrice.Name].Value = dtl.ContractBasePrice;
            row.Cells[usageContractQnyPricePercent.Name].Value = dtl.ContractPricePercent;
            row.Cells[usageContractQnyPriceResult.Name].Value = dtl.ContractQuantityPrice;
            row.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
            row.Cells[usageContractWorkAmount.Name].Value = dtl.ContractProjectAmount;
            row.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

            row.Cells[usageResponsibleQuotaNum.Name].Value = dtl.ResponsibleQuotaNum;
            row.Cells[usageResponsibleQuantityPrice.Name].Value = dtl.ResponsibleBasePrice;
            row.Cells[usageResponsibleQnyPricePercent.Name].Value = dtl.ResponsiblePricePercent;
            row.Cells[usageResponsibleQnyPriceResult.Name].Value = dtl.ResponsibilitilyPrice;
            row.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
            row.Cells[usageResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[usagePlanQuotaNum.Name].Value = dtl.PlanQuotaNum;
            row.Cells[usagePlanQuantityPrice.Name].Value = dtl.PlanBasePrice;
            row.Cells[usagePlanQnyPricePercent.Name].Value = dtl.PlanPricePercent;
            row.Cells[usagePlanQnyPriceResult.Name].Value = dtl.PlanPrice;
            row.Cells[usagePlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
            row.Cells[usagePlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

            if (dtl.ResourceUsageQuota != null)
            {
                row.Cells[usageCostItemQuotaQuantity.Name].Value = dtl.ResourceUsageQuota.QuotaProjectAmount;
                row.Cells[usageCostItemQuotaPrice.Name].Value = dtl.ResourceUsageQuota.QuotaPrice;

                if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.ContractQuotaQuantity)
                    row.Cells[usageContractQuotaNum.Name].Style.BackColor = selectRowBackColor;
                if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractBasePrice)
                    row.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                //if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractQuantityPrice)
                //    row.Cells[usageContractQnyPriceResult.Name].Style.BackColor = selectRowBackColor;

                if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.ResponsibleQuotaNum)
                    row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibleBasePrice)
                    row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                //if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibilitilyPrice)
                //    row.Cells[usageResponsibleQnyPriceResult.Name].Style.BackColor = selectRowBackColor;

                if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.PlanQuotaNum)
                    row.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanBasePrice)
                    row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                //if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanPrice)
                //    row.Cells[usagePlanQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
            }
            else
            {
                row.Cells[usageContractQuotaNum.Name].Style.BackColor = selectRowBackColor;
                row.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                row.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
            }

            row.Tag = dtl;

            if (isSelected)
            {
                row.Cells[usageSelect.Name].Value = true;
                row.Cells[usageSelect.Name].Style.BackColor = selectRowBackColor;
            }
        }
        private void UpdateDetailUsageInfoInGrid(GWBSDetailCostSubject dtl)
        {
            for (int i = 0; i < gridDtlUsage.Rows.Count; i++)
            {
                DataGridViewRow row = gridDtlUsage.Rows[i];
                GWBSDetailCostSubject d = row.Tag as GWBSDetailCostSubject;
                if (d.Id == dtl.Id)
                {
                    row.Cells[usageName.Name].Value = dtl.Name;
                    row.Cells[usageResourceType.Name].Value = dtl.ResourceTypeName;
                    row.Cells[usageSpec.Name].Value = dtl.ResourceTypeSpec;
                    row.Cells[usageDiagramNumber.Name].Value = dtl.DiagramNumber;
                    row.Cells[usageIsMainResource.Name].Value = dtl.MainResTypeFlag ? "是" : "否";
                    row.Cells[usageAccountSubject.Name].Value = dtl.CostAccountSubjectName;

                    row.Cells[usageProjectQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;

                    row.Cells[usageContractQuotaNum.Name].Value = dtl.ContractQuotaQuantity;
                    row.Cells[usageContractQuantityPrice.Name].Value = dtl.ContractBasePrice;
                    row.Cells[usageContractQnyPricePercent.Name].Value = dtl.ContractPricePercent;
                    row.Cells[usageContractQnyPriceResult.Name].Value = dtl.ContractQuantityPrice;
                    row.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
                    row.Cells[usageContractWorkAmount.Name].Value = dtl.ContractProjectAmount;
                    row.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

                    row.Cells[usageResponsibleQuotaNum.Name].Value = dtl.ResponsibleQuotaNum;
                    row.Cells[usageResponsibleQuantityPrice.Name].Value = dtl.ResponsibleBasePrice;
                    row.Cells[usageResponsibleQnyPricePercent.Name].Value = dtl.ResponsiblePricePercent;
                    row.Cells[usageResponsibleQnyPriceResult.Name].Value = dtl.ResponsibilitilyPrice;
                    row.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                    row.Cells[usageResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    row.Cells[usagePlanQuotaNum.Name].Value = dtl.PlanQuotaNum;
                    row.Cells[usagePlanQuantityPrice.Name].Value = dtl.PlanBasePrice;
                    row.Cells[usagePlanQnyPricePercent.Name].Value = dtl.PlanPricePercent;
                    row.Cells[usagePlanQnyPriceResult.Name].Value = dtl.PlanPrice;
                    row.Cells[usagePlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
                    row.Cells[usagePlanQuantity.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

                    if (dtl.ResourceUsageQuota != null)
                    {
                        row.Cells[usageCostItemQuotaQuantity.Name].Value = dtl.ResourceUsageQuota.QuotaProjectAmount;
                        row.Cells[usageCostItemQuotaPrice.Name].Value = dtl.ResourceUsageQuota.QuotaPrice;


                        if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.ContractQuotaQuantity)
                            row.Cells[usageContractQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usageContractQuotaNum.Name].Style.BackColor = unSelectRowBackColor_White;

                        if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractBasePrice)
                            row.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usageContractQuantityPrice.Name].Style.BackColor = unSelectRowBackColor_White;

                        //if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractQuantityPrice)
                        //    row.Cells[usageContractQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
                        //else
                        //    row.Cells[usageContractQnyPriceResult.Name].Style.BackColor = unSelectRowBackColor_Control;


                        if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.ResponsibleQuotaNum)
                            row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = unSelectRowBackColor_White;

                        if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibleBasePrice)
                            row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = unSelectRowBackColor_White;

                        //if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibilitilyPrice)
                        //    row.Cells[usageResponsibleQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
                        //else
                        //    row.Cells[usageResponsibleQnyPriceResult.Name].Style.BackColor = unSelectRowBackColor_Control;


                        if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.PlanQuotaNum)
                            row.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usagePlanQuotaNum.Name].Style.BackColor = unSelectRowBackColor_White;


                        if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanBasePrice)
                            row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = unSelectRowBackColor_White;

                        //if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanPrice)
                        //    row.Cells[usagePlanQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
                        //else
                        //    row.Cells[usagePlanQnyPriceResult.Name].Style.BackColor = unSelectRowBackColor_Control;
                    }
                    else
                    {
                        row.Cells[usageContractQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        row.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        row.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                    }

                    row.Tag = dtl;
                }
            }

        }
        private void UpdateDetailUsageQnyPriceBackColor()
        {
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;

                if (dtl.ResourceUsageQuota != null)
                {
                    if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractBasePrice)
                        row.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                    else
                        row.Cells[usageContractQuantityPrice.Name].Style.BackColor = unSelectRowBackColor_White;
                    //if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractQuantityPrice)
                    //    row.Cells[usageContractQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
                    //else
                    //    row.Cells[usageContractQnyPriceResult.Name].Style.BackColor = unSelectRowBackColor_Control;

                    if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibleBasePrice)
                        row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                    else
                        row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = unSelectRowBackColor_White;
                    //if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibilitilyPrice)
                    //    row.Cells[usageResponsibleQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
                    //else
                    //    row.Cells[usageResponsibleQnyPriceResult.Name].Style.BackColor = unSelectRowBackColor_Control;

                    if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanBasePrice)
                        row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                    else
                        row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = unSelectRowBackColor_White;
                    //if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanPrice)
                    //    row.Cells[usagePlanQnyPriceResult.Name].Style.BackColor = selectRowBackColor;
                    //else
                    //    row.Cells[usagePlanQnyPriceResult.Name].Style.BackColor = unSelectRowBackColor_Control;
                }
                else
                {
                    row.Cells[usageContractQuotaNum.Name].Style.BackColor = selectRowBackColor;
                    row.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                    row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                    row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                    row.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                    row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                }
            }
        }

        private void UpdateTaskDtlMainResource(GWBSDetailCostSubject dtl, bool deleteFlag)
        {
            DataGridViewRow rowDtl = GetTaskDtlByDtlUsage(dtl);
            if (rowDtl != null)
            {
                GWBSDetail taskDtl = rowDtl.Tag as GWBSDetail;

                taskDtl.MainResourceTypeId = deleteFlag ? null : dtl.ResourceTypeGUID;
                taskDtl.MainResourceTypeName = deleteFlag ? null : dtl.ResourceTypeName;
                taskDtl.MainResourceTypeQuality = deleteFlag ? null : dtl.ResourceTypeQuality;
                taskDtl.MainResourceTypeSpec = deleteFlag ? null : dtl.ResourceTypeSpec;
                taskDtl.DiagramNumber = deleteFlag ? null : dtl.DiagramNumber;

                rowDtl.Cells[colMainResourceName.Name].Value = taskDtl.MainResourceTypeName;
                rowDtl.Cells[colSpec.Name].Value = taskDtl.MainResourceTypeSpec;
                rowDtl.Cells[colDiagramNumber.Name].Value = taskDtl.DiagramNumber;

                rowDtl.Tag = taskDtl;
            }
        }
        private void UpdateTaskDtlDiagramNumber(GWBSDetailCostSubject dtl)
        {
            DataGridViewRow rowDtl = GetTaskDtlByDtlUsage(dtl);
            if (rowDtl != null)
            {
                GWBSDetail taskDtl = rowDtl.Tag as GWBSDetail;

                taskDtl.DiagramNumber = dtl.DiagramNumber;

                rowDtl.Cells[colDiagramNumber.Name].Value = dtl.DiagramNumber;

                rowDtl.Tag = taskDtl;
            }
        }
        private DataGridViewRow GetTaskDtlByDtlUsage(GWBSDetailCostSubject dtlUsage)
        {
            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                GWBSDetail dtl = row.Tag as GWBSDetail;
                if (dtl.Id == dtlUsage.TheGWBSDetail.Id)
                    return row;
            }
            return null;
        }
        private List<GWBSDetailCostSubject> GetTaskDtlUsageByDtlId(string dtlId)
        {
            List<GWBSDetailCostSubject> listDtlUsage = new List<GWBSDetailCostSubject>();
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                if (dtl.TheGWBSDetail.Id == dtlId)
                    listDtlUsage.Add(dtl);
            }
            return listDtlUsage;
        }

        private List<DataGridViewRow> GetTaskDtlSelectRows()
        {
            List<DataGridViewRow> listTaskDtlSelectRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                bool isSelect = false;
                object value = row.Cells[colTaskDtlSelect.Name].EditedFormattedValue;
                if (value != null)
                {
                    isSelect = Convert.ToBoolean(value);
                }

                if (isSelect)
                    listTaskDtlSelectRows.Add(row);

                if (listTaskDtlSelectRows.Count > 1)
                    break;

            }
            return listTaskDtlSelectRows;
        }
        /// <summary>
        /// 获取明细耗用选择行的耗用总量和总价
        /// </summary>
        /// <returns>（1.总量，2.总价）</returns>
        private void GetDtlUsageSumQnyAndPrice()
        {
            decimal sumQny = 0;
            decimal sumPrice = 0;

            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                bool isSelect = false;
                object value = row.Cells[usageSelect.Name].EditedFormattedValue;
                if (value != null)
                {
                    isSelect = Convert.ToBoolean(value);
                }

                if (isSelect)
                {
                    GWBSDetailCostSubject dtlUsage = row.Tag as GWBSDetailCostSubject;
                    if (OptionCostType == OptCostType.合同收入)
                    {
                        sumQny += dtlUsage.ContractProjectAmount;
                        sumPrice += dtlUsage.ContractTotalPrice;
                    }
                    else if (OptionCostType == OptCostType.责任成本)
                    {
                        sumQny += dtlUsage.ResponsibilitilyWorkAmount;
                        sumPrice += dtlUsage.ResponsibilitilyTotalPrice;
                    }
                    else if (OptionCostType == OptCostType.计划成本)
                    {
                        sumQny += dtlUsage.PlanWorkAmount;
                        sumPrice += dtlUsage.PlanTotalPrice;
                    }
                }
            }

            txtUsageQuantity.Text = string.Format("{0:N}", sumQny);
            txtUsageTotalPrice.Text = string.Format("{0:N}", sumPrice);
        }
    }
}
