using System;
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
using Application.Business.Erp.SupplyChain.Client.FinanceMng.IncomeSettlementMng;

namespace Application.Business.Erp.SupplyChain.Client.FinnaceMng.IncomeSettlementMng
{
    public partial class VIncomeSettlementQuery : TBasicDataView
    {
        private MIncomeSettlementMng model = new MIncomeSettlementMng();

        public VIncomeSettlementQuery()
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
                IncomeSettlementMaster master = model.IncomeSettlementSrv.GetIncomeSettlementMasterByCode(dgvCell.Value.ToString());
                VIncomeSettlementMng vmro = new VIncomeSettlementMng();
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
                    condition = condition + "and t1.Code like '%" + this.txtCodeBegin.Text + "%'";//模糊查询
                }
                else
                {
                    condition = condition + " and t1.Code between '" + this.txtCodeBegin.Text + "' and '" + this.txtCodeEnd.Text + "'";//精确查询
                }
            }
            //制单日期
            if (rbCreateDate.Checked)
            {
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }
            }
            //负责人
            if (!txtHandlePerson.Text.Trim().Equals("") && txtHandlePerson.Result != null)
            {
                condition = condition + " and t1.handleperson='" + (txtHandlePerson.Result[0] as PersonInfo).Id + "'";
            }
            //制单人
            if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
            {
                condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
            }
            //业务日期
            if (rbBorrowDate.Checked)
            {
                if (StaticMethod.IsUseSQLServer())
                {
                    condition += " and t1.RealOperationDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.RealOperationDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
                }
                else
                {
                    condition += " and t1.RealOperationDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.RealOperationDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
                }
            }
            #endregion
            DataSet dataSet = model.IncomeSettlementSrv.IncomeSettlementMasterQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumQuantity = 0;
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


                dgDetail[colIncomeProject.Name, rowIndex].Value = dataRow["IncomeProject"];
                dgDetail[colIncomeCategories.Name, rowIndex].Value = dataRow["IncomeCategories"];
                dgDetail[colMoney.Name, rowIndex].Value = dataRow["Money"];//金额
                dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"];//制单人
                string b = dataRow["CreateDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dgDetail[colCreateDate.Name, rowIndex].Value = strb;//制单日期
                object money = dataRow["Money"];//总金额
                if (money != null)
                {
                    sumMoney += decimal.Parse(money.ToString());
                }
                dgDetail[colHandlePerson.Name,rowIndex].Value = dataRow["HandlePersonName"];
                string a = dataRow["RealOperationDate"].ToString();
                string[] aArray = a.Split(' ');
                string stra = aArray[0];
                dgDetail[colYWDate.Name, rowIndex].Value = stra;
                
            }
            this.txtSumMoney.Text = sumMoney.ToString("#,###,####");
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
            
        }
    }
}
