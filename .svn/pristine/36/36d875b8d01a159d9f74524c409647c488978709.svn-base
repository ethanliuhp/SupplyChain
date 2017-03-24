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
using Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng
{
    public partial class VTargetRespBookSearchList : SearchList
    {
        private CTargetRespBook cTargetRespBook;

        public VTargetRespBookSearchList(CTargetRespBook cTargetRespBook)
        {
            InitializeComponent();
            this.cTargetRespBook = cTargetRespBook;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object CreateDate = dgSearchResult.Rows[e.RowIndex].Cells["CreateDate"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (CreateDate != null)
            {
                cTargetRespBook.Find(CreateDate.ToString(), ClientUtil.ToString(Id));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列号");
            AddColumn("ProjectName", "项目名称");
            AddColumn("ProjectManagerName", "项目经理");
            AddColumn("ProjectScale", "项目规模");
            AddColumn("DocumentName", "文档名称");
            AddColumn("SignDate", "签订日期");
            AddColumn("Riskpaymentstate", "风险抵押金缴纳情况");
            AddColumn("HandlePerson", "责任人");
            AddColumn("CreateDate", "创建日期");

            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;

            dgSearchResult.Columns["Id"].Width = 100;
            dgSearchResult.Columns["ProjectName"].Width = 100;
            dgSearchResult.Columns["ProjectManagerName"].Width = 100;
            dgSearchResult.Columns["ProjectScale"].Width = 100;
            dgSearchResult.Columns["DocumentName"].Width = 100;
            dgSearchResult.Columns["SignDate"].Width = 100;
            dgSearchResult.Columns["Riskpaymentstate"].Width = 200;
            dgSearchResult.Columns["HandlePerson"].Width = 100;
            dgSearchResult.Columns["CreateDate"].Width = 100;

            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.RowHeadersVisible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }
        public void RefreshData(IList lst)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();
            foreach (TargetRespBook obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["ProjectName"].Value = obj.ProjectName;
                //dr.Cells["Code"].Value = obj.Code;
                dr.Cells["ProjectManagerName"].Value = obj.ProjectManagerName;
                dr.Cells["ProjectScale"].Value = obj.ProjectScale;
                dr.Cells["DocumentName"].Value = obj.DocumentName;
                dr.Cells["SignDate"].Value = obj.RealOperationDate.ToShortDateString();
                dr.Cells["Riskpaymentstate"].Value = obj.RiskPaymentState;
                dr.Cells["HandlePerson"].Value = obj.HandlePerson;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToString();
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
