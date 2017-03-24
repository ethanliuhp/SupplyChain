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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDtlUsageEditByTaskChange : TBasicDataView
    {
        /// <summary>
        /// 是否保存变更
        /// </summary>
        public bool IsOK = false;

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

        public VGWBSDtlUsageEditByTaskChange()
        {
            InitializeComponent();
            InitialForm();
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

            btnDeleteDetail.Click += new EventHandler(btnDeleteDetail_Click);
            btnSaveDetail.Click += new EventHandler(btnSaveDetail_Click);
            btnSaveAndExit.Click += new EventHandler(btnSaveAndExit_Click);
            btnCancelUpdate.Click += new EventHandler(btnCancelUpdate_Click);
            btnExit.Click += new EventHandler(btnExit_Click);


            gridGWBDetailUsage.CellValidating += new DataGridViewCellValidatingEventHandler(gridGWBDetailUsage_CellValidating);

            gridGWBDetailUsage.CellEndEdit += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellEndEdit);

            gridGWBDetailUsage.CellDoubleClick += new DataGridViewCellEventHandler(gridGWBDetailUsage_CellDoubleClick);

            this.Load += new EventHandler(VGWBSDetailUsageInfoEdit_Load);
        }

        void gridGWBDetailUsage_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridGWBDetailUsage.Rows[e.RowIndex].ReadOnly == false)
            {
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
                            || colName == DtlContractWorkAmountPrice.Name)//合同耗用
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
                    colName == DtlResourceTypeSpec.Name)//选择资源类型
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
                        if (list.Count > 0)
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

            foreach (GWBSDetailCostSubject item in OptionGWBSDtl.ListCostSubjectDetails)
            {
                AddTaskDetailResUsageInGrid(item, false, false);
            }
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

            item.ProjectAmountUnitGUID = OptionGWBSDtl.WorkAmountUnitGUID;
            item.ProjectAmountUnitName = OptionGWBSDtl.WorkAmountUnitName;

            item.PriceUnitGUID = OptionGWBSDtl.PriceUnitGUID;
            item.PriceUnitName = OptionGWBSDtl.PriceUnitName;

            item.TheGWBSTree = OptionGWBSDtl.TheGWBS;
            item.TheGWBSTreeName = OptionGWBSDtl.TheGWBS.Name;
            item.TheGWBSTreeSyscode = OptionGWBSDtl.TheGWBS.SysCode;

            item.State = GWBSDetailCostSubjectState.编制;

            AddTaskDetailResUsageInGrid(item, true, false);

            gridGWBDetailUsage.BeginEdit(false);
        }
        private void AddTaskDetailResUsageInGrid(GWBSDetailCostSubject item, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridGWBDetailUsage.Rows.Add();
            DataGridViewRow row = gridGWBDetailUsage.Rows[index];

            row.Cells[DtlUsageName.Name].Value = item.Name;
            row.Cells[DtlAccountSubject.Name].Value = item.CostAccountSubjectName;

            //string matStr = string.IsNullOrEmpty(dtl.ResourceTypeQuality) ? "" : dtl.ResourceTypeQuality + ".";
            //matStr += string.IsNullOrEmpty(dtl.ResourceTypeSpec) ? "" : dtl.ResourceTypeSpec + ".";
            //matStr += dtl.ResourceTypeName;

            row.Cells[DtlResourceTypeName.Name].Value = item.ResourceTypeName;
            row.Cells[DtlResourceTypeSpec.Name].Value = item.ResourceTypeSpec;
            row.Cells[DtlDiagramNumber.Name].Value = item.DiagramNumber;

            row.Cells[DtlMainResourceFlag.Name].Value = item.MainResTypeFlag ? "是" : "否";
            row.Cells[DtlQuantityUnit.Name].Value = item.ProjectAmountUnitName;
            row.Cells[DtlPriceUnit.Name].Value = item.PriceUnitName;

            row.Cells[DtlContractQuotaQuantity.Name].Value = item.ContractQuotaQuantity;
            row.Cells[DtlContractBasePrice.Name].Value = item.ContractBasePrice;
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

            row.Tag = item;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridGWBDetailUsage.CurrentCell = row.Cells[DtlUsageName.Name];
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
                LoadDtlUsageByWBSDetail();

                IList list = new List<GWBSDetailCostSubject>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridGWBDetailUsage.SelectedRows)
                {
                    GWBSDetailCostSubject dtl = row.Tag as GWBSDetailCostSubject;

                    list.Add(dtl);

                    listRowIndex.Add(row.Index);
                }

                if (MessageBox.Show("确认要删除选择的资源耗用吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                if (list.Count > 0)
                {
                    for (int i = list.Count - 1; i > -1; i--)
                    {
                        GWBSDetailCostSubject dtl = list[i] as GWBSDetailCostSubject;
                        OptionGWBSDtl.ListCostSubjectDetails.Remove(dtl);
                    }
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

        //放弃修改
        void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            IsOK = false;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", OptionGWBSDtl.Id));
            oq.AddFetchMode("ListCostSubjectDetails", NHibernate.FetchMode.Eager);
            OptionGWBSDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;

            //gridGWBDetailUsage.Rows.Clear();
            //foreach (GWBSDetailCostSubject item in OptionGWBSDtl.ListCostSubjectDetails)
            //{
            //    AddTaskDetailResUsageInGrid(item, false, true);
            //}

            this.Close();
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
                IsOK = true;

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

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridGWBDetailUsage.Rows[rowIndex].Tag = OptionGWBSDtl.ListCostSubjectDetails.ElementAt(listDetailIndex[i]);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
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
                OptionGWBSDtl = model.ObjectQuery(typeof(GWBSDetail), oq)[0] as GWBSDetail;
            }
        }
    }
}
