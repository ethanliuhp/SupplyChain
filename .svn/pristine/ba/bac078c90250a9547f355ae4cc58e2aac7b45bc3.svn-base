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
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement
{
    public partial class VConstructionSearchList : SearchList
    {
        private CConstruction cContruction;
        public VConstructionSearchList(CConstruction cContruction)
        {
            InitializeComponent();
            this.cContruction = cContruction;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            string code = ClientUtil.ToString(dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value).Replace("_","");
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;

            cContruction.Find(code.ToString(),ClientUtil.ToString(Id));
            
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("Id", "序列码");
            dgSearchResult.Columns["Id"].Width = 100;
            AddColumn("Code","编号");
            AddColumn("WeatherCondition", "天气状况");
            dgSearchResult.Columns["WeatherCondition"].Width = 120;
            AddColumn("Emergency", "突发事件");
            dgSearchResult.Columns["Emergency"].Width = 100;
            AddColumn("CreateDate", "日期");
            AddColumn("ProductionRecord", "生产状况");
            dgSearchResult.Columns["ProductionRecord"].Width = 100;
            AddColumn("WorkRecord", "技术安全状工作记录");
            dgSearchResult.Columns["WorkRecord"].Width = 150;
            AddColumn("ConstructSite", "施工部位");
            dgSearchResult.Columns["ConstructSite"].Width = 100;
            AddColumn("HandlePerson", "负责人");
            dgSearchResult.Columns["HandlePerson"].Width = 100;
            AddColumn("RealOperationDate", "制单日期");
            dgSearchResult.Columns["RealOperationDate"].Width = 120;
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
            foreach (ConstructionManage obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["WeatherCondition"].Value = obj.WeatherGlass.WeatherCondition;
                dr.Cells["Emergency"].Value = obj.Emergency;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["ProductionRecord"].Value = obj.ProductionRecord;
                dr.Cells["WorkRecord"].Value = obj.WorkRecord;
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
