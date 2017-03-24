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
using Application.Business.Erp.SupplyChain.EngineerManage.CollectionManage.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage;


namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.CollectionManage
{
    public partial class VCollectionSearchList : SearchList
    {
        private CCollectionManage control;

        public VCollectionSearchList(CCollectionManage c)
        {
            InitializeComponent();
            this.control = c;
            InitDgSearch();
        }
        //双击
        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["LetterN"].Value + "-" + dgSearchResult.Rows[e.RowIndex].Cells["CreateDate"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (Id != null)
            {
                control.Find(code.ToString(), ClientUtil.ToString(Id));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序号");
            AddColumn("PlanName", "项目名称");
            AddColumn("DutyOfficer", "创建人");
            AddColumn("CreateDate", "业务日期");
            AddColumn("LetterN", "函件名称");
            AddColumn("Remark", "备注说明");

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

            foreach (ColletionManage obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["PlanName"].Value = obj.ProjectName;
                dr.Cells["DutyOfficer"].Value = obj.CreatePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["LetterN"].Value = obj.LettersName;
                dr.Cells["Remark"].Value = obj.Remark;
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
