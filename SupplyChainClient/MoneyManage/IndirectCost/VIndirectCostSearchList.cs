using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VIndirectCostSearchList : SearchList
    {
        private CIndirectCost cIndirectCost;

        public VIndirectCostSearchList(CIndirectCost cIndirectCost)
        {
            InitializeComponent();
            this.cIndirectCost = cIndirectCost;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex<0)  return;
            IndirectCostMaster oIndirectCostMaster = dgSearchResult.Rows[e.RowIndex].Tag as IndirectCostMaster;
            if (oIndirectCostMaster != null)
            {
                cIndirectCost.Find(oIndirectCostMaster.Code, IndirectCostExecType.费用信息维护, oIndirectCostMaster.Id);
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");
            //AddColumn("SupplyOrderCode", "采购合同号");
            //AddColumn("SupplyRelation", "供应商");
            AddColumn("State", "状态");
            //AddColumn("SumQuantity", "总数量");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "业务日期");
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

            foreach (IndirectCostMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Tag = obj;
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                //dr.Cells["SupplyOrderCode"].Value = obj.SupplyOrderCode;
                //dr.Cells["SupplyRelation"].Value = obj.TheSupplierName;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                //dr.Cells["SumQuantity"].Value = obj.SumQuantity;
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["Descript"].Value = obj.Descript;
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
