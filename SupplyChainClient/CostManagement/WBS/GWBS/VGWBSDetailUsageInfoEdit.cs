﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Iesi.Collections;
using Iesi.Collections.Generic;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDetailUsageInfoEdit : TBasicDataView
    {
        private OptUsageType _OptionUsageType = OptUsageType.合同耗用;
        /// <summary>
        /// 操作耗用的类型
        /// </summary>
        public OptUsageType OptionUsageType
        {
            get { return _OptionUsageType; }
            set { _OptionUsageType = value; }
        }

        public MainViewState OptionViewState = MainViewState.Modify;

        /// <summary>
        /// 操作任务明细
        /// </summary>
        public GWBSDetail OptionGWBSDtl = null;

        CurrentProjectInfo projectInfo = null;

        public MGWBSTree model = new MGWBSTree();

        private GWBSDetailCostSubject costsub = null;
        private bool costsubCanEdit = false;
        private ComboBox gv_ProfessionalType;

        public VGWBSDetailUsageInfoEdit()
        {
            InitializeComponent();
            InitialForm();
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.老项目)
            {
                this.splitContainer1.Panel2Collapsed = true;
            }

        }
        private void InitialForm()
        {
            InitialEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            DtlMainResourceFlag.Items.Add("是");
            DtlMainResourceFlag.Items.Add("否");
        }
        private void InitialEvents()
        {
            btnAddDetail.Click += new EventHandler(btnAddDetail_Click);
            btnUpdateDetail.Click += new EventHandler(btnUpdateDetail_Click);
            btnDeleteDetail.Click += new EventHandler(btnDeleteDetail_Click);
            btnSaveDetail.Click += new EventHandler(btnSaveDetail_Click);
            btnSaveAndExit.Click += new EventHandler(btnSaveAndExit_Click);
            btnExit.Click += new EventHandler(btnExit_Click);


            gridGWBDetailUsage.CellValidating += new DataGridViewCellValidatingEventHandler(gridGWBDetailUsage_CellValidating);

            gridGWBDetailUsage.CellEndEdit += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellEndEdit);
            gridGWBDetailUsage.CellDoubleClick += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellDoubleClick);
            gridGWBDetailUsage.SelectionChanged += new EventHandler(gridGWBDetailUsage_SelectionChanged);
            gridGWBDetailUsage.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(gridGWBDetailUsage_EditingControlShowing);

            this.gridSelFeeDetial.CellEndEdit += new DataGridViewCellEventHandler(gridSelFeeDetial_CellEndEdit);
            this.Load += new EventHandler(VGWBSDetailUsageInfoEdit_Load);
        }


        void gridGWBDetailUsage_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (this.gridGWBDetailUsage.CurrentCell.RowIndex != -1 && this.gridGWBDetailUsage.CurrentCell.ColumnIndex == this.gridGWBDetailUsage.Columns[this.dtlProfessionalType.Name].Index)
            {
                gv_ProfessionalType = e.Control as ComboBox;
                this.gridGWBDetailUsage.CurrentCell.Value = gv_ProfessionalType.Text;
                gv_ProfessionalType.SelectedIndexChanged -= new EventHandler(gv_ProfessionalType_SelectedIndexChanged);
                gv_ProfessionalType.SelectedIndexChanged += new EventHandler(gv_ProfessionalType_SelectedIndexChanged);
            }
        }

        void gv_ProfessionalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string txt = gv_ProfessionalType.Tag as string;
            if (txt == gv_ProfessionalType.Text) return;
            //if (MessageBox.Show("更改专业类会更改非实体部分成本取费费率，确定要更改吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            //{
            GWBSDetailCostSubject dtl = gridGWBDetailUsage.CurrentRow.Tag as GWBSDetailCostSubject;
            string colName = this.gridGWBDetailUsage.Columns[this.dtlProfessionalType.Name].Name;
            if (colName == dtlProfessionalType.Name)
            {
                dtl.ListGWBSDtlCostSubRate.Clear();
                this.gridSelFeeDetial.Rows.Clear();
                costsub = dtl;
                costsubCanEdit = gridGWBDetailUsage.Rows[gridGWBDetailUsage.CurrentRow.Index].ReadOnly;
                LoadFeeRate(gv_ProfessionalType.Text);
            }
            //}
            //else
            //{
            //    gv_ProfessionalType.SelectedIndexChanged -= new EventHandler(gv_ProfessionalType_SelectedIndexChanged);
            //    gv_ProfessionalType.Text = gv_ProfessionalType.Tag as string ;
            //}
        }



        void gridSelFeeDetial_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridSelFeeDetial.Rows[e.RowIndex].ReadOnly == false)
            {
                SelFeeDtl oSelFeeDtl = gridSelFeeDetial.Rows[e.RowIndex].Tag as SelFeeDtl;


                GWBSDtlCostSubRate costsubRate = GetGWBSDtlCostSubRate(oSelFeeDtl);
                if (costsubRate == null)
                {
                    if (oSelFeeDtl.Rate == ClientUtil.ToDecimal(gridSelFeeDetial[this.colRate.Name, e.RowIndex].Value))
                        return;
                    costsubRate = new GWBSDtlCostSubRate();
                    costsubRate.Master = costsub;
                    costsubRate.Rate = ClientUtil.ToDecimal(gridSelFeeDetial[this.colRate.Name, e.RowIndex].Value);
                    costsubRate.SelFeelDtl = oSelFeeDtl;
                    costsub.ListGWBSDtlCostSubRate.Add(costsubRate);
                    gridSelFeeDetial[this.colRate.Name, e.RowIndex].Style.BackColor = Color.DarkOliveGreen;
                }
                else
                {
                    costsubRate.Rate = ClientUtil.ToDecimal(gridSelFeeDetial[this.colRate.Name, e.RowIndex].Value);
                    gridSelFeeDetial[this.colRate.Name, e.RowIndex].Style.BackColor = Color.DarkOliveGreen;
                }
            }
        }


        void gridGWBDetailUsage_SelectionChanged(object sender, EventArgs e)
        {
            this.gridSelFeeDetial.Rows.Clear();
            if (gv_ProfessionalType != null)
                gv_ProfessionalType.Tag = null;
            if (gridGWBDetailUsage.CurrentRow == null || gridGWBDetailUsage.CurrentRow.Tag == null)
            {
                costsub = new GWBSDetailCostSubject();
            }
            else
            {
                costsub = gridGWBDetailUsage.CurrentRow.Tag as GWBSDetailCostSubject;
            costsubCanEdit = gridGWBDetailUsage.CurrentRow.ReadOnly;
            }
            LoadFeeRate(costsub.ProfessionalType);

        }

        /// <summary>
        /// 加载
        /// </summary>
        private void LoadFeeRate(string strProType)
        {

            if (costsub == null) return;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("SpecialType", strProType));
            oq.AddCriterion(Expression.Eq("ProjectInfo", projectInfo));
            IList list = model.ObjectQuery(typeof(SelFeeDtl), oq);

            foreach (SelFeeDtl oSelFeeDtl in list)
            {
                int iRow = gridSelFeeDetial.Rows.Add();
                gridSelFeeDetial[colSpecialType.Name, iRow].Value = oSelFeeDtl.SpecialType;
                gridSelFeeDetial[this.colAccountSubjectName.Name, iRow].Value = oSelFeeDtl.AccountSubjectName;
                gridSelFeeDetial[this.colAccountSubjectCode.Name, iRow].Value = oSelFeeDtl.AccountSubjectCode;

                GWBSDtlCostSubRate costsubRate = GetGWBSDtlCostSubRate(oSelFeeDtl);
                gridSelFeeDetial[this.colRate.Name, iRow].Value = costsubRate == null ? oSelFeeDtl.Rate : costsubRate.Rate;
                if (costsubRate != null)
                    gridSelFeeDetial[this.colRate.Name, iRow].Style.BackColor = Color.DarkOliveGreen;
                gridSelFeeDetial[this.colRate.Name, iRow].ReadOnly = costsubCanEdit;

                gridSelFeeDetial[this.colBeginMoney.Name, iRow].Value = oSelFeeDtl.BeginMoney;
                gridSelFeeDetial[this.colEndMoney.Name, iRow].Value = oSelFeeDtl.EndMoney;
                gridSelFeeDetial[this.colMainAccSubjectCode.Name, iRow].Value = oSelFeeDtl.MainAccSubjectCode;
                gridSelFeeDetial[this.colMainAccSubjectName.Name, iRow].Value = oSelFeeDtl.MainAccSubjectName;
                gridSelFeeDetial.Rows[iRow].Tag = oSelFeeDtl;
            }


        }

        private GWBSDtlCostSubRate GetGWBSDtlCostSubRate(SelFeeDtl oSelFeeDtl)
        {
            List<GWBSDtlCostSubRate> list_rate = costsub.ListGWBSDtlCostSubRate.OfType<GWBSDtlCostSubRate>().ToList<GWBSDtlCostSubRate>();
            if (list_rate == null)
                return null;
            GWBSDtlCostSubRate costsubRate = list_rate.Find(a => a.SelFeelDtl.Id == oSelFeeDtl.Id);

            return costsubRate;

        }
        void gridGWBDetailUsage_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridGWBDetailUsage.Rows[e.RowIndex].ReadOnly == false)
            {
                //object value = gridGWBDetailUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue;
                object tempValue = e.FormattedValue;
                if (tempValue != null)
                {
                    string value = "";
                    if (tempValue != null)
                        value = tempValue.ToString().Trim();
                    try
                    {
                        string colName = gridGWBDetailUsage.Columns[e.ColumnIndex].Name;
                        GWBSDetailCostSubject quota = gridGWBDetailUsage.Rows[e.RowIndex].Tag as GWBSDetailCostSubject;

                        // 移除主资源有且只能有一个的限制 by zzb 20161222
                        //if (colName == DtlMainResourceFlag.Name)//主资源标志
                        //{
                        //    if (value == "是")
                        //    {
                        //        foreach (DataGridViewRow row in gridGWBDetailUsage.Rows)
                        //        {
                        //            if (row.Index != e.RowIndex && row.Cells[DtlMainResourceFlag.Name].Value.ToString().Trim() == "是")
                        //            {
                        //                MessageBox.Show("主资源类型有且只能有一个！");
                        //                e.Cancel = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}

                        //数据格式校验
                        if (colName == DtlContractQuotaQuantity.Name || colName == DtlContractBasePrice.Name || colName == DtlContractPricePercent.Name
                            || colName == DtlContractWorkAmountPrice.Name
                            || colName == dtlLaborMachineBasePrice.Name
                            || colName == dtlMaterialBasePrice.Name)//合同耗用
                        {
                            if (value.ToString() != "")
                                Convert.ToDecimal(value);
                        }
                        if (colName == DtlResponsibleQuotaQuantity.Name || colName == DtlResponsibleBasePrice.Name || colName == DtlResponsiblePricePercent.Name
                            || colName == DtlResponsibleWorkAmountPrice.Name)//责任耗用
                        {
                            if (value.ToString() != "")
                                Convert.ToDecimal(value);
                        }
                        if (colName == DtlPlanQuotaQuantity.Name || colName == DtlPlanBasePrice.Name || colName == DtlPlanPricePercent.Name
                            || colName == DtlPlanWorkAmountPrice.Name)//计划耗用
                        {
                            if (value.ToString() != "")
                                Convert.ToDecimal(value);
                        }

                        if (colName == DtlQuantityUnit.Name && quota.ProjectAmountUnitName != value)
                        {
                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("Name", value));

                            IList listUnit = model.ObjectQuery(typeof(StandardUnit), oq);
                            if (listUnit.Count == 0)
                            {
                                MessageBox.Show("输入计量单位不存在,请检查！");
                                e.Cancel = true;
                            }
                            else
                            {
                                StandardUnit unit = listUnit[0] as StandardUnit;
                                quota.ProjectAmountUnitGUID = unit;
                                quota.ProjectAmountUnitName = unit.Name;
                            }
                        }

                        if (colName == DtlPriceUnit.Name && quota.PriceUnitName != value)
                        {
                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("Name", value));

                            IList listUnit = model.ObjectQuery(typeof(StandardUnit), oq);
                            if (listUnit.Count == 0)
                            {
                                MessageBox.Show("输入计量单位不存在,请检查！");
                                e.Cancel = true;
                            }
                            else
                            {
                                StandardUnit unit = listUnit[0] as StandardUnit;
                                quota.PriceUnitGUID = unit;
                                quota.PriceUnitName = unit.Name;
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

        void gridGWBDetailUsage_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridGWBDetailUsage.Rows[e.RowIndex].ReadOnly == false)
            {
                object tempValue = gridGWBDetailUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                DataGridViewRow currEditRow = gridGWBDetailUsage.Rows[e.RowIndex];
                GWBSDetailCostSubject dtl = gridGWBDetailUsage.Rows[e.RowIndex].Tag as GWBSDetailCostSubject;

                string colName = gridGWBDetailUsage.Columns[e.ColumnIndex].Name;

                if (colName == DtlUsageName.Name)
                {
                    dtl.Name = value;
                }
                else if (colName == DtlMainResourceFlag.Name)
                {
                    dtl.MainResTypeFlag = value == "是";
                }
                else if (colName == colTechnologyParam.Name)
                {
                    dtl.TechnicalParam = value;
                }
                else if (colName == DtlDiagramNumber.Name)
                {
                    dtl.DiagramNumber = value;
                }
                //耗用数据
                else if (OptionUsageType == OptUsageType.合同耗用)
                {
                    #region 合同耗用
                    if (colName == DtlContractQuotaQuantity.Name)
                    {
                        decimal ContractQuotaQuantity = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractQuotaQuantity = ClientUtil.ToDecimal(value);

                        dtl.ContractQuotaQuantity = ContractQuotaQuantity;

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractProjectAmount = dtl.ContractQuotaQuantity * OptionGWBSDtl.ContractProjectQuantity;
                        dtl.ContractTotalPrice = dtl.ContractProjectAmount * dtl.ContractQuantityPrice;

                        currEditRow.Cells[DtlContractWorkAmountPrice.Name].Value = ToDecimailString(dtl.ContractPrice);
                        currEditRow.Cells[DtlContractUsageQuantity.Name].Value = ToDecimailString(dtl.ContractProjectAmount);
                        currEditRow.Cells[DtlContractUsageTotal.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }
                    else if (colName == DtlContractBasePrice.Name)
                    {
                        decimal ContractBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractBasePrice = ClientUtil.ToDecimal(value);

                        dtl.ContractBasePrice = ContractBasePrice;
                        dtl.ContractQuantityPrice = dtl.ContractBasePrice * dtl.ContractPricePercent;

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;

                        currEditRow.Cells[DtlContractQuantityPrice.Name].Value = ToDecimailString(dtl.ContractQuantityPrice);
                        currEditRow.Cells[DtlContractWorkAmountPrice.Name].Value = ToDecimailString(dtl.ContractPrice);
                        currEditRow.Cells[DtlContractUsageTotal.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }
                    else if (colName == DtlContractPricePercent.Name)
                    {
                        decimal ContractPricePercent = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractPricePercent = ClientUtil.ToDecimal(value);

                        dtl.ContractPricePercent = ContractPricePercent;
                        dtl.ContractQuantityPrice = ContractPricePercent * dtl.ContractBasePrice;

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;

                        currEditRow.Cells[DtlContractQuantityPrice.Name].Value = ToDecimailString(dtl.ContractQuantityPrice);
                        currEditRow.Cells[DtlContractWorkAmountPrice.Name].Value = ToDecimailString(dtl.ContractPrice);
                        currEditRow.Cells[DtlContractUsageTotal.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }
                    else if (colName == DtlContractWorkAmountPrice.Name)
                    {
                        decimal ContractPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractPrice = ClientUtil.ToDecimal(value);

                        if (dtl.ContractPrice != ContractPrice)
                        {
                            dtl.ContractPrice = ContractPrice;
                            dtl.ContractQuantityPrice = dtl.ContractPrice;
                            dtl.ContractQuotaQuantity = 1;

                            dtl.ContractProjectAmount = OptionGWBSDtl.ContractProjectQuantity * dtl.ContractQuotaQuantity;
                            dtl.ContractTotalPrice = dtl.ContractProjectAmount * dtl.ContractQuantityPrice;

                            currEditRow.Cells[DtlContractQuantityPrice.Name].Value = ToDecimailString(dtl.ContractQuantityPrice);
                            currEditRow.Cells[DtlContractQuotaQuantity.Name].Value = ToDecimailString(dtl.ContractQuotaQuantity);
                            currEditRow.Cells[DtlContractUsageQuantity.Name].Value = ToDecimailString(dtl.ContractProjectAmount);
                            currEditRow.Cells[DtlContractUsageTotal.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                        }
                    }
                    else if (colName == DtlContractUsageQuantity.Name)
                    {
                        decimal ContractProjectAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractProjectAmount = ClientUtil.ToDecimal(value);

                        dtl.ContractProjectAmount = ContractProjectAmount;

                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;

                        currEditRow.Cells[DtlContractUsageTotal.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }
                    else if (colName == DtlContractUsageTotal.Name)
                    {
                        decimal ContractTotalPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractTotalPrice = ClientUtil.ToDecimal(value);

                        dtl.ContractTotalPrice = ContractTotalPrice;
                    }
                    else if (colName == dtlLaborMachineBasePrice.Name)
                    {
                        decimal LaborMachineBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            LaborMachineBasePrice = ClientUtil.ToDecimal(value);

                        dtl.LaborMachineBasePrice = LaborMachineBasePrice;
                        currEditRow.Cells[DtlContractQuantityPrice.Name].Value = ToDecimailString(dtl.LaborMachineBasePrice);
                    }
                    else if (colName == dtlMaterialBasePrice.Name)
                    {
                        decimal MaterialBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            MaterialBasePrice = ClientUtil.ToDecimal(value);

                        dtl.MaterialBasePrice = MaterialBasePrice;
                        currEditRow.Cells[dtlMaterialBasePrice.Name].Value = ToDecimailString(dtl.MaterialBasePrice);
                    }
                    else if (colName == dtlProfessionalType.Name)
                    {
                        dtl.ProfessionalType = value;
                    }

                    if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                    {
                        dtl.ContractBasePrice = dtl.LaborMachineBasePrice + dtl.MaterialBasePrice;
                        dtl.ContractQuantityPrice = dtl.ContractBasePrice * dtl.ContractPricePercent;

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;

                        currEditRow.Cells[DtlContractQuantityPrice.Name].Value = ToDecimailString(dtl.ContractQuantityPrice);
                        currEditRow.Cells[DtlContractWorkAmountPrice.Name].Value = ToDecimailString(dtl.ContractPrice);
                        currEditRow.Cells[DtlContractUsageTotal.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                        currEditRow.Cells[DtlContractBasePrice.Name].Value = ToDecimailString(dtl.ContractBasePrice);
                        currEditRow.Cells[DtlContractUsageTotal.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }

                    #endregion
                }
                else if (OptionUsageType == OptUsageType.责任耗用)
                {
                    #region 责任耗用
                    if (colName == DtlResponsibleQuotaQuantity.Name)
                    {
                        decimal ResponsibleQuotaNum = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibleQuotaNum = ClientUtil.ToDecimal(value);

                        dtl.ResponsibleQuotaNum = ResponsibleQuotaNum;

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;
                        dtl.ResponsibilitilyWorkAmount = dtl.ResponsibleQuotaNum * OptionGWBSDtl.ResponsibilitilyWorkAmount;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        currEditRow.Cells[DtlResponsibleWorkAmountPrice.Name].Value = ToDecimailString(dtl.ResponsibleWorkPrice);
                        currEditRow.Cells[DtlResponsibleUsageQuantity.Name].Value = ToDecimailString(dtl.ResponsibilitilyWorkAmount);
                        currEditRow.Cells[DtlResponsibleUsageTotal.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);
                    }
                    else if (colName == DtlResponsibleBasePrice.Name)
                    {
                        decimal ResponsibilitilyBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyBasePrice = ClientUtil.ToDecimal(value);

                        dtl.ResponsibleBasePrice = ResponsibilitilyBasePrice;
                        dtl.ResponsibilitilyPrice = ResponsibilitilyBasePrice * dtl.ResponsiblePricePercent;

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        currEditRow.Cells[DtlResponsibleQuantityPrice.Name].Value = ToDecimailString(dtl.ResponsibilitilyPrice);
                        currEditRow.Cells[DtlResponsibleWorkAmountPrice.Name].Value = ToDecimailString(dtl.ResponsibleWorkPrice);
                        currEditRow.Cells[DtlResponsibleUsageTotal.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);
                    }
                    else if (colName == DtlResponsiblePricePercent.Name)
                    {
                        decimal ResponsibilitilyPricePercent = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyPricePercent = ClientUtil.ToDecimal(value);

                        dtl.ResponsiblePricePercent = ResponsibilitilyPricePercent;
                        dtl.ResponsibilitilyPrice = ResponsibilitilyPricePercent * dtl.ResponsibleBasePrice;

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        currEditRow.Cells[DtlResponsibleQuantityPrice.Name].Value = ToDecimailString(dtl.ResponsibilitilyPrice);
                        currEditRow.Cells[DtlResponsibleWorkAmountPrice.Name].Value = ToDecimailString(dtl.ResponsibleWorkPrice);
                        currEditRow.Cells[DtlResponsibleUsageTotal.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);

                    }
                    else if (colName == DtlResponsibleWorkAmountPrice.Name)
                    {
                        decimal ResponsibleWorkPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibleWorkPrice = ClientUtil.ToDecimal(value);

                        if (dtl.ResponsibleWorkPrice != ResponsibleWorkPrice)
                        {

                            dtl.ResponsibleWorkPrice = ResponsibleWorkPrice;
                            dtl.ResponsibilitilyPrice = dtl.ResponsibleWorkPrice;
                            dtl.ResponsibleQuotaNum = 1;

                            dtl.ResponsibilitilyWorkAmount = OptionGWBSDtl.ResponsibilitilyWorkAmount * dtl.ResponsibleQuotaNum;
                            dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                            currEditRow.Cells[DtlResponsibleQuantityPrice.Name].Value = ToDecimailString(dtl.ResponsibilitilyPrice);
                            currEditRow.Cells[DtlResponsibleQuotaQuantity.Name].Value = ToDecimailString(dtl.ResponsibleQuotaNum);
                            currEditRow.Cells[DtlResponsibleUsageQuantity.Name].Value = ToDecimailString(dtl.ResponsibilitilyWorkAmount);
                            currEditRow.Cells[DtlResponsibleUsageTotal.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);
                        }
                    }
                    else if (colName == DtlResponsibleUsageQuantity.Name)
                    {
                        decimal ResponsibilitilyWorkAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(value);

                        dtl.ResponsibilitilyWorkAmount = ResponsibilitilyWorkAmount;

                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        currEditRow.Cells[DtlResponsibleUsageTotal.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);
                    }
                    else if (colName == DtlResponsibleUsageTotal.Name)
                    {
                        decimal ResponsibilitilyTotalPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyTotalPrice = ClientUtil.ToDecimal(value);

                        dtl.ResponsibilitilyTotalPrice = ResponsibilitilyTotalPrice;
                    }
                    #endregion
                }
                else if (OptionUsageType == OptUsageType.计划耗用)
                {
                    #region 计划耗用
                    if (colName == DtlPlanQuotaQuantity.Name)
                    {
                        decimal PlanQuotaNum = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanQuotaNum = ClientUtil.ToDecimal(value);

                        dtl.PlanQuotaNum = PlanQuotaNum;

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;
                        dtl.PlanWorkAmount = dtl.PlanQuotaNum * OptionGWBSDtl.PlanWorkAmount;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        currEditRow.Cells[DtlPlanWorkAmountPrice.Name].Value = ToDecimailString(dtl.PlanWorkPrice);
                        currEditRow.Cells[DtlPlanUsageQuantity.Name].Value = ToDecimailString(dtl.PlanWorkAmount);
                        currEditRow.Cells[DtlPlanUsageTotal.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                    }
                    else if (colName == DtlPlanBasePrice.Name)
                    {
                        decimal PlanBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanBasePrice = ClientUtil.ToDecimal(value);

                        dtl.PlanBasePrice = PlanBasePrice;
                        dtl.PlanPrice = PlanBasePrice * dtl.PlanPricePercent;

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        currEditRow.Cells[DtlPlanQuantityPrice.Name].Value = ToDecimailString(dtl.PlanPrice);
                        currEditRow.Cells[DtlPlanWorkAmountPrice.Name].Value = ToDecimailString(dtl.PlanWorkPrice);
                        currEditRow.Cells[DtlPlanUsageTotal.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                    }
                    else if (colName == DtlPlanPricePercent.Name)
                    {
                        decimal PlanPricePercent = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanPricePercent = ClientUtil.ToDecimal(value);

                        dtl.PlanPricePercent = PlanPricePercent;
                        dtl.PlanPrice = PlanPricePercent * dtl.PlanBasePrice;

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        currEditRow.Cells[DtlPlanQuantityPrice.Name].Value = ToDecimailString(dtl.PlanPrice);
                        currEditRow.Cells[DtlPlanWorkAmountPrice.Name].Value = ToDecimailString(dtl.PlanWorkPrice);
                        currEditRow.Cells[DtlPlanUsageTotal.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                    }
                    else if (colName == DtlPlanWorkAmountPrice.Name)
                    {
                        decimal PlanWorkPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanWorkPrice = ClientUtil.ToDecimal(value);

                        if (dtl.PlanWorkPrice != PlanWorkPrice)
                        {
                            dtl.PlanWorkPrice = PlanWorkPrice;
                            dtl.PlanPrice = dtl.PlanWorkPrice;
                            dtl.PlanQuotaNum = 1;

                            dtl.PlanWorkAmount = OptionGWBSDtl.PlanWorkAmount * dtl.PlanQuotaNum;
                            dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                            currEditRow.Cells[DtlPlanQuantityPrice.Name].Value = ToDecimailString(dtl.PlanPrice);
                            currEditRow.Cells[DtlPlanQuotaQuantity.Name].Value = ToDecimailString(dtl.PlanQuotaNum);
                            currEditRow.Cells[DtlPlanUsageQuantity.Name].Value = ToDecimailString(dtl.PlanWorkAmount);
                            currEditRow.Cells[DtlPlanUsageTotal.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                        }
                    }
                    else if (colName == DtlPlanUsageQuantity.Name)
                    {
                        decimal PlanWorkAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanWorkAmount = ClientUtil.ToDecimal(value);

                        dtl.PlanWorkAmount = PlanWorkAmount;

                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        currEditRow.Cells[DtlPlanUsageTotal.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                    }
                    else if (colName == DtlPlanUsageTotal.Name)
                    {
                        decimal PlanTotalPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanTotalPrice = ClientUtil.ToDecimal(value);

                        dtl.PlanTotalPrice = PlanTotalPrice;
                    }
                    #endregion
                }


                currEditRow.Tag = dtl;
            }
        }

        private string ToDecimailString(decimal value)
        {
            return decimal.Round(value, 5).ToString();
        }

        void gridGWBDetailUsage_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && gridGWBDetailUsage.Rows[e.RowIndex].ReadOnly == false)
            {
                GWBSDetailCostSubject dtl = gridGWBDetailUsage.Rows[e.RowIndex].Tag as GWBSDetailCostSubject;

                string colName = gridGWBDetailUsage.Columns[e.ColumnIndex].Name;

                if (colName == DtlAccountSubject.Name)//核算科目
                {
                    VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());

                    //设置缺省选择的核算科目
                    frm.DefaultSelectedAccountSubject = dtl.CostAccountSubjectGUID;
                    frm.ShowDialog();

                    if (frm.SelectAccountSubject != null)
                    {
                        dtl.CostAccountSubjectGUID = frm.SelectAccountSubject;
                        dtl.CostAccountSubjectName = frm.SelectAccountSubject.Name;
                        dtl.CostAccountSubjectSyscode = frm.SelectAccountSubject.SysCode;

                        gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlAccountSubject.Name].Value = dtl.CostAccountSubjectName;
                        gridGWBDetailUsage.Rows[e.RowIndex].Tag = dtl;
                    }
                }
                else if (colName == DtlResourceTypeName.Name ||
                    colName == DtlResourceTypeSpec.Name ||
                    colName == DtlResourceTypeQuality.Name)//选择资源类型
                {
                    bool isSelectInCostItemUsageQuota = false;//是否需要从成本项耗用定额的资源组里面选择资源类型

                    ISet<ResourceGroup> listResources = null;
                    if (dtl.ResourceUsageQuota != null)//不是新建的
                    {
                        try
                        {
                            string id = dtl.ResourceUsageQuota.Id;

                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("Id", id));
                            oq.AddFetchMode("ListResources", NHibernate.FetchMode.Eager);
                            IList list = model.ObjectQuery(typeof(SubjectCostQuota), oq);
                            SubjectCostQuota tempSubjectCostQuota = list[0] as SubjectCostQuota;
                            listResources = tempSubjectCostQuota.ListResources;
                        }
                        catch
                        {
                            ObjectQuery oq = new ObjectQuery();
                            oq.AddCriterion(Expression.Eq("Id", dtl.Id));
                            oq.AddFetchMode("ResourceUsageQuota.ListResources", NHibernate.FetchMode.Eager);
                            IList list = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
                            GWBSDetailCostSubject tempAccSubject = list[0] as GWBSDetailCostSubject;
                            listResources = tempAccSubject.ResourceUsageQuota.ListResources;
                        }

                        if (listResources != null && listResources.Count > 0)
                            isSelectInCostItemUsageQuota = true;
                    }

                    if (isSelectInCostItemUsageQuota)
                    {
                        VSelectResourceTypeByUsage frm = new VSelectResourceTypeByUsage();
                        frm.ListResourceGroup = listResources.ToList();
                        frm.DefaultSelectedMaterilId = dtl.ResourceTypeGUID;
                        frm.ShowDialog();

                        if (frm.SelectedMateril != null)
                        {
                            Material mat = frm.SelectedMateril;
                            dtl.ResourceTypeGUID = mat.Id;
                            dtl.ResourceTypeCode = mat.Code;
                            dtl.ResourceTypeName = mat.Name;
                            dtl.ResourceTypeQuality = mat.Quality;
                            dtl.ResourceTypeSpec = mat.Specification;
                            dtl.ResourceCateSyscode = mat.TheSyscode;

                            dtl.ProjectAmountUnitGUID = mat.BasicUnit;
                            dtl.ProjectAmountUnitName = mat.BasicUnit != null ? mat.BasicUnit.Name : "";

                            gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeName.Name].Value = dtl.ResourceTypeName;
                            gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeQuality.Name].Value = dtl.ResourceTypeQuality;
                            gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
                            gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;
                            gridGWBDetailUsage.Rows[e.RowIndex].Tag = dtl;
                        }
                    }
                    else
                    {
                        CommonMaterial materialSelector = new CommonMaterial();
                        materialSelector.OpenSelect();

                        IList list = materialSelector.Result;
                        if (list != null && list.Count > 0)
                        {
                            Material mat = list[0] as Material;
                            dtl.ResourceTypeGUID = mat.Id;
                            dtl.ResourceTypeCode = mat.Code;
                            dtl.ResourceTypeName = mat.Name;
                            dtl.ResourceTypeQuality = mat.Quality;
                            dtl.ResourceTypeSpec = mat.Specification;
                            dtl.ResourceCateSyscode = mat.TheSyscode;

                            dtl.ProjectAmountUnitGUID = mat.BasicUnit;
                            dtl.ProjectAmountUnitName = mat.BasicUnit != null ? mat.BasicUnit.Name : "";

                            gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeName.Name].Value = dtl.ResourceTypeName;
                            gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeQuality.Name].Value = dtl.ResourceTypeQuality;
                            gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
                            gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;
                            gridGWBDetailUsage.Rows[e.RowIndex].Tag = dtl;
                        }
                    }
                }
            }
        }

        void VGWBSDetailUsageInfoEdit_Load(object sender, EventArgs e)
        {
            SetVisibleColumn();

            gbUsage.Text = OptionUsageType + "明细";

            LoadDtlUsageByWBSDetail();

            txtBudgetProjectUnit.Text = OptionGWBSDtl.WorkAmountUnitName;
            txtBudgetPriceUnit.Text = OptionGWBSDtl.PriceUnitName;
            if (OptionGWBSDtl.TheCostItem != null)
                txtQuotaCode.Text = OptionGWBSDtl.TheCostItem.QuotaCode;
            txtTaskDtlName.Text = OptionGWBSDtl.Name;


            foreach (GWBSDetailCostSubject item in OptionGWBSDtl.ListCostSubjectDetails)
            {
                AddTaskDetailResUsageInGrid(item, false, false);
            }
            if (OptionViewState == MainViewState.Browser)
            {
                btnAddDetail.Enabled = false;
                btnUpdateDetail.Enabled = false;
                btnDeleteDetail.Enabled = false;
                btnSaveDetail.Enabled = false;
                btnSaveAndExit.Enabled = false;

                gridGWBDetailUsage.ReadOnly = true;
            }
            gridGWBDetailUsage_SelectionChanged(null, null);
        }

        private void AddUsageDetailInfoInGrid(GWBSDetailCostSubject dtl)
        {
            int index = gridGWBDetailUsage.Rows.Add();
            DataGridViewRow row = gridGWBDetailUsage.Rows[index];
            row.Cells[DtlUsageName.Name].Value = dtl.Name;
            row.Cells[DtlAccountSubject.Name].Value = dtl.CostAccountSubjectName;

            //string matStr = string.IsNullOrEmpty(dtl.ResourceTypeQuality) ? "" : dtl.ResourceTypeQuality + ".";
            //matStr += string.IsNullOrEmpty(dtl.ResourceTypeSpec) ? "" : dtl.ResourceTypeSpec + ".";
            //matStr += dtl.ResourceTypeName;

            row.Cells[DtlResourceTypeName.Name].Value = dtl.ResourceTypeName;
            row.Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[DtlResourceTypeQuality.Name].Value = dtl.ResourceTypeQuality;

            row.Cells[DtlMainResourceFlag.Name].Value = dtl.MainResTypeFlag ? "是" : "否";
            row.Cells[DtlQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[DtlContractQuotaQuantity.Name].Value = dtl.ContractQuotaQuantity;
            row.Cells[DtlContractBasePrice.Name].Value = dtl.ContractBasePrice;
            row.Cells[DtlContractPricePercent.Name].Value = dtl.ContractPricePercent;
            row.Cells[DtlContractQuantityPrice.Name].Value = dtl.ContractQuantityPrice;
            row.Cells[DtlContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
            row.Cells[DtlContractUsageQuantity.Name].Value = dtl.ContractProjectAmount;
            row.Cells[DtlContractUsageTotal.Name].Value = dtl.ContractTotalPrice;

            row.Cells[DtlResponsibleQuotaQuantity.Name].Value = dtl.ResponsibleQuotaNum;
            row.Cells[DtlResponsibleBasePrice.Name].Value = dtl.ResponsibleBasePrice;
            row.Cells[DtlResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
            row.Cells[DtlResponsibleQuantityPrice.Name].Value = dtl.ResponsibilitilyPrice;
            row.Cells[DtlResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
            row.Cells[DtlResponsibleUsageQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[DtlResponsibleUsageTotal.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[DtlPlanQuotaQuantity.Name].Value = dtl.PlanQuotaNum;
            row.Cells[DtlPlanBasePrice.Name].Value = dtl.PlanBasePrice;
            row.Cells[DtlPlanPricePercent.Name].Value = dtl.PlanPricePercent;
            row.Cells[DtlPlanQuantityPrice.Name].Value = dtl.PlanPrice;
            row.Cells[DtlPlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
            row.Cells[DtlPlanUsageQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[DtlPlanUsageTotal.Name].Value = dtl.PlanTotalPrice;

            row.Cells[colTechnologyParam.Name].Value = dtl.TechnicalParam;

            row.Tag = dtl;

            gridGWBDetailUsage.CurrentCell = row.Cells[0];
        }
        private void UpdateUsageDetailInfoInGrid(GWBSDetailCostSubject dtl)
        {
            foreach (DataGridViewRow row in gridGWBDetailUsage.SelectedRows)
            {
                GWBSDetailCostSubject d = row.Tag as GWBSDetailCostSubject;
                if (d.Id == dtl.Id)
                {
                    row.Cells[DtlUsageName.Name].Value = dtl.Name;
                    row.Cells[DtlAccountSubject.Name].Value = dtl.CostAccountSubjectName;

                    //string matStr = string.IsNullOrEmpty(dtl.ResourceTypeQuality) ? "" : dtl.ResourceTypeQuality + ".";
                    //matStr += string.IsNullOrEmpty(dtl.ResourceTypeSpec) ? "" : dtl.ResourceTypeSpec + ".";
                    //matStr += dtl.ResourceTypeName;

                    row.Cells[DtlResourceTypeName.Name].Value = dtl.ResourceTypeName;
                    row.Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
                    row.Cells[DtlResourceTypeQuality.Name].Value = dtl.ResourceTypeQuality;

                    row.Cells[DtlMainResourceFlag.Name].Value = dtl.MainResTypeFlag ? "是" : "否";
                    row.Cells[DtlQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;
                    row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

                    row.Cells[DtlContractQuotaQuantity.Name].Value = dtl.ContractQuotaQuantity;
                    row.Cells[DtlContractBasePrice.Name].Value = dtl.ContractBasePrice;
                    row.Cells[DtlContractPricePercent.Name].Value = dtl.ContractPricePercent;
                    row.Cells[DtlContractQuantityPrice.Name].Value = dtl.ContractQuantityPrice;
                    row.Cells[DtlContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
                    row.Cells[DtlContractUsageQuantity.Name].Value = dtl.ContractProjectAmount;
                    row.Cells[DtlContractUsageTotal.Name].Value = dtl.ContractTotalPrice;

                    row.Cells[DtlResponsibleQuotaQuantity.Name].Value = dtl.ResponsibleQuotaNum;
                    row.Cells[DtlResponsibleBasePrice.Name].Value = dtl.ResponsibleBasePrice;
                    row.Cells[DtlResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
                    row.Cells[DtlResponsibleQuantityPrice.Name].Value = dtl.ResponsibilitilyPrice;
                    row.Cells[DtlResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                    row.Cells[DtlResponsibleUsageQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                    row.Cells[DtlResponsibleUsageTotal.Name].Value = dtl.ResponsibilitilyTotalPrice;

                    row.Cells[DtlPlanQuotaQuantity.Name].Value = dtl.PlanQuotaNum;
                    row.Cells[DtlPlanBasePrice.Name].Value = dtl.PlanBasePrice;
                    row.Cells[DtlPlanPricePercent.Name].Value = dtl.PlanPricePercent;
                    row.Cells[DtlPlanQuantityPrice.Name].Value = dtl.PlanPrice;
                    row.Cells[DtlPlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
                    row.Cells[DtlPlanUsageQuantity.Name].Value = dtl.PlanWorkAmount;
                    row.Cells[DtlPlanUsageTotal.Name].Value = dtl.PlanTotalPrice;

                    row.Cells[colTechnologyParam.Name].Value = dtl.TechnicalParam;

                    row.Tag = dtl;
                    break;
                }
            }
        }

        private void SetVisibleColumn()
        {
            bool contractUsageFlag = false;
            bool responsibleUsageFlag = false;
            bool planUsageFlag = false;
            if (OptionUsageType == OptUsageType.合同耗用)
                contractUsageFlag = true;
            else if (OptionUsageType == OptUsageType.责任耗用)
                responsibleUsageFlag = true;
            else if (OptionUsageType == OptUsageType.计划耗用)
                planUsageFlag = true;

            gridGWBDetailUsage.Columns[DtlContractBasePrice.Name].Visible = contractUsageFlag;
            gridGWBDetailUsage.Columns[DtlContractPricePercent.Name].Visible = contractUsageFlag;
            gridGWBDetailUsage.Columns[DtlContractQuantityPrice.Name].Visible = contractUsageFlag;
            gridGWBDetailUsage.Columns[DtlContractQuotaQuantity.Name].Visible = contractUsageFlag;
            gridGWBDetailUsage.Columns[DtlContractUsageQuantity.Name].Visible = contractUsageFlag;
            gridGWBDetailUsage.Columns[DtlContractUsageTotal.Name].Visible = contractUsageFlag;
            //gridGWBDetailUsage.Columns[DtlContractWorkAmountPrice.Name].Visible = contractUsageFlag;

            gridGWBDetailUsage.Columns[DtlResponsibleBasePrice.Name].Visible = responsibleUsageFlag;
            gridGWBDetailUsage.Columns[DtlResponsiblePricePercent.Name].Visible = responsibleUsageFlag;
            gridGWBDetailUsage.Columns[DtlResponsibleQuantityPrice.Name].Visible = responsibleUsageFlag;
            gridGWBDetailUsage.Columns[DtlResponsibleQuotaQuantity.Name].Visible = responsibleUsageFlag;
            gridGWBDetailUsage.Columns[DtlResponsibleUsageQuantity.Name].Visible = responsibleUsageFlag;
            gridGWBDetailUsage.Columns[DtlResponsibleUsageTotal.Name].Visible = responsibleUsageFlag;
            //gridGWBDetailUsage.Columns[DtlResponsibleWorkAmountPrice.Name].Visible = responsibleUsageFlag;

            gridGWBDetailUsage.Columns[DtlPlanBasePrice.Name].Visible = planUsageFlag;
            gridGWBDetailUsage.Columns[DtlPlanPricePercent.Name].Visible = planUsageFlag;
            gridGWBDetailUsage.Columns[DtlPlanQuantityPrice.Name].Visible = planUsageFlag;
            gridGWBDetailUsage.Columns[DtlPlanQuotaQuantity.Name].Visible = planUsageFlag;
            gridGWBDetailUsage.Columns[DtlPlanUsageQuantity.Name].Visible = planUsageFlag;
            gridGWBDetailUsage.Columns[DtlPlanUsageTotal.Name].Visible = planUsageFlag;
            //gridGWBDetailUsage.Columns[DtlPlanWorkAmountPrice.Name].Visible = planUsageFlag;

            //如果是新项目显示：“取费基准单价”，“非取费基准单价”，“专业类型”
            if (OptionUsageType == OptUsageType.合同耗用 && projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
            {
                gridGWBDetailUsage.Columns[dtlLaborMachineBasePrice.Name].Visible = true;
                gridGWBDetailUsage.Columns[dtlMaterialBasePrice.Name].Visible = true;
                gridGWBDetailUsage.Columns[dtlProfessionalType.Name].Visible = true;
                gridGWBDetailUsage.Columns[DtlContractBasePrice.Name].Visible = false;
                List<BasicDataOptr> lst = VBasicDataOptr.GetSelFeeTemplateSpecialType();
                lst.Insert(0, new BasicDataOptr() { BasicName = "" });
                this.dtlProfessionalType.DataSource = lst;
                this.dtlProfessionalType.DisplayMember = "BasicName";
                this.dtlProfessionalType.ValueMember = "BasicName";
            }
            else//如果是老项目显示：“合同基准单价”，注：【合同基准单价】=【取费基准单价】+【非取费基准单价】
            {
                gridGWBDetailUsage.Columns[dtlLaborMachineBasePrice.Name].Visible = false;
                gridGWBDetailUsage.Columns[dtlMaterialBasePrice.Name].Visible = false;
                gridGWBDetailUsage.Columns[dtlProfessionalType.Name].Visible = false;
                gridGWBDetailUsage.Columns[DtlContractBasePrice.Name].Visible = true;
            }
        }

        /// <summary>
        /// 设置需要启用或禁用的单元格
        /// </summary>
        /// <param name="rowIndex"></param>
        private void SetEnabledColumn(int rowIndex)
        {
            //合同工程量单价：当所属{工程任务明细}_【成本核算标志】=1时为直接控制值
            //责任工程量单价：当所属{工程任务明细}_【责任核算标志】=1时为直接控制值
            //计划工程量单价：当所属{工程任务明细}_【成本核算标志】=1时为直接控制值
            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlContractWorkAmountPrice.Name].ReadOnly = OptionGWBSDtl.CostingFlag == 0;
            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlResponsibleWorkAmountPrice.Name].ReadOnly = OptionGWBSDtl.ResponseFlag == 0;
            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlPlanWorkAmountPrice.Name].ReadOnly = OptionGWBSDtl.CostingFlag == 0;

            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlContractUsageQuantity.Name].ReadOnly = OptionGWBSDtl.CostingFlag == 0;
            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlResponsibleUsageQuantity.Name].ReadOnly = OptionGWBSDtl.ResponseFlag == 0;
            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlPlanUsageQuantity.Name].ReadOnly = OptionGWBSDtl.CostingFlag == 0;

            //双击选择值的列
            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlAccountSubject.Name].ReadOnly = true;
            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlResourceTypeName.Name].ReadOnly = true;
            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlResourceTypeQuality.Name].ReadOnly = true;
            gridGWBDetailUsage.Rows[rowIndex].Cells[DtlResourceTypeSpec.Name].ReadOnly = true;
        }

        //新增
        void btnAddDetail_Click(object sender, EventArgs e)
        {
            GWBSDetailCostSubject item = new GWBSDetailCostSubject();
            if (projectInfo != null)
            {
                item.TheProjectGUID = projectInfo.Id;
                item.TheProjectName = projectInfo.Name;
            }
            item.TheGWBSDetail = OptionGWBSDtl;

            if (OptionGWBSDtl.ListCostSubjectDetails != null && OptionGWBSDtl.ListCostSubjectDetails.Count > 0)
            {
                GWBSDetailCostSubject tempUsage = OptionGWBSDtl.ListCostSubjectDetails.ElementAt(0) as GWBSDetailCostSubject;

                item.ProjectAmountUnitGUID = tempUsage.ProjectAmountUnitGUID;
                item.ProjectAmountUnitName = tempUsage.ProjectAmountUnitName;

                item.PriceUnitGUID = tempUsage.PriceUnitGUID;
                item.PriceUnitName = tempUsage.PriceUnitName;
            }

            if (item.PriceUnitGUID == null)
            {
                item.PriceUnitGUID = OptionGWBSDtl.PriceUnitGUID;
                item.PriceUnitName = OptionGWBSDtl.PriceUnitName;
            }

            item.TheGWBSTree = OptionGWBSDtl.TheGWBS;
            item.TheGWBSTreeName = OptionGWBSDtl.TheGWBS.Name;
            item.TheGWBSTreeSyscode = OptionGWBSDtl.TheGWBS.SysCode;

            item.State = GWBSDetailCostSubjectState.编制;

            AddTaskDetailResUsageInGrid(item, true, false);

            SetEnabledColumn(gridGWBDetailUsage.Rows.Count - 1);

            gridGWBDetailUsage.BeginEdit(false);
        }
        private void AddTaskDetailResUsageInGrid(GWBSDetailCostSubject item, bool isSetCurrentCell, bool isReadOnly)
        {
            gridGWBDetailUsage.SelectionChanged -= new EventHandler(gridGWBDetailUsage_SelectionChanged);

            int index = gridGWBDetailUsage.Rows.Add();
            DataGridViewRow row = gridGWBDetailUsage.Rows[index];
            if (string.IsNullOrEmpty(item.Name))
            {
                row.Cells[DtlUsageName.Name].Value = "耗用名称";
            }
            else
            {
                row.Cells[DtlUsageName.Name].Value = item.Name;
            }
            row.Cells[DtlAccountSubject.Name].Value = item.CostAccountSubjectName;

            //string matStr = string.IsNullOrEmpty(dtl.ResourceTypeQuality) ? "" : dtl.ResourceTypeQuality + ".";
            //matStr += string.IsNullOrEmpty(dtl.ResourceTypeSpec) ? "" : dtl.ResourceTypeSpec + ".";
            //matStr += dtl.ResourceTypeName;

            row.Cells[DtlResourceTypeName.Name].Value = item.ResourceTypeName;
            row.Cells[DtlResourceTypeSpec.Name].Value = item.ResourceTypeSpec;
            row.Cells[DtlResourceTypeQuality.Name].Value = item.ResourceTypeQuality;
            row.Cells[DtlDiagramNumber.Name].Value = item.DiagramNumber;

            row.Cells[DtlMainResourceFlag.Name].Value = item.MainResTypeFlag ? "是" : "否";
            row.Cells[DtlQuantityUnit.Name].Value = item.ProjectAmountUnitName;
            row.Cells[DtlPriceUnit.Name].Value = item.PriceUnitName;

            row.Cells[DtlContractQuotaQuantity.Name].Value = item.ContractQuotaQuantity;
            row.Cells[DtlContractBasePrice.Name].Value = item.ContractBasePrice;

            row.Cells[dtlLaborMachineBasePrice.Name].Value = item.LaborMachineBasePrice;
            row.Cells[dtlMaterialBasePrice.Name].Value = item.MaterialBasePrice;
            row.Cells[dtlProfessionalType.Name].Value = item.ProfessionalType;

            row.Cells[DtlContractPricePercent.Name].Value = item.ContractPricePercent;
            row.Cells[DtlContractQuantityPrice.Name].Value = item.ContractQuantityPrice;
            row.Cells[DtlContractWorkAmountPrice.Name].Value = item.ContractPrice;
            row.Cells[DtlContractUsageQuantity.Name].Value = item.ContractProjectAmount;
            row.Cells[DtlContractUsageTotal.Name].Value = item.ContractTotalPrice;

            row.Cells[DtlResponsibleQuotaQuantity.Name].Value = item.ResponsibleQuotaNum;
            row.Cells[DtlResponsibleBasePrice.Name].Value = item.ResponsibleBasePrice;
            row.Cells[DtlResponsiblePricePercent.Name].Value = item.ResponsiblePricePercent;
            row.Cells[DtlResponsibleQuantityPrice.Name].Value = item.ResponsibilitilyPrice;
            row.Cells[DtlResponsibleWorkAmountPrice.Name].Value = item.ResponsibleWorkPrice;
            row.Cells[DtlResponsibleUsageQuantity.Name].Value = item.ResponsibilitilyWorkAmount;
            row.Cells[DtlResponsibleUsageTotal.Name].Value = item.ResponsibilitilyTotalPrice;

            row.Cells[DtlPlanQuotaQuantity.Name].Value = item.PlanQuotaNum;
            row.Cells[DtlPlanBasePrice.Name].Value = item.PlanBasePrice;
            row.Cells[DtlPlanPricePercent.Name].Value = item.PlanPricePercent;
            row.Cells[DtlPlanQuantityPrice.Name].Value = item.PlanPrice;
            row.Cells[DtlPlanWorkAmountPrice.Name].Value = item.PlanWorkPrice;
            row.Cells[DtlPlanUsageQuantity.Name].Value = item.PlanWorkAmount;
            row.Cells[DtlPlanUsageTotal.Name].Value = item.PlanTotalPrice;

            row.Cells[colTechnologyParam.Name].Value = item.TechnicalParam;

            row.Tag = item;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridGWBDetailUsage.CurrentCell = row.Cells[DtlUsageName.Name];

            gridGWBDetailUsage.SelectionChanged += new EventHandler(gridGWBDetailUsage_SelectionChanged);
        }

        //修改
        void btnUpdateDetail_Click(object sender, EventArgs e)
        {
            if (gridGWBDetailUsage.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridGWBDetailUsage.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }

            int rowIndex = gridGWBDetailUsage.SelectedRows[0].Index;
            int colIndex = 0;
            if (gridGWBDetailUsage.CurrentCell != null)
                colIndex = gridGWBDetailUsage.CurrentCell.ColumnIndex;

            GWBSDetailCostSubject dtl = gridGWBDetailUsage.SelectedRows[0].Tag as GWBSDetailCostSubject;


            gridGWBDetailUsage.SelectedRows[0].ReadOnly = false;
            gridGWBDetailUsage.SelectedRows[0].Selected = false;
            SetEnabledColumn(rowIndex);
            gridGWBDetailUsage.CurrentCell = gridGWBDetailUsage.Rows[rowIndex].Cells[colIndex];
            gridGWBDetailUsage.BeginEdit(false);
        }
        //删除
        void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            if (gridGWBDetailUsage.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的行！");
                return;
            }
            try
            {

                IList list = new List<GWBSDetailCostSubject>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridGWBDetailUsage.SelectedRows)
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    if (!string.IsNullOrEmpty(dtl.Id))
                    {
                        list.Add(dtl);
                    }
                    listRowIndex.Add(row.Index);
                }

                if (MessageBox.Show("删除后不能恢复，您确认要删除选择的耗用明细吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                if (list.Count > 0)
                {
                    //model.DeleteCostSubject(list);

                    for (int i = list.Count - 1; i > -1; i--)
                    {
                        GWBSDetailCostSubject item = list[i] as GWBSDetailCostSubject;

                        OptionGWBSDtl.ListCostSubjectDetails.Remove(item);

                        if (item.MainResTypeFlag)
                        {
                            OptionGWBSDtl.MainResourceTypeId = null;
                            OptionGWBSDtl.MainResourceTypeName = null;
                            OptionGWBSDtl.MainResourceTypeQuality = null;
                            OptionGWBSDtl.MainResourceTypeSpec = null;
                            OptionGWBSDtl.DiagramNumber = null;
                        }
                    }

                    IList listResult = model.SaveOrUpdateDetail(OptionGWBSDtl, null);
                    OptionGWBSDtl = listResult[0] as GWBSDetail;
                }

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridGWBDetailUsage.Rows.RemoveAt(listRowIndex[i]);
                }

                gridGWBDetailUsage.ClearSelection();

            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //保存
        void btnSaveDetail_Click(object sender, EventArgs e)
        {
            if (ValidateSaveDetail())
                MessageBox.Show("保存成功！");
        }
        //保存并退出
        void btnSaveAndExit_Click(object sender, EventArgs e)
        {
            if (ValidateSaveDetail())
                this.Close();
        }
        //退出
        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateSaveDetail()
        {
            try
            {
                //数据校验 
                if (!Verify())
                    return false;

                //主资源标志有且只有一个,此处只校验存在主资源标志（不允许多个主资源已在单元格结束编辑事件中做了校验）
                //bool mainResourceFlag = false;
                //foreach (DataGridViewRow row in gridGWBDetailUsage.Rows)
                //{
                //    GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                //    if (item.MainResTypeFlag)
                //    {
                //        mainResourceFlag = true;
                //        break;
                //    }
                //}
                //if (mainResourceFlag == false)
                //{
                //    MessageBox.Show("请设置一个主资源！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    gridGWBDetailUsage.Focus();
                //    return false;
                //}

                LoadDtlUsageByWBSDetail();

                List<int> listRowIndex = new List<int>();
                List<int> listDetailIndex = new List<int>();
                DateTime serverTime = model.GetServerTime();
                foreach (DataGridViewRow row in gridGWBDetailUsage.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        int detailIndex = 0;

                        GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                        if (!string.IsNullOrEmpty(dtl.Id))
                        {
                            for (int i = 0; i < OptionGWBSDtl.ListCostSubjectDetails.Count; i++)
                            {
                                GWBSDetailCostSubject temp = OptionGWBSDtl.ListCostSubjectDetails.ElementAt(i);
                                if (temp.Id == dtl.Id)
                                {
                                    temp.Name = dtl.Name;
                                    temp.CostAccountSubjectGUID = dtl.CostAccountSubjectGUID;
                                    temp.CostAccountSubjectName = dtl.CostAccountSubjectName;
                                    temp.CostAccountSubjectSyscode = dtl.CostAccountSubjectSyscode;

                                    temp.ResourceTypeGUID = dtl.ResourceTypeGUID;
                                    temp.ResourceTypeCode = dtl.ResourceTypeCode;
                                    temp.ResourceTypeName = dtl.ResourceTypeName;
                                    temp.ResourceTypeQuality = dtl.ResourceTypeQuality;
                                    temp.ResourceTypeSpec = dtl.ResourceTypeSpec;
                                    temp.ResourceCateSyscode = dtl.ResourceCateSyscode;
                                    temp.DiagramNumber = dtl.DiagramNumber;
                                    temp.MainResTypeFlag = dtl.MainResTypeFlag;

                                    temp.ProjectAmountUnitGUID = dtl.ProjectAmountUnitGUID;
                                    temp.ProjectAmountUnitName = dtl.ProjectAmountUnitName;
                                    temp.PriceUnitGUID = dtl.PriceUnitGUID;
                                    temp.PriceUnitName = dtl.PriceUnitName;

                                    if (OptionUsageType == OptUsageType.合同耗用)
                                    {
                                        temp.ContractQuotaQuantity = dtl.ContractQuotaQuantity;
                                        temp.ContractBasePrice = dtl.ContractBasePrice;
                                        temp.ContractPricePercent = dtl.ContractPricePercent;
                                        temp.ContractQuantityPrice = dtl.ContractQuantityPrice;
                                        temp.ContractPrice = dtl.ContractPrice;
                                        temp.ContractProjectAmount = dtl.ContractProjectAmount;
                                        temp.ContractTotalPrice = dtl.ContractTotalPrice;

                                        temp.LaborMachineBasePrice = dtl.LaborMachineBasePrice;
                                        temp.MaterialBasePrice = dtl.MaterialBasePrice;
                                        temp.ProfessionalType = dtl.ProfessionalType;
                                    }
                                    else if (OptionUsageType == OptUsageType.责任耗用)
                                    {
                                        temp.ResponsibleQuotaNum = dtl.ResponsibleQuotaNum;
                                        temp.ResponsibleBasePrice = dtl.ResponsibleBasePrice;
                                        temp.ResponsiblePricePercent = dtl.ResponsiblePricePercent;
                                        temp.ResponsibilitilyPrice = dtl.ResponsibilitilyPrice;
                                        temp.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount;
                                        temp.ResponsibleWorkPrice = dtl.ResponsibleWorkPrice;
                                        temp.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyTotalPrice;
                                    }
                                    else if (OptionUsageType == OptUsageType.计划耗用)
                                    {
                                        temp.PlanQuotaNum = dtl.PlanQuotaNum;
                                        temp.PlanBasePrice = dtl.PlanBasePrice;
                                        temp.PlanPricePercent = dtl.PlanPricePercent;
                                        temp.PlanPrice = dtl.PlanPrice;
                                        temp.PlanWorkPrice = dtl.PlanWorkPrice;
                                        temp.PlanWorkAmount = dtl.PlanWorkAmount;
                                        temp.PlanTotalPrice = dtl.PlanTotalPrice;
                                    }

                                    temp.UpdateTime = serverTime;

                                    detailIndex = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            dtl.TheGWBSDetail = OptionGWBSDtl;

                            OptionGWBSDtl.ListCostSubjectDetails.Add(dtl);

                            detailIndex = OptionGWBSDtl.ListCostSubjectDetails.Count - 1;
                        }

                        listRowIndex.Add(row.Index);

                        listDetailIndex.Add(detailIndex);
                    }
                }

                if (listRowIndex.Count == 0)
                {
                    return true;
                }

                bool mainFlag = false;
                foreach (DataGridViewRow row in gridGWBDetailUsage.Rows)
                {
                    GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                    if (item.MainResTypeFlag)
                    {

                        OptionGWBSDtl.MainResourceTypeId = item.ResourceTypeGUID;
                        OptionGWBSDtl.MainResourceTypeName = item.ResourceTypeName;
                        OptionGWBSDtl.MainResourceTypeQuality = item.ResourceTypeQuality;
                        OptionGWBSDtl.MainResourceTypeSpec = item.ResourceTypeSpec;

                        OptionGWBSDtl.DiagramNumber = item.DiagramNumber;

                        mainFlag = true;
                        break;
                    }
                }
                if (mainFlag == false)
                {
                    OptionGWBSDtl.MainResourceTypeId = null;
                    OptionGWBSDtl.MainResourceTypeName = null;
                    OptionGWBSDtl.MainResourceTypeQuality = null;
                    OptionGWBSDtl.MainResourceTypeSpec = null;
                    OptionGWBSDtl.DiagramNumber = null;
                }

                if (!string.IsNullOrEmpty(OptionGWBSDtl.Id))
                {
                    //IList listResult = model.SaveOrUpdateDetail(OptionGWBSDtl, null);
                    //OptionGWBSDtl = listResult[0] as GWBSDetail;
                }

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridGWBDetailUsage.Rows[rowIndex].Tag = OptionGWBSDtl.ListCostSubjectDetails.ElementAt(listDetailIndex[i]);
                    gridGWBDetailUsage.Rows[rowIndex].ReadOnly = true;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
        }
        //数据校验
        bool Verify()
        {
            foreach (DataGridViewRow row in gridGWBDetailUsage.Rows)
            {
                if (!row.ReadOnly)
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    if (dtl.CostAccountSubjectGUID == null)
                    {
                        MessageBox.Show("请选择资源耗用“" + dtl.Name + "”的核算科目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage.CurrentCell = row.Cells[DtlAccountSubject.Name];
                        return false;
                    }
                    if (dtl.ResourceTypeGUID == null)
                    {
                        MessageBox.Show("请选择资源耗用“" + dtl.Name + "”的资源！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage.CurrentCell = row.Cells[this.DtlResourceTypeName.Name];
                        return false;
                    }

                    if (OptionUsageType == OptUsageType.合同耗用)
                    {
                        if (dtl.ContractQuotaQuantity <= 0)
                        {
                            MessageBox.Show("请输入资源耗用“" + dtl.Name + "”的合同定额数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            gridGWBDetailUsage.CurrentCell = row.Cells[DtlContractQuotaQuantity.Name];
                            return false;
                        }
                    }
                    else if (OptionUsageType == OptUsageType.责任耗用)
                    {
                        if (dtl.ResponsibleQuotaNum <= 0)
                        {
                            MessageBox.Show("请输入资源耗用“" + dtl.Name + "”的责任定额数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            gridGWBDetailUsage.CurrentCell = row.Cells[DtlResponsibleQuotaQuantity.Name];
                            return false;
                        }
                    }
                    else if (OptionUsageType == OptUsageType.计划耗用)
                    {
                        if (dtl.PlanQuotaNum <= 0)
                        {
                            MessageBox.Show("请输入资源耗用“" + dtl.Name + "”的计划定额数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            gridGWBDetailUsage.CurrentCell = row.Cells[DtlPlanQuotaQuantity.Name];
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 加载工程任务明细耗用
        /// </summary>
        /// <returns></returns>
        private void LoadDtlUsageByWBSDetail()
        {
            try
            {
                int dtlCount = OptionGWBSDtl.ListCostSubjectDetails.Count;//未加载时加载
            }
            catch
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", OptionGWBSDtl.Id));
                oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("ListCostSubjectDetails.ListGWBSDtlCostSubRate", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                OptionGWBSDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;
            }
        }
    }

    /// <summary>
    /// 操作耗用的类型
    /// </summary>
    public enum OptUsageType
    {
        合同耗用 = 1,
        责任耗用 = 2,
        计划耗用 = 3
    }
}