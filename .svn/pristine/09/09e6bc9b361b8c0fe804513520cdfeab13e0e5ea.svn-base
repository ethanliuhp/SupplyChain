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
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CompleteSettlementBook.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook
{
    public partial class VCompleteSearchList : SearchList
    {
        private CCompleteMng cCompleteMng;

        public VCompleteSearchList(CCompleteMng cCompleteMng)
        {
            InitializeComponent();
            this.cCompleteMng = cCompleteMng;
            InitDgSearch();
            
        }

        void dgSearchResult_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex < 0) return;
            object CreateTime = dgSearchResult.Rows[e.RowIndex].Cells["CreateTime"].Value;
            object Id = dgSearchResult.Rows[e.RowIndex].Cells["Id"].Value;
            if (Id != null)
            {
                cCompleteMng.Find(CreateTime.ToString(), ClientUtil.ToString(Id));
            }
        }

        private void InitDgSearch()
        {
            dgSearchResult.Rows.Clear();
            AddColumn("No.", "序号");
            AddColumn("Id", "项目名称");
            AddColumn("txtProjectName", "项目名称");
            AddColumn("SubmitMoney", "报送总金额");
            //AddColumn("SupplyOrderCode", "采购合同号");
            AddColumn("ShendingMoney", "审定总金额");
            AddColumn("planTime","计划结算完成时间");
            AddColumn("EndTime", "实际完成结算时间");
            AddColumn("Person", "责任人");
            AddColumn("CreateTime", "业务时间");
            //AddColumn("CreateDate", "责任人");
            //AddColumn("Descript", "创建日期");
            //dgSearchResult.Columns["No"].Width = 100;
            dgSearchResult.Columns["Id"].Width = 200;
            dgSearchResult.Columns["txtProjectName"].Width = 200;
            dgSearchResult.Columns["SubmitMoney"].Width = 200;
            dgSearchResult.Columns["ShendingMoney"].Width = 200;
            dgSearchResult.Columns["planTime"].Width = 200;
            dgSearchResult.Columns["EndTime"].Width = 200;
            dgSearchResult.Columns["Person"].Width = 200;
            dgSearchResult.Columns["CreateTime"].Width = 200;


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
            foreach (CompleteInfo obj in lst)
            {
                int r = dgSearchResult.Rows.Add();
                DataGridViewRow dr = dgSearchResult.Rows[r];
                dr.Cells["No."].Value = r + 1;
                dr.Cells["Id"].Value = obj.Id;
                //dr.Cells["AccountName"] = obj.AccountName.ToString();
                dr.Cells["txtProjectName"].Value = obj.ProjectName;
                decimal SubmitMoney = obj.SubmitMoney;
                SubmitMoney = SubmitMoney / 10000;
                dr.Cells["SubmitMoney"].Value = SubmitMoney;


                decimal shendingMoney = obj.ShendingMoney;
                shendingMoney = shendingMoney / 10000;
                dr.Cells["ShendingMoney"].Value = shendingMoney;

                dr.Cells["planTime"].Value = obj.PlanTime.ToShortDateString();
                dr.Cells["EndTime"].Value = obj.EndTime.ToShortDateString();
                dr.Cells["Person"].Value = obj.HandlePersonName;
                dr.Cells["CreateTime"].Value = obj.CreateDate.ToShortDateString();

                //string strWeek = ClientUtil.ToString(obj.Week);
                //if (strWeek.Equals("1"))
                //{
                //    strWeek = "星期一";
                //}
                //if (strWeek.Equals("2"))
                //{
                //    strWeek = "星期二";
                //}
                //if (strWeek.Equals("3"))
                //{
                //    strWeek = "星期三";
                //}
                //if (strWeek.Equals("4"))
                //{
                //    strWeek = "星期四";
                //}
                //if (strWeek.Equals("5"))
                //{
                //    strWeek = "星期五";
                //}
                //if (strWeek.Equals("6"))
                //{
                //    strWeek = "星期六";
                //}
                //if (strWeek.Equals("7"))
                //{
                //    strWeek = "星期日";
                //}
                //dr.Cells["Week"].Value = strWeek;
                //dr.Cells["HandlePerson"].Value = obj.HandlePersonName;
                //dr.Cells["CreateDate"].Value = obj.CreateDate.ToShortDateString();
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
