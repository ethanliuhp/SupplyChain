using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    public partial class VProfitInList : SearchList
    {
        //private IMProfitIn theMProfitIn = StaticMethod.GetRefModule(typeof(MProfitIn)) as IMProfitIn;
        MProfitIn theMProfitIn = new MProfitIn();
        public ProfitIn theProfitIn;
        public CProfitIn theCProfitIn;

        public VProfitInList(CProfitIn csm)
        {
            theCProfitIn = csm;

            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");
            AddColumn("SupplierRelationInfo", "供应商");
            //AddColumn("StationCategory", "仓库");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "业务日期");
            AddColumn("Remark", "备注");
            AddColumn("IsTally", "记账标志");

            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex<0)
                return;
            ProfitIn obj = dgSearchResult.Rows[e.RowIndex].Tag as ProfitIn;
            if (obj != null)
            {
                theCProfitIn.Find(obj.Code, (EnumStockExecType)Enum.Parse(typeof(EnumStockExecType), obj.Special));
            }
        }

        public void RefreshData(IList list)
        {
            dgSearchResult.Rows.Clear();

            foreach (ProfitIn sm in list)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Tag = sm;
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = sm.Id;
                dr.Cells["Code"].Value = sm.Code;
                dr.Cells["SupplierRelationInfo"].Value = sm.TheSupplierName;
                //dr.Cells["StationCategory"].Value = sm.TheStationCategory;

                if (sm.CreatePerson != null)
                    dr.Cells["CreatePerson"].Value = sm.CreatePersonName;

                dr.Cells["CreateDate"].Value = sm.CreateDate.ToShortDateString();

                dr.Cells["Remark"].Value = sm.Descript;
                dr.Cells["IsTally"].Value = sm.IsTally == 0 ? "未记账" : "已记账";
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

