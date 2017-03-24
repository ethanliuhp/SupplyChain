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
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public partial class VProjectTaskAccountBillSearchList : SearchList
    {
        private CProjectTaskAccount control;

        public VProjectTaskAccountBillSearchList(CProjectTaskAccount c)
        {
            InitializeComponent();
            this.control = c;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            ProjectTaskAccountBill bill = dgSearchResult.Rows[e.RowIndex].Tag as ProjectTaskAccountBill;
            if (bill != null)
            {
                if (bill.FrontConfirmBillType == Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain.EnumConfirmBillType.计划工单)
                    control.Find(bill.Code, bill.Id);
                else
                    control.Find2(bill.Code, bill.Id);
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("State", "状态");

            AddColumn("AccountRange", "核算范围");
            AddColumn("AccountPerson", "核算员");
            AddColumn("CreateDate", "开单时间");

            AddColumn("BeginDate", "开始时间");
            AddColumn("EndDate", "结束时间");
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

            foreach (ProjectTaskAccountBill obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);

                dr.Cells["AccountRange"].Value = obj.AccountTaskName;
                dr.Cells["AccountPerson"].Value = obj.AccountPersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate;

                dr.Cells["BeginDate"].Value = obj.BeginTime;
                dr.Cells["EndDate"].Value = obj.EndTime;
                dr.Cells["Descript"].Value = obj.Remark;

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
