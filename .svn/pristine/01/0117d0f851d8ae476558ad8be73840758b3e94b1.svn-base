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
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.InspectionLotMng
{
    public partial class VInspectionLotSearchList : SearchList
    {
        private CInspectionLotMng cInspectionLot;

        public VInspectionLotSearchList(CInspectionLotMng cInspectionLot)
        {
            InitializeComponent();
            this.cInspectionLot = cInspectionLot;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (code != null)
            {
                cInspectionLot.Find(code.ToString(),ClientUtil.ToString(Id));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("State", "状态");
            AddColumn("ProjectTaskName","工程项目任务");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "制单日期");
            AddColumn("HandlePersonName","负责人");
            AddColumn("InsUpdateDate", "验收更新日期");
            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellContentDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        public void RefreshData(IList lst)
        {
            //从Model中取数
            dgSearchResult.Rows.Clear();

            foreach (InspectionLot obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["ProjectTaskName"].Value = obj.ProjectTaskName;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                dr.Cells["HandlePersonName"].Value = obj.HandlePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["InsUpdateDate"].Value = obj.InsUpdateDate.ToShortDateString();
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
