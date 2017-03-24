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
using Application.Business.Erp.SupplyChain.ConcreteManage.PumpingPoundsMng.Domain;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng
{
    public partial class VPumpingPoundSearchList : SearchList
    {
        private CPumpingPounds cPumpingPounds;

        public VPumpingPoundSearchList(CPumpingPounds cPumpingPounds)
        {
            InitializeComponent();
            this.cPumpingPounds = cPumpingPounds;
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
            AddColumn("SumDiffQuantity", "总量差");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "业务日期");
            AddColumn("Descript", "备注");

            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }
        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            object id = dgSearchResult.Rows[e.RowIndex].Cells["id"].Value;

            if (code != null)
            {
                cPumpingPounds.Find(code.ToString(),id.ToString());
            }
        }

        public void RefreshData(IList list)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();
            foreach (PumpingPoundsMaster obj in list)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                if (obj.TheSupplierRelationInfo != null)
                    dr.Cells["SupplyRelation"].Value = obj.SupplierName;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["SumQuantity"].Value = obj.SumWeight;
                dr.Cells["SumDiffQuantity"].Value = obj.SumDelta;
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
