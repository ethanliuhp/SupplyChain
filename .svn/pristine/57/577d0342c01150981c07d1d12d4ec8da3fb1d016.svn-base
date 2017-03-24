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
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.PenaltyDeductionManagement
{
    public partial class VPenaltyDeductionSearchList : SearchList
    {
        private CPenaltyDeductionMng cPenaltyDeductionMng;

        public VPenaltyDeductionSearchList(CPenaltyDeductionMng cPenaltyDeductionMng)
        {
            InitializeComponent();
            this.cPenaltyDeductionMng = cPenaltyDeductionMng;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (code != null)
            {
                cPenaltyDeductionMng.Find(code.ToString(),ClientUtil.ToString(Id));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("PenaltyDeductionRantName","罚款队伍");
            //AddColumn("PenaltyType", "罚款原因");
            AddColumn("State", "状态");
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
            foreach (PenaltyDeductionMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["PenaltyDeductionRantName"].Value = obj.PenaltyDeductionRantName;//罚扣队伍
                //dr.Cells["PenaltyType"].Value = obj.PenaltyDeductionReason;//罚扣原因
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                string a = obj.CreateDate.ToString();
                string[] aArray = a.Split(' ');
                string stra = aArray[0];
                dr.Cells["CreateDate"].Value = stra;
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
