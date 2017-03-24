using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDetailShow : TBasicDataView
    {
        MGWBSTree model = new MGWBSTree();
        //IList gwbsDetaiList = null;
        public VGWBSDetailShow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="detaiList">工程任务明细集合</param>
        public VGWBSDetailShow(IList detailList)
        {
            InitializeComponent();
            InitEvent();
            ShowDetail(detailList);
            DtlMainResourceFlag.Items.Add("是");
            DtlMainResourceFlag.Items.Add("否");
        }
        void InitEvent()
        {
            this.gridGWBDetail.CellClick += new DataGridViewCellEventHandler(gridGWBDetail_CellClick);
        }

        /// <summary>
        /// 资源耗用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridGWBDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                gridGWBDetailUsage.Rows.Clear();
                GWBSDetail detail = gridGWBDetail.Rows[e.RowIndex].Tag as GWBSDetail;
                ShowUsage(detail);
            }
        }

        /// <summary>
        /// 显示明细信息
        /// </summary>
        /// <param name="list"></param>
        void ShowDetail(IList detailList)
        {
            foreach (GWBSDetail dtl in detailList)
            {
                int index = gridGWBDetail.Rows.Add();
                DataGridViewRow row = gridGWBDetail.Rows[index];
                row.Cells[DtlName.Name].Value = dtl.Name;
                row.Cells[DtlOrderNo.Name].Value = dtl.OrderNo.ToString();
                if (dtl.TheCostItem != null)
                {
                    row.Cells[DtlCostItem.Name].Value = dtl.TheCostItem.Name;
                    row.Cells[DtlCostItemCode.Name].Value = dtl.TheCostItem.QuotaCode;
                }
                row.Cells[DtlMainResourceType.Name].Value = dtl.MainResourceTypeName;
                row.Cells[DtlMainResourceTypeQuality.Name].Value = dtl.MainResourceTypeQuality;
                row.Cells[DtlMainResourceTypeSpec.Name].Value = dtl.MainResourceTypeSpec;
                row.Cells[DtlDesc.Name].Value = dtl.ContentDesc;
                row.Cells[DtlState.Name].Value =  StaticMethod.GetWBSTaskStateText(dtl.State);

                row.Tag = dtl;

                //gridGWBDetail.CurrentCell = row.Cells[0];
            }
            gridGWBDetail.ClearSelection();
            gridGWBDetail.Rows[0].Selected = true;
            //关联的资源耗用
            GWBSDetail detail = gridGWBDetail.Rows[0].Tag as GWBSDetail;
            ShowUsage(detail);
        }

        void ShowUsage(GWBSDetail detail)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheGWBSDetail.Id", detail.Id));
            IList usageList = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
            ShowUsage(usageList);
        }

        void ShowUsage(IList usageList)
        {
            gridGWBDetailUsage.Rows.Clear();
            foreach (GWBSDetailCostSubject dtl in usageList)
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

                row.Cells[DtlContractQuotaQuantity.Name].Value = dtl.ContractQuotaQuantity.ToString();
                row.Cells[DtlContractQuantityPrice.Name].Value = dtl.ContractQuantityPrice.ToString();
                row.Cells[DtlContractWorkAmountPrice.Name].Value = dtl.ContractPrice.ToString();
                row.Cells[DtlContractUsageQuantity.Name].Value = dtl.ContractProjectAmount.ToString();
                row.Cells[DtlContractUsageTotal.Name].Value = dtl.ContractTotalPrice.ToString();

                row.Cells[DtlResponsibleQuotaQuantity.Name].Value = dtl.ResponsibleQuotaNum;
                row.Cells[DtlResponsibleQuantityPrice.Name].Value = dtl.ResponsibilitilyPrice;
                row.Cells[DtlResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
                row.Cells[DtlResponsibleUsageQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
                row.Cells[DtlResponsibleUsageTotal.Name].Value = dtl.ResponsibilitilyTotalPrice;

                row.Cells[DtlPlanQuotaQuantity.Name].Value = dtl.PlanQuotaNum;
                row.Cells[DtlPlanQuantityPrice.Name].Value = dtl.PlanPrice;
                row.Cells[DtlPlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
                row.Cells[DtlPlanUsageQuantity.Name].Value = dtl.PlanWorkAmount;
                row.Cells[DtlPlanUsageTotal.Name].Value = dtl.PlanTotalPrice;

                row.Tag = dtl;

                //gridGWBDetailUsage.CurrentCell = row.Cells[0];
            }
        }


    }
}
