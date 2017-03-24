using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VResourcesDemandPlanManagementSearchList : SearchList
    {
        private CRollingDemandPlan control;
        //private RemandPlanType optPlanType = 0;

        public VResourcesDemandPlanManagementSearchList(CRollingDemandPlan c)
        {
            InitializeComponent();
            this.control = c;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            ResourceRequireReceipt bill = dgSearchResult.Rows[e.RowIndex].Tag as ResourceRequireReceipt;
            //control.Find(bill.Code);
            control.Find(bill.ReceiptName, bill.Id);
        }

        private void InitDgSearch()
        {

            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("ReceiptName", "计划名称");
            AddColumn("State", "状态");
            AddColumn("CreateTime", "创建时间");
            AddColumn("Descript", "备注");

            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        public void RefreshData( IList lst)
        {


            //从Model中取数
            dgSearchResult.Rows.Clear();

            foreach (ResourceRequireReceipt obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                //dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["State"].Value = Enum.GetName(typeof(ResourceRequirePlanState), obj.State);

                dr.Cells["ReceiptName"].Value = obj.ReceiptName;
                dr.Cells["CreateTime"].Value = obj.CreateDate;

                dr.Cells["Descript"].Value = obj.Descript;

                dr.Tag = obj;
            }


    

            this.ViewShow();
            this.dgSearchResult.AutoResizeColumns();
        }
        public void RemoveRow(string id)
        {
            foreach (DataGridViewRow row in this.dgSearchResult.Rows)
            {
                if (!row.IsNewRow)
                {
                    if (row.Cells["Id"].Value.ToString() == id)
                    {
                        this.dgSearchResult.Rows.Remove(row);
                        break;
                    }
                }
            }
        }
    }
}
