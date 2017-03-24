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
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng
{
    public partial class VRectificationNoticeSearchList : SearchList
    {
        private CRectificationNoticeMng cRectificationNoticeMng;

        public VRectificationNoticeSearchList(CRectificationNoticeMng cRectificationNoticeMng)
        {
            InitializeComponent();
            this.cRectificationNoticeMng = cRectificationNoticeMng;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex < 0) return;
            RectificationNoticeMaster rectificationNoticeMaster = dgSearchResult.Rows[e.RowIndex].Tag as RectificationNoticeMaster;
            if (rectificationNoticeMaster != null)
            {
                cRectificationNoticeMng.Find(rectificationNoticeMaster.Code, rectificationNoticeMaster.Id);
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单据号");
            AddColumn("State", "状态");
            AddColumn("InspectionType", "检查类型");
            AddColumn("SupplierUnitName", "受检承担单位");
            AddColumn("HandlePersonName", "受检管理负责人");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "制单日期");
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
            foreach (RectificationNoticeMaster obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Tag = obj;
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                if (obj.InspectionType.Equals(0))
                {
                    dr.Cells["InspectionType"].Value = "工程日常检查";
                }
                if (obj.InspectionType.Equals(1))
                {
                    dr.Cells["InspectionType"].Value = "检验批验收检查";
                }
                if (obj.InspectionType.Equals("2"))
                {
                    dr.Cells["InspectionType"].Value = "专业检查";
                }
                //dr.Cells["InspectionType"].Value = obj.InspectionType;
                dr.Cells["SupplierUnitName"].Value = obj.SupplierUnitName;
                dr.Cells["CreatePerson"].Value = obj.CreatePersonName;
                dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
                dr.Cells["HandlePersonName"].Value = obj.HandlePersonName;
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
