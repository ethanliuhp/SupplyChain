using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;

using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutRedUI
{
    public partial class VStockOutRedList : SearchList
    {
        private CStockOutRed control;

        public VStockOutRedList(CStockOutRed control)
        {
            InitializeComponent();
            this.control = control;
            InitDgSearch();
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            StockOutRed obj = dgSearchResult.Rows[e.RowIndex].Tag as StockOutRed;
            if (obj != null)
            {
                control.Find(obj.Code, (EnumStockExecType)Enum.Parse(typeof(EnumStockExecType), obj.Special));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "���");
            AddColumn("Id", "������");
            AddColumn("Code", "���ݺ�");
            AddColumn("SupplyRelation", "ʹ���������");
            AddColumn("IsLimited", "�Ƿ��޶�");
            AddColumn("State", "״̬");
            AddColumn("SumQuantity", "��������");
            AddColumn("CreatePerson", "�Ƶ���");
            AddColumn("CreateDate", "ҵ������");
            AddColumn("Descript", "��ע");

            dgSearchResult.RowTemplate.Height = 18;
            dgSearchResult.ColumnHeadersHeight = 20;
            dgSearchResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgSearchResult.Columns["Id"].Visible = false;
            dgSearchResult.CellDoubleClick += new DataGridViewCellEventHandler(dgSearchResult_CellContentDoubleClick);
        }

        public void RefreshData(IList lst)
        {
            //��Model��ȡ��
            dgSearchResult.Rows.Clear();

            foreach (StockOutRed obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Tag = obj;
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                dr.Cells["Code"].Value = obj.Code;
                dr.Cells["SupplyRelation"].Value = obj.TheSupplierName;
                dr.Cells["IsLimited"].Value = (obj.IsLimited == 1) ? "��" : "��";
                dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);
                dr.Cells["SumQuantity"].Value = Math.Abs(obj.SumQuantity);
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