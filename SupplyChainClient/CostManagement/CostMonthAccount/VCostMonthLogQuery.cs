using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VCostMonthLogQuery : TBasicDataView
    {
        private MCostMonthAccount model = new MCostMonthAccount();
        CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();

        public VCostMonthLogQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            this.txtKjn.Text = ConstObject.TheLogin.LoginDate.Year + "";
            this.txtKjy.Text = ConstObject.TheLogin.LoginDate.Month + "";
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {            
            #region 查询条件处理
            string condition = " and t1.TheProjectName ='" + projectInfo.Name + "'";

            if (this.txtKjn.Text != "")
            {
                condition = condition + " and t1.kjn = " + this.txtKjn.Text;
            }
            if (this.txtKjy.Text != "")
            {
                condition = condition + " and t1.kjy = " + this.txtKjy.Text;
            }
            if (this.txtTaskNode.Text != "")
            {
                condition = condition + " and t1.AccountTaskName like '%" + this.txtTaskNode.Text + "%'";
            }
            if (this.txtDescript.Text != "")
            {
                condition = condition + " and t1.Descripts like '%" + this.txtDescript.Text + "%'";
            }

            #endregion
            DataSet dataSet =model.CostMonthAccSrv.QuerytCostMonthAccountLog(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[this.colNumber.Name, rowIndex].Value = ClientUtil.ToString(dataRow["SerialNum"]);
                dgDetail[this.colKjn.Name, rowIndex].Value = ClientUtil.ToString(dataRow["Kjn"]);
                dgDetail[this.colKjy.Name, rowIndex].Value = ClientUtil.ToString(dataRow["Kjy"]);

                dgDetail[this.colBillType.Name, rowIndex].Value = ClientUtil.ToString(dataRow["LogType"]);
                dgDetail[this.colDescript.Name, rowIndex].Value = ClientUtil.ToString(dataRow["Descripts"]);
                dgDetail[this.colTaskNode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["AccountTaskName"]);
            }
                
            lblRecordTotal.Text = "共【"+dataTable.Rows.Count+"】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            //MessageBox.Show("查询完毕！");
        }
    }
}
