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
    public partial class VElseProTaskDtlPricePreview : Form
    {
        OptCostType optionCostType = OptCostType.合同收入;
        /// <summary>
        /// 显示的任务明细集合
        /// </summary>
        public IList DefaultListTaskDtl = null;

        public VElseProTaskDtlPricePreview()
        {
            InitializeComponent();

            this.Load += new EventHandler(VElseProTaskDtlPricePreview_Load);
        }
        public VElseProTaskDtlPricePreview(OptCostType optCostType)
            : this()
        {
            optionCostType = optCostType;
        }

        void VElseProTaskDtlPricePreview_Load(object sender, EventArgs e)
        {
            if (DefaultListTaskDtl != null)
            {
                foreach (GWBSDetail dtl in DefaultListTaskDtl)
                {
                    AddDetailInfoInGrid(dtl);
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


            gridGWBDetail.Columns[ContractPrice.Name].Visible = contractFlag;
            gridGWBDetail.Columns[ResponsiblePrice.Name].Visible = responsibleFlag;
            gridGWBDetail.Columns[PlanPrice.Name].Visible = planFlag;
        }

        private void AddDetailInfoInGrid(GWBSDetail dtl)
        {
            int index = gridGWBDetail.Rows.Add();
            DataGridViewRow row = gridGWBDetail.Rows[index];

            row.Cells[projectName.Name].Value = dtl.TheProjectName;
            row.Cells[GWBSTaskType.Name].Value = dtl.TheGWBS.ProjectTaskTypeName;
            row.Cells[updateTime.Name].Value = dtl.UpdatedDate;

            row.Cells[ContractPrice.Name].Value = dtl.ContractPrice;
            row.Cells[ResponsiblePrice.Name].Value = dtl.ResponsibilitilyPrice;
            row.Cells[PlanPrice.Name].Value = dtl.PlanPrice;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
