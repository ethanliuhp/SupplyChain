using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.ProjectPlanningMange.Domain;
using System.Collections;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage
{
    public partial class VConstructionDesignSearchList : SearchList
    {
        private CContructionDesign cContructionDesign;

        public VConstructionDesignSearchList(CContructionDesign cContructionDesign)
        {
            InitializeComponent();
            this.cContructionDesign = cContructionDesign;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object Enginner = dgSearchResult.Rows[e.RowIndex].Cells["EnginnerName"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (Enginner != null)
            {
                cContructionDesign.Find(Enginner.ToString(), ClientUtil.ToString(Id));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列号");
            AddColumn("ProjectName", "项目名称");
            AddColumn("SubmitDate", "提交日期");
            AddColumn("CreatePersonName", "责任人");
            AddColumn("EnginnerName", "文档名称");

            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.RowHeadersVisible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        public void RefreshData(IList lst)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();
            foreach (SpecialityProposal obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["ProjectName"].Value = obj.ProjectName;
                dr.Cells["SubmitDate"].Value = obj.SubmitDate.ToShortDateString();
                dr.Cells["CreatePersonName"].Value = obj.CreatePersonName;
                dr.Cells["EnginnerName"].Value = obj.EnginnerName;
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
