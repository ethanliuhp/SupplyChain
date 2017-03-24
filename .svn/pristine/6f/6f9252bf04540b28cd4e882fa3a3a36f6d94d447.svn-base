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
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng
{
    public partial class VExpensesRowBillSearchList : SearchList
    {
        private CExpensesRowBillMng cExpensesRowBillMng;

        public VExpensesRowBillSearchList(CExpensesRowBillMng cExpensesRowBillMng)
        {
            InitializeComponent();
            this.cExpensesRowBillMng = cExpensesRowBillMng;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (code != null)
            {
                cExpensesRowBillMng.Find(code.ToString(),ClientUtil.ToString(Id));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("HandlePerson", "划账人");
            AddColumn("IssueDate", "划账日期");
            AddColumn("TotalMoney", "划账金额");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "制单日期");
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

            foreach (ExpensesRowBill obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["HandlePerson"].Value = obj.HandlePersonName;
                dr.Cells["IssueDate"].Value = obj.IssueDate.ToShortDateString();
                dr.Cells["TotalMoney"].Value = obj.TotalMoney;
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
