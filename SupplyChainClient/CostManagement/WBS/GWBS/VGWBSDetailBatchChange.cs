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
    public partial class VGWBSDetailBatchChange : TBasicDataView
    {
        /// <summary>
        /// 初始任务类型
        /// </summary>
        public TreeNode DefaultGWBSTreeNode = null;
        private GWBSTree oGWBSTreeNode = null;
        private bool IsUpdated = false;
        public GWBSTree GWBSTreeNode
        {
            get
            {
                if (oGWBSTreeNode == null)
                {
                    if (DefaultGWBSTreeNode != null && DefaultGWBSTreeNode.Tag != null)
                    {
                        oGWBSTreeNode= DefaultGWBSTreeNode.Tag as GWBSTree;
                        ObjectQuery oQuery = new ObjectQuery();
                        oQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                        oQuery.AddCriterion(Expression.Eq("Id", oGWBSTreeNode.Id)); 
                        IList lst = model.ObjectQuery(typeof(GWBSTree), oQuery);
                        if (lst != null && lst.Count > 0)
                        {
                            oGWBSTreeNode = lst[0] as GWBSTree;
                        }
                    }
                }
                return oGWBSTreeNode;
            }
        }
        CurrentProjectInfo projectInfo = null;

        private OptCostType OptionCostType = OptCostType.合同收入;

        Color selectRowBackColor = ColorTranslator.FromHtml("#D7E8FE");//淡蓝色
        Color unSelectRowBackColor_Control = ColorTranslator.FromHtml("#ECE9D8");//控件颜色
        Color unSelectRowBackColor_White = ColorTranslator.FromHtml("#FFFFFF");//白颜色

        private List<string> listDeleteDtlUsages = new List<string>();
        private GWBSDetail addUsageTaskDtl = null;//要添加耗用的任务明细
        private GWBSDetailCostSubject copyDtlUsage = null;//被复制的资源耗用

        private MGWBSTree model = new MGWBSTree();

        public VGWBSDetailBatchChange()
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
            this.btnQuery_Click(null,null);
            this.tabControl1.TabPages.Remove(this.tabPageResource);
            SetButton();
        }
        private void InitEvents()
        {
           // btnSelectGWBS.Click += new EventHandler(btnSelectGWBS_Click);
            txtCostItemBySearch.LostFocus += new EventHandler(txtCostItemBySearch_LostFocus);
            btnSelectCostItemCate.Click += new EventHandler(btnSelectCostItemCate_Click);
            btnSelectCostItemBySearch.Click += new EventHandler(btnSelectCostItemBySearch_Click);
            btnSelectAccountSubject.Click += new EventHandler(btnSelectAccountSubject_Click);
            btnSelectContract.Click += new EventHandler(btnSelectContract_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);

            this.Load += new EventHandler(VGWBSDetailBatchChange_Load);
            btnChange.Click+=new EventHandler(btnChange_Click);
            chkAllSelect.CheckedChanged+=new EventHandler(chkAllSelect_CheckedChanged);
            chkUnSelect.CheckedChanged+=new EventHandler(chkUnSelect_CheckedChanged);
            chkAllSelectRes.CheckedChanged+=new EventHandler(chkAllSelectRes_CheckedChanged);
            chkUnSelectRes.CheckedChanged+=new EventHandler(chkUnSelectRes_CheckedChanged);
            chkAllSelectUse.CheckedChanged+=new EventHandler(chkAllSelectUse_CheckedChanged);
            chkUnSelectUse.CheckedChanged+=new EventHandler(chkUnSelectUse_CheckedChanged);
            btnBack.Click+=new EventHandler(btnBack_Click);
            btnResourceSet.Click+=new EventHandler(btnResourceSet_Click);
            btnSave.Click+=new EventHandler(btnSave_Click);
            btnSaveAndExit.Click+=new EventHandler(btnSaveAndExit_Click);
            this.FormClosing += new FormClosingEventHandler(VGWBSDetailBatchChange_FormClosing);
            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);
            btnTabContract.Click += new EventHandler(btnTabContract_Click);
            btnTabResponsible.Click += new EventHandler(btnTabResponsible_Click);
            btnTabPlan.Click += new EventHandler(btnTabPlan_Click);

            #region 工程wbs明细变更
            this.gridGWBDetailRes.CellContentClick+=new DataGridViewCellEventHandler(gridGWBDetailRes_CellContentClick);
           this.gridGWBDetailRes.CellMouseDown+=new DataGridViewCellMouseEventHandler(gridGWBDetailRes_CellMouseDown);
            this.gridGWBDetailRes.CellEndEdit+=new DataGridViewCellEventHandler(gridGWBDetailRes_CellEndEdit);
            this.gridGWBDetailRes.CellValidating += new DataGridViewCellValidatingEventHandler(gridGWBDetailRes_CellValidating);
  
            #endregion
            #region 消耗
            this.gridDtlUsage.CellContentClick+=new DataGridViewCellEventHandler(gridDtlUsage_CellContentClick);
            this.gridDtlUsage.CellMouseDown+=new DataGridViewCellMouseEventHandler(gridDtlUsage_CellMouseDown);
            this.gridDtlUsage.CellEndEdit+=new DataGridViewCellEventHandler(gridDtlUsage_CellEndEdit);
            this.gridDtlUsage.CellValidating+=new DataGridViewCellValidatingEventHandler(gridDtlUsage_CellValidating);
            gridDtlUsage.MouseDown += new MouseEventHandler(gridDtlUsage_MouseDown);
            btnAccountCostData.Click += new EventHandler(btnAccountCostData_Click);
           // btnSubcontractFeeAcc.Click += new EventHandler(btnSubcontractFeeAcc_Click);
            #endregion

            contextMenuQueryElsePriceDtl.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuQueryElsePriceDtl_ItemClicked);
            contextMenuQueryElsePriceDtlUsage.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuQueryElsePriceDtlUsage_ItemClicked);

        }
        void contextMenuQueryElsePriceDtl_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuQueryElsePriceDtl.Hide();

            if (e.ClickedItem.Name == 查询其他项目单价ToolStripMenuItem.Name)
            {
                #region 查询其他项目单价

                GWBSDetail optDtl = gridGWBDetailRes.Rows[gridGWBDetailRes.CurrentCell.RowIndex].Tag as GWBSDetail;
                #region 不能查询 因为有可能该wbs明细无法保存
                ObjectQuery oq = new ObjectQuery();
                //oq.AddCriterion(Expression.Eq("Id", optDtl.Id));
                //oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                //optDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;
                //oq.Criterions.Clear();
                //oq.FetchModes.Clear();
                #endregion

                oq.AddCriterion(Expression.Not(Expression.Eq("TheProjectGUID", projectInfo.Id)));
                oq.AddCriterion(Expression.Eq("TheCostItem.Id", optDtl.TheCostItem.Id));
                oq.AddCriterion(Expression.Eq("MainResourceTypeId", optDtl.MainResourceTypeId));
                oq.AddCriterion(Expression.Eq("State", DocumentState.InExecute));

                IList listElsePriceDtl = model.ObjectQuery(typeof(GWBSDetail), oq);
                IList lstTemp = this.gridGWBDetailRes.Rows.OfType<DataGridViewRow>().Select(a => a.Tag as GWBSDetail).Where(a =>
                    (string.IsNullOrEmpty(a.Id) && a.TheCostItem == optDtl.TheCostItem && a.MainResourceTypeId == optDtl.MainResourceTypeId && a.State == DocumentState.InExecute)).ToList();
                if (lstTemp != null && lstTemp.Count > 0)
                {
                    foreach (GWBSDetail obj in lstTemp)
                    {
                        listElsePriceDtl.Add(obj);
                    }
                }
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
                    foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[colTaskDtlSelectRes.Name].Value))
                        {
                            GWBSDetail dtl = row.Tag as GWBSDetail;

                            if (OptionCostType == OptCostType.合同收入)
                            {
                                dtl.ContractPrice = priceValue;

                                row.Cells[colContractPriceRes.Name].Value = dtl.ContractPrice;
                            }
                            else if (OptionCostType == OptCostType.责任成本)
                            {
                                dtl.ResponsibilitilyPrice = priceValue;

                                row.Cells[colResponsiblePriceRes.Name].Value = dtl.ResponsibilitilyPrice;
                            }
                            else if (OptionCostType == OptCostType.计划成本)
                            {
                                dtl.PlanPrice = priceValue;

                                row.Cells[colPlanPriceRes.Name].Value = dtl.PlanPrice;
                            }

                            row.Tag = dtl;
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[colTaskDtlSelectRes.Name].Value))
                        {
                            GWBSDetail dtl = row.Tag as GWBSDetail;

                            if (OptionCostType == OptCostType.合同收入)
                            {
                                //dtl.ContractPricePercent = priceValue;
                                dtl.ContractPrice = dtl.ContractPrice * priceValue;

                                //row.Cells[colContractPricePercent.Name].Value = dtl.ContractPricePercent;
                                row.Cells[colContractPriceRes.Name].Value = dtl.ContractPrice;
                            }
                            else if (OptionCostType == OptCostType.责任成本)
                            {
                                //dtl.ResponsiblePricePercent = priceValue;
                                dtl.ResponsibilitilyPrice = dtl.ResponsibilitilyPrice * priceValue;

                                //row.Cells[colResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
                                row.Cells[colResponsiblePriceRes.Name].Value = dtl.ResponsibilitilyPrice;
                            }
                            else if (OptionCostType == OptCostType.计划成本)
                            {
                                //dtl.PlanPricePercent = priceValue;
                                dtl.PlanPrice = dtl.PlanPrice * priceValue;

                                //row.Cells[colPlanPricePercent.Name].Value = dtl.PlanPricePercent;
                                row.Cells[colPlanPriceRes.Name].Value = dtl.PlanPrice;
                            }

                            row.Tag = dtl;
                        }
                    }
                }
                #endregion
            }
            else if (e.ClickedItem.Name == 设置勾选行为责任明细ToolStripMenuItem.Name)
            {
                foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
                {
                    bool isSelect = false;
                    object value = row.Cells[colTaskDtlSelectRes.Name].EditedFormattedValue;
                    if (value != null)
                    {
                        isSelect = Convert.ToBoolean(value);
                    }
                    if (isSelect == false)
                        continue;

                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    dtl.ResponseFlag = 1;
                    row.Tag = dtl;

                    row.Cells[colResponsibleAccFlagRes.Name].Value = "是";
                }
            }
            else if (e.ClickedItem.Name == 设置勾选行为计划明细ToolStripMenuItem.Name)
            {
                foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
                {
                    bool isSelect = false;
                    object value = row.Cells[colTaskDtlSelectRes.Name].EditedFormattedValue;
                    if (value != null)
                    {
                        isSelect = Convert.ToBoolean(value);
                    }
                    if (isSelect == false)
                        continue;

                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    dtl.CostingFlag = 1;
                    row.Tag = dtl;

                    row.Cells[colCostAccFlagRes.Name].Value = "是";
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
                //oq.AddCriterion(Expression.Eq("Id", optDtl.Id));
                //oq.AddFetchMode("CostAccountSubjectGUID", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("TheGWBSDetail", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("TheGWBSDetail.TheCostItem", NHibernate.FetchMode.Eager);
                //optDtl = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq)[0] as GWBSDetailCostSubject;


                //oq.Criterions.Clear();
                //oq.FetchModes.Clear();
                oq.AddCriterion(Expression.Not(Expression.Eq("TheProjectGUID", projectInfo.Id)));
                oq.AddCriterion(Expression.Eq("State", GWBSDetailCostSubjectState.生效));
                oq.AddCriterion(Expression.Eq("ResourceTypeGUID", optDtl.ResourceTypeGUID));
                if (optDtl.CostAccountSubjectGUID != null)
                {
                    oq.AddCriterion(Expression.Eq("CostAccountSubjectGUID.Id", optDtl.CostAccountSubjectGUID.Id));
                }
                oq.AddCriterion(Expression.Eq("TheGWBSDetail.MainResourceTypeId", optDtl.TheGWBSDetail.MainResourceTypeId));
                if (optDtl.TheGWBSDetail.TheCostItem != null)
                {
                    oq.AddCriterion(Expression.Eq("TheGWBSDetail.TheCostItem.Id", optDtl.TheGWBSDetail.TheCostItem.Id));
                }
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
                addUsageTaskDtl.ListCostSubjectDetails.Add(dtlUsage);
                dtlUsage.TheGWBSTree = addUsageTaskDtl.TheGWBS;
                dtlUsage.TheGWBSTreeName = addUsageTaskDtl.TheGWBS.Name;
                dtlUsage.TheGWBSTreeSyscode = addUsageTaskDtl.TheGWBSSysCode;
                dtlUsage.TheProjectGUID = addUsageTaskDtl.TheProjectGUID;
                dtlUsage.TheProjectName = addUsageTaskDtl.TheProjectName;
                dtlUsage.MainResTypeFlag = false;

                AddDetailUsageInfoInGrid(dtlUsage);

                #endregion
            }
            else if (e.ClickedItem.Name == 删除资源耗用ToolStripMenuItem1.Name)
            {
                #region 删除资源耗用

                DataGridViewRow row = gridDtlUsage.Rows[gridDtlUsage.CurrentCell.RowIndex];
                GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;

                if (MessageBox.Show("确认要删除资源耗用“" + dtl.Name + "”吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                if  (dtl!=null)
                {
                    listDeleteDtlUsages.Add(dtl.Id);
                  IList<GWBSDetail> lst=  gridGWBDetailRes.Rows.OfType<DataGridViewRow>().Select(a => a.Tag as GWBSDetail).Where(a => a == dtl.TheGWBSDetail).ToList();
                  if (lst != null && lst.Count > 0)
                  {
                      lst[0].ListCostSubjectDetails.Remove(dtl);
                  }
                }

                gridDtlUsage.Rows.RemoveAt(row.Index);

                #endregion
            }
            else if (e.ClickedItem.Name == 拷贝责任成本到计划成本ToolStripMenuItem.Name)
            {
                if (MessageBox.Show("确认要拷贝勾选行的责任成本到计划成本吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                btnCopyResponsibleToPlan_Click(null,null);

            }
            else if (e.ClickedItem.Name == 拷贝计划成本到责任成本ToolStripMenuItem.Name)
            {
                if (MessageBox.Show("确认要拷贝勾选行的计划成本到责任成本吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                btnCopyPlanToResponsible_Click(null, null);
            }
            else if (e.ClickedItem.Name == 拷贝计划成本到合同收入ToolStripMenuItem.Name)
            {
                if (MessageBox.Show("确认要拷贝勾选行的计划成本到合同收入吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                CopyPlanToContractIncome();
            }
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
        void VGWBSDetailBatchChange_Load(object sender, EventArgs e)
        {
            if (DefaultGWBSTreeNode != null)
                txtCurrentPath.Text = DefaultGWBSTreeNode.FullPath;

            SetVisibleColumn();

            this.WindowState = FormWindowState.Maximized;
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

        void chkAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllSelect.Checked)
            {
                foreach (DataGridViewRow oRow in gridGWBDetail.Rows)
                {
                    oRow.Cells[colTaskDtlSelect.Name].Value = true;
                }
                chkUnSelect.CheckedChanged -= chkUnSelect_CheckedChanged;
                chkUnSelect.Checked = false;
                chkUnSelect.CheckedChanged += chkUnSelect_CheckedChanged;
            }
        }
        void chkUnSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnSelect.Checked)
            {
                foreach (DataGridViewRow oRow in gridGWBDetail.Rows)
                {
                    oRow.Cells[colTaskDtlSelect.Name].Value = !ClientUtil.ToBool(oRow.Cells[colTaskDtlSelect.Name].Value);
                }
                chkAllSelect.CheckedChanged -= chkAllSelect_CheckedChanged;
                chkAllSelect.Checked = false;
                chkAllSelect.CheckedChanged += chkAllSelect_CheckedChanged;
            }
        }
        void chkAllSelectRes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllSelectRes.Checked)
            {
                foreach (DataGridViewRow oRow in gridGWBDetailRes.Rows)
                {
                    oRow.Cells[colTaskDtlSelectRes.Name].Value = true;
                }
                chkUnSelectRes.CheckedChanged -= chkUnSelectRes_CheckedChanged;
                chkUnSelectRes.Checked = false;
                chkUnSelectRes.CheckedChanged += chkUnSelectRes_CheckedChanged;
            }
        }
        void chkUnSelectRes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnSelectRes.Checked)
            {
                foreach (DataGridViewRow oRow in gridGWBDetailRes.Rows)
                {
                    oRow.Cells[colTaskDtlSelectRes.Name].Value = !ClientUtil.ToBool(oRow.Cells[colTaskDtlSelectRes.Name].Value);
                }
                chkAllSelectRes.CheckedChanged -= chkAllSelectRes_CheckedChanged;
                chkAllSelectRes.Checked = false;
                chkAllSelectRes.CheckedChanged += chkAllSelectRes_CheckedChanged;
            }
        }
        void chkAllSelectUse_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllSelect.Checked)
            {
                foreach (DataGridViewRow oRow in this.gridDtlUsage.Rows)
                {
                    oRow.Cells[usageSelect.Name].Value = true;
                }
                chkUnSelectUse.CheckedChanged -= chkUnSelectUse_CheckedChanged;
                chkUnSelectUse.Checked = false;
                chkUnSelectUse.CheckedChanged += chkUnSelectUse_CheckedChanged;
            }
        }
        void chkUnSelectUse_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnSelectUse.Checked)
            {
                foreach (DataGridViewRow oRow in this.gridDtlUsage.Rows)
                {
                    oRow.Cells[usageSelect.Name].Value = !ClientUtil.ToBool(oRow.Cells[usageSelect.Name].Value);
                }
                chkAllSelectUse.CheckedChanged -= chkAllSelectUse_CheckedChanged;
                chkAllSelectUse.Checked = false;
                chkAllSelectUse.CheckedChanged += chkAllSelectUse_CheckedChanged;
            }
        }

        void btnResourceSet_Click(object sender, EventArgs e)
        {
            GWBSDetail dtl = null;
            try
            {
                if (this.gridGWBDetailBG.Rows.Count == 0) throw new Exception("工程任务明细没有进行签证变更");
                this.tabControl1.TabPages.Remove(this.tabPageBG);
                this.tabControl1.TabPages.Add(this.tabPageResource);
                gridGWBDetailRes.Rows.Clear();
                gridDtlUsage.Rows.Clear();
                foreach (DataGridViewRow oRow in this.gridGWBDetailBG.Rows)
                {
                    dtl = oRow.Tag as GWBSDetail;
                    if (dtl == null) throw new Exception("签证变更的工程任务明细为空");
                    AddDetailInfoInGrid1(dtl);
                }
                SetButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show("工程资源耗用显示出错:"+ExceptionUtil.ExceptionMessage(ex));
            }
            
        }
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridGWBDetailBG.Rows.Count == 0) throw new Exception("没有进行签证变更,不需要保存");
                if (ValidateSave())
                {
                    if (MessageBox.Show("是否确定保存", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        AccountCostData();
                        oGWBSTreeNode = model.SaveOrUpdateGWBSTree(this.oGWBSTreeNode, null);
                       
                            UpdateGridGWBSDetailBG();
                        
                        IsUpdated = false;
                        MessageBox.Show("保存成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("工程任务明细保存失败:"+ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridGWBDetailBG.Rows.Count == 0) throw new Exception("没有进行签证变更,不需要保存");
                if (ValidateSave())
                {
                    if (MessageBox.Show("是否确定保存", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        AccountCostData();
                        oGWBSTreeNode = model.SaveOrUpdateGWBSTree(this.oGWBSTreeNode, null);
                        MessageBox.Show("保存成功");
                        IsUpdated = false;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("工程任务明细保存失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
      
        void btnChange_Click(object sender, EventArgs e)
        {
            int iMaxOrderNo = 0;
            int iMaxCode = 0;
            int iNum = 0;
            DateTime dServiceTime;
            Hashtable ht = new Hashtable();
            IList<GWBSDetail> lstGWBSDetail = null;
            IList<GWBSDetail> lstBGGWBSDetail = null;
            GWBSDetail oGWBSDetail = null;
            IList lstTemp = null;
            ContractGroup oContractGroup;
            try
            {
                 oContractGroup = txtContractGroupName.Tag==null?null:txtContractGroupName.Tag as ContractGroup;
                 if (oContractGroup == null) throw new Exception("请选择[选择变更契约]");
                if (gridGWBDetail.Rows.Count == 0) throw new Exception("工程WBS明细列表为空");
                lstGWBSDetail = gridGWBDetail.Rows.OfType<DataGridViewRow>().Where(a => ClientUtil.ToBool(a.Cells[colTaskDtlSelect.Name].Value) == true).Select(a => a.Tag as GWBSDetail).ToList();//获取需要签证变更的GWBS
                lstTemp = lstGWBSDetail.Where(a => !string.IsNullOrEmpty(a.ChangeParentID)).ToList();
                if (lstTemp != null && lstTemp.Count > 0)
                {
                    throw new Exception(string.Format("[{0}]无法进行签证变更，因为这些单据是签证变更后的工程WBS明细", string.Join("、", lstTemp.OfType<GWBSDetail>().Select(a => a.Name).ToArray())));
                }
                if (lstGWBSDetail == null || lstGWBSDetail.Count == 0) throw new Exception("请选择需要签证变更的WBS明细");
                lstBGGWBSDetail = this.gridGWBDetailBG.Rows.OfType<DataGridViewRow>().Select(a => a.Tag as GWBSDetail).ToList();//获取临时存储的签证变更明细
                if (MessageBox.Show("是否进行签证变更", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    #region 签证变更
                    if (GWBSTreeNode == null) throw new Exception("请选择工程WBS树节点");
                    lstTemp = model.GetWBSDetailNum(GWBSTreeNode.Id, lstGWBSDetail.Select(a => a.Id).ToArray());
                    int iTemp = (lstBGGWBSDetail == null ? 0 : lstBGGWBSDetail.Where(a => string.IsNullOrEmpty(a.Id)).Count());
                    iMaxCode = ClientUtil.ToInt(lstTemp[0]) + iTemp;
                    iMaxOrderNo = ClientUtil.ToInt(lstTemp[1]) + iTemp;
                    dServiceTime = ClientUtil.ToDateTime(lstTemp[2]);
                    ht = lstTemp[3] as Hashtable;
                    if (lstBGGWBSDetail != null && lstBGGWBSDetail.Count > 0)
                    {
                        foreach (GWBSDetail obj in lstBGGWBSDetail.Where(a => string.IsNullOrEmpty(a.Id)).ToList())
                        {
                            if (ht.Contains(obj.ChangeParentID))
                            {
                                ht[obj.ChangeParentID] = ClientUtil.ToInt(ht[obj.ChangeParentID]) + 1;
                            }
                            else
                            {
                                ht[obj.ChangeParentID] = 1;
                            }
                        }
                    }
                    foreach (GWBSDetail obj in lstGWBSDetail)
                    {
                        oGWBSDetail = obj.Clone(oContractGroup);
                       
                        oGWBSDetail.Code = GetCode(++iMaxCode);
                        oGWBSDetail.OrderNo = ++iMaxOrderNo;
                        iNum = ht.Contains(obj.Id) ? ClientUtil.ToInt(ht[obj.Id]) : 0;
                        oGWBSDetail.DiagramNumber = GetDiagramNumber(obj, ++iNum);
                        oGWBSDetail.Name = GetName(obj, iNum);
                        oGWBSDetail.TheGWBS = oGWBSTreeNode;
                        oGWBSTreeNode.Details.Add(oGWBSDetail);
                        AddDetailInfoInBGGrid(oGWBSDetail);
                        IsUpdated = true;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("批量签证变更失败:" + ex.Message);
            }
            finally
            {
                if (IsUpdated)
                {
                    this.btnResourceSet.Visible = this.btnSave.Visible = this.btnSaveAndExit.Visible = true;
                }
            }
        }
        void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                GWBSDetail dtl = null;
                this.tabControl1.TabPages.Remove(tabPageResource);
                this.tabControl1.TabPages.Add(tabPageBG);
                this.gridGWBDetailBG.Rows.Clear();
                foreach (DataGridViewRow oRow in this.gridGWBDetailRes.Rows)
                {
                    dtl = oRow.Tag as GWBSDetail;
                    if (dtl == null) throw new Exception("签证变更的工程任务明细为空");
                    AddDetailInfoInBGGrid(dtl);
                }
                SetButton();
            }
            catch (Exception ex)
            {
                MessageBox.Show("显示工程任务明细失败:" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void VGWBSDetailBatchChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsUpdated & MessageBox.Show("是否确定退出:修改结果没有保存", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
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
                FlashScreen.Show("正在查询加载数据,请稍候......");

                gridGWBDetail.Rows.Clear();
                gridGWBDetailBG.Rows.Clear();
                QueryGWBSDetailInGrid();
                //cbTaskDtlSelect_CheckedChanged(cbTaskDtlSelect, new EventArgs());
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
            //if (txtResourceCateBySearch.Text.Trim() != "" && txtResourceCateBySearch.Result != null && txtResourceCateBySearch.Result.Count > 0)
            //{
            //    matCate = txtResourceCateBySearch.Result[0] as MaterialCategory;
            //}
            //if (txtResourceTypeBySearch.Text.Trim() != "")
            //    mat = txtResourceTypeBySearch.Tag as Material;

            if (txtContractBySearch.Text.Trim() != "")
                cg = txtContractBySearch.Tag as ContractGroup;

            //string usageName = txtResourceUsageName.Text.Trim();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheGWBS", gwbs));
           oq.AddCriterion(Expression.IsNull("ChangeParentID"));
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


            if (taskType != null)
                oq.AddCriterion(Expression.Eq("ProjectTaskTypeCode", taskType.Code));//代码唯一

            if (costItem != null)
                oq.AddCriterion(Expression.Eq("TheCostItem.Id", costItem.Id));

            if (cg != null)
                oq.AddCriterion(Expression.Eq("ContractGroupGUID", cg.Id));

            if (costItemCate != null)
                oq.AddCriterion(Expression.Like("TheCostItemCateSyscode", costItemCate.SysCode, MatchMode.Start));

            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ListCostSubjectDetails",NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ListCostSubjectDetails.ResourceUsageQuota", NHibernate.FetchMode.Eager);

            IList listTempDtl = model.ObjectQuery(typeof(GWBSDetail), oq);

            foreach (GWBSDetail dtl in listTempDtl)
            {
                AddDetailInfoInGrid(dtl);
            }
            gridGWBDetailBG.ClearSelection();

            //加载明细耗用数据
           
        }
        //分包取费计算
        void btnSubcontractFeeAcc_Click(object sender, EventArgs e)
        {
            bool flag = false;//是否勾选任务明细或资源耗用

            IList listSubContractFeeDtl = new ArrayList();
            IList listSubContractFeeDtlUsage = new ArrayList();

            foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
            {
                GWBSDetail dtl = row.Tag as GWBSDetail;
                if (Convert.ToBoolean(row.Cells[colTaskDtlSelectRes.Name].Value) && dtl.SubContractFeeFlag == true)
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
                gridGWBDetailRes.Focus();
                return;
            }
            if (listSubContractFeeDtl.Count == 0)
            {
                MessageBox.Show("选择的分包取费任务明细中数据不完整（未标记“责任核算明细”或“成本核算明细”）,请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gridGWBDetailRes.Focus();
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


        //复选框选择处理(任务明细)
        void gridGWBDetailRes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                string colName = gridGWBDetailRes.Columns[e.ColumnIndex].Name;
                if (colName == colTaskDtlSelectRes.Name)
                {
                    object value = gridGWBDetailRes.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
                    if (value != null)
                    {
                        bool isSelect = Convert.ToBoolean(value);
                        DataGridViewRow row = gridGWBDetailRes.Rows[e.RowIndex];
                        if (isSelect)
                        {
                            row.Cells[colTaskDtlSelectRes.Name].Style.BackColor = selectRowBackColor;
                        }
                        else
                        {
                            row.Cells[colTaskDtlSelectRes.Name].Style.BackColor = unSelectRowBackColor_White;
                        }


                        GWBSDetail dtl = row.Tag as GWBSDetail;

                        if (isSelect)
                        {
                            AddDetailUsageInfoInGrid(dtl);
                        }
                        else
                        {
                            RemoveDetailUsageInfoInGrid(dtl);
                        }
                    }

                    GetDtlUsageSumQnyAndPrice();
                }
            }
        }
        void gridGWBDetailRes_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                gridGWBDetailRes.Rows[e.RowIndex].Selected = true;
                gridGWBDetailRes.CurrentCell = gridGWBDetailRes.Rows[e.RowIndex].Cells[e.ColumnIndex];
            }
        }
        void gridGWBDetailRes_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridGWBDetailRes.Rows[e.RowIndex].ReadOnly == false)
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
                        string colName = gridGWBDetailRes.Columns[e.ColumnIndex].Name;

                        //数据格式校验
                        if (colName == colContractQuantityRes.Name || colName == colContractPriceRes.Name)//合同
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                        if (colName == colResponsibleQuantityRes.Name || colName == colResponsiblePriceRes.Name)//责任
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                        if (colName == colPlanQuantityRes.Name || colName == colPlanPriceRes.Name)//计划
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
        void gridGWBDetailRes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridGWBDetailRes.Rows[e.RowIndex].ReadOnly == false)
            {
                object tempValue = gridGWBDetailRes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                DataGridViewRow currEditRow = gridGWBDetailRes.Rows[e.RowIndex];
                GWBSDetail dtl = gridGWBDetailRes.Rows[e.RowIndex].Tag as GWBSDetail;

                string colName = gridGWBDetailRes.Columns[e.ColumnIndex].Name;

                //耗用数据
                if (OptionCostType == OptCostType.合同收入)
                {
                    if (colName == colContractQuantityRes.Name)
                    {
                        decimal ContractProjectQuantity = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractProjectQuantity = ClientUtil.ToDecimal(value);

                        dtl.ContractProjectQuantity = ContractProjectQuantity;
                    }
                    else if (colName == colContractPriceRes.Name)
                    {
                        decimal ContractPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractPrice = ClientUtil.ToDecimal(value);

                        dtl.ContractPrice = ContractPrice;
                    }
                }
                else if (OptionCostType == OptCostType.责任成本)
                {
                    if (colName == colResponsibleQuantityRes.Name)
                    {
                        decimal ResponsibilitilyWorkAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(value);

                        dtl.ResponsibilitilyWorkAmount = ResponsibilitilyWorkAmount;
                    }
                    else if (colName == colResponsiblePriceRes.Name)
                    {
                        decimal ResponsibilitilyPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyPrice = ClientUtil.ToDecimal(value);

                        dtl.ResponsibilitilyPrice = ResponsibilitilyPrice;

                    }

                }
                else if (OptionCostType == OptCostType.计划成本)
                {
                    if (colName == colPlanQuantityRes.Name)
                    {
                        decimal PlanWorkAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanWorkAmount = ClientUtil.ToDecimal(value);

                        dtl.PlanWorkAmount = PlanWorkAmount;
                    }
                    else if (colName == colPlanPriceRes.Name)
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
        private GWBSDetail getTaskDtl(GWBSDetailCostSubject subject)
        {
            foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
            {
                GWBSDetail dtl = row.Tag as GWBSDetail;
                if (dtl== subject.TheGWBSDetail)
                {
                    return dtl;
                }
            }
            return null;
        }
        private void AddDetailInfoInGrid(GWBSDetail dtl)
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

            
        }
      
        private void AddDetailInfoInBGGrid(GWBSDetail dtl)
        {
            int index = gridGWBDetailBG.Rows.Add();
            DataGridViewRow row = gridGWBDetailBG.Rows[index];

            row.Cells[colBGProjectTaskName.Name].Value = dtl.TheGWBS.Name;
            row.Cells[colBGProjectTaskName.Name].ToolTipText = dtl.TheGWBS.FullPath;// Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBS);

            row.Cells[colBGProjectTaskDtlName.Name].Value = dtl.Name;

            row.Cells[colBGResponsibleAccFlag.Name].Value = dtl.ResponseFlag == 1 ? "是" : "否";
            row.Cells[colBGCostAccFlag.Name].Value = dtl.CostingFlag == 1 ? "是" : "否";
            row.Cells[colBGSubContractFlag.Name].Value = dtl.SubContractFeeFlag == true ? "是" : "否";

            row.Cells[colBGContractName.Name].Value = dtl.ContractGroupName;
            if (dtl.TheCostItem != null)
            {
                row.Cells[colBGCostItemName.Name].Value = dtl.TheCostItem.Name;
                row.Cells[colBGCostItemQuotaCode.Name].Value = dtl.TheCostItem.QuotaCode;
            }
            row.Cells[colBGMainResourceName.Name].Value = dtl.MainResourceTypeName;
            row.Cells[colBGSpec.Name].Value = dtl.MainResourceTypeSpec;
            row.Cells[colBGDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Cells[colBGContractQuantity.Name].Value = dtl.ContractProjectQuantity;
            row.Cells[colBGContractPrice.Name].Value = dtl.ContractPrice;
            //row.Cells[colContractPricePercent.Name].Value = dtl.ContractPricePercent;
            //row.Cells[colContractPriceResult.Name].Value = dtl.ContractPrice * dtl.ContractPricePercent;
            row.Cells[colBGContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

            row.Cells[colBGResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[colBGResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
            //row.Cells[colResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
            //row.Cells[colResponsiblePriceResult.Name].Value = dtl.ResponsibilitilyPrice * dtl.ResponsiblePricePercent;
            row.Cells[colBGResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[colBGPlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[colBGPlanPrice.Name].Value = dtl.PlanPrice;
            //row.Cells[colPlanPricePercent.Name].Value = dtl.PlanPricePercent;
            //row.Cells[colPlanPriceResult.Name].Value = dtl.PlanPrice * dtl.PlanPricePercent;
            row.Cells[colBGPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

            row.Cells[colBGQuantityUnit.Name].Value = dtl.WorkAmountUnitName;
            row.Cells[colBGPriceUnit.Name].Value = dtl.PriceUnitName;

            //if (dtl.TheCostItem != null)
            //    row.Cells[colCostItemBasePrice.Name].Value = dtl.TheCostItem.Price;

            row.Cells[colBGTaskDtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);

            row.Tag = dtl;

        }
        private void AddDetailInfoInGrid1(GWBSDetail dtl)
        {
            #region 填充资源耗用中的签证变更明细
            int index = gridGWBDetailRes.Rows.Add();
            DataGridViewRow row = gridGWBDetailRes.Rows[index];
            row.Cells[colProjectTaskNameRes.Name].Value = dtl.TheGWBS.Name;
            row.Cells[colProjectTaskNameRes.Name].ToolTipText = dtl.TheGWBS.FullPath;// Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBS);

            row.Cells[colProjectTaskDtlNameRes.Name].Value = dtl.Name;

            row.Cells[colResponsibleAccFlagRes.Name].Value = dtl.ResponseFlag == 1 ? "是" : "否";
            row.Cells[colCostAccFlagRes.Name].Value = dtl.CostingFlag == 1 ? "是" : "否";
            row.Cells[colSubContractFlagRes.Name].Value = dtl.SubContractFeeFlag == true ? "是" : "否";

            row.Cells[colContractNameRes.Name].Value = dtl.ContractGroupName;
            if (dtl.TheCostItem != null)
            {
                row.Cells[colCostItemNameRes.Name].Value = dtl.TheCostItem.Name;
                row.Cells[colCostItemQuotaCodeRes.Name].Value = dtl.TheCostItem.QuotaCode;
            }
            row.Cells[colMainResourceNameRes.Name].Value = dtl.MainResourceTypeName;
            row.Cells[colSpecRes.Name].Value = dtl.MainResourceTypeSpec;
            row.Cells[colDiagramNumberRes.Name].Value = dtl.DiagramNumber;

            row.Cells[colContractQuantityRes.Name].Value = dtl.ContractProjectQuantity;
            row.Cells[colContractPriceRes.Name].Value = dtl.ContractPrice;
            //row.Cells[colContractPricePercent.Name].Value = dtl.ContractPricePercent;
            //row.Cells[colContractPriceResult.Name].Value = dtl.ContractPrice * dtl.ContractPricePercent;
            row.Cells[colContractTotalPriceRes.Name].Value = dtl.ContractTotalPrice;

            row.Cells[colResponsibleQuantityRes.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[colResponsiblePriceRes.Name].Value = dtl.ResponsibilitilyPrice;
            //row.Cells[colResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
            //row.Cells[colResponsiblePriceResult.Name].Value = dtl.ResponsibilitilyPrice * dtl.ResponsiblePricePercent;
            row.Cells[colResponsibleTotalPriceRes.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[colPlanQuantityRes.Name].Value = dtl.PlanWorkAmount;
            row.Cells[colPlanPriceRes.Name].Value = dtl.PlanPrice;
            //row.Cells[colPlanPricePercent.Name].Value = dtl.PlanPricePercent;
            //row.Cells[colPlanPriceResult.Name].Value = dtl.PlanPrice * dtl.PlanPricePercent;
            row.Cells[colPlanTotalPriceRes.Name].Value = dtl.PlanTotalPrice;

            row.Cells[colQuantityUnitRes.Name].Value = dtl.WorkAmountUnitName;
            row.Cells[colPriceUnitRes.Name].Value = dtl.PriceUnitName;

            //if (dtl.TheCostItem != null)
            //    row.Cells[colCostItemBasePrice.Name].Value = dtl.TheCostItem.Price;

            row.Cells[colTaskDtlStateRes.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);

            row.Tag = dtl;
            #endregion
        }
        private void AddDetailUsageInfoInGrid(GWBSDetail oGWBSDetail)
        {
            foreach (GWBSDetailCostSubject dtl in oGWBSDetail.ListCostSubjectDetails)
            {
                AddDetailUsageInfoInGrid(dtl);
            }
        }
        void AddDetailUsageInfoInGrid(GWBSDetailCostSubject dtl)
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
        }
        private void RemoveDetailUsageInfoInGrid(GWBSDetail dtl)
        {
            foreach (GWBSDetailCostSubject oGWBSDetailCostSubject in dtl.ListCostSubjectDetails)
            {
                IList<DataGridViewRow> lst = this.gridDtlUsage.Rows.OfType<DataGridViewRow>().Where(a => a.Tag != null && (a.Tag as GWBSDetailCostSubject).TheGWBSDetail == dtl).ToList();
                if (lst.Count > 0)
                {
                    foreach (DataGridViewRow oRow in lst)
                    {
                        this.gridDtlUsage.Rows.Remove(oRow);
                    }
                }
            }
        }
        
        private List<DataGridViewRow> GetTaskDtlSelectRows()
        {
            List<DataGridViewRow> listTaskDtlSelectRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
            {
                bool isSelect = false;
                object value = row.Cells[colTaskDtlSelectRes.Name].EditedFormattedValue;
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
        /// 获取签证变更Code
        /// </summary>
        /// <param name="iCode"></param>
        /// <returns></returns>
        string GetCode(int iCode)
        {
            return this.GWBSTreeNode.Code + "-" + (iCode).ToString().PadLeft(5, '0');
        }
        /// <summary>
        /// 获取签证变更图号
        /// </summary>
        /// <param name="oDetail"></param>
        /// <param name="iNum"></param>
        /// <returns></returns>
        string GetDiagramNumber(GWBSDetail oDetail, int iNum)
        {
            if (string.IsNullOrEmpty(oDetail.DiagramNumber))
            {
                return string.Format("[{0}]", iNum);
            }
            else
            {
                return string.Format("{0}-[{1}]", oDetail.DiagramNumber, iNum);
            }
        }
        /// <summary>
        /// 获取签证变更名称
        /// </summary>
        /// <param name="oDetail"></param>
        /// <param name="iNum"></param>
        /// <returns></returns>
        string GetName(GWBSDetail oDetail, int iNum)
        {
            if (string.IsNullOrEmpty(oDetail.Name))
            {
                return string.Format("[{0}]", iNum);
            }
            else
            {
                return string.Format("{0}-[{1}]", oDetail.Name, iNum);
            }
        }
        void UpdateGridGWBSDetailBG()
        {
            IList<GWBSDetail> lstTemp = null;
            IList lstDetail = this.gridGWBDetailBG.Rows.OfType<DataGridViewRow>().Select(a => a.Tag as GWBSDetail).ToList();
            if (lstDetail != null && lstDetail.Count > 0)
            {
                DataGridView oGrid = this.tabControl1.TabPages[0] == tabPageBG ? this.gridGWBDetailBG : this.gridGWBDetailRes;
                oGrid.Rows.Clear();
                if (oGrid == gridGWBDetailRes) this.gridDtlUsage.Rows.Clear();
                foreach (GWBSDetail oDetail in lstDetail)
                {
                    lstTemp = oGWBSTreeNode.Details.Where(a => a.OrderNo == oDetail.OrderNo && a.Code == oDetail.Code).ToList();
                    if (lstTemp != null && lstTemp.Count > 0)
                    {
                        if (oGrid == this.gridGWBDetailRes)
                        {
                           AddDetailInfoInGrid1(lstTemp[0]);
                        }
                        else
                        { 
                            AddDetailInfoInBGGrid(lstTemp[0]);
                        }      
                    }
                }
            }

        }
        private void SetVisibleColumn()
        {
            bool contractFlag = false;
            bool responsibleFlag = false;
            bool planFlag = false;

            if (OptionCostType == OptCostType.合同收入) contractFlag = true;
            else if (OptionCostType == OptCostType.责任成本) responsibleFlag = true;
            else if (OptionCostType == OptCostType.计划成本) planFlag = true;


            gridGWBDetailRes.Columns[colContractPriceRes.Name].Visible = contractFlag;
            //gridGWBDetail.Columns[colContractPricePercent.Name].Visible = contractFlag;
            //gridGWBDetail.Columns[colContractPriceResult.Name].Visible = contractFlag;
            gridGWBDetailRes.Columns[colContractQuantityRes.Name].Visible = contractFlag;
            gridGWBDetailRes.Columns[colContractTotalPriceRes.Name].Visible = contractFlag;

            gridGWBDetailRes.Columns[colResponsiblePriceRes.Name].Visible = responsibleFlag;
            //gridGWBDetail.Columns[colResponsiblePricePercent.Name].Visible = responsibleFlag;
            //gridGWBDetail.Columns[colResponsiblePriceResult.Name].Visible = responsibleFlag;
            gridGWBDetailRes.Columns[colResponsibleQuantityRes.Name].Visible = responsibleFlag;
            gridGWBDetailRes.Columns[colResponsibleTotalPriceRes.Name].Visible = responsibleFlag;

            gridGWBDetailRes.Columns[colPlanPriceRes.Name].Visible = planFlag;
            //gridGWBDetail.Columns[colPlanPricePercent.Name].Visible = planFlag;
            //gridGWBDetail.Columns[colPlanPriceResult.Name].Visible = planFlag;
            gridGWBDetailRes.Columns[colPlanQuantityRes.Name].Visible = planFlag;
            gridGWBDetailRes.Columns[colPlanTotalPriceRes.Name].Visible = planFlag;



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
        private bool ValidateMainResourceFlag(GWBSDetailCostSubject dtlUsage)
        {
            List<GWBSDetailCostSubject> listUsage = gridDtlUsage.Rows.OfType<DataGridViewRow>().Select(a => a.Tag as GWBSDetailCostSubject).Where(a => a.TheGWBSDetail == dtlUsage.TheGWBSDetail).ToList();//GetTaskDtlUsageByDtlId(dtlUsage.TheGWBSDetail.Id);
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
        private DataGridViewRow GetTaskDtlByDtlUsage(GWBSDetailCostSubject dtlUsage)
        {
            return gridGWBDetailRes.Rows.OfType<DataGridViewRow>().Where(a => (a.Tag as GWBSDetail) == dtlUsage.TheGWBSDetail).FirstOrDefault();
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

                rowDtl.Cells[colMainResourceNameRes.Name].Value = taskDtl.MainResourceTypeName;
                rowDtl.Cells[colSpecRes.Name].Value = taskDtl.MainResourceTypeSpec;
                rowDtl.Cells[colDiagramNumberRes.Name].Value = taskDtl.DiagramNumber;

                rowDtl.Tag = taskDtl;
            }
        }
        private void SetTaskDtlChecked(DataGridViewRow usageRow, bool isSelect)
        {
            GWBSDetailCostSubject dtlUsage = usageRow.Tag as GWBSDetailCostSubject;
            //string dtlId = dtlUsage.TheGWBSDetail.Id;

            bool isChecked = isSelect;//同一任务明细下的耗用是否有选中，如果有则选中所属任务明细，没有则取消所属任务明细的选中

            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                if (row.Index != usageRow.Index && item.TheGWBSDetail == dtlUsage.TheGWBSDetail && Convert.ToBoolean(row.Cells[usageSelect.Name].Value) == true)
                {
                    isChecked = true;
                    break;
                }
            }
            foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
            {
                GWBSDetail dtl = row.Tag as GWBSDetail;
                if (dtl == dtlUsage.TheGWBSDetail)
                {
                    row.Cells[colTaskDtlSelectRes.Name].Value = isChecked;

                    if (isChecked)
                        row.Cells[colTaskDtlSelectRes.Name].Style.BackColor = selectRowBackColor;
                    else
                        row.Cells[colTaskDtlSelectRes.Name].Style.BackColor = unSelectRowBackColor_White;

                    gridGWBDetailRes.CurrentCell = row.Cells[colTaskDtlSelectRes.Name];

                    break;
                }
            }

        }
        #region 消耗
        void btnAccountCostData_Click(object sender, EventArgs e)
        {
            bool flag = false;//是否勾选任务明细或资源耗用
            bool flag2 = false;//是否选择了有效的任务明细
            foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
            {
                if (Convert.ToBoolean(row.Cells[colTaskDtlSelectRes.Name].Value))
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
                gridGWBDetailRes.Focus();
                return;
            }
            

            AccountCostData();
        }
        bool ValidateSave()
        {
            #region 数据验证
            try
            {
                
                foreach (DataGridViewRow row in gridDtlUsage.Rows)
                {
                    GWBSDetailCostSubject dtlUsage = row.Tag as GWBSDetailCostSubject;
                    if (dtlUsage.CostAccountSubjectGUID == null)
                    {
                      
                        if (row.Visible)
                            gridDtlUsage.CurrentCell = row.Cells[usageAccountSubject.Name];
                        throw new Exception("请选择(" + (row.Index + 1) + "行)资源耗用“" + dtlUsage.Name + "”的核算科目！");
                    }
                    else if (OptionCostType == OptCostType.合同收入 && dtlUsage.ContractQuotaQuantity == 0)
                    {
                       
                        if (row.Visible)
                            gridDtlUsage.CurrentCell = row.Cells[usageContractQuotaNum.Name];
                        throw new Exception("请输入(" + (row.Index + 1) + "行)资源耗用“" + dtlUsage.Name + "”的合同定额数量！"  );
                    }
                    else if (OptionCostType == OptCostType.责任成本 && dtlUsage.ResponsibleQuotaNum == 0)
                    {
                       
                        if (row.Visible)
                            gridDtlUsage.CurrentCell = row.Cells[usageResponsibleQuotaNum.Name];
                        throw new Exception("请输入(" + (row.Index + 1) + "行)资源耗用“" + dtlUsage.Name + "”的责任定额数量！" );
                
                    }
                    else if (OptionCostType == OptCostType.计划成本 && dtlUsage.PlanQuotaNum == 0)
                    {
                      
                        if (row.Visible)
                            gridDtlUsage.CurrentCell = row.Cells[usagePlanQuotaNum.Name];
                         throw new  Exception("请输入(" + (row.Index + 1) + "行)资源耗用“" + dtlUsage.Name + "”的计划定额数量！" );
              
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:"+ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
           
            #endregion
         
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
            foreach (DataGridViewRow row in gridGWBDetailRes.Rows)
            {
                bool isSelect = false;
                object value = row.Cells[colTaskDtlSelectRes.Name].EditedFormattedValue;
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
                        if (subject.TheGWBSDetail== dtl)
                        {
                            dtlUsageProjectAmountPrice += subject.ContractPrice;

                            GWBSDetailCostSubject tempSubject = new GWBSDetailCostSubject();
                            tempSubject.Id = subject.Id;
                            listTempUsage.Add(tempSubject);
                        }
                    }
                    dtl.ContractPrice = dtlUsageProjectAmountPrice;
                    dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;

                    row.Cells[colContractPriceRes.Name].Value = dtl.ContractPrice;
                    row.Cells[colContractTotalPriceRes.Name].Value = dtl.ContractTotalPrice;
                }
                else if (OptionCostType == OptCostType.责任成本)
                {
                    //【责任单价】：=下属{工程资源耗用明细}_【责任工程量单价】
                    //【责任合价】：=【责任工程量】*【责任单价】

                    decimal dtlUsageProjectAmountPrice = 0;
                    foreach (DataGridViewRow rowTemp in gridDtlUsage.Rows)
                    {
                        GWBSDetailCostSubject subject = rowTemp.Tag as GWBSDetailCostSubject;
                        if (subject.TheGWBSDetail == dtl)
                        {
                            dtlUsageProjectAmountPrice += subject.ResponsibleWorkPrice;

                            GWBSDetailCostSubject tempSubject = new GWBSDetailCostSubject();
                            tempSubject.Id = subject.Id;
                            listTempUsage.Add(tempSubject);
                        }
                    }
                    dtl.ResponsibilitilyPrice = dtlUsageProjectAmountPrice;
                    dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                    row.Cells[colResponsiblePriceRes.Name].Value = dtl.ResponsibilitilyPrice;
                    row.Cells[colResponsibleTotalPriceRes.Name].Value = dtl.ResponsibilitilyTotalPrice;
                }
                else if (OptionCostType == OptCostType.计划成本)
                {
                    //【计划单价】：=下属{工程资源耗用明细}_【计划工程量单价】
                    //【计划合价】：=【计划工程量】*【计划单价】

                    decimal dtlUsageProjectAmountPrice = 0;
                    foreach (DataGridViewRow rowTemp in gridDtlUsage.Rows)
                    {
                        GWBSDetailCostSubject subject = rowTemp.Tag as GWBSDetailCostSubject;
                        if (subject.TheGWBSDetail== dtl)
                        {
                            dtlUsageProjectAmountPrice += subject.PlanWorkPrice;

                            GWBSDetailCostSubject tempSubject = new GWBSDetailCostSubject();
                            tempSubject.Id = subject.Id;
                            listTempUsage.Add(tempSubject);
                        }
                    }
                    dtl.PlanPrice = dtlUsageProjectAmountPrice;
                    dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                    row.Cells[colPlanPriceRes.Name].Value = dtl.PlanPrice;
                    row.Cells[colPlanTotalPriceRes.Name].Value = dtl.PlanTotalPrice;
                }
                listTempDtl.Add(row.Tag as GWBSDetail);
                //GWBSDetail tempDtl = new GWBSDetail();
                //tempDtl.Id = dtl.Id;
                //tempDtl.ListCostSubjectDetails = new HashedSet<GWBSDetailCostSubject>();
                //tempDtl.ListCostSubjectDetails.AddAll(listTempUsage);
                //listTempDtl.Add(tempDtl);

                //row.Tag = dtl;
            }
            #region 为什么重复计算
            //if (listTempDtl.Count > 0)
            //{
            //    //listTempDtl = model.AccountTaskDtlPrice(listTempDtl);
            //    foreach (int rowIndex in listRowIndex)
            //    {
            //        DataGridViewRow row = gridGWBDetailRes.Rows[rowIndex];
            //        GWBSDetail dtl = row.Tag as GWBSDetail;
            //        var query = from d in listTempDtl
            //                    where d== dtl
            //                    select d;
            //        if (query.Count() > 0)
            //        {
            //            GWBSDetail tempDtl = query.ElementAt(0);
            //            if (OptionCostType == OptCostType.合同收入)
            //            {
            //                dtl.ContractPrice += tempDtl.ContractPrice;
            //                dtl.ContractTotalPrice = dtl.ContractProjectQuantity * dtl.ContractPrice;
            //                row.Cells[colContractPriceRes.Name].Value = dtl.ContractPrice;
            //                row.Cells[colContractTotalPriceRes.Name].Value = dtl.ContractTotalPrice;
            //            }
            //            else if (OptionCostType == OptCostType.责任成本)
            //            {
            //                dtl.ResponsibilitilyPrice += tempDtl.ResponsibilitilyPrice;
            //                dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;
            //                row.Cells[colResponsiblePriceRes.Name].Value = dtl.ResponsibilitilyPrice;
            //                row.Cells[colResponsibleTotalPriceRes.Name].Value = dtl.ResponsibilitilyTotalPrice;
            //            }
            //            else if (OptionCostType == OptCostType.计划成本)
            //            {
            //                dtl.PlanPrice += tempDtl.PlanPrice;
            //                dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;
            //                row.Cells[colPlanPriceRes.Name].Value = dtl.PlanPrice;
            //                row.Cells[colPlanTotalPriceRes.Name].Value = dtl.PlanTotalPrice;
            //            }
            //            row.Tag = dtl;
            //        }
            //    }
            //}
            #endregion
            GetDtlUsageSumQnyAndPrice();
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
        private void UpdateDetailInfoInGrid(GWBSDetail dtl)
        {
            for (int i = 0; i < gridGWBDetailRes.Rows.Count; i++)
            {
                DataGridViewRow row = gridGWBDetailRes.Rows[i];
                GWBSDetail d = row.Tag as GWBSDetail;
                if (d.Id == dtl.Id)
                {
                    row.Cells[colProjectTaskNameRes.Name].Value = dtl.TheGWBS.Name;
                    row.Cells[colProjectTaskNameRes.Name].ToolTipText = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBS);

                    row.Cells[colProjectTaskDtlNameRes.Name].Value = dtl.Name;

                    row.Cells[colResponsibleAccFlagRes.Name].Value = dtl.ResponseFlag == 1 ? "是" : "否";
                    row.Cells[colCostAccFlagRes.Name].Value = dtl.CostingFlag == 1 ? "是" : "否";
                    row.Cells[colSubContractFlagRes.Name].Value = dtl.SubContractFeeFlag == true ? "是" : "否";

                    row.Cells[colContractNameRes.Name].Value = dtl.ContractGroupName;
                    if (dtl.TheCostItem != null)
                    {
                        row.Cells[colCostItemNameRes.Name].Value = dtl.TheCostItem.Name;
                        row.Cells[colCostItemQuotaCodeRes.Name].Value = dtl.TheCostItem.QuotaCode;
                    }
                    row.Cells[colMainResourceNameRes.Name].Value = dtl.MainResourceTypeName;
                    row.Cells[colSpecRes.Name].Value = dtl.MainResourceTypeSpec;
                    row.Cells[colDiagramNumberRes.Name].Value = dtl.DiagramNumber;

                    row.Cells[colContractQuantityRes.Name].Value = dtl.ContractProjectQuantity;
                    row.Cells[colContractPriceRes.Name].Value = dtl.ContractPrice;
                    //row.Cells[colContractPricePercent.Name].Value = dtl.ContractPricePercent;
                    //row.Cells[colContractPriceResult.Name].Value = dtl.ContractPrice * dtl.ContractPricePercent;
                    row.Cells[colContractTotalPriceRes.Name].Value = dtl.ContractTotalPrice;

                    row.Cells[colResponsibleQuantityRes.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[colResponsiblePriceRes.Name].Value = dtl.ResponsibilitilyPrice;
                    //row.Cells[colResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
                    //row.Cells[colResponsiblePriceResult.Name].Value = dtl.ResponsibilitilyPrice * dtl.ResponsiblePricePercent;
                    row.Cells[colResponsibleTotalPriceRes.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    row.Cells[colPlanQuantityRes.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[colPlanPriceRes.Name].Value = dtl.PlanPrice;
                    //row.Cells[colPlanPricePercent.Name].Value = dtl.PlanPricePercent;
                    //row.Cells[colPlanPriceResult.Name].Value = dtl.PlanPrice * dtl.PlanPricePercent;
                    row.Cells[colPlanTotalPriceRes.Name].Value = dtl.PlanTotalPrice;

                    row.Cells[colQuantityUnitRes.Name].Value = dtl.WorkAmountUnitName;
                    row.Cells[colPriceUnitRes.Name].Value = dtl.PriceUnitName;

                    //if (dtl.TheCostItem != null)
                    //    row.Cells[colCostItemBasePrice.Name].Value = dtl.TheCostItem.Price;

                    row.Cells[colTaskDtlStateRes.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);

                    row.Tag = dtl;

                    gridGWBDetailRes.CurrentCell = row.Cells[0];
                    break;
                }
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
        void SetButton()
        {
            if (tabControl1.TabPages.Count > 0)
            {
                if (tabControl1.TabPages[0] == tabPageBG)
                {
                    this.btnAccountCostData.Visible = false;
                   // this.btnSubcontractFeeAcc.Visible = false;
                    this.btnSave.Visible = true;
                    this.btnSaveAndExit.Visible = true;
                    this.btnBack.Visible = false;
                    this.btnResourceSet.Visible = true;
                }
                else if (tabControl1.TabPages[0] == tabPageResource)
                {
                    this.btnAccountCostData.Visible = true;
                   // this.btnSubcontractFeeAcc.Visible = true;
                    this.btnSave.Visible = true;
                    this.btnSaveAndExit.Visible = true;
                    this.btnBack.Visible = true;
                    this.btnResourceSet.Visible = false;
                }
            }
        }
        #endregion

        

    }
}