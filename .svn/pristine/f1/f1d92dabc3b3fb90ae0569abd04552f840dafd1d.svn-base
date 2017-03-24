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
using Application.Business.Erp.SupplyChain.StockManage.StockInventory.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng
{
    public partial class VStockInventorySearchList : SearchList
    {
        private CStockInventoryMng cStockInventory;

        public VStockInventorySearchList(CStockInventoryMng cStockInventory)
        {
            InitializeComponent();
            this.cStockInventory = cStockInventory;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            //if (code != null)
            //{
            //   // cStockInventory.Find(code.ToString(),ClientUtil.ToString(Id));
            //    cStockInventory.Find(code.ToString(), ClientUtil.ToString(Id),Enum .Parse (CStockInventoryMng_ExecType,);
            //}
            StockInventoryMaster oStockInventoryMaster  = dgSearchResult.Rows[e.RowIndex].Tag as StockInventoryMaster;
            if (oStockInventoryMaster != null)
            {
               // cStockInventory.Find(code.ToString(),ClientUtil.ToString(Id));
                CStockInventoryMng_ExecType oExecType= string.IsNullOrEmpty ( oStockInventoryMaster.Special )? CStockInventoryMng_ExecType.土建 :(CStockInventoryMng_ExecType  )Enum .Parse (typeof ( CStockInventoryMng_ExecType),oStockInventoryMaster.Special );
                cStockInventory.Find(oStockInventoryMaster.Code, oStockInventoryMaster.Id, oExecType);
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("State", "状态");
            AddColumn("SumQuantity", "总数量");
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

            foreach (StockInventoryMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["SumQuantity"].Value = obj.SumQuantity;
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
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
