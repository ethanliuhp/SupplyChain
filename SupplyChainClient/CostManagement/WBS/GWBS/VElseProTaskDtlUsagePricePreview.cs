using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VElseProTaskDtlUsagePricePreview : Form
    {
        OptCostType optionCostType = OptCostType.合同收入;
        /// <summary>
        /// 显示的明细耗用集合
        /// </summary>
        public IList DefaultListTaskDtlUsage = null;
        /// <summary>
        /// 当前操作的任务明细耗用
        /// </summary>
        public GWBSDetailCostSubject DefaultTaskDtlUsage = null;

        public VElseProTaskDtlUsagePricePreview()
        {
            InitializeComponent();

            this.Load += new EventHandler(VElseProTaskDtlUsagePricePreview_Load);
        }
        public VElseProTaskDtlUsagePricePreview(OptCostType optCostType)
            : this()
        {
            optionCostType = optCostType;
        }

        void VElseProTaskDtlUsagePricePreview_Load(object sender, EventArgs e)
        {
            if (DefaultListTaskDtlUsage != null)
            {
                foreach (GWBSDetailCostSubject item in DefaultListTaskDtlUsage)
                {
                    AddDetailUsageInfoInGrid(item);
                }
            }
            SetVisibleColumn();
        }
        private void SetVisibleColumn()
        {
            bool contractFlag = false;
            bool responsibleFlag = false;
            bool planFlag = false;

            if (optionCostType == OptCostType.合同收入) contractFlag = true;
            else if (optionCostType == OptCostType.责任成本) responsibleFlag = true;
            else if (optionCostType == OptCostType.计划成本) planFlag = true;

            gridDtlUsage.Columns[ContractQuotaNum.Name].Visible = contractFlag;
            gridDtlUsage.Columns[ContractQuantityPrice.Name].Visible = contractFlag;
            gridDtlUsage.Columns[ContractWorkAmountPrice.Name].Visible = contractFlag;

            gridDtlUsage.Columns[ResponsibleQuotaNum.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[ResponsibleQuantityPrice.Name].Visible = responsibleFlag;
            gridDtlUsage.Columns[ResponsibleWorkAmountPrice.Name].Visible = responsibleFlag;

            gridDtlUsage.Columns[PlanQuotaNum.Name].Visible = planFlag;
            gridDtlUsage.Columns[PlanQuantityPrice.Name].Visible = planFlag;
            gridDtlUsage.Columns[PlanWorkAmountPrice.Name].Visible = planFlag;
        }
        private void AddDetailUsageInfoInGrid(GWBSDetailCostSubject dtl)
        {
            int index = gridDtlUsage.Rows.Add();
            DataGridViewRow row = gridDtlUsage.Rows[index];

            row.Cells[ProjectName.Name].Value = dtl.TheProjectName;
            //row.Cells[GWBSTaskType.Name].Value = DefaultTaskDtlUsage.TheGWBSDetail.TheGWBS.ProjectTaskTypeName;
            row.Cells[GWBSTaskType.Name].Value = dtl.TheGWBSDetail.TheGWBS.ProjectTaskTypeName;
            row.Cells[UpdateTime.Name].Value = dtl.UpdateTime;

            row.Cells[CostItem.Name].Value = DefaultTaskDtlUsage.TheGWBSDetail.TheCostItem.Name;
            row.Cells[MainResourceType.Name].Value = DefaultTaskDtlUsage.TheGWBSDetail.MainResourceTypeName;

            row.Cells[ContractQuotaNum.Name].Value = dtl.ContractQuotaQuantity;
            row.Cells[ContractQuantityPrice.Name].Value = dtl.ContractQuantityPrice;
            row.Cells[ContractWorkAmountPrice.Name].Value = dtl.ContractPrice;

            row.Cells[ResponsibleQuotaNum.Name].Value = dtl.ResponsibleQuotaNum;
            row.Cells[ResponsibleQuantityPrice.Name].Value = dtl.ResponsibilitilyPrice;
            row.Cells[ResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;

            row.Cells[PlanQuotaNum.Name].Value = dtl.PlanQuotaNum;
            row.Cells[PlanQuantityPrice.Name].Value = dtl.PlanPrice;
            row.Cells[PlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
