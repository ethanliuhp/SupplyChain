﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesSingleBillMng
{
    public partial class VExpensesSingleQuery : TBasicDataView
    {
        private MExpensesSingleBillMng model = new MExpensesSingleBillMng();

        public VExpensesSingleQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitEvent()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colCode.Name)
            {
                ExpensesSingleBill master = model.ExpensesSingleBillSrv.GetExpensesSingleBillByCode(dgvCell.Value.ToString());
                VExpensesSingleMng vmro = new VExpensesSingleMng();
                vmro.CurBillMaster = master;
                vmro.Preview();
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            #region 查询条件处理
            string condition = "";
            //单据号
            if (this.txtCodeBegin.Text != "")
            {
                if (this.txtCodeBegin.Text == this.txtCodeEnd.Text)
                {
                    condition = condition + "and Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
                }
                else
                {
                    condition = condition + " and Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";//精确查询
                }
            }
            //制单日期
            if (rbCreateDate.Checked)
            {
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }
            }

            //借款日期
            if (rbBorrowDate.Checked)
            {
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and ExpensesDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and ExpensesDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and ExpensesDate >=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and ExpensesDate <to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }
            }

            //报销人
            if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
            {
                condition = condition + " and ExpensesPerson='" + txtHandlePerson.Text + "'";
            }
            //制单人
            if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
            {
                condition += " and CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
            }
            #endregion
            DataSet dataSet = model.ExpensesSingleBillSrv.ExpensesSingleBillQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumMoney = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];

                object objState = dataRow["State"];
                if (objState != null)
                {
                    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                }


                object Money = dataRow["TotalMoney"];//总数量
                if (Money != null)
                {
                    sumMoney += ClientUtil.ToDecimal(Money);
                }
                string a = dataRow["ExpensesDate"].ToString();
                string[] aArray = a.Split(' ');
                string stra = aArray[0];
                dgDetail[colExpensesDate.Name, rowIndex].Value = stra;
                dgDetail[colExpensesPerson.Name, rowIndex].Value = dataRow["ExpensesPerson"];
                dgDetail[colExpensesMoney.Name, rowIndex].Value = dataRow["TotalMoney"];
                object money = dataRow["TotalMoney"];
                if (money != null)
                {
                    sumMoney += ClientUtil.ToDecimal(money);
                }
                dgDetail[colCHJKMoney.Name, rowIndex].Value = dataRow["Money"];
                string b = dataRow["CreateDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dgDetail[colCreateDate.Name, rowIndex].Value = strb;//制单日期
                dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"];
                dgDetail[colDigest.Name, rowIndex].Value = dataRow["Purpose"];
                dgDetail[colExpensesType.Name, rowIndex].Value = dataRow["ExpensesType"];
                
            }
            this.txtSumMoney.Text = sumMoney.ToString("#,###,####");
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }
    }
}