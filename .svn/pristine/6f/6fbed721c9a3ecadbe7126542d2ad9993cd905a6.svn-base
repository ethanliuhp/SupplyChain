using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VLogStatReport : TBasicDataView    
    {
        MStockMng model = new MStockMng();
        public VLogStatReport()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin.Value = DateTime.Now.AddDays(-7);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();

            string condition = " and t1.OperDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.OperDate<=to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            if (this.txtProject.Text != "")
            {
                condition += " and t1.ProjectName like '%" + this.txtProject.Text + "%'";
            }
            
            MStockMng mStockIn = new MStockMng();
            try
            {
                IList logList = model.StockInSrv.GetLogStatReport(condition);
                ShowLogData(logList);
                
            } catch (Exception ex)
            {
                MessageBox.Show("查询日志出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void ShowLogData(IList logList)
        {
            if (logList == null || logList.Count <= 0) return;
            foreach (DataDomain domain in logList)
            {
                int rowIndex = dgDetail.Rows.Add();
                DataGridViewRow row = dgDetail.Rows[rowIndex];
                row.Cells[this.colProName.Name].Value = domain.Name1;
                row.Cells[this.colPeriod.Name].Value = domain.Name2;
                row.Cells[this.colProjectMoney.Name].Value = domain.Name3;
                row.Cells[this.colBillType.Name].Value = domain.Name4;
                row.Cells[this.colCount.Name].Value = domain.Name5;
            }
        }
    }
}
