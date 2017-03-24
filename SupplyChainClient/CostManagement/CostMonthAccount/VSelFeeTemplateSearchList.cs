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
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VSelFeeTemplateSearchList : SearchList
    {
        private CCostMonthAccount control;
        private CCostMonthAccount_ExecType enumType = CCostMonthAccount_ExecType.SelFeeTemplate;

        public VSelFeeTemplateSearchList(CCostMonthAccount c)
        {
            InitializeComponent();
            this.control = c;
            InitDgSearch();
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序号");
            AddColumn("Code", "单据号");
            AddColumn("Name", "取费模板名称");
            AddColumn("State", "状态");
            
            AddColumn("CreatePersonName", "创建人");
            AddColumn("CreateDate", "创建日期");

            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }
        public void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            SelFeeTemplateMaster master = dgSearchResult.Rows[e.RowIndex].Tag as SelFeeTemplateMaster;
            if (!ClientUtil.isEmpty(master))
            {
                control.Find(master.Code, enumType, master.Id);
            }
        }
        public void RefreshData(IList lst)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();

            foreach (SelFeeTemplateMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Tag = obj;
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["Name"].Value = obj.Name;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                
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
