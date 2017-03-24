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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekScheduleSearchList : SearchList
    {
        private CWeekSchedule control;

        public VWeekScheduleSearchList(CWeekSchedule control)
        {
            InitializeComponent();
            this.control = control;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex < 0) return;
            WeekScheduleMaster wsm = dgSearchResult.Rows[e.RowIndex].Tag as WeekScheduleMaster;
            if (wsm != null)
            {
                if (wsm.SummaryStatus == EnumSummaryStatus.汇总生成)
                {
                    //周计划汇总
                    control.Find(wsm.Code);
                }
                else
                {
                    //周计划或月计划
                    control.Find(wsm.Code, wsm.ExecScheduleType);
                }
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");

            AddColumn("PlanName", "计划名称");
            AddColumn("Code", "单号");

            AddColumn("PlanType", "计划类型");

            AddColumn("State", "状态");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "业务日期");

            AddColumn("PlannedBeginDate", "计划开始时间");
            AddColumn("PlannedEndDate", "计划结束时间");

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

            foreach (WeekScheduleMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Tag = obj;
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["PlanName"].Value = obj.PlanName;
                dr.Cells["PlanType"].Value = obj.ExecScheduleType;
                dr.Cells["PlannedBeginDate"].Value = obj.PlannedBeginDate;
                dr.Cells["PlannedEndDate"].Value = obj.PlannedEndDate;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
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
