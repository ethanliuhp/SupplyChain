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
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDetailCostEdit : TBasicDataView
    {
        /// <summary>
        /// 初始任务类型
        /// </summary>
        public TreeNode DefaultGWBSTreeNode = null;

        CurrentProjectInfo projectInfo = null;

        private OptCostType OptionCostType = OptCostType.合同收入;

        private MGWBSTree model = new MGWBSTree();

        Color selectRowBackColor = ColorTranslator.FromHtml("#D7E8FE");
        Color unSelectRowBackColor = ColorTranslator.FromHtml("#FFFFFF");

        public VGWBSDetailCostEdit()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            btnTabPlan_Click(btnTabPlan, new EventArgs());
        }

        private void InitEvents()
        {
            txtCostItemBySearch.LostFocus += new EventHandler(txtCostItemBySearch_LostFocus);

            btnSelectTaskTypeBySearch.Click += new EventHandler(btnSelectGWBSBySearch_Click);
            btnSelectCostItemCate.Click += new EventHandler(btnSelectCostItemCate_Click);
            btnSelectCostItemBySearch.Click += new EventHandler(btnSelectCostItemBySearch_Click);
            btnSelectAccountSubject.Click += new EventHandler(btnSelectAccountSubject_Click);
            btnSelectResourceType.Click += new EventHandler(btnSelectResourceType_Click);

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
            btnSave.Click += new EventHandler(btnSave_Click);
            btnSaveAndExit.Click += new EventHandler(btnSaveAndExit_Click);
            btnGiveup.Click += new EventHandler(btnGiveup_Click);

            gridGWBDetail.CellContentClick += new DataGridViewCellEventHandler(gridGWBDetail_CellContentClick);
            gridGWBDetail.CellMouseDown += new DataGridViewCellMouseEventHandler(gridGWBDetail_CellMouseDown);

            gridGWBDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridGWBDetail_CellValidating);
            gridGWBDetail.CellEndEdit += new DataGridViewCellEventHandler(gridGWBDetail_CellEndEdit);

            gridDtlUsage.CellValidating += new DataGridViewCellValidatingEventHandler(gridDtlUsage_CellValidating);
            gridDtlUsage.CellEndEdit += new DataGridViewCellEventHandler(gridDtlUsage_CellEndEdit);

            gridDtlUsage.CellMouseDown += new DataGridViewCellMouseEventHandler(gridDtlUsage_CellMouseDown);
            gridDtlUsage.CellContentClick += new DataGridViewCellEventHandler(gridDtlUsage_CellContentClick);

            contextMenuQueryElsePriceDtl.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuQueryElsePriceDtl_ItemClicked);
            contextMenuQueryElsePriceDtlUsage.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuQueryElsePriceDtlUsage_ItemClicked);

            this.Load += new EventHandler(VGWBSDetailCostEdit_Load);
        }

        void contextMenuQueryElsePriceDtl_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "查询其他项目单价")
            {
                contextMenuQueryElsePriceDtl.Hide();

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
            }
            else if (e.ClickedItem.Text.Trim() == "批量设置单价")
            {
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
                                dtl.ContractPrice = dtl.ContractPrice * priceValue;

                                row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
                            }
                            else if (OptionCostType == OptCostType.责任成本)
                            {
                                dtl.ResponsibilitilyPrice = dtl.ResponsibilitilyPrice * priceValue;

                                row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
                            }
                            else if (OptionCostType == OptCostType.计划成本)
                            {
                                dtl.PlanPrice = dtl.PlanPrice * priceValue;

                                row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
                            }

                            row.Tag = dtl;
                        }
                    }
                }
            }
        }
        void contextMenuQueryElsePriceDtlUsage_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "查询其他项目单价")
            {
                contextMenuQueryElsePriceDtlUsage.Hide();

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
            }
            else if (e.ClickedItem.Text.Trim() == "批量设置单价")
            {
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
                                subject.ContractQuantityPrice = quantityPriceValue;
                                subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;

                                row.Cells[usageContractQuantityPrice.Name].Value = subject.ContractQuantityPrice;
                                row.Cells[usageContractWorkAmountPrice.Name].Value = subject.ContractPrice;
                            }
                            else if (OptionCostType == OptCostType.责任成本)
                            {
                                subject.ResponsibilitilyPrice = quantityPriceValue;
                                subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;

                                row.Cells[usageResponsibleQuantityPrice.Name].Value = subject.ResponsibilitilyPrice;
                                row.Cells[usageResponsibleWorkAmountPrice.Name].Value = subject.ResponsibleWorkPrice;
                            }
                            else if (OptionCostType == OptCostType.计划成本)
                            {
                                subject.PlanPrice = quantityPriceValue;
                                subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;

                                row.Cells[usagePlanQuantityPrice.Name].Value = subject.PlanPrice;
                                row.Cells[usagePlanWorkAmountPrice.Name].Value = subject.PlanWorkPrice;
                            }

                            row.Tag = subject;
                        }
                    }

                    //未使用，所以projectAmountPriceValue始终为0
                    if (projectAmountPriceValue != 0)
                    {
                        //修改后联动计算【责任数量单价】=【责任工程量单价】，【责任定额数量】=1，【数量计量单位】=所属{工程任务明细}_【工程量计量单位】,合同收入和计划成本算法相似
                        foreach (DataGridViewRow row in gridDtlUsage.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value))
                            {
                                GWBSDetailCostSubject subject = row.Tag as GWBSDetailCostSubject;

                                if (OptionCostType == OptCostType.合同收入)
                                {
                                    subject.ContractPrice = projectAmountPriceValue;
                                    subject.ContractQuantityPrice = subject.ContractPrice;
                                    subject.ContractQuotaQuantity = 1;

                                    subject.ProjectAmountUnitGUID = subject.TheGWBSDetail.WorkAmountUnitGUID;
                                    subject.ProjectAmountUnitName = subject.TheGWBSDetail.WorkAmountUnitName;

                                    row.Cells[usageContractWorkAmountPrice.Name].Value = subject.ContractPrice;
                                    row.Cells[usageContractQuantityPrice.Name].Value = subject.ContractQuantityPrice;
                                    row.Cells[usageContractQuotaNum.Name].Value = subject.ContractQuotaQuantity;
                                }
                                else if (OptionCostType == OptCostType.责任成本)
                                {
                                    subject.ResponsibleWorkPrice = projectAmountPriceValue;
                                    subject.ResponsibilitilyPrice = subject.ResponsibleWorkPrice;
                                    subject.ResponsibleQuotaNum = 1;

                                    subject.ProjectAmountUnitGUID = subject.TheGWBSDetail.WorkAmountUnitGUID;
                                    subject.ProjectAmountUnitName = subject.TheGWBSDetail.WorkAmountUnitName;

                                    row.Cells[usageResponsibleWorkAmountPrice.Name].Value = subject.ResponsibleWorkPrice;
                                    row.Cells[usageResponsibleQuantityPrice.Name].Value = subject.ResponsibilitilyPrice;
                                    row.Cells[usageResponsibleQuotaNum.Name].Value = subject.ResponsibleQuotaNum;
                                }
                                else if (OptionCostType == OptCostType.计划成本)
                                {
                                    subject.PlanWorkPrice = projectAmountPriceValue;
                                    subject.PlanPrice = subject.PlanWorkPrice;
                                    subject.PlanQuotaNum = 1;

                                    subject.ProjectAmountUnitGUID = subject.TheGWBSDetail.WorkAmountUnitGUID;
                                    subject.ProjectAmountUnitName = subject.TheGWBSDetail.WorkAmountUnitName;

                                    row.Cells[usagePlanWorkAmountPrice.Name].Value = subject.PlanWorkPrice;
                                    row.Cells[usagePlanQuantityPrice.Name].Value = subject.PlanPrice;
                                    row.Cells[usagePlanQuotaNum.Name].Value = subject.PlanQuotaNum;
                                }

                                row.Tag = subject;
                            }
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
                                subject.ContractQuantityPrice = subject.ContractQuantityPrice * quantityPriceValue;
                                subject.ContractPrice = subject.ContractQuotaQuantity * subject.ContractQuantityPrice;

                                row.Cells[usageContractQuantityPrice.Name].Value = subject.ContractQuantityPrice;
                                row.Cells[usageContractWorkAmountPrice.Name].Value = subject.ContractPrice;
                            }
                            else if (OptionCostType == OptCostType.责任成本)
                            {
                                subject.ResponsibilitilyPrice = subject.ResponsibilitilyPrice * quantityPriceValue;
                                subject.ResponsibleWorkPrice = subject.ResponsibleQuotaNum * subject.ResponsibilitilyPrice;

                                row.Cells[usageResponsibleQuantityPrice.Name].Value = subject.ResponsibilitilyPrice;
                                row.Cells[usageResponsibleWorkAmountPrice.Name].Value = subject.ResponsibleWorkPrice;
                            }
                            else if (OptionCostType == OptCostType.计划成本)
                            {
                                subject.PlanPrice = subject.PlanPrice * quantityPriceValue;
                                subject.PlanWorkPrice = subject.PlanQuotaNum * subject.PlanPrice;

                                row.Cells[usagePlanQuantityPrice.Name].Value = subject.PlanPrice;
                                row.Cells[usagePlanWorkAmountPrice.Name].Value = subject.PlanWorkPrice;
                            }

                            row.Tag = subject;
                        }
                    }

                    #endregion
                }
            }
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
                    row.Cells[colTaskDtlSelect.Name].Style.BackColor = unSelectRowBackColor;

                SetDtlUsageVisable(row.Tag as GWBSDetail, cbTaskDtlSelect.Checked);

            }

            cbUsageSelect.Checked = cbTaskDtlSelect.Checked;
            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                row.Cells[usageSelect.Name].Value = cbUsageSelect.Checked;

                if (cbUsageSelect.Checked)
                    row.Cells[usageSelect.Name].Style.BackColor = selectRowBackColor;
                else
                    row.Cells[usageSelect.Name].Style.BackColor = unSelectRowBackColor;
            }
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
                    row.Cells[usageSelect.Name].Style.BackColor = unSelectRowBackColor;
            }

            cbTaskDtlSelect.Checked = isChecked;

            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                row.Cells[colTaskDtlSelect.Name].Value = isChecked;

                if (isChecked)
                    row.Cells[colTaskDtlSelect.Name].Style.BackColor = selectRowBackColor;
                else
                    row.Cells[colTaskDtlSelect.Name].Style.BackColor = unSelectRowBackColor;

            }
            cbTaskDtlSelect.CheckedChanged += new EventHandler(cbTaskDtlSelect_CheckedChanged);

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

                        if (isSelect)
                            gridGWBDetail.Rows[e.RowIndex].Cells[colTaskDtlSelect.Name].Style.BackColor = selectRowBackColor;
                        else
                            gridGWBDetail.Rows[e.RowIndex].Cells[colTaskDtlSelect.Name].Style.BackColor = unSelectRowBackColor;


                        GWBSDetail dtl = gridGWBDetail.Rows[e.RowIndex].Tag as GWBSDetail;
                        SetDtlUsageChecked(dtl, isSelect);

                        SetDtlUsageVisable(dtl, isSelect);
                    }
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


                        DataGridViewRow row = gridDtlUsage.Rows[e.RowIndex];
                        GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;

                        if (dtl.ResourceUsageQuota != null)
                        {
                            //和成本项定额数量、单价对比
                            if (colName == usageContractQuotaNum.Name)
                            {
                                if (dtl.ResourceUsageQuota.QuotaProjectAmount != editValue)
                                    row.Cells[usageContractQuotaNum.Name].Style.BackColor = selectRowBackColor;
                                else
                                    row.Cells[usageContractQuotaNum.Name].Style.BackColor = unSelectRowBackColor;
                            }
                            else if (colName == usageResponsibleQuotaNum.Name)
                            {
                                if (dtl.ResourceUsageQuota.QuotaProjectAmount != editValue)
                                    row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                                else
                                    row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = unSelectRowBackColor;
                            }
                            else if (colName == usagePlanQuotaNum.Name)
                            {
                                if (dtl.ResourceUsageQuota.QuotaProjectAmount != editValue)
                                    row.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                                else
                                    row.Cells[usagePlanQuotaNum.Name].Style.BackColor = unSelectRowBackColor;
                            }
                            else if (colName == usageContractQuantityPrice.Name)
                            {
                                if (dtl.ResourceUsageQuota.QuotaPrice != editValue)
                                    row.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                                else
                                    row.Cells[usageContractQuantityPrice.Name].Style.BackColor = unSelectRowBackColor;
                            }
                            else if (colName == usageResponsibleQuantityPrice.Name)
                            {
                                if (dtl.ResourceUsageQuota.QuotaPrice != editValue)
                                    row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                                else
                                    row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = unSelectRowBackColor;
                            }
                            else if (colName == usagePlanQuantityPrice.Name)
                            {
                                if (dtl.ResourceUsageQuota.QuotaPrice != editValue)
                                    row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                                else
                                    row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = unSelectRowBackColor;
                            }
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

                //耗用数据
                if (OptionCostType == OptCostType.合同收入)
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

                        dtl.ContractQuantityPrice = ContractQuantityPrice;

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;//合同耗用数量包括定额数量值，合同工程量单价包括定额数量值，故此处用耗用数量*数量单价，防止重复计算定额数量

                        currEditRow.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
                        currEditRow.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;
                    }
                    //else if (colName == usageContractWorkAmountPrice.Name)
                    //{
                    //    decimal ContractPrice = 0;
                    //    if (!string.IsNullOrEmpty(value))
                    //        ContractPrice = ClientUtil.ToDecimal(value);

                    //    if (dtl.ContractPrice != ContractPrice)
                    //    {
                    //        dtl.ContractPrice = ContractPrice;
                    //        dtl.ContractQuantityPrice = dtl.ContractPrice;
                    //        dtl.ContractQuotaQuantity = 1;
                    //        dtl.ProjectAmountUnitGUID = dtl.TheGWBSDetail.WorkAmountUnitGUID;
                    //        dtl.ProjectAmountUnitName = dtl.TheGWBSDetail.WorkAmountUnitName;

                    //        currEditRow.Cells[usageContractQuantityPrice.Name].Value = dtl.ContractQuantityPrice;
                    //        currEditRow.Cells[usageContractQuotaNum.Name].Value = dtl.ContractQuotaQuantity;
                    //    }
                    //}
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

                        dtl.ResponsibilitilyPrice = ResponsibilitilyPrice;

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyPrice * dtl.ResponsibilitilyWorkAmount;

                        currEditRow.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                        currEditRow.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;
                    }
                    //else if (colName == usageResponsibleWorkAmountPrice.Name)
                    //{
                    //    decimal ResponsibleWorkPrice = 0;
                    //    if (!string.IsNullOrEmpty(value))
                    //        ResponsibleWorkPrice = ClientUtil.ToDecimal(value);

                    //    dtl.ResponsibleWorkPrice = ResponsibleWorkPrice;
                    //    dtl.ResponsibilitilyPrice = dtl.ResponsibleWorkPrice;
                    //    dtl.ResponsibleQuotaNum = 1;
                    //    dtl.ProjectAmountUnitGUID = dtl.TheGWBSDetail.WorkAmountUnitGUID;
                    //    dtl.ProjectAmountUnitName = dtl.TheGWBSDetail.WorkAmountUnitName;

                    //    currEditRow.Cells[usageResponsibleQuantityPrice.Name].Value = dtl.ResponsibilitilyPrice;
                    //    currEditRow.Cells[usageResponsibleQuotaNum.Name].Value = dtl.ResponsibleQuotaNum;
                    //}
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

                        dtl.PlanPrice = PlanPrice;

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;
                        dtl.PlanTotalPrice = dtl.PlanPrice * dtl.PlanWorkAmount;

                        currEditRow.Cells[usagePlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
                        currEditRow.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
                    }
                    //else if (colName == usagePlanWorkAmountPrice.Name)
                    //{
                    //    decimal PlanWorkPrice = 0;
                    //    if (!string.IsNullOrEmpty(value))
                    //        PlanWorkPrice = ClientUtil.ToDecimal(value);

                    //    dtl.PlanWorkPrice = PlanWorkPrice;
                    //    dtl.PlanPrice = dtl.PlanWorkPrice;
                    //    dtl.PlanQuotaNum = 1;
                    //    dtl.ProjectAmountUnitGUID = dtl.TheGWBSDetail.WorkAmountUnitGUID;
                    //    dtl.ProjectAmountUnitName = dtl.TheGWBSDetail.WorkAmountUnitName;

                    //    currEditRow.Cells[usagePlanQuantityPrice.Name].Value = dtl.PlanPrice;
                    //    currEditRow.Cells[usagePlanQuotaNum.Name].Value = dtl.PlanQuotaNum;
                    //}
                }

                currEditRow.Tag = dtl;
            }
        }

        //private decimal DecimalRound(Decimal val)
        //{
        //    return decimal.Round(val, 5);
        //}

        void gridDtlUsage_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                gridDtlUsage.Rows[e.RowIndex].Selected = true;
                gridDtlUsage.CurrentCell = gridDtlUsage.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
                            gridDtlUsage.Rows[e.RowIndex].Cells[usageSelect.Name].Style.BackColor = unSelectRowBackColor;


                        SetTaskDtlChecked(gridDtlUsage.Rows[e.RowIndex], isSelect);
                    }
                }
            }
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
                        row.Cells[usageSelect.Name].Style.BackColor = unSelectRowBackColor;

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
                        row.Cells[colTaskDtlSelect.Name].Style.BackColor = unSelectRowBackColor;

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
                txtResourceTypeBySearch.Text = mat.Name;
                txtResourceTypeBySearch.Tag = mat;
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

                QueryGWBSDetailInGrid();

                cbTaskDtlSelect.Checked = true;
                cbTaskDtlSelect_CheckedChanged(cbTaskDtlSelect, new EventArgs());
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

            if (txtResourceTypeBySearch.Text.Trim() != "")
                mat = txtResourceTypeBySearch.Tag as Material;

            string usageName = txtResourceUsageName.Text.Trim();

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("State", DocumentState.Edit));

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

            if (costItemCate != null)
                oq.AddCriterion(Expression.Eq("TheCostItem.TheCostItemCategory.Id", costItemCate.Id));

            oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);

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
                AddDetailInfoInGrid(dtl);
            }
            gridGWBDetail.ClearSelection();

            //加载明细耗用数据
            if (listTempDtlUsage != null)
            {
                foreach (GWBSDetailCostSubject dtl in listTempDtlUsage)
                {
                    AddDetailUsageInfoInGrid(dtl);
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
                AddDetailInfoInGrid(dtl);
            }
            gridGWBDetail.ClearSelection();

            //加载明细耗用数据
            if (listTempDtlUsage != null)
            {
                foreach (GWBSDetailCostSubject dtl in listTempDtlUsage)
                {
                    AddDetailUsageInfoInGrid(dtl);
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
        }
        void btnTabResponsible_Click(object sender, EventArgs e)
        {
            btnTabContract.ForeColor = SystemColors.ControlText;
            btnTabResponsible.ForeColor = Color.Blue;
            btnTabPlan.ForeColor = SystemColors.ControlText;

            OptionCostType = OptCostType.责任成本;

            SetVisibleColumn();
        }
        void btnTabPlan_Click(object sender, EventArgs e)
        {
            btnTabContract.ForeColor = SystemColors.ControlText;
            btnTabResponsible.ForeColor = SystemColors.ControlText;
            btnTabPlan.ForeColor = Color.Blue;

            OptionCostType = OptCostType.计划成本;

            SetVisibleColumn();
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
            gridGWBDetail.Columns[colContractQuantity.Name].Visible = contractFlag;
            gridGWBDetail.Columns[colContractTotalPrice.Name].Visible = contractFlag;

            gridGWBDetail.Columns[colResponsiblePrice.Name].Visible = responsibleFlag;
            gridGWBDetail.Columns[colResponsibleQuantity.Name].Visible = responsibleFlag;
            gridGWBDetail.Columns[colResponsibleTotalPrice.Name].Visible = responsibleFlag;

            gridGWBDetail.Columns[colPlanPrice.Name].Visible = planFlag;
            gridGWBDetail.Columns[colPlanQuantity.Name].Visible = planFlag;
            gridGWBDetail.Columns[colPlanTotalPrice.Name].Visible = planFlag;



            gridDtlUsage.Columns[usageContractQuotaNum.Name].Visible = contractFlag;
            gridDtlUsage.Columns[usageContractQuantityPrice.Name].Visible = contractFlag;
            gridDtlUsage.Columns[usageContractWorkAmountPrice.Name].Visible = contractFlag;
            gridDtlUsage.Columns[usageContractWorkAmount.Name].Visible = contractFlag;
            gridDtlUsage.Columns[usageContractTotalPrice.Name].Visible = contractFlag;

            gridDtlUsage.Columns[usageResponsibleQuotaNum.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[usageResponsibleQuantityPrice.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[usageResponsibleWorkAmountPrice.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[usageResponsibleQuantity.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[usageResponsibleTotalPrice.Name].Visible = responsibleFlag;

            gridDtlUsage.Columns[usagePlanQuotaNum.Name].Visible = planFlag;
            gridDtlUsage.Columns[usagePlanQuantityPrice.Name].Visible = planFlag;
            gridDtlUsage.Columns[usagePlanWorkAmountPrice.Name].Visible = planFlag;
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
                    dtl.ResponsibilitilyPrice = dtl.PlanPrice;
                    dtl.ResponsibleWorkPrice = dtl.PlanWorkPrice;
                    dtl.ResponsibilitilyWorkAmount = dtl.PlanWorkAmount;
                    dtl.ResponsibilitilyTotalPrice = dtl.PlanTotalPrice;

                    row.Cells[usageResponsibleQuotaNum.Name].Value = dtl.ResponsibleQuotaNum;
                    row.Cells[usageResponsibleQuantityPrice.Name].Value = dtl.ResponsibilitilyPrice;
                    row.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                    row.Cells[usageResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    row.Tag = dtl;
                }
            }
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
                    dtl.PlanPrice = dtl.ResponsibilitilyPrice;
                    dtl.PlanWorkPrice = dtl.ResponsibleWorkPrice;
                    dtl.PlanWorkAmount = dtl.ResponsibilitilyWorkAmount;
                    dtl.PlanTotalPrice = dtl.ResponsibilitilyTotalPrice;

                    row.Cells[usagePlanQuotaNum.Name].Value = dtl.PlanQuotaNum;
                    row.Cells[usagePlanQuantityPrice.Name].Value = dtl.PlanPrice;
                    row.Cells[usagePlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
                    row.Cells[usagePlanQuantity.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

                    row.Tag = dtl;
                }
            }
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

                    GWBSDetail dtl = row.Tag as GWBSDetail;
                    if (dtl.SubContractFeeFlag == false)
                        flag2 = true;
                }

                if (flag && flag2)
                    break;
            }

            if (flag == false)
            {
                MessageBox.Show("请勾选任务明细或明细资源耗用！");
                gridGWBDetail.Focus();
                return;
            }
            if (flag2 == false)
            {
                MessageBox.Show("勾选的任务明细不存在“责任核算明细”或“成本核算明细”,请检查！");
                gridGWBDetail.Focus();
                return;
            }



            foreach (DataGridViewRow row in gridDtlUsage.Rows)
            {
                if (Convert.ToBoolean(row.Cells[usageSelect.Name].Value) == false)
                    continue;

                GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;

                GWBSDetail taskDtl = getTaskDtl(dtl);
                if (taskDtl == null)
                    taskDtl = dtl.TheGWBSDetail;

                if (taskDtl.SubContractFeeFlag == true)//成本计算时不计算分包取费明细的预算量（算法不一致）
                    continue;

                if (OptionCostType == OptCostType.合同收入)
                {
                    //i）如果【成本核算科目】为“人工费”，则【资源合同工程量单价】等于所属{工程任务明细}_【合同单价】减去其余兄弟{工程资源耗用明细}_【资源合同工程量单价】之和；
                    //ii）【资源合同工程量】：=所属{工程任务明细}_【合同工程量】*【合同定额数量】；
                    //iii）【资源合同合价】：=【资源合同工程量】*【资源合同工程量单价】；

                    if (!string.IsNullOrEmpty(dtl.CostAccountSubjectName) && dtl.CostAccountSubjectName.Trim() == "人工费")
                    {
                        decimal brotherProjectAmountPrice = 0;//兄弟合同工程量单价之和
                        foreach (DataGridViewRow rowTemp in gridDtlUsage.Rows)
                        {
                            GWBSDetailCostSubject tempDtl = rowTemp.Tag as GWBSDetailCostSubject;
                            if (tempDtl.TheGWBSDetail.Id == taskDtl.Id && tempDtl.Id != dtl.Id)
                            {
                                brotherProjectAmountPrice += tempDtl.ContractPrice;
                            }
                        }

                        dtl.ContractPrice = taskDtl.ContractPrice - brotherProjectAmountPrice;
                    }

                    dtl.ContractProjectAmount = taskDtl.ContractProjectQuantity * dtl.ContractQuotaQuantity;
                    dtl.ContractTotalPrice = dtl.ContractProjectAmount * dtl.ContractPrice;

                    row.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
                    row.Cells[usageContractWorkAmount.Name].Value = dtl.ContractProjectAmount;
                    row.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;
                }
                else if (OptionCostType == OptCostType.责任成本)
                {
                    //a）针对<操作{工程资源耗用明细}集>在列表框中显示的每一个对象进行如下操作：
                    //【责任耗用数量】：=所属{工程任务明细}_【责任工程量】*【责任定额数量】；
                    //【责任耗用合价】：=【责任耗用数量】*【责任工程量单价】；

                    dtl.ResponsibilitilyWorkAmount = taskDtl.ResponsibilitilyWorkAmount * dtl.ResponsibleQuotaNum;
                    dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                    row.Cells[usageResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;
                }
                else if (OptionCostType == OptCostType.计划成本)
                {
                    //a）针对<操作{工程资源耗用明细}集>在列表框中显示的每一个对象进行如下操作：
                    //【计划耗用数量】：=所属{工程任务明细}_【计划工程量】*【计划定额数量】；
                    //【计划耗用合价】：=【计划耗用数量】*【计划工程量单价】；

                    dtl.PlanWorkAmount = taskDtl.PlanWorkAmount * dtl.PlanQuotaNum;
                    dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                    row.Cells[usagePlanQuantity.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
                }

                row.Tag = dtl;

            }

            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                if (Convert.ToBoolean(row.Cells[colTaskDtlSelect.Name].Value) == false)
                    continue;

                GWBSDetail dtl = row.Tag as GWBSDetail;
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
                        }
                    }
                    dtl.PlanPrice = dtlUsageProjectAmountPrice;
                    dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                    row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
                    row.Cells[colPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
                }

                row.Tag = dtl;
            }
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

                IList listResult = model.AccountSubContractFeeDtl(listSubContractFeeDtl, listSubContractFeeDtlUsage);
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
                listDtl = model.ObjectQuery(typeof(GWBSDetail), oq);
                oq.Criterions.Clear();

                for (int i = 0; i < listDtl.Count; i++)
                {
                    GWBSDetail dtl = listDtl[i] as GWBSDetail;
                    foreach (DataGridViewRow row in gridGWBDetail.Rows)
                    {
                        GWBSDetail currDtl = row.Tag as GWBSDetail;
                        if (dtl.Id == currDtl.Id)
                        {
                            dtl.ContractPrice = currDtl.ContractPrice;
                            dtl.ContractProjectQuantity = currDtl.ContractProjectQuantity;
                            dtl.ContractTotalPrice = currDtl.ContractTotalPrice;

                            dtl.ResponsibilitilyPrice = currDtl.ResponsibilitilyPrice;
                            dtl.ResponsibilitilyWorkAmount = currDtl.ResponsibilitilyWorkAmount;
                            dtl.ResponsibilitilyTotalPrice = currDtl.ResponsibilitilyTotalPrice;

                            dtl.PlanPrice = currDtl.PlanPrice;
                            dtl.PlanWorkAmount = currDtl.PlanWorkAmount;
                            dtl.PlanTotalPrice = currDtl.PlanTotalPrice;
                            break;
                        }
                    }
                }


                foreach (DataGridViewRow row in gridDtlUsage.Rows)
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    dis.Add(Expression.Eq("Id", dtl.Id));
                }
                oq.AddCriterion(dis);
                listDtlUsage = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);

                for (int i = 0; i < listDtlUsage.Count; i++)
                {
                    GWBSDetailCostSubject dtl = listDtlUsage[i] as GWBSDetailCostSubject;
                    foreach (DataGridViewRow row in gridDtlUsage.Rows)
                    {
                        GWBSDetailCostSubject currDtl = row.Tag as GWBSDetailCostSubject;
                        if (dtl.Id == currDtl.Id)
                        {
                            dtl.ContractQuotaQuantity = currDtl.ContractQuotaQuantity;
                            dtl.ContractQuantityPrice = currDtl.ContractQuantityPrice;
                            dtl.ContractPrice = currDtl.ContractPrice;
                            dtl.ContractProjectAmount = currDtl.ContractProjectAmount;
                            dtl.ContractTotalPrice = currDtl.ContractTotalPrice;

                            dtl.ResponsibleQuotaNum = currDtl.ResponsibleQuotaNum;
                            dtl.ResponsibilitilyPrice = currDtl.ResponsibilitilyPrice;
                            dtl.ResponsibleWorkPrice = currDtl.ResponsibleWorkPrice;
                            dtl.ResponsibilitilyWorkAmount = currDtl.ResponsibilitilyWorkAmount;
                            dtl.ResponsibilitilyTotalPrice = currDtl.ResponsibilitilyTotalPrice;

                            dtl.PlanQuotaNum = currDtl.PlanQuotaNum;
                            dtl.PlanPrice = currDtl.PlanPrice;
                            dtl.PlanWorkPrice = currDtl.PlanWorkPrice;
                            dtl.PlanWorkAmount = currDtl.PlanWorkAmount;
                            dtl.PlanTotalPrice = currDtl.PlanTotalPrice;

                            break;
                        }
                    }
                }

                FlashScreen.Show("正在保存,请稍候......");

                IList listResult = model.SaveOrUpdateDetailByCostEdit(listDtl, null, listDtlUsage, new List<string>());
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

        private void AddDetailInfoInGrid(GWBSDetail dtl)
        {
            int index = gridGWBDetail.Rows.Add();
            DataGridViewRow row = gridGWBDetail.Rows[index];

            row.Cells[colProjectTaskName.Name].Value = dtl.TheGWBS.Name;
            row.Cells[colProjectTaskName.Name].ToolTipText = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBS);

            row.Cells[colProjectTaskDtlName.Name].Value = dtl.Name;

            row.Cells[colResponsibleAccFlag.Name].Value = dtl.ResponseFlag == 1 ? "是" : "否";
            row.Cells[colCostAccFlag.Name].Value = dtl.CostingFlag == 1 ? "是" : "否";
            row.Cells[colSubContractFlag.Name].Value = dtl.SubContractFeeFlag == true ? "是" : "否";

            if (dtl.TheCostItem != null)
            {
                row.Cells[colCostItemName.Name].Value = dtl.TheCostItem.Name;
                row.Cells[colCostItemQuotaCode.Name].Value = dtl.TheCostItem.QuotaCode;
            }
            row.Cells[colMainResourceName.Name].Value = dtl.MainResourceTypeName;
            row.Cells[colSpe.Name].Value = dtl.MainResourceTypeSpec;
            row.Cells[colDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Cells[colContractQuantity.Name].Value = dtl.ContractProjectQuantity;
            row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
            row.Cells[colContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

            row.Cells[colResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
            row.Cells[colResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[colPlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
            row.Cells[colPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

            row.Cells[colQuantityUnit.Name].Value = dtl.WorkAmountUnitName;
            row.Cells[colPriceUnit.Name].Value = dtl.PriceUnitName;

            //if (dtl.TheCostItem != null)
            //    row.Cells[colCostItemBasePrice.Name].Value = dtl.TheCostItem.Price;

            row.Tag = dtl;
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

                    if (dtl.TheCostItem != null)
                    {
                        row.Cells[colCostItemName.Name].Value = dtl.TheCostItem.Name;
                        row.Cells[colCostItemQuotaCode.Name].Value = dtl.TheCostItem.QuotaCode;
                    }
                    row.Cells[colMainResourceName.Name].Value = dtl.MainResourceTypeName;
                    row.Cells[colSpe.Name].Value = dtl.MainResourceTypeSpec;
                    row.Cells[colDiagramNumber.Name].Value = dtl.DiagramNumber;

                    row.Cells[colContractQuantity.Name].Value = dtl.ContractProjectQuantity;
                    row.Cells[colContractPrice.Name].Value = dtl.ContractPrice;
                    row.Cells[colContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

                    row.Cells[colResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[colResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
                    row.Cells[colResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    row.Cells[colPlanQuantity.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[colPlanPrice.Name].Value = dtl.PlanPrice;
                    row.Cells[colPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

                    row.Cells[colQuantityUnit.Name].Value = dtl.WorkAmountUnitName;
                    row.Cells[colPriceUnit.Name].Value = dtl.PriceUnitName;

                    //if (dtl.TheCostItem != null)
                    //    row.Cells[colCostItemBasePrice.Name].Value = dtl.TheCostItem.Price;

                    row.Tag = dtl;

                    gridGWBDetail.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }

        private void AddDetailUsageInfoInGrid(GWBSDetailCostSubject dtl)
        {
            int index = gridDtlUsage.Rows.Add();
            DataGridViewRow row = gridDtlUsage.Rows[index];

            row.Cells[usageName.Name].Value = dtl.Name;
            row.Cells[usageResourceType.Name].Value = dtl.ResourceTypeName;
            row.Cells[usageSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[usageDiagramNumber.Name].Value = dtl.DiagramNumber;
            row.Cells[usageAccountSubject.Name].Value = dtl.CostAccountSubjectName;

            row.Cells[usageProjectQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;

            row.Cells[usageContractQuotaNum.Name].Value = dtl.ContractQuotaQuantity;
            row.Cells[usageContractQuantityPrice.Name].Value = dtl.ContractQuantityPrice;
            row.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
            row.Cells[usageContractWorkAmount.Name].Value = dtl.ContractProjectAmount;
            row.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

            row.Cells[usageResponsibleQuotaNum.Name].Value = dtl.ResponsibleQuotaNum;
            row.Cells[usageResponsibleQuantityPrice.Name].Value = dtl.ResponsibilitilyPrice;
            row.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
            row.Cells[usageResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[usagePlanQuotaNum.Name].Value = dtl.PlanQuotaNum;
            row.Cells[usagePlanQuantityPrice.Name].Value = dtl.PlanPrice;
            row.Cells[usagePlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
            row.Cells[usagePlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[usagePlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

            if (dtl.ResourceUsageQuota != null)
            {
                row.Cells[usageCostItemQuotaQuantity.Name].Value = dtl.ResourceUsageQuota.QuotaProjectAmount;
                row.Cells[usageCostItemQuotaPrice.Name].Value = dtl.ResourceUsageQuota.QuotaPrice;


                if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.ContractQuotaQuantity)
                    row.Cells[usageContractQuotaNum.Name].Style.BackColor = selectRowBackColor;
                if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractQuantityPrice)
                    row.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;

                if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.ResponsibleQuotaNum)
                    row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibilitilyPrice)
                    row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;

                if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.PlanQuotaNum)
                    row.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanPrice)
                    row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
            }

            row.Tag = dtl;

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
                    row.Cells[usageAccountSubject.Name].Value = dtl.CostAccountSubjectName;

                    row.Cells[usageProjectQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;

                    row.Cells[usageContractQuotaNum.Name].Value = dtl.ContractQuotaQuantity;
                    row.Cells[usageContractQuantityPrice.Name].Value = dtl.ContractQuantityPrice;
                    row.Cells[usageContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
                    row.Cells[usageContractWorkAmount.Name].Value = dtl.ContractProjectAmount;
                    row.Cells[usageContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

                    row.Cells[usageResponsibleQuotaNum.Name].Value = dtl.ResponsibleQuotaNum;
                    row.Cells[usageResponsibleQuantityPrice.Name].Value = dtl.ResponsibilitilyPrice;
                    row.Cells[usageResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                    row.Cells[usageResponsibleQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[usageResponsibleTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    row.Cells[usagePlanQuotaNum.Name].Value = dtl.PlanQuotaNum;
                    row.Cells[usagePlanQuantityPrice.Name].Value = dtl.PlanPrice;
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
                            row.Cells[usageContractQuotaNum.Name].Style.BackColor = unSelectRowBackColor;

                        if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ContractQuantityPrice)
                            row.Cells[usageContractQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usageContractQuantityPrice.Name].Style.BackColor = unSelectRowBackColor;


                        if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.ResponsibleQuotaNum)
                            row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usageResponsibleQuotaNum.Name].Style.BackColor = unSelectRowBackColor;

                        if (dtl.ResourceUsageQuota.QuotaPrice != dtl.ResponsibilitilyPrice)
                            row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usageResponsibleQuantityPrice.Name].Style.BackColor = unSelectRowBackColor;


                        if (dtl.ResourceUsageQuota.QuotaProjectAmount != dtl.PlanQuotaNum)
                            row.Cells[usagePlanQuotaNum.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usagePlanQuotaNum.Name].Style.BackColor = unSelectRowBackColor;

                        if (dtl.ResourceUsageQuota.QuotaPrice != dtl.PlanPrice)
                            row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = selectRowBackColor;
                        else
                            row.Cells[usagePlanQuantityPrice.Name].Style.BackColor = unSelectRowBackColor;
                    }

                    row.Tag = dtl;
                }
            }

        }
    }

    /// <summary>
    /// 操作成本类型
    /// </summary>
    public enum OptCostType
    {
        合同收入 = 1,
        责任成本 = 2,
        计划成本 = 3
    }
    /// <summary>
    /// 设置单价方式
    /// </summary>
    public enum OptSetPriceType
    {
        直接输入 = 1,
        根据系数调整 = 2
    }
}
