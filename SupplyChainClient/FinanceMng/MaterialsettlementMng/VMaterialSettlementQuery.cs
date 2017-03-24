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

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.MaterialsettlementMng
{
    public partial class VMaterialSettlementQuery : TBasicDataView
    {
        private MMaterialSettlementMng model = new MMaterialSettlementMng();

        public VMaterialSettlementQuery()
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
            txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
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
                MaterialSettlementMaster master = model.MaterialSettlementSrv.GetMaterialSettlementMasterByCode(dgvCell.Value.ToString());
                VMaterialSettlementMng vmro = new VMaterialSettlementMng();
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

            //结算日期
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

           
            //制单人
            if (!txtCreatePerson.Text.Trim().Equals("") && txtCreatePerson.Result != null && txtCreatePerson.Result.Count > 0)
            {
                condition += " and t1.CreatePerson='" + (txtCreatePerson.Result[0] as PersonInfo).Id + "'";
            }
            //供应商
            if (this.txtSupplier.Text != "")
            {
                condition = condition + "and t1.SupplierName like '%" + this.txtSupplier.Text + "%'";
            }
            #endregion
            DataSet dataSet = model.MaterialSettlementSrv.MaterialSettlementMasterQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumQuantity = 0;
            decimal sumMoney = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[colCode.Name, rowIndex].Value = dataRow["code"];
                //dgDetail[colOriContractNo.Name, rowIndex].Value = dataRow["originalContractNo"];
                //dgDetail[colSupplyInfo.Name, rowIndex].Value = dataRow["ORGNAME"];

                object objState = dataRow["State"];
                if (objState != null)
                {
                    dgDetail[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(int.Parse(objState.ToString()));
                }
                dgDetail[colSupplier.Name,rowIndex].Value = dataRow["SupplierName"];
                dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MaterialCode"];//物资编码
                dgDetail[colMaterialName.Name, rowIndex].Value = dataRow["MaterialName"];//物资名称
                dgDetail[colSpec.Name, rowIndex].Value = dataRow["MaterialSpec"]; //规格型号
                dgDetail[colUnit.Name, rowIndex].Value = dataRow["MatStandardUnitName"];//计量单位
                object quantity = dataRow["Quantity"];//总数量
                if (quantity != null)
                {
                    sumQuantity += decimal.Parse(quantity.ToString());
                }
                dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                dgDetail[colPrice.Name, rowIndex].Value = dataRow["ContainTaxPrice"];//单价
                dgDetail[colCreatePerson.Name, rowIndex].Value = dataRow["CreatePersonName"];//制单人
                string b = dataRow["CreateDate"].ToString();
                string[] bArray = b.Split(' ');
                string strb = bArray[0];
                dgDetail[colCreateDate.Name, rowIndex].Value = strb;//制单日期
                dgDetail[colSumMoney.Name, rowIndex].Value = dataRow["ContainTaxMoney"];//金额
                object money = dataRow["ContainTaxMoney"];//总数量
                if (money != null)
                {
                    sumMoney += decimal.Parse(money.ToString());
                }
            }

            this.txtSumQuantity.Text = sumQuantity.ToString("#,###.####");
            this.txtSumMoney.Text = sumMoney.ToString("#,###,####");
            lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }
    }
}
