using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public partial class VMaterialHireCollectionSearchList : SearchList
    {
        private CMaterialHireCollection cMaterialCollection;
        public  EnumMatHireType collectionType;
        public VMaterialHireCollectionSearchList(CMaterialHireCollection cMaterialCollection)
        {
            InitializeComponent();
            this.cMaterialCollection = cMaterialCollection;
            InitDgSearch();
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");
            AddColumn("OldContractNum", "原始合同号");
            AddColumn("BillCode", "原单据号");
            AddColumn("SupplyRelation", "出租方");
            AddColumn("ProjectName", "租赁方");
            AddColumn("State", "状态");
            AddColumn("SumQuantity", "总数量");
            AddColumn("ExtSumMoney", "附加费用总金额");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "业务日期");
            AddColumn("RealOperationDate", "制单日期");
            AddColumn("Descript", "备注");
            
            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
            dgSearchResult.Columns["ExtSumMoney"].Width = 125;
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (code != null)
            {
                cMaterialCollection.Find(code.ToString(), ClientUtil.ToString(Id),collectionType);
            }
        }

        public void RefreshData(IList list)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();
            foreach (MatHireCollectionMaster obj in list)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["OldContractNum"].Value = obj.OldContractNum;
                if (obj.TheSupplierRelationInfo != null)
                    dr.Cells["SupplyRelation"].Value = obj.SupplierName;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["SumQuantity"].Value = obj.SumQuantity;
                dr.Cells["ExtSumMoney"].Value = obj.SumExtMoney;
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["RealOperationDate"].Value = obj.RealOperationDate.ToShortDateString();
                dr.Cells["Descript"].Value = obj.Descript;
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
