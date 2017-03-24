using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireTransportCost.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireTransportCost
{
    public partial class VMaterialHireTransportCostSearchList  : SearchList
    {
        private CMaterialHireTransportCost cMaterialHireTransportCost;

        public VMaterialHireTransportCostSearchList(CMaterialHireTransportCost cMaterialHireTransportCost)
        {
            InitializeComponent();
            this.cMaterialHireTransportCost = cMaterialHireTransportCost;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            object id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (code != null)
            {
                cMaterialHireTransportCost.Find(code.ToString(), id.ToString());
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");
            AddColumn("BillCode", "原单据号");
            AddColumn("SupplyRelation", "出租方");
            AddColumn("ProjectName", "租赁方");

            AddColumn("SumMoney", "总金额");
            AddColumn("State", "状态");
  
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "业务日期");
            AddColumn("RealOperationDate", "制单日期");
            AddColumn("Descript", "备注");

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

            foreach (MatHireTranCostMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;

                dr.Cells["SupplyRelation"].Value = obj.SupplierName;
                dr.Cells["ProjectName"].Value = obj.ProjectName;
                dr.Cells["SumMoney"].Value = obj.SumMoney;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);

                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["RealOperationDate"].Value = obj.RealOperationDate.ToShortDateString();
                dr.Cells["Descript"].Value = obj.Descript;
                dr.Cells["BillCode"].Value = obj.BillCode;
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

