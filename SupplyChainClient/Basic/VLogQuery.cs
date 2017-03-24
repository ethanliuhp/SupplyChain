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

namespace Application.Business.Erp.SupplyChain.Client.Basic
{
    public partial class VLogQuery : TBasicDataView    
    {
        MStockMng model = new MStockMng();
        public VLogQuery()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin.Value = DateTime.Now.AddDays(-7);

            //dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddMonths(-1);
            //dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;

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
            
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("OperDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("OperDate",dtpDateEnd.Value.Date.AddDays(1)));
            oq.AddCriterion(Expression.Like("OperPerson", "%" + this.txtOptrPerson.Text + "%"));
            oq.AddCriterion(Expression.Like("ProjectName", "%" + this.txtProject.Text + "%"));
            MStockMng mStockIn = new MStockMng();
            try
            {
                //IList logList = StaticMethod.GetLogData(oq);
                IList logList = model.StockInSrv.GetDomainByCondition(typeof(LogData),oq);
                ShowLogData(logList);
                dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                //dgDetail.EditMode = DataGridViewEditMode.EditOnKeystroke;
                MessageBox.Show("查询完成。");
            } catch (Exception ex)
            {
                MessageBox.Show("查询日志出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void ShowLogData(IList logList)
        {
            if (logList == null || logList.Count <= 0) return;
            foreach (LogData log in logList)
            {
                int rowIndex = dgDetail.Rows.Add();
                DataGridViewRow row = dgDetail.Rows[rowIndex];
                row.Cells[colBillCode.Name].Value = log.Code;
                row.Cells[colBillId.Name].Value = log.BillId;
                row.Cells[this.colBillType.Name].Value = log.BillType;
                row.Cells[colDescript.Name].Value = log.Descript;
                row.Cells[colOperation.Name].Value = log.OperType;
                row.Cells[colOperDate.Name].Value = log.OperDate;
                row.Cells[colOperPerson.Name].Value = log.OperPerson;
                row.Cells[colProName.Name].Value = log.ProjectName;
            }
        }
    }
}
