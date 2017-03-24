using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialReturn
{
    public partial class VMaterialReturnSearchList : SearchList
    {
        private CMaterialReturn cMaterialReturn;
        private int returnType = 1;

        public VMaterialReturnSearchList(CMaterialReturn cMaterialReturn, int type)
        {
            InitializeComponent();
            this.cMaterialReturn = cMaterialReturn;
            returnType = type;
            InitSearch();
        }

        private void InitSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");
            AddColumn("OldContractNum", "原始合同号");
            AddColumn("SupplyRelation", "出租方");
            AddColumn("State", "状态");
            AddColumn("SumExitQuantity", "总数量");
            AddColumn("ExtSumMoney", "附加费用总金额");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "业务日期");
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
            object id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (code != null)
            {
                cMaterialReturn.Find(code.ToString(), id.ToString(), returnType);
            }
        }

        public void RefreshData(IList list)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();
            foreach (MaterialReturnMaster obj in list)
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
                dr.Cells["SumExitQuantity"].Value = obj.SumExitQuantity;
                dr.Cells["ExtSumMoney"].Value = obj.SumExtMoney;
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
