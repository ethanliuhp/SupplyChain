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
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost
{
    public partial class VSpecialCostSearchList : SearchList
    {
        private CSpecialCost cSpecialCostOrder;
        public VSpecialCostSearchList(CSpecialCost cSpecialCostOrder)
        {
            InitializeComponent();
            this.cSpecialCostOrder = cSpecialCostOrder;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            object date = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            if (Id != null)
            {
                cSpecialCostOrder.Find(date.ToString(), ClientUtil.ToString(Id));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code","编号");
            AddColumn("State", "状态");
            AddColumn("CostType", "费用类型");
            AddColumn("EngTaskName", "工程任务名称");
            dgSearchResult.Columns["EngTaskName"].Width = 120;
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "业务日期");
            //dgSearchResult.Columns["Descript"].Width = 300;//设置检查结果列宽
            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        public void RefreshData(IList lst)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();
            foreach (SpecialCostMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["CostType"].Value = obj.CostType;
                dr.Cells["EngTaskName"].Value = obj.EngTaskName;
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                
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
