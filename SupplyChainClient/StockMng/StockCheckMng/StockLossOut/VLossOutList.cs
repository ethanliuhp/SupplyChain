using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut
{
    public partial class VLossOutList : SearchList
    {
        MLossOut theMIossOut = new MLossOut();
        public LossOut theLossOut;
        public CLossOut theCLossOut;

        public VLossOutList(CLossOut csm)
        {
            theCLossOut = csm;

            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");

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
            LossOut obj = dgSearchResult.Rows[e.RowIndex].Tag as LossOut;
            if (obj != null)
            {
                theCLossOut.Find(obj.Code, (EnumStockExecType)Enum.Parse(typeof(EnumStockExecType), obj.Special));
            }
        }

        public void RefreshData(IList list)
        {
            if (list != null)
            {
                dgSearchResult.Rows.Clear();

                foreach (LossOut sm in list)
                {
                    int r = dgSearchResult.Rows.Add();
                    DataGridViewRow dr = dgSearchResult.Rows[r];
                    dr.Tag = sm;
                    dr.Cells["No."].Value = r + 1;
                    dr.Cells["Id"].Value = sm.Id;
                    dr.Cells["Code"].Value = sm.Code;
                    dr.Cells["CreatePerson"].Value = sm.CreatePersonName;
                    dr.Cells["CreateDate"].Value = sm.CreateDate.ToShortDateString();
                    dr.Cells["Remark"].Value = sm.Descript;
                    dr.Cells["IsTally"].Value = sm.IsTally == 0 ? "未记账" : "己记账";
                }

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

