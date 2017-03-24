using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteBalanceMng.Domain;
using VirtualMachine.Component.Util;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.ConBalanceRedUI
{
    public partial class VConBalanceRedSearchList : SearchList
    {
        CConBalanceRed cConBalanceRed;

        public VConBalanceRedSearchList(CConBalanceRed cConBalanceRed)
        {
            this.cConBalanceRed = cConBalanceRed;
            InitializeComponent();
            InitDgSearch();
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");
            AddColumn("SupplyRelation", "供应商");
            AddColumn("State", "状态");
            AddColumn("SumQuantity", "总数量");
            AddColumn("SumMoney", "总金额");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "制单日期");
            AddColumn("Descript", "备注");

            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellContentDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            if (code != null)
            {
                cConBalanceRed.Find(code.ToString());
            }
        }

        public void RefreshData(IList list)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();
            foreach (ConcreteBalanceRedMaster obj in list)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                if (obj.TheSupplierRelationInfo != null)
                    dr.Cells["SupplyRelation"].Value = obj.TheSupplierRelationInfo.SupplierInfo.Name;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["SumQuantity"].Value = Math.Abs(obj.SumVolumeQuantity);
                dr.Cells["SumMoney"].Value = Math.Abs(obj.SumMoney);
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate;
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
