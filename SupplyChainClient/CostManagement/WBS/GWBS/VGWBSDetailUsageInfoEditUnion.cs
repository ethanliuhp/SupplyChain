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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDetailUsageInfoEditUnion : TBasicDataView
    {


        public MainViewState OptionViewState = MainViewState.Modify;

        /// <summary>
        /// 操作任务明细
        /// </summary>
        public GWBSDetail OptionGWBSDtl_HT = null;

        /// <summary>
        /// 操作任务明细
        /// </summary>
        public GWBSDetail OptionGWBSDtl_ZR = null;

        /// <summary>
        /// 操作任务明细
        /// </summary>
        public GWBSDetail OptionGWBSDtl_JH = null;



        CurrentProjectInfo projectInfo = null;

        public MGWBSTree model = new MGWBSTree();

        public VGWBSDetailUsageInfoEditUnion()
        {
            InitializeComponent();
            InitialForm();
        }
        private void InitialForm()
        {
            InitialEvents();

            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            //如果是新项目显示：“取费基准单价”，“非取费基准单价”，“专业类型”
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
            {
                //gridGWBDetailUsage_HT.ColumnAdded
            }
            else//如果是老项目显示：“合同基准单价”，注：【合同基准单价】=【取费基准单价】+【非取费基准单价】
            {

            }

            DtlMainResourceFlag_HT.Items.Add("是");
            DtlMainResourceFlag_HT.Items.Add("否");

            DtlMainResourceFlag_ZR.Items.Add("是");
            DtlMainResourceFlag_ZR.Items.Add("否");

            DtlMainResourceFlag_JH.Items.Add("是");
            DtlMainResourceFlag_JH.Items.Add("否");
        }
        private void InitialEvents()
        {

            btnSaveDetail.Click += new EventHandler(btnSaveDetail_Click);
            btnSaveAndExit.Click += new EventHandler(btnSaveAndExit_Click);
            btnExit.Click += new EventHandler(btnExit_Click);


            gridGWBDetailUsage_HT.CellValidating += new DataGridViewCellValidatingEventHandler(gridGWBDetailUsage_CellValidating);
            gridGWBDetailUsage_ZR.CellValidating += new DataGridViewCellValidatingEventHandler(gridGWBDetailUsage_CellValidating);
            gridGWBDetailUsage_JH.CellValidating += new DataGridViewCellValidatingEventHandler(gridGWBDetailUsage_CellValidating);

            gridGWBDetailUsage_HT.CellEndEdit += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellEndEdit);
            gridGWBDetailUsage_ZR.CellEndEdit += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellEndEdit);
            gridGWBDetailUsage_JH.CellEndEdit += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellEndEdit);

            gridGWBDetailUsage_HT.CellDoubleClick += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellDoubleClick);
            gridGWBDetailUsage_ZR.CellDoubleClick += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellDoubleClick);
            gridGWBDetailUsage_JH.CellDoubleClick += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellDoubleClick);

            this.Load += new EventHandler(VGWBSDetailUsageInfoEdit_Load);
        }

        void gridGWBDetailUsage_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridGWBDetailUsage = sender as VirtualMachine.Component.WinControls.Controls.CustomDataGridView;
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridGWBDetailUsage.Rows[e.RowIndex].ReadOnly == false)
            {
                //object value = gridGWBDetailUsage_HT.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue;
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
                        //if ((gridGWBDetailUsage.Name == "gridGWBDetailUsage_HT" &&  colName == DtlMainResourceFlag_HT.Name)||
                        //    (gridGWBDetailUsage.Name == "gridGWBDetailUsage_ZR" &&  colName == DtlMainResourceFlag_ZR.Name)||
                        //    (gridGWBDetailUsage.Name == "gridGWBDetailUsage_JH" &&  colName == DtlMainResourceFlag_JH.Name))       //主资源标志
                        //{
                        //    if (value == "是")
                        //    {
                        //        foreach (DataGridViewRow row in gridGWBDetailUsage.Rows)
                        //        {
                        //            if ((gridGWBDetailUsage.Name == "gridGWBDetailUsage_HT" && row.Index != e.RowIndex && row.Cells[DtlMainResourceFlag_HT.Name].Value.ToString().Trim() == "是")||
                        //                (gridGWBDetailUsage.Name == "gridGWBDetailUsage_ZR" && row.Index != e.RowIndex && row.Cells[DtlMainResourceFlag_ZR.Name].Value.ToString().Trim() == "是")||
                        //                (gridGWBDetailUsage.Name == "gridGWBDetailUsage_JH" && row.Index != e.RowIndex && row.Cells[DtlMainResourceFlag_JH.Name].Value.ToString().Trim() == "是"))
                        //            {
                        //                MessageBox.Show("主资源类型有且只能有一个！");
                        //                e.Cancel = true;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}

                        //数据格式校验
                        if (gridGWBDetailUsage.Name == "gridGWBDetailUsage_HT" && (colName == DtlContractQuotaQuantity_HT.Name || colName == DtlContractBasePrice_HT.Name || colName == DtlContractPricePercent_HT.Name
                            || colName == DtlContractWorkAmountPrice_HT.Name
                            || colName == dtlLaborMachineBasePrice_HT.Name
                            || colName == dtlMaterialBasePrice_HT.Name))//合同耗用
                        {
                            if (value.ToString() != "")
                                Convert.ToDecimal(value);
                        }
                        if (gridGWBDetailUsage.Name == "gridGWBDetailUsage_ZR" && (colName == DtlResponsibleQuotaQuantity_ZR.Name || colName == DtlResponsibleBasePrice_ZR.Name || colName == DtlResponsiblePricePercent_ZR.Name
                            || colName == DtlResponsibleWorkAmountPrice_ZR.Name))//责任耗用
                        {
                            if (value.ToString() != "")
                                Convert.ToDecimal(value);
                        }
                        if (gridGWBDetailUsage.Name == "gridGWBDetailUsage_JH" && (colName == DtlPlanQuotaQuantity_JH.Name || colName == DtlPlanBasePrice_JH.Name || colName == DtlPlanPricePercent_JH.Name
                            || colName == DtlPlanWorkAmountPrice_JH.Name))//计划耗用
                        {
                            if (value.ToString() != "")
                                Convert.ToDecimal(value);
                        }

                        if (((gridGWBDetailUsage.Name == "gridGWBDetailUsage_HT" && colName == DtlQuantityUnit_HT.Name) ||
                            (gridGWBDetailUsage.Name == "gridGWBDetailUsage_ZR" && colName == DtlQuantityUnit_ZR.Name) ||
                            (gridGWBDetailUsage.Name == "gridGWBDetailUsage_JH" && colName == DtlQuantityUnit_HT.Name))
                            && quota.ProjectAmountUnitName != value)
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

                        if (((gridGWBDetailUsage.Name == "gridGWBDetailUsage_HT" && colName == DtlPriceUnit_HT.Name) ||
                            (gridGWBDetailUsage.Name == "gridGWBDetailUsage_ZR" && colName == DtlPriceUnit_ZR.Name) ||
                            (gridGWBDetailUsage.Name == "gridGWBDetailUsage_JH" && colName == DtlPriceUnit_JH.Name))
                            && quota.PriceUnitName != value)
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
            VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridGWBDetailUsage = sender as VirtualMachine.Component.WinControls.Controls.CustomDataGridView;
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridGWBDetailUsage.Rows[e.RowIndex].ReadOnly == false)
            {
                object tempValue = gridGWBDetailUsage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();

                DataGridViewRow currEditRow = gridGWBDetailUsage.Rows[e.RowIndex];
                GWBSDetailCostSubject dtl = gridGWBDetailUsage.Rows[e.RowIndex].Tag as GWBSDetailCostSubject;

                string colName = gridGWBDetailUsage.Columns[e.ColumnIndex].Name;

                if (colName == DtlUsageName_HT.Name || colName == DtlUsageName_ZR.Name || colName == DtlUsageName_JH.Name)
                {
                    dtl.Name = value;
                }
                else if (colName == DtlMainResourceFlag_HT.Name || colName == DtlMainResourceFlag_ZR.Name || colName == DtlMainResourceFlag_JH.Name)
                {
                    dtl.MainResTypeFlag = value == "是";
                }
                else if (colName == colTechnologyParam_HT.Name || colName == colTechnologyParam_ZR.Name || colName == colTechnologyParam_JH.Name)
                {
                    dtl.TechnicalParam = value;
                }
                else if (colName == DtlDiagramNumber_HT.Name || colName == DtlDiagramNumber_ZR.Name || colName == DtlDiagramNumber_JH.Name)
                {
                    dtl.DiagramNumber = value;
                }
                //耗用数据
                else if (gridGWBDetailUsage.Name == "gridGWBDetailUsage_HT")
                {
                    #region 合同耗用
                    if (colName == DtlContractQuotaQuantity_HT.Name)
                    {
                        decimal ContractQuotaQuantity = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractQuotaQuantity = ClientUtil.ToDecimal(value);

                        dtl.ContractQuotaQuantity = ContractQuotaQuantity;

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractProjectAmount = dtl.ContractQuotaQuantity * OptionGWBSDtl_HT.ContractProjectQuantity;
                        dtl.ContractTotalPrice = dtl.ContractProjectAmount * dtl.ContractQuantityPrice;

                        currEditRow.Cells[DtlContractWorkAmountPrice_HT.Name].Value = ToDecimailString(dtl.ContractPrice);
                        currEditRow.Cells[DtlContractUsageQuantity_HT.Name].Value = ToDecimailString(dtl.ContractProjectAmount);
                        currEditRow.Cells[DtlContractUsageTotal_HT.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }
                    else if (colName == DtlContractBasePrice_HT.Name)
                    {
                        decimal ContractBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractBasePrice = ClientUtil.ToDecimal(value);

                        dtl.ContractBasePrice = ContractBasePrice;
                        dtl.ContractQuantityPrice = dtl.ContractBasePrice * dtl.ContractPricePercent;

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;

                        currEditRow.Cells[DtlContractQuantityPrice_HT.Name].Value = ToDecimailString(dtl.ContractQuantityPrice);
                        currEditRow.Cells[DtlContractWorkAmountPrice_HT.Name].Value = ToDecimailString(dtl.ContractPrice);
                        currEditRow.Cells[DtlContractUsageTotal_HT.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }
                    else if (colName == DtlContractPricePercent_HT.Name)
                    {
                        decimal ContractPricePercent = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractPricePercent = ClientUtil.ToDecimal(value);

                        dtl.ContractPricePercent = ContractPricePercent;
                        dtl.ContractQuantityPrice = ContractPricePercent * dtl.ContractBasePrice;

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;

                        currEditRow.Cells[DtlContractQuantityPrice_HT.Name].Value = ToDecimailString(dtl.ContractQuantityPrice);
                        currEditRow.Cells[DtlContractWorkAmountPrice_HT.Name].Value = ToDecimailString(dtl.ContractPrice);
                        currEditRow.Cells[DtlContractUsageTotal_HT.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }
                    else if (colName == DtlContractWorkAmountPrice_HT.Name)
                    {
                        decimal ContractPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractPrice = ClientUtil.ToDecimal(value);

                        if (dtl.ContractPrice != ContractPrice)
                        {
                            dtl.ContractPrice = ContractPrice;
                            dtl.ContractQuantityPrice = dtl.ContractPrice;
                            dtl.ContractQuotaQuantity = 1;

                            dtl.ContractProjectAmount = OptionGWBSDtl_HT.ContractProjectQuantity * dtl.ContractQuotaQuantity;
                            dtl.ContractTotalPrice = dtl.ContractProjectAmount * dtl.ContractQuantityPrice;

                            currEditRow.Cells[DtlContractQuantityPrice_HT.Name].Value = ToDecimailString(dtl.ContractQuantityPrice);
                            currEditRow.Cells[DtlContractQuotaQuantity_HT.Name].Value = ToDecimailString(dtl.ContractQuotaQuantity);
                            currEditRow.Cells[DtlContractUsageQuantity_HT.Name].Value = ToDecimailString(dtl.ContractProjectAmount);
                            currEditRow.Cells[DtlContractUsageTotal_HT.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                        }
                    }
                    else if (colName == DtlContractUsageQuantity_HT.Name)
                    {
                        decimal ContractProjectAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractProjectAmount = ClientUtil.ToDecimal(value);

                        dtl.ContractProjectAmount = ContractProjectAmount;

                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;

                        currEditRow.Cells[DtlContractUsageTotal_HT.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }
                    else if (colName == DtlContractUsageTotal_HT.Name)
                    {
                        decimal ContractTotalPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ContractTotalPrice = ClientUtil.ToDecimal(value);

                        dtl.ContractTotalPrice = ContractTotalPrice;
                    }
                    else if (colName == dtlLaborMachineBasePrice_HT.Name)
                    {
                        decimal LaborMachineBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            LaborMachineBasePrice = ClientUtil.ToDecimal(value);

                        dtl.LaborMachineBasePrice = LaborMachineBasePrice;
                        currEditRow.Cells[DtlContractQuantityPrice_HT.Name].Value = ToDecimailString(dtl.LaborMachineBasePrice);
                    }
                    else if (colName == dtlMaterialBasePrice_HT.Name)
                    {
                        decimal MaterialBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            MaterialBasePrice = ClientUtil.ToDecimal(value);

                        dtl.MaterialBasePrice = MaterialBasePrice;
                        currEditRow.Cells[dtlMaterialBasePrice_HT.Name].Value = ToDecimailString(dtl.MaterialBasePrice);
                    }
                    else if (colName == dtlProfessionalType_HT.Name)
                    {
                        dtl.ProfessionalType = value;
                    }

                    if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                    {
                        dtl.ContractBasePrice = dtl.LaborMachineBasePrice + dtl.MaterialBasePrice;
                        dtl.ContractQuantityPrice = dtl.ContractBasePrice * dtl.ContractPricePercent;

                        dtl.ContractPrice = dtl.ContractQuotaQuantity * dtl.ContractQuantityPrice;
                        dtl.ContractTotalPrice = dtl.ContractQuantityPrice * dtl.ContractProjectAmount;

                        currEditRow.Cells[DtlContractQuantityPrice_HT.Name].Value = ToDecimailString(dtl.ContractQuantityPrice);
                        currEditRow.Cells[DtlContractWorkAmountPrice_HT.Name].Value = ToDecimailString(dtl.ContractPrice);
                        currEditRow.Cells[DtlContractUsageTotal_HT.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                        currEditRow.Cells[DtlContractBasePrice_HT.Name].Value = ToDecimailString(dtl.ContractBasePrice);
                        currEditRow.Cells[DtlContractUsageTotal_HT.Name].Value = ToDecimailString(dtl.ContractTotalPrice);
                    }
                    #endregion
                }
                else if (gridGWBDetailUsage.Name == "gridGWBDetailUsage_ZR")
                {
                    #region 责任耗用
                    if (colName == DtlResponsibleQuotaQuantity_ZR.Name)
                    {
                        decimal ResponsibleQuotaNum = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibleQuotaNum = ClientUtil.ToDecimal(value);

                        dtl.ResponsibleQuotaNum = ResponsibleQuotaNum;

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;
                        dtl.ResponsibilitilyWorkAmount = dtl.ResponsibleQuotaNum * OptionGWBSDtl_ZR.ResponsibilitilyWorkAmount;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        currEditRow.Cells[DtlResponsibleWorkAmountPrice_ZR.Name].Value = ToDecimailString(dtl.ResponsibleWorkPrice);
                        currEditRow.Cells[DtlResponsibleUsageQuantity_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyWorkAmount);
                        currEditRow.Cells[DtlResponsibleUsageTotal_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);
                    }
                    else if (colName == DtlResponsibleBasePrice_ZR.Name)
                    {
                        decimal ResponsibilitilyBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyBasePrice = ClientUtil.ToDecimal(value);

                        dtl.ResponsibleBasePrice = ResponsibilitilyBasePrice;
                        dtl.ResponsibilitilyPrice = ResponsibilitilyBasePrice * dtl.ResponsiblePricePercent;

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        currEditRow.Cells[DtlResponsibleQuantityPrice_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyPrice);
                        currEditRow.Cells[DtlResponsibleWorkAmountPrice_ZR.Name].Value = ToDecimailString(dtl.ResponsibleWorkPrice);
                        currEditRow.Cells[DtlResponsibleUsageTotal_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);
                    }
                    else if (colName == DtlResponsiblePricePercent_ZR.Name)
                    {
                        decimal ResponsibilitilyPricePercent = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyPricePercent = ClientUtil.ToDecimal(value);

                        dtl.ResponsiblePricePercent = ResponsibilitilyPricePercent;
                        dtl.ResponsibilitilyPrice = ResponsibilitilyPricePercent * dtl.ResponsibleBasePrice;

                        dtl.ResponsibleWorkPrice = dtl.ResponsibleQuotaNum * dtl.ResponsibilitilyPrice;
                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        currEditRow.Cells[DtlResponsibleQuantityPrice_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyPrice);
                        currEditRow.Cells[DtlResponsibleWorkAmountPrice_ZR.Name].Value = ToDecimailString(dtl.ResponsibleWorkPrice);
                        currEditRow.Cells[DtlResponsibleUsageTotal_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);

                    }
                    else if (colName == DtlResponsibleWorkAmountPrice_ZR.Name)
                    {
                        decimal ResponsibleWorkPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibleWorkPrice = ClientUtil.ToDecimal(value);

                        if (dtl.ResponsibleWorkPrice != ResponsibleWorkPrice)
                        {

                            dtl.ResponsibleWorkPrice = ResponsibleWorkPrice;
                            dtl.ResponsibilitilyPrice = dtl.ResponsibleWorkPrice;
                            dtl.ResponsibleQuotaNum = 1;

                            dtl.ResponsibilitilyWorkAmount = OptionGWBSDtl_ZR.ResponsibilitilyWorkAmount * dtl.ResponsibleQuotaNum;
                            dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                            currEditRow.Cells[DtlResponsibleQuantityPrice_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyPrice);
                            currEditRow.Cells[DtlResponsibleQuotaQuantity_ZR.Name].Value = ToDecimailString(dtl.ResponsibleQuotaNum);
                            currEditRow.Cells[DtlResponsibleUsageQuantity_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyWorkAmount);
                            currEditRow.Cells[DtlResponsibleUsageTotal_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);
                        }
                    }
                    else if (colName == DtlResponsibleUsageQuantity_ZR.Name)
                    {
                        decimal ResponsibilitilyWorkAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyWorkAmount = ClientUtil.ToDecimal(value);

                        dtl.ResponsibilitilyWorkAmount = ResponsibilitilyWorkAmount;

                        dtl.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyWorkAmount * dtl.ResponsibilitilyPrice;

                        currEditRow.Cells[DtlResponsibleUsageTotal_ZR.Name].Value = ToDecimailString(dtl.ResponsibilitilyTotalPrice);
                    }
                    else if (colName == DtlResponsibleUsageTotal_ZR.Name)
                    {
                        decimal ResponsibilitilyTotalPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            ResponsibilitilyTotalPrice = ClientUtil.ToDecimal(value);

                        dtl.ResponsibilitilyTotalPrice = ResponsibilitilyTotalPrice;
                    }
                    #endregion
                }
                else if (gridGWBDetailUsage.Name == "gridGWBDetailUsage_JH")
                {
                    #region 计划耗用
                    if (colName == DtlPlanQuotaQuantity_JH.Name)
                    {
                        decimal PlanQuotaNum = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanQuotaNum = ClientUtil.ToDecimal(value);

                        dtl.PlanQuotaNum = PlanQuotaNum;

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;
                        dtl.PlanWorkAmount = dtl.PlanQuotaNum * OptionGWBSDtl_JH.PlanWorkAmount;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        currEditRow.Cells[DtlPlanWorkAmountPrice_JH.Name].Value = ToDecimailString(dtl.PlanWorkPrice);
                        currEditRow.Cells[DtlPlanUsageQuantity_JH.Name].Value = ToDecimailString(dtl.PlanWorkAmount);
                        currEditRow.Cells[DtlPlanUsageTotal_JH.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                    }
                    else if (colName == DtlPlanBasePrice_JH.Name)
                    {
                        decimal PlanBasePrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanBasePrice = ClientUtil.ToDecimal(value);

                        dtl.PlanBasePrice = PlanBasePrice;
                        dtl.PlanPrice = PlanBasePrice * dtl.PlanPricePercent;

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        currEditRow.Cells[DtlPlanQuantityPrice_JH.Name].Value = ToDecimailString(dtl.PlanPrice);
                        currEditRow.Cells[DtlPlanWorkAmountPrice_JH.Name].Value = ToDecimailString(dtl.PlanWorkPrice);
                        currEditRow.Cells[DtlPlanUsageTotal_JH.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                    }
                    else if (colName == DtlPlanPricePercent_JH.Name)
                    {
                        decimal PlanPricePercent = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanPricePercent = ClientUtil.ToDecimal(value);

                        dtl.PlanPricePercent = PlanPricePercent;
                        dtl.PlanPrice = PlanPricePercent * dtl.PlanBasePrice;

                        dtl.PlanWorkPrice = dtl.PlanQuotaNum * dtl.PlanPrice;
                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        currEditRow.Cells[DtlPlanQuantityPrice_JH.Name].Value = ToDecimailString(dtl.PlanPrice);
                        currEditRow.Cells[DtlPlanWorkAmountPrice_JH.Name].Value = ToDecimailString(dtl.PlanWorkPrice);
                        currEditRow.Cells[DtlPlanUsageTotal_JH.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                    }
                    else if (colName == DtlPlanWorkAmountPrice_JH.Name)
                    {
                        decimal PlanWorkPrice = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanWorkPrice = ClientUtil.ToDecimal(value);

                        if (dtl.PlanWorkPrice != PlanWorkPrice)
                        {
                            dtl.PlanWorkPrice = PlanWorkPrice;
                            dtl.PlanPrice = dtl.PlanWorkPrice;
                            dtl.PlanQuotaNum = 1;

                            dtl.PlanWorkAmount = OptionGWBSDtl_JH.PlanWorkAmount * dtl.PlanQuotaNum;
                            dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                            currEditRow.Cells[DtlPlanQuantityPrice_JH.Name].Value = ToDecimailString(dtl.PlanPrice);
                            currEditRow.Cells[DtlPlanQuotaQuantity_JH.Name].Value = ToDecimailString(dtl.PlanQuotaNum);
                            currEditRow.Cells[DtlPlanUsageQuantity_JH.Name].Value = ToDecimailString(dtl.PlanWorkAmount);
                            currEditRow.Cells[DtlPlanUsageTotal_JH.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                        }
                    }
                    else if (colName == DtlPlanUsageQuantity_JH.Name)
                    {
                        decimal PlanWorkAmount = 0;
                        if (!string.IsNullOrEmpty(value))
                            PlanWorkAmount = ClientUtil.ToDecimal(value);

                        dtl.PlanWorkAmount = PlanWorkAmount;

                        dtl.PlanTotalPrice = dtl.PlanWorkAmount * dtl.PlanPrice;

                        currEditRow.Cells[DtlPlanUsageTotal_JH.Name].Value = ToDecimailString(dtl.PlanTotalPrice);
                    }
                    else if (colName == DtlPlanUsageTotal_JH.Name)
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
            VirtualMachine.Component.WinControls.Controls.CustomDataGridView gridGWBDetailUsage = sender as VirtualMachine.Component.WinControls.Controls.CustomDataGridView;
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && gridGWBDetailUsage.Rows[e.RowIndex].ReadOnly == false)
            {
                GWBSDetailCostSubject dtl = gridGWBDetailUsage.Rows[e.RowIndex].Tag as GWBSDetailCostSubject;

                string colName = gridGWBDetailUsage.Columns[e.ColumnIndex].Name;

                if (colName == DtlAccountSubject_HT.Name)//核算科目
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

                        switch (gridGWBDetailUsage.Name)
                        {
                            case "gridGWBDetailUsage_HT":
                                gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlAccountSubject_HT.Name].Value = dtl.CostAccountSubjectName;
                                break;
                            case "gridGWBDetailUsage_ZR":
                                gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlAccountSubject_ZR.Name].Value = dtl.CostAccountSubjectName;
                                break;
                            case "gridGWBDetailUsage_JH":
                                gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlAccountSubject_JH.Name].Value = dtl.CostAccountSubjectName;
                                break;
                        }

                        gridGWBDetailUsage.Rows[e.RowIndex].Tag = dtl;
                    }
                }
                else if (colName == DtlResourceTypeName_HT.Name ||
                    colName == DtlResourceTypeSpec_HT.Name ||
                    colName == DtlResourceTypeQuality_HT.Name)//选择资源类型
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

                            switch (gridGWBDetailUsage.Name)
                            {
                                case "gridGWBDetailUsage_HT":
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeName_HT.Name].Value = dtl.ResourceTypeName;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeQuality_HT.Name].Value = dtl.ResourceTypeQuality;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeSpec_HT.Name].Value = dtl.ResourceTypeSpec;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlQuantityUnit_HT.Name].Value = dtl.ProjectAmountUnitName;
                                    break;
                                case "gridGWBDetailUsage_ZR":
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeName_ZR.Name].Value = dtl.ResourceTypeName;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeQuality_ZR.Name].Value = dtl.ResourceTypeQuality;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeSpec_ZR.Name].Value = dtl.ResourceTypeSpec;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlQuantityUnit_ZR.Name].Value = dtl.ProjectAmountUnitName;
                                    break;
                                case "gridGWBDetailUsage_JH":
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeName_JH.Name].Value = dtl.ResourceTypeName;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeQuality_JH.Name].Value = dtl.ResourceTypeQuality;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeSpec_JH.Name].Value = dtl.ResourceTypeSpec;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlQuantityUnit_JH.Name].Value = dtl.ProjectAmountUnitName;
                                    break;
                            }

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

                            switch (gridGWBDetailUsage.Name)
                            {
                                case "gridGWBDetailUsage_HT":
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeName_HT.Name].Value = dtl.ResourceTypeName;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeQuality_HT.Name].Value = dtl.ResourceTypeQuality;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeSpec_HT.Name].Value = dtl.ResourceTypeSpec;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlQuantityUnit_HT.Name].Value = dtl.ProjectAmountUnitName;
                                    break;
                                case "gridGWBDetailUsage_ZR":
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeName_ZR.Name].Value = dtl.ResourceTypeName;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeQuality_ZR.Name].Value = dtl.ResourceTypeQuality;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeSpec_ZR.Name].Value = dtl.ResourceTypeSpec;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlQuantityUnit_ZR.Name].Value = dtl.ProjectAmountUnitName;
                                    break;
                                case "gridGWBDetailUsage_JH":
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeName_JH.Name].Value = dtl.ResourceTypeName;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeQuality_JH.Name].Value = dtl.ResourceTypeQuality;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlResourceTypeSpec_JH.Name].Value = dtl.ResourceTypeSpec;
                                    gridGWBDetailUsage.Rows[e.RowIndex].Cells[DtlQuantityUnit_JH.Name].Value = dtl.ProjectAmountUnitName;
                                    break;
                            }

                            gridGWBDetailUsage.Rows[e.RowIndex].Tag = dtl;
                        }
                    }
                }
            }
        }

        void VGWBSDetailUsageInfoEdit_Load(object sender, EventArgs e)
        {
            SetVisibleColumn();

            //gbUsage_HT.Text = OptionUsageType + "明细";

            LoadDtlUsageByWBSDetail();

            txtBudgetProjectUnit.Text = OptionGWBSDtl_HT.WorkAmountUnitName;
            txtBudgetPriceUnit.Text = OptionGWBSDtl_HT.PriceUnitName;
            if (OptionGWBSDtl_HT.TheCostItem != null)
                txtQuotaCode.Text = OptionGWBSDtl_HT.TheCostItem.QuotaCode;
            txtTaskDtlName.Text = OptionGWBSDtl_HT.Name;


            foreach (GWBSDetailCostSubject item in OptionGWBSDtl_HT.ListCostSubjectDetails)
            {
                AddTaskDetailResUsageInGrid("HT", item, false, false);
            }
            foreach (GWBSDetailCostSubject item in OptionGWBSDtl_ZR.ListCostSubjectDetails)
            {
                AddTaskDetailResUsageInGrid("ZR", item, false, false);
            }
            foreach (GWBSDetailCostSubject item in OptionGWBSDtl_JH.ListCostSubjectDetails)
            {
                AddTaskDetailResUsageInGrid("JH", item, false, false);
            }
            if (OptionViewState == MainViewState.Browser)
            {
                //btnAddDetail.Enabled = false;
                //btnUpdateDetail.Enabled = false;
                //btnDeleteDetail.Enabled = false;
                btnSaveDetail.Enabled = false;
                btnSaveAndExit.Enabled = false;

                gridGWBDetailUsage_ZR.ReadOnly = true;
                gridGWBDetailUsage_HT.ReadOnly = true;
                gridGWBDetailUsage_JH.ReadOnly = true;

            }
        }


        private void SetVisibleColumn()
        {
            #region 合同耗用
            gridGWBDetailUsage_HT.Columns[DtlContractBasePrice_HT.Name].Visible = true;
            gridGWBDetailUsage_HT.Columns[DtlContractPricePercent_HT.Name].Visible = true;
            gridGWBDetailUsage_HT.Columns[DtlContractQuantityPrice_HT.Name].Visible = true;
            gridGWBDetailUsage_HT.Columns[DtlContractQuotaQuantity_HT.Name].Visible = true;
            gridGWBDetailUsage_HT.Columns[DtlContractUsageQuantity_HT.Name].Visible = true;
            gridGWBDetailUsage_HT.Columns[DtlContractUsageTotal_HT.Name].Visible = true;
            //gridGWBDetailUsage.Columns[DtlContractWorkAmountPrice_HT.Name].Visible = true;

            gridGWBDetailUsage_HT.Columns[DtlResponsibleBasePrice_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlResponsiblePricePercent_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlResponsibleQuantityPrice_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlResponsibleQuotaQuantity_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlResponsibleUsageQuantity_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlResponsibleUsageTotal_HT.Name].Visible = false;
            //gridGWBDetailUsage.Columns[DtlResponsibleWorkAmountPrice_HT.Name].Visible = false;

            gridGWBDetailUsage_HT.Columns[DtlPlanBasePrice_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlPlanPricePercent_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlPlanQuantityPrice_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlPlanQuotaQuantity_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlPlanUsageQuantity_HT.Name].Visible = false;
            gridGWBDetailUsage_HT.Columns[DtlPlanUsageTotal_HT.Name].Visible = false;
            //gridGWBDetailUsage.Columns[DtlPlanWorkAmountPrice_HT.Name].Visible = false;

            //如果是新项目显示：“取费基准单价”，“非取费基准单价”，“专业类型”
            if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
            {
                gridGWBDetailUsage_HT.Columns[dtlLaborMachineBasePrice_HT.Name].Visible = true;
                gridGWBDetailUsage_HT.Columns[dtlMaterialBasePrice_HT.Name].Visible = true;
                gridGWBDetailUsage_HT.Columns[dtlProfessionalType_HT.Name].Visible = true;
                gridGWBDetailUsage_HT.Columns[DtlContractBasePrice_HT.Name].Visible = false;
                List<BasicDataOptr> lst = VBasicDataOptr.GetSelFeeTemplateSpecialType();
                this.dtlProfessionalType_HT.DataSource = lst;
                this.dtlProfessionalType_HT.DisplayMember = "BasicName";
                this.dtlProfessionalType_HT.ValueMember = "BasicName";
            }
            else//如果是老项目显示：“合同基准单价”，注：【合同基准单价】=【取费基准单价】+【非取费基准单价】
            {
                gridGWBDetailUsage_HT.Columns[dtlLaborMachineBasePrice_HT.Name].Visible = false;
                gridGWBDetailUsage_HT.Columns[dtlMaterialBasePrice_HT.Name].Visible = false;
                gridGWBDetailUsage_HT.Columns[dtlProfessionalType_HT.Name].Visible = false;
                gridGWBDetailUsage_HT.Columns[DtlContractBasePrice_HT.Name].Visible = true;
            }
            #endregion

            #region 责任耗用
            gridGWBDetailUsage_ZR.Columns[DtlContractBasePrice_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlContractPricePercent_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlContractQuantityPrice_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlContractQuotaQuantity_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlContractUsageQuantity_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlContractUsageTotal_ZR.Name].Visible = false;
            //gridGWBDetailUsage.Columns[DtlContractWorkAmountPrice_ZR.Name].Visible = false;

            gridGWBDetailUsage_ZR.Columns[DtlResponsibleBasePrice_ZR.Name].Visible = true;
            gridGWBDetailUsage_ZR.Columns[DtlResponsiblePricePercent_ZR.Name].Visible = true;
            gridGWBDetailUsage_ZR.Columns[DtlResponsibleQuantityPrice_ZR.Name].Visible = true;
            gridGWBDetailUsage_ZR.Columns[DtlResponsibleQuotaQuantity_ZR.Name].Visible = true;
            gridGWBDetailUsage_ZR.Columns[DtlResponsibleUsageQuantity_ZR.Name].Visible = true;
            gridGWBDetailUsage_ZR.Columns[DtlResponsibleUsageTotal_ZR.Name].Visible = true;
            //gridGWBDetailUsage.Columns[DtlResponsibleWorkAmountPrice_ZR.Name].Visible = true;

            gridGWBDetailUsage_ZR.Columns[DtlPlanBasePrice_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlPlanPricePercent_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlPlanQuantityPrice_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlPlanQuotaQuantity_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlPlanUsageQuantity_ZR.Name].Visible = false;
            gridGWBDetailUsage_ZR.Columns[DtlPlanUsageTotal_ZR.Name].Visible = false;
            //gridGWBDetailUsage.Columns[DtlPlanWorkAmountPrice_ZR.Name].Visible = false;
            #endregion


            #region 计划耗用
            gridGWBDetailUsage_JH.Columns[DtlContractBasePrice_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlContractPricePercent_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlContractQuantityPrice_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlContractQuotaQuantity_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlContractUsageQuantity_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlContractUsageTotal_JH.Name].Visible = false;
            //gridGWBDetailUsage.Columns[DtlContractWorkAmountPrice_JH.Name].Visible = false;

            gridGWBDetailUsage_JH.Columns[DtlResponsibleBasePrice_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlResponsiblePricePercent_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlResponsibleQuantityPrice_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlResponsibleQuotaQuantity_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlResponsibleUsageQuantity_JH.Name].Visible = false;
            gridGWBDetailUsage_JH.Columns[DtlResponsibleUsageTotal_JH.Name].Visible = false;
            //gridGWBDetailUsage.Columns[DtlResponsibleWorkAmountPrice_JH.Name].Visible = false;

            gridGWBDetailUsage_JH.Columns[DtlPlanBasePrice_JH.Name].Visible = true;
            gridGWBDetailUsage_JH.Columns[DtlPlanPricePercent_JH.Name].Visible = true;
            gridGWBDetailUsage_JH.Columns[DtlPlanQuantityPrice_JH.Name].Visible = true;
            gridGWBDetailUsage_JH.Columns[DtlPlanQuotaQuantity_JH.Name].Visible = true;
            gridGWBDetailUsage_JH.Columns[DtlPlanUsageQuantity_JH.Name].Visible = true;
            gridGWBDetailUsage_JH.Columns[DtlPlanUsageTotal_JH.Name].Visible = true;
            //gridGWBDetailUsage.Columns[DtlPlanWorkAmountPrice_JH.Name].Visible = true;
            #endregion
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
            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlContractWorkAmountPrice_HT.Name].ReadOnly = OptionGWBSDtl_HT.CostingFlag == 0;
            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlResponsibleWorkAmountPrice_HT.Name].ReadOnly = OptionGWBSDtl_HT.ResponseFlag == 0;
            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlPlanWorkAmountPrice_HT.Name].ReadOnly = OptionGWBSDtl_HT.CostingFlag == 0;

            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlContractUsageQuantity_HT.Name].ReadOnly = OptionGWBSDtl_HT.CostingFlag == 0;
            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlResponsibleUsageQuantity_HT.Name].ReadOnly = OptionGWBSDtl_HT.ResponseFlag == 0;
            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlPlanUsageQuantity_HT.Name].ReadOnly = OptionGWBSDtl_HT.CostingFlag == 0;

            //双击选择值的列
            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlAccountSubject_HT.Name].ReadOnly = true;
            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlResourceTypeName_HT.Name].ReadOnly = true;
            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlResourceTypeQuality_HT.Name].ReadOnly = true;
            gridGWBDetailUsage_HT.Rows[rowIndex].Cells[DtlResourceTypeSpec_HT.Name].ReadOnly = true;

            //合同工程量单价：当所属{工程任务明细}_【成本核算标志】=1时为直接控制值
            //责任工程量单价：当所属{工程任务明细}_【责任核算标志】=1时为直接控制值
            //计划工程量单价：当所属{工程任务明细}_【成本核算标志】=1时为直接控制值
            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlContractWorkAmountPrice_ZR.Name].ReadOnly = OptionGWBSDtl_ZR.CostingFlag == 0;
            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlResponsibleWorkAmountPrice_ZR.Name].ReadOnly = OptionGWBSDtl_ZR.ResponseFlag == 0;
            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlPlanWorkAmountPrice_ZR.Name].ReadOnly = OptionGWBSDtl_ZR.CostingFlag == 0;

            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlContractUsageQuantity_ZR.Name].ReadOnly = OptionGWBSDtl_ZR.CostingFlag == 0;
            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlResponsibleUsageQuantity_ZR.Name].ReadOnly = OptionGWBSDtl_ZR.ResponseFlag == 0;
            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlPlanUsageQuantity_ZR.Name].ReadOnly = OptionGWBSDtl_ZR.CostingFlag == 0;

            //双击选择值的列
            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlAccountSubject_ZR.Name].ReadOnly = true;
            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlResourceTypeName_ZR.Name].ReadOnly = true;
            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlResourceTypeQuality_ZR.Name].ReadOnly = true;
            gridGWBDetailUsage_ZR.Rows[rowIndex].Cells[DtlResourceTypeSpec_ZR.Name].ReadOnly = true;

            //合同工程量单价：当所属{工程任务明细}_【成本核算标志】=1时为直接控制值
            //责任工程量单价：当所属{工程任务明细}_【责任核算标志】=1时为直接控制值
            //计划工程量单价：当所属{工程任务明细}_【成本核算标志】=1时为直接控制值
            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlContractWorkAmountPrice_JH.Name].ReadOnly = OptionGWBSDtl_JH.CostingFlag == 0;
            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlResponsibleWorkAmountPrice_JH.Name].ReadOnly = OptionGWBSDtl_JH.ResponseFlag == 0;
            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlPlanWorkAmountPrice_JH.Name].ReadOnly = OptionGWBSDtl_JH.CostingFlag == 0;

            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlContractUsageQuantity_JH.Name].ReadOnly = OptionGWBSDtl_JH.CostingFlag == 0;
            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlResponsibleUsageQuantity_JH.Name].ReadOnly = OptionGWBSDtl_JH.ResponseFlag == 0;
            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlPlanUsageQuantity_JH.Name].ReadOnly = OptionGWBSDtl_JH.CostingFlag == 0;

            //双击选择值的列
            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlAccountSubject_JH.Name].ReadOnly = true;
            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlResourceTypeName_JH.Name].ReadOnly = true;
            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlResourceTypeQuality_JH.Name].ReadOnly = true;
            gridGWBDetailUsage_JH.Rows[rowIndex].Cells[DtlResourceTypeSpec_JH.Name].ReadOnly = true;


        }


        private void AddTaskDetailResUsageInGrid(string type, GWBSDetailCostSubject item, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = 0;
            DataGridViewRow row = null;

            switch (type)
            {
                case "HT":
                    index = gridGWBDetailUsage_HT.Rows.Add();
                    row = gridGWBDetailUsage_HT.Rows[index];
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        row.Cells[DtlUsageName_HT.Name].Value = "耗用名称";
                    }
                    else
                    {
                        row.Cells[DtlUsageName_HT.Name].Value = item.Name;
                    }
                    row.Cells[DtlAccountSubject_HT.Name].Value = item.CostAccountSubjectName;


                    row.Cells[DtlResourceTypeName_HT.Name].Value = item.ResourceTypeName;
                    row.Cells[DtlResourceTypeSpec_HT.Name].Value = item.ResourceTypeSpec;
                    row.Cells[DtlResourceTypeQuality_HT.Name].Value = item.ResourceTypeQuality;
                    row.Cells[DtlDiagramNumber_HT.Name].Value = item.DiagramNumber;

                    row.Cells[DtlMainResourceFlag_HT.Name].Value = item.MainResTypeFlag ? "是" : "否";
                    row.Cells[DtlQuantityUnit_HT.Name].Value = item.ProjectAmountUnitName;
                    row.Cells[DtlPriceUnit_HT.Name].Value = item.PriceUnitName;

                    row.Cells[DtlContractQuotaQuantity_HT.Name].Value = item.ContractQuotaQuantity;
                    row.Cells[DtlContractBasePrice_HT.Name].Value = item.ContractBasePrice;

                    row.Cells[dtlLaborMachineBasePrice_HT.Name].Value = item.LaborMachineBasePrice;
                    row.Cells[dtlMaterialBasePrice_HT.Name].Value = item.MaterialBasePrice;
                    row.Cells[dtlProfessionalType_HT.Name].Value = item.ProfessionalType;

                    row.Cells[DtlContractPricePercent_HT.Name].Value = item.ContractPricePercent;
                    row.Cells[DtlContractQuantityPrice_HT.Name].Value = item.ContractQuantityPrice;
                    row.Cells[DtlContractWorkAmountPrice_HT.Name].Value = item.ContractPrice;
                    row.Cells[DtlContractUsageQuantity_HT.Name].Value = item.ContractProjectAmount;
                    row.Cells[DtlContractUsageTotal_HT.Name].Value = item.ContractTotalPrice;

                    row.Cells[DtlResponsibleQuotaQuantity_HT.Name].Value = item.ResponsibleQuotaNum;
                    row.Cells[DtlResponsibleBasePrice_HT.Name].Value = item.ResponsibleBasePrice;
                    row.Cells[DtlResponsiblePricePercent_HT.Name].Value = item.ResponsiblePricePercent;
                    row.Cells[DtlResponsibleQuantityPrice_HT.Name].Value = item.ResponsibilitilyPrice;
                    row.Cells[DtlResponsibleWorkAmountPrice_HT.Name].Value = item.ResponsibleWorkPrice;
                    row.Cells[DtlResponsibleUsageQuantity_HT.Name].Value = item.ResponsibilitilyWorkAmount;
                    row.Cells[DtlResponsibleUsageTotal_HT.Name].Value = item.ResponsibilitilyTotalPrice;

                    row.Cells[DtlPlanQuotaQuantity_HT.Name].Value = item.PlanQuotaNum;
                    row.Cells[DtlPlanBasePrice_HT.Name].Value = item.PlanBasePrice;
                    row.Cells[DtlPlanPricePercent_HT.Name].Value = item.PlanPricePercent;
                    row.Cells[DtlPlanQuantityPrice_HT.Name].Value = item.PlanPrice;
                    row.Cells[DtlPlanWorkAmountPrice_HT.Name].Value = item.PlanWorkPrice;
                    row.Cells[DtlPlanUsageQuantity_HT.Name].Value = item.PlanWorkAmount;
                    row.Cells[DtlPlanUsageTotal_HT.Name].Value = item.PlanTotalPrice;

                    row.Cells[colTechnologyParam_HT.Name].Value = item.TechnicalParam;

                    row.Tag = item;

                    row.ReadOnly = isReadOnly;

                    if (isSetCurrentCell)
                        gridGWBDetailUsage_ZR.CurrentCell = row.Cells[DtlUsageName_HT.Name];
                    break;
                case "ZR":
                    index = gridGWBDetailUsage_ZR.Rows.Add();
                    row = gridGWBDetailUsage_ZR.Rows[index];
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        row.Cells[DtlUsageName_ZR.Name].Value = "耗用名称";
                    }
                    else
                    {
                        row.Cells[DtlUsageName_ZR.Name].Value = item.Name;
                    }
                    row.Cells[DtlAccountSubject_ZR.Name].Value = item.CostAccountSubjectName;


                    row.Cells[DtlResourceTypeName_ZR.Name].Value = item.ResourceTypeName;
                    row.Cells[DtlResourceTypeSpec_ZR.Name].Value = item.ResourceTypeSpec;
                    row.Cells[DtlResourceTypeQuality_ZR.Name].Value = item.ResourceTypeQuality;
                    row.Cells[DtlDiagramNumber_ZR.Name].Value = item.DiagramNumber;

                    row.Cells[DtlMainResourceFlag_ZR.Name].Value = item.MainResTypeFlag ? "是" : "否";
                    row.Cells[DtlQuantityUnit_ZR.Name].Value = item.ProjectAmountUnitName;
                    row.Cells[DtlPriceUnit_ZR.Name].Value = item.PriceUnitName;

                    row.Cells[DtlContractQuotaQuantity_ZR.Name].Value = item.ContractQuotaQuantity;
                    row.Cells[DtlContractBasePrice_ZR.Name].Value = item.ContractBasePrice;
                    row.Cells[DtlContractPricePercent_ZR.Name].Value = item.ContractPricePercent;
                    row.Cells[DtlContractQuantityPrice_ZR.Name].Value = item.ContractQuantityPrice;
                    row.Cells[DtlContractWorkAmountPrice_ZR.Name].Value = item.ContractPrice;
                    row.Cells[DtlContractUsageQuantity_ZR.Name].Value = item.ContractProjectAmount;
                    row.Cells[DtlContractUsageTotal_ZR.Name].Value = item.ContractTotalPrice;

                    row.Cells[DtlResponsibleQuotaQuantity_ZR.Name].Value = item.ResponsibleQuotaNum;
                    row.Cells[DtlResponsibleBasePrice_ZR.Name].Value = item.ResponsibleBasePrice;
                    row.Cells[DtlResponsiblePricePercent_ZR.Name].Value = item.ResponsiblePricePercent;
                    row.Cells[DtlResponsibleQuantityPrice_ZR.Name].Value = item.ResponsibilitilyPrice;
                    row.Cells[DtlResponsibleWorkAmountPrice_ZR.Name].Value = item.ResponsibleWorkPrice;
                    row.Cells[DtlResponsibleUsageQuantity_ZR.Name].Value = item.ResponsibilitilyWorkAmount;
                    row.Cells[DtlResponsibleUsageTotal_ZR.Name].Value = item.ResponsibilitilyTotalPrice;

                    row.Cells[DtlPlanQuotaQuantity_ZR.Name].Value = item.PlanQuotaNum;
                    row.Cells[DtlPlanBasePrice_ZR.Name].Value = item.PlanBasePrice;
                    row.Cells[DtlPlanPricePercent_ZR.Name].Value = item.PlanPricePercent;
                    row.Cells[DtlPlanQuantityPrice_ZR.Name].Value = item.PlanPrice;
                    row.Cells[DtlPlanWorkAmountPrice_ZR.Name].Value = item.PlanWorkPrice;
                    row.Cells[DtlPlanUsageQuantity_ZR.Name].Value = item.PlanWorkAmount;
                    row.Cells[DtlPlanUsageTotal_ZR.Name].Value = item.PlanTotalPrice;

                    row.Cells[colTechnologyParam_ZR.Name].Value = item.TechnicalParam;

                    row.Tag = item;

                    row.ReadOnly = isReadOnly;

                    if (isSetCurrentCell)
                        gridGWBDetailUsage_ZR.CurrentCell = row.Cells[DtlUsageName_ZR.Name];
                    break;

                case "JH":
                    index = gridGWBDetailUsage_JH.Rows.Add();
                    row = gridGWBDetailUsage_JH.Rows[index];
                    if (string.IsNullOrEmpty(item.Name))
                    {
                        row.Cells[DtlUsageName_JH.Name].Value = "耗用名称";
                    }
                    else
                    {
                        row.Cells[DtlUsageName_JH.Name].Value = item.Name;
                    }
                    row.Cells[DtlAccountSubject_JH.Name].Value = item.CostAccountSubjectName;


                    row.Cells[DtlResourceTypeName_JH.Name].Value = item.ResourceTypeName;
                    row.Cells[DtlResourceTypeSpec_JH.Name].Value = item.ResourceTypeSpec;
                    row.Cells[DtlResourceTypeQuality_JH.Name].Value = item.ResourceTypeQuality;
                    row.Cells[DtlDiagramNumber_JH.Name].Value = item.DiagramNumber;

                    row.Cells[DtlMainResourceFlag_JH.Name].Value = item.MainResTypeFlag ? "是" : "否";
                    row.Cells[DtlQuantityUnit_JH.Name].Value = item.ProjectAmountUnitName;
                    row.Cells[DtlPriceUnit_JH.Name].Value = item.PriceUnitName;

                    row.Cells[DtlContractQuotaQuantity_JH.Name].Value = item.ContractQuotaQuantity;
                    row.Cells[DtlContractBasePrice_JH.Name].Value = item.ContractBasePrice;
                    row.Cells[DtlContractPricePercent_JH.Name].Value = item.ContractPricePercent;
                    row.Cells[DtlContractQuantityPrice_JH.Name].Value = item.ContractQuantityPrice;
                    row.Cells[DtlContractWorkAmountPrice_JH.Name].Value = item.ContractPrice;
                    row.Cells[DtlContractUsageQuantity_JH.Name].Value = item.ContractProjectAmount;
                    row.Cells[DtlContractUsageTotal_JH.Name].Value = item.ContractTotalPrice;

                    row.Cells[DtlResponsibleQuotaQuantity_JH.Name].Value = item.ResponsibleQuotaNum;
                    row.Cells[DtlResponsibleBasePrice_JH.Name].Value = item.ResponsibleBasePrice;
                    row.Cells[DtlResponsiblePricePercent_JH.Name].Value = item.ResponsiblePricePercent;
                    row.Cells[DtlResponsibleQuantityPrice_JH.Name].Value = item.ResponsibilitilyPrice;
                    row.Cells[DtlResponsibleWorkAmountPrice_JH.Name].Value = item.ResponsibleWorkPrice;
                    row.Cells[DtlResponsibleUsageQuantity_JH.Name].Value = item.ResponsibilitilyWorkAmount;
                    row.Cells[DtlResponsibleUsageTotal_JH.Name].Value = item.ResponsibilitilyTotalPrice;

                    row.Cells[DtlPlanQuotaQuantity_JH.Name].Value = item.PlanQuotaNum;
                    row.Cells[DtlPlanBasePrice_JH.Name].Value = item.PlanBasePrice;
                    row.Cells[DtlPlanPricePercent_JH.Name].Value = item.PlanPricePercent;
                    row.Cells[DtlPlanQuantityPrice_JH.Name].Value = item.PlanPrice;
                    row.Cells[DtlPlanWorkAmountPrice_JH.Name].Value = item.PlanWorkPrice;
                    row.Cells[DtlPlanUsageQuantity_JH.Name].Value = item.PlanWorkAmount;
                    row.Cells[DtlPlanUsageTotal_JH.Name].Value = item.PlanTotalPrice;

                    row.Cells[colTechnologyParam_JH.Name].Value = item.TechnicalParam;

                    row.Tag = item;

                    row.ReadOnly = isReadOnly;

                    if (isSetCurrentCell)
                        gridGWBDetailUsage_JH.CurrentCell = row.Cells[DtlUsageName_JH.Name];
                    break;
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



                LoadDtlUsageByWBSDetail();

                List<int> listRowIndex = new List<int>();
                List<int> listDetailIndex = new List<int>();
                DateTime serverTime = model.GetServerTime();

                #region  合同耗用
                listRowIndex.Clear();
                listDetailIndex.Clear();
                foreach (DataGridViewRow row in gridGWBDetailUsage_HT.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        int detailIndex = 0;

                        GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                        if (!string.IsNullOrEmpty(dtl.Id))
                        {
                            for (int i = 0; i < OptionGWBSDtl_HT.ListCostSubjectDetails.Count; i++)
                            {
                                GWBSDetailCostSubject temp = OptionGWBSDtl_HT.ListCostSubjectDetails.ElementAt(i);
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


                                    temp.ContractQuotaQuantity = dtl.ContractQuotaQuantity;
                                    temp.ContractBasePrice = dtl.ContractBasePrice;

                                    temp.LaborMachineBasePrice = dtl.LaborMachineBasePrice;
                                    temp.MaterialBasePrice = dtl.MaterialBasePrice;
                                    temp.ProfessionalType = dtl.ProfessionalType;

                                    temp.ContractPricePercent = dtl.ContractPricePercent;
                                    temp.ContractQuantityPrice = dtl.ContractQuantityPrice;
                                    temp.ContractPrice = dtl.ContractPrice;
                                    temp.ContractProjectAmount = dtl.ContractProjectAmount;
                                    temp.ContractTotalPrice = dtl.ContractTotalPrice;


                                    temp.UpdateTime = serverTime;

                                    detailIndex = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            dtl.TheGWBSDetail = OptionGWBSDtl_HT;

                            OptionGWBSDtl_HT.ListCostSubjectDetails.Add(dtl);

                            detailIndex = OptionGWBSDtl_HT.ListCostSubjectDetails.Count - 1;
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
                foreach (DataGridViewRow row in gridGWBDetailUsage_HT.Rows)
                {
                    GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                    if (item.MainResTypeFlag)
                    {

                        OptionGWBSDtl_HT.MainResourceTypeId = item.ResourceTypeGUID;
                        OptionGWBSDtl_HT.MainResourceTypeName = item.ResourceTypeName;
                        OptionGWBSDtl_HT.MainResourceTypeQuality = item.ResourceTypeQuality;
                        OptionGWBSDtl_HT.MainResourceTypeSpec = item.ResourceTypeSpec;

                        OptionGWBSDtl_HT.DiagramNumber = item.DiagramNumber;

                        mainFlag = true;
                        break;
                    }
                }
                if (mainFlag == false)
                {
                    OptionGWBSDtl_HT.MainResourceTypeId = null;
                    OptionGWBSDtl_HT.MainResourceTypeName = null;
                    OptionGWBSDtl_HT.MainResourceTypeQuality = null;
                    OptionGWBSDtl_HT.MainResourceTypeSpec = null;
                    OptionGWBSDtl_HT.DiagramNumber = null;
                }

                if (!string.IsNullOrEmpty(OptionGWBSDtl_HT.Id))
                {
                    //IList listResult = model.SaveOrUpdateDetail(OptionGWBSDtl, null);
                    //OptionGWBSDtl = listResult[0] as GWBSDetail;
                }

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridGWBDetailUsage_HT.Rows[rowIndex].Tag = OptionGWBSDtl_HT.ListCostSubjectDetails.ElementAt(listDetailIndex[i]);
                    gridGWBDetailUsage_HT.Rows[rowIndex].ReadOnly = true;
                }
                #endregion

                #region  责任耗用
                listRowIndex.Clear();
                listDetailIndex.Clear();
                foreach (DataGridViewRow row in gridGWBDetailUsage_ZR.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        int detailIndex = 0;

                        GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                        if (!string.IsNullOrEmpty(dtl.Id))
                        {
                            for (int i = 0; i < OptionGWBSDtl_ZR.ListCostSubjectDetails.Count; i++)
                            {
                                GWBSDetailCostSubject temp = OptionGWBSDtl_ZR.ListCostSubjectDetails.ElementAt(i);
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


                                    temp.ResponsibleQuotaNum = dtl.ResponsibleQuotaNum;
                                    temp.ResponsibleBasePrice = dtl.ResponsibleBasePrice;
                                    temp.ResponsiblePricePercent = dtl.ResponsiblePricePercent;
                                    temp.ResponsibilitilyPrice = dtl.ResponsibilitilyPrice;
                                    temp.ResponsibilitilyWorkAmount = dtl.ResponsibilitilyWorkAmount;
                                    temp.ResponsibleWorkPrice = dtl.ResponsibleWorkPrice;
                                    temp.ResponsibilitilyTotalPrice = dtl.ResponsibilitilyTotalPrice;


                                    temp.UpdateTime = serverTime;

                                    detailIndex = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            dtl.TheGWBSDetail = OptionGWBSDtl_ZR;

                            OptionGWBSDtl_ZR.ListCostSubjectDetails.Add(dtl);

                            detailIndex = OptionGWBSDtl_ZR.ListCostSubjectDetails.Count - 1;
                        }

                        listRowIndex.Add(row.Index);

                        listDetailIndex.Add(detailIndex);
                    }
                }

                if (listRowIndex.Count == 0)
                {
                    return true;
                }

                mainFlag = false;
                foreach (DataGridViewRow row in gridGWBDetailUsage_ZR.Rows)
                {
                    GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                    if (item.MainResTypeFlag)
                    {

                        OptionGWBSDtl_ZR.MainResourceTypeId = item.ResourceTypeGUID;
                        OptionGWBSDtl_ZR.MainResourceTypeName = item.ResourceTypeName;
                        OptionGWBSDtl_ZR.MainResourceTypeQuality = item.ResourceTypeQuality;
                        OptionGWBSDtl_ZR.MainResourceTypeSpec = item.ResourceTypeSpec;

                        OptionGWBSDtl_ZR.DiagramNumber = item.DiagramNumber;

                        mainFlag = true;
                        break;
                    }
                }
                if (mainFlag == false)
                {
                    OptionGWBSDtl_ZR.MainResourceTypeId = null;
                    OptionGWBSDtl_ZR.MainResourceTypeName = null;
                    OptionGWBSDtl_ZR.MainResourceTypeQuality = null;
                    OptionGWBSDtl_ZR.MainResourceTypeSpec = null;
                    OptionGWBSDtl_ZR.DiagramNumber = null;
                }

                if (!string.IsNullOrEmpty(OptionGWBSDtl_ZR.Id))
                {
                    //IList listResult = model.SaveOrUpdateDetail(OptionGWBSDtl, null);
                    //OptionGWBSDtl = listResult[0] as GWBSDetail;
                }

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridGWBDetailUsage_ZR.Rows[rowIndex].Tag = OptionGWBSDtl_ZR.ListCostSubjectDetails.ElementAt(listDetailIndex[i]);
                    gridGWBDetailUsage_ZR.Rows[rowIndex].ReadOnly = true;
                }
                #endregion

                #region  计划耗用
                listRowIndex.Clear();
                listDetailIndex.Clear();
                foreach (DataGridViewRow row in gridGWBDetailUsage_JH.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        int detailIndex = 0;

                        GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                        if (!string.IsNullOrEmpty(dtl.Id))
                        {
                            for (int i = 0; i < OptionGWBSDtl_JH.ListCostSubjectDetails.Count; i++)
                            {
                                GWBSDetailCostSubject temp = OptionGWBSDtl_JH.ListCostSubjectDetails.ElementAt(i);
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


                                    temp.PlanQuotaNum = dtl.PlanQuotaNum;
                                    temp.PlanBasePrice = dtl.PlanBasePrice;
                                    temp.PlanPricePercent = dtl.PlanPricePercent;
                                    temp.PlanPrice = dtl.PlanPrice;
                                    temp.PlanWorkPrice = dtl.PlanWorkPrice;
                                    temp.PlanWorkAmount = dtl.PlanWorkAmount;
                                    temp.PlanTotalPrice = dtl.PlanTotalPrice;


                                    temp.UpdateTime = serverTime;

                                    detailIndex = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            dtl.TheGWBSDetail = OptionGWBSDtl_JH;

                            OptionGWBSDtl_JH.ListCostSubjectDetails.Add(dtl);

                            detailIndex = OptionGWBSDtl_JH.ListCostSubjectDetails.Count - 1;
                        }

                        listRowIndex.Add(row.Index);

                        listDetailIndex.Add(detailIndex);
                    }
                }

                if (listRowIndex.Count == 0)
                {
                    return true;
                }

                mainFlag = false;
                foreach (DataGridViewRow row in gridGWBDetailUsage_JH.Rows)
                {
                    GWBSDetailCostSubject item = row.Tag as GWBSDetailCostSubject;
                    if (item.MainResTypeFlag)
                    {

                        OptionGWBSDtl_JH.MainResourceTypeId = item.ResourceTypeGUID;
                        OptionGWBSDtl_JH.MainResourceTypeName = item.ResourceTypeName;
                        OptionGWBSDtl_JH.MainResourceTypeQuality = item.ResourceTypeQuality;
                        OptionGWBSDtl_JH.MainResourceTypeSpec = item.ResourceTypeSpec;

                        OptionGWBSDtl_JH.DiagramNumber = item.DiagramNumber;

                        mainFlag = true;
                        break;
                    }
                }
                if (mainFlag == false)
                {
                    OptionGWBSDtl_JH.MainResourceTypeId = null;
                    OptionGWBSDtl_JH.MainResourceTypeName = null;
                    OptionGWBSDtl_JH.MainResourceTypeQuality = null;
                    OptionGWBSDtl_JH.MainResourceTypeSpec = null;
                    OptionGWBSDtl_JH.DiagramNumber = null;
                }

                if (!string.IsNullOrEmpty(OptionGWBSDtl_JH.Id))
                {
                    //IList listResult = model.SaveOrUpdateDetail(OptionGWBSDtl, null);
                    //OptionGWBSDtl = listResult[0] as GWBSDetail;
                }

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridGWBDetailUsage_JH.Rows[rowIndex].Tag = OptionGWBSDtl_JH.ListCostSubjectDetails.ElementAt(listDetailIndex[i]);
                    gridGWBDetailUsage_JH.Rows[rowIndex].ReadOnly = true;
                }
                #endregion

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
            #region 合同耗用
            foreach (DataGridViewRow row in gridGWBDetailUsage_HT.Rows)
            {
                if (!row.ReadOnly)
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    if (dtl.CostAccountSubjectGUID == null)
                    {
                        MessageBox.Show("请选择资源耗用“" + dtl.Name + "”的核算科目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage_HT.CurrentCell = row.Cells[DtlAccountSubject_HT.Name];
                        return false;
                    }
                    if (dtl.ResourceTypeGUID == null)
                    {
                        MessageBox.Show("请选择资源耗用“" + dtl.Name + "”的资源！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage_HT.CurrentCell = row.Cells[this.DtlResourceTypeName_HT.Name];
                        return false;
                    }

                    if (dtl.ContractQuotaQuantity <= 0)
                    {
                        MessageBox.Show("请输入资源耗用“" + dtl.Name + "”的合同定额数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage_HT.CurrentCell = row.Cells[DtlContractQuotaQuantity_HT.Name];
                        return false;
                    }



                }
            }
            #endregion

            #region 责任耗用
            foreach (DataGridViewRow row in gridGWBDetailUsage_ZR.Rows)
            {
                if (!row.ReadOnly)
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    if (dtl.CostAccountSubjectGUID == null)
                    {
                        MessageBox.Show("请选择资源耗用“" + dtl.Name + "”的核算科目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage_ZR.CurrentCell = row.Cells[DtlAccountSubject_ZR.Name];
                        return false;
                    }
                    if (dtl.ResourceTypeGUID == null)
                    {
                        MessageBox.Show("请选择资源耗用“" + dtl.Name + "”的资源！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage_ZR.CurrentCell = row.Cells[this.DtlResourceTypeName_ZR.Name];
                        return false;
                    }



                    if (dtl.ResponsibleQuotaNum <= 0)
                    {
                        MessageBox.Show("请输入资源耗用“" + dtl.Name + "”的责任定额数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage_ZR.CurrentCell = row.Cells[DtlResponsibleQuotaQuantity_ZR.Name];
                        return false;
                    }

                }
            }
            #endregion

            #region 计划耗用
            foreach (DataGridViewRow row in gridGWBDetailUsage_JH.Rows)
            {
                if (!row.ReadOnly)
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;
                    if (dtl.CostAccountSubjectGUID == null)
                    {
                        MessageBox.Show("请选择资源耗用“" + dtl.Name + "”的核算科目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage_JH.CurrentCell = row.Cells[DtlAccountSubject_JH.Name];
                        return false;
                    }
                    if (dtl.ResourceTypeGUID == null)
                    {
                        MessageBox.Show("请选择资源耗用“" + dtl.Name + "”的资源！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage_JH.CurrentCell = row.Cells[this.DtlResourceTypeName_JH.Name];
                        return false;
                    }


                    if (dtl.PlanQuotaNum <= 0)
                    {
                        MessageBox.Show("请输入资源耗用“" + dtl.Name + "”的计划定额数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        gridGWBDetailUsage_JH.CurrentCell = row.Cells[DtlPlanQuotaQuantity_JH.Name];
                        return false;
                    }

                }
            }
            #endregion

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
                int dtlCount = OptionGWBSDtl_HT.ListCostSubjectDetails.Count;//未加载时加载
            }
            catch
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", OptionGWBSDtl_HT.Id));
                oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                OptionGWBSDtl_HT = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;
            }
        }
    }


}
