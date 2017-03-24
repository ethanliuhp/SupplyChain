using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using FlexCell;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public partial class VGWBSTreeDetailSelect : TBasicDataView
    {
        public List<GWBSDetail> RtnLstDetail = null;
        private DataGridViewRowCollection GridRows = null;
        public VGWBSTreeDetailSelect(DataGridViewRowCollection rows)
        {
            InitializeComponent();
            InitData(rows);

        }

        private void InitData(DataGridViewRowCollection rows)
        {
            GridRows = rows;
            RtnLstDetail = new List<GWBSDetail>();
            InitGrid();
            InitEvent();
        }

        private void InitEvent()
        {
            btnSave.Click += new EventHandler(btnSave_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }
        void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridGWBDetail.Rows)
            {
                GWBSDetail rowData = row.Tag as GWBSDetail;
                var obj = RtnLstDetail.FirstOrDefault(p => p.Id == rowData.Id);
                bool isChecked = ClientUtil.ToBool(row.Cells[ckb.Name].Value);
                if (isChecked)
                {
                    if (obj == null)
                    {
                        RtnLstDetail.Add(rowData);
                    }
                }
                else
                {
                    if (obj != null)
                    {
                        RtnLstDetail.Remove(obj);
                    }
                }
            }

            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            RtnLstDetail = new List<GWBSDetail>();
            this.Close();
        }

        #region Grid表格

        private void InitGrid()
        {
            foreach (DataGridViewRow row in GridRows)
            {
                GWBSDetail dtl = row.Tag as GWBSDetail;
                AddDetailInfoInGrid(dtl, true);
            }
        }

        private void AddDetailInfoInGrid(GWBSDetail dtl, bool isSetCurrCell)
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
            row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Cells[DtlQuantityUnit.Name].Value = dtl.WorkAmountUnitName;
            row.Cells[DtlPlanQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[DtlPlanPrice.Name].Value = dtl.PlanPrice;
            row.Cells[DtlPlanTotalPrice.Name].Value = dtl.PlanTotalPrice;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[DtlState.Name].Value = StaticMethod.GetWBSTaskStateText(dtl.State);
            row.Cells[DtlDesc.Name].Value = dtl.ContentDesc;
            row.Cells[this.colContractGroupName.Name].Value = dtl.ContractGroupName;
            row.Tag = dtl;

            if (isSetCurrCell)
            {
                gridGWBDetail.CurrentCell = row.Cells[0];
            }
        }

        #endregion
    }
}
