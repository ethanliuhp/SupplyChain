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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport
{
    public partial class VConstructionReportSearchList : SearchList
    {
        private CConstructionReport cContructionReport;
        public VConstructionReportSearchList(CConstructionReport cContructionReport)
        {
            InitializeComponent();
            this.cContructionReport = cContructionReport;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            string code = ClientUtil.ToDateTime(dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value).ToShortDateString().Replace("-","");
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;

            cContructionReport.Find(code,ClientUtil.ToString(Id));
            
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("Id", "序列码");
            dgSearchResult.Columns["Id"].Width = 100;
            AddColumn("Code", "编号");
            AddColumn("WeatherCondition", "天气状况");
            dgSearchResult.Columns["WeatherCondition"].Width = 120;
            AddColumn("ProjectManage", "项目管理情况");
            dgSearchResult.Columns["ProjectManage"].Width = 150;
            AddColumn("CreateDate", "日期");
            AddColumn("SafetyControl", "生产安全控制情况");
            dgSearchResult.Columns["SafetyControl"].Width = 150;
            AddColumn("Problem", "存在问题");
            dgSearchResult.Columns["Problem"].Width = 120;
            AddColumn("OtherActivities", "其他活动情况");
            dgSearchResult.Columns["OtherActivities"].Width = 150;
            AddColumn("MaterialCase", "材料情况");
            dgSearchResult.Columns["MaterialCase"].Width = 120;
            AddColumn("CompletionSchedule", "工作内容及完成情况");
            dgSearchResult.Columns["CompletionSchedule"].Width = 170;
            AddColumn("ConstructSite", "施工部位");
            dgSearchResult.Columns["ConstructSite"].Width = 120;
            AddColumn("HandlePerson", "负责人");
            dgSearchResult.Columns["HandlePerson"].Width = 120;
            AddColumn("RealOperationDate", "制单日期");
            dgSearchResult.Columns["CreateDate"].Width = 120;
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
            foreach (ConstructReport obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                if (obj.WeatherGlass != null)
                {
                    dr.Cells["WeatherCondition"].Value = obj.WeatherGlass.WeatherCondition;
                }
                dr.Cells["ProjectManage"].Value = obj.ProjectManage;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["SafetyControl"].Value = obj.SafetyControl;
                dr.Cells["Problem"].Value = obj.Problem;
                dr.Cells["OtherActivities"].Value = obj.OtherActivities;
                dr.Cells["MaterialCase"].Value = obj.MaterialCase;
                dr.Cells["CompletionSchedule"].Value = obj.CompletionSchedule;
                dr.Cells["ConstructSite"].Value = obj.ConstructSite;
                dr.Cells["HandlePerson"].Value = obj.HandlePersonName;
                dr.Cells["RealOperationDate"].Value = obj.RealOperationDate.ToShortDateString();
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
