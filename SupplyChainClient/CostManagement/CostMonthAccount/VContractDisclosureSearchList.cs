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
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.EngineerManage.MeetingManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount {
    public partial class VContractDisclosureSearchList : SearchList
    {
        private CCostMonthAccount control;
        private CCostMonthAccount_ExecType enumType;

        public VContractDisclosureSearchList(CCostMonthAccount c)
        {
            InitializeComponent();
            this.control = c;
            InitDgSearch();
        }
        //双击
        public void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DisclosureMaster master = dgSearchResult.Rows[e.RowIndex].Tag as DisclosureMaster;
            if (!ClientUtil.isEmpty(master))
            {
                control.Find(master.Code,enumType, master.Id);
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序号");
            AddColumn("Code", "单据号");
            AddColumn("State", "状态");
            AddColumn("ProjectName", "项目名称");
            AddColumn("ContractName", "合同名称");
            AddColumn("BearerOrgName", "分包单位名称");
            AddColumn("CreatePersonName", "创建人");
            AddColumn("CreateDate", "创建日期");

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

            foreach (DisclosureMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Tag = obj;
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["ProjectName"].Value = obj.ProjectName;
                dr.Cells["ContractName"].Value = obj.ContractName;
                dr.Cells["BearerOrgName"].Value = obj.BearerOrgName;
                dr.Cells["CreatePersonName"].Value = obj.CreatePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
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
