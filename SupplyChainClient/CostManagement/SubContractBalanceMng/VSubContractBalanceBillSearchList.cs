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
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VSubContractBalanceBillSearchList : SearchList
    {
        private CSubContractBalance control;

        public VSubContractBalanceBillSearchList(CSubContractBalance c)
        {
            InitializeComponent();
            this.control = c;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            SubContractBalanceBill bill = dgSearchResult.Rows[e.RowIndex].Tag as SubContractBalanceBill;
            if (bill != null)
            {
                control.Find(bill.Code, bill.Id);
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("State", "状态");

            AddColumn("BalanceRange", "结算范围");
            AddColumn("OwnerPerson", "责任人");
            AddColumn("BillingTime", "开单时间");

            AddColumn("BeginDate", "结算起始时间");
            AddColumn("EndDate", "结算结束时间");
            AddColumn("Descript", "备注");
            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgSearchResult.Columns["Id"].Visible = false;

            dgSearchResult.Columns["BeginDate"].Width = 120;
            dgSearchResult.Columns["EndDate"].Width = 120;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        public void RefreshData(IList lst)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();

            foreach (SubContractBalanceBill obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);

                dr.Cells["BalanceRange"].Value = obj.BalanceTaskName;
                dr.Cells["OwnerPerson"].Value = obj.CreatePersonName;
                dr.Cells["BillingTime"].Value = obj.CreateDate.ToShortDateString();

                dr.Cells["BeginDate"].Value = obj.BeginTime.ToShortDateString();
                dr.Cells["EndDate"].Value = obj.EndTime.ToShortDateString();
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
