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
using Application.Resource.MaterialResource.Domain;


namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockQuery
{
    public partial class VStockSequenceQuery : TBasicDataView
    {
        private MStockMng model = new MStockMng();

        public VStockSequenceQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }        

        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-10);
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
            
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            } else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }

                //物资
            if (this.txtMaterial.Text != "")
            {
                condition = condition + " and t3.matname like '%" + this.txtMaterial.Text + "%'";
            }
            if (this.txtMaterial.Text != "" && this.txtMaterial.Result != null && this.txtMaterial.Result.Count != 0)
            {
                Material material = this.txtMaterial.Result[0] as Material;
                if (material.Name == this.txtMaterial.Text)
                {
                    condition = condition + " and t3.matcode='" + material.Code + "'";
                }
            }
            if (this.txtSpec.Text != "")
            {
                condition = condition + " and t3.MatSpecification like '%" + this.txtSpec.Text + "%'";
            }

            #endregion
            DataSet dataSet =model.StockRelationSrv.StockSequenceQuery(condition);
            this.dgDetail.Rows.Clear();

            DataTable dataTable = dataSet.Tables[0];
            //if (dataTable == null || dataTable.Rows.Count == 0) return;
            decimal sumQuantity=0,sumMoney=0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int rowIndex = this.dgDetail.Rows.Add();
                dgDetail[colCreateDate.Name,rowIndex].Value =string.Format(dataRow["CreateDate"].ToString(),"G");
                object billType=dataRow["BillType"];
                int stockInOutManner = int.Parse(dataRow["StockInOutManner"].ToString());
                #region 业务类型
                //业务类型
                if (billType != null)
                {
                    string billTypeStr = billType.ToString();
                    if (stockInOutManner == 10 && billTypeStr == "StockIn")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "收料入库";
                    } else if (stockInOutManner == 10 && billTypeStr == "StockInRed")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "收料入库红单";
                    } else if (stockInOutManner == 11 && billTypeStr == "StockIn")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "调拨入库";
                    } else if (stockInOutManner == 11 && billTypeStr == "StockInRed")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "调拨入库红单";
                    } else if (stockInOutManner == 20 && billTypeStr == "StockOut")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "领料出库";
                    } else if (stockInOutManner == 20 && billTypeStr == "StockOutRed")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "领料出库红单";
                    } else if (stockInOutManner == 21 && billTypeStr == "StockOut")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "调拨出库";
                    } else if (stockInOutManner == 21 && billTypeStr == "StockOutRed")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "调拨出库红单";
                    } else if (stockInOutManner == 11 && billTypeStr == "出库红单增加库存")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "调拨出库红单增加库存";
                    } else if (stockInOutManner == 10 && billTypeStr == "出库红单增加库存")
                    {
                        dgDetail[colBillType.Name, rowIndex].Value = "领料出库红单增加库存";
                    }

                }
                #endregion
                dgDetail[colOriContractNo.Name, rowIndex].Value =ClientUtil .ToString ( dataRow["SupplyOrderCode"]);
                dgDetail[colSupplyInfo.Name, rowIndex].Value = ClientUtil .ToString (dataRow["SupplierRelationName"]);

                dgDetail[colMaterialCode.Name, rowIndex].Value = dataRow["MATCODE"];
                dgDetail[colMaterialName.Name, rowIndex].Value = ClientUtil .ToString (dataRow["MATNAME"]);
                dgDetail[colSpec.Name, rowIndex].Value =ClientUtil .ToString ( dataRow["MATSPECIFICATION"]);

                object quantity=dataRow["Quantity"];
                if (quantity != null)
                {
                    sumQuantity += ClientUtil.ToDecimal(quantity);
                }else
                {
                    quantity = "0";
                }

                dgDetail[colQuantity.Name, rowIndex].Value = quantity;
                dgDetail[colUnit.Name, rowIndex].Value =ClientUtil .ToString ( dataRow["STANDUNITNAME"]);
            }
                
            this.txtSumQuantity.Text = sumQuantity.ToString("#######.####");
            //txtSumMoney.Text = sumMoney.ToString("#,###.####");
            lblRecordTotal.Text = "共【"+dataTable.Rows.Count+"】条记录";

            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            MessageBox.Show("查询完毕！");
        }
    }
}
