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
using Application.Business.Erp.SupplyChain.ImplementationPlan.Domain;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan
{
    public partial class VImplementationSearchList : SearchList
    {
        private CImplementationPlan control;

        public VImplementationSearchList(CImplementationPlan c)
        {
            InitializeComponent();
            this.control = c;
            InitDgSearch();
        }
        //双击
        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
             if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Create_Time1"].Value;
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
            AddColumn("Pro_Name1", "项目名称");
            AddColumn("Duty_Officer1", "责任人");
            AddColumn("Create_Time1", "业务日期");
            AddColumn("State_Name1", "状态");
            AddColumn("Lv_Construction1", "层数结构");
           
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

            foreach (ImplementationMaintain obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value =obj.Id;
                dr.Cells["Pro_Name1"].Value = obj.ProName;
                dr.Cells["Duty_Officer1"].Value = obj.DutyOfficer;
                dr.Cells["Create_Time1"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["State_Name1"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["Lv_Construction1"].Value = obj.FloorStructure;
                //dr.Cells["State_Name"].Value = obj;
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
