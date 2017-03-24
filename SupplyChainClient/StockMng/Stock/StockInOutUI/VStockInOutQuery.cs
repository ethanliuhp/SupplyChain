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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI
{
    public partial class VStockInOutQuery : TBasicDataView
    {
        private MStockMng model = new MStockMng();

        public VStockInOutQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode;
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            for (int i = 0; i < 13; i++)
            {
                this.cboFiscalYear.Items.Add(ConstObject.TheLogin.TheComponentPeriod.NowYear + (i - 3));
            }
            for (int i = 1; i < 13; i++)
            {
                this.cboFiscalMonth.Items.Add(i);
            }

            this.cboFiscalYear.Text = ConstObject.TheLogin.TheComponentPeriod.NowYear.ToString();
            this.cboFiscalMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();
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
            string condition = "";
            string addCondition = "";
            int fiscalYear = 0;
            int fiscalMonth = 0;
            try
            {
                fiscalYear = int.Parse(cboFiscalYear.SelectedItem.ToString());
                fiscalMonth = int.Parse(cboFiscalMonth.SelectedItem.ToString());
            } catch (Exception ex)
            {
                MessageBox.Show("请输入正确的会计期！");
                return;
            }
            FlashScreen.Show("正在查询信息......");
            condition += " AND t1.FiscalYear=" + fiscalYear + " AND t1.FiscalMonth=" + fiscalMonth;
            addCondition += " AND t1.FiscalYear*100+t1.FiscalMonth>=" + (fiscalYear * 100 + 1) + " AND t1.FiscalYear*100+t1.FiscalMonth<="+(fiscalYear * 100 + fiscalMonth);

            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t1.MatName like '%" + this.txtMaterial.Text + "%'";
                addCondition += " and t1.MatName like '%" + this.txtMaterial.Text + "%'";
            }
            if (this.txtSpec.Text != "")
            {
                condition = condition + " and t1.MatSpec like '%" + this.txtSpec.Text + "%'";
                addCondition += " and t1.MatSpec like '%" + this.txtSpec.Text + "%'";
            }
            if (txtSupply.Text != "" && txtSupply.Result.Count > 0)
            {
                string supplierId = (txtSupply.Result[0] as SupplierRelationInfo).Id;
                condition += " and t3.SupplierRelation='" + supplierId + "'";
                addCondition += " and t3.SupplierRelation='" + supplierId + "'";
            }
            if (txtContractNo.Text != "")
            {
                condition += " and t3.SupplyOrderCode like '%" + txtContractNo.Text + "%'";
                addCondition += " and t3.SupplyOrderCode like '%" + txtContractNo.Text + "%'";
            }
            if (comMngType.Text != "")
            {
                System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
                int values = ClientUtil.ToInt(li.Value);
                condition += "and t1.State= '" + values + "'";
            }
            #endregion
            try
            {
                DataSet dataSet = model.StockInOutSrv.StockInOutQuery(condition, addCondition);
                this.dgDetail.Rows.Clear();

                DataTable dataTable = dataSet.Tables[0];
                //if (dataTable == null || dataTable.Rows.Count == 0) return;
                decimal sumQuantity = 0, sumMoney = 0;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    int rowIndex = this.dgDetail.Rows.Add();
                    dgDetail[colFiscalYear.Name, rowIndex].Value = fiscalYear;
                    dgDetail[colFiscalMonth.Name, rowIndex].Value = fiscalMonth;
                    dgDetail[colMaterialCode.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MatCode"]);
                    dgDetail[colMaterialName.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MatName"]);
                    dgDetail[colSpec.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MatSpec"]);
                    dgDetail[colUnit.Name, rowIndex].Value = ClientUtil.ToString(dataRow["UnitName"]);
                    dgDetail[colLastQuantity.Name, rowIndex].Value = ClientUtil.ToString(dataRow["LastQuantity"]);
                    dgDetail[colLastMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["LastMoney"]);
                    dgDetail[colBuyInQuantity.Name, rowIndex].Value = ClientUtil.ToString(dataRow["BuyInQuantity"]);
                    dgDetail[colBuyInMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["BuyInMoney"]);
                    dgDetail[colBuyInAddQty.Name, rowIndex].Value = ClientUtil.ToString(dataRow["BuyInAddQuantity"]);
                    dgDetail[colBuyInAddMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["BuyInAddMoney"]);
                    dgDetail[colMoveInQty.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MoveInQuantity"]);
                    dgDetail[colMoveInMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MoveInMoney"]);
                    dgDetail[colMoveInAddQty.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MoveInAddQuantity"]);
                    dgDetail[colMoveInAddMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MoveInAddMoney"]);
                    dgDetail[colSaleOutQty.Name, rowIndex].Value = ClientUtil.ToString(dataRow["SaleOutQuantity"]);
                    dgDetail[colSaleOutMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["SaleOutMoney"]);
                    dgDetail[colSaleOutAddQty.Name, rowIndex].Value = ClientUtil.ToString(dataRow["SaleOutAddQuantity"]);
                    dgDetail[colSaleOutAddMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["SaleOutAddMoney"]);
                    dgDetail[colMoveOutQty.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MoveOutQuantity"]);
                    dgDetail[colMoveOutMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MoveOutMoney"]);
                    dgDetail[colMoveOutAddQty.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MoveOutAddQuantity"]);
                    dgDetail[colMoveOutAddMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["MoveOutAddMoney"]);
                    decimal profitQty = ClientUtil.ToDecimal(dataRow["ProfitInQuantity"]) - ClientUtil.ToDecimal(dataRow["LossOutQuantity"]);
                    decimal profitMoney = ClientUtil.ToDecimal(dataRow["ProfitInMoney"]) - ClientUtil.ToDecimal(dataRow["LossOutMoney"]);
                    decimal profitAddQty = ClientUtil.ToDecimal(dataRow["ProfitInAddQuantity"]) - ClientUtil.ToDecimal(dataRow["LossOutAddQuantity"]);
                    decimal profitAddMoney = ClientUtil.ToDecimal(dataRow["ProfitInAddMoney"]) - ClientUtil.ToDecimal(dataRow["LossOutAddMoney"]);
                    dgDetail[colProfitInQty.Name, rowIndex].Value = profitQty;
                    dgDetail[colProfitInMoney.Name, rowIndex].Value = profitMoney;
                    dgDetail[colProfitInAddQty.Name, rowIndex].Value = profitAddQty;
                    dgDetail[colProfitInAddMoney.Name, rowIndex].Value = profitAddMoney;
                    dgDetail[colNowQty.Name, rowIndex].Value = ClientUtil.ToString(dataRow["NowQuantity"]);
                    dgDetail[colNowMoney.Name, rowIndex].Value = ClientUtil.ToString(dataRow["NowMoney"]);
                }
                FlashScreen.Close();
                //this.txtSumQuantity.Text = sumQuantity.ToString("#######.####");
                //txtSumMoney.Text = sumMoney.ToString("#,###.####");
                lblRecordTotal.Text = "共【" + dataTable.Rows.Count + "】条记录";

                this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);

            } catch (Exception ex)
            {
                MessageBox.Show("查询出错。\n"+ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
    }
}
