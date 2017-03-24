using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder
{
    public partial class VStockBlockMaterialSearchList : SearchList
    {
        private CMaterialHireOrder oCMaterialHireOrder;
        public EnumMatHireType MatHireType;
        public VStockBlockMaterialSearchList(CMaterialHireOrder oCMaterialHireOrder, EnumMatHireType MatHireType)
        {
            InitializeComponent();
            this.oCMaterialHireOrder = oCMaterialHireOrder;
            this.MatHireType = MatHireType;
            InitDgSearch();
        }
        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            object id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (code != null)
            {
                oCMaterialHireOrder.Find(code.ToString(), ClientUtil.ToString(id), 
                    this.MatHireType==EnumMatHireType.普通料具?EnumMaterialHireOrder_ExecType.料具封存单:
                    (this.MatHireType == EnumMatHireType.钢管 ? EnumMaterialHireOrder_ExecType.钢管封存单 : EnumMaterialHireOrder_ExecType.碗扣封存单)
                    );
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");
            AddColumn("BillCode", "原单据号");
            AddColumn("Theme", "主题");
            AddColumn("StockReason", "封存事由");
            AddColumn("SupplyRelation", "出租方");
            AddColumn("ProjectName", "租赁方");
            AddColumn("BlockStartTime", "开始时间");
            AddColumn("BlockFinishTime", "结束时间");
            AddColumn("HandlePersonName", "责任人");
            AddColumn("State", "状态");
            AddColumn("CreateDate", "业务日期");
            AddColumn("CreatePerson", "制表人");
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

            foreach (MatHireStockBlockMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["Theme"].Value = obj.Theme;
                dr.Cells["StockReason"].Value = obj.StockReason;
                dr.Cells["BlockStartTime"].Value = obj.BlockStartTime.ToShortDateString();
                dr.Cells["BlockFinishTime"].Value = obj.BlockFinishTime.ToShortDateString();
                dr.Cells["HandlePersonName"].Value = obj.HandlePersonName;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                dr.Cells["RealOperationDate"].Value = obj.RealOperationDate.ToShortDateString();
                dr.Cells["Descript"].Value = obj.Descript;
                dr.Cells["SupplyRelation"].Value = obj.SupplierName;
                dr.Cells["ProjectName"].Value = obj.ProjectName;
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
