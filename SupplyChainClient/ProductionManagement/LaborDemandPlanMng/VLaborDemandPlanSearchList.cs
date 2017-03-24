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
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng
{
    public partial class VLaborDemandPlanSearchList : SearchList
    {
        private CLaborDemandPlan cLaborDemandPlan;
        public VLaborDemandPlanSearchList(CLaborDemandPlan cLaborDemandPlan)
        {
            InitializeComponent();
            this.cLaborDemandPlan = cLaborDemandPlan;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (code != null)
            {
                cLaborDemandPlan.Find(code.ToString(),ClientUtil.ToString(Id));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");
            AddColumn("PlanName", "计划名称");
            AddColumn("OperOrgInfoName", "负责组织");
            AddColumn("ReportTime", "提报时间");
            AddColumn("CreatePersonName", "制单人");
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
            foreach (LaborDemandPlanMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["PlanName"].Value = obj.PlanName;
                dr.Cells["OperOrgInfoName"].Value = obj.OperOrgInfoName;
                string a = obj.ReportTime.ToString();
                string[] aArray = a.Split(' ');
                string stra = aArray[0];
                dr.Cells["ReportTime"].Value = stra;
                dr.Cells["CreatePersonName"].Value = obj.CreatePersonName;
                string b = obj.CreateDate.ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dr.Cells["CreateDate"].Value = strb;
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
