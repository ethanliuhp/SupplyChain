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
//using Application.Business.Erp.SupplyChain.StockAccManage.OutGo1Mng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
//using Application.Business.Erp.SupplyChain.Client.StockAccManage.MonthEndPriceUI;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    public partial class VAcctLoseAndProfitList : SearchList
    {
        private IMProfitIn theMProfitIn = StaticMethod.GetRefModule(typeof(MProfitIn)) as IMProfitIn;
        public AcctLoseAndProfit theAcctLostAndProfit;
        public CAcctLoseAndProfit theCPrice;

        public VAcctLoseAndProfitList(CAcctLoseAndProfit csm)
        {
            theCPrice = csm;

            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "序列码");
            AddColumn("Code", "单号");
            AddColumn("SupplierRelationInfo", "供应商");
            AddColumn("CustomerRelationInfo", "客户");
            AddColumn("CreatePerson", "制单人");
            AddColumn("CreateDate", "业务日期");
            AddColumn("Remark", "备注");

            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            object code = dgSearchResult.Rows[e.RowIndex].Cells["Code"].Value;
            if (code != null)
            {
                theCPrice.Find(code.ToString());
            }
        }

        public void RefreshData(IList list)
        {
            dgSearchResult.Rows.Clear();

            foreach (AcctLoseAndProfit sm in list)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = sm.Id;
                dr.Cells["Code"].Value = sm.Code;

                if (sm.BusinessType == 0)
                {
                    dr.Cells["SupplierRelationInfo"].Value = sm.TheSupplierRelationInfo.SupplierInfo.Name;
                }
                else
                {
                    dr.Cells["CustomerRelationInfo"].Value = sm.TheCustomerRelationInfo.CustomerInfo.Name;
                }

                if (sm.CreatePerson != null)
                    dr.Cells["CreatePerson"].Value = sm.CreatePerson.Name;

                dr.Cells["CreateDate"].Value = sm.CreateDate.ToShortDateString();

                dr.Cells["Remark"].Value = sm.Descript;
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

