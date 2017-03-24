using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSDetailLedgerQuery2 : TBasicDataView
    {
        public VGWBSDetailLedgerQuery2()
        {
            InitializeComponent();

            InitForm();
        }

        MGWBSTree model = new MGWBSTree();
        CurrentProjectInfo projectInfo = null;
        private void InitForm()
        {
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            InitEvents();
        }
        private void InitEvents()
        {
            btnSelectContractGroup.Click += new EventHandler(btnSelectContractGroup_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);
        }

        void btnSelectContractGroup_Click(object sender, EventArgs e)
        {
            VSelectWBSContractGroup frm = new VSelectWBSContractGroup(new MWBSContractGroup(), false);
            if (txtContract.Text.Trim() != "" && txtContract.Tag != null)
                frm.DefaultSelectedContract = txtContract.Tag as ContractGroup;
            frm.ShowDialog();

            if (frm.SelectResult != null && frm.SelectResult.Count > 0)
            {
                ContractGroup cg = frm.SelectResult[0];
                txtContract.Text = "【" + cg.Code + "】" + cg.ContractName;
                txtContract.Tag = cg;
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            if (txtContract.Text.Trim() == "" || txtContract.Tag == null) return;

            try
            {

                FlashScreen.Show("正在查询，请稍候........");

                var list = model.SearchGWBSDetailByContractGroupId((txtContract.Tag as ContractGroup).Id);

                //显示数据
                gridGWBDetail.Rows.Clear();
                foreach (GWBSDetail item in list)
                {
                    InsertToGridDetail(item);
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("查询异常，请重试！详细信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void InsertToGridDetail(GWBSDetail dtl)
        {
            int index = gridGWBDetail.Rows.Add();
            DataGridViewRow row = gridGWBDetail.Rows[index];

            row.Cells[DtlName.Name].Value = dtl.Name;
            row.Cells[DtlCostItemCode.Name].Value = dtl.TheCostItem.QuotaCode;
            row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;
            row.Cells[DtlQuantityUnit.Name].Value = dtl.WorkAmountUnitName;

            row.Cells[DtlRespQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[DtlRespPrice.Name].Value = dtl.ResponsibilitilyPrice;
            row.Cells[DtlRespTotalPrice.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[DtlContractQuantity.Name].Value = dtl.ContractProjectQuantity;
            row.Cells[DtlContractPrice.Name].Value = dtl.ContractPrice;
            row.Cells[DtlContractTotalPrice.Name].Value = dtl.ContractTotalPrice;

            row.Cells[DtlPlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[DtlPlanPrice.Name].Value = dtl.PlanPrice;
            row.Cells[DtlPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;

            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[DtlTeamName.Name].Value = dtl.ContractProjectName;
            row.Cells[DtlGwbsName.Name].Value = dtl.TheGWBSFullPath;

            row.Cells[DtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);
            row.Cells[DtlDesc.Name].Value = dtl.ContentDesc;
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(gridGWBDetail, true);
        }
    }
}
