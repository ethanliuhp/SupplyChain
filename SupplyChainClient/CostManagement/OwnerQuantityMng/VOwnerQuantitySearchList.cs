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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.OwnerQuantityMng
{
    public partial class VOwnerQuantitySearchList : SearchList
    {
        private COwnerQuantityMng cOwnerQuantityMng;

        public VOwnerQuantitySearchList(COwnerQuantityMng cOwnerQuantityMng)
        {
            InitializeComponent();
            this.cOwnerQuantityMng = cOwnerQuantityMng;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex < 0) return;
            OwnerQuantityMaster ownerMaster = dgSearchResult.Rows[e.RowIndex].Tag as OwnerQuantityMaster;
            if (ownerMaster != null)
            {
                cOwnerQuantityMng.Find(ownerMaster.Code,ownerMaster.Id);
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("State", "状态");
            AddColumn("SubmitSumQuantity", "报送总金额(万元)");
            dgSearchResult.Columns["SubmitSumQuantity"].Width = 150;
            AddColumn("ConfirmSumMoney", "审定总金额(万元)");
            dgSearchResult.Columns["ConfirmSumMoney"].Width = 150;
            AddColumn("QuantityType", "报量类型");
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

            foreach (OwnerQuantityMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Tag = obj;
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["SubmitSumQuantity"].Value = obj.SubmitSumQuantity/10000;
                dr.Cells["QuantityType"].Value = obj.QuantityType;
                dr.Cells["ConfirmSumMoney"].Value = obj.ConfirmSumMoney/10000;
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
